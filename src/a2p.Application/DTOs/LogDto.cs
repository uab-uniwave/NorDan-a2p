using Domain.Enums;

namespace Application.DTOs
{
    public class LogDto

    {

        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; } = LogLevel.Information;

        public string? Message { get; set; }

    }
}
