using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControls
{
    public partial class FlatButton: Button
    {
        private bool _checked = false;
        private bool _mouseOver = false;
        private bool _checkStateChanged = false;

        public FlatButton()
        {
            InitializeComponent();
            FlatStyle = FlatStyle.Flat;
            UseVisualStyleBackColor = false;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.BorderColor = BackColor;
        }

        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                Refresh();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            _checkStateChanged = true;
            Checked = !Checked;
            base.OnClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _mouseOver = true;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _mouseOver = false;
            _checkStateChanged = false;
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;            
            Rectangle borderRect = new Rectangle(pevent.ClipRectangle.X, pevent.ClipRectangle.Y, pevent.ClipRectangle.Width - 1, pevent.ClipRectangle.Height - 1);
            DrawCheckState();
            if (_mouseOver)
            {
                if (!Checked)
                    DrawBorder();
                PunchOutSides();
            }
            /*
            if (_mouseOver)
            {
                g.FillRectangle(new SolidBrush(BackColor), pevent.ClipRectangle);
                if (!_checkStateChanged)
                {
                    DrawBorder();
                    PunchOutSides();
                }
                else
                {
                    DrawCheckState();
                }
            }
            else
            {
                DrawCheckState();
            }
            */
            if (ImageList != null && ImageKey != null)
            {
                if (Checked)
                    g.DrawImage(ImageList.Images[$"{ImageKey}C"], new Point(4, 4));
                else
                    g.DrawImage(ImageList.Images[ImageKey], new Point(4, 4));
            }

            // Inner methods
            void DrawCheckState()
            {
                if (Checked)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 170, 224, 250)), pevent.ClipRectangle);
                    DrawBorder();
                }
                else
                    g.FillRectangle(new SolidBrush(BackColor), pevent.ClipRectangle);
            }

            void DrawBorder()
            {
                using (Pen p = new Pen(Brushes.DodgerBlue, 1))
                    g.DrawRectangle(p, borderRect);
            }

            void PunchOutSides()
            {
                using (Pen l = new Pen(new SolidBrush(BackColor), 1))
                {
                    g.DrawLine(l, new Point(borderRect.X, borderRect.Y + 4), new Point(borderRect.X, borderRect.Y + borderRect.Height - 4));
                    g.DrawLine(l, new Point(borderRect.X + borderRect.Width, borderRect.Y + 4), new Point(borderRect.X + borderRect.Width, borderRect.Y + borderRect.Height - 4));
                    g.DrawLine(l, new Point(borderRect.X + 4, borderRect.Y), new Point(borderRect.X + borderRect.Width - 4, borderRect.Y));
                    g.DrawLine(l, new Point(borderRect.X + 4, borderRect.Y + borderRect.Height), new Point(borderRect.X + borderRect.Width - 4, borderRect.Y + borderRect.Height));
                }
            }
        }
    }
}
