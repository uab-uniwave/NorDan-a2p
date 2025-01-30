USE [Prefsuite]
GO

/****** Object:  UserDefinedFunction [dbo].[Uniwave_SAPA_EDI_Data]    Script Date: 2024-12-29 21:17:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER FUNCTION [dbo].[Uniwave_SAPA_EDI_Data] ( @PurchaseNumber INT, @PurchaseNumeration INT)
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
	 CAST(ISNULL(RG.Product,'') AS CHAR(26)),
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
	WHERE psd.Number = @PurchaseNumber AND psd.Numeration = @PurchaseNumeration --((sup.Tipo=0 AND sup.Composite=1) OR sup.Tipo=2) AND  
	ORDER BY psd.LineID

        RETURN 
    END
GO


