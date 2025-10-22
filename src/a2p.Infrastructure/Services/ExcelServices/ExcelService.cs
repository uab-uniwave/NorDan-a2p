// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Application.Interfaces.Excel;
using Application.Interfaces.Files;
using Application.Models;

using ClosedXML.Excel;

using Domain.Entities;
using Domain.Enums;

using Microsoft.Extensions.Logging;

using System.Globalization;

namespace Infrastructure.Services.ExcelServices
{
    public class ExcelService : IExcelService
    {

        private readonly ILogger<ExcelService> _logger;
        private readonly IFileService _fileService;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        private string _currency = string.Empty;

        public ExcelService(ILogger<ExcelService> logger, IFileService fileService)
        {

            _logger = logger;
            _fileService = fileService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task<List<WorksheetDto>> GetWorksheetsAsync(Application.Models.FileDto file, ProgressValue progressValue, IProgress<ProgressValue>? progress)
        {

            XLWorkbook workbook = new(file.FilePath);
            List<WorksheetDto> worksheets = [];
            int worksheetCounter = 0;
            try
            {

                foreach (IXLWorksheet ixlWorksheet in workbook.Worksheets)
                {
                    worksheetCounter++;

                    WorksheetDto worksheet = new();
                    worksheet.OrderNumber = file.OrderNumber;
                    worksheet.OrderId = file.OrderId;
                    worksheet.ProjectNumber = file.ProjectNumber;
                    worksheet.SalesDocumentNumber = file.SalesDocumentNumber;
                    worksheet.SalesDocumentVersion = file.SalesDocumentVersion;
                    worksheet.WorksheetType = GetWorksheetType(file.FileName, ixlWorksheet.Name);
                    worksheet.Name = ixlWorksheet.Name;
                    worksheet.RowCount = ixlWorksheet.RowsUsed().Count();
                    worksheet.FileName = file.FileName;

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
                                    _logger.LogDebug("Extracting from cell {Cell} Numeric Value: {Double}", cell.Address.ToString() ?? "", numericValue);
                                    cell.Value = numericValue.ToString();
                                    rowValues.Add(cell.Value);
                                }
                                else
                                {
                                    _logger.LogDebug("Extracted from cell {Cell} Text: {Text}", cell.Address.ToString() ?? "", cell.Value.ToString());
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
                _logger.LogError("Excel Service. Unhandled error: Reading worksheet list from file {$FileName}. Exception:{$Exception}", file.FileName, ex.Message);
                return worksheets;

            }
        }
        private WorksheetType GetWorksheetType(string fileName, string worksheetName)
        {
            try
            {
                WorksheetType worksheetType = WorksheetType.Unknown;

                if (string.IsNullOrEmpty(fileName))
                {

                    return WorksheetType.Unknown;
                }

                if (string.IsNullOrEmpty(worksheetName))
                {
                    return WorksheetType.Unknown;
                }

                if (worksheetName.Trim().Contains("Price Details") == true && fileName?.Contains("Price_Details") == true)
                {

                    worksheetType = WorksheetType.Items;

                }

                else if (worksheetName.Trim().Equals("ND_Accessories") == true && fileName?.Contains("SumList") == true)

                {

                    worksheetType = WorksheetType.Materials;
                }
                else if (worksheetName.Trim().Equals("ND_Others") == true && fileName?.Contains("SumList") == true)

                {

                    worksheetType = WorksheetType.Materials;
                }

                else if (worksheetName.Trim().Equals("ND_Gaskets") == true && fileName?.Contains("SumList") == true)

                {

                    worksheetType = WorksheetType.Materials;
                }
                else if (worksheetName.Trim().Equals("ND_Profiles") == true && fileName?.Contains("SumList") == true)

                {

                    worksheetType = WorksheetType.Materials;
                }

                else if (worksheetName.Trim().Equals("ND_Glasses") == true && fileName?.Contains("SumList") == true)

                {

                    worksheetType = WorksheetType.Glasses;
                }

                else if (worksheetName.Trim().Equals("ND_Panels") == true && fileName?.Contains("SumList") == true)

                {

                    worksheetType = WorksheetType.Panels;
                }

                else
                {
                    _logger.LogError("Excel Service. Unable to determine worksheet type from file {$FileName} and worksheet {$WorksheetName}.", fileName, worksheetName);
                    worksheetType = WorksheetType.Unknown;
                }

                return worksheetType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Excel Service. Unhandled error: Determining worksheet type from file {$FileName} and worksheet {$WorksheetName}. Exception:{$Exception}", fileName, worksheetName, ex.Message);
                return WorksheetType.Unknown;
            }
        }

        private async Task<double> ParseNumberToDoubleOrZero(IXLCell cell, IXLWorksheet worksheet) => await Task.Run(() =>
        {
            try
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
                        CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
                        parsed = double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, culture, out value);
                        if (parsed)
                        {
                            break;
                        }
                    }
                }

                return parsed ? value : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Excel Service. Unhandled error: Parsing numeric value from cell {Cell} in worksheet {WorksheetDto}. Exception:{$Exception}", cell.Address.ToString() ?? "", worksheet.Name, ex.Message);
                return 0;
            }
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

        public void WriteExcelErrorLog(string file, List<ErrorEntity> errors)
        {

            List<ErrorEntity> criticalErrors = errors
                .Where(e => e.Level is ErrorLevel.Fatal or ErrorLevel.Error)
                .ToList();

            file = file.Replace(".xslx", " Errors.xslx");

            if (criticalErrors.Count > 0)
            {
                System.Data.DataTable dataTable = new();

                _ = dataTable.Columns.Add("OrderNumber", typeof(string));
                _ = dataTable.Columns.Add("Level", typeof(string));
                _ = dataTable.Columns.Add("Code", typeof(string));
                _ = dataTable.Columns.Add("Message", typeof(string));

                using XLWorkbook workbook = new();

                foreach (ErrorEntity error in criticalErrors)
                {
                    _ = dataTable.Rows.Add(error.OrderNumber, error.Level.ToString(), error.Code.ToString(), error.Message);

                    _logger.LogInformation(",.Log saved successfully to {FileName}", file);
                }

                _ = workbook.Worksheets.Add(dataTable, "LogRecords");

                workbook.SaveAs(file);

            }
        }
    }

}
