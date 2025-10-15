using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2p.Domain.Entities
{
    public class OrderEntity : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string CustomerTitle { get; set; } = string.Empty;
        public string CustomerNumber { get; set; } = string.Empty;
        public string ProjectNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public DateTime? CorrectionAvailableUntil { get; set; }
        public string ResponsibleManager { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; }
        public int SalesDocumentVersion { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalWeightWithoutGlass { get; set; }
        public decimal TotalWeightGlass { get; set; }
        public decimal TotalArea { get; set; }
        public decimal TotalHours { get; set; }
        public decimal TotalMaterialCost { get; set; }
        public decimal TotalLaborCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
    }
}
