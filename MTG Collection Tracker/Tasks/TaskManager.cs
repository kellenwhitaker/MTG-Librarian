﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using CustomControls;
using System.Collections;
//TODO5: figure out why task manager quits partway through a queue of tasks
namespace MTG_Librarian
{
    [DesignerCategory("Code")]
    public class TaskManager : BackgroundWorker
    {
        private readonly ConcurrentDeque<BackgroundTask> _IncomingTasks;
        private readonly ConcurrentDeque<BackgroundTask> _AllTasks;
        private readonly List<BackgroundTask> _activeTasks;
        private readonly List<BackgroundTask> _completedTasks;
        private readonly BrightIdeasSoftware.ObjectListView listView;
        private readonly Label tasksLabel;
        private readonly BlockProgressBar progressBar;
        public int TaskCount => _AllTasks.Count;

        public TaskManager(Label label, BlockProgressBar progressBar)
        {
            listView = Globals.Forms.TasksForm.tasksListView;
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
                if (task.AddFirst)
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

        public void AddTasks(List<BackgroundTask> tasks)
        {
            if (tasks != null)
            {
                foreach (BackgroundTask task in tasks)
                {
                    if (task.AddFirst)
                        _IncomingTasks.AddFirst(task);
                    else
                        _IncomingTasks.Enqueue(task);
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
                if (_activeTasks.Count > 0)
                {
                    double totalWorkUnits = _AllTasks.Count;
                    int completedUnits = _completedTasks.Count;

                    if (progressBar.MaxBlocks == 0) progressBar.MaxBlocks = 5;
                    progressBar.CurrentBlocks = (int)(completedUnits / totalWorkUnits * progressBar.MaxBlocks);
                    int percentDone = (int)(completedUnits / totalWorkUnits * 100);
                    tasksLabel.Text = $"{_completedTasks.Count} / {_AllTasks.Count} tasks completed —— {percentDone}%";
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

        public event EventHandler<SetDownloadedEventArgs> SetDownloaded;
        private void OnSetDownloaded(SetDownloadedEventArgs args)
        {
            SetDownloaded?.Invoke(this, args);
        }

        public event EventHandler<PricesUpdatedEventArgs> PricesUpdated;
        private void OnPricesUpdated(PricesUpdatedEventArgs args)
        {
            PricesUpdated?.Invoke(this, args);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            var watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                while (_IncomingTasks.TryDequeue(out BackgroundTask nextTask))
                {
                    if (nextTask.AddFirst)
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
                    var completedOrFailed = _activeTasks.FindCompletedOrFailed().ToArray();
                    foreach (var task in completedOrFailed)
                    {
                        _activeTasks.Remove(task);
                        _completedTasks.Add(task);
                        if (_activeTasks.Count == 0 && _IncomingTasks.Count == 0)
                            ResetState();
                        if (task is DownloadSetTask downloadTask && task.RunState != RunState.Failed)
                            OnSetDownloaded(new SetDownloadedEventArgs { SetCode = downloadTask.CardSet.Code });
                        else if (task is GetTCGPlayerPricesTask getPricesTask && task.RunState != RunState.Failed)
                            OnPricesUpdated(new PricesUpdatedEventArgs { Prices = getPricesTask.productIdDictionary });
                            
                    }
                    if (completedOrFailed.Count() > 0 && listView.Objects != null)
                        MoveLVObjects(completedOrFailed);

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
