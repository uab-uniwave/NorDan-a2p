
using a2p.Application.Services;
using a2p.Application.Services.MappingService;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Models;
using a2p.Domain.Respoitories;

using System.Text.RegularExpressions;
namespace a2p.Infrastructure.Services.MappingService
{
    public class MapperTechDesign : IMapperTechDesign
    {
        private readonly ILogService _logService;

        private readonly ISQLRepository _sqlRepository;
        //  private IPrefSuiteService _prefSuiteService;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public MapperTechDesign(ILogService logService, ISQLRepository sqlRepository)
        {
            _logService = logService;
            _sqlRepository = sqlRepository;
            _progressValue = new ProgressValue();
        }

        public async Task<(List<ItemEntity>, List<ErrorEntity>)> MapItemsAsync(Worksheet worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress;

            List<ItemEntity> items = [];
            List<ErrorEntity> errors = [];
            if (worksheet == null || !worksheet.WorksheetData.Any())
            {
                return (items, errors);
            }

            try
            {

                int rowCounter = 0;
                int sortOrder = -1;

                decimal totalSellingPrice = decimal.TryParse(worksheet.WorksheetData[worksheet.RowCount - 1][20].ToString(), out decimal orderPrice) ? orderPrice : 0;
                decimal totalQuotePrice = decimal.TryParse(worksheet.WorksheetData[worksheet.RowCount - 1][22].ToString(), out decimal orderDiscount) ? orderDiscount : 0;
                decimal discountCoeficient = 1;
                if (totalSellingPrice != 0)
                {
                    discountCoeficient = totalQuotePrice / totalSellingPrice;

                }

                for (int i = 1; i < worksheet.RowCount; i++)
                {
                    ItemEntity item = new();
                    try
                    {

                        sortOrder++;
                        rowCounter++;

                        int line = i + 1;
                        _progressValue.ProgressTask3 = $"Reading row {rowCounter} of {worksheet.RowCount - 2})";
                        _progress?.Report(_progressValue);

                        item.Order = worksheet.Order ?? string.Empty;
                        item.Worksheet = worksheet.Name ?? string.Empty;
                        item.Line = line;
                        item.Column = -1;
                        item.ItemName = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        item.SortOrder = sortOrder;
                        item.Description = worksheet.WorksheetData[i][0].ToString();
                        item.Quantity = int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 0;
                        item.Width = decimal.TryParse(worksheet.WorksheetData[i][3].ToString(), out decimal width) ? width : 0;
                        item.Height = decimal.TryParse(worksheet.WorksheetData[i][4].ToString(), out decimal height) ? height : 0;
                        item.Weight = decimal.TryParse(worksheet.WorksheetData[i][6].ToString(), out decimal weight) ? weight : 0;
                        item.WeightGlass = decimal.TryParse(worksheet.WorksheetData[i][7].ToString(), out decimal weightGlass) ? weightGlass : 0;
                        item.LaborCost = decimal.TryParse(worksheet.WorksheetData[i][17].ToString(), out decimal laborCost) ? laborCost : 0;
                        item.Hours = decimal.TryParse(worksheet.WorksheetData[i][18].ToString(), out decimal hours) ? hours : 0;
                        item.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][22].ToString(), out decimal price) ? price : 0;
                        item.WorksheetType = worksheet.WorksheetType;
                        item.CurrencyCode = worksheet.Currency ?? "Unknown";

                        if (string.IsNullOrEmpty(item.ItemName))
                        {
                            _logService.Debug("{$Class}.{$Method}." +
                           "\nOrder {$Order}." +
                           "\nWorksheet {$Worksheet}." +
                           "\nLine {$Line}. Item name is missing." +
                           "\nItem {$Data}.",
                          nameof(MapperTechDesign),
                            nameof(MapItemsAsync),
                           item.Order ?? string.Empty,
                           item.Worksheet ?? string.Empty,
                           item.Line,
                           worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty);
                            continue;

                        }
                        _progressValue.ProgressTask3 = $"Item {sortOrder} of {worksheet.RowCount - 2} - Item # \"{item.ItemName}\"";
                        _progress?.Report(_progressValue);

                        decimal profileCost = decimal.TryParse(worksheet.WorksheetData[i][8].ToString(), out decimal profile) ? profile : 0;
                        decimal fittingCost = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal fitting) ? fitting : 0;
                        decimal gasketAccessoriesCost = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal gasketAccessories) ? gasketAccessories : 0;
                        decimal aluminumSheetCost = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal aluminumSheet) ? aluminumSheet : 0;
                        decimal surchargeALuProfilesCost = decimal.TryParse(worksheet.WorksheetData[i][12].ToString(), out decimal surchargeALuProfiles) ? surchargeALuProfiles : 0;
                        decimal surfaceTreatmentCost = decimal.TryParse(worksheet.WorksheetData[i][13].ToString(), out decimal surfaceTreatment) ? surfaceTreatment : 0;
                        decimal clientMaterialsCost = decimal.TryParse(worksheet.WorksheetData[i][14].ToString(), out decimal clientMaterials) ? clientMaterials : 0;
                        decimal glassCost = decimal.TryParse(worksheet.WorksheetData[i][15].ToString(), out decimal glass) ? glass : 0;
                        decimal panelCost = decimal.TryParse(worksheet.WorksheetData[i][16].ToString(), out decimal panel) ? panel : 0;
                        decimal specialCost = decimal.TryParse(worksheet.WorksheetData[i][19].ToString(), out decimal special) ? special : 0;

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
                        item.Price = Math.Round(item.TotalPrice / item.Quantity, 4);

                        item.ExchangeRateEUR = 1; //TODO': Exchange Rate 

                        item.MaterialCostEUR = Math.Round(item.MaterialCost * item.ExchangeRateEUR, 4);
                        item.TotalMaterialCostEUR = Math.Round(item.TotalMaterialCost * item.ExchangeRateEUR, 4);
                        item.TotalLaborCostEUR = Math.Round(item.TotalLaborCost * item.ExchangeRateEUR, 4);
                        item.CostEUR = Math.Round(item.Cost * item.ExchangeRateEUR, 4);
                        item.TotalCostEUR = Math.Round(item.TotalCost * item.ExchangeRateEUR, 4);
                        item.PriceEUR = Math.Round(item.Price * item.ExchangeRateEUR, 4);
                        item.TotalPriceEUR = Math.Round(item.TotalPrice * item.ExchangeRateEUR, 4);

                        // item.WorksheetType = WorksheetType.Items;
                        items.Add(item);

                        await LogMappedItemEntityAsync(item);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$Order}." +
                            "\nWorksheet {$Worksheet}." +
                            "\nLine {$Line}" +
                            "\nItem {$Item}." +
                            "\nDescription {$Description}." +
                            "\nData {$Data}." +
                            "\nException {$Exception}.",
                           nameof(MapperTechDesign),
                            nameof(MapItemsAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            item.Line,
                            item.ItemName ?? string.Empty,
                            item.Description ?? string.Empty,
                            worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty,
                        ex.Message ?? string.Empty);

                        errors.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapItemsAsync)}, " +
                          $"\nOrder: {worksheet.Order ?? string.Empty}," +
                          $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                          $"\nLine {item.Line}," +
                          $"\nItem: {item.ItemName ?? string.Empty}," +
                          $"\nDescription: {item.Description ?? string.Empty}," +
                          $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                          $"\nException: {ex.Message ?? string.Empty}."
                        });

                        continue;
                    }
                }

                return (items, errors);
            }

            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapItemsAsync),
                    worksheet.Order ?? string.Empty,
                    ex.Message);

                return (items, errors);
                ;
            }

        }

        public async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapMaterialsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress;

            List<MaterialEntity> materials = [];
            List<ErrorEntity> errors = [];
            if (worksheet == null || !worksheet.WorksheetData.Any())
            {
                return (materials, errors);
            }

            try
            {

                //=============================================================================================================
                // Iterate Files
                // =============================================================================================================

                if (worksheet.Name == "ND_Profiles")
                {
                    (List<MaterialEntity>, List<ErrorEntity>) result = await MapProfilesAsync(worksheet);
                    if (result.Item1 != null)
                    {
                        materials.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        errors.AddRange(result.Item2);
                    }

                }
                else if (worksheet.Name == "ND_Gaskets")
                {
                    (List<MaterialEntity>, List<ErrorEntity>) result = await MapGasketsAsync(worksheet);
                    if (result.Item1 != null)
                    {
                        materials.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        errors.AddRange(result.Item2);
                    }

                }

                else if (worksheet.Name == "ND_Accessories")
                {
                    (List<MaterialEntity>, List<ErrorEntity>) result = await MapAccessoriesAsync(worksheet);
                    if (result.Item1 != null)
                    {
                        materials.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        errors.AddRange(result.Item2);
                    }
                }

                else if (worksheet.Name == "ND_Panels")
                {
                    (List<MaterialEntity>, List<ErrorEntity>) result = await MapPanelsAsync(worksheet);
                    if (result.Item1 != null)
                    {
                        materials.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        errors.AddRange(result.Item2);
                    }

                }
                else if (worksheet.Name == "ND_Glasses")
                {
                    (List<MaterialEntity>, List<ErrorEntity>) result = await MapGlassesAsync(worksheet);
                    if (result.Item1 != null)
                    {
                        materials.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        errors.AddRange(result.Item2);
                    }
                }
                else if (worksheet.Name == "ND_Others")
                {
                    (List<MaterialEntity>, List<ErrorEntity>) result = await MapOthersAsync(worksheet);
                    if (result.Item1 != null)
                    {
                        materials.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        errors.AddRange(result.Item2);
                    }
                }

                return (materials, errors);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapMaterialsAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                errors.Add(new ErrorEntity()
                {
                    OrderNumber = worksheet.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.MappingService_MapMaterial,
                    Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapMaterialsAsync)}, " +
                       $"\nOrder: {worksheet.Order ?? string.Empty}," +
                       $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                       $"\nException: {ex.Message ?? string.Empty}."
                });

                return (materials, errors);
            }

        }

        private async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapProfilesAsync(Worksheet worksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialEntity> materials = [];
            List<ErrorEntity> ErrorEntitys = [];
            try
            {

                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    sortOrder++;
                    line = i + 1;
                    MaterialEntity material = new();
                    try
                    {
                        //===================================================================================================
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Materials;
                        material.Item = null; // not used in profiles
                        material.SortOrder = -1; // not used in profiles

                        //===================================================================================================
                        material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();

                        //===================================================================================================
                        material.ReferenceBase = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        (string, ErrorEntity?) result = TransformReference(material.ReferenceBase, material.SourceColor ?? string.Empty, worksheet, line);
                        if (string.IsNullOrEmpty(result.Item1))
                        {
                            continue;
                        }

                        material.Reference = result.Item1;

                        if (result.Item2 != null)
                        {
                            ErrorEntitys.Add(result.Item2);
                        }

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

                        //===================================================================================================
                        await LogMappedMaterialEntityAsync(material);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nReference: {$Reference }, " +
                            "\nColor: {$Color }, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                            nameof(MapperTechDesign),
                            nameof(MapProfilesAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                            ex.Message ?? string.Empty);


                        ErrorEntitys.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapProfilesAsync)}, " +
                           $"\nOrder: {worksheet.Order ?? string.Empty}," +
                           $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                           $"\nLine {material.Line}," +
                           $"\nItem: {material.Item ?? string.Empty}," +
                           $"\nDescription: {material.Description ?? string.Empty}," +
                           $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                           $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }

                }

                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return (materials, ErrorEntitys);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapProfilesAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return (materials, ErrorEntitys);
            }

        }

        private async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapGasketsAsync(Worksheet worksheet)
        {

            int sortOrder = -1;
            int line = -1;
            List<MaterialEntity> materials = [];
            List<ErrorEntity> ErrorEntitys = [];

            try
            {



                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialEntity material = new();
                    sortOrder++;
                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.Order = worksheet.Order ?? string.Empty;
                        //===================================================================================================
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Materials;
                        material.Item = null; // not used 
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
                            _logService.Error("{$Class}.{$Method}. Sapa article and color are missing. Line will be skipped." +
                              "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nDescription {$Description}," +
                            nameof(MapperTechDesign),
                            nameof(MapGasketsAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.Description ?? string.Empty
                         );

                            ErrorEntitys.Add(new ErrorEntity()
                            {
                                OrderNumber = worksheet.Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Sapa article and color are missing. Line will be skipped." +
                               $"\nOrder: {worksheet.Order ?? string.Empty}," +
                               $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                               $"\nDescription: {material.Description ?? string.Empty}," +
                               $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}"
                            });
                            continue;
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
                            (string, ErrorEntity?) result = TransformReference(material.ReferenceBase, material.Color, worksheet, line);
                            if (string.IsNullOrEmpty(result.Item1))
                            {
                                continue;
                            }

                            material.Reference = result.Item1;
                            if (result.Item2 != null)
                            {
                                ErrorEntitys.Add(result.Item2);
                            }

                        }

                        else
                        {
                            (string, ErrorEntity?) result = TransformReference(material.ReferenceBase, "", worksheet, line);
                            if (string.IsNullOrEmpty(result.Item1))
                            {
                                continue;

                            }
                            material.Reference = result.Item1;
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
                        await LogMappedMaterialEntityAsync(material);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nReference: {$Reference }, " +
                            "\nColor: {$Color }, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                              nameof(MapperTechDesign),
                            nameof(MapGasketsAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);
                        ErrorEntitys.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapGasketsAsync)}, " +
                           $"\nOrder: {worksheet.Order ?? string.Empty}," +
                           $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                           $"\nLine {material.Line}," +
                           $"\nItem: {material.Item ?? string.Empty}," +
                           $"\nDescription: {material.Description ?? string.Empty}," +
                           $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                           $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }



                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                }
                return (materials, ErrorEntitys);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapGasketsAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return (materials, ErrorEntitys);
            }

        }

        private async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapAccessoriesAsync(Worksheet worksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<ErrorEntity> ErrorEntitys = [];
            List<MaterialEntity> materials = [];
            try
            {



                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialEntity material = new();
                    sortOrder++;

                    line = i + 1;
                    try
                    {

                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.Order = worksheet.Order ?? string.Empty;


                        //===================================================================================================
                        material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                        material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();

                        //===================================================================================================
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Materials;
                        material.Item = null; // not used 
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
                            (string, ErrorEntity?) result = TransformReference(material.ReferenceBase, material.Color, worksheet, line);
                            if (string.IsNullOrEmpty(result.Item1))
                            {
                                continue;
                            }
                            material.Reference = result.Item1;
                            if (result.Item2 != null)
                            {
                                ErrorEntitys.Add(result.Item2);
                            }

                        }
                        else
                        {
                            (string, ErrorEntity?) result = TransformReference(material.ReferenceBase, "", worksheet, line);
                            if (string.IsNullOrEmpty(result.Item1))
                            {
                                continue;

                            }
                            material.Reference = result.Item1;
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
                        material.MaterialType = MaterialType.Piece;

                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Accessories {sortOrder} of {worksheet.RowCount - 5} - {material.Description}";
                        _progress?.Report(_progressValue);

                        //===================================================================================================
                        materials.Add(material);

                        //===================================================================================================
                        await LogMappedMaterialEntityAsync(material);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                           "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nReference: {$Reference }, " +
                            "\nColor: {$Color }, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                              nameof(MapperTechDesign),
                            nameof(MapAccessoriesAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        ErrorEntitys.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapAccessoriesAsync)}, " +
                           $"\nOrder: {worksheet.Order ?? string.Empty}," +
                           $"\nWorksheet: {worksheet.Name ?? string.Empty}," +


                           $"\nDescription: {material.Description ?? string.Empty}," +
                           $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                           $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }



                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                }

                return (materials, ErrorEntitys);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapAccessoriesAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return (materials, ErrorEntitys);
            }

        }

        private async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapPanelsAsync(Worksheet worksheet)
        {

            int sortOrder = -1;
            int line = -1;
            List<MaterialEntity> materials = [];
            List<ErrorEntity> ErrorEntitys = [];

            try
            {




                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialEntity material = new();

                    sortOrder++;

                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.Order = worksheet.Order ?? string.Empty;
                        //===================================================================================================
                        //material.SourceReference = null;
                        material.SourceDescription = worksheet.WorksheetData[i][4]?.ToString();
                        material.SourceColor = worksheet.WorksheetData[i][2]?.ToString();
                        material.SourceColorDescription = worksheet.WorksheetData[i][3] == null ? null : worksheet.WorksheetData[i][2].ToString();

                        //===================================================================================================
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Panels;

                        //===================================================================================================
                        material.Item = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        //Reset Sort Order if new item
                        //===================================================================================================
                        if (material.Item != worksheet.WorksheetData[i - 1][1].ToString())
                        {
                            sortOrder = 0;
                        }
                        material.SortOrder = sortOrder;

                        //===================================================================================================                          
                        material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                        var pattern = @"\(XPS\)\s+\d{1,2}mm$";
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

                                (string, ErrorEntity?) result = TransformReference("AluSheet1", material.SourceColor ?? string.Empty, worksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }


                                material.Reference = result.Item1;

                                if (result.Item2 != null)
                                {
                                    ErrorEntitys.Add(result.Item2);
                                }

                            }
                            else if (string.IsNullOrEmpty(material.Reference) && (material.Description == "1.25 mm aluminium sheet" || material.Description == "1.25mm aluminium sheet"))
                            {

                                (string, ErrorEntity?) result = TransformReference("AluSheet1.25", material.SourceColor ?? string.Empty, worksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }
                                material.Reference = result.Item1;
                                if (result.Item2 != null)
                                {
                                    ErrorEntitys.Add(result.Item2);
                                }

                            }
                            else if (string.IsNullOrEmpty(material.Reference) && (material.Description == "1.5 mm aluminium sheet" || material.Description == "1.5mm aluminium sheet"))
                            {

                                (string, ErrorEntity?) result = TransformReference("AluSheet1.5", material.SourceColor ?? string.Empty, worksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }
                                material.Reference = result.Item1;
                                if (result.Item2 != null)
                                {
                                    ErrorEntitys.Add(result.Item2);
                                }

                            }
                            else
                            {
                                (string, ErrorEntity?) result = TransformReference(material.SourceReference ?? string.Empty, material.SourceColor ?? string.Empty, worksheet, line);

                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }

                                material.Reference = result.Item1;

                                if (result.Item2 != null)
                                {
                                    ErrorEntitys.Add(result.Item2);
                                }
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
                        await LogMappedMaterialEntityAsync(material);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{$Method}." +
                            "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nReference: {$Reference}, " +
                            "\nColor: {$Color}, " +
                            "\nPrefSuite Reference Base {$ReferenceBase}, " +
                            "\nPrefSuite Reference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  {$Exception}",
                             nameof(MapperTechDesign),
                            nameof(MapPanelsAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceColor ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        ErrorEntitys.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapPanelsAsync)}, " +
                        $"\nOrder: {worksheet.Order ?? string.Empty}," +
                        $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                        $"\nReference: {material.SourceReference ?? string.Empty}," +
                        $"\nColor: {material.SourceColor ?? string.Empty}," +
                        $"\nDescription: {material.Description ?? string.Empty}," +
                        $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                        $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }



                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                }

                return (materials, ErrorEntitys);

            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapPanelsAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return (materials, ErrorEntitys);
            }

        }

        private async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapGlassesAsync(Worksheet worksheet)
        {
            int sortOrder = -1;
            int line = -1;

            List<MaterialEntity> materials = [];
            List<ErrorEntity> ErrorEntitys = [];

            try
            {




                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialEntity material = new();
                    sortOrder++;
                    line = i + 1;
                    try
                    {
                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.Order = worksheet.Order ?? string.Empty;
                        material.SourceReference = null;
                        material.SourceDescription = worksheet.WorksheetData[i][2]?.ToString();
                        material.SourceColor = null;
                        material.SourceColorDescription = null;
                        //===================================================================================================
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Glasses;

                        //===================================================================================================
                        material.Item = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        //===================================================================================================
                        //Reset Sort Order if new item
                        if (material.Item != worksheet.WorksheetData[i - 1][1].ToString())
                        {
                            sortOrder = 0;
                        }
                        material.SortOrder = sortOrder;
                        //===================================================================================================
                        material.Description = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(material.Description))
                        {
                            _logService.Error("{$Class}.{$Method}. Glass description is missing." +
                           "\nOrder {$Order}, " +
                           "\nWorksheet: {$Worksheet}, " +
                           "\nReference {$Reference}, " +
                           "\nColor {$Color}," +

                           nameof(MapperTechDesign),
                           nameof(MapGlassesAsync),
                           worksheet.Order ?? string.Empty,
                           worksheet.Name ?? string.Empty,
                           material.SourceReference ?? string.Empty,
                           material.Description ?? string.Empty
                            );

                            ErrorEntitys.Add(new ErrorEntity()
                            {
                                OrderNumber = worksheet.Order!,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Glass description is missing." +
                                $"\nOrder: {worksheet.Order}, " +
                                $"\nWorksheet: {worksheet.Name}, " +
                                $"\nReference: {material.SourceReference ?? "not found"}," +
                                $"\nDescription: {material.Description ?? "not found"}"

                            });
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
                            _logService.Error("{$Class}.{$Method}. Glass not exists in PrefSuite DB." +
                          "\nOrder {$Order}, " +
                          "\nWorksheet: {$Worksheet}, " +
                          "\nReference: {$Reference}, " +
                          "\nDescription: {$Color} not found. " +
                          "\nExpected Reference {$ExpectedReference} of glass",

                            nameof(MapperTechDesign),
                              nameof(MapGlassesAsync),
                              worksheet.Order ?? string.Empty,
                              worksheet.Name ?? string.Empty,
                              material.SourceReference ?? string.Empty,
                              material.SourceDescription ?? string.Empty,
                              resultPredicted);

                            ErrorEntitys.Add(new ErrorEntity()
                            {
                                OrderNumber = worksheet.Order!,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Glass not exists in PrefSuite DB." +
                                $"\nOrder: {worksheet.Order}," +
                                $"\nWorksheet: {worksheet.Name}." +
                                $"\nGlass description: {material.SourceDescription}" +
                                $"\nExpected PrefSuite Reference: {resultPredicted ?? "not found"}."

                            });
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

                        await LogMappedMaterialEntityAsync(material);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error  {$Class}.{Method}" +
                            "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nReference {$Reference}, " +
                            "\nColor {$Color}, " +
                            "\nPrefSuite Reference {$PrefSuiteReference}," +
                            "\nPrefSuite Reference Base {$PrefSuiteReferenceBase}," +
                            "\nException  ${Exception}",
                              nameof(MapperTechDesign),
                            nameof(MapGlassesAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            material.SourceReference ?? string.Empty,
                            material.SourceDescription ?? string.Empty,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        ErrorEntitys.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapGlassesAsync)}, " +
                     $"\nOrder: {worksheet.Order ?? string.Empty}," +
                     $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                     $"\nReference: {material.SourceReference ?? string.Empty}," +
                     $"\nColor: {material.SourceColor ?? string.Empty}," +
                     $"\nItem: {material.Item ?? string.Empty}," +
                     $"\nDescription: {material.Description ?? string.Empty}," +
                     $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }

                }

                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                return (materials, ErrorEntitys);

            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapGlassesAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return (materials, ErrorEntitys);
            }

        }

        private async Task<(List<MaterialEntity>, List<ErrorEntity>)> MapOthersAsync(Worksheet worksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialEntity> materials = [];
            List<ErrorEntity> ErrorEntitys = [];

            try
            {





                for (int i = 4; i < worksheet.RowCount; i++)
                {
                    MaterialEntity material = new();
                    sortOrder++;

                    line = i + 1;
                    try
                    {


                        material.Worksheet = worksheet.Name ?? string.Empty;
                        material.Order = worksheet.Order ?? string.Empty;
                        material.Line = line;
                        material.WorksheetType = WorksheetType.Materials;
                        material.Item = null;// not used in others
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

                            (string, ErrorEntity?) result = TransformReference(material.ReferenceBase, material.Color, worksheet, line);
                            if (string.IsNullOrEmpty(result.Item1))
                            {
                                continue;
                            }
                            material.Reference = result.Item1;
                            if (result.Item2 != null)
                            {
                                ErrorEntitys.Add(result.Item2);
                            }

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
                        material.CustomField3 = null; // not used in others
                                                      //================================================================================================================
                        material.CustomField4 = null; // not used in others
                        material.CustomField5 = null; // not used in others
                                                      //================================================================================================================
                        material.MaterialType = MaterialType.Piece;
                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Other materials {sortOrder} of {worksheet.RowCount - 5} -  {material.ReferenceBase}_{material.Color}";
                        _progress?.Report(_progressValue);

                        materials.Add(material);

                        await LogMappedMaterialEntityAsync(material);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$Order}, " +
                            "\nWorksheet: {$Worksheet}, " +
                            "\nLine {$Line}, " +
                            "\nReference base {$ReferenceBase}, " +
                            "\nReference {$Reference}," +
                            "\nDescription {$Description}," +
                            "\nException  ${Exception}",
                              nameof(MapperTechDesign),
                            nameof(MapOthersAsync),
                            worksheet.Order ?? string.Empty,
                            worksheet.Name ?? string.Empty,
                            line,
                            material.ReferenceBase ?? string.Empty,
                            material.Reference ?? string.Empty,
                            material.Description ?? string.Empty,
                             ex.Message ?? string.Empty);

                        ErrorEntitys.Add(new ErrorEntity()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapOthersAsync)}, " +
                             $"\nOrder: {worksheet.Order ?? string.Empty}," +
                             $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                             $"\nLine {material.Line}," +
                             $"\nItem: {material.Item ?? string.Empty}," +
                             $"\nDescription: {material.Description ?? string.Empty}," +
                             $"\nData: {worksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                             $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }



                }
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return (materials, ErrorEntitys);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapOthersAsync),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    ex.Message);

                return (materials, ErrorEntitys);
            }

        }

        private async Task LogMappedMaterialEntityAsync(MaterialEntity material)
        {
            await Task.Run(() =>
            {
                _logService.Verbose("Mapper Sapa 2 Service: Map Materials | Order : {$Order} " +
                                                              "| Worksheet {Worksheet$} " +
                                                              "| Line: {$Line} " +
                                                              "| Sort order: " +
                                                              "| Reference : {$Reference}  " +
                                                              "| Message : {$Message} " +
                                                              "| Color : {$Color} " +
                                                              "| ColorDescription : {$ColorDescription} " +
                                                              "| Width : {$Width} " +
                                                              "| Height : {$Height} " +
                                                              "| Weight : {$Weight} " +
                                                              "| Area : {$Area} " +
                                                              "| Quantity : {$Quantity} " +
                                                              "| PackageQuantity : {$PackageQuantity} " +
                                                              "| TotalQuantity : {$TotalQuantity} " +
                                                              "| RequiredQuantity : {$RequiredQuantity} " +
                                                              "| LeftOverQuantity : {$LeftOverQuantity} " +
                                                              "| Waste : {$Waste} " +
                                                              "| TotalWeight : {$TotalWeight} " +
                                                              "| RequiredWeight : {$RequiredWeight} " +
                                                              "| LeftOverWeight : {$LeftOverWeight} " +
                                                              "| TotalArea : {$TotalArea} " +
                                                              "| RequiredArea : {$RequiredArea} " +
                                                              "| LeftOverArea : {$LeftOverArea} " +
                                                              "| Price : {$Price} " +
                                                              "| TotalPrice : {$TotalPrice} " +
                                                              "| RequiredPrice : {$RequiredPrice} " +
                                                              "| LeftOverPrice : {$LeftOverPrice} " +
                                                              "| Pallet : {$Pallet} " +
                                                              "| MaterialType : {$MaterialType} " +
                                                              "| CustomField1 : {$CustomField1} " +
                                                              "| CustomField2 : {$CustomField2} " +
                                                              "| CustomField3 : {$CustomField3} " +
                                                              "| CustomField4 : {$CustomField4} " +
                                                              "| CustomField5 : {$CustomField5} " +
                                                              "| SquareMeterPrice : {$SquareMeterPrice} " +
                                                              "| SourceReference : {$SourceReference} " +
                                                              "| SourceDescription : {$SourceDescription} " +
                                                              "| SourceColor : {$SourceColor} " +
                                                              "| SourceColorDescription : {$SourceColorDescription} " +
                                                              "| WorksheetType : {$WorksheetType} " +
                                                              "|",
                                                              material.Order ?? string.Empty,
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

        private async Task LogMappedItemEntityAsync(ItemEntity item)
        {
            await Task.Run(() =>
            {
                _logService.Verbose(
                    "Mapper Sapa 2 Service: Map Items | Order : {$Order} " +
                    "| Worksheet {Worksheet$} " +
                    "| Line: {$Line} " +
                    "| Sort order: " +
                    "| Item : {$Item}  " +
                    "| Sort order : {$SortOrder} " +
                    "| Description : {$Description} " +
                    "| Quantity : {$Quantity} " +
                    "| Width : {$Width} " +
                    "| Height : {$Height} " +
                    "| Weight : {$Weight} " +
                    "| Weight Without Glass : {$WeightWithoutGlass} " +
                    "| Weight Glass : {$WeightGlass} " +
                    "| Total Weight : {$TotalWeight} " +
                    "| Total Weight Glass : {$TotalWeightGlass} " +
                    "| Area : {$Area} " +
                    "| Total Area : {$TotalArea} " +
                    "| Hours : {$Hours} " +
                    "| Total Hours : {$TotalHours} " +
                    "| Material Cost : {$MaterialCost}" +
                    "| Labor Cost : {$LaborCost} " +
                    "| Cost : {$Cost} " +
                    "| Total Material Cost : {$TotalMaterialCost} " +
                    "| Total Labor Cost : {$TotalLaborCost} " +
                    "| Total Cost : {$TotalCost} " +
                    "| Price : {$Price} " +
                    "| Total Price : {$TotalPrice} " +
                    "| Currency Code : {$CurrencyCode} " +
                    "| Exchange Rate EUR : {$ExchangeRateEUR} " +
                    "| Material Cost EUR : {$MaterialCostEUR} " +
                    "| Labor Cost EUR : {$LaborCostEUR} " +
                    "| Cost EUR : {$CostEUR} " +
                    "| Total Material Cost EUR : {$TotalMaterialCostEUR} " +
                    "| Total Labor Cost EUR : {$TotalLaborCostEUR} " +
                    "| Total Cost EUR : {$TotalCostEUR} " +
                    "| Price EUR : {$PriceEUR} " +
                    "| Total Price EUR : {$TotalPriceEUR} " +
                    "| Worksheet Type : {$WorksheetType} ",
                    item.Order ?? string.Empty,
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
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nGlass description: {$GlassDescription}." +
                    "\nGlass predicted reference:  {$PredictedReference}." +
                    "\nException {$Exception}",
               nameof(MapperTechDesign),
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

                reference = await _sqlRepository.GetGlassReferenceAsync(description);

                return reference?.Trim();
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nPredicted glass reference: {$PredictedReference}," +
                    "\nException {$Exception}",
               nameof(MapperTechDesign),
                    nameof(GetGlassReferenceAsync),
                        description ?? string.Empty,
                    ex.Message);

                return null;

            }
        }

        private (string, ErrorEntity?) TransformReference(string sapaReference, string sapaColor, Worksheet worksheet, int line)
        {

            string reference = string.Empty;
            string initialReference = sapaReference?.Trim() ?? string.Empty;
            string initialColor = sapaColor?.Trim() ?? string.Empty;

            try
            {
                // Log an error if both fields are empty
                if (string.IsNullOrEmpty(sapaReference) && string.IsNullOrEmpty(sapaColor))
                {
                    _logService.Error("{$Class}.{$Method}. " +
                    "Error Sapa article and color are empty." +
                    "\nOrder: {$Order}, " +
                    "\nWorksheet: {$Worksheet}, " +
                    "\nReference: {$Reference}, " +
                    "\nColor: {$Color}.",
                    nameof(MapperTechDesign),
                    nameof(TransformReference),
                    worksheet.Order ?? string.Empty,
                    worksheet.Name ?? string.Empty,
                    initialReference ?? string.Empty,
                    initialColor ?? string.Empty);

                    ErrorEntity ErrorEntity = new()
                    {
                        OrderNumber = worksheet.Order ?? string.Empty,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.MappingService_MapMaterial,
                        Message = $"Error Sapa article and color are empty" +
                       $"\nLine will be skipped." +
                       $"\nOrder: {worksheet.Order ?? string.Empty}, " +
                       $"\nWorksheet: {worksheet.Name ?? string.Empty}, " +
                       $"\nReference: {initialReference ?? string.Empty}, " +
                       $"\nColor: {initialColor ?? string.Empty}"

                    };

                    return (string.Empty, ErrorEntity);
                }



                if (worksheet.Name is "ND_Gaskets" or "ND_Accessories")
                {
                    if (string.IsNullOrEmpty(sapaReference))
                    {

                        return (sapaReference ?? string.Empty, null);
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
                            return (sapaReference, null);
                        }
                    }
                }

                // Processing for ND_Profiles and ND_Accessories
                if (worksheet.Name is "ND_Profiles" or "ND_Accessories" or "ND_Gaskets" or "ND_Panels")
                {
                    if (string.IsNullOrEmpty(sapaReference))
                    {

                        return (sapaReference ?? string.Empty, null);
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


                        _logService.Error("Mapper Sapa 2 Service: Warning." +
                           "Reference > 25 characters." +
                           "\nOrder: {$Order}, " +
                           "\nWorksheet: {$Worksheet}," +
                           "\nReference: {$Reference}," +
                           "\nColor: {$Color}," +
                           "\nGenerated PrefSuite Reference: {$PrefSuiteReference}, length:{$PrefSuiteReferenceLength}." +
                           "\nReference inserted into DB Reference {$PrefSuiteTruncatedReference}, length:{$PrefsuiteTrunctaedLength}." +
                           "\n",
                           worksheet.Order ?? string.Empty,
                           worksheet.Name ?? string.Empty,
                           initialReference ?? string.Empty,
                           initialColor ?? string.Empty,
                           reference,
                           reference.Length,
                           newReference,
                           newReference.Length);

                        ErrorEntity ErrorEntity = new()
                        {
                            OrderNumber = worksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Mapper Sapa 2: Generated material Reference is > 25 characters!" +
                           $"\nLine will be skipped." +
                           $"\nOrder: {worksheet.Order ?? string.Empty}," +
                           $"\nWorksheet: {worksheet.Name ?? string.Empty}, " +
                           $"\nReference: {initialReference ?? string.Empty}, length:{(initialReference ?? string.Empty).Length})." +
                           $"\nColor: {sapaColor ?? string.Empty}, length:{(initialColor ?? string.Empty).Length})." +
                           $"\nGenerated PrefSuite Reference: {reference}, length:{reference.Length}." +
                           $"\nReference inserted into DB: {newReference}, length{newReference.Length}." +
                           "\n"
                        };

                        reference = newReference; // Use the new reference
                        return (reference, ErrorEntity);
                    }

                }

                return (reference, null);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nException {$Exception}",
                   nameof(MapperTechDesign),
                    nameof(TransformReference),
                    worksheet.Order ?? string.Empty,
                    ex.Message);

                ErrorEntity ErrorEntity = new()
                {
                    OrderNumber = worksheet.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.MappingService_MapMaterial,
                    Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(TransformReference)}, " +

                   $"\nOrder: {worksheet.Order ?? string.Empty}," +
                   $"\nWorksheet: {worksheet.Name ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}."
                };

                return (reference, ErrorEntity);
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