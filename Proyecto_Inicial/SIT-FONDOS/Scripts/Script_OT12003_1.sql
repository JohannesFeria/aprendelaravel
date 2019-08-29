----------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: EJECUTAR SCRIPST Y OBJETOS DE BASE DE DATOS
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Creación		: 16/05/2019
-- Modificado por			: Ian Pastor M.
-- Nro. Orden de Trabajo	: 12003
-- Descripción				: -
----------------------------------------------------------------------------------------------------------------------------------------
USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log

update SaldosCarteraTitulo
set SALDODISPONIBLE = 0
where CODIGOPORTAFOLIOSBS = '104'
	and CodigoMnemonico in ('SURARPIDOL','SURARPISOL')
	and FECHASALDO BETWEEN 20190514 AND 20190517
GO

update CustodioSaldo
set SaldoInicialUnidades = 0
where CODIGOPORTAFOLIOSBS = '104'
	and CodigoMnemonico in ('SURARPIDOL','SURARPISOL')
	and FECHASALDO BETWEEN 20190514 AND 20190517
GO


PRINT 'INICIO - STORE PROCEDURES'
PRINT '[dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE NAME = 'SP_Mandatos_ObtenerPrecioSucio_ValorCuota')
	DROP PROCEDURE [dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 20/03/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: creación del SP para mostrar los valores de inversiones total, rentabilidad e inversiones t-1 para mandatos, en valor cuota.
----------------------------------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 09/05/2019
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: Filtrar las órdenes de depósitos en la fecha que se genera el reporte consolidado de mandatos.
----------------------------------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 21/05/2019
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12003
--	Descripcion del cambio: Optimización store procedure.
----------------------------------------------------------------------------------------------------------------------------------------------------------
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
		--WHERE VA.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		WHERE VA.CodigoPortafolioSBS = @p_CodigoPortafolio --OT12003
			AND VA.FechaProceso = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
		UNION
		-----DEPOSITOS A PLAZO
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
			INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND @p_FechaProceso = CTV.FechaValoracion
				AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
		--WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		WHERE CTV.CodigoPortafolioSBS = @p_CodigoPortafolio --OT12003
			AND OI.FechaContrato >= @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
		--UNION
		------OPERACIONES DE RENTA VARIABLE
		-- SELECT
			-- OI.CodigoPortafolioSBS
			-- ,OI.CodigoOrden
			-- ,TI.CodigoTipoInstrumentoSBS
			-- ,DescripcionTipoInstrumento = TI.Descripcion
		-- ,ISNULL(OI.MontoNetoOperacion,0) AS TIRRAZ_ValorActual
			-- ,ISNULL(OI.MontoNetoOperacion,0) AS PRELIM_ValorActual
			-- ,0 AS TIRRAZ_ValorActual_PrecioLimpio
			-- ,0 AS PRELIM_ValorActual_PrecioLimpio
		-- FROM OrdenInversion OI
			-- INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
			-- INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
			-- INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		-- WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
			-- AND OI.FechaOperacion = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
			-- AND EXISTS(
				-- SELECT 1 FROM CarteraTituloValoracion CTV WHERE CTV.CodigoMnemonico = OI.CodigoMnemonico
				-- AND CTV.FechaValoracion = @p_FechaProceso AND CTV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS)
		) t 
		GROup by 
		t.CodigoPortafolioSBS
END
GO

GRANT EXECUTE ON [dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Pr_Sit_listarValorCuota]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Pr_Sit_listarValorCuota]    Script Date: 05/21/2019 14:34:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pr_Sit_listarValorCuota]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 29/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se amplia ambito de variable portafolio a VARCHAR(50).
----------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 21/05/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 12003
--	Descripcion del cambio: Se incluye los nuevos campos que suman a CxC y CxP
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Pr_Sit_listarValorCuota](
	@p_codigoPortafolio VARCHAR(20),
	@p_fechaInicial NUMERIC(8),
	@p_fechaFinal NUMERIC(8)
)
AS
BEGIN
	DECLARE @Tbl AS TABLE(
		portafolio VARCHAR(40),
		FechaProceso Char(10),
		Serie VARCHAR(20),
		CuotasEmitidas NUMERIC(22,7),
		ImporteRecaudado NUMERIC(22,7),
		rescates NUMERIC(22,7),
		valorCuota NUMERIC(22,7),
		comisionAdministradora	NUMERIC(22,7), 
		totalInversiones NUMERIC(22,7),
		Caja NUMERIC(22,7),
		cxppre NUMERIC(22,7),
		cxc NUMERIC(22,7),
		cxppreImporteRecaudado	NUMERIC(22,7), 
		patrimonioPreCierre NUMERIC(22,7),
		patrimonioCierre NUMERIC(22,7), 
		cxpcierre NUMERIC(22,7),
		OtrosGastos NUMERIC(22,7),
		OtrosIngresos NUMERIC(22,7), 
		Fecha NUMERIC(8) 
	)
	INSERT INTO 
		@Tbl
	--Lista los portafolios no seriados
	SELECT 
		portafolio = p.Descripcion,
		FechaProceso = dbo.FN_SIT_OBT_FechaFormateada(vc.FechaProceso),
		Serie = '',
		CuotasEmitidas = vc.ValCuotaCierre,
		ImporteRecaudado = vc.AportesValores,
		rescates = vc.RescateValores,
		valorCuota = vc.ValCuotaValoresCierre,
		comisionAdministradora = vc.ComisionSAFM,
		totalInversiones = vc.InversionesSubTotal,
		vc.Caja,
		cxppre = VC.CXPPreCierre,
		cxc = VC.CXCPreCierre,
		cxppreImporteRecaudado = vc.OtrasCXP - vc.AportesValores,
		patrimonioPreCierre = vc.ValPatriPreCierre1,
		patrimonioCierre = vc.ValPatriCierreValores,
		cxpcierre = vc.OtrasCXPCierre,
		vc.OtrosGastos,
		vc.OtrosIngresos, 
		VC.FechaProceso 
	FROM 
		ValorCuota vc
	JOIN 
		Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	WHERE 
		(vc.CodigoPortafolioSBS = @p_codigoPortafolio OR @p_codigoPortafolio = '')
		 AND vc.FechaProceso BETWEEN @p_fechaInicial AND @p_fechaFinal 
		 AND p.PorSerie = 'N'
	UNION ALL
	--Lista los portafolios seriados
	SELECT
		portafolio = p.Descripcion,
		FechaProceso = dbo.FN_SIT_OBT_FechaFormateada(vc.FechaProceso),
		Serie = ISNULL(PS.NombreSerie,''),
		CuotasEmitidas = vc.ValCuotaCierre,
		ImporteRecaudado = vc.AportesValores,
		rescates = vc.RescateValores,
		valorCuota = vc.ValCuotaValoresCierre,
		comisionAdministradora = vc.ComisionSAFM,
		totalInversiones = vc.InversionesSubTotal,
		vc.Caja,
		cxppre = VC.CXPPreCierre,
		cxc = VC.CXCPreCierre,
		cxppreImporteRecaudado = vc.OtrasCXP - vc.AportesValores,
		patrimonioPreCierre = vc.ValPatriPreCierre1,
		patrimonioCierre = vc.ValPatriCierreValores,
		cxpcierre = vc.OtrasCXPCierre,
		vc.OtrosGastos,vc.OtrosIngresos, 
		VC.FechaProceso 
	FROM 
		ValorCuota vc
	JOIN 
		Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN 
		PortafolioSerie PS ON PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS 
							  AND VC.CodigoSerie = PS.CodigoSerie
	WHERE 
		(vc.CodigoPortafolioSBS = @p_codigoPortafolio OR @p_codigoPortafolio = '') 
		AND vc.FechaProceso BETWEEN @p_fechaInicial AND @p_fechaFinal 
		AND p.PorSerie = 'S'
		
	SELECT 
		portafolio,
		FechaProceso,
		Serie,
		CuotasEmitidas,
		ImporteRecaudado,
		rescates,
		valorCuota,
		comisionAdministradora,
		totalInversiones,
		Caja,
		cxppre,
		cxc,
		cxppreImporteRecaudado, 
		patrimonioPreCierre,
		patrimonioCierre,
		cxpcierre,
		OtrosGastos,	
		OtrosIngresos 
	FROM 
		@Tbl 
	ORDER BY  Fecha 
	--ORDER BY  vc.FechaProceso
END


GO

GRANT EXECUTE ON [dbo].[Pr_Sit_listarValorCuota] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]    Script Date: 05/21/2019 15:10:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]
GO


---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 29/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se amplia ambito de variable portafolio a VARCHAR(50).
----------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 21/05/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 12003
--	Descripcion del cambio: Se incluye los nuevos campos que suman a CxC y CxP
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado](
	@p_codigoPortafolio VARCHAR(20),
	@p_fechaInicial NUMERIC(8),
	@p_fechaFinal NUMERIC(8)
)
AS
BEGIN
	DECLARE @tTotalValorCuota AS TABLE(
		portafolio VARCHAR(50),
		fechaProceso NUMERIC(8),
		serie VARCHAR(20),
		cuotasEmitidas NUMERIC(22,7),
		valorCuota NUMERIC(22,7),
		comisionAdministradora NUMERIC(22,7),
		rescates NUMERIC(22,7),
		totalInversiones NUMERIC(22,7),
		caja NUMERIC(22,7),
		cxppre NUMERIC(22,7),
		cxc NUMERIC(22,7), 
		Fecha NUMERIC(8) 
	)
	
	DECLARE @tbl AS TABLE (
		portafolio VARCHAR(50),
		fechaProceso CHAR(10), 
		serie VARCHAR(20),
		cuotasEmitidas NUMERIC(22,7),
		valorCuota NUMERIC(22,7),
		comisionAdministradora NUMERIC(22,7),
		rescates NUMERIC(22,7),
		totalInversiones NUMERIC(22,7),
		caja NUMERIC(22,7),
		cxppre NUMERIC(22,7),
		cxc NUMERIC(22,7), 
		Fecha NUMERIC(8) 
	)
	
	INSERT INTO 
		@tTotalValorCuota
	SELECT 
		Portafolio = p.Descripcion,
		vc.FechaProceso,
		Serie = PS.NombreSerie,
		CuotasEmitidas = vc.ValCuotaCierre,
		valorCuota = ValCuotaValoresCierre,
		comisionAdministradora = ComisionSAFM,
		rescates = RescateValores,
		totalInversiones = vc.InversionesSubTotal,
		Caja = Caja,
		cxppre = CXPPreCierre,
		cxc = CXCPreCierre,
		vc.FechaProceso
	FROM 
		ValorCuota vc
	JOIN 
		Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN 
		PortafolioSerie PS ON PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS 
							  AND VC.CodigoSerie = PS.CodigoSerie 
	WHERE 
		(vc.CodigoPortafolioSBS = @p_codigoPortafolio OR @p_codigoPortafolio = '') 
		AND vc.FechaProceso BETWEEN @p_fechaInicial AND @p_fechaFinal
		AND p.PorSerie = 'S' 
		AND p.Situacion = 'A' 
	ORDER BY  
		VC.FechaProceso
	
	SELECT 
		portafolio,
		fechaProceso = dbo.FN_SIT_OBT_FechaFormateada(fechaProceso),
		serie,
		cuotasEmitidas,
		valorCuota,
		comisionAdministradora,
		rescates,
		totalInversiones,
		caja,
		cxppre,
		cxc
	FROM 
		@tTotalValorCuota
	ORDER BY  
		portafolio,
		Fecha,
		serie
	
	SELECT 
		portafolio,
		fechaProceso = dbo.FN_SIT_OBT_FechaFormateada(fechaProceso),
		totalInversiones = sum(totalInversiones),
		caja = sum(caja),
		cxppre = sum(cxppre),
		cxc = sum(cxc)
	FROM 
		@tTotalValorCuota
	GROUP BY 
		portafolio, 
		fechaProceso
END

GO

GRANT EXECUTE ON [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'FIN - STORE PROCEDURES'
GO

PRINT 'Inicio de Ejecucion - OT 12003'
GO

PRINT 'Insert Column Table Portafolio'
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Portafolio' AND COLUMN_NAME = 'TipoComision')
BEGIN
  ALTER TABLE Portafolio ADD TipoComision VARCHAR(10) NULL
END

PRINT 'Fin de Insert Column Table Portafolio'
GO

PRINT 'Insert Column Table ValorCuota'
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ValorCuota' AND COLUMN_NAME = 'AporteMandato')
BEGIN
  ALTER TABLE ValorCuota DROP COLUMN AporteMandato
END

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ValorCuota' AND COLUMN_NAME = 'RetiroMandato')
BEGIN
  ALTER TABLE ValorCuota DROP COLUMN RetiroMandato
END


IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ValorCuota' AND COLUMN_NAME = 'ComisionUnificadaCuota')
BEGIN
  ALTER TABLE ValorCuota ADD ComisionUnificadaCuota NUMERIC(22,7) NULL
END

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ValorCuota' AND COLUMN_NAME = 'ComisionUnificadaMandato')
BEGIN
  ALTER TABLE ValorCuota ADD ComisionUnificadaMandato NUMERIC(22,7) NULL
END

PRINT 'Fin de Insert Column Table ValorCuota'
GO

PRINT 'Insert Parametros Generales'
GO

DELETE FROM ParametrosGenerales WHERE Clasificacion = 'TipoComision'
GO
INSERT INTO ParametrosGenerales (clasificacion,Nombre,Valor,Comentario,Valor2) VALUES ('TipoComision','Diario','1','','')
GO
INSERT INTO ParametrosGenerales (clasificacion,Nombre,Valor,Comentario,Valor2) VALUES ('TipoComision','Mensual','2','','')
GO

PRINT 'Fin Parametros Generales'
GO

PRINT 'Inicio Procedure "Portafolio_Modificar"'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Portafolio_Modificar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Portafolio_Modificar]
GO
---------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 23/09/2016  
-- Modificado por: Marlon E. Peña  
-- Nro. Orden de Trabajo: 9362  
-- Descripción del cambio: Agregar parámetro CodigoRenta para actualizarlo en la tabla Portafolio.  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Creación: 19/07/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10442  
-- Descripción: Incluir el ingreso del campo Fecha Caja de Operaciones y valorización mensual  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 06/11/2017  
-- Modificado por: Hanz Cocchi G.  
-- Nro. Orden de Trabajo: 10916  
-- Descripcion del cambio: Agregar el parámetro "@p_CuotasLiberadas" y el campo "CuotasLiberadas"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 22/02/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11237  
-- Descripcion del cambio: Agregar el parámetro "@p_CPPadreSisOpe" y el campo "CPPadreSisOpe"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 22/02/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11432  
-- Descripcion del cambio: Agregar el parámetro "@p_VectorPrecioVal" y el campo "VectorPrecioVal"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 2018-08-23  
-- Modificado por: CRumiche  
-- Nro. Orden de Trabajo: OT11590  
-- Descripcion del cambio: Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 17/09/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11568      
-- Descripcion del cambio: Agregar el parámetro "@p_FlagAumentoCapital" y el campo "FlagAumentoCapital"      
----------------------------------------------------------------------------------------------------------------------------------------    
-- Fecha modificacion: 07/10/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Agregar el campo Fondo Cliente en el mantenimiento Administración de Portafolio. Es un cambio solicitado  
--                         para el desarrollo de Límites Fondo, dónde se identifica si un portafolio es un Cliente de una empresa o no.  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 24/10/2018  
-- Modificado por: CRumiche  
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Agregar el campo Cliente  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 23/05/2019  
-- Modificado por: Junior Huallullo P.  
-- Nro. Orden de Trabajo: 12003  
-- Descripcion del cambio: Agregar el campo Tipo Comision
----------------------------------------------------------------------------------------------------------------------------------------  
    
CREATE PROCEDURE [dbo].[Portafolio_Modificar] (    
 @p_CodigoPortafolioSBS varchar(10), -- Código Portafolio SBS    
 @p_Descripcion varchar(40),  -- Descripcion del registro    
 @p_FechaConstitucion numeric(8,0), -- Fecha de constitucion del registro    
 @p_CodigoTipoValorizacion varchar(3), -- Codigo del tipo de valorizacion del registro    
 @p_FechaTermino  numeric(8,0),    
 @p_FechaAperturaContable  numeric(8,0),  --20090225 LETV  
 @p_CodigoMoneda varchar(10),    
 @p_UsuarioModificacion varchar(15), -- Usuario que modifico el registro    
 @p_Situacion varchar(1),  -- Situación del registro ('I' = Inactivo , 'A' = Activo)    
 @p_FechaModificacion numeric(8, 0), -- Fecha de modificacion del registro    
 @p_HoraModificacion varchar(10), -- Hora de modificacion del registro    
 @p_CodigoNegocio varchar(4),  -- Codigo de negocio del registro    
 @p_Host varchar(20),   -- IP de la PC local    
 @p_InterfazContable varchar(10),  
 @p_TipoCartera varchar(100),  
 @p_CodContabilidadFondo varchar(10),  
 @p_PorcentajeComision numeric(22,7),  
 @p_CodigoFondosMutuos varchar(10),  
 @p_ValorInicialFondo numeric(22,7),  
 @p_IndicadorFondo varchar(100),  
 @p_NumeroCuotaPreCierre numeric(22,7),  
 @p_PorSerie varchar(10),  
 @p_NombreCompleto VARCHAR(100),  
 @FlagComisionVariable BIT,  
 @MontoSuscripcionInicial NUMERIC(22,7),  
 @TopeValorCuota NUMERIC(22,7),  
 @FlagComisionSuscripInicial BIT,  
 @BDConta VarChar(12),  
 @CodigoPortafolioSisOpe VarChar(20),  
 @p_RUC VarChar(11),  
 @p_CodigoRenta varchar(3), -- OT 9362  
 @p_TipoCalculoValorCuota varchar(2),  
 @p_ValoracionMensual VARCHAR(1),   
 @p_CuotasLiberadas VARCHAR(1), -- OT10916  
 @p_CPPadreSisOpe VARCHAR(10),  
 @p_VectorPrecioVal VARCHAR(4),  
 @p_TipoNegocio VARCHAR(10), -- Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio  
 @p_FondoCliente VARCHAR(1),  
 @p_CodigoTerceroCliente VARCHAR(12),  
 @p_FlagAumentoCapital BIT,
 @p_TipoComision VARCHAR(10) 
)  
AS    
BEGIN  
 SET NOCOUNT ON    
 UPDATE Portafolio SET Descripcion = @p_Descripcion,FechaConstitucion = @p_FechaConstitucion,FechaAperturaContable =@p_FechaAperturaContable,  
  FechaAperturaContableAnterior = case when @p_FechaAperturaContable<>FechaAperturaContable then FechaAperturaContable else  FechaAperturaContableAnterior end,  
  CodigoTipoValorizacion = @p_CodigoTipoValorizacion,CodigoNegocio = @p_CodigoNegocio,FechaTermino =@p_FechaTermino,CodigoMoneda=@p_CodigoMoneda,    
  Situacion = @p_Situacion,UsuarioModificacion = @p_UsuarioModificacion,FechaModificacion = @p_FechaModificacion,HoraModificacion = @p_HoraModificacion,    
  Host = @p_Host,InterfazContable = @p_InterfazContable,TipoCartera = @p_TipoCartera,CodContabilidadFondo = @p_CodContabilidadFondo,  
  PorcentajeComision = @p_PorcentajeComision,CodigoFondosMutuos = @p_CodigoFondosMutuos,ValorInicialFondo = @p_ValorInicialFondo,IndicadorFondo = @p_IndicadorFondo,  
  NumeroCuotaPreCierre = @p_NumeroCuotaPreCierre,PorSerie = @p_PorSerie  ,NombreCompleto = @p_NombreCompleto,FlagComisionVariable=@FlagComisionVariable,  
  MontoSuscripcionInicial=@MontoSuscripcionInicial,TopeValorCuota=@TopeValorCuota,FlagComisionSuscripInicial=@FlagComisionSuscripInicial,  
  BDConta = @BDConta, CodigoPortafolioSisOpe = @CodigoPortafolioSisOpe, RUCPortafolio = @p_RUC, TipoCalculoValorCuota = @p_TipoCalculoValorCuota,  
  IdTipoRenta_FK = @p_CodigoRenta, ValoracionMensual = @p_ValoracionMensual -- OT 9362  
  ,CuotasLiberadas = @p_CuotasLiberadas /*OT10916*/, CPPadreSisOpe = @p_CPPadreSisOpe, VectorPrecioVal = @p_VectorPrecioVal  
  ,TipoNegocio = @p_TipoNegocio,FondoCliente = @p_FondoCliente, CodigoTerceroCliente = @p_CodigoTerceroCliente  
  ,FlagAumentoCapital = @p_FlagAumentoCapital,TipoComision = @p_TipoComision                
 WHERE  
  CodigoPortafolioSBS = @p_CodigoPortafolioSBS  
END  
GO

GRANT EXECUTE ON [dbo].[Portafolio_Modificar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - Procedure "Portafolio_Modificar"'
GO

PRINT 'Inicio Procedure "Portafolio_Insertar"'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Portafolio_Insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Portafolio_Insertar]
GO
---------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Insertar un portafolio  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 23/09/2016  
-- Modificado por: Marlon E. Peña  
-- Nro. Orden de Trabajo: 9362  
-- Descripción del cambio: Se agrega el parametro CodigoRenta, para insertarlo en la tabla Portafolio.  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Creación: 19/07/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10442  
-- Descripción: Incluir el ingreso del campo Fecha Caja de Operaciones y valorización mensual  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 06/11/2017  
-- Modificado por: Hanz Cocchi G.  
-- Nro. Orden de Trabajo: 10916  
-- Descripcion del cambio: Agregar el parámetro "@p_CuotasLiberadas" y el campo "CuotasLiberadas"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 22/02/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11237  
-- Descripcion del cambio: Agregar el parámetro "@p_CPPadreSisOpe" y el campo "CPPadreSisOpe"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 22/02/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11432  
-- Descripcion del cambio: Agregar el parámetro "@p_VectorPrecioVal" y el campo "VectorPrecioVal"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 2018-08-23  
-- Modificado por: CRumiche  
-- Nro. Orden de Trabajo: OT11590  
-- Descripcion del cambio: Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 17/09/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11568      
-- Descripcion del cambio: Agregar el parámetro "@p_FlagAumentoCapital" y el campo "FlagAumentoCapital"      
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha modificacion: 07/10/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Agregar el campo Fondo Cliente en el mantenimiento Administración de Portafolio. Es un cambio solicitado  
--                         para el desarrollo de Límites Fondo, dónde se identifica si un portafolio es un Cliente de una empresa o no.  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 23/05/2019  
-- Modificado por: Junior Huallullo P.
-- Nro. Orden de Trabajo: 12003
-- Descripcion del cambio: Agregar el campo TipoComision para que las comisiones aparezcan diarias o mensuales  
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[Portafolio_Insertar](  
 @p_CodigoPortafolioSBS VARCHAR(10), -- Código Portafolio SBS  
 @p_Descripcion VARCHAR(40),  -- Descripcion del registro  
 @p_FechaConstitucion NUMERIC(8,0), -- Fecha de constitucion del registro  
 @p_CodigoTipoValorizacion VARCHAR(3), -- Codigo tipo de valorizacion del registro  
 @p_FechaTermino  NUMERIC(8,0),  
 @p_CodigoMoneda VARCHAR(10),  
 @p_FechaAperturaContable NUMERIC(8,0),  
 @p_UsuarioCreacion VARCHAR(15), -- Usuario que creó el registro  
 @p_Situacion VARCHAR(1),  -- Situación del registro ('I' = Inactivo , 'A' = Activo)  
 @p_FechaCreacion NUMERIC(8, 0), -- Fecha de creación del registro  
 @p_HoraCreacion VARCHAR(10),  -- Hora de creación del registro  
 @p_CodigoNegocio VARCHAR(4),  -- Codigo de negocio del registro  
 @p_Host VARCHAR(20),   -- IP de la PC local  
 @p_EsMultiPortafolio VARCHAR(1), -- Grupo Portafolio  
 @p_INTerfazContable VARCHAR(10),  
 @p_TipoCartera VARCHAR(100),  
 @p_CodContabilidadFondo VARCHAR(10),  
 @p_PorcentajeComision NUMERIC(22,7),  
 @p_CodigoFondosMutuos VARCHAR(10),  
 @p_ValorInicialFondo NUMERIC(22,7),  
 @p_IndicadorFondo VARCHAR(100),  
 @p_NumeroCuotaPreCierre NUMERIC(22,7),  
 @p_PorSerie VARCHAR(10),  
 @p_NombreCompleto VARCHAR(100),  
 @FlagComisionVariable BIT,  
 @MontoSuscripcionInicial NUMERIC(22,7),  
 @TopeValorCuota NUMERIC(22,7),  
 @FlagComisionSuscripInicial BIT,  
 @BDConta VARCHAR(12),  
 @CodigoPortafolioSisOpe VARCHAR(20),  
 @p_RUC VARCHAR(11),  
 @p_CodigoRenta VARCHAR(3), -- OT 9362  
 @p_TipoCalculoValorCuota VARCHAR(2),  
 @p_FechaCajaOperaciones NUMERIC(8),  
 @p_ValoracionMensual VARCHAR(1),   
 @p_CuotasLiberadas VARCHAR(1),-- OT10916  
 @p_CPPadreSisOpe VARCHAR(10),  
 @p_VectorPrecioVal VARCHAR(4),  
    @p_FlagAumentoCapital BIT,            
 @p_TipoNegocio VARCHAR(10), -- Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio  
 @p_FondoCliente VARCHAR(1),  
 @p_CodigoTerceroCliente VARCHAR(12),
 @p_TipoComision VARCHAR(10) -- Proyecto SIT Fondos - Comision | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio  
)  
AS  
BEGIN  
 SET NOCOUNT ON  
 DECLARE @TranNombre VARCHAR(50)  
 SET @TranNombre = 'InsertaFondo'  
 BEGIN TRANSACTION @TranNombre  
 DECLARE @p_Correlativo NUMERIC(18,0), @CodigoMultifondo VARCHAR(12)  
 SET @p_Correlativo = (SELECT ISNULL(MAX(IndicePortafolio), 0) + 1 FROM Portafolio)  
 INSERT INTO [Portafolio]([CodigoPortafolioSBS],[Descripcion],[FechaConstitucion],[CodigoTipoValorizacion],FechaTermino,CodigoMoneda,  
 FechaAperturaContable,[UsuarioCreacion],[Situacion],[FechaCreacion],[HoraCreacion],[CodigoNegocio],[Host],IndicePortafolio,EsMultiPortafolio,  
 INTerfazContable,TipoCartera,CodContabilidadFondo,PorcentajeComision,CodigoFondosMutuos,ValorInicialFondo,IndicadorFondo,NumeroCuotaPreCierre,PorSerie,  
 FechaNegocio,FechaAperturaContableAnterior,NombreCompleto, FlagComisionVariable, MontoSuscripcionInicial,TopeValorCuota,FlagComisionSuscripInicial,  
 FechaCreacionFondo,BDConta,CodigoPortafolioSisOpe,RUCPortafolio, IdTipoRenta_FK, TipoCalculoValorCuota, FechaCajaOperaciones,  
 ValoracionMensual, -- OT9362  
 CuotasLiberadas,CPPadreSisOpe,VectorPrecioVal, TipoNegocio,FondoCliente,CodigoTerceroCliente,FlagAumentoCapital,TipoComision) -- OT10916  
            
 VALUES(@p_CodigoPortafolioSBS,@p_Descripcion,@p_FechaConstitucion,@p_CodigoTipoValorizacion,@p_FechaTermino ,@p_CodigoMoneda,  
 @p_FechaAperturaContable,@p_UsuarioCreacion,@p_Situacion,@p_FechaCreacion,@p_HoraCreacion,@p_CodigoNegocio,@p_Host,@p_Correlativo,@p_EsMultiPortafolio,  
 @p_INTerfazContable,@p_TipoCartera,@p_CodContabilidadFondo,@p_PorcentajeComision,@p_CodigoFondosMutuos,@p_ValorInicialFondo,@p_IndicadorFondo,@p_NumeroCuotaPreCierre,  
 @p_PorSerie,@p_FechaConstitucion,@p_FechaAperturaContable,@p_NombreCompleto,@FlagComisionVariable,@MontoSuscripcionInicial,  
 @TopeValorCuota,@FlagComisionSuscripInicial,@p_FechaConstitucion,@BDConta,@CodigoPortafolioSisOpe,@p_RUC, @p_CodigoRenta,@p_TipoCalculoValorCuota, @p_FechaCajaOperaciones,  
 @p_ValoracionMensual, -- OT 9362  
 @p_CuotasLiberadas /*OT109116*/,@p_CPPadreSisOpe,@p_VectorPrecioVal, @p_TipoNegocio,@p_FondoCliente,@p_CodigoTerceroCliente,@p_FlagAumentoCapital,@p_TipoComision)   
   
     
 SELECT @CodigoMultifondo = VALOR FROM PARAMETROSGENERALES WHERE CLASIFICACION = 'GrupoFondo'  
 IF @p_EsMultiPortafolio = 'N'  
 BEGIN  
  DECLARE @FechaSaldo NUMERIC(8),@Fondobase VARCHAR(20)  
  SELECT @Fondobase = VALOR FROM ParametrosGenerales WHERE  clasificacion = 'FONDOBASE' AND NOMBRE = @p_CodigoNegocio  
  SELECT @FechaSaldo = MAX(fechasaldo) FROM SaldosCarteraTitulo  
  WHERE CodigoPortafolioSBS = @Fondobase AND fechasaldo <= @p_FechaConstitucion  
  INSERT INTO SaldosCarteraTitulo(CodigoPortafolioSBS, NumeroTitulo, CodigoMnemonico,FechaSaldo, SaldoDisponible, SaldoProcesoCompra,SaldoProcesoVenta,  
  SaldoUnidadesLiberadas, SaldoUnidEntregGarantia,SaldoUnidadesBloqueadas, Redencion, UsuarioCreacion,FechaCreacion, HoraCreacion, Host)  
  SELECT @p_CodigoPortafolioSBS,NumeroTitulo,CodigoMnemonico,@p_FechaConstitucion,SaldoDisponible = 0,aldoProcesoCompra = 0,SaldoProcesoVenta = 0,  
  SaldoUnidadesLiberadas = 0,SaldoUnidEntregGarantia = 0,SaldoUnidadesBloqueadas = 0,Redencion = 0,@p_UsuarioCreacion, @p_FechaCreacion,@p_HoraCreacion,@p_Host  
  FROM SaldosCarteraTitulo    
  WHERE CodigoPortafolioSBS = @Fondobase and FechaSaldo = @FechaSaldo     
  INSERT INTO CustodioValores  
  SELECT CodigoNemonico,@p_CodigoPortafolioSBS,CodigoCustodio,CuentaDepositaria,Situacion,@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,  
  null, null, null,@p_Host FROM CustodioValores  
  WHERE CodigoPortafolioSBS = @Fondobase and Situacion = 'A'  
  SELECT @FechaSaldo = MAX(FechaSaldo) FROM CustodioSaldo    
  WHERE fechasaldo < @p_FechaConstitucion and CodigoPortafolioSBS = @Fondobase  
  CREATE TABLE #tmpCustodio(Secuencia INT IDENTITY (1, 1),CodigoCustodio VARCHAR (12),CodigoMnemonico VARCHAR (15))  
  INSERT INTO #tmpCustodio      
  SELECT CodigoCustodio, CodigoMnemonico FROM CustodioSaldo  
  WHERE FechaSaldo = @FechaSaldo and CodigoPortafolioSBS = @Fondobase  
  DECLARE @IdSecuencial INT  
  set @IdSecuencial = ISNULL((SELECT MAX(Secuencia) FROM CustodioSaldo), 0)  
  INSERT INTO CustodioSaldo(Secuencia, CodigoPortafolioSBS, CodigoCustodio,CuentaDepositaria, CodigoMnemonico, FechaSaldo,NumeroLamina,HoraSaldo,  
  NumeroTitulo, OrdenGenera,UnidadesBloqueadas, Redencion, UnidadesGarantia,SaldoInicialUnidades, EgresoUnidades, IngresoUnidades,UsuarioCreacion,  
  FechaCreacion, HoraCreacion, Host)  
  SELECT Secuencia+@IdSecuencial, @p_CodigoPortafolioSBS, CodigoCustodio,'', CodigoMnemonico, @p_FechaConstitucion,0, @p_HoraCreacion, 0, '',0, 0,  
  0,0, 0, 0,@p_UsuarioCreacion, @p_FechaCreacion, @p_HoraCreacion, @p_Host  
  FROM #tmpCustodio  
 END  
 COMMIT TRANSACTION @TranNombre  
 IF @@ERROR <> 0  
 BEGIN  
  PRINT 'Error al insertar portafolio'  
  ROLLBACK TRANSACTION @TranNombre  
  RETURN 99  
 END  
END  
GO

GRANT EXECUTE ON [dbo].[Portafolio_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - Procedure "Portafolio_Insertar"'
GO



PRINT 'Inicio Procedure "Portafolio_Seleccionar"'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Portafolio_Seleccionar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Portafolio_Seleccionar]
GO
-------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Seleccionar un portafolio  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 23/09/2016  
-- Modificado por: Marlon E. Peña  
-- Nro. Orden de Trabajo: 9362  
-- Descripción del cambio: Agregar el campo IdTipoRenta_FK al resultado del select.  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Creación: 24/07/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10442  
-- Descripción: Obtener un portafolio. Se agregó la columna TipoCalculoValorCuota y ValoracionMensual  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 06/11/2017  
-- Modificado por: Hanz Cocchi G.  
-- Nro. Orden de Trabajo: 10916  
-- Descripcion del cambio: Agregar el campo "CuotasLiberadas"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 15/02/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11157  
-- Descripcion del cambio: Agregar el campo "CPPadreSisOpe"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 15/02/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11432  
-- Descripcion del cambio: Agregar el campo "VectorPrecioVal"  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 15/02/2018      
-- Modificado por: Ricardo Colonia      
-- Nro. Orden de Trabajo: 11568      
-- Descripcion del cambio: Agregar el campo "FlagAumentoCapital"      
----------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha de Creación  : 2018-08-23  
-- Modificado por   : Carlos Rumiche  
-- Nro. Orden de Trabajo : OT11590  
-- Descripción    : Elimina secuencia Vector  
--        Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 07/10/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Agregar el campo Fondo Cliente en el mantenimiento Administración de Portafolio. Es un cambio solicitado  
--                         para el desarrollo de Límites Fondo, dónde se identifica si un portafolio es un Cliente de una empresa o no.  
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 24/10/2018  
-- Modificado por: CRumiche  
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Agregar el campo Cliente  
----------------------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[Portafolio_Seleccionar](  
 @p_CodigoPortafolio VARCHAR(10) = ''  
)AS  
BEGIN  
 SET NOCOUNT ON  
 DECLARE @FechaValoracion NUMERIC(8)  
   
 SELECT @FechaValoracion = MAX(FechaValoracion) FROM CarteraTituloValoracion  WHERE CodigoPortafolioSBS = @p_CodigoPortafolio  
   
 SELECT PO.[CodigoPortafolioSBS],PO.[Descripcion],PO.[FechaConstitucion],PO.[CodigoTipoValorizacion],TV.Descripcion AS 'NombreTipoValorizacion',   
   PO.[UsuarioCreacion],PO.[Situacion],PG.Nombre AS 'NombreSituacion',PO.[FechaCreacion],PO.[HoraCreacion],PO.[UsuarioModificacion],PO.[FechaModificacion],   
   PO.[HoraModificacion],ISNULL(PO.FechaTermino,0) 'FechaTermino',PO.CodigoMoneda,PO.[CodigoNegocio],NE.Descripcion AS 'NombreNegocio',   
   ISNULL(PO.InterfazContable, 'N') InterfazContable,ISNULL(PO.TipoCartera, 0) TipoCartera,ISNULL(PO.CodContabilidadFondo, '') CodContabilidadFondo,   
   ISNULL(PO.PorcentajeComision, 0.00) PorcentajeComision,ISNULL(PO.CodigoFondosMutuos, '') CodigoFondosMutuos,ISNULL(PO.ValorInicialFondo, 0.00) ValorInicialFondo,   
   ISNULL(PO.NumeroCuotaPreCierre, 0.00) NumeroCuotaPreCierre,ISNULL(PO.IndicadorFondo,0) IndicadorFondo,PO.[Host],PO.IndicePortafolio  ,PO.FechaAperturaContable,   
   PO.EsMultiPortafolio,ISNULL(PO.PorSerie, 'N') PorSerie,NombreCompleto = ISNULL(PO.NombreCompleto,''),FlagComisionVariable= ISNULL(FlagComisionVariable,0),   
   MontoSuscripcionInicial=ISNULL(MontoSuscripcionInicial,0),TopeValorCuota=ISNULL(TopeValorCuota,0),FlagComisionSuscripInicial = ISNULL(FlagComisionSuscripInicial, 0),   
   ISNULL(@FechaValoracion,0) as FechaValoracion, ISNULL(PO.BDConta,'') BDConta,ISNULL(CodigoPortafolioSisOpe,'') CodigoPortafolioSisOpe, ISNULL(RUCPortafolio,'') RUCPortafolio,   
   ISNULL(IdTipoRenta_FK,'') as CodigoRenta, -- OT 9362  
   ISNULL(TipoCalculoValorCuota,'') AS TipoCalculoValorCuota, ISNULL(ValoracionMensual,'') AS ValoracionMensual   
   ,ISNULL(CuotasLiberadas,'0') AS CuotasLiberadas -- OT10916  
   ,ISNULL(PO.CPPadreSisOpe,'') AS CPPadreSisOpe  
  
   -- INICIO | CRumiche | Proy - Fondos II | 2018-06-22 | Campo Agregado  
   ,ISNULL(PO.FechaCajaOperaciones,0) AS FechaCajaOperaciones   
   -- FIN | CRumiche | Proy - Fondos II | 2018-06-22 | Campo Agregado  
   ,ISNULL(PO.VectorPrecioVal,'') AS VectorPrecioVal,isnull(TipoNegocio,'') AS TipoNegocio,ISNULL(FondoCliente,'') AS FondoCliente  
     
   -- INICIO | CRumiche | Proy - Fondos II Limites | 2018-10-24  
   ,ISNULL(PO.CodigoTerceroCliente,'') AS CodigoTerceroCliente  
   ,ISNULL(Ter.Descripcion,'') AS DescTerceroCliente  
   -- FIN | CRumiche | Proy - Fondos II Limites | 2018-10-24   
   ,ISNULL(PO.FlagAumentoCapital,0) AS FlagAumentoCapital
   ,ISNULL(TipoComision,'') AS TipoComision    
 FROM [Portafolio] AS PO   
  LEFT JOIN TipoValorizacion AS TV ON PO.CodigoTipovalorizacion = TV.CodigoTipoValorizacion   
  INNER JOIN Negocio AS NE ON PO.CodigoNegocio = NE.CodigoNegocio   
  INNER JOIN ParametrosGenerales AS PG ON PO.Situacion = PG.Valor  
  LEFT JOIN Terceros AS Ter ON Ter.CodigoTercero = PO.CodigoTerceroCliente   
 WHERE   PO.CodigoPortafolioSBS = @p_CodigoPortafolio AND PG.Clasificacion = 'Situación'  
END  
GO

GRANT EXECUTE ON [dbo].[Portafolio_Seleccionar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - Procedure "Portafolio_Seleccionar"'
GO

PRINT 'Inicio - Procedure "sp_SIT_ins_ValorCuota"'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ins_ValorCuota]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_ins_ValorCuota]
GO
------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Insertar los datos en valor cuota  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 15/12/2016  
-- Modificado por: Everth Martinez  
-- Nro. Orden de Trabajo: 9679  
-- Descripcion del cambio: Se agregan un execute para ejecutar el procedimiento donde calculara la variacion porcentual del registro  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 18/01/2017  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9851  
-- Descripcion del cambio: Se agregan las columnas de auditoria  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 15/02/2017  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9981  
-- Descripcion del cambio: Se modifica el campo ComisionSAFMAnterior  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 06/11/2017  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 10916  
-- Descripcion del cambio: Agregar el parámetro "@p_CXCVentaTituloDividendos" y el campo "CXCVentaTituloDividendos"  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 08/11/2017  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 10883  
-- Descripcion del cambio: Agregar los campos AportesLiberadas y RetencionPendiente  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 16/07/2018  
-- Modificado por: Ricardo Colonia  
-- Proyecto: Fondos-II  
-- Nro. Orden de Trabajo: 11473  
-- Descripcion del cambio: Agregar Campo AjustesCXC   
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 28/12/2018  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11717  
-- Descripcion del cambio: Agregar el campo DevolucionComisionUnificada  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 07/01/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11687  
-- Descripcion del cambio: Agregar el campo OtrasCxCPreCierre, ValSwaps  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 24/05/2019
-- Modificado por: Junior Huallullo P.
-- Nro. Orden de Trabajo: 12003  
-- Descripcion del cambio: Agregar el campo AporteMandato, Retiro Mandato 
-----------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[sp_SIT_ins_ValorCuota]   
 @CodigoPortafolioSBS varchar(10),  
 @CodigoSerie varchar(100),  
 @FechaProceso NUMERIC(8,0),  
 @InversionesT1 NUMERIC(22,7),  
 @VentasVencimientos NUMERIC(22,7),  
 @Rentabilidad NUMERIC(22,7),  
 @ValForwards NUMERIC(22,7),  
 @CajaPreCierre NUMERIC(22,7),  
 @CXCVentaTitulo NUMERIC(22,7),  
 @OtrasCXC NUMERIC(22,7),  
 @OtrasCXCExclusivos NUMERIC(22,7),  
 @CXCPreCierre NUMERIC(22,7),  
 @CXPCompraTitulo NUMERIC(22,7),  
 @OtrasCXP NUMERIC(22,7),  
 @OtrasCXPExclusivos NUMERIC(22,7),  
 @CXPPreCierre NUMERIC(22,7),  
 @OtrosGastos NUMERIC(22,7),  
 @OtrosGastosExclusivos NUMERIC(22,7),  
 @OtrosIngresos NUMERIC(22,7),  
 @OtrosIngresosExclusivos NUMERIC(22,7),  
 @ValPatriPreCierre1 NUMERIC(22,7),  
 @ComisionSAFM NUMERIC(22,7),  
 @ValPatriPreCierre2 NUMERIC(22,7),  
 @ValCuotaPreCierre NUMERIC(22,7),  
 @ValCuotaPreCierreVal NUMERIC(22,7),  
 @AportesCuotas NUMERIC(22,7),  
 @AportesValores NUMERIC(22,7),  
 @RescateCuotas NUMERIC(22,7),  
 @RescateValores NUMERIC(22,7),  
 @Caja NUMERIC(22,7),  
 @CXCVentaTituloCierre NUMERIC(22,7),  
 @OtrosCXCCierre NUMERIC(22,7),  
 @OtrosCXCExclusivoCierre NUMERIC(22,7),  
 @CXCCierre NUMERIC(22,7),  
 @CXPCompraTituloCierre NUMERIC(22,7),  
 @OtrasCXPCierre NUMERIC(22,7),  
 @OtrasCXPExclusivoCierre NUMERIC(22,7),  
 @CXPCierre NUMERIC(22,7),  
 @OtrosGastosCierre NUMERIC(22,7),  
 @OtrosGastosExclusivosCierre NUMERIC(22,7),  
 @OtrosIngresosCierre NUMERIC(22,7),  
 @OtrosIngresosExclusivosCierre NUMERIC(22,7),  
 @ValPatriCierreCuota NUMERIC(22,7),  
 @ValPatriCierreValores NUMERIC(22,7),  
 @ValCuotaCierre NUMERIC(22,7),  
 @ValCuotaValoresCierre NUMERIC(22,7),  
 @InversionesSubTotal NUMERIC(22,7),  
 @InversionesSubTotalSerie NUMERIC(22,7),  
 @ValCuotaPreCierreAnt NUMERIC(22,7),  
 @p_Usuario VarChar(100),  
 @p_Fecha NUMERIC(8),  
 @p_Hora char(8),  
 @p_ChequePendiente NUMERIC(22,7),  
 @p_RescatePendiente NUMERIC(22,7),  
 @p_ComisionSAFMAnterior NUMERIC(8),   
 @p_AjustesCXP NUMERIC(22,7) /*9851, 9981*/,   
 @p_CXCVentaTituloDividendos NUMERIC(22,7), --OT10916  
 @p_AportesLiberadas NUMERIC(22,7),   
 @p_RetencionPendiente NUMERIC(22,7)  
 ,@p_AjustesCXC NUMERIC(22,7) -- RCE | Nuevo Campo AjustesCXC | 16/07/2018  
 ,@p_DevolucionComisionUnificada NUMERIC(22,7) = 0  
 ,@p_OtrasCxCPrecierre NUMERIC(22,7)  
 ,@p_ValSwaps NUMERIC(22,7)  
 ,@p_ComisionUnificadaCuota NUMERIC(22,7)
 ,@p_ComisionUnificadaMandato NUMERIC(22,7)
AS  
BEGIN  
 DECLARE @ValorCuoTaCierre NUMERIC(22,7),@ValorPatriCierreCuota NUMERIC(22,7)    
 IF EXISTS(SELECT 1 FROM ValorCuota WHERE CodigoPortafolioSBS = @CodigoPortafolioSBS AND FechaProceso = @FechaProceso AND CodigoSerie = @CodigoSerie )  BEGIN  
  UPDATE   
    ValorCuota   
  SET     
    InversionesT1 = @InversionesT1 ,  
    VentasVencimientos = @VentasVencimientos ,  
    Rentabilidad = @Rentabilidad ,  
    ValForwards = @ValForwards ,  
    CajaPreCierre = @CajaPreCierre ,   
    CXCVentaTitulo = @CXCVentaTitulo ,  
    OtrasCXC = @OtrasCXC ,  
    OtrasCXCExclusivos = @OtrasCXCExclusivos ,  
    CXCPreCierre = @CXCPreCierre ,  
    CXPCompraTitulo = @CXPCompraTitulo ,  
    OtrasCXP = @OtrasCXP ,  
    OtrasCXPExclusivos = @OtrasCXPExclusivos ,  
    CXPPreCierre = @CXPPreCierre ,  
    OtrosGastos = @OtrosGastos ,   
    OtrosGastosExclusivos = @OtrosGastosExclusivos ,  
    OtrosIngresos = @OtrosIngresos ,  
    OtrosIngresosExclusivos = @OtrosIngresosExclusivos ,   
    ValPatriPreCierre1 = @ValPatriPreCierre1 ,  
    ComisionSAFM = @ComisionSAFM ,  
    ValPatriPreCierre2 = @ValPatriPreCierre2 ,  
    ValCuotaPreCierre = @ValCuotaPreCierre ,   
    ValCuotaPreCierreVal = @ValCuotaPreCierreVal ,  
    AportesCuotas = @AportesCuotas ,  
    AportesValores = @AportesValores ,  
    RescateCuotas = @RescateCuotas ,    
    RescateValores = @RescateValores ,  
    Caja = @Caja ,  
    CXCVentaTituloCierre = @CXCVentaTituloCierre ,  
    OtrosCXCCierre = @OtrosCXCCierre ,    
    OtrosCXCExclusivoCierre = @OtrosCXCExclusivoCierre ,  
    CXCCierre = @CXCCierre ,  
    CXPCompraTituloCierre = @CXPCompraTituloCierre ,  
    OtrasCXPCierre = @OtrasCXPCierre ,    
    @OtrasCXPExclusivoCierre = OtrasCXPExclusivoCierre ,  
    CXPCierre = @CXPCierre ,  
    OtrosGastosCierre = @OtrosGastosCierre ,    
    OtrosGastosExclusivosCierre = @OtrosGastosExclusivosCierre ,  
    OtrosIngresosCierre = @OtrosIngresosCierre ,    
    OtrosIngresosExclusivosCierre = @OtrosIngresosExclusivosCierre ,  
    ValPatriCierreCuota = @ValPatriCierreCuota ,  
    ValPatriCierreValores = @ValPatriCierreValores ,  
    ValCuotaCierre = @ValCuotaCierre ,  
    ValCuotaValoresCierre = @ValCuotaValoresCierre ,  
    InversionesSubTotal = @InversionesSubTotal ,    
    InversionesSubTotalSerie = @InversionesSubTotalSerie,  
    ValCuotaPreCierreAnt = @ValCuotaPreCierreAnt,  
    UsuarioModificacion = @p_Usuario,  
    FechaModificacion = @p_Fecha,  
    HoraModificacion = @p_Hora,  
    ChequePendiente = @p_ChequePendiente,  
    RescatePendiente = @p_RescatePendiente,   
    ComisionSAFMAnterior = @p_ComisionSAFMAnterior ,   
    AjustesCXP = @p_AjustesCXP, --9851 -- 9981  
    CXCVentaTituloDividendos = @p_CXCVentaTituloDividendos,   
    AportesLiberadas = @p_AportesLiberadas,   
    RetencionPendiente = @p_RetencionPendiente,  
    AjustesCXC = @p_AjustesCXC,  -- RCE | Nuevo Campo AjustesCXC | 16/07/2018  
    DevolucionComisionUnificada = ISNULL(@p_DevolucionComisionUnificada,0),  
    OtrasCxCPreCierre = @p_OtrasCxCPrecierre,  
    ValSwaps = @p_ValSwaps,
	ComisionUnificadaCuota = @p_ComisionUnificadaCuota,
	ComisionUnificadaMandato = @p_ComisionUnificadaMandato
  WHERE   
    CodigoPortafolioSBS = @CodigoPortafolioSBS   
    AND CodigoSerie = @CodigoSerie   
    AND FechaProceso = @FechaProceso  
 END  
 ELSE BEGIN   
  INSERT INTO   
    ValorCuota (CodigoPortafolioSBS,  
       CodigoSerie,  
       FechaProceso,  
       InversionesT1 ,  
       VentasVencimientos ,  
       Rentabilidad ,  
       ValForwards ,  
       CajaPreCierre ,  
       CXCVentaTitulo ,   
       OtrasCXC ,  
       OtrasCXCExclusivos ,  
       CXCPreCierre ,  
       CXPCompraTitulo ,  
       OtrasCXP ,  
       OtrasCXPExclusivos ,  
       CXPPreCierre ,  
       OtrosGastos ,  
       OtrosGastosExclusivos ,  
       OtrosIngresos ,    
       OtrosIngresosExclusivos ,  
       ValPatriPreCierre1 ,  
       ComisionSAFM ,  
       ValPatriPreCierre2 ,  
       ValCuotaPreCierre ,  
       ValCuotaPreCierreVal ,  
       AportesCuotas ,  
       AportesValores ,  
       RescateCuotas ,    
       RescateValores ,  
       Caja ,  
       CXCVentaTituloCierre ,  
       OtrosCXCCierre ,  
       OtrosCXCExclusivoCierre ,  
       CXCCierre ,  
       CXPCompraTituloCierre ,  
       OtrasCXPCierre ,  
       OtrasCXPExclusivoCierre ,  
       CXPCierre ,  
       OtrosGastosCierre ,  
       OtrosGastosExclusivosCierre ,  
       OtrosIngresosCierre ,  
       OtrosIngresosExclusivosCierre ,  
       ValPatriCierreCuota ,  
       ValPatriCierreValores ,  
       ValCuotaCierre ,  
       ValCuotaValoresCierre ,  
       InversionesSubTotal ,  
       InversionesSubTotalSerie,  
       ValCuotaPreCierreAnt,  
       UsuarioCreacion,  
       FechaCreacion,  
       HoraCreacion,  
       UsuarioModificacion,  
       FechaModificacion,  
       HoraModificacion,  
       ChequePendiente,   
       RescatePendiente,   
       ComisionSAFMAnterior,  
       AjustesCXP,   
       CXCVentaTituloDividendos,  
       AportesLiberadas,  
       RetencionPendiente,  
       AjustesCXC, -- RCE | Nuevo Campo AjustesCXC | 16/07/2018  
       DevolucionComisionUnificada,  
       OtrasCxCPreCierre,  
       ValSwaps,
	   ComisionUnificadaCuota,
	   ComisionUnificadaMandato)   
  VALUES  
    (@CodigoPortafolioSBS,  
    @CodigoSerie,  
    @FechaProceso,  
    @InversionesT1 ,  
    @VentasVencimientos ,  
    @Rentabilidad ,  
    @ValForwards ,  
    @CajaPreCierre ,  
    @CXCVentaTitulo ,  
    @OtrasCXC ,  
    @OtrasCXCExclusivos ,  
    @CXCPreCierre ,  
    @CXPCompraTitulo ,  
    @OtrasCXP ,  
    @OtrasCXPExclusivos ,  
    @CXPPreCierre ,  
    @OtrosGastos ,  
    @OtrosGastosExclusivos ,  
    @OtrosIngresos ,  
    @OtrosIngresosExclusivos ,  
    @ValPatriPreCierre1 ,  
    @ComisionSAFM ,  
    @ValPatriPreCierre2 ,  
    @ValCuotaPreCierre ,  
    @ValCuotaPreCierreVal ,  
    @AportesCuotas ,  
    @AportesValores ,    
    @RescateCuotas ,  
    @RescateValores ,  
    @Caja ,  
    @CXCVentaTituloCierre ,  
    @OtrosCXCCierre ,  
    @OtrosCXCExclusivoCierre ,  
    @CXCCierre ,  
    @CXPCompraTituloCierre ,  
    @OtrasCXPCierre ,    
    @OtrasCXPExclusivoCierre ,  
    @CXPCierre ,  
    @OtrosGastosCierre ,  
    @OtrosGastosExclusivosCierre ,  
    @OtrosIngresosCierre ,  
    @OtrosIngresosExclusivosCierre ,  
    @ValPatriCierreCuota ,    
    @ValPatriCierreValores ,  
    @ValCuotaCierre ,  
    @ValCuotaValoresCierre ,  
    @InversionesSubTotal ,  
    @InversionesSubTotalSerie ,  
    @ValCuotaPreCierreAnt,  
    @p_Usuario ,  
    @p_Fecha,  
    @p_Hora ,  
    @p_Usuario ,  
    @p_Fecha,  
    @p_Hora,  
    @p_ChequePendiente,  
    @p_RescatePendiente,  
    @p_ComisionSAFMAnterior,  
    @p_AjustesCXP /*9851 - 9981*/,   
    @p_CXCVentaTituloDividendos, /*OT10916*/  
    @p_AportesLiberadas,   
    @p_RetencionPendiente,  
    @p_AjustesCXC, -- RCE | Nuevo Campo AjustesCXC | 16/07/2018  
    ISNULL(@p_DevolucionComisionUnificada,0),  
    @p_OtrasCxCPrecierre,  
    @p_ValSwaps,
	@p_ComisionUnificadaCuota,
	@p_ComisionUnificadaMandato)  
 END     
 --Actualiza el valor del ciere de los seriados    
 IF (SELECT PorSerie FROM Portafolio WHERE CodigoPortafolioSBS = @CodigoPortafolioSBS ) = 'S'    
 BEGIN  
  SELECT   
    @ValorCuoTaCierre = SUM(ValCuotaCierre) , @ValPatriCierreCuota = sum(ValPatriCierreCuota)  
  FROM   
    ValorCuota   
  WHERE   
    CodigoPortafolioSBS = @CodigoPortafolioSBS   
    AND FechaProceso = @FechaProceso    
    AND CodigoSerie <> ''    
    
  UPDATE   
    ValorCuota   
  SET  ValCuotaCierre = @ValorCuoTaCierre ,   
    ValPatriCierreCuota = @ValPatriCierreCuota  
  WHERE   
    CodigoPortafolioSBS = @CodigoPortafolioSBS   
    AND FechaProceso = @FechaProceso     
    AND CodigoSerie = ''    
 END    
   
 EXECUTE sp_SIT_Pro_ActualizarVariacion_ValorCuota @FechaProceso,@CodigoPortafolioSBS,@CodigoSerie  
END  
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ins_ValorCuota] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - Procedure "sp_SIT_ins_ValorCuota"'
GO


PRINT 'Inicio - Procedure "sp_SIT_sel_ValorCuota"'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_sel_ValorCuota]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_sel_ValorCuota]
GO

-----------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Seleccionar los datos en valor cuota  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 18/01/2016  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9851  
-- Descripcion del cambio: Se agregan los campos nuevos  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 15/02/2016  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9981  
-- Descripcion del cambio: Se agregan el campo AjustesCXP  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 06/11/2017  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 10916  
-- Descripcion del cambio: Agregar el campo AjustesCXP  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 08/11/2017  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 10883  
-- Descripcion del cambio: Agregar los campos AportesLiberadas y RetencionPendiente  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 16/07/2018  
-- Modificado por: Ricardo Colonia  
-- Proyecto: FONDOS-II - Sprint 3  
-- Nro. Orden de Trabajo: 11473  
-- Descripcion del cambio: Agregar el campo AjustesCXC  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 28/12/2018  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11717  
-- Descripcion del cambio: Agregar el campo DevolucionComisionUnificada  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 07/01/2018  
-- Modificado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11687  
-- Descripcion del cambio: Agregar el campo OtrasCxCPreCierre, ValSwaps  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 24/05/2019  
-- Modificado por: Junior Huallullo P.  
-- Nro. Orden de Trabajo: 12003
-- Descripcion del cambio: Agregar el campo AporteMandato, RetiroMandato
-----------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[sp_SIT_sel_ValorCuota](  
 @CodigoPortafolioSBS varchar(10),  
 @CodigoSerie varchar(100),  
 @FechaProceso numeric(8, 0))  
AS  
BEGIN  
 SELECT   
   CodigoPortafolioSBS,  
   CodigoSerie,  
   FechaProceso,  
   InversionesT1,  
   VentasVencimientos,  
   Rentabilidad,  
   ValForwards,  
   CajaPreCierre,  
   CXCVentaTitulo,  
   OtrasCXC,  
   OtrasCXCExclusivos,  
   CXCPreCierre,  
   CXPCompraTitulo,  
   OtrasCXP,  
   OtrasCXPExclusivos,  
   CXPPreCierre,  
   OtrosGastos,  
   OtrosGastosExclusivos,  
   OtrosIngresos,  
   OtrosIngresosExclusivos,  
   ValPatriPreCierre1,  
   ComisionSAFM,  
   ValPatriPreCierre2,  
   ValCuotaPreCierre,  
   ValCuotaPreCierreVal,  
   AportesCuotas,  
   AportesValores,  
   RescateCuotas,  
   RescateValores,  
   Caja,  
   CXCVentaTituloCierre,  
   OtrosCXCCierre,  
   OtrosCXCExclusivoCierre,  
   CXCCierre,  
   CXPCompraTituloCierre,  
   OtrasCXPCierre,  
   OtrasCXPExclusivoCierre,  
   CXPCierre,  
   OtrosGastosCierre,  
   OtrosGastosExclusivosCierre,  
   OtrosIngresosCierre,  
   OtrosIngresosExclusivosCierre,  
   ISNULL(ValPatriCierreCuota,0) ValPatriCierreCuota,  
   ValPatriCierreValores,  
   ISNULL(ValCuotaCierre,0) ValCuotaCierre,  
   ValCuotaValoresCierre,  
   InversionesSubTotal,  
   InversionesSubTotalSerie,  
   ValCuotaPreCierre AS CuotaPreCierreAnt,  
   ISNULL(ChequePendiente,0) ChequePendiente,  
   ISNULL(RescatePendiente,0) RescatePendiente,  
   ComisionSAFMAnterior,   
   AjustesCXP /*9981*/,   
   ISNULL(CXCVentaTituloDividendos,0) AS CXCVentaTituloDividendos,  
   ISNULL(AportesLiberadas,0) AS AportesLiberadas,   
   ISNULL(RetencionPendiente,0) AS RetencionPendiente,  
   ISNULL(AjustesCXC,0) AjustesCXC, -- RCE | Se agrega nuevo campo AjustesCXC   
   ISNULL(DevolucionComisionUnificada,0) DevolucionComisionUnificada,  
   ISNULL(OtrasCxCPreCierre,0) AS OtrasCxCPreCierre,  
   ISNULL(ValSwaps,0) AS ValSwaps,
   ISNULL(ComisionUnificadaCuota,0) AS ComisionUnificadaCuota,
   ISNULL(ComisionUnificadaMandato,0) AS ComisionUnificadaMandato
 FROM   
   ValorCuota   
 WHERE   
   CodigoPortafolioSBS = @CodigoPortafolioSBS   
   AND CodigoSerie = @CodigoSerie   
   AND FechaProceso = @FechaProceso  
END  
GO

GRANT EXECUTE ON [dbo].[sp_SIT_sel_ValorCuota] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - Procedure "sp_SIT_sel_ValorCuota"'
GO

PRINT 'Fin de Ejecucion - Procedures OT 12003'
GO

PRINT 'INICIO EJECUCIÓN - ReporteGestion_OperacionesVencimientosOTC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReporteGestion_OperacionesVencimientosOTC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReporteGestion_OperacionesVencimientosOTC]
GO


/*
exec ReporteGestion_OperacionesVencimientosOTC @p_CodigoPortafolio=N'2666,2777,',@p_FechaInicio=20181201,@p_FechaFin=20181231
*/
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: GENERAR REPORTE DE OPERACIONES, VENCIMIENTOS Y OTC
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 04/09/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11590
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 18/12/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Se incluye la constitución de depósitos a plazo y operaciones de reporte
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 18/12/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 12003
-- Descripcion del cambio: Se incluye para Bonos solo muestre MontoOperacion en operaciones de Compra y Venta.
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ReporteGestion_OperacionesVencimientosOTC]
(
	@p_CodigoPortafolio VARCHAR(100)
	,@p_FechaInicio NUMERIC(8)
	,@p_FechaFin NUMERIC(8)
)
AS
BEGIN
	--RENTA FIJA Y RENTA VARIABLE
	SELECT OI.CodigoPortafolioSBS, OI.CodigoOrden, OI.FechaOperacion, TI.CodigoTipoInstrumento AS TipoInstrumento, OI.CodigoMnemonico
		,OI.CodigoISIN
		,NroDeposito = 	(select Descripcion from Terceros 
						 where CodigoTercero in (select top 1 CodigoTercero from Entidad 
												where CodigoEntidad in (select top 1 CodigoEmisor from Valores 
																		where CodigoNemonico = OI.CodigoMnemonico)))
		, M.SinonimoISO AS CodigoMoneda, v.BaseInteresCorrido AS BaseCuponAnual, ISNULL(OI.YTM,0) AS YTM
		,OI.MontoNominalOrdenado
		--,((V.ValorUnitario * OI.CantidadOperacion) * dbo.CalcularSumaAmortizacionPendiente(OI.FechaOperacion,V.CodigoNemonico)/100) AS ValorNominalResidual
		,ISNULL(CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_SaldoNominalVigente
			WHEN 'A_VENCI' THEN VA.TIRCOM_SaldoNominalVigente
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_SaldoNominalVigente
		END,0) AS ValorNominalResidual
		,ISNULL(OI.CantidadOperacion,0) AS CantidadOperacion,(CASE WHEN OI.CategoriaInstrumento = 'BO' THEN OI.MontoOperacion 
																   ELSE ISNULL(OI.MontoNetoOperacion - OI.TotalComisiones,0) END) AS ValorPagado
		,ISNULL(OI.TotalComisiones,0) - dbo.RetornarMontoComisionPorTipo(OI.CodigoOrden,OI.CodigoPortafolioSBS,'IGV') AS TotalComisiones
		,dbo.RetornarMontoComisionPorTipo(OI.CodigoOrden,OI.CodigoPortafolioSBS,'IGV') AS IGV, OI.MontoNetoOperacion, OI.PrecioNegociacionSucio
		,O.Descripcion AS DescripcionOperacion, V.FechaVencimiento, P.Descripcion AS NombrePortafolio, '' AS Observacion, OI.FechaLiquidacion
	FROM OrdenInversion OI
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN Operacion O ON OI.CodigoOperacion = O.CodigoOperacion
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		LEFT JOIN ValorizacionAmortizada VA ON OI.CodigoOrden = VA.CodigoOrden AND OI.CodigoPortafolioSBS = VA.CodigoPortafolioSBS
			AND OI.FechaOperacion = VA.FechaProceso
		INNER JOIN Moneda M ON V.CodigoMoneda = M.CodigoMoneda
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND OI.FechaOperacion BETWEEN @p_FechaInicio AND @p_FechaFin
		AND O.CodigoOperacion IN ('1','2') AND OI.Estado IN ('E-EJE','E-CON')
	UNION
	
	/*DEPËSITOS A PLAZO*/
	SELECT
		OI.CodigoPortafolioSBS, OI.CodigoOrden, OI.FechaOperacion, TI.CodigoTipoInstrumento AS TipoInstrumento, OI.CodigoMnemonico
		,(CASE WHEN LEN(OI.CodigoISIN) >= 12 THEN OI.CodigoISIN ELSE '' END) AS CodigoISIN
		,NroDeposito = 
			CASE WHEN OI.CategoriaInstrumento in ('DP','FD') THEN (select top 1 Descripcion from Terceros where CodigoTercero = OI.CodigoTercero)
				ELSE	(select Descripcion from Terceros 
						 where CodigoTercero in (select top 1 CodigoTercero from Entidad 
												where CodigoEntidad in (select top 1 CodigoEmisor from Valores 
																		where CodigoNemonico = OI.CodigoMnemonico)))				
				END
		, M.SinonimoISO AS CodigoMoneda, v.BaseInteresCorrido AS BaseCuponAnual,OI.TasaPorcentaje
		,OI.MontoNominalOrdenado
		,OI.MontoNominalOrdenado AS ValorNominalResidual
		,ISNULL(OI.CantidadOperacion,0) AS CantidadOperacion, OI.MontoNominalOrdenado AS ValorPagado
		,ISNULL(OI.TotalComisiones,0) AS TotalComisiones
		,dbo.RetornarMontoComisionPorTipo(OI.CodigoOrden,OI.CodigoPortafolioSBS,'IGV') AS IGV, ISNULL(OI.MontoNominalOrdenado,0) AS MontoNetoOperacion
		,100 AS PrecioNegociacionSucio
		-- CRumiche: Cambio requerido por IAN (2019-02-08)
		,DescripcionOperacion = 
				case when TI.CodigoTipoInstrumento = 'DPZ' and O.CodigoOperacion = '3' /* CONSTITUCION TITULOS UNICOS */ then 'Compra'
					when TI.CodigoTipoInstrumento = 'FORW' and O.CodigoOperacion = '93' /* Constitucion Compra Forward */ then 'Compra'
					when TI.CodigoTipoInstrumento = 'DPZ' and O.CodigoOperacion = '4' /* CANCELACION TITULOS UNICOS */ then 'Venta'
					when TI.CodigoTipoInstrumento = 'FORW' and O.CodigoOperacion = '94' /* Constitucion Venta Forward */ then 'Venta'
					else O.Descripcion end
		-- ,O.Descripcion AS DescripcionOperacion
		, OI.FechaContrato AS FechaVencimiento, P.Descripcion AS NombrePortafolio, '' AS Observacion, OI.FechaLiquidacion
	FROM OrdenInversion OI
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN Operacion O ON OI.CodigoOperacion = O.CodigoOperacion
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero
		INNER JOIN Moneda M ON V.CodigoMoneda = M.CodigoMoneda
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND OI.FechaOperacion BETWEEN @p_FechaInicio AND @p_FechaFin
		--AND OI.CategoriaInstrumento IN ('DP','OR') AND OI.Estado IN ('E-EJE','E-CON')
		AND OI.CodigoOperacion IN ('3','101') AND OI.Estado IN ('E-EJE','E-CON')
	ORDER BY 3,1,5
END

GO

GRANT EXECUTE ON [dbo].[ReporteGestion_OperacionesVencimientosOTC] TO [rol_sit_fondos] AS [dbo]
GO



PRINT 'FIN EJECUCIÓN - ReporteGestion_OperacionesVencimientosOTC'
PRINT 'INICIO EJECUCIÓN - DiferenciasInstrumentosRecompile_Listar_ConDetalle'

USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'DiferenciasInstrumentosRecompile_Listar_ConDetalle')
	DROP PROCEDURE [dbo].[DiferenciasInstrumentosRecompile_Listar_ConDetalle]
GO
----------------------------------------------------------------------------------------------------------------------------
--Objetivo: Generar detalle en las diferencias de Tenencias
----------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 28/03/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10176
--	Descripcion del cambio: Se valida cuando el campo CantidadOrdenado sea nulo se tome el valor del campo CantidadOperacion
----------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 08/11/2017
-- Modificado por: Hanz Cocchi Guerrero
-- Nro. Orden de Trabajo: 10883
-- Descripcion del cambio: Se agrega condición para el custodio BBH y consultar datos de la tabla CustodioSaldoBBH
----------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 17/05/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 10959
-- Descripcion del cambio: Incluir columna de aportes liberadas en el reporte de custodia detallado
----------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 29/05/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 12003
-- Descripcion del cambio: Ampliar campo CodigoPortafolioSBS de la tabla temporal #InstrumentosNoRegistrados
----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[DiferenciasInstrumentosRecompile_Listar_ConDetalle]
(
	@fechaCorte NUMERIC(8,0),
	@CodigoPortafolioSBS VARCHAR(20),
	@CodigoCustodio VARCHAR(12)
)
AS
BEGIN
	
	DECLARE @FlagPortafolio BIT, @FlagCustodio BIT
	
	DECLARE @Name_Portafolio VARCHAR(80) = IsNull((SELECT Descripcion FROM Portafolio WHERE CodigoPortafolioSBS = @CodigoPortafolioSBS),'')  
	DECLARE @Name_Custodio  VARCHAR(80) = IsNull((SELECT Descripcion FROM Custodio WHERE CodigoCustodio = @CodigoCustodio),'')  
	CREATE TABLE #TABLA_CABECERA (CodigoISIN  VARCHAR(100),CodigoMnemonico VARCHAR(100),CodigoSBS  VARCHAR(100),Emisor   VARCHAR(400),  
	TipoTitulo  VARCHAR(100),Sinonimo  VARCHAR(100),NroTitulo  VARCHAR(100),Unidades  NUMERIC(22,7),UnidLiberadas  NUMERIC(22,7),VPNOrigen  NUMERIC(22,7),ValCustodio  NUMERIC(22,7),  
	DIFerencia  NUMERIC(22,7),Valor   NUMERIC(22,7),Portafolio  VARCHAR(100),CodCustodio  VARCHAR(100),Custodio  VARCHAR(400) )
	CREATE TABLE #TABLA_DETALLE ( IDREGISTRO INT IDENTITY(1,1), CodigoMnemonico VARCHAR(100), FechaOperacion VARCHAR(100), FechaLiquidacion VARCHAR(100),
	CodigoOrden VARCHAR(400), Operacion VARCHAR(100),CantidadOperacion NUMERIC(22,7),Contraparte VARCHAR(100),Estado VARCHAR(100),
	TotalVentas NUMERIC(22,7), TotalCompras NUMERIC(22,7), CodigoISIN VARCHAR(100))
	CREATE TABLE #InstrumentosNoRegistrados (Id INT IDENTITY PRIMARY KEY,CodigoMnemonico VARCHAR(15),CodigoISIN VARCHAR(12),DescripcionEmi	VARCHAR(100),
	SaldoDisponible NUMERIC(22,7),SaldoContable NUMERIC(22,7),CodigoCustodio VARCHAR(12),CodigoPortafolioSBS VARCHAR(80),CuentaDepositariaCustodio VARCHAR(10),
	CodigoSBS VARCHAR(12),CodigoTipoInstrumento VARCHAR(12)	)
	
	DECLARE @MIN INT = 1  
	DECLARE @MAX INT 
	
	DECLARE @NEMONICO VARCHAR(200)

	IF @CodigoCustodio = 'BBH'
		BEGIN  
			SET @FlagPortafolio = 0    
			SET @FlagCustodio = 0    
			IF @CodigoPortafolioSBS = '' SET @FlagPortafolio = 1    
			IF @CodigoCustodio = '' SET @FlagCustodio = 1    
			INSERT INTO #InstrumentosNoRegistrados 
			EXEC InstrumentosNoRegistradosRecompile_Listar @fechaCorte, @CodigoPortafolioSBS, @CodigoCustodio
			INSERT INTO #TABLA_CABECERA  
			SELECT df.CodigoISIN, df.CodigoMnemonico,  df.CodigoSBS, df.Emisor, ISNULL(DF.CodigoMnemonico, '') + ' ' + df.CodigoEntidad, df.CodigoTipoInstrumento,   
			df.NUMEROTITULO, df.Unidades, df.UnidLiberadas, df.VPNLocal, df.Saldo, (df.Unidades+df.UnidLiberadas)-df.Saldo DIFerencia, 0 Valor, @Name_Portafolio, @CodigoCustodio, @Name_Custodio     
			FROM (
				SELECT v.CodigoNemonico CodigoMnemonico,  v.CodigoISIN,  v.CodigoSBS,left(t.Descripcion,50) Emisor,  v.CodigoTipoTitulo,  ti.CodigoTipoInstrumento,
				0 Nro, isnull(sc.SaldoDisponible,0)+isnull(sc.SaldoProcesoCompra,0) - isnull(sc.SaldoProcesoVenta,0)-isnull(sc.Redencion,0) Unidades, 
				isnull(sc.SALDOUNIDADESLIBERADAS,0) as UnidLiberadas,
				0 Vpn, isnull(ci.SaldoContable,0) Saldo , e.CodigoEntidad, sc.NUMEROTITULO , ctv.VPNLocal
				FROM  Valores v
				INNER JOIN TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = v.CodigoTipoInstrumentoSBS
				INNER JOIN CustodioSaldoBBH cs ON v.CodigoNemonico=cs.CodigoMnemonico AND cs.CodigoPortafolioSBS = @CodigoPortafolioSBS   
				AND cs.CodigoCustodio = @CodigoCustodio AND cs.FechASaldo = @fechaCorte
				INNER JOIN Custodio c ON c.CodigoCustodio = cs.CodigoCustodio
				INNER JOIN Entidad e ON v.CodigoEmisor=e.CodigoEntidad
				INNER JOIN Terceros t ON e.CodigoTercero=t.CodigoTercero  
				LEFT JOIN CustodioInformacion ci ON c.CodigoCustodio = ci.CodigoCustodio AND cs.CodigoPortafolioSBS = ci.CodigoPortafolioSBS
				AND cs.FechASaldo = ci.FechaCorte AND v.CodigoIsin = ci.CodigoIsin
				INNER JOIN CustodioValores cv ON v.CodigoNemonico=cv.CodigoNemonico AND cv.CodigoPortafolioSBS = @CodigoPortafolioSBS
				AND cv.CodigoCustodio = @CodigoCustodio AND cv.Situacion = 'A' AND v.Situacion = 'A'
				LEFT JOIN SaldosCarteraTitulo sc on sc.CodigoMnemonico = v.CodigoNemonico and cs.CodigoPortafolioSBS = sc.CODIGOPORTAFOLIOSBS
				and cs.FechaSaldo = sc.FECHASALDO
				LEFT JOIN CarteraTituloValoracion ctv ON ctv.CodigoMnemonico = v.CodigoNemonico AND ctv.FechaValoracion = @fechaCorte
				AND (ctv.CodigoPortafolioSBS = @CodigoPortafolioSBS Or @FlagPortafolio = 1) AND Escenario = 'REAL'
				WHERE (case when isnull(v.FechaVencimiento,0) = 0 then @fechaCorte else v.FechaVencimiento end) >= @fechaCorte
				AND (cs.CodigoPortafolioSBS=@CodigoPortafolioSBS Or @FlagPortafolio = 1)
				AND (cs.CodigoCustodio=@CodigoCustodio Or @FlagCustodio = 1)
			) DF
			WHERE Unidades + UnidLiberadas <> Saldo
			UNION ALL
			SELECT VAL.CodigoISIN, VAL.CodigoNemonico 'CodigoMnemonico', VAL.CodigoSBS,left(TER.Descripcion,20) Emisor,
			ISNULL(VAL.CodigoNemonico ,'') + ' ' + ENT.CodigoEntidad, TIN.CodigoTipoInstrumento,0 NUMEROTITULO,
			0 Unidades,0 UnidLiberadas, ISNULL(CTV.VPNLocal,0)VPNLocal ,ISNULL(CUI.SaldoContable,0) Saldo,0-ISNULL(CUI.SaldoContable,0) DIFerencia,
			0 Valor, @Name_Portafolio, @CodigoCustodio, @Name_Custodio     
			FROM #InstrumentosNoRegistrados INR
			JOIN Valores VAL ON  INR.CodigoISIN = VAL.CodigoISIN
			LEFT JOIN Entidad ENT ON VAL.CodigoEmisor = ENT.CodigoEntidad
			LEFT JOIN Terceros TER ON ENT.CodigoTercero = TER.CodigoTercero
			JOIN TipoInstrumento TIN ON VAL.CodigoTipoInstrumentoSBS = TIN.CodigoTipoInstrumentoSBS
			LEFT JOIN CustodioInformacion CUI on VAL.CodigoISIN = CUI.CodigoISIN AND @CodigoCustodio = CUI.CodigoCustodio 
			AND @CodigoPortafolioSBS = CUI.CodigoPortafolioSBS AND CUI.FechaCorte = @fechaCorte 
			LEFT JOIN CarteraTituloValoracion CTV ON CTV.CodigoMnemonico = VAL.CodigoNemonico AND CTV.FechaValoracion = @fechaCorte AND (CTV.CodigoPortafolioSBS = @CodigoPortafolioSBS Or @FlagPortafolio = 1) AND Escenario = 'REAL'                                
			ORDER BY CodigoISIN
			
			/*LISTAMOS TODOS LOS CUSTODIOS DEL REPORTE*/
			SELECT DISTINCT Portafolio, CodCustodio, Custodio  FROM #TABLA_CABECERA
			
			/* LISTAMOS LAS CABECERAS */
			SELECT * FROM #TABLA_CABECERA
			ORDER BY CodigoISIN
			
			/* LISTAMOS LOS DETALLES */  
			INSERT INTO #TABLA_DETALLE
			SELECT CodigoMnemonico, dbo.fn_SIT_gl_ConvertirFechaaString(FechaOperacion) FechaOperacion,
			dbo.fn_SIT_gl_ConvertirFechaaString(FechaLiquidacion) FechaLiquidacion, CodigoOrden, UPPER(O.Descripcion) Operacion,ISNULL(OI.CantidadOrdenado,OI.CantidadOperacion), --10176
			T.Descripcion Contraparte, EO.Descripcionestado + '-Sin liquidar' Estado,0,0,CodigoISIN
			FROM OrdenInversion OI  
			INNER JOIN Operacion O ON OI.CodigoOperacion = O.CodigoOperacion  
			INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero  
			INNER JOIN EstadoOI EO ON EO.CodigoEstado = OI.Estado  
			WHERE CodigoMnemonico IN (SELECT CodigoMnemonico FROM #TABLA_CABECERA) AND OI.Situacion = 'A'  
			AND CodigoPortafolioSBS = @CodigoPortafolioSBS AND FechaOperacion <= @fechaCorte AND FechaLiquidacion > @fechaCorte
			AND OI.Estado <> 'E-ELI' AND OI.CodigoOperacion in ('1','2') -- Ipastor: Solo se consideran las operaciones de compra y venta
			
			--Liberadas
			INSERT INTO #TABLA_DETALLE
			SELECT DRL.CodigoNemonico,dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaIDI),dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaEntrega),DRL.Identificador,
			O.Descripcion,MontoNominalLocal = ISNULL(DRL.MontoNominalLocal, ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico, DRL.FechaCorte),0,1)),
			T.Descripcion ,'Confirmada-Sin liquidar',0,
			MontoNominalLocal = ISNULL(DRL.MontoNominalLocal, ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico, DRL.FechaCorte),0,1)),v.CodigoISIN
			FROM DividendosRebatesLiberadas DRL
			JOIN Operacion O ON O.CodigoOperacion = '37'
			JOIN Valores V ON V.CodigoNemonico = DRL.CodigoNemonico
			JOIN Entidad E ON E.CodigoEntidad = V.CodigoEmisor
			JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
			WHERE DRL.TipoDistribucion  = 'L' AND DRL.Situacion = 'A' AND DRL.CodigoPortafolioSBS = @CodigoPortafolioSBS
			AND DRL.FechaEntrega > @fechaCorte AND DRL.FechaIDI <= @fechaCorte
			--
			
			SELECT @MAX = MAX(IDREGISTRO) FROM #TABLA_DETALLE
			WHILE @MIN <= @MAX  
			BEGIN  
				SELECT @NEMONICO = CodigoMnemonico FROM #TABLA_DETALLE WHERE IDREGISTRO = @MIN
				UPDATE #TABLA_DETALLE  
				SET  TotalCompras = ISNULL((SELECT SUM(ISNULL(CantidadOperacion,0)) FROM #TABLA_DETALLE WHERE CodigoMnemonico = @NEMONICO AND Operacion = 'COMPRA'),0),  
				TotalVentas = ISNULL((SELECT SUM(ISNULL(CantidadOperacion,0)) FROM #TABLA_DETALLE WHERE CodigoMnemonico = @NEMONICO AND Operacion = 'VENTA'),0)  
				WHERE IDREGISTRO = @MIN  
				SET @MIN = @MIN + 1  
			END
			----Liberadas
			--INSERT INTO #TABLA_DETALLE
			--SELECT DRL.CodigoNemonico,dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaIDI),dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaEntrega),DRL.Identificador,
			--O.Descripcion,DRL.MontoNominalLocal, T.Descripcion ,'Confirmada-Sin liquidar',0,drl.MontoNominalLocal, v.CodigoISIN
			--FROM DividendosRebatesLiberadas DRL
			--JOIN Operacion O ON O.CodigoOperacion = '37'
			--JOIN Valores V ON V.CodigoNemonico = DRL.CodigoNemonico 
			--JOIN Entidad E ON E.CodigoEntidad = V.CodigoEmisor 
			--JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
			--WHERE DRL.TipoDistribucion  = 'L' AND DRL.Situacion = 'A' AND DRL.CodigoPortafolioSBS = @CodigoPortafolioSBS 
			--AND DRL.FechaEntrega > @fechaCorte AND DRL.FechaIDI <= @fechaCorte
			----
			SELECT CodigoMnemonico,FechaOperacion,FechaLiquidacion,CodigoOrden,Operacion,CantidadOperacion,Contraparte,  
			Estado,TotalVentas,TotalCompras,CodigoISIN
			FROM #TABLA_DETALLE  
			ORDER BY CodigoISIN
		END
	ELSE
		BEGIN
			SET @FlagPortafolio = 0
			SET @FlagCustodio = 0
			IF @CodigoPortafolioSBS = '' SET @FlagPortafolio = 1
			IF @CodigoCustodio = '' SET @FlagCustodio = 1
			INSERT INTO #InstrumentosNoRegistrados 
			EXEC InstrumentosNoRegistradosRecompile_Listar @fechaCorte, @CodigoPortafolioSBS, @CodigoCustodio
			INSERT INTO #TABLA_CABECERA  
			SELECT df.CodigoISIN, df.CodigoMnemonico,  df.CodigoSBS, df.Emisor, ISNULL(DF.CodigoMnemonico, '') + ' ' + df.CodigoEntidad, df.CodigoTipoInstrumento,   
			df.NUMEROTITULO, df.Unidades, df.UnidLiberadas, df.VPNLocal, df.Saldo, (df.Unidades+df.UnidLiberadas)-df.Saldo DIFerencia, 0 Valor, @Name_Portafolio, @CodigoCustodio, @Name_Custodio     
			FROM (
				SELECT v.CodigoNemonico CodigoMnemonico,  v.CodigoISIN,  v.CodigoSBS,left(t.Descripcion,50) Emisor,  v.CodigoTipoTitulo,  ti.CodigoTipoInstrumento,
				0 Nro, isnull(sc.SaldoDisponible,0)+isnull(sc.SaldoProcesoCompra,0) - isnull(sc.SaldoProcesoVenta,0)-isnull(sc.Redencion,0) Unidades,
				isnull(sc.SALDOUNIDADESLIBERADAS,0) as UnidLiberadas,
				0 Vpn, isnull(ci.SaldoContable,0) Saldo , e.CodigoEntidad, sc.NUMEROTITULO , ctv.VPNLocal
				FROM  Valores v
				INNER JOIN TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = v.CodigoTipoInstrumentoSBS
				INNER JOIN CustodioSaldo cs ON v.CodigoNemonico=cs.CodigoMnemonico AND cs.CodigoPortafolioSBS = @CodigoPortafolioSBS   
				AND cs.CodigoCustodio = @CodigoCustodio AND cs.FechASaldo = @fechaCorte
				INNER JOIN Custodio c ON c.CodigoCustodio = cs.CodigoCustodio
				INNER JOIN Entidad e ON v.CodigoEmisor=e.CodigoEntidad
				INNER JOIN Terceros t ON e.CodigoTercero=t.CodigoTercero  
				LEFT JOIN CustodioInformacion ci ON c.CodigoCustodio = ci.CodigoCustodio AND cs.CodigoPortafolioSBS = ci.CodigoPortafolioSBS
				AND cs.FechASaldo = ci.FechaCorte AND v.CodigoIsin = ci.CodigoIsin
				INNER JOIN CustodioValores cv ON v.CodigoNemonico=cv.CodigoNemonico AND cv.CodigoPortafolioSBS = @CodigoPortafolioSBS
				AND cv.CodigoCustodio = @CodigoCustodio AND cv.Situacion = 'A' AND v.Situacion = 'A'
				LEFT JOIN SaldosCarteraTitulo sc on sc.CodigoMnemonico = v.CodigoNemonico and cs.CodigoPortafolioSBS = sc.CODIGOPORTAFOLIOSBS
				and cs.FechaSaldo = sc.FECHASALDO
				LEFT JOIN CarteraTituloValoracion ctv ON ctv.CodigoMnemonico = v.CodigoNemonico AND ctv.FechaValoracion = @fechaCorte
				AND (ctv.CodigoPortafolioSBS = @CodigoPortafolioSBS Or @FlagPortafolio = 1) AND Escenario = 'REAL'
				WHERE (case when isnull(v.FechaVencimiento,0) = 0 then @fechaCorte else v.FechaVencimiento end) >= @fechaCorte
				AND (cs.CodigoPortafolioSBS=@CodigoPortafolioSBS Or @FlagPortafolio = 1)
				AND (cs.CodigoCustodio=@CodigoCustodio Or @FlagCustodio = 1)
			) DF
			WHERE Unidades + UnidLiberadas <> Saldo
			UNION ALL
			SELECT VAL.CodigoISIN, VAL.CodigoNemonico 'CodigoMnemonico', VAL.CodigoSBS,left(TER.Descripcion,20) Emisor,
			ISNULL(VAL.CodigoNemonico ,'') + ' ' + ENT.CodigoEntidad, TIN.CodigoTipoInstrumento,0 NUMEROTITULO,
			0 Unidades,0 UnidLiberadas, ISNULL(CTV.VPNLocal,0)VPNLocal ,ISNULL(CUI.SaldoContable,0) Saldo,0-ISNULL(CUI.SaldoContable,0) DIFerencia,
			0 Valor, @Name_Portafolio, @CodigoCustodio, @Name_Custodio     
			FROM #InstrumentosNoRegistrados INR
			JOIN Valores VAL ON  INR.CodigoISIN = VAL.CodigoISIN
			LEFT JOIN Entidad ENT ON VAL.CodigoEmisor = ENT.CodigoEntidad
			LEFT JOIN Terceros TER ON ENT.CodigoTercero = TER.CodigoTercero
			JOIN TipoInstrumento TIN ON VAL.CodigoTipoInstrumentoSBS = TIN.CodigoTipoInstrumentoSBS
			LEFT JOIN CustodioInformacion CUI on VAL.CodigoISIN = CUI.CodigoISIN AND @CodigoCustodio = CUI.CodigoCustodio 
			AND @CodigoPortafolioSBS = CUI.CodigoPortafolioSBS AND CUI.FechaCorte = @fechaCorte 
			LEFT JOIN CarteraTituloValoracion CTV ON CTV.CodigoMnemonico = VAL.CodigoNemonico AND CTV.FechaValoracion = @fechaCorte AND (CTV.CodigoPortafolioSBS = @CodigoPortafolioSBS Or @FlagPortafolio = 1) AND Escenario = 'REAL'                                
			ORDER BY CodigoISIN
			
			/*LISTAMOS TODOS LOS CUSTODIOS DEL REPORTE*/
			SELECT DISTINCT Portafolio, CodCustodio, Custodio  FROM #TABLA_CABECERA
			
			/* LISTAMOS LAS CABECERAS */
			SELECT * FROM #TABLA_CABECERA
			ORDER BY CodigoISIN
			
			/* LISTAMOS LOS DETALLES */  
			INSERT INTO #TABLA_DETALLE
			SELECT CodigoMnemonico, dbo.fn_SIT_gl_ConvertirFechaaString(FechaOperacion) FechaOperacion,
			dbo.fn_SIT_gl_ConvertirFechaaString(FechaLiquidacion) FechaLiquidacion, CodigoOrden, UPPER(O.Descripcion) Operacion,ISNULL(OI.CantidadOrdenado,OI.CantidadOperacion), --10176
			T.Descripcion Contraparte, EO.Descripcionestado + '-Sin liquidar' Estado,0,0,CodigoISIN
			FROM OrdenInversion OI  
			INNER JOIN Operacion O ON OI.CodigoOperacion = O.CodigoOperacion  
			INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero  
			INNER JOIN EstadoOI EO ON EO.CodigoEstado = OI.Estado  
			WHERE CodigoMnemonico IN (SELECT CodigoMnemonico FROM #TABLA_CABECERA) AND OI.Situacion = 'A'  
			AND CodigoPortafolioSBS = @CodigoPortafolioSBS AND FechaOperacion <= @fechaCorte AND FechaLiquidacion > @fechaCorte
			AND OI.Estado <> 'E-ELI' AND OI.CodigoOperacion in ('1','2') -- Ipastor: Solo se consideran las operaciones de compra y venta
			
			--Liberadas
			INSERT INTO #TABLA_DETALLE
			SELECT DRL.CodigoNemonico,dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaIDI),dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaEntrega),DRL.Identificador,
			O.Descripcion,MontoNominalLocal = ISNULL(DRL.MontoNominalLocal, ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico, DRL.FechaCorte),0,1)),
			T.Descripcion ,'Confirmada-Sin liquidar',0,
			MontoNominalLocal = ISNULL(DRL.MontoNominalLocal, ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico, DRL.FechaCorte),0,1)),v.CodigoISIN
			FROM DividendosRebatesLiberadas DRL
			JOIN Operacion O ON O.CodigoOperacion = '37'
			JOIN Valores V ON V.CodigoNemonico = DRL.CodigoNemonico
			JOIN Entidad E ON E.CodigoEntidad = V.CodigoEmisor
			JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
			WHERE DRL.TipoDistribucion  = 'L' AND DRL.Situacion = 'A' AND DRL.CodigoPortafolioSBS = @CodigoPortafolioSBS
			AND DRL.FechaEntrega > @fechaCorte AND DRL.FechaIDI <= @fechaCorte
			--
			
			SELECT @MAX = MAX(IDREGISTRO) FROM #TABLA_DETALLE
			WHILE @MIN <= @MAX  
			BEGIN  
				SELECT @NEMONICO = CodigoMnemonico FROM #TABLA_DETALLE WHERE IDREGISTRO = @MIN
				UPDATE #TABLA_DETALLE  
				SET  TotalCompras = ISNULL((SELECT SUM(ISNULL(CantidadOperacion,0)) FROM #TABLA_DETALLE WHERE CodigoMnemonico = @NEMONICO AND Operacion = 'COMPRA'),0),  
				TotalVentas = ISNULL((SELECT SUM(ISNULL(CantidadOperacion,0)) FROM #TABLA_DETALLE WHERE CodigoMnemonico = @NEMONICO AND Operacion = 'VENTA'),0)  
				WHERE IDREGISTRO = @MIN  
				SET @MIN = @MIN + 1  
			END
			----Liberadas
			--INSERT INTO #TABLA_DETALLE
			--SELECT DRL.CodigoNemonico,dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaIDI),dbo.fn_SIT_gl_ConvertirFechaaString(DRL.FechaEntrega),DRL.Identificador,
			--O.Descripcion,DRL.MontoNominalLocal, T.Descripcion ,'Confirmada-Sin liquidar',0,drl.MontoNominalLocal, v.CodigoISIN
			--FROM DividendosRebatesLiberadas DRL
			--JOIN Operacion O ON O.CodigoOperacion = '37'
			--JOIN Valores V ON V.CodigoNemonico = DRL.CodigoNemonico 
			--JOIN Entidad E ON E.CodigoEntidad = V.CodigoEmisor 
			--JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
			--WHERE DRL.TipoDistribucion  = 'L' AND DRL.Situacion = 'A' AND DRL.CodigoPortafolioSBS = @CodigoPortafolioSBS 
			--AND DRL.FechaEntrega > @fechaCorte AND DRL.FechaIDI <= @fechaCorte
			----
			SELECT CodigoMnemonico,FechaOperacion,FechaLiquidacion,CodigoOrden,Operacion,CantidadOperacion,Contraparte,
			Estado,TotalVentas,TotalCompras,CodigoISIN
			FROM #TABLA_DETALLE
			ORDER BY CodigoISIN
		END
	DROP TABLE #TABLA_CABECERA
	DROP TABLE #TABLA_DETALLE
	DROP TABLE #InstrumentosNoRegistrados
END
GO

GRANT EXECUTE ON [dbo].[DiferenciasInstrumentosRecompile_Listar_ConDetalle] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN EJECUCIÓN - DiferenciasInstrumentosRecompile_Listar_ConDetalle'

PRINT 'INICIO EJECUCIÓN - sp_SIT_OBT_OtrasCXP'

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_OBT_OtrasCXP]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_OBT_OtrasCXP]
GO
-------------------------------------------------------------------------      
--Objetivo: Calcula el campo Otras CXP segun los datos de la fecha en consulta      
--Parámetros:      
-- @p_CodigoPortafolio - Codigo del Portafolio      
-- @p_FechaOperacion - Fecha de la operacion      
-- @p_ChequePendiente - Monto ingresado de los cheques pendientes      
-- @P_@OtrasCXPo  - Resultado de la operacion      
----------------------------------------------------------------------------------------------------      
-- Fecha Modificación: 25/01/2017      
-- Modificado por: Carlos Espejo      
-- Nro. Orden de Trabajo: 9851      
-- Descripcion del cambio: Creacion      
----------------------------------------------------------------------------------------------------      
-- Fecha Modificación: 16/02/2017      
-- Modificado por: Carlos Espejo      
-- Nro. Orden de Trabajo: 9993      
-- Descripcion del cambio: Se cambia RECAUDO por SUSCRIPCION      
-------------------------------------------------------------------------      
-- Fecha Modificación: 20/02/2017      
-- Modificado por: Carlos Espejo      
-- Nro. Orden de Trabajo: 10001      
-- Descripcion del cambio: Se ajusta la fecha para los dias domingos y feriados      
--------------------------------------------------------------------------------      
-- Fecha Modificación: 16/02/2017      
-- Modificado por: Carlos Espejo      
-- Nro. Orden de Trabajo: 9981      
-- Descripcion del cambio: Se cambio el campo @p_MesAnterior para obtener el numero de meses a usar      
----------------------------------------------------------------------------------------------------      
-- Fecha Modificación: 02/05/2017      
-- Modificado por: Carlos Espejo      
-- Nro. Orden de Trabajo: 10335      
-- Descripcion del cambio:  Se indica si existe datos para la caja de recaudo      
-----------------------------------------------------------------------------------------------------    
-- Fecha de Modificación: 21/09/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11568    
-- Descripción del cambio: Se agrega detalle de interés de aumento de capital.    
------------------------------------------------------------------------------------------------------    
-- Fecha de Modificación: 02/10/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11547    
-- Descripción del cambio: Se retira detalle de interés de aumento de capital.    
------------------------------------------------------------------------------------------------------    
-- Fecha de Modificación: 02/11/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11652   
-- Descripción del cambio: Se agrega detalle de interés de aumento de capital.    
------------------------------------------------------------------------------------------------------    
    
CREATE PROC [dbo].[sp_SIT_OBT_OtrasCXP](      
 @p_CodigoPortafolio VarChar(10),      
 @p_FechaOperacion Numeric(8),      
 @p_ChequePendiente Numeric(22,7),      
 @p_RescatePendiente Numeric(22,7),      
 @p_MesAnterior  NUMERIC(8) --9981      
)      
AS      
BEGIN      
 DECLARE @ComisionSAFM NUMERIC(22,7), @CajaRecaudo NUMERIC(22,7), @Suscripcion NUMERIC(22,7),@p_FechaOperacionFeriado NUMERIC(8)      
 --Si no hay registros se toma el dia ultimo registrado para las Operaciones de Caja      
 IF NOT EXISTS(SELECT 1 FROM SaldosBancarios SB      
  JOIN CuentaEconomica CE ON CE.NumeroCuenta = SB.NumeroCuenta AND CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND CE.CodigoClaseCuenta = '10'--10335      
  WHERE SB.CodigoPortafolioSBS = @p_CodigoPortafolio AND SB.FechaOperacion = @p_FechaOperacion )      
   SELECT TOP 1 @p_FechaOperacionFeriado = FechaOperacion FROM SaldosBancarios SB      
   JOIN CuentaEconomica CE ON CE.NumeroCuenta = SB.NumeroCuenta AND CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND CE.CodigoClaseCuenta = '10'--10335      
   WHERE SB.CodigoPortafolioSBS = @p_CodigoPortafolio AND SB.FechaOperacion < @p_FechaOperacion      
   ORDER BY FechaOperacion DESC      
  ELSE      
   SET @p_FechaOperacionFeriado = @p_FechaOperacion      
 --ComisionSAFM      
 SELECT @ComisionSAFM = SUM(CASE WHEN P.TipoNegocio = 'MANDA' THEN ComisionUnificadaMandato ELSE ComisionSAFM END) FROM ValorCuota VC      
 JOIN Portafolio P ON P.CodigoPortafolioSBS = VC.CodigoPortafolioSBS      
 WHERE VC.CodigoPortafolioSBS = @p_CodigoPortafolio AND FechaProceso < @p_FechaOperacion AND FechaProceso >= @p_MesAnterior --9981      
 --Saldo Bancario - Caja Recaudo      
 SELECT @CajaRecaudo = SUM(SB.SaldoDisponibleFinalConfirmado) FROM SaldosBancarios SB      
 JOIN CuentaEconomica CE ON CE.NumeroCuenta = SB.NumeroCuenta AND CE.CodigoMoneda = SB.CodigoMoneda      
 AND CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND CE.CodigoClaseCuenta = '10'      
 JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera      
 JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero       
 WHERE SB.CodigoPortafolioSBS = @p_CodigoPortafolio AND SB.FechaOperacion = @p_FechaOperacionFeriado      
 --SUSCRIPCION      
 SELECT @Suscripcion = SUM(Importe) FROM OperacionesCaja      
 WHERE  CodigoPortafolioSBS = @p_CodigoPortafolio AND FechaOperacion = @p_FechaOperacion AND CodigoOperacion = '108' AND Situacion = 'A' --9993      
     
-- Aumento de Capital  
DECLARE @totalInteresDistribucion NUMERIC(22,7)  
SELECT    
 @totalInteresDistribucion = SUM(O.MontoNetoOperacion - ACD.GastoComision)  
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
  O.CodigoPortafolioSBS = @p_CodigoPortafolio    
  AND O.Situacion='A'    
  AND O.CodigoOperacion IN ('35')    
  AND O.Estado = 'E-CON'  
  AND O.FechaOperacion <= @p_FechaOperacion  
  AND CxP.FechaVencimiento > @p_FechaOperacion  
  AND P.FlagAumentoCapital = 1  
  AND AC.Estado = 'A'  
  
 --Resultado      
 SELECT ISNULL(@ComisionSAFM,0) ComisionSAFM,ISNULL(@CajaRecaudo,0) CajaRecaudo,ISNULL(@Suscripcion,0) Suscripcion,      
 @p_ChequePendiente  ChequePendiente, @p_RescatePendiente RescatePendiente,    ISNULL(@totalInteresDistribucion,0) totalInteresDistribucion,   
 ISNULL(@ComisionSAFM,0) + ISNULL(@CajaRecaudo,0) + ISNULL(@Suscripcion,0) + @p_ChequePendiente + @p_RescatePendiente  +  ISNULL(@totalInteresDistribucion,0)  OtrasCXP      
END   
GO
   
GRANT EXECUTE ON [dbo].[sp_SIT_OBT_OtrasCXP] TO [rol_sit_fondos] AS [dbo]
GO
 
PRINT 'FIN EJECUCIÓN - sp_SIT_OBT_OtrasCXP'
GO

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 
  



 