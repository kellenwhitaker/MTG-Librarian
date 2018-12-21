using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;

namespace MTG_Librarian
{
    public class DownloadSetTask : BackgroundTask
    {
        public CardSet CardSet { get; private set; }
        private Image commonIcon, uncommonIcon, rareIcon, mythicIcon;

        public DownloadSetTask(CardSet set)
        {
            CardSet = set;
            Caption = "Set: " + set.Name;
        }

        public override void Run()
        {
            RunState = RunState.Running;
            RunWorkerAsync();
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            watch.Start();
            TotalWorkUnits = 5;
            try
            {
                DownloadIcons();
                CompletedWorkUnits = 4;
                UpdateIcon();
                string json = DownloadJSON(CardSet.MTGJSONURL);
                CardSet = JsonConvert.DeserializeObject<CardSet>(json);
                foreach (var card in CardSet.Cards)
                {
                    card.SetCode = CardSet.Code;
                    card.Edition = CardSet.Name;
                }
                (CardSet.MythicRareIcon, CardSet.RareIcon, CardSet.UncommonIcon, CardSet.CommonIcon) = (mythicIcon, rareIcon, uncommonIcon, commonIcon);
                using (var context = new MyDbContext())
                {
                    context.Add(CardSet);
                    foreach (var card in CardSet.Cards)
                        context.Add(card);
                    context.SaveChanges();
                }
                RunState = RunState.Completed;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                RunState = RunState.Failed;
            }
            finally
            {
                CompletedWorkUnits = 5;
                UpdateIcon();
                watch.Stop();
            }
        }

        private static string DownloadJSON(string uri)
        {
            string json = "";
            HttpClient client = new HttpClient();
            try
            {
                json = client.GetStringAsync(uri).Result;
            }
            catch (Exception ex) { DebugOutput.WriteLine(ex.ToString()); }
            return json;
        }

        private static Image DownloadRemoteImageFile(string uri)
        {
            Image img = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = new MemoryStream())
                {
                    byte[] buffer = new byte[4096];
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
            string URL = "http://gatherer.wizards.com/Handlers/Image.ashx?type=symbol&set={0}&size=small&rarity={1}";
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
    }
}
