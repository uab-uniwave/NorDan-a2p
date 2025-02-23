namespace a2p.Shared.Core.DTO
{
    namespace a2p.Shared.Core.DTO

    {
        public class OrderDTO
        {
            public string Order { get; set; } = string.Empty;
            public string Currency { get; set; } = string.Empty;
            public int FileCount { get; set; } = 0;
            public string FileList { get; set; } = string.Empty;
            public int WorksheetCount { get; set; } = 0;
            public string WorksheetList { get; set; } = string.Empty;
            public int ItemCount { get; set; } = 0;
            public string ItemList { get; set; } = string.Empty;
            public int Materials { get; set; } = 0;
            public bool Import { get; set; } = true;
            public int ErrorCount { get; set; } = 0;
            public string ErrorList { get; set; } = string.Empty;
            public int WarningCount { get; set; } = 0;
            public string WarningList { get; set; } = string.Empty;
        }
    }
}