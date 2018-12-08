﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//TODO make card properties uniform
namespace MTG_Collection_Tracker
{
    public class CardsDroppedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
    }

    public class CardsUpdatedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
    }

    public class CardSelectedEventArgs : EventArgs
    {
        public int MultiverseId { get; set; }
        public string Edition { get; set; }
    }

    public class CardActivatedEventArgs : EventArgs
    {
        public MagicCard MagicCard { get; set; }
    }

    public class CardImageRetrievedEventArgs : EventArgs
    {
        public int MultiverseId { get; set; }
        public Image CardImage { get; set; }
    }

    public class CardResourceArgs : BasicCardArgs
    {
        public byte[] Data { get; set; }
        public BasicCardArgs BasicCardArgs { set { MultiverseId = value.MultiverseId; Edition = value.Edition; } }
    }

    public class BasicCardArgs
    {
        public int MultiverseId { get; set; }
        public string Edition { get; set; }
    }
}
