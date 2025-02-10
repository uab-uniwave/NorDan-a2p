using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;

using System.Text.RegularExpressions;

namespace a2p.Shared.Infrastructure.Mappers
{
    public class MapperServiceSapa_V2 : IMapperServiceSapa_V2
    {
        private readonly ILogService _logService;


        public MapperServiceSapa_V2(ILogService logService)
        {
            _logService = logService;

        }




        // Maps a Sapa v2 AppWorksheet to a list of MaterialDTO objects asynchronously
        //=============================================================================================================
        public async Task<List<MaterialDTO>> MapMaterialAsync(A2PWorksheet wr)
        {
            int line = -1;
            int column = -1;

            try
            {
                //validate the excel workbook is not null
                if (wr == null)
                {
                    _logService.Error("MS2: Excel file is null.");
                    return new List<MaterialDTO>();
                }

                //validate the worksheet is not null
                if (string.IsNullOrEmpty(wr.Worksheet))
                {
                    _logService.Error("MS2: Workbook , Worksheet name is empty OR worksheet not exists.");
                    return new List<MaterialDTO>();
                }

                //validate the order number is not null
                if (string.IsNullOrEmpty(wr.Order))
                {
                    _logService.Error("MS2: Worksheet {$Worksheet} Order number is empty OR null", wr.Worksheet);
                    return new List<MaterialDTO>();
                }

                //validate worksheet has rows
                if (wr.RowCount == 0)
                {
                    _logService.Error("MS2: Error at Order: {$Order}, Worksheet: {$Worksheet). Worksheet has no rows!", wr.Worksheet, wr.Order);
                    return new List<MaterialDTO>();
                }

                if (wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MMDTO Sapa v.2.Error at , Order: {$Order}, Worksheet: {$Worksheet). Worksheet has no data!", wr.Order ?? string.Empty, wr.Worksheet ?? string.Empty);
                    return new List<MaterialDTO>();
                }

                List<MaterialDTO> materials = new List<MaterialDTO>();



                await Task.Run(() =>
                {
                    for (int i = 4; i < wr.RowCount + 4; i++)
                    {
                        line = i + 1;
                        try
                        {
                            if (string.IsNullOrEmpty(wr.WorksheetData[i][1].ToString()))
                            {
                                _logService.Error("MMDTO Sapa v.2. Article field empty. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", wr.Order ?? string.Empty, wr.Worksheet ?? string.Empty, line);
                                continue;
                            }

                            MaterialDTO material = new()
                            {
                                Worksheet = wr.Worksheet ?? string.Empty,
                                Order = wr.Order ?? string.Empty,
                                Line = line,
                                Column = column,
                                Reference = "Unknown",
                                Color = "Unknown",
                                SourceReference = wr.WorksheetData[i][1].ToString() ?? string.Empty,
                                SourceColor = wr.WorksheetData[i][2].ToString() ?? string.Empty,
                                SourceColorDescription = wr.WorksheetData[i][3].ToString() ?? string.Empty,
                                SourceDescription = wr.WorksheetData[i][4].ToString() ?? string.Empty,
                                Quantity = 0,
                                RequiredQuantity = double.TryParse(wr.WorksheetData[i][8].ToString(), out double quantityRequired) ? quantityRequired : throw new Exception($"Error Required Quantity is not a number. LineNumber: {line}, Value: {wr.WorksheetData[i][8]}"),
                                WorksheetType = wr.Type,
                                MaterialType = MaterialType.Unknown


                            };

                            if (wr.Worksheet is "ND_Profiles" or "ND_Others" or "ND_Accessories")
                            {
                                material.Description = wr.WorksheetData[i][4].ToString() ?? string.Empty;
                                material.Color = wr.WorksheetData[i][2].ToString() ?? string.Empty;
                                material.ColorDescription = wr.WorksheetData[i][3].ToString() ?? string.Empty;
                                material.Quantity = wr.WorksheetData[i][5] == null ? 1 : int.TryParse(wr.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                material.PackageQuantity = wr.WorksheetData[i][6] == null ? 0 : double.TryParse(wr.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 0;
                                material.TotalQuantity = wr.WorksheetData[i][7] == null ? 0 : double.TryParse(wr.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                material.RequiredQuantity = wr.WorksheetData[i][8] == null ? 0 : double.TryParse(wr.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                material.LeftOverQuantity = material.TotalQuantity - material.RequiredQuantity;

                                if (wr.Worksheet is "ND_Profiles")
                                {
                                    material.Reference = GetSapa_V2Code(wr.WorksheetData[i][1].ToString() ?? string.Empty, wr.WorksheetData[i][2].ToString() ?? string.Empty, wr.Order ?? string.Empty, wr.Worksheet ?? string.Empty, line);
                                    material.Price = decimal.TryParse(wr.WorksheetData[i][12].ToString(), out decimal price) ? price : 0;
                                    material.TotalPrice = decimal.TryParse(wr.WorksheetData[i][13].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                    material.TotalWeight = wr.WorksheetData[i][11] == null ? 0 : double.TryParse(wr.WorksheetData[i][11].ToString(), out double totalWeight) ? totalWeight : 0;
                                    material.TotalArea = wr.WorksheetData[i][10] == null ? 0 : double.TryParse(wr.WorksheetData[i][10].ToString(), out double totalArea) ? totalArea : 0;
                                    material.MaterialType = MaterialType.Profiles;
                                    if (material.TotalQuantity != 0)
                                    {
                                        material.Weight = material.TotalWeight / material.TotalQuantity;
                                        material.Area = material.TotalArea - material.TotalQuantity;
                                    }
                                    else
                                    {
                                        material.Weight = 0;
                                        material.Area = 0;
                                    }

                                    material.RequiredWeight = material.Weight * material.RequiredQuantity;
                                    material.RequiredArea = material.Area * material.RequiredQuantity;
                                    material.RequiredPrice = material.Price * (decimal)material.RequiredQuantity;
                                    if (material.RequiredWeight != 0)
                                    {
                                        material.Waste = (wr.WorksheetData[i][9] == null ? 0 : double.TryParse(wr.WorksheetData[i][9].ToString(), out double lostWeight) ? lostWeight : 0 / material.RequiredWeight * 100);
                                    }
                                    else
                                    {
                                        material.Waste = 0;
                                    }
                                    material.LeftOverWeight = material.TotalWeight - material.RequiredWeight;

                                    material.Area = wr.WorksheetData[i][10] == null ? 0 : double.TryParse(wr.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                    material.RequiredArea = material.Area * material.RequiredQuantity;
                                    material.LeftOverArea = material.TotalArea - material.RequiredArea;

                                    material.Width = material.PackageQuantity * 1000;
                                }

                                if (wr.Worksheet == "ND_Others" || wr.Worksheet == "ND_Accessories" || wr.Worksheet == "ND_Gaskets")
                                {
                                    material.Description = wr.WorksheetData[i][4].ToString() ?? string.Empty;
                                    material.Color = wr.WorksheetData[i][2].ToString() ?? string.Empty;
                                    material.ColorDescription = wr.WorksheetData[i][3].ToString() ?? string.Empty;

                                    if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                    {
                                        material.Color = "Without";
                                    }

                                    if (wr.Worksheet == "ND_Others")
                                    {
                                        material.MaterialType = MaterialType.Piece;
                                        material.Reference = $"SAPA_{wr.WorksheetData[i][1].ToString() ?? string.Empty}";
                                    }

                                    if (wr.Worksheet == "ND_Accessories")
                                    {
                                        material.MaterialType = MaterialType.Piece;
                                        material.Reference = GetSapa_V2Code(wr.WorksheetData[i][1].ToString() ?? string.Empty, wr.WorksheetData[i][2].ToString() ?? string.Empty, wr.Order ?? string.Empty, wr.Worksheet ?? string.Empty, line);
                                    }

                                    if (wr.Worksheet == "ND_Accessories" || wr.Worksheet == "ND_Others")
                                    {
                                        material.Price = decimal.TryParse(wr.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                        material.TotalPrice = decimal.TryParse(wr.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                                    }

                                    if (wr.Worksheet == "ND_Gaskets")
                                    {
                                        material.MaterialType = MaterialType.Gaskets;
                                        material.Price = decimal.TryParse(wr.WorksheetData[i][10].ToString(), out decimal price) ? price : 0;
                                        material.TotalPrice = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;

                                        if (!string.IsNullOrEmpty(wr.WorksheetData[i][9]?.ToString()))
                                        {
                                            if (wr.WorksheetData[i][9]?.ToString()?.Contains('/') == true)
                                            {
                                                string[] split = wr.WorksheetData[i][9]?.ToString()?.Split('/') ?? Array.Empty<string>();
                                                if (split.Length == 2)
                                                {
                                                    material.Width = double.TryParse(split[0], out double width) ? width : 0;
                                                    material.Height = double.TryParse(split[1], out double height) ? height : 0;
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                            material.WorksheetType = WorksheetType.Materials_Sapa_v2;
#nullable disable
                            _logService.Debug("MS2: Map Materials: | Order : {$Order} " +
                                                                    "| Worksheet {Worksheet$} " +
                                                                    "| Line: {$Line} " +
                                                                    "| Reference : {$Reference}  " +
                                                                    "| Description : {$Description} " +
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
                                                                    material.Order,
                                                                    material.Worksheet,
                                                                    material.Line,
                                                                    material.Reference,
                                                                    material.Description,
                                                                    material.Color,
                                                                    material.ColorDescription,
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
                                                                    material.Pallet,
                                                                    material.MaterialType.ToString(),
                                                                    material.CustomField1,
                                                                    material.CustomField2,
                                                                    material.CustomField3,
                                                                    material.CustomField4,
                                                                    material.CustomField5,
                                                                    material.SquareMeterPrice,
                                                                    material.SourceReference,
                                                                    material.SourceDescription,
                                                                    material.SourceColor,
                                                                    material.SourceColorDescription,
                                                                    material.WorksheetType.ToString());
#nullable enable
                            materials.Add(material);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error("MMDTO Sapa v.2. For Each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line} + ${Exception}", wr.Order ?? string.Empty, wr.Worksheet ?? string.Empty, line, ex.Message);
                            continue;
                        }
                    }
                });

                return materials;
            }
            catch (Exception ex)
            {
                _logService.Error("MMDTO Sapa v.2. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", wr.Order ?? string.Empty, wr.Worksheet ?? string.Empty, line, ex.Message);
                return new List<MaterialDTO>();
            }
        }

        // Maps a Schuco AppWorksheet to a list of MaterialDTO objects asynchronously
        //=============================================================================================================


        // Generates a Sapa code by merging article and color fields
        //=============================================================================================================
        private string GetSapa_V2Code(string sapaArticle_v2, string sapaColor_v2, string order, string worksheetName, int line)
        {
            try
            {


                if (string.IsNullOrEmpty(sapaArticle_v2) && string.IsNullOrEmpty(sapaColor_v2))
                {
                    _logService.Error("MMDTO Sapa v.2. Article and Color fields are empty. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", order ?? string.Empty, worksheetName ?? string.Empty, line);
                }

                if (string.IsNullOrEmpty(sapaColor_v2) && (worksheetName == "ND_Gaskets" || worksheetName == "ND_Accessories"))
                {
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];

                    }

                    return sapaArticle_v2;

                }







                if (worksheetName == "ND_Profiles")
                {

                    // Remove 'S' if it is the first character of field1
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];

                    }
                    // Remove 'S' if it is the first character of field1
                    if ((sapaColor_v2.EndsWith("F") || sapaColor_v2.EndsWith("R")) && sapaColor_v2 != "MF")
                    {
                        sapaColor_v2 = sapaColor_v2[..^1];
                    }
                    _ = sapaColor_v2.Replace("A | N", "|N");
                }
                // Merge the fields with a '-'
                string merged = $"{sapaArticle_v2}-{sapaColor_v2}";

                // Regex pattern to keep only letters, numbers, dots, and '-'
                string reference = Regex.Replace(merged, @"[^a-zA-Z0-9.\-|]", "");


                // Ensure the final string is not more than 25 characters
                if (reference.Length > 25)
                {
                    _logService.Warning("MMDTO Sapa v.2. Generate PrefSuite reference {$Reference}. Length of reference is {$Length} characters.", reference, reference.Length);
                    reference = $"*{reference[..24]}";
                    _logService.Warning("MMDTO Sapa v.2. Generate PrefSuite reference {$Reference}, trimmed from the end to 25 characters.", reference);
                }
                return reference;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MMDTO Sapa v.2. Generate PrefSuite reference using Sapa v.2 article: {$ArticleNumber} and Sapa v.2 Color {$Color}.", new { sapaArticle_v2, sapaColor_v2 });
                return "Unknown";
            }
        }


        public async Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet wr)
        {
            int line = -1;
            int column = -1;

            try
            {
                if (wr == null)
                {
                    _logService.Error("MS2: Mapping Item.  Worksheet is null.");
                    return new List<ItemDTO>();
                }

                if (string.IsNullOrEmpty(wr.Worksheet))
                {
                    _logService.Error("MS2: Mapping Item. Worksheet name is empty or not found.");
                    return new List<ItemDTO>();
                }

                if (string.IsNullOrEmpty(wr.Order))
                {
                    _logService.Error("MS2: Mapping Item. Worksheet {$Worksheet} Order number is empty or null.", wr.Worksheet);
                    return new List<ItemDTO>();
                }

                if (wr.RowCount == 0 || wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MS2: Mapping Item.  Error in Worksheet {$Worksheet}, Order {$Order}. No rows or data found!", wr.Worksheet, wr.Order);
                    return new List<ItemDTO>();
                }

                List<ItemDTO> items = new();
                await Task.Run(() =>
                {
                    for (int i = 1; i < wr.RowCount + 1; i++)
                    {
                        line = i + 1;

                        try
                        {
                            if (string.IsNullOrEmpty(wr.WorksheetData[i][1].ToString()))
                            {
                                _logService.Error("MI2: Item field is empty. Skipping line at Order: {Order}, : Worksheet {Worksheet}, Line: {$Line} ", wr.Order!, wr.Worksheet!, line);
                                continue;
                            }

                            ItemDTO item = new()
                            {

                                Order = wr.Order ?? string.Empty,
                                Worksheet = wr.Worksheet ?? string.Empty,
                                Line = line,
                                Column = column,
                                Item = wr.WorksheetData[i][2].ToString() ?? string.Empty,
                                SortOrder = int.TryParse(wr.WorksheetData[i][1].ToString(), out int sortOrder) ? sortOrder : -1,
                                Description = wr.WorksheetData[i][0].ToString(),
                                Quantity = int.TryParse(wr.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1,
                                Width = double.TryParse(wr.WorksheetData[i][3].ToString(), out double width) ? width : 0,
                                Height = double.TryParse(wr.WorksheetData[i][4].ToString(), out double height) ? height : 0,
                                Weight = double.TryParse(wr.WorksheetData[i][6].ToString(), out double weight) ? weight : 0,
                                WeightGlass = double.TryParse(wr.WorksheetData[i][7].ToString(), out double weightGlass) ? weightGlass : 0,
                                LaborCost = decimal.TryParse(wr.WorksheetData[i][17].ToString(), out decimal laborCost) ? laborCost : 0,
                                Hours = double.TryParse(wr.WorksheetData[i][18].ToString(), out double hours) ? hours : 0,
                                Price = decimal.TryParse(wr.WorksheetData[i][22].ToString(), out decimal price) ? price : 0,
                                WorksheetType = wr.Type,



                            };



                            decimal profileCost = decimal.TryParse(wr.WorksheetData[i][8].ToString(), out decimal profile) ? profile : 0;
                            decimal fittingCost = decimal.TryParse(wr.WorksheetData[i][9].ToString(), out decimal fitting) ? fitting : 0;
                            decimal gasketAccessoriesCost = decimal.TryParse(wr.WorksheetData[i][10].ToString(), out decimal gasketAccessories) ? gasketAccessories : 0;
                            decimal aluminumSheetCost = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal aluminumSheet) ? aluminumSheet : 0;
                            decimal surchargeALuProfilesCost = decimal.TryParse(wr.WorksheetData[i][12].ToString(), out decimal surchargeALuProfiles) ? surchargeALuProfiles : 0;
                            decimal surfaceTreatmentCost = decimal.TryParse(wr.WorksheetData[i][13].ToString(), out decimal surfaceTreatment) ? surfaceTreatment : 0;
                            decimal clientMaterialsCost = decimal.TryParse(wr.WorksheetData[i][14].ToString(), out decimal clientMaterials) ? clientMaterials : 0;
                            decimal glassCost = decimal.TryParse(wr.WorksheetData[i][15].ToString(), out decimal glass) ? glass : 0;
                            decimal panelCost = decimal.TryParse(wr.WorksheetData[i][16].ToString(), out decimal panel) ? panel : 0;
                            decimal specialCost = decimal.TryParse(wr.WorksheetData[i][19].ToString(), out decimal special) ? special : 0;


                            item.WeightWithoutGlass = item.Weight - item.WeightGlass;
                            item.TotalWeight = item.Weight * item.Quantity;
                            item.TotalWeightWithoutGlass = item.WeightWithoutGlass * item.Quantity;
                            item.TotalWeightGlass = item.WeightGlass * item.Quantity;
                            item.Area = item.Width * item.Height / 1000000;
                            item.TotalArea = item.Area * item.Quantity;

                            item.TotalHours = item.Hours * item.Quantity;
                            item.MaterialCost = profileCost + fittingCost + gasketAccessoriesCost + aluminumSheetCost + surchargeALuProfilesCost + surfaceTreatmentCost + clientMaterialsCost + panelCost + glassCost;
                            item.Cost = item.MaterialCost + item.LaborCost;
                            item.TotalMaterialCost = item.MaterialCost * item.Quantity;
                            item.TotalLaborCost = item.LaborCost * item.Quantity;
                            item.TotalCost = item.Cost * item.Quantity;
                            item.TotalPrice = item.Price * item.Quantity;




                            item.CurrencyCode = wr.Currency;

                            if (item.CurrencyCode == "EUR")
                            {
                                item.ExchangeRateEUR = 1;
                            }

                            if (item.ExchangeRateEUR != null)
                            {

                                item.MaterialCostEUR = item.MaterialCost * (item.ExchangeRateEUR ?? 0.0m);
                                item.TotalMaterialCostEUR = item.TotalMaterialCost * (item.ExchangeRateEUR ?? 0.0m);
                                item.TotalLaborCostEUR = item.TotalLaborCost * (item.ExchangeRateEUR ?? 0.0m);
                                item.CostEUR = item.Cost * (item.ExchangeRateEUR ?? 0.0m);
                                item.TotalCostEUR = item.TotalCost * (item.ExchangeRateEUR ?? 0.0m);
                                item.PriceEUR = item.Price * (item.ExchangeRateEUR ?? 0.0m);
                                item.TotalPriceEUR = item.TotalPrice * (item.ExchangeRateEUR ?? 0.0m);
                            }




                            item.WorksheetType = WorksheetType.Items_Sapa_v2;



                            items.Add(item);

# nullable disable
                            _logService.Debug("MS2: Map Items: | Order: {$Order} " +
                                                              "| Worksheet: {$Worksheet} " +
                                                                "| Line: {$Line} " +
                                                                "| Item: {$Item} " +
                                                                "| SortOrder: {$SortOrder} " +
                                                                "| Description: {$Description} " +
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
                                                                item.Order,
                                                                item.Worksheet,
                                                                item.Line,
                                                                item.Item,
                                                                item.SortOrder,
                                                                item.Description,
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
                                                                item.CurrencyCode,
                                                                item.ExchangeRateEUR,
                                                                item.MaterialCostEUR,
                                                                item.TotalMaterialCostEUR,
                                                                item.TotalLaborCostEUR,
                                                                item.CostEUR,
                                                                item.TotalCostEUR,
                                                                item.PriceEUR,
                                                                item.TotalPriceEUR,
                                                                item.WorksheetType.ToString());

# nullable enable







                        }
                        catch (Exception ex)
                        {
                            _logService.Error("MS2: Error processing  Order: {$Order}, Worksheet: {$Worksheet}. Line {$Line} Exception: {3}", wr.Order ?? "Unknown", wr.Worksheet ?? "Unknown", line, ex.Message);
                        }
                    }
                });

                return items;
            }
            catch (Exception ex)
            {
                _logService.Error("MS2: Unhandled error in Order: {$Order}, Worksheet: {$Worksheet}. Line {$Line} Exception: {3}", wr.Order!, wr.Worksheet ?? string.Empty, ex.Message);
                return new List<ItemDTO>();
            }
        }


    }
}