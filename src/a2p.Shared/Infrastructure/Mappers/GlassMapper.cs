using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Utils;

namespace a2p.Shared.Infrastructure.Mappers
{
 public class GlassMapper : IGlassMapper
 {
  private readonly ILogService _logger;

  public GlassMapper(ILogService logger)
  {
   _logger=logger;
  }

  public async Task<List<GlassDTO>> MapToGlassDTOAsync(A2PWorksheet wr)
  {
   string worksheetName = string.Empty;
   string order = string.Empty;

   try
   {
    if (wr==null)
    {
     _logger.Error("MGDTO, AppWorksheet is null");
     return [];
    }


    List<GlassDTO> glasses = [];
    worksheetName=wr.Name??"Unknown";

    switch (wr.WorksheetType)
    {
     case WorksheetType.Glasses_Sapa_v1: // Sapa v1
      glasses=await Task.Run(() => GetSapa1Async(wr));
      break;

     case WorksheetType.Glasses_Sapa_v2:
      glasses=await Task.Run(() => GetSapa2Async(wr));
      break;

     case WorksheetType.Glasses_Schuco:
      glasses=await Task.Run(() => GetSchucoAsync(wr));
      break;

     default:
      _logger.Error("MGDTO. Vendor and/or version unknown, Order: {$Order}, Worksheet: {$Name}", order, worksheetName);
      break;
    }
    return glasses;
   }
   catch (Exception ex)
   {
    _logger.Error(ex, "MGDTO. Unhandled error: Last known Order: {$Order}, Worksheet: {$Name}.", order, worksheetName);
    return [];
   }
  }

  private async Task<List<GlassDTO>> GetSapa1Async(A2PWorksheet wr)
  {

   if (wr==null)
   {
    _logger.Error("MGDTO: Sapa v.1. AppWorksheet is empty");
    return [];
   }

   if (string.IsNullOrEmpty(wr.Name))
   {
    _logger.Error("MGDTO: Sapa v.1. AppWorksheet is empty. Order: {$Order}", wr.Order);
    return [];
   }
   int lineNumber = 0;
   string worksheetName = wr.Name??"Unknown";
   string order = wr.Order??"Unknown";
   try
   {
    if (wr.Data==null||wr.Data.Count==0)
    {
     _logger.Error("MGDTO: Sapa v.1. AppWorksheet is empty. Order: {$Order}", order);
     return [];
    }

    List<GlassDTO> glasses = [];
    for (int i = 0; i<wr.RowCount; i++)
    {
     try
     {
      lineNumber=i+1;
      GlassDTO glass = new()
      {
       Type=WorksheetType.Glasses_Sapa_v1
      };

      glasses.Add(glass);
     }
     catch (Exception ex)
     {
      _logger.Error(ex.Message, "MGDTO: Sapa v.1. For each. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line}", order, worksheetName, lineNumber);
      continue;
     }
    }
    return await Task.FromResult(glasses);
   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MGDTO: Sapa v.1. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line}", order, worksheetName, lineNumber);
    return [];
   }
  }

  private async Task<List<GlassDTO>> GetSapa2Async(A2PWorksheet wr)
  {
   if (wr==null)
   {
    _logger.Error("MGDTO: Sapa v.2. AppWorksheet is null");
    return [];
   }

   int lineNumber = 0;
   string worksheetName = wr.Name??"Unknown";
   string order = wr.Order??"Unknown";

   try
   {
    if (wr.Data==null||wr.Data.Count==0)
    {
     _logger.Error("MGDTO: Sapa v.2. AppWorksheet is empty. Order: {$Order}", order);
     return [];
    }

    List<GlassDTO> glasses = [];
    for (int i = 4; i<wr.RowCount-1; i++)
    {
     try
     {
      lineNumber=i+1;
      GlassDTO glass = new()
      {
       WorksheetName=wr.Name??string.Empty,
       Order=wr.Order??string.Empty,
       Item=wr.Data[i][1].ToString()??string.Empty,
       SortOrder=i-3,
       Reference=string.Empty,
       Description=wr.Data[i][2].ToString()??string.Empty,
       Quantity=int.TryParse(wr.Data[i][3].ToString(), out int quantity) ? quantity : 0,
       Width=double.TryParse(wr.Data[i][4].ToString(), out double width) ? width : 0,
       Height=double.TryParse(wr.Data[i][5].ToString(), out double height) ? height : 0,
       Weight=double.TryParse(wr.Data[i][8].ToString(), out double weight) ? weight : 0,
       TotalWeight=double.TryParse(wr.Data[i][9].ToString(), out double totalWeight) ? totalWeight : 0,
       Area=double.TryParse(wr.Data[i][10].ToString(), out double area) ? area : 0,
      };

      glass.TotalArea=glass.Area*glass.Quantity;
      glass.AreaUsed=glass.Area*glass.Quantity;
      glass.AreaOrdered=glass.Area*glass.Quantity;
      glass.Price=decimal.TryParse(wr.Data[i][7].ToString(), out decimal price) ? price : 0;
      glass.SquareMeterPrice=decimal.TryParse(wr.Data[i][6].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
      glass.TotalPrice=decimal.TryParse(wr.Data[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
      try
      {
       glass.Pallet=wr.Data[i][12].ToString()??string.Empty;
      }
      catch
      {
       _logger.Error("MGDTO: Sapa v.2. Order: {$Order}, worksheet: {$Name}, line number {$Line} pallet column is missing!", new { order, worksheetName, lineNumber });
       glass.Pallet=string.Empty;
      }



      _logger.Debug("GLASS: | Order: {$Order} | Name: {$Worksheet} | Line Number: {$Line} | Item: {$Item} | SortOrder: {$SortOrder} | Reference: {$Reference} | Description: {$Description} | Quantity: {$Quantity} | Width: {$Width} | Height: {$Height} | Weight: {$Weight} | TotalWeight: {$TotalWeight} | Area: {$Area} | TotalArea: {$TotalArea} | Price: {$Price} | SquareMeterPrice: {$SquareMeterPrice} | TotalPrice: {$TotalPrice} | Palett: {$Palett}",
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
      glass.Type=WorksheetType.Glasses_Sapa_v2;
      glasses.Add(glass);
     }
     catch (Exception ex)
     {
      _logger.Error(ex.Message, "MGDTO: Sapa v.2. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
     }
    }

    return await Task.FromResult(glasses);
   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MGDTO: Sapa v.2. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
    return [];
   }
  }

  private async Task<List<GlassDTO>> GetSchucoAsync(A2PWorksheet wr)
  {
   int lineNumber = 0;
   string worksheetName = wr.Name;
   string order = wr.Order;
   try
   {
    if (wr==null)

    {
     _logger.Error("MGDTO: Schuco. Worksheet is empty. Order: {$Order}", order);
     return await Task.Run(() => new List<GlassDTO>());
    }
    if (wr.RowCount==0)
    {
     _logger.Error("MGDTO: Schuco. AppWorksheet is empty. Order: {$Order}", order);
     return await Task.Run(() => new List<GlassDTO>());
    }

    List<GlassDTO> glasses = [];
    for (int i = 0; i<wr.RowCount; i++)
    {
     try
     {
      lineNumber=i+1;
      GlassDTO glass = new()
      {
       Type=WorksheetType.Glasses_Schuco
      };
      glasses.Add(glass);
     }
     catch (Exception ex)
     {
      _logger.Error(ex.Message, "MGDTO: Schuco. For each. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line}", order, worksheetName, lineNumber);
      continue;
     }

    }

    return await Task.Run(() => glasses);
   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MGDTO: Schuco. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line}", order, worksheetName, lineNumber);
    return await Task.Run(() => new List<GlassDTO>());
   }
  }
 }
}