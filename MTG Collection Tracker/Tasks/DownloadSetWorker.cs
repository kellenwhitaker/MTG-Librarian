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
//todo: add new sets to db view

namespace MTG_Collection_Tracker
{
    public class DownloadSetTask : BackgroundTask
    {
        private List<GathererCardDocument> docs;
        private Dictionary<int, int> ids;
        private CardSet cardSet;

        public DownloadSetTask(string setName)
        {
            cardSet = new CardSet { Name = setName };
            Caption = "Set: " + setName;
            docs = new List<GathererCardDocument>();
        }

        public override void Run()
        {
            RunState = RunState.Running;
            RunWorkerAsync();
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //base.OnDoWork(e);
            watch.Start();
            string set = cardSet.Name;
            set = WebUtility.UrlEncode(set);
            bool hasNextPage = true;
            int pageNum = 0;
            ids = new Dictionary<int, int>();
            int rowNumber = 0;
            while (hasNextPage)
            {
                string URL = "http://gatherer.wizards.com/Pages/Search/Default.aspx?sort=cn+&page={0}&output=checklist&set=[\"{1}\"]";
                URL = String.Format(URL, pageNum, set);
                var web = new HtmlWeb();
                var doc = web.Load(URL);
                var nameLinks = doc.DocumentNode
                                .Descendants("a")
                                .Where(x => x.HasClass("nameLink"));
                
                foreach (var nameLink in nameLinks)
                {
                    rowNumber++;
                    string link = nameLink.Attributes["href"].Value;
                    int id = Convert.ToInt32(link.Substring(link.IndexOf("=") + 1));
                    if (!ids.Keys.Contains(id))
                    {
                        ids.Add(id, rowNumber);
                        Console.WriteLine($"{id}, {rowNumber}");
                    }
                }
                
                var pagingControls = doc.DocumentNode
                                    .Descendants("div")
                                    .Where(x => x.HasClass("pagingcontrols"))
                                    .First();
                if (pagingControls.Descendants("a").Where(x => x.InnerText.Contains("&gt;")).Count() == 0)
                    hasNextPage = false;

                pageNum++;
            }
            
            var downloads = (from KV in ids
                            select DownloadGathererCard(KV)).ToList();

            TotalWorkUnits = downloads.Count();
            DownloadCards(downloads);
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
                    catch (Exception ex) { }
                }
            }
            return img;
        }

        private void DownloadIcons()
        {
            string URL = "http://gatherer.wizards.com/Handlers/Image.ashx?type=symbol&set={0}&size=small&rarity={1}";
            string imgURL = String.Format(URL, cardSet.Code, "C");
            Icon = cardSet.CommonIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, cardSet.Code, "U");
            cardSet.UncommonIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, cardSet.Code, "R");
            cardSet.RareIcon = DownloadRemoteImageFile(imgURL);
            imgURL = String.Format(URL, cardSet.Code, "M");
            cardSet.MythicRareIcon = DownloadRemoteImageFile(imgURL);
        }

        private void UpdateIcon()
        { 
            int percentComplete = (int)((double)CompletedWorkUnits / TotalWorkUnits * 100);
            if (percentComplete == 100)
                Icon = cardSet.MythicRareIcon ?? cardSet.RareIcon;
            else if (percentComplete > 66)
                Icon = cardSet.RareIcon;
            else if (percentComplete > 33)
                Icon = cardSet.UncommonIcon;
            else
                Icon = cardSet.CommonIcon;
        }

        private async void DownloadCards(List<Task<GathererCardDocument>> downloads)
        {
            bool iconsRetrieved = false;
            while (downloads.Count() > 0)
            {
                var t = await Task.WhenAny(downloads);
                if (!t.Result.Ignore)
                {
                    cardSet.Code = t.Result.SetCode;
                    t.Result.Edition = cardSet.Name;
                    docs.Add(t.Result);
                    if (t.Result.Variations != null)
                        foreach (var variationID in t.Result.Variations)
                            if (!docs.Exists(x => x.Id == variationID) && !ids.Keys.Contains(variationID))
                            {
                                ids.Add(variationID, t.Result.RowIndex);
                                downloads.Add(DownloadGathererCard(new KeyValuePair<int, int>(variationID, t.Result.RowIndex)));
                                TotalWorkUnits = ids.Count;
                            }
                    if (cardSet.Code != "" && !iconsRetrieved)
                    {
                        DownloadIcons();
                        iconsRetrieved = true;
                    }
                }

                CompletedWorkUnits++;
                if (iconsRetrieved) UpdateIcon();
                downloads.Remove(t);
            }
            
            using (var context = new MyDbContext())
            {                
                using (var trans = context.Database.BeginTransaction())
                {
                    context.Sets.Add(cardSet);
                    context.SaveChanges();
                    foreach (var doc in docs)
                    {
                        try
                        {
                            foreach (var part in doc.MCards)
                            {
                                part.Edition = cardSet.Name;
                                var dbPart = from p in context.Catalog
                                             where p.Edition == part.Edition && p.ColNumber == part.ColNumber && p.Part == part.Part && p.MVid == part.MVid
                                             select p;

                                if (dbPart.FirstOrDefault() == null)
                                    context.Catalog.Add(part);
                            }
                            context.SaveChanges();
                        }
                        catch (InvalidOperationException ex) { if (ex.Message.Contains("same key value")) continue; throw ex; }
                        catch (DbUpdateException dex) { Console.WriteLine(dex.ToString()); if (dex.InnerException.Message.Contains("UNIQUE constraint failed")) continue; throw dex; }
                    }
                    trans.Commit();
                }
            }
            watch.Stop();
            RunState = RunState.Completed;
        }

        public static Task<GathererCardDocument> DownloadGathererCard(KeyValuePair<int, int> KV)
        {
            return Task.Run(() =>
            {
                GathererCardDocument doc = new GathererCardDocument(KV.Key, KV.Value);
                return doc;
            });
        }

        //public override void OnTaskRun(object sender, DoWorkEventArgs e)
        //{
            //throw new NotImplementedException();
        //}
    }
}
