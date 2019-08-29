USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log_Datos
PRINT 'Agregar Campo FuenteLibor a la tabla dbo.Indicador'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Indicador]') and upper(name) = upper('FuenteLibor'))
ALTER TABLE Indicador add FuenteLibor VARCHAR(4);

PRINT 'Agregar Campo MostrarLibor a la tabla dbo.Indicador'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Indicador]') and upper(name) = upper('MostrarLibor'))
ALTER TABLE Indicador add MostrarLibor VARCHAR(4);

PRINT 'Agregar Campo MesLibor a la tabla dbo.Indicador'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Indicador]') and upper(name) = upper('MesLibor'))
ALTER TABLE Indicador add MesLibor int;


PRINT 'Agregar Campo Ficticia a la tabla dbo.PrevOrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrevOrdenInversion]') and upper(name) = upper('Ficticia'))
ALTER TABLE PrevOrdenInversion add Ficticia VARCHAR(1);

PRINT 'Agregar Campo RegulaSBS a la tabla dbo.PrevOrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrevOrdenInversion]') and upper(name) = upper('RegulaSBS'))
ALTER TABLE PrevOrdenInversion add RegulaSBS VARCHAR(1);

PRINT 'Modificar Campo Cantidad a numeric(22,7) de la tabla dbo.PrevOrdenInversion'
ALTER TABLE PrevOrdenInversion
ALTER COLUMN Cantidad numeric (22,7);

PRINT 'Modificar Campo CantidadOperacion a numeric(22,7) de la tabla dbo.PrevOrdenInversion'
ALTER TABLE PrevOrdenInversion
ALTER COLUMN CantidadOperacion numeric (22,7);

PRINT ' '
PRINT 'INICIO --- > Creacion Tabla ValorCuota_Variacion'
IF NOT EXISTS (SELECT 1 FROM sys.tables  WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota_Variacion]')  and type_desc='USER_TABLE')
CREATE TABLE ValorCuota_Variacion(
CodigoPortafolioSBS varchar(10),
CodigoSerie  varchar(100),
FechaProceso numeric(8,0),
CarteraPrecio numeric(22,7),
CarteraTipoCambio numeric(22,7),
Derivados numeric(22,7),
CuentasPorCobrarPrecio numeric(22,7),
CuentasPorCobrarTipoCambio numeric(22,7),
CuentasPorPagarPrecio numeric(22,7),
CuentasPorPagarTipoCambio numeric(22,7),
CajaTipoCambio numeric(22,7),
Comision numeric(22,7),
PorcentageVariacionEstimado numeric(22,7),
TotalRentabilidadInversiones numeric(22,7),
PorcentageVariacionSIT  numeric(22,7),
DiferenciaEstimadoSIT  numeric(22,7),
EstadoSemaforo varchar(1),
HoraCreacion varchar(10),
FechaCreacion numeric(8,0),
UsuarioCreacion varchar(15),
HoraModificacion varchar(10),
FechaModificacion numeric(8,0),
UsuarioModificacion varchar(15)
)
PRINT 'FIN --- > Creacion Tabla ValorCuota_Variacion'






IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log_Datos
ELSE
    COMMIT TRANSACTION __Transaction_Log_Datos
GO 


USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log



PRINT ' '
PRINT 'INICIO --- > Eliminacion de registro parametrizables'
delete from dbo.ParametrosGenerales where Clasificacion='VALORCUOTA_SEMAFORO' and Valor='0.5'
delete from dbo.ParametrosGenerales where Clasificacion='ImporSBS' and Valor='TB'
delete from dbo.ParametrosGenerales where Clasificacion='FUENTE_LIBOR' 
PRINT 'FIN --- > Eliminacion de registro parametrizables'


PRINT ' '
PRINT 'INICIO --- > Insercion de registro parametrizables'
INSERT INTO ParametrosGenerales
(Clasificacion,
Nombre,
Valor,
Comentario
)VALUES
(
'VALORCUOTA_SEMAFORO',
'semaforo valor cuota',
'0.5',
'semaforo valor cuota'
)



INSERT INTO ParametrosGenerales
(Clasificacion,
Nombre,
Valor,
Comentario
)VALUES
(
'ImporSBS',
'Vector Tasa Libor Bloomberg',
'TB',
''
)

INSERT INTO ParametrosGenerales
(Clasificacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FUENTE_LIBOR',
'PIP',
'PIP',
''
)
INSERT INTO ParametrosGenerales
(Clasificacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FUENTE_LIBOR',
'Bloomberg',
'BLO',
''
)
PRINT 'FIN --- > Insercion de registro parametrizables'




PRINT 'INICIO --- > Actualizacion de Indicadores con nuevo codigo LIBOR'
delete from ValorIndicador where CodigoIndicador like 'SW%'


UPDATE Indicador
SET 
CodigoIndicador='SW-3ML-P',
NombreIndicador='3M - LIBOR - PIP',
FuenteLibor ='PIP',
MostrarLibor='S',
MesLibor=3
WHERE CodigoIndicador='SW-3MLIB'

UPDATE Indicador
SET 
CodigoIndicador='SW-6ML-P',
NombreIndicador='6M - LIBOR - PIP',
FuenteLibor ='PIP',
MostrarLibor='S',
MesLibor=6
WHERE CodigoIndicador='SW-6MLIB'


INSERT INTO	ValorIndicador
(Secuencia,CodigoIndicador,DiasPeriodo,Fecha, Valor, Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion, Host)
VALUES
(600157,'SW-3ML-P',0,20190120,2.8030000,'A','SYSTEM',20190120,'20:18:55',null )

INSERT INTO	ValorIndicador
(Secuencia,CodigoIndicador,DiasPeriodo,Fecha, Valor, Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion, Host)
VALUES
(600158,'SW-6ML-P',0,20190120,1.8000000,'A','SYSTEM',20190120,'20:18:55',null )

INSERT INTO	ValorIndicador
(Secuencia,CodigoIndicador,DiasPeriodo,Fecha, Valor, Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion, Host)
VALUES
(600159,'SW-3ML-P',0,20190325,2.6087500,'A','P500777',20190412,'12:05:55','10.219.120.82' )
INSERT INTO	ValorIndicador
(Secuencia,CodigoIndicador,DiasPeriodo,Fecha, Valor, Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion, Host)
VALUES
(600160,'SW-3ML-P',0,20190503,2.5917500,'A','P500775',20190506,'10:00:32','10.219.120.82' )

IF NOT EXISTS  (SELECT * FROM INDICADOR WHERE CodigoIndicador='SW-6ML-B')
INSERT INTO Indicador (
CodigoIndicador,
NombreIndicador,
ManejaPeriodo,
TipoIndicador,
CodigoPeriodicidad,
NumeroPeriodo,
BaseTasa,
Vigencia,
Situacion,
UsuarioCreacion,
FechaCreacion,
HoraCreacion,
UsuarioModificacion,
FechaModificacion,
HoraModificacion,
Host,
FuenteLibor,
MostrarLibor,
MesLibor
) SELECT
'SW-6ML-B',
'6M - LIBOR - BLO',
ManejaPeriodo,
TipoIndicador,
CodigoPeriodicidad,
NumeroPeriodo,
BaseTasa,
Vigencia,
Situacion,
UsuarioCreacion,
FechaCreacion,
HoraCreacion,
UsuarioModificacion,
FechaModificacion,
HoraModificacion,
Host,
'BLO',
MostrarLibor,
MesLibor  
FROM 
Indicador
WHERE CodigoIndicador='SW-6ML-P'


IF NOT EXISTS  (SELECT * FROM INDICADOR WHERE CodigoIndicador='SW-3ML-B')
INSERT INTO Indicador (
CodigoIndicador,
NombreIndicador,
ManejaPeriodo,
TipoIndicador,
CodigoPeriodicidad,
NumeroPeriodo,
BaseTasa,
Vigencia,
Situacion,
UsuarioCreacion,
FechaCreacion,
HoraCreacion,
UsuarioModificacion,
FechaModificacion,
HoraModificacion,
Host,
FuenteLibor,
MostrarLibor,
MesLibor
) SELECT
'SW-3ML-B',
'3M - LIBOR - BLO',
ManejaPeriodo,
TipoIndicador,
CodigoPeriodicidad,
NumeroPeriodo,
BaseTasa,
Vigencia,
Situacion,
UsuarioCreacion,
FechaCreacion,
HoraCreacion,
UsuarioModificacion,
FechaModificacion,
HoraModificacion,
Host,
'BLO',
MostrarLibor,
MesLibor  
FROM 
Indicador
WHERE CodigoIndicador='SW-3ML-P'


PRINT 'FIN --- > Actualizacion de Indicadores con nuevo codigo LIBOR'
PRINT ' '






PRINT 'INICIO --- > Actualizacion de OrdenInversion_DetalleSWAP con nuevo codigo LIBOR'

UPDATE [dbo].[OrdenInversion_DetalleSWAP]
SET TasaFlotanteLeg2='SW-3ML-P'

PRINT 'FIN --- > Actualizacion de OrdenInversion_DetalleSWAP con nuevo codigo LIBOR'

PRINT 'INICIO --- > Functions'
PRINT '[dbo].[fnObtenerFilaSplit]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnObtenerFilaSplit]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnObtenerFilaSplit]
GO

---------------------------------------------------------------------------------------------------------------------
--Objetivo: Obtener subcadena, de una cadena, que se divide  por medio de caracteres
---------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 08/05/2019
--	Creado por: Ernesto Galarza
--	Descripcion del cambio: Creacion
--  Nro. Orden de Trabajo: 12025
---------------------------------------------------------------------------------------------------------------------

create FUNCTION [dbo].[fnObtenerFilaSplit] 
( 
    @cadena NVARCHAR(MAX), 
    @delimiter CHAR(1),
	@fila int
) 
RETURNS Varchar(100) 
BEGIN 
    DECLARE @salida Varchar(100)=''
	set @salida = (select Cadena  from
	(
	SELECT 
	ROW_NUMBER() OVER(ORDER BY (select 1) desc) AS Fila,
	splitdata as Cadena
	 FROM [dbo].[fnSplitString] (@cadena ,@delimiter)) t where t.Fila=@fila
	 )
    RETURN isnull(@salida,'')
END
GO
GRANT EXECUTE ON [dbo].[fnObtenerFilaSplit] TO [rol_sit_fondos] AS [dbo]
GO
PRINT 'FIN --- > Functions'
PRINT ' '
PRINT 'INICIO --- > Store Procedures'

PRINT '[dbo].[pr_SIT_gl_PrevOrdenInversion_SeleccionarPorCodigoOrden]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_SIT_gl_PrevOrdenInversion_SeleccionarPorCodigoOrden]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pr_SIT_gl_PrevOrdenInversion_SeleccionarPorCodigoOrden]
GO
-----------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Obtener datos de la preorden a partir del codigo de orden
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 10/05/2019  
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Creacion del store procedure
-----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[pr_SIT_gl_PrevOrdenInversion_SeleccionarPorCodigoOrden]
@p_CodigoOrden varchar(12)
AS
BEGIN

SELECT pre.* 
FROM PrevOrdenInversion pre 
INNER JOIN PrevOrdenInversion_OI preOrd ON pre.CodigoPrevOrden =  preOrd.CodigoPrevOrden
  WHERE preOrd.CodigoOrden=@p_CodigoOrden

END
GO


GRANT EXECUTE ON [dbo].[pr_SIT_gl_PrevOrdenInversion_SeleccionarPorCodigoOrden] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[pr_SIT_ins_PrevOrdenInversion_Insertar_detalle]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_SIT_ins_PrevOrdenInversion_Insertar_detalle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pr_SIT_ins_PrevOrdenInversion_Insertar_detalle]
GO
----------------------------------------------------------------------------------------------------------------------------
--	Objetivo: Inserta las preOrdenes
----------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 25/04/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10090
--	Descripcion del cambio: Se orden el procedimiento, y se adecuo para masivos nuevos
----------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 23/08/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar el parámetro "@p_TipoValorizacion"
----------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 15/05/2019
--	Modificado por: Ernesto Galarza C.
--	Nro. Orden de Trabajo: 12025
--	Descripcion del cambio: Agregar el parámetro "@p_Ficticia" y "@p_RegulaSBS"
----------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[pr_SIT_ins_PrevOrdenInversion_Insertar_detalle]
	@p_FechaOperacion  NUMERIC(8) = NULL,
	@p_HoraOperacion  VARCHAR(10) = NULL,
	@p_UsuarioCreacion  VARCHAR(15) = NULL,
	@p_CodigoNemonico  VARCHAR(15) = NULL,
	@p_CodigoOperacion  VARCHAR(6) = NULL,
	@p_Cantidad     NUMERIC(22,7) = NULL,
	@p_Precio      NUMERIC(22,7) = NULL,
	@p_TipoCondicion   VARCHAR(5) = NULL,
	@p_CodigoTercero   VARCHAR(11) = NULL,
	@p_CodigoPlaza   VARCHAR(3) = NULL,
	@p_IntervaloPrecio   NUMERIC(22,7) = NULL,
	@p_CantidadOperacion NUMERIC(22,7) = NULL,
	@p_PrecioOperacion  NUMERIC(22,7) = NULL,
	@p_AsignacionF1   NUMERIC(22,7) = NULL,
	@p_AsignacionF2   NUMERIC(22,7) = NULL,
	@p_AsignacionF3   NUMERIC(22,7) = NULL,
	@p_MedioNegociacion VARCHAR(1) = NULL,
	@p_Tasa      NUMERIC(22,7) = NULL,
	@p_Situacion     VARCHAR(1) = NULL,
	@p_FechaCreacion   NUMERIC(8) = NULL,
	@p_HoraCreacion   VARCHAR(10) = NULL,
	@p_Host      VARCHAR(20) = NULL,
	@p_Estado     VARCHAR(5) = NULL,
	@p_TipoRenta    VARCHAR(5) = NULL,
	@p_FechaLiquidacion  NUMERIC(8) = NULL,
	@p_TipoTasa     VARCHAR(1) = NULL,
	@p_MonedaNegociada VARCHAR(10) = NULL,
	@p_Moneda     VARCHAR(10) = NULL,
	@p_CodigoTipoTitulo  VARCHAR(15) = NULL,
	@p_ModalidadForward VARCHAR(1) = NULL,
	@p_PrecioFuturo   NUMERIC(22, 7) = NULL,
	@p_CodigoMotivo   VARCHAR(5) = NULL,
	@p_FechaContrato  NUMERIC(8) = NULL,
	@p_ClaseInstrumentoFx VARCHAR(5) = NULL,
	@p_MontoNominal   NUMERIC(22,7) = NULL,
	@p_MontoOperacion  NUMERIC(22,7) = NULL,
	@p_IndPrecioTasa  char(1) = NULL,
	@p_CodigoContacto  VARCHAR(3) = NULL,
	@p_TipoFondo   VARCHAR(10) = NULL,
	@p_TipoTramo   VARCHAR(10) = NULL,
	@p_HoraEjecucion  VARCHAR(10) = NULL,
	@p_VencimientoAno  VARCHAR(10) = NULL,
	@p_VencimientoMes  VARCHAR(10) = NULL,
	@p_TotalExposicion  NUMERIC(22,7) = NULL,
	@p_Fixing    NUMERIC(22,7) = NULL,
	@p_porcentaje VARCHAR(1),
	@p_detalle XML,
	@p_InteresCorrido NUMERIC(22,7) = 0,
	@p_TipoValorizacion VARCHAR(10),
	@p_Ficticia  VARCHAR(1) = NULL,
	@p_RegulaSBS  VARCHAR(1)  = NULL
AS      
SET NOCOUNT ON
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY

		DECLARE @p_CodigoPrevOrdenInversion NUMERIC(12), @p_Correlativo NUMERIC(12)
		SELECT @p_codigoPrevOrdenInversion = ISNULL(MAX(CodigoPrevOrden),0) + 1 FROM PrevOrdenInversion

		IF (ISNULL(@p_CodigoOperacion,'') = '') 
			-- INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
			RAISERROR ('Uno o más de los siguientes campos no se ha indicado: Operación', 16, 1);
			-- FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion

		IF @p_TipoRenta <> '3' AND @p_TipoRenta <> 'FX' --10090
		BEGIN	
			SELECT @p_Correlativo = ISNULL(MAX(p.Correlativo),0) + 1 FROM  PrevOrdenInversion p       
			INNER JOIN Valores v ON p.CodigoNemonico = v.CodigoNemonico      
			WHERE  p.FechaOperacion = @p_FechaOperacion AND v.TipoRenta = @p_TipoRenta AND p.CodigoNemonico NOT IN(
				SELECT CodigoTipoTitulo
				FROM TipoTitulo WHERE CodigoTipoRenta = 1 AND Situacion = 'A' AND CodigoTipoTitulo LIKE 'DPZ%'
			) AND p.CodigoNemonico <> 'DIVISA'
						
			if (isNULL(@p_TipoCondicion,'') = '' or isNULL(@p_CodigoPlaza,'') = '')		
			BEGIN
				-- INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
				RAISERROR ('Uno o más de los siguientes campos no se ha indicado: Tipo Condición o Plaza Neg.', 16, 1);
				-- FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
			END
		END
		ELSE
		BEGIN			
			IF @p_ClaseInstrumentoFx = 'FT'
			BEGIN
				SELECT @p_Correlativo = ISNULL(MAX(p.Correlativo),0) + 1 FROM PrevOrdenInversion p
				WHERE p.FechaOperacion = @p_FechaOperacion AND p.ClaseInstrumentoFx = @p_ClaseInstrumentoFx
			END
			ELSE IF @p_ClaseInstrumentoFx = 'DP' OR @p_ClaseInstrumentoFx = 'OR' --10090
			BEGIN      
				SELECT @p_Correlativo = ISNULL(MAX(p.Correlativo),0) + 1 FROM  PrevOrdenInversion p
				WHERE p.FechaOperacion = @p_FechaOperacion AND p.CodigoNemonico IN (SELECT CodigoTipoTitulo FROM TipoTitulo WHERE CodigoTipoRenta = 1 AND Situacion = 'A')
			END
			ELSE
			BEGIN
				SELECT @p_Correlativo = ISNULL(MAX(p.Correlativo),0) + 1 FROM  PrevOrdenInversion p
				WHERE p.FechaOperacion = @p_FechaOperacion AND p.CodigoNemonico IN('DIVISA','FORWARD')

				IF ISNULL(@p_ClaseInstrumentoFx,'') = ''
					-- INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
					RAISERROR ('Uno o más de los siguientes campos no se ha indicado: Operación, Clase Instrumento Fx', 16, 1);
					-- FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
			END      
		END
		
		INSERT INTO PrevOrdenInversion( CodigoPrevOrden,Correlativo,FechaOperacion,HoraOperacion,UsuarioCreacion,CodigoNemonico,CodigoOperacion,Cantidad,Precio,
			TipoCondicion,CodigoTercero,CodigoPlaza,IntervaloPrecio,CantidadOperacion,PrecioOperacion,AsignacionF1,AsignacionF2,AsignacionF3,MedioNegociacion,Tasa,
			Situacion,FechaCreacion,HoraCreacion,Host,Estado,FechaLiquidacion,TipoTasa,IndPrecioTasa,MontoOperacion,MontoNominal,MonedaNegociada,Moneda,CodigoTipoTitulo,
			ModalidadForward,PrecioFuturo,CodigoMotivo,FechaContrato,ClaseInstrumentoFx,CodigoContacto,TipoFondo,TipoTramo,HoraEjecucion,VencimientoMes,VencimientoAno,
			TotalExposicion,Fixing,Porcentaje, InteresCorrido,TipoValorizacion,Ficticia, RegulaSBS)      
		VALUES ( @p_CodigoPrevOrdenInversion,@p_Correlativo,@p_FechaOperacion,@p_HoraOperacion,@p_UsuarioCreacion,@p_CodigoNemonico,@p_CodigoOperacion,@p_Cantidad,@p_Precio,
			@p_TipoCondicion,@p_CodigoTercero,@p_CodigoPlaza,@p_IntervaloPrecio,@p_Cantidad,@p_PrecioOperacion,@p_AsignacionF1,@p_AsignacionF2,@p_AsignacionF3,
			@p_MedioNegociacion,@p_Tasa,@p_Situacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host,@p_Estado,@p_FechaLiquidacion,@p_TipoTasa,@p_IndPrecioTasa,@p_MontoOperacion,
			@p_MontoNominal,@p_MonedaNegociada,@p_Moneda,@p_CodigoTipoTitulo,@p_ModalidadForward,@p_PrecioFuturo,@p_CodigoMotivo,@p_FechaContrato,@p_ClaseInstrumentoFx,
			@p_CodigoContacto,@p_TipoFondo,@p_TipoTramo,@p_HoraEjecucion,@p_VencimientoMes,@p_VencimientoAno,@p_TotalExposicion,@p_Fixing,@p_porcentaje,@p_InteresCorrido,@p_TipoValorizacion,@p_Ficticia, @p_RegulaSBS)
		
		DECLARE @i int    
		EXECUTE SP_XML_PREPAREDOCUMENT @i OUTPUT,@p_detalle    
		INSERT INTO PrevOrdenInversion_Portafolio (CodigoPrevOrden,CodigoPortafolio,Asignacion)    
		SELECT @p_CodigoPrevOrdenInversion,CodigoPortafolio,Asignacion FROM OPENXML (@i,'/Detalles/Detalle',2)
		WITH (CodigoPortafolio VARCHAR(20),Asignacion NUMERIC(22,7)) as XMLdetalle
		EXECUTE SP_XML_REMOVEDOCUMENT @i  

		COMMIT TRANSACTION
	END try  
	BEGIN catch  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		SELECT ERROR_MESSAGE()  
	END catch
END


GRANT EXECUTE ON [dbo].[pr_SIT_ins_PrevOrdenInversion_Insertar_detalle] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[pr_SIT_ins_InsertarOIRV_PrevOrdenInversion_Sura]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_SIT_ins_InsertarOIRV_PrevOrdenInversion_Sura]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pr_SIT_ins_InsertarOIRV_PrevOrdenInversion_Sura]
GO

---------------------------------------------------------------------------------------------
--Objetivo: Inserta orden de RV
---------------------------------------------------------------------------------------------
--	Fecha modificacion: 26/04/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10090
--	Descripcion del cambio: Se orden el procedimiento y se considera el Medio de negociacion
---------------------------------------------------------------------------------------------
--	Fecha modificacion: 17/05/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 12025
--	Descripcion del cambio: Se agregaron las variables Ficticia y RegulaSBS para las preordenes que se hayan creado en Ordenes de Fondo
---------------------------------------------------------------------------------------------
CREATE PROC [dbo].[pr_SIT_ins_InsertarOIRV_PrevOrdenInversion_Sura]  
	@p_FechaOperacion NUMERIC(8),
	@p_UsuarioCreacion VARCHAR(15) = '',
	@p_FechaCreacion NUMERIC(8) = null,
	@p_HoraCreacion VARCHAR(10) = '',
	@p_Host VARCHAR(20) = '',
	@p_NProceso NUMERIC(8,0) = 0
AS
BEGIN
	SET NOCOUNT ON  
	CREATE TABLE #TmpFechaVencimiento(FechaVencimiento VARCHAR(10))
	CREATE TABLE #TmpComisiones(Id INT identity,CodigoComision VARCHAR(20),ValorComision NUMERIC(22,7))
	CREATE TABLE #TmpCodigoOI(Id INT identity,CodigoOrden  VARCHAR(12))
	DECLARE @CodigoNemonico VARCHAR(15),@CodigoTercero VARCHAR(15),@CodigoOperacion VARCHAR(5),@PrecioPromedio NUMERIC(22,7),@Fondo VARCHAR(15),@Cantidad NUMERIC(22,7),
	@EstadoLimites VARCHAR(5),@EstadoBroker VARCHAR(5),@CodigoISIN VARCHAR(12),@CodigoSBS VARCHAR(12),@CodigoTipoInstrumentoSBS VARCHAR(3),@CodigoTipoTitulo VARCHAR(15),
	@CodigoMoneda VARCHAR(10),@CategoriaInstrumento VARCHAR(4),@FechaVencimiento NUMERIC(8),@CodigoPlaza VARCHAR(5),@EntidadEmisora VARCHAR(3),@EntidadComisionista VARCHAR(3),
	@EntidadCustodio VARCHAR(3),@EntidadFinanciera VARCHAR(3),@EntidadAval VARCHAR(3),@EntidadRiesgocero VARCHAR(3),@EntidadVinculada VARCHAR(3),@EntidadVigilada VARCHAR(3),
	@EntidadReguladora VARCHAR(3),@EntidadBroker VARCHAR(3),@GrupoIntermediario VARCHAR(5),@CantidadOrdenado NUMERIC(22,7),@CantidadOperacion NUMERIC(22,7),@Precio NUMERIC(22,7),
	@Estado VARCHAR(5),@CodigoEntidad VARCHAR(15),@CodigoComision VARCHAR(15),@ValorComision NUMERIC(22,7),@TotalComisiones NUMERIC(22,7),@PrecioPromedioOp NUMERIC(22,7),
	@MontoOperacion NUMERIC(22,7),@MontoNetoOperacion NUMERIC(22,7),@p_codigoOrdenInversion VARCHAR(12),@CodigoRenta VARCHAR(1),@TipoCondicion VARCHAR(5),@TipoTramo VARCHAR(10),
	@TipoFondo VARCHAR(10),@CodigoContacto VARCHAR(3),@HoraOperacion VARCHAR(10),@MedioNegociacion VARCHAR(10),@HoraEjecucion VARCHAR(10),@CodigoPrevOrden NUMERIC(12),
	@Correlativo NUMERIC(12),@Asignacion NUMERIC(22,7),@AsignacionF1 NUMERIC(22,7),@AsignacionF2 NUMERIC(22,7),@AsignacionF3 NUMERIC(22,7),@antCodigoPrevOrden NUMERIC(12),
	@Id INT,@IdMax INT,@Id2 INT,@IdMax2 INT,@Ficticia VARCHAR(1),@RegulaSBS VARCHAR(1)
	--
	SET @antCodigoPrevOrden = 0  
	SET @CodigoRenta = '2'  
	DECLARE TmpPrevOrdenInversion_Cursor CURSOR FOR  
	SELECT CodigoNemonico, CodigoOperacion, CodigoTercero, Fondo, Cantidad, PrecioPromedio, ISNULL(EstadoLimites,''), ISNULL(EstadoBroker,''), Id  
	FROM TmpPrevOrdenInversion WHERE NProceso = @p_NProceso order by Id  
	OPEN TmpPrevOrdenInversion_Cursor  
	FETCH NEXT FROM TmpPrevOrdenInversion_Cursor INTO @CodigoNemonico, @CodigoOperacion, @CodigoTercero, @Fondo, @Cantidad, @PrecioPromedio, @EstadoLimites, @EstadoBroker, @Id2  
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		IF (SELECT COUNT(1) FROM TmpPrevOrdenInversion p 
		JOIN PrevOrdenInversion po ON p.CodigoNemonico = po.CodigoNemonico AND p.CodigoOperacion = po.CodigoOperacion AND p.CodigoTercero = po.CodigoTercero
		AND po.FechaOperacion = @p_FechaOperacion AND Situacion = 'A'  
		WHERE CantidadOperacion = 0 AND PrecioOperacion = 0 AND p.Id = @Id2) > 0
		BEGIN  
			UPDATE PrevOrdenInversion SET Flag = '0',Estado = 'EJE'      
			WHERE CodigoPrevOrden IN (SELECT CodigoPrevOrden FROM TmpPrevOrdenInversion p  
			JOIN PrevOrdenInversion po ON p.CodigoNemonico = po.CodigoNemonico AND p.CodigoOperacion = po.CodigoOperacion AND p.CodigoTercero = po.CodigoTercero  
			AND po.FechaOperacion = @p_FechaOperacion AND Situacion = 'A'  
			WHERE CantidadOperacion = 0 AND PrecioOperacion = 0 AND p.Id = @Id2)
			DELETE TmpPrevOrdenInversion FROM TmpPrevOrdenInversion p  
			JOIN PrevOrdenInversion po ON p.CodigoNemonico = po.CodigoNemonico AND p.CodigoOperacion = po.CodigoOperacion AND p.CodigoTercero = po.CodigoTercero
			AND po.FechaOperacion = @p_FechaOperacion AND Situacion = 'A'  
			WHERE CantidadOperacion = 0 AND PrecioOperacion = 0 AND p.Id = @Id2  
			GOTO continua  
		END
		SELECT TOP 1 @CodigoPlaza = CodigoPlaza,@TipoCondicion = TipoCondicion,@TipoFondo = TipoFondo,@TipoTramo = TipoTramo,@CodigoContacto = CodigoContacto,
		@HoraOperacion = HoraOperacion,@MedioNegociacion = MedioNegociacion,@HoraEjecucion = HoraEjecucion,@Correlativo = Correlativo,@CodigoPrevOrden = CodigoPrevOrden,
		@Ficticia=isnull(Ficticia,'N'),@RegulaSBS=isnull(RegulaSBS,'N')
		FROM PrevOrdenInversion
		WHERE CodigoPrevOrden IN (SELECT CodigoPrevOrden FROM TmpPrevOrdenInversion_OI WHERE CodigoOrden = @Id2)      
		TRUNCATE TABLE #TmpFechaVencimiento
		TRUNCATE TABLE #TmpComisiones
		--OBTENER DATOS PARA EL REGISTRO DE LA ORDEN DE INVERSION         
		SELECT @CodigoISIN = v.[CodigoISIN],@CodigoSBS = v.[CodigoSBS],@CodigoTipoInstrumentoSBS = v.[CodigoTipoInstrumentoSBS],@CodigoTipoTitulo = v.[CodigoTipoTitulo],
		@CodigoMoneda = v.[CodigoMoneda],@CategoriaInstrumento = ci.[Categoria]
		FROM Valores v
		JOIN TipoInstrumento ti ON ti.[CodigoTipoInstrumentoSBS] = v.[CodigoTipoInstrumentoSBS]
		JOIN ClaseInstrumento ci ON ci.[CodigoClaseInstrumento] = ti.[CodigoClaseInstrumento]
		WHERE v.[CodigoNemonico] = @CodigoNemonico
		IF @CategoriaInstrumento NOT IN ('FM','FI')
			IF @TipoCondicion = 'PT'
				SET @TipoTramo = 'PRINCIPAL'
			ELSE
				SET @TipoTramo = 'AGENCIA'
		INSERT INTO #TmpFechaVencimiento        
		EXEC OrdenInversion_RetornarFechaVencimiento @p_FechaOperacion,@CodigoNemonico,@Fondo      
		SELECT @FechaVencimiento = CONVERT(NUMERIC,(RIGHT(FechaVencimiento,4) + SUBSTRING(FechaVencimiento,4,2) + LEFT(FechaVencimiento,2))) FROM #TmpFechaVencimiento
		SELECT @MontoOperacion = @Cantidad * @PrecioPromedio,@CantidadOrdenado = @Cantidad,@CantidadOperacion = @Cantidad,@Precio = @PrecioPromedio
		--Obtener el grupo intermediario por el codigo tercero        
		SELECT @EntidadEmisora = e.EntidadEmisora,@EntidadComisionista = e.EntidadComisionista,@EntidadCustodio = e.EntidadCustodio,@EntidadFinanciera = e.EntidadFinanciera,
		@EntidadAval = e.EntidadAval,@EntidadRiesgocero = e.EntidadRiesgocero,@EntidadVinculada = e.EntidadVinculada,@EntidadVigilada = e.EntidadVigilada,
		@EntidadReguladora = e.EntidadReguladora,@EntidadBroker = e.EntidadBroker
		FROM Terceros t        
		JOIN Entidad e ON t.CodigoTercero = e.CodigoTercero        
		WHERE t.CodigoTercero = @CodigoTercero              
		IF @EntidadEmisora = 'S'SET @GrupoIntermediario = 'EMI'        
		IF @EntidadComisionista = 'S' SET @GrupoIntermediario = 'COM'
		IF @EntidadCustodio = 'S' SET @GrupoIntermediario = 'CUS'
		IF @EntidadFinanciera = 'S' SET @GrupoIntermediario = 'FIN'
		IF @EntidadAval = 'S' SET @GrupoIntermediario = 'AVA'
		IF @EntidadRiesgocero = 'S' SET @GrupoIntermediario = 'RIC'
		IF @EntidadVinculada = 'S' SET @GrupoIntermediario = 'VIN'
		IF @EntidadVigilada = 'S' SET @GrupoIntermediario = 'VIG'
		IF @EntidadReguladora = 'S' SET @GrupoIntermediario = 'REG'
		IF @EntidadBroker = 'S' SET @GrupoIntermediario = 'BRO'
		--Establecer el estado de la orden de inversion        
		SET @Estado = 'E-ING'          
		IF @EstadoLimites = 'E-EXC' AND @EstadoBroker = 'E-EXC'        
		BEGIN        
			SET @Estado = 'E-EBL'        
		END        
		ELSE        
		BEGIN        
			IF @EstadoBroker = 'E-EXC'
			BEGIN
				SET @Estado = 'E-ENV'
			END
			IF @EstadoLimites = 'E-EXC'
			BEGIN
				SET @Estado = 'E-EXC'
			END
		END
		SET @TotalComisiones = 0
		SET @MontoNetoOperacion = @TotalComisiones + @MontoOperacion
		SET @PrecioPromedioOp = @MontoNetoOperacion / @CantidadOperacion
		/*--------------------------------------*/      
		--REGISTRO EN LA TABLA ORDEN DE INVERSION      
		INSERT INTO #TmpCodigoOI      
		EXEC OrdenInversion_InsertarOI @p_FechaOperacion,@FechaVencimiento,null,null,null,null,null,null,@MontoOperacion,null,null,null,null,null,null,null,null,
		null,null,null,null,null,null,@Precio,null,null,      null,null,null,null,null,null,null,null,null,null,null,@CodigoMoneda,null,null,@p_UsuarioCreacion,@p_HoraCreacion,
		@TotalComisiones,@PrecioPromedioOp,@MontoNetoOperacion,@Estado,'A',null,@p_Host,@CodigoISIN,@CodigoSBS,@CodigoTercero,@p_FechaCreacion,@Fondo,@CodigoNemonico,
		@CodigoContacto,@CodigoOperacion,@CantidadOrdenado,@CantidadOperacion,null,@p_UsuarioCreacion,@HoraOperacion,null,null,null,null,null,null,@CategoriaInstrumento,null,
		null,null,null,null,null,null,@TipoFondo,@p_FechaOperacion,null,null,null,null,null,@CodigoPlaza,null,@TipoTramo,@GrupoIntermediario,null,@Ficticia,null,null,@RegulaSBS,
		@MedioNegociacion,@HoraEjecucion
		SELECT @p_CodigoOrdenInversion = CodigoOrden FROM #TmpCodigoOI
		truncate table #TmpCodigoOI
		-------------------------------
		EXEC pr_SIT_pro_CalcularComisiones_OrdenInversion @p_CodigoOrdenInversion, @CodigoRenta  
		---------------------------------
		IF (SELECT COUNT(1) FROM OrdenInversion WHERE CodigoOrden = @p_codigoOrdenInversion) > 0      
		BEGIN      
			UPDATE PrevOrdenInversion SET Flag = '0',Estado = 'EJE' WHERE CodigoPrevOrden IN (SELECT CodigoPrevOrden FROM TmpPrevOrdenInversion_OI WHERE CodigoOrden = @Id2)
			UPDATE TmpPrevOrdenInversion_OI SET CodigoOrden = @p_codigoOrdenInversion WHERE CodigoOrden = @Id2
			UPDATE TmpPrevOrdenInversion SET CodigoOrden = @p_codigoOrdenInversion WHERE Id = @Id2      
			EXEC OrdenInversion_EjecutarOI @p_codigoOrdenInversion, @Fondo, ''  
			SET @Asignacion = @CantidadOperacion
			EXEC dbo.pr_SIT_ins_TrazabilidadOperaciones_Insertar_sura @p_FechaOperacion,@Correlativo,'E-EJE','2',@CodigoPrevOrden,@p_CodigoOrdenInversion,@CodigoNemonico,
			@CodigoOperacion,@CantidadOperacion,@Precio,@CodigoTercero,@CantidadOperacion,@Precio,@Fondo,@Asignacion,  '1','4','','Genera Orden Inversión del Masivo',
			@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host
			IF @antCodigoPrevOrden <> @CodigoPrevOrden  
			BEGIN  
				SET @antCodigoPrevOrden = @CodigoPrevOrden  
				EXEC dbo.pr_SIT_ins_TrazabilidadOperaciones_Insertar_sura @p_FechaOperacion,@Correlativo,'EJE','1',@CodigoPrevOrden,'',@CodigoNemonico,@CodigoOperacion,
				@CantidadOrdenado,@Precio,@CodigoTercero,@CantidadOperacion,@PrecioPromedioOp,@Fondo,@Asignacion,'1','4','','Ejecuta Operación Previa del Masivo',
				@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host
			END
		END
		ELSE
		BEGIN
			UPDATE PrevOrdenInversion SET Flag = '0' WHERE CodigoPrevOrden IN (SELECT CodigoPrevOrden FROM TmpPrevOrdenInversion_OI WHERE CodigoOrden = @Id2)
		END
		continua:
		FETCH NEXT FROM TmpPrevOrdenInversion_Cursor INTO @CodigoNemonico,@CodigoOperacion,@CodigoTercero,@Fondo,@Cantidad,@PrecioPromedio,@EstadoLimites,@EstadoBroker,@Id2
	END
	CLOSE TmpPrevOrdenInversion_Cursor  
	DEALLOCATE TmpPrevOrdenInversion_Cursor  
	INSERT INTO PrevOrdenInversion_OI      
	SELECT toi.CodigoPrevOrden, toi.CodigoPrevOrdenDet, toi.CodigoOrden FROM TmpPrevOrdenInversion_OI toi  
	JOIN PrevOrdenInversion po on toi.CodigoPrevOrden = po.CodigoPrevOrden  
	JOIN TmpPrevOrdenInversion t on t.CodigoNemonico = po.CodigoNemonico AND t.CodigoOperacion = po.CodigoOperacion AND t.CodigoTercero = po.CodigoTercero  
	AND po.FechaOperacion = @p_FechaOperacion AND po.Situacion = 'A'  
	WHERE toi.NProceso = @p_NProceso AND t.NProceso = @p_NProceso
	DROP TABLE #TmpFechaVencimiento    
	DROP TABLE #TmpComisiones    
	DROP TABLE #TmpCodigoOI
END
GO

GRANT EXECUTE ON [dbo].[pr_SIT_ins_InsertarOIRV_PrevOrdenInversion_Sura] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[TasaLibor_EliminarVectorCarga_PIP]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TasaLibor_EliminarVectorCarga_PIP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TasaLibor_EliminarVectorCarga_PIP]
GO
---------------------------------------------------------------------------------------------------------------------
-- Objetivo: ELIMINAR VECTOR CARGA TASA LIBOR 
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 10/05/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Se agrego un parametro para eliminar por el tipo de fuente (PIP o Bloomberg)
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[TasaLibor_EliminarVectorCarga_PIP] 
	(
		@p_Fecha NUMERIC(8, 0),
		@p_Fuente VARCHAR(1)=''
	)
AS
BEGIN
	DELETE FROM 
		ValorIndicador
	WHERE 
		Fecha=@p_Fecha 
		AND CodigoIndicador LIKE 'SW-%'
		and  [dbo].[fnObtenerFilaSplit] (CodigoIndicador,'-',3)=@p_Fuente
END


GRANT EXECUTE ON [dbo].[TasaLibor_EliminarVectorCarga_PIP] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Indicador_SeleccionarIndicadorLibor]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Indicador_SeleccionarIndicadorLibor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Indicador_SeleccionarIndicadorLibor]
GO


-----------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Obtiene los valores de indicadores de libor
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 12/03/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Nuevo Store Procedure
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 10/05/2019  
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Se agrego para que la consulta devuelve el mes libor y el codigo. Ademas agrego como parámetro de entrada la fuente (PIP, BLO)
-----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Indicador_SeleccionarIndicadorLibor]
(
	@p_CodigoIndicador	VARCHAR(10) = '',
	@p_Fuente VARCHAR(1)=''
)
AS
BEGIN
	SELECT
		IR.[CodigoIndicador], 
		CONVERT(INT, LEFT(NombreIndicador, PATINDEX('%[^0-9]%',NombreIndicador +' ')-1))*30 DiasLibor,
		LEFT(NombreIndicador, PATINDEX('%[^0-9]%',NombreIndicador +' ')) AS CodigoLibor,
		CONVERT(INT, LEFT(NombreIndicador, PATINDEX('%[^0-9]%',NombreIndicador +' ')-1)) AS MesLibor
	FROM
		Indicador AS IR
	WHERE
        IR.CodigoIndicador LIKE (CASE WHEN LEN(@p_CodigoIndicador) = 0 THEN IR.CodigoIndicador ELSE '%' + @p_CodigoIndicador + '%' END)
		AND IR.Situacion = 'A'
		and  [dbo].[fnObtenerFilaSplit] (CodigoIndicador,'-',3)=@p_Fuente
		
	Order By 
		IR.NombreIndicador
END
GO

GRANT EXECUTE ON [dbo].[Indicador_SeleccionarIndicadorLibor] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[dbo].[Indicador_Seleccionar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Indicador_Seleccionar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Indicador_Seleccionar]
GO


-----------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Obtiene los valores de indicadores de libor
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 10/05/2019  
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Se agrego para que la consulta devuelve el mes libor, fuente y el flag de mostrar o no el control de libor en el mantenimiento de indicadores
-----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Indicador_Seleccionar]
(
	@p_CodigoIndicador VARCHAR(10) = ''
)
AS
BEGIN
	SELECT
		IR.[NombreIndicador], 
		IR.[CodigoIndicador], 
		IR.[Situacion],
		PG.Nombre AS 'NombreSituacion', 
		IR.[UsuarioCreacion], 		
		IR.[FechaCreacion], 
		IR.[ManejaPeriodo],
		case when IR.ManejaPeriodo = 'S' then 'SI' when IR.ManejaPeriodo='N' then 'NO' end AS 'NombreManejaPeriodo',
		IR.[HoraCreacion], 
		IR.[UsuarioModificacion], 
		IR.[FechaModificacion], 
		IR.[HoraModificacion], 
		IR.[Host],
		IR.[TipoIndicador],
		isnull(IR.[MostrarLibor],'N') as MostrarLibor,
		IR.[MesLibor],		
		case when IR.MostrarLibor = 'S' then 'SI' when IR.MostrarLibor='N' then 'NO' end AS 'NombreMostrarLibor',
		FL.[Nombre] AS 'NombreFuenteLibor',
		IR.[FuenteLibor]
	FROM
		Indicador AS IR
		LEFT JOIN ParametrosGenerales AS PG ON IR.Situacion = PG.Valor
		LEFT JOIN ParametrosGenerales AS FL ON IR.FuenteLibor = FL.Valor AND FL.Clasificacion='FUENTE_LIBOR'
	WHERE
		IR.CodigoIndicador = @p_CodigoIndicador
		AND PG.Clasificacion = 'Situación'
END
GO

GRANT EXECUTE ON [dbo].[Indicador_Seleccionar] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[dbo].[Indicador_Insertar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Indicador_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Indicador_Insertar]
GO

-----------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Obtiene los valores de indicadores de libor
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 10/05/2019  
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Se agregaron como parametros de entrada p_MostrarLibor, p_FuenteLibor, p_MesLibor
-----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Indicador_Insertar]
(
	@p_CodigoIndicador	VARCHAR(10),
	@p_NombreIndicador	VARCHAR(50),
	@p_Situacion		VARCHAR(1),
	@p_BaseTasa		NUMERIC(6,0),
	@p_DiasPeriodo		NUMERIC(3,0),
	@p_FechaVigencia	NUMERIC(8,0),
	@p_ManejaPeriodo	VARCHAR(1),
	@p_CodigoPeriodicidad	VARCHAR(4),
	@p_CodigoTipoCupon	VARCHAR(4),
    @p_TipoIndicador	VARCHAR(1),
	@p_UsuarioCreacion	VARCHAR(15),
	@p_FechaCreacion	NUMERIC(8,0),
	@p_HoraCreacion		VARCHAR(10),
	@p_Host			VARCHAR(20),
	@p_MostrarLibor			VARCHAR(4),
	@p_FuenteLibor			VARCHAR(4),
	@p_MesLibor			int
)
AS
BEGIN
	INSERT INTO Indicador
	(
		[NombreIndicador], 
		[CodigoIndicador], 
		[Situacion], 
		[TipoIndicador],
		[ManejaPeriodo], 
		[UsuarioCreacion], 
		[FechaCreacion], 
		[HoraCreacion], 
		[Host],
		[MostrarLibor] ,
		[FuenteLibor],
		[MesLibor] 
	)
	VALUES
	(
		@p_NombreIndicador,
		@p_CodigoIndicador,
		@p_Situacion,
		@p_TipoIndicador,
		@p_ManejaPeriodo,
		@p_UsuarioCreacion,
		@p_FechaCreacion,
		@p_HoraCreacion,
		@p_Host,
		@p_MostrarLibor ,
		@p_FuenteLibor,
		@p_MesLibor
	)

END
GO

GRANT EXECUTE ON [dbo].[Indicador_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[ValorCuota_GraficoDatos]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota_GraficoDatos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorCuota_GraficoDatos]
GO

---------------------------------------------------------------------------------------------------------------------
--Objetivo: Generar los datos de los ejes X, Y  que se muestra en la grafic de valor cuota
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 26/04/2019
-- Creado por: Ernesto Galarza Contreras
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Creacion del store
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ValorCuota_GraficoDatos]

@p_CodigoPortafolio varchar(20)='',
@p_CodigoSerie varchar(20)='',
@p_FechaProceso varchar(10) ='',
@p_TipoPeriodo varchar(4)='1'

AS

BEGIN

--1-Semana, 2-Mes, 3-Trimestral, 4-Semestral, 5-Anual
DECLARE @TablaDatos TABLE
(
 Secuencia int identity,
  Fecha decimal(8,0),
  RangoX varchar(20),
  RangoY numeric(22,7),
  ValorCuotaCierre numeric(22,7),
  ValorCuotaVariacion numeric(22,7),
  Mes int,
  Anio int,
  FechaCadena varchar(20),
  Portafolio varchar(40)
)

DECLARE @TablaDatosAux TABLE
(
 Secuencia int,
  Fecha decimal(8,0),
  RangoX varchar(20),
  RangoY numeric(22,7),
  ValorCuotaCierre numeric(22,7),
  ValorCuotaVariacion numeric(22,7),
  Mes int,
  Anio int,
  FechaCadena varchar(20),
  Portafolio varchar(40)
)

-- SEMANAL
declare @vDias int=0

declare @vFechaInicialCadena varchar(10)
declare @vFechaInicial numeric(8,0)
declare @vFechaFinal numeric(8,0)



IF @p_TipoPeriodo = '1'
	set @vDias=7	

IF @p_TipoPeriodo = '2'
set @vDias=30

IF @p_TipoPeriodo = '3'
set @vDias=90

IF @p_TipoPeriodo = '4'
set @vDias=180

IF @p_TipoPeriodo = '5'
set @vDias=360

	set @vFechaInicialCadena =(select  CONVERT(varchar,DATEADD(DAY,(@vDias*-1), CONVERT(datetime, @p_FechaProceso,103)),103 ))

	set @vFechaInicial =( select [dbo].[fn_SIT_gl_ConvertirFechaaDecimal](@vFechaInicialCadena))
	set @vFechaFinal =( select [dbo].[fn_SIT_gl_ConvertirFechaaDecimal](@p_FechaProceso))

	INSERT INTO @TablaDatos
	(
	Fecha,
	ValorCuotaCierre,
	RangoX,
	RangoY,
	ValorCuotaVariacion,
	Mes,
	Anio,
	FechaCadena,
	Portafolio
	)
	SELECT 
	FechaProceso,
	ValCuotaValoresCierre,
	 [dbo].[fn_SIT_gl_ConvertirFechaaString] (FechaProceso),
	 0,
	 ValCuotaValoresCierre,
	 0,
	 0,
	 [dbo].[fn_SIT_gl_ConvertirFechaaString] (FechaProceso),
	 P.Descripcion 
	FROM
	ValorCuota V 
	INNER JOIN Portafolio P on V.CodigoPortafolioSBS=P.CodigoPortafolioSBS 
	where V.FechaProceso>=@vFechaInicial and V.FechaProceso<=@vFechaFinal
	and V.CodigoPortafolioSBS=@p_CodigoPortafolio
	and V.CodigoSerie = (CASE WHEN LEN(@p_CodigoSerie) = 0 THEN CodigoSerie ELSE @p_CodigoSerie END)
	order by FechaProceso DESC 

	insert into @TablaDatosAux select * from @TablaDatos

	update @TablaDatosAux set Secuencia = Secuencia-1


	update @TablaDatos
	set  ValorCuotaVariacion=t1.ValorCuotaCierre- t2.ValorCuotaCierre
	from @TablaDatos t1 inner join @TablaDatosAux t2 on t1.Secuencia =t2.Secuencia
	


	update @TablaDatos set 
	Mes=convert(int,substring(FechaCadena,4,2)),
	Anio=convert(int,substring(FechaCadena,7,4)),
	RangoY= (CASE WHEN @p_CodigoSerie<>'' THEN ValorCuotaVariacion 
	ELSE 
	(ValorCuotaVariacion/ (CASE WHEN ValorCuotaCierre =0 then 1 else ValorCuotaCierre end)*100 )  END),
	RangoX=  case when (@p_TipoPeriodo='1' or @p_TipoPeriodo='2' or @p_TipoPeriodo='3'  or @p_TipoPeriodo='4' or @p_TipoPeriodo='5')  then  substring(FechaCadena,1,5) else substring(FechaCadena,4,7) end 
	
	declare @vIndiceAuxiliar int = (select MAX(Secuencia) from @TablaDatos)
	delete from @TablaDatos where Secuencia=@vIndiceAuxiliar

	if (@p_TipoPeriodo='1' or @p_TipoPeriodo='2' or @p_TipoPeriodo='3' or @p_TipoPeriodo='4' or @p_TipoPeriodo='5') 
	BEGIN
		select * from @TablaDatos
		order by  fECHA ASC
	END
	ELSE
	BEGIN
	select * from (
		SELECT  
		SUM(ValorCuotaVariacion) AS ValorCuotaVariacion,
		SUM(ValorCuotaVariacion) AS RangoY,
		Mes,
		Anio,
		RangoX 
		FROM @TablaDatos
		GROUP BY MES,ANIO, RangoX) t  
		order by  t.Anio ASC, t.mes ASC
	END


END
GO

GRANT EXECUTE ON [dbo].[ValorCuota_GraficoDatos] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[ValorCuota_FormulaVariacion]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota_FormulaVariacion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorCuota_FormulaVariacion]
GO
---------------------------------------------------------------------------------------------------------------------
--Objetivo: Realizar los calculos para guardar estos valores en la tabla ValorCuota_Variacion
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 22/05/2019
-- Creado por: Ernesto Galarza Contreras
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Creacion del store procedure
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ValorCuota_FormulaVariacion]
@p_CodigoPortafolioSBS VARCHAR(12),
@p_FechaOperacion DECIMAL(8),
@p_CodigoSerie VARCHAR(10) = ''
AS 

BEGIN 


	DECLARE 	@CodigoMonedaPortafolio VARCHAR(10)  = '',
		@CXPTituloLiberadas DECIMAL(22,7) = 0


DECLARE @TC DECIMAL(22,7) = dbo.RetornarSecuenciaVectorTipoCambio(@p_CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion) 		
SELECT  
	@CodigoMonedaPortafolio = P.CodigoMoneda  
FROM 
	Portafolio P  
WHERE  
	P.CodigoPortafolioSBS = @p_CodigoPortafolioSBS   

DECLARE @Patrimonio DECIMAL(22,7) = ISNULL((
SELECT ValPatriPreCierre1 FROM ValorCuota
WHERE FechaProceso = @p_FechaOperacion
AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
AND CodigoSerie = @p_CodigoSerie),0)

DECLARE @VariacionInstrumentoCarteraPrecio DECIMAL(22,7) = 0,
		@VariacionInstrumentoCarteraTC DECIMAL(22,7) = 0,
		
		@VariacionDerivados DECIMAL(22,7) = 0,
		
		@VariacionCXCPrecio DECIMAL(22,7) = 0,
		@VariacionCXCTC DECIMAL(22,7) = 0,
		
		@VariacionCXPPrecio DECIMAL(22,7) = 0,
		@VariacionCXPTC DECIMAL(22,7) = 0,
		
		@VariacionCajaTC DECIMAL(22,7) = 0
		
DECLARE @FechaT_1 DECIMAL(8) = [dbo].[RetornarFechaModificadaEnDias](@p_FechaOperacion ,-1)
--- >> Instrumentos en cartera / Precio
SET @VariacionInstrumentoCarteraPrecio =
			 ISNULL(
				  (SELECT SUM(((CASE WHEN ISNULL(VP_T.PrecioSucio,0) <> 0 THEN VP_T.PrecioSucio ELSE ISNULL(VP_T.PrecioLimpio,0) END) 
								- (CASE WHEN ISNULL(VP_T_1.PrecioSucio,0) <> 0 THEN VP_T_1.PrecioSucio ELSE ISNULL(VP_T_1.PrecioLimpio,0) END))
							   * RVL.Valorizacion/@Patrimonio) 
					FROM 
						ReporteVL RVL
					LEFT JOIN 
						VectorPrecio VP_T ON VP_T.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,RVL.CodigoNemonico,@p_FechaOperacion)
											 AND VP_T.fecha=@p_FechaOperacion 
											 AND VP_T.CodigoMnemonico=RVL.CodigoNemonico  
					LEFT JOIN 
						VectorPrecio VP_T_1 ON VP_T_1.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,RVL.CodigoNemonico,@FechaT_1)
											   AND VP_T_1.fecha= @FechaT_1 
											   AND VP_T_1.CodigoMnemonico=RVL.CodigoNemonico  
					WHERE 
						RVL.Fecha = @p_FechaOperacion
						AND RVL.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
						AND RVL.ImprimeVL  = '1'
						AND RVL.FormaValorizacion = '1')
					,0)
SET @VariacionInstrumentoCarteraTC =
			 ISNULL(
				  (SELECT SUM((RVL_T.TipoCambio - RVL_T_1.TipoCambio)
							   * RVL_T.Valorizacion/@Patrimonio) 
					FROM 
						ReporteVL RVL_T
					LEFT JOIN 
						ReporteVL RVL_T_1 ON RVL_T.CodigoPortafolioSBS = RVL_T_1.CodigoPortafolioSBS
											 AND RVL_T.CodigoNemonico = RVL_T_1.CodigoNemonico
											 AND RVL_T_1.Fecha = @FechaT_1
											 AND RVL_T_1.ImprimeVL  = '1'
											 AND RVL_T_1.FormaValorizacion = '1'
					
					WHERE 
						RVL_T.Fecha = @p_FechaOperacion
						AND RVL_T.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
						AND RVL_T.ImprimeVL  = '1'
						AND RVL_T.FormaValorizacion = '1'
						AND RVL_T.TipoCambio > 1)
					,0)
	
SET @VariacionDerivados = 
			ISNULL(
				  (SELECT SUM((RVL_T.Valorizacion - RVL_T_1.Valorizacion)
							   * RVL_T.Valorizacion/@Patrimonio) 
					FROM 
						ReporteVL RVL_T
					LEFT JOIN 
						ReporteVL RVL_T_1 ON RVL_T.CodigoPortafolioSBS = RVL_T_1.CodigoPortafolioSBS
											 AND RVL_T.CodigoNemonico = RVL_T_1.CodigoNemonico
											 AND RVL_T_1.Fecha = @FechaT_1
											 AND RVL_T_1.ImprimeVL  = '1'
											 AND RVL_T_1.FormaValorizacion = '5'
					
					WHERE 
						RVL_T.Fecha = @p_FechaOperacion
						AND RVL_T.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
						AND RVL_T.ImprimeVL  = '1'
						AND RVL_T.FormaValorizacion = '5')
					,0)

SELECT  
	@CXPTituloLiberadas = 
			SUM(  
			((ROUND((isnull(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor(@p_CodigoPortafolioSBS,DRL.CodigoNemonico, drl.FechaCorte),0,1)  
			* (CASE WHEN ISNULL(VP.ValorPrecio,0) <> 0 THEN VP.PrecioSucio ELSE VP.PrecioLimpio END))  
			* dbo.RetornarSecuenciaVectorTipoCambio(DRL.CodigoPortafolioSBS,DRL.CodigoMoneda,@p_FechaOperacion)) 
			/ dbo.RetornarSecuenciaVectorTipoCambio(DRL.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)  
 )  
FROM 
	DividendosRebatesLiberadas DRL  
JOIN 
	Valores VA on VA.CodigoNemonico = DRL.CodigoNemonico  
JOIN 
	TipoInstrumento TI on TI.CodigoTipoInstrumentoSBS = VA.CodigoTipoInstrumentoSBS  
LEFT JOIN 
	VectorPrecio VP ON VP.CodigoMnemonico = DRL.CodigoNemonico 
					   AND VP.Fecha = @p_FechaOperacion  
					   AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico,@p_FechaOperacion)  
JOIN 
	Moneda M ON M.CodigoMoneda = DRL.CodigoMoneda  
JOIN 
	VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion 
						   AND TC.CodigoMoneda = CASE WHEN DRL.CodigoMoneda = 'CAD' THEN 'DOL' ELSE DRL.CodigoMoneda END  
						   AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(DRL.CodigoPortafolioSBS,TC.CodigoMoneda,@p_FechaOperacion)  
WHERE  
	DRL.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND DRL.FechaCorte <= @p_FechaOperacion 
	AND DRL.FechaEntrega > @p_FechaOperacion 
	AND DRL.TipoDistribucion = 'L'  
	AND DRL.Situacion = 'A'  
SET @VariacionCXCPrecio = 
		ISNULL((SELECT 
					SUM(((CASE WHEN ISNULL(VP_T.PrecioSucio,0) <> 0 THEN VP_T.PrecioSucio ELSE ISNULL(VP_T.PrecioLimpio,0) END) 
						  - (CASE WHEN ISNULL(VP_T_1.PrecioSucio,0) <> 0 THEN VP_T_1.PrecioSucio ELSE ISNULL(VP_T_1.PrecioLimpio,0) END)) 
					     * ((ISNULL(MontoNetoOperacion,MontoOperacion) * 
					      dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
			              /dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
			              + ISNULL(@CXPTituloLiberadas,0))/@Patrimonio)
				FROM 
					OrdenInversion OI  
				JOIN 
					Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
								   AND O.CodigoTipoOperacion IN ('2')  
				JOIN 
					VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion 
										   AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END   
										   AND FechaOperacion <= @p_FechaOperacion  
										   AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,@p_FechaOperacion)  
				JOIN 
					Moneda M ON M.CodigoMoneda = TC.CodigoMoneda  
				LEFT JOIN 
					VectorPrecio VP_T ON VP_T.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,OI.CodigoMnemonico,@p_FechaOperacion)
										 AND VP_T.fecha=@p_FechaOperacion 
										 AND VP_T.CodigoMnemonico=OI.CodigoMnemonico  
				LEFT JOIN 
					VectorPrecio VP_T_1 ON VP_T_1.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,OI.CodigoMnemonico,@FechaT_1)
										   AND VP_T_1.fecha= @FechaT_1 
										   AND VP_T_1.CodigoMnemonico=OI.CodigoMnemonico  
				WHERE 
					OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
					AND OI.FechaLiquidacion > @p_FechaOperacion    
					AND OI.Estado = 'E-CON' 
					AND OI.Situacion = 'A' 
					AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR') 
					AND OI.CodigoOperacion NOT IN ('36','37')
				),0)
SET @VariacionCXCTC= 
		ISNULL((SELECT 
					SUM((TC_T.ValorPrimario - TC_T_1.ValorPrimario)
					     * ((ISNULL(MontoNetoOperacion,MontoOperacion) * 
					      dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
			              /dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
			              + ISNULL(@CXPTituloLiberadas,0))/@Patrimonio)
				FROM 
					OrdenInversion OI  
				JOIN 
					Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
								   AND O.CodigoTipoOperacion IN ('2')  
				JOIN 
					VectorTipoCambio TC_T ON TC_T.Fecha = @p_FechaOperacion 
										     AND TC_T.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END   
										     AND FechaOperacion <= @p_FechaOperacion  
										     AND TC_T.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC_T.CodigoMoneda,@p_FechaOperacion) 
				JOIN 
					VectorTipoCambio TC_T_1 ON TC_T_1.Fecha = @FechaT_1 
										   AND TC_T_1.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END   
										   AND FechaOperacion <= @FechaT_1  
										   AND TC_T_1.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC_T_1.CodigoMoneda,@FechaT_1)  
				JOIN 
					Moneda M ON M.CodigoMoneda = TC_T.CodigoMoneda  
				JOIN
					Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
				WHERE 
					OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
					AND OI.FechaLiquidacion > @p_FechaOperacion    
					AND OI.Estado = 'E-CON' 
					AND OI.Situacion = 'A' 
					AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR') 
					AND OI.CodigoOperacion NOT IN ('36','37')
					AND P.CodigoMoneda <> M.CodigoMoneda
				),0)

SET @VariacionCXPPrecio = 
		ISNULL((SELECT 
					SUM(((CASE WHEN ISNULL(VP_T.PrecioSucio,0) <> 0 THEN VP_T.PrecioSucio ELSE ISNULL(VP_T.PrecioLimpio,0) END) 
						  - (CASE WHEN ISNULL(VP_T_1.PrecioSucio,0) <> 0 THEN VP_T_1.PrecioSucio ELSE ISNULL(VP_T_1.PrecioLimpio,0) END)) 
					     * ((ISNULL(MontoNetoOperacion,MontoOperacion) * 
					      dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
			              /dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
			              + ISNULL(@CXPTituloLiberadas,0))/@Patrimonio)
				FROM 
					OrdenInversion OI  
				JOIN 
					Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
								   AND O.CodigoTipoOperacion IN ('1')  
				JOIN 
					VectorTipoCambio TC ON TC.Fecha = @p_FechaOperacion 
										   AND TC.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END   
										   AND FechaOperacion <= @p_FechaOperacion  
										   AND TC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC.CodigoMoneda,@p_FechaOperacion)  
				JOIN 
					Moneda M ON M.CodigoMoneda = TC.CodigoMoneda  
				LEFT JOIN 
					VectorPrecio VP_T ON VP_T.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,OI.CodigoMnemonico,@p_FechaOperacion)
										 AND VP_T.fecha=@p_FechaOperacion 
										 AND VP_T.CodigoMnemonico=OI.CodigoMnemonico  
				LEFT JOIN 
					VectorPrecio VP_T_1 ON VP_T_1.EntidadExt=dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,OI.CodigoMnemonico,@FechaT_1)
										   AND VP_T_1.fecha= @FechaT_1 
										   AND VP_T_1.CodigoMnemonico=OI.CodigoMnemonico  
				WHERE 
					OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
					AND OI.FechaLiquidacion > @p_FechaOperacion    
					AND OI.Estado = 'E-CON' 
					AND OI.Situacion = 'A' 
					AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR') 
				--	AND OI.CodigoOperacion NOT IN ('36','37')
				),0)
SET @VariacionCXPTC= 
		ISNULL((SELECT 
					SUM((TC_T.ValorPrimario - TC_T_1.ValorPrimario)
					     * ((ISNULL(MontoNetoOperacion,MontoOperacion) * 
					      dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,OI.CodigoMoneda,@p_FechaOperacion))
			              /dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,@CodigoMonedaPortafolio,@p_FechaOperacion)
			              + ISNULL(@CXPTituloLiberadas,0))/@Patrimonio)
				FROM 
					OrdenInversion OI  
				JOIN 
					Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
								   AND O.CodigoTipoOperacion IN ('1')  
				JOIN 
					VectorTipoCambio TC_T ON TC_T.Fecha = @p_FechaOperacion 
										     AND TC_T.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END   
										     AND FechaOperacion <= @p_FechaOperacion  
										     AND TC_T.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC_T.CodigoMoneda,@p_FechaOperacion) 
				JOIN 
					VectorTipoCambio TC_T_1 ON TC_T_1.Fecha = @FechaT_1 
										   AND TC_T_1.CodigoMoneda = CASE WHEN OI.CodigoMoneda = 'CAD' THEN 'DOL' ELSE OI.CodigoMoneda END   
										   AND FechaOperacion <= @FechaT_1  
										   AND TC_T_1.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(OI.CodigoPortafolioSBS,TC_T_1.CodigoMoneda,@FechaT_1)  
				JOIN 
					Moneda M ON M.CodigoMoneda = TC_T.CodigoMoneda  
				JOIN
					Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
				WHERE 
					OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
					AND OI.FechaLiquidacion > @p_FechaOperacion    
					AND OI.Estado = 'E-CON' 
					AND OI.Situacion = 'A' 
					AND OI.CategoriaInstrumento NOT IN ('DP','FD','OR') 
					--AND OI.CodigoOperacion NOT IN ('36','37')
					AND P.CodigoMoneda <> M.CodigoMoneda
				),0)

SET @VariacionCXPPrecio =-1* @VariacionCXPPrecio 					
SET @VariacionCXPTC =-1* @VariacionCXPTC

DECLARE @NuevoMonto NUMERIC(22,7), 
		@FechaRecaudo NUMERIC(8), 
		@FechaInversiones NUMERIC(8),
		@RecaudoMonto NUMERIC(22,7),
		@InversionesMonto NUMERIC(22,7)    

SET @RecaudoMonto = 0 
SET @InversionesMonto = 0    

 --Inversiones    
SET @FechaInversiones = (    
		SELECT 
			MAX(SB.FechaOperacion)    
		FROM 
			SaldosBancarios SB    
		JOIN 
			CuentaEconomica CE ON SB.CodigoPortafolioSBS = CE.CodigoPortafolioSBS    
								  AND SB.NumeroCuenta = CE.NumeroCuenta 
								  AND CE.CodigoClaseCuenta = '20' 
								  AND CE.Situacion = 'A'    
		WHERE 
			SB.CodigoPortafolioSBS = @p_CodigoPortafolioSBS   
			AND SB.FechaOperacion <= @p_FechaOperacion    
		)    
  
SELECT 
	@InversionesMonto = SUM(DBO.sp_SIT_OBT_CambioMoneda(@p_CodigoPortafolioSBS,
														SB.SaldoDisponibleFinalConfirmado,
														SB.FechaOperacion,
														CE.CodigoMoneda,
														P.CodigoMoneda))    
FROM 
	SaldosBancarios SB    
JOIN 
	CuentaEconomica CE ON CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS 
						  AND SB.NumeroCuenta = CE.NumeroCuenta  
						  AND SB.CodigoMoneda = CE.CodigoMoneda    
						  AND CE.CodigoClaseCuenta IN ('20') 
						  AND CE.Situacion = 'A'
JOIN 
	Portafolio P ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS    
JOIN 
	VectorTipoCambio VTC ON VTC.CodigoMoneda = CE.CodigoMoneda 
							AND VTC.Fecha = SB.FechaOperacion    
							AND VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,CE.CodigoMoneda,SB.FechaOperacion)   
JOIN 
	VectorTipoCambio VTCP ON VTCP.CodigoMoneda = P.CodigoMoneda 
							 AND VTCP.Fecha = SB.FechaOperacion    
							 AND VTCP.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,P.CodigoMoneda,SB.FechaOperacion)   
WHERE 
	SB.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND SB.FechaOperacion = @FechaInversiones   
	AND SB.CodigoMoneda <> P.CodigoMoneda 
	
--Recaudo    
SET @FechaRecaudo = (    
		SELECT 
			MAX(SB.FechaOperacion)    
		FROM 
			SaldosBancarios SB    
		JOIN 
			CuentaEconomica CE ON SB.CodigoPortafolioSBS = CE.CodigoPortafolioSBS    
								  AND SB.NumeroCuenta = CE.NumeroCuenta 
								  AND CE.CodigoClaseCuenta = '10' 
								  AND CE.Situacion = 'A'    
		WHERE 
			SB.CodigoPortafolioSBS = @p_CodigoPortafolioSBS    
			AND SB.FechaOperacion <= @p_FechaOperacion    
		)    
  
SELECT 
	@RecaudoMonto = SUM(DBO.sp_SIT_OBT_CambioMoneda(@p_CodigoPortafolioSBS,
													SB.SaldoDisponibleFinalConfirmado,
													SB.FechaOperacion,
													CE.CodigoMoneda,
													P.CodigoMoneda))    
FROM 
	SaldosBancarios SB    
JOIN 
	CuentaEconomica CE ON CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS 
						  AND SB.NumeroCuenta = CE.NumeroCuenta  
						  AND SB.CodigoMoneda = CE.CodigoMoneda    
						  AND CE.CodigoClaseCuenta IN ('10') 
						  AND CE.Situacion = 'A'
JOIN 
	Portafolio P ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS    
JOIN 
	VectorTipoCambio VTC ON VTC.CodigoMoneda = CE.CodigoMoneda 
							AND VTC.Fecha = SB.FechaOperacion    
							AND  VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,CE.CodigoMoneda,sb.FechaOperacion)   
JOIN 
	VectorTipoCambio VTCP ON VTCP.CodigoMoneda = P.CodigoMoneda 
							 AND VTCP.Fecha = SB.FechaOperacion    
							 AND VTCP.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolioSBS,P.CodigoMoneda,sb.FechaOperacion)   
WHERE 
	SB.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND SB.FechaOperacion = @FechaRecaudo    
	AND SB.CodigoMoneda <> P.CodigoMoneda 

SET @NuevoMonto =  ISNULL(@RecaudoMonto,0) + ISNULL(@InversionesMonto,0)  

SET @VariacionCajaTC =
	ISNULL((SELECT 
				SUM((TC_T.ValorPrimario - TC_T_1.ValorPrimario)
					     *@NuevoMonto/@Patrimonio)
				FROM 
					Portafolio P
				JOIN 
					VectorTipoCambio TC_T ON TC_T.Fecha = @p_FechaOperacion 
										     AND TC_T.CodigoMoneda = CASE WHEN P.CodigoMoneda = 'CAD' THEN 'DOL' ELSE P.CodigoMoneda END   
										     AND TC_T.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(P.CodigoPortafolioSBS,TC_T.CodigoMoneda,@p_FechaOperacion) 
				JOIN 
					VectorTipoCambio TC_T_1 ON TC_T_1.Fecha = @FechaT_1 
										   AND TC_T_1.CodigoMoneda = CASE WHEN P.CodigoMoneda = 'CAD' THEN 'DOL' ELSE P.CodigoMoneda END   
										   AND TC_T_1.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(P.CodigoPortafolioSBS,TC_T_1.CodigoMoneda,@FechaT_1) 
				),0)

DECLARE @Comision DECIMAL (22,7) = 0


IF @p_CodigoSerie = ''
	BEGIN
		SET @Comision = ISNULL((SELECT ValorPorcentaje 
								FROM PortafolioPorcentajeComision
								WHERE CodigoPortafolio = @p_CodigoPortafolioSBS),0)					
	END
ELSE
	BEGIN
		SET @Comision = ISNULL((SELECT Porcentaje 
								FROM PortafolioSerie
								WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS
								AND CodigoSerie=@p_CodigoSerie),0)
	END
	
SELECT 
	@p_CodigoPortafolioSBS 'CodigoPortafolioSBS',
	@p_CodigoSerie 'CodigoSerie',
	@p_FechaOperacion 'FechaProceso',
	@VariacionInstrumentoCarteraPrecio 'CarteraPrecio',
	@VariacionInstrumentoCarteraTC 'CarteraTipoCambio',
	@VariacionDerivados 'Derivados',
	@VariacionCXCPrecio 'CuentasPorCobrarPrecio',
	@VariacionCXCTC 'CuentasPorCobrarTipoCambio',
	@VariacionCXPPrecio 'CuentasPorPagarPrecio',
	@VariacionCXPTC 'CuentasPorPagarTipoCambio',
	@VariacionCajaTC 'CajaTipoCambio',
	@VariacionInstrumentoCarteraPrecio +
	@VariacionInstrumentoCarteraTC +
	@VariacionDerivados +
	@VariacionCXCPrecio +
	@VariacionCXCTC +
	@VariacionCXPPrecio + 
	@VariacionCXPTC +
	@VariacionCajaTC  'Total',
	@Comision 'Comision'	

END
GO


GRANT EXECUTE ON [dbo].[ValorCuota_FormulaVariacion] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[dbo].[ValorCuota_InsertarVariacion]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota_InsertarVariacion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorCuota_InsertarVariacion]
GO

---------------------------------------------------------------------------------------------------------------------
--Objetivo: Insertar datos en la tabla ValorCuota_InsertarVariacion
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 26/04/2019
-- Creado por: Ernesto Galarza Contreras
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Creacion del store procedure
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ValorCuota_InsertarVariacion]
@p_CodigoPortafolioSBS varchar(20)='',
@p_CodigoSerie varchar(20)='',
@p_FechaProceso numeric(8,0),
@p_CarteraPrecio numeric(22,7),
@p_CarteraTipoCambio numeric(22,7),
@p_Derivados numeric(22,7),
@p_CuentasPorCobrarPrecio numeric(22,7),
@p_CuentasPorCobrarTipoCambio numeric(22,7),
@p_CuentasPorPagarPrecio numeric(22,7),
@p_CuentasPorPagarTipoCambio numeric(22,7),
@p_CajaTipoCambio numeric(22,7),
@p_Comision numeric(22,7),
@p_PorcentageVariacionEstimado numeric(22,7),
@p_TotalRentabilidadInversiones numeric(22,7),
@p_PorcentageVariacionSIT numeric(22,7),
@p_DiferenciaEstimadoSIT numeric(22,7),
@p_EstadoSemaforo varchar(1),
@p_Hora varchar(10),
@p_Fecha numeric(22,7),
@p_Usuario varchar(15)
AS
BEGIN
IF EXISTS(SELECT 1 FROM ValorCuota_Variacion WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND FechaProceso = @p_FechaProceso AND CodigoSerie = @p_CodigoSerie )  
BEGIN

	 UPDATE ValorCuota_Variacion
	 SET 
		CarteraPrecio=@p_CarteraPrecio,
		CarteraTipoCambio=@p_CarteraTipoCambio,
		Derivados=@p_Derivados,
		CuentasPorCobrarPrecio=@p_CuentasPorCobrarPrecio,
		CuentasPorCobrarTipoCambio=@p_CuentasPorCobrarTipoCambio,
		CuentasPorPagarPrecio=@p_CuentasPorPagarPrecio,
		CuentasPorPagarTipoCambio=@p_CuentasPorPagarTipoCambio,
		CajaTipoCambio=@p_CajaTipoCambio,
		Comision=@p_Comision,
		PorcentageVariacionEstimado=@p_PorcentageVariacionEstimado,
		TotalRentabilidadInversiones=@p_TotalRentabilidadInversiones,
		PorcentageVariacionSIT=@p_PorcentageVariacionSIT,
		DiferenciaEstimadoSIT= @p_DiferenciaEstimadoSIT,
		EstadoSemaforo=@p_EstadoSemaforo,
		HoraModificacion=@p_Hora,
		FechaModificacion=@p_Fecha,
		UsuarioModificacion=@p_Usuario
	 WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	 AND FechaProceso = @p_FechaProceso 
	 AND CodigoSerie = @p_CodigoSerie
END

ELSE
	BEGIN

		INSERT INTO ValorCuota_Variacion
		 (
			CodigoPortafolioSBS,
			CodigoSerie,
			FechaProceso,
			CarteraPrecio,
			CarteraTipoCambio,
			Derivados,
			CuentasPorCobrarPrecio,
			CuentasPorCobrarTipoCambio,
			CuentasPorPagarPrecio,
			CuentasPorPagarTipoCambio,
			CajaTipoCambio,
			Comision,
			PorcentageVariacionEstimado,
			TotalRentabilidadInversiones,
			PorcentageVariacionSIT,
			DiferenciaEstimadoSIT,
			EstadoSemaforo,
			HoraCreacion,
			FechaCreacion,
			UsuarioCreacion
		 ) VALUES(
			@p_CodigoPortafolioSBS,
			@p_CodigoSerie,
			@p_FechaProceso,
			@p_CarteraPrecio,
			@p_CarteraTipoCambio,
			@p_Derivados,
			@p_CuentasPorCobrarPrecio,
			@p_CuentasPorCobrarTipoCambio,
			@p_CuentasPorPagarPrecio,
			@p_CuentasPorPagarTipoCambio,
			@p_CajaTipoCambio,
			@p_Comision,
			@p_PorcentageVariacionEstimado,
			@p_TotalRentabilidadInversiones,
			@p_PorcentageVariacionSIT,
			@p_DiferenciaEstimadoSIT,
			@p_EstadoSemaforo,
			@p_Hora,
			@p_Fecha,
			@p_Usuario
		 )
	END
END
GO

GRANT EXECUTE ON [dbo].[ValorCuota_InsertarVariacion] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[ValorCuota_ObtenerVariacion]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota_ObtenerVariacion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorCuota_ObtenerVariacion]
GO

---------------------------------------------------------------------------------------------------------------------
--Objetivo: Obtiene registro de la tabla ValorCuota_InsertarVariacion
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 26/04/2019
-- Creado por: Ernesto Galarza Contreras
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Creacion del store procedure
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ValorCuota_ObtenerVariacion]
@p_CodigoPortafolioSBS varchar(20)='',
@p_CodigoSerie varchar(20)='',
@p_FechaProceso numeric(8,0),
@p_ValidarExistencia int=0
AS
BEGIN

	select 
	* 
	from 
	ValorCuota_Variacion
	where 
	CodigoPortafolioSBS = @p_CodigoPortafolioSBS
	and CodigoSerie = (CASE WHEN LEN(@p_CodigoSerie) = 0 THEN CodigoSerie ELSE @p_CodigoSerie END)
	and FechaProceso=@p_FechaProceso

END
GO

GRANT EXECUTE ON [dbo].[ValorCuota_ObtenerVariacion] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[ValorCuota_ValidarExistenciaVariacion]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota_ValidarExistenciaVariacion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValorCuota_ValidarExistenciaVariacion]
GO

---------------------------------------------------------------------------------------------------------------------
--Objetivo: Obtiene registro de la tabla ValorCuota_InsertarVariacion
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creación: 26/04/2019
-- Creado por: Ernesto Galarza Contreras
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Creacion del store procedure
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ValorCuota_ValidarExistenciaVariacion]
@p_CodigoPortafolioSBS varchar(20)='',
@p_CodigoSerie varchar(20)='',
@p_FechaProceso numeric(8,0)
AS
BEGIN

	IF EXISTS(SELECT 1 FROM ValorCuota_Variacion WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND FechaProceso <@p_FechaProceso AND CodigoSerie = @p_CodigoSerie )  
	SELECT '1' AS EXISTE
	else
	SELECT '0' AS EXISTE
END
GO

GRANT EXECUTE ON [dbo].[ValorCuota_ValidarExistenciaVariacion] TO [rol_sit_fondos] AS [dbo]
GO



PRINT 'FIN --- > Store Procedures'
PRINT ' '


PRINT 'INICIO --- > RetornarExisteCuentaEconomicaTercero'

USE [SIT-FONDOS]
GO

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'RetornarExisteCuentaEconomicaTercero')
	DROP FUNCTION [dbo].[RetornarExisteCuentaEconomicaTercero]
GO
----------------------------------------------------------------------------------------------------------------------
--Objetivo: RETORNAR 'S' SI EXISTE UNA CUENTA PARA UN TERCERO
----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 10/06/2019
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12025
--	Descripcion del cambio: Nuevo
----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[RetornarExisteCuentaEconomicaTercero]
(
	@p_CodigoPortafolio VARCHAR(20),
	@p_CodigoTercero VARCHAR(15),
	@p_CodigoMoneda VARCHAR(5)
)
RETURNS VARCHAR(1)
AS
BEGIN
	DECLARE @thereIs VARCHAR(1)
	DECLARE @codigoEntidad VARCHAR(10)

	SET @codigoEntidad = (SELECT CodigoEntidad FROM Entidad WHERE CodigoTercero = @p_CodigoTercero)
	SELECT @thereIs = 'S' FROM CuentaEconomica
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND @p_CodigoMoneda = @p_CodigoMoneda AND EntidadFinanciera = @codigoEntidad

	RETURN ISNULL(@thereIs,'N')
END

GO

GRANT EXECUTE ON [dbo].[RetornarExisteCuentaEconomicaTercero] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'FIN --- > RetornarExisteCuentaEconomicaTercero'



PRINT 'INICIO --- > sp_SIT_Gen_Carta_OperacionForwardVcto'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_SIT_Gen_Carta_OperacionForwardVcto')
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
-- Fecha Modificación: 11/06/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 12025
-- Descripcion del cambio: Agregar función que permita saber si un fondo de sura tiene alguna cuenta en un banco especificado
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
	--BancoMatrizOrigen = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizOrigen) ,'') ,
	BancoMatrizOrigen =  dbo.RetornarExisteCuentaEconomicaTercero(OC.CodigoPortafolioSBS,OC.CodigoTerceroDestino,OI.CodigoMoneda),
	CEO.NumeroCuentaInterBancario,
	T.Descripcion DescripcionIntermediario , --tercero origen
	--BancoMatrizDestino = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizDestino) ,'') ,
	BancoMatrizDestino = dbo.RetornarExisteCuentaEconomicaTercero(OC.CodigoPortafolioSBS,OC.BancoGlosaDestino,OI.codigoMonedaDestino),
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

PRINT 'FIN --- > sp_SIT_Gen_Carta_OperacionForwardVcto'



IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 