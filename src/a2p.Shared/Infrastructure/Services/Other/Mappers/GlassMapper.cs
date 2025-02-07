using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;

namespace a2p.Shared.Infrastructure.Services.Other.Mappers
{
    public class GlassMapper : IGlassMapper
    {
        private readonly ILogService _logService;

        public GlassMapper(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<List<GlassDTO>> GetSapa_v1Async(A2PWorksheet wr)
        {

            if (wr == null)
            {
                _logService.Error("MGDTO: Sapa v.1. AppWorksheet is empty");
                return [];
            }

            if (string.IsNullOrEmpty(wr.Worksheet))
            {
                _logService.Error("MGDTO: Sapa v.1. AppWorksheet is empty. Order: {$Order}", wr.OrderNumber);
                return [];
            }
            int lineNumber = 0;
            string worksheetName = wr.Worksheet ?? "Unknown";
            string order = wr.OrderNumber ?? "Unknown";
            try
            {
                if (wr.WorksheetData == null || wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MGDTO: Sapa v.1. AppWorksheet is empty. Order: {$Order}", order);
                    return [];
                }

                List<GlassDTO> glasses = [];
                for (int i = 0; i < wr.RowCount; i++)
                {
                    try
                    {
                        lineNumber = i + 1;
                        GlassDTO glass = new()
                        {
                            Type = WorksheetType.Glasses_Sapa_v1
                        };

                        glasses.Add(glass);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex.Message, "MGDTO: Sapa v.1. For each. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                        continue;
                    }
                }
                return await Task.FromResult(glasses);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MGDTO: Sapa v.1. Last known success action. Order: {$Order}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }

        public async Task<List<GlassDTO>> GetSapa_v2Async(A2PWorksheet wr)
        {
            if (wr == null)
            {
                _logService.Error("MGDTO: Sapa v.2. AppWorksheet is null");
                return [];
            }

            int lineNumber = 0;
            string worksheetName = wr.Worksheet ?? "Unknown";
            string order = wr.OrderNumber ?? "Unknown";

            try
            {
                if (wr.WorksheetData == null || wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MGDTO: Sapa v.2. AppWorksheet is empty. Order: {$Order}", order);
                    return [];
                }

                List<GlassDTO> glasses = [];
                for (int i = 4; i < wr.RowCount - 1; i++)
                {
                    try
                    {
                        lineNumber = i + 1;
                        GlassDTO glass = new()
                        {
                            Worksheet = wr.Worksheet ?? string.Empty,
                            Order = wr.OrderNumber ?? string.Empty,
                            Item = wr.WorksheetData[i][1].ToString() ?? string.Empty,
                            SortOrder = i - 3,
                            Reference = string.Empty,
                            SourceReference = string.Empty,
                            SourceDescription = string.Empty,
                            Description = wr.WorksheetData[i][2].ToString() ?? string.Empty,
                            Quantity = int.TryParse(wr.WorksheetData[i][3].ToString(), out int quantity) ? quantity : 0,
                            Width = double.TryParse(wr.WorksheetData[i][4].ToString(), out double width) ? width : 0,
                            Height = double.TryParse(wr.WorksheetData[i][5].ToString(), out double height) ? height : 0,
                            Weight = double.TryParse(wr.WorksheetData[i][8].ToString(), out double weight) ? weight : 0,
                            TotalWeight = double.TryParse(wr.WorksheetData[i][9].ToString(), out double totalWeight) ? totalWeight : 0,
                            Area = double.TryParse(wr.WorksheetData[i][10].ToString(), out double area) ? area : 0,
                        };

                        glass.TotalArea = glass.Area * glass.Quantity;
                        glass.AreaUsed = glass.Area * glass.Quantity;
                        glass.Ordered = glass.Area * glass.Quantity;
                        glass.Waste = glass.Ordered - glass.AreaUsed;
                        glass.Price = decimal.TryParse(wr.WorksheetData[i][7].ToString(), out decimal price) ? price : 0;
                        glass.SquareMeterPrice = decimal.TryParse(wr.WorksheetData[i][6].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
                        glass.TotalPrice = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        try
                        {
                            glass.Pallet = wr.WorksheetData[i][12].ToString() ?? string.Empty;
                        }
                        catch
                        {
                            _logService.Error("MGDTO: Sapa v.2. Order: {$Order}, Worksheet: {$Worksheet}, Line {$Line} pallet column is missing!", new { order, worksheetName, lineNumber });
                            glass.Pallet = string.Empty;
                        }



                        _logService.Debug("GLASS: | Order: {$Order}" +
                                                 "| Worksheet: {$Worksheet} " +
                                                 "| Line: {$Line} " +
                                                 "| Item: {$Item} " +
                                                 "| SortOrder: {$SortOrder} " +
                                                 "| Reference: {$Reference} " +
                                                 "| Description: {$Description} " +
                                                 "| Quantity: {$Quantity} " +
                                                 "| Width: {$Width} " +
                                                 "| Height: {$Height} " +
                                                 "| Weight: {$Weight} " +
                                                 "| Area: {$Area} " +
                                                 "| Price: {$Price} " +
                                                 "| TotalPrice: {$TotalPrice} " +
                                                 "| SquareMeterPrice: {$SquareMeterPrice} " +
                                                 "| TotalWeight: {$TotalWeight} " +
                                                 "| TotalArea: {$TotalArea} " +
                                                 "| AreaUsed: {$AreaUsed} " +
                                                 "| Ordered: {$Ordered} ",
                                                 "| Waste: {$Waste} ",
                                                 "| Pallet: {$Pallet}",
                                                 "| SourceReference: {$SourceReference} " +
                                                 "| SourceDescription: {$SourceDescription} ",
                                                 "| Type: {$Type}",

                                                    order,
                                                    worksheetName,
                                                    lineNumber,
                                                    glass.Item,
                                                    glass.SortOrder,
                                                    glass.Reference,
                                                    glass.Description,
                                                    glass.Quantity,
                                                    glass.Width,
                                                    glass.Height,
                                                    glass.Weight,
                                                    glass.Area,
                                                    glass.Price,
                                                    glass.TotalPrice,
                                                    glass.SquareMeterPrice,
                                                    glass.TotalWeight,
                                                    glass.TotalArea,
                                                    glass.AreaUsed,
                                                    glass.Ordered,
                                                    glass.Waste,
                                                    glass.Pallet,
                                                    glass.SourceReference,
                                                    glass.SourceDescription,
                                                    glass.Type.ToString());

                        glasses.Add(glass);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex.Message, "MGDTO: Sapa v.2. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                    }
                }

                return await Task.FromResult(glasses);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MGDTO: Sapa v.2. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }

        public async Task<List<GlassDTO>> GetSchucoAsync(A2PWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = wr.Worksheet;
            string order = wr.OrderNumber;
            try
            {
                if (wr == null)

                {
                    _logService.Error("MGDTO: Schuco. Worksheet is empty. Order: {$Order}", order);
                    return await Task.Run(() => new List<GlassDTO>());
                }
                if (wr.RowCount == 0)
                {
                    _logService.Error("MGDTO: Schuco. AppWorksheet is empty. Order: {$Order}", order);
                    return await Task.Run(() => new List<GlassDTO>());
                }

                List<GlassDTO> glasses = [];
                for (int i = 0; i < wr.RowCount; i++)
                {
                    try
                    {
                        lineNumber = i + 1;
                        GlassDTO glass = new()
                        {
                            Type = WorksheetType.Glasses_Schuco
                        };
                        glasses.Add(glass);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex.Message, "MGDTO: Schuco. For each. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                        continue;
                    }

                }

                return await Task.Run(() => glasses);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MGDTO: Schuco. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return await Task.Run(() => new List<GlassDTO>());
            }
        }
    }
}