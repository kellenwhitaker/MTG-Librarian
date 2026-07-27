using BrightIdeasSoftware;
using KW.WinFormsUI.Docking;
using System;
using System.Net.Http;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class TasksForm : DockForm
    {
        public TaskManager TaskManager { get; private set; }

        public TasksForm()
        {
            InitializeComponent();
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
            tasksListView.CellToolTip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tasksListView.CellToolTipGetter = delegate (OLVColumn column, Object model) 
            {
                var task = model as BackgroundTask;
                return $"{task.Caption}\n{task.CompletedWorkUnits} / {task.TotalWorkUnits} units completed\n{task.Runtime / 1000} s";
            };
        }

        public void InitializeTaskManager()
        {
            TaskManager = new TaskManager(Globals.Forms.MainForm.TasksLabel, Globals.Forms.MainForm.TasksProgressBar);
        }
    }
}
