using System;
using System.ComponentModel;
using System.Net;
using System.Drawing;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;

namespace MTG_Librarian
{
    public class DownloadSetTask : BackgroundTask
    {
        #region Properties

        public ScryfallCardSet CardSet { get; private set; }

        #endregion Properties

        #region Fields

        private Image commonIcon, uncommonIcon, rareIcon, mythicIcon;

        #endregion Fields

        #region Constructors

        public DownloadSetTask(ScryfallCardSet set)
        {
            CardSet = set;
            Caption = "Set: " + set.name;
            TotalWorkUnits = 5;
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            base.Run();
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                DownloadIcons();
                CompletedWorkUnits = 3;
                (CardSet.MythicRareIcon, CardSet.RareIcon, CardSet.UncommonIcon, CardSet.CommonIcon) = (mythicIcon, rareIcon, uncommonIcon, commonIcon);
                
                using (var context = new ScryfallCardsDbContext())
                {
                    context.Upsert(CardSet);
                    context.SaveChanges();
                }
                
                RunState = RunState.Completed;
                CompletedWorkUnits = TotalWorkUnits;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                RunState = RunState.Failed;
            }
            finally
            {
                UpdateIcon();
                watch.Stop();
            }
        }

        private static Image DownloadRemoteImageFile(string uri)
        {
            Image img = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Timeout = 15000;
                var response = (HttpWebResponse)request.GetResponse();
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    using (Stream inputStream = response.GetResponseStream())
                    using (Stream outputStream = new MemoryStream())
                    {
                        var buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                        img = Image.FromStream(outputStream);
                    }
                }        
            }
            catch (Exception ex) { DebugOutput.WriteLine(ex.ToString()); }

            return img;
        }

        private void DownloadIcons()
        {
            string gathererSetCode = Globals.Methods.ConvertScryfallSetCodeToGatherer(CardSet.code);
            const string URL = "http://gatherer-static.wizards.com/set_symbols/{0}/small-{1}-{0}.png";
            string imgURL = String.Format(URL, gathererSetCode.ToUpper(), "common");
            DebugOutput.WriteLine(imgURL);
            Icon = commonIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, gathererSetCode.ToUpper(), "uncommon");
            uncommonIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, gathererSetCode.ToUpper(), "rare");
            rareIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, gathererSetCode.ToUpper(), "mythic");
            mythicIcon = DownloadRemoteImageFile(imgURL);
        }

        private void UpdateIcon()
        {
            int percentComplete = (int)((double)CompletedWorkUnits / TotalWorkUnits * 100);
            if (percentComplete == 100)
                Icon = mythicIcon ?? rareIcon;
            else if (percentComplete > 66)
                Icon = rareIcon;
            else if (percentComplete > 33)
                Icon = uncommonIcon;
            else
                Icon = commonIcon;
        }

        #endregion Methods
    }
}