namespace Application.DTOs

{
    public class ApiExcelExcelOrderDto
    {

        public string OrderNumber { get; set; } = string.Empty;
        public string? ProjectNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerTitle { get; set; }
        public string? CustomerNumber { get; set; }
        public string? DeliveryAddress { get; set; }
        public DateOnly? CorrectionAvailableUnitil { get; set; }
        public string? ResponsibleManager { get; set; }
        public List<string> Files { get; set; } = [];
    }
}