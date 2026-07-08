using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MTG_Librarian.LiveMagicCard;

namespace MTG_Librarian
{
    public partial class SimulatorForm : Form
    {
        private bool handDrawn = false;
        private List<LiveMagicCard> cardHand = new List<LiveMagicCard>();
        private List<LiveMagicCard> lands = new List<LiveMagicCard>();
        private List<LiveMagicCard> battlefield = new List<LiveMagicCard>();
        private List<LiveMagicCard> graveyard = new List<LiveMagicCard>();
        private List<LiveMagicCard> exile = new List<LiveMagicCard>();
        private List<ScryfallMagicCardBase> mainboard;
        private PictureBox cardZoomPictureBox = new PictureBox();
        private ZoneSearchForm zoneSearchForm = new ZoneSearchForm();
        private int Mulligans = 0;
        private bool handKept = false;

        public List<ScryfallMagicCardBase> Mainboard 
        { 
            get { return mainboard; } 
            set 
            {
                mainboard = value;
                cardLibrary = new CardLibrary(mainboard);
                foreach (var card in cardLibrary.GetLibrary())
                    SetLiveCardEvents(card);
            } 
        }
        private CardLibrary cardLibrary;
        public SimulatorForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            var img = new Bitmap(Properties.Resources.Magic_card_back);
            libraryPictureBox.Image = img.ScaleImage(libraryPictureBox.Width, libraryPictureBox.Height);
        }

        private void UpdatePictureBoxImage(PictureBox pictureBox, ScryfallMagicCardBase card)
        {
            if (card == null)
            {
                pictureBox.Image = null;
                return;
            }
            pictureBox.Image = CardImageCache.GetScaledImage(card.ScryfallId, card.set_name, pictureBox.Width, pictureBox.Height);
        }
        private void DiscardButton_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)((Button)sender).Parent;
            MoveToGraveyard(liveCard);
        }
        private void ArrangeCardsInZone(GameZone zone)
        {
            switch (zone)
            {
                case GameZone.Hand:
                    ArrangeCardsInPanel(cardHand, handPanel);
                    break;
                case GameZone.Battlefield:
                    ArrangeCardsInPanel(battlefield, battlefieldPanel);
                    ArrangeCardsInPanel(lands, landPanel);
                    break;
                case GameZone.Graveyard:
                    // Graveyard is represented by a single picture box, no arrangement needed
                    break;
                case GameZone.Exile:
                    // Exile is represented by a single picture box, no arrangement needed
                    break;
            }
        }
        private void ArrangeCardsInPanel(List<LiveMagicCard> cards, Panel panel)
        {
            int index = 0;
            int totalWidth = 0;
            foreach (var card in cards)
            {
                
                card.Location = new Point(totalWidth + 10, 0);
                totalWidth += card.Width + 10;
                index++;
            }
        }
        private void MoveToBattlefield(LiveMagicCard liveCard)
        {
            var zone = liveCard.Zone;
            var card = liveCard.GetCard();
            if (card == null)
                return;

            bool isLand = card.type_line.Contains("Land");
            var targetList = isLand ? lands : battlefield;
            var targetPanel = isLand ? landPanel : battlefieldPanel;

            bool addedToExistingStack = false;
            int insertionIndex = -1;

            foreach (var existingCard in targetList)
            {
                if (existingCard.CardName == card.Name)
                {
                    if (existingCard.CanStack)
                    {
                        existingCard.AddCard(card);
                        addedToExistingStack = true;
                        break;
                    }
                    insertionIndex = targetList.IndexOf(existingCard);
                    break;
                }
            }

            if (!addedToExistingStack)
            {
                liveCard.Zone = LiveMagicCard.GameZone.Battlefield;
                if (insertionIndex > -1)
                    targetList.Insert(insertionIndex, liveCard);
                else
                    targetList.Add(liveCard);
                targetPanel.Controls.Add(liveCard);
            }

            RemoveFromZone(liveCard, zone);
            ArrangeCardsInZone(GameZone.Battlefield);
            ArrangeCardsInZone(zone);
        }
        private void MoveToHand(LiveMagicCard liveCard)
        {
            var zone = liveCard.Zone;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveHandCard = new LiveMagicCard(card);
                    SetLiveCardEvents(liveHandCard);
                    liveHandCard.Zone = LiveMagicCard.GameZone.Hand;
                    cardHand.Add(liveHandCard);
                    handPanel.Controls.Add(liveHandCard);
                }
            }
            else
            {
                var card = liveCard.GetCard();
                if (card != null)
                {
                    liveCard.Zone = LiveMagicCard.GameZone.Hand;
                    liveCard.ShowButtons();
                    cardHand.Add(liveCard);
                    handPanel.Controls.Add(liveCard);
                    RemoveFromZone(liveCard, zone);
                }
            }
            ArrangeCardsInZone(GameZone.Hand);
            ArrangeCardsInZone(zone);
        }
        private void MoveToExile(LiveMagicCard liveCard)
        {
            var zone = liveCard.Zone;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveExileCard = new LiveMagicCard(card);
                    SetLiveCardEvents(liveExileCard);
                    liveExileCard.Zone = LiveMagicCard.GameZone.Exile;
                    exile.Add(liveExileCard);
                    UpdatePictureBoxImage(exilePictureBox, card);
                }
            }
            else
            {
                var card = liveCard.GetCard();
                if (card != null)
                {
                    liveCard.Zone = LiveMagicCard.GameZone.Exile;
                    exile.Add(liveCard);
                    RemoveFromZone(liveCard, zone);
                    UpdatePictureBoxImage(exilePictureBox, card);
                }
            }
        }
        private void RemoveFromZone(LiveMagicCard livecard, GameZone zone)
        {
            switch (zone)
            {
                case GameZone.Hand:
                    livecard.HideButtons();
                    cardHand.Remove(livecard);
                    handPanel.Controls.Remove(livecard);
                    break;
                case GameZone.Battlefield:
                    livecard.ClearCounters();
                    livecard.Tapped = false;
                    if (livecard.type_line.Contains("Land"))
                    {
                        lands.Remove(livecard);
                        landPanel.Controls.Remove(livecard);
                    }
                    else
                    {
                        battlefield.Remove(livecard);
                        battlefieldPanel.Controls.Remove(livecard);
                    }
                    break;
                case GameZone.Graveyard:
                    graveyard.Remove(livecard);
                    zoneSearchForm.cardsPanel.Controls.Remove(livecard);
                    UpdatePictureBoxImage(graveyardPictureBox, graveyard.LastOrDefault()?.GetCard());
                    break;
                case GameZone.Exile:
                    exile.Remove(livecard);
                    zoneSearchForm.cardsPanel.Controls.Remove(livecard);
                    UpdatePictureBoxImage(exilePictureBox, exile.LastOrDefault()?.GetCard());
                    break;
                case GameZone.Library:
                    cardLibrary.Remove(livecard);
                    CheckEmptyLibrary();
                    zoneSearchForm.cardsPanel.Controls.Remove(livecard);
                    break;
            }
        }
        private void CheckEmptyLibrary()
        {
            if (!handKept) return;

            if (cardLibrary.IsEmpty())
            {
                drawButton.Enabled = false;
                libraryPictureBox.Image = null;
            }
            else
            {
                drawButton.Enabled = true;
                libraryPictureBox.Image = new Bitmap(Properties.Resources.Magic_card_back).ScaleImage(libraryPictureBox.Width, libraryPictureBox.Height);
            }
        }
        private void MoveToGraveyard(LiveMagicCard liveCard)
        {
            var zone = liveCard.Zone;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveGraveyardCard = new LiveMagicCard(card);
                    SetLiveCardEvents(liveGraveyardCard);
                    liveGraveyardCard.Zone = LiveMagicCard.GameZone.Graveyard;
                    graveyard.Add(liveGraveyardCard);
                    UpdatePictureBoxImage(graveyardPictureBox, card);
                }
            }
            else
            {
                var card = liveCard.GetCard();
                if (card != null)
                {
                    liveCard.Zone = LiveMagicCard.GameZone.Graveyard;
                    graveyard.Add(liveCard);
                    RemoveFromZone(liveCard, zone);
                    UpdatePictureBoxImage(graveyardPictureBox, card);
                }
            }
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)((Button)sender).Parent;
            MoveToBattlefield(liveCard);
        }
        private void liveCardMouseUp(object sender, MouseEventArgs args)
        {
            cardZoomPictureBox.Image = null;
            Controls.Remove(cardZoomPictureBox);
        }
        private void liveCardMouseDown(object sender, MouseEventArgs args)
        {
            var liveCard = (LiveMagicCard)((PictureBox)sender).Parent;
            if (args.Button == MouseButtons.Left)
            {
                if (!liveCard.Tapped)
                {
                    cardZoomPictureBox.Width = (int)(liveCard.pictureBox.Width * 1.5);
                    cardZoomPictureBox.Height = (int)(liveCard.pictureBox.Height * 1.5);
                }
                else
                {
                    cardZoomPictureBox.Width = (int)(liveCard.UntappedImage.Width * 1.5);
                    cardZoomPictureBox.Height = (int)(liveCard.UntappedImage.Height * 1.5);
                }
                cardZoomPictureBox.Location = this.PointToClient(liveCard.Parent.PointToScreen(liveCard.Location));
                if (cardZoomPictureBox.Top + cardZoomPictureBox.Height > this.Height)
                    cardZoomPictureBox.Top = this.Height - cardZoomPictureBox.Height - 30;
                
                var card = liveCard.GetCard();
                cardZoomPictureBox.Image = CardImageCache.GetScaledImage(card.ScryfallId, card.set_name, cardZoomPictureBox.Width, cardZoomPictureBox.Height);
                Controls.Add(cardZoomPictureBox);
                cardZoomPictureBox.BringToFront();
            }
        }
        void SetLiveCardEvents(LiveMagicCard card)
        {
            card.playButton.Click += PlayButton_Click;
            card.discardButton.Click += DiscardButton_Click;
            card.pictureBox.MouseDown += liveCardMouseDown;
            card.pictureBox.MouseUp += liveCardMouseUp;
            card.ContextMenuStrip = liveCardMenuStrip;
        }
        private void drawButton_Click(object sender, EventArgs e)
        {
            if (!cardLibrary.IsEmpty())
            {
                var cardsAdded = new List<LiveMagicCard>();
                if (!handDrawn)
                {
                    var hand = cardLibrary.DrawHand();
                    foreach (var card in hand)
                    {
                        card.ContextMenuStrip = null;
                        card.HideButtons();
                        card.Zone = LiveMagicCard.GameZone.Hand;
                        cardHand.Add(card);
                        cardsAdded.Add(card);
                    }
                    handDrawn = true;
                    mulliganButton.Enabled = true;
                    keepHandButton.Enabled = true;
                    drawButton.Enabled = false;
                }
                else
                {
                    var card = cardLibrary.Draw();
                    card.ShowButtons();
                    card.Zone = LiveMagicCard.GameZone.Hand;
                    cardHand.Add(card);
                    cardsAdded.Add(card);
                }

                CheckEmptyLibrary();
                handPanel.Controls.AddRange(cardsAdded.ToArray());
                ArrangeCardsInZone(GameZone.Hand);
            }
        }

        private void SimulatorForm_Resize(object sender, EventArgs e)
        {
            handPanel.Width = this.Width - libraryPictureBox.Width - 30;
            landPanel.Width = handPanel.Width;
            battlefieldPanel.Width = handPanel.Width;
        }

        private void moveToGraveyardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            MoveToGraveyard(liveCard);
            if (zoneSearchForm.Visible)
                zoneSearchForm.RemoveCard(liveCard);
        }

        private void moveToExileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            MoveToExile(liveCard);
            if (zoneSearchForm.Visible)
                zoneSearchForm.RemoveCard(liveCard);
        }

        private void zoneMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var source = (PictureBox)zoneMenuStrip.SourceControl;
            if (source == libraryPictureBox)
            {
                searchZoneToolStripMenuItem.Text = "Search Library";
            }
            else if (source == graveyardPictureBox)
            {
                searchZoneToolStripMenuItem.Text = "Search Graveyard";
            }
            else if (source == exilePictureBox)
            {
                searchZoneToolStripMenuItem.Text = "Search Exile";
            }
        }
        private void searchZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var source = (PictureBox)zoneMenuStrip.SourceControl;
            List<LiveMagicCard> zoneCards = null;
            if (source == libraryPictureBox)
            {
                zoneCards = cardLibrary.GetLibrary();
            }
            else if (source == graveyardPictureBox)
            {
                zoneCards = graveyard;
            }
            else if (source == exilePictureBox)
            {
                zoneCards = exile;
            }

            if (zoneCards != null && zoneCards.Count > 0)
            {
                zoneSearchForm.Cards = zoneCards;
                zoneSearchForm.Width = this.Width;
                zoneSearchForm.ShowDialog();
                if (source == libraryPictureBox)
                    cardLibrary.Reshuffle();
            }
        }
        private void moveToHandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            MoveToHand(liveCard);
            if (zoneSearchForm.Visible)
                zoneSearchForm.RemoveCard(liveCard);
        }
        private void moveToBattlefieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            MoveToBattlefield(liveCard);
            if (zoneSearchForm.Visible)
                zoneSearchForm.RemoveCard(liveCard);
        }
        private void putOnTopOfLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            var zone = liveCard.Zone;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveLibraryCard = new LiveMagicCard(card);
                    SetLiveCardEvents(liveLibraryCard);
                    liveLibraryCard.Zone = LiveMagicCard.GameZone.Library;
                    cardLibrary.PlaceCardOnTop(liveLibraryCard);
                }
            }
            else
            {
                liveCard.Zone = LiveMagicCard.GameZone.Library;
                RemoveFromZone(liveCard, zone);
                cardLibrary.PlaceCardOnTop(liveCard);
            }
            if (zoneSearchForm.Visible)
                zoneSearchForm.RemoveCard(liveCard);

            CheckEmptyLibrary();
            ArrangeCardsInZone(zone);
        }
        private void PutOnBottomOfLibrary(LiveMagicCard liveCard)
        {
            var zone = liveCard.Zone;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveLibraryCard = new LiveMagicCard(card);
                    SetLiveCardEvents(liveLibraryCard);
                    liveLibraryCard.Zone = LiveMagicCard.GameZone.Library;
                    cardLibrary.PlaceCardOnBottom(liveLibraryCard);
                }
            }
            else
            {
                liveCard.Zone = LiveMagicCard.GameZone.Library;
                RemoveFromZone(liveCard, zone);
                cardLibrary.PlaceCardOnBottom(liveCard);
            }
            if (zoneSearchForm.Visible)
                zoneSearchForm.RemoveCard(liveCard);
            CheckEmptyLibrary();
            ArrangeCardsInZone(zone);
        }
        private void putOnBottomOfLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            PutOnBottomOfLibrary(liveCard);
            if (Mulligans > 0)
            {
                Mulligans--;
                if (Mulligans == 0)
                {
                    foreach (var card in cardHand)
                    {
                        card.ShowButtons();
                        card.ContextMenuStrip = liveCardMenuStrip;
                    }
                    drawButton.Enabled = true;
                    messageLabel.Text = null;
                }
            }
        }
        private void add11CounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveCardWithCounter = new LiveMagicCard(card);
                    SetLiveCardEvents(liveCardWithCounter);
                    liveCardWithCounter.HideButtons();
                    liveCardWithCounter.Zone = liveCard.Zone;
                    liveCardWithCounter.P1P1Counters = 1;
                    if (liveCard.type_line.Contains("Land"))
                    {
                        int insertionIndex = lands.IndexOf(liveCard);
                        lands.Insert(insertionIndex, liveCardWithCounter);
                        landPanel.Controls.Add(liveCardWithCounter);
                    }
                    else
                    {
                        int insertionIndex = battlefield.IndexOf(liveCard);
                        battlefield.Insert(insertionIndex, liveCardWithCounter);
                        battlefieldPanel.Controls.Add(liveCardWithCounter);
                    }
                }
            }
            else
            {
                liveCard.P1P1Counters++;
            }
            ArrangeCardsInZone(GameZone.Battlefield);
        }
        private void add11CounterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveCardWithCounter = new LiveMagicCard(card);
                    SetLiveCardEvents(liveCardWithCounter);
                    liveCardWithCounter.HideButtons();
                    liveCardWithCounter.Zone = liveCard.Zone;
                    liveCardWithCounter.P1P1Counters = -1;
                    if (liveCard.type_line.Contains("Land"))
                    {
                        int insertionIndex = lands.IndexOf(liveCard);
                        lands.Insert(insertionIndex, liveCardWithCounter);
                        landPanel.Controls.Add(liveCardWithCounter);
                    }
                    else
                    {
                        int insertionIndex = battlefield.IndexOf(liveCard);
                        battlefield.Insert(insertionIndex, liveCardWithCounter);
                        battlefieldPanel.Controls.Add(liveCardWithCounter);
                    }
                }
            }
            else
            {
                liveCard.P1P1Counters--;
            }
            ArrangeCardsInZone(GameZone.Battlefield);
        }
        private void liveCardMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (Mulligans > 0)
            {
                if (!keepHandButton.Enabled)
                {
                    tapuntapToolStripMenuItem.Enabled = false;
                    moveToBattlefieldToolStripMenuItem.Enabled = false;
                    moveToHandToolStripMenuItem.Enabled = false;
                    moveToGraveyardToolStripMenuItem.Enabled = false;
                    moveToExileToolStripMenuItem.Enabled = false;
                    putOnTopOfLibraryToolStripMenuItem.Enabled = false;
                    putOnBottomOfLibraryToolStripMenuItem.Enabled = true;
                    add11CounterToolStripMenuItem.Enabled = false;
                    add11CounterToolStripMenuItem1.Enabled = false;
                    addCounterToolStripMenuItem.Enabled = false;
                    removeCounterToolStripMenuItem.Enabled = false;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
                switch (liveCard.Zone)
                {
                    case GameZone.Battlefield:
                        tapuntapToolStripMenuItem.Enabled = true;
                        moveToBattlefieldToolStripMenuItem.Enabled = false;
                        moveToHandToolStripMenuItem.Enabled = true;
                        moveToGraveyardToolStripMenuItem.Enabled = true;
                        moveToExileToolStripMenuItem.Enabled = true;
                        putOnTopOfLibraryToolStripMenuItem.Enabled = true;
                        putOnBottomOfLibraryToolStripMenuItem.Enabled = true;
                        add11CounterToolStripMenuItem.Enabled = true;
                        add11CounterToolStripMenuItem1.Enabled = true;
                        addCounterToolStripMenuItem.Enabled = true;
                        removeCounterToolStripMenuItem.Enabled = true;
                        break;
                    case GameZone.Hand:
                        tapuntapToolStripMenuItem.Enabled = false;
                        moveToHandToolStripMenuItem.Enabled = false;
                        moveToBattlefieldToolStripMenuItem.Enabled = true;
                        moveToGraveyardToolStripMenuItem.Enabled = true;
                        moveToExileToolStripMenuItem.Enabled = true;
                        putOnTopOfLibraryToolStripMenuItem.Enabled = true;
                        putOnBottomOfLibraryToolStripMenuItem.Enabled = true;
                        add11CounterToolStripMenuItem.Enabled = false;
                        add11CounterToolStripMenuItem1.Enabled = false;
                        addCounterToolStripMenuItem.Enabled = false;
                        removeCounterToolStripMenuItem.Enabled = false;
                        break;
                    case GameZone.Graveyard:
                        tapuntapToolStripMenuItem.Enabled = false;
                        moveToHandToolStripMenuItem.Enabled = true;
                        moveToBattlefieldToolStripMenuItem.Enabled = true;
                        moveToGraveyardToolStripMenuItem.Enabled = false;
                        moveToExileToolStripMenuItem.Enabled = true;
                        putOnTopOfLibraryToolStripMenuItem.Enabled = true;
                        putOnBottomOfLibraryToolStripMenuItem.Enabled = true;
                        add11CounterToolStripMenuItem.Enabled = false;
                        add11CounterToolStripMenuItem1.Enabled = false;
                        addCounterToolStripMenuItem.Enabled = false;
                        removeCounterToolStripMenuItem.Enabled = false;
                        break;
                    case GameZone.Exile:
                        tapuntapToolStripMenuItem.Enabled = false;
                        moveToHandToolStripMenuItem.Enabled = true;
                        moveToBattlefieldToolStripMenuItem.Enabled = true;
                        moveToGraveyardToolStripMenuItem.Enabled = true;
                        moveToExileToolStripMenuItem.Enabled = false;
                        putOnTopOfLibraryToolStripMenuItem.Enabled = true;
                        putOnBottomOfLibraryToolStripMenuItem.Enabled = true;
                        add11CounterToolStripMenuItem.Enabled = false;
                        add11CounterToolStripMenuItem1.Enabled = false;
                        addCounterToolStripMenuItem.Enabled = false;
                        removeCounterToolStripMenuItem.Enabled = false;
                        break;
                    case GameZone.Library:
                        tapuntapToolStripMenuItem.Enabled = false;
                        moveToHandToolStripMenuItem.Enabled = true;
                        moveToBattlefieldToolStripMenuItem.Enabled = true;
                        moveToGraveyardToolStripMenuItem.Enabled = true;
                        moveToExileToolStripMenuItem.Enabled = true;
                        putOnTopOfLibraryToolStripMenuItem.Enabled = false;
                        putOnBottomOfLibraryToolStripMenuItem.Enabled = false;
                        add11CounterToolStripMenuItem.Enabled = false;
                        add11CounterToolStripMenuItem1.Enabled = false;
                        addCounterToolStripMenuItem.Enabled = false;
                        removeCounterToolStripMenuItem.Enabled = false;
                        break;
                }
            }
        }

        private void addCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveCardWithCounter = new LiveMagicCard(card);
                    SetLiveCardEvents(liveCardWithCounter);
                    liveCardWithCounter.HideButtons();
                    liveCardWithCounter.Zone = liveCard.Zone;
                    liveCardWithCounter.Counters = 1;
                    if (liveCard.type_line.Contains("Land"))
                    {
                        int insertionIndex = lands.IndexOf(liveCard);
                        lands.Insert(insertionIndex, liveCardWithCounter);
                        landPanel.Controls.Add(liveCardWithCounter);
                    }
                    else
                    {
                        int insertionIndex = battlefield.IndexOf(liveCard);
                        battlefield.Insert(insertionIndex, liveCardWithCounter);
                        battlefieldPanel.Controls.Add(liveCardWithCounter);
                    }
                }
            }
            else
            {
                liveCard.Counters++;
            }
        }

        private void removeCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var livecard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            if (livecard.Count > 1)
            {
                var card = livecard.RemoveCard();
                if (card != null)
                {
                    var liveCardWithCounter = new LiveMagicCard(card);
                    SetLiveCardEvents(liveCardWithCounter);
                    liveCardWithCounter.HideButtons();
                    liveCardWithCounter.Zone = livecard.Zone;
                    liveCardWithCounter.Counters = -1;
                    if (livecard.type_line.Contains("Land"))
                    {
                        int insertionIndex = lands.IndexOf(livecard);
                        lands.Insert(insertionIndex, liveCardWithCounter);
                        landPanel.Controls.Add(liveCardWithCounter);
                    }
                    else
                    {
                        int insertionIndex = battlefield.IndexOf(livecard);
                        battlefield.Insert(insertionIndex, liveCardWithCounter);
                        battlefieldPanel.Controls.Add(liveCardWithCounter);
                    }
                }
            }
            else
            {
                livecard.Counters--;
            }
        }

        private void tapuntapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var liveCard = (LiveMagicCard)liveCardMenuStrip.SourceControl;
            var zone = liveCard.Zone;
            if (liveCard.Count > 1)
            {
                var card = liveCard.RemoveCard();
                if (card != null)
                {
                    var liveCardWithTappedState = new LiveMagicCard(card);
                    SetLiveCardEvents(liveCardWithTappedState);
                    liveCardWithTappedState.HideButtons();
                    liveCardWithTappedState.Zone = liveCard.Zone;
                    liveCardWithTappedState.Tapped = true;
                    if (liveCard.type_line.Contains("Land"))
                    {
                        int insertionIndex = lands.IndexOf(liveCard);
                        lands.Insert(insertionIndex, liveCardWithTappedState);
                        landPanel.Controls.Add(liveCardWithTappedState);
                    }
                    else
                    {
                        int insertionIndex = battlefield.IndexOf(liveCard);
                        battlefield.Insert(insertionIndex, liveCardWithTappedState);
                        battlefieldPanel.Controls.Add(liveCardWithTappedState);
                    }
                }
            }
            else
                liveCard.Tapped = !liveCard.Tapped;
            ArrangeCardsInZone(zone);
        }
        private void mulliganButton_Click(object sender, EventArgs e)
        {
            while (cardHand.Count > 0)
            {
                var liveCard = cardHand[0];
                cardHand.RemoveAt(0);
                PutOnBottomOfLibrary(liveCard);
            }
            cardLibrary.Reshuffle();
            cardLibrary.DrawHand().ForEach(card =>
            {
                card.ContextMenuStrip = null;
                card.HideButtons();
                card.Zone = LiveMagicCard.GameZone.Hand;
                cardHand.Add(card);
                handPanel.Controls.Add(card);
            });
            Mulligans++;
            ArrangeCardsInZone(GameZone.Hand);
        }
        private void keepHandButton_Click(object sender, EventArgs e)
        {
            mulliganButton.Enabled = false;
            keepHandButton.Enabled = false;
            handKept = true;
            foreach (var card in cardHand)
            {
                card.ContextMenuStrip = liveCardMenuStrip;
                if (Mulligans == 0)
                    card.ShowButtons();
            }

            if (Mulligans > 0)
                messageLabel.Text = $"Select {Mulligans} card(s) to put on the bottom of your library.";
            else
                drawButton.Enabled = true;
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisposeAllCards();
            cardHand.Clear();
            lands.Clear();
            battlefield.Clear();
            graveyard.Clear();
            exile.Clear();
            cardLibrary.Shuffle();
            handDrawn = false;
            handKept = false;
            Mulligans = 0;
            drawButton.Enabled = true;
            mulliganButton.Enabled = false;
            keepHandButton.Enabled = false;
            messageLabel.Text = null;
            foreach (var card in cardLibrary.GetLibrary())
                SetLiveCardEvents(card);
        }

        private void SimulatorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisposeAllCards();
        }         

        private void DisposeAllCards()
        {
            handPanel.Controls.Clear();
            landPanel.Controls.Clear();
            battlefieldPanel.Controls.Clear();
            foreach (var oldCard in cardHand)
                oldCard.Dispose();
            foreach (var oldCard in lands)
                oldCard.Dispose();
            foreach (var oldCard in battlefield)
                oldCard.Dispose();
            foreach (var oldCard in graveyard)
                oldCard.Dispose();
            foreach (var oldCard in exile)
                oldCard.Dispose();
            foreach (var oldCard in cardLibrary.GetLibrary())
                oldCard.Dispose();
        }

        private void SimulatorForm_Load(object sender, EventArgs e)
        {
        }
    }
    public static class CardImageCache
    {
        private static Dictionary<string, Image> cache = new Dictionary<string, Image>();
        private static Dictionary<string, Image> scaledImageCache = new Dictionary<string, Image>();

        public static Image GetScaledImage(string scryfallId, string setName, int width, int height)
        {
            string key = $"{setName}_{scryfallId}_{width}_{height}";
            if (scaledImageCache.ContainsKey(key))
            {
                return scaledImageCache[key];
            }
            else
            {
                var originalImage = GetImage(scryfallId, setName);
                if (originalImage != null)
                {
                    var scaledImage = originalImage.ScaleImage(width, height);
                    scaledImageCache[key] = scaledImage;
                    return scaledImage;
                }
                return null;
            }
        }
        public static Image GetImage(string scryfallId, string setName)
        {
            string key = $"{setName}_{scryfallId}";
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }
            else
            {
                using (var context = new CardImagesDbContext(setName))
                {
                    var image = context.CardImages.FirstOrDefault(x => x.ScryfallId == scryfallId);
                    if (image != null)
                    {
                        using (var ms = new System.IO.MemoryStream(image.CardImageBytes))
                        {
                            var img = Image.FromStream(ms);
                            cache[key] = img;
                            return img;
                        }
                    }
                }
                return null;
            }
        }
    }

}
