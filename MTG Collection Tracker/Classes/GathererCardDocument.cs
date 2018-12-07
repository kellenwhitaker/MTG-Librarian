using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
//TODO make sure everything still works with card variations
//TODO add support for Special rarity
//TODO each card variation should have own entry in OLV list
namespace MTG_Collection_Tracker
{
    /*
    public class GathererCardDocument
    {
        public string DocUrl { get; private set; }
        public MCard MCard => MCards[0];
        public List<MCard> MCards { get; private set; }
        public string SetCode { get; private set; }
        public int Id { get => MCards[0].MVid; private set => MCards[0].MVid = value; }
        public string CardName => MCards[0].name;
        public string ManaCost => MCards[0].manaCost;
        public string CMC { get; private set; }
        public string Types => MCards[0].type;
        public string OracleText => MCards[0].text;
        public string PrintedText
        {
            get
            {
                if (MCards[0].originalText == null) GetPrintedText();
                return MCards[0].Text;
            }
        }
        public string FlavorText => MCards[0].FlavorText;
        public string Rarity => MCards[0].Rarity;
        public string CollectorNumber => MCards[0].ColNumber;
        public string PT => $"{MCards[0].Power} / {MCards[0].Toughness}";
        public string Artist => MCards[0].Artist;
        public double Rating => MCards[0].Rating;
        public int RowIndex { get; set; }
        public List<(string, string)> Legalities { get; private set; }
        public bool Ignore { get; set; } = false; 

        public GathererCardDocument(int id, int rowIndex)
        {
            RowIndex = rowIndex;
            MCards = new List<MCard>();
            DocUrl = "http://gatherer.wizards.com/Pages/Card/Details.aspx?printed=false&multiverseid=" + id;
            var doc = HtmlAgilityPackExtensions.FromURL(DocUrl);
            if (doc == null) throw new Exception("Could not load document.");
            var detailsTables = doc.DocumentNode
                                .Descendants("table")
                                .Where(x => x.HasClass("cardDetails"));

            MCard mCard;
            bool hasText = false;
            for (int i = 0; i < detailsTables.Count(); i++)
            {
                mCard = new MCard { MVid = id };
                MCards.Add(mCard);
                var details = detailsTables.ElementAt(i);
                mCard.Part = ((char)(65 + i)).ToString();
                mCard.Name = GetName(details);
                if (mCard.Part == "A")
                    mCard.QueryName = GetQueryName(doc);
                else if (mCard.Part == "B")
                {
                    if (mCard.Name == GetSubtitle(doc))
                    {
                        Ignore = true;
                        return;
                    }
                }
                (mCard.Power, mCard.Toughness) = GetPowerToughness(details);
                (mCard.ManaCost,         mCard.OracleText,     mCard.FlavorText,       mCard.Rarity,       mCard.Artist,       mCard.Rating,       mCard._rulings) 
                = (GetManaCost(details), GetCardText(details), GetFlavorText(details), GetRarity(details), GetArtist(details), GetRating(details), GetRulings(details));
                (mCard.Type,           mCard.ColorIndicator,       mCard.ColNumber,       mCard.CMC)
                = (GetCardType(details), GetColorIndicator(details), GetColNumber(details), GetCMC(details));

                if (i == 0) mCard._legalities = GetLegalities(id);
                if (mCard.OracleText != "") hasText = true;
                if (mCard.ColNumber == "") mCard.ColNumber = $"V{RowIndex}";
                SetCode = GetSetCode(details);
            }
            GetVariations(doc);
            if (hasText) GetPrintedText();
        }

        private string GetCardType(HtmlNode node) => ValueFromGathererCardDetails(node, "typeRow");
        private string GetName(HtmlNode node) => ValueFromGathererCardDetails(node, "nameRow");
        private string GetColorIndicator(HtmlNode node) => ValueFromGathererCardDetails(node, "colorIndicatorRow");
        private string GetColNumber(HtmlNode node) => new string(ValueFromGathererCardDetails(node, "numberRow").Where(x => char.IsDigit(x)).ToArray());
        private int? GetCMC(HtmlNode node) => Int32.TryParse(ValueFromGathererCardDetails(node, "cmcRow"), out int cmc) ? cmc : (int?)null;
        private (string, string) GetPowerToughness(HtmlNode node)
        {
            var pt = ValueFromGathererCardDetails(node, "ptRow").Split('/');
            return pt.Count() > 1 ? (pt[0].Trim(), pt[1].Trim()) : (null, null);
        }

        private string GetSubtitle(HtmlDocument doc)
        {
            return doc.DocumentNode
                   .Descendants("span")
                   .Where(x => x.Id.Contains("subtitleDisplay"))
                   .FirstOrDefault()?.InnerText?.Trim();
        }

        private string GetQueryName(HtmlDocument doc)
        {
            string queryName = null;
            var nameTarget = GetSubtitle(doc);
            if (nameTarget != null)
            {
                queryName = nameTarget.Contains("//") ? nameTarget : MCards[0].Name;
            }
            return queryName;
        }

        private List<(string, string)> GetRulings(HtmlNode Node)
        {
            List<(string, string)> rulings = null;
            var container = Node
                            .Descendants("div")
                            .Where(x => x.Id.Contains("rulingsContainer"))
                            .FirstOrDefault();
            if (container != null)
            {
                rulings = new List<(string, string)>();
                var rows = container
                           .Descendants("tr")
                           .Where(x => x.HasClass("post"));
                foreach (var row in rows)
                {
                    AddIconNotation(row);
                    var date = row.Descendants("td").First().InnerText.Trim();
                    var ruling = row.Descendants("td").ElementAt(1).InnerText.Trim();
                    rulings.Add((date, ruling));
                }
            }
            return rulings;
        }
        
        public string Edition { get => MCards[0].Edition; set => MCards[0].Edition = value; }
        public List<int> Variations { get; private set; }

        private void GetVariations(HtmlDocument doc)
        {
            var vardiv = doc.DocumentNode
                         .Descendants("div")
                         .Where(x => x.HasClass("variations"))
                         .FirstOrDefault();

            if (vardiv != null)
            {
                Variations = new List<int>();
                var links = vardiv.Descendants("a");
                foreach (var link in links)
                {
                    string href = link.Attributes["href"].Value;
                    int id = Convert.ToInt32(href.Substring(href.IndexOf("=") + 1));
                    if (id != MCards[0].MVid)
                        Variations.Add(id);
                }
                Console.WriteLine($"{MCards[0].Name}: {Variations.Count} variations");
            }                         
        }

        private string GetSetCode(HtmlNode Node)
        {
            string setCode = "";
            string iconSrc = Node
                       .Descendants("img")
                       .Where(x => x.ParentNode.ParentNode.Id.Contains("currentSetSymbol"))
                       .First()
                       .Attributes["src"]
                       .Value;

            int codePIndex = iconSrc.IndexOf("set=");
            if (codePIndex > -1)
            {
                setCode = iconSrc.Substring(codePIndex + 4);
                setCode = setCode.Substring(0, setCode.IndexOf("&amp;"));
            }
            return setCode;
        }

        private double GetRating(HtmlNode Node)
        {
            var ratingTarget = Node
                             .Descendants("span")
                             .Where(x => x.Id.Contains("textRating"))
                             .FirstOrDefault();
            string rating = ratingTarget != null ? ratingTarget.InnerText.Trim() : "5";
            return Convert.ToDouble(rating);
        }

        private string GetArtist(HtmlNode Node)
        {
            var artistTarget = Node
                    .Descendants("a")
                    .Where(x => x.ParentNode.Id.Contains("ArtistCredit"))
                    .FirstOrDefault();
            string artist = artistTarget != null ? artistTarget.InnerText.Trim() : "";
            return artist;
        }

        private string GetRarity(HtmlNode Node)
        {
            var rarityTarget = Node
                             .Descendants("span")
                             .Where(x => x.ParentNode.ParentNode.Id.Contains("rarityRow"))
                             .FirstOrDefault();
            string rarity = rarityTarget != null ? rarityTarget.InnerText.Trim() : "";
            return rarity;
        }

        private string GetFlavorText(HtmlNode Node)
        {
            var flavorTextTarget = Node
                                       .Descendants("div")
                                       .Where(x => x.HasClass("flavortextbox"))
                                       .Where(x => x.ParentNode.Id.Contains("FlavorText"))
                                       .FirstOrDefault();
            string flavorText = flavorTextTarget != null ? flavorTextTarget.InnerText.Trim() : "";
            return flavorText;
        }

        private string GetCardText(HtmlNode Node)
        {
            var cardTextTargets = Node
                                 .Descendants("div")
                                 .Where(x => x.HasClass("cardtextbox"))
                                 .Where(x => x.ParentNode.ParentNode.Id.Contains("textRow"));

            if (cardTextTargets.Count() == 0) return null;
            string cardText = "";
            foreach (var div in cardTextTargets)
                cardText += AddIconNotation(div).InnerText.Trim() + " \n";

            return cardText;
        }

        private string GetManaCost(HtmlNode Node)
        {
            var manaCostTarget = Node
                                .Descendants("img")
                                .Where(x => x.ParentNode.ParentNode.Id.Contains("manaRow"));
            string manaCost = "";
            foreach (var node in manaCostTarget)
                manaCost += GetIconNotation(node);
            
            return manaCost;
        }

        private HtmlNode AddIconNotation(HtmlNode node)
        {
            foreach (var imgNode in node.Descendants("img"))
            {             
                string notation = GetIconNotation(imgNode);
                var newNode = HtmlNode.CreateNode(notation);
                imgNode.AppendChild(newNode);
            }
            return node;
        }

        private string GetIconNotation(HtmlNode imgNode)
        {
            string alt = imgNode.Attributes["alt"].Value;
            string notation = "";
            if (alt == "Blue")
                notation += "{U}";
            else if (alt == "Tap")
                notation += "{T}";
            else if (alt == "Untap")
                notation += "{Q}";
            else if (alt.Contains(" or "))
            {
                string[] words = alt.Split(' ');
                string firstWord = words[0];
                char firstColor = firstWord == "Blue" ? 'U' : firstWord == "Two" ? '2' : firstWord[0];
                string thirdWord = words[2];
                char secondColor = thirdWord == "Blue" ? 'U' : thirdWord == "Two" ? '2' : thirdWord[0];
                notation += String.Format("{{{0}/{1}}}", firstColor, secondColor);
            }
            else if (alt.Contains("Phyrexian"))
            {
                string[] words = alt.Split(' ');
                char phyColor = words.Count() > 1 ? words[1] == "Blue" ? 'U' : words[1][0] : '\0';
                notation += String.Format("{{{0}P}}", phyColor);
            }
            else if (alt.Contains("Variable"))
                notation += "{X}";
            else if (alt.Contains("Colorless"))
                notation += "{C}";
            else
                notation += String.Format("{{{0}}}", alt[0]);
            return notation;
        }

        private string ValueFromGathererCardDetails(HtmlNode Node, string elementID)
        {
            var targetElement = Node
                                .Descendants("div")
                                .Where(x => x.Id.Contains(elementID))
                                .FirstOrDefault();
            return targetElement != null ?
                   targetElement
                   .Descendants("div")
                   .Where(x => x.HasClass("value"))
                   .FirstOrDefault()
                   .InnerText.Trim()
                   : "";
        }

        private List<(string, string)> GetLegalities(int id)
        {
            var URL = $"http://gatherer.wizards.com/Pages/Card/Printings.aspx?multiverseid={id}";
            var web = new HtmlWeb { PreRequest = x => { x.Timeout = 15000; return true; } };
            HtmlDocument doc = null;
            while (doc == null)
                try { doc = web.Load(URL); }
                catch (System.Net.WebException ex) { if (ex.HResult == -2146233079) continue; throw ex; };
            List<(string, string)> legalities = null;
            if (web.StatusCode == System.Net.HttpStatusCode.OK)
            {
                legalities = new List<(string, string)>();
                var legalityRows = doc.DocumentNode
                                    .Descendants("tr")
                                    .Where(x => x.HasClass("cardItem"))
                                    .Where(x => x.Descendants("td").Last().HasClass("conditionTableData"));

                foreach (var row in legalityRows)
                {
                    var format = row.Descendants("td").ElementAt(0).InnerText.Trim();
                    var legality = row.Descendants("td").ElementAt(1).InnerText.Trim();
                    legalities.Add((format, legality));
                }
            }
            return legalities;
        }

        private void GetPrintedText()
        {
            string url = $"http://gatherer.wizards.com/Pages/Card/Details.aspx?printed=true&multiverseid={MCards[0].MVid}";
            var web = new HtmlWeb { PreRequest = x => { x.Timeout = 15000; return true; } };
            HtmlDocument doc = null;
            while (doc == null)
                try { doc = web.Load(url); }
                catch (System.Net.WebException ex) { if (ex.HResult == -2146233079) continue; throw ex; };
            if (web.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var detailsTables = doc.DocumentNode
                                    .Descendants("table")
                                    .Where(x => x.HasClass("cardDetails"));
                for (int i = 0; i < detailsTables.Count(); i++)
                    MCards[i].Text = GetCardText(detailsTables.ElementAt(i));
            }
        }               
    }
    */
}
