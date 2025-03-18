
namespace a2p.Shared.Core.DTO
{
    namespace a2p.Shared.Core.DTO

    {
        public class OrderDTO
        {
            
            public string Order = string.Empty;
            public string SalesDocument { get; set; } = string.Empty;
            public int Quantity { get; set; } = 0;
            public double Area { get; set; } = 0;
            public double Weight { get; set; } = 0;
            public double Hours { get; set; } = 0;
            public double  Cost { get; set; } = 0;
            public double  Amount { get; set; } = 0;
            public string Currency { get; set; } = string.Empty;
            public int FileCount { get; set; } = 0;
            public string FileList { get; set; } = string.Empty;
            public int WorksheetCount { get; set; } = 0;
            public string WorksheetList { get; set; } = string.Empty;
            public int Items { get; set; } = 0;
            public string ItemList { get; set; } = string.Empty;
            public int Materials { get; set; } = 0;
            public bool Import { get; set; } = true;
            public int WarningCount { get; set; } = 0;
            public string WarningList { get; set; } = string.Empty;
            public int ErrorCount { get; set; } = 0;
            public string ErrorList { get; set; } = string.Empty;
            public int FatalCount { get; set; } = 0;
            public string FatalList { get; set; } = string.Empty;
        }
    }
}
