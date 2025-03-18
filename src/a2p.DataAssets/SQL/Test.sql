





DECLARE @Number INT = 88200048

--====================================================================
-- ITEMS
--====================================================================
--Order Items
select * from Uniwave_a2p_Items where salesDocumentNumber=@Number and  DeletedUTCDateTime is   null
Select * From ContenidoPAF where numero = @Number 

--Cross check 
SELECT * From ContenidoPAF Where IdPos in 
(
Select Distinct IdPos from ContenidoPAF cp 
Inner Join Paf on cp.Numero = PAF.Numero
Inner Join Uniwave_a2p_Items it on  PAF.Referencia = it.[order] and it.DeletedUTCDateTime is null
)


--Order Items from previouse imports 
select * from Uniwave_a2p_Items where salesDocumentNumber=@Number and  DeletedUTCDateTime is not null 

-- material Need
SELECT * FROM MaterialNeeds Where Number  = @Number 

select * from Uniwave_a2p_Materials   
select * from Colores Where Nivel1 like '988%'
select * from ColorConfigurations Where ColorName not In (SELECT Nombre FROM Colores Where  Nivel1 like '988%')
select * from MaterialesBase Where Nivel1 like '988%'
select * from Materiales Where ReferenciaBase  In (SELECT ReferenciaBase FROM  MaterialesBase  Where Nivel1 like '988%')
select * from Piezas Where ReferenciaBase  In (SELECT ReferenciaBase FROM  MaterialesBase  Where Nivel1 like '988%')
select * from Perfiles Where ReferenciaBase  In (SELECT ReferenciaBase FROM  MaterialesBase Where Nivel1 like '988%')
select * from Metros Where ReferenciaBase  In (SELECT ReferenciaBase FROM  MaterialesBase  Where Nivel1 like '988%')
select * from Superficies Where ReferenciaBase  In (SELECT ReferenciaBase FROM  MaterialesBase  Where Nivel1 like '988%')
