using Domain.Enums;

namespace Application.Models
{
    public class WorksheetDto
    {
        //   public string FileName { get; set; } = string.Empty;

        public string OrderNumber { get; set; } = string.Empty;
        public string ProjectNumber { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;

        public Guid OrderId { get; set; } = Guid.Empty;

        public string Currency { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;

        public string Name { get; set; } = string.Empty;
        public int RowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];

        public double Price { get; set; } = 0;
        public double DiscountAmount { get; set; } = 0;
        public double FinalPrice { get; set; } = 0;

    }

}
