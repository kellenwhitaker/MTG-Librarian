using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Forms;

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
        [NotMapped]
        public List<ScryfallCardRuling> rulings { get; set; }
        [NotMapped]
        public List<ScryfallCard> printings { get; set; }
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
        public DbSet<InventoryCardBase> Library { get; set; }
        public DbSet<InventoryCard> LibraryView { get; set; }
        public DbSet<ScryfallCardSet> Sets { get; set; }
        public DbSet<CardCollection> Collections { get; set; }
        public DbSet<CollectionGroup> CollectionGroups { get; set; }
        public DbSet<CollectionHistory> CollectionHistories { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<CardQuantityHistory> CardQuantityHistories { get; set; }
        public DbSet<CollectionSnapshot> CollectionSnapshots { get; set; }
        public DbSet<CardCollectionItem> CollectionsView { get; set; }
        #endregion

        public ScryfallCardsDbContext()
        {
            this.Database.ExecuteSqlCommand("PRAGMA foreign_keys = ON;");
            EnsureDatabaseCreated();
            UpgradeDatabase();
        }
        private void AddColumnIfNotExists(SqliteConnection connection, string tableName, string columnName, string columnType)
        {
            using (var checkCommand = connection.CreateCommand())
            {
                checkCommand.CommandText = $"PRAGMA table_info({tableName});";
                using (var reader = checkCommand.ExecuteReader())
                {
                    bool columnExists = false;
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == columnName)
                        {
                            columnExists = true;
                            break;
                        }
                    }
                    if (!columnExists)
                    {
                        using (var addCommand = connection.CreateCommand())
                        {
                            addCommand.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnType};";
                            addCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private void UpgradeDatabase()
        {
            using (var sqliteConn = new SqliteConnection($"Data Source=cards.db"))
            {
                sqliteConn.Open();
                AddColumnIfNotExists(sqliteConn, "Collections", "ColorIdentity", "TEXT");
            }
        }
        private void EnsureDatabaseCreated()
        {
            string fileName = "cards.db";
            var file = new FileInfo(fileName);
            if (!file.Exists)
            {
                using (var sqliteConn = new SqliteConnection($"Data Source={fileName}"))
                {
                    sqliteConn.Open();
                    CreateDB(sqliteConn);
                }   
            }
            else
            {
                using (var sqliteConn = new SqliteConnection($"Data Source={fileName}"))
                {
                    sqliteConn.Open();
                    using (var checkCommand = sqliteConn.CreateCommand())
                    {
                        checkCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Catalog';";
                        var result = checkCommand.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            CreateDB(sqliteConn);
                        }
                    }
                }
            }
                void CreateDB(SqliteConnection sqliteConn)
                {
                    try
                    {
                        //var sqliteConn = new SqliteConnection("Data Source=cards.db;");
                        //sqliteConn.Open();

                        // Database does not exist, create it
                        using (var createCommand = sqliteConn.CreateCommand())
                        {
                            createCommand.CommandText = @"
                            CREATE TABLE ""Catalog"" (
	                        ""ScryfallId""	TEXT,
	                        ""oracle_id""	TEXT,
	                        ""MultiverseIds""	TEXT,
	                        ""mtgo_id""	INTEGER,
	                        ""mtgo_foil_id""	INTEGER,
	                        ""tcgplayer_product_id""	INTEGER,
	                        ""cardmarket_product_id""	INTEGER,
	                        ""Name""	TEXT,
	                        ""lang""	TEXT,
	                        ""released_at""	TEXT,
	                        ""Uri""	TEXT,
	                        ""scryfall_uri""	TEXT,
	                        ""layout""	TEXT,
	                        ""highres_image""	INTEGER,
	                        ""image_status""	TEXT,
	                        ""ImageURIs""	TEXT,
	                        ""mana_cost""	TEXT,
	                        ""cmc""	NUMERIC,
	                        ""type_line""	TEXT,
	                        ""text""	TEXT,
	                        ""power""	TEXT,
	                        ""toughness""	TEXT,
	                        ""Colors""	TEXT,
	                        ""ColorIdentity""	TEXT,
	                        ""Keywords""	TEXT,
	                        ""Legalities""	TEXT,
	                        ""Games""	TEXT,
	                        ""reserved""	INTEGER,
	                        ""has_foil""	INTEGER,
	                        ""has_nonfoil""	INTEGER,
	                        ""Finishes""	TEXT,
	                        ""oversized""	INTEGER,
	                        ""promo""	INTEGER,
	                        ""reprint""	INTEGER,
	                        ""variation""	INTEGER,
	                        ""set_id""	TEXT,
	                        ""set""	TEXT,
	                        ""set_name""	TEXT,
	                        ""set_type""	TEXT,
	                        ""set_uri""	TEXT,
	                        ""set_search_uri""	TEXT,
	                        ""scryfall_set_uri""	TEXT,
	                        ""rulings_uri""	TEXT,
	                        ""prints_search_uri""	TEXT,
	                        ""collector_number""	INTEGER,
	                        ""digital""	INTEGER,
	                        ""rarity""	TEXT,
	                        ""card_back_id""	TEXT,
	                        ""artist""	TEXT,
	                        ""ArtistIds""	TEXT,
	                        ""illustration_id""	TEXT,
	                        ""border_color""	TEXT,
	                        ""frame_version""	TEXT,
	                        ""full_art""	INTEGER,
	                        ""textless""	INTEGER,
	                        ""booster""	INTEGER,
	                        ""story_spotlight""	INTEGER,
	                        ""edhrec_rank""	INTEGER,
	                        ""penny_rank""	INTEGER,
	                        ""Prices""	TEXT,
	                        ""RelatedURIs""	TEXT,
	                        ""PurchaseURIs""	TEXT,
	                        ""flavor_text""	TEXT,
	                        ""oracle_text""	TEXT,
	                        ""security_stamp""	TEXT, 
                            CardFaces TEXT, 
                            printed_name TEXT, 
                            printed_type_line TEXT, 
                            printed_text TEXT, 
                            ""loyalty"" TEXT, 
                            ""flavor_name"" TEXT, 
                            ""FrameEffects"" TEXT, 
                            ""AttractionLights"" TEXT, 
                            ""PromoTypes"" TEXT, 
                            ""variation_of"" TEXT, 
                            ""watermark"" TEXT, 
                            ""defense"" TEXT, 
                            ""hand_modifier"" TEXT, 
                            ""life_modifier"" TEXT, 
                            ""ProducedMana"" TEXT, 
                            ""game_changer"" INTEGER,
                            ""ColorIndicator"" TEXT,
	                        PRIMARY KEY(""ScryfallId""),
                            FOREIGN KEY(""set_id"") REFERENCES ""Sets""(""id""));
                        CREATE TABLE CollectionGroups (
                            GroupName TEXT NOT NULL UNIQUE, 
                            Permanent BOOLEAN NOT NULL DEFAULT FALSE, 
                            Id INTEGER PRIMARY KEY, 
                            ""Virtual"" BOOLEAN NOT NULL);
                        CREATE TABLE Collections (
                            CollectionName TEXT NOT NULL, 
                            Type TEXT NOT NULL, 
                            ""Virtual"" BOOLEAN NOT NULL DEFAULT FALSE, 
                            GroupName TEXT, 
                            Permanent BOOLEAN DEFAULT FALSE, 
                            GroupId, 
                            Id INTEGER PRIMARY KEY, 
                            ""Platform"" TEXT, 
                            ""Commander"" INTEGER, 
                            ""CollapsedView"" INTEGER,
                            ""ColorIdentity"" TEXT,
                            UNIQUE(""CollectionName"", ""Platform""),
                            FOREIGN KEY(""GroupId"") REFERENCES ""CollectionGroups""(""Id""));
                        CREATE TABLE Library (
                            InventoryId INTEGER PRIMARY KEY AUTOINCREMENT, 
                            Count INTEGER DEFAULT 1 NOT NULL ON CONFLICT REPLACE, 
                            Cost NUMERIC, 
                            Tags TEXT, 
                            TimeAdded DATETIME DEFAULT (datetime('now', 'localtime')) NOT NULL, 
                            InsertionIndex INTEGER, 
                            CollectionId INTEGER REFERENCES Collections (Id), 
                            ScryfallId TEXT, 
                            Foil BOOLEAN DEFAULT (0), 
                            PartB_ScryfallId TEXT, 
                            ""Virtual"" BOOLEAN NOT NULL, 
                            Condition TEXT, 
                            ""Finish"" TEXT, 
                            ""Platform"" TEXT, 
                            ""Board"" TEXT, 
                            ""IsCommander"" INTEGER,
                            ""SoldPrice"" NUMERIC,
                            ""SoldTime"" DATETIME,
                            FOREIGN KEY (ScryfallId) REFERENCES Catalog(ScryfallId));
                        CREATE TABLE ""Metadata"" (
	                        ""Name""	TEXT,
	                        ""Value""	TEXT,
	                        PRIMARY KEY(""Name""));
                        CREATE TABLE ""Sets"" (
	                        ""id""	TEXT,
	                        ""code""	TEXT,
	                        ""mtgo_code""	TEXT,
	                        ""tcgplayer_id""	INTEGER,
	                        ""name""	TEXT,
	                        ""uri""	TEXT,
	                        ""scryfall_uri""	TEXT,
	                        ""search_uri""	TEXT,
	                        ""released_at""	TEXT,
	                        ""set_type""	TEXT,
	                        ""card_count""	INTEGER,
	                        ""printed_size""	INTEGER,
	                        ""digital""	INTEGER,
	                        ""nonfoil_only""	INTEGER,
	                        ""foil_only""	INTEGER,
	                        ""icon_svg_uri""	TEXT,
	                        ""CommonIconBytes""	BLOB,
	                        ""UncommonIconBytes""	BLOB,
	                        ""RareIconBytes""	BLOB,
	                        ""MythicRareIconBytes""	BLOB,
	                        ""arena_code""	TEXT,
	                        ""LastUpdated""	TEXT,
	                        PRIMARY KEY(""id""));
                        CREATE TABLE ""CollectionSnapshots"" (
	                        ""CollectionId""	INTEGER NOT NULL,
	                        ""Time""	TEXT NOT NULL,
	                        ""Count""	INTEGER NOT NULL,
	                        ""Cost""	NUMERIC NOT NULL,
	                        ""Price""	NUMERIC NOT NULL);
                        CREATE VIEW LibraryView(
                            InventoryId, 
                            ""Count"", 
                            Cost, 
                            Tags, 
                            TimeAdded, 
                            InsertionIndex, 
                            CollectionId, 
                            ScryfallId, 
                            Foil, 
                            PartB_ScryfallId, 
                            ""Virtual"", 
                            Condition, 
                            Finish, 
                            Platform, 
                            Board, 
                            IsCommander,
                            SoldPrice,
                            SoldTime,
                            ScryfallId, 
                            oracle_id, 
                            MultiverseIds, 
                            mtgo_id, 
                            mtgo_foil_id, 
                            tcgplayer_product_id, 
                            cardmarket_product_id, 
                            Name, 
                            lang, 
                            released_at, 
                            Uri, 
                            scryfall_uri, 
                            layout, 
                            highres_image, 
                            image_status, 
                            ImageURIs, 
                            mana_cost, 
                            cmc, 
                            type_line, 
                            ""text"", 
                            ""power"", 
                            toughness, 
                            Colors, 
                            ColorIdentity, 
                            Keywords, 
                            Legalities, 
                            Games, 
                            reserved, 
                            has_foil, 
                            has_nonfoil, 
                            Finishes, 
                            oversized, 
                            promo, 
                            reprint, 
                            variation, 
                            set_id, 
                            ""set"", 
                            set_name, 
                            set_type, 
                            set_uri, 
                            set_search_uri, 
                            scryfall_set_uri, 
                            rulings_uri, 
                            prints_search_uri, 
                            collector_number, 
                            digital, 
                            rarity, 
                            card_back_id, 
                            artist, 
                            ArtistIds, 
                            illustration_id, 
                            border_color, 
                            frame_version, 
                            full_art, 
                            textless, 
                            booster, 
                            story_spotlight, 
                            edhrec_rank, 
                            penny_rank, 
                            Prices, 
                            RelatedURIs, 
                            PurchaseURIs, 
                            flavor_text, 
                            oracle_text, 
                            security_stamp, 
                            CardFaces, 
                            printed_name, 
                            printed_type_line, 
                            printed_text, 
                            loyalty, 
                            flavor_name, 
                            FrameEffects, 
                            AttractionLights, 
                            PromoTypes, 
                            variation_of, 
                            watermark, 
                            defense, 
                            hand_modifier, 
                            life_modifier, 
                            ProducedMana, 
                            game_changer, 
                            ColorIndicator) AS SELECT * FROM Library INNER JOIN Catalog ON Library.ScryfallId = Catalog.ScryfallId;
                        CREATE INDEX ""idx_catalog"" ON ""Catalog"" (
	                        ""ScryfallId"",
	                        ""oracle_id"",
	                        ""MultiverseIds"",
	                        ""mtgo_id"",
	                        ""mtgo_foil_id"",
	                        ""tcgplayer_product_id"",
	                        ""cardmarket_product_id"",
	                        ""Name"",
	                        ""lang"",
	                        ""released_at"",
	                        ""Uri"",
	                        ""scryfall_uri"",
	                        ""layout"",
	                        ""highres_image"",
	                        ""image_status"",
	                        ""ImageURIs"",
	                        ""mana_cost"",
	                        ""cmc"",
	                        ""type_line"",
	                        ""text"",
	                        ""power"",
	                        ""toughness"",
	                        ""Colors"",
	                        ""ColorIdentity"",
	                        ""Keywords"",
	                        ""Legalities"",
	                        ""Games"",
	                        ""reserved"",
	                        ""has_foil"",
	                        ""has_nonfoil"",
	                        ""Finishes"",
	                        ""oversized"",
	                        ""promo"",
	                        ""reprint"",
	                        ""variation"",
	                        ""set_id"",
	                        ""set"",
	                        ""set_name"",
	                        ""set_type"",
	                        ""set_uri"",
	                        ""set_search_uri"",
	                        ""scryfall_set_uri"",
	                        ""rulings_uri"",
	                        ""prints_search_uri"",
	                        ""collector_number"",
	                        ""digital"",
	                        ""rarity"",
	                        ""card_back_id"",
	                        ""artist"",
	                        ""ArtistIds"",
	                        ""illustration_id"",
	                        ""border_color"",
	                        ""frame_version"",
	                        ""full_art"",
	                        ""textless"",
	                        ""booster"",
	                        ""story_spotlight"",
	                        ""edhrec_rank"",
	                        ""penny_rank"",
	                        ""Prices"",
	                        ""RelatedURIs"",
	                        ""PurchaseURIs"",
	                        ""flavor_text"",
	                        ""oracle_text"",
	                        ""security_stamp"",
	                        ""CardFaces"",
	                        ""printed_name"",
	                        ""printed_type_line"",
	                        ""printed_text"",
	                        ""loyalty"",
	                        ""flavor_name"",
	                        ""FrameEffects"",
	                        ""AttractionLights"",
	                        ""PromoTypes"",
	                        ""variation_of"",
	                        ""watermark"",
	                        ""defense"",
	                        ""hand_modifier"",
	                        ""life_modifier"",
	                        ""ProducedMana"",
	                        ""game_changer"",
	                        ""ColorIndicator"");
                        CREATE INDEX ""idx_library"" ON ""Library"" (
	                        ""InventoryId"",
	                        ""ScryfallId"",
	                        ""CollectionId"",
	                        ""Count"",
	                        ""Cost"",
	                        ""Tags"",
	                        ""TimeAdded"",
	                        ""InsertionIndex"",
	                        ""Foil"",
	                        ""PartB_ScryfallId"",
	                        ""Virtual"",
	                        ""Condition"",
	                        ""Finish"",
	                        ""Platform"",
	                        ""Board"",
	                        ""IsCommander"",
                            ""SoldPrice"",
                            ""SoldTime"");
                        CREATE INDEX ""idx_sets"" ON ""Sets"" (
	                        ""id"",
	                        ""code"",
	                        ""mtgo_code"",
	                        ""tcgplayer_id"",
	                        ""name"",
	                        ""uri"",
	                        ""scryfall_uri"",
	                        ""search_uri"",
	                        ""released_at"",
	                        ""set_type"",
	                        ""card_count"",
	                        ""printed_size"",
	                        ""digital"",
	                        ""nonfoil_only"",
	                        ""foil_only"",
	                        ""icon_svg_uri"",
	                        ""CommonIconBytes"",
	                        ""UncommonIconBytes"",
	                        ""RareIconBytes"",
	                        ""MythicRareIconBytes"",
	                        ""arena_code"",
	                        ""LastUpdated"");
                        CREATE INDEX ""idx_collectionsnapshots"" ON ""CollectionSnapshots"" (
	                        ""CollectionId"",
	                        ""Time"",
	                        ""Count"",
	                        ""Cost"",
	                        ""Price"");
                        ";
                            createCommand.ExecuteNonQuery();                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating database: {ex.Message}");
                    }
                }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ScryfallMagicCardBase>();
            modelBuilder.Entity<InventoryCardBase>().Property(b => b.TimeAdded).HasDefaultValueSql("datetime('now','localtime')");
            modelBuilder.Entity<ScryfallMagicCard>().HasKey(x => x.ScryfallId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection("Data Source=cards.db;");
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
