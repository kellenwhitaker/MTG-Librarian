﻿using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using CustomControls;

namespace MTG_Librarian
{
    public interface IBackgroundTask
    {
        object TaskObject { get; set; }
        string Caption { get; set; }
        RunWorkerCompletedEventHandler OnTaskCompleted { set; }
        BlockProgressBar ProgressBar { get; }
        int TotalWorkUnits { get; }
        int CompletedWorkUnits { get; }
        int Runtime { get; }
        bool Running { get; }
        RunState RunState { get; }
        Image Icon { get; }
    }

    public enum RunState { Initialized, Running, Paused, Stopped, Completed, Failed }

    [DesignerCategory("Code")]
    public abstract class BackgroundTask : BackgroundWorker, IBackgroundTask
    {
        protected Stopwatch watch;
        protected object lockObject = new object();
        public object TaskObject { get; set; }
        public string Caption { get; set; }
        public RunWorkerCompletedEventHandler OnTaskCompleted { set => RunWorkerCompleted += value; }
        public int Runtime { get => (int)(watch.ElapsedMilliseconds / 1000); }
        public bool Running { get => RunState == RunState.Running; }
        public BlockProgressBar ProgressBar { get; private set; }
        public bool ForDisplay { get; set; } = false;
        private RunState _runState;
        public RunState RunState { get => _runState; protected set { _runState = value; if (_runState == RunState.Failed) Caption = $"[Failed] {Caption}"; } }
        private Image _icon;
        public Image Icon
        {
            get => _icon;
            protected set
            {
                if (value != null)
                {
                    Rectangle cloneRect = new Rectangle(0, 0, value.Width, value.Height);
                    System.Drawing.Imaging.PixelFormat format = value.PixelFormat;
                    _icon = ((Bitmap)value).GetCopyOf();
                }
                else
                    _icon = null;
            }
        }
        private int _TotalWorkUnits;
        public int TotalWorkUnits
        {
            get => _TotalWorkUnits;
            protected set
            {
                _TotalWorkUnits = value;
                if (ProgressBar != null)
                {
                    if (TotalWorkUnits != 0)
                        ProgressBar.CurrentBlocks = _CompletedWorkUnits / _TotalWorkUnits;
                    else
                        ProgressBar.CurrentBlocks = 0;
                }
            }
        }
        private int _CompletedWorkUnits;
        public int CompletedWorkUnits
        {
            get => _CompletedWorkUnits;
            set
            {
                _CompletedWorkUnits = value;
                ProgressBar.CurrentBlocks = (int)((float)_CompletedWorkUnits / _TotalWorkUnits * ProgressBar.MaxBlocks);
            }
        }
        public abstract void Run();

        public BackgroundTask()
        {
            ProgressBar = new BlockProgressBar
            {
                Height = 5,
                Width = 190,
                MaxBlocks = 5,
                CurrentBlocks = 0
            };
            watch = new Stopwatch();
            RunState = RunState.Initialized;
        }
    }
}
