using a2p.Shared.Core.DTO;
using a2p.Shared.Domain.Enums;

namespace a2p.Shared.Domain.Entities
{
    public class A2PWorksheet
    {
        public string FileName { get; set; } = string.Empty;
        public WorksheetType Type { get; set; } = WorksheetType.Unknown;
        public string Worksheet { get; set; } = string.Empty;
        public int RowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];
        public List<MaterialDTO> Materials { get; set; } = new List<MaterialDTO>();
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
       
    }

}
