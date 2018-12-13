using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnhancedTextBox
{
    public partial class EnhancedTextBox : TextBox
    {
        public string Placeholder { get; set; }
        public string UserText = "";

        public EnhancedTextBox()
        {
            InitializeComponent();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (Text == Placeholder)
            {
                if (UserText != "")
                    base.OnTextChanged(e);
            }
            else if (Text == "")
            {
                if (UserText != "")
                    base.OnTextChanged(e);

                UserText = "";
            }
            else
            {
                UserText = Text;
                base.OnTextChanged(e);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (UserText == "")
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
            if (UserText == "" && Text == Placeholder)
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
                InvokeLostFocus(this, null);
        }
    }
}
