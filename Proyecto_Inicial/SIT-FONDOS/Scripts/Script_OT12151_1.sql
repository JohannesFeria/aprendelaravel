-----------------------------------------------------------------------------------------------------
--   Objetivo: Crear script que se encargue de lo siguiente: 
--		1. Configurar los estados de las solicitudes de reversion.
--   Par?metros: No aplica
-----------------------------------------------------------------------------------------------------
-- Fecha de Modificación: 25/07/2019
-- Modificado por: Robert Castillo
-- Nro. Orden de Trabajo: OT12151
-- Descripción del cambio: Creación del script
-----------------------------------------------------------------------------------------------------	
use [SIT-FONDOS]
go

BEGIN TRANSACTION Transaction_Log_1
/* Inicio Secuencias SQL */

	DELETE from ParametrosGenerales where clasificacion='USUARIO_APRUEBA_SOLICITUD_REVERSION'
	INSERT INTO ParametrosGenerales
	(ClasIFicacion,
	Nombre,
	Valor,
	Comentario
	)VALUES
	(
	'USUARIO_APRUEBA_SOLICITUD_REVERSION',
	'CODIGO_DOMINIO',
	'P500652',
	''
	)
	
	DELETE from ParametrosGenerales where clasificacion='SOLICITUD_REVERSION_EMAIL'
	INSERT INTO ParametrosGenerales
	(ClasIFicacion,
	Nombre,
	Valor,
	Comentario
	)VALUES
	(
	'SOLICITUD_REVERSION_EMAIL',	
	'PARA',
	'reversionvc@sura.pe',
	''
	)

	DELETE from ParametrosGenerales where clasificacion='APROBACION_SOLICITUD_REVERSION_EMAIL'
	INSERT INTO ParametrosGenerales
	(ClasIFicacion,
	Nombre,
	Valor,
	Comentario
	)VALUES
	(
	'APROBACION_SOLICITUD_REVERSION_EMAIL',	
	'PARA',
	'reversionvc@sura.pe',
	''
	)
	
	DECLARE @IP_Address varchar(255);    
	SELECT @IP_Address = client_net_address    FROM sys.dm_exec_connections    WHERE Session_id = @@SPID;    
	
	IF (SELECT COUNT(ArchivoCodigo) FROM ArchivoPlano WITH(NOLOCK) WHERE ArchivoCodigo  = '029') = 0
	BEGIN
		INSERT INTO ArchivoPlano
		VALUES('029','SolicitudReversion','Archivo para envio de correo de Solicitud Reversion','html','\\SPPEAPP00023\SIT_Fondos\Datos\Plantillas\Correo\','','',0,'',0,0,SYSTEM_USER,CONVERT(VARCHAR(10), GETDATE(), 112),convert(varchar, getdate(), 108),'',NULL,NULL,@IP_Address,'','','')
	END
	
	IF (SELECT COUNT(ArchivoCodigo) FROM ArchivoPlano WITH(NOLOCK) WHERE ArchivoCodigo  = '030') = 0
	BEGIN
		INSERT INTO ArchivoPlano
		VALUES('030','ConfirmacionSolicitudReversion','Archivo para envio de correo de Confirmacion de Solicitud Reversion','html','\\SPPEAPP00023\SIT_Fondos\Datos\Plantillas\Correo\','','',0,'',0,0,SYSTEM_USER,CONVERT(VARCHAR(10), GETDATE(), 112),convert(varchar, getdate(), 108),'',NULL,NULL,@IP_Address,'','','')
	END
	
	/* Fin Secuencias SQL */

IF @@ERROR <> 0
   ROLLBACK TRANSACTION Transaction_Log_1
ELSE
   COMMIT TRANSACTION Transaction_Log_1

/* FIN */
GO
	
