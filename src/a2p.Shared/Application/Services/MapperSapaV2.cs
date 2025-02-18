using a2p.Shared.Application.Interfaces;
using a2p.Shared.Core.DTO;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using System.Text.RegularExpressions;

namespace a2p.Shared.Application.Services
{
    public class MapperSapaV2 : IMapperSapaV2
    {
        private readonly ILogService _logService;



        public MapperSapaV2(ILogService logService)
        {
            _logService = logService;

        }



        public async Task<A2POrder> MapItemsAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            try
            {
                foreach (A2PFile? file in order.Files.Where(file => file.GetType().GetProperties().Any(prop => prop.Name == "IsOrderItemFile")).ToList())
                {
                    if (file.Worksheets.Count == 0)
                    {
                        _logService.Error("Mapper Sapa 2 Service: Mapping Item. Order  {$Order} {$File} has no worksheets.", order.Order, file);
                        continue;
                    }

                    foreach (A2PWorksheet worksheet in file.Worksheets)
                    {
                        int line = -1;
                        int column = -1;

                        if (worksheet.RowCount == 0)
                        {
                            _logService.Error("Mapper Sapa 2 Service: Mapping Item.  Error in Worksheet {$Worksheet}, Order {$Order}. No rows or data found!", worksheet.Worksheet, order.Order);
                            continue;
                        }


                        await Task.Run(() =>
                        {
                            for (int i = 1; i < worksheet.RowCount + 1; i++)
                            {
                                line = i + 1;

                                try
                                {
                                    if (string.IsNullOrEmpty(worksheet.WorksheetData[i][1].ToString()))
                                    {
                                        _logService.Error("MI2: Item field is empty. Skipping line at Order: {Order}, : Worksheet {Worksheet}, Line: {$Line} ", order.Order!, worksheet.Worksheet!, line);
                                        continue;
                                    }

                                    ItemDTO item = new()
                                    {

                                        Order = order.Order ?? string.Empty,
                                        Worksheet = worksheet.Worksheet ?? string.Empty,
                                        Line = line,
                                        Column = column,
                                        Item = worksheet.WorksheetData[i][2].ToString() ?? string.Empty,
                                        SortOrder = int.TryParse(worksheet.WorksheetData[i][1].ToString(), out int sortOrder) ? sortOrder : -1,
                                        Description = worksheet.WorksheetData[i][0].ToString(),
                                        Quantity = int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 0,
                                        Width = double.TryParse(worksheet.WorksheetData[i][3].ToString(), out double width) ? width : 0,
                                        Height = double.TryParse(worksheet.WorksheetData[i][4].ToString(), out double height) ? height : 0,
                                        Weight = double.TryParse(worksheet.WorksheetData[i][6].ToString(), out double weight) ? weight : 0,
                                        WeightGlass = double.TryParse(worksheet.WorksheetData[i][7].ToString(), out double weightGlass) ? weightGlass : 0,
                                        LaborCost = decimal.TryParse(worksheet.WorksheetData[i][17].ToString(), out decimal laborCost) ? laborCost : 0,
                                        Hours = double.TryParse(worksheet.WorksheetData[i][18].ToString(), out double hours) ? hours : 0,
                                        Price = decimal.TryParse(worksheet.WorksheetData[i][22].ToString(), out decimal price) ? price : 0,
                                        WorksheetType = worksheet.Type,
                                        CurrencyCode = order.Currency ?? "NOK"


                                    };



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
                                    item.TotalPrice = Math.Round(item.Price * item.Quantity, 6);


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

                                        item.WorksheetType = WorksheetType.Items;
                                        order.Items.Add(item);
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

                                }
                                catch (Exception ex)
                                {
                                    _logService.Error("MS2: Error processing  Order: {$Order}, Worksheet: {$Worksheet}. Line {$Line} Exception: {3}", order.Order!, worksheet.Worksheet!, line, ex.Message);
                                }
                            }
                        });
                    }
                }
                return order;
            }




            catch (Exception ex)
            {
                _logService.Error("MS2: Unhandled error in Order: {$Order}, Worksheet: {$Worksheet}. Line {$Line} Exception: {3}", order.Order, worksheet.Worksheet, ex.Message);
                return order;
            }
        }
        // Maps a Sapa v2 AppWorksheet to a list of MaterialDTO objects asynchronously
        //============================================================================================================
        public async Task<A2POrder> MapMaterialsAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            int line = -1;
            int column = -1;
            if (!order.Files.Any())
            {
                _logService.Error("Mapper Sapa 2 Service: Order {$Order} has no files.", order.Order);
                return order;
            }


            foreach (A2PFile file in order.Files.Where(file => !file.GetType().GetProperties().Any(prop => prop.Name == "IsOrderItemFile")).ToList())
            {
                if (!file.Worksheets.Any())
                {
                    _logService.Error("Mapper Sapa 2 Service: Order {$Order}, file {$File} has no worksheets.", order.Order, file);
                    continue;
                }

                foreach (A2PWorksheet worksheet in file.Worksheets)
                {
                    try
                    {
                        //validate the excel workbook is not null
                        if (worksheet.RowCount == 0)

                        {
                            _logService.Error("Mapper Sapa 2 Service: Order {$Order}, file {$File}, worksheet has no rows.", order.Order, file, worksheet);
                            continue;
                        }
                        try
                        {


                            string lastItem = string.Empty;
                            int sortOrder = 0;
                            await Task.Run(() =>
                            {

                                for (int i = 4; i < worksheet.RowCount + 4; i++)
                                {

                                    line = i + 1;
                                    try
                                    {
                                        if (string.IsNullOrEmpty(worksheet.WorksheetData[i][1].ToString()))
                                        {
                                            _logService.Warning("Mapper Sapa 2 Service: Reference (Sapa article) field empty. Line will be skipped. Order: {$Order}, Worksheet: {$Worksheet}, LineNumber: {$Line}.", order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line);
                                        }

                                        MaterialDTO material = new()
                                        {
                                            Worksheet = worksheet.Worksheet ?? string.Empty,
                                            Order = order.Order ?? string.Empty,
                                            Line = line,
                                            Column = column,
                                            //===================================================
                                            Quantity = 0,
                                            RequiredQuantity = 0,
                                            Reference = string.Empty,
                                            //===================================================
                                            WorksheetType = worksheet.Type,
                                            MaterialType = MaterialType.Unknown
                                        };


                                        if (worksheet.Worksheet is "ND_Glasses")
                                        {

                                            material.Item = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;
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
                                            material.Description = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                            //===================================================================================================
                                            material.Color = string.Empty; // not used in glasses
                                            material.ColorDescription = null; // not used in glasses
                                                                              //===================================================================================================
                                            material.Width = double.TryParse(worksheet.WorksheetData[i][4].ToString(), out double width) ? width : 0;
                                            material.Height = double.TryParse(worksheet.WorksheetData[i][5].ToString(), out double height) ? height : 0;
                                            //===================================================================================================
                                            material.Quantity = worksheet.WorksheetData[i][3] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
                                            material.PackageQuantity = 0;
                                            material.TotalQuantity = material.Quantity;
                                            material.RequiredQuantity = material.TotalQuantity;
                                            material.LeftOverQuantity = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                                          //===================================================================================================
                                            material.Weight = double.TryParse(worksheet.WorksheetData[i][8].ToString(), out double weight) ? weight : 0;
                                            material.TotalWeight = double.TryParse(worksheet.WorksheetData[i][9].ToString(), out double totalWeight) ? totalWeight : 0;
                                            material.RequiredWeight = material.TotalWeight;
                                            material.LeftOverWeight = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                                        //===================================================================================================
                                            material.Area = double.TryParse(worksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                            material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                                            material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                                            material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 
                                                                      //===================================================================================================
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
                                            material.SourceDescription = worksheet.WorksheetData[i][2]?.ToString();
                                            material.SourceColor = null;
                                            material.SourceColorDescription = null;


                                        }
                                        if (worksheet.Worksheet is "ND_Panels")
                                        {

                                            material.Item = worksheet.WorksheetData[i][1].ToString() ?? string.Empty;
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
                                            material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                            // Pattern to match "XPS <number>mm"
                                            string pattern1 = @"(XPS\s+\d+mm)";
                                            Match match = Regex.Match(material.Description, pattern1);
                                            material.Reference = match.Success ? $"LOB_XPS{match.Groups[1].Value}" : string.Empty;
                                            //===================================================================================================
                                            material.Reference = string.IsNullOrEmpty(material.Reference) && material.Description == "1mm aluminium sheet"
                                                ? GetSapa_V2Code("AluSheet1", worksheet.WorksheetData[i][2].ToString() ?? string.Empty, order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line)
                                                : GetSapa_V2Code(worksheet.WorksheetData[i][1].ToString() ?? string.Empty, worksheet.WorksheetData[i][2].ToString() ?? string.Empty, order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line);






                                            //===================================================================================================
                                            material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                            material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;  // not used in glasses
                                                                                                                                   //===================================================================================================
                                            material.Width = double.TryParse(worksheet.WorksheetData[i][6].ToString(), out double width) ? width : 0;
                                            material.Height = double.TryParse(worksheet.WorksheetData[i][7].ToString(), out double height) ? height : 0;
                                            //===================================================================================================
                                            material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 1;
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
                                            material.Area = double.TryParse(worksheet.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                            material.TotalArea = Math.Round(material.Area * material.Quantity, 6);
                                            material.RequiredArea = material.TotalArea; // not used. Threated as unique piece material.
                                            material.LeftOverArea = 0;// not used. Threated as unique piece material, that has no leftovers 
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
                                            material.SourceDescription = worksheet.WorksheetData[i][4]?.ToString();
                                            material.SourceColor = worksheet.WorksheetData[i][2]?.ToString();
                                            material.SourceColorDescription = worksheet.WorksheetData[i][3] == null ? null : worksheet.WorksheetData[i][2].ToString();

                                        }
                                        if (worksheet.Worksheet is "ND_Profiles")
                                        {
                                            material.Item = null; // not used in profiles
                                            material.SortOrder = -1; // not used in profiles
                                                                     //===================================================================================================
                                            material.Reference = GetSapa_V2Code(worksheet.WorksheetData[i][1].ToString() ?? string.Empty, worksheet.WorksheetData[i][2].ToString() ?? string.Empty, order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line);
                                            material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                            //===================================================================================================
                                            material.Color = worksheet.WorksheetData[i][2].ToString() ?? string.Empty;
                                            material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                            //===================================================================================================
                                            material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                            material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(worksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                            material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                            material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                            material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);
                                            //===================================================================================================
                                            material.Width = material.PackageQuantity * 1000; //used as bar length in mm
                                            material.Height = 0;
                                            //===================================================================================================
                                            material.TotalWeight = worksheet.WorksheetData[i][11] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][11].ToString(), out double totalWeight) ? totalWeight : 0;
                                            material.Weight = material.TotalQuantity == 0 ? 0 : Math.Round(material.TotalWeight / material.TotalQuantity, 6);
                                            material.RequiredWeight = Math.Round(material.Weight * material.RequiredQuantity, 6);
                                            material.LeftOverWeight = Math.Round(material.TotalWeight - material.RequiredWeight, 6) < 0 ? 0 : Math.Round(material.TotalWeight - material.RequiredWeight, 6);
                                            //===================================================================================================
                                            material.TotalArea = worksheet.WorksheetData[i][10] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][10].ToString(), out double totalArea) ? totalArea : 0;
                                            material.Area = material.TotalQuantity == 0 ? 0 : Math.Round(material.TotalArea / material.TotalQuantity, 6);
                                            material.RequiredArea = Math.Round(material.Area * material.RequiredQuantity, 6);
                                            material.LeftOverArea = Math.Round(material.TotalArea - material.RequiredArea, 6) < 0 ? 0 : Math.Round(material.TotalArea - material.RequiredArea, 6);
                                            //===================================================================================================
                                            material.Waste = material.RequiredWeight != 0
                                                ? worksheet.WorksheetData[i][9] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][9].ToString(), out double lostWeight) ? lostWeight : 0 / material.RequiredWeight * 100
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
                                            material.CustomField1 = null; // not used in glasses
                                            material.CustomField2 = null; // not used in glasses
                                            material.CustomField3 = null; // not used in glasses
                                                                          //===================================================================================================
                                            material.CustomField4 = null; // not used in glasses
                                            material.CustomField5 = null; // not used in glasses
                                                                          //===================================================================================================
                                            material.MaterialType = MaterialType.Profiles;
                                            //===================================================================================================
                                            material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                                            material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                                            material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                                            material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                                        }
                                        if (worksheet.Worksheet is "ND_Other")
                                        {
                                            material.Item = null; // not used in others
                                            material.SortOrder = -1; // not used in others
                                                                     //===================================================================================================
                                            material.Reference = $"ASA_{worksheet.WorksheetData[i][1].ToString() ?? string.Empty}";
                                            material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                            //===================================================================================================                                                         
                                            material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                            if (string.IsNullOrEmpty(material.Color) && string.IsNullOrEmpty(material.ColorDescription)
                                            | material.ColorDescription.Contains("Without finish"))
                                            {
                                                material.Color = "Without";
                                            }
                                            //===================================================================================================
                                            material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                            material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(worksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                            material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                            material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
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
                                            material.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                            material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
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
                                            material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                                            material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                                            material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                                            material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                                        }
                                        if (worksheet.Worksheet is "ND_Accessories")
                                        {
                                            material.Item = null; // not used in accessories
                                            material.SortOrder = -1; // not used in accessories
                                                                     //===================================================================================================                                                         
                                            material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                            if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                            {
                                                material.Color = "Without";
                                            }
                                            material.Reference = GetSapa_V2Code(worksheet.WorksheetData[i][1].ToString() ?? string.Empty, material.Color ?? string.Empty, order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line);
                                            material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                            //===================================================================================================
                                            material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                            material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 1 : double.TryParse(worksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 1;
                                            material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                            material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
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
                                            material.Price = decimal.TryParse(worksheet.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                            material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
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
                                            material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                                            material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                                            material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                                            material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                                        }
                                        if (worksheet.Worksheet is "ND_Gaskets")
                                        {



                                            material.Item = null; // not used in accessories
                                            material.SortOrder = -1; // not used in accessories
                                                                     //===================================================================================================                                                         
                                            material.ColorDescription = worksheet.WorksheetData[i][3].ToString() ?? string.Empty;
                                            if (string.IsNullOrEmpty(material.Color) && (string.IsNullOrEmpty(material.ColorDescription) || material.ColorDescription.Contains("Without finish")))
                                            {
                                                material.Color = "Without";
                                            }
                                            material.Reference = GetSapa_V2Code(worksheet.WorksheetData[i][1].ToString() ?? string.Empty, material.Color ?? string.Empty, order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line);
                                            material.Description = worksheet.WorksheetData[i][4].ToString() ?? string.Empty;
                                            //===================================================================================================
                                            material.Quantity = worksheet.WorksheetData[i][5] == null ? 1 : int.TryParse(worksheet.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1;
                                            material.PackageQuantity = worksheet.WorksheetData[i][6] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][6].ToString(), out double packageQuantity) ? packageQuantity : 0;
                                            material.TotalQuantity = worksheet.WorksheetData[i][7] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][7].ToString(), out double totalQuantity) ? totalQuantity : 0;
                                            material.RequiredQuantity = worksheet.WorksheetData[i][8] == null ? 0 : double.TryParse(worksheet.WorksheetData[i][8].ToString(), out double requiredQuantity) ? requiredQuantity : 0;
                                            material.LeftOverQuantity = Math.Round(material.TotalQuantity - material.RequiredQuantity, 6) < 0 ? 0 : Math.Round(material.TotalQuantity - material.RequiredQuantity, 6);
                                            //===================================================================================================
                                            if (!string.IsNullOrEmpty(worksheet.WorksheetData[i][9]?.ToString()))
                                            {
                                                if (worksheet.WorksheetData[i][9]?.ToString()?.Contains('/') == true)
                                                {
                                                    string[] split = worksheet.WorksheetData[i][9]?.ToString()?.Split('/') ?? Array.Empty<string>();
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
                                            material.Price = decimal.TryParse(worksheet.WorksheetData[i][10].ToString(), out decimal price) ? price : 0;
                                            material.TotalPrice = decimal.TryParse(worksheet.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
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
                                            material.SourceReference = worksheet.WorksheetData[i][1]?.ToString();
                                            material.SourceColor = worksheet.WorksheetData[i][2].ToString() == null ? null : worksheet.WorksheetData[i][2].ToString();
                                            material.SourceColorDescription = worksheet.WorksheetData[i][3].ToString() == null ? null : worksheet.WorksheetData[i][3].ToString();
                                            material.SourceDescription = worksheet.WorksheetData[i][4].ToString() == null ? null : worksheet.WorksheetData[i][4].ToString();
                                        }

                                        _logService.Verbose("Mapper Sapa 2 Service: Map Materials | Order : {$Order} " +
                                                                                "| Worksheet {Worksheet$} " +
                                                                                "| Line: {$Line} " +
                                                                                "| Sort order: " +
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

                                        order.Materials.Add(material);
                                    }

                                    catch (Exception ex)
                                    {
                                        _logService.Error("Mapper Sapa 2 Service: Unhandled Error. Map single material from material list.  Last known success action. Order: {$Order}, worksheet: {$Worksheet}, line {$Line}. Exception  ${Exception} ", order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line, ex.Message);
                                        continue;
                                    }
                                }
                            });

                            return order;
                        }
                        catch (Exception ex)
                        {
                            _logService.Error("Mapper Sapa 2 Service: Unhandled Error. Mapping material map materials. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line, ex.Message);
                            return order;
                        }

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Mapper Sapa 2 Service: Unhandled Error. Mapping material map materials. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", order.Order ?? string.Empty, worksheet.Worksheet ?? string.Empty, line, ex.Message);

                    }
                }

            }
            return order;
        }


        private string GetSapa_V2Code(string sapaArticle_v2, string sapaColor_v2, string order, string worksheetName, int line)
        {
            try
            {


                if (string.IsNullOrEmpty(sapaArticle_v2) && string.IsNullOrEmpty(sapaColor_v2))
                {
                    _logService.Error("Mapper Sapa 2 Service: Mapping material Article and Color fields are empty. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", order ?? string.Empty, worksheetName ?? string.Empty, line);
                }

                if (string.IsNullOrEmpty(sapaColor_v2) && (worksheetName == "ND_Gaskets" || worksheetName == "ND_Accessories"))
                {
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];

                    }

                    return sapaArticle_v2;
                }


                if (worksheetName is "ND_Profiles" or "ND_Accessories")
                {

                    // Remove 'S' if it is the first character of field1
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];

                    }
                    // Remove 'S' if it is the first character of field1
                    if ((sapaColor_v2.EndsWith("F") || sapaColor_v2.EndsWith("R") || sapaColor_v2.EndsWith("M")) && sapaColor_v2 != "MF")
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

                    string newReference = $"*{reference[..24]}";
                    _logService.Error("Mapper Sapa 2 Service: Warning. Reference > 25 character. Order {$Order}, " +
                                                                                                  "worksheet {$Worksheet}, " +
                                                                                                  "line {$Line}, " +
                                                                                                  "Sapa Article {$SapaArticle}({$SapaArticleLength}), " +
                                                                                                  "Sapa Color {$SapaColor}({$SapaColorLength}), " +
                                                                                                  "generated reference {$Reference}({$ReferenceLength}), " +
                                                                                                  "Truncated reference {$NewReference}({$NewReferenceLength}).",

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
                                                                                                  newReference.Length);

                    return newReference!;
                }



                return reference;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "Mapper Sapa 2 Service: Error Order {$Order}, worksheet {$Worksheet}, line {$Line}. Can't generate PrefSuite reference using Sapa v.2 article: {$Article} and Sapa v.2 Color {$Color}.", order ?? string.Empty, sapaArticle_v2, sapaColor_v2);
                return "Unknown";
            }
        }




    }
}