using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public partial class Form1
    {
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
        /*
        private void AddLibraryCards()
        {
            XElement root = XElement.Load("Owned.xml");
            var results = root.Element("list").Elements("mcp");
            SQLiteConnection conn = new SQLiteConnection("Data Source=cards.db;Version=3;");
            conn.Open();            
            var transaction = conn.BeginTransaction();
            SQLiteCommand addCard = new SQLiteCommand("INSERT OR IGNORE INTO library(id, count, cost, tags) VALUES (:id, :count, :cost, :tags)", conn);
            addCard.Parameters.Add("id", DbType.Int32);
            addCard.Parameters.Add("count", DbType.Int32);
            addCard.Parameters.Add("cost", DbType.Double);
            addCard.Parameters.Add("tags", DbType.String);
            addCard.Prepare();
            foreach (XElement mcp in results)
            {
                var card = mcp.Element("card");
                addCard.Parameters["id"].Value = card.Element("id").Value;
                addCard.Parameters["count"].Value = mcp.Element("count").Value;
                addCard.Parameters["cost"].Value = mcp.Element("price") != null ? mcp.Element("price").Value : null;
                addCard.Parameters["tags"].Value = mcp.Element("special") != null ? mcp.Element("special").Value : null;
                addCard.ExecuteNonQuery();               
            }
            transaction.Commit();
            conn.Close();
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
