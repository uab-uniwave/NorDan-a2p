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
        public SourceAppType SourceAppType { get; set; } = Enums.SourceAppType.Unknown;
        //===================================================================================

        public int ItemCount { get; set; } = 0; //Number of  positions. Single Position can have quantity > 1

        public int ErrorCount { get; set; } = 0; //Number of errors for this order

        public int TotalQuantity { get; set; } = 0; //Quantity of all items , sum of quantities of all positions

        public decimal TotalWeight { get; set; } = 0;  //Weight of all items , sum of Weight of all positions
        public decimal TotalWeightWithoutGlass { get; set; } = 0; //Weight of all items without glass , sum of WeightWithoutGlass of all positions
        public decimal TotalWeightGlass { get; set; } = 0; //Weight of all glass , sum of WeightGlass of all positions
        public decimal TotalArea { get; set; } = 0; //Area of all items , sum of Area of all positions
        public decimal TotalHours { get; set; } = 0; //Hours of all items , sum of Hours of all positions
        public decimal TotalMaterialCost { get; set; } = 0; //Total Material Cost of all items , sum of MaterialCost of all positions
        public decimal TotalLaborCost { get; set; } = 0; //Total Labor Cost of all items , sum of LaborCost of all positions
        public decimal TotalCost { get; set; } = 0; //Total Cost of all items , sum of Cost of all positions
        public decimal TotalPrice { get; set; } = 0; //Total Price of all items , sum of Price of all positions
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