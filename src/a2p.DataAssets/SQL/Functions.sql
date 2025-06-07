/****** Object:  UserDefinedFunction [dbo].[Uniwave_a2p_GetColorConfiguration]    Script Date: 6/7/2025 1:59:35 PM ******/
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

/****** Object:  UserDefinedFunction [dbo].[Uniwave_a2p_GetExternalReference]    Script Date: 6/7/2025 1:59:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

/****** Object:  UserDefinedFunction [dbo].[Uniwave_a2p_GetSalesDocumentState]    Script Date: 6/7/2025 1:59:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

/****** Object:  UserDefinedFunction [dbo].[uniwave_a2p_GetSapaColor]    Script Date: 6/7/2025 1:59:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

/****** Object:  UserDefinedFunction [dbo].[Uniwave_a2p_GetSapaReference]    Script Date: 6/7/2025 1:59:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[Uniwave_a2p_GetSapaReference] 
(
	-- Add the parameters for the function here
	@TechDesignReference nvarchar (25),
	@SapaColor nvarchar(60)
	
	)
RETURNS nvarchar (60)
AS
BEGIN
	DECLARE @SapaReference nvarchar (25)

	-- Add the T-SQL statements to compute the return value here
	SELECT @SapaReference = Referencia from Materiales Where ReferenciaBase =  Replace(@TechDesignReference, 'S','SAPA_') and Color =@SapaColor

	-- Return the result of the function
	RETURN @SapaReference

END
GO


