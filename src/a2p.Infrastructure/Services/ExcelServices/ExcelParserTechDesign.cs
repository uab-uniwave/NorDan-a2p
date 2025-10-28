using Application.DTOs;
using Application.Interfaces.Excel;
using Application.Interfaces.Services;
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

        public async Task<List<ItemDto>> ParseItemsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue ?? new ProgressValue();
            _progress = progress;

            List<ItemDto> itemsDto = [];
            if (worksheet == null || !worksheet.WorksheetData.Any())
            {
                return itemsDto;
            }

            try
            {
                int rowCounter = 0;
                int sortOrder = -1;
                int line = 0;

                // Use the actual parsed row count to avoid mismatches between RowCount and WorksheetData
                int rowCount = worksheet.WorksheetData.Count;
                if (rowCount == 0)
                {
                    return itemsDto;
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
                    ItemDto itemDto = new();
                    try
                    {

                        List<object> row = worksheet.WorksheetData[i];
                        sortOrder++;
                        rowCounter++;

                        line = i + 1;
                        // Basic row validation: ensure row exists and has expected minimum columns

                        if (row == null)
                        {
                            _logger.LogWarning("$Class}.{$Method}. Error parsing TechDesign items worksheet. Rows are missing. Order: {$Order},\nWorksheet: {$Worksheet}",
                                nameof(ExcelParserTechDesign), nameof(ParseItemsAsync), orderDto.OrderNumber, worksheet.Name);
                            continue;
                        }

                        const int requiredColumns = 23;
                        if (row.Count < requiredColumns)
                        {
                            _logger.LogError("{$Class}.{$Method}. Error parsing TechDesign items worksheet row! Column count less then expected!\nOrder: {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nColumns: {$Columns},\nRequired Columns: {$RequiredColumns}",
                                nameof(ExcelParserTechDesign), nameof(ParseItemsAsync), orderDto.OrderNumber, worksheet.Name, line, row.Count, requiredColumns);

                            continue;
                        }

                        _progressValue.ProgressTask3 = $"Reading row {rowCounter} of {rowCount - 2})";
                        _progress?.Report(_progressValue);

                        itemDto.OrderId = orderDto.Id;
                        itemDto.Worksheet = worksheet.Name;
                        itemDto.Line = line;
                        itemDto.ItemName = GetCell(worksheet.WorksheetData, i, 2) ?? string.Empty;
                        itemDto.SortOrder = sortOrder;
                        itemDto.Description = GetCell(worksheet.WorksheetData, i, 0);
                        itemDto.Quantity = int.TryParse(GetCell(worksheet.WorksheetData, i, 5), out int quantity) ? quantity : 0;
                        itemDto.Width = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 3), out decimal width) ? width : 0;
                        itemDto.Height = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 4), out decimal height) ? height : 0;
                        itemDto.Weight = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 6), out decimal weight) ? weight : 0;
                        itemDto.WeightGlass = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 7), out decimal weightGlass) ? weightGlass : 0m;
                        itemDto.LaborCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 17), out decimal laborCost) ? laborCost : 0m;
                        itemDto.Hours = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 18), out decimal hours) ? hours : 0m;
                        itemDto.TotalPrice = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 22), out decimal price) ? price : 0m;
                        itemDto.WorksheetType = worksheet.WorksheetType;

                        if (string.IsNullOrEmpty(itemDto.ItemName))
                        {
                            _logger.LogDebug(@"{$Class}.{$Method}.Error parsing items worksheet. ItemName name is missing.\nOrder; {$Order},\nWorksheet: {$Worksheet},\nLine {$Line}.",
                          nameof(ExcelParserTechDesign), nameof(ParseItemsAsync), orderDto.Id, itemDto.Worksheet, line);
                            continue;
                        }

                        _progressValue.ProgressTask3 = $"ItemName {sortOrder} of {rowCount - 2} - ItemName # \"{itemDto.ItemName}\"";
                        _progress?.Report(_progressValue);

                        decimal profileCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 8), out decimal profile) ? profile : 0m;
                        decimal fittingCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 9), out decimal fitting) ? fitting : 0m;
                        decimal gasketAccessoriesCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 10), out decimal gasketAccessories) ? gasketAccessories : 0m;
                        decimal aluminumSheetCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 11), out decimal aluminumSheet) ? aluminumSheet : 0;
                        decimal surchargeALuProfilesCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 12), out decimal surchargeALuProfiles) ? surchargeALuProfiles : 0m;
                        decimal surfaceTreatmentCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 13), out decimal surfaceTreatment) ? surfaceTreatment : 0m;
                        decimal clientMaterialsCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 14), out decimal clientMaterials) ? clientMaterials : 0m;
                        decimal glassCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 15), out decimal glass) ? glass : 0m;
                        decimal panelCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 16), out decimal panel) ? panel : 0m;
                        decimal specialCost = decimal.TryParse(GetCell(worksheet.WorksheetData, i, 19), out decimal special) ? special : 0m;

                        itemDto.WeightWithoutGlass = Math.Round(itemDto.Weight - itemDto.WeightGlass, 4);
                        itemDto.TotalWeight = Math.Round(itemDto.Weight * itemDto.Quantity, 4);
                        itemDto.TotalWeightWithoutGlass = Math.Round(itemDto.WeightWithoutGlass * itemDto.Quantity, 4);
                        itemDto.TotalWeightGlass = Math.Round(itemDto.WeightGlass * itemDto.Quantity, 4);
                        itemDto.Area = Math.Round(itemDto.Width * itemDto.Height / 1000000, 4);
                        itemDto.TotalArea = Math.Round(itemDto.Area * itemDto.Quantity, 4);

                        itemDto.TotalHours = Math.Round(itemDto.Hours * itemDto.Quantity, 4);
                        itemDto.MaterialCost = Math.Round(profileCost + fittingCost + gasketAccessoriesCost + aluminumSheetCost + surchargeALuProfilesCost + surfaceTreatmentCost + clientMaterialsCost + panelCost + glassCost, 6);
                        itemDto.Cost = Math.Round(itemDto.MaterialCost + itemDto.LaborCost, 4);
                        itemDto.TotalMaterialCost = Math.Round(itemDto.MaterialCost * itemDto.Quantity, 4);
                        itemDto.TotalLaborCost = Math.Round(itemDto.LaborCost * itemDto.Quantity, 4);
                        itemDto.TotalCost = Math.Round(itemDto.Cost * itemDto.Quantity, 4);

                        itemDto.TotalPrice = Math.Round(itemDto.TotalPrice / discountCoeficient, 4);
                        itemDto.Price = itemDto.Quantity == 0 ? 0 : Math.Round(itemDto.TotalPrice / itemDto.Quantity, 4);

                        itemsDto.Add(itemDto);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign items worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);
                        continue;

                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return itemsDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign profiles worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), worksheet.Name, worksheet.Name, ex.Message);
                return itemsDto;
            }
        }
        public async Task<List<MaterialDto>> ParseMaterialsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue = null, IProgress<ProgressValue>? progress = null)
        {
            List<MaterialDto> materialsDto = [];
            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign materials  worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), orderDto.OrderNumber, worksheet.Name);
                return [];
            }
            _progressValue = progressValue ?? new ProgressValue();
            _progress = progress;

            if (worksheet == null || !worksheet.WorksheetData.Any())
            {
                return materialsDto;
            }

            try
            {
                // Iterate ExcelFiles
                if (worksheet.Name == "ND_Profiles")
                {

                    materialsDto.AddRange(await ParseProfilesAsync(worksheet, orderDto));

                }
                else if (worksheet.Name == "ND_Gaskets")
                {
                    materialsDto.AddRange(await ParseGasketsAsync(worksheet, orderDto));
                }

                else if (worksheet.Name == "ND_Accessories")
                {

                    materialsDto.AddRange(await ParseAccessoriesAsync(worksheet, orderDto));

                }
                else if (worksheet.Name == "ND_Panels")
                {

                    materialsDto.AddRange(await ParsePanelsAsync(worksheet, orderDto));

                }
                else if (worksheet.Name == "ND_Glasses")
                {

                    materialsDto.AddRange(await ParseGlassesAsync(worksheet, orderDto));

                }
                else if (worksheet.Name == "ND_Others")
                {

                    materialsDto.AddRange(await ParseOthersAsync(worksheet, orderDto));

                }

                return materialsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{Method}.Unhandled error parsing TechDesign materials worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet},\nException: {$Exception} ",
                    nameof(ExcelParserTechDesign), nameof(ParseMaterialsAsync), orderDto.OrderNumber, worksheet.Name, ex.Message);
                return materialsDto;
            }
        }

        private async Task<List<MaterialDto>> ParseProfilesAsync(Worksheet worksheet, OrderDto orderDto)
        {

            int sortOrder = 0;
            int line;
            List<MaterialDto> materialsDto = [];
            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign profiles worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), orderDto.OrderNumber, worksheet.Name);
                return [];
            }
            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    sortOrder++;
                    line = i + 1;
                    MaterialDto materialDto = new();
                    try
                    {
                        materialDto.Worksheet = worksheet.Name ?? string.Empty;
                        materialDto.MaterialType = MaterialType.Profiles;
                        materialDto.WorksheetType = WorksheetType.Materials;
                        materialDto.OrderId = orderDto.Id;
                        materialDto.Line = line;
                        //==================================================================================================================================================================================================
                        materialDto.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        materialDto.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        materialDto.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        materialDto.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                        //==================================================================================================================================================================================================
                        materialDto.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        string? result = TransformReference(materialDto.ReferenceBase ?? string.Empty, materialDto.SourceColor ?? string.Empty, orderDto.OrderNumber, worksheet.Name!, line);
                        if (string.IsNullOrEmpty(result))
                        {
                            _logger.LogError(@"{$Class}.{$Method}. Error parsing TechDesign profile reference.\nOrder: {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}",
                                nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), orderDto.OrderNumber, worksheet.Name, line);
                            continue;
                        }
                        materialDto.Reference = result;
                        materialDto.Description = worksheet.WorksheetData[i][4].ToString();
                        //==================================================================================================================================================================================================
                        materialDto.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        materialDto.ColorDescription = worksheet.WorksheetData[i][3].ToString();
                        //==================================================================================================================================================================================================
                        materialDto.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        materialDto.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 1;
                        materialDto.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        materialDto.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        materialDto.LeftOverQuantity = Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6);

                        //==================================================================================================================================================================================================
                        materialDto.Width = materialDto.PackageQuantity * 1000; //used as bar length in mm
                        materialDto.Height = 0;

                        //==================================================================================================================================================================================================
                        materialDto.TotalWeight = worksheet.WorksheetData[i][11] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalWeight) ? totalWeight : 0;
                        materialDto.Weight = materialDto.TotalQuantity == 0 ? 0 : Math.Round(materialDto.TotalWeight / materialDto.TotalQuantity, 6);
                        materialDto.RequiredWeight = Math.Round(materialDto.Weight * materialDto.RequiredQuantity, 6);
                        materialDto.LeftOverWeight = Math.Round(materialDto.TotalWeight - materialDto.RequiredWeight, 6) < 0 ? 0 : Math.Round(materialDto.TotalWeight - materialDto.RequiredWeight, 6);

                        //==================================================================================================================================================================================================
                        materialDto.TotalArea = worksheet.WorksheetData[i][10] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalArea) ? totalArea : 0;
                        materialDto.Area = materialDto.TotalQuantity == 0 ? 0 : Math.Round(materialDto.TotalArea / materialDto.TotalQuantity, 6);
                        materialDto.RequiredArea = Math.Round(materialDto.Area * materialDto.RequiredQuantity, 6);
                        materialDto.LeftOverArea = Math.Round(materialDto.TotalArea - materialDto.RequiredArea, 6) < 0 ? 0 : Math.Round(materialDto.TotalArea - materialDto.RequiredArea, 6);

                        //==================================================================================================================================================================================================
                        materialDto.Waste = materialDto.RequiredWeight != 0
                            ? worksheet.WorksheetData[i][9] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal lostWeight) ? lostWeight : 0 / materialDto.RequiredWeight * 100
                            : 0;
                        //==================================================================================================================================================================================================                                                         
                        materialDto.Price = decimal.TryParse(worksheet.WorksheetData[i][12].ToString(), out decimal price) ? price : 0;
                        materialDto.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][13].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        materialDto.RequiredPrice = Math.Round(materialDto.Price * (decimal)materialDto.RequiredQuantity, 6);
                        materialDto.LeftOverPrice = Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6);

                        //==================================================================================================================================================================================================

                        if (!string.IsNullOrWhiteSpace(materialDto.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(materialDto.SourceColor);

                            if (customColors != null)
                            {
                                materialDto.CustomField1 = customColors.Value.Item1; // used for custom color
                                materialDto.CustomField2 = customColors.Value.Item2;
                            }

                        }

                        _progressValue.ProgressTask3 = $"Profiles {sortOrder} of {worksheet.RowCount - 5} - {materialDto.Reference}";
                        _progress?.Report(_progressValue);
                        materialsDto.Add(materialDto);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign profiles worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);
                        continue;

                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materialsDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign profiles worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParseProfilesAsync), worksheet.Name, worksheet.Name, ex.Message);
                return materialsDto;
            }
        }

        private async Task<List<MaterialDto>> ParseGasketsAsync(Worksheet worksheet, OrderDto orderDto)
        {

            int sortOrder = 0;
            int line;
            List<MaterialDto> materialsDto = [];
            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign gaskets worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParseGasketsAsync), orderDto.OrderNumber, worksheet.Name);
                return [];
            }

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto materialDto = new();
                    sortOrder++;
                    line = i + 1;
                    try
                    {
                        materialDto.Worksheet = worksheet.Name ?? string.Empty;
                        materialDto.MaterialType = MaterialType.Gaskets;
                        materialDto.WorksheetType = WorksheetType.Materials;
                        materialDto.OrderId = orderDto.Id;
                        materialDto.Line = line;
                        //==================================================================================================================================================================================================
                        materialDto.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        materialDto.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        materialDto.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        materialDto.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                        //==================================================================================================================================================================================================
                        materialDto.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(materialDto.Color) && (string.IsNullOrEmpty(materialDto.ColorDescription) || materialDto.ColorDescription.Contains("Without finish")))
                        {
                            materialDto.Color = "Without";
                        }

                        materialDto.ColorDescription = worksheet.WorksheetData[i][3].ToString();

                        if (string.IsNullOrEmpty(materialDto.SourceReference) && string.IsNullOrEmpty(materialDto.SourceColor))
                        {
                            _logger.LogError(@"{$Class}.{$Method}. Error parsing TechDesign gasket reference. Source reference and source color are missing. Order: {$Order},n\Worksheet: {$Worksheet},\nLine: {$Line},\nDescription: {$Description}",
                                nameof(ExcelParserTechDesign), nameof(ParseGasketsAsync), orderDto.OrderNumber, worksheet.Name, line, materialDto.SourceDescription);
                            continue;
                        }

                        //==================================================================================================================================================================================================
                        materialDto.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                        if (materialDto.Color != "Without")
                        {
                            string? result = TransformReference(materialDto.ReferenceBase, materialDto.Color, orderDto.OrderNumber, worksheet.Name!, line);
                            if (string.IsNullOrEmpty(result))
                            {
                                _logger.LogError(@"{$Class}.{$Method}. Error parsing TechDesign gasket reference. Order: {$Order},n\Worksheet: {$Worksheet},\nLine: {$Line},\nDescription: {$Description}",
                                    nameof(ExcelParserTechDesign), nameof(ParseGasketsAsync), orderDto.OrderNumber, worksheet.Name, line, materialDto.SourceDescription);
                                continue;
                            }

                            materialDto.Reference = result;

                        }

                        else
                        {
                            string? result = TransformReference(materialDto.ReferenceBase, string.Empty, orderDto.OrderNumber, worksheet.Name!, line);
                            if (string.IsNullOrEmpty(result))
                            {

                                _logger.LogError(@"{$Class}.{$Method}. Error parsing TechDesign gasket reference. Order: {$Order},n\Worksheet: {$Worksheet},\nLine: {$Line},\nDescription: {$Description}",
                                 nameof(ExcelParserTechDesign), nameof(ParseGasketsAsync), orderDto.OrderNumber, worksheet.Name, line, materialDto.SourceDescription);
                                continue;

                            }
                            materialDto.Reference = result;
                        }

                        materialDto.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                        //==================================================================================================================================================================================================
                        materialDto.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        materialDto.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 0;
                        materialDto.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        materialDto.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        materialDto.LeftOverQuantity = Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6);

                        //==================================================================================================================================================================================================
                        if (!string.IsNullOrEmpty(worksheet.WorksheetData[i][9]?.ToString()))
                        {
                            //Extract dimmensions from gasket materialDto description 
                            if (worksheet.WorksheetData[i][9]?.ToString()?.Contains('/') == true)
                            {
                                string[] split = worksheet.WorksheetData[i][9]?.ToString()?.Split('/') ?? Array.Empty<string>();
                                if (split.Length == 2)
                                {
                                    materialDto.Width = decimal.TryParse(split[0], out decimal width) ? width : 0;
                                    materialDto.Height = decimal.TryParse(split[1], out decimal height) ? height : 0;
                                }
                            }
                        }

                        //=================================================================================================                                
                        materialDto.Price = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal price) ? price : 0;
                        materialDto.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        materialDto.RequiredPrice = Math.Round(materialDto.Price * (decimal)materialDto.RequiredQuantity, 6);
                        materialDto.LeftOverPrice = Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6);

                        //==================================================================================================================================================================================================
                        materialDto.SquareMeterPrice = 0; // not used 

                        //=================================================================================================================================================================================================================================================================================================
                        materialDto.Pallet = null;

                        //==================================================================================================================================================================================================\

                        if (!string.IsNullOrWhiteSpace(materialDto.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(materialDto.SourceColor);

                            if (customColors != null)
                            {
                                materialDto.CustomField1 = customColors.Value.Item1; // used for custom color
                                materialDto.CustomField2 = customColors.Value.Item2;
                            }

                        }

                        materialDto.MaterialType = MaterialType.Gaskets;

                        //==================================================================================================================================================================================================
                        _progressValue.ProgressTask3 = $"Gaskets {sortOrder} of {worksheet.RowCount - 5} - {materialDto.Description}";
                        _progress?.Report(_progressValue);

                        //=================================================================================================================================================================================================================================================================================================
                        materialsDto.Add(materialDto);

                    }

                    catch (Exception ex)
                    {
                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign gaskets worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParseGasketsAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);
                        continue;
                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materialsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign gaskets worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParseGasketsAsync), worksheet.Name, worksheet.Name, ex.Message);
                return materialsDto;
            }
        }

        private async Task<List<MaterialDto>> ParseAccessoriesAsync(Worksheet worksheet, OrderDto orderDto)
        {

            int sortOrder = 0;
            int line;
            List<MaterialDto> materialsDto = [];
            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign accessories worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParseAccessoriesAsync), orderDto.OrderNumber, worksheet.Name);
                return [];
            }

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto materialDto = new();
                    sortOrder++;

                    line = i + 1;
                    try
                    {
                        materialDto.Worksheet = worksheet.Name ?? string.Empty;
                        materialDto.MaterialType = MaterialType.Piece;
                        materialDto.WorksheetType = WorksheetType.Materials;
                        materialDto.OrderId = orderDto.Id;
                        materialDto.Line = line;
                        //==================================================================================================================================================================================================
                        materialDto.SourceReference = worksheet.WorksheetData[i][1].ToString() == null ? null : worksheet.WorksheetData[i][1].ToString();
                        materialDto.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        materialDto.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        materialDto.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                        //==================================================================================================================================================================================================
                        materialDto.Color = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        materialDto.ColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        if (string.IsNullOrEmpty(materialDto.Color) && (string.IsNullOrEmpty(materialDto.ColorDescription) || materialDto.ColorDescription.Contains("Without finish")))
                        {
                            materialDto.Color = "Without";
                        }

                        //================================================================================================================================================================================================== 
                        materialDto.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                        if (materialDto.Color != "Without")
                        {
                            string? result = TransformReference(materialDto.ReferenceBase, materialDto.Color, orderDto.OrderNumber, worksheet.Name!, line);
                            if (string.IsNullOrEmpty(result))
                            {
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError(@"{$Class}.{$Method}. Error parsing TechDesign accessory reference.\nOrder: {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}",
                                        nameof(ExcelParserTechDesign), nameof(ParseAccessoriesAsync), orderDto.OrderNumber, worksheet.Name, line);
                                    continue;

                                }
                                materialDto.Reference = result;

                            }
                            else
                            {
                                result = TransformReference(materialDto.ReferenceBase, string.Empty, orderDto.OrderNumber, worksheet.Name!, line);
                                if (string.IsNullOrEmpty(result))
                                {

                                    if (string.IsNullOrEmpty(result))
                                    {
                                        _logger.LogError(@"{$Class}.{$Method}. Error parsing TechDesign accessory reference.\nOrder: {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}",
                                            nameof(ExcelParserTechDesign), nameof(ParseAccessoriesAsync), orderDto.OrderNumber, worksheet.Name, line);
                                        continue;
                                    }

                                }
                                materialDto.Reference = result;
                            }
                            materialDto.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                            //==================================================================================================================================================================================================
                            materialDto.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                            materialDto.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 1;
                            materialDto.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                            materialDto.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                            materialDto.LeftOverQuantity = Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6);

                            //==================================================================================================================================================================================================
                            materialDto.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                            materialDto.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                            materialDto.RequiredPrice = Math.Round(materialDto.Price * (decimal)materialDto.RequiredQuantity, 6);
                            materialDto.LeftOverPrice = Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6);
                            //==================================================================================================================================================================================================

                            if (!string.IsNullOrWhiteSpace(materialDto.SourceColor))
                            {
                                (string, string)? customColors = SplitColors(materialDto.SourceColor);

                                if (customColors != null)
                                {
                                    materialDto.CustomField1 = customColors.Value.Item1; // used for custom color
                                    materialDto.CustomField2 = customColors.Value.Item2;
                                }
                            }

                            //==================================================================================================================================================================================================
                            materialDto.MaterialType = MaterialType.Piece;

                            //==================================================================================================================================================================================================
                            _progressValue.ProgressTask3 = $"Accessories {sortOrder} of {worksheet.RowCount - 5} - {materialDto.Description}";
                            _progress?.Report(_progressValue);

                            //==================================================================================================================================================================================================
                            materialsDto.Add(materialDto);

                            //==================================================================================================================================================================================================
                        }
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign accessories worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParseAccessoriesAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);

                        continue;

                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materialsDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign accessories worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParseAccessoriesAsync), worksheet.Name, worksheet.Name, ex.Message);
                return materialsDto;
            }
        }

        private async Task<List<MaterialDto>> ParsePanelsAsync(Worksheet worksheet, OrderDto orderDto)
        {

            int sortOrder = 0;
            int line;
            List<MaterialDto> materialsDto = [];

            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign panel worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name);
                return materialsDto;
            }

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto materialDto = new();

                    sortOrder++;

                    line = i + 1;
                    try
                    {
                        materialDto.Worksheet = worksheet.Name;
                        materialDto.MaterialType = MaterialType.Panels;
                        materialDto.WorksheetType = WorksheetType.Materials;
                        materialDto.OrderId = orderDto.Id;
                        //==================================================================================================================================================================================================
                        materialDto.SourceReference = string.Empty;
                        materialDto.SourceDescription = worksheet.WorksheetData[i][4]?.ToString();
                        materialDto.SourceColor = worksheet.WorksheetData[i][2]?.ToString();
                        materialDto.SourceColorDescription = worksheet.WorksheetData[i][3] == null ? null : worksheet.WorksheetData[i][2].ToString();

                        //==================================================================================================================================================================================================
                        materialDto.Line = line;
                        materialDto.WorksheetType = WorksheetType.Panels;

                        //==================================================================================================================================================================================================
                        materialDto.ItemName = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        //Reset Sort OrderNumber if new item
                        //==================================================================================================================================================================================================
                        if (materialDto.ItemName != worksheet.WorksheetData[i - 1][1].ToString())
                        {
                            sortOrder = 0;
                        }
                        materialDto.SortOrder = sortOrder;

                        //==================================================================================================================================================================================================                          
                        materialDto.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                        string pattern = @"\(XPS\)\s+\d{1,2}mm$";
                        Match match = Regex.Match(materialDto.Description, pattern);

                        materialDto.ReferenceBase = match.Success
                            ? $"LOB_XPS{match.Groups[0].Value.Replace("(XPS)", "").Replace("mm", "").Trim()}"
                            : string.Empty;

                        materialDto.Reference = match.Success
                            ? $"LOB_XPS{match.Groups[0].Value.Replace("(XPS)", "").Replace("mm", "").Trim()}"
                            : string.Empty;

                        materialDto.Color = match.Success ?
                        $"LOB_Surface" : string.Empty;

                        //==================================================================================================================================================================================================

                        if (materialDto.Color != "LOB_Surface")
                        {

                            if (string.IsNullOrEmpty(materialDto.Reference) && (materialDto.Description == "1 mm aluminium sheet" || materialDto.Description == "1mm aluminium sheet"))
                            {

                                string? result = TransformReference("AluSheet1", materialDto.SourceColor, orderDto.OrderNumber, worksheet.Name, line);
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError("@{$Class}.{$Method}. Error parsing TechDesign panel reference!\nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}.",
                                      nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name, line);
                                }

                                materialDto.Reference = result;
                                materialDto.ReferenceBase = "AluSheet1";

                            }
                            else if (string.IsNullOrEmpty(materialDto.Reference) && (materialDto.Description == "1.25 mm aluminium sheet" || materialDto.Description == "1.25mm aluminium sheet"))
                            {

                                string? result = TransformReference("AluSheet1.25", materialDto.SourceColor, orderDto.OrderNumber, worksheet.Name, line);
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError("@{$Class}.{$Method}. TechDesign reference parsing failed!\nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}.",
                                       nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name, line);
                                }
                                materialDto.Reference = result;
                                materialDto.ReferenceBase = "AluSheet1.25";

                            }
                            else if (string.IsNullOrEmpty(materialDto.Reference) && (materialDto.Description == "1.5 mm aluminium sheet" || materialDto.Description == "1.5mm aluminium sheet"))
                            {

                                string? result = TransformReference("AluSheet1.5", materialDto.SourceColor, orderDto.OrderNumber, worksheet.Name, line);
                                if (string.IsNullOrEmpty(result))
                                {
                                    _logger.LogError(@"{$Class}.{$Method}. TechDesign reference parsing failed!\nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}.",
                                   nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name, line);
                                }

                                materialDto.Reference = result;
                                materialDto.ReferenceBase = "AluSheet1.5";

                            }
                            else
                            {
                                string? result = TransformReference(materialDto.SourceReference, materialDto.SourceColor, orderDto.OrderNumber, worksheet.Name, line);

                                if (string.IsNullOrEmpty(result))
                                {

                                    _logger.LogError(@"{$Class}.{$Method}. TechDesign article parsing failed.\nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}",
                                        nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name, line);
                                }

                                materialDto.Reference = result;

                            }
                            materialDto.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        }
                        //==================================================================================================================================================================================================

                        materialDto.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;  // not used

                        //==================================================================================================================================================================================================
                        materialDto.Width = decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal width) ? width : 0;
                        materialDto.Height = decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal height) ? height : 0;
                        //==================================================================================================================================================================================================
                        materialDto.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                        materialDto.PackageQuantity = 1;
                        materialDto.TotalQuantity = materialDto.Quantity;
                        materialDto.RequiredQuantity = materialDto.TotalQuantity;
                        //==================================================================================================================================================================================================
                        materialDto.Area = materialDto.Width * materialDto.Height;
                        materialDto.TotalArea = materialDto.Area * materialDto.Quantity;
                        //==================================================================================================================================================================================================
                        materialDto.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                        materialDto.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;

                        //==================================================================================================================================================================================================
                        materialDto.SquareMeterPrice = decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;

                        //==================================================================================================================================================================================================
                        materialDto.Pallet = null;

                        //==================================================================================================================================================================================================

                        if (!string.IsNullOrWhiteSpace(materialDto.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(materialDto.SourceColor);

                            if (customColors != null)
                            {
                                materialDto.CustomField1 = customColors.Value.Item1;
                                materialDto.CustomField2 = customColors.Value.Item2;
                            }

                        }

                        //==================================================================================================================================================================================================
                        materialDto.MaterialType = MaterialType.Panels;

                        if (string.IsNullOrEmpty(materialDto.SourceColor))
                        {
                            materialDto.SourceColor = materialDto.Color;

                        }

                        if (string.IsNullOrEmpty(materialDto.SourceReference))
                        {
                            materialDto.SourceReference = materialDto.ReferenceBase;

                        }

                        //==================================================================================================================================================================================================
                        _progressValue.ProgressTask3 = $"Panels {sortOrder} of {worksheet.RowCount - 5} - {materialDto.Description}";
                        _progress?.Report(_progressValue);

                        //==================================================================================================================================================================================================

                        materialsDto.Add(materialDto);

                        //==================================================================================================================================================================================================

                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign panels worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);

                        continue;

                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materialsDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign panels worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), worksheet.Name, worksheet.Name, ex.Message);
                return materialsDto;
            }

        }

        private async Task<List<MaterialDto>> ParseGlassesAsync(Worksheet worksheet, OrderDto orderDto)
        {
            int sortOrder = 0;
            int line;
            List<MaterialDto> materialsDto = [];
            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign glass worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), orderDto.OrderNumber, worksheet.Name);
                return materialsDto;
            }

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto materialDto = new();
                    sortOrder++;
                    line = i + 1;
                    try
                    {
                        materialDto.Worksheet = worksheet.Name ?? string.Empty;
                        materialDto.MaterialType = MaterialType.Glasses;
                        materialDto.WorksheetType = WorksheetType.Materials;
                        materialDto.OrderId = orderDto.Id;
                        //==================================================================================================================================================================================================
                        materialDto.SourceReference = null;
                        materialDto.SourceDescription = worksheet.WorksheetData[i][2]?.ToString();
                        materialDto.SourceColor = null;
                        materialDto.SourceColorDescription = null;
                        //==================================================================================================================================================================================================
                        materialDto.Line = line;
                        //==================================================================================================================================================================================================
                        materialDto.ItemName = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        //==================================================================================================================================================================================================
                        //Reset Sort OrderNumber if new item
                        if (materialDto.ItemName != worksheet.WorksheetData[i - 1][1].ToString())
                        {
                            sortOrder = 0;
                        }
                        materialDto.SortOrder = sortOrder;
                        //==================================================================================================================================================================================================
                        materialDto.Description = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(materialDto.Description))
                        {
                            _logger.LogError("@{$Class}.{$Method}. Error parsing TechDesign glass! Glass source description is missing!\nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nSapa Reference: {$Reference}.",
                           nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), orderDto.OrderNumber, worksheet.Name, line, materialDto.SourceReference);
                            continue;
                        }
                        //==================================================================================================================================================================================================
                        string resultPredicted = await GetGlassPredictedReferenceAsync(materialDto.Description) ?? string.Empty;
                        string resultGlassReference = string.Empty;
                        if (!string.IsNullOrEmpty(resultPredicted))
                        {
                            resultGlassReference = await GetGlassReferenceAsync(resultPredicted) ?? string.Empty;

                        }
                        if (string.IsNullOrEmpty(resultGlassReference))
                        {
                            _logger.LogError(@"{$Class}.{$Method}.Error finding glass in PrefSuite DB. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nReference: {$Reference},\nDescription: {$Description},\nExpected Reference: {$ExpectedReference}.",
                                nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), orderDto.OrderNumber, worksheet.Name, line, materialDto.SourceReference, materialDto.SourceDescription, resultPredicted);
                            continue;
                        }
                        materialDto.ReferenceBase = resultGlassReference;
                        materialDto.Reference = resultGlassReference;

                        //==================================================================================================================================================================================================
                        materialDto.Width = decimal.TryParse(worksheet.WorksheetData[i][4].ToString(), out decimal width) ? width : 0;
                        materialDto.Height = decimal.TryParse(worksheet.WorksheetData[i][5].ToString(), out decimal height) ? height : 0;
                        //==================================================================================================================================================================================================
                        materialDto.Quantity = worksheet.WorksheetData[i][3] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                        materialDto.PackageQuantity = 0;
                        materialDto.TotalQuantity = materialDto.Quantity;
                        materialDto.RequiredQuantity = materialDto.TotalQuantity;

                        //=================================================================================================================================================================================================================================================================================================
                        materialDto.Weight = decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal weight) ? weight : 0;
                        materialDto.TotalWeight = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal totalWeight) ? totalWeight : 0;
                        materialDto.RequiredWeight = materialDto.TotalWeight;

                        //=================================================================================================================================================================================================
                        materialDto.Area = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal area) ? area : 0;
                        materialDto.TotalArea = Math.Round(materialDto.Area * materialDto.Quantity, 6);

                        //=================================================================================================================================================================================================
                        materialDto.Price = decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal price) ? price : 0;
                        materialDto.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;

                        //=================================================================================================================================================================================================
                        materialDto.SquareMeterPrice = decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
                        //=================================================================================================================================================================================================
                        materialDto.Pallet = worksheet.WorksheetData[i][12].ToString();

                        materialDto.MaterialType = MaterialType.Glasses;
                        //=================================================================================================================================================================================================

                        _progressValue.ProgressTask3 = $"Glasses {sortOrder} of {worksheet.RowCount - 5} - {materialDto.Description}";
                        _progress?.Report(_progressValue);

                        materialsDto.Add(materialDto);

                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign glass worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);

                        continue;

                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materialsDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign glass  worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), worksheet.Name, worksheet.Name, ex.Message);
                return materialsDto;
            }

        }

        private async Task<List<MaterialDto>> ParseOthersAsync(Worksheet worksheet, OrderDto orderDto)
        {

            int sortOrder = 0;
            int line;
            List<MaterialDto> materialsDto = [];
            if (string.IsNullOrEmpty(worksheet.Name) || string.IsNullOrEmpty(orderDto.OrderNumber))
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign other materials worksheet. Worksheet name or order number are missing. \nOrder {$Order},\nWorksheet: {$Worksheet}",
                             nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), orderDto.OrderNumber, worksheet.Name);
                return materialsDto;
            }

            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialDto materialDto = new();
                    sortOrder++;

                    line = i + 1;
                    try
                    {

                        materialDto.Worksheet = worksheet.Name;
                        materialDto.MaterialType = MaterialType.Piece;
                        materialDto.WorksheetType = WorksheetType.Materials;
                        materialDto.OrderId = orderDto.Id;
                        // ================================
                        materialDto.Line = line;
                        materialDto.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        materialDto.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        materialDto.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        materialDto.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                        //==================================================================================================================================================================================================
                        materialDto.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        materialDto.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                        if ((string.IsNullOrEmpty(materialDto.Color) && string.IsNullOrEmpty(materialDto.ColorDescription)) || materialDto.ColorDescription.Contains("Mill finished") || materialDto.Color == "MF")
                        {
                            materialDto.Color = "Without";
                        }
                        //==================================================================================================================================================================================================
                        materialDto.ReferenceBase = $"ASSA_{worksheet.WorksheetData[i][1].ToString() ?? string.Empty}";
                        if (materialDto.Color != "Without")
                        {

                            string? result = TransformReference(materialDto.ReferenceBase, materialDto.Color, orderDto.OrderNumber, worksheet.Name, line);
                            if (string.IsNullOrEmpty(result))
                            {
                                _logger.LogError(@"{$Class}.{$Method}. TechDesign article parsing failed.\nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line}",
                                          nameof(ExcelParserTechDesign), nameof(ParsePanelsAsync), orderDto.OrderNumber, worksheet.Name, line);

                                continue;

                            }

                            materialDto.Reference = result;
                        }

                        else
                        {
                            materialDto.Reference = materialDto.ReferenceBase;
                        }
                        materialDto.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                        //==================================================================================================================================================================================================
                        materialDto.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        materialDto.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal packageQuantity) ? packageQuantity : 1;
                        materialDto.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal totalQuantity) ? totalQuantity : 0;
                        materialDto.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal requiredQuantity) ? requiredQuantity : 0;
                        materialDto.LeftOverQuantity = Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDto.TotalQuantity - materialDto.RequiredQuantity, 6);

                        //=================================================================================================
                        materialDto.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                        materialDto.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        materialDto.RequiredPrice = Math.Round(materialDto.Price * (decimal)materialDto.RequiredQuantity, 6);
                        materialDto.LeftOverPrice = Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDto.TotalPrice - materialDto.RequiredPrice, 6);
                        //==================================================================================================================================================================================================

                        if (!string.IsNullOrWhiteSpace(materialDto.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(materialDto.SourceColor);

                            if (customColors != null)
                            {
                                materialDto.CustomField1 = customColors.Value.Item1; // used for custom color
                                materialDto.CustomField2 = customColors.Value.Item2;
                            }

                            //==================================================================================================================================================================================================
                            materialDto.MaterialType = MaterialType.Piece;
                            //==================================================================================================================================================================================================
                            _progressValue.ProgressTask3 = $"Other materialsDto {sortOrder} of {worksheet.RowCount - 5} -  {materialDto.ReferenceBase}_{materialDto.Color}";
                            _progress?.Report(_progressValue);

                            materialsDto.Add(materialDto);

                        }
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign other materialsDto worksheet row. \nOrder {$Order},\nWorksheet: {$Worksheet},\nLine: {$Line},\nExecption: {$Exception}",
                            nameof(ExcelParserTechDesign), nameof(ParseGlassesAsync), orderDto.OrderNumber, worksheet.Name, line, ex.Message);

                        continue;

                    }

                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return materialsDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}. Unhandled error parsing TechDesign other materialsDto  worksheet!\nOrder: {$Order},\nWorksheet: {$Worksheet}.\n{$Exception}",
               nameof(ExcelParserTechDesign), nameof(ParseOthersAsync), worksheet.Name, worksheet.Name, ex.Message);
                return materialsDto;
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

        /*
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

        */
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

        private string? TransformReference(string? sapaReference, string? sapaColor, string orderNumber, string worksheet, int line)
        {

            if (worksheet == null || orderNumber == null || line == 0)
            {
                return null;
            }

            string reference = string.Empty;
            string initialReference = sapaReference?.Trim() ?? string.Empty;
            string initialColor = sapaColor?.Trim() ?? string.Empty;

            try
            {
                // Log an error if both fields are empty
                if (string.IsNullOrEmpty(sapaReference) && string.IsNullOrEmpty(sapaColor))
                {
                    _logger.LogError("@{$Class}.{$Method}. Error generating reference. Reference and color are empty. \nOrder: {$Order},\nWorksheet: {$Worksheet}, \nReference: {$Reference},\nColor: {$Color}.",
                    nameof(ExcelParserTechDesign), nameof(TransformReference), orderNumber, worksheet, initialReference, initialColor);
                    return string.Empty;
                }

                if (worksheet is "ND_Gaskets" or "ND_Accessories")
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
                if (worksheet is "ND_Profiles" or "ND_Accessories" or "ND_Gaskets" or "ND_Panels")
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

                        _logger.LogError(@"{$Class}.($Method}. Error generating reference.Reference > 25 characters!\nOrder: {$OrderNumber}\nWorksheet: {$Worksheet},\nLine: {$Line},\nReference: {$Reference},
                                                \nColor: {$Color},\nGenerated PrefSuite Reference: {$PrefSuiteReference}, length:{$PrefSuiteReferenceLength}.
                                                \nReference inserted into DB Reference {$PrefSuiteTruncatedReference}, length:{$PrefsuiteTrunctaedLength}." +
                           "\n",
                             nameof(ExcelParserTechDesign),
                           nameof(TransformReference),
                           orderNumber,
                           worksheet,
                           line,
                           initialReference,
                           initialColor,
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
                _logger.LogError(@"{$Class}.{ Method} Unhandled error generating reference!\nOrder: {$Order},\nWorksheet: {$Worksheet}\nException: {$Exception}", nameof(ExcelParserTechDesign), nameof(TransformReference), orderNumber, worksheet, ex.Message);

                return null;
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