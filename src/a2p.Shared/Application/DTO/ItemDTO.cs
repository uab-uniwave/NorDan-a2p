using System.ComponentModel.DataAnnotations;

using a2p.Shared.Application.Domain.Enums;

namespace a2p.Shared.Application.DTO
{
    public class ItemDTO
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
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        //============================================================================================================================                                
        public double Weight { get; set; } = 0;
        public double WeightWithoutGlass { get; set; } = 0;
        public double WeightGlass { get; set; } = 0;
        //============================================================================================================================
        public double TotalWeight { get; set; } = 0;
        public double TotalWeightWithoutGlass { get; set; } = 0;
        public double TotalWeightGlass { get; set; } = 0;
        //============================================================================================================================
        public double Area { get; set; } = 0;
        public double TotalArea { get; set; } = 0;
        //============================================================================================================================
        public double Hours { get; set; } = 0;
        public double TotalHours { get; set; } = 0;
        //============================================================================================================================
        public double MaterialCost { get; set; } = 0;
        public double LaborCost { get; set; } = 0;
        public double Cost { get; set; } = 0;
        //============================================================================================================================
        public double TotalMaterialCost { get; set; } = 0;
        public double TotalLaborCost { get; set; } = 0;
        public double TotalCost { get; set; } = 0;
        //============================================================================================================================
        public double Price { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;
        //============================================================================================================================
        public string CurrencyCode { get; set; } = "Unknown";
        public double ExchangeRateEUR { get; set; } = 1;

        //============================================================================================================================
        public double MaterialCostEUR { get; set; } = 0;
        public double LaborCostEUR { get; set; } = 0;
        public double CostEUR { get; set; } = 0;
        //============================================================================================================================
        public double TotalMaterialCostEUR { get; set; } = 0;
        public double TotalLaborCostEUR { get; set; } = 0;
        public double TotalCostEUR { get; set; } = 0;
        public double PriceEUR { get; set; } = 0;
        public double TotalPriceEUR { get; set; } = 0;
        public WorksheetType WorksheetType { get; set; } = 0;

    }
}
