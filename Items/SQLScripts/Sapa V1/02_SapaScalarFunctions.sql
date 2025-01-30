USE [Prefsuite]
GO

/****** Object:  UserDefinedFunction [dbo].[Uniwave_SAPAGetOrderState]    Script Date: 2024-12-29 21:18:33 ******/
DROP FUNCTION [dbo].[Uniwave_SAPAGetOrderState]
GO

/****** Object:  UserDefinedFunction [dbo].[Uniwave_SAPAGetColor]    Script Date: 2024-12-29 21:18:33 ******/
DROP FUNCTION [dbo].[Uniwave_SAPAGetColor]
GO

/****** Object:  UserDefinedFunction [dbo].[Uniwave_SAPAGetColor]    Script Date: 2024-12-29 21:18:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







	-- =============================================
	-- Author:		Name
	-- Create date: 
	-- Description:	
	-- =============================================
	CREATE FUNCTION [dbo].[Uniwave_SAPAGetColor] 
	(
		-- Add the parameters for the function here
		@sColor NVARCHAR(50)
	)
	RETURNS NVARCHAR(50)
	AS
	BEGIN
		-- Declare the return variable here
		DECLARE @Result NVARCHAR(50)

		SELECT @Result = ISNULL(OldColor,@sColor)  FROM SAPA_ColorsMapping WHERE NewColor= @sColor

		RETURN @Result

	END


GO

/****** Object:  UserDefinedFunction [dbo].[Uniwave_SAPAGetOrderState]    Script Date: 2024-12-29 21:18:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





	-- =============================================
	-- Author:		Name
	-- Create date: 
	-- Description:	
	-- =============================================
	CREATE FUNCTION [dbo].[Uniwave_SAPAGetOrderState] 
	(
		-- Add the parameters for the function here
		@sOrder NVARCHAR(50)
	)
	RETURNS INT
	AS
	BEGIN
		-- Declare the return variable here
		DECLARE @Result INT
		DECLARE @Number INT , @Version INT , @RowId UNIQUEIDENTIFIER 
		SELECT @Number=Numero , @Version = Version, @RowId=RowId FROM PAF WHERE Referencia =@sOrder

		IF NOT EXISTS(SELECT * FROM PAF WHERE Referencia = @sOrder )
		RETURN 0

		IF NOT EXISTS (SELECT * FROM MaterialNeeds WHERE Number=@Number AND Version=@Version)
		SELECT @Result = 1
	
		IF EXISTS (SELECT * FROM dbo.MaterialNeeds WHERE Number=@Number AND Version=@Version)
		SELECT @Result =2
	
		IF EXISTS (	SELECT Number, Numeration FROM Purchases WHERE DocumentId IN (
		SELECT  DestDocumentId FROM dbo.DocumentRelationships WHERE SrcDocumentId=@RowId AND DestDocumentType =4))
		SELECT @Result =3

		IF EXISTS (SELECT * FROM dbo.ContenidoPAF WHERE Numero=@Number AND Version=@Version)
		SELECT @Result =4

		RETURN @Result

	END


GO


