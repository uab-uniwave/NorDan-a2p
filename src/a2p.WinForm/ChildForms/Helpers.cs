using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;

namespace a2p.WinForm.ChildForms
{
    /// <summary>
    /// Provides helper methods for various operations.
    /// </summary>
    public class Helpers
    {
        private readonly ILogService _logService;

        public Helpers( ILogService logService)
        {
            _logService = logService;
        }

        public A2POrder? FindOrder(List<A2POrder> orders, string orderNumber)
        {
            try
            {
                return orders.FirstOrDefault(o => o.Order == orderNumber);
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error finding order with number {Order}", orderNumber);
                return null;
            }
        }

        public A2POrderFile? FindFile(List<A2POrder> orders, string orderNumber, string file)
        {
            try
            {
                return orders
                    .FirstOrDefault(o => o.Order == orderNumber)?
                    .OrderFiles.FirstOrDefault(f => f.File == file);
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error finding file {File} in order {Order}", file, orderNumber);
                return null;
            }
        }

        public A2POrderFileWorksheet? FindWorksheet(List<A2POrder> orders, string orderNumber, string file, string worksheet)
        {
            try
            {
                return orders
                    .FirstOrDefault(o => o.Order == orderNumber)?
                    .OrderFiles.FirstOrDefault(f => f.File == file)?
                    .OrderFileWorksheets.FirstOrDefault(w => w.Worksheet == worksheet);
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error finding worksheet {Worksheet} in file {File} of order {Order}", worksheet, file, orderNumber);
                return null;
            }
        }

        public A2POrderFileWorksheet? FindWorksheetByType(List<A2POrder> orders, string orderNumber, string file, WorksheetType Type)
        {
            try
            {
                return orders
                    .FirstOrDefault(o => o.Order == orderNumber)?
                    .OrderFiles.FirstOrDefault(f => f.File == file)?
                    .OrderFileWorksheets.FirstOrDefault(w => w.Type == Type);
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Unhandled error finding worksheets by Type");
                return null;
            }
        }

        public List<A2POrder> UpdateOrderFiles(List<A2POrder> orderList, string orderNumber, List<A2POrderFile> files)
        {
            try
            {
                // Find the order to update
                A2POrder? orderToUpdate = FindOrder(orderList, orderNumber);

                // Update the files list for the found order
                orderToUpdate?.OrderFiles.AddRange(files); // AddRange is more concise for adding multiple items

                // Return the updated list
                return orderList;
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error updating files for order {Order}", orderNumber);
                return orderList;
            }
        }
    }

}
