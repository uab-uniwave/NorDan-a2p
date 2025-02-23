using System.Globalization;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using ClosedXML.Excel;

namespace a2p.Shared.Application.Services
{
    public class ExcelReadService : IExcelReadService
    {

        private readonly ILogService _logService;

        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        private string _currency = string.Empty;

        public ExcelReadService(ILogService logService)
        {

            _logService = logService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }



        public async Task<List<A2PWorksheet>> GetWorksheetsAsync(A2PFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress)
        {

            XLWorkbook workbook = new(file.File);
            List<A2PWorksheet> worksheets = [];
            int worksheetCount = 0;
            try
            {
                foreach (IXLWorksheet ixlWorksheet in workbook.Worksheets)
                {

                    _progressValue.ProgressTask2 = $"Processing {ixlWorksheet.Name}. Worksheet {worksheetCount + 1} of {workbook.Worksheets.Count()}.";
                    _progressValue.ProgressTask3 = $"Searching rows...";
                    _progress?.Report(_progressValue);

                    if (ixlWorksheet == null)
                    {
                        continue;
                    }

                    if (ixlWorksheet.RowsUsed().Count() == 0)
                    {
                        continue;
                    }

                    A2PWorksheet worksheet = new()
                    {
                        WorksheetType = GetWorksheetType(file.FileName, ixlWorksheet.Name),
                        Name = ixlWorksheet.Name,
                        RowCount = ixlWorksheet.RowsUsed().Count(),
                        FileName = file.FileName
                    };

                    CultureInfo culture = CultureInfo.InvariantCulture;
                    int totalColumns = ixlWorksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
                    string tempTask3 = _progressValue.ProgressTask3;

                    IEnumerable<IXLRow> rows = ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>();

                    int totalCount = rows.Count();
                    int rowCount = 0;

                    _progressValue.ProgressTask3 = $"Found {totalCount} rows.";
                    _progress?.Report(_progressValue);

                    //iterate through all rows
                    foreach (IXLRow row in ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>())
                    {

                        _progressValue.ProgressTask3 = $"Processing row {rowCount + 1} of {totalCount}...";
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
                                double numericValue = await GetRawNumericValueAsDoubleAsync(cell, ixlWorksheet, culture);

                                if (numericValue != 0)
                                {
                                    _logService.Verbose("Extracting from cell {Cell} Numeric Value: {Double}", cell.Address.ToString() ?? "", numericValue);
                                    cell.Value = numericValue.ToString(culture);
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
                                _logService.Debug("Cell at column {Col} is empty!", cell.Address.ToString() ?? "-1");
                                rowValues.Add(string.Empty); // Add empty string for empty cells
                            }

                        }
                        worksheet.WorksheetData.Add(rowValues);
                        rowCount++;
                    }

                    worksheets.Add(worksheet);
                    worksheetCount++;
                }
                return worksheets;

            }
            catch (Exception ex)
            {
                _logService.Error(ex, "ES. Unhandled error: Reading worksheet list from file ${FileName}", file.FileName);
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

            WorksheetType worksheetType = (worksheetName.Trim().Contains("Litteralista") && fileName.Contains("CalcSapaLogic")) ||
                    (worksheetName.Trim().Contains("Price Details") && fileName?.Contains("Price_Details") == true)
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

            // SAPA V2 Positions
            return worksheetType;
        }
        private static async Task<double> GetRawNumericValueAsDoubleAsync(IXLCell cell, IXLWorksheet worksheet, CultureInfo culture)
        {
            // Get the raw value as a string

            string rawValue = await Task.Run(() =>

             GetRawNumericValue(cell, worksheet));

            return await Task.Run(() =>
            {
                // Attempt to parse the raw value as a double using the specified culture
                return !double.TryParse(rawValue, NumberStyles.Any, culture, out double result)
        ? 0
        : double.Parse(rawValue, NumberStyles.Any, culture);
            });
        }

        private static async Task<string> GetRawNumericValue(IXLCell cell, IXLWorksheet worksheet)
        {
            return await Task.Run(() =>
            { // If the cell has a value, return it as a string
                if (!cell.IsEmpty())
                {
                    // Return the unformatted numeric value directly from the cell
                    return cell.Value.ToString();
                }

                return string.Empty;
            });
        }
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

    }
}
