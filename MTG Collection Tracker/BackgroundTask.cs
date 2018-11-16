using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using CustomControls;

namespace MTG_Collection_Tracker
{
    public interface IBackgroundTask
    {
        string Caption { get; set; }
        BlockProgressBar ProgressBar { get; }
        int TotalWorkUnits { get; }
        int CompletedWorkUnits { get; }
        int Runtime { get; }
        bool Running { get; }
        RunState RunState { get; }
        Image Icon { get; }
    }

    public enum RunState { Created, Initialized, Running, Paused, Stopped, Completed }

    public abstract class BackgroundTask : BackgroundWorker, IBackgroundTask
    {
        protected Stopwatch watch;
        protected object lockObject = new object();
        public string Caption { get; set; }
        public int Runtime { get => (int)(watch.ElapsedMilliseconds / 1000); }
        public bool Running { get => RunState == RunState.Running; }
        public BlockProgressBar ProgressBar { get; private set; }
        public RunState RunState { get; protected set; }
        private Image _icon;
        public Image Icon
        {
            get
            {
                if (_icon == null) return _icon;
                Image copy;
                lock (lockObject)
                    copy = (Image)_icon.Clone();
                return copy;
            }
            protected set
            {
                lock (lockObject)
                    _icon = value;
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

        public BackgroundTask(int totalWorkUnits)
        {
            TotalWorkUnits = totalWorkUnits;
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
