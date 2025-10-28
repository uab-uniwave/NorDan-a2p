using Application.DTOs;

using Domain.Enums;

namespace Application.Models
{
    public class Worksheet
    {
        //   public string FileName { get; set; } = string.Empty;


        public SourceAppType SourceAppType { get; set; } = SourceAppType.Unknown;
        public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;

        public string Name { get; set; } = string.Empty;
        public int RowCount { get; set; } = 0;
        public List<List<object>> WorksheetData { get; set; } = [];
        public string Currency { get; set; } = string.Empty;
        public double Price { get; set; } = 0;      //TODO: remove
        public double DiscountAmount { get; set; } = 0;       //TODO: remove
        public double FinalPrice { get; set; } = 0;   //TODO: remove

    }

}
