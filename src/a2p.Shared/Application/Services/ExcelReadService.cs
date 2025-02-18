using a2p.Shared.Application.Interfaces;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using ClosedXML.Excel;

using DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage;

using System.Globalization;


namespace a2p.Shared.Application.Services
{
    public class ExcelReadService : IExcelReadService
    {


        private readonly ILogService _logService;
        private ProgressValue _progressValue;
        private readonly IFileService _fileService;
        private readonly IMapperSapaV1 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSchuco _mapperSchuco;
        private IProgress<ProgressValue>? _progress;
        private string _currency = string.Empty;
     

        public ExcelReadService(ILogService logService, IFileService fileService, IMapperSapaV1 mapperSapaV1, IMapperSapaV2 mapperSapaV2, IMapperSchuco mapperSchuco)
        {

            _progressValue = new ProgressValue();

            _logService = logService;
            _fileService = fileService;
            _progress = new Progress<ProgressValue>();
            _mapperSapaV1 = mapperSapaV1;    
            _mapperSapaV2 = mapperSapaV2; 
            _mapperSchuco = mapperSchuco;



        }



        private async Task<A2POrder> GetWorksheetsAsync(A2POrder order,  IXLWorksheet ixlWorksheet, List<List<object>> values, IProgress<ProgressValue>? progress)
        {

            foreach (var file in order.Files)
            {
                foreach (var worksheet in file.Worksheets)
                {


                    if (ixlWorksheet == null)
                    {
                        _logService.Error("Read Service: Error reading worksheet.Order {$Order}, file {$File} worksheet is null.", order.Order, worksheet.FileName);
                        return order;
                    }
                    worksheet.Worksheet = ixlWorksheet.Name;
                    worksheet.RowCount = ixlWorksheet.RowCount();
                    worksheet.WorksheetData = values;

                    CultureInfo culture = CultureInfo.InvariantCulture;
                    int totalColumns = ixlWorksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
                    string tempTask3 = _progressValue.ProgressTask3;
                    int rowCount = 0;


                    IEnumerable<IXLRow> rows = ixlWorksheet.RowsUsed() ?? Enumerable.Empty<IXLRow>();

                    int totalCount = rows.Count();


                    _progressValue.ProgressTask3 = $"Found {totalCount} rows.";
                    progress?.Report(_progressValue);

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
                                _progressValue.ProgressTask3 = $"Processing row {rowCount + 1} of {totalCount} - cell {cell.Address}";
                                progress?.Report(_progressValue);

                                if (string.IsNullOrEmpty(_currency))

                                {
                                    _currency = GetCurrency(cell.Style.NumberFormat.Format);
                                }

                                // Attempt to parse numeric values
                                double numericValue = await GetRawNumericValueAsDoubleAsync(cell, ixlWorksheet, culture);

                                if (numericValue != 0)
                                {
                                    _logService.Verbose("Read Service. Cell is Empty. Order {$Order}, " +
                                                                                     "worksheet {$Worksheet}, " +
                                                                                     "line  {$Line}, " +
                                                                                     "cell {$Cell}." +
                                                                                     " Extracting numeric value: {$Value}", order, a2pWorksheet.Worksheet, rowCount + 1, cell!.Address.ToString() ?? "", numericValue);
                                    cell!.Value = numericValue.ToString(culture);
                                    rowValues.Add(cell.Value);
                                }
                                else
                                {
                                    _logService.Verbose("Read Service. Extracted from cell {Cell} Text: {Text}", cell.Address.ToString() ?? "", cell.Value.ToString());
                                    rowValues.Add(cell.Value.ToString());
                                }
                            }
                            else
                            {
                                _logService.Verbose("Read Service. Cell is Empty. Order {$Order}, worksheet {$Worksheet}, line  {$Line}, cell {$Cell}", order.Order, a2pWorksheet.Worksheet, row.RowNumber() + 1, cell!.Address.ToString() ?? "");
                                rowValues.Add(string.Empty); // Add empty string for empty cells
                            }


                        }
                        a2pWorksheet.WorksheetData.Add(rowValues);
                        rowCount++;
                    }

                    file.Worksheets.Add(worksheet);
                }

                order.Files.Add(file);
           
            }


            A2PWorksheet a2pWorksheet = new();
           




           

            return order;

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

            //Items
            //=======================================================================================================
            if ((worksheetName.Trim().Contains("Litteralista") && fileName.Contains("CalcSapaLogic")) ||
                (worksheetName.Trim().Contains("Price Details") && fileName?.Contains("Price_Details") == true))
            {
                worksheetType = WorksheetType.Items;
            }
            //Materials
            //=======================================================================================================
            else if ((worksheetName.Trim().Contains("Sapa Accessories") ||
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
                      fileName?.Contains("Profile_summary") == true))
            {
                worksheetType = WorksheetType.Materials;
            }

            //Glasses
            //=======================================================================================================
            else if ((worksheetName.Trim().Contains("Default glazing supplier") ||
                      worksheetName.Trim().Contains("Glasses") ||
                      worksheetName.Trim().Contains("2")) &&
                      (fileName?.Contains("FillingList") == true ||
                      fileName?.Contains("SumList") == true ||
                      fileName?.Contains("Glass_panel") == true))
            {
                worksheetType = WorksheetType.Glasses;
            }
            else if ((worksheetName.Trim().Contains("Metal sheet optimization") ||
                     worksheetName.Trim().Contains("ND_Panel")) &&
                     fileName?.Contains("FillingList") == true)
            {
                worksheetType = WorksheetType.Panels;
            }

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


