using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MTG_Librarian
{
    static class Program
    {
        private static Mutex mutex = new Mutex(true, @"Global\53a1cf84-5079-4de4-9d50-5c9b55969123");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                // Another instance is already running
                MessageBox.Show("An instance of MTG Librarian is already running. This instance will now close.", "Instance Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string[] args = Environment.GetCommandLineArgs();

                if (args.Length > 1)
                {
                    string firstUserArgument = args[1];
                    foreach (var arg in args)
                    {
                        if (arg == "-prices")
                        {
                            Globals.States.UpdatePricesMode = true;
                            using (var context = new ScryfallCardsDbContext())
                            {
                                var catalogMatch = context.Metadata.FirstOrDefault(m => m.Name == "LastCatalogUpdate");

                                var lastUpdate = DateTime.MinValue;
                                if (catalogMatch != null && catalogMatch.Value != "")
                                    DateTime.TryParse(catalogMatch.Value, out lastUpdate);
                                if (!(catalogMatch == null || (DateTime.Now.Date - lastUpdate.Date).Days > 0))
                                    return;
                            }
                        }
                    }
                }

                SQLitePCL.Batteries_V2.Init();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainForm form = new MainForm();
                Application.Run();
            }
            finally
            {
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
        }
    }
}
