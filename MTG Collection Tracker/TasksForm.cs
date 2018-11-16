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

namespace MTG_Collection_Tracker
{
    public partial class TasksForm : DockContent
    {
        protected internal TaskManager taskManager;
        public TasksForm(Label tasksLabel, BlockProgressBar progressBar)
        {
            InitializeComponent();
            taskManager = new TaskManager(tasksListView, tasksLabel, progressBar);
            tasksListView.BackColor = Color.Transparent;
        }
    }
}
