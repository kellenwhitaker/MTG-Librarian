using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public static class ControlExtensions
    {
        public static int GetChildIndex(this Control c)
        {
            return c.Parent.Controls.GetChildIndex(c);
        }

        public static void SetDoubleBuffered(this Control c)
        {
            try
            {
                System.Reflection.PropertyInfo aProp =
                      typeof(System.Windows.Forms.Control).GetProperty(
                            "DoubleBuffered",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance);

                aProp.SetValue(c, true, null);
            }
            catch (Exception ex) { }
        }
    }
}
