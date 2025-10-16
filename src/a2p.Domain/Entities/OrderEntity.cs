// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Domain.Enums;


namespace a2p.Domain.Entities
{
    public class OrderEntity : BaseEntity
    {

        //===============================================
        public string OrderNumber { get; set; } = string.Empty;
        public string? ProjectNumber { get; set; }
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;
        public DateOnly? OrderDate { get; set; }

        public string CustomerTitle { get; set; } = string.Empty;
        public string CustomerNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public DateOnly? CorrectionAvailableUnitil { get; set; }

        public string ResponsibleManager { get; set; } = string.Empty;

        //===================================================================================
        public Wo SourceAppType { get; set; } = Enums.Wo.Unknown;
        //===================================================================================

        public int ItemCount { get; set; } = 0;
        public int MaterialCount { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;
        public int TotalQuantity { get; set; } = 0;

        public Decimal TotalUnits { get; set; }

        public Decimal TotalWeight { get; set; } = 0;
        public Decimal TotalWeightWithoutGlass { get; set; } = 0;
        public Decimal TotalWeightGlass { get; set; } = 0;
        public Decimal TotalArea { get; set; } = 0;
        public Decimal TotalHours { get; set; } = 0;
        public Decimal TotalMaterialCost { get; set; } = 0;
        public Decimal TotalLaborCost { get; set; } = 0;
        public Decimal TotalCost { get; set; } = 0;
        public Decimal TotalPrice { get; set; } = 0;
        public string? Currency { get; set; } //3 char ISO code
        public double ExchangeRate { get; set; }
        public DateOnly ExchangeRateDate { get; set; }
        //======================================================= ============================

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