USE [Prefsuite]
GO

/****** Object:  Table [dbo].[Schueco_RecordsPositions]    Script Date: 2024-12-29 21:14:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schueco_RecordsPositions]') AND type in (N'U'))
DROP TABLE [dbo].[Schueco_RecordsPositions]
GO

/****** Object:  Table [dbo].[Schueco_RecordsMaterials]    Script Date: 2024-12-29 21:14:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schueco_RecordsMaterials]') AND type in (N'U'))
DROP TABLE [dbo].[Schueco_RecordsMaterials]
GO

/****** Object:  Table [dbo].[Schueco_RecordsGlasses]    Script Date: 2024-12-29 21:14:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schueco_RecordsGlasses]') AND type in (N'U'))
DROP TABLE [dbo].[Schueco_RecordsGlasses]
GO

/****** Object:  Table [dbo].[Schueco_RecordsGlasses]    Script Date: 2024-12-29 21:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Schueco_RecordsGlasses](
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
 CONSTRAINT [PK_Schueco_RecordsGlasses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Schueco_RecordsMaterials]    Script Date: 2024-12-29 21:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Schueco_RecordsMaterials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[File] [nvarchar](255) NULL,
	[Project] [nvarchar](255) NULL,
	[ArticleNo] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Color] [nvarchar](255) NULL,
	[TotalPrice] [float] NULL,
	[QuantityEA] [float] NULL,
	[Quantity] [int] NULL,
	[Delivery] [nvarchar](255) NULL,
	[Dimensions] [nvarchar](255) NULL,
	[Weight] [float] NULL,
	[InsertDate] [datetime] NULL,
	[DocNumber] [int] NULL,
	[DocVersion] [int] NULL,
 CONSTRAINT [PK_Schueco_RecordsMaterials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Schueco_RecordsPositions]    Script Date: 2024-12-29 21:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Schueco_RecordsPositions](
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
 CONSTRAINT [PK_Schueco_PositionsRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


