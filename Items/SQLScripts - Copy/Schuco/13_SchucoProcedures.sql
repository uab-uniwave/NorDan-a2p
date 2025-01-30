USE [Prefsuite]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterialPurchaseData]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoMaterialPurchaseData]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterialNeeds]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoMaterialNeeds]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterialColor]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoMaterialColor]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterial]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoMaterial]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertWorkforce]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoInsertWorkforce]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertPositions]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoInsertPositions]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertMNRecordGlass]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoInsertMNRecordGlass]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertMNRecord]    Script Date: 2024-12-29 21:19:31 ******/
DROP PROCEDURE [dbo].[Uniwave_SchuecoInsertMNRecord]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertMNRecord]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


?CREATE   PROCEDURE [dbo].[Uniwave_SchuecoInsertMNRecord] 
	@File nvarchar(255),
	@Project nvarchar(255),
	@ArticleNo nvarchar(255),
	@Description nvarchar(255),
	@Color nvarchar(255),
	@TotalPrice float,
	@QuantityEA float,
	@Quantity int,
	@Delivery nvarchar(255),
	@Dimensions nvarchar(255),
	@Weight float,
	@InsertDate datetime 
?
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @id int
?
INSERT INTO Schueco_RecordsMaterials ([File], Project, ArticleNo, Description, Color,TotalPrice,QuantityEA,Quantity,Delivery,Dimensions,Weight,InsertDate)
VALUES (@File,@Project,@ArticleNo,@Description,@Color,@TotalPrice,@QuantityEA,@Quantity,@Delivery,@Dimensions,@Weight,@InsertDate)
set @id = @@IDENTITY
?
 DECLARE @Prefix Nvarchar(5)
?
	SET @Prefix='S_'
?
?
	DECLARE 
?
	@Number INT, 
	@Version INT,
	@Color1 NVARCHAR(50)='',
	@Color2 NVARCHAR(50)='',
	@P_Color1 NVARCHAR(50)='',
	@P_Color2 NVARCHAR(50)='',
	@pPackage FLOAT,
	@MaterialType NVARCHAR(10),
	@Reference NVARCHAR(25),
	@ProviderReference nvarchar(50),
	@ReferenceBase NVARCHAR(25),
	@pWidth FLOAT, 
    @pHeight FLOAT,
	@Price float,	
	@mnWidth FLOAT,
	@mnHeight FLOAT,
	@mnWeight FLOAT,
	@LastMatCounter int,
	@pToOrder smallint
	
	 
	IF NOT EXISTS (SELECT * FROM PAF WHERE Referencia = @Project)
	RETURN 0
?
	/*Identify Number and version of Order */
	SELECT @Number=Numero , @Version = Version FROM PAF WHERE Referencia = @Project

	-- update Document number and version
	update dbo.Schueco_RecordsMaterials set DocNumber = @Number, DocVersion = @Version where Id = @id

?
	set @Color =  REPLACE(REPLACE(@Color, CHAR(13), ''), CHAR(10), '')
	set @Color = REPLACE(@Color,',','')
	set @Color = REPLACE(@Color,'.','')
	set @Color = REPLACE(@Color,' Gloss 10','G1')
	set @Color = REPLACE(@Color,' Gloss 20','G2')
	set @Color = REPLACE(@Color,' Gloss 30','G3')
	set @Color = REPLACE(@Color,' Gloss 40','G4')
	set @Color = REPLACE(@Color,' Gloss 50','G5')
	set @Color = REPLACE(@Color,' Gloss 60','G6')
	set @Color = REPLACE(@Color,' Gloss 70','G7')
	set @Color = REPLACE(@Color,' Gloss 80','G8')
	set @Color = REPLACE(@Color,' Gloss 90','G9')
    set @Color = REPLACE(@Color,' Gloss 77','G77')
?
	SELECT top 1 @Color1 = Name FROM dbo.splitstring(@Color)
	where Name not like '%inside%'
?
	if (@Color like '%inside%')
	BEGIN
		SELECT @Color2 = Name FROM dbo.splitstring(@Color)
		where Name not like '%inside%'
?
		set @Color2 = SUBSTRING(@Color2, (LEN(@Color2) -  CHARINDEX(' ', REVERSE(@Color2))+1),200)
?
		set @Color2 = LTRIM(RTRIM(@Color2))
	END
?
?
	set @Color1 = REPLACE(@Color1,'Outside','')
	set @Color1 = REPLACE(@Color1,'Centre','')
?
	set @Color1 = SUBSTRING(@Color1, (LEN(@Color1) -  CHARINDEX(' ', REVERSE(@Color1))+1),200)
	set @Color1 = LTRIM(RTRIM(@Color1))
?
?
	if (@Color1 = '')
	BEGIN
		set @P_Color1 = CONCAT(@Prefix,'None')
	END
	else
	BEGIN
		set @P_Color1 = CONCAT(@Prefix, @Color)
	END
?
	set @Color = @P_Color1
?
	EXEC dbo.Uniwave_SchuecoMaterialColor @P_Color1 ,''
?
	if (@Color2 != '')
	BEGIN
		
		set @P_Color2 = CONCAT(@Prefix, @Color2)
?
		set @Color = CONCAT(@P_Color1,'/',@P_Color2)
		
		EXEC dbo.Uniwave_SchuecoMaterialColor @P_Color2 ,''
?
		IF NOT EXISTS (SELECT * FROM ColorConfigurations WHERE ColorName = @Color)
		BEGIN
			INSERT INTO dbo.ColorConfigurations(ConfigurationCode,ColorName,InnerColor,	OuterColor)
			VALUES(  (SELECT MAX(ConfigurationCode)+1 FROM dbo.ColorConfigurations),
				@Color,
				@P_Color1,
				@P_Color2
				)
		END
	END
?
	
?
	SELECT @MaterialType = CASE WHEN @Delivery like '%Sta%' THEN 'Bar'
	    					    WHEN @Delivery like '%UNCLEAR%'  THEN 'Meter'
							    WHEN @Delivery like '%Number%' or @Delivery like '%PU%' THEN 'Piece'
								WHEN @Dimensions like  '% x %' THEN 'Surface'								
						        ELSE 'Piece' END
?
	/* Figured out that some reference has "(and they should be imported sd well as seprate references)"
	cahnge requested By arunas Gulbinas implemented on 2022-06-11)
	
	if (@ArticleNo like '%(%')
	BEGIN
		--set @ArticleNo = RTRIM(SUBSTRING(@ArticleNo,1,CHARINDEX('(',@ArticleNo)-1))
		set @ArticleNo = RTRIM(SUBSTRING(@ArticleNo,CHARINDEX('(',@ArticleNo)+1, LEN(@ArticleNo)-CHARINDEX('(',@ArticleNo)-1))
	END*/
?
	set @ArticleNo = LTRIM(RTRIM(@ArticleNo))
	
	set @ReferenceBase = LEFT(CONCAT(@Prefix,@ArticleNo),25)
	set @Reference =   LEFT(CONCAT(@Prefix,@ArticleNo,' ',@Color1,IIF(@Color2 != '','/',''),@Color2),25)
	set @ProviderReference = LEFT(CONCAT(@ArticleNo,' ',@Color1,IIF(@Color2 != '','/',''),@Color2),25)
?
	if @MaterialType != 'Piece'
	BEGIN
		set @QuantityEA = @Quantity
	END
?
	SELECT @pPackage=CASE WHEN @MaterialType = 'Bar' THEN CAST(@Dimensions as float)/1000		
				WHEN @MaterialType = 'Meter' THEN CAST(@Dimensions as float)		
				WHEN @MaterialType = 'Piece' and @Delivery like '%Number%' THEN 1
				WHEN @MaterialType = 'Piece' and @Delivery like '%PU%' THEN CAST(@Dimensions as float)
				WHEN @MaterialType = 'Surface' THEN 1
		END
?
?
	SELECT @mnWidth=CASE WHEN @MaterialType = 'Bar' THEN @pPackage*1000
						 WHEN @MaterialType = 'Meter' THEN @pPackage*1000
						 WHEN @MaterialType = 'Piece' THEN 0
						 WHEN @MaterialType = 'Surface' THEN CAST(SUBSTRING(@Dimensions,1, CHARINDEX(' ',@Dimensions)-1)AS FLOAT) 
					END
    SELECT @mnHeight=CASE WHEN @MaterialType = 'Bar' THEN 0
						  WHEN @MaterialType = 'Meter' THEN 0
						  WHEN @MaterialType = 'Piece' THEN 0
						  WHEN @MaterialType = 'Surface' THEN CAST(SUBSTRING(@Dimensions, CHARINDEX('x',@Dimensions)+1,LEN(@Dimensions)) AS FLOAT) 
					 END  
	
    IF (@QuantityEA >0 AND @pPackage>0)
    BEGIN 
    SELECT @mnWeight=CASE WHEN @MaterialType = 'Bar' THEN (@Weight/@QuantityEA)/@pPackage
                          WHEN @MaterialType = 'Meter' THEN (@Weight/@QuantityEA)/@pPackage
                          WHEN @MaterialType = 'Piece' THEN @Weight/@QuantityEA
                          WHEN @MaterialType = 'Surface' THEN (@Weight/@QuantityEA)/ (@mnWidth*@mnHeight/1000000)
	END
    
?
	SELECT @Price=CASE WHEN @MaterialType = 'Bar' THEN (@TotalPrice/@QuantityEA)/@pPackage
		WHEN @MaterialType = 'Meter' THEN (@TotalPrice/@QuantityEA)/@pPackage
		WHEN @MaterialType = 'Piece' THEN @TotalPrice/@QuantityEA
		WHEN @MaterialType = 'Surface' THEN (@TotalPrice/@QuantityEA)/ (@mnWidth*@mnHeight/1000000)
	END
    set @Price = ROUND(@Price,3)
    END?

    ELSE 
    BEGIN 
        SET @Price=0
        SET @mnWeight=0
    END
	
?
?
?
	EXEC dbo.Uniwave_SchuecoMaterial @ReferenceBase, @Reference, @Description, @MaterialType, @Color, @pPackage
?
	IF @MaterialType = 'Bar'
	EXEC dbo.Uniwave_SchuecoMaterialPurchaseData @Reference,  1, @Price, @ProviderReference ,@Description,@Color
	ELSE 
	EXEC dbo.Uniwave_SchuecoMaterialPurchaseData @Reference, @pPackage, @Price,  @ProviderReference,@Description,@Color
?

	/*Check color, if not exists than insert*/
	/*======================================*/

	/*Identity of  Material on Demand or not By request of AG 13.11.2018*/
	SET @pToOrder =1
	IF (@MaterialType = 'Piece' OR  @MaterialType = 'Meter') AND  	@QuantityEA/@Dimensions <0.5  
	SET @pToOrder =0

						  

	
	EXEC dbo.Uniwave_SchuecoMaterialNeeds @Number, @Version, @Reference, @MaterialType, @mnWidth, @mnHeight,@mnWeight,@QuantityEA,'', @pToOrder
	
	END
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertMNRecordGlass]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE    PROCEDURE [dbo].[Uniwave_SchuecoInsertMNRecordGlass] 
	@Project nvarchar(255),
	@Item nvarchar(255),
	@Field int,
	@Quantity int,
	@Width float,
	@Height float,
	@Weight float,
	@Price float,
	@Description nvarchar(255),
	@InsertDate datetime
AS
BEGIN
	SET NOCOUNT ON;

	set @Weight = @Weight / @Quantity
	
	set @Price = ROUND(@Price / (@Width * @Height / 1000000),3)

INSERT INTO [dbo].[Schueco_RecordsGlasses]
           (
			[Project] ,
			[Item] ,
			[Field],
			[Quantity],
			[Width],
			[Height],
			[Weight],
			[Price],
			[Description],
			[InsertDate]
           )
     VALUES
           (@Project,
			@Item,
			@Field,
			@Quantity,
			@Width,
			@Height,
			@Weight,
			@Price,
			@Description,
			@InsertDate)
           
		   
	DECLARE 

	@Number INT, 
	@Version INT,
	@pColor NVARCHAR(50),
	@pPackage FLOAT,
	@pReferenciaBase NVARCHAR(25), 
	@pReferencia NVARCHAR(25), 
	@MaterialType NVARCHAR(10),
	@pDescription NVARCHAR(255)
	 
	
	IF NOT EXISTS (SELECT * FROM PAF WHERE Referencia =@Project)

	RETURN 0

	DECLARE @Prefix Nvarchar(5)
	SET @Prefix =''
	SET @pColor = RTRIM(@Prefix)+'Glass'
	/*Identify Number and version of Order */
	SELECT @Number=Numero , @Version = Version FROM PAF WHERE Referencia =@Project


		
	/* Define , Material type */
	SELECT @MaterialType = 'Glass'
	
	/*Identify package */
	SELECT @pPackage= 1

	set @Description = LTRIM(@Description)
	set @Description = RTRIM(@Description)


	SET @pDescription =  SUBSTRING(@Description ,1, CASE WHEN  CHARINDEX(',', @Description)>0 THEN  CHARINDEX(',', @Description)-1 ELSE LEN(@Description) END  ) 
	SET @pDescription = REPLACE (@pDescription, 'LowE', 'Sel')
	SET @pDescription = REPLACE (@pDescription, '-F2-', '-2-')
	SET @pDescription = REPLACE (@pDescription, '-F3-', '-3-')
	SET @pDescription = REPLACE (@pDescription, '-F4-', '-4-')
	SET @pDescription = REPLACE (@pDescription, '-F5-', '-5-')
	SET @pDescription = REPLACE (@pDescription, '-F6-', '-6-')
	SET @pDescription = REPLACE (@pDescription, '-F7-', '-7-')
	SET @pDescription = REPLACE (@pDescription, '-F8-', '-8-')
	SET @pDescription = REPLACE (@pDescription, '-F9-', '-9-')
	SET @pDescription = REPLACE (@pDescription, '-F2', '-2')
	SET @pDescription = REPLACE (@pDescription, '-F3', '-3')
	SET @pDescription = REPLACE (@pDescription, '-F4', '-4')
	SET @pDescription = REPLACE (@pDescription, '-F5', '-5')
	SET @pDescription = REPLACE (@pDescription, '-F6', '-6')
	SET @pDescription = REPLACE (@pDescription, '-F7', '-7')
	SET @pDescription = REPLACE (@pDescription, '-F8', '-8')
	SET @pDescription = REPLACE (@pDescription, '-F9', '-9')
	SET @pDescription = REPLACE (@pDescription, 'F2-', '2-')
	SET @pDescription = REPLACE (@pDescription, 'F3-', '3-')
	SET @pDescription = REPLACE (@pDescription, 'F4-', '4-')
	SET @pDescription = REPLACE (@pDescription, 'F5-', '5-')
	SET @pDescription = REPLACE (@pDescription, 'F6-', '6-')
	SET @pDescription = REPLACE (@pDescription, 'F7-', '7-')
	SET @pDescription = REPLACE (@pDescription, 'F8-', '8-')
	SET @pDescription = REPLACE (@pDescription, 'F9-', '9-')

	
	/*Identify referenciabase */
	--SELECT @pReferenciaBase = LEFT(RTRIM(@Prefix)+@sReference,25)

	IF EXISTS(SELECT *  FROM dbo.MaterialesBase WHERE Descripcion = @pDescription)
	SELECT TOP 1 @pReferenciaBase =  ReferenciaBase FROM MaterialesBase WHERE Descripcion = @pDescription AND TipoCalculo = 'Superficies' AND Role='Glass'


	IF EXISTS(SELECT *  FROM dbo.MaterialesBase MB 
	INNER JOIN dbo.Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE Descripcion = @pDescription AND TipoCalculo = 'Superficies' AND Role='Glass')

	SELECT TOP 1 @pReferencia =  Referencia FROM dbo.MaterialesBase MB 
	INNER JOIN dbo.Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE Descripcion = @pDescription AND TipoCalculo = 'Superficies' AND Role='Glass'
	ELSE
	BEGIN
		DECLARE @Error NVARCHAR (255) =  'Glass missing in Prefsuite, please check: ' + @pDescription
		RAISERROR (15600,-1,-1, @Error);  
		RETURN 0
	END 

	/*Check color, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SchuecoMaterialColor 'Glass', @Prefix
	IF EXISTS(SELECT * FROM dbo.MaterialesBase MB 
	INNER JOIN dbo.Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE Descripcion = @pDescription)
	SELECT TOP 1 @pColor =  Color FROM dbo.MaterialesBase MB 
	INNER JOIN dbo.Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE Descripcion = @pDescription
	
	

	/*Check material, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SchuecoMaterial @pReferenciaBase, @pReferencia, @pDescription, @MaterialType, @pColor, @pPackage

	/*Check purchase data, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SchuecoMaterialPurchaseData @pReferencia, @pPackage, @Price, @pReferencia ,@pDescription,''


	/*Check material needs, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SchuecoMaterialNeeds @Number, @Version, @pReferencia, @MaterialType, @Width,@Height,@Weight,@Quantity,@Field,1
	
	 
	END
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertPositions]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-----------------------------------------------------[Uniwave_SchuecoInsertPositions]

CREATE    PROCEDURE [dbo].[Uniwave_SchuecoInsertPositions]     
	@ProjectNumber nvarchar(255),
	@ProjectName nvarchar(255),
	@ItemNumber nvarchar(255),
	@ItemDescription nvarchar(255),
	@ProfileSystem nvarchar(255),
	@Quantity int,
	@Width float,
	@Height float,
	@Weight float,
	@WeightGlass float,
	@Price float,
	@TotalLabour float,
	@Number int,
	@Version int,
	@SortOrder int,
	@InsertDate datetime
	

AS
BEGIN


	SET NOCOUNT ON;

INSERT INTO dbo.Schueco_RecordsPositions
(
    ProjectNumber,
	ProjectName,
	ItemNumber,
	ItemDescription,
	ProfileSystem,
	Quantity,
	Width,
	Height,
	Weight,
	WeightGlass,
	Price,
	Number,
	Version,
	SortOrder,
	InsertDate,
	ModifiedDate,
	TotalLabour
)
VALUES
(   
	@ProjectNumber ,
	@ProjectName ,
	@ItemNumber ,
	@ItemDescription ,
	@ProfileSystem ,
	@Quantity ,
	@Width ,
	@Height ,
	@Weight ,
	@WeightGlass ,
	@Price ,
	@Number ,
	@Version ,
	@SortOrder ,
	@InsertDate ,
	null ,
	@TotalLabour
    ) 
	END

------------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoInsertWorkforce]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE    PROCEDURE [dbo].[Uniwave_SchuecoInsertWorkforce]	
@Number INT, 
@Version INT, 
@sModified DATETIME 
AS
BEGIN
	SET NOCOUNT ON;
	
	
	DECLARE @Orden INT,
			@Seconds INT , 
			@Quantity INT, 
			@TotalLabour FLOAT,
			@RowId int
			
			
	
	DELETE FROM dbo.ManoObraPAF WHERE Numero =@Number AND Version =@Version	
	
	
	DECLARE mo CURSOR FOR 

	SELECT srp.TotalLabour, srp.Quantity, cpf.Orden, srp.Id
	FROM dbo.Schueco_RecordsPositions srp
	join ContenidoPAF cpf on cpf.Numero = srp.Number and cpf.Version = srp.Version and cpf.Nomenclatura = srp.ItemNumber
	WHERE srp.Number = @Number and srp.Version = @Version and  ModifiedDate is null

	OPEN MO
	FETCH NEXT FROM MO INTO @TotalLabour, @Quantity, @Orden, @RowId
	WHILE @@FETCH_STATUS = 0 
	BEGIN 
	SET @Seconds = 0

	SET @Seconds=ROUND((@TotalLabour/94)*3600,0)

	update Schueco_RecordsPositions set ModifiedDate = @sModified where Id = @RowId
			
	INSERT INTO dbo.ManoObraPAF
	(
	    Numero,
	    Version,
	    Orden,
	    SubModelId,
	    Puesto,
	    Segundos,
	    idWorkforce,
	    ProductTypeCode,
	    ProcessingClassInstance
	)
	VALUES
	(   @Number,   -- Numero - int
	    @Version,   -- Version - int
	    @Orden,   -- Orden - smallint
	    1,   -- SubModelId - smallint
	    N'Schueco', -- Puesto - nchar(30)
	    @Seconds/@Quantity, -- Segundos - real
	    0,   -- idWorkforce - int
	    0,   -- ProductTypeCode - int
	    0    -- ProcessingClassInstance - smallint
	    )
			FETCH NEXT FROM MO INTO  @TotalLabour, @Quantity, @Orden, @RowId
END
CLOSE MO
DEALLOCATE MO
	END 


------------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterial]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE    PROCEDURE [dbo].[Uniwave_SchuecoMaterial] 
	-- Add the parameters for the stored procedure here
	@pReferenciaBase NVARCHAR(25),
	@pReferencia NVARCHAR(25),
	@sDescription NVARCHAR(255),
	@MaterialType NVARCHAR(10),
	@pColor NVARCHAR(50),
    @pPackage real
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* Check material base not exists*/
	IF NOT EXISTS (SELECT * FROM dbo.MaterialesBase WHERE ReferenciaBase = @pReferenciaBase) 
	BEGIN
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
	    @pReferenciaBase,  -- ReferenciaBase - nchar(25)
	    
		CASE WHEN @MaterialType = 'Panel'  AND @pReferenciaBase LIKE 'Schueco_AluSheet%' THEN @sDescription + ' ' + SUBSTRING(@pColor,6,44)
		     WHEN @MaterialType = 'Surface' AND @pReferenciaBase LIKE 'Schueco_AluSheet%' THEN @sDescription + ' ' + SUBSTRING(@pColor,6,44)
			 ELSE
			 @sDescription
			 END, -- Descripcion - nvarchar(255)
	    
		CASE WHEN @MaterialType = 'Bar' THEN 'Barras'
			 WHEN @MaterialType = 'Meter' THEN 'Metros'
			 WHEN @MaterialType = 'Piece' THEN 'Piezas'
			 WHEN @MaterialType = 'Glass' THEN 'Superficies'
			 WHEN @MaterialType = 'Panel' THEN 'Superficies'
			 WHEN @MaterialType = 'Surface' THEN 'Superficies'
			 ELSE 'Piezas' END,  -- TipoCalculo - nchar(15)
	    N'990 Schueco',  -- Nivel1 - nvarchar(150)
	    CASE WHEN @MaterialType = 'Bar' THEN '01 Bars'
			 WHEN @MaterialType = 'Meter' THEN '02 Meters'
			 WHEN @MaterialType = 'Piece' THEN '03 Pieces'
			 WHEN @MaterialType = 'Glass' THEN '04 Surfaces'
			 WHEN @MaterialType = 'Panel' THEN '04 Surfaces'
			 WHEN @MaterialType = 'Surface' THEN 'Superficies'
			 ELSE 'Pieces' END,  -- Nivel2 - nvarchar(150)
	    990,    -- CodigoProveedor - int
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
		CASE WHEN @MaterialType = 'Glass' THEN 'Glass'
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
	
	IF NOT EXISTS (SELECT * FROM dbo.Materiales WHERE Referencia = @pReferencia) 
	BEGIN
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
	    @pReferenciaBase,       -- ReferenciaBase - nchar(25)
	    @pReferencia,       -- Referencia - nchar(25)
	    @pColor,       -- Color - nchar(50)
	    990,         -- Almacen - smallint
	    1,         -- UE1 - int
	    @pPackage,         -- UE2 - int
	    1,         -- ControlDeStock - smallint
	    1,         -- PedirBajoDemanda - smallint
	    0,         -- ManageRemnants - smallint
	    0.0,       -- LongitudBarra - real
	    0.000000,      -- WastageAllowance - decimal(19, 6)
	    0,         -- UseWastageAllowanceInMN - smallint
	    0,         -- UseFullRodsInMN - smallint
	    0,         -- IsModel - smallint
	    1,         -- TargetLevel - int
	    0,         -- PrefShopStatus - smallint
	    0,         -- DefaultValue - smallint
	    990,         -- MaterialSupplierCode - int
	    1,         -- ProductionPreparationTime - int
	    14         -- AverageDeliveryTime - smallint
	    )
	END

	IF NOT EXISTS (SELECT * FROM dbo.Perfiles WHERE ReferenciaBase = @pReferenciaBase) 
	BEGIN

		IF @MaterialType = 'Bar'
		BEGIN
		INSERT INTO dbo.Perfiles
		(
			MakerId,
			ReferenciaBase,
			LongitudBarra,
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
			@pReferenciaBase,  -- ReferenciaBase - nchar(25)
			@pPackage*1000,  -- LongitudBarra - real
			0.0,  -- AnchoExterior - real
			0.0,  -- AnchoInterior - real
			0.0,  -- Altura - real
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

			update Materiales SET LongitudBarra = @pPackage*1000 where ReferenciaBase = @pReferenciaBase
		END
	END
	
	IF NOT EXISTS (SELECT * FROM dbo.Metros WHERE ReferenciaBase = @pReferenciaBase) 
	BEGIN
		IF @MaterialType = 'Meter'
		BEGIN
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
			   @pReferenciaBase,  -- ReferenciaBase - nchar(25)
				2,    -- PriceUnitsType - smallint
				0.0,  -- LinearWeightKg_m - real
				0,    -- LossType - smallint
				0.0   -- CustomLoss - float
				)
		END
	END

	IF NOT EXISTS (SELECT * FROM dbo.Piezas WHERE ReferenciaBase = @pReferenciaBase) 
	BEGIN
		IF @MaterialType = 'Piece'
		BEGIN
			INSERT INTO dbo.Piezas
				(
					MakerId,
					ReferenciaBase,
					UnitWeightKg
				)
				VALUES
				(   'AE8D70E6-C414-412A-B272-AE141FCFA63F', -- MakerId - uniqueidentifier
					@pReferenciaBase,  -- ReferenciaBase - nchar(25)
					0.0   -- UnitWeightKg - real
					)
		END
	END

	IF NOT EXISTS (SELECT * FROM dbo.Superficies WHERE ReferenciaBase = @pReferenciaBase) 
	BEGIN
		IF @MaterialType = 'Glass' OR @MaterialType = 'Panel'  OR @MaterialType = 'Surface' 
		BEGIN
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
				@pReferenciaBase,  -- ReferenciaBase - nchar(25)
				0.0,  -- MultiploVertical - float
				0.0,  -- MultiploHorizontal - float
				0.0,  -- PesoSuperficial - real
				1,  -- Espesor - real
				0.0,  -- MinimoM2 - float
				0.0,  -- DescuentoBarrotillo - float
				CASE WHEN @MaterialType = 'Glass' THEN 0 ELSE 2 END,
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
	/* Possibly materialbase exists , but final reference not*/
	IF NOT EXISTS (SELECT * FROM Materiales WHERE Referencia = @pReferencia)
	BEGIN
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
	    NewId(),      -- RowId - uniqueidentifier
	    @pReferenciaBase,       -- ReferenciaBase - nchar(25)
	    @pReferencia,       -- Referencia - nchar(25)
	    @pColor,       -- Color - nchar(50)
	    990,         -- Almacen - smallint
	    1,         -- UE1 - int
	    @pPackage,         -- UE2 - int
	    1,         -- ControlDeStock - smallint
	    1,         -- PedirBajoDemanda - smallint
	    0,         -- ManageRemnants - smallint
	    0.0,       -- LongitudBarra - real
	    0.000000,      -- WastageAllowance - decimal(19, 6)
	    0,         -- UseWastageAllowanceInMN - smallint
	    0,         -- UseFullRodsInMN - smallint
	    0,         -- IsModel - smallint
	    1,         -- TargetLevel - int
	    0,         -- PrefShopStatus - smallint
	    0,         -- DefaultValue - smallint
	    990,         -- MaterialSupplierCode - int
	    1,         -- ProductionPreparationTime - int
	    14         -- AverageDeliveryTime - smallint
	    )
	END
END

---------------------------------------------------------------------------------------------------------------
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterialColor]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE    PROCEDURE [dbo].[Uniwave_SchuecoMaterialColor] 
	-- Add the parameters for the stored procedure here
	@sColor nvarchar(50) ,
	@Prefix NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE 

	@pColor NVARCHAR(50) = ISNULL(RTRIM(@Prefix),'')+ @sColor 
    
	IF NOT EXISTS (SELECT * FROM Colores WHERE Nombre = @pColor)
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
	    @pColor,  -- Nombre - nchar(50)
	    16777215,    -- RGB - int
	    0,    -- Numero - smallint
	    '990 Schueco',  -- Nivel1 - nvarchar(150)
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
	    N'_Schueco',  -- Family - nchar(25)
	    0,    -- FamilyOrder - int
	    N'',  -- BasicRawMaterial - nchar(25)
	    0,    -- RawMaterial - int
	    NULL, -- Image - image
	    1,    -- Generico - smallint
	    N'',  -- Material - nchar(25)
	    @sColor,  -- Description - nvarchar(120)
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


	IF NOT EXISTS (SELECT * FROM ColorConfigurations WHERE ColorName = @pColor)
	INSERT INTO dbo.ColorConfigurations
	(
	    ConfigurationCode,
	    ColorName,
	    InnerColor,
	    OuterColor
	)
	VALUES
	(   (SELECT MAX(ConfigurationCode)+1 FROM dbo.ColorConfigurations),   -- ConfigurationCode - int
	    @pColor, -- ColorName - nvarchar(50)
	    NULL, -- InnerColor - nvarchar(50)
	    NULL  -- OuterColor - nvarchar(50)
	    )
END

--------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterialNeeds]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE    PROCEDURE [dbo].[Uniwave_SchuecoMaterialNeeds] 
	-- Add the parameters for the stored procedure here
	@Number INT, 
	@Version INT, 
	@pReferencia NVARCHAR(25),
	@MaterialType NVARCHAR(10),
	@Width float, 
	@Height FLOAT,
	@sWeight FLOAT,
    @sQuantity FLOAT,
	@sLineId NVARCHAR(25),
	@pOnDemand int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	IF NOT EXISTS(SELECT * FROM dbo.Proveedores WHERE CodigoProveedor = 990)
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
	    990,         -- CodigoProveedor - int
	    N'Schueco',       -- Nombre - nvarchar(60)
	    N'EUR',       -- Divisa - nchar(25)
	    N'EUR'       -- Divisa2 - nchar(25)
	    )
		IF NOT EXISTS (SELECT * FROM dbo.Almacenes WHERE Codigo =990)
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
		(   990,   -- Codigo - smallint
		    N'Schueco', -- Descripcion - nvarchar(60)
		    0,   -- Externo - smallint
		    990,   -- ProviderCode - int
		    N'', -- Address - nvarchar(60)
		    N'', -- Address2 - nvarchar(60)
		    N'', -- City - nvarchar(60)
		    N'', -- PostalCode - nvarchar(25)
		    N'', -- County - nvarchar(60)
		    N'', -- Country - nvarchar(50)
		    1,   -- UsedInMRP - smallint
		    0    -- Kind - smallint
		    )

IF NOT EXISTS (SELECT * FROM dbo.MaterialNeedsMaster WHERE Number=@Number AND VERSION=@Version)
INSERT INTO dbo.MaterialNeedsMaster
(
    Number,
    Version,
    ProductionSet,
    ReproductionNeedsCode,
    MNSet,
    CalculationDate,
    Obsolete,
    Description,
    Discounted,
    TypeMNSet,
    ComponentsAssemblyUTCDate,
    CalculationUTCDate
)
VALUES
(   @Number,         -- Number - int
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
--ELSE UPDATE dbo.MaterialNeedsMaster
--SET MNSET = (SELECT MAX(MNSET)+1 FROM MaterialNeedsMaster  WHERE Number=@Number AND Version=@Version) , 
--    CalculationDate =GETDATE(), 
--	Description = RTRIM(CAST((SELECT MAX(MNSET)+1 FROM MaterialNeedsMaster  WHERE Number=@Number AND Version=@Version) AS NVARCHAR(2)))+'.- ' +CAST (GETDATE() AS NVARCHAR(16)),
--	CalculationUTCDate = GETUTCDATE()
	
--	WHERE Number=@Number AND Version=@Version

	if exists (select top 1 1 from Materiales mt
	join MaterialesBase mb on mt.ReferenciaBase = mb.ReferenciaBase
	where mt.Referencia = @pReferencia and (mb.Descripcion like '% gasket %' OR mb.Descripcion like '% gasket%' or mb.Descripcion like '% gaskt %'  or mb.Descripcion like '% gskt %'))
	BEGIN		
	 	set @sQuantity = CEILING(@sQuantity)		
	END

	--IF(@MaterialType = 'Meter')
	--set @sQuantity = CEILING(@sQuantity)

	IF NOT EXISTS (SELECT * FROM dbo.MaterialNeeds WHERE Reference = @pReferencia AND Number=@Number AND Version=@Version AND ElementId=@sLineId) OR @MaterialType = 'Surface'
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
	VALUES
	(   NEWID(), -- GUID - uniqueidentifier
	    @Number,    -- Number - int
	    @Version,    -- Version - int
	    -1,    -- ProductionSet - int
	    -1,    -- ReproductionNeedsCode - int
	     1,    -- MNSet - smallint
	    -1,    -- Position - int
	    -1,    -- SquareId - int
	    -1,    -- HoleId - int
	    CASE WHEN @MaterialType = 'Glass' THEN 'G'+RTRIM(CAST(@sLineId AS NVARCHAR(9)))
			 ELSE ''
			 END,  -- ElementId - nvarchar(10)
		CASE WHEN @MaterialType = 'Bar' THEN 1
			 WHEN @MaterialType = 'Meter' THEN 3
			 WHEN @MaterialType = 'Piece' THEN 2
			 WHEN @MaterialType = 'Glass' THEN 4
			 WHEN @MaterialType = 'Panel' THEN 4
			 WHEN @MaterialType = 'Surface' THEN 4
			 END,  -- TipoCalculo - nchar(15),    -- MaterialType - smallint
	    0,    -- Complex - smallint
	    @pReferencia,  -- Reference - nchar(25)
	    (SELECT TOP 1 CC.ConfigurationCode FROM dbo.Materiales M 
		INNER JOIN dbo.ColorConfigurations CC ON M.Color=CC.ColorName
		WHERE Referencia = @pReferencia),
	    0,    -- RawMaterialColorConfiguration - int
	    N'',  -- RawReference - nchar(25)
	    @sQuantity,  -- Quantity - float
	    @Width,  -- Length - real
	    @Height,  -- Height - real
	    0.0,  -- Volume - real
	   ISNULL((SELECT TOP 1 MaterialSupplierCode FROM dbo.Materiales WHERE Referencia = @pReferencia),990),    -- ProviderCode - int
	    ISNULL((SELECT TOP 1 Almacen FROM dbo.Materiales WHERE Referencia = @pReferencia),990),    -- WarehouseCode - smallint
	    N'',  -- XMLDoc - ntext
	    @pOnDemand,    -- AllowToOrder - smallint
	    @sQuantity,  -- QuantityToOrder - float
	    @sQuantity,  -- QuantityToDiscount - float
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
	    ISNULL(@sWeight,0)   -- Weight - float
	    )

END
------------------------------------------------------------------------------------------------------------------------


SET ANSI_NULLS ON
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SchuecoMaterialPurchaseData]    Script Date: 2024-12-29 21:19:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE    PROCEDURE [dbo].[Uniwave_SchuecoMaterialPurchaseData] 
	-- Add the parameters for the stored procedure here
	@pReferencia  NVARCHAR(25), 
	@pPackage real,
	@sPrice FLOAT,
	@sReference NVARCHAR(50),
	@sDescription NVARCHAR(255),
	@sColor NVARCHAR(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    /*If purchase data not exists, then insert it*/
	
	Declare @SchedulerTime int = 14

	if exists(
		select top 1 1 from Materiales mt
		join Perfiles pf on mt.ReferenciaBase = pf.ReferenciaBase
		where mt.Referencia = @pReferencia
	)
	BEGIN
		set @SchedulerTime = 28
	END

	--DECLARE @ExcahngeRate FLOAT
    
	--SELECT @ExcahngeRate = 1/ISNULL(Relacion,1) FROM Monedas WHERE Nombre = 'NOK'
	
	IF NOT EXISTS (SELECT *  FROM COMPRAS WHERE Referencia =@pReferencia )
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
	(   @pReferencia,       -- Referencia - nchar(25)
	    990,         -- Proveedor - int
	    1,         -- APartir - int
	    1,         -- UP1 - int
	    --1,       -- UP2 - float
		ISNULL(@pPackage,1),       -- UP2 - float
	    GETDATE(), -- FechaUltimaCompra - datetime
	    @sPrice,--*ISNULL(@ExcahngeRate,1),       -- PrecioUltimaCompra - float
	    @sReference,       -- ReferenciaProveedor - nchar(50)
	    ISNULL(@sDescription,@sReference),       -- SupplierDescription - nvarchar(255)
	    N'NOK',       -- Divisa - nchar(25)
	   NULL, -- FechaEVPrecioSC - datetime
	    NULL,       -- PrecioSC - float
	    N'NOK',       -- DivisaPrecioSC - nchar(25)
	    @SchedulerTime,         -- EntregaMedia - int
	    N'',       -- CodigoEAN13 - nchar(13)
	    N'',       -- DescripcionUP1 - nvarchar(50)
	    N'',       -- DescripcionUP2 - nvarchar(50)
	    1,         -- ByDefault - smallint
	    @SchedulerTime,         -- SchedulerTime - int
	    0          -- ReorderingTime - int
	    )
		----select * from Materiales where Referencia = '4030VD-u'

		update Materiales set PrecioCompraPonderado = @sPrice, DivisaPrecioCompraPonderado = 'NOK' where Referencia = @pReferencia

		DECLARE @ColorConfiguration INT, 
				@Length FLOAT
                

			SELECT  TOP 1  @ColorConfiguration = C.ConfigurationCode FROM dbo.ColorConfigurations C
			INNER JOIN Materiales M ON C.ColorName=M.Color WHERE M.Referencia =@pReferencia	
			

		    SELECT @Length = M.LongitudBarra  FROM dbo.Perfiles P
			INNER JOIN dbo.MaterialesBase MB ON P.ReferenciaBase=MB.ReferenciaBase
			INNER JOIN Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE M.Referencia =@pReferencia


		IF NOT EXISTS (SELECT * FROM dbo.MaterialLevels WHERE Reference=@pReferencia AND ColorConfiguration=@ColorConfiguration AND Warehouse=990 AND Length=ISNULL(@Length,0) AND Height=0)
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
		(   NEWID(), -- RowId - uniqueidentifier
		    @pReferencia,  -- Reference - nchar(25)
		    @ColorConfiguration ,    -- ColorConfiguration - int
		    990,    -- Warehouse - smallint
		    ISNULL(@Length,0) ,  -- Length - real
		    0.0,  -- Height - real
		    0.0,  -- Level1 - float
		    0.0   -- Level2 - float
		    )
		
		IF NOT EXISTS (SELECT *  FROM ReferenceSuppliers WHERE Reference =@pReferencia )
		INSERT INTO dbo.ReferenceSuppliers
		(
		    Reference,
		    SupplierCode,
		    Percentage
		)
		VALUES
		(   @pReferencia, -- Reference - nchar(25)
		    990,   -- SupplierCode - int
		    100  -- Percentage - float
		)

END

-------------------------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO


