namespace Application.DTOs
{
    public class TaskDto
    {

        public Guid Id { get; set; } = Guid.Empty;
        public string? OrderNumber { get; set; } = string.Empty;
        public Guid? OrderId { get; set; } = Guid.Empty;
        public string? ProjectNumber { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;
        public string PayloadJson { get; set; } = string.Empty;
        public int State { get; set; } = 0;
        public DateTime? ProcessedUTCDateTime { get; set; }
    }
}