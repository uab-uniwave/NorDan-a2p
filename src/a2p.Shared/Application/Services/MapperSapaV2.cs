// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class MapperSapaV2 : IMapperSapaV2
    {
        private readonly ILogService _logService;
        private ProgressValue _progressValue;
        private DataCache _dataCache;
        private IProgress<ProgressValue>? _progress;
        private IPrefSuiteService _prefSuiteService;

        public MapperSapaV2(ILogService logService, IExcelReadService excelReadService, DataCache dataCache, IPrefSuiteService prefSuiteService)
        {
            _logService = logService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
            _prefSuiteService = prefSuiteService;
            _logService = logService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
        }

        public async Task MapItemsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            // Set the culture globally

            List<ItemDTO> items = [];

            A2POrder? a2pOrder = _dataCache.GetOrder(a2pWorksheet.Order);

            try
            {

                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                int line = -1;
                int column = -1;

                await Task.Run(() =>
                {

                    int rowCounter = 0;

                    double totalSellingPrice = double.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][20].ToString(), out double orderPrice) ? orderPrice : 0;
                    double totalQuotePrice = double.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][22].ToString(), out double orderDiscount) ? orderDiscount : 0;
                    double discountCoeficient = 1;
                    if (totalSellingPrice != 0)
                    {
                        discountCoeficient = totalQuotePrice / totalSellingPrice;
                    }
                    for (int i = 1; i < (a2pWorksheet.RowCount - 1); i++)
                    {

                        rowCounter++;
                        line = i + 1;
                        _progressValue.ProgressTask3 = $"Reading row {rowCounter} of {a2pWorksheet.RowCount - 1}";
                        _progress?.Report(_progressValue);

                        ItemDTO item = new()
                        {

                            Order = a2pWorksheet.Order ?? string.Empty,
                            Worksheet = a2pWorksheet.Name ?? string.Empty,
                            Line = line,
                            Column = column,
                            Item = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty,
                            SortOrder = int.TryParse(a2pWorksheet.WorksheetData[i][1].ToString(), out int sortOrder) ? sortOrder - 1 : -1,
                            Description = a2pWorksheet.WorksheetData[i][0].ToString(),
                            Quantity = int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 0,
                            Width = double.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out double width) ? width : 0,
                            Height = double.TryParse(a2pWorksheet.WorksheetData[i][4].ToString(), out double height) ? height : 0,
                            Weight = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double weight) ? weight : 0,
                            WeightGlass = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double weightGlass) ? weightGlass : 0,
                            LaborCost = double.TryParse(a2pWorksheet.WorksheetData[i][17].ToString(), out double laborCost) ? laborCost : 0,
                            Hours = double.TryParse(a2pWorksheet.WorksheetData[i][18].ToString(), out double hours) ? hours : 0,
                            TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][22].ToString(), out double price) ? price : 0,
                            WorksheetType = a2pWorksheet.WorksheetType,
                            CurrencyCode = a2pWorksheet.Currency ?? "NOK"

                        };

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

                        item.TotalPrice = Math.Round(item.TotalPrice * discountCoeficient, 4);
                        item.Price = Math.Round(item.TotalPrice / item.Quantity, 4);

                        item.ExchangeRateEUR = 1; //TODO': Exchange Rate 

                        item.MaterialCostEUR = Math.Round(item.MaterialCost * item.ExchangeRateEUR, 4);
                        item.TotalMaterialCostEUR = Math.Round(item.TotalMaterialCost * item.ExchangeRateEUR, 4);
                        item.TotalLaborCostEUR = Math.Round(item.TotalLaborCost * item.ExchangeRateEUR, 4);
                        item.CostEUR = Math.Round(item.Cost * item.ExchangeRateEUR, 4);
                        item.TotalCostEUR = Math.Round(item.TotalCost * item.ExchangeRateEUR, 4);
                        item.PriceEUR = Math.Round(item.Price * item.ExchangeRateEUR, 4);
                        item.TotalPriceEUR = Math.Round(item.TotalPrice * item.ExchangeRateEUR, 4);

                        item.WorksheetType = WorksheetType.Items;
                        items.Add(item);

                        _logService.Debug("MS2: Map Items: | Order: {$Order} " +
                                                            "| Worksheet: {$Worksheet} " +
                                                            "| Line: {$Line} " +
                                                            "| Item: {$Item} " +
                                                            "| SortOrder: {$SortOrder} " +
                                                            "| Message: {$Message} " +
                                                            "| Quantity: {$Quantity} " +
                                                            "| Width: {$Width} " +
                                                            "| Height: {$Height} " +
                                                            "| Weight: {$Weight} " +
                                                            "| WeightGlass: {$WeightGlass} " +
                                                            "| WeightWithoutGlass: {$WeightWithoutGlass} " +
                                                            "| TotalWeight: {$TotalWeight} " +
                                                            "| TotalWeightGlass: {$TotalWeightGlass} " +
                                                            "| TotalWeightWithoutGlass: {$TotalWeightWithoutGlass} " +
                                                            "| Area: {$Area} " +
                                                            "| TotalArea: {$TotalArea} " +
                                                            "| Hours: {$Hours} " +
                                                            "| TotalHours: {$TotalHours} " +
                                                            "| MaterialCost: {$MaterialCost} " +
                                                            "| LaborCost: {$LaborCost} " +
                                                            "| Cost: {$Cost} " +
                                                            "| TotalMaterialCost: {$TotalMaterialCost} " +
                                                            "| TotalLaborCost: {$TotalLaborCost} " +
                                                            "| TotalCost: {$TotalCost} " +
                                                            "| Price: {$Price} " +
                                                            "| TotalPrice: {$TotalPrice} " +
                                                            "| CurrencyCode: {$CurrencyCode} " +
                                                            "| ExchangeRateEUR: {$ExchangeRateEUR} " +
                                                            "| MaterialCostEUR: {$MaterialCostEUR} " +
                                                            "| TotalMaterialCostEUR: {$TotalMaterialCostEUR} " +
                                                            "| TotalLaborCostEUR: {$TotalLaborCostEUR} " +
                                                            "| CostEUR: {$CostEUR} " +
                                                            "| TotalCostEUR: {$TotalCostEUR} " +
                                                            "| PriceEUR: {$PriceEUR} " +
                                                            "| TotalPriceEUR: {$TotalPriceEUR} " +
                                                            "| WorksheetType: {$WorksheetType} ",
                                                            item.Order.ToString() ?? string.Empty,
                                                            item.Worksheet.ToString() ?? string.Empty,
                                                            item.Line,
                                                            item.Item.ToString() ?? string.Empty,
                                                            item.SortOrder.ToString() ?? string.Empty,
                                                            item.Description ?? string.Empty,
                                                            item.Quantity,
                                                            item.Width,
                                                            item.Height,
                                                            item.Weight,
                                                            item.WeightGlass,
                                                            item.WeightWithoutGlass,
                                                            item.TotalWeight,
                                                            item.TotalWeightGlass,
                                                            item.TotalWeightWithoutGlass,
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
                                                            item.CurrencyCode ?? "Unknown",
                                                            item.ExchangeRateEUR,
                                                            item.MaterialCostEUR,
                                                            item.TotalMaterialCostEUR,
                                                            item.TotalLaborCostEUR,
                                                            item.CostEUR,
                                                            item.TotalCostEUR,
                                                            item.PriceEUR,
                                                            item.TotalPriceEUR,
                                                            item.WorksheetType.ToString());
                    }

                    if (string.IsNullOrEmpty(a2pOrder!.Currency) &&
                               (!string.IsNullOrEmpty(a2pWorksheet.Currency)))
                    {
                        a2pOrder.Currency = a2pWorksheet.Currency;
                    }

                });

                _dataCache.UpdateOrderInCache(a2pOrder!.Order, updatedOrder =>
                {
                    updatedOrder.Items.AddRange(items);
                });

            }

            catch (Exception ex)
            {
                _logService.Error("Mapper Sapa V2: Unhandled error in Order: {$Order}, Exception:{Exception,} ", a2pWorksheet.Order, ex.Message);
                A2POrderError writeError = new()
                {
                    Order = a2pWorksheet.Order,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.MappingService_MapItem,
                    Message = $"Mapper Sapa V2: Unhandled error mapping Items in Order: {a2pWorksheet.Order}, Exception:{ex.Message}"
                };

                a2pOrder!.WriteErrors.Add(writeError);

            }
            finally
            {

            }

        }

        //============================================================================================================
        public async Task MapMaterialsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            int sortOrder = -1;
            int line = -1;
            int column = -1;

            A2POrder a2pOrder = _dataCache.GetOrder(a2pWorksheet.Order)!;
            List<A2POrderError> a2pOrderErrors = [];
            List<MaterialDTO> materials = [];
            try
            {

                await Task.Run(async () =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {

                        line = i + 1;
                        try
                        {

                            MaterialDTO material = new()
                            {
                                Line = line,
                                Column = column,
                                WorksheetType = a2pWorksheet.WorksheetType
                            };

                            if (a2pWorksheet.Name is "ND_Glasses")
                            {
                                sortOrder++;

                                material.SortOrder = sortOrder;
                                material.Item = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                                material.Description = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;

                                if (string.IsNullOrEmpty(material.Description))
                                {
                                    _logService.Error("Mapper Sapa V2: Error. Glass description is empty. Can't get reference.Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", a2pWorksheet.Order ?? "Unknown", a2pWorksheet.Name, line);
                                    if (a2pOrder != null)
                                    {
                                        a2pOrderErrors.Add(new()
                                        {
                                            Order = a2pWorksheet.Order!,
                                            Level = ErrorLevel.Error,
                                            Code = ErrorCode.MappingService_MapMaterial,
                                            Message = $"Mapper Sapa V2: Glass Description is missing. Line will be skipped. Order: {a2pWorksheet.Order}, Worksheet: {a2pWorksheet.Name}, LineNumber: {line}."
                                        });
                                    }

                                    continue;
                                }

                                string? result = await GetGlassReferenceAsync(material.Description, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.FileName ?? string.Empty, line);

                                if (string.IsNullOrEmpty(result))
                                {

                                    A2POrderError writeError = new()
                                    {
                                        Order = a2pWorksheet.Order!,
                                        Level = ErrorLevel.Error,
                                        Code = ErrorCode.MappingService_MapMaterial,
                                        Message = $"Mapper Sapa 2 : Glass for {material.Description} not exists in PrefSuite. Line will be skipped. Order: {a2pWorksheet.Order}, Worksheet: {a2pWorksheet.Name}, LineNumber: {line}."
                                    };

                                    continue;

                                }

                                material.ReferenceBase = result!;
                                material.Reference = result!;

                                //===================================================================================================
                                material.Color = string.Empty; // not used in glasses
                                material.ColorDescription = null; // not used in glasses
                                                                  //================================================================================================================
                                material.Width = double.TryParse(a2pWorksheet.WorksheetData[i][4].ToString(), out double width) ? width : 0;
                                material.Height = double.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out double height) ? height : 0;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][3] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = 0;
                                material.TotalQuantity = material.Quantity;
                                material.RequiredQuantity = material.TotalQuantity;
                                material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                              //================================================================================================================
                                material.Weight = double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double weight) ? weight : 0;
                                material.TotalWeight = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double totalWeight) ? totalWeight : 0;
                                material.RequiredWeight = material.TotalWeight;
                                material.LeftOverWeight = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                            //================================================================================================================
                                material.Area = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                                material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                                material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                          //================================================================================================================
                                material.Waste = 0; // not used, glasses are not cut in production. Threated as piece material. 
                                                    //===================================================================================================
                                material.Price = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double price) ? price : 0;
                                material.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = material.TotalPrice; // not used. Threated as unique piece material 
                                material.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 
                                //===================================================================================================
                                material.SquareMeterPrice = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double squareMeterPrice) ? squareMeterPrice : 0;
                                //===================================================================================================
                                material.Pallet = a2pWorksheet.WorksheetData[i][12].ToString();
                                //===================================================================================================
                                material.CustomField1 = null; // not used in glasses
                                material.CustomField2 = null; // not used in glasses
                                material.CustomField3 = null; // not used in glasses
                                                              //================================================================================================================
                                material.CustomField4 = null; // not used in glasses
                                material.CustomField5 = null; // not used in glasses
                                                              //================================================================================================================
                                material.MaterialType = MaterialType.Glasses;
                                //===================================================================================================
                                material.SourceReference = null;
                                material.SourceDescription = a2pWorksheet.WorksheetData[i][2]?.ToString();
                                material.SourceColor = null;
                                material.SourceColorDescription = null;

                            }
                            if (a2pWorksheet.Name is "ND_Panels")
                            {

                                material.Item = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                                //Reset Sort Order if new item
                                //===================================================================================================

                                sortOrder++;
                                material.SortOrder = sortOrder;
                                //===================================================================================================
                                // material.Reference  //TODO: Add Reference for Glass from description field   
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                // Pattern to match "XPS <number>mm"
                                string pattern1 = @"(XPS\s+\d+mm)";

                                Match match = Regex.Match(material.Description, pattern1);
                                material.ReferenceBase = match.Success
                                    ? $"LOB_XPS{match.Groups[1].Value}"
                                    : string.Empty;

                                material.Reference = match.Success ?
                                    $"LOB_XPS{match.Groups[1].Value}"
                                    : string.Empty;

                                material.Reference = GetSapa_V2Code(material.ReferenceBase, a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);
                                //===================================================================================================
                                material.Reference = string.IsNullOrEmpty(material.Reference) && material.Description == "1mm aluminium sheet"
                                    ? GetSapa_V2Code("AluSheet1", a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line)
                                    : GetSapa_V2Code(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);

                                //===================================================================================================
                                material.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;  // not used in glasses
                                                                                                                          //================================================================================================================
                                material.Width = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double width) ? width : 0;
                                material.Height = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double height) ? height : 0;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = 0;
                                material.TotalQuantity = material.Quantity;
                                material.RequiredQuantity = material.TotalQuantity;
                                material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                              //================================================================================================================
                                material.Weight = 0;// not used in panels
                                material.TotalWeight = 0;// not used in panels
                                material.RequiredWeight = 0;// not used in panels
                                material.LeftOverWeight = 0;// not used in panels
                                                            //================================================================================================================
                                material.Area = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                                material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                                material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                          //================================================================================================================
                                material.Waste = 0; // not used, panels are not cut in production. Threated as piece material. 
                                                    //===================================================================================================
                                material.Price = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double price) ? price : 0;
                                material.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = material.TotalPrice; // not used. Threated as unique piece material 
                                material.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 
                                //===================================================================================================
                                material.SquareMeterPrice = double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double squareMeterPrice) ? squareMeterPrice : 0;
                                //===================================================================================================
                                material.Pallet = null;
                                //===================================================================================================
                                material.CustomField1 = null; // not used in glasses
                                material.CustomField2 = null; // not used in glasses
                                material.CustomField3 = null; // not used in glasses
                                                              //================================================================================================================
                                material.CustomField4 = null; // not used in glasses
                                material.CustomField5 = null; // not used in glasses
                                                              //================================================================================================================
                                material.MaterialType = MaterialType.Glasses;
                                //===================================================================================================
                                material.SourceReference = null;
                                material.SourceDescription = a2pWorksheet.WorksheetData[i][4]?.ToString();
                                material.SourceColor = a2pWorksheet.WorksheetData[i][2]?.ToString();
                                material.SourceColorDescription = a2pWorksheet.WorksheetData[i][3] == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();

                            }
                            if (a2pWorksheet.Name is "ND_Profiles")
                            {
                                material.Item = null; // not used in profiles
                                material.SortOrder = -1; // not used in profiles
                                //===================================================================================================
                                material.ReferenceBase = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                                material.Reference = GetSapa_V2Code(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                //===================================================================================================
                                material.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                material.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                material.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);
                                //===================================================================================================
                                material.Width = material.PackageQuantity * 1000; //used as bar length in mm
                                material.Height = 0;
                                //===================================================================================================
                                material.TotalWeight = a2pWorksheet.WorksheetData[i][11] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalWeight) ? totalWeight : 0;
                                material.Weight = material.TotalQuantity == 0 ? 0 : Math.Round(material.TotalWeight / material.TotalQuantity, 6);
                                material.RequiredWeight = Math.Round(material.Weight * material.RequiredQuantity, 6);
                                material.LeftOverWeight = Math.Round(material.TotalWeight - material.RequiredWeight, 6) < 0 ? 0 : Math.Round(material.TotalWeight - material.RequiredWeight, 6);
                                //===================================================================================================
                                material.TotalArea = a2pWorksheet.WorksheetData[i][10] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double totalArea) ? totalArea : 0;
                                material.Area = material.TotalQuantity == 0 ? 0 : Math.Round(material.TotalArea / material.TotalQuantity, 6);
                                material.RequiredArea = Math.Round(material.Area * material.RequiredQuantity, 6);
                                material.LeftOverArea = Math.Round(material.TotalArea - material.RequiredArea, 6) < 0 ? 0 : Math.Round(material.TotalArea - material.RequiredArea, 6);
                                //===================================================================================================
                                material.Waste = material.RequiredWeight != 0
                                    ? a2pWorksheet.WorksheetData[i][9] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double lostWeight) ? lostWeight : 0 / material.RequiredWeight * 100
                                    : 0;
                                //===================================================================================================                                
                                material.Price = double.TryParse(a2pWorksheet.WorksheetData[i][12].ToString(), out double price) ? price : 0;
                                material.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][13].ToString(), out double totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (double)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in profiles 
                                                               //================================================================================================================
                                material.Pallet = null;
                                //===================================================================================================
                                material.CustomField1 = null; // not used in glasses
                                material.CustomField2 = null; // not used in glasses
                                material.CustomField3 = null; // not used in glasses
                                                              //================================================================================================================
                                material.CustomField4 = null; // not used in glasses
                                material.CustomField5 = null; // not used in glasses
                                                              //================================================================================================================
                                material.MaterialType = MaterialType.Profiles;
                                //===================================================================================================
                                material.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                                material.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                                material.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                                material.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();
                            }
                            if (a2pWorksheet.Name is "ND_Other")
                            {
                                material.Item = null; // not used in others
                                material.SortOrder = -1; // not used in others
                                //===================================================================================================
                                material.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                if (string.IsNullOrEmpty(material.Color) && string.IsNullOrEmpty(material.ColorDescription)
                                | material.ColorDescription.Contains("Without finish"))
                                {
                                    material.Color = "Without";
                                }
                                //===================================================================================================
                                material.ReferenceBase = $"ASA_{a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty}";
                                material.Reference = material.Color != "Without"
                               ? GetSapa_V2Code(material.ReferenceBase, material.SourceColor, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line)
                                        : material.ReferenceBase;
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                material.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                material.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
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
                                material.Price = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double price) ? price : 0;
                                material.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (double)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in others
                                                               //================================================================================================================
                                material.Pallet = null; // not used in others
                                //================================================================================================================\
                                material.CustomField1 = null; // not used in others
                                material.CustomField2 = null; // not used in others
                                material.CustomField3 = null; // not used in others
                                                              //================================================================================================================
                                material.CustomField4 = null; // not used in others
                                material.CustomField5 = null; // not used in others
                                //================================================================================================================
                                material.MaterialType = MaterialType.Piece;
                                //===================================================================================================
                                material.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                                material.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                                material.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                                material.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();
                            }
                            if (a2pWorksheet.Name is "ND_Accessories")
                            {
                                material.Item = null; // not used in accessories
                                material.SortOrder = -1; // not used in accessories
                                                         //================================================================================================================                                                         
                                material.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;

                                if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                {
                                    material.Color = "Without";
                                }
                                //================================================================================================================       
                                material.ReferenceBase = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                                material.Reference = material.Color != "Without"
                              ? GetSapa_V2Code(material.ReferenceBase, material.Color, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line)
                              : material.ReferenceBase;
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                material.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                material.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);
                                //===================================================================================================
                                material.Width = 0; // not used in accessories
                                material.Height = 0; // not used in accessories
                                //================================================================================================================
                                material.TotalWeight = 0; // not used in accessories
                                material.Weight = 0; // not used in accessories
                                material.RequiredWeight = 0; // not used in accessories
                                material.LeftOverWeight = 0; // not used in accessories
                                //===================================================================================================
                                material.TotalArea = 0; // not used in accessories
                                material.Area = 0; // not used in accessories
                                material.RequiredArea = 0; // not used in accessories
                                material.LeftOverArea = 0; // not used in accessories
                                //===================================================================================================
                                material.Waste = 0; // not used in accessories
                                                    //=================================================================================================                                
                                material.Price = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double price) ? price : 0;
                                material.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (double)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in accessories
                                                               //================================================================================================================
                                material.Pallet = null;
                                //===================================================================================================\
                                material.CustomField1 = null; // not used in accessories
                                material.CustomField2 = null; // not used in accessories
                                material.CustomField3 = null; // not used in accessories
                                                              //================================================================================================================
                                material.CustomField4 = null; // not used in accessories
                                material.CustomField5 = null; // not used in accessories
                                                              //================================================================================================================
                                material.MaterialType = MaterialType.Piece;
                                //===================================================================================================
                                material.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                                material.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                                material.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                                material.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();
                            }
                            if (a2pWorksheet.Name is "ND_Gaskets")
                            {

                                material.Item = null; // not used in accessories
                                material.SortOrder = -1; // not used in accessories
                                //===================================================================================================
                                material.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                {
                                    material.Color = "Without";
                                }
                                //===================================================================================================
                                material.ReferenceBase = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                                material.Reference = material.Color != "Without"
                                    ? GetSapa_V2Code(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, material.Color ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line)
                                    : material.ReferenceBase;
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 0;
                                material.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                material.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);
                                //===================================================================================================
                                if (!string.IsNullOrEmpty(a2pWorksheet.WorksheetData[i][9]?.ToString()))
                                {
                                    //Extract dimmensions from gasket matearial description 
                                    if (a2pWorksheet.WorksheetData[i][9]?.ToString()?.Contains('/') == true)
                                    {
                                        string[] split = a2pWorksheet.WorksheetData[i][9]?.ToString()?.Split('/') ?? Array.Empty<string>();
                                        if (split.Length == 2)
                                        {
                                            material.Width = double.TryParse(split[0], out double width) ? width : 0;
                                            material.Height = double.TryParse(split[1], out double height) ? height : 0;
                                        }
                                    }

                                }
                                else
                                {

                                    material.Width = 0; // not used in accessories
                                    material.Height = 0; // not used in accessories
                                }
                                //===================================================================================================
                                material.TotalWeight = 0; // not used in accessories
                                material.Weight = 0; // not used in accessories
                                material.RequiredWeight = 0; // not used in accessories
                                material.LeftOverWeight = 0; // not used in accessories
                                                             //================================================================================================================
                                material.TotalArea = 0; // not used in accessories
                                material.Area = 0; // not used in accessories
                                material.RequiredArea = 0; // not used in accessories
                                material.LeftOverArea = 0; // not used in accessories
                                                           //================================================================================================================
                                material.Waste = 0; // not used in accessories
                                                    //=================================================================================================                                
                                material.Price = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double price) ? price : 0;
                                material.TotalPrice = double.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out double totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (double)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in accessories
                                                               //================================================================================================================
                                material.Pallet = null;
                                //===================================================================================================\
                                material.CustomField1 = null; // not used in accessories
                                material.CustomField2 = null; // not used in accessories
                                material.CustomField3 = null; // not used in accessories
                                                              //================================================================================================================
                                material.CustomField4 = null; // not used in accessories
                                material.CustomField5 = null; // not used in accessories
                                                              //================================================================================================================
                                material.MaterialType = MaterialType.Gaskets;
                                //===================================================================================================
                                material.SourceReference = a2pWorksheet.WorksheetData[i][1]?.ToString();
                                material.SourceColor = a2pWorksheet.WorksheetData[i][2].ToString() == null ? null : a2pWorksheet.WorksheetData[i][2].ToString();
                                material.SourceColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() == null ? null : a2pWorksheet.WorksheetData[i][3].ToString();
                                material.SourceDescription = a2pWorksheet.WorksheetData[i][4].ToString() == null ? null : a2pWorksheet.WorksheetData[i][4].ToString();
                            }

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

                            materials.Add(material);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error("Mapper Sapa V2: Unhandled Error. Map single material from material list." +
                                "\nLast known success action. Order: {$Order}, a2pWorksheet: {$Worksheet}, line {$Line}. Exception  ${Exception} ", a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line, ex.Message);
                            continue;
                        }
                    }
                });

                _dataCache.UpdateOrderInCache(a2pWorksheet.Order!, updatedOrder =>
                {
                    updatedOrder.Materials.AddRange(materials);
                    updatedOrder.WriteErrors.AddRange(a2pOrderErrors);
                });

            }
            catch (Exception ex)
            {
                _logService.Error("Mapper Sapa 2 Service: Unhandled Error. Mapping material map materials. " +
                    "\nLast known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line, ex.Message);
            }
        }

        public string GetSapa_V2Code(string sapaArticle_v2, string sapaColor_v2, string order, string worksheetName, int line)
        {

            try
            {

                A2POrder? a2pOrder = _dataCache.GetOrder(order);

                // Log an error if both fields are empty
                if (string.IsNullOrEmpty(sapaArticle_v2) && string.IsNullOrEmpty(sapaColor_v2))
                {
                    _logService.Error("Mapper Sapa V2: Mapping material Article and Color fields are empty." +
                        "\nLine will be skipped. Order: {Order}, FileName: {Worksheet}, LineNumber: {Line}.",
                        order ?? string.Empty, worksheetName ?? string.Empty, line
                    );

                    A2POrderError writeError = new()
                    {
                        Order = order!,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.MappingService_MapMaterial,
                        Message = $"Mapper Sapa V2: Mapping material Article and Color fields are empty. " +
                        $"\nLine will be skipped. Order: {order}, FileName: {worksheetName}, LineNumber: {line}.",
                    };

                    a2pOrder!.WriteErrors.Add(writeError);
                    _dataCache.UpdateOrderInCache(order!, updatedOrder =>
                    {
                        updatedOrder.WriteErrors.Add(writeError);
                    });

                    return "Unknown";
                }

                // If sapaColor_v2 is empty and the worksheet is ND_Gaskets or ND_Accessories, return modified sapaArticle_v2
                if (string.IsNullOrEmpty(sapaColor_v2) && (worksheetName == "ND_Gaskets" || worksheetName == "ND_Accessories"))
                {
                    return sapaArticle_v2.StartsWith("S") ? sapaArticle_v2[1..] : sapaArticle_v2;
                }

                // Processing for ND_Profiles and ND_Accessories
                if (worksheetName is "ND_Profiles" or "ND_Accessories")
                {
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];
                    }

                    if ((sapaColor_v2.EndsWith("F") || sapaColor_v2.EndsWith("R") || sapaColor_v2.EndsWith("M")) && sapaColor_v2 != "MF")
                    {
                        sapaColor_v2 = sapaColor_v2[..^1]; // Remove last character
                    }

                    // Regex pattern to replace cases like "A | R" with "|R"
                    sapaColor_v2 = Regex.Replace(sapaColor_v2, @"[A-Z] \| ([RN])", "|$1");
                }

                // Merge the fields with a '-'
                string merged = $"{sapaArticle_v2}-{sapaColor_v2}";

                // Keep only allowed characters (letters, numbers, dots, and '-')
                string reference = Regex.Replace(merged, @"[^a-zA-Z0-9.\-|]", "");

                // Ensure the final string is not more than 25 characters
                if (reference.Length > 25)
                {
                    string newReference = $"*{reference[..24]}";
                    _logService.Error("Mapper Sapa 2 Service: Warning." +
                        "Reference > 25 characters." +
                        "\nOrder: {Order}, " +
                        "\nWorksheet: {Worksheet}," +
                        "\nLine: {Line}," +
                        "\nSapa Article: {SapaArticle}({SapaArticleLength}):, " +
                        "\nSapa Color: {SapaColor}({SapaColorLength})." +
                        "\n" +
                        "\nGenerated PrefSuite Reference: {Reference}({ReferenceLength})." +
                        "\nReference inserted into DB Reference {NewReference}({NewReferenceLength}).",
                        order ?? string.Empty,
                        worksheetName ?? string.Empty,
                        line,
                        sapaArticle_v2 ?? string.Empty,
                        (sapaArticle_v2 ?? string.Empty).Length,
                        sapaColor_v2 ?? string.Empty,
                        (sapaColor_v2 ?? string.Empty).Length,
                        reference,
                        reference.Length,
                        newReference,
                        newReference.Length
                    );

                    A2POrderError writeError = new()
                    {
                        Order = order!,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.MappingService_MapMaterial,
                        Message = $"Mapper Sapa 2: Generated material Reference is  > 25 characters!" +
                        $"\nLine will be skipped." +
                        $"\nOrder: {order}," +
                        $"\nWorksheet: {worksheetName}, " +
                        $"\nLine: {line}," +
                        $"\nSapa Article: {sapaArticle_v2 ?? string.Empty}({(sapaArticle_v2 ?? string.Empty).Length})." +
                        $"\nSapa Color: {sapaColor_v2 ?? string.Empty}({(sapaColor_v2 ?? string.Empty).Length})." +
                        "\n" +
                        $"\nGenerated PrefSuite Reference: {reference}({reference.Length})." +
                        $"\nReference inserted into DB: {newReference}({newReference.Length})."
                    };

                    a2pOrder!.WriteErrors.Add(writeError);
                    _dataCache.UpdateOrderInCache(order!, updatedOrder =>
                    {
                        updatedOrder.WriteErrors.Add(writeError);
                    });

                    A2POrder? testOrder = _dataCache.GetOrder(order!);
                }

                return reference;
            }
            catch (Exception ex)
            {
                _logService.Error(
                    ex.Message,
                    "Mapper Sapa V2: Error Order {Order}, a2pWorksheet {Worksheet}, line {Line}." +
                    "\nCan't generate PrefSuite reference using Sapa v.2 article: {Article} and Sapa v.2 Color {Color}.",
                    order ?? string.Empty, worksheetName ?? string.Empty, line, sapaArticle_v2, sapaColor_v2
                );
                return "Unknown";
            }
        }

        private async Task<string?> GetGlassReferenceAsync(string description, string order, string worksheetName, int line)
        {

            string tempString = description;
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

            string? reference = await _prefSuiteService.GetGlassReferenceAsync(tempString);

            if (string.IsNullOrEmpty(reference))
            {
                _logService.Error("Mapper Sapa V2: Error glass not exists in PrefSuite.Order: {$Order}, a2pWorksheet: {$Worksheet}, line {$Line}." +
                    "\nUsing Sapa description: {SapaDescription}, transforming into PrefSuite description: {PrefSuiteDescription}." +
                    "\nReference, with such description not exists.", order, worksheetName, line, description, tempString);
                return reference;

            }

            else
            {

                return reference.Trim();
            }

        }

    }
}
