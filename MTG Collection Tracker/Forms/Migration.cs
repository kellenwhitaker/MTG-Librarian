using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MTG_Librarian
{
    public partial class MainForm : Form
    {
        private void AddPrices()
        {
            XElement root = XElement.Load("TCG_Player__Medium_.xml");
            var results = root.Element("list").Elements("mc");
            using (var context = new MyDbContext())
            {
                foreach (var result in results)
                {
                    int id = Convert.ToInt32(result.Element("id").Value);
                    double price = Convert.ToDouble(result.Element("dbprice").Value);
                    var card = from c in context.Catalog
                               where c.multiverseId == id
                               select c;
                    if (card != null && card.Count() > 0)
                        card.First().OnlinePrice = price;
                }
                context.SaveChanges();
            }
        }
        /*
        private void AddLibraryCards()
        {
        }
        */
    }
}
