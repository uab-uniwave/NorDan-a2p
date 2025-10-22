using Application.DTOs;
using Application.Interfaces.Excel;
using Application.Interfaces.PrefSuite;
using Application.Models;

using Domain.Enums;

using Microsoft.Extensions.Logging;

using System.Text.RegularExpressions;
namespace Infrastructure.Services.ExcelServices
{
    public class ExcelParserTechDesign : IExcelParserTechDesign
    {
        private readonly ILogger<ExcelParserTechDesign> _logger;

        private IPrefSuiteDataService _prefSuiteDataService;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public ExcelParserTechDesign(ILogger<ExcelParserTechDesign> logger, IPrefSuiteDataService prefSuiteDataService)
        {
            _logger = logger;
            _prefSuiteDataService = prefSuiteDataService;
            _progressValue = new ProgressValue();
        }

        public async Task<List<ItemDto>> ParseItemsAsync(WorksheetDto worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue ?? new ProgressValue();
            _progress = progress;

            List<ItemDto> items = [];
            List<ErrorDto> errors = [];

            if (worksheet == null || !worksheet.WorksheetData.Any())
            {
                return items;
            }

            try
            {
                int rowCounter = 0;
                int sortOrder = -1;

                // Use the actual parsed row count to avoid mismatches between RowCount and WorksheetData
                int rowCount = worksheet.WorksheetData.Count;
                if (rowCount == 0)
                {
                    return items;
                }

                int lastRow = Math.Max(0, rowCount - 1);

                decimal totalSellingPrice = decimal.TryParse(GetCell(worksheet.WorksheetData, lastRow, 20), out decimal orderPrice) ? orderPrice : 0;
                decimal totalQuotePrice = decimal.TryParse(GetCell(worksheet.WorksheetData, lastRow, 22), out decimal orderDiscount) ? orderDiscount : 0;
                decimal discountCoeficient = 1;
                if (totalSellingPrice != 0)
                {
                    discountCoeficient = totalQuotePrice / totalSellingPrice;
                }

                for (int i = 1; i < rowCount; i++)
                {
                    ItemDto item = new();
                    try
                    {
                        // Basic row validation: ensure row exists and has expected minimum columns
                        List<object> row = worksheet.WorksheetData[i];
                        if (row == null)
                        {
                            _logger.LogWarning("{$Class}.{$Method}. Row {Row} is null. Skipping.", nameof(ExcelParserTechDesign), nameof(ParseItemsAsync), i + 1);
                            continue;
                        }

                        // Many fields below assume at least 23 columns (index 0..22). If row is shorter, skip and log.
                        const int requiredColumns = 23;
                        if (row.Count < requiredColumns)
                        {
                            _logger.LogWarning("{$Class}.{$Method}. Row {Row} has {Columns} columns but {Required} are required. Skipping.",
                                nameof(ExcelParserTechDesign), nameof(ParseItemsAsync), i + 1, row.Count, requiredColumns);

                            errors.Add(new ErrorDto()
                            {
                                OrderNumber = worksheet.OrderNumber ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.Excel_Material_Parsing,
                                Message = $"Row {i + 1} has {row.Count} columns; expected at least {requiredColumns}."
                            });

                            continue;
                        }

                        sortOrder++;
                        rowCounter++;

                        int line = i + 1;
                        _progressValue.ProgressTask3 = $"Reading row {rowCounter} of {rowCount - 2})";
                        _progress?.Report(_progressValue);

                        item.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        item.Worksheet = worksheet.Name ?? string.Empty;
                        item.OrderId = worksheet.OrderId;
                        item.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        item.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        item.SalesDocumentVersion = worksheet.SalesDocumentVersion;
                        item.Line = line;
                        item.Column = -1;
                        item.ItemName = GetCell(worksheet.WorksheetData, i, 2) ?? string.Empty;
                        item.SortOrder = sortOrder;
                        item.Description = GetCell(worksheet.WorksheetData, i, 0);
                        item.Quantity = int.TryParse(GetCell(worksheet.WorksheetData, i, 5), out int quantity) ? quantity : 0;
                        item.Width = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 3), out decimal width) ? width : 0;
                        item.Height = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 4), out decimal height) ? height : 0;
                        item.Weight = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 6), out decimal weight) ? weight : 0;
                        item.WeightGlass = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 7), out decimal weightGlass) ? weightGlass : 0;
                        item.LaborCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 17), out decimal laborCost) ? laborCost : 0;
                        item.Hours = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 18), out decimal hours) ? hours : 0;
                        item.TotalPrice = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 22), out decimal price) ? price : 0;
                        item.WorksheetType = worksheet.WorksheetType;
                        item.CurrencyCode = worksheet.Currency ?? "Unknown";

                        if (string.IsNullOrEmpty(item.ItemName))
                        {
                            _logger.LogDebug("{$Class}.{$Method}." +
                           "\nOrder {$OrderNumber}." +
                           "\nWorksheet {$WorksheetDto}." +
                           "\nLine {$Line}. ItemName name is missing." +
                           "\nItem {$Data}.",
                          nameof(ExcelParserTechDesign),
                            nameof(ParseItemsAsync),
                           item.OrderNumber ?? string.Empty,
                           item.Worksheet ?? string.Empty,
                           item.Line,
                           string.Join(",", row.Select(o => o?.ToString() ?? string.Empty)));
                            continue;
                        }

                        _progressValue.ProgressTask3 = $"ItemName {sortOrder} of {rowCount - 2} - ItemName # \"{item.ItemName}\"";
                        _progress?.Report(_progressValue);

                        decimal profileCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 8), out decimal profile) ? profile : 0;
                        decimal fittingCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 9), out decimal fitting) ? fitting : 0;
                        decimal gasketAccessoriesCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 10), out decimal gasketAccessories) ? gasketAccessories : 0;
                        decimal aluminumSheetCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 11), out decimal aluminumSheet) ? aluminumSheet : 0;
                        decimal surchargeALuProfilesCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 12), out decimal surchargeALuProfiles) ? surchargeALuProfiles : 0;
                        decimal surfaceTreatmentCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 13), out decimal surfaceTreatment) ? surfaceTreatment : 0;
                        decimal clientMaterialsCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 14), out decimal clientMaterials) ? clientMaterials : 0;
                        decimal glassCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 15), out decimal glass) ? glass : 0;
                        decimal panelCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 16), out decimal panel) ? panel : 0;
                        decimal specialCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 19), out decimal special) ? special : 0;

                        item.WeightWithoutGlass = Math.Round(item.Weight - item.WeightGlass, 4);
                        item.TotalWeight = Math.Round(item.Weight * item.Quantity, 4);
                        item.TotalWeightWithoutGlass = Math.Round(item.WeightWithoutGlass * item.Quantity, 4);
                        item.TotalWeightGlass = Math.Round(item.WeightGlass * item.Quantity, 4);
                        item.Area = Math.Round(item.Width * item.Height / 1000000, 4);
                        item.TotalArea = Math.Round(item.Area * item.Quantity, 4);

                        item.TotalHours = Math.Round(item.Hours * item.Quantity, 4);
                        item.MaterialCost = Math.Round(profileCost + fittingCost + gasketAccessoriesCost + aluminumSheetCost + surchargeALuProfilesCost + surfaceTreatmentCost + clientMaterialsCost + panelCost + glassCost, 6);
                        item.Cost = Math.Round(item.MaterialCost + item.LaborCost, 4);
                        item.TotalMaterialCost = Math.Round(item.MaterialCost * item.Quantity, 4);
                        item.TotalLaborCost = Math.Round(item.LaborCost * item.Quantity, 4);
                        item.TotalCost = Math.Round(item.Cost * item.Quantity, 4);

                        item.TotalPrice = Math.Round(item.TotalPrice / discountCoeficient, 0);
                        item.Price = item.Quantity == 0 ? 0 : Math.Round(item.TotalPrice / item.Quantity, 4);

                        item.ExchangeRateEUR = 1; //TODO': Exchange Rate 

                        item.MaterialCostEUR = Math.Round(item.MaterialCost * item.ExchangeRateEUR, 4);
                        item.TotalMaterialCostEUR = Math.Round(item.TotalMaterialCost * item.ExchangeRateEUR, 4);
                        item.TotalLaborCostEUR = Math.Round(item.TotalLaborCost * item.ExchangeRateEUR, 4);
                        item.CostEUR = Math.Round(item.Cost * item.ExchangeRateEUR, 4);
                        item.TotalCostEUR = Math.Round(item.TotalCost * item.ExchangeRateEUR, 4);
                        item.PriceEUR = Math.Round(item.Price * item.ExchangeRateEUR, 4);
                        item.TotalPriceEUR = Math.Round(item.TotalPrice * item.ExchangeRateEUR, 4);

                        items.Add(item);

                        await LogParsedItemDtoAsync(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$OrderNumber}." +
                            "\nWorksheet {$WorksheetDto}." +
                            "\nLine {$Line}" +
                            "\nItem {$ItemName}." +
                            "\nDescription {$Description}." +
                            "\nData {$Data}." +
                            "\nException {$Exception}.",
                           nameof(ExcelParserTechDesign),
                            nameof(ParseItemsAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            item.Line,
                            item.ItemName ?? string.Empty,
                            item.Description ?? string.Empty,
                            GetRowDataSafe(worksheet.WorksheetData, i),
                        ex.Message ?? string.Empty);

                        continue;
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\n{$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(ParseItemsAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    ex.Message);

                return items;
            }
        }

        // Safe cell accessor helper
        private static string GetCell(List<List<object>> sheet, int rowIdx, int colIdx)
        {
            if (sheet == null)
            {
                return string.Empty;
            }

            if (rowIdx < 0 || rowIdx >= sheet.Count)
            {
                return string.Empty;
            }

            List<object> row = sheet[rowIdx];
            return row == null ? string.Empty : colIdx < 0 || colIdx >= row.Count ? string.Empty : row[colIdx]?.ToString() ?? string.Empty;
        }

        // Safe row-to-string helper for logging
        private static string GetRowDataSafe(List<List<object>> sheet, int rowIdx)
        {
            if (sheet == null)
            {
                return string.Empty;
            }

            if (rowIdx < 0 || rowIdx >= sheet.Count)
            {
                return string.Empty;
            }

            List<object> row = sheet[rowIdx];
            return row == null ? string.Empty : string.Join(",", row.Select(o => o?.ToString() ?? string.Empty));
        }

        public async Task<List<MaterialDto>> ParseMaterialsAsync(WorksheetDto worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue ?? new ProgressValue();
            _progress = progress;

            List<MaterialDto> materials = [];
            List<ErrorDto> errors = [];

            if (worksheet == null || !worksheet.WorksheetData.Any())
            {
                return materials;
            }

            try
            {
                // Iterate FilesDto
                if (worksheet.Name == "ND_Profiles")
                {

                    materials.AddRange(await ParseProfilesAsync(worksheet));

                }
                else if (worksheet.Name == "ND_Gaskets")
                {
                    materials.AddRange(await ParseGasketsAsync(worksheet));
                }

                else if (worksheet.Name == "ND_Accessories")
                {

                    materials.AddRange(await ParseAccessoriesAsync(worksheet));

                }
                else if (worksheet.Name == "ND_Panels")
                {

                    materials.AddRange(await ParsePanelsAsync(worksheet));

                }
                else if (worksheet.Name == "ND_Glasses")
                {

                    materials.AddRange(await ParseGlassesAsync(worksheet));

                }
                else if (worksheet.Name == "ND_Others")
                {

                    materials.AddRange(await ParseOthersAsync(worksheet));

                }

                return materials;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder: {$OrderNumber}." +
                    "\nWorksheet: {$WorksheetDto}." +
                    "\nException: {$Exception} ", nameof(ExcelParserTechDesign),
                    nameof(ParseMaterialsAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return materials;
            }
        }

        private async Task<List<MaterialDto>> ParseProfilesAsync(WorksheetDto worksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialDto> materials = [];
            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    sortOrder++;
                    line = i + 1;
                    MaterialDto material = new();
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.MaterialType = MaterialType.Profiles;
                        material.WorksheetType = WorksheetType.Materials;
                        material.OrderId = worksheet.OrderId;
                        material.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        material.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        material.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        material.SalesDocumentVersion = worksheet.SalesDocumentVersion;

                        //===================================================================================================
                        material.Line = line;
                        //   material.WorksheetType = WorksheetType.Materials;
                        material.ItemName = string.Empty; // not used in profiles
                        material.SortOrder = -1; // not used in profiles

                        //===================================================================================================
                        material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();

                        //===================================================================================================
                        material.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        string result = TransformReference(material.ReferenceBase, material.SourceColor ?? string.Empty, worksheet, line);
                        if (string.IsNullOrEmpty(result))
                        {
                            _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                     "\nOrder {$OrderNumber}, " +
                                     "\nWorksheet: {$Worksheet}, " +
                                     "\nLine: {$Line}, ",
                                     nameof(ExcelParserTechDesign),
                                        nameof(ParseProfilesAsync),
                                        worksheet.OrderNumber ?? string.Empty,
                                        worksheet.Name ?? string.Empty,
                                        line);
                            continue;
                        }

                        material.Reference = result;

                        material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                        //===================================================================================================
                        material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                        //===================================================================================================
                        material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 1;
                        material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);

                        //===================================================================================================
                        material.Width = material.PackageQuantity * 1000; //used as bar length in mm
                        material.Height = 0;

                        //===================================================================================================
                        material.TotalWeight = worksheet.WorksheetData[i][11] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalWeight) ? totalWeight : 0;
                        material.Weight = material.TotalQuantity == 0 ? 0 : Math.Round(material.TotalWeight / material.TotalQuantity, 6);
                        material.RequiredWeight = Math.Round(material.Weight * material.RequiredQuantity, 6);
                        material.LeftOverWeight = Math.Round(material.TotalWeight - material.RequiredWeight, 6) < 0 ? 0 : Math.Round(material.TotalWeight - material.RequiredWeight, 6);

                        //===================================================================================================
                        material.TotalArea = worksheet.WorksheetData[i][10] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalArea) ? totalArea : 0;
                        material.Area = material.TotalQuantity == 0 ? 0 : Math.Round(material.TotalArea / material.TotalQuantity, 6);
                        material.RequiredArea = Math.Round(material.Area * material.RequiredQuantity, 6);
                        material.LeftOverArea = Math.Round(material.TotalArea - material.RequiredArea, 6) < 0 ? 0 : Math.Round(material.TotalArea - material.RequiredArea, 6);

                        //===================================================================================================
                        material.Waste = material.RequiredWeight != 0
                            ? worksheet.WorksheetData[i][9] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal lostWeight) ? lostWeight : 0 / material.RequiredWeight * 100
                            : 0;
                        //===================================================================================================                                                         
                        material.Price = decimal.TryParse(worksheet.WorksheetData[i][12].ToString(), out decimal price) ? price : 0;
                        material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][13].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                        material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);

                        //===================================================================================================
                        material.SquareMeterPrice = 0; // not used in profiles 

                        //===================================================================================================
                        material.Pallet = null;

                        //===================================================================================================

                        if (!string.IsNullOrWhiteSpace(material.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(material.SourceColor);

                            if (customColors != null)
                            {
                                material.CustomField1 = customColors.Value.Item1; // used for custom color
                                material.CustomField2 = customColors.Value.Item2;
                            }

                            else
                            {
                                material.CustomField1 = null; // not used
                                material.CustomField2 = null; // not used
                            }
                        }
                        else
                        {
                            material.CustomField1 = null; // not used
                            material.CustomField2 = null; // not used
                        }
                        material.CustomField3 = null; // not used
                        material.CustomField4 = null; // not used
                        material.CustomField5 = null; // not used

                        //===================================================================================================
                        material.MaterialType = MaterialType.Profiles;

                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Profiles {sortOrder} of {worksheet.RowCount - 5} - {material.Reference}";
                        _progress?.Report(_progressValue);

                        //===================================================================================================
                        materials.Add(material);
                        await LogParsedMaterialDtoAsync(material);

                        //===================================================================================================

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }

                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materials;
            }
            catch (Exception)
            {
                return materials;
            }

        }

        private async Task<List<MaterialDto>> ParseGasketsAsync(WorksheetDto worksheet)
        {

            int sortOrder = -1;
            int line = -1;
            List<MaterialDto> materials = [];
            List<ErrorDto> ErrorDtos = [];

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto material = new();
                    sortOrder++;
                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.MaterialType = MaterialType.Gaskets;
                        material.WorksheetType = WorksheetType.Materials;
                        material.OrderId = worksheet.OrderId;
                        material.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        material.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        material.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        material.SalesDocumentVersion = worksheet.SalesDocumentVersion;
                        //===================================================================================================
                        material.Line = line;
                        material.ItemName = null; // not used 
                        material.SortOrder = -1; // not used 

                        //===================================================================================================
                        material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                        //===================================================================================================
                        material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                        if (string.IsNullOrEmpty(material.SourceReference) && string.IsNullOrEmpty(material.SourceColor))
                        {
                            _logger.LogError("{$Class}.{$Method}. Sapa article and color are missing. Line will be skipped." +
                             "\nOrder {$OrderNumber}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nDescription {$Description}.",
                            nameof(ExcelParserTechDesign),
                            nameof(ParseGasketsAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.Description ?? string.Empty
                         );

                        }
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();

                        if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                        {
                            material.Color = "Without";
                        }

                        //===================================================================================================
                        material.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                        if (material.Color != "Without")
                        {
                            string result = TransformReference(material.ReferenceBase, material.Color, worksheet, line);
                            if (string.IsNullOrEmpty(result))
                            {
                                _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                     "\nOrder {$OrderNumber}, " +
                                     "\nWorksheet: {$Worksheet}, " +
                                     "\nLine: {$Line}, ",
                                     nameof(ExcelParserTechDesign),
                                        nameof(ParseProfilesAsync),
                                        worksheet.OrderNumber ?? string.Empty,
                                        worksheet.Name ?? string.Empty,
                                        line);
                                continue;
                            }

                            material.Reference = result;

                        }

                        else
                        {
                            string result = TransformReference(material.ReferenceBase, "", worksheet, line);
                            if (string.IsNullOrEmpty(result))
                            {

                                _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                     "\nOrder {$OrderNumber}, " +
                                     "\nWorksheet: {$Worksheet}, " +
                                     "\nLine: {$Line}, ",
                                     nameof(ExcelParserTechDesign),
                                        nameof(ParseProfilesAsync),
                                        worksheet.OrderNumber ?? string.Empty,
                                        worksheet.Name ?? string.Empty,
                                        line);
                                continue;

                            }
                            material.Reference = result;
                        }

                        material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                        //===================================================================================================
                        material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 0;
                        material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);

                        //===================================================================================================
                        if (!string.IsNullOrEmpty(worksheet.WorksheetData[i][9]?.ToString()))
                        {
                            //Extract dimmensions from gasket material description 
                            if (worksheet.WorksheetData[i][9]?.ToString()?.Contains('/') == true)
                            {
                                string[] split = worksheet.WorksheetData[i][9]?.ToString()?.Split('/') ?? Array.Empty<string>();
                                if (split.Length == 2)
                                {
                                    material.Width = decimal.TryParse(split[0], out decimal width) ? width : 0;
                                    material.Height = decimal.TryParse(split[1], out decimal height) ? height : 0;
                                }
                            }
                        }
                        else
                        {

                            material.Width = 0; // not used 
                            material.Height = 0; // not used 
                        }
                        //===================================================================================================
                        material.TotalWeight = 0; // not used 
                        material.Weight = 0; // not used 
                        material.RequiredWeight = 0; // not used 
                        material.LeftOverWeight = 0; // not used 

                        //================================================================================================================
                        material.TotalArea = 0; // not used 
                        material.Area = 0; // not used 
                        material.RequiredArea = 0; // not used 
                        material.LeftOverArea = 0; // not used 

                        //================================================================================================================
                        material.Waste = 0; // not used 

                        //=================================================================================================                                
                        material.Price = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal price) ? price : 0;
                        material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                        material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);

                        //===================================================================================================
                        material.SquareMeterPrice = 0; // not used 

                        //================================================================================================================
                        material.Pallet = null;

                        //===================================================================================================\

                        if (!string.IsNullOrWhiteSpace(material.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(material.SourceColor);

                            if (customColors != null)
                            {
                                material.CustomField1 = customColors.Value.Item1; // used for custom color
                                material.CustomField2 = customColors.Value.Item2;
                            }

                            else
                            {
                                material.CustomField1 = null; // not used
                                material.CustomField2 = null; // not used
                            }
                        }
                        else
                        {
                            material.CustomField1 = null; // not used
                            material.CustomField2 = null; // not used
                        }
                        material.CustomField3 = null; // not used 
                        material.CustomField4 = null; // not used 
                        material.CustomField5 = null; // not used 

                        //================================================================================================================
                        material.MaterialType = MaterialType.Gaskets;

                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Gaskets {sortOrder} of {worksheet.RowCount - 5} - {material.Description}";
                        _progress?.Report(_progressValue);

                        //================================================================================================================
                        materials.Add(material);

                        //================================================================================================================

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$OrderNumber}, " +
                            "\nWorksheet: {$WorksheetDto}, " +
                            "\nReference: {$Reference }, " +
                            "\nColor: {$Color }, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                              nameof(ExcelParserTechDesign),
                            nameof(ParseGasketsAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);
                        continue;
                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                }
                return materials;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\nWorksheet {$WorksheetDto}." +
                    "\n{$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(ParseGasketsAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return materials;
            }

        }

        private async Task<List<MaterialDto>> ParseAccessoriesAsync(WorksheetDto worksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialDto> materials = [];
            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto material = new();
                    sortOrder++;

                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.MaterialType = MaterialType.Piece;
                        material.WorksheetType = WorksheetType.Materials;
                        material.OrderId = worksheet.OrderId;
                        material.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        material.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        material.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        material.SalesDocumentVersion = worksheet.SalesDocumentVersion;
                        //===================================================================================================

                        //===================================================================================================
                        material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();

                        //===================================================================================================
                        material.Line = line;
                        material.ItemName = string.Empty; // not used 
                        material.SortOrder = -1; // not used           

                        //===================================================================================================
                        material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                        {
                            material.Color = "Without";
                        }

                        //=================================================================================================== 
                        material.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                        if (material.Color != "Without")
                        {
                            string result = TransformReference(material.ReferenceBase, material.Color, worksheet, line);
                            if (string.IsNullOrEmpty(result))
                            {
                                _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                     "\nOrder {$OrderNumber}, " +
                                     "\nWorksheet: {$Worksheet}, " +
                                     "\nLine: {$Line}, ",
                                     nameof(ExcelParserTechDesign),
                                        nameof(ParseAccessoriesAsync),
                                        worksheet.OrderNumber ?? string.Empty,
                                        worksheet.Name ?? string.Empty,
                                        line);
                                continue;

                            }
                            material.Reference = result;

                        }
                        else
                        {
                            string result = TransformReference(material.ReferenceBase, "", worksheet, line);
                            if (string.IsNullOrEmpty(result))
                            {

                                _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                     "\nOrder {$OrderNumber}, " +
                                     "\nWorksheet: {$Worksheet}, " +
                                     "\nLine: {$Line}, ",
                                     nameof(ExcelParserTechDesign),
                                        nameof(ParseAccessoriesAsync),
                                        worksheet.OrderNumber ?? string.Empty,
                                        worksheet.Name ?? string.Empty,
                                        line);

                            }
                            material.Reference = result;
                        }
                        material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                        //===================================================================================================
                        material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 1;
                        material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);

                        //===================================================================================================
                        material.Width = 0; // not used 
                        material.Height = 0; // not used 

                        //===================================================================================================
                        material.TotalWeight = 0; // not used 
                        material.Weight = 0; // not used 
                        material.RequiredWeight = 0; // not used 
                        material.LeftOverWeight = 0; // not used 

                        //===================================================================================================
                        material.TotalArea = 0; // not used 
                        material.Area = 0; // not used 
                        material.RequiredArea = 0; // not used 
                        material.LeftOverArea = 0; // not used 

                        //===================================================================================================
                        material.Waste = 0; // not used 

                        //===================================================================================================
                        material.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                        material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                        material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);

                        //===================================================================================================
                        material.SquareMeterPrice = 0; // not used 

                        //===================================================================================================
                        material.Pallet = string.Empty;

                        //===================================================================================================

                        if (!string.IsNullOrWhiteSpace(material.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(material.SourceColor);

                            if (customColors != null)
                            {
                                material.CustomField1 = customColors.Value.Item1; // used for custom color
                                material.CustomField2 = customColors.Value.Item2;
                            }

                            else
                            {
                                material.CustomField1 = null; // not used
                                material.CustomField2 = null; // not used
                            }
                        }
                        else
                        {
                            material.CustomField1 = null; // not used
                            material.CustomField2 = null; // not used
                        }
                        material.CustomField3 = null; // not used 
                        material.CustomField4 = null; // not used 
                        material.CustomField5 = null; // not used 

                        //===================================================================================================
                        material.MaterialType = MaterialType.Piece;

                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Accessories {sortOrder} of {worksheet.RowCount - 5} - {material.Description}";
                        _progress?.Report(_progressValue);

                        //===================================================================================================
                        materials.Add(material);

                        //===================================================================================================

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unhandled error {$Class}.{Method}." +
                           "\nOrder {$OrderNumber}, " +
                            "\nWorksheet: {$WorksheetDto}, " +
                            "\nReference: {$Reference }, " +
                            "\nColor: {$Color }, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                              nameof(ExcelParserTechDesign),
                            nameof(ParseAccessoriesAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        continue;
                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                }

                return materials;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\nWorksheet {$WorksheetDto}." +
                    "\n{$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(ParseAccessoriesAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return materials;
            }

        }

        private async Task<List<MaterialDto>> ParsePanelsAsync(WorksheetDto worksheet)
        {

            int sortOrder = -1;
            int line = -1;
            List<MaterialDto> materials = [];

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto material = new();

                    sortOrder++;

                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.MaterialType = MaterialType.Panels;
                        material.WorksheetType = WorksheetType.Materials;
                        material.OrderId = worksheet.OrderId;
                        material.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        material.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        material.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        material.SalesDocumentVersion = worksheet.SalesDocumentVersion;
                        //===================================================================================================
                        material.SourceReference = string.Empty;
                        material.SourceDescription = worksheet.WorksheetData[i][4]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2]?.ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3] == null ? null : worksheet.WorksheetData[i][2].ToString();

                        //===================================================================================================
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Panels;

                        //===================================================================================================
                        material.ItemName = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        //Reset Sort OrderNumber if new item
                        //===================================================================================================
                        if (material.ItemName != worksheet.WorksheetData[i - 1][1].ToString())
                        {
                            sortOrder = 0;
                        }
                        material.SortOrder = sortOrder;

                        //===================================================================================================                          
                        material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                        string pattern = @"\(XPS\)\s+\d{1,2}mm$";
                        Match match = Regex.Match(material.Description, pattern);

                        material.ReferenceBase = match.Success
                            ? $"LOB_XPS{match.Groups[0].Value.Replace("(XPS)", "").Replace("mm", "").Trim()}"
                            : string.Empty;

                        material.Reference = match.Success
                            ? $"LOB_XPS{match.Groups[0].Value.Replace("(XPS)", "").Replace("mm", "").Trim()}"
                            : string.Empty;

                        material.Color = match.Success ?
                        $"LOB_Surface" : string.Empty;

                        //===================================================================================================

                        if (material.Color != "LOB_Surface")
                        {

                            if (string.IsNullOrEmpty(material.Reference) && (material.Description == "1 mm aluminium sheet" || material.Description == "1mm aluminium sheet"))
                            {

                                string result = TransformReference("AluSheet1", material.SourceColor ?? string.Empty, worksheet, line);
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                       "\nOrder {$OrderNumber}, " +
                                       "\nWorksheet: {$Worksheet}, " +
                                       "\nLine: {$Line}, ",
                                       nameof(ExcelParserTechDesign),
                                          nameof(ParsePanelsAsync),
                                          worksheet.OrderNumber ?? string.Empty,
                                          worksheet.Name ?? string.Empty,
                                          line);
                                }

                                material.Reference = result;
                                material.ReferenceBase = "AluSheet1";

                            }
                            else if (string.IsNullOrEmpty(material.Reference) && (material.Description == "1.25 mm aluminium sheet" || material.Description == "1.25mm aluminium sheet"))
                            {

                                string result = TransformReference("AluSheet1.25", material.SourceColor ?? string.Empty, worksheet, line);
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                       "\nOrder {$OrderNumber}, " +
                                       "\nWorksheet: {$Worksheet}, " +
                                       "\nLine: {$Line}, ",
                                       nameof(ExcelParserTechDesign),
                                          nameof(ParsePanelsAsync),
                                          worksheet.OrderNumber ?? string.Empty,
                                          worksheet.Name ?? string.Empty,
                                          line);
                                }
                                material.Reference = result;
                                material.ReferenceBase = "AluSheet1.25";

                            }
                            else if (string.IsNullOrEmpty(material.Reference) && (material.Description == "1.5 mm aluminium sheet" || material.Description == "1.5mm aluminium sheet"))
                            {

                                string result = TransformReference("AluSheet1.5", material.SourceColor ?? string.Empty, worksheet, line);
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                                  "\nOrder {$OrderNumber}, " +
                                   "\nWorksheet: {$Worksheet}, " +
                                   "\nLine: {$Line}, ",
                                   nameof(ExcelParserTechDesign),
                                      nameof(ParsePanelsAsync),
                                      worksheet.OrderNumber ?? string.Empty,
                                      worksheet.Name ?? string.Empty,
                                      line);
                                }

                                material.Reference = result;
                                material.ReferenceBase = "AluSheet1.5";

                            }
                            else
                            {
                                string result = TransformReference(material.SourceReference ?? string.Empty, material.SourceColor ?? string.Empty, worksheet, line);

                                if (string.IsNullOrEmpty(result))
                                {

                                    _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                              "\nOrder {$OrderNumber}, " +
                                "\nWorksheet: {$Worksheet}, " +
                                "\nLine: {$Line}, ",
                                nameof(ExcelParserTechDesign),
                                   nameof(ParsePanelsAsync),
                                   worksheet.OrderNumber ?? string.Empty,
                                   worksheet.Name ?? string.Empty,
                                   line);
                                }

                                material.Reference = result;

                            }
                            material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        }
                        //===================================================================================================

                        material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;  // not used

                        //===================================================================================================
                        material.Width = decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal width) ? width : 0;
                        material.Height = decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal height) ? height : 0;

                        //===================================================================================================
                        material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                        material.PackageQuantity = 1;
                        material.TotalQuantity = material.Quantity;
                        material.RequiredQuantity = material.TotalQuantity;
                        material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 

                        //===================================================================================================
                        material.Weight = 0;// not used
                        material.TotalWeight = 0;// not used
                        material.RequiredWeight = 0;// not used
                        material.LeftOverWeight = 0;// not used

                        //===================================================================================================
                        material.Area = material.Width * material.Height;

                        material.TotalArea = material.Area * material.Quantity;

                        material.RequiredArea = material.TotalArea; // not used 
                        material.LeftOverArea = 0; // not used 

                        //===================================================================================================
                        material.Waste = 0; // not used, panels are not cut in production. Threated as piece material. 

                        //===================================================================================================
                        material.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                        material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        material.RequiredPrice = material.TotalPrice; // not used. Threated as unique piece material 
                        material.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 

                        //===================================================================================================
                        material.SquareMeterPrice = decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;

                        //===================================================================================================
                        material.Pallet = null;

                        //===================================================================================================

                        if (!string.IsNullOrWhiteSpace(material.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(material.SourceColor);

                            if (customColors != null)
                            {
                                material.CustomField1 = customColors.Value.Item1; // used for custom color
                                material.CustomField2 = customColors.Value.Item2;
                            }

                            else
                            {
                                material.CustomField1 = null; // not used
                                material.CustomField2 = null; // not used
                            }
                        }
                        else
                        {
                            material.CustomField1 = null; // not used
                            material.CustomField2 = null; // not used
                        }
                        material.CustomField3 = null; // not used
                        material.CustomField4 = null; // not used
                        material.CustomField5 = null; // not used

                        //===================================================================================================
                        material.MaterialType = MaterialType.Panels;

                        if (string.IsNullOrEmpty(material.SourceColor))
                        {
                            material.SourceColor = material.Color;

                        }

                        if (string.IsNullOrEmpty(material.SourceReference))
                        {
                            material.SourceReference = material.ReferenceBase;

                        }

                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Panels {sortOrder} of {worksheet.RowCount - 5} - {material.Description}";
                        _progress?.Report(_progressValue);

                        //===================================================================================================

                        materials.Add(material);

                        //===================================================================================================

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unhandled error {$Class}.{$Method}." +
                            "\nOrder {$OrderNumber}, " +
                            "\nWorksheet: {$WorksheetDto}, " +
                            "\nReference: {$Reference}, " +
                            "\nColor: {$Color}, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                             nameof(ExcelParserTechDesign),
                            nameof(ParsePanelsAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        continue;
                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                }

                return materials;

            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\nWorksheet {$WorksheetDto}." +
                    "\n{$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(ParsePanelsAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return materials;
            }

        }

        private async Task<List<MaterialDto>> ParseGlassesAsync(WorksheetDto worksheet)
        {
            int sortOrder = -1;
            int line = -1;

            List<MaterialDto> materials = [];

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto material = new();
                    sortOrder++;
                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.MaterialType = MaterialType.Glasses;
                        material.WorksheetType = WorksheetType.Materials;
                        material.OrderId = worksheet.OrderId;
                        material.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        material.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        material.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        material.SalesDocumentVersion = worksheet.SalesDocumentVersion;
                        //===================================================================================================
                        material.SourceReference = null;
                        material.SourceDescription = worksheet.WorksheetData[i][2]?.ToString();
                        material.SourceColor = null;
                        material.SourceColorDescription = null;
                        //===================================================================================================
                        material.Line = line;
                        //===================================================================================================
                        material.ItemName = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        //===================================================================================================
                        //Reset Sort OrderNumber if new item
                        if (material.ItemName != worksheet.WorksheetData[i - 1][1].ToString())
                        {
                            sortOrder = 0;
                        }
                        material.SortOrder = sortOrder;
                        //===================================================================================================
                        material.Description = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(material.Description))
                        {
                            _logger.LogError("{$Class}.{$Method}. Glass description is missing." +
                           "\nOrder {$OrderNumber}, " +
                           "\nWorksheet: {$WorksheetDto}, " +
                           "\nReference {$Reference}, " +
                           "\nDescription {$Description},",

                           nameof(ExcelParserTechDesign),
                           nameof(ParseGlassesAsync),
                           worksheet.OrderNumber ?? string.Empty,
                           worksheet.Name ?? string.Empty,
                           material.SourceReference ?? string.Empty,
                           material.Description ?? string.Empty
                            );
                            continue;
                        }
                        //===================================================================================================
                        string resultPredicted = await GetGlassPredictedReferenceAsync(material.Description) ?? string.Empty;
                        string resultGlassReference = string.Empty;
                        if (!string.IsNullOrEmpty(resultPredicted))
                        {
                            resultGlassReference = await GetGlassReferenceAsync(resultPredicted) ?? string.Empty;

                        }
                        if (string.IsNullOrEmpty(resultGlassReference))
                        {
                            _logger.LogError("{$Class}.{$Method}. Glass not exists in PrefSuite DB." +
                          "\nOrder {$OrderNumber}, " +
                          "\nWorksheet: {$WorksheetDto}, " +
                          "\nReference: {$Reference}, " +
                          "\nDescription: {$Color} not found. " +
                          "\nExpected Reference {$ExpectedReference} of glass",

                            nameof(ExcelParserTechDesign),
                              nameof(ParseGlassesAsync),
                              worksheet.OrderNumber ?? string.Empty,
                              worksheet.Name ?? string.Empty,
                              material.SourceReference ?? string.Empty,
                              material.SourceDescription ?? string.Empty,
                              resultPredicted);
                            continue;
                        }
                        material.ReferenceBase = resultGlassReference;
                        material.Reference = resultGlassReference;

                        //===================================================================================================
                        material.Color = "Transparent"; // not used
                        material.ColorDescription = "Transparent"; // not used

                        //================================================================================================================
                        material.Width = decimal.TryParse(worksheet.WorksheetData[i][4].ToString(), out decimal width) ? width : 0;
                        material.Height = decimal.TryParse(worksheet.WorksheetData[i][5].ToString(), out decimal height) ? height : 0;
                        //===================================================================================================
                        material.Quantity = worksheet.WorksheetData[i][3] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                        material.PackageQuantity = 0;
                        material.TotalQuantity = material.Quantity;
                        material.RequiredQuantity = material.TotalQuantity;
                        material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 

                        //================================================================================================================
                        material.Weight = decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal weight) ? weight : 0;
                        material.TotalWeight = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal totalWeight) ? totalWeight : 0;
                        material.RequiredWeight = material.TotalWeight;
                        material.LeftOverWeight = 0;// not used. Threated as unique piece material, that has no leftovers 

                        //================================================================================================================
                        material.Area = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal area) ? area : 0;
                        material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                        material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                        material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 

                        //================================================================================================================
                        material.Waste = 0; // not used, glasses are not cut in production. Threated as piece material. 

                        //===================================================================================================
                        material.Price = decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal price) ? price : 0;
                        material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        material.RequiredPrice = material.TotalPrice; // not used. Threated as unique piece material 
                        material.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 

                        //===================================================================================================
                        material.SquareMeterPrice = decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;

                        //===================================================================================================
                        material.Pallet = worksheet.WorksheetData[i][12].ToString();

                        //===================================================================================================
                        material.CustomField1 = null; // not used
                        material.CustomField2 = null; // not used 
                        material.CustomField3 = null; // not used
                        material.CustomField4 = null; // not used 
                        material.CustomField5 = null; // not used

                        //================================================================================================================
                        material.MaterialType = MaterialType.Glasses;
                        //===================================================================================================

                        _progressValue.ProgressTask3 = $"Glasses {sortOrder} of {worksheet.RowCount - 5} - {material.Description}";
                        _progress?.Report(_progressValue);

                        materials.Add(material);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unhandled error {$Class}.{Method}" +
                            "\nOrder {$OrderNumber}, " +
                            "\nWorksheet: {$WorksheetDto}, " +
                            "\nSource Reference {$SourceReference}, " +
                            "\nSource Description {$SourceDescription}, " +
                            "\nPrefSuite Reference {$ReferenceBase}," +
                            "\nPrefSuite Reference{$Reference}," +
                            "\nPrefSuite Description{$Description}," +
                            "\nException  ${Exception}",

                              nameof(ExcelParserTechDesign),
                            nameof(ParseGlassesAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceDescription ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                            ex.Message ?? string.Empty);

                        continue;
                    }

                }

                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                return materials;

            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\nWorksheet {$WorksheetDto}." +
                    "\n{$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(ParseGlassesAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return materials;
            }

        }

        private async Task<List<MaterialDto>> ParseOthersAsync(WorksheetDto worksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialDto> materials = [];

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto material = new();
                    sortOrder++;

                    line = i + 1;
                    try
                    {

                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.MaterialType = MaterialType.Piece;
                        material.WorksheetType = WorksheetType.Materials;
                        material.OrderId = worksheet.OrderId;
                        material.OrderNumber = worksheet.OrderNumber ?? string.Empty;
                        material.ProjectNumber = worksheet.ProjectNumber ?? string.Empty;
                        material.SalesDocumentNumber = worksheet.SalesDocumentNumber;
                        material.SalesDocumentVersion = worksheet.SalesDocumentVersion;
                        //===================================================================================================
                        material.Line = line;
                        material.ItemName = string.Empty;// not used in others
                        material.SortOrder = -1;// not used in others
                        material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                        //===================================================================================================
                        material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                        if ((string.IsNullOrEmpty(material.Color) && string.IsNullOrEmpty(material.ColorDescription)) || material.ColorDescription.Contains("Mill finished") || material.Color == "MF")
                        {
                            material.Color = "Without";
                        }
                        //===================================================================================================
                        material.ReferenceBase = $"ASSA_{worksheet.WorksheetData[i][1].ToString() ?? string.Empty}";
                        if (material.Color != "Without")
                        {

                            string result = TransformReference(material.ReferenceBase, material.Color, worksheet, line);
                            if (string.IsNullOrEmpty(result))
                            {
                                _logger.LogError("{$Class}.{$Method}. TechDesign article parsing failed." +
                         "\nOrder {$OrderNumber}, " +
                         "\nWorksheet: {$Worksheet}, " +
                         "\nLine: {$Line}, ",
                         nameof(ExcelParserTechDesign),
                            nameof(ParseOthersAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            line); continue;

                            }

                            material.Reference = result;
                        }

                        else
                        {
                            material.Reference = material.ReferenceBase;
                        }
                        material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                        //===================================================================================================
                        material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 1;
                        material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);

                        //===================================================================================================
                        material.Width = 0; // not used in others
                        material.Height = 0; // not used in others
                                             //================================================================================================================
                        material.TotalWeight = 0; // not used in others
                        material.Weight = 0; // not used in others
                        material.RequiredWeight = 0; // not used in others
                        material.LeftOverWeight = 0; // not used in others
                                                     //================================================================================================================
                        material.TotalArea = 0; // not used in others
                        material.Area = 0; // not used in others
                        material.RequiredArea = 0; // not used in others
                        material.LeftOverArea = 0; // not used in others
                                                   //================================================================================================================
                        material.Waste = 0; // not used in others

                        //=================================================================================================
                        material.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                        material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                        material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                        //===================================================================================================
                        material.SquareMeterPrice = 0; // not used in others
                                                       //================================================================================================================
                        material.Pallet = null; // not used in others
                                                //================================================================================================================\

                        if (!string.IsNullOrWhiteSpace(material.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(material.SourceColor);

                            if (customColors != null)
                            {
                                material.CustomField1 = customColors.Value.Item1; // used for custom color
                                material.CustomField2 = customColors.Value.Item2;
                            }

                            else
                            {
                                material.CustomField1 = null; // not used
                                material.CustomField2 = null; // not used
                            }
                        }
                        else
                        {
                            material.CustomField1 = null; // not used
                            material.CustomField2 = null; // not used
                        }
                        material.CustomField3 = null; // not used 
                        material.CustomField4 = null; // not used 
                        material.CustomField5 = null; // not used 

                        //===================================================================================================
                        material.MaterialType = MaterialType.Piece;
                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Other materials {sortOrder} of {worksheet.RowCount - 5} -  {material.ReferenceBase}_{material.Color}";
                        _progress?.Report(_progressValue);

                        materials.Add(material);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$OrderNumber}, " +
                            "\nWorksheet: {$WorksheetDto}, " +
                            "\nLine {$Line}, " +
                            "\nReference base {$ReferenceBase}, " +
                            "\nReference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  ${Exception}",
                              nameof(ExcelParserTechDesign),
                            nameof(ParseOthersAsync),
                            worksheet.OrderNumber ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            line,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        continue;
                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materials;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\nWorksheet {$WorksheetDto}." +
                    "\n{$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(ParseOthersAsync),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return materials;
            }

        }

        private async Task LogParsedItemDtoAsync(ItemDto item)
        {
            await Task.Run(() =>
            {
                _logger.LogDebug(
                    "{$Class}.{$Method} " +
                    "Mapper Sapa 2 Service: Map ItemsDto | OrderNumber : {$OrderNumber} " +
                    "WorksheetDto {WorksheetDto$} " +
                    "Line: {$Line} " +
                    "Sort order: " +
                    "ItemName : {$ItemName}  " +
                    "Sort order : {$SortOrder} " +
                    "Description : {$Description} " +
                    "Quantity : {$Quantity} " +
                    "Width : {$Width} " +
                    "Height : {$Height} " +
                    "Weight : {$Weight} " +
                    "Weight Without Glass : {$WeightWithoutGlass} " +
                    "Weight Glass : {$WeightGlass} " +
                    "Total Weight : {$TotalWeight} " +
                    "Total Weight Glass : {$TotalWeightGlass} " +
                    "Area : {$Area} " +
                    "Total Area : {$TotalArea} " +
                    "Hours : {$Hours} " +
                    "Total Hours : {$TotalHours} " +
                    "Material Cost : {$MaterialCost}" +
                    "Labor Cost : {$LaborCost} " +
                    "Cost : {$Cost} " +
                    "Total Material Cost : {$TotalMaterialCost} " +
                    "Total Labor Cost : {$TotalLaborCost} " +
                    "Total Cost : {$TotalCost} " +
                    "Price : {$Price} " +
                    "Total Price : {$TotalPrice} " +
                    "Currency Code : {$CurrencyCode} " +
                    "Exchange Rate EUR : {$ExchangeRateEUR} " +
                    "Material Cost EUR : {$MaterialCostEUR} " +
                    "Labor Cost EUR : {$LaborCostEUR} " +
                    "Cost EUR : {$CostEUR} " +
                    "Total Material Cost EUR : {$TotalMaterialCostEUR} " +
                    "Total Labor Cost EUR : {$TotalLaborCostEUR} " +
                    "Total Cost EUR : {$TotalCostEUR} " +
                    "Price EUR : {$PriceEUR} " +
                    "Total Price EUR : {$TotalPriceEUR} " +
                    "WorksheetDto Type : {$WorksheetType} ",
                    nameof(ExcelParserTechDesign),
                    nameof(LogParsedItemDtoAsync),
                    item.OrderNumber ?? string.Empty,
                    item.Worksheet ?? string.Empty,
                    item.Line,
                    item.ItemName ?? string.Empty,
                    item.SortOrder,
                    item.Description ?? string.Empty,
                    item.Quantity,
                    item.Width,
                    item.Height,
                    item.Weight,
                    item.WeightWithoutGlass,
                    item.WeightGlass,
                    item.TotalWeight,
                    item.TotalWeightGlass,
                    item.Area,
                    item.TotalArea,
                    item.Hours,
                    item.TotalHours,
                    item.MaterialCost,
                    item.LaborCost,
                    item.Cost,
                    item.TotalMaterialCost,
                    item.TotalLaborCost,
                    item.TotalCost,
                    item.Price,
                    item.TotalPrice,
                    item.CurrencyCode,
                    item.ExchangeRateEUR,
                    item.MaterialCostEUR,
                    item.LaborCostEUR,
                    item.CostEUR,
                    item.TotalMaterialCostEUR,
                    item.TotalLaborCostEUR,
                    item.TotalCostEUR,
                    item.PriceEUR,
                    item.TotalPriceEUR,
                    item.WorksheetType.ToString() ?? string.Empty
                    );

            });
        }
        private async Task LogParsedMaterialDtoAsync(MaterialDto material)
        {
            await Task.Run(() =>
            {
                _logger.LogDebug("{$Class}.{$Method} " +
                     "OrderNumber : {$OrderNumber} " +
                     "WorksheetDto {WorksheetDto$} " +
                     "Line: {$Line} " +
                     "Sort order: " +
                     "Reference : {$Reference}  " +
                     "Message : {$Message} " +
                     "Color : {$Color} " +
                     "ColorDescription : {$ColorDescription} " +
                     "Width : {$Width} " +
                     "Height : {$Height} " +
                     "Weight : {$Weight} " +
                     "Area : {$Area} " +
                     "Quantity : {$Quantity} " +
                     "PackageQuantity : {$PackageQuantity} " +
                     "TotalQuantity : {$TotalQuantity} " +
                     "RequiredQuantity : {$RequiredQuantity} " +
                     "LeftOverQuantity : {$LeftOverQuantity} " +
                     "Waste : {$Waste} " +
                     "TotalWeight : {$TotalWeight} " +
                     "RequiredWeight : {$RequiredWeight} " +
                     "LeftOverWeight : {$LeftOverWeight} " +
                     "TotalArea : {$TotalArea} " +
                     "RequiredArea : {$RequiredArea} " +
                     "LeftOverArea : {$LeftOverArea} " +
                     "Price : {$Price} " +
                     "TotalPrice : {$TotalPrice} " +
                     "RequiredPrice : {$RequiredPrice} " +
                     "LeftOverPrice : {$LeftOverPrice} " +
                     "Pallet : {$Pallet} " +
                     "MaterialType : {$MaterialType} " +
                     "CustomField1 : {$CustomField1} " +
                     "CustomField2 : {$CustomField2} " +
                     "CustomField3 : {$CustomField3} " +
                     "CustomField4 : {$CustomField4} " +
                     "CustomField5 : {$CustomField5} " +
                     "SquareMeterPrice : {$SquareMeterPrice} " +
                     "SourceReference : {$SourceReference} " +
                     "SourceDescription : {$SourceDescription} " +
                     "SourceColor : {$SourceColor} " +
                     "SourceColorDescription : {$SourceColorDescription} " +
                     "WorksheetType : {$WorksheetType} ",
                      nameof(ExcelParserTechDesign),
                    nameof(LogParsedMaterialDtoAsync),
                     material.OrderNumber ?? string.Empty,
                     material.Worksheet ?? string.Empty,
                     material.Line,
                     material.Reference ?? string.Empty,
                     material.Description ?? string.Empty,
                     material.Color ?? string.Empty,
                     material.ColorDescription ?? string.Empty,
                     material.Width,
                     material.Height,
                     material.Weight,
                     material.Area,
                     material.Quantity,
                     material.PackageQuantity,
                     material.TotalQuantity,
                     material.RequiredQuantity,
                     material.LeftOverQuantity,
                     material.Waste,
                     material.TotalWeight,
                     material.RequiredWeight,
                     material.LeftOverWeight,
                     material.TotalArea,
                     material.RequiredArea,
                     material.LeftOverArea,
                     material.Price,
                     material.TotalPrice,
                     material.RequiredPrice,
                     material.LeftOverPrice,
                     material.Pallet ?? string.Empty,
                     material.MaterialType.ToString() ?? string.Empty,
                     material.CustomField1 ?? string.Empty,
                     material.CustomField2 ?? string.Empty,
                     material.CustomField3 ?? string.Empty,
                     material.CustomField4 ?? string.Empty,
                     material.CustomField5 ?? string.Empty,
                     material.SquareMeterPrice,
                     material.SourceReference ?? string.Empty,
                     material.SourceDescription ?? string.Empty,
                     material.SourceColor ?? string.Empty,
                     material.SourceColorDescription ?? string.Empty,
                     material.WorksheetType);
            });
        }

        private async Task<string?> GetGlassPredictedReferenceAsync(string description)
        {
            string tempString = description;

            try
            {
                await Task.Run(() =>
                {

                    tempString = tempString.Split(",").First();
                    tempString = tempString.Replace("ESG_LE", "T Sel");
                    tempString = tempString.Replace("ESG_LE", "T Sel");
                    tempString = tempString.Replace("ESG_LE", "T Sel");
                    tempString = tempString.Replace("ESG_ES", "T U1.0");
                    tempString = tempString.Replace("ESG", "T");
                    tempString = tempString.Replace("LE", " Sel");
                    tempString = tempString.Replace("ES", " U1.0");

                    // Replace thickness values with glass codes
                    tempString = tempString.Replace("6.38", "33.1");
                    tempString = tempString.Replace("6.76", "33.2");
                    tempString = tempString.Replace("8.76", "44.2");
                    tempString = tempString.Replace("10.38", "55.1");
                    tempString = tempString.Replace("10.76", "55.2");
                    tempString = tempString.Replace("11.52", "55.4");
                    tempString = tempString.Replace("12.38", "66.1");
                    tempString = tempString.Replace("12.76", "66.2");
                    tempString = tempString.Replace("13.52", "66.4");
                    tempString = tempString.Replace("15.04", "66.8");

                    for (int i = 2; i <= 12; i++)
                    {
                        tempString = tempString.Replace($"-F{i}-", $"-{i}-");
                        tempString = tempString.Replace($"-F{i}", $"-{i}");
                        tempString = tempString.Replace($"F{i}-", $"{i}-");
                    }
                });
                return tempString;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nGlass description: {$GlassDescription}." +
                    "\nGlass predicted reference:  {$PredictedReference}." +
                    "\nException {$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(GetGlassPredictedReferenceAsync),
                    description ?? string.Empty,
                    tempString ?? string.Empty,
                    ex.Message);

                return tempString;
            }

        }
        private async Task<string?> GetGlassReferenceAsync(string description)
        {
            string? reference;
            try
            {

                reference = await _prefSuiteDataService.GetGlassReferenceAsync(description);

                return reference?.Trim();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nPredicted glass reference: {$PredictedReference}," +
                    "\nException {$Exception}",
               nameof(ExcelParserTechDesign),
                    nameof(GetGlassReferenceAsync),
                        description ?? string.Empty,
                    ex.Message);

                return null;

            }
        }

        private string TransformReference(string sapaReference, string sapaColor, WorksheetDto worksheet, int line)
        {

            string reference = string.Empty;
            string initialReference = sapaReference?.Trim() ?? string.Empty;
            string initialColor = sapaColor?.Trim() ?? string.Empty;

            try
            {
                // Log an error if both fields are empty
                if (string.IsNullOrEmpty(sapaReference) && string.IsNullOrEmpty(sapaColor))
                {
                    _logger.LogError("{$Class}.{$Method}. " +
                    "ErrorDto Sapa article and color are empty." +
                    "\nOrder: {$OrderNumber}, " +
                    "\nWorksheet: {$WorksheetDto}, " +
                    "\nReference: {$Reference}, " +
                    "\nColor: {$Color}.",
                    nameof(ExcelParserTechDesign),
                    nameof(TransformReference),
                    worksheet.OrderNumber ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    initialReference ?? string.Empty,
                    initialColor ?? string.Empty);

                    return string.Empty;
                }

                if (worksheet.Name is "ND_Gaskets" or "ND_Accessories")
                {
                    if (string.IsNullOrEmpty(sapaReference))
                    {

                        return sapaReference ?? string.Empty;
                    }

                    if (sapaReference.StartsWith("S"))
                    {
                        sapaReference = sapaReference[1..];
                    }

                    if (string.IsNullOrEmpty(sapaColor) && !string.IsNullOrEmpty(sapaReference))
                    {
                        if (sapaReference.StartsWith("S"))
                        {
                            sapaReference = sapaReference[1..];
                            return sapaReference;
                        }
                    }
                }

                // Processing for ND_Profiles and ND_Accessories
                if (worksheet.Name is "ND_Profiles" or "ND_Accessories" or "ND_Gaskets" or "ND_Panels")
                {
                    if (string.IsNullOrEmpty(sapaReference))
                    {

                        return sapaReference ?? string.Empty;
                    }

                    if (sapaReference.StartsWith("S"))
                    {
                        sapaReference = sapaReference[1..];
                    }

                    sapaColor = TransformColor(sapaColor ?? string.Empty);

                    if (string.IsNullOrEmpty(sapaColor))
                    {
                        reference = sapaReference;

                    }
                    else
                    {
                        // Merge the fields with a '-'
                        string merged = $"{sapaReference}-{sapaColor}";

                        // Keep only allowed characters (letters, numbers, dots, and '-')
                        reference = Regex.Replace(merged, @"[^a-zA-Z0-9.\-|]", "");
                    }

                    // Ensure the final string is not more than 25 characters
                    if (reference.Length > 25)
                    {
                        string newReference = $"*{reference[..24]}";

                        _logger.LogError("Mapper Sapa 2 Service: Warning." +
                           "Reference > 25 characters." +
                           "\nOrder: {$OrderNumber}, " +
                           "\nWorksheet: {$WorksheetDto}," +
                           "\nReference: {$Reference}," +
                           "\nColor: {$Color}," +
                           "\nGenerated PrefSuite Reference: {$PrefSuiteReference}, length:{$PrefSuiteReferenceLength}." +
                           "\nReference inserted into DB Reference {$PrefSuiteTruncatedReference}, length:{$PrefsuiteTrunctaedLength}." +
                           "\n",
                           worksheet.OrderNumber ?? string.Empty,
                           worksheet.Name ?? string.Empty,
                           initialReference ?? string.Empty,
                           initialColor ?? string.Empty,
                           reference,
                           reference.Length,
                           newReference,
                           newReference.Length);

                        reference = newReference; // Use the new reference
                        return reference;
                    }

                }

                return reference;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    "\nException {$Exception}",
                   nameof(ExcelParserTechDesign),
                    nameof(TransformReference),
                    worksheet.OrderNumber ?? string.Empty,
                    ex.Message);

                return reference;
            }

        }

        private static string TransformColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return color;
            }

            if (color.Contains("|"))
            {
                string[] complexColor = color.Split('|');
                string sideOneColor = TransformColorPart(complexColor[0].Trim());
                string sideTwoColor = TransformColorPart(complexColor[1].Trim());
                return sideOneColor + "|" + sideTwoColor;
            }
            else
            {
                return TransformColorPart(color.Trim());
            }

        }

        private static string TransformColorPart(string colorPart)
        {
            // Match LDDDD.DD0L → e.g., N8010.840F → N8010.84
            if (Regex.IsMatch(colorPart, @"^[A-Z]\d{4}\.\d{2}0[A-Z0-9]$"))
            {
                return colorPart[..^2]; // Remove 0 and last char
            }

            // Match LDDDD.DD0 → e.g., R8506.340 → R8506.34
            if (Regex.IsMatch(colorPart, @"^[A-Z]\d{4}\.\d{2}0$"))
            {
                return colorPart[..^1]; // Remove last digit
            }

            return colorPart; // Leave unchanged
        }

        private static (string, string)? SplitColors(string sourceColor)
        {
            if (string.IsNullOrWhiteSpace(sourceColor))
            {
                return null;
            }

            if (sourceColor.Contains("|"))
            {

                string[] complexColor = sourceColor.Split('|');
                (string, string)? colorParts = (complexColor.Length == 2) ?
                    (complexColor[0].Trim(), complexColor[1].Trim()) :
                    (complexColor[0].Trim(), string.Empty);

                return colorParts;
            }
            else
            {
                return null;
            }

        }
    }
}