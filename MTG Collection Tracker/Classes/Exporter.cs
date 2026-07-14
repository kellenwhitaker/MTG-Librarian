using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class Exporter
    {
        private List<InventoryCard> cards;
        private CardCollection Collection;
        public Exporter(CardCollection collection, List<InventoryCard> cards) 
        { 
            Collection = collection;
            this.cards = cards;
        }
        public void ExportToMTGODek(string filePath)
        {
            var lines = new List<string>();
            lines.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            lines.Add("<Deck xmlns:xsd=\"http://w3.org\" xmlns:xsi=\"http://w3.org-instance\">");
            lines.Add("  <NetDeckID>0</NetDeckID>");

            InventoryCard commander = null;
            if (Collection.Commander.HasValue)
            {
                commander = cards.FirstOrDefault(c => c.InventoryId == Collection.Commander.Value);
                if (commander != null)
                {
                    cards.Remove(commander);
                }
            }
            foreach (var card in cards)
            {
                lines.Add($"  <Cards CatID=\"{card.mtgo_id}\" Quantity=\"{card.Count}\" Sideboard=\"{(card.Board == "sideboard" ? "true" : "false")}\" Name=\"{card.Name}\" Annotation=\"0\"/>");
            }
            if (commander != null)
            {
                lines.Add($"  <Cards CatID=\"{commander.mtgo_id}\" Quantity=\"1\" Sideboard=\"true\" Name=\"{commander.Name}\" Annotation=\"0\"/>");
            }
            lines.Add("</Deck>");
            System.IO.File.WriteAllLines(filePath, lines);
        }
        public void ExportToArenaText(string filePath)
        {
            var lines = new List<string>();
            lines.Add("About");
            lines.Add($"Name {Collection.CollectionName}");
            lines.Add("");
            if (Collection.Commander.HasValue)
            {
                var commander = cards.FirstOrDefault(c => c.InventoryId == Collection.Commander);
                if (commander != null)
                {
                    cards.Remove(commander);
                    lines.Add("Commander");
                    var set = Globals.Methods.ConvertScryfallSetCodeToGatherer(commander.set).ToUpper();
                    lines.Add($"1 {commander.Name} ({set}) {commander.collector_number}");
                    lines.Add("");
                }
            }
            lines.Add("Deck");
            var sideboard = false;
            foreach (var card in cards)
            {
                if (sideboard == false && card.Board == "sideboard")
                {
                    sideboard = true;
                    lines.Add("");
                    lines.Add("Sideboard");
                }
                var set = Globals.Methods.ConvertScryfallSetCodeToGatherer(card.set).ToUpper();
                lines.Add($"{card.Count.Value} {card.Name} ({set}) {card.collector_number}");
            }
            System.IO.File.WriteAllLines(filePath, lines);
        }
        public void ExportToMTGOText(string filePath)
        {
            var lines = new List<string>();
            InventoryCard commander = null;
            var sideboard = false;
            if (Collection.Commander.HasValue)
            {
                commander = cards.FirstOrDefault(c => c.InventoryId == Collection.Commander);
                if (commander != null)
                {
                    cards.Remove(commander);
                }
            }
            foreach (var card in cards)
            {
                if (sideboard == false && card.Board == "sideboard")
                {
                    sideboard = true;
                    lines.Add("");
                }
                lines.Add($"{card.Count.Value} {card.Name}");
            }
            if (Collection.Commander.HasValue)
            {
                if (commander != null)
                {
                    lines.Add("");
                    lines.Add($"1 {commander.Name}");
                }
            }
            System.IO.File.WriteAllLines(filePath, lines);
        }
        public void ExportToCSV(string filePath)
        {
            bool isDeck = Collection.Type == "deck";
            var csvLines = new List<string>();
            csvLines.Add($"Quantity,Name,Code,SetName,CollectorNumber,PurchasePrice,SoldPrice,Finish,Condition,Language,PurchaseDate,SoldDate{(isDeck ? ",Board" : "")}");
            foreach (var card in cards)
            {
                var line = $"{card.Count},\"{card.Name}\",{card.set},\"{card.set_name}\",{card.collector_number},{card.Cost},{card.SoldPrice},{card.Finish},{card.Condition},{card.lang},{card.TimeAdded},{card.SoldTime}{(isDeck ? $",{card.Board}" : "")}";
                csvLines.Add(line);
            }
            System.IO.File.WriteAllLines(filePath, csvLines);
        }
    }
}
