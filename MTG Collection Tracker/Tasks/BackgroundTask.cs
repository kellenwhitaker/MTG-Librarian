using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using CustomControls;

namespace MTG_Librarian
{
    #region Interfaces

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

    #endregion Interfaces

    #region Enums

    public enum RunState { Initialized, Running, Paused, Stopped, Completed, Failed }

    #endregion Enums

    #region Classes

    [DesignerCategory("Code")]
    public abstract class BackgroundTask : BackgroundWorker, IBackgroundTask
    {
        #region Properties

        public RunWorkerCompletedEventHandler OnTaskCompleted { set => RunWorkerCompleted += value; }
        public int Runtime { get => (int)(watch.ElapsedMilliseconds / 1000); }
        public bool Running { get => RunState == RunState.Running; }
        public BlockProgressBar ProgressBar { get; private set; }
        public bool AddFirst { get; set; } = false;
        public object TaskObject { get; set; }
        public string Caption { get; set; }
        public RunState RunState { get => _runState; protected set { _runState = value; if (_runState == RunState.Failed) Caption = $"[Failed] {Caption}"; } }

        public Image Icon
        {
            get => _icon;
            protected set
            {
                if (value != null)
                {
                    var cloneRect = new Rectangle(0, 0, value.Width, value.Height);
                    var format = value.PixelFormat;
                    _icon = ((Bitmap)value).GetCopyOf();
                }
                else
                    _icon = null;
            }
        }

        public int TotalWorkUnits
        {
            get => _TotalWorkUnits;
            protected set
            {
                _TotalWorkUnits = value;
                if (ProgressBar != null)
                {
                    if (TotalWorkUnits != 0)
                        SetProgressBarCurrentBlocks(_CompletedWorkUnits / _TotalWorkUnits);
                    else
                        SetProgressBarCurrentBlocks(0);
                }
            }
        }

        public int CompletedWorkUnits
        {
            get => _CompletedWorkUnits;
            set
            {
                _CompletedWorkUnits = value;
                SetProgressBarCurrentBlocks((int)((float)_CompletedWorkUnits / _TotalWorkUnits * ProgressBar.MaxBlocks));
            }
        }

        #endregion Properties

        #region Fields

        protected Stopwatch watch;
        protected object lockObject = new object();
        private RunState _runState;
        private Image _icon;
        private int _TotalWorkUnits;
        private int _CompletedWorkUnits;

        #endregion Fields

        #region Constructors

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

        #endregion Constructors

        #region Methods

        private delegate void SetProgressBarCurrentBlocksDelegate(int blocks);

        private void SetProgressBarCurrentBlocks(int blocks)
        {
            if (ProgressBar.InvokeRequired)
                ProgressBar.BeginInvoke(new SetProgressBarCurrentBlocksDelegate(SetProgressBarCurrentBlocks), blocks);
            else
                ProgressBar.CurrentBlocks = blocks;
        }

        public virtual void Run()
        {
            RunState = RunState.Running;
            watch.Start();
            RunWorkerAsync();
        }

        #endregion Methods
    }

    #endregion Classes
}