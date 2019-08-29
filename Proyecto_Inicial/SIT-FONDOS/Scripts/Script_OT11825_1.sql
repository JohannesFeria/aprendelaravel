USE [SIT-FONDOS]
GO

-- ====================================================================================================================
-- Autor: Ricardo Colonia	
-- Fecha Creación: 14/02/2019
-- Órden de Trabajo: 11825
-- Descripción: Crea la tabla CronogramaPagos y añade el campo fechaCronogramaPago en tabla CuentasPorCobrarPagar
--				Modificación de SP.
-- ====================================================================================================================
PRINT 'NUEVO CAMPO: fechaCronogramaPagos EN TABLA dbo.CuentasPorCobrarPagar.PRC'
BEGIN TRANSACTION __Transaction_Log

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CuentasPorCobrarPagar]') and upper(name) = upper('fechaCronogramaPagos'))
	ALTER TABLE CuentasPorCobrarPagar add fechaCronogramaPagos DECIMAL(8);

GO

PRINT 'CREACIÓN DE TABLA dbo.CronogramaPagos.TAB'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CronogramaPagos_Portafolio]') AND parent_object_id = OBJECT_ID(N'[dbo].[CronogramaPagos]'))
ALTER TABLE [dbo].[CronogramaPagos] DROP CONSTRAINT [FK_CronogramaPagos_Portafolio]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CronogramaPagos_Estado]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CronogramaPagos] DROP CONSTRAINT [DF_CronogramaPagos_Estado]
END

GO

USE [SIT-FONDOS]
GO

/****** Object:  Table [dbo].[CronogramaPagos]    Script Date: 01/31/2019 17:28:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos]') AND type in (N'U'))
DROP TABLE [dbo].[CronogramaPagos]
GO

USE [SIT-FONDOS]
GO

/****** Object:  Table [dbo].[CronogramaPagos]    Script Date: 01/31/2019 17:28:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CronogramaPagos](
	[idCronogramaPagos] [int] IDENTITY(1,1) NOT NULL,
	[CodigoPortafolioSBS] [varchar](10) NOT NULL,
	[fechaCronogramaPagos] [decimal](8, 0) NOT NULL,
	[Estado] [char](1) NOT NULL,
	[UsuarioCreacion] [varchar](15) NULL,
	[FechaCreacion] [decimal](8, 0) NULL,
	[HoraCreacion] [varchar](10) NULL,
	[UsuarioModificacion] [varchar](15) NULL,
	[FechaModificacion] [decimal](8, 0) NULL,
	[HoraModificacion] [varchar](10) NULL,
	[Host] [varchar](20) NULL,
 CONSTRAINT [PK_CronogramaPagos] PRIMARY KEY CLUSTERED 
(
	[idCronogramaPagos] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CronogramaPagos]  WITH CHECK ADD  CONSTRAINT [FK_CronogramaPagos_Portafolio] FOREIGN KEY([CodigoPortafolioSBS])
REFERENCES [dbo].[Portafolio] ([CodigoPortafolioSBS])
GO

ALTER TABLE [dbo].[CronogramaPagos] CHECK CONSTRAINT [FK_CronogramaPagos_Portafolio]
GO

ALTER TABLE [dbo].[CronogramaPagos] ADD  CONSTRAINT [DF_CronogramaPagos_Estado]  DEFAULT ('A') FOR [Estado]
GO

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 

PRINT 'INICIO --- > SECCIÓN DE FUNCTION'
PRINT 'dbo.CalcularFlujoActualTCSwap.FNC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcularFlujoActualTCSwap]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalcularFlujoActualTCSwap]
GO

----------------------------------------------------------------------------------------------------
-- Objetivo	: Retornar flujo actual de tipo de cambio de Swap.
----------------------------------------------------------------------------------------------------
-- Fecha de Modificación    : 22/02/2019
-- Modificado por  			: Ricardo Colonia
-- Nro. Orden de Trabajo	: OT11825
-- Descripción del Cambio	: Creación de Función.
----------------------------------------------------------------------------------------------------  

CREATE FUNCTION [dbo].[CalcularFlujoActualTCSwap] (
	@p_CodigoOrden VARCHAR(12),
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_fechaOperacion DECIMAL(8)
)
	RETURNS NUMERIC(22,7)
	BEGIN
		DECLARE 
			@flujoTC NUMERIC(22,7)
		
		SELECT
			@flujoTC = (ISNULL(MontoAmortizacion,0) + ISNULL(MontoInteres,0)) /
					   (CASE WHEN (ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)) = 0 THEN 1 
							 ELSE (ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)) END)
		FROM
			Cuponera_Bono_Swap CBS
		JOIN
			OrdenInversion OI ON OI.CodigoOrden = CBS.CodigoOrden
		WHERE 
			CBS.CodigoOrden = @p_CodigoOrden
			AND (@p_fechaOperacion BETWEEN CBS.FechaIni AND CBS.FechaFin) 
			AND (@p_fechaOperacion BETWEEN CBS.FechaIniOriginal AND CBS.FechaFinOriginal) 
			AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			AND OI.Estado <> 'E-ELI'
			AND OI.Situacion = 'A'
	
		RETURN ISNULL(@flujoTC,0)
	END

GO

GRANT EXECUTE ON [dbo].[CalcularFlujoActualTCSwap] TO [rol_sit_fondos] AS [dbo]
GO
PRINT 'dbo.CalcularInteresAcumuladoSWAP.FNC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcularInteresAcumuladoSWAP]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalcularInteresAcumuladoSWAP]
GO

----------------------------------------------------------------------------------------------------
-- Objetivo	: Retornar el interés acumulado total por Leg de un swap.
----------------------------------------------------------------------------------------------------
-- Fecha de Modificación    : 22/02/2019
-- Modificado por  			: Ricardo Colonia
-- Nro. Orden de Trabajo	: OT11825
-- Descripción del Cambio	: Creación de Función.
----------------------------------------------------------------------------------------------------  

CREATE FUNCTION [dbo].[CalcularInteresAcumuladoSWAP] (
	@p_CodigoOrden VARCHAR(12),
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_leg INT
)
	RETURNS NUMERIC(22,7)
	BEGIN
		DECLARE 
			@InteresAcumuladoLeg1 NUMERIC(22,7),
			@InteresAcumuladoLeg2 NUMERIC(22,7),
			@InteresAcumulado NUMERIC(22,7)
			
		SELECT
			@InteresAcumuladoLeg1 = SUM(CBS.MontoInteresOriginal),
			@InteresAcumuladoLeg2 = SUM(CBS.MontoInteres)
		FROM
			Cuponera_Bono_Swap CBS
		JOIN
			OrdenInversion OI ON OI.CodigoOrden = CBS.CodigoOrden
		WHERE 
			CBS.CodigoOrden = @p_CodigoOrden
			AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			AND OI.Estado <> 'E-ELI'
			AND OI.Situacion = 'A'
			
		IF @p_leg = 1 
			SET @InteresAcumulado = @InteresAcumuladoLeg1
		ELSE IF @p_leg = 2
			SET @InteresAcumulado = @InteresAcumuladoLeg2
	
		RETURN @InteresAcumulado
	END

GO

GRANT EXECUTE ON [dbo].[CalcularInteresAcumuladoSWAP] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN --- > SECCIÓN DE FUNCTION'

PRINT 'INICIO --- > SECCIÓN DE SP'
BEGIN TRANSACTION __Transaction_SP

PRINT 'dbo.CronogramaPagos_ActualizarCuentasPorCobrarPagar.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos_ActualizarCuentasPorCobrarPagar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CronogramaPagos_ActualizarCuentasPorCobrarPagar]
GO

----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Actualiza la fecha de cronograma de pagos en la tabla de CuentasPorCobrarPagar
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 14/02/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11825
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 
--EXEC CronogramaPagos_ActualizarCuentasPorCobrarPagar '47',20190528,'SYSTEM',20190215,'HORA','1111'
CREATE PROCEDURE [dbo].[CronogramaPagos_ActualizarCuentasPorCobrarPagar]
(  
	@p_CodigoPortafolioSBS VARCHAR(12), 
	@p_FechaCronogramaPagos NUMERIC(8,0),
	@p_UsuarioModificacion VARCHAR(15),
	@p_FechaModificacion DECIMAL(8,0),
	@p_HoraModificacion VARCHAR(10),
	@p_Host VARCHAR(20) 
)
AS
BEGIN
	BEGIN TRANSACTION 
		BEGIN TRY
			
			DECLARE 
				@tblInstrumentosCronogramaPagos TABLE (
														codigoMnemonico VARCHAR(15),
														tipoPago VARCHAR(30),
														fechaLiquidacion VARCHAR(10)
													   )
			DECLARE
				@tblCuentasPorCobrarPagar TABLE (
												 CodigoPortafolioSBS VARCHAR(10),
												 NumeroOperacion VARCHAR(12),
												 CodigoOperacion VARCHAR(6),
												 CodigoNemonico VARCHAR(15),
												 FechaOperacion DECIMAL(8),
												 FechaVencimiento DECIMAL(8)
												)
			INSERT INTO 
				@tblInstrumentosCronogramaPagos
			EXEC 
				[CronogramaPagos_ListarbyDetalleInstrumento] 
															  @p_CodigoPortafolioSBS,
															  @p_FechaCronogramaPagos	
			
			INSERT INTO
				@tblCuentasPorCobrarPagar
			SELECT 
				@p_CodigoPortafolioSBS,
				CCP.NumeroOperacion,
				CCP.CodigoOperacion,
				CCP.CodigoNemonico,
				CCP.FechaOperacion, 
				MAX(CCP.FechaVencimiento)
			FROM 
				CuentasPorCobrarPagar CCP
			JOIN
				@tblInstrumentosCronogramaPagos TIC ON CCP.CodigoNemonico = TIC.codigoMnemonico
													   AND CCP.FechaVencimiento >= DBO.fn_SIT_gl_ConvertirFechaaDecimal(TIC.fechaLiquidacion)
			WHERE
				CCP.CodigoOperacion IN ('35','38','39')
			GROUP BY 
				CodigoNemonico, FechaOperacion,CodigoOperacion,NumeroOperacion
			ORDER BY 
				CodigoNemonico,FechaOperacion DESC
			
			UPDATE
				CuentasPorCobrarPagar
			SET 
				fechaCronogramaPagos = @p_FechaCronogramaPagos,
				UsuarioModificacion = @p_UsuarioModificacion,
				FechaModificacion = @p_FechaModificacion,
				HoraModificacion = @p_HoraModificacion,
				Host = @p_Host
				
			FROM
				CuentasPorCobrarPagar CCP
			JOIN
				@tblCuentasPorCobrarPagar TCCP ON CCP.CodigoNemonico = TCCP.CodigoNemonico
												  AND CCP.NumeroOperacion = TCCP.NumeroOperacion
												  AND CCP.CodigoOperacion = TCCP.CodigoOperacion
												  AND CCP.CodigoPortafolioSBS = TCCP.CodigoPortafolioSBS
		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		EXEC pr_SIT_retornarError 
	END CATCH
	
END


GO

GRANT EXECUTE ON [dbo].[CronogramaPagos_ActualizarCuentasPorCobrarPagar] TO [rol_sit_fondos] AS [dbo]
GO



PRINT 'dbo.CronogramaPagos_ListarbyRangoFechaPortafolio.PRC'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos_ListarbyRangoFechaPortafolio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CronogramaPagos_ListarbyRangoFechaPortafolio]
GO

----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Listar registros de Cronograma de pagos por Portafolio y rango de fechas.
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 18/09/2018  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11568
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[CronogramaPagos_ListarbyRangoFechaPortafolio]
(  
	@p_CodigoPortafolioSBS VARCHAR(12), 
	@p_FechaINI NUMERIC(8,0),
	@p_FechaFIN NUMERIC(8,0)
)
AS
BEGIN
	SELECT 
		idCronogramaPagos,
		CodigoPortafolioSBS,
		fechaCronogramaPagos FechaPagos,
		dbo.FN_SIT_OBT_FechaFormateada(fechaCronogramaPagos)fechaCronogramaPagos,
		'R' Accion -- Controla el estado en la grilla R: Registrado.
	FROM 
		CronogramaPagos
	WHERE
		CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		AND fechaCronogramaPagos BETWEEN @p_FechaINI AND @p_FechaFIN
		AND Estado = 'A'
	ORDER BY 
		3
	
END

GO

GRANT EXECUTE ON [dbo].[CronogramaPagos_ListarbyRangoFechaPortafolio] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.OrdenInversion_ConfirmarOI.PRC'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_ConfirmarOI]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrdenInversion_ConfirmarOI]
GO

-----------------------------------------------------------------------------------------------------------------------
--Objetivo: Confirmar una Orden de Inversión
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 21/09/2017
--	Creado por: Ian Pastor
--	Nro. Orden de Trabajo: 10783
--	Descripcion del cambio: Agregar la actualización del campo "NumeroIdentificacion" en la tabla OrdenInversion
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 10/07/2018
--	Modificado por: Ricardo Colonia
--	Proyecto: Fondos - II
--	OT: 11473
--	Descripcion del cambio: Se agrega Begin Transaction y Try ... Catch
-----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 14/02/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11825
--	Descripcion del cambio: Se agrega fecha de cronograma de pago de portafolio.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[OrdenInversion_ConfirmarOI](
	@p_CodigoOrden  VARCHAR(6),
	@p_CodigoPortafolioSBS  VARCHAR(10),
	@p_Poliza  VARCHAR(20),
	@p_Result VARCHAR(12) = '' OUT
)
AS
BEGIN
	BEGIN TRANSACTION 
	BEGIN TRY
	
		DECLARE 
			@m_CorrelativoFD VARCHAR(3), 
			@m_SecuenciaFD INT,
			@m_OperacionFD CHAR(1),
			@m_CodigoOperacion VARCHAR(6)
			
		SELECT 
			@m_CodigoOperacion = CodigoOperacion
		FROM 
			OrdenInversion
		WHERE 
			CodigoOrden = @p_CodigoOrden 
			AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			
		IF @p_Poliza = 'CM'
		BEGIN
			SELECT 
				@m_SecuenciaFD = MAX(cf.Secuencia)
			FROM 
				OrdenInversion oi
			INNER JOIN 
				CorrelativoForward cf ON cf.Codigo = SUBSTRING(oi.numeropoliza,3,3)
			WHERE 
				oi.codigoMnemonico = 'FORWARD' 
				AND oi.estado ='E-CON' 
				AND oi.situacion = 'A' 
				AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			
			SELECT 
				@m_CorrelativoFD = Codigo
			FROM 
				CorrelativoForward
			WHERE 
				Secuencia = ISNULL(@m_SecuenciaFD,0) + 1
			
			SELECT 
				@m_OperacionFD = CASE WHEN @m_CodigoOperacion = '93' THEN 'C'
								      WHEN @m_CodigoOperacion = '94' THEN 'V' END
			SET @p_Poliza = 'I' + @m_OperacionFD + @m_CorrelativoFD
			SET @p_Result = '0'
		END
		ELSE 
		BEGIN
			IF ((SELECT CodigoMnemonico 
				 FROM OrdenInversion 
				 WHERE CodigoOrden = @p_CodigoOrden 
					   AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS  ) = 'FORWARD')
			BEGIN
				SELECT 
					@p_Result = COUNT(1) 
					FROM OrdenInversion 
					WHERE NumeroPoliza = @p_Poliza 
					      AND CodigoOrden <> @p_CodigoOrden
				IF @p_Result <> '0' RETURN
			END
			ELSE
				SET @p_Result = '0'
		END
		DECLARE 
			@FechaConstitucion NUMERIC(8),
			@ComisionAgentes VARCHAR(6),
			@TipoOperacionComision VARCHAR(1),
			@TipoOperacion VARCHAR(1),
			@CodigoOperacion VARCHAR(4),
			@CodigoNemonico VARCHAR(15),
			@FechaOperacion NUMERIC(8),
			@FechaLiquidacion NUMERIC(8),
			@NumeroOperacion VARCHAR(12),
			@decImporte DECIMAL(22,7),
			@decIngreso DECIMAL(22,7),
			@decEgreso DECIMAL(22,7),
			@decTipoCambio DECIMAL(22,7),
			@decImporteRegistrado DECIMAL(22,7),
			@nvcNewCodigoOrdenInversion VARCHAR(6),
			@decTotalComisiones DECIMAL(22,7),
			@spot VARCHAR(1),
			@secuencia NUMERIC,
			@vcCategoriaInstrumento VARCHAR(2),
			@CodigoMonedaNemonico VARCHAR(10),
			@MonedaPago VARCHAR(10),
			@MonedaOI VARCHAR(10),
			@MontoDestinoOI DECIMAL(22,7),
			@CantOIPoliza INT,
			@TotalComisionesOrden DECIMAL(22,7),
			@codigoSBS VARCHAR(12),
			@OrdenGenera VARCHAR(6),
			@Compra NUMERIC(22,7),
			@Venta NUMERIC(22,7),
			@Delivery VARCHAR(3),
			@Ficticia VARCHAR(1),
			@Usuario VARCHAR(15),
			@Fixing DECIMAL(22,7)

		DECLARE 
			@Cantidad NUMERIC(22,7),
			@Precio NUMERIC(22,7),
			@Asignacion NUMERIC(22,7),
			@CantidadOrdenado NUMERIC(22,7),
			@PrecioNegociacionLimpio NUMERIC(22,7),
			@MontoCancelar NUMERIC(22,7),
			@MontoNominalOperacion NUMERIC(22,7),
			@MontoOperacion NUMERIC(22,7),
			@CodigoPrevOrden NUMERIC(12),
			@Correlativo NUMERIC(12),
			@CodigoTercero VARCHAR(12),
			@FechaCreacion NUMERIC(8),
			@HoraCreacion VARCHAR(20),
			@Host VARCHAR(20),
			@Estado VARCHAR(5),
			@NumeroIdentificacion INT
		
		SELECT 
			@Estado = Estado 
		FROM 
			OrdenInversion    
		WHERE 
			CodigoOrden = @p_CodigoOrden 
			AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS    
			
		SET @p_Poliza = UPPER(@p_Poliza)
		SET @TotalComisionesOrden = ISNULL((SELECT SUM(ValorComision) 
											FROM ImpuestosComisionesOrdenPreOrden
											WHERE CodigoOrdenPreOrden = @p_CodigoOrden  
												  AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
												  AND Situacion = 'A'),0)
		
		--> Averigua valores                      
		SELECT 
			@ComisionAgentes = Valor 
		FROM 
			ParametrosGenerales 
		WHERE 
			Nombre LIKE 'ComisionAgentes'
			
		SELECT 
			@TipoOperacionComision = Egreso 
		FROM 
			Operacion O
		INNER JOIN 
			TipoOperacion T ON O.CodigoTipoOperacion = T.CodigoTipoOperacion
		WHERE 
			CodigoOperacion = @ComisionAgentes                      

		SELECT 
			@FechaConstitucion = FechaConstitucion 
		FROM 
			Portafolio
		WHERE 
			CodigoPortafolioSBS = @p_CodigoPortafolioSBS

		SELECT 
			@CodigoOperacion = oi.CodigoOperacion,
			@CodigoNemonico = oi.CodigoMnemonico,
			@FechaOperacion = oi.FechaOperacion,
			@FechaLiquidacion = oi.FechaLiquidacion,
			@decIngreso= oi.CantidadOperacion,
			@MontoDestinoOI = oi.MontoDestino,
			@MonedaOI = oi.CodigoMoneda,
			@decTipoCambio = oi.TipoCambio,
			@vcCategoriaInstrumento = oi.CategoriaInstrumento,
			@spot= ISNULL(oi.delibery,'S'),
			@codigoSBS = oi.codigoSBS,
			@OrdenGenera = oi.OrdenGenera,
			@Delivery = oi.delibery,
			@decImporte = CASE WHEN oi.CodigoOperacion = '3' 
									OR OI.CodigoOperacion = '101' THEN
																	  CASE WHEN oi.CodigoMoneda = 'BRL' THEN  ISNULL(oi.MontoNominalOPeracion, 0) * oi.Fixing 
																			ELSE ISNULL(oi.MontoNominalOPeracion, 0) END
							   WHEN oi.CodigoOperacion = '4' 
							        OR oi.CodigoOperacion = '5' THEN
																  	CASE WHEN oi.CodigoMoneda = 'BRL' THEN  ISNULL(oi.MontoNominalOPeracion, 0) * oi.Fixing
																		 WHEN oi.MontoOperacion IS NULL THEN oi.MontoNominalOperacion ELSE oi.MontoOperacion  END
							   WHEN OI.CodigoOperacion = '6' THEN ISNULL(oi.MontoNetoOperacion,oi.MontoOperacion)
							   ELSE ISNULL(oi.MontoNetoOperacion,oi.MontoOperacion) END,
			@Compra = (CASE WHEN oi.CodigoOperacion IN (1,61) THEN ISNULL(oi.CantidadOperacion,0) ELSE 0 END),              
			@Venta = (CASE WHEN oi.CodigoOperacion IN (2,62) THEN ISNULL(oi.CantidadOperacion,0) ELSE 0 END),      
			@Ficticia= Ficticia,
			@Usuario = ISNULL(oi.usuariomodificacion,''),
			@CantidadOrdenado = CantidadOrdenado,
			@PrecioNegociacionLimpio = PrecioNegociacionLimpio,
			@Precio = Precio,
			@MontoCancelar = MontoCancelar,
			@MontoNominalOperacion = MontoNominalOperacion,
			@MontoOperacion = MontoOperacion,
			@CodigoTercero = oi.CodigoTercero,
			@Host = oi.Host
		FROM 
			OrdenInversion oi
		INNER JOIN 
			Operacion op ON oi.CodigoOperacion = op.CodigoOperacion
		INNER JOIN 
			TipoOperacion tp ON op.CodigoTipoOperacion = tp.CodigoTipoOperacion
		LEFT JOIN 
			Terceros tc ON oi.CodigoTercero = tc.CodigoTercero
		WHERE 
			oi.CodigoOrden = @p_CodigoOrden 
			AND oi.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		
		SET @Usuario = ISNULL(@Usuario,'')
		
		--OT10783 - Inicio
		IF @vcCategoriaInstrumento IN ('DP','OR')
		BEGIN
			SELECT 
				@NumeroIdentificacion = MAX(CONVERT(INT, ISNULL(NumeroIdentificacion ,0)))
			FROM 
				OrdenInversion
			WHERE 
				CategoriaInstrumento = @vcCategoriaInstrumento 
				AND Situacion = 'A' 
				AND Estado <> 'E-ELI' 
				AND CodigoTercero = @CodigoTercero
				AND CodigoMoneda = @MonedaOI 
				AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			
			SET @NumeroIdentificacion = ISNULL(@NumeroIdentificacion,0) + 1
		END
		--OT10783 - Fin
		
		--> INICIO | Obtener Valores de Cronograma de Pagos
		DECLARE
				@fechaCronogramaPagos DECIMAL(8) = 0
				
		IF @CodigoOperacion IN ('35','38','39')
		BEGIN
			SET	@fechaCronogramaPagos = ISNULL((SELECT MIN(fechaCronogramaPagos) 
												FROM CronogramaPagos
												WHERE fechaCronogramaPagos >= @FechaOperacion
													  AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS),@FechaLiquidacion)
		END
  	    --> FIN | Obtener Valores de Cronograma de Pagos
  	    
		UPDATE 
			OrdenInversion
		SET 
			Estado = 'E-CON',
			NumeroPoliza = @p_Poliza,
			TotalComisiones = @TotalComisionesOrden,
			codigoSBS = CASE WHEN CategoriaInstrumento in ('DP','FD','OR') THEN dbo.fn_CodigoSBS(@p_Poliza,CodigoTipoTitulo,CodigoTercero,CodigoMoneda)
						     ELSE codigoSBS END,
			NumeroIdentificacion = CAST(@NumeroIdentificacion AS VARCHAR(10))
		WHERE 
			CodigoOrden = @p_CodigoOrden 
			AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		
		IF @Estado = 'E-EJE'    
		BEGIN
			SELECT 
				@CodigoPrevOrden = p.CodigoPrevOrden, 
				@Correlativo = p.Correlativo    
			FROM 
				PrevOrdenInversion p    
			INNER JOIN 
				PrevOrdenInversion_OI poi ON p.CodigoPrevOrden = poi.CodigoPrevOrden    
			WHERE 
				poi.CodigoOrden = @p_CodigoOrden    

			SET @FechaCreacion = CAST(CONVERT(VARCHAR(8), GETDATE(), 112) AS NUMERIC(8))    
			SET @HoraCreacion = CONVERT(VARCHAR(8), GETDATE(), 108)    
			SET @Correlativo = ISNULL(@Correlativo, 0)    
			SET @CodigoPrevOrden = ISNULL(@CodigoPrevOrden, 0)    

			SET @Cantidad = CASE WHEN @decIngreso=0 THEN @CantidadOrdenado ELSE ISNULL(@decIngreso, @CantidadOrdenado) END    
			SET @Precio = CASE WHEN @PrecioNegociacionLimpio=0 THEN @Precio ELSE ISNULL(@PrecioNegociacionLimpio, @Precio) END
			SET @Cantidad = CASE @vcCategoriaInstrumento WHEN 'FD' THEN @MontoCancelar 
														 WHEN 'DP' THEN @MontoNominalOperacion 
														 WHEN 'CV' THEN @MontoOperacion 
														 ELSE @Cantidad END    
			SET @Asignacion = @Cantidad 
			
			EXEC dbo.pr_SIT_ins_TrazabilidadOperaciones_Insertar_sura 
																	  @FechaOperacion,
																	  @Correlativo,
																	  'E-CON',
																	  '2',
																	  @CodigoPrevOrden,
																	  @p_CodigoOrden,
																	  @CodigoNemonico,
																	  @CodigoOperacion,
																	  @Cantidad,
																	  @Precio,
																	  @CodigoTercero,
																	  @Cantidad,
																	  @Precio,
																	  @p_CodigoPortafolioSBS,
																	  @Asignacion,
																	  '2',
																	  '8',
																	  '','Confirma Orden de Inversión',
																	  @Usuario,
																	  @FechaCreacion,
																	  @HoraCreacion,
																	  @Host
		END
		IF ISNULL(@Ficticia,'') = '' SET @Ficticia = 'N'
		--Pagar con Otra Moneda                     
		SELECT 
			@CodigoMonedaNemonico=CodigoMoneda,
			@MonedaPago=MonedaPago 
		FROM 
			Valores                     
		WHERE 
			CodigoNemonico=@CodigoNemonico 
			
		SELECT 
			@TipoOperacion = Egreso 
		FROM 
			Operacion O
		INNER JOIN 
			TipoOperacion T ON O.CodigoTipoOperacion = T.CodigoTipoOperacion
		WHERE 
			CodigoOperacion = @CodigoOperacion
			
		IF @TipoOperacion = 'S'                      
			SET @decEgreso= 0                      
		ELSE
		BEGIN
			SET @decEgreso= @decIngreso                      
			SET @decIngreso= 0                      
		END
		
		SET @decImporte = ISNULL(@decImporte,0)
		SET @Delivery = ISNULL(@Delivery,'')
		
		DECLARE 
			@LiquidaFechaAnt VARCHAR(1),
			@UsuarioLiqFecAnt VARCHAR(15)    
			
		IF(SELECT COUNT(1) 
		   FROM tmpOrdenLiquidaFechaAnt 
		   WHERE(CASE WHEN ISNUMERIC(SUBSTRING(NumeroOperacion,1,1)) = 1 THEN NumeroOperacion 
				      ELSE SUBSTRING(NumeroOperacion,2,LEN(NumeroOperacion)) END) = @p_CodigoOrden
				 AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS) > 0
		BEGIN
			SET @LiquidaFechaAnt = '1'    
			SET @UsuarioLiqFecAnt = @Usuario    
		END
		ELSE
		BEGIN
			SET @LiquidaFechaAnt = NULL    
			SET @UsuarioLiqFecAnt = NULL    
		END
		IF @Ficticia ='N'--NO es Ficticia debe de bajar a CXCXP y tbm  baja Comisiones. OT 11343      
		BEGIN      
			IF (@Delivery <> 'N' OR @vcCategoriaInstrumento <> 'FD') AND  @CodigoOperacion <> '5'
			BEGIN      
				--> Graba en CxCxP      
				IF (EXISTS(SELECT 1 
						   FROM CuentasPorCobrarPagar 
						   WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS
							     AND NumeroOperacion = @p_CodigoOrden 
							     AND CodigoOperacion = @CodigoOperacion))
					UPDATE 
						ccp 
					SET 
						ccp.Importe = CASE WHEN LEFT(oi.codigoSBS,2)='83' THEN oi.MontoPrima ELSE @decImporte END,
						ccp.FechaVencimiento = oi.FechaLiquidacion
					FROM 
						CuentasPorCobrarPagar ccp
					INNER JOIN 
						OrdenInversion oi ON oi.CodigoOrden = ccp.CodigoOrden 
											 AND OI.CodigoPortafolioSBS = CCP.CodigoPortafolioSBS 
					WHERE 
						ccp.CodigoPortafolioSBS = @p_CodigoPortafolioSBS  
						AND ccp.NumeroOperacion = @p_CodigoOrden 
						AND ccp.CodigoOperacion = @CodigoOperacion
				ELSE
				BEGIN
					IF LEFT(@codigoSBS,2) = '83' --RGF 20090327 Los estructurados no bajan a CxCxP, solo baja la prima
						INSERT INTO 
							CuentasPorCobrarPagar(
												  CodigoPortafolioSBS,
												  NumeroOperacion,
												  CodigoOperacion,
												  Referencia,
												  CodigoOrden,
												  Importe,
												  FechaIngreso,  
												  CodigoClaseCuenta,
												  NumeroCuenta,
												  CodigoMoneda,
												  CodigoMercado,
												  FechaOperacion,
												  FechaPago,
												  TipoMovimiento,
												  Egreso,
												  Situacion,
												  UsuarioCreacion,
												  HoraCreacion,
												  FechaCreacion,
												  UsuarioModificacion,
												  Host,
												  FechaModificacion,
												  HoraModificacion,
												  CodigoTercero,
												  Estado,
												  FechaVencimiento,
												  NumeroAsiento,
												  FechaAsiento,
												  CodigoNemonico,
												  Spot,
												  LiquidaFechaAnt,
												  UsuarioLiqFecAnt,
												  fechaCronogramaPagos
												 )      
						SELECT 
							OI.CodigoPortafolioSBS,
							OI.CodigoOrden,
							OI.CodigoOperacion,
							V.Descripcion,
							OI.CodigoOrden,
							CASE WHEN oi.codigotipotitulo = 'DPZBRL360' THEN oi.Fixing * oi.montonominaloperacion 
							     ELSE oi.MontoPrima END, 
							@FechaConstitucion,
							NULL,
							NULL,
							(CASE WHEN oi.CodigoMoneda = 'BRL' THEN 'DOL' 
								  ELSE ie.MonedaPrima END) AS CodigoMoneda,
							CodigoMercado = dbo.ObtenerCodigoMercado(@p_CodigoOrden),    
							oi.FechaOperacion,
							NULL,
							'A',
							tp.Egreso,
							'A',
							@Usuario,
							CONVERT(VARCHAR(8),GETDATE(), 108),
							CONVERT(VARCHAR(8), GETDATE(), 112),
							NULL,
							NULL,      
							NULL,
							NULL,
							OI.CodigoTercero,
							NULL,
							OI.FechaLiquidacion,
							NULL,
							NULL,
							v.CodigoNemonico,
							@spot,
							@LiquidaFechaAnt,
							@UsuarioLiqFecAnt,
							(CASE WHEN @fechaCronogramaPagos = 0 THEN NULL ELSE @fechaCronogramaPagos END) fechaCronogramaPagos
						FROM 
							OrdenInversion OI   
						LEFT JOIN 
							Valores v ON v.CodigoNemonico = oi.CodigoMnemonico      
						INNER JOIN 
							Terceros t ON t.CodigoTercero = oi.CodigoTercero      
						INNER JOIN 
							Operacion op ON op.CodigoOperacion = oi.CodigoOperacion      
						INNER JOIN 
							TipoOperacion tp ON op.CodigoTipoOperacion = tp.CodigoTipoOperacion      
						INNER JOIN 
							InstrumentosEstructurados ie ON ie.CodigoNemonico = v.CodigoNemonico      
						WHERE 
							oi.CodigoOrden = @p_CodigoOrden      
							AND oi.CodigoPortafolioSBS = @p_CodigoPortafolioSBS      
							AND oi.MontoPrima >0      
					ELSE 
					BEGIN					
						--Las operaciones con monto igual a cero no deben bajar a CxCxP
						--> Graba en CxCxP con el mismo numero de la Orden
						IF @decImporte > 0 AND NOT EXISTS (SELECT 1 
														   FROM OrdenInversion 
														   WHERE codigoOrden = @OrdenGenera 
															     AND CategoriaInstrumento = 'IE' 
															     AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS)
														   AND (@vcCategoriaInstrumento <> 'OR' OR @m_CodigoOperacion = '6')
						BEGIN
							INSERT INTO 
								CuentasPorCobrarPagar(
													  CodigoPortafolioSBS,
													  NumeroOperacion,
													  CodigoOperacion,
													  Referencia,
													  CodigoOrden,
													  Importe,
													  FechaIngreso,
													  CodigoClaseCuenta,
													  NumeroCuenta,
													  CodigoMoneda,
													  CodigoMercado,
													  FechaOperacion,
													  FechaPago,
													  TipoMovimiento,
													  Egreso,
													  Situacion,
													  UsuarioCreacion,
													  HoraCreacion,
													  FechaCreacion,
													  UsuarioModificacion,
													  Host,
													  FechaModificacion,
													  HoraModificacion,
													  CodigoTercero,
													  Estado,
													  FechaVencimiento,
													  NumeroAsiento,
													  FechaAsiento,
													  CodigoNemonico,
													  Spot,
													  LiquidaFechaAnt,
													  UsuarioLiqFecAnt,
													  fechaCronogramaPagos
													 )
							SELECT 
								OI.CodigoPortafolioSBS,
								OI.CodigoOrden,
								OI.CodigoOperacion,
								V.Descripcion,
								OI.CodigoOrden,
								@decImporte,
								@FechaConstitucion,
								NULL,
								NULL,
								(CASE WHEN oi.CodigoMoneda = 'BRL' THEN 'DOL' 
									  ELSE CASE WHEN RTRIM(LTRIM(@MonedaOI))='VAC' THEN 'NSOL' 
											    ELSE RTRIM(LTRIM(@MonedaOI)) END END) CodigoMoneda,        
								CodigoMercado = CASE WHEN oi.TipoFondo IN ('CC_SNC','CC_CNC') THEN oi.CodigoPlaza 
												ELSE dbo.ObtenerCodigoMercado(@p_CodigoOrden) END, --RGF 20081124 --HDG 20120424    
								oi.FechaOperacion,
								NULL,
								'A',
								tp.Egreso,
								'A',
								@Usuario,
								CONVERT(VARCHAR(8), GETDATE(), 108),
								CONVERT(VARCHAR(8), GETDATE(), 112),
								NULL,
								NULL,
								NULL,
								NULL,
								OI.CodigoTercero,
								NULL,
								OI.FechaLiquidacion,
								NULL,
								NULL,
								v.CodigoNemonico,
								@spot,
								@LiquidaFechaAnt,
								@UsuarioLiqFecAnt,
								(CASE WHEN @fechaCronogramaPagos = 0 THEN NULL ELSE @fechaCronogramaPagos END) fechaCronogramaPagos    
							FROM  
								OrdenInversion OI
							LEFT JOIN 
								Valores v ON v.CodigoNemonico = oi.CodigoMnemonico          
							INNER JOIN 
								Operacion op ON op.CodigoOperacion = oi.CodigoOperacion          
							INNER JOIN 
								TipoOperacion tp ON op.CodigoTipoOperacion = tp.CodigoTipoOperacion          
							LEFT JOIN 
								Terceros t ON t.CodigoTercero = oi.CodigoTercero 
							LEFT JOIN 
								CustodioValores cv ON cv.CodigoNemonico = OI.CodigoMnemonico 
													  AND cv.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
													  AND cv.CodigoCustodio='BBH'        
							WHERE 
								oi.CodigoOrden = @p_CodigoOrden      
								AND oi.CodigoPortafolioSBS = @p_CodigoPortafolioSBS      
						END
						ELSE IF @vcCategoriaInstrumento = 'OR'
						BEGIN
							INSERT INTO 
								CuentasPorCobrarPagar(
													  CodigoPortafolioSBS,
													  NumeroOperacion,
													  CodigoOperacion,
													  Referencia,
													  CodigoOrden,
													  Importe,
													  FechaIngreso,
													  CodigoClaseCuenta,
													  NumeroCuenta,
													  CodigoMoneda,
													  CodigoMercado,
													  FechaOperacion,
													  FechaPago,
													  TipoMovimiento,
													  Egreso,
													  Situacion,
													  UsuarioCreacion,
													  HoraCreacion,
													  FechaCreacion,
													  UsuarioModificacion,
													  Host,
													  FechaModificacion,
													  HoraModificacion,
													  CodigoTercero,
													  Estado,
													  FechaVencimiento,
													  NumeroAsiento,
													  FechaAsiento,
													  CodigoNemonico,
													  Spot,
													  LiquidaFechaAnt,
													  UsuarioLiqFecAnt,
													  fechaCronogramaPagos
													 )        
							SELECT 
								oi.CodigoPortafolioSBS,
								oi.CodigoOrden,
								@CodigoOperacion,
								V.Descripcion,
								oi.CodigoOrden,
								@decImporte,
								@FechaConstitucion,
								NULL,
								NULL,
								CASE WHEN RTRIM(LTRIM(@MonedaOI))='VAC' THEN 'NSOL' 
									 ELSE rtrim(ltrim(@MonedaOI)) END CodigoMoneda,
								CodigoMercado = dbo.ObtenerCodigoMercado(@p_CodigoOrden),
								oi.FechaOperacion,
								NULL,
								'A',
								tp.Egreso,
								'A',
								@Usuario,  
								CONVERT(VARCHAR(8), GETDATE(), 108),
								CONVERT(VARCHAR(8), GETDATE(), 112),
								NULL,
								NULL,
								NULL,
								NULL,
								OI.CodigoTercero,
								NULL,
								OI.FechaLiquidacion,
								NULL,
								NULL,
								CASE WHEN OI.CodigoMoneda = 'NSOL' THEN 'REPORTESOL' 
									 ELSE 'REPORTEDOL' END ,
								@spot,
								@LiquidaFechaAnt,
								@UsuarioLiqFecAnt,
								(CASE WHEN @fechaCronogramaPagos = 0 THEN NULL ELSE @fechaCronogramaPagos END) fechaCronogramaPagos  
							FROM 
								ORDENINVERSION oi
							INNER JOIN 
								Valores v ON v.CodigoNemonico = oi.CodigoMnemonico
							INNER JOIN 
								Operacion op ON op.codigooperacion = @CodigoOperacion
							INNER JOIN 
								TipoOperacion tp ON tp.CodigoTipoOperacion = op.CodigoTipoOperacion
							WHERE  
								oi.CodigoOrden = @p_CodigoOrden
								AND oi.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
						END
					END
				END
			END
		END
		--Actualiza el saldo de Compra Venta - Saldos Cartera Titulo
		EXEC sp_sit_ActualizaSaldoUnidadesT 
											@CodigoNemonico,
											@p_CodigoPortafolioSBS,
											@FechaOperacion 
		--Actualiza el saldo para dias posteriores en caso existieran
		EXEC sp_SIT_ActualizaSaldoUnidadesPosterior 
													@FechaOperacion,
													@CodigoNemonico,
													@p_CodigoPortafolioSBS
		
	COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		SELECT @p_Result = CAST(ERROR_NUMBER() AS VARCHAR(12))
	END CATCH
END

GO

GRANT EXECUTE ON [dbo].[OrdenInversion_ConfirmarOI] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.CronogramaPagos_ListarbyDetalleInstrumento.PRC'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos_ListarbyDetalleInstrumento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CronogramaPagos_ListarbyDetalleInstrumento]
GO

----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Listar el detalle del cronograma de pagos por instrumento.
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 14/02/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11825
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[CronogramaPagos_ListarbyDetalleInstrumento]
(  
	@p_CodigoPortafolioSBS VARCHAR(12), 
	@p_fechaPago NUMERIC(8,0)
)
AS
BEGIN
	DECLARE
		@tblCuponeraNormalFechaAnterior TABLE (
											   fechaAnterior DECIMAL(8),
											   codigoMnemonico VARCHAR(15)
											   )
	INSERT INTO
		@tblCuponeraNormalFechaAnterior
	SELECT DISTINCT
		MAX(CN1.FechaTermino),
		CN1.CodigoNemonico
	FROM
		CuponeraNormal CN1
	WHERE
		CN1.FechaTermino < (SELECT MAX(FechaTermino) 
							FROM CuponeraNormal CN2
							WHERE FechaTermino <= @p_fechaPago
								  AND CN2.CodigoNemonico = CN1.CodigoNemonico)
	GROUP BY 
		CN1.CodigoNemonico
	ORDER BY 
		2
	

	SELECT 
		codigoMnemonico = CN.CodigoNemonico , 
		tipoPago = (CASE WHEN ISNULL(CN.TasaCupon,0) > 0 THEN 'Interés' ELSE '-' END +  
					CASE WHEN ISNULL(CN.Amortizacion,0) > 0 THEN ' / Amortización' ELSE '' END), 
		fechaLiquidacion = dbo.ConvertirFecha(MAX(CN.FechaTermino)) 
	FROM
		CuponeraNormal CN 
	JOIN
		Valores VA ON VA.CodigoNemonico = CN.CodigoNemonico
	JOIN
		OrdenInversion OI ON OI.CodigoMnemonico = CN.CodigoNemonico
	JOIN
		Portafolio PO ON OI.CodigoPortafolioSBS = PO.CodigoPortafolioSBS
	LEFT JOIN
		@tblCuponeraNormalFechaAnterior TC ON CN.CodigoNemonico = TC.codigoMnemonico
    WHERE
		OI.CodigoOperacion = '1'
		AND CN.FechaTermino <= @p_fechaPago
		AND CN.FechaTermino > ISNULL((SELECT MAX(fechaCronogramaPagos)
									  FROM CronogramaPagos CP 
									  WHERE CP.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
									    	AND CP.fechaCronogramaPagos < @p_fechaPago
									    	AND CP.Estado = 'A'),ISNULL(TC.fechaAnterior,0))
		AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		AND OI.Estado <> 'E-ELI'
		AND OI.Situacion = 'A'
		AND VA.FechaVencimiento >= @p_fechaPago
	GROUP BY
		CN.CodigoNemonico, CN.TasaCupon,CN.Amortizacion
	ORDER BY
		1,3

END

GO

GRANT EXECUTE ON [dbo].[CronogramaPagos_ListarbyDetalleInstrumento] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.CronogramaPagos_Modificar.PRC'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[AumentoCapital_Modificar]    Script Date: 01/31/2019 17:12:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos_Modificar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CronogramaPagos_Modificar]
GO

----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Modifica registros en el Cronograma de Pagos.
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 14/02/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11825
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[CronogramaPagos_Modificar]
(  
    @p_idCronogramaPagos INT,
	@p_CodigoPortafolioSBS VARCHAR(12), 
	@p_FechaCronogramaPagos NUMERIC(8,0),
	@p_Estado CHAR(1),
	@p_UsuarioModificacion VARCHAR(15),
	@p_FechaModificacion DECIMAL(8,0),
	@p_HoraModificacion VARCHAR(10),
	@p_Host VARCHAR(20)
)
AS
BEGIN
	BEGIN TRANSACTION 
		BEGIN TRY

			UPDATE
				CronogramaPagos
			SET 
				fechaCronogramaPagos = @p_FechaCronogramaPagos,
				Estado = @p_Estado,
				UsuarioModificacion = @p_UsuarioModificacion,
				FechaModificacion = @p_FechaModificacion,
				HoraModificacion = @p_HoraModificacion,
				Host = @p_Host
			WHERE
				idCronogramaPagos = @p_idCronogramaPagos

			EXEC CronogramaPagos_ActualizarCuentasPorCobrarPagar 
																  @p_CodigoPortafolioSBS,
																  @p_FechaCronogramaPagos,
																  @p_UsuarioModificacion,
																  @p_FechaModificacion,
																  @p_HoraModificacion,
																  @p_Host
		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		EXEC pr_SIT_retornarError 
	END CATCH
END

GO


GRANT EXECUTE ON [dbo].[CronogramaPagos_Modificar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.CronogramaPagos_Insertar.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CronogramaPagos_Insertar]
GO

----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Inserta a los registros de Cronograma de pagos por Portafolio y fechas.
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 14/02/2019   
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11825
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [dbo].[CronogramaPagos_Insertar]
(  
	@p_CodigoPortafolioSBS VARCHAR(12), 
	@p_FechaCronogramaPagos NUMERIC(8,0),
	@p_UsuarioCreacion VARCHAR(15),
	@p_FechaCreacion DECIMAL(8,0),
	@p_HoraCreacion VARCHAR(10),
	@p_Host VARCHAR(20)
)
AS
BEGIN
	BEGIN TRANSACTION 
		BEGIN TRY
			
			INSERT INTO 
				CronogramaPagos (
								 CodigoPortafolioSBS,
								 fechaCronogramaPagos,
								 UsuarioCreacion,
								 FechaCreacion,
								 HoraCreacion,
								 Host
								)
			VALUES(
				   @p_CodigoPortafolioSBS,
				   @p_FechaCronogramaPagos,
				   @p_UsuarioCreacion,
				   @p_FechaCreacion,
				   @p_HoraCreacion,
				   @p_Host
				  )
			
			EXEC CronogramaPagos_ActualizarCuentasPorCobrarPagar 
													  @p_CodigoPortafolioSBS,
													  @p_FechaCronogramaPagos,
													  @p_UsuarioCreacion,
													  @p_FechaCreacion,
													  @p_HoraCreacion,
													  @p_Host
			
		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		EXEC pr_SIT_retornarError 
	END CATCH
	
END


GO

GRANT EXECUTE ON [dbo].[CronogramaPagos_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.CuponeraOI_ConfirmarVencimiento.PRC'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CuponeraOI_ConfirmarVencimiento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CuponeraOI_ConfirmarVencimiento]
GO

---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 17/07/2017
-- Modificado por: Ian Pastor Mendoza
-- Nro. Orden de Trabajo: 10244
-- Descripcion del cambio: Se corrigió la grabación del monto de operación que estaba guardando el valor cero
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 20/12/2018
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11687
-- Descripcion del cambio: Se incluye vencimientos de bonos Swap.
---------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 14/02/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11825
--	Descripcion del cambio: Se agrega fecha de cronograma de pago de portafolio.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CuponeraOI_ConfirmarVencimiento](    
	@p_CodigoPortafolioSBS VARCHAR(20),    
	@p_CodigoNemonico VARCHAR(15),    
	@p_OrdenInversion VARCHAR(12),
	@p_FechaVencimiento NUMERIC(8),    
	@p_MontoNominalLocal NUMERIC(22,7),    
	@p_MonedaDestino VARCHAR(1),    
	@p_Secuencial  VARCHAR(5),    
	@decFechaIDI  NUMERIC(8),    
	@decFechaPago  NUMERIC(8),    
	@p_UsuarioModificacion VARCHAR(15),    
	@p_FechaModificacion NUMERIC(8),    
	@p_HoraModificacion VARCHAR(10),    
	@p_Host   VARCHAR(20),  
	@p_MontoOrigen NUMERIC(22,7)
)    
AS    
BEGIN
	IF EXISTS( SELECT 1 FROM OrdenInversion WHERE CodigoOrden = @p_OrdenInversion AND CategoriaInstrumento = 'CD' )
	BEGIN
		UPDATE 
			OrdenInversion 
		SET 
			FechaOperacion = @decFechaIDI,
			MontoOperacion = @p_MontoNominalLocal, 
			MontoNetoOperacion = @p_MontoNominalLocal
		WHERE 
			CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			AND CodigoMnemonico = @p_CodigoNemonico 
			AND CodigoOperacion IN ('35','38','39','22','23','24','25')    
			AND CodigoOrden=@p_OrdenInversion 
			
		EXEC OrdenInversion_ConfirmarOI @p_OrdenInversion,
										@p_CodigoPortafolioSBS,
										@p_OrdenInversion,
										''
	END
	ELSE
	BEGIN
		DECLARE 
			@CodigoMoneda VARCHAR(10),
			@DescripcionValor VARCHAR(50),
			@nvcCodigoOrden NVARCHAR(12),
			@CodigoMonedaNemonico VARCHAR(10),
			@MonedaPago VARCHAR(10),
			@MonedaOI VARCHAR(10),
			@MontoDestinoOI DECIMAL(22,7),
			@CodigoTercero VARCHAR(11),
			@CodigoMercado VARCHAR(3),
			@CodigoOperacion VARCHAR(6),
			@OrdenGenera VARCHAR(20)
			
		--Obteniendo el Codigo de Orden    
		SELECT 
			@nvcCodigoOrden = CodigoOrden,
			@MontoDestinoOI = MontoDestino, 
			@CodigoOperacion = CodigoOperacion, --RGF 20081119    
			@OrdenGenera = OrdenGenera
		FROM 
			OrdenInversion    
		WHERE 
			CodigoPortafolioSBS = @p_CodigoPortafolioSBS    
			AND CodigoMnemonico = @p_CodigoNemonico 
			AND CodigoOperacion IN ('35','38','39','22','23','24','25')      
			AND CodigoOrden = @p_OrdenInversion --Agregado por LC    
			
		IF (ISNULL(@p_MonedaDestino,'N')='N')    
		BEGIN
			UPDATE 
				OrdenInversion 
			SET 
				Estado = 'E-CON', 
				FechaOperacion = @decFechaIDI,
				MontoOperacion = @p_MontoNominalLocal, 
				MontoNetoOperacion = @p_MontoNominalLocal,
				TipoCambio = NULL,
				MontoDestino = NULL, 
				FechaLiquidacion = @decFechaPago	--HDG 20130719
			WHERE 
				CodigoPortafolioSBS = @p_CodigoPortafolioSBS     
				AND CodigoMnemonico = @p_CodigoNemonico 
				AND CodigoOperacion IN ('35','38','39','22','23','24','25')      
				AND CodigoOrden=@p_OrdenInversion --Agregado por LC        
		END
		ELSE
		BEGIN
			UPDATE 
				OrdenInversion 
			SET 
				Estado = 'E-CON', 
				FechaOperacion =  @decFechaIDI,
				MontoDestino = @p_MontoNominalLocal,  
				MontoOperacion = @p_MontoOrigen, 
				MontoNetoOperacion = @p_MontoOrigen, 
				FechaLiquidacion = @decFechaPago	--HDG 20130719
			WHERE 
				CodigoPortafolioSBS = @p_CodigoPortafolioSBS     
				AND CodigoMnemonico = @p_CodigoNemonico 
				AND CodigoOperacion IN ('35','38','39','22','23','24','25')      
				AND CodigoOrden = @p_OrdenInversion --Agregado por LC        
		END
		
		--> INICIO | Obtener Valores de Cronograma de Pagos
		DECLARE
				@fechaCronogramaPagos DECIMAL(8) = 0
				
		IF @CodigoOperacion IN ('35','38','39')
		BEGIN
			SET	@fechaCronogramaPagos = ISNULL((SELECT MIN(fechaCronogramaPagos) 
												FROM CronogramaPagos
												WHERE fechaCronogramaPagos >= @decFechaPago
													  AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS),@decFechaPago)
		END
  	    --> FIN | Obtener Valores de Cronograma de Pagos
  	    
		IF EXISTS (SELECT 1 FROM CuentasPorCobrarPagar WHERE CodigoOrden = @p_OrdenInversion and CodigoPortafolioSBS = @p_CodigoPortafolioSBS)
			UPDATE 
				CuentasPorCobrarPagar     
			SET 
				Importe = @p_MontoNominalLocal, 
				FechaVencimiento = @decFechaPago,
				SecuenciaCuponera = @p_Secuencial
			WHERE 
				CodigoOrden = @p_OrdenInversion 
				AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS    
		ELSE    
		BEGIN --Inicio NO EXISTE EN CuentasPorCobrarPagar    
			--Obteniendo el Monto Destino    
			SELECT 
				@MontoDestinoOI = MontoDestino 
			FROM 
				OrdenInversion    
			WHERE 
				CodigoPortafolioSBS = @p_CodigoPortafolioSBS    
				AND CodigoMnemonico = @p_CodigoNemonico 
				AND CodigoOperacion IN ('35','38','39','22','23','24','25')     
				AND CodigoOrden = @p_OrdenInversion --Agregado por LC
				
			IF @p_CodigoNemonico <> 'SWAP'
			BEGIN
				SELECT 
					@CodigoTercero = T.CodigoTercero,                
					@CodigoMoneda = CodigoMoneda                
				FROM 
					Valores V                 
				JOIN 
					Entidad E ON E.CodigoEntidad = V.CodigoEmisor                
				JOIN 
					Terceros T ON T.CodigoTercero = E.CodigoTercero                
				WHERE 
					CodigoNemonico = @p_CodigoNemonico    
			END
			ELSE
			BEGIN
				SELECT 
					@CodigoTercero = CodigoTercero,                
					@CodigoMoneda = CodigoMoneda                
				FROM 
					OrdenInversion    
				WHERE
					CodigoOrden = @p_OrdenInversion   
			END
				
			SELECT 
				@CodigoMercado = dbo.ObtenerCodigoMercado(@p_OrdenInversion)
				
			IF (ISNULL(@p_MonedaDestino,'N')='Y')       
			BEGIN
				SELECT 
					@CodigoMonedaNemonico = CodigoMoneda,
					@MonedaPago = MonedaPago    
				FROM 
					Valores    
				WHERE 
					CodigoNemonico=@p_CodigoNemonico    
					
				IF (@CodigoMonedaNemonico <> @MonedaPago AND ISNULL(@MontoDestinoOI,0)>0 AND @p_CodigoNemonico <> 'SWAP')    
				BEGIN
					SET @p_MontoNominalLocal = @MontoDestinoOI    
					SET @CodigoMoneda = @MonedaPago    
				END  
			END    
			SELECT 
				@DescripcionValor = Descripcion    
			FROM   
				Valores 
			WHERE 
				CodigoNemonico = @p_CodigoNemonico 
				   
			DECLARE 
				@LiquidaFechaAnt VARCHAR(1),
				@UsuarioLiqFecAnt VARCHAR(15)
			
			/*
			SE QUITO ESTA FUNCIONALIDAD PORQUE LA INTERFAZ DE LIQUIDACIÓN DE CXC Y CXP NO ESTÁ PREPARADA PARA SOPORTAR
			ESTA FUNCIONALIDAD
			If (SELECT COUNT(1) FROM tmpOrdenLiquidaFechaAnt WHERE 
				(CASE WHEN isNUMERIC(substring(NumeroOperacion,1,1)) = 1 then NumeroOperacion else substring(NumeroOperacion,2,len(NumeroOperacion)) end) = @p_OrdenInversion
				And CodigoPortafolioSBS = @p_CodigoPortafolioSBS) > 0
			BEGIN
				SET @LiquidaFechaAnt = '1'
				SET @UsuarioLiqFecAnt = @p_UsuarioModificacion
			END
			ELSE
			BEGIN
				SET @LiquidaFechaAnt = NULL
				SET @UsuarioLiqFecAnt = NULL
			END
			*/
			INSERT INTO 
				CuentasPorCobrarPagar(
									  CodigoPortafolioSBS,
									  NumeroOperacion,
									  CodigoOperacion,
									  Referencia,
									  CodigoOrden,
									  Importe,
									  FechaIngreso,
									  CodigoClaseCuenta,
									  NumeroCuenta,
									  CodigoMoneda,
									  CodigoMercado,
									  FechaOperacion,
									  FechaPago,
									  TipoMovimiento,
									  Egreso,
									  Situacion,
									  UsuarioCreacion,
									  HoraCreacion,
									  FechaCreacion,
									  UsuarioModificacion,
									  Host,
									  FechaModificacion,
									  HoraModificacion,
									  CodigoTercero,
									  Estado,
									  FechaVencimiento,
									  NumeroAsiento,
									  FechaAsiento,
									  SecuenciaCuponera,
									  CodigoNemonico,
									  LiquidaFechaAnt,
									  UsuarioLiqFecAnt,
									  fechaCronogramaPagos
									 )
			VALUES (@p_CodigoPortafolioSBS,
					@nvcCodigoOrden,
					@CodigoOperacion,
					@DescripcionValor,
					@nvcCodigoOrden,
					@p_MontoNominalLocal,
					@decFechaIDI,
					NULL,
					NULL,
					CASE WHEN LTRIM(RTRIM(@CodigoMoneda))='VAC' THEN 'NSOL' ELSE LTRIM(RTRIM(@CodigoMoneda)) END,
					@CodigoMercado,
					@decFechaIDI,
					NULL,
					'A',
					'N',
					'A',
					@p_UsuarioModificacion,
					CONVERT(VARCHAR(10),GETDATE(),108),
					CONVERT(VARCHAR(10),GETDATE(),112),
					NULL,
					@p_Host,
					NULL,
					NULL,
					@CodigoTercero,
					NULL,
					@decFechaPago,
					NULL,
					NULL,
					@p_Secuencial,
					@p_CodigoNemonico,
					@LiquidaFechaAnt,
					@UsuarioLiqFecAnt,
					(CASE WHEN @fechaCronogramaPagos = 0 THEN NULL ELSE @fechaCronogramaPagos END) 
				    )                  
		END
	    
		--Si se modifica La fecha de operacion de la OI ENTONCES fecha de VENCIMIENTO cambia  
		IF @p_CodigoNemonico <> 'SWAP'
		BEGIN
			UPDATE 
				CuponeraPagos  
			SET 
				FechaPago = @decFechaPago  
			WHERE 
				OrdenGenera = @p_OrdenInversion
		END
		ELSE 
		BEGIN
			IF @CodigoOperacion IN ('22','23','24','25')
			BEGIN
				UPDATE 
					Cuponera_Bono_Swap
				SET 
					GeneradoInteres = (CASE WHEN @CodigoOperacion = '22' THEN '1' ELSE GeneradoInteres END),
					GeneradoAmortizar = (CASE WHEN @CodigoOperacion = '23' THEN '1' ELSE GeneradoAmortizar END),
					GeneradoInteresOriginal = (CASE WHEN @CodigoOperacion = '24' THEN '1' ELSE GeneradoInteresOriginal END),
					GeneradoAmortizarOriginal = (CASE WHEN @CodigoOperacion = '25' THEN '1' ELSE GeneradoAmortizarOriginal END)
				WHERE 
					CodigoOrden = @OrdenGenera
					AND Correlativo = @p_Secuencial
			END
		END
	END
END


GO

GRANT EXECUTE ON [dbo].[CuponeraOI_ConfirmarVencimiento] TO [rol_sit_fondos] AS [dbo]
GO
PRINT 'dbo.SP_SIT_LIS_Swap.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_SIT_LIS_Swap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_SIT_LIS_Swap]
GO

----------------------------------------------------------------------------------------------------
-- Objetivo	: Lista la cartera por Swap
----------------------------------------------------------------------------------------------------
-- Parámetros:
--	1.- @p_CodigoPortafolioSBS - IN - varchar(10) - Codigo Portafolio
--	2.- @p_FechaValoracion - IN - numeric(8,0) - Fecha
----------------------------------------------------------------------------------------------------
-- Fecha de Modificación    : 07/02/2019
-- Modificado por  			: Ricardo Colonia
-- Nro. Orden de Trabajo	: OT11825
-- Descripción del Cambio	: Creación de SP
----------------------------------------------------------------------------------------------------  
CREATE PROC [dbo].[SP_SIT_LIS_Swap]
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_FechaValoracion NUMERIC(8,0)
AS
BEGIN
	SELECT
		P.Descripcion as Portafolio,
		'Fecha' = ISNULL(dbo.FormatDate(RVL.Fecha),''),
		'Codigo ISIN' = ISNULL(OI.CodigoISIN,''),
		Moneda = dbo.FN_SIT_OBT_CodigoMonedaSBS(OID.CodigoMonedaLeg2),
		Tercero = T.Descripcion,
		Entidad = E.CodigoEntidad,  
		'Fecha Emision' = ISNULL(dbo.FormatDate(OI.FechaOperacion),''),
		'Fecha Vencimiento' = ISNULL(dbo.FormatDate(OI.FechaContrato),''),
		'Valor Nominal' = ISNULL(OI.MontoNominalOrdenado,OI.MontoOperacion),
		'Valoración Moneda Local' =  ISNULL(RVL.Valorizacion,0),						
		'Portafolio_Nominal' = p.Descripcion + '_' + 
		                         CASE WHEN (ISNULL(oi.MontoNominalOrdenado,oi.MontoOperacion) - round(ISNULL(oi.MontoNominalOrdenado,oi.MontoOperacion),0,1)) > 0.5
		                              THEN CAST(CAST(ROUND(ISNULL(oi.MontoNominalOrdenado,oi.MontoOperacion),0) AS numeric(22,0)) AS varchar)
		                              ELSE CAST(CAST(ROUND(ISNULL(oi.MontoNominalOrdenado,oi.MontoOperacion),0,1) AS numeric(22,0)) AS varchar)
		                         END,
		'Portafolio_Entidad' = P.Descripcion + '_' + E.CodigoEntidad,
		'Codigo Moneda Compra' = CASE WHEN OI.CodigoOperacion = '21' 
		                                 THEN dbo.FN_SIT_OBT_CodigoMonedaSBS(OID.CodigoMonedaLeg1) 
		                                 ELSE dbo.FN_SIT_OBT_CodigoMonedaSBS(OID.CodigoMonedaLeg2) END,
		'Moneda Compra' = CASE WHEN OI.CodigoOperacion = '21' THEN OID.CodigoMonedaLeg1 ELSE OID.CodigoMonedaLeg2 END,
        'Monto Compra' = CASE WHEN OI.CodigoOperacion = '21' THEN OI.MontoOperacion ELSE OI.MontoNetoOperacion END,
		'Monto Total Interes Compra' = ISNULL(dbo.CalcularInteresAcumuladoSWAP(OI.CodigoOrden,OI.CodigoPortafolioSBS,1),0) ,
		'Codigo Moneda Venta' = CASE WHEN OI.CodigoOperacion = '21' 
		                                 THEN dbo.FN_SIT_OBT_CodigoMonedaSBS(OID.CodigoMonedaLeg2) 
		                                 ELSE dbo.FN_SIT_OBT_CodigoMonedaSBS(OID.CodigoMonedaLeg1) END,
        'Moneda Venta' = CASE WHEN OI.CodigoOperacion = '21' THEN OID.CodigoMonedaLeg2 ELSE OID.CodigoMonedaLeg1 END,
        'Monto Venta' = CASE WHEN OI.CodigoOperacion = '21' THEN OI.MontoNetoOperacion ELSE OI.MontoOperacion END,
        'Monto Total Interes Compra' = ISNULL(dbo.CalcularInteresAcumuladoSWAP(OI.CodigoOrden,OI.CodigoPortafolioSBS,2),0) ,
        'TipoSwap' = CASE WHEN OI.TipoCambioSpot = 1 THEN 'IRS' ELSE 'CCS' END,
		OI.TipoCambioSpot,
		'Plazo Vencimiento' = DATEDIFF(dd,dbo.fn_SIT_gl_ConvertirFechaPU(RVL.Fecha),dbo.fn_SIT_gl_ConvertirFechaPU(OI.FechaContrato)),
		'Estado' = CASE dbo.FN_SIT_OBT_EstadoCuentaxCobrarPagar(RVL.CodigoPortafolioSBS,OI.CodigoOrden)
		                   WHEN 'L' THEN 'Liquidado'
		                   WHEN ''  THEN 'Por Liquidar'
		                   WHEN '0' THEN 'Pendiente'
		            END,
        'Mercado' = M.Descripcion,
        'Tasa Implicita' = CASE WHEN OI.TipoCambioSpot = 1 THEN ((ISNULL(OID.TasaInteresLeg2,0)+ISNULL(VI2.Valor,0)) -
																(ISNULL(OID.TasaInteresLeg1,0)+ISNULL(VI1.Valor,0)))/100
														   ELSE (dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,'DOL',@p_FechaValoracion)/OI.TipoCambioSpot - 1) /
																 DATEDIFF(dd,dbo.fn_SIT_gl_ConvertirFechaPU(OI.FechaOperacion),dbo.fn_SIT_gl_ConvertirFechaPU(OI.FechaContrato)) 
																 * 365 END
						
	FROM 
		ReporteVL RVL
	JOIN 
		OrdenInversion OI ON OI.CodigoISIN  = RVL.CodigoValor  
							 AND OI.CodigoPortafolioSBS = RVL.CodigoPortafolioSBS
	JOIN
		OrdenInversion_DetalleSWAP OID ON OI.CodigoOrden = OID.CodigoOrden
										  AND OI.CodigoPortafolioSBS = OID.CodigoPortafolioSBS
	JOIN 
		Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	JOIN
		Terceros T ON T.CodigoTercero = OI.CodigoTercero 
					  AND t.Situacion = 'A' 
	JOIN 
		Entidad E ON E.CodigoTercero = T.CodigoTercero 
					 AND e.Situacion = 'A'  
	LEFT JOIN 
		Mercado M ON E.CodigoMercado = M.CodigoMercado 
					 AND M.Situacion = 'A'	
	LEFT JOIN
		ValorIndicador VI2 ON VI2.CodigoIndicador = OID.TasaFlotanteLeg2
						      AND VI2.Fecha = (SELECT MAX(Fecha) FROM ValorIndicador VIT
										       WHERE VIT.CodigoIndicador = OID.TasaFlotanteLeg2)
	LEFT JOIN
		ValorIndicador VI1 ON VI1.CodigoIndicador = OID.TasaFlotanteLeg1
						      AND VI1.Fecha = (SELECT MAX(Fecha) FROM ValorIndicador VIT
										       WHERE VIT.CodigoIndicador = OID.TasaFlotanteLeg1)
	WHERE
		RVL.Fecha = @p_FechaValoracion	
		AND RVL.ImprimeVL = '1'
		AND OI.Estado <> 'E-ELI'
		AND OI.Situacion = 'A'
		AND OI.CategoriaInstrumento = 'BS'
		AND OI.CodigoOperacion = '21'
END

GO


GRANT EXECUTE ON [dbo].[SP_SIT_LIS_Swap] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.AumentoCapital_ObtenerDistribucion.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AumentoCapital_ObtenerDistribucion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AumentoCapital_ObtenerDistribucion]
GO

-----------------------------------------------------------------------------------------------------------------------  
--Objetivo: Listar vencimientos de interes de la distribución de aumento de capital en un rango de fecha.
-----------------------------------------------------------------------------------------------------------------------   
-- Fecha de Modificación: 19/09/2018  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11568
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[AumentoCapital_ObtenerDistribucion]
(  
	@p_CodigoPortafolio VARCHAR(10),
	@p_FechaInicio NUMERIC(8),
	@p_FechaFin NUMERIC(8)  
)  
AS  
BEGIN  
	SELECT  
		dbo.ConvertirFecha(O.FechaOperacion) AS FechaOperacion  
		,CONVERT(DATE,dbo.ConvertirFecha(O.FechaOperacion),104) AS FechaOperacion_Flujo  
		,P.Descripcion AS CodigoPortafolioSBS  
		,O.HoraOperacion
		,M.CodigoMoneda AS Moneda  
		,TI.CodigoTipoInstrumento AS TipoInstrumento  
		,O.CodigoMnemonico  
		,OP.CodigoOperacion
		,O.TasaPorcentaje AS Tasa
		,ISNULL(O.InteresCorridoNegociacion ,0) InteresCorridoNegociacion  
		,(CASE WHEN v.TipoRenta = '2' THEN O.Precio ELSE O.PrecioNegociacionLimpio END) AS Precio  
		,O.MontoOrigen  
		,O.MontoOperacion AS MontoNetoOperacion  
		,O.CantidadOperacion  
		,C.CodigoEntidad AS Intermediario  
		,E.DescripcionEstado AS Estado  
		,O.CodigoOrden AS CodigoOrden  
		,O.CategoriaInstrumento  
		,ISNULL(ISNULL(O.TipoCambio,O.TipoCambioFuturo),0) TipoCambio  
		,DescOperacion = OP.Descripcion   
		,O.CodigoPortafolioSBS AS CodigoPortafolio  
		,dbo.ConvertirFecha(O.FechaLiquidacion) FechaLiquidacion  
		,CONVERT(DATE,dbo.ConvertirFecha(O.FechaLiquidacion),104) AS FechaLiquidacion_Flujo  
		,dbo.ConvertirFecha(O.FechaContrato) FechaContrato  
		,CONVERT(DATE,dbo.ConvertirFecha(O.FechaContrato),104) AS FechaContrato_Flujo  
		,ROUND(ISNULL(O.MontoNominalOperacion,0),2) MontoNominal  
		,DBO.fn_EquivalenciaNemonico(O.CodigoMnemonico,O.CodigoTipoCupon) EquivalenciaNemonico  
	FROM 
		OrdenInversion O  
	JOIN 
		Valores V ON O.codigoMnemonico=v.CodigoNemonico  
	JOIN 
		Entidad C ON O.CodigoTercero=C.CodigoTercero 
					 AND C.Situacion = 'A'  
	JOIN 
		EstadoOI E ON O.Estado=e.CodigoEstado  
	JOIN 
		Moneda M ON O.CodigoMoneda=m.CodigoMoneda  
	JOIN 
		Portafolio P ON O.CodigoPortafolioSBS = P.CodigoPortafolioSBS  
	JOIN 
		TipoInstrumento TI on v.CodigoTipoInstrumentoSBS=TI.CodigoTipoInstrumentoSBS  
	JOIN 
		Operacion OP on OP.CodigoOperacion = O.CodigoOperacion  
	JOIN
		AumentoCapitalDetalle ACD ON ACD.CodigoOrden = O.CodigoOrden
	JOIN 
		AumentoCapital AC ON AC.idAumentoCapital= ACD.idAumentoCapital
	JOIN
		CuentasPorCobrarPagar CCP ON CCP.CodigoPortafolioSBS = O.CodigoPortafolioSBS
									 AND CCP.CodigoOrden = O.CodigoOrden
									 AND CCP.CodigoOperacion = O.CodigoOperacion
	WHERE 
		O.CodigoPortafolioSBS = @p_CodigoPortafolio  
		AND CCP.fechaCronogramaPagos BETWEEN @p_FechaInicio AND @p_FechaFin  
		AND O.Situacion='A'  
		AND O.CodigoOperacion IN ('35')  
		AND O.Estado = 'E-CON'
		AND P.FlagAumentoCapital = 1
		AND AC.Estado = 'A'
	ORDER BY 
		O.FechaOperacion,
		M.CodigoMonedaSBS  
 
END  


GO

GRANT EXECUTE ON [dbo].[AumentoCapital_ObtenerDistribucion] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.sp_SIT_ins_PrecioValorCuota.PRC'
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

PRINT 'dbo.Valorizacion_listar.PRC'
USE [SIT-FONDOS]
GO

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
-- Fecha modificacion: 20/02/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11825    
-- Descripcion del cambio: Se adiciona validación de icono de advertencia cuando la fecha de valoración no coincide con la última fecha 
--						   de valorización.
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
	DECLARE @valorizacionMensual CHAR(1)
	
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
					SET @valorizacionMensual = ISNULL((SELECT ValoracionMensual FROM Portafolio WHERE CodigoPortafolioSBS = @CODIGOPORTAFOLIO),'')
					IF @CODIGOPORTAFOLIO <> '43' AND UPPER(@valorizacionMensual) <> 'S' SET @PIP = 2
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

PRINT 'dbo.Valores_VerificarCodigoPortafolioSBS.PRC'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_VerificarCodigoPortafolioSBS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Valores_VerificarCodigoPortafolioSBS]
GO

----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Verificar si existe el codigo de portafolio sbs asignado a un instrumento.
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 21/02/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11825
-- Descripción del cambio: Creación de Store Procedure
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[Valores_VerificarCodigoPortafolioSBS]
(  
	@codigoPortafolioSBS VARCHAR(10)
)
AS
BEGIN
	DECLARE @CodigoNemonico VARCHAR(15) = ISNULL((SELECT 
													 CodigoNemonico
												  FROM
											 		 Valores
												  WHERE
													 CodigoPortafolioSBS = @codigoPortafolioSBS),'')
													 
	SELECT @CodigoNemonico
	
END

GO

GRANT EXECUTE ON [dbo].[Valores_VerificarCodigoPortafolioSBS] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.SP_SIT_LIS_ValorCuota.PRC'
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

PRINT 'dbo.sp_ReporteDiarioOperaciones.PRC'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ReporteDiarioOperaciones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ReporteDiarioOperaciones]
GO
CREATE PROC [dbo].[sp_ReporteDiarioOperaciones] 
 @P_CodigoPortafolio VarChar(10), 
 @P_Fecha Numeric(8), 
 @P_TipoConsulta Char(1)
AS
BEGIN
	IF @P_TipoConsulta = '1'
		--Depositos
		SELECT OI.CodigoOrden, P.Descripcion Portafolio,OP.Descripcion TipoOperacion , T.Descripcion Tercero ,M.Descripcion Moneda, 
		CASE WHEN OI.CodigoTipoCupon = '1' THEN 'Nominal' ELSE 'Efectiva' END TipoTasa,OI.MontoNominalOperacion Nominal,OI.TasaPorcentaje Tasa,
		V.BaseTIR Base,OI.MontoOperacion MontoVencimiento,CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8), OI.FechaOperacion)), 103) FechaEmision,
		CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),OI.FechaContrato)), 103) FechaContrato, OI.Plazo Dias
		FROM OrdenInversion OI
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		JOIN Terceros T ON T.CodigoTercero  = OI.CodigoTercero 
		JOIN Moneda M ON M.CodigoMoneda = OI.CodigoMoneda
		JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico
		JOIN Operacion OP ON OP.CodigoOperacion  = OI.CodigoOperacion
		WHERE OI.CategoriaInstrumento = 'DP' AND OI.FechaOperacion <= @P_Fecha AND OI.FechaContrato > @P_Fecha AND OI.CodigoOperacion = '3' AND OI.Situacion = 'A' AND OI.Estado <> 'E-ELI'
		AND OI.CodigoPortafolioSBS = CASE WHEN @P_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @P_CodigoPortafolio END
		
		ORDER BY OI.CodigoPortafolioSBS , T.Descripcion ,OI.FechaContrato 
	ELSE IF @P_TipoConsulta = '2'
		--Forward
		SELECT OI.CodigoOrden,P.Descripcion Portafolio, OP.Descripcion TipoOperacion , T.Descripcion Tercero ,Mo.Descripcion MonedaOrigen,
		md.Descripcion MonedaDestino,
		CASE WHEN OI.CodigoOperacion = '94' THEN OI.MontoNominalOperacion ELSE OI.MontoCancelar END Nominal,OI.TipoCambioFuturo TipoCambio, CASE WHEN OI.Delibery = 'S' THEN 'Delivery' ELSE 'Non-Delivery' END ,
		CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),OI.FechaOperacion)), 103) FechaEmision,
		CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),OI.FechaContrato)), 103) FechaContrato ,
		OI.Plazo Dias
		FROM OrdenInversion OI
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		JOIN Terceros T ON T.CodigoTercero  = OI.CodigoTercero 
		JOIN Moneda Mo ON Mo.CodigoMoneda = OI.CodigoMonedaOrigen
		JOIN Moneda Md ON Md.CodigoMoneda = OI.CodigoMonedaDestino
		JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico
		JOIN Operacion OP ON OP.CodigoOperacion  = OI.CodigoOperacion
		WHERE OI.CategoriaInstrumento = 'FD' AND OI.FechaOperacion <= @P_Fecha  AND OI.FechaContrato > @P_Fecha AND OI.Situacion = 'A' AND OI.Estado <> 'E-ELI'
		AND OI.CodigoPortafolioSBS = CASE WHEN @P_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @P_CodigoPortafolio END
		ORDER BY OI.CodigoPortafolioSBS , T.Descripcion, OI.FechaContrato 
	ELSE IF @P_TipoConsulta = '3'
		--OpeReportes
		SELECT OI.CodigoOrden, P.Descripcion Portafolio, OI.CodigoMnemonico , ISNULL(OI.CantidadOrdenado, OI.CantidadOperacion)  CantidadOperacion  ,T.Descripcion Tercero ,Mo.Descripcion Moneda,
		OI.MontoOperacion,
		CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),OI.FechaOperacion)), 103) FechaEmision,
		CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),OI.FechaContrato)), 103) FechaContrato ,
		OI.Plazo Dias
		FROM OrdenInversion OI
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		JOIN Terceros T ON T.CodigoTercero  = OI.CodigoTercero 
		JOIN Moneda Mo ON Mo.CodigoMoneda = OI.CodigoMoneda
		JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico
		JOIN Operacion OP ON OP.CodigoOperacion  = '3'
		WHERE OI.CategoriaInstrumento = 'OR' AND OI.FechaOperacion <= @P_Fecha AND OI.FechaContrato > @P_Fecha AND OI.Situacion = 'A' AND OI.Estado <> 'E-ELI' AND OI.CodigoOperacion IN ('101','102','3')
		AND OI.CodigoPortafolioSBS = CASE WHEN @P_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @P_CodigoPortafolio END
		ORDER BY OI.CodigoPortafolioSBS , T.Descripcion, OI.FechaContrato 
	ELSE IF @P_TipoConsulta = '4'
	BEGIN
		--Actualiza masivamente los saldos de custodio
		EXEC sp_SIT_ActualizaSaldoCustodioMasivo @P_CodigoPortafolio,@P_Fecha
		--Tenencias
		SELECT P.Descripcion Portafolio, CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),CS.FechaSaldo)), 103) FechaSaldo, CS.CodigoCustodio,V.CodigoNemonico, 
		V.CodigoSBS, V.CodigoISIN, ISNULL(CS.SaldoInicialUnidades,0) AS SaldoInicialUnidades , ISNULL(CS.IngresoUnidades,0) AS IngresoUnidades , ISNULL(CS.EgresoUnidades,0) AS EgresoUnidades
		,ISNULL(CS.SaldoInicialUnidades,0) + ISNULL(CS.IngresoUnidades,0) - ISNULL(CS.EgresoUnidades,0)
		FROM CustodioSaldo CS
		JOIN Valores V ON V.CodigoNemonico = CS.CodigoMnemonico AND (ISNULL(V.FechaVencimiento,99999999) >= @P_Fecha OR V.FechaVencimiento = 0)
		JOIN Portafolio P ON P.CodigoPortafolioSBS = CS.CodigoPortafolioSBS  
		WHERE CS.FechaSaldo = @P_Fecha AND (ISNULL(CS.SaldoInicialUnidades,0) + ISNULL(CS.IngresoUnidades,0) - ISNULL(CS.EgresoUnidades,0)) > 0
		AND CS.CodigoPortafolioSBS = CASE WHEN @P_CodigoPortafolio = '' THEN  CS.CodigoPortafolioSBS ELSE @P_CodigoPortafolio END
	END
END
GO

GRANT EXECUTE ON [dbo].[sp_ReporteDiarioOperaciones] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.ReporteGestion_ReporteConsolidado.PRC'
USE [SIT-FONDOS]
GO
/*
exec ReporteGestion_ReporteConsolidado @p_CodigoPortafolio = '2666,2777', @p_FechaProceso = 20190131
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
	FROM ValorizacionAmortizada VA
		INNER JOIN OrdenInversion OI ON OI.CodigoOrden = VA.CodigoOrden AND OI.CodigoPortafolioSBS = VA.CodigoPortafolioSBS
		INNER JOIN Valores V ON VA.CodigoNemonico = V.CodigoNemonico
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		LEFT JOIN ParametrosGenerales PG ON PG.Clasificacion = 'RATING' AND V.Rating = PG.Valor
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
		,V.CodigoMoneda
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
		,OI.MontoNominalOrdenado * dbo.RetornarSecuenciaVectorTipoCambio(CTV.CodigoPortafolioSBS, OI.CodigoMoneda, @p_FechaProceso) AS TIRRAZ_ValorActual_PrecioLimpio
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
	FROM CarteraTituloValoracion CTV
		INNER JOIN Portafolio P ON CTV.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON V.CodigoNemonico = CTV.CodigoMnemonico
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN OrdenInversion OI ON OI.CategoriaInstrumento IN ('DP','OR') AND OI.FechaOperacion = CTV.FechaValoracion
			AND OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN ('E-EJE','E-CON')
		INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero
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
	FROM OrdenInversion OI
		INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
		INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico AND V.TipoRenta = '2'
		INNER JOIN TipoInstrumento TI ON V.CodigoTipoInstrumentoSBS = TI.CodigoTipoInstrumentoSBS
		INNER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
		INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
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






PRINT 'dbo.ReporteGestion_OperacionesVencimientosOTC.PRC'
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




PRINT 'FIN --- > SECCIÓN DE SP'

IF @@ERROR <> 0 BEGIN
    ROLLBACK TRANSACTION __Transaction_SP
    PRINT 'ERROR: ROLLBACK SP'
END
ELSE
    COMMIT TRANSACTION __Transaction_SP
GO 