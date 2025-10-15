namespace a2p.Domain.Entities
{

    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedUTCDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedUTCDateTime { get; set; }

        public string? CreatedBy { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; } = string.Empty;

    }
}
