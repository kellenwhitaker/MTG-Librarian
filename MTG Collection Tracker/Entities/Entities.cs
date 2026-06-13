using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace MTG_Librarian
{
    public class ScryfallMagicCardBase : ScryfallCard
    {
        [NotMapped]
        public string DisplayName => card_faces != null ? (card_faces[0].DisplayName + " // " + card_faces[1].DisplayName) : (printed_name != null ? printed_name : Name);
        [NotMapped]
        public string DisplayTypeLine => card_faces != null ? (card_faces[0].DisplayTypeLine + " // " + card_faces[1].DisplayTypeLine) : (printed_type_line != null ? printed_type_line : type_line);
        [NotMapped]
        public string DisplayText => card_faces != null ? (card_faces[0].DisplayText) : (printed_text != null ? printed_text : oracle_text);
        [NotMapped]
        public string SymbolCode => set != null && set.Length == 4 && (set_type == "token" || set_type == "promo" || set_type == "memorabilia") ? set.Substring(1) : set;
        [NotMapped]
        public ScryfallMagicCard PartB { get; set; }
    }

    public class Metadata
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
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
    public class ScryfallCardsDbContext : DbContext
    {
        #region DbSet
        public DbSet<Metadata> Metadata { get; set; }
        public DbSet<ScryfallMagicCard> Catalog { get; set; }
        public DbSet<InventoryCard> Library { get; set; }
        public DbSet<FullInventoryCard> LibraryView { get; set; }
        public DbSet<ScryfallCardSet> Sets { get; set; }
        public DbSet<CardCollection> Collections { get; set; }
        public DbSet<CollectionGroup> CollectionGroups { get; set; }
        public DbSet<CardCollectionItem> CollectionsView { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ScryfallMagicCardBase>();
            modelBuilder.Entity<InventoryCard>().Property(b => b.TimeAdded).HasDefaultValueSql("datetime('now','localtime')");
            modelBuilder.Entity<ScryfallMagicCard>().HasKey(x => x.ScryfallId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection("Data Source=scryfallcards.db;");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
    public class CardImagesDbContext : DbContext
    {
        #region DbSet
        public DbSet<DbCardImage> CardImages { get; set; }
        #endregion
        private readonly string Edition;

        public CardImagesDbContext(string Edition)
        {
            this.Edition = Edition.SanitizeFilename();
            var dir = new DirectoryInfo($"Card Images/");
            if (!dir.Exists) dir.Create();
            string fileName = $"Card Images/{this.Edition}.db";
            var file = new FileInfo(fileName);
            if (!file.Exists)
            {
                using (SqliteConnection conn = new SqliteConnection($"Data Source={fileName}"))
                using (SqliteCommand createDB = new SqliteCommand("CREATE TABLE CardImages (ScryfallId TEXT, Side TEXT DEFAULT A, MVid INTEGER, CardImageBytes BLOB, PRIMARY KEY (ScryfallId, Side)); ", conn))
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
