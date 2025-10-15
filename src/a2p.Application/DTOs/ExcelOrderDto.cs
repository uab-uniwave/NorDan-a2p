using a2p.Application.Models;
using a2p.Domain.Enums;
namespace a2p.Application.DTOs

{
    public class ExcelOrderDto
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string OrderNumber { get; set; } = string.Empty;
        public string? ProjectNumber { get; set; }
        public PrefExcelExcelOrderDto SalesDocument { get; set; } = new PrefExcelExcelOrderDto();
        public int SalesDocumentState { get; set; } = -1;

        public List<Models.File> Files { get; set; } = [];
        public List<ExcelItemDto> ItemsDto { get; set; } = [];
        public List<ErrorEntity> ErrorsDto { get; set; } = [];
        public List<ExcelMaterialDto> MaterialDto { get; set; } = [];
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

        public Decimal TotalUnits { get; set; }

        public Decimal TotalWeight { get; set; } = 0;
        public Decimal TotalWeightWithoutGlass { get; set; } = 0;
        public Decimal TotalWeightGlass { get; set; } = 0;
        public Decimal TotalArea { get; set; } = 0;
        public Decimal TotalHours { get; set; } = 0;
        public Decimal TotalMaterialCost { get; set; } = 0;
        public Decimal TotalLaborCost { get; set; } = 0;
        public Decimal TotalCost { get; set; } = 0;
        public Decimal TotalPrice { get; set; } = 0;
        public double ExchangeRate { get; set; }
        public DateOnly ExchangeRateDate { get; set; }


        public bool DeleteExistsing { get; set; } = false;
        public SourceAppType SourceAppType { get; set; } = SourceAppType.Unknown;



    }
}