namespace a2p.Application.Models
{
    public class FileDto
    {

        public string OrderNumber { get; set; } = string.Empty;

        public List<WorksheetDto> Worksheets { get; set; } = [];

        public string Currency { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsOrderItemsFile { get; set; } = false;

    }
}
