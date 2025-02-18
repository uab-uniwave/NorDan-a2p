using a2p.Shared.Domain.Enums;

namespace a2p.Shared.Domain.Entities
{
    public class A2POrderError
    {
        required public string Order { get; set; }

        required public ErrorLevel Level { get; set; }
        required public ErrorCode Code { get; set; }

        required public string Description { get; set; }

    }
}
