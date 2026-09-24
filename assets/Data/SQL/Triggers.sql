SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER  TRIGGER [dbo].[TI_NavisionMateriales] 
   ON  [dbo].[Materiales] 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;


DECLARE  @Referencia nvarchar(25),
		 @RowId UNIQUEIDENTIFIER,
		 @Max INT

SELECT @Max = MAX(CAST(SUBSTRING(ExternalReference,5,16)AS INT)) FROM UniwaveApi_Mapping Where  EntityType = 1 and LEN(ExternalReference)=10

DECLARE MT CURSOR FOR SELECT [RowId],[Referencia] FROM Inserted
OPEN MT
FETCH NEXT FROM MT INTO @RowId, @Referencia
While @@FETCH_STATUS = 0
BEGIN

IF(@RowId IS NOT NULL AND @RowId IS NOT NULL )
BEGIN
IF NOT EXISTS (SELECT * FROM UniwaveApi_Mapping WHERE PrefSuiteReference = @Referencia AND EntityType = 1)
	BEGIN
		SET @Max=@Max+1

		INSERT INTO  UniwaveApi_Mapping (RowId, ExternalSourceName, EntityType, PrefSuiteRowId, PrefSuiteReference, ExternalReference)
			SELECT newId(),'BC', 1, @RowId, @Referencia,
			 CASE WHEN mb.Nivel1 = '980 SAPA' OR Nivel1 = '990 Schueco' OR Nivel1 = '988 TechDesign'
			 THEN 'ALU_' 	
			 ELSE 'NAV_' END + RTRIM(CAST(@Max AS  NVARCHAR(10))) 
			FROM Inserted i 
			INNER JOIN MaterialesBase mb ON i.ReferenciaBase = mb.ReferenciaBase 
			Where Referencia = @Referencia 
		/* for backward comnpatability insert same value into NAvsison Codes Table 
		*/
		Insert Into NavisionCodes (PrefsuiteReference, NavisionReference)
			SELECT  @referencia,
			 CASE WHEN mb.Nivel1 = '980 SAPA' OR Nivel1 = '990 Schueco' OR Nivel1 = '988 TechDesign'
			 THEN 'ALU_' 	
			 ELSE 'NAV_' END + RTRIM(CAST(@Max AS  NVARCHAR(10))) 
			FROM Inserted i 
			INNER JOIN MaterialesBase mb ON i.ReferenciaBase = mb.ReferenciaBase 
			Where Referencia = @Referencia
		END
	ELSE 
		BEGIN
			UPDATE UniwaveApi_Mapping Set PrefSuiteRowId = @RowId,  LastModifiedDate = GETDATE() Where PrefSuiteReference = @Referencia AND EntityType = 1
		END
END

	FETCH NEXT FROM MT INTO @RowId, @Referencia 
END 
CLOSE MT
DEALLOCATE MT
END

