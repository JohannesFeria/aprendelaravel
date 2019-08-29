USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log_Datos
PRINT 'Agregar Campo CuotasCierre a la tabla dbo.PorcentajeSeries'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PorcentajeSeries]') and upper(name) = upper('CuotasCierre'))
ALTER TABLE PorcentajeSeries add CuotasCierre numeric(22,7);

PRINT 'Agregar Campo ValoresCierre a la tabla dbo.PorcentajeSeries'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PorcentajeSeries]') and upper(name) = upper('ValoresCierre'))
ALTER TABLE PorcentajeSeries add ValoresCierre numeric(22,7);


PRINT 'Actualizando cuentas contables FIRBI'
DELETE FROM [ConceptoContableFirbi] WHERE CodConceptoCont='CXP'
INSERT INTO [dbo].[ConceptoContableFirbi]
	   ([IdConceptoCont]
	   ,[CodConceptoCont]
	   ,[NombreConceptoCont]
	   ,[ReglaCuentas]
	   ,[ExclusionRegla]
	   ,[Signo]
	   ,[Situacion])
 VALUES
	   ((SELECT MAX(IdConceptoCont)  FROM  [ConceptoContableFirbi])+1
	   ,'CXP'
	   ,'Cuentas por Pagar'
	   ,'4'
	   ,'401121,401122,401131,401132,401101,425123'
	   ,'+'
	   ,'A')
 
 
DELETE FROM [ConceptoContableFirbi] WHERE CodConceptoCont='CXC'
INSERT INTO [dbo].[ConceptoContableFirbi]
	   ([IdConceptoCont]
	   ,[CodConceptoCont]
	   ,[NombreConceptoCont]
	   ,[ReglaCuentas]
	   ,[ExclusionRegla]
	   ,[Signo]
	   ,[Situacion])
 VALUES
	   ((SELECT MAX(IdConceptoCont)  FROM  [ConceptoContableFirbi])+1
	   ,'CXC'
	   ,'Cuentas por Cobrar'
	   ,'12'
	   ,''
	   ,'+'
	   ,'A')

	   
INSERT INTO [dbo].[ConceptoContableFirbi]
	   ([IdConceptoCont]
	   ,[CodConceptoCont]
	   ,[NombreConceptoCont]
	   ,[ReglaCuentas]
	   ,[ExclusionRegla]
	   ,[Signo]
	   ,[Situacion])
 VALUES
	   ((SELECT MAX(IdConceptoCont)  FROM  [ConceptoContableFirbi])+1
	   ,'CXC'
	   ,'Cuentas por Cobrar'
	   ,'4011'
	   ,'401102'
	   ,'+'
	   ,'A')


INSERT INTO [dbo].[ConceptoContableFirbi]
	   ([IdConceptoCont]
	   ,[CodConceptoCont]
	   ,[NombreConceptoCont]
	   ,[ReglaCuentas]
	   ,[ExclusionRegla]
	   ,[Signo]
	   ,[Situacion])
 VALUES
	   ((SELECT MAX(IdConceptoCont)  FROM  [ConceptoContableFirbi])+1
	   ,'CXC'
	   ,'Cuentas por Cobrar'
	   ,'425123'
	   ,''
	   ,'+'
	   ,'A')

	   
	   

	  
PRINT 'Actualizando devolucion diaria para fondos con serie'
---actualizar devolucion diaria a cero para valorcuota padre
UPDATE ValorCuota SET DevolucionComisionDiaria=0 where FechaProceso >=20190801 and CodigoSerie=''
and CodigoPortafolioSBS in (
'111','17','18','19','47','98')



----------------------------------------------------------------------------------------
--Actualizar capestre1 20190801
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.60  from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.60  from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='17'


--Actualizar capestre1 20190802
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.00  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.00  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='17'

--Actualizar capestre1 20190803
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.03  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.03  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='17'

--Actualizar capestre1 20190804
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.05 from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *437.05  from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='17'


--Actualizar capestre1 20190805
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *435.77 from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *435.77  from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='17'


--Actualizar capestre1 20190806
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *435.54 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *435.54 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='17'

--Actualizar capestre1 20190807
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *436.23 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='17'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *436.23 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='17' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='17'
------------------------------------------------------------------------------------------------------------

---capestre2
--Actualizar capestre2 20190801
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *459.09  from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *459.09  from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='18'


--Actualizar capestre2 20190802
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *457.49  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *457.49  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='18'

--Actualizar capestre2 20190803
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *457.50  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *457.50  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='18'

--Actualizar capestre2 20190804
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *457.50 from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *457.50  from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='18'


--Actualizar capestre2 20190805
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *454.34 from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *454.34  from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='18'

--Actualizar capestre2 20190806
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *454.06 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *454.06 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='18'

--Actualizar capestre2 20190807
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *455.39 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='18'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *455.39 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='18' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='18'
------------------------------------------------------------------------------------------------------------

---capestre3
--Actualizar capestre3 20190801
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *59.05 from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *59.05  from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='19'


--Actualizar capestre3 20190802
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.69  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.69  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='19'

--Actualizar capestre3 20190803
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.69  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.69  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='19'

--Actualizar capestre3 20190804
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.69 from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.69  from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='19'


--Actualizar capestre3 20190805
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *57.99 from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *57.99  from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='19'

--Actualizar capestre3 20190806
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.21 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.21 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='19'

--Actualizar capestre3 20190807
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.62 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='19'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *58.62 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='19' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='19'
------------------------------------------------------------------------------------------------------------






---PRES LATIN
--Actualizar PRES LATIN 20190801
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *959.47 from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *959.47  from PorcentajeSeries where FechaProceso =20190801 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190801 AND  CodigoPortafolioSBS='47'


--Actualizar PRES LATIN 20190802
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *960.35  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *960.35  from PorcentajeSeries where FechaProceso =20190802 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190802 AND  CodigoPortafolioSBS='47'

--Actualizar PRES LATIN 20190803
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *960.49  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *960.49  from PorcentajeSeries where FechaProceso =20190803 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190803 AND  CodigoPortafolioSBS='47'

--Actualizar PRES LATIN 20190804
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *960.64 from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *960.64  from PorcentajeSeries where FechaProceso =20190804 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190804 AND  CodigoPortafolioSBS='47'


--Actualizar PRES LATIN 20190805
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *959.27 from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *959.27  from PorcentajeSeries where FechaProceso =20190805 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190805 AND  CodigoPortafolioSBS='47'

--Actualizar PRES LATIN  20190806
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *988.15 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *988.15 from PorcentajeSeries where FechaProceso =20190806 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190806 AND  CodigoPortafolioSBS='47'

--Actualizar PRES LATIN  20190807
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *987.73 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERA') WHERE CodigoSerie='SERA' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='47'
UPDATE ValorCuota SET DevolucionComisionDiaria=(select (Porcentaje/100) *987.73 from PorcentajeSeries where FechaProceso =20190807 and CodigoPortafolioSBS='47' 
and CodigoSerie='SERB') WHERE CodigoSerie='SERB' AND FechaProceso=20190807 AND  CodigoPortafolioSBS='47'

 IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log_Datos
ELSE
    COMMIT TRANSACTION __Transaction_Log_Datos
GO 


USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log


PRINT '[dbo].[PorcentajeSeries_Insertar]'
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PorcentajeSeries_Insertar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PorcentajeSeries_Insertar]
GO
-----------------------------------------------------------------------------------------------------------
--Objetivo: Insertar en la tabla PorcentajeSeries
-----------------------------------------------------------------------------------------------------------
--	Fecha modificacion: 18/07/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 12126
--	Descripcion del cambio: Agregar campos @p_ValoresCierre, @p_CuotasCierre
-----------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[PorcentajeSeries_Insertar] 
(
	@p_CodigoPortafolioSBS	VARCHAR(10),
	@p_CodigoSerie			VARCHAR(100),
	@p_FechaProceso			NUMERIC(8,0),
	@p_Porcentaje			NUMERIC(22,7),
	@p_UsuarioCreacion		VARCHAR(50),
	@p_FechaCreacion		NUMERIC(8,0),
	@p_HoraCreacion			VARCHAR(10),
	@p_Host					VARCHAR(50),
	@p_CuotasCierre			NUMERIC(22,7)=NULL,
	@p_ValoresCierre		NUMERIC(22,7)=NULL
)
AS
BEGIN

IF NOT EXISTS (SELECT 1 FROM PorcentajeSeries WHERE CodigoPortafolioSBS= @p_CodigoPortafolioSBS and FechaProceso=@p_FechaProceso AND CodigoSerie=@p_CodigoSerie)
	BEGIN
		INSERT INTO PorcentajeSeries([CodigoPortafolioSBS],[CodigoSerie],[FechaProceso],[Porcentaje],[UsuarioCreacion],[FechaCreacion],[HoraCreacion],[Host],[CuotasCierre],[ValoresCierre])
		VALUES (@p_CodigoPortafolioSBS,@p_CodigoSerie,@p_FechaProceso,@p_Porcentaje,@p_UsuarioCreacion,@p_FechaCreacion,@p_HoraCreacion,@p_Host,@p_CuotasCierre,@p_ValoresCierre)

	END
ELSE
	BEGIN
		UPDATE PorcentajeSeries 
		SET 
		UsuarioModificacion = @p_UsuarioCreacion,
		FechaModificacion = @p_FechaCreacion,
		HoraModificacion = @p_HoraCreacion,
		CuotasCierre=@p_CuotasCierre,
		ValoresCierre=@p_ValoresCierre,
		Porcentaje=@p_Porcentaje
		WHERE CodigoPortafolioSBS= @p_CodigoPortafolioSBS and FechaProceso=@p_FechaProceso AND CodigoSerie=@p_CodigoSerie
	END

END

GRANT EXECUTE ON [dbo].[PorcentajeSeries_Insertar] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[Portafolio_Series_CuotasFirbi]'
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Portafolio_Series_CuotasFirbi]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Portafolio_Series_CuotasFirbi]
GO
----------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: Obtener las series cuotas del fondo FIRBI
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Modificación: 05/03/2018
-- Modificado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11143
-- Descripción del cambio: Obtener las series cuotas del fondo FIRBI
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Modificación: 18/07/2019
-- Modificado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12126
-- Descripción del cambio: Se agrego el campo CodigoPortafolioSO
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Portafolio_Series_CuotasFirbi]  
(
	@P_CodigoPortafolioSBS VARCHAR(10),
	@P_FechaVal NUMERIC(8,0)
)	
AS
BEGIN
	DECLARE @fechaUltValCuo NUMERIC(8)
	SET @fechaUltValCuo = ISNULL(
		(SELECT MAX(FechaProceso) FROM ValorCuota
		WHERE CodigoPortafolioSBS = @P_CodigoPortafolioSBS AND FechaProceso < @P_FechaVal),@P_FechaVal
	)
	SELECT  Valor = 0,PS.CodigoPortafolioSBS,CodigoSerie = PS.CodigoSerie,Nombre = ISNULL(PS.NombreSerie,''),
	--CuotasPrecierre = ISNULL(VC.ValCuotaPreCierre,0),ValoresPrecieree = ISNULL(VC.ValCuotaPreCierreVal,0),
		--CuotasCierre = ISNULL(VC.ValCuotaCierre,0),
		CuotasCierre=ISNULL(PR.CuotasCierre,0.0000000),
		CuotasPrecierre=ISNULL(PR.CuotasCierre,0.0000000),
		ValoresCierre= ISNULL(PR.ValoresCierre,0.0000000),
		ValoresPrecierre = ISNULL(PR.ValoresCierre,0),
		CuotasCierreOnline=0.0000000,
		ValoresCierreOnline=0.0000000,
		PorcentajeOnline=0.00,
		ValoresCieree = ISNULL(VC.ValCuotaValoresCierre,0.0000000),
		Porcentaje = ISNULL(PR.Porcentaje,0),
		PS.CodigoPortafolioSO,
		PorcentajeSerie = PS.Porcentaje,
		DiferenciaValoresCierre = 0.0000000,
		FlagDiferencia ='0',
		DiferenciaMensaje='PRUEBA',
		ValorCero=0.0000000
	FROM PortafolioSerie PS
	LEFT JOIN PorcentajeSeries PR ON Ps.CodigoSerie = PR.CodigoSerie AND PS.CodigoPortafolioSBS = PR.CodigoPortafolioSBS
		AND PR.FechaProceso = @P_FechaVal AND PR.Porcentaje > 0 
	LEFT JOIN ValorCuota VC ON PS.CodigoSerie = VC.CodigoSerie AND PS.CodigoPortafolioSBS = VC.CodigoPortafolioSBS
		 --AND VC.FechaProceso = PR.FechaProceso
		 AND VC.FechaProceso = @fechaUltValCuo
	WHERE PS.CodigoPortafolioSBS = @P_CodigoPortafolioSBS 
END

GRANT EXECUTE ON [dbo].[PorcentajeSeries_Insertar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Pr_Sit_Rpt_Rentabilidad]'

USE [SIT-FONDOS]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='Pr_Sit_Rpt_Rentabilidad') BEGIN 
	DROP PROCEDURE [dbo].[Pr_Sit_Rpt_Rentabilidad]
END
GO


---------------------------------------------------------------------------------------------------------------------  
--Objetivo: Listar el reporte de rentabilidad  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 15-12-2016  
-- Modificado por: Carlos Espejo  
-- Nro. Orden de Trabajo: 9682  
-- Descripcion del cambio: Se modifico el procedimiento      
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 28-12-2016  
-- Modificado por: Everth Martinez  
-- Nro. Orden de Trabajo: 9764  
-- Descripcion del cambio: retirar el select del temporal  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 08/05/2017  
-- Modificado por: Espejo Huerta Carlos  
-- Nro. Orden de Trabajo: 10328  
-- Descripcion del cambio: Se valida el cambio de moneda para los forward y los campos que se muestran en un OR  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 11/05/2017  
-- Modificado por: Espejo Huerta Carlos  
-- Nro. Orden de Trabajo: 10372  
-- Descripcion del cambio: Para las Operaciones de Reporte no se muestra el nemonico de la accion, se toma el titulo  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 18/05/2017  
-- Modificado por: Espejo Huerta Carlos  
-- Nro. Orden de Trabajo: 10362  
-- Descripcion del cambio: Para los forwards se solicita que tambien presente valor en el campo CODIGO_ISIN  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Modificacion: 01/06/2017  
-- Modificado por: Magno Sanchez  
-- Nro. Orden de Trabajo: 10525  
-- Descripcion del cambio: Para los forwards se solicita que la informacion debe ser con la moneda del fondo y no en la del instrumento  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 10/07/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10610  
-- Descripcion del cambio: Se corrigio la obtencion del VPNLoca y VPNAyer para los portafolios que no presentan valo-  
--                         rizacion en la fecha T  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 08/08/2017  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 10686  
-- Descripcion del cambio: Se corrigio que las pre-cancelaciones de DPZ al momento de su vencimiento aparezca el monto  
--                         en el campo de vencimiento de la rentabilidad  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 17/08/2018  
-- Creado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11512  
-- Descripcion del cambio: Reemplazar la funcion "dbo.retornarPrecioVector" por "dbo.RetornarVectorPrecioT_1"  
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 30/08/2018  
-- Creado por: Ian Pastor M.  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar la funcion dbo.RetornarSecuenciaVectorTipoCambio  
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 31/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecucion del VECTOR PRECIO  
-----------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 31/07/2019  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 12028
-- Descripcion del cambio: Tomar la moneda de fondo en las negociaciones del dia
-----------------------------------------------------------------------------------------------------------------------  
--	Fecha modificacion: 18/07/2019
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 12126
--	Descripcion del cambio: Agregar tipo de Instrumentos facturas negociables
-----------------------------------------------------------------------------------------------------------------------
/*
EXEC Pr_Sit_Rpt_Rentabilidad '112',20190712,'A'
*/

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
   
	 --Declaracion e inicializacion de la fechASecuencial.  
	 DECLARE 
		@fechASecuencial NUMERIC(8) = @p_fechaOperaciON  
   
	 --Declaracion e inicializacion fecha anterior  
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
 
 	--Cartera Facturas del día
	INSERT INTO 
		@tCartera
	SELECT 
		PORTAFOLIO = p.DescripciON,GRUPO = ISNULL(ti.CodigoTipoInstrumentoSBS,'') + '-' + M.CodigoMONedaSBS,
		DESCRIPCION = ISNULL(TIH.Descripcion, ti.DescripciON),
		MONEDA = m.CodigoMONeda,EMISION = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE '' END,
		ENTIDAD = E.CodigoEntidad,
		CODIGO_SBS = ctv.CodigoSBS,CODIGO_ISIN = '',
		FECHA = ctv.FechaValoraciON,
		FECHA_EMISION = ISNULL(dbo.FormatDate(OI.FechaOperaciON),''),
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.FechaValoraciON),''),
		NRO_TITULO = OI.NumeroIdentificaciON,
		TITULO = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE DBO.fn_EquivalenciaNemonico(OI.CodigoMnemonico,OI.CodigoTipoCupon) END,
		PRECIO = 0,
		CANTIDAD = 0,
		MONTO = 0,
		VPN_HOY = ctv.VPNMonedaFondo,
		VPN_AYER = dbo.Fn_Sit_RetornarVPNLocalAnterior(ctv.CodigoPortafolioSBS,ctv.CodigoSBS,ctv.FechaValoraciON),
		COMPRAS = 0,
		VENTAS = 0,
		VENCIMIENTOS = (CASE WHEN (OI.CategoriaInstrumento = 'OR' AND OI.FechaContrato = @p_fechaOperaciON) 
		                     THEN dbo.Fn_Sit_RetornarVPNLocalAnterior(ctv.CodigoPortafolioSBS,ctv.CodigoSBS,ctv.FechaValoraciON) ELSE 0 END),
		AMORTIZACIONES = 0,
		INTERESES = 0,
		DIVIDENDOS = 0,
		RENTABILIDAD = 0
		FROM 
			CarteraTituloValoraciON ctv
		JOIN 
			OrdenInversiON OI ON OI.CodigoMnemonico = CTV.CodigoMnemonico 
								 AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS 
								 AND OI.CategoriaInstrumento IN ('FA')
								 AND CodigoOperaciON  IN ('1')
		JOIN 
			Portafolio p ON ctv.CodigoPortafolioSBS = p.CodigoPortafolioSBS
		JOIN 
			Valores v ON ctv.CodigoMnemONico = v.CodigONemONico 
						 AND v.SituaciON = @p_estado
		JOIN 
			TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = v.CodigoTipoInstrumentoSBS 
								  AND ti.SituaciON = @p_estado
		JOIN 
			MONeda m ON v.CodigoMONeda = m.CodigoMONeda AND m.SituaciON = @p_estado 
		JOIN 
			Entidad E ON E.CodigoTercero = OI.CodigoTercero AND E.SituaciON = 'A'
		LEFT JOIN 
			TipoInstrumentoHomologacion TIH ON TIH.CodigoTipoInstumentoSBS = V.CodigoTipoInstrumentoSBS 
											   AND TIH.CodigoMoneda = V.CodigoMoneda 
		WHERE 
			ctv.CodigoPortafolioSBS = @p_codigoPortafolioSbs 
			AND ctv.FechaValoraciON = @fechASecuencial 
			AND ctv.Escenario = 'REAL'

	--Cartera Facturas fecha anterior
	INSERT INTO 
		@tCartera
	SELECT 
		PORTAFOLIO = p.DescripciON,
		GRUPO = ISNULL(ti.CodigoTipoInstrumentoSBS,'') + '-' + M.CodigoMONedaSBS,
		DESCRIPCION = ISNULL(TIH.Descripcion, ti.DescripciON),
		MONEDA = m.CodigoMONeda,EMISION = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE '' END,
		ENTIDAD = E.CodigoEntidad,
		CODIGO_SBS = ctv.CodigoSBS,
		CODIGO_ISIN = '',
		FECHA = @fechASecuencial,
		FECHA_EMISION = ISNULL(dbo.FormatDate(OI.FechaOperaciON),''),
		FECHA_VALORACION = ISNULL(dbo.FormatDate(ctv.FechaValoraciON),''),
		NRO_TITULO = OI.NumeroIdentificaciON,
		TITULO = CASE WHEN OI.CategoriaInstrumento = 'OR' THEN OI.CODIGOTIPOTITULO ELSE DBO.fn_EquivalenciaNemonico(OI.CodigoMnemonico,OI.CodigoTipoCupon) END,
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
	JOIN OrdenInversiON OI ON OI.CodigoMnemonico = CTV.CodigoMnemonico 
							  AND OI.CodigoPortafolioSBS = CTV.CodigoPortafolioSBS 
							  AND OI.CategoriaInstrumento IN ('FA')
							  AND CodigoOperaciON  IN ('1')
	JOIN 
		Portafolio p ON ctv.CodigoPortafolioSBS = p.CodigoPortafolioSBS
	JOIN 
		Valores v ON ctv.CodigoMnemONico = v.CodigONemONico 
					 AND v.SituaciON = @p_estado
	JOIN 
		TipoInstrumento ti ON ti.CodigoTipoInstrumentoSBS = v.CodigoTipoInstrumentoSBS 
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
		AND NOT EXISTS(SELECT 1 
					   FROM @tCartera 
					   WHERE fecha = @fechASecuencial 
					         AND CodigoNemonico = ctv.CodigoMnemonico 
							 AND portafolio = p.Descripcion)

 
  
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
   
 --Se obtienen los movimientos de lAS operaciONes del dia  
	 INSERT INTO 
		@t_Inversiones_prev  
	 SELECT  
		 Portafolio = p.DescripciON,
		 codigoSbs = CASE WHEN oi.CodigoOperacion = '6' 
		                  THEN (SELECT CodigoSBS FROM OrdenInversion WHERE CodigoOrden = oi.OrdenGenera)
					      ELSE (CASE WHEN oi.CategoriaInstrumento = 'FA' THEN oi.CodigoMnemonico ELSE oi.CodigoSBS END) END,
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
-- Fecha de ModIFicación: 08/08/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12126 
-- Descripción del cambio: Se modifico el store para que devuelva el codigo de los fondos que negocian en otros
----------------------------------------------------------------------------------------------------------------------------------------
--SELECT CodigoPortafolioSisOpe,* FROM Portafolio WHERE CodigoPortafolioSBS IN ('5','7','8')
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
 SELECT ''
 ELSE
 BEGIN

	DECLARE @FechaMaxima numeric(8,0)=(SELECT  MAX(FechaSaldo) FROM CustodioSaldo 
	WHERE CodigoPortafolioSBS  = @p_CodigoPortafolioSBS and FechaSaldo<=@p_FechaOperacion and (SaldoInicialUnidades + IngresoUnidades - EgresoUnidades) > 0)
	--select @FechaMaxima
   IF  EXISTS ( SELECT 1 FROM Valores WHERE CodigoNemonico in ( (SELECT  CodigoMnemonico FROM CustodioSaldo 
	WHERE CodigoPortafolioSBS  = @p_CodigoPortafolioSBS and FechaSaldo = @FechaMaxima
	and (SaldoInicialUnidades + IngresoUnidades - EgresoUnidades) > 0) 
  ) AND ISNULL(CodigoPortafolioSBS,'')<>'' )
		BEGIN
		SELECT STUFF((
		SELECT ',' + ISNULL(CodigoPortafolioSisOpe,'')
		FROM portafolio WHERE CodigoPortafolioSBS IN ( SELECT CodigoPortafolioSBS FROM Valores WHERE CodigoNemonico in ( (SELECT  CodigoMnemonico FROM	CustodioSaldo 
			WHERE CodigoPortafolioSBS  = @p_CodigoPortafolioSBS and FechaSaldo = @FechaMaxima
			and (SaldoInicialUnidades + IngresoUnidades - EgresoUnidades) > 0) 
		  ) AND ISNULL(CodigoPortafolioSBS,'')<>'')
		FOR XML PATH ('')),1,1, '')
		END

	ELSE
	SELECT ''
 END
 
END
GO

GRANT EXECUTE ON [dbo].[sp_SIT_ValidarNegociacionFondosEnOtros] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[PagoFechaComision_Seleccionar]'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_Seleccionar]') AND TYPE IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[PagoFechaComision_Seleccionar] 
GO

----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de ModIFicacion: 06/06/2019
-- ModIFicado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 12126 
-- Descripcion del cambio: Se obtiene como fecha máxima la fecha de caja
----------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PagoFechaComision_Seleccionar]  
	@p_Id int,  
	@p_NumeroCuenta varchar(25) ='',  
	@p_CodigoPortafolioSBS varchar(10)=''  
AS  
BEGIN  
  
	DECLARE @SaldoOnline NUMERIC(22,7)  
	DECLARE @NumeroCuenta VARCHAR(25)   
	DECLARE @CodigoPortafolioSBS VARCHAR(10)  
  
  
	SELECT  
		@CodigoPortafolioSBS=CodigoPortafolioSBS,  
		@NumeroCuenta = NumeroCuenta  
	FROM   
		PagoFechaComision WITH(NOLOCK) 
	WHERE 
		Id= @p_Id  
  
  
	IF @p_NumeroCuenta<>''  
	SET @NumeroCuenta=@p_NumeroCuenta  
  
	IF @p_CodigoPortafolioSBS<>''  
	SET @CodigoPortafolioSBS=@p_CodigoPortafolioSBS  
  
	DECLARE @FechaMaxima numeric(8,0) = (  
	 --SELECT   
	 --MAX(SB.FechaOperacion)  
	 --FROM    
	 --SaldosBancarios SB   
	 --WHERE SB. CodigoPortafolioSBS=@p_CodigoPortafolioSBS and NumeroCuenta in (select NumeroCuenta  from CuentaEconomica where CodigoClaseCuenta='20' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and numerocuenta<>'' and situacion='A')  
 		SELECT
			FechaCajaOperaciones
		FROM
			Portafolio 
		WHERE
			CodigoPortafolioSBS=@p_CodigoPortafolioSBS 

	 )  
  
	 SELECT 
		@SaldoOnline = SaldoEstadoCTA 
	 FROM  
		SaldosBancarios SB  
	 WHERE 
		SB.FechaOperacion = @FechaMaxima  
		AND SB.CodigoPortafolioSBS = @CodigoPortafolioSBS  
		AND SB.NumeroCuenta = @NumeroCuenta  
  
  
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
	 INNER JOIN 
		Portafolio po ON po.CodigoPortafolioSBS=pf.CodigoPortafolioSBS   
	 LEFT JOIN 
		PortafolioCaja Pc ON  Pc.CodigoClaseCuenta='20' 
							  AND Pc.CodigoPortafolioSBS=po.CodigoPortafolioSBS   
	 WHERE 
		pf.Id= @p_Id  
  
END  
GRANT EXECUTE ON  [dbo].[PagoFechaComision_Seleccionar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[PagoFechaComision_ListarNumeroDeCuentas]'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoFechaComision_ListarNumeroDeCuentas]') AND TYPE IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[PagoFechaComision_ListarNumeroDeCuentas]
GO


----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicacion: 06/06/2019
-- ModIFicado por: Ernesto Galarza
-- Nro. Orden de Trabajo: 12028 
-- Descripcion del cambio: Creacion del Store procedure
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de ModIFicacion: 06/06/2019
-- ModIFicado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 12126 
-- Descripcion del cambio: Se obtiene como fecha máxima la fecha de caja
----------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PagoFechaComision_ListarNumeroDeCuentas]
	@p_CodigoPortafolioSBS varchar(10),
	@p_CodigoMoneda varchar(12),
	@p_CodigoBanco varchar(12)
AS
BEGIN

	DECLARE @FechaMaxima numeric(8,0) = (
	--SELECT 

	--MAX(SB.FechaOperacion)
	--FROM  
	--SaldosBancarios SB 
	--WHERE SB. CodigoPortafolioSBS=@p_CodigoPortafolioSBS and NumeroCuenta in (select NumeroCuenta  from CuentaEconomica where CodigoClaseCuenta='20' and CodigoPortafolioSBS=@p_CodigoPortafolioSBS and numerocuenta<>'' and Situacion='A' )
		SELECT
			FechaCajaOperaciones
		FROM
			Portafolio 
		WHERE
			CodigoPortafolioSBS=@p_CodigoPortafolioSBS 

	)

	
---OBTENER NUMEROS DE CUENTAS
	SELECT DISTINCT
		TE.*,
		CASE WHEN @p_CodigoPortafolioSBS='23'  
			 THEN TE.NumeroCuenta +' | ' + TE.CodigoMoneda +  ' ' + [dbo].[FN_SIT_OBT_NumeroSepador](TE.Saldo)
			 ELSE  TE.NumeroCuenta +' | ' +  [dbo].[FN_SIT_OBT_NumeroSepador](TE.Saldo) END AS 'NombreNumeroDeCuenta'
	FROM (
		SELECT 
			T.CodigoTercero,
			E.CodigoEntidad AS CodigoBanco,
			T.Descripcion Tercero,
			ce.CodigoMoneda ,
			ce.NumeroCuenta,
			ISNULL(SaldoEstadoCTA,0) as 'Saldo'
		FROM 
			CuentaEconomica CE
		LEFT JOIN 
			SaldosBancarios SB ON SB.FechaOperacion = @FechaMaxima  
								  AND SB.CodigoPortafolioSBS = CE.CodigoPortafolioSBS  
			                      AND SB.NumeroCuenta = CE.NumeroCuenta
		JOIN 
			Entidad E ON E.CodigoEntidad = CE.EntidadFinanciera 
						 AND E.Situacion = 'A'
		JOIN 
			Terceros T ON T.CodigoTercero = E.CodigoTercero
		WHERE 
			CE.CodigoPortafolioSBS =@p_CodigoPortafolioSBS
			AND CE.CodigoClaseCuenta='20'
			AND CE.CodigoMoneda = CASE WHEN @p_CodigoPortafolioSBS='23'  THEN CE.CodigoMoneda ELSE  @p_CodigoMoneda END
			AND E.CodigoEntidad =@p_CodigoBanco
			AND ISNULL(ce.NumeroCuenta,'' )<>''
	) TE 
	WHERE TE.Saldo>0
	ORDER BY TE.Tercero

END

GO

GRANT EXECUTE ON [dbo].[PagoFechaComision_ListarNumeroDeCuentas] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[Pr_Sit_listarValorCuotaTotalPorFondo]'

USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuotaTotalPorFondo]') AND TYPE IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondo]
GO


----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de ModIFicacion: 17/12/2018
-- ModIFicado por: Ricardo Colonia
-- Descripcion del cambio: ModIFicacion de campo Descripcion a VARCHAR(40)
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha ModIFicacion: 27/12/2018
-- ModIFicado por: Ian Pastor M.
-- Nro. Orden de Trabajo: 11717
-- Descripcion del cambio: Se cambio la funcion ("FormatDate") de formato de fecha, ya que la anterior ocasionaba error.
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha ModIFicacion: 09/08/2019
-- ModIFicado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 12126
-- Descripcion del cambio: Se cambia calculo de comision neta.
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Pr_Sit_listarValorCuotaTotalPorFondo](
	@p_fechaInicial NUMERIC(8),
	@p_fechaFinal NUMERIC(8))  
AS  
BEGIN  
	DECLARE 
		@tbl AS TABLE (  
						fechaProceso VARCHAR(10),
						Descripcion VARCHAR(100),
						valorCuota NUMERIC(22,7),
						CuotasEmitidas NUMERIC(22,7),
						comisionAdministradoraBruta NUMERIC(22,7),
						devolucionAdministradora NUMERIC(22,7),
						comisionAdministradoraNeta NUMERIC(22,7),
						Fecha NUMERIC(8)  
					   )  
	INSERT INTO 
		@tbl  
	SELECT 
		fechaProceso = dbo.FormatDate(t.FechaProceso),
		p.Descripcion,
		t.valorCuota,
		t.CuotasEmitidas,
	--	comisionAdministradoraBruta = t.comisionAdministradora +t.devolucionAdministradora,
		comisionAdministradoraBruta = t.comisionAdministradora ,
		devolucionAdministradora = t.devolucionAdministradora,
		comisionAdministradoraNeta= ISNULL(t.comisionAdministradora,0) - ISNULL(t.devolucionAdministradora,0),
		t.FechaProceso  
	FROM 
		(  
		SELECT
			FechaProceso,CodigoPortafolioSBS,
			valorCuota = SUM(ValCuotaValoresCierre),
			CuotasEmitidas = SUM(ValCuotaCierre),
			comisionAdministradora = SUM(ISNULL(ComisionSAFM,0)),
			devolucionAdministradora=SUM(ISNULL(DevolucionComisionDiaria,0))  
		FROM 
			ValorCuota  
		WHERE 
			FechaProceso BETWEEN @p_fechaInicial AND @p_fechaFinal  
		GROUP BY 
			FechaProceso,
			CodigoPortafolioSBS  
	) t 
	JOIN 
		Portafolio p ON p.CodigoPortafolioSBS = t.CodigoPortafolioSBS  

	SELECT 
		fechaProceso,
		Descripcion,
		valorCuota,
		CuotasEmitidas,
		comisionAdministradoraBruta,
		devolucionAdministradora,
		comisionAdministradoraNeta
	FROM 
		@tbl 
	ORDER BY  
		Fecha  
END  

GO

GRANT EXECUTE ON [dbo].[Pr_Sit_listarValorCuotaTotalPorFondo] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[dbo].[Pr_Sit_listarValorCuota]'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuota]') AND TYPE IN (N'P', N'PC'))
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
-- Fecha ModIFicacion: 09/08/2019
-- ModIFicado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 12126
-- Descripcion del cambio: Se cambia calculo de comision neta.
-------------------------------------------------------------------------------------------------------------------------------

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
		comisionAdministradora = ISNULL(vc.ComisionSAFM,0) - ISNULL(vc.DevolucionComisionDiaria,0),
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
		comisionAdministradora = ISNULL(vc.ComisionSAFM,0)  - ISNULL(vc.DevolucionComisionDiaria,0),
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

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pr_Sit_listarValorCuotaTotalPorFondoSeriado]') AND TYPE IN (N'P', N'PC'))
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
-- Fecha ModIFicacion: 09/08/2019
-- ModIFicado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 12126
-- Descripcion del cambio: Se cambia calculo de comision neta.
-------------------------------------------------------------------------------------------------------------------------------

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
		comisionAdministradora = ISNULL(vc.ComisionSAFM,0) - ISNULL(vc.DevolucionComisionDiaria,0),
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


IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 