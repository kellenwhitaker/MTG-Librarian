using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MTG_Collection_Tracker
{
    public partial class MainForm : Form
    {
        private void InitUIWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading collections...", 1));
            navForm.LoadGroups();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading catalog...", 2));
            dbViewForm.LoadSets();
            AddSetIcons();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Starting application...", 3));
            Thread.Sleep(100);
        }

        private void InitUIWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            splash.ProgressChanged(e.UserState as SplashProgressObject);
        }

        private void InitUIWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cardInfoForm.Show(dockPanel1, DockState.DockLeft);
            navForm.LoadTree();
            navForm.CollectionActivated += navFormCollectionActivated;
            navForm.Show(dockPanel1, DockState.DockRight);
            tasksForm.Show(dockPanel1, DockState.DockRight);
            navForm.Activate();
            dbViewForm.LoadTree();
            dbViewForm.Show(dockPanel1, DockState.DockBottom);
            dockPanel1.UpdateDockWindowZOrder(DockStyle.Left, true);
            dockPanel1.UpdateDockWindowZOrder(DockStyle.Right, true);
            CheckForNewSetsWorker.RunWorkerAsync();
            Show();
        }

        private void checkForNewSetsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sets = GetGathererSets();
            var archiveSets = GetWizardsArchiveSets();
            //var wikiSets = GetWikipediaSets();
            foreach (var set in sets)
            {
                string setName = (set.Replace(" & ", " AND ")).StripPunctuation().Trim();
                setName = setName.ToUpper();
                if (setName.EndsWith(" EDITION"))
                    setName = setName.Replace(" EDITION", "");
                setName = setName.ToUpper().Replace("CORE SET", "").Replace("MAGIC THE GATHERING", "").Replace("DUEL DECKS", "").Replace("BOX SET", "").Trim();
            }
            using (var context = new MyDbContext())
            {
                var DBSets = from s in context.Sets
                             select s;

                foreach (var set in DBSets)
                {
                    if (sets.Contains(set.Name))
                        sets.Remove(set.Name);
                }
            }
            e.Result = sets;
        }

        private void CheckForNewSetsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sets = e.Result as List<string>;
            if (sets.Count > 0)
            {
                string newSets = "";
                foreach (var set in sets)
                    newSets += "[" + set + "] ";

                if (MessageBox.Show("The following new sets are available for download: \n\n" + newSets + "\n\nWould you like to download them now?", "New Sets Are Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (var set in sets)
                    {
                        DownloadSetTask task = new DownloadSetTask(set);
                        tasksForm.taskManager.AddTask(task);
                    }
                }
            }
        }
    }
}
