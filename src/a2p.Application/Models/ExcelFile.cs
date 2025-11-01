namespace Application.Models
{
    public class ExcelFile
    {
        public List<Worksheet> Worksheets { get; set; } = [];

        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsOrderItemsFile { get; set; } = false;

    }
}
