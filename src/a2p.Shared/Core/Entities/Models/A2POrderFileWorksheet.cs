using a2p.Shared.Core.Enums;

namespace a2p.Shared.Core.Entities.Models
{
    public class A2POrderFileWorksheet
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string OrderCurrency { get; set; } = string.Empty;
        public int Items { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
        public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;
        public string WorksheetName { get; set; } = string.Empty;
        public int WorkSheetRowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];

    }

}
