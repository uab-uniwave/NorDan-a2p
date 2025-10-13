namespace a2p.Infrastructure.Models
{

    public class LogRecord
    {
        public string Timestamp { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public string Worksheet { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}