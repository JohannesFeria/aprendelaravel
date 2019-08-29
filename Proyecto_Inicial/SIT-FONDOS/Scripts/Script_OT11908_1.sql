USE [SIT-FONDOS]
GO


BEGIN TRANSACTION __Transaction_Log
PRINT 'INICIO --- > SECCIÓN DE SP'
PRINT '[dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]'
 USE [SIT-FONDOS]
GO
/*
exec SP_Mandatos_ObtenerPrecioSucio_ValorCuota @p_CodigoPortafolio = '2666', @p_FechaProceso = 20190228
*/
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]
GO
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 20/03/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: creación del SP para mostrar los valores de inversiones total, rentabilidad e inversiones t-1 para mandatos, en valor cuota.
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]
(
	@p_CodigoPortafolio VARCHAR(10),
	@p_FechaProceso NUMERIC(8)
)
AS
BEGIN

	select 
	T.CodigoPortafolioSBS
	,SUM(T.PRELIM_ValorActual) as PRELIM
	,SUM(t.TIRRAZ_ValorActual)AS TIRRAZ
	,@p_FechaProceso as FechaProceso
	 from (
		--OPERACIONES DE RENTA FIJA
		SELECT
			VA.CodigoPortafolioSBS		
			,OI.CodigoOrden
			,TI.CodigoTipoInstrumentoSBS
			,TI.Descripcion AS DescripcionTipoInstrumento
			,'TIRRAZ_ValorActual' = VA.TIRRAZ_ValorActual 
			* dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
			,'PRELIM_ValorActual' = VA.PRELIM_ValorActual
				* dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/			
			,'TIRRAZ_ValorActual_PrecioLimpio' =( VA.TIRRAZ_PrecioLimpio * VA.TIRRAZ_SaldoNominalVigente) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso)
			, 'PRELIM_ValorActual_PrecioLimpio' =  (CASE WHEN ISNULL(VA.PRELIM_PrecioLimpio,0)=0 THEN 0 ELSE  (VA.PRELIM_PrecioLimpio * VA.PRELIM_SaldoNominalVigente) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso ) END)
		
		FROM ValorizacionAmortizada VA
			INNER JOIN OrdenInversion OI ON OI.CodigoOrden = VA.CodigoOrden AND OI.CodigoPortafolioSBS = VA.CodigoPortafolioSBS
			INNER JOIN Valores V ON VA.CodigoNemonico = V.CodigoNemonico
			INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
			INNER JOIN Portafolio P ON VA.CodigoPortafolioSBS = P.CodigoPortafolioSBS
			INNER JOIN VectorPrecio VP ON VA.CodigoNemonico = VP.CodigoMnemonico AND VP.Fecha = @p_FechaProceso
				AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(VA.CodigoPortafolioSBS,VA.CodigoNemonico,@p_FechaProceso)
		WHERE VA.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
			AND VA.FechaProceso = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
		UNION
		-----DEPÓSITOS A PLAZO
		SELECT
			CTV.CodigoPortafolioSBS		
		,OI.CodigoOrden
			,TI.CodigoTipoInstrumentoSBS
			,DescripcionTipoInstrumento = TI.Descripcion
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
			
		FROM CarteraTituloValoracion CTV
			INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
			INNER JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
			INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
			INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND OI.FechaOperacion = CTV.FechaValoracion
				AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
		WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
			AND OI.FechaContrato >= @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
		UNION
		------OPERACIONES DE RENTA VARIABLE
		SELECT
			OI.CodigoPortafolioSBS
			,OI.CodigoOrden
			,TI.CodigoTipoInstrumentoSBS
			,DescripcionTipoInstrumento = TI.Descripcion
		,ISNULL(OI.MontoNetoOperacion,0) AS TIRRAZ_ValorActual
			,ISNULL(OI.MontoNetoOperacion,0) AS PRELIM_ValorActual
			,0 AS TIRRAZ_ValorActual_PrecioLimpio
			,0 AS PRELIM_ValorActual_PrecioLimpio
		FROM OrdenInversion OI
			INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
			INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
			INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
			AND OI.FechaOperacion = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
			AND EXISTS(
				SELECT 1 FROM CarteraTituloValoracion CTV WHERE CTV.CodigoMnemonico = OI.CodigoMnemonico
				AND CTV.FechaValoracion = @p_FechaProceso AND CTV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS)
		) t 
		GROup by 
		t.CodigoPortafolioSBS
END
GO

GRANT EXECUTE ON [dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]  TO [rol_sit_fondos] AS [dbo]
GO

 
 
PRINT '[dbo].[SP_VALORCUOTA_TRAN_CalcularValoresCuotas]' 
USE [SIT-FONDOS]
GO
/*
exec SP_VALORCUOTA_TRAN_CalcularValoresCuotas @p_CodigoPortafolio = '2666', @p_FechaProceso = 20190228
*/
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_VALORCUOTA_TRAN_CalcularValoresCuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_VALORCUOTA_TRAN_CalcularValoresCuotas]
GO
----------------------------------------------------------------------------------------------------------
--Objetivo: Mostrar los datos iniciales para el calculo del valor cuota
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 18/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9851
--	Descripcion del cambio: Se agregan nuevos campos recuperados desde SIT
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 20/02/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10001
-- Descripcion del cambio: Se ajusta la fecha para los dias domingos y feriados
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 17/02/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9981
--	Descripcion del cambio: Se agrega la fecha incial del mes, ademas de validar los dias no validos
--  para los datos de operaciones caja
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/07/2017
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 10598
--	Descripcion del cambio: No tomar el día feriado al momento de calcular el valor de caja precierre
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/07/2017
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 10598
--	Descripcion del cambio: Separar el cálculo montos de dividendos del resultado de CXCVentaTitulo
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 17/04/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11169
--	Descripcion del cambio: No tomar los feriados al calcular la caja pre-cierre
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 22/06/2018
--	Modificado por: Ricardo Colonia
--	Proyecto: Fondos II
--	Descripcion del cambio: Obtener Movimiento de caja para nuevo concepto y cargar en CxC
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 22/06/2018
--	Modificado por: Carlos Rumiche
--	Proyecto: 11430
--	Descripcion del cambio: Obtener Divisas Compra o Venta con Fecha Liquidación Posterior al Cierre
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 27/07/2018
--	Modificado por: Ian Pastor Mendoza
--	Proyecto: 11450
--	Descripcion del cambio: Corrección del tipo de cambio para operaciones que tienen la misma moneda de
--                          su portafolio.
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 16/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Proyecto: 11512
--	Descripcion del cambio: Utilizar PrecioSucio o PrecioLimpio si uno de ellos es cero.
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 30/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Proyecto: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del tipo de cambio.
-----------------------------------------------------------------------------------------------------------
--  Fecha de Modificación: 02/10/2018  
--  Modificado por: Ricardo Colonia 
--  Nro. Orden de Trabajo: 11547
--  Descripción del cambio: Se agrega detalle de interes de aumento de capital.
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 31/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 14/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11687
--	Descripcion del cambio: Agregar valorización de SWAPS.
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 20/03/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: Se actualizaron los campos de inversiones total, rentabilidad e inversiones t-1 para mandatos.
-----------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SP_VALORCUOTA_TRAN_CalcularValoresCuotas]
(
	@p_CodigoPortafolioSBS varchar(10),
	@p_FechaOperacion numeric(8,0),
	@CodigoSerie varchar(100) = ''
)
AS
BEGIN
	DECLARE @ValoracionForwards as numeric(22,7),@CXCVentaTitulo as numeric(22,7),@CXPCompraTitulo as numeric(22,7),@CXPTituloLiberadas as numeric(22,7),
	@ValCuotaPreCierreAnt AS NUMERIC(22,7), @FechaCreacionFondo numeric(8), @CXCVentaTituloDividendos as numeric(22,7),
	@CajaPrecierre Numeric(22,7),@Suscripciones Numeric(22,7), @VCAnterior Numeric(22,7), @FechaInicioMes NUMERIC(8), @p_FechaOperacionFeriado NUMERIC(8),
	@CodigoMonedaPortafolio VARCHAR(10)
	,@ValorizacionSwaps AS NUMERIC(22,7)
	--@FactorValorPrimario NUMERIC(22,7)
	
	-- INICIO | RCE | Variable para almacenar el Importe en otras CxC | 22/06/2018
	DECLARE @montoCxC NUMERIC (22,7)
	-- FIN | RCE | Variable para almacenar el Importe en otras CxC
	
	--Si no hay registros se toma el dia ultimo registrado para las Operaciones de Caja
	SET @p_FechaOperacionFeriado = (
		SELECT MAX(SB.FechaOperacion)
		FROM SaldosBancarios SB
		WHERE SB.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			--AND SB.FechaOperacion NOT IN (SELECT Fecha FROM Feriado WHERE CodigoMercado = '1' AND Situacion = 'A')
			AND SB.FechaOperacion <= @p_FechaOperacion
		)
	--Fecha Inicio Mes
	SET @FechaInicioMes = CONVERT( DECIMAL(8), LEFT(@p_FechaOperacion,6) +  '01') --9981
	--Suscripciones
	SELECT @Suscripciones = SUM(Importe) FROM OperacionesCaja WHERE CodigoOperacion = '108' AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND FechaOperacion = @p_FechaOperacion AND Situacion = 'A' --9851
	--VC Anterior
	SELECT TOP 1 @VCAnterior = ValCuotaValoresCierre FROM ValorCuota WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND FechaProceso < @p_FechaOperacion
	AND CodigoSerie = @CodigoSerie ORDER BY FechaProceso DESC --9851
	--Suma de la caja de Recaudo e Inversiones
	SET @CajaPrecierre = DBO.sp_SIT_OBT_TotalCaja(@p_CodigoPortafolioSBS ,@p_FechaOperacionFeriado,'') --9851 --10001
	-- INICIO | RCE | Obteniedo valor de sumatoria de movimiento de caja para operaciones OP0089 | 22/06/2018
	SET @montoCxC = DBO.FN_RetornarCaja_OtrasCxC(@p_CodigoPortafolioSBS,@p_FechaOperacion)
	-- FIN | RCE | Obteniedo valor de sumatoria de movimiento de caja para operaciones OP0089 | 22/06/2018

	--Valor y tipo de cambio a la moneda del portafolio
	SELECT
	--@FactorValorPrimario = case when M.TipoCalculo  = 'I' THEN VTC.ValorPrimario else 1.0 / VTC.ValorPrimario end
	@CodigoMonedaPortafolio = P.CodigoMoneda
	FROM Portafolio P
	WHERE  P.CodigoPortafolioSBS = @p_CodigoPortafolioSBS	
	
	--Se realiza el calculo del REGAUX, que contiene los valores que necesitamos,
	--EL CALCULO DEVUELTO SIEMPRE ES EN LA MONEDA DEL PORTAFOLIO
	EXECUTE llenarTabla_VaxRegaux @p_CodigoPortafolioSBS,@p_FechaOperacion

	--PreCierre Anterior
	SET @ValCuotaPreCierreAnt = 0
	SELECT @ValCuotaPreCierreAnt = ValCuotaCierre FROM ValorCuota WHERE FechaProceso = DBO.fn_SIT_gl_DiaAnterior(@p_FechaOperacion) --9769
	AND CodigoSerie = @CodigoSerie AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS	

	--Valoración Forwards
	Select @ValoracionForwards = SUM(ROUND(ISNULL(MtmUSD,0),2)) From VectorForwardSBS VF
	JOIN OrdenInversion OI ON OI.CodigoISIN  = VF.NumeroPoliza  AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND OI.Estado <> 'E-ELI'
	Where Fecha = @p_FechaOperacion
	
	--Valoración SWAPS
	SELECT 
		@ValorizacionSwaps = CASE WHEN @CodigoMonedaPortafolio = 'DOL' THEN SUM(ROUND(ISNULL(Valorizacion,0),2) / 
																	        dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion))
															           ELSE SUM(ROUND(ISNULL(Valorizacion,0),2)) END
	FROM 
		ReporteVL RVL
	JOIN 
		OrdenInversion OI ON OI.CodigoISIN  = RVL.CodigoValor  
							 AND OI.CodigoPortafolioSBS = RVL.CodigoPortafolioSBS
						 
	WHERE 
		RVL.Fecha = @p_FechaOperacion
		AND RVL.ImprimeVL = '1'
		AND RVL.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		AND OI.Estado <> 'E-ELI'
		AND OI.Situacion = 'A'
		AND OI.CategoriaInstrumento = 'BS'
		AND OI.CodigoOperacion = '21'
	--Liberadas
	SELECT
	@CXPTituloLiberadas = SUM(
		((ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(@p_CodigoPortafolioSBS,DRL.CodigoNemonico, drl.FechaCorte),0,1)
		* (CASE WHEN ISNULL(VP.ValorPrecio,0) <> 0 THEN VP.PrecioSucio ELSE VP.PrecioLimpio END) )
		--* dbo.RetornarTipoCambioT_1(DRL.CodigoMoneda,'REAL',@p_FechaOperacion)) / dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion)
		* dbo.RetornarSecuenciaVectorTipoCambio(DRL.CodigoPortafolioSBS,DRL.CodigoMoneda,@p_FechaOperacion)) / dbo.RetornarSecuenciaVectorTipoCambio(DRL.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
	)
	FROM  DividendosRebatesLiberadas DRL
		INNER JOIN Valores VA on VA.CodigoNemonico = DRL.CodigoNemonico
		INNER JOIN TipoInstrumento TI on TI.CodigoTipoInstrumentoSBS = VA.CodigoTipoInstrumentoSBS
		LEFT JOIN VectorPrecio VP ON VP.CodigoMnemonico = DRL.CodigoNemonico AND VP.Fecha = @p_FechaOperacion
			AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico,@p_FechaOperacion)
		JOIN Moneda M ON M.CodigoMoneda = DRL.CodigoMoneda
		JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN DRL.CodigoMoneda = 'CAD' THEN 'DOL' ELSE DRL.CodigoMoneda END
			AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(DRL.CodigoPortafolioSBS,TC.CodigoMoneda,@p_FechaOperacion)
	WHERE  DRL.CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND  DRL.FechaCorte <= @p_FechaOperacion AND DRL.FechaEntrega > @p_FechaOperacion AND DRL.TipoDistribucion = 'L'
		AND DRL.Situacion = 'A'
	/*
	SELECT @CXPTituloLiberadas = SUM(CASE WHEN M.TipoCalculo  = 'D' THEN 
		(ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(@p_CodigoPortafolioSBS,DRL.CodigoNemonico, drl.FechaCorte),0,1) * VP.ValorPrecio) * TC.ValorPrimario 
	ELSE
		(ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(@p_CodigoPortafolioSBS,DRL.CodigoNemonico, drl.FechaCorte),0,1) * VP.ValorPrecio) / TC.ValorPrimario 
	END)
	FROM  DividendosRebatesLiberadas DRL
	INNER JOIN Valores VA on VA.CodigoNemonico = DRL.CodigoNemonico
	INNER JOIN TipoInstrumento TI on TI.CodigoTipoInstrumentoSBS = VA.CodigoTipoInstrumentoSBS
	LEFT JOIN VectorPrecio VP ON VP.CodigoMnemonico = DRL.CodigoNemonico AND VP.Fecha = @p_FechaOperacion
	JOIN Moneda M ON M.CodigoMoneda = DRL.CodigoMoneda
	JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN DRL.CodigoMoneda = 'CAD' THEN 'DOL' ELSE DRL.CodigoMoneda END
	WHERE  DRL.CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND  DRL.FechaCorte <= @p_FechaOperacion AND DRL.FechaEntrega > @p_FechaOperacion AND DRL.TipoDistribucion = 'L'
	AND DRL.Situacion = 'A'
	*/
	
	--CxP Compra de Título
	Select @CXPCompraTitulo = SUM(
		--(ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarTipoCambioT_1(OI.CodigoMoneda,'REAL',@p_FechaOperacion))
		--/ dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion)
		(ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
		/ dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
	)
	FROM OrdenInversion OI
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion AND O.CodigoTipoOperacion = '1'
		JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion 
			AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END 
			AND FechaOperacion <= @p_FechaOperacion
			AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,TC.Fecha)
		JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
	WHERE OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
		AND OI.FechaLiquidacion > @p_FechaOperacion  
		AND OI.Estado = 'E-CON' 
		AND OI.Situacion = 'A' 
		AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR')
	/*
	Select @CXPCompraTitulo = SUM(CASE WHEN M.TipoCalculo  = 'D' THEN 
									ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0)  * TC.ValorPrimario 
								ELSE 
									ROUND(ISNULL(OI.MontoNetoOperacion,MontoOperacion),7,0) / TC.ValorPrimario END)
	FROM OrdenInversion OI
	JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion AND O.CodigoTipoOperacion = '1'
	
	JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion 
		AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END 
		AND FechaOperacion <= @p_FechaOperacion
	JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
	
	WHERE OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND OI.FechaLiquidacion > @p_FechaOperacion  
	AND OI.Estado = 'E-CON' 
	AND OI.Situacion = 'A' 
	AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR')
	*/
	
	--CXC Venta de Título
	--Dividendos
	SELECT @CXCVentaTituloDividendos = SUM(
		--(ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarTipoCambioT_1(OI.CodigoMoneda,'REAL',@p_FechaOperacion))
		--/ dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion)
		(ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
		/ dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
	)
	FROM ordeninversion OI
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion
		JOIN DividendosRebatesLiberadas DVR ON DVR.TipoDistribucion + CONVERT(VARCHAR(10),Identificador ) = OI.CodigoOrden  AND DVR.CodigoPortafolioSBS = DVR.CodigoPortafolioSBS 
		AND DVR.Situacion = 'A' AND DVR.TipoDistribucion = 'D' --AND DVR.FechaCorte <= @p_FechaOperacion  AND DVR.FechaEntrega >=  @p_FechaOperacion
		JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END
			AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,@p_FechaOperacion)
		JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
		JOIN ClaseInstrumento CI ON CI.Categoria = OI.CategoriaInstrumento
	WHERE OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND OI.Estado <> 'E-ELI' AND OI.Situacion = 'A' And oi.FechaOperacion <= @p_FechaOperacion AND OI.FechaLiquidacion > @p_FechaOperacion
		AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR')
	/*
	SELECT @CXCVentaTituloDividendos = SUM(CASE WHEN M.TipoCalculo  = 'D' THEN
										ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * TC.ValorPrimario 
									ELSE 
										ROUND(ISNULL(OI.MontoNetoOperacion,MontoOperacion),7,0) / TC.ValorPrimario END)
	from ordeninversion oi
	JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion
	JOIN DividendosRebatesLiberadas DVR ON DVR.TipoDistribucion + CONVERT(VARCHAR(10),Identificador ) = OI.CodigoOrden  AND DVR.CodigoPortafolioSBS = DVR.CodigoPortafolioSBS 
	AND DVR.Situacion = 'A'  AND DVR.TipoDistribucion = 'D' --AND DVR.FechaCorte <= @p_FechaOperacion  AND DVR.FechaEntrega >=  @p_FechaOperacion
	JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END 
	JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
	JOIN ClaseInstrumento CI ON CI.Categoria = OI.CategoriaInstrumento
	WHERE OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND OI.Estado <> 'E-ELI' AND OI.Situacion = 'A' And oi.FechaOperacion <= @p_FechaOperacion AND OI.FechaLiquidacion > @p_FechaOperacion
	AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR')
	*/
	
	--Venta
	SELECT @CXCVentaTitulo = ISNULL(SUM(
		--( ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarTipoCambioT_1(OI.CodigoMoneda,'REAL',@p_FechaOperacion) )
		--/ dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion)
		( ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0) * dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion) )
		/ dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
	),0) + ISNULL(@CXPTituloLiberadas,0)
	FROM OrdenInversion OI
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion AND O.CodigoTipoOperacion IN ('2')
		JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END 
			AND FechaOperacion <= @p_FechaOperacion
			AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,@p_FechaOperacion)
		JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
	WHERE OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND OI.FechaLiquidacion > @p_FechaOperacion  
		AND OI.Estado = 'E-CON' AND OI.Situacion = 'A' AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR') AND NOT (OI.CodigoOperacion IN ('36','37'))
	/*
	Select @CXCVentaTitulo = ISNULL(SUM(CASE WHEN M.TipoCalculo  = 'D' THEN
		ROUND(ISNULL(MontoNetoOperacion,MontoOperacion),7,0)  * TC.ValorPrimario 
	ELSE 
		--ROUND(ISNULL(OI.MontoNetoOperacion,MontoOperacion),7,0) / TC.ValorPrimario END),0) + ISNULL(@CXPTituloLiberadas,0) + ISNULL(@CXCVentaTituloDividendos,0)
		ROUND(ISNULL(OI.MontoNetoOperacion,MontoOperacion),7,0) / TC.ValorPrimario END),0) + ISNULL(@CXPTituloLiberadas,0) --OTXXXX - 06/11/2017 - Ian Pastor M.
	FROM OrdenInversion OI
	JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion AND O.CodigoTipoOperacion IN ('2')
	JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END 
	AND FechaOperacion <= @p_FechaOperacion
	JOIN Moneda M ON M.CodigoMoneda = TC.CodigoMoneda
	WHERE OI.CodigoPortafolioSBS   = @p_CodigoPortafolioSBS AND OI.FechaLiquidacion > @p_FechaOperacion  
	AND OI.Estado = 'E-CON' AND OI.Situacion = 'A' AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR') AND NOT (OI.CodigoOperacion IN ('36','37'))
	*/

	-- INICIO | CRumiche | Divisas Compra o Venta con Fecha Liquidación Posterior | 2018-06-22
	DECLARE @CxP_DivisasLiquidacionPost AS NUMERIC(22,7), @CxC_DivisasLiquidacionPost AS NUMERIC(22,7)
	
	SELECT
		 @CxP_DivisasLiquidacionPost = SUM(CompraDivisas)
		,@CxC_DivisasLiquidacionPost = SUM(VentaDivisas)	
	FROM (
		SELECT
			 --CompraDivisas = (((CASE WHEN OI.CodigoOperacion = '65' /* 65: Compra Divisas */
				--THEN ISNULL(MontoNetoOperacion,MontoOperacion) ELSE 0 END) * dbo.RetornarTipoCambioT_1(OI.CodigoMoneda,'REAL',@p_FechaOperacion))
				--/ dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion))
			CompraDivisas = (((CASE WHEN OI.CodigoOperacion = '65' /* 65: Compra Divisas */
				THEN ISNULL(MontoNetoOperacion,MontoOperacion) ELSE 0 END) * dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
				/ dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion))
			--,VentaDivisas = (((CASE WHEN OI.CodigoOperacion = '66' /* 66: Venta Divisas */
			--	THEN ISNULL(MontoNetoOperacion,MontoOperacion) ELSE 0 END) * dbo.RetornarTipoCambioT_1(OI.CodigoMoneda,'REAL',@p_FechaOperacion))
			--	/ dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion))
			,VentaDivisas = (((CASE WHEN OI.CodigoOperacion = '66' /* 66: Venta Divisas */
				THEN ISNULL(MontoNetoOperacion,MontoOperacion) ELSE 0 END) * dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
				/ dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion))
		FROM ordeninversion OI
			JOIN VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END
				AND FechaOperacion <= @p_FechaOperacion
				AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,TC.Fecha)
		WHERE OI.CodigoOperacion IN ('65','66') -- 65: Compra Divisas , 66: Venta de Divisas
			AND OI.CategoriaInstrumento = 'CV' -- Negciacion de Divisas	
			AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
			AND OI.Estado <> 'E-ELI' 
			AND OI.Situacion = 'A'
			And OI.FechaOperacion <= @p_FechaOperacion
			AND OI.FechaLiquidacion > @p_FechaOperacion
	) AS R	
	-- FIN | CRumiche | Divisas Compra o Venta con Fecha Liquidación Posterior | 2018-06-22
		
  -- Inicio | Aumento de Capital | 04/10/2018
DECLARE @totalInteresAumentoCapital NUMERIC(22,7)
SELECT  
 @totalInteresAumentoCapital = (SUM(ACD.InteresCorrido* dbo.RetornarTipoCambioT_1(O.CodigoMoneda,'REAL',@p_FechaOperacion))/
								    dbo.RetornarTipoCambioT_1(@CodigoMonedaPortafolio,'REAL',@p_FechaOperacion))
FROM 
  OrdenInversion O  
INNER JOIN
  AumentoCapitalDetalle ACD ON ACD.CodigoOrden = O.CodigoOrden
INNER JOIN 
  AumentoCapital AC ON AC.idAumentoCapital= ACD.idAumentoCapital
INNER JOIN 
  Portafolio P ON O.CodigoPortafolioSBS = P.CodigoPortafolioSBS  
INNER JOIN 
  CuentasPorCobrarPagar CxP ON CxP.CodigoOrden = O.CodigoOrden 
							   AND CxP.CodigoPortafolioSBS = O.CodigoPortafolioSBS
							   AND CxP.NumeroOperacion = O.CodigoOrden
							   AND CxP.CodigoOperacion = O.CodigoOperacion
							   AND CxP.CodigoNemonico = O.CodigoMnemonico
WHERE 
  O.CodigoPortafolioSBS = @p_CodigoPortafolioSBS  
  AND O.Situacion='A'  
  AND O.CodigoOperacion IN ('35')  
  AND O.Estado = 'E-CON'
  AND O.FechaOperacion <= @p_FechaOperacion
  AND CxP.FechaVencimiento > @p_FechaOperacion
  AND P.FlagAumentoCapital = 1
  AND AC.Estado = 'A'
 
 -- Fin | Aumento de Capital | 04/10/2018   
	--Seleccion de los valores calculados
	SELECT @FechaCreacionFondo = FechaCreacionFondo FROM Portafolio WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS

	-- INICIO MANDATOS - TABLA ValorizacionAmortizada
	DECLARE @tbMandatosSumaActual TABLE (MandatosPortafolio varchar(8),MandatosSumaPrelim numeric(22,7),MandatosSumaTirraz numeric(22,7),Fecha numeric(8,0),Rentabilidad numeric(22,7),MandatosSumaPrelimAnterior numeric(22,7))
	DECLARE @tbMandatosSumaAnterior TABLE (MandatosPortafolio varchar(8),MandatosSumaPrelim numeric(22,7),MandatosSumaTirraz numeric(22,7),Fecha numeric(8,0))

	declare @EsMandato int=0	
	declare @TipoNegocio varchar(10)=''


	SELECT @EsMandato= CASE WHEN  TipoNegocio='MANDA' THEN 1 ELSE 0 END, @TipoNegocio=TipoNegocio  FROM Portafolio where Situacion='A' and  CodigoPortafolioSBS=@p_CodigoPortafolioSBS
	 IF @EsMandato=1
	 BEGIN
		 INSERT INTO @tbMandatosSumaActual(MandatosPortafolio,MandatosSumaPrelim,MandatosSumaTirraz,Fecha)
		 exec SP_Mandatos_ObtenerPrecioSucio_ValorCuota @p_CodigoPortafolioSBS , @p_FechaOperacion  

		 declare @FechaAnteriorMandatos numeric(8,0) = ( select top 1 FechaProceso from ValorizacionAmortizada  WITH (NOLOCK) where FechaProceso<@p_FechaOperacion ORDER BY FechaProceso DESC )

		  INSERT INTO @tbMandatosSumaAnterior(MandatosPortafolio,MandatosSumaPrelim,MandatosSumaTirraz,Fecha)
		 exec SP_Mandatos_ObtenerPrecioSucio_ValorCuota @p_CodigoPortafolioSBS , @FechaAnteriorMandatos  

		 UPDATE @tbMandatosSumaActual
		SET MandatosSumaPrelimAnterior = tAnterior.MandatosSumaPrelim,
		   Rentabilidad = (tActual.MandatosSumaPrelim - tAnterior.MandatosSumaPrelim)	
		  from  @tbMandatosSumaActual as tActual 
		  INNER JOIN @tbMandatosSumaAnterior AS tAnterior ON tActual.MandatosPortafolio = tAnterior.MandatosPortafolio 
		  where  tActual.MandatosPortafolio = tAnterior.MandatosPortafolio 
	 END

	-- FIN MANDATOS - TABLA ValorizacionAmortizada
	SELECT 
		ISNULL(SUM(TotalCompras),0) ComprasT,
		ISNULL((SUM(TotalVentas)),0)  VentasyVencimientosT,		
		ISNULL(@ValoracionForwards,0) ValoracionForwards,
		ISNULL(@ValorizacionSwaps,0) ValorizacionSwaps,
		--OT11876 - 20/03/2018 
		CASE WHEN @EsMandato=0 THEN ISNULL(SUM(SaldoAnteriorMonedaLocal ),0) ELSE (SELECT ISNULL(MandatosSumaPrelimAnterior,0) from @tbMandatosSumaActual) END as InversionesT1,
		CASE WHEN @EsMandato=0 THEN ISNULL(SUM(TotalRentabilidad ),0) ELSE (SELECT ISNULL(Rentabilidad,0) from @tbMandatosSumaActual) END as  RentabilidadT,
		CASE WHEN @EsMandato=0 THEN
		ISNULL(SUM(SaldoAnteriorMonedaLocal),0) + ISNULL(SUM(TotalCompras),0) + ISNULL((SUM(TotalVentas)),0)  + ISNULL(SUM(TotalRentabilidad ),0) 
	    + ISNULL(@ValoracionForwards,0) + ISNULL(@ValorizacionSwaps,0) 
		ELSE (SELECT ISNULL(MandatosSumaPrelim,0) from @tbMandatosSumaActual) END as  InversionesSubTotal,
		@TipoNegocio as TipoNegocio,
		------------------------------		
		ISNULL(@CXCVentaTitulo,0) AS CXCVentaTitulo,
		ISNULL(@CXPCompraTitulo,0) AS CXPCompraTitulo,
		ISNULL(@ValCuotaPreCierreAnt,0) CuotaPreCierreAnterior,@FechaCreacionFondo AS FechaCreacionFondo,@CajaPrecierre AS CajaPrecierre, 
		ISNULL(@Suscripciones,0) Suscripcion, ISNULL(@VCAnterior,0) VCAnterior,@FechaInicioMes FechaInicioMes, --9851 -- 9981
		--OT10598 - 06/11/2017 - Ian Pastor M. Agregar el campo CxC de venta de titulos de dividendos.		
		ISNULL(@CXCVentaTituloDividendos,0) AS CXCVentaTituloDividendos
		,@montoCxC montoCxC -- RCE | Proyecto: Fondos II		
		-- INICIO | CRumiche | Divisas Compra o Venta con Fecha Liquidación Posterior | 2018-06-22
		,ISNULL(@CxP_DivisasLiquidacionPost, 0) AS CxP_DivisasLiquidacionPost
		,ISNULL(@CxC_DivisasLiquidacionPost, 0) AS CxC_DivisasLiquidacionPost
		-- FIN | CRumiche | Divisas Compra o Venta con Fecha Liquidación Posterior | 2018-06-22	
		-- Inicio | Aumento de Capital | 04/10/2018
		  ,ISNULL(@totalInteresAumentoCapital,0) totalInteresAumentoCapital
		-- Fin | Aumento de Capital | 04/10/2018
	FROM VaxRegaux VR
END
GO

GRANT EXECUTE ON [dbo].[SP_VALORCUOTA_TRAN_CalcularValoresCuotas]  TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_SIT_sel_RegistroInversiones]'
USE [SIT-FONDOS]
GO
/*
exec sp_SIT_sel_RegistroInversiones @FechaOperacion = 20190228 ,@CodigoPortafolioSBS = '2666'
*/
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_sel_RegistroInversiones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_sel_RegistroInversiones]
GO
----------------------------------------------------------------------------------------------------------------------
--Objetivo: LISTAR TERCEROS POR TIPO ENTIDAD
----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 01/06/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11339
--	Descripcion del cambio: Sólo debe mostrarse los datos de las inversiones sin importar que se haya liquidado o no.
----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 20/03/2019
--	Modificado por: Ernesto Galarza.
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: Se cambio el valor de TasaEfectiva que muestre el YTM para los que sean distinto a deposito a plazo.
----------------------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_sel_RegistroInversiones]
(
	@FechaOperacion Numeric(8), 
	@CodigoPortafolioSBS Varchar(5)
)
AS
BEGIN
	--OPERACIONES AL CONTADO Y DEPÓSITOS
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , CodigoEntidad = CASE WHEN CI.Categoria  IN ('CD','DP','OR','FW') THEN ISNULL( E.CodigoEntidad,'')
	ELSE 
		ISNULL( V.CodigoEmisor ,'')
	END
	,CodigoISIN = ISNULL((CASE WHEN OI.CategoriaInstrumento = 'DP' THEN '' ELSE OI.CodigoISIN END),''), 
	CodigoTipoTitulo = ISNULL( V.CodigoNemonico ,''),OI.FechaOperacion  , 
	CantidadOperacion = ISNULL(OI.CantidadOperacion ,0),Precio = ISNULL(OI.Precio,0),V.CodigoMoneda ,
	MontoNominalOperacion = (CASE WHEN OI.CategoriaInstrumento = 'DP' THEN OI.MontoNominalOperacion ELSE ISNULL(OI.MontoOperacion,0) END) ,
	MontoOperacion = (CASE WHEN OI.CodigoOperacion = '3' THEN OI.MontoNominalOperacion  ELSE OI.MontoNetoOperacion END),
	FechaVencimiento = (CASE WHEN OI.CodigoOperacion = '4' THEN 0 ELSE ISNULL(OI.FechaContrato, OI.FechaLiquidacion) END) ,
	Tercero = T.Descripcion,CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	TotalComisiones = (CASE WHEN ISNULL(ICO.CodigoOrdenPreOrden,'') <> '' THEN ISNULL(ICO.ComisionSMV,0) ELSE ISNULL( OI.TotalComisiones,0) END), 
	RestoComisiones = (CASE WHEN ISNULL(ICO.CodigoOrdenPreOrden,'') <> '' THEN ISNULL(ICO.OtraComision,0) ELSE 0 END) ,SUBYACENTE = '',ValorEjecucion = 0, FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),
	Prima = 0, OI.CodigoSBS  
	FROM OrdenInversion OI
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero 
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion AND OI.CodigoOperacion NOT IN ('35','36','37','38','39','65','66')
	JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico  
	JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS 
	JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	LEFT JOIN (
		SELECT T.CodigoOrdenPreOrden,T.CodigoPortafolioSBS,ComisionSMV = ISNULL(SUM(CASE WHEN T.CodigoComision LIKE '%SAB%' THEN T.ValorComision ELSE 0 END),0),
		OtraComision = ISNULL(SUM(CASE WHEN T.CodigoComision LIKE '%SAB%' THEN 0 ELSE T.ValorComision  END),0)
		FROM ImpuestosComisionesOrdenPreOrden T
		GROUP BY T.CodigoOrdenPreOrden, T.CodigoPortafolioSBS
	) ICO(CodigoOrdenPreOrden,CodigoPortafolioSBS,ComisionSMV,OtraComision)  ON OI.CodigoOrden = ICO.CodigoOrdenPreOrden AND OI.CodigoPortafolioSBS = ICO.CodigoPortafolioSBS
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza
	WHERE OI.FechaOperacion = @FechaOperacion AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS AND OI.CategoriaInstrumento NOT IN ('OR','FD')
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A' ORDER BY oi.CodigoOperacion  DESC
	--OPERACIONES DE REPORTE Y OPERACIONES DE PACTO
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , E.CodigoEntidad,
	CodigoISIN = CASE WHEN OI.CodigoOperacion IN ('101','3') THEN ISNULL(OI.CodigoISIN,'') ELSE '' END, 
	CodigoTipoTitulo = ISNULL(  OI.CodigoMnemonico  ,''),OI.FechaOperacion , 
	CantidadOperacion = ISNULL(OI.CantidadOperacion ,0),Precio = ISNULL(OI.MontoNominalOperacion/OI.CantidadOperacion,0),OI.CodigoMoneda , 
	OI.MontoNominalOperacion ,MontoOperacion = CASE WHEN OI.CodigoOperacion IN ('101','3') THEN OI.MontoNominalOperacion  ELSE OI.MontoOperacion END ,
	FechaVencimiento = (CASE WHEN OI.CodigoOperacion = '4' THEN 0 ELSE ISNULL(OI.FechaContrato, OI.FechaLiquidacion) END), 
	Tercero = T.Descripcion,CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	TotalComisiones = (ISNULL(COR.ComisionAIVenta,0) + ISNULL(COR.ComisionAICompra,0)), 
	RestoComisiones = (ISNULL(COR.ImpuestoCompra,0) + ISNULL(COR.ImpuestoVenta,0) + ISNULL(COR.RestoComisionCompra,0) + ISNULL(COR.RestoComisionVenta,0)),
	SUBYACENTE = '',ValorEjecucion = 0,FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),Prima = 0, OI.CodigoSBS 
	FROM OrdenInversion OI
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero 
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	LEFT JOIN ComisionesOR COR ON OI.CodigoOrden = COR.CodigoOrden
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza 
	WHERE OI.FechaOperacion = @FechaOperacion AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND OI.CategoriaInstrumento = 'OR'
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A'
	--CUPONES Y DIVIDENDOS COBRADOS
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , CodigoEntidad = V.CodigoEmisor ,CodigoISIN = ISNULL(OI.CodigoISIN,''), CodigoTipoTitulo = ISNULL(v.CodigoNemonico  ,'') ,OI.FechaOperacion , 
	CantidadOperacion = CantidadOrdenado ,Precio = ISNULL(OI.Precio,0),OI.CodigoMoneda , MontoNominalOperacion = 0 ,OI.MontoOperacion,
	FechaVencimiento = OI.FechaLiquidacion,Tercero = '',CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	TotalComisiones =ISNULL( OI.TotalComisiones,0) , RestoComisiones = 0,SUBYACENTE = '',ValorEjecucion = 0, FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),Prima = 0, OI.CodigoSBS 
	FROM OrdenInversion OI
	LEFT JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico 
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza 
	WHERE OI.FechaOperacion = @FechaOperacion AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND OI.CodigoOperacion IN ('35','36','37','38','39')
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A' ORDER BY oi.CodigoOperacion  DESC
	--DERIVADOS
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , E.CodigoEntidad,CodigoISIN = ISNULL(OI.CodigoISIN,''), OI.CodigoMNemonico AS CodigoTipoTitulo ,OI.FechaOperacion , 
	CantidadOperacion = ISNULL(OI.CantidadOperacion ,0),Precio = ISNULL(OI.Precio,0),OI.CodigoMoneda, 
	MontoNominalOperacion = CASE WHEN OI.CodigoOperacion = '93' THEN OI.MontoCancelar ELSE OI.MontoNominalOperacion END,
	MontoOperacion = CASE WHEN OI.CodigoOperacion = '93' THEN OI.MontoCancelar ELSE OI.MontoOperacion END,
	FechaVencimiento = OI.FechaLiquidacion,Tercero = T.Descripcion,CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	ISNULL(OI.TotalComisiones,0) TotalComisiones, RestoComisiones = 0,SUBYACENTE = 'DIVISAS',
	ValorEjecucion = CASE WHEN OI.CodigoOperacion = '93' THEN OI.MontoOperacion ELSE 0 END,
	FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),
	Prima = CASE WHEN OI.CodigoOperacion = '93' THEN OI.TipoCambioFuturo ELSE 0 END,
	OI.CodigoSBS 
	FROM OrdenInversion OI
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	--LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza 
	WHERE OI.FechaOperacion = @FechaOperacion AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND OI.CodigoMnemonico  = 'FORWARD'
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A'
	--Nombre completo portafolio	
	SELECT NombreCompleto = ISNULL(NombreCompleto,'') FROM Portafolio WHERE CodigoPortafolioSBS = @CodigoPortafolioSBS
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_sel_RegistroInversiones]  TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Valorizacion_listar]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Valorizacion_listar]    Script Date: 03/12/2019 09:45:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valorizacion_listar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Valorizacion_listar]
GO


---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Obtiene lista de Portafolio x Valorizar.    
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificaci¾n: 17/07/2018    
-- Modificado por: Diego    
-- Nro. Orden de Trabajo: 11450
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificaci¾n: 06/08/2018    
-- Modificado por: Ian Pastor M.    
-- Nro. Orden de Trabajo: 11432    
-- Descripcion del cambio: Discriminar vector precio por su tipo de fuente: MANUL o PIP    
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificaci¾n: 31/08/2018  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11547  
-- Descripcion del cambio: Se agrega Filtro de carga de archivo precio forward   
---------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion: 31/08/2018
-- Modificado por: Ian Pastor Mendoza
-- Nro. Orden de Trabajo: 11590
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecuci¾n del VECTOR PRECIO
---------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion: 11/12/2018
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11698
-- Descripcion del cambio: Se agrega validaci¾n para portafolio que no tengan OI vigentes.
--------------------------------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion: 24/01/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11636    
-- Descripcion del cambio: Se adiciona validaci¾n por fecha de vencimiento en tipo renta variable y se implementa validaci¾n de portafolio 
--						   cuando se reciÚn se ha creado sin negociaciones.
--------------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 20/02/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11825    
-- Descripcion del cambio: Se adiciona validaci¾n de icono de advertencia cuando la fecha de valoraci¾n no coincide con la última fecha 
--						   de valorizaci¾n.
--------------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 12/03/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11908    
-- Descripcion del cambio: Se adiciona validaci¾n para valorización de fondo nuevo y no muestre icono de advertencia.
--------------------------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[Valorizacion_listar]    
(    
	@p_FechaOperacion NUMERIC(8,0),
	@p_TipoNegocio VARCHAR(10),
	@p_estado CHAR(2)    
)    
AS
BEGIN    
	CREATE TABLE #valorizacion      
	( ID INT IDENTITY(1,1),    
	codigoPortafolio VARCHAR(10),    
	fondo VARCHAR(40),    
	FechaConstitucion NUMERIC(8,0),    
	estado INT,     
	archivosPIP INT,    
	cierreCaja INT )    

	CREATE TABLE #lstNemonico    
	(ID INT IDENTITY(1,1),    
	CodigoMnemonico VARCHAR(15))    

	DECLARE @CODIGOPORTAFOLIO VARCHAR(10),    
			@PIP INT ,   
			@CAJA INT,    
			@FECHA NUMERIC(8),    
			@FECHAAYER DATETIME,    
			@CodigoMnemonico VARCHAR(15),   
			@COUNT INT,    
			@FILA INT = 1,    
			@COUNTNEMONICO INT,   
			@COUNTVL INT,
			@FERIADO INT,   
			@FINSEMANA INT,    
			@VectorPrecioVal VARCHAR(4),
			@COUNTVPF INT,
			@valorizacionMensual CHAR(1),
			@esFondoNuevo CHAR(1)
	
	SET @CAJA = 1    

	IF @p_estado = '0'    
	BEGIN
		INSERT #valorizacion    
		SELECT P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion, 0,0,0    
		FROM Portafolio P    
		WHERE @p_FechaOperacion <= P.FechaConstitucion AND P.TipoNegocio = @p_TipoNegocio
		AND P.Situacion = 'A' AND P.CodigoPortafolioSBS NOT IN (SELECT DISTINCT CTV.CodigoPortafolioSBS FROM CarteraTituloValoracion CTV    
		WHERE CTV.FechaValoracion =  @p_FechaOperacion AND escenario='REAL')-- AND P.CodigoPortafolioSBS = '57'    
		GROUP BY P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion
	END    
	ELSE IF @p_estado = '1'    
	BEGIN

		INSERT #valorizacion
		SELECT P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion, 1,0,0
		FROM Portafolio P
		--WHERE P.FechaConstitucion >= @p_FechaOperacion AND    
		WHERE @p_FechaOperacion <= P.FechaConstitucion AND P.TipoNegocio = @p_TipoNegocio
		AND P.Situacion = 'A' AND P.CodigoPortafolioSBS IN (SELECT DISTINCT CTV.CodigoPortafolioSBS FROM CarteraTituloValoracion CTV
		WHERE CTV.FechaValoracion =  @p_FechaOperacion AND escenario='REAL') --AND P.CodigoPortafolioSBS = '57'
		GROUP BY P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion

	END    
	ELSE     
	BEGIN    
		INSERT #valorizacion
		SELECT P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion, 0,0,0
		FROM Portafolio P
		WHERE P.FechaConstitucion >= @p_FechaOperacion AND P.TipoNegocio = @p_TipoNegocio
		AND P.Situacion = 'A' AND P.CodigoPortafolioSBS NOT IN (SELECT DISTINCT CTV.CodigoPortafolioSBS FROM CarteraTituloValoracion CTV
		WHERE CTV.FechaValoracion =  @p_FechaOperacion AND escenario='REAL')-- AND P.CodigoPortafolioSBS = '57'
		GROUP BY P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion
		
		UNION
		
		SELECT P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion, 1,0,0
		FROM Portafolio P
		WHERE P.FechaConstitucion >= @p_FechaOperacion AND P.TipoNegocio = @p_TipoNegocio
		AND P.Situacion = 'A' AND P.CodigoPortafolioSBS IN (SELECT DISTINCT CTV.CodigoPortafolioSBS FROM CarteraTituloValoracion CTV
		WHERE CTV.FechaValoracion =  @p_FechaOperacion AND escenario='REAL')-- AND P.CodigoPortafolioSBS = '57'
		GROUP BY P.CodigoPortafolioSBS,P.Descripcion,P.FechaConstitucion

	END    

	SET @FERIADO = (SELECT COUNT(F.Fecha) FROM Feriado F WHERE F.Fecha = @p_FechaOperacion AND Situacion = 'A')    
	SET @FINSEMANA = ISNULL((SELECT DATEPART(dw, CONVERT(DATETIME, CONVERT(CHAR(8), @p_FechaOperacion)))),0)    

	IF (@FERIADO > 0 OR @FINSEMANA = 1 OR @FINSEMANA = 7)
	BEGIN
		SET @COUNT = (SELECT COUNT(VP.Fecha) FROM VectorPrecio VP WHERE VP.Fecha = @p_FechaOperacion)
		SET @COUNTVPF = (SELECT COUNT(VPF.Fecha) FROM VectorForwardSBS VPF WHERE VPF.Fecha = @p_FechaOperacion)
		IF @COUNT > 4 AND @COUNTVPF > 4
		BEGIN
			SET @PIP = 1
			UPDATE #valorizacion SET archivosPIP = @PIP, cierreCaja = @CAJA
		END
		SET @COUNT = (SELECT COUNT(id) FROM #valorizacion)
		WHILE (@FILA <= @COUNT)
		BEGIN
			SET @PIP = 1
			SET @CODIGOPORTAFOLIO =  (SELECT codigoPortafolio FROM #valorizacion WHERE ID = @FILA)
			IF NOT EXISTS(
						  SELECT 1
						  FROM 
							  ValoracionCartera
						  WHERE
							  FechaValoracion =  dbo.RetornarFechaModificadaEnDias(@p_FechaOperacion,-1)
							  AND CodigoPortafolioSBS = @CODIGOPORTAFOLIO)
			BEGIN
				SET @valorizacionMensual = ISNULL((SELECT ValoracionMensual FROM Portafolio WHERE CodigoPortafolioSBS = @CODIGOPORTAFOLIO),'')
				IF @CODIGOPORTAFOLIO <> '43' AND UPPER(@valorizacionMensual) <> 'S' SET @PIP = 2
			END	
			UPDATE #valorizacion SET archivosPIP = @PIP	WHERE ID = @FILA
			SET @FILA =  @FILA +1
		END			
	END    
	ELSE    
	BEGIN    
		SET @COUNT = (SELECT COUNT(id) FROM #valorizacion)
		SET @FILA = 1
		WHILE (@FILA <= @COUNT)
		BEGIN
			DELETE FROM #lstNemonico
			SET @CODIGOPORTAFOLIO =  (SELECT codigoPortafolio FROM #valorizacion WHERE id = @FILA)
			--SET @VectorPrecioVal = (SELECT VectorPrecioVal FROM Portafolio WHERE CodigoPortafolioSBS = @CODIGOPORTAFOLIO)
			SET @VectorPrecioVal = (SELECT FuenteVector FROM Portafolio_SecuenciaVector WHERE Secuencia = 1 AND TipoVector = 'PRECIO' AND CodigoPortafolioSBS = @CODIGOPORTAFOLIO)
			SET @FECHAAYER = CONVERT(DATETIME, CONVERT(CHAR(8), @p_FechaOperacion))
			SET @FECHAAYER = DATEADD(day,-1, @FECHAAYER)
			SET @FECHA =(SELECT YEAR(@FECHAAYER) * 10000 + MONTH(@FECHAAYER) * 100 + DAY(@FECHAAYER))

			SET @PIP = (SELECT COUNT(OI.CodigoMnemonico) FROM OrdenInversion OI INNER JOIN ClaseInstrumento CI
			ON OI.CategoriaInstrumento = CI.Categoria
			WHERE
			CI.Categoria NOT IN ('CV','DP','FD','OR')
			AND OI.FechaOperacion = @p_FechaOperacion
			AND OI.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
			AND OI.Estado <> 'E-ELI')

			IF @PIP > 0
			BEGIN
				INSERT #lstNemonico
				SELECT OI.CodigoMnemonico FROM OrdenInversion OI
				INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria
				INNER JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico
				WHERE CI.Categoria NOT IN ('CV','DP','FD','OR')
				AND OI.FechaOperacion = @p_FechaOperacion
				AND OI.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND V.TipoRenta = 1
				AND (ISNULL(V.FechaVencimiento,0) = 0
				OR ISNULL(V.FechaVencimiento,0) > @p_FechaOperacion
				AND OI.Estado <> 'E-ELI')
				
				UNION
				
				SELECT OI.CodigoMnemonico FROM OrdenInversion OI
				INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria
				INNER JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico
				WHERE CI.Categoria NOT IN ('CV','DP','FD','OR')
				AND OI.FechaOperacion = @p_FechaOperacion
				AND OI.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND V.TipoRenta = 2
				AND OI.Estado <> 'E-ELI'
				AND (ISNULL(V.FechaVencimiento,0) = 0
				OR ISNULL(V.FechaVencimiento,0) > @p_FechaOperacion)
				
				UNION
				
				SELECT VL.CodigoNemonico FROM ReporteVL VL
				INNER JOIN Valores V ON VL.CodigoNemonico = V.CodigoNemonico
				INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS =  TI.CodigoTipoInstrumentoSBS
				INNER JOIN ClaseInstrumento CI ON TI.CodigoClaseInstrumento = CI.CodigoClaseInstrumento
				WHERE VL.Fecha =  @FECHA AND VL.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND CI.Categoria NOT IN ('CV','DP','FD','OR')
				AND V.TipoRenta = 1
				AND (ISNULL(V.FechaVencimiento,0) = 0 or ISNULL(V.FechaVencimiento,0) > @p_FechaOperacion)
				
				UNION
				
				SELECT  VL.CodigoNemonico FROM ReporteVL VL
				INNER JOIN Valores V ON VL.CodigoNemonico = V.CodigoNemonico
				INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS =  TI.CodigoTipoInstrumentoSBS
				INNER JOIN ClaseInstrumento CI ON TI.CodigoClaseInstrumento = CI.CodigoClaseInstrumento
				WHERE VL.Fecha =  @FECHA AND VL.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND CI.Categoria NOT IN ('CV','DP','FD','OR')
				AND V.TipoRenta = 2
				AND (ISNULL(V.FechaVencimiento,0) = 0
				OR ISNULL(V.FechaVencimiento,0) > @p_FechaOperacion)

				IF EXISTS (SELECT 1 FROM #lstNemonico)
				BEGIN
					--SET @COUNTNEMONICO = (SELECT COUNT(VP.CodigoMnemonico) FROM VectorPrecio VP
					--WHERE VP.CodigoMnemonico IN (SELECT CodigoMnemonico FROM #lstNemonico)
					--AND VP.Fecha =  @p_FechaOperacion AND VP.EntidadExt = @VectorPrecioVal)
					
					SET @COUNTNEMONICO = (
						SELECT COUNT(VP.CodigoMnemonico) FROM VectorPrecio VP
							INNER JOIN #lstNemonico LN ON VP.CodigoMnemonico = LN.CodigoMnemonico
						WHERE VP.Fecha =  @p_FechaOperacion AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(@CODIGOPORTAFOLIO,LN.CodigoMnemonico,@p_FechaOperacion)
					)
					
					IF @COUNTNEMONICO > 0
					BEGIN
						IF @COUNTNEMONICO = (SELECT COUNT(ID) FROM #lstNemonico)
						BEGIN
							SET @PIP = 1
						END
						ELSE
						BEGIN
							SET @PIP=0
						END
					END
					ELSE
					BEGIN
						SET @PIP = 0
					END
				END
				ELSE
				BEGIN
					SET @PIP = 1
				END
			END
			ELSE
			BEGIN
				INSERT #lstNemonico
				SELECT VL.CodigoNemonico FROM ReporteVL VL
				INNER JOIN Valores V ON VL.CodigoNemonico = V.CodigoNemonico
				INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS =  TI.CodigoTipoInstrumentoSBS
				INNER JOIN ClaseInstrumento CI ON TI.CodigoClaseInstrumento = CI.CodigoClaseInstrumento
				WHERE VL.Fecha =  @FECHA AND VL.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND CI.Categoria NOT IN ('CV','DP','FD','OR')
				AND V.TipoRenta = 1
				AND (ISNULL(V.FechaVencimiento,0) = 0 or ISNULL(V.FechaVencimiento,0) > @p_FechaOperacion)
				
				UNION
				
				SELECT  VL.CodigoNemonico FROM ReporteVL VL
				INNER JOIN Valores V ON VL.CodigoNemonico = V.CodigoNemonico
				INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS =  TI.CodigoTipoInstrumentoSBS
				INNER JOIN ClaseInstrumento CI ON TI.CodigoClaseInstrumento = CI.CodigoClaseInstrumento
				WHERE VL.Fecha =  @FECHA AND VL.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND CI.Categoria NOT IN ('CV','DP','FD','OR')
				AND V.TipoRenta = 2
				
				IF EXISTS (SELECT 1 FROM #lstNemonico)
				BEGIN
					--SET @COUNTNEMONICO = (SELECT COUNT(VP.CodigoMnemonico) FROM VectorPrecio VP
					--WHERE VP.CodigoMnemonico IN (SELECT CodigoMnemonico FROM #lstNemonico)
					--AND VP.Fecha = @p_FechaOperacion AND VP.EntidadExt = @VectorPrecioVal)
					
					SET @COUNTNEMONICO = (
						SELECT COUNT(VP.CodigoMnemonico) FROM VectorPrecio VP
							INNER JOIN #lstNemonico LN ON VP.CodigoMnemonico = LN.CodigoMnemonico
						WHERE VP.Fecha = @p_FechaOperacion AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(@CODIGOPORTAFOLIO,LN.CodigoMnemonico,@p_FechaOperacion)
					)
					
					IF @COUNTNEMONICO > 0
					BEGIN
						IF @COUNTNEMONICO = (SELECT COUNT(ID) FROM #lstNemonico)
						BEGIN
							SET @PIP = 1
						END
						ELSE
						BEGIN
							SET @PIP = 0
						END
					END
					ELSE
					BEGIN
						SET @PIP = 0
					END
				END
				ELSE
				BEGIN
					SET @COUNTNEMONICO = (SELECT COUNT(VL.CodigoNemonico) FROM ReporteVL VL
					INNER JOIN OrdenInversion OI ON VL.CodigoNemonico = OI.CodigoOrden
					INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria
					WHERE VL.Fecha =  @FECHA
					AND VL.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
					AND CI.Categoria IN ('CV','DP','OR')
					AND OI.Estado <> 'E-ELI')

					SET @COUNTNEMONICO = @COUNTNEMONICO + (SELECT COUNT(VL.CodigoNemonico) FROM ReporteVL VL
					INNER JOIN OrdenInversion OI ON VL.CodigoNemonico = OI.CodigoMnemonico
					INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria
					WHERE VL.Fecha =  @FECHA
					AND VL.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
					AND CI.Categoria IN ('FD')
					AND OI.Estado <> 'E-ELI')

					SET @COUNTNEMONICO = @COUNTNEMONICO + (SELECT COUNT(OI.CategoriaInstrumento) FROM OrdenInversion OI
					INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria
					WHERE OI.FechaOperacion = @p_FechaOperacion
					AND OI.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
					AND CI.Categoria IN ('CV','DP','FD','OR')
					AND OI.Estado <> 'E-ELI')

					IF @COUNTNEMONICO > 0
					BEGIN
						SET @PIP = 1
					END
					ELSE
					BEGIN
						SET @PIP = 0
						IF EXISTS (SELECT 1 FROM CarteraTituloValoracion CTV    
											WHERE CTV.FechaValoracion =  @FECHA 
												  AND escenario='REAL'
												  AND CodigoPortafolioSBS = @CODIGOPORTAFOLIO)
						BEGIN
							SET @PIP = 1
						END
						ELSE IF NOT EXISTS (SELECT 1 FROM ValorCuota VC    
											WHERE CodigoPortafolioSBS = @CODIGOPORTAFOLIO)
						BEGIN
						 	SET @PIP = 1
						END
					END
				END
			END
			--    INICIO | Se verifica si se ha cargado el vector precio forward para los instrumentos negociados.   
			IF @PIP > 0
			BEGIN
				IF EXISTS (SELECT 1 FROM OrdenInversion OI WHERE OI.FechaOperacion = @p_FechaOperacion
				AND OI.Estado <> 'E-ELI'
				AND OI.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
				AND OI.CategoriaInstrumento = 'FD')
				BEGIN
					SET @COUNTNEMONICO = (SELECT COUNT(OI.CodigoMnemonico) FROM OrdenInversion OI
					INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria
					INNER JOIN VectorForwardSBS VFS ON VFS.NumeroPoliza = OI.CodigoISIN AND VFS.Fecha = OI.FechaOperacion
					WHERE OI.FechaOperacion = @p_FechaOperacion
					AND OI.Estado <> 'E-ELI'
					AND OI.CodigoPortafolioSBS = @CODIGOPORTAFOLIO
					AND CI.Categoria = 'FD')
					IF @COUNTNEMONICO > 0
					BEGIN
						SET @PIP = 1
					END
					ELSE
					BEGIN
						SET @PIP = 0
					END  
				END  
			END  
			--   FIN | Se verifica si se ha cargado el vector precio forward para los instrumentos negociados.   
			
			
			IF @PIP = 0    
			BEGIN    
				IF @CODIGOPORTAFOLIO = '43'
				BEGIN
					SET @PIP = 1
				END
			END
			ELSE
			BEGIN
				IF NOT EXISTS(
							  SELECT 1
							  FROM 
								ValoracionCartera
							  WHERE
								FechaValoracion =  dbo.RetornarFechaModificadaEnDias(@p_FechaOperacion,-1)
								AND CodigoPortafolioSBS = @CODIGOPORTAFOLIO)
				BEGIN
					SET @valorizacionMensual = ISNULL((SELECT TOP 1 ValoracionMensual FROM Portafolio WHERE CodigoPortafolioSBS = @CODIGOPORTAFOLIO),'')
					SET @esFondoNuevo = ISNULL((SELECT TOP 1 'N' FROM ValoracionCartera WHERE CodigoPortafolioSBS = @CODIGOPORTAFOLIO),'')
					IF @CODIGOPORTAFOLIO <> '43' AND UPPER(@valorizacionMensual) <> 'S' AND UPPER(@esFondoNuevo) = 'N' SET @PIP = 2
				END					
			END

			UPDATE #valorizacion SET archivosPIP = @PIP, cierreCaja = @CAJA WHERE  id = @FILA
			SET @FILA =  @FILA +1
		END
	END

	SELECT codigoPortafolio,fondo,FechaConstitucion,estado,archivosPIP,@CAJA AS 'cierreCaja' FROM #valorizacion
	GROUP BY  codigoPortafolio,fondo,FechaConstitucion,estado,archivosPIP,cierreCaja
	ORDER BY fondo ASC

	DROP TABLE #valorizacion
END

GO

GRANT EXECUTE ON [dbo].[Valorizacion_listar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_FormaValorizacion1]'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_FormaValorizacion1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_FormaValorizacion1]
GO

------------------------------------------------------------------------------------------------------------------------------------- 
--Objetivo: Guarda los registros de reporte VL para Certificado de depósito, Papeles comerciales, 
--Acciones, Bonos y Warrants
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha Creacion: 11/10/2016
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9396
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 05/12/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9683
--	Descripcion del cambio: Se mostrara el monto nominal en los instruementos de tipo Opcion
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 03/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9777
--	Descripcion del cambio: Se calcula el monto nominal en la moneda del fondo
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 26/07/2017
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 10599
--	Descripcion del cambio: Cálculo de amortización
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 25/09/2017
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 10813
--	Descripcion del cambio: Cálculo de amortización
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 21/03/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11169
--	Descripcion del cambio: Colocar cero si el valor de la TIR es negativo
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 06/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11432
--	Descripcion del cambio: Filtrar vector precio por su tipo de fuente => Manual o PIP
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 16/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11512
--	Descripcion del cambio: Reemplazar tabla VectorPrecioPIP por VectorPrecio
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 31/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del tipo de cambio
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 31/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO
------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha modificacion: 16/11/2018  
-- Modificado por: Ricardo Colonia  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del tipo de cambio en fn_TipoCambioVL
-------------------------------------------------------------------------------------------------------------------------------------  
--	Fecha modificacion: 15/01/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se modifico para que contemple las ordenes que pertecen a la clase facturas negociables
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 13/03/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: Se elimina restricción de generación de interesescorridocompra cuando la orden es CD
------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROC [dbo].[sp_SIT_FormaValorizacion1]
(
	@p_Fecha NUMERIC(8)
	,@p_CodigoPortafolioSBS VARCHAR(10)
)
AS
BEGIN
	--Forma valorizacion 1, Certificado de depósito, Papeles comerciales, Acciones y Bonos	
	INSERT INTO ReporteVL
	SELECT
		TipoRegistro = 'D', --1
		Administradora = 'M00019', --2
		Fondo = P.CodigoFondosMutuos , --3
		Fecha= @p_Fecha, --4
		TipoCodigoValor = V.TipoCodigoValor , --5
		CodigoValor = V.CodigoISIN , -- 6
		IdentificadorOperacion = '' , --7
		FormaValorizacion = CASE WHEN CI.Categoria = 'WA' THEN '7' ELSE '1' END,--8
		/*
		OT10813 - El cálculo del valor nominal de bonos es incorrecto. El cálculo real es las unidades disponibles por
				  por el valor unitario y por sus amortizaciones pendientes.
		MontoNominal =	CASE WHEN  CI.Categoria = 'AC' OR  CI.Categoria = 'FM' OR CI.Categoria = 'FI' THEN ROUND(CTV.Cantidad,2) 
							WHEN CI.Categoria = 'WA' THEN (CASE WHEN M.TipoCalculo = 'D' THEN V.ValorNominal * VTCo.ValorPrimario ELSE V.ValorNominal / VTCo.ValorPrimario END) --9683
							--WHEN (SELECT COUNT(1) FROM ParametrosGenerales WHERE Clasificacion = 'INST_AMORT' AND Valor = V.CodigoNemonico) > 0 THEN (CTV.ValorNominalMonedaOrigen * (dbo.CalcularSumaAmortizacionPendiente(ctv.FechaValoracion,V.CodigoNemonico)/100) * dbo.RetornarTipoCambioT_1(p.CodigoMoneda,'REAL',ctv.FechaValoracion) )
							WHEN (SELECT COUNT(1) FROM ParametrosGenerales WHERE Clasificacion = 'INST_AMORT' AND Valor = V.CodigoNemonico) > 0 THEN (CTV.ValorNominalMonedaOrigen * (dbo.CalcularSumaAmortizacionPendiente(ctv.FechaValoracion,V.CodigoNemonico)/100) )
						ELSE ROUND(CTV.ValorNominalMonedaOrigen  - ISNULL(dbo.NominalLiberado(P.CodigoPortafolioSBS ,V.CodigoISIN,@p_Fecha,'1'),0),2) END, ----9
		*/
		MontoNominal =	CASE WHEN  CI.Categoria = 'AC' OR  CI.Categoria = 'FM' OR CI.Categoria = 'FI' THEN ROUND(CTV.Cantidad,2) 
							WHEN CI.Categoria = 'WA' THEN (CASE WHEN M.TipoCalculo = 'D' THEN V.ValorNominal * VTCo.ValorPrimario ELSE V.ValorNominal / VTCo.ValorPrimario END) --9683
							WHEN CI.Categoria = 'FA' THEN (SELECT MontoNominal FROM PrevOrdenInversion where CodigoNemonico = V.CodigoNemonico) --11636
						ELSE ROUND((ctv.Cantidad * V.ValorUnitario * (dbo.CalcularSumaAmortizacionPendiente(ctv.FechaValoracion,V.CodigoNemonico)/100) ),2) END, ----9
		Precio = CASE WHEN  TI.CodigoClaseInstrumento IN('6','12') OR CI.Categoria = 'WA' THEN ROUND( CTV.ValorUnidad,6)
				 WHEN TI.CodigoClaseInstrumento IN('10') THEN ROUND(ISNULL(VP.PrecioSucio,CTV.ValorUnidad),6) ELSE ROUND(ISNULL(VP.PorcPrecioSucio ,0),6) END,--10
		Tipocambio = DBO.fn_TipoCambioVL(@p_Fecha,V.CodigoMoneda,@p_CodigoPortafolioSBS), --11
		MontoFinal =  CASE WHEN CI.Categoria = 'CD' THEN ROUND(DBO.CalcularMontoFinalCD(CTV.CodigoPortafolioSBS,V.CodigoISIN,CTV.FechaValoracion ),2) 
		WHEN CI.Categoria = 'LH' THEN CTV.ValorNominalMonedaOrigen WHEN CI.Categoria = 'PC' THEN CTV.Cantidad * V.ValorUnitario ELSE 0 END, --12
		Montoinversion =  ROUND(DBO.fn_MontoInversionVL(CTV.FechaValoracion, P.CodigoPortafolioSBS ,V.CodigoISIN),2),
		Fechaoperacion = DBO.fn_FechaOperacionVL (CTV.FechaValoracion, P.CodigoPortafolioSBS ,V.CodigoISIN,CI.Categoria),--14
		Fechainiciopagointeres = CASE WHEN CI.Categoria = 'AC' OR CI.Categoria = 'FM' OR CI.Categoria = 'FI'
									  THEN 0
									  ELSE DBO.fn_FechaOperacionVL (CTV.FechaValoracion, P.CodigoPortafolioSBS ,V.CodigoISIN,CI.Categoria) END,--15
		FechaVencimiento = CASE WHEN V.CodigoTipoInstrumentoSBS IN ('51','49') THEN 0  ELSE ISNULL(V.FechaVencimiento ,0) END,--16
		--InteresCorridoCompra = CASE WHEN CI.Categoria = 'CD' THEN 0 ELSE ROUND(DBO.CalcularInteresCorridoCompra(CTV.CodigoPortafolioSBS,V.CodigoISIN,CTV.FechaValoracion,'1'),2) END, --17
		InteresCorridoCompra = ROUND(DBO.CalcularInteresCorridoCompra(CTV.CodigoPortafolioSBS,V.CodigoISIN,CTV.FechaValoracion,'1'),2), --17
		InteresGanado = CASE WHEN (CI.Categoria = 'BO' OR CI.Categoria = 'PC' OR CI.Categoria ='CD' ) THEN
			ROUND(DBO.CalcularInteresGanadoBono(CTV.CodigoPortafolioSBS,V.CodigoISIN,CTV.FechaValoracion ,'1'),2)
		ELSE 0 END, --18
		GananciaPerdida = ROUND(CTV.VPNLocal,2,0),--19
		Valorizacion = ROUND(CTV.VPNLocal,2,0) , --20
		TipoInstrumento = CASE WHEN CI.Categoria = 'WA' THEN '5' ELSE '0' END, --21
		ClasificacionRiesgo = ISNULL(VP.ClasificacionRiesgo,''), --22
		ComisionContado = 0,--23
		ComisionPlazo = 0,--24
		--TIR = ROUND(ISNULL(PIP.TIR,0),6), --25
		TIR = ROUND(ISNULL(CASE WHEN VP.TIR >= 0 THEN VP.TIR ELSE 0 END,0),6), --25 OT11169
		Duracion = ISNULL(VP.Duracion,0), --26
		V.CodigoNemonico ,TI.Descripcion,V.CodigoEmisor, P.CodigoPortafolioSBS ,1, M.Descripcion
	FROM carteratitulovaloracion ctv
		JOIN Portafolio P ON P.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND (P.codigofondosmutuos IS NOT NULL OR P.codigofondosmutuos <> '')
		JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico AND V.Situacion = 'A'
		JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS AND TI.CodigoClaseInstrumento IN ('6','7','3','1','10','12','11','18','19')
		JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento
		--JOIN VectorTipoCambio VTC ON VTC.CodigoMoneda = V.CodigoMoneda AND VTC.Fecha  = CTV.FechaValoracion
		/*OT9777 INICIO */
		LEFT JOIN VectorTipoCambio VTCo ON VTCo.CodigoMoneda = V.CodigoMoneda AND VTCo.Fecha  = V.FechaEmision
			AND VTCo.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,V.CodigoMoneda,V.FechaEmision) AND VTCO.FechaCreacion IS NOT NULL											
		/*OT9777 FIN */
		JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda 
		--LEFT JOIN VectorPrecioPIP PIP ON PIP.CodigoISIN = V.CodigoISIN AND PIP.Fecha = CTV.FechaValoracion AND PIP.Escenario = P.VectorPrecioVal
		LEFT JOIN VectorPrecio VP ON VP.CodigoISIN = V.CodigoISIN AND VP.Fecha = CTV.FechaValoracion
			AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(ctv.CodigoPortafolioSBS,V.CodigoNemonico,CTV.FechaValoracion)
	WHERE ctv.fechavaloracion = @p_Fecha AND CTV.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
END


GO

GRANT EXECUTE ON [dbo].[sp_SIT_FormaValorizacion1] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_ins_PrecioValorCuota] '
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ins_PrecioValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ins_PrecioValorCuota]
GO


-----------------------------------------------------------------------------------------------------------
--Objetivo: Insertar los valores cuotas calculados en le tabla de precios cuando valoriza.
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/01/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripción del cambio: Creación
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/03/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11908
--	Descripción del cambio: Se cambia el cálculo del monto nominal teniendo en cuenta las amortizaciones 
--							en el precio devengado.
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_ins_PrecioValorCuota] 
	@p_CodigoPortafolioSBS VARCHAR(12), 
	@p_FechaProceso NUMERIC(8),
	@p_Usuario VARCHAR(100), 
	@p_FechaCreacion NUMERIC(8),
	@p_HoraCreacion VARCHAR(10), 
	@p_Host VARCHAR(100)
AS
BEGIN
	--Manual = C, siginifica que viene del valor cuota
	DECLARE 
		@CodigoNemonico VARCHAR(12), 
		@Precio NUMERIC(22,7), 
		@PorcPrecio NUMERIC(22,7),
		@fuenteVector VARCHAR(10)
		
	SELECT TOP 1
		@CodigoNemonico = CodigoNemonico 
	FROM 
		Valores 
	WHERE 
		CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		
	SET @fuenteVector = (SELECT 
							FuenteVector 
						 FROM 
							Portafolio_SecuenciaVector 
						 WHERE 
							CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
							AND TipoVector = 'PRECIO' 
							AND Secuencia = 1)
							   
	IF ISNULL(@CodigoNemonico,'') <> ''
	BEGIN
		SELECT 
			@Precio = ValCuotaValoresCierre 
		FROM 
			ValorCuota 
		WHERE 
			CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
			AND FechaProceso = @p_FechaProceso
			
		IF EXISTS(SELECT 1 
				  FROM VectorPrecio 
				  WHERE CodigoMnemonico = @CodigoNemonico 
					    AND Fecha = @p_FechaProceso)
			UPDATE 
				VectorPrecio 
			SET 
				ValorPrecio = @Precio, 
				Manual = 'C', 
				PrecioSucio = @Precio, 
				PrecioLimpio = @Precio
			WHERE 
				Fecha = @p_FechaProceso 
				AND CodigoMnemonico = @CodigoNemonico 
		ELSE
			INSERT INTO 
				VectorPrecio(
							 CodigoMnemonico,
							 Fecha,
							 EntidadExt,
							 ValorPrecio,
							 Situacion,
							 Manual,
							 UsuarioCreacion,
							 FechaCreacion,
							 HoraCreacion,
							 UsuarioModificacion,
							 FechaModificacion,
							 HoraModificacion,
							 Host,
							 PrecioLimpio,
							 PrecioSucio
							)
			VALUES (
					@CodigoNemonico,
					@p_FechaProceso,
					@fuenteVector,
					@Precio,
					'A',
					'C',
					@p_Usuario,
					@p_FechaCreacion,
					@p_HoraCreacion,
					@p_Usuario,
					@p_FechaCreacion,
					@p_HoraCreacion,
					@p_host,
					@Precio,
					@Precio
				   )
	END
	
	-- Generaci¾n de Precios para Instrumentos con Flag Precio Devengado
	DECLARE
		@tmpInstrumentosDevengados TABLE(
										  codigoMnemonico VARCHAR(20),
										  montoNominal DECIMAL(22,7),
										  interes DECIMAL(22,7),
										  cantidad DECIMAL(22,7),
										  codigoTipoCupon VARCHAR(4),
										  codigoIsin VARCHAR(20)
										)
	--DECLARE
	--		@p_CodigoPortafolioSBS VARCHAR(12)='47' , 
	--	@p_FechaProceso NUMERIC(8)=20190314
	DECLARE
		@FechaDiaSiguiente DATETIME = CONVERT(DATETIME, CONVERT(CHAR(8), @p_FechaProceso)),
		@FechaT1 DECIMAL(8)
	
		SET @FechaDiaSiguiente = DATEADD(DAY,1, @FechaDiaSiguiente)
		SET @FechaT1 =(SELECT YEAR(@FechaDiaSiguiente) * 10000 + MONTH(@FechaDiaSiguiente) * 100 + DAY(@FechaDiaSiguiente))	
		
	INSERT INTO 
		@tmpInstrumentosDevengados 
	SELECT 
		VL.CodigoNemonico,
	--	ISNULL(VL.MontoNominal,0),
		ISNULL(CTV.Cantidad * VA.ValorUnitario * (dbo.CalcularSumaAmortizacionPendiente(CTV.FechaValoracion,VA.CodigoNemonico)/100),0),
		ISNULL((CASE WHEN VA.CodigoMoneda = 'DOL' THEN DBO.CalcularInteresGanadoBono(@p_CodigoPortafolioSBS,VA.CodigoISIN,@FechaT1 ,'X')
									              ELSE DBO.CalcularInteresGanadoBono(@p_CodigoPortafolioSBS,VA.CodigoISIN,@FechaT1 ,'X') * VTC.ValorPrimario END),0),
		ISNULL(CTV.Cantidad,0),
		ISNULL(VA.CodigoTipoCupon,'1'),
		ISNULL(VA.CodigoISIN,'')
	FROM 
		ReporteVL VL 
	JOIN
		CarteraTituloValoracion CTV ON VL.CodigoNemonico = CTV.CodigoMnemonico
									   AND VL.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS
									   AND VL.Fecha = CTV.FechaValoracion
									   
	JOIN
		Valores VA ON VL.CodigoNemonico = VA.CodigoNemonico
					  AND VL.CodigoValor = VA.CodigoISIN
	JOIN 
		TipoInstrumento TI ON VA.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
	JOIN
		ClaseInstrumento CL ON TI.CodigoClaseInstrumento = CL.CodigoClaseInstrumento
							   AND CL.Categoria = 'BO'
	JOIN 
		VectorTipoCambio VTC ON VTC.CodigoMoneda = VA.CodigoMoneda 
								 AND VTC.Fecha = @p_FechaProceso     
								 AND VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,VA.CodigoMoneda,@p_FechaProceso)        
	WHERE 
		  VL.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		  AND VL.Fecha = @p_FechaProceso
		  AND VA.PrecioDevengado = '1'
		  
	IF EXISTS (SELECT 1 FROM @tmpInstrumentosDevengados)
	BEGIN
		MERGE VectorPrecio AS VP
		USING @tmpInstrumentosDevengados AS TMP
		ON (VP.CodigoMnemonico = TMP.codigoMnemonico
			AND VP.CodigoISIN = TMP.CodigoISIN
			AND VP.Fecha = @FechaT1
			AND VP.EntidadExt = @fuenteVector)
		WHEN MATCHED  THEN
		    UPDATE SET 
				ValorPrecio = (montoNominal + interes)/cantidad, 
				Manual = 'S', 
				PrecioSucio = (montoNominal + interes)/cantidad, 
				PrecioLimpio = (montoNominal + interes)/cantidad,
				PorcPrecioSucio = ((montoNominal + interes)/montoNominal)*100,
				PorcPrecioLimpio = ((montoNominal + interes)/montoNominal)*100,
				UsuarioModificacion = @p_Usuario,
				FechaModificacion = @p_FechaCreacion,
				HoraModificacion = @p_HoraCreacion,
				Host = @p_host
		WHEN NOT MATCHED THEN
			INSERT ( 
					CodigoMnemonico,
					Fecha,
					EntidadExt,
					ValorPrecio,
					Situacion,
					Manual,
					UsuarioCreacion,
					FechaCreacion,
					HoraCreacion,
					Host,
					PrecioLimpio,
					PrecioSucio,
					PorcPrecioLimpio,
					PorcPrecioSucio,
					CodigoISIN
				   )
			VALUES (
					codigoMnemonico,
					@FechaT1,
					@fuenteVector,
					(montoNominal + interes)/cantidad,
					'A',
					'S',
					@p_Usuario,
					@p_FechaCreacion,
					@p_HoraCreacion,
					@p_host,
					(montoNominal + interes)/cantidad,
					(montoNominal + interes)/cantidad,
					((montoNominal + interes)/montoNominal)*100,
					((montoNominal + interes)/montoNominal)*100,
					CodigoISIN
				   );
			
	END
END


GO

GRANT EXECUTE ON [dbo].[sp_SIT_ins_PrecioValorCuota] TO [rol_sit_fondos] AS [dbo]
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
-- Fecha Modificacion: 12/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11876
-- Descripcion del cambio: store procedure para listar el personal
-----------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificacion: 26/03/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 11908
-- Descripcion del cambio: se modifico para que listar registros que tengan el codigo de centro de costo vacio
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
	LEFT JOIN CentrosCosto AS CC ON PE.CodigoCentroCosto = CC.CodigoCentroCosto  
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





PRINT 'FIN --- > SECCIÓN DE SP'

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 
