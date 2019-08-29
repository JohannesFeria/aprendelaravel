USE [SIT-FONDOS]
GO

--Creacion de tabla

PRINT 'Creacion tabla SaldosNoAdministrados'
IF EXISTS (SELECT * FROM sysobjects WHERE name='SaldosNoAdministrados') BEGIN 
	DROP TABLE [dbo].[SaldosNoAdministrados]
END 
GO
CREATE TABLE SaldosNoAdministrados
(
	CodigoSaldoNoAdmnistrado varchar(12) PRIMARY KEY,
	CodigoTercero 			 varchar(12),
	Fecha					 numeric(8),
	CodigoTercerFinanciero 	 varchar(12),
	TipoCuenta				 varchar(2),
	Saldo					 Numeric(22,7),
	CodigoMoneda			 varchar(10),
	Situacion				 varchar(1),
	UsuarioCreacion			 varchar(50),
	FechaCreacion			 numeric(8),
	HoraCreacion			 varchar(10),
	UsuarioModificacion		 varchar(50),
	FechaModificacion		 numeric(8),
	HoraModificacion		 varchar(10),
	Host					 varchar(20)
)
GO

PRINT 'Creacion tabla ClaseActivoLimite'
IF EXISTS (SELECT * FROM sysobjects WHERE name='ClaseActivoLimite') BEGIN 
	DROP TABLE [dbo].[ClaseActivoLimite]
END 
GO
CREATE TABLE [dbo].[ClaseActivoLimite]
(
	[CodigoClaseActivoLimite] [varchar](20) NULL,
	[Descripcion] [varchar](100) NULL,
	[Situacion] [char](1) NULL,
	[UsuarioCreacion] [varchar](50) NULL,
	[FechaCreacion] [numeric](8, 0) NULL,
	[HoraCreacion] [varchar](10) NULL,
	[UsuarioModificacion] [varchar](50) NULL,
	[FechaModificacion] [numeric](8, 0) NULL,
	[HoraModificacion] [varchar](10) NULL,
	[Host] [varchar](20) NULL
) ON [PRIMARY]

GO

PRINT 'Creacion tabla ClaseActivoInstrumentoLimite'
IF EXISTS (SELECT * FROM sysobjects WHERE name='ClaseActivoInstrumentoLimite') BEGIN 
	DROP TABLE [dbo].[ClaseActivoInstrumentoLimite]
END 
GO
CREATE TABLE [dbo].[ClaseActivoInstrumentoLimite]
(
	[CodigoClaseActivoInstrumentoLimite] [varchar](20) NULL,
	[Descripcion] [varchar](150) NULL,
	[CodigoClaseActivoLimite] [varchar](20) NULL,
	[FlagInstrumento] [char](1) NULL,
	[FlagSaldo] [char](1) NULL,
	[CodigoTipoInstrumentoSBS] [varchar](100) NULL,
	[Situacion] [char](1) NULL,
	[UsuarioCreacion] [varchar](50) NULL,
	[FechaCreacion] [numeric](8, 0) NULL,
	[HoraCreacion] [varchar](10) NULL,
	[UsuarioModificacion] [varchar](50) NULL,
	[FechaModificacion] [numeric](8, 0) NULL,
	[HoraModificacion] [varchar](10) NULL,
	[Host] [varchar](20) NULL
) ON [PRIMARY]

GO


PRINT 'Creacion tabla DatosCarta'
IF EXISTS (SELECT * FROM sysobjects WHERE name='DatosCarta') BEGIN 
	DROP TABLE [dbo].[DatosCarta]
END 
GO

CREATE TABLE DatosCarta
(
	CodigoDatosCarta int identity(1,1),
	CodigoTipoDato CHAR(1),
	CodigoTercero VARCHAR(20),
	CodigoMoneda VARCHAR(5),
	Valor VARCHAR(50)
)
GO

PRINT 'Creacion tabla ObservacionCarta'
IF EXISTS (SELECT * FROM sysobjects WHERE name='ObservacionCarta') BEGIN 
	DROP TABLE [dbo].[ObservacionCarta]
END 
GO

CREATE TABLE ObservacionCarta
(
	CodigoAgrupacion	int Primary key,
	Observacion varchar(2000),
	Situacion	varchar(1),
	UsuarioCreacion	varchar(15),
	FechaCreacion	numeric(8),
	HoraCreacion	varchar(10),
	UsuarioModificacion	varchar(15),
	FechaModificacion	numeric(8),
	HoraModificacion	varchar(10),
	Host	varchar(20)
)
GO


PRINT 'Creacion Vista VT_ClaseActivoLimiteInstrumento'
IF EXISTS (SELECT * FROM sysobjects WHERE name='VT_ClaseActivoLimiteInstrumento') BEGIN 
	DROP VIEW [dbo].[VT_ClaseActivoLimiteInstrumento]
END 
GO
CREATE VIEW [dbo].[VT_ClaseActivoLimiteInstrumento]
AS
	SELECT CodigoClaseActivoInstrumentoLimite AS Codigo,
		   Descripcion,
		   CodigoClaseActivoLimite
	FROM ClaseActivoInstrumentoLimite
	WHERE Situacion = 'A'

GO


PRINT 'Creacion Vista VT_ClaseActivoLimite'
IF EXISTS (SELECT * FROM sysobjects WHERE name='VT_ClaseActivoLimite') BEGIN 
	DROP VIEW [dbo].[VT_ClaseActivoLimite]
END 
GO
CREATE VIEW [dbo].[VT_ClaseActivoLimite] 
AS
	SELECT CodigoClaseActivoLimite AS Codigo,
		   Descripcion
	FROM ClaseActivoLimite
	WHERE Situacion = 'A'

GO


--Agregar campos
PRINT 'Nueva columna SWIFT en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('SWIFT'))
ALTER TABLE Terceros ADD SWIFT varchar(100) null
GO

PRINT 'Nueva columna ForFurtherCredit en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('ForFurtherCredit'))
ALTER TABLE Terceros ADD ForFurtherCredit varchar(100) null
GO

PRINT 'Nueva columna ABA en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('ABA'))
ALTER TABLE Terceros ADD ABA varchar(100) null
GO

PRINT 'Nueva columna BancoDestino en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('BancoDestino'))
ALTER TABLE Terceros ADD BancoDestino varchar(100) null
GO

PRINT 'Nueva columna NumeroCuentaDestino en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('NumeroCuentaDestino'))
ALTER TABLE Terceros ADD NumeroCuentaDestino varchar(100) null
GO

PRINT 'Nueva columna CiudadDestino en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('CiudadDestino'))
ALTER TABLE Terceros ADD CiudadDestino varchar(50) null
GO

PRINT 'Nueva columna PaisDestino en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('PaisDestino'))
ALTER TABLE Terceros add PaisDestino varchar(100)
GO

PRINT 'Nueva columna SwiftIntermediario en Terceros'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Terceros]') and upper(name) = upper('SwiftIntermediario'))
ALTER TABLE Terceros ADD SwiftIntermediario VARCHAR(100)
GO


PRINT 'Nueva columna CodigoCaracteristicaRelacion en ReporteLimites'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ReporteLimites]') and upper(name) = upper('CodigoCaracteristicaRelacion'))
ALTER TABLE ReporteLimites ADD CodigoCaracteristicaRelacion varchar(100)
GO

PRINT 'Nueva columna CodigoRelacion en ReporteLimites'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ReporteLimites]') and upper(name) = upper('CodigoRelacion'))
ALTER TABLE ReporteLimites ADD CodigoRelacion varchar(100)
GO


PRINT 'Nueva columna Consolidado en Portafolio'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Portafolio]') and upper(name) = upper('Consolidado'))
ALTER TABLE Portafolio ADD Consolidado int
GO


PRINT 'Nueva columna NumeroCuenta en OrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion]') and upper(name) = upper('NumeroCuenta'))
ALTER TABLE OrdenInversion ADD NumeroCuenta varchar(25)
GO

PRINT 'Nueva columna DTC en OrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion]') and upper(name) = upper('DTC'))
ALTER TABLE OrdenInversion ADD DTC Varchar(5)
GO

PRINT 'Nueva columna CodigoModelo en OrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion]') and upper(name) = upper('CodigoModelo'))
ALTER TABLE OrdenInversion ADD CodigoModelo Varchar(5)
GO

PRINT 'Nueva columna ObservacionCarta en OrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion]') and upper(name) = upper('ObservacionCarta'))
ALTER TABLE OrdenInversion ADD ObservacionCarta varchar(2000)
GO

PRINT 'Nueva columna CodigoAgrupado en OrdenInversion'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion]') and upper(name) = upper('CodigoAgrupado'))
ALTER TABLE OrdenInversion ADD CodigoAgrupado  varchar(10)
GO


PRINT 'Nueva columna Tipo en ValidadorLimiteDetalle'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ValidadorLimiteDetalle]') and upper(name) = upper('Tipo'))
ALTER TABLE ValidadorLimiteDetalle ADD Tipo CHAR(1)
GO


PRINT 'Nueva columna CodigoCaracteristicaRelacion en detallenivellimite2'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[detallenivellimite2]') and upper(name) = upper('CodigoCaracteristicaRelacion'))
ALTER TABLE detallenivellimite2 ADD CodigoCaracteristicaRelacion VARCHAR(5)
GO

PRINT 'Nueva columna CodigoRelacion en detallenivellimite2'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[detallenivellimite2]') and upper(name) = upper('CodigoRelacion'))
ALTER TABLE detallenivellimite2 ADD CodigoRelacion VARCHAR(5)
GO


PRINT 'Nueva columna CodigoRelacion en NivelLimite'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[NivelLimite]') and upper(name) = upper('CodigoRelacion'))
ALTER TABLE NivelLimite ADD CodigoRelacion VARCHAR(5)
GO


--Quitar columna
-- PRINT 'eliminar campo del limite'
-- IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[limite]') and upper(name) = upper('TipoCuenta'))
-- alter table limite drop column TipoCuenta 
-- GO
--Nuevo Registros.

PRINT 'Nuevo en ParametrosGenerales 1'
IF NOT EXISTS (SELECT * FROM ParametrosGenerales WHERE Clasificacion = 'TipoCuenta' and Nombre ='AH')
INSERT INTO ParametrosGenerales(Clasificacion,Nombre,Valor,Comentario)
VALUES('TipoCuenta','AH','1','Cuenta Ahorro')
GO

PRINT 'Nuevo en ParametrosGenerales 2'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'TipoCuenta' and Nombre ='CC')
INSERT INTO ParametrosGenerales(Clasificacion,Nombre,Valor,Comentario)
VALUES('TipoCuenta','CC','2','Cuenta Corriente')
GO

PRINT 'Nuevo en ParametrosGenerales 3'
IF NOT EXISTS (SELECT * FROM ParametrosGenerales WHERE Clasificacion = 'DATOSFONDOLIMITE' and Nombre ='DTC')
INSERT INTO ParametrosGenerales(Clasificacion,Nombre,Valor)
VALUES('DATOSFONDOLIMITE','DTC','1')
GO

PRINT 'Nuevo en ParametrosGenerales 4'
IF NOT EXISTS (SELECT * FROM ParametrosGenerales WHERE Clasificacion = 'DATOSFONDOLIMITE' and Nombre ='Euroclear')
INSERT INTO ParametrosGenerales(Clasificacion,Nombre,Valor)
VALUES('DATOSFONDOLIMITE','Euroclear','2')
GO

PRINT 'Nuevo en ParametrosGenerales 5'
IF NOT EXISTS (SELECT * FROM ParametrosGenerales WHERE Clasificacion = 'DATOSFONDOLIMITE' and Nombre ='Euroclear')
INSERT INTO ParametrosGenerales(Clasificacion,Nombre,Valor)
VALUES('DATOSFONDOLIMITE','Cedel','3')
GO

PRINT 'Nuevo en ParametrosGenerales 6'
IF NOT EXISTS (SELECT * FROM ParametrosGenerales WHERE Clasificacion = 'DATOSFONDOLIMITE' and Nombre ='Euroclear')
INSERT INTO ParametrosGenerales(Clasificacion,Nombre,Valor)
VALUES('DATOSFONDOLIMITE','Crest','4')
GO

PRINT 'Nuevo en ParametrosGenerales 7'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'DetalleLimite' and Nombre ='Validador' and valor = '1')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor)
VALUES ('DetalleLimite','Validador','1')
GO

PRINT 'Nuevo en ParametrosGenerales 8'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'DetalleLimite' and Nombre ='TipoCuenta' and valor = '2')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor)
VALUES ('DetalleLimite','TipoCuenta','2')
GO

PRINT 'Nuevo en ParametrosGenerales 9'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '25')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','25','OTROS BONOS DEL SISTEMA FINANCIERO',1)
GO

PRINT 'Nuevo en ParametrosGenerales 10'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '01')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','01','BONOS GOBIERNO CENTRAL REPUBLICA',2)
GO

PRINT 'Nuevo en ParametrosGenerales 11'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '22')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','22','BONOS DE EMPRESAS PRIVADAS',3)
GO

PRINT 'Nuevo en ParametrosGenerales 12'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '15')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','15','BONOS SISTEMA FINANC EXTERIOR',4)
GO

PRINT 'Nuevo en ParametrosGenerales 13'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '08')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','08','TITULOS ESTADOS DEL EXTRANJERO',5)
GO

PRINT 'Nuevo en ParametrosGenerales 14'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '20')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','20','BONOS DE ARRENDAMIENTO SISTEMA FINANCIER',6)
GO

PRINT 'Nuevo en ParametrosGenerales 15'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoBonos' and Comentario ='Bonos' and Valor = '21')
INSERT ParametrosGenerales(Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoBonos','Bonos','21','BONOS SUBORDINADOS SISTEMA FINANCIERO',7)
GO

PRINT 'Nuevo en ParametrosGenerales 16'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoLetras' and Comentario ='Letras' and Valor = '04')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoLetras','Letras','04','LETRAS DEL TESORO PERUANO',8)
GO

PRINT 'Nuevo en ParametrosGenerales 17'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoPapelesComerciales' and Comentario ='Papeles Comerciales' and Valor = '23')
INSERT ParametrosGenerales (Clasificacion,Comentario,Valor,Nombre,Orden)
VALUES ('ClaseActivoPapelesComerciales','Papeles Comerciales','23','PAPELES COMERCIALES',9)
GO

PRINT 'Nuevo en ParametrosGenerales 18'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertDeposito' and Nombre ='CERTIFICADOS DE DEPOSITOS DE LARGO PLAZO' and Valor = '63')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertDeposito','CERTIFICADOS DE DEPOSITOS DE LARGO PLAZO','63','Certificados de depósitos',10)
GO

PRINT 'Nuevo en ParametrosGenerales 19'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertDeposito' and Nombre ='CERTIFICADOS DEPOSITO  BCR' and Valor = '64')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertDeposito','CERTIFICADOS DEPOSITO  BCR','64','Certificados de depósitos',11)
GO

PRINT 'Nuevo en ParametrosGenerales 20'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertDeposito' and Nombre ='CERTIFICADOS DE DEPOSITO A PLAZO EXTRANJERO' and Valor = '67')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertDeposito','CERTIFICADOS DE DEPOSITO A PLAZO EXTRANJERO','67','Certificados de depósitos',12)
GO

--- Instrumentos titulizados o instrumentos de deuda emitido por fideicomisos
GO

PRINT 'Nuevo en ParametrosGenerales 21'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoInst' and Nombre ='BONOS TITULIZADOS HIPOTECARIOS' and Valor = '47')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoInst','BONOS TITULIZADOS HIPOTECARIOS','47','Instrumentos titulizados o instrumentos de deuda emitido por fideicomisos',13)
GO

PRINT 'Nuevo en ParametrosGenerales 22'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoInst' and Nombre ='TITULOS DE CONTENIDO CREDITICIO' and Valor = '27')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoInst','TITULOS DE CONTENIDO CREDITICIO','27','Instrumentos titulizados o instrumentos de deuda emitido por fideicomisos',14)
GO

PRINT 'Nuevo en ParametrosGenerales 23'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertParticipacion' and Nombre ='FONDO MUTUO' and Valor = '50')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertParticipacion','FONDO MUTUO','50','Certificados de participación en fondos mutuos y fondos de inversión',15)
GO

PRINT 'Nuevo en ParametrosGenerales 24'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertParticipacion' and Nombre ='FONDO DE INVERSION' and Valor = '51')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertParticipacion','FONDO DE INVERSION','51','Certificados de participación en fondos mutuos y fondos de inversión',16)
GO

PRINT 'Nuevo en ParametrosGenerales 25'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertParticipacion' and Nombre ='ETF' and Valor = '54')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertParticipacion','ETF','54','Certificados de participación en fondos mutuos y fondos de inversión',17)
GO

PRINT 'Nuevo en ParametrosGenerales 26'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertParticipacion' and Nombre ='FONDOS MUTUOS EXTRANJEROS' and Valor = '52')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertParticipacion','FONDOS MUTUOS EXTRANJEROS','52','Certificados de participación en fondos mutuos y fondos de inversión',18)
GO

PRINT 'Nuevo en ParametrosGenerales 27'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoCertParticipacion' and Nombre ='FONDO DE INVERSION TRADICIONAL' and Valor = '49')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoCertParticipacion','FONDO DE INVERSION TRADICIONAL','49','Certificados de participación en fondos mutuos y fondos de inversión',19)
GO

-- ACCIONES


PRINT 'Nuevo en ParametrosGenerales 28'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoAcciones' and Nombre ='ACCIONES COMUNES' and Valor = '30')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoAcciones','ACCIONES COMUNES','30','ACCIONES',20)
GO

PRINT 'Nuevo en ParametrosGenerales 29'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoAcciones' and Nombre ='ACCIONES DE INVERSIÓN' and Valor = '31')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoAcciones','ACCIONES DE INVERSIÓN','31','ACCIONES',21)
GO

PRINT 'Nuevo en ParametrosGenerales 30'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoAcciones' and Nombre ='ADRS DE EMPRESAS PERUANAS' and Valor = '32')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoAcciones','ADRS DE EMPRESAS PERUANAS','32','ACCIONES',22)
GO

PRINT 'Nuevo en ParametrosGenerales 31'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoAcciones' and Nombre ='ACCIONES DEL EXTRANJERO' and Valor = '34')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoAcciones','ACCIONES DEL EXTRANJERO','34','ACCIONES',23)
GO

PRINT 'Nuevo en ParametrosGenerales 32'
IF NOT EXISTS (SELECT * FROM [ParametrosGenerales] WHERE Clasificacion = 'ClaseActivoAcciones' and Nombre ='OPERACIONES DE REPORTE CON ACCIONES DEL TRABAJO' and Valor = '81')
INSERT ParametrosGenerales (Clasificacion,Nombre,Valor,Comentario,Orden)
VALUES ('ClaseActivoAcciones','OPERACIONES DE REPORTE CON ACCIONES DEL TRABAJO','81','ACCIONES',24)
GO



PRINT 'Nuevo Registro CLIENTES MANDATOS en TIPOTERCERO'
IF NOT EXISTS(SELECT * FROM TIPOTERCERO WHERE CodigoTipoTercero = 'CM')
INSERT INTO TIPOTERCERO (CodigoTipoTercero, Descripcion, Situacion, UsuarioCreacion,FechaCreacion,HoraCreacion,Host)
values ('CM','CLIENTES MANDATOS','A','SYSTEM',20190614,'10:00:00','192.168.0.50')

  
  
PRINT 'Nuevo Registro OPERACIONES DE REPORTE en ModeloCarta'
IF NOT EXISTS(SELECT * FROM ModeloCarta WHERE CodigoModelo = 'VNOR' and CodigoOperacion = '4')
INSERT INTO ModeloCarta (CodigoModelo,CodigoOperacion,Descripcion,ArchivoPlantilla,UsuarioValidador1,UsuarioValidador2,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,UsuarioModificacion,Host,NumeroCartas)
VALUES('VNOR','4','RENOVACION OPERACIONES DE REPORTE','Modulos\ModelosCarta\rpt_PlantillaCartas17.rdlc','H17759','H02337','A','20150622','20190607','14:33:58','ADMIN_TEST','10.219.66.19',1)

PRINT 'Nuevo Registro OPERACIONES DE REPORTE en ModeloCarta'
IF NOT EXISTS(SELECT * FROM ModeloCarta WHERE CodigoModelo = 'VNOR' and CodigoOperacion = '101')
INSERT INTO ModeloCarta (CodigoModelo,CodigoOperacion,Descripcion,ArchivoPlantilla,UsuarioValidador1,UsuarioValidador2,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,UsuarioModificacion,Host,NumeroCartas)
VALUES('VNOR','101','RENOVACION OPERACIONES DE REPORTE','Modulos\ModelosCarta\rpt_PlantillaCartas17.rdlc','H17759','H02337','A','20150622','20190607','14:33:58','ADMIN_TEST','10.219.66.19',1)

PRINT 'Bono Nacional/Exterior en ModeloCarta'
IF NOT EXISTS (SELECT * FROM [ModeloCarta] WHERE CodigoModelo = 'BO01' and CodigoOperacion = '1')
INSERT INTO ModeloCarta (CodigoModelo, CodigoOperacion,Descripcion,ArchivoPlantilla,UsuarioValidador1,UsuarioValidador2,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,Host,NumeroCartas)
VALUES('BO01','1','Bono Nacional/Exterior','Modulos\ModelosCarta\rpt_PlantillaCartas19.rdlc','H17759','H02337','A','SYSTEM',20190605,'11:27:00','127.0.0.1',1)
GO


PRINT 'Bono Nacional/Exterior en ModeloCarta'
IF NOT EXISTS (SELECT * FROM [ModeloCarta] WHERE CodigoModelo = 'BO01' and CodigoOperacion = '2')
INSERT INTO ModeloCarta (CodigoModelo, CodigoOperacion,Descripcion,ArchivoPlantilla,UsuarioValidador1,UsuarioValidador2,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,Host,NumeroCartas)
VALUES('BO01','2','Bono Nacional/Exterior','Modulos\ModelosCarta\rpt_PlantillaCartas19.rdlc','H17759','H02337','A','SYSTEM',20190605,'11:27:00','127.0.0.1',1)
GO


PRINT 'Nuevo Registro Transferencia al Exterior en ModeloCarta'
IF NOT EXISTS (SELECT * FROM [ModeloCarta] WHERE CodigoModelo = 'TE01')
INSERT INTO ModeloCarta (CodigoModelo, CodigoOperacion,Descripcion,ArchivoPlantilla,UsuarioValidador1,UsuarioValidador2,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,Host,NumeroCartas)
VALUES('TE01','63','Transferencia al Exterior','Modulos\ModelosCarta\rpt_PlantillaCartas20.rdlc','H17759','H02337','A','SYSTEM',20190605,'16:36:00','127.0.0.1',1)
GO


--JEFFERIES LLC
PRINT 'INSERTAR DatosCarta JEFFERIES LLC'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='08051826333')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','019','08051826333','NSOL')
GO

PRINT 'INSERTAR DatosCarta JEFFERIES LLC'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='08051826333')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','90793','08051826333','DOL')
GO


--MARKET AXESS
PRINT 'INSERTAR DatosCarta MARKET AXESS'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='120717')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0443','120717','NSOL')
GO

PRINT 'INSERTAR DatosCarta MARKET AXESS'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='120717')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','98730','120717','DOL')
GO

--FYNSA FINANZAS Y NEGOCIOS CORREDORES
PRINT 'INSERTAR DatosCarta FYNSA FINANZAS Y NEGOCIOS CORREDORES'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='117587666')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0443','117587666','NSOL')
GO

PRINT 'INSERTAR DatosCarta FYNSA FINANZAS Y NEGOCIOS CORREDORES'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='117587666')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','98730','117587666','DOL')
GO

--BTG PACTUAL US CAPITAL LLC
PRINT 'INSERTAR DatosCarta BTG PACTUAL US CAPITAL LLC'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='0801201919')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','1970','0801201919','NSOL')
GO

PRINT 'INSERTAR DatosCarta BTG PACTUAL US CAPITAL LLC'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='0801201919')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','96941','0801201919','DOL')
GO

--INVEX, INC.
PRINT 'INSERTAR DatosCarta INVEX, INC.'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='121216')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0443','121216','NSOL')
GO

PRINT 'INSERTAR DatosCarta INVEX, INC.'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='121216')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','98730','121216','DOL')
GO

--LARRAIN VIAL S.A.
PRINT 'INSERTAR DatosCarta LARRAIN VIAL S.A.'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='987654321')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','1970 G21691','987654321','NSOL')
GO

PRINT 'INSERTAR DatosCarta LARRAIN VIAL S.A.'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='987654321')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','43523','987654321','DOL')
GO

--MORGAN STANLEY SMITH
PRINT 'INSERTAR DatosCarta MORGAN STANLEY SMITH'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='1003')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0015','1003','NSOL')
GO

PRINT 'INSERTAR DatosCarta MORGAN STANLEY SMITH'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='1003')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','97375','1003','DOL')
GO

--OPPENHEIMER
PRINT 'INSERTAR DatosCarta OPPENHEIMER'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='10102018010')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','90051','10102018010','DOL')
GO

--XP INVESTMENTS
PRINT 'INSERTAR DatosCarta XP INVESTMENTS'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='XPIN')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','94276','XPIN','DOL')
GO

--CREDICORP CAPITAL
PRINT 'INSERTAR DatosCarta CREDICORP CAPITAL'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='2012')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','901 – 159300','2012','NSOL')
GO

PRINT 'INSERTAR DatosCarta CREDICORP CAPITAL'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='2012')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','47728','2012','DOL')
GO

--GMP Securities, LLC
PRINT 'INSERTAR DatosCarta GMP Securities, LLC'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='61502184524')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','98730','61502184524','DOL')
GO

--MCC SEMINARIO SECURITIES INC.
PRINT 'INSERTAR DatosCarta MCC SEMINARIO SECURITIES INC.'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 3 and CodigoTercero ='20520554310')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('3','26725','20520554310',NULL)
GO

--ADCAP SECURITIES PERU
PRINT 'INSERTAR DatosCarta ADCAP SECURITIES PERU'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='ADCAP')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','79224','ADCAP','DOL')
GO

--BANK OF AMERICA MERRILL LYNCH
PRINT 'INSERTAR DatosCarta BANK OF AMERICA MERRILL LYNCH'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='170419')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','773','170419','NSOL')
GO


--SCOTIA CAPITAL 
PRINT 'INSERTAR DatosCarta SCOTIA CAPITAL 1'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='46213')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','096','46213','NSOL')
GO

PRINT 'INSERTAR DatosCarta SCOTIA CAPITAL 2'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 4 and CodigoTercero ='46213')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('4','Crest-5iKAV','46213',NULL)
GO


--XP INVESTMENTS
PRINT 'INSERTAR DatosCarta XP INVESTMENTS'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='XPIN')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0443','XPIN','NSOL')
GO


PRINT 'INSERTAR DatosCarta INTL FC Stone 1'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='11111115')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0443','11111115','NSOL')
GO

PRINT 'INSERTAR DatosCarta INTL FC Stone 2'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 2 and CodigoTercero ='11111115')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('2','98730','11111115','NSOL')
GO


PRINT 'INSERTAR DatosCarta BULLTICK'
IF NOT EXISTS (SELECT * FROM [DatosCarta] WHERE CodigoTipoDato = 1 and CodigoTercero ='11111113')
INSERT DatosCarta (CodigoTipoDato,Valor,CodigoTercero,CodigoMoneda)
VALUES('1','0443','11111113','NSOL')
GO


PRINT 'INSERTAR Terceros Mandatos Chubb'
IF NOT EXISTS (SELECT * FROM [Terceros] WHERE CodigoTercero = '203906250072')
INSERT INTO Terceros (CodigoTercero,Descripcion,Situacion,Direccion,CodigoPais,CodigoTipoDocumento,CodigoDocumento,UsuarioCreacion,FechaCreacion,CodigoSectorEmpresarial,Host,TipoTercero,ClasificacionTercero,CodigoCustodio)
VALUES('203906250072','CHUBB CONSOLIDADO','A','CAL.AMADOR MERINO REYNA NRO. 267 INT. 402 LIMA - SAN ISIDRO','604','NRUC','20390625007','P500641',20190614,'OEFL','10.219.112.111','CM','T','CAVALI')
GO



PRINT 'INSERTAR ClaseActivoLimite 1'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '1')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'1', N'Efectivo y Depósitos', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoLimite 2'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '2')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'2', N'Instrumentos Representativos de Deuda', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoLimite 3'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '3')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'3', N'Instrumentos representativos de capital', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoLimite 4'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '4')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'4', N'Inversión en inmuebles y otras formas de inversión inmobiliaria', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoLimite 5'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '5')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'5', N'Primas por cobrar del seguro previsional', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoLimite 6'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '6')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'6', N'Préstamos con garantía de pólizas de seguros de vida', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoLimite 7'
IF NOT EXISTS (SELECT * FROM [ClaseActivoLimite] WHERE [CodigoClaseActivoLimite] = '7')
INSERT [dbo].[ClaseActivoLimite] ([CodigoClaseActivoLimite], [Descripcion], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'7', N'Inversiones elegibles bajo proceso de notificación o autorización', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'11:59', NULL, NULL, NULL, N'0.0.0.0')
GO



PRINT 'INSERTAR ClaseActivoInstrumentoLimite 1'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '1')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'1', N'Efectivo en ctas,ctes más ctas por cobrar menos ctas por pagar inversiones', N'1', N'0', N'1', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 2'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '2')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'2', N'Depósitos', N'1', N'0', N'1', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')

GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 3'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '3')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'3', N'Bonos', N'2', N'1', N'0', N'ClaseActivoBonos', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')

GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 4'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '4')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'4', N'Letras', N'2', N'1', N'0', N'ClaseActivoLetras', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 5'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '5')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'5', N'Papeles Comerciales', N'2', N'1', N'0', N'ClaseActivoPapelesComerciales', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 6'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '6')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'6', N'Certificados de depósitos', N'2', N'1', N'0', N'ClaseActivoCertDeposito', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 7'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '7')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'7', N'Instrumentos titulizados o instrumentos de deuda emitido por fideicomisos', N'2', N'1', N'0', N'ClaseActivoInst', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 8'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '8')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'8', N'Certificados de participación en fondos mutuos y fondos de inversión', N'2', N'1', N'0', N'ClaseActivoCertParticipacion', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 9'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '9')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'9', N'Acciones', N'3', N'1', N'0', N'ClaseActivoAcciones', N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 10'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '10')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'10', N'Exchange - Traded Funds', N'3', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 11'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '11')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'11', N'Inmuebles', N'4', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 12'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '12')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'12', N'Certificado de participaciónen fondos mutuos o fondos inmobiliarios', N'4', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 13'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '13')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'13', N'Inversión directa en proyectos inmobiliarios', N'4', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 14'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '14')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'14', N'Inmuebles arrendados con opción de compra a favor del arrendatario', N'4', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 15'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '15')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'15', N'Créditos inmobiliarios', N'4', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 16'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '16')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'16', N'Fondos mutuos o de inversión', N'7', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 17'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '17')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'17', N'Otras inversiones', N'7', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 18'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '18')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'18', N'Otras inversiones sujetas a proceso de autorización', N'7', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO

PRINT 'INSERTAR ClaseActivoInstrumentoLimite 19'
IF NOT EXISTS (SELECT * FROM [ClaseActivoInstrumentoLimite] WHERE [CodigoClaseActivoLimite] = '19')
INSERT [dbo].[ClaseActivoInstrumentoLimite] ([CodigoClaseActivoInstrumentoLimite], [Descripcion], [CodigoClaseActivoLimite], [FlagInstrumento], [FlagSaldo], [CodigoTipoInstrumentoSBS], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion], [UsuarioModificacion], [FechaModificacion], [HoraModificacion], [Host]) 
VALUES (N'19', N'Fondos de inversión con subyacentes de instrumentos representativos de derechos sobre participación patrimonial no inscritas en bolsas de valores', N'7', N'0', N'0', NULL, N'A', N'Admin', CAST(20190625 AS Numeric(8, 0)), N'12:13', NULL, NULL, NULL, N'0.0.0.0')
GO


PRINT 'Actualizacion ModeloCarta'
UPDATE ModeloCarta SET ArchivoPlantilla='Modulos\ModelosCarta\rpt_PlantillaCartas16.rdlc'
where CodigoModelo in ('CVA1','CVA2','OIAC','OIAC')
GO

PRINT 'Actualizacion Portafolio'
UPDATE Portafolio SET Consolidado = '1',
  CodigoTerceroCliente ='203906250072',
  FondoCliente = 'S'
WHERE CodigoPortafolioSBS IN ('2666','2777')


PRINT 'Actualizacion ModelosCarta Acciones'
UPDATE ModeloCarta set CodigoModelo = 'CVA1'
where CodigoModelo = 'CVA2' and CodigoOperacion ='2'


PRINT 'Actualizacion Terceros 7853653'
UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '26008714', BancoDestino = 'FEDERAL RESERVE BANK NEW YORK', NumeroCuentaDestino = '026008714', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'FRNYUS33'
WHERE CodigoTercero = '7853653'
GO

PRINT 'Actualizacion Terceros 55'
UPDATE Terceros
SET SWIFT = 'CFSUPRSJ', ForFurtherCredit = '', ABA = '026009593', BancoDestino = 'BANK OF AMERICA, N.A.', NumeroCuentaDestino = '1901332103', CiudadDestino = 'MIAMI, FLORIDA', PaisDestino = 'USA', SwiftIntermediario = 'BOFAUS3M'
WHERE CodigoTercero = '55'
GO

PRINT 'Actualizacion Terceros 54'
UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '', BancoDestino = 'JPMORGAN CHASE', NumeroCuentaDestino = '464647085', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CHASUS33'
WHERE CodigoTercero = '54'
GO

PRINT 'Actualizacion Terceros 56'
UPDATE Terceros
SET SWIFT = 'MIDLPAPA', ForFurtherCredit = '', ABA = '021000089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '36322415', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CITIUS33', Direccion = '111 Wall St. New York City, NY 10043, USA'
WHERE CodigoTercero = '56'
GO

PRINT 'Actualizacion Terceros 89'
UPDATE Terceros
SET SWIFT = 'BPABKYKG', ForFurtherCredit = '', ABA = '021000089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '36317288', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CITIUS33'
WHERE CodigoTercero = '89'
GO

PRINT 'Actualizacion Terceros 2773311'
UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '26014627', BancoDestino = 'FEDERAL RESERVE BANK NEW YORK', NumeroCuentaDestino = '220004000800', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'FRNYUS33'
WHERE CodigoTercero = '2773311'
GO

PRINT 'Actualizacion Terceros 20651654516'
UPDATE Terceros
SET SWIFT = 'MCTBPAPA', ForFurtherCredit = '', ABA = '021000018', BancoDestino = 'The Bank Of New York Mellon', NumeroCuentaDestino = '', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'IRVTUS3N', Direccion = 'One Wall Street, New York, NY 10286, USA'
WHERE CodigoTercero = '20651654516'
GO

PRINT 'Actualizacion Terceros 1345123'
UPDATE Terceros
SET SWIFT = 'BBVAUS33', ForFurtherCredit = '', ABA = '021000089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '36005487', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CITIUS33'
WHERE CodigoTercero = '1345123'
GO

PRINT 'Actualizacion Terceros 5100'
UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '21-000-089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '09250276', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = ''
WHERE CodigoTercero = '5100'
GO


--Creacion de Limite
BEGIN
	DECLARE @IDcaracteristica1 as varchar(10)
	DECLARE @IDcaracteristica2 as varchar(10)
	Declare @IDLimite as varchar(10)

	select @IDcaracteristica1 = (max(cast(CodigoCaracteristica as int)) + 1) from CaracteristicaGrupo
	select @IDcaracteristica2 = (max(cast(CodigoCaracteristica as int)) + 2) from CaracteristicaGrupo



	---------------------------------------------
	PRINT 'Nuevo Registro CaracteristicaGrupo 65'
	IF NOT EXISTS (SELECT * FROM [CaracteristicaGrupo] WHERE CodigoCaracteristica = @IDcaracteristica1)
	INSERT INTO CaracteristicaGrupo
	(
		[CodigoCaracteristica],[Descripcion],[NombreTabla],[NombreColumna],[NombreVista]
		,[Situacion],[UsuarioCreacion],[FechaCreacion],[HoraCreacion],[Host],[NombreColumnaVista]
	)
	VALUES
	(
		@IDcaracteristica1,'Clase de Activo','','CodigoClaseActivoLimite','VT_ClaseActivoLimite','A',
		'SYSTEM','20190722','10:00:00','','Codigo'
	)

	PRINT 'Nuevo Registro CaracteristicaGrupo 66'
	IF NOT EXISTS (SELECT * FROM [CaracteristicaGrupo] WHERE CodigoCaracteristica = @IDcaracteristica2)
	INSERT INTO CaracteristicaGrupo
	(
		[CodigoCaracteristica],[Descripcion],[NombreTabla],[NombreColumna],[NombreVista],[Situacion]
		,[UsuarioCreacion],[FechaCreacion],[HoraCreacion],[Host],[NombreColumnaVista],[SQL_WHERE]
	)
	VALUES
	(
		@IDcaracteristica2,'Clase de Activo Instrumento','','','VT_ClaseActivoLimiteInstrumento','A',
		'SYSTEM','20190722','10:00:00','','Codigo'
		,'AND TI.CodigoTipoInstrumentoSBS in (select Valor From ParametrosGenerales where Clasificacion IN (select CodigoTipoInstrumentoSBS  from ClaseActivoInstrumentoLimite where CodigoClaseActivoInstrumentoLimite = ''{VALOR_CARACTERISTICA}'' ))'
	)

	--------------------------------------------------------
	PRINT 'Elimintar Limite'
	IF EXISTS (SELECT * FROM Limite WHERE NombreLimite = 'Límite de Clase de Activos')
	BEGIN
		Declare @codLimite as varchar(5)

		select @codLimite = CodigoLimite FROM Limite WHERE NombreLimite = 'Límite de Clase de Activos'

		DELETE DetalleNivelLimite2 
		WHERE CodigoNivelLimite in (SELECT CodigoNivelLimite FROM NivelLimite WHERE CodigoLimiteCaracteristica in 
										(SELECT CodigoLimiteCaracteristica FROM LimiteCaracteristica WHERE CodigoLimite = @codLimite))
									
		DELETE NivelLimite WHERE CodigoLimiteCaracteristica in (SELECT CodigoLimiteCaracteristica FROM LimiteCaracteristica	WHERE CodigoLimite = @codLimite)

		DELETE LimiteCaracteristica WHERE CodigoLimite = @codLimite

		DELETE ReporteLimites where CodigoLimite = @codLimite

		DELETE Limite WHERE CodigoLimite = @codLimite
	END

	select @IDLimite = (max(cast(CodigoLimite as int)) + 1) from Limite

	PRINT 'CREAR Limite'
	INSERT INTO Limite 
	(
		CodigoLimite,NombreLimite,Tipo,UnidadPosicion,ValorBase,ClaseLimite,AplicarCastigo,AplicaDPZ,TipoCalculo,Tope,CodigoPortafolio,Replicar
		,TipoFactor,SaldoBanco,Posicion,MarketShare,IsAgrupadoPorcentaje,ValorAgrupadoPorcentaje,Cuadrar,Situacion,UsuarioCreacion,FechaCreacion	
	) 
	SELECT
		CodigoLimite = @IDLimite,NombreLimite = 'Límite de Clase de Activos',Tipo = 'L',UnidadPosicion = 'VM',ValorBase = 'CARTERA',ClaseLimite = 'F'
		,AplicarCastigo = 'N',AplicaDPZ = 'S',TipoCalculo = 1,Tope = 'MAX',CodigoPortafolio = '',Replicar = '0',TipoFactor = '',SaldoBanco = 'N'
		,Posicion = 'GI000',MarketShare = 0,IsAgrupadoPorcentaje = 0,ValorAgrupadoPorcentaje = 0,Cuadrar = 0,Situacion = 'A',UsuarioCreacion = 'ADMIN'
		,FechaCreacion	= '20190722'


	----------------------------------------------------------------------------------------
	DECLARE @CaracteristicaLimite1 as varchar(5)
	DECLARE @CaracteristicaLimite2 as varchar(5)	

	select @CaracteristicaLimite1 = (max(cast(CodigoLimiteCaracteristica as int)) + 1) from LimiteCaracteristica
	select @CaracteristicaLimite2 = (max(cast(CodigoLimiteCaracteristica as int)) + 2) from LimiteCaracteristica


	PRINT 'Asignar Portafolio 2666 al limite'
	INSERT into LimiteCaracteristica 
	(
			CodigoLimiteCaracteristica,CodigoPortafolioSBS,CodigoLimite,Tipo,Situacion ,UsuarioCreacion ,FechaCreacion,PorcentajeCercaLimite
		)
	SELECT 
		CodigoLimiteCaracteristica = @CaracteristicaLimite1,CodigoPortafolioSBS = '2666',CodigoLimite = @IDLimite,Tipo = 'L',Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722'
		,PorcentajeCercaLimite = 0


	PRINT 'Asignar Portafolio 2777 al limite'
	INSERT into LimiteCaracteristica 
	(
			CodigoLimiteCaracteristica,CodigoPortafolioSBS,CodigoLimite,Tipo,Situacion ,UsuarioCreacion ,FechaCreacion,PorcentajeCercaLimite
		)
	SELECT 
		CodigoLimiteCaracteristica = @CaracteristicaLimite2,CodigoPortafolioSBS = '2777',CodigoLimite = @IDLimite,Tipo = 'L',Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722'
		,PorcentajeCercaLimite = 0

	--------------------------------------------------------

	DECLARE @codigoNivel1 as varchar(5)
	SELECT @codigoNivel1 =  (max(cast(CodigoNivelLimite as int)) + 1)  FROM NivelLimite

	PRINT 'CREAR Nivel 1 de limite Portafolio 2666'
	insert into NivelLimite 
	(
		Secuencial,CodigoNivelLimite,CodigoCaracteristica,CodigoLimiteCaracteristica,FlagTipoPorcentaje,ValorPorcentaje,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1',CodigoNivelLimite = @codigoNivel1,CodigoCaracteristica = '00',CodigoLimiteCaracteristica = @CaracteristicaLimite1,FlagTipoPorcentaje = 'G',ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722',ValorPorcentajeM = 0, BandaLimites = ''

	----------------------
	set @codigoNivel1 = @codigoNivel1+ 1


	PRINT 'CREAR Nivel 2 de limite Portafolio 2666'
	insert into NivelLimite 
	(
		Secuencial,CodigoNivelLimite,CodigoCaracteristica,CodigoLimiteCaracteristica,FlagTipoPorcentaje,ValorPorcentaje,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2',CodigoNivelLimite = @codigoNivel1,CodigoCaracteristica = @IDcaracteristica1 ,CodigoLimiteCaracteristica = @CaracteristicaLimite1,FlagTipoPorcentaje = 'G' ,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722',ValorPorcentajeM = 0, BandaLimites = ''

	-------------------------
	set @codigoNivel1 = @codigoNivel1+ 1

	PRINT 'CREAR Nivel 3 de limite Portafolio 2666'
	insert into NivelLimite 
	(
		Secuencial,CodigoNivelLimite,CodigoCaracteristica,CodigoLimiteCaracteristica,FlagTipoPorcentaje,ValorPorcentaje,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '3',CodigoNivelLimite = @codigoNivel1,CodigoCaracteristica = @IDcaracteristica2 ,CodigoLimiteCaracteristica = @CaracteristicaLimite1,FlagTipoPorcentaje = 'D',ValorPorcentaje = Null	
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722',ValorPorcentajeM = 0, BandaLimites = ''
	
	-------------------------------------------------------------
	--Limite Nivel
	DECLARE @codigoNivel2 as varchar(5)
	SELECT @codigoNivel2 =  (max(cast(CodigoNivelLimite as int)) + 1)  FROM NivelLimite


	PRINT 'CREAR Nivel 1 de limite Portafolio 2777'
	insert into NivelLimite 
	(
		Secuencial,CodigoNivelLimite,CodigoCaracteristica,CodigoLimiteCaracteristica,FlagTipoPorcentaje,ValorPorcentaje,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1',CodigoNivelLimite = @codigoNivel2,CodigoCaracteristica = '00',CodigoLimiteCaracteristica = @CaracteristicaLimite2 ,FlagTipoPorcentaje = 'G',ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722',ValorPorcentajeM = 0, BandaLimites = ''


	set @codigoNivel2 = @codigoNivel2+ 1

	PRINT 'CREAR Nivel 2 de limite Portafolio 2777'	
	insert into NivelLimite 
	(
		Secuencial,CodigoNivelLimite,CodigoCaracteristica,CodigoLimiteCaracteristica,FlagTipoPorcentaje,ValorPorcentaje,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2',CodigoNivelLimite = @codigoNivel2,CodigoCaracteristica = @IDcaracteristica1,CodigoLimiteCaracteristica = @CaracteristicaLimite2,FlagTipoPorcentaje = 'G',ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722',ValorPorcentajeM = 0, BandaLimites = ''



	set @codigoNivel2 = @codigoNivel2+ 1

	PRINT 'CREAR Nivel 3 de limite Portafolio 2777'
	insert into NivelLimite 
	(
		Secuencial,CodigoNivelLimite,CodigoCaracteristica,CodigoLimiteCaracteristica,FlagTipoPorcentaje,ValorPorcentaje,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '3',CodigoNivelLimite = @codigoNivel2,CodigoCaracteristica = @IDcaracteristica2,CodigoLimiteCaracteristica = @CaracteristicaLimite2,FlagTipoPorcentaje = 'D' ,ValorPorcentaje = Null	
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20190722',ValorPorcentajeM = 0, BandaLimites = ''
	

--PRINT 'Asignar detalle al nivel 3, Portafolio 2666'
--BEGIN
	DECLARE @ValorPorc01 int,  @CodigoEntidad VARCHAR(40), @CodigoRelacion varchar(100)	
	DECLARE db_cursorDetalle CURSOR FOR 
	SELECT CodigoClaseActivoInstrumentoLimite, CodigoClaseActivoLimite FROM ClaseActivoInstrumentoLimite
		 
	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad , @CodigoRelacion 	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 100 /* Por Default debe ser 10% */	
	
		insert into DetalleNivelLimite2 
		(
			CodigoNivelLimite,CodigoCaracteristica,ValorCaracteristica,ValorPorcentaje,Situacion
			,UsuarioCreacion ,FechaCreacion,ClaseNormativa ,CodigoCaracteristicaRelacion,CodigoRelacion
		)
		select
			CodigoNivelLimite = @codigoNivel1,CodigoCaracteristica = @IDcaracteristica2,ValorCaracteristica = @CodigoEntidad		
			,ValorPorcentaje = @ValorPorc01	,Situacion = 'A',UsuarioCreacion = 'ADMIN',FechaCreacion	= '20190722'
			,ClaseNormativa = '',@IDcaracteristica1,@CodigoRelacion

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad, @CodigoRelacion 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle
--END
--GO

--PRINT 'Asignar detalle al nivel 3, Portafolio 2777'
--BEGIN
	DECLARE @ValorPorc02 int,  @CodigoEntidad2 VARCHAR(40), @CodigoRelacion2 varchar(100)	
	DECLARE db_cursorDetalle2 CURSOR FOR 
	SELECT CodigoClaseActivoInstrumentoLimite, CodigoClaseActivoLimite FROM ClaseActivoInstrumentoLimite
		 
	OPEN db_cursorDetalle2
	FETCH NEXT FROM db_cursorDetalle2 INTO @CodigoEntidad2 , @CodigoRelacion2 	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc02 = 100 /* Por Default debe ser 10% */	
	
		insert into DetalleNivelLimite2 
		(
			CodigoNivelLimite,CodigoCaracteristica,ValorCaracteristica,ValorPorcentaje,Situacion
			,UsuarioCreacion ,FechaCreacion	,ClaseNormativa ,CodigoCaracteristicaRelacion,CodigoRelacion
		)
		select
			CodigoNivelLimite = @codigoNivel2,CodigoCaracteristica = @IDcaracteristica2,ValorCaracteristica = @CodigoEntidad2
			,ValorPorcentaje = @ValorPorc02,Situacion = 'A',UsuarioCreacion = 'ADMIN',FechaCreacion	= '20190722'
			,ClaseNormativa = '',@IDcaracteristica1,@CodigoRelacion2

		FETCH NEXT FROM db_cursorDetalle2 INTO @CodigoEntidad2, @CodigoRelacion2 
	END 

	CLOSE db_cursorDetalle2  
	DEALLOCATE db_cursorDetalle2 
END
GO




--Creacion de Procedimientos Almacenados

PRINT '[ObtenerCodigoMoneda]'
USE [SIT-FONDOS]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='ObtenerCodigoMoneda') BEGIN 
	DROP PROCEDURE [dbo].[ObtenerCodigoMoneda]
END 
GO

-----------------------------------------------------------------------------------------------------------
--Objetivo: Obtener codigo de moneda
--	Fecha Creación: 12/06/2019
--	Creado por: Diego Tueros
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se obtiene el codigo de la moneda mediante el sinonimo.
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ObtenerCodigoMoneda]
(
	@SinonimoMoneda AS VARCHAR(10)
)
AS
BEGIN

	SELECT 
		CodigoMoneda
	FROM Moneda 
	WHERE SinonimoISO = @SinonimoMoneda

END
GO

GRANT EXECUTE ON [ObtenerCodigoMoneda] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SaldoNoAdministrado_Eliminar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SaldoNoAdministrado_Eliminar') BEGIN 
	DROP PROCEDURE [dbo].[sp_SaldoNoAdministrado_Eliminar]
END 
GO
-- ================================================
-- Autor:		Diego tueros
-- Fecha Creación:	13/06/2019
-- Descripción:		Eliminar de manera logica un regitro de la tabla 'Saldos no administrados'
-- el atributo 'Situacion' con el valor 'I' (Inactivo)
-- Nro. Orden de Trabajo: 12163
-- ================================================

CREATE PROCEDURE [dbo].[sp_SaldoNoAdministrado_Eliminar]
(
	@p_CodigoSaldoNoAdministrado varchar(12),
	@p_UsuarioModificacion varchar(15),	-- Usuario que elimin¾ el registro
	@p_FechaModificacion numeric(8, 0),	-- Fecha de eliminaci¾n del registro
	@p_HoraModificacion varchar(10)	,	-- Hora de eliminaci¾n del registro
	@p_Host varchar(20)
)
AS
BEGIN
	
	SET NOCOUNT ON
	UPDATE
		[SaldosNoAdministrados]
	SET
		[Situacion] = 'I',
		[Host] = @p_Host,
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion

WHERE
	[CodigoSaldoNoAdmnistrado] = @p_CodigoSaldoNoAdministrado
END
GO

GRANT EXECUTE ON [sp_SaldoNoAdministrado_Eliminar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SaldoNoAdministrado_Insertar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SaldoNoAdministrado_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[sp_SaldoNoAdministrado_Insertar]  

END 
GO

------------------------------------------------------------------------
-- Fecha Creacion: 17-06-2019
-- Creado por: Diego Tueros
-- Descripcion del cambio: Grabar Saldos no administrados
-- Nro. Orden de Trabajo: 12163
-------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[sp_SaldoNoAdministrado_Insertar]
(
	@p_CodigoTercero	varchar(12),
	@p_Fecha			numeric(8),
	@p_CodigoTerceroFinanciero varchar(12),
	@p_TipoCuenta		varchar(2),
	@p_Saldo			numeric(22,7),
	@p_CodigoMoneda		varchar(10),
	@p_Situacion		varchar(1),
	@p_UsuarioCreacion	varchar(50),
	@p_FechaCreacion	numeric(8,0),
	@p_HoraCreacion		varchar(10),
	@p_Host				varchar(20)
) 
AS 
BEGIN
	declare @p_CodigoSaldoNoAdministrado as varchar(12)
	
	if ((select count(*) from SaldosNoAdministrados) = 0)
		set @p_CodigoSaldoNoAdministrado = '1'
	else
		set @p_CodigoSaldoNoAdministrado =  (select max(convert(int,CodigoSaldoNoAdmnistrado)) from SaldosNoAdministrados) + 1
	--SET NOCOUNT ON 

	INSERT INTO SaldosNoAdministrados 
	(
		CodigoSaldoNoAdmnistrado,
		CodigoTercero,
		Fecha,
		CodigoTercerFinanciero,
		TipoCuenta,
		Saldo,
		CodigoMoneda,
		Situacion,
		UsuarioCreacion,
		FechaCreacion,
		HoraCreacion,
		Host
	)
	VALUES
	(
		@p_CodigoSaldoNoAdministrado,
		@p_CodigoTercero,
		@p_Fecha,
		@p_CodigoTerceroFinanciero,
		@p_TipoCuenta,
		@p_Saldo,
		@p_CodigoMoneda,
		@p_Situacion,
		@p_UsuarioCreacion,
		@p_FechaCreacion,
		@p_HoraCreacion,
		@p_Host
	)		

END
GO
GRANT EXECUTE ON [sp_SaldoNoAdministrado_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SaldoNoAdministrado_InsertarExcel]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SaldoNoAdministrado_InsertarExcel') BEGIN 
	DROP PROCEDURE [dbo].[sp_SaldoNoAdministrado_InsertarExcel]
END 
GO
------------------------------------------------------------------------
-- Fecha Creacion: 17-06-2019
-- Creado por: Diego Tueros
-- Descripcion del cambio: Grabar Saldos no administrados importado de excel
-- Nro. Orden de Trabajo: 12163
-------------------------------------------------------------------------  

CREATE PROCEDURE sp_SaldoNoAdministrado_InsertarExcel
(
	@p_CodigoTercero			varchar(12),
	@p_Fecha					numeric(8),
	@p_CodigoTerceroFinanciero	varchar(12),
	@p_TipoCuenta				varchar(2),
	@p_Saldo					Numeric(22,7),
	@p_CodigoMoneda				varchar(10),
	@p_UsuarioCreacion			varchar(50),
	@p_FechaCreacion			numeric(8),
	@p_HoraCreacion				varchar(10),
	@p_Host						varchar(20)
)
AS
BEGIN
	
	declare @p_CodigoSaldoNoAdministrado as varchar(12)
	
	if ((select count(*) from SaldosNoAdministrados) = 0)
		set @p_CodigoSaldoNoAdministrado = '1'
	else
		set @p_CodigoSaldoNoAdministrado =  (select max(convert(int,CodigoSaldoNoAdmnistrado)) from SaldosNoAdministrados) + 1
	
		INSERT INTO SaldosNoAdministrados (CodigoSaldoNoAdmnistrado,CodigoTercero,Fecha,CodigoTercerFinanciero,TipoCuenta,Saldo,CodigoMoneda,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,Host)
		VALUES(@p_CodigoSaldoNoAdministrado,@p_CodigoTercero,@p_Fecha,@p_CodigoTerceroFinanciero,@p_TipoCuenta,@p_Saldo,@p_CodigoMoneda,'A',@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host)

END
GO
GRANT EXECUTE ON [sp_SaldoNoAdministrado_InsertarExcel] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SaldoNoAdministrado_Modificar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SaldoNoAdministrado_Modificar') BEGIN 
	DROP PROCEDURE [dbo].[sp_SaldoNoAdministrado_Modificar]
END 
GO
-------------------------------------------------------------------------
-- Fecha Creacion: 17-06-2019
-- Creado por: Diego Tueros
-- Descripcion del cambio: Actualizar saldos no administrados
-- Nro. Orden de Trabajo: 12163
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_SaldoNoAdministrado_Modificar]
(
	@p_CodigoSaldoNoAdministrado varchar(12),
	@p_CodigoTercero	varchar(12),
	@p_Fecha	numeric(8,0),
	@p_CodigoTerceroFinanciero varchar(12),
	@p_TipoCuenta varchar(2),
	@p_Saldo	numeric(22,7),
	@p_CodigoMoneda	varchar(10),
	@p_Situacion varchar(1),
	@p_UsuarioModificacion varchar(50),
	@p_FechaModificacion numeric(8,0),
	@p_HoraModificacion varchar(10),
	@p_Host varchar(20)
)
AS
BEGIN
	
	UPDATE SaldosNoAdministrados
	SET
		CodigoTercero = @p_CodigoTercero,
		Fecha = @p_Fecha,
		CodigoTercerFinanciero = @p_CodigoTerceroFinanciero,
		TipoCuenta = @p_TipoCuenta,
		Saldo = @p_Saldo,
		CodigoMoneda = @p_CodigoMoneda,
		Situacion = @p_Situacion,
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion,
		Host = @p_Host
	WHERE CodigoSaldoNoAdmnistrado = @p_CodigoSaldoNoAdministrado

END
GO

GRANT EXECUTE ON [sp_SaldoNoAdministrado_Modificar] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[SP_SaldoNoAdministrado_SeleccionarPorFiltro]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='SP_SaldoNoAdministrado_SeleccionarPorFiltro') BEGIN 
	DROP PROCEDURE [dbo].[SP_SaldoNoAdministrado_SeleccionarPorFiltro]
END 
GO
-------------------------------------------------------------------------
-- Fecha Creacion: 13-06-2019
-- Creado por: Diego Tueros
-- Descripcion del cambio: Listar Saldo no Administrativo
-- Nro. Orden de Trabajo: 12163
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[SP_SaldoNoAdministrado_SeleccionarPorFiltro] 
(
	@p_CodigoSaldoNoAdministrado varchar(12),
	@P_CodigoTercero varchar(12),
	@p_Fecha numeric(8),
	@P_CodigoTerceroFinanciero varchar(12),
	@p_TipoCuenta varchar(2),
	@p_Moneda varchar(10),
	@p_TipoConsulta varchar(1)
)
AS
BEGIN
	SET NOCOUNT ON

	IF len(@p_TipoConsulta) = 0
	BEGIN
		SELECT
			CodigoSaldoNoAdmnistrado = S.CodigoSaldoNoAdmnistrado,
			CodigoMandato = S.CodigoTercero,
			DesMandato = T.Descripcion,
			Fecha = S.Fecha,
			CodigoBanco = S.CodigoTercerFinanciero,
			DesBanco = TB.Descripcion,
			CodigoTipoCuenta = S.TipoCuenta,
			DesTipoCuenta = PG.Comentario,
			Saldo = S.Saldo,
			CodigoMoneda = S.CodigoMoneda,
			DesMoneda = M.Descripcion,
			Situacion = S.Situacion,
			DesSituacion = PGS.Nombre,
			FechaFormat = dbo.FN_SIT_OBT_FechaFormateada(s.Fecha)
		FROM
		SaldosNoAdministrados S
		INNER JOIN Terceros T ON S.CodigoTercero = T.CodigoTercero
		INNER JOIN Terceros TB ON S.CodigoTercerFinanciero = TB.CodigoTercero
		INNER JOIN ParametrosGenerales PG ON S.TipoCuenta = PG.Valor AND PG.Clasificacion = 'TipoCuenta'
		INNER JOIN ParametrosGenerales PGS ON S.Situacion = PGS.Valor AND PGS.Clasificacion = 'Situación'
		INNER JOIN Moneda M ON S.CodigoMoneda = M.CodigoMoneda
		WHERE s.CodigoSaldoNoAdmnistrado = (case when len(@p_CodigoSaldoNoAdministrado) = 0 then s.CodigoSaldoNoAdmnistrado else @p_CodigoSaldoNoAdministrado end )  
		and	s.Fecha = (case when @p_Fecha= 0 then s.Fecha else  @p_Fecha end ) 
		and s.CodigoTercero = (case when len(@P_CodigoTercero) = 0 then s.CodigoTercero else @P_CodigoTercero end ) 
		and s.CodigoTercerFinanciero = (case when len(@P_CodigoTerceroFinanciero) = 0 then s.CodigoTercerFinanciero else @P_CodigoTerceroFinanciero end ) 
		and s.TipoCuenta = (case when len(@p_TipoCuenta) = 0 then s.TipoCuenta else @p_TipoCuenta end ) 
		and s.CodigoMoneda = (case when len(@p_Moneda) = 0 then s.CodigoMoneda else @p_Moneda end ) 
		ORDER BY s.Situacion asc    
	END
	ELSE
	BEGIN
	SELECT
			CodigoSaldoNoAdmnistrado = S.CodigoSaldoNoAdmnistrado,
			CodigoMandato = S.CodigoTercero,
			DesMandato = T.Descripcion,
			Fecha = S.Fecha,
			CodigoBanco = S.CodigoTercerFinanciero,
			DesBanco = TB.Descripcion,
			CodigoTipoCuenta = S.TipoCuenta,
			DesTipoCuenta = PG.Comentario,
			Saldo = S.Saldo,
			CodigoMoneda = S.CodigoMoneda,
			DesMoneda = M.Descripcion,
			Situacion = S.Situacion,
			DesSituacion = PGS.Nombre,
			FechaFormat = dbo.FN_SIT_OBT_FechaFormateada(s.Fecha)
		FROM
		SaldosNoAdministrados S
		INNER JOIN Terceros T ON S.CodigoTercero = T.CodigoTercero
		INNER JOIN Terceros TB ON S.CodigoTercerFinanciero = TB.CodigoTercero
		INNER JOIN ParametrosGenerales PG ON S.TipoCuenta = PG.Valor AND PG.Clasificacion = 'TipoCuenta'
		INNER JOIN ParametrosGenerales PGS ON S.Situacion = PGS.Valor AND PGS.Clasificacion = 'Situación'
		INNER JOIN Moneda M ON S.CodigoMoneda = M.CodigoMoneda
		WHERE 
		s.CodigoSaldoNoAdmnistrado <> @p_CodigoSaldoNoAdministrado
		and	s.Fecha = (case when @p_Fecha= 0 then s.Fecha else  @p_Fecha end ) 
		and s.CodigoTercero = (case when len(@P_CodigoTercero) = 0 then s.CodigoTercero else @P_CodigoTercero end ) 
		and s.CodigoTercerFinanciero = (case when len(@P_CodigoTerceroFinanciero) = 0 then s.CodigoTercerFinanciero else @P_CodigoTerceroFinanciero end ) 
		and s.TipoCuenta = (case when len(@p_TipoCuenta) = 0 then s.TipoCuenta else @p_TipoCuenta end ) 
		and s.CodigoMoneda = (case when len(@p_Moneda) = 0 then s.CodigoMoneda else @p_Moneda end ) 
		ORDER BY s.Situacion asc    
	END

END
GO

GRANT EXECUTE ON [SP_SaldoNoAdministrado_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'sp_SIT_CompraVentaBonos'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_CompraVentaBonos') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_CompraVentaBonos]
END 
GO
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 30/05/2019
--	Creado por: Diego Tueros
--	Nro. Orden de Trabajo: 12163
--  Descripcion: Selecciona la estructura para la creacion de cartas tipo Compra/Venta Bonos
-----------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_SIT_CompraVentaBonos] --'068074'
(
	@CodigoOperacionCaja	varchar(7)
)
AS
BEGIN
	DECLARE @BancoPublico VarChar(100), @CodigoBBH varchar(10), @EntidadBancoPublico varchar(100)

	SET @CodigoBBH = '5100'

	SELECT @BancoPublico = T.Descripcion, @EntidadBancoPublico = e.CodigoEntidad From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	JOIN Entidad E ON T.CodigoTercero = E.CodigoTercero
	WHERE Clasificacion = 'BANFONPUB'


	SELECT 
	DISTINCT
	CodigoNegocio = isnull(p.Descripcion,''),
	FechaDDMMYY = DBO.FN_SIT_OBT_FechaFormateada_DDMMYY(OI.FechaOperacion),
	TipoNegocio = isnull(p.TipoNegocio,''),
	NombrePortafolio = isnull(p.NombreCompleto,''),
	FechaCarta = dbo.RetornarFechaCompleta(OI.FechaOperacion),
	--BancoPublico = CASE WHEN P.CodigoFondosMutuos = '' THEN '' ELSE @BancoPublico END,
	BancoPublico = @BancoPublico,
	DescripcionBBH = isnull(TBBH.Descripcion,''),
	CuentaBBH = isnull(CEBBH.numerocuenta,''),
	Contraparte = isnull(T.Descripcion,''),
	CuentaFondo = isnull(CEFondo.NumeroCuenta,''),
	CuentaCCIFondo = isnull(CEFondo.NumeroCuentaInterBancario,''),
	CuentaContraparte = isnull(OI.NumeroCuenta,''),
	CuentaCCIContraparte = isnull(CECON.CuentaInterBancario,''),
	BancoContraparte = isnull(TECON.Descripcion,''),
	FechaPacto = DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion),
	FechaLiquidacion = DBO.FN_SIT_OBT_FechaFormateada(OI.FechaLiquidacion),
	CodigoISIN = isnull(OI.CodigoISIN,''),
	CodigoNemonico = isnull(OI.CodigoMnemonico,''),
	ValorNominal = isnull(v.ValorUnitario,0),
	Cantidad = isnull(OI.CantidadOperacion,0),
	Tasa = isnull(OI.YTM,0),
	Precio = isnull(OI.PrecioNegociacionSucio,0),
	Interes = isnull(OI.InteresCorrido,0),
	Monto = isnull(OI.MontoOperacion,0),
	Simbolo = isnull(M.Simbolo,''),
	Firma1 = ISNULL(AC1.Firma,'') , 
	Firma2 = ISNULL(AC2.Firma,''),
	CodigoUsuarioF1 = ISNULL(CF.VBGERF1,''),
	CodigoUsuarioF2 = ISNULL(CF.VBGERF2,''),
	NombreUsuarioF1 = ISNULL(DBO.RetornarNombrePersonal(CF.VBGERF1),''),
	NombreUsuarioF2 = ISNULL(DBO.RetornarNombrePersonal(CF.VBGERF2),''),
	CargoUsuarioF1 = ISNULL(dbo.RetornarCargoPersonal(CF.VBGERF1),''),
	CargoUsuarioF2 = ISNULL(dbo.RetornarCargoPersonal(CF.VBGERF2),''),
	Glosa = isnull(OI.ObservacionCarta,''),
	TipoCuenta = 
			CASE OI.CodigoMoneda 
				WHEN 'NSOL' THEN 'CCMN'
				ELSE 'CCME'
			END,
	Operacion = OI.CodigoOperacion,
	CategoriaInstrumento = OI.CategoriaInstrumento,
	DatosCartaDes = isNull(PG.Nombre,''),
	DatosCartaValor = isnull(DC.valor,''),
	FlagExterior = (CASE WHEN T.CodigoPais = '604' THEN '0' ELSE '1' END)
	FROM OrdenInversion OI 
	INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
	INNER JOIN Terceros T ON OI.CodigoTercero = T.CodigoTercero
	--INNER JOIN Entidad E ON T.CodigoTercero = E.CodigoTercero
	INNER JOIN Valores V ON OI.CodigoMnemonico = V.CodigoNemonico
	INNER JOIN Moneda M ON OI.CodigoMoneda = M.CodigoMoneda
	LEFT JOIN CuentaEconomica CEFondo ON OI.CodigoPortafolioSBS = CEFondo.CodigoPortafolioSBS and CEFondo.EntidadFinanciera = @EntidadBancoPublico 
		and OI.CodigoMoneda = CEFondo.CodigoMoneda and CEFondo.CodigoClaseCuenta = '20'
	LEFT JOIN Terceros TBBH ON TBBH.CodigoTercero = @CodigoBBH
	LEFT JOIN CuentaEconomica CEBBH ON OI.CodigoPortafolioSBS = CEBBH.CodigoPortafolioSBS and CEBBH.EntidadFinanciera = 'BBH' 
		and CEBBH.CodigoClaseCuenta ='20' and OI.CodigoMoneda = CEBBH.CodigoMoneda
	LEFT JOIN CuentaTerceros CECON ON OI.NumeroCuenta = CECON.NumeroCuenta
	LEFT JOIN Entidad ECON ON CECON.EntidadFinanciera = ECON.CodigoEntidad
	LEFT JOIN Terceros TECON on ECON.CodigoTercero = TECON.CodigoTercero
	LEFT JOIN DatosCarta DC ON OI.DTC = DC.CodigoDatosCarta 
	LEFT JOIN ParametrosGenerales PG ON DC.CodigoTipoDato = PG.Valor AND PG.Clasificacion = 'DATOSFONDOLIMITE'
	LEFT JOIN ClaveFirmantesCarta_OI CF ON OI.codigoOrden = CF.codigoOrden
	LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = CF.VBGERF1
	LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = CF.VBGERF2
	where OI.CodigoOrden = @CodigoOperacionCaja

END
GO

GRANT EXECUTE ON [sp_SIT_CompraVentaBonos] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SIT_TransferenciasExterior]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_TransferenciasExterior') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_TransferenciasExterior]
END 
GO
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 03/06/2019
--	Creado por: Diego Tueros
--  Descripción: Cartas de Transferencias al Exterior
--	Nro. Orden de Trabajo: 12163
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_TransferenciasExterior]
(
	@CodigoOperacionCaja	varchar(7)
)
AS
BEGIN
	DECLARE @BancoPublico VarChar(100)

	SELECT @BancoPublico = T.Descripcion From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	WHERE Clasificacion = 'BANFONPUB'

	SELECT 
		FechaDDMMYY = DBO.FN_SIT_OBT_FechaFormateada_DDMMYY(OC.FechaOperacion),
		FechaCarta = dbo.RetornarFechaCompleta(OC.FechaOperacion),
		FechaOperacion = DBO.FN_SIT_OBT_FechaFormateada(OC.FechaOperacion),
		TipoCuenta = 
			Case t.CodigoPais 
				WHEN '604' THEN
					CASE OC.CodigoMoneda 
						WHEN 'NSOL' THEN 'Cuenta Corriente M.N'
						ELSE 'Cuenta Corriente M.E'
					END
				ELSE
					CASE OC.CodigoMoneda 
					WHEN 'NSOL' THEN 'Cuenta M.N'
						ELSE 'Cuenta M.E'
					END
				END, 
		TipoCuentaDestino = 
			Case TB.CodigoPais 
				WHEN '604' THEN
					CASE OC.CodigoMoneda 
						WHEN 'NSOL' THEN 'Cuenta Corriente M.N'
						ELSE 'Cuenta Corriente M.E'
					END
				ELSE
					'Cuenta M.E'
				END, 
		Importe = OC.Importe,2,
		CodigoNegocio = P.Descripcion, 
		NombreFondo = p.NombreCompleto, 
		BancoOrigen = T.Descripcion,
		MercadoOrigen = CASE t.CodigoPais WHEN '604' THEN '1' --Nacional
							ELSE '2' --Exterior 
						END, 
		NumeroCuentaOrigen = CE.NumeroCuenta, 
		BancoDestino = TB.Descripcion,
		MercadoDestino = CASE tb.CodigoPais WHEN '604' THEN '1' --Nacional
							ELSE '2' --Exterior 
						END, 
		NumeroCuentaDestino = TCE.NumeroCuenta, 
		NumeroCuentaCCIDestino = TCE.NumeroCuentaInterBancario,
		Moneda = M.Descripcion,
		SimboloMoneda = M.Simbolo,
		CodigoMoneda = m.SinonimoISO,
		--CamposNuevos
		CuentaCustodio = isnull(TB.NumeroCuentaDestino,''),
		ForFurtherCredit = isnull(TB.ForFurtherCredit,''),
		ABA = isnull(TB.ABA,''),
		CiudadCustodio = isnull(TB.CiudadDestino,''),
		DestinoCustodio = isnull(TB.BancoDestino,''),
		CodigoOperacion = OC.codigoOperacion,
		Firma1 = ISNULL(AC1.Firma,'') , 
		Firma2 = ISNULL(AC2.Firma,''),
		CodigoUsuarioF1 = ISNULL(OC.VBGERF1,''),
		CodigoUsuarioF2 = ISNULL(OC.VBGERF2,''),
		NombreUsuarioF1 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF1),''),
		NombreUsuarioF2 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF2),''),
		CargoUsuarioF1 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF1),''),
		CargoUsuarioF2 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF2),''),
		BancoPublico = CASE WHEN P.CodigoFondosMutuos = '' THEN '' ELSE @BancoPublico END,
		Glosa = isnull(OC.ObservacionCarta,''),
		TipoCarta = CASE WHEN T.CodigoPais = '604' and TB.CodigoPais <> '604' THEN 'Transferencia al exterior'
						 WHEN T.CodigoPais <> '604' and TB.CodigoPais  = '604' THEN 'Transferencia del exterior'
						 ELSE 'Transferencia'
					END
	FROM
	OperacionesCaja OC
	INNER JOIN Portafolio P ON OC.CodigoPortafolioSBS = p.CodigoPortafolioSBS 
	INNER JOIN Terceros T ON OC.CodigoTerceroOrigen = T.CodigoTercero
	LEFT JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	LEFT JOIN CuentaEconomica CE ON CE.EntidadFinanciera = E.CodigoEntidad 
		AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
		AND CE.CodigoClaseCuenta = '20'
		AND CE.CodigoMoneda = OC.CodigoMoneda 
		AND CE.Situacion = 'A'
	INNER JOIN Terceros TB ON OC.CodigoTerceroDestino = TB.CodigoTercero
	LEFT JOIN Entidad TE ON TE.CodigoTercero = TB.CodigoTercero AND TE.Situacion = 'A'
	LEFT JOIN CuentaEconomica TCE ON TCE.EntidadFinanciera = TE.CodigoEntidad 
		AND TCE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
		AND TCE.CodigoClaseCuenta = '20'
		AND TCE.CodigoMoneda = OC.CodigoMoneda 
		AND TCE.Situacion = 'A'
	INNER JOIN Moneda M ON OC.CodigoMoneda = M.CodigoMoneda
	--LEFT JOIN Custodio C on TB.CodigoCustodio = c.CodigoCustodio
	LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = OC.VBGERF1
	LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = OC.VBGERF2
	where CodigoOperacionCaja = @CodigoOperacionCaja
END
GO

GRANT EXECUTE ON [sp_SIT_TransferenciasExterior] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[Tercero_Financiero]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Tercero_Financiero') BEGIN 
	DROP PROCEDURE [dbo].[Tercero_Financiero]
END 
GO
-----------------------------------------------------------------------------------------------------------
--	Fecha Creación: 03/06/2019
--	Creado por: Diego Tueros
--	Nro. Orden de Trabajo: 12163
--  Descripcion: Lista entidades financieras y bancos locales
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Tercero_Financiero
AS
BEGIN

	SELECT 
		CodigoTercero,
		Descripcion
	FROM  Terceros 
	WHERE TipoTercero in ('ENFI','BANC') 
	AND Situacion ='A'
	AND CodigoPais = '604' 
	AND CodigoSectorEmpresarial in ('BCOL','FINA') 
	AND CodigoTipoDocumento not in ('NRUC', 'DNI')
	order by 2 asc

END
GO

GRANT EXECUTE ON [Tercero_Financiero] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[TransferenciaInterna_Insertar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='TransferenciaInterna_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[TransferenciaInterna_Insertar]
END 
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Insertar las trasnferencias en Operaciones de caja
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 21/04/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10238
--	Descripcion del cambio: Se incluye el calculo para los saldos finales
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 17/06/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se incluye el modelo de carta para egresos al exterior
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[TransferenciaInterna_Insertar] 
	@p_CodigoMercado varchar(3),
	@p_CodigoPortafolio varchar(10),
	@p_CodigoPortafolioDestino varchar(10),
	@p_CodigoMoneda varchar(10),
	@p_NroCuenta varchar(25),
	@p_NroCuentaDestino varchar(25),
	@p_Importe numeric(12,2),
	@p_usuario varchar(15),
	@p_fecha numeric(8,0),
	@p_Hora varchar(10),
	@p_Host varchar(20),
	@p_CodigoEjecucion varchar(10),
	@nvcCodigoModelo nvarchar(4),
	@p_CodigoContacto varchar(3) = NULL,
	@p_fechaOperacion numeric(8,0) = 0, 
	@P_TipoTransferencia VarChar(3), 
	@P_TranFictizia Char(1),
	@p_CodigoTerceroDestino VarChar(20),
	@p_ObservacionCarta Varchar(1000)
AS
BEGIN
	DECLARE @v_CodigoOperacionesCajaE int, @v_CodigoOperacionesCajaI INT,@ClaseCuentaOrigen varchar(3),@ClaseCuentaDestino varchar(3),@CodigoEntidadOrigen varchar(4),
	@CodigoEntidadDestino varchar(4),@CodigoTerceroOrigen varchar(11),@CodigoTerceroDestino varchar(11),@codigoMercado varchar(3),
	@codigoMercadoDestino varchar(3),@nvcTerceroOrigen nvarchar(50),@nvcTerceroDestino nvarchar(50),@EstadoCarta varchar(1),@FechaOperacion numeric(8), 
	@CodigoOperacionIngreso VarChar(6),@CodigoOperacionEgreso VarChar(6),@CorrelativoCartas INT, @CodigoCartaEgreso varchar(4), @CodigoCartaIngreso varchar(4),@ObservacionCartaEgreso varchar(1000),
	@ObservacionCartaIngreso varchar(1000)
	--Setea el codigo de la operacion segun el tipo de traspaso

	IF @P_TipoTransferencia = '63'
		BEGIN 
			SET @CodigoCartaIngreso = 'SC01' 
			SET @CodigoCartaEgreso = @nvcCodigoModelo 
			SET @ObservacionCartaIngreso = ''
			SET @ObservacionCartaEgreso = @p_ObservacionCarta
		END
	ELSE IF @P_TipoTransferencia = '64'
		BEGIN
			SET @CodigoCartaIngreso = 'SC01' 
			SET @CodigoCartaEgreso = 'SC01' 
			SET @ObservacionCartaIngreso = ''
			SET @ObservacionCartaEgreso = ''
		END
	ELSE
		BEGIN
			SET @CodigoCartaIngreso = @nvcCodigoModelo
			SET @CodigoCartaEgreso = @nvcCodigoModelo
			SET @ObservacionCartaIngreso = @p_ObservacionCarta
			SET @ObservacionCartaEgreso = @p_ObservacionCarta
		END

	IF @P_TipoTransferencia = '63' OR @P_TipoTransferencia = '64'
		BEGIN 
			SET @CodigoOperacionIngreso = '64' 
			SET @CodigoOperacionEgreso = '63' 
		END
	ELSE
	BEGIN 
			SET @CodigoOperacionIngreso = 'BCRI' 
			SET @CodigoOperacionEgreso = 'BCRE' 
		END

	SET	@EstadoCarta = '1'
	
	SELECT 
			@v_CodigoOperacionesCajaE = ISNULL(max(CONVERT(INT,substring(CodigoOperacionCaja,2,LEN(CodigoOperacionCaja)-1))),0)+ 1
	FROM OperacionesCaja 
	WHERE SUBSTRING(CodigoOperacionCaja,1,1) = 'T'
	
	--
	SELECT 
			@ClaseCuentaOrigen=CodigoClaseCuenta,
			@codigoMercado=CodigoMercado,
			@CodigoEntidadOrigen=EntidadFinanciera
	FROM CuentaEconomica 
	WHERE NumeroCuenta = @p_NroCuenta 
	AND CodigoPortafolioSBS = @p_CodigoPortafolio
	
	--
	SELECT 
			@ClaseCuentaDestino = CodigoClaseCuenta,
			@CodigoEntidadDestino = EntidadFinanciera,
			@codigoMercadoDestino = CodigoMercado
	FROM CuentaEconomica 
	WHERE NumeroCuenta = @p_NroCuentaDestino 
	AND	CodigoPortafolioSBS = @p_CodigoPortafolioDestino

	--
	SELECT 
			@CodigoTerceroOrigen = e.CodigoTercero,
			@nvcTerceroOrigen = t.Descripcion
	FROM Entidad e
	INNER JOIN Terceros t ON e.CodigoTercero = t.CodigoTercero
	WHERE e.CodigoEntidad = @CodigoEntidadOrigen
	--
	IF @p_NroCuentaDestino <> 'Matriz'
		SELECT 
				@CodigoTerceroDestino = e.CodigoTercero,
				@nvcTerceroDestino = t.Descripcion
		FROM Entidad e
		INNER JOIN Terceros t ON e.CodigoTercero = t.CodigoTercero 
		WHERE e.CodigoEntidad = @CodigoEntidadDestino
	ELSE
		SELECT 
				@CodigoTerceroDestino = @p_CodigoTerceroDestino ,
				@nvcTerceroDestino = t.Descripcion
		FROM Terceros T  
		WHERE T.CodigoTercero = @p_CodigoTerceroDestino

	--
	SELECT 
			@FechaOperacion = CASE 
								WHEN @p_fechaOperacion = 0 THEN FechaNegocio 
								ELSE @p_fechaOperacion 
							  END
	FROM Portafolio 
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio

	--EGRESO
	--Correlativo Cartas
	IF @nvcCodigoModelo = 'SC01'
		SET @CorrelativoCartas = 0
	ELSE
	BEGIN
		SELECT 
				@CorrelativoCartas = MAX(CorrelativoCartas) 
		FROM OperacionesCaja 
		WHERE FechaOperacion = @p_fecha AND CodigoModelo <> 'SC01'
		SET @CorrelativoCartas = ISNULL(@CorrelativoCartas,0) + 1
	END
	--
	INSERT INTO OperacionesCaja
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
			CodigoPortafolioSBSDestino,
			Estado,
			FechaOperacion, 
			CodigoModelo, 
			CodigoContacto,
			EstadoCarta,
			OperacionFictizia,
			CorrelativoCartas,
			ObservacionCarta  
	)
	VALUES( 
			'T'+CONVERT(varchar(7),@v_CodigoOperacionesCajaE), 
			@CodigoMercado, 
			@ClaseCuentaOrigen, 
			@p_NroCuenta,
			@nvcTerceroDestino + '/' + @p_NroCuentaDestino,
			@p_Importe,
			@p_usuario, 
			@p_fecha,
			@p_CodigoMoneda, 
			'M', 
			'A', 
			@p_Host,
			NULL, 
			@p_Hora, 
			@p_NroCuentaDestino, 
			NULL,
			@ClaseCuentaDestino, 
			NULL, 
			@CodigoTerceroOrigen,
			@p_Hora,
			NULL,
			@CodigoOperacionEgreso,
			@CodigoTerceroDestino,
			@p_CodigoPortafolio, 
			@p_CodigoPortafolioDestino,
			'I',
			@FechaOperacion, 
			--@nvcCodigoModelo,
			@CodigoCartaEgreso,
			@p_CodigoContacto,
			@EstadoCarta,
			@P_TranFictizia,
			@CorrelativoCartas,
			@ObservacionCartaEgreso
	)

	--Actualiza Ingreso y egreso
	EXEC usp_UpdateSaldosBancariosOperaciones @p_CodigoPortafolio, @FechaOperacion, @p_NroCuenta, 'E', @p_Importe
	--Actualiza el saldo final con la logica correspondiente
	EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,	@p_NroCuenta,@FechaOperacion --10238
	
	--INGRESO
	SELECT 
			@v_CodigoOperacionesCajaI = ISNULL(MAX(CONVERT(INT,SUBSTRING(CodigoOperacionCaja,2,LEN(CodigoOperacionCaja)-1))),0)+ 1
	FROM OperacionesCaja 
	WHERE SUBSTRING(CodigoOperacionCaja,1,1) = 'T'
	--

	SELECT 
			@FechaOperacion = CASE 
								WHEN @p_fechaOperacion = 0 THEN FechaNegocio 
								ELSE @p_fechaOperacion 
							  END
	FROM Portafolio 
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolioDestino


	--Se toma la moneda de la cuenta donde se hace el deposito
	SELECT 
			@p_CodigoMoneda = CodigoMoneda 
	FROM CuentaEconomica 
	WHERE NumeroCuenta = @p_NroCuentaDestino

	INSERT INTO OperacionesCaja
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
		CodigoPortafolioSBSDestino,
		Estado,
		FechaOperacion, 
		CodigoModelo, 
		CodigoContacto,
		EstadoCarta, 
		CodigoOrden_Rel,
		OperacionFictizia,
		CorrelativoCartas,
		ObservacionCarta 
	)
	VALUES
	(
		'T'+CONVERT(varchar(7),@v_CodigoOperacionesCajaI), 
		@codigoMercadoDestino, 
		@ClaseCuentaDestino, 
		@p_NroCuentaDestino,
		@nvcTerceroOrigen + '/' + @p_NroCuenta,
		@p_Importe,
		@p_usuario, 
		@p_fecha,
		@p_CodigoMoneda, 
		'M', 
		'A', 
		@p_Host,
		NULL, 
		@p_Hora, 
		@p_NroCuenta, 
		NULL,
		@ClaseCuentaOrigen, 
		NULL, 
		@CodigoTerceroDestino, 
		@p_Hora,
		NULL,
		@CodigoOperacionIngreso,
		@CodigoTerceroOrigen, 
		@p_CodigoPortafolioDestino, 
		@p_CodigoPortafolio,
		'I',
		@FechaOperacion, 
		--@nvcCodigoModelo, 
		@CodigoCartaIngreso,
		@p_CodigoContacto,
		@EstadoCarta,
		'T'+CONVERT(varchar(7),@v_CodigoOperacionesCajaE),
		@P_TranFictizia,
		@CorrelativoCartas,
		@ObservacionCartaIngreso
	)
	
	--Atualiza codigoordenREL
	UPDATE OperacionesCaja 
		SET CodigoOrden_Rel = 'T'+CONVERT(varchar(7),@v_CodigoOperacionesCajaI)
	WHERE CodigoOperacionCaja = 'T'+CONVERT(varchar(7),@v_CodigoOperacionesCajaE)
	
	--Actualiza Ingreso y egreso
	EXEC usp_UpdateSaldosBancariosOperaciones @p_CodigoPortafolioDestino, @FechaOperacion, @p_NroCuentaDestino, 'I', @p_Importe
	
	--Actualiza el saldo final con la logica correspondiente
	EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,	@p_NroCuentaDestino,@FechaOperacion --10238
END
GO

GRANT EXECUTE ON [TransferenciaInterna_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[usp_ListaModeloCartaOperacion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='usp_ListaModeloCartaOperacion') BEGIN 
	DROP PROCEDURE [dbo].[usp_ListaModeloCartaOperacion]
END 
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Listar los modelos de carta por operacion
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion		: 07/12/2016
--	Modificado por			: Carlos Espejo
--	Nro. Orden de Trabajo	: 9678
--	Descripcion del cambio	: Solo se presenta las cartas por operacion
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion		: 10/06/2019
--	Modificado por			: Diego Tueros
--	Nro. Orden de Trabajo	: 12163
--	Descripcion del cambio	: Se presentaran las cartas por operacion y por clase de instrumento si tiene uno en relacion
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[usp_ListaModeloCartaOperacion](
	@nvcCodigoOperacion nvarchar(6),
	@p_CodigoOrden VarChar(112)
)
AS
BEGIN
	
	SELECT 
		CodigoModelo,
		Descripcion 
	FROM ModeloCarta 
	WHERE CodigoOperacion = @nvcCodigoOperacion 
		 AND Situacion = 'A'  
END
GO

GRANT EXECUTE ON [usp_ListaModeloCartaOperacion] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[CuentasPorCobrar_SeleccionarPorFiltro]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='CuentasPorCobrar_SeleccionarPorFiltro') BEGIN 
	DROP PROCEDURE [dbo].[CuentasPorCobrar_SeleccionarPorFiltro]
END 
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Selecciona las CXC y CxP
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 03/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9777
--	Descripcion del cambio: Se cambia la extension de la variable a 6
-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 31/07/2017
-- Modificado por: Carlos Espejo
-- Nro. Orden de Trabajo: 10090
-- Descripcion del cambio: Fecha de operacion por fecha vencimiento
-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 02/10/2017
-- Modificado por: Ian Pastor
-- Nro. Orden de Trabajo: 10795
-- Descripcion del cambio: Mejora y ordenamiento de store procedure
-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 18/12/2017
-- Modificado por: Hanz Cocchi
-- Nro. Orden de Trabajo: 10979
-- Descripcion del cambio: Optimización de store procedure
-- ===================================================================================================
-- Autor:			Ricardo Colonia
-- Fecha Creación:	01/08/2018
-- Proyecto:		Fondos-II-SPRINT 4
-- Nro OT:			11512
-- Descripción:		Se agrega nuevo campo CodigoTipoInstrumentoSBS de Situación solo mostrar activos
-- ===================================================================================================
-- Autor:			Diego Tueros
-- Fecha Creación:	10/06/2019
-- Nro OT:			12163
-- Descripción:		Se agrega nuevo campo CodigoClaseInstrumento para obtener el modelo carta correspondiente
-- ===================================================================================================
CREATE PROCEDURE [dbo].[CuentasPorCobrar_SeleccionarPorFiltro]
	@p_CodigoPortafolio			VARCHAR(100) = '',
	@p_CodigoMercado			VARCHAR(40) = '',
	@p_CodigoMoneda				VARCHAR(10) = '',
	@p_CodigoOperacion			VARCHAR(6) = '',
	@p_CodigoIntermediario		VARCHAR(11) = '',
	@p_CodigoClaseInstrumento	VARCHAR(10) = '',
	@p_Egreso					VARCHAR(1) = '',
	@p_FechaOperacionIni		NUMERIC(8) = 0,
	@p_FechaOperacionFin		NUMERIC(8) = 0,
	@p_LiquidaFechaAnt			VARCHAR(1) = ''
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT	NumeroOperacion NroOperacion, 
			dbo.FormatDate(CxC.FechaOperacion) FechaNegociacion, 
			dbo.FormatDate(CxC.FechaVencimiento) FechaVencimiento, 
			CASE 
				WHEN LEFT(NumeroOperacion,1) = 'K' THEN LTRIM(CxC.CodigoNemonico) + '-' + T.Descripcion + '-Cuponera' 
				WHEN CxC.CodigoNemonico = 'FORWARD' THEN LTRIM(CxC.CodigoNemonico) + '-' + (CASE 
																								WHEN OI.Delibery = 'N' THEN 'NOM-DELIVERY' 
																								ELSE 'DELIVERY' 
																							END) 
				ELSE LTRIM(ISNULL(CxC.CodigoNemonico, '')) + '-' + ISNULL(CI.Descripcion, '') 
			END AS 'Referencia', 
			CASE 
				WHEN OI.CategoriaInstrumento = 'FD' AND OI.Delibery = 'S' THEN OI.MontoCancelar 
				ELSE Importe 
			END AS Importe, 
			ISNULL(M.Descripcion, '') DescripcionMercado, 
			P.Descripcion DescripcionPortafolio, 
			Mo.CodigoMoneda DescripcionMoneda, 
			T.Descripcion DescripcionIntermediario, 
			O.Descripcion DescripcionOperacion, 
			CASE 
				WHEN TipoMovimiento = 'A' THEN 'Automático' 
				ELSE 'Manual' 
			END TipoMovimiento, 
			CxC.CodigoPortafolioSBS, CxC.CodigoMoneda, 
			ISNULL(CI.Categoria, '') Categoria, 
			ISNULL(CxC.CodigoOrden, '') CodigoOrden, 
			OI.CodigoMnemonico, 
			CxC.CodigoOperacion, 
			CASE 
				WHEN ISNULL(CxC.Estado, '') = '' THEN 'Por Liquidar' 
				ELSE 'Anulado' 
			END DescripcionEstado, 
			ISNULL(CxC.Estado, '') Estado, 
			TR.CodigoRenta, 
			CxC.FechaVencimiento AS FechaVencimientoSort, 
			CxC.CodigoMercado, 
			CxC.CodigoTercero, 
			UPPER(ISNULL(E.CodigoTipoEntidad, '')) AS CodigoTipoEntidad, 
			UPPER(ISNULL(E.CodigoEntidad, '')) AS CodigoEntidad,
			ti.CodigoTipoInstrumentoSBS, -- RCE | Se agrega nuevo campo para calcular suscripción de fondo | 01/08/2018
			CI.CodigoClaseInstrumento --OT2012
	FROM	dbo.CuentasPorCobrarPagar CxC 
			LEFT JOIN Mercado M ON M.CodigoMercado = CxC.CodigoMercado 
			INNER JOIN Portafolio P ON P.CodigoPortafolioSBS = CxC.CodigoPortafolioSBS 
			INNER JOIN Moneda Mo ON Mo.CodigoMoneda = CxC.CodigoMoneda 
			INNER JOIN Terceros T ON T.CodigoTercero = CxC.CodigoTercero 
			LEFT JOIN Entidad E ON T.CodigoTercero = E.CodigoTercero 
				  AND E.Situacion = 'A' 
			INNER JOIN Operacion O ON O.CodigoOperacion = CxC.CodigoOperacion 
			LEFT OUTER JOIN Valores V ON V.CodigoNemonico = CxC.CodigoNemonico 
			LEFT OUTER JOIN TipoInstrumento ti ON v.CodigoTipoInstrumentoSBS = ti.CodigoTipoInstrumentoSBS 
--			LEFT JOIN ClaseInstrumento ci ON ti.CodigoClaseInstrumento = ci.CodigoClaseInstrumento 
			LEFT OUTER JOIN ClaseInstrumento ci ON ti.CodigoClaseInstrumento = ci.CodigoClaseInstrumento 
			LEFT OUTER JOIN TipoRenta TR ON TR.CodigoRenta = V.TipoRenta 
--			INNER JOIN OrdenInversion OI ON OI.CodigoOrden = CxC.CodigoOrden 
			LEFT OUTER JOIN OrdenInversion OI ON OI.CodigoOrden = CxC.CodigoOrden 
				   AND OI.CodigoPortafolioSBS = CxC.CodigoPortafolioSBS 
--				   AND OI.CategoriaInstrumento NOT IN ('FI','FM') 
	WHERE	
--			(CxC.CodigoPortafolioSBS in (SELECT valor from #ValoresPortafolio) OR @p_CodigoPortafolio='') AND
			(CxC.CodigoPortafolioSBS = @p_CodigoPortafolio OR @p_CodigoPortafolio = '') AND 
--			(CxC.CodigoMercado in (SELECT valor from #ValoresMercado) OR @p_CodigoMercado='') AND 
			(CxC.CodigoMercado = @p_CodigoMercado OR @p_CodigoMercado = '') AND 
			(CxC.CodigoMoneda = CASE WHEN LEN(@p_CodigoMoneda) > 0 THEN @p_CodigoMoneda ELSE CxC.CodigoMoneda END) AND 
			(CxC.CodigoOperacion = CASE WHEN LEN(@p_CodigoOperacion) > 0 THEN @p_CodigoOperacion ELSE CxC.CodigoOperacion END) AND 
			(CxC.CodigoTercero = CASE WHEN LEN(@p_CodigoIntermediario) > 0 THEN @p_CodigoIntermediario ELSE CxC.CodigoTercero END) AND 
			(CxC.FechaVencimiento >= @p_FechaOperacionIni) AND 
			(CxC.FechaVencimiento <= @p_FechaOperacionFin) AND 
			(ISNULL(CxC.Estado, '') = '' OR CxC.Estado = 'E') AND 
			CxC.Egreso = @p_Egreso AND 
			CxC.Situacion = 'A' AND 
--			CxC.CodigoOperacion <> '3' AND 
			ISNULL(CxC.Estado, '') <> 'L' AND 
			ISNULL(CxC.LiquidaFechaAnt, '') = @p_LiquidaFechaAnt AND 
			ISNULL(OI.TipoFondo, '') NOT IN('CC_CNC', 'CC_SNC') 
		
	SET NOCOUNT OFF

END
GO

GRANT EXECUTE ON [CuentasPorCobrar_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[Mandatos_Listar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Mandatos_Listar') BEGIN 
	DROP PROCEDURE [dbo].[Mandatos_Listar]
END 
GO
-- ===================================================================================================
-- Autor:			Diego Tueros
-- Fecha Creación:	17/06/2019
-- Nro OT:			12163
-- Descripción:		Lista los terceros tipo mandato
-- ===================================================================================================

CREATE  PROCEDURE [dbo].[Mandatos_Listar]  
AS  
BEGIN
  
	SELECT   
	  TE.[CodigoTercero],   
		TE.[Situacion],   
		PG.Nombre AS 'NombreSituacion',  
		TE.[CodigoTercero],  
		TE.[Descripcion],   
		TE.[Direccion],   
		TE.EntidadFinanciera,  
		TE.Beneficiario,  
		TE.[CodigoPostal],   
		TE.[CodigoPais],  
		PA.Descripcion AS 'NombrePais',  
		TE.[CodigoTipoDocumento],   
		TD.Descripcion AS 'NombreTipoDocumento',  
		TE.[CodigoSectorEmpresarial],  
		SE.Descripcion AS 'NombreSectorEmpresarial',  
		TE.[TipoTercero],   
		TE.[ClasificacionTercero],   
		TE.[CodigoCustodio] ,  
		CU.Descripcion AS 'NombreCustodio',  
		TE.UsuarioCreacion,  
		TE.FechaCreacion,  
		TE.HoraCreacion,  
		TE.FechaModificacion,  
		TE.UsuarioModificacion,  
		TE.HoraModificacion,  
		TE.Host  
	FROM   
	Terceros AS TE  
	LEFT JOIN TipoDocumento AS TD ON TE.CodigoTipoDocumento = TD.CodigoTipoDocumento  
	LEFT JOIN SectorEmpresarial as SE ON TE.CodigoSectorEmpresarial = SE.CodigoSectorEmpresarial  
	LEFT JOIN Pais AS PA ON TE.CodigoPais = PA.CodigoPais  
	LEFT JOIN ParametrosGenerales AS PG ON TE.Situacion = PG.Valor  
	LEFT JOIN Custodio AS CU ON TE.CodigoCustodio = CU.CodigoCustodio  
	WHERE PG.Clasificacion = 'Situación'  
	AND TE.SITUACION='A'
	AND TE.TipoTercero = 'CM'
	ORDER BY  
	TE.Descripcion ASC

END
GO

GRANT EXECUTE ON [Mandatos_Listar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SIT_ListarClienteMandato]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_ListarClienteMandato') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_ListarClienteMandato]
END 
GO

-- ===================================================================================================
-- Autor:			Diego Tueros
-- Fecha Creación:	17/06/2019
-- Nro OT:			12163
-- Descripción:		Lista los terceros tipo mandato
-- ===================================================================================================

CREATE PROCEDURE [dbo].[sp_SIT_ListarClienteMandato]
@p_Descripcion varchar(64),
@p_Situacion char(1),
@p_CodigoTercero varchar(12)=NULL
as
------------------------------------------------------------------------------------------------------------------------
-- @Test: exec sp_SIT_ListarClienteMandato @p_Descripcion=N'',@p_Situacion=N'',@p_CodigoTercero=N''
--------------------------------------------------------------------------------------------------------------------------
begin

DECLARE @v_TipoTercero char(2)='CM'

select T.CodigoTercero,
	   T.Descripcion,
	   CodigoCustodio,
	   SE.Descripcion NombreSectorEmpresarial,
	   E.CodigoSBS,
	   E.sinonimo,
	   PA.Descripcion NombrePais,
	   PG.Nombre NombreSituacion,
	   T.CodigoTipoDocumento
from Terceros T
JOIN SectorEmpresarial SE ON SE.CodigoSectorEmpresarial = T.CodigoSectorEmpresarial
LEFT JOIN Pais	AS PA ON T.CodigoPais = PA.CodigoPais
LEFT JOIN Entidad E ON E.CodigoEntidad = T.EntidadFinanciera
LEFT JOIN ParametrosGenerales PG ON PG.Valor = T.SITUACION AND PG.Clasificacion = 'Situación'
where (T.TipoTercero = @v_TipoTercero) AND
	  (@p_Situacion = '' OR T.Situacion = @p_Situacion) AND
	  (@p_Descripcion = '' OR T.Descripcion LIKE '%'+ @p_Descripcion +'%') AND
	  (@p_CodigoTercero = '' OR	T.CodigoTercero = @p_CodigoTercero)

end
GO

GRANT EXECUTE ON [sp_SIT_ListarClienteMandato] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[Portafolio_Insertar]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Portafolio_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[Portafolio_Insertar]
END 
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
-- Fecha modificacion: 17/06/2019
-- Modificado por: Karina Gomez
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Se agrego el campo Consolidado en el mantenimiento Administración de Portafolio. Es un cambio solicitado
--                         para el desarrollo de Límites Fondo, dónde se selecciona un Cliente del tipo Mandato y se indica si el reporte consolidara los saldos no administrativos.
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Portafolio_Insertar](
	@p_CodigoPortafolioSBS VARCHAR(10),	-- Código Portafolio SBS
	@p_Descripcion VARCHAR(40),		-- Descripcion del registro
	@p_FechaConstitucion NUMERIC(8,0),	-- Fecha de constitucion del registro
	@p_CodigoTipoValorizacion VARCHAR(3),	-- Codigo tipo de valorizacion del registro
	@p_FechaTermino  NUMERIC(8,0),
	@p_CodigoMoneda VARCHAR(10),
	@p_FechaAperturaContable NUMERIC(8,0),
	@p_UsuarioCreacion VARCHAR(15),	-- Usuario que creó el registro
	@p_Situacion VARCHAR(1),		-- Situación del registro ('I' = Inactivo , 'A' = Activo)
	@p_FechaCreacion NUMERIC(8, 0),	-- Fecha de creación del registro
	@p_HoraCreacion VARCHAR(10),		-- Hora de creación del registro
	@p_CodigoNegocio VARCHAR(4),		-- Codigo de negocio del registro
	@p_Host VARCHAR(20),			-- IP de la PC local
	@p_EsMultiPortafolio VARCHAR(1),	-- Grupo Portafolio
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
	@p_CodigoRenta VARCHAR(3),	-- OT 9362
	@p_TipoCalculoValorCuota VARCHAR(2),
	@p_FechaCajaOperaciones NUMERIC(8),
	@p_ValoracionMensual VARCHAR(1), 
	@p_CuotasLiberadas VARCHAR(1),-- OT10916
	@p_CPPadreSisOpe VARCHAR(10),
	@p_VectorPrecioVal VARCHAR(4),
    @p_FlagAumentoCapital BIT,  							 
	@p_TipoNegocio VARCHAR(10), -- Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
	@p_FondoCliente	VARCHAR(1),
	@p_CodigoTerceroCliente VARCHAR(12),
	@p_TipoComision VARCHAR(10), -- Proyecto SIT Fondos - Comision | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
	@p_Consolidado INT  
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
	CuotasLiberadas,CPPadreSisOpe,VectorPrecioVal, TipoNegocio,FondoCliente,CodigoTerceroCliente,FlagAumentoCapital,Consolidado,TipoComision) -- OT10916  
								  
	VALUES(@p_CodigoPortafolioSBS,@p_Descripcion,@p_FechaConstitucion,@p_CodigoTipoValorizacion,@p_FechaTermino ,@p_CodigoMoneda,
	@p_FechaAperturaContable,@p_UsuarioCreacion,@p_Situacion,@p_FechaCreacion,@p_HoraCreacion,@p_CodigoNegocio,@p_Host,@p_Correlativo,@p_EsMultiPortafolio,
	@p_INTerfazContable,@p_TipoCartera,@p_CodContabilidadFondo,@p_PorcentajeComision,@p_CodigoFondosMutuos,@p_ValorInicialFondo,@p_IndicadorFondo,@p_NumeroCuotaPreCierre,
	@p_PorSerie,@p_FechaConstitucion,@p_FechaAperturaContable,@p_NombreCompleto,@FlagComisionVariable,@MontoSuscripcionInicial,
	@TopeValorCuota,@FlagComisionSuscripInicial,@p_FechaConstitucion,@BDConta,@CodigoPortafolioSisOpe,@p_RUC, @p_CodigoRenta,@p_TipoCalculoValorCuota, @p_FechaCajaOperaciones,
	@p_ValoracionMensual, -- OT 9362
	@p_CuotasLiberadas /*OT109116*/,@p_CPPadreSisOpe,@p_VectorPrecioVal, @p_TipoNegocio,@p_FondoCliente,@p_CodigoTerceroCliente,@p_FlagAumentoCapital, @p_Consolidado,@p_TipoComision) 
	
   
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

GRANT EXECUTE ON [Portafolio_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[Limite_ProcesarLimite_NEW]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Limite_ProcesarLimite_NEW') BEGIN 
	DROP PROCEDURE [dbo].[Limite_ProcesarLimite_NEW]
END 
GO
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha modificacion:02/08/2019
-- Modificado por: Diego Tueros
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Adaptacion del reporte para sumar los saldos no administrados en el consolidado, se agrego el limite clase activo con multinivel
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Limite_ProcesarLimite_NEW]
	@p_CodigoLimite VARCHAR(5)
	,@p_CodigoLimiteCaracteristica VARCHAR(5)
	,@p_Fecha NUMERIC(8,0)
	,@p_Escenario VARCHAR(20) = 'REAL'
	,@p_CodigoMnemonico VARCHAR(15) = ''
AS
BEGIN 
	DECLARE @p_procesar NUMERIC(8,0) = 1

	----INI OT 9362
	--DECLARE @v_IsAgrupadoPorcLimite BIT = dbo.fn_SIT_RetornarIsAgrupadoPorcentaje_Limite(@p_CodigoLimite);
	----FIN OT 9362
	--PRINT 'ES AGRUPADOR PORCENTAJE'
	--IF @v_IsAgrupadoPorcLimite = 0 		-- OT 9362
	--BEGIN								-- OT 9362
	
		SET NOCOUNT ON
		DECLARE @NumeroNiveles INT -- Se obtiene el maximo nivel del limite, para validar Participacion al final  
		SET @NumeroNiveles = ISNULL((SELECT MAX(Secuencial) FROM NivelLimite WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Situacion = 'A'), 0)  
		--Variables
		--OT 9577 Inicio
		DECLARE  @TipoCalculo VARCHAR(1),@Tope  VARCHAR(6),@ValorBase VARCHAR(100),@SaldoBanco VARCHAR(1),@CodGrupoInstrumento VARCHAR(5),
		--OT 9577 Fin
		@CodigoPortafolio VARCHAR(10),@CodigoPortafolioSB VARCHAR(10),@Tipo VARCHAR(5),@marketShare NUMERIC(18,7),@Valor NUMERIC(22,7),
		@SaldoBancoValor NUMERIC(22,7),@cxc NUMERIC(22,7),@cxp NUMERIC(22,7),@TotalInversiones NUMERIC(22,7),@Escenario VARCHAR(20)

		------Temporales
		----IF @p_procesar in (1,2)
		----BEGIN
		
			CREATE TABLE #Reporte(
				CodigoReporte INT IDENTITY(1,1),
				Secuencial VARCHAR(5),
				CodigoNivelLimite VARCHAR(5),
				ValorPorcentajeM NUMERIC(22,7),	/* Valor Porcntaje Minimo */			
				ValorPorcentaje NUMERIC(22,7), /* Valor Porcntaje Maximo */	
				MontoValorBase NUMERIC(22,7),
				
				Posicion NUMERIC(22,7),
				ValorNivel VARCHAR(100),
				DescripcionNivel  VARCHAR(100),
				Patrimonio NUMERIC(22,7)  DEFAULT 0,
				FechaVencimiento NUMERIC(8,0),
				CodigoMoneda VARCHAR(10),
				FechaOperacion NUMERIC(10),

				-- Extras que se llenan en este nivel y en la parte final  
				PorVal VARCHAR(5)   DEFAULT '',
				Factor NUMERIC(10,7),
				
				ValorEfectivoColocado NUMERIC(22,7),
				FloatOficioMultiple NUMERIC(22,7),
				UnidadesEmitidas NUMERIC(22,7),
				TotalActivo NUMERIC(22,7),
				TotalPasivo NUMERIC(22,7),
				NivelSaldoBanco NUMERIC(22,7),
				IndFwdSwp VARCHAR(1),

				Alerta VARCHAR(5) DEFAULT '',
				Valor1 NUMERIC(22,7)  DEFAULT 0,  
				Valor2 NUMERIC(22,7)  DEFAULT 0,
				Valor3 NUMERIC(22,7)  DEFAULT 0,
				CodigoCaracteristicaRelacion varchar(100),
				CodigoRelacion varchar(100)
			)
			CREATE TABLE #Reporte_Fondos(
				CodigoReporte NUMERIC(18, 0),
				CodigoPortafolio VARCHAR(20),
				PosicionPortafolio NUMERIC(22, 7),
				ValorNivel VARCHAR(100)
			)
			CREATE TABLE #Posicion_Fondos(
				CodigoPortafolio VARCHAR(20),
				PosicionPortafolio NUMERIC(22, 7)
			)
			CREATE TABLE #ValoresNivel  (  
				Orden   INT IDENTITY(1,1) PRIMARY KEY,
				Secuencial int,
				Valor NVARCHAR(100),
				NombreColumna VARCHAR(256),
				NombreVista VARCHAR(100),
				CodigoCaracteristica VARCHAR(5),
				CodigoNivelLimite VARCHAR(5)
			)
			
			------CREATE  NONCLUSTERED INDEX Idx1Val ON [#ValoresNivel](Secuencial)
			----IF @p_procesar <> 2
			----BEGIN
			-- Se debe borrar la data preexistente en ReporteLimites_Portafolio
			  
			DELETE ReporteLimites_Portafolio
			FROM ReporteLimites_Portafolio RP inner join ReporteLimites RL
			on RP.CodigoReporte = RL.CodigoReporte
			AND RL.CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica  
			AND RL.Escenario = @p_Escenario
			--Se debe borrar la data preexistente en LimiteValores
			
			DELETE LimiteValores
			FROM LimiteValores LV inner join ReporteLimites RL  
			on LV.CodigoReporte = RL.CodigoReporte  
			AND RL.CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica  
			AND RL.Escenario = @p_Escenario --RGF 20090924  
			-- Se debe borrar la data preexistente en ReporteLimites  
			
			DELETE ReporteLimites
			WHERE  FechaReporte = @p_Fecha AND  
			CodigoLimite = @p_CodigoLimite AND  
			CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica  
			AND Escenario = @p_Escenario

			-- Actualiza el Grupo al que pertenece cado instrumento (La Renta fija, Variable y Derivados)  
			exec usp_UPDATEGrupoTipoRentaV2 @p_Fecha
			----END

			SELECT Top 1 @TipoCalculo = L.TipoCalculo,@Tope = L.Tope,@ValorBase = L.ValorBase,@Tipo = L.Tipo,@SaldoBanco = L.SaldoBanco,
				@CodGrupoInstrumento = L.Posicion,@marketShare = ISNULL(L.MarketShare, 0),@CodigoPortafolio = LC.CodigoPortafolioSBS
			FROM Limite L, LimiteCaracteristica LC
			WHERE L.CodigoLimite = @p_CodigoLimite AND LC.CodigoLimite = @p_CodigoLimite AND LC.CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica
			SET @Escenario = @p_Escenario

			If @p_Escenario = 'ESTIMADO'
			BEGIN
				IF (SELECT COUNT(1) FROM CarteraTituloValoracion WHERE CodigoPortafolioSBS = @CodigoPortafolio AND FechaValoracion = @p_Fecha AND Escenario = @p_Escenario) = 0
					set @Escenario = 'REAL'
			END
			
			-- Obtenemos el filtro de GrupoInstrumento para el calculos de ValorBase y el proceso recursivo en base a la columna de la vista 
			DECLARE @FiltroGrupoIns VARCHAR(4000) 
			SET @FiltroGrupoIns = dbo.Limite_GenerarFiltroDeGrupoInstrumento(@CodGrupoInstrumento)
			SET @CodigoPortafolioSB = @CodigoPortafolio

			EXEC ValorBase_CalcularV2 @ValorBase,@p_fecha,@CodigoPortafolio,0,'S',@FiltroGrupoIns,@p_Escenario,@Valor out,@SaldoBancoValor out,
			@cxc out,@cxp out,@TotalInversiones  out,'0', @p_CodigoLimiteCaracteristica
								
			DECLARE @MaximoNivel INT, @VPorcentajeMShare NUMERIC(22,7)
			SET @MaximoNivel = (SELECT MAX(Secuencial) FROM NivelLimite WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Situacion = 'A')  
			-- Necesitamos multiplicar el @valorPorcentaje * @marketShare para llevarlo hasta el ultimo nivel  
			SET @VPorcentajeMShare = ISNULL(( SELECT ValorPorcentaje FROM NivelLimite WHERE Secuencial = 1 AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Situacion = 'A'), 0)    
			SET @VPorcentajeMShare = @VPorcentajeMShare * @marketShare 

			-- Empezamos a realizar los calculos de los niveles 
			EXEC dbo.Posicion_Calcular_Recursivo_NEW @p_CodigoLimite,@p_CodigoLimiteCaracteristica,1,'1=1',@FiltroGrupoIns,@p_Fecha,@CodigoPortafolio  
				,@Escenario,@MaximoNivel,@VPorcentajeMShare,@p_procesar,@p_CodigoMnemonico,@SaldoBanco,'0'

			-- Los siguientes campos recien se setean a este nivel
			UPDATE #Reporte
			SET Patrimonio=(CASE @ValorBase WHEN 'PACON' then (SELECT top 1 Patrimonio FROM TipoBalanceContable WHERE codigoEmisor = ValorNivel AND Situacion='A')  
											WHEN 'PF' then dbo.ObtenerPatrimonioValorV2(ValorNivel,@p_Fecha)   
											WHEN 'PATRIMFIDE' then (SELECT Patrimonio FROM PatrimonioFideicomiso WHERE codigoPatrimonioFideicomiso = ValorNivel AND Situacion ='A')  
											WHEN 'LA' then (SELECT LineaAsignada FROM LineasAsignadas WHERE CodigoEmisor = ValorNivel)
											WHEN 'CARTERA' THEN @Valor
											ELSE NULL end),
											
				ValorPorcentaje = ISNULL(ValorPorcentaje,0),  
				ValorPorcentajeM = ISNULL(ValorPorcentajeM,0),
				Posicion = (ISNULL(Posicion,0) + CASE WHEN @SaldoBanco='S' then ISNULL(dbo.Limite_SaldoBancos(@p_Fecha, @CodigoPortafolioSB, ValorNivel, @p_Escenario, '0'),0) ELSE 0 end),
				DescripcionNivel = LEFT(ISNULL(DescripcionNivel,''), 60) + ' ' + ISNULL(dbo.RetornarFlagEntidadVinculadaV2(@p_CodigoLimite, ISNULL(ValorNivel,'')),''),
				PorVal =  CASE WHEN ValorPorcentaje >= 0 AND @TipoCalculo = 1 THEN '%'   
							WHEN ValorPorcentaje >= 0 AND @TipoCalculo <> 1 THEN 'Val'   
							ELSE '' END,  
				Factor = dbo.ObtenerFactorLimiteV2(ValorNivel,@p_CodigoLimite,@p_Fecha),
				ValorEfectivoColocado = (CASE   
											WHEN @ValorBase='VALNOM' THEN (SELECT top 1 ValorEfectivoColocado FROM valores WHERE codigonemonico = ValorNivel)  
											WHEN @ValorBase='VALFON' THEN (SELECT top 1 NumeroUnidades FROM valores WHERE codigonemonico = ValorNivel)   
											ELSE NULL END),
											
				FloatOficioMultiple = ISNULL(dbo.ObtenerFactorEmisionLimiteV2(ValorNivel,@p_CodigoLimite,@p_Fecha) ,0),
				UnidadesEmitidas = NULL,
				TotalActivo = (CASE  WHEN @ValorBase='ACTIVO' then (SELECT top 1 TotalActivo FROM TipoBalanceContable WHERE codigoEmisor = ValorNivel AND Situacion='A')   
								WHEN @ValorBase='ACTIVOFIDE' THEN (SELECT  TotalActivo FROM Patrimoniofideicomiso WHERE codigoPatrimonioFideicomiso = ValorNivel AND Situacion='A')  
								ELSE NULL end), --RGF 20081016
								
				TotalPasivo = (CASE  WHEN @ValorBase='PASIVO' then (SELECT top 1 TotalPasivo FROM TipoBalanceContable WHERE codigoEmisor = ValorNivel AND Situacion='A')   
								WHEN @ValorBase='PASIVOFIDE' then (SELECT  TotalPasivo FROM Patrimoniofideicomiso WHERE codigoPatrimonioFideicomiso = ValorNivel AND Situacion='A')  
								ELSE NULL end),
								
				ValorNivel =  ValorNivel,
				NivelSaldoBanco = ISNULL(dbo.Limite_SaldoBancos(@p_Fecha, @CodigoPortafolioSB, ValorNivel, @p_Escenario, '0'),0),
				Valor1 = 0,Valor2 = 0,Valor3 = 0								
				,MontoValorBase= CASE WHEN @ValorBase IN ('PATE' /*Patrimonio de la Entidad*/ , 'PASE' /*Pasivo de la Entidad*/ , 'ACTIVO_ENTIDAD' /*Activo de la Entidad*/ , 'OBLIG_TECNICAS' /*Obligacion Tecnica*/)
										THEN MontoValorBase ELSE @Valor END
			
			-- Eliminamos los registros que no cumplan con la restriccion de abajo
			DELETE #Reporte_Fondos
			FROM #Reporte_Fondos rf inner join #Reporte r
			ON rf.CodigoReporte = r.CodigoReporte
			--WHERE not(@NumeroNiveles > r.Secuencial or (@NumeroNiveles = r.Secuencial AND ISNULL(r.Posicion,0) <> 0))  
			WHERE not(@NumeroNiveles > r.Secuencial or (@NumeroNiveles = r.Secuencial AND ISNULL(r.Posicion,0) >= 0))  

			--DELETE #Reporte WHERE not(@NumeroNiveles > Secuencial or (@NumeroNiveles = Secuencial AND ISNULL(Posicion,0) <> 0))  
			DELETE #Reporte WHERE not(@NumeroNiveles > Secuencial or (@NumeroNiveles = Secuencial AND ISNULL(Posicion,0) >= 0))  

			--UPDATE #Reporte set Posicion = 0 WHERE ValorNivel = 'SaldoBanco4'			
			--IF @p_procesar = 2  SET @p_Escenario = 'ONLINE'
			
			DECLARE @CodigoReporte NUMERIC(12,0)
			SET @CodigoReporte = (SELECT MAX(tb.maxreg) 
				FROM (SELECT maxreg = ISNULL(MAX(CodigoReporte),0) FROM ReporteLimites 
					union SELECT ISNULL(MAX(CodigoReporte),0) FROM ReporteLimites_Portafolio) tb);			


			INSERT INTO ReporteLimites(CodigoReporte,CodigoLimite,CodigoLimiteCaracteristica,FechaReporte,Secuencial,CodigoNivelLimite,Tipo,
				Tope,ValorPorcentaje,Posicion,Participacion,Margen,Alerta,ValorNivel,DescripcionNivel,PorVal,ValorBase, cxc, cxp,Patrimonio,TotalInversiones ,
				SaldoBanco,Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,CodigoMoneda,FechaOperacion,
				Escenario,NivelSaldoBanco,ValorPorcentajeM,Valor1,Valor2,Valor3, CodigoCaracteristicaRelacion, CodigoRelacion)

				SELECT 
					'CodigoReporte' = CodigoReporte + @CodigoReporte,
					@p_CodigoLimite,
					@p_CodigoLimiteCaracteristica,
					@p_Fecha,
					Secuencial, 
					CodigoNivelLimite,
					@Tipo,
					@Tope,
					ValorPorcentaje,						
					Posicion,
					'Participacion' = CASE WHEN isnull(MontoValorBase,0) > 0 THEN (Posicion / isnull(MontoValorBase,0)) * 100 /* Es Porcentaje*/ ELSE 0 END,
					Margen = MontoValorBase - Posicion,	
					'Alerta' = '0',
					ValorNivel,
					DescripcionNivel,
					'Simbolo' = PorVal,
					MontoValorBase, 										
					@cxc, 
					@cxp,
					Patrimonio,
					'TotalInversion' = CASE @ValorBase WHEN 'OBLITEC' THEN  (SELECT top 1 isnull(Valor,0) FROM PatrimonioEmisor WHERE TipoValor = 'OT' AND CodigoEntidad = rp.ValorNivel) ELSE @TotalInversiones END,				
					
					@SaldoBancoValor,RP.Factor,RP.ValorEfectivoColocado,RP.FloatOficioMultiple,RP.UnidadesEmitidas,RP.TotalActivo,RP.TotalPasivo,RP.FechaVencimiento,
					RP.CodigoMoneda,RP.FechaOperacion,@p_Escenario,RP.NivelSaldoBanco,RP.ValorPorcentajeM,RP.Valor1,RP.Valor2,RP.Valor3, RP.CodigoCaracteristicaRelacion, RP.CodigoRelacion
					
				FROM #Reporte RP
				
			/*CRumiche: Solo mostramos la info isertada pero esto no tendria utilidad en este proceso puntual */
			SELECT CodigoReporte	,CodigoLimite	,CodigoLimiteCaracteristica	,FechaReporte	,Secuencial	,CodigoNivelLimite	,Tipo	,Tope	
				,ValorPorcentaje	,Posicion	,Participacion	,Margen	,Alerta	,ValorNivel	,DescripcionNivel	,PorVal	,Patrimonio	,ValorBase
				,Cxc	,Cxp	,TotalInversiones	,SaldoBanco
			-- select *
			FROM ReporteLimites 
			WHERE CodigoLimite = @p_CodigoLimite AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND FechaReporte = @p_Fecha

			
			DROP TABLE #Reporte --Eliminamos la tabla temporal  
END
GO

GRANT EXECUTE ON [Limite_ProcesarLimite_NEW] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[Limite_SaldoBancos_ClaseActivo]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Limite_SaldoBancos_ClaseActivo') BEGIN 
	DROP FUNCTION [dbo].[Limite_SaldoBancos_ClaseActivo]
END 
GO
------------------------------------------------------------------------------------------------------------------------
--	Fecha de Creación:	27/06/2019
--	Creado por:			Diego tueros
--	Nro. Orden de Trabajo:	12163
--	Descripción del cambio:	Obtener los saldos bancarios de un fondo por Portafolio y Fecha.
------------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[Limite_SaldoBancos_ClaseActivo]
(
	@p_CodigoPortafolio VARCHAR(10),
	@p_Fecha NUMERIC(8),
	@p_CodigoClaseCuenta VARCHAR(10)
)
RETURNS NUMERIC(22,7)
AS
BEGIN

	DECLARE @p_saldo NUMERIC(22,7)
	DECLARE @p_FechaSaldo NUMERIC(22,7)

	--Inversiones  
	SET @p_FechaSaldo = (  
	SELECT MAX(SB.FechaOperacion)  
	FROM SaldosBancarios SB (NOLOCK)
	INNER JOIN CuentaEconomica CE ON SB.CodigoPortafolioSBS = CE.CodigoPortafolioSBS  
		AND SB.NumeroCuenta = CE.NumeroCuenta AND CE.CodigoClaseCuenta = @p_CodigoClaseCuenta AND CE.Situacion = 'A'  
	WHERE SB.CodigoPortafolioSBS = @p_CodigoPortafolio
	AND SB.FechaOperacion <= @p_Fecha  
	)  

	SELECT @p_saldo = SUM(DBO.sp_SIT_OBT_CambioMoneda(@p_CodigoPortafolio,SB.SaldoDisponibleFinalConfirmado,sb.FechaOperacion,CE.CodigoMoneda,P.CodigoMoneda))  
	FROM SaldosBancarios (NOLOCK) SB
	JOIN CuentaEconomica CE ON CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND SB.NumeroCuenta = CE.NumeroCuenta  AND SB.CodigoMoneda = CE.CodigoMoneda  
	AND CE.CodigoClaseCuenta = @p_CodigoClaseCuenta AND CE.Situacion = 'A'
	JOIN Portafolio P ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS  
	WHERE SB.CODIGOPORTAFOLIOSBS = @p_CodigoPortafolio AND SB.FechaOperacion = @p_FechaSaldo

	return isnull(@p_saldo,0)

END
GO

GRANT EXECUTE ON [Limite_SaldoBancos_ClaseActivo] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[ObtenerMontoSaldoNoAdministrado]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ObtenerMontoSaldoNoAdministrado') BEGIN 
	DROP FUNCTION [dbo].[ObtenerMontoSaldoNoAdministrado]
END 
GO

------------------------------------------------------------------------------------------------------------------------
--	Fecha de Creación:	02/08/2019
--	Creado por:			Diego tueros
--	Nro. Orden de Trabajo:	12163
--	Descripción del cambio:	Obtener los montos de saldos no administrados por tecero segun el limite
------------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION ObtenerMontoSaldoNoAdministrado 
(
--DECLARE
	@CodigoTercero as varchar(12) --= '20255993225'
	--,@TipoCuenta as varchar(2) --= '1'
	,@CodigoMonedaPortafolio as varchar(10) --='NSOL'
	,@Fecha	as decimal(8) --= 20190628
	,@vMes as varchar(10)
	,@Anhio as varchar(10)
	,@CodigoLimite as varchar(5)
)
RETURNS numeric(22,7)
AS
BEGIN
	declare @saldo numeric(22,7) = 0
	Declare @tipoCuenta as varchar(2) = ''

	IF(@CodigoLimite = '302' OR @CodigoLimite = '303' OR @CodigoLimite = '304' OR @CodigoLimite = '308')
	BEGIN
		
		DECLARE db_cursorDetalle CURSOR FOR 
		SELECT CodigoValidador FROM ValidadorLimiteDetalle where CodigoLimite = @CodigoLimite 
		OPEN db_cursorDetalle
		FETCH NEXT FROM db_cursorDetalle INTO @tipoCuenta  	

		WHILE @@FETCH_STATUS = 0
		BEGIN	

			IF(@CodigoMonedaPortafolio = 'NSOL')
			BEGIN

				SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
				(
					select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero = @CodigoTercero
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
						
					UNION ALL
					
					Select (isnull(sum(Saldo),0) * dbo.RetornarTipoCambio(CodigoMoneda,@Fecha)) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero = @CodigoTercero
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					group by CodigoMoneda
				) a
			END
			ELSE IF (@CodigoMonedaPortafolio = 'DOL')
			BEGIN
				SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
				(
				
					select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero = @CodigoTercero
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
					UNION
				
					Select (isnull(sum(Saldo),0)  / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,@Fecha)) Monto from SaldosNoAdministrados where CodigoTercerFinanciero = @CodigoTercero
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
				) a
			END
			ELSE
			BEGIN
				Select @saldo = @saldo + 0
			END

		FETCH NEXT FROM db_cursorDetalle INTO @tipoCuenta 
		END 

		CLOSE db_cursorDetalle  
		DEALLOCATE db_cursorDetalle

		
	END
	ELSE IF(@CodigoLimite = '305')
	BEGIN

		DECLARE db_cursorDetalle CURSOR FOR 
		SELECT CodigoValidador FROM ValidadorLimiteDetalle where CodigoLimite = @CodigoLimite	 
		OPEN db_cursorDetalle
		FETCH NEXT FROM db_cursorDetalle INTO @tipoCuenta  	

		WHILE @@FETCH_STATUS = 0
		BEGIN	

			IF(@CodigoMonedaPortafolio = 'NSOL')
			BEGIN

				SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
				(
					select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (SELECT e.CodigoTercero FROM Entidad E where e.CodigoGrupoEconomico = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'

					UNION ALL
				
					Select (isnull(sum(Saldo),0) * dbo.RetornarTipoCambio(CodigoMoneda,@Fecha)) Saldo  from SaldosNoAdministrados where CodigoTercerFinanciero in (SELECT e.CodigoTercero FROM Entidad E where e.CodigoGrupoEconomico = @CodigoTercero)					
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					group by CodigoMoneda
				) a
			END
			ELSE IF (@CodigoMonedaPortafolio = 'DOL')
			BEGIN
				SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
				(
					--select Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (SELECT e.CodigoTercero FROM Entidad E where e.CodigoGrupoEconomico = @CodigoTercero)
					--AND TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
					select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (SELECT e.CodigoTercero FROM Entidad E where e.CodigoGrupoEconomico = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
					UNION

					--Select (Saldo / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,Fecha)) Monto from SaldosNoAdministrados where CodigoTercerFinanciero in (SELECT e.CodigoTercero FROM Entidad E where e.CodigoGrupoEconomico = @CodigoTercero)
					--AND TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					Select (isnull(sum(Saldo),0)  / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,@Fecha)) Monto from SaldosNoAdministrados where CodigoTercerFinanciero in (SELECT e.CodigoTercero FROM Entidad E where e.CodigoGrupoEconomico = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
				) a
			END
			ELSE
			BEGIN
				Select @saldo = @saldo + 0
			END

		FETCH NEXT FROM db_cursorDetalle INTO @tipoCuenta 
		END 

		CLOSE db_cursorDetalle  
		DEALLOCATE db_cursorDetalle
		
	END
	ELSE IF(@CodigoLimite = '307')
	BEGIN
		DECLARE db_cursorDetalle CURSOR FOR 
		SELECT CodigoValidador FROM ValidadorLimiteDetalle where CodigoLimite = @CodigoLimite	 
		OPEN db_cursorDetalle
		FETCH NEXT FROM db_cursorDetalle INTO @tipoCuenta  	
		
		WHILE @@FETCH_STATUS = 0
		BEGIN	
			IF(@CodigoMonedaPortafolio = 'NSOL')
			BEGIN

				SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
				(
					--select Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					--AND TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
					select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'

					UNION ALL

					--Select Saldo * dbo.RetornarTipoCambio(CodigoMoneda,Fecha) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					--AND TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					Select (isnull(sum(Saldo),0) * dbo.RetornarTipoCambio(CodigoMoneda,@Fecha)) Saldo  from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					group by CodigoMoneda
				) a
			END
			ELSE IF (@CodigoMonedaPortafolio = 'DOL')
			BEGIN
				SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
				(
					--select Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					--AND TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
					select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'

					UNION

					--Select (Saldo / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,Fecha)) Monto from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					--AND TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					Select (isnull(sum(Saldo),0)  / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,@Fecha)) Monto from SaldosNoAdministrados where CodigoTercerFinanciero in (select CodigoTercero from Terceros where SectorGigs = @CodigoTercero)
					AND TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
				) a
			END
			ELSE
			BEGIN
				Select @saldo = @saldo + 0
			END

		FETCH NEXT FROM db_cursorDetalle INTO @tipoCuenta 
		END 

		CLOSE db_cursorDetalle  
		DEALLOCATE db_cursorDetalle
	END
	ELSE IF(@CodigoLimite = '310')
	BEGIN
			IF(@CodigoTercero = '1')
			BEGIN
				SET @TipoCuenta = '2'
			END
			ELSE IF(@CodigoTercero = '2')
			BEGIN
				SET @TipoCuenta = '1'
			END
			ELSE
			BEGIN
				SET @tipoCuenta = '0'
			END

				IF(@CodigoMonedaPortafolio = 'NSOL')
				BEGIN

					SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
					(
						--select Saldo from SaldosNoAdministrados where TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
						select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio 
						and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'

						UNION ALL

						--Select Saldo * dbo.RetornarTipoCambio(CodigoMoneda,Fecha) Saldo from SaldosNoAdministrados 
						--where  TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
						Select (isnull(sum(Saldo),0) * dbo.RetornarTipoCambio(CodigoMoneda,@Fecha)) Saldo  from SaldosNoAdministrados 
						where  TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
						group by CodigoMoneda
					) a
				END
				ELSE IF (@CodigoMonedaPortafolio = 'DOL')
				BEGIN
					SELECT @saldo = @saldo + isnull(SUM(a.Saldo),0) FROM
					(
						--select Saldo from SaldosNoAdministrados where TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
						select isnull(sum(Saldo),0) Saldo from SaldosNoAdministrados where TipoCuenta = @TipoCuenta 
						and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda = @CodigoMonedaPortafolio and Situacion ='A'
						
						UNION

						--Select (Saldo / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,Fecha)) Monto from SaldosNoAdministrados where TipoCuenta = @TipoCuenta and Fecha = @Fecha and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
						Select (isnull(sum(Saldo),0) / dbo.RetornarTipoCambio(@CodigoMonedaPortafolio,@Fecha)) Saldo from SaldosNoAdministrados 
						where TipoCuenta = @TipoCuenta and SUBSTRING(cast(fecha as varchar(10)),1,4) = @Anhio and SUBSTRING(cast(fecha as varchar(10)),5,2) = @vMes and CodigoMoneda <> @CodigoMonedaPortafolio and Situacion ='A'
					) a
				END
				ELSE
				BEGIN
					Select @saldo = @saldo + 0
				END
	END
	return @saldo
END
GO
GRANT EXECUTE ON [dbo].[ObtenerMontoSaldoNoAdministrado] TO [rol_sit_fondos] AS [dbo]  
GO





PRINT '[Posicion_Calcular_Recursivo_NEW]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Posicion_Calcular_Recursivo_NEW') BEGIN 
	DROP PROCEDURE [dbo].[Posicion_Calcular_Recursivo_NEW]
END 
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Carlos Rumiche.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Posicion_Calcular_Recursivo_NEW
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Modificar la opcion secuencia en duro por el nivel mas bajo del limite
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Posicion_Calcular_Recursivo_NEW]
	 @p_CodigoLimite Varchar(5)
	,@p_CodigoLimiteCaracteristica Varchar(5)
	,@p_Secuencial VARCHAR(5)
	,@p_Filtro  VARCHAR(8000)
	,@p_FiltroGrupo  VARCHAR(4000)
	,@p_Fecha Numeric(8,0)
	,@p_CodigoPortafolioSBS VARCHAR(10)
	,@p_Escenario varchar(20) 
	,@p_MaximoNivel int
	,@p_ValorPorcentajeMarketShare numeric(20,7)
	,@p_procesar Numeric(8,0) = 1
	,@p_CodigoMnemonico varchar(15) = ''
	,@p_SaldoBanco varchar(1) = ''
	,@p_CodMultifondo varchar(10)
AS
BEGIN
	PRINT 'Posicion_Calcular_Recursivo_NEW'
	DECLARE @SQL Varchar(1000),
	@ValorEntidad Varchar(100),
	@CodigoCaracteristica Varchar(5),
	@CodigoNivelLimite Varchar(5),
	@porcentaje decimal(22,7),
	@decPorcentaje decimal(22,7),
	@ValorPosicion decimal(22,7),
	@ValorPosicionF1 decimal(22,7), 
	@ValorPosicionF2 decimal(22,7), 
	@ValorPosicionF3 decimal(22,7),
	@NombreTabla varchar(256),
	@IndFwdSwp varchar(1),
	@ContIndSB  numeric(10),
	@ContSB numeric(10),
	@SaldoBanco varchar(1),
	@porcentajeM decimal(22,7)
	
	--Obtiene los datos de agrupamiento de acuerdo al nive del limite para saber si es G=General, A=Agrupado, D=Detallado
	SELECT top 1 @CodigoCaracteristica = CodigoCaracteristica,@CodigoNivelLimite = CodigoNivelLimite,@decPorcentaje = isnull(ValorPorcentaje,0)
  	FROM NivelLimite
  	WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial=@p_Secuencial AND Situacion = 'A'
	
	-- Caracteristica TODO, solo sirve para calculo de limite en una agrupacion global
	IF @CodigoCaracteristica = '00' 
	BEGIN
	
		DECLARE @intCont numeric(10) = 1

		--while @intCont < @p_MaximoNivel
		--BEGIN
		--	if(@intCont=1)
		--	BEGIN
				SELECT top 1 @CodigoCaracteristica = CodigoCaracteristica,@CodigoNivelLimite = CodigoNivelLimite,@decPorcentaje = isnull(ValorPorcentaje,0)
  				FROM NivelLimite
  				WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial=@intCont AND Situacion = 'A'

				INSERT INTO #Reporte(Secuencial, CodigoNivelLimite, ValorPorcentaje, Posicion, ValorNivel, DescripcionNivel)
				VALUES( @intCont, @CodigoNivelLimite, @decPorcentaje, 0, @ValorEntidad, 'SUMA VALORES DE LOS GRUPOS')
		--	END
		--	ELSE
		--	BEGIN
		--		SELECT top 1 @CodigoCaracteristica = CodigoCaracteristica,@CodigoNivelLimite = CodigoNivelLimite,@decPorcentaje = isnull(ValorPorcentaje,0)
  --				FROM NivelLimite
  --				WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial=@intCont AND Situacion = 'A'

		--		INSERT INTO #Reporte(Secuencial, CodigoNivelLimite, ValorPorcentaje, Posicion, ValorNivel, DescripcionNivel)
		--		VALUES( @intCont, @CodigoNivelLimite, @decPorcentaje, 0, @ValorEntidad, 'SUMA VALORES DE LOS SUB-GRUPOS')
		--	END
				
		--	SELECT @intCont = @intCont + 1
		--END
		----INSERT INTO #Reporte(Secuencial, CodigoNivelLimite, ValorPorcentaje, Posicion, ValorNivel, DescripcionNivel)
		----VALUES( @p_Secuencial, @CodigoNivelLimite, @decPorcentaje, 0, @ValorEntidad, 'SUMA VALORES DE LOS GRUPOS')

		EXEC dbo.Posicion_Calcular_Recursivo_NEW
			 @p_CodigoLimite
			,@p_CodigoLimiteCaracteristica
			,@p_MaximoNivel
			,'1=1'
			,@p_FiltroGrupo
			,@p_Fecha
			,@p_CodigoPortafolioSBS
			,@p_Escenario
			,@p_MaximoNivel
			,@p_ValorPorcentajeMarketShare
			,@p_procesar	--HDG INC 61360	20101013
			,@p_CodigoMnemonico	--HDG INC 61360	20101013
			,@p_SaldoBanco
			,@p_CodMultifondo
		return
	END	
	
	Declare @NombreColumna Varchar(256),@NombreColumnaVista Varchar(50),@Vista Varchar(50)
  	Select	@NombreColumna = NombreColumna, @NombreColumnaVista = NombreColumnaVista,  @Vista = NombreVista,@NombreTabla = isnull(NombreTabla,'')
	From CaracteristicaGrupo where CodigoCaracteristica = @CodigoCaracteristica
	
	--Ahora enviamos la vista como parametro y la columna de la vista para hacerlo mas dinamico
	--LLenamaos la tabla #ValoresNivel
	Exec dbo.Posicion_CrearFiltroEnProcesoLimite  @p_CodigoLimite, @p_Secuencial,  @p_CodigoPortafolioSBS
	PRINT  'STATUS C: ' + CAST( CURSOR_STATUS('global','cur_Nivel')  AS VARCHAR(100))
	IF CURSOR_STATUS('global','cur_Nivel')  = 1
		DEALLOCATE cur_Nivel
	
	PRINT 'ENTRO AL RECURSIVO'

	DECLARE cur_Nivel CURSOR FOR
		SELECT Valor,NombreColumna,NombreVista,CodigoCaracteristica,CodigoNivelLimite FROM #ValoresNivel
	OPEN cur_Nivel
		FETCH NEXT FROM cur_Nivel INTO @ValorEntidad,@NombreColumna,@Vista,@CodigoCaracteristica,@CodigoNivelLimite
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			print  'Operacion: ' + @NombreColumna			
			print 'Vista: ' + @Vista
			print 'NombreColumns: ' + @NombreColumna
			PRINT '@ValorEntidad: ' + @ValorEntidad
			-- Aqui se trata de procesar los  registro de ##ValoresNivel en base al secuencial
			--SELECT @ValorEntidad = Valor FROM #ValoresNivel WHERE Orden = @indNV AND Secuencial = @p_Secuencial
			--OT12012
			Declare @FiltroHeredado  VARCHAR(1000), @EsUltimoNivel bit, @CodigoCaracteristicaRelacion varchar(100), @CodigoRelacion varchar(100)
			--Cuando la caracteristica es 32 - Segun Moneda solo se envia el valor de la tabla mas no el filtro
			IF LEN(ISNULL(@NombreColumna,'')) > 0
				SET @FiltroHeredado = @p_Filtro + ' AND ' + @NombreColumna + ' = ''' + @ValorEntidad+ ''''
			
			PRINT '@FiltroHeredado: ' + @FiltroHeredado

			Set @EsUltimoNivel = (case when @p_Secuencial = @p_MaximoNivel then 1 else 0 end)
			Update #Posicion_Fondos set PosicionPortafolio = 0
			
			declare @p_MontoValorBase NUMERIC(22,7)
			
			EXEC dbo.Posicion_Calcular_Reporte_NEW @p_CodigoLimite,@CodigoCaracteristica,@p_CodigoLimiteCaracteristica,@CodigoNivelLimite,
			@p_fecha,@p_CodigoPortafolioSBS,@ValorEntidad,@p_FiltroGrupo,@FiltroHeredado,@p_Escenario, @EsUltimoNivel,@p_ValorPorcentajeMarketShare,
			@Porcentaje out,@PorcentajeM out,@ValorPosicion out, @p_MontoValorBase out, @p_procesar,@p_CodigoMnemonico,@p_CodMultifondo			
			
			PRINT '@Porcentaje: ' + CAST(@Porcentaje AS VARCHAR)
				
			IF (NOT (@Porcentaje IS NULL)) or @p_SaldoBanco = 'M' 
			BEGIN							
				Declare @Descripcion varchar(200),  @SQLd nvarchar(300)
		  		DECLARE @FechaVencimiento numeric(8,0),@FechaOperacion numeric(8),@CodigoMoneda varchar(10)
		  		
		  		print '@EsUltimoNivel '+  cast(@EsUltimoNivel as varchar)
		  		print '@ValorPosicion '+  cast(@ValorPosicion as varchar)
		  		print '@p_SaldoBanco '+  cast(@p_SaldoBanco as varchar)
		  		
				IF (NOT (@EsUltimoNivel = 1 and @ValorPosicion < 0 and @p_SaldoBanco <> 'S')) OR @p_SaldoBanco = 'M'
				BEGIN
					print 'Operacion xxx: ' + @NombreColumna
					
					IF  @NombreColumna = 'Operacion'  
					BEGIN
						Set @Descripcion = @ValorEntidad
						Select @FechaVencimiento = FechaContrato, @FechaOperacion = FechaOperacion,@CodigoMoneda = CodigoMoneda
						From OrdenInversion
						Where numeroPoliza = @ValorEntidad And CodigoPortafolioSBS = @p_CodigoPortafolioSBS	--Un filtro por fondo para obtener los datos correctos
					END
					Else 
					BEGIN
						set @Descripcion = null 
						If @Vista = 'VT_GrupoInstrumento'
		     				SET @SQLd = 'SELECT @Descripcion = Codigo + ''-'' + Descripcion FROM ' + @Vista + ' WHERE Codigo = ''' + @ValorEntidad+''''
						Else if @NombreColumna ='CodigoPatrimonioFideicomiso'
							SET @SQLd = 'SELECT @Descripcion = Descripcion FROM Patrimoniofideicomiso WHERE CodigoPatrimonioFideicomiso = ''' + @ValorEntidad+''''
						Else if @NombreColumna = 'CodigoMercado'				
							SET @SQLd = 'SELECT @Descripcion = Descripcion FROM ' +@NombreTabla +' WHERE  CodigoMercado =  ''' + @ValorEntidad+''''
						Else if @NombreColumna = 'CriterioLiquidez'
							SET @SQLd = 'SELECT @Descripcion = Nombre FROM  ParametrosGenerales WHERE Clasificacion =  ''CriterioLi'' and valor = ''' + @ValorEntidad+''''
						Else if @NombreColumna = 'DerivadoXPrima'							
							Set	@Descripcion = @ValorEntidad	
						Else if @Vista = 'VT_CategoriaInstrumento' 
							BEGIN
								IF @p_Secuencial = 1
									SET @SQLd = 'SELECT @Descripcion = GrupoCategoria + ''-'' + Descripcion FROM ' + @Vista + ' WHERE GrupoCategoria = ''' + @ValorEntidad+''''
								ELSE IF @p_Secuencial = 2
									SET @SQLd = 'SELECT @Descripcion = ''' + @ValorEntidad + '-'' + Nombre FROM ParametrosGenerales WHERE Clasificacion = ''SubClaseAc'' AND Valor = ''' + @ValorEntidad + ''''									
							END
						ELSE
							
		     				SET @SQLd = 'SELECT @Descripcion = Descripcion FROM ' + @Vista + ' WHERE Codigo = ''' + @ValorEntidad+''''

						print '@NombreColumna 	: ' + isnull(@NombreColumna,'NULL')
						print '@NombreTabla 	: ' + isnull(@NombreTabla,'NULL')
						print '@ValorEntidad 	: ' + isnull(@ValorEntidad,'NULL')
						print '@Vista			: ' + isnull(@Vista,'NULL')
						print 'Descripcion @SQLd:'  + isnull(@SQLd,'NULL')
		      			exec sp_executesql @SQLd, N'@Descripcion varchar(200) out', @Descripcion out
					END
					
					--OT12012
					SELECT  
							 @CodigoCaracteristicaRelacion = CodigoCaracteristicaRelacion,
							 @CodigoRelacion = CodigoRelacion
					FROM   detallenivellimite2 
					WHERE  codigonivellimite = @CodigoNivelLimite 
					AND    codigocaracteristica = @CodigoCaracteristica 
					AND    valorcaracteristica = @ValorEntidad    
					--Fin


					INSERT INTO #Reporte(Secuencial, CodigoNivelLimite, ValorPorcentaje, ValorNivel, DescripcionNivel,Posicion,
						FechaVencimiento,FechaOperacion,CodigoMoneda,IndFwdSwp,ValorPorcentajeM,Alerta, MontoValorBase, CodigoCaracteristicaRelacion, CodigoRelacion)
					VALUES(
						@p_Secuencial, @CodigoNivelLimite,@Porcentaje , @ValorEntidad, CAST( @Descripcion as varchar(100)),
						isnull(@ValorPosicion,0) ,@FechaVencimiento,@FechaOperacion,@CodigoMoneda,@IndFwdSwp,
						@PorcentajeM,substring(@p_Filtro,19,1), @p_MontoValorBase, @CodigoCaracteristicaRelacion, @CodigoRelacion)
					
			 		If @p_CodMultifondo = @p_CodigoPortafolioSBS
						INSERT INTO #Reporte_Fondos(CodigoReporte, CodigoPortafolio, PosicionPortafolio, ValorNivel )
						Select @@identity,CodigoPortafolio,PosicionPortafolio,@ValorEntidad
						from #Posicion_Fondos
				END
				
				
				--Declare @tempGrupoTipoRenta varchar(2)
				--Set @tempGrupoTipoRenta = substring(@p_Filtro,27,1)
				-- Esta parte forma la recursividad
				-- Si aun hay un nivel por explorar continuamos con la recursividad
				--Siempre que haya un nivel 3 en adelante
				IF @p_Secuencial < @p_MaximoNivel
				BEGIN 				
					DECLARE @CodigoNivelSiguiente VARCHAR(5)
			   		SET @CodigoNivelSiguiente = convert(varchar(5),convert(int,@p_Secuencial)+1)
					IF @ContSB > 0
					BEGIN
						SET @SaldoBanco = @p_SaldoBanco
						SET @p_SaldoBanco = 'M'
					END
					EXEC dbo.Posicion_Calcular_Recursivo_NEW @p_CodigoLimite,@p_CodigoLimiteCaracteristica,@CodigoNivelSiguiente,@FiltroHeredado,
					@p_FiltroGrupo,@p_Fecha,  @p_CodigoPortafolioSBS,@p_Escenario,@p_MaximoNivel,@p_ValorPorcentajeMarketShare,@p_procesar,
					@p_CodigoMnemonico,@p_SaldoBanco,@p_CodMultifondo
					IF @ContSB > 0
					BEGIN
						SET @p_SaldoBanco = @SaldoBanco
					END
				END
			END
			FETCH NEXT FROM cur_Nivel INTO @ValorEntidad,@NombreColumna,@Vista,@CodigoCaracteristica,@CodigoNivelLimite
		END
	CLOSE cur_Nivel 
	DEALLOCATE cur_Nivel
	DELETE #ValoresNivel WHERE Secuencial = @p_Secuencial
END
GO

GRANT EXECUTE ON [dbo].[Posicion_Calcular_Recursivo_NEW] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[Posicion_Calcular_Reporte_NEW]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Posicion_Calcular_Reporte_NEW') BEGIN 
	DROP PROCEDURE [dbo].[Posicion_Calcular_Reporte_NEW]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Carlos Rumiche.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Modificar la opcion secuencia en duro por el nivel mas bajo del limite
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Modificar la opcion secuencia en duro por el nivel mas bajo del limite
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Posicion_Calcular_Reporte_NEW] 
	@p_CodigoLimite               VARCHAR(5), 
	@p_CodigoCaracteristica       VARCHAR(5), 
	@p_CodigoLimiteCaracteristica VARCHAR(5), 
	@p_CodigoNivelLimite          VARCHAR(5), 
	@p_fecha                      NUMERIC(8), 
	@p_CodigoPortafolioSBS        VARCHAR(10), 
	@p_ValorEntidad               VARCHAR(100), 
	@p_FiltroGrupo                VARCHAR(8000), 
	@p_Filtro                     VARCHAR(8000), 
	@p_Escenario                  VARCHAR(20), 
	@p_EsUltimoNivel              BIT, 
	@p_ValorPorcentajeMarketShare NUMERIC(20,7), 
	
	@p_Porcentaje                 NUMERIC(20,7) out, 
	@p_PorcentajeM                NUMERIC(20,7) out, 
	@p_Valor                      NUMERIC(22,7) out, 
	@p_MontoValorBase			  NUMERIC(22,7) out,
		
	@p_procesar                   NUMERIC(8,0) = 1, 
	@p_CodigoMnemonico            VARCHAR(15) = '', 
	@p_CodMultifondo              VARCHAR(10) 
AS 
BEGIN 
    DECLARE @CMDSQL           NVARCHAR(4000), 
		@SQL_FROM                NVARCHAR(4000), 
		@claseNormativa         VARCHAR(4), 
		@CodigoGrupoInstrumento VARCHAR(5), 
		@Filtro                 VARCHAR(8000) = '', /* inicializamos */
		@ValorBase              VARCHAR(100), 
		@CodigoReporte          NUMERIC(18), 
		@CMDSQLF                NVARCHAR(4000),  
		@UnidadPosicion         VARCHAR(5), 
		@vIndOpcion             INT = 0, /* inicializamos */
		@nvcNombreVista         NVARCHAR(256), 
		@columna                VARCHAR(256),
		@AplicarCastigoRating	VARCHAR(1) = 'N',
		@NivelesCastigo			INT = 0,
		@v_texto_case			varchar(250), /*CRumiche: Solo util para obtener valores de CASE WHEN (Para generar claridad de CËDIGO)*/

		@NombreTabla			VARCHAR(256), /* De la tabla CARACTERISTICA */
		@SQL_SELECT_CARAC		VARCHAR(2048), /* De la tabla CARACTERISTICA */
		@SQL_SELECT_SOLO_DPZ	VARCHAR(2048), /* De la tabla CARACTERISTICA */		
		@SQL_FROM_CARAC			VARCHAR(2048), /* De la tabla CARACTERISTICA */
		@SQL_WHERE_CARAC		VARCHAR(2048), /* De la tabla CARACTERISTICA */
		@ES_AGRUPADOR			VARCHAR(1), /* De la tabla CARACTERISTICA */

		@TieneValorEspecifico	VARCHAR(1), /*De la tabla DetalleNivelLimite2*/
		@TieneLimiteFijoEnDetalle VARCHAR(1), /*De la Tabla NivelLimite*/
		@LimiteFijoEnDetalleMin NUMERIC(22,7), /*De la Tabla NivelLimite*/
		@LimiteFijoEnDetalleMax	NUMERIC(22,7), /*De la Tabla NivelLimite*/

		@AplicaForward			VARCHAR(1), /* De la Tabla Limite*/
		@AplicaDPZ				VARCHAR(1), /* De la Tabla Limite*/
		@AplicaSaldoBanco		VARCHAR(1), /* De la Tabla Limite*/ -- Soporte para Saldo Bancos en la Posicion de cada Caracteristica
		@CodigoMonedaPortafolio VARCHAR(100) /* De la Tabla Portafolio*/,
		@CodigoCaracteristicaRelacion varchar(100),
		@CodigoRelacion varchar(100),
		@FlagSaldoInstrumento char(1)='0',
		@FlagSaldoBancos char(1) = '0'
    
	PRINT '@p_ValorEntidad REPORTE V2: ' + @p_ValorEntidad
	        
    SET @p_Valor = 0 /* inicializamos */

    SELECT TOP 1 
           @CodigoGrupoInstrumento	= l.posicion , 
           @ValorBase				= l.valorbase , 
           @UnidadPosicion			= l.unidadposicion ,
		   @AplicarCastigoRating	= ISNULL(l.TieneCastigo,'N'),
		   @NivelesCastigo			= ISNULL(l.CastigoRating,0),
		   @AplicaForward			= ISNULL(l.AplicaForward,'N'),
		   @AplicaDPZ				= ISNULL(l.AplicaDPZ,'N'),
		   @AplicaSaldoBanco		= ISNULL(l.SaldoBanco,'N')
    FROM Limite L WHERE  codigolimite = @p_CodigoLimite
    
    /* Requerido para los limites de Moneda */
	SELECT 
		@CodigoMonedaPortafolio = CodigoMoneda
	FROM Portafolio where CodigoPortafolioSBS = @p_CodigoPortafolioSBS

	print 'ValorBase: ' + @ValorBase

	/*CRumiche: Cßlculo del Valor Base a nivel de Item de Caracteristica en forma dinßmica*/
    IF @ValorBase IN ('PATE' /*Patrimonio de la Entidad*/ 
					, 'PASE' /*Pasivo de la Entidad*/ 
					, 'ACTIVO_ENTIDAD' /*Activo de la Entidad*/ 
					, 'OBLIG_TECNICAS' /*Obligacion Tecnica*/ )
    BEGIN
		/*CRumiche: Primero obtenemos el query para el MontoValorBase, luego reemplazamos las variables posibles utilizadas*/			
		DECLARE @SQL_CALCULO_VALOR NVARCHAR(2048)
		SELECT @SQL_CALCULO_VALOR = SQL_CALCULO_VALOR FROM [ValorBaseLimite] WHERE CodigoValorBase = @ValorBase
		SET @SQL_CALCULO_VALOR = REPLACE(@SQL_CALCULO_VALOR, '{FECHA_PROCESO}', @p_fecha)
		SET @SQL_CALCULO_VALOR = REPLACE(@SQL_CALCULO_VALOR, '{VALOR_CARACTERISTICA}', @p_ValorEntidad)		
		SET @SQL_CALCULO_VALOR = REPLACE(@SQL_CALCULO_VALOR, '{CODIGO_PORTAFOLIO}', @p_CodigoPortafolioSBS)				
		print 'Script Cßlculo de Valor Base @SQL_CALCULO_VALOR: ' + isnull(@SQL_CALCULO_VALOR,'')
		
		/*CRumiche. Ahora ejecutamos la query armada. Es indispensable que el QUERY configurado por el usuario en la variable */
		EXEC sp_executesql @SQL_CALCULO_VALOR, N'@p_resul numeric(22,7) out', @p_MontoValorBase out		
		print '@p_MontoValorBase: ' + convert(varchar, @p_MontoValorBase) 		
    END   
        
    -- Analizamos el nombre de la vista para VT_GrupoInstrumento, y VT_GrupoTipoRenta 
    SELECT TOP 1 
           @nvcNombreVista = nombrevista, 
		   @columna = nombrecolumna,
           @NombreTabla = NombreTabla,
           @SQL_SELECT_CARAC = SQL_SELECT,
           @SQL_SELECT_SOLO_DPZ = SQL_SELECT_SOLO_DPZ,
           @SQL_FROM_CARAC = SQL_FROM,
           @SQL_WHERE_CARAC = SQL_WHERE,
           @ES_AGRUPADOR = ISNULL(ES_AGRUPADOR,'N')
    FROM   CaracteristicaGrupo 
    WHERE  codigocaracteristica = @p_CodigoCaracteristica
    	
    -- Select 
    IF @nvcNombreVista IN ('VT_GrupoInstrumento'
							,'VT_GrupoTipoRenta'
							,'VT_CategoriaInstrumento')						
    BEGIN 
		SELECT TOP 1 @claseNormativa = clasenormativa 
		FROM   caracteristicagruponivel 
		WHERE  codigogrupoinstrumento = @CodigoGrupoInstrumento 
		AND    codigocaracteristica = @p_CodigoCaracteristica 
		AND    clasenormativa = @p_ValorEntidad 
    END

	DECLARE @cont NUMERIC(20,7)
	
    SELECT @p_Porcentaje = Isnull(Max(Isnull(ValorPorcentaje,0)),0), 
           @p_PorcentajeM = Isnull(Max(Isnull(ValorPorcentajeM,0)),0),
           @cont = Count(CodigoNivelLimite) 
    FROM   NivelLimite 
    WHERE  codigonivellimite = @p_CodigoNivelLimite 
    AND    codigolimitecaracteristica = @p_CodigoLimiteCaracteristica 
    AND    situacion = 'A' 
           
    IF @p_Porcentaje = 0
    BEGIN
		DECLARE @FloatOficioMultiple NUMERIC(22,7) = dbo.Obtenerfactoremisionlimitev2(@p_ValorEntidad,@p_CodigoLimite,@p_fecha)
		SET @p_Porcentaje = @p_ValorPorcentajeMarketShare * @FloatOficioMultiple/100 
	END
        	
	DECLARE @p_FlagTipoPorc VARCHAR(2) = ISNULL((SELECT flagtipoporcentaje 
											   FROM   NivelLimite 
											   WHERE  codigonivellimite = @p_CodigoNivelLimite AND situacion = 'A'
											   AND    codigolimitecaracteristica = @p_CodigoLimiteCaracteristica), '')
              
	IF @p_FlagTipoPorc = 'A' 
    BEGIN 
		SELECT	@p_Porcentaje = Max(valorporcentaje) , 
				@p_PorcentajeM = Isnull(Max(valorporcentajem),0),
				@cont = Count(CodigoNivelLimite)
		FROM   DetalleNivelLimite1 
		WHERE  codigonivellimite = @p_CodigoNivelLimite 
		AND    codigocaracteristica = @p_CodigoCaracteristica 
		AND    clasenormativa = @claseNormativa

		IF Isnull(@p_Porcentaje,0) = 0 AND Charindex(' AND ',@p_Filtro,9) >= 9 
		BEGIN 
			SET @CMDSQL = N'SELECT @claseNormativa = CodigoTipoInstrumento from VT_Instrumentos where ' + Substring(@p_Filtro,9,Charindex(' AND ',@p_Filtro,9)-9)
			EXEC sp_executesql @CMDSQL, N'@claseNormativa varchar(4) out', @claseNormativa out 

			SET @p_Porcentaje = isnull((SELECT valorporcentaje FROM detallenivellimite1 
										WHERE  codigonivellimite = @p_CodigoNivelLimite AND clasenormativa = @claseNormativa),0)
		END 
    END 

    IF @p_FlagTipoPorc = 'D' /*Para Caracteristicas de Nivel con "Porcentaje por Detalle de Agrupaci¾n" */
    BEGIN 
		PRINT '@p_FlagTipoPorc: '+ @p_FlagTipoPorc
		PRINT '@AplicarCastigoRating: ' + @AplicarCastigoRating

		IF @AplicarCastigoRating = 'S'
			SET @p_ValorEntidad = dbo.fn_AplicaCastigoRating(@p_ValorEntidad, @NivelesCastigo)
    
		--SELECT @p_Porcentaje = Max(valorporcentaje), 
		--		 @p_PorcentajeM = Max(ValorPorcentajeM),
		--		 @cont = Count(CodigoNivelLimite),
		--		 @TieneValorEspecifico =  TieneValorEspecifico
		SELECT   @p_Porcentaje = valorporcentaje, 
				 @p_PorcentajeM = ValorPorcentajeM,
				 @cont = CodigoNivelLimite,
				 @TieneValorEspecifico =  ISNULL(TieneValorEspecifico,'N'),
				 --OT12012
				 @CodigoCaracteristicaRelacion = CodigoCaracteristicaRelacion,
				 @CodigoRelacion = CodigoRelacion
				 --
		FROM   detallenivellimite2 
		WHERE  codigonivellimite = @p_CodigoNivelLimite 
		AND    codigocaracteristica = @p_CodigoCaracteristica 
		AND    valorcaracteristica = @p_ValorEntidad    

		
		PRINT '@p_ValorEntidad: ' + ISNULL(@p_ValorEntidad,'')
		PRINT '@p_CodigoCaracteristica: ' + @p_CodigoCaracteristica
		PRINT '@p_CodigoNivelLimite: ' + @p_CodigoNivelLimite
		PRINT '@p_Porcentaje: ' + CAST(@p_Porcentaje AS VARCHAR)
		PRINT '@cont: ' + CAST(@cont AS VARCHAR)

		--OT12012
		PRINT '@p_CodigoCaracteristica: ' + @p_CodigoCaracteristica
		PRINT '@@CodigoCaracteristicaRelacion: ' + @p_CodigoCaracteristica

		IF(@p_CodigoCaracteristica = '66' and @CodigoCaracteristicaRelacion = '65')
		BEGIN
				SELECT 
					@FlagSaldoBancos = FlagSaldo,
					@FlagSaldoInstrumento = FlagInstrumento
				FROM ClaseActivoInstrumentoLimite
				WHERE CodigoClaseActivoInstrumentoLimite = @p_ValorEntidad
		END
		
		
		SELECT 
				@TieneLimiteFijoEnDetalle = ISNULL(TieneLimiteFijoEnDetalle,'N'),
				@LimiteFijoEnDetalleMin = ISNULL(LimiteFijoEnDetalleMin,0),
				@LimiteFijoEnDetalleMax = ISNULL(LimiteFijoEnDetalleMax,0)
		FROM	NivelLimite 
		WHERE	CodigoNivelLimite = @p_CodigoNivelLimite 
		AND		CodigoCaracteristica = @p_CodigoCaracteristica

		IF @TieneLimiteFijoEnDetalle = 'S' AND @TieneValorEspecifico = 'N'
		BEGIN
			SET @p_Porcentaje = @LimiteFijoEnDetalleMax
			SET	@p_PorcentajeM = @LimiteFijoEnDetalleMin
		END		

		/*APLICAMOS LAS CONDICIONES ASIGNADAS AL LIMITE NIVEL*/
		DECLARE @PorcentajeMin_Condicion NUMERIC(22,7)
		DECLARE @PorcentajeMax_Condicion NUMERIC(22,7)

		EXEC Limite_ObtenerPorcentajePorReglaCondicion @p_CodigoNivelLimite, @p_ValorEntidad, @nvcNombreVista, @PorcentajeMin_Condicion OUT, @PorcentajeMax_Condicion OUT

		IF @PorcentajeMin_Condicion > 0 SET @p_PorcentajeM = @PorcentajeMin_Condicion
		IF @PorcentajeMax_Condicion > 0 SET @p_Porcentaje = @PorcentajeMax_Condicion

		PRINT '@PorcentajeMin_Condicion: ' + CAST(ISNULL(@PorcentajeMin_Condicion,0) AS VARCHAR)
		PRINT '@@PorcentajeMax_Condicion: ' + CAST(ISNULL(@PorcentajeMax_Condicion,0) AS VARCHAR)
		/************************/
    END 


    IF @p_Porcentaje IS NULL AND @cont > 0
		SET @p_Porcentaje = -1 
    
    IF @p_Porcentaje IS NULL 
		RETURN 0
    
	PRINT '@p_EsUltimoNivel: ' + CAST(@p_EsUltimoNivel AS VARCHAR)    
    IF (@p_EsUltimoNivel = 0) 
    BEGIN
		IF (@p_Porcentaje <= 0) 
		BEGIN 
			SET @p_Valor = 0 
			RETURN 0 
		END 
		        
		SET @vIndOpcion = 3 
    END   

    /*Condici¾n para @p_FiltroGrupo si es vacÝo*/ 
    IF Len(Isnull(@p_FiltroGrupo,'')) = 0 OR @UnidadPosicion IN ('N3D', 'MSC')
		SET @Filtro = @p_Filtro 
    ELSE     
		SET @Filtro = '(' + @p_Filtro + ')' 
        
    /*CRumiche: Posible codigo a retirar si se configura ADECUADAMENTE la tabla CaracteristicaGrupo */            
    -- SET @Filtro = Replace(@Filtro, 'CodigoGrupoEconomico', 'ISNULL(CodigoGrupoEconomico,''SG'')')        

    /*CRumiche: Posible codigo a retirar si se configura ADECUADAMENTE la tabla CaracteristicaGrupo */  
    SET @Filtro = Replace(@Filtro, 'ce ', 'e.CodigoEntidad ')
    
    SET @Filtro =	CASE WHEN @UnidadPosicion IN ('NM','ND') THEN Replace(@Filtro, 'cn = ', 'oi.CodigoMnemonico = ') 
					WHEN @UnidadPosicion IN ('N3D') THEN Replace(@Filtro, 'cn = ', 'Codigonemonico = ') 
					ELSE Replace(@Filtro, 'cn = ', 'ctv.CodigoMnemonico = ') END 

	----SET @Filtro = Replace(@Filtro, 'GrupoTipoInstrumento', 'gti.GrupoInstrumento') 
	----SET @Filtro = Replace(@Filtro, 'GrupoTipoRenta = ', 'v.GrupoTipoRenta = ') 
	----SET @Filtro = Replace(@Filtro, 'CodigoTipoInstrumento', 'TI.CodigoTipoInstrumento')
	
	PRINT '@Filtro: ' + CAST(ISNULL(@Filtro,'') AS VARCHAR)    
	PRINT '@UnidadPosicion: ' + CAST(ISNULL(@UnidadPosicion,'') AS VARCHAR)    
    /*Inicio de cßlculo de posiciones seg·n LÝmite*/ 
    IF @UnidadPosicion IN ('NM','ND') -- MONTO Negociacion Mensual y MONTO Negociacion diaria 
    BEGIN 
		DECLARE @fechaInicioMes NUMERIC(8) = @p_fecha /* Por Default para ND */
		
		IF @UnidadPosicion = 'NM' /*Restamos 1 MES */
			SET @fechaInicioMes = CONVERT(VARCHAR(8), Dateadd(month, -1, CONVERT(DATETIME, dbo.Convertirfecha(@p_fecha), 103)), 112)

		SET @CMDSQL = N' select  @p_valor = sum(r.monto - r.mexcep)   
					From (select  (oi.montooperacion * case when oi.CodigoMoneda<>''NSOL'' then dbo.RetornarTipoCambio((case when oi.CodigoMNemonico = ''MPLE LN'' then oi.CodigoMonedaDestino else oi.CodigoMoneda end),' + Cast(@p_fecha AS VARCHAR(8)) 
					+ ')      else 1     end) as monto, ISNULL(((select CantidadExcepcion from ExcepcionesLimiteNegociacion where CodigoOrden = oi.CodigoOrden) * oi.Precio *     case when oi.CodigoMoneda<>''NSOL'' 
					then dbo.RetornarTipoCambio((case when oi.CodigoMNemonico = ''MPLE LN'' then oi.CodigoMonedaDestino else oi.CodigoMoneda end),' + Cast(@p_fecha AS VARCHAR(8)) 
					+ ')      else 1     end), 0) as mexcep      from ordeninversion oi      inner join valores v on v.codigonemonico = oi.codigomnemonico      
					inner join Tipoinstrumento ti on v.codigotipoinstrumentosbs = ti.codigotipoinstrumentosbs      
					left join GrupoPorTipoInstrumento gti ON ti.CodigoTipoInstrumento = gti.CodigoTipoInstrumento and gti.situacion = ''A''      
					inner join criterioliquidezacciones cl on cl.codigomnemonico = v.codigonemonico      where Estado <> ''E-ELI'' and codigooperacion IN (1,2) and oi.FechaOperacion between '+ Cast(@fechaInicioMes AS VARCHAR(8)) 
					+ ' and ' + Cast(@p_fecha AS VARCHAR(8)) + ' and ' + @Filtro + ') r '
    END 
    ELSE 
		IF @UnidadPosicion ='N3D' /*MONTO Negociaci¾n a 3 DIAS*/
		BEGIN 
			SET @CMDSQL = N' select  @p_valor = round(ISNULL((select  sum(r.cantidad - r.canexcep) from (select  oi.CantidadOperacion as cantidad, 
			     ISNULL((select CantidadExcepcion from ExcepcionesLimiteNegociacion where CodigoOrden = oi.CodigoOrden), 0) as canexcep    
			     from ordeninversion oi    inner join valores v on v.codigonemonico = oi.codigomnemonico    
			     inner join Tipoinstrumento ti on v.codigotipoinstrumentosbs = ti.codigotipoinstrumentosbs    
			     inner join criterioliquidezacciones cl on cl.codigomnemonico = v.codigonemonico    where Estado <> ''E-ELI''    
			     and codigooperacion IN (1,2)    and oi.FechaOperacion=' + Cast(@p_fecha AS VARCHAR(8)) + ' and ' + @Filtro + ') r) / (select CantidadOperacion - ISNULL((select sum(CantidadExcepcion) from ExcepcionesLimiteNegociacion el      
			     inner join       (select CodigoOrden, ''Codigonemonico'' = CodigoMnemonico from ordeninversion       
			     where Estado <> ''E-ELI''     and codigooperacion IN (1,2)     and CodigoMnemonico = ob.CodigoNemonico     
			     and FechaOperacion=' + Cast(@p_fecha AS VARCHAR(8)) + ') oi on el.CodigoOrden = oi.CodigoOrden      
			     where ' + @Filtro + '), 0)     from operacionbvl ob     where fechaoperacion=' + Cast(@p_fecha AS VARCHAR(8)) + ' and ' + @Filtro + ')    * 100, 0),2) ' 
		END 
		ELSE 
		BEGIN 
			--Un flujo normal entra por aqui 
			--Para la caracteristica 32 que es por origen de moneda Y 34 que es por derivados 
			DECLARE @CodMonedaP VARCHAR(10), 
				@MonedaPortafolio VARCHAR(1), 
				@Forward          CHAR(1), 
				@Swap             CHAR(1), 
				@Local            CHAR(1), 
				@DPZ              CHAR(1), 
				@NominalRecibir   CHAR(1), 
				@Gobierno         CHAR(1) 
			
			SELECT @CodMonedaP = codigomoneda 
			FROM   portafolio 
			WHERE  codigoportafoliosbs = @p_CodigoPortafolioSBS
      
			IF @p_CodigoCaracteristica = '03' 
			BEGIN 
				SELECT @Gobierno = gobierno 
				FROM   grupoportipoinstrumento 
				WHERE  grupoinstrumento = @p_ValorEntidad 
			END
      
			IF @p_CodigoCaracteristica = '32' 
			BEGIN
				SELECT @MonedaPortafolio = monedaportafolio , 
					   @Forward =          forward 
				FROM   grupoportipomoneda 
				WHERE  codigotipomoneda = @p_ValorEntidad 
				--Si el campo es S, se toman todas la negociaciones que sean de la moneda del portafolio, 
				--caso contrario todas las que no sean del portafolio 
				SET @Filtro = 'V.CodigoMoneda ' + case when @MonedaPortafolio = 'S' then '=' else '<>' end + ' ''' + @CodMonedaP + '''' 					
			END
						
      
			IF @p_CodigoCaracteristica = '34' 
			BEGIN 
				SELECT @MonedaPortafolio = monedaportafolio , 
					   @Forward =          forward, 
					   @Swap =             @Swap, 
					   @NominalRecibir =   nominalrecibir 
				FROM   grupoporderivados 
				WHERE  codigogrupoderivado = @p_ValorEntidad 
			END
      
			IF @p_CodigoCaracteristica = '35' 
			BEGIN 
				SELECT @Local = local, @DPZ =   dpz 
				FROM   grupoporcalificacion 
				WHERE  [CodigoGrupoClasificacion] = @p_ValorEntidad 
			END
      
			--Codigo de mercado solo es usado cuando el limites es por Origen de Mercado, @p_CodigoCaracteristica 33
			DECLARE @CodigoMercado VARCHAR(1) 

			SET @CodigoMercado = CASE WHEN Charindex('''1''',@Filtro) <> 0 THEN  '1' ELSE '2' END 
      
			IF @p_CodigoCaracteristica IN ('38','34', '40')
			BEGIN
    			IF @p_CodigoCaracteristica = '38' 
					SET @CMDSQL = N' SELECT @p_Valor =  dbo.Valorizacion_ClaseInstrumento('''+ @p_CodigoPortafolioSBS +''','''+ @p_ValorEntidad +''','+ CONVERT(NVARCHAR(8), @p_fecha) +','''+ @p_Escenario +''')'

				IF @p_CodigoCaracteristica = '34' 
				BEGIN 
					IF @Forward = 'S' 
						IF @NominalRecibir = 'S' 
							SET @CMDSQL = N'SELECT @p_Valor = dbo.NominalRecibirFD_Moneda('''+ @p_CodigoPortafolioSBS +''','''+ @CodMonedaP +''','''+ @MonedaPortafolio +''','+ CONVERT(NVARCHAR(8), @p_fecha) + ')'
						ELSE 
							SET @CMDSQL = N'SELECT @p_Valor = dbo.Valorizacion_Forward('''+ @p_CodigoPortafolioSBS +''','+ CONVERT(NVARCHAR(8), @p_fecha) + ')'
					ELSE 
						IF @Swap = 'S' 
							SET @CMDSQL = N'SELECT @p_Valor = 0 ' 
				END

				IF @p_CodigoCaracteristica = '40' 		 
					SET @CMDSQL = N'SELECT @p_Valor = dbo.FN_SIT_OBT_CalcularDuracion('''+ @p_CodigoPortafolioSBS +''','+ CONVERT(NVARCHAR(8), @p_fecha) +','''+ @p_Escenario +''') '					 			     
			END
			--OT12012
			IF(@p_CodigoCaracteristica = '66' and @FlagSaldoBancos = '1')
			BEGIN
				IF(@p_ValorEntidad = '1')
				BEGIN
					--Calcular Efectivo
					SET @CMDSQL = N'SELECT @p_Valor = dbo.Limite_SaldoBancos_ClaseActivo('''+ @p_CodigoPortafolioSBS +''','+ CONVERT(NVARCHAR(8), @p_fecha) +',''20'')'
				END
				IF(@p_ValorEntidad = '2')
				BEGIN
					--Calcular Depositos
					SET @CMDSQL = N'SELECT @p_Valor = ISNULL(round(sum(ISNULL(ct.VPNMonedaFondo,0)),2),0) '	
									+ ' FROM CarteraTituloValoracion CT '
									+ ' INNER JOIN Valores V ON CT.CodigoMnemonico = v.CodigoNemonico '
									+ ' WHERE ct.Escenario = ''{ESCENARIO}'' '
									+ ' AND ct.FechaValoracion = {FECHA_PROCESO} '
									+ ' AND ct.CodigoPortafolioSBS = ''{CODIGO_PORTAFOLIO}'' '	
									+ ' AND v.CodigoTipoInstrumentoSBS = ''60'' '

				SET @CMDSQL = REPLACE(@CMDSQL, '{FECHA_PROCESO}', @p_fecha)
				SET @CMDSQL = REPLACE(@CMDSQL, '{ESCENARIO}', @p_Escenario)
				SET @CMDSQL = REPLACE(@CMDSQL, '{CODIGO_PORTAFOLIO}', @p_CodigoPortafolioSBS)

				END
			END
			ELSE -- Caracteristica ESTANDAR
			BEGIN 
			
				set @v_texto_case = case when @UnidadPosicion = 'U' OR @ValorBase = 'PACON' then 'Cantidad' else 'VPNMonedaFondo' end 				
									
				IF ISNULL(@SQL_SELECT_CARAC,'') = ''
					SET @SQL_SELECT_CARAC = N'SELECT @p_Valor = ISNULL(round(sum(ISNULL(ctv.' + @v_texto_case + ',0)),2),0) '					
						
				/* Ahora estructuramos el FROM (STANDAR)*/
				SET @SQL_FROM = ' FROM CarteraTituloValoracion ctv '
								+ ' INNER JOIN Valores V on V.CodigoNemonico = ctv.CodigoMnemonico '
									+ ' AND ctv.FechaValoracion = {FECHA_PROCESO} '
									+ ' AND ctv.Escenario = ''{ESCENARIO}'' '									
									+ ' AND ctv.CodigoPortafolioSBS = ''{CODIGO_PORTAFOLIO}'' '															
								+ ' INNER JOIN Entidad E ON E.Situacion = ''A'' AND E.CodigoEntidad = V.CodigoEmisor and E.EntidadEmisora = ''S'' '
								+ ' INNER JOIN TipoInstrumento TI on TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS ' 
								+ ' INNER JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento AND CI.Categoria  NOT IN (''OR'',''DP'',''FD'') ' 							
								-- + ' INNER JOIN OrdenInversion OI ON OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.Estado IN (''E-CON'',''E-EJE'') '							

				/* CRumiche: Ahora estructuramos el WHERE (STANDAR)*/
				DECLARE @SQL_WHERE VARCHAR(8000) = ' WHERE ctv.Situacion = ''A'' '	/* Comodin para no dejar WHERE VACIO*/										
				
				/* CRumiche: Excluimos DPZ Y ORD. DE REPORTE de no ser requerido */
				DECLARE @SQL_DPZ VARCHAR(8000) = ''
				
				IF ISNULL(@AplicaDPZ,'S') = 'S' /* CRumiche: Por defecto si es VACIO asumimos que SI APLICA */
				BEGIN
					/* CRumiche: Primero se definirß en el SELECT BASE */													
					IF ISNULL(@SQL_SELECT_SOLO_DPZ,'') <> '' /* SELECT exclusivo para DPZ */
						SET @SQL_DPZ = @SQL_SELECT_SOLO_DPZ
					ELSE IF ISNULL(@SQL_SELECT_CARAC,'') <> '' /* SELECT tanto para la query standar como para DPZ, Reemplazamos la cluasula SELECT por  SELECT @p_Valor = @p_Valor */
						SET @SQL_DPZ = REPLACE(@SQL_SELECT_CARAC, 'SELECT @p_Valor =', ' SELECT @p_Valor = @p_Valor + ')
					ELSE /* Por defecto */												
						SET @SQL_DPZ = ' SELECT @p_Valor = @p_Valor + ISNULL(round(sum(ISNULL(ctv.' + @v_texto_case + ',0)),2),0) '									
					
					/* CRumiche: Ahora definimos el FROM BASE */				
					SET @SQL_DPZ =  @SQL_DPZ + ' FROM CarteraTituloValoracion ctv '
					+ ' INNER JOIN OrdenInversion OI ON OI.CodigoSBS = CTV.CodigoSBS AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS AND OI.CodigoOperacion IN (''3'',''101'') AND OI.Estado IN (''E-CON'',''E-EJE'') '
						+ ' AND ctv.FechaValoracion = {FECHA_PROCESO} '
						+ ' AND ctv.Escenario = ''{ESCENARIO}'' '									
						+ ' AND ctv.CodigoPortafolioSBS = ''{CODIGO_PORTAFOLIO}'' '	
						+ ' AND ({FECHA_PROCESO} BETWEEN OI.FechaOperacion AND OI.FechaContrato) '													
					+ ' INNER JOIN Entidad E ON E.Situacion = ''A'' AND E.CodigoTercero = OI.CodigoTercero '
					+ ' INNER JOIN Valores V on V.CodigoNemonico = ctv.CodigoMnemonico '
					+ ' INNER JOIN TipoInstrumento TI on TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS ' 
					+ ' INNER JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento '						
				END	
				
								
				print '@SQL_DPZ: ' + convert(varchar, @SQL_DPZ) 				
				-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-30
					/* CRumiche: Parte DINAMICA de los Limites en la que se complementa el QUERY principal (FROM y WHERE)
					esto se obtiene de la tabla CaracteristicaGrupo, de las columnas SQL_FROM, etc.  */

				/* CRumiche: Complementamos el QUERY principal con restricciones configuradas en la CARACTERISTICA */

				IF ISNULL(@SQL_FROM_CARAC,'') <> '' 
				BEGIN
					SET @SQL_FROM = @SQL_FROM + ' ' + @SQL_FROM_CARAC
					SET @SQL_DPZ = @SQL_DPZ + ' ' + @SQL_FROM_CARAC	
				END
							
				IF ISNULL(@SQL_WHERE_CARAC,'') <> '' SET @SQL_WHERE = @SQL_WHERE + ' ' + @SQL_WHERE_CARAC
				
				/* CRumiche: Finalmente concatenamos QUERYS antes de la ejecuci¾n Y agrega el FILTRO heredado */
				SET @CMDSQL = @SQL_SELECT_CARAC + @SQL_FROM + @SQL_WHERE + ' /* Filtro del Nivel Padre */ AND ' + ISNULL(@Filtro,'1=1')
				IF ISNULL(@SQL_DPZ,'') <> '' SET @CMDSQL = @CMDSQL + @SQL_DPZ + @SQL_WHERE + ' /* Filtro del Nivel Padre */ AND ' + ISNULL(@Filtro,'1=1')					
										
				/* CRumiche: De requerirse FORWARD se debe aplicar QUERY adicional */				
				IF ISNULL(@AplicaForward,'N') = 'S'
				BEGIN
					DECLARE @SQL_FROM_FORWARD VARCHAR(2500)
					DECLARE @SQL_WHERE_FORWARD VARCHAR(2500)
					
					SET @SQL_FROM_FORWARD = N' SELECT @p_Valor = @p_Valor + ISNULL(round(sum(ISNULL(VF.MtmUSD ,0)),2),0) '
						+ ' FROM VectorForwardSBS VF '
						+ ' INNER JOIN OrdenInversion OI ON OI.CodigoISIN = VF.NumeroPoliza AND OI.CodigoPortafolioSBS = ''{CODIGO_PORTAFOLIO}'' AND OI.Estado IN (''E-CON'',''E-EJE'') '
						+ ' INNER JOIN Entidad e ON e.CodigoTercero = OI.CodigoTercero  and e.Situacion = ''A'' '
					
					IF ISNULL(@SQL_FROM_CARAC,'') <> '' SET @SQL_FROM_FORWARD = @SQL_FROM_FORWARD + ' ' + @SQL_FROM_CARAC
				
					/* CRumiche: Adicionamos el WHERE directamente*/
					SET @SQL_FROM_FORWARD = @SQL_FROM_FORWARD + ' WHERE VF.Fecha = {FECHA_PROCESO} ' + ' /* Filtro del Nivel Padre */ AND ' + ISNULL(@Filtro,'1=1')
					
					/* CRumiche: unimos al QUERY principal*/														
					SET @CMDSQL = @CMDSQL +  @SQL_FROM_FORWARD																					
				END
				
				/* CRumiche: @ES_AGRUPADOR podrÝa permitir la utilizaci¾n del Comodin {IN_NOT_IN}
							los cuales podrÝan configurarse en @SQL_FROM_CARAC o @SQL_WHERE_CARAC */
				DECLARE @p_IN_NOT_IN VARCHAR(10) = ' IN ', @p_SQL_FINAL_CALCULO varchar(2048)
				
				SELECT 
					@p_IN_NOT_IN = case when ES_GRUPO_EXCEPCION = 'S' then ' NOT IN ' else ' IN ' end
					,@p_SQL_FINAL_CALCULO = SQL_FINAL_CALCULO
				FROM GrupoCaracteristicaCabecera 
				WHERE TablaGrupo = @NombreTabla AND CodigoGrupo = @p_ValorEntidad
				
				/* CRumiche: Aplicamos defaults en caso de NULL*/
				SELECT @p_IN_NOT_IN = ISNULL(@p_IN_NOT_IN, ' IN '), @p_SQL_FINAL_CALCULO = ISNULL(@p_SQL_FINAL_CALCULO, '')


				--OT12012
				IF(@FlagSaldoInstrumento = '1')
				BEGIN
					SELECT @p_SQL_FINAL_CALCULO = @p_SQL_FINAL_CALCULO + ' ' + SQL_FINAL_CALCULO
					FROM LimiteFuncionesPosicion WHERE Situacion = 'A' 
				END
				ELSE
				BEGIN
					/* CRumiche: Soporte para Aplicar Funciones Complemento a las Posiciones en todos los LÝmites (Ejemplo: Funcion Saldo Banco)*/
					SELECT @p_SQL_FINAL_CALCULO = @p_SQL_FINAL_CALCULO + ' ' + SQL_FINAL_CALCULO
					FROM LimiteFuncionesPosicion WHERE Situacion = 'A' AND ES_PARA_AGRUPADORES = @ES_AGRUPADOR /* Hay funciones para Caracteristicas y otras para Grupos de Caracteristicas */
				END
				
				SET @CMDSQL = @CMDSQL + ' ' + @p_SQL_FINAL_CALCULO 
				--IF ISNULL(@ES_AGRUPADOR,'N') = 'S' BEGIN 
				--	print 'La Caracteristica es de tipo AGRUPADOR DE CARACTERISTICAS'																		
				--	/* CRumiche: Verificamos si el AGRUPADOR esta marcado como EXCEPCION */
				--	IF EXISTS(SELECT TOP 1 1 FROM GrupoCaracteristicaCabecera 
				--				WHERE TablaGrupo = @NombreTabla AND CodigoGrupo = @p_ValorEntidad AND ES_GRUPO_EXCEPCION = 'S')
				--		SET @p_IN_NOT_IN = ' NOT IN '				
				--END

				SET @CMDSQL = REPLACE(@CMDSQL, '{VALOR_CARACTERISTICA}', @p_ValorEntidad)
				SET @CMDSQL = REPLACE(@CMDSQL, '{IN_NOT_IN}', @p_IN_NOT_IN)
				SET @CMDSQL = REPLACE(@CMDSQL, '{FECHA_PROCESO}', @p_fecha)
				SET @CMDSQL = REPLACE(@CMDSQL, '{ESCENARIO}', @p_Escenario)
				SET @CMDSQL = REPLACE(@CMDSQL, '{CODIGO_PORTAFOLIO}', @p_CodigoPortafolioSBS)
				SET @CMDSQL = REPLACE(@CMDSQL, '{CODIGO_MONEDA_FONDO}', @CodigoMonedaPortafolio)
				SET @CMDSQL = REPLACE(@CMDSQL, '{FLAG_SALDO_BANCOS}', @AplicaSaldoBanco) -- CRumiche: Soporte para Saldo Bancos
				SET @CMDSQL = REPLACE(@CMDSQL, '{NOMBRE_VISTA}', @nvcNombreVista)

				PRINT '### QUERY: Suma de Instrumentos en Forma Dinßmica: ' + @CMDSQL
				
				/* Solo para cummplir con el INSERT q estß en lineas mßs ABAJO */
				SET @SQL_FROM = REPLACE(@SQL_FROM, '{VALOR_CARACTERISTICA}', @p_ValorEntidad)
				SET @SQL_FROM = REPLACE(@SQL_FROM, '{IN_NOT_IN}', @p_IN_NOT_IN)
				SET @SQL_FROM = REPLACE(@SQL_FROM, '{FECHA_PROCESO}', @p_fecha)
				SET @SQL_FROM = REPLACE(@SQL_FROM, '{ESCENARIO}', @p_Escenario)
				SET @SQL_FROM = REPLACE(@SQL_FROM, '{CODIGO_PORTAFOLIO}', @p_CodigoPortafolioSBS)
				SET @SQL_FROM = REPLACE(@SQL_FROM, '{CODIGO_MONEDA_FONDO}', @CodigoMonedaPortafolio)
			END			
		END	
	
		IF @vIndOpcion = 3 
			SET @p_Valor = 1.0 
		ELSE 
		BEGIN
			PRINT '### QUERY: Ejecuci¾n A: ' + @CMDSQL
			EXEC sp_executesql @CMDSQL, N'@p_Valor numeric(22,7) out', @p_Valor out 
			IF (Charindex(@p_CodigoMnemonico, @p_Filtro, 1) > 0 AND @p_procesar = 2 AND Isnull(@p_Valor,0) = 0) 
				SET @p_Valor = 1.0 
		END 

    IF @UnidadPosicion = 'N3D' 
    BEGIN 
      SET @p_CodigoPortafolioSBS = 'x' 
    END 
    --Para el caso de MULTIFONDO, se necesita obtener los montos de los 3 fondos independientemente 
    IF @p_CodigoPortafolioSBS = @p_CodMultifondo 
    AND 
    Isnull(@p_Valor,0) <> 0 
    AND 
    @vIndOpcion <> 3 
    BEGIN 
      DECLARE @CodFondo VARCHAR(5), 
        @Valor          NUMERIC(22,7) 
      DECLARE curfondo CURSOR FOR 
      SELECT codigoportafoliohijo 
      FROM   multiportafolio 
      WHERE  codigoportafoliopadre = @p_CodMultifondo 
      OPEN curfondo 
      FETCH next 
      FROM  curfondo 
      INTO  @CodFondo 
      WHILE @@FETCH_STATUS = 0 
      BEGIN 
        DECLARE @CodigoPortafolioSBS VARCHAR(30) 
        SET @CodigoPortafolioSBS = 
        CASE 
        WHEN @UnidadPosicion IN ('NM', 
                                 'ND', 
                                 'CRE') THEN 
          ' oi.CodigoPortafolioSBS ' 
          ELSE ' ctv.CodigoPortafolioSBS ' 
        END 
        SET @CMDSQLF = Replace(@CMDSQL,'@p_Valor = ','@Valor = ') 
        SET @CMDSQLF = @CMDSQLF + ' and '+ @CodigoPortafolioSBS + ' = ''' + @CodFondo + ''' ' 

PRINT '## Query Suma de Instrumentos 2' 
PRINT @CMDSQLF

        EXEC sp_executesql 
          @CMDSQLF, 
          N'@Valor numeric(22,7) out', 
          @Valor out 
        UPDATE #posicion_fondos 
        SET    posicionportafolio = @Valor 
        WHERE  codigoportafolio = @CodFondo 
        FETCH next 
        FROM  curfondo 
        INTO  @CodFondo 
      END 
      CLOSE curfondo 
      DEALLOCATE curfondo 
    END 
    IF (@p_EsUltimoNivel = 1  OR  @p_Porcentaje > 0)  AND Isnull(@p_Valor,0) > 0 
    BEGIN 			
		--Solo guardamos el detalle de los q tengan posicion mayor q cero 
		SET @CodigoReporte = Isnull((SELECT Max(codigoreporte) FROM   reportelimites),0) 
		SET @CodigoReporte = @CodigoReporte + ( SELECT Count(1) FROM #reporte) + 1
		
		SET @SQL_FROM = 'insert into LimiteValores select distinct ' + Cast(@CodigoReporte AS VARCHAR(18)) + ', ctv.CodigoMnemonico ' + @SQL_FROM

		PRINT '## Query Suma de Instrumentos 3: ' + @SQL_FROM
		EXEC sp_executesql @SQL_FROM 
		
      IF EXISTS 
      ( 
             SELECT 1 
             FROM   limitevalores 
             WHERE  codigoreporte = @CodigoReporte 
             AND    codigonemonico LIKE 'DPZ%') 
      INSERT INTO limitevalores 
                  ( 
                              codigoreporte, 
                              codigonemonico 
                  ) 
      SELECT @CodigoReporte, 
             codigotipotitulo 
      FROM   tipotitulo 
      WHERE  codigotipoinstrumentosbs = '60' 
      AND    situacion = 'A' 
      AND    codigotipotitulo NOT IN 
             ( 
                    SELECT codigonemonico 
                    FROM   limitevalores 
                    WHERE  codigoreporte = @CodigoReporte) 
    END 
END
GO

GRANT EXECUTE ON [Posicion_Calcular_Reporte_NEW] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_ObtenerSaldo_NemonicoPortafolioFecha]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObtenerSaldo_NemonicoPortafolioFecha') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObtenerSaldo_NemonicoPortafolioFecha]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Obtener saldo de la cartera por codigo de portafolio 
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ObtenerSaldo_NemonicoPortafolioFecha 
(
   @CodigoNemonico varchar(15),          
   @FechaIDI numeric(8,0),
   @CodigoPortafolio varchar(20)
)
AS
BEGIN
	
	DECLARE @decimal as decimal(22,2) = 0
	
	select 
		@decimal = isnull(isnull(SALDODISPONIBLE,0) + isnull(SALDOPROCESOCOMPRA,0) - isnull(SALDOPROCESOVENTA,0) + isnull(SALDOUNIDADESLIBERADAS,0),0) 
	from SaldosCarteraTitulo 
	where CodigoMnemonico = @CodigoNemonico
	and CODIGOPORTAFOLIOSBS = @CodigoPortafolio
	and FECHASALDO = @FechaIDI

	select @decimal as saldo

END


GRANT EXECUTE ON [sp_ObtenerSaldo_NemonicoPortafolioFecha] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[sp_ReporteDiarioOperaciones]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ReporteDiarioOperaciones') BEGIN 
	DROP PROCEDURE [dbo].[sp_ReporteDiarioOperaciones]
END 
GO
---------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Modificar el reporte para agregar la consulta de dividendos
---------------------------------------------------------------------------------------------------------
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
	ELSE IF @P_TipoConsulta = '5'
		--Dividendos

				SELECT
			Tabla.Portafolio,
			Tabla.FechaSaldo,
			Tabla.CodigoCustodio,
			Tabla.CodigoNemonico,
			Tabla.CodigoSBS,
			Tabla.CodigoISIN,
			oi.CantidadOperacion,
			Tabla.Factor,
			(isnull(oi.CantidadOperacion,0) * isnull(Tabla.Factor,0)) Monto,
			CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),OI.FechaLiquidacion)), 103) FechaLiquidacion
		FROM	
		(
			SELECT	
				p.Descripcion Portafolio, 
				CONVERT(VARCHAR(10),CONVERT(DATE,CONVERT(CHAR(8),d.FechaIDI)), 103) FechaSaldo, 
				cv.CodigoCustodio, 
				d.CodigoNemonico,
				d.CodigoSBS,
				d.CodigoISIN,
				--isnull(NumeroUnidades,0) Unidades, 
				isnull(Factor,0) Factor, 
				--(isnull(NumeroUnidades,0)*isnull(Factor,0)) Monto,
				d.TipoDistribucion + '' + cast(D.Identificador as varchar(10)) Codigo
			FROM DIVIDENDOSREBATESLIBERADAS D 
			INNER JOIN Portafolio P on d.CodigoPortafolioSBS = p.CodigoPortafolioSBS 
			INNER JOIN CustodioValores CV on d.CodigoNemonico = cv.CodigoNemonico and d.CodigoPortafolioSBS = cv.CodigoPortafolioSBS
			where d.FechaIDI = @P_Fecha
			AND d.CodigoPortafolioSBS = CASE WHEN @P_CodigoPortafolio = '' THEN d.CodigoPortafolioSBS ELSE @P_CodigoPortafolio END
		) as Tabla
		INNER JOIN OrdenInversion OI on Tabla.Codigo = oi.CodigoOrden
		WHERE OI.Estado = 'E-CON'

END
GO

GRANT EXECUTE ON [sp_ReporteDiarioOperaciones] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[sp_SIT_ImpresionCarta_Transferencias]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_ImpresionCarta_Transferencias') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_ImpresionCarta_Transferencias]
END 
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Lista la estructura para la generacion de cartas de Trasnferencias
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/12/2016
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9678
-----------------------------------------------------------------------------------------------------------
-- Fecha ModificaciÃ³n: 28/02/2017
-- Modificado por: Carlos Espejo
-- Nro. Orden de Trabajo: 10025
-- Descripcion del cambio: Se agrega el codigo del fimante 1 y 2
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Modificar el reporte de transferencia para los nuevos casos del proyecto mandatos
-------------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_ImpresionCarta_Transferencias] --'T10312'
(
	@p_CodigoOperacionCaja varchar(10)
)
AS
BEGIN
DECLARE @BancoPublico VarChar(100)
	SELECT @BancoPublico = T.Descripcion From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	WHERE Clasificacion = 'BANFONPUB'
	--
	SELECT 
	NombrePortafolio = P.Descripcion,
	CodigoNegocio = P.codigoNegocio,
	TipoNegocio = P.TipoNegocio,
	FechaDDMMYY = DBO.FN_SIT_OBT_FechaFormateada_DDMMYY(OC.FechaOperacion),
	FechaCarta = dbo.RetornarFechaCompleta(OC.FechaOperacion),
	NombreBanco = T.Descripcion,
	NombreCompletoPortafolio = P.NombreCompleto, 
	RUCPortafolio = ISNULL(P.RUCPortafolio,''),
	ValorOperacion = CONVERT(NUMERIC(22,2),OC.Importe), 
	TituloCarta = MC.Descripcion,
	Moneda = CASE WHEN M.CodigoMoneda = 'DOL' THEN 'USD' ELSE M.Descripcion END,
	NumeroCuentaCargo  = CASE WHEN OC.OperacionFictizia = 'S' THEN CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME '	END + OC.NumeroCuenta ELSE CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END + OC.NumeroCuentaDestino END,
	NumeroCuentaAbono  = CASE WHEN OC.OperacionFictizia = 'S' THEN CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME '	END + OC.NumeroCuentaDestino ELSE CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END + OC.NumeroCuenta END,
	BancoAbono = TD.Descripcion,
	BancoCargo = T.Descripcion,
	NumeroCuentaCargoCCI = 'CCI ' + CE1.NumeroCuentaInterBancario ,
	NumeroCuentaAbonoCCI  = CASE WHEN OC.OperacionFictizia = 'S' THEN 'CUENTA MATRIZ' ELSE 'CCI ' + CE2.NumeroCuentaInterBancario END, 
	TipoDocumentoBancoAbono = isnull(TD.CodigoTipoDocumento,''),
	RUCBancoAbono = isnull(TD.CodigoDocumento,''),
	SimboloMoneda =  M.Simbolo,
	Firma1 = ISNULL(AC1.Firma,''), 
	Firma2 = ISNULL(AC2.Firma,''),
	NombreUsuarioF1 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF1),''),
	NombreUsuarioF2 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF2),''),
	CargoUsuarioF1 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF1),'') ,
	CargoUsuarioF2 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF2),''),
	BancoAbonoCodigo =ISNULL(TD.CodigoDocumento,'Falta Parametria') , 
	BancoCargoCodigo = ISNULL(T.CodigoDocumento,'Falta Parametria'),
	BancoPublico = CASE WHEN P.CodigoFondosMutuos = '' THEN '' ELSE @BancoPublico END, 
	ObservacionCarta = CASE WHEN ISNULL(OC.ObservacionCarta,'') = '' THEN '' ELSE 'OBS: ' + OC.ObservacionCarta END,
	oc.VBGERF1 CodigoUsuarioF1 , oc.VBGERF2 CodigoUsuarioF2, --10025
	OperacionFictizia
	FROM OperacionesCaja OC
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
	LEFT JOIN Terceros TD ON TD.CodigoTercero = OC.CodigoTerceroDestino
	JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo  AND MC.Situacion = 'A'
	JOIN Moneda M ON M.CodigoMoneda = OC.CodigoMoneda
	JOIN CuentaEconomica CE1 ON CE1.NumeroCuenta = OC.NumeroCuenta AND CE1.Situacion = 'A'
	LEFT JOIN CuentaEconomica CE2 ON CE2.NumeroCuenta = OC.NumeroCuentaDestino  AND CE2.Situacion = 'A'
	LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = OC.VBGERF1
	LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = OC.VBGERF2
	WHERE OC.CodigoOperacionCaja = @p_CodigoOperacionCaja
END
GO

GRANT EXECUTE ON [sp_SIT_ImpresionCarta_Transferencias] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[CuentasPorCobrar_Liquidar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='CuentasPorCobrar_Liquidar') BEGIN 
	DROP PROCEDURE [dbo].[CuentasPorCobrar_Liquidar]
END 
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
--	Fecha Modificacion: 17/07/2019
--	Modificado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se agregó el campo "@p_CodigoRelacion" para crear una la relacion entre un reg. del tipo cierre y una apertura 
--							para el reporte de "Operación Reporte"
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
		@p_BancoGlosaDestino VARCHAR(20),
		@p_CodigoRelacion VARCHAR(20)
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
		CorrelativoCartas,BancoMatrizOrigen,BancoMatrizDestino,ObservacionCartaDestino,BancoGlosaDestino,CodigoOrden_Rel)--10150
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
		@p_BancoGlosaDestino,
		@p_CodigoRelacion
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

GRANT EXECUTE ON [CuentasPorCobrar_Liquidar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[Limite_ModificarCaracteristicasNiveles]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Limite_ModificarCaracteristicasNiveles') BEGIN 
	DROP PROCEDURE [dbo].[Limite_ModificarCaracteristicasNiveles]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregaron los campos de configuración de Limite Nivel y Limite Fijo en Detalle
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 02/08/2019
--	Modificado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se adapto para actualizar datos de  limites multinivel
---------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Limite_ModificarCaracteristicasNiveles] (
	@p_Secuencial varchar(5),
	@p_CodigoLimiteCaracteristica varchar(5),
    @p_CodigoNivelLimite varchar(5),
	@p_CodigoCaracteristica varchar(5),
	@p_FlagTipoPorcentaje varchar(1),
	@p_ValorPorcentaje numeric(12,2),
    @p_situacion varchar(1),
	@p_UsuarioModificacion varchar(15),
	@p_FechaModificacion numeric(8,0),
	@p_HoraModificacion varchar(10),
	@p_Host varchar(20),
	
	/*CAMBIOS GT 20181114*/
	@p_TieneLimiteNivel varchar(1),
	@p_LimiteNivelMin numeric(22,7),
	@p_LimiteNivelMax numeric(22,7),
	@p_TieneLimiteFijoEnDetalle varchar(1),
	@p_LimiteFijoEnDetalleMin numeric(22,7),
	@p_LimiteFijoEnDetalleMax numeric(22,7),

	@p_ValorPorcentajeM numeric(22,7) = 0,	--HDG OT 65023 20120522
	@p_CodigoRelacion varchar(5)
)
AS
BEGIN
	SET NOCOUNT ON

	--ini HDG OT 65023 20120522
	DECLARE @CodigoLimite VARCHAR(5),
		@Tope VARCHAR(6),
		@BandaLimites CHAR(1)

	SELECT @CodigoLimite = CodigoLimite
	FROM LimiteCaracteristica
	WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica

	SELECT @Tope = Tope
	FROM Limite
	WHERE CodigoLimite = @CodigoLimite

	if @Tope = 'BAN'
	begin
		set @BandaLimites = 'B'
	end
	else
	begin
		set @p_ValorPorcentajeM = 0
		set @BandaLimites = ''
	end
	--fin HDG OT 65023 20120522

	Update
		[NivelLimite]
	set 
	Secuencial = @p_Secuencial,
	CodigoCaracteristica = @p_CodigoCaracteristica,
	FlagTipoPorcentaje = @p_FlagTipoPorcentaje,
	ValorPorcentaje = @p_ValorPorcentaje,
	Situacion =@p_situacion,
	UsuarioModificacion =@p_UsuarioModificacion,
	FechaModificacion =@p_FechaModificacion,
	HoraModificacion =@p_HoraModificacion,
	Host=@p_Host,
	ValorPorcentajeM=@p_ValorPorcentajeM,	--HDG OT 65023 20120522
	BandaLimites=@BandaLimites,	--HDG OT 65023 20120522
		/*CAMBIOS GT 20181114*/
		TieneLimiteNivel			= @p_TieneLimiteNivel,
		LimiteNivelMin				= @p_LimiteNivelMin,
		LimiteNivelMax				= @p_LimiteNivelMax,
		TieneLimiteFijoEnDetalle	= @p_TieneLimiteFijoEnDetalle,
		LimiteFijoEnDetalleMin		= @p_LimiteFijoEnDetalleMin,
		LimiteFijoEnDetalleMax		= @p_LimiteFijoEnDetalleMax,
		CodigoRelacion = @p_CodigoRelacion 
	where 
	CodigoLimiteCaracteristica=@p_CodigoLimiteCaracteristica and
	CodigoNivelLimite=@p_CodigoNivelLimite

	select CodigoNivelLimite 
	from NivelLimite 
	where CodigoNivelLimite = @p_CodigoNivelLimite

END
GO

GRANT EXECUTE ON [dbo].[Limite_ModificarCaracteristicasNiveles] TO [rol_sit_fondos] AS [dbo]  
GO

PRINT '[Limite_SeleccionarCaracteristicasNiveles]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Limite_SeleccionarCaracteristicasNiveles') BEGIN 
	DROP PROCEDURE [dbo].[Limite_SeleccionarCaracteristicasNiveles]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregaron los campos de configuración de Limite Nivel y Limite Fijo en Detalle
--------------------------------------------------------------------------------------------------------------------------- 
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12012
--	Descripcion del cambio: Se agregaron el campo CodigoRelacion
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[Limite_SeleccionarCaracteristicasNiveles]
(
	@p_CodigoLimiteCaracteristica varchar(5)
)
AS
BEGIN
	SET NOCOUNT ON
	select
				CodigoNivelLimite,
				CodigoLimiteCaracteristica,
				NL.CodigoCaracteristica,
				Secuencial,
				CG.Descripcion as 'DescripcionCaracteristica',
				FlagTipoPorcentaje,
				ValorPorcentaje,
				Case NL.Situacion when 'A' then 'ACTIVO' when 'I' then 'INACTIVO' end as Situacion,
				ValorPorcentajeM,	--HDG OT 65023 20120522
				/*CAMBIOS GT 20181114*/
				ISNULL(TieneLimiteNivel,'N') AS TieneLimiteNivel,
				ISNULL(LimiteNivelMin,0) AS LimiteNivelMin,
				ISNULL(LimiteNivelMax,0) AS LimiteNivelMax,
				ISNULL(TieneLimiteFijoEnDetalle,'N') AS TieneLimiteFijoEnDetalle,
				ISNULL(LimiteFijoEnDetalleMin,0) AS LimiteFijoEnDetalleMin,
				ISNULL(LimiteFijoEnDetalleMax,0) AS LimiteFijoEnDetalleMax,
				ISNULL(NL.CodigoRelacion,'') AS CodigoRelacion
	FROM		dbo.NivelLimite NL
	INNER JOIN	CaracteristicaGrupo CG on NL.CodigoCaracteristica = CG.CodigoCaracteristica
	WHERE		CodigoLimiteCaracteristica=@p_CodigoLimiteCaracteristica
	AND			NL.Situacion = 'A' --RGF 20080709
	ORDER BY	Secuencial asc
END
GO

GRANT EXECUTE ON [dbo].[Limite_SeleccionarCaracteristicasNiveles] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[Listar_OrdenesInversion_Relacion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Listar_OrdenesInversion_Relacion') BEGIN 
	DROP PROCEDURE [dbo].[Listar_OrdenesInversion_Relacion]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion: Lista las ordenes de inversion de cierre para crear la relacion con la nueva apertura - OP
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[Listar_OrdenesInversion_Relacion]
@p_FechaOperacion numeric(8),
@p_CodigoPortafolioSBS varchar(5),
@p_CategoriaInstrumento varchar(5),
@p_Estado varchar(5)
AS
BEGIN
	declare @v_Activo char(1)='A',
			@v_CodigoOperacion char(1)='101'

	select P.Descripcion As Portafolio,
		   OI.FechaOperacion,
		   OP.Descripcion As Operacion,
		   MontoNominalOrdenado,
		   MontoNominalOperacion 
	from OrdenInversion OI
	JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS 
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	where (OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS) AND
		  (OI.FechaOperacion = @p_FechaOperacion) AND
		  (OI.Estado = @p_Estado) AND
		  (OI.Situacion = @v_Activo) AND
		  (OI.CodigoOperacion = @v_CodigoOperacion) AND
		  (OI.CategoriaInstrumento = @p_CategoriaInstrumento)
		  
END
GO

GRANT EXECUTE ON [dbo].[Listar_OrdenesInversion_Relacion] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[ListarBancosConfirmacion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ListarBancosConfirmacion') BEGIN 
	DROP PROCEDURE [dbo].[ListarBancosConfirmacion]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Lista la relacion de bancos disponibles por OI
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[ListarBancosConfirmacion]
@p_CodigoOrden varchar(20)
AS
BEGIN

	declare @Activo char(1)='A'
	declare @codigotercero varchar(20)

	select @codigotercero=CodigoTercero
		from OrdenInversion
		where CodigoOrden = @p_CodigoOrden and
			  Situacion = @Activo

	select distinct EntidadFinanciera 
	into #entidades
	From CuentaTerceros 
	where (CodigoTercero = @codigotercero) AND
		  (LiquidacionAutomatica ='N') AND
		  (Situacion = 'A') 

	select CodigoTercero,Descripcion 
	from Terceros 
	where CodigoTercero in ( select CodigoTercero from Entidad 
							 where CodigoEntidad in (select EntidadFinanciera From #entidades)
	)
	drop table #entidades 

END
GO

GRANT EXECUTE ON [dbo].[ListarBancosConfirmacion] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[ListarCuentasBancosConfirmacion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ListarCuentasBancosConfirmacion') BEGIN 
	DROP PROCEDURE [dbo].[ListarCuentasBancosConfirmacion]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Listar numero de cuenta relacionada al banco intermediario
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[ListarCuentasBancosConfirmacion]
@p_CodigoTercero varchar(20),
@p_CodigoOrden varchar(20)
AS
BEGIN

declare @v_CodigoMoneda varchar(5),
		@v_CodigoTercero varchar(25),
		@v_Activo char(1)='A'

select @v_CodigoMoneda = CodigoMoneda, @v_CodigoTercero= CodigoTercero from OrdenInversion
where (CodigoOrden = @p_CodigoOrden) and
	  (Situacion = @v_Activo) 


select case when (Isnull(NumeroCuenta,'')='') Then CuentaInterBancario else NumeroCuenta end  as Codigo,
	  case when (Isnull(NumeroCuenta,'')='') Then CuentaInterBancario else NumeroCuenta end Descripcion  
from CuentaTerceros 
where (CodigoTercero = @v_CodigoTercero) and
	  (CodigoMoneda = @v_CodigoMoneda) and
	  (LiquidacionAutomatica ='N') AND
	  (Situacion = @v_Activo) 


END
GO

GRANT EXECUTE ON [dbo].[ListarCuentasBancosConfirmacion] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[ListarDatosCarta]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ListarDatosCarta') BEGIN 
	DROP PROCEDURE [dbo].[ListarDatosCarta]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Listar datos adicionales para cartas
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[ListarDatosCarta]  
@p_CodigoOrden varchar(20),  
@p_CodigoTipoDato CHAR(1)  
AS  
BEGIN  
	  
	 declare @p_CodigoTercero varchar(25)  
	  
	 select @p_CodigoTercero = CodigoTercero  
	 from OrdenInversion  
	 where CodigoOrden = @p_CodigoOrden  
	  
	 SELECT Valor,CodigoDatosCarta  
	 FROM DatosCarta   
	 WHERE (@p_CodigoTipoDato = '' OR CodigoTipoDato = @p_CodigoTipoDato ) AND  
		(@p_CodigoTercero = '' OR CodigoTercero = @p_CodigoTercero )  
END 
GO

GRANT EXECUTE ON [dbo].[ListarDatosCarta] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[OrdenInversion_DatosCartas]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_DatosCartas') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_DatosCartas]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Actualiza los datos adicionales para cartas en Ordenes de inversion 
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[OrdenInversion_DatosCartas]
@p_CodigoOrden varchar(12),
@p_NumeroCuenta varchar(25),
@p_DTC varchar(5)
AS
BEGIN
	declare @v_CodigoISIN varchar(12), @v_CodigoMnemonico varchar(15), @v_claseInstrumento char(1)

	select splitdata As CodigoClaseInstrumento into #ClaseInstrumento
	from dbo.fnSplitString('1,6,11,15,3,7,6',',')

	select @v_CodigoMnemonico=CodigoMnemonico,@v_CodigoISIN=CodigoISIN From OrdenInversion where CodigoOrden = @p_CodigoOrden
	
	
	select @v_claseInstrumento = CodigoClaseInstrumento from TipoInstrumento 
	where CodigoTipoInstrumentoSBS in ( select CodigoTipoInstrumentoSBS from Valores 
										where CodigoISIN = @v_CodigoISIN  and
											  CodigoNemonico = @v_CodigoMnemonico) and
		  CodigoClaseInstrumento in (select CodigoClaseInstrumento from #ClaseInstrumento)

	update OrdenInversion set NumeroCuenta = @p_NumeroCuenta,
						      DTC = @p_DTC,
							  CodigoModelo = case when(@v_claseInstrumento = '1') then 'BO01' 
												  when(@v_claseInstrumento = '11') then 'BO01' 
												  when(@v_claseInstrumento = '15') then 'BO01' 
												  when(@v_claseInstrumento = '3') then 'BO01' 
												  when(@v_claseInstrumento = '7') then 'BO01' 
												  when(@v_claseInstrumento = '6') then 'CVA1' end
	where CodigoOrden = @p_CodigoOrden

END
GO

GRANT EXECUTE ON [dbo].[OrdenInversion_DatosCartas] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[OrdenInversion_ValidaExterior]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_ValidaExterior') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_ValidaExterior]
END 
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 02/08/2018
--	Creado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Valida si la Orden de Inversion es nacional o del exterior
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[OrdenInversion_ValidaExterior]
@p_CodigoOrden varchar(12),
@p_CodigoOrigen varchar(5) output
AS
BEGIN

	declare @v_Activo char(1)='A'

	select @p_CodigoOrigen = (case when (CodigoPais='604') then 'NAC' ELSE 'EXT' END ) 
	from Terceros where CodigoTercero in ( select CodigoTercero from OrdenInversion where CodigoOrden = @p_CodigoOrden and Situacion = @v_Activo )

END
GO

GRANT EXECUTE ON [dbo].[OrdenInversion_ValidaExterior] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[Portafolio_Modificar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Portafolio_Modificar') BEGIN 
	DROP PROCEDURE [dbo].[Portafolio_Modificar]
END 
GO


---------------------------------------------------------------------------------------------------------------------------------------
--	Fecha de Modificación: 23/09/2016
--	Modificado por: Marlon E. Peña
--	Nro. Orden de Trabajo: 9362
--	Descripción del cambio: Agregar parámetro CodigoRenta para actualizarlo en la tabla Portafolio.
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
-- Fecha modificacion: 17/06/2019
-- Modificado por: Karina Gomez
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Se agrego el campo Consolidado en el mantenimiento Administración de Portafolio. Es un cambio solicitado
--                         para el desarrollo de Límites Fondo, dónde se selecciona un Cliente del tipo Mandato y se indica si el reporte consolidara los saldos no administrativos.
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
	@p_CodigoRenta varchar(3),	-- OT 9362
	@p_TipoCalculoValorCuota varchar(2),
	@p_ValoracionMensual VARCHAR(1), 
	@p_CuotasLiberadas VARCHAR(1), -- OT10916
	@p_CPPadreSisOpe VARCHAR(10),
	@p_VectorPrecioVal VARCHAR(4),
	@p_TipoNegocio VARCHAR(10), -- Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
	@p_FondoCliente VARCHAR(1),
	@p_CodigoTerceroCliente VARCHAR(12),
	@p_FlagAumentoCapital BIT ,
	@p_TipoComision VARCHAR(10),
	@p_Consolidado INT
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
		,FlagAumentoCapital = @p_FlagAumentoCapital
		,Consolidado = @p_Consolidado
		,TipoComision = @p_TipoComision  								   
	WHERE
		CodigoPortafolioSBS = @p_CodigoPortafolioSBS
END
GO

GRANT EXECUTE ON [dbo].[Portafolio_Modificar] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[Portafolio_Seleccionar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Portafolio_Seleccionar') BEGIN 
	DROP PROCEDURE [dbo].[Portafolio_Seleccionar]
END 
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
-- Fecha de Creación		: 2018-08-23
-- Modificado por			: Carlos Rumiche
-- Nro. Orden de Trabajo	: OT11590
-- Descripción				: Elimina secuencia Vector
--								Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
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
-- Fecha modificacion: 17/06/2019
-- Modificado por: Karina Gomez
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Se agrego el campo Consolidado en el mantenimiento Administración de Portafolio. Es un cambio solicitado
--                         para el desarrollo de Límites Fondo, dónde se selecciona un Cliente del tipo Mandato y se indica si el reporte consolidara los saldos no administrativos.
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Portafolio_Seleccionar](
	@p_CodigoPortafolio VARCHAR(10) = ''
)AS
BEGIN
	SET NOCOUNT ON
	DECLARE @FechaValoracion NUMERIC(8)
	
	SELECT	@FechaValoracion = MAX(FechaValoracion) FROM CarteraTituloValoracion  WHERE CodigoPortafolioSBS = @p_CodigoPortafolio
	
	SELECT	PO.[CodigoPortafolioSBS],PO.[Descripcion],PO.[FechaConstitucion],PO.[CodigoTipoValorizacion],TV.Descripcion AS 'NombreTipoValorizacion', 
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
			,ISNULL(PO.Consolidado,0) AS Consolidado
			,ISNULL(TipoComision,'0') AS TipoComision
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



PRINT '[Pr_Generar_ReporteLimites]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Pr_Generar_ReporteLimites') BEGIN 
	DROP PROCEDURE [dbo].[Pr_Generar_ReporteLimites]
END 
GO

----------------------------------------------------------------------------------------------------------------------------------------    
-- Fecha modificacion: 19/06/2019
-- Modificado por: Karina Gomez
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Se modifico el procedimiento para el listado de la nueva pestaña "Consolidado - Cliente Mandato".
-- @Test:EXEC Pr_Generar_ReporteLimites  20190528,'28/05/2019','2666,2777','S'
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Pr_Generar_ReporteLimites]
(
	@p_FechaProceso NUMERIC(8),
	@p_FechaCadena VARCHAR(20),
	@p_CodigoPortafolioSBS VARCHAR(100),
	@p_ClienteMandato CHAR(1)
)
AS

BEGIN
	SET NOCOUNT ON


	DECLARE @tPortafolios AS TABLE(
		Id INT IDENTITY,
		CodigoPortafolio VARCHAR(20)
	);
	DECLARE @tReporteLimite AS TABLE (
		Id INT IDENTITY
		,CodigoLimite VARCHAR(10)
		,Fecha NUMERIC(8)
		,FechaString VARCHAR(10)
		,CodigoPortafolio VARCHAR(20)
		,DescripcionPortafolio VARCHAR(50)
		,CodigoMonedaSBS VARCHAR(5)
		,CodigoMoneda VARCHAR(5)
		,PatrimonioCierre NUMERIC(22,7)
		,CarteraFondo NUMERIC(22,7)
		,TipoLimite VARCHAR(300)
		,Emisor VARCHAR(300)
		,CodigoTercero VARCHAR(20)
		,CodigoEntidad VARCHAR(10)
		,CodigoGruEco VARCHAR(10)
		,DescripcionGruEco VARCHAR(300)
		,PatrimonioEmpresa NUMERIC(22,7)
		,PasivoEmpresa NUMERIC(22,7)
		,RatingFF VARCHAR(10)
		,RatingInterno VARCHAR(10)
		,Rating VARCHAR(10)
		,LineaPlazo VARCHAR(10)
		,Participacion_Por NUMERIC(22,7)
		,LimiteMinInt_Por NUMERIC(22,7)
		,LimiteMaxInt_Por NUMERIC(22,7)
		,LimiteMinLeg_Por NUMERIC(22,7)
		,LimiteMaxLeg_Por NUMERIC(22,7)
		,ExcesoInterno_Por NUMERIC(22,7)
		,ExcesoInterno NUMERIC(22,7)
		,ExcesoLegal_Por NUMERIC(22,7)
		,ExcesoLegal NUMERIC(22,7)
		,Alerta_Por NUMERIC(22,7)
		,DescripcionAlerta VARCHAR(300)
		,FlagExceso VARCHAR(1)
		,CabeceraLimite VARCHAR(1)
		,ValorBase NUMERIC(22,7)
		,Consumo NUMERIC(22,7)
		
		,CodigoTerceroLimite varchar(20)
		,DescripcionTerceroLimite varchar(100)
		,ValorBaseLimite  NUMERIC(22,7)
		,ValorPorcentaje  NUMERIC(22,7)
		,Posicion  NUMERIC(22,7)
		--,TipoCuenta VARCHAR(10)
		,ValorNivel VARCHAR(100)
		,CodigoCaracteristicaRelacion VARCHAR(5)
		,CodigoRelacion VARCHAR(5)
	)
	
	DECLARE @TotalPortafolio INT=0
	DECLARE @CONT INT= 1


	INSERT INTO @tPortafolios
	SELECT CAMPO FROM DBO.fn_SplitLista(@p_CodigoPortafolioSBS) 

	SET @TotalPortafolio=(SELECT COUNT(*) FROM @tPortafolios)

	if(@TotalPortafolio = 0) 	
	BEGIN
		INSERT INTO @tPortafolios VALUES('')
		SET @TotalPortafolio=(SELECT COUNT(*) FROM @tPortafolios)
	END

	WHILE(@CONT <= @TotalPortafolio)
	BEGIN 

	set @p_CodigoPortafolioSBS = (select CodigoPortafolio from @tPortafolios where Id = @CONT)

		SELECT p.CodigoPortafolioSBS, SUM(ISNULL(VPNMonedaFondo,0)) Total
		INTO #TotalCartera
		FROM Portafolio p WITH(NOLOCK) INNER JOIN CarteraTituloValoracion c WITH(NOLOCK) ON p.CodigoPortafolioSBS = c.CodigoPortafolioSBS
		WHERE p.Situacion = 'A' AND c.FechaValoracion = @p_FechaProceso AND c.Escenario = 'REAL' 
		GROUP BY p.CodigoPortafolioSBS

		SELECT r.CodigoReporte, r.CodigoLimite, r.CodigoLimiteCaracteristica, r.FechaReporte, r.Secuencial, r.CodigoNivelLimite, r.Tipo, r.Tope,
			r.ValorPorcentaje, r.Posicion, r.Participacion, r.Margen, r.Alerta, r.ValorNivel, r.DescripcionNivel, r.PorVal, r.Patrimonio, r.ValorBase,
			r.Cxc, r.Cxp, r.TotalInversiones, r.SaldoBanco, r.Factor, r.ValorEfectivoColocado, r.FloatOficioMultiple, r.UnidadesEmitidas, r.TotalActivo,
			r.TotalPasivo, r.PosicionF1, r.PosicionF2, r.PosicionF3, r.FechaVencimiento, r.FechaOperacion, r.CodigoMoneda, r.Escenario, r.NivelSaldoBanco,
			r.ValorPorcentajeM, r.Valor1, r.Valor2, r.Valor3, r.ValorPorcentajeMinimo, r.MargenMinimo, lc.CodigoPortafolioSBS,
			r.CodigoCaracteristicaRelacion, r.CodigoRelacion
		INTO #ReporteLimites
		FROM ReporteLimites r WITH(NOLOCK)
			INNER JOIN LimiteCaracteristica lc WITH(NOLOCK) ON r.CodigoLimiteCaracteristica = lc.CodigoLimiteCaracteristica AND lc.Situacion = 'A'
		WHERE FechaReporte = @p_FechaProceso
			AND (lc.CodigoPortafolioSBS = @p_CodigoPortafolioSBS OR @p_CodigoPortafolioSBS = '')
		ORDER BY CodigoLimite ASC
	
		SELECT 
				CodigoLimite,
				CodigoLimiteCaracteristica
		INTO	#RerpoteLimitesCabeceras
		FROM	#ReporteLimites 
		WHERE	Secuencial = 1

		Declare @CodigoLimite varchar(10), @CodigoLimiteCaracteristica varchar(10), @secuencialMax int, @LimiteNivelMin numeric(22,7), @LimiteNivelMax numeric(22,7)

		DECLARE cur_Nivel CURSOR FOR
			SELECT CodigoLimite,CodigoLimiteCaracteristica FROM #RerpoteLimitesCabeceras
		OPEN cur_Nivel
			FETCH NEXT FROM cur_Nivel INTO @CodigoLimite,@CodigoLimiteCaracteristica
			WHILE @@FETCH_STATUS = 0 
			BEGIN
				SELECT @secuencialMax = MAX(Secuencial) FROM NivelLimite WHERE CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica 

				SELECT @LimiteNivelMin = LimiteNivelMin, @LimiteNivelMax = LimiteNivelMax FROM NivelLimite WHERE CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica AND Secuencial = @secuencialMax

				UPDATE	#ReporteLimites
				SET		ValorPorcentaje = @LimiteNivelMax,
						ValorPorcentajeM = @LimiteNivelMin
				WHERE	CodigoLimite = @CodigoLimite
				AND		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
				AND		Secuencial = 1

				FETCH NEXT FROM cur_Nivel INTO @CodigoLimite,@CodigoLimiteCaracteristica
			END
		CLOSE cur_Nivel 
		DEALLOCATE cur_Nivel
	

		SELECT a.CodigoPortafolioSBS, ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite) Grupo,
			--Validacion = ''
			Validacion = CASE
						WHEN b.Cuadrar = 1 AND (f.TipoRenta IS NULL OR f.TipoRenta = e.IdTipoRenta_FK) THEN CASE --Limites 01, 02, 03, 04, 06, 10, 14
													--OT 9733 Inicio
													WHEN (ROUND(SUM(d.Participacion), 2) = 100 AND ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite) <> '10') OR (ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite) = '10' AND ROUND((SELECT Total FROM #TotalCartera WHERE CodigoPortafolioSBS = a.CodigoPortafolioSBS), 1, 1) = ROUND(SUM(d.Posicion), 1, 1)) THEN 0
													--OT 9733 Fin
													ELSE 1
												END
						WHEN b.Cuadrar = 2 THEN CASE --Limites 15, 16
													WHEN SUM(CASE
																WHEN d.ValorBase = 0 THEN 1
																ELSE 0
															END) > 1 THEN 1
													ELSE 0
												END
						WHEN b.Cuadrar = 3 THEN CASE --Limites 17
													WHEN SUM(CASE
																WHEN LEFT(LTRIM(RTRIM(ISNULL(DescripcionNivel, ''))), 11) = 'SIN ASIGNAR' THEN 1
																ELSE 0
															END) > 0 THEN 1
													WHEN SUM(CASE
																WHEN d.ValorBase = 0 THEN 1
																ELSE 0
															END) > 1 THEN 1
													ELSE SUM(CASE
																WHEN ValorPorcentaje IS NULL THEN 1
																ELSE 0
															END)
													END
						ELSE 0 --Limites 05, 07, 08, 09
						END
		INTO #Limites
		FROM LimiteCaracteristica a WITH(NOLOCK) INNER JOIN Limite b WITH(NOLOCK) ON a.CodigoLimite = b.CodigoLimite
									INNER JOIN Portafolio e WITH(NOLOCK) ON a.CodigoPortafolioSBS = e.CodigoPortafolioSBS
									LEFT JOIN Grupo_Limite c WITH(NOLOCK) ON b.CodigoLimite = c.CodigoLimite AND c.TipoRenta = ''
									LEFT JOIN Grupo_Limite f WITH(NOLOCK) ON b.CodigoLimite = f.CodigoLimite AND f.TipoRenta <> ''
									LEFT JOIN #ReporteLimites d ON a.CodigoLimiteCaracteristica = d.CodigoLimiteCaracteristica
		WHERE a.Situacion = 'A' AND b.Situacion = 'A' AND a.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS = '' THEN a.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END
		GROUP BY a.CodigoPortafolioSBS, ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite), b.Cuadrar, e.IdTipoRenta_FK, f.TipoRenta
	
	
		SELECT a.CodigoPortafolioSBS, b.CodigoLimite, b.NombreLimite
				, (SELECT Validacion FROM #Limites WHERE CodigoPortafolioSBS = a.CodigoPortafolioSBS AND Grupo = ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite)) Validacion,
			b.Cuadrar, ROUND(SUM(ISNULL(r.Posicion, 0)), 2) TotalPosicion, ROUND(SUM(ISNULL(r.Participacion, 0)), 2) TotalParticipacion
		INTO #Validacion
		FROM LimiteCaracteristica a WITH(NOLOCK) INNER JOIN Limite b WITH(NOLOCK) ON a.CodigoLimite = b.CodigoLimite
									LEFT JOIN Grupo_Limite c WITH(NOLOCK) ON b.CodigoLimite = c.CodigoLimite AND c.TipoRenta = ''
									LEFT JOIN #ReporteLimites r WITH(NOLOCK) ON a.CodigoLimite = r.CodigoLimite AND a.CodigoLimiteCaracteristica = r.CodigoLimiteCaracteristica
		WHERE a.Situacion = 'A' AND b.Situacion = 'A' AND a.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS = '' THEN a.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END
		GROUP BY a.CodigoPortafolioSBS, b.CodigoLimite, b.NombreLimite, ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite), b.Cuadrar
	
	
		INSERT INTO @tReporteLimite (
			CodigoLimite
			,Fecha
			,FechaString
			,CodigoPortafolio
			,DescripcionPortafolio
			,CodigoMonedaSBS
			,CodigoMoneda
			,PatrimonioCierre
			,CarteraFondo
			,TipoLimite
			,Emisor
			,CodigoTercero
			,CodigoEntidad
			,CodigoGruEco
			,DescripcionGruEco
			,PatrimonioEmpresa
			,PasivoEmpresa
			,RatingFF
			,RatingInterno
			,Rating
			,LineaPlazo
			,Participacion_Por
			,LimiteMinInt_Por
			,LimiteMaxInt_Por
			,LimiteMinLeg_Por,LimiteMaxLeg_Por,ExcesoInterno_Por,ExcesoInterno,ExcesoLegal_Por,ExcesoLegal
			,Alerta_Por,DescripcionAlerta,FlagExceso,CabeceraLimite
			,ValorBase
			,Consumo
			,CodigoTerceroLimite
			,DescripcionTerceroLimite
			,ValorBaseLimite
			,ValorPorcentaje
			,Posicion
			--,TipoCuenta
			,ValorNivel
			,CodigoCaracteristicaRelacion
			,CodigoRelacion
		)
		SELECT
			r.CodigoLimite,
			r.FechaReporte,
			@p_FechaCadena,
			r.CodigoPortafolioSBS
			,p.Descripcion
			,m.CodigoMonedaSBS
			,m.CodigoMoneda
			/* ,v.TotalPosicion  */ -- CRumiche : 
			,PatrimonioCierre = dbo.fn_sit_ObtenerPatrimonioPorPortafolio(@p_FechaProceso, @p_CodigoPortafolioSBS)
			,r.Posicion,
			CASE WHEN r.ValorNivel IS NULL THEN v.NombreLimite ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END 
			,''
			, CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.ValorNivel, '')) END entidad
			,''
			,''
			,''
			,0
			,0
			,''
			,''
			,''
			,''
			,CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.Participacion, 0) END
			,0
			,ISNULL((CASE L.Tipo WHEN 'L' THEN 0 WHEN 'I' THEN r.ValorPorcentaje END),0)
			,ISNULL(r.ValorPorcentajeM, 0)
			,ISNULL((CASE L.Tipo WHEN 'L' THEN r.ValorPorcentaje WHEN 'I' THEN 0 END),0)
			,0
			,0
			,0
			,0
			,0,'',CASE WHEN r.Participacion > r.ValorPorcentaje THEN 'S' ELSE 'N' END,CASE WHEN r.DescripcionNivel = 'SUMA VALORES DE LOS GRUPOS' THEN 'S' ELSE 'N' END
	
			,CASE WHEN ISNULL(r.ValorBase,0) <> 0 and ISNULL(r.valorporcentaje,0) <> 0 THEN (r.ValorBase * ( ValorPorcentaje / 100)) ELSE 0 END
			,CASE when ISNULL(r.ValorBase,0) <> 0 and ISNULL(r.valorporcentaje,0) <> 0 THEN (r.Posicion / (r.ValorBase * ( ValorPorcentaje / 100))) ELSE 0 END
			,ISNULL(E.CodigoTercero,r.ValorNivel) AS CodigoTerceroLimite
			,ISNULL(T.Descripcion,'') TerceroLimite
			,r.ValorBase as ValorBaseLimite
			,r.valorporcentaje
			,r.Posicion
			--,VLD.CodigoValidador as TipoCuenta
			,r.ValorNivel
			,r.CodigoCaracteristicaRelacion
			,r.CodigoRelacion
		FROM #ReporteLimites r INNER JOIN #Validacion v ON r.CodigoPortafolioSBS = v.CodigoPortafolioSBS AND r.CodigoLimite = v.CodigoLimite
								INNER JOIN Portafolio p WITH(NOLOCK) ON r.CodigoPortafolioSBS = p.CodigoPortafolioSBS
								INNER JOIN Moneda m WITH(NOLOCK) ON p.CodigoMoneda = m.CodigoMoneda
								INNER JOIN Limite L ON r.CodigoLimite = L.CodigoLimite
								--LEFT JOIN ValidadorLimiteDetalle VLD ON VLD.CodigoLimite = L.CodigoLimite and VLD.Tipo='2'
								LEFT JOIN Entidad E ON E.CodigoEntidad = r.ValorNivel
								LEFT JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero
		WHERE r.CodigoLimiteCaracteristica NOT IN (SELECT z.CodigoLimiteCaracteristica
													FROM (SELECT x.CodigoLimiteCaracteristica,  COUNT(1) Cantidad
															FROM #ReporteLimites x INNER JOIN #Validacion y ON x.CodigoPortafolioSBS = y.CodigoPortafolioSBS AND x.CodigoLimite = y.CodigoLimite
															WHERE y.Validacion = 0
															GROUP BY x.CodigoLimiteCaracteristica) z
													WHERE z.Cantidad = 1)
		ORDER BY p.Descripcion, r.CodigoLimite, CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END
			
		drop table #TotalCartera 
		drop table #ReporteLimites
		drop table #RerpoteLimitesCabeceras 
		drop table #Limites 
		drop table #Validacion

		SET @CONT = @CONT +1 
		
	END
	


	select * into #DetalleNiveles
	from  @tReporteLimite RL 
	where (CodigoCaracteristicaRelacion is not null) AND (CodigoRelacion is not null)
	
	select ROW_NUMBER() OVER(ORDER BY CodigoCaracteristicaRelacion) Id, CodigoCaracteristicaRelacion, CodigoRelacion
	INTO #Niveles
	from (  select DISTINCT CodigoCaracteristicaRelacion,CodigoRelacion
			from  #DetalleNiveles RL 
			where (CodigoCaracteristicaRelacion is not null) AND
				  (CodigoRelacion is not null)
	) as TRelacion where (CodigoCaracteristicaRelacion !='' and CodigoRelacion !='')

	declare @total int=0, @contNivel int=1, @CodigoCaracteristicaRelacion varchar(5), @sql varchar(max),
			@nombreColumna varchar(50),@NombreVista varchar(100),@CodigoRelacion varchar(5), @v_codigoLimite varchar(5),@letra char(2),@CarteraFodo  NUMERIC(22,7)
	
	DECLARE @Abecedario varchar(max)='A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z'
	
	SELECT ROW_NUMBER() OVER(ORDER BY SPLITDATA) as Id, SPLITDATA as Letra 
	INTO #Abecedario FROM DBO.fnSplitString(@Abecedario,',')
	
	SELECT DISTINCT ABC.Letra,RL.TipoLimite,RL.CodigoRelacion,RL.VALORNIVEL,ROW_NUMBER() OVER(PARTITION BY RL.CODIGORELACION ORDER BY RL.CODIGORELACION,RL.VALORNIVEL, RL.TipoLimite ) as Contador 
	INTO #NivelCabecera
	from  @tReporteLimite RL 
	JOIN #Abecedario ABC ON RL.CodigoRelacion = ABC.Id 
	where (CodigoCaracteristicaRelacion is not null) AND (CodigoRelacion is not null)
	GROUP BY ABC.Letra,RL.TipoLimite,RL.CODIGORELACION,RL.VALORNIVEL
	
	UPDATE RL SET RL.TipoLimite = CHAR(32)+CHAR(32)+ NC.Letra+'.'+Convert(Varchar,NC.Contador)+') '+NC.TipoLimite
	FROM @tReporteLimite RL 
	JOIN  #NivelCabecera NC ON RL.ValorNivel = NC.ValorNivel AND 
							   RL.CodigoRelacion = NC.CodigoRelacion 
	

	set @total = (select count(*)from #Niveles)
	
	WHILE(@contNivel <= @total)
	begin
		select @CodigoCaracteristicaRelacion = CodigoCaracteristicaRelacion,
		       @CodigoRelacion = CodigoRelacion
		from #Niveles where Id = @contNivel
		
		select @nombreColumna=NombreColumna,@NombreVista=NombreVista 
		from CaracteristicaGrupo 
		where CodigoCaracteristica=@CodigoCaracteristicaRelacion

		select @letra=Letra+')' from #Abecedario where Id = @contNivel 
		
		select @CarteraFodo=sum(CarteraFondo) from #DetalleNiveles  where CodigoRelacion = @CodigoRelacion 

		set @sql='select Distinct Fecha, FechaString,CodigoPortafolio,DescripcionPortafolio,CodigoMonedaSBS,CodigoLimite,PatrimonioCierre, '''+@letra+' ''+Descripcion,'+Convert(varchar,@CarteraFodo)+','''+@CodigoRelacion+''',0 
				  from '+@NombreVista+' T 
				  join #DetalleNiveles N on N.CodigoRelacion = T.Codigo AND 1=1
				  where T.Codigo='''+@CodigoRelacion+''''
		print @sql
		insert @tReporteLimite (Fecha,FechaString,CodigoPortafolio,DescripcionPortafolio,CodigoMonedaSBS,CodigoLimite,PatrimonioCierre,TipoLimite,CarteraFondo,CodigoRelacion,ValorNivel)
		exec(@sql)

		set @contNivel= @contNivel+1
	end


	IF(@p_ClienteMandato = 'S' )
		BEGIN
			
			DECLARE @CODIGOCLIENTEMANDATO VARCHAR(20), @CLIENTEMANDATO VARCHAR(100)
			SELECT DISTINCT @CODIGOCLIENTEMANDATO=T.CodigoTercero,@CLIENTEMANDATO=T.Descripcion
			FROM @tPortafolios P 
			JOIN Portafolio PF ON P.CodigoPortafolio = PF.CodigoPortafolioSBS 
			JOIN Terceros T ON T.CodigoTercero = PF.CodigoTerceroCliente  
			-- Cabecera
			SELECT
				   RL.Fecha,
				   RL.FechaString,
				   @CLIENTEMANDATO ClienteMandato,
				   SUM(RL.PatrimonioCierre) AS PatrimonioCierre,
				   0 AS CarteraFondo,
				   RL.TipoLimite,
				   RL.CodigoTerceroLimite,
				   RL.CodigoMoneda,
				   RL.CodigoMonedaSBS,
				   0 AS ValorBaseLimite,
				   0 AS Consumo,
				   --ISNULL(RL.TipoCuenta,'') AS TipoCuenta,
				   RL.CabeceraLimite,
				   RL.ValorNivel,
				   RL.CodigoCaracteristicaRelacion,
				   RL.CodigoRelacion,
				   RL.CodigoLimite 
			INTO #ResultadoMandatoCabecera
			FROM @tReporteLimite RL
			WHERE RL.CabeceraLimite = 'S'
			GROUP BY RL.Fecha, RL.FechaString,RL.TipoLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.CodigoMonedaSBS,RL.CabeceraLimite,RL.ValorNivel, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion,RL.CodigoLimite 
			--GROUP BY RL.Fecha, RL.FechaString,RL.TipoLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.CodigoMonedaSBS,RL.CabeceraLimite, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion,RL.CodigoLimite 
			--GROUP BY RL.Fecha, RL.FechaString,RL.TipoLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.CodigoMonedaSBS,RL.TipoCuenta,RL.CabeceraLimite, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion,RL.CodigoLimite 


			-- Detalle
			SELECT RL.Fecha,
				   RL.FechaString,
				   @CLIENTEMANDATO ClienteMandato,
				   SUM(RL.PatrimonioCierre) AS PatrimonioCierre,
				   SUM(RL.CarteraFondo) AS CarteraFondo,
				   RL.TipoLimite,
				   RL.CodigoTerceroLimite,
				   RL.CodigoMoneda,
				   RL.CodigoMonedaSBS,
				   SUM(CASE WHEN ISNULL(RL.ValorBaseLimite,0) <> 0 and ISNULL(RL.valorPorcentaje,0) <> 0 THEN (RL.ValorBaseLimite * ( ValorPorcentaje / 100)) ELSE 0 END)/@TotalPortafolio AS ValorBaseLimite,
				   SUM(CASE when ISNULL(RL.ValorBaseLimite,0) <> 0 and ISNULL(RL.valorPorcentaje,0) <> 0 THEN (RL.Posicion / (RL.ValorBaseLimite * ( ValorPorcentaje / 100))) ELSE 0 END) /@TotalPortafolio AS Consumo,
				   --ISNULL(RL.TipoCuenta,'') AS TipoCuenta,
				   RL.CabeceraLimite,
				   RL.ValorNivel,
				   RL.CodigoCaracteristicaRelacion,
				   RL.CodigoRelacion,
				   RL.CodigoLimite 
			INTO #ResultadoMandatoDetalle
			FROM @tReporteLimite RL
			WHERE CabeceraLimite = 'N' OR CabeceraLimite is null
			--GROUP BY RL.Fecha, RL.FechaString,RL.TipoLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.CodigoMonedaSBS,RL.TipoCuenta,RL.CabeceraLimite,RL.ValorNivel, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion,RL.CodigoLimite 
			GROUP BY RL.Fecha, RL.FechaString,RL.TipoLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.CodigoMonedaSBS,RL.CabeceraLimite,RL.ValorNivel, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion,RL.CodigoLimite 
			
			
			update #ResultadoMandatoDetalle set CarteraFondo =CarteraFondo/@TotalPortafolio
			where ValorNivel = '0' 

			declare @vMes as varchar(10), @Anhio as varchar(10), @FechaProcesoCadena varchar(10)

			Select @FechaProcesoCadena = cast(@p_FechaProceso as varchar(10))

			SELECT @vMes= SUBSTRING(@FechaProcesoCadena,5,2),  @Anhio = SUBSTRING(@FechaProcesoCadena,1,4)

			--set @vFecha=(select max(Fecha) from SaldosNoAdministrados)

			--update #ResultadoMandatoDetalle SET CarteraFondo = 
			--	(isnull(CarteraFondo,0) + dbo.ObtenerMontoSaldoNoAdministrado(CodigoTerceroLimite,CodigoMoneda,@vFecha,CodigoLimite))

				update #ResultadoMandatoDetalle SET CarteraFondo = 
				(isnull(CarteraFondo,0) + dbo.ObtenerMontoSaldoNoAdministrado(CodigoTerceroLimite,CodigoMoneda,@p_FechaProceso,@vMes,@Anhio,CodigoLimite))

			--update RL SET RL.CarteraFondo = --SNA.Saldo +  ISNULL(RL.CarteraFondo,0)
			--	ISNULL(RL.CarteraFondo,0) +
			--							(SNA.Saldo * (case when(SNA.CodigoMoneda = 'NSOL') then 1 ELSE dbo.RetornarTipoCambio(SNA.CodigoMoneda,SNA.Fecha) END)) 
			--FROM #ResultadoMandatoDetalle RL 
			--JOIN SaldosNoAdministrados SNA ON SNA.CodigoTercerFinanciero = RL.CodigoTerceroLimite AND
			--								  --SNA.CodigoMoneda = RL.CodigoMoneda AND 
			--								  SNA.TipoCuenta = RL.TipoCuenta 
			--WHERE SNA.CodigoTercero = @CODIGOCLIENTEMANDATO AND
			--	  SNA.Situacion = 'A' AND
			--	  SNA.Fecha = @vFecha


			--Actualizacion de la CarteraFondo del segundo nivel del limite 310
			UPDATE A1 SET carterafondo = A2.Monto
			FROM #ResultadoMandatoDetalle A1 INNER JOIN 
			(select sum(R2.CarteraFondo) Monto, R2.CodigoRelacion from #ResultadoMandatoDetalle R1
			INNER JOIN #ResultadoMandatoDetalle R2 on R1.CodigoRelacion = R2.CodigoRelacion
			where R1.ValorNivel = '0' and R2.CabeceraLimite = 'N'
			group by R2.CodigoRelacion) A2 on A1.CodigoRelacion = A2.CodigoRelacion 
			and A1.ValorNivel = '0'

			delete from @tReporteLimite

			INSERT INTO @tReporteLimite(Fecha,FechaString,DescripcionPortafolio, PatrimonioCierre, CarteraFondo,TipoLimite,CodigoTerceroLimite,CodigoMoneda,CodigoMonedaSBS,ValorBase,Consumo,ValorNivel,CodigoCaracteristicaRelacion,CodigoRelacion,CodigoLimite)
								 SELECT Fecha,FechaString,ClienteMandato, PatrimonioCierre,CarteraFondo,TipoLimite,CodigoTerceroLimite,CodigoMoneda,CodigoMonedaSBS,ValorBaseLimite,Consumo,ValorNivel,CodigoCaracteristicaRelacion,CodigoRelacion,CodigoLimite
			FROM #ResultadoMandatoCabecera

			INSERT INTO @tReporteLimite(Fecha,FechaString,DescripcionPortafolio, PatrimonioCierre, CarteraFondo,TipoLimite,CodigoTerceroLimite,CodigoMoneda,CodigoMonedaSBS,ValorBase,Consumo,ValorNivel,CodigoCaracteristicaRelacion,CodigoRelacion,CodigoLimite)
								 SELECT Fecha,FechaString,ClienteMandato, PatrimonioCierre,CarteraFondo,TipoLimite,CodigoTerceroLimite,CodigoMoneda,CodigoMonedaSBS,ValorBaseLimite,Consumo,ValorNivel,CodigoCaracteristicaRelacion,CodigoRelacion,CodigoLimite
			FROM #ResultadoMandatoDetalle
			WHERE CarteraFondo > 0
			
			DROP TABLE #ResultadoMandatoCabecera
			DROP TABLE #ResultadoMandatoDetalle

		END

		IF(@p_ClienteMandato = 'S' )
		BEGIN
			--print 'A'
			SELECT * FROM @tReporteLimite a
			ORDER BY  a.CodigoLimite, a.CodigoRelacion,
			a.ValorNivel, a.TipoLimite

		END
		ELSE
		BEGIN
		--print 'B'
			
			SELECT * FROM  
			(
				SELECT * FROM @tReporteLimite b where b.CabeceraLimite = 'S'  
				UNION
				SELECT * FROM @tReporteLimite c where (CabeceraLimite is null or CabeceraLimite = 'N')
				AND c.CarteraFondo > 0
			) a
			ORDER BY a.CodigoLimite, a.CodigoRelacion,
			a.ValorNivel, a.TipoLimite
		END

	DROP TABLE #DetalleNiveles
	DROP TABLE #Niveles
	DROP TABLE #Abecedario
	DROP TABLE #NivelCabecera

	SET NOCOUNT OFF
END

GO

GRANT EXECUTE ON [dbo].[Pr_Generar_ReporteLimites] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[sp_SIT_CompraVentaAcciones]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_CompraVentaAcciones') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_CompraVentaAcciones]
END 
GO

CREATE PROCEDURE [dbo].[sp_SIT_CompraVentaAcciones]
@p_CodigoOrden VARCHAR(500),
@p_CodigoModelo VARCHAR(20)
AS
BEGIN
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 28/05/2019
--	Creado por: Karina Gomez
--	Nro. Orden de Trabajo: 12163
--  Descripción: Lista los Datos para la carta Compra y Acciones Nacionales
-----------------------------------------------------------------------------------------------------------
declare @Situacion char(1) = 'A',
	    @BancoPublico VarChar(100),
		@CuentaBancoPublico varchar(50)='2867187'

declare @Resultado as Table(BancoPublico varchar(100), Portafolio varchar(100),NumeroCuentaOrigen varchar(100),
NumeroCuentaDestino varchar(100),FechaOperacion varchar(10),FechaLiquidacion varchar(10),Contraparte varchar (100),
Empresa varchar(50), Importe numeric(27,6), Cantidad INT, CodigoISIN varchar(20),Simbolo varchar(10), 
Moneda varchar(10), Operacion varchar(100), CodigoPortafolioSBS varchar(10),CodigoOrden varchar(10),
Tipo char(3),NombreBanco varchar(100),ObservacionCarta varchar(500), TipoObservacionCarta varchar(20),
Firma1 varchar(100), Firma2 varchar(100),NumeroCuenta varchar(100), CodigoOperacion varchar(10), 
NombreUsuarioF1 varchar(100),NombreUsuarioF2 varchar(100), CargoUsuarioF1 varchar(100),CargoUsuarioF2 varchar(100),
CodigoUsuarioF1 varchar(20), CodigoUsuarioF2 varchar(20),FechaLiquidacionCompleto varchar(50),FechaOperacionCompleto varchar(50), Caso char(1), 
DTC varchar(20), CuentaAbono varchar(50), Custodio varchar(100), DtcCustodio varchar(50), NombreCortoPortafolio varchar(50))

SELECT @BancoPublico = T.Descripcion From ParametrosGenerales pg
JOIN Terceros T ON T.CodigoTercero = PG.Valor
WHERE Clasificacion = 'BANFONPUB'

DECLARE @codigoOrden VARCHAR(12)
DECLARE @NombreBanco VARCHAR(100)
DECLARE @codigoFirmante1 VARCHAR(20)
DECLARE @codigoFirmante2 VARCHAR(20)

						SELECT
							 @NombreBanco = T.Descripcion
							,@codigoFirmante1 = ISNULL(FC.VBGERF1,'')
							,@codigoFirmante2 = ISNULL(FC.VBGERF2,'')
						FROM OrdenInversion OI INNER JOIN CuentaTerceros CE 
						  ON OI.CodigoMoneda = CE.CodigoMoneda  AND
							 LiquidacionAutomatica = 'S' AND 
							 OI.CodigoTercero = CE.CodigoTercero 
							INNER JOIN Entidad E ON CE.EntidadFinanciera = E.CodigoEntidad
							INNER JOIN Terceros T ON E.CodigoTercero = T.CodigoTercero
							LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
						WHERE (@p_CodigoOrden IS NOT NULL AND OI.CodigoOrden   in 
										(select  campo from [dbo].[fn_SplitLista](@p_CodigoOrden))) 
						
						INSERT INTO @Resultado
						SELECT
							@BancoPublico AS BancoPublico
							,'Fondo ' + P.NombreCompleto AS Portafolio
							, CASE WHEN OI.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END +  ISNULL(CE.NumeroCuenta,@CuentaBancoPublico)  AS NumeroCuentaOrigen
							, ISNULL(OI.NumeroCuenta,'SIN DEFINIR') AS NumeroCuentaDestino --CTE.NumeroCuenta
							, dbo.FormatDate(OI.FechaOperacion) AS FechaOperacion
							, dbo.FormatDate(OI.FechaLiquidacion ) AS FechaLiquidacion
							,TE.Descripcion AS Contraparte
							,OI.CodigoMnemonico AS Empresa
							,OI.MontoNetoOperacion  AS Importe
							,convert(INT, OI.CantidadOperacion) AS Cantidad
							,OI.CodigoISIN
							,M.Simbolo
							,M.SinonimoISO as Moneda
							,OP.Descripcion as Operacion
							,OI.CodigoPortafolioSBS
							,RANK() OVER(ORDER BY  TE.Descripcion) AS CodigoOrden
							,CASE WHEN(TE.CodigoPais = '604') THEN 'NAC' ELSE 'EXT' END Tipo
							,isnull(T.Descripcion,@BancoPublico) AS NombreBanco
							,'OBS: ' + ISNULL(OBS.Observacion,OI.ComentariosE) AS ObservacionCarta
							,TipoObservacionCarta = ISNULL(CASE WHEN ((T.CodigoTercero IS NULL) OR (T.CodigoTercero = '')) AND OI.CodigoOperacion = '4' THEN '1'
								WHEN ((T.CodigoTercero IS NOT NULL) OR (T.CodigoTercero != '')) AND (OI.CodigoOperacion = '4') THEN '0' 
								WHEN (OI.CodigoOperacion = '3') THEN '2' END, '')
							,Firma1 = ISNULL(AC1.Firma,'')  
							,Firma2 = ISNULL(AC2.Firma,'')
							,NumeroCuenta = CASE WHEN TE.Descripcion IS NULL THEN
												CASE WHEN OI.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END +  ISNULL(OI.NumeroCuenta,@CuentaBancoPublico) --CTE.NumeroCuenta
											ELSE
												CASE WHEN ISNULL(OI.NumeroCuenta,'SIN DEFINIR') IS NULL THEN
													'CUENTA MATRIZ'
												ELSE
													CASE WHEN OI.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END + ISNULL(OI.NumeroCuenta,@CuentaBancoPublico) 
												END
											END
							, OI.CodigoOperacion
							, NombreUsuarioF1 = RTRIM(LTRIM(ISNULL(DBO.RetornarNombrePersonal(FC.VBGERF1),'')))
							, NombreUsuarioF2 = RTRIM(LTRIM(ISNULL(DBO.RetornarNombrePersonal(FC.VBGERF2),'')))
							, CargoUsuarioF1 = RTRIM(LTRIM(ISNULL(dbo.RetornarCargoPersonal(FC.VBGERF1),'')))
							, CargoUsuarioF2 = RTRIM(LTRIM(ISNULL(dbo.RetornarCargoPersonal(FC.VBGERF2),'')))
							, CodigoUsuarioF1 = FC.VBGERF1 
							, CodigoUsuarioF2 = FC.VBGERF2 
							, FechaLiquidacionCompleto = dbo.RetornarFechaCompleta(OI.FechaLiquidacion) 
							, FechaOperacionCompleto = dbo.RetornarFechaCompleta(OI.FechaOperacion) 
							,CASE WHEN(OI.CodigoOperacion = 1) THEN 'C' ELSE 'V' END Caso
							, isnull(DC.Valor,'') as DTC
							--Numero de cuenta de la Contraparte
							--,CASE WHEN(isnull(OI.NumeroCuenta,'') = '') then  isnull(CTE.NumeroCuenta,'SIN DEFINIR') else isnull(OI.NumeroCuenta,'') END AS CuentaAbono
							,isnull(OI.NumeroCuenta,'')  AS CuentaAbono
							,Case when(TE.CodigoPais <> '604') then COI.Descripcion ELSE isnull(C.Descripcion,'') END as Custodio
							--DTC Orden de Inversion 
							,isnull(DC.Valor,'') as DtcCustodio
							,P.Descripcion As NombreCortoPortafolio
						FROM OrdenInversion OI
							INNER JOIN Portafolio P		   ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS
							LEFT JOIN CuentasPorCobrarPagar CPCP ON CPCP.CodigoOrden = OI.CodigoOrden AND  
																	CPCP.Situacion = OI.Situacion AND
																	CPCP.CodigoMoneda = OI.CodigoMoneda
							LEFT JOIN CuentaEconomica  CE ON   CE.codigoclasecuenta = '20'	AND
															   CE.SITUACION ='A' AND
															   CE.CodigoMoneda = OI.CodigoMoneda AND 
															   CE.NumeroCuenta = ISNULL(OI.NumeroCuenta,CPCP.NumeroCuenta) 
							LEFT JOIN Entidad E ON CE.EntidadFinanciera = E.CodigoEntidad
							LEFT JOIN Terceros T ON E.CodigoTercero = T.CodigoTercero
							LEFT JOIN CUENTATERCEROS CT ON CT.CodigoTercero = OI.CodigoTercero AND
															CT.LiquidacionAutomatica = 'N' AND
															CT.EntidadFinanciera = CE.EntidadFinanciera AND
															CT.CodigoMoneda  = CE.CodigoMoneda AND
														    (OI.NumeroCuenta IS NOT NULL AND CT.NumeroCuenta = OI.NumeroCuenta)
							LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
							LEFT JOIN Terceros TE ON OI.CodigoTercero = TE.CodigoTercero
							INNER JOIN Moneda M ON M.CodigoMoneda = OI.CodigoMoneda
							--LEFT JOIN CuentaTerceros CTE ON TE.CodigoTercero = CTE.CodigoTercero AND OI.CodigoMoneda = CTE.CodigoMoneda AND CTE.LiquidacionAutomatica = 'N'
							JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
							LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = FC.VBGERF1
							LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = FC.VBGERF2
							LEFT JOIN Custodio C ON C.CodigoCustodio = T.CodigoCustodio 
							LEFT JOIN Custodio COI ON COI.CodigoCustodio = TE.CodigoCustodio 
							LEFT JOIN DatosCarta DC ON DC.CodigoDatosCarta = OI.DTC
							LEFT JOIN observacioncarta OBS ON OBS.CodigoAgrupacion = OI.CodigoAgrupado and OBS.Situacion = 'A'
						WHERE (@p_CodigoOrden IS NOT NULL AND OI.CodigoOrden   in 
										(select  campo from [dbo].[fn_SplitLista](@p_CodigoOrden))) 

 ----------------------------------------------------------
-- CodigoOperacion: Compra = 1 | Venta = 2
----------------------------------------------------------

declare @v_tipo int, @v_compra int, @v_venta int, @v_cont int,
@v_cmonto numeric(22, 7), @v_vmonto numeric(22, 7), @v_residuo numeric(22, 7)

	set @v_cont = (select COUNT(*) from @Resultado)

	if(@v_cont = 1)
	begin
		set @v_tipo = (select CodigoOperacion from @Resultado )
	end
	else
	begin 
		select @v_compra=COUNT(*),@v_cmonto=isnull(sum(Importe),0) from @Resultado where CodigoOperacion = 1 
		select @v_venta=COUNT(*),@v_vmonto=isnull(sum(Importe),0) from @Resultado where CodigoOperacion = 2 
	
		if( @v_cmonto > @v_vmonto  ) set @v_tipo = 1 --compra - cargo
		else set @v_tipo = 2   --venta - abono

		update @Resultado set Caso=case when(@v_tipo=1) then 'C' else 'V' end

	end

	select * from @Resultado
	order by Contraparte,Importe,CodigoOperacion DESC

END  
GO

GRANT EXECUTE ON [dbo].[sp_SIT_CompraVentaAcciones] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[sp_SIT_gen_Cartas_Deposito_Renovacion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_gen_Cartas_Deposito_Renovacion') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_gen_Cartas_Deposito_Renovacion]
END 
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Devuelve la estructura para la generacion de cartas renovacion
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 07/12/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9678
--	Descripcion del cambio: Nuevo campo ObservacionCarta
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 03/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9777
--	Descripcion del cambio: Se actualiza la longitud del codigo de operacion a 6
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 21/03/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11169
--	Descripcion del cambio: Redondear los montos a dos decimales
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 27/07/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11450
--	Descripcion del cambio: Corrección de generación de cartas de renovación DPZ. Se cruza con operaciones que tienen el mismo código de orden.
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 02/08/2019
--	Modificado por: Karina Gomez
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Validacion para depositos al exterior
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_gen_Cartas_Deposito_Renovacion] @p_CodigoOperacionCaja VarChar(12) , @p_Constitucion Char(1)
AS
BEGIN
	--El codigo operacion de caja siempre esta en la cabecera por que bloquea que se le asigne tipo de carta a la orden relacionada por 
	--(Constitucion/Cancelacion)
	DECLARE @tblOrdenes AS TABLE (Correlativo INT Identity, CodigoOorden VarChar(10))
	/*OT9777 INICIO */
	DECLARE @CodigoOperacion VarChar(6),@DatoCabecera Char(1),@BancoPublico VarChar(100)
	/*OT9777 FIN */
	DECLARE @CodPaisPeru char(3)='604'
	
	--
	SELECT @BancoPublico = T.Descripcion From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	WHERE Clasificacion = 'BANFONPUB'
	--
	SELECT @CodigoOperacion = CodigoOperacion FROM OrdenInversion WHERE CodigoOrden = @p_CodigoOperacionCaja AND Situacion = 'A' AND Estado <> 'E-ELI'
	--
	--Si necesitamos la constitucion y es constitucion O necesitamos la cancelacion y es cancelacion
	IF (@p_Constitucion = '1' AND @CodigoOperacion = '3') OR (@p_Constitucion = '2' AND @CodigoOperacion = '4')
	BEGIN
		INSERT INTO @tblOrdenes VALUES (@p_CodigoOperacionCaja)
		SET @DatoCabecera = '1'
	END
	ELSE IF EXISTS(SELECT 1 FROM DPZRenovacionCabecera WHERE CodigoOrden = @p_CodigoOperacionCaja AND CodigoOperacion = '3' ) AND @p_Constitucion = '1'
	BEGIN
		INSERT INTO @tblOrdenes VALUES (@p_CodigoOperacionCaja)
		SET @DatoCabecera = '1'
	END
	ELSE IF EXISTS(SELECT 1 FROM DPZRenovacionCabecera WHERE CodigoOrden = @p_CodigoOperacionCaja AND CodigoOperacion = '4' ) AND @p_Constitucion = '1'
	BEGIN
		INSERT INTO @tblOrdenes
		SELECT RD.CodigoOrden FROM DPZRenovacionCabecera RC JOIN DPZRenovacionDetalle RD ON RD.CodigoCabecera = RC.CodigoCabecera
		WHERE RC.CodigoOrden = @p_CodigoOperacionCaja
		SET @DatoCabecera = '2'
	END
	ELSE IF EXISTS(SELECT 1 FROM DPZRenovacionDetalle WHERE CodigoOrden = @p_CodigoOperacionCaja AND CodigoOperacion = '3' ) AND @p_Constitucion = '2'
	BEGIN
		INSERT INTO @tblOrdenes
		SELECT DISTINCT RC.CodigoOrden FROM DPZRenovacionCabecera RC JOIN DPZRenovacionDetalle RD ON RD.CodigoCabecera = RC.CodigoCabecera 
		AND RD.CodigoOrden = @p_CodigoOperacionCaja
		SET @DatoCabecera = '1'
	END
	ELSE IF EXISTS(SELECT 1 FROM DPZRenovacionCabecera WHERE CodigoOrden = @p_CodigoOperacionCaja AND CodigoOperacion = '4' ) AND @p_Constitucion = '2'
	BEGIN
		INSERT INTO @tblOrdenes
		SELECT CodigoCabecera FROM DPZRenovacionDetalle
		WHERE CodigoCabecera  IN (
			SELECT RC.CodigoCabecera FROM DPZRenovacionCabecera RC JOIN DPZRenovacionDetalle RD ON RD.CodigoCabecera = RC.CodigoCabecera 
			AND RD.CodigoOrden = @p_CodigoOperacionCaja AND RC.Situacion = 'A'
		) 
		SET @DatoCabecera = '2'
	END
	ELSE
	BEGIN
		INSERT INTO @tblOrdenes
		SELECT RD.CodigoOrden FROM DPZRenovacionCabecera RC JOIN DPZRenovacionDetalle RD ON RD.CodigoCabecera = RC.CodigoCabecera
		WHERE RC.CodigoOrden = @p_CodigoOperacionCaja
		SET @DatoCabecera = '2'
	END
	--Completamo la consulta con las ordenes seleccionadas	
	SELECT NombreCompletoPortafolio = P.NombreCompleto,FechaDDMMYY = DBO.FN_SIT_OBT_FechaFormateada_DDMMYY(OI.FechaLiquidacion),
	FechaCarta = dbo.RetornarFechaCompleta(OI.FechaLiquidacion),
	Contraparte = case when(OI.CodigoOperacion='3') then TB.Descripcion ELSE T.Descripcion END,
	RUCPortafolio = ISNULL(P.RUCPortafolio,''),
	TituloCarta = ISNULL(MC.Descripcion,''),Moneda = CASE WHEN M.CodigoMoneda = 'DOL' THEN 'USD' ELSE M.Descripcion END,SimboloMoneda = M.Simbolo,
	NumeroCuentaRenovacion =
	CASE WHEN @DatoCabecera = '1' THEN 
		CASE WHEN TR.Descripcion IS NULL THEN
			CASE WHEN CE.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE (CASE WHEN(CEB.EntidadFinanciera='BBH') THEN 'SUBCUENTA ' ELSE 'CCME ' END)  END + ISNULL(CE.NumeroCuenta,'') + ' con CCI ' + ISNULL(CE.NumeroCuentaInterBancario,'')
		ELSE
			CASE WHEN CER.NumeroCuenta IS NULL THEN
				'CUENTA MATRIZ'
			ELSE
				CASE WHEN CER.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE (CASE WHEN(CEB.EntidadFinanciera='BBH') THEN 'SUBCUENTA ' ELSE 'CCME ' END)  END + ISNULL(CER.NumeroCuenta,'') + ' con CCI ' + ISNULL(CER.NumeroCuentaInterBancario,'')
			END
		END
	ELSE '' END,
	TerceroRenovacion = 
	CASE WHEN @DatoCabecera = '1' THEN CASE WHEN TR.Descripcion IS NULL THEN T.Descripcion ELSE TR.Descripcion END ELSE '' END ,Diferencia = 0,
	NombreUsuarioF1 = RTRIM(LTRIM(ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF1),''))),
	NombreUsuarioF2 = RTRIM(LTRIM(ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF2),''))),
	CargoUsuarioF1 = RTRIM(LTRIM(ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF1),''))),
	CargoUsuarioF2 = RTRIM(LTRIM(ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF2),''))),
	OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2,
	CodigoOrdenCancelacion = OI.CodigoOrden, Capital = ROUND(OI.MontoNominalOperacion,2),
	FechaOperacion = CASE WHEN OI.CodigoOperacion = '3' THEN DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion) ELSE 
		DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion)  END,
	FechaVencimiento =  CASE WHEN OI.CodigoOperacion = '3' THEN DBO.FN_SIT_OBT_FechaFormateada(OI.FechaContrato) ELSE 
		DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion) END,
	--Tasa = OI.TasaPorcentaje,
	Tasa = isnull(PO.Tasa,OI.TasaPorcentaje),
	Interes = ROUND(OI.MontoNetoOperacion,2) - ROUND(OI.MontoNominalOperacion,2),Dias = ISNULL(OI.Plazo ,0),
	ValorOperacion =  ROUND(OI.MontoNetoOperacion,2) ,MontoCancelacion = ROUND(OI.MontoNetoOperacion,2), CodigoMoneda = OI.CodigoMoneda ,
	CodigoPortafolio = OI.CodigoPortafolioSBS , CodigoTerceroReno = OI.CodigoTercero ,NombrePortafolio = P.Descripcion,
	BancoPublico = CASE WHEN P.CodigoFondosMutuos = '' THEN '' ELSE @BancoPublico END,
	ObservacionCarta = CASE WHEN OC.ObservacionCarta = '' THEN '' ELSE 'OBS: ' + OC.ObservacionCarta END,
	Origen = case when(T.CodigoPais = @CodPaisPeru) THEN 'NAC' ELSE 'EXT' END,	
	case when(OI.CodigoOperacion='3') then TB.SWIFT ELSE T.SWIFT END SWIFT,
	case when(OI.CodigoOperacion='3') then TB.ABA ELSE T.ABA END ABA,
	case when(OI.CodigoOperacion='3') then TB.BancoDestino ELSE T.BancoDestino END BancoDestino,
	case when(OI.CodigoOperacion='3') then TB.NumeroCuentaDestino ELSE T.NumeroCuentaDestino END NumeroCuentaDestino,
	case when(OI.CodigoOperacion='3') then TB.CiudadDestino ELSE T.CiudadDestino END CiudadDestino,
	case when(OI.CodigoOperacion='3') then TB.PaisDestino ELSE T.PaisDestino END PaisDestino,
	isnull(OC.CodigoTerceroDestino,'')As CodigoTerceroDestino,
	case when(OI.CodigoOperacion='3') then TB.Direccion ELSE T.Direccion END Direccion,
	M.Descripcion As NombreMoneda,
	OC.NumeroCuenta,
	CEB.EntidadFinanciera,
	TB.Descripcion As Entidad,
	PO.TipoTasa
	FROM @tblOrdenes TBLO
	JOIN OrdenInversion OI ON OI.CodigoOrden = TBLO.CodigoOorden
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	LEFT JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo AND MC.CodigoOperacion = OI.CodigoOperacion AND MC.Situacion = 'A'
	JOIN Moneda M ON M.CodigoMoneda = OI.CodigoMoneda
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero
	LEFT JOIN Terceros TR ON TR.CodigoTercero = OC.CodigoTerceroDestino
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	LEFT JOIN Entidad ER ON ER.CodigoTercero = TR.CodigoTercero AND ER.Situacion = 'A'
	LEFT JOIN CuentaEconomica CE ON CE.EntidadFinanciera = E.CodigoEntidad AND CE.CodigoMoneda = OI.CodigoMoneda  AND CE.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	AND CE.Situacion = 'A' AND CE.CodigoClaseCuenta = '20'
	LEFT JOIN CuentaEconomica CER ON CER.EntidadFinanciera = ER.CodigoEntidad AND CER.CodigoMoneda = OI.CodigoMoneda  AND CER.CodigoPortafolioSBS = OI.CodigoPortafolioSBS AND CER.Situacion = 'A' AND CER.CodigoClaseCuenta = '20'
	LEFT JOIN CuentaEconomica CEB ON OC.NumeroCuenta = CEB.NumeroCuenta 
	LEFT JOIN Entidad EB ON EB.CodigoEntidad  = CEB.EntidadFinanciera
	LEFT JOIN Terceros TB ON TB.CodigoTercero = EB.CodigoTercero
	LEFT JOIN PrevOrdenInversion_OI POI ON OI.CodigoOrden = POI.CodigoOrden 
	LEFT JOIN PrevOrdenInversion PO ON PO.CodigoPrevOrden = POI.CodigoPrevOrden 
	WHERE OI.CategoriaInstrumento IN ('DP')

END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_gen_Cartas_Deposito_Renovacion] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[sp_SIT_ImpresionCarta_Constitucion_CancelacionDPZ]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_ImpresionCarta_Constitucion_CancelacionDPZ') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_ImpresionCarta_Constitucion_CancelacionDPZ]
END 
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Selecciona la estructura para la creacion de cartas tipo Constitucion/Cancelacion DPZ
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 07/12/2016
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9678
-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 24/03/2017
-- Modificado por: Carlos Espejo
-- Nro. Orden de Trabajo: 10150
-- Descripcion del cambio: Se agrega la cuenta economica de la contraparte y su numero de RUC
----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/03/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10200
--	Descripcion del cambio: Se regulariza el pase
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 24/05/2016
--	Modificado por: Magno Sanchez
--	Nro. Orden de Trabajo: 10412
--	Descripcion del cambio: Se agrego la columna TipoObservacionCarta
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 13/03/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11192
--	Descripcion del cambio: Filtrar las cuentas económicas activas
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 02/08/2019
--	Modificado por: Karina Gomez
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Adaptar la carta para los nuevos casos del proyecto mandatos
-----------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_ImpresionCarta_Constitucion_CancelacionDPZ]  
@p_CodigoOperacionCaja varchar(7) = ''
AS
BEGIN
	DECLARE @BancoPublico VarChar(100)
	SELECT @BancoPublico = T.Descripcion From ParametrosGenerales pg
	JOIN Terceros T ON T.CodigoTercero = PG.Valor
	WHERE Clasificacion = 'BANFONPUB'

	DECLARE @CodPaisPeru char(3)='604'
	--
	SELECT NombrePortafolio = P.Descripcion,FechaDDMMYY = DBO.FN_SIT_OBT_FechaFormateada_DDMMYY(OC.FechaOperacion),
	NombreCompletoPortafolio = P.NombreCompleto,FechaCarta = dbo.RetornarFechaCompleta(OC.FechaOperacion),
	Contraparte = T.Descripcion,TituloCarta = MC.Descripcion,Moneda = CASE WHEN M.CodigoMoneda = 'DOL' THEN 'USD' ELSE M.Descripcion END,
	NumeroCuentaContraparte = CASE WHEN CET.NumeroCuenta IS NULL THEN
		'CUENTA MATRIZ'
	ELSE
		CASE WHEN CET.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END + ISNULL(CET.NumeroCuenta,'') + ' con CCI ' + ISNULL(CET.NumeroCuentaInterBancario,'')
	END, --10150
	RUCContraparte = T.CodigoDocumento, --10150
	Capital = OI.MontoNominalOperacion,
	FechaOperacion = CASE WHEN OI.CodigoOperacion = '3' THEN DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion) ELSE DBO.FN_SIT_OBT_FechaFormateada(OI.FechaContrato) END ,
	FechaVencimiento = CASE WHEN OI.CodigoOperacion = '3' THEN DBO.FN_SIT_OBT_FechaFormateada(OI.FechaContrato)ELSE DBO.FN_SIT_OBT_FechaFormateada(OI.FechaOperacion) END,
	--Tasa = OI.TasaPorcentaje,
	Tasa = isnull(PO.Tasa,OI.TasaPorcentaje),
	Interes = OI.MontoOperacion - OI.MontoNominalOperacion ,Dias = ISNULL(OI.Plazo ,0),ValorOperacion =  OI.MontoOperacion,SimboloMoneda = M.Simbolo ,
	NumeroCuenta =
	CASE WHEN TR.Descripcion IS NULL THEN
		CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END +  ISNULL(CE.NumeroCuenta,'') + ' con CCI ' + ISNULL(CE.NumeroCuentaInterBancario,'')
	ELSE
		CASE WHEN CER.NumeroCuenta IS NULL THEN
			'CUENTA MATRIZ'
		ELSE
			CASE WHEN CER.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END + ISNULL(CER.NumeroCuenta,'') + ' con CCI ' + ISNULL(CER.NumeroCuentaInterBancario,'')
		END
	END,
	Firma1 = ISNULL(AC1.Firma,'') , Firma2 = ISNULL(AC2.Firma,'') , 
	NombreUsuarioF1 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF1),''),
	NombreUsuarioF2 = ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF2),''),
	CargoUsuarioF1 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF1),'') ,CargoUsuarioF2 = ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF2),''),
	RUCPortafolio = ISNULL(P.RUCPortafolio,''), 
	NombreBanco = CASE WHEN TR.Descripcion IS NULL THEN TB.Descripcion ELSE TR.Descripcion END,
	OC.VBGERF1 CodigoUsuarioF1 ,OC.VBGERF2  CodigoUsuarioF2,
	BancoPublico = CASE WHEN P.CodigoFondosMutuos = '' THEN '' ELSE @BancoPublico END,
	oi.CodigoOperacion,
	ObservacionCarta = CASE WHEN OC.ObservacionCarta = '' THEN '' 
							WHEN OC.ObservacionCarta != '' AND ((OC.CodigoTerceroDestino IS NULL) OR (OC.CodigoTerceroDestino = '')) AND OC.CodigoOperacion = '4' THEN OC.ObservacionCarta
							ELSE 'OBS: ' + OC.ObservacionCarta END,
	
	TipoObservacionCarta = CASE WHEN ((OC.CodigoTerceroDestino IS NULL) OR (OC.CodigoTerceroDestino = '')) AND OC.CodigoOperacion = '4' THEN '1'
							WHEN ((OC.CodigoTerceroDestino IS NOT NULL) OR (OC.CodigoTerceroDestino != '')) AND (OC.CodigoOperacion = '4') THEN '0' 
							WHEN (OC.CodigoOperacion = '3') THEN '2' END, --10412
	Origen = case when(T.CodigoPais = @CodPaisPeru) THEN 'NAC' ELSE 'EXT' END,
	case when(oi.CodigoOperacion='4') then TB.SWIFT ELSE T.SWIFT END SWIFT,
	case when(oi.CodigoOperacion='4') then TB.ABA ELSE T.ABA END ABA,
	case when(oi.CodigoOperacion='4') then TB.BancoDestino ELSE T.BancoDestino END BancoDestino,
	case when(oi.CodigoOperacion='4') then TB.NumeroCuentaDestino ELSE T.NumeroCuentaDestino END NumeroCuentaDestino,
	case when(oi.CodigoOperacion='4') then TB.CiudadDestino ELSE T.CiudadDestino END CiudadDestino,
	case when(oi.CodigoOperacion='4') then TB.PaisDestino ELSE T.PaisDestino END PaisDestino,
	isnull(OC.CodigoTerceroDestino,'')As CodigoTerceroDestino,
	case when(oi.CodigoOperacion='4') then TB.Direccion ELSE T.Direccion END Direccion,
	M.Descripcion As NombreMoneda,
	OC.NumeroCuenta,
	CE.EntidadFinanciera,
	TB.Descripcion As Entidad,
	PO.TipoTasa
	FROM OperacionesCaja OC 
	JOIN OrdenInversion OI ON OI.CodigoOrden = OC.CodigoOrden AND OI.CodigoPortafolioSBS = OC.CodigoPortafolioSBS AND OI.Situacion = 'A'
	JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
	JOIN Entidad  E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	LEFT JOIN CuentaEconomica CET ON CET.EntidadFinanciera = E.CodigoEntidad AND CET.CodigoPortafolioSBS = OC.CodigoPortafolioSBS AND CET.CodigoClaseCuenta = '20'
		AND CET.CodigoMoneda = OC.CodigoMoneda AND CET.Situacion = 'A'
	JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo and mc.CodigoOperacion = oc.CodigoOperacion 
	JOIN Moneda M ON M.CodigoMoneda = OC.CodigoMoneda
	JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta AND CE.Situacion = 'A' AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS
	JOIN Entidad EB ON EB.CodigoEntidad = CE.EntidadFinanciera AND EB.Situacion = 'A'
	JOIN Terceros TB ON TB.CodigoTercero = EB.CodigoTercero 
	LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = OC.VBGERF1
	LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = OC.VBGERF2
	LEFT JOIN Terceros TR ON TR.CodigoTercero = OC.CodigoTerceroDestino
	LEFT JOIN Entidad ER ON ER.CodigoTercero = TR.CodigoTercero AND ER.Situacion = 'A'
	LEFT JOIN CuentaEconomica CER ON CER.EntidadFinanciera = ER.CodigoEntidad AND CER.CodigoMoneda = OI.CodigoMoneda  AND CER.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	AND CER.Situacion = 'A' AND CER.CodigoClaseCuenta = '20'
	LEFT JOIN PrevOrdenInversion_OI POI ON OI.CodigoOrden = POI.CodigoOrden 
	LEFT JOIN PrevOrdenInversion PO ON PO.CodigoPrevOrden = POI.CodigoPrevOrden 
	WHERE OC.CodigoOperacionCaja = @p_CodigoOperacionCaja
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ImpresionCarta_Constitucion_CancelacionDPZ] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[sp_SIT_ListaOperacionesReporte]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_ListaOperacionesReporte') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_ListaOperacionesReporte]
END 
GO

CREATE PROCEDURE [dbo].[sp_SIT_ListaOperacionesReporte] 
@p_CodigoOrden VARCHAR(500)
as
BEGIN
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 07/06/2019
--	Creado por: Karina Gomez
--	Nro. Orden de Trabajo: 12163
--  Descripción:  Lista los Datos para la carta de Operaciones de Reporte
-----------------------------------------------------------------------------------------------------------

declare @Situacion char(1) = 'A',
	    @BancoPublico VarChar(100)

		
SELECT @BancoPublico = T.Descripcion From ParametrosGenerales pg
JOIN Terceros T ON T.CodigoTercero = PG.Valor
WHERE Clasificacion = 'BANFONPUB'

		SELECT 
			   @BancoPublico BancoPublico,	
			   CONVERT(NUMERIC(27,7),Importe) AS Monto,
			   OI.CantidadOperacion,
			   OI.CodigoMnemonico AS Principal,
			   dbo.FormatDate(case when(OC.CodigoOperacion = '101') THEN OI.FechaLiquidacion ELSE OI.FechaContrato END)  AS FechaInicio,
			   dbo.FormatDate(case when(OC.CodigoOperacion = '101') THEN  OI.FechaContrato ELSE OI.FechaLiquidacion END) AS FechaVencimiento,
			   OI.Plazo,
			   OI.CodigoOperacion,
			   NumeroCarta,
			   CodigoTerceroOrigen,
			   T.Descripcion Tercero,
			   P.NombreCompleto Portafolio,
			   OC.NumeroCuenta,
			   CE.NumeroCuentaInterBancario,
			   ObservacionCarta = CASE WHEN OC.ObservacionCarta = '' THEN '' 
									   WHEN OC.ObservacionCarta != '' AND ((OC.CodigoTerceroDestino IS NULL) OR (OC.CodigoTerceroDestino = '')) AND OC.CodigoOperacion = '4' THEN OC.ObservacionCarta
									   ELSE 'OBS: ' + OC.ObservacionCarta END,
			   TipoObservacionCarta = ISNULL(CASE WHEN ((OC.CodigoTerceroDestino IS NULL) OR (OC.CodigoTerceroDestino = '')) AND OC.CodigoOperacion = '4' THEN '1'
												  WHEN ((OC.CodigoTerceroDestino IS NOT NULL) OR (OC.CodigoTerceroDestino != '')) AND (OC.CodigoOperacion = '4') THEN '0' 
												  WHEN (OC.CodigoOperacion = '3') THEN '2' END, ''),
			   Firma1 = ISNULL(AC1.Firma,''), 
			   Firma2 = ISNULL(AC2.Firma,''),
			   CASE WHEN T.Descripcion IS NULL OR CE.NumeroCuenta IS NOT NULL THEN
							CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCME ' END + ISNULL(CE.NumeroCuenta,'') 
			        WHEN CE.NumeroCuenta IS NULL THEN
							'CUENTA MATRIZ'
				END NumeroCuentaCompleto,
			   CASE WHEN T.Descripcion IS NULL OR CE.NumeroCuentaInterBancario IS NOT NULL THEN
							CASE WHEN OC.CodigoMoneda = 'NSOL' THEN 'CCMN ' ELSE 'CCI ' END + ISNULL(CE.NumeroCuentaInterBancario,'') 
			        WHEN CE.NumeroCuentaInterBancario IS NULL THEN
							'CUENTA MATRIZ'
				END NumeroInterbancarioCompleto,
				dbo.RetornarFechaCompleta(OI.FechaLiquidacion) FechaLiquidacionCompleto,
			    dbo.RetornarFechaCompleta(OC.FechaOperacion) FechaOperacionCompleto,
				isnull(DC.Valor,'') as DTC,
				OC.CodigoMoneda,
				case when (MN.SinonimoISO='USD') THEN  MN.Simbolo+'.'  ELSE MN.SinonimoISO END AS Simbolo,
				cast(0 as decimal(22,7)) MontoDiferencia,
				'NINGUNO' Tipo,
				CT.NumeroCuenta as NumeroCuentaTercero,
				dbo.FormatDate(OC.FechaOperacion)  AS FechaLiquidacion,
				NombreUsuarioF1 = RTRIM(LTRIM(ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF1),''))),
				NombreUsuarioF2 = RTRIM(LTRIM(ISNULL(DBO.RetornarNombrePersonal(OC.VBGERF2),''))),
				CargoUsuarioF1 = RTRIM(LTRIM(ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF1),''))),
				CargoUsuarioF2 = RTRIM(LTRIM(ISNULL(dbo.RetornarCargoPersonal(OC.VBGERF2),''))),
				CodigoUsuarioF1 = ISNULL(OC.VBGERF1,''),
				CodigoUsuarioF2 = ISNULL(OC.VBGERF2,''),
				P.Descripcion AS NombreCortoPortafolio 
		into #Resultado
		FROM OperacionesCaja OC 
		JOIN OrdenInversion OI ON OC.CodigoOrden = OI.CodigoOrden
		JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS
		JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta
		LEFT JOIN AprobadorCarta AC1 ON AC1.CodigoInterno = OC.VBGERF1
		LEFT JOIN AprobadorCarta AC2 ON AC2.CodigoInterno = OC.VBGERF2
		JOIN Moneda MN ON MN.CodigoMoneda = OC.CodigoMoneda
		LEFT JOIN CuentaTerceros CT ON CT.CodigoTercero = OC.CodigoTerceroOrigen AND CT.LiquidacionAutomatica = 'N' AND CT.CodigoMoneda = OC.CodigoMoneda
		LEFT JOIN DatosCarta DC ON DC.CodigoDatosCarta = OI.DTC
		WHERE (OC.CodigoOrden in (select  campo from [dbo].[fn_SplitLista](@p_CodigoOrden)) ) AND
			  (OC.Situacion = @Situacion)

			  
		declare @v_monto1 numeric(22,7), @v_monto2 numeric(22,7),@v_total numeric(27,6) 
		
		set @v_monto1 =(select sum(Monto) from #Resultado where CodigoOperacion ='101') --apertura --
		set @v_monto2 =(select sum(Monto) from #Resultado where CodigoOperacion ='4') --vencimiento  --
	
		set @v_total=(@v_monto2-@v_monto1)
		declare @tipo varchar(10)
			
		if(@v_total IS NOT NULL)
		BEGIN
		if(@v_monto1>@v_monto2)
		BEGIN
			 set @tipo='CARGO'
		END
		ELSE IF(@v_monto1<@v_monto2)
		BEGIN
			 SET  @tipo='ABONO'
		END

		update #Resultado set MontoDiferencia = (case when(@v_total<0) then (@v_total*-1) else @v_total end),
							  Tipo= @tipo 
		END 

		SELECT * FROM #Resultado

		DROP TABLE #Resultado 
			
			
END 
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ListaOperacionesReporte] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[sp_SIT_ReporteLimites_New]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_ReporteLimites_New') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_ReporteLimites_New]
END 
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se modificaron las cabeceras del reporte y se agrego la columna porcentaje minimo
--  Test: exec sp_SIT_ReporteLimites_New @p_FechaProceso=20190603,@p_FechaCadena=N'03/06/2019',@p_CodigoPortafolioSBS=N'2666'
--------------------------------------------------------------------------------------------------------------------------- 
--	Fecha Modificacion: 02/07/2019
--	Modificado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se modifico el reporte para el limite clase activo y la logica de saldos no administrados
--------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[sp_SIT_ReporteLimites_New]

	@p_FechaProceso NUMERIC(8),  
	@p_FechaCadena VARCHAR(20)='', 
	@p_CodigoPortafolioSBS VARCHAR(100) = '',
	@p_ClienteMandato CHAR(1)
----------------------------------------------------------------------------------------------------------------------------------------------
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @tPortafolios AS TABLE(
		Id INT IDENTITY,
		CodigoPortafolio VARCHAR(20)
	);

	DECLARE @tReporteLimite AS TABLE (
		Fecha varchar(20),
		CodigoPortafolioSBS VARCHAR(5),
		NombrePortafolio VARCHAR(50),
		Moneda  VARCHAR(100),
		CodigoLimite VARCHAR(10),
		NombreLimite VARCHAR(100),
		Cuadrar INT,
		Estado VARCHAR(100),
		TotalPosicion NUMERIC(29,6),
		TotalParticipacion NUMERIC(29,6),
		Codigo VARCHAR(20),
		Descripcion VARCHAR(100), 
		ValorBase NUMERIC(22,7),
		Posicion NUMERIC(22,7),
		Participacion NUMERIC(22,7),
		ValorPorcentajeMinimo NUMERIC(22,7),
		MargenMinimo NUMERIC(22,7),
		ValorPorcentajeMaximo NUMERIC(22,7),
		MargenMaximo NUMERIC(22,7),
		Alerta VARCHAR(100),
		ValorNivelCM VARCHAR(50),
		ValorBaseCM NUMERIC(22,7),
		CodigoLimiteCM VARCHAR(20),
		ValorPorcentajeCM NUMERIC(22,7),
		PosicionCM  NUMERIC(22,7),
		MargenCM NUMERIC(22,7),
		CodigoTerceroLimite VARCHAR(20),
		--TipoCuenta CHAR(1),
		CodigoMoneda VARCHAR(10),
		ValorNivel VARCHAR(100),
		CodigoCaracteristicaRelacion VARCHAR(5),
		CodigoRelacion VARCHAR(5),
		CabeceraLimite varchar(1)
	)

	DECLARE @TotalPortafolio INT=0
	DECLARE @CONT INT= 1


	INSERT INTO @tPortafolios
	SELECT CAMPO FROM DBO.fn_SplitLista(@p_CodigoPortafolioSBS) 

	SET @TotalPortafolio=(SELECT COUNT(*) FROM @tPortafolios)


	--OT 9733 Inicio
	SELECT p.CodigoPortafolioSBS, SUM(ISNULL(VPNMonedaFondo,0)) Total
	INTO #TotalCartera
	FROM Portafolio p WITH(NOLOCK) INNER JOIN CarteraTituloValoracion c WITH(NOLOCK) ON p.CodigoPortafolioSBS = c.CodigoPortafolioSBS
	WHERE p.Situacion = 'A' AND c.FechaValoracion = @p_FechaProceso AND c.Escenario = 'REAL'
	GROUP BY p.CodigoPortafolioSBS
	--OT 9733 Fin

	if(@TotalPortafolio = 0) 	
	BEGIN
		INSERT INTO @tPortafolios VALUES('')
		SET @TotalPortafolio=(SELECT COUNT(*) FROM @tPortafolios)
	END
	
	WHILE(@CONT <= @TotalPortafolio)
	BEGIN 

	set @p_CodigoPortafolioSBS = (select CodigoPortafolio from @tPortafolios where Id = @CONT)
	set @CONT = @CONT +1 

	SELECT r.CodigoReporte, r.CodigoLimite, r.CodigoLimiteCaracteristica, r.FechaReporte, r.Secuencial, r.CodigoNivelLimite, r.Tipo, r.Tope,
		r.ValorPorcentaje, r.Posicion, r.Participacion, r.Margen, r.Alerta, r.ValorNivel, r.DescripcionNivel, r.PorVal, r.Patrimonio, r.ValorBase,
		r.Cxc, r.Cxp, r.TotalInversiones, r.SaldoBanco, r.Factor, r.ValorEfectivoColocado, r.FloatOficioMultiple, r.UnidadesEmitidas, r.TotalActivo,
		r.TotalPasivo, r.PosicionF1, r.PosicionF2, r.PosicionF3, r.FechaVencimiento, r.FechaOperacion, r.CodigoMoneda, r.Escenario, r.NivelSaldoBanco,
		r.ValorPorcentajeM, r.Valor1, r.Valor2, r.Valor3, r.ValorPorcentajeMinimo, r.MargenMinimo, lc.CodigoPortafolioSBS, r.CodigoCaracteristicaRelacion, r.CodigoRelacion
	INTO #ReporteLimites
	FROM ReporteLimites r WITH(NOLOCK) INNER JOIN LimiteCaracteristica lc WITH(NOLOCK) ON r.CodigoLimiteCaracteristica = lc.CodigoLimiteCaracteristica AND lc.Situacion = 'A'
	WHERE FechaReporte = @p_FechaProceso AND lc.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS = '' THEN lc.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END

	SELECT 
			CodigoLimite,
			CodigoLimiteCaracteristica
	INTO	#RerpoteLimitesCabeceras
	FROM	#ReporteLimites 
	WHERE	Secuencial = 1

	Declare @CodigoLimite varchar(10), @CodigoLimiteCaracteristica varchar(10), @secuencialMax int, @LimiteNivelMin numeric(22,7), @LimiteNivelMax numeric(22,7)

	DECLARE cur_Nivel CURSOR FOR
		SELECT CodigoLimite,CodigoLimiteCaracteristica FROM #RerpoteLimitesCabeceras
	OPEN cur_Nivel
		FETCH NEXT FROM cur_Nivel INTO @CodigoLimite,@CodigoLimiteCaracteristica
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			SELECT @secuencialMax = MAX(Secuencial) FROM NivelLimite WHERE CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica 

			SELECT @LimiteNivelMin = LimiteNivelMin, @LimiteNivelMax = LimiteNivelMax FROM NivelLimite WHERE CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica AND Secuencial = @secuencialMax

			UPDATE	#ReporteLimites
			SET		ValorPorcentaje = @LimiteNivelMax,
					ValorPorcentajeM = @LimiteNivelMin
			WHERE	CodigoLimite = @CodigoLimite
			AND		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
			AND		Secuencial = 1

			FETCH NEXT FROM cur_Nivel INTO @CodigoLimite,@CodigoLimiteCaracteristica
		END
	CLOSE cur_Nivel 
	DEALLOCATE cur_Nivel

		SELECT a.CodigoPortafolioSBS, ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite) Grupo,
			--Validacion = ''
			Validacion = CASE
						WHEN b.Cuadrar = 1 AND (f.TipoRenta IS NULL OR f.TipoRenta = e.IdTipoRenta_FK) THEN CASE --Limites 01, 02, 03, 04, 06, 10, 14
													--OT 9733 Inicio
													WHEN (ROUND(SUM(d.Participacion), 2) = 100 AND ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite) <> '10') OR (ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite) = '10' AND ROUND((SELECT Total FROM #TotalCartera WHERE CodigoPortafolioSBS = a.CodigoPortafolioSBS), 1, 1) = ROUND(SUM(d.Posicion), 1, 1)) THEN 0
													--OT 9733 Fin
													ELSE 1
												END
						WHEN b.Cuadrar = 2 THEN CASE --Limites 15, 16
													WHEN SUM(CASE
																WHEN d.ValorBase = 0 THEN 1
																ELSE 0
															END) > 1 THEN 1
													ELSE 0
												END
						WHEN b.Cuadrar = 3 THEN CASE --Limites 17
													WHEN SUM(CASE
																WHEN LEFT(LTRIM(RTRIM(ISNULL(DescripcionNivel, ''))), 11) = 'SIN ASIGNAR' THEN 1
																ELSE 0
															END) > 0 THEN 1
													WHEN SUM(CASE
																WHEN d.ValorBase = 0 THEN 1
																ELSE 0
															END) > 1 THEN 1
													ELSE SUM(CASE
																WHEN ValorPorcentaje IS NULL THEN 1
																ELSE 0
															END)
													END
						ELSE 0 --Limites 05, 07, 08, 09
						END
		INTO #Limites
		FROM LimiteCaracteristica a WITH(NOLOCK) INNER JOIN Limite b WITH(NOLOCK) ON a.CodigoLimite = b.CodigoLimite
									INNER JOIN Portafolio e WITH(NOLOCK) ON a.CodigoPortafolioSBS = e.CodigoPortafolioSBS
									LEFT JOIN Grupo_Limite c WITH(NOLOCK) ON b.CodigoLimite = c.CodigoLimite AND c.TipoRenta = ''
									LEFT JOIN Grupo_Limite f WITH(NOLOCK) ON b.CodigoLimite = f.CodigoLimite AND f.TipoRenta <> ''
									LEFT JOIN #ReporteLimites d ON a.CodigoLimiteCaracteristica = d.CodigoLimiteCaracteristica
		WHERE a.Situacion = 'A' AND b.Situacion = 'A' AND a.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS = '' THEN a.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END
		GROUP BY a.CodigoPortafolioSBS, ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite), b.Cuadrar, e.IdTipoRenta_FK, f.TipoRenta


	SELECT a.CodigoPortafolioSBS, b.CodigoLimite, b.NombreLimite, (SELECT Validacion FROM #Limites WHERE CodigoPortafolioSBS = a.CodigoPortafolioSBS AND Grupo = ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite)) Validacion,
		b.Cuadrar, ROUND(SUM(ISNULL(r.Posicion, 0)), 2) TotalPosicion, ROUND(SUM(ISNULL(r.Participacion, 0)), 2) TotalParticipacion
	INTO #Validacion
	FROM LimiteCaracteristica a WITH(NOLOCK) INNER JOIN Limite b WITH(NOLOCK) ON a.CodigoLimite = b.CodigoLimite
								LEFT JOIN Grupo_Limite c WITH(NOLOCK) ON b.CodigoLimite = c.CodigoLimite AND c.TipoRenta = ''
								LEFT JOIN #ReporteLimites r WITH(NOLOCK) ON a.CodigoLimite = r.CodigoLimite AND a.CodigoLimiteCaracteristica = r.CodigoLimiteCaracteristica
	WHERE a.Situacion = 'A' AND b.Situacion = 'A' AND a.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS = '' THEN a.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END
	GROUP BY a.CodigoPortafolioSBS, b.CodigoLimite, b.NombreLimite, ISNULL('G' + CONVERT(varchar, c.Grupo), b.CodigoLimite), b.Cuadrar

	DECLARE @valorCartera_Inv NUMERIC(22,7)
	SET @valorCartera_Inv = dbo.ObtenerValorizacionPorPortafolio(@p_CodigoPortafolioSBS,@p_FechaProceso)
	
	DECLARE @valorCartera_Mon NUMERIC(22,7)
	SET @valorCartera_Mon = dbo.ObtenerTotalCarteraPorMoneda(@p_CodigoPortafolioSBS,@p_FechaProceso)
	
	INSERT INTO @tReporteLimite
	 (
		Fecha,
		CodigoPortafolioSBS,
		NombrePortafolio,
		Moneda,
		CodigoLimite,
		NombreLimite,
		Cuadrar,
		Estado,
		TotalPosicion,
		TotalParticipacion,
		Codigo,
		Descripcion, 
		ValorBase,
		Posicion,
		Participacion,
		ValorPorcentajeMinimo,
		MargenMinimo,
		ValorPorcentajeMaximo,
		MargenMaximo,
		Alerta,
		ValorNivelCM,
		ValorBaseCM,
		CodigoLimiteCM,
		ValorPorcentajeCM,
		PosicionCM,
		MargenCM,
		CodigoTerceroLimite ,
		--TipoCuenta,
		CodigoMoneda,
		ValorNivel,
		CodigoCaracteristicaRelacion,
		CodigoRelacion,
		CabeceraLimite
	)
	SELECT 
				@p_FechaCadena Fecha, 
				r.CodigoPortafolioSBS, 
				p.Descripcion NombrePortafolio, 
				'MONEDA: ' + m.Descripcion Moneda, 
				r.CodigoLimite, 
				v.NombreLimite,
				v.Cuadrar,
				Estado = dbo.ValidarLimite(r.CodigoLimite,@p_CodigoPortafolioSBS,@p_FechaProceso,v.TotalPosicion),
				CONVERT(NUMERIC(29,6),isnull(v.TotalPosicion,0)) as TotalPosicion, 
				CONVERT(NUMERIC(29,6),isnull(v.TotalParticipacion,0)) as TotalParticipacion,
				CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.ValorNivel, '')) END Codigo,
				CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END Descripcion,
				CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.ValorBase, 0) END ValorBase,
				CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.Posicion, 0) END Posicion,
				CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.Participacion, 0) END Participacion,
				--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.ValorPorcentajeMinimo, 0) END ValorPorcentajeMinimo,
				ISNULL(r.ValorPorcentajeM, 0) ValorPorcentajeMinimo, /*ValorPorcentajeM guarda el valor del minimo*/
				CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.MargenMinimo, 0) END MargenMinimo,
				ISNULL(r.ValorPorcentaje, 0) ValorPorcentajeMaximo,
				CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE CASE WHEN r.CodigoLimite = '07' THEN ISNULL(r.ValorPorcentaje, 0) - ISNULL(r.Posicion, 0) ELSE ISNULL(r.Margen, 0) END END MargenMaximo,
				CASE WHEN r.ValorNivel IS NULL THEN '' ELSE CASE WHEN ISNULL(r.ValorBase, 0) > 0 AND ISNULL(r.Posicion, 0) > 0 AND ISNULL(r.ValorPorcentaje, 0) > 0 THEN CASE WHEN ROUND(ISNULL(r.Margen, 0), 2) < 0 OR (r.CodigoLimite = '07' AND ROUND((ISNULL(r.ValorPorcentaje, 0) - ISNULL(r.Posicion, 0)), 2) < 0) THEN 'EXCESO LIMITE' ELSE '' END ELSE '' END END Alerta,
				r.ValorNivel as ValorNivelCM,
				r.ValorBase as ValorBaseCM,
				r.CodigoLimite as CodigoLimiteCM,
				r.ValorPorcentaje as ValorPorcentajeCM,
				r.Posicion AS PosicionCM,
				r.Margen AS MargenCM,
				ISNULL(E.CodigoTercero,r.ValorNivel) AS CodigoTerceroLimite,
				--VLD.CodigoValidador TipoCuenta,
				m.CodigoMoneda,
				r.ValorNivel,
				r.CodigoCaracteristicaRelacion,
				r.CodigoRelacion,
				CASE WHEN r.DescripcionNivel = 'SUMA VALORES DE LOS GRUPOS' THEN 'S' ELSE 'N' END
	FROM		#ReporteLimites r 
	INNER JOIN	#Validacion v ON r.CodigoPortafolioSBS = v.CodigoPortafolioSBS AND r.CodigoLimite = v.CodigoLimite
	INNER JOIN	Portafolio p WITH(NOLOCK) ON r.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	INNER JOIN	Moneda m WITH(NOLOCK) ON p.CodigoMoneda = m.CodigoMoneda
	LEFT JOIN	Entidad E ON E.CodigoEntidad = r.ValorNivel
	LEFT JOIN	Limite L  ON L.CodigoLimite = R.CodigoLimite
	--LEFT JOIN ValidadorLimiteDetalle VLD ON VLD.CodigoLimite = L.CodigoLimite and VLD.Tipo='2'
	WHERE		r.CodigoLimiteCaracteristica NOT IN (SELECT z.CodigoLimiteCaracteristica
												FROM (SELECT x.CodigoLimiteCaracteristica,  COUNT(1) Cantidad
														FROM #ReporteLimites x INNER JOIN #Validacion y ON x.CodigoPortafolioSBS = y.CodigoPortafolioSBS AND x.CodigoLimite = y.CodigoLimite
														WHERE y.Validacion = 0
														GROUP BY x.CodigoLimiteCaracteristica) z
												WHERE z.Cantidad = 1)
	ORDER BY	p.Descripcion, 
				r.CodigoLimite, 
				CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END

	DROP TABLE #ReporteLimites
	DROP TABLE #RerpoteLimitesCabeceras
	DROP TABLE #Limites
	DROP TABLE #Validacion

	END
	

	select * into #DetalleNiveles 
	from  @tReporteLimite RL 
	where (CodigoCaracteristicaRelacion is not null) AND
			(CodigoRelacion is not null)

	
	select ROW_NUMBER() OVER(ORDER BY CodigoCaracteristicaRelacion) Id, CodigoCaracteristicaRelacion, CodigoRelacion
	INTO #Niveles
	from (  select DISTINCT CodigoCaracteristicaRelacion,CodigoRelacion
			from  #DetalleNiveles RL 
			where (CodigoCaracteristicaRelacion is not null) AND
				  (CodigoRelacion is not null)
	) as TRelacion where (CodigoCaracteristicaRelacion !='' and CodigoRelacion !='')
	
	declare @total int=0, @contNivel int=1, @CodigoCaracteristicaRelacion varchar(5), @sql varchar(max),
			@nombreColumna varchar(50),@NombreVista varchar(100),@CodigoRelacion varchar(5), @v_codigoLimite varchar(5),@letra char(2),@TotalPosicionDetalle NUMERIC(29,6)
	
	DECLARE @Abecedario varchar(max)='A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z'
	
	SELECT ROW_NUMBER() OVER(ORDER BY SPLITDATA) as Id, SPLITDATA as Letra 
	INTO #Abecedario FROM DBO.fnSplitString(@Abecedario,',')
	
	--camiar
	SELECT DISTINCT ABC.Letra,RL.Descripcion,RL.CodigoRelacion,RL.VALORNIVEL,ROW_NUMBER() OVER(PARTITION BY RL.CODIGORELACION ORDER BY RL.CODIGORELACION,RL.VALORNIVEL, RL.Descripcion ) as Contador 
	INTO #NivelCabecera
	from  @tReporteLimite RL 
	JOIN #Abecedario ABC ON RL.CodigoRelacion = ABC.Id 
	where (CodigoCaracteristicaRelacion is not null) AND (CodigoRelacion is not null)
	GROUP BY ABC.Letra,RL.Descripcion,RL.CODIGORELACION,RL.VALORNIVEL
	
	UPDATE RL SET RL.Descripcion = CHAR(32)+CHAR(32)+ NC.Letra+'.'+Convert(Varchar,NC.Contador)+') '+NC.Descripcion
	FROM @tReporteLimite RL 
	JOIN  #NivelCabecera NC ON RL.ValorNivel = NC.ValorNivel AND 
							   RL.CodigoRelacion = NC.CodigoRelacion 

	--UPDATE TABLA SET TABLA.Descripcion =CHAR(32)+CHAR(32)+ TABLA.Letra+'.'+Convert(Varchar,Contador)+') '+Descripcion
	--FROM (
	--SELECT ABC.Letra,RL.Descripcion,ROW_NUMBER() OVER(PARTITION BY RL.CODIGORELACION ORDER BY RL.CODIGORELACION,RL.CODIGO, RL.DESCRIPCION ) as Contador 
	--from  @tReporteLimite RL 
	--JOIN #Abecedario ABC ON RL.CodigoRelacion = ABC.Id 
	--where (CodigoCaracteristicaRelacion is not null) AND (CodigoRelacion is not null)
	--) AS TABLA 


	set @total = (select count(*)from #Niveles)
	
	WHILE(@contNivel <= @total)
	begin
		select @CodigoCaracteristicaRelacion = CodigoCaracteristicaRelacion,
		       @CodigoRelacion = CodigoRelacion
		from #Niveles where Id = @contNivel
		
		select @nombreColumna=NombreColumna,@NombreVista=NombreVista 
		from CaracteristicaGrupo 
		where CodigoCaracteristica=@CodigoCaracteristicaRelacion

		select @letra=Letra+')' from #Abecedario where Id = @contNivel 
		
		select @TotalPosicionDetalle=sum(Posicion) from #DetalleNiveles  where CodigoRelacion = @CodigoRelacion 

		set @sql='select DISTINCT Fecha,CodigoPortafolioSBS,NombrePortafolio,Moneda,CodigoLimite,NombreLimite,TotalPosicion,'''+@letra+' ''+T.Descripcion,''0'','+CONVERT(VARCHAR,@TotalPosicionDetalle)+','''+@CodigoRelacion+''',0 
				  from '+@NombreVista+' T 
				  join #DetalleNiveles N on N.CodigoRelacion = T.Codigo  
				 where T.Codigo='''+@CodigoRelacion+''''
		print @sql
		insert @tReporteLimite (Fecha,CodigoPortafolioSBS,NombrePortafolio,Moneda,CodigoLimite,NombreLimite,TotalPosicion,Descripcion,Codigo,Posicion,CodigoRelacion,ValorNivel)
		exec(@sql)

		set @contNivel= @contNivel+1
	end

	IF(@p_ClienteMandato = 'S' )
		BEGIN
			
			DECLARE @CODIGOCLIENTEMANDATO VARCHAR(20), @CLIENTEMANDATO VARCHAR(100)
			SELECT DISTINCT @CODIGOCLIENTEMANDATO=T.CodigoTercero,@CLIENTEMANDATO=T.Descripcion
			FROM @tPortafolios P 
			JOIN Portafolio PF ON P.CodigoPortafolio = PF.CodigoPortafolioSBS 
			JOIN Terceros T ON T.CodigoTercero = PF.CodigoTerceroCliente 

			-- Cabecera
			SELECT RL.Fecha,
				   RL.CodigoLimite,
				   RL.NombreLimite,
				   @CLIENTEMANDATO ClienteMandato,
				   SUM(RL.TotalPosicion) AS TotalPosicion, 
				   SUM(RL.TotalParticipacion) / @TotalPortafolio AS TotalParticipacion, 
				   SUM(RL.ValorBase) AS ValorBase,
				   SUM(RL.Posicion) AS Posicion,
				   SUM(RL.Participacion)/ @TotalPortafolio AS Participacion,
				   sum(RL.MargenMinimo) AS MargenMinimo,
				   SUM(ValorPorcentajeMinimo)/ @TotalPortafolio AS ValorPorcentajeMinimo,
				   SUM(ValorPorcentajeMaximo)/ @TotalPortafolio AS ValorPorcentajeMaximo,
				   --RL.TipoCuenta,
				   RL.CodigoTerceroLimite,
				   RL.CodigoMoneda,
				   RL.Moneda AS Moneda,
				   RL.Codigo,
				   RL.Descripcion,
				   SUM(CASE WHEN RL.ValorNivelCM IS NULL THEN NULL ELSE CASE WHEN RL.CodigoLimiteCM = '07' THEN ISNULL(RL.ValorPorcentajeCM, 0) - ISNULL(RL.PosicionCM, 0) ELSE ISNULL(RL.MargenCM, 0) END END) AS MargenMaximo,
				   RL.Alerta,
				   RL.CodigoCaracteristicaRelacion,
				   RL.CodigoRelacion
			INTO #ResultadoMandatoCabecera
			FROM @tReporteLimite RL
			WHERE RL.CabeceraLimite = 'S'
			GROUP BY RL.Fecha,RL.NombreLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.Moneda,RL.CodigoLimite, RL.Codigo,RL.Descripcion,RL.Alerta, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion
			--GROUP BY RL.Fecha,RL.NombreLimite,RL.TipoCuenta,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.Moneda,RL.CodigoLimite, RL.Codigo,RL.Descripcion,RL.Alerta

			-- Detalle
			SELECT RL.Fecha,
				   RL.CodigoLimite,
				   RL.NombreLimite,
				   @CLIENTEMANDATO ClienteMandato,
				   SUM(RL.TotalPosicion) AS TotalPosicion, 
				   SUM(RL.TotalParticipacion) / @TotalPortafolio AS TotalParticipacion, 
				   SUM(RL.ValorBase) AS ValorBase,
				   SUM(RL.Posicion) AS Posicion,
				   SUM(RL.Participacion)/ @TotalPortafolio AS Participacion,
				   sum(RL.MargenMinimo)/ @TotalPortafolio  AS MargenMinimo,
				   SUM(ValorPorcentajeMinimo)/ @TotalPortafolio AS ValorPorcentajeMinimo,
				   SUM(ValorPorcentajeMaximo)/ @TotalPortafolio AS ValorPorcentajeMaximo,
				   --RL.TipoCuenta,
				   RL.CodigoTerceroLimite,
				   RL.CodigoMoneda,
				   RL.Moneda AS Moneda,
				   RL.Codigo,
				   RL.Descripcion,
				   SUM(CASE WHEN RL.ValorNivelCM IS NULL THEN NULL ELSE CASE WHEN RL.CodigoLimiteCM = '07' THEN ISNULL(RL.ValorPorcentajeCM, 0) - ISNULL(RL.PosicionCM, 0) ELSE ISNULL(RL.MargenCM, 0) END END)/ @TotalPortafolio  AS MargenMaximo,
				   RL.Alerta,
				   RL.ValorNivel,
				   RL.CodigoCaracteristicaRelacion,
				   RL.CodigoRelacion
			INTO #ResultadoMandatoDetalle
			FROM @tReporteLimite RL
			WHERE CabeceraLimite = 'N' OR CabeceraLimite is null
			GROUP BY RL.Fecha,RL.NombreLimite,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.Moneda,RL.CodigoLimite, RL.Codigo,RL.Descripcion,RL.Alerta,RL.ValorNivel, RL.CodigoCaracteristicaRelacion,RL.CodigoRelacion
			--GROUP BY RL.Fecha,RL.NombreLimite,RL.TipoCuenta,RL.CodigoTerceroLimite,RL.CodigoMoneda,RL.Moneda,RL.CodigoLimite, RL.Codigo,RL.Descripcion,RL.Alerta

			
			update #ResultadoMandatoDetalle set Posicion =Posicion/@TotalPortafolio
			where codigo = '0' 

			declare @vMes as varchar(10), @Anhio as varchar(10), @FechaProcesoCadena varchar(10)

			Select @FechaProcesoCadena = cast(@p_FechaProceso as varchar(10))

			SELECT @vMes= SUBSTRING(@FechaProcesoCadena,5,2),  @Anhio = SUBSTRING(@FechaProcesoCadena,1,4)

			update #ResultadoMandatoDetalle SET Posicion = 
			(isnull(Posicion,0) + dbo.ObtenerMontoSaldoNoAdministrado(CodigoTerceroLimite,CodigoMoneda,@p_FechaProceso,@vMes,@Anhio,CodigoLimite))


			--Actualizacion de la CarteraFondo del segundo nivel del limite 310
			UPDATE A1 SET Posicion = A2.Monto
			FROM #ResultadoMandatoDetalle A1 INNER JOIN 
			(select sum(R2.Posicion) Monto, R2.CodigoRelacion from #ResultadoMandatoDetalle R1
			INNER JOIN #ResultadoMandatoDetalle R2 on R1.CodigoRelacion = R2.CodigoRelacion
			where R1.ValorNivel = '0' and R2.ValorNivel <> '0' and R2.ValorNivel is not null
			group by R2.CodigoRelacion) A2 on A1.CodigoRelacion = A2.CodigoRelacion 
			and A1.ValorNivel = '0'

			delete from @tReporteLimite

			INSERT INTO @tReporteLimite(Fecha,CodigoPortafolioSBS,NombrePortafolio,Moneda,CodigoLimite,NombreLimite,
										TotalPosicion,TotalParticipacion,Codigo,Descripcion,ValorBase,Posicion,Participacion,MargenMinimo,MargenMaximo,Alerta,ValorPorcentajeMinimo,ValorPorcentajeMaximo,CodigoCaracteristicaRelacion,CodigoRelacion)
								 SELECT Fecha,0,ClienteMandato,Moneda,CodigoLimite,NombreLimite,TotalPosicion,TotalParticipacion,Codigo,Descripcion,ValorBase,Posicion,
								 Participacion,MargenMinimo,MargenMaximo,Alerta,ValorPorcentajeMinimo,ValorPorcentajeMaximo,CodigoCaracteristicaRelacion,CodigoRelacion
			FROM #ResultadoMandatoCabecera
			
			INSERT INTO @tReporteLimite(Fecha,CodigoPortafolioSBS,NombrePortafolio,Moneda,CodigoLimite,NombreLimite,
										TotalPosicion,TotalParticipacion,Codigo,Descripcion,ValorBase,Posicion,Participacion,MargenMinimo,MargenMaximo,Alerta,ValorPorcentajeMinimo,ValorPorcentajeMaximo,CodigoCaracteristicaRelacion,CodigoRelacion,ValorNivel)
								 SELECT Fecha,0,ClienteMandato,Moneda,CodigoLimite,NombreLimite,TotalPosicion,TotalParticipacion,Codigo,Descripcion,ValorBase,Posicion,
								 Participacion,MargenMinimo,MargenMaximo,Alerta,ValorPorcentajeMinimo,ValorPorcentajeMaximo,CodigoCaracteristicaRelacion,CodigoRelacion,ValorNivel
			FROM #ResultadoMandatoDetalle
			WHERE Posicion > 0

			DROP TABLE #ResultadoMandatoCabecera
			DROP TABLE #ResultadoMandatoDetalle
		END

		IF(@p_ClienteMandato = 'S' )
		BEGIN
			print 'A'
			SELECT * FROM @tReporteLimite
			ORDER BY ISNULL(CodigoLimite,0),
			ISNULL(CodigoRelacion,0),
			ISNULL(ValorNivel,0) ASC
			--TipoLimite ASC

		END
		ELSE
		BEGIN
		print 'B'
			SELECT * FROM  
			(
				SELECT * FROM @tReporteLimite b where b.CabeceraLimite = 'S'  
				UNION
				SELECT * FROM @tReporteLimite c where (CabeceraLimite is null or CabeceraLimite = 'N')
				AND c.Posicion > 0
			) a
			ORDER BY a.CodigoLimite, a.CodigoRelacion,
			a.ValorNivel --, a.TipoLimite
			asc 
		END

				
	DROP TABLE #DetalleNiveles
	DROP TABLE #Niveles
	DROP TABLE #Abecedario
	DROP TABLE #NivelCabecera			 

	SET NOCOUNT OFF
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ReporteLimites_New] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[sp_SIT_SeleccionCartas]'
USE [SIT-FONDOS]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_SeleccionCartas') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_SeleccionCartas]
END 
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
--	Fecha Modificacion: 02/08/2019
--	Modificado por: Karina Gomez
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Incluir las nuevas cartas y sus agrupaciones
----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_SeleccionCartas] 
	@p_CodigoMercado VARCHAR(30) = '',
	@p_CodigoPortafolio  VARCHAR(50) = '', --10025
	@p_CodigoTercero VARCHAR(15) = '', --BANCO INTERMEDIARIO
	@p_CodigoTerceroBanco VARCHAR(15) = '', --BANCO
	@p_Fecha NUMERIC(8) = 0 ,
	@p_EstadoCarta VARCHAR(1) = '',
	@p_CodigoOperacionCaja VARCHAR(7) = '',
	@p_CodigoCartaAgrupado INT=0,
	@p_Resumen CHAR(1)=''
AS
BEGIN

	DECLARE @BancoPublico VarChar(100)
	declare @portafolio varchar(50)=''

		--if(@p_CodigoOperacionCaja='' AND @p_CodigoCartaAgrupado > 0) 
		--begin 
		--	set @portafolio = @p_CodigoPortafolio
		--	set @p_CodigoPortafolio = ''
		--end

		SELECT 
		@BancoPublico = 
		T.Descripcion From ParametrosGenerales pg
		JOIN Terceros T ON T.CodigoTercero = PG.Valor
		WHERE Clasificacion = 'BANFONPUB'

		SELECT P.Descripcion DescripcionPortafolio, O.Descripcion DescripcionOperacion, ISNULL(MC.Descripcion,'SIN CARTA') ModeloCarta , T.Descripcion DescripcionIntermediario ,OC.CodigoMoneda,
		OC.Importe, ISNULL(OC.CodigoOrden , OC.CodigoOperacionCaja) NumeroOrden,
		LTRIM(RTRIM(ISNULL(P1.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoMaterno,''))) VBADMIN,
		LTRIM(RTRIM(ISNULL(P2.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoMaterno,''))) VBGERF1,
		LTRIM(RTRIM(ISNULL(P3.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoMaterno,''))) VBGERF2,
		OC.CodigoOperacion,OC.CodigoOperacionCaja,OC.EstadoCarta, OC.CodigoModelo,P.CodigoPortafolioSBS,ISNULL(OC.CorrelativoCartas,0) CorrelativoCartas --10150
		,OC.NumeroCuenta, ISNULL(TB.Descripcion,'') AS Banco, ISNULL(OC.CodigoCartaAgrupado,0) AS CodigoCartaAgrupado,
		OC.FechaOperacion,
		CASE WHEN(T2.CodigoPais = '604') THEN 'NAC' ELSE 'EXT' END Tipo,
		T2.Descripcion CONTRAPARTE,
		OC.CodigoOrden_Rel As CodigoRelacion
		into #Operaciones
		FROM OperacionesCaja OC
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS
		JOIN Operacion O ON O.CodigoOperacion = OC.CodigoOperacion 
		LEFT JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
		LEFT JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta AND CE.Situacion = 'A' AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
		LEFT JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera AND E.Situacion = 'A' AND
			E.CodigoTercero = CASE WHEN @p_CodigoTerceroBanco = '' THEN E.CodigoTercero ELSE @p_CodigoTerceroBanco END
		LEFT JOIN Terceros TB ON E.CodigoTercero = TB.CodigoTercero AND E.Situacion = 'A'
		INNER JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo  AND MC.Situacion = 'A' AND MC.CodigoOperacion = OC.CodigoOperacion 
		LEFT JOIN Personal P1 ON P1.CodigoInterno = OC.VBADMIN
		LEFT JOIN Personal P2 ON P2.CodigoInterno = OC.VBGERF1
		LEFT JOIN Personal P3 ON P3.CodigoInterno = OC.VBGERF2
		LEFT JOIN Terceros T2 ON OC.CodigoTerceroOrigen = T2.CodigoTercero
		WHERE OC.EstadoCarta   = CASE WHEN  @p_EstadoCarta = '' THEN OC.EstadoCarta ELSE @p_EstadoCarta END  
		AND OC.CodigoMercado = CASE WHEN  @p_CodigoMercado = '' THEN OC.CodigoMercado  ELSE @p_CodigoMercado END
		AND OC.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OC.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END --10025
		AND OC.CodigoTerceroOrigen = CASE WHEN @p_CodigoTercero = '' THEN OC.CodigoTerceroOrigen  ELSE @p_CodigoTercero END
		AND OC.CodigoOperacionCaja = CASE WHEN @p_CodigoOperacionCaja = '' THEN OC.CodigoOperacionCaja  ELSE @p_CodigoOperacionCaja END
		AND OC.FechaOperacion = @p_Fecha

		UNION

		SELECT 
		P.Descripcion DescripcionPortafolio
		, O.Descripcion DescripcionOperacion, ISNULL(MC.Descripcion,'SIN CARTA') ModeloCarta , T.Descripcion DescripcionIntermediario ,OC.CodigoMoneda,
		OC.Importe, RD.CodigoOrden NumeroOrden,
		LTRIM(RTRIM(ISNULL(P1.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P1.ApellidoMaterno,''))) VBADMIN,
		LTRIM(RTRIM(ISNULL(P2.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P2.ApellidoMaterno,''))) VBGERF1,
		LTRIM(RTRIM(ISNULL(P3.PrimerNombre,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoPaterno,''))) + ' ' + LTRIM(RTRIM(ISNULL(P3.ApellidoMaterno,''))) VBGERF2,
		RD.CodigoOperacion,RD.CodigoOrden,OC.EstadoCarta, OC.CodigoModelo,P.CodigoPortafolioSBS,ISNULL(OC.CorrelativoCartas,0) CorrelativoCartas --10150
		,OC.NumeroCuenta, ISNULL(TB.Descripcion,'') AS Banco, ISNULL(OC.CodigoCartaAgrupado,0) AS CodigoCartaAgrupado,
		OC.FechaOperacion,
		CASE WHEN(T2.CodigoPais = '604') THEN 'NAC' ELSE 'EXT' END Tipo,
		T2.Descripcion CONTRAPARTE,
		OC.CodigoOrden_Rel As CodigoRelacion
		FROM OperacionesCaja OC
		JOIN DPZRenovacionDetalle RD ON RD.CodigoOrden = OC.CodigoOperacionCaja
		JOIN DPZRenovacionCabecera RC ON RC.CodigoCabecera = RD.CodigoCabecera AND RC.CodigoOperacion = '101'
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OC.CodigoPortafolioSBS
		JOIN Operacion O ON O.CodigoOperacion = OC.CodigoOperacion 
		LEFT JOIN Terceros T ON T.CodigoTercero = OC.CodigoTerceroOrigen
		LEFT JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta AND CE.Situacion = 'A' AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
		LEFT JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera AND E.Situacion = 'A' AND
			E.CodigoTercero = CASE WHEN @p_CodigoTerceroBanco = '' THEN E.CodigoTercero ELSE @p_CodigoTerceroBanco END
		LEFT JOIN Terceros TB ON E.CodigoTercero = TB.CodigoTercero AND E.Situacion = 'A'
		LEFT JOIN ModeloCarta MC ON MC.CodigoModelo = OC.CodigoModelo  AND MC.Situacion = 'A' AND MC.CodigoOperacion = OC.CodigoOperacion 
		LEFT JOIN Personal P1 ON P1.CodigoInterno = OC.VBADMIN
		LEFT JOIN Personal P2 ON P2.CodigoInterno = OC.VBGERF1
		LEFT JOIN Personal P3 ON P3.CodigoInterno = OC.VBGERF2
		LEFT JOIN Terceros T2 ON OC.CodigoTerceroOrigen = T2.CodigoTercero
		WHERE
		OC.EstadoCarta   = CASE WHEN  @p_EstadoCarta = '' THEN OC.EstadoCarta ELSE @p_EstadoCarta END  
		AND OC.CodigoMercado = CASE WHEN  @p_CodigoMercado = '' THEN OC.CodigoMercado  ELSE @p_CodigoMercado END
		AND OC.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OC.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END --10025
		AND OC.CodigoTerceroOrigen = CASE WHEN @p_CodigoTercero = '' THEN OC.CodigoTerceroOrigen  ELSE @p_CodigoTercero END
		AND OC.CodigoOperacionCaja = CASE WHEN @p_CodigoOperacionCaja = '' THEN OC.CodigoOperacionCaja  ELSE @p_CodigoOperacionCaja END
		AND OC.FechaOperacion = @p_Fecha
	

		SELECT * INTO #Resultado
		FROM (

		SELECT DescripcionPortafolio, DescripcionOperacion, ModeloCarta, DescripcionIntermediario, CodigoMoneda,
		Importe,NumeroOrden,VBADMIN,VBGERF1,VBGERF2,CodigoOperacion,CodigoOperacionCaja,EstadoCarta,CodigoModelo,CodigoPortafolioSBS,CorrelativoCartas,
		NumeroCuenta, Banco,CodigoCartaAgrupado, FechaOperacion,Tipo,CONTRAPARTE,CodigoRelacion,0 CodigoAgrupado
		FROM #Operaciones OC
		WHERE OC.CodigoModelo not in ('SC01','CVA1','CVA2','OIAC','BO01') 
	
		UNION ALL

	
		SELECT OC.DescripcionPortafolio, OC.DescripcionOperacion, OC.ModeloCarta, OC.DescripcionIntermediario, OC.CodigoMoneda,
		OC.Importe,OC.NumeroOrden,OC.VBADMIN,OC.VBGERF1,OC.VBGERF2,OC.CodigoOperacion,OC.CodigoOperacionCaja,OC.EstadoCarta,'VNOR' AS CodigoModelo,OC.CodigoPortafolioSBS,OC.CorrelativoCartas,
		OC.NumeroCuenta, OC.Banco,OC.CodigoCartaAgrupado, OC.FechaOperacion,OC.Tipo,OC.CONTRAPARTE,OC.CodigoRelacion,0 CodigoAgrupado
		FROM #Operaciones OC JOIN
		--OrdenInversion OI ON OC.NumeroOrden = OI.CodigoOrden AND OI.CategoriaInstrumento = 'OR' AND OC.CodigoOperacion ='4'
		OrdenInversion OI ON OC.NumeroOrden = OI.CodigoOrden AND OI.CategoriaInstrumento = 'OR' AND OC.CodigoOperacion ='4' and OC.CodigoModelo not in ('OPR1','OPR0')
		WHERE EXISTS (SELECT CodigoOrden FROM DPZRenovacionDetalle WHERE CodigoOrden = OC.NumeroOrden)
		
		UNION ALL
		-- ACCIONES

		SELECT 
		P.Descripcion DescripcionPortafolio, 
		O.Descripcion DescripcionOperacion, 
 		ModeloCarta = CASE  WHEN OI.CategoriaInstrumento = 'AC' THEN  
								(SELECT MC.Descripcion FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('CVA1')) 
							ELSE isnull(Mo.Descripcion, 'OPERACION DE ACCIONES') END  , 
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
		CodigoModelo = CASE  WHEN OI.CategoriaInstrumento = 'AC' THEN  (SELECT CodigoModelo FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion  AND CodigoModelo IN ('CVA1'))  
							 ELSE isnull(Mo.CodigoModelo, 'OIAC') END,
		P.CodigoPortafolioSBS,
		0 AS CorrelativoCartas, 
		'' AS NumeroCuenta, 
		Banco = CASE WHEN P.IndicadorFondo  = 'S' THEN @BancoPublico  ELSE T.Descripcion END, 
		ISNULL(CodigoCartaAgrupado,0) AS CodigoCartaAgrupado,
		OI.FechaOperacion,
		CASE WHEN(T.CodigoPais = '604') THEN 'NAC' ELSE 'EXT' END Tipo,
		T.Descripcion CONTRAPARTE,
		NULL As CodigoRelacion,
		ISNULL(OI.CodigoAgrupado,0) AS CodigoAgrupado
		FROM OrdenInversion OI
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
		JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero  --BANCO INTERMEDIARIO
		LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
		INNER JOIN ModeloCarta MO ON OI.CodigoModelo = Mo.CodigoModelo AND MO.CodigoOperacion = OI.CodigoOperacion 
		WHERE
		OI.Estado = 'E-CON' AND
		OI.CategoriaInstrumento ='AC' AND
		OI.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END AND
		(CASE WHEN(OI.CategoriaInstrumento ='AC')  THEN OI.FechaOperacion ELSE OI.FechaLiquidacion END) = @p_Fecha AND
		OI.CodigoTercero  = CASE WHEN @p_CodigoTercero = '' THEN OI.CodigoTercero  ELSE @p_CodigoTercero END AND
		OI.CodigoOrden    = CASE WHEN @p_CodigoOperacionCaja = '' THEN OI.CodigoOrden  ELSE @p_CodigoOperacionCaja END AND
		'SC01' != CASE  WHEN OI.CategoriaInstrumento = 'AC' THEN  (SELECT CodigoModelo FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion  AND CodigoModelo IN ('CVA1'))  
						ELSE isnull(Mo.CodigoModelo, 'OIAC') end AND
		ISNULL(OI.CodigoAgrupado,0) > 0

		--BONOS
		UNION ALL
		SELECT 
		P.Descripcion DescripcionPortafolio, 
		O.Descripcion DescripcionOperacion, 
 		ModeloCarta = CASE WHEN OI.CategoriaInstrumento IN ('BO','CS','CD','PC','LH') THEN 
								(SELECT ISNULL(MC.Descripcion,'OPERACION DE ACCIONES') FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('BO01') AND CodigoOperacion = OI.CodigoOperacion ) 
							--WHEN (OI.CategoriaInstrumento = 'OR' AND OI.CodigoOperacion = '101')THEN
							--	 (SELECT ISNULL(MC.Descripcion,'SIN CARTA') FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('VNOR') ) 
							ELSE isnull(Mo.Descripcion, 'OPERACION DE ACCIONES') END  , 
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
		CodigoModelo = CASE WHEN OI.CategoriaInstrumento IN ('BO','CS','CD','PC','LH') THEN
								(SELECT ISNULL(MC.CodigoModelo,'SC01') FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('BO01') AND CodigoOperacion = OI.CodigoOperacion ) 
							 --WHEN (OI.CategoriaInstrumento = 'OR' AND OI.CodigoOperacion = '101')THEN 'VNOR' 
							 ELSE isnull(Mo.CodigoModelo, 'OIAC') END,
		P.CodigoPortafolioSBS,
		0 AS CorrelativoCartas, 
		'' AS NumeroCuenta, 
		Banco = CASE WHEN P.IndicadorFondo  = 'S' THEN @BancoPublico  ELSE T.Descripcion END, 
		ISNULL(CodigoCartaAgrupado,0) AS CodigoCartaAgrupado,
		OI.FechaOperacion,
		CASE WHEN(T.CodigoPais = '604') THEN 'NAC' ELSE 'EXT' END Tipo,
		T.Descripcion CONTRAPARTE,
		NULL As CodigoRelacion,
		ISNULL(OI.CodigoAgrupado,0) AS CodigoAgrupado
		FROM OrdenInversion OI
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
		JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero  --BANCO INTERMEDIARIO
		LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
		INNER JOIN ModeloCarta MO ON OI.CodigoModelo = Mo.CodigoModelo AND MO.CodigoOperacion = OI.CodigoOperacion 
		WHERE
		OI.Estado = 'E-CON' AND
		OI.CategoriaInstrumento IN ('BO','CS','CD','PC','LH') AND
		OI.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END AND
		(CASE WHEN(OI.CategoriaInstrumento in ('BO','CS','CD','PC','LH'))  THEN OI.FechaOperacion ELSE OI.FechaLiquidacion END) = @p_Fecha AND
		OI.CodigoTercero  = CASE WHEN @p_CodigoTercero = '' THEN OI.CodigoTercero  ELSE @p_CodigoTercero END AND
		OI.CodigoOrden    = CASE WHEN @p_CodigoOperacionCaja = '' THEN OI.CodigoOrden  ELSE @p_CodigoOperacionCaja END AND
		'SC01' != CASE  WHEN OI.CategoriaInstrumento IN ('BO','CS','CD','PC','LH') THEN
										(SELECT ISNULL(MC.CodigoModelo,'SC01') FROM ModeloCarta MC WHERE MC.CodigoOperacion = OI.CodigoOperacion AND CodigoModelo IN ('BO01') AND CodigoOperacion = OI.CodigoOperacion ) 
						ELSE isnull(Mo.CodigoModelo, 'OIAC') end 

		union all

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
		ISNULL(CodigoCartaAgrupado,0) AS CodigoCartaAgrupado,
		OI.FechaOperacion,
		CASE WHEN(T.CodigoPais = '604') THEN 'NAC' ELSE 'EXT' END Tipo,
		T.Descripcion CONTRAPARTE,
		NULL As CodigoRelacion,
		ISNULL(OI.CodigoAgrupado,0) AS CodigoAgrupado
		FROM OrdenInversion OI
		JOIN Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		JOIN Operacion O ON O.CodigoOperacion = OI.CodigoOperacion 
		JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero  --BANCO INTERMEDIARIO
		LEFT JOIN ClaveFirmantesCarta_OI FC ON FC.CodigoOrden = OI.CodigoOrden
		WHERE OI.Estado ='E-EJE' AND
		OI.CategoriaInstrumento='FD' AND
		OI.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END AND
		OI.FechaOperacion = @p_Fecha AND
		OI.CodigoTercero  = CASE WHEN @p_CodigoTercero = '' THEN OI.CodigoTercero  ELSE @p_CodigoTercero END AND
		OI.CodigoOrden    = CASE WHEN @p_CodigoOperacionCaja = '' THEN OI.CodigoOrden  ELSE @p_CodigoOperacionCaja END 
		AND (OI.CodigoCartaAgrupado = @p_CodigoCartaAgrupado OR @p_CodigoCartaAgrupado = 0)

	) AS TT
	WHERE   TT.EstadoCarta   = CASE WHEN  @p_EstadoCarta = '' THEN TT.EstadoCarta ELSE @p_EstadoCarta END  
		 ORDER BY ISNULL(TT.CorrelativoCartas,0) ASC , ISNULL(TT.CodigoCartaAgrupado,0) ASC


	UPDATE #Resultado SET ModeloCarta = 'SIN CARTA'
	WHERE ModeloCarta IS NULL 


	/************************************************************ ACCIONES ***************************************************************/

	UPDATE R SET R.CodigoCartaAgrupado = R.CodigoAgrupado 
	FROM #Resultado R
	WHERE CodigoAgrupado > 0

	/************************************************************ OPERACION REPORTE ***************************************************************/
	/*
	SELECT ROW_NUMBER() OVER(ORDER BY CodigoRelacion) as Id, CodigoRelacion 
	INTO #CodigoRelacion FROM #Resultado 
	where CodigoModelo = 'VNOR' AND CodigoRelacion is not null

	DECLARE @vModeloRenovacion varchar(100)
	select @vModeloRenovacion=Descripcion from ModeloCarta WHERE CodigoModelo= 'VNOR'

	declare @total_relacion int = (select count(*) from #CodigoRelacion)
	declare @ini int = 1, @CodigoRelacion varchar(20)

	while(@total_relacion >= @ini )
	begin
		set @CodigoRelacion = (select CodigoRelacion from #CodigoRelacion where Id=@ini)
	
		update r set r.CodigoCartaAgrupado = @ini,ModeloCarta = @vModeloRenovacion
		from #Resultado r 
		where  CodigoModelo= 'VNOR' AND (NumeroOrden = @CodigoRelacion OR CodigoRelacion = @CodigoRelacion)
	
		set @ini = @ini+1
	end
	*/
	
	DECLARE @vModeloRenovacion varchar(100)
	SELECT @vModeloRenovacion=Descripcion FROM ModeloCarta WHERE CodigoModelo= 'VNOR'

	DECLARE @tRenovacionOR AS TABLE(
		Id INT IDENTITY
		,CodigoPortafolioSBS VARCHAR(4)
		,Banco VARCHAR(100)
		,NumeroCuenta VARCHAR(30)
		,Tercero VARCHAR(100)
	)

	INSERT INTO @tRenovacionOR
	SELECT CodigoPortafolioSBS,Banco,NumeroCuenta,DescripcionIntermediario
	FROM #Resultado
	WHERE CodigoModelo = 'VNOR' AND CodigoOperacion = '101'
	GROUP BY CodigoPortafolioSBS,Banco,NumeroCuenta,DescripcionIntermediario

	DECLARE @codigoPortafolio VARCHAR(12)
	DECLARE @banco VARCHAR(100)
	DECLARE @numeroCuenta VARCHAR(30)
	DECLARE @tercero VARCHAR(100)
	DECLARE @codigoOrden VARCHAR(12)
	DECLARE @iMin AS INT = (SELECT MIN(Id) FROM @tRenovacionOR)
	DECLARE @iMax AS INT = (SELECT MAX(Id) FROM @tRenovacionOR)

	WHILE @iMin <= @iMax
	BEGIN
		SET @codigoPortafolio = (SELECT CodigoPortafolioSBS FROM @tRenovacionOR WHERE Id = @iMin)
		SET @banco = (SELECT Banco FROM @tRenovacionOR WHERE Id = @iMin)
		SET @numeroCuenta = (SELECT NumeroCuenta FROM @tRenovacionOR WHERE Id = @iMin)
		SET @tercero = (SELECT Tercero FROM @tRenovacionOR WHERE Id = @iMin)

		DECLARE @tCodigoRelacion AS TABLE (
			Id INT IDENTITY
			,CodigoOrden VARCHAR(12)
		)

		INSERT INTO @tCodigoRelacion
		SELECT NumeroOrden
		FROM #Resultado
		WHERE CodigoPortafolioSBS = @codigoPortafolio AND Banco = @banco AND NumeroCuenta = @numeroCuenta AND DescripcionIntermediario = @tercero

		DECLARE @jMin AS INT = (SELECT MIN(Id) FROM @tCodigoRelacion)
		DECLARE @jMax as INT = (SELECT MAX(Id) FROM @tCodigoRelacion)

		WHILE @jMin <= @jMax
		BEGIN
			SET @codigoOrden = (SELECT CodigoOrden FROM @tCodigoRelacion WHERE Id = @jMin)

			UPDATE r SET r.CodigoCartaAgrupado = @iMin,ModeloCarta = @vModeloRenovacion
			FROM #Resultado r 
			WHERE CodigoModelo= 'VNOR' and CodigoOperacion = '101'
				AND NumeroOrden IN (SELECT CodigoOrden FROM DPZRenovacionCabecera WHERE CodigoOrden = @codigoOrden)

			UPDATE r set r.CodigoCartaAgrupado = @iMin,ModeloCarta = @vModeloRenovacion
			FROM #Resultado r
			WHERE CodigoModelo= 'VNOR' and CodigoOperacion = '4'
				AND NumeroOrden IN (SELECT CodigoOrden FROM DPZRenovacionDetalle WHERE CodigoOrden = @codigoOrden)

			SET @jMin = @jMin + 1
		END
		SET @iMin = @iMin + 1
	END

	/************************************************************ DEPOSITOS AL EXTERIOR ***************************************************************/
	declare @total_relacion int
	declare @ini int = 1, @CodigoRelacion varchar(20)
	
	SELECT CodigoCabecera,CodigoOrden
	into #RenovacionCabecera
	FROM DPZRenovacionCabecera WHERE CodigoOrden IN (SELECT NumeroOrden FROM #Resultado WHERE CodigoModelo IN ('CO02','CO03'))

	SELECT ROW_NUMBER() over(order by c.CodigoCabecera)Id, d.CodigoOrden Relacion,CodigoOperacion, c.CodigoOrden 
	into #RenovacionRelacion
	FROM DPZRenovacionDetalle d join #RenovacionCabecera c on d.CodigoCabecera = c.CodigoCabecera 

	declare @v_CodigoOrden varchar(20)=''
	set @total_relacion = (select count(*) from #RenovacionRelacion)
	set @ini = 1

	while(@total_relacion >= @ini )
	begin
		select @CodigoRelacion=Relacion, @v_CodigoOrden=CodigoOrden  from #RenovacionRelacion where Id=@ini
	
		update r set r.CodigoCartaAgrupado = @ini
		from #Resultado r
		where (NumeroOrden in (@CodigoRelacion,@v_CodigoOrden))
		AND CodigoModelo IN ('CO02','CO03')

		set @ini = @ini+1
	end

	----------------------------------------------------------------------------------------------------------------------------------------------------------------

	select CodigoCartaAgrupado, ROW_NUMBER() over(order by CodigoPortafolioSBS,CodigoCartaAgrupado,tipo,CodigoModelo) orden,tipo,CodigoModelo,CodigoPortafolioSBS
	into #tbl1
	From #Resultado w where CodigoCartaAgrupado > 0  AND CodigoModelo != 'CVA1'
	group by CodigoCartaAgrupado,tipo,CodigoModelo,CodigoPortafolioSBS


	update t1 set t1.CodigoCartaAgrupado = tb.orden
	 from #Resultado t1 join (
	select CodigoCartaAgrupado, Orden,tipo,CodigoModelo,CodigoPortafolioSBS From #tbl1 
	)as tb  on t1.CodigoCartaAgrupado = tb.CodigoCartaAgrupado and t1.Tipo = tb.tipo and t1.CodigoModelo = tb.CodigoModelo and tb.CodigoPortafolioSBS=t1.CodigoPortafolioSBS
	where t1.CodigoCartaAgrupado > 0 
	drop table #tbl1


	IF(@p_Resumen = 'S')
	BEGIN

	select ROW_NUMBER() OVER(PARTITION BY CodigoCartaAgrupado ORDER BY CodigoCartaAgrupado) Orden,*   
	INTO #AGRUPADO
	from #Resultado

	select DescripcionPortafolio,DescripcionOperacion,ModeloCarta,DescripcionIntermediario,CodigoMoneda,Importe,NumeroOrden,
	VBADMIN,VBGERF1,VBGERF2,CodigoOperacion,CodigoOperacionCaja, EstadoCarta,CodigoModelo,CodigoPortafolioSBS,CorrelativoCartas,NumeroCuenta,Banco,CodigoCartaAgrupado,
	FechaOperacion,tipo, CONTRAPARTE,CodigoAgrupado 
	from #AGRUPADO --where Orden = '1' 

	drop table #AGRUPADO

	END
	ELSE
	BEGIN
	--select * from #Resultado
	select DescripcionPortafolio,DescripcionOperacion,ModeloCarta,DescripcionIntermediario,CodigoMoneda,Importe,NumeroOrden,
	VBADMIN,VBGERF1,VBGERF2,CodigoOperacion,CodigoOperacionCaja, EstadoCarta,CodigoModelo,CodigoPortafolioSBS,CorrelativoCartas,NumeroCuenta,Banco,CodigoCartaAgrupado,
	FechaOperacion,tipo, CONTRAPARTE,CodigoAgrupado from #Resultado 
	where (CodigoCartaAgrupado = @p_CodigoCartaAgrupado OR @p_CodigoCartaAgrupado = 0) AND 
		  (@p_CodigoPortafolio = '' OR CodigoPortafolioSBS = @p_CodigoPortafolio)
	ORDER BY CodigoOperacion DESC 
	END

	drop table #Resultado
	--drop table #CodigoRelacion
	drop table #Operaciones

END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_SeleccionCartas] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[ValidadorLimiteDetalle_Insertar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ValidadorLimiteDetalle_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[ValidadorLimiteDetalle_Insertar]
END 
GO
-------------------------------------------------------------------------------------------------------------------------  
--Objetivo: INSERTAR LOS REGISTROS DE LOS LIMITES VINCULADOS CON LOS VALIDADORES
-------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 25/10/2018  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Nuevo  
-------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 25/10/2018  
-- Modificado por: Karina Gomez
-- Nro. Orden de Trabajo: 12163  
-- Descripcion del cambio:   Se agrego el campo Tipo
-------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[ValidadorLimiteDetalle_Insertar]  
(  
 @p_CodigoLimite VARCHAR(10)  
 ,@p_CodigoValidador VARCHAR(10)  
 ,@p_UsuarioCreacion VARCHAR(20)  
 ,@p_FechaCreacion NUMERIC(8)  
 ,@p_HoraCreacion VARCHAR(10)  
 ,@p_Host VARCHAR(20) 
 ,@p_Tipo CHAR(1)
)  
AS    
BEGIN  
 DECLARE @id INT 
 SET @id = (SELECT ISNULL(MAX(Id),0) + 1 FROM ValidadorLimiteDetalle)  
 INSERT INTO 
		ValidadorLimiteDetalle  (
								 Id,
								 CodigoValidador,
								 CodigoLimite,
								 UsuarioCreacion,
								 FechaCreacion,
								 HoraCreacion,
								 Host,
								 Tipo
								)  
 VALUES (
		 @id,
		 @p_CodigoValidador,
		 @p_CodigoLimite,
		 @p_UsuarioCreacion,
		 @p_FechaCreacion,
		 @p_HoraCreacion,
		 @p_Host,
		 @p_Tipo
		 )  
END  
GO

GRANT EXECUTE ON [dbo].[ValidadorLimiteDetalle_Insertar] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[ValidadorLimiteDetalle_Seleccionar]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='ValidadorLimiteDetalle_Seleccionar') BEGIN 
	DROP PROCEDURE [dbo].[ValidadorLimiteDetalle_Seleccionar]
END 
GO
-------------------------------------------------------------------------------------------------------------------------  
--Objetivo: OBTENER LOS LIMITES VINCULADOS CON LOS VALIDADORES
-------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 25/10/2018  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11655  
-- Descripcion del cambio: Nuevo  
-------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 02/08/2019  
-- Modificado por: Karina Gomez 
-- Nro. Orden de Trabajo: 12163  
-- Descripcion del cambio:  Se agrego el campo Tipo
-------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[ValidadorLimiteDetalle_Seleccionar]  
(  
  @p_CodigoLimite VARCHAR(10)  
 ,@p_CodigoValidador VARCHAR(10)  
)  
AS    
BEGIN  
 SELECT 
		Id,
		CodigoValidador,
		CodigoLimite,
		Tipo
FROM 
		ValidadorLimiteDetalle  
 WHERE   
	    CodigoLimite = @p_CodigoLimite 
		AND CodigoValidador = (CASE WHEN @p_CodigoValidador = '' THEN CodigoValidador ELSE @p_CodigoValidador END)
END  
GO

GRANT EXECUTE ON [dbo].[ValidadorLimiteDetalle_Seleccionar] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[sp_ListarPortafolio_Descripcion]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ListarPortafolio_Descripcion') BEGIN 
	DROP PROCEDURE [dbo].[sp_ListarPortafolio_Descripcion]
END 
GO
-------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 02/08/2019  
-- Modificado por: Diego Tueros
-- Nro. Orden de Trabajo: 12163  
-- Descripcion del cambio: Obtener codigo de portafolio por descripcion
-------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE sp_ListarPortafolio_Descripcion
(
	@p_DesPortafolio	varchar(1000),
	@p_Out	varchar(10) output
)
AS
BEGIN
	
		SELECT @p_Out = isnull(CodigoPortafolioSBS,'') FROM Portafolio WHERE Descripcion = @p_DesPortafolio

		return @p_Out

END
GO

GRANT EXECUTE ON [dbo].[sp_ListarPortafolio_Descripcion] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[SP_SIT_ListaVistaCaracteristica]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='SP_SIT_ListaVistaCaracteristica') BEGIN 
	DROP PROCEDURE [dbo].[SP_SIT_ListaVistaCaracteristica]
END 
GO
-------------------------------------------------------------------------------------------------------------------------  
-- Fecha creacion: 02/08/2019  
-- creado por: Karina Gomez
-- Nro. Orden de Trabajo: 12163  
-- Descripcion del cambio: lista las vistas por cartacteristica grupo
-------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE SP_SIT_ListaVistaCaracteristica
@p_CodigoCaracteristica VARCHAR(10)
AS
----------------------------------------------------
--@Test: exec SP_SIT_ListaVistaCaracteristica '65'
----------------------------------------------------
BEGIN
	
	DECLARE @v_Vista VARCHAR(100)
	SELECT @v_Vista = NombreVista FROM CaracteristicaGrupo WHERE CodigoCaracteristica = @p_CodigoCaracteristica 

	DECLARE @sql varchar(max)

	set @sql= 'select * from '+@v_Vista +' where isnull(Descripcion,'''') <> '''' '
	
	exec (@sql)   

END
GO

GRANT EXECUTE ON [dbo].[SP_SIT_ListaVistaCaracteristica] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[Limite_SeleccionarCaracteristicaDetalleNiveles]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Limite_SeleccionarCaracteristicaDetalleNiveles') BEGIN 
	DROP PROCEDURE [dbo].[Limite_SeleccionarCaracteristicaDetalleNiveles]
END 
GO
/*  
<Inicio_Cabecera>  
	<Nombre>Limite_SeleccionarCaracteristicaDetalleNiveles</Nombre>  
	<Nro_OT></Nro_OT>  
	<Objetivo>Selecciona los porcentajes de los valores de los niveles de las caracteristicas de los limites</Objetivo>
	<Autor>(Zoluxiones Consulting) Fanny Nina</Autor>  
	<Fecha>03/01/2008</Fecha>         
	<Parametros_Entrada>@p_CodigoNivelLimite</Parametros_Entrada>           
	<Parametros_Entrada>@p_FlagTipoPorcentaje</Parametros_Entrada>           
</Fin_Cabecera>  
<Inicio_Detalle>
	<Nro_OT>65023</Nro_OT>
	<Objetivo>3 se agrega el campo ValorPorcentajeM</Objetivo>
	<Autor>(Zoluxiones Consulting) Hector Delgado G.</Autor>
	<Fecha>22/05/2012</Fecha>
</Inicio_Detalle>
*/  
--------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregaron los campos para indicar si el detalle tiene valor de limite especifico
---------------------------------------------------------------------------------------------------------------------------  
--	Fecha Modificacion: 27/06/2019
--	Creado por: Karina Gomez
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se agregaron campos para la relacion multinivel
---------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[Limite_SeleccionarCaracteristicaDetalleNiveles]
 (
	@p_CodigoNivelLimite varchar(5),
	@p_FlagTipoPorcentaje varchar(2)
)
AS
begin
SET NOCOUNT ON

	if (@p_FlagTipoPorcentaje='A')
	begin
		select
		'S' as Selected,
		D.CodigoNivelLimite,	
		D.CodigoCaracteristica,
		D.ClaseNormativa,
		C.NombreVista,
		'' as ValorCaracteristica,
		'' as 'DescripcionValorCaracteristica',
		D.ValorPorcentaje,
		Case D.Situacion when 'A' then 'ACTIVO' when 'I' then 'INACTIVO' end as Situacion,
		D.ValorPorcentajeM,	--HDG OT 65023 20120522
		ISNULL(D.TieneValorEspecifico,'N') AS ValorEspecifico
		from dbo.DetalleNivelLimite1 D, dbo.CaracteristicaGrupo C
		where 
		D.CodigoNivelLimite = case when len(@p_CodigoNivelLimite)=0 then D.CodigoNivelLimite else @p_CodigoNivelLimite end
		and D.codigoCaracteristica = C.CodigoCaracteristica
		order by CodigoNivelLimite asc
	end	
	if (@p_FlagTipoPorcentaje='D')
	begin
		select
		'S' as Selected,
		D.CodigoNivelLimite,	
		D.CodigoCaracteristica,
		D.ClaseNormativa,
		C.NombreVista,
		D.ValorCaracteristica,
		'Falta' as 'DescripcionValorCaracteristica',
		D.ValorPorcentaje,
		Case D.Situacion when 'A' then 'ACTIVO' when 'I' then 'INACTIVO' end as Situacion,
		D.ValorPorcentajeM,	--HDG OT 65023 20120522
		ISNULL(D.TieneValorEspecifico,'N') AS ValorEspecifico,
		D.CodigoCaracteristicaRelacion,
		D.CodigoRelacion,
		'' AS CodigoClaseActivoLimite
		from dbo.DetalleNivelLimite2 D, dbo.CaracteristicaGrupo C
		where 
		D.CodigoNivelLimite = case when len(@p_CodigoNivelLimite)=0 then D.CodigoNivelLimite else @p_CodigoNivelLimite end
		and D.codigoCaracteristica = C.CodigoCaracteristica
		order by CodigoNivelLimite asc
	end
end
GO

GRANT EXECUTE ON [dbo].[Limite_SeleccionarCaracteristicaDetalleNiveles] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[OrdenInversion_ListarPorCodigoOrden]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_ListarPorCodigoOrden') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_ListarPorCodigoOrden]
END 
GO


-----------------------------------------------------------------------------------------------------------------------
--Objetivo: Listar ordenes por codigo
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 20/07/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10090
--	Descripcion del cambio: Se orden el procedimiento
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 27/08/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: OT11590
--	Descripcion del cambio: Agregar el campo TipoValorización para los tipos de instrumentos Bonos
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 2018-09-19
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: OT11590
--	Descripcion del cambio: Agregar el campo Tir Neta para los tipos de instrumentos Bonos
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 2019-07-30
--	Modificado por: Diego 12163
--	Nro. Orden de Trabajo: OR12012
--	Descripcion del cambio: Agregar el campo Observacion Carta 
-----------------------------------------------------------------------------------------------------------------------
create PROCEDURE [dbo].[OrdenInversion_ListarPorCodigoOrden]
	@p_CodigoOrden varchar(6),
	@p_CodigoPortafolioSBS varchar(10) ,
	@p_LlamadoForward varchar(1) = 'N'
AS
BEGIN
	Declare @CodigoMonedaNemonico varchar(10),@MonedaPago varchar(10),@Nemonico varchar(15),@FechaOperacion numeric(8),@MontoDestinoOI  decimal(22,7),        
	@MontoOrigen  decimal(22,7),@CodigoOperacion varchar(6),@TC1  numeric(22,7),@TC2  numeric(22,7),@NumeroPoliza varchar(20),@OrdenGenera varchar(20),
	@CategoriaInstrumento varchar(20),@CodigoPreOrden varchar(6),@VencimientoAno varchar(10),@VencimientoMes varchar(10),@TipoCondicion varchar(5)
	SELECT @VencimientoAno = '',@VencimientoMes = '',@TipoCondicion = ''
	SELECT @CodigoOperacion = CodigoOperacion,  @CategoriaInstrumento = categoriaInstrumento,@CodigoPreOrden = CodigoPreOrden
	FROM OrdenInversion WHERE CodigoOrden = @p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
	IF @CodigoOperacion not in ('65', '66', '93', '94', '95', '97','101') --No aplica para DIVISAS y FORWARDS        
	BEGIN        
		SELECT @Nemonico=CodigoMnemonico,@FechaOperacion=FechaOperacion,@MontoOrigen=MontoOperacion from OrdenInversion          
		WHERE  CodigoOrden=@p_CodigoOrden AND CodigoPortafolioSBS=@p_CodigoPortafolioSBS          
		--LC. Pagar con Otra Moneda                       
		SELECT @CodigoMonedaNemonico = CodigoMoneda,@MonedaPago = MonedaPago FROM Valores WHERE CodigoNemonico = @Nemonico
		IF (@CategoriaInstrumento <> 'BO')
		BEGIN
			IF (@CategoriaInstrumento <> 'AC')
			BEGIN  
				--Verifico que este en valores con CodigoMoneda diferente a MonedaPago            
				IF (@CodigoMonedaNemonico is not null AND @MonedaPago is not null AND @CodigoMonedaNemonico <> @MonedaPago)            
				BEGIN
					SELECT @TC1=dbo.RetornarTipoCambioOperacion(@CodigoMonedaNemonico,@FechaOperacion)             
					SELECT @TC2=dbo.RetornarTipoCambioOperacion(@MonedaPago,@FechaOperacion)        
					SET @TC1=isnull(@TC1,0)/ isnull(@TC2,1)        
					SET @MontoDestinoOI=isnull(@TC1,0)*isnull(@MontoOrigen,0)        
					UPDATE OrdenInversion SET TipoCambio=@TC1,MontoDestino=@MontoDestinoOI,CodigoMonedaDestino=@MonedaPago        
					WHERE CodigoOrden=@p_CodigoOrden AND CodigoPortafolioSBS=@p_CodigoPortafolioSBS        
				END
				ELSE
				BEGIN
					UPDATE OrdenInversion SET TipoCambio=null,MontoDestino=null,CodigoMonedaDestino=null        
					WHERE CodigoOrden=@p_CodigoOrden AND CodigoPortafolioSBS=@p_CodigoPortafolioSBS        
				END
			END
		END
	END
	--Actualiza el NumeroPoliza de las CANCELACIONES q hayan bajado sin poliza en la apertura
	IF @CodigoOperacion = '4'
	BEGIN
		SELECT @NumeroPoliza = isnull(NumeroPoliza,''), @OrdenGenera = isnull(OrdenGenera,'') from OrdenInversion
		WHERE CodigoOrden = @p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		if @NumeroPoliza = '' AND @OrdenGenera <> ''
		BEGIN
			SELECT @NumeroPoliza = isnull(NumeroPoliza,'') from OrdenInversion WHERE CodigoOrden = @OrdenGenera AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
			update OrdenInversion set NumeroPoliza = @NumeroPoliza, CodigoSBS = LEFT(CodigoSBS,7) + @NumeroPoliza WHERE CodigoOrden = @p_CodigoOrden        
			AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
		END
	END
	DECLARE @m_CorrelativoFD VARCHAR(3),@m_SecuenciaFD int,@m_OperacionFD char(1),@p_NumeroPoliza varchar(20)
	SELECT @m_SecuenciaFD = max(cf.Secuencia) FROM OrdenInversion oi
 	inner join CorrelativoForward cf on cf.Codigo = substring(oi.numeropoliza,3,3)
	WHERE oi.codigoMnemonico = 'FORWARD' AND oi.estado ='E-CON' AND oi.situacion = 'A'      
	--
	SELECT @m_CorrelativoFD = Codigo from CorrelativoForward WHERE Secuencia = isnull(@m_SecuenciaFD,0) + 1
	SELECT @m_OperacionFD = case when @CodigoOperacion = '93' then 'C' when @CodigoOperacion = '94' then 'V' end
	--Generacion del Correlativo de Poliza de Forward
	--Se debe generar numeropoliza,para oi en estado eje.
	--Pero no se debe generar cuando se realizar el llamado , debe de mostrar vacio, ni cuando este confirmada sino mostrar la existente
	if @p_LlamadoForward ='S' set @p_NumeroPoliza = ''
	else set @p_NumeroPoliza = 'H' + @m_OperacionFD + @m_CorrelativoFD
	if dbo.fn_EsMultiPortafolio(@p_CodigoPortafolioSBS) = 'N'
	BEGIN
		SELECT CodigoOrden,FechaOperacion,FechaLiquidacion,MontoNominalOrdenado,MontoNominalOperacion,CodigoTipoCupon,YTM,PrecioNegociacionLimpio,PrecioNegociacionSucio,
		MontoOperacion,Observacion,Plazo,PTasa,InteresAcumulado,InteresCAstigado,TasaCastigo,MontoPreCancelar,InteresCorridoNegociacion,PorcentajeAcciones,PorcentajeDolares,
		PorcentajeBonos,CantidadValor,MontoContado,MontoPlazo,Precio,TipoCambio,MontoOrigen,MontoDestino,TipoCobertura,FechaPago,ContadoSoles,TipoCambioSpot,ContadoDolares,
		PlazoSoles,TipoCambioForw,PlazoDolares,PorcentajeRendimiento,TipoCambioFuturo,CodigoMoneda,CodigoMonedaDestino,CodigoPreOrden,TotalComisiones,PrecioPromedio,
		MontoNetoOperacion,Estado,Delibery,CodigoISIN,CodigoTercero,CodigoPortafolioSBS,CodigoMnemonico,
		CASE WHEN CodigoOperacion = '61' THEN '1' WHEN CodigoOperacion = '62' THEN '2' ELSE CodigoOperacion END AS CodigoOperacion,
		CantidadOrdenado,CantidadOperacion, case when codigomnemonico = 'FORWARD' AND estado <>'E-CON' then '' else CodigoSBS end as CodigoSBS,CodigoContacto,
		(case when CodigoMnemonico = 'FORWARD' AND Estado<>'E-CON' then @p_NumeroPoliza else NumeroPoliza end) AS NumeroPoliza,CodigoUsuario,HoraOperacion,CodigoGestor,
		CodigoMonedaOrigen,Diferencial,CodigoMotivo,MontoCancelar,CodigoTipoTitulo,CategoriaInstrumento,CodigoMnemonicoReporte,FechaContrato,OR_CantidadRV ,OR_CantidadNominal,
		OR_CantidadRF,TasaPorcentaje,TipoFondo,FechaTrato,'' isTemporal,PrecioCalculado,InteresCorrido,AfectaFlujoCaja,CodigoPlaza,Fixing,GrupoIntermediario,MontoPrima,
		TipoTramo,CodigoMotivoCambio,case when Ficticia = '' then 'N' else Ficticia end as 'Ficticia',Renovacion,TipoMonedaForw,RegulaSBS,HoraEjecucion,MedioNegociacion,
		EventoFuturo = isnull(EventoFuturo,0),VencimientoAno as VencimientoAno,VencimientoMes as VencimientoMes,TipoCondicion as TipoCondicion, TipoValorizacion, TirNeta = isnull(TirNeta,0),
		ObservacionCarta
		from OrdenInversion
		WHERE CodigoOrden=@p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	END
	ELSE
	BEGIN
		SELECT CodigoPreOrden,FechaOperacion,FechaLiquidacion,MontoNominalOrdenado,
		MontoNominalOperacion = case when codigoMnemonico = 'DIVISA' then MontoOperacion else isnull(MontoNominalOperacion,0) end,CodigoTipoCupon,YTM,PrecioNegociacionLimpio,
		PrecioNegociacionSucio,MontoOperacion,Observacion,Plazo,PTasa,InteresAcumulado,InteresCAstigado,TasaCastigo,MontoPreCancelar,InteresCorridoNegociacion,
		PorcentajeAcciones,PorcentajeDolares,PorcentajeBonos,CantidadValor,MontoContado,MontoPlazo,Precio,TipoCambio,MontoOrigen,MontoDestino,TipoCobertura,FechaPago,
		ContadoSoles,TipoCambioSpot,ContadoDolares,PlazoSoles,TipoCambioForw,PlazoDolares,PorcentajeRendimiento,TipoCambioFuturo,CodigoMoneda,CodigoMonedaDestino,
		TotalComisiones,PrecioPromedio,MontoNetoOperacion,Estado,Delibery,CodigoISIN,CodigoTercero,CodigoPortafolioSBS,CodigoMnemonico,CodigoOperacion,CantidadOrdenado,
		'CantidadOperacion' = case when left(CodigoSBS,2) = '60' then MontoNominalOperacion when codigoMnemonico = 'DIVISA' then MontoOperacion else CantidadOperacion end,
		CodigoSBS,CodigoContacto,(case CodigoMnemonico when 'FORWARD' then @p_NumeroPoliza else NumeroPoliza end) AS NumeroPoliza,CodigoUsuario,HoraOperacion,CodigoGestor,
		CodigoMonedaOrigen,Diferencial,CodigoMotivo,MontoCancelar,CodigoTipoTitulo,CategoriaInstrumento,CodigoMnemonicoReporte,FechaContrato,OR_CantidadRV,OR_CantidadNominal,
		OR_CantidadRF,TasaPorcentaje,TipoFondo,FechaTrato,'' isTemporal,PrecioCalculado,InteresCorrido,AfectaFlujoCaja,CodigoPlaza,Fixing,GrupoIntermediario,TipoTramo,
		MontoPrima = 0,'' as 'CodigoMotivoCambio','' as 'Ficticia','' as Renovacion,'' as TipoMonedaForw,'' as RegulaSBS,HoraEjecucion,'' as MedioNegociacion,
		@VencimientoAno as VencimientoAno,@VencimientoMes as VencimientoMes,@TipoCondicion as TipoCondicion,
		'' as ObservacionCarta
		from PreOrdenInversion WHERE CodigoPreOrden=@p_CodigoOrden
	END
END
GO

GRANT EXECUTE ON [dbo].[OrdenInversion_ListarPorCodigoOrden] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[OrdenInversion_InsertarOI]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_InsertarOI') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_InsertarOI]
END 
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
--	Fecha modificacion: 2019-07-30
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Agregar el campo Observacion Carta 
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
	@p_TirNeta NUMERIC(22,7) = NULL,
	@p_ObservacionCarta varchar(2000) = NULL
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


PRINT '[OrdenInversion_ModificarOI]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_ModificarOI') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_ModificarOI]
END 
GO

-----------------------------------------------------------------------------------------------------------------------
--Objetivo: Modificar Orden de Inversión
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 27/08/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: OT11590
--	Descripcion del cambio: Agregar el campo TipoValorización para los tipos de instrumentos Bonos
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 13/12/2018
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11687
--	Descripcion del cambio: Aumento de tamaño input: @p_Plazo a NUMERIC(4)
-----------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 30/07/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Agregar la actualizacion del campo ObservacionCarta
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[OrdenInversion_ModificarOI]
	@p_CodigoOrden VARCHAR(6) ,--Codigo de la orden de Inversion        
	@p_FechaOperacion      NUMERIC(8) ,        
	@p_FechaLiquidacion     NUMERIC(8) ,        
	@p_MontoNominalOrdenado NUMERIC(22,7) ,        
	@p_MontoNominalOperacion NUMERIC(22,7) ,        
	@p_CodigoTipoCupon      VARCHAR(4) ,        
	@p_YTM                  NUMERIC(22,7) ,        
	@p_PrecioNegociacionLimpio NUMERIC(22,7) ,        
	@p_PrecioNegociacionSucio NUMERIC(22,7) ,        
	@p_MontoOperacion       NUMERIC(22,7) ,        
	@p_Observacion          VARCHAR(20) ,        
	@p_Plazo                NUMERIC(4) ,        
	@p_PTasa                NUMERIC(22,7) ,        
	@p_InteresAcumulado     NUMERIC(22,7) ,        
	@p_InteresCastigado     NUMERIC(22,7) ,        
	@p_TasaCastigo          NUMERIC(22,7) ,        
	@p_MontoPreCancelar     NUMERIC(22,7) ,        
	@p_InteresCorridoNegociacion NUMERIC(22,7) ,        
	@p_PorcentajeAcciones   NUMERIC(22,6) ,        
	@p_PorcentajeDolares    NUMERIC(22,6) ,        
	@p_PorcentajeBonos      NUMERIC(22,6) ,        
	@p_CantidadValor        NUMERIC(22,7) ,        
	@p_MontoContado         NUMERIC(22,7) ,        
	@p_MontoPlazo           NUMERIC(22,7) ,        
	@p_Precio               NUMERIC(22,7) ,        
	@p_TipoCambio           NUMERIC(22,7) ,        
	@p_MontoOrigen          NUMERIC(22,7) ,        
	@p_MontoDestino         NUMERIC(22,7) ,        
	@p_TipoCobertura        VARCHAR(1) ,        
	@p_FechaPago            NUMERIC(8) ,        
	@p_ContadoSoles         NUMERIC(22,7) ,        
	@p_TipoCambioSpot       NUMERIC(22,7) ,        
	@p_ContadoDolares       NUMERIC(22,7) ,        
	@p_PlazoSoles   NUMERIC(22,7) ,        
	@p_TipoCambioForw       NUMERIC(22,7) ,        
	@p_PlazoDolares         NUMERIC(22,7) ,        
	@p_PorcentajeRendimiento NUMERIC(22,7) ,        
	@p_TipoCambioFuturo     NUMERIC(22,7) ,        
	@p_CodigoMoneda         VARCHAR(10) ,        
	@p_CodigoMonedaDestino  VARCHAR(10) ,        
	@p_CodigoPreOrden       VARCHAR(6) ,        
	@p_UsuarioModIFicacion      VARCHAR(15) ,        
	@p_HoraModIFicacion         VARCHAR(10) ,        
	@p_TotalComisiones      NUMERIC(22,7) ,        
	@p_PrecioPromedio       NUMERIC(22,7) ,        
	@p_MontoNetoOperacion   NUMERIC(22,7) ,        
	@p_Estado               VARCHAR(5) ,        
	@p_Situacion            VARCHAR(1) ,        
	@p_Delibery             VARCHAR(1) ,        
	@p_Host                 VARCHAR(20) ,        
	@p_CodigoTercero        VARCHAR(11) ,        
	@p_FechaModIFicacion        NUMERIC(8) ,        
	@p_CodigoPortafolioSBS  VARCHAR(10) ,        
	@p_CodigoContacto       VARCHAR(3) ,        
	@p_CodigoOperacion  VARCHAR(2) ,        
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
	@p_CodigoMotivoCambio VARCHAR(2),
	@p_IndicaCambio VARCHAR(1),
	@p_AfectaFlujoCaja VARCHAR(1),
	@p_CodigoPlaza VARCHAR(3),
	@p_Fixing NUMERIC(22,7),
	@p_GrupoIntermediario VARCHAR(10),
	@p_montoPrima NUMERIC (22,7), 
	@p_tipotramo VARCHAR(10),
	@p_Renovacion VARCHAR(1),
	@p_TipoMonedaForw VARCHAR(10),
	@p_RegulaSBS VARCHAR(1),
	@p_MedioNegociacion VARCHAR(1),
	@p_HoraEjecucion VARCHAR(10),
	@p_TipoCondicion VARCHAR(4),
	@p_VencimientoAno VARCHAR(10),
	@p_VencimientoMes VARCHAR(10),
	@p_TipoValorizacion VARCHAR(10),
	@p_TirNeta numeric(22,7),
	@p_ObservacionCarta VARCHAR(2000)
AS
SET @p_TipoFondo = isnull(@p_TipoFondo,'')
IF @p_IsTemporal = '1' BEGIN      
	SET @p_Estado = 'E-TMP'      
END ELSE BEGIN      
	IF  dbo.fn_EsMultiPortafolio(@p_CodigoPortafolioSBS) = 'N'
		SELECT @p_Estado = estado FROM OrdenInversion WHERE CodigoOrden=@p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	ELSE        
		SELECT @p_Estado = estado FROM PreOrdenInversion WHERE CodigoPreOrden=@p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
END
IF dbo.fn_EsMultiPortafolio(@p_CodigoPortafolioSBS) = 'N' -- (@p_CodigoPortafolioSBS != 'MULTIFONDO')        
BEGIN        
	Declare @CodigoMonedaNemonico VARCHAR(15),@MonedaPago VARCHAR(10),@Nemonico VARCHAR(15),@CantidadOperacion NUMERIC(22,7),@CantidadOrdenado NUMERIC(22,7),
	@PrecioNegociacionLimpio NUMERIC(22,7),@Precio NUMERIC(22,7),@MontoCancelar NUMERIC(22,7),@MontoNominalOperacion NUMERIC(22,7),@MontoOperacion NUMERIC(22,7),
	@CambioTrz VARCHAR(1)
	SELECT @Nemonico=CodigoMnemonico,@CantidadOperacion = CantidadOperacion,@CantidadOrdenado = CantidadOrdenado,@PrecioNegociacionLimpio = PrecioNegociacionLimpio,
	@Precio = Precio,@MontoCancelar = MontoCancelar,@MontoNominalOperacion = MontoNominalOperacion,@MontoOperacion = MontoOperacion
	FROM OrdenInversion WHERE CodigoOrden=@p_CodigoOrden and CodigoPortafolioSBS=@p_CodigoPortafolioSBS
	SELECT @CodigoMonedaNemonico=CodigoMoneda,@MonedaPago=MonedaPago FROM Valores WHERE CodigoNemonico=@Nemonico and CodigoMoneda<>MonedaPago
	--VerIFico que este en valores con CodigoMoneda dIFerente a MonedaPago
	IF (@CodigoMonedaNemonico is not null and @MonedaPago is not null)      
	BEGIN
		Declare @TC1  NUMERIC(22,7),@TC2  NUMERIC(22,7)      
		SELECT @TC1=dbo.RetornarTipoCambioOperacion(@CodigoMonedaNemonico,@p_fechaOperacion)       
		SELECT @TC2=dbo.RetornarTipoCambioOperacion(@MonedaPago,@p_fechaOperacion)      
		SET @TC1=isnull(@TC1,0)/ isnull(@TC2,1)      
		SET @p_TipoCambio=@TC1      
		SET @p_CodigoMonedaDestino=@MonedaPago      
		IF(isnull(@p_MontoDestino,0)=0)      
		BEGIN
			SET @p_MontoDestino=isnull(@TC1,0)*isnull(@p_MontoOperacion,0)      
		END    
	END   
	UPDATE OrdenInversion      
	SET CodigoMnemonicoReporte = @p_CodigoMnemonicoReporte,FechaLiquidacion=@p_FechaLiquidacion,MontoNominalOrdenado=@p_MontoNominalOrdenado,
	MontoNominalOperacion=@p_MontoNominalOperacion,CodigoTipoCupon=@p_CodigoTipoCupon,YTM=@p_YTM,PrecioNegociacionLimpio=@p_PrecioNegociacionLimpio,
	PrecioNegociacionSucio=@p_PrecioNegociacionSucio,MontoOperacion=@p_MontoOperacion,Observacion=@p_Observacion,Plazo=@p_Plazo,PTasa=@p_PTasa,
	InteresAcumulado=@p_InteresAcumulado,InteresCastigado=@p_InteresCastigado,TasaCastigo=@p_TasaCastigo,MontoPreCancelar=@p_MontoPreCancelar,
	InteresCorridoNegociacion=@p_InteresCorridoNegociacion,PorcentajeAcciones=@p_PorcentajeAcciones,PorcentajeDolares=@p_PorcentajeDolares,
	PorcentajeBonos=@p_PorcentajeBonos,CantidadValor=@p_CantidadValor,MontoContado=@p_MontoContado,MontoPlazo=@p_MontoPlazo,Precio=@p_Precio,
	TipoCambio=@p_TipoCambio,MontoOrigen=@p_MontoOrigen,MontoDestino=@p_MontoDestino,TipoCobertura=@p_TipoCobertura,FechaPago=@p_FechaPago,
	ContadoSoles=@p_ContadoSoles,TipoCambioSpot=@p_TipoCambioSpot,ContadoDolares=@p_ContadoDolares,PlazoSoles=@p_PlazoSoles,TipoCambioForw=@p_TipoCambioForw,
	PlazoDolares=@p_PlazoDolares,PorcentajeRendimiento=@p_PorcentajeRendimiento,TipoCambioFuturo=@p_TipoCambioFuturo,CodigoMoneda=@p_CodigoMoneda,
	CodigoMonedaDestino=@p_CodigoMonedaDestino,CodigoPreOrden=@p_CodigoPreOrden,UsuarioModIFicacion=@p_UsuarioModIFicacion,HoraModIFicacion=@p_HoraModIFicacion,
	TotalComisiones=@p_TotalComisiones,PrecioPromedio=@p_PrecioPromedio,MontoNetoOperacion=@p_MontoNetoOperacion,Situacion=@p_Situacion,Delibery=@p_Delibery,
	Host=@p_Host,CodigoTercero=@p_CodigoTercero,FechaModIFicacion=@p_FechaModIFicacion,CodigoContacto=@p_CodigoContacto,CantidadOrdenado=@p_CantidadOrdenado,
	CantidadOperacion=@p_CantidadOperacion,NumeroPoliza = @p_NumeroPoliza,CodigoUsuario = @p_CodigoUsuario,HoraOperacion = @p_HoraOperacion,CodigoGestor = @p_CodigoGestor,
	CodigoMonedaOrigen = @p_CodigoMonedaOrigen,DIFerencial = @p_DIFerencial,CodigoMotivo = @p_CodigoMotivo,MontoCancelar = @p_MontoCancelar,
	CodigoTipoTitulo = @p_CodigoTipoTitulo,CategoriaInstrumento = @p_CategoriaInstrumento,FechaContrato = @p_FechaContrato,OR_CantidadRV = @p_OR_CantidadRV,
	OR_CantidadNominal = @p_OR_CantidadNominal,OR_CantidadRF = @p_OR_CantidadRF,TasaPorcentaje = @p_TasaPorcentaje,TipoFondo = @p_TipoFondo,FechaTrato = @p_FechaTrato,
	Estado = @p_Estado,PrecioCalculado = @p_PrecioCalculado,InteresCorrido = @p_InteresCorrido,
	CodigoMotivoCambio = case when @p_CodigoMotivoCambio is null then CodigoMotivoCambio ELSE @p_CodigoMotivoCambio end,IndicaCambio = @p_IndicaCambio,
	AfectaFlujoCaja = @p_AfectaFlujoCaja,CodigoPlaza = @p_CodigoPlaza,Fixing = @p_Fixing,GrupoIntermediario = @p_GrupoIntermediario,MontoPrima =@p_montoPrima,
	TipoTramo = @p_tipotramo,Renovacion = @p_Renovacion,TipoMonedaForw = @p_TipoMonedaForw,RegulaSBS = @p_RegulaSBS,MedioNegociacion = @p_MedioNegociacion,
	HoraEjecucion = @p_HoraEjecucion,VencimientoAno = @p_VencimientoAno,VencimientoMes = @p_VencimientoMes,TipoCondicion = @p_TipoCondicion,
	TipoValorizacion = @p_TipoValorizacion, TirNeta = @p_TirNeta,
	ObservacionCarta = @p_ObservacionCarta
	WHERE CodigoOrden = @p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
	IF @p_CategoriaInstrumento = 'DP'      
	BEGIN      
		DECLARE @m_CodigoSBSEmisor VARCHAR(4)
		SELECT @m_CodigoSBSEmisor = CodigoSBS FROM Entidad WHERE CodigoTercero = @p_CodigoTercero
		UPDATE OrdenInversion SET 
		CodigoSBS =  CASE WHEN (LEN(CodigoSBS)-6) < 1 THEN '' ELSE  substring(CodigoSBS,1,2) + @m_CodigoSBSEmisor + substring(CodigoSBS,7, len(CodigoSBS)-6) END
		WHERE CodigoOrden = @p_CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS
	END
	DECLARE  @CodigoCustodio VARCHAR(12)
	SELECT @CodigoCustodio = dbo.ObtenerCustodio(@p_CodigoPortafolioSBS,@Nemonico,@p_CodigoTercero)   
	EXEC GeneraSaldosCustodioPorNemonico @p_FechaOperacion,@Nemonico,@CodigoCustodio,@p_CodigoPortafolioSBS,@p_UsuarioModIFicacion,
	@p_FechaModIFicacion,@p_HoraModIFicacion,@p_Host
	IF @p_IndicaCambio = '1'
	BEGIN
		SET @CambioTrz = ''
		IF @p_CantidadOperacion <> @CantidadOperacion	SET @CambioTrz = '1'
		IF @p_CantidadOrdenado <> @CantidadOrdenado	SET @CambioTrz = '1'
		IF @p_PrecioNegociacionLimpio <> @PrecioNegociacionLimpio	SET @CambioTrz = '1'
		IF @p_Precio <> @Precio	SET @CambioTrz = '1'
		IF @p_MontoCancelar <> @MontoCancelar	SET @CambioTrz = '1'
		IF @p_MontoNominalOperacion <> @MontoNominalOperacion	SET @CambioTrz = '1'
		IF @p_MontoOperacion <> @MontoOperacion	SET @CambioTrz = '1'
		IF @CambioTrz = '1'
		BEGIN
			Declare @Cantidad NUMERIC(22,7),@Asignacion NUMERIC(22,7),@CodigoPrevOrden NUMERIC(12),@Correlativo NUMERIC(12,0),@ModoIngreso VARCHAR(1)
			SELECT distinct @Correlativo = Correlativo, @CodigoPrevOrden = p.CodigoPrevOrden
			FROM PrevOrdenInversion p
			inner join PrevOrdenInversion_OI poi on p.CodigoPrevOrden = poi.CodigoPrevOrden
			WHERE poi.CodigoOrden = @p_CodigoOrden
			SET @Correlativo = isnull(@Correlativo, 0)
			SET @CodigoPrevOrden = isnull(@CodigoPrevOrden, 0)
			SET @ModoIngreso = case when @Correlativo = 0 then '2' ELSE '1' end
			SET @Cantidad = case when @p_CantidadOperacion=0 then @p_CantidadOrdenado ELSE ISNULL(@p_CantidadOperacion, @p_CantidadOrdenado) end
			SET @Precio = case when @p_PrecioNegociacionLimpio=0 then @p_Precio ELSE ISNULL(@p_PrecioNegociacionLimpio, @p_Precio) end
			SET @Cantidad = case @p_CategoriaInstrumento when 'FD' then @p_MontoCancelar when 'DP' then @p_MontoNominalOperacion when 'CV' then @p_MontoOperacion ELSE @Cantidad end
			SET @Asignacion = @Cantidad
			EXEC dbo.pr_SIT_ins_TrazabilidadOperaciones_Insertar_sura @p_FechaOperacion,@Correlativo,@p_Estado,'2',@CodigoPrevOrden,@p_CodigoOrden,@Nemonico,
			@p_CodigoOperacion,@Cantidad,@Precio,@p_CodigoTercero,@Cantidad,@Precio,@p_CodigoPortafolioSBS,@Asignacion,@ModoIngreso,'11',@p_CodigoMotivoCambio,'',
			@p_UsuarioModIFicacion,@p_FechaModIFicacion,@p_HoraModIFicacion,@p_Host
		END
	END
END
ELSE
BEGIN
	UPDATE PreOrdenInversion
	SET CodigoMnemonicoReporte = @p_CodigoMnemonicoReporte,FechaLiquidacion=@p_FechaLiquidacion,MontoNominalOrdenado=@p_MontoNominalOrdenado,
	MontoNominalOperacion=@p_MontoNominalOperacion,CodigoTipoCupon=@p_CodigoTipoCupon,YTM=@p_YTM,PrecioNegociacionLimpio=@p_PrecioNegociacionLimpio,
	PrecioNegociacionSucio=@p_PrecioNegociacionSucio,MontoOperacion=@p_MontoOperacion,Observacion=@p_Observacion,Plazo=@p_Plazo,PTasa=@p_PTasa,
	InteresAcumulado=@p_InteresAcumulado,InteresCastigado=@p_InteresCastigado,TasaCastigo=@p_TasaCastigo,MontoPreCancelar=@p_MontoPreCancelar,
	InteresCorridoNegociacion=@p_InteresCorridoNegociacion,PorcentajeAcciones=@p_PorcentajeAcciones,PorcentajeDolares=@p_PorcentajeDolares,
	PorcentajeBonos=@p_PorcentajeBonos,CantidadValor=@p_CantidadValor,MontoContado=@p_MontoContado,MontoPlazo=@p_MontoPlazo,Precio=@p_Precio,TipoCambio=@p_TipoCambio,
	MontoOrigen=@p_MontoOrigen,MontoDestino=@p_MontoDestino,TipoCobertura=@p_TipoCobertura,FechaPago=@p_FechaPago,ContadoSoles=@p_ContadoSoles,
	TipoCambioSpot=@p_TipoCambioSpot,ContadoDolares=@p_ContadoDolares,PlazoSoles=@p_PlazoSoles,TipoCambioForw=@p_TipoCambioForw,PlazoDolares=@p_PlazoDolares,
	PorcentajeRendimiento=@p_PorcentajeRendimiento,TipoCambioFuturo=@p_TipoCambioFuturo,CodigoMoneda=@p_CodigoMoneda,CodigoMonedaDestino=@p_CodigoMonedaDestino,
	UsuarioModIFicacion=@p_UsuarioModIFicacion,HoraModIFicacion=@p_HoraModIFicacion,TotalComisiones=@p_TotalComisiones,PrecioPromedio=@p_PrecioPromedio,
	MontoNetoOperacion=@p_MontoNetoOperacion,Situacion=@p_Situacion,Delibery=@p_Delibery,Host=@p_Host,CodigoTercero=@p_CodigoTercero,FechaModIFicacion=@p_FechaModIFicacion,
	CodigoPortafolioSBS=@p_CodigoPortafolioSBS,CodigoContacto=@p_CodigoContacto,CodigoOperacion=@p_CodigoOperacion,CantidadOrdenado=@p_CantidadOrdenado,
	CantidadOperacion=@p_CantidadOperacion,NumeroPoliza = @p_NumeroPoliza,CodigoUsuario = @p_CodigoUsuario,HoraOperacion = @p_HoraOperacion,CodigoGestor = @p_CodigoGestor,
	CodigoMonedaOrigen = @p_CodigoMonedaOrigen,DIFerencial = @p_DIFerencial,CodigoMotivo = @p_CodigoMotivo,MontoCancelar = @p_MontoCancelar,
	CodigoTipoTitulo = @p_CodigoTipoTitulo,CategoriaInstrumento = @p_CategoriaInstrumento,FechaContrato = @p_FechaContrato,OR_CantidadRV = @p_OR_CantidadRV,
	OR_CantidadNominal = @p_OR_CantidadNominal,OR_CantidadRF = @p_OR_CantidadRF,TasaPorcentaje = @p_TasaPorcentaje,TipoFondo = @p_TipoFondo,FechaTrato = @p_FechaTrato,
	Estado = @p_Estado,PrecioCalculado = @p_PrecioCalculado,InteresCorrido = @p_InteresCorrido,AfectaFlujoCaja = @p_AfectaFlujoCaja,CodigoPlaza = @p_CodigoPlaza,
	Fixing = @p_Fixing,GrupoIntermediario = @p_GrupoIntermediario,TipoTramo = @p_tipotramo,HoraEjecucion = @p_HoraEjecucion
	WHERE CodigoPreOrden=@p_CodigoOrden AND  CodigoPortafolioSBS = @p_CodigoPortafolioSBS
End
GO

GRANT EXECUTE ON [dbo].[OrdenInversion_ModificarOI] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[sp_SaldoNoAdministrado_DesactivarRegistros]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SaldoNoAdministrado_DesactivarRegistros') BEGIN 
	DROP PROCEDURE [dbo].[sp_SaldoNoAdministrado_DesactivarRegistros]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 31-07-2019
-- Creado por: Diego Tueros
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Desactivar registros de una fecha especifica por carga de excel - Saldos No Administrados
-------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_SaldoNoAdministrado_DesactivarRegistros]
(
	@p_Mes					varchar(10),
	@p_Anhio				varchar(10),
	@p_UsuarioModificacion	varchar(50),
	@p_FechaModificacion	numeric(8,0),
	@p_HoraModificacion		varchar(10),
	@p_Host					varchar(20)
)
AS
BEGIN

	UPDATE SaldosNoAdministrados
	SET
		Situacion = 'I',
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion,
		Host = @p_Host
	WHERE SUBSTRING(cast(fecha as varchar(10)),5,2) = @p_Mes
	AND SUBSTRING(cast(fecha as varchar(10)),1,4) = @p_Anhio

END
GO

GRANT EXECUTE ON [dbo].[sp_SaldoNoAdministrado_DesactivarRegistros] TO [rol_sit_fondos] AS [dbo]  
GO

PRINT '[sp_GrabarObservacionCarta]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GrabarObservacionCarta') BEGIN 
	DROP PROCEDURE [dbo].[sp_GrabarObservacionCarta]
END 
GO

-------------------------------------------------------------------------------------------
-- Fecha Creacion: 31-07-2019
-- Creado por: Diego Tueros
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Guarda una Observacion para un grupo de acciones, este campo
--						   se muestra en la carta agrupada de acciones
-------------------------------------------------------------------------------------------

CREATE PROCEDURE sp_GrabarObservacionCarta
(
	@p_CodigoAgrupacion	int,
	@p_Observacion	varchar(2000),
	@p_UsuarioCreacion	varchar(15),
	@p_FechaCreacion	numeric(8),
	@p_HoraCreacion	varchar(10),
	@p_Host	varchar(20)
)
AS
BEGIN

	IF(@p_CodigoAgrupacion <> 0)
	BEGIN
		IF EXISTS(SELECT * FROM ObservacionCarta where CodigoAgrupacion = @p_CodigoAgrupacion)
		BEGIN
				UPDATE ObservacionCarta SET 
						Observacion = @p_Observacion,
						usuarioModificacion = @p_UsuarioCreacion,
						FechaModificacion = @p_FechaCreacion,
						HoraModificacion = @p_HoraCreacion,
						Host = @p_Host
				WHERE CodigoAgrupacion = @p_CodigoAgrupacion
				
		END
		ELSE
		BEGIN
		
				INSERT INTO ObservacionCarta (CodigoAgrupacion,Observacion,Situacion,UsuarioCreacion,FechaCreacion,HoraCreacion,Host)
				VALUES (@p_CodigoAgrupacion,@p_Observacion,'A',@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host)
		END
	END
END
GO

GRANT EXECUTE ON [dbo].[sp_GrabarObservacionCarta] TO [rol_sit_fondos] AS [dbo]  
GO


PRINT '[sp_ObtenerObservacionCarta]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObtenerObservacionCarta') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObtenerObservacionCarta]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 31-07-2019
-- Modificado por: Diego Tueros
-- Nro. Orden de Trabajo: 12163
-- Descripcion del cambio: Obtiene la observación por grupo de acciones
-------------------------------------------------------------------------

CREATE PROCEDURE sp_ObtenerObservacionCarta
(
	@p_CodigoAgrupacion int
)
AS
BEGIN

	SELECt isnull(Observacion,'') Observacion FROM ObservacionCarta WHERE  CodigoAgrupacion = @p_CodigoAgrupacion

END
GO

GRANT EXECUTE ON [dbo].[sp_ObtenerObservacionCarta] TO [rol_sit_fondos] AS [dbo]  
GO



PRINT '[Limite_InsertarDetalleNivelLimite]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Limite_InsertarDetalleNivelLimite') BEGIN 
	DROP PROCEDURE [dbo].[Limite_InsertarDetalleNivelLimite]
END 
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregaron los campos para indicar si el detalle tiene valor de limite especifico
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 22/08/2019
--	Modificado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se agregaron los campos CodigoCaracteristicaRelacion y CodigoRelacion para el nuevo limite
---------------------------------------------------------------------------------------------------------------------------
 CREATE PROCEDURE [dbo].[Limite_InsertarDetalleNivelLimite] (
 @p_CodigoPortafolioSBS varchar(20),  
 @p_FlagTipoPorcentaje varchar(3),  
 @p_CodigoNivelLimite varchar(5),  
 @p_CodigoCaracteristica varchar(5),  
 @p_ValorCaracteristica varchar(50),  
 @p_ClaseNormativa varchar(5),  
 @p_ValorPorcentaje numeric(18,7),  
 @p_situacion varchar(1),  
 @p_UsuarioCreacion varchar(15),  
 @p_FechaCreacion numeric(8,0),  
 @p_HoraCreacion varchar(10),  
 @p_Host varchar(20),  
 @p_ValorPorcentajeM numeric(22,7) = 0, --HDG OT 65023 20120522  
 @p_ValorEspecifico varchar(1),
 @p_CodigoCaracteristicaRelacion varchar(5),  
 @p_CodigoRelacion varchar(5)
)  
AS  
  
SET NOCOUNT ON  
  
 DECLARE @CodigoLimite VARCHAR(5),  
  @Secuencial  VARCHAR(5),  
  @CodigoNivelLimite VARCHAR(5),  
  @IND INT,  
  @Tope VARCHAR(6), --HDG OT 65023 20120522  
  @BandaLimites CHAR(1) --HDG OT 65023 20120522  
   
 SELECT @CodigoLimite = CodigoLimite,  
  @Secuencial = NL.Secuencial  
 FROM   LimiteCaracteristica LC INNER JOIN NivelLimite NL ON LC.CodigoLimiteCaracteristica = NL.CodigoLimiteCaracteristica  
 WHERE  CodigoNivelLimite = @p_CodigoNivelLimite  
   
 --ini HDG OT 65023 20120522  
 SELECT @Tope = Tope  
 FROM Limite  
 WHERE CodigoLimite = @CodigoLimite  
   
 if @Tope = 'BAN'  
 begin  
  set @BandaLimites = 'B'  
 end  
 else  
 begin  
  set @p_ValorPorcentajeM = 0  
  set @BandaLimites = ''  
 end  
 --fin HDG OT 65023 20120522  
 --###UPDATE###  
 IF EXISTS (SELECT 1 FROM Limite WHERE CodigoLimite = @CodigoLimite and Replicar ='1')  
 BEGIN  
  SELECT IDENTITY(INT,1,1) IND, CodigoNivelLimite  
  INTO #NivelLimite  
  FROM NivelLimite NL  
  INNER JOIN LimiteCaracteristica LC ON NL.CodigoLimiteCaracteristica =LC.CodigoLimiteCaracteristica  
  WHERE LC.CodigoPortafolioSBS IN (SELECT CodigoPortafolio FROM fn_ListarPortafoliosUnitarios(@p_CodigoPortafolioSBS)) AND   
        LC.CodigoLimite  = @CodigoLimite AND  
        NL.Secuencial = @Secuencial  
  
  SET @IND = 1  
   
  WHILE(@IND<=(SELECT COUNT(1) FROM #NivelLimite))   
  BEGIN  
   SELECT @CodigoNivelLimite = CodigoNivelLimite  
   FROM #NivelLimite  
   WHERE IND = @IND  
  
   if @p_FlagTipoPorcentaje='A'  
   begin  
    if exists (select 1 from DetalleNivelLimite1  
       where CodigoNivelLimite = @CodigoNivelLimite  
        and CodigoCaracteristica = @p_CodigoCaracteristica  
        and ClaseNormativa = @p_ClaseNormativa)  
     update DetalleNivelLimite1  
     set ValorPorcentaje = @p_ValorPorcentaje,  
      Situacion = @p_situacion,  
      UsuarioModificacion = @p_UsuarioCreacion,  
      FechaModificacion = @p_FechaCreacion,  
      HoraModificacion = @p_HoraCreacion,  
      Host = @p_Host,  
      ValorPorcentajeM = @p_ValorPorcentajeM, --HDG OT 65023 20120522  
      BandaLimites = @BandaLimites --HDG OT 65023 20120522  
	  ,TieneValorEspecifico = @p_ValorEspecifico
     where CodigoNivelLimite=@CodigoNivelLimite  
      and CodigoCaracteristica=@p_CodigoCaracteristica  
      and ClaseNormativa=@p_ClaseNormativa  
    else  
     insert into dbo.DetalleNivelLimite1  
     (  
      CodigoNivelLimite,  
      CodigoCaracteristica,     
      ClaseNormativa,  
      ValorPorcentaje,  
      Situacion,  
      UsuarioCreacion,  
      Fechacreacion,  
      Horacreacion,  
      Host,  
      ValorPorcentajeM, --HDG OT 65023 20120522  
      BandaLimites --HDG OT 65023 20120522  
	  ,TieneValorEspecifico
     )  
     values  
     (  
      @CodigoNivelLimite ,  
      @p_CodigoCaracteristica ,     
      @p_ClaseNormativa,  
      @p_ValorPorcentaje ,  
      @p_situacion ,  
      @p_UsuarioCreacion ,  
      @p_FechaCreacion ,  
      @p_HoraCreacion ,  
      @p_Host,  
      @p_ValorPorcentajeM, --HDG OT 65023 20120522  
      @BandaLimites --HDG OT 65023 20120522  
	  ,@p_ValorEspecifico
     )  
       
   end  
  
   if @p_FlagTipoPorcentaje='D'  
   begin  
    if exists (select 1 from DetalleNivelLimite2  
       where CodigoNivelLimite = @CodigoNivelLimite  
        and CodigoCaracteristica = @p_CodigoCaracteristica  
        and ValorCaracteristica = @p_ValorCaracteristica  
        and ClaseNormativa = @p_ClaseNormativa)  
     update DetalleNivelLimite2  
     set ValorPorcentaje = @p_ValorPorcentaje,  
      Situacion = @p_situacion,  
      UsuarioModificacion = @p_UsuarioCreacion,  
      FechaModificacion = @p_FechaCreacion,  
      HoraModificacion = @p_HoraCreacion,  
      Host = @p_Host,  
      ValorPorcentajeM = @p_ValorPorcentajeM, --HDG OT 65023 20120522  
      BandaLimites = @BandaLimites --HDG OT 65023 20120522  
	  ,TieneValorEspecifico = @p_ValorEspecifico
     where CodigoNivelLimite = @CodigoNivelLimite  
      and CodigoCaracteristica = @p_CodigoCaracteristica  
      and ValorCaracteristica = @p_ValorCaracteristica  
      and ClaseNormativa = @p_ClaseNormativa  
    else  
     insert into dbo.DetalleNivelLimite2  
     (  
      CodigoNivelLimite,  
      CodigoCaracteristica,   
      ValorCaracteristica,  
      ClaseNormativa,  
      ValorPorcentaje,  
      Situacion,  
      UsuarioCreacion,  
      Fechacreacion,  
      Horacreacion,  
      Host,  
      ValorPorcentajeM, --HDG OT 65023 20120522  
      BandaLimites, --HDG OT 65023 20120522
	  TieneValorEspecifico,
	  CodigoCaracteristicaRelacion,
	  CodigoRelacion
     )  
     values  
     (  
      @CodigoNivelLimite ,  
      @p_CodigoCaracteristica ,  
      @p_ValorCaracteristica ,  
      @p_ClaseNormativa,  
      @p_ValorPorcentaje ,  
      @p_situacion ,  
      @p_UsuarioCreacion ,  
      @p_FechaCreacion ,  
      @p_HoraCreacion ,  
      @p_Host,  
      @p_ValorPorcentajeM, --HDG OT 65023 20120522  
      @BandaLimites, --HDG OT 65023 20120522  
	  @p_ValorEspecifico,
	  @p_CodigoCaracteristicaRelacion,
	  @p_CodigoRelacion
     )  
   end  
     
   SET @IND = @IND + 1  
  END  
 END
   
 ELSE  
 
 BEGIN  
  if @p_FlagTipoPorcentaje='A'  
  begin  
   --ini CMB 20130522  
   SELECT @CodigoNivelLimite = case when   
           (SELECT CodigoLimite from LimiteCaracteristica WHERE CodigoLimiteCaracteristica IN(SELECT CodigoLimiteCaracteristica FROM NivelLimite WHERE CodigoNivelLimite = @p_CodigoNivelLimite)) = '63'  
           then @p_CodigoNivelLimite  
            else @CodigoNivelLimite  
          end  
   --fin CMB 20130522  
   if exists (select 1 from DetalleNivelLimite1  
      where CodigoNivelLimite = @CodigoNivelLimite  
       and CodigoCaracteristica = @p_CodigoCaracteristica  
       and ClaseNormativa = @p_ClaseNormativa)  
  
    update DetalleNivelLimite1  
    set ValorPorcentaje = @p_ValorPorcentaje,  
     Situacion = @p_situacion,  
     UsuarioModificacion = @p_UsuarioCreacion,  
     FechaModificacion = @p_FechaCreacion,  
     HoraModificacion = @p_HoraCreacion,  
     Host = @p_Host,  
     ValorPorcentajeM = @p_ValorPorcentajeM, --HDG OT 65023 20120522  
     BandaLimites = @BandaLimites --HDG OT 65023 20120522  
	 ,TieneValorEspecifico = @p_ValorEspecifico
    where CodigoNivelLimite=@CodigoNivelLimite  
     and CodigoCaracteristica=@p_CodigoCaracteristica  
     and ClaseNormativa=@p_ClaseNormativa  
   else  
    insert into dbo.DetalleNivelLimite1  
    (  
     CodigoNivelLimite,  
     CodigoCaracteristica,     
     ClaseNormativa,  
     ValorPorcentaje,  
     Situacion,  
     UsuarioCreacion,  
     Fechacreacion,  
     Horacreacion,  
     Host,  
     ValorPorcentajeM, --HDG OT 65023 20120522  
     BandaLimites --HDG OT 65023 20120522  
	 ,TieneValorEspecifico
    )  
    values(  
     @p_CodigoNivelLimite ,  
     @p_CodigoCaracteristica ,     
     @p_ClaseNormativa,  
     @p_ValorPorcentaje ,  
     @p_situacion ,  
     @p_UsuarioCreacion ,  
     @p_FechaCreacion ,  
     @p_HoraCreacion ,  
     @p_Host,  
     @p_ValorPorcentajeM, --HDG OT 65023 20120522  
     @BandaLimites --HDG OT 65023 20120522  
	 ,@p_ValorEspecifico
    )  
  end  
  
  if @p_FlagTipoPorcentaje='D'  
  begin  
   if exists (select 1 from DetalleNivelLimite2  
      where CodigoNivelLimite = @CodigoNivelLimite  
       and CodigoCaracteristica = @p_CodigoCaracteristica  
       and ValorCaracteristica = @p_ValorCaracteristica  
       and ClaseNormativa = @p_ClaseNormativa)  
    update DetalleNivelLimite2  
    set ValorPorcentaje = @p_ValorPorcentaje,  
     Situacion = @p_situacion,  
     UsuarioModificacion = @p_UsuarioCreacion,  
     FechaModificacion = @p_FechaCreacion,  
     HoraModificacion = @p_HoraCreacion,  
     Host = @p_Host,  
     ValorPorcentajeM = @p_ValorPorcentajeM, --HDG OT 65023 20120522  
     BandaLimites = @BandaLimites, --HDG OT 65023 20120522 
	 TieneValorEspecifico = @p_ValorEspecifico,
	 CodigoCaracteristicaRelacion = @p_CodigoCaracteristicaRelacion,
	 CodigoRelacion = @p_CodigoRelacion
    where CodigoNivelLimite = @CodigoNivelLimite  
     and CodigoCaracteristica = @p_CodigoCaracteristica  
     and ValorCaracteristica = @p_ValorCaracteristica  
     and ClaseNormativa = @p_ClaseNormativa  
   else  
    insert into dbo.DetalleNivelLimite2  
    (  
     CodigoNivelLimite,  
     CodigoCaracteristica,   
     ValorCaracteristica,  
     ClaseNormativa,  
     ValorPorcentaje,  
     Situacion,  
     UsuarioCreacion,  
     Fechacreacion,  
     Horacreacion,  
     Host,  
     ValorPorcentajeM, --HDG OT 65023 20120522  
     BandaLimites, --HDG OT 65023 20120522  
	 TieneValorEspecifico,
	 CodigoCaracteristicaRelacion,
	 CodigoRelacion
    )  
    values(  
     @p_CodigoNivelLimite ,  
     @p_CodigoCaracteristica ,  
     @p_ValorCaracteristica ,  
     @p_ClaseNormativa,  
     @p_ValorPorcentaje ,  
     @p_situacion ,  
     @p_UsuarioCreacion ,  
     @p_FechaCreacion ,  
     @p_HoraCreacion ,  
     @p_Host,  
     @p_ValorPorcentajeM, --HDG OT 65023 20120522  
     @BandaLimites, --HDG OT 65023 20120522  
	 @p_ValorEspecifico,
	 @p_CodigoCaracteristicaRelacion,
	 @p_CodigoRelacion
    )  
  end  
  
 END
GO 
GRANT EXECUTE ON [dbo].[Limite_InsertarDetalleNivelLimite] TO [rol_sit_fondos] AS [dbo]  
go


PRINT '[OrdenInversion_AccionesConfirmadas]'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'OrdenInversion_AccionesConfirmadas')
	DROP PROCEDURE [dbo].[OrdenInversion_AccionesConfirmadas]
GO
CREATE PROCEDURE [dbo].[OrdenInversion_AccionesConfirmadas](    
	@p_Fondo varchar(10),        
	@p_Fecha numeric(8),
	@p_Mercado varchar(5)
)    
AS
-- ====================================================================================
-- Autor:			Karina Gomez
-- Fecha Creación:	02/08/2019
-- Proyecto:		Mandatos
-- OT: 12163
-- Descripción:		Consulta para la agrupacion de acciones 
-- ====================================================================================
-----------------------------------------------------------------------------------------
--@Test: exec OrdenInversion_AccionesConfirmadas '7',20190722,''
-----------------------------------------------------------------------------------------
BEGIN
	SELECT dbo.fn_SIT_gl_ConvertirFechaaString(OI.FechaOperacion) AS FechaOperacion,
			dbo.fn_SIT_gl_ConvertirFechaaString(OI.FechaLiquidacion) AS FechaLiquidacion,
			P.NombreCompleto As Portafolio,
			OI.CategoriaInstrumento aS TipoInstrumento,
			OI.CodigoOrden,
			OP.Descripcion as Operacion,
			CASE WHEN OI.TipoFondo in('CC_CNC','CC_SNC') THEN UPPER(TF.Nombre) ELSE TE.Descripcion END + '-' + LTRIM(OI.CodigoMnemonico) + '-' + CI.Descripcion AS 'Descripcion',
			ISNULL(OI.CantidadOperacion,0) as Cantidad,
			OI.MontoNetoOperacion as Monto,
			MO.CodigoMoneda as Moneda,
			OI.CodigoAgrupado,
			--MC.Descripcion AS  CodigoMercado
			--CASE WHEN TE.CodigoPais = '604' THEN 'LOCAL' else 'Extranjero' END AS CodigoMercado
			M.Descripcion AS CodigoMercado
	FROM  OrdenInversion OI   
	INNER JOIN Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS  
	LEFT JOIN Terceros TE ON OI.CodigoTercero = TE.CodigoTercero
	LEFT JOIN Entidad E ON TE.CodigoTercero = E.CodigoTercero AND E.Situacion = 'A'
	LEFT JOIN Mercado M ON E.CodigoMercado = M.CodigoMercado
	LEFT JOIN Moneda MO ON OI.CodigoMoneda = MO.CodigoMoneda    
	LEFT JOIN ClaseInstrumento CI ON CI.Categoria = OI.CategoriaInstrumento    
	LEFT JOIN ParametrosGenerales TF ON OI.TipoFondo = TF.Valor and TF.Clasificacion = 'TipoFondoI' --CMB OT 64769 20120327
	INNER JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion 
	LEFT JOIN ParametrosGenerales TC ON OI.CodigoTipoCupon = TC.Valor AND TC.ClasIFicacion = 'TipoTasaI'
	LEFT JOIN Plaza PL ON PL.CodigoPlaza = OI.CodigoPlaza   
	--LEFT JOIN CuentasPorCobrarPagar CCP ON CCP.CodigoOrden = OI.CodigoOrden 
	--LEFT JOIN Mercado MC ON CCP.CodigoMercado = MC.CodigoMercado 
	WHERE   OI.Estado = 'E-CON' 
			AND OI.FechaOperacion = @p_Fecha
			--AND ISNULL(FechaIDI,OI.FechaOperacion) = (CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(OI.CodigoPortafolioSBS) ELSE @p_Fecha END)
			AND OI.CodigoPortafolioSBS =(CASE WHEN ISNULL(@p_Fondo,'') = '' THEN OI.CodigoPortafolioSBS ELSE @p_Fondo END)
			AND CI.Categoria = 'AC'
			--AND (CCP.CodigoMercado = @p_Mercado OR @p_Mercado = '')
			AND (E.CodigoMercado = @p_Mercado OR @p_Mercado = '')
	ORDER BY 1,2,3,7,10,12
END

GO

GRANT EXECUTE ON [dbo].[OrdenInversion_AccionesConfirmadas] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[OrdenInversion_AgrupadoAccionesOI]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_AgrupadoAccionesOI') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_AgrupadoAccionesOI]
END 
GO
CREATE PROCEDURE [dbo].[OrdenInversion_AgrupadoAccionesOI]
@p_CodigoOrden varchar(1000),
@p_Opcion char(1),
@p_Valida varchar(200) output
AS
-- ====================================================================================
-- Autor:			Karina Gomez
-- Fecha Creación:	02/08/2019
-- Proyecto:		Mandatos
-- OT: 12163
-- Descripción:		Agrupa y desagrupa las Ordenes de Inversion
-- ====================================================================================
BEGIN

	declare @CodAgrupador int,@total int,@SinAgrupar int,@grupos int,@NuevoGrupo int
	set @p_Valida = ''
	set @CodAgrupador = (select MAX(ISNULL(CodigoAgrupado,0))+1 From OrdenInversion)
	
	select CodigoOrden,CodigoAgrupado into #tb_Agrupados
	from OrdenInversion 
	where CodigoOrden in (select splitdata from dbo.fnSplitString(@p_CodigoOrden,','))

	set @total = (select count(distinct CodigoAgrupado) from #tb_Agrupados)
	set @grupos = (select count(distinct CodigoAgrupado) from #tb_Agrupados where CodigoAgrupado is not null)
	set @NuevoGrupo = (select count(distinct CodigoAgrupado) from #tb_Agrupados where CodigoAgrupado is null)
	

	if(@p_Opcion = 'S') -- agrupa
		begin

			if(@total = @NuevoGrupo) 
				begin
					update OrdenInversion set CodigoAgrupado = @CodAgrupador 
					where CodigoOrden in (select CodigoOrden from #tb_Agrupados)
				end
			else
			begin
			
				set @SinAgrupar = (select COUNT(*) from #tb_Agrupados where CodigoAgrupado is null )
		
				if(@grupos > 1)
					begin
						set @p_Valida = 'Solo puede realizar la agrupación con un grupo existente. Favor de verificar.'
					end 
				else
					begin
						set @CodAgrupador =(select distinct CodigoAgrupado from #tb_Agrupados where CodigoAgrupado is not null)

						update OrdenInversion set CodigoAgrupado = @CodAgrupador 
						where CodigoOrden in (select CodigoOrden from #tb_Agrupados)
					end 
			end

		end
	else -- desagrupa 
		begin
		
			update OrdenInversion set CodigoAgrupado = null 
			where CodigoOrden in (select CodigoOrden from #tb_Agrupados)

		end
	
	drop table #tb_Agrupados
	return @p_Valida 

END
GO
GRANT EXECUTE ON [dbo].[OrdenInversion_AgrupadoAccionesOI] TO [rol_sit_fondos] AS [dbo]  
go


PRINT '[OrdenInversion_ValidaAgrupado]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='OrdenInversion_ValidaAgrupado') BEGIN 
	DROP PROCEDURE [dbo].[OrdenInversion_ValidaAgrupado]
END 
GO
CREATE PROCEDURE [dbo].[OrdenInversion_ValidaAgrupado]
@p_CodigoOrden varchar(10),
@p_valida int output
AS
-- ====================================================================================
-- Autor:			Karina Gomez
-- Fecha Creación:	02/08/2019
-- Proyecto:		Mandatos
-- OT: 12163
-- Descripción:		Valida si existe el grupo de accion por codigo de orden
-- ====================================================================================
BEGIN
	
	set @p_valida = 0
	
	select @p_valida=isnull(CodigoAgrupado,0) 
	from OrdenInversion 
	where CodigoOrden = @p_CodigoOrden and Situacion = 'A' AND Estado='E-CON'
	
	return @p_valida
END
GO
GRANT EXECUTE ON [dbo].[OrdenInversion_ValidaAgrupado] TO [rol_sit_fondos] AS [dbo]  
go


PRINT '[sp_SIT_Lista_Vencimientos_OR]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_Lista_Vencimientos_OR') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_Lista_Vencimientos_OR]
END 
GO
CREATE PROCEDURE [dbo].[sp_SIT_Lista_Vencimientos_OR]
@p_CodigoPortafolioSBS VarChar(20),
@p_CodigoTercero VarChar(20),
@p_FechaOperacion Numeric(8)
AS
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion			: 07/12/2016
--	Creado por				: Karina Gomez
--	Nro. Orden de Trabajo	: 12163
--  Descripcion				: Listar las Ordenes de Inversion - Vencimientos
-----------------------------------------------------------------------------------------------------------
BEGIN
-- BUSCANDO VENCIMIENTOS
	SELECT ISNULL(OI.CodDatatec,'') CodDatatec, OI.CodigoOrden,CASE WHEN OI.CodigoTipoCupon = '1' THEN 'Nominal' ELSE 'Efectiva' END CodigoTipoCupon,OI.TasaPorcentaje,OI.MontoNominalOperacion,
	OI.MontoOperacion,OI.CodigoMoneda,TT.Descripcion Nemonico
	FROM ordeninversion OI
	LEFT JOIN TipoTitulo TT ON TT.CodigoTipoTitulo = OI.CodigoMnemonico 
	WHERE CodigoPortafolioSBS =  @p_CodigoPortafolioSBS AND CategoriaInstrumento = 'OR' AND CodigoTercero = @p_CodigoTercero AND CodigoOperacion = '4' 
	AND OI.Situacion = 'A' AND OI.Estado <> 'E-ELI' AND OI.FechaOperacion =  @p_FechaOperacion AND (CodDatatec IS NULL OR CodDatatec ='')
END
GO
GRANT EXECUTE ON [dbo].[sp_SIT_Lista_Vencimientos_OR] TO [rol_sit_fondos] AS [dbo]  
go

PRINT '[sp_SIT_obt_AmpliacionBCR]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_obt_AmpliacionBCR') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_obt_AmpliacionBCR]
END 
GO
CREATE PROCEDURE [dbo].[sp_SIT_obt_AmpliacionBCR] @p_CodigoOrden VarChar(12), @p_AmpliacionBCR CHAR(1) OUT
AS
-----------------------------------------------------------------------------------------------------------
--Objetivo: Valida si la orden es una renovacion con ampliacion BCR
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 07/12/2016
--	Creado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9678
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 03/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9777
--	Descripcion del cambio: Se actualiza la longitud del codigo de operacion a 6
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Karina Gomez.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Valida si es nacional o exterior
--------------------------------------------------------------------------------------------------------------
BEGIN
	/*OT9777 INICIO */
	DECLARE @TotalConstitucion NUMERIC(22,7),@TotalCancelacion NUMERIC(22,7), @CodigoOperacion VarChar(6),
	@CodigoDestino VarChar(14),@Importe NUMERIC(22,7), @CodigoTerceroNegociacion VarChar(14)
	/*OT9777 FIN */
	SELECT @CodigoOperacion = OI.CodigoOperacion,@CodigoTerceroNegociacion = oi.CodigoTercero, @CodigoDestino = OC.CodigoTerceroDestino, @Importe = OC.Importe  
	FROM OrdenInversion OI
	JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	WHERE OI.CodigoOrden = @p_CodigoOrden
	--	
	DECLARE @vPais varchar(4), @vOrigen varchar(3)

	SELECT @vPais = CodigoPais FROM Terceros WHERE CodigoTercero = @CodigoTerceroNegociacion 
	
	IF (@vPais <> '604') 
	BEGIN
		SET @p_AmpliacionBCR = '0'
	END
	ELSE 
	
		BEGIN
		IF @CodigoDestino = '' or @CodigoTerceroNegociacion = @CodigoDestino
			SET @p_AmpliacionBCR = '0'
		ELSE
			BEGIN
				IF @CodigoOperacion = '3'
				BEGIN
					SET @TotalConstitucion = @Importe
					SELECT @TotalCancelacion = SUM(MontoNetoOperacion) FROM OrdenInversion WHERE CodigoOrden IN(
						SELECT RD.CodigoOrden FROM DPZRenovacionCabecera RC
						JOIN DPZRenovacionDetalle RD ON RD.CodigoCabecera = RC.CodigoCabecera
						WHERE RC.CodigoOrden = @p_CodigoOrden
					)
				END
				ELSE
				BEGIN
					SET @TotalCancelacion = @Importe
					SELECT @TotalConstitucion = SUM(MontoNetoOperacion) FROM OrdenInversion WHERE CodigoOrden IN (
						SELECT RD.CodigoOrden FROM DPZRenovacionCabecera RC
						JOIN DPZRenovacionDetalle RD ON RD.CodigoCabecera = RC.CodigoCabecera
						WHERE RC.CodigoOrden = @p_CodigoOrden
					)
				END
				IF ROUND(@TotalConstitucion,2) > ROUND(@TotalCancelacion,2)
					SET @p_AmpliacionBCR = '1'
				ELSE
					SET @p_AmpliacionBCR = '0'
			END
	END 
END
GO
GRANT EXECUTE ON [dbo].[sp_SIT_obt_AmpliacionBCR] TO [rol_sit_fondos] AS [dbo]  
go




PRINT 'INICIO - ACTUALIZAR DATOS TERCEROS'
UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '021-000-089', BancoDestino = 'FEDERAL RESERVE BANK NEW YORK', NumeroCuentaDestino = '026008714', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'FRNYUS33'
WHERE CodigoTercero = '7853653'
GO

UPDATE Terceros
SET SWIFT = 'CFSUPRSJ', ForFurtherCredit = '', ABA = '026009593', BancoDestino = 'BANK OF AMERICA, N.A.', NumeroCuentaDestino = '1901332103', CiudadDestino = 'MIAMI, FLORIDA', PaisDestino = 'USA', SwiftIntermediario = 'BOFAUS3M'
WHERE CodigoTercero = '55'
GO

UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '', BancoDestino = 'JPMORGAN CHASE', NumeroCuentaDestino = '464647085', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CHASUS33'
WHERE CodigoTercero = '54'
GO

UPDATE Terceros
SET SWIFT = 'MIDLPAPA', ForFurtherCredit = '', ABA = '021000089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '36322415', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CITIUS33', Direccion = '111 Wall St. New York City, NY 10043, USA'
WHERE CodigoTercero = '56'
GO

UPDATE Terceros
SET SWIFT = 'BPABKYKG', ForFurtherCredit = '', ABA = '021-000-089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '36317288', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CITIUS33'
WHERE CodigoTercero = '89'
GO

UPDATE Terceros
SET SWIFT = '', ForFurtherCredit = '', ABA = '26014627', BancoDestino = 'FEDERAL RESERVE BANK NEW YORK', NumeroCuentaDestino = '220004000800', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'FRNYUS33'
WHERE CodigoTercero = '2773311'
GO

UPDATE Terceros
SET SWIFT = 'MCTBPAPA', ForFurtherCredit = '', ABA = '021-000-018', BancoDestino = 'The Bank Of New York Mellon', NumeroCuentaDestino = '', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'IRVTUS3N', Direccion = 'One Wall Street, New York, NY 10286, USA'
WHERE CodigoTercero = '20651654516'
GO

UPDATE Terceros
SET SWIFT = 'BBVAUS33', ForFurtherCredit = '', ABA = '021-000-089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '36005487', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = 'CITIUS33'
WHERE CodigoTercero = '1345123'
GO

UPDATE Terceros
SET ForFurtherCredit = '', ABA = '021-000-089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '09250276', CiudadDestino = 'NEW YORK', PaisDestino = 'USA', SwiftIntermediario = ''
WHERE CodigoTercero = '5100'
GO

UPDATE Terceros
SET ForFurtherCredit = '', ABA = '021-000-089', BancoDestino = 'CITIBANK, N.A., NEW YORK', NumeroCuentaDestino = '10934835', CiudadDestino = '', PaisDestino = '', SwiftIntermediario = ''
WHERE CodigoTercero = '20100043140'
GO
PRINT 'FIN - ACTUALIZAR DATOS TERCEROS'




PRINT 'INICIO - SaldosBancarios_Actualizar2'

USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'SaldosBancarios_Actualizar2')
	DROP PROCEDURE [dbo].[SaldosBancarios_Actualizar2]
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Actualizar los saldos de bancos
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/10/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9515
--	Descripcion del cambio: Correcion de calculo para la monda CAD
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 24/08/2016
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 10719
--	Descripcion del cambio: Corrección del cálculo de los saldos bancarios
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: valida los saldos no administrados
------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[SaldosBancarios_Actualizar2]
	@CodigoPortafolioSBS  varchar(10) = NULL,
	@FechaOperacion numeric(8)
AS   
BEGIN
	--OT10719 - Se comentó el store procedure que calculaba nuevamente los saldos iniciales.
	--Esto se realiza en la apertura del portafolio
	--Actualiza primer el saldo inicial con respecto al dia anterior.
	--EXEC [sp_SIT_ActualizaSaldoBancoInicial] @CodigoPortafolioSBS,@FechaOperacion
		--Actuliza las compras ventas del dia.
	DECLARE	@PortafolioModificar varchar(10),@NumeroCuentaModificar varchar(25),@IngresoModificar numeric(22,7),@EgresoModificar numeric(22,7),
	@MonedaModificar varchar(10),@OperacionCaja VARCHAR(7),@EgresoCTA numeric(22,7),@SaldoIntradia numeric(22,7),@Pendiente numeric(22,7),
	@Inversion  numeric(22,7)
	CREATE TABLE #tmpsaldos1(CodigoPortafolioSBS varchar(10),NumeroCuenta varchar(25),IngresoModificar numeric(22,7),EgresoModificar numeric(22,7),
	Moneda varchar(10),EgresoCTA NUMERIC(22,7),SaldoIntradia Numeric(22,7),Pendiente Numeric(22,7), Inversion  Numeric(22,7),)
	IF ISNULL(@CodigoPortafolioSBS,'') = ''
		SET @CodigoPortafolioSBS = dbo.fn_CodPortafolioPorNombre('MULTIFONDO')
	--Actualiza los saldos de movimientos bancarios a 0.
	UPDATE SaldosBancarios SET IngresosEstimados = 0,EgresosEstimados = 0,SaldoDisponibleFinalConfirmado = SaldoDisponibleInicial , 
	SaldoContableFinalEstimado = SaldoDisponibleInicial,SaldoEstadoCTA = SaldoDisponibleInicial 
	WHERE FechaOperacion = @FechaOperacion AND CodigoPortafolioSBS = @CodigoPortafolioSBS
	--Calcula los saldos de las cuentas tomando divisa inversamente. LC 01082008
	INSERT INTO	#tmpsaldos1
	SELECT CE.CodigoPortafolioSBS,CE.NumeroCuenta,
	SUM(ISNULL((
		CASE WHEN ((oc.CodigoOperacion in ('93','94') AND ISNULL(oi.Delibery,'N') = 'S') OR (oc.CodigoOperacion IN ('93','94') AND ISNULL(oi.Delibery,'N') = 'N'))
		AND SUBSTRING(oc.CodigoOperacionCaja,1,1) <> 'V' THEN
			(CASE WHEN (ccp.Egreso = 'N') THEN  ROUND( oc.Importe,2) ELSE 0 END)
		ELSE
			(CASE WHEN (ti.egreso='N') THEN ROUND( oc.Importe ,2) ELSE 0 END)
	END),0)) as Ingreso,
	SUM(ISNULL((CASE WHEN ((oc.CodigoOperacion in ('93','94') AND ISNULL(oi.Delibery,'N') = 'S') OR (oc.CodigoOperacion IN ('93','94') AND ISNULL(oi.Delibery,'N') = 'N')) 
	AND SUBSTRING(oc.CodigoOperacionCaja,1,1) <> 'V' then
		(CASE WHEN (ccp.Egreso = 'S') THEN ROUND(oc.Importe ,2) ELSE 0 END)
	ELSE
		(CASE WHEN (ti.egreso='S') THEN ROUND( oc.Importe ,2) ELSE 0 END)
	END),0)) as Egreso,CE.CodigoMoneda,
	SUM(CASE WHEN OC.CodigoOperacion = '19' OR OC.CodigoOperacion = '20' THEN 0 ELSE
		CASE WHEN (o.CodigoTipoOperacion  = '1') THEN ROUND(oc.Importe ,2) * -1 ELSE ROUND(oc.Importe ,2) END
	END) AS EgresoCTA,
	SaldoIntradia = SUM(CASE WHEN OC.CodigoOperacion = '8' THEN ROUND(oc.Importe ,2) ELSE 0 END),
	SUM(CASE WHEN OC.CodigoOperacion = '19' OR OC.CodigoOperacion = '20' THEN 
		CASE WHEN (o.CodigoTipoOperacion  = '1') THEN ROUND(oc.Importe ,2) * -1 ELSE ROUND(oc.Importe ,2) END
	ELSE 0 END) AS Pendiente,
	SUM(CASE WHEN OC.CodigoOperacion = '10' THEN
		CASE WHEN (o.CodigoTipoOperacion  = '1') THEN ROUND(oc.Importe ,2) * -1 ELSE ROUND(oc.Importe ,2) END
	ELSE 0 END) + DBO.fn_SIT_ImporteInversionCorte(CE.NumeroCuenta ,@FechaOperacion ) AS Inversion
	FROM OperacionesCaja oc
	INNER JOIN Operacion o ON o.CodigoOperacion = oc.CodigoOperacion
	INNER JOIN TipoOperacion ti ON ti.CodigoTipoOperacion = o.CodigoTipoOperacion
	LEFT JOIN CuentasPorCobrarPagar ccp ON CCP.CodigoOrden = OC.CodigoOrden AND CCP.CodigoPortafolioSBS  = OC.CodigoPortafolioSBS
	AND CCP.FechaVencimiento = oc.fechaoperacion
	LEFT JOIN OrdenInversion oi ON OC.CodigoOperacionCaja = oi.CodigoOrden AND OI.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	JOIN CuentaEconomica CE ON CE.NumeroCuenta = OC.NumeroCuenta AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS AND CE.Situacion = 'A'
	WHERE oc.situacion = 'A' AND oc.fechaoperacion = @FechaOperacion AND (ISNULL(OC.OperacionFictizia,'') <> 'S' OR OC.CodigoOperacion = 'BCRE')
	AND (oc.CodigoPortafolioSBS IN (SELECT CodigoPortafolio FROM dbo.fn_ListarPortafoliosUnitarios(@CodigoPortafolioSBS)))
	GROUP BY CE.CodigoPortafolioSBS,CE.NumeroCuenta,CE.CodigoMoneda --9515
	DECLARE CursorActualizarSaldos CURSOR FOR
	SELECT CodigoPortafolioSBS,NumeroCuenta,IngresoModificar,EgresoModificar,Moneda,EgresoCTA,SaldoIntradia,Pendiente, Inversion 
	FROM #tmpsaldos1
	OPEN CursorActualizarSaldos
	FETCH NEXT FROM CursorActualizarSaldos INTO @PortafolioModificar,@NumeroCuentaModificar,@IngresoModificar,@EgresoModificar,
	@MonedaModificar,@EgresoCTA,@SaldoIntradia,@Pendiente,@Inversion
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--Si existe en saldos bancarios se actualiza la cuenta
		IF EXISTS (	SELECT TOP 1 1 FROM	SaldosBancarios WHERE	NumeroCuenta = @NumeroCuentaModificar AND FechaOperacion = @FechaOperacion
		AND CodigoPortafolioSBS = @PortafolioModificar AND CodigoMoneda = @MonedaModificar)
		BEGIN
			UPDATE SaldosBancarios SET IngresosEstimados = @IngresoModificar,EgresosEstimados = @EgresoModificar,
			SaldoDisponibleFinalConfirmado = SaldoDisponibleInicial + ISNULL(@IngresoModificar,0) - ISNULL(@EgresoModificar,0) - ISNULL(@Pendiente,0),
			SaldoContableFinalEstimado = SaldoDisponibleInicial + ISNULL(@IngresoModificar,0) -  ISNULL(@EgresoModificar,0)- ISNULL(@Pendiente,0),
			SaldoEstadoCTA = SaldoDisponibleInicial + ISNULL(@IngresoModificar,0) -  ISNULL(@EgresoModificar,0) - ISNULL(@SaldoIntradia,0) - ISNULL(@Inversion,0)
			WHERE NumeroCuenta = @NumeroCuentaModificar AND FechaOperacion = @FechaOperacion
			AND CodigoPortafolioSBS = @PortafolioModificar AND CodigoMoneda = @MonedaModificar
		END
		ELSE
		BEGIN
			INSERT INTO SaldosBancarios(CodigoSaldo, CodigoPortafolioSBS, CodigoMoneda, FechaOperacion,CodigoTercero, NumeroCuenta,
			SaldoDisponibleInicial, SaldoContableInicial,IngresosEstimados, EgresosEstimados, TipoIngreso, UsuarioCreacion,
			FechaCreacion, HoraCreacion,Host,SaldoDisponibleFinalConfirmado,SaldoContableFinalEstimado,SaldoEstadoCTA)
			VALUES( 1, @PortafolioModificar, @MonedaModificar, @FechaOperacion,NULL, @NumeroCuentaModificar, 0, 0,@IngresoModificar, @EgresoModificar, 'A', NULL,
			CONVERT(VARCHAR(8), GETDATE(), 112), CONVERT(VARCHAR(8), GETDATE(), 108), NULL,@IngresoModificar -  @EgresoModificar,
			@IngresoModificar - @EgresoModificar,@EgresoCTA)
		END
		FETCH NEXT FROM CursorActualizarSaldos INTO @PortafolioModificar,@NumeroCuentaModificar,@IngresoModificar,@EgresoModificar,
		@MonedaModificar,@EgresoCTA,@SaldoIntradia,@Pendiente,@Inversion
	END
	CLOSE CursorActualizarSaldos
	DEALLOCATE CursorActualizarSaldos
	DROP TABLE #tmpsaldos1
END


GO

GRANT EXECUTE ON [dbo].[SaldosBancarios_Actualizar2] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN - SaldosBancarios_Actualizar2'





PRINT 'INICIO - sp_SIT_gen_SaldosBancarios_Actualizar'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE NAME = 'sp_SIT_gen_SaldosBancarios_Actualizar')
	DROP PROCEDURE [dbo].[sp_SIT_gen_SaldosBancarios_Actualizar]
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Actualizar los saldos de bancos
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/10/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10238
--	Descripcion del cambio: Correcion de calculo para la monda CAD
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 15/08/2017
--	Modificado por: Robert Castillo
--	Nro. Orden de Trabajo: 10689
--	Descripcion del cambio: Se corrigió el cálculo de los saldos bancarios
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 26/09/2017
--	Modificado por: Ian Pastor
--	Nro. Orden de Trabajo: 10813
--	Descripcion del cambio: Ampliar parámetro @p_NumeroCuenta a VARCHAR(30)
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: valida los saldos no administrados
------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_SIT_gen_SaldosBancarios_Actualizar]
	@p_CodigoPortafolioSBS  VARCHAR(10),
	@p_NumeroCuenta  VARCHAR(30),
	@p_FechaOperacion NUMERIC(8)
AS   
BEGIN
	--OT-10689
	--EXEC [sp_SIT_ActualizaSaldoBancoInicial] @p_CodigoPortafolioSBS,@p_FechaOperacion
	
	--Actualiza las compras ventas del dia.
	DECLARE	@PortafolioModificar VARCHAR(10),@NumeroCuentaModificar VARCHAR(25),@IngresoModificar NUMERIC(22,7),
	@EgresoModificar NUMERIC(22,7),@MonedaModificar VARCHAR(10),@OperacionCaja VARCHAR(7),@EgresoCTA NUMERIC(22,7),
	@SaldoIntradia NUMERIC(22,7),@Pendiente NUMERIC(22,7),@Inversion NUMERIC(22,7)
	--
	CREATE TABLE #tmpsaldos1(
		CodigoPortafolioSBS VARCHAR(10),
		NumeroCuenta VARCHAR(25),
		IngresoModificar NUMERIC(22,7),
		EgresoModificar NUMERIC(22,7),
		Moneda VARCHAR(10),
		EgresoCTA NUMERIC(22,7),
		SaldoIntradia NUMERIC(22,7),
		Pendiente NUMERIC(22,7), 
		Inversion  NUMERIC(22,7),	
	)
	
	--Actualiza los saldos de movimientos bancarios a 0.
	UPDATE SaldosBancarios 
	SET IngresosEstimados = 0,
	EgresosEstimados = 0,
	SaldoDisponibleFinalConfirmado = SaldoDisponibleInicial,
	SaldoContableFinalEstimado = SaldoDisponibleInicial,
	SaldoEstadoCTA = SaldoDisponibleInicial 
	WHERE FechaOperacion = @p_FechaOperacion 
	AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND NumeroCuenta = @p_NumeroCuenta
	
	--Calcula los saldos de las cuentas tomando divisa inversamente. LC 01082008
	INSERT INTO	#tmpsaldos1
	SELECT CE.CodigoPortafolioSBS,
		CE.NumeroCuenta,
		SUM(ISNULL((
			CASE WHEN ((oc.CodigoOperacion in ('93','94') AND ISNULL(oi.Delibery,'N') = 'S') OR (oc.CodigoOperacion IN ('93','94') AND ISNULL(oi.Delibery,'N') = 'N'))
			AND SUBSTRING(oc.CodigoOperacionCaja,1,1) <> 'V' THEN
				(CASE WHEN (ccp.Egreso = 'N') THEN  ROUND( oc.Importe,2) ELSE 0 END)
			ELSE
				(CASE WHEN (ti.egreso='N') THEN ROUND( oc.Importe ,2) ELSE 0 END)
		END),0)) as Ingreso,
		SUM(ISNULL((CASE WHEN ((oc.CodigoOperacion in ('93','94') AND ISNULL(oi.Delibery,'N') = 'S') OR (oc.CodigoOperacion IN ('93','94') AND ISNULL(oi.Delibery,'N') = 'N')) 
		AND SUBSTRING(oc.CodigoOperacionCaja,1,1) <> 'V' then
			(CASE WHEN (ccp.Egreso = 'S') THEN ROUND(oc.Importe ,2) ELSE 0 END)
		ELSE
			(CASE WHEN (ti.egreso='S') THEN ROUND( oc.Importe ,2) ELSE 0 END)
		END),0)) as Egreso,
		CE.CodigoMoneda,
		SUM(CASE WHEN OC.CodigoOperacion = '19' OR OC.CodigoOperacion = '20' THEN 0 ELSE
			CASE WHEN (o.CodigoTipoOperacion  = '1') THEN ROUND(oc.Importe ,2) * -1 ELSE ROUND(oc.Importe ,2) END
		END) AS EgresoCTA,
		SaldoIntradia = SUM(CASE WHEN OC.CodigoOperacion = '8' THEN ROUND(oc.Importe ,2) ELSE 0 END),
		SUM(CASE WHEN OC.CodigoOperacion = '19' OR OC.CodigoOperacion = '20' THEN 
			CASE WHEN (o.CodigoTipoOperacion  = '1') THEN ROUND(oc.Importe ,2) * -1 ELSE ROUND(oc.Importe ,2) END
		ELSE 0 END) AS Pendiente,
		SUM(CASE WHEN OC.CodigoOperacion = '10' THEN
			CASE WHEN (o.CodigoTipoOperacion  = '1') THEN ROUND(oc.Importe ,2) * -1 ELSE ROUND(oc.Importe ,2) END
		ELSE 0 END) + DBO.fn_SIT_ImporteInversionCorte(CE.NumeroCuenta ,@p_FechaOperacion ) AS Inversion
	FROM OperacionesCaja oc WITH(NOLOCK)
	INNER JOIN Operacion o WITH(NOLOCK) 
		ON o.CodigoOperacion = oc.CodigoOperacion
	INNER JOIN TipoOperacion ti WITH(NOLOCK)
		ON ti.CodigoTipoOperacion = o.CodigoTipoOperacion
	LEFT JOIN CuentasPorCobrarPagar ccp WITH(NOLOCK) 
		ON CCP.CodigoOrden = OC.CodigoOrden 
		AND CCP.CodigoPortafolioSBS  = OC.CodigoPortafolioSBS
		AND CCP.FechaVencimiento = oc.fechaoperacion
	LEFT JOIN OrdenInversion oi WITH(NOLOCK)
		ON OC.CodigoOperacionCaja = oi.CodigoOrden 
		AND OI.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
	JOIN CuentaEconomica CE WITH(NOLOCK)
		ON CE.NumeroCuenta = OC.NumeroCuenta 
		AND CE.CodigoPortafolioSBS = OC.CodigoPortafolioSBS 
		AND CE.Situacion = 'A'
	WHERE oc.situacion = 'A' 
	AND oc.fechaoperacion = @p_FechaOperacion 
	AND (ISNULL(OC.OperacionFictizia,'') <> 'S' OR OC.CodigoOperacion = 'BCRE')
	AND oc.CodigoPortafolioSBS = @p_CodigoPortafolioSBS 
	AND oc.NumeroCuenta = @p_NumeroCuenta
	GROUP BY CE.CodigoPortafolioSBS,
		CE.NumeroCuenta,
		CE.CodigoMoneda
	
	--DECLARE CursorActualizarSaldos CURSOR FOR
	SELECT @PortafolioModificar = CodigoPortafolioSBS,
		@NumeroCuentaModificar = NumeroCuenta,
		@IngresoModificar = IngresoModificar,
		@EgresoModificar = EgresoModificar,
		@MonedaModificar = Moneda,
		@EgresoCTA = EgresoCTA,
		@SaldoIntradia = SaldoIntradia,
		@Pendiente = Pendiente, 
		@Inversion = Inversion 
	FROM #tmpsaldos1
	
	--Si existe en saldos bancarios se actualiza la cuenta
	IF EXISTS (	SELECT TOP 1 1 FROM	SaldosBancarios WITH(NOLOCK)
		WHERE	NumeroCuenta = @p_NumeroCuenta AND FechaOperacion = @p_FechaOperacion
		AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS  AND CodigoMoneda = @MonedaModificar)
	BEGIN
		
		UPDATE SaldosBancarios 
		SET IngresosEstimados = @IngresoModificar,
		EgresosEstimados = @EgresoModificar,
		SaldoDisponibleFinalConfirmado = SaldoDisponibleInicial + ISNULL(@IngresoModificar,0) - ISNULL(@EgresoModificar,0) - ISNULL(@Pendiente,0),
		SaldoContableFinalEstimado = SaldoDisponibleInicial + ISNULL(@IngresoModificar,0) -  ISNULL(@EgresoModificar,0)- ISNULL(@Pendiente,0),
		SaldoEstadoCTA = SaldoDisponibleInicial + ISNULL(@IngresoModificar,0) -  ISNULL(@EgresoModificar,0) - ISNULL(@SaldoIntradia,0) - ISNULL(@Inversion,0)
		WHERE NumeroCuenta = @NumeroCuentaModificar 
		AND FechaOperacion = @p_FechaOperacion
		AND CodigoPortafolioSBS = @PortafolioModificar 
		AND CodigoMoneda = @MonedaModificar
	END
	ELSE
	BEGIN
		INSERT INTO SaldosBancarios(
			CodigoSaldo, 
			CodigoPortafolioSBS, 
			CodigoMoneda, 
			FechaOperacion,
			CodigoTercero, 
			NumeroCuenta,
			SaldoDisponibleInicial, 
			SaldoContableInicial,
			IngresosEstimados, 
			EgresosEstimados, 
			TipoIngreso, 
			UsuarioCreacion,
			FechaCreacion, 
			HoraCreacion,
			Host,
			SaldoDisponibleFinalConfirmado,
			SaldoContableFinalEstimado,
			SaldoEstadoCTA
		)
		VALUES( 
			1, 
			@PortafolioModificar, 
			@MonedaModificar, 
			@p_FechaOperacion,
			NULL, 
			@NumeroCuentaModificar, 
			0, 
			0,
			@IngresoModificar, 
			@EgresoModificar, 
			'A', 
			NULL,
			CONVERT(VARCHAR(8), GETDATE(), 112), 
			CONVERT(VARCHAR(8), GETDATE(), 108), 
			NULL,
			@IngresoModificar -  @EgresoModificar,
			@IngresoModificar - @EgresoModificar,
			@EgresoCTA)
	END
	DROP TABLE #tmpsaldos1
END


GO

GRANT EXECUTE ON [dbo].[sp_SIT_gen_SaldosBancarios_Actualizar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN - sp_SIT_gen_SaldosBancarios_Actualizar'




PRINT 'INICIO - sp_SIT_sel_CuentaEconomica_Movimientos'
USE [SIT-FONDOS]
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE NAME = 'sp_SIT_sel_CuentaEconomica_Movimientos')
	DROP PROCEDURE [dbo].[sp_SIT_sel_CuentaEconomica_Movimientos]
GO
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 26/06/2019
--	Modificado por: Diego Tueros.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Modificar la consulta de movimientos de la cuenta econonomica
---------------------------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_sel_CuentaEconomica_Movimientos]
	@CodigoPortafolioSBS VARCHAR(10),
	@FechaOperacion numeric(8),
	@NumeroCuenta VARCHAR(25)
AS
BEGIN
	EXEC SaldosBancarios_Actualizar2 @CodigoPortafolioSBS,@FechaOperacion
	SELECT Referencia = CASE WHEN ISNULL(OC.Referencia,'') = '' THEN O.Descripcion  ELSE OC.Referencia END ,
	Importe = CASE WHEN  TIO.Egreso = 'S' THEN ROUND( -1 * ISNULL(OC.Importe,0),2) ELSE ROUND(ISNULL(OC.Importe,0),2) END
	FROM OperacionesCaja OC
	JOIN Operacion O ON O.CodigoOperacion = OC.CodigoOperacion 
	JOIN TipoOperacion TIO ON TIO.CodigoTipoOperacion = O.CodigoTipoOperacion 
	WHERE OC.FechaOperacion = @FechaOperacion AND OC.CodigoPortafolioSBS = @CodigoPortafolioSBS AND OC.NumeroCuenta = @NumeroCuenta
	AND OC.Situacion = 'A'
	AND (ISNULL(OC.OperacionFictizia,'') <> 'S' OR OC.CodigoOperacion = 'BCRE')
	AND OC.Importe > 0
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_sel_CuentaEconomica_Movimientos] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN - sp_SIT_sel_CuentaEconomica_Movimientos'



PRINT 'INICIO - CuentasPorCobrar_Anular'
USE [SIT-FONDOS]
GO

IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'CuentasPorCobrar_Anular')
	DROP PROCEDURE [dbo].[CuentasPorCobrar_Anular]
GO
------------------------------------------------------------------------------------------------------------
--Objetivo: Anula las CXC
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 03/01/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9777
--	Descripcion del cambio: Se actualiza la longitud del codigo de operacion a 6
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 25/04/2017
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 10238
--	Descripcion del cambio: Se incluye el calculo para los saldos finales
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 22/08/2019
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 12163
--	Descripcion del cambio: Se elimina los vencimientos y aperturas asociadas en una renovación
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CuentasPorCobrar_Anular]
	@p_CodigoCuenta varchar(20),
	@p_CodigoPortafolio varchar(10)='',
	@p_Usuario varchar(15),
	@p_Fecha numeric(8),
	@p_Hora varchar(10)
AS
BEGIN
	/*OT9777 INICIO */
	DECLARE @TipoMovimiento varchar(20),@CodigoOrden varchar(6),@m_TipCup VARCHAR(1),@CodigoOperacion varchar(6),@nvcCodigoNemonico nvarchar(15),
	@nvcCodigoOperacion nvarchar(12),@nvcSecuenciaCuponera nvarchar(5),@nvcCodOrden nvarchar(15),@NumeroCuenta VarChar(25), @FechaOperacion Numeric(8),
	@codigoCabecera INT
	/*OT9777 FIN */
	SELECT @TipoMovimiento = TipoMovimiento,@nvcCodigoNemonico = CodigoNemonico,@nvcSecuenciaCuponera = SecuenciaCuponera,
	@nvcCodigoOperacion = CodigoOperacion,@nvcCodOrden = CodigoOrden, @NumeroCuenta = NumeroCuenta, @FechaOperacion = FechaVencimiento
	FROM CuentasPorCobrarPagar
	WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND NumeroOperacion = @p_CodigoCuenta
	IF (@nvcCodigoOperacion = '35')      
	BEGIN      
		UPDATE CuponeraNormal SET Estado = NULL
		WHERE CodigoNemonico = @nvcCodigoNemonico AND Secuencia = @nvcSecuenciaCuponera      
	END
	DELETE FROM OperacionesCaja WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND CodigoOrden = @nvcCodOrden
	UPDATE CuentasPorCobrarPagar SET Estado = NULL WHERE  NumeroOperacion = @p_CodigoCuenta AND CodigoPortafolioSBS = @p_CodigoPortafolio
	SET @codigoCabecera = (SELECT CodigoCabecera FROM DPZRenovacionCabecera WHERE CodigoOrden = @nvcCodOrden AND CodigoOperacion = '101')
	DELETE DPZRenovacionDetalle WHERE CodigoCabecera = @codigoCabecera
	DELETE DPZRenovacionCabecera WHERE CodigoCabecera = @codigoCabecera
	--Actualiza el saldo final con la logica correspondiente	
	EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,	@NumeroCuenta,@FechaOperacion --10238
END
GO

GRANT EXECUTE ON [dbo].[CuentasPorCobrar_Anular] TO [rol_sit_fondos] AS [dbo]
GO


