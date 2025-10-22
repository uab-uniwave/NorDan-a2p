namespace Application.Models
{
    public class FileDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string ProjectNumber { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;

        public Guid OrderId { get; set; } = Guid.Empty;

        public List<WorksheetDto> Worksheets { get; set; } = [];

        public string Currency { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsOrderItemsFile { get; set; } = false;

    }
}
