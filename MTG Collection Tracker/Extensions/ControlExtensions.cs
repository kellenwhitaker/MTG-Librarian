using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public static class ControlExtensions
    {
        public static void SetDoubleBuffered(this System.Windows.Forms.Control c)
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
