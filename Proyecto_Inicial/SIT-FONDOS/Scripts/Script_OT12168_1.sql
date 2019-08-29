USE [SIT-FONDOS]
GO 
PRINT '[dbo].[CodigoPostal_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[CodigoPostal_SeleccionarPorFiltro]
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ================================================
-- Autor:		Daphne Orihuela
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'CodigoPostal' que coincidan con los 
-- parámetros de entrada
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CodigoPostal_SeleccionarPorFiltro] (
	@p_CodigoPostal varchar(3),  --Codigo de CodigoPostal
	@p_Descripcion varchar(30),  --Descripcion de CodigoPostal
	@p_Situacion varchar(1)  --Situacion del registro CodigoPostal.Inactivo=I  ,  Activo=A
)
AS

SET NOCOUNT ON

SELECT
	c.CodigoPostal,
	c.Descripcion,
	c.UsuarioCreacion,
	c.FechaCreacion,
	c.UsuarioModificacion,
	c.HoraCreacion,
	c.FechaModificacion,
	c.Situacion,
	c.Host,
	c.HoraModificacion,
	CASE c.Situacion WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'NombreSituacion'
FROM
	CodigoPostal as c
WHERE
((c.CodigoPostal like COALESCE(@p_CodigoPostal,c.CodigoPostal) + '%') or (c.CodigoPostal='')) and 
((c.Descripcion like  COALESCE(@p_Descripcion,c.Descripcion) + '%') or (c.Descripcion='')) and
((c.Situacion like  COALESCE(@p_Situacion,c.Situacion) + '%') or (c.Situacion=''))
ORDER BY NombreSituacion asc -- OT12168 

GRANT EXECUTE ON [dbo].[CodigoPostal_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[CotizacionVAC_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[CotizacionVAC_SeleccionarPorFiltro]
GO
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[CotizacionVAC_SeleccionarPorFiltro]
(
	@p_Fecha	NUMERIC(8,0) = 0,
	@p_CodigoIndicador VARCHAR(10) = ''
)
AS

	SET NOCOUNT ON

	SELECT distinct
		CV.[CodigoIndicador], 
		IR.NombreIndicador AS 'NombreIndicador',
		dbo.ConvertirFecha(CV.[Fecha]) as 'Fecha', 
		CV.[CodigoPortafolioSBS], 
		CV.[Valor], 
		CV.[Situacion],
		PG.Nombre AS 'NombreSituacion'
	FROM
		ValorIndicador AS CV
		INNER JOIN ParametrosGenerales AS PG ON CV.Situacion = PG.Valor
		LEFT JOIN Indicador AS IR ON CV.CodigoIndicador = IR.CodigoIndicador
	WHERE
		CV.CodigoIndicador = (CASE WHEN LEN(@p_CodigoIndicador) = 0 THEN CV.CodigoIndicador ELSE @p_CodigoIndicador END)
		AND CV.Fecha = (CASE WHEN @p_Fecha = 0 THEN CV.Fecha ELSE @p_Fecha END)
		AND PG.Clasificacion = 'Situación'
--	GROUP BY
	--	CV.[CodigoIndicador],
		--IR.NombreIndicador,
--		CV.[Fecha], 
--		CV.[Situacion],
--		PG.Nombre

	order by NombreIndicador -- OT12168 

GRANT EXECUTE ON [dbo].[CotizacionVAC_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Custodio_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Custodio_SeleccionarPorFiltro]
GO

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

--EXEC Custodio_SeleccionarPorFiltro '','',''
CREATE   PROCEDURE [dbo].[Custodio_SeleccionarPorFiltro] 
(
	@p_CodigoCustodio varchar(12),  --Codigo de Custodio
	@p_Descripcion varchar(30),  --Descripcion de Custodio
	@p_Situacion varchar(1)  --Situacion del registro [Inactivo=I  ,  Activo=A]
)
AS

SET NOCOUNT ON

SELECT
	[CodigoCustodio],
	[UsuarioCreacion],
	[Descripcion],
	[FechaCreacion],
	[UsuarioModificacion],
	[HoraCreacion],
	[FechaModificacion],
	[Situacion],
	PG.Nombre AS 'NombreSituacion',
	[Host],
	[HoraModificacion]
FROM
	[Custodio] AS CU
	INNER JOIN ParametrosGenerales AS PG ON CU.Situacion = PG.Valor
WHERE
	((CodigoCustodio like COALESCE(@p_CodigoCustodio,CodigoCustodio) + '%') or (CodigoCustodio='')) and 
	((Descripcion like COALESCE(@p_Descripcion,Descripcion) + '%') or (Descripcion='')) and
	((Situacion like COALESCE(@p_Situacion,Situacion) + '%') or (Situacion=''))
	AND PG.Clasificacion = 'Situación'
ORDER BY
	NombreSituacion ASC -- OT12168

GRANT EXECUTE ON [dbo].[Custodio_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[GrupoEconomico_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[GrupoEconomico_SeleccionarPorFiltro]
GO

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GrupoEconomico_SeleccionarPorFiltro] (  
 @p_CodigoGrupoEconomico varchar(4),  --Codigo de GrupoEconomico  
 @p_Descripcion varchar(50),  --Descripcion de GrupoEconomico  
 @p_Situacion varchar(1)  --Situacion del registro [Inactivo=I  ,  Activo=A]  
)  
AS  
  
SET NOCOUNT ON  
  
SELECT  
 [CodigoGrupoEconomico],  
 [Situacion],  
 [UsuarioCreacion],  
 [Descripcion],  
 [FechaCreacion],  
 [UsuarioModificacion],  
 [HoraCreacion],  
 [FechaModificacion],  
 [Host],  
 [HoraModificacion],  
 [EntidadVinculada],
 (SELECT Nombre FROM ParametrosGenerales WHERE Valor = Situacion AND Clasificacion = 'Situación') AS 'NombreSituacion'  
FROM  
 [GrupoEconomico]  
WHERE  
((CodigoGrupoEconomico like '%' + COALESCE(@p_CodigoGrupoEconomico,CodigoGrupoEconomico) + '%') or (CodigoGrupoEconomico='')) and   
((Descripcion like  COALESCE(@p_Descripcion,Descripcion) + '%') or (Descripcion='')) and  
((Situacion like  '%'+ COALESCE(@p_Situacion,Situacion) + '%') or (Situacion=''))  
ORDER BY  
 NombreSituacion ASC -- OT12168

GRANT EXECUTE ON [dbo].[GrupoEconomico_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Limite_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Limite_SeleccionarPorFiltro]
GO

---------------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 22/11/2018
--	Creado por: Carlos Rumiche.
--	Nro. Orden de Trabajo: 11655
--	Descripcion del cambio: 
---------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
create procedure [dbo].[Limite_SeleccionarPorFiltro]  
(@p_CodigoLimite varchar(5) = '',  
@p_nombrelimite varchar(50) = '',  
@p_Situacion varchar(1) =''  
)  
   
as  
begin  
	select   
		codigoLimite,  
		NombreLimite,
		(SELECT Nombre FROM ParametrosGenerales WHERE Valor = tipoCalculo AND Clasificacion = 'TipoCalcLi') AS 'tipoCalculo',  
		(SELECT Nombre FROM ParametrosGenerales WHERE Valor = UnidadPosicion AND Clasificacion = 'UnidPosc') AS 'UnidadPosicion',  
		(SELECT Nombre FROM ParametrosGenerales WHERE Valor = valorBase AND Clasificacion = 'ValBase') AS 'valorBase',  
		(SELECT Nombre FROM ParametrosGenerales WHERE Valor = ClaseLimite AND Clasificacion = 'ClasLim') AS 'ClaseLimite',  
		AplicarCastigo,  
		TipoFactor,  
		SaldoBanco,  
		(SELECT Nombre FROM ParametrosGenerales WHERE Valor = Situacion AND Clasificacion = 'Situación') AS 'Situacion' ,
		codigoLimite + '-' + NombreLimite as Descripcion 
		,TipoLimite = case when Tipo = 'L' then 'Según Ley' else 'Interno' end 
	from Limite  
	where (CodigoLimite = @p_CodigoLimite or isnull(@p_CodigoLimite,'') = '')
	and (NombreLimite like '%' + @p_nombrelimite + '%' or isnull(@p_nombrelimite,'') = '')
	and (Situacion = @p_Situacion or isnull(@p_Situacion,'') = '')	
	ORDER BY Situacion, convert(int,codigoLimite), NombreLimite -- OT12168
end

GRANT EXECUTE ON [dbo].[Limite_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]  
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[MatrizContable_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[MatrizContable_SeleccionarPorFiltro]
GO

-----------------------------------------------------------------------------------------------------------
--Objetivo: Seleccionar la parametria de matriz contable
-----------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 31/10/2016
--	Modificado por: Carlos Espejo
--	Nro. Orden de Trabajo: 9515
--	Descripcion del cambio: Filtro por campo nuevo, MatrizFondo
-----------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MatrizContable_SeleccionarPorFiltro](@p_Descripcion varchar(20),@p_Situacion varchar(1),@p_MatrizFondo CHAR(1))
AS
BEGIN
	SELECT m.CodigoMatrizContable,m.Descripcion,m.UsuarioCreacion,m.FechaCreacion,m.UsuarioModificacion,m.HoraCreacion,  
	m.FechaModificacion,m.Host,m.HoraModificacion,UPPER(p.Nombre) AS 'Situacion'
	FROM MatrizContable M
	JOIN ParametrosGenerales  p  ON P.Clasificacion = 'Situación' AND P.Valor = M.Situacion 
	WHERE M.MatrizFondo = @p_MatrizFondo AND
	((m.Descripcion like  LTRIM(RTRIM(COALESCE(@p_Descripcion,m.Descripcion))) + '%') or (m.Descripcion=''))   
	AND M.Situacion = @p_Situacion
	order by Situacion asc -- OT12168
END

GRANT EXECUTE ON [dbo].[MatrizContable_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Negocio_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Negocio_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Daphne Orihuela
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'Negocio' que coincidan con los parámetros
-- de entrada
-- ================================================

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE       PROCEDURE [dbo].[Negocio_SeleccionarPorFiltro] (
	@p_CodigoNegocio varchar(4),  --Codigo de Negocio
	@p_Descripcion varchar(30),  --Descripcion de Negocio
	@p_Situacion varchar(1)  --Situacion del registro n.Inactivo=I  ,  Activo=A
)
AS

SET NOCOUNT ON

SELECT
	n.CodigoNegocio,
	n.Descripcion,
	n.UsuarioCreacion,
	n.FechaCreacion,
	n.UsuarioModificacion,
	n.HoraCreacion,
	n.FechaModificacion,
	n.Situacion,
	n.Host,
	n.HoraModificacion,
	ISNULL(CodigoMoneda, '0') CodigoMoneda,
	CASE n.Situacion WHEN 'A' THEN 'ACTIVO' WHEN 'I' THEN 'INACTIVO' END AS 'NombreSituacion'
FROM
	Negocio as n
WHERE
((n.CodigoNegocio like COALESCE(@p_CodigoNegocio,n.CodigoNegocio) + '%') or (n.CodigoNegocio='')) and 
((n.Descripcion like COALESCE(@p_Descripcion,n.Descripcion) + '%') or (n.Descripcion='')) and
((n.Situacion like COALESCE(@p_Situacion,n.Situacion) + '%') or (n.Situacion=''))
ORDER BY NombreSituacion asc -- OT12168

GRANT EXECUTE ON [dbo].[Negocio_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Pais_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Pais_SeleccionarPorFiltro]
GO

-- ================================================    
-- Autor:  Daphne Orihuela    
-- Fecha Creación: 10/09/2007    
-- Descripción:  Selecciona los registros de la tabla 'Pais' que coincida con los parámetros de    
-- entrada     
-- ================================================    
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
--EXEC Pais_SeleccionarPorFiltro '','','I'    
CREATE   PROCEDURE [dbo].[Pais_SeleccionarPorFiltro] (    
 @p_CodigoPais varchar(4) = '',  --Codigo de Pais    
 @p_Descripcion varchar(30) = '',  --Descripcion de Pais    
 @p_Situacion varchar(1) = ''  --Situacion del registro [Inactivo=I  ,  Activo=A]      
)    
AS    
    
SET NOCOUNT ON    
    
SELECT    
 [CodigoPais],    
 [Descripcion],    
 [UsuarioCreacion],    
 [FechaCreacion],    
 [UsuarioModificacion],    
 [HoraCreacion],    
 [FechaModificacion],    
 [Situacion],    
 CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'NombreSituacion',     
 [Host],    
 [HoraModificacion],  
 [Paraiso] ,-- JRM 20100429
 [DobleImposicion] -- JLPA 20100602 
FROM    
 [Pais]    
WHERE    
((CodigoPais = RTRIM(LTRIM(COALESCE(@p_CodigoPais,CodigoPais)))) or (@p_CodigoPais='')) and     
((Descripcion like '%' + RTRIM(LTRIM(COALESCE(@p_Descripcion,Descripcion))) + '%') or (Descripcion='')) and    
((Situacion like '%' + RTRIM(LTRIM(COALESCE(@p_Situacion,Situacion))) + '%') or (Situacion=''))    
ORDER BY NombreSituacion asc -- OT12168

GRANT EXECUTE ON [dbo].[Pais_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Periodicidad_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Periodicidad_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Luisa Gonzales
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros en la tabla 'Periodicidad' que coincidan con los 
-- parámetros de entrada 
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE       PROCEDURE [dbo].[Periodicidad_SeleccionarPorFiltro] (
	@p_DiasPeriodo numeric(3, 0),	-- Código de periodicidad
	@p_Descripcion varchar(15),	-- Descripción del registro
	@p_Situacion varchar(1)		-- Situacion del Registro
)
AS

SET NOCOUNT ON
if @p_DiasPeriodo<>0
SELECT
	[CodigoPeriodicidad],
	[Descripcion],
	[DiasPeriodo],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	[UsuarioCreacion],
	[FechaCreacion],
	[HoraCreacion],
	[UsuarioModificacion],
	[FechaModificacion],
	[Host],
	[HoraModificacion]
FROM
	[Periodicidad]
WHERE
  ((Situacion like  '%'+ coalesce ( @p_Situacion,Situacion)+ '%')or(Situacion='')) and ((Descripcion like  coalesce ( @p_Descripcion,Descripcion)+ '%')or(Descripcion=''))
 and   ((DiasPeriodo=@p_DiasPeriodo))
order by descripcion

else
SELECT
	[CodigoPeriodicidad],
	[Descripcion],
	[DiasPeriodo],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	
	[UsuarioCreacion],
	[FechaCreacion],
	[HoraCreacion],
	[UsuarioModificacion],
	[FechaModificacion],
	[Host],
	[HoraModificacion]
FROM
	[Periodicidad]
WHERE
  ((Situacion like  '%'+ coalesce ( @p_Situacion,Situacion)+ '%')or(Situacion='')) and ((Descripcion like  coalesce ( @p_Descripcion,Descripcion)+ '%')or(Descripcion=''))
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[Periodicidad_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO 
PRINT '[dbo].[PeriodoLibor_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[PeriodoLibor_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Luisa Gonzales
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros en la tabla 'PeriodoLibor' que coincidan con los 
-- parámetro de entrada
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE      PROCEDURE [dbo].[PeriodoLibor_SeleccionarPorFiltro] (
	@p_CodigoPeriodoLibor varchar(4) = '',	-- Código de periodo libor
	@p_Descripcion varchar(15) = '',		-- Descripción del registro
	@p_Situacion varchar(1)	 = ''		-- Situacion del Registro
)
AS

SET NOCOUNT ON

SELECT
	[CodigoPeriodoLibor],
	[UsuarioCreacion],
	[Descripcion],
	[FechaCreacion],
	[UsuarioModificacion],
	[HoraCreacion],
	[FechaModificacion],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	
	[Host],
	[HoraModificacion]
FROM
	[PeriodoLibor]
WHERE
	
((CodigoPeriodoLibor   like coalesce ( @p_CodigoPeriodoLibor,CodigoPeriodoLibor)+ '%')or(CodigoPeriodoLibor='')) and ((Descripcion like  coalesce ( @p_Descripcion,Descripcion)+ '%')or(Descripcion=''))and
  ((Situacion like  '%'+ coalesce ( @p_Situacion,Situacion)+ '%')or(Situacion=''))
order by Situacion asc --OT12168

GRANT EXECUTE ON [dbo].[PeriodoLibor_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Portafolio_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Portafolio_SeleccionarPorFiltro]
GO
--###COMENTARIO###
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Portafolio_SeleccionarPorFiltro](  
 @p_Descripcion  varchar(40) = '',
 @p_Situacion VARCHAR(1) = ''
)
AS
BEGIN
	SET NOCOUNT ON  
	SELECT  PO.[CodigoPortafolioSBS],
			PO.[Descripcion],
			PO.[FechaConstitucion],
			PO.[CodigoTipoValorizacion],
			TV.Descripcion AS 'NombreTipoValorizacion',
			PO.[UsuarioCreacion],
			PO.[Situacion],
			PG.Nombre AS 'NombreSituacion',
			PO.[FechaCreacion],
			PO.[HoraCreacion],
			PO.[UsuarioModificacion],
			PO.[FechaModificacion],
			PO.[HoraModificacion],
			PO.[CodigoNegocio],
			NE.Descripcion AS 'NombreNegocio',
			PO.[Host],
			PO.IndicePortafolio,
			dbo.fn_SIT_gl_ConvertirFechaaString(PO.[FechaNegocio]) AS 'FechaNegocio'
	FROM  [Portafolio] AS PO  
	LEFT JOIN TipoValorizacion AS TV ON PO.CodigoTipovalorizacion = TV.CodigoTipoValorizacion  
	INNER JOIN Negocio AS NE ON PO.CodigoNegocio = NE.CodigoNegocio
	INNER JOIN ParametrosGenerales AS PG ON PO.Situacion = PG.Valor
	WHERE  
	PO.Descripcion LIKE (CASE WHEN LEN(@p_Descripcion) = 0 THEN PO.Descripcion ELSE @p_Descripcion + '%' END)  
	AND PO.Situacion = (CASE WHEN LEN(@p_Situacion) = 0 THEN PO.Situacion ELSE @p_Situacion END)  
	AND PG.Clasificacion = 'Situación'  AND CodigoPortafolioSBS <> '0'
	ORDER BY NombreSituacion ASC -- OT12168
END

GRANT EXECUTE ON [dbo].[Portafolio_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[pr_SIT_gl_GrupoLimiteTrader_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[pr_SIT_gl_GrupoLimiteTrader_SeleccionarPorFiltro]
GO

/*
<Inicio_Cabecera>
     <Nombre>pr_SIT_gl_GrupoLimiteTrader_SeleccionarPorFiltro</Nombre>     
     <Nro_OT>64291</Nro_OT>
     <Objetivo>Selecciona los registros de Grupo Limite Trading</Objetivo>
     <Autor>(Zoluxiones Consulting) Hector Delgado Gonzales</Autor>
     <Fecha>02/12/2011</Fecha>     	 
	 <Parametros_Entrada>@p_Descripcion</Parametros_Entrada>
	 <Parametros_Entrada>@p_Situacion</ParametrosEntrada>
</Inicio_Cabecera>
*/

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[pr_SIT_gl_GrupoLimiteTrader_SeleccionarPorFiltro]     
(
	@p_Descripcion varchar(30),
	@p_Situacion varchar(1)
)
AS

SET NOCOUNT ON

SELECT
	gl.CodigoGrupLimTrader,
	TipoRenta = tr.Descripcion,
	Descripcion = gl.Nombre,
	pg.Nombre AS 'NombreSituacion'
FROM
	GrupoLimiteTrader AS gl
	INNER JOIN ParametrosGenerales AS pg ON gl.Situacion = pg.Valor
	INNER JOIN TipoRenta tr ON gl.CodigoRenta = tr.CodigoRenta
WHERE
	((gl.Nombre like COALESCE(@p_Descripcion, gl.Nombre) + '%') or (gl.Nombre='')) AND
	gl.Situacion = (case when len(@p_Situacion) = 0 then gl.Situacion else @p_Situacion end) AND
	pg.Clasificacion = 'Situación'
ORDER BY
	NombreSituacion ASC -- OT12168

GRANT EXECUTE ON [dbo].[pr_SIT_gl_GrupoLimiteTrader_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[SectorEmpresarial_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[SectorEmpresarial_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Daphne Orihuela
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'SectorEmpresarial' que coincidan con los
-- parámetros de entrada
-- ================================================

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE        PROCEDURE [dbo].[SectorEmpresarial_SeleccionarPorFiltro] (
	@p_CodigoSectorEmpresarial varchar(4),  --Codigo de SectorEmpresarial
	@p_Descripcion varchar(40),  --Descripcion de SectorEmpresarial
	@p_Situacion varchar(1)  --Situacion del registro s.Inactivo=I  ,  Activo=A
)
AS

SET NOCOUNT ON

SELECT
	s.CodigoSectorEmpresarial,
	s.Descripcion,
	s.UsuarioCreacion,
	s.FechaCreacion,
	s.UsuarioModificacion,
	s.HoraCreacion,
	s.FechaModificacion,
	s.Situacion,
	s.Host,
	s.HoraModificacion,
	
	CASE s.Situacion WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'NombreSituacion'
FROM
	SectorEmpresarial as s
WHERE
((s.CodigoSectorEmpresarial like COALESCE(@p_CodigoSectorEmpresarial,s.CodigoSectorEmpresarial) + '%') or (s.CodigoSectorEmpresarial='')) and 
((s.Descripcion like  COALESCE(@p_Descripcion,s.Descripcion) + '%') or (s.Descripcion='')) and
((s.Situacion like  COALESCE(@p_Situacion,s.Situacion) + '%') or (s.Situacion='')) 
ORDER BY NombreSituacion asc -- OT12168

GRANT EXECUTE ON [dbo].[SectorEmpresarial_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[TipoAmortizacion_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[TipoAmortizacion_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Luisa Gonzales
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'TipoAmortizacion' que coinciden con
-- los parámetros de entrada (filtros)
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE       PROCEDURE [dbo].[TipoAmortizacion_SeleccionarPorFiltro] 
(
	@p_Situacion varchar(1) = '', 	--Situacion del registro
	@p_Descripcion varchar(20) = '' 	-- descripcion del Tipo de Amortizacion
)
AS

SET NOCOUNT ON

SELECT
	[CodigoTipoAmortizacion],
	[UsuarioCreacion],
	[Descripcion],
	[NumeroDias],
	[FechaCreacion],
	[UsuarioModificacion],
	[HoraCreacion],
	[FechaModificacion],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	[Host],
	[HoraModificacion]

FROM
	[TipoAmortizacion]
WHERE
	((Situacion like  '%'+ coalesce ( @p_Situacion,Situacion)+ '%')or(Situacion='')) and ((Descripcion like  coalesce ( @p_Descripcion,Descripcion)+ '%')or(Descripcion=''))
	AND Descripcion LIKE (CASE WHEN LEN(@p_Descripcion) = 0 THEN Descripcion ELSE @p_Descripcion + '%' END)
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[TipoAmortizacion_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[TipoBursatilidad_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[TipoBursatilidad_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Luisa Gonzales
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'TipoBursatilidad' que 
-- coincidan con los parámetros de entrada
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
--EXEC TipoBursatilidad_SeleccionarPorFiltro '',''
CREATE      PROCEDURE [dbo].[TipoBursatilidad_SeleccionarPorFiltro] 
(
	@p_Situacion varchar (1) = '',	-- Situacion del Registro
	@p_Descripcion varchar(50) = ''	-- Descripción del registro
	
)
AS

SET NOCOUNT ON

SELECT
	[CodigoBursatilidad],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	[UsuarioCreacion],
	[Descripcion],
	[FechaCreacion],
	[HoraCreacion],
	[UsuarioModificacion],
	[FechaModificacion],
	[Host],
	[HoraModificacion]
FROM
	[TipoBursatilidad]
WHERE

 	((Situacion like  '%'+ coalesce ( @p_Situacion,Situacion)+ '%')or(Situacion='')) and ((Descripcion like  coalesce ( @p_Descripcion,Descripcion)+ '%')or(Descripcion=''))
	AND Descripcion LIKE (CASE WHEN LEN(@p_Descripcion) = 0 THEN Descripcion ELSE @p_Descripcion + '%' END)
order by Situacion asc

GRANT EXECUTE ON [dbo].[TipoBursatilidad_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO 
PRINT '[dbo].[TipoCupon_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[TipoCupon_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Luisa Gonzales
-- Fecha Creación:	10/09/2007
-- Descripción:		Seleccionar los registros de la tabla 'TipoCupon' que coincidan con el
-- parámetro de entrada (filtro)
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE        PROCEDURE [dbo].[TipoCupon_SeleccionarPorFiltro] (
	@p_CodigoTipoCupon varchar(4) = '',		-- Código del tipo de cupón
	@p_Descripcion varchar(25) = '',		-- Descripción del registro
	@p_Situacion varchar(1) = '' 		-- Situacion del Registro
)
AS

SET NOCOUNT ON

SELECT
	[CodigoTipoCupon],
	[Descripcion],
	[UsuarioCreacion],
	[Observaciones],
	[FechaCreacion],
	[UsuarioModificacion],
	[FechaModificacion],
	[HoraCreacion],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	[Host],
	[HoraModificacion]
FROM
	[TipoCupon]
WHERE
	((CodigoTipoCupon   like  coalesce ( @p_CodigoTipoCupon,CodigoTipoCupon)+ '%')or(CodigoTipoCupon='')) and ((Descripcion like coalesce ( @p_Descripcion,Descripcion)+ '%')or(Descripcion=''))and
	((Situacion like  '%'+ coalesce ( @p_Situacion,Situacion)+ '%')or(Situacion=''))
	AND Descripcion LIKE (CASE WHEN LEN(@p_Descripcion) = 0 THEN Descripcion ELSE @p_Descripcion + '%' END)
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[TipoCupon_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO


USE [SIT-FONDOS]
GO 
PRINT '[dbo].[TipoDocumento_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[TipoDocumento_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Daphne Orihuela
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'TipoDocumento' que coincidan
-- con los parámetros de entrada
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

--EXEC TipoDocumento_SeleccionarPorFiltro '','',''
CREATE   PROCEDURE [dbo].[TipoDocumento_SeleccionarPorFiltro] (
	@p_CodigoTipoDocumento varchar(4),  --Codigo de TipoDocumento
	@p_Descripcion varchar(40),  --Descripcion de TipoDocumento
	@p_Situacion varchar(1)  --Situacion del registro [Inactivo=I  ,  Activo=A]
)
AS

SET NOCOUNT ON

SELECT
	t.CodigoTipoDocumento,
	t.Descripcion,
	t.Situacion,
	CASE t.Situacion WHEN 'A' THEN 'ACTIVO' WHEN 'I' THEN 'INACTIVO' END AS 'NombreSituacion',
	t.LongitudMaxima,
	t.DigitoChekeo,
	CASE t.DigitoChekeo WHEN 'S' THEN 'SI' WHEN 'N' THEN 'NO' END AS 'Chekeo',
	t.UsuarioCreacion,
	t.TipoPersona,
	CASE t.TipoPersona WHEN 'N' THEN 'NATURAL' WHEN 'J' THEN 'JURIDICA' END AS 'NombreTipoPersona',
	t.FechaCreacion,
	t.HoraCreacion,
	t.UsuarioModificacion,
	t.FechaModificacion,
	t.Host,
	t.HoraModificacion
FROM
	TipoDocumento as t
WHERE
((t.CodigoTipoDocumento like COALESCE(@p_CodigoTipoDocumento,t.CodigoTipoDocumento) + '%') or (t.CodigoTipoDocumento='')) and 
((t.Descripcion like COALESCE(@p_Descripcion,t.Descripcion) + '%') or (t.Descripcion='')) and
((t.Situacion like COALESCE(@p_Situacion,t.Situacion) + '%') or (t.Situacion=''))
ORDER BY NombreSituacion asc -- OT12168

GRANT EXECUTE ON [dbo].[TipoDocumento_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[TipoRenta_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[TipoRenta_SeleccionarPorFiltro]
GO
/*
<Inicio_Cabecera>
     <Nombre>TipoRenta_SeleccionarPorFiltro</Nombre>     
     <Nro_OT></Nro_OT>
     <Objetivo>Selecciona los registros de la tabla 'TipoRenta' que coinciden con
				los parámetros de entrada (filtro)</Objetivo>
     <Autor>(Zoluxiones Consulting) Daphne Orihuela</Autor>
     <Fecha>10/09/2007</Fecha>     	 
	 <Parametros_Entrada>@p_CodigoRenta: Codigo de TipoRenta</Parametros_Entrada>         
	 <Parametros_Entrada>@p_Descripcion: Descripcion de TipoRenta</Parametros_Entrada>         
	 <Parametros_Entrada>@p_Situacion: Situacion del registro t.Inactivo=I, Activo=A</Parametros_Entrada>         
</Inicio_Cabecera>
<Inicio_Detalle>
	<Nro_OT>62087</Nro_OT>
	<Objetivo>Nro14-R23 Se agrega campo TipoFactor</Objetivo>
	<Autor>(Zoluxiones Consulting) Hector Delgado G.</Autor>
	<Fecha>23/02/2011</Fecha>
</Inicio_Detalle>
*/
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[TipoRenta_SeleccionarPorFiltro] ( 
	@p_CodigoRenta varchar(3),
	@p_Descripcion varchar(50),
	@p_Situacion varchar(1)
)
AS

SET NOCOUNT ON

SELECT
	t.CodigoRenta,
	t.Descripcion,
	t.UsuarioCreacion,
	t.FechaCreacion,
	t.UsuarioModificacion,
	t.HoraCreacion,
	t.FechaModificacion,
	t.Situacion,
	CASE t.Situacion WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'NombreSituacion',
	t.Host,
	t.HoraModificacion,
	t.TipoFactor	--HDG OT 62087 Nro14-R23 20110223
FROM
	TipoRenta as t
WHERE
((t.CodigoRenta like COALESCE(@p_CodigoRenta,t.CodigoRenta) + '%') or (t.CodigoRenta='')) and 
((t.Descripcion like COALESCE(@p_Descripcion,t.Descripcion) + '%') or (t.Descripcion='')) and
((t.Situacion like COALESCE(@p_Situacion,t.Situacion) + '%') or (t.Situacion=''))
ORDER BY NombreSituacion asc -- OT12168

GRANT EXECUTE ON [dbo].[TipoRenta_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[TipoValorizacion_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[TipoValorizacion_SeleccionarPorFiltro]
GO

-- ================================================
-- Autor:		Luisa Gonzales
-- Fecha Creación:	10/09/2007
-- Descripción:		Selecciona los registros de la tabla 'TipoValorizacion' que coinciden con
-- el parámetro de entrada 
-- ================================================
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE        PROCEDURE [dbo].[TipoValorizacion_SeleccionarPorFiltro] 
(
	@p_Situacion varchar(1) = '',	-- código de tipo de valorización
	@p_Descripcion varchar(50) = ''  	-- descripcion del tipo de valorización 
)
AS

SET NOCOUNT ON

SELECT
	[CodigoTipoValorizacion],
	[Descripcion],
	CASE [Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',
	[UsuarioCreacion],
	[FechaCreacion],
	[HoraCreacion],
	[UsuarioModificacion],
	[FechaModificacion],
	[HoraModificacion],
	[Host]
FROM
	[tipoValorizacion]
WHERE
	Descripcion LIKE (CASE WHEN LEN(@p_Descripcion) = 0 THEN Descripcion ELSE @p_Descripcion + '%' END)
	AND Situacion = (CASE WHEN LEN(@p_Situacion) = 0 THEN Situacion ELSE @p_Situacion END)
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[TipoValorizacion_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Valores_SeleccionarPorFiltro]'
DROP PROCEDURE IF EXISTS [dbo].[Valores_SeleccionarPorFiltro]
GO

-- ================================================  
-- Autor:  Luisa Gonzales  
-- Fecha Creación: 10/09/2007  
-- Descripción:  Selecciona los registros de la tabla 'Valores' que coinciden con  
-- los parámetros de entrada (filtros)  
-- ================================================  
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
------------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [dbo].[Valores_SeleccionarPorFiltro] (  
 @p_TipoRenta varchar(5),    -- Codigo TipoRenta  
 @p_CodigoISIN varchar(12),        -- Codigo ISIN  
 @p_CodigoNemonico varchar(15),   -- Codigo Nemonico  
 @p_CodigoEmisor varchar(10),   -- Codigo Emisor  
 @p_CodigoSBS varchar(12),   -- Codigo SBS  
 @p_Moneda varchar(10)           -- Codigo Moneda  
)  
AS  
  
SET NOCOUNT ON  
  
SELECT  
CodigoISIN,  
V.CodigoNemonico,  
V.CodigoSBS,   
Case V.Agrupacion WHEN 'S' THEN 'SIMPLE' WHEN 'E' THEN 'ESTRUCTURADO' WHEN 'C' THEN 'COMPUESTO' END AS 'Agrupacion' ,  
TE.Descripcion Emisor,  
R.Descripcion TipoRenta,  
Me.Descripcion Mercado,  
M.Simbolo Moneda,  
T.Descripcion TipoTitulo,  
CASE V.[Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion',  
V.CodigoTipoInstrumentoSBS,  
T.CodigoTipoRenta,  
V.CodigoMoneda,  
V.CodigoTipoCupon  
FROM  
Valores V LEFT OUTER JOIN  
Entidad E ON V.CodigoEmisor = E.CodigoEntidad LEFT OUTER JOIN  
Moneda M ON V.CodigoMoneda = M.CodigoMoneda LEFT OUTER JOIN  
TipoTitulo T ON V.CodigoTipoTitulo = T.CodigoTipoTitulo LEFT OUTER JOIN  
TipoRenta R ON T.CodigoTipoRenta = R.CodigoRenta LEFT OUTER JOIN  
TipoEntidad TE ON E.CodigoTipoEntidad = TE.CodigoTipoEntidad LEFT OUTER JOIN  
Mercado Me ON E.CodigoMercado = Me.CodigoMercado  
WHERE  
((V.CodigoISIN like (CASE WHEN LEN(@p_CodigoISIN) = 0 THEN V.CodigoISIN ELSE @p_CodigoISIN+'%' END))   
--or V.CodigoISIN is null  
) and  
((V.CodigoNemonico like (CASE WHEN LEN(@p_CodigoNemonico) = 0 THEN V.CodigoNemonico ELSE @p_CodigoNemonico + '%' END))  
--or V.codigoNemonico is null  
) and  
((V.CodigoSBS like (CASE WHEN LEN(@p_CodigoSBS) = 0 THEN V.CodigoSBS ELSE @p_CodigoSBS +'%' END))  
--or V.codigoSBS is null  
) and  
((V.CodigoEmisor like (CASE WHEN LEN(@p_CodigoEmisor) = 0 THEN V.CodigoEmisor ELSE @p_CodigoEmisor +'%' END))  
--or V.codigoEmisor is null  
) and  
((V.CodigoMoneda like (CASE WHEN LEN(@p_Moneda) = 0 THEN V.CodigoMoneda ELSE @p_Moneda+'%' END))  
--or V.codigoMoneda is null  
) and  
((V.TipoRenta like (CASE WHEN LEN(@p_TipoRenta) = 0 THEN V.TipoRenta ELSE @p_TipoRenta+'%' END))  
--or V.tipoRenta is null  
)  
  
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[Valores_SeleccionarPorFiltro] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Valores_SeleccionarPorFiltro_Prueba]'
DROP PROCEDURE IF EXISTS [dbo].[Valores_SeleccionarPorFiltro_Prueba]
GO

--SELECT * FROM VALORES WHERE CODIGOEMISOR = 'CITI'  
  
--EXEC Valores_SeleccionarPorFiltro_Prueba '','','','CITI','',''  
  
-- ================================================  
-- Autor:    
-- Fecha Creación: 10/09/2007  
-- Descripción:  Selecciona los registros de la tabla 'Valores' que coinciden con  
-- los parámetros de entrada (filtros)  
-- ================================================  

-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
  
CREATE PROCEDURE [dbo].[Valores_SeleccionarPorFiltro_Prueba] (  
 @p_TipoRenta varchar(5),    -- Codigo TipoRenta  
 @p_CodigoISIN varchar(12),        -- Codigo ISIN  
 @p_CodigoNemonico varchar(15),   -- Codigo Nemonico  
 @p_CodigoEmisor varchar(10),   -- Codigo Emisor  
 @p_CodigoSBS varchar(12),   -- Codigo SBS  
 @p_Moneda varchar(10)           -- Codigo Moneda  
)  
AS  
  
SET NOCOUNT ON  
  
SELECT  
CodigoISIN,  
V.CodigoNemonico,  
V.CodigoSBS,   
V.CodigoEmisor,  
Case V.Agrupacion WHEN 'S' THEN 'SIMPLE' WHEN 'E' THEN 'ESTRUCTURADO' WHEN 'C' THEN 'COMPUESTO' END AS 'Agrupacion' ,  
TE.Descripcion Emisor,  
R.Descripcion TipoRenta,  
Me.Descripcion Mercado,  
M.Simbolo Moneda,  
T.Descripcion TipoTitulo,  
CASE V.[Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion'  
FROM  
Valores V LEFT OUTER JOIN  
Entidad E ON V.CodigoEmisor = E.CodigoEntidad LEFT OUTER JOIN  
Moneda M ON V.CodigoMoneda = M.CodigoMoneda LEFT OUTER JOIN  
TipoTitulo T ON V.CodigoTipoTitulo = T.CodigoTipoTitulo LEFT OUTER JOIN  
TipoRenta R ON V.TipoRenta = R.CodigoRenta LEFT OUTER JOIN  
TipoEntidad TE ON E.CodigoTipoEntidad = TE.CodigoTipoEntidad LEFT OUTER JOIN  
Mercado Me ON E.CodigoMercado = Me.CodigoMercado  
WHERE  
((V.CodigoISIN like (CASE WHEN LEN(@p_CodigoISIN) = 0 THEN V.CodigoISIN ELSE @p_CodigoISIN+'%' END))   
--or V.CodigoISIN is null  
) and  
((V.CodigoNemonico like (CASE WHEN LEN(@p_CodigoNemonico) = 0 THEN V.CodigoNemonico ELSE @p_CodigoNemonico + '%' END))  
--or V.codigoNemonico is null  
) and  
((V.CodigoSBS like (CASE WHEN LEN(@p_CodigoSBS) = 0 THEN V.CodigoSBS ELSE @p_CodigoSBS +'%' END))  
--or V.codigoSBS is null  
) and  
((V.CodigoEmisor like (CASE WHEN LEN(@p_CodigoEmisor) = 0 THEN V.CodigoEmisor ELSE @p_CodigoEmisor +'%' END))  
--or V.codigoEmisor is null  
) and  
((V.CodigoMoneda like (CASE WHEN LEN(@p_Moneda) = 0 THEN V.CodigoMoneda ELSE @p_Moneda+'%' END))  
--or V.codigoMoneda is null  
) and  
((V.TipoRenta like (CASE WHEN LEN(@p_TipoRenta) = 0 THEN V.TipoRenta ELSE @p_TipoRenta+'%' END))  
--or V.tipoRenta is null  
)  
  
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[Valores_SeleccionarPorFiltro_Prueba] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO 
PRINT '[dbo].[Valores_SeleccionarPorFiltro2]'
DROP PROCEDURE IF EXISTS [dbo].[Valores_SeleccionarPorFiltro2]
GO

/*
<Inicio_Cabecera>            
 <Nombre>[Valores_SeleccionarPorFiltro2]</Nombre>                 
 <Nro_OT></Nro_OT>            
      <Objetivo></Objetivo>            
      <Autor>(Zoluxiones Consulting) </Autor>            
    <Fecha></Fecha>                   
    <Parametros_Entrada>@p_TipoRenta</Parametros_Entrada> 
	<Parametros_Entrada>@p_CodigoISIN</Parametros_Entrada>              
    <Parametros_Entrada>@p_CodigoNemonico</Parametros_Entrada>            
    <Parametros_Entrada>@p_CodigoEmisor</Parametros_Entrada>            
    <Parametros_Entrada>@p_CodigoSBS</Parametros_Entrada>             
    <Parametros_Entrada>@p_Moneda</Parametros_Entrada>             
</Inicio_Cabecera>            
<Inicio_Detalle>
	<Nro_OT>66056</Nro_OT>
	<Objetivo>Modificar la consulta para adecuar a los instrumentos de futuros</Objetivo>
	<Autor>(Zoluxiones Consulting) Cristian Molina B.</Autor>
	<Fecha>28/09/2012</Fecha>
</Inicio_Detalle>
*/
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 06/08/2019
--	Modificado por: Miguel Corzo
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Valores_SeleccionarPorFiltro2] (    
 @p_TipoRenta varchar(5),    -- Codigo TipoRenta    
 @p_CodigoISIN varchar(12),        -- Codigo ISIN    
 @p_CodigoNemonico varchar(15),   -- Codigo Nemonico    
 @p_CodigoEmisor varchar(10),   -- Codigo Emisor    
 @p_CodigoSBS varchar(12),   -- Codigo SBS    
 @p_Moneda varchar(10)           -- Codigo Moneda    
)    
AS    
    
SET NOCOUNT ON    
    
SELECT    
CodigoISIN,    
V.CodigoNemonico,    
V.CodigoSBS,     
V.CodigoEmisor,    
Case V.Agrupacion WHEN 'S' THEN 'SIMPLE' WHEN 'E' THEN 'ESTRUCTURADO' WHEN 'C' THEN 'COMPUESTO' END AS 'Agrupacion' ,    
TE.Descripcion Emisor,    
R.Descripcion TipoRenta,    
--ini CMB OT 66056 20120928
case when V.CodigoTipoInstrumentoSBS <> '86' then
isnull(Me.Descripcion, case when TER.CodigoPais= '604' then 'LOCAL' else 'EXTRANJERO' end)
else ''
end Mercado,         
--fin CMB OT 66056 20120928
M.Simbolo Moneda,    
T.Descripcion TipoTitulo,    
CASE V.[Situacion] WHEN 'I' THEN 'INACTIVO' WHEN 'A' THEN 'ACTIVO' END AS 'Situacion'    
FROM    
Valores V 
LEFT OUTER JOIN Entidad E ON V.CodigoEmisor = E.CodigoEntidad 
LEFT OUTER JOIN Moneda M ON V.CodigoMoneda = M.CodigoMoneda 
LEFT OUTER JOIN TipoTitulo T ON V.CodigoTipoTitulo = T.CodigoTipoTitulo 
LEFT OUTER JOIN TipoRenta R ON V.TipoRenta = R.CodigoRenta 
LEFT OUTER JOIN TipoEntidad TE ON E.CodigoTipoEntidad = TE.CodigoTipoEntidad 
LEFT OUTER JOIN Mercado Me ON E.CodigoMercado = Me.CodigoMercado    
LEFT OUTER JOIN Terceros TER ON TER.CodigoTercero = E.CodigoTercero  --CMB OT 66056 20120928
WHERE    
(isnull(V.CodigoISIN,'') like (CASE WHEN LEN(@p_CodigoISIN) = 0 THEN isnull(V.CodigoISIN,'') ELSE @p_CodigoISIN+'%'  END)     
) and    
(isnull(V.CodigoNemonico,'') like (CASE WHEN LEN(@p_CodigoNemonico) = 0 THEN isnull(V.CodigoNemonico,'') ELSE @p_CodigoNemonico + '%' END)    
) and    
(isnull(V.CodigoSBS,'') like (CASE WHEN LEN(@p_CodigoSBS) = 0 THEN isnull(V.CodigoSBS,'') ELSE @p_CodigoSBS +'%' END)    
) and    
(isnull(V.CodigoEmisor,'') like (CASE WHEN LEN(@p_CodigoEmisor) = 0 THEN isnull(V.CodigoEmisor,'') ELSE @p_CodigoEmisor +'%' END)    
) and    
(isnull(V.CodigoMoneda,'') like (CASE WHEN LEN(@p_Moneda) = 0 THEN isnull(V.CodigoMoneda,'') ELSE @p_Moneda+'%' END)   
) and    
(isnull(V.TipoRenta,'') like (CASE WHEN LEN(@p_TipoRenta) = 0 THEN isnull(V.TipoRenta,'') ELSE @p_TipoRenta+'%' END)    
)    
    
order by Situacion asc -- OT12168

GRANT EXECUTE ON [dbo].[Valores_SeleccionarPorFiltro2] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO
PRINT '[dbo].[AsientosContable_Generar_Provision]'
DROP PROCEDURE IF EXISTS [dbo].[AsientosContable_Generar_Provision]
GO


--###COMENTARIO###Genera los Asientos Contables de "PROVISIÓN DE POLIZAS AGENTES DE BOLSA" para la ADMINISTRADORA
-------------------------------------------------------------------------------------------------------------------------
--Objetivo: Se modifica SP para Migración SQL 2016
-------------------------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 05/08/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 12168
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AsientosContable_Generar_Provision] 
	@p_CodigoPortafolio varchar(10),
	@p_fechaOperacion numeric(8),
	@p_usuario varchar(15),
	@p_fecha numeric(8),
	@p_Hora varchar(10),
	@p_Host varchar(20),
	@p_numAsientos numeric(8) output
AS

/*************************GLOBALES******************************************************************************/
DECLARE @Importe numeric(22,7),
		@CodigoOrden varchar(6), --RGF 20080904
		@CodigoMoneda varchar(10),
		@TipoCambio numeric(22,7),
		@ConversionMonto numeric(17,2), --RGF 20081201 era (22,7) pero la contabilidad se trabaja a 2 decimales
		--@Aux varchar(10),
		@CodigoMatrizAnt NUMERIC(5),
		@TipoContabilidad varchar(1),
		@CodigoCentrocostoDestino varchar(6),
		@CodigoCentroCosto varchar(6), @IndicaNroDocumento varchar(3), --RGF 20080717
		@anio varchar(2),
		@mes varchar(2),
		@lote_fecha_mes_ant NUMERIC(8,0),
		@NumeroAsiento varchar(20),
		@Secuencia NUMERIC(10),
		@NumeroMax Int,
		@Aplicar varchar(20),
		--@Glosa varchar(20),
		@CodigoMatriz NUMERIC(5),
		@NumeroCuentaContable VARCHAR(20),
		@DebeHaber VARCHAR(20),
		@AsientoAnteriorP varchar(20),
		@NumeroPoliza varchar(20),
		@TotalComisiones numeric(22, 7),
		@CodigoTipoRenta varchar(5),
		@intermediario varchar(50),
		@igv numeric(22,2),
		@CodigoMercado varchar(3),
		@ComisionTotalSinIGV numeric(22,7), --RGF 20081203
		@Diferencia numeric(17,2), --RGF 20081203
		@CodigoPortafolioSBS varchar(10), --RGF 20081204
		@CodigoMonedaCuenta varchar(1), --RGF 20081229
		@flagMoneda  int --LETV 20100319

/*******************************************************************************************************************/

--set @igv = 1.19
/*********************************************************************************************************************/
DELETE FROM AsientoContable
WHERE 
FechaAsiento = @p_fechaOperacion AND 
CodigoPortafolioSBS = @p_CodigoPortafolio AND
TipoAsiento = 'A' AND left(NumeroAsiento,1) = 'P'


set @TipoContabilidad = ''
set @CodigoCentrocostoDestino = ''
select @CodigoCentroCosto = '', @IndicaNroDocumento = '' --RGF 20080717
/*******************************************************************************************************************
---------------------------------------INVERSIONES-COMPRA/VENTA-----------------------------------------------------------------------------
*********************************************************************************************************************/

--print 'Inicia Inversiones'

--	iniciando numero de asientos I
	set @p_numAsientos = 0
	set @AsientoAnteriorP = 'aux'

	select @NumeroMax = 1 --RGF 20080724

	DECLARE CuentasPorCobrar_cursor CURSOR FOR
	SELECT
		ccp.CodigoOrden,
		ccp.CodigoMoneda,
		v.TipoRenta,
		oi.NumeroPoliza,
		isnull(e.sinonimo,t.descripcion) as intermediario	,	
		ccp.CodigoMercado,
		oi.CodigoPortafolioSBS --RGF 20081204
	FROM cuentasPorCobrarPagar ccp
		INNER JOIN OrdenInversion oi on oi.CodigoOrden = ccp.CodigoOrden
		INNER JOIN Valores v on v.codigoNemonico = oi.codigoMnemonico
		INNER JOIN Terceros t on t.codigotercero=oi.codigotercero
		INNER JOIN Entidad e on e.CodigoTercero = t.Codigotercero
			
	WHERE ccp.FechaOperacion = @p_fechaOperacion
		AND ccp.CodigoPortafolioSBS = @p_CodigoPortafolio
		AND ccp.CodigoOperacion = '67'
		AND ccp.situacion = 'A'


	OPEN CuentasPorCobrar_cursor
	-- Perform the first fetch.
	FETCH NEXT FROM CuentasPorCobrar_cursor INTO @CodigoOrden, @CodigoMoneda, @CodigoTipoRenta, @NumeroPoliza, @intermediario, @CodigoMercado, @CodigoPortafolioSBS


	WHILE @@FETCH_STATUS = 0
	BEGIN
		--print 'RGF itera CuentasPorCobrar_cursor'
/*-------------------------------------------------------------------------------------------------------------------*/		
		SET @CodigoMatrizAnt = 0

		PRINT 'RGF @NumeroPoliza = ' + @NumeroPoliza

		if @CodigoMercado=1
		begin
			select @igv = round(sum(isnull(ValorComision,0)),2)
			from ImpuestosComisionesOrdenPreOrden
			where CodigoOrdenPreOrden in (select CodigoOrden from OrdenInversion where NumeroPoliza = @NumeroPoliza and FechaOperacion = @p_fechaOperacion  and estado<> 'E-ELI')
				and Situacion = 'A'
				and CodigoComision = 'P I.G.V. TOT'
				and CodigoPortafolioSBS = @CodigoPortafolioSBS --RGF 20081204

			set @igv = isnull(@igv,0)
			print '@igv = ' + cast(@igv as varchar)
			
			select @ComisionTotalSinIGV = sum(isnull(ValorComision,0))
			from ImpuestosComisionesOrdenPreOrden
			where CodigoOrdenPreOrden in (select CodigoOrden from OrdenInversion where NumeroPoliza = @NumeroPoliza and FechaOperacion = @p_fechaOperacion and estado<> 'E-ELI')
				and Situacion = 'A'
				and CodigoComision in ('PCOMIS SAB','PCUOTA B.V.L','PCONT CONASE','PCUOT CAVLI','PFND GARNTIA','PFND GARNTIA CAVA','P SEC FEES','COM EXT')
				and CodigoPortafolioSBS = @CodigoPortafolioSBS --RGF 20081204

			print '@Importe total sin IGV = ' + cast(@ComisionTotalSinIGV as varchar)
		end
		else
			select @igv = 0, @ComisionTotalSinIGV = 1

		--RGF 20080718 se saco fuera del fetch MatrizInv_cursor
		------------------------------------------------------------
		--Generacion de Numero de Asiento
		------------------------------------------------------------
		set @anio = substring(convert(varchar(8),@p_fechaOperacion), 3,2)
		set @mes = substring(convert(varchar(8),@p_fechaOperacion), 5,2)

		SELECT @lote_fecha_mes_ant = MAX(FechaAsiento) FROM AsientoContable

		set @NumeroAsiento = 'P' + @anio + @mes +  STUFF('0000', 5-len(CONVERT(varchar(5),@NumeroMax)), 
		len(CONVERT(varchar(5),@NumeroMax)), CONVERT(varchar(5),@NumeroMax))
		------------------------------------------------------------
		print 'RGF P @CodigoOrden = ' + @CodigoOrden
		print 'RGF P @CodigoMoneda = ' + @CodigoMoneda
		print 'RGF P @p_CodigoPortafolio = ' + @p_CodigoPortafolio
		
		DECLARE MatrizInv_cursor CURSOR FOR
		SELECT  Aplicar,
			isnull(CMC.CodigoMatrizContable,0),
			DMC.NumeroCuentaContable,
			DMC.DebeHaber,
			DMC.TipoContabilidad,
			DMC.CodigoCentrocostoDestino
			,DMC.CodigoCentroCosto, isnull(DMC.IndicaNroDocumento, '') --RGF 20080717
		FROM MatrizContable MC
		INNER JOIN CabeceraMatrizContable CMC ON MC.CodigoMatrizContable = CMC.CodigoMatrizContable
		INNER JOIN DetalleMatrizContable DMC ON DMC.CodigoCabeceraMatriz = CMC.CodigoCabeceraMatriz
		WHERE
			DMC.Situacion = 'A' AND
			MC.Situacion = 'A' AND
			CMC.Situacion = 'A' AND
			CMC.CodigoMoneda = @CodigoMoneda AND
			CMC.CodigoOperacion = '67' AND
			CMC.CodigoPortafolioSBS = @p_CodigoPortafolio AND
			MC.TablaMatriz = 'P' 
		ORDER BY DMC.Secuencia
		
		OPEN MatrizInv_cursor
		FETCH NEXT FROM MatrizInv_cursor 
		INTO @Aplicar, @CodigoMatriz , @NumeroCuentaContable,@DebeHaber, @TipoContabilidad, @CodigoCentrocostoDestino
			, @CodigoCentroCosto, @IndicaNroDocumento --RGF 20080717
		--SET @contador = 1
		WHILE @@FETCH_STATUS = 0
		BEGIN
			set @flagMoneda = 0
			print 'RGF itera MatrizInv_cursor @Aplicar = ' + @Aplicar
			IF NOT @Aplicar is NULL 
			BEGIN
				--print 'RGF P @NumeroPoliza = ' + cast(@NumeroPoliza as varchar)
				--print 'RGF P @NumeroOperacionAnt = ' + cast(@NumeroOperacionAnt as varchar)
				print 'RGF P @CodigoMatriz = ' + cast(@CodigoMatriz as varchar)
				print 'RGF P @CodigoMatrizAnt = ' + cast(@CodigoMatrizAnt as varchar)
				--RGF 20080724
				--IF ((@NumeroPoliza <> @NumeroOperacionAnt) or (@CodigoMatrizAnt <> @CodigoMatriz))
				IF @CodigoMatrizAnt <> @CodigoMatriz
				begin
					SELECT @NumeroMax = @NumeroMax + 1
					set @CodigoMatrizAnt = @CodigoMatriz
					--set @NumeroOperacionAnt = @NumeroPoliza
				end
				print 'RGF 2 P @NumeroMax = ' + cast(@NumeroMax as varchar)
				print 'RGF 2 P @NumeroAsiento = ' + @NumeroAsiento
				--set @NumeroOperacionAnt = @NumeroPoliza

				SELECT @Secuencia=ISNULL(MAX(Secuencia),0)+1
				FROM AsientoContable
				WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND
		      		FechaAsiento = @p_fechaOperacion AND
					NumeroAsiento = @NumeroAsiento

				if @DebeHaber = 'D'
				begin
					if @NumeroCuentaContable in ('4163221','4163222')
						select @Importe = round(sum(isnull(ValorComision,0))*(1+@igv/@ComisionTotalSinIGV),2)
						--select @Importe = sum(round(isnull(ValorComision,0)*(1+@igv/@ComisionTotalSinIGV),2))
						from ImpuestosComisionesOrdenPreOrden
						where CodigoOrdenPreOrden in (select CodigoOrden from OrdenInversion where NumeroPoliza = @NumeroPoliza and FechaOperacion = @p_fechaOperacion and estado<> 'E-ELI')
							and Situacion = 'A'
							and CodigoComision in ('PCOMIS SAB','P SEC FEES','COM EXT')
							and CodigoTipoRenta = right(@NumeroCuentaContable,1)
							and CodigoPortafolioSBS = @CodigoPortafolioSBS --RGF 20081204
					else
						select @Importe = round(sum(isnull(ValorComision,0))*(1+@igv/@ComisionTotalSinIGV),2)
						--select @Importe = sum(round(isnull(ValorComision,0)*(1+@igv/@ComisionTotalSinIGV),2))
						from ImpuestosComisionesOrdenPreOrden
						where CodigoOrdenPreOrden in (select CodigoOrden from OrdenInversion where NumeroPoliza = @NumeroPoliza and FechaOperacion = @p_fechaOperacion and estado<> 'E-ELI')
							and Situacion = 'A'
							and CodigoComision like
								(case @NumeroCuentaContable
									when '4163223' then 'PCUOTA B.V.L'
									when '4163224' then 'PCONT CONASE'
									when '4163225' then 'PCUOT CAVLI'
									when '4163226' then 'PFND GARNTIA%'
								end)
							and CodigoTipoRenta = @CodigoTipoRenta
							and CodigoPortafolioSBS = @CodigoPortafolioSBS --RGF 20081204
				end
				else --Haber
					select @Importe = sum(isnull(ValorComision,0))
					from ImpuestosComisionesOrdenPreOrden
					where CodigoOrdenPreOrden in (select CodigoOrden from OrdenInversion where NumeroPoliza = @NumeroPoliza and FechaOperacion = @p_fechaOperacion and estado<> 'E-ELI')
						and Situacion = 'A'
						and CodigoComision in ('PCOMIS SAB','PCUOTA B.V.L','PCONT CONASE','PCUOT CAVLI','PFND GARNTIA','PFND GARNTIA CAVA','P SEC FEES','COM EXT','P I.G.V. TOT')
						and CodigoPortafolioSBS = @CodigoPortafolioSBS --RGF 20081204

				set @Importe = isnull(@Importe,0)
				print 'RGF @Importe = ' + cast(@Importe as varchar)

				set @TipoCambio = 1
				set @ConversionMonto = @Importe
				print 'RGF P previo @ConversionMonto = ' + cast(@ConversionMonto as varchar)
				--set @Aux=@CodigoMoneda

				--RGF 20081229 La moneda se debe sacar segun la cuenta
				select @CodigoMonedaCuenta = CodigoMoneda from PlanCuenta
				where CodigoContablePU = @NumeroCuentaContable and Ano = year(getdate())

				/*if @CodigoMonedaCuenta = '1' --RGF 20081229
					set @CodigoMoneda = 'DOL'
				else --0
					set @CodigoMoneda = 'NSOL'--*/

				--El lote contable todos los monto deben de ser soles
				--Si la operacion fue hecha en dolares se tranforma a soles y se mantiene el monto original en dolares
				--Si la operacion fue hecha en CLP,COP,..etc, 1. se transforma a soles 2.el monto original debe de ser dolares.
				IF @CodigoMonedaCuenta = '1' and @CodigoMoneda <> 'DOL' --HDG 20100225 inc 58733  si el indicador de moneda dolares es igual a 1 se hace la conversion al tipo de cambio
					SELECT @TipoCambio=isnull(valorprimario,0) FROM VECTORTIPOCAMBIO
					WHERE CODIGOMONEDA = 'DOL'
						AND ENTIDADEXT='Real' and fecha=@p_fechaOperacion				
						
				ELSE
				BEGIN					
					--IF @CodigoMoneda <> 'NSOL' or @CodigoMonedaCuenta = '1'	--HDG 20100225 inc 58733 no es necesario
					IF @CodigoMoneda <> 'NSOL'
					begin
						--convertir el monto a soles
						print 'RGF P moneda no es SOL'
						SELECT @TipoCambio=isnull(valorprimario,0) FROM VECTORTIPOCAMBIO
						--WHERE CODIGOMONEDA = 'DOL'
						WHERE CODIGOMONEDA = @CodigoMoneda --LETV 20091123
							AND ENTIDADEXT='Real' and fecha=@p_fechaOperacion										
						--if @CodigoMoneda <> 'NSOL' --RGF 20081229	--HDG 20100225 inc 58733 no es necesario
							set @ConversionMonto=@Importe*@TipoCambio
						--set @CodigoMoneda = 'NSOL'							
						--LETV 20100319						
						if @CodigoMoneda<>'DOL'
						begin
								select @TipoCambio=isnull(valorprimario,0) FROM VECTORTIPOCAMBIO
								where  CODIGOMONEDA = 'DOL'
								and ENTIDADEXT='Real' and fecha=@p_fechaOperacion
								
								set @flagMoneda = 1					
						end
					end
				END

				--SELECT @ConversionMonto
				print 'RGF MatrizInv_cursor'
				print 'RGF @CodigoCentroCosto = ' + @CodigoCentroCosto
				print 'RGF @CodigoCentrocostoDestino = ' + @CodigoCentrocostoDestino
				print 'RGF @IndicaNroDocumento = ' + @IndicaNroDocumento
				print 'RGF @NumeroAsiento = ' + @NumeroAsiento
				print 'RGF @Secuencia = ' + cast(@Secuencia as varchar)
				print 'RGF @CodigoMatriz = ' + cast(@CodigoMatriz as varchar)
				print 'RGF @CodigoOrden = ' + @CodigoOrden
				print 'RGF @CodigoMoneda = ' + @CodigoMoneda
				print 'RGF @TipoContabilidad = ' + @TipoContabilidad
				print 'RGF @CodigoCentroCosto = ' + @CodigoCentroCosto
				print 'RGF @NumeroCuentaContable = ' + @NumeroCuentaContable
				--print 'RGF @Glosa = ' + @Glosa
				print 'RGF @DebeHaber = ' + @DebeHaber
				print 'RGF @ConversionMonto = ' + cast(@ConversionMonto as varchar)
				print 'RGF @NumeroPoliza = ' + @NumeroPoliza--*/

				if @ConversionMonto <> 0 --RGF 20081209 No debe salir los ceros
					INSERT INTO AsientoContable([CodigoPortafolioSBS], [NumeroAsiento], [Secuencia], 
								[FechaAsiento], [CodigoMatrizContable], 
								[CodigoOrden], 
								[CodigoMoneda], [CodigoOperacion], [TipoContable], 
								[CodigoCentrosCosto], [CuentaContable], 
								[Glosa], 
								[DebeHaber], [Importe], [Situacion], 
								[UsuarioCreacion], [FechaCreacion], [HoraCreacion], 
								[UsuarioMoificacion], [FechaModificacion], 
					 			[HoraModificacion], [Host],[CodigoTipoInstrumentoSBS], [CodigoOrigen], [TipoAsiento],
					 			[CodigoCentrocostoDestino], [IndicaNroDocumento],
					 			ImporteOrigen --RGF 20081202
					 			)
					VALUES(@p_CodigoPortafolio, @NumeroAsiento,@Secuencia,
								@p_fechaOperacion, @CodigoMatriz,
								@CodigoOrden,
								case when @flagMoneda = 1 then 'DOL' else @CodigoMoneda end, '67', @TipoContabilidad,
								@CodigoCentroCosto, @NumeroCuentaContable,
								isnull(@intermediario,'') + ' ' + @NumeroPoliza, --RGF 20080818
								@DebeHaber, @ConversionMonto, 'A',
		      					@p_Usuario, @p_fecha,@p_hora, 
		      					NULL, NULL,NULL,@p_host, NULL, @NumeroPoliza, 'A'
								, @CodigoCentrocostoDestino, @IndicaNroDocumento, --RGF 20080717
								round(@ConversionMonto/@TipoCambio,2) --RGF 20081202
								)

				--numero asientos de I
				if (@AsientoAnteriorP <> @NumeroAsiento)
				begin
					set @p_numAsientos = @p_numAsientos +1
					set @AsientoAnteriorP = @NumeroAsiento
				end

				--RGF 20080701
				UPDATE ccp
				SET ccp.NumeroAsiento = @NumeroAsiento,
		    	    ccp.FechaAsiento = @p_fechaOperacion
				FROM CuentasPorCobrarPagar ccp
				WHERE ccp.CodigoPortafolioSBS = @p_CodigoPortafolio
		      	    AND ccp.CodigoOrden = @CodigoOrden

				if not exists (select 1 from AsientoContable WHERE NumeroAsiento=@NumeroAsiento AND CodigoPortafolioSBS=@p_CodigoPortafolio AND Secuencia=0 and FechaAsiento = @p_fechaOperacion)
				--Leo - cambios aqui, agregar fecha en asiento de secuencia 0
					INSERT INTO AsientoContable([CodigoPortafolioSBS], [NumeroAsiento], [Secuencia], [FechaAsiento],[CodigoMatrizContable], 
								[CodigoOrden],
								[CodigoMoneda],[Situacion],[Glosa],[DebeHaber],[CuentaContable],[CodigoTipoInstrumentoSBS],[CodigoOrigen],[TipoContable], [CodigoCentrosCosto] ,[Importe], [TipoAsiento]
								, [CodigoCentrocostoDestino], [IndicaNroDocumento] --RGF 20080717
								)
							VALUES     (@p_CodigoPortafolio, @NumeroAsiento,0,@p_fechaOperacion,  @CodigoMatriz, 
								@CodigoOrden,
								@CodigoMoneda, 'A', 'Cabecera Asiento - '+@NumeroAsiento+' ', 'Total', '0', null, @NumeroPoliza , @TipoContabilidad, @CodigoCentroCosto, 0 ,'A'
								, @CodigoCentrocostoDestino, @IndicaNroDocumento --RGF 20080717
								)

				--set @CodigoMoneda=@Aux

			END
			FETCH NEXT FROM MatrizInv_cursor
			INTO @Aplicar, @CodigoMatriz , @NumeroCuentaContable,@DebeHaber, @TipoContabilidad, @CodigoCentrocostoDestino
				, @CodigoCentroCosto, @IndicaNroDocumento --RGF 20080717
			--SET @contador = @contador + 1
		END
		--SET @contador = 1
		CLOSE MatrizInv_cursor
		DEALLOCATE MatrizInv_cursor

		-------------- Ajuste de descuadres del asiento --------------
		select @Diferencia = dbo.RetornarImporteTotalAsiento(@p_CodigoPortafolio,@NumeroAsiento,0,'A',@p_fechaOperacion)
		if @Diferencia <> 0
		begin
			--SET ROWCOUNT 1 --RGF Actualizamos solo un registro con Importe mayor que cero

			update TOP (1) AsientoContable
			set Importe = Importe + @Diferencia,
				ImporteOrigen = round((Importe + @Diferencia)*ImporteOrigen/Importe,2)
			where NumeroAsiento = @NumeroAsiento
				and fechaAsiento = @p_fechaOperacion
				and CodigoPortafolioSBS = @p_CodigoPortafolio
				and Importe > 0
				and Secuencia < 7
				
		--	SET ROWCOUNT 0
		end
		-------------- FIN Ajuste de descuadres del asiento --------------
		
		/*-------------------------------------------------------------------------------------------------------------------*/		
		FETCH NEXT FROM CuentasPorCobrar_cursor INTO @CodigoOrden, @CodigoMoneda, @CodigoTipoRenta, @NumeroPoliza, @intermediario, @CodigoMercado, @CodigoPortafolioSBS
	END
	
	CLOSE CuentasPorCobrar_cursor
	DEALLOCATE CuentasPorCobrar_cursor

GO

GRANT EXECUTE ON [dbo].[AsientosContable_Generar_Provision] TO [rol_sit_fondos] AS [dbo]
GO

USE [SIT-FONDOS]
GO
PRINT '[dbo].[Contabilidad_Reportes_Resumen_Envio_PU_FONDO]'
DROP PROCEDURE IF EXISTS [dbo].[Contabilidad_Reportes_Resumen_Envio_PU_FONDO]
GO

--===============================================================
--Creado Por    : Zoluxiones Consulting S.A.C (JVC)
--Fecha Creación: 28/05/2009
--Descripción   : Obtiene datos de resumen de PU FONDOS
--PARÁMETROS ENTRADA: @p_CodigoPortafolioSBS : CODIGO DE FONDO
--					  @p_FechaAperturaDesde  : FECHA DE INICIO
--                    @p_FechaAperturaHasta  : FECHA DE FIN
--===============================================================
--Modificado Por    : Ricardo Colonia
--Fecha modificacion: 05/08/2019
--OT: 12168
--Descripción   : Se modifica SP para Migración SQL 2016
--===============================================================
--EXEC dbo.Contabilidad_Reportes_Resumen_Envio_PU_FONDO 'HO-FONDO1','20090117','20090217'
CREATE PROCEDURE [dbo].[Contabilidad_Reportes_Resumen_Envio_PU_FONDO]
(
	 @p_CodigoPortafolioSBS varchar(10)  = '',
	 @p_FechaAperturaDesde  numeric(8,0) = 0,
	 @p_FechaAperturaHasta  numeric(8,0) = 0
)
AS
BEGIN
	DECLARE @STRSQL NVARCHAR(4000);
	SET QUOTED_IDENTIFIER ON;
	SET NOCOUNT ON;
	SET @STRSQL =  ' SELECT ''FechaContable'' = '''',
					         ''Glosa''         = CASE WHEN LEFT(NUMEROASIENTO,1) = ''I'' THEN ''COMPRA Y VENTA DE INVERSIONES''
                                                    WHEN LEFT(NUMEROASIENTO,1) = ''P'' THEN ''PROVISIÓN DE POLIZAS''
													WHEN LEFT(NUMEROASIENTO,1) = ''V'' THEN ''VALORIZACION DE LA CARTERA''
												    WHEN LEFT(NUMEROASIENTO,1) = ''T'' THEN ''COBRANZA Y CANCELACION''
													WHEN LEFT(NUMEROASIENTO,1) = ''R'' THEN ''PROVISIÓN DE INTERESES''
												END,
		  					 ''MontoDebe''     = SUM((CASE DebeHaber WHEN ''D'' THEN ROUND(IMPORTE,2) ELSE 0 END)),
   							 ''MontoHaber''    = SUM((CASE DebeHaber WHEN ''H'' THEN ROUND(IMPORTE,2) ELSE 0 END))
                      FROM ASIENTOCONTABLE
	                  WHERE FECHAASIENTO BETWEEN ' + CAST(@p_FechaAperturaDesde AS NVARCHAR) + ' AND ' + CAST(@p_FechaAperturaHasta AS NVARCHAR) + '
                            AND SITUACION = ''A''
                            AND SECUENCIA <> 0
			    AND NOT LEFT(NUMEROASIENTO,1) = ''R''';
	--CODIGO DE PORTAFOLIO SBS
	IF @p_CodigoPortafolioSBS <> ''
	BEGIN
	  SET @STRSQL = @STRSQL + ' AND CODIGOPORTAFOLIOSBS = ''' + @p_CodigoPortafolioSBS + '''';
	END
	--AGRUPAMIENTO
    SET @STRSQL = @STRSQL + ' GROUP BY CASE WHEN LEFT(NUMEROASIENTO,1) = ''I'' THEN ''COMPRA Y VENTA DE INVERSIONES''
                                            WHEN LEFT(NUMEROASIENTO,1) = ''P'' THEN ''PROVISIÓN DE POLIZAS''
									        WHEN LEFT(NUMEROASIENTO,1) = ''V'' THEN ''VALORIZACION DE LA CARTERA''
									        WHEN LEFT(NUMEROASIENTO,1) = ''T'' THEN ''COBRANZA Y CANCELACION''
										    WHEN LEFT(NUMEROASIENTO,1) = ''R'' THEN ''PROVISIÓN DE INTERESES''
					                   END';
	--ORDENAMIENTO
	SET @STRSQL = @STRSQL + ' ORDER BY 2'
	EXEC SP_EXECUTESQL @STRSQL;
	--PRINT @STRSQL;
	SET NOCOUNT OFF;
END

GO

GRANT EXECUTE ON [dbo].[Contabilidad_Reportes_Resumen_Envio_PU_FONDO] TO [rol_sit_fondos] AS [dbo]
GO




