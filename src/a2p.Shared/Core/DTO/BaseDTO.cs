public class BaseDTO
{
    public int SalesDocumentNumber { get; set; } = 0;
    public int SalesDocumentVersion { get; set; } = 0;
    public string Worksheet { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
    public int Line { get; set; } = 0;
    public int Column { get; set; } = 0;
    public string Item { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Area { get; set; } = 0;
    public int Quantity { get; set; } = 0;
    public decimal Price { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
    public string SourceReference { get; set; } = string.Empty;
    public string SourceColor { get; set; } = string.Empty;
    public string SourceDescription { get; set; } = string.Empty;



}
