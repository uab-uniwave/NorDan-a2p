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

        public async Task<List<GlassDTO>> GetSapa_v1Async(A2POrderFileWorksheet wr)
        {

            if (wr == null)
            {
                _logService.Error("MGDTO: Sapa v.1. AppWorksheet is empty");
                return [];
            }

            if (string.IsNullOrEmpty(wr.WorksheetName))
            {
                _logService.Error("MGDTO: Sapa v.1. AppWorksheet is empty. OrderNumber: {$OrderNumber}", wr.OrderNumber);
                return [];
            }
            int lineNumber = 0;
            string worksheetName = wr.WorksheetName ?? "Unknown";
            string order = wr.OrderNumber ?? "Unknown";
            try
            {
                if (wr.WorksheetData == null || wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MGDTO: Sapa v.1. AppWorksheet is empty. OrderNumber: {$OrderNumber}", order);
                    return [];
                }

                List<GlassDTO> glasses = [];
                for (int i = 0; i < wr.WorkSheetRowCount; i++)
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
                        _logService.Error(ex.Message, "MGDTO: Sapa v.1. For each. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                        continue;
                    }
                }
                return await Task.FromResult(glasses);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MGDTO: Sapa v.1. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }

        public async Task<List<GlassDTO>> GetSapa_v2Async(A2POrderFileWorksheet wr)
        {
            if (wr == null)
            {
                _logService.Error("MGDTO: Sapa v.2. AppWorksheet is null");
                return [];
            }

            int lineNumber = 0;
            string worksheetName = wr.WorksheetName ?? "Unknown";
            string order = wr.OrderNumber ?? "Unknown";

            try
            {
                if (wr.WorksheetData == null || wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MGDTO: Sapa v.2. AppWorksheet is empty. OrderNumber: {$OrderNumber}", order);
                    return [];
                }

                List<GlassDTO> glasses = [];
                for (int i = 4; i < wr.WorkSheetRowCount - 1; i++)
                {
                    try
                    {
                        lineNumber = i + 1;
                        GlassDTO glass = new()
                        {
                            WorksheetName = wr.WorksheetName ?? string.Empty,
                            Order = wr.OrderNumber ?? string.Empty,
                            Item = wr.WorksheetData[i][1].ToString() ?? string.Empty,
                            SortOrder = i - 3,
                            Reference = string.Empty,
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
                        glass.AreaOrdered = glass.Area * glass.Quantity;
                        glass.Price = decimal.TryParse(wr.WorksheetData[i][7].ToString(), out decimal price) ? price : 0;
                        glass.SquareMeterPrice = decimal.TryParse(wr.WorksheetData[i][6].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
                        glass.TotalPrice = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                        try
                        {
                            glass.Pallet = wr.WorksheetData[i][12].ToString() ?? string.Empty;
                        }
                        catch
                        {
                            _logService.Error("MGDTO: Sapa v.2. OrderNumber: {$OrderNumber}, worksheet: {$FileName}, line number {$Line} pallet column is missing!", new { order, worksheetName, lineNumber });
                            glass.Pallet = string.Empty;
                        }



                        _logService.Debug("GLASS: | OrderNumber: {$OrderNumber} | FileName: {$Worksheet} | Line OrderNumber: {$Line} | Item: {$Item} | SortOrder: {$SortOrder} | Reference: {$Reference} | Description: {$Description} | Quantity: {$Quantity} | Width: {$Width} | Height: {$Height} | Weight: {$Weight} | TotalWeight: {$TotalWeight} | Area: {$Area} | TotalArea: {$TotalArea} | Price: {$Price} | SquareMeterPrice: {$SquareMeterPrice} | TotalPrice: {$TotalPrice} | Palett: {$Palett}",
                         worksheetName,
                         lineNumber,
                         order,
                         glass.Item,
                         glass.SortOrder,
                         glass.Reference,
                         glass.Description,
                         glass.Quantity,
                         glass.Width,
                         glass.Height,
                         glass.Weight,
                         glass.TotalWeight,
                         glass.Area,
                         glass.TotalArea,
                         glass.Price,
                         glass.SquareMeterPrice,
                         glass.TotalPrice,
                         glass.Pallet);
                        glass.Type = WorksheetType.Glasses_Sapa_v2;
                        glasses.Add(glass);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex.Message, "MGDTO: Sapa v.2. OrderNumber: {$OrderNumber}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                    }
                }

                return await Task.FromResult(glasses);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MGDTO: Sapa v.2. Last known success action. OrderNumber: {$OrderNumber}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }

        public async Task<List<GlassDTO>> GetSchucoAsync(A2POrderFileWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = wr.WorksheetName;
            string order = wr.OrderNumber;
            try
            {
                if (wr == null)

                {
                    _logService.Error("MGDTO: Schuco. Worksheet is empty. OrderNumber: {$OrderNumber}", order);
                    return await Task.Run(() => new List<GlassDTO>());
                }
                if (wr.WorkSheetRowCount == 0)
                {
                    _logService.Error("MGDTO: Schuco. AppWorksheet is empty. OrderNumber: {$OrderNumber}", order);
                    return await Task.Run(() => new List<GlassDTO>());
                }

                List<GlassDTO> glasses = [];
                for (int i = 0; i < wr.WorkSheetRowCount; i++)
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
                        _logService.Error(ex.Message, "MGDTO: Schuco. For each. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                        continue;
                    }

                }

                return await Task.Run(() => glasses);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MGDTO: Schuco. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return await Task.Run(() => new List<GlassDTO>());
            }
        }
    }
}