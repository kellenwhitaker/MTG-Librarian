using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public static class DockPanelSuiteExtensions
    {
        public static DockStyle ToDockStyle(this DockState dockState)
        {
            switch (dockState)
            {
                case DockState.DockBottom:
                case DockState.DockBottomAutoHide: return DockStyle.Bottom;
                case DockState.DockLeft:
                case DockState.DockLeftAutoHide: return DockStyle.Left;
                case DockState.DockRight:
                case DockState.DockRightAutoHide: return DockStyle.Right;
                case DockState.DockTop:
                case DockState.DockTopAutoHide: return DockStyle.Top;
                default: return DockStyle.Fill;
            }
        }
    }
}
