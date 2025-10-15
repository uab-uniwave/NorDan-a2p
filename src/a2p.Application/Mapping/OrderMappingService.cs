using a2p.Domain.Entities;

namespace a2p.Application.Mapping
{

    public class OrderMappingService : IOrderMappingService
    {




        public OrderEntity MapExcelDtoToEntity(ExcelOrderDto dto)
        {

            try
            {
                OrderEntity order = new OrderEntity();
                order.Id = dto.SalesDocument.Id;
                order.OrderNumber = dto.OrderNumber;
                order.ProjectNumber = dto.ProjectNumber;
                order.SalesDocumentNumber = dto.SalesDocument.Number;
                order.SalesDocumentVersion = dto.SalesDocument.Version;
                order.SourceAppType = dto.SourceAppType;
                order.ItemCount = dto.ItemCount;
                order.MaterialCount = dto.MaterialCount;
                order.ErrorCount = dto.ErrorCount;
                order.TotalQuantity = dto.TotalQuantity;
                order.TotalUnits = dto.TotalUnits;
                order.TotalWeight = dto.TotalWeight;
                order.TotalWeightWithoutGlass = dto.TotalWeightWithoutGlass;
                order.TotalWeightGlass = dto.TotalWeightGlass;
                order.TotalArea = dto.TotalArea;
                order.TotalHours = dto.TotalHours;
                order.TotalMaterialCost = dto.TotalMaterialCost;
                order.TotalLaborCost = dto.TotalLaborCost;
                order.TotalCost = dto.TotalCost;
                order.TotalPrice = dto.TotalPrice;
                order.Currency = dto.Currency;
                order.ExchangeRate = dto.ExchangeRate;
                order.ExchangeRateDate = dto.ExchangeRateDate;
                return order;


            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error mapping ItemDto to ItemEntity: {ex.Message}", ex);
            }

        }
    }


}
