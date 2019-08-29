USE [SIT-FONDOS]
GO

-- ==================================================================================================
-- Autor: Ernesto Galarza	
-- Fecha Creación: 15/01/2019
-- Órden de Trabajo: 11636
-- Descripción:		Insertar tipo instrumento y clase instrumento para facturas negociables. 
--					Ademas de la opcion Vector Factoring para la carga de importacion SBS.
-- ==================================================================================================

BEGIN TRANSACTION __Transaction_Log

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Valores]') and upper(name) = upper('CodigoFactura'))
	ALTER TABLE Valores add CodigoFactura VARCHAR(30);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Valores]') and upper(name) = upper('PrecioDevengado'))
	ALTER TABLE Valores add PrecioDevengado CHAR(1);

GO

--Eliminacion previa	
delete from dbo.ParametrosGenerales where Clasificacion='ImporSBS' and Valor='VF'
delete from ClaseInstrumento where CodigoClaseInstrumento='19'
delete from TipoInstrumento where CodigoTipoInstrumento='FACN'

--Insercion
INSERT INTO [ParametrosGenerales] (Clasificacion,Nombre,Valor,Comentario,Valor2,Orden,Valor3) 
VALUES('ImporSBS','Vector Factoring','VF','','',NULL,NULL)

INSERT INTO dbo.ClaseInstrumento (CodigoClaseInstrumento, Categoria, Situacion, Descripcion, UsuarioCreacion, FechaCreacion, HoraCreacion, UsuarioModificacion, FechaModificacion, Host, HoraModificacion)
VALUES ('19', 'FA', 'A', 'Facturas Negociables', 'SYSTEM', 20181220, '10:00:00', '', 0, '192.168.0.50', '')

INSERT INTO dbo.TipoInstrumento (CodigoTipoInstrumento, CodigoTipoInstrumentoSBS, Descripcion, PlazoLiquidacion, UsuarioCreacion, FechaCreacion, CodigoRenta, UsuarioModificacion, CodigoClaseInstrumento, HoraCreacion, Situacion, FechaModificacion, CodigoTipoValorizacion, Host, HoraModificacion, GrupoRiesgo, PND, PNND, MetodoCalculoRenta)
VALUES ('FACN', '98', 'FACTURAS NEGOCIABLES', '2', 'SYSTEM', 20181220, '1', '', '19', '10:00:00', 'A', 20181220, '1', '141.81.2.55', '', 'FACN', 5, 5, '1')


UPDATE 
	Valores
SET
	PrecioDevengado = '1'
WHERE 
	CodigoNemonico IN (
					   'BDCA1BC1A',
					   'BDCA1BC1B',
					   'BDCA1BC1C',
					   'BDCA1BC1E',
					   'BDCA1BC2',
					   'BDCA1BC3A'
					   )

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 


USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[sp_SIT_ins_PrecioValorCuota]    Script Date: 01/23/2019 11:47:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ins_PrecioValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ins_PrecioValorCuota]
GO

-----------------------------------------------------------------------------------------------------------
--Objetivo: Insertar los valores cuotas calculados en le tabla de precios cuando valoriza.
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/01/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Creacion
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
	
	-- Generación de Precios para Instrumentos con Flag Precio Devengado
	DECLARE
		@tmpInstrumentosDevengados TABLE(
										  codigoMnemonico VARCHAR(20),
										  montoNominal DECIMAL(22,7),
										  interes DECIMAL(22,7),
										  cantidad DECIMAL(22,7),
										  codigoTipoCupon VARCHAR(4),
										  codigoIsin VARCHAR(20)
										 )
	DECLARE
		@FechaDiaSiguiente DATETIME = CONVERT(DATETIME, CONVERT(CHAR(8), @p_FechaProceso)),
		@FechaT1 DECIMAL(8)--,
	--		@p_CodigoPortafolioSBS VARCHAR(12)='47', 
	--@p_FechaProceso NUMERIC(8)=20190122
	
		SET @FechaDiaSiguiente = DATEADD(DAY,1, @FechaDiaSiguiente)
		SET @FechaT1 =(SELECT YEAR(@FechaDiaSiguiente) * 10000 + MONTH(@FechaDiaSiguiente) * 100 + DAY(@FechaDiaSiguiente))	
		
	INSERT INTO 
		@tmpInstrumentosDevengados 
	SELECT 
		VL.CodigoNemonico,
		ISNULL(VL.MontoNominal,0),
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
								 AND VTC.Fecha = @FechaT1     
								 AND VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,VA.CodigoMoneda,@FechaT1)        
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


USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_FormaValorizacion1]') AND type in (N'P', N'PC'))
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
		InteresCorridoCompra = CASE WHEN CI.Categoria = 'CD' THEN 0 ELSE ROUND(DBO.CalcularInteresCorridoCompra(CTV.CodigoPortafolioSBS,V.CodigoISIN,CTV.FechaValoracion,'1'),2) END, --17
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


USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ActualizaSaldoUnidadesPosterior]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_ActualizaSaldoUnidadesPosterior]
GO
-------------------------------------------------------------------------------------------------------------------------------------  
--	Fecha modificacion: 23/01/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se modifico para que considere las cantidad de facturas negociables como una unidad
------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[sp_SIT_ActualizaSaldoUnidadesPosterior] (@P_FechaOperacion  NUMERIC(8),@P_CodigoNemonico VarChar(20) , @P_CodigoPortafolio VarChar(20))
AS
BEGIN
	DECLARE @Compra Numeric(22,7), @Disponible Numeric(22,7),@Venta Numeric(22,7), @Liberada Numeric(22,7), @Redencion Numeric(22,7), 
	@SaldoDisponibleActual Numeric(22,7),@FechaSaldo Numeric(8)
	--Saldo del dia de proceso
	SELECT @SaldoDisponibleActual = SALDODISPONIBLE + SaldoProcesoCompra - SaldoProcesoVenta + SaldoUnidadesLiberadas - Redencion FROM SaldosCarteraTitulo
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio  and fechaSaldo = @P_FechaOperacion  and CodigoMnemonico = @P_CodigoNemonico 
	--verificar si es factura.
	IF (EXISTS(SELECT 1 FROM valores WHERE CodigoNemonico = @P_CodigoNemonico and CodigoFactura is not null))	
	SELECT @SaldoDisponibleActual = (SELECT MontoNominal FROM PrevOrdenInversion WHERE  CodigoNemonico = @P_CodigoNemonico )
	
	--Recorremos los dias posteriores y actualizaremos el saldo incial del dia tomando en cuenta los saldos de compra, venta, liberadas y redencion
	DECLARE curPortaNem CURSOR FOR 
	SELECT SALDODISPONIBLE,SALDOPROCESOCOMPRA,SALDOPROCESOVENTA,SALDOUNIDADESLIBERADAS ,Redencion ,FECHASALDO  FROM SaldosCarteraTitulo
	WHERE FECHASALDO > @P_FechaOperacion AND CODIGOPORTAFOLIOSBS = @P_CodigoPortafolio AND CodigoMnemonico = @P_CodigoNemonico 
	ORDER BY FECHASALDO ASC
	OPEN curPortaNem 
	FETCH NEXT FROM curPortaNem INTO @Disponible,@Compra,@Venta,@Liberada,@Redencion,@FechaSaldo
	WHILE @@FETCH_STATUS = 0 
	BEGIN		
		DECLARE @SaldoInicialNuevo Numeric(22,7)
		--La primera vez actulizamos el saldo inicial posterior con el calculado del dia
		IF @SaldoDisponibleActual > 0
		BEGIN
			--Saldos Carter
			UPDATE SaldosCarteraTitulo SET SALDODISPONIBLE = @SaldoDisponibleActual 
			WHERE CODIGOPORTAFOLIOSBS = @P_CodigoPortafolio AND FECHASALDO = @FechaSaldo AND CodigoMnemonico = @P_CodigoNemonico
			--Custodio Saldo
			UPDATE CS
			SET CS.SaldoInicialUnidades = SCT.SALDODISPONIBLE , CS.IngresoUnidades = (SCT.SALDOPROCESOCOMPRA + SCT.SALDOUNIDADESLIBERADAS ),
			CS.EgresoUnidades = SCT.SALDOPROCESOVENTA, CS.Redencion = 0 , CS.UnidadesBloqueadas = 0, CS.UnidadesGarantia = 0
			FROM SaldosCarteraTitulo SCT
			JOIN CustodioSaldo CS ON CS.CodigoMnemonico = SCT.CodigoMnemonico  AND CS.FechaSaldo = SCT.FECHASALDO  AND CS.CodigoPortafolioSBS = SCT.CODIGOPORTAFOLIOSBS 
			WHERE CS.CodigoPortafolioSBS = @P_CodigoPortafolio AND CS.fechaSaldo = @FechaSaldo  AND CS.CodigoMnemonico = @P_CodigoNemonico
			--Obetenmos el saldo inicial nuevo en caso haya mas dias afectados
			SELECT @SaldoInicialNuevo = SALDODISPONIBLE + SaldoProcesoCompra - SaldoProcesoVenta + SaldoUnidadesLiberadas - Redencion FROM SaldosCarteraTitulo
			WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND fechaSaldo = @FechaSaldo AND CodigoMnemonico = @P_CodigoNemonico  
			SET @SaldoDisponibleActual = 0
		END
		ELSE
		BEGIN
			--Se actualiz el saldo con el saldo calculado del dia anterior
			UPDATE SaldosCarteraTitulo SET SALDODISPONIBLE = @SaldoInicialNuevo 
			WHERE CODIGOPORTAFOLIOSBS = @P_CodigoPortafolio AND FECHASALDO = @FechaSaldo AND CodigoMnemonico = @P_CodigoNemonico
			--Custodio Saldo
			UPDATE CS
			SET CS.SaldoInicialUnidades = SCT.SALDODISPONIBLE , CS.IngresoUnidades = (SCT.SALDOPROCESOCOMPRA + SCT.SALDOUNIDADESLIBERADAS ),
			CS.EgresoUnidades = SCT.SALDOPROCESOVENTA, CS.Redencion = 0 , CS.UnidadesBloqueadas = 0, CS.UnidadesGarantia = 0
			FROM SaldosCarteraTitulo SCT
			JOIN CustodioSaldo CS ON CS.CodigoMnemonico = SCT.CodigoMnemonico  AND CS.FechaSaldo = SCT.FECHASALDO  AND CS.CodigoPortafolioSBS = SCT.CODIGOPORTAFOLIOSBS 
			WHERE CS.CodigoPortafolioSBS = @P_CodigoPortafolio AND CS.fechaSaldo = @FechaSaldo  AND CS.CodigoMnemonico = @P_CodigoNemonico
			--Obtenmos el saldo inicial nuevo en caso haya mas dias afectados
			SELECT @SaldoInicialNuevo = SALDODISPONIBLE + SaldoProcesoCompra - SaldoProcesoVenta + SaldoUnidadesLiberadas - Redencion FROM SaldosCarteraTitulo
			WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND fechaSaldo = @FechaSaldo AND CodigoMnemonico = @P_CodigoNemonico  
		END
		FETCH NEXT FROM curPortaNem INTO @Disponible,@Compra,@Venta,@Liberada,@Redencion,@FechaSaldo
	END
	CLOSE curPortaNem 
	DEALLOCATE curPortaNem
END


GO

GRANT EXECUTE ON [dbo].[sp_SIT_ActualizaSaldoUnidadesPosterior] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorizacionCartera_Generar2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorizacionCartera_Generar2]
GO

--------------------------------------------------------------------------------------------------------------------------
--Objetivo: Valorizar todos los tipos de instrumentos
--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 11/10/2016
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9396
--	Descripcion del cambio: Creacion
--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 07/08/2018
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11432
--	Descripcion del cambio: Valorizar teniendo en cuenta el tipo de precio del portafolio
--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 16/08/2018
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11512
--	Descripcion del cambio: Valorizar teniendo en cuenta el PrecioLimpio o PrecioSucio si uno de ellos es cero
--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 30/08/2018
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Reemplazar la función dbo.RetornarVectorPrecioT_1 por dbo.RetornarSecuenciaVectorTipoCambio
--                          y dbo.RetornarSecuenciaFuenteVTC
--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 11/12/2018
--	Creado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11698
--	Descripcion del cambio: Se agrega validación para portafolio que no tengan OI vigentes.
--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 11/12/2018
--	Creado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se incrementa tamaño input @difd1 a NUMERIC(4)
--------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ValorizacionCartera_Generar2]
	@p_FechaOperacion NUMERIC(8,0),
	@p_CodigoPortafolio VARCHAR(10),
	@p_EntidadExtPrecio VARCHAR(11),
	@p_EntidadExtTipoCambio VARCHAR(11),
	@p_TipoValorizacion VARCHAR(1),
	@p_usuario VARCHAR(15),
	@p_fecha NUMERIC(8,0),
	@p_Hora VARCHAR(10),
	@p_Host VARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE 
		@escenario VARCHAR(8),  
		@tem DECIMAL(25, 10),
		@tem1 DECIMAL(25, 10),
		@Secuencial NUMERIC(20,8),
		@intMin INT,
		@intMax INT,
		@p_cantidad NUMERIC(22,10),
		@p_codigonemonico VARCHAR(15),
		@valorprecio NUMERIC(22,10),
		@valorUnitario NUMERIC(22,7),
		@valorTC NUMERIC(22,10),  
		@p_ValorNominalMonedaOrigen NUMERIC(22,7),
		@p_error INT,
		@Renta VARCHAR(3),
		@p_intTipTas VARCHAR(4),
		@duranio NUMERIC(22,7),
		@durmod NUMERIC(22,7),
		@sumaTIR NUMERIC(22,7),
		@sumaTIR2 NUMERIC(22,7),
		@intdpzMin INT,
		@intdpzMax INT,
		@monto NUMERIC(28,20),
		@MiDias NUMERIC(8,0),
		@MaDias NUMERIC(8,0),
		@Mi1 NUMERIC(22,7),
		@Mi2 NUMERIC(22,7),
		@Mi3 NUMERIC(22,7),
		@Mi4 NUMERIC(22,7),
		@tasa1 NUMERIC(22,7),
		@tasa2 NUMERIC(22,7),
		@RMN NUMERIC(22,7),
		@RME NUMERIC(22,7),
		@orden VARCHAR(10),
		@e NUMERIC(22,0),
		@N NUMERIC(22,7),
		@Fot NUMERIC(22,7),
		@Tt integer,
		@t integer,  
		@TCSpot NUMERIC(22,7),
		@intMindpz INT,
		@intMaxdpz INT,
		@FechaOperacion NUMERIC (8,0),
		@FechaContrato NUMERIC(8,0),
		@p_dias INT,
		@vpnantlocal NUMERIC(22,7),
		@vpnantoriginal NUMERIC(22,7),
		@ValorDPZLocal NUMERIC(22,9),
		@ValorDPZOrigen NUMERIC(22,9),
		@difd NUMERIC(22),
		@difd1 NUMERIC(4,0),
		@baset NUMERIC(22,17),
		@ptasa  NUMERIC(22,17),
		@vFechaOperacion NUMERIC(8,0),
		@tasaefecdiaria NUMERIC(22,9),
		@tmp1 NUMERIC(32,10),
		@LimiteSuperior NUMERIC(22,7),
		@delta NUMERIC(22,7),
		@LimiteInferior NUMERIC(22,7),
		@cont INT,
		@tmpYTMAnt NUMERIC(22,7),
		@tmpMO NUMERIC(22,7),
		@nvcCodigoSBS NVARCHAR(12),
		@FechaUltimaValoracion NUMERIC(8),
		@Compras NUMERIC(22,7),
		@Ventas NUMERIC(22,7),
		@CodigoTipoInstrumentoSBS VARCHAR(3),
		@FechaNegocio NUMERIC(8),
		@TipoCalculo CHAR(1),
		@CodMonedaFondo VARCHAR(20), 
		@TipoCambioFonfdo NUMERIC(22,10), 
		@TipoCalculoPorttafolio CHAR(1), 
		@TipoCupon Char(1),
		@MontoCancelacion VARCHAR(22)
		
	SELECT 
		@CodMonedaFondo = CodigoMoneda
	FROM 
		Portafolio 
	WHERE 
		CodigoPortafolioSBS = @p_CodigoPortafolio
	
	SELECT 
		@TipoCalculoPorttafolio = TipoCalculo,
		@TipoCambioFonfdo = VTC.ValorPrimario
	FROM 
		Moneda M
	JOIN 
		VectorTipoCambio VTC ON VTC.CodigoMoneda = M.CodigoMoneda 
								AND VTC.Fecha = @p_FechaOperacion
								AND VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,M.CodigoMoneda,@p_FechaOperacion)
	WHERE 
		M.CodigoMoneda = @CodMonedaFondo
	
	CREATE TABLE #CarteraTituloValoracion(
		Secuencial INT IDENTITY(1,1),
		Escenario VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS,
		CODIGOPORTAFOLIOSBS VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS,
		Cantidad NUMERIC(22,9),
		VectorPrecio NUMERIC (22,9),
		VectorTipoCambio NUMERIC(22,9),
		VPNOriginal NUMERIC(22,9),
		VPNLocal NUMERIC(22,9),
		ValorUnitario NUMERIC(22,9),
		ValorNominalMonedaOrigen NUMERIC(22,9),
		ValorNominalMonedaLocal NUMERIC(22,9),
		CodigoMnemonico VARCHAR(15) COLLATE SQL_Latin1_General_CP1_CI_AS,
		Tir360 NUMERIC(22,9),
		Duracion NUMERIC(22,9),
		TIROrigen NUMERIC(22,9),
		TIRSoles NUMERIC(22,9),
		Convexidad NUMERIC(22,9),
		ValorCausadoLocal NUMERIC(22,9),
		ValorCausadoOriginal NUMERIC(22,9),
		codigosbs VARCHAR(12) COLLATE SQL_Latin1_General_CP1_CI_AS,
		TipoRenta VARCHAR(5) COLLATE SQL_Latin1_General_CP1_CI_AS,
		CodigoTipoCupon VARCHAR(4) COLLATE SQL_Latin1_General_CP1_CI_AS,
		TipoCalculo CHAR(1),
		CodigoTipoInstrumentoSBS VARCHAR(3)
	)
	CREATE TABLE #tmpdpz(
		Secuencia INT IDENTITY(1,1),
		codigoportafoliosbs VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS,
		montoprecancelar NUMERIC(22,9),
		codigomoneda VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS,
		codigomnemonico VARCHAR(15) COLLATE SQL_Latin1_General_CP1_CI_AS,
		TasaPorcentaje NUMERIC(22,9),
		FechaOperacion NUMERIC(8,0),
		FechaContrato NUMERIC(8,0),
		CodigoSBS NVARCHAR(12) COLLATE SQL_Latin1_General_CP1_CI_AS,
		VectorTipoCambio NUMERIC(22,8),
		BaseTIR NUMERIC(18),
		TipoCalculo char(1),
		TipoCupon Char(1)		
	)
				
	SELECT 
		@p_error=0 
		 
	SELECT 
		@FechaUltimaValoracion = MAX(FechaValoracion) 
	FROM 
		CarteraTituloValoracion  
	WHERE 
		CodigoPortafolioSbs = @p_CodigoPortafolio 
		AND FechaValoracion < @p_FechaOperacion 
		AND Escenario='REAL' 
		 
	SELECT 
		@FechaNegocio = FechaNegocio 
	FROM 
		Portafolio 
	WHERE 
		CodigoPortafolioSBS = @p_CodigoPortafolio
		
	BEGIN TRANSACTION
		DELETE FROM 
			tmpNemonicoValorizacion
			
		IF @p_TipoValorizacion = 'R'
		BEGIN
			SET @escenario = 'REAL'
			EXEC dbo.GeneraSaldosCarteraTitulo_Insertar 
														@FechaUltimaValoracion, 
														@p_FechaOperacion, 
														@p_CodigoPortafolio, 
														@p_usuario, 
														@p_fecha, 
														@p_Hora, 
														@p_Host  
		END
		ELSE
			SET @escenario = 'ESTIMADO'
			  
		INSERT INTO 
			#CarteraTituloValoracion  
		SELECT 
			@escenario,
			sct.CodigoPortafolioSbs,
			Cantidad = (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0)+ISNULL(saldounidadesliberadas,0)),
			VectorPrecio = CASE WHEN @p_TipoValorizacion = 'R' THEN	ROUND((CASE WHEN ISNULL(VP.PrecioSucio,0) <> 0 THEN VP.PrecioSucio ELSE VP.PrecioLimpio END),7,1)
															   ELSE ROUND(dbo.RetornarSecuenciaVectorPrecio(SCT.CODIGOPORTAFOLIOSBS,V.CodigoNemonico,@p_FechaOperacion),7,1) END,
			--ROUND(dbo.RetornarVectorPrecioT_1(V.CodigoNemonico,@p_EntidadExtPrecio,@p_FechaOperacion),9,1)
			VectorTipoCambio = dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,V.CodigoMoneda,@p_FechaOperacion),
			0 VPNOriginal,
			0 VPNLocal, 
			ValorUnitario = ISNULL(v.ValorUnitario,1),
			ValorNominalMonedaOrigen = (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0) + 
										ISNULL(saldounidadesliberadas,0))*V.ValorUnitario,
			ValorNominalMonedaLocal = (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0) + 
									   ISNULL(saldounidadesliberadas,0))*V.ValorUnitario*dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,V.CodigoMoneda,@p_FechaOperacion),
			sct.CodigoMnemonico, 
			0 TIR360, 
			0 Duracion,
			0 TIROrigen, 
			0 TIRSoles, 
			0 Convexidad,
			0 ValorCausadoLocal, 
			0 ValorCausadoOriginal, 
			V.Codigosbs,
			V.TipoRenta, 
			CodigoTipoCupon = ISNULL(V.codigotipocupon,'1') 
			,M.TipoCalculo,
			V.CodigoTipoInstrumentoSBS
		FROM 
			dbo.SaldosCarteraTitulo SCT  
		INNER JOIN 
			Valores V ON V.CodigoNemonico = SCT.CodigoMnemonico
		LEFT JOIN
			VectorPrecio VP ON VP.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolio,V.CodigoNemonico,@p_FechaOperacion)
							   AND VP.fecha=@p_FechaOperacion and VP.CodigoMnemonico=SCT.CodigoMnemonico  
		INNER JOIN 
			Moneda M ON M.CodigoMoneda = V.CodigoMoneda 

		WHERE 
			SCT.FECHASALDO = CASE WHEN @p_TipoValorizacion = 'R' THEN @p_FechaOperacion ELSE @FechaUltimaValoracion END  
			AND SCT.CODIGOPORTAFOLIOSBS = @p_CodigoPortafolio 
			AND V.TipoRenta = 2 --RENTA VARIABLE  
			AND V.CodigoNemonico <> 'DIVISA'
			AND (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0)+ISNULL(saldounidadesliberadas,0))>0  
			AND V.situacion='A'
		UNION  
		SELECT 
			@escenario,
			sct.CodigoPortafolioSbs,
			Cantidad = (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0)+ISNULL(saldounidadesliberadas,0)),  
			VectorPrecio = CASE WHEN @p_TipoValorizacion = 'R' THEN ROUND( (CASE WHEN ISNULL(VP.PrecioSucio,0) <> 0 THEN VP.PrecioSucio ELSE VP.PrecioLimpio END) ,7,1)
	   														   ELSE ROUND(dbo.RetornarSecuenciaVectorPrecio(SCT.CODIGOPORTAFOLIOSBS,V.CodigoNemonico,@p_FechaOperacion),7,1) END,
														  			--ROUND(vp.ValorPrecio,9,1)
																	--ROUND(dbo.RetornarVectorPrecioT_1(V.CodigoNemonico,@p_EntidadExtPrecio,@p_FechaOperacion),9,1)

			VectorTipoCambio = dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,V.CodigoMoneda,@p_FechaOperacion),  
			0 VPNOriginal,
			0 VPNLocal, 
			ValorUnitario = ISNULL(v.ValorUnitario,1),
			ValorNominalMonedaOrigen = (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0) + 
										ISNULL(saldounidadesliberadas,0))*V.ValorUnitario,  
			ValorNominalMonedaLocal = (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0) + 
									   ISNULL(saldounidadesliberadas,0))*V.ValorUnitario*dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,V.CodigoMoneda,@p_FechaOperacion),  
			SCT.CodigoMnemonico, 
			0 TIR360, 
			0 Duracion,
			0 TIROrigen, 
			0 TIRSoles, 
			0 Convexidad,
			0 ValorCausadoLocal, 
			0 ValorCausadoOriginal, 
			V.codigosbs,
			V.TipoRenta, 
			CodigoTipoCupon = ISNULL(V.codigotipocupon,'1'),
			M.TipoCalculo,
			V.CodigoTipoInstrumentoSBS
		FROM 
			dbo.SaldosCarteraTitulo SCT
		INNER JOIN 
			Valores V ON V.CodigoNemonico = SCT.CodigoMnemonico
		LEFT JOIN 
			VectorPrecio VP ON VP.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolio,V.CodigoNemonico,@p_FechaOperacion)
							   AND VP.fecha=@p_FechaOperacion 
							   AND VP.CodigoMnemonico=SCT.CodigoMnemonico
		INNER JOIN 
			Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		WHERE 
			SCT.FECHASALDO = CASE WHEN @p_TipoValorizacion = 'R' THEN @p_FechaOperacion ELSE @FechaUltimaValoracion END
			AND SCT.CODIGOPORTAFOLIOSBS = @p_CodigoPortafolio  
			AND V.TipoRenta = 1 
			AND V.CodigoNemonico <> 'DIVISA'  
			AND (ISNULL(SCT.SALDODISPONIBLE,0)+ISNULL(SCT.SALDOPROCESOCOMPRA,0)-ISNULL(SCT.SALDOPROCESOVENTA,0)-ISNULL(SCT.Redencion,0)+ISNULL(saldounidadesliberadas,0))>0  
			AND V.situacion='A'  
			AND (ISNULL(v.FechaVencimiento,0) = 0 OR ISNULL(v.FechaVencimiento,0) > @p_FechaOperacion)
		
		IF @p_TipoValorizacion = 'V' --VALORACION ESTIMADA
			INSERT INTO 
				#CarteraTituloValoracion
			SELECT DISTINCT 
				@escenario, 
				oi.CODIGOPORTAFOLIOSBS,
				0 Cantidad,
				--VectorPrecio = dbo.RetornarVectorPrecioT_1(oi.CodigoMnemonico, @VectorPrecioVal, @p_FechaOperacion),
				VectorPrecio = dbo.RetornarSecuenciaVectorPrecio(oi.CodigoPortafolioSBS, V.CodigoNemonico, @p_FechaOperacion),
				VectorTipoCambio = dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,V.CodigoMoneda,@p_FechaOperacion),
				0 VPNOriginal,
				0 VPNLocal,
				V.ValorUnitario,
				ISNULL(V.ValorNominal,0) ValorNominalMonedaOrigen,
				0 ValorNominalMonedaLocal,
				oi.CodigoMnemonico, 
				0 TIR360,
				0 Duracion,
				0 TIROrigen, 
				0 TIRSoles,
				0 Convexidad,
				0 ValorCausadoLocal,
				0 ValorCausadoOriginal,
				v.codigosbs,
				V.TipoRenta, 
				CodigoTipoCupon = ISNULL(V.codigotipocupon,'1'),
				V.CodigoTipoInstrumentoSBS  
			FROM 
				OrdenInversion oi
			INNER JOIN 
				Valores V ON V.CodigoNemonico = oi.CodigoMnemonico 
			WHERE 
				oi.FechaOperacion > @FechaUltimaValoracion
				AND oi.FechaOperacion <= @p_FechaOperacion
				AND oi.Situacion = 'A'
				AND oi.estado IN ('E-EJE', 'E-CON')
				AND oi.CategoriaInstrumento NOT IN ('DP','FD','CV')
				AND oi.CodigoOperacion IN (1,61,37,39)  
				AND ISNULL(oi.CantidadOperacion,0) > 0  
				AND ISNULL(oi.CodigoSBS,'') <> ''  
				AND oi.CodigoMnemonico NOT IN (SELECT DISTINCT CodigoMnemonico FROM #CarteraTituloValoracion)  
				AND oi.CodigoPortafolioSBS = @p_CodigoPortafolio
				AND oi.TipoFondo <> 'CC_SNC'
			
		SET @intMin = (SELECT MIN(Secuencial) FROM #CarteraTituloValoracion)  
		SET @intMax = (SELECT MAX(Secuencial) FROM #CarteraTituloValoracion) 
	 
		WHILE (@intMin <= @intMax)  
		BEGIN
			SELECT 
				@p_cantidad=Cantidad, 
				@p_codigonemonico=CodigoMnemonico,
				@p_ValorNominalMonedaOrigen=ValorNominalMonedaOrigen,
				@valorprecio = (CASE WHEN CI.Categoria = 'FA' THEN ISNULL(VectorPrecio,-1)/100 ELSE ISNULL(VectorPrecio,-1) END),
				@valorTC=VectorTipoCambio,
				@Renta=Tiporenta, 
				@p_intTipTas = codigotipocupon, 
				@ValorUnitario = ValorUnitario, 
				@TipoCalculo = TipoCalculo
			FROM 
				#CarteraTituloValoracion CTV
			JOIN 
				TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = CTV.CodigoTipoInstrumentoSBS   
			JOIN 
				ClaseInstrumento CI	ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento 
						
			WHERE 
				secuencial = @intMin
				
			IF @p_TipoValorizacion = 'V' --VALORACION ESTIMADA
			BEGIN
				SELECT 
					@Compras = 0, 
					@Ventas = 0  
				--Hallamos el total (P*Q) de compras de las operaciones aun no valoradas  
				SELECT 
					@Compras = SUM(oi.CantidadOperacion) 
				FROM 
					OrdenInversion oi
				JOIN 
					Operacion o ON o.CodigoOperacion = oi.CodigoOperacion
				WHERE 
					oi.FechaOperacion > @FechaUltimaValoracion
					AND oi.FechaOperacion <= @p_FechaOperacion
					AND oi.Situacion = 'A'
					AND oi.estado IN ('E-EJE', 'E-CON')
					AND oi.CategoriaInstrumento NOT IN ('DP','FD','CV') --No se consideran instrumentos UNICOS  
					AND oi.CodigoOperacion IN (1,61,37) --Se quitaron operaciones que no suman unidades  
					AND ISNULL(oi.CantidadOperacion,0) > 0
					AND ISNULL(oi.CodigoSBS,'') <> ''
					AND oi.CodigoMnemonico = @p_codigonemonico  
					AND oi.CodigoPortafolioSBS = @p_CodigoPortafolio
					AND oi.TipoFondo <> 'CC_SNC'
				--Hallamos el total (P*Q) de ventas de las operaciones aun no valoradas  
				SELECT 
					@Ventas = SUM(oi.CantidadOperacion)
				FROM 
					OrdenInversion oi
				JOIN 
					Operacion o ON o.CodigoOperacion = oi.CodigoOperacion
				WHERE 
					oi.FechaOperacion > @FechaUltimaValoracion
					AND oi.FechaOperacion <= @p_FechaOperacion
					AND oi.Situacion = 'A'
					AND oi.estado IN ('E-EJE', 'E-CON')
					AND oi.CategoriaInstrumento NOT IN ('DP','FD','CV') --No se consideran instrumentos UNICOS  
					AND oi.CodigoOperacion in (2,62,39) --Se quitaron operaciones que no disminuyen unidades  
					AND ISNULL(oi.CantidadOperacion,0) > 0
					AND ISNULL(oi.CodigoSBS,'') <> ''
					AND oi.CodigoMnemonico = @p_codigonemonico
					AND oi.CodigoPortafolioSBS = @p_CodigoPortafolio
					AND oi.TipoFondo <> 'CC_SNC'
					
				SELECT 
					@Compras = ISNULL(@Compras,0), 
					@Ventas = ISNULL(@Ventas,0)
					
				SET @p_cantidad = @p_cantidad + @Compras - @Ventas
			END
			--Si es CS Preferente , debe de valorar asi tenga precio 0  
			SELECT 
				@CodigoTipoInstrumentoSBS = CodigoTipoInstrumentoSBS 
			FROM 
				Valores 
			WHERE 
				CodigoNemonico = @p_codigonemonico 
				 
			IF @valorprecio >= 0 OR @CodigoTipoInstrumentoSBS = '40'
			BEGIN
				SET @tem = @p_cantidad * @valorprecio
				
				IF @TipoCalculo = 'I'
				BEGIN
					Set @tem1  = ROUND(@tem / @valorTC , 7, 1)
				END
				ELSE
				BEGIN
					Set @tem1  = ROUND(@tem * @valorTC , 7, 1)
				END
				
				SELECT 
					@vpnantlocal = VPNLOCAL, 
					@vpnantoriginal = VPNORIGINAL
				FROM 
					CarteraTituloValoracion
				WHERE 
					CodigoMnemonico = @p_codigonemonico 
					AND codigoportafoliosbs = @p_CodigoPortafolio 
					AND fechavaloracion = @FechaUltimaValoracion 
					AND situacion = 'A'		
				
				UPDATE 
					#CarteraTituloValoracion
				SET 
					VPNOriginal = (@p_cantidad*@valorprecio),
					VPNLocal =  @tem1, 
					ValorNominalMonedaLocal = CASE WHEN @TipoCalculo = 'I' THEN (@p_ValorNominalMonedaOrigen / @valorTC) 
																		   ELSE (@p_ValorNominalMonedaOrigen * @valorTC) END,
					ValorCausadoLocal = (@tem1 - @vpnantlocal), 
					ValorCausadoOriginal = (@tem - @vpnantoriginal),  
					Cantidad = @p_cantidad
				WHERE 
					Secuencial = @intMin
					
				IF @Renta = '2' OR @Renta = '3' OR NOT Exists(SELECT 1 FROM CuponeraNormal WHERE codigonemonico = @p_codigonemonico)  
					UPDATE 
						#CarteraTituloValoracion
					SET 
						Tir360 = 0,
						Duracion = 0 
					WHERE Secuencial = @intMin  
				ELSE
				BEGIN
					--> Calculo de TIR        
					SELECT @LimiteSuperior= 50,@delta= 0.001,@LimiteInferior= 0,@cont= 0,@tmpMO= 0,@sumaTIR= 0,@tmpYTMAnt= 99
					WHILE (abs(@ValorPrecio-@tmpMO) > @delta  and @sumaTIR <> @tmpYTMAnt and  @cont< 100)      
					BEGIN
						SET @tmpYTMAnt = @sumaTIR
						SET @sumaTIR = (@LimiteSuperior+@LimiteInferior)/2
						EXEC OrdenInversion_CalcularVPN 
														'', 
														@p_codigoNemonico, 
														'', 
														@p_fechaoperacion, 
														@ValorUnitario, 
														@sumaTIR, 
														@p_intTipTas, 
														@tmpMO OUTPUT
						IF (@ValorPrecio-@tmpMO) < 0
							Set @LimiteInferior = @sumaTIR
						ELSE  
							Set @LimiteSuperior = @sumaTIR
						SET @cont = @cont + 1
					END
					--> Calculo de Duracion  
					EXEC OrdenInversion_CalcularDuracion 
														 @p_codigonemonico, 
														 @p_fechaoperacion, 
														 @ValorUnitario,
														 @sumaTIR,  
														 @p_intTipTas,  
														 @durmod OUT,  
														 @duranio OUT
					---aqui hay que actualizar la temporal--------         
					UPDATE
						#CarteraTituloValoracion
					SET 
						Tir360 = ISNULL(@sumaTIR,0),
						Duracion = ISNULL(@durmod,0),
						Convexidad=ISNULL(@duranio,0),
						TIROrigen=ISNULL(@sumaTIR2,0),        
						TIRSoles = CASE WHEN @TipoCalculo = 'I' THEN ISNULL((@sumaTIR2 / @valorTC),0)
																ELSE ISNULL((@sumaTIR2 * @valorTC),0) END
					WHERE 
						Secuencial = @intMin
				END
			END
			ELSE
			BEGIN
				UPDATE 
					#CarteraTituloValoracion 
				SET 
					VPNOriginal = -1 
				WHERE 
					Secuencial = @intMin
					
				INSERT INTO 
					tmpNemonicoValorizacion VALUES(@p_codigonemonico)
			END
			SET @intMin = @intMin + 1
		END
		IF @@ERROR <> 0  
		BEGIN
			SET @p_error=1  
			GOTO Error  
		END
		IF (SELECT COUNT(1) FROM tmpNemonicoValorizacion) > 0
		BEGIN
			DELETE FROM #CarteraTituloValoracion  
			SET @p_error=3  
		END
		ELSE
		BEGIN
			SELECT @Secuencial = MAX(SecuenciaValorizacion) FROM CarteraTituloValoracion
			
			INSERT INTO 
				CarteraTituloValoracion(
										[Escenario], 
										[CodigoPortafolioSBS], 
										[FechaValoracion],
										[Numerotitulo],
										[VPNOriginal], 
										[VPNLocal],
										[ValorCausadoOriginal],
										[ValorCausadoLocal],
										[ValorUnidad],
										[Cantidad],
										[ValorNominalMonedaOrigen], 
										[ValorNominalMonedaLocal],
										[ValorTIROriginal],
										[ValorTIRLocal],
										[DuracionModificada],
										[ValorTIR360],
										[Convexidad],
										[UsuarioCreacion], 
										[FechaCreacion], 
										[HoraCreacion],
										[Host],
										[CodigoMnemonico],
										SecuenciaValorizacion,
										Situacion,
										codigosbs,
										VPNMonedaFondo)
			SELECT 
				Escenario,
				CODIGOPORTAFOLIOSBS,
				@p_FechaOperacion,
				1,
				VPNOriginal,
				VPNLocal,
				ValorCausadoOriginal,
				ValorCausadoLocal,
				VectorPrecio,
				Cantidad,
				ValorNominalMonedaOrigen,
				ValorNominalMonedaLocal,
				TIROrigen,
				TIRSoles,
				Duracion,
				Tir360,
				Convexidad,
				@p_usuario,
				@p_fecha,
				@p_Hora,
				@p_Host,
				CodigoMnemonico,
				Secuencial+@Secuencial,
				'A',
				codigosbs,
				CASE WHEN @TipoCalculoPorttafolio = 'D' THEN VPNLocal /  @TipoCambioFonfdo ELSE  VPNLocal * @TipoCambioFonfdo END
			FROM 
				#CarteraTituloValoracion
			IF @@ERROR <> 0
			BEGIN
				SET @p_error = 1
				GOTO Error
			END
		END
	--> Valoracion de Depositos a Plazo - Operaciones de Reporte  --------
	IF @p_error = 0
	BEGIN
		IF @p_TipoValorizacion = 'R'
			BEGIN
				INSERT INTO 
					#tmpdpz
				SELECT 
					oi.codigoportafoliosbs,
					MontoOperacion = oi.MontoNominalOperacion,
					oi.codigomoneda,
					CASE WHEN OI.CategoriaInstrumento = 'DP' THEN  oi.codigomnemonico ELSE OI.CodigoTipoTitulo END, 
					oi.TasaPorcentaje,
					oi.FechaOperacion, 
					oi.FechaContrato, 
					oi.CodigoSBS,
					VectorTipoCambio = dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,oi.codigomoneda,@p_FechaOperacion),
					ISNULL(v.BaseTIR,360),
					M.TipoCalculo, 
					OI.CodigoTipoCupon 
				FROM 
					OrdenInversion oi
				LEFT JOIN 
					Valores v ON v.Codigonemonico = CASE WHEN OI.CategoriaInstrumento = 'DP' THEN OI.CodigoMnemonico ELSE OI.CodigoTipoTitulo END 
								 AND v.situacion='A'
				JOIN 
					Moneda M ON M.CodigoMoneda = OI.CodigoMoneda
				WHERE
					oi.categoriainstrumento IN ('DP','OR')
					AND oi.Situacion='A' 
					AND oi.FechaContrato >= @p_FechaOperacion 
					AND oi.FechaOperacion <= @p_FechaOperacion 
					AND oi.CodigoOperacion IN ('3','101')
					AND oi.Codigoportafoliosbs = @p_CodigoPortafolio 
					AND oi.Estado IN ('E-EJE', 'E-CON')
					AND OI.TasaPorcentaje IS NOT NULL 
					AND OI.MontoNominalOrdenado IS NOT NULL
			END
		ELSE IF @p_TipoValorizacion = 'V'
			BEGIN
				INSERT INTO 
					#tmpdpz  
				SELECT 
					oi.codigoportafoliosbs,
					MontoOperacion = oi.MontoNominalOperacion,
					oi.codigomoneda, 
					oi.codigomnemonico, 
					oi.TasaPorcentaje,
					oi.FechaOperacion, 
					oi.FechaContrato, 
					oi.CodigoSBS,
					VectorTipoCambio = dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolio,oi.codigomoneda,@p_FechaOperacion),
					ISNULL(v.BaseTIR,360),
					M.TipoCalculo,
					OI.CodigoTipoCupon 
				FROM 
					OrdenInversion oi
				LEFT JOIN 
					Valores v ON v.Codigonemonico=oi.CodigoMnemonico 
								 AND v.situacion='A'
				JOIN 
					Moneda M ON M.CodigoMoneda = OI.CodigoMoneda
				WHERE 
					oi.categoriainstrumento IN ('DP','OR') 
					AND oi.Situacion = 'A' 
					AND oi.FechaContrato > @FechaUltimaValoracion
					AND oi.FechaOperacion <= @p_FechaOperacion 
					AND oi.CodigoOperacion IN ('3','101')
					AND oi.Codigoportafoliosbs = @p_CodigoPortafolio
					AND oi.Estado in ('E-EJE', 'E-CON') 
					AND '' = (CASE WHEN @p_FechaOperacion < @FechaNegocio THEN '' ELSE ISNULL(oi.ordengenera,'') END)
					AND OI.TasaPorcentaje IS NOT NULL 
					AND OI.MontoNominalOrdenado IS NOT NULL
			END
		DECLARE 
			@CodigoNemonico VARCHAR(20)
			
		SET @intdpzMin = (SELECT MIN(Secuencia) FROM #tmpdpz)  
		SET @intdpzMax = (SELECT MAX(Secuencia) FROM #tmpdpz)  
		
		SELECT 
			@Secuencial = MAX(SecuenciaValorizacion) 
		FROM 
			CarteraTituloValoracion
			
		WHILE (@intdpzMin <= @intdpzMax)  
		BEGIN
			SELECT 
				@monto = montoprecancelar,
				@ptasa = TasaPorcentaje, 
				@vFechaOperacion = FechaOperacion,
				@fechacontrato = FechaContrato, 
				@nvcCodigoSBS = CodigoSBS,
				@valorTC = VectorTipoCambio, 
				@baset = CONVERT(NUMERIC(22,14), BaseTIR), 
				@TipoCalculo = TipoCalculo,
				@TipoCupon = TipoCupon,
				@CodigoNemonico = codigomnemonico
			FROM 
				#tmpdpz 
			WHERE 
				secuencia = @intdpzMin
				
			SET @difd1= DATEDIFF(DAY,CAST(@p_FechaOperacion AS VARCHAR(12)),CAST(@fechacontrato AS VARCHAR(12)))
			SET @difd= DATEDIFF(DAY,CAST(@vFechaOperacion AS VARCHAR(12)),CAST(@p_FechaOperacion AS VARCHAR(12)))
			
			IF @difd1 = 0
			BEGIN
				SET @ValorDPZLocal = 0
				SET @ValorDPZOrigen = 0
			END
			ELSE
			BEGIN
				--Valoriza de forma distinta para los DPZ con tasa Efectiva y Nominal
				IF @CodigoNemonico = 'DPZDOL365B' OR @CodigoNemonico = 'DPZNSOL365B'
				BEGIN
					--Valorizacion para los bisiestos
					IF @vFechaOperacion <> @p_FechaOperacion
					BEGIN
						EXEC DBO.SP_sit_CalculoDPZBisiesto 
														   @vFechaOperacion,
														   @p_FechaOperacion,
														   @monto,
														   @ptasa,
														   @TipoCupon,
														   @MontoCancelacion OUT
														   
						SET @ValorDPZOrigen = CONVERT(NUMERIC(22,7),@MontoCancelacion)
					END
					ELSE
						SET @ValorDPZOrigen = @monto
				END
				ELSE IF @TipoCupon = '1'
	                SET @ValorDPZOrigen = ((@monto * ((@ptasa / 100) / @baset) * CAST(dbo.DiasAnx(@vFechaOperacion,@p_FechaOperacion ) AS NUMERIC(22,12))) + @monto)
				ELSE
					SET @ValorDPZOrigen = @monto * POWER((1 + (@ptasa / 100)), (CAST(dbo.DiasAnx(@vFechaOperacion,@p_FechaOperacion ) AS NUMERIC(22,12)) / @baset))
				
				--Tipo de cambio
				IF @TipoCalculo = 'I'
					SET @ValorDPZLocal = ROUND((@ValorDPZOrigen / @valorTC), 7, 1)
				ELSE
					SET @ValorDPZLocal =  ROUND((@ValorDPZOrigen * @valorTC), 7, 1)
			END
			INSERT INTO 
				CarteraTituloValoracion(
										[Escenario], 
										[CodigoPortafolioSBS],
										[FechaValoracion],
										[NumeroTitulo], 
										[ValorCausadoOriginal],
										[ValorCausadoLocal],
										[VPNOriginal], 
										[VPNLocal],
										[ValorUnidad],
										[Cantidad],
										[ValorNominalMonedaOrigen],
										[ValorNominalMonedaLocal],
										[MetodoValoracion], 
										[UsuarioCreacion], 
										[FechaCreacion],
										[HoraCreacion],
										[Host], 
										[NumeroAsiento], 
										[FechaAsiento], 
										[CodigoMnemonico],
										SecuenciaValorizacion,
										Situacion, 
										CodigoSBS,
										VPNMonedaFondo
									    )
			SELECT 
				@escenario, 
				@p_CodigoPortafolio, 
				@p_FechaOperacion,
				1,
				NULL,
				NULL,
				@ValorDPZOrigen, 
				@ValorDPZLocal,
				NULL,
				NULL,
				NULL,
				NULL,
				NULL,
				@p_usuario, 
				@p_fecha,
				@p_Hora, 
				@p_Host, 
				NULL,
				NULL,
				CodigoMnemonico, 
				@Secuencial+@intdpzMin,
				'A', 
				@nvcCodigoSBS,
				CASE WHEN @TipoCalculoPorttafolio = 'D' THEN @ValorDPZLocal /  @TipoCambioFonfdo ELSE  @ValorDPZLocal * @TipoCambioFonfdo END
			FROM 
				#tmpdpz 
			WHERE 
				Secuencia=@intdpzMin
				
			IF @@ERROR <> 0  
			BEGIN
				SET @p_error = 1
				GOTO Error  
			END
			SET @intdpzMin = @intdpzMin + 1
		END
		DROP TABLE #tmpdpz
	END
	INSERT INTO 
		ValoracionCartera 
	VALUES
		(
		 @p_FechaOperacion,
		 @p_CodigoPortafolio,
		 @escenario,
		 @p_usuario,
		 @p_fecha,
		 @p_Hora,
		 @p_Host
		)
	IF NOT EXISTS(SELECT 1 FROM CarteraTituloValoracion WHERE FechaValoracion = @p_FechaOperacion AND CodigoPortafolioSBS = @p_CodigoPortafolio) 
	BEGIN
		SELECT @Secuencial = MAX(SecuenciaValorizacion) FROM CarteraTituloValoracion
		INSERT INTO CarteraTituloValoracion 
											(
											   [SecuenciaValorizacion]
											  ,[Escenario]
											  ,[CodigoPortafolioSBS]
											  ,[CodigoMnemonico]
											  ,[FechaValoracion]
											  ,[NumeroTitulo]
											  ,[VPNOriginal]
											  ,[VPNLocal]
											  ,[UsuarioCreacion]
											  ,[FechaCreacion]
											  ,[HoraCreacion]
											  ,[Host]
											  ,[Situacion]
											  ,[CodigoSBS]
											  ,[VPNMonedaFondo]
											  ,[VPNValorContable]
											  ,[VPNValorContableMonedaFondo]
											 ) 
	  VALUES
			(
			 @Secuencial+1,
			 'REAL',
			 @p_CodigoPortafolio,
			 '',
			 @p_FechaOperacion,
			 1,
			 0,
			 0,
			 @p_usuario, 
			 @p_fecha,
			 @p_Hora, 
			 @p_Host,
			 'A',
			 '',
			 0,
			 0,
			 0
			)
	END
	--Actualiza el valor conTABLE 
	-- INI OT 9362
	UPDATE 
		CTV 
	SET 
		CTV.VPNValorConTABLE = (ISNULL(V.ValorUnitario,0) * ISNULL(Cantidad,0)),
		CTV.VPNValorConTABLEMonedaFondo = CASE WHEN MP.TipoCalculo = 'D' THEN CASE WHEN MV.TipoCalculo = 'D' 
																				        THEN (ISNULL(V.ValorUnitario,0) * ISNULL(Cantidad,0)) * VTCV.ValorPrimario 
																				        ELSE (ISNULL(V.ValorUnitario,0) * ISNULL(Cantidad,0)) / VTCV.ValorPrimario END --Moneda Soles 
																				/ VTCP.ValorPrimario
																		 ELSE CASE WHEN MV.TipoCalculo = 'D' 
																				   THEN	(ISNULL(V.ValorUnitario,0) * ISNULL(Cantidad,0)) * VTCV.ValorPrimario 
																			       ELSE (ISNULL(V.ValorUnitario,0) * ISNULL(Cantidad,0)) / VTCV.ValorPrimario END --Moneda Soles
																			   * VTCP.ValorPrimario 
																		 END
	FROM 
		CarteraTituloValoracion CTV
	JOIN 
		Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
	JOIN 
		Portafolio P ON P.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS
	JOIN 
		VectorTipoCambio VTCV ON VTCV.CodigoMoneda = V.CodigoMoneda 
								 AND VTCV.Fecha = CTV.FechaValoracion
								 AND VTCV.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,V.CodigoMoneda,@p_FechaOperacion)
	JOIN 
		MONEDA MV ON MV.CodigoMoneda = V.CodigoMoneda
	JOIN 
		VectorTipoCambio VTCP ON VTCP.CodigoMoneda = P.CodigoMoneda 
								 AND VTCP.Fecha = CTV.FechaValoracion
								 AND VTCP.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,P.CodigoMoneda,@p_FechaOperacion)
	JOIN 
		MONEDA MP ON MP.CodigoMoneda = P.CodigoMoneda
	WHERE 
		CTV.FechaValoracion = @p_FechaOperacion 
		AND CTV.CodigoPortafolioSBS = @p_CodigoPortafolio
	-- FIN OT 9362
	--Valoracion de Warrants
	EXEC sp_SIT_Gen_ValoracionWarrant 
									  @p_FechaOperacion,
									  @p_CodigoPortafolio,
									  @p_Usuario,
									  @p_Fecha
	--
	DROP TABLE #CarteraTituloValoracion
	Error:
	IF @p_error=1  
		ROLLBACK TRANSACTION  
	ELSE  
	BEGIN
		SET @p_error = 2  
		COMMIT TRANSACTION  
	END
SELECT @p_error
END

GO

GRANT EXECUTE ON [dbo].[ValorizacionCartera_Generar2]  TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Valorizacion_listar]    Script Date: 01/24/2019 10:14:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valorizacion_listar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Valorizacion_listar]
GO

---------------------------------------------------------------------------------------------------------------------
-- Objetivo: Obtiene lista de Portafolio x Valorizar.    
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 17/07/2018    
-- Modificado por: Diego    
-- Nro. Orden de Trabajo: 11450
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 06/08/2018    
-- Modificado por: Ian Pastor M.    
-- Nro. Orden de Trabajo: 11432    
-- Descripcion del cambio: Discriminar vector precio por su tipo de fuente: MANUL o PIP    
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 31/08/2018  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11547  
-- Descripcion del cambio: Se agrega Filtro de carga de archivo precio forward   
---------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion: 31/08/2018
-- Modificado por: Ian Pastor Mendoza
-- Nro. Orden de Trabajo: 11590
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO
---------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion: 11/12/2018
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11698
-- Descripcion del cambio: Se agrega validación para portafolio que no tengan OI vigentes.
--------------------------------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion: 24/01/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11636    
-- Descripcion del cambio: Se adiciona validación por fecha de vencimiento en tipo renta variable y se implementa validación de portafolio 
--						   cuando se recién se ha creado sin negociaciones.
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

	DECLARE @CODIGOPORTAFOLIO VARCHAR(10)    
	DECLARE @PIP INT    
	DECLARE @CAJA INT     
	DECLARE @FECHA NUMERIC(8)    
	DECLARE @FECHAAYER DATETIME    
	DECLARE @CodigoMnemonico VARCHAR(15)    
	DECLARE @COUNT INT    
	DECLARE @FILA INT = 1    
	DECLARE @COUNTNEMONICO INT    
	DECLARE @COUNTVL INT    
	DECLARE @FERIADO INT    
	DECLARE @FINSEMANA INT    
	DECLARE @VectorPrecioVal VARCHAR(4)    
	DECLARE @COUNTVPF INT  
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
	END    
	ELSE    
	BEGIN    
		SET @COUNT = (SELECT COUNT(id) FROM #valorizacion)

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


USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReporteGestion_VencimientosOperaciones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReporteGestion_VencimientosOperaciones]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: GENERAR REPORTE DE VENCIMIENTOS DE OPERACIONES
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 04/09/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11590
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 18/12/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Se incluye los vencimientos de depósitos a plazo y operaciones de reporte
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 30/01/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11636
-- Descripcion del cambio: Cambio de denominación de moneda (Formato ISO)
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ReporteGestion_VencimientosOperaciones]-- '2777,2666',20190101,20190118
(
	@p_CodigoPortafolio VARCHAR(100)
	,@p_FechaInicio NUMERIC(8)
	,@p_FechaFin NUMERIC(8)
)
AS
BEGIN
	DECLARE @tPortafolio TABLE(
		Id INT IDENTITY
		,CodigoPortafolio VARCHAR(10)
	)
	
	DECLARE @tVencimientos TABLE(
		Id INT IDENTITY
		,FechaInicioCupon NUMERIC(8)
		,CodigoMoneda VARCHAR(5)
		,MontoInicial NUMERIC(22,7)
		,TasaCupon NUMERIC(22,7)
		,FechaVencimientoCupon NUMERIC(8)
		,MontoVencimiento NUMERIC(22,7)
		,Institucion_Instrumento VARCHAR(100)
		,FechaEmision NUMERIC(8)
		,TipoInstrumento VARCHAR(50)
		,DescripcionPortafolio VARCHAR(100)
		,Comentario VARCHAR(150)
		,CodigoOrden VARCHAR(12)
	)
	
	INSERT INTO @tPortafolio
	SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,',')
	
	DECLARE @iMin INT, @iMax INT, @CodigoPortafolio VARCHAR(10), @fechaNegocio NUMERIC(8)
	SET @iMin = (SELECT MIN(Id) FROM @tPortafolio)
	SET @iMax = (SELECT MAX(Id) FROM @tPortafolio)
	
	WHILE @iMin <= @iMax
	BEGIN
		SET @CodigoPortafolio = (SELECT CodigoPortafolio FROM @tPortafolio WHERE Id = @iMin)
		SET @fechaNegocio = (SELECT FechaNegocio FROM Portafolio WHERE CodigoPortafolioSBS = @CodigoPortafolio)
		--EXEC usp_Vencimiento_Generar_Rango @CodigoPortafolio, @p_FechaInicio, @p_FechaFin, @fechaNegocio
		EXEC ReporteGestion_GenerarVencimientosOperaciones @CodigoPortafolio, @p_FechaInicio, @p_FechaFin, @fechaNegocio
		
		INSERT INTO @tVencimientos
		SELECT ISNULL(OI.FechaLiquidacion,IV.FechaOperacion),IV.CodigoMoneda,IV.MontoOperacion,IV.TasaPorcentaje,IV.FechaContrato
			,IV.MontoOperacion,IV.CodigoMnemonico,V.FechaEmision, TI.Descripcion, P.Descripcion, '',IV.CodigoOrden
		FROM InstrumentosVencidosTemporal IV
			INNER JOIN Valores V ON IV.CodigoMnemonico = V.CodigoNemonico
			INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
			INNER JOIN Portafolio P ON IV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
			LEFT JOIN OrdenInversion OI ON IV.CodigoOrden = OI.CodigoOrden AND IV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		WHERE IV.CodigoPortafolioSBS = @CodigoPortafolio
		
		DELETE FROM InstrumentosVencidosTemporal
		
		SET @iMin = @iMin + 1
	END
	
	/*DEPÓSITOS A PLAZO*/
	INSERT INTO @tVencimientos
	SELECT
		OI.FechaOperacion,OI.CodigoMoneda,ISNULL(OI.MontoNominalOperacion,OI.MontoNominalOrdenado),ISNULL(OI.TasaPorcentaje,0) AS TasaPorcentaje,OI.FechaOperacion
		,OI.MontoOperacion,OI.CodigoMnemonico + ' - ' + T.Descripcion,OI.FechaContrato,TI.Descripcion,P.Descripcion,'',OI.CodigoOrden
	FROM OrdenInversion OI
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN Operacion O ON OI.CodigoOperacion = O.CodigoOperacion
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND OI.FechaOperacion BETWEEN @p_FechaInicio AND @p_FechaFin
		--AND OI.CategoriaInstrumento IN ('DP','OR') AND OI.Estado IN ('E-EJE','E-CON')
		AND OI.CodigoOperacion IN ('4','6') AND OI.Estado IN ('E-EJE','E-CON')
	
	SELECT Id,FechaInicioCupon,M.SinonimoISO AS CodigoMoneda,MontoInicial,TasaCupon,FechaVencimientoCupon,MontoVencimiento
		,Institucion_Instrumento,FechaEmision,TipoInstrumento,DescripcionPortafolio,Comentario,CodigoOrden
	FROM @tVencimientos TV
		INNER JOIN Moneda M ON TV.CodigoMoneda = M.CodigoMoneda
	ORDER BY 2,11
	
	----DETALLE OPERACIONES
	--SELECT OI.CodigoPortafolioSBS, OI.CodigoOrden, OI.FechaOperacion, TI.CodigoTipoInstrumento AS TipoInstrumento, OI.CodigoMnemonico
	--	,OI.CodigoISIN, '' AS NroDeposito, V.CodigoMoneda, v.BaseInteresCorrido AS BaseCuponAnual, OI.YTM
	--	,OI.MontoNominalOrdenado,((V.ValorUnitario * OI.CantidadOperacion) * dbo.CalcularSumaAmortizacionPendiente(OI.FechaOperacion,V.CodigoNemonico)/100) AS ValorNominalResidual
	--	,OI.CantidadOperacion, OI.MontoNetoOperacion - OI.TotalComisiones AS ValorPagado, OI.TotalComisiones
	--	,dbo.RetornarMontoComisionPorTipo(OI.CodigoOrden,OI.CodigoPortafolioSBS,'IGV') AS IGV, OI.MontoNetoOperacion, OI.PrecioNegociacionSucio
	--	,O.Descripcion AS DescripcionOperacion, V.FechaVencimiento, P.Descripcion AS NombrePortafolio, '' AS Observacion
	--FROM OrdenInversion OI
	--	INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico
	--	INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
	--	INNER JOIN Operacion O ON OI.CodigoOperacion = O.CodigoOperacion
	--	INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
	--WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
	--	AND OI.FechaOperacion BETWEEN @p_FechaInicio AND @p_FechaFin
	--	AND O.CodigoOperacion IN ('1','2')
END


GO

GRANT EXECUTE ON [dbo].[ReporteGestion_VencimientosOperaciones] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReporteGestion_OperacionesVencimientosOTC]') AND type in (N'P', N'PC'))
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
-- Fecha Modificación: 30/01/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11636
-- Descripcion del cambio: Cambio de denominación de moneda (Formato ISO)
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
		,OI.CodigoISIN, OI.CodigoMnemonico AS NroDeposito, M.SinonimoISO AS CodigoMoneda, v.BaseInteresCorrido AS BaseCuponAnual, ISNULL(OI.YTM,0) AS YTM
		,OI.MontoNominalOrdenado
		--,((V.ValorUnitario * OI.CantidadOperacion) * dbo.CalcularSumaAmortizacionPendiente(OI.FechaOperacion,V.CodigoNemonico)/100) AS ValorNominalResidual
		,ISNULL(CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_SaldoNominalVigente
			WHEN 'A_VENCI' THEN VA.TIRCOM_SaldoNominalVigente
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_SaldoNominalVigente
		END,0) AS ValorNominalResidual
		,ISNULL(OI.CantidadOperacion,0) AS CantidadOperacion, ISNULL(OI.MontoNetoOperacion - OI.TotalComisiones,0) AS ValorPagado
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
	
	/*DEPÓSITOS A PLAZO*/
	SELECT
		OI.CodigoPortafolioSBS, OI.CodigoOrden, OI.FechaOperacion, TI.CodigoTipoInstrumento AS TipoInstrumento, OI.CodigoMnemonico
		,(CASE WHEN LEN(OI.CodigoISIN) >= 12 THEN OI.CodigoISIN ELSE '' END) AS CodigoISIN
		,(CASE WHEN OI.CategoriaInstrumento = 'DP' THEN OI.CodigoMnemonico + ' - ' + T.Descripcion ELSE OI.CodigoMnemonico END) AS NroDeposito, M.SinonimoISO AS CodigoMoneda, v.BaseInteresCorrido AS BaseCuponAnual,OI.TasaPorcentaje
		,OI.MontoNominalOrdenado
		,OI.MontoNominalOrdenado AS ValorNominalResidual
		,ISNULL(OI.CantidadOperacion,0) AS CantidadOperacion, OI.MontoNominalOrdenado AS ValorPagado
		,ISNULL(OI.TotalComisiones,0) AS TotalComisiones
		,dbo.RetornarMontoComisionPorTipo(OI.CodigoOrden,OI.CodigoPortafolioSBS,'IGV') AS IGV, ISNULL(OI.MontoNetoOperacion,0) AS MontoNetoOperacion
		,100 AS PrecioNegociacionSucio
		,O.Descripcion AS DescripcionOperacion, OI.FechaContrato AS FechaVencimiento, P.Descripcion AS NombrePortafolio, '' AS Observacion, OI.FechaLiquidacion
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


USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerReporteVL]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerReporteVL]
GO

-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 15-12-2016
-- Modificado por: Everth Martinez
-- Nro. Orden de Trabajo: 9682
-- Descripcion del cambio: Se agrego una columna para saber la clase de instrumento
-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 29/01/2019    
-- Modificado por: Ernesto Galarza    
-- Nro. Orden de Trabajo: 11636   
-- Descripcion del cambio: Se agrego condicion en columna Calculo100 para considerar la clase de instrumentos facturas negociables (FA) como decimal.  
---------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[ObtenerReporteVL] @Fecha numeric(8), @p_Privados Numeric(1)
AS
BEGIN
	DECLARE @Correlativo INT
	SELECT @Correlativo = Correlativo FROM CorrelativoVL WHERE Fecha = @Fecha 
	SELECT RVL.TipoRegistro,RVL.Administradora,CASE WHEN RVL.Fondo = '' THEN P.Descripcion  ELSE RVL.Fondo  END AS Fondo ,RVL.Fecha,RVL.TipoCodigoValor,RVL.CodigoValor,
	RVL.IdentificadorOperacion,RVL.FormaValorizacion,ROUND(RVL.MontoNominal,2) MontoNominal ,
	ROUND(RVL.PrecioTasa,6) PrecioTasa ,RVL.TipoCambio,RVL.MontoFinal,CASE WHEN RVL.MontoInversion = 0 AND RVL.FormaValorizacion <> '5' THEN 0.01 ELSE RVL.MontoInversion END MontoInversion,
	RVL.FechaOperacion,RVL.FechaInicioPagaIntereses,RVL.FechaVencimiento,RVL.InteresesCorrido,
	RVL.InteresesGanado,RVL.Ganancia_Perdida,RVL.Valorizacion,RVL.TipoInstrumento,RVL.Clasificacion,RVL.ComisionContado,RVL.Comisionplazo,RVL.TIR,RVL.Duracion,
	RVL.CodigoNemonico,RVL.DescripcionTipoInstrumento,RVL.CodigoTercero, @Correlativo 'Correlativo',ISNULL(RVL.Moneda,'') Moneda,
	CASE WHEN CI.Categoria IN ('BO','PC','CD','FA') THEN 'SI' ELSE 'NO' END AS Calculo100
	FROM ReporteVL RVL
	JOIN Portafolio P ON P.CodigoPortafolioSBS  = RVL.CodigoPortafolioSBS AND P.IndicadorFondo = CASE WHEN @p_Privados = 1 THEN 'N' ELSE 'S' END
	LEFT JOIN  Valores V ON V.CodigoISIN = RVL.CodigoValor 
	/*OT9682 INICIO */
	LEFT JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS 
	LEFT JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento 
	/*OT9682 FIN */
	WHERE RVL.Fecha = @Fecha AND RVL.ImprimeVL = '1' ORDER BY Fondo 
END
GO

GRANT EXECUTE ON [dbo].[ObtenerReporteVL] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Pr_Sit_listarValorCuota]    Script Date: 01/29/2019 18:01:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pr_Sit_listarValorCuota]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 29/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se amplia ambito de variable portafolio a VARCHAR(50).
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Pr_Sit_listarValorCuota](
	@p_codigoPortafolio VARCHAR(20),
	@p_fechaInicial NUMERIC(8),
	@p_fechaFinal NUMERIC(8)
)
AS
BEGIN
	DECLARE @Tbl AS TABLE(
		portafolio VARCHAR(40),FechaProceso Char(10),Serie VARCHAR(20), CuotasEmitidas NUMERIC(22,7),ImporteRecaudado NUMERIC(22,7),rescates NUMERIC(22,7),
		valorCuota NUMERIC(22,7),comisionAdministradora	NUMERIC(22,7), totalInversiones NUMERIC(22,7),Caja NUMERIC(22,7),cxppre NUMERIC(22,7),cxc NUMERIC(22,7),
		cxppreImporteRecaudado	NUMERIC(22,7), patrimonioPreCierre NUMERIC(22,7),patrimonioCierre NUMERIC(22,7), cxpcierre NUMERIC(22,7),OtrosGastos NUMERIC(22,7),
		OtrosIngresos NUMERIC(22,7), Fecha NUMERIC(8) 
	)
	INSERT INTO @Tbl
	--Lista los portafolios no seriados
	SELECT portafolio = p.Descripcion,FechaProceso = dbo.FN_SIT_OBT_FechaFormateada(vc.FechaProceso),Serie = '',CuotasEmitidas = vc.ValCuotaCierre,ImporteRecaudado = vc.AportesValores,
	rescates = vc.RescateValores,valorCuota = vc.ValCuotaValoresCierre,comisionAdministradora = vc.ComisionSAFM,totalInversiones = vc.InversionesSubTotal,
	vc.Caja,cxppre = VC.CXPCompraTitulo + vc.OtrasCXP,cxc = vc.CXCVentaTitulo + VC.OtrasCXC,
	cxppreImporteRecaudado = vc.OtrasCXP - vc.AportesValores,
	patrimonioPreCierre = vc.ValPatriPreCierre1,patrimonioCierre = vc.ValPatriCierreValores,cxpcierre = vc.OtrasCXPCierre,vc.OtrosGastos,vc.OtrosIngresos, VC.FechaProceso 
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	WHERE (vc.CodigoPortafolioSBS = @p_codigoPortafolio or @p_codigoPortafolio = '') AND vc.FechaProceso between @p_fechaInicial and @p_fechaFinal AND p.PorSerie = 'N'
	UNION ALL
	--Lista los portafolios seriados
	SELECT portafolio = p.Descripcion,FechaProceso = dbo.FN_SIT_OBT_FechaFormateada(vc.FechaProceso),Serie = ISNULL(PS.NombreSerie,''),CuotasEmitidas = vc.ValCuotaCierre,ImporteRecaudado = vc.AportesValores,
	rescates = vc.RescateValores,valorCuota = vc.ValCuotaValoresCierre,comisionAdministradora = vc.ComisionSAFM,totalInversiones = vc.InversionesSubTotal,
	vc.Caja,cxppre = VC.CXPCompraTitulo + vc.OtrasCXP,cxc = vc.CXCVentaTitulo + VC.OtrasCXC,
	cxppreImporteRecaudado = vc.OtrasCXP - vc.AportesValores,
	patrimonioPreCierre = vc.ValPatriPreCierre1,patrimonioCierre = vc.ValPatriCierreValores,cxpcierre = vc.OtrasCXPCierre,vc.OtrosGastos,vc.OtrosIngresos, VC.FechaProceso 
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN PortafolioSerie PS ON PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS AND VC.CodigoSerie = PS.CodigoSerie
	WHERE (vc.CodigoPortafolioSBS = @p_codigoPortafolio or @p_codigoPortafolio = '') AND vc.FechaProceso between @p_fechaInicial and @p_fechaFinal AND p.PorSerie = 'S'
	SELECT portafolio,FechaProceso,Serie,CuotasEmitidas,ImporteRecaudado,rescates,valorCuota,comisionAdministradora,totalInversiones,Caja,cxppre,cxc,
	cxppreImporteRecaudado, patrimonioPreCierre,patrimonioCierre,cxpcierre,OtrosGastos,	OtrosIngresos FROM @Tbl ORDER BY  Fecha 
	--ORDER BY  vc.FechaProceso
END

GO

GRANT EXECUTE ON [dbo].[Pr_Sit_listarValorCuota] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]    Script Date: 01/29/2019 17:54:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 29/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se amplia ambito de variable portafolio a VARCHAR(50).
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado](
	@p_codigoPortafolio VARCHAR(20),
	@p_fechaInicial NUMERIC(8),
	@p_fechaFinal NUMERIC(8)
)
AS
BEGIN
	DECLARE @tTotalValorCuota AS table(	portafolio VARCHAR(50),fechaProceso NUMERIC(8),serie VARCHAR(20),cuotasEmitidas NUMERIC(22,7),valorCuota NUMERIC(22,7),
	comisionAdministradora NUMERIC(22,7),rescates NUMERIC(22,7),totalInversiones NUMERIC(22,7),caja NUMERIC(22,7),cxppre NUMERIC(22,7),cxc NUMERIC(22,7), Fecha NUMERIC(8) )
	
	DECLARE @tbl AS TABLE (portafolio VARCHAR(50), fechaProceso CHAR(10), serie VARCHAR(20),cuotasEmitidas NUMERIC(22,7),valorCuota NUMERIC(22,7),
	comisionAdministradora NUMERIC(22,7),rescates NUMERIC(22,7),totalInversiones NUMERIC(22,7),caja NUMERIC(22,7),cxppre NUMERIC(22,7),cxc NUMERIC(22,7), Fecha NUMERIC(8) )
	
	INSERT INTO @tTotalValorCuota
	SELECT Portafolio = p.Descripcion,vc.FechaProceso,Serie = PS.NombreSerie,CuotasEmitidas = vc.ValCuotaCierre,valorCuota = ValCuotaValoresCierre,
	comisionAdministradora = ComisionSAFM,rescates = RescateValores,totalInversiones = vc.InversionesSubTotal,Caja = Caja,cxppre = OtrasCXP,cxc = CXCVentaTitulo + OtrasCXC,
	vc.FechaProceso
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN PortafolioSerie PS ON PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS AND VC.CodigoSerie = PS.CodigoSerie 
	WHERE (vc.CodigoPortafolioSBS = @p_codigoPortafolio or @p_codigoPortafolio = '') and vc.FechaProceso between @p_fechaInicial and @p_fechaFinal
	and p.PorSerie = 'S' and p.Situacion = 'A' ORDER BY  VC.FechaProceso
	
	SELECT portafolio,fechaProceso = dbo.FN_SIT_OBT_FechaFormateada(fechaProceso),serie,cuotasEmitidas,valorCuota,comisionAdministradora,rescates,
	totalInversiones,caja,cxppre,cxc
	FROM @tTotalValorCuota
	ORDER BY  portafolio,Fecha,serie
	
	SELECT portafolio,fechaProceso = dbo.FN_SIT_OBT_FechaFormateada(fechaProceso),totalInversiones = sum(totalInversiones),caja = sum(caja),cxppre = sum(cxppre),cxc = sum(cxc)
	FROM @tTotalValorCuota
	GROUP BY portafolio, fechaProceso
END
GO



GRANT EXECUTE ON [dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenerarReporteVL_Inconsistencias]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GenerarReporteVL_Inconsistencias]
GO

---------------------------------------------------------------------------------------------------------------------    
-- Objetivo: Obtiene las Inconsistencias    
---------------------------------------------------------------------------------------------------------------------    
-- Fecha Modificación: 17/07/2018    
-- Modificado por: Diego    
-- Nro. Orden de Trabajo: 11450    
-- Descripcion del cambio: Nuevo    
---------------------------------------------------------------------------------------------------------------------   
-- Fecha Modificación: 15/10/2018    
-- Modificado por: Ricardo Colonia    
-- Nro. Orden de Trabajo: 11547    
-- Descripcion del cambio: Se modifica filtro de rango de inconsistencia PxQ.  
---------------------------------------------------------------------------------------------------------------------    
-- Fecha Modificación: 21/01/2019    
-- Modificado por: Ricardo Colonia    
-- Nro. Orden de Trabajo: 11687   
-- Descripcion del cambio: Se agrega filtro para considerar SWAP.  
---------------------------------------------------------------------------------------------------------------------   
-- Fecha Modificación: 29/01/2019    
-- Modificado por: Ernesto Galarza    
-- Nro. Orden de Trabajo: 11636   
-- Descripcion del cambio: Se agrego condicion en columna PxQ para considerar la clase de instrumentos facturas negociables (FA) como decimal.  
---------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[GenerarReporteVL_Inconsistencias]
(    
 @p_Portafolio varchar(250),    
 @p_fecha NUMERIC(8)    
)    
AS    
BEGIN    
 IF @p_Portafolio = ''    
 BEGIN    
 SET @p_Portafolio = '0'    
 END    
    
 DECLARE @p_Privados Numeric(1)    
    
 CREATE TABLE #lstPortafolio    
 (    
 id int identity(1,1),    
 CodigoPortafolioSBS varchar(20)    
 )    
    
 CREATE TABLE #TMP_ReporteVL    
 (    
 id INT IDENTITY(1,1),    
 CodigoPortafolioSBS varchar(20),    
 TipoRegistro [varchar](1) NULL,    
 Administradora [varchar](6) NULL,    
 Fondo [varchar](250) NULL,    
 Fecha [numeric](8, 0) NULL,    
 TipoCodigoValor [varchar](1) NULL,    
 CodigoValor [varchar](12) NULL,    
 IdentificadorOperacion [varchar](15) NULL,    
 FormaValorizacion [varchar](1) NULL,    
 MontoNominal [decimal](22, 7) NULL,    
 PrecioTasa [decimal](22, 7) NULL,    
 TipoCambio [numeric](22, 8) NULL,    
 MontoFinal [decimal](22, 7) NULL,    
 MontoInversion [decimal](22, 7) NULL,    
 FechaOperacion [numeric](8, 0) NULL,    
 FechaInicioPagaIntereses [numeric](8, 0) NULL,    
 FechaVencimiento [numeric](8, 0) NULL,    
 InteresesCorrido [decimal](22, 7) NULL,    
 InteresesGanado [decimal](22, 7) NULL,    
 Ganancia_Perdida [decimal](22, 7) NULL,    
 Ganacia_perdida_T_1 [decimal](22, 7) NULL,    
 Valorizacion [decimal](22, 7) NULL,    
 TipoInstrumento [varchar](1) NULL,    
 Clasificacion [varchar](40) NULL,    
 ComisionContado [decimal](22, 7) NULL,    
 ComisionPlazo [decimal](22, 7) NULL,    
 TIR [decimal](22, 7) NULL,    
 Duracion [numeric](22, 7) NULL,    
 CodigoNemonico [varchar](20) NULL,    
 DescripcionTipoInstrumento [varchar](100) NULL,    
 Moneda [varchar](20) NULL,    
 Validador [decimal](22, 7) NULL,    
 [Diferencia PXQ]  [decimal](22, 7) NULL,    
 )    
    
 CREATE TABLE #TMP_ReporteRango    
 (    
 Portafolio varchar(20),    
 CONT_PXQ INT,     
 CONT_GANADO INT,    
 CONT_VARIACION INT,    
 CONT_INVERSIONNULA INT,    
 CONT_VALORIZACIONNULA INT    
 )    
    
 INSERT #lstPortafolio    
 SELECT * FROM dbo.fnSplitString(@p_Portafolio,',')     
    
 DECLARE @REGISTRO INT    
 SET @REGISTRO =  (SELECT COUNT(id) from #lstPortafolio)    
    
 DECLARE @Portafolio varchar(20)    
 DECLARE @CONTADOR INT = 1    
    
 WHILE (@CONTADOR <= @REGISTRO)    
 BEGIN    
    
  SET @Portafolio = (SELECT CodigoPortafolioSBS FROM #lstPortafolio WHERE id = @CONTADOR)    
    
  IF @Portafolio = '99' OR @Portafolio = '102' OR @Portafolio = '104' OR     
  @Portafolio = '106' OR @Portafolio = '27' OR @Portafolio = '31' OR @Portafolio = '10' OR    
  @Portafolio = '11' OR @Portafolio = '47'     
  BEGIN    
   SET @p_Privados = 1    
  END    
  ELSE    
  BEGIN    
   SET @p_Privados = 0    
  END    
    
    
  INSERT #TMP_ReporteVL    
  SELECT P.CodigoPortafolioSBS,RVL.TipoRegistro,    
  RVL.Administradora, CASE WHEN RVL.Fondo = '' THEN P.Descripcion + ' - ' + P.Descripcion  ELSE RVL.Fondo + ' - ' + P.Descripcion END AS Fondo,    
  RVL.Fecha,RVL.TipoCodigoValor,    
  RVL.CodigoValor,RVL.IDENTIFICADOROPERACION,    
  RVL.FORMAVALORIZACION,ROUND(RVL.MontoNominal,2) MontoNominal,    
  ROUND(RVL.PrecioTasa,6) PrecioTasa,RVL.TipoCambio,    
  RVL.MONTOFINAL,CASE WHEN RVL.MontoInversion = 0 AND RVL.FormaValorizacion <> '5' THEN 0.01 ELSE RVL.MontoInversion END MontoInversion,    
  RVL.FECHAOPERACION,RVL.FECHAINICIOPAGAINTERESES,    
  RVL.FECHAVENCIMIENTO,RVL.InteresesCorrido,RVL.InteresesGanado,    
  RVL.GANANCIA_PERDIDA,    
  NULL AS [Ganacia o perdida T-1],RVL.VALORIZACION,    
  RVL.TIPOINSTRUMENTO,RVL.CLASIFICACION,    
  RVL.COMISIONCONTADO,RVL.COMISIONPLAZO,    
  RVL.TIR,RVL.DURACION,RVL.CODIGONEMONICO,    
  RVL.DESCRIPCIONTIPOINSTRUMENTO,RVL.MONEDA,    
  CASE WHEN RVL.TipoInstrumento IN ('0') THEN    
   CASE WHEN CI.Categoria IN ('BO','PC','CD') THEN    
    CAST ( ROUND((RVL.MontoNominal * RVL.PrecioTasa / 100),2) AS VARCHAR)    
   ELSE    
     CAST ( ROUND((RVL.MontoNominal * RVL.PrecioTasa),2) AS VARCHAR)       
   END     
  ELSE    
   NULL    
  END AS Validador,    
   CASE WHEN RVL.TipoInstrumento IN ('0') THEN    
    CASE WHEN CI.Categoria IN ('BO','PC','CD','FA') THEN    
    CAST ( ROUND((RVL.MontoNominal * RVL.PrecioTasa) / 100 * RVL.TipoCambio - RVL.Valorizacion,2) AS VARCHAR)    
   ELSE    
    CAST ( ROUND((RVL.MontoNominal * RVL.PrecioTasa)  * RVL.TipoCambio - RVL.Valorizacion,2) AS VARCHAR)    
   END     
   ELSE    
   NULL    
   END AS [Diferencia PXQ]    
   FROM ReporteVL RVL    
   INNER JOIN Portafolio P ON P.CodigoPortafolioSBS  = RVL.CodigoPortafolioSBS     
   AND P.IndicadorFondo = CASE WHEN @p_Privados = 1 THEN 'N' ELSE 'S' END    
   LEFT JOIN  Valores V ON V.CodigoISIN = RVL.CodigoValor     
   LEFT JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS     
   LEFT JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento    
   WHERE  RVL.ImprimeVL = 1 AND     
   LTRIM(RTRIM(RVL.CodigoPortafolioSBS)) = LTRIM(RTRIM(@Portafolio)) AND RVL.Fecha = @p_fecha    
      
  PRINT @CONTADOR    
  PRINT  @Portafolio    
  PRINT @p_fecha    
  SET @CONTADOR = @CONTADOR + 1    
    
 END    
    
 ---Se obtiene las Inconsistencias    
 DECLARE @CONT_PXQ INT = 0    
 DECLARE @CONT_GANADO INT = 0    
 DECLARE @CONT_VARIACION INT = 0    
 DECLARE @CONT_INVERSIONNULA INT = 0    
 DECLARE @CONT_VALORIZACIONNULA INT = 0    
    
 DECLARE @DiferenciaPXQ decimal(22, 7)    
 DECLARE @InteresGanado decimal(22, 7)    
 DECLARE @Variacion decimal(22, 7)    
 DECLARE @InversionNula decimal(22, 7)    
 DECLARE @ValorizacionNula decimal(22, 7)    
    
 DECLARE @ID_VAR VARCHAR(200) = ''    
    
 SET @CONTADOR = 1    
 SET @REGISTRO =  (SELECT COUNT(id) from #lstPortafolio)    
    
 DECLARE @FILA INT    
 DECLARE @CANT_REPORTE INT    
 SET @CANT_REPORTE = (SELECT COUNT(id) FROM #TMP_ReporteVL)    
    
 WHILE (@CONTADOR <= @REGISTRO)    
 BEGIN    
    
   SET @Portafolio = ''    
   SET @Portafolio = (SELECT CodigoPortafolioSBS FROM #lstPortafolio WHERE id = @CONTADOR)    
       
   SET @FILA = 1    
   SET @CONT_PXQ = 0    
   SET @CONT_GANADO = 0    
   SET @CONT_VARIACION = 0    
   SET @CONT_INVERSIONNULA = 0    
   SET @CONT_VALORIZACIONNULA = 0    
    
   SET @CANT_REPORTE = (SELECT COUNT(id) FROM #TMP_ReporteVL WHERE CodigoPortafolioSBS = @Portafolio)    
    
   WHILE (@FILA <= @CANT_REPORTE)    
   BEGIN    
    
   SET @DiferenciaPXQ = (SELECT [Diferencia PXQ] FROM #TMP_ReporteVL WHERE ID= @FILA AND CodigoPortafolioSBS = @Portafolio)    
   IF @DiferenciaPXQ IS NOT NULL    
   BEGIN    
    IF @DiferenciaPXQ >= 7 OR @DiferenciaPXQ <= -7    
    BEGIN    
     SET @CONT_PXQ = @CONT_PXQ + 1    
    END    
   END    
    
   SET @InteresGanado = (SELECT InteresesGanado FROM #TMP_ReporteVL WHERE ID= @FILA AND CodigoPortafolioSBS = @Portafolio)    
   IF @InteresGanado IS NOT NULL    
   BEGIN    
    IF @InteresGanado < 0     
    BEGIN    
     SET @CONT_GANADO = @CONT_GANADO + 1    
    END    
   END    
    
   SET @Variacion = (SELECT CASE WHEN MontoInversion = 0 THEN 0 ELSE (Ganancia_Perdida / MontoInversion)*100 END FROM #TMP_ReporteVL     
   WHERE ID= @FILA AND CodigoPortafolioSBS = @Portafolio AND Ganancia_Perdida <> 0 AND LTRIM(RTRIM(DescripcionTipoInstrumento)) NOT IN ('FORWARD','SWAP'))    
   IF @Variacion = 0 OR (@Variacion >= 50 OR  @Variacion <= -50)    
   BEGIN     
     SET @CONT_VARIACION = @CONT_VARIACION + 1    
     SET @ID_VAR = @ID_VAR + CAST(@FILA AS varchar) + ','    
   END     
    
   SET @InversionNula = (SELECT ISNULL(MontoInversion,0)  FROM #TMP_ReporteVL WHERE ID= @FILA AND CodigoPortafolioSBS = @Portafolio AND LTRIM(RTRIM(DescripcionTipoInstrumento)) NOT IN ('FORWARD','SWAP'))    
   IF @InversionNula = 0    
   BEGIN    
    SET @CONT_INVERSIONNULA = @CONT_INVERSIONNULA + 1    
   END    
    
   SET @ValorizacionNula = (SELECT ISNULL(Valorizacion,0)  FROM #TMP_ReporteVL WHERE ID= @FILA AND CodigoPortafolioSBS = @Portafolio )    
   IF @ValorizacionNula = 0    
   BEGIN    
    SET @CONT_VALORIZACIONNULA = @CONT_VALORIZACIONNULA + 1    
   END    
    
    SET @FILA =  @FILA +1    
   END    
    
   INSERT INTO #TMP_ReporteRango    
   SELECT @Portafolio,@CONT_PXQ AS 'Diferencia PXQ',@CONT_GANADO AS 'Interés Ganado',@CONT_VARIACION AS 'Variación Ganancia/Pérdida',    
   @CONT_INVERSIONNULA AS 'Inversión Nula',@CONT_VALORIZACIONNULA AS 'Valorización Nula'    
    
  SET @CONTADOR =@CONTADOR +1    
 END     
    
 --Rango de Valores    
 SELECT Portafolio,CONT_PXQ AS 'Diferencia PXQ',CONT_GANADO AS 'Interés Ganado',CONT_VARIACION AS 'Variación Ganancia/Pérdida',    
 CONT_INVERSIONNULA AS 'Inversión Nula',CONT_VALORIZACIONNULA AS 'Valorización Nula' FROM #TMP_ReporteRango    
    
 ---PXQ    
 SELECT codigoPortafolioSBS,PXQ.TipoRegistro,PXQ.Administradora,PXQ.Fondo,CONVERT(VARCHAR(10),CONVERT(DATETIME, CONVERT(CHAR(8),PXQ.Fecha)),103) AS Fecha,    
 PXQ.TipoCodigoValor,    
 PXQ.CodigoValor,PXQ.IdentificadorOperacion,PXQ.FormaValorizacion,PXQ.MontoNominal,    
 PXQ.PrecioTasa,PXQ.TipoCambio,PXQ.MontoFinal,PXQ.MontoInversion,PXQ.FechaOperacion,PXQ.FechaInicioPagaIntereses,    
 PXQ.FechaVencimiento,PXQ.InteresesCorrido,PXQ.InteresesGanado,PXQ.Ganancia_Perdida,PXQ.Valorizacion,PXQ.TipoInstrumento,    
 PXQ.Clasificacion,PXQ.ComisionContado,PXQ.ComisionPlazo,PXQ.TIR,PXQ.Duracion,PXQ.CodigoNemonico,PXQ.DescripcionTipoInstrumento,    
 PXQ.Moneda,PXQ.Validador,PXQ.[Diferencia PXQ] FROM #TMP_ReporteVL PXQ    
 WHERE PXQ.[Diferencia PXQ] <= -7 OR PXQ.[Diferencia PXQ] >= 7    
    
 --Interés Ganado    
 SELECT codigoPortafolioSBS,IGANADO.TipoRegistro,IGANADO.Administradora,IGANADO.Fondo,CONVERT(VARCHAR(10),CONVERT(DATETIME, CONVERT(CHAR(8),IGANADO.Fecha)),103) AS Fecha,IGANADO.TipoCodigoValor,    
 IGANADO.CodigoValor,IGANADO.IdentificadorOperacion,IGANADO.FormaValorizacion,IGANADO.MontoNominal,    
 IGANADO.PrecioTasa,IGANADO.TipoCambio,IGANADO.MontoFinal,IGANADO.MontoInversion,IGANADO.FechaOperacion,IGANADO.FechaInicioPagaIntereses,    
 IGANADO.FechaVencimiento,IGANADO.InteresesCorrido,IGANADO.InteresesGanado,IGANADO.Ganancia_Perdida,IGANADO.Valorizacion,IGANADO.TipoInstrumento,    
 IGANADO.Clasificacion,IGANADO.ComisionContado,IGANADO.ComisionPlazo,IGANADO.TIR,IGANADO.Duracion,IGANADO.CodigoNemonico,    
 IGANADO.DescripcionTipoInstrumento,IGANADO.Moneda,IGANADO.Validador,IGANADO.[Diferencia PXQ] FROM #TMP_ReporteVL IGANADO    
 WHERE IGANADO.InteresesGanado < 0    
    
 --Variación Ganancia/Pérdida    
 CREATE TABLE #TMP_ID    
 (    
 id VARCHAR(20)    
 )    
    
 INSERT #TMP_ID    
 SELECT * FROM dbo.fnSplitString(@ID_VAR,',')     
    
 SELECT codigoPortafolioSBS,GPERDIDA.TipoRegistro,GPERDIDA.Administradora,GPERDIDA.Fondo,CONVERT(VARCHAR(10),CONVERT(DATETIME, CONVERT(CHAR(8),GPERDIDA.Fecha)),103) AS Fecha,    
 GPERDIDA.TipoCodigoValor,    
 GPERDIDA.CodigoValor,GPERDIDA.IdentificadorOperacion,GPERDIDA.FormaValorizacion,GPERDIDA.MontoNominal,    
 GPERDIDA.PrecioTasa,GPERDIDA.TipoCambio,GPERDIDA.MontoFinal,GPERDIDA.MontoInversion,GPERDIDA.FechaOperacion,GPERDIDA.FechaInicioPagaIntereses,    
 GPERDIDA.FechaVencimiento,GPERDIDA.InteresesCorrido,GPERDIDA.InteresesGanado,    
 GPERDIDA.Ganancia_Perdida,GPERDIDA.Ganacia_perdida_T_1 AS Ganacia_perdida_T_1,    
 GPERDIDA.Valorizacion,GPERDIDA.TipoInstrumento,GPERDIDA.Clasificacion,GPERDIDA.ComisionContado,GPERDIDA.ComisionPlazo,    
 GPERDIDA.TIR,GPERDIDA.Duracion,GPERDIDA.CodigoNemonico,GPERDIDA.DescripcionTipoInstrumento,GPERDIDA.Moneda,GPERDIDA.Validador,    
 GPERDIDA.[Diferencia PXQ] FROM #TMP_ReporteVL GPERDIDA    
 WHERE id IN (SELECT CAST(id AS INT) FROM #TMP_ID) AND LTRIM(RTRIM(GPERDIDA.DescripcionTipoInstrumento)) NOT IN ('FORWARD','SWAP')   
    
 --Inversión Nula    
 SELECT codigoPortafolioSBS,INULA.TipoRegistro,INULA.Administradora,INULA.Fondo,CONVERT(VARCHAR(10),CONVERT(DATETIME, CONVERT(CHAR(8),INULA.Fecha)),103) AS Fecha,    
 INULA.TipoCodigoValor,    
 INULA.CodigoValor,INULA.IdentificadorOperacion,INULA.FormaValorizacion,INULA.MontoNominal,    
 INULA.PrecioTasa,INULA.TipoCambio,INULA.MontoFinal,INULA.MontoInversion,INULA.FechaOperacion,INULA.FechaInicioPagaIntereses,    
 INULA.FechaVencimiento,INULA.InteresesCorrido,INULA.InteresesGanado,INULA.Ganancia_Perdida,INULA.Valorizacion,INULA.TipoInstrumento,    
 INULA.Clasificacion,INULA.ComisionContado,INULA.ComisionPlazo,INULA.TIR,INULA.Duracion,INULA.CodigoNemonico,    
 INULA.DescripcionTipoInstrumento,INULA.Moneda,INULA.Validador,INULA.[Diferencia PXQ] FROM #TMP_ReporteVL INULA    
 WHERE ISNULL(INULA.MontoInversion, 0) = 0 AND LTRIM(RTRIM(INULA.DescripcionTipoInstrumento)) NOT IN ('FORWARD','SWAP')  
    
 --Valorización Nula    
 SELECT codigoPortafolioSBS,VNULA.TipoRegistro,VNULA.Administradora,VNULA.Fondo, CONVERT(VARCHAR(10),CONVERT(DATETIME, CONVERT(CHAR(8),VNULA.Fecha)),103) AS Fecha,    
 VNULA.TipoCodigoValor,    
 VNULA.CodigoValor,VNULA.IdentificadorOperacion,VNULA.FormaValorizacion,VNULA.MontoNominal,    
 VNULA.PrecioTasa,VNULA.TipoCambio,VNULA.MontoFinal,VNULA.MontoInversion,VNULA.FechaOperacion,VNULA.FechaInicioPagaIntereses,    
 VNULA.FechaVencimiento,VNULA.InteresesCorrido,VNULA.InteresesGanado,VNULA.Ganancia_Perdida,VNULA.Valorizacion,VNULA.TipoInstrumento,    
 VNULA.Clasificacion,VNULA.ComisionContado,VNULA.ComisionPlazo,VNULA.TIR,VNULA.Duracion,VNULA.CodigoNemonico,    
 VNULA.DescripcionTipoInstrumento,VNULA.Moneda,VNULA.Validador,VNULA.[Diferencia PXQ] FROM #TMP_ReporteVL VNULA    
 WHERE ISNULL(VNULA.Valorizacion, 0) = 0    
    
    
 DROP TABLE #TMP_ReporteRango    
 DROP TABLE #TMP_ReporteVL    
 DROP TABLE #lstPortafolio    
 DROP TABLE #TMP_ID    
END 
GO

GRANT EXECUTE ON [dbo].[GenerarReporteVL_Inconsistencias] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[sp_SIT_Gen_Carta_OperacionCambio]    Script Date: 01/28/2019 16:40:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_Gen_Carta_OperacionCambio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionCambio]
GO

-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/01/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Modificación de redondeo de Tipo Cambio.
-----------------------------------------------------------------------------------------------------------
--exec sp_SIT_Gen_Carta_OperacionCambio '056094' --COMPRA 65
--exec sp_SIT_Gen_Carta_OperacionCambio '056095' --VENTA  66

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
	OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2
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

END;


GO


GRANT EXECUTE ON [dbo].[sp_SIT_Gen_Carta_OperacionCambio] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_Insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Valores_Insertar]
GO
----------------------------------------------------------------------------------------------------------
--Objetivo: Insertar registros en la tabla de valores
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/10/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9515
--	Descripcion del cambio: Insertar nuevo campo
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 22/05/2018
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11377
--	Descripcion del cambio: Insertar nuevo campo TipoRentaRiesgo, Frecuencia Dividendo, Base Interes Corrido
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2018-10-05
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregó el guardado Rating en Histórico
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se adiciono el campo de Subordinado
-------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 15/01/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Creacion de correlativos para establacer codigo nemonico para valores que sean facturas
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Valores_Insertar] (@p_CodigoNemonico varchar(15),@p_CodigoTipoTitulo varchar(15),@p_Descripcion  varchar(50),@p_Agrupacion  varchar(5),
	@p_CodigoEmisor varchar(4),@p_CodigoBursatilidad varchar(10),@p_CodigoMoneda varchar(10),@p_NumeroUnidades numeric(24,7),@p_CodigoISIN  varchar(12),
	@p_ValorUnitario numeric(22,7),@p_CodigoSBS  varchar(12),@p_ValorNominal numeric(22,7),@p_CodigoCalificacion varchar(10),@p_ValorEfectivoColocado numeric(22,7),
	@p_TasaEncaje numeric(8,2),@p_FechaEmision numeric(8),@p_CodigoTipoCupon varchar(4),@p_FechaVencimiento numeric(8),@p_TasaCupon  numeric(12,7),
	@p_FechaPrimerCupon numeric(8),@p_CodigoPeriodicidad varchar(4),@p_CodigoTipoAmortizacion varchar(4),@p_TasaSpread numeric(12,7),@p_CodigoIndicador varchar(50),
	@p_ValorIndicador numeric(22,7),@p_Situacion  varchar(1),@p_TirBase  numeric,@p_TirNDias  numeric,@p_CuponBase  numeric,@p_CuponNDias numeric,@p_Observacion varchar(35),
	@p_TipoRenta  varchar(10),@p_TipoCuponera varchar(10),@p_UsuarioCreacion varchar(15),@p_FechaCreacion numeric(8),@p_HoraCreacion varchar(10),@p_Host  varchar(20),
	@p_CodigoTipoInstrumentoSBS varchar(3),@p_NemonicoTemporal varchar(15),@p_CantidadIE  numeric(22,7),@p_RentaFijaIE  numeric(22,7),@p_RentaVarIE  numeric(22,7),
	@P_GrupoContable varchar(3),@p_Rescate  varchar(1),@p_MonedaCupon varchar(5),@p_MonedaPago varchar(5),@p_TipoRentaFija varchar(1)=null,
	@p_AplicaReduccionUnidades varchar(2),@p_MontoDividendo numeric(22,7),@p_Rating varchar(10),@p_TipoDerivado varchar(5),@p_CodigoTipoDerivado varchar(100),
	@p_FactorFlotante numeric(22,7),@p_SpreadFlotante numeric(22,7),@p_CodigoEmisorIE varchar(11),@p_PosicionAct numeric(22,7),@p_PorcPosicion numeric(22,12),
	@p_Categoria varchar(2),@p_CondicionImpuesto varchar(2),@p_MargenInicial numeric(22,7),@p_MargenMantenimiento numeric(22,7),@p_ContractSize	numeric(22,7),
	-- INICIO - 22/05/2018 - RCE - Cambio de tipo de dato de numeric a varchar
	@p_ICBase varchar(4),@p_ICNDias varchar(4)
	-- FIN - 22/05/2018 - RCE - Cambio de tipo de dato de numeric a varchar
	,@p_FirmaLlamado varchar(1),@p_Estilo varchar(3) = '',@p_GeneraInteres char(1),@p_TipoCodigoValor char(1),@P_CodigoMercado VarChar(3),
	@p_MontoPiso NUMERIC(22,7),@p_MontoTecho NUMERIC(22,7),@p_Garante VarChar(100),@p_Subyacente VarChar(100),@p_PrecioEjercicio NUMERIC(22,7),	@p_TamanoEmision NUMERIC(22,7),
	@p_CodigoPortafolioSBS VarChar(10)
	
	-- INICIO - 17/05/2018 - RCE - Nuevo Parametro para TipoRentaRiesgo y Frecuencia Pago Dividendo
	,@P_TipoRentaRiesgo VARCHAR(10), @P_CodigoFrecuenciaPago VARCHAR(4)
	-- FIN - 17/05/2018 - RCE - Nuevo Parametro para TipoRentaRiesgo y Frecuencia Pago Dividendo
	
	-- INICIO - 22/05/2018 - RCE - Nuevo Parametro para Estado de Base Interes Corrido
	,@P_EstadoBaseIC VARCHAR(1), @P_BaseInteresCorrido VARCHAR(4), @P_BaseInteresCorridoDias VARCHAR(4)
	-- FIN - 22/05/2018 - RCE - Nuevo Parametro para Estado de Base Interes Corrido
	-- INICIO - 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,	@p_Subordinado char(1)
	-- FIN - 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	-- INICIO - 26/12/2018 - EG - Nuevo Parametro para guardar el numero serie factura
	,	@p_CodigoFactura VARCHAR(30) =NULL
	-- FIN -  26/12/2018 - EG -
	,   @p_PrecioDevengado CHAR(1)
)
AS    
BEGIN
	SET NOCOUNT ON
	
	IF(ISNULL(@p_CodigoFactura,'') <>'')
	BEGIN
	--declare @p_CodigoNemonico varchar(15)
	
	DECLARE @ValorMaximo int
		if( (select count(1) from Valores WHERE CodigoFactura is not null) =0 )
		 set @ValorMaximo=1
		 else
	    set @ValorMaximo = (select MAX(SUBSTRING(isnull(CodigoNemonico,'F00000000000'), 2, 11))+1 from Valores WHERE CodigoFactura is not null)
	
	set @p_CodigoNemonico = (select 'F' + RIGHT('00000000000'+CAST(@ValorMaximo AS VARCHAR),11))
    set @p_CodigoISIN = 	@p_CodigoNemonico
	set @p_CodigoSBS = @p_CodigoISIN
	END
	

	--Inserta Valores
	INSERT INTO Valores (CodigoNemonico,CodigoTipoTitulo,Descripcion,Agrupacion,CodigoEmisor,CodigoBursatilidad,CodigoMoneda,NumeroUnidades,CodigoISIN,ValorUnitario,
	CodigoSBS,ValorNominal,CodigoCalificacion,ValorEfectivoColocado,TasaEncaje,FechaEmision,CodigoTipoCupon,FechaVencimiento,TasaCupon,FechaPrimerCupon,
	CodigoPeriodicidad,CodigoTipoAmortizacion,TasaSpread,CodigoIndicador,ValorIndicador,Situacion,BaseTir,BaseTirDias,BaseCupon, BaseCuponDias,Observacion,
	TipoRenta,TipoCuponera,UsuarioCreacion,FechaCreacion,HoraCreacion,Host,CodigoTipoInstrumentoSBS,NemonicoTemporal,CantidadIE,RentaFijaIE,RentaVarIE,GrupoContable,
	Rescate,MonedaCupon,MonedaPago,TipoRentaFija,AplicaReduccionUnidades,MontoDividendo,Rating,TipoDerivado,CodigoTipoDerivado,FactorFlotante,SpreadFlotante,CodigoEmisorIE,
	PosicionAct,PorcPosicion,Categoria,CondicionImpuesto,EstadoAp,FechaAprobacion,MargenInicial,MargenMantenimiento,ContractSize,BaseIC,BaseICDias,FirmaLlamado,
	Estilo,GeneraInteres,TipoCodigoValor, CodigoMercado,MontoPiso,MontoTecho,Garante,Subyacente,PrecioEjercicio,TamanoEmision,CodigoPortafolioSBS
	,TipoRentaRiesgo, CodigoFrecuenciaDividendo -- 17/05/18 - RCE - Nuevo Campo Tipo Renta Riesgo y Frecuencia Dividendo
	,EstadoBaseIC, BaseInteresCorrido, BaseInteresCorridoDias  -- 22/05/18 - RCE - Nuevo Campo Estado Base IC
	,Subordinado -- 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,CodigoFactura
	,PrecioDevengado
	)
	VALUES (UPPER(@p_CodigoNemonico),@p_CodigoTipoTitulo,@p_Descripcion,@p_Agrupacion,@p_CodigoEmisor,@p_CodigoBursatilidad,@p_CodigoMoneda,@p_NumeroUnidades,
	@p_CodigoISIN,@p_ValorUnitario,@p_CodigoSBS,@p_ValorNominal,@p_CodigoCalificacion,@p_ValorEfectivoColocado,@p_TasaEncaje,@p_FechaEmision,@p_CodigoTipoCupon,
	@p_FechaVencimiento,@p_TasaCupon,@p_FechaPrimerCupon,@p_CodigoPeriodicidad,@p_CodigoTipoAmortizacion,@p_TasaSpread,@p_CodigoIndicador,@p_ValorIndicador,
	@p_Situacion,@p_TirBase,@p_TirNDias,@p_CuponBase,@p_CuponNDias,@p_Observacion,@p_TipoRenta,@p_TipoCuponera,@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,
	@p_Host,@p_CodigoTipoInstrumentoSBS,@p_NemonicoTemporal,@p_CantidadIE,@p_RentaFijaIE,@p_RentaVarIE,@p_GrupoContable,@p_Rescate,@p_MonedaCupon,@p_MonedaPago,
	@p_TipoRentaFija,@p_AplicaReduccionUnidades,@p_MontoDividendo,@p_Rating,@p_TipoDerivado,@p_CodigoTipoDerivado,@p_FactorFlotante,@p_SpreadFlotante,@p_CodigoEmisorIE,
	@p_PosicionAct,@p_PorcPosicion,@p_Categoria,@p_CondicionImpuesto,
	CASE WHEN (@p_CodigoTipoInstrumentoSBS in ('64','79','69','60') OR (@p_CodigoTipoInstrumentoSBS = '08' and @p_CodigoEmisor = 'US-T' and @p_CodigoTipoCupon = '3'))
	THEN '2' ELSE '1' END,
	CASE WHEN (@p_CodigoTipoInstrumentoSBS in ('64','79','69','60') or (@p_CodigoTipoInstrumentoSBS = '08' and @p_CodigoEmisor = 'US-T' and @p_CodigoTipoCupon = '3'))
	THEN convert(numeric(8),convert(varchar,getdate(),112)) ELSE NULL END,@p_MargenInicial,@p_MargenMantenimiento,@p_ContractSize,@p_ICBase,@p_ICNDias,@p_FirmaLlamado,
	@p_Estilo,@p_GeneraInteres,@p_TipoCodigoValor,@P_CodigoMercado,@p_MontoPiso,@p_MontoTecho,@p_Garante,@p_Subyacente,@p_PrecioEjercicio,@p_TamanoEmision,@p_CodigoPortafolioSBS
	,@P_TipoRentaRiesgo, @P_CodigoFrecuenciaPago -- 17/05/18 - RCE - Nuevo Campo Tipo Renta Riesgo y Frecuencia Dividendo
	,@P_EstadoBaseIC, @P_BaseInteresCorrido, @P_BaseInteresCorridoDias -- 22/05/18 - RCE - Nuevo Campo Estado Base IC
	,@p_Subordinado -- 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,@p_CodigoFactura
	,@p_PrecioDevengado
	)  
	
		--Historia
	-- EXEC sp_SIT_InsertaRatingValorHistoria @p_CodigoISIN,@p_Rating,@p_UsuarioCreacion ,@p_FechaCreacion
	
	-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	delete from RatingValorHistoria where CodigoIsin = @p_CodigoISIN and FechaCreacion = @p_FechaCreacion
	
	INSERT INTO RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion)
	VALUES (@p_CodigoISIN, @p_Rating, @p_UsuarioCreacion, @p_FechaCreacion)
	-- FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
END


GO

GRANT EXECUTE ON [dbo].[Valores_Insertar] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Valores_Modificar]    Script Date: 01/24/2019 11:21:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_Modificar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Valores_Modificar]
GO


----------------------------------------------------------------------------------------------------------
--Objetivo: Moficar los datos de Valores
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/10/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9515
--	Descripcion del cambio: Modificar nuevo campo
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 16-11-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9577
--	Descripcion del cambio: Se inserta el Rating a la tabla historial
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 22/05/2018
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11377
--	Descripcion del cambio: Insertar nuevo campo TipoRentaRiesgo, Frecuencia Dividendo, Base Interes Corrido
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2018-10-05
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregó el guardado Rating en Histórico
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se adiciono el campo de Subordinado.
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 15/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se adicionó el campo de PrecioDevengado.
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 23/01/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se adicionó el campo de CodigoFactura
----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Valores_Modificar] (@p_CodigoNemonico varchar(15),@p_CodigoTipoTitulo varchar(15),@p_Descripcion  varchar(50),@p_Agrupacion  varchar(5),
	@p_CodigoEmisor varchar(4),@p_CodigoBursatilidad varchar(10),@p_CodigoMoneda varchar(10),@p_NumeroUnidades numeric(24,7),@p_CodigoISIN  varchar(12),
	@p_ValorUnitario numeric(22,7),@p_CodigoSBS  varchar(12),@p_ValorNominal numeric(22,7),@p_CodigoCalificacion varchar(10),@p_ValorEfectivoColocado numeric(22,7),
	@p_TasaEncaje numeric(8,2),@p_FechaEmision numeric(8),@p_CodigoTipoCupon varchar(4),@p_FechaVencimiento numeric(8),@p_TasaCupon  numeric(12,7),
	@p_FechaPrimerCupon numeric(8),@p_CodigoPeriodicidad varchar(4),@p_CodigoTipoAmortizacion varchar(4),@p_TasaSpread numeric(12,7),@p_CodigoIndicador varchar(50),
	@p_ValorIndicador numeric(22,7),@p_Situacion  varchar(1),@p_TirBase  numeric,@p_TirNDias  numeric,@p_CuponBase  numeric,@p_CuponNDias numeric,@p_Observacion varchar(35),
	@p_TipoRenta  varchar(10),@p_TipoCuponera varchar(10),@p_CodigoTipoInstrumentoSBS varchar(3),@p_NemonicoTemporal varchar(15),@p_UsuarioModificacion  varchar(15),
	@p_FechaModificacion  numeric(8),@p_HoraModificacion varchar(10),@p_Host  varchar(20),@p_CantidadIE  numeric(22,7),@p_RentaFijaIE  numeric(22,7),
	@p_RentaVarIE  numeric(22,7),@p_GrupoContable varchar(3),@p_Rescate  varchar(1),@p_MonedaCupon varchar(5),@p_MonedaPago varchar(5),@p_TipoRentaFija varchar(1)=null,
	@p_AplicaReduccionUnidades	varchar(2),@p_MontoDividendo numeric(22,7),@p_Rating varchar(10),@p_TipoDerivado varchar(5),@p_CodigoTipoDerivado varchar(100),
	@p_FactorFlotante numeric(22,7),@p_SpreadFlotante numeric(22,7),@p_CodigoEmisorIE varchar(11),@p_PosicionAct numeric(22,7),@p_PorcPosicion numeric(22,12),
	@p_Categoria varchar(2),@p_CondicionImpuesto varchar(2),@p_MargenInicial numeric(22,7),@p_MargenMantenimiento numeric(22,7),@p_ContractSize numeric(22,7),
	-- INICIO - 22/05/2018 - RCE - Cambio de tipo de dato de numeric a varchar
	@p_ICBase varchar(4),@p_ICNDias varchar(4)
	-- FIN - 22/05/2018 - RCE - Cambio de tipo de dato de numeric a varchar
	,@p_FirmaLlamado varchar(1),@p_Estilo varchar(5) = '',@p_GeneraInteres char(1) = '',@p_TipoCodigoValor char(1) = '',
	@P_CodigoMercado VarChar(3),@p_MontoPiso NUMERIC(22,7),@p_MontoTecho NUMERIC(22,7),@p_Garante VarChar(100),@p_Subyacente VarChar(100),@p_PrecioEjercicio NUMERIC(22,7),
	@p_TamanoEmision NUMERIC(22,7),@p_CodigoPortafolioSBS VarChar(10)
	-- INICIO - 17/05/2018 - RCE - Nuevo Parametro para TipoRentaRiesgo y Frecuencia Pago Dividendo
	,@P_TipoRentaRiesgo VARCHAR(10), @P_CodigoFrecuenciaPago VARCHAR(4)
	-- FIN - 17/05/2018 - RCE - Nuevo Parametro para TipoRentaRiesgo y Frecuencia Pago Dividendo

	-- INICIO - 22/05/2018 - RCE - Nuevo Parametro para Estado de Base Interes Corrido
	,@P_EstadoBaseIC VARCHAR(1), @P_BaseInteresCorrido VARCHAR(4), @P_BaseInteresCorridoDias VARCHAR(4)
	-- FIN - 22/05/2018 - RCE - Nuevo Parametro para Estado de Base Interes Corrido
		-- INICIO - 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,	@p_Subordinado char(1)
	-- FIN - 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,  @p_PrecioDevengado CHAR(1)
	-- INICIO - 23/01/2019 - EGC - Nuevo Parametro para CodigoFactura
	,@p_CodigoFactura varchar(30)=NULL
	-- FIN - 23/01/2019 - EGC - Nuevo Parametro para CodigoFactura
)
AS
BEGIN
	SET NOCOUNT ON
	--OT 9577 Inicio
	DECLARE @Rating varchar(10)

	SELECT @Rating = LTRIM(RTRIM(ISNULL(Rating, '')))
	FROM Valores
	WHERE CodigoNemonico = @p_CodigoNemonico
	
	--OT 9577 Fin

	--Modifica Valores
	UPDATE Valores SET TipoRenta = @p_TipoRenta,TipoCuponera = @p_TipoCuponera,CodigoTipoInstrumentoSBS = @p_CodigoTipoInstrumentoSBS,
	CodigoTipoTitulo = @p_CodigoTipoTitulo,Descripcion = @p_Descripcion,Agrupacion = @p_Agrupacion,CodigoEmisor = @p_CodigoEmisor,
	CodigoBursatilidad  = @p_CodigoBursatilidad,CodigoMoneda = @p_CodigoMoneda,NumeroUnidades = @p_NumeroUnidades,CodigoISIN = @p_CodigoISIN,
	ValorUnitario = @p_ValorUnitario,CodigoSBS = @p_CodigoSBS,ValorNominal = @p_ValorNominal,CodigoCalificacion = @p_CodigoCalificacion,
	ValorEfectivoColocado = @p_ValorEfectivoColocado,TasaEncaje = @p_TasaEncaje,FechaEmision = @p_FechaEmision,CodigoTipoCupon = @p_CodigoTipoCupon,
	FechaVencimiento = @p_FechaVencimiento,TasaCupon = @p_TasaCupon,FechaPrimerCupon = @p_FechaPrimerCupon,CodigoPeriodicidad = @p_CodigoPeriodicidad,
	CodigoTipoAmortizacion = @p_CodigoTipoAmortizacion,TasaSpread = @p_TasaSpread,CodigoIndicador = @p_CodigoIndicador,ValorIndicador = @p_ValorIndicador,
	Situacion = @p_Situacion,BaseTir = @p_TirBase,BaseTirDias = @p_TirNDias,BaseCupon = @p_CuponBase,BaseCuponDias = @p_CuponNDias,Observacion = @p_Observacion,
	NemonicoTemporal = @p_NemonicoTemporal,UsuarioModificacion = @p_UsuarioModificacion,FechaModificacion = @p_FechaModificacion,HoraModificacion = @p_HoraModificacion,
	Host = @p_Host ,CantidadIE = @p_CantidadIE,RentaFijaIE  = @p_RentaFijaIE,RentaVarIE = @p_RentaVarIE,GrupoContable = @p_GrupoContable,Rescate = @p_Rescate,
	MonedaCupon = @p_MonedaCupon,MonedaPago = @p_MonedaPago,TipoRentaFija= @p_TipoRentaFija,AplicaReduccionUnidades = @p_AplicaReduccionUnidades,
	MontoDividendo = @p_MontoDividendo,Rating = @p_Rating,TipoDerivado = @p_TipoDerivado,CodigoTipoDerivado = @p_CodigoTipoDerivado,FactorFlotante = @p_FactorFlotante,
	SpreadFlotante = @p_SpreadFlotante,CodigoEmisorIE = @p_CodigoEmisorIE,PosicionAct = @p_PosicionAct,PorcPosicion = @p_PorcPosicion,Categoria = @p_Categoria,
	CondicionImpuesto = @p_CondicionImpuesto,EstadoAp = case when @p_Situacion = 'A' and FechaAprobacion is not null then '2' else EstadoAp end,
	MargenInicial = @p_MargenInicial,MargenMantenimiento = @p_MargenMantenimiento,ContractSize = @p_ContractSize,BaseIC = @p_ICBase,BaseICDias = @p_ICNDias,
	FirmaLlamado = @p_FirmaLlamado,Estilo = @p_Estilo,GeneraInteres = @p_GeneraInteres,TipoCodigoValor = @p_TipoCodigoValor, CodigoMercado = @P_CodigoMercado,
	MontoPiso = @p_MontoPiso,MontoTecho = @p_MontoTecho,Garante = @p_Garante,Subyacente = @p_Subyacente,PrecioEjercicio = @p_PrecioEjercicio,
	TamanoEmision = @p_TamanoEmision, CodigoPortafolioSBS = @p_CodigoPortafolioSBS --9515
	,TipoRentaRiesgo=@P_TipoRentaRiesgo, CodigoFrecuenciaDividendo=@P_CodigoFrecuenciaPago -- 17/05/18 - RCE - Nuevo Campo Tipo Renta Riesgo y Frecuencia Dividendo
	,EstadoBaseIC=@P_EstadoBaseIC, BaseInteresCorrido = @P_BaseInteresCorrido,  BaseInteresCorridoDias = @P_BaseInteresCorridoDias -- 22/05/18 - RCE - Nuevo Campo Estado Base Interés Corrido
	,Subordinado = @p_Subordinado -- 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,PrecioDevengado =  @p_PrecioDevengado 
	WHERE  CodigoNemonico = @p_CodigoNemonico
	--VL Codigo Valor
	UPDATE ReporteVL SET TipoCodigoValor = @p_TipoCodigoValor  WHERE CodigoValor = @p_CodigoISIN
	
	--	--Historia
	--IF LTRIM(RTRIM(ISNULL(@p_Rating, ''))) <> @Rating
	--	EXEC sp_SIT_InsertaRatingValorHistoria @p_CodigoISIN, @p_Rating, @p_UsuarioModificacion, @p_FechaModificacion
		
	-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	delete from RatingValorHistoria where CodigoIsin = @p_CodigoISIN and FechaCreacion = @p_FechaModificacion
	
	INSERT INTO RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion)
	VALUES (@p_CodigoISIN, @p_Rating, @p_UsuarioModificacion, @p_FechaModificacion)
	-- FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico			
END

GO

GRANT EXECUTE ON [dbo].[Valores_Modificar] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[ListarNemonicoXValorizar]    Script Date: 01/24/2019 10:22:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ListarNemonicoXValorizar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ListarNemonicoXValorizar]
GO

---------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Obtiene lista de los Nemonicos X Valorizar      
---------------------------------------------------------------------------------------------------------------------      
-- Fecha Modificación: 25/07/2018      
-- Modificado por: Diego      
-- Nro. Orden de Trabajo: 11450    
-- Descripcion del cambio: Nuevo      
---------------------------------------------------------------------------------------------------------------------      
-- Fecha Modificación: 31/08/2018      
-- Modificado por: Ricardo Colonia      
-- Nro. Orden de Trabajo: 11547      
-- Descripcion del cambio: Se agrega Filtro de carga de archivo precio forward y precios no cargados para fechas     
--         anteriores en reporteVL    
---------------------------------------------------------------------------------------------------------------------    
-- Fecha modificacion: 31/08/2018    
-- Modificado por: Ian Pastor Mendoza    
-- Nro. Orden de Trabajo: 11590    
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO    
---------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 24/01/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11636    
-- Descripcion del cambio: Se adiciona validación por fecha de vencimiento en tipo renta variable.
---------------------------------------------------------------------------------------------------------------------  
  
CREATE PROCEDURE [dbo].[ListarNemonicoXValorizar]    
(      
 @p_Portafolio varchar(250),      
 @p_fecha NUMERIC(8)      
)      
AS      
BEGIN      
    
 DECLARE @FECHA NUMERIC(8)        
 DECLARE @FECHAAYER DATETIME        
 --DECLARE @VectorPrecioVal VARCHAR(4)      
    DECLARE @COUNTNEMONICO INT     
         
 CREATE TABLE #lstNemonico     
   (ID INT IDENTITY(1,1),        
    CodigoMnemonico VARCHAR(15))        
    
 SET @FECHAAYER = CONVERT(DATETIME, CONVERT(CHAR(8), @p_fecha))        
 SET @FECHAAYER = DATEADD(day,-1, @FECHAAYER)         
 SET @FECHA =(SELECT YEAR(@FECHAAYER) * 10000 + MONTH(@FECHAAYER) * 100 + DAY(@FECHAAYER))     
 --SET @VectorPrecioVal = (SELECT VectorPrecioVal FROM Portafolio WHERE CodigoPortafolioSBS = @p_Portafolio)      
         
 INSERT #lstNemonico        
 SELECT  VL.CodigoNemonico FROM ReporteVL VL         
 INNER JOIN Valores V ON VL.CodigoNemonico = V.CodigoNemonico        
 INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS =  TI.CodigoTipoInstrumentoSBS        
 INNER JOIN ClaseInstrumento CI ON TI.CodigoClaseInstrumento = CI.CodigoClaseInstrumento        
 WHERE VL.Fecha =  @FECHA AND VL.CodigoPortafolioSBS = @p_Portafolio        
 AND CI.Categoria NOT IN ('CV','DP','FD','OR')        
 AND V.TipoRenta = 1      
 AND (ISNULL(V.FechaVencimiento,0) = 0 or ISNULL(V.FechaVencimiento,0) > @p_fecha)          
 UNION      
 SELECT  VL.CodigoNemonico FROM ReporteVL VL         
 INNER JOIN Valores V ON VL.CodigoNemonico = V.CodigoNemonico        
 INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS =  TI.CodigoTipoInstrumentoSBS        
 INNER JOIN ClaseInstrumento CI ON TI.CodigoClaseInstrumento = CI.CodigoClaseInstrumento        
 WHERE VL.Fecha =  @FECHA AND VL.CodigoPortafolioSBS = @p_Portafolio        
 AND CI.Categoria NOT IN ('CV','DP','FD','OR')        
 AND V.TipoRenta = 2 
 AND (ISNULL(V.FechaVencimiento,0) = 0 or ISNULL(V.FechaVencimiento,0) > @p_fecha)      
    
 --SET @COUNTNEMONICO = (SELECT COUNT(VP.CodigoMnemonico) FROM VectorPrecio VP    
 --WHERE VP.CodigoMnemonico IN (SELECT CodigoMnemonico FROM #lstNemonico)    
 --AND VP.Fecha = @p_fecha AND VP.EntidadExt = @VectorPrecioVal)    
     
 SET @COUNTNEMONICO = (    
 SELECT COUNT(VP.CodigoMnemonico) FROM VectorPrecio VP    
  INNER JOIN #lstNemonico LN ON VP.CodigoMnemonico = LN.CodigoMnemonico    
 WHERE VP.Fecha = @p_fecha AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(@p_Portafolio,LN.CodigoMnemonico,@p_fecha)    
 )    
     
 DELETE FROM tmpNemonicoValorizacion      
    
 INSERT INTO tmpNemonicoValorizacion       
 SELECT DISTINCT OI.CodigoMnemonico FROM OrdenInversion OI     
 INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria       
 WHERE OI.FechaOperacion = @p_fecha     
 AND OI.CodigoPortafolioSBS = @p_Portafolio      
 AND OI.Estado <> 'E-ELI'    
 AND OI.CodigoMnemonico NOT IN (SELECT VP.CodigoMnemonico FROM VectorPrecio VP       
           WHERE VP.Fecha =  @p_fecha )     
 AND CI.Categoria NOT IN ('CV','DP','FD','OR')       
    
 UNION    
    
 SELECT DISTINCT OI.CodigoMnemonico FROM OrdenInversion OI     
 INNER JOIN ClaseInstrumento CI ON OI.CategoriaInstrumento = CI.Categoria       
 WHERE OI.FechaOperacion = @p_fecha    
 AND OI.CodigoPortafolioSBS = @p_Portafolio     
 AND CodigoISIN NOT IN (SELECT NumeroPoliza FROM VectorForwardSBS VPS     
         WHERE VPS.Fecha = @p_fecha)     
 AND CI.Categoria = 'FD'    
 AND OI.Estado <> 'E-ELI'    
    
 IF @COUNTNEMONICO <> (SELECT COUNT(ID) FROM #lstNemonico) BEGIN    
     
  INSERT INTO tmpNemonicoValorizacion       
  SELECT DISTINCT CodigoMnemonico     
  FROM #lstNemonico    
  WHERE CodigoMnemonico NOT IN (SELECT VP.CodigoMnemonico FROM VectorPrecio VP       
           WHERE VP.Fecha = @p_fecha   
           AND (ISNULL(PrecioLimpio,0) <> 0 OR ISNULL(PrecioSucio,0) <> 0)
           UNION     
           SELECT CodigoNemonico FROM tmpNemonicoValorizacion    
           )       
 END    
    
 DROP TABLE #lstNemonico    
 SELECT COUNT(tmp.CodigoNemonico) FROM tmpNemonicoValorizacion tmp    
END 
GO

GRANT EXECUTE ON [dbo].[ListarNemonicoXValorizar] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Valores_Seleccionar]    Script Date: 01/24/2019 11:26:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_Seleccionar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Valores_Seleccionar]
GO

-----------------------------------------------------------------------------------------------------------
--Objetivo: Listar datos de valores
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/10/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9515
--	Descripcion del cambio: Listar campos nuevos
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 13/03/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10090
--	Descripcion del cambio: Se agrega el codigo SBS de la moneda
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 22/05/2018
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11377
--	Descripcion del cambio: Se agrega campo TipoRentaRiesgo, Frecuencia de Pago (Dividendo) y Base Interes Corrido
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se adiciono el campo de Subordinado
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 15/01/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se adicionó el campo de PrecioDevengado.
----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Valores_Seleccionar] @p_CodigoNemonico varchar(15)
AS
BEGIN
	SET NOCOUNT ON
	SELECT v.CodigoNemonico,v.CodigoTipoInstrumentoSBS,ISNULL(ti.Descripcion, '') AS DescripcionTipoInstrumento,v.CodigoTipoTitulo,v.Descripcion,
	v.Agrupacion,v.CodigoEmisor,v.CodigoBursatilidad,v.CodigoMoneda,v.NumeroUnidades,v.CodigoISIN,v.ValorUnitario,v.CodigoSBS,v.ValorNominal,v.CodigoCalificacion,
	v.ValorEfectivoColocado,v.TasaEncaje,v.FechaEmision,v.CodigoTipoCupon,v.FechaVencimiento,v.TasaCupon,v.FechaPrimerCupon,v.CodigoPeriodicidad,
	v.CodigoTipoAmortizacion,v.TasaSpread,v.CodigoIndicador,v.ValorIndicador,v.Situacion,v.BaseTir,v.BaseTirDias,TipoRenta,v.TipoCuponera,v.BaseCupon,
	v.BaseCuponDias,v.Observacion,v.CantidadIE,v.RentaFijaIE,v.RentaVarIE,v.GrupoContable,
	v.Rescate,v.MonedaCupon,v.MonedaPago,v.TipoRentaFija,ti.CodigoClaseInstrumento,v.AplicaReduccionUnidades,
	v.MontoDividendo,v.Rating,v.TipoDerivado,v.CodigoTipoDerivado,v.FactorFlotante,v.SpreadFlotante,
	v.CodigoEmisorIE,v.PosicionAct,v.PorcPosicion,v.Categoria,v.CondicionImpuesto,v.Liberada,
	v.FechaLiberada,v.MargenInicial,v.MargenMantenimiento,v.ContractSize,v.BaseIC,v.BaseICDias,
	t.Situacion SituacionEmi,v.FirmaLlamado,v.ValorUnitarioActual,v.Estilo,v.GeneraInteres,
	v.TipoCodigoValor,V.CodigoMercado, V.MontoPiso,V.MontoTecho,V.Garante,V.Subyacente,V.PrecioEjercicio,V.TamanoEmision,
	ISNULL(V.CodigoPortafolioSBS,'') CodigoPortafolioSBS, M.CodigoMonedaSBS  --9515--10090
	,ISNULL(v.TipoRentaRiesgo,'') TipoRentaRiesgo  -- 22/05/2018 - RCE - Agregar nuevo campo TipoRentaRiesgo
	,ISNULL(v.CodigoFrecuenciaDividendo,'') CodigoFrecuenciaDividendo -- 22/05/2018 - RCE - Agregar nuevo campo CodigoFrecuenciaDividendo
	,ISNULL(V.EstadoBaseIC,'0')EstadoBaseIC,ISNULL(V.BaseInteresCorrido,'0') BaseInteresCorrido, ISNULL(V.BaseInteresCorridoDias,'0') BaseInteresCorridoDias  -- 22/05/2018 - RCE - Nuevo Campo Estado Base Interés Corrido
	,ISNULL(V.Subordinado,'0') Subordinado -- 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,ISNULL(V.PrecioDevengado,'0') PrecioDevengado
	FROM VALORES  v
	LEFT JOIN TipoInstrumento ti ON v.CodigoTipoInstrumentoSBS=ti.CodigoTipoInstrumentoSBS      
	LEFT OUTER JOIN Entidad e ON v.CodigoEmisor = e.CodigoEntidad
	LEFT JOIN Terceros t ON t.CodigoTercero = e.CodigoTercero
	JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
	WHERE v.CODIGONEMONICO = @p_CodigoNemonico
END

GO

GRANT EXECUTE ON [dbo].[Valores_Seleccionar] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_SeleccionarPorCodigoFactura]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Valores_SeleccionarPorCodigoFactura]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 15/01/2019
--	Creado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: creacion del procedimiento Valores_SeleccionarPorCodigoFactura
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Valores_SeleccionarPorCodigoFactura]  
	@p_CodigoMnemonico VARCHAR(15) = '',  
	@p_CodigoFactura VARCHAR(30) = ''  
AS  
BEGIN  

	SELECT  
		CodigoNemonico,   
		CodigoISIN,
		Descripcion,  
		Situacion, 
		CodigoFactura      
	FROM Valores      
		WHERE  
		CodigoNemonico LIKE (CASE WHEN LEN(@p_CodigoMnemonico) = 0 THEN '%' ELSE @p_CodigoMnemonico END)  
		AND (CodigoFactura LIKE (CASE WHEN LEN(@p_CodigoFactura) = 0 THEN '%' ELSE @p_CodigoFactura  END))  
END

GO

GRANT EXECUTE ON [dbo].[Valores_SeleccionarPorCodigoFactura] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_Validar_Facturas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Valores_Validar_Facturas]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 15/01/2019
--	Creado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: creacion del procedimiento Valores_Validar_Facturas
---------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Valores_Validar_Facturas]
	@p_Facturas  NVARCHAR(MAX)
AS
BEGIN
	SELECT CodigoFactura,CodigoNemonico, CodigoISIN FROM Valores  where CodigoFactura in (
	select * from  [dbo].[fnSplitString] (@p_Facturas,',')
	)
END


GO

GRANT EXECUTE ON [dbo].[Valores_Validar_Facturas] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Seleccionar_CodigoOrdenyPreOrden]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Seleccionar_CodigoOrdenyPreOrden]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 15/01/2019
--	Creado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: creacion del procedimiento Seleccionar_CodigoOrdenyPreOrden
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Seleccionar_CodigoOrdenyPreOrden]  
 @p_CodigoNemonico VARCHAR(15) = '' 
  
AS  
BEGIN
	SELECT CodigoOrden FROM  OrdenInversion  WHERE CodigoMnemonico=@p_CodigoNemonico
	SELECT CodigoPrevOrden FROM  PrevOrdenInversion  WHERE CodigoNemonico=@p_CodigoNemonico
END

GO

GRANT EXECUTE ON [dbo].[Seleccionar_CodigoOrdenyPreOrden] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_Actualizar_PrecioLimpioSucio]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[OrdenInversion_Actualizar_PrecioLimpioSucio]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 15/01/2019
--	Creado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: creacion del procedimiento OrdenInversion_Actualizar_PrecioLimpioSucio
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[OrdenInversion_Actualizar_PrecioLimpioSucio]
	@p_CodigoNemonico  varchar(15),
	@p_Precio  varchar(12)
AS
BEGIN
	declare @precioLocal decimal(13,7) = (select top 1 isnull(PrecioNegociacionLimpio,0) from OrdenInversion where CodigoMnemonico=@p_CodigoNemonico and CodigoOperacion='1' order by FechaOperacion asc)

	if (ISNULL(@precioLocal,0) =0)
	BEGIN
		update OrdenInversion set 
		PrecioNegociacionLimpio =@p_Precio,
		PrecioNegociacionSucio = @p_Precio
		where CodigoMnemonico=@p_CodigoNemonico
		and CodigoOperacion='1'
	END

END

GO

GRANT EXECUTE ON [dbo].[OrdenInversion_Actualizar_PrecioLimpioSucio] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_InsertarOI]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[OrdenInversion_InsertarOI]
GO
-----------------------------------------------------------------------------------------------------------------------
--Objetivo: Insertar en Orden de Inversion
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 11/10/2016
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9396
--	Descripcion del cambio: Creacion
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 10/03/2017
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10099
--	Descripcion del cambio: Se validara el numero de identificacion para cuando no hay negociaciones anteriores
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 21/09/2017
--	Creado por: Ian Pastor
--	Nro. Orden de Trabajo: 10783
--	Descripcion del cambio: El número de identificación debe de ingresarse en el proceso de Confirmación de órdenes
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 17/08/2018
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Reemplazar la función "dbo.retornarPrecioVector" por "dbo.RetornarVectorPrecioT_1"
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 27/08/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar el campo TipoValorización para los tipos de instrumentos Bonos
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 31/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO.
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 11/12/2018
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11687
--	Descripcion del cambio: Se aumenta tamaño de input: @p_Plazo  a NUMERIC(4) y se añade la opción SWAP Bonos.
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 15/01/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se modifico para que contemple las ordenes que pertecen a la clase facturas negociables
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[OrdenInversion_InsertarOI]
	@p_FechaOperacion NUMERIC(8) ,
	@p_FechaLiquidacion     NUMERIC(8),
	@p_MontoNominalOrdenado NUMERIC(22,7),
	@p_MontoNominalOperacion NUMERIC(22,7),
	@p_CodigoTipoCupon VARCHAR(4),
	@p_YTM   NUMERIC(22,7),
	@p_PrecioNegociacionLimpio NUMERIC(22,7),
	@p_PrecioNegociacionSucio NUMERIC(22,7),
	@p_MontoOperacion  NUMERIC(22,7),
	@p_Observacion     VARCHAR(20),
	@p_Plazo      NUMERIC(4),
	@p_PTasa      NUMERIC(22,7),
	@p_InteresAcumulado     NUMERIC(22,7),
	@p_InteresCastigado     NUMERIC(22,7),
	@p_TasaCastigo     NUMERIC(22,7),
	@p_MontoPreCancelar     NUMERIC(22,7),
	@p_InteresCorridoNegociacion NUMERIC(22,7) ,   
	@p_PorcentajeAcciones   NUMERIC(22,6) ,   
	@p_PorcentajeDolares    NUMERIC(22,6) ,   
	@p_PorcentajeBonos NUMERIC(22,6) ,   
	@p_CantidadValor   NUMERIC(22,7) ,   
	@p_MontoContado    NUMERIC(22,7) ,   
	@p_MontoPlazo      NUMERIC(22,7) ,   
	@p_Precio     NUMERIC(22,7) ,   
	@p_TipoCambio      NUMERIC(22,7) ,   
	@p_MontoOrigen     NUMERIC(22,7) ,   
	@p_MontoDestino    NUMERIC(22,7) ,   
	@p_TipoCobertura   VARCHAR(1) ,   
	@p_FechaPago  NUMERIC(8) ,   
	@p_ContadoSoles    NUMERIC(22,7) ,   
	@p_TipoCambioSpot  NUMERIC(22,7) ,   
	@p_ContadoDolares  NUMERIC(22,7) ,   
	@p_PlazoSoles      NUMERIC(22,7) ,   
	@p_TipoCambioForw  NUMERIC(22,7) ,   
	@p_PlazoDolares    NUMERIC(22,7) ,   
	@p_PorcentajeRENDimiento NUMERIC(22,7) ,   
	@p_TipoCambioFuturo     NUMERIC(22,7) ,   
	@p_CodigoMoneda    VARCHAR(10) ,   
	@p_CodigoMonedaDestino  VARCHAR(10) ,   
	@p_CodigoPreOrden  VARCHAR(6) ,   
	@p_UsuarioCreacion VARCHAR(15) ,   
	@p_HoraCreacion    VARCHAR(10) ,   
	@p_TotalComisiones NUMERIC(22,7) ,   
	@p_PrecioPromedio  NUMERIC(22,7) ,   
	@p_MontoNetoOperacion   NUMERIC(22,7) ,   
	@p_Estado     VARCHAR(5) ,   
	@p_Situacion  VARCHAR(1) ,   
	@p_Delibery   VARCHAR(1) ,   
	@p_Host       VARCHAR(20) ,   
	@p_CodigoISIN      VARCHAR(12) ,   
	@p_CodigoSBS  VARCHAR(12) ,   
	@p_CodigoTercero   VARCHAR(11) ,   
	@p_FechaCreacion   NUMERIC(8) ,   
	@p_CodigoPortafolioSBS  VARCHAR(10) ,   
	@p_CodigoMnemonico VARCHAR(15) ,   
	@p_CodigoContacto  VARCHAR(3) ,   
	@p_CodigoOperacion  VARCHAR(6) ,   
	@p_CantidadOrdenado NUMERIC(22,7),   
	@p_CantidadOperacion NUMERIC(22,7), 
	@p_NumeroPoliza VARCHAR(20), 
	@p_CodigoUsuario VARCHAR(20), 
	@p_HoraOperacion VARCHAR(10), 
	@p_CodigoGestor VARCHAR(20), 
	@p_CodigoMonedaOrigen VARCHAR(10), 
	@p_DIFerencial NUMERIC(22,7), 
	@p_CodigoMotivo VARCHAR(10), 
	@p_MontoCancelar NUMERIC(22,7), 
	@p_CodigoTipoTitulo VARCHAR(20), 
	@p_CategoriaInstrumento VARCHAR(2), 
	@p_CodigoMnemonicoReporte VARCHAR(15), 
	@p_Pagina VARCHAR(2), 
	@p_FechaContrato NUMERIC(8), 
	@p_OR_CantidadRV NUMERIC(22,7), 
	@p_OR_CantidadNominal NUMERIC(22,7), 
	@p_OR_CantidadRF NUMERIC(22,7), 
	@p_TasaPorcentaje NUMERIC(22,7), 
	@p_TipoFondo  VARCHAR(10), 
	@p_FechaTrato  NUMERIC(8), 
	@p_IsTemporal  VARCHAR(1), 
	@p_PrecioCalculado NUMERIC(22,7), 
	@p_InteresCorrido NUMERIC(22,7), 
	@p_AfectaFlujoCaja VARCHAR(1),
	@p_OrdenGenera VARCHAR(20) = NULL,
	@p_CodigoPlaza VARCHAR(3) = NULL,
	@p_MontoPrima NUMERIC(22,7),
	@p_TipoTramo VARCHAR(10) = NULL,
	@p_GrupoIntermediario VARCHAR(10) =NULL,
	@p_Fixing NUMERIC(22,7) = NULL,
	@p_Ficticia VARCHAR(1)='N',
	@p_Renovacion VARCHAR(1) = '',
	@p_TipoMonedaForw VARCHAR(10) = '',
	@p_RegulaSBS VARCHAR(1) = '',
	@p_MedioNegociacion VARCHAR(1) = NULL,
	@p_HoraEjecucion VARCHAR(10) = NULL,
	@p_EventoFuturo NUMERIC(1,0) = 0,
	@p_TipoCondicion VARCHAR(5) = '',
	@p_VencimientoAno VARCHAR(10) = '',
	@p_VencimientoMes VARCHAR(10) = '',
	@p_TipoValorizacion VARCHAR(10) = NULL,
	@p_TirNeta NUMERIC(22,7) = NULL
AS
--Obtener Tipo Titulo
BEGIN
	DECLARE @chrExceso CHAR(1), @m_CodigoTipoTitulo VARCHAR(20), @m_CodigoNemonico VARCHAR(20), @m_CodigoSBS VARCHAR(12),@m_CodigoSBSEmisor VARCHAR(4),
	@m_CodigoMonedaSBS VARCHAR(1),@m_CorrelativoDPZ VARCHAR(5),@m_CorrelativoFD VARCHAR(3),@m_SecuenciaFD int,@m_OperacionFD char(1),
	@CodigoTipoInstrumentoSBS VARCHAR(3),@m_TotalExposicion NUMERIC(22,7),@m_NumeroIdentificacion VARCHAR(10)
	SET @m_CodigoSBS = @p_CodigoSBS
	SET @p_TipoFondo = ISNULL(@p_TipoFondo,'')
	SET @p_Ficticia = ISNULL(@p_Ficticia,'')
	
	IF @p_CategoriaInstrumento in ('AC','BO','CD','CS','IC','IE','LH','FI','FM','PA','PC','WA','SW','BS','FA') 
	BEGIN
		SET @m_CodigoTipoTitulo  = ( SELECT  ISNULL(CodigoTipoTitulo, '') FROM   valores WHERE CodigoNemonico = @p_CodigoMnemonico  ) 
		SET @m_CodigoNemonico = @p_CodigoMnemonico 
	END 
	ELSE IF @p_CategoriaInstrumento = 'CV' 
	BEGIN
		SET @m_CodigoTipoTitulo = 'DIVISA'
		SET @m_CodigoNemonico = @p_CodigoMnemonico
		IF ISNULL(@p_CodigoPreOrden,'') <> '' AND ISNULL(@p_MontoDestino,0) = 0
			SELECT @p_MontoDestino = @p_MontoOrigen * TipoCambio,@p_TipoCambio = TipoCambio,
			@p_CodigoMonedaDestino = CASE WHEN ISNULL(@p_CodigoMonedaDestino,'') = '' THEN CodigoMonedaDestino ELSE @p_CodigoMonedaDestino END,
			@p_AfectaFlujoCaja = CASE WHEN ISNULL(@p_AfectaFlujoCaja,'') = '' THEN AfectaFlujoCaja ELSE @p_AfectaFlujoCaja END 
			FROM PreOrdenInversion WHERE CodigoPreOrden = @p_CodigoPreOrden
	END
	ELSE IF @p_CategoriaInstrumento = 'OR' 
	BEGIN
		SET @m_CodigoTipoTitulo = @p_CodigoTipoTitulo 
		SET @m_CodigoNemonico = @p_CodigoMnemonico 
	END 
	ELSE IF @p_CategoriaInstrumento = 'DP' 
	BEGIN
		--Existira un Tipo de Titulo Especial para Depositos a PLazos (Su Nemonico es el mismo)   
		SET @m_CodigoTipoTitulo = @p_CodigoTipoTitulo
		SET @m_CodigoNemonico = @p_CodigoTipoTitulo   
		--Formato codigoSBS = 60eeeem00001 (e=emisor, m=moneda, 00001=correlativo)   
		SELECT @m_CodigoSBSEmisor = CodigoSBS
		FROM Entidad WHERE CodigoTercero = @p_CodigoTercero
		SELECT @m_CodigoMonedaSBS = CodigoMonedaSBS
		FROM Moneda WHERE CodigoMoneda = @p_CodigoMoneda
		--No se estaban considerANDo los DPX (69)
		SELECT @CodigoTipoInstrumentoSBS = CodigoTipoInstrumentoSBS
		FROM Valores WHERE CodigoNemonico = @p_CodigoTipoTitulo
		SELECT @m_CorrelativoDPZ = right(replicate('0',5) + cast(ISNULL(max(substring(codigoSBS,8,5)),0) + 1 as VARCHAR), 5)
		FROM ordenInversion
		WHERE codigoSBS like @CodigoTipoInstrumentoSBS+'%' AND isNUMERIC(substring(codigoSBS,8,5)) = 1
		SET @m_CodigoSBS = @CodigoTipoInstrumentoSBS + @m_CodigoSBSEmisor + @m_CodigoMonedaSBS + @m_CorrelativoDPZ
	END
	ELSE IF @p_CategoriaInstrumento = 'FD' 
	BEGIN
		--Existe un Nemonico y Tipo de Titulo Expecial para Forward de Divisas, este srá ingresado 
		--solamente por   
		SET @m_CodigoTipoTitulo = 'OTRODIVISA'
		SET @m_CodigoNemonico = 'FORWARD'
		--Formato codigoSBS = 84eeeemHX001 (e=emisor, m=moneda, X= C(compra) o V(venta), 001=correlativo) 
		SELECT @m_CodigoSBSEmisor = CodigoSBS 
		FROM Entidad WHERE CodigoTercero = @p_CodigoTercero 
		SELECT @m_CodigoMonedaSBS = CodigoMonedaSBS 
		FROM Moneda WHERE CodigoMoneda = CASE @p_CodigoOperacion  WHEN '93' THEN @p_CodigoMoneda WHEN '94' THEN @p_CodigoMonedaDestino END
		--RGF 20090217 Se cambio el formato del correlativo por pedido de la SBS, ahora es alfaNUMERICo 
		SELECT @m_SecuenciaFD = max(cf.Secuencia)
		FROM OrdenInversion oi 
		inner join CorrelativoForward cf on cf.Codigo = oi.NumeroPoliza
		WHERE oi.codigoMnemonico = 'FORWARD' AND oi.estado <> 'E-ELI' AND oi.situacion = 'A' 
		SELECT @m_CorrelativoFD = Codigo FROM CorrelativoForward
		WHERE Secuencia = ISNULL(cast(@m_SecuenciaFD as int),0) + 1
		SELECT @m_OperacionFD = CASE 
		WHEN @p_CodigoOperacion = '93' THEN 'C' --compra
		WHEN @p_CodigoOperacion = '94' THEN 'V' END 
		SET @p_NumeroPoliza = 'H' + @m_OperacionFD + @m_CorrelativoFD
		SET @m_CodigoSBS = '84' + @m_CodigoSBSEmisor + @m_CodigoMonedaSBS + 'H' + @m_OperacionFD + @m_CorrelativoFD 
		SET @p_CodigoISIN = (SELECT Valor FROM ParametrosGenerales WHERE ClasIFicacion = 'IDSAFM') + RIGHT('0000' + @m_CorrelativoFD, 4) +
		(SELECT TOP 1 ISNULL(CodigoEmision, '00') FROM Terceros WHERE CodigoTercero = @p_CodigoTercero) +
		(SELECT SUBSTRING(CAST(@p_FechaLiquidacion AS VARCHAR),7,2) + SUBSTRING(CAST(@p_FechaLiquidacion AS VARCHAR),5,2))
		DECLARE @NEMONICONEW VARCHAR(12)
		SET @NEMONICONEW = (SELECT Valor FROM ParametrosGenerales WHERE ClasIFicacion = 'IDSAFM') +
		 RIGHT('0000' + @m_CorrelativoFD, 4) +
		(SELECT TOP 1 ISNULL(CodigoEmision, '00') FROM Terceros WHERE CodigoTercero = @p_CodigoTercero) +
		(SELECT SUBSTRING(CAST(@p_FechaLiquidacion AS VARCHAR),3,2))
		UPDATE	CorrelativoForward
		SET		CodigoNemonico = @NEMONICONEW
		WHERE	Secuencia = ISNULL(cast(@m_SecuenciaFD as int),0) + 1
	END
	ELSE IF @p_CategoriaInstrumento = 'FT'
	BEGIN
		--Existe un Nemonico y Tipo de Titulo Expecial para Forward de Divisas, este srá ingresado 
		--solamente por 
		SELECT	@m_CodigoTipoTitulo = 'OTRODIVISA',@m_CodigoNemonico = @p_CodigoMnemonico
		SELECT @m_TotalExposicion = (SELECT TOP 1 TotalExposicion FROM PrevOrdenInversion WHERE CodigoPrevOrden = @p_CodigoPreOrden)
		--Formato codigoSBS = 84eeeemHX001 (e=emisor, m=moneda, X= C(compra) o V(venta), 001=correlativo) 
		SELECT	@m_CodigoSBSEmisor = CodigoSBS FROM	Entidad WHERE	CodigoTercero = @p_CodigoTercero 
		SELECT	@m_CodigoMonedaSBS = CodigoMonedaSBS FROM	Moneda WHERE
		CodigoMoneda =	CASE @p_CodigoOperacion WHEN '93' THEN @p_CodigoMoneda WHEN '94' THEN @p_CodigoMonedaDestino END
		--RGF 20090217 Se cambio el formato del correlativo por pedido de la SBS, ahora es alfaNUMERICo 
		SELECT @m_SecuenciaFD = MAX(cf.Secuencia) FROM OrdenInversion oi 
		INNER JOIN	CorrelativoForward cf ON	cf.Codigo = SUBSTRING(oi.codigoSBS,10,3) 
		WHERE oi.codigoMnemonico = @p_CodigoMnemonico AND oi.estado <> 'E-ELI' AND oi.situacion = 'A' 
		SELECT	@m_CorrelativoFD = Codigo FROM	CorrelativoForward 
		WHERE	Secuencia = ISNULL(CAST(@m_SecuenciaFD as int),0) + 1
		SELECT	@m_OperacionFD = CASE WHEN @p_CodigoOperacion = '93' THEN 'C' WHEN @p_CodigoOperacion = '94' THEN 'V' END
		SET @p_NumeroPoliza = 'H' + @m_OperacionFD + @m_CorrelativoFD
	END
	SET @chrExceso = ''
	IF ISNULL(@p_Estado,'') in ('E-EXC','E-ENV','E-EBL','E-ELC')
		SET @chrExceso = 'S' 
	ELSE
	BEGIN
		IF (@p_Pagina = 'DA' OR @p_CategoriaInstrumento = 'BS' OR  @p_CategoriaInstrumento = 'WA' OR  @p_CategoriaInstrumento = 'FA' ) 
			SET @p_Estado = 'E-EJE' 
		ELSE
			SET @p_Estado = 'E-ING'
	END
	IF dbo.fn_EsMultiPortafolio(@p_CodigoPortafolioSBS) = 'N'
	BEGIN
		DECLARE @p_codigoOrdenInversion VARCHAR(6)   
		--se llama al procedimiento almacenado GenerarCodigoOrdenInversion para generar otro codigo   
		EXEC GenerarCodigoOrdenInversion @p_codigoOrdenInversion output, @p_CodigoPortafolioSBS
		--Correlativo para los DP Y OR
		/*
		OT10783
		IF @p_CategoriaInstrumento = 'DP' OR @p_CategoriaInstrumento = 'OR'
		BEGIN
			SELECT @m_NumeroIdentificacion = MAX(CONVERT(INT, ISNULL(NumeroIdentificacion ,0))) FROM OrdenInversion 
			WHERE CategoriaInstrumento = @p_CategoriaInstrumento AND Situacion = 'A' AND Estado <> 'E-ELI' AND CodigoTercero = @p_CodigoTercero
			AND CodigoMoneda = @p_CodigoMoneda  AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			SET @m_NumeroIdentificacion = ISNULL(@m_NumeroIdentificacion,0) + 1 --10099
		END
		*/
		--Inserta los datos de la orden		
		INSERT INTO OrdenInversion(CodigoOrden,FechaOperacion,FechaLiquidacion,MontoNominalOrdenado,MontoNominalOperacion,CodigoTipoCupon,YTM,
		PrecioNegociacionLimpio,PrecioNegociacionSucio,MontoOperacion,Observacion,Plazo,PTasa,InteresAcumulado,InteresCastigado,TasaCastigo,
		MontoPreCancelar,InteresCorridoNegociacion,PorcentajeAcciones,PorcentajeDolares,PorcentajeBonos,CantidadValor,MontoContado,MontoPlazo,
		Precio,TipoCambio,MontoOrigen,MontoDestino,TipoCobertura,FechaPago,ContadoSoles,TipoCambioSpot,ContadoDolares,PlazoSoles,TipoCambioForw,
		PlazoDolares,PorcentajeRENDimiento,TipoCambioFuturo,CodigoMoneda,CodigoMonedaDestino,CodigoPreOrden,UsuarioCreacion,HoraCreacion,TotalComisiones,
		PrecioPromedio,MontoNetoOperacion,Estado,Situacion,Delibery,Host,CodigoISIN,CodigoSBS,CodigoTercero,FechaCreacion,CodigoPortafolioSBS,CodigoMnemonico,
		CodigoContacto,CodigoOperacion,CantidadOrdenado,CantidadOperacion,NumeroPoliza,CodigoUsuario,HoraOperacion,CodigoGestor,CodigoMonedaOrigen,DIFerencial,
		CodigoMotivo,MontoCancelar,CodigoTipoTitulo,CategoriaInstrumento, CodigoMnemonicoReporte,FechaContrato,OR_CantidadRV,OR_CantidadNominal,OR_CantidadRF,
		TasaPorcentaje,TipoFondo,FechaTrato,PrecioCalculado,InteresCorrido, Exceso, AfectaFlujoCaja, OrdenGenera,CodigoPlaza,MontoPrima,TipoTramo,
		GrupoIntermediario,Fixing,Ficticia,Renovacion,TipoMonedaForw,RegulaSBS,MedioNegociacion,HoraEjecucion,EventoFuturo,VencimientoMes,VencimientoAno,
		TipoCondicion,TotalExposicion, NumeroIdentificacion, TipoValorizacion )
		VALUES(@p_codigoOrdenInversion,@p_FechaOperacion,@p_FechaLiquidacion,@p_MontoNominalOrdenado,@p_MontoNominalOperacion,@p_CodigoTipoCupon,@p_YTM,
		@p_PrecioNegociacionLimpio,@p_PrecioNegociacionSucio,@p_MontoOperacion,@p_Observacion,@p_Plazo,@p_PTasa,@p_InteresAcumulado,@p_InteresCastigado,
		@p_TasaCastigo,@p_MontoPreCancelar,@p_InteresCorridoNegociacion,@p_PorcentajeAcciones,@p_PorcentajeDolares,@p_PorcentajeBonos,@p_CantidadValor,
		@p_MontoContado,@p_MontoPlazo,@p_Precio,@p_TipoCambio,@p_MontoOrigen,@p_MontoDestino,@p_TipoCobertura,@p_FechaPago,@p_ContadoSoles,@p_TipoCambioSpot,
		@p_ContadoDolares,@p_PlazoSoles,@p_TipoCambioForw,@p_PlazoDolares,@p_PorcentajeRENDimiento,@p_TipoCambioFuturo,@p_CodigoMoneda,@p_CodigoMonedaDestino,
		@p_CodigoPreOrden,@p_UsuarioCreacion,@p_HoraCreacion,@p_TotalComisiones,@p_PrecioPromedio,@p_MontoNetoOperacion,@p_Estado,@p_Situacion,@p_Delibery,@p_Host,
		@p_CodigoISIN,@m_CodigoSBS,@p_CodigoTercero,@p_FechaCreacion,@p_CodigoPortafolioSBS,@m_CodigoNemonico,@p_CodigoContacto,@p_CodigoOperacion,@p_CantidadOrdenado,
		@p_CantidadOperacion,CASE @m_CodigoNemonico WHEN 'FORWARD' THEN RIGHT('0000' + @m_CorrelativoFD, 4) ELSE @p_NumeroPoliza END ,@p_CodigoUsuario,@p_HoraOperacion,
		@p_CodigoGestor,@p_CodigoMonedaOrigen,@p_DIFerencial,@p_CodigoMotivo,@p_MontoCancelar,@m_CodigoTipoTitulo,@p_CategoriaInstrumento,@p_CodigoMnemonicoReporte,
		@p_FechaContrato,@p_OR_CantidadRV,@p_OR_CantidadNominal,@p_OR_CantidadRF,@p_TasaPorcentaje,@p_TipoFondo,@p_FechaTrato,@p_PrecioCalculado,@p_InteresCorrido,
		@chrExceso,@p_AfectaFlujoCaja,@p_OrdenGenera,  @p_CodigoPlaza,@p_MontoPrima,@p_TipoTramo,@p_GrupoIntermediario,@p_Fixing,@p_Ficticia,@p_Renovacion,
		@p_TipoMonedaForw,@p_RegulaSBS,@p_MedioNegociacion,@p_HoraEjecucion,@p_EventoFuturo,@p_VencimientoMes,@p_VencimientoAno,@p_TipoCondicion,@m_TotalExposicion,
		@m_NumeroIdentificacion, @p_TipoValorizacion)
		DECLARE  @CodigoCustodio VARCHAR(12)
		SELECT @CodigoCustodio = dbo.ObtenerCustodio(@p_CodigoPortafolioSBS,@p_CodigoMnemonico,@p_CodigoTercero)
		--Validacion de generacion de saldos para las emisiones primarias.
		DECLARE @fechaNegocio NUMERIC(8,0)
		SELECT @fechaNegocio = ISNULL(FechaNegocio,0)
		FROM Portafolio 
		WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND Situacion = 'A'
		DECLARE @Cantidad NUMERIC(22,7),@Precio NUMERIC(22,7),@Asignacion NUMERIC(22,7)
		SET @Cantidad = ISNULL((SELECT count(1) FROM PrevOrdenInversion po
		inner join TmpPrevOrdenInversion_OI toi on po.CodigoPrevOrden = toi.CodigoPrevOrden
		inner join TmpPrevOrdenInversion t on t.CodigoNemonico = po.CodigoNemonico AND t.CodigoOperacion = po.CodigoOperacion AND t.CodigoTercero = po.CodigoTercero
		AND po.FechaOperacion = @p_FechaOperacion AND po.Situacion = 'A'
		WHERE t.CodigoNemonico = @m_CodigoNemonico AND t.CodigoOperacion = @p_CodigoOperacion AND t.CodigoTercero = @p_CodigoTercero AND t.Fondo = @p_CodigoPortafolioSBS), 0)
		IF @Cantidad = 0
		BEGIN
			SET @Cantidad = CASE WHEN @p_CantidadOperacion=0 THEN @p_CantidadOrdenado ELSE ISNULL(@p_CantidadOperacion, @p_CantidadOrdenado) END
			SET @Precio = CASE WHEN @p_PrecioNegociacionLimpio=0 THEN @p_Precio ELSE ISNULL(@p_PrecioNegociacionLimpio, @p_Precio) END
			SET @Cantidad = CASE @p_CategoriaInstrumento WHEN 'FD' THEN @p_MontoCancelar WHEN 'DP' THEN @p_MontoNominalOperacion WHEN 'CV' THEN @p_MontoOperacion ELSE @Cantidad END
			SET @Asignacion = @Cantidad 
			EXEC dbo.pr_SIT_ins_TrazabilidadOperaciones_Insertar_sura @p_FechaOperacion,0,@p_Estado,'2',0,@p_codigoOrdenInversion,@m_CodigoNemonico,
			@p_CodigoOperacion,@Cantidad,@Precio,@p_CodigoTercero,@Cantidad,@Precio,@p_CodigoPortafolioSBS,@Asignacion,'2','15','','Ingreso Orden de Inversión',
			@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host
		END
		SELECT CodigoOrden = @p_codigoOrdenInversion
	END
	ELSE  
	BEGIN  
		DECLARE @p_codigoPreOrdenInversion VARCHAR(6)   
		--se llama al procedimiento almacenado GenerarCodigoPreOrdenInversion para generar otro codigo   
		EXEC GenerarCodigoPreOrdenInversion @p_codigoPreOrdenInversion output, @p_CodigoPortafolioSBS   
		--Inserta los datos de la orden   
		INSERT INTO PreOrdenInversion(CodigoPreOrden,FechaOperacion,FechaLiquidacion,MontoNominalOrdenado,MontoNominalOperacion,CodigoTipoCupon,
		YTM,PrecioNegociacionLimpio,PrecioNegociacionSucio,MontoOperacion,Observacion,Plazo,PTasa,InteresAcumulado,InteresCastigado,TasaCastigo,
		MontoPreCancelar,InteresCorridoNegociacion,PorcentajeAcciones,PorcentajeDolares,PorcentajeBonos,CantidadValor,MontoContado,MontoPlazo,
		Precio,TipoCambio,MontoOrigen,MontoDestino,TipoCobertura,FechaPago,ContadoSoles,TipoCambioSpot,ContadoDolares,PlazoSoles, TipoCambioForw,
		PlazoDolares,PorcentajeRENDimiento,TipoCambioFuturo,CodigoMoneda,CodigoMonedaDestino,UsuarioCreacion,HoraCreacion,TotalComisiones,PrecioPromedio,
		MontoNetoOperacion,Estado,Situacion,Delibery,Host,CodigoISIN,CodigoSBS,CodigoTercero,FechaCreacion,CodigoPortafolioSBS,CodigoMnemonico,CodigoContacto,
		CodigoOperacion,CantidadOrdenado,CantidadOperacion,NumeroPoliza,CodigoUsuario,HoraOperacion,CodigoGestor,CodigoMonedaOrigen,DIFerencial,CodigoMotivo,
		MontoCancelar,CodigoTipoTitulo,CategoriaInstrumento ,CodigoMnemonicoReporte,FechaContrato,OR_CantidadRV ,OR_CantidadNominal,OR_CantidadRF,TasaPorcentaje,
		TipoFondo,FechaTrato, PrecioCalculado,InteresCorrido, CodigoPlaza,  AfectaFlujoCaja,TipoTramo,GrupoIntermediario,Fixing,HoraEjecucion)
		VALUES(@p_codigoPreOrdenInversion,@p_FechaOperacion,@p_FechaLiquidacion,@p_MontoNominalOrdenado,@p_MontoNominalOperacion,@p_CodigoTipoCupon, @p_YTM,
		@p_PrecioNegociacionLimpio,@p_PrecioNegociacionSucio,@p_MontoOperacion,@p_Observacion,@p_Plazo,@p_PTasa,@p_InteresAcumulado,@p_InteresCastigado,
		@p_TasaCastigo,@p_MontoPreCancelar,@p_InteresCorridoNegociacion,@p_PorcentajeAcciones,@p_PorcentajeDolares,@p_PorcentajeBonos,@p_CantidadValor,
		@p_MontoContado,@p_MontoPlazo, @p_Precio,@p_TipoCambio,@p_MontoOrigen,@p_MontoDestino,@p_TipoCobertura,@p_FechaPago,@p_ContadoSoles,@p_TipoCambioSpot,
		@p_ContadoDolares,@p_PlazoSoles,@p_TipoCambioForw,@p_PlazoDolares,@p_PorcentajeRENDimiento,@p_TipoCambioFuturo,@p_CodigoMoneda,@p_CodigoMonedaDestino,
		@p_UsuarioCreacion,@p_HoraCreacion,@p_TotalComisiones,@p_PrecioPromedio,@p_MontoNetoOperacion,@p_Estado,@p_Situacion,@p_Delibery,@p_Host, 
		@p_CodigoISIN,@m_CodigoSBS,@p_CodigoTercero,@p_FechaCreacion,@p_CodigoPortafolioSBS,@m_CodigoNemonico,@p_CodigoContacto,@p_CodigoOperacion,
		@p_CantidadOrdenado,@p_CantidadOperacion,CASE @m_CodigoNemonico WHEN 'FORWARD' THEN NULL ELSE @p_NumeroPoliza END,
		@p_CodigoUsuario,@p_HoraOperacion,@p_CodigoGestor,@p_CodigoMonedaOrigen,@p_DIFerencial,@p_CodigoMotivo,@p_MontoCancelar,@m_CodigoTipoTitulo,
		@p_CategoriaInstrumento,@p_CodigoMnemonicoReporte,@p_FechaContrato,@p_OR_CantidadRV,@p_OR_CantidadNominal,@p_OR_CantidadRF,@p_TasaPorcentaje,
		@p_TipoFondo,@p_FechaTrato,@p_PrecioCalculado,@p_InteresCorrido, @p_CodigoPlaza,  @p_AfectaFlujoCaja,@p_tipotramo,@p_GrupoIntermediario,
		@p_Fixing,@p_HoraEjecucion) 
		SELECT @p_codigoPreOrdenInversion
	END
	--RGF 20090327 Al negociar la compra de un estructurado, se debe generar la orden de venta de su correspondiente bono
	IF @p_CategoriaInstrumento = 'IE'
	BEGIN
		SELECT @p_MontoNominalOperacion = @p_CantidadOperacion * ISNULL(ie.Cantidad,0) * ISNULL(v.ValorUnitario,0),
		@p_CantidadOperacion = @p_CantidadOperacion * ISNULL(ie.Cantidad,0),@p_CodigoMnemonico = ie.CodigoNemonicoAsociado,
		@p_CategoriaInstrumento = 'BO',@p_OrdenGenera = @p_codigoOrdenInversion,@p_CodigoOperacion = CASE @p_CodigoOperacion WHEN 1 THEN 2 WHEN 2 THEN 1 END,
		--@p_Precio = dbo.RetornarVectorPrecioT_1(ie.CodigoNemonicoAsociado, @VectorPrecioVal, @p_FechaOperacion)
		@p_Precio = dbo.RetornarSecuenciaVectorPrecio(@p_CodigoPortafolioSBS,ie.CodigoNemonicoAsociado,@p_FechaOperacion)
		,@p_CodigoTipoCupon = 1,@p_MontoOperacion = @p_Precio*@p_MontoNominalOperacion/ISNULL(v.ValorUnitario,1)
		FROM InstrumentosEstructurados ie
		inner join Valores v on v.CodigoNemonico = ie.CodigoNemonicoAsociado
		WHERE ie.CodigoNemonico = @p_CodigoMnemonico AND ie.situacion = 'A'
		SELECT @p_CodigoISIN = CodigoISIN,@p_CodigoSBS = CodigoSBS,@p_PrecioNegociacionLimpio = @p_Precio/100
		FROM valores
		WHERE codigoNemonico = @p_CodigoMnemonico
		create table #tmpYTM (YTM NUMERIC(22,7))
		INSERT INTO #tmpYTM EXEC OrdenInversion_CalculoTasaNegociacionIteracionPrecio '', @p_CodigoMnemonico, @p_FechaOperacion, @p_MontoNominalOperacion, @p_CodigoTipoCupon, @p_PrecioNegociacionLimpio
		SELECT @p_YTM = YTM FROM #tmpYTM
		drop table #tmpYTM
		SET @p_PrecioNegociacionSucio = @p_Precio/10
		EXEC OrdenInversion_CalcularInteresCorridos '', @p_CodigoMnemonico, @p_FechaOperacion, @p_MontoNominalOperacion, 0, @p_CodigoTipoCupon, @p_InteresCorridoNegociacion output
		EXEC OrdenInversion_InsertarOI @p_FechaOperacion,@p_FechaLiquidacion,@p_MontoNominalOperacion,@p_MontoNominalOperacion,@p_CodigoTipoCupon,
		@p_YTM,@p_PrecioNegociacionLimpio,@p_PrecioNegociacionSucio,@p_MontoOperacion,@p_Observacion,@p_Plazo,@p_PTasa,@p_InteresAcumulado,@p_InteresCastigado,
		@p_TasaCastigo,@p_MontoPreCancelar,@p_InteresCorridoNegociacion,@p_PorcentajeAcciones,@p_PorcentajeDolares,@p_PorcentajeBonos,@p_CantidadValor,@p_MontoContado,
		@p_MontoPlazo,@p_Precio,@p_TipoCambio,@p_MontoOrigen,@p_MontoDestino,@p_TipoCobertura,@p_FechaPago,@p_ContadoSoles,@p_TipoCambioSpot,@p_ContadoDolares,
		@p_PlazoSoles,@p_TipoCambioForw,@p_PlazoDolares,@p_PorcentajeRENDimiento,@p_TipoCambioFuturo,@p_CodigoMoneda,@p_CodigoMonedaDestino,@p_CodigoPreOrden,
		@p_UsuarioCreacion,@p_HoraCreacion,@p_TotalComisiones,@p_PrecioPromedio,@p_MontoOperacion,@p_Estado,@p_Situacion,@p_Delibery,@p_Host,@p_CodigoISIN,
		@p_CodigoSBS,@p_CodigoTercero,@p_FechaCreacion,@p_CodigoPortafolioSBS,@p_CodigoMnemonico,@p_CodigoContacto,@p_CodigoOperacion,@p_CantidadOperacion,
		@p_CantidadOperacion,@p_NumeroPoliza, @p_CodigoUsuario,@p_HoraOperacion,@p_CodigoGestor,@p_CodigoMonedaOrigen,@p_DIFerencial,@p_CodigoMotivo,
		@p_MontoCancelar,@p_CodigoTipoTitulo,@p_CategoriaInstrumento,@p_CodigoMnemonicoReporte,@p_Pagina,@p_FechaContrato,@p_OR_CantidadRV,@p_OR_CantidadNominal,
		@p_OR_CantidadRF,@p_TasaPorcentaje,@p_TipoFondo,@p_FechaTrato,@p_IsTemporal,@p_PrecioNegociacionLimpio,@p_InteresCorridoNegociacion,@p_AfectaFlujoCaja,
		@p_OrdenGenera,@p_CodigoPlaza,@p_MontoPrima,NULL,@p_GrupoIntermediario,NULL,@p_Ficticia
	END
END

GO

GRANT EXECUTE ON [dbo].[OrdenInversion_InsertarOI] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

/****** Object:  UserDefinedFunction [dbo].[CalcularInteresCorridoCompra]    Script Date: 01/24/2019 15:55:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcularInteresCorridoCompra]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalcularInteresCorridoCompra]
GO


-------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 15/11/2018  
-- Creado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11636  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR TIPO CAMBIO
-------------------------------------------------------------------------------------------------------------------------------------  

CREATE FUNCTION [dbo].[CalcularInteresCorridoCompra](  
 @p_codigoportafolio VARCHAR(20),  
 @p_CodigoIsin VARCHAR(20),  
 @p_FechaOperacion NUMERIC(8),  
 @p_MonedaLocal CHAR(1)  
)  
RETURNS NUMERIC(22,7)  
AS  
BEGIN  
	DECLARE 
		@importe NUMERIC(22,7), 
		@FechaOperacion NUMERIC(8)  
	
	SELECT TOP 1 
		@FechaOperacion = FechaOperacion 
	FROM 
		OrdenInversion 
	WHERE 
		CodigoISIN = @p_CodigoIsin 
		AND CodigoPortafolioSBS = @p_codigoportafolio  
		AND Estado <> 'E-ELI' 
		AND Situacion = 'A' 
		AND CodigoOperacion IN( '35','2') 
		AND FechaOperacion <= @p_FechaOperacion  
	ORDER BY 
		FechaLiquidacion DESC,
		FechaOperacion DESC
		
	--Suma los intereses corridos despues de la ultima compra o un ultimo vencimiento   
	SELECT 
		@importe =  SUM(ISNULL(OI.InteresCorridoNegociacion,0))  
	FROM 
		OrdenInversion OI   
	WHERE 
		OI.CodigoISIN = @p_CodigoIsin 
		AND OI.CodigoPortafolioSBS = @p_codigoportafolio 
		AND OI.Estado <> 'E-ELI' AND OI.Situacion = 'A'  
		AND OI.CodigoOperacion = '1' 
		AND OI.FechaOperacion >= ISNULL(@FechaOperacion,0) 
		AND OI.FechaOperacion <= @p_FechaOperacion 
		 
	--Tipo de cambio  
	IF @p_MonedaLocal = '1'  
	SELECT 
		@importe = CASE WHEN M.TipoCalculo = 'D' THEN @importe * VTC.ValorPrimario ELSE @importe / VTC.ValorPrimario END  
	FROM 
		Valores V  
	JOIN 
		VectorTipoCambio VTC ON VTC.CodigoMoneda = V.CodigoMoneda 
								AND VTC.Fecha = @p_FechaOperacion  
								AND VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_codigoportafolio,V.CodigoMoneda,@p_FechaOperacion)  
	JOIN 
		Moneda M ON M.CodigoMoneda = V.CodigoMoneda  
	WHERE 
		V.CodigoISIN = @p_CodigoIsin   
		
	RETURN ISNULL(@importe,0)  
END
GO

GRANT EXECUTE ON [dbo].[CalcularInteresCorridoCompra] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO

/****** Object:  UserDefinedFunction [dbo].[CalcularInteresGanadoBono]    Script Date: 01/25/2019 14:30:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcularInteresGanadoBono]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalcularInteresGanadoBono]
GO

----------------------------------------------------------------------------------------------------------------------  
--Objetivo: RETORNAR EL IMPORTE DE INTERESES CALCULADOS DE BONOS  
----------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 31/10/2018  
-- Creado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11652  
-- Descripcion del cambio: Se incluye descuento por aumento de capital.  
----------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 15/11/2018  
-- Creado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR TIPO CAMBIO.
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 15/12/2018  
-- Creado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11687  
-- Descripcion del cambio: Cambiar descuento de importe por aumento de capital en cuponeranormal.
------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 15/12/2018  
-- Creado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11636  
-- Descripcion del cambio: Agregar funcionalidad para calcular en base a carteratitulovalorizacion con dia anterior a fecha ingreso.
-------------------------------------------------------------------------------------------------------------------------------------  
  
CREATE FUNCTION [dbo].[CalcularInteresGanadoBono](    
 @p_codigoportafolio VARCHAR(20),    
 @p_CodigoIsin VARCHAR(20) ,    
 @p_FechaOperacion NUMERIC(8),    
 @p_MonedaLocal CHAR(1)    
)    
RETURNS numeric(22,7)    
AS    
BEGIN    
	DECLARE 
		@IMPORTE NUMERIC(22,7),
		@p_montoNominalLocal NUMERIC(22,7),
		@p_tasaCupon NUMERIC(22,12), 
		@p_diasUltimoFlujo NUMERIC(18,0),
		@Modalidad NUMERIC(22,8),    
		@p_valorUnitario NUMERIC(22,7),
		@p_base NUMERIC(22,7), 
		@CodigoNemonico VARCHAR(20),
		@GeneraInteres CHAR(1), 
		@p_diasFlujo NUMERIC(18,0),    
		@BaseCalculoDiferenciaDias VARCHAR(4),
		@InteresCorridoCompra NUMERIC(22,7),
		@p_ValorIndicador NUMERIC(22,7) ,
		@BaseCalculoDiferenciaDiasTir VARCHAR(4),    
		@CategoriaIns VARCHAR(20) 
		   
	--Interes Corrido de Compra, moneda original    
	SET @InteresCorridoCompra = DBO.CalcularInteresCorridoCompra(@p_codigoportafolio,@p_CodigoIsin,@p_FechaOperacion,'2')    
	
	--Valores    
	SELECT TOP 1 
		@p_ValorIndicador = Valor 
	FROM 
		ValorIndicador  
	WHERE 
		CodigoIndicador = (SELECT ISNULL(CodigoIndicador,'') FROM Valores WHERE CodigoISIN = @p_CodigoIsin)    
		AND Fecha >= @p_FechaOperacion 
	ORDER BY 
		FECHA ASC    
	 
	SELECT 
		@p_valorUnitario = ValorUnitario, 
		@p_base =  BaseCupon,
		@CodigoNemonico = CodigoNemonico ,    
		@BaseCalculoDiferenciaDias = BaseInteresCorridoDias, 
		@BaseCalculoDiferenciaDiasTir = BaseInteresCorridoDias, 
		@CategoriaIns = CI.Categoria    
	FROM 
		Valores V    
	JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = v.CodigoTipoInstrumentoSBS    
	JOIN 
		ClaseInstrumento ci ON ci.CodigoClaseInstrumento = ti.CodigoClaseInstrumento    
	
	WHERE 
		CodigoISIN = @p_CodigoIsin   
		 
	--Cartera 
	DECLARE
		@FechaDiaAnterior DATETIME = CONVERT(DATETIME, CONVERT(CHAR(8), @p_FechaOperacion)),
		@FechaT_1 DECIMAL(8)  
		 
	SET @FechaDiaAnterior = DATEADD(DAY,-1, @FechaDiaAnterior)
	SET @FechaT_1 =(SELECT YEAR(@FechaDiaAnterior) * 10000 + MONTH(@FechaDiaAnterior) * 100 + DAY(@FechaDiaAnterior))	
		
	SELECT 
		@p_montoNominalLocal = ValorNominalMonedaOrigen - ISNULL(dbo.NominalLiberado(CodigoPortafolioSBS ,@p_CodigoIsin ,FechaValoracion ,'1'),0)
	FROM 
		CarteraTituloValoracion 
	WHERE 
		CodigoMnemonico = @CodigoNemonico 
		AND CodigoPortafolioSBS = @p_codigoportafolio    
		AND FechaValoracion = (CASE WHEN @p_MonedaLocal = 'X' THEN @FechaT_1 ELSE @p_FechaOperacion END)
		
	--Cuponera    
	SELECT 
		@p_tasaCupon = TasaCupon + ISNULL(@p_ValorIndicador,0), 
		@p_diasUltimoFlujo = CASE WHEN @BaseCalculoDiferenciaDiastir = '30' THEN dbo.DATEDIFF360(CN.FechaInicio,@p_FechaOperacion,0)     
																		   ELSE DATEDIFF(DAY,CAST(CN.FechaInicio  AS CHAR(8)),CAST(@p_FechaOperacion AS CHAR(8)))    
																		   END,    
		@Modalidad =  CN.DiasPago,    
		@p_diasFlujo  =     
		CASE WHEN @BaseCalculoDiferenciaDias = '30' THEN dbo.DATEDIFF360(CN.FechaInicio,CN.FechaTermino,0)     
												   ELSE DATEDIFF(DAY,CAST(CN.FechaInicio AS CHAR(8)),CAST(CN.FechaTermino AS CHAR(8)))    
												   END    
	FROM 
		CuponeraNormal  CN    
	WHERE 
		CN.CodigoNemonico = @CodigoNemonico 
		AND CN.FechaInicio <= @p_FechaOperacion  
		AND CN.FechaTermino > @p_FechaOperacion    
		
	IF @Modalidad - @p_diasFlujo > 1 AND @CategoriaIns <> 'PC' SET @p_diasFlujo = @Modalidad    

	--SET @importe = ROUND(((@p_montoNominalLocal * @p_tasaCupon * @Modalidad ) / (100 * @p_base))  * (@p_diasUltimoFlujo/@p_diasFlujo ),2) - @InteresCorridoCompra     
	SET @importe = ROUND(((@p_montoNominalLocal * @p_tasaCupon * @p_diasUltimoFlujo ) / (100 * @p_base)) ,2) - @InteresCorridoCompra     
	
	IF @p_MonedaLocal = '1'    
		SELECT 
			@importe = CASE WHEN M.TipoCalculo = 'D' THEN @importe * VTC.ValorPrimario ELSE @importe / VTC.ValorPrimario END    
		FROM 
			Valores V    
		JOIN 
			VectorTipoCambio VTC ON VTC.CodigoMoneda = V.CodigoMoneda 
									AND VTC.Fecha = @p_FechaOperacion   
									AND VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_codigoportafolio,V.CodigoMoneda,@p_FechaOperacion)   
		JOIN 
			Moneda M ON M.CodigoMoneda = V.CodigoMoneda    
		WHERE 
			V.CodigoISIN = @p_CodigoIsin    

-- INICIO AUMENTO DE CAPITAL  
	DECLARE 
		@totalInteresAumentoCapital NUMERIC(22,7)    
	SELECT    
		@totalInteresAumentoCapital = SUM(O.MontoNetoOperacion)  
	FROM   
		OrdenInversion O    
	INNER JOIN  
		AumentoCapitalDetalle ACD ON ACD.CodigoOrden = O.CodigoOrden  
	INNER JOIN   
		AumentoCapital AC ON AC.idAumentoCapital= ACD.idAumentoCapital  
	INNER JOIN   
		Portafolio P ON O.CodigoPortafolioSBS = P.CodigoPortafolioSBS    
	INNER JOIN   
		CuponeraNormal CN ON O.CodigoMnemonico = CN.CodigoNemonico
							 AND CN.Secuencia = ACD.SecuenciaCuponera
	WHERE   
		O.CodigoPortafolioSBS = @p_codigoportafolio    
		AND O.Situacion='A'    
		AND O.CodigoOperacion IN ('35')    
		AND O.Estado = 'E-CON'  
		AND O.FechaOperacion <= @p_FechaOperacion  
		AND CN.FechaInicio <= @p_FechaOperacion 
		AND CN.FechaTermino > @p_FechaOperacion    
		AND P.FlagAumentoCapital = 1  
		AND AC.Estado = 'A'  
		AND O.CodigoISIN = @p_CodigoIsin  

-- FIN AUMENTO DE CAPITAL  
   
	RETURN  ISNULL(@importe,0)  - ISNULL(@totalInteresAumentoCapital,0)  
END    
    


GO

GRANT EXECUTE ON [dbo].[CalcularInteresGanadoBono] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
/*
exec ReporteGestion_ReporteConsolidado @p_CodigoPortafolio = '2666,', @p_FechaProceso = 20190128
*/
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
---------------------------------------------------------------------------------------------------------------------
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
		,V.CodigoMoneda
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
		
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) * /* Pasamos a MONEDA LOCAL NSOL*/	
			CASE OI.TipoValorizacion
				WHEN 'DIS_VENTA' THEN VA.VTA_ValorNominal
				WHEN 'A_VENCI' THEN VA.TIRCOM_ValorNominal
				WHEN 'VAL_RAZO' THEN VA.TIRRAZ_ValorNominal
			END AS MontoNominalInicial
				
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) /* Pasamos a MONEDA LOCAL NSOL*/
			* CASE OI.TipoValorizacion
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
			
		,'TIRRAZ_ValorActual_PrecioLimpio' = VA.TIRRAZ_PrecioLimpio * VA.TIRRAZ_SaldoNominalVigente
		
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
		END)) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) AS Sobre_BajoPrecio
		
		,(VA.TIRRAZ_ValorActual - VA.TIRCOM_ValorActual) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) AS FluctuacionValor
		 
		,0 AS DuracionSBS
		,(CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_CantidadOperacion * VA.VTA_PrecioSucio
			WHEN 'A_VENCI' THEN VA.TIRCOM_CantidadOperacion * VA.TIRCOM_PrecioSucio
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_CantidadOperacion * VA.TIRRAZ_PrecioSucio
		END)/dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso) AS ParticipacionPatrimonio
		--,CASE dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso)
		--	WHEN 0 THEN 0
		--	ELSE VA.TIRCOM_ValorActual/dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso)
		-- END AS ParticipacionPatrimonio
		
		,ISNULL((CASE OI.TipoValorizacion
			WHEN 'DIS_VENTA' THEN VA.VTA_CantidadOperacion * VA.VTA_PrecioSucio
			WHEN 'A_VENCI' THEN VA.TIRCOM_CantidadOperacion * VA.TIRCOM_PrecioSucio
			WHEN 'VAL_RAZO' THEN VA.TIRRAZ_CantidadOperacion * VA.TIRRAZ_PrecioSucio
		END), 0) AS VPNFondo
		,dbo.RetornarInversiones_ValorFondo(VA.CodigoPortafolioSBS,VA.FechaProceso) AS PatrimonioFondo
		
	FROM ValorizacionAmortizada VA
		INNER JOIN OrdenInversion OI ON OI.CodigoOrden = VA.CodigoOrden AND OI.CodigoPortafolioSBS = VA.CodigoPortafolioSBS
		INNER JOIN Valores V ON VA.CodigoNemonico = V.CodigoNemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		LEFT JOIN ParametrosGenerales PG ON PG.Clasificacion = 'RATING' AND V.Rating = PG.Valor
		INNER JOIN Portafolio P ON VA.CodigoPortafolioSBS = P.CodigoPortafolioSBS
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
		,V.CodigoISIN
		,V.CodigoMoneda
		,Rating = ''
		,FechaVencimiento = OI.FechaContrato
		,ProximoCuponFecha = NULL
		,0 AS ProximoCuponMonto
		,0 AS CantidadOperacion
		,OI.MontoNominalOrdenado AS MontoNominalInicial
		,OI.MontoNominalOrdenado AS MontoNominalVigente
		,0 AS PrecioOperacion
		,OI.MontoNominalOrdenado AS ValorCompra
		,0 AS TIRCOM_PrecioSucio
		,CTV.VPNMonedaFondo AS TIRCOM_ValorActual
		,0 AS TIRRAZ_PrecioLimpio
		,CTV.VPNMonedaFondo AS TIRRAZ_ValorActual
		,0 AS TIRRAZ_ValorActual_PrecioLimpio
		,OI.TasaPorcentaje AS TasaDescuentoSBS
		,OI.TasaPorcentaje AS TasaCupon
		,OI.TasaPorcentaje AS TasaDescuentoCompra
		,CTV.VPNMonedaFondo - OI.MontoNominalOrdenado AS InteresesCorridos
		,0 AS Sobre_BajoPrecio
		,0 AS FluctuacionValor
		,0 AS DuracionSBS
		,CASE dbo.RetornarInversiones_ValorFondo(CTV.CodigoPortafolioSBS,CTV.FechaValoracion)
			WHEN 0 THEN 0
			ELSE CTV.VPNMonedaFondo/dbo.RetornarInversiones_ValorFondo(CTV.CodigoPortafolioSBS,CTV.FechaValoracion)
		 END AS ParticipacionPatrimonio
		 ,ISNULL(CTV.VPNMonedaFondo, 0) AS VPNFondo
		 ,dbo.RetornarInversiones_ValorFondo(CTV.CodigoPortafolioSBS,CTV.FechaValoracion) AS PatrimonioFondo
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
		OI.CodigoPortafolioSBS
		,DescripcionPortafolio = P.Descripcion
		,TI.CodigoTipoInstrumentoSBS
		,DescripcionTipoInstrumento = TI.Descripcion
		,V.CodigoNemonico
		,V.CodigoISIN
		,V.CodigoMoneda
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
		,0 AS TIRRAZ_ValorActual_PrecioLimpio
		,0 AS TasaDescuentoSBS
		,OI.TasaPorcentaje AS TasaCupon
		,OI.TasaPorcentaje AS TasaDescuentoCompra
		,OI.TotalComisiones AS InteresesCorridos
		,0 AS Sobre_BajoPrecio
		,0 AS FluctuacionValor
		,0 AS DuracionSBS
		,CASE dbo.RetornarInversiones_ValorFondo(OI.CodigoPortafolioSBS,OI.FechaOperacion)
			WHEN 0 THEN 0
			ELSE OI.MontoNetoOperacion/dbo.RetornarInversiones_ValorFondo(OI.CodigoPortafolioSBS,OI.FechaOperacion)
		 END AS ParticipacionPatrimonio
		 ,ISNULL(OI.MontoNetoOperacion, 0) AS VPNFondo
		 ,dbo.RetornarInversiones_ValorFondo(OI.CodigoPortafolioSBS,OI.FechaOperacion) AS PatrimonioFondo
	FROM OrdenInversion OI
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
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


USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_SIT_LIS_ValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_SIT_LIS_ValorCuota]
GO
----------------------------------------------------------------------------------------------------
-- Objetivo	: Lista la cartera por Fondo
----------------------------------------------------------------------------------------------------
-- Parámetros:
--	1.- @p_CodigoPortafolioSBS - IN - varchar(20) - Codigo Portafolio
--	2.- @p_FechaInicial - IN - numeric(8,0) - Fecha Inicial
--	3.- @p_FechaFinal - IN - numeric(8,0) - Fecha Final
----------------------------------------------------------------------------------------------------
-- Fecha de Creación	    : 03/05/2015
-- Creado por    			: Michell Cornejo
-- Nro. Orden de Trabajo	: 8716 
-- Descripción del Cambio	: Creación
----------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[SP_SIT_LIS_ValorCuota](
	@p_CodigoPortafolioSBS varchar(20),
	@p_FechaInicial numeric(8),
	@p_FechaFinal numeric(8)
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
	       vc.Caja,
	       patrimonioPreCierre = vc.ValPatriPreCierre1,
	       patrimonioCierre = vc.ValPatriCierreValores,	       
	       vc.OtrosGastos,
	       vc.OtrosIngresos,
	       p.Descripcion + '_' + dbo.FormatDate(vc.FechaProceso),
	       p.CodigoMoneda as CodigoMonedaFondo
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	WHERE (vc.CodigoPortafolioSBS = @p_CodigoPortafolioSBS or @p_CodigoPortafolioSBS = '') 
	  AND vc.FechaProceso between @p_FechaInicial and @p_FechaFinal AND p.PorSerie = 'N'
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
	       vc.Caja,	              
	       patrimonioPreCierre = vc.ValPatriPreCierre1,
	       patrimonioCierre = vc.ValPatriCierreValores,	       
	       vc.OtrosGastos,
	       vc.OtrosIngresos,
	       p.Descripcion + '_' + dbo.FormatDate(vc.FechaProceso),
	       p.CodigoMoneda as CodigoMonedaFondo
	FROM ValorCuota vc
	JOIN Portafolio p on vc.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN PortafolioSerie PS ON PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS AND VC.CodigoSerie = PS.CodigoSerie
	WHERE (vc.CodigoPortafolioSBS = @p_CodigoPortafolioSBS or @p_CodigoPortafolioSBS = '')
	  AND vc.FechaProceso between @p_FechaInicial and @p_FechaFinal AND p.PorSerie = 'S'
END
GO

GRANT EXECUTE ON [dbo].[SP_SIT_LIS_ValorCuota] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO

-- ==============================================================================================================
-- Autor: Ernesto Galarza	
-- Fecha Creación: 15/01/2019
-- Órden de Trabajo: 11636
-- Descripción:		Actualización de Intereses de ReporteVL y cambio de cuenta NT en el SIT de P500608 a P900014.
-- ==============================================================================================================

BEGIN TRANSACTION __Transaction_Log

PRINT 'Actualización BONO - BDCA1BC1C --> PRES-LATIN'
UPDATE 
	ReporteVL
SET 
	InteresesCorrido = ROUND(DBO.CalcularInteresCorridoCompra(CodigoPortafolioSBS,'CVL79590C039',Fecha,'1'),2),
	InteresesGanado = ROUND(DBO.CalcularInteresGanadoBono(CodigoPortafolioSBS,'CVL79590C039',Fecha ,'1'),2)
WHERE 
	CodigoNemonico = 'BDCA1BC1C'
	AND CodigoPortafolioSBS = '47'
	AND Fecha > 20190110

UPDATE 
	ReporteVL
SET 
	Ganancia_Perdida = ROUND(Valorizacion - (MontoInversion + InteresesCorrido + InteresesGanado),2) 
WHERE 
	CodigoNemonico = 'BDCA1BC1C'
	AND CodigoPortafolioSBS = '47'
	AND Fecha > 20190110

PRINT 'Actualización BONO - CONTI5BC5U --> CASHSOL'

UPDATE 
	ReporteVL
SET 
	InteresesCorrido = ROUND(DBO.CalcularInteresCorridoCompra(CodigoPortafolioSBS,'PEP11600M194',Fecha,'1'),2),
	InteresesGanado = ROUND(DBO.CalcularInteresGanadoBono(CodigoPortafolioSBS,'PEP11600M194',Fecha ,'1'),2)
WHERE 
	CodigoNemonico = 'CONTI5BC5U'
	AND CodigoPortafolioSBS = '13'
	AND Fecha > 20190122
	
UPDATE 
	ReporteVL
SET 
	Ganancia_Perdida = ROUND(Valorizacion - (MontoInversion + InteresesCorrido + InteresesGanado),2) 
WHERE 
	CodigoNemonico = 'CONTI5BC5U'
	AND CodigoPortafolioSBS = '13'
	AND Fecha > 20190122

PRINT 'Actualización de Cuenta P500608 --> P900014'
UPDATE 
	PERSONAL 
SET 
	FechaCese = NULL,
	CodigoInterno = 'P900014',
	CodigoUsuario = 'P900014'
WHERE 
	PrimerNombre = 'GONZALO'
	AND ApellidoPaterno = 'MIRANDA'
	AND ApellidoMaterno = 'NAVARRO'

UPDATE 
	AprobadorTrader
SET 
	CodigoInterno = 'P900014',
	CodigoUsuario = 'P900014'
WHERE
	CodigoInterno = 'P500608'
	AND CodigoUsuario = 'P500608'

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 


