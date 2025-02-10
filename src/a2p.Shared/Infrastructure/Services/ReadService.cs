using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;

using ClosedXML.Excel;

using System.Globalization;


namespace a2p.Shared.Infrastructure.Services
{
    public class ReadService : IReadService
    {


        private readonly ILogService _logService;
        private ProgressValue _progressValue;

        private string _currency = string.Empty;
        //   private int _item = 0;


        public ReadService(ILogService logService)
        {
            _logService = logService;
            _progressValue = new ProgressValue();


        }



        public async Task<List<A2PWorksheet>> GetWorksheetListAsync(List<A2PFile> a2pFiles, ProgressValue progressValue, IProgress<ProgressValue>? progress)
        {
            try
            {

                _progressValue = progressValue;
                if (a2pFiles == null)
                {
                    return [];
                }

                List<A2PWorksheet> a2pWorksheetList = [];


                int fileCount = 0;
                foreach (A2PFile a2pFile in a2pFiles)
                {

                    try
                    {

                        using XLWorkbook workbook = new(a2pFile.File);

                        //progress found worksheets
                        {
                            _progressValue.ProgressTask2 = $"Found {workbook.Worksheets.Count + 1} worksheets.";
                            progress?.Report(_progressValue);
                            //Task.Delay(1000).Wait();
                        }


                        fileCount++;
                        int worksheetCount = 0;

                        foreach (IXLWorksheet worksheet in workbook.Worksheets)
                        {

                            {
                                _progressValue.ProgressTask2 = $"Processing {worksheet.Name}. Worksheet {worksheetCount + 1} of {workbook.Worksheets.Count()}.";
                                _progressValue.ProgressTask3 = $"Searching rows...";
                                progress?.Report(_progressValue);
                            }
                            //Task.Delay(1000).Wait();

                            if (worksheet == null)
                            {
                                continue;
                            }

                            if (worksheet.RowsUsed().Count() == 0)
                            {
                                continue;
                            }

                            A2PWorksheet a2pWorksheet = await GetWorksheetAsync(worksheet, [], progress);

                            if (a2pWorksheet == null)
                            {
                                continue;
                            }
                            if (a2pWorksheet.RowCount == 0)
                            {
                                continue;
                            }


                            a2pWorksheet.Order = a2pFile.Order;
                            a2pWorksheet.FileName = a2pFile.FileName;
                            a2pWorksheet.Worksheet = worksheet.Name;
                            a2pWorksheet.RowCount = worksheet.RowsUsed().Count() - 4;
                            a2pWorksheet.Type = GetWorksheetType(a2pFile.FileName, worksheet.Name);

                            if (a2pWorksheet.Type == WorksheetType.Items_Sapa_v2)
                            {
                                a2pWorksheet.Items = worksheet.RowsUsed().Count() - 2;
                                a2pWorksheet.RowCount = worksheet.RowsUsed().Count() - 2;

                            }





                            a2pWorksheetList.Add(a2pWorksheet);
                            worksheetCount++;

                        }
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex, "ES. Unhandled error: Reading worksheet list from file ${FileName}", a2pFile.FileName);
                        continue;
                    }


                }
                return a2pWorksheetList;
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "ES. Unhandled error: reading worksheet list from workbook)");
                return [];

            }
        }
        private async Task<A2PWorksheet> GetWorksheetAsync(IXLWorksheet ixlWorksheet, List<List<object>> values, IProgress<ProgressValue>? progress)
        {

            A2PWorksheet a2pWorksheet = new();

            if (ixlWorksheet == null)
            {
                _logService.Error("ES: Error getting worksheet. Worksheet is null!");
                throw new ArgumentNullException(nameof(ixlWorksheet), "Error getting worksheet. Worksheet is null!");
            }
            a2pWorksheet.Worksheet = ixlWorksheet.Name;
            a2pWorksheet.RowCount = ixlWorksheet.RowCount();
            a2pWorksheet.WorksheetData = values;


            CultureInfo culture = CultureInfo.InvariantCulture;
            int totalColumns = ixlWorksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
            string tempTask3 = _progressValue.ProgressTask3;
            int rowCount = 0;


            IEnumerable<IXLRow> rows = ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>();

            int totalCount = rows.Count();


            {

                _progressValue.ProgressTask3 = $"Found {totalCount} rows.";
                progress?.Report(_progressValue);
            }

            foreach (IXLRow row in ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>())
            {
                {

                    _progressValue.ProgressTask3 = $"Processing row {rowCount + 1} of {totalCount}...";
                    progress?.Report(_progressValue);
                }


                List<object> rowValues = [];
                for (int col = 1; col <= totalColumns; col++) // Iterate through all columns
                {
                    IXLCell cell = row.Cell(col); // Access the cell by column index
                    if (cell != null && !cell.IsEmpty())
                    {

                        if (string.IsNullOrEmpty(a2pWorksheet.Currency.Trim()))
                        {
                            a2pWorksheet.Currency = GetCurrency(cell.Style.NumberFormat.Format);
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
                        _logService.Debug("Cell at column {Col} is empty!", col);
                        rowValues.Add(string.Empty); // Add empty string for empty cells
                    }


                }
                a2pWorksheet.WorksheetData.Add(rowValues);
                rowCount++;
            }
            return a2pWorksheet;

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

            WorksheetType worksheetType = WorksheetType.Unknown;


            if (worksheetName.Trim().Contains("Litteralista") && fileName.Contains("CalcSapaLogic") == true &&
                worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Items_Sapa_v1;
            }

            else if (worksheetName.Trim().Contains("Litteralista") &&
              fileName?.Contains("CalcSapaLogic") == true &&
             worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Items_Sapa_v1;
            }

            // SAPA V1 Materials
            else if ((worksheetName.Trim().Contains("Sapa Accessories") ||
             worksheetName.Trim().Contains("Sapa Profiles") ||
             worksheetName.Trim().Contains("Default hardware supplier")) &&
             fileName?.Contains("MaterialList") == true &&
             worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Materials_Sapa_v1;
            }

            // SAPA V1 SortOrder
            else if (worksheetName.Trim().Contains("Default glazing supplier") &&
             fileName?.Contains("FillingList") == true &&
             worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Glasses_Sapa_v1;
            }

            // SAPA V1 Panels
            else if (worksheetName.Trim().Contains("Metal sheet optimization") &&
             fileName?.Contains("FillingList") == true &&
             worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Panels_Sapa_v1;
            }

            // SAPA V2 Positions
            else if (worksheetName.Trim().Contains("Price Details") &&
               fileName?.Contains("Price_Details") == true && worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Items_Sapa_v2;
            }
            else if ((worksheetName.Trim().Contains("Accessories") ||
               worksheetName.Trim().Contains("Others") ||
               worksheetName.Trim().Contains("Gaskets") ||
               worksheetName.Trim().Contains("Profiles")) &&
               (fileName?.Contains("SumList")) == true &&
               worksheetType == WorksheetType.Unknown)
            {
                worksheetType = WorksheetType.Materials_Sapa_v2;
            }
            else
            {
                worksheetType = worksheetName.Trim().Contains("Glasses") &&
                      fileName?.Contains("SumList") == true &&
                      worksheetType == WorksheetType.Unknown
                    ? WorksheetType.Glasses_Sapa_v2
                    : worksheetName.Trim().Contains("ND_Panel") &&
                         worksheetType == WorksheetType.Unknown
                     ? WorksheetType.Panels_Sapa_v2
                     : worksheetName.Trim().Contains("1")
                            && worksheetType == WorksheetType.Unknown
                         ? WorksheetType.Items_Schuco
                         : worksheetName.Trim().Contains("2") && fileName?.Contains("Profile_summary") == true && worksheetType == WorksheetType.Unknown
                                 ||
                                 worksheetName.Trim().Contains("1") && fileName?.Contains("Accessory_summary") == true && worksheetType == WorksheetType.Unknown
                             ? WorksheetType.Materials_Schuco
                             : worksheetName.Trim().Contains("1") && fileName?.Contains("Glass_panel") == true &&
                                   worksheetType == WorksheetType.Unknown
                                 ? WorksheetType.Glasses_Schuco
                                 : WorksheetType.Unknown;
            }

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


