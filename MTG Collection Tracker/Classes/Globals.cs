using System.Collections.Generic;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public static class Globals
    {
        public static class Collections
        {
            public static Dictionary<string, MagicCard> AllMagicCards { get; set; } = new Dictionary<string, MagicCard>();
        }

        public static class States
        {  
            public static string CardFocusedUuid { get; set; }
        }
    
        public static class Forms
        {
            public static KW.WinFormsUI.Docking.DockPanel DockPanel { get; set; }
            public static CardInfoForm CardInfoForm { get; set; }
            public static CardNavigatorForm NavigationForm { get; set; }
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
