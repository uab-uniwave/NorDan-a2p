using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;


namespace a2p.Shared.Infrastructure.Services.Other.Mappers
{
    public class ItemMapper : IItemMapper
    {

        private readonly ILogService _logService;
        public ItemMapper(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<List<ItemDTO>> GetSapa_v1Async(A2POrderFileWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = "Unknown";
            string order = "Unknown";
            try
            {

                if (wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MIDTO Sapa v.1. AppWorksheet is empty. Order: {$Order}", order);
                    return [];
                }
                List<ItemDTO> items = [];


                return items = await Task.Run(() =>
                {
                    List<ItemDTO> items = [];
                    for (int i = 1; i < wr.RowCount - 1; i++)
                    {

                        try
                        {
                            lineNumber = i + 1;
                            ItemDTO item = new()
                            {

                                WorksheetName = wr.Worksheet,
                                WorksheetType = WorksheetType.Items_Sapa_v1
                            };
                            items.Add(item);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(ex.Message, "MIDTO Sapa v.1. For Each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                            continue;
                        }
                    }

                    return items;
                }
                );
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MIDTO Sapa v.1. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }


        }

        public async Task<List<ItemDTO>> GetSapa_v2Async(A2POrderFileWorksheet wr)
        {

            int lineNumber = 0;
            string worksheetName = string.Empty;
            string order = string.Empty;
            try
            {
                if (wr.WorksheetData.Count == 0)
                {
                    order = wr.OrderNumber;
                    _logService.Error("MIDTO Sapa v.2. AppWorksheet is empty. Order: {$Order}", order);
                    return [];
                }


                List<ItemDTO> items = [];
                for (int i = 1; i < wr.RowCount - 1; i++)
                {
                    try
                    {
                        lineNumber = i;
                        decimal vProfiles = decimal.TryParse(wr.WorksheetData[i][8].ToString(), out decimal profiles) ? profiles : throw new Exception($"Profiles is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][8]}");
                        decimal vFittings = decimal.TryParse(wr.WorksheetData[i][9].ToString(), out decimal fittings) ? fittings : throw new Exception($"Fittings is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][9]}");
                        decimal vGasketsAccessories = decimal.TryParse(wr.WorksheetData[i][10].ToString(), out decimal gasketsAccessories) ? gasketsAccessories : throw new Exception($"GasketsAccessories is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][10]}");
                        decimal vAluminumSheets = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal aluminumSheets) ? aluminumSheets : throw new Exception($"AluminumSheets is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][11]}");
                        decimal vSurchargesAluminum = decimal.TryParse(wr.WorksheetData[i][12].ToString(), out decimal surchargesAluminum) ? surchargesAluminum : throw new Exception($"SurchargesAluminum is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][12]}");
                        decimal vSurfaceTreatment = decimal.TryParse(wr.WorksheetData[i][13].ToString(), out decimal surfaceTreatment) ? surfaceTreatment : throw new Exception($"SurfaceTreatment is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][13]}");
                        decimal vClient = decimal.TryParse(wr.WorksheetData[i][14].ToString(), out decimal client) ? client : throw new Exception($"Client is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][14]}");
                        decimal vGlass = decimal.TryParse(wr.WorksheetData[i][15].ToString(), out decimal glass) ? glass : throw new Exception($"Glass is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][15]}");
                        decimal vPanels = decimal.TryParse(wr.WorksheetData[i][16].ToString(), out decimal panel) ? panel : throw new Exception($"Panels is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][16]}");
                        decimal vWages = decimal.TryParse(wr.WorksheetData[i][17].ToString(), out decimal wages) ? wages : throw new Exception($"Wages is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][17]}");
                        decimal vSpecialCosts = decimal.TryParse(wr.WorksheetData[i][19].ToString(), out decimal specialCosts) ? specialCosts : throw new Exception($"SpecialCosts is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][19]}");
                        decimal vMaterialCosts = vProfiles + vFittings + vGasketsAccessories + vAluminumSheets + vSurchargesAluminum + vSurfaceTreatment + vClient + vGlass + vPanels + vSpecialCosts;
                        decimal vCost = vMaterialCosts + vWages;


                        ItemDTO item = new()
                        {
                            WorksheetName = wr.Worksheet,
                            Order = wr.OrderNumber,

                            Description = wr.WorksheetData[i][0]?.ToString() ?? string.Empty,
                            SortOrder = int.TryParse(wr.WorksheetData[i][1].ToString(), out int sortOrder) ? sortOrder : throw new Exception($"SortOrder is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][1]}"),
                            Item = wr.WorksheetData[i][2]?.ToString() ?? string.Empty,
                            Width = double.TryParse(wr.WorksheetData[i][3].ToString(), out double width) ? width : throw new Exception($"Width is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][3]}"),
                            Height = double.TryParse(wr.WorksheetData[i][4].ToString(), out double height) ? height : throw new Exception($"Height is not a number LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][4]}"),
                            Quantity = int.TryParse(wr.WorksheetData[i][5].ToString(), out int quantity) ? quantity : throw new Exception($"Quantity is not a number LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][5]}"),
                            Weight = double.TryParse(wr.WorksheetData[i][6].ToString(), out double weight) ? weight : throw new Exception($"Weight is not a number LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][6]}"),
                            WeightGlass = double.TryParse(wr.WorksheetData[i][7].ToString(), out double weightGlass) ? weightGlass : throw new Exception($"WeightGlass is not a number LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][7]}")
                        };
                        item.WeightWithoutGlass = item.Weight - item.WeightGlass;
                        item.WeightTotal = item.Weight * item.Quantity;
                        item.Area = width * height / 1000000;
                        item.TotalArea = item.Area * item.Quantity;
                        item.Hours = double.TryParse(wr.WorksheetData[i][18].ToString(), out double hours) ? hours : throw new Exception($"Hours is not a number LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][18]}");
                        item.LaborCost = vWages;
                        item.MaterialCost = vMaterialCosts;
                        item.Cost = vCost;
                        item.Price = decimal.TryParse(wr.WorksheetData[i][21].ToString(), out decimal price) ? price : throw new Exception($"Price is not a number LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][21]}");
                        item.TotalPrice = item.Quantity * item.Price;
                        item.WorksheetType = WorksheetType.Items_Sapa_v2;
                        //item.CurrencyCode = wr.Currency;

                        _logService.Debug("MIDTO Sapa v.2. ITEM: | FileName: {$Worksheet} | Line Order: {$Line} | Order: {$Order} | Description: {$Description} | SortOrder: {$SortOrder} | Item: {$Item} | Width: {$Width} | Height: {$Height} | Quantity: {$Quantity} | Weight: {$Weight} | WeightGlass: {$WeightGlass} | WeightWithoutGlass: {$WeightWithoutGlass} | WeightTotal: {$WeightTotal} | Area: {$Area} | TotalArea: {$TotalArea} | Hours: {$Hours} | Price: {$Price} | TotalPrice: {$TotalPrice} | LaborCost: {$LaborCost} | MaterialCost: {$MaterialCost} | Cost: {$Cost} | Type: {$Type} | Profiles: {$Profiles} | Fittings: {$Fittings} | GasketsAccessories: {$GasketsAccessories} | AluminumSheets: {$AluminumSheets} | SurchargesAluminum: {$SurchargesAluminum} | SurfaceTreatment: {$SurfaceTreatment} | Client: {$Client} | Glass: {$Glass} | Panels: {$Panels} | Wages: {$Wages} | SpecialCosts: {$SpecialCosts} |",
                        item.WorksheetName ?? "Unknown",
                        lineNumber,
                        item.Order ?? "Unknown",
                        item.Description ?? "Unknown",
                        item.SortOrder,
                        item.Item ?? "Unknown",
                        item.Width,
                        item.Height,
                        item.Quantity,
                        item.Weight,
                        item.WeightGlass,
                        item.WeightWithoutGlass,
                        item.WeightTotal,
                        item.Area,
                        item.TotalArea,
                        item.Hours,
                        item.Price,
                        item.TotalPrice,
                        item.LaborCost,
                        item.MaterialCost,
                        item.Cost,
                        item.WorksheetType.ToString(),
                        vProfiles,
                        vFittings,
                        vGasketsAccessories,
                        vAluminumSheets,
                        vSurchargesAluminum,
                        vSurfaceTreatment,
                        vClient,
                        vGlass,
                        vPanels,
                        vWages,
                        vSpecialCosts);

                        items.Add(item);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex.Message, "MIDTO Sapa v.2. For Each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                        continue;
                    }
                }
                return await Task.FromResult(items);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MIDTO Sapa v.2 . Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }
        public async Task<List<ItemDTO>> GetSchucoAsync(A2POrderFileWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = string.Empty;
            string order = string.Empty;
            try
            {
                if (wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MIDTO Schuco. AppWorksheet is empty. Order: {$Order}", order);
                    return [];
                }
                List<ItemDTO> items = [];

                return items = await Task.Run(() =>
                {
                    List<ItemDTO> items = [];


                    for (int i = 0; i < wr.RowCount; i++)
                    {
                        try
                        {
                            lineNumber = i + 1;
                            ItemDTO item = new();
                            {
                                item.WorksheetType = WorksheetType.Items_Schuco;
                                item.Order = wr.OrderNumber;
                                item.WorksheetName = wr.Worksheet;
                            }
             ;
                            items.Add(item);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(ex.Message, "MIDTO Schuco. For Each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                            continue;
                        }
                    }
                    return items;
                });

            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MIDTO Schuco. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }

        }

    }
}