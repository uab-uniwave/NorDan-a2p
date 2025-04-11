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

