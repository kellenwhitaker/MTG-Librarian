using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public static class Globals
    {
        public static Dictionary<string, MagicCard> AllCards = new Dictionary<string, MagicCard>();
        public static WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel { get; set; }
    }
}
