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
            _logger = logger;
        }

        public A2POrder? FindOrder(List<A2POrder> orders, string orderNumber)
        {
            try
            {
                return orders.FirstOrDefault(o => o.Number == orderNumber);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error finding order with number {OrderNumber}", orderNumber);
                return null;
            }
        }

        public A2PFile? FindFile(List<A2POrder> orders, string orderNumber, string file)
        {
            try
            {
                return orders
                    .FirstOrDefault(o => o.Number == orderNumber)?
                    .Files.FirstOrDefault(f => f.File == file);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error finding file {File} in order {OrderNumber}", file, orderNumber);
                return null;
            }
        }

        public A2PWorksheet? FindWorksheet(List<A2POrder> orders, string orderNumber, string file, string worksheet)
        {
            try
            {
                return orders
                    .FirstOrDefault(o => o.Number == orderNumber)?
                    .Files.FirstOrDefault(f => f.File == file)?
                    .FileWorksheets.FirstOrDefault(w => w.Name == worksheet);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error finding worksheet {Worksheet} in file {File} of order {OrderNumber}", worksheet, file, orderNumber);
                return null;
            }
        }

        public A2PWorksheet? FindWorksheetByType(List<A2POrder> orders, string orderNumber, string file, WorksheetType Type)
        {
            try
            {
                return orders
                    .FirstOrDefault(o => o.Number == orderNumber)?
                    .Files.FirstOrDefault(f => f.File == file)?
                    .FileWorksheets.FirstOrDefault(w => w.WorksheetType == Type);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled error finding worksheets by Type");
                return null;
            }
        }

        public List<A2POrder> UpdateOrderFiles(List<A2POrder> orderList, string orderNumber, List<A2PFile> files)
        {
            try
            {
                // Find the order to update
                A2POrder? orderToUpdate = FindOrder(orderList, orderNumber);

                // Update the files list for the found order
                orderToUpdate?.Files.AddRange(files); // AddRange is more concise for adding multiple items

                // Return the updated list
                return orderList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating files for order {OrderNumber}", orderNumber);
                return orderList;
            }
        }
    }

}
