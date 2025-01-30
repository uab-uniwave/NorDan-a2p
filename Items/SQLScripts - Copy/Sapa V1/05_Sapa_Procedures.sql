USE [Prefsuite]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPARemovePurchaseOrders]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPARemovePurchaseOrders]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPARemovePositions]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPARemovePositions]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPARemoveMaterialNeeds]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPARemoveMaterialNeeds]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAReferenceAutoDes]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAReferenceAutoDes]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterialPurchaseData]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAMaterialPurchaseData]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterialNeeds]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAMaterialNeeds]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterialColor]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAMaterialColor]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterial]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAMaterial]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertWorkforce]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAInsertWorkforce]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertPositions]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAInsertPositions]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertMNRecordPanels]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAInsertMNRecordPanels]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertMNRecordGlass]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAInsertMNRecordGlass]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertMNRecord]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAInsertMNRecord]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertCurrency]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPAInsertCurrency]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPA_SetOrderMapping]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPA_SetOrderMapping]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPA_EDI_GLASS]    Script Date: 2024-12-29 21:16:55 ******/
DROP PROCEDURE [dbo].[Uniwave_SAPA_EDI_GLASS]
GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPA_EDI_GLASS]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE    PROCEDURE [dbo].[Uniwave_SAPA_EDI_GLASS]
    @Number INT,
    @Numeration INT
AS
BEGIN

DECLARE @fil NVARCHAR(25)
DECLARE @path NVARCHAR(1000)
DECLARE @txt VARCHAR(MAX)
DECLARE @txt1 VARCHAR(MAX)
DECLARE @txt2 VARCHAR(MAX)
DECLARE @txt3 VARCHAR(MAX)

IF
(
    SELECT COUNT(*)
    FROM PurchasesSubDetail psd
        INNER JOIN MAterialNeeds mn
            ON psd.MaterialNeedId = mn.GUID
        INNER JOIN MaterialesPAF mp
            ON mp.Numero = mn.Number
               AND mp.Version = mn.Version
               AND mn.Position = mp.Orden
               AND mp.Referencia = mn.Reference
    WHERE psd.Number = @Number
          AND psd.Numeration = @Numeration
) = 0 AND @Numeration = 5
BEGIN

    SELECT @txt1
        = '*Purchase Order Number:' + LTRIM(RTRIM(CAST(@Number AS NCHAR(10)))) + ' Estimated receiption date:'
          + +RTRIM(LTRIM(REPLACE(CONVERT(VARCHAR(20), pr.EstimatedReceptionDate, 1), '/', ''))) + CHAR(13) + CHAR(10)
          + 'VENTA WINDOWS UAB |20|' + CHAR(13) + CHAR(10)
    FROM Purchases pr
    WHERE pr.Number = @Number
          AND pr.Numeration = @Numeration

    SELECT @txt2 = 'Total:' + RTRIM(CAST(SUM(CAST(Quantity AS INT)) AS VARCHAR(5)))
    FROM Uniwave_SAPA_EDI_Data(@Number, @Numeration) 

    SET @path = 'C:\\Temp\\1\\' + LTRIM(RTRIM(CAST(@Number AS NCHAR(10)))) + '.txt'

    SELECT @txt
        = ISNULL(@txt + CHAR(13) + CHAR(10), '') + CIP + '|' + Nomenclatura + '|' + PurchaseDetailId + '|'
          + ReferenceBase + '|' + Description + '|' + Quantity + '|' + Length + '|' + Height + '|' + ProductionLot
          + '|' + ''
    FROM Uniwave_SAPA_EDI_Data(@Number, @Numeration)

	SELECT @txt3 = 'DEFINITIONS:
* - Meaning insulated glass unit is shaped or with internal bars. Please look into pdf file;
W = warm spacer;
A = ALU spacer;
LowE = Sel = Low-emissivity;
U1.0 = CG = LowE1.0 = PO;
SR = sound reduction;
MAT = WM = frosted;
T = ESG = tought = tempered;
SSCl = Stopsol Classic Clear;
SSBr = Stopsol Classic Bronze;
Hydro = Bioclean = Active;
Antifog = Anti-condensation = Gfast = ViewClear;
FPBr = Bronze = Ton r = Antisol brown;
FPGr = Grey = Ton g = Antison grey'

    SELECT @txt = @txt1 + @txt + CHAR(13) + CHAR(10) + @txt2 + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + @txt3

    --SELECT @txt

    SELECT --ur.Name, 
        @path = ISNULL(x.SaveToFolderName, ''),
        @fil = 'SAPA_' + ISNULL(REPLACE(x.SaveToFileName, '<number>', LTRIM(RTRIM(CAST(@Number AS NCHAR(10))))), '')
    FROM
    (
        SELECT TOP 1
               x.SaveToFolderName,
               x.SaveToFileName
        FROM Uniwave_XsltProtocol x
            INNER JOIN Uniwave_Recipient ur
                ON x.RecipientId = ur.RowId
            INNER JOIN Proveedores p
                ON p.Nombre = ur.Name
            INNER JOIN Purchases pr
                ON pr.Number = @Number
                   AND pr.Numeration = @Numeration
                   AND p.CodigoProveedor = pr.ProviderCode
    ) x

    IF LEN(@path) > 1
       AND LEN(@fil) > 10
    BEGIN

        SET @path = @path + '\' + @fil
        --SELECT @path
        EXEC USR_WriteToFile @path, @txt, 1
    END
END

END 


GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPA_SetOrderMapping]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[Uniwave_SAPA_SetOrderMapping]
	-- Add the parameters for the stored procedure here
	@SFullOrder nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE 
  	    @sOrder NVARCHAR(50),
		@SOrderBase NVARCHAR(50),
		@SOrderVersion  NVARCHAR(50),
		@pVersion NVARCHAR(50)

IF NOT EXISTS(SELECT * FROM SAPA_OrdersMapping Where SFullOrder=@SFullOrder)
BEGIN
	SET @sOrder  =  SUBSTRING(@SFullOrder,0, CHARINDEX('_',@SFullOrder))

	if @sOrder = ''
	SET @sOrder= @SFullOrder

	SET @SOrderBase  =  SUBSTRING(@SOrder,0, CHARINDEX('-',@SOrder))
	SET @SOrderVersion  =  SUBSTRING(@sOrder,CHARINDEX('-',@sOrder)+1,LEN(@sOrder))
		IF  @SOrderBase = ''
		BEGIN
			Set @sOrderBase = @sOrder
			SET @SOrderVersion = 0 
			SET @pVersion =0
		END
		ELSE
		SELECT @pVersion =COUNT(*) From SAPA_OrdersMapping Where SOrderBase =@SOrderBase
	
	Insert Into SAPA_OrdersMapping
	SELECT @SFullOrder, @sOrder,@SOrderBase,@SOrderVersion,@pVersion

	
END
END

GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertCurrency]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Uniwave_SAPAInsertCurrency]
	@Number int,
	@Version Int, 
	@NOK Float, 
	@SEK Float,
	@GBP Float,
	@DKK Float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT * FROM PAFCurrencies Where Number = @Number and Version = @Version and Currency ='NOK')
	UPDATE PAFCurrencies SET Ratio = @NOK, RatioinLastCalculation =@NOK Where Number = @Number and Version = @Version and Currency ='NOK'
	ELSE 
	INSERT INTO PAFCurrencies (Number, Version, Currency,Ratio,RatioinLastCalculation ) VALUES(@Number,@Version,'NOK', @NOK, @NOK)

	IF EXISTS (SELECT * FROM PAFCurrencies Where Number = @Number and Version = @Version and Currency ='SEK')
	UPDATE PAFCurrencies SET Ratio = @SEK, RatioinLastCalculation =@SEK Where Number = @Number and Version = @Version and Currency ='SEK'
	ELSE 
	INSERT INTO PAFCurrencies (Number, Version, Currency,Ratio,RatioinLastCalculation ) VALUES(@Number,@Version,'SEK', @SEK, @SEK)

	IF EXISTS (SELECT * FROM PAFCurrencies Where Number = @Number and Version = @Version and Currency ='GBP')
	UPDATE PAFCurrencies SET Ratio = @GBP, RatioinLastCalculation =@GBP Where Number = @Number and Version = @Version and Currency ='GBP'
	ELSE 
	INSERT INTO PAFCurrencies (Number, Version, Currency,Ratio,RatioinLastCalculation ) VALUES(@Number,@Version,'GBP', @GBP, @GBP)

	IF EXISTS (SELECT * FROM PAFCurrencies Where Number = @Number and Version = @Version and Currency ='DKK')
	UPDATE PAFCurrencies SET Ratio = @DKK, RatioinLastCalculation =@DKK Where Number = @Number and Version = @Version and Currency ='DKK'
	ELSE 
	INSERT INTO PAFCurrencies (Number, Version, Currency,Ratio,RatioinLastCalculation ) VALUES(@Number,@Version,'DKK', @DKK, @DKK)

	IF EXISTS (SELECT * FROM PAFCurrencies Where Number = @Number and Version = @Version and Currency ='EUR')
	UPDATE PAFCurrencies SET Ratio = 1, RatioinLastCalculation =1 Where Number = @Number and Version = @Version and Currency ='EUR'
	ELSE 
	INSERT INTO PAFCurrencies (Number, Version, Currency,Ratio,RatioinLastCalculation ) VALUES(@Number,@Version,'EUR', 1, 1)


 END

GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertMNRecord]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		U
-- Create date: 
-- Description:	
-- =============================================
CREATE   PROCEDURE [dbo].[Uniwave_SAPAInsertMNRecord] 
	-- Add the parameters for the stored procedure here
	@sWName NVARCHAR(255),
	@sOrder NVARCHAR(50),
	@sReference NVARCHAR(25),
	@sColor NVARCHAR(255),
	@sC1 NVARCHAR(255),
	@sC2 NVARCHAR(255),
	@sC3 NVARCHAR(255),
	@sDescription NVARCHAR( 255), 
	@sQuantity INT,
	@sPackage NVARCHAR(20),
	@sGross FLOAT,
	@sNet FLOAT, 
	@sExcahnge FLOAT,
    @sm2 FLOAT, 
	@sWeight FLOAT,
	@sPrice FLOAT, 
	@sTotalPrice FLOAT,
	@sModified DATETIME

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO SAPA_RecordsMaterials (WorksheetName, [Order], ArticleNo,Color, Description, Quantity, Package, Gross, Net, Exchange,m2, Weight, Price, TotalPrice,Modified)
VALUES (@sWname, @sOrder, @sReference, @sColor,@sDescription,@sQuantity,@sPackage,@sGross,@sNet,@sExcahnge,@sm2,@sWeight,@sPrice,@sTotalPrice,@sModified)
 

 DECLARE @Prefix Nvarchar(5)
	
	IF @sWName = 'Default hardware supplier_2' 
	SET @Prefix = 'ASSA_'
	ELSE 
	SET @Prefix='SAPA_'

	DECLARE 

	@Number INT, 
	@Version INT,
	@pColor NVARCHAR(50) = RTRIM(@Prefix)+ @sColor ,
	@pPackage FLOAT,
	@pReferenciaBase NVARCHAR(25), 
	@pReferencia NVARCHAR(25), 
	@pAutoDes NVARCHAR(120),
	@MaterialType NVARCHAR(10),
	@pWidth FLOAT, 
    @pHeight FLOAT,
	@pToOrder smallint
	 
	IF NOT EXISTS (SELECT * FROM PAF WHERE Referencia =@sOrder)
	RETURN 0

	/*Identify Number and version of Order */
	SELECT @Number=Numero , @Version = Version FROM PAF WHERE Referencia =@sOrder

	/* Define , Material type */
	SELECT @MaterialType = CASE WHEN @sPackage COLLATE SQL_Latin1_General_CP1_CS_AS LIKE '%M%' THEN 'Bar'
	    					    WHEN @sPackage COLLATE SQL_Latin1_General_CP1_CS_AS LIKE  '%m%' THEN 'Meter'
							    WHEN @sPackage COLLATE SQL_Latin1_General_CP1_CS_AS LIKE  '%pcs%' THEN 'Piece'
								WHEN @sPackage COLLATE SQL_Latin1_General_CP1_CS_AS LIKE  '% x %' THEN 'Surface'
						        ELSE 'Piece' END
	
	/*Identify package */
	IF @MaterialType != 'Surface'
	SELECT @pPackage= CAST(SUBSTRING(@sPackage,1, CHARINDEX(' ',@sPackage)-1)AS FLOAT) 
	IF @MaterialType = 'Surface' 
	Begin
		SET @pPackage = 1 
		SET  @pWidth= CAST(SUBSTRING(@sPackage,1, CHARINDEX(' ',@sPackage)-1)AS FLOAT) 
		SET  @pHeight= CAST(SUBSTRING(@sPackage, CHARINDEX('x',@sPackage)+1,LEN(@sPackage)) AS FLOAT) 
	END 

	/*Identify referenciabase */
	IF SUBSTRING (@sReference,1,1)='S' AND @Prefix<>'ASSA_'
    SET @sReference = SUBSTRING(@sReference,2,24)
	SELECT @pReferenciaBase = LEFT(RTRIM(@Prefix)+@sReference,25)

	/*Identify referencia */

	IF @Prefix !='ASSA_'
	BEGIN 
	IF @MaterialType ='Bar' OR (@MaterialType ='Piece'  AND @Sc1 LIKE 'RAL%')
	BEGIN
		EXEC Uniwave_SAPAReferenceAutoDes @sC1 , @Sc2 ,@Sc3,'1'
		SELECT @pAutoDes = AutoDes FROM SAPA_ReferenceAutoDes WHERE C1=@Sc1 AND C2=@sC2 AND C3=@sC3 AND [Type]='1'
	END 
	ELSE 
	BEGIN 
		EXEC Uniwave_SAPAReferenceAutoDes @sC1 , @Sc2 ,@Sc3,'0'
		SELECT @pAutoDes = AutoDes FROM SAPA_ReferenceAutoDes WHERE C1=@Sc1 AND C2=@sC2 AND C3=@sC3 AND [Type]='0'
	END
	 
	END
		
	SELECT @pReferencia = LEFT(RTRIM(@Prefix)+@sReference + ISNULL(@pAutoDes,'') ,25)
	
	
	
	/*Identity of  Material on Demand or not By request of AG 13.11.2018*/
	SET @pToOrder =1
	IF (@MaterialType = 'Piece' OR  @MaterialType = 'Meter') AND  @sNet/@sGross <0.5  
	SET @pToOrder =0

						  

	/*Check color, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterialColor @sColor ,@Prefix

	/*Check material, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterial @pReferenciaBase, @pReferencia, @sDescription, @MaterialType, @pColor, @pPackage

	/*Check material, if not exists than insert*/
	/*======================================*/
	IF @MaterialType = 'Bar'
	EXEC dbo.Uniwave_SAPAMaterialPurchaseData @pReferencia,  1, @sPrice, @sReference ,@sDescription,@sColor
	ELSE 
	EXEC dbo.Uniwave_SAPAMaterialPurchaseData @pReferencia, @pPackage, @sPrice, @sReference ,@sDescription,@sColor

	DECLARE @mnWidth FLOAT,
			@mnHeight FLOAT,
            @mnWeight FLOAT,
            @mnQuantity FLOAT
	SELECT @mnWidth=CASE WHEN @MaterialType = 'Bar' THEN @pPackage*1000
						 WHEN @MaterialType = 'Meter' THEN 0
						 WHEN @MaterialType = 'Piece' THEN 0
						 WHEN @MaterialType = 'Surface' THEN @pWidth
					END
    SELECT @mnHeight=CASE WHEN @MaterialType = 'Bar' THEN 0
						  WHEN @MaterialType = 'Meter' THEN 0
						  WHEN @MaterialType = 'Piece' THEN 0
						  WHEN @MaterialType = 'Surface' THEN @pHeight
					 END  
	SELECT @mnWeight=CASE WHEN @MaterialType = 'Bar' THEN @sWeight
						 WHEN @MaterialType = 'Meter' THEN @sWeight
						 WHEN @MaterialType = 'Piece' THEN @sWeight
						 WHEN @MaterialType = 'Surface' THEN @sWeight
					END
    SELECT @mnQuantity=CASE WHEN @MaterialType = 'Bar' THEN @sQuantity
						  WHEN @MaterialType = 'Meter' THEN CEILING(@sNet)
						  WHEN @MaterialType = 'Piece' THEN @sNet
						  WHEN @MaterialType = 'Surface' THEN @sQuantity
					 END

	/*Check material needs, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterialNeeds @Number, @Version, @pReferencia, @MaterialType, @mnWidth, @mnHeight,@mnWeight,@mnQuantity,'', @pToOrder
	
	END




GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertMNRecordGlass]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		U
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAInsertMNRecordGlass] 
	-- Add the parameters for the stored procedure here
	@sOrder NVARCHAR(50),
	@sId NVARCHAR(25),
	@sLineId INT,
	@sProduct NVARCHAR(255),
	@sReference NVARCHAR(25),
	@sDescription NVARCHAR(255), 
	@sQuantity INT,
	@sWidth FLOAT,
	@sHeight FLOAT,
	@sPrice FLOAT,
	@sWeight FLOAT,
    @sm2pcs FLOAT, 
	@sm2Sum FLOAT, 
	@sm2Total FLOAT, 
	@sTotalPrice FLOAT,
	@sModified  DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
INSERT INTO [dbo].[SAPA_RecordsGlasses]
           ([Order]
           ,[Id]
		   ,[LineId]
           ,[Product]
           ,[ArticleNo]
           ,[Description]
		   ,[Quantity]
           ,[Width]
           ,[Height]
           ,[Price]
           ,[Weight]
           ,[m2pcs]
           ,[m2Total]
           ,[TotalPrice]
		   ,[Modified]
           )
     VALUES
           (@sOrder,
           @sId,
		   @sLineId,
           @sProduct,
           @sReference,
           @sDescription,
		   @sQuantity,
		   @sWidth,
           @sHeight,
           @sPrice,
           @sWeight,
           @sm2pcs,
           @sm2Total,
		   @sTotalPrice,
		   @sModified)
           
		   
	DECLARE 

	@Number INT, 
	@Version INT,
	@pColor NVARCHAR(50),
	@pPackage FLOAT,
	@pReferenciaBase NVARCHAR(25), 
	@pReferencia NVARCHAR(25), 
	@MaterialType NVARCHAR(10),
	@pDescription NVARCHAR(255)
	 
	
	IF NOT EXISTS (SELECT * FROM PAF WHERE Referencia =@sOrder)

	RETURN 0

	DECLARE @Prefix Nvarchar(5)
	SET @Prefix =''
	SET @pColor = RTRIM(@Prefix)+'Glass'
	/*Identify Number and version of Order */
	SELECT @Number=Numero , @Version = Version FROM PAF WHERE Referencia =@sOrder


		
	/* Define , Material type */
	SELECT @MaterialType = 'Glass'
	
	/*Identify package */
	SELECT @pPackage= 1



	SET @pDescription =  SUBSTRING(@sDescription ,1, CASE WHEN  CHARINDEX(',', @sDescription)>0 THEN  CHARINDEX(',', @sDescription)-1 ELSE LEN(@sDescription) END  ) 
	SET @pDescription = REPLACE (@pDescription, 'SR', ' SR')
	SET @pDescription = REPLACE (@pDescription, 'LowE 1.0', 'U1.0')
	SET @pDescription = REPLACE (@pDescription, 'SR LowE', 'Sel SR')
	SET @pDescription = REPLACE (@pDescription, 'Gfast', 'Antifog')
	SET @pDescription = REPLACE (@pDescription, '66.2 ESG VSG', '6T6T.2')
	SET @pDescription = REPLACE (@pDescription, '88.2 ESG VSG', '8T8T.2')
	SET @pDescription = REPLACE (@pDescription, 'Bioclean+SKN165', 'Bioclean SKN165')
	SET @pDescription = REPLACE (@pDescription, 'Bioclean+SKN176', 'Bioclean SKN176')
	SET @pDescription = REPLACE (@pDescription, 'LowE', 'Sel')
	SET @pDescription = REPLACE (@pDescription, '-F2-', '-2-')
	SET @pDescription = REPLACE (@pDescription, '-F3-', '-3-')
	SET @pDescription = REPLACE (@pDescription, '-F4-', '-4-')
	SET @pDescription = REPLACE (@pDescription, '-F5-', '-5-')
	SET @pDescription = REPLACE (@pDescription, '-F6-', '-6-')
	SET @pDescription = REPLACE (@pDescription, '-F7-', '-7-')
	SET @pDescription = REPLACE (@pDescription, '-F8-', '-8-')
	SET @pDescription = REPLACE (@pDescription, '-F9-', '-9-')
	SET @pDescription = REPLACE (@pDescription, '-F10-', '-10-')
	SET @pDescription = REPLACE (@pDescription, '-F2', '-2')
	SET @pDescription = REPLACE (@pDescription, '-F3', '-3')
	SET @pDescription = REPLACE (@pDescription, '-F4', '-4')
	SET @pDescription = REPLACE (@pDescription, '-F5', '-5')
	SET @pDescription = REPLACE (@pDescription, '-F6', '-6')
	SET @pDescription = REPLACE (@pDescription, '-F7', '-7')
	SET @pDescription = REPLACE (@pDescription, '-F8', '-8')
	SET @pDescription = REPLACE (@pDescription, '-F9', '-9')
	SET @pDescription = REPLACE (@pDescription, '-F10', '-10')
	SET @pDescription = REPLACE (@pDescription, 'F2-', '2-')
	SET @pDescription = REPLACE (@pDescription, 'F3-', '3-')
	SET @pDescription = REPLACE (@pDescription, 'F4-', '4-')
	SET @pDescription = REPLACE (@pDescription, 'F5-', '5-')
	SET @pDescription = REPLACE (@pDescription, 'F6-', '6-')
	SET @pDescription = REPLACE (@pDescription, 'F7-', '7-')
	SET @pDescription = REPLACE (@pDescription, 'F8-', '8-')
	SET @pDescription = REPLACE (@pDescription, 'F9-', '9-')
	SET @pDescription = REPLACE (@pDescription, 'F10-', '10-')

	
	/*Identify referenciabase */
	--SELECT @pReferenciaBase = LEFT(RTRIM(@Prefix)+@sReference,25)

	IF EXISTS(SELECT *  FROM dbo.MaterialesBase WHERE Descripcion = @pDescription)
	SELECT TOP 1 @pReferenciaBase =  ReferenciaBase FROM MaterialesBase WHERE Descripcion = @pDescription AND TipoCalculo = 'Superficies' AND Role='Glass'


	/*Identify referencia */
	SELECT @pReferencia = LEFT(RTRIM(@Prefix)+@sReference,25)

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
	EXEC dbo.Uniwave_SAPAMaterialColor 'Glass', @Prefix
	IF EXISTS(SELECT * FROM dbo.MaterialesBase MB 
	INNER JOIN dbo.Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE Descripcion = @sDescription)
	SELECT TOP 1 @pColor =  Color FROM dbo.MaterialesBase MB 
	INNER JOIN dbo.Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE Descripcion = @sDescription
	


	/*Check material, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterial @pReferenciaBase, @pReferencia, @sDescription, @MaterialType, @pColor, @pPackage

	/*Check purchase data, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterialPurchaseData @pReferencia, @pPackage, @sPrice, @sReference ,@sDescription,''


	/*Check material needs, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterialNeeds @Number, @Version, @pReferencia, @MaterialType, @sWidth,@sHeight,@sWeight,@sQuantity,@sLineId,1
	
	 
	END




GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertMNRecordPanels]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






--=============================================
-- Author:		U
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAInsertMNRecordPanels] 
	-- Add the parameters for the stored procedure here
	@sOrder NVARCHAR(50),
	@sId NVARCHAR(25),
	@sLineId INT,
	@sReference NVARCHAR(25),
	@sColor NVARCHAR(255),
	@sC1 NVARCHAR(255),
	@sC2 NVARCHAR(255),
	@sC3 NVARCHAR(255),
	@sWidth FLOAT,
	@sHeight FLOAT,
	@sQuantity INT,
	@sTotalArea FLOAT,
	@sUsedArea FLOAT,
	@sTotalWaste FLOAT,
	@sPrice FLOAT,
	@sCutSpecification Nvarchar(255),
	@sModified DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [dbo].[SAPA_RecordsPanels]
           ([Order]
           ,[Id]
		   ,LineId
           ,[ArticleNo]
		   ,[Color]
		   ,[C1]
		   ,[C2]
		   ,[C3]
           ,[Width]
           ,[Height]
           ,[Quantity]
           ,[TotalArea]
           ,[UsedArea]
           ,[TotalWaste]
           ,[Price]
           ,[CutSpecification]
		   ,[Modified]
            )

     VALUES
           (@sOrder
           ,@sId
		   ,@sLineId
           ,@sReference
		   ,@sColor
		   ,@sC1
		   ,@sC2
		   ,@sC3
           ,@sWidth
           ,@sHeight
		   ,@sQuantity
           ,@sTotalArea
           ,@sUsedArea
		   ,@sTotalWaste
		   ,@sPrice
		   ,@sCutSpecification
		   ,@sModified
)

	DECLARE 

	@Number INT, 
	@Version INT,
	@pColor NVARCHAR(50),
	@pPackage FLOAT,
	@pReferenciaBase NVARCHAR(25), 
	@pReferencia NVARCHAR(25), 
	@MaterialType NVARCHAR(10),
	@pAutoDes NVARCHAR(120)
	 
	
	IF NOT EXISTS (SELECT * FROM PAF WHERE Referencia =@sOrder)

	RETURN 0

	DECLARE @Prefix NVARCHAR(5) 

	IF SUBSTRING (@sReference,1,1)='S'
    SET @sReference = SUBSTRING(@sReference,2,24)


	IF @sReference LIKE '%XPS%'
	SET @Prefix ='LOB_'
	ELSE 
	SET @Prefix=''

	SET @pColor = RTRIM(@Prefix)+ 'Panel'


	/*Identify Number and version of Order */
	SELECT @Number=Numero , @Version = Version FROM PAF WHERE Referencia =@sOrder
	
	/* Define , Material type */
	SELECT @MaterialType = 'Panel'
	
	/*Identify package */
	SELECT @pPackage= 1
	
	IF @sReference LIKE 'AluSheet%'	
	begin
	EXEC Uniwave_SAPAReferenceAutoDes @sC1 , @Sc2 ,@Sc3,'1'
	SELECT @pAutoDes = AutoDes FROM SAPA_ReferenceAutoDes WHERE C1=@Sc1 AND C2=@sC2 AND C3=@sC3 AND [Type]='1'
	end
	ELSE 
	begin
	EXEC Uniwave_SAPAReferenceAutoDes @sC1 , @Sc2 ,@Sc3,'0'
	SELECT @pAutoDes = AutoDes FROM SAPA_ReferenceAutoDes WHERE C1=@Sc1 AND C2=@sC2 AND C3=@sC3 AND [Type]='0'
	end



	IF @sReference LIKE 'AluSheet%'	
	BEGIN
       SET  @Prefix = 'SAPA_'
		/*Identify referenciabase */
		SELECT @pReferenciaBase = LEFT(RTRIM(@Prefix)+@sReference + @pAutoDes ,25)

		/*Identify referencia */
		SELECT @pReferencia = LEFT(RTRIM(@Prefix)+@sReference + @pAutoDes ,25)

		SET @pColor = ISNULL(RTRIM(@Prefix),'')+ @sColor 
		
		/*Check color, if not exists than insert*/
		/*======================================*/
		EXEC dbo.Uniwave_SAPAMaterialColor @sColor ,@Prefix
		
		/*Check material, if not exists than insert*/
		/*======================================*/
		EXEC dbo.Uniwave_SAPAMaterial @pReferenciaBase, @pReferencia, @sReference, @MaterialType, @pColor, @pPackage		
		/*Check material, if not exists than insert*/
		/*======================================*/
		EXEC dbo.Uniwave_SAPAMaterialPurchaseData @pReferencia, @pPackage, @sPrice, @sReference ,@sReference,@pColor
	END 

	IF @sReference NOT LIKE 'AluSheet%'	
	BEGIN
		/*Identify referenciabase */
		SELECT @pReferenciaBase = LEFT(Rtrim(@Prefix)+@sReference,25)


		/*Identify referencia */
		SELECT @pReferencia = LEFT(Rtrim(@Prefix)+@sReference ,25)
			/*Check color, if not exists than insert*/
		/*======================================*/
		EXEC dbo.Uniwave_SAPAMaterialColor 'Panel' ,@Prefix

		/*Check material, if not exists than insert*/
		/*======================================*/
		EXEC dbo.Uniwave_SAPAMaterial @pReferenciaBase, @pReferencia, @sReference, @MaterialType, @pColor, @pPackage
		
		/*Check material, if not exists than insert*/
		/*======================================*/
		EXEC dbo.Uniwave_SAPAMaterialPurchaseData @pReferencia, @pPackage, @sPrice, @sReference ,@sReference,''
	END



	

	/*Check material needs, if not exists than insert*/
	/*======================================*/
	EXEC dbo.Uniwave_SAPAMaterialNeeds @Number, @Version, @pReferencia, @MaterialType, @sWidth,@sHeight,0,@sQuantity,@sLineId,1
	
    
	END




GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertPositions]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		U
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAInsertPositions] 
    
	@sOrder NVARCHAR(50),
	@sPhase NVARCHAR(50),
	@sProduct NVARCHAR(50),
	@sQuantity INT,
	@sWidth Float,
	@sHeight Float,
	@sWeight FLOAT,
	@sWeightWOGlass Float, 
	@sDirectMtrl Float,
	@sDirectLW Float,
	@sPrice FLOAT,
	@sTotal FLOAT, 
	@sPriceEUR FLOAT,
    @sTotalEUR FLOAT,
	@sDirectMtrlEUR Float,
	@sDirectLWEUR Float,
	@sdNumber int, 
	@sdVersion int,
	@sdSortOrder int,
	@sModified DateTime

	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO dbo.SAPA_RecordsPositions
(
    [Order],
    Phase,
    Product,
    Quantity,
    Width,
    Height,
    Weight,
    WeightWOGlass,
    DirectMtrl,
    DirectLW,
    Price,
    Total,
    PriceEUR,
    TotalEUR,
	DirectMtrlEUR,
    DirectLWEUR,
	Number,
	Version, 
	SortOrder,
	Modified
)
VALUES
(   @sOrder, -- Order - nvarchar(255)
    @sPhase, -- Phase - nvarchar(255)
    @sProduct, -- Product - nvarchar(255)
    @sQuantity,   -- Quantity - int
    @sWidth, -- Width - float
    @sHeight, -- Height - float
    @sWeight/@sQuantity, -- Weight - float
    @sWeightWOGlass/@sQuantity, -- WeightWOGlass - float
    @sDirectMtrl, -- DirectMtrl - float
    @sDirectLW, -- DirectLW - float
    @sPrice, -- Price - float
    @sTotal, -- Total - float
    @sPriceEUR, -- PriceEUR - float
    @sTotalEUR,  -- TotalEUR - float
    @sDirectMtrlEUR, -- DirectMtrl - float
    @sDirectLWEUR, -- DirectLW - float
	@sdNumber,
	@sdVersion,
	@sdSortOrder,
	@sModified
    ) 
	END





GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAInsertWorkforce]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE    PROCEDURE [dbo].[Uniwave_SAPAInsertWorkforce]
	-- Add the parameters for the stored procedure here
@Number INT, 
@Version INT, 
@sModified DATETIME 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	DECLARE @Order NVARCHAR(50),  
			@Orden INT,
			@Seconds INT , 
			@Quantity INT, 
			@DirectLW FLOAT
			
	
	DELETE FROM dbo.ManoObraPAF WHERE Numero =@Number AND Version =@Version
	SELECT @Order=Referencia FROM PAF WHERE Numero=@Number AND Version=@Version
	

	DECLARE mo CURSOR FOR SELECT DirectLW, Quantity FROM dbo.SAPA_RecordsPositions WHERE [Order]=@Order AND Modified=@sModified
	OPEN MO
	FETCH NEXT FROM MO INTO @DirectLW, @Quantity
	WHILE @@FETCH_STATUS = 0 
	BEGIN 
	SET @Seconds = 0
	
	SET @Seconds=ROUND((@DirectLW/94)*3600,0)
	
	SELECT @Orden =  isnull(MAX(Orden)+1,0) FROM ManoObraPAF WHERE numero=@Number AND Version = @Version  
	

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
	    N'SAPA', -- Puesto - nchar(30)
	    @Seconds/@Quantity, -- Segundos - real
	    0,   -- idWorkforce - int
	    0,   -- ProductTypeCode - int
	    0    -- ProcessingClassInstance - smallint
	    )
			FETCH NEXT FROM MO INTO  @DirectLW, @Quantity
END
CLOSE MO
DEALLOCATE MO
	END 


GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterial]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO










-- =============================================
-- Author:		MJ
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAMaterial] 
	-- Add the parameters for the stored procedure here
	@pReferenciaBase NVARCHAR(25),
	@pReferencia NVARCHAR(25),
	@sDescription NVARCHAR(255),
	@MaterialType NVARCHAR(10),
	@pColor NVARCHAR(50),
    @pPackage INT
	 
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
	    
		CASE WHEN @MaterialType = 'Panel'  AND @pReferenciaBase LIKE 'SAPA_AluSheet%' THEN @sDescription + ' ' + SUBSTRING(@pColor,6,44)
		     WHEN @MaterialType = 'Surface' AND @pReferenciaBase LIKE 'SAPA_AluSheet%' THEN @sDescription + ' ' + SUBSTRING(@pColor,6,44)
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
	    N'980 SAPA',  -- Nivel1 - nvarchar(150)
	    CASE WHEN @MaterialType = 'Bar' THEN '01 Bars'
			 WHEN @MaterialType = 'Meter' THEN '02 Meters'
			 WHEN @MaterialType = 'Piece' THEN '03 Pieces'
			 WHEN @MaterialType = 'Glass' THEN '04 Surfaces'
			 WHEN @MaterialType = 'Panel' THEN '04 Surfaces'
			 WHEN @MaterialType = 'Surface' THEN 'Superficies'
			 ELSE 'Pieces' END,  -- Nivel2 - nvarchar(150)
	    979,    -- CodigoProveedor - int
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
	    980,         -- Almacen - smallint
	    1,         -- UE1 - int
	    @pPackage,         -- UE2 - int
	    1,         -- ControlDeStock - smallint
	    1,         -- PedirBajoDemanda - smallint
	    0,         -- ManageRemnants - smallint
	    @pPackage*1000,       -- LongitudBarra - real
	    0.000000,      -- WastageAllowance - decimal(19, 6)
	    0,         -- UseWastageAllowanceInMN - smallint
	    0,         -- UseFullRodsInMN - smallint
	    0,         -- IsModel - smallint
	    1,         -- TargetLevel - int
	    0,         -- PrefShopStatus - smallint
	    0,         -- DefaultValue - smallint
	    979,         -- MaterialSupplierCode - int
	    1,         -- ProductionPreparationTime - int
	    14         -- AverageDeliveryTime - smallint
	    )
	
	IF @MaterialType = 'Bar'
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
	    1,  -- AnchoExterior - real
	    0.0,  -- AnchoInterior - real
	    1,  -- Altura - real
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

	IF @MaterialType = 'Meter'
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

	IF @MaterialType = 'Piece'
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
	
	IF @MaterialType = 'Glass' OR @MaterialType = 'Panel'  OR @MaterialType = 'Surface' 
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
    
	/* Possibly materialbase exists , but final reference not*/
	IF NOT EXISTS (SELECT * FROM Materiales WHERE Referencia = @pReferencia)
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
	    980,         -- Almacen - smallint
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
	    979,         -- MaterialSupplierCode - int
	    1,         -- ProductionPreparationTime - int
	    14         -- AverageDeliveryTime - smallint
	    )
	
END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterialColor]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MJ
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAMaterialColor] 
	-- Add the parameters for the stored procedure here
	@sColor nvarchar(50) ,
	@Prefix NVARCHAR(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE 

	@pColor NVARCHAR(50) = ISNULL(RTRIM(@Prefix),'')+ @sColor 
    

	/*Check color, if not exists than insert*/
	/*======================================*/
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
	    '980 SAPA',  -- Nivel1 - nvarchar(150)
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
	    N'_SAPA',  -- Family - nchar(25)
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



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterialNeeds]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MJ
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAMaterialNeeds] 
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


	IF NOT EXISTS(SELECT * FROM dbo.Proveedores WHERE CodigoProveedor = 979)
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
	    979,         -- CodigoProveedor - int
	    N'SAPA',       -- Nombre - nvarchar(60)
	    N'NOK',       -- Divisa - nchar(25)
	    N'NOK'       -- Divisa2 - nchar(25)
	    )
		IF NOT EXISTS (SELECT * FROM dbo.Almacenes WHERE Codigo =979)
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
		(   979,   -- Codigo - smallint
		    N'SAPA', -- Descripcion - nvarchar(60)
		    0,   -- Externo - smallint
		    979,   -- ProviderCode - int
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
	   ISNULL((SELECT TOP 1 MaterialSupplierCode FROM dbo.Materiales WHERE Referencia = @pReferencia),979),    -- ProviderCode - int
	    ISNULL((SELECT TOP 1 Almacen FROM dbo.Materiales WHERE Referencia = @pReferencia),979),    -- WarehouseCode - smallint
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
	    @sWeight   -- Weight - float
	    )

END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAMaterialPurchaseData]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE    PROCEDURE [dbo].[Uniwave_SAPAMaterialPurchaseData] 
	-- Add the parameters for the stored procedure here
	@pReferencia  NVARCHAR(25), 
	@pPackage INT,
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
	

	DECLARE @ExcahngeRate FLOAT
    
	--Removed conversion by Request of Arunas Gulbinas 2021-10-09
	--SELECT @ExcahngeRate = 1/ISNULL(Relacion,1) FROM Monedas WHERE Nombre = 'NOK'
	SELECT @ExcahngeRate = 1 FROM Monedas WHERE Nombre = 'NOK'

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
	    979,         -- Proveedor - int
	    1,         -- APartir - int
	    1,         -- UP1 - int
	    --1,       -- UP2 - float
		@pPackage,       -- UP2 - float
	    GETDATE(), -- FechaUltimaCompra - datetime
	    @sPrice*ISNULL(@ExcahngeRate,1),       -- PrecioUltimaCompra - float
	    @sReference,       -- ReferenciaProveedor - nchar(50)
	    ISNULL(@sDescription,@sReference) +' '+ @sColor,       -- SupplierDescription - nvarchar(255)
	    N'NOK',       -- Divisa - nchar(25)
	   NULL, -- FechaEVPrecioSC - datetime
	    NULL,       -- PrecioSC - float
	    N'NOK',       -- DivisaPrecioSC - nchar(25)
	    14,         -- EntregaMedia - int
	    N'',       -- CodigoEAN13 - nchar(13)
	    N'',       -- DescripcionUP1 - nvarchar(50)
	    N'',       -- DescripcionUP2 - nvarchar(50)
	    1,         -- ByDefault - smallint
	    14,         -- SchedulerTime - int
	    0          -- ReorderingTime - int
	    )


		DECLARE @ColorConfiguration INT, 
				@Length FLOAT
                

			SELECT  TOP 1  @ColorConfiguration = C.ConfigurationCode FROM dbo.ColorConfigurations C
			INNER JOIN Materiales M ON C.ColorName=M.Color WHERE M.Referencia =@pReferencia

		    SELECT @Length = M.LongitudBarra  FROM dbo.Perfiles P
			INNER JOIN dbo.MaterialesBase MB ON P.ReferenciaBase=MB.ReferenciaBase
			INNER JOIN Materiales M ON MB.ReferenciaBase=M.ReferenciaBase WHERE M.Referencia =@pReferencia


		IF NOT EXISTS (SELECT * FROM dbo.MaterialLevels WHERE Reference=@pReferencia AND ColorConfiguration=@ColorConfiguration AND Warehouse=980 AND Length=ISNULL(@Length,0) AND Height=0)
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
		    980,    -- Warehouse - smallint
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
		    979,   -- SupplierCode - int
		    100  -- Percentage - float
		)

END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPAReferenceAutoDes]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPAReferenceAutoDes] 

	@sC1 NVARCHAR(255),
	@sC2 NVARCHAR(255),
	@sC3 NVARCHAR(255),
	@Type NVARCHAR(10)
	
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ColorIndex INT, 
			@Gloss INT, 
			@AutoDes NVARCHAR(120)
	
	IF NOT EXISTS (SELECT * FROM dbo.SAPA_ReferenceAutoDes WHERE C1=@sC1  AND C2=@sC2 AND C3=@sC3 AND [Type]=@Type)
	BEGIN
		IF (@sC1 LIKE 'RAL%' OR @sC1 LIKE '%N'  OR @sC1 LIKE '%Y')  AND @sC2 ='' AND @Type='1'
			SELECT @ColorIndex =ISNULL(MAX(ColorIndex),1000)+1  FROM SAPA_ReferenceAutoDes WHERE ColorIndex BETWEEN 1000 AND 2999
		
		IF (@sC1 LIKE 'RAL%' OR @sC1 LIKE '%N'  OR @sC1 LIKE '%Y') AND (@sC2 LIKE 'RAL%' OR @sC2 LIKE '%N'  OR @sC2 LIKE '%Y')  AND @Type='1'
			SELECT @ColorIndex =ISNULL(MAX(ColorIndex),3000)+1  FROM SAPA_ReferenceAutoDes WHERE ColorIndex BETWEEN 3000 AND 4999

		IF @sC1 !=''  AND(@sC1 NOT LIKE 'RAL%' AND  @sC1 NOT LIKE '%N'  and @sC1 NOT LIKE '%Y') AND @Type='1'
			SELECT @ColorIndex =ISNULL(MAX(ColorIndex),5000)+1  FROM SAPA_ReferenceAutoDes WHERE ColorIndex BETWEEN 5000 AND 6999
				
		IF @Type != '1'
			SELECT @ColorIndex = ISNULL(MAX(ColorIndex),0)+1 FROM SAPA_ReferenceAutoDes WHERE ColorIndex BETWEEN 0 AND 99
		
		/*PC2 Second segment for bar materials */
		SET @Gloss  = CASE WHEN @sC3 LIKE 'GL%' THEN CAST(SUBSTRING (@sC3,3,3) AS INT)
						ELSE 0
						END
		
		SELECT  @AutoDes = CASE WHEN @Type='1' AND LEN((ISNULL(@sC1,'')+ISNULL(@sC2,'')+ISNULL(@sC3,''))) > 1 THEN '_'+RIGHT('0000'+CAST(ISNULL(@ColorIndex,0) AS VARCHAR(4)),4)+'_'+RIGHT('00'+CAST(ISNULL(@Gloss,0) AS VARCHAR(2)),2)+'00'
		                   ELSE '_0000'+'_'+RIGHT('00'+CAST(ISNULL(@Gloss,0) AS VARCHAR(2)),2)+RIGHT('00'+CAST(@ColorIndex AS VARCHAR(2)),2)
						   END

			INSERT INTO dbo.SAPA_ReferenceAutoDes
		(
			C1,
			C2,
			C3, 
			[Type], 
			ColorIndex,
			AutoDes
		)
		VALUES
		(   @sC1, -- C1 - nvarchar(25)
			@sC2, -- C2 - nvarchar(25)
			@sC3,
			@Type,  -- C3 - nvarchar(25)
			@ColorIndex,
			@AutoDes
			)

		END
		
		

	

	
	
END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPARemoveMaterialNeeds]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MJ
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPARemoveMaterialNeeds] 
	-- Add the parameters for the stored procedure here
	@sOrder NVARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE 
	@Number INT, 
	@Version INT
	SELECT @Number=Numero, @Version =Version FROM PAF WHERE referencia =@sOrder
	DELETE FROM dbo.MaterialNeeds WHERE Number=@Number AND Version=@Version
	DELETE FROM dbo.MaterialNeedsMaster WHERE Number=@Number AND Version=@Version
END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPARemovePositions]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MJ
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPARemovePositions] 
	-- Add the parameters for the stored procedure here
	@sOrder NVARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE 
	@Number INT, 
	@Version INT
	SELECT @Number=Numero, @Version =Version FROM PAF WHERE referencia =@sOrder
	DELETE FROM dbo.ContenidoPAF WHERE Numero=@Number AND Version=@Version
END



GO

/****** Object:  StoredProcedure [dbo].[Uniwave_SAPARemovePurchaseOrders]    Script Date: 2024-12-29 21:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MJ
-- Create date: 
-- Description:	
-- =============================================
CREATE    PROCEDURE [dbo].[Uniwave_SAPARemovePurchaseOrders] 
	-- Add the parameters for the stored procedure here
	@sOrder NVARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE 
	@RowId UNIQUEIDENTIFIER, 
	@pNumber INT,
	@pNumeration INT

	SELECT TOP 1 @RowId=RowId FROM PAF WHERE referencia =@sOrder
	
	DECLARE PR CURSOR FAST_FORWARD FOR
	
	SELECT Number, Numeration FROM Purchases WHERE DocumentId IN (
	SELECT  DestDocumentId FROM dbo.DocumentRelationships WHERE SrcDocumentId=@RowId AND DestDocumentType =4)





	OPEN PR 
	FETCH NEXT FROM PR INTO @pNumber, @pNumeration
	WHILE @@FETCH_STATUS =0
	BEGIN

		DELETE FROM dbo.PurchasesSubDetail WHERE Number =@pNumber AND  Numeration=@pNumeration
		DELETE FROM dbo.PurchasesDetail WHERE Number =@pNumber AND  Numeration=@pNumeration
		DELETE FROM dbo.Purchases WHERE Number =@pNumber AND  Numeration=@pNumeration
        
	FETCH NEXT FROM PR INTO @pNumber, @pNumeration
	END
	CLOSE PR
	DEALLOCATE PR
END



GO


