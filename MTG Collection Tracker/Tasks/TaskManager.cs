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

namespace MTG_Collection_Tracker
{
    [DesignerCategory("Code")]
    public class TaskManager : BackgroundWorker
    {
        private ConcurrentQueue<BackgroundTask> _taskQ;
        private BlockingCollection<BackgroundTask> _tasks;
        private List<BackgroundTask> _activeTasks;
        private List<BackgroundTask> _completedTasks;
        private EnhancedOLV listView;
        private Label tasksLabel;
        private BlockProgressBar progressBar;

        public TaskManager(EnhancedOLV lv, Label label, BlockProgressBar progressBar)
        {
            listView = lv;
            tasksLabel = label;
            this.progressBar = progressBar;
            _taskQ = new ConcurrentQueue<BackgroundTask>();
            _tasks = new BlockingCollection<BackgroundTask>();
            _completedTasks = new List<BackgroundTask>();
            _activeTasks = new List<BackgroundTask>();
            RunWorkerAsync();
        }

        public void AddTask(BackgroundTask task)
        {
            if (task != null)
            {
                _taskQ.Enqueue(task);
                if (listView != null)
                    listView.AddObject(task);
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
                    double totalWorkUnits = (from t in _tasks
                                             select t.TotalWorkUnits).Sum();
                    int completedUnits = (from t in _tasks
                                          select t.CompletedWorkUnits).Sum();

                    if (progressBar.MaxBlocks == 0) progressBar.MaxBlocks = 5;
                    progressBar.CurrentBlocks = (int)(completedUnits / totalWorkUnits * progressBar.MaxBlocks);
                    int percentDone = totalWorkUnits != 0 ? (int)(completedUnits / totalWorkUnits * 100) : 0;
                    tasksLabel.Text = string.Format("{0} / {1} tasks completed —— {2}%", _completedTasks.Count, _tasks.Count, percentDone);
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
            _tasks.Clear();
            _completedTasks.Clear();
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                while (_taskQ.TryDequeue(out BackgroundTask nextTask))
                {
                    _tasks.Add(nextTask);
                }
                while (_activeTasks.Count < 10 && _tasks.FindInitialized().Count() > 0)
                {
                    var notStarted = _tasks.FindInitialized().FirstOrDefault();
                    if (notStarted != null)
                    {
                        _activeTasks.Add(notStarted);
                        notStarted.Run();
                    }
                }
                Thread.Sleep(100);
                if (watch.ElapsedMilliseconds > 1000)
                {
                    var completed = _activeTasks.FindCompleted().ToArray();
                    foreach (var task in completed)
                    {
                        _activeTasks.Remove(task);
                        _completedTasks.Add(task);
                        if (_activeTasks.Count == 0 && _taskQ.Count == 0)
                            ResetState();
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
