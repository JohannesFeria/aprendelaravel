----------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: EJECUTAR OBJETOS DE BASE DE DATOS
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Creación		: 09/04/2019
-- Modificado por			: Ian Pastor M.
-- Nro. Orden de Trabajo	: 11934
-- Descripción				: -
----------------------------------------------------------------------------------------------------------------------------------------
USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log
PRINT 'INICIO --- > Actualizar Campo: activo con valor 01'
UPDATE CentrosCosto 
SET Activo = '01'
PRINT 'FIN --- > Actualizar Campo: activo con valor 01'
PRINT '[dbo].[RetornarImporteCuentas_PagarCobrar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RetornarImporteCuentas_PagarCobrar]') AND type in (N'F', N'FN'))
DROP FUNCTION [dbo].[RetornarImporteCuentas_PagarCobrar]
GO
----------------------------------------------------------------------------------------------------
-- Objetivo	: Lista la cartera para mandatos
----------------------------------------------------------------------------------------------------
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11934
-- Descripcion del Cambio	: Creacion de la funcion. Obtencion de CxC y CxP en dolares y soles
----------------------------------------------------------------------------------------------------
create FUNCTION [dbo].[RetornarImporteCuentas_PagarCobrar]( 
	@p_CodigoPortafolioSBS varchar(10),
	@p_FechaOperacion numeric(8,0),
	@p_CodigoMoneda varchar(10),
	@p_CodigoTipoOperacion varchar(2)
)
RETURNS numeric(22,7)
AS
BEGIN
	 declare @Resultado numeric(22,7) = 0
	set @Resultado = (
		Select isnull(  SUM( 
		(ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
		/ dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@p_CodigoMoneda,@p_FechaOperacion)
	),0)
	FROM OrdenInversion OI
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion AND O.CodigoTipoOperacion = @p_CodigoTipoOperacion
		JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion 
			AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END 
			AND FechaOperacion <= @p_FechaOperacion
			AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,TC.Fecha)
		JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
	WHERE OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		AND OI.FechaLiquidacion > @p_FechaOperacion
		AND OI.CodigoMoneda = @p_CodigoMoneda
		AND OI.Estado = 'E-CON' 
		AND OI.Situacion = 'A' 
		AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR')
		)

	RETURN  @Resultado
END
GO

GRANT EXECUTE ON [dbo].[RetornarImporteCuentas_PagarCobrar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[SP_SIT_LIS_ValorCuota_Mandatos]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_SIT_LIS_ValorCuota_Mandatos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_SIT_LIS_ValorCuota_Mandatos]
GO
----------------------------------------------------------------------------------------------------
-- Objetivo	: Lista la cartera para mandatos
----------------------------------------------------------------------------------------------------
-- Parßmetros:
--	1.- @p_CodigoPortafolioSBS - IN - varchar(20) - Codigo Portafolio
--	2.- @p_FechaOperacion - IN - numeric(8,0) - Fecha Operación
----------------------------------------------------------------------------------------------------
-- Fecha de Creación	    : 13/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11876
-- Descripci¾n del Cambio	: Obtencion de valor cuota para mandatos 
----------------------------------------------------------------------------------------------------
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11934
-- Descripci¾n del Cambio	: Visualizacion de CxP y CxC en dolares y soles
----------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[SP_SIT_LIS_ValorCuota_Mandatos](
	@p_CodigoPortafolioSBS varchar(20),
	@p_FechaOperacion numeric(8)
)
AS
BEGIN
	DECLARE @ultimaFechaCaja NUMERIC(22,7)
	SELECT TOP 1 @ultimaFechaCaja = FechaOperacion FROM SaldosBancarios
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND FechaOperacion <= @p_FechaOperacion
	ORDER BY FechaOperacion DESC

	--Lista los portafolios no seriados
	SELECT portafolio = p.Descripcion,
	       FechaProceso = dbo.FormatDate(vc.FechaProceso),
	       Serie = '',
	       CuotasEmitidas = vc.ValCuotaCierre,
	       ImporteRecaudado = vc.AportesValores,
	       rescates = vc.RescateValores,
	       valorCuota = vc.ValCuotaValoresCierre,
	       comisionAdministradora = vc.ComisionSAFM,
	       totalInversiones = vc.InversionesSubTotal,
	       --cxc = vc.CXCPreCierre, --vc.CXCVentaTitulo + VC.OtrasCXC,
		    cxc = [dbo].[RetornarImporteCuentas_PagarCobrar](@p_CodigoPortafolioSBS,@p_FechaOperacion,SB.CodigoMoneda ,'2'),
	       cxppre = vc.CXPPreCierre, --VC.CXPCompraTitulo + vc.OtrasCXP,
	       --cxpcierre = vc.CXPCierre, --vc.OtrasCXPCierre,
		    cxpcierre = [dbo].[RetornarImporteCuentas_PagarCobrar](@p_CodigoPortafolioSBS,@p_FechaOperacion,SB.CodigoMoneda ,'1'),
	       cxppreImporteRecaudado = vc.OtrasCXP - vc.AportesValores,
	       SB.SaldoDisponibleFinalConfirmado AS Caja,
	       patrimonioPreCierre = vc.ValPatriPreCierre1,
	       patrimonioCierre = vc.ValPatriCierreValores,	       
	       vc.OtrosGastos,
	       vc.OtrosIngresos,
	       p.Descripcion + '_' + dbo.FormatDate(vc.FechaProceso),
	       p.CodigoMoneda as CodigoMonedaFondo,
		   SB.CodigoMoneda AS CodigoMonedaCta,
		   SB.NumeroCuenta
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN SaldosBancarios SB ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND SB.FechaOperacion = VC.FechaProceso
	WHERE (vc.CodigoPortafolioSBS = @p_CodigoPortafolioSBS or @p_CodigoPortafolioSBS = '') 
	  AND vc.FechaProceso = @ultimaFechaCaja AND p.PorSerie = 'N'
	UNION
	--Lista los portafolios seriados
	SELECT portafolio = p.Descripcion,
	       FechaProceso = dbo.FormatDate(vc.FechaProceso),
	       Serie = ISNULL(PS.NombreSerie,''),
	       CuotasEmitidas = vc.ValCuotaCierre,
	       ImporteRecaudado = vc.AportesValores,
	       rescates = vc.RescateValores,valorCuota = vc.ValCuotaValoresCierre,
	       comisionAdministradora = vc.ComisionSAFM,
	       totalInversiones = vc.InversionesSubTotal,
	       --cxc = vc.CXCPreCierre,--vc.CXCVentaTitulo + VC.OtrasCXC,
		   cxc = [dbo].[RetornarImporteCuentas_PagarCobrar](@p_CodigoPortafolioSBS,@p_FechaOperacion,SB.CodigoMoneda ,'2'),
	       cxppre = vc.CXPPreCierre,-- VC.CXPCompraTitulo + vc.OtrasCXP,
	       --vc.CXPCierre,--vc.OtrasCXPCierre,
		    cxpcierre = [dbo].[RetornarImporteCuentas_PagarCobrar](@p_CodigoPortafolioSBS,@p_FechaOperacion,SB.CodigoMoneda ,'1'),
	       cxppreImporteRecaudado = vc.OtrasCXP - vc.AportesValores,
	       SB.SaldoDisponibleFinalConfirmado AS Caja,
	       patrimonioPreCierre = vc.ValPatriPreCierre1,
	       patrimonioCierre = vc.ValPatriCierreValores,	       
	       vc.OtrosGastos,
	       vc.OtrosIngresos,
	       p.Descripcion + '_' + dbo.FormatDate(vc.FechaProceso),
	       p.CodigoMoneda as CodigoMonedaFondo,
		   SB.CodigoMoneda AS CodigoMonedaCta,
		   SB.NumeroCuenta
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN PortafolioSerie PS ON PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS AND VC.CodigoSerie = PS.CodigoSerie
	JOIN SaldosBancarios SB ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND SB.FechaOperacion = VC.FechaProceso
	WHERE (vc.CodigoPortafolioSBS = @p_CodigoPortafolioSBS or @p_CodigoPortafolioSBS = '')
	  AND vc.FechaProceso = @ultimaFechaCaja AND p.PorSerie = 'S'
END
GO

GRANT EXECUTE ON [dbo].[SP_SIT_LIS_ValorCuota_Mandatos] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[ReporteGestion_ReporteConsolidado]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReporteGestion_ReporteConsolidado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReporteGestion_ReporteConsolidado]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: GENERAR REPORTE CONSOLIDADO
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 04/09/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11590
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 27/12/2018
-- Modificado por: Carlos Rumiche
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Modificación campos
-------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: Se agregaron nuevos campos al reporte como ActividadEconomica.
-----------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 29/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11934
-- Descripcion del cambio: Correccion deposito a plazo, que no aparezcan las ordenes que ya han sido vencidas.
-----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ReporteGestion_ReporteConsolidado]
(
	 @p_CodigoPortafolio VARCHAR(100)
	,@p_FechaProceso NUMERIC(8)
)
-- WITH RECOMPILE
AS
BEGIN
	/***************************************************CABECERA***************************************************/
	
	-- OPERACIONES DE RENTA FIJA
	SELECT DISTINCT	VA.CodigoPortafolioSBS,P.Descripcion AS DescripcionPortafolio
		,TI.CodigoTipoInstrumentoSBS,TI.Descripcion AS DescripcionTipoInstrumento
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, 'DOL', @p_FechaProceso) AS TipoCambio
		
	FROM ValorizacionAmortizada VA
		INNER JOIN Valores V ON VA.CodigoNemonico = V.CodigoNemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		LEFT JOIN ParametrosGenerales PG ON PG.Clasificacion = 'RATING' AND V.Rating = PG.Valor
		INNER JOIN Portafolio P ON VA.CodigoPortafolioSBS = P.CodigoPortafolioSBS
	--WHERE VA.CodigoPortafolioSBS = '8' AND FechaProceso = 20180827
	WHERE VA.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND FechaProceso = @p_FechaProceso
	--ORDER BY VA.CodigoPortafolioSBS,P.Descripcion
	UNION
	--DEPÓSITOS A PLAZO
	SELECT DISTINCT	CTV.CodigoPortafolioSBS,P.Descripcion AS DescripcionPortafolio
		,TI.CodigoTipoInstrumentoSBS,TI.Descripcion AS DescripcionTipoInstrumento
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, 'DOL', @p_FechaProceso) AS TipoCambio
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND OI.FechaOperacion = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
	WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		--AND CTV.FechaValoracion = @p_FechaProceso
		AND OI.FechaContrato > @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
	UNION
	--OPERACIONES DE RENTA VARIABLE
	SELECT
		OI.CodigoPortafolioSBS,P.Descripcion AS DescripcionPortafolio
		,TI.CodigoTipoInstrumentoSBS,TI.Descripcion AS DescripcionTipoInstrumento
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, 'DOL',@p_FechaProceso) AS TipoCambio
	FROM OrdenInversion OI
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND OI.FechaOperacion = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
		AND EXISTS(
			SELECT 1 FROM CarteraTituloValoracion CTV WHERE CTV.CodigoMnemonico = OI.CodigoMnemonico
			AND CTV.FechaValoracion = @p_FechaProceso AND CTV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS)
	ORDER BY 1,2
	
	
	/**********************************************OPERACIONES DETALLADAS*******************************************/
	--OPERACIONES DE RENTA FIJA
	SELECT
		VA.CodigoPortafolioSBS
		,P.Descripcion AS DescripcionPortafolio
		,TI.CodigoTipoInstrumentoSBS
		,TI.Descripcion AS DescripcionTipoInstrumento
		,VA.CodigoNemonico
		,V.CodigoISIN
		--,V.CodigoMoneda
		,M.SinonimoISO as CodigoMoneda
		,PG.Nombre AS Rating
		,V.FechaVencimiento
		,CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_FechaFinCuponActual
			WHEN 'A_VENCI' THEN VA.TIRCOM_FechaFinCuponActual
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_FechaFinCuponActual
		END AS ProximoCuponFecha
		,CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_PagoCuponVigente
			WHEN 'A_VENCI' THEN VA.TIRCOM_PagoCuponVigente
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_PagoCuponVigente
		END AS ProximoCuponMonto

		,VA.CantidadEnStock AS CantidadOperacion
		
		,--dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) * /* Pasamos a MONEDA LOCAL NSOL*/	
			CASE OI.TipoValorizacion
				WHEN 'DIS_VENTA' THEN VA.VTA_ValorNominal
				WHEN 'A_VENCI' THEN VA.TIRCOM_ValorNominal
				WHEN 'VAL_RAZO' THEN VA.TIRRAZ_ValorNominal
			END AS MontoNominalInicial
				
		,--dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) * /* Pasamos a MONEDA LOCAL NSOL*/		
			 CASE OI.TipoValorizacion
				WHEN 'DIS_VENTA' THEN VA.VTA_SaldoNominalVigente
				WHEN 'A_VENCI' THEN VA.TIRCOM_SaldoNominalVigente
				WHEN 'VAL_RAZO' THEN VA.TIRRAZ_SaldoNominalVigente
			END AS MontoNominalVigente
		
		,OI.PrecioNegociacionLimpio AS PrecioOperacion
		--,VA.NEG_ValorActual AS ValorCompra
				
		,'ValorCompra' = (OI.PrecioNegociacionLimpio / 100)
			* (CASE OI.TipoValorizacion
				WHEN 'DIS_VENTA' THEN VA.VTA_SaldoNominalVigente
				WHEN 'A_VENCI' THEN VA.TIRCOM_SaldoNominalVigente
				WHEN 'VAL_RAZO' THEN VA.TIRRAZ_SaldoNominalVigente
			END)
			* dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
				
		,VA.TIRCOM_PrecioLimpio
		
		,'TIRCOM_ValorActual' = VA.TIRCOM_ValorActual  
			* dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
			
		,VA.TIRRAZ_PrecioLimpio
		
		,'TIRRAZ_ValorActual' = VA.TIRRAZ_ValorActual 
		* dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
		,'PRELIM_ValorActual' = VA.PRELIM_ValorActual
			* dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
			
		,'TIRRAZ_ValorActual_PrecioLimpio' =( VA.TIRRAZ_PrecioLimpio * VA.TIRRAZ_SaldoNominalVigente) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso)
		, 'PRELIM_ValorActual_PrecioLimpio' =  (CASE WHEN ISNULL(VA.PRELIM_PrecioLimpio,0)=0 THEN 0 ELSE  (VA.PRELIM_PrecioLimpio * VA.PRELIM_SaldoNominalVigente) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso ) END)
		,CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_TIR
			WHEN 'A_VENCI' THEN VA.TIRCOM_TIR
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_TIR
		END AS TasaDescuentoSBS
		,V.TasaCupon
		--,CASE OI.TipoValorizacion
		--	WHEN 'DIS_VENTA' THEN VA.VTA_TIR
		--	WHEN 'A_VENCI' THEN VA.TIRCOM_TIR
		--	WHEN 'VAL_RAZO' THEN VA.TIRRAZ_TIR
		--END AS TasaDescuentoCompra
		
		,TIRCOM_TIR AS TasaDescuentoCompra
		
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
			* CASE OI.TipoValorizacion
				WHEN 'DIS_VENTA' THEN VA.VTA_InteresCorrido
				WHEN 'A_VENCI' THEN VA.TIRCOM_InteresCorrido
				WHEN 'VAL_RAZO' THEN VA.TIRRAZ_InteresCorrido
			END AS InteresesCorridos
			
		--,((CASE OI.TipoValorizacion
		--	WHEN 'DIS_VENTA' THEN VA.VTA_PrecioSucio
		--	WHEN 'A_VENCI' THEN VA.TIRCOM_PrecioSucio
		--	WHEN 'VAL_RAZO' THEN VA.TIRRAZ_PrecioSucio
		--END) - OI.PrecioNegociacionSucio) AS Sobre_BajoPrecio
		
		/*
		,(CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN (VA.VTA_ValorActual - VA.VTA_InteresCorrido) - VA.VTA_SaldoNominalVigente
			WHEN 'A_VENCI' THEN (VA.TIRCOM_ValorActual - VA.TIRCOM_InteresCorrido) - VA.TIRCOM_SaldoNominalVigente
			WHEN 'VAL_RAZO' THEN (VA.TIRRAZ_ValorActual - VA.TIRRAZ_InteresCorrido) - VA.TIRRAZ_SaldoNominalVigente
		END) AS Sobre_BajoPrecio
		*/

		,(VA.TIRCOM_ValorActual
		- (CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_InteresCorrido
			WHEN 'A_VENCI' THEN VA.TIRCOM_InteresCorrido
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_InteresCorrido
		END)
		- (CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_SaldoNominalVigente
			WHEN 'A_VENCI' THEN VA.TIRCOM_SaldoNominalVigente
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_SaldoNominalVigente
		END)) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, OI.CodigoMoneda, @p_FechaProceso) AS Sobre_BajoPrecio
		
		,(VA.TIRRAZ_ValorActual - VA.TIRCOM_ValorActual) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) AS FluctuacionValor
		 
		,VP.Duracion AS DuracionSBS
		
		--,(CASE OI.TipoValorizacion
		--	WHEN 'DIS_VENTA' THEN VA.VTA_CantidadOperacion * VA.VTA_PrecioSucio
		--	WHEN 'A_VENCI' THEN VA.TIRCOM_CantidadOperacion * VA.TIRCOM_PrecioSucio
		--	WHEN 'VAL_RAZO' THEN VA.TIRRAZ_CantidadOperacion * VA.TIRRAZ_PrecioSucio
		--END)/dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso) AS ParticipacionPatrimonio
		--,CASE dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso)
		--	WHEN 0 THEN 0
		--	ELSE VA.TIRCOM_ValorActual/dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso)
		-- END AS ParticipacionPatrimonio
		
		,ISNULL((CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_CantidadOperacion * VA.VTA_PrecioSucio
			WHEN 'A_VENCI' THEN VA.TIRCOM_CantidadOperacion * VA.TIRCOM_PrecioSucio
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_CantidadOperacion * VA.TIRRAZ_PrecioSucio
		END), 0) AS VPNFondo
		
		--,dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso) AS PatrimonioFondo
		
		,T.Descripcion AS Tercero
		,OI.CodigoOrden
		,'' AS EmpresaClasificadora
		,'' AS FechaClasificacion
		,isnull(PGA.Nombre,'') AS ActividadEconomica 
	FROM ValorizacionAmortizada VA
		INNER JOIN OrdenInversion OI ON OI.CodigoOrden = VA.CodigoOrden AND OI.CodigoPortafolioSBS = VA.CodigoPortafolioSBS
		INNER JOIN Valores V ON VA.CodigoNemonico = V.CodigoNemonico
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		LEFT JOIN ParametrosGenerales PG ON PG.Clasificacion = 'RATING' AND V.Rating = PG.Valor
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion LIKE 'sector%' AND T.SectorGigs=PGA.Valor
		INNER JOIN Portafolio P ON VA.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN VectorPrecio VP ON VA.CodigoNemonico = VP.CodigoMnemonico AND VP.Fecha = @p_FechaProceso
			AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(VA.CodigoPortafolioSBS,VA.CodigoNemonico,@p_FechaProceso)
	WHERE VA.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND VA.FechaProceso = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
	UNION
	--DEPÓSITOS A PLAZO
	SELECT
		CTV.CodigoPortafolioSBS
		,DescripcionPortafolio = P.Descripcion
		,TI.CodigoTipoInstrumentoSBS
		,DescripcionTipoInstrumento = TI.Descripcion
		,V.CodigoNemonico
		--,V.CodigoISIN
		,'' AS CodigoISIN
		--,V.CodigoMoneda
		,M.SinonimoISO as CodigoMoneda
		,Rating = ''
		,FechaVencimiento = OI.FechaContrato
		,ProximoCuponFecha = OI.FechaContrato
		,OI.MontoOperacion AS ProximoCuponMonto
		,0 AS CantidadOperacion
		,OI.MontoNominalOrdenado AS MontoNominalInicial
		,OI.MontoNominalOrdenado AS MontoNominalVigente
		,0 AS PrecioOperacion
		,OI.MontoNominalOrdenado AS ValorCompra
		,0 AS TIRCOM_PrecioSucio
		--,CTV.VPNMonedaFondo AS TIRCOM_ValorActual
		,(SELECT VPNMonedaFondo FROM CarteraTituloValoracion
			WHERE CodigoPortafolioSBS = CTV.CodigoPortafolioSBS
			AND CodigoSBS = CTV.CodigoSBS
			AND FechaValoracion = @p_FechaProceso) AS TIRCOM_ValorActual
		,0 AS TIRRAZ_PrecioLimpio
		--,CTV.VPNMonedaFondo AS TIRRAZ_ValorActual
		,(SELECT VPNMonedaFondo FROM CarteraTituloValoracion
			WHERE CodigoPortafolioSBS = CTV.CodigoPortafolioSBS
			AND CodigoSBS = CTV.CodigoSBS
			AND FechaValoracion = @p_FechaProceso) AS TIRRAZ_ValorActual
			,(SELECT VPNMonedaFondo FROM CarteraTituloValoracion
			WHERE CodigoPortafolioSBS = CTV.CodigoPortafolioSBS
			AND CodigoSBS = CTV.CodigoSBS
			AND FechaValoracion = @p_FechaProceso) AS PRELIM_ValorActual
		,OI.MontoNominalOrdenado * dbo.RetornarSecuenciaVectorTipoCambio(CTV.CodigoPortafolioSBS, OI.CodigoMoneda, @p_FechaProceso) AS TIRRAZ_ValorActual_PrecioLimpio
		,OI.MontoNominalOrdenado * dbo.RetornarSecuenciaVectorTipoCambio(CTV.CodigoPortafolioSBS, OI.CodigoMoneda, @p_FechaProceso) AS PRELIM_ValorActual_PrecioLimpio
		,OI.TasaPorcentaje AS TasaDescuentoSBS
		,OI.TasaPorcentaje AS TasaCupon
		,OI.TasaPorcentaje AS TasaDescuentoCompra
		--,CTV.VPNMonedaFondo - OI.MontoNominalOrdenado AS InteresesCorridos
		,((SELECT VPNOriginal FROM CarteraTituloValoracion
			WHERE CodigoPortafolioSBS = CTV.CodigoPortafolioSBS
			AND CodigoSBS = CTV.CodigoSBS
			AND FechaValoracion = @p_FechaProceso) - OI.MontoNominalOrdenado
		) * dbo.RetornarSecuenciaVectorTipoCambio(CTV.CodigoPortafolioSBS, (CASE WHEN P.CodigoMoneda <> OI.CodigoMoneda THEN OI.CodigoMoneda ELSE P.CodigoMoneda END), @p_FechaProceso) AS InteresesCorridos
		,0 AS Sobre_BajoPrecio
		,0 AS FluctuacionValor
		,0 AS DuracionSBS
		--,CASE dbo.RetornarInversiones_ValorFondo(CTV.CodigoPortafolioSBS,CTV.FechaValoracion)
		--	WHEN 0 THEN 0
		--	ELSE CTV.VPNMonedaFondo/dbo.RetornarInversiones_ValorFondo(CTV.CodigoPortafolioSBS,CTV.FechaValoracion)
		-- END AS ParticipacionPatrimonio
		,ISNULL(CTV.VPNMonedaFondo, 0) AS VPNFondo
		--,dbo.RetornarInversiones_ValorFondo(CTV.CodigoPortafolioSBS,CTV.FechaValoracion) AS PatrimonioFondo
		,T.Descripcion AS Tercero
		,OI.CodigoOrden
		,'' AS EmpresaClasificadora
		,'' AS FechaClasificacion
		--,'' AS ActividadEconomica 
		,isnull(PGA.Nombre,'') AS ActividadEconomica 
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND OI.FechaOperacion = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
		INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion LIKE 'sector%' AND T.SectorGigs=PGA.Valor
	WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		--AND CTV.FechaValoracion = @p_FechaProceso
		AND OI.FechaContrato > @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
	UNION
	--OPERACIONES DE RENTA VARIABLE
	SELECT
		OI.CodigoPortafolioSBS
		,DescripcionPortafolio = P.Descripcion
		,TI.CodigoTipoInstrumentoSBS
		,DescripcionTipoInstrumento = TI.Descripcion
		,V.CodigoNemonico
		,V.CodigoISIN
		--,V.CodigoMoneda
		,M.SinonimoISO as CodigoMoneda
		,Rating = ''
		,FechaVencimiento = OI.FechaContrato
		,ProximoCuponFecha = NULL
		,0 AS ProximoCuponMonto
		,0 AS CantidadOperacion
		,OI.MontoOperacion AS MontoNominalInicial
		,OI.MontoOperacion AS MontoNominalVigente
		,0 AS PrecioOperacion
		,OI.MontoNetoOperacion AS ValorCompra
		,0 AS TIRCOM_PrecioSucio
		,OI.MontoNetoOperacion AS TIRCOM_ValorActual
		,0 AS TIRRAZ_PrecioLimpio
		,OI.MontoNetoOperacion AS TIRRAZ_ValorActual
		,OI.MontoNetoOperacion AS PRELIM_ValorActual
		,0 AS TIRRAZ_ValorActual_PrecioLimpio
		,0 AS PRELIM_ValorActual_PrecioLimpio
		,0 AS TasaDescuentoSBS
		,OI.TasaPorcentaje AS TasaCupon
		,OI.TasaPorcentaje AS TasaDescuentoCompra
		,OI.TotalComisiones AS InteresesCorridos
		,0 AS Sobre_BajoPrecio
		,0 AS FluctuacionValor
		,0 AS DuracionSBS
		--,CASE dbo.RetornarInversiones_ValorFondo(OI.CodigoPortafolioSBS,OI.FechaOperacion)
		--	WHEN 0 THEN 0
		--	ELSE OI.MontoNetoOperacion/dbo.RetornarInversiones_ValorFondo(OI.CodigoPortafolioSBS,OI.FechaOperacion)
		-- END AS ParticipacionPatrimonio
		,ISNULL(OI.MontoNetoOperacion, 0) AS VPNFondo
		--,dbo.RetornarInversiones_ValorFondo(OI.CodigoPortafolioSBS,OI.FechaOperacion) AS PatrimonioFondo
		,T.CodigoTercero AS Tercero
		,OI.CodigoOrden
		,'' AS EmpresaClasificadora
		,'' AS FechaClasificacion
		--,'' AS ActividadEconomica 
		,isnull(PGA.Nombre,'') AS ActividadEconomica 
	FROM OrdenInversion OI
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion LIKE 'sector%' AND T.SectorGigs=PGA.Valor
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND OI.FechaOperacion = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
		AND EXISTS(
			SELECT 1 FROM CarteraTituloValoracion CTV WHERE CTV.CodigoMnemonico = OI.CodigoMnemonico
			AND CTV.FechaValoracion = @p_FechaProceso AND CTV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS)
	ORDER BY 3,2
		
END
GO

GRANT EXECUTE ON [dbo].[ReporteGestion_ReporteConsolidado] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_Gen_Carta_OperacionCambio]'

USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_Gen_Carta_OperacionCambio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionCambio]
GO
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/01/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Modificación de redondeo de Tipo Cambio.
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 26/03/2019
--	Craedo por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11934
--	Descripcion del cambio: Agregar los campos "ObservacionCarta" y "ObservacionCartaDestino"
-----------------------------------------------------------------------------------------------------------
CREATE procedure [dbo].[sp_SIT_Gen_Carta_OperacionCambio]
	@p_CodigoOperacionCaja VarChar(12) 
AS
BEGIN

	DECLARE @BancoPublico VarChar(100)

	SELECT 
	@BancoPublico = 
	T.Descripcion From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	WHERE Clasificacion = 'BANFONPUB'
 
	SELECT
	NombrePortafolio = P.Descripcion,
	NombreCompletoPortafolio = P.NombreCompleto,
	FechaCarta = dbo.RetornarFechaCompleta(OI.FechaLiquidacion),
	BancoPublico = CASE WHEN P.IndicadorFondo  = 'S' THEN @BancoPublico   ELSE  T.Descripcion END, 
	Contraparte = T.Descripcion,
	RUCPortafolio = ISNULL(P.RUCPortafolio,''),
	OC.FechaOperacion, 
	O.Descripcion DescripcionOperacion, --operacion de cambio
	MC.Descripcion ModeloCarta , 
	OC.CodigoMoneda,
	BancoMatrizOrigen = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizOrigen) ,'') ,
	SimboloMoneda = M.Simbolo,
	Importe = ROUND(OC.Importe,2), 
	SimboloCuenta = CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END, 
	ce.NumeroCuentaInterBancario ,
    OC.NumeroCuenta, 
	TB.Descripcion AS Banco, 
	ISNULL(OC.CodigoOrden,OC.CodigoOperacionCaja) NumeroOrden,
	BancoMatrizDestino = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizDestino) ,'') ,
    BancoDestino  = ISNULL( (SELECT BD.Descripcion FROM Terceros BD WHERE BD.CodigoTercero = ED.CodigoTercero) ,'')  ,
	OI.CodigoMonedaDestino,
	SimboloCuentaDestino = CASE WHEN OI.CodigoMonedaDestino = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END, 
	OC.NumeroCuentaDestino,
	NumeroCuentaInterBancarioDestino = CED.NumeroCuentaInterBancario,
	SimboloMonedaDestino = MOI.Simbolo ,
	MontoDestino = ROUND(OI.MontoDestino,2),
	T.Descripcion DescripcionIntermediario ,
	TipoCambio = ISNULL(OI.TipoCambio,0),
	CodigoMonedaOrigen = OI.CodigoMoneda,
	MontoOrigen = ROUND(OI.MontoOrigen,2),
	LTRIM(RTRIM(ISNULL(P1.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoMaterno,''))) VBADMIN,
	LTRIM(RTRIM(ISNULL(P2.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoMaterno,''))) VBGERF1,
	LTRIM(RTRIM(ISNULL(P3.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoMaterno,''))) VBGERF2,
	OC.CodigoOperacion,
	OC.CodigoOperacionCaja,
	OC.EstadoCarta, 
	OC.CodigoModelo,
	P.CodigoPortafolioSBS,
	ISNULL(OC.CorrelativoCartas,0) CorrelativoCartas,
	ISNULL(OC.CodigoCartaAgrupado,0) AS CodigoCartaAgrupado,
	Firma1 = ISNULL(AC1.Firma,''),
	Firma2 = ISNULL(AC2.Firma,''),
	NombreUsuarioF1 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF1),''),
	NombreUsuarioF2 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF2),''),
	CargoUsuarioF1 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF1),'') ,
	CargoUsuarioF2 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF2),''),
	OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2,OC.ObservacionCarta,OC.ObservacionCartaDestino
	FROM OperacionesCaja OC
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS
	JOIN Operacion O ON O.CodigoOperacion = OC.CodigoOperacion 
	JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
	JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta AND CE.Situacion = 'A' AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera AND E.Situacion = 'A' 
		--AND E.CodigoTercero = CASE WHEN @p_CodigoTerceroBanco = '' THEN E.CodigoTercero ELSE @p_CodigoTerceroBanco END
	JOIN Terceros TB ON E.CodigoTercero = TB.CodigoTercero AND E.Situacion = 'A'
	JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo  AND MC.Situacion = 'A'
							AND MC.CodigoOperacion = OC.CodigoOperacion
	JOIN OrdenInversion OI ON OC.CodigoOperacionCaja = OI.CodigoOrden
	JOIN Moneda M ON M.CodigoMoneda = OC.CodigoMoneda
	JOIN Moneda MOI ON MOI.CodigoMoneda = oi.CodigoMonedaDestino
	LEFT JOIN CuentaEconomica CED ON CED.NumeroCuenta = OC.NumeroCuentaDestino 
										AND CED.Situacion = 'A' 
										AND CED.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	LEFT JOIN Entidad ED ON ED.CodigoEntidad = CED.EntidadFinanciera AND E.Situacion = 'A' 
	LEFT JOIN Personal P1 ON P1.CodigoInterno = OC.VBADMIN
	LEFT JOIN Personal P2 ON P2.CodigoInterno = OC.VBGERF1
	LEFT JOIN Personal P3 ON P3.CodigoInterno = OC.VBGERF2
	LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = OC.VBGERF1
	LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = OC.VBGERF2
	WHERE OC.CodigoModelo <> 'SC01'
		  AND OC.CodigoOperacion NOT IN ('63','BCRE')  
		  AND OC.CodigoOperacionCaja = @p_CodigoOperacionCaja-- IN ('054117','054118')

END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_Gen_Carta_OperacionCambio] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[RetornarFechaT_1]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RetornarFechaT_1]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[RetornarFechaT_1]
GO
---------------------------------------------------------------------------------------------------------------------
--Objetivo: Retorna fecha T - 1
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 22/06/2017
-- Creado por: Ian Pastor
-- Nro. Orden de Trabajo: 10741
-- Descripcion del cambio: Optimización de función
---------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[RetornarFechaT_1]
(
	@p_fecha NUMERIC(8),
	@p_portafolio VARCHAR(10),
	@p_nemonico VARCHAR(12)
)  
RETURNS NUMERIC
AS
BEGIN
	DECLARE	@p_fechaFin NUMERIC(8)

	SELECT @p_fechaFin = MAX(fechaValoracion)
	FROM ValoracionCartera
	WHERE codigoPortafolioSBS = @p_portafolio
		AND fechaValoracion < @p_fecha
		AND Escenario = 'REAL'
	RETURN @p_fechaFin
END
GO

GRANT EXECUTE ON [dbo].[RetornarFechaT_1] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'INICIO --- > CREACIÓN DE INDICE EN TABLA ValoracionCartera'
/*ÍNDICE*/
CREATE CLUSTERED INDEX IX_ValoracionCartera ON ValoracionCartera(FechaValoracion DESC, CodigoPortafolioSBS DESC, Escenario DESC)
GO
PRINT 'FIN --- > CREACIÓN DE INDICE EN TABLA ValoracionCartera'

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 