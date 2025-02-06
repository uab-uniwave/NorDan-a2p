using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Interfaces.Services;

namespace a2p.Shared.Infrastructure.Services.Other
{
    public class A2POrderMapper : IA2POrderMapper
    {
        public async Task<OrderDTO> MapToOrderDTOAsync(A2POrder order)
        {


            return await Task.Run(() =>
            {
                OrderDTO orderDTO = new()
                {
                    Order = order.OrderNumber,
                    Currency = order.OrderCurrency,
                    FileCount = order.OrderFiles.Count,
                    FileList = string.Join("\n ", order.OrderFiles.Select(file => file.FileName)),
                    LockedFileCount = order.OrderFiles.Count(file => file.IsLocked),
                    LockedFileList = string.Join("\n ", order.OrderFiles.Where(file => file.IsLocked).Select(file => file.FileName)),
                    WorksheetCount = order.OrderFiles.Sum(file => file.OrderFileWorksheets?.Count ?? 0),
                    WorksheetList = string.Join("\n ", order.OrderFiles.SelectMany(file => file.OrderFileWorksheets).Select(ws => ws.WorksheetName)),
                    ItemCount = order.OrderFiles.Sum(file => file.OrderFileWorksheets?.Sum(ws => ws.WorkSheetRowCount) ?? 0),
                    Import = true, // Assuming all records are importable for no;
                    ErrorCount = 0, // Assuming no errors for no;
                    ErrorList = string.Empty // Assuming no errors for;
                };
                return orderDTO;
            });

        }

    }
}
