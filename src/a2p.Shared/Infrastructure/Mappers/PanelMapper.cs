using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Utils;

namespace a2p.Shared.Infrastructure.Mappers
{
 public class PanelMapper : IPanelMapper
 {
  private readonly ILogService _logger;


  public PanelMapper(ILogService logger)
  {
   _logger=logger;
  }
  public async Task<List<PanelDTO>> MapToPanelDTOAsync(A2PWorksheet wr)
  {
   string worksheetName = string.Empty;
   string order = string.Empty;
   {
    try
    {
     if (wr==null)
     {
      _logger.Error("MPDTO. Worksheet is empty");
      return [];
     }
     worksheetName=wr.Name;
     order=wr.Order;

     List<PanelDTO> panels = [];

     switch (wr.WorksheetType)
     {
      case WorksheetType.Panels_Sapa_v1:
       panels=await Task.Run(() => GetSapa1Async(wr));
       break;
      case WorksheetType.Panels_Sapa_v2:
       panels=await Task.Run(() => GetSapa2Async(wr));
       break;
      default:

       _logger.Error("MPDTO. Vendor and/or version unknown, Order: {$Order}, Name {$Name}", order, worksheetName);
       break;
     }

     return panels.Where(p => p!=null).Select(p => p!).ToList();
    }
    catch (Exception ex)
    {
     _logger.Error(ex, "MPDTO. Unhandled error: Last known Order: {$Order}, Name {$Name}.", order, worksheetName);
     return [];

    }
   }

  }
  private async Task<List<PanelDTO>> GetSapa1Async(A2PWorksheet wr)
  {
   int lineNumber = 0;
   string worksheetName = wr.Name;
   string order = wr.Order;
   try
   {
    if (wr.Data.Count==0)
    {
     _logger.Error("MPDTO Sapa v.1. Worksheet is empty. Order: {$Order}", order);

     return [];
    }
    worksheetName=wr.Name;
    order=wr.Order;

    List<PanelDTO> panels = [];
    await Task.Run(() =>
    {

     for (int i = 0; i<wr.RowCount; i++)
     {
      try
      {
       lineNumber=i+1;
       PanelDTO panel = new()
       {
        Type=WorksheetType.Panels_Sapa_v1
       };
       panels.Add(panel);
      }
      catch (Exception ex)
      {
       _logger.Error(ex.Message, "MPDTO Sapa v.1. For each Panel. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line}", order, worksheetName, lineNumber);
       continue;

      }
     }
    });

    return panels;
   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MPDTO Sapa v.1. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line}", order, worksheetName, lineNumber);
    return [];
   }
  }
  private async Task<List<PanelDTO>> GetSapa2Async(A2PWorksheet wr)
  {

   int lineNumber = 0;
   string worksheetName = wr.Name;
   string order = wr.Order;
   try
   {
    if (wr.Data.Count==0)
    {
     _logger.Error("MPDTO Sapa v.2. Worksheet is empty. Order: {$Order}", new { order });
     return [];
    }
    worksheetName=wr.Name;
    order=wr.Order;

    List<PanelDTO> panelsResult = [];


    return panelsResult=await Task.Run(() =>
     {


      List<PanelDTO> panels = [];
      for (int i = 4; i<wr.RowCount; i++)
      {
       try
       {
        lineNumber=i+1;

        PanelDTO panel = new()
        {
         WorksheetName=wr.Name,
         Order=wr.Order,
         Item=wr.Data[i][1]?.ToString()??"",
         SortOrder=i-3,
         Reference=wr.Data[i][3]?.ToString()??"",
         Color=wr.Data[i][2]?.ToString()??string.Empty,
         Description=wr.Data[i][4]?.ToString()??string.Empty,
         Quantity=int.TryParse(wr.Data[i][5].ToString(), out int quantity) ? quantity : 0,
         Width=double.TryParse(wr.Data[i][6].ToString(), out double width) ? width : 0,
         Height=double.TryParse(wr.Data[i][7].ToString(), out double height) ? height : 0,
         Area=double.TryParse(wr.Data[i][10].ToString(), out double area) ? area : 0
        };
        panel.TotalArea=panel.Area*panel.Quantity;
        panel.Price=decimal.TryParse(wr.Data[i][9].ToString(), out decimal price) ? price : 0;
        panel.SquareMeterPrice=decimal.TryParse(wr.Data[i][8].ToString(), out decimal squareMeterPrice) ? squareMeterPrice : 0;
        panel.TotalPrice=decimal.TryParse(wr.Data[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
        panel.Type=WorksheetType.Panels_Sapa_v2;
        _logger.Debug("MPDTO Sapa v.2. PANEL: | Name: {$Worksheet} | LineNumber {$Line} | Order: {$Order} | Item: {$Item} | SortOrder: {$SortOrder} | Reference: {$Reference} | Description: {$Description} | Quantity: {$Quantity} | Width: {$Width} | Height: {$Height} | Area: {$Area} | TotalArea: {$TotalArea} | Price: {$Price} | SquareMeterPrice: {$SquareMeterPrice} | TotalPrice: {$TotalPrice} |",

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
        _logger.Error("MPDTO Sapa v.2. For each Panel. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$Line},${Exception}", order, worksheetName, lineNumber, ex.Message);
        continue;

       }
      }

      return panels;

     });
   }



   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MPDTO Sapa v.2. For each Panel. Unhandled Error. Last known success action. Order: {$Order}, Worksheet: {$Name}, LineNumber: {$LineNumber, ${Exception}", order, worksheetName, lineNumber, ex.Message);
    return [];
   }
  }

 }
}