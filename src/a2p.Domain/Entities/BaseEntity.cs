namespace Domain.Entities
{

    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
 
        public string? OrderNumber { get; set; } = string.Empty;
        public string? ProjectNumber { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; } = -1;
         public int SalesDocumentVersion { get; set; } = -1;


        public DateTime CreatedUTCDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedUTCDateTime { get; set; }

        public string? CreatedBy { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; } = string.Empty;

    }
}
