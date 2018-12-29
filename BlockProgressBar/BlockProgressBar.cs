using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls
{
    public partial class BlockProgressBar : UserControl
    {
        private readonly List<ProgressBlock> blocks;
        private int _progress = 0;
        private int _maxblocks = 0;
        private int _curblocks = 0;
        private Color _barcolor = Color.SlateGray;
        private Color _bordercolor = Color.DimGray;
        private Color _blankbarcolor = Color.FromArgb(225, 225, 225);

        public BlockProgressBar()
        {
            blocks = new List<ProgressBlock>();
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnResize(EventArgs e)
        {
            RecreateBlocks();
            FillBlocks();
            Refresh();
        }

        public Color BarColor
        {
            get => _barcolor;
            set
            {
                _barcolor = value;
                FillBlocks();
                Refresh();
            }
        }

        public Color BlankBarColor
        {
            get => _blankbarcolor;
            set
            {
                _blankbarcolor = value;
                FillBlocks();
                Refresh();
            }
        }

        public Color BorderColor
        {
            get => _bordercolor;
            set
            {
                _bordercolor = value;
                FillBlocks();
                Refresh();
            }
        }

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                Refresh();
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
                Refresh();
            }
        }

        public int CurrentBlocks
        {
            get => _curblocks;
            set
            {
                _curblocks = value;
                FillBlocks();
                Refresh();
            }
        }

        private void FillBlocks()
        {
            for (int i = 0; i < _maxblocks; i++)
            {
                if (i < _curblocks)
                    blocks[i].Filled = true;
                else
                    blocks[i].Filled = false;
            }
        }

        private void RecreateBlocks()
        {
            blocks.Clear();
            for (int i = 0; i < _maxblocks; i++)
            {
                var block = new ProgressBlock
                {
                    Height = Height - 1,
                    Width = Width / _maxblocks - 4
                };
                block.Left = i == 0 ? 0 : (block.Width + 4) * i;
                blocks.Add(block);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var filledBrush = new SolidBrush(_barcolor))
            using (var blankBrush = new SolidBrush(_blankbarcolor))
            using (var pen = new Pen(_bordercolor))
                foreach (var block in blocks)
                {
                    if (block.Filled)
                        e.Graphics.FillRectangle(filledBrush, block.Left, block.Top, block.Width, block.Height);
                    else
                        e.Graphics.FillRectangle(blankBrush, block.Left, block.Top, block.Width, block.Height);
                        e.Graphics.DrawRectangle(pen, block.Left, block.Top, block.Width, block.Height);
                }
        }

        private class ProgressBlock
        {
            public int Height { get; set; } = 0;
            public int Width { get; set; } = 0;
            public int Left { get; set; } = 0;
            public int Top { get; set; } = 0;
            public bool Filled { get; set; } = false;
        }
    }    
}
