using KW.WinFormsUI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace MTG_Librarian
{
    public class SetDownloadedEventArgs : EventArgs
    {
        public string SetCode { get; set; }
    }

    public class CardsDroppedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
        public CardCollection TargetCollection { get; set; }
        public DockContent SourceForm { get; set; }
        public string SourceBoard { get; set; }
        public string TargetBoard { get; set; } = "mainboard";
    }

    public class InventoryChangedEventArgs : EventArgs
    {
        public List<FullInventoryCard> Cards { get; set; }
    }

    public class CardsUpdatedFromScryfallEventArgs : EventArgs
    {
        public List<ScryfallMagicCard> Cards { get; set; }
    }
    public class CardsUpdatedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
        public CollectionViewForm CollectionViewForm { get; set; }
        public string Board { get; set; }
    }

    public class CardFocusedEventArgs : EventArgs
    {
        public string ScryfallId { get; set; }
        public string Edition { get; set; }
    }

    public class CardSelectedEventArgs : EventArgs
    {
        public ScryfallMagicCardBase MagicCard { get; set; }
        public IList MagicCards { get; set; }
    }

    public class CardsActivatedEventArgs : EventArgs
    {
        public ArrayList CardItems { get; set; }
    }

    public class CardImageRetrievedEventArgs : EventArgs
    {
        public string uuid { get; set; }
        public int MultiverseId { get; set; }
        public Image CardImage { get; set; }
    }

    public class PricesUpdatedEventArgs : EventArgs
    {
        public Dictionary<int, double?> Prices { get; set; }
    }

    public class ScryfallSearchEndedEventArgs : EventArgs
    {
        public List<ScryfallMagicCard> Results { get; set; }
        public int TotalCards { get; set; }
        public bool Waiting { get; set; } = false;
        public string Query { get; set; }
    }

    public class CardResourceArgs : BasicCardArgs
    {
        public byte[] Data { get; set; }
        public BasicCardArgs BasicCardArgs { set { uuid = value.uuid; Side = value.Side; MultiverseId = value.MultiverseId; Edition = value.Edition; } }
    }

    public class BasicCardArgs
    {
        public string uuid { get; set; }
        public string Side { get; set; } = "A";
        public int MultiverseId { get; set; }
        public string Edition { get; set; }
    }
    public class RulingsDownloadedEventArgs : EventArgs
    {
        public ScryfallMagicCardBase Card { get; set; }
    }
    public class PrintingsDownloadedEventArgs : EventArgs
    {
        public ScryfallMagicCardBase Card { get; set; }
    }
}