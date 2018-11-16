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
    public partial class BlockProgressBar : UserControl
    {
        List<ProgressPanel> blocks;
        private int _progress = 0;
        private int _maxblocks = 0;
        private int _curblocks = 0;
        private Color _barcolor = Color.SlateGray;
        private Color _bordercolor = Color.DimGray;
        private Color _blankbarcolor = Color.FromArgb(225, 225, 225);

        public BlockProgressBar()
        {
            blocks = new List<ProgressPanel>();
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            RecreateBlocks();
            FillBlocks();
        }

        public Color BarColor
        {
            get => _barcolor;
            set
            {
                _barcolor = value;
                FillBlocks();
            }
        }

        public Color BlankBarColor
        {
            get => _blankbarcolor;
            set
            {
                _blankbarcolor = value;
                FillBlocks();
            }
        }

        public Color BorderColor
        {
            get => _bordercolor;
            set
            {
                _bordercolor = value;
                FillBlocks();
            }
        }

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;                
            }
        }

        public int MaxBlocks
        {
            get => _maxblocks;
            set
            {
                _maxblocks = value;
                RecreateBlocks();
                FillBlocks();
            }
        }

        public int CurrentBlocks
        {
            get => _curblocks;
            set
            {
                _curblocks = value;
                FillBlocks();
            }
        }

        private void FillBlocks()
        {
            for (int i = 0; i < _maxblocks; i++)
            {
                blocks[i].ForeColor = _bordercolor;
                if (i < _curblocks)
                    blocks[i].BackColor = _barcolor;
                else
                    blocks[i].BackColor = _blankbarcolor;
            }
        }

        private void RecreateBlocks()
        {
            blocks.Clear();
            Controls.Clear();
            for (int i = 0; i < _maxblocks; i++)
            {
                ProgressPanel block = new ProgressPanel();
                block.BorderStyle = BorderStyle.None;
                block.ForeColor = _bordercolor;
                block.Height = Height - 1;
                block.Width = Width / _maxblocks - 4;
                block.Left = i == 0 ? 0 : (block.Width + 4) * i;
                blocks.Add(block);
                Controls.Add(block);
            }
        }

        private class ProgressPanel : Panel
        {
            public ProgressPanel()
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                e.Graphics.DrawRectangle(new Pen(ForeColor), 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            }
        }
    }    
}
