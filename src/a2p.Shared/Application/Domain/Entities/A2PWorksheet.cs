using a2p.Shared.Domain.Enums;

namespace a2p.Shared.Application.Domain.Entities
{
    public class A2PWorksheet
    {
        //   public string FileName { get; set; } = string.Empty;

        public string Order { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;
        public string Name { get; set; } = string.Empty;
        public int RowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];

        public decimal Price { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal FinalPrice { get; set; } = 0;

    }

}
