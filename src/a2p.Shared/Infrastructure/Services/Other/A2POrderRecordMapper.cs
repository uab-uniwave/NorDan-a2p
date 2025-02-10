using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;

namespace a2p.Shared.Infrastructure.Services.Other
{
    public class A2POrderRecordMapper : IA2POrderMapper
    {
        public async Task<OrderDTO> MapToOrderDTOAsync(A2POrder order)
        {
            return await Task.Run(() =>
            {
                OrderDTO orderDTO = new()
                {
                    Order = order.Order,
                    Currency = order.Files
                        .SelectMany(file => file.Worksheets)
                        .FirstOrDefault(worksheet => !string.IsNullOrEmpty(worksheet.Currency))?.Currency ?? string.Empty,
                    FileCount = order.Files.Count,
                    FileList = string.Join("\n ", order.Files.Select(file => file.FileName)),
                    LockedFileCount = order.Files.Count(file => file.IsLocked),
                    LockedFileList = string.Join("\n ", order.Files.Where(file => file.IsLocked).Select(file => file.FileName)),
                    WorksheetCount = order.Files.Sum(file => file.Worksheets?.Count ?? 0),
                    WorksheetList = string.Join("\n ", order.Files.SelectMany(file => file.Worksheets).Select(ws => ws.Worksheet)),
                    ItemCount = order.Files.Sum(file => file.Worksheets?.Sum(ws => ws.Items) ?? 0),

                    ErrorCount = order.ReadErrors.Count(error => error.Level == ErrorLevel.Error || error.Level == ErrorLevel.Fatal),
                    ErrorList = string.Join("\n ", order.ReadErrors
                        .Where(error => error.Level == ErrorLevel.Error || error.Level == ErrorLevel.Fatal)
                        .Select(error => error.Description)),
                };

                if (order.ReadErrors.Any(error => error.Level == ErrorLevel.Error || error.Level == ErrorLevel.Fatal))
                {
                    orderDTO.Import = false;
                }

                return orderDTO;
            });
        }

    }
}
