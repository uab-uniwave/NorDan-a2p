﻿/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_DeleteExistingData]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_DeleteExistingData]
	@SalesDocumentNumber int, 
	@SalesDocumentVersion int
AS
BEGIN
	
	SET NOCOUNT ON;
	UPDATE Uniwave_a2p_Items Set DeletedUTCDateTime = GETDATE() Where SalesDocumentNumber = @SalesDocumentNumber  and SalesDocumentVersion = @SalesDocumentVersion
	UPDATE Uniwave_a2p_Materials Set DeletedUTCDateTime = GETDATE() Where SalesDocumentNumber = @SalesDocumentNumber  and SalesDocumentVersion = @SalesDocumentVersion
	DELETE FROM ContenidoPAF Where Numero = @SalesDocumentNumber  and [Version] = @SalesDocumentVersion
	DELETE FROM MaterialNeeds  Where Number = @SalesDocumentNumber  and [Version] = @SalesDocumentVersion
	DELETE FROM MaterialNeedsMaster Where Number = @SalesDocumentNumber  and [Version] = @SalesDocumentVersion
END


GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertItem]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertItem]
	@SalesDocumentNumber [int],
	@SalesDocumentVersion [int],
	@SalesDocumentIdPos [uniqueidentifier],
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
		[SalesDocumentIdPos],
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
		@SalesDocumentIdPos,
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

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertMaterial]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertMaterial] 
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
	, @ReferenceBase [nvarchar] (25)
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
		, [ReferenceBase]
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
		, @ReferenceBase
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

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteColor]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteColor] 
	
	@Color nvarchar(50), 
	@ColorDescription nvarchar (120)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	/*Check color, if not exists than insert*/
	/*======================================*/
	IF NOT EXISTS (SELECT * FROM Colores WHERE Nombre = @Color)
	BEGIN
	INSERT INTO dbo.Colores
	(
		MakerId,
		RowId,
		Nombre,
		RGB,
		Numero,
		Nivel1,
		Nivel2,
		Nivel3,
		Nivel4,
		Nivel5,
		Decoracion,
		DesAuto,
		DesProd,
		AmbientRed,
		AmbientGreen,
		AmbientBlue,
		DiffuseRed,
		DiffuseGreen,
		DiffuseBlue,
		SpecularRed,
		SpecularGreen,
		SpecularBlue,
		Transparency,
		Texture,
		AngleTexture,
		TextureScaleX,
		TextureScaleY,
		Family,
		FamilyOrder,
		BasicRawMaterial,
		RawMaterial,
		Image,
		Generico,
		Material,
		Description,
		InnerAllowed,
		OuterAllowed,
		RuleGenerator,
		CustomTariffCalculation,
		Pattern,
		Standard,
		EffectivePerimeterIgnored,
		ColorTypeCode,
		Alpha,
		Render3DMaterial,
		InnerColorEditable,
		OuterColorEditable
	)
	VALUES
	(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', 
		NEWID(), -- RowId - uniqueidentifier
		@Color,  -- Nombre - nchar(50)
		16777215,    -- RGB - int
		0,    -- Numero - smallint
		'988 SAPA',  -- Nivel1 - nvarchar(150)
		NULL,  -- Nivel2 - nvarchar(150)
		NULL,  -- Nivel3 - nvarchar(150)
		NULL,  -- Nivel4 - nvarchar(150)
		NULL,  -- Nivel5 - nvarchar(150)
		(SELECT Decoracion FROM Colores WHERE Nombre = 'White') , -- Decoracion - image
		' *' ,  -- DesAuto - nvarchar(120)
		N'',  -- DesProd - nvarchar(120)
		0.0,  -- AmbientRed - float
		0.0,  -- AmbientGreen - float
		0.0,  -- AmbientBlue - float
		0.99609375,  -- DiffuseRed - float
		0.99609375,  -- DiffuseGreen - float
		0.99609375,  -- DiffuseBlue - float
		0.0,  -- SpecularRed - float
		0.0,  -- SpecularGreen - float
		0.0,  -- SpecularBlue - float
		1,  -- Transparency - float
		NULL, -- Texture - image
		0.0,  -- AngleTexture - float
		0.0,  -- TextureScaleX - float
		0.0,  -- TextureScaleY - float
		N'_SAPA_V2',  -- Family - nchar(25)
		0,    -- FamilyOrder - int
		N'',  -- BasicRawMaterial - nchar(25)
		0,    -- RawMaterial - int
		NULL, -- Image - image
		1,    -- Generico - smallint
		N'',  -- Material - nchar(25)
		@ColorDescription,  -- Description - nvarchar(120)
		1,    -- InnerAllowed - smallint
		1,    -- OuterAllowed - smallint
		1,    -- RuleGenerator - smallint
		0,    -- CustomTariffCalculation - smallint
		NULL,  -- Pattern - nchar(50)
		0,    -- Standard - smallint
		0,    -- EffectivePerimeterIgnored - smallint
		NULL,    -- ColorTypeCode - smallint
		0.0,  -- Alpha - float
		NULL,    -- Render3DMaterial - int
		0,    -- InnerColorEditable - smallint
		0     -- OuterColorEditable - smallint
		)
	END 

END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration] 
	
	@Color nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	/*Check color, if not exists than insert*/
	/*======================================*/
	

	IF NOT EXISTS (SELECT * FROM ColorConfigurations WHERE ColorName = @Color)
	INSERT INTO dbo.ColorConfigurations
	(
		ConfigurationCode,
		ColorName,
		InnerColor,
		OuterColor
	)
	VALUES
	(   (SELECT MAX(ConfigurationCode)+1 FROM dbo.ColorConfigurations),   -- ConfigurationCode - int
		@Color, -- ColorName - nvarchar(50)
		NULL, -- InnerColor - nvarchar(50)
		NULL  -- OuterColor - nvarchar(50)
		)

END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterial]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE[dbo].[Uniwave_a2p_InsertPrefSuiteMaterial] 
	@ReferenceBase NVARCHAR(25),
	@Reference NVARCHAR(25),
	@Color NVARCHAR(50),
	@PackageQuantity FLOAT,
	@Weight FLOAT,
	@MaterialType INT
	

AS
BEGIN




	IF NOT EXISTS (SELECT * FROM dbo.Materiales WHERE Referencia = @Reference)
	INSERT INTO dbo.Materiales
	(
		MakerId,
		RowId,
		ReferenciaBase,
		Referencia,
		Color,
		Almacen,
		UE1,
		UE2,
		ControlDeStock,
		PedirBajoDemanda,
		ManageRemnants,
		LongitudBarra,
		WastageAllowance,
		UseWastageAllowanceInMN,
		UseFullRodsInMN,
		IsModel,
		TargetLevel,
		PrefShopStatus,
		DefaultValue,
		MaterialSupplierCode,
		ProductionPreparationTime,
		AverageDeliveryTime
	)
	VALUES
	(   'AE8D70E6-C414-412A-B272-AE141FCFA63F',      -- MakerId - uniqueidentifier
		NEWID(),      -- RowId - uniqueidentifier
		@ReferenceBase,       -- ReferenciaBase - nchar(25)
		@Reference,       -- Referencia - nchar(25)
		@Color,       -- Color - nchar(50)
		988,         -- Almacen - smallint
		1,         -- UE1 - int
		@PackageQuantity,         -- UE2 - int
		1,         -- ControlDeStock - smallint
		1,         -- PedirBajoDemanda - smallint
		0,         -- ManageRemnants - smallint
		Case When @MaterialType = 1   -- LongitudBarra - real
		THEN @PackageQuantity*1000
		ELSE 0
		END,
		0.000000,      -- WastageAllowance - double (19, 6)
		0,         -- UseWastageAllowanceInMN - smallint
		0,         -- UseFullRodsInMN - smallint
		0,         -- IsModel - smallint
		1,         -- TargetLevel - int
		0,         -- PrefShopStatus - smallint
		0,         -- DefaultValue - smallint
		988,         -- MaterialSupplierCode - int
		1,         -- ProductionPreparationTime - int
		14         -- AverageDeliveryTime - smallint
		)

END	
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase] 
	@ReferenceBase NVARCHAR(25),
	@Description NVARCHAR(255),
	@MaterialType INT
	

AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT * FROM dbo.MaterialesBase WHERE ReferenciaBase = @ReferenceBase)
	INSERT INTO dbo.MaterialesBase
	(
		MakerId,
		RowId,
		ReferenciaBase,
		Descripcion,
		TipoCalculo,
		Nivel1,
		Nivel2,
		CodigoProveedor,
		NoIncluirEnHojaDeTrabajo,
		NoOptimizar,
		NoIncluirEnMaterialNeeds,
		OrdenPrecioKg,
		IdGrupoPresupuestado,
		IdGrupoProduccion,
		OrdenDesAuto,
		OrdenDesProd,
		OrdenOptimizacion,
		Valorador,
		IsFrameFitting,
		Role,
		WorkPlace,
		ConditionalWorkPlace,
		StockInWorkPlace,
		CustomTariffCalculation,
		DoNotShowInMonitors,
		DoNotShowInTree,
		Area,
		InnerColorPerimeter,
		OuterColorPerimeter,
		InsertionPointX,
		InsertionPointY,
		ShowIn3D,
		ShowIn2DInner,
		ShowIn2DOuter,
		MaterialSide,
		IsDummy,
		IsTransparent,
		ColorControl,
		UnMountable,
		MountedDefaultState,
		PackedQuantity,
		PackedUnitsType,
		PriceBookLevel,
		PrefShopStatus,
		DontIncludeInMaterialReport
	  
	)
	VALUES
	(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		NEWID(), -- RowId - uniqueidentifier
		@ReferenceBase,  -- ReferenciaBase - nchar(25) 
		@Description,	    
			CASE WHEN @MaterialType = 1 THEN 'Barras'
			 WHEN @MaterialType = 2 THEN 'Metros'
			 WHEN @MaterialType = 3 THEN 'Piezas'
			 WHEN @MaterialType = 4 THEN 'Superficies'
			 WHEN @MaterialType =  5 THEN 'Superficies'
		ELSE 'Piezas' END,  -- TipoCalculo - nchar(15)
		N'988 SAPA',  -- Nivel1 - nvarchar(150)
		CASE WHEN @MaterialType = 1 THEN 'Barras'
			 WHEN @MaterialType = 2 THEN 'Metros'
			 WHEN @MaterialType = 3 THEN 'Piezas'
			 WHEN @MaterialType = 4 THEN 'Superficies'
			 WHEN @MaterialType =  5 THEN 'Superficies'
		ELSE 'Piezas' END,  -- TipoCalculo - nchar(15)
		988,    -- CodigoProveedor - int
		0,    -- NoIncluirEnHojaDeTrabajo - smallint
		0,    -- NoOptimizar - smallint
		2,    -- NoIncluirEnMaterialNeeds - smallint
		0,    -- OrdenPrecioKg - smallint
		0,    -- IdGrupoPresupuestado - smallint
		0,    -- IdGrupoProduccion - smallint
		0,    -- OrdenDesAuto - smallint
		0,    -- OrdenDesProd - smallint
		0,    -- OrdenOptimizacion - smallint
		0,    -- Valorador - smallint
		0,    -- IsFrameFitting - smallint
		CASE WHEN @MaterialType = 5 THEN 'Glass' --Role,
			 ELSE 'Unknown' END,
		0,    -- WorkPlace - smallint
		0,    -- ConditionalWorkPlace - smallint
		0,    -- StockInWorkPlace - smallint
		0,    -- CustomTariffCalculation - smallint
		0,    -- DoNotShowInMonitors - smallint
		0,    -- DoNotShowInTree - smallint
		0.0,  -- Area - float
		0.0,  -- InnerColorPerimeter - float
		0.0,  -- OuterColorPerimeter - float
		0.0,  -- InsertionPointX - float
		0.0,  -- InsertionPointY - float
		1,    -- ShowIn3D - smallint
		1,    -- ShowIn2DInner - smallint
		0,    -- ShowIn2DOuter - smallint
		0,    -- MaterialSide - smallint
		0,    -- IsDummy - smallint
		0,    -- IsTransparent - smallint
		-1,    -- ColorControl - smallint
		0,    -- UnMountable - smallint
		0,    -- MountedDefaultState - smallint
		0.0,  -- PackedQuantity - float
		0,    -- PackedUnitsType - smallint
		0,    -- PriceBookLevel - smallint
		0,    -- PrefShopStatus - smallint
		0    -- DontIncludeInMaterialReport - smallint
		)
END
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialMeter]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialMeter] 

	@ReferenceBase NVARCHAR(25),
	@Weight FLOAT


AS
BEGIN
	SET NOCOUNT ON;

	
	BEGIN
	IF NOT EXISTS (SELECT * FROM dbo.Metros WHERE ReferenciaBase = @ReferenceBase)
	INSERT INTO dbo.Metros
	(
		MakerId,
		ReferenciaBase,
		PriceUnitsType,
		LinearWeightKg_m,
		LossType,
		CustomLoss
	)
	VALUES
	(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		@ReferenceBase,  -- ReferenciaBase - nchar(25)
		2,    -- PriceUnitsType - smallint
		@Weight,  -- LinearWeightKg_m - real
		0,    -- LossType - smallint
		0.0   -- CustomLoss - float
		)
	END
	END
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeeds]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeeds] 
	-- Add the parameters for the stored procedure here
	@Number INT, 
	@Version  INT


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	
	IF NOT EXISTS (SELECT * FROM MaterialNeeds Where  Number=@Number AND Version=@Version)
	INSERT INTO dbo.MaterialNeeds
	(
		GUID,
		Number,
		Version,
		ProductionSet,
		ReproductionNeedsCode,
		MNSet,
		Position,
		SquareId,
		HoleId,
		ElementId,
		MaterialType,
		Complex,
		Reference,
		ColorConfiguration,
		RawMaterialColorConfiguration,
		RawReference,
		Quantity,
		Length,
		Height,
		Volume,
		ProviderCode,
		WarehouseCode,
		XMLDoc,
		AllowToOrder,
		QuantityToOrder,
		QuantityToDiscount,
		DiscountedQuantity,
		ReservedQuantity,
		IsCopy,
		FromNumber,
		FromVersion,
		TargetLevel,
		Unmounted,
		ProductTypeCode,
		CustomLengthType,
		DeltaQuantity,
		OrderComponents,
		Weight
	)
SELECT
		NEWID(), -- GUID - uniqueidentifier
		SalesDocumentNumber,    -- Number - int
		SalesDocumentVersion,    -- Version - int
		-1,    -- ProductionSet - int
		-1,    -- ReproductionNeedsCode - int
		 1,    -- MNSet - smallint
		-1,    -- Position - int
		-1,    -- SquareId - int
		-1,    -- HoleId - int
		CASE WHEN MaterialType = '5' THEN 'G'+RTRIM(CAST(SortOrder AS NVARCHAR(9)))
			 ELSE ''
			 END,  
		CASE WHEN MaterialType = 1 THEN 1
			 WHEN MaterialType = 2 THEN 3
			 WHEN MaterialType = 3 THEN 2
			 WHEN MaterialType = 5 THEN 4
			 WHEN MaterialType = 4 THEN 4
			 WHEN MaterialType = 4 THEN 4
			 END,  -- TipoCalculo - nchar(15),    -- MaterialType - smallint
		0,    -- Complex - smallint
		Reference,  -- Reference - nchar(25)
		(dbo.Uniwave_a2p_GetColorConfiguration(Uniwave_a2p_Materials.Color)),
		0,    -- RawMaterialColorConfiguration - int
		N'',  -- RawReference - nchar(25)
		Quantity,  -- Quantity - float
		Width,  -- Length - real
		Height,  -- Height - real
		0.0,  -- Volume - real
		ISNULL((SELECT TOP 1 MaterialSupplierCode FROM dbo.Materiales WHERE Referencia = Reference),988),  -- ProviderCode - int
		ISNULL((SELECT TOP 1 Almacen FROM dbo.Materiales WHERE Referencia = Reference),988),  -- WarehouseCode - smallint
		 N'',  -- XMLDoc - ntext
		RequiredQuantity,    -- AllowToOrder - smallint
		TotalQuantity,  -- QuantityToOrder - float
		RequiredQuantity,  -- QuantityToDiscount - float
		0.0,  -- DiscountedQuantity - float
		0.0,  -- ReservedQuantity - float
		0,    -- IsCopy - smallint
		0,    -- FromNumber - int
		0,    -- FromVersion - int
		0,    -- TargetLevel - int
		0,    -- Unmounted - smallint
		0,    -- ProductTypeCode - int
		0,    -- CustomLengthType - smallint
		0.0,  -- DeltaQuantity - float
		0,    -- OrderComponents - smallint
		TotalWeight   -- Weight - float
	 
FROM Uniwave_a2p_Materials Where SalesDocumentNumber = @Number and SalesDocumentVersion =@Version and DeletedUTCDateTime is null



END
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeedsMaster]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeedsMaster]
	@Number int, 
	@Version int
AS
BEGIN
DELETE FROM dbo.MaterialNeedsMaster	Where Number=@Number and [Version] =@Version
INSERT INTO dbo.MaterialNeedsMaster
(
	[dbo].[Number],
	[dbo].[Version],
	[dbo].[ProductionSet],
	[dbo].[ReproductionNeedsCode],
	[dbo].[MNSet],
	[dbo].[CalculationDate],
	[dbo].[Obsolete],
	[dbo].[Description],
	[dbo].[Discounted],
	[dbo].[TypeMNSet],
	[dbo].[ComponentsAssemblyUTCDate],
	[dbo].[CalculationUTCDate]
)
 VALUES (@Number,         -- Number - int
		@Version,         -- Version - int
	   -1,         -- ProductionSet - int
	   -1,         -- ReproductionNeedsCode - int
	   1,         -- MNSet - smallint
	   GETDATE(), -- CalculationDate - datetime
	   0,         -- Obsolete - smallint
	   N'1.- ' +CAST (GETDATE() AS NVARCHAR(16)) , -- Description - nvarchar(50)
	   0,         -- Discounted - smallint
	   1,         -- TypeMNSet - smallint
	  NULL, -- ComponentsAssemblyUTCDate - datetime
	  GETUTCDATE()  -- CalculationUTCDate - datetime
	  )
END
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPiece]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPiece] 
	-- Add the parameters for the stored procedure here
	@ReferenceBase NVARCHAR(25),
	@Weight FLOAT

	

AS
	BEGIN
	IF NOT EXISTS (SELECT * FROM dbo.Piezas WHERE ReferenciaBase = @ReferenceBase)
	INSERT INTO dbo.Piezas
		(
			MakerId,
			ReferenciaBase,
			UnitWeightKg
		)
		VALUES
		(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
			@ReferenceBase,  -- ReferenciaBase - nchar(25)
			@Weight    -- UnitWeightKg - real
			)
	END

GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile] 
	-- Add the parameters for the stored procedure here
	@ReferenceBase NVARCHAR(25),
	@PackageQuantity FLOAT,
	@Weight FLOAT
	

AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
	IF NOT EXISTS (SELECT * FROM dbo.Perfiles WHERE ReferenciaBase = @ReferenceBase)
	INSERT INTO dbo.Perfiles
	(
		MakerId,
		ReferenciaBase,
		LongitudBarra,
		PesoLineal,
		AnchoExterior,
		AnchoInterior,
		Altura,
		CuerpoInterior,
		PerimetroSeccion,
		CuerpoExterior,
		Soldable,
		Divisible,
		Torsion,
		InerciaX,
		InerciaY,
		InertiaXY,
		Structural,
		ShearAreaX,
		ShearAreaY,
		ModulusOfElasticityX,
		ModulusOfElasticityY,
		LongestLength,
		LongestThickness,
		SigmaMax,
		SigmaMin,
		TurnRadioX,
		TurnRadioY,
		InnerFaceOffset,
		OuterFaceOffset,
		MinWidth,
		ForgedLevel,
		Wing,
		MirrorHorizontalForMachining,
		MirrorVerticalForMachining,
		RotationForMachining,
		PriceUnitsType,
		AutoDivisible,
		Turnable,
		GenerateSquare,
		FixedInnerFaceName,
		FixedOuterFaceName,
		BendingMachineLoss,
		ExteriorSnapinMuntin,
		BottomMarginForFullRod,
		AngleCut,
		MullionCorneringType,
		Composite,
		OrderComponents,
		TimeOptimization,
		WeightPriceCalculation,
		PaintPriceCalculation
	)
	VALUES
	(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		@ReferenceBase,  -- ReferenciaBase - nchar(25)
		@PackageQuantity*1000,  -- LongitudBarra - real
		@Weight,
		1,  -- AnchoExterior - real
		0.0,  -- AnchoInterior - real
		1,     -- Altura - real
		0.0,  -- CuerpoInterior - real
		0.0,  -- PerimetroSeccion - real
		0.0,  -- CuerpoExterior - real
		0,    -- Soldable - int
		0,    -- Divisible - int
		0.0,  -- Torsion - float
		0.0,  -- InerciaX - float
		0.0,  -- InerciaY - float
		0.0,  -- InertiaXY - float
		0,    -- Structural - smallint
		0.0,  -- ShearAreaX - float
		0.0,  -- ShearAreaY - float
		0.0,  -- ModulusOfElasticityX - float
		0.0,  -- ModulusOfElasticityY - float
		0.0,  -- LongestLength - float
		0.0,  -- LongestThickness - float
		0.0,  -- SigmaMax - float
		0.0,  -- SigmaMin - float
		0.0,  -- TurnRadioX - float
		0.0,  -- TurnRadioY - float
		0.0,  -- InnerFaceOffset - real
		0.0,  -- OuterFaceOffset - real
		0.0,  -- MinWidth - real
		0,    -- ForgedLevel - smallint
		0.0,  -- Wing - float
		0,    -- MirrorHorizontalForMachining - smallint
		0,    -- MirrorVerticalForMachining - smallint
		0.0,  -- RotationForMachining - float
		0,    -- PriceUnitsType - smallint
		0,    -- AutoDivisible - smallint
		0,    -- Turnable - smallint
		0,    -- GenerateSquare - smallint
		0,    -- FixedInnerFaceName - smallint
		0,    -- FixedOuterFaceName - smallint
		0.0,  -- BendingMachineLoss - float
		0,    -- ExteriorSnapinMuntin - smallint
		0.0,  -- BottomMarginForFullRod - real
		0,    -- AngleCut - smallint
		0,    -- MullionCorneringType - smallint
		0,    -- Composite - smallint
		0,    -- OrderComponents - smallint
		0,    -- TimeOptimization - smallint
		0,    -- WeightPriceCalculation - smallint
		0     -- PaintPriceCalculation - smallint
		)
	END
	
END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface] 
	-- Add the parameters for the stored procedure here
	@ReferenceBase NVARCHAR(25),
	@Weight FLOAT,
	@MaterialType INT
	

AS
BEGIN
	BEGIN
	IF NOT EXISTS (SELECT * FROM dbo.Superficies WHERE ReferenciaBase = @ReferenceBase)
	INSERT INTO dbo.Superficies
	(
		MakerId,
		ReferenciaBase,
		MultiploVertical,
		MultiploHorizontal,
		PesoSuperficial,
		Espesor,
		MinimoM2,
		DescuentoBarrotillo,
		Tipo,
		AltoPanel,
		AnchoPanel,
		Tabla,
		Composite,
		HasDirection,
		Turnable,
		Mirrorable,
		MinimumWidth,
		MinimumHeight,
		MinArea,
		MaximumWidth,
		MaximumHeight,
		MaxArea,
		ProportionalFactorNum,
		ProportionalFactorDen,
		KFactor,
		GFactor,
		AcousticFactor,
		LightTransFactor,
		PsiFactor,
		UFactor,
		Offset,
		PricingAfterMatrixLine,
		Tempered,
		Gas,
		PriceUnitsType,
		MaximumWeight,
		LowEmissive,
		AcousticCFactor,
		AcousticCtrFactor,
		ThermalConductivity,
		AllowInternalGeorgianBar,
		AllowExternalGeorgianBar,
		SubType
		
	)
	VALUES
	(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		@ReferenceBase,  -- ReferenciaBase - nchar(25)
		0.0,  -- MultiploVertical - float
		0.0,  -- MultiploHorizontal - float
		@weight,  -- PesoSuperficial - real
		1,  -- Espesor - real
		0.0,  -- MinimoM2 - float
		0.0,  -- DescuentoBarrotillo - float
		CASE WHEN @MaterialType = 5 THEN 0 ELSE 2 END,
		0.0,  -- AltoPanel - float
		0.0,  -- AnchoPanel - float
		0,    -- Tabla - smallint
		0,    -- Composite - smallint
		0,    -- HasDirection - smallint
		0,    -- Turnable - smallint
		0,    -- Mirrorable - smallint
		0.0,  -- MinimumWidth - float
		0.0,  -- MinimumHeight - float
		0.0,  -- MinArea - float
		0.0,  -- MaximumWidth - float
		0.0,  -- MaximumHeight - float
		0.0,  -- MaxArea - float
		0,    -- ProportionalFactorNum - int
		0,    -- ProportionalFactorDen - int
		0.0,  -- KFactor - float
		0.0,  -- GFactor - float
		0.0,  -- AcousticFactor - float
		0.0,  -- LightTransFactor - float
		0.0,  -- PsiFactor - float
		0.0,  -- UFactor - float
		0.0,  -- Offset - float
		0,    -- PricingAfterMatrixLine - smallint
		0,    -- Tempered - smallint
		N'',  -- Gas - nchar(25)
		0,    -- PriceUnitsType - smallint
		0.0,  -- MaximumWeight - float
		0,    -- LowEmissive - smallint
		0.0,  -- AcousticCFactor - float
		0.0,  -- AcousticCtrFactor - float
		0.0,  -- ThermalConductivity - float
		0,    -- AllowInternalGeorgianBar - smallint
		0,    -- AllowExternalGeorgianBar - smallint
		0    -- SubType - smallint
		)
	END
	
END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertSapaProvider]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertSapaProvider] 
	-- Add the parameters for the stored procedure here
	@Code int = 988,
	@Name NVARCHAR(60) =  N'SAPA V2',
	@Currency NVARCHAR(25) = N'NOK'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	IF NOT EXISTS(SELECT * FROM dbo.Proveedores WHERE CodigoProveedor = @Code)
	INSERT INTO dbo.Proveedores
	(
		RowId,
		CodigoProveedor,
		Nombre,
		Divisa,
		Divisa2
		
	)
	VALUES
	(   NEWID(),      -- RowId - uniqueidentifier
		@Code,         -- CodigoProveedor - int
		@Name,       -- Nombre - nvarchar(60)
		@Currency,       -- Divisa - nchar(25)
		@Currency       -- Divisa2 - nchar(25)
		)
END

GO

/****** Object:  StoredProcedure [dbo].[Uniwave_a2p_InsertStock]    Script Date: 6/7/2025 2:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE[dbo].[Uniwave_a2p_InsertStock] 
	-- Add the parameters for the stored procedure here
	@Code INT =988, 
	@ProviderCode  INT = 988


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		IF NOT EXISTS (SELECT * FROM dbo.Almacenes WHERE Codigo =@Code)
		INSERT INTO dbo.Almacenes
		(
			Codigo,
			Descripcion,
			Externo,
			ProviderCode,
			Address,
			Address2,
			City,
			PostalCode,
			County,
			Country,
			UsedInMRP,
			Kind
		)
		VALUES
		(   @Code,   -- Codigo - smallint
			N'SAPA V2', -- Descripcion - nvarchar(60)
			0,   -- Externo - smallint
			@ProviderCode,   -- ProviderCode - int
			N'', -- Address - nvarchar(60)
			N'', -- Address2 - nvarchar(60)
			N'', -- City - nvarchar(60)
			N'', -- PostalCode - nvarchar(25)
			N'', -- County - nvarchar(60)
			N'', -- Country - nvarchar(50)
			1,   -- UsedInMRP - smallint
			0    -- Kind - smallint
			)
END
GO


