using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using CustomControls;

namespace MTG_Librarian
{
    [DesignerCategory("Code")]
    public class TaskManager : BackgroundWorker
    {
        #region Fields

        private readonly ConcurrentDeque<BackgroundTask> incomingTasks;
        private readonly ConcurrentDeque<BackgroundTask> allTasks;
        private readonly List<BackgroundTask> activeTasks;
        private readonly List<BackgroundTask> completedTasks;
        private readonly List<BackgroundTask> waitingTasks;
        private readonly BrightIdeasSoftware.ObjectListView listView;
        private readonly Label tasksLabel;
        private readonly BlockProgressBar progressBar;

        #endregion Fields

        #region Properties

        public int TaskCount => allTasks.Count;

        #endregion Properties

        #region Constructors

        public TaskManager(Label label, BlockProgressBar progressBar)
        {
            listView = Globals.Forms.TasksForm.tasksListView;
            tasksLabel = label;
            this.progressBar = progressBar;
            incomingTasks = new ConcurrentDeque<BackgroundTask>();
            allTasks = new ConcurrentDeque<BackgroundTask>();
            completedTasks = new List<BackgroundTask>();
            activeTasks = new List<BackgroundTask>();
            waitingTasks = new List<BackgroundTask>();
            RunWorkerAsync();
        }

        #endregion Constructors

        #region Methods
        public void ContinueWaitingTask()
        {
            if (waitingTasks.Count > 0)
            {
                var waiting = waitingTasks[0];
                waitingTasks.Remove(waiting);
                activeTasks.Add(waiting);
                waiting.Run();
            }
        }
        public void AddTask(BackgroundTask task)
        {
            if (task != null)
            {
                if (task is ScryfallSearchTask)
                    ClearSearchTasks();
                if (task.AddFirst)
                {
                    incomingTasks.AddFirst(task);
                    listView?.InsertObject(0, task);
                }
                else
                {
                    incomingTasks.Enqueue(task);
                    listView?.AddObject(task);
                }
            }
        }
        // TODO: background workers should be canceled
        private void ClearSearchTasks()
        {
            activeTasks.RemoveAll(x => x is ScryfallSearchTask);
            waitingTasks.RemoveAll(x => x is ScryfallSearchTask);
        }

        public void AddTasks(List<BackgroundTask> tasks)
        {
            if (tasks != null)
            {
                foreach (BackgroundTask task in tasks)
                {
                    if (task.AddFirst)
                        incomingTasks.AddFirst(task);
                    else
                        incomingTasks.Enqueue(task);
                }
                listView?.AddObjects(tasks);
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
            if (progressBar.InvokeRequired)
                progressBar.BeginInvoke(new UpdateStatusBarDelegate(UpdateStatusBar));
            else
            {
                if (activeTasks.Count > 0)
                {
                    double totalWorkUnits = allTasks.Count;
                    int completedUnits = completedTasks.Count;

                    if (progressBar.MaxBlocks == 0) progressBar.MaxBlocks = 5;
                    progressBar.CurrentBlocks = (int)(completedUnits / totalWorkUnits * progressBar.MaxBlocks);
                    int percentDone = (int)(completedUnits / totalWorkUnits * 100);
                    tasksLabel.Text = $"{completedTasks.Count} / {allTasks.Count} tasks completed —— {percentDone}%";
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
            while (allTasks.TryTake(out var _)) ;
            completedTasks.Clear();
        }

        private void PullIncomingTasks()
        {
            while (incomingTasks.TryDequeue(out BackgroundTask nextTask))
            {
                if (nextTask.AddFirst)
                    allTasks.AddFirst(nextTask);
                else
                    allTasks.Enqueue(nextTask);
            }
        }

        private void FillActiveTasks()
        {
            while (activeTasks.Count < 10 && allTasks.FindInitialized().Count() > 0)
            {
                var notStarted = allTasks.FindInitialized().FirstOrDefault();
                if (notStarted != null)
                {
                    if (notStarted is DownloadSetTask)
                        Thread.Sleep(100);
                    activeTasks.Add(notStarted);
                    notStarted.Run();
                }
            }
        }
        private void CheckWaitingTasks()
        {
            var waiting = activeTasks.FindWaiting().FirstOrDefault();
            if (waiting != null)
            {
                activeTasks.Remove(waiting);
                waitingTasks.Add(waiting);
                if (waiting is ScryfallSearchTask searchTask)
                {
                    OnScryfallSearchEnded(new ScryfallSearchEndedEventArgs { Results = searchTask.Results, Waiting = true });
                }
            }
        }
        private void CheckEndedTasks()
        {
            var completedOrFailed = activeTasks.FindCompletedOrFailed().ToArray();
            foreach (var task in completedOrFailed)
            {
                activeTasks.Remove(task);
                completedTasks.Add(task);
                if (task is DownloadSetTask downloadTask && task.RunState != RunState.Failed)
                    OnSetDownloaded(new SetDownloadedEventArgs { SetCode = downloadTask.CardSet.code });
                else if (task is UpdateCardsTask updateCardsTask && task.RunState != RunState.Failed)
                    OnCardsUpdatedFromScryfall(new CardsUpdatedFromScryfallEventArgs { Cards = updateCardsTask.CardsUpdated });
                else if (task is ScryfallSearchTask searchTask && task.RunState != RunState.Failed)
                    OnScryfallSearchEnded(new ScryfallSearchEndedEventArgs { Results = searchTask.Results });
            }
            if (completedOrFailed.Count() > 0)
                MoveLVObjects(completedOrFailed);
        }

        private void UpdateGUI()
        {
            if (tasksLabel != null && progressBar != null)
                UpdateStatusBar();
            if (listView != null)
                RefreshListView();
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            while (true)
            {
                PullIncomingTasks();
                FillActiveTasks();
                Thread.Sleep(100);
                CheckEndedTasks();
                CheckWaitingTasks();
                UpdateGUI();
            }
        }

        #endregion Methods

        #region Events

        public event EventHandler<CardsUpdatedFromScryfallEventArgs> CardsUpdatedFromScryfall;
        private void OnCardsUpdatedFromScryfall(CardsUpdatedFromScryfallEventArgs args)
        {
            CardsUpdatedFromScryfall.Invoke(this, args);
        }

        public event EventHandler<SetDownloadedEventArgs> SetDownloaded;

        private void OnSetDownloaded(SetDownloadedEventArgs args)
        {
            SetDownloaded?.Invoke(this, args);
        }

        public event EventHandler<PricesUpdatedEventArgs> PricesFetched;

        private void OnPricesFetched(PricesUpdatedEventArgs args)
        {
            PricesFetched?.Invoke(this, args);
        }

        public event EventHandler<ScryfallSearchEndedEventArgs> ScryfallSearchEnded;

        private void OnScryfallSearchEnded(ScryfallSearchEndedEventArgs args)
        {
            ScryfallSearchEnded?.Invoke(this, args);
        }
        #endregion Events
    }
}