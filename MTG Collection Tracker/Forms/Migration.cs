using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MTG_Collection_Tracker
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
private void LoadCache()
{
    SQLiteCommand load = new SQLiteCommand("SELECT * FROM gatherer;", GCache);
    using (SQLiteDataReader myreader = load.ExecuteReader())
    {
        while (myreader.Read())
        {
            //dataGridView1.Rows.Add(myreader[0]);
        }
    }

}

private void SetupCache()
{
    GCache = new SQLiteConnection("Data source=:memory:;Version=3;");
    SQLiteConnection GDB = new SQLiteConnection("Data source=cards.db;Version=3;");
    GCache.Open();
    GDB.Open();
    GDB.BackupDatabase(GCache, "main", "main", -1, null, 0);
    GDB.Close();
}
*/       
        private void AddLibraryCards()
        {
            XElement root = XElement.Load("Owned.xml");
            var results = root.Element("list").Elements("mcp");
            var failed = new List<InventoryCard>();
            
            SQLiteConnection conn = new SQLiteConnection("Data Source=cards.db;Version=3;");
            conn.Open();            
            var transaction = conn.BeginTransaction();
            SQLiteCommand addCard = new SQLiteCommand("INSERT INTO library(MVId, Count, Cost, Tags, CollectionName, TimeAdded) VALUES (:id, :count, :cost, :tags, :CollectionName, :TimeAdded)", conn);
            addCard.Parameters.Add("id", DbType.Int32);
            addCard.Parameters.Add("count", DbType.Int32);
            addCard.Parameters.Add("cost", DbType.Double);
            addCard.Parameters.Add("tags", DbType.String);
            addCard.Parameters.Add("CollectionName", DbType.String);
            addCard.Parameters.Add("TimeAdded", DbType.DateTime);
            addCard.Prepare();
            addCard.Parameters["CollectionName"].Value = "Main";
            foreach (XElement mcp in results)
            {
                try
                {
                    var card = mcp.Element("card");
                    addCard.Parameters["id"].Value = card.Element("id").Value;
                    addCard.Parameters["count"].Value = mcp.Element("count").Value;
                    addCard.Parameters["cost"].Value = mcp.Element("price")?.Value;
                    addCard.Parameters["tags"].Value = mcp.Element("special")?.Value;
                    string dateValue = mcp.Element("date")?.Value;
                    if (dateValue != "")
                    {
                        dateValue = dateValue.Substring(3);
                        string year = dateValue.Substring(dateValue.Length - 4);
                        dateValue = dateValue.Remove(dateValue.Length - 4);
                        dateValue = year + dateValue;
                        dateValue = dateValue.Trim().Replace(" ", "-").Replace("Jan", "01").Replace("Feb", "02").Replace("Mar", "03").Replace("Apr", "04").Replace("May", "05").Replace("Jun", "06").Replace("Jul", "07").Replace("Aug", "08").Replace("Sep", "09").Replace("Oct", "10").Replace("Nov", "11").Replace("Dec", "12").Replace("-PDT", " -6").Replace("-PST", " -7");
                        char[] array = dateValue.ToCharArray();
                        if (array[10] == '-')
                            array[10] = ' ';
                        dateValue = new string(array);
                        Console.WriteLine(dateValue);
                        DateTime dt;
                        DateTime.TryParse(dateValue, out dt);
                        addCard.Parameters["TimeAdded"].Value = dt;
                    }
                    else
                        addCard.Parameters["TimeAdded"].Value = DBNull.Value;
                    addCard.ExecuteNonQuery();
                }
                catch { failed.Add(new InventoryCard { CatalogID = Convert.ToInt32(addCard.Parameters["id"].Value) }); };
            }
            transaction.Commit();
            conn.Close();
        }
        /*
        private void UpdateCatalogIDs()
        {
            var cards = new List<(int instanceID, int MVId)>();
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cards.db;Version=3;"))
            {
                conn.Open();
                using (SQLiteCommand getCards = new SQLiteCommand("SELECT * FROM Library;", conn))
                using (SQLiteDataReader reader = getCards.ExecuteReader())
                {
                    while (reader.Read())
                        cards.Add((Convert.ToInt32(reader["CardInstanceId"]), Convert.ToInt32(reader["MVid"])));
                }
                conn.Close();
            }
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cards.db;Version=3;"))
            {
                conn.Open();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    SQLiteCommand updateCatalogID = new SQLiteCommand(@"UPDATE library SET CatalogId=(SELECT CatalogId FROM Catalog WHERE MVid=:Mvid LIMIT 1)
                                                                    WHERE CardInstanceId=:CardInstanceId;", conn);
                    updateCatalogID.Parameters.Add("MVid", DbType.Int32);
                    updateCatalogID.Parameters.Add("CardInstanceId", DbType.Int32);
                    updateCatalogID.Prepare();
                    foreach (var (instanceID, MVId) in cards)
                    {
                        updateCatalogID.Parameters["MVid"].Value = MVId;
                        updateCatalogID.Parameters["CardInstanceId"].Value = instanceID;
                        updateCatalogID.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                conn.Close();
            }        
        }
        */
        /*
        private void AddSets()
        {
            StreamReader sr = new StreamReader("editions.txt");
            string line;
            string[] pars;
            SQLiteConnection conn = new SQLiteConnection("Data Source=cards.db;Version=3;");
            conn.Open();
            var transaction = conn.BeginTransaction();
            SQLiteCommand addSet = new SQLiteCommand("INSERT OR IGNORE INTO sets VALUES (:value1, :value2, :value3, :value4, :value5, :value6, :value7)", conn);
            addSet.Parameters.Add("value1", DbType.String); addSet.Parameters.Add("value2", DbType.String); addSet.Parameters.Add("value3", DbType.String);
            addSet.Parameters.Add("value4", DbType.String); addSet.Parameters.Add("value5", DbType.String); addSet.Parameters.Add("value6", DbType.String);
            addSet.Parameters.AddWithValue("value7", null);
            addSet.Prepare();
            while ((line = sr.ReadLine()) != null)
            {
                pars = line.Split('|');
                for (int i = 0; i < 6; i++)
                    addSet.Parameters[i].Value = pars[i];
                addSet.ExecuteNonQuery();
            }
            transaction.Commit();
            conn.Close();
        }
        */
        /*
        private void FillGatherer()
        {
            var files = Directory.EnumerateFiles(@"C:\Users\User\Google Drive\MAWorkspace\magiccards\MagicDB", "*.xml");
            SQLiteConnection conn = new SQLiteConnection("Data Source=cards.db;Version=3;");
            conn.Open();
            var transaction = conn.BeginTransaction();
            foreach (string f in files)
            {
                FillGathererSet(f, conn);
            }
            transaction.Commit();
            conn.Close();
        }
        */
        /*
        private void FillGathererSet(string fileName, SQLiteConnection conn)
        {
            XElement root = XElement.Load(fileName);
            string name = root.Element("name").Value;
            var cards = root.Element("list").Elements("mc");
            SQLiteCommand addCard = new SQLiteCommand("INSERT OR IGNORE INTO gatherer(id, name, manacost, type, power, toughness, oracletext, edition, rarity, rating, artist, colnumber, text) VALUES (:id, :name, :manacost, :type, :power, :toughness, :oracletext, :edition, :rarity, :rating, :artist, :colnumber, :text)", conn);
            addCard.Parameters.Add("id", DbType.Int32);
            addCard.Parameters.Add("name", DbType.String);
            addCard.Parameters.Add("manacost", DbType.String);
            addCard.Parameters.Add("type", DbType.String);
            addCard.Parameters.Add("power", DbType.String);
            addCard.Parameters.Add("toughness", DbType.String);
            addCard.Parameters.Add("oracletext", DbType.String);
            addCard.Parameters.Add("edition", DbType.String);
            addCard.Parameters.Add("rarity", DbType.String);
            addCard.Parameters.Add("rating", DbType.VarNumeric);
            addCard.Parameters.Add("artist", DbType.String);
            addCard.Parameters.Add("colnumber", DbType.String);
            addCard.Parameters.Add("text", DbType.String);
            addCard.Prepare();
            foreach (var card in cards)
            {
                addCard.Parameters["id"].Value = card.Element("id").Value;
                addCard.Parameters["name"].Value = card.Element("name").Value;
                addCard.Parameters["manacost"].Value = card.Element("cost") != null ? card.Element("cost").Value : null;
                addCard.Parameters["type"].Value = card.Element("type") != null ? card.Element("type").Value : null;
                addCard.Parameters["power"].Value = card.Element("power") != null ? card.Element("power").Value : null;
                addCard.Parameters["toughness"].Value = card.Element("toughness") != null ? card.Element("toughness").Value : null;
                addCard.Parameters["oracletext"].Value = card.Element("oracleText") != null ? card.Element("oracleText").Value : null;
                addCard.Parameters["edition"].Value = card.Element("edition") != null ? card.Element("edition").Value : null;
                addCard.Parameters["rarity"].Value = card.Element("rarity") != null ? card.Element("rarity").Value : null;
                addCard.Parameters["rating"].Value = card.Element("rating") != null ? Convert.ToDouble(card.Element("rating").Value) : 5;
                addCard.Parameters["artist"].Value = card.Element("artist") != null ? card.Element("artist").Value : null;
                addCard.Parameters["colnumber"].Value = card.Element("num") != null ? card.Element("num").Value : null;
                addCard.Parameters["text"].Value = card.Element("text") != null ? card.Element("text").Value : null;
                addCard.ExecuteNonQuery();
            }
        }
        */
    }
}
