
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
	SELECT TOP 1 @SapaColor ='SAPA_'+SapaLogicColor FROM Uniwave_a2p_ColorMapping Where TechDesignColor = @TechDesignColor 
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
RETURNS Float
AS
BEGIN
    DECLARE @Weight Float 
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


