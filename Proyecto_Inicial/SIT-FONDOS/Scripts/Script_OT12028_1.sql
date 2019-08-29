USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log_Datos
PRINT 'Agregar Campo DevolucionComisionDiaria a la tabla dbo.ValorCuota'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValorCuota]') and upper(name) = upper('DevolucionComisionDiaria'))
ALTER TABLE ValorCuota add DevolucionComisionDiaria numeric(22,7);
 
PRINT 'Agregar Campo PagoFechaComision a la tabla dbo.OperacionesCaja'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OperacionesCaja]') and upper(name) = upper('PagoFechaComision'))
ALTER TABLE OperacionesCaja add PagoFechaComision int;


PRINT 'Alterar campo Cantidad en TmpPrevOrdenInversion'
ALTER TABLE TmpPrevOrdenInversion ALTER COLUMN Cantidad NUMERIC(22,7);

PRINT ' '
PRINT 'INICIO --- > Creacion Tabla PagoFechaComision'
IF NOT EXISTS (SELECT 1 FROM sys.tables  WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision]')  and type_desc='USER_TABLE')
BEGIN
CREATE TABLE PagoFechaComision(
Id INT IDENTITY(1,1),
CodigoPortafolioSBS varchar(10),
CodigoBanco varchar(12),
NumeroCuenta varchar(25),
CodigoBancoAdministradora varchar(12),
NumeroCuentaAdministradora varchar(25),
FechaInicio numeric(8,0),
FechaFin numeric(8,0),
FechaSolicitud numeric(8,0),
FechaConfirmacion numeric(8,0),
ComisionAcumulada numeric(22,7),
SaldoDisponible numeric(22,7),
CodigoOperacionCaja varchar(7),
UsuarioSolicitud varchar(15),
HoraCreacion varchar(10),
FechaCreacion numeric(8,0),
UsuarioCreacion varchar(15),
HoraModIFicacion varchar(10),
FechaModIFicacion numeric(8,0),
UsuarioModIFicacion varchar(15),
Estado varchar(3)
)
ALTER table PagoFechaComision
add Constraint pkPagoFechaComision
primary key (Id);

END

IF NOT EXISTS (SELECT 1 FROM sys.tables  WHERE object_id = OBJECT_ID(N'[dbo].[PortafolioCaja]')  and type_desc='USER_TABLE')
BEGIN
CREATE TABLE PortafolioCaja(
Id int identity(1,1),
CodigoPortafolioSBS varchar(10),
CodigoClaseCuenta varchar(3),
FechaCajaOperaciones numeric(8,0),
HoraCreacion varchar(10),
FechaCreacion numeric(8,0),
UsuarioCreacion varchar(15),
HoraModIFicacion varchar(10),
FechaModIFicacion numeric(8,0),
UsuarioModIFicacion varchar(15)
)

ALTER table PortafolioCaja
add Constraint pkPortafolioCaja
primary key (Id);
END


update ClaseCuenta
set Situacion='I' where CodigoClaseCuenta not in ('10','20')

DELETE FROM ParametrosGenerales WHERE ClasIFicacion='FECHACOBRO_EMAIL_INGRESAR'

INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_EMAIL_INGRESAR',
'custodiadeinversionesfondos@sura.pe',
'PARA',
''
)

INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_EMAIL_INGRESAR',
'mesadeinversionesfondos@sura.pe;contabilidadfondos@sura.pe',
'COPIA',
''
)


DELETE FROM ParametrosGenerales WHERE ClasIFicacion='FECHACOBRO_EMAIL_ELIMINAR'
INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_EMAIL_ELIMINAR',
'mesadeinversionesfondos@sura.pe;custodiadeinversionesfondos@sura.pe',
'PARA',
''
)

INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_EMAIL_ELIMINAR',
'contabilidadfondos@sura.pe',
'COPIA',
''
)


DELETE FROM ParametrosGenerales WHERE ClasIFicacion='FECHACOBRO_EMAIL_CONFIRMAR'
INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_EMAIL_CONFIRMAR',
'contabilidadfondos@sura.pe',
'PARA',
''
)

INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_EMAIL_CONFIRMAR',
'custodiadeinversionesfondos@sura.pe;mesadeinversionesfondos@sura.pe',
'COPIA',
''
)

DELETE FROM ParametrosGenerales WHERE ClasIFicacion='FECHACOBRO_ESTADO'


INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_ESTADO',
'Disponible',
'DIS',
''
)

INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_ESTADO',
'Ingresado',
'ING',
''
)


INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_ESTADO',
'Confirmado',
'CON',
''
)

INSERT INTO ParametrosGenerales
(ClasIFicacion,
Nombre,
Valor,
Comentario
)VALUES
(
'FECHACOBRO_ESTADO',
'Pendiente',
'PEN',
''
)


DELETE FROM ArchivoPlano WHERE ArchivoCodigo='028'
INSERT INTO ArchivoPlano (ArchivoCodigo, ArchivoNombre,ArchivoDescripcion,ArchivoExtension,ArchivoUbicacion,UsuarioCreacion,FechaCreacion,Horacreacion) 
VALUES('028', 'Plantilla correo','Plantilla correo html','html','\\SPPEAPP00023\SIT_Fondos\Datos\Plantillas\Correo\','SYSTEM',20190606,'10:00:00')

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log_Datos
ELSE
    COMMIT TRANSACTION __Transaction_Log_Datos
GO 


USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log

PRINT ' '
PRINT 'INICIO --- > Store Procedures'

PRINT '[dbo].[sp_SIT_ins_ValorCuota]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ins_ValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ins_ValorCuota]
GO
------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Insertar los datos en valor cuota  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 15/12/2016  
-- ModIFicado por: Everth Martinez  
-- Nro. Orden de Trabajo: 9679  
-- Descripcion del cambio: Se agregan un execute para ejecutar el procedimiento donde calculara la variacion porcentual del registro  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modIFicacion: 18/01/2017  
-- ModIFicado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9851  
-- Descripcion del cambio: Se agregan las columnas de auditoria  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modIFicacion: 15/02/2017  
-- ModIFicado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9981  
-- Descripcion del cambio: Se modIFica el campo ComisionSAFMAnterior  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modIFicacion: 06/11/2017  
-- ModIFicado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 10916  
-- Descripcion del cambio: Agregar el parámetro "@p_CXCVentaTituloDividendos" y el campo "CXCVentaTituloDividendos"  
------------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 08/11/2017  
-- ModIFicado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 10883  
-- Descripcion del cambio: Agregar los campos AportesLiberadas y RetencionPendiente  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 16/07/2018  
-- ModIFicado por: Ricardo Colonia  
-- Proyecto: Fondos-II  
-- Nro. Orden de Trabajo: 11473  
-- Descripcion del cambio: Agregar Campo AjustesCXC   
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 28/12/2018  
-- ModIFicado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11717  
-- Descripcion del cambio: Agregar el campo DevolucionComisionUnIFicada  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 07/01/2018  
-- ModIFicado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11687  
-- Descripcion del cambio: Agregar el campo OtrasCxCPreCierre, ValSwaps  
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 24/05/2019
-- ModIFicado por: Junior Huallullo P.
-- Nro. Orden de Trabajo: 12003  
-- Descripcion del cambio: Agregar el campo AporteMandato, Retiro Mandato 
-----------------------------------------------------------------------------------------------------------------------------  
-- Fecha ModIFicación: 05/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripcion del cambio: Agregar el campo DevolucionComisionDiaria
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
 ,@p_DevolucionComisionUnIFicada NUMERIC(22,7) = 0  
 ,@p_OtrasCxCPrecierre NUMERIC(22,7)  
 ,@p_ValSwaps NUMERIC(22,7)  
 ,@p_ComisionUnIFicadaCuota NUMERIC(22,7)
 ,@p_ComisionUnIFicadaMandato NUMERIC(22,7)
 ,@p_DevolucionComisionDiaria NUMERIC(22,7) =null
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
    UsuarioModIFicacion = @p_Usuario,  
    FechaModIFicacion = @p_Fecha,  
    HoraModIFicacion = @p_Hora,  
    ChequePendiente = @p_ChequePendiente,  
    RescatePendiente = @p_RescatePendiente,   
    ComisionSAFMAnterior = @p_ComisionSAFMAnterior ,   
    AjustesCXP = @p_AjustesCXP, --9851 -- 9981  
    CXCVentaTituloDividendos = @p_CXCVentaTituloDividendos,   
    AportesLiberadas = @p_AportesLiberadas,   
    RetencionPendiente = @p_RetencionPendiente,  
    AjustesCXC = @p_AjustesCXC,  -- RCE | Nuevo Campo AjustesCXC | 16/07/2018  
    DevolucionComisionUnIFicada = ISNULL(@p_DevolucionComisionUnIFicada,0),  
    OtrasCxCPreCierre = @p_OtrasCxCPrecierre,  
    ValSwaps = @p_ValSwaps,
	ComisionUnIFicadaCuota = @p_ComisionUnIFicadaCuota,
	ComisionUnIFicadaMandato = @p_ComisionUnIFicadaMandato,
	DevolucionComisionDiaria = @p_DevolucionComisionDiaria
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
       UsuarioModIFicacion,  
       FechaModIFicacion,  
       HoraModIFicacion,  
       ChequePendiente,   
       RescatePendiente,   
       ComisionSAFMAnterior,  
       AjustesCXP,   
       CXCVentaTituloDividendos,  
       AportesLiberadas,  
       RetencionPendiente,  
       AjustesCXC, -- RCE | Nuevo Campo AjustesCXC | 16/07/2018  
       DevolucionComisionUnIFicada,  
       OtrasCxCPreCierre,  
       ValSwaps,
	   ComisionUnIFicadaCuota,
	   ComisionUnIFicadaMandato,
	   DevolucionComisionDiaria)   
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
    ISNULL(@p_DevolucionComisionUnIFicada,0),  
    @p_OtrasCxCPrecierre,  
    @p_ValSwaps,
	@p_ComisionUnIFicadaCuota,
	@p_ComisionUnIFicadaMandato,
	@p_DevolucionComisionDiaria)  
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


PRINT '[dbo].[FechaSaldoPortafolioPorCuenta]'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'FechaSaldoPortafolioPorCuenta')
	DROP FUNCTION [dbo].[FechaSaldoPortafolioPorCuenta]
GO
----------------------------------------------------------------------------------------------------------------------
--Objetivo: RETORNAR LA FECHA MAXIMA EN QUE SE ENCUENTRA EL SALDO EN LAS CUENTAS POR FONDOS
----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 10/06/2019
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Creacion de la funcion
----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[FechaSaldoPortafolioPorCuenta]
(
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_CodigoClase VARCHAR(10)
)
RETURNS numeric(8,0)
AS
BEGIN
	DECLARE @FechaMaxima numeric(8,0) = (
	SELECT 

	MAX(SB.FechaOperacion)
	FROM  
	SaldosBancarios SB 
	WHERE SB. CodigoPortafolioSBS=@p_CodigoPortafolioSBS and NumeroCuenta in (select NumeroCuenta  from CuentaEconomica where CodigoClaseCuenta=@p_CodigoClase and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and numerocuenta<>'' )
	)

	RETURN ISNULL(@FechaMaxima,0)
END
GO

GRANT EXECUTE ON [dbo].[FechaSaldoPortafolioPorCuenta] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[Portafolio_listarCodigoPortafolio]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Portafolio_listarCodigoPortafolio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Portafolio_listarCodigoPortafolio]
GO
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: LISTAR PORTAFOLIOS
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 04/08/2018
--	ModIFicado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Agregar parámetro TipoNegocio para filtrar los portafolios según sea: Mandatos o
--                          Fondos.
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 05/06/2019
--	ModIFicado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Agregar el campo CodigoPortafolioSisOpe Y FechaCajaOperaciones al query de salida
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Portafolio_listarCodigoPortafolio] 
 @p_CodigoPortafolio VARCHAR(20),  
 @p_Administra VARCHAR(1), --si maneja administradora
 @p_PorSerie VARCHAR(1),
 @p_Estado VARCHAR(1),
 @p_TipoNegocio VARCHAR(10) = '',
 @p_FondoVinculado VARCHAR(1) = ''
AS  
BEGIN
	DECLARE @CodMultIFondo VARCHAR(12)
	IF @p_CodigoPortafolio = '0'
		BEGIN
			SET @p_CodigoPortafolio = ''
		END
	SELECT @CodMultIFondo = Valor  FROM ParametrosGenerales WHERE ClasIFicacion = 'Grupofondo'
	IF @CodMultIFondo = @p_CodigoPortafolio
		SET @p_CodigoPortafolio = ''
	SELECT CodigoPortafolio = CodigoPortafolioSBS,Descripcion,ISNULL(FondoCliente,'') AS FondoCliente
		,(CASE WHEN ISNULL(CodigoTerceroCliente,'')<>'' THEN
			CodigoPortafolioSBS + ',' + ISNULL(CodigoTerceroCliente,'')
		ELSE '' END) AS CodPorTerCli
		,ISNULL(CodigoTerceroCliente,'') AS CodigoTerceroCliente
		,dbo.RetornarDescripcionTercero(CodigoTerceroCliente) AS DescripcionTerceroCliente,
		CodigoPortafolioSisOpe
		,FechaCajaOperaciones 
	FROM Portafolio as p  
	WHERE CodigoPortafolioSBS = case when @p_CodigoPortafolio = '' then CodigoPortafolioSBS else @p_CodigoPortafolio end  
		AND (PorSerie = @p_PorSerie or @p_PorSerie = '')
		--AND (Situacion = @p_Estado or @p_Estado = '')
		AND Situacion = 'A' --Que muestre siempre los fondos positivos
		AND CodigoPortafolioSBS <> '0'
		AND (TipoNegocio = @p_TipoNegocio OR @p_TipoNegocio = '')
		AND (FondoCliente = @p_FondoVinculado OR @p_FondoVinculado = '')
	ORDER BY Descripcion 
END
GO





GRANT EXECUTE ON [dbo].[Portafolio_listarCodigoPortafolio] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_AperturaCajaRecaudo]'
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_AperturaCajaRecaudo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_AperturaCajaRecaudo]
GO
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 27/06/2019
--	ModIFicado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: se modifica para que actualice la tabla PortafolioCaja
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 18/07/2019
--	ModIFicado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 12028 - PSC004
--	Descripcion del cambio: Se elimina validación para caja de recaudo.
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_AperturaCajaRecaudo]
	@p_CodigoPortafolio VarChar(12), 
	@p_FechaCajaOperaciones NUMERIC(8), 
	@p_FechaCreacion NUMERIC(8), 
	@p_CodigoClaseCuenta VARCHAR(3)=''
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM PortafolioCaja WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND  CodigoClaseCuenta= @p_CodigoClaseCuenta) 
		BEGIN
			INSERT INTO 
					PortafolioCaja
								(CodigoPortafolioSBS,
								 CodigoClaseCuenta,
								 FechaCajaOperaciones)
			VALUES
				(
				 @p_CodigoPortafolio,
				 @p_CodigoClaseCuenta,
				 @p_FechaCajaOperaciones
				)
		END 
	ELSE
		BEGIN
			UPDATE 
				PortafolioCaja 
			SET 
				FechaCajaOperaciones=@p_FechaCajaOperaciones    
			WHERE 
				CodigoPortafolioSBS = @p_CodigoPortafolio 
				AND  CodigoClaseCuenta= @p_CodigoClaseCuenta
		END

	--OBTENER LE FECHA MINIMO DE OPERACIONES DE TODAS LAS CAJAS
	DECLARE @FechaCajaTotal NUMERIC(8,0)= ISNULL ((SELECT MIN(FechaCajaOperaciones) 
												   FROM PortafolioCaja 
												   WHERE CodigoPortafolioSBS = @p_CodigoPortafolio
														 AND CodigoClaseCuenta <> '10'),-- 'OT12028 - PSC004 
												   @p_FechaCajaOperaciones) 
	
	--UPDATE Portafolio SET FechaCajaOperaciones = @FechaCajaTotal WHERE CodigoPortafolioSBS = @p_CodigoPortafolio

	UPDATE 
		Portafolio 
	SET 
		FechaCajaOperaciones = @FechaCajaTotal 
	WHERE 
		CodigoPortafolioSBS = @p_CodigoPortafolio
		

	EXEC sp_SIT_InsertaPendieteCaja 
									@p_CodigoPortafolio,
									@p_FechaCajaOperaciones,
									@p_FechaCreacion
	END

GO

GRANT EXECUTE ON [dbo].[sp_SIT_AperturaCajaRecaudo] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_ReversaCajaRecaudo]'
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ReversaCajaRecaudo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ReversaCajaRecaudo]
GO
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Actualiza la FechaCajaOperaciones del fondo
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 27/06/2019
--	ModIFicado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: se modifica para que actualice la tabla PortafolioCaja
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 18/07/2019
--	ModIFicado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 12028 - PSC004
--	Descripcion del cambio: Se elimina validación para caja de recaudo.
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_SIT_ReversaCajaRecaudo]
	@p_CodigoPortafolio VarChar(12), 
	@p_FechaCajaOperaciones NUMERIC(8), 
	@p_CodigoClaseCuenta VARCHAR(3)=''
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM PortafolioCaja WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND  CodigoClaseCuenta= @p_CodigoClaseCuenta) 
		BEGIN
			INSERT INTO 
				PortafolioCaja
						(CodigoPortafolioSBS,
						 CodigoClaseCuenta,
						 FechaCajaOperaciones
						)
			VALUES(
					@p_CodigoPortafolio,
					@p_CodigoClaseCuenta,
					@p_FechaCajaOperaciones
				   )
		END 
	ELSE
		BEGIN
			UPDATE 
				PortafolioCaja 
			SET 
				FechaCajaOperaciones=@p_FechaCajaOperaciones    
			WHERE 
				CodigoPortafolioSBS = @p_CodigoPortafolio 
				AND CodigoClaseCuenta= @p_CodigoClaseCuenta
		END

	

		--OBTENER LE FECHA MINIMO DE OPERACIONES DE TODAS LAS CAJAS
		DECLARE @FechaCajaTotal NUMERIC(8,0)= ISNULL ((SELECT MIN(FechaCajaOperaciones) 
													   FROM PortafolioCaja 
													   WHERE CodigoPortafolioSBS = @p_CodigoPortafolio
															 AND CodigoClaseCuenta <> '10'),-- 'OT12028 - PSC004 
													   @p_FechaCajaOperaciones) 
		
		UPDATE 
			Portafolio 
		SET 
			FechaCajaOperaciones = @FechaCajaTotal 
		WHERE 
			CodigoPortafolioSBS = @p_CodigoPortafolio	
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ReversaCajaRecaudo] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PortafolioCaja_ObtenerFechaCajaOperaciones]'
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PortafolioCaja_ObtenerFechaCajaOperaciones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PortafolioCaja_ObtenerFechaCajaOperaciones]
GO
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Obtener las fechas de cierre de caja de cada clase de cuenta
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modIFicacion: 05/06/2019
--	ModIFicado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Creacion del store procedure
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PortafolioCaja_ObtenerFechaCajaOperaciones]
@p_CodigoPortafolioSBS varchar(10),
@p_CodigoClaseCuenta varchar(3) =''
as
BEGIN


DECLARE @FechaMaximaCajaFondo NUMERIC(8,0) =  (
select MAX(FechaCajaOperaciones) from PortafolioCaja 
where CodigoPortafolioSBS = @p_CodigoPortafolioSBS)


DECLARE @FechaMininaCajaFondo NUMERIC(8,0) =  (
select MIN(FechaCajaOperaciones) from PortafolioCaja 
where CodigoPortafolioSBS = @p_CodigoPortafolioSBS)


SELECT 
ISNULL(@FechaMaximaCajaFondo,0) AS 'FechaMaximaCierre',
ISNULL(@FechaMininaCajaFondo,0) AS 'FechaMinima',
PF.FechaCajaOperaciones as 'FechaCaja',
[dbo].[fn_SIT_gl_ConvertirFechaaString] (PF.FechaCajaOperaciones) as 'FechaCajaCadena',
[dbo].[fn_SIT_gl_ConvertirFechaaString] (P.FechaCajaOperaciones) as 'FechaCajaPortafolioCadena',
PF.CodigoPortafolioSBS as 'CodigoPortafolioSBS',
P.FechaCajaOperaciones as 'FechaCajaPortafolio',
P.Descripcion as 'Portafolio',
CC.Descripcion as 'ClaseCuenta',
PF.CodigoClaseCuenta as 'CodigoClaseCuenta'
FROM PortafolioCaja PF
INNER JOIN Portafolio P on P.CodigoPortafolioSBS=PF.CodigoPortafolioSBS
INNER JOIN ClaseCuenta CC ON CC.CodigoClaseCuenta =PF.CodigoClaseCuenta and CC.Situacion='A'
WHERE PF.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
and PF.CodigoClaseCuenta = CASE WHEN @p_CodigoClaseCuenta='' THEN PF.CodigoClaseCuenta ELSE @p_CodigoClaseCuenta END
END


GRANT EXECUTE ON [dbo].[PortafolioCaja_ObtenerFechaCajaOperaciones] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Pr_Sit_listarValorCuotaTotalPorFondo]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuotaTotalPorFondo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondo]
GO
----------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 17/12/2018
-- ModIFicado por: Ricardo Colonia
-- Descripción del cambio: ModIFicación de campo Descripcion a VARCHAR(40)
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha ModIFicación: 27/12/2018
-- ModIFicado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Se cambió la función ("FormatDate") de formato de fecha, ya que la anterior ocasionaba error.
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondo](@p_fechaInicial NUMERIC(8),@p_fechaFinal NUMERIC(8))  
AS  
BEGIN  
 DECLARE @tbl AS TABLE (  
  fechaProceso VARCHAR(10),Descripcion VARCHAR(100),valorCuota NUMERIC(22,7),CuotasEmitidas NUMERIC(22,7),
  comisionAdministradoraBruta NUMERIC(22,7),
  devolucionAdministradora NUMERIC(22,7),
  comisionAdministradoraNeta NUMERIC(22,7),
  Fecha NUMERIC(8)  
 )  
 INSERT INTO @tbl  
 --SELECT fechaProceso = dbo.FN_SIT_OBT_FechaFormateada(t.FechaProceso),p.Descripcion,t.valorCuota,t.CuotasEmitidas,t.comisionAdministradora,t.FechaProceso
 SELECT fechaProceso = dbo.FormatDate(t.FechaProceso),p.Descripcion,t.valorCuota,t.CuotasEmitidas,
 comisionAdministradoraBruta = t.comisionAdministradora +t.devolucionAdministradora,
 devolucionAdministradora = t.devolucionAdministradora,
comisionAdministradoraNeta= t.comisionAdministradora 
 ,t.FechaProceso  
 FROM (  
  SELECT FechaProceso,CodigoPortafolioSBS,valorCuota = sum(ValCuotaValoresCierre),
  CuotasEmitidas = sum(ValCuotaCierre),
  comisionAdministradora = sum(ComisionSAFM),
  devolucionAdministradora=sum(Isnull(DevolucionComisionDiaria,0))  
  FROM ValorCuota  
  WHERE FechaProceso between @p_fechaInicial and @p_fechaFinal  
  GROUP BY FechaProceso,CodigoPortafolioSBS  
 ) t inner join Portafolio p on p.CodigoPortafolioSBS = t.CodigoPortafolioSBS  
 SELECT fechaProceso,Descripcion,valorCuota,CuotasEmitidas,comisionAdministradoraBruta,  devolucionAdministradora,comisionAdministradoraNeta
 FROM @tbl ORDER BY  Fecha  
END  
GO

GRANT EXECUTE ON [dbo].[Pr_Sit_listarValorCuotaTotalPorFondo] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[sp_SIT_ValidarNegociacionFondosEnOtros]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ValidarNegociacionFondosEnOtros]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ValidarNegociacionFondosEnOtros]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_SIT_ValidarNegociacionFondosEnOtros]
@p_FechaOperacion decimal,
@p_CodigoPortafolioSBS as Varchar(10)
AS
BEGIN

DECLARE @CodigoNemonico VARCHAR(20) = ISNULL((
SELECT CodigoNemonico
FROM
Valores
WHERE CodigoPortafolioSBS = @p_CodigoPortafolioSBS),'')

IF  EXISTS (SELECT 1 FROM CustodioSaldo 
WHERE CodigoMnemonico = @CodigoNemonico and FechaSaldo = @p_FechaOperacion
  and (SaldoInicialUnidades + IngresoUnidades - EgresoUnidades) > 0)
 SELECT '1'
 ELSE
 BEGIN

	DECLARE @FechaMaxima numeric(8,0)=(SELECT  MAX(FechaSaldo) FROM CustodioSaldo 
	WHERE CodigoPortafolioSBS  = @p_CodigoPortafolioSBS and FechaSaldo<=@p_FechaOperacion and (SaldoInicialUnidades + IngresoUnidades - EgresoUnidades) > 0)
	--select @FechaMaxima
   IF  EXISTS ( SELECT 1 FROM Valores WHERE CodigoNemonico in ( (SELECT  CodigoMnemonico FROM CustodioSaldo 
	WHERE CodigoPortafolioSBS  = @p_CodigoPortafolioSBS and FechaSaldo = @FechaMaxima
	and (SaldoInicialUnidades + IngresoUnidades - EgresoUnidades) > 0) 
  ) AND ISNULL(CodigoPortafolioSBS,'')<>'' )
	SELECT '0'
	ELSE
	SELECT '1'
 END
 
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ValidarNegociacionFondosEnOtros] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[PagoFechaComision_ListarPortafolios]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ListarPortafolios]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ListarPortafolios]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PagoFechaComision_ListarPortafolios]
@p_FechaCorte numeric(8,0)
AS
BEGIN
--- obtener todos los portafolios
DECLARE @TbFechaValorCuota TABLE
(
  CodigoPortafolioSBS varchar(10),
  Fecha decimal(8,0),
  Descripcion varchar(40)
)

DECLARE @TbSaldoFecha TABLE
(
  CodigoPortafolioSBS varchar(10),
  Fecha decimal(8,0)
)


DECLARE @TbSaldo TABLE
(
  CodigoPortafolioSBS varchar(10),
  Fecha decimal(8,0),
  NumeroCuenta varchar(25),
  Saldo numeric(22,7)
)

--pendiente 
DECLARE @TbFechaCobroIngresado TABLE
(  Id int,
	CodigoPortafolioSBS varchar(10),
	FechaInicio decimal(8,0),
	FechaFin decimal(8,0),
	Estado varchar(3),
	CodigoBanco varchar(12),
	NombreBanco varchar(64),
	NumeroCuenta varchar(25),
	UsuarioSolicitud varchar(15),
	Saldo decimal(22,7)
)

DECLARE @TbFechaCobro TABLE
(
  CodigoPortafolioSBS varchar(10),
  FechaInicio decimal(8,0),
  Estado varchar(3)
)


insert into 
@TbFechaValorCuota
SELECT 
VC.CodigoPortafolioSBS,
MAX(vc.ComisionSAFMAnterior ) as FechaProceso,
PO.Descripcion
FROM 
 ValorCuota  VC WITH (NOLOCK)
 inner join Portafolio PO on PO.CodigoPortafolioSBS=VC.CodigoPortafolioSBS and PO.Situacion='A'
 group by 
 VC.CodigoPortafolioSBS ,
 PO.Descripcion

 insert into @TbSaldoFecha
SELECT CodigoPortafolioSBS,
max(FechaOperacion) 
 FROM SaldosBancarios S
 WHERE S.CodigoPortafolioSBS IN (SELECT v.CodigoPortafolioSBS FROM @TbFechaValorCuota v)
 GROUP BY
  CodigoPortafolioSBS

   insert into @TbSaldo 
   SELECT 
   tf.CodigoPortafolioSBS,
   tf.fecha,
   SB.NumeroCuenta,
   SB.SaldoEstadoCTA
   FROM @TbSaldoFecha tf INNER JOIN
   SaldosBancarios SB ON SB.FechaOperacion = [dbo].[FechaSaldoPortafolioPorCuenta](tf.CodigoPortafolioSBS,'20')
   AND SB.CodigoPortafolioSBS = tf.CodigoPortafolioSBS 

 INSERT INTO 
 @TbFechaCobroIngresado
 SELECT 
 F.Id,
F.CodigoPortafolioSBS,
F.FechaInicio,
F.FechaFin,
F.Estado,
F.CodigoBanco,
t.Descripcion as 'NombreBanco',
F.NumeroCuenta,
F.UsuarioSolicitud, 
ISNULL(TS.Saldo ,0) as 'Saldo'
--0 as 'Saldo'
FROM PagoFechaComision F
--INNER JOIN Portafolio P ON P.CodigoPortafolioSBS=F.CodigoPortafolioSBS AND P.Situacion='A'
JOIN Entidad E ON E.CodigoEntidad = F.CodigoBanco AND E.Situacion = 'A'
JOIN Terceros T ON T.CodigoTercero  = e.CodigoTercero  
LEFT JOIN @TbSaldo TS ON F.CodigoPortafolioSBS=TS.CodigoPortafolioSBS 
AND TS.NumeroCuenta = F.NumeroCuenta
WHERE F.Estado='ING'

----------------------------------------------------------------------------------------------------------------




----------------------------------------------------------------------------------------------------------------
 INSERT INTO 
 @TbFechaCobro
 SELECT 
 f1.CodigoPortafolioSBS,
 f1.FechaFin ,
F1.Estado 
FROM PagoFechaComision F1 
inner join (
 SELECT 
F.CodigoPortafolioSBS,
MAX(F.FechaFin) as 'FechaMaxima'
FROM PagoFechaComision F
GROUP BY 
F.CodigoPortafolioSBS
) F2 ON F2.FechaMaxima =F1.FechaFin  and f2.CodigoPortafolioSBS=f1.CodigoPortafolioSBS




SELECT 
T.*,
PG.Nombre as 'NombreEstado',
T.NumeroDeCuenta +' | ' + [dbo].[FN_SIT_OBT_NumeroSepador](T.Saldo) as 'NombreNumeroDeCuenta',
-- OBTENER MONTO ACUMULATIVO
ISNULL(
(SELECT Sum(ComisionSAFM) FROM ValorCuota WITH (NOLOCK)
 WHERE CodigoPortafolioSBS =T.CodigoPortafolioSBS and  
 FechaProceso >=T.CodigoPortafolioSBS and  FechaProceso >=t.FechaInicio and FechaProceso <=t.FechaFin  ),0) as 'Comision',
[dbo].[fn_SIT_gl_ConvertirFechaaString] (T.FechaInicio) + ' - ' +
[dbo].[fn_SIT_gl_ConvertirFechaaString] (T.FechaFin) AS 'Periodo'
FROM
(
SELECT 
Po.CodigoPortafolioSBS,
ISNULL(FI.Id,0) as 'Id',
Po.Descripcion as 'Portafolio',
Po.CodigoPortafolioSisOpe,
Mo.Descripcion as 'NombreMoneda',
Mo.CodigoMoneda as 'CodigoMoneda',
ISNULL(FI.Estado,'DIS')  as 'CodigoEstado',
CASE 
	WHEN FC.Estado IS NOT NULL THEN 
	( CASE WHEN FC.Estado='ING' THEN FI.FechaInicio
	 WHEN FC.Estado='CON' THEN [dbo].[RetornarFechaProxima](FC.FechaInicio,1)   END )
	ELSE  VC.Fecha
	END AS 'FechaInicio',
CASE 
	WHEN FC.Estado IS NOT NULL THEN 
	( CASE WHEN FC.Estado='ING' THEN FI.FechaFin
	 WHEN FC.Estado='CON' THEN @p_FechaCorte  END )
	ELSE  @p_FechaCorte
	END AS 'FechaFin',
ISNULL(FI.CodigoBanco,'') as 'CodigoBanco',
ISNULL(FI.NombreBanco,'') as 'NombreBanco',
ISNULL(FI.NumeroCuenta,'') as 'NumeroDeCuenta',
ISNULL(FI.UsuarioSolicitud,'') as  'UsuarioSolicitud',
ISNULL(FI.Saldo,0) AS 'Saldo',
Po.FechaCajaOperaciones,
Pc.FechaCajaOperaciones as 'FechaCaja',
[dbo].[fn_SIT_gl_ConvertirFechaaString] (Pc.FechaCajaOperaciones) as 'FechaCajaCadena'
FROM 
Portafolio Po 
INNER JOIN Moneda Mo ON Mo.CodigoMoneda =Po.CodigoMoneda 
INNER JOIN @TbFechaValorCuota VC ON VC.CodigoPortafolioSBS=Po.CodigoPortafolioSBS
LEFT JOIN  @TbFechaCobroIngresado FI ON FI.CodigoPortafolioSBS=Po.CodigoPortafolioSBS
LEFT JOIN  @TbFechaCobro FC ON FC.CodigoPortafolioSBS=Po.CodigoPortafolioSBS
LEFT JOIN PortafolioCaja  Pc ON  Pc.CodigoClaseCuenta='20'  and Pc.CodigoPortafolioSBS=Po.CodigoPortafolioSBS 

WHERE Po.Situacion='A') T
INNER JOIN  ParametrosGenerales PG ON PG.Valor=T.CodigoEstado AND PG.ClasIFicacion='FECHACOBRO_ESTADO'
ORDER BY T.Portafolio 

--SELECT * FROM @TbFechaCobroIngresado
--SELECT * FROM @TbFechaCobro
END
GO


GRANT EXECUTE ON [dbo].[PagoFechaComision_ListarPortafolios] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PagoFechaComision_Actualizar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_Actualizar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_Actualizar]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[PagoFechaComision_Actualizar]
@p_Id INT,
@p_Estado varchar(3),
@p_CodigoBanco varchar(12),
@p_NumeroCuenta varchar(25),
@p_CodigoBancoAdministradora varchar(12),
@p_NumeroCuentaAdministradora varchar(25),
@p_ComisionAcumulada numeric(22,7),
@p_SaldoDisponible numeric(22,7),
@p_Usuario varchar(15),
@p_Hora varchar(10),
@p_Fecha numeric(8),
@p_FechaConfirmacion numeric(8)

AS
BEGIN
 UPDATE PagoFechaComision 
 set Estado=@p_Estado,
 NumeroCuenta=@p_NumeroCuenta,
 CodigoBanco=@p_CodigoBanco,
 ComisionAcumulada= @p_ComisionAcumulada,
 SaldoDisponible=@p_SaldoDisponible,
 CodigoBancoAdministradora=@p_CodigoBancoAdministradora,
 NumeroCuentaAdministradora=@p_NumeroCuentaAdministradora,
 FechaConfirmacion=@p_FechaConfirmacion,
 HoraModIFicacion=@p_Hora,
 FechaModIFicacion=@p_Fecha,
 UsuarioModIFicacion=@p_Usuario 
 WHERE Id = @p_Id

END

GO


GRANT EXECUTE ON [dbo].[PagoFechaComision_Actualizar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PagoFechaComision_ActualizarEstado]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ActualizarEstado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ActualizarEstado]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[PagoFechaComision_ActualizarEstado]
@p_Id INT,
@p_Estado varchar(3),
@p_Usuario varchar(15),
@p_Hora varchar(10),
@p_Fecha numeric(8)
AS
BEGIN
 update PagoFechaComision 
 set Estado=@p_Estado,
 HoraModIFicacion=@p_Hora,
 FechaModIFicacion=@p_Fecha,
 UsuarioModIFicacion=@p_Usuario 
 WHERE Id = @p_Id

END

GO


GRANT EXECUTE ON [dbo].[PagoFechaComision_ActualizarEstado] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PagoFechaComision_EliminarConfirmado]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_EliminarConfirmado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_EliminarConfirmado]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 28/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[PagoFechaComision_EliminarConfirmado]
@p_IdPagoFechaComision int,
@p_CodigoPortafolioSBS varchar(10),
@p_Usuario varchar(15),
@p_Hora varchar(10),
@p_Fecha numeric(8)
AS
BEGIN
declare @FechaFinMaxima numeric(8,0)=(
select MAX(fechafin) from PagoFechaComision where estado='CON' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS
)

declare @IdPagoFechaComision int
declare @FechaInicio numeric(8,0)
declare @FechaFin numeric(8,0)
set @IdPagoFechaComision =( select Id from PagoFechaComision where estado='CON' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and FechaFin=@FechaFinMaxima)

select @IdPagoFechaComision=Id,@FechaInicio=FechaInicio,@FechaFin=FechaFin  from PagoFechaComision where estado='CON' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and FechaFin=@FechaFinMaxima

---

IF @p_IdPagoFechaComision<>ISNULL(@IdPagoFechaComision,0)
BEGIN
			IF ISNULL(@IdPagoFechaComision,0) = 0			
				BEGIN
					PRINT'NO EXISTE ID'
					SELECT 'No existe una fecha de pago en estado confirmada'
				END
			ELSE
				BEGIN
				PRINT'DIFERENTE ID'
					SELECT 'Solo se puede eliminar la ultima fecha de pago confirmada. Fecha de corte: '
					+ [dbo].[fn_SIT_gl_ConvertirFechaaString](@FechaFin)
				END

		
		 
END
ELSE
BEGIN
	--VERIFICAR SI EXISTE MOVIMIENTO ACTIVO
	IF NOT EXISTS (SELECT 1 FROM OperacionesCaja WHERE PagoFechaComision= @IdPagoFechaComision and Situacion='A')
		BEGIN
		PRINT'UPDATE PagoFechaComision '
		--ACTUALIZO EL ESTADO A INGRESADO
		UPDATE PagoFechaComision 
		SET Estado='ING',
		FechaConfirmacion=NULL,
		 HoraModIFicacion=@p_Hora,
		 FechaModIFicacion=@p_Fecha,
		 UsuarioModIFicacion=@p_Usuario 
		  WHERE  Id= @IdPagoFechaComision
		SELECT ''
		END
	ELSE
		BEGIN
		PRINT'Eliminar OperacionesCaja'
		--MENSAJE DE VALIDACION QUE DEBE ELIMINAR EL REGISTRO DE OPERACION DE CAJA
			declare @FechaPago numeric(8,0)
			declare @CodigoCaja varchar(10) 
			declare @NumeroDeCuenta varchar(25)
			declare @Banco varchar(65)

			SELECT @FechaPago=FechaOperacion,@CodigoCaja=CodigoOperacionCaja FROM OperacionesCaja WHERE PagoFechaComision= @IdPagoFechaComision and Situacion='A'

			select @Banco=T.Descripcion,@NumeroDeCuenta = P.NumeroCuenta
			from PagoFechaComision P 
			INNER JOIN  entidad E ON E.CodigoEntidad=P.CodigoBanco 
			INNER JOIN Terceros T on E.codigotercero=T.CodigoTercero WHERE P.ID=@IdPagoFechaComision
			

			SELECT 'Se debe eliminar el movimiento de caja con codigo ' + @CodigoCaja + ':<br>-Fecha de pago: ' + [dbo].[fn_SIT_gl_ConvertirFechaaString](@FechaPago) +'<br>-Banco: '+@Banco +'<br>-NumeroCuenta: ' +@NumeroDeCuenta
		END

END

END



GRANT EXECUTE ON [dbo].[PagoFechaComision_EliminarConfirmado] TO [rol_sit_fondos] AS [dbo]
GO





PRINT '[dbo].[PagoFechaComision_ListarBancos]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ListarBancos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ListarBancos]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[PagoFechaComision_ListarBancos]
@p_CodigoPortafolioSBS varchar(10),
@p_CodigoMoneda varchar(10)
AS
BEGIN
	---OBTENER BANCOS
	SELECT 
	DISTINCT 
	T.CodigoTercero,
	T.Descripcion as 'Descripcion',
	E.CodigoEntidad  as CodigoBanco
	FROM CuentaEconomica CE
	JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera AND E.Situacion = 'A'
	JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
	WHERE 
	CE.CodigoPortafolioSBS =@p_CodigoPortafolioSBS
	and CE.CodigoMoneda =	CASE WHEN @p_CodigoPortafolioSBS='23'  THEN CE.CodigoMoneda ELSE  @p_CodigoMoneda END
	and ce.CodigoClaseCuenta='20'
	and ISNULL(ce.NumeroCuenta,'' )<>''
	order by t.Descripcion 
END
GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_ListarBancos] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[PagoFechaComision_ListarNumeroDeCuentas]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ListarNumeroDeCuentas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ListarNumeroDeCuentas]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PagoFechaComision_ListarNumeroDeCuentas]
@p_CodigoPortafolioSBS varchar(10),
@p_CodigoMoneda varchar(12),
@p_CodigoBanco varchar(12)
AS
BEGIN

	DECLARE @FechaMaxima numeric(8,0) = (
	SELECT 

	MAX(SB.FechaOperacion)
	FROM  
	SaldosBancarios SB 
	WHERE SB. CodigoPortafolioSBS=@p_CodigoPortafolioSBS and NumeroCuenta in (select NumeroCuenta  from CuentaEconomica where CodigoClaseCuenta='20' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and numerocuenta<>'' and Situacion='A' )
	)

	
---OBTENER NUMEROS DE CUENTAS
	SELECT DISTINCT
	TE.*,
	CASE WHEN @p_CodigoPortafolioSBS='23'  THEN TE.NumeroCuenta +' | ' + TE.CodigoMoneda +  ' ' + [dbo].[FN_SIT_OBT_NumeroSepador](TE.Saldo)
	 ELSE  TE.NumeroCuenta +' | ' +  [dbo].[FN_SIT_OBT_NumeroSepador](TE.Saldo) END as 'NombreNumeroDeCuenta'
	 FROM (
	SELECT 
	T.CodigoTercero,
	E.CodigoEntidad AS CodigoBanco,
	T.Descripcion Tercero,
	ce.CodigoMoneda ,
	ce.NumeroCuenta,
	ISNULL(SaldoEstadoCTA,0) as 'Saldo'
	FROM CuentaEconomica CE
	LEFT JOIN SaldosBancarios SB ON SB.FechaOperacion = @FechaMaxima  AND SB.CodigoPortafolioSBS = CE.CodigoPortafolioSBS  AND SB.NumeroCuenta = CE.NumeroCuenta
	JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera AND E.Situacion = 'A'
	JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
	WHERE 
	CE.CodigoPortafolioSBS =@p_CodigoPortafolioSBS
	and CE.CodigoClaseCuenta='20'
	and CE.CodigoMoneda = CASE WHEN @p_CodigoPortafolioSBS='23'  THEN CE.CodigoMoneda ELSE  @p_CodigoMoneda END
	and E.CodigoEntidad =@p_CodigoBanco
	and ISNULL(ce.NumeroCuenta,'' )<>''
	) TE 
	WHERE TE.Saldo>0
	order by te.Tercero

END
GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_ListarNumeroDeCuentas] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_OBT_OtrasCXP]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_OBT_OtrasCXP]') AND type in (N'P', N'PC'))
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
-- Fecha ModIFicación: 25/01/2017    
-- ModIFicado por: Carlos Espejo    
-- Nro. Orden de Trabajo: 9851    
-- Descripcion del cambio: Creacion    
----------------------------------------------------------------------------------------------------    
-- Fecha ModIFicación: 16/02/2017    
-- ModIFicado por: Carlos Espejo    
-- Nro. Orden de Trabajo: 9993    
-- Descripcion del cambio: Se cambia RECAUDO por SUSCRIPCION    
-------------------------------------------------------------------------    
-- Fecha ModIFicación: 20/02/2017    
-- ModIFicado por: Carlos Espejo    
-- Nro. Orden de Trabajo: 10001    
-- Descripcion del cambio: Se ajusta la fecha para los dias domingos y feriados    
--------------------------------------------------------------------------------    
-- Fecha ModIFicación: 16/02/2017    
-- ModIFicado por: Carlos Espejo    
-- Nro. Orden de Trabajo: 9981    
-- Descripcion del cambio: Se cambio el campo @p_MesAnterior para obtener el numero de meses a usar    
----------------------------------------------------------------------------------------------------    
-- Fecha ModIFicación: 02/05/2017    
-- ModIFicado por: Carlos Espejo    
-- Nro. Orden de Trabajo: 10335    
-- Descripcion del cambio:  Se indica si existe datos para la caja de recaudo    
-----------------------------------------------------------------------------------------------------  
-- Fecha de ModIFicación: 21/09/2018    
-- ModIFicado por: Ricardo Colonia   
-- Nro. Orden de Trabajo: 11568  
-- Descripción del cambio: Se agrega detalle de interés de aumento de capital.  
------------------------------------------------------------------------------------------------------  
-- Fecha de ModIFicación: 02/10/2018    
-- ModIFicado por: Ricardo Colonia   
-- Nro. Orden de Trabajo: 11547  
-- Descripción del cambio: Se retira detalle de interés de aumento de capital.  
------------------------------------------------------------------------------------------------------  
-- Fecha de ModIFicación: 02/11/2018    
-- ModIFicado por: Ricardo Colonia   
-- Nro. Orden de Trabajo: 11652 
-- Descripción del cambio: Se agrega detalle de interés de aumento de capital.  
------------------------------------------------------------------------------------------------------  
-- Fecha de ModIFicación: 07/06/2019    
-- ModIFicado por: Ernesto Galarza  
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Quitando al total, la suma de la devolucion de comision. 
------------------------------------------------------------------------------------------------------  
CREATE PROC [dbo].[sp_SIT_OBT_OtrasCXP](    
 @p_CodigoPortafolio VarChar(10),    
 @p_FechaOperacion Numeric(8),    
 @p_ChequePendiente Numeric(22,7),    
 @p_RescatePendiente Numeric(22,7),    
 @p_MesAnterior  NUMERIC(8), --9981
 @p_DevolucionAcumulada  Numeric(22,7)
)    
AS    
BEGIN    
 DECLARE @ComisionSAFM NUMERIC(22,7),  @DevolucionAcumulada NUMERIC(22,7),@CajaRecaudo NUMERIC(22,7), @Suscripcion NUMERIC(22,7),@p_FechaOperacionFeriado NUMERIC(8)    
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
 SELECT @ComisionSAFM = SUM(ComisionSAFM), @DevolucionAcumulada=SUM(DevolucionComisionDiaria) FROM ValorCuota VC    
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
 ISNULL(@ComisionSAFM,0) + ISNULL(@CajaRecaudo,0) + ISNULL(@Suscripcion,0) + @p_ChequePendiente + @p_RescatePendiente  +  (ISNULL(@totalInteresDistribucion,0) - ISNULL(@p_DevolucionAcumulada,0)  )OtrasCXP    
END    



GRANT EXECUTE ON [dbo].[sp_SIT_OBT_OtrasCXP] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[PagoFechaComision_Insertar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_Insertar]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 12/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[PagoFechaComision_Insertar]
@p_CodigoPortafolioSBS varchar(10),
@p_CodigoBanco varchar(12),
@p_NumeroCuenta varchar(25),
@p_FechaInicio numeric(8),
@p_FechaFin numeric(8),
@p_FechaSolicitud numeric(8),
@p_UsuarioSolicitud varchar(15),
@p_Usuario varchar(15),
@p_Hora varchar(10),
@p_Fecha numeric(8),
@p_Estado varchar(3),
@p_ComisionAcumulada numeric(22,7),
@p_SaldoDisponible numeric(22,7)
AS
BEGIN


INSERT INTO PagoFechaComision(
CodigoPortafolioSBS,
CodigoBanco,
NumeroCuenta,
FechaInicio,
FechaFin,
FechaSolicitud,
UsuarioSolicitud,
HoraCreacion,
FechaCreacion,
UsuarioCreacion,
Estado,
ComisionAcumulada,
SaldoDisponible
)
VALUES(
@p_CodigoPortafolioSBS,
@p_CodigoBanco,
@p_NumeroCuenta,
@p_FechaInicio,
@p_FechaFin,
(select isnull(FechaCajaOperaciones,0) from PortafolioCaja where CodigoPortafolioSBS=@p_CodigoPortafolioSBS and CodigoClaseCuenta='20'),
@p_UsuarioSolicitud,
@p_Hora,
@p_Fecha,
@p_Usuario,
@p_Estado,
@p_ComisionAcumulada,
@p_SaldoDisponible
)
END
GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PagoFechaComision_Eliminar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_Eliminar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_Eliminar]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 12/06/2019
-- ModIFicado por: Ernesto Galarza
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[PagoFechaComision_Eliminar]
@p_Id Int
AS
BEGIN
	DELETE FROM PagoFechaComision WHERE Id = @p_Id
END
GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_Eliminar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PagoFechaComision_ListarFondosCustodio]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ListarFondosCustodio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ListarFondosCustodio]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 12/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PagoFechaComision_ListarFondosCustodio]
@p_CodigoPortafolioSBS varchar(10),
@p_FechaCorte numeric(8,0)
AS
BEGIN
	SELECT 
	PF.Id,
	PF.CodigoPortafolioSBS,
	Po.CodigoMoneda,
	Po.Descripcion as 'Portafolio',
	[dbo].[fn_SIT_gl_ConvertirFechaaString] (PF.FechaFin)  as 'FechaCorte',
	PF.FechaFin,
	PF.UsuarioSolicitud,
	PF.ComisionAcumulada,
	PF.NumeroCuenta,
	PF.CodigoBanco,
	Mo.Descripcion as 'NombreMoneda',
	PF.Estado as 'CodigoEstado',
	PG.Nombre as 'NombreEstado',
	Po.Descripcion as 'NombreFondo',
	[dbo].[fn_SIT_gl_ConvertirFechaaString] (PF.FechaInicio) + ' - ' +
	[dbo].[fn_SIT_gl_ConvertirFechaaString] (PF.FechaFin) AS 'Periodo',
	ISNULL(Pc.FechaCajaOperaciones,0) as 'FechaCaja',
	PF.FechaSolicitud,
	[dbo].[fn_SIT_gl_ConvertirFechaaString] (PF.FechaSolicitud)  as 'FechaSolicitudCadena'
	FROM 
	PagoFechaComision PF 
	INNER JOIN  Portafolio Po ON Po.CodigoPortafolioSBS=PF.CodigoPortafolioSBS
	INNER JOIN  ParametrosGenerales PG ON PG.Valor=PF.Estado AND PG.ClasIFicacion='FECHACOBRO_ESTADO'
	INNER JOIN Moneda Mo ON Mo.CodigoMoneda =Po.CodigoMoneda 
	LEFT JOIN PortafolioCaja  Pc ON  Pc.CodigoClaseCuenta='20'  and Pc.CodigoPortafolioSBS=Po.CodigoPortafolioSBS 
	WHERE PF.FECHAFIN=@p_FechaCorte
	AND PF.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS='' THEN PF.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END
END
GO


GRANT EXECUTE ON [dbo].[PagoFechaComision_ListarFondosCustodio] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[sp_SIT_ListarFondosNombreConSerie]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ListarFondosNombreConSerie]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ListarFondosNombreConSerie]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 12/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[sp_SIT_ListarFondosNombreConSerie]
@p_CodigoPortafolioSBS varchar(10) =''
AS
BEGIN
SELECT 
po.CodigoPortafolioSBS, 
po.Descripcion + ' ' +isnull(ps.CodigoSerie,'') as 'NombreFondoSerie',
PO.CodigoMoneda   
  FROM Portafolio po 
left  JOIN PortafolioSerie ps on ps.CodigoPortafolioSBS=po.CodigoPortafolioSBS
WHERE po.CodigoPortafolioSBS  =  case when  @p_CodigoPortafolioSBS='' then po.CodigoPortafolioSBS else @p_CodigoPortafolioSBS end
order by po.CodigoPortafolioSBS

END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ListarFondosNombreConSerie] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[PagoFechaComision_Seleccionar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_Seleccionar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_Seleccionar]
GO
CREATE PROCEDURE [dbo].[PagoFechaComision_Seleccionar]
@p_Id int,
@p_NumeroCuenta varchar(25) ='',
@p_CodigoPortafolioSBS varchar(10)=''
AS
BEGIN

DECLARE @SaldoOnline numeric(22,7)
DECLARE @NumeroCuenta varchar(25) 
DECLARE @CodigoPortafolioSBS varchar(10)


SELECT
@CodigoPortafolioSBS=CodigoPortafolioSBS,
@NumeroCuenta = NumeroCuenta
FROM 
PagoFechaComision with(NOLOCK) WHERE Id= @p_Id


IF @p_NumeroCuenta<>''
	set @NumeroCuenta=@p_NumeroCuenta

IF @p_CodigoPortafolioSBS<>''
	set @CodigoPortafolioSBS=@p_CodigoPortafolioSBS

DECLARE @FechaMaxima numeric(8,0) = (
	SELECT 
	MAX(SB.FechaOperacion)
	FROM  
	SaldosBancarios SB 
	WHERE SB. CodigoPortafolioSBS=@p_CodigoPortafolioSBS and NumeroCuenta in (select NumeroCuenta  from CuentaEconomica where CodigoClaseCuenta='20' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and numerocuenta<>'' and situacion='A')
	)

   SELECT @SaldoOnline = SaldoEstadoCTA FROM
   SaldosBancarios SB
   WHERE SB.FechaOperacion = @FechaMaxima  AND SB.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND SB.NumeroCuenta = @NumeroCuenta


	SELECT
	pf.Id,
	pf.CodigoPortafolioSBS,
	pf.CodigoBanco,
	pf.NumeroCuenta,
	pf.FechaInicio,
	pf.FechaFin,
	pf.FechaSolicitud,
	pf.ComisionAcumulada,
	pf.SaldoDisponible as 'SaldoDisponible',
	@SaldoOnline as 'SaldoOnline',
	pf.CodigoOperacionCaja,
	pf.UsuarioSolicitud,
	po.CodigoMoneda,
	pf.Estado,
	ISNULL(Pc.FechaCajaOperaciones,0) as 'FechaCaja',
	po.FechaCajaOperaciones 
	FROM 
	PagoFechaComision pf
	INNER JOIN Portafolio po on po.CodigoPortafolioSBS=pf.CodigoPortafolioSBS 
	LEFT JOIN PortafolioCaja  Pc ON  Pc.CodigoClaseCuenta='20'  and Pc.CodigoPortafolioSBS=po.CodigoPortafolioSBS 
	WHERE pf.Id= @p_Id

END
GO
GRANT EXECUTE ON [dbo].[PagoFechaComision_Seleccionar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[PagoFechaComision_ObtenerFechaComision]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ObtenerFechaComision]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ObtenerFechaComision]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicación: 12/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripción del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[PagoFechaComision_ObtenerFechaComision]
@p_CodigoPortafolioSBS varchar(10) ='',
@p_FechaOperacion numeric(8,0)
AS
BEGIN

DECLARE @FechaMaximaPago numeric(8,0) = (SELECT MAX(FechaFin) FROM PagoFechaComision WHERE ESTADO='CON' AND CodigoPortafolioSBS=@P_CodigoPortafolioSBS )

DECLARE @FechaComision numeric(8,0)

IF @p_FechaOperacion>@FechaMaximaPago
SET @FechaComision = [dbo].[RetornarFechaProxima](@FechaMaximaPago,1)
ELSE
SET @FechaComision = (SELECT FechaInicio  FROM PagoFechaComision WHERE FechaFin = @FechaMaximaPago AND  ESTADO='CON' AND CodigoPortafolioSBS=@P_CodigoPortafolioSBS)

select @FechaComision as 'FechaComision', [dbo].[fn_SIT_gl_ConvertirFechaaString] (@FechaComision) as 'FechaComisionCadena'

END
GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_ObtenerFechaComision] TO [rol_sit_fondos] AS [dbo]
GO







PRINT '[dbo].[SP_SIT_OperacionesCaja_Insertar]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_SIT_OperacionesCaja_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_SIT_OperacionesCaja_Insertar]
GO

-----------------------------------------------------------------------------------------------------------  
-- Objetivo: Insertar las operaciones de caja  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 28-12-2016  
-- Modificado por: Everth Martinez  
-- Nro. Orden de Trabajo: 9759  
-- Descripcion del cambio: se modifico el tamanio del parametro p_CodigoOperacion a 10 caracteres  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 21/04/2017  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 10238  
-- Descripcion del cambio: Se incluye el calculo para los saldos finales  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 06/09/2017  
-- Modificado por: Ian Pastor  
-- Nro. Orden de Trabajo: 10749  
-- Descripcion del cambio: Corrección de los calculos de los saldos bancarios para los días posteriores  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 26/09/2017  
-- Modificado por: Ian Pastor  
-- Nro. Orden de Trabajo: 10813  
-- Descripcion del cambio: Ampliar parámetro @p_NroCuenta a VARCHAR(30)  
-----------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 19/09/2018  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11568
-- Descripción del cambio: Se Agrega Generación de OI en Aumento Capital
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Modificación: 18/06/2019  
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028
-- Descripción del cambio: Se agrego el campo PagoFechaComision
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[SP_SIT_OperacionesCaja_Insertar]    
(  
 @p_CodigoMercado VARCHAR(3),  
 @p_CodigoPortafolio VARCHAR(10),  
 /*OT9759 INICIO */  
 @p_CodigoOperacion VARCHAR(10),  
 /*OT9759 FIN */  
 @p_NroCuenta VARCHAR(30),  
 @p_CodigoModalidadPago VARCHAR(4),  
 @p_CodigoTercero VARCHAR(10),  
 @p_CodigoTerceroDestino VARCHAR(10),  
 @p_NroCuentaRef VARCHAR(25),  
 @p_Descripcion VARCHAR(50),  
 @p_CodigoMoneda VARCHAR(10),  
 @p_Importe NUMERIC(22,7),  
 @p_usuario VARCHAR(15),  
 @p_fecha NUMERIC(8,0),  
 @p_Hora VARCHAR(10),  
 @p_Host VARCHAR(20),  
@p_CodigoEjecucion VARCHAR(10),  
 @nvcCodigoModelo NVARCHAR(4),  
 @p_FechaOperacion NUMERIC(8,0),
 @p_PagoFechaComision INT=NULL,
 @p_Result VARCHAR(12) = '' OUT
)  
AS    
BEGIN    
	BEGIN TRANSACTION 
		BEGIN TRY
			 DECLARE @CodigoOperacionCaja VARCHAR(20),@CorrelativoCartas INT  
			 DECLARE @v_CodigoOperacionesCaja int,@ClaseCuentaOrigen VARCHAR(3),@chrTipoMovimiento Char(1)    
			 DECLARE @FechaPosterior NUMERIC(8)  
			 DECLARE @V_OperacionGenerada AS VARCHAR (10)
			 SELECT @v_CodigoOperacionesCaja = ISNULL(MAX(CONVERT(INT,SUBSTRING(CodigoOperacionCaja,2,LEN(CodigoOperacionCaja)-1))),0)+ 1    
			 FROM OperacionesCaja WHERE CodigoPortafolioSBS = @p_CodigoPortafolio  AND SUBSTRING(CodigoOperacionCaja,1,1) = 'N'    
			 IF @p_CodigoModalidadPago='' SET @p_CodigoModalidadPago=NULL    
			 IF @p_CodigoTercero='' SET @p_CodigoTercero=NULL    
			 IF @p_CodigoTerceroDestino='' SET @p_CodigoTerceroDestino=NULL    
			 IF @p_NroCuentaRef='' SET @p_NroCuentaRef=NULL  
			 SET @p_Result = '0'
			 --OT10749 Inicio  
			 SELECT @ClaseCuentaOrigen = CodigoClaseCuenta    
			 FROM CuentaEconomica    
			 WHERE NumeroCuenta = @p_NroCuenta AND CodigoPortafolioSBS = @p_CodigoPortafolio AND Situacion = 'A'  
			 --OT10749 Fin  
			   
			 --Correlativo Cartas  
			 IF @nvcCodigoModelo = 'SC01'  
			  SET @CorrelativoCartas = 0  
			 ELSE  
			 BEGIN  
			  SELECT @CorrelativoCartas = MAX(CorrelativoCartas) FROM OperacionesCaja WHERE FechaOperacion = @p_fecha AND CodigoModelo <> 'SC01'  
			  SET @CorrelativoCartas = ISNULL(@CorrelativoCartas,0) + 1  
			 END 
			   
			 --  
			 SELECT @v_CodigoOperacionesCaja    
			 INSERT INTO dbo.OperacionesCaja(CodigoOperacionCaja, CodigoMercado, CodigoClaseCuenta,NumeroCuenta,Referencia, Importe, UsuarioCreacion, FechaCreacion,    
			 CodigoMoneda, TipoMovimiento, Situacion, Host,NumeroCarta, HoraCreacion, NumeroCuentaDestino, UsuarioModificacion,CodigoClaseCuentaDestino, FechaModificacion,    
			 CodigoTerceroOrigen, HoraModificacion,CodigoModalidadPago, CodigoOperacion, CodigoTerceroDestino, CodigoPortafolioSBS,Estado,FechaOperacion, CodigoModelo,CorrelativoCartas,PagoFechaComision)  
			 VALUES('N'+CONVERT(VARCHAR(6),@v_CodigoOperacionesCaja), @p_CodigoMercado, @ClaseCuentaOrigen, @p_NroCuenta,@p_Descripcion, @p_Importe,@p_usuario, @p_fecha,  
			 @p_CodigoMoneda, 'M', 'A', @p_Host,NULL, @p_Hora, @p_NroCuentaRef, NULL,NULL, NULL, @p_CodigoTercero, @p_Hora,@p_CodigoModalidadPago, @p_CodigoOperacion,  
			 @p_CodigoTerceroDestino,@p_CodigoPortafolio,'A',@p_FechaOperacion, @nvcCodigoModelo,@CorrelativoCartas,@p_PagoFechaComision)  
			   
			 --Actualiza Ingreso y egreso  
			 SELECT @chrTipoMovimiento = CASE WHEN t.Egreso = 'S' THEN 'E' ELSE 'I' END FROM Operacion o    
			 INNER JOIN TipoOperacion t ON o.CodigoTipoOperacion = t.CodigoTipoOperacion    
			 WHERE o.CodigoOperacion = @p_CodigoOperacion  
			   
			 --OT10749 Inicio  
			 /*  
			 --Actualiza los saldos bancarios de la operacion si esta se ha realizado en dias anteriores    
			 SET @CodigoOperacionCaja = 'N'+CONVERT(VARCHAR(6),@v_CodigoOperacionesCaja)  
			 EXEC sp_SIT_ActualizaSaldoBancoDiasPosteriores @p_FechaOperacion, @CodigoOperacionCaja, @p_CodigoPortafolio  
			 */  
			   
			 --Actualizar los saldos bancarios del día  
			 EXEC usp_UpdateSaldosBancariosOperaciones @p_CodigoPortafolio,@p_FechaOperacion,@p_NroCuenta,@chrTipoMovimiento,@p_Importe  
			 EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,@p_NroCuenta,@p_FechaOperacion --10238  
			   
			 --Actualizar los saldos iniciales para el día posterior  
			 SET @FechaPosterior = dbo.DiaHabilSiguiente(@p_FechaOperacion,@p_CodigoMercado)  
			 IF EXISTS(SELECT 1 FROM SaldosBancarios WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND FechaOperacion = @FechaPosterior AND NumeroCuenta = @p_NroCuenta)  
			 BEGIN  
			  EXEC sp_SIT_ActualizaSaldoBancoDiasPosteriores @p_NroCuenta,@p_CodigoPortafolio,@p_FechaOperacion,@FechaPosterior,@p_usuario,@p_fecha,@p_Hora,@p_Host  
			  EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio, @p_NroCuenta,@FechaPosterior  
			 END  
			 --OT10749 Fin  
			 
			 -- Inicio | Generación de OI en Aumento Capital | 19092018
			
			 IF @p_CodigoOperacion = 'OP0092' BEGIN
				 SET @V_OperacionGenerada = ('N' + CONVERT(VARCHAR(6),@v_CodigoOperacionesCaja))
				 EXEC AumentoCapital_GenerarOI 
												@p_CodigoPortafolio,
												@p_FechaOperacion,
												@V_OperacionGenerada,
												@p_usuario,  
												@p_fecha,  
												@p_Hora,  
												@p_Host
			 END
-- FIN | Generación de OI en Aumento Capital | 19092018
		COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		SELECT @p_Result = CAST(ERROR_NUMBER() AS VARCHAR(12))
	END CATCH
END 
GO


GRANT EXECUTE ON [dbo].[SP_SIT_OperacionesCaja_Insertar] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[PagoFechaComision_ValidarExistenciaIngresados]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ValidarExistenciaIngresados]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PagoFechaComision_ValidarExistenciaIngresados]
GO
CREATE PROCEDURE [dbo].[PagoFechaComision_ValidarExistenciaIngresados]
@p_CodigoPortafolioSBS VARCHAR(10)
AS
BEGIN

SELECT * FROM PagoFechaComision where Estado='ING' AND CodigoPortafolioSBS=@p_CodigoPortafolioSBS
END
GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_ValidarExistenciaIngresados] TO [rol_sit_fondos] AS [dbo]
GO



PRINT 'INICIO - ELIMINAR CONSTRAINT'
IF EXISTS (select 1 from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'FK_AumentoCapitalDetalle_Valores')
BEGIN
	ALTER TABLE AumentoCapitalDetalle DROP CONSTRAINT FK_AumentoCapitalDetalle_Valores
END
GO
PRINT 'FIN - ELIMINAR CONSTRAINT'


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
--	Fecha Creacion: 10/06/2019
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Se filtran las cuentas bancarias activas y de tipo de inversión.
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
		AND Situacion = 'A' AND CodigoClaseCuenta = '20'

	RETURN ISNULL(@thereIs,'N')
END
GO

GRANT EXECUTE ON [dbo].[RetornarExisteCuentaEconomicaTercero] TO [rol_sit_fondos] AS [dbo]
GO



USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'sp_SIT_ActualizaNemomicoIsin')
	DROP PROCEDURE [dbo].[sp_SIT_ActualizaNemomicoIsin]
GO
-------------------------------------------------------------------------------------------------------------------------------
--Objetivo: ACTUALIZAR EL NEMÓNICO DE UN INSTRUMENTO FINANCIERO
-------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 08/06/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11432
--	Descripcion del cambio: Incluir transacción en el proceso de actualización (Nemónico) de un instrumento financiero
-------------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 19/06/2019
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Incluir nuevas tablas: ValorizacionAmortizada, DividendosRebatesLiberadas, AumentoCapitalDetalle
-------------------------------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_ActualizaNemomicoIsin]
	@p_TipoActualizacion CHAR(1),
	@p_CodigoMnemonico VARCHAR(12),
	@p_CodigoIsin VARCHAR(12)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
		--Se actualiza el nemonico y se toma como llave el ISIN
		IF @p_TipoActualizacion = '1'
		BEGIN
			DECLARE @CodNemonicoACT VARCHAR(20)
			SELECT @CodNemonicoACT = CodigoNemonico FROM Valores WHERE CodigoISIN = @p_CodigoIsin
			UPDATE Valores SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT
			UPDATE OrdenInversion SET CodigoMnemonico = @p_CodigoMnemonico WHERE CodigoMnemonico = @CodNemonicoACT
			UPDATE CarteraTituloValoracion SET CodigoMnemonico = @p_CodigoMnemonico WHERE CodigoMnemonico = @CodNemonicoACT
			UPDATE CustodioSaldo SET CodigoMnemonico = @p_CodigoMnemonico WHERE CodigoMnemonico = @CodNemonicoACT
			UPDATE CustodioValores SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT
			UPDATE SaldosCarteraTitulo SET CodigoMnemonico = @p_CodigoMnemonico WHERE CodigoMnemonico = @CodNemonicoACT
			UPDATE cuentasporcobrarpagar SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT
			UPDATE CodigoValor SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT
			UPDATE CuponeraNormal SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT
			UPDATE CuponeraPagos SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT 
			UPDATE GrupoPorNemonico SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT 
			UPDATE VectorPrecio SET CodigoMnemonico = @p_CodigoMnemonico WHERE CodigoMnemonico = @CodNemonicoACT 
			UPDATE VectorPrecioPIP SET Codigonemonico = @p_CodigoMnemonico WHERE Codigonemonico = @CodNemonicoACT 
			UPDATE ValorizacionAmortizada SET CodigoNemonico = @p_CodigoMnemonico WHERE CodigoNemonico = @CodNemonicoACT
			UPDATE DividendosRebatesLiberadas SET CodigoNemonico = @p_CodigoMnemonico WHERE CodigoNemonico = @CodNemonicoACT
			UPDATE AumentoCapitalDetalle SET CodigoMnemonico = @p_CodigoMnemonico WHERE CodigoMnemonico = @CodNemonicoACT
		END
		ELSE
		BEGIN
			--Se actualiza el ISIN y se toma como llave el Nemonico
			DECLARE @CodISINACT VARCHAR(20)
			SELECT @CodISINACT = CodigoISIN FROM Valores WHERE CodigoNemonico = @p_CodigoMnemonico
			UPDATE Valores SET CodigoISIN  = @p_CodigoIsin WHERE CodigoISIN = @CodISINACT
			UPDATE OrdenInversion SET CodigoISIN = @p_CodigoIsin WHERE CodigoISIN = @CodISINACT
			UPDATE preMontoInversion SET CodigoISIN = @p_CodigoIsin WHERE CodigoISIN = @CodISINACT
			UPDATE ReporteVL SET CodigoValor = @p_CodigoIsin WHERE CodigoValor = @CodISINACT
			UPDATE VectorPrecioPIP SET CodigoISIN = @p_CodigoIsin WHERE CodigoISIN = @CodISINACT 
			UPDATE CustodioInformacion SET CodigoISIN = @p_CodigoIsin WHERE CodigoISIN = @CodISINACT
			UPDATE DividendosRebatesLiberadas SET CodigoISIN = @p_CodigoIsin WHERE CodigoISIN = @CodISINACT
		END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		EXECUTE pr_SIT_retornarError
	END CATCH
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ActualizaNemomicoIsin] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'FIN --- > Store Procedures'


PRINT 'INICIO - ACTUALIZAR TERCERO SCOTIA BOLSA A SCOTIABANK'
UPDATE PrevOrdenInversion SET CodigoTercero = '20100043140' WHERE CodigoPrevOrden = 18382
GO

UPDATE OrdenInversion SET CodigoTercero = '20100043140' WHERE CodigoOrden = '067101'
GO

UPDATE CuentasPorCobrarPagar SET CodigoTercero = '20100043140' WHERE CodigoOrden = '067101'
GO

UPDATE OperacionesCaja SET CodigoTerceroOrigen = '20100043140', Referencia = 'SB12SEP23-SCOTIABANK DEL PERÚ-Bonos' WHERE CodigoOperacionCaja = '067101'
GO
PRINT 'FIN - ACTUALIZAR TERCERO SCOTIA BOLSA A SCOTIABANK'


PRINT 'INICIO - ReporteGestion_ReporteConsolidado'

USE [SIT-FONDOS]
GO

IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'ReporteGestion_ReporteConsolidado')
	DROP PROCEDURE [dbo].[ReporteGestion_ReporteConsolidado]
GO

-----------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------
-- Objetivo: GENERAR REPORTE CONSOLIDADO
-----------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 04/09/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11590
-- Descripcion del cambio: Nuevo
-----------------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 27/12/2018
-- Modificado por: Carlos Rumiche
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Modificacion campos
-----------------------------------------------------------------------------------------------------------------------------
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
-- Fecha Modificación: 25/06/2019
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 12028
-- Descripcion del cambio: Cambio de fórmula FluctuacionValor = VA.PRELIM_ValorActual - VA.TIRCOM_ValorActual
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
		
		--,(VA.TIRRAZ_ValorActual - VA.TIRCOM_ValorActual) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) AS FluctuacionValor
		,(VA.PRELIM_ValorActual - VA.TIRCOM_ValorActual) * dbo.RetornarSecuenciaVectorTipoCambio(P.CodigoPortafolioSBS, V.CodigoMoneda, @p_FechaProceso) AS FluctuacionValor
		 
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
	--DEPOSITOS A PLAZO
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

PRINT 'FIN - ReporteGestion_ReporteConsolidado'

PRINT 'INICIO - GenerarReporteVL'

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenerarReporteVL]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GenerarReporteVL]
GO
------------------------------------------------------------------------------------------------------------  
--Objetivo: Genera el reporte VL  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 11/10/2016  
-- Creado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9396  
-- Descripcion del cambio: Creacion  
-----------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 20/06/2019  
-- Creado por: Junior Huallullo  
-- Nro. Orden de Trabajo: 12028  
-- Descripcion del cambio: Validacion de Ganancia Perdida  
----------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[GenerarReporteVL] @Fecha numeric(8), @p_CodigoPortafolioSBS VarChar(10)  
AS  
BEGIN  
 --Actualizamos un campo dentro del vectorprecio para soles, para el calculo del tipo de cambio  
 DELETE FROM ReporteVL WHERE Fecha = @Fecha AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS  
 EXEC sp_SIT_FormaValorizacion1 @Fecha,@p_CodigoPortafolioSBS  
 EXEC sp_SIT_FormaValorizacion2 @Fecha,@p_CodigoPortafolioSBS  
 EXEC sp_SIT_FormaValorizacion3 @Fecha,@p_CodigoPortafolioSBS  
 --DPZ Y OR  

 DECLARE @MONEDA VARCHAR(100)
 SET @MONEDA = ( SELECT dbo.RetornarDescripcionMoneda(CodigoMoneda) from Portafolio where CodigoPortafolioSBS = @p_CodigoPortafolioSBS)

 UPDATE ReporteVL SET Ganancia_Perdida = 0  
 --InteresesGanado = ROUND(Valorizacion - MontoInversion,2)  
 WHERE Fecha = @Fecha AND FormaValorizacion IN ('4','3') AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS  
 --  
 UPDATE ReporteVL SET Ganancia_Perdida = CASE WHEN DescripcionTipoInstrumento = 'FACTURAS NEGOCIABLES' THEN 
											CASE WHEN Moneda <> @MONEDA THEN
												ROUND(Valorizacion - (MontoInversion + InteresesCorrido + InteresesGanado),2)   
											ELSE 0 END
										ELSE
											ROUND(Valorizacion - (MontoInversion + InteresesCorrido + InteresesGanado),2)   
										END
 WHERE Fecha = @Fecha AND  FormaValorizacion NOT IN ('4','3') AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS  
 --Correlativo VL  
 DECLARE @FechaAnterior Numeric(8)  
 IF NOT EXISTS (SELECT 1 FROM CorrelativoVL WHERE Fecha = @Fecha)  
 BEGIN  
  INSERT INTO CorrelativoVL  
  SELECT MAX(Correlativo) + 1,@Fecha FROM CorrelativoVL WHERE FECHA < @Fecha   
 END  
END
GO

GRANT EXECUTE ON [dbo].[GenerarReporteVL] TO [rol_sit_fondos] AS [dbo]
GO
PRINT 'FIN - GenerarReporteVL'


PRINT 'INICIO - sp_SIT_FormaValorizacion1'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_FormaValorizacion1]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_FormaValorizacion1]
GO
  
-------------------------------------------------------------------------------------------------------------------------------------   
--Objetivo: Guarda los registros de reporte VL para Certificado de depósito, Papeles comerciales,   
--Acciones, Bonos y Warrants  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha Creacion: 11/10/2016  
-- Creado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9396  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 05/12/2016  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9683  
-- Descripcion del cambio: Se mostrara el monto nominal en los instruementos de tipo Opcion  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 03/01/2017  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9777  
-- Descripcion del cambio: Se calcula el monto nominal en la moneda del fondo  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 26/07/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10599  
-- Descripcion del cambio: Cálculo de amortización  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 25/09/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10813  
-- Descripcion del cambio: Cálculo de amortización  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 21/03/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11169  
-- Descripcion del cambio: Colocar cero si el valor de la TIR es negativo  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 06/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11432  
-- Descripcion del cambio: Filtrar vector precio por su tipo de fuente => Manual o PIP  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 16/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11512  
-- Descripcion del cambio: Reemplazar tabla VectorPrecioPIP por VectorPrecio  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 31/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del tipo de cambio  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 31/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 16/11/2018    
-- Modificado por: Ricardo Colonia    
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del tipo de cambio en fn_TipoCambioVL  
-------------------------------------------------------------------------------------------------------------------------------------    
-- Fecha modificacion: 15/01/2019  
-- Modificado por: Ernesto Galarza  
-- Nro. Orden de Trabajo: 11636  
-- Descripcion del cambio: Se modifico para que contemple las ordenes que pertecen a la clase facturas negociables  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha modificacion: 13/03/2019  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11908  
-- Descripcion del cambio: Se elimina restricción de generación de interesescorridocompra cuando la orden es CD  
-------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha Modificación: 12/04/2019      
-- Modificado por: Ernesto Galarza      
-- Nro. Orden de Trabajo: 11851     
-- Descripcion del cambio: Se modifico el redondeo, en monto nominal de 2 decimales a 6  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 20/06/2019      
-- Modificado por: Junior Huallullo
-- Nro. Orden de Trabajo: 12028     
-- Descripcion del cambio: Se modifico le interes ganado de las facturas negociables
---------------------------------------------------------------------------------------------------------------------    
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
  MontoNominal = CASE WHEN  CI.Categoria = 'AC' OR  CI.Categoria = 'FM' OR CI.Categoria = 'FI' THEN ROUND(CTV.Cantidad,6)   
       WHEN CI.Categoria = 'WA' THEN (CASE WHEN M.TipoCalculo = 'D' THEN V.ValorNominal * VTCo.ValorPrimario ELSE V.ValorNominal / VTCo.ValorPrimario END) --9683  
       WHEN CI.Categoria = 'FA' THEN (SELECT MontoNominal FROM PrevOrdenInversion where CodigoNemonico = V.CodigoNemonico) --11636  
      ELSE ROUND((ctv.Cantidad * V.ValorUnitario * (dbo.CalcularSumaAmortizacionPendiente(ctv.FechaValoracion,V.CodigoNemonico)/100) ),6) END, ----9  
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
   WHEN (CI.Categoria = 'FA') THEN ROUND(CTV.VPNLocal,2,0) - ROUND(DBO.fn_MontoInversionVL(CTV.FechaValoracion, P.CodigoPortafolioSBS ,V.CodigoISIN),2)
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
  V.CodigoNemonico ,TI.Descripcion,V.CodigoEmisor, P.CodigoPortafolioSBS ,1, LEFT(M.Descripcion  ,20)  Descripcion
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
PRINT 'FIN - sp_SIT_FormaValorizacion1'

PRINT 'INICIO - Actualización descripción Cartas Divisas'

UPDATE ModeloCarta
SET Descripcion = 'COMPRA DE DIVISAS'
WHERE CodigoModelo = 'CDIV'
GO

UPDATE ModeloCarta
SET Descripcion = 'VENTA DE DIVISAS'
WHERE CodigoModelo = 'VDIV'
GO

PRINT 'FIN - Actualización descripción Cartas Divisas'




PRINT 'INICIO - sp_SIT_Gen_Carta_OperacionCambio'

USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'sp_SIT_Gen_Carta_OperacionCambio')
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
--	Fecha Creacion: 26/06/2019
--	Craedo por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Agregar los campos "BancoGlosa" y "BancoGlosaDestino"
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
	--BancoMatrizOrigen = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizOrigen) ,'') ,
	BancoMatrizOrigen = dbo.RetornarExisteCuentaEconomicaTercero(OC.CodigoPortafolioSBS,OC.CodigoTerceroDestino,OI.CodigoMoneda),
	SimboloMoneda = M.Simbolo,
	Importe = ROUND(OC.Importe,2), 
	SimboloCuenta = CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END, 
	ce.NumeroCuentaInterBancario ,
    OC.NumeroCuenta, 
	TB.Descripcion AS Banco, 
	ISNULL(OC.CodigoOrden,OC.CodigoOperacionCaja) NumeroOrden,
	--BancoMatrizDestino = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoMatrizDestino) ,'') ,
	BancoMatrizDestino = dbo.RetornarExisteCuentaEconomicaTercero(OC.CodigoPortafolioSBS,OC.BancoGlosaDestino,OI.codigoMonedaDestino),
    BancoDestino  = ISNULL( (SELECT BD.Descripcion FROM Terceros BD WHERE BD.CodigoTercero = ED.CodigoTercero) ,''),
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
	OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2,OC.ObservacionCarta,OC.ObservacionCartaDestino,
	BancoGlosa = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.CodigoTerceroDestino) ,''),
	BancoGlosaDestino = ISNULL( (SELECT BM.Descripcion FROM Terceros BM WHERE BM.CodigoTercero =  OC.BancoGlosaDestino) ,'')
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

PRINT 'FIN - sp_SIT_Gen_Carta_OperacionCambio'


PRINT 'INICIO - [dbo].[CierreCajas_ValidarFechaIngresoPagoComision]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CierreCajas_ValidarFechaIngresoPagoComision]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CierreCajas_ValidarFechaIngresoPagoComision]
GO
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Validar la fecha de ingreso pendiente de pago de comisión con fecha de cierre de caja
-------------------------------------------------------------------------------------------------------------------------
--	Fecha creación: 09/07/2019
--	Creado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 12028
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CierreCajas_ValidarFechaIngresoPagoComision] 
	@p_CodigoPortafolioSBS VARCHAR(20),  
	@p_CodigoClaseCuenta VARCHAR(3),
	@p_fechaActualCaja DECIMAL(8)
AS  
BEGIN
	DECLARE
		@FechaSolicitud DECIMAL(8) =( 
									 SELECT 
									 	ISNULL(COUNT(PFC.FechaSolicitud),0)
									 FROM
									 	PagoFechaComision PFC
									 JOIN
									 	CuentaEconomica CE ON PFC.CodigoBanco = CE.EntidadFinanciera
									 						  AND PFC.CodigoPortafolioSBS = CE.CodigoPortafolioSBS
									 						  AND PFC.NumeroCuenta = CE.NumeroCuenta
									 JOIN
									 	ClaseCuenta CC ON CC.CodigoClaseCuenta = CE.CodigoClaseCuenta
									 WHERE
									 	PFC.Estado = 'ING'
									 	AND PFC.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
									 	AND CC.CodigoClaseCuenta = @p_CodigoClaseCuenta
									 	AND PFC.FechaSolicitud <= @p_fechaActualCaja
									)

	SELECT ISNULL(@FechaSolicitud,0) CantidadRegistroPendientePagoComision
END
GO

GRANT EXECUTE ON [dbo].[CierreCajas_ValidarFechaIngresoPagoComision] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN - [dbo].[CierreCajas_ValidarFechaIngresoPagoComision]'


PRINT 'INICIO - [dbo].[pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta]'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta]
GO

-----------------------------------------------------------------------------------------------------------
--Objetivo: Seleccionar cartas de instrucción
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 03/10/2017
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Seleccionar cartas de instrucción
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta]
(
	@p_CodigoInterno VARCHAR(7) = ''
	,@p_Rol VARCHAR(1) = ''
	,@p_Situacion VARCHAR(1) = ''
)
AS
BEGIN
SET NOCOUNT ON
	SELECT
		 ac.CodigoInterno
		,Nombre = dbo.RetornarNombrePersonal(ac.CodigoInterno)
		,Cargo = dbo.RetornarCargoPersonal(ac.CodigoInterno)
		,DescripcionRol = rl.Nombre
		,ac.Rol
		,Firma = ISNULL(ac.Firma,'')
		,DescripcionSituacion = ISNULL(st.Nombre,'')
		,ac.Situacion
		,CodigoUsuario = ISNULL(p.CodigoUsuario,'')
		,email_trabajo = ISNULL(p.email_trabajo,'')
		,email_personal = ISNULL(p.email_personal,'')
		,ac.TipoFirmante
	FROM AprobadorCarta ac
		INNER JOIN Personal p ON p.CodigoInterno = ac.CodigoInterno
		LEFT OUTER JOIN ParametrosGenerales st ON st.Valor = ac.Situacion AND st.Clasificacion = 'Situación'
		LEFT OUTER JOIN ParametrosGenerales rl ON rl.Valor = ac.Rol AND rl.Clasificacion = 'ROL_CARTA'
	WHERE ac.Rol = (CASE WHEN LEN(@p_Rol) = 0 THEN ac.Rol ELSE @p_Rol END) AND ac.Situacion = (CASE WHEN LEN(@p_Situacion) = 0 THEN ac.Situacion ELSE @p_Situacion END)
		AND ac.CodigoInterno = (CASE WHEN LEN(@p_CodigoInterno) = 0 THEN ac.CodigoInterno ELSE @p_CodigoInterno END)
	ORDER BY Rol
END
GO

GRANT EXECUTE ON [dbo].[pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN - [dbo].[pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta]'

PRINT 'INICIO - Creacion de fechas (periodo) inicial para la fecha de comision. Fecha corte 30 junio'
INSERT INTO [dbo].[PagoFechaComision]
           ([CodigoPortafolioSBS]
           ,[CodigoBanco]
           ,[NumeroCuenta]
           ,[CodigoBancoAdministradora]
           ,[NumeroCuentaAdministradora]
           ,[FechaInicio]
           ,[FechaFin]
           ,[FechaSolicitud]
           ,[FechaConfirmacion]
           ,[ComisionAcumulada]
           ,[SaldoDisponible]
           ,[CodigoOperacionCaja]
           ,[UsuarioSolicitud]
           ,[HoraCreacion]
           ,[FechaCreacion]
           ,[UsuarioCreacion]
           ,[HoraModIFicacion]
           ,[FechaModIFicacion]
           ,[UsuarioModIFicacion]
           ,[Estado])
   SELECT VC.CodigoPortafolioSBS
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,20190601
           ,20190630
           ,20190630
           ,0
           ,0
           ,0
           ,NULL
           ,'SYSTEM'
           ,NULL
           ,NULL
           ,'SYSTEM'
           ,NULL
           ,NULL
           ,NULL
           ,'CON'
FROM 
 ValorCuota  VC WITH (NOLOCK)
 inner join Portafolio PO on PO.CodigoPortafolioSBS=VC.CodigoPortafolioSBS and PO.Situacion='A'
 group by 
 VC.CodigoPortafolioSBS

PRINT 'FIN - Creacion de fechas (periodo) inicial para la fecha de comision. Fecha corte 30 junio'

PRINT 'INICIO - ACTUALIZAR DATO DE IMAGEN FIRMA EN AprobadorCarta'

-- RUTA CENTRAL COMPARTIDA DE LAS IMAGENES DE FIRMA DE CARTAS
update A set Comentario = '\\Sppeapp00070\cartas\Imagenes\' from [ParametrosGenerales] A where Clasificacion = 'RUTAFIRMA'

-- AJUSTE DE FIRMA EN AprobadorCarta
update A set Firma = 'P500088.jpg' from AprobadorCarta A where CodigoInterno = 'P500088'
update A set Firma = 'P500091.jpg' from AprobadorCarta A where CodigoInterno = 'P500091'
update A set Firma = 'P500110.jpg' from AprobadorCarta A where CodigoInterno = 'P500110'
update A set Firma = 'P500161.jpg' from AprobadorCarta A where CodigoInterno = 'P500161'
update A set Firma = 'P500240.jpg' from AprobadorCarta A where CodigoInterno = 'P500240'
update A set Firma = 'P500273.jpg' from AprobadorCarta A where CodigoInterno = 'P500273'
update A set Firma = 'P500569.jpg' from AprobadorCarta A where CodigoInterno = 'P500569'
update A set Firma = 'P500611.jpg' from AprobadorCarta A where CodigoInterno = 'P500611'
update A set Firma = 'P500623.jpg' from AprobadorCarta A where CodigoInterno = 'P500623'
update A set Firma = 'P500629.jpg' from AprobadorCarta A where CodigoInterno = 'P500629'
update A set Firma = 'P500652.jpg' from AprobadorCarta A where CodigoInterno = 'P500652'
update A set Firma = 'P500678.jpg' from AprobadorCarta A where CodigoInterno = 'P500678'
update A set Firma = 'P500685.jpg' from AprobadorCarta A where CodigoInterno = 'P500685'
update A set Firma = 'P500709.jpg' from AprobadorCarta A where CodigoInterno = 'P500709'
update A set Firma = 'P500781.jpg' from AprobadorCarta A where CodigoInterno = 'P500781'
update A set Firma = 'P900011.jpg' from AprobadorCarta A where CodigoInterno = 'P900011'
GO
PRINT 'FIN - ACTUALIZAR DATO DE IMAGEN FIRMA EN AprobadorCarta'


PRINT 'INICIO - ACTUALIZAR TASA CUPÓN - FCONF1CD6E'
UPDATE Valores SET TasaCupon = 0 WHERE CodigoNemonico = 'FCONF1CD6E'
GO
PRINT 'FIN - ACTUALIZAR TASA CUPÓN - FCONF1CD6E'



delete from PortafolioCaja where CodigoClaseCuenta='10'
INSERT INTO PortafolioCaja (CodigoPortafolioSBS,
CodigoClaseCuenta,
FechaCajaOperaciones)
SELECT CodigoPortafolioSBS,'10',FechaCajaOperaciones  FROM Portafolio 
WHERE CodigoPortafolioSBS IN (
select  CodigoPortafolioSBS   from CuentaEconomica where CodigoClaseCuenta='10'  and Situacion='A'
and CodigoPortafolioSBS in (select CodigoPortafolioSBS from Portafolio where situacion='A')
group by CodigoPortafolioSBS
)



delete from PortafolioCaja where CodigoClaseCuenta='20'
INSERT INTO PortafolioCaja (CodigoPortafolioSBS,
CodigoClaseCuenta,
FechaCajaOperaciones)
SELECT CodigoPortafolioSBS,'20',FechaCajaOperaciones  FROM Portafolio 
WHERE CodigoPortafolioSBS IN (
select  CodigoPortafolioSBS   from CuentaEconomica where CodigoClaseCuenta='20'  and Situacion='A'
and CodigoPortafolioSBS in (select CodigoPortafolioSBS from Portafolio where situacion='A')
group by CodigoPortafolioSBS
)



PRINT 'Inicio - RetornarExisteCuentaEconomicaTercero'

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
--	Fecha Creacion: 10/06/2019
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12028
--	Descripcion del cambio: Se filtran las cuentas bancarias activas y de tipo de inversión.
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

	SET @codigoEntidad = (SELECT CodigoEntidad FROM Entidad WHERE CodigoTercero = @p_CodigoTercero and Situacion = 'A')
	SELECT @thereIs = 'S' FROM CuentaEconomica
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND CodigoMoneda = @p_CodigoMoneda AND EntidadFinanciera = @codigoEntidad
		AND Situacion = 'A' AND CodigoClaseCuenta = '20'

	RETURN ISNULL(@thereIs,'N')
END
GO

GRANT EXECUTE ON [dbo].[RetornarExisteCuentaEconomicaTercero] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - RetornarExisteCuentaEconomicaTercero'

PRINT 'Ini - sp_SIT_GenerarCuponera_Fechas'
USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_GenerarCuponera_Fechas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_SIT_GenerarCuponera_Fechas]
GO


----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Generar Cuponera SWAP opción automático  
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 05/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 04/03/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11851    
-- Descripción del cambio: Se habilita para considerar la fecha primer cupon.    
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 24/07/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 12028    
-- Descripción del cambio: Se cambia función dbo.dias por fn_SIT_obt_Diferencia_Dias para calcular los dias reales en base ACT
----------------------------------------------------------------------------------------------------------------------------------------  
--EXEC sp_SIT_GenerarCuponera_Fechas '6', 20160427,20190427,20161027,6.40625,'360',0,'5','ACT'
--EXEC sp_SIT_GenerarCuponera_Fechas '6', 20190301,20240301,20190501,5,'360',0,'5','30'
CREATE PROCEDURE [dbo].[sp_SIT_GenerarCuponera_Fechas](
	@p_CodigoTipoAmortizacion VARCHAR(4),
	@p_fechaEmision NUMERIC(8),
	@p_fechaVcto NUMERIC(8),
	@p_fechaPriCupon NUMERIC(8),
	@p_tasaCupon NUMERIC(22,7),
	@p_baseCupon VARCHAR(4),
	@p_tasaSpread NUMERIC(15,12),
	@p_CodigoPeriodicidad VARCHAR(4),
	@p_numeroDias VARCHAR(4)
)          
AS          
BEGIN 
	DECLARE	
		@p_fecha NUMERIC(8),
		@p_fechaD NUMERIC(8),
		@p_SfechaD VARCHAR(2),
		@p_fechaM VARCHAR(8),
		@p_SfechaM VARCHAR(2),
		@p_fechaA NUMERIC(8),
		@p_fechaIni NUMERIC(8),
		@p_fechaFin NUMERIC(8),
		@p_contFecha NUMERIC(4),
		@p_NroMeses NUMERIC(8),
		@p_NroDias NUMERIC(4),
		@p_NroDiasTranscurridos NUMERIC(20),
		@p_NroCuotasAmortizacion INT,
		@p_NroDiasAmortizacion NUMERIC(5),
		@p_AmortizacionAux NUMERIC(22,15),
		@p_Amortizacion NUMERIC(22,15),
		@p_fechaFinAmortizacion DECIMAL(8),
		@p_TotalAmortizacion NUMERIC(22,15),
		@p_fechaReal NUMERIC(8),
		@p_fechaRealInicial NUMERIC(8),
		@p_NroDiasXMes NUMERIC(8),
		@iCount INT, 
		@DiasPeriodo NUMERIC(8)

	SET @DiasPeriodo = 0
	
	SELECT 
		@DiasPeriodo = DiasPeriodo 
	FROM 
		Periodicidad 
	WHERE 
		CodigoPeriodicidad = @p_CodigoPeriodicidad
		
	CREATE TABLE 
		#tmpCuponera
				(
				 Consecutivo INT IDENTITY(1,1),
				 FechaIni NUMERIC(8),
				 FechaFin NUMERIC(8),
				 Amortizac NUMERIC(22,7),
				 DifDias NUMERIC(8),
				 TasaCupon NUMERIC(22,12),
				 BaseCupon VARCHAR(4),
				 DiasPago VARCHAR(4),
				 fechaRealInicial NUMERIC(8),
				 fechaRealFinal NUMERIC(8) ,
				 AmortizacConsolidado NUMERIC(22,7),
				 MontoInteres NUMERIC(22,7),
				 MontoAmortizacion NUMERIC(22,7),
				 NominalRestante NUMERIC(22,7)
				)
	IF (@DiasPeriodo >= 30) 
		SELECT 
			@p_NroMeses = @DiasPeriodo/30,
			@p_NroDias = 0
	ELSE 
		SELECT	
			@p_NroMeses = 0,
			@p_NroDias = @DiasPeriodo

	SELECT	
		@p_fecha = @p_fechaEmision,
		@p_fechaRealInicial = @p_fechaEmision,
		@p_TotalAmortizacion = 0,
		@p_NroDiasTranscurridos = 0,
		@iCount = 1
		
	WHILE (@p_fecha+@DiasPeriodo) <= @p_fechaVcto
	BEGIN
		SELECT	
			@p_NroDiasTranscurridos=@p_NroDiasTranscurridos+@DiasPeriodo,
			@p_fechaIni = @p_fecha,
			@p_fechaD = CONVERT(NUMERIC(8),SUBSTRING(CONVERT(VARCHAR(8),@p_fecha),7,2)),
			@p_fechaM = CONVERT(NUMERIC(8),SUBSTRING(CONVERT(VARCHAR(8),@p_fecha),5,2)),
			@p_fechaA = CONVERT(NUMERIC(8),SUBSTRING(CONVERT(VARCHAR(8),@p_fecha),1,4))
			
		IF (@p_NroMeses <> 0)
		BEGIN
			SET @p_fechaM = @p_fechaM + @p_NroMeses
			
			IF (@p_fechaM > 12)
				SELECT 
					@p_fechaA = @p_fechaA + 1,
					@p_fechaM = @p_fechaM - 12
			
			SET @p_NroDiasXMes = dbo.RetornarCantidadDias(@p_fechaM,@p_fechaA)
			
			IF (@p_fechaD > @p_NroDiasXMes)
				SET @p_fechaD = @p_NroDiasXMes
		END
		ELSE
		BEGIN
			-- para q en caso sea menos de un mes
			SET @p_fechaD = @p_fechaD + @p_NroDias
			SET @p_NroDiasXMes = dbo.RetornarCantidadDias(@p_fechaM,@p_fechaA)
			IF (@p_fechaD > @p_NroDiasXMes)
			BEGIN
				SET @p_fechaD = @p_fechaD - @p_NroDiasXMes
				SET @p_fechaM = @p_fechaM + 1
				IF (@p_fechaM > 12)
				BEGIN
					SET @p_fechaM = 1
					SET @p_fechaA = @p_fechaA + 1
				END
			END
		END
		
		IF (LEN(CONVERT(VARCHAR(2),@p_fechaM)) = 1)
			SET @p_SfechaM = '0' + CONVERT(VARCHAR(1),@p_fechaM)
		ELSE
			SET @p_SfechaM = CONVERT(VARCHAR(2),@p_fechaM)
			
		IF (LEN(CONVERT(VARCHAR(2),@p_fechaD)) = 1) 
			SET @p_SfechaD = '0' + CONVERT(VARCHAR(1), @p_fechaD)
		ELSE 
			SET @p_SfechaD = CONVERT(VARCHAR(2), @p_fechaD)
		
		IF (@p_fecha = @p_fechaEmision AND @iCount = 1 AND @p_fechaPriCupon <> @p_fechaEmision)  
			SET @p_fechaFin = @p_fechaPriCupon  
		ELSE  
			SET @p_fechaFin = CONVERT(NUMERIC(8),CONVERT(VARCHAR(4),@p_fechaA)+@p_SfechaM+@p_SfechaD)  
		--SET @p_fechaFin = CONVERT(NUMERIC(8), CONVERT(VARCHAR(4), @p_fechaA) + @p_SfechaM + @p_SfechaD)
		SET @p_fecha = @p_fechaFin
		SET @p_fechaReal = dbo.RetornarFechaProxima(@p_fechaRealInicial, @DiasPeriodo)
		
		PRINT 'ENTRO AQUI PUNTO 3  @p_fecha = ' + CAST(@p_fecha AS VARCHAR) 
		PRINT '(@p_fecha+@DiasPeriodo) <= @p_fechaVcto = ' +  CAST(@p_fecha+@DiasPeriodo AS VARCHAR) + ' Y ' + CAST(@p_fechaVcto AS VARCHAR) 
		
		SET @p_NroDiasAmortizacion = (SELECT 
										  NumeroDias 
									  FROM 
										  TipoAmortizacion 
									  WHERE 
										  CodigoTipoAmortizacion=@p_CodigoTipoAmortizacion)
		IF (@p_NroDiasAmortizacion = 0)
			SET @p_NroDiasAmortizacion = 1
			
		IF CONVERT(INT,DATEDIFF(DAY,CAST(@p_fechaEmision AS VARCHAR(12)),CAST(@p_fechaVcto AS VARCHAR(12))) % CONVERT(INT, @p_NroDiasAmortizacion)) = 0
			SET @p_NroCuotasAmortizacion = CONVERT(INT,DATEDIFF(DAY,CAST(@p_fechaEmision AS VARCHAR(12)),CAST(@p_fechaVcto AS VARCHAR(12)))/@p_NroDiasAmortizacion)
		ELSE
			SET @p_NroCuotasAmortizacion = CONVERT(INT,DATEDIFF(DAY,CAST(@p_fechaEmision AS VARCHAR(12)),CAST(@p_fechaVcto AS VARCHAR(12)))/@p_NroDiasAmortizacion) + 2
			
		IF ((SELECT NumeroDias FROM TipoAmortizacion WHERE CodigoTipoAmortizacion = @p_CodigoTipoAmortizacion) = 0)
		BEGIN
			IF (@p_fecha+@DiasPeriodo) > @p_fechaVcto
				SET @p_Amortizacion = 100
			ELSE
			BEGIN
				IF (@p_Fecha = @p_fechaVcto)
					SET @p_Amortizacion = 100
				ELSE
					SET @p_Amortizacion = 0
			END
		END
		ELSE
		BEGIN
			IF (@p_NroCuotasAmortizacion=0)
				SET @p_AmortizacionAux = 100
			ELSE
			BEGIN
				IF DATEDIFF(DAY,CAST(@p_fechaEmision AS VARCHAR(12)),CAST(@p_fechaVcto AS VARCHAR(12))) % CONVERT(INT,@p_NroDiasAmortizacion) = 0
					SET @p_AmortizacionAux = 100/@p_NroCuotasAmortizacion
				ELSE
					SET @p_AmortizacionAux = convert(NUMERIC(25,7),100)/(convert(NUMERIC(25,12),@p_NroCuotasAmortizacion))
					
				print @p_NroCuotasAmortizacion
				print @p_AmortizacionAux
			END
			IF CAST(@p_NroDiasTranscurridos AS INT) % CAST(@p_NroDiasAmortizacion AS INT)=0
			BEGIN
				SET @p_FechaFinAmortizacion = CAST(CAST(YEAR(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(4))+
				CASE WHEN LEN(CAST(MONTH(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(2)))=1 THEN
					'0'+CAST(MONTH(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(2)) 
				ELSE
					CAST(MONTH(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(2)) END +
				CASE WHEN LEN(CAST(DAY(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(2)))=1 THEN
					'0'+CAST(DAY(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(2)) 
				ELSE
					CAST(DAY(DATEADD(DAY,@p_NroDiasAmortizacion,CAST(@p_fecha AS VARCHAR(12))))AS VARCHAR(2)) END AS NUMERIC)
					
				print @p_FechaFinAmortizacion
				
				IF @p_FechaFinAmortizacion > CONVERT(NUMERIC(8), CONVERT(VARCHAR(12), DATEADD(DAY, CAST(@p_NroDiasAmortizacion AS INT), CAST(@p_fechaVcto AS VARCHAR(12))), 112))
					SET @p_Amortizacion = ROUND(100 - @p_TotalAmortizacion,7)
				ELSE
				BEGIN
					IF (@p_Fecha = @p_fechaVcto)
						SET @p_Amortizacion = ROUND(100 - @p_TotalAmortizacion,7)
					ELSE
					BEGIN
						SET @p_Amortizacion = ROUND(@p_AmortizacionAux,7)
						SET @p_TotalAmortizacion = ROUND(@p_TotalAmortizacion + @p_AmortizacionAux,7)
					END
				END
			END
			ELSE
				SET @p_Amortizacion = 0
		END
		IF (@p_fechaPriCupon = @p_fechaEmision and @iCount = 1)
			print 'PRIMER CUPON IGUAL FECHA EMISION'
		ELSE
			DECLARE @Diferencia INT
		IF @p_numeroDias = '30'
			SELECT @Diferencia= dbo.dias360(@p_fechaIni,@p_fechaFin)
		ELSE
			SELECT @Diferencia= dbo.fn_SIT_obt_Diferencia_Dias(@p_fechaIni,@p_fechaFin)

		BEGIN
			INSERT INTO 
				#tmpCuponera
			SELECT	
				@p_fechaIni AS FechaIni, 
				@p_fechaFin AS FechaFin,
				0 AS Amortizac,
				@Diferencia AS DifDias,
				--CASE WHEN @iCount = 1 THEN (@p_tasaCupon + @p_tasaSpread) ELSE @p_tasaCupon END AS TasaCupon,
				(@p_tasaCupon + @p_tasaSpread) ,
			    (CASE WHEN @p_baseCupon = 'ACT' THEN 
												(CASE WHEN dbo.SIT_fn_EsAnioBisiesto(LEFT(@p_fechaFin,4))= 1 THEN '366' ELSE '365' END)
												ELSE @p_baseCupon END) AS BaseCupon,
				@DiasPeriodo DiasPago,
				@p_fechaRealInicial fechaRealInicial,
				@p_fechaReal fechaRealFinal,
				0 AmortizacConsolidado,
				0,
				0,
				0
				
			SET @p_fechaRealInicial = @p_fechaReal
		END
		SET @iCount = @iCount + 1
	END

	UPDATE 
		#tmpCuponera 
	SET 
		FechaFin = @p_fechaVcto,
		DifDias = (CASE WHEN @p_numeroDias = '30' THEN dbo.dias360(FechaIni,@p_fechaVcto) 
					    ELSE dbo.fn_SIT_obt_Diferencia_Dias(FechaIni,@p_fechaVcto) END)
	WHERE FechaFin = (SELECT MAX( FechaFin) FROM #tmpCuponera)
	
	SELECT 	
		Consecutivo,
		FechaIni,
		FechaFin,
		DifDias,
		Amortizac,
		TasaCupon,
		BaseCupon,
		DiasPago,
		fechaRealInicial,
		fechaRealFinal,
		AmortizacConsolidado,
		MontoInteres,
		MontoAmortizacion,
		NominalRestante
	FROM 
		#tmpCuponera
		
	DROP TABLE #tmpCuponera

END



GO

GRANT EXECUTE ON [dbo].[sp_SIT_GenerarCuponera_Fechas] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - sp_SIT_GenerarCuponera_Fechas'

PRINT 'Ini - Amortizar_CuponeraNormal'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Amortizar_CuponeraNormal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Amortizar_CuponeraNormal]
GO

-----------------------------------------------------------------------------------------------------------
-- OBJETIVO: OBTENER LA AMORTIZACION, EL INTERES Y EL SALDO DE LOS CUPONES
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 25/02/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Creacion
---------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Modificación: 24/07/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 12028    
-- Descripción del cambio: Se cambia función dbo.dias por fn_SIT_obt_Diferencia_Dias para calcular los dias reales en base ACT
----------------------------------------------------------------------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[Amortizar_CuponeraNormal](
	@p_TablaCuponera [dbo].[UT_CuponeraTemporal] READONLY,
	@p_fechaINI DECIMAL(8),
	@p_fechaFIN DECIMAL(8),
	@p_CodigoTipoAmortizacion VARCHAR(4),
	@p_baseCupon VARCHAR(4),
	@p_tasaCupon NUMERIC(22,7),
	@p_tasaSpread NUMERIC(15,12),
	@p_CodigoPeriodicidad VARCHAR(4),
	@p_MontoNominal NUMERIC(22,7),
	@p_numeroDias VARCHAR(4),
	@p_flagCuponesAnteriores CHAR(1),
	@p_AmortizacionAnterior NUMERIC(22,7) = 0
) 
AS          
BEGIN 
	DECLARE
		@tmpCuponera UT_CuponeraTemporal
	INSERT INTO @tmpCuponera
	SELECT * FROM @p_TablaCuponera
	DECLARE
		@sumCuponesMenoUltimoCupon NUMERIC(22,7)
	IF @p_flagCuponesAnteriores = '1'  -- >> Calcular Cupones por procesar.
	BEGIN

		-- >> Asignar Amortizaciones
		DECLARE 
			@NroDiasAmortizacion INT,
			@iCount INT,
			@DiasPeriodo NUMERIC(3) = 0
			
		SELECT 
			@DiasPeriodo = DiasPeriodo 
		FROM 
			Periodicidad 
		WHERE 
			CodigoPeriodicidad = @p_CodigoPeriodicidad
		
		SELECT 
		   @NroDiasAmortizacion = NumeroDias 
		FROM 
		   TipoAmortizacion 
		WHERE 
		   CodigoTipoAmortizacion=@p_CodigoTipoAmortizacion
									
		IF (@NroDiasAmortizacion = 0) 
			SET @NroDiasAmortizacion = 1
			
		IF @NroDiasAmortizacion = 1
		BEGIN
			UPDATE 
				 @tmpCuponera 
			SET 
				 Amortizac = CONVERT(NUMERIC(22,7),0)
				 
			UPDATE 
				@tmpCuponera 
			SET 
				Amortizac = CONVERT(NUMERIC(22,7),100) 
			WHERE FechaFin = (SELECT MAX(FechaFin) FROM @tmpCuponera)
		END
		ELSE
		BEGIN
			DECLARE 
				@Diferencia INT,
				@numCuponesAmort INT,
				@FechaFinAmortizacion NUMERIC(8),
				@porcentaAmort NUMERIC(22,7),
				@i INT,
				@fechaTemp NUMERIC(8),
				@fechaTempRealINI NUMERIC(8),
				@fechaTempRealFIN NUMERIC(8),
				@secuencia INT,
				@diasPago INT

			DECLARE @cantidadRegistosTabla INT = (SELECT  COUNT(*) FROM @tmpCuponera),
					@cantidadAmortizaciones INT,
					@distanciaCuponAmortizado INT = 1
					
			SET @i = 1
			IF @p_baseCupon = '360'
				SELECT @Diferencia= dbo.dias360(@p_fechaIni,@p_fechaFin)
			ELSE
				SELECT @Diferencia= dbo.fn_SIT_obt_Diferencia_Dias(@p_fechaIni,@p_fechaFin)

			IF @DiasPeriodo= 0 
				BEGIN
					SET @cantidadAmortizaciones =1
					SET @numCuponesAmort =1
				END
			ELSE
				BEGIN
					SET @cantidadAmortizaciones = (@cantidadRegistosTabla/(@NroDiasAmortizacion/@DiasPeriodo))
					SET @numCuponesAmort = @cantidadAmortizaciones + (CASE WHEN (@cantidadRegistosTabla%(@NroDiasAmortizacion/@DiasPeriodo)) > 0 THEN 1 ELSE 0 END)
					SET @distanciaCuponAmortizado = (@NroDiasAmortizacion/@DiasPeriodo)
				END
			
			SET @porcentaAmort = CONVERT(NUMERIC(22,7),(100-@p_AmortizacionAnterior)) / CONVERT(NUMERIC(22,7),@numCuponesAmort)

			WHILE @i < @numCuponesAmort 
			BEGIN
				--SET @p_fechaFin = dbo.RetornarFechaProxima360(@p_fechaINI,@NroDiasAmortizacion)
				SET @p_fechaFin = (SELECT FechaFin FROM @tmpCuponera WHERE Consecutivo=(@i*@distanciaCuponAmortizado))
				print('@i: '+ CONVERT(varchar,@i))
				print('@p_fechaFin: '+ CONVERT(varchar,@p_fechaFin))
				IF EXISTS(SELECT 1 FROM @tmpCuponera WHERE FechaFin = @p_fechaFIN) 
				BEGIN
				--cuando base dias es 360
					UPDATE 
						@tmpCuponera
					SET
						Amortizac = @porcentaAmort
					WHERE
						FechaFin = @p_fechaFIN
						AND FechaFin <> (SELECT MAX(FechaFin) FROM @tmpCuponera)
				END
				ELSE
				BEGIN
				--cuando base dias es ACT 
					SELECT TOP 1 @fechaTemp = FechaFin,
								 @fechaTempRealINI = fechaRealFinal
					FROM @tmpCuponera
					WHERE FechaFin < @p_fechaFin
					ORDER BY FechaFin DESC
					
					SET @fechaTemp = ISNULL(@fechaTemp,@p_fechaINI)
					SET @fechaTempRealINI =  ISNULL(@fechaTempRealINI,@p_fechaINI)
					
					IF @p_numeroDias = '30'
						SELECT @Diferencia= dbo.dias360(@fechaTemp,@p_fechaFin)
					ELSE
						SELECT @Diferencia= dbo.fn_SIT_obt_Diferencia_Dias(@fechaTemp,@p_fechaFin)
					
					SET @diasPago = dbo.dias360(@fechaTemp,@p_fechaFin)	
					SET @fechaTempRealFIN = dbo.RetornarFechaProxima(@fechaTempRealINI, @Diferencia)
					
					INSERT INTO @tmpCuponera (FechaIni ,
											  FechaFin ,
											  DifDias ,
											  Amortizac ,
											  TasaCupon ,
											  BaseCupon ,
											  DiasPago,
											  fechaRealInicial ,
											  fechaRealFinal,
											  AmortizacConsolidado ,
											  MontoInteres ,
											  MontoAmortizacion ,
											  NominalRestante)
							VALUES (@fechaTemp,
									@p_fechaFin,
									@Diferencia,
									@porcentaAmort,
									--CASE WHEN @i=1 then @p_tasaCupon+@p_tasaSpread ELSE @p_tasaCupon END, 
									@p_tasaCupon+@p_tasaSpread,
									(CASE WHEN @p_baseCupon = 'ACT' THEN 
																		(CASE WHEN dbo.SIT_fn_EsAnioBisiesto(LEFT(@p_fechaFin,4))= 1 THEN '366' ELSE '365' END)
																		 ELSE @p_baseCupon END),
									@diasPago,
									@fechaTempRealINI,
									@fechaTempRealFIN,
									0,
									0,
									0,
									0)
						
					UPDATE 
						@tmpCuponera
					SET
						FechaIni = @p_fechaFin,
						fechaRealInicial = @fechaTempRealFIN,
						DifDias = (CASE WHEN @p_numeroDias = '30' THEN dbo.dias360(@p_fechaFin,FechaFin) 
									    ELSE dbo.fn_SIT_obt_Diferencia_Dias(@p_fechaFin,FechaFin) END),
						DiasPago = dbo.dias360(@p_fechaFin,FechaFin)
					WHERE 
						FechaFin IN (SELECT MIN(FechaFin) FROM @tmpCuponera WHERE FechaFin > @p_fechaFin)
				END
				SET @p_fechaIni = @p_fechaFIN
				SET @i = @i + 1
			END
						
			SET @sumCuponesMenoUltimoCupon = (SELECT 
													SUM(Amortizac) 
											  FROM 
													@tmpCuponera 
											  WHERE FechaFin <> (SELECT MAX(FechaFin) FROM @tmpCuponera))
			UPDATE 
				@tmpCuponera 
			SET 
				Amortizac = (CONVERT(NUMERIC(22,7),(100 - @p_AmortizacionAnterior)) - @sumCuponesMenoUltimoCupon) 
			WHERE 
				FechaFin = (SELECT MAX(FechaFin) FROM @tmpCuponera)
	-- >> REORDENAR CONSECUTIVOS EN CUPONERA
			SET @i = 1
			SET @iCount = (SELECT COUNT(*) FROM @tmpCuponera)
			SET @p_fechaFIN = (SELECT MIN(FechaFin) FROM @tmpCuponera)
			WHILE @i <= @iCount
			BEGIN
				UPDATE
					@tmpCuponera
				SET
					Consecutivo = @i
				WHERE 
					FechaFin = @p_fechaFIN
					
				SET @p_fechaFIN = (SELECT MIN(FechaFin) FROM @tmpCuponera WHERE FechaFin > @p_fechaFIN)
				SET @i = @i + 1
			END
		END
	END

-- >> Amortizar Cuponera	
	DECLARE 
		@montototal FLOAT,
		@montorestante FLOAT,
		@saldoamortizacion FLOAT,
		@amortizacion FLOAT,
		@interes FLOAT,
		@consecutivo INT,
		@periodocontable INT,
		@periodoscalcular INT

	DECLARE 
		@sumaamortizacion FLOAT,
		@amort FLOAT, 
		@SumaMontoAmortizacion NUMERIC(22,7),
		@MontoAmortizacion  NUMERIC(22,7)

	SET @consecutivo=1 
	SET @sumaamortizacion=@p_AmortizacionAnterior
	SET @SumaMontoAmortizacion = 0 

	SET @iCount = (SELECT COUNT(*) FROM @tmpCuponera)
	
	WHILE (@consecutivo) <= @iCount
	BEGIN
		UPDATE 
			@tmpCuponera 
		SET 
			AmortizacConsolidado = @sumaamortizacion,
			MontoInteres = dbo.fn_SIT_CalcularImporteCupon(@p_MontoNominal-@SumaMontoAmortizacion,TasaCupon,DifDias,BaseCupon),
			MontoAmortizacion = ((Amortizac /(100)) * @p_MontoNominal) , 
			NominalRestante = @p_MontoNominal-@SumaMontoAmortizacion
		WHERE consecutivo=@consecutivo
		
		SELECT 
			@amort = Amortizac, 
			@MontoAmortizacion = MontoAmortizacion
		FROM 
			@tmpCuponera 
		WHERE consecutivo = @consecutivo
		
		SET @sumaamortizacion = @amort + @sumaamortizacion
		SET @SumaMontoAmortizacion = @SumaMontoAmortizacion + @MontoAmortizacion
		SET @consecutivo=@consecutivo+1
	END

		SET @sumCuponesMenoUltimoCupon = ISNULL((SELECT 
												     SUM(Amortizac) 
												 FROM 
													@tmpCuponera 
												 WHERE FechaFin <> (SELECT MAX(FechaFin) FROM @tmpCuponera)),0)
		UPDATE 
			@tmpCuponera 
		SET 
			Amortizac = (CASE WHEN (CONVERT(NUMERIC(22,7),(100 - @p_AmortizacionAnterior)) - @sumCuponesMenoUltimoCupon) < 0 THEN 0 
							 ELSE  (CONVERT(NUMERIC(22,7),(100 - @p_AmortizacionAnterior)) - @sumCuponesMenoUltimoCupon) END)
		WHERE 
			FechaFin = (SELECT MAX(FechaFin) FROM @tmpCuponera)

	SELECT 
		 Consecutivo,
		 FechaIni,
		 FechaFin ,
		 DifDias ,
		 Amortizac ,
		 TasaCupon ,
		 BaseCupon ,
		 DiasPago,
		 fechaRealInicial,
		 fechaRealFinal,
		 AmortizacConsolidado,
		 MontoInteres ,
		 MontoAmortizacion,
		 NominalRestante
	FROM
		@tmpCuponera
	ORDER BY
		Consecutivo
END



GRANT EXECUTE ON [dbo].[Amortizar_CuponeraNormal] TO [rol_sit_fondos] AS [dbo]

GO

PRINT 'Fin - Amortizar_CuponeraNormal'

PRINT 'Ini - Calcular_CuponeraNormal'
USE [SIT-FONDOS]
GO
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[Calcular_CuponeraNormal]    Script Date: 07/24/2019 15:17:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Calcular_CuponeraNormal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Calcular_CuponeraNormal]
GO

------------------------------------------------------------------------------------------------------------
-- OBJETIVO: CALCULO DE PERIODO Y AMORTIZACION DE LOS CUPONES
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 25/02/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Creacion
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Modificación: 24/07/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 12028    
-- Descripción del cambio: Se cambia función dbo.dias por fn_SIT_obt_Diferencia_Dias para calcular los dias reales en base ACT
----------------------------------------------------------------------------------------------------------------------------------------  
--exec Calcular_CuponeraNormal 20160312,20220312,20160412,500000,'2','7',5,0,'365','30','BDCAPERIODICO','1'
--exec Calcular_CuponeraNormal 20160720,20230720,20190120,625000000,'6','5',5,0,'365','30','CREAL 07/23','1'
--exec Calcular_CuponeraNormal 20180415,20201214,20190614,4500000,'4','3',5.8356300,0,'360','ACT','BDCA1BC1A','0'
CREATE PROCEDURE [dbo].[Calcular_CuponeraNormal]
(
	@p_FechaInicio NUMERIC(8,0),
	@p_FechaFin NUMERIC(8,0),
	@p_FechaFinPrimerCupon NUMERIC(8,0),
	@p_ValorNominal NUMERIC(22, 7),
	@p_CodigoTipoAmortizacion VARCHAR(4),
	@p_CodigoPeriocidad VARCHAR(4),
	@p_TasaCupon NUMERIC(22, 12),
	@p_TasaSpread NUMERIC(22, 12),
	@p_BaseCupon VARCHAR(4),
	@p_NumeroDias VARCHAR(4),
	@p_CodigoNemonico VARCHAR(15) = NULL,
	@p_FlagSinCalcular CHAR(1)
) AS
BEGIN

	DECLARE @TablaPeriodo TABLE
	(
		[Consecutivo] [INT] NULL,
		[FechaIni] [NUMERIC](8, 0) NULL,
		[FechaFin] [NUMERIC](8, 0) NULL,
		[DifDias] [NUMERIC](8, 0) NULL,
		[Amortizac] [NUMERIC](22, 7) NULL,
		[TasaCupon] [NUMERIC](22, 12) NULL,
		[BaseCupon]  [NUMERIC](22, 7) NULL,
		[DiasPago] [NUMERIC](18, 0)  NULL,
		[fechaRealInicial] [NUMERIC](8, 0) NULL,
		[fechaRealFinal] [NUMERIC](8, 0) NULL,
		[AmortizacConsolidado] [NUMERIC](22, 7) NULL
	)

	DECLARE @TablaPeriodoNuevo TABLE
	(
		[Consecutivo] [INT] NULL,
		[FechaIni] [NUMERIC](8, 0) NULL,
		[FechaFin] [NUMERIC](8, 0) NULL,
		[DifDias] [NUMERIC](8, 0) NULL,
		[Amortizac] [NUMERIC](22, 7) NULL,
		[TasaCupon] [NUMERIC](22, 12) NULL,
		[BaseCupon] [NUMERIC](22, 7) NULL,
		[DiasPago] [NUMERIC](18, 0)  NULL,
		[fechaRealInicial] [NUMERIC](8, 0) NULL,
		[fechaRealFinal] [NUMERIC](8, 0) NULL,
		[AmortizacConsolidado] [NUMERIC](22, 7) NULL,
		[MontoInteres] [NUMERIC](22, 7) NULL,
		[MontoAmortizacion] [NUMERIC](22, 7) NULL,
		[NominalRestante] [NUMERIC] (22, 7) NULL
	)

	DECLARE @TablaPeriodoConjunto TABLE
	(
		[Consecutivo] [INT] NULL,
		[FechaIni] [NUMERIC](8, 0) NULL,
		[FechaFin] [NUMERIC](8, 0) NULL,
		[DifDias] [NUMERIC](8, 0) NULL,
		[Amortizac] [NUMERIC](22, 7) NULL,
		[TasaCupon] [NUMERIC](22, 12) NULL,
		[BaseCupon] [NUMERIC](22, 7) NULL,
		[DiasPago] [NUMERIC](18, 0)  NULL,
		[fechaRealInicial] [NUMERIC](8, 0) NULL,
		[fechaRealFinal] [NUMERIC](8, 0) NULL,
		[AmortizacConsolidado] [NUMERIC](22, 7) NULL,
		[MontoInteres] [NUMERIC](22, 7) NULL,
		[MontoAmortizacion] [NUMERIC](22, 7) NULL,
		[NominalRestante] [NUMERIC] (22, 7) NULL
	)
	---tabla resultante
	DECLARE @TablaCuponeraProcesada TABLE
	(
		[Consecutivo] [INT] NULL,
		[FechaIni] [NUMERIC](8, 0) NULL,
		[FechaFin] [NUMERIC](8, 0) NULL,
		[DifDias] [NUMERIC](8, 0) NULL,
		[Amortizac] [NUMERIC](22, 7) NULL,
		[TasaCupon] [NUMERIC](22, 12) NULL,
		[BaseCupon] [VARCHAR](4) NULL,
		[DiasPago] [VARCHAR](20) NULL,
		[fechaRealInicial] [NUMERIC](8, 0) NULL,
		[fechaRealFinal] [NUMERIC](8, 0) NULL,
		[AmortizacConsolidado] [NUMERIC](22, 7) NULL,
		[MontoInteres] [NUMERIC](22, 7) NULL,
		[NominalRestante] [NUMERIC](22, 7) NULL,
		[Compra] [NUMERIC](22, 7) NULL,
		[CompraAcumulativo] [NUMERIC](22, 7) NULL,
		[Venta] [NUMERIC](22, 7) NULL,
		[Saldo] [NUMERIC](22, 7) NULL,
		[AmortizacionReal] [NUMERIC](22, 7) NULL,
		[InteresReal] [NUMERIC](22, 7) NULL
	)

	DECLARE @fechaTrabajar NUMERIC(8, 0)= @p_FechaInicio,
			@fechaInicioNuevo NUMERIC(8, 0)= @p_FechaInicio

	DECLARE @tmpCuponeraInicialNominal UT_CuponeraTemporal
	
	
	--VERIFICAR SI EXISTE
	DECLARE @existeCupon BIT = 0,
			@valorNominalAnterior NUMERIC(22,7),
			@codigoTipoAmortizacionAnterior VARCHAR(4),
			@codigoPeriocidadAnterior VARCHAR(4),
			@tasaCuponAnterior NUMERIC(22,12),
			@tasaSpreadAnterior NUMERIC(22,12),
			@baseCuponAnterior VARCHAR(4),
			@numeroDiasAnterior VARCHAR(4),
			@fechaFinPrimerCuponAnterior NUMERIC(8,0),
			@fechaFinAnterior NUMERIC(8,0),
			@fechaInicioAnterior NUMERIC(8,0),
			@fechaFinPrimerCuponNuevo NUMERIC(8,0),
			@secuenciaMaximoAnterior INT =0
	
	DECLARE @tmpCuponeraNueva UT_CuponeraTemporal,
			@tmpCuponeraNominal UT_CuponeraTemporal,
			@tmpCuponeraNominalNueva UT_CuponeraTemporal,
			@tmpCuponeraNominalProcesada UT_CuponeraTemporal,
			@tmpCuponeraNominalProcesadaNueva UT_CuponeraTemporal

	DECLARE @diasInteresAnterior VARCHAR(4) = '30',
			@baseCuponInteresAnterior VARCHAR(4) = '360'

	SET @fechaFinPrimerCuponNuevo = @p_FechaFinPrimerCupon
	SET @p_FlagSinCalcular = (CASE WHEN  @p_CodigoPeriocidad = '7' THEN '0' ELSE @p_FlagSinCalcular END)
	SET @p_CodigoTipoAmortizacion = (CASE WHEN @p_CodigoPeriocidad = '7' THEN '1' ELSE @p_CodigoTipoAmortizacion END)
	
	IF @p_CodigoNemonico IS NOT NULL
	BEGIN
		IF EXISTS (SELECT 1 FROM CuponeraNormal WHERE CodigoNemonico=@p_CodigoNemonico) 
		BEGIN
			SET @existeCupon=1

			-- Fecha máxima del portafolio que negocia el instrumento.
			SET @fechaTrabajar = ISNULL((SELECT MAX(FechaNegocio) 
								  FROM Portafolio PO
								  JOIN OrdenInversion CV ON PO.CodigoPortafolioSBS = CV.CodigoPortafolioSBS
								  WHERE CV.CodigoMnemonico = @p_CodigoNemonico),
								  (SELECT MAX(FechaNegocio) 
								   FROM Portafolio PO
								   WHERE PO.Situacion = 'A'))
			
			SELECT 
				@codigoPeriocidadAnterior = CodigoPeriodicidad,
				@codigoTipoAmortizacionAnterior = CodigoTipoAmortizacion,
				@valorNominalAnterior = NumeroUnidades*ValorUnitario,
				@tasaCuponAnterior = TasaCupon,
				@baseCuponAnterior = BaseInteresCorrido,
				@numeroDiasAnterior = BaseInteresCorridoDias,
				@tasaSpreadAnterior = TasaSpread,
				@fechaInicioAnterior = FechaEmision,
				@fechaFinAnterior = FechaVencimiento,
				@fechaFinPrimerCuponAnterior = FechaPrimerCupon,
				@diasInteresAnterior = BaseInteresCorridoDias,
				@baseCuponInteresAnterior = BaseInteresCorrido
			FROM
				Valores 
			WHERE 
				CodigoNemonico = @p_CodigoNemonico 
				AND Situacion='A'
			
			IF EXISTS(SELECT COUNT(1) 
					  FROM CuponeraNormal			
					  WHERE FechaInicio = @fechaTrabajar
							AND CodigoNemonico = @p_CodigoNemonico
							AND Secuencia > 1)
				BEGIN
					SET @fechaTrabajar = dbo.RetornarFechaModificadaEnDias(@fechaTrabajar,-1)
				END

			SELECT 
				@fechaFinAnterior = FechaInicio, 
				@fechaFinPrimerCuponNuevo = FechaTermino 
			FROM 
				CuponeraNormal 
			WHERE 
				CodigoNemonico = @p_CodigoNemonico 
				AND @fechaTrabajar BETWEEN FechaInicio AND FechaTermino 
			ORDER BY CONVERT(INT,Secuencia) DESC
			
		END
	END

--	SET @p_TasaCupon = @p_TasaCupon + @p_tasaSpread

	DECLARE @diasInteres VARCHAR(4) = '30',
			@baseCuponInteres VARCHAR(4) = '360'

--	IF @p_NumeroDias=365
	SET @diasInteres=@p_NumeroDias

	--IF @p_BaseCupon=365
	SET @baseCuponInteres=@p_BaseCupon


	IF(@existeCupon=1)
	BEGIN
		INSERT INTO @TablaPeriodo
								 (Consecutivo,
								  FechaIni,
								  FechaFin,
								  DifDias,
								  Amortizac,
								  TasaCupon,
								  BaseCupon,
								  DiasPago,
								  fechaRealInicial,
								  fechaRealFinal,
								  AmortizacConsolidado)
		SELECT 
			Secuencia,
			FechaInicio,
			FechaTermino,
			DiferenciaDias,
			Amortizacion,
			TasaCupon,
			Base,
			DiasPago,
			FechaInicio,
			FechaTermino,
			MontoNominalLocal
		FROM 
			CuponeraNormal
		WHERE 
			CodigoNemonico = @p_CodigoNemonico

		INSERT INTO @tmpCuponeraNominal
								(Consecutivo,
								 FechaIni,
								 FechaFin,
								 Amortizac,
								 DifDias,
								 TasaCupon,
								 BaseCupon,
								 DiasPago,
								 fechaRealInicial,
								 fechaRealFinal,
								 AmortizacConsolidado,
								 MontoInteres,
								 MontoAmortizacion,
								 NominalRestante)
				 
		SELECT 
			(ROW_NUMBER() OVER(ORDER BY Consecutivo ASC)),
			FechaIni,
			FechaFin,
			Amortizac,
			DifDias,
			TasaCupon,
			SUBSTRING(CONVERT(VARCHAR(20), BaseCupon),0,4),
			CONVERT(VARCHAR(4), DiasPago),
			fechaRealInicial,
			fechaRealFinal,
			0,
			0,
			0,
			0
		FROM 
			@TablaPeriodo
		
		--> CUPONES PROCESADOS ANTES DE LA FECHA NEGOCIO DEL PORTAFOLIO NEGOCIADO.
		INSERT INTO @tmpCuponeraNominalProcesada
		EXEC Amortizar_CuponeraNormal 
									  @tmpCuponeraNominal,
									  @fechaInicioAnterior,
									  @fechaFinAnterior,
									  @codigoTipoAmortizacionAnterior,
									  @baseCuponInteresAnterior,  
									  @tasaCuponAnterior,
									  @tasaSpreadAnterior,
									  @codigoPeriocidadAnterior,
									  @valorNominalAnterior,
									  @diasInteresAnterior,
									  '0',
									  0

		DELETE FROM @tmpCuponeraNominalProcesada WHERE FechaFin > @fechaTrabajar	


		IF @p_FlagSinCalcular = '0'
		BEGIN
			INSERT INTO 
				@TablaPeriodoNuevo
			SELECT 
				Consecutivo,
				FechaIni,
				FechaFin,
				DifDias,
				Amortizac,
				TasaCupon,
				BaseCupon,
				DiasPago,
				fechaRealInicial,
				fechaRealFinal,
				AmortizacConsolidado,
				0,
				0,
				0
			FROM
				@TablaPeriodo
			WHERE
				FechaFin > @fechaTrabajar
		END
		ELSE
		BEGIN
			INSERT INTO @TablaPeriodoNuevo
										(Consecutivo,
										 FechaIni,
										 FechaFin,	
										 DifDias,								 
										 Amortizac,
										 TasaCupon,
										 BaseCupon,
										 DiasPago,
										 fechaRealInicial,
										 fechaRealFinal,
										 AmortizacConsolidado,
										 MontoInteres,
										 MontoAmortizacion,
										 NominalRestante)
			EXEC sp_SIT_GenerarCuponera_Fechas 
										@p_CodigoTipoAmortizacion,
										@fechaFinAnterior,
										@p_FechaFin,
										@p_FechaFinPrimerCupon,
										@p_TasaCupon,
										@p_baseCupon,
										@p_TasaSpread,
										@p_CodigoPeriocidad,
										@p_NumeroDias
		END
		
		DELETE FROM @TablaPeriodo WHERE FechaFin > @fechaTrabajar
		SET @secuenciaMaximoAnterior = ISNULL((SELECT MAX(Consecutivo) FROM @TablaPeriodo),0)
		
		DECLARE @valorNominalRestanteAnterior NUMERIC(22, 7) = ISNULL((SELECT ISNULL(NominalRestante,0) - ISNULL(MontoAmortizacion,0)
																	   FROM @tmpCuponeraNominalProcesada 
																	   WHERE Consecutivo = @secuenciaMaximoAnterior), @valorNominalAnterior)

		DECLARE @valorAmortizacionConsolidadoAnterior NUMERIC(22, 7) = ISNULL((SELECT ISNULL(AmortizacConsolidado,0) + ISNULL(Amortizac,0)
																			   FROM @tmpCuponeraNominalProcesada 
																			   WHERE Consecutivo = @secuenciaMaximoAnterior), 0)

		INSERT INTO @tmpCuponeraNominalNueva
										(Consecutivo,
										 FechaIni,
										 FechaFin,
										 Amortizac,
										 DifDias,
										 TasaCupon,
										 BaseCupon,
										 DiasPago,
										 fechaRealInicial,
										 fechaRealFinal,
										 AmortizacConsolidado,
										 MontoInteres,
										 MontoAmortizacion,
										 NominalRestante)
		SELECT 
				(ROW_NUMBER() OVER(ORDER BY Consecutivo ASC)),
				FechaIni,
				FechaFin,
				Amortizac,
				DifDias,
				TasaCupon,
				substring(convert(varchar(20), BaseCupon),0,4),
				convert(varchar(4), DiasPago),
				fechaRealInicial,
				fechaRealFinal,
				0,
				0,
				0,
				0
		FROM 
			@TablaPeriodoNuevo

		INSERT INTO @tmpCuponeraNominalProcesadaNueva
		EXEC Amortizar_CuponeraNormal 
									 @tmpCuponeraNominalNueva,
									 @fechaFinAnterior,
									 @p_FechaFin,
									 @p_CodigoTipoAmortizacion,
									 @baseCuponInteres,  
									 @p_TasaCupon,
									 @p_tasaSpread,
									 @p_CodigoPeriocidad,
									 @valorNominalRestanteAnterior,
									 @diasInteres,
									 @p_FlagSinCalcular,
									 @valorAmortizacionConsolidadoAnterior

		UPDATE @tmpCuponeraNominalProcesadaNueva 
		SET t.Consecutivo = t.Consecutivo + @secuenciaMaximoAnterior
		FROM @tmpCuponeraNominalProcesadaNueva t

		INSERT INTO @TablaPeriodoConjunto
		SELECT 
			Consecutivo,
			FechaIni,
			FechaFin,
			DifDias,
			Amortizac,
			TasaCupon,
			BaseCupon,
			DiasPago,
			fechaRealInicial,
			fechaRealFinal,
			AmortizacConsolidado,
			0,
			0,
			0
		FROM 
			@TablaPeriodo
		UNION 
		SELECT
			(ROW_NUMBER() OVER(ORDER BY Consecutivo ASC)) + @secuenciaMaximoAnterior,
			FechaIni,
			FechaFin,
			DifDias,
			Amortizac,
			TasaCupon,
			BaseCupon,
			DiasPago,
			fechaRealInicial,
			fechaRealFinal,
			AmortizacConsolidado,
			0,
			0,
			0
		FROM
			@TablaPeriodoNuevo

	END
	ELSE
	BEGIN
		IF @p_CodigoPeriocidad <> '7'
		BEGIN
			INSERT INTO @TablaPeriodoConjunto
											(Consecutivo,
											 FechaIni,
											 FechaFin,	
											 DifDias,								 
											 Amortizac,
											 TasaCupon,
											 BaseCupon,
											 DiasPago,
											 fechaRealInicial,
											 fechaRealFinal,
											 AmortizacConsolidado,
											 MontoInteres,
											 MontoAmortizacion,
											 NominalRestante)
			EXEC sp_SIT_GenerarCuponera_Fechas 
									  @p_CodigoTipoAmortizacion,
									  @p_FechaInicio,
									  @p_FechaFin,
									  @p_FechaFinPrimerCupon,
									  @p_TasaCupon,
									  @p_BaseCupon,
									  @p_TasaSpread,
									  @p_CodigoPeriocidad,
									  @p_NumeroDias
		END
		ELSE
		BEGIN
			DECLARE @Diferencia INT = 0
			IF @p_numeroDias = '30'
				SELECT @Diferencia= dbo.dias360(@p_FechaInicio,@p_FechaFin)
			ELSE
				SELECT @Diferencia= dbo.fn_SIT_obt_Diferencia_Dias(@p_FechaInicio,@p_FechaFin)

			INSERT INTO @TablaPeriodoConjunto
											(Consecutivo,
											 FechaIni,
											 FechaFin,	
											 DifDias,								 
											 Amortizac,
											 TasaCupon,
											 BaseCupon,
											 DiasPago,
											 fechaRealInicial,
											 fechaRealFinal,
											 AmortizacConsolidado,
											 MontoInteres,
											 MontoAmortizacion,
											 NominalRestante)
				VALUES(1,
					   @p_FechaInicio,
					   @p_FechaFin,
					   @Diferencia,
					   100,
					   @p_TasaCupon,
					   (CASE WHEN @p_baseCupon = 'ACT' THEN (CASE WHEN dbo.SIT_fn_EsAnioBisiesto(LEFT(@p_fechaFin,4))= 1 THEN '366' 
																														ELSE '365' END)
																  ELSE @p_baseCupon END),
					   @Diferencia,
					   @p_FechaInicio,
					   @p_FechaFin,
					   0,
					   0,
					   0,
					   0)
		END
		 INSERT INTO @tmpCuponeraNominal
										(Consecutivo,
										 FechaIni,
										 FechaFin,
										 Amortizac,
										 DifDias,
										 TasaCupon,
										 BaseCupon,
										 DiasPago,
										 fechaRealInicial,
										 fechaRealFinal,
										 AmortizacConsolidado,
										 MontoInteres,
										 MontoAmortizacion,
										 NominalRestante)
		 SELECT 
				(ROW_NUMBER() OVER(ORDER BY Consecutivo ASC)),
				FechaIni,
				FechaFin,
				0,
				DifDias,
				TasaCupon,
				SUBSTRING(CONVERT(VARCHAR(20), BaseCupon),0,4),
				convert(varchar(4), DiasPago),
				fechaRealInicial,
				fechaRealFinal,
				0,
				0,
				0,
				0
		FROM 
			@TablaPeriodoConjunto
		PRINT('INSERT INTO @tmpCuponeraNominalProcesadaNueva')

		INSERT INTO @tmpCuponeraNominalProcesada
		EXEC Amortizar_CuponeraNormal 
									@tmpCuponeraNominal,
									@p_FechaInicio,
									@p_FechaFin,
									@p_CodigoTipoAmortizacion,
									@baseCuponInteres,  
									@p_TasaCupon,
									@p_tasaSpread,
									@p_CodigoPeriocidad,
									@p_ValorNominal,
									@diasInteres,
									'1',
									0
	END

	INSERT INTO @tmpCuponeraNueva
							   (Consecutivo,
								FechaIni,
								FechaFin,
								Amortizac,
								DifDias,
								TasaCupon,
								BaseCupon,
								DiasPago,
								fechaRealInicial,
								fechaRealFinal,
								AmortizacConsolidado,
								MontoInteres,
								MontoAmortizacion,
								NominalRestante)
	SELECT
		(ROW_NUMBER() OVER(ORDER BY Consecutivo ASC)),
		 FechaIni,
		 FechaFin,
		 Amortizac,
		 DifDias,
		 TasaCupon,
		 SUBSTRING(CONVERT(VARCHAR(20), BaseCupon),0,4),
		 CONVERT(VARCHAR(4), DiasPago),
		 fechaRealInicial,
		 fechaRealFinal,
		 0,
		 0,
		 0,
		 0
	FROM 
		@TablaPeriodoConjunto 
	WHERE 
		Consecutivo > @secuenciaMaximoAnterior

	DECLARE @tmpCuponeraInicial UT_CuponeraTemporal

	INSERT INTO @tmpCuponeraInicial
								   (Consecutivo,
									FechaIni,
									FechaFin,
									Amortizac,
									DifDias,
									TasaCupon,
									BaseCupon,
									DiasPago,
									fechaRealInicial,
									fechaRealFinal,
									AmortizacConsolidado,
									MontoInteres,
									MontoAmortizacion,
									NominalRestante)
	SELECT
		 (ROW_NUMBER() OVER(ORDER BY Consecutivo ASC)),
	     FechaIni,
	     FechaFin,
		 Amortizac,
		 DifDias,
		 TasaCupon,
		 SUBSTRING(CONVERT(VARCHAR(20), BaseCupon),0,4),
		 CONVERT(VARCHAR(4), DiasPago),
		 fechaRealInicial,
		 fechaRealFinal,
		 AmortizacConsolidado,
		 ((SELECT ISNULL(SUM(MontoOperacion),0) 
		   FROM OrdenInversion 
		   WHERE CodigoMnemonico=@p_CodigoNemonico 
				 AND FechaOperacion BETWEEN CASE WHEN Consecutivo = 1 THEN 1 ELSE FechaIni+1 END AND FechaFin 
				 AND Situacion='A' 
				 AND codigoOperacion=35)),
		 ((SELECT ISNULL(SUM(MontoOperacion),0) 
		   FROM OrdenInversion 
		   WHERE CodigoMnemonico=@p_CodigoNemonico 
				 AND FechaOperacion BETWEEN CASE WHEN Consecutivo =1 THEN 1 ELSE FechaIni+1 END AND FechaFin 
				 AND Situacion='A' 
				 AND codigoOperacion in (38,39))),
		 ((SELECT  ISNULL(SUM(CASE WHEN OI.CodigoOperacion = 1 THEN ISNULL(MontoNominalOrdenado,0)
								   WHEN OI.CodigoOperacion = 2 THEN 0 - ISNULL(MontoNominalOrdenado,0) END),0) 
				   --+ ISNULL(SUM(ISNULL(CS.SaldoInicialUnidades,0)*VA.ValorUnitario),0)
		   FROM OrdenInversion OI
		   --LEFT JOIN CustodioSaldo CS ON OI.CodigoPortafolioSBS = CS.CodigoPortafolioSBS
					--				     AND OI.CodigoMnemonico = CS.CodigoMnemonico
					--				      AND CS.FechaSaldo =(CASE WHEN Consecutivo =1 AND OI.FechaOperacion < FechaIni THEN 1
					--											   ELSE dbo.fn_SIT_gl_DiaHabilAnterior(dbo.RetornarFechaModificadaEnDias(FechaIni,1))  END)
		   --JOIN Valores VA ON OI.CodigoMnemonico=VA.CodigoNemonico					  				
		   WHERE OI.CodigoMnemonico=@p_CodigoNemonico 
				 AND OI.FechaOperacion BETWEEN CASE WHEN Consecutivo =1 THEN 1 ELSE FechaIni+1 END AND FechaFin 
				 AND OI.Situacion='A' 
			--	 AND VA.Situacion = 'A'
				 AND OI.CodigoOperacion IN (1,2)
				))
	FROM 
		@TablaPeriodoConjunto 
	WHERE Consecutivo <= @secuenciaMaximoAnterior

	DECLARE @cont INT = 1,
			@IndiceMaximoCupon INT = ISNULL((SELECT MAX(Consecutivo) FROM @tmpCuponeraInicial),0),
			@MontoNominalInicial DECIMAL(22,7) = ISNULL((SELECT TOP 1 NominalRestante FROM @tmpCuponeraInicial ORDER BY Consecutivo),0),
			@SumandoSaldo NUMERIC(22,7) = 0,
			@SumandoAmortizacion NUMERIC(22,7) = 0,
			@Amortizacion NUMERIC(22,7) = 0

	WHILE @cont <= @IndiceMaximoCupon  
	BEGIN  
		SET @SumandoSaldo=@SumandoSaldo+(SELECT NominalRestante FROM @tmpCuponeraInicial WHERE Consecutivo=@cont) - @Amortizacion
		
		UPDATE 
			@tmpCuponeraInicial 
		SET 
			NominalRestante=@SumandoSaldo 
		WHERE 
			Consecutivo=@cont

		SET @Amortizacion = (SELECT (Amortizac*@MontoNominalInicial)/100 FROM @tmpCuponeraInicial WHERE Consecutivo = @cont)	
		SET @cont=@cont+1
	END  

	DECLARE @tmpCuponeraProcesada UT_CuponeraTemporal

	DECLARE @fechaInicioPeriodoNuevo NUMERIC(8,0),
			@fechaFinPeriodoNuevo NUMERIC(8,0),
			@secuenciaMinimoNuevo INT,
			@secuenciaMaximoNuevo INT

	SELECT 
		 @secuenciaMaximoNuevo = MAX(CONVERT(INT,Consecutivo)),
		 @secuenciaMinimoNuevo = MIN(CONVERT(INT,Consecutivo))
	FROM 
		@tmpCuponeraNueva

	SET @fechaInicioPeriodoNuevo =(SELECT FechaIni FROM @tmpCuponeraNueva WHERE Consecutivo=@secuenciaMinimoNuevo)
	SET @fechaFinPeriodoNuevo =(SELECT FechaFin FROM @tmpCuponeraNueva WHERE Consecutivo=@secuenciaMaximoNuevo)

	--obtener el monto
	DECLARE @SaldoActualReal NUMERIC (22,7) = ISNULL((SELECT ISNULL(SUM(CASE WHEN OI.CodigoOperacion = 1 THEN ISNULL(MontoNominalOrdenado,0)
																				WHEN OI.CodigoOperacion = 2 THEN 0 - ISNULL(MontoNominalOrdenado,0)
																				WHEN OI.CodigoOperacion IN (38,39) THEN 0 - ISNULL(MontoOperacion,0) END),0) 
														 FROM OrdenInversion OI 
														 WHERE OI.CodigoMnemonico=@p_CodigoNemonico 
															   AND OI.FechaOperacion BETWEEN 1  AND @p_FechaFin 
															   AND OI.Situacion='A' 
															   AND OI.CodigoOperacion IN (1,2,38,39)),0) 


	SET @p_FlagSinCalcular = (CASE WHEN @p_CodigoPeriocidad = '7' THEN '0' ELSE '1' END)

	INSERT INTO @tmpCuponeraProcesada
	EXEC Amortizar_CuponeraNormal 
							@tmpCuponeraNueva,
							@fechaInicioPeriodoNuevo,
							@fechaFinPeriodoNuevo,
							@p_CodigoTipoAmortizacion,
							@baseCuponInteres,  
							@p_TasaCupon,
							@p_tasaSpread,
							@p_CodigoPeriocidad,
							@SaldoActualReal,
							@diasInteres,
							@p_FlagSinCalcular,
							@valorAmortizacionConsolidadoAnterior

	UPDATE @tmpCuponeraProcesada 
	SET t.Consecutivo =  t.Consecutivo + @secuenciaMaximoAnterior
	FROM  @tmpCuponeraProcesada t

--	UPDATE @tmpCuponeraNominalProcesada SET amortizac=0 , AmortizacConsolidado=0
--	UPDATE @tmpCuponeraInicial SET amortizac=0 , AmortizacConsolidado=0

	DECLARE @tmpTablaResultado TABLE (
									  Consecutivo INT,
									  FechaIni NUMERIC(8),
									  FechaFin NUMERIC(8),
									  fechaRealInicial NUMERIC(8),
									  fechaRealFinal NUMERIC(8),
									  DifDias NUMERIC(8),
									  TasaCupon NUMERIC(22,7),
									  AmortizacionFondo NUMERIC(22,7),
									  InteresFondo NUMERIC(22,7),
									  PrincipalFondo NUMERIC(22,7),
									  AmortizacionNominal NUMERIC(22,7),
									  InteresNominal NUMERIC(22,7),
									  PrincipalNominal NUMERIC(22,7),
									  Amortizac NUMERIC(22,7),
									  AmortizacConsolidado NUMERIC(22,7),
									  Estado CHAR(1),
									  BaseCupon VARCHAR(4),
									  DiasPago VARCHAR(4),
									  PorcentajeFondo NUMERIC(22,7),
									  TasaVariable NUMERIC(22,7)
									 )
	SET @fechaTrabajar = ISNULL((SELECT MAX(FechaNegocio) 
				  FROM Portafolio PO
				  JOIN CustodioValores CV ON PO.CodigoPortafolioSBS = CV.CodigoPortafolioSBS
				  WHERE CV.CodigoNemonico = @p_CodigoNemonico),
				  (SELECT MAX(FechaNegocio) 
				   FROM Portafolio PO
				   WHERE PO.Situacion = 'A'))
			 
	INSERT INTO @tmpTablaResultado								  
	SELECT 
		cr.Consecutivo,
		cr.FechaIni,
		cr.FechaFin,
		cr.fechaRealInicial,
		cr.fechaRealFinal,
		cr.DifDias,
		cr.TasaCupon,
		cr.MontoAmortizacion 'AmortizacionFondo' ,
		cr.MontoInteres 'InteresFondo' ,
		cr.NominalRestante 'PrincipalFondo',
		cn.MontoAmortizacion 'AmortizacionNominal' ,
		cn.MontoInteres 'InteresNominal' ,
		cn.NominalRestante 'PrincipalNominal',
		cn.Amortizac ,
		cr.AmortizacConsolidado,
		CASE WHEN @existeCupon=0 THEN (CASE WHEN @fechaTrabajar <= cr.FechaFin THEN '1' 
										    ELSE '0' END)
			 ELSE cn.Estado END 'Estado',
		cr.BaseCupon,
		cr.DiasPago,
		CASE WHEN cr.NominalRestante=0 THEN 0 ELSE (cr.NominalRestante / (CASE WHEN cn.NominalRestante = 0 THEN 1 ELSE cn.NominalRestante END)) * 100 END 'PorcentajeFondo',
		cr.TasaVariable
	FROM (
		  SELECT 
			Consecutivo,
			FechaIni,
			FechaFin,
			Amortizac,
			DifDias,
			TasaCupon,
			BaseCupon,
			DiasPago,
			fechaRealInicial,
			fechaRealFinal,
			AmortizacConsolidado,
			MontoInteres,
			MontoAmortizacion,
			NominalRestante, 
			'0' Estado		
		  FROM 
			 @tmpCuponeraNominalProcesada
		  UNION 
		  SELECT 
			Consecutivo,
			FechaIni,
			FechaFin,
			Amortizac,
			DifDias,
			TasaCupon,
			BaseCupon,
			DiasPago,
			fechaRealInicial,
			fechaRealFinal,
			AmortizacConsolidado,
			MontoInteres,
			MontoAmortizacion,
			NominalRestante, 
			'1' Estado
		  FROM 
			@tmpCuponeraNominalProcesadaNueva t
		 ) cn
	JOIN (
		SELECT 
			Consecutivo,
			FechaIni,
			FechaFin,
			Amortizac,
			DifDias,
			TCI.TasaCupon,
			BaseCupon,
			TCI.DiasPago,
			fechaRealInicial,
			fechaRealFinal,
			AmortizacConsolidado,
			MontoInteres,
			MontoAmortizacion,
			NominalRestante,
			ISNULL(TasaVariable,0) TasaVariable
		FROM 
			@tmpCuponeraInicial TCI
		JOIN
			CuponeraNormal CN ON CN.FechaInicio = TCI.FechaIni
								 AND CN.FechaTermino = TCI.FechaFin
		WHERE
			CN.CodigoNemonico = @p_CodigoNemonico
		UNION
		SELECT 
			Consecutivo,
			FechaIni,
			FechaFin,
			Amortizac,
			DifDias,
			TasaCupon,
			BaseCupon,
			DiasPago,
			fechaRealInicial,
			fechaRealFinal,
			AmortizacConsolidado,
			MontoInteres,
			MontoAmortizacion,
			NominalRestante,
			--(CASE WHEN Consecutivo = (SELECT MIN(Consecutivo) FROM @tmpCuponeraProcesada) THEN @p_TasaSpread ELSE 0 END)TasaVariable
			@p_TasaSpread TasaVariable
		FROM 
			@tmpCuponeraProcesada
	) cr ON cn.Consecutivo=cr.Consecutivo

	SELECT 
		Consecutivo, 
		FechaIni,
		FechaFin, 
		fechaRealInicial, 
		fechaRealFinal, 
		DifDias, 
		ROUND(TasaCupon,7) TasaCupon, 
		AmortizacionFondo,
		InteresFondo, 
		ROUND(PrincipalFondo,7) PrincipalFondo, 
		AmortizacionNominal,
		InteresNominal,
		ROUND(PrincipalNominal,7)PrincipalNominal,
		Amortizac, 
		AmortizacConsolidado,
		Estado,
		BaseCupon, 
		DiasPago,
		PorcentajeFondo,
		TasaVariable
	FROM 
		@tmpTablaResultado
END


GRANT EXECUTE ON [dbo].[Calcular_CuponeraNormal] TO [rol_sit_fondos] AS [dbo]

GO

PRINT 'Fin - Calcular_CuponeraNormal'
PRINT 'Inicio - Valorizacion_listar'
USE [SIT-FONDOS]
GO


IF EXISTS (SELECT * FROM sysobjects WHERE name='Valorizacion_listar') BEGIN 
	DROP PROCEDURE [dbo].[Valorizacion_listar]
END 
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
-- Descripcion del cambio: Se adiciona validación de icono de advertencia cuando la fecha de valoraci┬¥n no coincide con la última fecha 
--						   de valorización.
--------------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 12/03/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11908    
-- Descripcion del cambio: Se adiciona validación para valorización de fondo nuevo y no muestre icono de advertencia.
--------------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 25/07/2019    
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 12028    
-- Descripcion del cambio: Se implementa control de apertura cajas.
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
								( 
								 ID INT IDENTITY(1,1),    
								 codigoPortafolio VARCHAR(10),    
								 fondo VARCHAR(40),    
								 FechaConstitucion NUMERIC(8,0),    
								 estado INT,     
								 archivosPIP INT,    
								 cierreCaja INT 
								)    

	CREATE TABLE #lstNemonico    
							(
							 ID INT IDENTITY(1,1),    
							 CodigoMnemonico VARCHAR(15)
							)    

	DECLARE 
		@CODIGOPORTAFOLIO VARCHAR(10),    
		@PIP INT ,   
		--@CAJA INT,    
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

		--SET @CAJA = 1    
	-- OT12028 Control de Cierre de Cajas
	UPDATE
		VL
	SET
		cierreCaja = (CASE WHEN ISNULL((SELECT MIN(FechaCajaOperaciones) 
								        FROM PortafolioCaja 
								        WHERE CodigoPortafolioSBS = VL.codigoPortafolio),0) > @p_FechaOperacion THEN 1
							ELSE 0 END)
	FROM
		#valorizacion VL
	-- Fin OT12028

	IF (@FERIADO > 0 OR @FINSEMANA = 1 OR @FINSEMANA = 7)
	BEGIN
		SET @COUNT = (SELECT COUNT(VP.Fecha) FROM VectorPrecio VP WHERE VP.Fecha = @p_FechaOperacion)
		SET @COUNTVPF = (SELECT COUNT(VPF.Fecha) FROM VectorForwardSBS VPF WHERE VPF.Fecha = @p_FechaOperacion)
		IF @COUNT > 4 AND @COUNTVPF > 4
		BEGIN
			SET @PIP = 1
			UPDATE #valorizacion SET archivosPIP = @PIP
			--, cierreCaja = @CAJA
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

			UPDATE #valorizacion SET archivosPIP = @PIP
			--, cierreCaja = @CAJA 
			WHERE  id = @FILA
			SET @FILA =  @FILA +1
		END
	END

	SELECT 
		codigoPortafolio,
		fondo,
		FechaConstitucion,
		estado,
		archivosPIP,
		--,@CAJA AS 'cierreCaja' 
		cierreCaja
	FROM 
		#valorizacion
	GROUP BY  
		codigoPortafolio,
		fondo,
		FechaConstitucion,
		estado,
		archivosPIP,
		cierreCaja
	ORDER BY 
		fondo ASC

	DROP TABLE #valorizacion
END


GO

GRANT EXECUTE ON [dbo].[Valorizacion_listar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'Fin - Valorizacion_listar'
GO

PRINT 'Inicio - Pr_Sit_Rpt_Rentabilidad '
USE [SIT-FONDOS]
GO

DROP PROCEDURE [dbo].[Pr_Sit_Rpt_Rentabilidad]
GO


---------------------------------------------------------------------------------------------------------------------  
--Objetivo: Listar el reporte de rentabilidad  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 15-12-2016  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9682  
-- Descripcion del cambio: Se modifico el procedimiento      
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 28-12-2016  
-- Modificado por: Everth Martinez  
-- Nro. Orden de Trabajo: 9764  
-- Descripcion del cambio: retirar el select del temporal  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 08/05/2017  
-- Modificado por: Espejo Huerta Carlos  
-- Nro. Orden de Trabajo: 10328  
-- Descripcion del cambio: Se valida el cambio de moneda para los forward y los campos que se muestran en un OR  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 11/05/2017  
-- Modificado por: Espejo Huerta Carlos  
-- Nro. Orden de Trabajo: 10372  
-- Descripcion del cambio: Para las Operaciones de Reporte no se muestra el nemonico de la accion, se toma el titulo  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 18/05/2017  
-- Modificado por: Espejo Huerta Carlos  
-- Nro. Orden de Trabajo: 10362  
-- Descripcion del cambio: Para los forwards se solicita que tambien presente valor en el campo CODIGO_ISIN  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 01/06/2017  
-- Modificado por: Magno Sanchez  
-- Nro. Orden de Trabajo: 10525  
-- Descripcion del cambio: Para los forwards se solicita que la informacion debe ser con la moneda del fondo y no en la del instrumento  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 10/07/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10610  
-- Descripcion del cambio: Se corrigio la obtención del VPNLoca y VPNAyer para los portafolios que no presentan valo-  
--                         rización en la fecha T  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 08/08/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10686  
-- Descripcion del cambio: Se corrigió que las pre-cancelaciones de DPZ al momento de su vencimiento aparezca el monto  
--                         en el campo de vencimiento de la rentabilidad  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 17/08/2018  
-- Creado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11512  
-- Descripcion del cambio: Reemplazar la función "dbo.retornarPrecioVector" por "dbo.RetornarVectorPrecioT_1"  
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 30/08/2018  
-- Creado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar la función dbo.RetornarSecuenciaVectorTipoCambio  
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 31/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO  
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 31/07/2019  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 12028
-- Descripcion del cambio: Tomar la moneda de fondo en las negociaciones del día
-----------------------------------------------------------------------------------------------------------------------  
--Pr_Sit_Rpt_Rentabilidad '104',20190716,'A'
CREATE PROCEDURE [dbo].[Pr_Sit_Rpt_Rentabilidad] (
	@p_codigoPortafolioSbs VARCHAR(20),
	@p_fechaOperaciON NUMERIC(8),
	@p_estado VARCHAR(1)
)  
AS  
BEGIN  
	DECLARE 
		@tCartera AS TABLE(
						   portafolio VARCHAR(20),
						   grupo VARCHAR(5),
						   descripciON VARCHAR(100),
						   mONeda VARCHAR(5),
						   emisiON VARCHAR(30),  
						   entidad VARCHAR(10),
						   codigoSbs VARCHAR(20),
						   codigoIsin VARCHAR(20),
						   fecha NUMERIC(8),
						   fechaEmisiON VARCHAR(20),
						   fechaValoraciON VARCHAR(20),  
						   nroTitulo NUMERIC(22),
						   titulo VARCHAR(20),
						   precio NUMERIC(22,7),
						   cantidad NUMERIC(22,7),
						   mONto NUMERIC(22,7),
						   vpnHoy NUMERIC(22,7),
						   vpnAyer NUMERIC(22,7),
						   comprAS NUMERIC(22,7),
						   ventAS NUMERIC(22,7),
						   vencimientos NUMERIC(22,7),
						   amortizaciONes NUMERIC(22,7),
						   intereses NUMERIC(33,7),
						   dividENDos NUMERIC(22,7),
						   rentabilidad NUMERIC(22,7)
						  )  
   
	 DECLARE 
		@t_Inversiones_prev AS TABLE (
									  Portafolio VARCHAR(20),
									  CodigoSBS VARCHAR(20),
									  Fecha NUMERIC(8),
									  Compras NUMERIC(22,7),
									  Ventas NUMERIC(22,7),  
									  Vencimientos NUMERIC(22,7), 
									  Amortizaciones NUMERIC(22,7), 
									  Intereses NUMERIC(33,7), 
									  Dividendos NUMERIC(22,7), 
									  TipoCalculo varchar(1), 
									  TipoCambio NUMERIC(22,7),  
									  TipoCambioNeg NUMERIC(22,7),
									  MonedaPortafolio VARCHAR(10),
									  MonedaNegociado VARCHAR(10)
									 )  
   
	 --Obtener fecha de inicio del mes  
	 DECLARE 
		@fechaInicioMes NUMERIC(8) = CAST(left(CAST(@p_fechaOperaciON AS VARCHAR),6) + '01' AS NUMERIC(8))  
   
	 --Declaración e inicialización de la fechASecuencial.  
	 DECLARE 
		@fechASecuencial NUMERIC(8) = @p_fechaOperaciON  
   
	 --Declaración e inicialización fecha anterior  
	 DECLARE 
		@fechaAnterior NUMERIC(8) = dbo.fn_SIT_ObtenerDiaAnterior(@p_fechaOperaciON)  
   
	 --Cartera Forward  
	 INSERT INTO 
		@tCartera  
	 SELECT 
		PORTAFOLIO = p.DescripciON,
		GRUPO = 'CSUR', 
		DESCRIPCION = 'FORWARD SOBRE DIVISAS FONDOS',
		MONEDA = m.CodigoMONeda,
		EMISION = '',
		ENTIDAD = E.CodigoEntidad,  	 
		CODIGO_SBS = OI.CodigoISIN,
		CODIGO_ISIN = '',
		FECHA = ctv.Fecha,
		FECHA_EMISION = ISNULL(dbo.FormatDate(OI.FechaOperaciON),''),  
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.Fecha),''),
		NRO_TITULO = OI.NumeroIdentificaciON ,  
		TITULO = v.CodigoNemonico ,
		PRECIO = 0,
		CANTIDAD = 0, 
		MONTO = 0,  
		--VPN_HOY = ROUND(ctv.MtmUSD / TC.ValorPrimario ,2) ,  
		VPN_HOY = ROUND(ctv.MtmUSD / dbo.RetornarSecuenciaVectorTipoCambio(OI.CodigoPortafolioSBS,p.CodigoMoneda,@fechASecuencial) ,2) ,  
		VPN_AYER = ROUND(dbo.Fn_Sit_RetornarVPNLocalForward(CTV.NumeroPoliza ,ctv.Fecha,'0') ,2),  
		COMPRAS = 0,
		VENTAS = 0,
		VENCIMIENTOS = 0,
		AMORTIZACIONES = 0,
		INTERESES = 0,
		DIVIDENDOS = 0,
		RENTABILIDAD = 0  
	 FROM 
		VectorForwardSBS  ctv  
	 JOIN 
		OrdenInversiON OI ON OI.CodigoISIN  = CTV.NumeroPoliza 
						     AND OI.CodigoPortafolioSBS = @p_codigoPortafolioSbs  
	 JOIN 
		Portafolio p ON p.CodigoPortafolioSBS = OI.CodigoPortafolioSBS  
	 JOIN 
		Valores v ON v.CodigONemONico = OI.CodigoMnemONico 
		             AND v.SituaciON = @p_estado  
	 JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS 
		                      AND ti.SituaciON = @p_estado  
	 JOIN 
		MONeda m ON m.CodigoMONeda = P.CodigoMONeda 
		            AND m.SituaciON = @p_estado ---  
	 JOIN 
		Entidad E ON E.CodigoTercero = OI.CodigoTercero 
		             AND E.SituaciON = @p_estado  
	 --JOIN VectorTipoCambio TC ON TC.Fecha = ctv.Fecha AND TC.CodigoMoneda = p.CodigoMoneda ---  
	 WHERE 
		ctv.Fecha = @fechASecuencial  
   
	 --Cartera Titulos Unicos  
	 INSERT INTO 
		@tCartera  
	 SELECT 
		PORTAFOLIO = p.DescripciON,
		GRUPO = ISNULL(ti.CodigoTipoInstrumentoSBS,'') + '-' + M.CodigoMONedaSBS,  
		DESCRIPCION = ISNULL(TIH.Descripcion, ti.DescripciON),
		MONEDA = m.CodigoMONeda,
		EMISION = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE '' END,  
		ENTIDAD = E.CodigoEntidad,
		CODIGO_SBS = ctv.CodigoSBS,
		CODIGO_ISIN = '',
		FECHA = ctv.FechaValoraciON,  
		FECHA_EMISION = ISNULL(dbo.FormatDate(OI.FechaOperaciON),''),
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.FechaValoraciON),''),  
		NRO_TITULO = OI.NumeroIdentificaciON,
		TITULO = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE DBO.fn_EquivalenciaNemonico(OI.CodigoMnemonico,OI.CodigoTipoCupon) END, --10372  
		PRECIO = 0,
		CANTIDAD = 0,
		MONTO = 0,
		VPN_HOY = ctv.VPNMonedaFondo,
		VPN_AYER = dbo.Fn_Sit_RetornarVPNLocalAnterior(ctv.CodigoPortafolioSBS,ctv.CodigoSBS,ctv.FechaValoraciON),  
		COMPRAS = 0,
		VENTAS = 0,  
		VENCIMIENTOS = (CASE WHEN (OI.CategoriaInstrumento = 'OR' AND OI.FechaContrato = @p_fechaOperaciON) 
							 THEN dbo.Fn_Sit_RetornarVPNLocalAnterior(ctv.CodigoPortafolioSBS,ctv.CodigoSBS,ctv.FechaValoraciON) 
							 ELSE 0 END),  
		AMORTIZACIONES = 0,
		INTERESES = 0,
		DIVIDENDOS = 0,
		RENTABILIDAD = 0  
	 FROM 
		CarteraTituloValoraciON ctv  
	 JOIN 
		OrdenInversiON OI ON OI.CodigoSBS = CTV.CodigoSBS 
							 AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS 
							 AND OI.CategoriaInstrumento  IN ('DP','OR') --10328  
							 AND CodigoOperaciON  IN ( '3', '101')--10328  
	 JOIN 
		Portafolio p ON ctv.CodigoPortafolioSBS = p.CodigoPortafolioSBS  
	 JOIN 
		Valores v ON ctv.CodigoMnemONico = v.CodigONemONico 
					 AND v.SituaciON = @p_estado  
	 JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = left(ctv.CodigoSBS,2) 
							  AND ti.SituaciON = @p_estado  
	 JOIN 
		MONeda m ON v.CodigoMONeda = m.CodigoMONeda 
					AND m.SituaciON = @p_estado   
	 JOIN 
		Entidad E ON E.CodigoTercero = OI.CodigoTercero 
					 AND E.SituaciON = 'A'  
	 LEFT JOIN 
		TipoInstrumentoHomologacion TIH ON TIH.CodigoTipoInstumentoSBS = V.CodigoTipoInstrumentoSBS 
										   AND TIH.CodigoMoneda = V.CodigoMoneda   
	 WHERE 
		ctv.CodigoPortafolioSBS = @p_codigoPortafolioSbs 
		AND ctv.FechaValoraciON = @fechASecuencial 
		AND ctv.Escenario = 'REAL'  
  
 --Cartera Titulos Unicos fecha anterior  
	 INSERT INTO 
		@tCartera  
	 SELECT 
		PORTAFOLIO = p.DescripciON,GRUPO = ISNULL(ti.CodigoTipoInstrumentoSBS,'') + '-' + M.CodigoMONedaSBS,  
		DESCRIPCION = ISNULL(TIH.Descripcion, 
		ti.DescripciON),
		MONEDA = m.CodigoMONeda,
		EMISION = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE '' END,  
		ENTIDAD = E.CodigoEntidad,
		CODIGO_SBS = ctv.CodigoSBS,
		CODIGO_ISIN = '',FECHA = @fechASecuencial,  
		FECHA_EMISION = ISNULL(dbo.FormatDate(OI.FechaOperaciON),''),
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.FechaValoraciON),''),  
		NRO_TITULO = OI.NumeroIdentificaciON,  
		TITULO = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE DBO.fn_EquivalenciaNemonico(OI.CodigoMnemonico,OI.CodigoTipoCupon) END, --10372  
		PRECIO = 0,
		CANTIDAD = 0, 
		MONTO = 0,
		VPN_HOY = 0,
		VPN_AYER = ctv.VPNMonedaFondo,  
		COMPRAS = 0,
		VENTAS = 0,  
		VENCIMIENTOS = 0,  
		AMORTIZACIONES = 0,
		INTERESES = 0,
		DIVIDENDOS = 0,
		RENTABILIDAD = 0  
	 FROM 
		CarteraTituloValoraciON ctv  
	 JOIN 
		OrdenInversiON OI ON OI.CodigoSBS = CTV.CodigoSBS 
							 AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS 
							 AND OI.CategoriaInstrumento  IN ('DP','OR') --10328  
							 AND CodigoOperaciON  IN ( '3', '101')--10328  
	 JOIN 
		Portafolio p ON ctv.CodigoPortafolioSBS = p.CodigoPortafolioSBS  
	 JOIN 
		Valores v ON ctv.CodigoMnemONico = v.CodigONemONico 
					 AND v.SituaciON = @p_estado  
	 JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = left(ctv.CodigoSBS,2) 
							 AND ti.SituaciON = @p_estado  
	 JOIN 
		MONeda m ON v.CodigoMONeda = m.CodigoMONeda 
					AND m.SituaciON = @p_estado   
	 JOIN 
		Entidad E ON E.CodigoTercero = OI.CodigoTercero 
					 AND E.SituaciON = 'A'  
	 LEFT JOIN 
		TipoInstrumentoHomologacion TIH ON TIH.CodigoTipoInstumentoSBS = V.CodigoTipoInstrumentoSBS 
										   AND TIH.CodigoMoneda = V.CodigoMoneda   
	 WHERE 
		ctv.CodigoPortafolioSBS = @p_codigoPortafolioSbs 
		AND ctv.FechaValoraciON = @fechaAnterior 
		AND ctv.Escenario = 'REAL'  
		AND NOT EXISTS(SELECT 1 FROM @tCartera WHERE fecha = @fechASecuencial AND CodigoSBS = ctv.CodigoSBS and portafolio = p.Descripcion)  
  
	 --Cartera Valores  
	 INSERT INTO 
		@tCartera  
	 SELECT 
		PORTAFOLIO = p.DescripciON,
		GRUPO = ISNULL(ti.CodigoTipoInstrumentoSBS,'') + '-' + dbo.FN_SIT_OBT_CodigoMonedaSBS(p.CodigoMoneda),  
		DESCRIPCION = ISNULL(TIH.Descripcion, ti.DescripciON),
		MONEDA = m.CodigoMONeda,
		EMISION = ctv.CodigoMnemONico,  
		ENTIDAD = e.CodigoEntidad,
		CODIGO_SBS = ctv.CodigoSBS,
		CODIGO_ISIN = v.CodigoISIN,
		FECHA = ctv.FechaValoraciON,
		FECHA_EMISION = ISNULL(dbo.FormatDate(v.FechaEmisiON),''),  
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.FechaValoraciON),''),
		NRO_TITULO = ctv.SecuenciaValorizaciON,
		TITULO = ctv.CodigoMnemONico,  
	 --PRECIO = dbo.RetornarVectorPrecioT_1(v.CodigONemONico,P.VectorPrecioVal,ctv.FechaValoraciON),  
		PRECIO = dbo.RetornarSecuenciaVectorPrecio(ctv.CodigoPortafolioSBS,v.CodigoNemonico,ctv.FechaValoraciON),  
		CANTIDAD = ctv.Cantidad,
		MONTO =  0 ,
		VPN_HOY = ctv.VPNMonedaFondo,  
		VPN_AYER = dbo.Fn_Sit_RetornarVPNLocalAnterior(ctv.CodigoPortafolioSBS,ctv.CodigoSBS,ctv.FechaValoraciON),
		COMPRAS = 0,
		VENTAS = 0,
		VENCIMIENTOS = 0,
		AMORTIZACIONES = 0,  
		INTERESES = 0,
		DIVIDENDOS = 0,
		RENTABILIDAD = 0  
	 FROM 
		CarteraTituloValoraciON ctv  
	 JOIN 
		Portafolio p ON ctv.CodigoPortafolioSBS = p.CodigoPortafolioSBS  
	 JOIN 
		Valores v ON ctv.CodigoMnemONico = v.CodigONemONico 
					 AND v.SituaciON = 'A'  
	 JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = left(ctv.CodigoSBS,2) 
							  AND ti.SituaciON = 'A'  
	 JOIN 
		ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento 
							   AND CI.Categoria NOT IN ( 'DP' ,'OR' )--10328  
	 JOIN 
		MONeda m ON v.CodigoMONeda = m.CodigoMONeda 
					AND m.SituaciON = 'A'  
	 JOIN 
		Entidad e ON v.CodigoEmisor = e.CodigoEntidad 
					 AND e.SituaciON = 'A'  
	 LEFT JOIN 
		TipoInstrumentoHomologacion TIH ON TIH.CodigoTipoInstumentoSBS = V.CodigoTipoInstrumentoSBS 
									       AND TIH.CodigoMoneda = V.CodigoMoneda   
	 WHERE 
		(ctv.CodigoPortafolioSBS = @p_codigoPortafolioSbs or @p_codigoPortafolioSbs = '')  
		AND ctv.FechaValoraciON = @fechASecuencial 
		AND ctv.Escenario = 'REAL'  
   
	 --Cartera Valores fecha anterior  
	 INSERT INTO 
		@tCartera  
	 SELECT 
		PORTAFOLIO = p.DescripciON,GRUPO = ISNULL(ti.CodigoTipoInstrumentoSBS,'') + '-' + dbo.FN_SIT_OBT_CodigoMonedaSBS(p.CodigoMoneda),  
		DESCRIPCION = ISNULL(TIH.Descripcion, 
		ti.DescripciON),
		MONEDA = m.CodigoMONeda,
		EMISION = ctv.CodigoMnemONico,  
		ENTIDAD = e.CodigoEntidad,
		CODIGO_SBS = ctv.CodigoSBS,
		CODIGO_ISIN = v.CodigoISIN,
		FECHA = @fechASecuencial,
		FECHA_EMISION = ISNULL(dbo.FormatDate(v.FechaEmisiON),''),  
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.FechaValoraciON),''),
		NRO_TITULO = ctv.SecuenciaValorizaciON,
		TITULO = ctv.CodigoMnemONico,  
		--PRECIO = dbo.RetornarVectorPrecioT_1(v.CodigONemONico,P.VectorPrecioVal,ctv.FechaValoraciON),  
		PRECIO = dbo.RetornarSecuenciaVectorPrecio(ctv.CodigoPortafolioSBS,v.CodigoNemonico,ctv.FechaValoraciON),  
		CANTIDAD = ctv.Cantidad,
		MONTO =  0 ,
		VPN_HOY = 0,  
		VPN_AYER = ctv.VPNMonedaFondo,
		COMPRAS = 0,
		VENTAS = 0,
		VENCIMIENTOS = 0,
		AMORTIZACIONES = 0,  
		INTERESES = 0,
		DIVIDENDOS = 0,
		RENTABILIDAD = 0  
	 FROM 
		CarteraTituloValoraciON ctv  
	 JOIN 
		Portafolio p ON ctv.CodigoPortafolioSBS = p.CodigoPortafolioSBS  
	 JOIN 
		Valores v ON ctv.CodigoMnemONico = v.CodigONemONico AND v.SituaciON = 'A'  
	 JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = left(ctv.CodigoSBS,2) AND ti.SituaciON = 'A'  
	 JOIN 
		ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento AND CI.Categoria NOT IN ( 'DP' ,'OR' )--10328  
	 JOIN 
		MONeda m ON v.CodigoMONeda = m.CodigoMONeda 
					AND m.SituaciON = 'A'  
	 JOIN 
		Entidad e ON v.CodigoEmisor = e.CodigoEntidad 
					AND e.SituaciON = 'A'  
	 LEFT JOIN 
		TipoInstrumentoHomologacion TIH ON TIH.CodigoTipoInstumentoSBS = V.CodigoTipoInstrumentoSBS 
										   AND TIH.CodigoMoneda = V.CodigoMoneda   
	 WHERE 
		(ctv.CodigoPortafolioSBS = @p_codigoPortafolioSbs or @p_codigoPortafolioSbs = '')  
		 AND ctv.FechaValoraciON = @fechaAnterior 
		 AND ctv.Escenario = 'REAL'  
		 AND NOT EXISTS(SELECT 1 FROM @tCartera WHERE fecha = @fechASecuencial AND CodigoSBS = ctv.CodigoSBS and portafolio = p.Descripcion)  
   
 --Se obtienen los movimientos de lAS operaciONes del día  
	 INSERT INTO 
		@t_Inversiones_prev  
	 SELECT  
		 Portafolio = p.DescripciON,
		 codigoSbs = CASE WHEN oi.CodigoOperacion = '6' THEN (SELECT CodigoSBS FROM OrdenInversion WHERE CodigoOrden = oi.OrdenGenera) ELSE oi.CodigoSBS END,
		 fecha = oi.FechaOperaciON,
		 comprAS = ISNULL((CASE WHEN oi.CodigoOperaciON in ('1','3','61','63') 
								THEN (CASE WHEN ci.CodigoClASeInstrumento = '2'  
								           THEN oi.MONtONominalOperaciON  
								           ELSE (CASE WHEN oi.CodigoMnemONico='MPLE LN' THEN MONtoDestino ELSE MONtoOperaciON END) END)END),0),
		 ventAS = ISNULL((CASE WHEN oi.CodigoOperaciON in ('2','62','64') 
							   THEN (CASE WHEN oi.CodigoMnemONico='MPLE LN' 
							              THEN oi.MONtoDestino 
										  ELSE oi.MONtoOperaciON END)END),0),
		vencimientos = CASE WHEN oi.CodigoOperaciON = '4' or oi.CodigoOperaciON = '6' or (oi.CodigoOperaciON= '39' 
		                           AND abs(dbo.DiAS(oi.FechaOperaciON,v.FechaVencimiento))  <= 2)  
							 THEN oi.MONtoOperaciON ELSE 0 END,
		amortizaciONes = CASE WHEN oi.CodigoOperaciON = '38' THEN oi.MONtoOperaciON ELSE 0 END,  
		intereses = CASE WHEN oi.CodigoOperaciON = '35' or (CodigoOperaciON= '39' AND abs(dbo.DiAS(oi.FechaOperaciON,v.FechaVencimiento)) > 2) THEN oi.MONtoOperaciON ELSE 0 END,
		dividENDos = CASE WHEN oi.CodigoOperaciON = '36' THEN oi.MONtoOperaciON ELSE 0 END,
		TipoCalculo = (SELECT m.TipoCalculo 
						FROM dbo.Moneda m 
						WHERE m.CodigoMoneda = CASE WHEN left(CodigoOrden,1) IN ('D','R') THEN oi.CodigoMonedaDestino  ELSE v.CodigoMoneda END),
		 --,tipoCambio = dbo.RetornarTipoCambio(CASE WHEN left(CodigoOrden,1) IN ('D','R') THEN oi.CodigoMonedaDestino ELSE v.CodigoMoneda END,oi.FechaOperacion)  
		 tipoCambio = dbo.RetornarSecuenciaVectorTipoCambio(oi.CodigoPortafolioSBS,(CASE WHEN left(CodigoOrden,1) IN ('D','R') THEN oi.CodigoMonedaDestino ELSE v.CodigoMoneda END ),oi.FechaOperacion),
		 tipoCambioNeg = dbo.RetornarSecuenciaVectorTipoCambio(oi.CodigoPortafolioSBS,'DOL',oi.FechaOperacion),
		 p.CodigoMoneda,
		 oi.CodigoMoneda
	 FROM 
		OrdenInversiON oi  
	 JOIN 
		Portafolio p ON oi.CodigoPortafolioSBS = p.CodigoPortafolioSBS  
	 JOIN 
		ClASeInstrumento ci ON oi.CategoriaInstrumento = ci.Categoria  
	 JOIN 
		Valores v ON oi.CodigoMnemONico = v.CodigONemONico  
	 WHERE 
		(oi.CodigoPortafolioSBS = @p_codigoPortafolioSbs or @p_codigoPortafolioSbs = '') 
		 AND (oi.FechaOperaciON between @fechaInicioMes AND @p_fechaOperaciON)  
		 AND oi.SituaciON = @p_estado  
		 AND oi.Estado='E-CON'  
		 AND oi.TipoFONdo <> 'CC_SNC'  
		 AND p.SituaciON = @p_estado  
		 AND ci.SituaciON = @p_estado  
		 AND v.SituaciON = @p_estado  
   
	 --Se actualizan los movimientos en la tabla @tCartera    
	 DECLARE 
		@v_CodigoMonedaPortafolio VARCHAR(10) = (SELECT p.CodigoMoneda FROM dbo.Portafolio p WHERE p.CodigoPortafolioSBS = @p_codigoPortafolioSbs)  

	 IF @v_CodigoMonedaPortafolio = 'NSOL'  
	 BEGIN  
		UPDATE 
			@tCartera  
		SET 
			comprAS = c.comprAS + ISNULL((SELECT SUM(comprAS * (CASE WHEN MonedaNegociado = 'DOL' THEN tipoCambio ELSE 1 END)) 
											FROM @t_Inversiones_prev   
											WHERE portafolio = c.portafolio 
												AND codigoSbs = c.codigoSbs 
												AND fecha = c.fecha),i.comprAS * (CASE WHEN MonedaNegociado = 'DOL' THEN i.tipoCambio ELSE 1 END)),  
			ventAS = c.ventAS + ISNULL((SELECT SUM(ventAS * (CASE WHEN MonedaNegociado = 'DOL' THEN tipoCambio ELSE 1 END)) 
										FROM @t_Inversiones_prev   
										WHERE portafolio = c.portafolio 
											  AND codigoSbs = c.codigoSbs 
											  AND fecha = c.fecha),i.ventAS * (CASE WHEN MonedaNegociado = 'DOL' THEN i.tipoCambio ELSE 1 END)), 
			vencimientos = c.vencimientos + ISNULL((SELECT SUM(vencimientos * (CASE WHEN MonedaNegociado = 'DOL' THEN tipoCambio ELSE 1 END)) 
													FROM @t_Inversiones_prev   
													WHERE portafolio = c.portafolio 
														  AND codigoSbs = c.codigoSbs 
														  AND fecha = c.fecha),i.vencimientos * (CASE WHEN MonedaNegociado = 'DOL' THEN i.tipoCambio ELSE 1 END)),  
			amortizaciONes = c.amortizaciONes + ISNULL((SELECT SUM(amortizaciONes * (CASE WHEN MonedaNegociado = 'DOL' THEN tipoCambio ELSE 1 END)) 
														FROM @t_Inversiones_prev   
														WHERE portafolio = c.portafolio 
														      AND codigoSbs = c.codigoSbs 
															  AND fecha = c.fecha),i.amortizaciONes * (CASE WHEN MonedaNegociado = 'DOL' THEN i.tipoCambio ELSE 1 END)),  
			intereses = c.intereses + ISNULL((SELECT SUM(intereses * (CASE WHEN MonedaNegociado = 'DOL' THEN tipoCambio ELSE 1 END)) 
											 FROM @t_Inversiones_prev   
											 WHERE portafolio = c.portafolio 
												   AND codigoSbs = c.codigoSbs 
												   AND fecha = c.fecha),i.intereses * (CASE WHEN MonedaNegociado = 'DOL' THEN i.tipoCambio ELSE 1 END)),  
			dividENDos = c.dividENDos + ISNULL((SELECT SUM(dividENDos * (CASE WHEN MonedaNegociado = 'DOL' THEN tipoCambio ELSE 1 END)) 
												FROM @t_Inversiones_prev   
												WHERE portafolio = c.portafolio 
													  AND codigoSbs = c.codigoSbs 
													  AND fecha = c.fecha),i.dividENDos * (CASE WHEN MonedaNegociado = 'DOL' THEN i.tipoCambio ELSE 1 END))  
		FROM 
			@tCartera c  
		JOIN 
			@t_Inversiones_prev i ON c.portafolio = i.portafolio 
									 AND c.codigoSbs = i.codigoSbs 
									 AND c.fecha = i.fecha  
	 END  
	 ELSE  
	 BEGIN  
		UPDATE 
			@tCartera  
		SET 
			comprAS = c.comprAS + ISNULL((SELECT SUM(comprAS/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(TipoCambioNeg,1) ELSE 1 END)) 
										  FROM @t_Inversiones_prev   
										  WHERE portafolio = c.portafolio 
										        AND codigoSbs = c.codigoSbs 
												AND fecha = c.fecha),i.comprAS/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(i.TipoCambioNeg,1) ELSE 1 END)),  
			ventAS = c.ventAS + ISNULL((SELECT SUM(ventAS/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(TipoCambioNeg,1) ELSE 1 END)) 
									    FROM @t_Inversiones_prev   
										WHERE portafolio = c.portafolio 
											  AND codigoSbs = c.codigoSbs 
											  AND fecha = c.fecha),i.ventAS/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(i.TipoCambioNeg,1) ELSE 1 END)), 
			vencimientos = c.vencimientos + ISNULL((SELECT SUM(vencimientos/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(TipoCambioNeg,1) ELSE 1 END)) 
													FROM @t_Inversiones_prev   
													WHERE portafolio = c.portafolio 
													      AND codigoSbs = c.codigoSbs 
														  AND fecha = c.fecha),i.vencimientos/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(i.TipoCambioNeg,1) ELSE 1 END)),  
			amortizaciONes = c.amortizaciONes + ISNULL((SELECT SUM(amortizaciONes/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(TipoCambioNeg,1) ELSE 1 END)) 
													    FROM @t_Inversiones_prev   
														WHERE portafolio = c.portafolio 
														      AND codigoSbs = c.codigoSbs 
															  AND fecha = c.fecha),i.amortizaciONes/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(i.TipoCambioNeg,1) ELSE 1 END)),  
			intereses = c.intereses + ISNULL((SELECT SUM(intereses/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(TipoCambioNeg,1) ELSE 1 END)) 
											  FROM @t_Inversiones_prev  
											  WHERE portafolio = c.portafolio 
													AND codigoSbs = c.codigoSbs 
													AND fecha = c.fecha),i.intereses/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(i.TipoCambioNeg,1) ELSE 1 END)),  
			dividENDos = c.dividENDos + ISNULL((SELECT SUM(dividENDos/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(TipoCambioNeg,1) ELSE 1 END)) 
												FROM @t_Inversiones_prev  
												WHERE portafolio = c.portafolio 
												      AND codigoSbs = c.codigoSbs 
													  AND fecha = c.fecha),i.dividENDos/(CASE WHEN MonedaNegociado = 'NSOL' THEN ISNULL(i.TipoCambioNeg,1) ELSE 1 END))  
		FROM 
			@tCartera c  
		JOIN 
			@t_Inversiones_prev i ON c.portafolio = i.portafolio 
			                         AND c.codigoSbs = i.codigoSbs 
									 AND c.fecha = i.fecha  
	 END  
	 --Se calcula la rentabilidad  
	 UPDATE 
		@tCartera  
	 SET 
		rentabilidad = (vpnHoy + ventAS + vencimientos + amortizaciONes + intereses + dividENDos) - (vpnAyer + comprAS)  
	 --Cartera  
	 SELECT 
		portafolio,
		grupo,
		descripciON,
		mONeda,
		emisiON,
		entidad,
		codigoSbs,
		codigoIsin,
		(CASE WHEN fechaEmisiON= '//0' THEN fechaValoraciON ELSE fechaEmisiON END) fechaEmisiON,
		fechaValoraciON, 
		ISNULL(nroTitulo,1) nroTitulo,  
		CASE WHEN titulo = 'FORWARD' THEN codigoSbs ELSE titulo END,
		precio,
		cantidad,
		vpnHoy, --10362  
		vpnAyer,
		comprAS,
		ventAS,
		vencimientos,
		amortizaciONes,
		intereses,
		dividENDos,   
		CASE WHEN rentabilidad > 0 THEN '+' + CONVERT(VARCHAR(100), rentabilidad) ELSE CONVERT(VARCHAR(100), rentabilidad)  END rentabilidad  
	 FROM 
		@tCartera 
	ORDER BY
		codigoSbs  
END  
GO

GRANT EXECUTE ON [dbo].[Pr_Sit_Rpt_Rentabilidad] TO [rol_sit_fondos] AS [dbo]
GO



PRINT 'Fin - Pr_Sit_Rpt_Rentabilidad'
GO

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 