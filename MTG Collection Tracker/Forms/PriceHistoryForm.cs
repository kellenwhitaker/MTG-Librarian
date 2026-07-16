using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MTG_Librarian
{
    public partial class PriceHistoryForm : Form
    {
        private CardCollection collection;
        private CartesianChart priceHistoryChart;
        private string defaultPaperCurrency;
        public CardCollection Collection { get { return collection; } set { collection = value; FillChart(); } }
        public PriceHistoryForm()
        {
            InitializeComponent();
            priceHistoryChart = new CartesianChart();
            priceHistoryChart.Dock = DockStyle.Fill;
            Controls.Add(priceHistoryChart);
            defaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
        }

        private void FillChart()
        {
            DebugOutput.WriteLine($"Filling price history chart for collection ID: {collection.Id}");
            var cultureInfo = new CultureInfo(defaultPaperCurrency == "USD" ? "en-US" : "fr-FR");
            var countSeries = new ObservableCollection<DateTimePoint>();
            var costSeries = new ObservableCollection<DateTimePoint>();
            var priceSeries = new ObservableCollection<DateTimePoint>();

            using (var context = new ScryfallCardsDbContext()) 
            {
                var watch = new Stopwatch();
                watch.Start();
                var collectionEntries = (from c in context.CollectionSnapshots
                                        where c.CollectionId == collection.Id
                                        select c)
                                        .OrderBy(c => c.Time)
                                        .GroupBy(c => c.Time.Date)
                                        .Select(g => g.Last())
                                        .ToList();

                collectionEntries = DateGapFiller.FillGaps(collectionEntries);
                int index = 0;
                foreach (var entry in collectionEntries)
                {
                    countSeries.Add(new DateTimePoint(entry.Time, entry.Count));
                    costSeries.Add(new DateTimePoint(entry.Time, entry.Cost));
                    priceSeries.Add(new DateTimePoint(entry.Time, entry.Price));
                    index++;
                }
                var span = collectionEntries.Count > 0 ? collectionEntries.Count : 0;//dateList.Count > 0 ? (dateList[dateList.Count - 1] - dateList[0]).Add(TimeSpan.FromDays(1)) : TimeSpan.Zero;
                SetChartTitle(span);
            }
            var platform = Collection.Platform;
            priceHistoryChart.Series = new ISeries[]
            {
                new ColumnSeries<DateTimePoint>
                {
                    Name = "Card Count",
                    Values = countSeries,
                    ScalesYAt = 0,
                },
                new LineSeries<DateTimePoint>
                {
                    EnableNullSplitting = false,
                    Name = "Price",
                    Values = priceSeries,
                    GeometrySize = 5,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    ScalesYAt = 1,
                    YToolTipLabelFormatter = (chartPoint) => platform == "Paper" ? chartPoint.Model.Value?.ToString("C", cultureInfo) : chartPoint.Model.Value?.ToString("F2")
                },
                new LineSeries<DateTimePoint>
                {
                    EnableNullSplitting = false,
                    Name = "Cost",
                    Values = costSeries,
                    GeometrySize = 5,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Green, 3),
                    ScalesYAt = 1,
                    YToolTipLabelFormatter = (chartPoint) => platform == "Paper" ? chartPoint.Model.Value?.ToString("C", cultureInfo) : chartPoint.Model.Value?.ToString("F2")
                }
            };
            var xAxis = new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("yyyy-MM-dd"));
            priceHistoryChart.XAxes = new LiveChartsCore.SkiaSharpView.Axis[] { xAxis };            
            priceHistoryChart.YAxes = new LiveChartsCore.SkiaSharpView.Axis[]
            {
                new LiveChartsCore.SkiaSharpView.Axis
                {
                    Name = "Card Count",
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    Labeler = (value) => value.ToString("N0"),
                    MinStep = 1
                },
                new LiveChartsCore.SkiaSharpView.Axis
                {
                    Name = "Price",
                    Position = LiveChartsCore.Measure.AxisPosition.End,
                    Labeler = (value) => platform == "Paper" ? value.ToString("C", cultureInfo) : value.ToString("F2")
                }
            };
            priceHistoryChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom;
            priceHistoryChart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
        }

        private void SetChartTitle(int span)
        {
            var years = span / 365;
            var days = span % 365;
            string yearsText = years > 0 ? $"{years} {(years == 1 ? "year" : "years")} " : "";

            var title = new DrawnLabelVisual(
            new LabelGeometry
            {
                Text = $"Price History ({yearsText}{days} {(days == 1 ? "day" : "days")})",
                Paint = new SolidColorPaint(SKColors.Black),
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(5)
            });
            priceHistoryChart.Title = title;
        } 
    }

    public class DateGapFiller
    {
        public static List<CollectionSnapshot> FillGaps(List<CollectionSnapshot> existingData)
        {
            if (existingData == null || existingData.Count == 0)
                return new List<CollectionSnapshot>();
            var filledData = new List<CollectionSnapshot>();
            CollectionSnapshot lastSnapshot = null;
            foreach (var item in existingData)
            {
                if (lastSnapshot == null)
                    filledData.Add(item);
                else
                {
                    var lastDate = lastSnapshot.Time.Date;
                    var currentDate = item.Time.Date;
                    var daysDifference = (currentDate - lastDate).Days;
                    for (int i = 1; i < daysDifference; i++)
                    {
                        var missingDate = lastDate.AddDays(i);
                        filledData.Add(new CollectionSnapshot
                        {
                            Time = missingDate,
                            Count = null,
                            Cost = null,
                            Price = null
                        });
                    }
                    filledData.Add(item);
                }
                lastSnapshot = item;
            }
            return filledData;
        }
    }

}
