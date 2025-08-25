// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using ClosedXML.Excel;

using System.Globalization;

namespace a2p.Shared.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {

        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        private string _currency = string.Empty;

        public ExcelService(ILogService logService, IFileService fileService)
        {

            _logService = logService;
            _fileService = fileService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task<List<A2PWorksheet>> GetWorksheetsAsync(A2PFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress)
        {

            XLWorkbook workbook = new(file.File);
            List<A2PWorksheet> worksheets = [];
            int worksheetCounter = 0;
            try
            {

                foreach (IXLWorksheet ixlWorksheet in workbook.Worksheets)
                {
                    worksheetCounter++;

                    A2PWorksheet worksheet = new()
                    {
                        Order = file.Order,
                        WorksheetType = GetWorksheetType(file.FileName, ixlWorksheet.Name),
                        Name = ixlWorksheet.Name,
                        RowCount = ixlWorksheet.RowsUsed().Count(),
                        FileName = file.FileName
                    };

                    //   CultureInfo culture = CultureInfo.InvariantCulture;
                    int totalColumns = ixlWorksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
                    string tempTask3 = _progressValue.ProgressTask3;
                    IEnumerable<IXLRow> rows = ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>();

                    int rowCounter = 0;

                    //iterate through all rows
                    foreach (IXLRow row in ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>())
                    {
                        rowCounter++;
                        _progressValue.ProgressTask3 = $"Processing row {rowCounter} of {rows.Count()}.";
                        _progress?.Report(_progressValue);

                        List<object> rowValues = [];

                        //iterate through all Columns
                        //=======================================================================================================
                        for (int col = 1; col <= totalColumns; col++) // Iterate through all columns
                        {
                            IXLCell cell = row.Cell(col); // Access the cell by column index
                            if (cell != null && !cell.IsEmpty())
                            {

                                if (string.IsNullOrEmpty(file.Currency))
                                {
                                    worksheet.Currency = GetCurrency(cell.Style.NumberFormat.Format).Trim();
                                    file.Currency = worksheet.Currency;
                                }

                                // Attempt to parse numeric values
                                double numericValue = await ParseNumberToDoubleOrZero(cell, ixlWorksheet);

                                if (numericValue != 0)
                                {
                                    _logService.Verbose("Extracting from cell {Cell} Numeric Value: {Double}", cell.Address.ToString() ?? "", numericValue);
                                    cell.Value = numericValue.ToString();
                                    rowValues.Add(cell.Value);
                                }
                                else
                                {
                                    _logService.Verbose("Extracted from cell {Cell} Text: {Text}", cell.Address.ToString() ?? "", cell.Value.ToString());
                                    rowValues.Add(cell.Value.ToString());
                                }
                            }
                            else
                            {
                                rowValues.Add(string.Empty); // Add empty string for empty cells
                            }

                        }
                        worksheet.WorksheetData.Add(rowValues);
                    }

                    worksheets.Add(worksheet);

                }
                return worksheets;

            }
            catch (Exception ex)
            {
                _logService.Error("Excel Service. Unhandled error: Reading worksheet list from file {$FileName}. Exception:{$Exception}", file.FileName, ex.Message);
                return worksheets;

            }
        }

        private WorksheetType GetWorksheetType(string fileName, string worksheetName)
        {

            if (string.IsNullOrEmpty(fileName))
            {

                return WorksheetType.Unknown;
            }

            if (string.IsNullOrEmpty(worksheetName))
            {
                return WorksheetType.Unknown;
            }

            WorksheetType worksheetType = worksheetName.Trim().Contains("Litteralista") && fileName.Contains("CalcSapaLogic") ||
                    worksheetName.Trim().Contains("Price Details") && fileName?.Contains("Price_Details") == true
                ? WorksheetType.Items
                //Materials
                //=======================================================================================================
                : (worksheetName.Trim().Contains("Sapa Accessories") ||
                                      worksheetName.Trim().Contains("Sapa Profiles") ||
                                      worksheetName.Trim().Contains("Default hardware supplier") ||
                                      worksheetName.Trim().Contains("Accessories") ||
                                      worksheetName.Trim().Contains("Accessory_summary") ||
                                      worksheetName.Trim().Contains("1") ||
                                      worksheetName.Trim().Contains("2") ||
                                      worksheetName.Trim().Contains("Others") ||
                                      worksheetName.Trim().Contains("Gaskets") ||
                                      worksheetName.Trim().Contains("Profiles")) &&
                                     (fileName?.Contains("MaterialList") == true ||
                                      fileName?.Contains("SumList") == true ||
                                      fileName?.Contains("Profile_summary") == true)
                    ? WorksheetType.Materials

                    //Glasses
                    //=======================================================================================================
                    : (worksheetName.Trim().Contains("Default glazing supplier") ||
                                                      worksheetName.Trim().Contains("Glasses") ||
                                                      worksheetName.Trim().Contains("2")) &&
                                                      (fileName?.Contains("FillingList") == true ||
                                                      fileName?.Contains("SumList") == true ||
                                                      fileName?.Contains("Glass_panel") == true)
                                    ? WorksheetType.Glasses
                                    : WorksheetType.Panels;

            //Items
            //=======================================================================================================

            // SAPA TechnoDesign Positions
            return worksheetType;
        }

        private async Task<double> ParseNumberToDoubleOrZero(IXLCell cell, IXLWorksheet worksheet) => await Task.Run(() =>
        {
            string input = cell.Value.ToString();
            bool parsed = false;

            // First try: current culture
            parsed = double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double value);

            if (!parsed)
            {
                // Try invariant culture
                parsed = double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value);
            }

            if (!parsed)
            {
                // Try common fallback cultures (optional)
                string[] fallbackCultures = new[] { "en-US", "fr-FR", "de-DE", "es-ES", "ru-RU" };
                foreach (string? cultureName in fallbackCultures)
                {
                    var culture = CultureInfo.GetCultureInfo(cultureName);
                    parsed = double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, culture, out value);
                    if (parsed)
                    {
                        break;
                    }
                }
            }

            return parsed ? value : 0;
        });


        private string GetCurrency(string customFormat)
        {

            string currency = string.Empty;

            // Simplify currency extraction logic
            string lowerFormat = customFormat.ToLower();
            if (lowerFormat.Contains("nok"))
            {
                currency = "NOK";
            }
            else if (lowerFormat.Contains("dkk"))
            {
                currency = "DKK";
            }
            else if (lowerFormat.Contains("dkr"))
            {
                currency = "DKK";
            }
            else if (lowerFormat.Contains("isk"))
            {
                currency = "ISK";
            }
            else if (lowerFormat.Contains("pln"))
            {
                currency = "PLN";
            }
            else if (lowerFormat.Contains("sek"))
            {
                currency = "SEK";
            }
            else if (lowerFormat.Contains("chf"))
            {
                currency = "CHF";
            }
            else if (lowerFormat.Contains("gbp"))
            {
                currency = "GBP";
            }
            else if (lowerFormat.Contains("eur") || lowerFormat.Contains("€"))
            {
                currency = "EUR";
            }
            else if (lowerFormat.Contains("usd") || lowerFormat.Contains("$"))
            {
                currency = "USD";
            }

            return currency;
        }



        public void WriteExcelErrorLog(string file, List<A2PError> A2PError)
        {


            List<A2PError> criticalErrors = A2PError
                .Where(e => e.Level == ErrorLevel.Fatal || e.Level == ErrorLevel.Error)
                .ToList();



            file = file.Replace(".xslx", " Errors.xslx");

            if (criticalErrors.Count > 0)
            {
                System.Data.DataTable dataTable = new();

                _ = dataTable.Columns.Add("Order", typeof(string));
                _ = dataTable.Columns.Add("Level", typeof(string));
                _ = dataTable.Columns.Add("Code", typeof(string));
                _ = dataTable.Columns.Add("Message", typeof(string));

                using (XLWorkbook workbook = new())
                {

                    foreach (A2PError error in criticalErrors)
                    {
                        _ = dataTable.Rows.Add(error.Order, error.Level.ToString(), error.Code.ToString(), error.Message);

                        _logService.Information("{$Class}.{$Method}.Log saved successfully to \"{$FileName}\"",
                    nameof(ExcelService),
                    nameof(WriteExcelErrorLog),
                    file);

                    }

                    _ = workbook.Worksheets.Add(dataTable, "LogRecords");

                    workbook.SaveAs(file);
                }

            }
        }
    }

}

