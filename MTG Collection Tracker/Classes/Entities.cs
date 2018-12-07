using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace MTG_Collection_Tracker
{
    [Table("Catalog")]
    public class MagicCard : MagicCardBase
    {
        [Key]
        public int CatalogID { get; set; }
        public int multiverseId { get; set; }
    }

    public class MagicCardBase
    {
        public string artist { get; set; }
        public string borderColor { get; set; }
        [NotMapped]
        public string[] colorIdentity { get; set; }
        [NotMapped]
        public string[] colorIndicator { get; set; }
        [NotMapped]
        public string[] colors { get; set; }
        public float convertedManaCost { get; set; }
        public string flavorText { get; set; }
        public ForeignData[] foreignData { get; set; }
        public string frameVersion { get; set; }
        public bool hasFoil { get; set; }
        public bool hasNonFoil { get; set; }
        public bool isFoilOnly { get; set; }
        public bool isOnlineOnly { get; set; }
        public bool isOversized { get; set; }
        public bool isReserved { get; set; }
        public string layout { get; set; }
        public string loyalty { get; set; }
        public string manaCost { get; set; }
        public string name { get; set; }
        [NotMapped]
        public string[] names { get; set; }
        public string number { get; set; }
        public string originalText { get; set; }
        public string originalType { get; set; }
        [NotMapped]
        public string[] printings { get; set; }
        public string power { get; set; }
        public string rarity { get; set; }
        public string SetCode { get; set; }
        public string side { get; set; }
        [NotMapped]
        public string[] subtypes { get; set; }
        [NotMapped]
        public string[] supertypes { get; set; }
        public string text { get; set; }
        public bool timeshifted { get; set; }
        public string toughness { get; set; }
        public string type { get; set; }
        [NotMapped]
        public string[] types { get; set; }
        public string uuid { get; set; }
        public string watermark { get; set; }
        public string Edition { get; set; }
        public Dictionary<string, string> legalities = new Dictionary<string, string>();
        public string   Legalities
        {
            get
            {
                if (legalities == null) return null;
                return JsonConvert.SerializeObject(legalities);
            }
            set
            {
                if (value == null) return;
                legalities = JsonConvert.DeserializeObject(value) as Dictionary<string, string>;
            }
        }
        public Dictionary<string, string>[] rulings;
        public string   Rulings
        {
            get
            {
                if (rulings == null) return null;
                return JsonConvert.SerializeObject(rulings);
            }
            set
            {
                if (value == null) return;
                rulings = JsonConvert.DeserializeObject(value) as Dictionary<string, string>[];
            }
        }
        public double?  OnlinePrice { get; set; }
    }

    public class ForeignData
    {
        [Key]
        public int Id { get; set; }
        public string flavorText { get; set; }
        public string language { get; set; }
        public int multiverseId { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }

    [Table("Library")]
    public class DBCardInstance
    {
        [Key]
        public int         CardInstanceId { get; set; }
        public int?         CatalogID { get; set; }
        public string       CollectionName { get; set; }
        public int          MVid { get; set; }
        public int?         Count { get; set; }
        public double?      Cost { get; set; }
        public string       Tags { get; set; }
        public DateTime?    TimeAdded { get; set; }
        public int?         InsertionIndex { get; set; }
    }

    public class CardInstance : MagicCardBase
    {
        [Key]
        public int          CardInstanceId { get => DBCardInstance.CardInstanceId; set => DBCardInstance.CardInstanceId = value; }
        public int?         CatalogID { get => DBCardInstance.CatalogID; set => DBCardInstance.CatalogID = value; }
        public string       CollectionName { get => DBCardInstance.CollectionName; set => DBCardInstance.CollectionName = value; }
        public int          MVid { get => DBCardInstance.MVid; set => DBCardInstance.MVid = value; }
        public int?         Count { get => DBCardInstance.Count; set => DBCardInstance.Count = value; }
        public double?      Cost { get => DBCardInstance.Cost; set => DBCardInstance.Cost = value; }
        public string       Tags { get => DBCardInstance.Tags; set => DBCardInstance.Tags = value; }
        public DateTime?    TimeAdded { get => DBCardInstance.TimeAdded; set { DBCardInstance.TimeAdded = value; UpdateSortableTimeAdded(); } }
        public int?         InsertionIndex { get => DBCardInstance.InsertionIndex; set { DBCardInstance.InsertionIndex = value; UpdateSortableTimeAdded();  } }
        [NotMapped]
        public String       SortableTimeAdded { get; set; }
                            
        [NotMapped]
        public string       ImageKey => $"{Edition}: {rarity}";
        [NotMapped]
        public string       PaddedName => name.PadRight(500);
        [NotMapped]
        public DBCardInstance DBCardInstance { get; set; } = new DBCardInstance();

        private void UpdateSortableTimeAdded()
        {
            if (TimeAdded.HasValue) SortableTimeAdded = $"{ TimeAdded.Value.ToString("s") } {InsertionIndex.ToString().PadLeft(5) }";
            //else return "";
        }
    }

    [Table("CollectionGroups")]
    public class CollectionGroup
    {
        [Key]
        public string   GroupName { get; set; }
        public bool     Permanent { get; set; }
    }

    [Table("Collections")]
    public class CardCollection
    {
        [Key]
        public string   CollectionName { get; set; }
        public string   GroupName { get; set; }
        public string   Type { get; set; }
        public bool     Virtual { get; set; }
        public bool     Permanent { get; set; }
    }
    
    public class CardCollectionItem
    {
        [Key]
        public int      clID { get; set; }
        public string   CollectionName { get; set; }
        public int?     CardInstanceId { get; set; }        
        public int      MVid { get; set; }
        public int      Count { get; set; }
        public double?  Cost { get; set; }
        public string   Tags { get; set; }
        public string   Type { get; set; }
        public bool     Virtual { get; set; }
    }

    [Table("Sets")]
    public class CardSet
    {
        [Key]
        public string   Name { get; set; }
        public string   Code { get; set; }
        public string   Code2 { get; set; }
        public string   ReleaseDate { get; set; }
        public string   Type { get; set; }
        public string   Block { get; set; }
        [NotMapped]
        public Image    CommonIcon { get; set; }
        public byte[]   CommonIconBytes { get => CommonIcon?.GetCopyOf().ToByteArray(); set => CommonIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image    UncommonIcon { get; set; }
        public byte[]   UncommonIconBytes { get => UncommonIcon?.GetCopyOf().ToByteArray(); set => UncommonIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image    RareIcon { get; set; }
        public byte[]   RareIconBytes { get => RareIcon?.GetCopyOf().ToByteArray(); set => RareIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image    MythicRareIcon { get; set; }
        public byte[]   MythicRareIconBytes { get => MythicRareIcon?.GetCopyOf().ToByteArray(); set => MythicRareIcon = ImageExtensions.FromByteArray(value); }
    }

    public class MyDbContext : DbContext
    {
        #region DbSet
        public DbSet<MagicCard> Catalog { get; set; }
        public DbSet<DBCardInstance> Library { get; set; }
        public DbSet<CardInstance> LibraryView { get; set; }
        public DbSet<CardSet> Sets { get; set; }
        public DbSet<CardCollection> Collections { get; set; }
        public DbSet<CollectionGroup> CollectionGroups { get; set; }
        public DbSet<CardCollectionItem> CollectionsView { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<MagicCardBase>();
            modelBuilder.Entity<DBCardInstance>().Property(b => b.TimeAdded).HasDefaultValueSql("datetime('now','localtime')");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection("Data Source=cards.db;");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }

    public class DbCardImage
    {
        [Key]
        public int MVid { get; set; }
        public byte[] CardImageBytes { get; set; }
    }

    public class CardImagesDbContext : DbContext
    {
        #region DbSet
        public DbSet<DbCardImage> CardImages { get; set; }
        #endregion
        private string Edition;

        public CardImagesDbContext(string Edition)
        {
            this.Edition = Edition.SanitizeFilename();
            DirectoryInfo di = new DirectoryInfo($"Card Images/");           
            if (!di.Exists) di.Create();
            String FileName = $"Card Images/{this.Edition}.db";
            FileInfo fi = new FileInfo(FileName);
            if (!fi.Exists)
            {
                using (SqliteConnection conn = new SqliteConnection($"Data Source={FileName}"))
                using (SqliteCommand createDB = new SqliteCommand("CREATE TABLE CardImages (MVid INTEGER PRIMARY KEY, CardImageBytes BLOB); ", conn))
                {
                    conn.Open();
                    createDB.ExecuteNonQuery();
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection($"Data Source=Card Images/{Edition}.db;");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
}
