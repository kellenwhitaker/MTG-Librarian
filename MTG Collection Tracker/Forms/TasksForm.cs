using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public partial class TasksForm : DockContent
    {
        public TaskManager TaskManager { get; private set; }

        public TasksForm()
        {
            InitializeComponent();
        }

        public void InitializeTaskManager()
        {
            TaskManager = new TaskManager(Globals.Forms.MainForm.TasksLabel, Globals.Forms.MainForm.TasksProgressBar);
        }
    }
}
