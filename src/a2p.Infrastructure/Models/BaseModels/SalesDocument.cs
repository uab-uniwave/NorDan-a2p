using a2p.Domain.Enums;

namespace a2p.Infrastructure.Models.BaseModels
{
    partial class SalesDocument
    {

        public Guid Id { get; set; } = Guid.Empty;
        public int Number { get; set; } = -1;
        public int Version { get; set; } = -1;

        public OrderState State { get; set; } = 0;

    }
}
