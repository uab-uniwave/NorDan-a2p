Delete From ContenidoPAF Where IdPos in 
(
Select Distinct IdPos from ContenidoPAF cp 
Inner Join Paf on cp.Numero = PAF.Numero
Inner Join Uniwave_a2p_Items it on  PAF.Referencia = it.[order]
)

Delete From MaterialNeedsMaster Where  [number] in ( SELECT SalesDocumentNumber  FROM Uniwave_a2p_Items )
Delete From MaterialNeeds Where  [number] in ( SELECT SalesDocumentNumber  FROM Uniwave_a2p_Items )
Delete from Uniwave_a2p_Items
Delete from Uniwave_a2p_Materials   
Delete from Colores Where  Nivel1 like '988%'
Delete from ColorConfigurations Where ColorName not In (SELECT Nombre FROM Colores)
Delete from MaterialesBase Where Nivel1 like '988%'
Delete from Materiales Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Piezas Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Perfiles Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Metros Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Superficies Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
DELETE FROM Compras WHERE Referencia NOT IN (SELECT Referencia FROM Materiales)	
delete from Compras Where Proveedor = 988 
DELETE FROM ReferenceSuppliers WHERE Reference NOT IN (SELECT Referencia FROM Materiales)
DELETE FROM ReferenceSuppliers WHERE SupplierCode = 988


--Attention NavisionCodes and UniwaveApi_Mapping

--DELETE FROM NavisionCodes WHERE  PrefsuiteReference NOT IN (SELECT Referencia FROM Materiales)	
--DELETE FROM UniwaveApi_Mapping WHERE PrefsuiteReference NOT IN (SELECT Referencia FROM Materiales)	 and EntityType = 1

DELETE FROM MaterialLevels WHERE Reference NOT IN (SELECT Referencia FROM Materiales)
DELETE FROM Uniwave_a2p_ReferenceMappingLog