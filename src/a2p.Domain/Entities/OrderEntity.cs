// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Domain.Enums;


namespace a2p.Domain.Entities
{
    public class OrderEntity : BaseEntity
    {

        //===============================================
        public string OrderNumber { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;
        //===================================================================================
        public SourceAppType SourceAppType { get; set; } = SourceAppType.TechDesign;
        //===================================================================================

        public List<ItemEntity> Items { get; set; } = new();
        public List<MaterialEntity> Materials { get; set; } = new();
        //===================================================================================

        public string? Currency { get; set; }
        public double ExchangeRate { get; set; }
        //===================================================================================

    }
}
