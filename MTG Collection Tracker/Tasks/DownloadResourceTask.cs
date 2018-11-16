using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public class DownloadResourceTask : BackgroundTask
    {
        public string URL { get; set; }
        private byte[] downloadData;
        public override void Run()
        {
            RunState = RunState.Running;
            RunWorkerAsync();            
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                using (var inputStream = response.GetResponseStream())
                using (var outputStream = new MemoryStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                    downloadData = outputStream.ToArray();
                    e.Result = new CardResourceArgs { Data = downloadData, BasicCardArgs = TaskObject as BasicCardArgs};
                    RunState = RunState.Completed;
                }
            }            
        }
    }
}
