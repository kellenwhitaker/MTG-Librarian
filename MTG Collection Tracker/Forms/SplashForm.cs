using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class SplashForm : Form
    {
        readonly Image srcImage;
        Double currentOpacity = 0.3;
        public SplashForm()
        {
            InitializeComponent();
            srcImage = (Image)pictureBox1.Image.Clone();
        }

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            var bmp = new Bitmap(img.Width, img.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                var colormatrix = new ColorMatrix { Matrix33 = opacityvalue };
                var imgAttribute = new ImageAttributes();
                imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            }
            return bmp;
        }

        public void ProgressChanged(SplashProgressObject progress)
        {
            statusLabel.Text = progress.ProgressText;
            blockProgressBar2.CurrentBlocks = progress.Ticks;
            if (progress.Ticks == blockProgressBar2.MaxBlocks)
            {
                blockProgressBar2.BarColor = bracketLabel.ForeColor = statusLabel.ForeColor = Color.Black;
                timer2.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentOpacity += 0.05;
            if (currentOpacity >= 1)
                timer1.Enabled = false;
            pictureBox1.Image = ChangeOpacity(srcImage, (float)currentOpacity);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var p = new Pen(Brushes.DimGray) { Width = 4 };
            e.Graphics.DrawRectangle(p, e.ClipRectangle);
        }
    }

    public class SplashProgressObject
    {
        public string ProgressText;
        public int Ticks;

        public SplashProgressObject(string text, int ticks)
        {
            ProgressText = text;
            Ticks = ticks;
        }
    }
}
