using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;
using CustomControls;
//TODO: figure out why task manager quits partway through a queue of tasks
namespace MTG_Collection_Tracker
{
    [DesignerCategory("Code")]
    public class TaskManager : BackgroundWorker
    {
        private ConcurrentDeque<BackgroundTask> _IncomingTasks;
        private ConcurrentDeque<BackgroundTask> _AllTasks;
        private List<BackgroundTask> _activeTasks;
        private List<BackgroundTask> _completedTasks;
        private TasksForm TasksForm;
        private EnhancedOLV listView;
        private Label tasksLabel;
        private BlockProgressBar progressBar;

        public TaskManager(TasksForm tasksForm, Label label, BlockProgressBar progressBar)
        {
            TasksForm = tasksForm;
            listView = tasksForm.tasksListView;
            tasksLabel = label;
            this.progressBar = progressBar;
            _IncomingTasks = new ConcurrentDeque<BackgroundTask>();
            _AllTasks = new ConcurrentDeque<BackgroundTask>();
            _completedTasks = new List<BackgroundTask>();
            _activeTasks = new List<BackgroundTask>();
            RunWorkerAsync();
        }

        public void AddTask(BackgroundTask task)
        {
            if (task != null)
            {
                if (task.ForDisplay)
                {
                    _IncomingTasks.AddFirst(task);
                    listView?.InsertObject(0, task);
                }
                else
                {
                    _IncomingTasks.Enqueue(task);
                    listView?.AddObject(task);
                }
            }
        }

        private delegate void RefreshListViewDelegate();
        private void RefreshListView()
        {
            if (listView.InvokeRequired)
                listView.BeginInvoke(new RefreshListViewDelegate(RefreshListView));
            else
                listView.Refresh();
        }

        private delegate void MoveLVObjectsDelegate(System.Collections.ICollection objects);
        private void MoveLVObjects(System.Collections.ICollection objects)
        {
            if (listView.InvokeRequired)
                listView.BeginInvoke(new MoveLVObjectsDelegate(MoveLVObjects), objects);
            else
                listView.MoveObjects(listView.GetItemCount(), objects);
        }

        private delegate void UpdateStatusBarDelegate();
        private void UpdateStatusBar()
        {
            if (tasksLabel.InvokeRequired)
                tasksLabel.BeginInvoke(new UpdateStatusBarDelegate(UpdateStatusBar));
            else
            {
                if (_activeTasks.Count > 0)
                {
                    double totalWorkUnits = _AllTasks.Count;
                    int completedUnits = _completedTasks.Count;

                    if (progressBar.MaxBlocks == 0) progressBar.MaxBlocks = 5;
                    progressBar.CurrentBlocks = (int)(completedUnits / totalWorkUnits * progressBar.MaxBlocks);
                    int percentDone = (int)(completedUnits / totalWorkUnits * 100);
                    tasksLabel.Text = string.Format("{0} / {1} tasks completed —— {2}%", _completedTasks.Count, _AllTasks.Count, percentDone);
                }
                else
                {
                    progressBar.MaxBlocks = progressBar.CurrentBlocks = 0;
                    tasksLabel.Text = "No active tasks";
                }
            }
        }

        private void ResetState()
        {
            while (_AllTasks.TryTake(out var _));
            _completedTasks.Clear();
        }

        internal event EventHandler<SetDownloadedEventArgs> SetDownloaded;
        private void OnSetDownloaded(SetDownloadedEventArgs args)
        {
            SetDownloaded?.Invoke(this, args);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                while (_IncomingTasks.TryDequeue(out BackgroundTask nextTask))
                {
                    if (nextTask.ForDisplay)
                        _AllTasks.AddFirst(nextTask);
                    else
                        _AllTasks.Enqueue(nextTask);
                }
                while (_activeTasks.Count < 10 && _AllTasks.FindInitialized().Count() > 0)
                {
                    var notStarted = _AllTasks.FindInitialized().FirstOrDefault();
                    if (notStarted != null)
                    {
                        _activeTasks.Add(notStarted);
                        notStarted.Run();
                    }
                }
                Thread.Sleep(100);
                if (watch.ElapsedMilliseconds > 500)
                {
                    var completed = _activeTasks.FindCompleted().ToArray();
                    foreach (var task in completed)
                    {
                        _activeTasks.Remove(task);
                        _completedTasks.Add(task);
                        if (_activeTasks.Count == 0 && _IncomingTasks.Count == 0)
                            ResetState();
                        if (task is DownloadSetTask downloadTask)
                            OnSetDownloaded(new SetDownloadedEventArgs { SetCode = downloadTask.CardSet.Code });
                    }
                    if (completed.Count() > 0 && listView.Objects != null)
                        MoveLVObjects(completed);

                    watch.Restart();
                    if (tasksLabel != null && progressBar != null)
                        UpdateStatusBar();
                    if (listView != null)
                        RefreshListView();
                }
            }
        }
    }
}
