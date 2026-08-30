using System;
using System.Diagnostics;
using System.IO;

namespace MTG_Librarian
{
    public static class DebugOutput
    {
        static DebugOutput()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string appFolder = Path.Combine(appDataPath, "MTG Librarian");
            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            string logFilePath = Path.Combine(appFolder, "debugoutput.txt");
            Trace.Listeners.Add(new TextWriterTraceListener(logFilePath));
        }

        public static void WriteLine(string s)
        {
            Trace.WriteLine(DateTime.Now);
            Trace.WriteLine(s);
            Trace.WriteLine("\n");
            Trace.Flush();
        }
    }
}
