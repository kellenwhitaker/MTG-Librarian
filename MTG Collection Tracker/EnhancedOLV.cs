using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Collection_Tracker
{
    public class EnhancedOLV : ObjectListView
    {
        public EnhancedOLV() : base()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Console.WriteLine(BackColor.Name);
            if (BackColor == Color.Transparent) return;
            base.OnPaintBackground(pevent);
        }
    }
}
