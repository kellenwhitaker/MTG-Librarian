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
    public class MCard : MCardBase
    {
        [Key]
        public int CatalogID { get; set; }
        public int MVid { get; set; }
    }

    public class MCardBase
    {
        public List<(string, string)> _legalities;
        public List<(string, string)> _rulings;
        public string   Part { get; set; }
        public string   Name { get; set; }
        public string   ManaCost { get; set; }
        public string   Type { get; set; }
        public string   Power { get; set; }
        public string   Toughness { get; set; }
        public string   OracleText { get; set; }
        public string   Edition { get; set; }
        public string   Rarity { get; set; }
        public double   Rating { get; set; }
        public string   Artist { get; set; }
        public string   ColNumber { get; set; }
        public string   Text { get; set; }
        public string   FlavorText { get; set; }
        public string   QueryName { get; set; }
        public string   ColorIndicator { get; set; }
        public string   Legalities
        {
            get
            {
                if (_legalities == null) return null;
                return JsonConvert.SerializeObject(_legalities);
            }
            set
            {
                if (value == null) return;
                _legalities = JsonConvert.DeserializeObject(value) as List<(string, string)>;
            }
        }
        public string   Rulings
        {
            get
            {
                if (_rulings == null) return null;
                return JsonConvert.SerializeObject(_rulings);
            }
            set
            {
                if (value == null) return;
                _rulings = JsonConvert.DeserializeObject(value) as List<(string, string)>;
            }
        }
        public int?     CMC { get; set; }
        public double?  OnlinePrice { get; set; }
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

    public class CardInstance : MCardBase
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
        public string       ImageKey => $"{Edition}: {Rarity}";
        [NotMapped]
        public string       PaddedName => Name.PadRight(500);
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
        public DbSet<MCard> Catalog { get; set; }
        public DbSet<DBCardInstance> Library { get; set; }
        public DbSet<CardInstance> LibraryView { get; set; }
        public DbSet<CardSet> Sets { get; set; }
        public DbSet<CardCollection> Collections { get; set; }
        public DbSet<CollectionGroup> CollectionGroups { get; set; }
        public DbSet<CardCollectionItem> CollectionsView { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<MCardBase>();
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
