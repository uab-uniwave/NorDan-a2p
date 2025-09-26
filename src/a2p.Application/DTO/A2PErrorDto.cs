using a2p.Domain.Enums;

namespace a2p.Application.DTO
{
    public class A2PErrorDto
    {
        public string Order { get; set; } = string.Empty;
        public ErrorLevel Level { get; set; } = ErrorLevel.Fatal;
        public ErrorCode Code { get; set; } = ErrorCode.Application;
        public string Message { get; set; } = string.Empty;

    }
}
