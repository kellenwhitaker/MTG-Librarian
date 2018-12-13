using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//TODO: clean up EnhancedTextBox
namespace EnhancedTextBox
{
    public partial class EnhancedTextBox : TextBox
    {
        public string Placeholder { get; set; }
        private bool NoUserText = true;
        public string UserText = "";

        public EnhancedTextBox()
        {
            InitializeComponent();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (Text == Placeholder)
            {
                if (!NoUserText)
                    base.OnTextChanged(e);
            }
            else if (Text == "")
            {
                if (!NoUserText)
                {
                    base.OnTextChanged(e);
                    NoUserText = true;
                }
                UserText = "";
            }
            else
            {
                base.OnTextChanged(e);
                NoUserText = false;
                UserText = Text;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (NoUserText)
            {
                Text = Placeholder;
                Font = new Font(Font, FontStyle.Italic);
                ForeColor = Color.FromArgb(64, 128, 128, 128);
                Refresh();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (NoUserText && Text == Placeholder)
            {
                Text = "";
                Font = Parent.Font;
                ForeColor = Parent.ForeColor;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Text == "")
            {
                NoUserText = true;
                InvokeLostFocus(this, null);
            }
        }
    }
}
