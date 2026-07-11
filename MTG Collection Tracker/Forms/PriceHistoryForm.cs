using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class PriceHistoryForm : Form
    {
        private int collectionId;
        private CartesianChart priceHistoryChart;
        private string defaultPaperCurrency;
        public int CollectionId { get { return collectionId; } set { collectionId = value; FillChart(); } }
        public PriceHistoryForm()
        {
            InitializeComponent();
            priceHistoryChart = new CartesianChart();
            var title = new DrawnLabelVisual(
               new LabelGeometry
               {
                   Text = "Price History",
                   Paint = new SolidColorPaint(SKColors.Black),
                   TextSize = 25,
                   Padding = new LiveChartsCore.Drawing.Padding(5)
               });

            priceHistoryChart.Title = title;
            priceHistoryChart.Dock = DockStyle.Fill;
            Controls.Add(priceHistoryChart);
            defaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
        }

        private void FillChart()
        {
            DebugOutput.WriteLine($"Filling price history chart for collection ID: {collectionId}");
            var cultureInfo = new CultureInfo(defaultPaperCurrency == "USD" ? "en-US" : "fr-FR");
            var priceList = new List<double>();
            var countList = new List<int>();
            var dateList = new List<DateTime>();
            
            using (var context = new ScryfallCardsDbContext()) 
            {
                var watch = new Stopwatch();
                watch.Start();
        
                var collectionEntries = context.CollectionHistories
                    .AsNoTracking()
                    .Where(c => c.DestinationCollectionId == collectionId || c.SourceCollectionId == collectionId)
                    .OrderBy(c => c.Time)
                    .ToList();
                watch.Stop();
                DebugOutput.WriteLine($"Collection history retrieval took {watch.ElapsedMilliseconds} ms");
                watch.Restart();
                if (collectionEntries.Count == 0)
                {
                    watch.Stop();
                    DebugOutput.WriteLine($"Data retrieval took {watch.ElapsedMilliseconds} ms");
                    return;
                }

                var startDate = collectionEntries[0].Time.Date;
                var endDate = DateTime.Now.Date;
                var inventoryIds = collectionEntries.Select(c => c.InventoryId).ToHashSet();

                var quantityEntries = context.CardQuantityHistories
                    .AsNoTracking()
                    .Where(q => inventoryIds.Contains(q.InventoryId))
                    .ToList();
                watch.Stop();
                DebugOutput.WriteLine($"Quantity history retrieval took {watch.ElapsedMilliseconds} ms");
                watch.Restart();
                var inventoryCards = context.Library
                    .Where(i => inventoryIds.Contains(i.InventoryId))
                    .Select(i => new 
                    { 
                        i.InventoryId, 
                        i.ScryfallId, 
                        i.Platform, 
                        i.Finish 
                    })
                    .ToDictionary(i => i.InventoryId, i => new InventoryCard 
                    { 
                        InventoryId = i.InventoryId, 
                        ScryfallId = i.ScryfallId, 
                        Platform = i.Platform, 
                        Finish = i.Finish 
                    });
                watch.Stop();
                DebugOutput.WriteLine($"Inventory retrieval took {watch.ElapsedMilliseconds} ms");
                watch.Restart();
                var scryfallIds = inventoryCards.Values.Select(i => i.ScryfallId).ToHashSet();

                var priceHistories = context.PriceHistories
                    .AsNoTracking()
                    .Where(p => scryfallIds.Contains(p.ScryfallId))
                    .ToList();

                watch.Stop();
                DebugOutput.WriteLine($"Price history retrieval took {watch.ElapsedMilliseconds} ms");
                watch.Restart();

                // Group data by date for faster lookup
                var priceHistoriesByDate = priceHistories
                    .GroupBy(p => p.Time.Date)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var quantityEntriesByDate = quantityEntries
                    .GroupBy(q => q.Time.Date)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var collectionEntriesByDate = collectionEntries
                    .GroupBy(c => c.Time.Date)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var selectionInventory = new Dictionary<int, InventoryCard>();
                bool foundActiveInventory = false;

                for (var selectionDate = startDate; selectionDate <= endDate; selectionDate = selectionDate.AddDays(1))
                {
                    // Update collection inventory
                    if (collectionEntriesByDate.TryGetValue(selectionDate, out var collectionHistoryEntries))
                    {
                        foreach (var entry in collectionHistoryEntries)
                        {
                            if (inventoryCards.TryGetValue(entry.InventoryId, out var inventoryCard))
                            {
                                if (entry.DestinationCollectionId == collectionId)
                                    selectionInventory[inventoryCard.InventoryId] = inventoryCard;
                                else
                                    selectionInventory.Remove(inventoryCard.InventoryId);
                            }
                        }
                    }

                    // Update quantities
                    if (quantityEntriesByDate.TryGetValue(selectionDate, out var quantityHistoryEntries))
                    {
                        foreach (var entry in quantityHistoryEntries)
                        {
                            if (selectionInventory.TryGetValue(entry.InventoryId, out var inventoryCard))
                                inventoryCard.Count = entry.Quantity;
                        }
                    }

                    // Update prices
                    if (priceHistoriesByDate.TryGetValue(selectionDate, out var priceHistoryEntries))
                    {
                        var pricesByScryfallId = priceHistoryEntries.GroupBy(p => p.ScryfallId).ToDictionary(g => g.Key, g => g.Last().Prices);
                        foreach (var inventoryCard in selectionInventory.Values)
                        {
                            if (pricesByScryfallId.TryGetValue(inventoryCard.ScryfallId, out var prices))
                                inventoryCard.Prices = prices;
                        }
                    }

                    if (selectionInventory.Count == 0 && !foundActiveInventory)
                        continue;

                    foundActiveInventory = true;
                    CountInventory(selectionInventory, out int totalCount, out double totalPrice);
                    priceList.Add(totalPrice);
                    countList.Add(totalCount);
                    dateList.Add(selectionDate);
                }

                watch.Stop();
                DebugOutput.WriteLine($"Data processing took {watch.ElapsedMilliseconds} ms");
            }

            priceHistoryChart.Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = countList.ToArray(),
                    ScalesYAt = 0,
                },
                new LineSeries<double>
                {
                    Values = priceList.ToArray(),
                    GeometrySize = 10,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    ScalesYAt = 1,
                    YToolTipLabelFormatter = (chartPoint) => chartPoint.Model.ToString("C", cultureInfo)
                }
            };

            priceHistoryChart.XAxes = new LiveChartsCore.SkiaSharpView.Axis[]
            {
                new LiveChartsCore.SkiaSharpView.Axis
                {
                    Labels = dateList.Select(x => x.ToString("yyyy-MM-dd")).ToArray(),
                    Name = "Date",
                }
            };

            priceHistoryChart.YAxes = new LiveChartsCore.SkiaSharpView.Axis[]
            {
                new LiveChartsCore.SkiaSharpView.Axis
                {
                    Name = "Count",
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    Labeler = (value) => value.ToString("N0"),
                    MinStep = 1
                },
                new LiveChartsCore.SkiaSharpView.Axis
                {
                    Name = "Price",
                    Position = LiveChartsCore.Measure.AxisPosition.End,
                    Labeler = (value) => value.ToString("C", cultureInfo)
                }
            }; 
        }

        private void CountInventory(Dictionary<int, InventoryCard> selectionInventory, out int totalCount, out double totalPrice)
        {
            totalCount = 0;
            totalPrice = 0.0;

            foreach (var card in selectionInventory.Values)
            {
                totalCount += card.Count.Value;
                var price = card.FindPrice(defaultPaperCurrency);
                if (price.HasValue)
                {
                    totalPrice += (double)(card.Count * price.Value);
                }
            }
        }
    }
}
