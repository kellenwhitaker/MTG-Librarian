using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class Exporter
    {
        private CardCollection Collection;
        public Exporter(CardCollection collection) 
        { 
            Collection = collection;
        }
        public void ExportToCSV(string filePath)
        {
            if (Collection != null && !string.IsNullOrEmpty(filePath))
            {
                bool isDeck = Collection.Type == "deck";
                List<InventoryCard> cards;
                using (var context = new ScryfallCardsDbContext())
                {
                    cards = context.LibraryView
                        .Where(c => c.CollectionId == Collection.Id)
                        .OrderBy(c => c.Board)
                        .ThenBy(c => c.TimeAdded)
                        .ToList();
                }
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
}
