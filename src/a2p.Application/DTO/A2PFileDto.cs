namespace a2p.Application.DTO
{
    public class A2PFileDto
    {

        public string Order { get; set; } = string.Empty;

       public string Currency { get; set; } = string.Empty;

        public string File { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

       // public bool IsOrderItemsFile { get; set; } = false;

        public List<A2PWorksheetDto> Worksheets { get; set; } = [];
    }
}
