using BrightIdeasSoftware;
using CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class ImportCollectionForm : Form
    {
        private string platform;
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; filenameLabel.Text = value; }
        }

        private FileFormat fileFormat;
        public FileFormat FileFormat
        {
            get { return fileFormat; }
            set
            {
                fileFormat = value;
                if (FileFormat == FileFormat.MTGODek)
                    platformComboBox.SelectedIndex = 2;
                else if (FileFormat == FileFormat.MTGAText)
                    platformComboBox.SelectedIndex = 1;
                else if (FileFormat == FileFormat.CSV)
                    platformComboBox.SelectedIndex = 0;
            }
        }
        private bool multipleCollections = false;
        private Importer importer;

        public ImportCollectionForm()
        {
            InitializeComponent();
        }
        private void platformComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (platformComboBox.SelectedIndex == 0)
            {
                platform = "Paper";
                collectionsListView.ModelFilter = new ModelFilter(c => ((CardCollection)c).Platform == "Paper");
            }
            else if (platformComboBox.SelectedIndex == 1)
            {
                platform = "Arena";
                collectionsListView.ModelFilter = new ModelFilter(c => ((CardCollection)c).Platform == "Arena");
            }
            else if (platformComboBox.SelectedIndex == 2)
            {
                platform = "MTGO";
                collectionsListView.ModelFilter = new ModelFilter(c => ((CardCollection)c).Platform == "MTGO");
            }
        }

        private void ImportCollectionForm_Shown(object sender, EventArgs e)
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var collections = context.Collections
                    .Where(c => c.GroupName != "Decks" && c.GroupName != "Wish Lists")
                    .OrderBy(c => c.GroupName)
                    .ToList();

                collectionsListView.AddObjects(collections);
                collectionsListView.AutoResizeColumns();
                var mainCollection = collections.FirstOrDefault(c => c.CollectionName == "Main" && c.Platform == platform);
                if (mainCollection != null)
                {
                    collectionsListView.SelectedObject = mainCollection;
                }
            }
            try
            {
                using (var reader = new System.IO.StreamReader(filePath))
                {
                    var firstLine = reader.ReadLine();
                    var secondLine = reader.ReadLine();
                    if (firstLine != null && firstLine.ToLower().Contains("binder name") || secondLine.ToLower().Contains("folder name"))
                    {
                        collectionNameTextBox.Text = "This file may contain multiple collections.";
                        collectionNameTextBox.Enabled = false;
                        collectionNameTextBox.Visible = true;
                        collectionNameTextBox.Top = collectionsListView.Top;
                        collectionsListView.Visible = false;
                        radioButton1.Enabled = radioButton2.Enabled = false;
                        multipleCollections = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                collectionsListView.Visible = false;
                collectionNameTextBox.Visible = true;
                collectionNameTextBox.Top = collectionsListView.Top;
            }
            else
            {
                collectionsListView.Visible = true;
                collectionNameTextBox.Visible = false;
            }
        }
        private void importButton_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (!multipleCollections)
                {
                var selectedCollection = (CardCollection)collectionsListView.SelectedObject;
                if (selectedCollection == null)
                {
                    MessageBox.Show("Please select a collection to import into.");
                    return;
                }
                importButton.Enabled = collectionsListView.Enabled = collectionNameTextBox.Enabled = radioButton1.Enabled = radioButton2.Enabled = false;
                blockProgressBar.MaxBlocks = 5;
                importer = new Importer(filePath, selectedCollection.CollectionName, FileFormat);
                importer.CollectionType = "collection";
                importer.Platform = platform;
                importer.ExistingCollection = selectedCollection;
                importWorker.RunWorkerAsync(importer);
                }
                else
                {
                    importButton.Enabled = collectionsListView.Enabled = collectionNameTextBox.Enabled = radioButton1.Enabled = radioButton2.Enabled = false;
                    blockProgressBar.MaxBlocks = 5;
                    importer = new Importer(filePath, null, FileFormat);
                    importer.CollectionType = "collection";
                    importer.Platform = platform;
                    importer.MultipleCollections = true;
                    importWorker.RunWorkerAsync(importer);
                }
            } 
            else 
            {
                var collectionName = collectionNameTextBox.Text.Trim();
                using (var context = new ScryfallCardsDbContext())
                {
                    var existingCollection = context.Collections.FirstOrDefault(c => c.CollectionName == collectionName && c.GroupName == "Collections");
                    if (existingCollection != null)
                    {
                        var count = 1;
                        while (context.Collections.Any(c => c.CollectionName == $"{collectionName} ({count})" && c.GroupName == "Collections"))
                        {
                            count++;
                        }
                        if (MessageBox.Show("A collection with this name already exists. If you'd like, a new collection will be created with a unique name.", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            collectionName = collectionNameTextBox.Text = $"{collectionName} ({count})";
                        }
                        else
                        {
                            return;
                        }
                    }
                    importButton.Enabled = collectionsListView.Enabled = collectionNameTextBox.Enabled = radioButton1.Enabled = radioButton2.Enabled = false;
                    blockProgressBar.MaxBlocks = 5;
                    importer = new Importer(filePath, collectionName, FileFormat);
                    importer.CollectionType = "collection";
                    if (platformComboBox.SelectedIndex == 0)
                        importer.Platform = "Paper";
                    else if (platformComboBox.SelectedIndex == 1)
                        importer.Platform = "Arena";
                    else if (platformComboBox.SelectedIndex == 2)
                        importer.Platform = "MTGO";
                    importWorker.RunWorkerAsync(importer);
                }
            }
        }
        private void importWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var importer = (Importer)e.Argument;
            var report = new ProgressReport { CurrentCards = 0, TotalCards = importer.CardCount, MessagePrefix = "Parsing file... " };
            importWorker.ReportProgress(0, report);
            importer.BeginImport();
            importer.Parse();
            report = new ProgressReport { CurrentCards = 0, TotalCards = importer.CardCount, MessagePrefix = "Finding items in existing catalog... " };
            importWorker.ReportProgress(0, report);
            int delay = 0;
            while (importer.ImportNextCardUsingCatalog())
            {
                if (importWorker.CancellationPending)
                {
                    e.Result = false;
                    return;
                }

                report.CurrentCards++;
                importWorker.ReportProgress(0, report);
            }
     
            importer.FillBatchableCards();
            if (importer.BatchableCount > 0)
            {
                report.MessagePrefix = "Fetching batchable items... ";
                report.CurrentCards = 0;
                report.TotalCards = importer.BatchableCount;
                importWorker.ReportProgress(0, report);
                while (importer.ImportNextBatch())
                {
                    if (importWorker.CancellationPending)
                    {
                        e.Result = false;
                        return;
                    }
     
                    report.CurrentCards += 75;
                    if (report.CurrentCards > report.TotalCards)
                        report.CurrentCards = report.TotalCards;
                    importWorker.ReportProgress(0, report);
                    Thread.Sleep(500);
                }
            }
           
            if (importer.UncataloguedCount > 0)
            {
                importer.CopyUncataloguedCards();
                report.MessagePrefix = "Importing remaining items... ";
                report.CurrentCards = 0;
                report.TotalCards = importer.CardCount;

                while (importer.ImportNextCard(out delay, false))
                {
                    if (importWorker.CancellationPending)
                    {
                        e.Result = false;
                        return;
                    }
           
                    report.CurrentCards++;
                    importWorker.ReportProgress(0, report);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }
            e.Result = true;
        }
        private void importWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var report = (ProgressReport)e.UserState;
            blockProgressBar.CurrentBlocks = (int)(report.CurrentCards / (double)report.TotalCards * blockProgressBar.MaxBlocks);
            if (blockProgressBar.CurrentBlocks == blockProgressBar.MaxBlocks)
                blockProgressBar.BarColor = Color.Black;
            progressLabel.Text = $"{report.MessagePrefix}";
            if (report.TotalCards > 0)
                progressLabel.Text += $"{report.CurrentCards}/{report.TotalCards} ({(double)report.CurrentCards / report.TotalCards:P0})";
            progressLabel.Width = blockProgressBar.Width;
        }
        private void importWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var success = (bool)e.Result;
            if (!success)
            {
                importer.CancelImport();
                MessageBox.Show("Import canceled.");
                return;
            }

            try
            {
                importer.CommitImport();
            }
            catch (Exception ex)
            {
                importer.CancelImport();
                MessageBox.Show($"An error occurred while committing the import: {ex.Message}\n{ex.InnerException.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            List<InventoryCard> cardsAdded = new List<InventoryCard>();
            using (var context = new ScryfallCardsDbContext())
                foreach (var card in importer.cardsAdded)
                    cardsAdded.Add(card.ToFullCard(context));
         
            Globals.Forms.DBViewForm.InventoryChanged(this, new InventoryChangedEventArgs { Cards = cardsAdded });
            if (importer.FailedCards.Count > 0)
            {
                failedLabel.Visible = true;
                failedTextBox.Visible = true;
                Height = 730;
                foreach (var card in importer.FailedCards)
                {
                    failedTextBox.AppendText($"{card.Quantity} {card.CardName}{(!string.IsNullOrEmpty(card.SetCode) ? $" {card.SetCode}" : "")}{(!string.IsNullOrEmpty(card.CollectorNumber) ? $" {card.CollectorNumber}" : "")}{Environment.NewLine}");
                }
            }
            else
            {
                MessageBox.Show("Collection imported successfully.");
            }
            if (importer.NewCollection != null)
            {
                var collection = importer.NewCollection;
                CardManager.LoadCollection(collection);
                var navForm = Globals.Forms.NavigationForm;
                if (navForm != null)
                {
                    navForm.AddCollection(collection);
                }
            }
            else if (importer.ExistingCollection != null)
            {
                var collection = importer.ExistingCollection;
                foreach (var form in Globals.Forms.OpenCollectionForms)
                {
                    if (form.Collection.Id == collection.Id)
                    {
                        form.Close();
                        Globals.Forms.OpenCollectionForms.Remove(form);
                        break;
                    }
                }
                CardManager.LoadCollection(collection);
            }
            else if (multipleCollections)
            {
                var navForm = Globals.Forms.NavigationForm;
                if (navForm != null)
                    navForm.ReloadCollections();

                foreach (var collection in importer.Collections.Values)
                {
                    foreach (var form in Globals.Forms.OpenCollectionForms)
                    {
                        if (form.Collection.Id == collection.Id)
                        {
                            form.Close();
                            Globals.Forms.OpenCollectionForms.Remove(form);
                            CardManager.LoadCollection(collection.Id);
                            break;
                        }
                    }
                }
            }
        }
        private void ImportCollectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (importWorker.IsBusy) importWorker.CancelAsync();
        }
    }
}
