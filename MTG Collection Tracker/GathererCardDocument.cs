using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTG_Collection_Tracker
{
    public class GathererCardDocument
    {
        public string DocUrl { get; private set; }
        public MCard MCard => MCards[0];
        public List<MCard> MCards { get; private set; }
        public string SetCode { get; private set; }
        public int Id { get => MCards[0].MVid; private set => MCards[0].MVid = value; }
        public string CardName => MCards[0].Name;
        public string ManaCost => MCards[0].ManaCost;
        public string CMC { get; private set; }
        public string Types => MCards[0].Type;
        public string OracleText => MCards[0].OracleText;
        public string PrintedText
        {
            get
            {
                if (MCards[0].Text == null) GetPrintedText();
                return MCards[0].Text;
            }
        }
        public string FlavorText => MCards[0].FlavorText;
        public string Rarity => MCards[0].Rarity;
        public string CollectorNumber => MCards[0].ColNumber;
        public string PT => $"{MCards[0].Power} / {MCards[0].Toughness}";
        public string Artist => MCards[0].Artist;
        public double Rating => MCards[0].Rating;
        public List<(string, string)> Legalities { get; private set; }

        public GathererCardDocument(int id)
        {
            MCards = new List<MCard>();
            DocUrl = "http://gatherer.wizards.com/Pages/Card/Details.aspx?printed=false&multiverseid=" + id;
            var web = new HtmlWeb { PreRequest = x => { x.Timeout = 15000; return true; } };
            HtmlDocument doc = null;
            while (doc == null)
                try { doc = web.Load(DocUrl); }
                catch (System.Net.WebException ex) { if (ex.HResult == -2146233079) continue; throw ex; };
            var detailsTables = doc.DocumentNode
                                .Descendants("table")
                                .Where(x => x.HasClass("cardDetails"));

            MCard mCard;
            bool hasText = false;
            for (int i = 0; i < detailsTables.Count(); i++)
            {
                mCard = new MCard { MVid = id };
                mCard.Part = ((char)(65 + i)).ToString();
                MCards.Add(mCard);
                var details = detailsTables.ElementAt(i);
                MCards[i].Name = ValueFromGathererCardDetails(details, "nameRow");
                if (Int32.TryParse(ValueFromGathererCardDetails(details, "cmcRow"), out int cmc))
                    MCards[i].CMC =  cmc;
                MCards[i].Type = ValueFromGathererCardDetails(details, "typeRow");
                MCards[i].ColNumber = new string(ValueFromGathererCardDetails(details, "numberRow").Where(x => char.IsDigit(x)).ToArray());
                var pt = ValueFromGathererCardDetails(details, "ptRow").Split('/');
                if (pt.Count() > 1)
                {
                    MCards[i].Power = pt[0].Trim();
                    MCards[i].Toughness = pt[1].Trim();
                }
                MCards[i].ManaCost = GetManaCost(details);
                MCards[i].OracleText = GetCardText(details);
                MCards[i].FlavorText = GetFlavorText(details);
                MCards[i].Rarity = GetRarity(details);
                MCards[i].Artist = GetArtist(details);
                MCards[i].Rating = GetRating(details);
                MCards[i].ColorIndicator = ValueFromGathererCardDetails(details, "colorIndicatorRow");
                if (MCards[i].OracleText != "")
                    hasText = true;
                if (i == 0)
                    MCards[i]._legalities = GetLegalities(id);
                MCards[i]._rulings = GetRulings(details);
                SetCode = GetSetCode(details);
            }
            MCards[0].QueryName = GetQueryName(doc);           
            GetVariations(doc);
            if (hasText) GetPrintedText();
        }

        private string GetQueryName(HtmlDocument doc)
        {
            string queryName = null;
            var nameTarget = doc.DocumentNode
                             .Descendants("span")
                             .Where(x => x.Id.Contains("subtitleDisplay"))
                             .FirstOrDefault();
            if (nameTarget != null)
            {
                string text = nameTarget.InnerText.Trim();
                if (text.Contains("//"))
                    queryName = text;
                else
                    queryName = MCards[0].Name;
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
                         .Where(x => x.HasClass("variations"));

            if (vardiv != null && vardiv.Count() > 0)
            {
                Variations = new List<int>();
                var links = vardiv
                            .First()
                            .Descendants("a");
                foreach (var link in links)
                {
                    string href = link.Attributes["href"].Value;
                    int id = Convert.ToInt32(href.Substring(href.IndexOf("=") + 1));
                    if (id != MCards[0].MVid)
                        Variations.Add(id);
                }
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
            string manaCost = manaCostTarget.Count() > 0 ? GetManaCostFromGathererImg(manaCostTarget) : "";
            return manaCost;
        }

        private HtmlNode AddIconNotation(HtmlNode node)
        {
            foreach (var img in node.Descendants("img"))
            {
                string alt = img.Attributes["alt"].Value;
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
                    char firstColor = firstWord == "Blue" ? 'U' : firstWord[0];
                    string thirdWord = words[2];
                    char secondColor = thirdWord == "Blue" ? 'U' : thirdWord[0];
                    notation += String.Format("{{{0}/{1}}}", firstColor, secondColor);
                }
                else if (alt.Contains("Phyrexian"))
                {
                    string[] words = alt.Split(' ');
                    char phyColor = words[1] == "Blue" ? 'U' : words[1][0];
                    notation += String.Format("{{{0}P}}", phyColor);
                }
                else if (alt.Contains("Variable"))
                    notation += "{X}";
                else if (alt.Contains("Colorless"))
                    notation += "{C}";
                else
                    notation += String.Format("{{{0}}}", alt[0]);

                var newNode = HtmlNode.CreateNode(notation);
                img.AppendChild(newNode);
            }
            return node;
        }

        private string GetManaCostFromGathererImg(IEnumerable<HtmlNode> nodes)
        {
            string manaCost = "";
            foreach (var node in nodes)
            {
                string colorValue = node.Attributes["alt"].Value;
                if (colorValue == "Blue")
                    manaCost += "{U}";
                else if (colorValue.Contains(" or "))
                {
                    string[] words = colorValue.Split(' ');
                    string firstWord = words[0];
                    char firstColor = firstWord == "Blue" ? 'U' : firstWord[0];
                    string thirdWord = words[2];
                    char secondColor = thirdWord == "Blue" ? 'U' : thirdWord[0];
                    manaCost += String.Format("{{{0}/{1}}}", firstColor, secondColor);
                }
                else if (colorValue.Contains("Phyrexian"))
                {
                    string[] words = colorValue.Split(' ');
                    char phyColor = words[1] == "Blue" ? 'U' : words[1][0];
                    manaCost += String.Format("{{{0}P}}", phyColor);
                }
                else if (colorValue.Contains("Variable"))
                    manaCost += "{X}";
                else
                    manaCost += String.Format("{{{0}}}", colorValue[0]);
            }
            return manaCost;
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
}
