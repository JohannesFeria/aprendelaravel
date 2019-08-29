----------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: EJECUTAR OBJETOS DE BASE DE DATOS
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Creación		: 09/05/2019
-- Modificado por			: Ian Pastor M.
-- Nro. Orden de Trabajo	: 11964
-- Descripción				: -
----------------------------------------------------------------------------------------------------------------------------------------
USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log

PRINT 'INICIO - ACTUALIZACIÓN DE SALDOS DE TENENCIAS DE FONDOS MUTUOS: SURARPIDOL, SURARPISOL'
update SaldosCarteraTitulo
set SALDODISPONIBLE = 0
where CODIGOPORTAFOLIOSBS = '104'
	and CodigoMnemonico in ('SURARPIDOL','SURARPISOL')
	and FECHASALDO >= 20190514
GO

update CustodioSaldo
set SaldoInicialUnidades = 0
where CODIGOPORTAFOLIOSBS = '104'
	and CodigoMnemonico in ('SURARPIDOL','SURARPISOL')
	and FECHASALDO >= 20190514
GO

PRINT 'FIN - ACTUALIZACIÓN DE SALDOS DE TENENCIAS DE FONDOS MUTUOS: SURARPIDOL, SURARPISOL'

PRINT 'INICIO - ACTUALIZAR PARAMETRÍA DE CARTAS'

UPDATE ModeloCarta
SET ArchivoPlantilla = 'Modulos\ModelosCarta\rpt_PlantillaCartas14.rdlc'
WHERE CodigoModelo = 'FWVV' AND CodigoOperacion = '94'
GO

UPDATE ModeloCarta
SET Situacion = 'I'
WHERE CodigoModelo IN ('CAF3','CAF5','CFV3','CFV4','CFV5','CFV7','CFV8','COF0','CAF1','COF1','COF2','COF4','COF5','COF8','COF9','CVF1','CVF2','COF3','CFV9','CAF4')
GO


UPDATE Portafolio
SET FechaCajaOperaciones = P.FechaNegocio
FROM Portafolio P
WHERE CodigoPortafolioSBS IN ('47','11','104','10','222','43','27','120','31','99','102','106','112','2666','2777')
GO

IF NOT EXISTS(
	SELECT 1
	FROM sys.columns c
		INNER JOIN sys.tables t ON c.object_id = t.object_id
	WHERE c.name = 'BancoGlosaDestino' AND t.name = 'OperacionesCaja'
	)
BEGIN
	ALTER TABLE OperacionesCaja	ADD BancoGlosaDestino VARCHAR(20)
END
GO

PRINT 'FIN - ACTUALIZAR PARAMETRÍA DE CARTAS'




PRINT 'INICIO - EJECUCIÓN DE STORE PROCEDURES'

PRINT '[dbo].[pr_SIT_gl_ObtenerEstado_PrevOrdenInversion]'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_SIT_gl_ObtenerEstado_PrevOrdenInversion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[pr_SIT_gl_ObtenerEstado_PrevOrdenInversion]
GO
-----------------------------------------------------------------------------------------------------------  
--Objetivo: Obtiene el estado del registro previo  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 25/04/2019  
-- Creado por: Junior Huallullo
-- Nro. Orden de Trabajo: 11964  
-- Descripcion del cambio: Creacion  
-----------------------------------------------------------------------------------------------------------  
 
CREATE PROCEDURE [dbo].[pr_SIT_gl_ObtenerEstado_PrevOrdenInversion]
@p_CodigoPrevOrden numeric(12)
AS  
BEGIN  
 SET NOCOUNT ON  
 DECLARE @p_Estado VARCHAR(50)
 SET @p_Estado = (SELECT TOP 1 Estado FROM PrevOrdenInversion WHERE CodigoPrevOrden = @p_CodigoPrevOrden)
 SELECT @p_Estado  
END
GO

GRANT EXECUTE ON [dbo].[pr_SIT_gl_ObtenerEstado_PrevOrdenInversion] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[CuentasPorCobrar_Liquidar]'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM SYS.procedures WHERE name = 'CuentasPorCobrar_Liquidar')
	DROP PROCEDURE [dbo].[CuentasPorCobrar_Liquidar]
GO
------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: Liquidar las CXC O CXP
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 07/12/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9678
--	Descripcion del cambio: Nuevo campo para la impresion de cartas
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 23/03/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10150
--	Descripcion del cambio: Se agrega un campo nuevo, correlativo de carta
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 21/04/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10238
--	Descripcion del cambio: Se incluye el calculo para los saldos finales
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 24/05/2017
--	Modificado por: Magno Sanchez
--	Nro. Orden de Trabajo: 10412
--	Descripcion del cambio: Se modifico el ancho de caracteres del parametro @p_ObservacionCarta de 300 a 1000
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 15/08/2017
--	Modificado por: Robert Castillo
--	Nro. Orden de Trabajo: 10689
--	Descripcion del cambio: Se corrigió el cálculo de los saldos bancarios
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 17/01/2019
--	Modificado por: Ian Pastor Mendoza.
--	Nro. Orden de Trabajo: 11732
--	Descripcion del cambio: Se agregaron el campos "@p_ObservacionCartaDestino" para la generación de cartas de instrucción
------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 06/05/2019
--	Modificado por: Ian Pastor Mendoza.
--	Nro. Orden de Trabajo: 11964
--	Descripcion del cambio: Se agregó el campo "@p_BancoGlosaDestino" para la generación de cartas de instrucción
------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CuentasPorCobrar_Liquidar]
		@p_NroOperacion VARCHAR(20),
		@p_NumeroCuenta VARCHAR(25),
		@p_CodigoPortafolio VARCHAR(10),
		@p_FechaPago NUMERIC(10)=0,
		@p_OperacionCaja int=1,
		@p_Importe NUMERIC(22,7) = 0,
		@p_CodigoModalidadPago VARCHAR(4)='',
		@p_usuario VARCHAR(15),
		@p_fecha NUMERIC(8,0),
		@p_Hora VARCHAR(10),
		@p_Host VARCHAR(20),
		@p_NumeroCarta VARCHAR(20)='',
		@nvcCodigoModelo NVARCHAR(4),
		@nvcBancoOrigen NVARCHAR(12),
		@nvcBancoDestino NVARCHAR(12),
		@nvcNumeroCuentaDestino NVARCHAR(25),
		@nvcCodigoContacto VARCHAR(3)='',
		@p_BancoMatrizOrigen VARCHAR(12)='',
		@p_BancoMatrizDestino VARCHAR(12)='',
		@p_CodigoContactoIntermediario VARCHAR(3) = null,
		@p_agrupado char(1) = null,
		@p_TasaImpuesto NUMERIC(22,7),
		@p_CodigoTerceroDestino VARCHAR(12), 
		@p_ObservacionCarta VARCHAR(1000),
		@p_ObservacionCartaDestino VARCHAR(1000),
		@p_BancoGlosaDestino VARCHAR(20)
AS
BEGIN
	DECLARE	@spot VARCHAR(1),@CodigoBanco VARCHAR(10),@CodigoClaseCuenta VARCHAR(3),@EstadoCarta VARCHAR(1),@CodigoOrden VARCHAR(6),
	@SecuenciaCuponera NUMERIC(18,0),@CodigoNemonico VARCHAR(15),@nvcCodigoOperacion NVARCHAR(4),@nvcCodigoMoneda NVARCHAR(4),
	@CodigoPortafolioOrigen VARCHAR(10),@CodigoMercado VARCHAR(3),@CodigoTercero VARCHAR(12),@CodigoCustodio VARCHAR(12),
	@FechaVencimiento NUMERIC(8), @CorrelativoCartas INT
	
	SET @spot = ISNULL((SELECT TOP 1 delibery FROM	ordeninversion WHERE codigoorden=@p_NroOperacion 
							AND codigoportafoliosbs=@p_CodigoPortafolio),'S')
	
	IF @p_NumeroCarta = '' or @p_NumeroCarta is null
	BEGIN
		SELECT @p_NumeroCarta = null, @EstadoCarta = '1'
	END
	ELSE
	BEGIN
		SET	@EstadoCarta = '1'
	END
	
	SELECT @CodigoClaseCuenta = CodigoClaseCuenta FROM CuentaEconomica WITH(NOLOCK)
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND NumeroCuenta = @p_NumeroCuenta
	
	IF (SELECT TOP 1 1 FROM	CuentasPorCobrarPagar WITH(NOLOCK) WHERE	NumeroOperacion = @p_NroOperacion AND	CodigoPortafoliosbs = @p_CodigoPortafolio
	AND	( LiquidaFechaAnt IS NOT NULL OR UsuarioLiqFecAnt IS NOT NULL)) > 0
		UPDATE	OrdenInversion 
		SET FechaLiquidacion = @p_FechaPago,
		FechaModificacion = @p_Fecha,
		HoraModificacion = @p_Hora,
		UsuarioModificacion = @p_Usuario, 
		Host = @p_Host
		WHERE CodigoOrden = (SELECT CodigoOrden FROM	CuentasPorCobrarPagar WITH(NOLOCK) WHERE CodigoPortafolioSBS = @p_CodigoPortafolio 
								AND NumeroOperacion = @p_NroOperacion AND FechaVencimiento = @p_FechaPago  ) 
		AND CodigoPortafolioSBS = @p_CodigoPortafolio
	
	UPDATE CuentasPorCobrarPagar
	SET FechaPago = @p_FechaPago,
	NumeroCuenta = @p_NumeroCuenta,
	CodigoClaseCuenta = CASE WHEN @CodigoClaseCuenta = '' THEN CodigoClaseCuenta ELSE @CodigoClaseCuenta END,
	UsuarioModificacion = @p_usuario,
	FechaModificacion = @p_fecha,
	HoraModificacion = @p_Hora,
	Estado = 'L',
	UsuarioLiqFecAnt = @p_usuario,
	LiquidaFechaAnt = CASE WHEN LiquidaFechaAnt = '1' THEN '2' ELSE NULL END,
	FechaLiquidaAnt = CASE WHEN LiquidaFechaAnt = '1' THEN	(SELECT FechaNegocio FROM portafolio WITH(NOLOCK) WHERE CodigoPortafolioSBS = @p_CodigoPortafolio) 
							ELSE NULL  END,
	FechaVencimiento = CASE WHEN LiquidaFechaAnt = '1' THEN @p_FechaPago ELSE FechaVencimiento END
	WHERE NumeroOperacion = @p_NroOperacion 
	AND	CodigoPortafoliosbs = @p_CodigoPortafolio
	
	DELETE	tmpOrdenLiquidaFechaAnt 
	WHERE NumeroOperacion = @p_NroOperacion 
	AND CodigoPortafolioSBS = @p_CodigoPortafolio
	
	If @p_OperacionCaja = 1
	BEGIN
		DECLARE @CodigoOperacionCaja VARCHAR(7),@CodigoTerceroSC VARCHAR(25),@CodigoAdministra VARCHAR(20)
		
		SET @CodigoAdministra = dbo.fn_CodPortafolioPorNombre('ADMINISTRA')
		
		SELECT @CodigoOperacionCaja = NumeroOperacion,
			@CodigoOrden=CodigoOrden,
			@SecuenciaCuponera = SecuenciaCuponera,
			@CodigoNemonico = CodigoNemonico ,
			@nvcCodigoOperacion = CodigoOperacion,
			@CodigoMercado = CodigoMercado,
			@FechaVencimiento = FechaVencimiento
		FROM CuentasPorCobrarPagar WITH(NOLOCK)
		WHERE NumeroOperacion = @p_NroOperacion 
		AND	CodigoPortafolioSBS = @p_CodigoPortafolio
		
		--CUPONERA
		IF NOT @SecuenciaCuponera IS NULL
		BEGIN
			UPDATE	CuponeraNormal
			SET FechaPago=@p_FechaPago,
			UsuarioModificacion = @p_usuario,
			FechaModificacion = @p_Fecha,
			HoraModificacion = @p_Hora
			WHERE Secuencia = @SecuenciaCuponera 
			AND	CodigoNemonico = @CodigoNemonico
		END
		
		DELETE OperacionesCaja
		WHERE CodigoOperacionCaja = @CodigoOperacionCaja 
		AND	CodigoPortafolioSBS = @p_CodigoPortafolio
		
		--Se obtiene el codigo tercero del numero de cuenta a liquidar, valido solo para operaciones cash call sin cuotas
		SELECT TOP 1 @CodigoTerceroSC = CodigoTercero FROM Entidad WITH(NOLOCK)
		WHERE CodigoEntidad = (	SELECT TOP 1 EntidadFinanciera FROM CuentaEconomica WITH(NOLOCK) WHERE	NumeroCuenta = @p_NumeroCuenta)
		
		--Correlativo Cartas
		IF @nvcCodigoModelo = 'SC01'
			SET @CorrelativoCartas = 0
		ELSE
		BEGIN
			SELECT @CorrelativoCartas = MAX(CorrelativoCartas) 
										FROM OperacionesCaja WITH(NOLOCK) WHERE FechaOperacion = @p_fecha AND CodigoModelo <> 'SC01'
			SET @CorrelativoCartas = ISNULL(@CorrelativoCartas,0) + 1
		END
		--
		INSERT INTO OperacionesCaja(CodigoOperacionCaja,CodigoMercado,CodigoClaseCuenta,NumeroCuenta,Referencia,Importe,UsuarioCreacion,FechaCreacion,CodigoMoneda,
		TipoMovimiento,Situacion,Host,NumeroCarta,EstadoCarta,HoraCreacion,NumeroCuentaDestino,UsuarioModificacion,CodigoClaseCuentaDestino,FechaModificacion,
		CodigoTerceroOrigen,HoraModificacion,CodigoModalidadPago,CodigoOperacion,CodigoTerceroDestino,CodigoPortafolioSBS,FechaOperacion,NumeroOperacion,CodigoOrden,
		CodigoModelo,BancoOrigen,BancoDestino,Spot,CodigoContacto,CodigoTipoOperacion,CodigoContactoIntermediario, CartaAgrupado ,TasaImpuesto,ObservacionCarta,
		CorrelativoCartas,BancoMatrizOrigen,BancoMatrizDestino,ObservacionCartaDestino,BancoGlosaDestino )--10150
		SELECT cc.NumeroOperacion,cc.CodigoMercado,cc.CodigoClaseCuenta,@p_NumeroCuenta,
		CASE 
			WHEN LEFT(cc.NumeroOperacion,1) = 'K' THEN LTRIM(cc.CodigoNemonico) + '-' + RTRIM(T.Descripcion) + '-Cuponera'
			ELSE (CASE 
				WHEN cc.CodigoOperacion IN ('93','94') THEN LTRIM(cc.CodigoNemonico) + '-' + (CASE WHEN oi.Delibery = 'S' THEN 'DELIVERY' ELSE 'NOM-DELIVERY' END) + '-' + RTRIM(T.Descripcion) + '-' + RTRIM(CI.Descripcion)
				ELSE LTRIM(cc.CodigoNemonico) + '-' + RTRIM(T.Descripcion) + '-' + RTRIM(CI.Descripcion)
			END)
		END AS 'Referencia', CASE WHEN @p_Importe = 0 THEN Importe ELSE @p_Importe END,
		@p_usuario,@p_fecha,
		CASE 
			WHEN cc.CodigoMoneda NOT IN ('NSOL','DOL') and cc.codigoportafoliosbs = @CodigoAdministra THEN 'NSOL' 
			ELSE cc.CodigoMoneda 
		END,
		cc.TipoMovimiento,'A',@p_Host,@p_NumeroCarta,@EstadoCarta,@p_Hora,@nvcNumeroCuentaDestino,NULL,NULL,NULL,cc.CodigoTercero,NULL,
		CASE WHEN @p_CodigoModalidadPago='' THEN NULL ELSE @p_CodigoModalidadPago END ,cc.CodigoOperacion,@p_CodigoTerceroDestino ,@p_CodigoPortafolio,@p_FechaPago,cc.NumeroOperacion,
		cc.CodigoOrden,@nvcCodigoModelo,(SELECT TOP 1 CodigoTercero FROM Entidad WHERE CodigoEntidad = @nvcBancoOrigen) ,
		(SELECT TOP 1 CodigoTercero FROM Entidad WHERE CodigoEntidad = @nvcBancoDestino),@Spot,
		CASE WHEN @nvcCodigoContacto = '' THEN null ELSE @nvcCodigoContacto END,o.CodigoTipoOperacion,@p_CodigoContactoIntermediario,
		CASE ISNULL(@p_agrupado,'') WHEN '' THEN NULL ELSE @p_agrupado END,@p_TasaImpuesto,@p_ObservacionCarta, @CorrelativoCartas,
		@p_BancoMatrizOrigen,
		@p_BancoMatrizDestino,
		@p_ObservacionCartaDestino,
		@p_BancoGlosaDestino
		FROM dbo.CuentasPorCobrarPagar cc WITH(NOLOCK) 
		INNER JOIN Terceros T WITH(NOLOCK) 
			ON T.CodigoTercero = cc.CodigoTercero
		LEFT JOIN Valores V WITH(NOLOCK) 
			ON V.CodigoNemonico = cc.CodigoNemonico
		LEFT JOIN TipoInstrumento ti WITH(NOLOCK) 
			ON v.CodigoTipoInstrumentoSBS = ti.CodigoTipoInstrumentoSBS
		LEFT JOIN ClaseInstrumento ci WITH(NOLOCK) 
			ON ti.CodigoClaseInstrumento = ci.CodigoClaseInstrumento
		LEFT JOIN Operacion o WITH(NOLOCK) 
			ON o.CodigoOperacion = cc.CodigoOperacion
		LEFT JOIN OrdenInversion oi WITH(NOLOCK) 
			ON oi.CodigoOrden = cc.NumeroOperacion 
			AND OI.CodigoPortafolioSBS = CC.CodigoPortafolioSBS
		WHERE cc.NumeroOperacion = @p_NroOperacion 
		AND	cc.CodigoPortafolioSBS = @p_CodigoPortafolio 
		AND CC.FechaVencimiento = @p_FechaPago
		
		--Actualiza el saldo final con la logica correspondiente
		EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,	@p_NumeroCuenta,@p_FechaPago --10238
		/*
		--OT10689
		--Para C/V de divisas, se actualiza los saldos bancarios desde CuentasPorCobrar_LiquidarDivisas
		--IF @nvcCodigoOperacion NOT IN ('65', '66')
		--	EXEC SaldosBancarios_Actualizar2 @p_CodigoPortafolio, @p_FechaPago
		*/
		IF @nvcCodigoOperacion in ('35','38','39')
		BEGIN
			DECLARE @FechaVencimientoC NUMERIC(8)
			CREATE TABLE #TmpVencimientoOI( Id int identity, CodigoOrden VARCHAR(12) )
			SELECT @FechaVencimientoC = FechaVencimiento FROM CuponeraPagos WITH(NOLOCK) 
			WHERE OrdenGenera = @CodigoOrden
			INSERT INTO #TmpVencimientoOI  SELECT @CodigoOrden
			EXEC pr_SIT_pro_CalcularValorUnitarioActual_Valores @FechaVencimientoC
		END
	END
	--Saldos de Custodio.
	--La logica para actualizar los saldos del custodio estan en GeneraSaldosCustodioPorNemonico
	DECLARE	@decFechaNegocio NUMERIC(8,0),@decEventoFuturo NUMERIC(1,0),@vcTipoFondo VARCHAR(10),@decFechaOperacion NUMERIC(8)
	SELECT	@decFechaNegocio = ISNULL(FechaNegocio,0) 
	FROM Portafolio WITH(NOLOCK) 
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio 
	AND Situacion = 'A'
	
	SELECT	@codigotercero = codigotercero,
		@vcTipoFondo = TipoFondo,
		@decEventoFuturo = EventoFuturo,
		@decFechaOperacion = FechaOperacion 
	FROM ordeninversion WITH(NOLOCK) 
	WHERE codigoorden = @CodigoOrden
	
	--Validacion para emisiones primarias y CashCall
	IF NOT (@decEventoFuturo = 1 AND @decFechaOperacion > @decFechaNegocio) OR NOT (@vcTipoFondo IN ('CC_CNC','CC_SNC') 
		AND @decFechaOperacion > @decFechaNegocio)
	BEGIN
		SELECT @Codigocustodio = dbo.ObtenerCustodio(@p_CodigoPortafolio, @CodigoNemonico, @codigotercero)
		EXEC GeneraSaldosCustodioPorNemonico @FechaVencimiento, @CodigoNemonico, @CodigoCustodio, @p_CodigoPortafolio,@p_usuario, @p_fecha, @p_Hora,@p_Host
	END
	--Para las cartas que han sido marcadas como renocación
	IF EXISTS(SELECT 1 FROM DPZRenovacionCabecera WITH(NOLOCK) WHERE CodigoOrden = @p_NroOperacion AND Situacion = 'A')
	BEGIN
		DELETE FROM DPZRenovacionDetalle 
		WHERE CodigoCabecera IN (SELECT CodigoCabecera  FROM DPZRenovacionCabecera WITH(NOLOCK)  WHERE CodigoOrden = @p_NroOperacion AND Situacion = 'A')
	END
	--Para las Comisiones Agentes de la ADMINISTRADORA del mercado extranjero
	--se debe insertar en OperacionesCaja un registro del portafolio original (Fondo 1, 2 o 3)
	IF @p_CodigoPortafolio = @CodigoAdministra AND @CodigoMercado = 2 AND @nvcCodigoOperacion = 67
	BEGIN
		SELECT @CodigoPortafolioOrigen = CodigoPortafolioSBS 
		FROM CuentasPorCobrarPagar WITH(NOLOCK) 
		WHERE NumeroOperacion = @CodigoOrden 
		AND CodigoPortafolioSBS <> @p_CodigoPortafolio
		
		INSERT INTO OperacionesCaja(CodigoOperacionCaja,CodigoMercado,CodigoClaseCuenta,NumeroCuenta,Referencia,Importe,UsuarioCreacion,FechaCreacion,CodigoMoneda,
		TipoMovimiento,Situacion,Host,NumeroCarta,EstadoCarta,HoraCreacion,NumeroCuentaDestino,CodigoClaseCuentaDestino,CodigoTerceroOrigen,CodigoModalidadPago,
		CodigoOperacion,CodigoTerceroDestino,CodigoPortafolioSBS,FechaOperacion,NumeroOperacion,CodigoOrden,CodigoModelo,BancoOrigen,BancoDestino,Spot,CodigoContacto,
		CodigoTipoOperacion,CodigoContactoIntermediario,CartaAgrupado,ObservacionCarta,CorrelativoCartas)--10150
		SELECT CodigoOperacionCaja,CodigoMercado,CodigoClaseCuenta,NumeroCuenta,Referencia,Importe,UsuarioCreacion,FechaCreacion,CodigoMoneda,TipoMovimiento,
		Situacion,Host,NumeroCarta,EstadoCarta,HoraCreacion,NumeroCuentaDestino,CodigoClaseCuentaDestino,CodigoTerceroOrigen,CodigoModalidadPago,98, --Cobro de comision Broker
		@p_CodigoTerceroDestino,@CodigoPortafolioOrigen,FechaOperacion,NumeroOperacion,CodigoOrden,CodigoModelo,BancoOrigen,BancoDestino,Spot,CodigoContacto,2,
		CodigoContactoIntermediario,CartaAgrupado,ObservacionCarta,@CorrelativoCartas --10150
		FROM OperacionesCaja WITH(NOLOCK) 
		WHERE CodigoOperacionCaja = @CodigoOperacionCaja 
		AND CodigoPortafolioSBS = @p_CodigoPortafolio
	END
END
GO

GRANT EXECUTE ON [dbo].[CuentasPorCobrar_Liquidar] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto]'
USE [SIT-FONDOS]
GO
/*
exec sp_SIT_Gen_Carta_OperacionForwardVcto @p_CodigoOperacionCaja=N'064803'
*/
IF EXISTS (SELECT 1 FROM SYS.procedures WHERE name = 'sp_SIT_Gen_Carta_OperacionForwardVcto')
	DROP PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionForwardVcto]
GO
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Objetivo: OBTIENE LOS DATOS PARA GENERAR LAS CARTAS DE OPERACIONES DE FORWARD VENCIDOS
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 17/01/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11732
-- Descripcion del cambio: Nuevo
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 09/05/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11964
-- Descripcion del cambio: Mostrar en la columna FechaCarta: La fecha de liquidación de la operación forward y agregar el campo "BancoGlosaDestino"
-----------------------------------------------------------------------------------------------------------------------------------------------------
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
	Banco = CASE WHEN P.IndicadorFondo  = 'S' THEN @BancoPublico ELSE T.Descripcion END,
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
	DescripcionIntermediarioDestino = TD.Descripcion,
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
    OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2,
	BancoGlosaDestino = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoGlosaDestino) ,'')
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




PRINT '[dbo].[OrdenInversion_CalcularPrecioBono]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_CalcularPrecioBono]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrdenInversion_CalcularPrecioBono]
GO
-------------------------------------------------------------------------------------------------------------------------------
--Objetivo: Calcula los precios e interes corrido para Bonos y CD
-------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 03/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9777
--	Descripcion del cambio: Se considera que le nominal en los CD es el mimso monto nominal negociado
-------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 18/07/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10090
--	Descripcion del cambio: Se agregan columnas al temporal para el calculo de los bonos amortizables y CD a la par
--------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 04/10/2017
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 10795
--	Descripcion del cambio: Correción del cálculo del valor nominal
--------------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 04/10/2017
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 11964
--	Descripcion del cambio: Se cambió el tamaño de las variables @TasaDiaria NUMERIC(22,19),@BaseCuponDivision NUMERIC(22,19)
--------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[OrdenInversion_CalcularPrecioBono](
	@p_CodigoPortafolioSBS VARCHAR(3),@p_codigoMnemonico VARCHAR(15),@p_FechaOperacion NUMERIC(8),@p_FechaLiquidacion NUMERIC(8),
	@p_TasaAnual NUMERIC(22,7),@P_MontoNominal NUMERIC(22,7),@p_TipoTasa CHAR(1),@p_Operacion VARCHAR(20)
)
AS  
BEGIN
	/*
	OT10795 06/10/2017 - El parámetro @P_MontoNominal ya incluye su amortización. (En caso tuviera)
	*/
	--Variables
	DECLARE @TasaCupon NUMERIC(22,7), @TasaDiaria NUMERIC(22,7),@BaseCuponDivision NUMERIC(22,7),@BaseCupon NUMERIC(22,7),@MaximaSecuencia INT,
	@ValorActual NUMERIC(22,7),@DiferenciDiasUltimoCupon int, @CuponCorrido NUMERIC(22,7), @ValorMercado NUMERIC(22,7),@PrecioLimpio  NUMERIC(22,7),
	@PrecioSucio NUMERIC(22,7), @FechaUltimaCupon NUMERIC(8), @DiasPago NUMERIC(22,7), @BaseCalculoDiferenciaDias  NUMERIC(22,7),
	@Categoria VARCHAR(10), @Secuencia Int, @AmortizaVencimiento CHAR(1),@CodigoTipoCupon VARCHAR(2)
	/*
	OT10795 - 06/10/2017
	--@MontoNominalSinAmorizacion NUMERIC(22,7)
	--SET @MontoNominalSinAmorizacion = @P_MontoNominal
	*/
	--@BaseCalculoDiferenciaDias / Indica si en el calculo de diferencias se consideran solo 30 o mas dias al mes
	--Datos de la emision	
	SELECT @TasaCupon = V.TasaCupon/100, @BaseCupon = V.BaseCupon,@BaseCalculoDiferenciaDias = V.BaseCuponDias , @CodigoTipoCupon = V.CodigoTipoCupon,
	@Categoria = CI.Categoria 
	FROM Valores V
	JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS  --10090
	JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento --10090
	 WHERE Codigonemonico = @p_codigoMnemonico
	--Cupon Actual
	SELECT TOP 1 @DiferenciDiasUltimoCupon =
	CASE WHEN @BaseCalculoDiferenciaDias = 360 THEN 
		dbo.DATEDIFF360(FechaInicio,@p_FechaLiquidacion,1) 
	ELSE
		Datediff(Day, Convert(Datetime, convert(CHAR(8),FechaInicio ),111), convert(datetime, convert(CHAR(8), @p_FechaLiquidacion ), 111))
	END,
	@FechaUltimaCupon = FechaInicio, @DiasPago = DiasPago, @Secuencia = Secuencia
	FROM CuponeraNormal WHERE CodigoNemonico = @p_codigoMnemonico AND FechaTermino > @p_FechaOperacion
	ORDER BY convert(int,secuencia) ASC
	--Ultimo cupon
	SELECT TOP 1 @MaximaSecuencia =Secuencia , @AmortizaVencimiento = CASE WHEN Amortizacion = 100 THEN  '1' ELSE '0' END	
	FROM CuponeraNormal WHERE CodigoNemonico = @p_codigoMnemonico AND FechaTermino > @p_FechaOperacion 
	ORDER BY convert(int,secuencia) DESC --10090
	--
	IF @p_TipoTasa = '1'
		SET @p_TasaAnual = (POWER((1+((@p_TasaAnual/100)/(@BaseCupon/@DiasPago))),(@BaseCupon/@DiasPago))-1) * 100
	--Tasa diaria en base a la tasa anual Ingresada
	SET @BaseCuponDivision = 1 / @BaseCupon
	SET @TasaDiaria = (POWER((1 + @p_TasaAnual/100) ,@BaseCuponDivision) - 1)
	--Tabla de proceso
	CREATE TABLE #tmpFlujoCupon(Secuencia int,FlujoEfectivo NUMERIC(22,7), DiferenciaDias NUMERIC(36,18),DiferenciaDiasUltimoCupon NUMERIC(36,18),
	ValorActual NUMERIC(36,18), Amortizacion NUMERIC(22,7), Fc NUMERIC(22,7),VaFc NUMERIC(22,7),Interes NUMERIC(22,7)) --10090
	--Cupon
	INSERT INTO #tmpFlujoCupon
	SELECT Secuencia,FlujoEfectivo = 
	CASE WHEN @Categoria = 'PC' OR (@Categoria = 'CD' AND @CodigoTipoCupon ='3') THEN @P_MontoNominal ELSE
		CASE WHEN @MaximaSecuencia = Secuencia THEN
			--Si el cupon amortiza el 100% vencimiento, se suma el total del nominal
			--Caso contrario se resta al nominal lo amortizado en cupones anteriores
			CASE WHEN @AmortizaVencimiento = '1' THEN
				@P_MontoNominal + (@P_MontoNominal * @TasaCupon) / (@BaseCupon/@DiasPago) 
			ELSE
				--(dbo.fn_SumaAmortizacion(CodigoNemonico , @MontoNominalSinAmorizacion,Secuencia )  * @TasaCupon) / (@BaseCupon/@DiasPago)
				(@P_MontoNominal * @TasaCupon) / (@BaseCupon/@DiasPago)--OT10795
			END
		ELSE
			--(dbo.fn_SumaAmortizacion(CodigoNemonico , @MontoNominalSinAmorizacion,Secuencia )  * @TasaCupon) / (@BaseCupon/@DiasPago)
			(@P_MontoNominal * @TasaCupon) / (@BaseCupon/@DiasPago)--OT10795
		END
	END, --10090
	DiferenciaDias = CASE WHEN @BaseCalculoDiferenciaDias = 360 THEN 
		dbo.DATEDIFF360(@p_FechaLiquidacion,FechaTermino,1)
	ELSE
		Datediff(Day, Convert(Datetime, convert(CHAR(8), @p_FechaLiquidacion),111), convert(datetime, convert(CHAR(8), FechaTermino), 111)) 
	END,
	DiferenciaDiasUltimoCupon = CASE WHEN @BaseCalculoDiferenciaDias = 360 THEN 
		dbo.DATEDIFF360( @FechaUltimaCupon, FechaTermino,1)
	ELSE
		Datediff(Day, Convert(Datetime, convert(CHAR(8), @FechaUltimaCupon),111), convert(datetime, convert(CHAR(8), FechaTermino), 111))
	END, ValorActual = 0--, Amortizacion = @P_MontoNominal * (Amortizacion /100), 0,0,0
	,Amortizacion = @P_MontoNominal,0,0,0 --OT10795
	FROM CuponeraNormal
	WHERE CodigoNemonico = @p_codigoMnemonico  AND FechaTermino > @p_FechaOperacion 
	ORDER BY convert(INT,secuencia)
	--Actualizar
	UPDATE #tmpFlujoCupon SET Interes = @P_MontoNominal * (@TasaCupon) --10090
	UPDATE #tmpFlujoCupon SET FC = FlujoEfectivo + Amortizacion, ValorActual = FlujoEfectivo / POWER(1 + @TasaDiaria,DiferenciaDias) --10090
	UPDATE #tmpFlujoCupon SET VaFc = Fc / POWER(1 + @TasaDiaria,DiferenciaDias)
	--Calculos Finales
	IF @AmortizaVencimiento = '1'
		SELECT @ValorActual = SUM(ValorActual ) FROM #tmpFlujoCupon
	ELSE
		SELECT @ValorActual = SUM(VaFc) FROM #tmpFlujoCupon
	--Solo si es cd a la par
	IF (@Categoria = 'CD' AND @CodigoTipoCupon ='5')
		SELECT TOP 1 @CuponCorrido = (@DiferenciDiasUltimoCupon / DiferenciaDiasUltimoCupon ) * Interes FROM #tmpFlujoCupon ORDER BY Secuencia ASC
	ELSE
		SELECT TOP 1 @CuponCorrido = (@DiferenciDiasUltimoCupon / DiferenciaDiasUltimoCupon ) * FlujoEfectivo  FROM #tmpFlujoCupon ORDER BY Secuencia ASC
	--
	SET @ValorMercado = @ValorActual - @CuponCorrido
	--El nominal se ve afectado por los poncetajes ya amortizados
	/*
	OT10795 - 06/10/2017
	IF @Categoria = 'BO'
		--SET @P_MontoNominal =  dbo.fn_SumaAmortizacion(@p_codigoMnemonico , @P_MontoNominal ,@Secuencia ) --10090
		--SET @P_MontoNominal = @MontoNominalSinAmorizacion * (dbo.CalcularSumaAmortizacionPendiente(@p_FechaOperacion,@p_codigoMnemonico)/100) --OT10795
	*/
	--Calculo de precios
	SET @PrecioLimpio = ROUND((@ValorMercado / @P_MontoNominal) * 100,7)
	SET @PrecioSucio = ROUND((@ValorActual / @P_MontoNominal) * 100,7)
	SELECT @PrecioLimpio PrecioLimpio,@PrecioSucio PrecioSucio,@CuponCorrido CuponCorrido, @ValorActual ValorActual --10090
	DROP TABLE #tmpFlujoCupon
END

GO

GRANT EXECUTE ON [dbo].[OrdenInversion_CalcularPrecioBono] TO [rol_sit_fondos] AS [dbo]
GO





PRINT '[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Mandatos_ObtenerPrecioSucio_ValorCuota]') AND type in (N'P', N'PC'))
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
		WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
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






PRINT '[dbo].[sp_SIT_Gen_Carta_OperacionForward]'
USE [SIT-FONDOS]
GO
/*
exec sp_SIT_Gen_Carta_OperacionForward @p_CodigoOperacionCaja=N'064445'
*/
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_Gen_Carta_OperacionForward]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionForward]
GO
-------------------------------------------------------------------------------------------------------------------------
-- Objetivo: OBTIENE LOS DATOS PARA GENERAR LAS CARTAS DE OPERACIONES DE FORWARD
-------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 17/01/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11732
-- Descripcion del cambio: Nuevo
-------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 09/05/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11964
-- Descripcion del cambio: La fecha de carta se muestra la fecha de negociación de la inversión
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_Gen_Carta_OperacionForward]
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
	P.CodigoFondosMutuos,
	NombrePortafolio = P.Descripcion,
	NombreCompletoPortafolio = P.NombreCompleto,
	FechaCarta = dbo.RetornarFechaCompleta(OI.FechaOperacion),
	Banco = CASE WHEN P.IndicadorFondo  = 'S' THEN @BancoPublico  ELSE T.Descripcion END, 
	OI.CodigoOperacion,
	O.Descripcion DescripcionOperacion, 
	T.Descripcion DescripcionIntermediario ,
	ModeloCarta = (SELECT 	MC.Descripcion FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('OFWC','OFWV') )  , 
	OI.CodigoMoneda,
	OI.TipoCambioFuturo,
	OI.TipoCambioSpot,
	OI.MontoOperacion,
	OI.MontoCancelar,
	OI.MontoNominalOperacion,
	OI.MontoNominalOrdenado,
	FechaOperacion   = DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion),
	FechaLiquidacion = DBO.FN_SIT_OBT_FechaFormateada(OI.FechaLiquidacion),
	OI.Plazo,
	Modalidad = CASE WHEN OI.Delibery  = 'S' THEN 'Delivery Forward' ELSE 'No Delivery Forward' END, 
	OI.CodigoOrden NumeroOrden,
  	NombreUsuarioF1 = ISNULL(DBO.RetornarNombrePersonal(FC.VBGERF1),''),
	NombreUsuarioF2 = ISNULL(DBO.RetornarNombrePersonal(FC.VBGERF2),''),
	CargoUsuarioF1 = ISNULL(dbo.RetornarCargoPersonal(FC.VBGERF1),'') ,
	CargoUsuarioF2 = ISNULL(dbo.RetornarCargoPersonal(FC.VBGERF2),''),
	FC.VBGERF1 CodigoUsuarioF1 ,FC.VBGERF2  CodigoUsuarioF2,
	P.CodigoPortafolioSBS
	FROM OrdenInversion OI
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero  --BANCO INTERMEDIARIO
	LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
	WHERE OI.CodigoOrden = @p_CodigoOperacionCaja-- IN ('043273','054927')
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_Gen_Carta_OperacionForward] TO [rol_sit_fondos] AS [dbo]
GO






PRINT '[dbo].[sp_SIT_lis_InconsisteciasValorizacion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_lis_InconsisteciasValorizacion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_lis_InconsisteciasValorizacion]
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo:	Listar inconsistencias para el fondo valorizado
-----------------------------------------------------------------------------------------------------------
-- Fecha Creaciòn: 08/05/2017
-- Creado por: Carlos Espejo
-- Nro. Orden de Trabajo: 10328
-- Descripcion del cambio: Creación
-----------------------------------------------------------------------------------------------------------
-- Fecha Creación: 26/03/2018
-- Creado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11223
-- Descripcion del cambio: Cambio de tabla temporal física a una tabla temporal en memoria
-----------------------------------------------------------------------------------------------------------
-- Fecha Creación: 07/05/2019
-- Creado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11964
-- Descripcion del cambio: Filtrar las operaciones forward en estado ejecutado y confirmado
-----------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_lis_InconsisteciasValorizacion] @p_CodigoPortafolio VarChar(20), @p_FechaOperacion NUMERIC(8)
AS
BEGIN
	CREATE TABLE #tmpValidadorValorizacion(Correlativo INT IDENTITY,Nemonico VarChar(20),ISINSBS VarCHar(20),TipoInstrumentoSBS VarChar(20), VpnOriginal Numeric(22,2),
	Categoria VarChar(20))
	--Instrumentos
	INSERT INTO #tmpValidadorValorizacion (Nemonico ,ISINSBS,TipoInstrumentoSBS,VpnOriginal,Categoria)
	SELECT CTV.CodigoMnemonico,V.CodigoISIN,ISNULL(V.CodigoTipoInstrumentoSBS,''),CTV.VPNOriginal,CI.Categoria FROM CarteraTituloValoracion CTV
	JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
	JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS  = V.CodigoTipoInstrumentoSBS  
	JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento AND CI.Categoria NOT IN ('DP','OR')
	WHERE CTV.FechaValoracion = @p_FechaOperacion AND CTV.CodigoPortafolioSBS = @p_CodigoPortafolio AND CTV.VPNMonedaFondo > 0
	--Titulos Unicos
	INSERT INTO #tmpValidadorValorizacion (Nemonico ,ISINSBS,TipoInstrumentoSBS,VpnOriginal,Categoria)
	SELECT CTV.CodigoMnemonico,CTV.CodigoSBS,ISNULL(V.CodigoTipoInstrumentoSBS,''),VPNMonedaFondo,CI.Categoria FROM CarteraTituloValoracion CTV
	JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
	JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS  = V.CodigoTipoInstrumentoSBS  
	JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento AND CI.Categoria IN ('DP','OR')
	WHERE CTV.FechaValoracion  = @p_FechaOperacion AND CTV.CodigoPortafolioSBS = @p_CodigoPortafolio AND CTV.VPNMonedaFondo > 0
	--Forward con problemas en valorizacion
	INSERT INTO #tmpValidadorValorizacion (Nemonico ,ISINSBS,TipoInstrumentoSBS,VpnOriginal,Categoria)
	SELECT OI.CodigoMnemonico,OI.CodigoISIN,V.CodigoTipoInstrumentoSBS ,ISNULL(VF.MtmUSD,0),'FD' FROM VectorForwardSBS VF
	JOIN OrdenInversion OI ON OI.CodigoISIN = VF.NumeroPoliza AND OI.CodigoPortafolioSBS = @p_CodigoPortafolio AND OI.CodigoPortafolioSBS = @p_CodigoPortafolio
		AND OI.Estado IN ('E-EJE','E-CON')
	JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico
	WHERE VF.Fecha = @p_FechaOperacion AND (MtmUSD IS NULL OR MtmUSD = 0)
	--	
	SELECT Nemonico ,ISINSBS,TipoInstrumentoSBS,VpnOriginal,Categoria FROM #tmpValidadorValorizacion WHERE VpnOriginal = 0 OR TipoInstrumentoSBS = ''
	--
	DROP TABLE #tmpValidadorValorizacion
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_lis_InconsisteciasValorizacion] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_SIT_SeleccionCartas]'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM SYS.procedures WHERE name = 'sp_SIT_SeleccionCartas')
	DROP PROCEDURE [dbo].[sp_SIT_SeleccionCartas]
GO
----------------------------------------------------------------------------------------------------------
--Objetivo: Listar las cartas y su estado
----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 02/12/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9678
--	Descripcion del cambio: Nuevo Formato para cartas
----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 21/02/2017
-- Modificado por: Carlos Espejo
-- Nro. Orden de Trabajo: 10025
-- Descripcion del cambio: Se agrega al codigo del portafolio a la consulta
----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 24/03/2017
-- Modificado por: Carlos Espejo
-- Nro. Orden de Trabajo: 10150
-- Descripcion del cambio: Se agrega el campo de carrelativo de cartas
----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/03/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10200
--	Descripcion del cambio: Se regulariza el pase
----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 17/07/2018
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 11432
--	Descripcion del cambio: Incluir cartas de compra y venta de acciones
----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 17/07/2018
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 11732
--	Descripcion del cambio: Incluir cartas de compra y venta de acciones
----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 09/05/2019
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11964
--	Descripcion del cambio: Incluir las cartas de constituciones forward en estado ejecutado
----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_SeleccionCartas]
	@p_CodigoMercado VARCHAR(30) = '',
	@p_CodigoPortafolio  VARCHAR(50) = '', --10025
	@p_CodigoTercero VARCHAR(15) = '', --BANCO INTERMEDIARIO
	@p_CodigoTerceroBanco VARCHAR(15) = '', --BANCO
	@p_Fecha NUMERIC(8) = 0,
	@p_EstadoCarta VARCHAR(1) = '',
	@p_CodigoOperacionCaja VARCHAR(7) = '',
	@p_CodigoCartaAgrupado INT
AS
BEGIN

	DECLARE @BancoPublico VarChar(100)

	SELECT 
	@BancoPublico = 
	T.Descripcion From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	WHERE Clasificacion = 'BANFONPUB'

	SELECT * FROM (
	SELECT P.Descripcion DescripcionPortafolio, O.Descripcion DescripcionOperacion, MC.Descripcion ModeloCarta , T.Descripcion DescripcionIntermediario ,OC.CodigoMoneda,
	OC.Importe, ISNULL(OC.CodigoOrden , OC.CodigoOperacionCaja) NumeroOrden,
	LTRIM(RTRIM(ISNULL(P1.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoMaterno,''))) VBADMIN,
	LTRIM(RTRIM(ISNULL(P2.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoMaterno,''))) VBGERF1,
	LTRIM(RTRIM(ISNULL(P3.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoMaterno,''))) VBGERF2,
	OC.CodigoOperacion,OC.CodigoOperacionCaja,OC.EstadoCarta, OC.CodigoModelo,P.CodigoPortafolioSBS,ISNULL(OC.CorrelativoCartas,0) CorrelativoCartas --10150
	,OC.NumeroCuenta, TB.Descripcion AS Banco, ISNULL(OC.CodigoCartaAgrupado,0) AS CodigoCartaAgrupado
	FROM OperacionesCaja OC
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS
	JOIN Operacion O ON O.CodigoOperacion = OC.CodigoOperacion 
	JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
	JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta AND CE.Situacion = 'A' AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera AND E.Situacion = 'A' AND
		E.CodigoTercero = CASE WHEN @p_CodigoTerceroBanco = '' THEN E.CodigoTercero ELSE @p_CodigoTerceroBanco END
	JOIN Terceros TB ON E.CodigoTercero = TB.CodigoTercero AND E.Situacion = 'A'
	JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo  AND MC.Situacion = 'A'
	LEFT JOIN Personal P1 ON P1.CodigoInterno = OC.VBADMIN
	LEFT JOIN Personal P2 ON P2.CodigoInterno = OC.VBGERF1
	LEFT JOIN Personal P3 ON P3.CodigoInterno = OC.VBGERF2
	WHERE OC.CodigoModelo <> 'SC01' 
	AND OC.EstadoCarta   = CASE WHEN  @p_EstadoCarta = '' THEN OC.EstadoCarta ELSE @p_EstadoCarta END  
	AND OC.CodigoMercado = CASE WHEN  @p_CodigoMercado = '' THEN OC.CodigoMercado  ELSE @p_CodigoMercado END
	AND OC.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OC.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END --10025
	AND OC.CodigoTerceroOrigen = CASE WHEN @p_CodigoTercero = '' THEN OC.CodigoTerceroOrigen  ELSE @p_CodigoTercero END
	AND OC.CodigoOperacionCaja = CASE WHEN @p_CodigoOperacionCaja = '' THEN OC.CodigoOperacionCaja  ELSE @p_CodigoOperacionCaja END
	AND OC.FechaOperacion = @p_Fecha AND OC.CodigoOperacion NOT IN ('63','BCRE')  
	AND (OC.CodigoCartaAgrupado = @p_CodigoCartaAgrupado OR @p_CodigoCartaAgrupado = 0)

	UNION ALL

	SELECT 
	P.Descripcion DescripcionPortafolio, 
	O.Descripcion DescripcionOperacion, 
 	ModeloCarta = CASE WHEN  OI.CategoriaInstrumento = 'FD' THEN  (SELECT MC.Descripcion FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('OFWC','OFWV') ) ELSE 'OPERACION DE ACCIONES' END  , 
	T.Descripcion DescripcionIntermediario ,
	OI.CodigoMoneda,
	Importe = CASE WHEN  OI.CategoriaInstrumento = 'FD' THEN  OI.MontoCancelar ELSE OI.MontoNetoOperacion END,   
	OI.CodigoOrden NumeroOrden,
	VBADMIN = ISNULL(DBO.RetornarNombrePersonal(FC.VBADMIN),''),
  	VBGERF1 = ISNULL(DBO.RetornarNombrePersonal(FC.VBGERF1),''),
	VBGERF2 = ISNULL(DBO.RetornarNombrePersonal(FC.VBGERF2),''),
 	OI.CodigoOperacion,
	OI.CodigoOrden AS CodigoOperacionCaja,
	ISNULL((SELECT EstadoCarta FROM ClaveFirmantesCarta_OI FC 
				WHERE FC.CodigoOrden = OI.CodigoOrden AND
				 FC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS),'1') AS EstadoCarta, 
    CodigoModelo = CASE WHEN  OI.CategoriaInstrumento = 'FD' THEN (SELECT MC.CodigoModelo FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('OFWC','OFWV') ) ELSE 'OIAC' END,
	P.CodigoPortafolioSBS,
	0 AS CorrelativoCartas, 
	'' AS NumeroCuenta, 
	Banco = CASE WHEN P.IndicadorFondo  = 'S' THEN @BancoPublico  ELSE T.Descripcion END, 
    ISNULL(CodigoCartaAgrupado,0) AS CodigoCartaAgrupado
	FROM OrdenInversion OI
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero  --BANCO INTERMEDIARIO
	LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
	WHERE
	--(OI.Estado ='E-EJE' OR OI.Estado ='E-CON') AND
	--OI.CodigoMnemonico = 'forward'   AND
	OI.Estado ='E-EJE' AND
	--(OI.CategoriaInstrumento='FD' OR  OI.CategoriaInstrumento = 'AC') AND
	OI.CategoriaInstrumento='FD' AND
	OI.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END AND
	OI.FechaOperacion = @p_Fecha AND
	OI.CodigoTercero  = CASE WHEN @p_CodigoTercero = '' THEN OI.CodigoTercero  ELSE @p_CodigoTercero END AND
	OI.CodigoOrden    = CASE WHEN @p_CodigoOperacionCaja = '' THEN OI.CodigoOrden  ELSE @p_CodigoOperacionCaja END 
	AND (OI.CodigoCartaAgrupado = @p_CodigoCartaAgrupado OR @p_CodigoCartaAgrupado = 0)

) AS TT
WHERE   TT.EstadoCarta   = CASE WHEN  @p_EstadoCarta = '' THEN TT.EstadoCarta ELSE @p_EstadoCarta END  
	 ORDER BY ISNULL(TT.CorrelativoCartas,0) ASC , ISNULL(TT.CodigoCartaAgrupado,0) ASC

END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_SeleccionCartas] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[pr_SIT_pro_CalcularComisiones_OrdenInversionRF]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='pr_SIT_pro_CalcularComisiones_OrdenInversionRF') BEGIN 
	DROP PROCEDURE [dbo].[pr_SIT_pro_CalcularComisiones_OrdenInversionRF]
END 
GO
/****** Object:  StoredProcedure [dbo].[pr_SIT_pro_CalcularComisiones_OrdenInversionRF]    Script Date: 29/04/2019 11:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Objetivo: Cálcula las comisiones en proceso masivo de Ejecutar OI para bonos locales
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 19/06/2018
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: ProyFondosII - RF012
--  OT: 11512
--	Descripción del cambio: Cálcula las comisiones en proceso masivo de Ejecutar OI para bonos locales
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 13/05/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 
--  OT: 11964
--	Descripción del cambio: Cálcula las comisiones en proceso masivo de Ejecutar OI para papeles comerciales
-----------------------------------------------------------------------------------------------------------
create PROCEDURE [dbo].[pr_SIT_pro_CalcularComisiones_OrdenInversionRF]
@p_CodigoOrden varchar(12),
@p_CodigoRenta varchar(1)
AS
SET NOCOUNT ON

declare @CodigoTercero varchar(11)
declare @CodigoPortafolioSBS varchar(10)
declare @GrupoIntermediario varchar(10)
declare @CantidadOperacion numeric(22)
declare @MontoOperacion decimal(22,7)
declare @UsuarioCreacion varchar(15)
declare @FechaCreacion numeric(8)
declare @HoraCreacion varchar(10)
declare @Host varchar(20)

declare @Mercado varchar(3)

declare @TotalComisiones decimal(22,7)
declare @ValorComision decimal(22,7)
declare @TipoImpuesto varchar(10)
declare @CodigoComision varchar(20)
declare @TipoTarifa varchar(4)  -- RCE | Proyecto: FondosII | Variable que almacena la el tipo de Tarifa | 19/06/2018

declare @cantidad decimal(22,7)
declare @MontoNetoOperacion decimal(22,7)
declare @CalculoIGV decimal(22,7)
declare @PrecioPromedioOp decimal(22,7)

declare @MontoComision decimal(22,7)	--JHC REQ 66056: Implementacion Futuros
declare @Plaza varchar(3)
declare @codigoMoneda varchar(10)
declare @codigoMonedaOI varchar(10)
declare @factorMontoBase decimal(22,7)
declare @fechaOperacion numeric(8)
declare @tipoCambio numeric(22,7)
declare @generaImpuestos bit
declare @totalComisionNoIGV decimal(22,7)
declare @CategoriaInstrumento varchar(2)

create table #TmpIntermediarios
(
	CodigoTercero varchar(20),
	Descripcion varchar(150),
	codigoCustodio varchar(15),
	mercado varchar(3),
	codigosectorempresarial varchar(15),
	codigoentidad varchar(4)
)

create table #TmpValorComisionCalculado
(
	CodigoComision varchar(20),
	ValorComision numeric(22,7)
)
create table #TmpCantidadImpComisionOI
(
	cantidad numeric(22)
)

CREATE TABLE #TmpImpComisiones
(
	CodigoComision varchar(20),
	ValorComision numeric(22,7),
	TipoImpuesto varchar(1),
	TipoTarifa varchar(4),
	CodigoMoneda varchar(10),
	TipoCambioOrden decimal(22,7),
	GeneraImpuestos bit
)
--OBTENER DATOS DE LA ORDEN DE INVERSION
SELECT @CodigoTercero = CodigoTercero, 
@CodigoPortafolioSBS = CodigoPortafolioSBS,
@GrupoIntermediario = GrupoIntermediario,
@CantidadOperacion = CantidadOperacion,
@MontoOperacion = MontoOperacion,
@UsuarioCreacion = UsuarioCreacion,
@FechaCreacion = FechaCreacion,
@HoraCreacion = HoraCreacion,
@Host = Host,
@Plaza = CodigoPlaza
,@codigoMonedaOI = CodigoMoneda
,@fechaOperacion = FechaOperacion
,@CategoriaInstrumento = CategoriaInstrumento
FROM OrdenInversion 
WHERE CodigoOrden = @p_CodigoOrden

IF @Plaza = 7 AND @CategoriaInstrumento in ('BO','LH','PC') BEGIN  -- RCE | Proyecto: FondosII | Validamos que el mercardo sea diferente a Lima (7) | 19/06/2018
	--JHC REQ 66056: Implementacion Futuros
	SELECT	@TotalComisiones = 0,
			@CalculoIGV = 0,
			@MontoComision = 0,
			@totalComisionNoIGV = 0

	SET @MontoNetoOperacion = @TotalComisiones + @MontoOperacion
	SET @PrecioPromedioOp = @MontoNetoOperacion / @CantidadOperacion

	INSERT INTO #TmpIntermediarios
	EXEC Tercero_ListarPorGrupoIntermediario 'I', @GrupoIntermediario

	SELECT @Mercado = mercado
	FROM #TmpIntermediarios
	WHERE CodigoTercero = @CodigoTercero

	--OBTENER LA ESTRUCTURA DE LA TABLA DEL IMPUESTO DE COMISIONES
	--INI JHC REQ 66056: Implementacion Futuros
		INSERT INTO #TmpImpComisiones
		SELECT	CodigoComision,   
				CONVERT(NUMERIC(22,7),(CASE WHEN TipoImpuesto = 'P' THEN ValorComision / 100 ELSE ValorComision END)) ValorComision,
				TipoImpuesto,
				TipoTarifa, -- RCE | Proyecto: FondosII | Se agrega para diferenciar el tipo de tarifa y calcular la comisión en base a Unidad (U) o Valor Nominal (N) | 19/06/2018
				CodigoMoneda,
				TipoCambioOrden = (CASE WHEN ISNULL(CodigoMoneda, @codigoMonedaOI) NOT IN (@codigoMonedaOI,'')
										 THEN ISNULL(dbo.RetornarTipoCambio('DOL', @fechaOperacion),1) 
										 ELSE 1 END),
				GeneraImpuestos
		FROM	impuestosComisiones
		WHERE	CodigoTipoRenta = @p_CodigoRenta
			AND	CodigoPlaza = @Plaza
			AND Situacion = 'A'

		DECLARE ImpuestoComision CURSOR 
		FOR
		SELECT [CodigoComision], [ValorComision], [TipoImpuesto], [TipoTarifa],[CodigoMoneda],[TipoCambioOrden],[GeneraImpuestos] FROM #TmpImpComisiones
		OPEN ImpuestoComision

		FETCH ImpuestoComision INTO @CodigoComision, @ValorComision, @TipoImpuesto, @TipoTarifa , @codigoMoneda, @tipoCambio, @generaImpuestos
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @CodigoComision <> 'P I.G.V. TOT'
			BEGIN
				SET @MontoComision = 0
				IF @TipoTarifa = 'M' SET @factorMontoBase = @MontoOperacion
				ELSE IF @TipoTarifa = 'F' BEGIN
						IF @codigoMoneda LIKE '%SOL%' SET @factorMontoBase = 1 / @tipoCambio 
						ELSE SET @factorMontoBase = 1 * @tipoCambio 
				   END
				ELSE IF @TipoTarifa = 'C' SET @factorMontoBase = 1 
				
				SET @MontoComision = @factorMontoBase * @ValorComision
				
				INSERT INTO #TmpValorComisionCalculado (CodigoComision, ValorComision) 
				VALUES (@CodigoComision, @MontoComision )				
				
				IF @TipoTarifa <> 'C' AND @generaImpuestos = 1 SET @TotalComisiones = @TotalComisiones + @MontoComision	
				ELSE SET @totalComisionNoIGV = @totalComisionNoIGV + @MontoComision
			END
			FETCH ImpuestoComision INTO @CodigoComision, @ValorComision, @TipoImpuesto, @TipoTarifa, @codigoMoneda, @tipoCambio, @generaImpuestos
		END
		CLOSE ImpuestoComision
		DEALLOCATE ImpuestoComision
		 
		--CALCULO DE LA COMISION DEL IGV
		IF (SELECT Count(1) FROM #TmpImpComisiones WHERE CodigoComision = 'P I.G.V. TOT') > 0
		BEGIN
			SELECT TOP 1 @ValorComision = ValorComision FROM #TmpImpComisiones WHERE CodigoComision = 'P I.G.V. TOT'
			SET @CalculoIGV = @TotalComisiones * @ValorComision  -- RCE | Proyecto: FondosII | En caso el instrumento es Acciones se descontarà la comisiòn de CONTRIBUCIÓN CONASEV | 19/06/2018  
			INSERT INTO #TmpValorComisionCalculado (CodigoComision, ValorComision)
			VALUES ('P I.G.V. TOT', @CalculoIGV )
		END
		SET @TotalComisiones = @TotalComisiones + @CalculoIGV + @totalComisionNoIGV

	 SET @MontoNetoOperacion = @TotalComisiones + @MontoOperacion      
	 SET @PrecioPromedioOp = @MontoNetoOperacion / @CantidadOperacion  

	UPDATE OrdenInversion 
	SET MontoNetoOperacion = @MontoNetoOperacion,
	PrecioPromedio = @PrecioPromedioOp,
	TotalComisiones = @TotalComisiones
	WHERE CodigoOrden = @p_CodigoOrden
	  
	DECLARE ImpuestoComisionesOI CURSOR 
	FOR
	SELECT CodigoComision, ValorComision FROM #TmpValorComisionCalculado
	OPEN ImpuestoComisionesOI

	FETCH ImpuestoComisionesOI INTO @CodigoComision, @ValorComision
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO #TmpCantidadImpComisionOI
		EXEC ImpuestosComisionesOrdenesPreordenes_VerificarExistencia @p_CodigoOrden, @CodigoPortafolioSBS, @CodigoComision, @p_CodigoRenta, @Mercado
		SELECT top 1 @cantidad = cantidad FROM #TmpCantidadImpComisionOI

		IF @cantidad > 0
		BEGIN
			EXEC ImpuestosComisionesOrdenesPreordenes_Modificar @p_CodigoOrden, @CodigoComision, @p_CodigoRenta, @Mercado, @CodigoPortafolioSBS, @ValorComision, @ValorComision, @UsuarioCreacion, @FechaCreacion, @HoraCreacion, @Host
		END
		ELSE
		BEGIN
			EXEC ImpuestosComisionesOrdenesPreordenes_Insertar @p_CodigoOrden, @CodigoPortafolioSBS, @CodigoComision, @ValorComision, @ValorComision, @UsuarioCreacion, @FechaCreacion, @HoraCreacion, @Host, @p_CodigoRenta, @Mercado, @Plaza
		END
		FETCH ImpuestoComisionesOI INTO @CodigoComision, @ValorComision
	END
	CLOSE ImpuestoComisionesOI
	DEALLOCATE ImpuestoComisionesOI

	drop table #TmpIntermediarios
	drop table #TmpValorComisionCalculado
	drop table #TmpCantidadImpComisionOI
	drop table #TmpImpComisiones
END

GO 
GRANT EXECUTE ON [dbo].[pr_SIT_pro_CalcularComisiones_OrdenInversionRF] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[pr_val_rf_amortizada_compras_en_stock]'

USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_val_rf_amortizada_compras_en_stock]') AND type in (N'P', N'PC'))
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
-------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 26/04/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11964
-- Descripcion del cambio: Agregar las negociaciones de Renta Variable para la valorización Amortizada
-------------------------------------------------------------------------------------------------------------------------
-- exec pr_val_rf_amortizada_compras_en_stock '854767', '20180829'
-- exec pr_val_rf_amortizada_compras_en_stock '999', '20180904'
-- exec pr_val_rf_amortizada_compras_en_stock '113', 20190101

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
					where TI.CodigoRenta in ('1','2') /* Renta Fija*/
					and TI.CodigoClaseInstrumento in 
					(select CodigoClaseInstrumento from ClaseInstrumento
						where Categoria in 
						(
							'BO',	/*Bonos*/
							'PC',	/*Papeles comerciales*/
							'CD',	/*Certificado de depósito*/	
							'LH',	/*Letras hipotecarias*/
							'DP',	/*Depósito a plazos*/
							'AC',	/*Acciones*/
							'FM',	/*Fondos Mutuos*/
							'FI'	/*Fondos de Inversión*/
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
		,(CASE WHEN v.TipoRenta = '1' THEN VP.PorcPrecioLimpio ELSE VP.ValorPrecio END) AS PorcentagePrecioLimpio
		,(CASE WHEN V.TipoRenta = '1' THEN VP.PorcPrecioSucio ELSE VP.ValorPrecio END) AS PorcentagePrecioSucio
		,v.TipoRenta AS TipoRenta
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

GRANT EXECUTE ON [dbo].[pr_val_rf_amortizada_compras_en_stock] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[dbo].[ReporteGestion_ReporteConsolidado]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ReporteGestion_ReporteConsolidado') BEGIN 
	DROP PROCEDURE [dbo].[ReporteGestion_ReporteConsolidado]
END 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
-- Descripcion del cambio: Modificacion campos
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
-- Fecha Modificación: 06/05/2019
-- Modificado por: Diego Tueros
-- Nro. Orden de Trabajo: 11964
-- Descripcion del cambio: Se comento las querys de renta variable
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
		--LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion = 'SECTOR_GIGS' AND T.SectorGigs=PGA.Valor
		LEFT JOIN ParametrosGenerales PG ON PG.Clasificacion = 'SECTOR_GIGS' AND V.Rating = PG.Valor
		INNER JOIN Portafolio P ON VA.CodigoPortafolioSBS = P.CodigoPortafolioSBS
	--WHERE VA.CodigoPortafolioSBS = '8' AND FechaProceso = 20180827
	WHERE VA.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND FechaProceso = @p_FechaProceso
	--ORDER BY VA.CodigoPortafolioSBS,P.Descripcion
	UNION
	--DEPËSITOS A PLAZO
	SELECT DISTINCT	CTV.CodigoPortafolioSBS,P.Descripcion AS DescripcionPortafolio
		,TI.CodigoTipoInstrumentoSBS,TI.Descripcion AS DescripcionTipoInstrumento
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, 'DOL', @p_FechaProceso) AS TipoCambio
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND @p_FechaProceso = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
	WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		--AND CTV.FechaValoracion = @p_FechaProceso
		AND OI.FechaContrato > @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
		/*
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
	*/
	/*
	SELECT
		OI.CodigoPortafolioSBS,P.Descripcion AS DescripcionPortafolio
		,TI.CodigoTipoInstrumentoSBS,TI.Descripcion AS DescripcionTipoInstrumento
		,dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, 'DOL',@p_FechaProceso) AS TipoCambio
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON CTV.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON CodigoOperacion IN ('1','2') AND @p_FechaProceso = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		--AND EXISTS(
		--	SELECT 1 FROM CarteraTituloValoracion CTV WHERE CTV.CodigoMnemonico = OI.CodigoMnemonico
		--	AND CTV.FechaValoracion = @p_FechaProceso AND CTV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS)
	ORDER BY 1,2
	*/
	
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
		,OI.FechaOperacion
	FROM ValorizacionAmortizada VA
		INNER JOIN OrdenInversion OI ON OI.CodigoOrden = VA.CodigoOrden AND OI.CodigoPortafolioSBS = VA.CodigoPortafolioSBS
		INNER JOIN Valores V ON VA.CodigoNemonico = V.CodigoNemonico
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		LEFT JOIN ParametrosGenerales PG ON PG.Clasificacion = 'RATING' AND V.Rating = PG.Valor
		--LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion LIKE 'sector%' AND T.SectorGigs=PGA.Valor
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion = 'SECTOR_GIGS' AND T.SectorGigs=PGA.Valor
		INNER JOIN Portafolio P ON VA.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN VectorPrecio VP ON VA.CodigoNemonico = VP.CodigoMnemonico AND VP.Fecha = @p_FechaProceso
			AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(VA.CodigoPortafolioSBS,VA.CodigoNemonico,@p_FechaProceso)
	WHERE VA.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND VA.FechaProceso = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
	UNION
	--DEPËSITOS A PLAZO
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
		,OI.FechaOperacion		
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND @p_FechaProceso = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
		INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		--LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion LIKE 'sector%' AND T.SectorGigs=PGA.Valor
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion = 'SECTOR_GIGS' AND T.SectorGigs=PGA.Valor
	WHERE CTV.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		--AND CTV.FechaValoracion = @p_FechaProceso
		AND OI.FechaContrato > @p_FechaProceso AND OI.FechaOperacion <= @p_FechaProceso
	/*
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
		,FechaVencimiento = @p_FechaProceso
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
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON CTV.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON CodigoOperacion IN ('1','2') AND @p_FechaProceso = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion = 'SECTOR_GIGS' AND T.SectorGigs=PGA.Valor
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
	ORDER BY 3,2
	*/
	/*
	FROM OrdenInversion OI
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		INNER JOIN Moneda M ON M.CodigoMoneda = V.CodigoMoneda
		LEFT JOIN ParametrosGenerales PGA ON PGA.Clasificacion = 'SECTOR_GIGS' AND T.SectorGigs=PGA.Valor
	WHERE OI.CodigoPortafolioSBS IN (SELECT splitdata FROM dbo.fnSplitString(@p_CodigoPortafolio,','))
		AND OI.FechaOperacion = @p_FechaProceso AND OI.Estado IN ('E-EJE','E-CON')
		AND EXISTS(
			SELECT 1 FROM CarteraTituloValoracion CTV WHERE CTV.CodigoMnemonico = OI.CodigoMnemonico
			AND CTV.FechaValoracion = @p_FechaProceso AND CTV.CodigoPortafolioSBS = OI.CodigoPortafolioSBS)
	ORDER BY 3,2
	*/
		
END
GO

GRANT EXECUTE ON [dbo].[ReporteGestion_ReporteConsolidado] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM SYS.procedures WHERE name = 'CuentasPorCobrar_LiquidarDivisas')
	DROP PROCEDURE [dbo].[CuentasPorCobrar_LiquidarDivisas]
GO
---------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: Liquidar las CXC O CXP de divisas
---------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 17/01/2019
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11732
--	Descripcion del cambio: Se agregó el campo "@p_ObservacionCartaDestino" para la generación de cartas de instrucción de divisas
---------------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 09/05/2019
--	Modificado por: Ian Pastor Mendoza.
--	Nro. Orden de Trabajo: 11964
--	Descripcion del cambio: Se agregó el campo "@p_BancoGlosaDestino" para la generación de cartas de instrucción
---------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CuentasPorCobrar_LiquidarDivisas]
	@p_NroOperacion VARCHAR(20),
	@p_NumeroCuenta VARCHAR(25),
	@p_CodigoPortafolio VARCHAR(10),
	@p_FechaPago NUMERIC(10)=0,
	@p_OperacionCaja int=1,
	@p_Importe NUMERIC(22,7) = 0,
	@p_CodigoModalidadPago VARCHAR(4)='',
	@p_usuario VARCHAR(15),
	@p_fecha NUMERIC(8,0),
	@p_Hora VARCHAR(10),
	@p_Host VARCHAR(20),
	@p_NumeroCarta VARCHAR(20)='',
	@nvcCodigoModelo NVARCHAR(4),
	@nvcCodigoMoneda NVARCHAR(4),
	@nvcCodigoContacto VARCHAR(3)='',
	@p_BancoMatrizOrigen VARCHAR(12)='',
	@p_BancoMatrizDestino VARCHAR(12)='',
	@p_CodigoContactoIntermediario VARCHAR(3)=null,
	@p_agrupado CHAR(1)=null,
	@p_ObservacionCartaDestino VARCHAR(1000),
	@p_BancoGlosaDestino VARCHAR(20)
AS

	DECLARE	@CodigoBanco VARCHAR(10),
			@CodigoClaseCuenta VARCHAR(3),
			@EstadoCarta VARCHAR(1),
			@CodigoOrden VARCHAR(6),
			@SecuenciaCuponera NUMERIC(18,0),
			@CodigoNemonico VARCHAR(15),
			@nvcCodigoOperacion NVARCHAR(4),
			@spot VARCHAR(1)

	SET @spot = ISNULL((SELECT	delibery 
						FROM	ordeninversion 
						WHERE	codigoorden = @p_NroOperacion
							AND codigoportafoliosbs = @p_CodigoPortafolio),'S')

	IF @p_NumeroCarta = '' or @p_NumeroCarta is null
	BEGIN
		SET @p_NumeroCarta = null
		SET @EstadoCarta = null
	END
	ELSE
	BEGIN
		SET @EstadoCarta = 'P'
	END

	SELECT	@CodigoClaseCuenta = CodigoClaseCuenta
	FROM	CuentaEconomica
	WHERE	CodigoPortafolioSBS = @p_CodigoPortafolio
		AND	NumeroCuenta = @p_NumeroCuenta

	IF @p_OperacionCaja = 1
	BEGIN
		DECLARE @FechaOperacion NUMERIC(8),
				@CodigoOperacionCaja VARCHAR(7)
				
		SELECT	@FechaOperacion = FechaNegocio
		FROM	Portafolio
		WHERE	CodigoPortafolioSBS = @p_CodigoPortafolio

		SELECT
				@CodigoOperacionCaja = CASE
				WHEN SUBSTRING(NumeroOperacion,1,1)='M' THEN 'N'+SUBSTRING(NumeroOperacion,2,LEN(NumeroOperacion)-1)
				ELSE NumeroOperacion END,
				@CodigoOrden=CodigoOrden,
				@SecuenciaCuponera = SecuenciaCuponera,
				@CodigoNemonico = CodigoNemonico,
				@nvcCodigoOperacion = CodigoOperacion
		FROM	CuentasPorCobrarPagar
		WHERE	NumeroOperacion = @p_NroOperacion 
			AND	CodigoPortafolioSBS = @p_CodigoPortafolio

		INSERT INTO dbo.OperacionesCaja
		(
			CodigoOperacionCaja,
			CodigoMercado,
			CodigoClaseCuenta,
			NumeroCuenta,
			Referencia,
			Importe,
			UsuarioCreacion,
			FechaCreacion,
			CodigoMoneda,
			TipoMovimiento,
			Situacion,
			Host,
			NumeroCarta,
			EstadoCarta,
			HoraCreacion,
			NumeroCuentaDestino,
			UsuarioModificacion,
			CodigoClaseCuentaDestino,
			FechaModificacion,
			CodigoTerceroOrigen,
			HoraModificacion,
			CodigoModalidadPago,
			CodigoOperacion,
			CodigoTerceroDestino,
			CodigoPortafolioSBS,
			FechaOperacion,
			NumeroOperacion,
			CodigoOrden,
			CodigoModelo,
			Spot,
			CodigoContacto,
			CodigoTipoOperacion,
			CodigoContactoIntermediario,
			CartaAgrupado,
			BancoMatrizOrigen,
			BancoMatrizDestino,
			ObservacionCartaDestino,
			BancoGlosaDestino
		)
		SELECT
					'V' + SUBSTRING(cc.NumeroOperacion, 2, LEN(cc.NumeroOperacion) - 1 ),
					cc.CodigoMercado,
					cc.CodigoClaseCuenta,
					@p_NumeroCuenta,
					CASE 
						WHEN LEFT(cc.NumeroOperacion,1) = 'K' THEN LTRIM(cc.CodigoNemonico) + '-' + RTRIM(T.Descripcion) + '-Cuponera'
						ELSE LTRIM(cc.CodigoNemonico) + '-' + RTRIM(T.Descripcion) + '-' + RTRIM(CI.Descripcion) 
					END AS Referencia,
					@p_Importe,
					@p_usuario,
					@p_fecha,
					@nvcCodigoMoneda,
					cc.TipoMovimiento,
					'A',
					@p_Host,
					@p_NumeroCarta,
					@EstadoCarta,
					@p_Hora,
					NULL,
					NULL,
					NULL,
					NULL,
					cc.CodigoTercero,
					NULL,
					CASE
						WHEN @p_CodigoModalidadPago='' THEN NULL 
						ELSE @p_CodigoModalidadPago
					END,
					CASE 
						WHEN cc.CodigoOperacion = '65' THEN '66'
						WHEN cc.CodigoOperacion = '66' THEN '65'
						WHEN cc.CodigoOperacion = '93' THEN '94'
						WHEN cc.CodigoOperacion = '94' THEN '93'
						ELSE cc.CodigoOperacion
					END,
					NULL ,
					@p_CodigoPortafolio,
					@FechaOperacion,
					cc.NumeroOperacion,
					cc.CodigoOrden,
					@nvcCodigoModelo,
					@Spot,
					CASE
						WHEN @nvcCodigoContacto = '' then null
						ELSE @nvcCodigoContacto
					END,
					CASE 
						WHEN o.CodigoTipoOperacion = 1 THEN 2
						WHEN o.CodigoTipoOperacion = 2 THEN 1
						ELSE o.CodigoTipoOperacion
					END,
					@p_CodigoContactoIntermediario,
					CASE ISNULL(@p_agrupado,'') WHEN '' THEN NULL ELSE @p_agrupado END,
					@p_BancoMatrizOrigen ,
					@p_BancoMatrizDestino ,
					@p_ObservacionCartaDestino,
					@p_BancoGlosaDestino
		FROM		CuentasPorCobrarPagar cc
		INNER JOIN	Terceros T ON T.CodigoTercero = cc.CodigoTercero
		LEFT JOIN	Valores V ON V.CodigoNemonico = cc.CodigoNemonico
		LEFT JOIN	TipoInstrumento ti ON v.CodigoTipoInstrumentoSBS = ti.CodigoTipoInstrumentoSBS
		LEFT JOIN	ClaseInstrumento ci ON ti.CodigoClaseInstrumento = ci.CodigoClaseInstrumento
		LEFT JOIN	Operacion o ON o.CodigoOperacion = cc.CodigoOperacion
		WHERE		cc.NumeroOperacion = @p_NroOperacion
				AND	cc.CodigoPortafolioSBS = @p_CodigoPortafolio

		EXEC SaldosBancarios_Actualizar2 @p_CodigoPortafolio, @p_FechaPago

	END
GO

GRANT EXECUTE ON [dbo].[CuentasPorCobrar_LiquidarDivisas] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'FIN - EJECUCIÓN DE STORE PROCEDURES'

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 
  


 