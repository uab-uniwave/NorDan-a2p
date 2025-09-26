using a2p.Domain.Enums;

namespace a2p.Application.DTO
{
    public class A2PWorksheetDto
    {
        //   public string FileName { get; set; } = string.Empty;

        public string Order { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;
        public string Name { get; set; } = string.Empty;
        public int RowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];

        public double  Price { get; set; } = 0;
        public double  DiscountAmount { get; set; } = 0;
        public double  FinalPrice { get; set; } = 0;

    }

}
