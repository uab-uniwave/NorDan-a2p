// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace a2p.Application.DTO
{
    public class A2PMaterialDto
    {

        [MaxLength(50)] public string Order { get; set; } = string.Empty;
        [MaxLength(255)] public string Worksheet { get; set; } = string.Empty;
        [Required] public int Line { get; set; } = -1;
        [Required] public int Column { get; set; } = -1;
        //============================================================================================================================
        public string? Item { get; set; } = string.Empty;
        public int SortOrder { get; set; } = -1;
        //============================================================================================================================
        [MaxLength(25)] public string ReferenceBase { get; set; } = string.Empty;
        [MaxLength(25)] public string Reference { get; set; } = string.Empty;
        [MaxLength(255)] public string? Description { get; set; }
        //============================================================================================================================
        [MaxLength(50)] public string Color { get; set; } = string.Empty;
        [MaxLength(120)] public string? ColorDescription { get; set; }
        //============================================================================================================================
        public decimal Width { get; set; } = 0m;
        public decimal Height { get; set; } = 0m;
        //============================================================================================================================       
        [Required] public int Quantity { get; set; } = 0;
        public decimal PackageQuantity { get; set; } = 0m;
        public decimal TotalQuantity { get; set; } = 0m;
        [Required] public decimal RequiredQuantity { get; set; } = 0m;
        public decimal LeftOverQuantity { get; set; } = 0m;
        //============================================================================================================================  
        public decimal Weight { get; set; } = 0m;
        public decimal TotalWeight { get; set; } = 0m;
        public decimal RequiredWeight { get; set; } = 0m;
        public decimal LeftOverWeight { get; set; } = 0m;
        //============================================================================================================================  
        public decimal Area { get; set; } = 0m;
        public decimal TotalArea { get; set; } = 0m;
        public decimal RequiredArea { get; set; } = 0m;
        public decimal LeftOverArea { get; set; } = 0m;
        //============================================================================================================================  
        public decimal Waste { get; set; } = 0m;
        //============================================================================================================================  
        public decimal Price { get; set; } = 0m;
        public decimal TotalPrice { get; set; } = 0m;
        public decimal RequiredPrice { get; set; } = 0m;
        public decimal LeftOverPrice { get; set; } = 0m;
        //============================================================================================================================  
        public decimal SquareMeterPrice { get; set; } = 0m;
        //============================================================================================================================`
        [MaxLength(255)] public string? Pallet { get; set; }
        //============================================================================================================================
        [MaxLength(255)] public string? CustomField1 { get; set; }
        [MaxLength(255)] public string? CustomField2 { get; set; }
        [MaxLength(255)] public string? CustomField3 { get; set; }
        //============================================================================================================================
        [MaxLength(255)] public string? CustomField4 { get; set; }
        [MaxLength(255)] public string? CustomField5 { get; set; }
        //============================================================================================================================
        [Required] public MaterialType MaterialType { get; set; } = 0;
        [Required] public WorksheetType WorksheetType { get; set; } = 0;
        //============================================================================================================================
        [MaxLength(255)] public string? SourceReference { get; set; }
        [MaxLength(255)] public string? SourceDescription { get; set; }
        [MaxLength(255)] public string? SourceColor { get; set; }
        [MaxLength(255)] public string? SourceColorDescription { get; set; }
        //============================================================================================================================
        public int? CommodityCode { get; set; }

    }
}

