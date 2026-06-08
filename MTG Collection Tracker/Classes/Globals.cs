using System.Collections.Generic;
using System.Web.Configuration;
using System.Windows.Forms;
// TODO: remove MagicCardCache
namespace MTG_Librarian
{
    public static class Globals
    {
        public static class Methods
        {
            public static string ConvertScryfallSetCodeToGatherer(string code)
            {
                switch (code)
                {
                    case "lea": return "1e";
                    case "leb": return "2e";
                    case "2ed": return "2u";
                    case "arn": return "an";
                    case "atq": return "aq";
                    case "3ed": return "3e";
                    case "leg": return "le";
                    case "drk": return "dk";
                    case "fem": return "fe";
                    case "4ed": return "4e";
                    case "ice": return "ia";
                    case "chr": return "ch";
                    case "hml": return "hm";
                    case "all": return "al";
                    case "mir": return "mi";
                    case "vis": return "vi";
                    case "5ed": return "5e";
                    case "por": return "po";
                    case "wth": return "wl";
                    case "tmp": return "te";
                    case "sth": return "st";
                    case "p02": return "p2";
                    case "exo": return "ex";
                    case "ugl": return "ug";
                    case "usg": return "uz";
                    case "ulg": return "gu";
                    case "6ed": return "6e";
                    case "ptk": return "pk";
                    case "uds": return "cg";
                    case "s99": return "p3";
                    case "mmq": return "mm";
                    case "brb": return "br";
                    case "nem": return "ne";
                    case "pcy": return "pr";
                    case "s00": return "p4";
                    case "inv": return "in";
                    case "btd": return "bd";
                    case "pls": return "ps";
                    case "7ed": return "7e";
                    case "apc": return "ap";
                    case "ody": return "od";
                    default: return code;
                }
            }
        }
        public static class Collections
        {
            public static Dictionary<string, ScryfallMagicCard> MagicCardCache { get; set; } = new Dictionary<string, ScryfallMagicCard>();
        }

        public static class States
        {
            public static string CardFocusedUuid { get; set; }
        }

        public static class Forms
        {
            public static KW.WinFormsUI.Docking.DockPanel DockPanel { get; set; }
            public static CardInfoForm CardInfoForm { get; set; }
            public static CollectionNavigatorForm NavigationForm { get; set; }
            public static DBViewForm DBViewForm { get; set; }
            public static TasksForm TasksForm { get; set; }
            public static MainForm MainForm { get; set; }
            public static List<CollectionViewForm> OpenCollectionForms { get; set; } = new List<CollectionViewForm>();
        }

        public static class ImageLists
        {
            public static ImageList ManaIcons { get; set; }
            public static ImageList SymbolIcons16 { get; set; }
            public static ImageList SmallIconList { get; set; }
        }
    }
}