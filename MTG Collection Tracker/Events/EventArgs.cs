using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//TODO make card properties uniform
namespace MTG_Collection_Tracker
{
    public class CardDroppedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
    }

    public class CardsUpdatedEventArgs : EventArgs
    {
        public ArrayList Items { get; set; }
    }

    public class CardSelectedEventArgs : EventArgs
    {
        public int MVid { get; set; }
        public string Edition { get; set; }
    }

    public class CardActivatedEventArgs : EventArgs
    {
        public MagicCard MCard { get; set; }
    }

    public class CardImageRetrievedEventArgs : EventArgs
    {
        public int MVid { get; set; }
        public Image CardImage { get; set; }
    }

    public class CardResourceArgs : BasicCardArgs
    {
        public byte[] Data { get; set; }
        public BasicCardArgs BasicCardArgs { set { MVid = value.MVid; Edition = value.Edition; } }
    }

    public class BasicCardArgs
    {
        public int MVid { get; set; }
        public string Edition { get; set; }
    }
}
