using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//TODO: only change card image if it's the one selected
namespace MTG_Collection_Tracker
{
    public partial class CardInfoForm : DockContent
    {
        public CardInfoForm()
        {
            InitializeComponent();
            MainForm.CardImageRetrieved += cardImageRetrieved;
        }

        private void cardImageRetrieved(object sender, CardImageRetrievedEventArgs e)
        {
            pictureBox1.Image = e.CardImage;
        }
    }
}
