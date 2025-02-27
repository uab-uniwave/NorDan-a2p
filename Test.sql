SELECT TOP 14 * FROM PAF ORDER BY [Number] DESC 
SELECT * FROM Uniwave_a2p_Items




Delete From ContenidoPAF Where IdPos in 
(
Select Distinct IdPos from ContenidoPAF cp 
Inner Join Paf on cp.Numero = PAF.Numero
Inner Join Uniwave_a2p_Items it on  PAF.Referencia = it.[order]
)


Delete from Uniwave_a2p_Items
Delete from Uniwave_a2p_Materials   
Delete from Colores Where Nivel1 like '988%'
Delete from ColorConfigurations Where ColorName not In (SELECT Nombre FROM Colores)
Delete from MaterialesBase Where Nivel1 like '988%'
Delete from Materiales Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Piezas Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Perfiles Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Metros Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
Delete from Superficies Where ReferenciaBase not In (SELECT ReferenciaBase FROM  MaterialesBase)
