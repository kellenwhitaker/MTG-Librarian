using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EnhancedTextBox
{
    public partial class EnhancedTextBox : System.Windows.Forms.TextBox
    {
        private string _placeholder;
        public string Placeholder 
        { 
            get => _placeholder; 
            set
            {
                _placeholder = value;
                SendMessage(this.Handle, EM_SETCUEBANNER, 0, _placeholder);
            }
        }

        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public EnhancedTextBox()
        {
            InitializeComponent();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Font = new Font(Font, FontStyle.Regular);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (string.IsNullOrEmpty(Text))
                Font = new Font(Font, FontStyle.Italic);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (string.IsNullOrEmpty(Text))
                Font = new Font(Font, FontStyle.Italic);
        }
    }
}
