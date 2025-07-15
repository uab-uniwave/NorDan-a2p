/****** Object:  Table [dbo].[Uniwave_a2p_ReferenceMappingLog]    Script Date: 2025-06-26 22:28:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_ReferenceMappingLog]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_ReferenceMappingLog]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_Materials]    Script Date: 2025-06-26 22:28:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_Materials]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_Materials]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_Items]    Script Date: 2025-06-26 22:28:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_Items]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_Items]
GO

/****** Object:  Table [dbo].[NorDan_a2p_IntrastatData]    Script Date: 2025-06-26 22:28:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NorDan_a2p_IntrastatData]') AND type in (N'U'))
DROP TABLE [dbo].[NorDan_a2p_IntrastatData]
GO

/****** Object:  Table [dbo].[NorDan_a2p_ColorMapping]    Script Date: 2025-06-26 22:28:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NorDan_a2p_ColorMapping]') AND type in (N'U'))
DROP TABLE [dbo].[NorDan_a2p_ColorMapping]
GO

/****** Object:  Table [dbo].[NorDan_a2p_ColorMapping]    Script Date: 2025-06-26 22:28:59 ******/
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
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Weight] [float] NULL,
	[WeightWithoutGlass] [float] NULL,
	[WeightGlass] [float] NULL,
	[TotalWeight] [float] NULL,
	[TotalWeightWithoutGlass] [float] NULL,
	[TotalWeightGlass] [float] NULL,
	[Area] [float] NULL,
	[TotalArea] [float] NULL,
	[Hours] [float] NULL,
	[TotalHours] [float] NULL,
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
	[Id] [int] IDENTITY(1,1) NOT NULL,
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
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Quantity] [int] NOT NULL,
	[PackageQuantity] [float] NULL,
	[TotalQuantity] [float] NULL,
	[RequiredQuantity] [float] NOT NULL,
	[LeftOverQuantity] [float] NULL,
	[Weight] [float] NULL,
	[TotalWeight] [float] NULL,
	[RequiredWeight] [float] NULL,
	[LeftOverWeight] [float] NULL,
	[Area] [float] NULL,
	[TotalArea] [float] NULL,
	[RequiredArea] [float] NULL,
	[LeftOverArea] [float] NULL,
	[Waste] [float] NULL,
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


