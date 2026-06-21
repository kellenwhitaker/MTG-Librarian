using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetterPanel
{
    public partial class BetterPanel: Panel
    {
        public BetterPanel()
        {
        }
        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            // Force the panel to keep its current display position instead of jumping
            return this.DisplayRectangle.Location;
        }
    }
}
