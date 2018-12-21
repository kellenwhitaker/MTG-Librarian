using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using KW.WinFormsUI.Docking;
using System.Drawing;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class ApplicationSettings : ApplicationSettingsBase
    {
        public enum DockContentEnum { CollectionViewForm, DBViewForm, NavigatorForm, CardInfoForm, TasksForm }

        public class DockContentSettings
        {
            public DockContentEnum ContentType { get; set; }
            public int DocumentId { get; set; }
            public bool IsActivated { get; set; } = false;
        }

        public class DockPaneSettings
        {
            public List<DockContentSettings> ContentPanes { get; set; } = new List<DockContentSettings>();
            public int ActivePaneIndex { get; set; }
            public int ZOrderIndex { get; set; }
        }

        public void ClearDockPaneSettings()
        {
            DockPaneLeft = new DockPaneSettings();
            DockPaneRight = new DockPaneSettings();
            DockPaneBottom = new DockPaneSettings();
            DockPaneDocuments = new DockPaneSettings();
            DockPaneLeftAutoHide = new DockPaneSettings();
            DockPaneRightAutoHide = new DockPaneSettings();
            DockPaneBottomAutoHide = new DockPaneSettings();
            DockRightPortion = 0.25;
            DockLeftPortion = 0.25;
            DockBottomPortion = 0.45;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0.25")]
        public double DockLeftPortion
        {
            get => (double)this["DockLeftPortion"];
            set => this["DockLeftPortion"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0.25")]
        public double DockRightPortion
        {
            get => (double)this["DockRightPortion"];
            set => this["DockRightPortion"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0.45")]
        public double DockBottomPortion
        {
            get => (double)this["DockBottomPortion"];
            set => this["DockBottomPortion"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0,0")]
        public Point MainFormLocation
        {
            get => (Point)this["MainFormLocation"];
            set => this["MainFormLocation"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("1024,768")]
        public Size MainFormSize
        {
            get => (Size)this["MainFormSize"];
            set => this["MainFormSize"] = value;
        }


        [UserScopedSetting()]
        [DefaultSettingValue("2")]
        public FormWindowState MainFormWindowState
        {
            get => (FormWindowState)this["MainFormWindowState"];
            set => this["MainFormWindowState"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneLeft
        {
            get => (DockPaneSettings)this["DockPaneLeft"];
            set => this["DockPaneLeft"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneDocuments
        {
            get => (DockPaneSettings)this["DockPaneDocuments"];
            set => this["DockPaneDocuments"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneBottom
        {
            get => (DockPaneSettings)this["DockPaneBottom"];
            set => this["DockPaneBottom"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneRight
        {
            get => (DockPaneSettings)this["DockPaneRight"];
            set => this["DockPaneRight"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneRightAutoHide
        {
            get => (DockPaneSettings)this["DockPaneRightAutoHide"];
            set => this["DockPaneRightAutoHide"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneLeftAutoHide
        {
            get => (DockPaneSettings)this["DockPaneLeftAutoHide"];
            set => this["DockPaneLeftAutoHide"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public DockPaneSettings DockPaneBottomAutoHide
        {
            get => (DockPaneSettings)this["DockPaneBottomAutoHide"];
            set => this["DockPaneBottomAutoHide"] = value;
        }

        public DockPaneSettings GetDockPaneSettings(DockState dockState, bool AutoHide)
        {
            if (!AutoHide)
            {
                switch (dockState)
                {
                    case DockState.DockBottom: return DockPaneBottom;
                    case DockState.DockLeft: return DockPaneLeft;
                    case DockState.DockRight: return DockPaneRight;
                    default: return DockPaneDocuments;
                }
            }
            else
            {
                switch (dockState)
                {
                    case DockState.DockBottom: return DockPaneBottomAutoHide;
                    case DockState.DockLeft: return DockPaneLeftAutoHide;
                    default: return DockPaneRightAutoHide;
                }
            }
        }

        public DockPaneSettings GetDockPaneSettings(DockState dockState)
        {
            switch (dockState)
            {
                case DockState.DockBottom: return DockPaneBottom;
                case DockState.DockLeft: return DockPaneLeft;
                case DockState.DockRight: return DockPaneRight;
                case DockState.DockBottomAutoHide: return DockPaneBottomAutoHide;
                case DockState.DockLeftAutoHide: return DockPaneLeftAutoHide;
                case DockState.DockRightAutoHide: return DockPaneRightAutoHide;
                default: return DockPaneDocuments;
            }
        }
    }
}

