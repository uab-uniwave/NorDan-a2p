namespace a2p.Domain.Entities
{

    public abstract class BaseEntity
    {
        public Guid RowId { get; set; } = Guid.NewGuid();
        public DateTime CreatedUTCDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedUTCDateTime { get; set; }
    }
}
