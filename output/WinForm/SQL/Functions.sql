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


USE [Prefsuite_Nordan_Development]
GO

/****** Object:  UserDefinedFunction [a2p].[fn_GetColorConfiguration]    Script Date: 27/09/2026 16:02:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--====================================================================
-- FUNCTIONS
--====================================================================

--====================================================================
-- Function: a2p.fn_GetColorConfiguration 
-- Returns the ConfigurationCode for a simple (non-complex) color.
-- Complex colors have InnerColor or OuterColor defined.
--====================================================================
CREATE OR ALTER   FUNCTION [a2p].[fn_GetColorConfiguration]
(
    @Color NVARCHAR(50)
)
RETURNS INT
AS
BEGIN
    DECLARE @ConfigurationCode INT = 0

    -- Get first matching simple color configuration (no inner/outer color)
    SELECT TOP 1
        @ConfigurationCode = [ConfigurationCode]
    FROM [dbo].[ColorConfigurations]
    WHERE [ColorName] = @Color
        AND [InnerColor] IS NULL
        AND [OuterColor] IS NULL

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



--====================================================================
-- Function: a2p.fn_GetSalesDocumentState 
-- [NEW IN THIS SCRIPT - present in Functions_old.sql but missing from the
--  supplied Routines.sql; added back with the a2p naming convention]
-- Returns bitwise flags indicating sales document processing state.
-- Uses same bit flags as a2p.fn_GetOrderState.
-- NOTE: @Referencia is declared but never assigned in the original legacy
--       code, so the "Items imported" / "Materials imported" checks below
--       will never match. Preserved as-is from Functions_old.sql - flagging
--       for the app team to confirm before this is relied upon.
--====================================================================
CREATE OR ALTER   FUNCTION [a2p].[fn_GetSalesDocumentState]
(
    @Number INT,
    @Version INT
)
RETURNS INT
AS
BEGIN
    DECLARE @Status INT = 0,
            @RowId UNIQUEIDENTIFIER,
            @Referencia NVARCHAR(50)

    -- Get PAF RowId for the sales document
    SELECT
        @RowId = [RowId]
    FROM [dbo].[PAF]
    WHERE [Numero] = @Number
        AND [Version] = @Version

    -- Check if PAF exists
    IF EXISTS (
        SELECT 1
        FROM [dbo].[PAF]
        WHERE [Numero] = @Number
            AND [Version] = @Version
    )
        SET @Status = @Status | 1 -- Bit 0: PAF exists

    -- Check if items exist for this order
    IF EXISTS (
        SELECT 1
        FROM [a2p].[Items] I
        INNER JOIN [a2p].[Orders] O ON I.[OrderId] = O.[Id]
        WHERE O.[OrderNumber] = @Referencia
    )
        SET @Status = @Status | 2 -- Bit 1: Items imported

    -- Check if materials exist for this order
    IF EXISTS (
        SELECT 1
        FROM [a2p].[Materials] M
        INNER JOIN [a2p].[Orders] O ON M.[OrderId] = O.[Id]
        WHERE O.[OrderNumber] = @Referencia
    )
        SET @Status = @Status | 4 -- Bit 2: Materials imported

    -- Check if PAF content exists
    IF EXISTS (
        SELECT 1
        FROM [dbo].[ContenidoPAF]
        WHERE [Numero] = @Number
            AND [Version] = @Version
    )
        SET @Status = @Status | 8 -- Bit 3: PAF content exists

    -- Check if material needs exist
    IF EXISTS (
        SELECT 1
        FROM [dbo].[MaterialNeeds]
        WHERE [Number] = @Number
            AND [Version] = @Version
    )
        SET @Status = @Status | 16 -- Bit 4: Material needs calculated

    -- Check if purchase orders exist
    IF EXISTS (
        SELECT 1
        FROM [dbo].[Purchases]
        WHERE [DocumentId] IN (
            SELECT [DestDocumentId]
            FROM [dbo].[DocumentRelationships]
            WHERE [SrcDocumentId] = @RowId
                AND [DestDocumentType] = 4 -- Purchase order document type
        )
    )
        SET @Status = @Status | 32 -- Bit 5: Purchase orders created

    RETURN @Status
END
GO



/****** Object:  UserDefinedFunction [a2p].[fn_GetSapaColor]    Script Date: 27/09/2026 15:58:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--====================================================================
-- Function: a2p.fn_GetSapaColor 
-- Converts TechDesign color to SAPA color format with 'SAPA_' prefix
--====================================================================
CREATE OR ALTER   FUNCTION [a2p].[fn_GetSapaColor]
(
    @TechDesignColor NVARCHAR(50)
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @SapaColor NVARCHAR(50)

    -- Map TechDesign color to SAPA color with prefix
    SELECT TOP 1
        @SapaColor = 'SAPA_' + [SapaLogicColor]
    FROM [a2p].[NorDan_ColorMapping]
    WHERE [TechDesignColor] = @TechDesignColor

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
--====================================================================
-- Function: a2p.fn_GetMaterialCommodityCode 
-- Returns the Intrastat commodity code ID for a TechDesign/Sapa/Schuco material reference
--====================================================================
CREATE ALTER   FUNCTION [a2p].[fn_GetMaterialCommodityCode]
(
    @SourceReference NVARCHAR(50),
    @SourceApplication NVARCHAR(50)
)
RETURNS INT
AS
BEGIN
    DECLARE @CommodityCode NVARCHAR(25),
            @Code INT

    IF @SourceApplication = 'TechDesign'
    BEGIN
        -- Get commodity code from Intrastat data, stripping any decimal suffix from material code
        SELECT TOP 1
            @CommodityCode = CommodityCode
        FROM [a2p].[NorDan_TechDesign]
        WHERE material = @SourceReference

        -- Get the internal commodity code ID
        SELECT TOP 1
            @Code = [Id]
        FROM [Intrastat].[CommodityCodes]
        WHERE [Code] = @CommodityCode
    END

    IF @SourceApplication = 'TechDesign'
    BEGIN
        -- Get commodity code from Intrastat data, stripping any decimal suffix from material code
        SELECT TOP 1
            @CommodityCode = CommodityCode
        FROM [a2p].[NorDan_TechDesign]
        WHERE
        CASE
            WHEN CHARINDEX('.', Material) > 0
            THEN LEFT(Material, CHARINDEX('.', Material) - 1)
            ELSE Material
        END = @SourceReference

        -- Get the internal commodity code ID
        SELECT TOP 1
            @Code = [Id]
        FROM [Intrastat].[CommodityCodes]
        WHERE [Code] = @CommodityCode
    END

    IF @SourceApplication = 'Schuco'
    BEGIN
        -- Get commodity code from Intrastat data, stripping any decimal suffix from material code
        SELECT TOP 1
            @CommodityCode = CommodityCode
        FROM [a2p].[NorDan_Schuco]
        WHERE material = @SourceReference

        -- Get the internal commodity code ID
        SELECT TOP 1
            @Code = [Id]
        FROM [Intrastat].[CommodityCodes]
        WHERE [Code] = @CommodityCode
    END

    IF @SourceApplication = 'Sapa'
    BEGIN
        -- Get commodity code from Intrastat data, stripping any decimal suffix from material code
        SELECT TOP 1
            @CommodityCode = CommodityCode
        FROM [a2p].[NorDan_SapaLogic]
        WHERE
        CASE
            WHEN CHARINDEX('.', Material) > 0
            THEN LEFT(Material, CHARINDEX('.', Material) - 1)
            ELSE Material
        END = @SourceReference

        -- Get the internal commodity code ID
        SELECT TOP 1
            @Code = [Id]
        FROM [Intrastat].[CommodityCodes]
        WHERE [Code] = @CommodityCode
    END

    RETURN @Code
END


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



/*
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
	 CAST(ISNULL(RG.[Product],ISNULL(UM.Item,'')) AS CHAR(26)),
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
*/