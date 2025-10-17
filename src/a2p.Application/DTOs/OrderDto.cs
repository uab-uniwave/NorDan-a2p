using a2p.Application.Models;
using a2p.Domain.Enums;
namespace a2p.Application.DTOs

{
    public class OrderDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OrderNumber { get; set; } = string.Empty;
        public string? ProjectNumber { get; set; }
        public PrefOrderDto SalesDocument { get; set; } = new PrefOrderDto();
        public int SalesDocumentState { get; set; } = -1;

        public List<Models.File> Files { get; set; } = [];
        public List<ItemDto> ItemsDto { get; set; } = [];
        public List<ErrorEntity> ErrorsDto { get; set; } = [];
        public List<MaterialDto> MaterialsDto { get; set; } = [];
        public string Currency { get; set; } = string.Empty;

        public int LockedCount { get; set; } = 0;
        public string LockedFiles { get; set; } = string.Empty;

        public int FileCount { get; set; } = 0;
        public string FileList { get; set; } = string.Empty;
        public int WorksheetCount { get; set; } = 0;
        public List<Worksheet> Worksheets { get; set; } = [];
        public int ItemCount { get; set; } = 0;
        public int MaterialCount { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;
        public string ErrorList { get; set; } = string.Empty;
        public bool Import { get; set; } = false;

        public int TotalQuantity { get; set; } = 0;

        public decimal TotalUnits { get; set; }

        public decimal TotalWeight { get; set; } = 0;
        public decimal TotalWeightWithoutGlass { get; set; } = 0;
        public decimal TotalWeightGlass { get; set; } = 0;
        public decimal TotalArea { get; set; } = 0;
        public decimal TotalHours { get; set; } = 0;
        public decimal TotalMaterialCost { get; set; } = 0;
        public decimal TotalLaborCost { get; set; } = 0;
        public decimal TotalCost { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public double ExchangeRate { get; set; }
        public DateOnly ExchangeRateDate { get; set; }

        public bool DeleteExistsing { get; set; } = false;
        public SourceAppType SourceAppType { get; set; } = SourceAppType.Unknown;

    }
}