// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class MapperSapaV2 : IMapperSapaV2
    {
        private readonly ILogService _logService;
        private ProgressValue _progressValue;
        private DataCache _dataCache;
        private IProgress<ProgressValue>? _progress;

        public MapperSapaV2(ILogService logService, IExcelReadService excelReadService, DataCache dataCache)
        {
            _logService = logService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
        }

        public async Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            if (a2pWorksheet == null)
            {
                _logService.Error("Mapper Sapa 2 Service: Order a2pWorksheet is null!");
                return [];

            }

            if (string.IsNullOrEmpty(a2pWorksheet.Order))
            {
                _logService.Error("Mapper Sapa 2 Service: Order a2pWorksheet Order is null!");
                return [];
            }

            A2POrder? a2pOrder = _dataCache.GetOrder(a2pWorksheet.Order);

            List<ItemDTO> items = [];

            try
            {
                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();
                _progressValue.ProgressTask3 = $"Reading rows";
                _progress?.Report(_progressValue);

                int line = -1;
                int column = -1;

                if (a2pWorksheet.RowCount == 0)
                {
                    _logService.Error("Mapper Sapa 2 Service: Mapping Item. Order {$Order}, File: {$File}, Worksheet {$Worksheet}, No rows or data found!", a2pWorksheet.Order, a2pWorksheet.FileName, a2pWorksheet.Name);

                    A2POrderError writeError = new()
                    {
                        Order = a2pWorksheet.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.MappingService_MapItem,
                        Message = $"Mapper Sapa V2: Unhandled error in order: {a2pWorksheet.Order}."
                    };

                    a2pOrder!.WriteErrors.Add(writeError);
                    _dataCache.UpdateOrderInCache(a2pOrder.Order, updatedOrder =>
                    {
                        updatedOrder.WriteErrors.Add(writeError);
                    });

                    return items;

                }
                await Task.Run(() =>
                {

                    int rowCounter = 0;

                    decimal totalOrderPrice = decimal.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][22].ToString(), out decimal orderPrice) ? orderPrice : 0;
                    decimal totalOrderDiscount = decimal.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][23].ToString(), out decimal orderDiscount) ? orderDiscount : 0;
                    decimal TotalFinalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[a2pWorksheet.RowCount - 1][20].ToString(), out decimal finalPrice) ? finalPrice : 0;
                    decimal discountCoeficient = 1 - (orderDiscount / totalOrderPrice);

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
                            LaborCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][17].ToString(), out decimal laborCost) ? laborCost : 0,
                            Hours = double.TryParse(a2pWorksheet.WorksheetData[i][18].ToString(), out double hours) ? hours : 0,
                            Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][22].ToString(), out decimal price) ? price : 0,
                            WorksheetType = a2pWorksheet.WorksheetType,
                            CurrencyCode = a2pWorksheet.Currency ?? "NOK"

                        };

                        decimal profileCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out decimal profile) ? profile : 0;
                        decimal fittingCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out decimal fitting) ? fitting : 0;
                        decimal gasketAccessoriesCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out decimal gasketAccessories) ? gasketAccessories : 0;
                        decimal aluminumSheetCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out decimal aluminumSheet) ? aluminumSheet : 0;
                        decimal surchargeALuProfilesCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][12].ToString(), out decimal surchargeALuProfiles) ? surchargeALuProfiles : 0;
                        decimal surfaceTreatmentCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][13].ToString(), out decimal surfaceTreatment) ? surfaceTreatment : 0;
                        decimal clientMaterialsCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][14].ToString(), out decimal clientMaterials) ? clientMaterials : 0;
                        decimal glassCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][15].ToString(), out decimal glass) ? glass : 0;
                        decimal panelCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][16].ToString(), out decimal panel) ? panel : 0;
                        decimal specialCost = decimal.TryParse(a2pWorksheet.WorksheetData[i][19].ToString(), out decimal special) ? special : 0;

                        item.WeightWithoutGlass = Math.Round(item.Weight - item.WeightGlass, 6);
                        item.TotalWeight = Math.Round(item.Weight * item.Quantity, 6);
                        item.TotalWeightWithoutGlass = Math.Round(item.WeightWithoutGlass * item.Quantity, 6);
                        item.TotalWeightGlass = Math.Round(item.WeightGlass * item.Quantity, 6);
                        item.Area = Math.Round(item.Width * item.Height / 1000000, 6);
                        item.TotalArea = Math.Round(item.Area * item.Quantity, 6);

                        item.TotalHours = Math.Round(item.Hours * item.Quantity, 6);
                        item.MaterialCost = Math.Round(profileCost + fittingCost + gasketAccessoriesCost + aluminumSheetCost + surchargeALuProfilesCost + surfaceTreatmentCost + clientMaterialsCost + panelCost + glassCost, 6);
                        item.Cost = Math.Round(item.MaterialCost + item.LaborCost, 6);
                        item.TotalMaterialCost = Math.Round(item.MaterialCost * item.Quantity, 6);
                        item.TotalLaborCost = Math.Round(item.LaborCost * item.Quantity, 6);
                        item.TotalCost = Math.Round(item.Cost * item.Quantity, 6);
                        item.TotalPrice = Math.Round(item.Price * discountCoeficient, 6);
                        item.TotalPrice = Math.Round(item.Price * item.Quantity * discountCoeficient, 6);

                        item.ExchangeRateEUR = 1; //TODO': Exchange Rate 

                        if (item.ExchangeRateEUR != null)
                        {

                            item.MaterialCostEUR = Math.Round(item.MaterialCost * item.ExchangeRateEUR, 6);
                            item.TotalMaterialCostEUR = Math.Round(item.TotalMaterialCost * item.ExchangeRateEUR, 6);
                            item.TotalLaborCostEUR = Math.Round(item.TotalLaborCost * item.ExchangeRateEUR, 6);
                            item.CostEUR = Math.Round(item.Cost * item.ExchangeRateEUR, 6);
                            item.TotalCostEUR = Math.Round(item.TotalCost * item.ExchangeRateEUR, 6);
                            item.PriceEUR = Math.Round(item.Price * item.ExchangeRateEUR, 6);
                            item.TotalPriceEUR = Math.Round(item.TotalPrice * item.ExchangeRateEUR, 6);
                        }

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

                });

                return items;
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
                _dataCache.UpdateOrderInCache(a2pOrder.Order, updatedOrder =>
                {
                    updatedOrder.WriteErrors.Add(writeError);
                });

                return items;
            }
        }

        //============================================================================================================
        public async Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            List<MaterialDTO> materials = [];
            if (a2pWorksheet == null)
            {
                _logService.Error("Mapper Sapa 2 Service: Order a2pWorksheet is null!");
                return materials;
            }

            A2POrder a2pOrder = _dataCache.GetOrder(a2pWorksheet.Order)!;
            //validate the excel workbook is not null
            if (a2pWorksheet.RowCount == 0)

            {
                _logService.Error("Mapper Sapa 2 Service: Order {$Order},a2pWorksheet {$Worksheet} has no rows. File {$File}", a2pWorksheet.Order, a2pWorksheet.FileName, a2pWorksheet.Name);

                if (a2pOrder != null)
                {
                    A2POrderError writeError = new()
                    {
                        Order = a2pWorksheet.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.MappingService_MapItem,
                        Message = $"Mapper Sapa V2: Order {a2pWorksheet.Order},a2pWorksheet {a2pWorksheet.Name} has no rows. File {a2pWorksheet.FileName}"
                    };

                    a2pOrder!.WriteErrors.Add(writeError);
                    _dataCache.UpdateOrderInCache(a2pOrder.Order, updatedOrder =>
                    {
                        updatedOrder.WriteErrors.Add(writeError);
                    });
                }

                return materials;

            }

            string lastItem = string.Empty;
            int sortOrder = -1;
            int line = -1;
            int column = -1;

            try
            {

                await Task.Run(() =>
                {

                    for (int i = 4; i < a2pWorksheet.RowCount; i++)
                    {

                        line = i + 1;
                        try
                        {
                            if (string.IsNullOrEmpty(a2pWorksheet.WorksheetData[i][1].ToString()))
                            {
                                _logService.Warning("Mapper Sapa 2: Reference (Sapa article) field empty. Line will be skipped. Order: {$Order}, Worksheet: {$Worksheet}, LineNumber: {$Line}.", a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);

                                if (a2pOrder != null)
                                {
                                    A2POrderError writeError = new()
                                    {
                                        Order = a2pWorksheet.Order!,
                                        Level = ErrorLevel.Error,
                                        Code = ErrorCode.MappingService_MapMaterial,
                                        Message = $"Mapper Sapa 2 : Reference (Sapa article) field empty. Line will be skipped. Order: {a2pWorksheet.Order}, Worksheet: {a2pWorksheet.Name}, LineNumber: {line}."
                                    };

                                    a2pOrder!.WriteErrors.Add(writeError);
                                    _dataCache.UpdateOrderInCache(a2pOrder.Order, updatedOrder =>
                                    {
                                        updatedOrder.WriteErrors.Add(writeError);
                                    });
                                }
                            }

                            MaterialDTO material = new()
                            {
                                Worksheet = a2pWorksheet.Name ?? string.Empty,
                                Order = a2pWorksheet.Order ?? string.Empty,
                                Line = line,
                                Column = column,
                                //===================================================
                                Quantity = 0,
                                RequiredQuantity = 0,
                                Reference = string.Empty,
                                //===================================================
                                WorksheetType = a2pWorksheet.WorksheetType,
                                MaterialType = MaterialType.Unknown
                            };

                            if (a2pWorksheet.Name is "ND_Glasses")
                            {

                                material.Item = a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty;
                                //Reset Sort Order if new item
                                //===================================================================================================
                                if (material.Item != lastItem)
                                {
                                    lastItem = material.Item;
                                    sortOrder = 0;
                                }
                                sortOrder++;
                                material.SortOrder = sortOrder;
                                //===================================================================================================
                                // material.Reference  //TODO: Add Reference for Glass from description field   
                                material.Description = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                //===================================================================================================
                                material.Color = string.Empty; // not used in glasses
                                material.ColorDescription = null; // not used in glasses
                                                                  //===================================================================================================
                                material.Width = double.TryParse(a2pWorksheet.WorksheetData[i][4].ToString(), out double width) ? width : 0;
                                material.Height = double.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out double height) ? height : 0;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][3] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = 0;
                                material.TotalQuantity = material.Quantity;
                                material.RequiredQuantity = material.TotalQuantity;
                                material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                              //===================================================================================================
                                material.Weight = double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double weight) ? weight : 0;
                                material.TotalWeight = double.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out double totalWeight) ? totalWeight : 0;
                                material.RequiredWeight = material.TotalWeight;
                                material.LeftOverWeight = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                            //===================================================================================================
                                material.Area = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                                material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                                material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                          //===================================================================================================
                                material.Waste = 0; // not used, glasses are not cut in production. Threated as piece material. 
                                                    //===================================================================================================
                                material.Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = material.TotalPrice; // not used. Threated as unique piece material 
                                material.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 
                                //===================================================================================================
                                material.SquareMeterPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
                                //===================================================================================================
                                material.Pallet = a2pWorksheet.WorksheetData[i][12].ToString();
                                //===================================================================================================
                                material.CustomField1 = null; // not used in glasses
                                material.CustomField2 = null; // not used in glasses
                                material.CustomField3 = null; // not used in glasses
                                                              //===================================================================================================
                                material.CustomField4 = null; // not used in glasses
                                material.CustomField5 = null; // not used in glasses
                                                              //===================================================================================================
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
                                if (material.Item != lastItem)
                                {
                                    lastItem = material.Item;
                                    sortOrder = 0;
                                }
                                sortOrder++;
                                material.SortOrder = sortOrder;
                                //===================================================================================================
                                // material.Reference  //TODO: Add Reference for Glass from description field   
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                // Pattern to match "XPS <number>mm"
                                string pattern1 = @"(XPS\s+\d+mm)";
                                Match match = Regex.Match(material.Description, pattern1);
                                material.Reference = match.Success ? $"LOB_XPS{match.Groups[1].Value}" : string.Empty;
                                //===================================================================================================
                                material.Reference = string.IsNullOrEmpty(material.Reference) && material.Description == "1mm aluminium sheet"
                                    ? GetSapa_V2Code("AluSheet1", a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line)
                                    : GetSapa_V2Code(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);

                                //===================================================================================================
                                material.Color = a2pWorksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;  // not used in glasses
                                                                                                                          //===================================================================================================
                                material.Width = double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double width) ? width : 0;
                                material.Height = double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double height) ? height : 0;
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = 0;
                                material.TotalQuantity = material.Quantity;
                                material.RequiredQuantity = material.TotalQuantity;
                                material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                              //===================================================================================================
                                material.Weight = 0;// not used in panels
                                material.TotalWeight = 0;// not used in panels
                                material.RequiredWeight = 0;// not used in panels
                                material.LeftOverWeight = 0;// not used in panels
                                                            //===================================================================================================
                                material.Area = double.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                                material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                                material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                          //===================================================================================================
                                material.Waste = 0; // not used, panels are not cut in production. Threated as piece material. 
                                                    //===================================================================================================
                                material.Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = material.TotalPrice; // not used. Threated as unique piece material 
                                material.LeftOverPrice = 0; /// not used. Threated as unique piece material, that has no leftovers 
                                //===================================================================================================
                                material.SquareMeterPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
                                //===================================================================================================
                                material.Pallet = null;
                                //===================================================================================================
                                material.CustomField1 = null; // not used in glasses
                                material.CustomField2 = null; // not used in glasses
                                material.CustomField3 = null; // not used in glasses
                                                              //===================================================================================================
                                material.CustomField4 = null; // not used in glasses
                                material.CustomField5 = null; // not used in glasses
                                                              //===================================================================================================
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
                                material.Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][12].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][13].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in profiles 
                                                               //===================================================================================================
                                material.Pallet = null;
                                //===================================================================================================
                                material.CustomField1 = null; // not used in glasses
                                material.CustomField2 = null; // not used in glasses
                                material.CustomField3 = null; // not used in glasses
                                                              //===================================================================================================
                                material.CustomField4 = null; // not used in glasses
                                material.CustomField5 = null; // not used in glasses
                                                              //===================================================================================================
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
                                material.Reference = $"ASA_{a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty}";
                                material.Description = a2pWorksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                //===================================================================================================                                                         
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                if (string.IsNullOrEmpty(material.Color) && string.IsNullOrEmpty(material.ColorDescription)
                                | material.ColorDescription.Contains("Without finish"))
                                {
                                    material.Color = "Without";
                                }
                                //===================================================================================================
                                material.Quantity = a2pWorksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(a2pWorksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = a2pWorksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(a2pWorksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                material.TotalQuantity = a2pWorksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                material.RequiredQuantity = a2pWorksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(a2pWorksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);
                                //===================================================================================================
                                material.Width = 0; // not used in others
                                material.Height = 0; // not used in others
                                                     //===================================================================================================
                                material.TotalWeight = 0; // not used in others
                                material.Weight = 0; // not used in others
                                material.RequiredWeight = 0; // not used in others
                                material.LeftOverWeight = 0; // not used in others
                                                             //===================================================================================================
                                material.TotalArea = 0; // not used in others
                                material.Area = 0; // not used in others
                                material.RequiredArea = 0; // not used in others
                                material.LeftOverArea = 0; // not used in others
                                                           //===================================================================================================
                                material.Waste = 0; // not used in others
                                                    //=================================================================================================                                
                                material.Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in others
                                                               //===================================================================================================
                                material.Pallet = null; // not used in others
                                                        //===================================================================================================\
                                material.CustomField1 = null; // not used in others
                                material.CustomField2 = null; // not used in others
                                material.CustomField3 = null; // not used in others
                                                              //===================================================================================================
                                material.CustomField4 = null; // not used in others
                                material.CustomField5 = null; // not used in others
                                                              //===================================================================================================
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
                                                         //===================================================================================================                                                         
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                {
                                    material.Color = "Without";
                                }
                                material.Reference = GetSapa_V2Code(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, material.Color ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);
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
                                                     //===================================================================================================
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
                                material.Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in accessories
                                                               //===================================================================================================
                                material.Pallet = null;
                                //===================================================================================================\
                                material.CustomField1 = null; // not used in accessories
                                material.CustomField2 = null; // not used in accessories
                                material.CustomField3 = null; // not used in accessories
                                                              //===================================================================================================
                                material.CustomField4 = null; // not used in accessories
                                material.CustomField5 = null; // not used in accessories
                                                              //===================================================================================================
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
                                material.ColorDescription = a2pWorksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                {
                                    material.Color = "Without";
                                }
                                material.Reference = GetSapa_V2Code(a2pWorksheet.WorksheetData[i][1].ToString() ?? string.Empty, material.Color ?? string.Empty, a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line);
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
                                                             //===================================================================================================
                                material.TotalArea = 0; // not used in accessories
                                material.Area = 0; // not used in accessories
                                material.RequiredArea = 0; // not used in accessories
                                material.LeftOverArea = 0; // not used in accessories
                                                           //===================================================================================================
                                material.Waste = 0; // not used in accessories
                                                    //=================================================================================================                                
                                material.Price = decimal.TryParse(a2pWorksheet.WorksheetData[i][10].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(a2pWorksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                material.RequiredPrice = Math.Round(material.Price * (decimal)material.RequiredQuantity, 6);
                                material.LeftOverPrice = Math.Round(material.TotalPrice - material.RequiredPrice, 6) < 0 ? 0 : Math.Round(material.TotalPrice - material.RequiredPrice, 6);
                                //===================================================================================================
                                material.SquareMeterPrice = 0; // not used in accessories
                                                               //===================================================================================================
                                material.Pallet = null;
                                //===================================================================================================\
                                material.CustomField1 = null; // not used in accessories
                                material.CustomField2 = null; // not used in accessories
                                material.CustomField3 = null; // not used in accessories
                                                              //===================================================================================================
                                material.CustomField4 = null; // not used in accessories
                                material.CustomField5 = null; // not used in accessories
                                                              //===================================================================================================
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
                            _logService.Error("Mapper Sapa 2 Service: Unhandled Error. Map single material from material list.  Last known success action. Order: {$Order}, a2pWorksheet: {$Worksheet}, line {$Line}. Exception  ${Exception} ", a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line, ex.Message);
                            continue;
                        }
                    }
                });

                return materials;
            }
            catch (Exception ex)
            {
                _logService.Error("Mapper Sapa 2 Service: Unhandled Error. Mapping material map materials. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", a2pWorksheet.Order ?? string.Empty, a2pWorksheet.Name ?? string.Empty, line, ex.Message);
                return materials;
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
                    _logService.Error(
                        "Mapper Sapa 2 Service: Mapping material Article and Color fields are empty. Line will be skipped. Order: {Order}, FileName: {Worksheet}, LineNumber: {Line}.",
                        order ?? string.Empty, worksheetName ?? string.Empty, line
                    );

                    
                        A2POrderError writeError = new()
                        {
                            Order = order!,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapMaterial,
                            Message = $"Mapper Sapa 2: Mapping material Article and Color fields are empty. Line will be skipped. Order: {order}, FileName: {worksheetName}, LineNumber: {line}.",
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
                    _logService.Error(
                        "Mapper Sapa 2 Service: Warning. " +
                        "Reference > 25 characters. \nOrder:{Order}, Worksheet: {Worksheet}, Line: {Line}, " +
                        "\nSapa Article: {SapaArticle}({SapaArticleLength}):, Sapa Color: {SapaColor}({SapaColorLength}). " +
                        "\n Generated PrefSuite Reference: {Reference}({ReferenceLength})." +
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
                        $"\n Line will be skipped. Order: {order}, FileName: {worksheetName}, LineNumber: {line}." +
                        $"\nSapa Article: {sapaArticle_v2 ?? string.Empty}({(sapaArticle_v2 ?? string.Empty).Length}), Sapa Color: {sapaColor_v2 ?? string.Empty}({(sapaColor_v2 ?? string.Empty).Length}). " +
                        $"\n Generated PrefSuite Reference: {reference}({reference.Length})." +
                        $"Reference inserted into DB: {newReference}({newReference.Length})."
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
                    "Mapper Sapa 2 Service: Error Order {Order}, a2pWorksheet {Worksheet}, line {Line}. " +
                    "Can't generate PrefSuite reference using Sapa v.2 article: {Article} and Sapa v.2 Color {Color}.",
                    order ?? string.Empty, worksheetName ?? string.Empty, line, sapaArticle_v2, sapaColor_v2
                );
                return "Unknown";
            }
        }

        public List<A2PFile> GetNonOrderItemFiles(A2POrder order) => order.Files.Where(file => !file.IsOrderItemsFile).ToList();

        public List<A2PFile> GetOrderItemFiles(A2POrder order) => order.Files.Where(file => file.IsOrderItemsFile).ToList();

    }
}
