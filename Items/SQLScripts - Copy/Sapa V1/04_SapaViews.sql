USE [Prefsuite]
GO

/****** Object:  View [dbo].[vwSAPA_OrdersMapping]    Script Date: 2024-12-29 21:14:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER VIEW [dbo].[vwSAPA_OrdersMapping]
AS
SELECT        sFullOrder AS [Order], sOrderBase + CASE WHEN pVersion = '0' THEN '' ELSE + '/' + CAST(CAST(pVersion AS int) + 1 AS nvarchar(3)) END AS pReference
FROM            dbo.SAPA_OrdersMapping

GO

	