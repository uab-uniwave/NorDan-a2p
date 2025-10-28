using Domain.Enums;

namespace Application.DTOs
{
    public class ErrorDto
    {
        public string? OrderNumber { get; set; }
        public ErrorLevel Level { get; set; } = ErrorLevel.Fatal;
        public ErrorCode Code { get; set; } = ErrorCode.Application;
        public string? Message { get; set; }

    }
}
