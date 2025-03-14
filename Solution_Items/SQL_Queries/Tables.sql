    GO
    SET ANSI_nullS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO

    -- Materials Table
    --==================================================================================
    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_Materials]') AND type in (N'U'))
    DROP TABLE [dbo].[Uniwave_a2p_Materials]

    CREATE TABLE [dbo].[Uniwave_a2p_Materials] (
        [Id]                     [int]             identity (1, 1) not null,
        --==================================================================================
        [SalesDocumentNumber]    [int]              not null,
        [SalesDocumentVersion]   [int]              not null,
       --===================================================================================
        [Order]                  [nvarchar] (50)    not null,
        [Worksheet]              [nvarchar] (255)   not null,
        [Line]                   [int]              not null,
        [Column]                 [int]              not null,
        --==================================================================================
        [Item]                   [nvarchar] (25)    null, 
        [SortOrder]              [int]              null,
        --==================================================================================
        [ReferenceBase]              [nvarchar] (25)    not null,
        [Reference]              [nvarchar] (25)    not null,
        [Description]            [nvarchar] (255)   null,
        [Color]                  [nvarchar] (50)    not null,
        [ColorDescription]       [nvarchar] (120)   null,
        --==================================================================================
        [Width]                  [float] (53)       null,
        [Height]                 [float] (53)       null,
        --==================================================================================
        [Quantity]               [int]              not null,
        [PackageQuantity]        [float] (53)       null,
        [TotalQuantity]          [float] (53)       null,
        [RequiredQuantity]       [float] (53)       not null,
        [LeftOverQuantity]       [float] (53)       null,
        --==================================================================================
        [Weight]                 [float] (53)       null,
        [TotalWeight]            [float] (53)       null,
        [RequiredWeight]         [float] (53)       null,
        [LeftOverWeight]         [float] (53)       null,
        --==================================================================================
        [Area]                   [float] (53)       null,
        [TotalArea]              [float] (53)       null,
        [RequiredArea]           [float] (53)       null,
        [LeftOverArea]           [float] (53)       null,
        --==================================================================================
        [Waste]                  [float] (53)       null,
        --==================================================================================
        [Price]                  [decimal] (38, 6)  null,
        [TotalPrice]             [decimal] (38, 6)  null,
        [RequiredPrice]          [decimal] (38, 6)  null,
        [LeftOverPrice]          [decimal] (38, 6)  null,
        --==================================================================================
        [SquareMeterPrice]       [decimal] (38, 6)  null,
        --==================================================================================
        [Pallet]                 [nvarchar] (255)   null,
         --==================================================================================
        [CustomField1]           [nvarchar] (255)   null,
        [CustomField2]           [nvarchar] (255)   null,
        [CustomField3]           [nvarchar] (255)   null,
        --==================================================================================
        [CustomField4]           [nvarchar] (255)   null,
        [CustomField5]           [nvarchar] (255)   null,    
       --==================================================================================
        [MaterialType]           [int]              not null,
        --==================================================================================
        [WorksheetType]          [int]              not null,
        --==================================================================================
        [SourceReference]        [nvarchar] (255)   null,
        [SourceDescription]      [nvarchar] (255)   null,
        [SourceColor]            [nvarchar] (255)   null,
        [SourceColorDescription] [nvarchar] (255)   null,
        --==================================================================================
        [CreatedUTCDateTime]     [dateTime]         not null,
        [ModifiedUTCDateTime]    [dateTime]         not null,
        [DeletedUTCDateTime]     [dateTime]         null
    );


    GO
    CREATE CLUSTERED INDEX [IX_Uniwave_a2p_Materials_Order]
        ON [dbo].[Uniwave_a2p_Materials]([Order] ASC);


    GO
    CREATE NONCLUSTERED INDEX [IX_Uniwave_a2p_Materials_SalesDocument]
        ON [dbo].[Uniwave_a2p_Materials]([SalesDocumentVersion] ASC, [SalesDocumentNumber] ASC)
        INCLUDE([ReferenceBase],[Reference], [Worksheet], [Line], [Order]);

    GO


    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Uniwave_a2p_Items]') AND type in (N'U'))
    DROP TABLE [dbo].[Uniwave_a2p_Items]
    -- Items Table
    --==================================================================================
    CREATE TABLE [dbo].[Uniwave_a2p_Items] (
        [Id]                        [int]               identity (1, 1) not null,
        --==================================================================================
        [SalesDocumentNumber]       [int]               not null,
        [SalesDocumentVersion]      [int]               not null,
        [SalesDocumentIdPos]        [uniqueidentifier]  not null,
        --==================================================================================
        [Order]                     [nvarchar] (50)   not null,
        [Worksheet]                 [nvarchar] (255)  not null,
        [Line]                      [int]             not null,
        [Column]                    [int]             not null,
        --==================================================================================
        [Project]                   [nvarchar] (50)   null,
        [Item]                      [nvarchar] (25)   not null,
        [SortOrder]                 [int]             not null,
        [Description]               [nvarchar] (255)  null,
        --==================================================================================
        [Quantity]                  [int]             not null,
        --==================================================================================
        [Width]                     [float] (53)      null,
        [Height]                    [float] (53)      null,
        --==================================================================================
        [Weight]                    [float] (53)      null,
        [WeightWithoutGlass]        [float] (53)      null,
        [WeightGlass]               [float] (53)      null,
        --==================================================================================
        [TotalWeight]               [float] (53)      null,
        [TotalWeightWithoutGlass]   [float] (53)      null,
        [TotalWeightGlass]          [float] (53)      null,
        --==================================================================================
        [Area]                      [float] (53)      null,
        [TotalArea]                 [float] (53)      null,
        --==================================================================================    
        [Hours]                     [float] (53)      null,
        [TotalHours]                [float] (53)      null,
        --==================================================================================    
        [MaterialCost]              [decimal] (38, 6) null,
        [LaborCost]                 [decimal] (38, 6) null,
        [Cost]                      [decimal] (38, 6) null,
        --==================================================================================    
        [TotalMaterialCost]         [decimal] (38, 6) null,
        [TotalLaborCost]            [decimal] (38, 6) null,
        [TotalCost]                 [decimal] (38, 6) null,
        --==================================================================================    
        [Price]                     [decimal] (38, 6) null,
        [TotalPrice]                [decimal] (38, 6) null,
        --==================================================================================    
        [CurrencyCode]              [nvarchar] (10)   null,
        [ExchangeRateEUR]           [decimal] (18, 4) null,
        --==================================================================================    
        [MaterialCostEUR]           [decimal] (38, 6) null,
        [LaborCostEUR]              [decimal] (38, 6) null,
        [CostEUR]                   [decimal] (38, 6) null,
        --==================================================================================    
        [TotalMaterialCostEUR]      [decimal] (38, 6) null,
        [TotalLaborCostEUR]         [decimal] (38, 6) null,
        [TotalCostEUR]              [decimal] (38, 6) null,
        --==================================================================================    
        [PriceEUR]                  [decimal] (38, 6) null,
        [TotalPriceEUR]             [decimal] (38, 6) null,
        --==================================================================================    
        [WorksheetType]             [int]             not null,
        --==================================================================================    
        [CreatedUTCDateTime]        [dateTime]        not null,
        [ModifiedUTCDateTime]       [dateTime]        not null,
        [DeletedUTCDateTime]        [dateTime]        null,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );


    GO
    CREATE NONCLUSTERED INDEX [IX_Uniwave_a2p_Order]
        ON [dbo].[Uniwave_a2p_Items]([Order] ASC)
        INCLUDE([DeletedUTCDateTime]);


    GO
    CREATE NONCLUSTERED INDEX [Index_Uniwave_a2p_SalesDoc]
        ON [dbo].[Uniwave_a2p_Items]([SalesDocumentNumber] ASC, [SalesDocumentVersion] ASC);

SET ANSI_nullS ON
GO
SET QUOTED_IDENTIFIER ON
GO
