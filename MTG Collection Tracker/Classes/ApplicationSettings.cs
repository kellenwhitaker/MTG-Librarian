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
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0,0")]
        public Point MainFormLocation
        {
            get => (Point)this["MainFormLocation"];
            set => this["MainFormLocation"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0,0")]
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

        public DockPaneSettings GetDockPaneSettings(DockState dockState)
        {
            switch (dockState)
            {
                case DockState.DockBottom: return DockPaneBottom;
                case DockState.DockLeft: return DockPaneLeft;
                case DockState.DockRight: return DockPaneRight;
                default: return DockPaneDocuments;
            }
        }
    }
}
