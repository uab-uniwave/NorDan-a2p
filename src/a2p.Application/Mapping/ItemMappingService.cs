using a2p.Application.DTOs;
using a2p.Domain.Entities;

namespace a2p.Application.Mapping
{

    public class ItemMappingService : IItemMappingService
    {
        public ItemEntity MapToEntity(ExcelItemDto dto)
        {
            try
            {
                ItemEntity item = new ItemEntity();
                item.Id = dto.RowId;
                item.SalesDocumentNumber = dto.SalesDocumentNumber;
                item.SalesDocumentVersion = dto.SalesDocumentVersion;
                item.OrderNumber = dto.OrderNumber;
                item.ItemName = dto.ItemName;
                item.Worksheet = dto.Worksheet;
                item.Line = dto.Line;
                item.Column = dto.Column;
                item.ItemName = dto.ItemName;
                item.SortOrder = dto.SortOrder;
                item.Description = dto.Description;
                item.Quantity = dto.Quantity;
                item.Width = dto.Width;
                item.Height = dto.Height;
                item.Weight = dto.Weight;
                item.WeightWithoutGlass = dto.WeightWithoutGlass;
                item.WeightGlass = dto.WeightGlass;
                item.TotalWeight = dto.TotalWeight;
                item.TotalWeightWithoutGlass = dto.TotalWeightWithoutGlass;
                item.TotalWeightGlass = dto.TotalWeightGlass;
                item.Area = dto.Area;
                item.TotalArea = dto.TotalArea;
                item.Hours = dto.Hours;
                item.TotalHours = dto.TotalHours;
                item.MaterialCost = dto.MaterialCost;
                item.LaborCost = dto.LaborCost;
                item.Cost = dto.Cost;
                item.TotalMaterialCost = dto.TotalMaterialCost;
                item.TotalLaborCost = dto.TotalLaborCost;
                item.TotalCost = dto.TotalCost;
                item.Price = dto.Price;
                item.TotalPrice = dto.TotalPrice;
                item.CurrencyCode = dto.CurrencyCode;
                item.ExchangeRateEUR = dto.ExchangeRateEUR;

                item.MaterialCostEUR = dto.MaterialCostEUR;
                item.LaborCostEUR = dto.LaborCostEUR;
                item.CostEUR = dto.CostEUR;
                item.TotalMaterialCostEUR = dto.TotalMaterialCostEUR;
                item.TotalLaborCostEUR = dto.TotalLaborCostEUR;
                item.TotalCostEUR = dto.TotalCostEUR;


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error mapping ItemDto to ItemEntity: {ex.Message}", ex);
            }



        }
    }



}