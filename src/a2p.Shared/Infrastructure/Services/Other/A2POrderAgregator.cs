using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;

namespace a2p.Shared.Infrastructure.Services.Other
{
    public static class A2POrderAgregator
    {
        // Find Methods
        public static async Task<A2POrder?> FindOrderAsync(List<A2POrder> orders, string orderNumber)
        {
            return await Task.FromResult(orders.FirstOrDefault(o => o.Order == orderNumber));
        }

        public static async Task<A2PFile?> FindFileAsync(List<A2POrder> orders, string orderNumber, string fileName)
        {
            return (await FindOrderAsync(orders, orderNumber))?.OrderFiles.FirstOrDefault(f => f.File == fileName);
        }

        public static async Task<A2PWorksheet?> FindWorksheetAsync(List<A2POrder> orders, string orderNumber, string fileName, string worksheetName)
        {
            return (await FindFileAsync(orders, orderNumber, fileName))?.OrderFileWorksheets.FirstOrDefault(w => w.Worksheet == worksheetName);
        }

        public static async Task<A2PWorksheet?> FindWorksheetByTypeAsync(List<A2POrder> orders, string orderNumber, string fileName, WorksheetType type)
        {
            return (await FindFileAsync(orders, orderNumber, fileName))?.OrderFileWorksheets.FirstOrDefault(w => w.Type == type);
        }

        // Add or Update Methods
        public static async Task<List<A2POrder>> AddOrUpdateOrderAsync(List<A2POrder> orders, A2POrder newOrder)
        {
            A2POrder? existingOrder = await FindOrderAsync(orders, newOrder.Order);
            if (existingOrder != null)
            {
                // Update existing order
                existingOrder.OrderFiles = newOrder.OrderFiles;
            }
            else
            {
                // Add new order
                orders.Add(newOrder);
            }
            return orders;
        }

        public static async Task<List<A2POrder>> UpdateOrderFilesAsync(List<A2POrder> orders, string orderNumber, List<A2PFile> files)
        {
            A2POrder? orderToUpdate = await FindOrderAsync(orders, orderNumber);
            if (orderToUpdate != null)
            {
                foreach (A2PFile file in files)
                {
                    A2PFile? existingFile = orderToUpdate.OrderFiles.FirstOrDefault(f => f.File == file.File);
                    if (existingFile != null)
                    {
                        // Update file properties
                        existingFile.FilePath = file.FilePath;
                        existingFile.FileName = file.FileName;
                        existingFile.IsLocked = file.IsLocked;
                        existingFile.OrderFileWorksheets = file.OrderFileWorksheets;
                    }
                    else
                    {
                        // Add new file
                        orderToUpdate.OrderFiles.Add(file);
                    }
                }
            }
            return orders;
        }

        public static async Task<List<A2POrder>> UpdateOrderWorksheetsAsync(List<A2POrder> orders, string orderNumber, string fileName, List<A2PWorksheet> worksheets)
        {
            A2PFile? fileToUpdate = await FindFileAsync(orders, orderNumber, fileName);
            if (fileToUpdate != null)
            {
                foreach (A2PWorksheet worksheet in worksheets)
                {
                    A2PWorksheet? existingWorksheet = fileToUpdate.OrderFileWorksheets.FirstOrDefault(w => w.Worksheet == worksheet.Worksheet);
                    if (existingWorksheet != null)
                    {
                        // Update worksheet properties
                        existingWorksheet.RowCount = worksheet.RowCount;
                        existingWorksheet.WorksheetData = worksheet.WorksheetData;
                        existingWorksheet.Type = worksheet.Type;
                    }
                    else
                    {
                        // Add new worksheet
                        fileToUpdate.OrderFileWorksheets.Add(worksheet);
                    }
                }
            }
            return orders;
        }

        // Delete Methods
        public static async Task<List<A2POrder>> DeleteOrderAsync(List<A2POrder> orders, string orderNumber)
        {
            A2POrder? orderToRemove = await FindOrderAsync(orders, orderNumber);
            if (orderToRemove != null)
            {
                _ = orders.Remove(orderToRemove);
            }
            return orders;
        }

        public static async Task<List<A2POrder>> DeleteFileAsync(List<A2POrder> orders, string orderNumber, string fileName)
        {
            A2POrder? order = await FindOrderAsync(orders, orderNumber);
            if (order != null)
            {
                A2PFile? fileToRemove = order.OrderFiles.FirstOrDefault(f => f.File == fileName);
                if (fileToRemove != null)
                {
                    _ = order.OrderFiles.Remove(fileToRemove);
                }
            }
            return orders;
        }

        public static async Task<List<A2POrder>> DeleteWorksheetAsync(List<A2POrder> orders, string orderNumber, string fileName, string worksheetName)
        {
            A2PFile? file = await FindFileAsync(orders, orderNumber, fileName);
            if (file != null)
            {
                A2PWorksheet? worksheetToRemove = file.OrderFileWorksheets.FirstOrDefault(w => w.Worksheet == worksheetName);
                if (worksheetToRemove != null)
                    _ = file.OrderFileWorksheets.Remove(worksheetToRemove);
            }

            return orders;
        }

        // Get Methods
        public static async Task<List<A2POrder>> GetOrdersAsync(List<A2POrder> orders)
        {
            return await Task.FromResult(orders);
        }

        public static async Task<List<A2PFile>> GetOrderFilesAsync(List<A2POrder> orders, string orderNumber)
        {
            return (await FindOrderAsync(orders, orderNumber))?.OrderFiles ?? [];
        }

        public static async Task<List<A2PWorksheet>> GetOrderFileWorksheetsAsync(List<A2POrder> orders, string orderNumber, string fileName)
        {
            return (await FindFileAsync(orders, orderNumber, fileName))?.OrderFileWorksheets ?? [];
        }

        public static async Task<List<A2PWorksheet>> GetOrderAllWorksheetsAsync(List<A2POrder> orders, string orderNumber)
        {
            return (await FindOrderAsync(orders, orderNumber))?.OrderFiles.SelectMany(f => f.OrderFileWorksheets).ToList() ?? [];
        }

        public static async Task<List<A2PWorksheet>> GetOrderWorksheetsByFileNameAsync(List<A2POrder> orders, string orderNumber, string fileName)
        {
            return await GetOrderFileWorksheetsAsync(orders, orderNumber, fileName);
        }
    }
}