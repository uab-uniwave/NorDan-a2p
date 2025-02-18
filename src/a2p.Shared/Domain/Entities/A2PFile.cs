using System.Collections.Generic;

namespace a2p.Shared.Domain.Entities
{
    public class A2PFile
    {
        public string File { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsOrderItemsFile { get; set; } = false;

        public List<A2PWorksheet> Worksheets { get; set; } = new List<A2PWorksheet>();
    }
}
