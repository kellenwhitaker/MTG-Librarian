using BrightIdeasSoftware;
using KW.WinFormsUI.Docking;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian.Forms
{
    public partial class QuickEntryForm : DockForm
    {
        private string lastPrintingsCardName = null;
        private ScryfallMagicCard lastAddedCard;
        public QuickEntryForm()
        {
            InitializeComponent();
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
            var img = Properties.Resources.Magic_card_back;
            cardPictureBox.Image = img.ScaleImage(cardPictureBox.Width, cardPictureBox.Height);
            EventManager.CardImageRetrieved += CardImageRetrieved;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChangedAsync;
        }

        private async void ComboBox1_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            string selectedCardName = comboBox1.SelectedItem?.ToString();
            await WaitAsync().ConfigureAwait(true);
            if (selectedCardName == comboBox1.SelectedItem?.ToString())
                GetPrintings();
        }
        private async Task WaitAsync()
        {
            await Task.Delay(100).ConfigureAwait(true);
            TimeSpan elapsed;
            lock (Globals.States.LastScryfallSearchLockObject)
            {
                elapsed = DateTime.Now - Globals.States.LastScryfallSearchTime;
            }
            if (elapsed < TimeSpan.FromMilliseconds(400))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(400) - elapsed).ConfigureAwait(true);
            }
        }
        private void CardImageRetrieved(object sender, CardImageRetrievedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CardImageRetrieved(sender, e)));
                return;
            }
            else
            {
                if (fastObjectListView1.SelectedObject is ScryfallMagicCard selectedCard && selectedCard.ScryfallId == e.uuid)
                    cardPictureBox.Image = e.CardImage.ScaleImage(cardPictureBox.Width, cardPictureBox.Height);
            }
        }
        public void ActiveListViewChanged()
        {
            if (Globals.Forms.DockPanel.ActiveDocument is CollectionViewForm activeCollectionForm)
            {
                if (activeCollectionForm.LastActiveListView == activeCollectionForm.cardListView)
                    radioButton1.Checked = true;
                else if (activeCollectionForm.LastActiveListView == activeCollectionForm.sideboardListView)
                    radioButton2.Checked = true;
            }
        }
        private async void GetPrintings()
        {
            DebugOutput.WriteLine("GetPrintings called: " + comboBox1.Text);
            var cardName = comboBox1.Text?.Trim();
            if (cardName == lastPrintingsCardName)
                return;
            lastPrintingsCardName = cardName;
            var DefaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
            ScryfallMagicCard lowestPrice = null;

            try
            {
                var searchLanguage = SettingsManager.ApplicationSettings.DefaultSearchLanguage;
                var collectionForm = Globals.Forms.DockPanel.ActiveDocument as CollectionViewForm;
                if (collectionForm == null)
                    return;
                var collection = collectionForm.Collection;
                if (string.IsNullOrWhiteSpace(cardName))
                    return;

                var printings = new List<ScryfallCard>();
                // Use exact-name search to target the specific card and request all prints
                var query = $"!\"{cardName}\"";
                var order = collection.Platform.ToLower() == "mtgo" ? "tix" : DefaultPaperCurrency.ToLower();
                var endpoint = "https://api.scryfall.com/cards/search?q=" + Uri.EscapeUriString(query) + $"+lang%3A{Globals.Methods.AbbreviateLanguage(searchLanguage)}+unique%3Aprints+include%3Aextras+game%3A{collection.Platform.ToLower()}" + $"&order={order}&dir=asc";

                string nextPageUri = endpoint;

                while (!string.IsNullOrEmpty(nextPageUri))
                {
                    // Create a client for the current page URI and request the root resource
                    var client = new RestClient(nextPageUri);
                    var request = new RestRequest("", Method.Get);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");

                    TimeSpan elapsed;
                    lock (Globals.States.LastScryfallSearchLockObject)
                    {
                        elapsed = DateTime.Now - Globals.States.LastScryfallSearchTime;
                    }
                    if (elapsed < TimeSpan.FromMilliseconds(500))
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500)).ConfigureAwait(true);
                    }
                    lock (Globals.States.LastScryfallSearchLockObject)
                    {
                        Globals.States.LastScryfallSearchTime = DateTime.Now;
                    }
                    // Await the async request to avoid blocking the UI thread
                    var response = await client.ExecuteAsync(request).ConfigureAwait(true);
                    if (cardName != comboBox1.Text?.Trim())
                        return; // User changed the card name during the request, abort processing
                    if (response == null)
                        throw new InvalidDataException("No response received from Scryfall.");

                    if (response.ErrorException != null)
                    {
                        // Log and abort paging on network errors
                        DebugOutput.WriteLine($"Network error fetching {nextPageUri}: {response.ErrorMessage ?? response.ErrorException.Message}");
                        return;
                    }

                    if (string.IsNullOrEmpty(response.Content))
                    {
                        DebugOutput.WriteLine($"Empty response content from {nextPageUri}");
                        return;
                    }

                    var responseObject = JsonConvert.DeserializeObject<ScryfallCardList>(response.Content);
                    if (responseObject == null)
                        throw new InvalidDataException("Invalid JSON encountered");

                    if (responseObject.Object == "error")
                    {
                        DebugOutput.WriteLine($"error: {nextPageUri}");
                        DebugOutput.WriteLine(response.Content);
                        return;
                    }

                    if (responseObject.data != null && responseObject.data.Count() > 0)
                    {
                        foreach (var card in responseObject.data)
                        {
                            var magicCard = card.ToScryfallMagicCard();
                            string priceString = null;
                            double? price = null;
                            string finish = "nonfoil";
                            if (card.finishes.Count() == 1)
                                finish = card.finishes[0];
                            if (collection.Platform.ToLower() == "mtgo")
                                card.prices.TryGetValue("tix", out priceString);
                            else if (collection.Platform.ToLower() == "paper")
                                card.prices.TryGetValue($"{DefaultPaperCurrency.ToLower()}{((finish != "nonfoil") ? $"_{finish}" : "")}", out priceString);

                            if (!string.IsNullOrEmpty(priceString))
                            {
                                magicCard.Price = Convert.ToDouble(priceString);
                                if (lowestPrice == null && magicCard.Price.HasValue)
                                    lowestPrice = magicCard;
                            }

                            printings.Add(magicCard);
                        }
                    }

                    // Check if there are more pages
                    if (responseObject.has_more && !string.IsNullOrEmpty(responseObject.next_page))
                    {
                        nextPageUri = responseObject.next_page;
                    }
                    else
                    {
                        nextPageUri = null;
                    }

                    // Non-blocking delay to avoid hitting rate limits
                    await Task.Delay(500).ConfigureAwait(true);
                    if (cardName != comboBox1.Text?.Trim())
                        return; // User changed the card name during the request, abort processing
                }

                if (printings.Count > 0)
                {
                    // Ensure UI update happens on UI thread — ConfigureAwait(true) above ensures continuation on UI thread
                    fastObjectListView1.SelectedObject = null;
                    fastObjectListView1.SetObjects(printings);
                    if (lowestPrice != null)
                        fastObjectListView1.SelectedObject = lowestPrice;
                    fastObjectListView1.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine($"Error fetching printings for {cardName}: {ex.Message}");
            }
        }
        private async void Search()
        {
            var searchText = comboBox1.Text?.Trim();
            if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 2)
                return;

            var endpoint = "https://api.scryfall.com/cards/autocomplete?q=" + Uri.EscapeUriString(searchText);

            try
            {
                // Use RestSharp types fully-qualified to avoid changing file-level usings
                var client = new RestSharp.RestClient();
                var request = new RestSharp.RestRequest(endpoint, RestSharp.Method.Get);
                request.Timeout = TimeSpan.FromMilliseconds(10000);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
                TimeSpan elapsed;
                lock (Globals.States.LastScryfallSearchLockObject)
                {
                    elapsed = DateTime.Now - Globals.States.LastScryfallSearchTime;
                }
                if (elapsed < TimeSpan.FromMilliseconds(100))
                {
                    Thread.Sleep(100);
                }
                lock (Globals.States.LastScryfallSearchLockObject)
                {
                    Globals.States.LastScryfallSearchTime = DateTime.Now;
                }

                var response = await client.ExecuteAsync(request).ConfigureAwait(true);

                if (response == null)
                {
                    MessageBox.Show("No response received from Scryfall.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (response.ErrorException != null)
                {
                    MessageBox.Show($"Network error contacting Scryfall: {response.ErrorMessage ?? response.ErrorException.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300 || string.IsNullOrEmpty(response.Content))
                {
                    MessageBox.Show($"Scryfall returned status {(int)response.StatusCode}.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var json = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                var data = json["data"] as Newtonsoft.Json.Linq.JArray;

                if (data != null && data.Count > 0)
                {
                    var suggestion = data[0]?.ToString();
                    if (string.IsNullOrWhiteSpace(suggestion))
                    {
                        MessageBox.Show("Scryfall returned an empty suggestion.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        comboBox1.Items.Clear();
                        try
                        {
                            var items = data.ToObject<string[]>();
                            if (items != null && items.Length > 0)
                                comboBox1.Items.AddRange(items);
                        }
                        catch (Newtonsoft.Json.JsonException)
                        {
                            // Fallback: add array elements individually if ToObject fails
                            comboBox1.Items.Clear();
                            foreach (var token in data)
                            {
                                var s = token?.ToString();
                                if (!string.IsNullOrEmpty(s))
                                    comboBox1.Items.Add(s);
                            }
                        }
                        comboBox1.DroppedDown = true;
                        ComboBox1_SelectedIndexChangedAsync(comboBox1, EventArgs.Empty);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No suggestions found from Scryfall for the given query.", "Search Results", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (System.Net.WebException ex)
            {
                System.Windows.Forms.MessageBox.Show($"Network error contacting Scryfall: {ex.Message}", "Search Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error parsing response from Scryfall: {ex.Message}", "Search Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Unexpected error performing search: {ex.Message}", "Search Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void fastObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (fastObjectListView1.SelectedObject is ScryfallMagicCard selectedCard)
                CardManager.RetrieveImage(selectedCard, "A");
        }
        internal void DockPanelActiveDocumentChanged(object sender, EventArgs e)
        {
            var activeCollectionForm = Globals.Forms.DockPanel.ActiveDocument as CollectionViewForm;
            if (activeCollectionForm != null)
            {
                var collection = activeCollectionForm.Collection;
                if (collection.Type == "deck")
                {
                    radioButton1.Enabled = radioButton2.Enabled = true;
                    if (activeCollectionForm.LastActiveListView == activeCollectionForm.cardListView)
                        radioButton1.Checked = true;
                    else if (activeCollectionForm.LastActiveListView == activeCollectionForm.sideboardListView)
                        radioButton2.Checked = true;
                }
                else
                {
                    radioButton1.Enabled = radioButton2.Enabled = false;
                    radioButton1.Checked = true;
                }
            }
        }
        private void QuickEntryForm_Shown(object sender, EventArgs e)
        {
            DockPanelActiveDocumentChanged(sender, e);
        }
        private void QuickEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (!comboBox1.DroppedDown)
                {
                    if (comboBox1.Text != lastPrintingsCardName)
                        Search();
                    else
                    {
                        var activeCollectionForm = Globals.Forms.DockPanel.ActiveDocument as CollectionViewForm;
                        if (activeCollectionForm != null)
                        {
                            var activeCollection = activeCollectionForm.Collection;
                            if (activeCollection != null && fastObjectListView1.SelectedObject is ScryfallMagicCard card)
                            {
                                if (card.ScryfallId == lastAddedCard?.ScryfallId && ((activeCollectionForm.LastAddedCardBoard == "mainboard" && radioButton1.Checked) || (activeCollectionForm.LastAddedCardBoard == "sideboard" && radioButton2.Checked)))
                                {
                                    using (var context = new ScryfallCardsDbContext())
                                    {
                                        var inventoryCard = activeCollectionForm.LastAddedCard;
                                        if (inventoryCard != null)
                                        {
                                            var listView = (radioButton1.Checked) ? activeCollectionForm.cardListView : activeCollectionForm.sideboardListView;
                                            inventoryCard.Count += e.Shift ? 4 : 1;
                                            context.Update(inventoryCard.InventoryCardBase);
                                            context.SaveChanges();
                                            if (activeCollection.CollapsedView.HasValue && activeCollection.CollapsedView.Value)
                                            {
                                                foreach (var item in listView.Objects)
                                                {
                                                    if (item is InventoryCardCluster cluster && cluster.Name == inventoryCard.Name)
                                                    {
                                                        listView.RefreshObject(cluster);
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                                listView.RefreshObject(inventoryCard);
                                        }
                                    }
                                }
                                else
                                {
                                    CardManager.AddMagicCardsToCollection(new List<OLVCardItem> { new OLVCardItem(card) }, activeCollection, radioButton1.Checked ? "mainboard" : "sideboard", e.Shift ? 4 : 1);
                                    lastAddedCard = card;
                                }
                            }
                        }
                    }
                }
                else
                {
                    comboBox1.DroppedDown = false;
                    GetPrintings();
                }
            }
        }
    }
}
