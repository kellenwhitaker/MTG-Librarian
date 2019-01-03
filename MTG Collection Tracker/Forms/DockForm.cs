using KW.WinFormsUI.Docking;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class DockForm : DockContent
    {
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            base.OnFormClosing(e);
        }
    }
}