using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Interfaces.Mappers;

namespace a2p.Shared.Infrastructure.Mappers
{
 public class OrderMapper : IOrderMapper
 {
  public async Task<OrderDTO> MapToOrderDTOAsync(A2POrder order)
  {


   return await Task.Run(() =>
   {
    OrderDTO orderDTO = new()
    {
     Order=order.Number,
     Currency=order.Currency,
     FileCount=order.Files.Count,
     FileList=string.Join("\n ", order.Files.Select(file => file.Name)),
     LockedFileCount=order.Files.Count(file => file.IsLocked),
     LockedFileList=string.Join("\n ", order.Files.Where(file => file.IsLocked).Select(file => file.Name)),
     WorksheetCount=order.Files.Sum(file => file.FileWorksheets?.Count??0),
     WorksheetList=string.Join("\n ", order.Files.SelectMany(file => file.FileWorksheets).Select(ws => ws.Name)),
     ItemCount=order.Files.Sum(file => file.FileWorksheets?.Sum(ws => ws.RowCount)??0),
     Import=true, // Assuming all records are importable for no;
     ErrorCount=0, // Assuming no errors for no;
     ErrorList=string.Empty // Assuming no errors for;
    };



    return orderDTO;
   });

  }

 }
}
