----------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: SCRIPT DE CREACIÓN DE LÍMITES DE MANDATOS
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Creación		: 17/04/2019
-- Modificado por			: Ian Pastor M.
-- Nro. Orden de Trabajo	: 11883
-- Descripción				: -
----------------------------------------------------------------------------------------------------------------------------------------
USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log
PRINT '[dbo].[CarteraIndirecta]'

/* Creacion de la tabla Cartera Indirecta*/
IF EXISTS (SELECT * FROM sysobjects WHERE name='CarteraIndirecta') BEGIN 
	DROP TABLE [dbo].[CarteraIndirecta]
END 
GO

CREATE TABLE CarteraIndirecta
(
	CodigoCarteraIndirecta varchar(12) PRIMARY KEY not null,
	CodigoPortafolioSBS varchar(10) not null,
	CodigoGrupoEconomico varchar(4) null,
	CodigoEntidad varchar(4) not null,
	CodigoPais varchar(10) null,
	CodigoActividadEconomica varchar(100) null,
	Rating varchar(10) null,
	Posicion decimal(22,7) null,
	Patrimonio decimal(22,7) null,
	Participacion decimal(22,7) null,
	Fecha decimal(8,0) null,
	Situacion varchar(1) null,
	UsuarioCreacion varchar(50) null,
	FechaCreacion decimal(8,0) null,
	HoraCreacion varchar(10) null,
	UsuarioModificacion varchar(50) null,
	FechaModificacion decimal(8,0) null,
	HoraModificacion varchar(10) null,
	Host varchar(20) null,
	FOREIGN KEY (CodigoPortafolioSBS) REFERENCES Portafolio(CodigoPortafolioSBS),
	FOREIGN KEY (CodigoEntidad) REFERENCES Entidad(CodigoEntidad)
)

GO

PRINT '[dbo].[ObligacionTecnica]'
/* Creacion de la tabla Obligacion Tecnica*/
IF EXISTS (SELECT * FROM sysobjects WHERE name='ObligacionTecnica') BEGIN 
	DROP TABLE [dbo].[ObligacionTecnica]
END 
GO

CREATE TABLE ObligacionTecnica
(
	CodigoObligacionTecnica varchar(12) not null,
	CodigoPortafolioSBS varchar(10) not null,
	--Descripcion varchar(40) not null,
	Fecha numeric(8,0) not null,
	Monto numeric(22,7) not null,
	Situacion varchar(1) null,
	UsuarioCreacion varchar(50) null,
	FechaCreacion numeric(8,0) null,
	HoraCreacion varchar(10) null,
	UsuarioModificacion varchar(50) null,
	FechaModificacion numeric(8,0) null,
	HoraModificacion varchar(10) null,
	Host varchar(20) null,
	Primary key (CodigoObligacionTecnica),
	FOREIGN KEY (CodigoPortafolioSBS) REFERENCES Portafolio(CodigoPortafolioSBS)
)
GO

PRINT 'ALTER TABLE dbo.CaracteristicaGrupo ALTER COLUMN Descripcion NULL'
/*Modificacion de tamaño de campo Descripcion*/
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CaracteristicaGrupo]') and upper(name) = upper('Descripcion'))
	ALTER TABLE dbo.CaracteristicaGrupo ALTER COLUMN Descripcion varchar(250)
GO  

PRINT 'ALTER TABLE dbo.VALORES ADD RatingMandato VARCHAR(10) NULL'
/*Agregado de columna RatingMandato*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VALORES]') and upper(name) = upper('RatingMandato'))
	ALTER TABLE dbo.VALORES ADD RatingMandato VARCHAR(10) NULL  
GO

PRINT 'ALTER TABLE dbo.RatingValorHistoria ADD Negocio VARCHAR(5) NULL'
/*Agregado de columna Negocio*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RatingValorHistoria]') and upper(name) = upper('Negocio'))
	ALTER TABLE dbo.RatingValorHistoria ADD Negocio VARCHAR(5) NULL  
GO

PRINT 'ALTER TABLE dbo.RatingValorHistoria ADD FechaClasificacion numeric(8,0) NULL '
/*Agregado de columna FechaClasificacion*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RatingValorHistoria]') and upper(name) = upper('FechaClasificacion'))
	ALTER TABLE dbo.RatingValorHistoria ADD FechaClasificacion numeric(8,0) NULL 
GO

PRINT 'ALTER TABLE dbo.RatingValorHistoria ADD Clasificadora VARCHAR(100) NULL '
/*Agregado de columna Clasificadora*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RatingValorHistoria]') and upper(name) = upper('Clasificadora'))
	ALTER TABLE dbo.RatingValorHistoria ADD Clasificadora VARCHAR(100) NULL 
GO

PRINT '[dbo].[LimiteFuncionesPosicion]'
/*Creacion de tabla LimiteFuncionesPosicion*/
IF EXISTS (SELECT * FROM sysobjects WHERE name='LimiteFuncionesPosicion') BEGIN 
	DROP TABLE [dbo].[LimiteFuncionesPosicion]
END 
GO

CREATE TABLE [dbo].[LimiteFuncionesPosicion](
	[Id]  int,	
	[NombreCorto] [varchar](128) NOT NULL,	
	[Descripcion] [varchar](256) NULL,
	[SQL_FINAL_CALCULO] [varchar](2048) NULL,
	[ES_PARA_AGRUPADORES] [varchar](1) NULL,
	[Situacion] [varchar](1) NULL,
	[UsuarioCreacion] [varchar](15) NULL,
	[FechaCreacion] [numeric](8, 0) NULL,
	[HoraCreacion] [varchar](10) NULL,
	[UsuarioModificacion] [varchar](15) NULL,
	[FechaModificacion] [numeric](8, 0) NULL,
	[HoraModificacion] [varchar](10) NULL,
	[Host] [varchar](20) NULL,
	constraint pk_LimiteFuncionesPosicion primary key ([Id])
)
go


PRINT '[LimiteFuncionesPosicion]'

DELETE FROM [LimiteFuncionesPosicion]
GO

INSERT INTO [LimiteFuncionesPosicion] ([Id], [NombreCorto], [Descripcion], [SQL_FINAL_CALCULO], [ES_PARA_AGRUPADORES], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion])
	SELECT
		 Id = '1'
		,NombreCorto = 'FUNC_SALDO_BANCOS_X_ENTIDAD'
		,Descripcion = 'Función para calcular los Saldos Bancos x Etidad'
		,SQL_FINAL_CALCULO = '; IF ''{FLAG_SALDO_BANCOS}'' = ''S'' AND ''{NOMBRE_VISTA}'' = ''VT_Entidad'' ' +
								' SELECT @p_Valor = @p_Valor + dbo.Limite_SaldoBancos_X_Entidad(''{CODIGO_PORTAFOLIO}'',''{VALOR_CARACTERISTICA}'',''{FECHA_PROCESO}'',''20'');'
		,ES_PARA_AGRUPADORES = 'N'

		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion = '20181030', HoraCreacion = '00:00:00'
GO

INSERT INTO [LimiteFuncionesPosicion] ([Id], [NombreCorto], [Descripcion], [SQL_FINAL_CALCULO], [ES_PARA_AGRUPADORES], [Situacion], [UsuarioCreacion], [FechaCreacion], [HoraCreacion])
	SELECT
		 Id = '2'
		,NombreCorto = 'FUNC_SALDO_BANCOS_X_GRUPO_ECONOMICO'
		,Descripcion = 'Función para calcular los Saldos Bancos x Grupo Económico'
		,SQL_FINAL_CALCULO = '; IF ''{FLAG_SALDO_BANCOS}'' = ''S'' AND ''{NOMBRE_VISTA}'' = ''VT_GrupoEconomico'' ' +
								' SELECT @p_Valor = @p_Valor + dbo.Limite_SaldoBancos_X_GrupoEconomico(''{CODIGO_PORTAFOLIO}'',''{VALOR_CARACTERISTICA}'',''{FECHA_PROCESO}'',''20'');'
		,ES_PARA_AGRUPADORES = 'N'
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion = '20181030', HoraCreacion = '00:00:00'
GO

PRINT '[ParametrosGenerales]'

UPDATE [ParametrosGenerales]
	SET Valor = 'OBLIG_TECNICAS'
where [Clasificacion]='ValBase' AND Valor = 'OBLITEC'
GO


alter table Terceros
	alter column SectorGigs varchar(10)
go

PRINT 'SECTOR_GIGS'
/***************************************************** SECTOR_GIGS **************************************************************/

DELETE FROM ParametrosGenerales WHERE Clasificacion = 'SECTOR_GIGS'
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'AGENCIA DE GOBIERNO', '901020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'AHORRO Y FINANCIACIÓN DE HIPOTECAS', '401020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'ALIMENTOS', '302020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'ARTÍCULOS TEXTILES, DE CONFECCIÓN Y BIENES DE LUJO', '252030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'AUTOMÓVILES', '251020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'BANCO SUPRANACIONAL', '902010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'BANCOS COMERCIALES', '401010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'BANCOS IMPORTADORES/EXPORTADORES', '901030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'BEBIDAS', '302010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'BIENES DE USO  DOMÉSTICO DURADERO', '252010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'BIOTECNOLOGÍA', '352010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'CARRETERAS Y FERROCARRILES', '203040', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'COMERCIO Y DISTRIBUCIÓN', '201070', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'COMPONENTES DE AUTOMÓVILES', '251010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'CONGLOMERADOS INDUSTRIALES', '201050', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'CONSTRUCCIÓN E INGENIERÍA', '201030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'CONTENEDORES Y EMBALAJE', '151030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'DESARROLLO Y ADMINISTRACIÓN DE BIENES INMOBILIARIOS', '601020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'DISTRIBUIDORES', '255010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPO DE COMUNICACIONES', '452010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPO Y PRODUCTOS RECREATIVOS', '252020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPO Y SERVICIOS MÉDICOS', '351010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPO, INSTRUMENTOS Y COMPONENTES ELECTRÓNICOS', '452030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPOS ELÉCTRICOS', '201040', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPOS TECNOLÓGICOS, ALMACENAMIENTO ELECTRÓNICO Y PERIFÉRICOS', '452020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'EQUIPOS Y SERVICIOS DE ENERGÍA', '101010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'FIDEICOMISO HIPOTECARIO DE INVERSIÓN EN BIENES INMOBILIARIOS (REIT)', '402040', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'FIDEICOMISOS HIPOTECARIOS DE INVERSIÓN EN BIENES INMOBILIARIOS (REIT)', '601010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'GOBIERNO REGIONAL /MUNICIPAL', '903010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'HERRAMIENTAS Y SERVICIOS PARA LAS CIENCIAS DE LA SALUD', '352030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'HOTELES, RESTAURANTES Y RECREACIÓN', '253010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'INDUSTRIA AERONÁUTICA Y DE DEFENSA', '201010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'INFRAESTRUCTURA DE TRANSPORTES', '203050', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'LÍNEAS AÉREAS', '203020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'MAQUINARIAS', '201060', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'MATERIALES DE CONSTRUCCIÓN', '151020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'MEDIOS DE COMUNICACIÓN', '254010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'MERCADOS DE CAPITALES', '402030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'METALES Y MINERÍA', '151040', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'MULTISERVICIOS', '551030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PETRÓLEO, GAS Y COMBUSTIBLES', '101020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTORES DE ENERGÍA INDEPENDIENTES Y DE ENERGÍA ELÉCTRICA RENOVABLE', '551050', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTOS DE CONSTRUCCIÓN', '201020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTOS DOMÉSTICOS', '303010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTOS FARMACÉUTICOS', '352020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTOS MADEREROS Y PAPELEROS', '151050', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTOS PERSONALES', '303020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PRODUCTOS QUÍMICOS', '151010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'PROVEEDORES DE SERVICIOS MÉDICOS', '351020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SEGUROS', '403010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SEMICONDUCTORES Y EQUIPO RELACIONADO', '453010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS DE CONSUMO DIVERSIFICADOS (EDUCACIÓN, CONSUMOS ESPECIALIZADOS)', '253020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS DE TECNOLOGÍA DE LA INFORMACIÓN', '451020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS DE TELECOMUNICACIONES DIVERSOS', '501010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS DE TELECOMUNICACIONES INALÁMBRICAS', '501020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS FINANCIEROS DIVERSIFICADOS', '402010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS FINANCIEROS PERSONALES', '402020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS LOGÍSTICOS Y DE TRANSPORTE AÉREO DE MERCANCÍAS', '203010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SERVICIOS PROFESIONALES', '202020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SOBERANO', '901010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SOFTWARE', '451030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SOFTWARE Y SERVICIOS DE INTERNET', '451010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SUMINISTRO DE AGUA', '551040', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SUMINISTRO DE GAS', '551020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SUMINISTRO ELÉCTRICO', '551010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'SUMINISTROS Y SERVICIOS COMERCIALES', '202010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'TABACO', '302030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'TECNOLOGÍA MÉDICOS', '351030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'TRANSPORTE MARÍTIMO', '203030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'VENTA DE ALIMENTOS Y PRODUCTOS DE PRIMERA NECESIDAD', '301010', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'VENTAS ESPECIALIZADAS', '255040', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'VENTAS MULTILÍNEA', '255030', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

INSERT INTO dbo.ParametrosGenerales (Clasificacion, Nombre, Valor, Comentario, Valor2, Orden, Valor3)
VALUES ('SECTOR_GIGS', 'VENTAS POR INTERNET Y MARKETING DIRECTO', '255020', 'PARAMETRÍA ACTIVIDAD ECONÓMICA', NULL, NULL, NULL)
GO

PRINT 'Terceros'
update Terceros set SectorGigs = '255010' where CodigoTercero = '20100128056'
update Terceros set SectorGigs = '903010' where CodigoTercero = '37833100'
update Terceros set SectorGigs = '101010' where CodigoTercero = '1111111112'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20100191921'
update Terceros set SectorGigs = '101010' where CodigoTercero = 'ADCAP'
update Terceros set SectorGigs = '255010' where CodigoTercero = '651321'
update Terceros set SectorGigs = '101010' where CodigoTercero = '511016'
update Terceros set SectorGigs = '255010' where CodigoTercero = '20100055237'
update Terceros set SectorGigs = '255010' where CodigoTercero = '16911213'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20511230188'
update Terceros set SectorGigs = '253010' where CodigoTercero = '651354'
update Terceros set SectorGigs = '101010' where CodigoTercero = '36'
update Terceros set SectorGigs = '101010' where CodigoTercero = '28102017'
update Terceros set SectorGigs = '101010' where CodigoTercero = '5002'
update Terceros set SectorGigs = '101010' where CodigoTercero = '65'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20122476309'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100130204'
update Terceros set SectorGigs = '101010' where CodigoTercero = '335'
update Terceros set SectorGigs = '101010' where CodigoTercero = '5713203734'
update Terceros set SectorGigs = '101010' where CodigoTercero = '7853653'
update Terceros set SectorGigs = '101010' where CodigoTercero = '21660205'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100047307'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100047218'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20460972621'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100030595'
update Terceros set SectorGigs = '101010' where CodigoTercero = '22102018122'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100200261'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20330401991'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20513074370'
update Terceros set SectorGigs = '101010' where CodigoTercero = '6688'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20333725071'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20101036813'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100053455'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100053455'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20108166534'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100105862'
update Terceros set SectorGigs = '101010' where CodigoTercero = '24102018113'
update Terceros set SectorGigs = '101010' where CodigoTercero = '2025970241'
update Terceros set SectorGigs = '101010' where CodigoTercero = '140317'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20516711559'
update Terceros set SectorGigs = '101010' where CodigoTercero = '54'
update Terceros set SectorGigs = '101010' where CodigoTercero = '55'
update Terceros set SectorGigs = '101010' where CodigoTercero = 'BSP'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20543681513'
update Terceros set SectorGigs = '101010' where CodigoTercero = '56'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20331285251'
update Terceros set SectorGigs = '101010' where CodigoTercero = '7018'
update Terceros set SectorGigs = '101010' where CodigoTercero = '15831072'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20101070248'
update Terceros set SectorGigs = '101010' where CodigoTercero = '100008'
update Terceros set SectorGigs = '101010' where CodigoTercero = '5102'
update Terceros set SectorGigs = '101010' where CodigoTercero = '5101'
update Terceros set SectorGigs = '101010' where CodigoTercero = '6004'
update Terceros set SectorGigs = '101010' where CodigoTercero = '6002'
update Terceros set SectorGigs = '101010' where CodigoTercero = '6003'
update Terceros set SectorGigs = '101010' where CodigoTercero = '2014'
update Terceros set SectorGigs = '101010' where CodigoTercero = '47'
update Terceros set SectorGigs = '101010' where CodigoTercero = '65999262222'
update Terceros set SectorGigs = '101010' where CodigoTercero = '352'
update Terceros set SectorGigs = '101010' where CodigoTercero = 'ISLP'
update Terceros set SectorGigs = '101010' where CodigoTercero = '3520'
update Terceros set SectorGigs = '101010' where CodigoTercero = '621354'
update Terceros set SectorGigs = '101010' where CodigoTercero = '27102017'
update Terceros set SectorGigs = '101010' where CodigoTercero = '110000'
update Terceros set SectorGigs = '101010' where CodigoTercero = '12030507'
update Terceros set SectorGigs = '101010' where CodigoTercero = '89'
update Terceros set SectorGigs = '101010' where CodigoTercero = '84177300-4'
update Terceros set SectorGigs = '101010' where CodigoTercero = '456331'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20492912132'
update Terceros set SectorGigs = '101010' where CodigoTercero = '0801201919'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20472315154'
update Terceros set SectorGigs = '101010' where CodigoTercero = '842313'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100209641'
update Terceros set SectorGigs = '253010' where CodigoTercero = '9027746'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20129497077'
update Terceros set SectorGigs = '253010' where CodigoTercero = '38'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20100137390'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20131742372'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20100220378'
update Terceros set SectorGigs = '253010' where CodigoTercero = '132536'
update Terceros set SectorGigs = '253010' where CodigoTercero = '225611567'
update Terceros set SectorGigs = '255010' where CodigoTercero = '302'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20170072465'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20100163552'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20347642551'
update Terceros set SectorGigs = '253010' where CodigoTercero = '20100218471'
update Terceros set SectorGigs = '253010' where CodigoTercero = '2013353000'
update Terceros set SectorGigs = '255010' where CodigoTercero = '20429683581'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100116635'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20100116716'
update Terceros set SectorGigs = '101010' where CodigoTercero = '274'
update Terceros set SectorGigs = '101010' where CodigoTercero = '5006'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20170879750'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20130200789'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20130098488'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20132243230'
update Terceros set SectorGigs = '255010' where CodigoTercero = '45689'
update Terceros set SectorGigs = '255010' where CodigoTercero = '6812651'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20369155360'
update Terceros set SectorGigs = '255010' where CodigoTercero = '22052018010'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20401319817'
update Terceros set SectorGigs = '255010' where CodigoTercero = '20100190797'
update Terceros set SectorGigs = '255010' where CodigoTercero = '2909'
update Terceros set SectorGigs = '255010' where CodigoTercero = '20331066703'
update Terceros set SectorGigs = '101010' where CodigoTercero = 'INTERCORP'
update Terceros set SectorGigs = '101010' where CodigoTercero = '333333333'
update Terceros set SectorGigs = '101010' where CodigoTercero = '999999999'
update Terceros set SectorGigs = '255010' where CodigoTercero = '20510992904'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20382036655'
update Terceros set SectorGigs = '101010' where CodigoTercero = '22548'
update Terceros set SectorGigs = '101010' where CodigoTercero = '888888'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20297508661'
update Terceros set SectorGigs = '101010' where CodigoTercero = '6513241'
update Terceros set SectorGigs = '253010' where CodigoTercero = '2718301'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20514900451'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20478076038'
update Terceros set SectorGigs = '101010' where CodigoTercero = '20478076381'
GO

PRINT 'OBLIG_TECNICAS' 

IF NOT EXISTS (SELECT * FROM [ValorBaseLimite] WHERE CodigoValorBase ='OBLIG_TECNICAS')

	insert into [ValorBaseLimite] (CodigoValorBase, Descripcion, SQL_CALCULO_VALOR, Situacion, UsuarioCreacion, FechaCreacion, HoraCreacion)
	values ('OBLIG_TECNICAS', 'Obligaciones TÃ©cnicas', 'set @p_resul = dbo.fn_ObtenerObligacionesTecnicas(''{FECHA_PROCESO}'',''{CODIGO_PORTAFOLIO}'')', 'A', 'ADMIN', '20181030', '00:00:00')
GO


PRINT 'LimiteCaracteristica'
UPDATE LimiteCaracteristica
SET situacion = 'I'
 where CodigoPortafolioSBS = '2666' and codigolimite not in (
'302','303','304','305','306','307','308','309'
)
GO

UPDATE LimiteCaracteristica
SET situacion = 'I'
 where CodigoPortafolioSBS = '2777' and codigolimite not in (
'302','303','304','305','306','307','308','309'
)
GO



PRINT '[dbo].[fn_ObtenerObligacionesTecnicas]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='fn_ObtenerObligacionesTecnicas') BEGIN 
	DROP FUNCTION [dbo].[fn_ObtenerObligacionesTecnicas]
END 
GO

--------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 2019-04-08
--	Creado por: Carlos Rumiche
--	Nro. Orden de Trabajo: xxxx
--	Descripcion del cambio: Se creo la función que retorna el valor Obligaciones Tecnicas
---------------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION [dbo].[fn_ObtenerObligacionesTecnicas]
(
	 @p_Fecha numeric(8) --= '20181231'
	,@p_CodigoPortafolio varchar(20) --= '2666'
)
returns numeric(22,7)
as
begin
	declare @v_resul as numeric(22,7)

	set @v_resul = isnull((
							SELECT top 1 Monto
							FROM ObligacionTecnica
							WHERE Fecha <= @p_Fecha and CodigoPortafolioSBS = @p_CodigoPortafolio and Situacion = 'A'
							ORDER BY Fecha desc
							), 0)
	
	return @v_resul		
end
GO

GRANT EXECUTE ON [dbo].[fn_ObtenerObligacionesTecnicas] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Limite_SaldoBancos_X_Entidad]'
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Limite_SaldoBancos_X_Entidad]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Limite_SaldoBancos_X_Entidad]
GO
------------------------------------------------------------------------------------------------------------------------
-- OBJETIVO: Obtener los saldos bancarios de un fondo por emisor y fecha.
------------------------------------------------------------------------------------------------------------------------
--	Fecha de Modificación:	04/04/2019
--	Modificado por:			Ian Pastor M.
--	Nro. Orden de Trabajo:	11883
--	Descripción del cambio:	Creación
------------------------------------------------------------------------------------------------------------------------
--SELECT dbo.Limite_SaldoBancos_X_Entidad('2666','ITBK',20181231,'20')
CREATE FUNCTION [dbo].[Limite_SaldoBancos_X_Entidad]
(
	@p_CodigoPortafolio VARCHAR(10),
	@p_CodigoEntidad VARCHAR(10),
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
	--AND SB.FechaOperacion NOT IN (SELECT Fecha FROM Feriado WHERE CodigoMercado = '1' AND Situacion = 'A')  
	AND SB.FechaOperacion <= @p_Fecha  
	)  

	SELECT @p_saldo = SUM(DBO.sp_SIT_OBT_CambioMoneda(@p_CodigoPortafolio,SB.SaldoDisponibleFinalConfirmado,sb.FechaOperacion,CE.CodigoMoneda,P.CodigoMoneda))  
	FROM SaldosBancarios (NOLOCK) SB
	JOIN CuentaEconomica CE ON CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND SB.NumeroCuenta = CE.NumeroCuenta  AND SB.CodigoMoneda = CE.CodigoMoneda  
	AND CE.CodigoClaseCuenta = @p_CodigoClaseCuenta AND CE.Situacion = 'A'
	JOIN Portafolio P ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS  
	--JOIN VectorTipoCambio VTC ON VTC.CodigoMoneda = CE.CodigoMoneda AND VTC.Fecha = SB.FechaOperacion  
	--							AND  VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,CE.CodigoMoneda,sb.FechaOperacion) 
	--JOIN VectorTipoCambio VTCP ON VTCP.CodigoMoneda = P.CodigoMoneda AND VTCP.Fecha = SB.FechaOperacion  
	--							AND  VTCP.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,P.CodigoMoneda,sb.FechaOperacion) 
	WHERE SB.CODIGOPORTAFOLIOSBS = @p_CodigoPortafolio AND SB.FechaOperacion = @p_FechaSaldo AND CE.EntidadFinanciera = @p_CodigoEntidad

	return isnull(@p_saldo,0)

END
GO

GRANT EXECUTE ON [dbo].[Limite_SaldoBancos_X_Entidad] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Limite_SaldoBancos_X_GrupoEconomico]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Limite_SaldoBancos_X_GrupoEconomico]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Limite_SaldoBancos_X_GrupoEconomico]
GO
------------------------------------------------------------------------------------------------------------------------
-- OBJETIVO: Obtener los saldos bancarios de un fondo por emisor y fecha.
------------------------------------------------------------------------------------------------------------------------
--	Fecha de Modificación:	04/04/2019
--	Modificado por:			Ian Pastor M.
--	Nro. Orden de Trabajo:	11883
--	Descripción del cambio:	Creación
------------------------------------------------------------------------------------------------------------------------
--SELECT dbo.Limite_SaldoBancos_X_GrupoEconomico('2666','90',20181231,'20')
CREATE FUNCTION [dbo].[Limite_SaldoBancos_X_GrupoEconomico]
(
	@p_CodigoPortafolio VARCHAR(10),
	@p_CodigoGrupoEconomico VARCHAR(10),
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
	--AND SB.FechaOperacion NOT IN (SELECT Fecha FROM Feriado WHERE CodigoMercado = '1' AND Situacion = 'A')  
	AND SB.FechaOperacion <= @p_Fecha  
	)  

	SELECT @p_saldo = SUM(DBO.sp_SIT_OBT_CambioMoneda(@p_CodigoPortafolio,SB.SaldoDisponibleFinalConfirmado,sb.FechaOperacion,CE.CodigoMoneda,P.CodigoMoneda))  
	FROM SaldosBancarios (NOLOCK) SB
	JOIN CuentaEconomica CE ON CE.CodigoPortafolioSBS = SB.CodigoPortafolioSBS AND SB.NumeroCuenta = CE.NumeroCuenta  AND SB.CodigoMoneda = CE.CodigoMoneda  
		AND CE.CodigoClaseCuenta = @p_CodigoClaseCuenta AND CE.Situacion = 'A'
	JOIN Portafolio P ON P.CodigoPortafolioSBS = SB.CodigoPortafolioSBS
	JOIN Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera
	--JOIN VectorTipoCambio VTC ON VTC.CodigoMoneda = CE.CodigoMoneda AND VTC.Fecha = SB.FechaOperacion  
	--							AND  VTC.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,CE.CodigoMoneda,sb.FechaOperacion) 
	--JOIN VectorTipoCambio VTCP ON VTCP.CodigoMoneda = P.CodigoMoneda AND VTCP.Fecha = SB.FechaOperacion  
	--							AND  VTCP.EntidadExt = dbo.RetornarSecuenciaFuenteVTC(@p_CodigoPortafolio,P.CodigoMoneda,sb.FechaOperacion) 
	WHERE SB.CODIGOPORTAFOLIOSBS = @p_CodigoPortafolio AND SB.FechaOperacion = @p_FechaSaldo AND E.CodigoGrupoEconomico = @p_CodigoGrupoEconomico

	return isnull(@p_saldo,0)

END
GO

GRANT EXECUTE ON [dbo].[Limite_SaldoBancos_X_GrupoEconomico] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_ObliacionTecnica_ValidarRegistro]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObliacionTecnica_ValidarRegistro') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObliacionTecnica_ValidarRegistro]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 06-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Validar si existe registro de Obligacion Tecnica
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_ObliacionTecnica_ValidarRegistro]
(
	@p_ObligacionTecnica varchar(12) = null,
	@p_Portafolio varchar(10),	-- Código portafolio
	@p_Fecha decimal(8,0), --Fecha de obligacion tecnica
	@p_Tipo char(1)
)
AS
BEGIN
	IF(@p_Tipo = 'N')
	BEGIN
		SELECT 
			CodigoObligacionTecnica,
			CodigoPortafolioSBS,
			Fecha,
			Monto
		FROM ObligacionTecnica
		WHERE CodigoPortafolioSBS = @p_Portafolio
		AND Fecha = @p_Fecha
		AND Situacion = 'A'
	END
	IF(@p_Tipo = 'E')
	BEGIN
		SELECT 
			CodigoObligacionTecnica,
			CodigoPortafolioSBS,
			Fecha,
			Monto
		FROM ObligacionTecnica
		WHERE CodigoPortafolioSBS = @p_Portafolio
		AND Fecha = @p_Fecha
		AND Situacion = 'A'
		AND CodigoObligacionTecnica <> @p_ObligacionTecnica
	END
END
GO

GRANT EXECUTE ON [dbo].[sp_ObliacionTecnica_ValidarRegistro] TO [rol_sit_fondos] AS [dbo]
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[sp_ObligacionTecnica_DesactivarRegistros]'

IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObligacionTecnica_DesactivarRegistros') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObligacionTecnica_DesactivarRegistros]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 07-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Desactivar registros de una fecha especifica por carga de excel - Obligaciones Tecnicas
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_ObligacionTecnica_DesactivarRegistros]
(
	@p_Fecha				numeric(8,0),
	@p_UsuarioModificacion	varchar(50),
	@p_FechaModificacion	numeric(8,0),
	@p_HoraModificacion		varchar(10),
	@p_Host					varchar(20)
)
AS
BEGIN

	UPDATE ObligacionTecnica
	SET
		Situacion = 'I',
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion,
		Host = @p_Host
	WHERE Fecha = @p_Fecha

END
GO

GRANT EXECUTE ON [dbo].[sp_ObligacionTecnica_DesactivarRegistros] TO [rol_sit_fondos] AS [dbo]
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[sp_ObligacionTecnica_Eliminar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObligacionTecnica_Eliminar') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObligacionTecnica_Eliminar]
END 
GO

-- ================================================
-- Autor:		Diego tueros
-- Fecha Creación:	08/03/2019
-- Descripción:		Elimina un regitro de la tabla 'ObligacionTecnica' que consiste en actualizar
-- el atributo 'Situacion' con el valor 'I' (Inactivo)
-- ================================================

CREATE PROCEDURE [dbo].[sp_ObligacionTecnica_Eliminar]
(
	@p_CodigoObligacionTecnica varchar(12),	-- Código Obligacion Tecnica
	@p_UsuarioModificacion varchar(15),	-- Usuario que eliminó el registro
	@p_FechaModificacion numeric(8, 0),	-- Fecha de eliminación del registro
	@p_HoraModificacion varchar(10)	,	-- Hora de eliminación del registro
	@p_Host varchar(20)
)
AS
BEGIN
	
	SET NOCOUNT ON
	UPDATE
		[ObligacionTecnica]
	SET
		[Situacion] = 'I',
		[Host] = @p_Host,
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion

WHERE
	[CodigoObligacionTecnica] = @p_CodigoObligacionTecnica
END
GO

GRANT EXECUTE ON [dbo].[sp_ObligacionTecnica_Eliminar] TO [rol_sit_fondos] AS [dbo]
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

PRINT '[dbo].[sp_ObligacionTecnica_Insertar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObligacionTecnica_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObligacionTecnica_Insertar]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 06-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Grabar Obligaciones Tecnicas
-------------------------------------------------------------------------  


CREATE PROCEDURE [dbo].[sp_ObligacionTecnica_Insertar]
(
	@p_CodigoPortafolio varchar(10), 
	@p_Fecha numeric(8,0), 
	@p_Monto numeric(22,7), 
	@p_UsuarioCreacion varchar(50) = '', 
	@p_FechaCreacion numeric(8,0) = null, 
	@p_HoraCreacion varchar(10) = '', 
	@p_Host varchar(20) = ''
) 
AS 
BEGIN
	declare @p_CodigoObligacionTecnica as varchar(12)
	
	if ((select count(*) from ObligacionTecnica) = 0)
		set @p_CodigoObligacionTecnica = '1'
	else
		set @p_CodigoObligacionTecnica =  (select max(convert(int,CodigoObligacionTecnica)) from ObligacionTecnica) + 1
	SET NOCOUNT ON 

	INSERT INTO ObligacionTecnica
	(
		CodigoObligacionTecnica,
		CodigoPortafolioSBS,
		Fecha,
		Monto,
		Situacion,
		UsuarioCreacion,
		FechaCreacion,
		HoraCreacion,
		Host
	)
	VALUES
	(
		@p_CodigoObligacionTecnica,
		@p_CodigoPortafolio,
		@p_Fecha,
		@p_Monto,
		'A',
		@p_UsuarioCreacion,
		@p_FechaCreacion,
		@p_HoraCreacion,
		@p_Host
	)

END
GO

GRANT EXECUTE ON [dbo].[sp_ObligacionTecnica_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_ObligacionTecnica_InsertarExcel]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObligacionTecnica_InsertarExcel') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObligacionTecnica_InsertarExcel]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 07-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Inserta los registros cargados del excel
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_ObligacionTecnica_InsertarExcel]
(
	@p_Fecha	numeric(8,0),
	@p_Portafolio	varchar(10),
	@p_Monto	numeric(22,7),
	@p_UsuarioCreacion	varchar(50),
	@p_FechaCreacion	numeric(8,0),
	@p_HoraCreacion		varchar(10),
	@p_Host		varchar(20)
)
AS
BEGIN
	
	if not exists (select CodigoObligacionTecnica from ObligacionTecnica where CodigoPortafolioSBS = @p_Portafolio
					AND Fecha = @p_Fecha and Situacion = 'A')
	BEGIN

		DECLARE @p_CodigoObligacionTecnica as varchar(12)
	
		if ((select count(*) from ObligacionTecnica) = 0)
			set @p_CodigoObligacionTecnica = '1'
		else
			set @p_CodigoObligacionTecnica =  (select max(convert(int,CodigoObligacionTecnica)) from ObligacionTecnica) + 1
		
		SET NOCOUNT ON 

		INSERT INTO ObligacionTecnica
		(
			CodigoObligacionTecnica,
			CodigoPortafolioSBS,
			Fecha,
			Monto,
			Situacion,
			UsuarioCreacion,
			FechaCreacion,
			HoraCreacion,
			Host
		)
		VALUES
		(
			@p_CodigoObligacionTecnica,
			@p_Portafolio,
			@p_Fecha,
			@p_Monto,
			'A',
			@p_UsuarioCreacion,
			@p_FechaCreacion,
			@p_HoraCreacion,
			@p_Host
		)
			
	END

END
GO

GRANT EXECUTE ON [dbo].[sp_ObligacionTecnica_InsertarExcel] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_ObligacionTecnica_Modificar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ObligacionTecnica_Modificar') BEGIN 
	DROP PROCEDURE [dbo].[sp_ObligacionTecnica_Modificar]
END 
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------
-- Fecha Creacion: 07-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Actualizar Obligaciones Tecnicas
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_ObligacionTecnica_Modificar]
(
	@p_CodigoObligacionTecnica	varchar(12),
	@p_CodigoPortafolioSBS		varchar(10),
	@p_Fecha					numeric(8,0),
	@p_Monto					numeric(22,7), 
	@p_UsuarioModificacion		varchar(50),
	@p_FechaModificacion		numeric(8,0),
	@p_HoraModificacion			varchar(10),
	@p_Host						varchar(20)
)
AS
BEGIN
	
	UPDATE ObligacionTecnica
	SET
		CodigoPortafolioSBS = @p_CodigoPortafolioSBS,
		Fecha = @p_Fecha,
		Monto = @p_Monto,
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion,
		Host = @p_Host
	WHERE CodigoObligacionTecnica = @p_CodigoObligacionTecnica

END
GO

GRANT EXECUTE ON [dbo].[sp_ObligacionTecnica_Modificar] TO [rol_sit_fondos] AS [dbo]
GO



---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

PRINT '[dbo].[SP_ObligacionTecnica_SeleccionarPorFiltro]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='SP_ObligacionTecnica_SeleccionarPorFiltro') BEGIN 
	DROP PROCEDURE [dbo].[SP_ObligacionTecnica_SeleccionarPorFiltro]
END 
GO


-------------------------------------------------------------------------
-- Fecha Creacion: 05-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Listar Obligaciones Tecnicas
-------------------------------------------------------------------------  

CREATE PROCEDURE SP_ObligacionTecnica_SeleccionarPorFiltro
(
	@p_CodObligacionTecnica varchar(12),
	@p_CodPortafolio varchar(10),
	@p_Descripcion varchar(40),
	@p_Fecha decimal(8,0),
	@p_Tipo varchar(10)
)
AS
BEGIN
	
	SET NOCOUNT ON

	IF len(@p_Tipo) = 0
	BEGIN
		SELECT 
			a.CodigoObligacionTecnica,
			a.CodigoPortafolioSBS,
			a.Fecha,
			a.Monto,
			a.Situacion,
			a.UsuarioCreacion,
			a.FechaCreacion,
			a.HoraCreacion,
			a.UsuarioModificacion,
			a.FechaModificacion,
			a.HoraModificacion,
			a.Host,
			b.Descripcion,
			FechaFormato= dbo.fn_SIT_gl_ConvertirFechaaString(Fecha)
		FROM ObligacionTecnica a
		INNER JOIN Portafolio b
		ON a.CodigoPortafolioSBS = b.CodigoPortafolioSBS
		WHERE
		a.CodigoObligacionTecnica = (case when len(@p_CodObligacionTecnica) = 0 then a.CodigoObligacionTecnica else @p_CodObligacionTecnica end )  and
		a.CodigoPortafolioSBS = (case when len(@p_CodPortafolio) = 0 then a.CodigoPortafolioSBS else @p_CodPortafolio end )  and
		a.Fecha = (case when @p_Fecha= 0 then Fecha else  @p_Fecha end ) and
		b.Descripcion like (case when len(@p_Descripcion) = 0 then b.Descripcion else @p_Descripcion + '%' end ) and
		a.Situacion = 'A'
		ORDER BY a.Situacion asc    
	END
	ELSE
	BEGIN
		SELECT 
			a.CodigoObligacionTecnica,
			a.CodigoPortafolioSBS,
			a.Fecha,
			a.Monto,
			a.Situacion,
			a.UsuarioCreacion,
			a.FechaCreacion,
			a.HoraCreacion,
			a.UsuarioModificacion,
			a.FechaModificacion,
			a.HoraModificacion,
			a.Host,
			b.Descripcion,
			FechaFormato= dbo.fn_SIT_gl_ConvertirFechaaString(Fecha)
		FROM ObligacionTecnica a
		INNER JOIN Portafolio b
		ON a.CodigoPortafolioSBS = b.CodigoPortafolioSBS
		WHERE
		--a.CodigoObligacionTecnica = (case when len(@p_CodObligacionTecnica) = 0 then a.CodigoObligacionTecnica else @p_CodObligacionTecnica end )  and
		a.CodigoObligacionTecnica <> @p_CodObligacionTecnica AND
		a.CodigoPortafolioSBS = (case when len(@p_CodPortafolio) = 0 then a.CodigoPortafolioSBS else @p_CodPortafolio end )  and
		a.Fecha = (case when @p_Fecha= 0 then Fecha else  @p_Fecha end ) and
		b.Descripcion like (case when len(@p_Descripcion) = 0 then b.Descripcion else @p_Descripcion + '%' end ) and
		a.Situacion = 'A'
		ORDER BY a.Situacion asc  
	END

END
GO

GRANT EXECUTE ON [dbo].[SP_ObligacionTecnica_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[sp_CarteraIndirecta_DesactivarRegistros]'

IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_CarteraIndirecta_DesactivarRegistros') BEGIN 
	DROP PROCEDURE [dbo].[sp_CarteraIndirecta_DesactivarRegistros]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 13-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Desactivar registros de una fecha especifica por carga de excel - Cartera Indirecta
-------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_CarteraIndirecta_DesactivarRegistros]
(
	@p_Fecha				numeric(8,0),
	@p_UsuarioModificacion	varchar(50),
	@p_FechaModificacion	numeric(8,0),
	@p_HoraModificacion		varchar(10),
	@p_Host					varchar(20)
)
AS
BEGIN

	UPDATE CarteraIndirecta
	SET
		Situacion = 'I',
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion,
		Host = @p_Host
	WHERE Fecha = @p_Fecha

END
GO

GRANT EXECUTE ON [dbo].[sp_CarteraIndirecta_DesactivarRegistros] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_CarteraIndirecta_Eliminar]'

IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_CarteraIndirecta_Eliminar') BEGIN 
	DROP PROCEDURE [dbo].[sp_CarteraIndirecta_Eliminar]
END 
GO

-- ================================================
-- Autor:		Diego tueros
-- Fecha Creación:	12/03/2019
-- Descripción:		Eliminar de manera logica un regitro de la tabla 'CarteraIndirecta'
-- el atributo 'Situacion' con el valor 'I' (Inactivo)
-- ================================================

CREATE PROCEDURE [dbo].[sp_CarteraIndirecta_Eliminar]
(
	@p_CodigoCarteraIndirecta varchar(12),	-- Código Cartera Indirecta
	@p_UsuarioModificacion varchar(15),	-- Usuario que eliminó el registro
	@p_FechaModificacion numeric(8, 0),	-- Fecha de eliminación del registro
	@p_HoraModificacion varchar(10)	,	-- Hora de eliminación del registro
	@p_Host varchar(20)
)
AS
BEGIN
	
	SET NOCOUNT ON
	UPDATE
		[CarteraIndirecta]
	SET
		[Situacion] = 'I',
		[Host] = @p_Host,
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion

WHERE
	[CodigoCarteraIndirecta] = @p_CodigoCarteraIndirecta
END
GO

GRANT EXECUTE ON [dbo].[sp_CarteraIndirecta_Eliminar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_CarteraIndirecta_Insertar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_CarteraIndirecta_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[sp_CarteraIndirecta_Insertar]
END 
GO


-------------------------------------------------------------------------
-- Fecha Creacion: 12-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Grabar Cartera Indirecta
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_CarteraIndirecta_Insertar]
(
	@p_CodigoPortafolio	varchar(10),
	@p_CodigoEntidad	varchar(4),
	@p_CodigoGrupoEconomico varchar(4),
	@p_CodigoActividadEconomica varchar(100),
	@p_CodigoPais	varchar(10),
	@p_Rating	varchar(10),
	@p_Posicion	numeric(22,7),
	@p_Patrimonio numeric(22,7),
	@p_Participacion numeric(22,7),
	@p_Fecha numeric(8,0),
	@p_Situacion varchar(1),
	@p_UsuarioCreacion varchar(50),
	@p_FechaCreacion numeric(8,0),
	@p_HoraCreacion varchar(10),
	@p_Host varchar(20)
) 
AS 
BEGIN
	declare @p_CodigoCarteraIndirecta as varchar(12)
	
	if ((select count(*) from CarteraIndirecta) = 0)
		set @p_CodigoCarteraIndirecta = '1'
	else
		set @p_CodigoCarteraIndirecta =  (select max(convert(int,CodigoCarteraIndirecta)) from CarteraIndirecta) + 1
	SET NOCOUNT ON 

	INSERT INTO CarteraIndirecta
	(
		CodigoCarteraIndirecta,
		CodigoPortafolioSBS,
		CodigoEntidad,
		CodigoGrupoEconomico,
		CodigoActividadEconomica,
		CodigoPais,
		Rating,
		Posicion,
		Patrimonio,
		Participacion,
		Fecha,
		Situacion,
		UsuarioCreacion,
		FechaCreacion,
		HoraCreacion,
		Host
	)
	VALUES
	(
		@p_CodigoCarteraIndirecta,
		@p_CodigoPortafolio,
		@p_CodigoEntidad,
		@p_CodigoGrupoEconomico,
		@p_CodigoActividadEconomica,
		@p_CodigoPais,
		@p_Rating,
		@p_Posicion,
		@p_Patrimonio,
		@p_Participacion,
		@p_Fecha,
		@p_Situacion,
		@p_UsuarioCreacion,
		@p_FechaCreacion,
		@p_HoraCreacion,
		@p_Host
	)

END
GO

GRANT EXECUTE ON [dbo].[sp_CarteraIndirecta_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_CarteraIndirecta_InsertarExcel]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_CarteraIndirecta_InsertarExcel') BEGIN 
	DROP PROCEDURE [dbo].[sp_CarteraIndirecta_InsertarExcel]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 12-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Inserta los registros cargados del excel
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_CarteraIndirecta_InsertarExcel]
(
	@p_CodigoPortafolio	varchar(10),
	@p_CodigoEntidad	varchar(4),
	@p_CodigoGrupoEconomico varchar(4),
	@p_CodigoActividadEconomica varchar(100),
	@p_CodigoPais	varchar(10),
	@p_Rating	varchar(10),
	@p_Posicion	numeric(22,7),
	@p_Patrimonio numeric(22,7),
	@p_Participacion numeric(22,7),
	@p_Fecha numeric(8,0),
	--@p_Situacion varchar(1),
	@p_UsuarioCreacion varchar(50),
	@p_FechaCreacion numeric(8,0),
	@p_HoraCreacion varchar(10),
	@p_Host varchar(20)
)
AS
BEGIN
	
	if not exists (select CodigoCarteraIndirecta from CarteraIndirecta where CodigoPortafolioSBS = @p_CodigoPortafolio
					AND CodigoEntidad = @p_CodigoEntidad AND Fecha = @p_Fecha and Situacion = 'A')
	BEGIN

		declare @p_CodigoCarteraIndirecta as varchar(12)
	
		if ((select count(*) from CarteraIndirecta) = 0)
			set @p_CodigoCarteraIndirecta = '1'
		else
			set @p_CodigoCarteraIndirecta =  (select max(convert(int,CodigoCarteraIndirecta)) from CarteraIndirecta) + 1
		SET NOCOUNT ON 

		INSERT INTO CarteraIndirecta
		(
			CodigoCarteraIndirecta,
			CodigoPortafolioSBS,
			CodigoEntidad,
			CodigoGrupoEconomico,
			CodigoActividadEconomica,
			CodigoPais,
			Rating,
			Posicion,
			Patrimonio,
			Participacion,
			Fecha,
			Situacion,
			UsuarioCreacion,
			FechaCreacion,
			HoraCreacion,
			Host
		)
		VALUES
		(
			@p_CodigoCarteraIndirecta,
			@p_CodigoPortafolio,
			@p_CodigoEntidad,
			@p_CodigoGrupoEconomico,
			@p_CodigoActividadEconomica,
			@p_CodigoPais,
			@p_Rating,
			@p_Posicion,
			@p_Patrimonio,
			@p_Participacion,
			@p_Fecha,
			--@p_Situacion,
			'A',
			@p_UsuarioCreacion,
			@p_FechaCreacion,
			@p_HoraCreacion,
			@p_Host
		)
			
	END

END
GO

GRANT EXECUTE ON [dbo].[sp_CarteraIndirecta_InsertarExcel] TO [rol_sit_fondos] AS [dbo]
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

PRINT '[dbo].[sp_CarteraIndirecta_Modificar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_CarteraIndirecta_Modificar') BEGIN 
	DROP PROCEDURE [dbo].[sp_CarteraIndirecta_Modificar]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 12-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Actualizar Cartera Indirecta
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[sp_CarteraIndirecta_Modificar]
(
	@p_CodigoCarteraIndirecta varchar(12),
	@p_CodigoPortafolio	varchar(10),
	@p_CodigoEntidad	varchar(4),
	@p_CodigoGrupoEconomico varchar(4),
	@p_CodigoActividadEconomica varchar(100),
	@p_CodigoPais	varchar(10),
	@p_Rating	varchar(10),
	@p_Posicion	numeric(22,7),
	@p_Patrimonio numeric(22,7),
	@p_Participacion numeric(22,7),
	@p_Fecha numeric(8,0),
	@p_Situacion varchar(1),
	@p_UsuarioModificacion varchar(50),
	@p_FechaModificacion numeric(8,0),
	@p_HoraModificacion varchar(10),
	@p_Host varchar(20)
)
AS
BEGIN
	
	UPDATE CarteraIndirecta
	SET
		CodigoPortafolioSBS = @p_CodigoPortafolio,
		CodigoEntidad = @p_CodigoEntidad,
		CodigoGrupoEconomico = @p_CodigoGrupoEconomico,
		CodigoActividadEconomica = @p_CodigoActividadEconomica,
		CodigoPais = @p_CodigoPais,
		Rating = @p_Rating,
		Posicion = @p_Posicion,
		Patrimonio = @p_Patrimonio,
		Participacion = @p_Participacion,
		Fecha = @p_Fecha,
		Situacion = @p_Situacion,
		UsuarioModificacion = @p_UsuarioModificacion,
		FechaModificacion = @p_FechaModificacion,
		HoraModificacion = @p_HoraModificacion,
		Host = @p_Host
	WHERE CodigoCarteraIndirecta = @p_CodigoCarteraIndirecta

END
GO

GRANT EXECUTE ON [dbo].[sp_CarteraIndirecta_Modificar] TO [rol_sit_fondos] AS [dbo]
GO



---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[SP_CarteraIndirecta_SeleccionarPorFiltro]'
USE [SIT-FONDOS]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='SP_CarteraIndirecta_SeleccionarPorFiltro') BEGIN 
	DROP PROCEDURE [dbo].[SP_CarteraIndirecta_SeleccionarPorFiltro]
END 
GO

-------------------------------------------------------------------------
-- Fecha Creacion: 12-03-2019
-- Modificado por: Diego Tueros
-- Descripcion del cambio: Listar Cartera Indirecta
-------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[SP_CarteraIndirecta_SeleccionarPorFiltro]
(
	@p_CodigoCarteraIndirecta varchar(12),
	@p_Fecha decimal(8,0),
	@p_GrupoEconomico varchar(100),
	@p_Fondo varchar(100),
	@p_Emisor varchar(100),
	@p_Tipo varchar(10),
	@p_CodigoPortafolio varchar(10),
	@p_CodigoEntidad varchar(4)
)
AS
BEGIN
	SET NOCOUNT ON

	IF len(@p_Tipo) = 0
	BEGIN
		SELECT 
			CodigoCarteraIndirecta = a.CodigoCarteraIndirecta,
			CodigoEntidad = a.CodigoEntidad,
			CodigoGrupoEconomico = a.CodigoGrupoEconomico,
			CodigoPais = a.CodigoPais,
			CodigoPortafolio = a.CodigoPortafolioSBS,
			Fecha = a.Fecha,
			Participacion = a.Participacion,
			Patrimonio = a.Patrimonio,
			Posicion = a.Posicion,
			Rating = a.Rating,
			Situacion = a.Situacion,
			CodigoActividadEconomica = a.CodigoActividadEconomica,
			DesEmisor = d.Descripcion, 
			DesGrupoEconomico = e.Descripcion,
			DesPais = f.Descripcion,
			DesPortafolio = b.Descripcion,
			DesActividadEconomica = g.Nombre,
			DesRating = h.Nombre,
			DesSituacion = i.Nombre,
			FechaFormat = dbo.FN_SIT_OBT_FechaFormateada(a.Fecha)
		FROM CarteraIndirecta a
		LEFT JOIN Portafolio b ON a.CodigoPortafolioSBS = b.CodigoPortafolioSBS
		LEFT JOIN Entidad c ON a.CodigoEntidad = c.CodigoEntidad
		LEFT JOIN Terceros d ON c.CodigoTercero = d.CodigoTercero
		LEFT JOIN GrupoEconomico e ON a.CodigoGrupoEconomico = e.CodigoGrupoEconomico
		LEFT JOIN Pais f on a.CodigoPais = f.CodigoPais
		LEFT join ParametrosGenerales g on g.Clasificacion = 'SECTOR_GIGS' and a.CodigoActividadEconomica = g.valor  
		LEFT JOIN ParametrosGenerales h on h.Clasificacion = 'RATING' and a.Rating = h.Valor
		LEFT JOIN ParametrosGenerales i on i.Clasificacion = 'Situación' and a.Situacion = i.Valor 
		WHERE
		a.CodigoCarteraIndirecta = (case when len(@p_CodigoCarteraIndirecta) = 0 then a.CodigoCarteraIndirecta else @p_CodigoCarteraIndirecta end )  and
		a.Fecha = (case when @p_Fecha= 0 then Fecha else  @p_Fecha end ) and
		isnull(e.Descripcion,'') like (case when len(@p_GrupoEconomico) = 0 then isnull(e.Descripcion,'') else @p_GrupoEconomico + '%' end ) and
		isnull(b.Descripcion,'') like (case when len(@p_Fondo) = 0 then isnull(b.Descripcion,'') else @p_Fondo + '%' end ) and
		isnull(d.Descripcion,'') like (case when len(@p_Emisor) = 0 then isnull(d.Descripcion,'') else @p_Emisor + '%' end ) and
		a.CodigoPortafolioSBS = (case when len(@p_CodigoPortafolio) = 0 then a.CodigoPortafolioSBS else @p_CodigoPortafolio end )  and
		a.CodigoEntidad = (case when len(@p_CodigoEntidad) = 0 then a.CodigoEntidad else @p_CodigoEntidad end )
		--and
		--a.Situacion = 'A'
		ORDER BY a.Situacion asc    
	END
	ELSE
	BEGIN
		SELECT 
			CodigoCarteraIndirecta = a.CodigoCarteraIndirecta,
			CodigoEntidad = a.CodigoEntidad,
			CodigoGrupoEconomico = a.CodigoGrupoEconomico,
			CodigoPais = a.CodigoPais,
			CodigoPortafolio = a.CodigoPortafolioSBS,
			Fecha = a.Fecha,
			Participacion = a.Participacion,
			Patrimonio = a.Patrimonio,
			Posicion = a.Posicion,
			Rating = a.Rating,
			Situacion = a.Situacion,
			CodigoActividadEconomica = a.CodigoActividadEconomica,
			DesEmisor = d.Descripcion, 
			DesGrupoEconomico = e.Descripcion,
			DesPais = f.Descripcion,
			DesPortafolio = b.Descripcion,
			DesActividadEconomica = g.Nombre,
			DesRating = h.Nombre,
			DesSituacion = i.Nombre,
			FechaFormat = dbo.FN_SIT_OBT_FechaFormateada(a.Fecha)
		FROM CarteraIndirecta a
		LEFT JOIN Portafolio b ON a.CodigoPortafolioSBS = b.CodigoPortafolioSBS
		LEFT JOIN Entidad c ON a.CodigoEntidad = c.CodigoEntidad
		LEFT JOIN Terceros d ON c.CodigoTercero = d.CodigoTercero
		LEFT JOIN GrupoEconomico e ON a.CodigoGrupoEconomico = e.CodigoGrupoEconomico
		LEFT JOIN Pais f on a.CodigoPais = f.CodigoPais
		LEFT join ParametrosGenerales g on g.Clasificacion = 'SECTOR_GIGS' and a.CodigoActividadEconomica = g.valor  
		LEFT JOIN ParametrosGenerales h on h.Clasificacion = 'RATING' and a.Rating = h.Valor
		LEFT JOIN ParametrosGenerales i on i.Clasificacion = 'Situación' and a.Situacion = i.Valor 
		WHERE
		--a.CodigoObligacionTecnica = (case when len(@p_CodObligacionTecnica) = 0 then a.CodigoObligacionTecnica else @p_CodObligacionTecnica end )  and
		a.CodigoCarteraIndirecta <> @p_CodigoCarteraIndirecta AND
		a.Fecha = (case when @p_Fecha= 0 then Fecha else  @p_Fecha end ) and
		isnull(e.Descripcion,'') like (case when len(@p_GrupoEconomico) = 0 then isnull(e.Descripcion,'') else @p_GrupoEconomico + '%' end ) and
		isnull(b.Descripcion,'') like (case when len(@p_Fondo) = 0 then isnull(b.Descripcion,'') else @p_Fondo + '%' end ) and
		isnull(d.Descripcion,'') like (case when len(@p_Emisor) = 0 then isnull(d.Descripcion,'') else @p_Emisor + '%' end ) and
		a.CodigoPortafolioSBS = (case when len(@p_CodigoPortafolio) = 0 then a.CodigoPortafolioSBS else @p_CodigoPortafolio end )  and
		a.CodigoEntidad = (case when len(@p_CodigoEntidad) = 0 then a.CodigoEntidad else @p_CodigoEntidad end )
		--and
		--a.Situacion = 'A'
		ORDER BY a.Situacion asc  
	END

END
GO

GRANT EXECUTE ON [dbo].[SP_CarteraIndirecta_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[sp_SIT_Rating_Insertar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_Rating_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_Rating_Insertar]
END 
GO

-----------------------------------------------------------------------------------------------------------  
--Objetivo: Permite insertar un registro en Historial de Valores o Terceros
--Parámetros:  
-- @p_Codigo - Codigo del Tercero o Valores
-- @p_Rating - Descripcion del Rating
-- @p_RatingInterno - Descripcion del Rating Interno
-- @p_Fecha - Fecha de Proceso  
-- @p_TipoInformacion - Tipo de Informacion del registro  
-- @p_Usuario - Usuario que realiza la operacion
-----------------------------------------------------------------------------------------------------------  
-- Fecha Modificación: 29-11-2016
-- Modificado por: Benjamin Pesates
-- Nro. Orden de Trabajo: 9679
-- Descripcion del cambio: Creacion
-------------------------------------------------------------------------  
--	Fecha Modificación: 2018-10-05
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregó el Rating Fortaleza Financiera y Línea Plazo
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2019-03-11
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 
--	Descripcion del cambio: Se agregó los campos TipoNegocio, Clasficiadora, FechaClasificadora
-----------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[sp_SIT_Rating_Insertar]
(
	@p_Codigo varchar(20),
	@p_Rating varchar(10),
	@p_RatingInterno varchar(10),
	
	@p_RatingFF varchar(10),
	@p_LineaPlazo varchar(15),
	@p_Fecha numeric(8,0),

	@p_TipoNegocio varchar(5),
	@p_Clasificadora varchar(100),
	@p_FechaClasificacion numeric(8,0),

	@p_TipoInformacion varchar(3),
	@p_Usuario varchar(10),
	@p_FechaCreacion numeric(8,0)
)
AS
BEGIN
	DECLARE @CodigoRating varchar(10) = '', @CodigoRatingInterno varchar(10) = '', @CodigoRatingFF varchar(10) = ''
	
	select 
			@CodigoRating = @p_Rating
			,@CodigoRatingInterno = @p_RatingInterno
			,@CodigoRatingFF = @p_RatingFF
	
	IF @p_TipoInformacion = 'EMI' -- EMISIONES
	BEGIN
		IF ISNULL(@CodigoRating, '') <> ''
		BEGIN
			set @p_Codigo = LEFT(@p_Codigo,12) -- Restricción, los CodigoIsin solo son de 12 caracteres
			/*CRumiche: Primero eliminamos el registros si existe*/

			DELETE RatingValorHistoria WHERE CodigoIsin = @p_Codigo AND FechaCreacion = @p_Fecha and negocio = @p_TipoNegocio
			
			INSERT INTO RatingValorHistoria (CodigoIsin, Rating, Negocio,UsuarioCreacion, FechaCreacion, FechaClasificacion, Clasificadora)
			VALUES (@p_Codigo, @CodigoRating, @p_TipoNegocio, @p_Usuario, @p_Fecha, @p_FechaClasificacion, @p_Clasificadora)
						
			if @p_Fecha >= isnull((select MAX(FechaCreacion) from RatingValorHistoria), 0) begin

				UPDATE 
					Valores 
				SET 
					Rating = 
						CASE @p_TipoNegocio
							when 'FONDO' THEN
								 @CodigoRating
							ELSE
								Rating
							END,
					RatingMandato =
						CASE @p_TipoNegocio
							when 'MANDA' THEN
								@CodigoRating
							ELSE
								RatingMandato
							END,
					UsuarioModificacion = @p_Usuario, 
					FechaModificacion = @p_Fecha, 
					HoraModificacion = CONVERT(varchar, GETDATE(),108)
				WHERE CodigoISIN = @p_Codigo
			end			
		END 
	END
	ELSE
	BEGIN
		declare @CodigoTercero varchar(12) = ''
		select top 1 @CodigoTercero = CodigoTercero from Entidad where CodigoEntidad = @p_Codigo	
	
		IF /*ISNULL(@CodigoRating, '') <> '' AND */ ISNULL(@CodigoTercero, '') <> ''
		BEGIN
			/*CRumiche: Primero eliminamos el registros si existe*/
			DELETE RatingTerceroHistoria WHERE CodigoTercero = @CodigoTercero AND FechaCreacion = @p_Fecha
				
			INSERT INTO RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, RatingFF, LineaPlazo, UsuarioCreacion, FechaCreacion)
			VALUES (@CodigoTercero, @CodigoRating, @CodigoRatingInterno, @CodigoRatingFF, @p_LineaPlazo, @p_Usuario, @p_Fecha)
			
			if @p_Fecha >= isnull((select MAX(FechaCreacion) from RatingTerceroHistoria), 0) begin
				
				UPDATE 
					Terceros 
				SET Rating = @CodigoRating, 
					RatingInterno = @CodigoRatingInterno, 
					RatingFF = @CodigoRatingFF, 
					LineaPlazo = @p_LineaPlazo, 
					UsuarioModificacion = @p_Usuario, 
					FechaModificacion = @p_Fecha, 
					HoraModificacion = CONVERT(varchar, GETDATE(),108)
				WHERE CodigoTercero = @CodigoTercero
			end	
		END 
	END
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_Rating_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_SeleccionaRatingTerceroHistoria]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_SeleccionaRatingTerceroHistoria') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_SeleccionaRatingTerceroHistoria]
END 
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 16-11-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9577
--	Descripcion del cambio: Se corrige la consulta y se utiliza el Rating Interno
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 06-12-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9679
--	Descripcion del cambio: Se pone un contador correlativo al resultado para el reporte
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2018-10-05
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregó el Rating Fortaleza Financiera y Línea Plazo
-----------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Giancarlo Tueros.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se adiciono la descripción del RantingFF
---------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2019-03-14
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agrego el Emisor, Clasificadora y Negocio
-----------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[sp_SIT_SeleccionaRatingTerceroHistoria]
(
	 @p_CodigoTercero varchar (12)
	,@p_FechaIni numeric(8)
	,@p_FechaFin numeric(8)
)
AS
BEGIN
	SELECT 
		ROW_NUMBER() OVER(ORDER BY T.Descripcion, RH.FechaCreacion, RH.Correlativo) Correlativo
		,Negocio = ''
		,DesNegocio = ''
		,Codigo = RH.CodigoTercero
		,CodigoTipoInstrumentoSBS = ''
		,Descripcion = T.Descripcion
		,Emisor = ''
		,Rating = ISNULL(PG.Nombre,' ')
		,RatingOficial = ISNULL(PG2.Nombre,' ')
		,RatingFF = ISNULL(PG3.Nombre,' ')
		,RH.LineaPlazo		
		,Fecha = dbo.FN_SIT_OBT_FechaFormateada(RH.FechaCreacion)
		,Clasificadora = ''
	FROM RatingTerceroHistoria RH 
	INNER JOIN Terceros T ON RH.CodigoTercero = T.CodigoTercero
	LEFT JOIN ParametrosGenerales PG ON  RH.Rating = PG.Valor AND PG.Clasificacion = 'RATING'
	LEFT JOIN ParametrosGenerales PG2 ON RH.CodigoCalificionOficial = PG2.Valor AND PG2.Clasificacion = 'RATING'  
	LEFT JOIN ParametrosGenerales PG3 ON RH.RatingFF = PG3.Valor AND PG3.Clasificacion = 'RATING'  
	--OT 9577 Fin
	WHERE RH.CodigoTercero = CASE WHEN @p_CodigoTercero = '' THEN RH.CodigoTercero ELSE @p_CodigoTercero END
	and (RH.FechaCreacion >= @p_FechaIni OR @p_FechaIni = 0)
	and (RH.FechaCreacion <= @p_FechaFin OR @p_FechaFin = 0)
	ORDER BY T.Descripcion, RH.Correlativo
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_SeleccionaRatingTerceroHistoria] TO [rol_sit_fondos] AS [dbo]
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[sp_SIT_SeleccionaRatingValorHistoria]'

IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SIT_SeleccionaRatingValorHistoria') BEGIN 
	DROP PROCEDURE [dbo].[sp_SIT_SeleccionaRatingValorHistoria]
END 
GO

-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 16-11-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9577
--	Descripcion del cambio: Se ordena el query
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 06-12-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9679
--	Descripcion del cambio: Se pone un contador correlativo al resultado para el reporte
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2018-10-05
--	Modificado por: Carlos Rumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregó el Rating Fortaleza Financiera y Línea Plazo
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2019-03-14
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agrego el Emisor, Clasificadora y Negocio
-----------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_SIT_SeleccionaRatingValorHistoria]
(
	 @p_CodigoIsin varchar (12)
	,@p_FechaIni numeric(8)
	,@p_FechaFin numeric(8)	
)
AS
BEGIN
	SELECT 
		 ROW_NUMBER() OVER(ORDER BY V.CodigoNemonico,RH.FechaCreacion, RH.Correlativo) Correlativo
		,Negocio = RH.Negocio
		,DesNegocio = N.Nombre
		,Codigo = RH.CodigoIsin
		,CodigoTipoInstrumentoSBS = v.CodigoTipoInstrumentoSBS
		,Descripcion = V.CodigoNemonico
		,Emisor = T.Descripcion
		,Rating = PG.Nombre
		,RatingOficial = ''
		,RatingFF = ''
		,LineaPlazo = ''		
		,Fecha = dbo.FN_SIT_OBT_FechaFormateada(RH.FechaCreacion)
		,Clasificadora = RH.Clasificadora
	FROM RatingValorHistoria RH 
	INNER JOIN Valores V ON RH.CodigoIsin = V.CodigoISIN
	LEFT JOIN ParametrosGenerales PG ON RH.Rating = PG.Valor AND PG.Clasificacion = 'RATING'	
	LEFT JOIN ParametrosGenerales N ON RH.Negocio = N.Valor AND N.Clasificacion = 'TipoNegoc'	
	LEFT JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad
	LEFT JOIN Terceros T ON E.CodigoTercero = T.CodigoTercero				
	WHERE RH.CodigoIsin = CASE WHEN @p_CodigoIsin = '' THEN RH.CodigoIsin ELSE @p_CodigoIsin END
	and (RH.FechaCreacion >= @p_FechaIni OR @p_FechaIni = 0)
	and (RH.FechaCreacion <= @p_FechaFin OR @p_FechaFin = 0)
	ORDER BY V.CodigoNemonico,RH.FechaCreacion, RH.Correlativo
	--OT 9577 Fin
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_SeleccionaRatingValorHistoria] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[ObtenerCodigoRating]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='ObtenerCodigoRating') BEGIN 
	DROP PROCEDURE [dbo].[ObtenerCodigoRating]
END 
GO

-----------------------------------------------------------------------------------------------------------
--Objetivo: Listar datos de valores
--	Fecha Modificacion: 08/03/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 
--	Descripcion del cambio: Se obtiene el codigo del rating importado.
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ObtenerCodigoRating]
(
	@Rating AS VARCHAR(10)
)
AS
BEGIN

	SELECT 
		Valor,
		Nombre
	FROM ParametrosGenerales 
	WHERE Clasificacion = 'RATING'
	AND Nombre = @Rating

END
GO

GRANT EXECUTE ON [dbo].[ObtenerCodigoRating] TO [rol_sit_fondos] AS [dbo]
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
PRINT '[dbo].[Valores_Insertar]'

IF EXISTS (SELECT * FROM sysobjects WHERE name='Valores_Insertar') BEGIN 
	DROP PROCEDURE [dbo].[Valores_Insertar]
END 
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
--	Fecha Modificacion: 08/03/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 
--	Descripcion del cambio: Se adicionó el campo de RatingMandato
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Valores_Insertar] 
(
	@p_CodigoNemonico varchar(15),@p_CodigoTipoTitulo varchar(15),@p_Descripcion  varchar(50),@p_Agrupacion  varchar(5),
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
	,@p_RatingMandato varchar(10)=NULL
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
	,RatingMandato
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
	,@p_RatingMandato
	)  
	
		--Historia
	-- EXEC sp_SIT_InsertaRatingValorHistoria @p_CodigoISIN,@p_Rating,@p_UsuarioCreacion ,@p_FechaCreacion
	
	-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	delete from RatingValorHistoria where CodigoIsin = @p_CodigoISIN and FechaCreacion = @p_FechaCreacion
	
	INSERT INTO RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio)
	VALUES (@p_CodigoISIN, @p_Rating, @p_UsuarioCreacion, @p_FechaCreacion, 'FONDO')
	-- FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico

	IF(isnull(@p_RatingMandato,'') <> '')
	BEGIN
		
		INSERT INTO RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio)
		VALUES (@p_CodigoISIN, @p_RatingMandato, @p_UsuarioCreacion, @p_FechaCreacion, 'MANDA')

	END
END
GO

GRANT EXECUTE ON [dbo].[Valores_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


----------------------------------------------------------------------------------------------------------
PRINT '[dbo].[Valores_Modificar]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='Valores_Modificar') BEGIN 
	DROP PROCEDURE [dbo].[Valores_Modificar]
END 
GO

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
--	Fecha Modificacion: 08/03/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 
--	Descripcion del cambio: Se adicionó el campo de RatingMandato
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Valores_Modificar] 
(
	@p_CodigoNemonico varchar(15),@p_CodigoTipoTitulo varchar(15),@p_Descripcion  varchar(50),@p_Agrupacion  varchar(5),
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

	--
	,@p_RatingMandato varchar(10)=NULL
	--
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
	,RatingMandato = @p_RatingMandato 
	WHERE  CodigoNemonico = @p_CodigoNemonico
	--VL Codigo Valor
	UPDATE ReporteVL SET TipoCodigoValor = @p_TipoCodigoValor  WHERE CodigoValor = @p_CodigoISIN
	
	--	--Historia
	--IF LTRIM(RTRIM(ISNULL(@p_Rating, ''))) <> @Rating
	--	EXEC sp_SIT_InsertaRatingValorHistoria @p_CodigoISIN, @p_Rating, @p_UsuarioModificacion, @p_FechaModificacion
		
	-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	delete from RatingValorHistoria where CodigoIsin = @p_CodigoISIN and FechaCreacion = @p_FechaModificacion
	
	INSERT INTO RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio)
	VALUES (@p_CodigoISIN, @p_Rating, @p_UsuarioModificacion, @p_FechaModificacion, 'FONDO')
	-- FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico	
	
	IF(isnull(@p_RatingMandato,'') <> '')
	BEGIN
		
		INSERT INTO RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio)
		VALUES (@p_CodigoISIN, @p_RatingMandato, @p_UsuarioModificacion, @p_FechaModificacion, 'MANDA')

	END		
END
GO

GRANT EXECUTE ON [dbo].[Valores_Modificar] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[Valores_Seleccionar]'


IF EXISTS (SELECT * FROM sysobjects WHERE name='Valores_Seleccionar') BEGIN 
	DROP PROCEDURE [dbo].[Valores_Seleccionar]
END 
GO

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
--	Descripcion del cambio: Se adicionA³ el campo de PrecioDevengado.
----------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 08/03/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 
--	Descripcion del cambio: Se adicionA³ el campo de RatingMandato
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Valores_Seleccionar] 
(
	@p_CodigoNemonico varchar(15)
)
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
	,ISNULL(V.EstadoBaseIC,'0')EstadoBaseIC,ISNULL(V.BaseInteresCorrido,'0') BaseInteresCorrido, ISNULL(V.BaseInteresCorridoDias,'0') BaseInteresCorridoDias  -- 22/05/2018 - RCE - Nuevo Campo Estado Base InterA©s Corrido
	,ISNULL(V.Subordinado,'0') Subordinado -- 23/10/2018 - GT - Nuevo Parametro para Renta Fija y utiilizado para el calculo de limitie subordinario
	,ISNULL(V.PrecioDevengado,'0') PrecioDevengado,
	isnull(v.RatingMandato,'') RatingMandato
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



PRINT '[dbo].[Pr_Generar_ReporteLimites]'
IF EXISTS (SELECT 1 FROM sysobjects WHERE name='Pr_Generar_ReporteLimites') BEGIN 
	DROP PROCEDURE [dbo].[Pr_Generar_ReporteLimites]
END 
GO

CREATE PROCEDURE [dbo].[Pr_Generar_ReporteLimites]
(
	@p_FechaProceso NUMERIC(8),
	@p_FechaCadena VARCHAR(20) = '',
	@p_CodigoPortafolioSBS VARCHAR(5) = ''
)
AS
BEGIN
	SET NOCOUNT ON
	
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
	)
	
	SELECT p.CodigoPortafolioSBS, SUM(ISNULL(VPNMonedaFondo,0)) Total
	INTO #TotalCartera
	FROM Portafolio p WITH(NOLOCK) INNER JOIN CarteraTituloValoracion c WITH(NOLOCK) ON p.CodigoPortafolioSBS = c.CodigoPortafolioSBS
	WHERE p.Situacion = 'A' AND c.FechaValoracion = @p_FechaProceso AND c.Escenario = 'REAL'
	GROUP BY p.CodigoPortafolioSBS

	SELECT r.CodigoReporte, r.CodigoLimite, r.CodigoLimiteCaracteristica, r.FechaReporte, r.Secuencial, r.CodigoNivelLimite, r.Tipo, r.Tope,
		r.ValorPorcentaje, r.Posicion, r.Participacion, r.Margen, r.Alerta, r.ValorNivel, r.DescripcionNivel, r.PorVal, r.Patrimonio, r.ValorBase,
		r.Cxc, r.Cxp, r.TotalInversiones, r.SaldoBanco, r.Factor, r.ValorEfectivoColocado, r.FloatOficioMultiple, r.UnidadesEmitidas, r.TotalActivo,
		r.TotalPasivo, r.PosicionF1, r.PosicionF2, r.PosicionF3, r.FechaVencimiento, r.FechaOperacion, r.CodigoMoneda, r.Escenario, r.NivelSaldoBanco,
		r.ValorPorcentajeM, r.Valor1, r.Valor2, r.Valor3, r.ValorPorcentajeMinimo, r.MargenMinimo, lc.CodigoPortafolioSBS
	INTO #ReporteLimites
	FROM ReporteLimites r WITH(NOLOCK)
		INNER JOIN LimiteCaracteristica lc WITH(NOLOCK) ON r.CodigoLimiteCaracteristica = lc.CodigoLimiteCaracteristica AND lc.Situacion = 'A'
	WHERE FechaReporte = @p_FechaProceso
		--AND lc.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolioSBS = '' THEN lc.CodigoPortafolioSBS ELSE @p_CodigoPortafolioSBS END
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
			PRINT '1'
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
		,r.Posicion,CASE WHEN r.ValorNivel IS NULL THEN v.NombreLimite ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END
		,''
		, CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.ValorNivel, '')) END
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
		--@p_FechaCadena Fecha, r.CodigoPortafolioSBS, p.Descripcion NombrePortafolio, 'MONEDA: ' + m.Descripcion Moneda, r.CodigoLimite, v.NombreLimite, v.Cuadrar,
	
		----Estado = CASE WHEN v.Validacion = 0 THEN 'OK' ELSE 'INCONSISTENCIA' END, v.TotalPosicion, v.TotalParticipacion,
		--Estado = CASE v.TotalPosicion 
		--	WHEN dbo.ObtenerValorizacionPorPortafolio(v.CodigoPortafolioSBS,@p_FechaProceso) THEN 'OK'
		--	ELSE 'No suma el 100%' END
		
		--, v.TotalPosicion, v.TotalParticipacion,
		--CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.ValorNivel, '')) END Codigo,
		--CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END Descripcion,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.ValorBase, 0) END ValorBase,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.Posicion, 0) END Posicion,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.Participacion, 0) END Participacion,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.ValorPorcentajeMinimo, 0) END ValorPorcentajeMinimo,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.MargenMinimo, 0) END MargenMinimo,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE ISNULL(r.ValorPorcentaje, 0) END ValorPorcentajeMaximo,
		--CASE WHEN r.ValorNivel IS NULL THEN NULL ELSE CASE WHEN r.CodigoLimite = '07' THEN ISNULL(r.ValorPorcentaje, 0) - ISNULL(r.Posicion, 0) ELSE ISNULL(r.Margen, 0) END END MargenMaximo,
		--CASE WHEN r.ValorNivel IS NULL THEN '' ELSE CASE WHEN ISNULL(r.ValorBase, 0) > 0 AND ISNULL(r.Posicion, 0) > 0 AND ISNULL(r.ValorPorcentaje, 0) > 0 THEN CASE WHEN ROUND(ISNULL(r.Margen, 0), 2) < 0 OR (r.CodigoLimite = '07' AND ROUND((ISNULL(r.ValorPorcentaje, 0) - ISNULL(r.Posicion, 0)), 2) < 0) THEN 'EXCESO LIMITE' ELSE '' END ELSE '' END END Alerta
		,
		 CASE WHEN ISNULL(r.ValorBase,0) <> 0 and ISNULL(r.valorporcentaje,0) <> 0 THEN (r.ValorBase * ( ValorPorcentaje / 100)) ELSE 0 END
		,CASE when ISNULL(r.ValorBase,0) <> 0 and ISNULL(r.valorporcentaje,0) <> 0 THEN (r.Posicion / (r.ValorBase * ( ValorPorcentaje / 100))) ELSE 0 END 
	FROM #ReporteLimites r INNER JOIN #Validacion v ON r.CodigoPortafolioSBS = v.CodigoPortafolioSBS AND r.CodigoLimite = v.CodigoLimite
							INNER JOIN Portafolio p WITH(NOLOCK) ON r.CodigoPortafolioSBS = p.CodigoPortafolioSBS
							INNER JOIN Moneda m WITH(NOLOCK) ON p.CodigoMoneda = m.CodigoMoneda
							INNER JOIN Limite L ON r.CodigoLimite = L.CodigoLimite
	WHERE r.CodigoLimiteCaracteristica NOT IN (SELECT z.CodigoLimiteCaracteristica
												FROM (SELECT x.CodigoLimiteCaracteristica,  COUNT(1) Cantidad
														FROM #ReporteLimites x INNER JOIN #Validacion y ON x.CodigoPortafolioSBS = y.CodigoPortafolioSBS AND x.CodigoLimite = y.CodigoLimite
														WHERE y.Validacion = 0
														GROUP BY x.CodigoLimiteCaracteristica) z
												WHERE z.Cantidad = 1)
	ORDER BY p.Descripcion, r.CodigoLimite, CASE WHEN r.ValorNivel IS NULL THEN '' ELSE UPPER(ISNULL(r.DescripcionNivel, '')) END
	
	SELECT * FROM @tReporteLimite
	ORDER BY PatrimonioCierre desc
	
	SET NOCOUNT OFF
END
GO

GRANT EXECUTE ON [dbo].[Pr_Generar_ReporteLimites] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Tercero_Modificar]'
IF EXISTS (SELECT * FROM sys.objects WHERE object_id=OBJECT_ID('Tercero_Modificar')) 
BEGIN 
	DROP PROCEDURE [dbo].[Tercero_Modificar]
END 
GO
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 03/10/2016
--	Modificado por: Marlon E. Peña
--	Nro. Orden de Trabajo: 9363
--	Descripcion del cambio: Se incluye el parametro RatingInterno para actualizar su valor en la tabla Limites.
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 16-11-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9577
--	Descripcion del cambio: Se inserta el Rating interno a la tabla historial
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 18/10/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Agregar guardado de historia de rating
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2018-10-23
--	Modificado por: CRumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se agregó el Rating FF
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[Tercero_Modificar]
(
	@p_Situacion VARCHAR(1),@p_CodigoTercero	VARCHAR(11),@p_Descripcion VARCHAR(64),@p_Direccion VARCHAR(64),
	@p_CodigoPostal VARCHAR(3),@p_CodigoPais VARCHAR(4),@p_CodigoTipoDocumento VARCHAR(4),@p_CodigoSectorEmpresarial VARCHAR(4),@p_Beneficiario VARCHAR(11),
	@p_TipoTercero VARCHAR(4),@p_ClasificacionTercero	VARCHAR(5),@p_CodigoCustodio VARCHAR(12),@p_TerceroBanco VARCHAR(10),@p_Rating  varchar(10),
	@p_CodigoEmision  VARCHAR(2),@p_UsuarioModificacion	VARCHAR(15),@p_FechaModificacion NUMERIC(8,0),@p_HoraModificacion VARCHAR(10),@p_Host VARCHAR(20),
	@p_CodigoDocumento varchar(15),@p_CodigoCalificionOficial VARCHAR(10) = '',@p_RatingInterno varchar(10),@p_CodigoRatingFF VARCHAR(10) = '',
	@p_LineaPlazo VARCHAR(15) = '',@p_SectorGIGS VARCHAR(6)
)
AS
BEGIN
	--OT 9577 Inicio
	DECLARE @Rating varchar(10), @RatingInterno varchar(10)
	
	SELECT @Rating = LTRIM(RTRIM(ISNULL(Rating, ''))), @RatingInterno = LTRIM(RTRIM(ISNULL(RatingInterno, '')))
	FROM Terceros
	WHERE CodigoTercero = @p_CodigoTercero 

	----Historia
	--IF LTRIM(RTRIM(ISNULL(@p_Rating, ''))) <> @Rating OR LTRIM(RTRIM(ISNULL(@p_RatingInterno, ''))) <> @RatingInterno
	--	EXEC sp_SIT_InsertaRatingTerceroHistoria @p_CodigoTercero, @p_Rating, @p_RatingInterno, @p_UsuarioModificacion, @p_FechaModificacion
	----OT 9577 Fin
	
	--Modifica Valores
	UPDATE Terceros SET Situacion = @p_Situacion,Descripcion = @p_Descripcion,Direccion = @p_Direccion,CodigoPostal = @p_CodigoPostal,CodigoPais = @p_CodigoPais,
	CodigoTipoDocumento = @p_CodigoTipoDocumento,UsuarioModificacion = @p_UsuarioModificacion,FechaModificacion = @p_FechaModificacion,
	CodigoSectorEmpresarial = @p_CodigoSectorEmpresarial,Beneficiario = @p_Beneficiario,HoraModificacion = @p_HoraModificacion,Rating = @p_Rating,
	Host = @p_Host,TipoTercero = @p_TipoTercero,ClasificacionTercero = @p_ClasificacionTercero,CodigoCustodio = @p_CodigoCustodio,CodigoEmision = @p_CodigoEmision,
	CodigoDocumento = @p_CodigoDocumento,CodigoCalificionOficial = @p_CodigoCalificionOficial,
    RatingInterno = @p_RatingInterno, RatingFF = @p_CodigoRatingFF, LineaPlazo = @p_LineaPlazo, SectorGigs=@p_SectorGIGS
	WHERE CodigoTercero = @p_CodigoTercero
	
	-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	DELETE FROM RatingTerceroHistoria where CodigoTercero = @p_CodigoTercero and FechaCreacion = @p_FechaModificacion
	
	INSERT INTO RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, RatingFF, LineaPlazo, UsuarioCreacion, FechaCreacion)
	VALUES (@p_CodigoTercero, @p_CodigoCalificionOficial, @p_RatingInterno, @p_CodigoRatingFF, @p_LineaPlazo, @p_UsuarioModificacion, @p_FechaModificacion)
	-- FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	
END
GO

GRANT EXECUTE ON [dbo].[Tercero_Modificar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Tercero_Insertar]'

IF EXISTS (SELECT * FROM sys.objects WHERE object_id=OBJECT_ID('Tercero_Insertar')) 
BEGIN 
	DROP PROCEDURE [dbo].[Tercero_Insertar]
END 
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 03/10/2016
--	Modificado por: Marlon E. Peña
--	Nro. Orden de Trabajo: 9363
--	Descripcion del cambio: Se incluye el parametro RatingInterno para guardarlo en la tabla Limites.
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 16-11-2016
--	Modificado por: Benjamin Pesantes
--	Nro. Orden de Trabajo: 9577
--	Descripcion del cambio: Se inserta el Rating interno a la tabla historial
-------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 2018-10-23
--	Modificado por: CRumiche
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: Se inserta el RatingFF
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Tercero_Insertar]
(
	@p_Situacion VARCHAR(1),@p_CodigoTercero VARCHAR(11),@p_Descripcion VARCHAR(64),@p_Direccion VARCHAR(64),
	@p_CodigoPostal VARCHAR(3),@p_CodigoPais VARCHAR(4),@p_CodigoTipoDocumento	VARCHAR(4),@p_CodigoSectorEmpresarial VARCHAR(4),@p_Beneficiario VARCHAR(11),
	@p_TipoTercero VARCHAR(4),@p_ClasificacionTercero	VARCHAR(5),@p_CodigoCustodio	VARCHAR(12),@p_TerceroBanco VARCHAR(10),@p_Rating  VARCHAR(10),
	@p_CodigoEmision  VARCHAR(2),@p_UsuarioCreacion	VARCHAR(15),@p_FechaCreacion	NUMERIC(8,0),@p_HoraCreacion VARCHAR(10),@p_Host VARCHAR(20),
	@p_CodigoDocumento varchar(15),@p_CodigoCalificionOficial VARCHAR(10) = '',@p_RatingInterno varchar(10),@p_CodigoRatingFF VARCHAR(10) = '',
	@p_LineaPlazo VARCHAR(15) = '',@p_SectorGIGS VARCHAR(6)
)
AS
BEGIN	
	--Insertar Tercero
	INSERT INTO Terceros(Situacion,CodigoTercero,Descripcion,Direccion,CodigoPostal,CodigoPais,CodigoTipoDocumento,UsuarioCreacion,FechaCreacion,
	CodigoSectorEmpresarial,Beneficiario,HoraCreacion,Host,TipoTercero,ClasificacionTercero,CodigoCustodio,Rating,CodigoEmision,CodigoDocumento,
	CodigoCalificionOficial, RatingInterno, RatingFF, LineaPlazo, SectorGigs) -- OT 9363
	VALUES(@p_Situacion,@p_CodigoTercero,@p_Descripcion,@p_Direccion,@p_CodigoPostal,@p_CodigoPais,@p_CodigoTipoDocumento,@p_UsuarioCreacion,@p_FechaCreacion,
	@p_CodigoSectorEmpresarial,@p_Beneficiario,@p_HoraCreacion,@p_Host,@p_TipoTercero,@p_ClasificacionTercero,
	(CASE WHEN LEN(@p_CodigoCustodio) = 0 THEN NULL ELSE @p_CodigoCustodio END),@p_Rating,@p_CodigoEmision,@p_CodigoDocumento,@p_CodigoCalificionOficial
   ,@p_RatingInterno, @p_CodigoRatingFF, @p_LineaPlazo, @p_SectorGIGS) -- OT 9363
   
 --  --OT 9577 Inicio	
	--EXEC sp_SIT_InsertaRatingTerceroHistoria @p_CodigoTercero,@p_Rating, @p_RatingInterno, @p_UsuarioCreacion,@p_FechaCreacion
	----OT 9577 Fin
	
	-- INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
	DELETE FROM RatingTerceroHistoria where CodigoTercero = @p_CodigoTercero and FechaCreacion = @p_FechaCreacion
	
	INSERT INTO RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, RatingFF, LineaPlazo, UsuarioCreacion, FechaCreacion)
	VALUES (@p_CodigoTercero, @p_CodigoCalificionOficial, @p_RatingInterno, @p_CodigoRatingFF, @p_LineaPlazo, @p_UsuarioCreacion, @p_FechaCreacion)
	-- FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-18 | Insertar Datos del Rating en el Histórico
END
GO

GRANT EXECUTE ON [dbo].[Tercero_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Posicion_Calcular_Reporte_NEW]'
IF EXISTS (SELECT * FROM sys.objects WHERE object_id=OBJECT_ID('Posicion_Calcular_Reporte_NEW')) 
BEGIN 
	DROP PROCEDURE [dbo].[Posicion_Calcular_Reporte_NEW]
END 
GO

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
		@v_texto_case			varchar(250), /*CRumiche: Solo util para obtener valores de CASE WHEN (Para generar claridad de CÓDIGO)*/

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
		@CodigoMonedaPortafolio VARCHAR(100) /* De la Tabla Portafolio*/
    
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

	/*CRumiche: Cálculo del Valor Base a nivel de Item de Caracteristica en forma dinámica*/
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
		print 'Script Cálculo de Valor Base @SQL_CALCULO_VALOR: ' + isnull(@SQL_CALCULO_VALOR,'')
		
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

    IF @p_FlagTipoPorc = 'D' /*Para Caracteristicas de Nivel con "Porcentaje por Detalle de Agrupación" */
    BEGIN 
		PRINT '@p_FlagTipoPorc: '+ @p_FlagTipoPorc
		PRINT '@AplicarCastigoRating: ' + @AplicarCastigoRating

		IF @AplicarCastigoRating = 'S'
			SET @p_ValorEntidad = dbo.fn_AplicaCastigoRating(@p_ValorEntidad, @NivelesCastigo)
    
		--SELECT @p_Porcentaje = Max(valorporcentaje), 
		--		 @p_PorcentajeM = Max(ValorPorcentajeM),
		--		 @cont = Count(CodigoNivelLimite),
		--		 @TieneValorEspecifico =  TieneValorEspecifico
		SELECT @p_Porcentaje = valorporcentaje, 
				 @p_PorcentajeM = ValorPorcentajeM,
				 @cont = CodigoNivelLimite,
				 @TieneValorEspecifico =  ISNULL(TieneValorEspecifico,'N')
		FROM   detallenivellimite2 
		WHERE  codigonivellimite = @p_CodigoNivelLimite 
		AND    codigocaracteristica = @p_CodigoCaracteristica 
		AND    valorcaracteristica = @p_ValorEntidad    
		
		PRINT '@p_ValorEntidad: ' + ISNULL(@p_ValorEntidad,'')
		PRINT '@p_CodigoCaracteristica: ' + @p_CodigoCaracteristica
		PRINT '@p_CodigoNivelLimite: ' + @p_CodigoNivelLimite
		PRINT '@p_Porcentaje: ' + CAST(@p_Porcentaje AS VARCHAR)
		PRINT '@cont: ' + CAST(@cont AS VARCHAR)
		
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

    /*Condición para @p_FiltroGrupo si es vacío*/ 
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
    /*Inicio de cálculo de posiciones según Límite*/ 
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
		IF @UnidadPosicion ='N3D' /*MONTO Negociación a 3 DIAS*/
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
					/* CRumiche: Primero se definirá en el SELECT BASE */													
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
				
				/* CRumiche: Finalmente concatenamos QUERYS antes de la ejecución Y agrega el FILTRO heredado */
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
				
				/* CRumiche: @ES_AGRUPADOR podría permitir la utilización del Comodin {IN_NOT_IN}
							los cuales podrían configurarse en @SQL_FROM_CARAC o @SQL_WHERE_CARAC */
				DECLARE @p_IN_NOT_IN VARCHAR(10) = ' IN ', @p_SQL_FINAL_CALCULO varchar(2048)
				
				SELECT 
					@p_IN_NOT_IN = case when ES_GRUPO_EXCEPCION = 'S' then ' NOT IN ' else ' IN ' end
					,@p_SQL_FINAL_CALCULO = SQL_FINAL_CALCULO
				FROM GrupoCaracteristicaCabecera 
				WHERE TablaGrupo = @NombreTabla AND CodigoGrupo = @p_ValorEntidad
				
				/* CRumiche: Aplicamos defaults en caso de NULL*/
				SELECT @p_IN_NOT_IN = ISNULL(@p_IN_NOT_IN, ' IN '), @p_SQL_FINAL_CALCULO = ISNULL(@p_SQL_FINAL_CALCULO, '')

				/* CRumiche: Soporte para Aplicar Funciones Complemento a las Posiciones en todos los Límites (Ejemplo: Funcion Saldo Banco)*/
				SELECT @p_SQL_FINAL_CALCULO = @p_SQL_FINAL_CALCULO + ' ' + SQL_FINAL_CALCULO
				FROM LimiteFuncionesPosicion WHERE Situacion = 'A' AND ES_PARA_AGRUPADORES = @ES_AGRUPADOR /* Hay funciones para Caracteristicas y otras para Grupos de Caracteristicas */
				
				
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

				PRINT '### QUERY: Suma de Instrumentos en Forma Dinámica: ' + @CMDSQL
				
				/* Solo para cummplir con el INSERT q está en lineas más ABAJO */
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
			PRINT '### QUERY: Ejecución A: ' + @CMDSQL
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

GRANT EXECUTE ON [dbo].[Posicion_Calcular_Reporte_NEW] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_CarteraIndirecta_DatosEntidad_Listar]'
IF EXISTS (SELECT * FROM sys.objects WHERE object_id=OBJECT_ID('sp_CarteraIndirecta_DatosEntidad_Listar')) 
BEGIN 
	DROP PROCEDURE [dbo].[sp_CarteraIndirecta_DatosEntidad_Listar]
END 
GO

CREATE PROCEDURE [dbo].[sp_CarteraIndirecta_DatosEntidad_Listar]
(
	@p_CodigoEntidad varchar(4)
)
AS
BEGIN

	SELECT 
		CodigoEntidad = isnull(a.CodigoEntidad,''),
		CodigoTercero = a.CodigoTercero,
		CodigoPais = b.CodigoPais,
		CodigoActividadEconomica = b.SectorGigs,
		CodigoGrupoEconomico = a.CodigoGrupoEconomico
	FROM Entidad a
	INNER JOIN Terceros b on a.CodigoTercero = b.CodigoTercero where codigoentidad = @p_CodigoEntidad

END
GO

GRANT EXECUTE ON [dbo].[sp_CarteraIndirecta_DatosEntidad_Listar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Limite_ProcesarLimite_NEW]'
IF EXISTS (SELECT * FROM sys.objects WHERE object_id=OBJECT_ID('Limite_ProcesarLimite_NEW')) 
BEGIN 
	DROP PROCEDURE [dbo].[Limite_ProcesarLimite_NEW]
END 
GO

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
				Valor3 NUMERIC(22,7)  DEFAULT 0
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
			WHERE not(@NumeroNiveles > r.Secuencial or (@NumeroNiveles = r.Secuencial AND ISNULL(r.Posicion,0) <> 0))  

			DELETE #Reporte WHERE not(@NumeroNiveles > Secuencial or (@NumeroNiveles = Secuencial AND ISNULL(Posicion,0) <> 0))  

			--UPDATE #Reporte set Posicion = 0 WHERE ValorNivel = 'SaldoBanco4'			
			--IF @p_procesar = 2  SET @p_Escenario = 'ONLINE'
			
			DECLARE @CodigoReporte NUMERIC(12,0)
			SET @CodigoReporte = (SELECT MAX(tb.maxreg) 
				FROM (SELECT maxreg = ISNULL(MAX(CodigoReporte),0) FROM ReporteLimites 
					union SELECT ISNULL(MAX(CodigoReporte),0) FROM ReporteLimites_Portafolio) tb);			

			----SELECT 
			----	'CodigoReporte' = CodigoReporte + @CodigoReporte,
			----	Secuencial, 
			----	CodigoNivelLimite,
			----	ValorPorcentaje,
			----	--ValorPorcentaje = CASE WHEN ValorPorcentaje > -1 
			----	--						THEN CASE WHEN (SELECT AplicarCastigo FROM Limite WHERE CodigoLimite = @p_CodigoLimite) = 'S' 
			----	--							THEN CASE WHEN (SELECT EntidadVinculada FROM GrupoEconomico 
			----	--											WHERE CodigoGrupoEconomico = (CASE (SELECT CodigoCaracteristica FROM NivelLimite WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial = rp.Secuencial AND Situacion = 'A')
			----	--																				WHEN '05' then (SELECT g.CodigoGrupoEconomico FROM Entidad e 
			----	--																								inner join GrupoEconomico g on e.CodigoGrupoEconomico = g.CodigoGrupoEconomico 
			----	--																								WHERE CodigoEntidad = rp.ValorNivel)
			----	--																				WHEN '06' then rp.ValorNivel
			----	--																				WHEN '09' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
			----	--																								WHERE cn in (	SELECT CodigoNemonico FROM PatrimonioFideicomisoDetalle
			----	--																												WHERE CodigoPatrimonioFideicomiso = rp.ValorNivel)) 
			----	--																				WHEN '04' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos WHERE cn = rp.ValorNivel)
			----	--																				end)) = 'S' 
			----	--											THEN ValorPorcentaje * 0.7
			----	--											ELSE ValorPorcentaje 
			----	--											END
			----	--							ELSE ValorPorcentaje    
			----	--							END    
			----	--						ELSE ValorPorcentaje END,						
			----	'ValorBase' = @Valor,
			----	Posicion, 
			----	'Participacion' = CASE WHEN isnull(@Valor,0) <> 0 THEN (Posicion / isnull(@Valor,0)) * 100 /* Es Porcentaje*/ ELSE 0 END,
				
			----	ValorNivel,
			----	DescripcionNivel,
			----	'Simbolo' = PorVal,
			----	Patrimonio,
			----	'TotalInversion' = CASE @ValorBase WHEN 'OBLITEC' THEN  (SELECT top 1 isnull(Valor,0) FROM PatrimonioEmisor WHERE TipoValor = 'OT' AND CodigoEntidad = rp.ValorNivel) ELSE @TotalInversiones END,
				
			----	Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,
			----	CodigoMoneda,FechaOperacion,NivelSaldoBanco,ValorPorcentajeM,Valor1,Valor2,Valor3 					
			----FROM #Reporte rp

			INSERT INTO ReporteLimites(CodigoReporte,CodigoLimite,CodigoLimiteCaracteristica,FechaReporte,Secuencial,CodigoNivelLimite,Tipo,
				Tope,ValorPorcentaje,Posicion,Participacion,Margen,Alerta,ValorNivel,DescripcionNivel,PorVal,ValorBase, cxc, cxp,Patrimonio,TotalInversiones ,
				SaldoBanco,Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,CodigoMoneda,FechaOperacion,
				Escenario,NivelSaldoBanco,ValorPorcentajeM,Valor1,Valor2,Valor3)

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
					RP.CodigoMoneda,RP.FechaOperacion,@p_Escenario,RP.NivelSaldoBanco,RP.ValorPorcentajeM,RP.Valor1,RP.Valor2,RP.Valor3
					
				FROM #Reporte RP
				
			/*CRumiche: Solo mostramos la info isertada pero esto no tendria utilidad en este proceso puntual */
			SELECT CodigoReporte	,CodigoLimite	,CodigoLimiteCaracteristica	,FechaReporte	,Secuencial	,CodigoNivelLimite	,Tipo	,Tope	
				,ValorPorcentaje	,Posicion	,Participacion	,Margen	,Alerta	,ValorNivel	,DescripcionNivel	,PorVal	,Patrimonio	,ValorBase
				,Cxc	,Cxp	,TotalInversiones	,SaldoBanco
			-- select *
			FROM ReporteLimites 
			WHERE CodigoLimite = @p_CodigoLimite AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND FechaReporte = @p_Fecha

							
			----	select x.* from (select * from WITH_ReporteProcesado) as X
			
			
				----IF @p_procesar = 1 
				----BEGIN
				
					---------- Pasamos a mostrar los datos obtenidos del proceso
					--------SELECT 'CodigoReporte' = CodigoReporte + @CodigoReporte,@p_CodigoLimite 'CodigoLimite',@p_CodigoLimiteCaracteristica 'CodigoLimiteCaracteristica',   
					--------@p_Fecha 'FechaReporte', Secuencial AS  Secuencial,CodigoNivelLimite,
					--------'Tipo'= CASE WHEN @Tipo = 'L' THEN 'Ley'  WHEN @Tipo = 'I' THEN 'Interno'END,
					--------'Tope' = CASE WHEN @Tope = 'BAN' THEN 'BANDas' WHEN @Tope = 'MAX' THEN 'Maximo' WHEN @Tope = 'MIN' THEN 'Minimo' END,
					--------CASE WHEN ValorPorcentaje > -1 THEN
					--------		CASE WHEN (SELECT AplicarCastigo FROM Limite WHERE CodigoLimite = @p_CodigoLimite) = 'S' THEN    
					--------				CASE WHEN (SELECT EntidadVinculada FROM GrupoEconomico
					--------				WHERE CodigoGrupoEconomico = (CASE (SELECT CodigoCaracteristica FROM NivelLimite 
					--------					WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial = rp.Secuencial AND Situacion = 'A')
					--------				WHEN '05' then (SELECT g.CodigoGrupoEconomico FROM Entidad e
					--------								inner join GrupoEconomico g on e.CodigoGrupoEconomico = g.CodigoGrupoEconomico
					--------								WHERE CodigoEntidad = ValorNivel)
					--------				WHEN '06' then ValorNivel
					--------				WHEN '09' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
					--------								WHERE cn in (SELECT CodigoNemonico FROM PatrimonioFideicomisoDetalle WHERE CodigoPatrimonioFideicomiso = ValorNivel)) 
					--------				WHEN '04' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
					--------								WHERE cn = ValorNivel)  end)) = 'S' THEN
					--------					ValorPorcentaje*0.7
					--------				ELSE
					--------					ValorPorcentaje    
					--------				END
					--------		ELSE
					--------			ValorPorcentaje    
					--------		END    
					--------ELSE ValorPorcentaje END AS ValorPorcentaje,
					--------Posicion,
				
					----------OT 9577 Inicio
					----------OT 9527 Inicio
					--------CASE WHEN (@Valor > 0 AND Posicion > 0) OR (@p_CodigoLimite = '04' AND @Valor > 0 AND Posicion < 0) THEN Posicion /@Valor * 100 ELSE 0 END 'Participacion',
					--------CASE WHEN (@Valor > 0 AND Posicion > 0) OR (@p_CodigoLimite = '04' AND @Valor > 0 AND Posicion < 0) THEN  (@Valor*(ValorPorcentaje/100)) - Posicion ELSE 0 END  'Margen',
					----------OT 9527 Fin
					----------OT 9577 Fin
					--------Alerta,ValorNivel,DescripcionNivel,PorVal 'Simbolo',@Valor 'ValorBase', @cxc 'CuentasPorCobrar', @cxp 'CuentasPorPagar',
					--------Patrimonio AS 'Patrimonio',CASE @ValorBase WHEN 'OBLITEC' THEN  (SELECT isnull(Valor,0) FROM PatrimonioEmisor WHERE TipoValor = 'OT' AND CodigoEntidad = rp.ValorNivel) ELSE @TotalInversiones END 'TotalInversion','Bancos' = @SaldoBancoValor ,






					--------'EntidadVinculada' = ISNULL((SELECT top 1 EntidadVinculada FROM entidad WHERE CodigoEntidad = ValorNivel),''),     
					--------Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,
					--------CodigoMoneda, FechaOperacion,NivelSaldoBanco,ValorPorcentajeM,DescPortafolio = '',PosicionPortafolio = 0,Valor1,Valor2,Valor3
					
					
					--------FROM #Reporte rp
					--------WHERE ValorPorcentaje <> -1 /*GTUEROS*/
					--------ORDER BY CodigoReporte
				
			----END
			
			DROP TABLE #Reporte --Eliminamos la tabla temporal  
			
		----END
		----ELSE 
		----BEGIN
		----	SELECT Top 1 @CodigoPortafolio = LC.CodigoPortafolioSBS
		----	FROM Limite L, LimiteCaracteristica LC  
		----	WHERE L.CodigoLimite = @p_CodigoLimite AND LC.CodigoLimite = @p_CodigoLimite AND LC.CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica  
		----	SELECT CodigoReporte,CodigoLimite,CodigoLimiteCaracteristica,FechaReporte,Secuencial,CodigoNivelLimite,
		----	'Tipo' = CASE WHEN Tipo = 'L' THEN 'Ley' WHEN Tipo = 'I' THEN 'Interno'END,
		----	'Tope' = CASE WHEN Tope = 'BAN' THEN 'BANDas' WHEN Tope = 'MAX' THEN 'Maximo' WHEN Tope = 'MIN' THEN 'Minimo' END,
		----	ValorPorcentaje,Posicion,Participacion,Margen,Alerta,ValorNivel,DescripcionNivel,PorVal 'Simbolo',ValorBase ,cxc 'CuentasPorCobrar', cxp 'CuentasPorPagar',   
		----	Patrimonio,TotalInversiones 'TotalInversion',SaldoBanco 'Bancos',
		----	'EntidadVinculada' = ISNULL((SELECT top 1 EntidadVinculada FROM entidad WHERE CodigoEntidad = ValorNivel),''),
		----	Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,CodigoMoneda,FechaOperacion,NivelSaldoBanco,
		----	ValorPorcentajeM,DescPortafolio = '',PosicionPortafolio = 0,Valor1,Valor2,Valor3
		----	FROM ReporteLimites
		----	WHERE FechaReporte = @p_Fecha AND CodigoLimite = @p_CodigoLimite AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica
		----	AND Escenario = @p_Escenario
		----	AND ValorPorcentaje <> -1 /*GTUEROS*/
		----	ORDER BY CodigoReporte
		----END


	----END -- OT 9362
	-- INI OT 9362


	----ELSE
	----BEGIN
	----	IF @p_procesar in (1,2)
	----	BEGIN
	----		IF @p_procesar <> 2 
	----		BEGIN
	----			DELETE FROM ReporteLimites 
	----			WHERE FechaReporte = @p_Fecha AND CodigoLimite = @p_CodigoLimite 
	----			AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica
	----			AND Escenario = @p_Escenario;
	----		END;
			

	----		PRINT 'VALORES: ' + CAST(@p_CodigoLimite AS VARCHAR) + ' - ' + @p_CodigoLimiteCaracteristica + ' - ' + CAST(@p_Fecha AS VARCHAR) + ' - ' + CAST(@p_Escenario AS VARCHAR)
	----		EXEC sp_SIT_InsertarReporteLimites_PatrimonioEntidad @p_CodigoLimite, @p_CodigoLimiteCaracteristica,@p_Fecha, @p_Escenario;
			
	----		-- Pasamos a mostrar los datos obtenidos del proceso
	----		SELECT CodigoReporte 'CodigoReporte', @p_CodigoLimite 'CodigoLimite', @p_CodigoLimiteCaracteristica 'CodigoLimiteCaracteristica',   
	----		@p_Fecha 'FechaReporte', Secuencial, CodigoNivelLimite,
	----		CASE rp.Tipo WHEN 'L' then 'Ley' WHEN 'I' then 'Interno' END Tipo, 
	----		CASE rp.Tope WHEN 'BAN' then 'BANDAS' WHEN 'MAX' then 'Maximo' WHEN 'MIN' then 'Minimo' end Tope,
	----		CASE WHEN ValorPorcentaje > -1 THEN
	----				CASE WHEN (SELECT AplicarCastigo FROM Limite WHERE CodigoLimite = @p_CodigoLimite) = 'S' THEN    
	----						CASE WHEN (SELECT EntidadVinculada FROM GrupoEconomico
	----						WHERE CodigoGrupoEconomico = (CASE (SELECT CodigoCaracteristica FROM NivelLimite 
	----							WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial = rp.Secuencial AND Situacion = 'A')
	----						WHEN '05' then (SELECT g.CodigoGrupoEconomico FROM Entidad e
	----										inner join GrupoEconomico g on e.CodigoGrupoEconomico = g.CodigoGrupoEconomico
	----										WHERE CodigoEntidad = ValorNivel)
	----						WHEN '06' then ValorNivel
	----						WHEN '09' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
	----										WHERE cn in (SELECT CodigoNemonico FROM PatrimonioFideicomisoDetalle WHERE CodigoPatrimonioFideicomiso = ValorNivel)) 
	----						WHEN '04' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
	----										WHERE cn = ValorNivel)  end)) = 'S' THEN
	----							ValorPorcentaje*0.7
	----						ELSE
	----							ValorPorcentaje    
	----						END
	----				ELSE
	----					ValorPorcentaje    
	----				END    
	----		ELSE ValorPorcentaje END AS ValorPorcentaje,
	----		rp.Posicion, rp.Participacion,
	----		rp.Margen, Alerta, ValorNivel,DescripcionNivel,PorVal 'Simbolo', ValorBase, Cxc 'CuentasPorCobrar', Cxp 'CuentasPorPagar',
	----		Patrimonio AS 'Patrimonio', TotalInversiones 'TotalInversion', SaldoBanco 'Bancos',
	----		--'EntidadVinculada' = ISNULL((SELECT top 1 EntidadVinculada FROM entidad WHERE CodigoEntidad = ValorNivel),''),     
	----		Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,
	----		CodigoMoneda, FechaOperacion, NivelSaldoBanco, ValorPorcentajeM, DescPortafolio = '',PosicionPortafolio = 0, Valor1, Valor2, Valor3
	----		FROM ReporteLimites rp
	----		WHERE FechaReporte = @p_Fecha AND CodigoLimite = @p_CodigoLimite AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica
	----		AND Escenario = @p_Escenario
	----		AND ValorPorcentaje <> -1 /*GTUEROS*/
	----		ORDER BY CodigoReporte;			
	----	END	
	----	ELSE
	----	BEGIN
	----		-- Pasamos a mostrar los datos obtenidos del proceso
	----		SELECT CodigoReporte 'CodigoReporte', @p_CodigoLimite 'CodigoLimite', @p_CodigoLimiteCaracteristica 'CodigoLimiteCaracteristica',   
	----		@p_Fecha 'FechaReporte', Secuencial, CodigoNivelLimite,
	----		CASE rp.Tipo WHEN 'L' then 'Ley' WHEN 'I' then 'Interno' END Tipo, 
	----		CASE rp.Tope WHEN 'BAN' then 'BANDAS' WHEN 'MAX' then 'Maximo' WHEN 'MIN' then 'Minimo' end Tope,
	----		CASE WHEN ValorPorcentaje > -1 THEN
	----				CASE WHEN (SELECT AplicarCastigo FROM Limite WHERE CodigoLimite = @p_CodigoLimite) = 'S' THEN    
	----						CASE WHEN (SELECT EntidadVinculada FROM GrupoEconomico
	----						WHERE CodigoGrupoEconomico = (CASE (SELECT CodigoCaracteristica FROM NivelLimite 
	----							WHERE CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica AND Secuencial = rp.Secuencial AND Situacion = 'A')
	----						WHEN '05' then (SELECT g.CodigoGrupoEconomico FROM Entidad e
	----										inner join GrupoEconomico g on e.CodigoGrupoEconomico = g.CodigoGrupoEconomico
	----										WHERE CodigoEntidad = ValorNivel)
	----						WHEN '06' then ValorNivel
	----						WHEN '09' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
	----										WHERE cn in (SELECT CodigoNemonico FROM PatrimonioFideicomisoDetalle WHERE CodigoPatrimonioFideicomiso = ValorNivel)) 
	----						WHEN '04' then (SELECT top 1 CodigoGrupoEconomico FROM VT_Instrumentos 
	----										WHERE cn = ValorNivel)  end)) = 'S' THEN
	----							ValorPorcentaje*0.7
	----						ELSE
	----							ValorPorcentaje    
	----						END
	----				ELSE
	----					ValorPorcentaje    
	----				END    
	----		ELSE ValorPorcentaje END AS ValorPorcentaje,
	----		rp.Posicion, rp.Participacion,
	----		rp.Margen, Alerta, ValorNivel,DescripcionNivel,PorVal 'Simbolo', ValorBase, Cxc 'CuentasPorCobrar', Cxp 'CuentasPorPagar',
	----		Patrimonio AS 'Patrimonio', TotalInversiones 'TotalInversion', SaldoBanco 'Bancos',
	----		--'EntidadVinculada' = ISNULL((SELECT top 1 EntidadVinculada FROM entidad WHERE CodigoEntidad = ValorNivel),''),     
	----		Factor,ValorEfectivoColocado,FloatOficioMultiple,UnidadesEmitidas,TotalActivo,TotalPasivo,FechaVencimiento,
	----		CodigoMoneda, FechaOperacion, NivelSaldoBanco, ValorPorcentajeM, DescPortafolio = '',PosicionPortafolio = 0, Valor1, Valor2, Valor3
	----		FROM ReporteLimites rp
	----		WHERE FechaReporte = @p_Fecha AND CodigoLimite = @p_CodigoLimite AND CodigoLimiteCaracteristica = @p_CodigoLimiteCaracteristica
	----		AND Escenario = @p_Escenario
	----		AND ValorPorcentaje <> -1 /*GTUEROS*/
	----		ORDER BY CodigoReporte;			
	----	END;
	----END
	------ FIN OT 9362
END
GO


PRINT '[dbo].[VT_ActividadEconomica]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='VT_ActividadEconomica') BEGIN 
	DROP VIEW [dbo].[VT_ActividadEconomica]
END 
GO

create view VT_ActividadEconomica
as
	select Valor as Codigo, Nombre as Descripcion 
	from ParametrosGenerales
	where Clasificacion in ('SECTOR_GIGS')
	AND Valor not in ('901010')
go


PRINT '[dbo].[VT_GrupoInversionesInteriorExterior]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='VT_GrupoInversionesInteriorExterior') BEGIN 
	DROP VIEW [dbo].[VT_GrupoInversionesInteriorExterior]
END 
GO

-- ================================================
-- Autor: Carlos Rumiche
-- Fecha Creación:	2019-03-20
-- Descripción:	Grupo Inversiones en el Interior / Exterior Económica
-- ================================================

create view VT_GrupoInversionesInteriorExterior
as
	select '01' as Codigo, 'Inversiones en el Interior' as Descripcion, '1' as CodigoMercadoEmisor
	union
	select '02' as Codigo, 'Inversiones en el Exterior' as Descripcion, '2' as CodigoMercadoEmisor
go

PRINT '[dbo].[VT_Entidad_SoloEstadoPeruano]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='VT_Entidad_SoloEstadoPeruano') BEGIN 
	DROP VIEW [dbo].[VT_Entidad_SoloEstadoPeruano]
END 
GO


-- ================================================
-- Autor: Carlos Rumiche
-- Fecha Creación:	2019-03-20
-- Descripción:	Entidades SOLAMENTE del Estado Peruano (Finalidad para Límites)
-- ================================================

create view VT_Entidad_SoloEstadoPeruano
as
	select Codigo, Descripcion
	from VT_Entidad 
	where Codigo IN ('BCRP','GOBP')
go

PRINT '[dbo].[VT_Entidad_SinEstadoPeruano]'
IF EXISTS (SELECT * FROM sysobjects WHERE name='VT_Entidad_SinEstadoPeruano') BEGIN 
	DROP VIEW [dbo].[VT_Entidad_SinEstadoPeruano]
END 
GO

-- ================================================
-- Autor: Carlos Rumiche
-- Fecha Creación:	2019-03-20
-- Descripción:	Entidades SIN Estado Peruano (Finalidad para Límites)
-- ================================================

create view VT_Entidad_SinEstadoPeruano
as
	select Codigo, Descripcion
	from VT_Entidad 
	where Codigo NOT IN ('BCRP','GOBP')
go



PRINT 'CaracteristicaGrupo / Limite'

declare  @CodigoLimite varchar(100) = '302'
		,@CodigoCaracteristica varchar(100) = '44' /* Rating FF Terceros */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */


-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
							where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																where CodigoLimite = @CodigoLimite))	
																
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
																									
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite

delete Limite where CodigoLimite = @CodigoLimite

-- Creacion de caracteristica ---------------------------------------------------------

DELETE CaracteristicaGrupo WHERE CodigoCaracteristica IN (@CodigoCaracteristica)

INSERT INTO CaracteristicaGrupo
(
	[CodigoCaracteristica]
	,[Descripcion]
	,[NombreTabla]
	,[NombreColumna]
	,[NombreVista]
	,[Situacion]
	,[UsuarioCreacion]
	,[FechaCreacion]
	,[HoraCreacion]
	,[UsuarioModificacion]
	,[FechaModificacion]
	,[HoraModificacion]
	,[Host]
	,[NombreColumnaVista]
	,[SQL_FROM]
	,[SQL_Where]
)
VALUES
(
	@CodigoCaracteristica,
	'Emisor con Rating-FF de "A" y que NO es el Estado Peruano',
	'',
	'E.CodigoEntidad', -- 
	'VT_Entidad_SinEstadoPeruano',
	'A',
	'SYSTEM',
	'20181029',
	'10:00:00',
	'',
	'20181029',
	'10:00:00',
	'',
	'Codigo',
	' INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero ',
	' and isnull(dbo.fn_ObtenerRatingHistoricoTercero(ctv.FechaValoracion,t.CodigoTercero, ''FF''), t.RatingFF) in (''71'',''73'')' /* 71: Código de Ratig FF = A*/
)


-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite Entidades Financieras Locales Rating FF de A'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'
	,TipoCalculo = 1
	,Tope = 'MAX'
	,CodigoPortafolio = ''	
	,Replicar = '0'	
	,TipoFactor = ''
	,SaldoBanco = 'N'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'



-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodigoEntidad VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_Entidad			
		where Codigo in (
			'BIFI'
			,'CRED'
			,'CONT'
			,'BSAN'
			,'ITBK'
			,'LETO'
			,'COFI'
			,'WIES')				 

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 10 /* Por Default debe ser 10% */	
		
		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodigoEntidad /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
PRINT 'Emisor con Rating-FF de "A" y que NO es el Estado Peruano'



declare  @CodigoLimite varchar(100) = '303'
		,@CodigoCaracteristica varchar(100) = '60' /* Rating FF Terceros */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite

-- Insert en Caracteristica -----------------------------------------------------------------


DELETE CaracteristicaGrupo WHERE CodigoCaracteristica IN (@CodigoCaracteristica)

INSERT INTO CaracteristicaGrupo
(
	[CodigoCaracteristica]
	,[Descripcion]
	,[NombreTabla]
	,[NombreColumna]
	,[NombreVista]
	,[Situacion]
	,[UsuarioCreacion]
	,[FechaCreacion]
	,[HoraCreacion]
	,[UsuarioModificacion]
	,[FechaModificacion]
	,[HoraModificacion]
	,[Host]
	,[NombreColumnaVista]
	,[SQL_FROM]
	,[SQL_Where]
)
VALUES
(
	@CodigoCaracteristica,
	'Emisor con Rating-FF diferente de "A" y que NO es el Estado Peruano',
	'',
	'E.CodigoEntidad', -- 
	'VT_Entidad_SinEstadoPeruano',
	'A',
	'SYSTEM',
	'20181029',
	'10:00:00',
	'',
	'20181029',
	'10:00:00',
	'',
	'Codigo',
	' INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero ',
	' and isnull(dbo.fn_ObtenerRatingHistoricoTercero(ctv.FechaValoracion,t.CodigoTercero, ''FF''), '''') in (''72'',''74'',''76'','''') ' /* 71: Código de Ratig FF = A*/
)



-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite Por Emisor sin Estado Peruano y Sin Entidades Financieras con FF de A'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'

	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'N'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'



-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2666 Chub2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodigoEntidad VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_Entidad_SinEstadoPeruano			
		where Codigo in (
			'BFIN'
			,'FINU'
			,'EDEL' -- 	ENEL DISTRIBUCIÓN PERÚ
			,'CPT'
			,'1812' -- EDPYME SANTANDER CONSUMO PERU S.A
			,'FCOR'
			,'ALIC'
			,'2222'
			,'GLOR'
			,'CINE'
			,'LUZS'
			,'RENP'
			,'CFIN'
			,'FCMR')				 

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 7 /* Por Default debe ser 7% */	
		
		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodigoEntidad /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
PRINT 'Emisor con Rating-FF diferente de "A" y que NO es el Estado Peruano'




declare  @CodigoLimite varchar(100) = '304'
		,@CodigoCaracteristica varchar(100) = '61' /* Rating FF Terceros */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite


-- Insert en Caracteristica -----------------------------------------------------------------

DELETE CaracteristicaGrupo WHERE CodigoCaracteristica IN (@CodigoCaracteristica)
INSERT INTO CaracteristicaGrupo
(
	[CodigoCaracteristica]
	,[Descripcion]
	,[NombreTabla]
	,[NombreColumna]
	,[NombreVista]
	,[Situacion]
	,[UsuarioCreacion]
	,[FechaCreacion]
	,[HoraCreacion]
	,[UsuarioModificacion]
	,[FechaModificacion]
	,[HoraModificacion]
	,[Host]
	,[NombreColumnaVista]
)
VALUES
(
	@CodigoCaracteristica,
	'Emisor - Estado Peruano',
	'',
	'E.CodigoEntidad', -- 
	'VT_Entidad_SoloEstadoPeruano',
	'A',
	'SYSTEM',
	'20181029',
	'10:00:00',
	'',
	'20181029',
	'10:00:00',
	'',
	'Codigo'
)


-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite Por Emisor frente al Estado Peruano'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'

	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'N'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'


-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2666 Chub2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodigoEntidad VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_Entidad_SoloEstadoPeruano			 

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 50 /* Por Default debe ser 50% */	
		
		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodigoEntidad /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
PRINT 'Emisor - Estado Peruano'




declare  @CodigoLimite varchar(100) = '305'
		,@CodigoCaracteristica varchar(100) = '50' /* Grupo Económico */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite

-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite por Grupo Económico de Empresas Vinculadas'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'

	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'S'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'


-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2666 Chub2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodigoEntidad VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_GrupoEconomico
		where Codigo in (
			 '90' --	GRUPO INTERCORP
			,'119' --	GRUPO SANTANDER
			,'70' --	GRUPO FIERRO
			,'110' -- GRUPO PICHINCHA
			,'55' -- GRUPO CREDICORP
			,'53' -- GRUPO BBVA
			,'61' --	GRUPO ENEL'
			,'127' --	GRUPO TELEFONICA
			,'112' --	GRUPO PROGRESO
			,'117' --	GRUPO RIPLEY
			,'118' --	GRUPO ROMERO
			,'107' --	GRUPO ODEBRECHT
			,'77' --	GRUPO GLORIA
			,'153'--,'AAAAA'  Gobierno Peru  
			,'120'--,'AAAAA'  -- Sepran Energia Internacional
			,'89'--,'AAAAA' -- Red Energia del peru
			,'129' --	GRUPO THE BANK OF NOVA SCOTIA
			,'76'--,'AAAAA' -- Gentera
			,'63' --	GRUPO FALABELLA
		)			 

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 15 /* Por Default debe ser 15% */	
		
		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodigoEntidad /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
PRINT 'Límite por Grupo Económico de Empresas Vinculadas'




declare  @CodigoLimite varchar(100) = '306'
		,@CodigoCaracteristica varchar(100) = '64' /* Rating FF Terceros */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite


-- Insert en Caracteristica -----------------------------------------------------------------

DELETE CaracteristicaGrupo WHERE CodigoCaracteristica IN (@CodigoCaracteristica)


DECLARE @sql_where varchar(1024)
set @sql_where = ' AND isnull(E.CodigoMercado, ''1'') {IN_NOT_IN} (select CodigoMercadoEmisor from VT_GrupoInversionesInteriorExterior Grupo where Grupo.Codigo = ''{VALOR_CARACTERISTICA}'') '

INSERT INTO CaracteristicaGrupo
(
	[CodigoCaracteristica]
	,[Descripcion]
	,[NombreTabla]
	,[NombreColumna]
	,[NombreVista]
	,[Situacion]
	,[UsuarioCreacion]
	,[FechaCreacion]
	,[HoraCreacion]
	,[UsuarioModificacion]
	,[FechaModificacion]
	,[HoraModificacion]
	,[Host]
	,[NombreColumnaVista]
	,[SQL_Where]
	,[ES_AGRUPADOR]
)
VALUES
(
	@CodigoCaracteristica,
	'Inversiones en el Interior / Exterior',
	'VT_GrupoInversionesInteriorExterior',
	'',
	'VT_GrupoInversionesInteriorExterior',
	'A',
	'SYSTEM',
	'20181029',
	'10:00:00',
	'',
	'20181029',
	'10:00:00',
	'',
	'Codigo',
	@sql_where,	
	'S'
)


-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite de Inversiones en el Exterior'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'
	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'N'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'


-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2666 Chub2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodItemDetalle VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_GrupoInversionesInteriorExterior
		WHERE Codigo in ('02') -- Solo Inversiones en el exterior

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodItemDetalle  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 50 /* Por Default debe ser 50% */	

		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodItemDetalle /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodItemDetalle 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
PRINT 'Límite de Inversiones en el Exterior'




declare  @CodigoLimite varchar(100) = '307'
		,@CodigoCaracteristica varchar(100) = '63' /* Rating FF Terceros */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite


-- Insert en Caracteristica -----------------------------------------------------------------

DELETE CaracteristicaGrupo WHERE CodigoCaracteristica IN (@CodigoCaracteristica)

INSERT INTO CaracteristicaGrupo
(
	[CodigoCaracteristica]
	,[Descripcion]
	,[NombreTabla]
	,[NombreColumna]
	,[NombreVista]
	,[Situacion]
	,[UsuarioCreacion]
	,[FechaCreacion]
	,[HoraCreacion]
	,[UsuarioModificacion]
	,[FechaModificacion]
	,[HoraModificacion]
	,[Host]
	,[NombreColumnaVista]
	,[SQL_FROM]
)
VALUES
(
	@CodigoCaracteristica,
	'Actividad Económica',
	'',
	'T.SectorGigs', -- 
	'VT_ActividadEconomica',
	'A',
	'SYSTEM',
	'20181029',
	'10:00:00',
	'',
	'20181029',
	'10:00:00',
	'',
	'Codigo',
	' INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero '
)


-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite Por Actividad Económica'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'

	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'N'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'


-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2666 Chub2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodItemDetalle VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_ActividadEconomica
		-- WHERE Codigo in ('xxxxx')

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodItemDetalle  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 40 /* Por Default debe ser 40% */	

		if @CodItemDetalle in ('902010','401010','901030','402010','402020')
			set @ValorPorc01 = 50

		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodItemDetalle /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodItemDetalle 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO



declare  @CodigoLimite varchar(100) = '308'
		,@CodigoCaracteristica varchar(100) = '62' /* Rating FF Terceros */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite


-- Insert en Caracteristica -----------------------------------------------------------------

DELETE CaracteristicaGrupo WHERE CodigoCaracteristica IN (@CodigoCaracteristica)

INSERT INTO CaracteristicaGrupo
(
	[CodigoCaracteristica]
	,[Descripcion]
	,[NombreTabla]
	,[NombreColumna]
	,[NombreVista]
	,[Situacion]
	,[UsuarioCreacion]
	,[FechaCreacion]
	,[HoraCreacion]
	,[UsuarioModificacion]
	,[FechaModificacion]
	,[HoraModificacion]
	,[Host]
	,[NombreColumnaVista]
	,[SQL_FROM]
	,[SQL_Where]
)
VALUES
(
	@CodigoCaracteristica,
	'Emisor - Considerando Solo sus Cuentas Corrientes',
	'',
	'E.CodigoEntidad', -- 
	'VT_Entidad',
	'A',
	'SYSTEM',
	'20181029',
	'10:00:00',
	'',
	'20181029',
	'10:00:00',
	'',
	'Codigo',
	' INNER JOIN Terceros T ON T.CodigoTercero = E.CodigoTercero ',
	' and 1=0 ' /* Condicion no valida, para que la querys no obtengan resultados*/
)



-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite de Cuentas Corrientes para Entidades Financieras Locales Rating FF de A'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'

	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'S'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'


-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- 2666 Chub2777	Chubb Vida	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodigoEntidad VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_Entidad
		WHERE Codigo in ('ITBK')

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 5 /* Por Default debe ser 5% */	
		
		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodigoEntidad /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
print 'Límite de Cuentas Corrientes para Entidades Financieras Locales Rating FF de A'




declare  @CodigoLimite varchar(100) = '309'
		,@CodigoCaracteristica varchar(100) = '05' /* Emisor */
		,@TipoLimite varchar(1) = 'L'  /* L: Local, I: Interno */

-- Eliminacion Previa -----------------------------------------------------------------
delete ReporteLimites where CodigoLimite = @CodigoLimite

delete DetalleNivelLimite2 
	where CodigoNivelLimite in (select CodigoNivelLimite from NivelLimite 
								where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
																	where CodigoLimite = @CodigoLimite))	
delete NivelLimite where CodigoLimiteCaracteristica in (select CodigoLimiteCaracteristica from LimiteCaracteristica 
														where CodigoLimite = @CodigoLimite)
delete LimiteCaracteristica where CodigoLimite = @CodigoLimite
delete Limite where CodigoLimite = @CodigoLimite

-- Insert en Limite -----------------------------------------------------------------
insert into Limite (
	CodigoLimite
	,NombreLimite
	,Tipo
	,UnidadPosicion
	,ValorBase
	,ClaseLimite
	,AplicarCastigo
	,AplicaDPZ

	,TipoCalculo
	,Tope
	,CodigoPortafolio
	,Replicar
	,TipoFactor
	,SaldoBanco
	,Posicion	
	,MarketShare
	,IsAgrupadoPorcentaje
	,ValorAgrupadoPorcentaje
	,Cuadrar

	,Situacion,UsuarioCreacion,FechaCreacion	
) 
select
	CodigoLimite = @CodigoLimite
	,NombreLimite = 'Límite de Certificados de Participación de Fondos Mutuos o Fondos de Inversión'
	,Tipo = @TipoLimite
	,UnidadPosicion = 'VM'
	,ValorBase = 'OBLIG_TECNICAS'
	,ClaseLimite = 'F'
	,AplicarCastigo = 'N'
	,AplicaDPZ = 'S'

	,TipoCalculo = 1		---
	,Tope = 'MAX'			---
	,CodigoPortafolio = ''	
	,Replicar = '0'			---
	,TipoFactor = ''
	,SaldoBanco = 'N'
	,Posicion = 'GI000'
	,MarketShare = 0
	,IsAgrupadoPorcentaje = 0
	,ValorAgrupadoPorcentaje = 0
	,Cuadrar = 0

	,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'


-- Insert Limite-Portafolio y Limite-Caracteristica ----------------------------------------------------------------------------------------------------------
declare @CodigoLimiteCaracteristica int
	,@CodigoNivelLimite int
	,@CodigoPortafolioSBS varchar(100)
	,@ValorPorc01 numeric(27,7)

DECLARE db_cursor CURSOR FOR 
	select CodigoPortafolioSBS from Portafolio where Situacion = 'A'
	and CodigoPortafolioSBS in ('2666','2777') -- xxxx Configurar COFACE	

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS  

WHILE @@FETCH_STATUS = 0  
BEGIN

	set @CodigoLimiteCaracteristica  = isnull((select max(convert(int,CodigoLimiteCaracteristica)) from LimiteCaracteristica), 0)
	set @CodigoNivelLimite  = isnull((select max(convert(int,CodigoNivelLimite)) from NivelLimite), 0)

	-- Insert en LimiteCaracteristica (Limite-Portafolio) -----------------------------------------------------------------
	set @CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica + 1

	insert into LimiteCaracteristica (
		CodigoLimiteCaracteristica
		,CodigoPortafolioSBS
		,CodigoLimite
		,Tipo
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,PorcentajeCercaLimite
	)
	select 
		CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,CodigoPortafolioSBS = @CodigoPortafolioSBS
		,CodigoLimite = @CodigoLimite
		,Tipo = @TipoLimite
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,PorcentajeCercaLimite = 0

	-- Insert en NivelLimite (Limite-Caracteristica) -----------------------------------------------------------------
	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '1'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = '00'
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica /*Código registro Padre*/
		,FlagTipoPorcentaje = 'G' /*General*/
		,ValorPorcentaje = 0
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	set @CodigoNivelLimite = @CodigoNivelLimite + 1
	insert into NivelLimite (
		Secuencial
		,CodigoNivelLimite
		,CodigoCaracteristica
		,CodigoLimiteCaracteristica
		,FlagTipoPorcentaje
		,ValorPorcentaje
		,Situacion ,UsuarioCreacion ,FechaCreacion
		,ValorPorcentajeM, BandaLimites
	)
	select
		Secuencial = '2'
		,CodigoNivelLimite = @CodigoNivelLimite
		,CodigoCaracteristica = @CodigoCaracteristica 
		,CodigoLimiteCaracteristica = @CodigoLimiteCaracteristica
		,FlagTipoPorcentaje = 'D' /*a nivel de Detalle*/
		,ValorPorcentaje = Null
		,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
		,ValorPorcentajeM = 0, BandaLimites = ''

	-- Insert en DetalleNivelLimite2 (Valores de Porcentaje para cada caracteristica del Limite) -----------------------------------------------------------------

	DECLARE @CodigoEntidad VARCHAR(40)
		
	DECLARE db_cursorDetalle CURSOR FOR 
		SELECT Codigo FROM VT_Entidad
		where Codigo in ('1122')

	OPEN db_cursorDetalle
	FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad  	

	WHILE @@FETCH_STATUS = 0
	BEGIN	

		set @ValorPorc01 = 20 /* Por Default debe ser 7% */	
		
		insert into DetalleNivelLimite2 (
			CodigoNivelLimite
			,CodigoCaracteristica
			,ValorCaracteristica		
			,ValorPorcentaje

			,Situacion ,UsuarioCreacion ,FechaCreacion
			,ClaseNormativa -- ,ValorPorcentajeM, BandaLimites
		)
		select
			CodigoNivelLimite = @CodigoNivelLimite -- Ultimo insertado
			,CodigoCaracteristica = @CodigoCaracteristica
			,ValorCaracteristica = @CodigoEntidad /* Entidades Seleccionadas */		
			,ValorPorcentaje = @ValorPorc01

			,Situacion = 'A', UsuarioCreacion = 'ADMIN', FechaCreacion	= '20181015'
			,ClaseNormativa = '' -- ,ValorPorcentajeM = 0, BandaLimites = ''

		FETCH NEXT FROM db_cursorDetalle INTO @CodigoEntidad 
	END 

	CLOSE db_cursorDetalle  
	DEALLOCATE db_cursorDetalle 	

	FETCH NEXT FROM db_cursor INTO @CodigoPortafolioSBS 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

GO
print 'Límite de Certificados de Participación de Fondos Mutuos o Fondos de Inversión'


PRINT 'INSERT INTO RatingTerceroHistoria'


DELETE FROM RatingTerceroHistoria WHERE FechaCreacion = 20181231
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20101036813', '43', '45', 'P500625', 20181231, '73', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('2025970241', '45', '45', 'P500625', 20181231, '72', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20369155360', '45', '44', 'P500625', 20181231, '76', 'CP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20513074370', '44', '45', 'P500625', 20181231, '71', 'CP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20522291201', '46', '45', 'P500625', 20181231, '76', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20377892918', '45', '47', 'P500625', 20181231, '71', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20100047218', '42', '42', 'P500625', 20181231, '73', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20100130204', '42', '43', 'P500625', 20181231, '73', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20255993225', '44', '43', 'P500625', 20181231, '71', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20382036655', '43', '43', 'P500625', 20181231, '71', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20546892175', '44', '43', 'P500625', 20181231, '71', 'CP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20100043140', '43', '42', 'P500625', 20181231, '73', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20100053455', '43', '44', 'P500625', 20181231, '71', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20330401991', '45', '44', 'P500625', 20181231, '72', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20100116392', '44', '43', 'P500625', 20181231, '71', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('2717', '42', '42', 'P500625', 20181231, '71', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20516711559', '42', '43', 'P500625', 20181231, '73', 'CP/LP')
GO

INSERT INTO dbo.RatingTerceroHistoria (CodigoTercero, Rating, CodigoCalificionOficial, UsuarioCreacion, FechaCreacion, RatingFF, LineaPlazo)
VALUES ('20100105862', '44', '47', 'P500625', 20181231, '72', 'CP')
GO
PRINT 'Ingreso de Rating Tercero Historia'



PRINT 'INSERT INTO RatingValorHistoria'
DELETE FROM RatingValorHistoria WHERE FechaCreacion = 20181231
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP16820M011', '4', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP75100K291', '4', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP12000M352', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP16870Q181', '5', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP14000M137', '1', 'P500625', 20181231, 'MANDA', 20180926, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70252M218', '1', 'P500625', 20181231, 'MANDA', 20190225, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP16870M016', '5', 'P500625', 20181231, 'MANDA', 20180925, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11100M286', '3', 'P500625', 20181231, 'MANDA', 20180925, 'Equilibrium')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70101M639', '1', 'P500625', 20181231, 'MANDA', 20181128, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP72900M012', '2', 'P500625', 20181231, 'MANDA', 20181030, 'Equilibrium')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T707', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP14800D139', '3', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP14300Q686', '4', 'P500625', 20181231, 'MANDA', 20180924, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('US715638AW21', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11600M293', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP16997V026', '5', 'P500625', 20181231, 'MANDA', 20181031, 'Equilibrium')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('US715638AS19', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11600M251', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP75700M096', '3', 'P500625', 20181231, 'MANDA', 20181129, 'Equilibrium')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP13100K049', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP75100K283', '4', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP14150M056', '4', 'P500625', 20181231, 'MANDA', 20180928, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP75100K341', '4', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70500M723', '2', 'P500625', 20181231, 'MANDA', 20180912, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP12000M303', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11600M210', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP14150M072', '4', 'P500625', 20181231, 'MANDA', 20180928, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11600K057', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70500M699', '2', 'P500625', 20181231, 'MANDA', 20180912, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP12000M337', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70310M149', '1', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70310M172', '1', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11600M194', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP12000M311', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP75100K309', '4', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70101M514', '1', 'P500625', 20181231, 'MANDA', 20181128, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70101M605', '1', 'P500625', 20181231, 'MANDA', 20181128, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70252M192', '1', 'P500625', 20181231, 'MANDA', 20190225, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP72840M010', '4', 'P500625', 20181231, 'MANDA', 20181122, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP21400VA24', '1', 'P500625', 20181231, 'MANDA', 20180910, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70500M731', '2', 'P500625', 20181231, 'MANDA', 20180912, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP36100M139', '1', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP12100K065', '2', 'P500625', 20181231, 'MANDA', 20181214, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP16940Q109', '6', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP70500M756', '1', 'P500625', 20181231, 'MANDA', 20180912, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP21400M072', '1', 'P500625', 20181231, 'MANDA', 20180910, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP36100M154', '1', 'P500625', 20181231, 'MANDA', 20181130, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP16870Q165', '6', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP11600K065', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP12000M329', '1', 'P500625', 20181231, 'MANDA', 20180927, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T723', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T673', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T665', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T699', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T681', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP14300M172', '4', 'P500625', 20181231, 'MANDA', 20180924, 'Class')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T715', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000T657', '1', 'P500625', 20181231, 'MANDA', 0, 'Apoyo')
GO

INSERT INTO dbo.RatingValorHistoria (CodigoIsin, Rating, UsuarioCreacion, FechaCreacion, Negocio, FechaClasificacion, Clasificadora)
VALUES ('PEP01000CY33', '41', 'P500625', 20181231, 'MANDA', 20140207, 'MOODYS')
GO

PRINT 'dbo.Terceros'




UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = ' 2033864680'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '01102018050'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '10102018010'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '1209'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '1345123'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '20090349494'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '20100041953'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '20100327172'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '2021355209'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '20259702411'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '246810'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = '261018'
GO

UPDATE dbo.Terceros
SET SectorGigs = NULL
WHERE CodigoTercero = 'VNCK'
GO

UPDATE dbo.Terceros
SET SectorGigs = ''
WHERE CodigoTercero = '0425170119'
GO

UPDATE dbo.Terceros
SET SectorGigs = ''
WHERE CodigoTercero = '12121801190'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '06092018104'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '08051826333'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '08092017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '0816'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '0903'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '10001'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '100052'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '100118'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '10012324'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '100217'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1020001'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '10748614'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1107'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1109'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '111'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '11111110'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '111111111'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1111111111'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '11111111111'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '11111111122'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1111111119'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '11111113'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '11111115'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '11111120'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '117587666'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '120717'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '121216'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1234'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '13'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1312'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1391012'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1408201801'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1412'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '149835'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '15'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '150217'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '151515'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '15263'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '15472184'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '158'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '159654'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '16032017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '170711'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '170925'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '18'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '18012018'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1892'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '1910'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '200075835'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100041791'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100042330'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100100127'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100118174'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100121477'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2010012627'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100130972'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100161771'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100210909'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100576474'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20100611563'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20101031340'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20117920811'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2012'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20126440350'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2013'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20131370564'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20133530003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2015'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20152153184'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20154981293'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20157036794'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2016'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20164766251'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2019'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '202'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2020'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20203188219'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20213552083'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20256192269'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20259829594'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20265391886'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20292928841'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20297652274'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20308668780'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20330791412'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20332874960'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20333363900'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20337996834'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20338205261'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20338646802'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20342020870'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20375312868'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20383316473'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20392456555'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20425641761'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2042691186'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20433434324'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20441805960'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20463627488'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20492172694'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20492942121'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20499432021'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '205'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2050259706'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20503595819'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20503955882'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20504311342'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20504893295'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20508260581'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20510000138'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20510615949'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20510889135'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20512293710'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20513155965'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20513808519'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20515577433'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20516670429'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20516748711'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20516980452'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20517406610'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20519005531'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20519379024'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20519495369'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20520554310'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20520974289'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20543406997'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20543508218'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20546209181'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20546209343'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20546892175'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20551653600'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20553430551'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20553431442'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20553431795'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20553961085'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20553961590'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20555481374'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20557811258'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20562933370'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600316142'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600627717'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600661737'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600669282'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600769708'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600800478'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600808720'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20600808801'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '20651654516'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2082'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '210617'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '213354'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2196026'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '22'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2299356'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2301'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2309255'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '233051853'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '25052018888'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '251'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2569572'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '261'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2711'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2717'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2719216'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2773311'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '2806585'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '28082018053'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '29082018948'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '291765'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '301'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '304'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '333'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '33333333'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '3333333333'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '345'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '370661011'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '3824988'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4004'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4100'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '41324'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '419841'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '425846'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4300'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4304'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '44007878'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '444444444'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '451423'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '45631'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '46213'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464286319'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464286426'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464286566'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464286608'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464286657'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464288216'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464298697'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '464342021'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '46434738'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '4719745'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '478521'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '48251'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '48917'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '49481056'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '5100'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '5105'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '5177'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '5270'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '564561'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '57'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '5899999'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '6001'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '6114613'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '6114614'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '61502184524'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '621345'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '6262'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '632135'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '63214'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '651624'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '651654'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '68152934215'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '68441'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '69'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7001'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7002'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7005'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7006'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7007'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7008'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7011'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7012'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7014'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7015'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7016'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7019'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7020'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7021'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7026'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7040'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7041'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7042'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7046'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7047'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7654651'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '7777'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '785393'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '798343'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '852258'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '852654'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '875621'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '87913'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '89712'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '911578'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '922042874'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '9562'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '981749'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '987'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '98701'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '9874161'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = '987654321'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'CREA'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'DEUX'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'EIBK'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'EWN'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'GAM'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'INTERFI'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'LARR'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'NOMURA'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'Q*'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'Schroder '
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'VANE'
GO

UPDATE dbo.Terceros
SET SectorGigs = '1'
WHERE CodigoTercero = 'XPIN'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '0801201919'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '100008'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '110000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '1111111112'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '12030507'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '140317'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '15831072'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20100030595'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20100047307'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20100116635'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20100116716'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20100200261'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20100209641'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20101070248'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20108166534'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20122476309'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20130098488'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20130200789'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20132243230'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '2014'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20170879750'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20297508661'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20331285251'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20333725071'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20382036655'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20401319817'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20460972621'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20472315154'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20478076038'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20478076381'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20492912132'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20511230188'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20513074370'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20514900451'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '20543681513'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '21660205'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '22102018122'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '22548'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '24102018113'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '27102017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '274'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '28102017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '333333333'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '335'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '352'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '3520'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '36'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '456331'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '47'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '5002'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '5006'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '5101'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '5102'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '511016'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '54'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '55'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '56'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '5713203734'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '6002'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '6003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '6004'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '621354'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '65'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '6513241'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '65999262222'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '6688'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '7018'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '7853653'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '84177300-4'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '842313'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '888888'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '89'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = '999999999'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = 'ADCAP'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = 'BSP'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = 'INTERCORP'
GO

UPDATE dbo.Terceros
SET SectorGigs = '101010'
WHERE CodigoTercero = 'ISLP'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '1000005'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '100003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '20101045995'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '20301837896'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '203018378961'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '20390625007'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '20504815812'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '332133'
GO

UPDATE dbo.Terceros
SET SectorGigs = '11'
WHERE CodigoTercero = '5'
GO

UPDATE dbo.Terceros
SET SectorGigs = '12'
WHERE CodigoTercero = '230112'
GO

UPDATE dbo.Terceros
SET SectorGigs = '12'
WHERE CodigoTercero = '46321'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '132536'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20100137390'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20100163552'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20100191921'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20100218471'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20100220378'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20129497077'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20131742372'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '2013353000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20170072465'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20347642551'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '20429683581'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '225611567'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '2718301'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '38'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '651354'
GO

UPDATE dbo.Terceros
SET SectorGigs = '253010'
WHERE CodigoTercero = '9027746'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '16911213'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '20331066703'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '20510992904'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '22052018010'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '2909'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '302'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '45689'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '651321'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255010'
WHERE CodigoTercero = '6812651'
GO

UPDATE dbo.Terceros
SET SectorGigs = '255030'
WHERE CodigoTercero = '20100128056'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '020317'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '03052018888'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '09042018888'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '10000009'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '10000018'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '1000004'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '100030'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '10041201888'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '1010005'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '10114'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '1040003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '1077'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '1100006'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '12'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '12654'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '1661018'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '171122'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '19011702'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '19012017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '2003115'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100017572'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100023386'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100027292'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100039207'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100070970'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100103657'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100110513'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100114187'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100123500'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100124221'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100136237'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100136741'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100147514'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100177421'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100181615'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100228352'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20100971772'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20101024645'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20102078781'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20104121374'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20110345519'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20110356553'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20112823264'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20137025354'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20137291313'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20143229816'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20160286068'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20163898200'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20170814'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20206553481'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20209133394'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20217427593'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20251293181'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20255322363'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20262786511'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20330791501'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20332600592'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20402885549'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20423264617'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20424297161'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20465261634'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20477815236'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20501439020'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20503238463'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20506675457'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20507828915'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20508100206'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20508433515'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20509093521'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20511866210'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20513320915'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20517057003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20519098017'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '20535936669'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '2085'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '211216'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '212223'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '2164'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '2196015'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '222222222222'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '271216'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '2727'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '29770441'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '30'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '31'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '321324'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '345020242204'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '356718570'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '36447508'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '4303'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '456321'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '457862'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '4686841'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '49841083'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '4987'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '5024'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '51025'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '550344303'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '56432'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '65321324'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '6979001'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '7198235'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '78565'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '865126106'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '8738681037'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '956'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '9654253'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '97'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = '98701142'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = 'ELEM'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = 'NEXARES'
GO

UPDATE dbo.Terceros
SET SectorGigs = '3'
WHERE CodigoTercero = 'UNACEM'
GO

UPDATE dbo.Terceros
SET SectorGigs = '302020'
WHERE CodigoTercero = '20100055237'
GO

UPDATE dbo.Terceros
SET SectorGigs = '302020'
WHERE CodigoTercero = '20100190797'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '0085'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '1000001'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '130117'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '153873'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '159'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '191544'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '20100031303'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '20100034744'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '20100113610'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '20141189850'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '20337564373'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '2051181'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '20521418185'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '2513213'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '32432'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '3654606'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '369181377'
GO

UPDATE dbo.Terceros
SET SectorGigs = '4'
WHERE CodigoTercero = '632133'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20100043140'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20100047218'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20100053455'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20100105862'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20100130204'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20101036813'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '2025970241'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20330401991'
GO

UPDATE dbo.Terceros
SET SectorGigs = '401010'
WHERE CodigoTercero = '20516711559'
GO

UPDATE dbo.Terceros
SET SectorGigs = '402010'
WHERE CodigoTercero = '181217'
GO

UPDATE dbo.Terceros
SET SectorGigs = '402010'
WHERE CodigoTercero = '20100116392'
GO

UPDATE dbo.Terceros
SET SectorGigs = '402010'
WHERE CodigoTercero = '20255993225'
GO

UPDATE dbo.Terceros
SET SectorGigs = '402010'
WHERE CodigoTercero = '20369155360'
GO

UPDATE dbo.Terceros
SET SectorGigs = '402010'
WHERE CodigoTercero = '20377892918'
GO

UPDATE dbo.Terceros
SET SectorGigs = '402010'
WHERE CodigoTercero = '20522291201'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '1010008'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '110003'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '20131823020'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '20131867744'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '2222222222'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '2311454'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '249051044'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '285'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '303'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '320321310'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '4589536'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '4650650'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '47586914'
GO

UPDATE dbo.Terceros
SET SectorGigs = '5'
WHERE CodigoTercero = '7254233'
GO

UPDATE dbo.Terceros
SET SectorGigs = '501010'
WHERE CodigoTercero = '20100017491'
GO

UPDATE dbo.Terceros
SET SectorGigs = '551010'
WHERE CodigoTercero = '20269985900'
GO

UPDATE dbo.Terceros
SET SectorGigs = '551010'
WHERE CodigoTercero = '20331898008'
GO

UPDATE dbo.Terceros
SET SectorGigs = '551010'
WHERE CodigoTercero = '20504645046'
GO

UPDATE dbo.Terceros
SET SectorGigs = '551010'
WHERE CodigoTercero = '2511'
GO

UPDATE dbo.Terceros
SET SectorGigs = '551040'
WHERE CodigoTercero = '11116666'
GO

UPDATE dbo.Terceros
SET SectorGigs = '6'
WHERE CodigoTercero = '321131015'
GO

UPDATE dbo.Terceros
SET SectorGigs = '6'
WHERE CodigoTercero = '32134'
GO

UPDATE dbo.Terceros
SET SectorGigs = '6'
WHERE CodigoTercero = '510121215'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '091216'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '20100017149'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '20100177774'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '20424653561'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '20467534026'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '452312'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '5000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '80020'
GO

UPDATE dbo.Terceros
SET SectorGigs = '9'
WHERE CodigoTercero = '8795236'
GO

UPDATE dbo.Terceros
SET SectorGigs = '901010'
WHERE CodigoTercero = '1000'
GO

UPDATE dbo.Terceros
SET SectorGigs = '903010'
WHERE CodigoTercero = '37833100'
GO


PRINT 'ACTUALIZAICON PARAMETRIA PARA LIMITES'

UPDATE entidad SET CodigoGrupoEconomico = 90
where CodigoEntidad = 'CINE'
Go

UPDATE GrupoEconomico set Descripcion = 'RED DE ENERGÍA DEL PERÚ'
where CodigoGrupoEconomico = '89'
GO

UPDATE Terceros set SectorGigs = '301010'
WHERE CodigoTercero = '20100055237'
GO




IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 