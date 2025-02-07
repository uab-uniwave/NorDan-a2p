

/****** Object:  Table [dbo].[Uniwave_a2p_RecordsPositions]    Script Date: 2024-12-29 21:14:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_RecordsPositions]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_RecordsPositions]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_RecordsMaterials]    Script Date: 2024-12-29 21:14:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_RecordsMaterials]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_RecordsMaterials]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_RecordsGlasses]    Script Date: 2024-12-29 21:14:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_RecordsGlasses]') AND type in (N'U'))
DROP TABLE [dbo].[Uniwave_a2p_RecordsGlasses]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_RecordsGlasses]    Script Date: 2024-12-29 21:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Uniwave_a2p_RecordsGlasses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Project] [nvarchar](255) NULL,
	[Item] [nvarchar](255) NULL,
	[Field] [int] NULL,
	[Quantity] [int] NULL,
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Weight] [float] NULL,
	[Price] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[InsertDate] [datetime] NULL,
 CONSTRAINT [PK_Uniwave_a2p_RecordsGlasses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_RecordsMaterials]    Script Date: 2024-12-29 21:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Uniwave_a2p_RecordsMaterials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [SalesDocumentNumber] [int] NOT NULL,
    [SalesDocumentVersion] [int] NOT NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Reference] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Color] [nvarchar](50) NULL,
    [ColorDescription] [nvarchar](120) NULL,
    [Quantity] [int] NOT NULL,
    [PackageUnit] [float] NULL,
	[Price] [decimal] NULL,
    [TotalPrice] [decimal] NULL,
    [QuantityOrdered] [float] NULL,
    [QuantityRequired] [float] NULL,
    [Waste] [float]  NULL,
    [Area] [float] NULL,
    [Weight] [float] NULL,
    [CustomField1] [nvarchar](255) NULL,
    [CustomField2] [nvarchar](255) NULL,
    [CustomField3] [nvarchar](255) NULL,
    [WorksheetName] [nvarchar](255) NULL,
    [SourceReference] [nvarchar](255) NULL,
    [SourceDescription] [nvarchar](255) NULL,
	[SoureColor] [nvarchar](255) NULL,
    [SourceType] [nvarchar](30) NULL,
	[InsertDate] [datetime] NULL,
 CONSTRAINT [PK_Uniwave_a2p_RecordsMaterials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Uniwave_a2p_RecordsPositions]    Script Date: 2024-12-29 21:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Uniwave_a2p_RecordsPositions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectNumber] [nvarchar](255) NULL,
	[ProjectName] [nvarchar](255) NULL,
	[ItemNumber] [nvarchar](255) NULL,
	[ItemDescription] [nvarchar](255) NULL,
	[ProfileSystem] [nvarchar](255) NULL,
	[Quantity] [int] NULL,
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Weight] [float] NULL,
	[WeightGlass] [float] NULL,
	[Price] [float] NULL,
	[Number] [int] NULL,
	[Version] [int] NULL,
	[SortOrder] [int] NULL,
	[InsertDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[TotalLabour] [float] NULL,
 CONSTRAINT [PK_Uniwave_a2p_PositionsRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


