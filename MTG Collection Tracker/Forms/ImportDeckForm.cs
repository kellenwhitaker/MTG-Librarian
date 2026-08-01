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
    public partial class ImportDeckForm : Form
    {
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
                if (FileFormat == FileFormat.MTGOText || FileFormat == FileFormat.MTGODek)
                    platformComboBox.SelectedIndex = 2;
                else if (FileFormat == FileFormat.MTGAText)
                    platformComboBox.SelectedIndex = 1;
            } 
        }

        private Importer importer;

        public ImportDeckForm()
        {
            InitializeComponent();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            var deckName = deckNameTextBox.Text.Trim();
            using (var context = new ScryfallCardsDbContext())
            {
                var existingDeck = context.Collections.FirstOrDefault(c => c.CollectionName == deckName && c.GroupName == "Decks");
                if (existingDeck != null)
                {
                    var count = 1;
                    while (context.Collections.Any(c => c.CollectionName == $"{deckName} ({count})" && c.GroupName == "Decks"))
                    {
                        count++;
                    }
                    if (MessageBox.Show("A deck with this name already exists. If you'd like, a new deck will be created with a unique name.", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        deckName = deckNameTextBox.Text = $"{deckName} ({count})";
                    }
                    else
                    {
                        return;
                    }
                }
                importButton.Enabled = false;
                blockProgressBar.MaxBlocks = 5;
                importer = new Importer(filePath, deckName, FileFormat);
                if (platformComboBox.SelectedIndex == 0)
                    importer.Platform = "Paper";
                else if (platformComboBox.SelectedIndex == 1)
                    importer.Platform = "Arena";
                else if (platformComboBox.SelectedIndex == 2)
                    importer.Platform = "MTGO";
                importWorker.RunWorkerAsync(importer);
            }
        }

        private void importWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var importer = (Importer)e.Argument;
            var report = new ProgressReport { CurrentCards = 0, TotalCards = 0, MessagePrefix = "Parsing file... "};
            importWorker.ReportProgress(0, report);
            importer.BeginImport();
            importer.Parse();
            report = new ProgressReport { CurrentCards = 0, TotalCards = importer.CardCount, MessagePrefix = "" };
            importWorker.ReportProgress(0, report);
            int delay = 0;
            while (importer.ImportNextCard(out delay))
            {
                if (importWorker.CancellationPending)
                {
                    e.Result = false;
                    return;
                }
       
                report.CurrentCards++;
                importWorker.ReportProgress(0, report);
                Thread.Sleep(delay);
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
            
            if (importer.FailedCards.Count > 0)
            {
                failedLabel.Visible = true;
                failedTextBox.Visible = true;
                Height = 440;
                foreach (var card in importer.FailedCards)
                {
                    failedTextBox.AppendText($"{card.Quantity} {card.CardName}{(!string.IsNullOrEmpty(card.SetCode) ? $" {card.SetCode}" : "")}{(!string.IsNullOrEmpty(card.CollectorNumber) ? $" {card.CollectorNumber}" : "")}{Environment.NewLine}");
                }
            }
            else
            {
                MessageBox.Show("Deck imported successfully.");
            }
            var collection = importer.NewCollection;
            if (collection != null)
            {
                CardManager.LoadCollection(collection);
                var navForm = Globals.Forms.NavigationForm;
                if (navForm != null)
                {
                    navForm.AddCollection(collection);
                }
            }
        }
        private void ImportDeckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (importWorker.IsBusy) importWorker.CancelAsync();
        }
    }

    public class ProgressReport
    {
        public int CurrentCards { get; set; }
        public int TotalCards { get; set; }
        public string MessagePrefix { get; set; }
    }
}
