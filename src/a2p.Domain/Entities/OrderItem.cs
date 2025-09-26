using System.ComponentModel.DataAnnotations;
using a2p.Domain.Enums;

namespace a2p.Domain.Entities
{
    public class OrderItem
    {
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;
        public string SalesDocumentIdPos { get; set; } = Guid.NewGuid().ToString();
        [MaxLength(50)] public string Order { get; set; } = string.Empty;
        [MaxLength(255)] public string Worksheet { get; set; } = string.Empty;
        public int Line { get; set; } = -1;
        public int Column { get; set; } = -1;
        //============================================================================================================================
        [MaxLength(50)] public string? Project { get; set; }

        [MaxLength(50)] public string Item { get; set; } = string.Empty;
        public int SortOrder { get; set; } = -1;
        [MaxLength(255)] public string? Description { get; set; }
        //============================================================================================================================
        public int Quantity { get; set; } = 0;
        //============================================================================================================================
        public decimal Width { get; set; } = 0m;
        public decimal Height { get; set; } = 0m;
        //============================================================================================================================                                
        public decimal Weight { get; set; } = 0m;
        public decimal WeightWithoutGlass { get; set; } = 0m;
        public decimal WeightGlass { get; set; } = 0m;
        //============================================================================================================================
        public decimal TotalWeight { get; set; } = 0m;
        public decimal TotalWeightWithoutGlass { get; set; } = 0m;
        public decimal TotalWeightGlass { get; set; } = 0m;
        //============================================================================================================================
        public decimal Area { get; set; } = 0m;
        public decimal TotalArea { get; set; } = 0m;
        //============================================================================================================================
        public decimal Hours { get; set; } = 0m;
        public decimal TotalHours { get; set; } = 0m;
        //============================================================================================================================
        public decimal MaterialCost { get; set; } = 0m;
        public decimal LaborCost { get; set; } = 0m;
        public decimal Cost { get; set; } = 0m;
        //============================================================================================================================
        public decimal TotalMaterialCost { get; set; } = 0m;
        public decimal TotalLaborCost { get; set; } = 0m;
        public decimal TotalCost { get; set; } = 0m;
        //============================================================================================================================
        public decimal Price { get; set; } = 0m;
        public decimal TotalPrice { get; set; } = 0m;
        //============================================================================================================================
        public string CurrencyCode { get; set; } = "Unknown";
        public decimal ExchangeRateEUR { get; set; } = 1m;

        //============================================================================================================================
        public decimal MaterialCostEUR { get; set; } = 0m;
        public decimal LaborCostEUR { get; set; } = 0m;
        public decimal CostEUR { get; set; } = 0m;
        //============================================================================================================================
        public decimal TotalMaterialCostEUR { get; set; } = 0m;
        public decimal TotalLaborCostEUR { get; set; } = 0m;
        public decimal TotalCostEUR { get; set; } = 0m;
        public decimal PriceEUR { get; set; } = 0m;
        public decimal TotalPriceEUR { get; set; } = 0m;
        public WorksheetType WorksheetType { get; set; } = 0;

    }
}
