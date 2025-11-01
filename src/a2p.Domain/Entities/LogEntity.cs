using Domain.Enums;

namespace Domain.Entities
{

    public class LogEntity
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public string? Message { get; set; }
        public string? Order { get; set; } = string.Empty;
        public string? Worksheet { get; set; } = string.Empty;
        public int? line { get; set; }

    }
}