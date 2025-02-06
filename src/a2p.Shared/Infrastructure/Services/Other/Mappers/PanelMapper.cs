using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;

namespace a2p.Shared.Infrastructure.Services.Other.Mappers
{
    public class PanelMapper : IPanelMapper
    {
        private readonly ILogService _logService;


        public PanelMapper(ILogService logService)
        {
            _logService = logService;
        }
        public async Task<List<PanelDTO>> GetSapa_v1Async(A2POrderFileWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = wr.WorksheetName;
            string order = wr.OrderNumber;
            try
            {
                if (wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MPDTO Sapa v.1. Worksheet is empty. OrderNumber: {$OrderNumber}", order);

                    return [];
                }
                worksheetName = wr.WorksheetName;
                order = wr.OrderNumber;

                List<PanelDTO> panels = [];
                await Task.Run(() =>
                {

                    for (int i = 0; i < wr.WorkSheetRowCount; i++)
                    {
                        try
                        {
                            lineNumber = i + 1;
                            PanelDTO panel = new()
                            {
                                Type = WorksheetType.Panels_Sapa_v1
                            };
                            panels.Add(panel);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(ex.Message, "MPDTO Sapa v.1. For each Panel. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                            continue;

                        }
                    }
                });

                return panels;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MPDTO Sapa v.1. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }
        public async Task<List<PanelDTO>> GetSapa_v2Async(A2POrderFileWorksheet wr)
        {

            int lineNumber = 0;
            string worksheetName = wr.WorksheetName;
            string order = wr.OrderNumber;
            try
            {
                if (wr.WorksheetData.Count == 0)
                {
                    _logService.Error("MPDTO Sapa v.2. Worksheet is empty. OrderNumber: {$OrderNumber}", new { order });
                    return [];
                }
                worksheetName = wr.WorksheetName;
                order = wr.OrderNumber;

                List<PanelDTO> panelsResult = [];


                return panelsResult = await Task.Run(() =>
                 {


                     List<PanelDTO> panels = [];
                     for (int i = 4; i < wr.WorkSheetRowCount; i++)
                     {
                         try
                         {
                             lineNumber = i + 1;

                             PanelDTO panel = new()
                             {
                                 WorksheetName = wr.WorksheetName,
                                 Order = wr.OrderNumber,
                                 Item = wr.WorksheetData[i][1]?.ToString() ?? "",
                                 SortOrder = i - 3,
                                 Reference = wr.WorksheetData[i][3]?.ToString() ?? "",
                                 Color = wr.WorksheetData[i][2]?.ToString() ?? string.Empty,
                                 Description = wr.WorksheetData[i][4]?.ToString() ?? string.Empty,
                                 Quantity = int.TryParse(wr.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 0,
                                 Width = double.TryParse(wr.WorksheetData[i][6].ToString(), out double width) ? width : 0,
                                 Height = double.TryParse(wr.WorksheetData[i][7].ToString(), out double height) ? height : 0,
                                 Area = double.TryParse(wr.WorksheetData[i][10].ToString(), out double area) ? area : 0
                             };
                             panel.TotalArea = panel.Area * panel.Quantity;
                             panel.Price = decimal.TryParse(wr.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                             panel.SquareMeterPrice = decimal.TryParse(wr.WorksheetData[i][8].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
                             panel.TotalPrice = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                             panel.Type = WorksheetType.Panels_Sapa_v2;
                             _logService.Debug("MPDTO Sapa v.2. PANEL: | FileName: {$Worksheet} | LineNumber {$Line} | OrderNumber: {$OrderNumber} | Item: {$Item} | SortOrder: {$SortOrder} | Reference: {$Reference} | Description: {$Description} | Quantity: {$Quantity} | Width: {$Width} | Height: {$Height} | Area: {$Area} | TotalArea: {$TotalArea} | Price: {$Price} | SquareMeterPrice: {$SquareMeterPrice} | TotalPrice: {$TotalPrice} |",

                      panel.WorksheetName,
                      lineNumber,
                      panel.Order,
                      panel.Item,
                      panel.SortOrder,
                      panel.Reference,
                      panel.Description,
                      panel.Quantity,
                      panel.Width,
                      panel.Height,
                      panel.Area,
                      panel.TotalArea,
                      panel.Price,
                      panel.SquareMeterPrice,
                      panel.TotalPrice);

                             panels.Add(panel);
                         }
                         catch (Exception ex)
                         {
                             _logService.Error("MPDTO Sapa v.2. For each Panel. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$Line},${Exception}", order, worksheetName, lineNumber, ex.Message);
                             continue;

                         }
                     }

                     return panels;

                 });
            }



            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MPDTO Sapa v.2. For each Panel. Unhandled Error. Last known success action. OrderNumber: {$OrderNumber}, Worksheet: {$FileName}, LineNumber: {$LineNumber, ${Exception}", order, worksheetName, lineNumber, ex.Message);
                return [];
            }
        }

    }
}