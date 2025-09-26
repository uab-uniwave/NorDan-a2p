namespace a2p.Application.DTO
{

    public class A2PLogRecord
    {
        public string Timestamp { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public string Worksheet { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public Dictionary<string, object?> Properties { get; set; } = [];
    }
}