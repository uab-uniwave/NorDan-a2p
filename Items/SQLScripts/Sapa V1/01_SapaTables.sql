USE [Prefsuite]
GO

/****** Object:  Table [dbo].[SAPA_ReferenceAutoDes]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_ReferenceAutoDes]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_ReferenceAutoDes]
GO

/****** Object:  Table [dbo].[SAPA_RecordsPositions]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_RecordsPositions]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_RecordsPositions]
GO

/****** Object:  Table [dbo].[SAPA_RecordsPanels]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_RecordsPanels]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_RecordsPanels]
GO

/****** Object:  Table [dbo].[SAPA_RecordsMaterials]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_RecordsMaterials]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_RecordsMaterials]
GO

/****** Object:  Table [dbo].[SAPA_RecordsGlasses]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_RecordsGlasses]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_RecordsGlasses]
GO

/****** Object:  Table [dbo].[SAPA_OrdersMapping]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_OrdersMapping]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_OrdersMapping]
GO

/****** Object:  Table [dbo].[SAPA_ColorsMapping]    Script Date: 2024-12-29 21:13:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPA_ColorsMapping]') AND type in (N'U'))
DROP TABLE [dbo].[SAPA_ColorsMapping]
GO

/****** Object:  Table [dbo].[SAPA_ColorsMapping]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_ColorsMapping](
	[OldColor] [nvarchar](50) NOT NULL,
	[NewColor] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SAPA_ColorsMapping] PRIMARY KEY CLUSTERED 
(
	[OldColor] ASC,
	[NewColor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SAPA_OrdersMapping]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_OrdersMapping](
	[sFullOrder] [nvarchar](50) NULL,
	[sOrder] [nvarchar](50) NULL,
	[sOrderBase] [nvarchar](50) NULL,
	[sOrderVersion] [nvarchar](50) NULL,
	[pVersion] [nvarchar](50) NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SAPA_RecordsGlasses]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_RecordsGlasses](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[Order] [nvarchar](255) NULL,
	[Id] [nvarchar](255) NULL,
	[LineId] [int] NULL,
	[Product] [nvarchar](255) NULL,
	[ArticleNo] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Quantity] [float] NULL,
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Price] [float] NULL,
	[Weight] [float] NULL,
	[m2pcs] [float] NULL,
	[m2Total] [float] NULL,
	[TotalPrice] [float] NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_SAPA_RecordsGlasses] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SAPA_RecordsMaterials]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_RecordsMaterials](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[WorksheetName] [nvarchar](255) NULL,
	[Order] [nvarchar](255) NULL,
	[ArticleNo] [nvarchar](255) NULL,
	[Color] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Quantity] [int] NULL,
	[Package] [nvarchar](255) NULL,
	[Gross] [float] NULL,
	[Net] [float] NULL,
	[Exchange] [float] NULL,
	[m2] [float] NULL,
	[Weight] [float] NULL,
	[Price] [float] NULL,
	[TotalPrice] [float] NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_SAPA_RecordsMaterials] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SAPA_RecordsPanels]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_RecordsPanels](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[Order] [nvarchar](255) NULL,
	[Id] [nvarchar](255) NULL,
	[LineId] [int] NULL,
	[ArticleNo] [nvarchar](255) NULL,
	[Color] [nvarchar](255) NULL,
	[C1] [nvarchar](255) NULL,
	[C2] [nvarchar](255) NULL,
	[C3] [nvarchar](255) NULL,
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Quantity] [int] NULL,
	[TotalArea] [float] NULL,
	[UsedArea] [float] NULL,
	[TotalWaste] [float] NULL,
	[Price] [float] NULL,
	[CutSpecification] [nvarchar](255) NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_SAPA_RecordsPanels] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SAPA_RecordsPositions]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_RecordsPositions](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[Order] [nvarchar](255) NULL,
	[Phase] [nvarchar](255) NULL,
	[Product] [nvarchar](255) NULL,
	[Quantity] [int] NULL,
	[Width] [float] NULL,
	[Height] [float] NULL,
	[Weight] [float] NULL,
	[WeightWOGlass] [float] NULL,
	[DirectMtrl] [float] NULL,
	[DirectLW] [float] NULL,
	[Price] [float] NULL,
	[Total] [float] NULL,
	[PriceEUR] [float] NULL,
	[TotalEUR] [float] NULL,
	[DirectMtrlEUR] [float] NULL,
	[DirectLWEUR] [float] NULL,
	[Number] [int] NULL,
	[Version] [int] NULL,
	[SortOrder] [int] NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_SAPA_PositionsRecords] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SAPA_ReferenceAutoDes]    Script Date: 2024-12-29 21:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SAPA_ReferenceAutoDes](
	[C1] [nvarchar](25) NOT NULL,
	[C2] [nvarchar](25) NOT NULL,
	[C3] [nvarchar](25) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
	[ColorIndex] [int] NULL,
	[AutoDes] [nvarchar](120) NULL,
 CONSTRAINT [PK_SAPA_Colors] PRIMARY KEY CLUSTERED 
(
	[C1] ASC,
	[C2] ASC,
	[C3] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


