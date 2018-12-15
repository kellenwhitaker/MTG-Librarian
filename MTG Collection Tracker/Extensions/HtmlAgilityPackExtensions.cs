using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public static class HtmlAgilityPackExtensions
    {
        public static string lowercase(string attribute) => $"translate({attribute}, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')";
        public static string CITextContains(string searchText) => $"text()[contains({lowercase(".")}, '{searchText.ToLower()}')]";

        public static HtmlDocument FromURL(string URL, int Timeout = 15000, int MaxTries = 3)
        {
            var web = new HtmlWeb { PreRequest = x => { x.Timeout = Timeout; return true; } };
            HtmlDocument doc = null;            
            for (int timesTried = 0; doc == null && timesTried <= MaxTries; timesTried++)
            {
                try { doc = web.Load(URL); }
                catch (System.Net.WebException ex) { if (ex.HResult == -2146233079) continue; throw ex; };
            }
            return doc;
        }
    }
}
