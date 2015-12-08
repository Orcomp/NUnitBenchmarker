// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotFactory.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public static class PlotFactory
    {
        public static PlotModel CreatePlotModel(BenchmarkResult result, bool isLinear = true)
        {
            int dummy = 0;

            var plotModel = int.TryParse(result.TestCases.FirstOrDefault(), out dummy)
                ? CreateLinearPlotModel(result, isLinear)
                : CreateCategoryPlotModel(result, isLinear);

            return plotModel;
        }

        private static PlotModel CreateLinearPlotModel(BenchmarkResult result, bool isLinear = true)
        {
            var plotModel = new PlotModel
            {
                Title = result.Key,
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Vertical,
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.TopLeft,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black,
                DefaultColors = new List<OxyColor>
                {
                    OxyColors.OrangeRed,
                    OxyColors.LightGreen,
                    OxyColors.DeepSkyBlue,
                    OxyColors.Yellow,
                    OxyColors.DarkRed,
                    OxyColors.ForestGreen,
                    OxyColors.Blue,
                    OxyColors.Orange,
                    OxyColors.Indigo,
                    OxyColors.Violet,
                }
            };

            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom
            };
            plotModel.Axes.Add(xAxis);

            Axis yAxis;
            if (isLinear)
            {
                yAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    Title = "Time (ms)",
                };
            }
            else
            {
                yAxis = new LogarithmicAxis
                {
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    Title = "Time (ms)",
                    Minimum = 0.01,
                    UseSuperExponentialFormat = true
                };
            }

            plotModel.Axes.Add(yAxis);

            foreach (var series in result.Values)
            {
                var lineSeries = new LineSeries
                {
                    Title = series.Key,
                    MarkerType = MarkerType.Diamond,
                    MarkerSize = 3
                };

                foreach (var dataPoint in series.Value)
                {
                    lineSeries.Points.Add(new DataPoint(int.Parse(dataPoint.Key), dataPoint.Value));
                }

                plotModel.Series.Add(lineSeries);
            }

            return plotModel;
        }

        private static PlotModel CreateCategoryPlotModel(BenchmarkResult result, bool isLinear = true)
        {
            var plotModel = new PlotModel
            {
                Title = result.Key,
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Vertical,
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black,
                DefaultColors = new List<OxyColor>
                {
                    OxyColors.OrangeRed,
                    OxyColors.LightGreen,
                    OxyColors.DeepSkyBlue,
                    OxyColors.Yellow,
                    OxyColors.DarkRed,
                    OxyColors.ForestGreen,
                    OxyColors.Blue,
                    OxyColors.Orange,
                    OxyColors.Indigo,
                    OxyColors.Violet,
                }
            };

            //var dateAxis = new CategoryAxis(AxisPosition.Bottom, "Categories", result.TestCases)
            var dateAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Categories"
            };

            plotModel.Axes.Add(dateAxis);

            Axis valueAxis;
            if (isLinear)
            {
                valueAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    Title = "Time (ms)"
                };
            }
            else
            {
                valueAxis = new LogarithmicAxis
                {
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    Title = "Time (ms)",
                    Minimum = 0.01,
                    UseSuperExponentialFormat = true,
                    StartPosition = 1,
                    EndPosition = 0
                };
            }

            plotModel.Axes.Add(valueAxis);

            var columns = new Dictionary<string, ColumnSeries>();

            foreach (var series in result.Values)
            {
                if (!columns.ContainsKey(series.Key))
                {
                    columns[series.Key] = new ColumnSeries
                    {
                        Title = series.Key
                    };
                }

                var columnSeries = columns[series.Key];

                var categoryIndex = 0;
                foreach (var dataPoint in series.Value.OrderBy(x => x.Key))
                {
                    if (!dateAxis.Labels.Contains(dataPoint.Key))
                    {
                        dateAxis.Labels.Add(dataPoint.Key);
                    }

                    columnSeries.Items.Add(new ColumnItem(dataPoint.Value, categoryIndex));
                    categoryIndex++;
                }

                plotModel.Series.Add(columnSeries);
            }

            return plotModel;
        }
    }
}