using System;
using System.ComponentModel;
using System.Net;
using System.Drawing;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO.Compression;

namespace MTG_Librarian
{
    public class DownloadSetTask : BackgroundTask
    {
        #region Properties

        public CardSet CardSet { get; private set; }

        #endregion Properties

        #region Fields

        private Image commonIcon, uncommonIcon, rareIcon, mythicIcon;

        #endregion Fields

        #region Constructors

        public DownloadSetTask(CardSet set)
        {
            CardSet = set;
            Caption = "Set: " + set.ScrapedName;
            TotalWorkUnits = 5;
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            base.Run();
        }

        private static byte[] Unzip(byte[] zipped)
        {
            using (var ms = new MemoryStream(zipped))
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Read))
            using (var entryStream = archive.Entries[0].Open())
            using (var outputStream = new MemoryStream())
            {
                entryStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                DownloadIcons();
                CompletedWorkUnits = 4;
                UpdateIcon();
                string scrapedName = CardSet.ScrapedName;
                string mtgjsonUrl = CardSet.MTGJSONURL;
                var zipped = DownloadZIP(CardSet.MTGJSONURL);
                var unzipped = Unzip(zipped);
                string json = System.Text.Encoding.UTF8.GetString(unzipped);
                CardSet = JsonConvert.DeserializeObject<CardSet>(json);
                if (CardSet == null) throw new InvalidDataException("Invalid JSON encountered");
                CardSet.ScrapedName = scrapedName;
                CardSet.MTGJSONURL = mtgjsonUrl;
                foreach (var card in CardSet.Cards)
                {
                    card.SetCode = CardSet.Code;
                    card.Edition = CardSet.Name;
                }
                (CardSet.MythicRareIcon, CardSet.RareIcon, CardSet.UncommonIcon, CardSet.CommonIcon) = (mythicIcon, rareIcon, uncommonIcon, commonIcon);
                using (var context = new MyDbContext())
                {
                    context.Upsert(CardSet);
                    foreach (var card in CardSet.Cards)
                    {
                        if (card.type.Contains("Basic Land")) // workaround needed because mtgjson thinks basic land is not a separate rarity from common
                            card.rarity = "basic land";
                        context.Upsert(card);
                    }

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

        private static byte[] DownloadZIP(string uri)
        {
            byte[] zip;
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 15);
                var httpResponseMessage = client.GetAsync(uri).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                    zip = httpResponseMessage.Content.ReadAsByteArrayAsync().Result;
                else
                    throw new HttpRequestException($"{(int)httpResponseMessage.StatusCode}: {httpResponseMessage.StatusCode.ToString()}");
                return zip;
            }
        }

        private static Image DownloadRemoteImageFile(string uri)
        {
            Image img = null;
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
                    try
                    {
                        img = Image.FromStream(outputStream);
                    }
                    catch (Exception ex) { DebugOutput.WriteLine(ex.ToString()); }
                }
            }
            return img;
        }

        private void DownloadIcons()
        {
            const string URL = "http://gatherer.wizards.com/Handlers/Image.ashx?type=symbol&set={0}&size=small&rarity={1}";
            string imgURL = String.Format(URL, CardSet.Code, "C");
            Icon = commonIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, CardSet.Code, "U");
            uncommonIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, CardSet.Code, "R");
            rareIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, CardSet.Code, "M");
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