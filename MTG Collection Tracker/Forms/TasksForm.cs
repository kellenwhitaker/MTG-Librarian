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
using CustomControls;

namespace MTG_Librarian
{
    public partial class TasksForm : DockContent
    {
        public TaskManager TaskManager { get; private set; }

        public TasksForm(Label tasksLabel, BlockProgressBar progressBar)
        {
            InitializeComponent();
            TaskManager = new TaskManager(this, tasksLabel, progressBar);
            tasksListView.BackColor = Color.Transparent;
        }
    }
}
