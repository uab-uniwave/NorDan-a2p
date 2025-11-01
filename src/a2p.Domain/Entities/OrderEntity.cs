// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Domain.Enums;

namespace Domain.Entities
{
    public class OrderEntity : BaseEntity
    {

        //===============================================
        public string? OrderNumber { get; set; }
        public string? ProjectNumber { get; set; }
        public int SalesDocumentNumber { get; set; }
        public int SalesDocumentVersion { get; set; }
        public DateTime? OrderDate { get; set; }

        public string? CustomerTitle { get; set; }
        public string? CustomerNumber { get; set; }
        public string? DeliveryAddress { get; set; }
        public DateTime? CorrectionAvailableUntil { get; set; }

        public string? ResponsibleManager { get; set; }

        //===================================================================================
        public SourceAppType SourceAppType { get; set; } = SourceAppType.Unknown;
        //===================================================================================

        public List<ItemEntity> Items { get; set; } = new List<ItemEntity>();
        public List<MaterialEntity> Materials { get; set; } = new List<MaterialEntity>();
        public string? Currency { get; set; } //3 char ISO code 
        public double? ExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        //===================================================================================

    }
}

/*
 * USE [PrefSuite_NorDan_Development]
GO

SELECT [Id]
 ,[OrderNumber]
 ,[OrderDate]
 ,[CustomerTitle]
 ,[CustomerNumber]
 ,[ProjectNumber]
 ,[DeliveryAddress]
 ,[CorrectionAvailableUnitil]
 ,[ResponsibleManager]
 ,[SalesDocumentNumber]
 ,[SalesDocumentVersion]
 FROM [dbo].[Uniwave_a2p_Order]

GO


 */