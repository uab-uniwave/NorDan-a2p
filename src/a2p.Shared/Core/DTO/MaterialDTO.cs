using a2p.Shared.Core.Enums;


using System.ComponentModel.DataAnnotations;

namespace a2p.Shared.Core.DTO
{
    public class MaterialDTO
    {
        [Required, MaxLength(5)] required public string Order { get; set; } = string.Empty;
        [Required, MaxLength(255)] required public string Worksheet { get; set; }
        [Required] required public int? Line { get; set; } = -1;
        [Required] required public int? Column { get; set; } = -1;
        //============================================================================================================================
        public string? Item { get; set; }
        public int? SortOrder { get; set; } = -1;
        [Required, MaxLength(25)] required public string Reference { get; set; } = string.Empty;
        [MaxLength(255)] public string? Description { get; set; }
        //============================================================================================================================
        [Required, MaxLength(50)] required public string Color { get; set; }
        [MaxLength(120)] public string? ColorDescription { get; set; }
        //============================================================================================================================
        public double? Width { get; set; } = 0;
        public double? Height { get; set; } = 0;
        //============================================================================================================================       
        [Required] required public int Quantity { get; set; } = 1;
        public double? PackageQuantity { get; set; } = 1;
        public double? TotalQuantity { get; set; } = 0;
        [Required] required public double RequiredQuantity { get; set; } = 1;
        public double? LeftOverQuantity { get; set; }

        //============================================================================================================================  
        public double? Weight { get; set; } = 0;
        public double? TotalWeight { get; set; } = 0;
        public double? RequiredWeight { get; set; } = 0;
        public double? LeftOverWeight { get; set; } = 0;
        //============================================================================================================================  
        public double? Area { get; set; } = 0;
        public double? TotalArea { get; set; } = 0;
        public double? RequiredArea { get; set; } = 0;
        public double? LeftOverArea { get; set; } = 0;
        //============================================================================================================================  
        public double? Waste { get; set; } = 0;
        //============================================================================================================================  

        public decimal? Price { get; set; } = 0;
        public decimal? TotalPrice { get; set; } = 0;
        public decimal? RequiredPrice { get; set; } = 0;
        public decimal? LeftOverPrice { get; set; } = 0;
        //============================================================================================================================  
        public decimal? SquareMeterPrice { get; set; } = 0;
        //============================================================================================================================
        [MaxLength(255)] public string? Pallet { get; set; }
        //============================================================================================================================
        [MaxLength(255)] public string? CustomField1 { get; set; }
        [MaxLength(255)] public string? CustomField2 { get; set; }
        [MaxLength(255)] public string? CustomField3 { get; set; }
        //============================================================================================================================
        [MaxLength(255)] public string? CustomField4 { get; set; }
        [MaxLength(255)] public string? CustomField5 { get; set; }
        //============================================================================================================================
        [Required] required public MaterialType MaterialType { get; set; } = 0;
        [Required] required public WorksheetType WorksheetType { get; set; } = 0;
        //============================================================================================================================
        [MaxLength(255)] public string? SourceReference { get; set; }
        [MaxLength(255)] public string? SourceDescription { get; set; }
        [MaxLength(255)] public string? SourceColor { get; set; }
        [MaxLength(255)] public string? SourceColorDescription { get; set; }










    }
}

