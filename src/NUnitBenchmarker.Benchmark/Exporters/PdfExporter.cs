// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Exporters
{
    using System;
    using System.IO;
    using Data;
    using Logging;
    using MigraDoc.DocumentObjectModel;
    using MigraDoc.DocumentObjectModel.Shapes;
    using MigraDoc.Rendering;
    using OxyPlot;

    public class PdfExporter : ExporterBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PdfExporter));

        public void Export(BenchmarkResult result, string folderPath = null)
        {
            var testName = result.Key;
            folderPath = GetFolderPath(folderPath);
            var fileName = Path.Combine(folderPath, testName) + ".pdf";

            var document = new Document();
            document.Info.Title = testName;

            var section = document.AddSection();

            DefineStyles(document);
            //ExportPlot(section, result);
            ExportTable(section, result);

            var renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();

            using (var fileStream = File.Create(fileName))
            {
                renderer.Save(fileStream, false);
            }

            //renderer.PdfDocument.Save(fileName);

            Log.Info("PDF export for test {0} was successful to file '{1}'", testName, fileName);
        }

        private void DefineStyles(Document document)
        {
            var normalStyle = document.Styles["Normal"];
            normalStyle.Font.Name = "Verdana";

            // Create a new style called Table based on style Normal
            var tableStyle = document.Styles.AddStyle("Table", "Normal");
            tableStyle.Font.Name = "Verdana";
            tableStyle.Font.Size = 9;
        }

        private void ExportPlot(Section section, BenchmarkResult result)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "NUnitBenchmarker");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            var tempFile = Path.Combine(tempDirectory, string.Format("{0}.svg", Guid.NewGuid()));
            using (var fileStream = File.Create(tempFile))
            {
                var plot = PlotFactory.CreatePlotModel(result);

                var pngExporter = new SvgExporter
                {
                    Width = 600,
                    Height = 400
                };

                pngExporter.Export(plot, fileStream);
            }

            var image = section.AddImage(tempFile);
            //image.Height = "2.5cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            //image.Top = ShapePosition.Top;
            //image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;
        }

        private void ExportTable(Section section, BenchmarkResult result)
        {
            var table = section.AddTable();
            table.Style = "Table";
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            var widthLeft = 17d;

            // Before you can add a row, you must define the columns
            var column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            widthLeft -= 5;

            var columnNames = result.GetColumnNames();
            var columnCount = columnNames.Count;
            var widthPerItem = widthLeft / columnCount;

            for (int i = 0; i < columnCount; i++)
            {
                var testCaseColumn = table.AddColumn(string.Format("{0}cm", widthPerItem));
                testCaseColumn.Format.Alignment = ParagraphAlignment.Left;
            }

            // Create the header of the table
            var headerRow = table.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = Colors.LightGray;

            headerRow.Cells[0].AddParagraph("Description");
            for (int i = 0; i < columnCount; i++)
            {
                headerRow.Cells[i + 1].AddParagraph(string.Format("{0} (ms)", columnNames[i]));
            }

            // Put in the results (1 row / testcase) (a test case is a column)
            var testCaseResultRows = result.GetTestResultRows();
            foreach (var testCaseResultRow in testCaseResultRows)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(testCaseResultRow.Key);

                for (int i = 0; i < columnCount; i++)
                {
                    row.Cells[i + 1].AddParagraph(testCaseResultRow.Value[i].Value.ToString());
                }
            }
        }
    }
}