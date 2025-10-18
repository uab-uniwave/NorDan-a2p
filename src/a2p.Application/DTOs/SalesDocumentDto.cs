using a2p.Domain.Enums;
namespace a2p.Application.DTOs
{
    public class SalesDocumentDto
    {

        public int Number
        { get; set; } = -1;
        public int Version { get; set; } = -1;
        public Guid Id { get; set; } = Guid.Empty;
        public OrderState State { get; set; } = OrderState.None;
        public string SalesPerson { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;

    }
}

