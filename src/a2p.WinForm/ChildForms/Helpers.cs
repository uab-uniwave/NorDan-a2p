using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Utils;

namespace a2p.WinForm.ChildForms
{
 /// <summary>
 /// Provides helper methods for various operations.
 /// </summary>
 public class Helpers
 {

  private readonly ILogService _logger;

  public Helpers(ILogService logger)
  {
   _logger=logger;
  }




  public static A2POrder? FindOrder(List<A2POrder> orders, string orderNumber)
  {
   return orders.FirstOrDefault(o => o.Number==orderNumber);
  }



  public static A2PFile? FindFile(List<A2POrder> orders, string orderNumber, string file)
  {

   return orders
    .FirstOrDefault(o => o.Number==orderNumber)?
    .Files.FirstOrDefault(f => f.File==file);
  }

  public static A2PWorksheet? FindWorksheet(List<A2POrder> orders, string orderNumber, string file, string worksheet)
  {
   return orders
    .FirstOrDefault(o => o.Number==orderNumber)?
    .Files.FirstOrDefault(f => f.File==file)?
    .FileWorksheets.FirstOrDefault(w => w.Name==worksheet);



  }
  public static A2PWorksheet? FindWorksheetByType(List<A2POrder> orders, string orderNumber, string file, WorksheetType Type)
  {
   return orders
    .FirstOrDefault(o => o.Number==orderNumber)?
    .Files.FirstOrDefault(f => f.File==file)?
    .FileWorksheets.FirstOrDefault(w => w.WorksheetType==Type);


  }
  public static List<A2POrder> UpdateOrderFiles(List<A2POrder> orderList, string orderNumber, List<A2PFile> files)
  {
   // Find the order to update
   A2POrder? orderToUpdate = FindOrder(orderList, orderNumber);

   // Update the files list for the found order
   orderToUpdate?.Files.AddRange(files); // AddRange is more concise for adding multiple items

   // Return the updated list
   return orderList;
  }


  //public static List<A2POrder> UpdateOrderWorksheets(List<A2POrder> orderList, string orderNumber, List<A2PWorksheet> worksheets)
  //{
  // Find the order to update
  // var orderToUpdate = UpdateOrderWorksheets(orderList, orderNumber);

  // if (orderToUpdate != null)
  // {
  //  Update the files list for the found order

  //  FindFile// AddRange is more concise for adding multiple items
  // }

  // Return the updated list
  // return orderList;
  //}
 }

}
