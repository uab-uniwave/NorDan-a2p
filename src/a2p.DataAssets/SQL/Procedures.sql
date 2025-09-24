
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- Insert Provider and Stock 
--==============================================================================
CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertProvider] 
	-- Add the parameters for the stored procedure here
	@Code int = 979,
	@Name NVARCHAR(60) = N'SAPA SWEDEN',
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
	( NEWID(), -- RowId - uniqueidentifier
		@Code,  -- CodigoProveedor - int
		@Name, -- Nombre - nvarchar(60)
		@Currency, -- Divisa - nchar(25)
		@Currency -- Divisa2 - nchar(25)
		)
END
GO

CREATE OR ALTER PROCEDURE[dbo].[Uniwave_a2p_InsertStock] 
	-- Add the parameters for the stored procedure here
	@Code INT =980, 
	@ProviderCode INT = 979


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
		( @Code, -- Codigo - smallint
			N'TechDesign', -- Descripcion - nvarchar(60)
			0, -- Externo - smallint
			@ProviderCode, -- ProviderCode - int
			N'', -- Address - nvarchar(60)
			N'', -- Address2 - nvarchar(60)
			N'', -- City - nvarchar(60)
			N'', -- PostalCode - nvarchar(25)
			N'', -- County - nvarchar(60)
			N'', -- Country - nvarchar(50)
			1, -- UsedInMRP - smallint
			0 -- Kind - smallint
			)
END
GO



-- Insert Order and materials of order into A2P tables
--==============================================================================

CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_DeleteExistingData]
	@SalesDocumentNumber int, 
	@SalesDocumentVersion int,
	@DeleteExisting int 
AS
BEGIN
	
	SET NOCOUNT ON;
	UPDATE Uniwave_a2p_Items Set DeletedUTCDateTime = GETDATE() Where SalesDocumentNumber = @SalesDocumentNumber and SalesDocumentVersion = @SalesDocumentVersion
	UPDATE Uniwave_a2p_Materials Set DeletedUTCDateTime = GETDATE() Where SalesDocumentNumber = @SalesDocumentNumber and SalesDocumentVersion = @SalesDocumentVersion
	
	IF (@DeleteExisting = 1)
	BEGIN
		DELETE FROM ContenidoPAF Where Numero = @SalesDocumentNumber and [Version] = @SalesDocumentVersion
		DELETE FROM MaterialNeeds Where Number = @SalesDocumentNumber and [Version] = @SalesDocumentVersion
		DELETE FROM MaterialNeedsMaster Where Number = @SalesDocumentNumber and [Version] = @SalesDocumentVersion
	END 
	Update vwSales Set BreakdownDate =NULL Where Number=@SalesDocumentNumber and Version=@SalesDocumentVersion
END
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
	@Width  decimal (38,6) null , 
	@Height  decimal (38,6)  null,
	--================== 
	@Weight  decimal (38,6)  null,
	@WeightWithoutGlass  decimal (38,6)  null,
	@WeightGlass  decimal (38,6)  null,
	--================== 
	@TotalWeight  decimal (38,6)  null,
	@TotalWeightWithoutGlass  decimal (38,6)  null,
	@TotalWeightGlass  decimal (38,6)  null,
	--================== 
	@Area  decimal (38,6)  null,
	@TotalArea  decimal (38,6)  null,
	--================== 
	@Hours  decimal (38,6)  null,
	@TotalHours  decimal (38,6)  null, 
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
	, @Width  decimal (38,6)  null
	, @Height  decimal (38,6)  null
	--==============================
	, @Quantity [int]
	, @PackageQuantity  decimal (38,6)  null
	, @TotalQuantity  decimal (38,6)  null
	, @RequiredQuantity decimal(38,6)
	, @LeftOverQuantity  decimal (38,6)  null
	--==============================
	, @Weight  decimal (38,6)  null
	, @TotalWeight  decimal (38,6)  null
	, @RequiredWeight  decimal (38,6)  null
	, @LeftOverWeight  decimal (38,6)  null
	--============================== 
	, @Area  decimal (38,6)  null
	, @TotalArea  decimal (38,6)  null
	, @RequiredArea  decimal (38,6)  null
	, @LeftOverArea  decimal (38,6)  null
	--============================== 
	, @Waste  decimal (38,6)  null
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
	, @MaterialType [int]
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


-- Insert Order Material Needs 
--==============================================================================
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
 VALUES (@Number,  -- Number - int
		@Version,  -- Version - int
	 -1,  -- ProductionSet - int
	 -1,  -- ReproductionNeedsCode - int
	 1,  -- MNSet - smallint
	 GETDATE(), -- CalculationDate - datetime
	 0,  -- Obsolete - smallint
	 N'1.- ' +CAST (GETDATE() AS NVARCHAR(16)) , -- Description - nvarchar(50)
	 0,  -- Discounted - smallint
	 1,  -- TypeMNSet - smallint
	 NULL, -- ComponentsAssemblyUTCDate - datetime
	 GETUTCDATE() -- CalculationUTCDate - datetime
	 )
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeeds] 
	-- Add the parameters for the stored procedure here
	@Number INT, 
	@Version INT


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	
	--IF NOT EXISTS (SELECT * FROM MaterialNeeds Where Number=@Number AND Version=@Version)
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
		NEWID(), -- GUID - uniqueidentifier -- 
		SalesDocumentNumber, -- Number - int -- 
		SalesDocumentVersion, -- Version - int -- 
		-1, -- ProductionSet - int -- 
		-1, -- ReproductionNeedsCode - int -- 
		 1, -- MNSet - smallint -- 
		-1, -- Position - int -- 
		-1, -- SquareId - int -- 
		-1, -- HoleId - int -- 
		CASE WHEN MaterialType = '5' THEN 'G'+RTRIM(CAST(SortOrder AS NVARCHAR(9))) -- 
			 ELSE '' -- 
			 END, -- 
		CASE WHEN MaterialType = 1 THEN 1 -- Profiles - Barras --
			 WHEN MaterialType = 2 THEN 3 -- Gaskets - Metros --
			 WHEN MaterialType = 3 THEN 2 -- Piece - Piezas --
			 WHEN MaterialType = 4 THEN 4 -- Panels - Superficies --
			 WHEN MaterialType = 5 THEN 4 -- Glass - Superficies --	
			 END, -- TipoCalculo - nchar(15), -- MaterialType - smallint -- 
		0, -- Complex - smallint -- 
		Reference, -- Reference - nchar(25) -- 
		(dbo.Uniwave_a2p_GetColorConfiguration(Uniwave_a2p_Materials.Color)), -- colorConfiguration int
		0, -- RawMaterialColorConfiguration - int -- 
		N'', -- RawReference - nchar(25) -- 
		CASE WHEN MaterialType = 1   THEN Quantity  
		     WHEN MaterialType = 2  OR MaterialType=3 THEN CEILING(RequiredQuantity)  
		ELSE TotalQuantity END, --float -- 
		Round(Width,0), -- Length - real -- 
		Round(Height,0), -- Height - real -- 
		0.0, -- Volume - real -- 
		ISNULL((SELECT TOP 1 MaterialSupplierCode FROM dbo.Materiales WHERE Referencia = Reference),979), -- ProviderCode - int -- 
		ISNULL((SELECT TOP 1 Almacen FROM dbo.Materiales WHERE Referencia = Reference),980), -- WarehouseCode - smallint -- 
		 N'', -- XMLDoc - ntext -- 
		
		CASE WHEN (MaterialType = 2  OR MaterialType=3) AND RequiredQuantity <= (SELECT UP2/2 FROM Compras Where Proveedor=979 and APartir=1 and UP1=1 and ByDefault =1 and Referencia = Reference)   THEN 0
		ELSE 1 END, -- AllowToOrder,
		
		

		CASE WHEN MaterialType = 1 THEN Quantity  
		WHEN MaterialType = 2  OR MaterialType=3 THEN CEILING(RequiredQuantity)  
		ELSE TotalQuantity END, 	-- QuantityToOrder
		
		RequiredQuantity, -- QuantityToDiscount,

		
		0.0, -- DiscountedQuantity - float -- 
		0.0, -- ReservedQuantity - float -- 
		0, -- IsCopy - smallint -- 
		0, -- FromNumber - int -- 
		0, -- FromVersion - int -- 
		1, -- TargetLevel - int -- 
		0, -- Unmounted - smallint -- 
		0, -- ProductTypeCode - int -- 
		0, -- CustomLengthType - smallint -- 
		0.0, -- DeltaQuantity - float -- 
		0, -- OrderComponents - smallint -- 
		TotalWeight -- Weight - float -- 
	 
FROM Uniwave_a2p_Materials Where SalesDocumentNumber = @Number and SalesDocumentVersion =@Version and DeletedUTCDateTime is null
Update vwSales Set BreakdownDate =GETDATE() Where Number=@Number and Version=@Version	

END
GO



-- Insert Order Navision codes
--==============================================================================
CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_UpdateBCMapping]
	-- Add the parameters for the stored procedure here
	@ReferenceBase nvarchar(25), 
	@Reference nvarchar(25), 
	@SourceReference nvarchar(50), 
	@SourceColor nvarchar(50), 
	@SourceColor1 nvarchar(50), 
	@SourceColor2 nvarchar(50)
	
AS
BEGIN
Declare @SapaReferenceBase nvarchar(25),
		@SapaReference nvarchar(25),
		@SapaColor nvarchar(50),
		@SapaColor1 nvarchar(50),
		@SapaColor2 nvarchar(50),
		@SapaExternalReference nvarchar(50),
		@ExternalReference nvarchar(50),
		@SapaPrefSuiteRowId uniqueidentifier,
		@PrefSuiteRowId uniqueidentifier,
		@ExternalRowId uniqueidentifier

--SAPA
--==============================================================================================================
IF (LEFT(@SourceReference, 1) = 'S')
BEGIN
	--Get old SAPA ReferenceBase
	--=============================================================
	SET @SapaReferenceBase = 'SAPA_'+SUBSTRING(@SourceReference,2,LEN(@SourceReference))
	SELECT @PrefSuiteRowId = RowId From Materiales Where Referencia =@Reference
	--Get old SAPA Color in case color is complex
 --============================================================================================
	IF (ISNULL(@SourceColor1,'')!='' and ISNULL(@SourceColor,'')!='')
	BEGIN 
			SELECT TOP 1 @SapaColor1 = SapaLogicColor FROM NorDan_a2p_ColorMapping Where TechDesignColor = @SourceColor1
			SELECT TOP 1 @SapaColor2 = SapaLogicColor FROM NorDan_a2p_ColorMapping Where TechDesignColor = @SourceColor2
			SELECT @SapaColor = 'SAPA_' +@SapaColor1+'/'+@SapaColor2
	END
	--Get Old SAPA Color in case color is simple
 --============================================================================================
	ELSE IF (ISNULL(@SourceColor1,'')='' )
	
	BEGIN
	--Source color not empty
	--=======================
		IF(ISNULL(@SourceColor,'') != '')
		BEGIN 
			SELECT TOP 1 @SapaColor = 'SAPA_'+SapaLogicColor FROM NorDan_a2p_ColorMapping Where TechDesignColor = @SourceColor
		END
 
 
	--Source color is empty
	--=======================
		ELSE
		BEGIN
			SET @SapaColor = 'SAPA_NoColor'
		END
	END 
	

 --Get Old SAPA Reference
 --============================================================================================
	SELECT @SapaReference = Referencia From Materiales MT inner Join MaterialesBase MB ON MB.ReferenciaBase = MT.ReferenciaBase and Nivel1 = '980 SAPA'
	Where MB.ReferenciaBase = @SapaReferenceBase and MT.Color =@SapaColor 
	
	--Get External Reference, and RowId 
 --============================================================================================
	SELECT TOP 1 @SapaPrefSuiteRowId = @PrefSuiteRowId, @SapaExternalReference=ExternalReference, @ExternalRowId =ExternalRowId From UniwaveApi_Mapping Where EntityType = 1 and PrefSuiteReference = @SapaReference and PrefSuiteRowId is not null
END


--Not SAPA (for Example ASSA)
--==============================================================================================================--
ELSE 
	BEGIN
	SELECT @SapaReference = Referencia From Materiales MT 
	inner Join MaterialesBase MB ON MB.ReferenciaBase = MT.ReferenciaBase and Nivel1 = '980 SAPA'
	Where MT.Referencia = @Reference
	SELECT TOP 1 @SapaPrefSuiteRowId = PrefSuiteRowId, @SapaExternalReference=ExternalReference, @ExternalRowId =ExternalRowId From UniwaveApi_Mapping Where EntityType = 1 and PrefSuiteReference = @SapaReference and PrefSuiteRowId is not null
END

 DECLARE @Max INT 
	SELECT @Max = MAX(CAST(SUBSTRING(ExternalReference,5,16)AS INT)) FROM UniwaveApi_Mapping Where EntityType = 1 and LEN(ExternalReference)=10
	SET @ExternalReference = 'ALU_'+RTRIM(CAST(@MAx+1 AS NVARCHAR(10))) 


IF (@SapaExternalReference IS NOT NULL AND @Reference != @SapaReference)
BEGIN 
	Insert Into UniwaveApi_Mapping (RowId, ExternalSourceName, EntityType, PrefSuiteRowId, ExternalRowId, PrefSuiteReference, ExternalReference, CreationDate, LastModifiedDate)
VALUES (NewId(), 'BC', 1, @PrefSuiteRowId, @ExternalRowId, @Reference, @SapaExternalReference, GetDAte(), GetDate() )
END 
ELSE IF (@SapaExternalReference IS NULL)
BEGIN 
 
	Insert Into UniwaveApi_Mapping (RowId, ExternalSourceName, EntityType, PrefSuiteRowId, PrefSuiteReference, ExternalReference, CreationDate, LastModifiedDate)
	VALUES (NewId(), 'BC', 1, @PrefSuiteRowId, @Reference, 'ALU_'+RTRIM(CAST(@MAx+1 AS NVARCHAR(10))) , GetDAte(), GetDate())
 
END



IF (NOT EXISTS (SELECT * FROM Uniwave_a2p_ReferenceMappingLog Where Reference =@Reference))
Insert Into Uniwave_a2p_ReferenceMappingLog
	( [RefereNceBase]
	 ,[Reference]
	 ,[SourceReference]
	 ,[SourceColor]
	 ,[SourceColor1]
	 ,[SourceColor2]
	 ,[SapaReferenceBase]
	 ,[SapaReference]
	 ,[SapaColor]
	 ,[SapaColor1]
	 ,[SapaColor2]
	 ,[ExternalReference])
VALUES(@ReferenceBase 
	 ,@Reference
	 ,@SourceReference
	 ,@SourceColor
	 ,@SourceColor1
	 ,@SourceColor2
	 ,@SapaReferenceBase
	 ,@SapaReference
	 ,@SapaColor
	 ,@SapaColor1
	 ,@SapaColor2
	 ,ISNULL(@SapaExternalReference,@ExternalReference))

END
GO

-- Insert Colors nto PrefSuite DB
--==============================================================================
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
	( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', 
		NEWID(), -- RowId - uniqueidentifier
		@Color, -- Nombre - nchar(50)
		16777215, -- RGB - int
		0, -- Numero - smallint
		'988 TechDesign', -- Nivel1 - nvarchar(150)
		NULL, -- Nivel2 - nvarchar(150)
		NULL, -- Nivel3 - nvarchar(150)
		NULL, -- Nivel4 - nvarchar(150)
		NULL, -- Nivel5 - nvarchar(150)
		(SELECT Decoracion FROM Colores WHERE Nombre = 'White') , -- Decoracion - image
		' *' , -- DesAuto - nvarchar(120)
		N'', -- DesProd - nvarchar(120)
		0.0, -- AmbientRed - float
		0.0, -- AmbientGreen - float
		0.0, -- AmbientBlue - float
		0.99609375, -- DiffuseRed - float
		0.99609375, -- DiffuseGreen - float
		0.99609375, -- DiffuseBlue - float
		0.0, -- SpecularRed - float
		0.0, -- SpecularGreen - float
		0.0, -- SpecularBlue - float
		1, -- Transparency - float
		NULL, -- Texture - image
		0.0, -- AngleTexture - float
		0.0, -- TextureScaleX - float
		0.0, -- TextureScaleY - float
		N'_TechDesign', -- Family - nchar(25)
		0, -- FamilyOrder - int
		N'', -- BasicRawMaterial - nchar(25)
		0, -- RawMaterial - int
		NULL, -- Image - image
		1, -- Generico - smallint
		N'', -- Material - nchar(25)
		@ColorDescription, -- Description - nvarchar(120)
		1, -- InnerAllowed - smallint
		1, -- OuterAllowed - smallint
		1, -- RuleGenerator - smallint
		0, -- CustomTariffCalculation - smallint
		NULL, -- Pattern - nchar(50)
		0, -- Standard - smallint
		0, -- EffectivePerimeterIgnored - smallint
		NULL, -- ColorTypeCode - smallint
		0.0, -- Alpha - float
		NULL, -- Render3DMaterial - int
		0, -- InnerColorEditable - smallint
		0 -- OuterColorEditable - smallint
		)
	END 

END
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
	( (SELECT MAX(ConfigurationCode)+1 FROM dbo.ColorConfigurations), -- ConfigurationCode - int
		@Color, -- ColorName - nvarchar(50)
		NULL, -- InnerColor - nvarchar(50)
		NULL -- OuterColor - nvarchar(50)
		)

END
GO


-- Insert Materials into Main PrefSuite DB tables 
--==============================================================================
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
	( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		NEWID(), -- RowId - uniqueidentifier
		@ReferenceBase, -- ReferenciaBase - nchar(25)
		@Reference, -- Referencia - nchar(25)
		@Color, -- Color - nchar(50)
		980,  -- Almacen - smallint
		1,  -- UE1 - int
		@PackageQuantity,  -- UE2 - int
		1,  -- ControlDeStock - smallint
		1,  -- PedirBajoDemanda - smallint
		0,  -- ManageRemnants - smallint
		Case When @MaterialType = 1 -- LongitudBarra - real
		THEN @PackageQuantity*1000
		ELSE 0
		END,
		0.000000, -- WastageAllowance - double (19, 6)
		0,  -- UseWastageAllowanceInMN - smallint
		0,  -- UseFullRodsInMN - smallint
		0,  -- IsModel - smallint
		1,  -- TargetLevel - int
		0,  -- PrefShopStatus - smallint
		0,  -- DefaultValue - smallint
		979,  -- MaterialSupplierCode - int
		1,  -- ProductionPreparationTime - int
		14  -- AverageDeliveryTime - smallint
		)

END	
GO

CREATE OR ALTER PROCEDURE[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase] 
	@ReferenceBase NVARCHAR(25),
	@Description NVARCHAR(255),
	@MaterialType INT, 
	@CommodityCode INT
	

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
		DontIncludeInMaterialReport, 
		CommodityCode
	 
	)
	VALUES
	( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		NEWID(), -- RowId - uniqueidentifier
		@ReferenceBase, -- ReferenciaBase - nchar(25) 
		@Description,	 
			CASE WHEN @MaterialType = 1 THEN 'Barras'
			 WHEN @MaterialType = 2 THEN 'Metros'
			 WHEN @MaterialType = 3 THEN 'Piezas'
			 WHEN @MaterialType = 4 THEN 'Superficies'
			 WHEN @MaterialType = 5 THEN 'Superficies'
		ELSE 'Piezas' END, -- TipoCalculo - nchar(15)
		N'988 TechDesign', -- Nivel1 - nvarchar(150)
		CASE WHEN @MaterialType = 1 THEN 'Bars'
			 WHEN @MaterialType = 2 THEN 'Gaskets'
			 WHEN @MaterialType = 3 THEN 'Piece Materials'
			 WHEN @MaterialType = 4 THEN 'Panels'
			 WHEN @MaterialType = 5 THEN 'Glasses'
		ELSE 'Piece Materials' END, -- Nivel2 - nchar(150)
		979, -- CodigoProveedor - int
		0, -- NoIncluirEnHojaDeTrabajo - smallint
		0, -- NoOptimizar - smallint
		2, -- NoIncluirEnMaterialNeeds - smallint
		0, -- OrdenPrecioKg - smallint
		0, -- IdGrupoPresupuestado - smallint
		0, -- IdGrupoProduccion - smallint
		0, -- OrdenDesAuto - smallint
		0, -- OrdenDesProd - smallint
		0, -- OrdenOptimizacion - smallint
		0, -- Valorador - smallint
		0, -- IsFrameFitting - smallint
		CASE WHEN @MaterialType = 5 THEN 'Glass' --Role,
			 ELSE 'Unknown' END,
		0, -- WorkPlace - smallint
		0, -- ConditionalWorkPlace - smallint
		0, -- StockInWorkPlace - smallint
		0, -- CustomTariffCalculation - smallint
		0, -- DoNotShowInMonitors - smallint
		0, -- DoNotShowInTree - smallint
		0.0, -- Area - float
		0.0, -- InnerColorPerimeter - float
		0.0, -- OuterColorPerimeter - float
		0.0, -- InsertionPointX - float
		0.0, -- InsertionPointY - float
		1, -- ShowIn3D - smallint
		1, -- ShowIn2DInner - smallint
		0, -- ShowIn2DOuter - smallint
		0, -- MaterialSide - smallint
		0, -- IsDummy - smallint
		0, -- IsTransparent - smallint
		-1, -- ColorControl - smallint
		0, -- UnMountable - smallint
		0, -- MountedDefaultState - smallint
		0.0, -- PackedQuantity - float
		0, -- PackedUnitsType - smallint
		0, -- PriceBookLevel - smallint
		0, -- PrefShopStatus - smallint
		0, -- DontIncludeInMaterialReport - smallint,
		@CommodityCode
		)
END
GO


-- Insert Materials technical details into PrefSuite DB tables 
--==============================================================================
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
	( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		@ReferenceBase, -- ReferenciaBase - nchar(25)
		2, -- PriceUnitsType - smallint
		@Weight, -- LinearWeightKg_m - real
		0, -- LossType - smallint
		0.0 -- CustomLoss - float
		)
	END
	END
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
		( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
			@ReferenceBase, -- ReferenciaBase - nchar(25)
			@Weight -- UnitWeightKg - real
			)
	END
GO

CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile] 
	-- Add the parameters for the stored procedure here
	@ReferenceBase NVARCHAR(25),
	@PackageQuantity decimal (38,6),
	@Weight decimal (38,6)
	

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
	( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		@ReferenceBase, -- ReferenciaBase - nchar(25)
		@PackageQuantity*1000, -- LongitudBarra - real
		@Weight,
		1, -- AnchoExterior - real
		0.0, -- AnchoInterior - real
		1, -- Altura - real
		0.0, -- CuerpoInterior - real
		0.0, -- PerimetroSeccion - real
		0.0, -- CuerpoExterior - real
		0, -- Soldable - int
		0, -- Divisible - int
		0.0, -- Torsion - float
		0.0, -- InerciaX - float
		0.0, -- InerciaY - float
		0.0, -- InertiaXY - float
		0, -- Structural - smallint
		0.0, -- ShearAreaX - float
		0.0, -- ShearAreaY - float
		0.0, -- ModulusOfElasticityX - float
		0.0, -- ModulusOfElasticityY - float
		0.0, -- LongestLength - float
		0.0, -- LongestThickness - float
		0.0, -- SigmaMax - float
		0.0, -- SigmaMin - float
		0.0, -- TurnRadioX - float
		0.0, -- TurnRadioY - float
		0.0, -- InnerFaceOffset - real
		0.0, -- OuterFaceOffset - real
		0.0, -- MinWidth - real
		0, -- ForgedLevel - smallint
		0.0, -- Wing - float
		0, -- MirrorHorizontalForMachining - smallint
		0, -- MirrorVerticalForMachining - smallint
		0.0, -- RotationForMachining - float
		0, -- PriceUnitsType - smallint
		0, -- AutoDivisible - smallint
		0, -- Turnable - smallint
		0, -- GenerateSquare - smallint
		0, -- FixedInnerFaceName - smallint
		0, -- FixedOuterFaceName - smallint
		0.0, -- BendingMachineLoss - float
		0, -- ExteriorSnapinMuntin - smallint
		0.0, -- BottomMarginForFullRod - real
		0, -- AngleCut - smallint
		0, -- MullionCorneringType - smallint
		0, -- Composite - smallint
		0, -- OrderComponents - smallint
		0, -- TimeOptimization - smallint
		0, -- WeightPriceCalculation - smallint
		0 -- PaintPriceCalculation - smallint
		)
	END
	
END
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
	( 'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
		@ReferenceBase, -- ReferenciaBase - nchar(25)
		0.0, -- MultiploVertical - float
		0.0, -- MultiploHorizontal - float
		@weight, -- PesoSuperficial - real
		1, -- Espesor - real
		0.0, -- MinimoM2 - float
		0.0, -- DescuentoBarrotillo - float
		CASE WHEN @MaterialType = 5 THEN 0 ELSE 2 END,
		0.0, -- AltoPanel - float
		0.0, -- AnchoPanel - float
		0, -- Tabla - smallint
		0, -- Composite - smallint
		0, -- HasDirection - smallint
		0, -- Turnable - smallint
		0, -- Mirrorable - smallint
		0.0, -- MinimumWidth - float
		0.0, -- MinimumHeight - float
		0.0, -- MinArea - float
		0.0, -- MaximumWidth - float
		0.0, -- MaximumHeight - float
		0.0, -- MaxArea - float
		0, -- ProportionalFactorNum - int
		0, -- ProportionalFactorDen - int
		0.0, -- KFactor - float
		0.0, -- GFactor - float
		0.0, -- AcousticFactor - float
		0.0, -- LightTransFactor - float
		0.0, -- PsiFactor - float
		0.0, -- UFactor - float
		0.0, -- Offset - float
		0, -- PricingAfterMatrixLine - smallint
		0, -- Tempered - smallint
		N'', -- Gas - nchar(25)
		0, -- PriceUnitsType - smallint
		0.0, -- MaximumWeight - float
		0, -- LowEmissive - smallint
		0.0, -- AcousticCFactor - float
		0.0, -- AcousticCtrFactor - float
		0.0, -- ThermalConductivity - float
		0, -- AllowInternalGeorgianBar - smallint
		0, -- AllowExternalGeorgianBar - smallint
		0 -- SubType - smallint
		)
	END
	
END
GO


-- Insert Materials purchase details into PrefSuite DB tables 
--==============================================================================
CREATE OR ALTER PROCEDURE [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPurchaseData] 
	-- Add the parameters for the stored procedure here
	@Reference NVARCHAR(25), 
	@Package INT,
	@Price decimal (38,6),
	@Description NVARCHAR(255),
	@Color NVARCHAR(50),
	@SourceReference NVARCHAR(50),
	@SourceColor NVARCHAR(50),
	@MaterialType int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*If purchase data not exists, then insert it*/
	

	IF NOT EXISTS (SELECT * FROM COMPRAS WHERE Referencia =@Reference )
	INSERT INTO dbo.Compras
	(
		Referencia,
		Proveedor,
		APartir,
		UP1,
		UP2,
		FechaUltimaCompra,
		PrecioUltimaCompra,
		ReferenciaProveedor,
		SupplierDescription,
		Divisa,
		FechaEVPrecioSC,
		PrecioSC,
		DivisaPrecioSC,
		EntregaMedia,
		CodigoEAN13,
		DescripcionUP1,
		DescripcionUP2,
		ByDefault,
		SchedulerTime,
		ReorderingTime
	)
	VALUES
	( @Reference, -- Referencia - nchar(25)
		979,  -- Proveedor - int
		1,  -- APartir - int
		1,  -- UP1 - int
		CASE WHEN @MaterialType = 1 THEN 1 ELSE  @Package END, -- UP2 - float
		GETDATE(), -- FechaUltimaCompra - datetime
		ISNULL(@Price,0), --PrecioUltimaCompra - float
		@SourceReference, -- ReferenciaProveedor - nchar(50)
		ISNULL(@Description,@SourceReference) +' '+ @SourceColor, -- SupplierDescription - nvarchar(255)
		N'NOK', -- Divisa - nchar(25)
		NULL, -- FechaEVPrecioSC - datetime
		0, -- PrecioSC - float
		N'NOK', -- DivisaPrecioSC - nchar(25)
		14,  -- EntregaMedia - int
		N'', -- CodigoEAN13 - nchar(13)
		CASE WHEN @MaterialType = 1 THEN 'Bar'
			 WHEN @MaterialType = 2 and @Package =1  THEN 'Meter'
			 WHEN @MaterialType = 2 and @Package >1  THEN 'Roll'
			 WHEN @MaterialType = 3 and @Package =1  THEN 'Piece'
			 WHEN @MaterialType = 3 and @Package >1  THEN 'Box'
		ELSE N'' END, -- DescripcionUP1 - nvarchar(50)

		CASE WHEN @MaterialType = 1 THEN 'Bar'
			 WHEN @MaterialType = 2 and @Package =1  THEN 'Meter'
			 WHEN @MaterialType = 2 and @Package >1  THEN 'Meters'
			 WHEN @MaterialType = 3 and @Package =1  THEN 'Piece'
			 WHEN @MaterialType = 3 and @Package >1  THEN 'Pieces'
		ELSE N'' END, -- DescripcionUP2 - nvarchar(50)
		1,  -- ByDefault - smallint
		14,  -- SchedulerTime - int
		0  -- ReorderingTime - int
		)


		DECLARE @ColorConfiguration INT, 
				@Length FLOAT
				

			SELECT TOP 1 @ColorConfiguration = C.ConfigurationCode FROM dbo.ColorConfigurations C
			INNER JOIN Materiales M ON C.ColorName=M.Color WHERE M.Referencia =@Reference

			SELECT @Length = M.LongitudBarra FROM dbo.Perfiles P
			INNER JOIN dbo.MaterialesBase MB ON P.ReferenciaBase=MB.ReferenciaBase
			INNER JOIN Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE M.Referencia =@Reference


		IF NOT EXISTS (SELECT * FROM dbo.MaterialLevels WHERE Reference=@Reference AND ColorConfiguration=@ColorConfiguration AND Warehouse=980 AND Length=ISNULL(@Length,0) AND Height=0)
		INSERT INTO dbo.MaterialLevels
		(
			RowId,
			Reference,
			ColorConfiguration,
			Warehouse,
			Length,
			Height,
			Level1,
			Level2
		)
		VALUES
		( NEWID(), -- RowId - uniqueidentifier
			@Reference, -- Reference - nchar(25)
			@ColorConfiguration , -- ColorConfiguration - int
			980, -- Warehouse - smallint
			ISNULL(@Length,0) , -- Length - real
			0.0, -- Height - real
			0.0, -- Level1 - float
			0.0 -- Level2 - float
			)
		
		IF NOT EXISTS (SELECT * FROM ReferenceSuppliers WHERE Reference =@Reference )
		INSERT INTO dbo.ReferenceSuppliers
		(
			Reference,
			SupplierCode,
			Percentage
		)
		VALUES
		( @Reference, -- Reference - nchar(25)
			979, -- SupplierCode - int
			100 -- Percentage - float
		)

END
GO




