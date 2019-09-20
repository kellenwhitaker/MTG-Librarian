using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace MTG_Librarian
{
    public class DownloadResourceTask : BackgroundTask
    {
        #region Properties

        public string URL { get; set; }

        #endregion Properties

        #region Fields

        private byte[] downloadData;

        #endregion Fields

        #region Constructors

        public DownloadResourceTask()
        {
            TotalWorkUnits = 1;
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
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 15000;
                var response = (HttpWebResponse)request.GetResponse();
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    using (var inputStream = response.GetResponseStream())
                    using (var outputStream = new MemoryStream())
                    {
                        var buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                        downloadData = outputStream.ToArray();
                        e.Result = new CardResourceArgs { Data = downloadData, BasicCardArgs = TaskObject as BasicCardArgs };
                        RunState = RunState.Completed;
                        CompletedWorkUnits = TotalWorkUnits;
                    }
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                RunState = RunState.Failed;
            }
            finally { watch.Stop(); }
        }

        #endregion Methods
    }
}