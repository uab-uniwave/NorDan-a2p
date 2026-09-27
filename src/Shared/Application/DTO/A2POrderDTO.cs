namespace a2p.Shared.Application.DTO
{
    public class A2POrderDTO
    {
        public string Order { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public int LockedCount { get; set; } = 0;
        public string LockedFiles { get; set; } = string.Empty;

        public int FileCount { get; set; } = 0;
        public string FileList { get; set; } = string.Empty;
        public int WorksheetCount { get; set; } = 0;
        public string WorksheetList { get; set; } = string.Empty;
        public int ItemCount { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;
        public string ErrorList { get; set; } = string.Empty;
        public bool Import { get; set; } = false;

    }
}