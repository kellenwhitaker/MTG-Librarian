using KW.WinFormsUI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//TODO2 make card properties uniform
namespace MTG_Librarian
{
    public class SetDownloadedEventArgs: EventArgs
    {
        public string SetCode { get; set; }
    }

    public class CardsDroppedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
        public CollectionViewForm TargetCollectionViewForm { get; set; }
        public int TargetCollectionId { get; set; } = -1;
        public DockContent SourceForm { get; set; }
    }

    public class CardsUpdatedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
        public CollectionViewForm CollectionViewForm { get; set; }
    }

    public class CardFocusedEventArgs : EventArgs
    {
        public string uuid { get; set; }
        public string Edition { get; set; }
    }

    public class CardSelectedEventArgs : EventArgs
    {
        public MagicCardBase MagicCard { get; set; }
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

    public class CardResourceArgs : BasicCardArgs
    {
        public byte[] Data { get; set; }
        public BasicCardArgs BasicCardArgs { set { uuid = value.uuid; MultiverseId = value.MultiverseId; Edition = value.Edition; } }
    }

    public class BasicCardArgs
    {
        public string uuid { get; set; }
        public int MultiverseId { get; set; }
        public string Edition { get; set; }
    }
}
