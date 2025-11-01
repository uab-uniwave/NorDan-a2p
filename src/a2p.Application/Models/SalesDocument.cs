using Domain.Enums;
namespace Application.Models
{
    public class SalesDocument
    {

        public Guid? RowId { get; set; }
        public int Number { get; set; }
        public int Version { get; set; }

        public DateTime? CreationDate { get; set; } // OrderDate
        public DateTime? ShopExitDate { get; set; } // FinishProductionUntil
        public DateTime? BreakdownDate { get; set; } // CorrectionAvailableUntil

        public string? PriceCurrency { get; set; } //Currency 
        public string? CustomerName { get; set; } //CustomerTitle
        public int CustomerCode { get; set; } //internal 
        public string? User1 { get; set; } //ProjectNumber
        public string? CustomerAddress1 { get; set; }
        public string? CustomerPostalCode { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerProvince { get; set; }
        public string? CustomerCountry { get; set; }
        public OrderState State { get; set; } = OrderState.None;
        public string? Reference { get; set; } //OrderNumber

        public int SalesmanCode { get; set; }

        public string? SalesmanName { get; set; }

    }
}

