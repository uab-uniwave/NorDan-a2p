SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetColorConfiguration] 
	(
		-- Add the parameters for the function here
		@Color NVARCHAR(50)
	)
	RETURNS INT
	AS
	BEGIN
		-- Declare the return variable here
		DECLARE @ConfigurationCode INT = 0
		SELECT Top 1  @ConfigurationCode = ConfigurationCode FROM ColorConfigurations WHERE ColorName = @Color and innerColor  is null and outerColor  is null
		RETURN @ConfigurationCode

	END


GO

CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetExternalReference] 
(
	-- Add the parameters for the function here
	@PrefSuiteReference nvarchar (25)
	
	
	)
RETURNS nvarchar (25)
AS
BEGIN
	DECLARE @ExternalReference nvarchar (25)

	-- Add the T-SQL statements to compute the return value here
  SELECT @ExternalReference =  [ExternalReference]
  FROM [dbo].[UniwaveApi_Mapping] WHERE EntityType=1 and PrefSuiteReference = @PrefSuiteReference and ExternalSourceName='BC'

	-- Return the result of the function
	RETURN @ExternalReference

END
GO

	CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetOrderState] 
	(
		-- Add the parameters for the function here
		@Order NVARCHAR(50)
	)
	RETURNS INT
	AS
	BEGIN
		-- Declare the return variable here
		DECLARE @Status INT = 0
		DECLARE @Number INT , @Version INT , @RowId UNIQUEIDENTIFIER 
		
		SELECT @Number=Numero , @Version = Version, @RowId=RowId FROM PAF WHERE Referencia =@Order

		IF EXISTS(SELECT * FROM PAF WHERE Referencia = @Order )
		SET @Status = @Status | 1;
		
		IF EXISTS(SELECT * FROM Uniwave_a2p_Items WHERE [Order] =  @Order and DeletedUTCDateTime IS NULL)
		SET @Status = @Status | 2;
		
		IF EXISTS(SELECT * FROM Uniwave_a2p_Materials WHERE [Order] =  @Order and DeletedUTCDateTime IS NULL)
		SET @Status = @Status | 4;
		
		IF EXISTS(SELECT * FROM ContenidoPAF WHERE [Numero]= @Number and [Version]= @Version)
		SET @Status = @Status | 8;
		
		IF EXISTS(SELECT * FROM MaterialNeeds WHERE [Number]= @Number and [Version]= @Version)
		SET @Status = @Status | 16;
			
		IF EXISTS (	SELECT Number, Numeration FROM Purchases WHERE DocumentId IN (
		SELECT  DestDocumentId FROM dbo.DocumentRelationships WHERE SrcDocumentId=@RowId AND DestDocumentType =4))

		SET @Status = @Status | 32;

		RETURN @Status

	END


GO



CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetSalesDocumentState] 
	(
		-- Add the parameters for the function here
		@Number int,
		@Version Int
	)
	RETURNS INT
	AS
	BEGIN
		-- Declare the return variable here
		DECLARE  @Status INT = 0,
				 @RowId Uniqueidentifier,
				 @Referencia nvarchar(50);
		
		SELECT @RowId=RowId FROM PAF WHERE Numero=@Number and [Version] = @Version

		IF EXISTS(SELECT * FROM PAF WHERE Numero=@Number and [Version] = @Version)
		SET @Status = @Status | 1;
		
		IF EXISTS(SELECT * FROM Uniwave_a2p_Items WHERE [Order] =  @Referencia and DeletedUTCDateTime IS NULL)
		SET @Status = @Status | 2;
		
		IF EXISTS(SELECT * FROM Uniwave_a2p_Materials WHERE [Order] =  @Referencia and DeletedUTCDateTime IS NULL)
		SET @Status = @Status | 4;
		
		IF EXISTS(SELECT * FROM ContenidoPAF WHERE [Numero]= @Number and [Version]= @Version)
		SET @Status = @Status | 8;
		
		IF EXISTS(SELECT * FROM MaterialNeeds WHERE [Number]= @Number and [Version]= @Version)
		SET @Status = @Status | 16;
			
		IF EXISTS (	SELECT Number, Numeration FROM Purchases WHERE DocumentId IN (
		SELECT  DestDocumentId FROM dbo.DocumentRelationships WHERE SrcDocumentId=@RowId AND DestDocumentType =4))

		SET @Status = @Status | 32;

		RETURN @Status

	END


GO


CREATE OR ALTER FUNCTION [dbo].[uniwave_a2p_GetSapaColor]
(
	@TechDesignColor nvarchar(50)
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @SapaColor nvarchar(50)
	SELECT TOP 1 @SapaColor ='SAPA_'+SapaLogicColor FROM Nordan_a2p_ColorMapping Where TechDesignColor = @TechDesignColor 
	RETURN @SapaColor
END
GO

CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetSapaPrefsuiteReference] 
(
	-- Add the parameters for the function here
	@TechDesignReference nvarchar (25),
	@SapaColor nvarchar(60)
	
	)
RETURNS nvarchar (60)
AS
BEGIN
	DECLARE @Reference nvarchar (25)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Reference = Referencia from Materiales Where ReferenciaBase =  Replace(@TechDesignReference, 'S','SAPA_') and Color =@SapaColor

	-- Return the result of the function
	RETURN @Reference

END
GO

CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetTechDesignCommodityCode]
(
 @SourceReference Nvarchar(50)
)
RETURNS int
AS
BEGIN
	DECLARE @CommodityCode Nvarchar(25), 
			 @Code INT 
	 
	SELECT TOP 1 @CommodityCode =  [Default commodity code]
	FROM Nordan_a2p_IntrastatData WHERE 
	CASE 
		WHEN CHARINDEX('.', Material) > 0 
		THEN LEFT(Material, CHARINDEX('.', Material) - 1)
		ELSE Material
	END  = @SourceReference
	
	SELECT TOP 1 @Code= Id FROM Intrastat.CommodityCodes Where Code = @CommodityCode


RETURN @Code

END
GO

CREATE OR ALTER FUNCTION [dbo].[Uniwave_a2p_GetTechDesignWeight]
(
 @SourceReference Nvarchar(50)
)
RETURNS decimal
AS
BEGIN
	DECLARE @Weight decimal (38,6) 
	SELECT TOP 1 @Weight = Weight
	FROM Nordan_a2p_IntrastatData NINT
	WHERE 

	CASE 
		WHEN CHARINDEX('.', NINT.Material) > 0 
		THEN LEFT(NINT.Material, CHARINDEX('.', NINT.Material) - 1)
		ELSE NINT.Material
	END =  @SourceReference

RETURN @Weight

END


GO




CREATE OR ALTER FUNCTION [dbo].[Uniwave_SAPA_EDI_Data] ( @PurchaseNumber INT, @PurchaseNumeration INT)
RETURNS @FP TABLE
    (
      CIP CHAR(17) NOT NULL ,
      Nomenclatura CHAR(26) NOT NULL ,
	  PurchaseDetailId CHAR(3) NOT NULL ,
	  ReferenceBase CHAR(6) NOT NULL ,
	  Description CHAR(60) NOT NULL , -- 50
	  Quantity CHAR(3) NOT NULL ,
	  Length CHAR(4) NOT NULL ,
	  Height CHAR(5) NOT NULL ,
	  ProductionLot CHAR(6) NOT NULL ,
	  ProductionSet CHAR(1) NOT NULL 
    )
    BEGIN
     
	 INSERT INTO @FP
	 SELECT 
	 CAST(ISNULL(P.Referencia,'') AS CHAR(17)),
	 CAST(ISNULL(ISNULL(RG.Product,''),UM.Item) AS CHAR(26)),
	 CAST(ISNULL(psd.LineID,0) + 1 AS CHAR(3)),
	 CAST(ISNULL(mn.Reference,'') AS CHAR(6)),
	 CAST(CASE WHEN LEN(ISNULL(lc.Translation,'')) = 0 THEN ISNULL(mb.Descripcion,'') ELSE lc.Translation END AS CHAR(50))+
	 CASE WHEN PATINDEX('%[024689]W%', mb.Descripcion)>0 OR PATINDEX('%[024689]W%', lc.Translation)>0 THEN  '|RAL9005   '
	 WHEN PATINDEX('%[024689]A%', mb.Descripcion)>0 OR PATINDEX('%[024689]A%', lc.Translation)>0 THEN  '|ALU       '
	 ELSE '|---------' END,
	 CAST(CAST(ROUND(ISNULL(mn.Quantity,0),0) AS INT) AS CHAR(3)),
	 CAST(CAST(ROUND(ISNULL(mn.Length,0),0) AS INT) AS CHAR(4)),
	 CAST(CAST(ROUND(ISNULL(mn.Height,0),0) AS INT) AS CHAR(5)),
	 CAST('NaN' AS CHAR(6)),
	 ''q
	 FROM    dbo.PurchasesSubDetail AS psd
	        INNER JOIN dbo.PurchasesDetail AS pd ON pd.Numeration = psd.Numeration AND pd.Number = psd.Number AND pd.Id = psd.LineID
			INNER JOIN dbo.Numeraciones AS NR ON psd.Numeration = NR.id AND  NR.DocumentType = 2
            INNER JOIN dbo.MaterialNeeds AS mn ON mn.GUID = psd.MaterialNeedId
            INNER JOIN dbo.PAF p ON mn.Number = p.Numero
                              AND mn.Version = p.Version
			INNER JOIN dbo.Materiales m ON m.Referencia=mn.Reference
			INNER JOIN dbo.MaterialesBase mb ON m.ReferenciaBase=mb.ReferenciaBase
			INNER JOIN dbo.Superficies sup ON sup.ReferenciaBase=m.ReferenciaBase
			LEFT OUTER JOIN LanguageContent lc ON mb.RowId = lc.ElementRowId AND lc.TableName = 'MaterialesBase' AND lc.FieldName = 'Descripcion'
 AND lc.LanguageId = 1063
			 LEFT OUTER JOIN (SELECT * FROM SAPA_RecordsGlasses R 
							  INNER JOIN (SELECT SRG.[Order] Order2, SOM.pReference,  SRG.LineId LineId2, MAX(SRG.Modified) DateTime2 
							  FROM SAPA_RecordsGlasses SRG --coment Datatime change to [Modified]  2019-06-11
							  INNER JOIN vwSAPA_OrdersMapping SOM ON SRG.[Order] = SOM.[Order]
							  GROUP BY SRG.[Order], SRG.LineId, SOM.pReference) t ON R.LineId = t.LineId2 AND R.Modified = T.DateTime2 AND r.[Order] = t.Order2 --coment Datatime change to [Modified] 2019-06-11
							  ) RG ON 
							  p.Referencia = RG.pReference AND mn.ElementId = 'G'+LTRIM(RTRIM(CAST(RG.LineId AS VARCHAR(4)))) 
		 
			 LEFT OUTER Join Uniwave_a2p_Materials UM ON psd.MaterialNeedId = UM.RowId
	WHERE psd.Number = @PurchaseNumber AND psd.Numeration = @PurchaseNumeration --((sup.Tipo=0 AND sup.Composite=1) OR sup.Tipo=2) AND  
	ORDER BY psd.LineID

        RETURN 
    END
