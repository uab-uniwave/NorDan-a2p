namespace a2p.Shared.Core.Entities.Models
{
    public class A2POrderFile
    {
        public string Order { get; set; } = string.Empty;

        public string File { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsOrderItemsFile { get; set; } = false;

        public List<A2POrderFileWorksheet> OrderFileWorksheets { get; set; } = [];
    }
}
