using a2p.Shared.Core.Enums;

namespace a2p.Shared.Core.Entities.Models
{
    public class A2PWorksheet
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int Items { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
        public WorksheetType Type { get; set; } = WorksheetType.Unknown;
        public string Worksheet { get; set; } = string.Empty;
        public int RowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];

    }

}
