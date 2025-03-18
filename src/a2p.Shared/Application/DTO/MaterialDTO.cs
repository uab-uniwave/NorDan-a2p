// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

using a2p.Shared.Application.Domain.Enums;

namespace a2p.Shared.Application.DTO
{
    public class MaterialDTO
    {
        [MaxLength(50)] public string Order { get; set; } = string.Empty;
        [MaxLength(255)] public string Worksheet { get; set; } = string.Empty;
        [Required] public int Line { get; set; } = -1;
        [Required] public int Column { get; set; } = -1;
        //============================================================================================================================
        public string? Item { get; set; } = string.Empty;
        public int SortOrder { get; set; } = -1;
        //============================================================================================================================
        [Required, MaxLength(25)] public string ReferenceBase { get; set; } = string.Empty;
        [Required, MaxLength(25)] public string Reference { get; set; } = string.Empty;
        [MaxLength(255)] public string? Description { get; set; }
        //============================================================================================================================
        [MaxLength(50)] public string Color { get; set; } = string.Empty;
        [MaxLength(120)] public string? ColorDescription { get; set; }
        //============================================================================================================================
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        //============================================================================================================================       
        [Required] public int Quantity { get; set; } = 0;
        public double PackageQuantity { get; set; } = 0;
        public double TotalQuantity { get; set; } = 0;
        [Required] public double RequiredQuantity { get; set; } = 0;
        public double LeftOverQuantity { get; set; } = 0;
        //============================================================================================================================  
        public double Weight { get; set; } = 0;
        public double TotalWeight { get; set; } = 0;
        public double RequiredWeight { get; set; } = 0;
        public double LeftOverWeight { get; set; } = 0;
        //============================================================================================================================  
        public double Area { get; set; } = 0;
        public double TotalArea { get; set; } = 0;
        public double RequiredArea { get; set; } = 0;
        public double LeftOverArea { get; set; } = 0;
        //============================================================================================================================  
        public double Waste { get; set; } = 0;
        //============================================================================================================================  
        public double  Price { get; set; } = 0;
        public double  TotalPrice { get; set; } = 0;
        public double  RequiredPrice { get; set; } = 0;
        public double  LeftOverPrice { get; set; } = 0;
        //============================================================================================================================  
        public double  SquareMeterPrice { get; set; } = 0;
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
        [Required]  public MaterialType MaterialType { get; set; } = 0;
        [Required]  public WorksheetType WorksheetType { get; set; } = 0;
        //============================================================================================================================
        [MaxLength(255)] public string? SourceReference { get; set; }
        [MaxLength(255)] public string? SourceDescription { get; set; }
        [MaxLength(255)] public string? SourceColor { get; set; }
        [MaxLength(255)] public string? SourceColorDescription { get; set; }

    }
}

