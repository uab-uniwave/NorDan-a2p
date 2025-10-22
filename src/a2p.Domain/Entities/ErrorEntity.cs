using Domain.Enums;

namespace Domain.Entities
{
    public class ErrorEntity

    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? OrderId { get; set; } = Guid.Empty;
        public string? OrderNumber { get; set; } = string.Empty;
        public ErrorLevel Level { get; set; } = ErrorLevel.Fatal;
        public ErrorCode Code { get; set; } = ErrorCode.Application;
        public string Message { get; set; } = string.Empty;

    }
}
