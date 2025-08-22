// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

using System.Text.RegularExpressions;

namespace a2p.Shared.Application.Services
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

        public async Task<(List<ItemDTO>, List<A2PError>)> MapItemsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress;

            List<ItemDTO> itemsDTO = [];
            List<A2PError> a2pErrors = [];
            if (a2pWorksheet == null || !a2pWorksheet.WorksheetData.Any())
            {
                return (itemsDTO, a2pErrors);
            }

            try
            {

                int rowCounter = 0;
                int sortOrder = -1;

                double totalSellingPrice = double.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][20].ToString(), out double orderPrice) ? orderPrice : 0;
                double totalQuotePrice = double.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][22].ToString(), out double orderDiscount) ? orderDiscount : 0;
                double discountCoeficient = 1;
                if (totalSellingPrice != 0)
                {
                    discountCoeficient = totalQuotePrice / totalSellingPrice;

                }

                for (int i = 1; i < a2pWorksheet.RowCount; i++)
                {
                    ItemDTO itemDTO = new();
                    try
                    {

                        sortOrder++;
                        rowCounter++;

                        int line = i + 1;
                        _progressValue.ProgressTask3 = $"Reading row {rowCounter} of {a2pWorksheet.RowCount - 2})";
                        _progress?.Report(_progressValue);

                        itemDTO.Order = a2pWorksheet.Order ?? string.Empty;
                        itemDTO.Worksheet = a2pWorksheet.Name ?? string.Empty;
                        itemDTO.Line = line;
                        itemDTO.Column = -1;
                        itemDTO.Item = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        itemDTO.SortOrder = sortOrder;
                        itemDTO.Description = a2pWorksheet.WorksheetData[i][0].ToString();
                        itemDTO.Quantity = int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 0;
                        itemDTO.Width = double.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out double width) ? width : 0;
                        itemDTO.Height = double.TryParse(a2pWorksheet.WorksheetData[i][4].ToString(), out double height) ? height : 0;
                        itemDTO.Weight = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double weight) ? weight : 0;
                        itemDTO.WeightGlass = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double weightGlass) ? weightGlass : 0;
                        itemDTO.LaborCost = double.TryParse(a2pWorksheet.WorksheetData[i][17].ToString(), out double laborCost) ? laborCost : 0;
                        itemDTO.Hours = double.TryParse(a2pWorksheet.WorksheetData[i][18].ToString(), out double hours) ? hours : 0;
                        itemDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][22].ToString(), out double price) ? price : 0;
                        itemDTO.WorksheetType = a2pWorksheet.WorksheetType;
                        itemDTO.CurrencyCode = a2pWorksheet.Currency ?? "Unknown";

                        if (string.IsNullOrEmpty(itemDTO.Item))
                        {
                            _logService.Debug("{$Class}.{$Method}." +
                           "\nOrder {$Order}." +
                           "\nWorksheet {$Worksheet}." +
                           "\nLine {$Line}. Item name is missing." +
                           "\nItem {$Data}.",
                          nameof(MapperTechDesign),
                            nameof(MapItemsAsync),
                           itemDTO.Order ?? string.Empty,
                           itemDTO.Worksheet ?? string.Empty,
                           itemDTO.Line,
                           a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty);
                            continue;

                        }
                        _progressValue.ProgressTask3 = $"Item {sortOrder} of {a2pWorksheet.RowCount - 2} - Item # \"{itemDTO.Item}\"";
                        _progress?.Report(_progressValue);

                        double profileCost = double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double profile) ? profile : 0;
                        double fittingCost = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double fitting) ? fitting : 0;
                        double gasketAccessoriesCost = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double gasketAccessories) ? gasketAccessories : 0;
                        double aluminumSheetCost = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double aluminumSheet) ? aluminumSheet : 0;
                        double surchargeALuProfilesCost = double.TryParse(a2pWorksheet.WorksheetData[i][12].ToString(), out double surchargeALuProfiles) ? surchargeALuProfiles : 0;
                        double surfaceTreatmentCost = double.TryParse(a2pWorksheet.WorksheetData[i][13].ToString(), out double surfaceTreatment) ? surfaceTreatment : 0;
                        double clientMaterialsCost = double.TryParse(a2pWorksheet.WorksheetData[i][14].ToString(), out double clientMaterials) ? clientMaterials : 0;
                        double glassCost = double.TryParse(a2pWorksheet.WorksheetData[i][15].ToString(), out double glass) ? glass : 0;
                        double panelCost = double.TryParse(a2pWorksheet.WorksheetData[i][16].ToString(), out double panel) ? panel : 0;
                        double specialCost = double.TryParse(a2pWorksheet.WorksheetData[i][19].ToString(), out double special) ? special : 0;

                        itemDTO.WeightWithoutGlass = Math.Round(itemDTO.Weight - itemDTO.WeightGlass, 4);
                        itemDTO.TotalWeight = Math.Round(itemDTO.Weight * itemDTO.Quantity, 4);
                        itemDTO.TotalWeightWithoutGlass = Math.Round(itemDTO.WeightWithoutGlass * itemDTO.Quantity, 4);
                        itemDTO.TotalWeightGlass = Math.Round(itemDTO.WeightGlass * itemDTO.Quantity, 4);
                        itemDTO.Area = Math.Round(itemDTO.Width * itemDTO.Height / 1000000, 4);
                        itemDTO.TotalArea = Math.Round(itemDTO.Area * itemDTO.Quantity, 4);

                        itemDTO.TotalHours = Math.Round(itemDTO.Hours * itemDTO.Quantity, 4);
                        itemDTO.MaterialCost = Math.Round(profileCost + fittingCost + gasketAccessoriesCost + aluminumSheetCost + surchargeALuProfilesCost + surfaceTreatmentCost + clientMaterialsCost + panelCost + glassCost, 6);
                        itemDTO.Cost = Math.Round(itemDTO.MaterialCost + itemDTO.LaborCost, 4);
                        itemDTO.TotalMaterialCost = Math.Round(itemDTO.MaterialCost * itemDTO.Quantity, 4);
                        itemDTO.TotalLaborCost = Math.Round(itemDTO.LaborCost * itemDTO.Quantity, 4);
                        itemDTO.TotalCost = Math.Round(itemDTO.Cost * itemDTO.Quantity, 4);

                        itemDTO.TotalPrice = Math.Round(itemDTO.TotalPrice * discountCoeficient, 4);
                        itemDTO.Price = Math.Round(itemDTO.TotalPrice / itemDTO.Quantity, 4);

                        itemDTO.ExchangeRateEUR = 1; //TODO': Exchange Rate 

                        itemDTO.MaterialCostEUR = Math.Round(itemDTO.MaterialCost * itemDTO.ExchangeRateEUR, 4);
                        itemDTO.TotalMaterialCostEUR = Math.Round(itemDTO.TotalMaterialCost * itemDTO.ExchangeRateEUR, 4);
                        itemDTO.TotalLaborCostEUR = Math.Round(itemDTO.TotalLaborCost * itemDTO.ExchangeRateEUR, 4);
                        itemDTO.CostEUR = Math.Round(itemDTO.Cost * itemDTO.ExchangeRateEUR, 4);
                        itemDTO.TotalCostEUR = Math.Round(itemDTO.TotalCost * itemDTO.ExchangeRateEUR, 4);
                        itemDTO.PriceEUR = Math.Round(itemDTO.Price * itemDTO.ExchangeRateEUR, 4);
                        itemDTO.TotalPriceEUR = Math.Round(itemDTO.TotalPrice * itemDTO.ExchangeRateEUR, 4);

                        itemDTO.WorksheetType = WorksheetType.Items;
                        itemsDTO.Add(itemDTO);

                        await LogMappedItemDTOAsync(itemDTO);

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
                            a2pWorksheet.Order ?? string.Empty,
                            a2pWorksheet.Name ?? string.Empty,
                            itemDTO.Line,
                            itemDTO.Item ?? string.Empty,
                            itemDTO.Description ?? string.Empty,
                            a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty,
                        ex.Message ?? string.Empty);

                        a2pErrors.Add(new A2PError()
                        {
                            Order = a2pWorksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapItemsAsync)}, " +
                          $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                          $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                          $"\nLine {itemDTO.Line}," +
                          $"\nItem: {itemDTO.Item ?? string.Empty}," +
                          $"\nDescription: {itemDTO.Description ?? string.Empty}," +
                          $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                          $"\nException: {ex.Message ?? string.Empty}."
                        });

                        continue;
                    }
                }

                return (itemsDTO, a2pErrors);
            }

            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapItemsAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    ex.Message);

                return (itemsDTO, a2pErrors);
                ;
            }

        }

        public async Task<(List<MaterialDTO>, List<A2PError>)> MapMaterialsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress;

            List<MaterialDTO> materialsDTO = [];
            List<A2PError> a2pErrors = [];
            if (a2pWorksheet == null || !a2pWorksheet.WorksheetData.Any())
            {
                return (materialsDTO, a2pErrors);
            }

            try
            {

                //=============================================================================================================
                // Iterate Files
                // =============================================================================================================

                if (a2pWorksheet.Name == "ND_Profiles")
                {
                    (List<MaterialDTO>, List<A2PError>) result = await MapProfilesAsync(a2pWorksheet);
                    if (result.Item1 != null)
                    {
                        materialsDTO.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        a2pErrors.AddRange(result.Item2);
                    }

                }
                else if (a2pWorksheet.Name == "ND_Gaskets")
                {
                    (List<MaterialDTO>, List<A2PError>) result = await MapGasketsAsync(a2pWorksheet);
                    if (result.Item1 != null)
                    {
                        materialsDTO.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        a2pErrors.AddRange(result.Item2);
                    }

                }

                else if (a2pWorksheet.Name == "ND_Accessories")
                {
                    (List<MaterialDTO>, List<A2PError>) result = await MapAccessoriesAsync(a2pWorksheet);
                    if (result.Item1 != null)
                    {
                        materialsDTO.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        a2pErrors.AddRange(result.Item2);
                    }
                }

                else if (a2pWorksheet.Name == "ND_Panels")
                {
                    (List<MaterialDTO>, List<A2PError>) result = await MapPanelsAsync(a2pWorksheet);
                    if (result.Item1 != null)
                    {
                        materialsDTO.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        a2pErrors.AddRange(result.Item2);
                    }

                }
                else if (a2pWorksheet.Name == "ND_Glasses")
                {
                    (List<MaterialDTO>, List<A2PError>) result = await MapGlassesAsync(a2pWorksheet);
                    if (result.Item1 != null)
                    {
                        materialsDTO.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        a2pErrors.AddRange(result.Item2);
                    }
                }
                else if (a2pWorksheet.Name == "ND_Others")
                {
                    (List<MaterialDTO>, List<A2PError>) result = await MapOthersAsync(a2pWorksheet);
                    if (result.Item1 != null)
                    {
                        materialsDTO.AddRange(result.Item1);
                    }
                    if (result.Item2 != null)
                    {
                        a2pErrors.AddRange(result.Item2);
                    }
                }

                return (materialsDTO, a2pErrors);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapMaterialsAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                a2pErrors.Add(new A2PError()
                {
                    Order = a2pWorksheet.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.MappingService_MapMaterial,
                    Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapMaterialsAsync)}, " +
                       $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                       $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                       $"\nException: {ex.Message ?? string.Empty}."
                });

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task<(List<MaterialDTO>, List<A2PError>)> MapProfilesAsync(A2PWorksheet a2pWorksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialDTO> materialsDTO = [];
            List<A2PError> a2pErrors = [];
            try
            {

                for (int i = 4; i < a2pWorksheet.RowCount; i++)
                {
                    sortOrder++;
                    line = i + 1;
                    MaterialDTO materialDTO = new();
                    try
                    {
                        //===================================================================================================
                        materialDTO.Line = line;
                        materialDTO.WorksheetType = WorksheetType.Materials;
                        materialDTO.Item = null; // not used in profiles
                        materialDTO.SortOrder = -1; // not used in profiles

                        //===================================================================================================
                        materialDTO.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                        materialDTO.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                        materialDTO.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                        materialDTO.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();

                        //===================================================================================================
                        materialDTO.ReferenceBase = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                        (string, A2PError?) result = TransformReference(materialDTO.ReferenceBase, materialDTO.SourceColor ?? string.Empty, a2pWorksheet, line);
                        if (string.IsNullOrEmpty(result.Item1))
                        {
                            continue;
                        }

                        materialDTO.Reference = result.Item1;

                        if (result.Item2 != null)
                        {
                            a2pErrors.Add(result.Item2);
                        }

                        materialDTO.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                        //===================================================================================================
                        materialDTO.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                        materialDTO.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                        //===================================================================================================
                        materialDTO.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                        materialDTO.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                        materialDTO.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                        materialDTO.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                        materialDTO.LeftOverQuantity = Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6);

                        //===================================================================================================
                        materialDTO.Width = materialDTO.PackageQuantity * 1000; //used as bar length in mm
                        materialDTO.Height = 0;

                        //===================================================================================================
                        materialDTO.TotalWeight = a2pWorksheet.WorksheetData[i][11] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalWeight) ? totalWeight : 0;
                        materialDTO.Weight = materialDTO.TotalQuantity == 0 ? 0 : Math.Round(materialDTO.TotalWeight / materialDTO.TotalQuantity, 6);
                        materialDTO.RequiredWeight = Math.Round(materialDTO.Weight * materialDTO.RequiredQuantity, 6);
                        materialDTO.LeftOverWeight = Math.Round(materialDTO.TotalWeight - materialDTO.RequiredWeight, 6) < 0 ? 0 : Math.Round(materialDTO.TotalWeight - materialDTO.RequiredWeight, 6);

                        //===================================================================================================
                        materialDTO.TotalArea = a2pWorksheet.WorksheetData[i][10] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double totalArea) ? totalArea : 0;
                        materialDTO.Area = materialDTO.TotalQuantity == 0 ? 0 : Math.Round(materialDTO.TotalArea / materialDTO.TotalQuantity, 6);
                        materialDTO.RequiredArea = Math.Round(materialDTO.Area * materialDTO.RequiredQuantity, 6);
                        materialDTO.LeftOverArea = Math.Round(materialDTO.TotalArea - materialDTO.RequiredArea, 6) < 0 ? 0 : Math.Round(materialDTO.TotalArea - materialDTO.RequiredArea, 6);

                        //===================================================================================================
                        materialDTO.Waste = materialDTO.RequiredWeight != 0
                            ? a2pWorksheet.WorksheetData[i][9] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double lostWeight) ? lostWeight : 0 / materialDTO.RequiredWeight * 100
                            : 0;
                        //===================================================================================================                                                        
                        materialDTO.Price = double.TryParse(a2pWorksheet.WorksheetData[i][12].ToString(), out double price) ? price : 0;
                        materialDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][13].ToString(), out double totalPrice) ? totalPrice : 0;
                        materialDTO.RequiredPrice = Math.Round(materialDTO.Price * (double)materialDTO.RequiredQuantity, 6);
                        materialDTO.LeftOverPrice = Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6);

                        //===================================================================================================
                        materialDTO.SquareMeterPrice = 0; // not used in profiles 

                        //===================================================================================================
                        materialDTO.Pallet = null;

                        //===================================================================================================


                        if (!string.IsNullOrWhiteSpace(materialDTO.SourceColor))
                        {
                            (string, string)? customColors = SplitColors(materialDTO.SourceColor);


                            if (customColors != null)
                            {
                                materialDTO.CustomField1 = customColors.Value.Item1; // used for custom color
                                materialDTO.CustomField2 = customColors.Value.Item2;
                            }

                            else
                            {
                                materialDTO.CustomField1 = null; // not used
                                materialDTO.CustomField2 = null; // not used
                            }
                        }
                        else
                        {
                            materialDTO.CustomField1 = null; // not used
                            materialDTO.CustomField2 = null; // not used
                        }
                        materialDTO.CustomField3 = null; // not used
                        materialDTO.CustomField4 = null; // not used
                        materialDTO.CustomField5 = null; // not used

                        //===================================================================================================
                        materialDTO.MaterialType = MaterialType.Profiles;

                        //===================================================================================================
                        _progressValue.ProgressTask3 = $"Profiles {sortOrder} of {a2pWorksheet.RowCount - 5} - {materialDTO.Reference}";
                        _progress?.Report(_progressValue);

                        //===================================================================================================
                        materialsDTO.Add(materialDTO);

                        //===================================================================================================
                        await LogMappedMaterialDTOAsync(materialDTO);

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
                            a2pWorksheet.Order ?? string.Empty,
                            a2pWorksheet.Name ?? string.Empty,
                            materialDTO.SourceReference ?? string.Empty,
                            materialDTO.SourceColor ?? string.Empty,
                            materialDTO.ReferenceBase ?? string.Empty,
                            materialDTO.Reference ?? string.Empty,
                            materialDTO.Description ?? string.Empty,
                            ex.Message ?? string.Empty);


                        a2pErrors.Add(new A2PError()
                        {
                            Order = a2pWorksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapProfilesAsync)}, " +
                           $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                           $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                           $"\nLine {materialDTO.Line}," +
                           $"\nItem: {materialDTO.Item ?? string.Empty}," +
                           $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                           $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                           $"\nException: {ex.Message ?? string.Empty}."
                        });
                        continue;
                    }

                }

                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return (materialsDTO, a2pErrors);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapProfilesAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task<(List<MaterialDTO>, List<A2PError>)> MapGasketsAsync(A2PWorksheet a2pWorksheet)
        {

            int sortOrder = -1;
            int line = -1;
            List<MaterialDTO> materialsDTO = [];
            List<A2PError> a2pErrors = [];

            try
            {
                await Task.Run(async () =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {
                        MaterialDTO materialDTO = new();
                        sortOrder++;
                        line = i + 1;
                        try
                        {
                            //===================================================================================================
                            materialDTO.Line = line;
                            materialDTO.WorksheetType = WorksheetType.Materials;
                            materialDTO.Item = null; // not used 
                            materialDTO.SortOrder = -1; // not used 

                            //===================================================================================================
                            materialDTO.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                            materialDTO.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                            materialDTO.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                            materialDTO.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();
                            //===================================================================================================
                            materialDTO.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                            materialDTO.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                            if (string.IsNullOrEmpty(materialDTO.SourceReference) && string.IsNullOrEmpty(materialDTO.SourceColor))
                            {
                                _logService.Error("{$Class}.{$Method}. Sapa article and color are missing. Line will be skipped." +
                                  "\nOrder {$Order}, " +
                                "\nWorksheet: {$Worksheet}, " +
                                "\nDescription {$Description}," +
                                nameof(MapperTechDesign),
                                nameof(MapGasketsAsync),
                                a2pWorksheet.Order ?? string.Empty,
                                a2pWorksheet.Name ?? string.Empty,
                                materialDTO.Description ?? string.Empty
                             );

                                a2pErrors.Add(new A2PError()
                                {
                                    Order = a2pWorksheet.Order ?? string.Empty,
                                    Level = ErrorLevel.Error,
                                    Code = ErrorCode.MappingService_MapMaterial,
                                    Message = $"Sapa article and color are missing. Line will be skipped." +
                                   $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                                   $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                                   $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                                   $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}"
                                });
                                continue;
                            }
                            materialDTO.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();

                            if (string.IsNullOrEmpty(materialDTO.Color) && (string.IsNullOrEmpty(materialDTO.ColorDescription) || materialDTO.ColorDescription.Contains("Without finish")))
                            {
                                materialDTO.Color = "Without";
                            }

                            //===================================================================================================
                            materialDTO.ReferenceBase = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                            if (materialDTO.Color != "Without")
                            {
                                (string, A2PError?) result = TransformReference(materialDTO.ReferenceBase, materialDTO.Color, a2pWorksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }

                                materialDTO.Reference = result.Item1;
                                if (result.Item2 != null)
                                {
                                    a2pErrors.Add(result.Item2);
                                }

                            }

                            else
                            {
                                (string, A2PError?) result = TransformReference(materialDTO.ReferenceBase, "", a2pWorksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;

                                }
                                materialDTO.Reference = result.Item1;
                            }

                            materialDTO.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                            //===================================================================================================
                            materialDTO.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                            materialDTO.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 0;
                            materialDTO.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                            materialDTO.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                            materialDTO.LeftOverQuantity = Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6);

                            //===================================================================================================
                            if (!string.IsNullOrEmpty(a2pWorksheet.WorksheetData[i][9]?.ToString()))
                            {
                                //Extract dimmensions from gasket material description 
                                if (a2pWorksheet.WorksheetData[i][9]?.ToString()?.Contains('/') == true)
                                {
                                    string[] split = a2pWorksheet.WorksheetData[i][9]?.ToString()?.Split('/') ?? Array.Empty<string>();
                                    if (split.Length == 2)
                                    {
                                        materialDTO.Width = double.TryParse(split[0], out double width) ? width : 0;
                                        materialDTO.Height = double.TryParse(split[1], out double height) ? height : 0;
                                    }
                                }
                            }
                            else
                            {

                                materialDTO.Width = 0; // not used 
                                materialDTO.Height = 0; // not used 
                            }
                            //===================================================================================================
                            materialDTO.TotalWeight = 0; // not used 
                            materialDTO.Weight = 0; // not used 
                            materialDTO.RequiredWeight = 0; // not used 
                            materialDTO.LeftOverWeight = 0; // not used 

                            //================================================================================================================
                            materialDTO.TotalArea = 0; // not used 
                            materialDTO.Area = 0; // not used 
                            materialDTO.RequiredArea = 0; // not used 
                            materialDTO.LeftOverArea = 0; // not used 

                            //================================================================================================================
                            materialDTO.Waste = 0; // not used 

                            //=================================================================================================                                
                            materialDTO.Price = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double price) ? price : 0;
                            materialDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalPrice) ? totalPrice : 0;
                            materialDTO.RequiredPrice = Math.Round(materialDTO.Price * (double)materialDTO.RequiredQuantity, 6);
                            materialDTO.LeftOverPrice = Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6);

                            //===================================================================================================
                            materialDTO.SquareMeterPrice = 0; // not used 

                            //================================================================================================================
                            materialDTO.Pallet = null;

                            //===================================================================================================\

                            if (!string.IsNullOrWhiteSpace(materialDTO.SourceColor))
                            {
                                (string, string)? customColors = SplitColors(materialDTO.SourceColor);


                                if (customColors != null)
                                {
                                    materialDTO.CustomField1 = customColors.Value.Item1; // used for custom color
                                    materialDTO.CustomField2 = customColors.Value.Item2;
                                }

                                else
                                {
                                    materialDTO.CustomField1 = null; // not used
                                    materialDTO.CustomField2 = null; // not used
                                }
                            }
                            else
                            {
                                materialDTO.CustomField1 = null; // not used
                                materialDTO.CustomField2 = null; // not used
                            }
                            materialDTO.CustomField3 = null; // not used 
                            materialDTO.CustomField4 = null; // not used 
                            materialDTO.CustomField5 = null; // not used 

                            //================================================================================================================
                            materialDTO.MaterialType = MaterialType.Gaskets;

                            //===================================================================================================
                            _progressValue.ProgressTask3 = $"Gaskets {sortOrder} of {a2pWorksheet.RowCount - 5} - {materialDTO.Description}";
                            _progress?.Report(_progressValue);

                            //================================================================================================================
                            materialsDTO.Add(materialDTO);

                            //================================================================================================================
                            await LogMappedMaterialDTOAsync(materialDTO);
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
                                a2pWorksheet.Order ?? string.Empty,
                                a2pWorksheet.Name ?? string.Empty,
                                materialDTO.SourceReference ?? string.Empty,
                                materialDTO.SourceColor ?? string.Empty,
                                materialDTO.ReferenceBase ?? string.Empty,
                                materialDTO.Reference ?? string.Empty,
                                materialDTO.Description ?? string.Empty,
                                 ex.Message ?? string.Empty);
                            a2pErrors.Add(new A2PError()
                            {
                                Order = a2pWorksheet.Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapGasketsAsync)}, " +
                               $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                               $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                               $"\nLine {materialDTO.Line}," +
                               $"\nItem: {materialDTO.Item ?? string.Empty}," +
                               $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                               $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                               $"\nException: {ex.Message ?? string.Empty}."
                            });
                            continue;
                        }

                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                });
                return (materialsDTO, a2pErrors);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapGasketsAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task<(List<MaterialDTO>, List<A2PError>)> MapAccessoriesAsync(A2PWorksheet a2pWorksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<A2PError> a2pErrors = [];
            List<MaterialDTO> materialsDTO = [];
            try
            {

                await Task.Run(async () =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {
                        MaterialDTO materialDTO = new();
                        sortOrder++;

                        line = i + 1;
                        try
                        {

                            //===================================================================================================
                            materialDTO.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                            materialDTO.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                            materialDTO.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                            materialDTO.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();

                            //===================================================================================================
                            materialDTO.Line = line;
                            materialDTO.WorksheetType = WorksheetType.Materials;
                            materialDTO.Item = null; // not used 
                            materialDTO.SortOrder = -1; // not used           

                            //===================================================================================================
                            materialDTO.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                            materialDTO.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                            if (string.IsNullOrEmpty(materialDTO.Color) && (string.IsNullOrEmpty(materialDTO.ColorDescription) || materialDTO.ColorDescription.Contains("Without finish")))
                            {
                                materialDTO.Color = "Without";
                            }

                            //=================================================================================================== 
                            materialDTO.ReferenceBase = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                            if (materialDTO.Color != "Without")
                            {
                                (string, A2PError?) result = TransformReference(materialDTO.ReferenceBase, materialDTO.Color, a2pWorksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }
                                materialDTO.Reference = result.Item1;
                                if (result.Item2 != null)
                                {
                                    a2pErrors.Add(result.Item2);
                                }

                            }
                            else
                            {
                                (string, A2PError?) result = TransformReference(materialDTO.ReferenceBase, "", a2pWorksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;

                                }
                                materialDTO.Reference = result.Item1;
                            }
                            materialDTO.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;

                            //===================================================================================================
                            materialDTO.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                            materialDTO.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                            materialDTO.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                            materialDTO.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                            materialDTO.LeftOverQuantity = Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6);

                            //===================================================================================================
                            materialDTO.Width = 0; // not used 
                            materialDTO.Height = 0; // not used 

                            //===================================================================================================
                            materialDTO.TotalWeight = 0; // not used 
                            materialDTO.Weight = 0; // not used 
                            materialDTO.RequiredWeight = 0; // not used 
                            materialDTO.LeftOverWeight = 0; // not used 

                            //===================================================================================================
                            materialDTO.TotalArea = 0; // not used 
                            materialDTO.Area = 0; // not used 
                            materialDTO.RequiredArea = 0; // not used 
                            materialDTO.LeftOverArea = 0; // not used 

                            //===================================================================================================
                            materialDTO.Waste = 0; // not used 

                            //===================================================================================================
                            materialDTO.Price = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double price) ? price : 0;
                            materialDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double totalPrice) ? totalPrice : 0;
                            materialDTO.RequiredPrice = Math.Round(materialDTO.Price * (double)materialDTO.RequiredQuantity, 6);
                            materialDTO.LeftOverPrice = Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6);

                            //===================================================================================================
                            materialDTO.SquareMeterPrice = 0; // not used 

                            //===================================================================================================
                            materialDTO.Pallet = null;

                            //===================================================================================================

                            if (!string.IsNullOrWhiteSpace(materialDTO.SourceColor))
                            {
                                (string, string)? customColors = SplitColors(materialDTO.SourceColor);


                                if (customColors != null)
                                {
                                    materialDTO.CustomField1 = customColors.Value.Item1; // used for custom color
                                    materialDTO.CustomField2 = customColors.Value.Item2;
                                }

                                else
                                {
                                    materialDTO.CustomField1 = null; // not used
                                    materialDTO.CustomField2 = null; // not used
                                }
                            }
                            else
                            {
                                materialDTO.CustomField1 = null; // not used
                                materialDTO.CustomField2 = null; // not used
                            }
                            materialDTO.CustomField3 = null; // not used 
                            materialDTO.CustomField4 = null; // not used 
                            materialDTO.CustomField5 = null; // not used 

                            //===================================================================================================
                            materialDTO.MaterialType = MaterialType.Piece;

                            //===================================================================================================
                            _progressValue.ProgressTask3 = $"Accessories {sortOrder} of {a2pWorksheet.RowCount - 5} - {materialDTO.Description}";
                            _progress?.Report(_progressValue);

                            //===================================================================================================
                            materialsDTO.Add(materialDTO);

                            //===================================================================================================
                            await LogMappedMaterialDTOAsync(materialDTO);
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
                                a2pWorksheet.Order ?? string.Empty,
                                a2pWorksheet.Name ?? string.Empty,
                                materialDTO.SourceReference ?? string.Empty,
                                materialDTO.SourceColor ?? string.Empty,
                                materialDTO.ReferenceBase ?? string.Empty,
                                materialDTO.Reference ?? string.Empty,
                                materialDTO.Description ?? string.Empty,
                                 ex.Message ?? string.Empty);

                            a2pErrors.Add(new A2PError()
                            {
                                Order = a2pWorksheet.Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapAccessoriesAsync)}, " +
                               $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                               $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +


                               $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                               $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                               $"\nException: {ex.Message ?? string.Empty}."
                            });
                            continue;
                        }

                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                });

                return (materialsDTO, a2pErrors);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapAccessoriesAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task<(List<MaterialDTO>, List<A2PError>)> MapPanelsAsync(A2PWorksheet a2pWorksheet)
        {

            int sortOrder = -1;
            int line = -1;
            List<MaterialDTO> materialsDTO = [];
            List<A2PError> a2pErrors = [];

            try
            {

                await Task.Run(async () =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {
                        MaterialDTO materialDTO = new();

                        sortOrder++;

                        line = i + 1;
                        try
                        {
                            //===================================================================================================
                            materialDTO.SourceReference = null;
                            materialDTO.SourceDescription = a2pWorksheet.WorksheetData[i][4]?.ToString();
                            materialDTO.SourceColor = a2pWorksheet.WorksheetData[i][2]?.ToString();
                            materialDTO.SourceColorDescription = a2pWorksheet.WorksheetData[i][3] == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();

                            //===================================================================================================
                            materialDTO.Line = line;
                            materialDTO.WorksheetType = WorksheetType.Panels;

                            //===================================================================================================
                            materialDTO.Item = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                            //Reset Sort Order if new item
                            //===================================================================================================
                            if (materialDTO.Item != a2pWorksheet.WorksheetData[i - 1][1].ToString())
                            {
                                sortOrder = 0;
                            }
                            materialDTO.SortOrder = sortOrder;

                            //===================================================================================================                          
                            materialDTO.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                            string pattern1 = @"(XPS\s+\d+mm)";
                            Match match = Regex.Match(materialDTO.Description, pattern1);
                            materialDTO.ReferenceBase = match.Success
                                ? $"LOB_XPS{match.Groups[1].Value}"
                                : string.Empty;
                            materialDTO.Reference = match.Success ?
                                $"LOB_XPS{match.Groups[1].Value}"
                                : string.Empty;
                            //===================================================================================================

                            if (string.IsNullOrEmpty(materialDTO.Reference) && materialDTO.Description == "1mm aluminium sheet")
                            {

                                (string, A2PError?) result = TransformReference("AluSheet1", a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }
                                materialDTO.Reference = result.Item1;
                                if (result.Item2 != null)
                                {
                                    a2pErrors.Add(result.Item2);
                                }

                            }
                            else
                            {
                                (string, A2PError?) result = TransformReference(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet, line);

                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }

                                materialDTO.Reference = result.Item1;

                                if (result.Item2 != null)
                                {
                                    a2pErrors.Add(result.Item2);
                                }
                            }

                            //===================================================================================================
                            materialDTO.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                            materialDTO.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;  // not used

                            //===================================================================================================
                            materialDTO.Width = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double width) ? width : 0;
                            materialDTO.Height = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double height) ? height : 0;

                            //===================================================================================================
                            materialDTO.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                            materialDTO.PackageQuantity = 0;
                            materialDTO.TotalQuantity = materialDTO.Quantity;
                            materialDTO.RequiredQuantity = materialDTO.TotalQuantity;
                            materialDTO.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 

                            //===================================================================================================
                            materialDTO.Weight = 0;// not used
                            materialDTO.TotalWeight = 0;// not used
                            materialDTO.RequiredWeight = 0;// not used
                            materialDTO.LeftOverWeight = 0;// not used

                            //===================================================================================================
                            materialDTO.Area = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                            materialDTO.TotalArea = Math.Round(materialDTO.Area * materialDTO.Quantity, 6);
                            materialDTO.RequiredArea = materialDTO.TotalArea; // not used. Threated as unique piece material.
                            materialDTO.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 

                            //===================================================================================================
                            materialDTO.Waste = 0; // not used, panels are not cut in production. Threated as piece material. 

                            //===================================================================================================
                            materialDTO.Price = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double price) ? price : 0;
                            materialDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalPrice) ? totalPrice : 0;
                            materialDTO.RequiredPrice = materialDTO.TotalPrice; // not used. Threated as unique piece material 
                            materialDTO.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 

                            //===================================================================================================
                            materialDTO.SquareMeterPrice = double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double squareMeterPrice) ? squareMeterPrice : 0;

                            //===================================================================================================
                            materialDTO.Pallet = null;

                            //===================================================================================================

                            if (!string.IsNullOrWhiteSpace(materialDTO.SourceColor))
                            {
                                (string, string)? customColors = SplitColors(materialDTO.SourceColor);


                                if (customColors != null)
                                {
                                    materialDTO.CustomField1 = customColors.Value.Item1; // used for custom color
                                    materialDTO.CustomField2 = customColors.Value.Item2;
                                }

                                else
                                {
                                    materialDTO.CustomField1 = null; // not used
                                    materialDTO.CustomField2 = null; // not used
                                }
                            }
                            else
                            {
                                materialDTO.CustomField1 = null; // not used
                                materialDTO.CustomField2 = null; // not used
                            }
                            materialDTO.CustomField3 = null; // not used
                            materialDTO.CustomField4 = null; // not used
                            materialDTO.CustomField5 = null; // not used

                            //===================================================================================================
                            materialDTO.MaterialType = MaterialType.Panels;

                            //===================================================================================================
                            _progressValue.ProgressTask3 = $"Panels {sortOrder} of {a2pWorksheet.RowCount - 5} - {materialDTO.Description}";
                            _progress?.Report(_progressValue);

                            //===================================================================================================
                            materialsDTO.Add(materialDTO);

                            //===================================================================================================
                            await LogMappedMaterialDTOAsync(materialDTO);

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
                                a2pWorksheet.Order ?? string.Empty,
                                a2pWorksheet.Name ?? string.Empty,
                                materialDTO.SourceReference ?? string.Empty,
                                materialDTO.SourceColor ?? string.Empty,
                                materialDTO.ReferenceBase ?? string.Empty,
                                materialDTO.Reference ?? string.Empty,
                                materialDTO.Description ?? string.Empty,
                                 ex.Message ?? string.Empty);

                            a2pErrors.Add(new A2PError()
                            {
                                Order = a2pWorksheet.Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapPanelsAsync)}, " +
                            $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                            $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                            $"\nReference: {materialDTO.SourceReference ?? string.Empty}," +
                            $"\nColor: {materialDTO.SourceColor ?? string.Empty}," +
                            $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                            $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                            $"\nException: {ex.Message ?? string.Empty}."
                            });
                            continue;
                        }

                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                });

                return (materialsDTO, a2pErrors);

            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapPanelsAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task<(List<MaterialDTO>, List<A2PError>)> MapGlassesAsync(A2PWorksheet a2pWorksheet)
        {
            int sortOrder = -1;
            int line = -1;

            List<MaterialDTO> materialsDTO = [];
            List<A2PError> a2pErrors = [];

            try
            {

                await Task.Run(async () =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {
                        MaterialDTO materialDTO = new();
                        sortOrder++;
                        line = i + 1;
                        try
                        {

                            materialDTO.SourceReference = null;
                            materialDTO.SourceDescription = a2pWorksheet.WorksheetData[i][2]?.ToString();
                            materialDTO.SourceColor = null;
                            materialDTO.SourceColorDescription = null;
                            //===================================================================================================
                            materialDTO.Line = line;
                            materialDTO.WorksheetType = WorksheetType.Glasses;

                            //===================================================================================================
                            materialDTO.Item = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;

                            //===================================================================================================
                            //Reset Sort Order if new item
                            if (materialDTO.Item != a2pWorksheet.WorksheetData[i - 1][1].ToString())
                            {
                                sortOrder = 0;
                            }
                            materialDTO.SortOrder = sortOrder;
                            //===================================================================================================
                            materialDTO.Description = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                            if (string.IsNullOrEmpty(materialDTO.Description))
                            {
                                _logService.Error("{$Class}.{$Method}. Glass description is missing." +
                               "\nOrder {$Order}, " +
                               "\nWorksheet: {$Worksheet}, " +
                               "\nReference {$Reference}, " +
                               "\nColor {$Color}," +

                               nameof(MapperTechDesign),
                               nameof(MapGlassesAsync),
                               a2pWorksheet.Order ?? string.Empty,
                               a2pWorksheet.Name ?? string.Empty,
                               materialDTO.SourceReference ?? string.Empty,
                               materialDTO.Description ?? string.Empty
                                );

                                a2pErrors.Add(new A2PError()
                                {
                                    Order = a2pWorksheet.Order!,
                                    Level = ErrorLevel.Error,
                                    Code = ErrorCode.MappingService_MapMaterial,
                                    Message = $"Glass description is missing." +
                                    $"\nOrder: {a2pWorksheet.Order}, " +
                                    $"\nWorksheet: {a2pWorksheet.Name}, " +
                                    $"\nReference: {materialDTO.SourceReference ?? "not found"}," +
                                    $"\nDescription: {materialDTO.Description ?? "not found"}"

                                });
                                continue;
                            }
                            //===================================================================================================
                            string resultPredicted = await GetGlassPredictedReferenceAsync(materialDTO.Description) ?? string.Empty;
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
                                  a2pWorksheet.Order ?? string.Empty,
                                  a2pWorksheet.Name ?? string.Empty,
                                  materialDTO.SourceReference ?? string.Empty,
                                  materialDTO.SourceDescription ?? string.Empty,
                                  resultPredicted);

                                a2pErrors.Add(new A2PError()
                                {
                                    Order = a2pWorksheet.Order!,
                                    Level = ErrorLevel.Error,
                                    Code = ErrorCode.MappingService_MapMaterial,
                                    Message = $"Glass not exists in PrefSuite DB." +
                                    $"\nOrder: {a2pWorksheet.Order}," +
                                    $"\nWorksheet: {a2pWorksheet.Name}." +
                                    $"\nGlass description: {materialDTO.SourceDescription}" +
                                    $"\nExpected PrefSuite Reference: {resultPredicted ?? "not found"}."

                                });
                                continue;
                            }
                            materialDTO.ReferenceBase = resultGlassReference;
                            materialDTO.Reference = resultGlassReference;

                            //===================================================================================================
                            materialDTO.Color = "Transparent"; // not used
                            materialDTO.ColorDescription = "Transparent"; // not used

                            //================================================================================================================
                            materialDTO.Width = double.TryParse(a2pWorksheet.WorksheetData[i][4].ToString(), out double width) ? width : 0;
                            materialDTO.Height = double.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out double height) ? height : 0;
                            //===================================================================================================
                            materialDTO.Quantity = a2pWorksheet.WorksheetData[i][3] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                            materialDTO.PackageQuantity = 0;
                            materialDTO.TotalQuantity = materialDTO.Quantity;
                            materialDTO.RequiredQuantity = materialDTO.TotalQuantity;
                            materialDTO.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 

                            //================================================================================================================
                            materialDTO.Weight = double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double weight) ? weight : 0;
                            materialDTO.TotalWeight = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double totalWeight) ? totalWeight : 0;
                            materialDTO.RequiredWeight = materialDTO.TotalWeight;
                            materialDTO.LeftOverWeight = 0;// not used. Threated as unique piece material, that has no leftovers 

                            //================================================================================================================
                            materialDTO.Area = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                            materialDTO.TotalArea = Math.Round(materialDTO.Area * materialDTO.Quantity, 6);
                            materialDTO.RequiredArea = materialDTO.TotalArea; // not used. Threated as unique piece material.
                            materialDTO.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 

                            //================================================================================================================
                            materialDTO.Waste = 0; // not used, glasses are not cut in production. Threated as piece material. 

                            //===================================================================================================
                            materialDTO.Price = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double price) ? price : 0;
                            materialDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalPrice) ? totalPrice : 0;
                            materialDTO.RequiredPrice = materialDTO.TotalPrice; // not used. Threated as unique piece material 
                            materialDTO.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 

                            //===================================================================================================
                            materialDTO.SquareMeterPrice = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double squareMeterPrice) ? squareMeterPrice : 0;

                            //===================================================================================================
                            materialDTO.Pallet = a2pWorksheet.WorksheetData[i][12].ToString();

                            //===================================================================================================
                            materialDTO.CustomField1 = null; // not used
                            materialDTO.CustomField2 = null; // not used 
                            materialDTO.CustomField3 = null; // not used
                            materialDTO.CustomField4 = null; // not used 
                            materialDTO.CustomField5 = null; // not used

                            //================================================================================================================
                            materialDTO.MaterialType = MaterialType.Glasses;
                            //===================================================================================================

                            _progressValue.ProgressTask3 = $"Glasses {sortOrder} of {a2pWorksheet.RowCount - 5} - {materialDTO.Description}";
                            _progress?.Report(_progressValue);

                            materialsDTO.Add(materialDTO);

                            await LogMappedMaterialDTOAsync(materialDTO);

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
                                a2pWorksheet.Order ?? string.Empty,
                                a2pWorksheet.Name ?? string.Empty,
                                materialDTO.SourceReference ?? string.Empty,
                                materialDTO.SourceDescription ?? string.Empty,
                                materialDTO.ReferenceBase ?? string.Empty,
                                materialDTO.Reference ?? string.Empty,
                                materialDTO.Description ?? string.Empty,
                                 ex.Message ?? string.Empty);

                            a2pErrors.Add(new A2PError()
                            {
                                Order = a2pWorksheet.Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapGlassesAsync)}, " +
                         $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                         $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                         $"\nReference: {materialDTO.SourceReference ?? string.Empty}," +
                         $"\nColor: {materialDTO.SourceColor ?? string.Empty}," +
                         $"\nItem: {materialDTO.Item ?? string.Empty}," +
                         $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                         $"\nException: {ex.Message ?? string.Empty}."
                            });
                            continue;
                        }

                    }

                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                });

                return (materialsDTO, a2pErrors);

            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapGlassesAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task<(List<MaterialDTO>, List<A2PError>)> MapOthersAsync(A2PWorksheet a2pWorksheet)
        {

            int sortOrder = -1;
            int line = -1;

            List<MaterialDTO> materialsDTO = [];
            List<A2PError> a2pErrors = [];

            try
            {

                await Task.Run(async () =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {
                        MaterialDTO materialDTO = new();
                        sortOrder++;

                        line = i + 1;
                        try
                        {

                            materialDTO.Line = line;
                            materialDTO.WorksheetType = WorksheetType.Materials;
                            materialDTO.Item = null;// not used in others
                            materialDTO.SortOrder = -1;// not used in others
                            materialDTO.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                            materialDTO.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                            materialDTO.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                            materialDTO.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();
                            //===================================================================================================
                            materialDTO.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                            materialDTO.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                            if ((string.IsNullOrEmpty(materialDTO.Color) && string.IsNullOrEmpty(materialDTO.ColorDescription)) || materialDTO.ColorDescription.Contains("Mill finished") || materialDTO.Color == "MF")
                            {
                                materialDTO.Color = "Without";
                            }
                            //===================================================================================================
                            materialDTO.ReferenceBase = $"ASSA_{a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty}";
                            if (materialDTO.Color != "Without")
                            {

                                (string, A2PError?) result = TransformReference(materialDTO.ReferenceBase, materialDTO.Color, a2pWorksheet, line);
                                if (string.IsNullOrEmpty(result.Item1))
                                {
                                    continue;
                                }
                                materialDTO.Reference = result.Item1;
                                if (result.Item2 != null)
                                {
                                    a2pErrors.Add(result.Item2);
                                }

                            }
                            else
                            {
                                materialDTO.Reference = materialDTO.ReferenceBase;
                            }
                            materialDTO.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                            //===================================================================================================
                            materialDTO.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                            materialDTO.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                            materialDTO.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                            materialDTO.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                            materialDTO.LeftOverQuantity = Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6) < 0 ? 0 : Math.Round(materialDTO.TotalQuantity - materialDTO.RequiredQuantity, 6);
                            //===================================================================================================
                            materialDTO.Width = 0; // not used in others
                            materialDTO.Height = 0; // not used in others
                                                    //================================================================================================================
                            materialDTO.TotalWeight = 0; // not used in others
                            materialDTO.Weight = 0; // not used in others
                            materialDTO.RequiredWeight = 0; // not used in others
                            materialDTO.LeftOverWeight = 0; // not used in others
                                                            //================================================================================================================
                            materialDTO.TotalArea = 0; // not used in others
                            materialDTO.Area = 0; // not used in others
                            materialDTO.RequiredArea = 0; // not used in others
                            materialDTO.LeftOverArea = 0; // not used in others
                                                          //================================================================================================================
                            materialDTO.Waste = 0; // not used in others
                                                   //=================================================================================================                                
                            materialDTO.Price = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double price) ? price : 0;
                            materialDTO.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double totalPrice) ? totalPrice : 0;
                            materialDTO.RequiredPrice = Math.Round(materialDTO.Price * (double)materialDTO.RequiredQuantity, 6);
                            materialDTO.LeftOverPrice = Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6) < 0 ? 0 : Math.Round(materialDTO.TotalPrice - materialDTO.RequiredPrice, 6);
                            //===================================================================================================
                            materialDTO.SquareMeterPrice = 0; // not used in others
                            //================================================================================================================
                            materialDTO.Pallet = null; // not used in others
                                                       //================================================================================================================\

                            if (!string.IsNullOrWhiteSpace(materialDTO.SourceColor))
                            {
                                (string, string)? customColors = SplitColors(materialDTO.SourceColor);


                                if (customColors != null)
                                {
                                    materialDTO.CustomField1 = customColors.Value.Item1; // used for custom color
                                    materialDTO.CustomField2 = customColors.Value.Item2;
                                }

                                else
                                {
                                    materialDTO.CustomField1 = null; // not used
                                    materialDTO.CustomField2 = null; // not used
                                }
                            }
                            else
                            {
                                materialDTO.CustomField1 = null; // not used
                                materialDTO.CustomField2 = null; // not used
                            }
                            materialDTO.CustomField3 = null; // not used in others
                                                             //================================================================================================================
                            materialDTO.CustomField4 = null; // not used in others
                            materialDTO.CustomField5 = null; // not used in others
                                                             //================================================================================================================
                            materialDTO.MaterialType = MaterialType.Piece;
                            //===================================================================================================
                            _progressValue.ProgressTask3 = $"Other materials {sortOrder} of {a2pWorksheet.RowCount - 5} -  {materialDTO.ReferenceBase}_{materialDTO.Color}";
                            _progress?.Report(_progressValue);

                            materialsDTO.Add(materialDTO);

                            await LogMappedMaterialDTOAsync(materialDTO);

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
                                a2pWorksheet.Order ?? string.Empty,
                                a2pWorksheet.Name ?? string.Empty,
                                line,
                                materialDTO.ReferenceBase ?? string.Empty,
                                materialDTO.Reference ?? string.Empty,
                                materialDTO.Description ?? string.Empty,
                                 ex.Message ?? string.Empty);

                            a2pErrors.Add(new A2PError()
                            {
                                Order = a2pWorksheet.Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.MappingService_MapMaterial,
                                Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(MapOthersAsync)}, " +
                                 $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                                 $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                                 $"\nLine {materialDTO.Line}," +
                                 $"\nItem: {materialDTO.Item ?? string.Empty}," +
                                 $"\nDescription: {materialDTO.Description ?? string.Empty}," +
                                 $"\nData: {a2pWorksheet.WorksheetData[i].ToArray().ToString() ?? string.Empty}," +
                                 $"\nException: {ex.Message ?? string.Empty}."
                            });
                            continue;
                        }

                    }

                });
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                return (materialsDTO, a2pErrors);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    "\nWorksheet {$Worksheet}." +
                    "\n{$Exception}",
               nameof(MapperTechDesign),
                    nameof(MapOthersAsync),
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    ex.Message);

                return (materialsDTO, a2pErrors);
            }

        }

        private async Task LogMappedMaterialDTOAsync(MaterialDTO materialDTO)
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
                                                              materialDTO.Order ?? string.Empty,
                                                              materialDTO.Worksheet ?? string.Empty,
                                                              materialDTO.Line,
                                                              materialDTO.Reference ?? string.Empty,
                                                              materialDTO.Description ?? string.Empty,
                                                              materialDTO.Color ?? string.Empty,
                                                              materialDTO.ColorDescription ?? string.Empty,
                                                              materialDTO.Width,
                                                              materialDTO.Height,
                                                              materialDTO.Weight,
                                                              materialDTO.Area,
                                                              materialDTO.Quantity,
                                                              materialDTO.PackageQuantity,
                                                              materialDTO.TotalQuantity,
                                                              materialDTO.RequiredQuantity,
                                                              materialDTO.LeftOverQuantity,
                                                              materialDTO.Waste,
                                                              materialDTO.TotalWeight,
                                                              materialDTO.RequiredWeight,
                                                              materialDTO.LeftOverWeight,
                                                              materialDTO.TotalArea,
                                                              materialDTO.RequiredArea,
                                                              materialDTO.LeftOverArea,
                                                              materialDTO.Price,
                                                              materialDTO.TotalPrice,
                                                              materialDTO.RequiredPrice,
                                                              materialDTO.LeftOverPrice,
                                                              materialDTO.Pallet ?? string.Empty,
                                                              materialDTO.MaterialType.ToString() ?? string.Empty,
                                                              materialDTO.CustomField1 ?? string.Empty,
                                                              materialDTO.CustomField2 ?? string.Empty,
                                                              materialDTO.CustomField3 ?? string.Empty,
                                                              materialDTO.CustomField4 ?? string.Empty,
                                                              materialDTO.CustomField5 ?? string.Empty,
                                                              materialDTO.SquareMeterPrice,
                                                              materialDTO.SourceReference ?? string.Empty,
                                                              materialDTO.SourceDescription ?? string.Empty,

                                                              materialDTO.SourceColor ?? string.Empty,
                                                              materialDTO.SourceColorDescription ?? string.Empty,
                                                              materialDTO.WorksheetType);
            });
        }

        private async Task LogMappedItemDTOAsync(ItemDTO itemDTO)
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
                    itemDTO.Order ?? string.Empty,
                    itemDTO.Worksheet ?? string.Empty,
                    itemDTO.Line,
                    itemDTO.Item ?? string.Empty,
                    itemDTO.SortOrder,
                    itemDTO.Description ?? string.Empty,
                    itemDTO.Quantity,
                    itemDTO.Width,
                    itemDTO.Height,
                    itemDTO.Weight,
                    itemDTO.WeightWithoutGlass,
                    itemDTO.WeightGlass,
                    itemDTO.TotalWeight,
                    itemDTO.TotalWeightGlass,
                    itemDTO.Area,
                    itemDTO.TotalArea,
                    itemDTO.Hours,
                    itemDTO.TotalHours,
                    itemDTO.MaterialCost,
                    itemDTO.LaborCost,
                    itemDTO.Cost,
                    itemDTO.TotalMaterialCost,
                    itemDTO.TotalLaborCost,
                    itemDTO.TotalCost,
                    itemDTO.Price,
                    itemDTO.TotalPrice,
                    itemDTO.CurrencyCode,
                    itemDTO.ExchangeRateEUR,
                    itemDTO.MaterialCostEUR,
                    itemDTO.LaborCostEUR,
                    itemDTO.CostEUR,
                    itemDTO.TotalMaterialCostEUR,
                    itemDTO.TotalLaborCostEUR,
                    itemDTO.TotalCostEUR,
                    itemDTO.PriceEUR,
                    itemDTO.TotalPriceEUR,
                    itemDTO.WorksheetType.ToString() ?? string.Empty
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

        private (string, A2PError?) TransformReference(string sapaReference, string sapaColor, A2PWorksheet a2pWorksheet, int line)
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
                    a2pWorksheet.Order ?? string.Empty,
                    a2pWorksheet.Name ?? string.Empty,
                    initialReference ?? string.Empty,
                    initialColor ?? string.Empty);

                    A2PError a2pError = new()
                    {
                        Order = a2pWorksheet.Order ?? string.Empty,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.MappingService_MapMaterial,
                        Message = $"Error Sapa article and color are empty" +
                       $"\nLine will be skipped." +
                       $"\nOrder: {a2pWorksheet.Order ?? string.Empty}, " +
                       $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}, " +
                       $"\nReference: {initialReference ?? string.Empty}, " +
                       $"\nColor: {initialColor ?? string.Empty}"

                    };

                    return (string.Empty, a2pError);
                }

                if (a2pWorksheet.Name is "ND_Gaskets" or "ND_Accessories")
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
                if (a2pWorksheet.Name is "ND_Profiles" or "ND_Accessories" or "ND_Gaskets")
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
                           a2pWorksheet.Order ?? string.Empty,
                           a2pWorksheet.Name ?? string.Empty,
                           initialReference ?? string.Empty,
                           initialColor ?? string.Empty,
                           reference,
                           reference.Length,
                           newReference,
                           newReference.Length);

                        A2PError a2pError = new()
                        {
                            Order = a2pWorksheet.Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Mapper Sapa 2: Generated material Reference is > 25 characters!" +
                           $"\nLine will be skipped." +
                           $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                           $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}, " +
                           $"\nReference: {initialReference ?? string.Empty}, length:{(initialReference ?? string.Empty).Length})." +
                           $"\nColor: {sapaColor ?? string.Empty}, length:{(initialColor ?? string.Empty).Length})." +
                           $"\nGenerated PrefSuite Reference: {reference}, length:{reference.Length}." +
                           $"\nReference inserted into DB: {newReference}, length{newReference.Length}." +
                           "\n"
                        };

                        reference = newReference; // Use the new reference
                        return (reference, a2pError);
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
                    a2pWorksheet.Order ?? string.Empty,
                    ex.Message);

                A2PError a2pError = new()
                {
                    Order = a2pWorksheet.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.MappingService_MapMaterial,
                    Message = $"Unhandled Error {nameof(MapperTechDesign)}.{nameof(TransformReference)}, " +

                   $"\nOrder: {a2pWorksheet.Order ?? string.Empty}," +
                   $"\nWorksheet: {a2pWorksheet.Name ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}."
                };

                return (reference, a2pError);
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