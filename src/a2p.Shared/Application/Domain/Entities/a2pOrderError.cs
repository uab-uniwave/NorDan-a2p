using a2p.Shared.Application.Domain.Enums;

namespace a2p.Shared.Application.Domain.Entities
{
    public class A2PError
    {
        public string Order { get; set; } = string.Empty;
        public ErrorLevel Level { get; set; } = ErrorLevel.Fatal;
        public ErrorCode Code { get; set; } = ErrorCode.Application;
        public string Message { get; set; } = string.Empty;

    }
}
