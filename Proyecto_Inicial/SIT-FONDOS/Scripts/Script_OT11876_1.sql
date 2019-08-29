USE [SIT-FONDOS]
GO


BEGIN TRANSACTION __Transaction_Log

PRINT 'NUEVO CAMPO: PRELIM_ValorActual en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_ValorActual'))
ALTER TABLE ValorizacionAmortizada add PRELIM_ValorActual numeric(22, 7);
	
PRINT 'NUEVO CAMPO: PRELIM_InteresCorrido en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_InteresCorrido'))
ALTER TABLE ValorizacionAmortizada add PRELIM_InteresCorrido numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_ValorPrincipal en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_ValorPrincipal'))
ALTER TABLE ValorizacionAmortizada add PRELIM_ValorPrincipal numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_PrecioLimpio en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_PrecioLimpio'))
ALTER TABLE ValorizacionAmortizada add PRELIM_PrecioLimpio numeric(22, 7);
	
PRINT 'NUEVO CAMPO: PRELIM_PrecioSucio en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_PrecioSucio'))
ALTER TABLE ValorizacionAmortizada add PRELIM_PrecioSucio numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_TIR en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_TIR'))
ALTER TABLE ValorizacionAmortizada add PRELIM_TIR numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_FechaFinCuponActual en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_FechaFinCuponActual'))
ALTER TABLE ValorizacionAmortizada add PRELIM_FechaFinCuponActual numeric(8, 0);

PRINT 'NUEVO CAMPO: PRELIM_MontoCuponActual en TABLA ValorizacionAmortizada'	
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_MontoCuponActual'))
ALTER TABLE ValorizacionAmortizada add PRELIM_MontoCuponActual numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_CantidadOperacion en TABLA ValorizacionAmortizada'	
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_CantidadOperacion'))
ALTER TABLE ValorizacionAmortizada add PRELIM_CantidadOperacion numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_ValorNominal en TABLA ValorizacionAmortizada'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_ValorNominal'))
ALTER TABLE ValorizacionAmortizada add PRELIM_ValorNominal numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_SaldoNominalVigente en TABLA ValorizacionAmortizada'	
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_SaldoNominalVigente'))
ALTER TABLE ValorizacionAmortizada add PRELIM_SaldoNominalVigente numeric(22, 7);

PRINT 'NUEVO CAMPO: PRELIM_PagoCuponVigente en TABLA ValorizacionAmortizada'	
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada]') and upper(name) = upper('PRELIM_PagoCuponVigente'))
ALTER TABLE ValorizacionAmortizada add PRELIM_PagoCuponVigente numeric(22, 7);

PRINT 'ACTUALIZAR CAMPO: activo con valor 01'	
update CentrosCosto set activo='01'


PRINT 'Script Ingresar Usuario SIT-FONDOS Armando Vidal - P900011'

DELETE FROM Personal WHERE CodigoInterno = 'P900011'
GO

INSERT INTO dbo.Personal (CodigoInterno, CodigoUsuario, CodigoCentroCosto, ApellidoPaterno, ApellidoMaterno, PrimerNombre, SegundoNombre, CodigoCargo, FechaCese, IndicadorFuncionario, FechaIngreso, LibretaElectoral, CodigoInternoVendedor, CodigoSafp, email_trabajo, email_personal, matricula)
VALUES ('P900011', 'P900011', '', 'VIDAL', 'CASTAÑAGA', 'ARMANDO', '', 'PE_2287 ', NULL, '02', 20141101, '44261707', '', '', 'armando.vidal@sura.pe', NULL, '0000500569')
GO

UPDATE OrdenInversion
SET MontoCancelar = 531591.39, MontoNetoOperacion = 531591.39, MontoOperacion = 531591.39
where CodigoOrden = '059208'
GO

UPDATE OrdenInversion
SET MontoOperacion = 406815.79, MontoNetoOperacion = 406815.79, MontoCancelar = 406815.79
WHERE CodigoOrden = '060695'
GO

DELETE FROM ModeloCarta WHERE CodigoModelo = 'VDIV'
GO
INSERT INTO ModeloCarta 
SELECT 'VDIV' as [CodigoModelo]
      , '66' as [CodigoOperacion]
      , 'VDIV - VENTA DE DIVISAS' [Descripcion]
      , 'Modulos\ModelosCarta\rpt_PlantillaCartas15.rdlc'  AS [ArchivoPlantilla]
      ,[UsuarioValidador1]
      ,[UsuarioValidador2]
      ,[Situacion]
      ,[UsuarioCreacion]
      ,[FechaCreacion]
      ,[HoraCreacion]
      ,[UsuarioModificacion]
      ,[FechaModificacion]
      ,[HoraModificacion]
      ,[Host]
      ,[NumeroCartas]
FROM [dbo].[ModeloCarta]
WHERE CodigoModelo ='CA01'
GO


PRINT 'INICIO --- > SECCIÓN DE SP'

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
		AND OI.FechaContrato >= @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
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
		AND OI.FechaContrato >= @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
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

 
 
 
PRINT '[dbo].[pr_val_rf_amortizada_compras_en_stock]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_val_rf_amortizada_compras_en_stock]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pr_val_rf_amortizada_compras_en_stock]
GO
------------------------------------------------------------------------------------------------------------------------
--Objetivo: Selecciona las negociaciones para la valorización amortizada 
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Creación: 2018-09-26
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: OT11590
-------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: Agregar el campo precio sucio y precio limpio a la comsulta de ordenes
-----------------------------------------------------------------------------------------------------------------------------

-- exec pr_val_rf_amortizada_compras_en_stock '854767', '20180829'
-- exec pr_val_rf_amortizada_compras_en_stock '999', '20180904'

CREATE proc [dbo].[pr_val_rf_amortizada_compras_en_stock]
(
	 @p_CodigoPortafolioSBS varchar(10)
	,@p_FechaOperacion numeric(12) 
)
as
begin
	
	DECLARE @InstrumentosConSaldo TABLE (CodigoNemonico varchar(15), Saldo numeric(22,7))
	/*CRumiche: Obtenemos los instrumentos con SALDO a la FECHA indicada*/
	insert into @InstrumentosConSaldo
			select CodigoMnemonico, Saldo from (
				select CodigoMnemonico
						,Saldo = isnull(SALDODISPONIBLE,0) + isnull(SALDOPROCESOCOMPRA,0) + isnull(SALDOUNIDADESLIBERADAS,0) 
								- isnull(SALDOPROCESOVENTA,0) - isnull(Redencion,0)
				from SaldosCarteraTitulo
				where CODIGOPORTAFOLIOSBS = @p_CodigoPortafolioSBS and FECHASALDO = @p_FechaOperacion
				/*CRumiche: Solo consideraremos los Tipos de Instrumentos requeridos*/
				and CodigoMnemonico in 
				(	select CodigoNemonico from Valores V
					inner join TipoInstrumento TI on V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
					where TI.CodigoRenta = 1 /* Renta Fija*/
					and TI.CodigoClaseInstrumento in 
					(select CodigoClaseInstrumento from ClaseInstrumento
						where Categoria in 
						(
							'BO',	/*Bonos*/
							'PC',	/*Papeles comerciales*/
							'CD',	/*Certificado de depósito*/	
							'LH',	/*Letras hipotecarias*/
							'DP'	/*Depósito a plazos*/
						)
					)
				)
			) R
			where Saldo > 0
			------/* Todos los Instrumentos de Renta Fija */
			------select Categoria, Descripcion from ClaseInstrumento
			------where CodigoClaseInstrumento in (select distinct CodigoClaseInstrumento from TipoInstrumento where CodigoRenta = 1 /* Renta Fija*/)	
										
	DECLARE @Orden TABLE (CodigoOrden varchar(12), FechaOrden numeric(10), CodigoNemonico varchar(15), CantidadEnStock numeric(22,7), AunEnStock bit)
	/*CRumiche: PASO 2: Obtenemos las Ordenes que cumplen los requisitos*/
	insert into @Orden		
			select OI.CodigoOrden, OI.FechaOperacion, OI.CodigoMNemonico, OI.CantidadOperacion, 0 as AunEnStock /* Inicialmente la cantidad debería ir directa a Stock*/
			from OrdenInversion OI
			where OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			and OI.FechaOperacion <= @p_FechaOperacion
			and OI.Estado = 'E-CON' -- Solo las confirmadas  (CRumiche)
			and OI.CodigoOperacion = '1' -- COMPRAS (Solamente)
			-- Solo las que tienen SALDO en stock (CRumiche)
			and CodigoMnemonico in (select CodigoNemonico from @InstrumentosConSaldo)			
			ORDER BY OI.CodigoMNemonico, OI.FechaOperacion DESC			

	/*CRumiche: PASO 3: Ahora determinaremos las compras que tienen unidades aun en stock*/
	declare @v_CodigoNemonico varchar(15), @v_Saldo numeric(22,7)
	declare @v_CodigoOrden varchar(12), @v_CantidadEnStock numeric(22,7), @v_Acumulado numeric(22,7)	
				
	declare cur cursor for select CodigoNemonico, Saldo from @InstrumentosConSaldo 

	open cur  
	fetch next from cur into @v_CodigoNemonico, @v_Saldo

	while @@FETCH_STATUS = 0  
	begin
		set @v_Acumulado = 0
		
		declare curOrden cursor for 
			select CodigoOrden, CantidadEnStock from @Orden 
			where CodigoNemonico = @v_CodigoNemonico order by FechaOrden desc
			 
		open curOrden  
		fetch next from curOrden into @v_CodigoOrden, @v_CantidadEnStock
		while @@FETCH_STATUS = 0 
		begin 
			if @v_Acumulado + @v_CantidadEnStock < @v_Saldo
			begin
				set @v_Acumulado = @v_Acumulado + @v_CantidadEnStock

				/*CRumiche: Actualizamos los datos*/
				update @Orden set AunEnStock = 1
				where CodigoNemonico = @v_CodigoNemonico and CodigoOrden = @v_CodigoOrden
				
				fetch next from curOrden into @v_CodigoOrden, @v_CantidadEnStock			
			end
			else
			begin 
				/*CRumiche: Determinamos la NUEVA CANTIDAD en Stock para esta Orden*/
				set @v_CantidadEnStock = @v_Saldo - @v_Acumulado
							
				/*CRumiche: Actualizamos los datos*/
				update @Orden set AunEnStock = 1, CantidadEnStock = @v_CantidadEnStock 
				where CodigoNemonico = @v_CodigoNemonico and CodigoOrden = @v_CodigoOrden
				
				BREAK /*CRumiche: Salimos de la evaluación de este NEMONICO*/
			end					
		end 
		close curOrden  
		deallocate curOrden 

		/*CRumiche: Continuamos con el siguiente NEMONICO*/
		fetch next from cur into @v_CodigoNemonico, @v_Saldo
	end 
	close cur  
	deallocate cur 

	/*CRumiche: PASO 4: Eliminamos los que no estén en Stock*/
	delete from @Orden where AunEnStock = 0
	
	/*CRumiche: PASO 5: Retornamos la lista de negociaciones*/		
	SELECT
		 OI.CodigoPortafolioSBS
		,OI.CodigoMnemonico as CodigoNemonico
		,OI.CodigoOrden
		,OI.FechaOperacion
		,OI.FechaLiquidacion

		,OI.CantidadOperacion
		,isnull(OI.TasaPorcentaje, 0) as TasaPorcentaje

		,isnull(v.TasaCupon,0) as TasaCupon
		,isnull(Per.DiasPeriodo,0) as DiasPeriodicidad-- 360(Anual), 180(Semestral), 90(Trimestral), etc
		,v.ValorUnitario
		,v.BaseInteresCorridoDias as BaseCuponMensual -- Nuevos campos de BASE
		,v.BaseInteresCorrido as BaseCuponAnual -- Nuevos campos de BASE
		,v.CodigoMoneda			
		,v.codigoTipoCupon
		,v.CodigoMercado
		
		,isnull(v.FechaEmision,0) as FechaEmision
			
		,isnull(OI.YTM,0) as TIR_COMPRA
		,case when isnull(OI.TIRNeta,0) = 0 then isnull(OI.YTM,0) else isnull(OI.TIRNeta,0) end as TIR_Neta
		,isnull(VP.TIR,0) as TIR_SBS	
		,O.CantidadEnStock
		,VP.PorcPrecioLimpio AS PorcentagePrecioLimpio
		,VP.PorcPrecioSucio AS PorcentagePrecioSucio
	FROM OrdenInversion OI
	inner join @Orden O on OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS and OI.CodigoOrden = O.CodigoOrden
	inner join Valores V on V.CodigoNemonico = OI.CodigoMnemonico
	left join Periodicidad Per  ON v.CodigoPeriodicidad = Per.CodigoPeriodicidad
	
	left join VectorPrecio VP /*CRumiche: Para obtener el Vector Precio según orden de prioridad*/
		on VP.Fecha = @p_FechaOperacion
		and VP.CodigoMnemonico = OI.CodigoMnemonico 
		and VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS, OI.CodigoMnemonico, @p_FechaOperacion)

	ORDER BY OI.FechaOperacion DESC

	/*CRumiche: PASO 6: Retornamos la Cuponera del VALOR. La tabla @Orden es clave, pues podrían haber negociaciones con el 
		mismo instrumento, con lo cual se traeria solo un conjunto de datos de su cuponera */
	select CodigoNemonico, Secuencia, FechaInicio, FechaTermino, Amortizacion
	from CuponeraNormal where CodigoNemonico in (select CodigoNemonico from @Orden)
	order by CodigoNemonico, convert(int,Secuencia)
		
END

GO

GRANT EXECUTE ON [dbo].[pr_val_rf_amortizada_compras_en_stock]  TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[ValorizacionAmortizada_Insertar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionAmortizada_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorizacionAmortizada_Insertar]
GO
------------------------------------------------------------------------------------------------------------------------
--Objetivo: Insertar Valorización Amortizada
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Creación: 2018-09-26
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: OT11590
-------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 27/12/2018
-- Modificado por: Carlos Rumiche
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Modificación campos
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: Agregando campos para la obtencion del  TIR por medio del precio limpio
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ValorizacionAmortizada_Insertar]
(
	 @p_IdProcesoValAmortizada int
	,@p_CodigoPortafolioSBS varchar(10)
	,@p_FechaProceso numeric(8)	
	,@p_CodigoOrden varchar(12)
	,@p_CodigoNemonico varchar(15)
    ,@p_CantidadOperacion numeric(22,7)
    ,@p_CantidadEnStock numeric(22,7)
    	
    ,@p_TIRCOM_ValorActual numeric(22,7)
    ,@p_TIRCOM_InteresCorrido numeric(22,7)
    ,@p_TIRCOM_ValorPrincipal numeric(22,7)
    ,@p_TIRCOM_PrecioLimpio numeric(22,7)
    ,@p_TIRCOM_PrecioSucio numeric(22,7)
    ,@p_TIRCOM_TIR numeric(22,7)
    
	,@p_TIRCOM_FechaFinCuponActual numeric(8)
    ,@p_TIRCOM_MontoCuponActual numeric(22,7)
    ,@p_TIRCOM_CantidadOperacion numeric(22,7)
	,@p_TIRCOM_ValorNominal numeric(22,7)    
	,@p_TIRCOM_SaldoNominalVigente numeric(22,7)
	,@p_TIRCOM_PagoCuponVigente numeric(22,7)        
    
    ,@p_TIRRAZ_ValorActual numeric(22,7)
    ,@p_TIRRAZ_InteresCorrido numeric(22,7)
    ,@p_TIRRAZ_ValorPrincipal numeric(22,7)
    ,@p_TIRRAZ_PrecioLimpio numeric(22,7)
    ,@p_TIRRAZ_PrecioSucio numeric(22,7)
    ,@p_TIRRAZ_TIR numeric(22,7)
    
	,@p_TIRRAZ_FechaFinCuponActual numeric(8)
    ,@p_TIRRAZ_MontoCuponActual numeric(22,7)
    ,@p_TIRRAZ_CantidadOperacion numeric(22,7)
	,@p_TIRRAZ_ValorNominal numeric(22,7)    
	,@p_TIRRAZ_SaldoNominalVigente numeric(22,7)
	,@p_TIRRAZ_PagoCuponVigente numeric(22,7)  	
		
    ,@p_VTA_ValorActual numeric(22,7)
    ,@p_VTA_InteresCorrido numeric(22,7)
    ,@p_VTA_ValorPrincipal numeric(22,7)
    ,@p_VTA_PrecioLimpio numeric(22,7)
    ,@p_VTA_PrecioSucio numeric(22,7)
    ,@p_VTA_TIR numeric(22,7)
    
	,@p_VTA_FechaFinCuponActual numeric(8)
    ,@p_VTA_MontoCuponActual numeric(22,7)
    ,@p_VTA_CantidadOperacion numeric(22,7)
	,@p_VTA_ValorNominal numeric(22,7)    
	,@p_VTA_SaldoNominalVigente numeric(22,7)
	,@p_VTA_PagoCuponVigente numeric(22,7)  
	
    ,@p_NEG_ValorActual numeric(22,7)
    ,@p_NEG_InteresCorrido numeric(22,7)
    ,@p_NEG_ValorPrincipal numeric(22,7)
    ,@p_NEG_PrecioLimpio numeric(22,7)
    ,@p_NEG_PrecioSucio numeric(22,7)
    ,@p_NEG_TIR numeric(22,7)
    
	,@p_NEG_FechaFinCuponActual numeric(8)
    ,@p_NEG_MontoCuponActual numeric(22,7)
    ,@p_NEG_CantidadOperacion numeric(22,7)
	,@p_NEG_ValorNominal numeric(22,7)    
	,@p_NEG_SaldoNominalVigente numeric(22,7)
	,@p_NEG_PagoCuponVigente numeric(22,7) 
	
	,@p_PRELIM_ValorActual numeric(22,7) = NULL
    ,@p_PRELIM_InteresCorrido numeric(22,7) = NULL
    ,@p_PRELIM_ValorPrincipal numeric(22,7) = NULL
    ,@p_PRELIM_PrecioLimpio numeric(22,7) = NULL
    ,@p_PRELIM_PrecioSucio numeric(22,7) = NULL
    ,@p_PRELIM_TIR numeric(22,7) = NULL
	,@p_PRELIM_FechaFinCuponActual numeric(8) = NULL
    ,@p_PRELIM_MontoCuponActual numeric(22,7) = NULL
    ,@p_PRELIM_CantidadOperacion numeric(22,7) = NULL
	,@p_PRELIM_ValorNominal numeric(22,7)    = NULL
	,@p_PRELIM_SaldoNominalVigente numeric(22,7) = NULL
	,@p_PRELIM_PagoCuponVigente numeric(22,7) = NULL
)
as
begin 
	insert into ValorizacionAmortizada (
		 IdProcesoValAmortizada
		,CodigoPortafolioSBS
		,FechaProceso
		,CodigoOrden		
		,CodigoNemonico
		,CantidadOperacion
		,CantidadEnStock
					
		-- A TIR DE COMPRA con INTERESES
		,TIRCOM_ValorActual
		,TIRCOM_InteresCorrido
		,TIRCOM_ValorPrincipal
		,TIRCOM_PrecioLimpio
		,TIRCOM_PrecioSucio
		,TIRCOM_TIR
		
		,TIRCOM_FechaFinCuponActual
		,TIRCOM_MontoCuponActual
		,TIRCOM_CantidadOperacion
		,TIRCOM_ValorNominal    
		,TIRCOM_SaldoNominalVigente
		,TIRCOM_PagoCuponVigente
				
	    -- A TIR SBS - VAL RAZONABLE
		,TIRRAZ_ValorActual
		,TIRRAZ_InteresCorrido
		,TIRRAZ_ValorPrincipal
		,TIRRAZ_PrecioLimpio
		,TIRRAZ_PrecioSucio
		,TIRRAZ_TIR
		
		,TIRRAZ_FechaFinCuponActual
		,TIRRAZ_MontoCuponActual
		,TIRRAZ_CantidadOperacion
		,TIRRAZ_ValorNominal    
		,TIRRAZ_SaldoNominalVigente
		,TIRRAZ_PagoCuponVigente  
		
		-- A TIR SBS - VAL RAZONABLE
		,VTA_ValorActual
		,VTA_InteresCorrido
		,VTA_ValorPrincipal
		,VTA_PrecioLimpio
		,VTA_PrecioSucio
		,VTA_TIR
		
		,VTA_FechaFinCuponActual
		,VTA_MontoCuponActual
		,VTA_CantidadOperacion
		,VTA_ValorNominal    
		,VTA_SaldoNominalVigente
		,VTA_PagoCuponVigente
		
		-- A TIR COMPRA (negociación)
		,NEG_ValorActual
		,NEG_InteresCorrido
		,NEG_ValorPrincipal
		,NEG_PrecioLimpio
		,NEG_PrecioSucio
		,NEG_TIR
		
		,NEG_FechaFinCuponActual
		,NEG_MontoCuponActual
		,NEG_CantidadOperacion
		,NEG_ValorNominal    
		,NEG_SaldoNominalVigente
		,NEG_PagoCuponVigente		


		-- A TIR PRECIO LIMPIO 
		,PRELIM_ValorActual
		,PRELIM_InteresCorrido
		,PRELIM_ValorPrincipal
		,PRELIM_PrecioLimpio
		,PRELIM_PrecioSucio
		,PRELIM_TIR		
		,PRELIM_FechaFinCuponActual
		,PRELIM_MontoCuponActual
		,PRELIM_CantidadOperacion
		,PRELIM_ValorNominal    
		,PRELIM_SaldoNominalVigente
		,PRELIM_PagoCuponVigente		


	)values (
	
		 @p_IdProcesoValAmortizada
		,@p_CodigoPortafolioSBS
		,@p_FechaProceso
		,@p_CodigoOrden		
		,@p_CodigoNemonico
		,@p_CantidadOperacion
		,@p_CantidadEnStock		
		
		-- A TIR DE COMPRA
		,@p_TIRCOM_ValorActual
		,@p_TIRCOM_InteresCorrido
		,@p_TIRCOM_ValorPrincipal
		,@p_TIRCOM_PrecioLimpio
		,@p_TIRCOM_PrecioSucio
		,@p_TIRCOM_TIR
		
		,@p_TIRCOM_FechaFinCuponActual
		,@p_TIRCOM_MontoCuponActual
		,@p_TIRCOM_CantidadOperacion
		,@p_TIRCOM_ValorNominal    
		,@p_TIRCOM_SaldoNominalVigente
		,@p_TIRCOM_PagoCuponVigente
				
	    -- A TIR SBS - VAL RAZONABLE
		,@p_TIRRAZ_ValorActual
		,@p_TIRRAZ_InteresCorrido
		,@p_TIRRAZ_ValorPrincipal
		,@p_TIRRAZ_PrecioLimpio
		,@p_TIRRAZ_PrecioSucio
		,@p_TIRRAZ_TIR
		
		,@p_TIRRAZ_FechaFinCuponActual
		,@p_TIRRAZ_MontoCuponActual
		,@p_TIRRAZ_CantidadOperacion
		,@p_TIRRAZ_ValorNominal    
		,@p_TIRRAZ_SaldoNominalVigente
		,@p_TIRRAZ_PagoCuponVigente 
		
		-- A TIR SBS - VAL RAZONABLE
		,@p_VTA_ValorActual
		,@p_VTA_InteresCorrido
		,@p_VTA_ValorPrincipal
		,@p_VTA_PrecioLimpio
		,@p_VTA_PrecioSucio
		,@p_VTA_TIR
		
		,@p_VTA_FechaFinCuponActual
		,@p_VTA_MontoCuponActual
		,@p_VTA_CantidadOperacion
		,@p_VTA_ValorNominal    
		,@p_VTA_SaldoNominalVigente
		,@p_VTA_PagoCuponVigente
		
		-- A TIR COMPRA (negociación)
		,@p_NEG_ValorActual
		,@p_NEG_InteresCorrido
		,@p_NEG_ValorPrincipal
		,@p_NEG_PrecioLimpio
		,@p_NEG_PrecioSucio
		,@p_NEG_TIR
		
		,@p_NEG_FechaFinCuponActual
		,@p_NEG_MontoCuponActual
		,@p_NEG_CantidadOperacion
		,@p_NEG_ValorNominal    
		,@p_NEG_SaldoNominalVigente
		,@p_NEG_PagoCuponVigente


		-- A PRELIM
		,@p_PRELIM_ValorActual
		,@p_PRELIM_InteresCorrido
		,@p_PRELIM_ValorPrincipal
		,@p_PRELIM_PrecioLimpio
		,@p_PRELIM_PrecioSucio
		,@p_PRELIM_TIR		
		,@p_PRELIM_FechaFinCuponActual
		,@p_PRELIM_MontoCuponActual
		,@p_PRELIM_CantidadOperacion
		,@p_PRELIM_ValorNominal    
		,@p_PRELIM_SaldoNominalVigente
		,@p_PRELIM_PagoCuponVigente
	)
end
GO

GRANT EXECUTE ON [dbo].[ValorizacionAmortizada_Insertar]  TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[Cargo_Listar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cargo_Listar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Cargo_Listar]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Listar Cargo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: store procedure para listar los cargos
-----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Cargo_Listar]  
AS
BEGIN
	select 
	CodigoCargo,
	Descripcion
	from 
	Cargo where Descripcion is not null

END  		
GO

GRANT EXECUTE ON [dbo].[Cargo_Listar]  TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Personal_Insertar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Personal_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Personal_Insertar]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Insertar Personal
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: store procedure para la creación de un personal
-----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Personal_Insertar]
(
	@p_CodigoInterno varchar(10),
	@p_CodigoUsuario varchar(10),
	@p_CodigoCentroCosto  varchar(11),
	@p_ApellidoPaterno  varchar(30),
	@p_ApellidoMaterno varchar(30),
	@p_PrimerNombre varchar(50),
	@p_SegundoNombre varchar(30),
	@p_CodigoCargo varchar(30),
	@p_FechaCese decimal(8,0),
	@p_FechaIngreso decimal(8,0),
	@p_LibretaElectoral varchar(12),
	@p_email_trabajo varchar(100),
	@p_email_personal  varchar(100),
	@p_matricula varchar(10)
)
AS
BEGIN

INSERT INTO [dbo].[Personal]
           ([CodigoInterno]
           ,[CodigoUsuario]
           ,[CodigoCentroCosto]
           ,[ApellidoPaterno]
           ,[ApellidoMaterno]
           ,[PrimerNombre]
           ,[SegundoNombre]
           ,[CodigoCargo]
           ,[FechaCese]
           ,[IndicadorFuncionario]
           ,[FechaIngreso]
           ,[LibretaElectoral]
           ,[CodigoInternoVendedor]
           ,[CodigoSafp]
           ,[email_trabajo]
           ,[email_personal]
           ,[matricula])
     VALUES
           (@p_CodigoInterno
           ,@p_CodigoUsuario
           ,@p_CodigoCentroCosto
           ,@p_ApellidoPaterno
           ,@p_ApellidoMaterno
           ,@p_PrimerNombre
           ,@p_SegundoNombre
           ,@p_CodigoCargo
           ,@p_FechaCese
           ,'02'
           ,@p_FechaIngreso
           ,@p_LibretaElectoral
           ,''
           ,''
           ,@p_email_trabajo
           ,@p_email_personal
           ,@p_matricula)
END
GO

GRANT EXECUTE ON [dbo].[Personal_Insertar]  TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Personal_Modificar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Personal_Modificar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Personal_Modificar]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Modificar Personal
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: store procedure para la modificacion del registro de un personal
-----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Personal_Modificar]
(
	@p_CodigoInterno varchar(10),
	@p_CodigoUsuario varchar(10),
	@p_CodigoCentroCosto  varchar(11),
	@p_ApellidoPaterno  varchar(30),
	@p_ApellidoMaterno varchar(30),
	@p_PrimerNombre varchar(50),
	@p_SegundoNombre varchar(30),
	@p_CodigoCargo varchar(30),
	@p_FechaCese decimal(8,0),
	@p_FechaIngreso decimal(8,0),
	@p_LibretaElectoral varchar(12),
	@p_email_trabajo varchar(100),
	@p_email_personal  varchar(100),
	@p_matricula varchar(10)
)
AS
BEGIN
UPDATE [dbo].[Personal]
   SET [CodigoInterno] = @p_CodigoInterno
      ,[CodigoUsuario] = @p_CodigoUsuario
      ,[CodigoCentroCosto] = @p_CodigoCentroCosto
      ,[ApellidoPaterno] = @p_ApellidoPaterno
      ,[ApellidoMaterno] = @p_ApellidoMaterno
      ,[PrimerNombre] = @p_PrimerNombre
      ,[CodigoCargo] = @p_CodigoCargo
      ,[FechaCese] = @p_FechaCese
      ,[FechaIngreso] = @p_FechaIngreso
      ,[LibretaElectoral] = @p_LibretaElectoral
      ,[email_trabajo] = @p_email_trabajo
      ,[email_personal] = @p_email_personal 
      ,[matricula] = @p_matricula
 WHERE [CodigoInterno] = @p_CodigoInterno

END
GO

GRANT EXECUTE ON [dbo].[Personal_Modificar]  TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Personal_Eliminar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Personal_Eliminar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Personal_Eliminar]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Eliminar Personal
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: store procedure para la eliminacion del registro de un personal
-----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Personal_Eliminar]
(
	@p_CodigoInterno varchar(10)
)
AS
BEGIN
	delete from Personal
	where CodigoInterno=@p_CodigoInterno
END
GO

GRANT EXECUTE ON [dbo].[Personal_Eliminar]  TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Personal_SeleccionarPorFiltro]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Personal_SeleccionarPorFiltro]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Personal_SeleccionarPorFiltro]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Listar tabla Personal por filtro
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: store procedure para listar el personal
-----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Personal_SeleccionarPorFiltro]  
(  
 @p_CodigoInterno VARCHAR(10) = '',  
 @p_NombreCompleto VARCHAR(80) = ''  
)  
AS  
BEGIN 
 SET NOCOUNT ON  
  
	SELECT  
	PE.[CodigoInterno],   
	PE.[CodigoUsuario],   
	PE.[CodigoCentroCosto],   
	CC.NombreCentroCosto,  
	PE.[ApellidoPaterno],   
	PE.[ApellidoMaterno],   
	PE.[PrimerNombre],   
	PE.[SegundoNombre],   
	PE.[CodigoCargo],   
	PE.[FechaCese],   
	PE.[IndicadorFuncionario],   
	PE.[FechaIngreso],   
	PE.[LibretaElectoral],   
	PE.[CodigoInternoVendedor],   
	PE.[CodigoSafp],  
	PE.[email_trabajo],
	PE.[email_personal],
	PE.[matricula],
	PE.ApellidoPaterno + ' ' + PE.ApellidoMaterno + ' ' + PE.PrimerNombre + ' ' + ISNULL(PE.SegundoNombre, '') AS 'NombreCompleto'  
	FROM  
	Personal AS PE  
	INNER JOIN CentrosCosto AS CC ON PE.CodigoCentroCosto = CC.CodigoCentroCosto  
	WHERE  
	CodigoInterno LIKE (CASE WHEN LEN(@p_CodigoInterno) = 0 THEN CodigoInterno ELSE @p_CodigoInterno + '%' END)  
	AND (ApellidoPaterno LIKE (CASE WHEN LEN(@p_NombreCompleto) = 0 THEN ApellidoPaterno ELSE @p_NombreCompleto + '%' END)  
	OR ApellidoMaterno LIKE (CASE WHEN LEN(@p_NombreCompleto) = 0 THEN ApellidoMaterno ELSE @p_NombreCompleto + '%' END)  
	OR PrimerNombre LIKE (CASE WHEN LEN(@p_NombreCompleto) = 0 THEN PrimerNombre ELSE @p_NombreCompleto + '%' END)  
	OR SegundoNombre LIKE (CASE WHEN LEN(@p_NombreCompleto) = 0 THEN SegundoNombre ELSE @p_NombreCompleto + '%' END))  
	--and FechaCese = 0 --and IndicadorFuncionario='01'  --CMB OT 65473 20120913

END			
GO

GRANT EXECUTE ON [dbo].[Personal_SeleccionarPorFiltro]  TO [rol_sit_fondos] AS [dbo]
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
CREATE PROCEDURE [dbo].[SP_SIT_LIS_ValorCuota_Mandatos](
	@p_CodigoPortafolioSBS varchar(20),
	@p_FechaOperacion numeric(8)
)
AS
BEGIN
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
	       cxc = vc.CXCPreCierre, --vc.CXCVentaTitulo + VC.OtrasCXC,
	       cxppre = vc.CXPPreCierre, --VC.CXPCompraTitulo + vc.OtrasCXP,
	       cxpcierre = vc.CXPCierre, --vc.OtrasCXPCierre,
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
	  AND vc.FechaProceso = @p_FechaOperacion AND p.PorSerie = 'N'
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
	       cxc = vc.CXCPreCierre,--vc.CXCVentaTitulo + VC.OtrasCXC,
	       cxppre = vc.CXPPreCierre,-- VC.CXPCompraTitulo + vc.OtrasCXP,
	       vc.CXPCierre,--vc.OtrasCXPCierre,
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
	  AND vc.FechaProceso = @p_FechaOperacion AND p.PorSerie = 'S'
END

GRANT EXECUTE ON [dbo].[SP_SIT_LIS_ValorCuota_Mandatos] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[MovimientoBancario_Extornar]'
 
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM SYS.procedures WHERE NAME = 'MovimientoBancario_Extornar')
	DROP PROCEDURE [dbo].[MovimientoBancario_Extornar]
GO
---------------------------------------------------------------------------------------------------------------------  
--Objetivo: Actualizar el monto de las operaciones de caja y saldos bancarios  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 06/09/2017  
-- Creado por: Ian Pastor  
-- Nro. Orden de Trabajo: 10749  
-- Descripcion del cambio: Agregar la funcionalidad de actualización de los montos de las operaciones de caja y  
--                         saldos bancarios para los días posteriores.  
---------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[MovimientoBancario_Extornar]  
 @p_CodigoOperacionCaja VARCHAR(7),  
 @p_CodigoPortafolio VARCHAR(10),  
 @p_Usuario VARCHAR(20),  
 @p_Fecha numeric(8),  
 @p_Hora VARCHAR(10)  
AS  
BEGIN  
 DECLARE @TipoMovimiento VARCHAR(20), @NumeroOperacion VARCHAR(12), @Importe numeric(22,7), @Egreso VARCHAR(1), @NumeroCuenta VARCHAR(25),  
 @FechaOp numeric(8),@CodOperacion VARCHAR(6), @FechaPosterior NUMERIC(8), @CodigoMercado VARCHAR(5)  
   
 SELECT @TipoMovimiento = oc.TipoMovimiento,@Importe = oc.Importe,@Egreso = ti.Egreso, @NumeroCuenta = oc.NumeroCuenta, @FechaOp=oc.FechaOperacion,              
  @CodOperacion=oc.CodigoOperacion, @CodigoMercado = CodigoMercado  
 FROM dbo.OperacionesCaja oc                
  INNER JOIN Operacion o on o.CodigoOperacion = oc.CodigoOperacion                
  INNER JOIN TipoOperacion ti on ti.CodigoTipoOperacion = o.CodigoTipoOperacion                
 WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 --###ANALISIS###          
 UPDATE OperacionesCaja SET Situacion='I',NumeroCarta = NULL,EstadoCarta = NULL,UsuarioModificacion =@p_usuario + '_X',FechaModificacion =@p_Fecha,  
 HoraModificacion = @p_Hora  
 WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio  
  
 SELECT  @NumeroOperacion  = NumeroOperacion  
 FROM OperacionesCaja  
 WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 ---Se anula las operaciones de tengan el mismo numero de operacion (caso de las divisas)  Cambiado por LC 05082008                
 DELETE FROM OperacionesCaja WHERE NumeroOperacion = @NumeroOperacion AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 UPDATE ImpresionCartas SET Estado = '5' WHERE CodigoOperacionCaja = @NumeroOperacion AND Estado = '1'  
 DELETE FROM ImpresionCartas WHERE CodigoOperacionCaja = @NumeroOperacion AND CodigoCarta IS NULL  
 DELETE FROM ClaveFirmantesCarta WHERE CodigoOperacionCaja = @NumeroOperacion  
 IF @NumeroOperacion IS NOT NULL                
 BEGIN                
        -- Operaciones automaticas  
  UPDATE CuentasPorCobrarPagar SET Estado = '',FechaPago=Null,UsuarioModificacion = @p_usuario + '_X',FechaModificacion = @p_Fecha,  
  HoraModificacion = @p_Hora  
  WHERE NumeroOperacion = @NumeroOperacion AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 END  
 IF (@CodOperacion not in ('63','64'))           
 BEGIN                 
  UPDATE SaldosBancarios  
  SET IngresosEstimados = case when (@Egreso = 'N') then (IngresosEstimados - @Importe) else IngresosEstimados end,  
  EgresosEstimados  = case when (@Egreso = 'S') then (EgresosEstimados - @Importe) else EgresosEstimados end,  
  UsuarioModificacion = @p_usuario + '_X',FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora  
  WHERE NumeroCuenta = @NumeroCuenta and FechaOperacion = @FechaOp  and CodigoPortafolioSBS = @p_CodigoPortafolio  
 END  
 IF (@CodOperacion  in ('63'))      
 BEGIN      
  UPDATE SaldosBancarios SET EgresosEstimados  = EgresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',  
  FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora  
  WHERE NumeroCuenta = @NumeroCuenta and FechaOperacion = @FechaOp and CodigoPortafolioSBS = @p_CodigoPortafolio  
 END  
 IF (@CodOperacion  in ('64'))        
 BEGIN      
  UPDATE SaldosBancarios SET IngresosEstimados = IngresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',  
  FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora    
  WHERE NumeroCuenta = @NumeroCuenta and FechaOperacion = @FechaOp and CodigoPortafolioSBS = @p_CodigoPortafolio         
 END  
 IF (@CodOperacion in ('63','64'))              
 BEGIN               
  DECLARE @NumeroCuentaDestino VARCHAR(25)              
  SELECT @NumeroCuentaDestino = oc.NumeroCuentaDestino FROM dbo.OperacionesCaja oc                
  INNER JOIN Operacion o on o.CodigoOperacion = oc.CodigoOperacion                
  INNER JOIN TipoOperacion ti on ti.CodigoTipoOperacion = o.CodigoTipoOperacion                
  WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio               
  DECLARE @operacionParalela VARCHAR(10)    
  SET @operacionParalela = @p_CodigoOperacionCaja  
  SET @operacionParalela = REPLACE(@operacionParalela,'T','')  
  SET @operacionParalela = REPLACE(@operacionParalela,'N','')  
  IF( @CodOperacion= '63')  
  BEGIN  
   SELECT @operacionParalela=codigooperacioncaja FROM OperacionesCaja  WHERE              
   codigooperacioncaja like '%' + Cast(Cast(@operacionParalela as int)+1 as VARCHAR(5))   and FechaOperacion = @FechaOp              
   and codigooperacion = '64'  
     
   UPDATE SaldosBancarios SET IngresosEstimados = IngresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',  
   FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora    
   WHERE NumeroCuenta = @NumeroCuentaDestino and FechaOperacion = @FechaOp          
  END  
  ELSE  
  BEGIN  
   UPDATE SaldosBancarios SET  EgresosEstimados  = EgresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',FechaModificacion = @p_Fecha,    
   HoraModificacion = @p_Hora    
   WHERE NumeroCuenta = @NumeroCuentaDestino AND FechaOperacion = @FechaOp  
   SELECT @operacionParalela=codigooperacioncaja FROM OperacionesCaja  WHERE  (codigooperacioncaja like '%'+ Cast(Cast(@operacionParalela as int)-1 as VARCHAR(5)))   and FechaOperacion = @FechaOp              
   and codigooperacion='63'
  END  
  --Se elimina la operacion complementaria             
  DELETE FROM OperacionesCaja WHERE codigooperacioncaja = @operacionParalela     
  UPDATE ImpresionCartas SET Estado = '5' WHERE CodigoOperacionCaja = @operacionParalela AND Estado = '1'  
  DELETE FROM ImpresionCartas WHERE CodigoOperacionCaja = @operacionParalela AND CodigoCarta IS NULL  
  DELETE FROM ClaveFirmantesCarta WHERE CodigoOperacionCaja = @operacionParalela   
 END  
 --Para las Comisiones Agentes de la ADMINISTRADORA del mercado extranjero  
 IF @CodOperacion = '67' and @p_CodigoPortafolio = 'ADMINISTRA' --###UPDATE###  
  DELETE FROM OperacionesCaja  
  WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja and CodigoPortafolioSBS <> @p_CodigoPortafolio  
 --> Saldos de Custodio.    
 --La lógica para actualizar los saldos del custodio estan en GeneraSaldosCustodioPorNemonico  
 DECLARE @codigoorden VARCHAR(10),@codigotercero VARCHAR(12),@codigocustodio VARCHAR(12),@codigonemonico VARCHAR(15)  
 SELECT @codigoorden =codigoorden FROM CuentasPorCobrarPagar WHERE numerooperacion = @NumeroOperacion  
 SELECT @codigotercero= codigotercero,@codigonemonico =codigomnemonico FROM ordeninversion WHERE codigoorden =@codigoorden  
 SELECT @Codigocustodio = dbo.ObtenerCustodio(@p_CodigoPortafolio,@CodigoNemonico,@codigotercero)  
 /*  
 EXEC GeneraSaldosCustodioPorNemonico @FechaOp,@CodigoNemonico,@CodigoCustodio,@p_CodigoPortafolio,@p_usuario,@p_fecha,@p_Hora,''  
 --Actualiza los saldos bancarios de la operacion si esta se ha realizado en dias anteriores  
 EXEC sp_SIT_DismunuyeSaldoBancoDiasPosteriores @FechaOp,@p_CodigoOperacionCaja,@p_CodigoPortafolio   
 EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio, @NumeroCuenta, @FechaOp   
 */  
   
 --OT10749 - Inicio  
 --Actualizar los saldos bancarios del día  
 EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,@NumeroCuenta,@FechaOp  
   
 --Actualizar los saldos bancarios para el día posterior  
 SET @FechaPosterior = dbo.DiaHabilSiguiente(@FechaOp,@CodigoMercado)  
 IF EXISTS(SELECT 1 FROM SaldosBancarios WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND @FechaPosterior = @FechaPosterior AND NumeroCuenta = @NumeroCuenta)  
 BEGIN  
  EXEC sp_SIT_ActualizaSaldoBancoDiasPosteriores @NumeroCuenta, @p_CodigoPortafolio, @FechaOp,@FechaPosterior,@p_usuario,@p_fecha,@p_Hora,''  
  EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,@NumeroCuenta,@FechaPosterior  
 END  
 --OT10749 - Fin  
END  
GO

GRANT EXECUTE ON [dbo].[MovimientoBancario_Extornar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto]'


USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto]
GO
-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Objetivo: OBTIENE LOS DATOS PARA GENERAR LAS CARTAS DE OPERACIONES DE FORWARD VENCIDOS  
-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 17/01/2019  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11732  
-- Descripcion del cambio: Nuevo  
-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 19/03/2019
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: Corregir script para obtener cartas de vencimiento de forward
-------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto]  
@p_CodigoOperacionCaja VarChar(12)   
AS  
BEGIN  
  
 DECLARE @BancoPublico VarChar(100)  
  
 SELECT   
 @BancoPublico =   
 T.Descripcion From ParametrosGenerales pg  
 JOIN Terceros T ON T.CodigoTercero = PG.Valor  
 WHERE Clasificacion = 'BANFONPUB'  
  
 --select IndicadorFondo , * from Portafolio P  
 --where IndicadorFondo = 'S' = PUBLICOS  = scotiabank  
 --                     = 'N'  = PRIVADOS =   
  
 SELECT  
 P.CodigoFondosMutuos,  
 NombrePortafolio = P.Descripcion,  
 NombreCompletoPortafolio = P.NombreCompleto,  
 FechaCarta = dbo.RetornarFechaCompleta(OI.FechaLiquidacion),  
 Banco = CASE WHEN P.IndicadorFondo  = 'S' THEN  @BancoPublico  ELSE T.Descripcion END,   
 RUCPortafolio = ISNULL(P.RUCPortafolio,''),  
 Operacion = CASE WHEN oc.CodigoOperacion  = '94' THEN  'Venta'  ELSE 'Compra' END,   
 O.Descripcion DescripcionOperacion,   
 SimboloMoneda = M.Simbolo,  
 Importe = ROUND(OC.Importe,2),   
 OI.CodigoMoneda,  
 OI.TipoCambioFuturo,  
 OI.TipoCambioSpot,  
 SimboloMonedaOrigen= (select SIMBOLO from Moneda MO where MO.CodigoMoneda=OI.CodigoMonedaOrigen),  
 SimboloMonedaDestino= (select SIMBOLO from Moneda MD where MD.CodigoMoneda=OI.codigoMonedaDestino),  
 OI.MontoOperacion,  
 OI.MontoCancelar,  
 OI.MontoNominalOperacion,  
 OI.MontoNominalOrdenado,  
 FechaOperacion   = DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion),  
 FechaLiquidacion = DBO.FN_SIT_OBT_FechaFormateada(OI.FechaLiquidacion),  
 OI.Plazo,  
 Modalidad = CASE WHEN OI.Delibery  = 'S' THEN 'Delivery Forward' ELSE 'No Delivery Forward' END,   
 BancoOrigen = (select BO.Descripcion FROM Terceros BO WHERE BO.CodigoTercero = OC.BancoOrigen),  
 SimboloCuenta = CASE WHEN CEO.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END,   
    OC.NumeroCuenta, -- Numero de cuenta Origen  
 BancoMatrizOrigen = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizOrigen) ,'') ,  
 CEO.NumeroCuentaInterBancario,  
 T.Descripcion DescripcionIntermediario , --tercero origen  
 BancoMatrizDestino = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizDestino) ,'') ,  
 NumeroCuentaInterBancarioDestino = (SELECT CE.NumeroCuentaInterBancario FROM CuentaEconomica CE   
          WHERE CE.NumeroCuenta = OC.NumeroCuentaDestino AND   
            CE.Situacion = 'A' AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS ) ,  
 BancoDestino = (select BD.Descripcion FROM Terceros BD WHERE BD.CodigoTercero = OC.BancoDestino ),  
 SimboloCuentaDestino= CASE WHEN  (select  CodigoMoneda  from CuentaEconomica CE INNER JOIN Entidad EN   
       ON CE.ENTIDADFINANCIERA = EN.CodigoEntidad  
       where CE.NumeroCuenta =OC.NumeroCuentaDestino  
          and en.CodigoTercero = oc.BancoDestino ) = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END ,  
 NumeroCuentaDestino,  
 DescripcionIntermediarioDestino = TD.Descripcion  ,  
 oc.ObservacionCarta ,  
 OC.ObservacionCartaDestino,  
 MC.Descripcion ModeloCarta ,   
 ISNULL(OC.CodigoOrden,OC.CodigoOperacionCaja) NumeroOrden,  
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
    OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2  
 FROM OperacionesCaja OC  
 JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS  
 JOIN Operacion O ON O.CodigoOperacion = OC.CodigoOperacion   
 LEFT JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen  
 JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo  AND MC.Situacion = 'A'  
       AND MC.CodigoOperacion = OC.CodigoOperacion  
 JOIN OrdenInversion OI ON OC.CodigoOperacionCaja = OI.CodigoOrden   
 JOIN Moneda M ON M.CodigoMoneda = OC.CodigoMoneda  
 LEFT JOIN CuentaEconomica CEO ON CEO.NumeroCuenta = OC.NumeroCuenta   
          AND CEO.Situacion = 'A'   
          AND CEO.CodigoPortafolioSBS = OC.CodigoPortafolioSBS   
    JOIN Entidad E ON E.CodigoEntidad = CEO.EntidadFinanciera AND E.Situacion = 'A'   
 --LEFT JOIN Terceros TD ON T.CodigoTercero = OC.CodigoTerceroDestino (OT 11876)
 LEFT JOIN Terceros TD ON TD.CodigoTercero = OC.CodigoTerceroDestino  
 LEFT JOIN Personal P1 ON P1.CodigoInterno = OC.VBADMIN  
 LEFT JOIN Personal P2 ON P2.CodigoInterno = OC.VBGERF1  
 LEFT JOIN Personal P3 ON P3.CodigoInterno = OC.VBGERF2  
 LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = OC.VBGERF1  
 LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = OC.VBGERF2  
   
 WHERE --OC.CodigoModelo <> 'SC01' AND OC.CodigoOperacion NOT IN ('63','BCRE')  AND   
 OC.CodigoOperacionCaja = --'054926'  
  @p_CodigoOperacionCaja-- IN ('054117','054118')
END  
GO

GRANT EXECUTE ON [dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'FIN --- > SECCIÓN DE SP'

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 
