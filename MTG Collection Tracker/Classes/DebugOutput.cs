using System;
using System.Diagnostics;

namespace MTG_Librarian
{
    public static class DebugOutput
    {
        static DebugOutput()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("debugoutput.txt"));
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
