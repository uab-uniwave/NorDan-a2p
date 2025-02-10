
SET ANSI_nullS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Materials Insert
--==================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_InsertMaterial]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_InsertMaterial]
GO


CREATE PROCEDURE [dbo].[Uniwave_a2p_InsertMaterial] 
      @SalesDocumentNumber [int]
    , @SalesDocumentVersion [int]
    --==============================
    , @Order [nvarchar] (50)
    , @Worksheet [nvarchar] (255)
    , @Line [int]
    , @Column [int] null
    --==============================
    , @Item [nvarchar] (25) null
    , @SortOrder [int] null
    --==============================
    , @Reference [nvarchar] (25)
    , @Description [nvarchar] (255) null
    --==============================
    , @Color [nvarchar] (50) null
    , @ColorDescription [nvarchar] (120) null
    --==============================
    , @Width [float] null
    , @Height [float] null
    --==============================
    , @Quantity [int]
    , @PackageQuantity [float] null
    , @TotalQuantity [float] null
    , @RequiredQuantity [float]
    , @LeftOverQuantity [float] null
    --==============================
    , @Weight [float] null
    , @TotalWeight [float] null
    , @RequiredWeight [float] null
    , @LeftOverWeight [float] null
    --==============================  
    , @Area [float] null
    , @TotalArea [float] null
    , @RequiredArea [float] null
    , @LeftOverArea [float] null
    --==============================      
    , @Waste [float] null
    --==============================  
    , @Price [decimal] (38 , 6) null
    , @TotalPrice [decimal] (38 , 6) null
    , @RequiredPrice [decimal] (38 , 6) null
    , @LeftOverPrice [decimal] (38 , 6) null
    --==============================      
    , @SquareMeterPrice [decimal] (38 , 6) null
    --==============================  
    , @Pallet [nvarchar] (255) null
    --==============================  
    , @CustomField1 [nvarchar] (255) null
    , @CustomField2 [nvarchar] (255) null
    , @CustomField3 [nvarchar] (255) null
    --==============================  
    , @CustomField4 [nvarchar] (255) null
    , @CustomField5 [nvarchar] (255) null
    --==============================  
    , @MaterialType  [int]
    --==============================  
    , @WorksheetType [int]
    --==============================  
    , @SourceReference [nvarchar] (255) null
    , @SourceDescription [nvarchar] (255) null
    , @SourceColor [nvarchar] (255) null
    , @SourceColorDescription [nvarchar] (255) null
    --==============================  
    , @CreatedUTCDateTime [datetime]
    , @ModifiedUTCDateTime [datetime]
AS
BEGIN
    INSERT INTO [dbo].[Uniwave_a2p_Materials] (
          [SalesDocumentNumber]
        , [SalesDocumentVersion]
        --==============================  
        , [Order]
        , [Worksheet]
        , [Line]
        , [Column]
        --==============================  
        , [Item]
        , [SortOrder]
         --==============================  
        , [Reference]
        , [Description]
        --==============================  
        , [Color]
        , [ColorDescription]
        --==============================  
        , [Width]
        , [Height]
        --==============================  
        , [Quantity]
        , [PackageQuantity]
        , [TotalQuantity]
        , [RequiredQuantity]
        , [LeftOverQuantity]
        --==============================  
        , [Weight]
        , [TotalWeight]
        , [RequiredWeight]
        , [LeftOverWeight]
        --==============================  
        , [Area]
        , [TotalArea]
        , [RequiredArea]
        , [LeftOverArea]
        --==============================  
        , [Waste]
        --==============================  
        , [Price]
        , [TotalPrice]
        , [RequiredPrice]
        , [LeftOverPrice]
        --==============================  
        , [SquareMeterPrice]
        --==============================  
        , [Pallet]
        --==============================  
        , [CustomField1]
        , [CustomField2]
        , [CustomField3]
        --==============================  
        , [CustomField4]
        , [CustomField5]
        --==============================  
        , [MaterialType]
        --==============================        
        , [WorksheetType]
        --==============================
        , [SourceReference]
        , [SourceDescription]
        , [SourceColor]
        , [SourceColorDescription]
        --==============================  
        , [CreatedUTCDateTime]
        , [ModifiedUTCDateTime]
        )
    VALUES (
          @SalesDocumentNumber
        , @SalesDocumentVersion      
        --==============================  
        , @Order
        , @Worksheet
        , @Line
        , @Column
        --==============================  
        , @Item
        , @SortOrder
        , @Reference
        , @Description
        --==============================  
        , @Color
        , @ColorDescription
        --==============================  
        , @Width
        , @Height
        --==============================  
        , @Quantity
        , @PackageQuantity
        , @TotalQuantity
        , @RequiredQuantity
        , @LeftOverQuantity
        --==============================  
        , @Weight
        , @TotalWeight
        , @RequiredWeight
        , @LeftOverWeight
        --==============================  
        , @Area
        , @TotalArea
        , @RequiredArea
        , @LeftOverArea
         
        , @Waste
        --============================== 
        , @Price
        , @TotalPrice
        , @RequiredPrice
        , @LeftOverPrice
        --==============================  
        , @SquareMeterPrice
        --==============================  
        , @Pallet
        --==============================  
        , @CustomField1
        , @CustomField2
        , @CustomField3
        --==============================  
        , @CustomField4
        , @CustomField5
        --==============================  
        , @MaterialType
        --==============================  
        , @WorksheetType
        --==============================  
        , @SourceReference
        , @SourceDescription
        , @SourceColor
        , @SourceColorDescription
        --==============================
        , @CreatedUTCDateTime
        , @ModifiedUTCDateTime
        );
END;
GO




--Materials Delete
--==================================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_DeleteMaterials]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_DeleteMaterials]
GO
CREATE PROCEDURE [dbo].[Uniwave_a2p_DeleteMaterials]
    @Order [nvarchar](50),
    @DeletedUTCDateTime [datetime]  

AS
BEGIN
    UPDATE [dbo].[Uniwave_a2p_Materials] SET @DeletedUTCDateTime = GETDATE() WHERE [Order] = @Order and [DeletedUTCDateTime] is null;
END;
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_MaterialsExists]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_MaterialsExists]
GO


--Materials Exists
--==================================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_MaterialsExists]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_MaterialsExists]
GO
CREATE PROCEDURE [dbo].[Uniwave_a2p_MaterialsExists]
    @Order [nvarchar](50),
    @ModifiedUTCDateTime DateTime OUTPUT
AS
BEGIN
    SET @ModifiedUTCDateTime = null;

    IF EXISTS (SELECT MAX(ModifiedUTCDateTime) FROM [dbo].[Uniwave_a2p_Materials] WHERE [Order] = @Order AND [DeletedUTCDateTime] IS null)
    BEGIN
        SET @ModifiedUTCDateTime = (SELECT MAX(ModifiedUTCDateTime) FROM [dbo].[Uniwave_a2p_Materials] WHERE [Order] = @Order AND [DeletedUTCDateTime] IS null)
    END
END;
GO


--Items Insert
--==================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_InsertItem]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_InsertItem]
GO

CREATE PROCEDURE [dbo].[Uniwave_a2p_InsertItem]
    @SalesDocumentNumber [int],
    @SalesDocumentVersion [int],
    --==================  
    @Order [nvarchar](255),
    @Worksheet [nvarchar](255),
    @Line [int] null,
    @Column [int] null,
    --==================
    @Project [nvarchar](60) null,
    --==================  
    @Item [nvarchar](50) null,
    @SortOrder [int],
    @Description [nvarchar](255) null,
    --==================  
    @Quantity [int],
    --==================  
    @Width [float] null,
    @Height [float] null,
    --==================  
    @Weight [float]  null,
    @WeightWithoutGlass [float] null,
    @WeightGlass [float]  null,
    --==================  
    @TotalWeight [float]  null,
    @TotalWeightWithoutGlass [float] null,
    @TotalWeightGlass [float] null,
    --==================  
    @Area [float]  null,
    @TotalArea [float]  null,
    --==================  
    @Hours [float]  null,
    @TotalHours [float]  null,      
    --==================  
    @MaterialCost [decimal](38, 6) null,
    @LaborCost [decimal](38, 6) null,
    @Cost [decimal](38, 6) null,
    --==================  
    @TotalMaterialCost [decimal](38, 6) null,
    @TotalLaborCost [decimal](38, 6) null,
    @TotalCost [decimal](38, 6) null,
    --==================  
    @Price [decimal](38, 6) null,
    @TotalPrice [decimal](38, 6) null,
    --==================
    @CurrencyCode [nvarchar](50) null,
    @ExchangeRateEUR [decimal](18, 4) null,
    --==================
    @MaterialCostEUR [decimal](38, 6) null,
    @LaborCostEUR [decimal](38, 6) null,
    @CostEUR [decimal](38, 6) null,   
    --==================
    @TotalMaterialCostEUR [decimal](38, 6) null,
    @TotalLaborCostEUR [decimal](38, 6) null,
    @TotalCostEUR [decimal](38, 6) null,
    --==================
    @PriceEUR [decimal](38, 6) null,
    @TotalPriceEUR [decimal](38, 6) null,
    --==================
    @WorksheetType [int],
    --==================
    @CreatedUTCDateTime [dateTime],
    @ModifiedUTCDateTime [dateTime],
    @DeletedUTCDateTime [dateTime] = null
AS
BEGIN
    INSERT INTO [dbo].[Uniwave_a2p_Items] (
        [SalesDocumentNumber],
        [SalesDocumentVersion],
        --==================
        [Order],
        [Worksheet],
        [Line],
        [Column],
        --==================
        [Project],
        --==================
        [Item],
        [SortOrder],
        [Description],
        --==================
        [Quantity],
        --==================
        [Width],
        [Height],
        --==================
        [Weight],
        [WeightWithoutGlass],
        [WeightGlass],
        --==================
        [TotalWeight],
        [TotalWeightWithoutGlass],
        [TotalWeightGlass],
        --==================
        [Area],
        [TotalArea],
        --==================
        [Hours],
        [TotalHours],
        --==================
        [MaterialCost],
        [LaborCost],
        [Cost],
        --==================
        [TotalMaterialCost],
        [TotalLaborCost],
        [TotalCost],
        --==================
        [Price],
        [TotalPrice],
       --==================
        [CurrencyCode],
        [ExchangeRateEUR],      
       --==================
        [MaterialCostEUR],
        [LaborCostEUR],
        [CostEUR],
       --==================
        [TotalMaterialCostEUR],
        [TotalLaborCostEUR],
        [TotalCostEUR],
       --==================
        [PriceEUR],
        [TotalPriceEUR],       
       --==================
        [WorksheetType],
       --==================
        [CreatedUTCDateTime],
        [ModifiedUTCDateTime],
        [DeletedUTCDateTime]
    )
    VALUES (
        @SalesDocumentNumber,
        @SalesDocumentVersion,
        --==================
        @Order,
        @Worksheet,
        @Line,
        @Column,
        --==================
        @Project,
        @Item,
        @SortOrder,
        @Description,
        --==================
        @Quantity,
        --==================
        @Width,
        @Height,
        --==================
        @Weight,
        @WeightWithoutGlass,
        @WeightGlass,       
        --==================
        @TotalWeight,
        @TotalWeightWithoutGlass,
        @TotalWeightGlass,
        --==================
        @Area,
        @TotalArea,
        --==================
        @Hours,
        @TotalHours,
        --==================      
        @MaterialCost,
        @LaborCost,
        @Cost,
        --==================
        @TotalMaterialCost,
        @TotalLaborCost,
        @TotalCost,
         --==================               
        @Price,
        @TotalPrice,
        --==================
        @CurrencyCode,
        @ExchangeRateEUR,
        --==================
        @MaterialCostEUR,
        @LaborCostEUR,
        @CostEUR,
        --==================
        @TotalMaterialCostEUR,
        @TotalLaborCostEUR,
        @TotalCostEUR,
        --==================
        @PriceEUR,
        @TotalPriceEUR,
        --==================
        @WorksheetType,
        --==================
        @CreatedUTCDateTime,
        @ModifiedUTCDateTime,
        @DeletedUTCDateTime
    );
END;

GO
--Items Delete
--==================================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_DeleteItems]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_DeleteItems]
GO

--Items Delte
--==================================================================================
CREATE PROCEDURE [dbo].[Uniwave_a2p_DeleteItems]
    @Order [nvarchar](50),
    @DeletedUTCDateTime [datetime]  

AS
BEGIN
    UPDATE [dbo].[Uniwave_a2p_Items] SET @DeletedUTCDateTime = GETDATE() WHERE [Order] = @Order and [DeletedUTCDateTime] is null;
END;
GO


--Items Exists
--==================================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_ItemsExists]') AND type in (N'P'))
DROP PROCEDURE [dbo].[Uniwave_a2p_ItemsExists]
GO
CREATE PROCEDURE [dbo].[Uniwave_a2p_ItemsExists]
    @Order [nvarchar](50),
    @ModifiedUTCDateTime DateTime OUTPUT
AS
BEGIN
    SET @ModifiedUTCDateTime = null;

    IF EXISTS (SELECT MAX(ModifiedUTCDateTime) FROM [dbo].[Uniwave_a2p_Materials] WHERE [Order] = @Order AND [DeletedUTCDateTime] IS null)
    BEGIN
        SET @ModifiedUTCDateTime = (SELECT MAX(ModifiedUTCDateTime) FROM [dbo].[Uniwave_a2p_Materials] WHERE [Order] = @Order AND [DeletedUTCDateTime] IS null)
    END
END;
GO

