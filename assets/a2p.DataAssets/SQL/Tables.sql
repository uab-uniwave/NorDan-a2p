


/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_Materials]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_Materials]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_Items]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_Items]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NorDan_a2p_ColorMapping]') AND type in (N'U'))
DROP TABLE [dbo].[NorDan_a2p_ColorMapping]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NorDan_a2p_Order]') AND type in (N'U'))
DROP TABLE [dbo].[NorDan_a2p_ColorMapping]
GO
*/


/* 

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NorDan_a2p_ColorMapping](
	[SapaLogicColor] [nvarchar](50) NOT NULL,
	[TechDesignColor] [nvarchar](50) NOT NULL,
	[SurfaceTreatmentGroup] [nvarchar](50) NOT NULL,
	[OutputText] [nvarchar](50) NOT NULL,
	[RGB] [nvarchar](50) NOT NULL,
	[DescriptionSE] [nvarchar](50) NOT NULL,
	[DescriptionNO] [nvarchar](50) NOT NULL,
	[DescriptionDK] [nvarchar](50) NOT NULL,
	[DescriptionFI] [nvarchar](50) NOT NULL,
	[DescriptionEN] [nvarchar](50) NOT NULL,
	[DescriptionPL] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[NorDan_a2p_IntrastatData](
	[Material] [nvarchar](255) NULL,
	[Surface Treatment#] [nvarchar](255) NULL,
	[Length] [decimal](18, 6) NULL,
	[Material Description] [nvarchar](255) NULL,
	[Material Type] [nvarchar](255) NULL,
	[Material Group] [nvarchar](255) NULL,
	[Default commodity code] [nvarchar](255) NULL,
	[Base Unit of Measure] [nvarchar](255) NULL,
	[Sales Organization] [nvarchar](255) NULL,
	[Distribution Channel] [nvarchar](255) NULL,
	[Weight] [decimal](18, 6) NULL,
	[Weight alu] [decimal](18, 6) NULL,
	[DChain-spec# status] [nvarchar](255) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Uniwave_a2p_Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SalesDocumentNumber] [int] NOT NULL,
	[SalesDocumentVersion] [int] NOT NULL,
	[SalesDocumentIdPos] [uniqueidentifier] NOT NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Worksheet] [nvarchar](255) NOT NULL,
	[Line] [int] NOT NULL,
	[Column] [int] NOT NULL,
	[Project] [nvarchar](50) NULL,
	[Item] [nvarchar](25) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Quantity] [int] NOT NULL,
	[Width] decimal(38,6) NULL,
	[Height] decimal(38,6) NULL,
	[Weight] decimal(38,6) NULL,
	[WeightWithoutGlass] decimal(38,6) NULL,
	[WeightGlass] decimal(38,6) NULL,
	[TotalWeight] decimal(38,6) NULL,
	[TotalWeightWithoutGlass] decimal(38,6) NULL,
	[TotalWeightGlass] decimal(38,6) NULL,
	[Area] decimal(38,6) NULL,
	[TotalArea] decimal(38,6) NULL,
	[Hours] decimal(38,6) NULL,
	[TotalHours] decimal(38,6) NULL,
	[MaterialCost] [decimal](38, 6) NULL,
	[LaborCost] [decimal](38, 6) NULL,
	[Cost] [decimal](38, 6) NULL,
	[TotalMaterialCost] [decimal](38, 6) NULL,
	[TotalLaborCost] [decimal](38, 6) NULL,
	[TotalCost] [decimal](38, 6) NULL,
	[Price] [decimal](38, 6) NULL,
	[TotalPrice] [decimal](38, 6) NULL,
	[CurrencyCode] [nvarchar](10) NULL,
	[ExchangeRateEUR] [decimal](18, 4) NULL,
	[MaterialCostEUR] [decimal](38, 6) NULL,
	[LaborCostEUR] [decimal](38, 6) NULL,
	[CostEUR] [decimal](38, 6) NULL,
	[TotalMaterialCostEUR] [decimal](38, 6) NULL,
	[TotalLaborCostEUR] [decimal](38, 6) NULL,
	[TotalCostEUR] [decimal](38, 6) NULL,
	[PriceEUR] [decimal](38, 6) NULL,
	[TotalPriceEUR] [decimal](38, 6) NULL,
	[WorksheetType] [int] NOT NULL,
	[CreatedUTCDateTime] [datetime] NOT NULL,
	[ModifiedUTCDateTime] [datetime] NOT NULL,
	[DeletedUTCDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Uniwave_a2p_Materials](
	[RowId] [uniqeidentifier] IDENTITY(1,1) NOT NULL,
	[SalesDocumentNumber] [int] NOT NULL,
	[SalesDocumentVersion] [int] NOT NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Worksheet] [nvarchar](255) NOT NULL,
	[Line] [int] NOT NULL,
	[Column] [int] NOT NULL,
	[Item] [nvarchar](25) NULL,
	[SortOrder] [int] NULL,
	[ReferenceBase] [nvarchar](25) NOT NULL,
	[Reference] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Color] [nvarchar](50) NOT NULL,
	[ColorDescription] [nvarchar](120) NULL,
	[Width] decimal(38,6) NULL,
	[Height] decimal(38,6) NULL,
	[Quantity] [int] NOT NULL,
	[PackageQuantity] decimal(38,6) NULL,
	[TotalQuantity] decimal(38,6) NULL,
	[RequiredQuantity] decimal(38,6) NOT NULL,
	[LeftOverQuantity] decimal(38,6) NULL,
	[Weight] decimal(38,6) NULL,
	[TotalWeight] decimal(38,6) NULL,
	[RequiredWeight] decimal(38,6) NULL,
	[LeftOverWeight] decimal(38,6) NULL,
	[Area] decimal(38,6) NULL,
	[TotalArea] decimal(38,6) NULL,
	[RequiredArea] decimal(38,6) NULL,
	[LeftOverArea] decimal(38,6) NULL,
	[Waste] decimal(38,6) NULL,
	[Price] [decimal](38, 6) NULL,
	[TotalPrice] [decimal](38, 6) NULL,
	[RequiredPrice] [decimal](38, 6) NULL,
	[LeftOverPrice] [decimal](38, 6) NULL,
	[SquareMeterPrice] [decimal](38, 6) NULL,
	[Pallet] [nvarchar](255) NULL,
	[CustomField1] [nvarchar](255) NULL,
	[CustomField2] [nvarchar](255) NULL,
	[CustomField3] [nvarchar](255) NULL,
	[CustomField4] [nvarchar](255) NULL,
	[CustomField5] [nvarchar](255) NULL,
	[MaterialType] [int] NOT NULL,
	[WorksheetType] [int] NOT NULL,
	[SourceReference] [nvarchar](255) NULL,
	[SourceDescription] [nvarchar](255) NULL,
	[SourceColor] [nvarchar](255) NULL,
	[SourceColorDescription] [nvarchar](255) NULL,
	[CreatedUTCDateTime] [datetime] NOT NULL,
	[ModifiedUTCDateTime] [datetime] NOT NULL,
	[DeletedUTCDateTime] [datetime] NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Uniwave_a2p_ReferenceMappingLog](
	[ReferenceBase] [nvarchar](25) NULL,
	[Reference] [nvarchar](25) NULL,
	[SourceReference] [nvarchar](50) NULL,
	[SourceColor] [nvarchar](50) NULL,
	[SourceColor1] [nvarchar](50) NULL,
	[SourceColor2] [nchar](10) NULL,
	[SapaReferenceBase] [nvarchar](25) NULL,
	[SapaReference] [nvarchar](25) NULL,
	[SapaColor] [nvarchar](50) NULL,
	[SapaColor1] [nvarchar](50) NULL,
	[SapaColor2] [nvarchar](50) NULL,
	[ExternalReference] [nvarchar](50) NULL
) ON [PRIMARY]
GO


GO


CREATE TABLE [dbo].[Uniwave_a2p_Order]
(
    -- Primary Key
    [RowId]                    UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),

    -- Core Order Information
    [OrderNumber]              NVARCHAR(50)     NOT NULL,
    [OrderDate]                DATETIME2(0)     NOT NULL,
    [CustomerTitle]            NVARCHAR(200)    NULL,
    [CustomerNumber]           NVARCHAR(50)     NULL,
    [ProjectNumber]            NVARCHAR(50)     NULL,
    [DeliveryAddress]          NVARCHAR(500)    NULL,
    [CorrectionAvailableUntil] DATETIME2(0)     NULL,
    [ResponsibleManager]       NVARCHAR(150)    NULL,

    [SalesDocumentNumber]      INT              NULL,
    [SalesDocumentVersion]     INT              NULL,

    -- Calculated / Aggregate Values
    [ItemCount]                INT              NOT NULL DEFAULT 0,
    [TotalQuantity]            DECIMAL(18,3)    NOT NULL DEFAULT 0,
    [TotalWeight]              DECIMAL(18,3)    NOT NULL DEFAULT 0,
    [TotalWeightWithoutGlass]  DECIMAL(18,3)    NOT NULL DEFAULT 0,
    [TotalWeightGlass]         DECIMAL(18,3)    NOT NULL DEFAULT 0,
    [TotalArea]                DECIMAL(18,3)    NOT NULL DEFAULT 0,
    [TotalHours]               DECIMAL(18,2)    NOT NULL DEFAULT 0,
    [TotalMaterialCost]        DECIMAL(18,2)    NOT NULL DEFAULT 0,
    [TotalLaborCost]           DECIMAL(18,2)    NOT NULL DEFAULT 0,
    [TotalCost]                DECIMAL(18,2)    NOT NULL DEFAULT 0,
    [TotalPrice]               DECIMAL(18,2)    NOT NULL DEFAULT 0,
    [CurrencyCode]             CHAR(3)          NOT NULL DEFAULT 'EUR',
    [ExchangeRate]             DECIMAL(18,6)    NULL,
    [ExchangeRateDate]         DATE             NULL,

    -- Audit Columns (UTC and User Tracking)
    [CreatedUTCDateTime]       DATETIME2(3)     NOT NULL DEFAULT SYSUTCDATETIME(),
    [ModifiedUTCDateTime]      DATETIME2(3)     NOT NULL DEFAULT SYSUTCDATETIME(),
    [CreatedBy]                NVARCHAR(100)    NULL,
    [ModifiedBy]               NVARCHAR(100)    NULL,

    CONSTRAINT [PK_Uniwave_a2p_Order] 
        PRIMARY KEY CLUSTERED ([RowId]),

    CONSTRAINT [UQ_Uniwave_a2p_Order_SalesDocNumber_Version] 
        UNIQUE ([SalesDocumentNumber], [SalesDocumentVersion])
);
GO

-- Automatically update ModifiedUTCDateTime on updates
CREATE TRIGGER [dbo].[TR_Uniwave_a2p_Order_UpdateModifiedUTC]
ON [dbo].[Uniwave_a2p_Order]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET 
        o.[ModifiedUTCDateTime] = SYSUTCDATETIME(),
        o.[ModifiedBy] = COALESCE(i.[ModifiedBy], o.[ModifiedBy])
    FROM [dbo].[Uniwave_a2p_Order] o
    INNER JOIN inserted i ON o.[RowId] = i.[RowId];
END;
GO
CREATE INDEX IX_Uniwave_a2p_Order_OrderNumber ON [dbo].[Uniwave_a2p_Order]([OrderNumber]);
CREATE INDEX IX_Uniwave_a2p_Order_CustomerNumber ON [dbo].[Uniwave_a2p_Order]([CustomerNumber]);
CREATE INDEX IX_Uniwave_a2p_Order_ProjectNumber ON [dbo].[Uniwave_a2p_Order]([ProjectNumber]);
CREATE INDEX IX_Uniwave_a2p_Order_OrderDate ON [dbo].[Uniwave_a2p_Order]([OrderDate]);