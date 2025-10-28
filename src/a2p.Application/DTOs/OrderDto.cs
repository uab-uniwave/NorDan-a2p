using Application.Models;
namespace Application.DTOs

{
    public class OrderDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? OrderNumber { get; set; }
        public string? ProjectNumber { get; set; }

        public SalesDocument SalesDocument { get; set; } = new();

        public List<ItemDto> ItemsDto { get; set; } = [];

        public List<MaterialDto> MaterialsDto { get; set; } = [];

        public List<ExcelFile> ExcelFiles { get; set; } = [];

        public DateTime? OrderDate { get; set; }

        public string? CustomerTitle { get; set; }
        public string? CustomerNumber { get; set; }
        public string? DeliveryAddress { get; set; }
        public DateTime? CorrectionAvailableUntil { get; set; }

        public string? ResponsibleManager { get; set; }

        public string? Currency { get; set; }

        public decimal ExchangeRate { get; set; } = 1m;
        public DateTime? ExchangeRateDate { get; set; }

        public bool Import { get; set; } = true;
        public bool DeleteExistsing { get; set; } = false;

    }
}