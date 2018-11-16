using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Collection_Tracker
{
    public class MCard : MCardBase
    {
        [Key]
        public int catalogID { get; set; }
    }

    public class MCardBase
    {
        public List<(string, string)> _legalities;
        public List<(string, string)> _rulings;
        public int MVid { get; set; }
        public string Part { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string Type { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string OracleText { get; set; }
        public string Edition { get; set; }
        public string Rarity { get; set; }
        public double Rating { get; set; }
        public string Artist { get; set; }
        public string ColNumber { get; set; }
        public string Text { get; set; }
        public string FlavorText { get; set; }
        public string QueryName { get; set; }
        public string ColorIndicator { get; set; }
        public string Legalities
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
        public string Rulings
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
        public int? CMC { get; set; }
        public double? OnlinePrice { get; set; }
    }
    
    public class DBCardInstance
    {
        [Key]
        public int rowid { get; set; }
        public int Count { get; set; }
        public double? Cost { get; set; }
        public string Tags { get; set; }
    }
    
    public class CardInstance : MCardBase
    {
        [Key]        
        public int clID { get; set; }
        public int rowid { get; set; }
        public int Count { get; set; }
        public double? Cost { get; set; }
        public string Tags { get; set; }
    }

    public class CardCollection
    {
        [Key]
        public string CollectionName { get; set; }
        [ForeignKey("CollectionGroups")]
        public string GroupName { get; set; }
        public string Type { get; set; }
        public bool Virtual { get; set; }
    }

    public class CardCollectionItem
    {
        [Key]
        public int clID { get; set; }
        public string CollectionName { get; set; }
        public int CardInstanceId { get; set; }        
        public int MVid { get; set; }
        public int Count { get; set; }
        public double? Cost { get; set; }
        public string Tags { get; set; }
        public string Type { get; set; }
        public bool Virtual { get; set; }
    }

    public class CollectionGroup
    {
        [Key]
        public string GroupName { get; set; }
        public bool CanDelete { get; set; }
    }

    public class CardSet
    {
        [Key]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Code2 { get; set; }
        public string ReleaseDate { get; set; }
        public string Type { get; set; }
        public string Block { get; set; }
        [NotMapped]
        public Image CommonIcon { get; set; }
        public byte[] CommonIconBytes { get => CommonIcon?.ToByteArray(); set => CommonIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image UncommonIcon { get; set; }
        public byte[] UncommonIconBytes { get => UncommonIcon?.ToByteArray(); set => UncommonIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image RareIcon { get; set; }
        public byte[] RareIconBytes { get => RareIcon?.ToByteArray(); set => RareIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image MythicRareIcon { get; set; }
        public byte[] MythicRareIconBytes { get => MythicRareIcon?.ToByteArray(); set => MythicRareIcon = ImageExtensions.FromByteArray(value); }
    }

    public class MyDbContext : DbContext
    {
        #region DbSet
        public DbSet<MCard> catalog { get; set; }
        public DbSet<DBCardInstance> library { get; set; }
        public DbSet<CardInstance> libraryview { get; set; }
        public DbSet<CardSet> sets { get; set; }
        public DbSet<CardCollection> collections { get; set; }
        public DbSet<CollectionGroup> CollectionGroups { get; set; }
        public DbSet<CardCollectionItem> collectionsview { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<MCardBase>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection("Data Source=cards.db;");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
}
