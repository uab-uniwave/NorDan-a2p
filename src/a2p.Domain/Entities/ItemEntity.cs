using a2p.Domain.Enums;

namespace a2p.Domain.Entities
{
    public class ItemEntity
    {
        public Guid IdPos { get; set; } = Guid.Empty;
        public int Line { get; set; } = -1;
        public int Column { get; set; } = -1;
        //============================================================================================================================
        public string OrderNumber { get; set; } = string.Empty;
        public string Worksheet { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;

        public string ItemName { get; set; } = string.Empty;
        public int SortOrder { get; set; } = -1;
        public string Description { get; set; } = string.Empty;
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