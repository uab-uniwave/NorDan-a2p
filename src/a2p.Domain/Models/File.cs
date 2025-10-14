

namespace a2p.Domain.Models
{
    public class File
    {

        public string OrderNumber { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        // public bool IsOrderItemsFile { get; set; } = false;

        public List<Worksheet> Worksheets { get; set; } = [];
    }
}
