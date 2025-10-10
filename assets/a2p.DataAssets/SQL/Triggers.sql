SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE OR  ALTER TRIGGER [dbo].[TI_NavisionMateriales] 
   ON  [dbo].[Materiales] 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

DECLARE @referencia nvarchar(25)
DECLARE @rowId UNIQUEIDENTIFIER

DECLARE MT CURSOR FOR SELECT [RowId],[Referencia] FROM Inserted
OPEN MT
FETCH NEXT FROM MT INTO @RowId, @Referencia 
While @@FETCH_STATUS = 0
BEGIN

DECLARE @Max INT 
SELECT @Max = MAX(CAST(SUBSTRING(ExternalReference,5,16)AS INT)) FROM UniwaveApi_Mapping Where  EntityType = 1 and LEN(ExternalReference)=10

Insert Into UniwaveApi_Mapping (RowId, ExternalSourceName, EntityType, PrefSuiteRowId, PrefSuiteReference, ExternalReference)
	SELECT newId(),'BC', 1, @RowId, @referencia,
	 CASE WHEN mb.Nivel1 = '980 SAPA' OR Nivel1 = '990 Schueco'
	 THEN 'ALU_' 	
	 ELSE 'NAV_' END + RTRIM(CAST(@MAx+1 AS  NVARCHAR(10))) 
	FROM Inserted i 
	INNER JOIN MaterialesBase mb ON i.ReferenciaBase = mb.ReferenciaBase and Nivel1 not Like '988%'
	Where Referencia = @Referencia
/* for backward comnpatability insert same value into NAvsison Codes Table 
*/
Insert Into NavisionCodes (PrefsuiteReference, NavisionReference)
	SELECT  @referencia,
	 CASE WHEN mb.Nivel1 = '980 SAPA' OR Nivel1 = '990 Schueco'
	 THEN 'ALU_' 	
	 ELSE 'NAV_' END + RTRIM(CAST(@MAx+1 AS  NVARCHAR(10))) 
	FROM Inserted i 
	INNER JOIN MaterialesBase mb ON i.ReferenciaBase = mb.ReferenciaBase and Nivel1 not Like '988%'
	Where Referencia = @Referencia
	FETCH NEXT FROM MT INTO @RowId, @Referencia 
END 
CLOSE MT
DEALLOCATE MT
END
GO


