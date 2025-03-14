using System.ComponentModel.DataAnnotations;

using a2p.Shared.Application.Domain.Enums;

namespace a2p.Shared.Application.DTO
{
    public class ItemDTO
    {
        public string SalesDocumentIdPos { get; set; } = Guid.NewGuid().ToString();
        [Required, MaxLength(50)] required public string Order { get; set; } = string.Empty;
        [Required, MaxLength(255)] required public string Worksheet { get; set; } = string.Empty;
        [Required] required public int Line { get; set; } = -1;
        [Required] required public int Column { get; set; } = -1;
        //============================================================================================================================
        [MaxLength(50)] public string? Project { get; set; }

        [Required, MaxLength(50)] required public string Item { get; set; } = string.Empty;
        [Required] required public int SortOrder { get; set; } = -1;
        [MaxLength(255)] public string? Description { get; set; }
        //============================================================================================================================
        [Required] required public int Quantity { get; set; } = 0;
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
        public decimal MaterialCost { get; set; } = 0;
        public decimal LaborCost { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        //============================================================================================================================
        public decimal TotalMaterialCost { get; set; } = 0;
        public decimal TotalLaborCost { get; set; } = 0;
        public decimal TotalCost { get; set; } = 0;
        //============================================================================================================================
        public decimal Price { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        //============================================================================================================================
        public required string CurrencyCode { get; set; } = "NOK";
        public decimal ExchangeRateEUR { get; set; } = 1;

        //============================================================================================================================
        public decimal MaterialCostEUR { get; set; } = 0;
        public decimal LaborCostEUR { get; set; } = 0;
        public decimal CostEUR { get; set; } = 0;
        //============================================================================================================================
        public decimal TotalMaterialCostEUR { get; set; } = 0;
        public decimal TotalLaborCostEUR { get; set; } = 0;
        public decimal TotalCostEUR { get; set; } = 0;
        //============================================================================================================================
        public decimal PriceEUR { get; set; } = 0;
        public decimal TotalPriceEUR { get; set; } = 0;
        //============================================================================================================================
        [Required] required public WorksheetType WorksheetType { get; set; } = 0;

    }
}