using KW.WinFormsUI.Docking;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class SettingsManager
    {
        public ApplicationSettings ApplicationSettings { get; set; }

        public SettingsManager()
        {
            ApplicationSettings = new ApplicationSettings();
        }

        public void FillSettings()
        {
            ApplicationSettings.InInitialState = false;
            FillMainFormSettings();
            FillDockSettings();
        }

        private void FillMainFormSettings()
        {
            ApplicationSettings.MainFormWindowState = Globals.Forms.MainForm.WindowState;
            ApplicationSettings.MainFormLocation = Globals.Forms.MainForm.Location;
            ApplicationSettings.MainFormSize = Globals.Forms.MainForm.Size;
        }

        private void FillDockSettings()
        {
            ApplicationSettings.ClearDockPaneSettings();
            ApplicationSettings.DockLeftPortion = Globals.Forms.DockPanel.DockLeftPortion;
            ApplicationSettings.DockRightPortion = Globals.Forms.DockPanel.DockRightPortion;
            ApplicationSettings.DockBottomPortion = Globals.Forms.DockPanel.DockBottomPortion;
            SaveDockState(DockState.DockLeft);
            SaveDockState(DockState.Document);
            SaveDockState(DockState.DockBottom);
            SaveDockState(DockState.DockRight);
        }

        public void SaveDockState(DockState dockState)
        {
            var dockWindow = Globals.Forms.DockPanel.DockWindows[dockState];
            if (dockWindow != null && dockWindow.NestedPanes.Count > 0)
            {
                var pane = dockWindow.NestedPanes[0];
                var settings = ApplicationSettings.GetDockPaneSettings(dockState, pane.IsAutoHide);
                settings.ZOrderIndex = dockWindow.GetChildIndex();
                var contentArray = pane.DockContentArray;
                var activeContent = pane.ActiveContent;
                foreach (var dockContent in contentArray)
                {
                    if (dockContent is CardInfoForm cardInfoForm)
                        settings.ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.CardInfoForm, IsActivated = cardInfoForm == activeContent });
                    else if (dockContent is DBViewForm dBViewForm)
                        settings.ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.DBViewForm, IsActivated = dBViewForm == activeContent });
                    else if (dockContent is CollectionNavigatorForm cardNavigatorForm)
                        settings.ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.NavigatorForm, IsActivated = cardNavigatorForm == activeContent });
                    else if (dockContent is TasksForm tasksForm)
                        settings.ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.TasksForm, IsActivated = tasksForm == activeContent });
                    else if (dockContent is CollectionViewForm collectionViewForm)
                        settings.ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.CollectionViewForm, IsActivated = collectionViewForm == activeContent, DocumentId = collectionViewForm.Collection.Id });
                }
            }
        }

        public void SaveSettings()
        {
            ApplicationSettings.Save();
        }

        public void LayoutMainForm()
        {
            Globals.Forms.MainForm.WindowState = ApplicationSettings.MainFormWindowState;
            Globals.Forms.MainForm.Location = ApplicationSettings.MainFormLocation;
            Globals.Forms.MainForm.Size = ApplicationSettings.MainFormSize;
        }

        public void LayoutDockPanel()
        {
            Globals.Forms.DockPanel.SuspendLayout();
            Globals.Forms.DBViewForm.SuspendLayout();
            Globals.Forms.CardInfoForm.SuspendLayout();
            Globals.Forms.NavigationForm.SuspendLayout();
            Globals.Forms.TasksForm.SuspendLayout();
            if (!ApplicationSettings.InInitialState)
            {
                var ZOrderDictionary = new SortedDictionary<int, DockState>();
                SetupDockPanel(DockState.DockBottom, ZOrderDictionary);
                SetupDockPanel(DockState.DockLeft, ZOrderDictionary);
                SetupDockPanel(DockState.DockRight, ZOrderDictionary);
                SetupDockPanel(DockState.Document, ZOrderDictionary);
                SetupDockPanel(DockState.DockBottomAutoHide, ZOrderDictionary);
                SetupDockPanel(DockState.DockLeftAutoHide, ZOrderDictionary);
                SetupDockPanel(DockState.DockRightAutoHide, ZOrderDictionary);
                RestoreZOrder(ZOrderDictionary);
            }
            else
                SetupDefaultDockConfiguration();

            Globals.Forms.DockPanel.DockLeftPortion = ApplicationSettings.DockLeftPortion;
            Globals.Forms.DockPanel.DockRightPortion = ApplicationSettings.DockRightPortion;
            Globals.Forms.DockPanel.DockBottomPortion = ApplicationSettings.DockBottomPortion;
            Globals.Forms.DockPanel.ResumeLayout();
            Globals.Forms.DBViewForm.ResumeLayout();
            Globals.Forms.CardInfoForm.ResumeLayout();
            Globals.Forms.NavigationForm.ResumeLayout();
            Globals.Forms.TasksForm.ResumeLayout();
        }

        private void SetupDockPanel(DockState dockState, SortedDictionary<int, DockState> ZOrderDictionary)
        {
            var settings = ApplicationSettings.GetDockPaneSettings(dockState);
            if (!ZOrderDictionary.Keys.Any(x => x == settings.ZOrderIndex))
                ZOrderDictionary.Add(settings.ZOrderIndex, dockState);
            var dockWindow = Globals.Forms.DockPanel.DockWindows[dockState];
            var contentPanes = settings.ContentPanes;
            DockContent activatedContent = null;
            if (contentPanes != null && contentPanes.Count > 0)
            {
                foreach (var contentPane in contentPanes)
                {
                    var dockContent = ShowForm(contentPane, dockState);
                    if (contentPane.IsActivated)
                        activatedContent = dockContent;
                }
                activatedContent?.Activate();
            }
        }

        private DockContent ShowForm(ApplicationSettings.DockContentSettings contentSettings, DockState dockState)
        {
            DockContent dockContent;
            if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.CardInfoForm)
                dockContent = Globals.Forms.CardInfoForm;
            else if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.DBViewForm)
                dockContent = Globals.Forms.DBViewForm;
            else if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.NavigatorForm)
                dockContent = Globals.Forms.NavigationForm;
            else if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.TasksForm)
                dockContent = Globals.Forms.TasksForm;
            else
                dockContent = Globals.Forms.MainForm.LoadCollection(contentSettings.DocumentId, dockState);

            dockContent?.Show(Globals.Forms.DockPanel, dockState);
            return dockContent;
        }

        private void RestoreZOrder(SortedDictionary<int, DockState> ZOrderDictionary)
        {
            foreach (KeyValuePair<int, DockState> pair in ZOrderDictionary)
                Globals.Forms.DockPanel.UpdateDockWindowZOrder(pair.Value.ToDockStyle(), true);
        }

        private void SetupDefaultDockConfiguration()
        {
            Globals.Forms.DBViewForm.Show(Globals.Forms.DockPanel, DockState.DockBottom);
            Globals.Forms.CardInfoForm.Show(Globals.Forms.DockPanel, DockState.DockLeft);
            Globals.Forms.TasksForm.Show(Globals.Forms.DockPanel, DockState.DockRight);
            Globals.Forms.NavigationForm.Show(Globals.Forms.DockPanel, DockState.DockRight);
            Globals.Forms.DockPanel.UpdateDockWindowZOrder(DockStyle.Left, true);
            Globals.Forms.DockPanel.UpdateDockWindowZOrder(DockStyle.Right, true);
            CardCollection mainCollection;
            using (var context = new MyDbContext())
                mainCollection = (from c in context.Collections
                                  where c.CollectionName == "Main"
                                  select c).FirstOrDefault();

            if (mainCollection != null)
                Globals.Forms.MainForm.LoadCollection(mainCollection);
        }
    }
}