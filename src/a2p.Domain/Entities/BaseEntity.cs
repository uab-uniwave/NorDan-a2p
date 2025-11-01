namespace Domain.Entities
{

    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
