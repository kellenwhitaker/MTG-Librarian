using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.TabControl;

// <summary>
// A custom TabControl that is compatible with FlatButtons.
// FlatButtons do not render correctly when placed directly on a TabPage
// </summary>
namespace CompatibleTabControl
{
    public class CompatibleTabControl : Panel
    {
        private TabControl tabControl;
        private SplitContainer splitContainer;

        public TabPageCollection TabPages => tabControl.TabPages;
        private List<Panel> tabPanels = new List<Panel>();
        
        public CompatibleTabControl()
        {
            InitializeComponent();
            this.Controls.Add(splitContainer);
            this.splitContainer.Panel1.Controls.Add(tabControl);
        }
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.splitContainer = new SplitContainer();
            this.SuspendLayout();
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Horizontal;
            this.splitContainer.Panel1MinSize = 6;
            this.splitContainer.SplitterDistance = 6;
            this.splitContainer.IsSplitterFixed = true;
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(200, 25);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabControl_ControlAdded);
            this.tabControl.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.tabControl_ControlRemoved);
            this.ResumeLayout(false);

        }

        private void tabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                Tag = e.Control
            };
            if (e.Control.Controls.Count > 0)
                panel.Controls.Add(e.Control.Controls[0]);
            this.splitContainer.Panel2.Controls.Add(panel);
            tabPanels.Add(panel);
            if (tabControl.SelectedIndex == 0)
                panel.Visible = true;
        }

        private void tabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            var panel = tabPanels.FirstOrDefault(p => p.Tag == e.Control);
            this.splitContainer.Panel2.Controls.Remove(panel);
            tabPanels.Remove(panel);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int index = 0; index < tabPanels.Count; index++)
            {
                if (index == tabControl.SelectedIndex)
                    tabPanels[index].Visible = true;
                else
                    tabPanels[index].Visible = false;
            }
        }
    }
}
