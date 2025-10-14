// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Domain.Enums;

namespace a2p.Domain.Entities
{
    public class MaterialEntity : BaseEntity
    {
        public Guid OrderId { get; set; } = Guid.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string Worksheet { get; set; } = string.Empty;
        public int Line { get; set; } = -1;
        public int Column { get; set; } = -1;
        //============================================================================================================================
        public string? ItemName { get; set; } = string.Empty;

        public Guid? ItemId { get; set; } = null;

        public int SortOrder { get; set; } = -1;
        //============================================================================================================================
        public string ReferenceBase { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string? Description { get; set; }
        //============================================================================================================================
        public string Color { get; set; } = string.Empty;
        public string ColorDescription { get; set; } = null;
        //============================================================================================================================
        public decimal Width { get; set; } = 0m;
        public decimal Height { get; set; } = 0m;
        //============================================================================================================================       
        public int Quantity { get; set; } = 0;
        public decimal PackageQuantity { get; set; } = 0m;
        public decimal TotalQuantity { get; set; } = 0m;
        public decimal RequiredQuantity { get; set; } = 0m;
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
        public string? Pallet { get; set; } = null;
        //============================================================================================================================
        public string? CustomField1 { get; set; } = null;
        public string? CustomField2 { get; set; } = null;
        public string? CustomField3 { get; set; } = null;
        //============================================================================================================================
        public string? CustomField4 { get; set; } = null;
        public string? CustomField5 { get; set; } = null;
        //============================================================================================================================
        public MaterialType MaterialType { get; set; } = 0;
        //============================================================================================================================
        public string SourceReference { get; set; } = string.Empty;
        public string SourceDescription { get; set; } = string.Empty;
        public string SourceColor { get; set; } = string.Empty;
        public string? SourceColorDescription { get; set; } = string.Empty;
        //============================================================================================================================
        public int? CommodityCode { get; set; } = null;

    }
}

