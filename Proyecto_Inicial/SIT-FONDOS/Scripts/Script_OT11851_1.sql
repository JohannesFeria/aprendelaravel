USE [SIT-FONDOS]
GO

-- ====================================================================================================================
-- Autor: Ricardo Colonia	
-- Fecha Creación: 14/02/2019
-- Órden de Trabajo: 11851
-- Descripción: - Creación/Modificación de SP/Function.
--				- Actualización de cupones.
--				- Actualiza las tablas que contengan las columnas CodigoMnemonico y CodigoNemonico
--				  con el valor de emisión TICKER + us.
-- ====================================================================================================================
BEGIN TRANSACTION __Transaction_Log
PRINT 'INICIO --> Nuevas Columnas en Tabla Valores'

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Valores]') and upper(name) = upper('TipoTasaVariable'))
	ALTER TABLE Valores add TipoTasaVariable VARCHAR(20);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Valores]') and upper(name) = upper('DiasTTasaVariable'))
	ALTER TABLE Valores add DiasTTasaVariable INT;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Valores]') and upper(name) = upper('TasaVariable'))
	ALTER TABLE Valores add TasaVariable NUMERIC(22,7);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Valores]') and upper(name) = upper('CuponTasaVariableReferencial'))
	ALTER TABLE Valores add CuponTasaVariableReferencial INT;
	
PRINT 'FIN --> Nuevas Columnas en Tabla Valores'

PRINT 'INICIO --> Nueva Columna en Tabla CuponeraNormal'

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CuponeraNormal]') and upper(name) = upper('TasaVariable'))
	ALTER TABLE CuponeraNormal add TasaVariable NUMERIC(22,7);

PRINT 'FIN --> Nueva Columna en Tabla CuponeraNormal'

PRINT 'INICIO --> Nueva Columna en Tabla OrdenInversion_DetalleSWAP'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_DetalleSWAP]') and upper(name) = upper('DiaTLeg1'))
ALTER TABLE OrdenInversion_DetalleSWAP add DiaTLeg1 int

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_DetalleSWAP]') and upper(name) = upper('DiaTLeg2'))
ALTER TABLE OrdenInversion_DetalleSWAP add  DiaTLeg2 int

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_DetalleSWAP]') and upper(name) = upper('TasaLiborLeg1'))
ALTER TABLE OrdenInversion_DetalleSWAP add TasaLiborLeg1 numeric(22, 7);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_DetalleSWAP]') and upper(name) = upper('TasaLiborLeg2'))
ALTER TABLE OrdenInversion_DetalleSWAP add TasaLiborLeg2 numeric(22, 7);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_DetalleSWAP]') and upper(name) = upper('CuponTasaVariableReferencialLeg1'))
	ALTER TABLE OrdenInversion_DetalleSWAP add CuponTasaVariableReferencialLeg1 INT;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_DetalleSWAP]') and upper(name) = upper('CuponTasaVariableReferencialLeg2'))
	ALTER TABLE OrdenInversion_DetalleSWAP add CuponTasaVariableReferencialLeg2 INT;
PRINT 'FIN --> Nueva Columna en Tabla OrdenInversion_DetalleSWAP'


PRINT 'INICIO --> Nueva Columna en Tabla Cuponera_Bono_Swap'
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cuponera_Bono_Swap]') and upper(name) = upper('FechaLiborOriginal'))
ALTER TABLE Cuponera_Bono_Swap add FechaLiborOriginal numeric(8, 0);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Cuponera_Bono_Swap]') and upper(name) = upper('FechaLibor'))
ALTER TABLE Cuponera_Bono_Swap add FechaLibor numeric(8, 0);

PRINT 'FIN --> Nueva Columna en Tabla Cuponera_Bono_Swap'

PRINT 'INICIO ---> Insetar nueva Parametría de Periodicidad Tasa Libor'
DELETE FROM
	ParametrosGenerales
WHERE
	Clasificacion = 'PeriodoTasaLibor'
INSERT INTO 
	ParametrosGenerales (Clasificacion,
						 Nombre,
						 Valor,
						 Comentario)
	VALUES
		('PeriodoTasaLibor',
		 'T',
		 '0',
		 'Periodo de tasa libor con respecto a dia T.'),
		('PeriodoTasaLibor',
		 'T-1',
		 '-1',
		 'Periodo de tasa libor con respecto a dia T.'),
		 ('PeriodoTasaLibor',
		 'T-2',
		 '-2',
		 'Periodo de tasa libor con respecto a dia T.'),
		 ('PeriodoTasaLibor',
		 'T-3',
		 '-3',
		 'Periodo de tasa libor con respecto a dia T.'),
		 ('PeriodoTasaLibor',
		 'T-4',
		 '-4',
		 'Periodo de tasa libor con respecto a dia T.'),
		 ('PeriodoTasaLibor',
		 'T-5',
		 '-5',
		 'Periodo de tasa libor con respecto a dia T.')
PRINT 'FIN ---> Insetar nueva Parametría de Periodicidad Tasa Libor'
PRINT 'INICIO ---> Actualización de Cupones con DiasPago < 0'
UPDATE
	CuponeraNormal
SET
	DiasPago = DiasPeriodo,
	DiferenciaDias = DiasPeriodo
FROM
	CuponeraNormal CN
JOIN
	Valores VA ON CN.CodigoNemonico = VA.CodigoNemonico
JOIN
	Periodicidad PE ON VA.CodigoPeriodicidad = PE.CodigoPeriodicidad
WHERE
	CN.DiasPago <0
	

UPDATE
	CuponeraNormal
SET
	DiasPago = dbo.Dias(FechaInicio,FechaTermino),
	DiferenciaDias = dbo.Dias(FechaInicio,FechaTermino),
	FechaPago = FechaTermino,
	MontoNominalLocal = 0	
WHERE
	DiasPago = 0
	AND CodigoNemonico LIKE 'LTP%'

PRINT 'FIN ---> Actualización de Cupones con DiasPago < 0'

PRINT 'INICIO ---> Actualización Parametro General:ParTipCup'
DELETE FROM
	ParametrosGenerales
WHERE
	Clasificacion = 'ParTipCup'
INSERT INTO 
	ParametrosGenerales (Clasificacion,
						 Nombre,
						 Valor,
						 Comentario)
	VALUES
		('ParTipCup',
		 'Cupon Actual',
		 'cuponActual',
		 'Valor de Modificación Cuponera Aperiódica.'),
		('ParTipCup',
		 'En Adelante',
		 'enAdelante',
		 'Valor de Modificación Cuponera Aperiódica.')
		 
PRINT 'FIN ---> Actualización Parametro General:ParTipCup'

PRINT 'INICIO ---> Actualización Parametro General:ImporSBS'
DELETE FROM ParametrosGenerales
WHERE Clasificacion = 'ImporSBS'
	  AND Valor = 'TL'
	  AND Nombre =  'Vector Tasa Libor PIP'
	  
INSERT INTO 
	ParametrosGenerales (Clasificacion,
						 Nombre,
						 Valor,
						 Comentario)
	VALUES
		('ImporSBS',
		 'Vector Tasa Libor PIP',
		 'TL',
		 'Valores diarios de Curva de Tasa Libor')
		 
PRINT 'FIN ---> Actualización Parametro General:ImporSBS'
GO
PRINT 'INICIO --- > Actualización de cuponera SWAP incluyendo tasa libor'
UPDATE
	OrdenInversion_DetalleSWAP
SET
	DiaTLeg2 = -2,
	TasaLiborLeg2 = 2.8030000,
	CuponTasaVariableReferencialLeg2 = 1
WHERE
	CodigoOrden = '058119'
	AND CodigoPortafolioSBS = '110'

UPDATE
	OrdenInversion_DetalleSWAP
SET
	DiaTLeg2 = -2,
	TasaLiborLeg2 = 2.60875,
	CuponTasaVariableReferencialLeg2 = 1
WHERE
	CodigoOrden = '063435'
	AND CodigoPortafolioSBS = '110'

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20181227
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 1

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20190328
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 2

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20190627
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 3

	UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20190926
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 4

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20191224
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 5

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20200326
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 6

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20200626
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 7

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20200928
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 8

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20201224
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 9

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20210326
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 10

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20210628
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 11

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20210928
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 12

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20211224
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 13

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20220328
WHERE
	CodigoOrden = '058119'
	AND Correlativo = 14

---- > SWAP 063435
UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20190325
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 1

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20190625
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 2

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20190925
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 3

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20191223
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 4

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20200325
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 5

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20200625
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 6

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20200924
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 7

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20201223
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 8

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20210325
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 9

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20210624
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 10

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20210923
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 11

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20211223
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 12

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20220324
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 13

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20220623
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 14

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20220923
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 15

UPDATE
	Cuponera_Bono_Swap
SET
	FechaLibor = 20221222
WHERE
	CodigoOrden = '063435'
	AND Correlativo = 16


PRINT 'FIN --- > Actualización de cuponera SWAP incluyendo tasa libor'

PRINT 'INICIO --- > Actualización de Bonos PRES LATIN'
DECLARE
	@TasaCupon NUMERIC(22,12) = 0,
	@codigoMnemonico VARCHAR (15),
	@iCount INT = 1,
	@cant INT = 0,
	@FechaNegocio DECIMAL(8,0) = (SELECT FechaNegocio
								  FROM Portafolio
								  WHERE CodigoPortafolioSBS = '47')


DECLARE
	@TmpTablaValores  TABLE ( ID INT IDENTITY(1,1),
							  codigoMnemonico VARCHAR(15),
							  tasaCupon NUMERIC(22,7))

INSERT INTO
	@TmpTablaValores
SELECT 
	VA.CodigoNemonico,
	CN.TasaCupon
FROM
	Valores VA
JOIN
	CuponeraNormal CN ON VA.CodigoNemonico = CN.CodigoNemonico
WHERE
	VA.CodigoNemonico LIKE 'BDCA%'
	AND FechaTermino  = (SELECT MAX(FechaTermino) FROM CuponeraNormal CN2
						 WHERE CN2.CodigoNemonico = VA.CodigoNemonico
							   AND FechaTermino <= @FechaNegocio)

SET @cant = (SELECT COUNT(*) FROM @TmpTablaValores)

WHILE (@iCount < @cant)
BEGIN
	SELECT 
		@codigoMnemonico = codigoMnemonico,
		@TasaCupon = tasaCupon
	FROM
		@TmpTablaValores
	WHERE
		ID = @iCount
	PRINT @codigoMnemonico
	UPDATE
		CuponeraNormal
	SET
		TasaCupon = @TasaCupon
	WHERE
		CodigoNemonico = @codigoMnemonico
		AND FechaTermino > @FechaNegocio

	UPDATE
		Valores
	SET 
		TasaCupon = @TasaCupon
	WHERE 
		CodigoNemonico = @codigoMnemonico

	SET @iCount = @iCount + 1

END

	UPDATE
		CuponeraNormal
	SET
		Amortizacion = 4.5454545
	WHERE 
		CodigoNemonico = 'BDCA1BC1B'
		AND FechaTermino > @FechaNegocio

	UPDATE
		CuponeraNormal
	SET
		Amortizacion = 4.5454555	
	WHERE 
		CodigoNemonico = 'BDCA1BC1B'
		AND Secuencia = 26 

	UPDATE
		CuponeraNormal
	SET
		Amortizacion = 5.4665738
	WHERE 
		CodigoNemonico = 'BDCA1BC1C'
		AND FechaTermino > @FechaNegocio

	UPDATE
		CuponeraNormal
	SET
		Amortizacion = 5.466574		
	WHERE 
		CodigoNemonico = 'BDCA1BC1C'
		AND Secuencia = 20 

	UPDATE
		CuponeraNormal
	SET
		Amortizacion = 0
	WHERE 
		CodigoNemonico = 'BDCA1BC2'
		AND FechaTermino > @FechaNegocio

	UPDATE
		CuponeraNormal
	SET
		Amortizacion = 86.1111120		
	WHERE 
		CodigoNemonico = 'BDCA1BC2'
		AND Secuencia = 20 
PRINT 'FIN --- > Actualización de Bonos PRES LATIN'
IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 

PRINT 'INICIO --- > SECCIÓN FUNCTION'

PRINT 'dbo.SIT_fn_EsAnioBisiesto.FNC'
USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SIT_fn_EsAnioBisiesto]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[SIT_fn_EsAnioBisiesto]
GO

----------------------------------------------------------------------------------------------------------------------  
--Objetivo: RETORNAR 1 SI ES AÑO BISIESTO O 0 SI NO ES AÑO BISIESTO.
----------------------------------------------------------------------------------------------------------------------  
-- Fecha Creacion: 21/03/2019  
-- Creado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11851  
-- Descripcion del cambio: Nuevo
----------------------------------------------------------------------------------------------------------------------  

CREATE FUNCTION [dbo].[SIT_fn_EsAnioBisiesto](@anio VARCHAR(4)) 
RETURNS INT AS
BEGIN
	DECLARE 
		@resultado AS INT
	IF DATEPART(dd,DATEADD(d,-1,(@anio+'0301')))=29 
		SET @resultado=1
	ELSE
		SET @resultado=0
	RETURN @resultado
END


GO

GRANT EXECUTE ON [dbo].[SIT_fn_EsAnioBisiesto] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'FIN --- > SECCIÓN FUNCTION'


PRINT 'INICIO --- > SECCIÓN DE SP'
BEGIN TRANSACTION __Transaction_SP

PRINT 'dbo.Amortizar_CuponeraNormal.PRC'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Amortizar_CuponeraNormal]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Amortizar_CuponeraNormal]
GO

-----------------------------------------------------------------------------------------------------------
-- OBJETIVO: OBTENER LA AMORTIZACION, EL INTERES Y EL SALDO DE LOS CUPONES
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 25/02/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Creacion
------------------------------------------------------------------------------------------------------------

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
				SELECT @Diferencia= dbo.dias(@p_fechaIni,@p_fechaFin)

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
						SELECT @Diferencia= dbo.dias(@fechaTemp,@p_fechaFin)
					
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
						DifDias = (CASE WHEN @p_numeroDias = '30' THEN dbo.dias360(@p_fechaFin,FechaFin) ELSE dbo.dias(@p_fechaFin,FechaFin) END),
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

PRINT 'dbo.sp_SIT_GenerarCuponera_Fechas.PRC'
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
			SELECT @Diferencia= dbo.dias(@p_fechaIni,@p_fechaFin)

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
		DifDias = (CASE WHEN @p_numeroDias = '30' THEN dbo.dias360(FechaIni,@p_fechaVcto) ELSE dbo.dias(FechaIni,@p_fechaVcto) END)
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


PRINT 'dbo.Obtener_CuponActual.PRC'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Obtener_CuponActual]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Obtener_CuponActual]
GO

------------------------------------------------------------------------------------------------------------
-- OBJETIVO: OBTENER EL CUPON ACTUAL
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 25/02/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 
--	Descripcion del cambio: Creacion
------------------------------------------------------------------------------------------------------------
--exec [Obtener_CuponActual] 'B 0 01/31/19 GO'
CREATE PROCEDURE [dbo].[Obtener_CuponActual]
(
	@p_CodigoNemonico VARCHAR(15)
)AS
BEGIN 
	DECLARE 
		@existeCupon VARCHAR(1) = '0'
		-- Verificar si la fecha de emision es la misma que la fecha inicio del primer cupon
	DECLARE 
		@fechaEmision NUMERIC (8,0) = (SELECT FechaEmision 
									   FROM Valores 
									   WHERE CodigoNemonico=@p_CodigoNemonico)

	IF EXISTS(SELECT 1 FROM CuponeraNormal WHERE CodigoNemonico=@p_CodigoNemonico)
	BEGIN
		SET @existeCupon='1'
	END	


	DECLARE @FechaNegocio NUMERIC(8,0) =ISNULL((SELECT MAX(FechaNegocio) 
												FROM Portafolio PO
												JOIN OrdenInversion CV ON PO.CodigoPortafolioSBS = CV.CodigoPortafolioSBS
												WHERE CV.CodigoMnemonico = @p_CodigoNemonico),
												(SELECT MAX(FechaNegocio) 
												 FROM Portafolio PO
												 WHERE PO.Situacion = 'A'))
	IF EXISTS(SELECT COUNT(1) 
					  FROM CuponeraNormal			
					  WHERE FechaInicio = @FechaNegocio
						    AND CodigoNemonico = @p_CodigoNemonico)
				BEGIN
					SET @FechaNegocio = dbo.RetornarFechaModificadaEnDias(@FechaNegocio,-1)
				END
	
	SELECT @existeCupon AS 'ExisteCupon'
	-- Verificar si existe el cupon
	SELECT TOP 1 * 
	FROM CuponeraNormal 
	WHERE CodigoNemonico = @p_CodigoNemonico 
		  AND @FechaNegocio BETWEEN FechaInicio AND FechaTermino 
	ORDER BY SECUENCIA ASC


END



GRANT EXECUTE ON [dbo].[Obtener_CuponActual] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.Calcular_CuponeraNormal.PRC'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Calcular_CuponeraNormal]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Calcular_CuponeraNormal]
GO

------------------------------------------------------------------------------------------------------------
-- OBJETIVO: CALCULO DE PERIODO Y AMORTIZACION DE LOS CUPONES
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 25/02/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Creacion
------------------------------------------------------------------------------------------------------------
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
							AND CodigoNemonico = @p_CodigoNemonico)
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
				SELECT @Diferencia= dbo.dias(@p_FechaInicio,@p_FechaFin)

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


PRINT 'dbo.TasaLibor_EliminarVectorCarga_PIP.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TasaLibor_EliminarVectorCarga_PIP]') AND type in (N'P', N'PC'))
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
CREATE PROCEDURE [dbo].[TasaLibor_EliminarVectorCarga_PIP] 
	(
		@p_Fecha NUMERIC(8, 0)
	)
AS
BEGIN
	DELETE FROM 
		ValorIndicador
	WHERE 
		Fecha=@p_Fecha 
		AND CodigoIndicador LIKE 'SW-%'
END

GO

GRANT EXECUTE ON [dbo].[TasaLibor_EliminarVectorCarga_PIP]  TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.Indicador_SeleccionarIndicadorLibor.PRC'
USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Indicador_SeleccionarIndicadorLibor]') AND type in (N'P', N'PC'))
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
CREATE PROCEDURE [dbo].[Indicador_SeleccionarIndicadorLibor]
(
	@p_CodigoIndicador	VARCHAR(10) = ''
)
AS

	SELECT
		IR.[CodigoIndicador], 
		CONVERT(INT, LEFT(NombreIndicador, PATINDEX('%[^0-9]%',NombreIndicador +' ')-1))*30 DiasLibor
	FROM
		Indicador AS IR
	WHERE
        IR.CodigoIndicador LIKE (CASE WHEN LEN(@p_CodigoIndicador) = 0 THEN IR.CodigoIndicador ELSE '%' + @p_CodigoIndicador + '%' END)
		AND IR.Situacion = 'A'
		
	Order By 
		IR.NombreIndicador


GO

GRANT EXECUTE ON [dbo].[Indicador_SeleccionarIndicadorLibor] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.TasaLibor_InsertarVectorCarga_PIP.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TasaLibor_InsertarVectorCarga_PIP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TasaLibor_InsertarVectorCarga_PIP] 
GO

---------------------------------------------------------------------------------------------------------------------
-- Objetivo: INSERTAR VECTOR CARGA TASA LIBOR 
---------------------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/03/2019
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Nuevo
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[TasaLibor_InsertarVectorCarga_PIP] 
	(
	 @p_fechaVector NUMERIC(8,0),
	 @p_plazoCurva INT,
	 @p_valorCurva NUMERIC(22,7),
	 @p_CodigoIndicador VARCHAR(10),
	 @p_UsuarioCreacion VARCHAR(15),  
	 @p_FechaCreacion NUMERIC(8,0),  
	 @p_HoraCreacion VARCHAR(10),  
	 @p_Host VARCHAR(20)  
	)
AS
BEGIN
	BEGIN TRANSACTION 
	BEGIN TRY
		DECLARE 
			@secuencia NUMERIC(10)
		
		SET @secuencia = (SELECT MAX(Secuencia) FROM ValorIndicador)
		
		INSERT INTO
			ValorIndicador
						  (Secuencia,
						   CodigoIndicador,
						   DiasPeriodo,
						   Fecha,
						   Valor,
						   Situacion,
						   UsuarioCreacion,
						   FechaCreacion,
						   HoraCreacion,
						   Host)
		VALUES
			 (@secuencia + 1,
			  @p_CodigoIndicador,
			  0,
			  @p_fechaVector,
			  @p_valorCurva,
			  'A',
			  @p_UsuarioCreacion,
			  @p_FechaCreacion,
			  @p_HoraCreacion,
			  @p_Host)
	
	COMMIT TRANSACTION 
	END TRY 
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION    
		EXEC pr_SIT_retornarError 
	END CATCH
END

GO

GRANT EXECUTE ON [dbo].[TasaLibor_InsertarVectorCarga_PIP]  TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.Indicador_SeleccionarSWAP.PRC' 
USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Indicador_SeleccionarSWAP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Indicador_SeleccionarSWAP]
GO


-----------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Obtiene las tasas flotantes para la negociación de SWAP
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 12/12/2018  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11687
-- Descripcion del cambio: Nuevo Store Procedure
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 13/03/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Se añade opción de obtener indicador de acuerdo al @p_CodigoIndicador
-----------------------------------------------------------------------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[Indicador_SeleccionarSWAP]
(
	@p_CodigoIndicador	VARCHAR(10) = ''
)
AS
	IF UPPER(@p_CodigoIndicador) = 'SW'
	BEGIN
		SELECT
			IR.[CodigoIndicador], 
			ISNULL(IR.[NombreIndicador],'') + ' | ' +  CONVERT(VARCHAR(30), ROUND(ISNULL(VIR.[Valor],0),7)) NombreIndicador
		FROM
			Indicador AS IR
		JOIN
			ValorIndicador AS VIR ON IR.CodigoIndicador = VIR.CodigoIndicador 
		WHERE
			IR.CodigoIndicador LIKE (CASE WHEN LEN(@p_CodigoIndicador) = 0 THEN IR.CodigoIndicador ELSE '%' + @p_CodigoIndicador + '%' END)
			AND IR.Situacion = 'A'
			AND VIR.Fecha IN (SELECT MAX(FECHA) FROM ValorIndicador VIR2 WHERE VIR2.CodigoIndicador = VIR.CodigoIndicador )
		Order By 
			IR.NombreIndicador
	END
	ELSE
	BEGIN
		SELECT
			IR.[CodigoIndicador], 
			IR.[NombreIndicador]
		FROM
			Indicador AS IR
		WHERE
			IR.CodigoIndicador LIKE (CASE WHEN LEN(@p_CodigoIndicador) = 0 THEN IR.CodigoIndicador ELSE '%' + @p_CodigoIndicador + '%' END)
			AND IR.Situacion = 'A'
		Order By 
			IR.NombreIndicador
	END

GO

GRANT EXECUTE ON [dbo].[Indicador_SeleccionarSWAP] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.Valores_ActualizarTasaVariable.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Valores_ActualizarTasaVariable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Valores_ActualizarTasaVariable]
GO

----------------------------------------------------------------------------------------------------------------------
-- Fecha Modificacion: 14/03/2019
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Se graban los nuevos campos de tabla Valores para Tasa Variable.
----------------------------------------------------------------------------------------------------------------------
CREATE  PROCEDURE [dbo].[Valores_ActualizarTasaVariable] (
	@p_CodNemonico VARCHAR(15),  
	@p_tipoTasaVariable VARCHAR (20) = '',
	@p_tasaVariable NUMERIC(22,7) = 0,
	@p_periodicidadTasaVariable INT = 0,
	@p_cuponTasaVariableReferencial INT

)  
AS  
  
SET NOCOUNT ON  

--RGF 20090615 se agrego IF EXISTS xq estaba saliendo "Violation of PRIMARY KEY"
	IF EXISTS (SELECT 1 
			   FROM CuponeraNormal 
			   WHERE CodigoNemonico = @p_CodNemonico)
	BEGIN
		UPDATE
			Valores
		SET
			TipoTasaVariable = @p_tipoTasaVariable,
			TasaVariable = @p_tasaVariable,
			DiasTTasaVariable = @p_periodicidadTasaVariable,
			CuponTasaVariableReferencial = @p_cuponTasaVariableReferencial
		WHERE 
			 CodigoNemonico = @p_CodNemonico
	END
GO

GRANT EXECUTE ON [dbo].[Valores_ActualizarTasaVariable] TO [rol_sit_fondos] AS [dbo]
GO



PRINT 'dbo.Indicador_SeleccionarValorLibor.PRC'
USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Indicador_SeleccionarValorLibor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Indicador_SeleccionarValorLibor]
GO


-----------------------------------------------------------------------------------------------------------------------------------------  
--Objetivo: Obtiene las tasas flotantes para una fecha de referencia.
-----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 14/03/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Nuevo Store Procedure
-----------------------------------------------------------------------------------------------------------------------------------------  

CREATE PROCEDURE [dbo].[Indicador_SeleccionarValorLibor]
(
	@p_CodigoIndicador	VARCHAR(10) = '',
	@p_fechaReferencia DECIMAL(8)
)
AS
	DECLARE @Resultado DECIMAL(22,7) = 0
	
	SET @Resultado = ISNULL((SELECT TOP 1
								ROUND(ISNULL(VIR.[Valor],0),7) ValorLibor
							 FROM
								Indicador AS IR
							 JOIN
								ValorIndicador AS VIR ON IR.CodigoIndicador = VIR.CodigoIndicador 
							 WHERE
								IR.CodigoIndicador = @p_CodigoIndicador
								AND IR.Situacion = 'A'
								AND VIR.Fecha = @p_fechaReferencia
							 ORDER BY 
								VIR.[Valor] DESC),0)
	SELECT @Resultado
	
GO

GRANT EXECUTE ON [dbo].[Indicador_SeleccionarValorLibor] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.Valores_Seleccionar.PRC' 
USE [SIT-FONDOS]
GO

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
--	Fecha Modificacion: 15/03/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Se adicionó los campos: DiasTTasaVariable, TipoTasaVariable, TasaVariable
----------------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 08/03/2019
--	Modificado por: Diego Tueros
--	Nro. Orden de Trabajo: 11883
--	Descripcion del cambio: Se adicionó el campo de RatingMandato
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
	,ISNULL(V.TipoTasaVariable,'') TipoTasaVariable
	,ISNULL(V.DiasTTasaVariable,0) DiasTTasaVariable
	,ISNULL(V.TasaVariable,0) TasaVariable
	,ISNULL(V.RatingMandato,'') RatingMandato
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




PRINT 'dbo.CuponeraNormal_ObtenerPorcentajeParticipacion.PRC'
USE [SIT-FONDOS]
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CuponeraNormal_ObtenerPorcentajeParticipacion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[CuponeraNormal_ObtenerPorcentajeParticipacion]
GO

------------------------------------------------------------------------------------------------------------
-- OBJETIVO: OBTENER EL PORCENTAJE DE PARTICIPACIÓN DEL CUPON ACTUAL
------------------------------------------------------------------------------------------------------------
--	Fecha Modificación: 18/03/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Creación
------------------------------------------------------------------------------------------------------------
--EXEC CuponeraNormal_ObtenerPorcentajeParticipacion 'CREAL 07/23',20181214,20190102,5,'0',1,0,0,'360',0,0
--EXEC CuponeraNormal_ObtenerPorcentajeParticipacion 'BDCA1BC1A',20180914,20181214,3,'0',1,0,0,'360',0,0
--EXEC CuponeraNormal_ObtenerPorcentajeParticipacion 'BDCA1BC1C',20190411,20190711,5,1,3034610.13,5.8695380,91,360,5.3907914,13.7473374
CREATE PROCEDURE [dbo].[CuponeraNormal_ObtenerPorcentajeParticipacion]
(
	@p_CodigoNemonico VARCHAR(15),
	@p_FechaIni DECIMAL(8,0),
	@p_FechaFin DECIMAL(8,0),
	@p_Consecutivo INT,
	@p_Estado BIT,
	@p_MontoNominalTotal NUMERIC(22,7),
	@p_TasaCupon NUMERIC(22,7),
	@p_DifDias NUMERIC(22,7),
	@p_BaseCupon INT,
	@p_Amortizac NUMERIC(22,7),
	@p_SumaMontoAmortizacion NUMERIC(22,7)
)AS
BEGIN 
	DECLARE @tmpTablaResultado TABLE (
									  Descripcion VARCHAR(100),
									  Principal NUMERIC(22,7),
									  Amortizacion NUMERIC(22,7),
									  Interes NUMERIC(22,7)
									 )
	IF @p_Estado = 0
	BEGIN
		INSERT INTO
			@tmpTablaResultado (
								Descripcion,
								Principal,
								Amortizacion,
								Interes
							   )				
		SELECT
			 PO.Descripcion, 
			 (ISNULL(SUM(CASE WHEN OI.CodigoOperacion = 1 THEN ISNULL(MontoNominalOrdenado,0) 
							 WHEN OI.CodigoOperacion = 2 THEN 0 - ISNULL(MontoNominalOrdenado,0) END),0)
			 + (ISNULL(ISNULL(SUM(ISNULL(CS.SaldoInicialUnidades,0)*VA.ValorUnitario),0)/COUNT(*),0)))
			 * (ISNULL(SUM(dbo.CalcularSumaAmortizacionPendiente((CASE WHEN @p_Consecutivo =1 THEN @p_FechaIni ELSE dbo.fn_SIT_gl_DiaHabilAnterior(dbo.RetornarFechaModificadaEnDias(@p_FechaIni,1)) END),@p_CodigoNemonico))/COUNT(*),0)/100) 
			 Principal,   
			 ISNULL(SUM(CASE WHEN OI.CodigoOperacion IN (38,39) THEN ISNULL(MontoOperacion,0) END),0) Amortizacion,
			 ISNULL(SUM(CASE WHEN OI.CodigoOperacion = 35 THEN ISNULL(MontoOperacion,0) END),0) Interes
		FROM
			OrdenInversion OI		
		JOIN
			Portafolio PO ON OI.CodigoPortafolioSBS = PO.CodigoPortafolioSBS
		JOIN
			Valores VA ON OI.CodigoMnemonico = VA.CodigoNemonico
		LEFT JOIN 
			CustodioSaldo CS ON OI.CodigoPortafolioSBS = CS.CodigoPortafolioSBS
								AND OI.CodigoMnemonico = CS.CodigoMnemonico
								AND CS.FechaSaldo = (CASE WHEN @p_Consecutivo =1 THEN @p_FechaIni ELSE dbo.fn_SIT_gl_DiaHabilAnterior(dbo.RetornarFechaModificadaEnDias(@p_FechaIni,1)) END)
		WHERE 
			OI.CodigoMnemonico=@p_CodigoNemonico 
			AND OI.FechaOperacion BETWEEN CASE WHEN @p_Consecutivo =1 THEN 1 ELSE @p_FechaIni+1 END AND @p_FechaFin 
			AND OI.Situacion='A' 
			AND OI.CodigoOperacion IN (1,2,35,38,39)
			AND VA.Situacion = 'A'
		GROUP BY
			PO.Descripcion	
	END	
	ELSE
	BEGIN
		DECLARE @tmpTablaNominalAcumulado TABLE (
												 codigoPortafolioSBS VARCHAR(10),
												 nominalAcumulado NUMERIC(22,7)
												)

		INSERT INTO 
			@tmpTablaNominalAcumulado (
										codigoPortafolioSBS,
										nominalAcumulado
									  )
		SELECT 
			OI.CodigoPortafolioSBS,
			ISNULL(SUM(CASE WHEN OI.CodigoOperacion = 1 THEN ISNULL(MontoNominalOrdenado,0)
							WHEN OI.CodigoOperacion = 2 THEN 0 - ISNULL(MontoNominalOrdenado,0) 
						--	WHEN OI.CodigoOperacion IN (38,39) THEN 0 - ISNULL(MontoOperacion,0) 
							END),0) 
		FROM 
			OrdenInversion OI 
		WHERE 
			OI.CodigoMnemonico=@p_CodigoNemonico 
			AND OI.FechaOperacion BETWEEN 1  AND @p_FechaFin 
			AND OI.Situacion='A' 
			AND OI.CodigoOperacion IN (1,2,38,39)
		GROUP BY OI.CodigoPortafolioSBS 

		UPDATE
			@tmpTablaNominalAcumulado
		SET 
			nominalAcumulado = nominalAcumulado*(dbo.CalcularSumaAmortizacionPendiente(PO.FechaNegocio,@p_CodigoNemonico))/100
		FROM 
			@tmpTablaNominalAcumulado TMP
		JOIN
			Portafolio PO ON TMP.codigoPortafolioSBS = PO.CodigoPortafolioSBS

		INSERT INTO
			@tmpTablaResultado (
								Descripcion,
								Principal,
								Amortizacion,
								Interes
							   )
		SELECT
			PO.Descripcion Portafolio,
			TMP.nominalAcumulado - (((@p_SumaMontoAmortizacion- (100 - dbo.CalcularSumaAmortizacionPendiente(PO.FechaNegocio,@p_CodigoNemonico))) /100)*TMP.nominalAcumulado) Principal,
			(TMP.nominalAcumulado*@p_Amortizac)/100 Amortizacion,
			dbo.fn_SIT_CalcularImporteCupon((TMP.nominalAcumulado-((@p_SumaMontoAmortizacion- (100 - dbo.CalcularSumaAmortizacionPendiente(PO.FechaNegocio,@p_CodigoNemonico))) /100)*TMP.nominalAcumulado),@p_TasaCupon,@p_DifDias,@p_BaseCupon) Interes
		FROM
			@tmpTablaNominalAcumulado TMP
		JOIN
			Portafolio PO ON TMP.codigoPortafolioSBS = PO.CodigoPortafolioSBS
		WHERE
			PO.Situacion = 'A'
		ORDER BY
			1		
	END	

	SELECT
		Descripcion,
		Principal,
		Amortizacion,
		Interes,
		ROUND((Principal/@p_MontoNominalTotal),7)*100 Participacion
	FROM 
		@tmpTablaResultado
	WHERE 
		Principal > 0

	  
END


GRANT EXECUTE ON [dbo].[CuponeraNormal_ObtenerPorcentajeParticipacion] TO [rol_sit_fondos] AS [dbo]
GO


PRINT 'dbo.CuponeraNormal_Ingresar.PRC'

USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CuponeraNormal_Ingresar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CuponeraNormal_Ingresar]
GO


----------------------------------------------------------------------------------------------------------------------
-- Autor:   David Bengoa
-- Fecha Creación: 10/09/2007
-- Descripción:  Registra una nueva cuponera normal
----------------------------------------------------------------------------------------------------------------------
-- Fecha Modificacion: 19/09/2017
-- Modificado por: Ian Pastor
-- Nro. Orden de Trabajo: 10784
-- Descripcion del cambio: Agregar el campo FechaPago al ingreso o modificación de un cupón
----------------------------------------------------------------------------------------------------------------------
-- Fecha Modificacion: 18/03/2019
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11851
-- Descripcion del cambio: Agregar nuevo campo de Tasa Variable.
----------------------------------------------------------------------------------------------------------------------
CREATE  PROCEDURE [dbo].[CuponeraNormal_Ingresar] (
	@p_CodNemonico VARCHAR(15),  
	@p_Secuencia  VARCHAR(10),  
	@p_fechaInicio  NUMERIC(8,0),  
	@p_fechaTermino NUMERIC(8,0),  
	@p_diferenciaDias  NUMERIC(10,0),  
	@p_amortizacion NUMERIC(22,7),  
	@p_tasaCupon  NUMERIC(22,7),  
	@p_tasaVariable  NUMERIC(22,7), 
	@p_base  NUMERIC(22,7),  
	@p_diasPago  NUMERIC(10,0),  
	@p_situacion  VARCHAR(1),  
	@p_montoNominal NUMERIC(22,7),  
	@p_UsuarioCreacion VARCHAR(15),  
	@p_FechaCreacion NUMERIC(8,0),  
	@p_HoraCreacion VARCHAR(10),  
	@p_Host  VARCHAR(20)  
)  
AS  
  
SET NOCOUNT ON  

--RGF 20090615 se agrego IF EXISTS xq estaba saliendo "Violation of PRIMARY KEY"
IF EXISTS (SELECT 1 
		   FROM CuponeraNormal 
		   WHERE CodigoNemonico = @p_CodNemonico
			 	 AND Secuencia = @p_Secuencia
				 AND FechaInicio = @p_fechaInicio)
	UPDATE 
		CuponeraNormal
	SET 
		FechaTermino = @p_fechaTermino,
		DiferenciaDias = @p_diferenciaDias,
		Amortizacion = @p_amortizacion,
		TasaCupon = @p_tasaCupon,
		TasaVariable = @p_tasaVariable,
		Base = @p_base,
		DiasPago = @p_diasPago,
		Situacion = @p_situacion,
		MontoNominalLocal = @p_montoNominal,
		FechaPago = @p_fechaTermino,
		UsuarioModificacion = @p_UsuarioCreacion,
		FechaModificacion = @p_FechaCreacion,
		HoraModificacion = @p_HoraCreacion,
		Host = @p_Host
	WHERE 
		CodigoNemonico = @p_CodNemonico
		AND Secuencia = @p_Secuencia
	 	AND FechaInicio = @p_fechaInicio
ELSE
	INSERT INTO 
		CuponeraNormal(
						CodigoNemonico,  
						Secuencia,  
						FechaInicio,  
						FechaTermino,  
						DiferenciaDias,   
						Amortizacion,  
						TasaCupon,
						TasaVariable, 
						Base,  
						DiasPago,  
						Situacion,
						MontoNominalLocal,
						FechaPago,  
						UsuarioCreacion,  
						FechaCreacion,  
						HoraCreacion,  
						Host  
				       ) 
	VALUES (  
			@p_CodNemonico,  
			@p_Secuencia,  
			@p_fechaInicio,  
			@p_fechaTermino,  
			@p_diferenciaDias,   
			@p_amortizacion,  
			@p_tasaCupon,  
			@p_tasaVariable,
			@p_base,  
			@p_diasPago,  
			@p_situacion, 
			@p_montoNominal,
			@p_fechaTermino,
			@p_UsuarioCreacion,  
			@p_FechaCreacion,  
			@p_HoraCreacion,  
			@p_Host  
	)


GO

GRANT EXECUTE ON [dbo].[CuponeraNormal_Ingresar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.MovimientoBancario_Extornar.PRC'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[MovimientoBancario_Extornar]    Script Date: 03/18/2019 14:40:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovimientoBancario_Extornar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MovimientoBancario_Extornar]
GO


---------------------------------------------------------------------------------------------------------------------  
--Objetivo: Actualizar el monto de las operaciones de caja y saldos bancarios  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 06/09/2017  
-- Creado por: Ian Pastor  
-- Nro. Orden de Trabajo: 10749  
-- Descripcion del cambio: Agregar la funcionalidad de actualización de los montos de las operaciones de caja y  
--                         saldos bancarios para los días posteriores.  
---------------------------------------------------------------------------------------------------------------------  
-- Fecha Creación: 18/03/2019  
-- Creado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11851  
-- Descripcion del cambio: Se cambia parametro codigooperacion de int a varchar.  
---------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[MovimientoBancario_Extornar]  
 @p_CodigoOperacionCaja VARCHAR(7),  
 @p_CodigoPortafolio VARCHAR(10),  
 @p_Usuario VARCHAR(20),  
 @p_Fecha numeric(8),  
 @p_Hora VARCHAR(10)  
AS  
BEGIN  
 DECLARE @TipoMovimiento VARCHAR(20), @NumeroOperacion VARCHAR(12), @Importe numeric(22,7), @Egreso VARCHAR(1), @NumeroCuenta VARCHAR(25),  
 @FechaOp numeric(8),@CodOperacion VARCHAR(6), @FechaPosterior NUMERIC(8), @CodigoMercado VARCHAR(5)  
   
 SELECT @TipoMovimiento = oc.TipoMovimiento,@Importe = oc.Importe,@Egreso = ti.Egreso, @NumeroCuenta = oc.NumeroCuenta, @FechaOp=oc.FechaOperacion,              
  @CodOperacion=oc.CodigoOperacion, @CodigoMercado = CodigoMercado  
 FROM dbo.OperacionesCaja oc                
  INNER JOIN Operacion o on o.CodigoOperacion = oc.CodigoOperacion                
  INNER JOIN TipoOperacion ti on ti.CodigoTipoOperacion = o.CodigoTipoOperacion                
 WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 --###ANALISIS###          
 UPDATE OperacionesCaja SET Situacion='I',NumeroCarta = NULL,EstadoCarta = NULL,UsuarioModificacion =@p_usuario + '_X',FechaModificacion =@p_Fecha,  
 HoraModificacion = @p_Hora  
 WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio  
  
 SELECT  @NumeroOperacion  = NumeroOperacion  
 FROM OperacionesCaja  
 WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 ---Se anula las operaciones de tengan el mismo numero de operacion (caso de las divisas)  Cambiado por LC 05082008                
 DELETE FROM OperacionesCaja WHERE NumeroOperacion = @NumeroOperacion AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 UPDATE ImpresionCartas SET Estado = '5' WHERE CodigoOperacionCaja = @NumeroOperacion AND Estado = '1'  
 DELETE FROM ImpresionCartas WHERE CodigoOperacionCaja = @NumeroOperacion AND CodigoCarta IS NULL  
 DELETE FROM ClaveFirmantesCarta WHERE CodigoOperacionCaja = @NumeroOperacion  
 IF @NumeroOperacion IS NOT NULL                
 BEGIN                
        -- Operaciones automaticas  
  UPDATE CuentasPorCobrarPagar SET Estado = '',FechaPago=Null,UsuarioModificacion = @p_usuario + '_X',FechaModificacion = @p_Fecha,  
  HoraModificacion = @p_Hora  
  WHERE NumeroOperacion = @NumeroOperacion AND CodigoPortafolioSBS = @p_CodigoPortafolio  
 END  
 IF (@CodOperacion not in ('63','64'))           
 BEGIN                 
  UPDATE SaldosBancarios  
  SET IngresosEstimados = case when (@Egreso = 'N') then (IngresosEstimados - @Importe) else IngresosEstimados end,  
  EgresosEstimados  = case when (@Egreso = 'S') then (EgresosEstimados - @Importe) else EgresosEstimados end,  
  UsuarioModificacion = @p_usuario + '_X',FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora  
  WHERE NumeroCuenta = @NumeroCuenta and FechaOperacion = @FechaOp  and CodigoPortafolioSBS = @p_CodigoPortafolio  
 END  
 IF (@CodOperacion  in ('63'))      
 BEGIN      
  UPDATE SaldosBancarios SET EgresosEstimados  = EgresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',  
  FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora  
  WHERE NumeroCuenta = @NumeroCuenta and FechaOperacion = @FechaOp and CodigoPortafolioSBS = @p_CodigoPortafolio  
 END  
 IF (@CodOperacion  in ('64'))        
 BEGIN      
  UPDATE SaldosBancarios SET IngresosEstimados = IngresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',  
  FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora    
  WHERE NumeroCuenta = @NumeroCuenta and FechaOperacion = @FechaOp and CodigoPortafolioSBS = @p_CodigoPortafolio         
 END  
 IF (@CodOperacion in ('63','64'))              
 BEGIN               
  DECLARE @NumeroCuentaDestino VARCHAR(25)              
  SELECT @NumeroCuentaDestino = oc.NumeroCuentaDestino FROM dbo.OperacionesCaja oc                
  INNER JOIN Operacion o on o.CodigoOperacion = oc.CodigoOperacion                
  INNER JOIN TipoOperacion ti on ti.CodigoTipoOperacion = o.CodigoTipoOperacion                
  WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja AND CodigoPortafolioSBS = @p_CodigoPortafolio               
  DECLARE @operacionParalela VARCHAR(10)    
  SET @operacionParalela = @p_CodigoOperacionCaja  
  SET @operacionParalela = REPLACE(@operacionParalela,'T','')  
  SET @operacionParalela = REPLACE(@operacionParalela,'N','')  
  IF( @CodOperacion= '63')  
  BEGIN  
   SELECT @operacionParalela=codigooperacioncaja FROM OperacionesCaja  WHERE              
   codigooperacioncaja like '%' + Cast(Cast(@operacionParalela as int)+1 as VARCHAR(5))   and FechaOperacion = @FechaOp              
   and codigooperacion = '64'  
     
   UPDATE SaldosBancarios SET IngresosEstimados = IngresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',  
   FechaModificacion = @p_Fecha,HoraModificacion = @p_Hora    
   WHERE NumeroCuenta = @NumeroCuentaDestino and FechaOperacion = @FechaOp          
  END  
  ELSE  
  BEGIN  
   UPDATE SaldosBancarios SET  EgresosEstimados  = EgresosEstimados - @Importe,UsuarioModificacion = @p_usuario + '_X',FechaModificacion = @p_Fecha,    
   HoraModificacion = @p_Hora    
   WHERE NumeroCuenta = @NumeroCuentaDestino AND FechaOperacion = @FechaOp  
   SELECT @operacionParalela=codigooperacioncaja FROM OperacionesCaja  WHERE  (codigooperacioncaja like '%'+ Cast(Cast(@operacionParalela as int)-1 as VARCHAR(5)))   and FechaOperacion = @FechaOp              
   and codigooperacion='63'
  END  
  --Se elimina la operacion complementaria             
  DELETE FROM OperacionesCaja WHERE codigooperacioncaja = @operacionParalela     
  UPDATE ImpresionCartas SET Estado = '5' WHERE CodigoOperacionCaja = @operacionParalela AND Estado = '1'  
  DELETE FROM ImpresionCartas WHERE CodigoOperacionCaja = @operacionParalela AND CodigoCarta IS NULL  
  DELETE FROM ClaveFirmantesCarta WHERE CodigoOperacionCaja = @operacionParalela   
 END  
 --Para las Comisiones Agentes de la ADMINISTRADORA del mercado extranjero  
 IF @CodOperacion = '67' and @p_CodigoPortafolio = 'ADMINISTRA' --###UPDATE###  
  DELETE FROM OperacionesCaja  
  WHERE CodigoOperacionCaja = @p_CodigoOperacionCaja and CodigoPortafolioSBS <> @p_CodigoPortafolio  
 --> Saldos de Custodio.    
 --La lógica para actualizar los saldos del custodio estan en GeneraSaldosCustodioPorNemonico  
 DECLARE @codigoorden VARCHAR(10),@codigotercero VARCHAR(12),@codigocustodio VARCHAR(12),@codigonemonico VARCHAR(15)  
 SELECT @codigoorden =codigoorden FROM CuentasPorCobrarPagar WHERE numerooperacion = @NumeroOperacion  
 SELECT @codigotercero= codigotercero,@codigonemonico =codigomnemonico FROM ordeninversion WHERE codigoorden =@codigoorden  
 SELECT @Codigocustodio = dbo.ObtenerCustodio(@p_CodigoPortafolio,@CodigoNemonico,@codigotercero)  
 /*  
 EXEC GeneraSaldosCustodioPorNemonico @FechaOp,@CodigoNemonico,@CodigoCustodio,@p_CodigoPortafolio,@p_usuario,@p_fecha,@p_Hora,''  
 --Actualiza los saldos bancarios de la operacion si esta se ha realizado en dias anteriores  
 EXEC sp_SIT_DismunuyeSaldoBancoDiasPosteriores @FechaOp,@p_CodigoOperacionCaja,@p_CodigoPortafolio   
 EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio, @NumeroCuenta, @FechaOp   
 */  
   
 --OT10749 - Inicio  
 --Actualizar los saldos bancarios del día  
 EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,@NumeroCuenta,@FechaOp  
   
 --Actualizar los saldos bancarios para el día posterior  
 SET @FechaPosterior = dbo.DiaHabilSiguiente(@FechaOp,@CodigoMercado)  
 IF EXISTS(SELECT 1 FROM SaldosBancarios WHERE CodigoPortafolioSBS = @p_CodigoPortafolio AND @FechaPosterior = @FechaPosterior AND NumeroCuenta = @NumeroCuenta)  
 BEGIN  
  EXEC sp_SIT_ActualizaSaldoBancoDiasPosteriores @NumeroCuenta, @p_CodigoPortafolio, @FechaOp,@FechaPosterior,@p_usuario,@p_fecha,@p_Hora,''  
  EXEC sp_SIT_gen_SaldosBancarios_Actualizar @p_CodigoPortafolio,@NumeroCuenta,@FechaPosterior  
 END  
 --OT10749 - Fin  
END  
GO

GRANT EXECUTE ON [dbo].[MovimientoBancario_Extornar] TO [rol_sit_fondos] AS [dbo]
GO

PRINT 'dbo.pr_SIT_gl_ValidacionApertura_TasaVariable.PRC'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pr_SIT_gl_ValidacionApertura_TasaVariable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pr_SIT_gl_ValidacionApertura_TasaVariable]
GO

--###COMENTARIO###Validar que la fecha de apertura tenga fechas y factores Libor  
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Validar que en la fecha de apertura las cuponeras con tasa variable tengan valor en tabla indicadores.
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 22/03/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11851    
-- Descripción del cambio: Creación de Store Procedure    
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificación: 01/04/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11851    
-- Descripción del cambio: Se implementa actualización de tasa variable en SWAP.
----------------------------------------------------------------------------------------------------------------------------------------  
--EXEC pr_SIT_gl_ValidacionApertura_TasaVariable 20190630
CREATE PROCEDURE [dbo].[pr_SIT_gl_ValidacionApertura_TasaVariable](     
	@decFechaApertura  DECIMAL(8,0)
)
AS    
BEGIN
	DECLARE @MENSAJE NVARCHAR(2000)    
	DECLARE @VALOR DECIMAL(18,7)      
	/* CREAMOS ESTA TABLA PARA RECUPERA LOS CUPONES POR FECHA DE APERTURA  */    
	DECLARE @CUPONES TABLE(
						   ROWID INT IDENTITY(1,1),    
						   CodigoNemonico VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS,    
						   FechaLibor NUMERIC(8,0),  
						   NombreIndicador VARCHAR(50),  
						   Secuencia VARCHAR(5),
						   TasaLibor NUMERIC(22,7),
						   TipoCupon VARCHAR(50),
						   CodigoOrden VARCHAR(12),
						   TasaCupon NUMERIC(22,7)
						   )    
	DECLARE  @MIN INT,
			 @MAX INT,
			 @NEMONICO VARCHAR(50),
			 @FECHA NUMERIC(8,0),
			 @TASA NUMERIC(22,7),
			 @SINLIBOR VARCHAR(500),
			 @TIPOCUPON VARCHAR(50),
			 @CODIGOORDEN VARCHAR(12),
			 @TASACUPON DECIMAL(22,7)

	SET @MENSAJE  = ''
	SET @MIN = 1
	SET @SINLIBOR = '' -- VARIABLE QUE GUARDARAS LOS NEMONICOS SIN FECHA LIBOR    

	DECLARE @Id INT,
			@IdMax INT,
			@CodigoIndicador VARCHAR(10),
			@NombreIndicador VARCHAR(50),
			@Secuencia VARCHAR(5),
			@fechaActual DECIMAL(8,0) = (CONVERT(CHAR(10),GETDATE(),112)),
			@horaActual VARCHAR(10) = (CONVERT(CHAR(10),GETDATE(),108))

	DECLARE @TmpIndicador TABLE(  
								Id INT IDENTITY,  
								CodigoIndicador VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS,
								NombreIndicador VARCHAR(50)
							   )

	INSERT INTO @TmpIndicador  
	SELECT 
		CodigoIndicador,
		NombreIndicador
	FROM 
		Indicador 
	WHERE 
		Situacion = 'A'  
		AND CodigoIndicador LIKE 'SW-%'

	SET @Id = 1  
	SELECT 
		@IdMax = Max(Id) 
	FROM 
		@TmpIndicador  
	
	WHILE @Id < @IdMax + 1  
	BEGIN
		SELECT 
			@CodigoIndicador = CodigoIndicador,
			@NombreIndicador = NombreIndicador
		FROM 
			@TmpIndicador  
		WHERE 
			Id = @Id  
		--La tasa libor se obtiene de valor indicador segun la fecha inicio de la cuponera, 
		--en caso no tenga libor se obtiene la tasa una fecha anterior habil  
		INSERT INTO 
			@CUPONES    
		SELECT 
			c.CodigoNemonico,
			ISNULL(dbo.RetornarFechaModificadaEnDias(c.FechaInicio,ISNULL(val.DiasTTasaVariable,0)),'00000000') FechaLibor,
			@NombreIndicador,
			c.Secuencia,
			TasaLibor =(CASE WHEN ISNULL(val.CuponTasaVariableReferencial,0) <> c.Secuencia THEN ISNULL((SELECT v.Valor
																						 -- (SELECT ISNULL(v2.Valor,0) 
																							--FROM ValorIndicador v2 
																							--WHERE v2.CodigoIndicador = @CodigoIndicador 
																							--	  AND v2.Situacion = 'A' 
																							--	  AND v2.Fecha = dbo.fn_SIT_gl_DiaHabilAnterior(dbo.RetornarFechaModificadaEnDias(c.FechaInicio,ISNULL(val.DiasTTasaVariable + 1,0))))
																								FROM ValorIndicador v 
																								WHERE v.CodigoIndicador = @CodigoIndicador 
																									AND v.Fecha = dbo.RetornarFechaModificadaEnDias(c.FechaInicio,ISNULL(val.DiasTTasaVariable,0))
																									AND v.Situacion = 'A'),0)
							 ELSE c.TasaVariable END),
			'CuponNormal',
			'0',
			val.TasaCupon
		FROM 
			CuponeraNormal c    
		JOIN 
			Valores val ON c.CodigoNemonico = val.CodigoNemonico 
						   AND val.TipoTasaVariable = @CodigoIndicador  
		WHERE 
			FechaTermino = @decFechaApertura  
			AND val.Situacion = 'A'

		UNION
		SELECT
			OI.CodigoMnemonico,
			ISNULL(CBS.FechaLiborOriginal,'00000000') FechaLibor,
			@NombreIndicador,
			CBS.Correlativo,
			TasaLibor =(CASE WHEN ISNULL(ODS.CuponTasaVariableReferencialLeg1,0) <> CBS.Correlativo THEN 
																								   ISNULL((SELECT v.Valor
																										   FROM ValorIndicador v 
																								           WHERE v.CodigoIndicador = @CodigoIndicador 
																									             AND v.Fecha = ISNULL(FechaLiborOriginal, dbo.RetornarFechaModificadaEnDias(CBS.FechaIniOriginal,ISNULL(ODS.DiaTLeg1 ,0)))
																									             AND v.Situacion = 'A'),0)			
						  ELSE CBS.TasaSpreadOriginal END),
			'LEG1',
			OI.CodigoOrden,
			0
		FROM	
			Cuponera_Bono_Swap CBS
		JOIN
			OrdenInversion_DetalleSWAP ODS ON CBS.CodigoOrden = ODS.CodigoOrden
		JOIN
			OrdenInversion OI ON ODS.CodigoOrden = OI.CodigoOrden
								 AND ODS.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		WHERE
			ODS.TasaFlotanteLeg1 = @CodigoIndicador
			AND CBS.FechaFinOriginal = @decFechaApertura
			AND OI.Situacion = 'A'	
			AND CBS.GeneradoInteresOriginal = 0

		UNION
		SELECT
			OI.CodigoMnemonico,
			ISNULL(CBS.FechaLibor,'00000000') FechaLibor,
			@NombreIndicador,
			CBS.Correlativo,
			TasaLibor =(CASE WHEN ISNULL(ODS.CuponTasaVariableReferencialLeg2,0) <> CBS.Correlativo THEN 
																							ISNULL((SELECT v.Valor
																									FROM ValorIndicador v 
																								    WHERE v.CodigoIndicador = @CodigoIndicador 
																									      AND v.Fecha = ISNULL(FechaLibor, dbo.RetornarFechaModificadaEnDias(CBS.FechaIni,ISNULL(ODS.DiaTLeg2,0)))
																									      AND v.Situacion = 'A'),0)	
							 ELSE CBS.TasaSpread END),
			'LEG2',
			OI.CodigoOrden,
			0
		FROM	
			Cuponera_Bono_Swap CBS
		JOIN
			OrdenInversion_DetalleSWAP ODS ON CBS.CodigoOrden = ODS.CodigoOrden
		JOIN
			OrdenInversion OI ON ODS.CodigoOrden = OI.CodigoOrden
								 AND ODS.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
		WHERE
			ODS.TasaFlotanteLeg2 = @CodigoIndicador
			AND CBS.FechaFin = @decFechaApertura	
			AND OI.Situacion = 'A'	
			AND CBS.GeneradoInteres = 0

		Set @Id = @Id + 1  
		CONTINUE  
	END

	SELECT 
		@MAX = Max(ROWID) 
	FROM 
		@CUPONES

	WHILE(@MIN < @MAX + 1)    
	BEGIN   
		SELECT 
			@NEMONICO = CodigoNemonico,
			@FECHA  = FechaLibor,
			@TASA  = ISNULL(TasaLibor,0),
			@NombreIndicador = NombreIndicador,
			@Secuencia = Secuencia,
			@TIPOCUPON  = TipoCupon,
			@CODIGOORDEN = CodigoOrden,
			@TASACUPON = TasaCupon
		FROM 
			@CUPONES    
		WHERE   
			ROWID = @MIN     
		
		IF ISNULL(@TASA,0) = 0  
		BEGIN
			If @MIN = 1  
			BEGIN  
				SET @SINLIBOR = @SINLIBOR + @NombreIndicador + ': ' + dbo.fn_SIT_gl_ConvertirFechaaString(@FECHA) + ' - ' + @NEMONICO  
			END
			ELSE 
			BEGIN
				SET @SINLIBOR = @SINLIBOR  + ',' + @NombreIndicador + ': ' + dbo.fn_SIT_gl_ConvertirFechaaString(@FECHA) + ' - ' + @NEMONICO  
			END
		END 
		ELSE
		BEGIN
			IF @NEMONICO = 'SWAP'
			BEGIN
				UPDATE
					Cuponera_Bono_Swap
				SET 
					TasaSpreadOriginal =(CASE WHEN @TIPOCUPON = 'LEG1' THEN @TASA ELSE TasaSpreadOriginal END),
					TasaSpread = (CASE WHEN @TIPOCUPON = 'LEG2' THEN @TASA ELSE TasaSpread END)
				FROM 
					Cuponera_Bono_Swap CBS
				JOIN
					OrdenInversion_DetalleSWAP ODS ON CBS.CodigoOrden = ODS.CodigoOrden
				JOIN
					OrdenInversion OI ON ODS.CodigoOrden = OI.CodigoOrden
									     AND ODS.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
				WHERE
					OI.CodigoMnemonico = @NEMONICO
					AND CBS.Correlativo >= CAST(@Secuencia AS INT)
					AND OI.Situacion = 'A'
					AND OI.CodigoOrden = @CODIGOORDEN

				UPDATE
					Cuponera_Bono_Swap
				SET 
					MontoInteresOriginal = (CASE WHEN @TIPOCUPON = 'LEG1' THEN  dbo.fn_SIT_CalcularImporteCupon(NominalRestanteOriginal,TasaCuponOriginal + TasaSpreadOriginal,DiasPagoOriginal,BaseCuponOriginal)
												 ELSE MontoInteresOriginal END),
					MontoInteres = (CASE WHEN @TIPOCUPON = 'LEG2' THEN  dbo.fn_SIT_CalcularImporteCupon(NominalRestante,TasaCupon + TasaSpread,DiasPago,BaseCupon)
												 ELSE MontoInteres END)
				FROM 
					Cuponera_Bono_Swap CBS
				JOIN
					OrdenInversion_DetalleSWAP ODS ON CBS.CodigoOrden = ODS.CodigoOrden
				JOIN
					OrdenInversion OI ON ODS.CodigoOrden = OI.CodigoOrden
									     AND ODS.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
				WHERE
					OI.CodigoMnemonico = @NEMONICO
					AND CBS.Correlativo >= CAST(@Secuencia AS INT)
					AND OI.Situacion = 'A'
					AND OI.CodigoOrden = @CODIGOORDEN

				UPDATE
					OrdenInversion_DetalleSWAP 
				SET
					TasaLiborLeg1 = (CASE WHEN @TIPOCUPON = 'LEG1' THEN @TASA ELSE TasaLiborLeg1 END),
					TasaLiborLeg2 = (CASE WHEN @TIPOCUPON = 'LEG2' THEN @TASA ELSE TasaLiborLeg2 END)
				FROM
					OrdenInversion_DetalleSWAP ODS
				JOIN
					OrdenInversion OI ON ODS.CodigoOrden = OI.CodigoOrden
									     AND ODS.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
				WHERE
					OI.CodigoMnemonico = @NEMONICO
					AND OI.Situacion = 'A'
					AND OI.CodigoOrden = @CODIGOORDEN
					
			END
			ELSE
			BEGIN
				UPDATE
					CuponeraNormal
				SET 
					TasaVariable = @TASA,
					TasaCupon = @TASACUPON + @TASA,
					FechaModificacion = @fechaActual,
					HoraModificacion = @horaActual
				WHERE
					CodigoNemonico = @NEMONICO
					AND CAST(Secuencia AS INT) >= CAST(@Secuencia AS INT)

				UPDATE 
					Valores
				SET 
					TasaVariable = @TASA
				WHERE 
					CodigoNemonico = @NEMONICO

			END
		END
		SET @MIN = @MIN + 1    
		CONTINUE
	END
	IF @SINLIBOR <> '' SET  @MENSAJE = @SINLIBOR    
	SELECT @MENSAJE    
END

GO

GRANT EXECUTE ON [dbo].[pr_SIT_gl_ValidacionApertura_TasaVariable] TO [rol_sit_fondos] AS [dbo]
GO




PRINT '[dbo].[sp_SIT_InsertaCuponera_Bono_Swap]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_InsertaCuponera_Bono_Swap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_InsertaCuponera_Bono_Swap]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Inserta la cuponera asociada a la negociación SWAP 
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 13/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando campos p_FechaLiborOriginal Y p_FechaLibor
---------------------------------------------------------------------------------------------------------------------------------------- 


CREATE PROCEDURE [dbo].[sp_SIT_InsertaCuponera_Bono_Swap]  
	@p_Correlativo INT,
	@p_CodigoOrden VARCHAR(20),  
	@p_FechaIniOriginal NUMERIC(8),  
	@p_FechaFinOriginal NUMERIC(8),  
	@p_DifDiasOriginal NUMERIC(8),  
	@p_AmortizacOriginal NUMERIC(22,7),  
	@p_TasaCuponOriginal NUMERIC(22,12),  
	@p_BaseCuponOriginal VARCHAR(4),  
	@p_DiasPagoOriginal VARCHAR(4),  
	@p_fechaRealInicialOriginal NUMERIC(8),  
	@p_fechaRealFinalOriginal  NUMERIC(8),  
	@p_AmortizacConsolidadoOriginal NUMERIC(22,7),  
	@p_MontoInteresOriginal NUMERIC(22,7),  
	@p_MontoAmortizacionOriginal NUMERIC(22,7),  
	@p_NominalRestanteOriginal NUMERIC(22,7),  
	@p_TasaSpreadOriginal NUMERIC(22,7),
	@p_FechaIni NUMERIC(8),
	@p_FechaFin NUMERIC(8),
	@p_DifDias NUMERIC(8),
	@p_Amortizac NUMERIC(22,7), 
	@p_TasaCupon NUMERIC(22,12),
	@p_BaseCupon VARCHAR(4), 
	@p_DiasPago VARCHAR(4), 
	@p_fechaRealInicial NUMERIC(8),
	@p_fechaRealFinal NUMERIC(8),
	@p_AmortizacConsolidado NUMERIC(22,7), 
	@p_MontoInteres NUMERIC(22,7), 
	@p_MontoAmortizacion NUMERIC(22,7), 
	@p_NominalRestante NUMERIC(22,7),
	@p_TasaSpread NUMERIC (22,7),
	@p_FechaLiborOriginal NUMERIC(8) =NULL,
	@p_FechaLibor NUMERIC(8) =NULL
AS  
BEGIN  
	INSERT INTO Cuponera_Bono_Swap (
								   [Correlativo]
								  ,[CodigoOrden]
								  ,[FechaIniOriginal]
								  ,[FechaFinOriginal]
								  ,[DifDiasOriginal]
								  ,[AmortizacOriginal]
								  ,[TasaCuponOriginal]
								  ,[BaseCuponOriginal]
								  ,[DiasPagoOriginal]
								  ,[fechaRealInicialOriginal]
								  ,[fechaRealFinalOriginal]
								  ,[AmortizacConsolidadoOriginal]
								  ,[MontoInteresOriginal]
								  ,[MontoAmortizacionOriginal]
								  ,[NominalRestanteOriginal]
								  ,[TasaSpreadOriginal]
								  ,[FechaIni]
								  ,[FechaFin]
								  ,[DifDias]
								  ,[Amortizac]
								  ,[TasaCupon]
								  ,[BaseCupon]
								  ,[DiasPago]
								  ,[fechaRealInicial]
								  ,[fechaRealFinal]
								  ,[AmortizacConsolidado]
								  ,[MontoInteres]
								  ,[MontoAmortizacion]
								  ,[NominalRestante]
								  ,[TasaSpread]
								  ,[GeneradoInteres]
								  ,[GeneradoAmortizar]
								  ,[GeneradoInteresOriginal]
								  ,[GeneradoAmortizarOriginal]
								  ,[FechaLiborOriginal]
								  ,[FechaLibor]
								  )
		VALUES (
				@p_Correlativo,
				@p_CodigoOrden,  
				@p_FechaIniOriginal,  
				@p_FechaFinOriginal,  
				@p_DifDiasOriginal,  
				@p_AmortizacOriginal,  
				@p_TasaCuponOriginal,  
				@p_BaseCuponOriginal,  
				@p_DiasPagoOriginal,  
				@p_fechaRealInicialOriginal,  
				@p_fechaRealFinalOriginal,  
				@p_AmortizacConsolidadoOriginal,  
				@p_MontoInteresOriginal,  
				@p_MontoAmortizacionOriginal,  
				@p_NominalRestanteOriginal,  
				@p_TasaSpreadOriginal,
				@p_FechaIni,
				@p_FechaFin,
				@p_DifDias,
				@p_Amortizac, 
				@p_TasaCupon,
				@p_BaseCupon, 
				@p_DiasPago, 
				@p_fechaRealInicial,
				@p_fechaRealFinal,
				@p_AmortizacConsolidado, 
				@p_MontoInteres, 
				@p_MontoAmortizacion, 
				@p_NominalRestante,
				@p_TasaSpread,
				'0',
				'0',
				'0',
				'0',
				@p_FechaLiborOriginal,
				@p_FechaLibor
				)
END    

GRANT EXECUTE ON [dbo].[sp_SIT_InsertaCuponera_Bono_Swap] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[sp_SIT_ConsultaOrdenSwapBono]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ConsultaOrdenSwapBono]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ConsultaOrdenSwapBono]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Obtiene la línea y detalle de negociación SWAP 
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 13/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando campos DiaT y tasa para Leg1 y Leg2
---------------------------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [dbo].[sp_SIT_ConsultaOrdenSwapBono]
	@p_CodigoPortafolio VARCHAR(12),
	@p_FechaOperacion NUMERIC(8),
	@p_CodigoOrden  VARCHAR(10)
AS
BEGIN
	SELECT 
		OI.Estado, 
		OI.CodigoOrden,
		P.CodigoPortafolioSBS,
		P.Descripcion 'Portafolio', 
		OI.TasaPorcentaje Tasa, 
		M.Descripcion Moneda, 
		MontoNominalOperacion,
		MontoNetoOperacion,
		CodigoTipoCupon, 
		ISNULL(CodigoISIN,'') CodigoISIN , 
		FechaOperacion,
		OI.FechaTrato,
		OI.FechaContrato,
		ISNULL(OI.CodigoMotivo,'') CodigoMotivo,
		ISNULL(OI.CodigoPlaza,'') CodigoPlaza,
		OI.Plazo,
		OI.TipoCambio,
		OI.TipoCobertura,
		OI.CodigoMoneda,
		OI.TipoFondo, 
		OI.TasaCastigo,
		OI.GrupoIntermediario,
		OI.CodigoTercero
	FROM
		OrdenInversion OI
	JOIN 
		Moneda M ON M.CodigoMoneda = OI.CodigoMoneda
	JOIN 
		Portafolio P ON P.CodigoPortafolioSBS =  OI.CodigoPortafolioSBS 
	WHERE 
		OI.CodigoOperacion = '21' 
		AND CategoriaInstrumento = 'BS'
		AND OI.CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN OI.CodigoPortafolioSBS ELSE @p_CodigoPortafolio END
		AND OI.FechaOperacion =  CASE WHEN @p_FechaOperacion = 0 THEN OI.FechaOperacion  ELSE @p_FechaOperacion END
		AND CodigoOrden = CASE WHEN @p_CodigoOrden = '' THEN OI.CodigoOrden  ELSE @p_CodigoOrden END 
		AND OI.Situacion = 'A'
	
	SELECT 
		 [CodigoOrden]
		,[CodigoPortafolioSBS]
		,[FechaIniLeg1]
		,[FechaFinLeg1]
		,[FechaIniLeg2]
		,[FechaFinLeg2]
		,[CodigoMonedaLeg1]
		,[CodigoMonedaLeg2]
		,[TipoCambioSpot]
		,[TasaInteresLeg1]
		,[TasaFlotanteLeg1]
		,[TasaInteresLeg2]
		,[TasaFlotanteLeg2]
		,[PeriodicidadLeg1]
		,[PeriodicidadLeg2]
		,[AmortizacionLeg1]
		,[AmortizacionLeg2]
		,[BaseDiasLeg1]
		,[BaseAniosLeg1]
		,[BaseDiasLeg2]
		,[BaseAniosLeg2]
		,[DiaTLeg1]
		,[DiaTLeg2]
		,ISNULL([TasaLiborLeg1],0) as TasaLiborLeg1
		,ISNULL([TasaLiborLeg2],0) as TasaLiborLeg2
	FROM 
		OrdenInversion_DetalleSWAP
	WHERE 
		CodigoPortafolioSBS = CASE WHEN @p_CodigoPortafolio = '' THEN CodigoPortafolioSBS ELSE @p_CodigoPortafolio END
		AND CodigoOrden = CASE WHEN @p_CodigoOrden = '' THEN CodigoOrden  ELSE @p_CodigoOrden END 
END




GRANT EXECUTE ON [dbo].[sp_SIT_ConsultaOrdenSwapBono] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[sp_SIT_ConsultaCuponera_Bono_Swap]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ConsultaCuponera_Bono_Swap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ConsultaCuponera_Bono_Swap]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Obtiene los bonos negociados en OI SWAP
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 13/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando campos p_FechaLiborOriginal Y p_FechaLibor
----------------------------------------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_ConsultaCuponera_Bono_Swap]  
	@p_CodigoOrden VARCHAR(20),
	@p_OrdenGenera VARCHAR(20)  
AS  
BEGIN  

	 IF ISNULL(@p_CodigoOrden,'') = ''
	 BEGIN
		SET @p_CodigoOrden = (SELECT 
								  OrdenGenera 
							  FROM 
								  OrdenInversion 
							  WHERE 
								  CodigoOrden = @p_OrdenGenera)
	 END
	 select 
	 *
	 ,(CASE WHEN FechaLiborOriginalDecimal IS NULL THEN T.FechaIniOriginal ELSE  dbo.FN_SIT_OBT_FechaFormateada(FechaLiborOriginalDecimal) END ) FechaLiborOriginal
	 ,(CASE WHEN FechaLiborDecimal  IS NULL THEN T.FechaIni ELSE  dbo.FN_SIT_OBT_FechaFormateada(FechaLiborDecimal) END ) FechaLibor
	 
	 from (
	 SELECT 
	   [Correlativo] Consecutivo
      ,[CodigoOrden]
	  ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaIniOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaIniOriginal) END) FechaIniOriginal
	  ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaFinOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaFinOriginal) END) FechaFinOriginal
      ,ISNULL([DifDiasOriginal],0) DifDiasOriginal
      ,ISNULL([AmortizacOriginal],0) AmortizacOriginal
      ,ISNULL([TasaCuponOriginal],0) TasaCuponOriginal
      ,ISNULL([BaseCuponOriginal],0) BaseCuponOriginal
      ,ISNULL([DiasPagoOriginal],0) DiasPagoOriginal
	  ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealInicialOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealInicialOriginal) END)  fechaRealInicialOriginal
	  ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealFinalOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealFinalOriginal) END)  fechaRealFinalOriginal
      ,ISNULL([AmortizacConsolidadoOriginal],0) AmortizacConsolidadoOriginal
      ,ISNULL([MontoInteresOriginal],0) MontoInteresOriginal
      ,ISNULL([MontoAmortizacionOriginal],0) MontoAmortizacionOriginal
      ,ISNULL([NominalRestanteOriginal],0) NominalRestanteOriginal
      ,ISNULL([TasaSpreadOriginal],0) TasaSpreadOriginal
      ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaIni,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaIni) END) FechaIni
	  ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaFin,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaFin) END) FechaFin
      ,ISNULL([DifDias],0) DifDias
      ,ISNULL([Amortizac],0) Amortizac
      ,ISNULL([TasaCupon],0) TasaCupon
      ,ISNULL([BaseCupon],'') BaseCupon
      ,ISNULL([DiasPago],'') DiasPago
      ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealInicial,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealInicial) END) fechaRealInicial
	  ,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealFinal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealFinal) END) fechaRealFinal
      ,ISNULL([AmortizacConsolidado],0) AmortizacConsolidado
      ,ISNULL([MontoInteres],0) MontoInteres
      ,ISNULL([MontoAmortizacion],0) MontoAmortizacion
      ,ISNULL([NominalRestante],0) NominalRestante
      ,ISNULL([TasaSpread],0) TasaSpread
	  ,TotalFlujoOriginal = ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)
	  ,TotalFlujo = ISNULL(MontoAmortizacion,0) + ISNULL(MontoInteres,0)
	  ,TCFlujo = (ISNULL(MontoAmortizacion,0) + ISNULL(MontoInteres,0)) /(CASE WHEN (ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)) = 0 
																			THEN 1 ELSE (ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)) END)
	  ,FechaLiborOriginal as FechaLiborOriginalDecimal
	  ,FechaLibor as FechaLiborDecimal
 FROM Cuponera_Bono_Swap ) t WHERE t.CodigoOrden =  @p_CodigoOrden 
END  

GRANT EXECUTE ON [dbo].[sp_SIT_ConsultaCuponera_Bono_Swap] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[OrdenInversion_ModificarOI_DetalleSWAP] '
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_ModificarOI_DetalleSWAP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrdenInversion_ModificarOI_DetalleSWAP]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Modifica las OI cuando en el proceso de generación de OI SWAP  
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 13/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando campos DiaT y tasa para Leg1 y Leg2
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[OrdenInversion_ModificarOI_DetalleSWAP] 
	@p_CodigoOrden VARCHAR(20), 
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_FechaIniLeg1 NUMERIC(8),
	@p_FechaFinLeg1 NUMERIC(8),
	@p_FechaIniLeg2 NUMERIC(8),
	@p_FechaFinLeg2 NUMERIC(8),
	@p_CodigoMonedaLeg1 VARCHAR(30),
	@p_CodigoMonedaLeg2 VARCHAR(30),
	@p_TipoCambioSpot NUMERIC(22,7),
	@p_TasaInteresLeg1 NUMERIC(22,12),
	@p_TasaFlotanteLeg1 VARCHAR(30),
	@p_TasaInteresLeg2 NUMERIC(22,12),
	@p_TasaFlotanteLeg2 VARCHAR(30),
	@p_PeriodicidadLeg1 VARCHAR(4),
	@p_PeriodicidadLeg2 VARCHAR(4),
	@p_AmortizacionLeg1 VARCHAR(4),
	@p_AmortizacionLeg2 VARCHAR(4),
	@p_BaseDiasLeg1 VARCHAR(4),
	@p_BaseAniosLeg1 VARCHAR(4),
	@p_BaseDiasLeg2 VARCHAR(4),
	@p_BaseAniosLeg2 VARCHAR(4),
	@p_DiaTLeg1 INT=NULL,
	@p_DiaTLeg2 INT=NULL,
	@p_TasaLiborLeg1 NUMERIC(22,7),
	@p_TasaLiborLeg2 NUMERIC(22,7),
	@p_CuponTasaVariableReferencialLeg1 INT,
	@p_CuponTasaVariableReferencialLeg2 INT
AS
BEGIN

	UPDATE
		OrdenInversion_DetalleSWAP
	SET
	   [FechaIniLeg1] = @p_FechaIniLeg1,
	   [FechaFinLeg1] = @p_FechaFinLeg1,
	   [FechaIniLeg2] = @p_FechaIniLeg2,
	   [FechaFinLeg2] = @p_FechaFinLeg2,
	   [CodigoMonedaLeg1] = @p_CodigoMonedaLeg1,
	   [CodigoMonedaLeg2] = @p_CodigoMonedaLeg2,
	   [TipoCambioSpot] = @p_TipoCambioSpot,
	   [TasaInteresLeg1] = @p_TasaInteresLeg1,
	   [TasaFlotanteLeg1] = @p_TasaFlotanteLeg1,
	   [TasaInteresLeg2] = @p_TasaInteresLeg2,
	   [TasaFlotanteLeg2] = @p_TasaFlotanteLeg2,
	   [PeriodicidadLeg1] = @p_PeriodicidadLeg1,
	   [PeriodicidadLeg2] = @p_PeriodicidadLeg2,
	   [AmortizacionLeg1] = @p_AmortizacionLeg1,
	   [AmortizacionLeg2] = @p_AmortizacionLeg2,
	   [BaseDiasLeg1] = @p_BaseDiasLeg1,
	   [BaseAniosLeg1] = @p_BaseAniosLeg1,
	   [BaseDiasLeg2] = @p_BaseDiasLeg2,
	   [BaseAniosLeg2] = @p_BaseAniosLeg2,
	   [DiaTLeg1] = @p_DiaTLeg1,
	   [DiaTLeg2] = @p_DiaTLeg2,
	   [TasaLiborLeg1] = @p_TasaLiborLeg1,
	   [TasaLiborLeg2] = @p_TasaLiborLeg2,
	   [CuponTasaVariableReferencialLeg1] = @p_CuponTasaVariableReferencialLeg1,
	   [CuponTasaVariableReferencialLeg2] = @p_CuponTasaVariableReferencialLeg2
	WHERE
		[CodigoOrden] = @p_CodigoOrden 
		AND [CodigoPortafolioSBS] = @p_CodigoPortafolioSBS
END


GRANT EXECUTE ON [dbo].[OrdenInversion_ModificarOI_DetalleSWAP] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[OrdenInversion_InsertarOI_DetalleSWAP]'
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_InsertarOI_DetalleSWAP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrdenInversion_InsertarOI_DetalleSWAP]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Inserta las OI en el proceso de generación de OI SWAP  
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 13/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando campos DiaT , tasa y CuponTasaVariableReferencial para Leg1 y Leg2
---------------------------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [dbo].[OrdenInversion_InsertarOI_DetalleSWAP] 
	@p_CodigoOrden VARCHAR(20), 
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_FechaIniLeg1 NUMERIC(8),
	@p_FechaFinLeg1 NUMERIC(8),
	@p_FechaIniLeg2 NUMERIC(8),
	@p_FechaFinLeg2 NUMERIC(8),
	@p_CodigoMonedaLeg1 VARCHAR(30),
	@p_CodigoMonedaLeg2 VARCHAR(30),
	@p_TipoCambioSpot NUMERIC(22,7),
	@p_TasaInteresLeg1 NUMERIC(22,12),
	@p_TasaFlotanteLeg1 VARCHAR(30),
	@p_TasaInteresLeg2 NUMERIC(22,12),
	@p_TasaFlotanteLeg2 VARCHAR(30),
	@p_PeriodicidadLeg1 VARCHAR(4),
	@p_PeriodicidadLeg2 VARCHAR(4),
	@p_AmortizacionLeg1 VARCHAR(4),
	@p_AmortizacionLeg2 VARCHAR(4),
	@p_BaseDiasLeg1 VARCHAR(4),
	@p_BaseAniosLeg1 VARCHAR(4),
	@p_BaseDiasLeg2 VARCHAR(4),
	@p_BaseAniosLeg2 VARCHAR(4),
	@p_DiaTLeg1 INT=NULL,
	@p_DiaTLeg2 INT=NULL,
	@p_TasaLiborLeg1 NUMERIC(22,7),
	@p_TasaLiborLeg2 NUMERIC(22,7),
	@p_CuponTasaVariableReferencialLeg1 INT,
	@p_CuponTasaVariableReferencialLeg2 INT
AS
BEGIN
	INSERT INTO 
		OrdenInversion_DetalleSWAP
								(
								   [CodigoOrden]
								  ,[CodigoPortafolioSBS]
								  ,[FechaIniLeg1]
								  ,[FechaFinLeg1]
								  ,[FechaIniLeg2]
								  ,[FechaFinLeg2]
								  ,[CodigoMonedaLeg1]
								  ,[CodigoMonedaLeg2]
								  ,[TipoCambioSpot]
								  ,[TasaInteresLeg1]
								  ,[TasaFlotanteLeg1]
								  ,[TasaInteresLeg2]
								  ,[TasaFlotanteLeg2]
								  ,[PeriodicidadLeg1]
								  ,[PeriodicidadLeg2]
								  ,[AmortizacionLeg1]
								  ,[AmortizacionLeg2]
								  ,[BaseDiasLeg1]
								  ,[BaseAniosLeg1]
								  ,[BaseDiasLeg2]
								  ,[BaseAniosLeg2]
								  ,[DiaTLeg1]
								  ,[DiaTLeg2]
								  ,[TasaLiborLeg1]
								  ,[TasaLiborLeg2]
								  ,[CuponTasaVariableReferencialLeg1]
								  ,[CuponTasaVariableReferencialLeg2]
								 ) 
		VALUES
			(
			 @p_CodigoOrden 
			,@p_CodigoPortafolioSBS
			,@p_FechaIniLeg1 
			,@p_FechaFinLeg1
			,@p_FechaIniLeg2
			,@p_FechaFinLeg2
			,@p_CodigoMonedaLeg1
			,@p_CodigoMonedaLeg2
			,@p_TipoCambioSpot
			,@p_TasaInteresLeg1
			,@p_TasaFlotanteLeg1
			,@p_TasaInteresLeg2
			,@p_TasaFlotanteLeg2
			,@p_PeriodicidadLeg1
			,@p_PeriodicidadLeg2
			,@p_AmortizacionLeg1
			,@p_AmortizacionLeg2
			,@p_BaseDiasLeg1
			,@p_BaseAniosLeg1
			,@p_BaseDiasLeg2
			,@p_BaseAniosLeg2			
			,@p_DiaTLeg1
			,@p_DiaTLeg2
			,@p_TasaLiborLeg1
			,@p_TasaLiborLeg2
			,@p_CuponTasaVariableReferencialLeg1
			,@p_CuponTasaVariableReferencialLeg2
		 )
END

GRANT EXECUTE ON [dbo].[OrdenInversion_InsertarOI_DetalleSWAP] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[sp_SIT_ObtenerCuponera] '
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ObtenerCuponera]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ObtenerCuponera]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Generar Cuponera SWAP opción automático  
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 26/11/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando para el codigo de portafolio, para saber su fecha de negocio
----------------------------------------------------------------------------------------------------------------------------------------   
CREATE PROCEDURE [dbo].[sp_SIT_ObtenerCuponera](
	@p_CodigoTipoAmortizacionOriginal VARCHAR(4),
	@p_fechaEmisionOriginal NUMERIC(8),
	@p_fechaVctoOriginal NUMERIC(8),
	@p_fechaPriCuponOriginal NUMERIC(8),
	@p_CodigoPeriodicidadOriginal VARCHAR(4),
	@p_tasaCuponOriginal NUMERIC(22,7),
	@p_baseCuponOriginal VARCHAR(4),
	@p_tasaSpreadOriginal NUMERIC(15,12),
	@p_numeroDiasOriginal VARCHAR(4),
	@p_MontoNominalOriginal NUMERIC(22,7),
	
	@p_CadenaNemonico VARCHAR(400),
	@p_ImporteVentaTotal NUMERIC(22,7),
	@p_tipoCuponera VARCHAR(2),
	
	@p_CodigoTipoAmortizacion VARCHAR(4),
	@p_fechaEmision NUMERIC(8),
	@p_fechaVcto NUMERIC(8),
	@p_fechaPriCupon NUMERIC(8),
	@p_CodigoPeriodicidad VARCHAR(4),
	@p_tasaCupon NUMERIC(22,7),
	@p_baseCupon VARCHAR(4),
	@p_tasaSpread NUMERIC(15,12),
	@p_numeroDias VARCHAR(4),
	@p_MontoNominal NUMERIC(22,7),	
	@p_FechaOperacion NUMERIC(8),
	@p_DiaTOriginal NUMERIC(8) =0,
	@p_DiaT NUMERIC(8) =0,
	@p_PortafolioSBS VARCHAR(10)=''

)          
AS          
BEGIN 
	DECLARE	
		@p_AmortizacionAux NUMERIC(22,15),
		@p_Amortizacion NUMERIC(22,15),
		@p_fechaFinAmortizacion DECIMAL(8),
		@p_TotalAmortizacion NUMERIC(22,15),
		@iCount INT, 
		@DiasPeriodo NUMERIC(3),
		@TotalConsecutivoOriginal INT,
		@TotalConsecutivo INT

	SET @DiasPeriodo = 0
	
	SELECT 
		@DiasPeriodo = DiasPeriodo 
	FROM 
		Periodicidad 
	WHERE 
		CodigoPeriodicidad = @p_CodigoPeriodicidad
		
	CREATE TABLE 
		#tmpCuponeraSWAP
				(
				 Consecutivo INT,							---- Datos Cuponera LEG1 
				 FechaIniOriginal NUMERIC(8),
				 FechaFinOriginal NUMERIC(8),
				 DifDiasOriginal NUMERIC(8),
				 AmortizacOriginal NUMERIC(22,7),
				 TasaCuponOriginal NUMERIC(22,12),
				 BaseCuponOriginal VARCHAR(4),
				 DiasPagoOriginal VARCHAR(4),
				 fechaRealInicialOriginal NUMERIC(8),
				 fechaRealFinalOriginal NUMERIC(8) ,
				 AmortizacConsolidadoOriginal NUMERIC(22,7),
				 MontoInteresOriginal NUMERIC(22,7),
				 MontoAmortizacionOriginal NUMERIC(22,7),
				 NominalRestanteOriginal NUMERIC(22,7),
				 FechaIni NUMERIC(8),                        ---- Datos Cuponera LEG2 
				 FechaFin NUMERIC(8),
				 DifDias NUMERIC(8),
				 Amortizac NUMERIC(22,7),
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

	DECLARE @tmpCuponeraSWAPLEG1 UT_CuponeraTemporal,
			@tmpCuponeraSWAPLEG2 UT_CuponeraTemporal,
			@tmpCuponeraSWAPProcesada UT_CuponeraTemporal
				
	
	--- >> Generación de Fechas Cuponera LEG1
	INSERT INTO @tmpCuponeraSWAPLEG1(
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
									)
	EXEC [sp_SIT_GenerarCuponera_Fechas] '0',
									     @p_fechaEmisionOriginal,
										 @p_fechaVctoOriginal,
									     @p_fechaPriCuponOriginal,
									     @p_tasaCuponOriginal,
									     @p_baseCuponOriginal,
									     @p_tasaSpreadOriginal,
									     @p_CodigoPeriodicidadOriginal,
									     @p_numeroDiasOriginal
									   
	--- >> Generación de Fechas Cuponera LEG2
	INSERT INTO @tmpCuponeraSWAPLEG2(
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
									 )
	EXEC [sp_SIT_GenerarCuponera_Fechas] '0',
										 @p_fechaEmision,
										 @p_fechaVcto,
									     @p_fechaPriCupon,
									     @p_tasaCupon,
									     @p_baseCupon,
									     @p_tasaSpread,
									     @p_CodigoPeriodicidad,
									     @p_numeroDias
	

	
	---->> OBTENCION DE CUPON ACTUAL

	DECLARE @CorrelativoActualLeg1 INT = 0
	DECLARE @CorrelativoActualLeg2 INT = 0

	-- Obteniendo Fecha Negocio
	DECLARE @FechaNegocio NUMERIC(8,0) = (SELECT FechaNegocio 
												FROM Portafolio PO
												WHERE PO.CodigoPortafolioSBS = @P_PortafolioSBS AND PO.Situacion='A')

	DECLARE @FechaNegocioLeg1 NUMERIC(8,0)=@FechaNegocio				
	DECLARE @FechaNegocioLeg2 NUMERIC(8,0)=@FechaNegocio

	--CUPON ACTUAL, PASO EL CODIGO
			IF EXISTS(SELECT COUNT(1) 
						  FROM @tmpCuponeraSWAPLEG1			
						  WHERE FechaIni = @FechaNegocio)
					BEGIN
						SET @FechaNegocioLeg1 = dbo.RetornarFechaModificadaEnDias(@FechaNegocio,-1)
					END
				
			IF EXISTS(SELECT COUNT(1) 
						  FROM @tmpCuponeraSWAPLEG2			
						  WHERE FechaIni = @FechaNegocio)
					BEGIN
						SET @FechaNegocioLeg2 = dbo.RetornarFechaModificadaEnDias(@FechaNegocio,-1)
					END				

		---CORRELATIVO Leg 1
			set @CorrelativoActualLeg1 = (SELECT ISNULL(Consecutivo,0) 
			FROM @tmpCuponeraSWAPLEG1			
			WHERE  @FechaNegocioLeg1 BETWEEN FechaIni AND FechaFin)

		---CORRELATIVO Leg 2				
			set @CorrelativoActualLeg2 = (SELECT ISNULL(Consecutivo,0)  
			FROM @tmpCuponeraSWAPLEG2			
			WHERE  @FechaNegocioLeg2 BETWEEN FechaIni AND FechaFin)		
							
PRINT ('@CorrelativoActualLeg1 ' + CONVERT(VARCHAR(10),@CorrelativoActualLeg1) )
PRINT ('@CorrelativoActualLeg2 ' + CONVERT(VARCHAR(10),@CorrelativoActualLeg2))
		
	--- >> OBTENER CUPONERA DE INSTRUMENTOS NEGOCIADOS EN SWAP
	IF @p_tipoCuponera = '1' 
	BEGIN
		DECLARE @NemonicoCuponera AS TABLE(
											Consecutivo INT IDENTITY(1,1), 
											CodigoNemonico VARCHAR(15)
										  )
		DECLARE @InstrumentosCuponeraNormal AS TABLE (
														FechaInicio NUMERIC(8),
														FechaTermino NUMERIC(8),
														DifDias INT,
														Amortizac NUMERIC(22,7),
														TasaCupon NUMERIC(22,12),
														BaseCupon NUMERIC(22,7),
														DiasPago INT,
														consecutivo VARCHAR(5),
														AmortizacConsolidado NUMERIC(22,7) 
													 )
		DECLARE 
			@name NVARCHAR(255),
			@pos INT,
			@cont INT = 0,
			@NumReg INT,
			@SumaAmortizacion NUMERIC(22,7),
			@MontoAmortizacionOriginal  NUMERIC(22,7),
			@Amort NUMERIC(22,7), 
			@MontoAmortizacion  NUMERIC(22,7),
			@SumaMontoAmortizacion NUMERIC(22,7), 
			@SumaMontoAmortizacionOriginal NUMERIC(22,7),
			@SumaAmorticAcumulado NUMERIC(22,7),
			@ImporteVenta NUMERIC(22,7),
			@DifdiasFechaOperacion NUMERIC(22,7),
			@consecutivo INT
		
		WHILE CHARINDEX(',', @p_CadenaNemonico) > 0
		BEGIN
			SET @pos  = CHARINDEX(',', @p_CadenaNemonico)  
			SET @name = SUBSTRING(@p_CadenaNemonico, 1, @pos-1)

			INSERT INTO 
				@NemonicoCuponera 
			SELECT 
				@name
				
			SET	@p_CadenaNemonico = SUBSTRING(@p_CadenaNemonico, @pos+1, LEN(@p_CadenaNemonico))
		END
			
		SET @NumReg = (SELECT COUNT(1) FROM @NemonicoCuponera)
		SET @iCount = (SELECT COUNT(1) FROM @tmpCuponeraSWAPLEG1)
		
		WHILE (@cont < @NumReg)
		BEGIN
			SET @cont = @cont + 1
			SELECT @p_CadenaNemonico = CodigoNemonico FROM @NemonicoCuponera WHERE Consecutivo = @cont
			
			DELETE FROM @InstrumentosCuponeraNormal
			
			INSERT INTO @InstrumentosCuponeraNormal
			EXEC [CuponeraNormal_Leer] @p_CadenaNemonico
			
			DELETE FROM @InstrumentosCuponeraNormal
			WHERE FechaTermino < @p_fechaEmisionOriginal
			
			
			SET @consecutivo = 1 
			SET @SumaMontoAmortizacion =  0
			SET @SumaMontoAmortizacionOriginal = 0
			SET @SumaAmorticAcumulado = 0
			
			WHILE (@consecutivo <= @iCount)
			BEGIN
				UPDATE 
					@tmpCuponeraSWAPLEG1 
				SET 
					MontoAmortizacion = (ISNULL(TMP.MontoAmortizacion,0) + ((((SELECT SUM(ISNULL(ICNT.Amortizac,0)) 
																			    FROM @InstrumentosCuponeraNormal ICNT
																			    WHERE ICNT.FechaTermino BETWEEN (TMP.FechaIni+1) AND TMP.FechaFin)/@NumReg)/100) * @p_MontoNominalOriginal)) , 
					NominalRestante  = (@p_MontoNominalOriginal - @SumaMontoAmortizacionOriginal),
					MontoInteres= (ISNULL(TMP.MontoInteres,0) + ISNULL(dbo.fn_SIT_CalcularImporteCupon((@p_MontoNominalOriginal - @SumaMontoAmortizacionOriginal), ICN.TasaCupon, TMP.DifDias, TMP.BaseCupon),0)),
					TasaCupon = ICN.TasaCupon,
					Amortizac = (ISNULL((SELECT SUM(ISNULL(ICNT.Amortizac,0)) 
										 FROM @InstrumentosCuponeraNormal ICNT
										 WHERE ICNT.FechaTermino BETWEEN (TMP.FechaIni+1) AND TMP.FechaFin),0)/@NumReg),
					AmortizacConsolidado = @SumaAmorticAcumulado
				FROM 
					@tmpCuponeraSWAPLEG1 TMP
				LEFT JOIN 
					@InstrumentosCuponeraNormal ICN ON ICN.FechaTermino BETWEEN (TMP.FechaIni+1) AND TMP.FechaFin
				WHERE
					TMP.consecutivo = @consecutivo
				
				SELECT 
					@MontoAmortizacionOriginal = ISNULL(MontoAmortizacion,0), 
					@SumaAmorticAcumulado = ISNULL(Amortizac,0) + ISNULL(@SumaAmorticAcumulado,0)
				FROM 
					@tmpCuponeraSWAPLEG1 
				WHERE
					consecutivo = @consecutivo

				SET @SumaMontoAmortizacionOriginal = @SumaMontoAmortizacionOriginal  + @MontoAmortizacionOriginal 
				SET @consecutivo = @consecutivo + 1
			END
			
			
		END	
	
		UPDATE 
			@tmpCuponeraSWAPLEG1
		SET 
			MontoAmortizacion = MontoAmortizacion + @p_ImporteVentaTotal
		WHERE
			FechaFin = (SELECT MAX(FechaFin) FROM @tmpCuponeraSWAPLEG1)
			
			
		--- >>>>>>>>>>>>>>>>>>>>>> 
		
		SET @DifdiasFechaOperacion = (SELECT 
										(CASE WHEN @p_numeroDiasOriginal = '30' THEN dbo.dias360(FechaIni,@p_FechaOperacion) ELSE dbo.dias(FechaIni,@p_FechaOperacion) END)
									  FROM 
										@tmpCuponeraSWAPLEG1
									  WHERE
										@p_FechaOperacion BETWEEN FechaIni AND FechaFin)
		
		UPDATE 
			@tmpCuponeraSWAPLEG1
		SET
			MontoInteres = MontoInteres - ISNULL(dbo.fn_SIT_CalcularImporteCupon(NominalRestante, TasaCupon, @DifdiasFechaOperacion, BaseCupon),0)
		WHERE
			@p_FechaOperacion BETWEEN FechaIni AND FechaFin
			
	 ---->>>>>>>>>>>>>>
	 SELECT * FROM @tmpCuponeraSWAPLEG1
	 EXEC [CuponeraNormal_Leer] @p_CadenaNemonico
	END 
	ELSE IF @p_tipoCuponera = '0' 
	BEGIN
	
		INSERT INTO @tmpCuponeraSWAPProcesada
		EXEC sp_SIT_GenerarCuponera_AmortizacionInteres 
														@tmpCuponeraSWAPLEG1, 
														@p_fechaEmisionOriginal, 
														@p_fechaVctoOriginal, 
														@p_CodigoTipoAmortizacionOriginal, 
														@p_baseCuponOriginal,
														@p_tasaCuponOriginal,
														@p_tasaSpreadOriginal, 
														@p_CodigoPeriodicidadOriginal, 
														@p_MontoNominalOriginal, 
														@p_numeroDiasOriginal,
														@CorrelativoActualLeg1
		
		DELETE FROM @tmpCuponeraSWAPLEG1
		
		INSERT INTO @tmpCuponeraSWAPLEG1
		SELECT * FROM @tmpCuponeraSWAPProcesada
	END

-- >> AMORTIZAR CUPONERA LEG2 
	DELETE FROM @tmpCuponeraSWAPProcesada
	
	INSERT INTO @tmpCuponeraSWAPProcesada
	EXEC sp_SIT_GenerarCuponera_AmortizacionInteres 
													@tmpCuponeraSWAPLEG2, 
													@p_fechaEmision, 
													@p_fechaVcto, 
													@p_CodigoTipoAmortizacion, 
													@p_baseCupon,
													@p_tasaCupon,
													@p_tasaSpread, 
													@p_CodigoPeriodicidad, 
													@p_MontoNominal, 
													@p_numeroDias,
													@CorrelativoActualLeg2
	
	DELETE FROM @tmpCuponeraSWAPLEG2
	
	INSERT INTO @tmpCuponeraSWAPLEG2
	SELECT * FROM @tmpCuponeraSWAPProcesada
	
	
	INSERT INTO #tmpCuponeraSWAP(
								 Consecutivo,
								 FechaIniOriginal,
								 FechaFinOriginal,
								 DifDiasOriginal,
								 AmortizacOriginal,
								 TasaCuponOriginal,
								 BaseCuponOriginal,
								 DiasPagoOriginal,
								 fechaRealInicialOriginal,
								 fechaRealFinalOriginal,
								 AmortizacConsolidadoOriginal,
								 MontoInteresOriginal,
								 MontoAmortizacionOriginal,
								 NominalRestanteOriginal
								 )
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
		@tmpCuponeraSWAPLEG1
	
	--- >> UNION Cuponera Fechas LEG2 a Cuponera Fechas LEG1								   
	UPDATE 
		#tmpCuponeraSWAP
	SET 
		FechaIni = TCS2.FechaIni,
		FechaFin = TCS2.FechaFin,
		DifDias = TCS2.DifDias,
		Amortizac = TCS2.Amortizac,
		TasaCupon = TCS2.TasaCupon,
		BaseCupon = TCS2.BaseCupon,
		DiasPago = TCS2.DiasPago,
		fechaRealInicial = TCS2.fechaRealInicial,
		fechaRealFinal = TCS2.fechaRealFinal,
		AmortizacConsolidado = TCS2.AmortizacConsolidado,
		MontoInteres = TCS2.MontoInteres,
		MontoAmortizacion = TCS2.MontoAmortizacion,
		NominalRestante = TCS2.NominalRestante						   
	FROM
		#tmpCuponeraSWAP TCS 
	JOIN
		@tmpCuponeraSWAPLEG2 TCS2 ON TCS.Consecutivo = TCS2.Consecutivo
	
	--- >> Insertar cupones de LEG2 cuando fecha de FIN cupon es mayor a fecha FIN cupon de LEG1
	
	IF (SELECT MAX(Consecutivo) FROM #tmpCuponeraSWAP) < (SELECT MAX(Consecutivo) FROM @tmpCuponeraSWAPLEG2) 
	BEGIN
		INSERT INTO #tmpCuponeraSWAP (
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
									 )
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
			@tmpCuponeraSWAPLEG2
		WHERE Consecutivo NOT IN (SELECT Consecutivo FROM #tmpCuponeraSWAP)
	END
	
	SELECT 
		 Consecutivo,						
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaIniOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaIniOriginal) END) FechaIniOriginal,
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaFinOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaFinOriginal) END) FechaFinOriginal,
		 ISNULL(DifDiasOriginal,0) DifDiasOriginal,
		 ISNULL(AmortizacOriginal,0) AmortizacOriginal ,
		 ISNULL(TasaCuponOriginal,0) - ISNULL(@p_tasaSpreadOriginal,0) TasaCuponOriginal,
		 ISNULL(BaseCuponOriginal,'') BaseCuponOriginal,
		 ISNULL(DiasPagoOriginal,'') DiasPagoOriginal,
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealInicialOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealInicialOriginal) END)  fechaRealInicialOriginal,
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealFinalOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealFinalOriginal) END)  fechaRealFinalOriginal,
		 ISNULL(AmortizacConsolidadoOriginal,0) AmortizacConsolidadoOriginal,
		 ISNULL(MontoInteresOriginal,0) MontoInteresOriginal,
		 ISNULL(MontoAmortizacionOriginal,0) MontoAmortizacionOriginal,
		 ISNULL(NominalRestanteOriginal,0) NominalRestanteOriginal,
		 ISNULL(@p_tasaSpreadOriginal,0) TasaSpreadOriginal,   
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaIni,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaIni) END) FechaIni,
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaFin,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaFin) END) FechaFin,
		 ISNULL(DifDias,0) DifDias,
		 ISNULL(Amortizac,0) Amortizac,
		 ISNULL(TasaCupon,0) -  ISNULL(@p_tasaSpread,0) TasaCupon,
		 ISNULL(BaseCupon,'') BaseCupon,
		 ISNULL(DiasPago,'') DiasPago,
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealInicial,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealInicial) END) fechaRealInicial,
		 (CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(fechaRealFinal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(fechaRealFinal) END) fechaRealFinal,
		 ISNULL(AmortizacConsolidado,0) AmortizacConsolidado,
		 ISNULL(MontoInteres,0) MontoInteres,
		 ISNULL(MontoAmortizacion,0) MontoAmortizacion,
		 ISNULL(NominalRestante,0) NominalRestante,
		 ISNULL(@p_tasaSpread,0) TasaSpread,  
		 TotalFlujoOriginal = ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0),
		 TotalFlujo = ISNULL(MontoAmortizacion,0) + ISNULL(MontoInteres,0),
		 TCFlujo = (ISNULL(MontoAmortizacion,0) + ISNULL(MontoInteres,0)) /(CASE WHEN (ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)) = 0 
		 																	THEN 1 ELSE (ISNULL(MontoAmortizacionOriginal,0) + ISNULL(MontoInteresOriginal,0)) END)
		,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaIniOriginal,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaIniOriginal+@p_DiaTOriginal) END) FechaLiborOriginal	
		,(CASE WHEN (dbo.FN_SIT_OBT_FechaFormateada(ISNULL(FechaIni,11111111))) = '11/11/1111' THEN '00/00/0000' ELSE dbo.FN_SIT_OBT_FechaFormateada(FechaIni+@p_DiaT) END) FechaLibor	
		
	FROM 
		#tmpCuponeraSWAP

	DROP TABLE #tmpCuponeraSWAP
	
END

GRANT EXECUTE ON [dbo].[sp_SIT_ObtenerCuponera] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[sp_SIT_GenerarCuponera_AmortizacionInteres] '
USE [SIT-FONDOS]
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_GenerarCuponera_AmortizacionInteres]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_GenerarCuponera_AmortizacionInteres]
GO
----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Generar Cuponera SWAP opción automático  
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 05/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Creación de Store Procedure    
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Creación	    : 29/03/2019
-- Creado por    			: Ernesto Galarza
-- Nro. Orden de Trabajo	: 11851
-- Descripcion del Cambio	: Agregando campos DiaT y tasa para Leg1 y Leg2
---------------------------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[sp_SIT_GenerarCuponera_AmortizacionInteres](
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
	@p_Consecutivo int = 0
) 
AS          
BEGIN 

PRINT '===>ENTRO [sp_SIT_GenerarCuponera_AmortizacionInteres]'
PRINT '@p_Consecutivo: '+CONVERT(VARCHAR(10),@p_Consecutivo)
PRINT '@p_tasaCupon: '+CONVERT(VARCHAR(30),@p_tasaCupon)
PRINT '@p_tasaSpread: '+CONVERT(VARCHAR(30),@p_tasaSpread)


-- >> Asignar Amortizaciones
DECLARE 
	@sumCuponesMenoUltimoCupon NUMERIC(22,7),
	@NroDiasAmortizacion INT,
	@iCount INT,
	@DiasPeriodo NUMERIC(3) = 0
	
	DECLARE
		@tmpCuponera UT_CuponeraTemporal
	
	INSERT INTO @tmpCuponera
	SELECT * FROM @p_TablaCuponera
	
--	UPDATE @tmpCuponera SET TasaCupon=@p_tasaCupon

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
		
		SET @i = 1
		IF @p_baseCupon = '360'
			SELECT @Diferencia= dbo.dias360(@p_fechaIni,@p_fechaFin)
		ELSE
			SELECT @Diferencia= dbo.dias(@p_fechaIni,@p_fechaFin)
		
		SET @numCuponesAmort = CAST(@Diferencia / @NroDiasAmortizacion AS INT) + (CASE WHEN (@Diferencia % @NroDiasAmortizacion) > 0 THEN 1 ELSE 0 END)
		SET @porcentaAmort = CONVERT(NUMERIC(22,7),100) / CONVERT(NUMERIC(22,7),@numCuponesAmort)
		
		WHILE @i < @numCuponesAmort 
		BEGIN
			SET @p_fechaFin = dbo.RetornarFechaProxima360(@p_fechaINI,@NroDiasAmortizacion)
			IF EXISTS(SELECT 1 FROM @tmpCuponera WHERE FechaFin = @p_fechaFIN) 
			BEGIN
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
					SELECT @Diferencia= dbo.dias(@fechaTemp,@p_fechaFin)
				
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
							--	@p_tasaCupon,
								@p_tasaCupon+@p_tasaSpread,
								  (CASE WHEN @p_baseCupon = 'ACT' THEN 
																    (CASE WHEN dbo.SIT_fn_EsAñoBisiesto(LEFT(@p_fechaFin,4))= 1 THEN '366' ELSE '365' END)
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
					DifDias = (CASE WHEN @p_numeroDias = '30' THEN dbo.dias360(@p_fechaFin,FechaFin) ELSE dbo.dias(@p_fechaFin,FechaFin) END),
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
			Amortizac = (CONVERT(NUMERIC(22,7),100) - @sumCuponesMenoUltimoCupon) 
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
		@MontoAmortizacion  NUMERIC(22,7),
		@SumaMontoAmortizacionOriginal NUMERIC(22,7), 
		@MontoAmortizacionOriginal NUMERIC(22,7)
		
	SET @consecutivo=1 
	SET @sumaamortizacion=0
	SET @SumaMontoAmortizacion = 0 
	SET @SumaMontoAmortizacionOriginal = 0 

	SET @iCount = (SELECT COUNT(*) FROM @tmpCuponera)

	--ACTUALIZO TASA EN CUPON ACTUAL @p_tasaCupon+@p_tasaSpread
	--UPDATE 
	--	@tmpCuponera 
	--	SET TasaCupon = TasaCupon+@p_tasaSpread
	--	WHERE consecutivo=@p_Consecutivo
	
	WHILE (@consecutivo) <= @iCount
	BEGIN
	
		UPDATE 
			@tmpCuponera 
		SET 
			AmortizacConsolidado = @sumaamortizacion,
			MontoInteres = dbo.fn_SIT_CalcularImporteCupon(@p_MontoNominal-@SumaMontoAmortizacion,TasaCupon,DifDias,BaseCupon),
			MontoAmortizacion = ((Amortizac /100) * @p_MontoNominal) , 
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
		SET @SumaMontoAmortizacionOriginal = @SumaMontoAmortizacionOriginal  + @MontoAmortizacionOriginal 
		SET @consecutivo=@consecutivo+1
	END
	
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


GRANT EXECUTE ON [dbo].[sp_SIT_GenerarCuponera_AmortizacionInteres] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[CuponeraOI_ConfirmarVencimiento]'
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CuponeraOI_ConfirmarVencimiento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CuponeraOI_ConfirmarVencimiento]
GO

---------------------------------------------------------------------------------------------------------------------
-- Fecha Creaci¾n: 17/07/2017
-- Modificado por: Ian Pastor Mendoza
-- Nro. Orden de Trabajo: 10244
-- Descripcion del cambio: Se corrigi¾ la grabaci¾n del monto de operaci¾n que estaba guardando el valor cero
---------------------------------------------------------------------------------------------------------------------
-- Fecha Creaci¾n: 20/12/2018
-- Modificado por: Ricardo Colonia
-- Nro. Orden de Trabajo: 11687
-- Descripcion del cambio: Se incluye vencimientos de bonos Swap.
---------------------------------------------------------------------------------------------------------------------
--	Fecha Modificaci¾n: 14/02/2019
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
			SE QUITO ESTA FUNCIONALIDAD PORQUE LA INTERFAZ DE LIQUIDACIËN DE CXC Y CXP NO EST┴ PREPARADA PARA SOPORTAR
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
					CASE WHEN @CodigoOperacion = '24' THEN 'S' ELSE 'N' END,
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

PRINT '[dbo].[CuponeraOI_BuscarVencimientos] '
USE [SIT-FONDOS]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CuponeraOI_BuscarVencimientos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CuponeraOI_BuscarVencimientos]
GO


----------------------------------------------------------------------------------------------------------------------------------------    
-- Objetivo: Calcular los intereses corridos a una fecha de corte para un portafolio determinado   
----------------------------------------------------------------------------------------------------------------------------------------    
-- Fecha de Modificación: 01/10/2018    
-- Modificado por: Ricardo Colonia   
-- Nro. Orden de Trabajo: 11547  
-- Descripción del cambio: Modificación de considerar 7 decimales en ValorNominalLocal  
----------------------------------------------------------------------------------------------------------------------------------------   
-- Fecha de Modificación: 20/12/2018    
-- Modificado por: Ricardo Colonia   
-- Nro. Orden de Trabajo: 11687  
-- Descripción del cambio: Se considera vctos intereses SWAP.
----------------------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[CuponeraOI_BuscarVencimientos](          
	@p_CodigoNemonico VARCHAR(15),          
	@p_ValorNominal  NUMERIC(22,7),           
	@p_Fecha  NUMERIC(8),    
	@nvcCodigoPortafolio NVARCHAR(12),        
	@p_CodigoOrden VARCHAR(12),  
	@p_Saldo NUMERIC(22,7) OUTPUT,    
	@p_CodigoISIN VARCHAR(12) OUTPUT,    
	@p_CodigoSBS VARCHAR(12) OUTPUT,    
	@p_FechaVencimiento NUMERIC(8) OUTPUT,    
	@p_Secuencial VARCHAR(5)    
)          
AS          
BEGIN    
 DECLARE 
	@ordenGenera VARCHAR(20) = '',
	@codigoOperacion VARCHAR(6) = ''
  
 SET 
	@p_FechaVencimiento = 0    
	
 SELECT 
	@p_FechaVencimiento = FechaVencimiento  
 FROM 
	CuponeraPagos    
 WHERE 
	CodigoNemonico = @p_CodigoNemonico      
	AND dbo.Dias(FechaPago, @p_Fecha) <= 2      
	AND CodigoPortafolioSBS = @nvcCodigoPortafolio    
    
 SELECT 
	@p_Saldo = CantidadOperacion,
	@p_CodigoISIN = CodigoISIN, 
	@p_CodigoSBS = CodigoSBS,
	@ordenGenera = OrdenGenera,
	@codigoOperacion = CodigoOperacion   
 FROM 
	OrdenInversion 
 WHERE 
	CodigoOrden = @p_CodigoOrden 
	AND CodigoPortafolioSBS = @nvcCodigoPortafolio      
     
 SET @p_Saldo = ISNULL(@p_Saldo,0)  
 SET @p_CodigoISIN = ISNULL(@p_CodigoISIN,'')
 SET @p_CodigoSBS = ISNULL(@p_CodigoSBS,'')
	
 IF EXISTS (SELECT 1 
			FROM CuponeraNormal 
			WHERE CodigoNemonico = @p_CodigoNemonico)   
 BEGIN 
	 SELECT 
		c.FechaInicio 'FechaInicio',
		c.FechaTermino 'FechaTermino', 
		CASE WHEN (S.Saldo IS NULL) THEN 0 ELSE CONVERT(NUMERIC(22,7),ROUND(((s.ValorUnitario * s.Saldo) * 
												(TasaCupon/100) * dbo.Dias360(FechaInicio, FechaTermino) / Base),7)) END AS ValorNominalLocal,    
		CONVERT(NUMERIC(22,6),ROUND(c.TasaCupon,6)) 'TasaNominal',    
		ISNULL(c.Estado, 'PENDIENTE') AS 'Situacion',
		@p_CodigoNemonico 'CodigoNemonico',
		ISNULL(c.FechaPago,0) 'FechaPago',
		c.Secuencia    
	 FROM 
		CuponeraNormal c    
	 LEFT JOIN(    
		SELECT 
			sct.CodigoMnemonico, 
			sct.FechaSaldo, 
			ISNULL(SaldoDisponible, 0) + ISNULL(SaldoProcesoCompra, 0) - ISNULL(SaldoProcesoVenta, 0) - ISNULL(SaldoUnidadesBloqueadas, 0) AS Saldo,
			v.ValorUnitario          
		FROM 
			SaldosCarteraTitulo sct          
		INNER JOIN 
			Valores v ON sct.CodigoMnemonico = v.CodigoNemonico          
		WHERE 
			sct.CodigoMnemonico = @p_CodigoNemonico 
			AND sct.FechaSaldo = @p_Fecha 
			AND sct.CodigoPortafolioSBS = @nvcCodigoPortafolio           
		) s ON c.CodigoNemonico = s.CodigoMnemonico          
	 WHERE 
		CodigoNemonico = @p_CodigoNemonico 
		AND Situacion = 'A'   
		AND C.Secuencia = @p_Secuencial       
	 ORDER BY 
		c.FechaTermino  
 END 
 ELSE IF (EXISTS (SELECT 1 
			FROM Cuponera_Bono_Swap 
			WHERE CodigoOrden = @ordenGenera) AND @p_CodigoNemonico = 'SWAP')
 BEGIN
	 IF @codigoOperacion = '22' 
	 BEGIN
		SET @p_Secuencial = (SELECT 
								MAX(Correlativo)
							 FROM 
								Cuponera_Bono_Swap
							 WHERE
								FechaFin <= @p_Fecha
								AND CodigoOrden = @ordenGenera)
	 END
	 ELSE IF @codigoOperacion = '24' 
	 BEGIN
		SET @p_Secuencial = (SELECT 
								MAX(Correlativo)
							 FROM 
								Cuponera_Bono_Swap
							 WHERE
								FechaFinOriginal <= @p_Fecha
								AND CodigoOrden = @ordenGenera)
	 END
	 SELECT
		(CASE WHEN (@codigoOperacion = '22') THEN CBS.FechaIni 
		      WHEN (@codigoOperacion = '24') THEN CBS.FechaIniOriginal END) 'FechaInicio',
		(CASE WHEN (@codigoOperacion = '22') THEN CBS.FechaFin 
		      WHEN (@codigoOperacion = '24') THEN CBS.FechaFinOriginal END) 'FechaTermino',
		 OI.MontoOperacion 'ValorNominalLocal',
		 CONVERT(NUMERIC(22,6),ROUND( ISNULL(OI.TasaPorcentaje , OI.YTM),6)) 'TasaNominal',  
		 'PENDIENTE' AS 'Situacion',
		  @p_CodigoNemonico 'CodigoNemonico',
		 ISNULL(OI.FechaPago,0) 'FechaPago',     
		 CBS.Correlativo AS Secuencia 
	 FROM 
		 Cuponera_Bono_Swap CBS
	 JOIN 
		 OrdenInversion OI ON OI.OrdenGenera = CBS.CodigoOrden 
	 WHERE
		CBS.Correlativo = @p_Secuencial
		AND OI.CodigoOrden = @p_CodigoOrden
		AND OI.CodigoPortafolioSBS = @nvcCodigoPortafolio
		AND OI.CodigoMnemonico = 'SWAP'
		AND OI.Situacion = 'A'
 END
 ELSE 
 BEGIN   
	 SELECT 
		V.FechaEmision 'FechaInicio',
		C.FechaContrato 'FechaTermino',
		C.MontoOperacion AS ValorNominalLocal,    
		CONVERT(NUMERIC(22,6),ROUND( ISNULL(C.TasaPorcentaje , C.YTM),6)) 'TasaNominal',    
		'PENDIENTE' AS 'Situacion',
		@p_CodigoNemonico 'CodigoNemonico',
		ISNULL(C.FechaPago,0) 'FechaPago',
		'' AS Secuencia    
	 FROM 
		OrdenInversion C    
	 JOIN 
		Valores V ON V.CodigoNemonico = C.CodigoMnemonico     
	 WHERE 
		CodigoOrden = @p_CodigoOrden 
		AND C.CodigoPortafolioSBS = @nvcCodigoPortafolio   
 END
END    
  

GO

GRANT EXECUTE ON [dbo].[CuponeraOI_BuscarVencimientos] TO [rol_sit_fondos] AS [dbo]
GO


PRINT '[dbo].[CronogramaPagos_ListarbyDetalleInstrumento]'

/****** Object:  StoredProcedure [dbo].[CronogramaPagos_ListarbyDetalleInstrumento]    Script Date: 04/02/2019 17:03:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CronogramaPagos_ListarbyDetalleInstrumento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CronogramaPagos_ListarbyDetalleInstrumento]
GO

USE [SIT-FONDOS]
GO


----------------------------------------------------------------------------------------------------------------------------------------  
-- Objetivo: Listar el detalle del cronograma de pagos por instrumento.
----------------------------------------------------------------------------------------------------------------------------------------  
-- Fecha de Modificaci¾n: 14/02/2019  
-- Modificado por: Ricardo Colonia 
-- Nro. Orden de Trabajo: 11825
-- Descripci¾n del cambio: Creaci¾n de Store Procedure
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




PRINT '[dbo].[sp_SIT_sel_RegistroInversiones]'
USE [SIT-FONDOS]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_sel_RegistroInversiones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_sel_RegistroInversiones]
GO

----------------------------------------------------------------------------------------------------------------------
--Objetivo: LISTAR TERCEROS POR TIPO ENTIDAD
----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 01/06/2018
--	Modificado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11339
--	Descripcion del cambio: S├│lo debe mostrarse los datos de las inversiones sin importar que se haya liquidado o no.
----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 20/03/2019
--	Modificado por: Ernesto Galarza.
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: Se cambio el valor de TasaEfectiva que muestre el YTM para los que sean distinto a deposito a plazo.
----------------------------------------------------------------------------------------------------------------------
--	Fecha Modificacion: 03/04/2019
--	Modificado por: Ernesto Galarza.
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Se agrega filtro de fecha final, para filtrar por rango (inicio y fin) la fecha de operación.
----------------------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[sp_SIT_sel_RegistroInversiones]
(
	@CodigoPortafolioSBS Varchar(5),
	@p_FechaInicio Numeric(8),
	@p_FechaFinal Numeric(8)
)
AS
BEGIN
	--OPERACIONES AL CONTADO Y DEP├ôSITOS
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , CodigoEntidad = CASE WHEN CI.Categoria  IN ('CD','DP','OR','FW') THEN ISNULL( E.CodigoEntidad,'')
	ELSE 
		ISNULL( V.CodigoEmisor ,'')
	END
	,CodigoISIN = ISNULL((CASE WHEN OI.CategoriaInstrumento = 'DP' THEN '' ELSE OI.CodigoISIN END),''), 
	CodigoTipoTitulo = ISNULL( V.CodigoNemonico ,''),OI.FechaOperacion  , 
	CantidadOperacion = ISNULL(OI.CantidadOperacion ,0),Precio = ISNULL(OI.Precio,0),V.CodigoMoneda ,
	MontoNominalOperacion = (CASE WHEN OI.CategoriaInstrumento = 'DP' THEN OI.MontoNominalOperacion ELSE ISNULL(OI.MontoOperacion,0) END) ,
	MontoOperacion = (CASE WHEN OI.CodigoOperacion = '3' THEN OI.MontoNominalOperacion  ELSE OI.MontoNetoOperacion END),
	FechaVencimiento = (CASE WHEN OI.CodigoOperacion = '4' THEN 0 ELSE ISNULL(OI.FechaContrato, OI.FechaLiquidacion) END) ,
	Tercero = T.Descripcion,CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	TotalComisiones = (CASE WHEN ISNULL(ICO.CodigoOrdenPreOrden,'') <> '' THEN ISNULL(ICO.ComisionSMV,0) ELSE ISNULL( OI.TotalComisiones,0) END), 
	RestoComisiones = (CASE WHEN ISNULL(ICO.CodigoOrdenPreOrden,'') <> '' THEN ISNULL(ICO.OtraComision,0) ELSE 0 END) ,SUBYACENTE = '',ValorEjecucion = 0, FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),
	Prima = 0, OI.CodigoSBS  
	FROM OrdenInversion OI
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero 
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion AND OI.CodigoOperacion NOT IN ('35','36','37','38','39','65','66')
	JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico  
	JOIN TipoInstrumento TI ON TI.CodigoTipoInstrumentoSBS = V.CodigoTipoInstrumentoSBS 
	JOIN ClaseInstrumento CI ON CI.CodigoClaseInstrumento = TI.CodigoClaseInstrumento
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
	LEFT JOIN (
		SELECT T.CodigoOrdenPreOrden,T.CodigoPortafolioSBS,ComisionSMV = ISNULL(SUM(CASE WHEN T.CodigoComision LIKE '%SAB%' THEN T.ValorComision ELSE 0 END),0),
		OtraComision = ISNULL(SUM(CASE WHEN T.CodigoComision LIKE '%SAB%' THEN 0 ELSE T.ValorComision  END),0)
		FROM ImpuestosComisionesOrdenPreOrden T
		GROUP BY T.CodigoOrdenPreOrden, T.CodigoPortafolioSBS
	) ICO(CodigoOrdenPreOrden,CodigoPortafolioSBS,ComisionSMV,OtraComision)  ON OI.CodigoOrden = ICO.CodigoOrdenPreOrden AND OI.CodigoPortafolioSBS = ICO.CodigoPortafolioSBS
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza
	WHERE 
	--OI.FechaOperacion = @FechaOperacion 
	OI.FechaOperacion >= @p_FechaInicio AND OI.FechaOperacion <= @p_FechaFinal
	AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS AND OI.CategoriaInstrumento NOT IN ('OR','FD')
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A' ORDER BY oi.CodigoOperacion  DESC
	--OPERACIONES DE REPORTE Y OPERACIONES DE PACTO
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , E.CodigoEntidad,
	CodigoISIN = CASE WHEN OI.CodigoOperacion IN ('101','3') THEN ISNULL(OI.CodigoISIN,'') ELSE '' END, 
	CodigoTipoTitulo = ISNULL(  OI.CodigoMnemonico  ,''),OI.FechaOperacion , 
	CantidadOperacion = ISNULL(OI.CantidadOperacion ,0),Precio = ISNULL(OI.MontoNominalOperacion/OI.CantidadOperacion,0),OI.CodigoMoneda , 
	OI.MontoNominalOperacion ,MontoOperacion = CASE WHEN OI.CodigoOperacion IN ('101','3') THEN OI.MontoNominalOperacion  ELSE OI.MontoOperacion END ,
	FechaVencimiento = (CASE WHEN OI.CodigoOperacion = '4' THEN 0 ELSE ISNULL(OI.FechaContrato, OI.FechaLiquidacion) END), 
	Tercero = T.Descripcion,CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	TotalComisiones = (ISNULL(COR.ComisionAIVenta,0) + ISNULL(COR.ComisionAICompra,0)), 
	RestoComisiones = (ISNULL(COR.ImpuestoCompra,0) + ISNULL(COR.ImpuestoVenta,0) + ISNULL(COR.RestoComisionCompra,0) + ISNULL(COR.RestoComisionVenta,0)),
	SUBYACENTE = '',ValorEjecucion = 0,FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),Prima = 0, OI.CodigoSBS 
	FROM OrdenInversion OI
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero 
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	LEFT JOIN ComisionesOR COR ON OI.CodigoOrden = COR.CodigoOrden
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza 
	WHERE 
	--OI.FechaOperacion = @FechaOperacion 
	OI.FechaOperacion >= @p_FechaInicio AND OI.FechaOperacion <= @p_FechaFinal
	AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND OI.CategoriaInstrumento = 'OR'
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A'
	--CUPONES Y DIVIDENDOS COBRADOS
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , CodigoEntidad = V.CodigoEmisor ,CodigoISIN = ISNULL(OI.CodigoISIN,''), CodigoTipoTitulo = ISNULL(v.CodigoNemonico  ,'') ,OI.FechaOperacion , 
	CantidadOperacion = CantidadOrdenado ,Precio = ISNULL(OI.Precio,0),OI.CodigoMoneda , MontoNominalOperacion = 0 ,OI.MontoOperacion,
	FechaVencimiento = OI.FechaLiquidacion,Tercero = '',CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	TotalComisiones =ISNULL( OI.TotalComisiones,0) , RestoComisiones = 0,SUBYACENTE = '',ValorEjecucion = 0, FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),Prima = 0, OI.CodigoSBS 
	FROM OrdenInversion OI
	LEFT JOIN Valores V ON V.CodigoNemonico = OI.CodigoMnemonico 
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza 
	WHERE 
	--OI.FechaOperacion = @FechaOperacion 
	OI.FechaOperacion >= @p_FechaInicio AND OI.FechaOperacion <= @p_FechaFinal
	AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND OI.CodigoOperacion IN ('35','36','37','38','39')
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A' ORDER BY oi.CodigoOperacion  DESC
	--DERIVADOS
	SELECT OI.CodigoOrden,Operacion =  OP.Descripcion , E.CodigoEntidad,CodigoISIN = ISNULL(OI.CodigoISIN,''), OI.CodigoMNemonico AS CodigoTipoTitulo ,OI.FechaOperacion , 
	CantidadOperacion = ISNULL(OI.CantidadOperacion ,0),Precio = ISNULL(OI.Precio,0),OI.CodigoMoneda, 
	MontoNominalOperacion = CASE WHEN OI.CodigoOperacion = '93' THEN OI.MontoCancelar ELSE OI.MontoNominalOperacion END,
	MontoOperacion = CASE WHEN OI.CodigoOperacion = '93' THEN OI.MontoCancelar ELSE OI.MontoOperacion END,
	FechaVencimiento = OI.FechaLiquidacion,Tercero = T.Descripcion,CodigoPlaza = ISNULL(PLZ.Descripcion,''), Mecanismo = ISNULL(PLZ.MnemonicoMecamismo,'') ,
	TasaPorcentaje = CASE WHEN OI.CodigoTipoCupon = '1' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END,
	--TasaEfectiva = CASE WHEN OI.CodigoTipoCupon = '2' THEN ISNULL(OI.TasaPorcentaje,0)/100 ELSE 0 END ,
	TasaEfectiva = CASE WHEN OI.CategoriaInstrumento <> 'DP' THEN ISNULL(OI.YTM,0)/100 ELSE ISNULL(OI.TasaPorcentaje,0)/100 END,
	ISNULL(OI.TotalComisiones,0) TotalComisiones, RestoComisiones = 0,SUBYACENTE = 'DIVISAS',
	ValorEjecucion = CASE WHEN OI.CodigoOperacion = '93' THEN OI.MontoOperacion ELSE 0 END,
	FechaLiquidacion = ISNULL(OI.FechaLiquidacion,0),
	Prima = CASE WHEN OI.CodigoOperacion = '93' THEN OI.TipoCambioFuturo ELSE 0 END,
	OI.CodigoSBS 
	FROM OrdenInversion OI
	JOIN Terceros T ON T.CodigoTercero = OI.CodigoTercero
	JOIN Entidad E ON E.CodigoTercero = T.CodigoTercero AND E.Situacion = 'A'
	JOIN Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion
	--LEFT JOIN OperacionesCaja OC ON OC.CodigoOrden = OI.CodigoOrden AND OC.CodigoPortafolioSBS = OI.CodigoPortafolioSBS 
	LEFT JOIN Plaza PLZ ON OI.CodigoPlaza = PLZ.CodigoPlaza 
	WHERE 
	--OI.FechaOperacion = @FechaOperacion
	OI.FechaOperacion >= @p_FechaInicio AND OI.FechaOperacion <= @p_FechaFinal 
	AND OI.CodigoPortafolioSBS = @CodigoPortafolioSBS  AND OI.CodigoMnemonico  = 'FORWARD'
	AND OI.Estado <> 'E-ELI'  AND OI.Situacion = 'A'
	--Nombre completo portafolio	
	SELECT NombreCompleto = ISNULL(NombreCompleto,'') FROM Portafolio WHERE CodigoPortafolioSBS = @CodigoPortafolioSBS
END


GRANT EXECUTE ON [dbo].[sp_SIT_sel_RegistroInversiones] TO [rol_sit_fondos] AS [dbo]
GO

PRINT '[dbo].[OrdenInversion_ListarOIEjecutadasConfirmacion]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[OrdenInversion_ListarOIEjecutadasConfirmacion]    Script Date: 04/04/2019 10:36:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenInversion_ListarOIEjecutadasConfirmacion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrdenInversion_ListarOIEjecutadasConfirmacion]
GO

-----------------------------------------------------------------------------------------------------------  
-- Autor:   Ricardo Colonia  
-- Fecha Creación: 02/07/2018  
-- Proyecto:  Fondos-II  
-- OT: 11473  
-- Descripción:  Se agrega nuevo parámetro de búsqueda tipoInstrumento  
-----------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 31/08/2018  
-- Modificado por: Ian Pastor Mendoza  
-- Nro. Orden de Trabajo: 11590  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO  
-----------------------------------------------------------------------------------------------------------  
-- Fecha modificacion: 19/12/2018  
-- Modificado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11687
-- Descripcion del cambio: Agregar opción de vctos y amortizaciones para SWAP 
-----------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[OrdenInversion_ListarOIEjecutadasConfirmacion](        
 @p_Fondo varchar(10),  
 @p_NumeroOrden varchar(12),  
 @p_Fecha numeric(8)  
 ,@p_tipoInstrumento varchar(10)  -- RCE | Nuevo Parámetro de búsqueda | 02072018  
)  
AS  
BEGIN  
  CREATE TABLE #DRLConfirmacion(  
   CodigoPortafolio VARCHAR(20),  
   Fondo    VARCHAR(20),  
   Descripcion   VARCHAR(100),  
   MontoOperacion  NUMERIC(22,7),  
   TipoOrden   VARCHAR(50),  
   NumeroTransaccion VARCHAR(80),  
   TipoOperacion  VARCHAR(30),  
   Categoria   VARCHAR(60),  
   FechaOperacion  VARCHAR(10),  
   Estado    VARCHAR(10),  
   FechaVencimiento VARCHAR(10),  
   ValorNominaLocalCupon NUMERIC(22,7),  
   TipoConfirmacion VARCHAR(10),  
   Secuencial   VARCHAR(50),  
   CodigoSBS   VARCHAR(12),  
   CodigoOperacion  VARCHAR(6),  
   CodigoISIN   VARCHAR(20),  
   CodigoMoneda  VARCHAR(4),  
   CodigoMnemonico  VARCHAR(15)  
  )  
  INSERT INTO #DRLConfirmacion  
  SELECT   
    P.CodigoPortafolioSBS  AS 'CodigoPortafolio',  
    P.Descripcion AS 'Fondo',  
    LTRIM(DRL.CodigoNemonico) + '-' + ISNULL(CL.Descripcion, '') AS 'Descripcion',  
    CASE WHEN DRL.TipoDistribucion = 'L' THEN  
      ROUND((ISNULL(DRL.Factor, 0)/100) * dbo.GetSaldoDisponibleValor((CASE WHEN ISNULL(@p_Fondo,'') = '' THEN DRL.CodigoPortafolioSBS 
																			ELSE @p_Fondo END) ,DRL.CodigoNemonico, drl.FechaCorte),7,1)  
    ELSE   
      (ISNULL(DRL.Factor, 0) *  dbo.GetSaldoDisponibleValor((CASE WHEN ISNULL(@p_Fondo,'') = '' THEN DRL.CodigoPortafolioSBS 
																 ELSE @p_Fondo END),DRL.CodigoNemonico, drl.FechaCorte)) 
																																													
    END AS 'MontoOperacion',  
    '--' AS 'TipoOrden',  
    'Nro.' + CONVERT(VARCHAR(50),DRL.IdentIFicador) AS 'NumeroTransaccion',  
    CASE DRL.TipoDistribucion WHEN 'D' THEN 'Dividendo'   
            WHEN 'R' THEN 'Rebate'   
            WHEN 'L' THEN 'Liberada' END AS 'TipoOperacion',  
    CL.Descripcion AS 'Categoria',  
    -- INICIO | RCE | Se cambia la fecha de operación de ingreso cuando el tipo de fondo @p_fondo='' por la fecha de constitución de portafolio  
    SUBSTRING(CONVERT(VARCHAR,(CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN ISNULL(dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(DRL.CodigoPortafolioSBS),'000000') ELSE @p_Fecha END)),7,2) + '/'   
    + SUBSTRING(CONVERT(VARCHAR,(CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN ISNULL(dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(DRL.CodigoPortafolioSBS),'000000') ELSE @p_Fecha END)),5,2) + '/'   
    + SUBSTRING(CONVERT(VARCHAR,(CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN ISNULL(dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(DRL.CodigoPortafolioSBS),'000000') ELSE @p_Fecha END)),1,4) AS 'FechaOperacion',    
    -- FIN | RCE | Se cambia la fecha de operación de ingreso cuando el tipo de fondo @p_fondo='' por la fecha de constitución de portafolio  
    'Vencido' AS Estado,  
    -- INICIO | RCE | Se cambia la fecha de operación de ingreso cuando el tipo de fondo @p_fondo='' por la fecha de constitución de portafolio  
    (CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(DRL.CodigoPortafolioSBS) ELSE @p_Fecha END) as 'FechaVencimiento',  
    -- FIN | RCE | Se cambia la fecha de operación de ingreso cuando el tipo de fondo @p_fondo='' por la fecha de constitución de portafolio  
    NULL AS 'ValorNominaLocalCupon',  
    'DRL' AS 'TipoConfirmacion',  
    CONVERT(VARCHAR(50),DRL.Identificador) AS 'Secuencial',  
    DRL.CodigoSBS 'CodigoSBS',  
    '' AS 'CodigoOperacion',  
    ISNULL(DRL.CodigoISIN,'-') AS 'CodigoISIN',  
    DRL.CodigoMoneda AS 'CodigoMoneda',  
    DRL.CodigoNemonico AS 'CodigoMnemonico'   
  FROM    
    DividendosRebatesLiberadas DRL  
  INNER JOIN   
     Portafolio P ON P.CodigoPortafolioSBS = DRL.CodigoPortafolioSBS  
  INNER JOIN   
     Valores VA on VA.CodigoNemonico = DRL.CodigoNemonico  
  INNER JOIN   
     TipoInstrumento TI on TI.CodigoTipoInstrumentoSBS = VA.CodigoTipoInstrumentoSBS  
  INNER JOIN   
     ClaseInstrumento CL ON CL.CodigoClaseInstrumento = TI.CodigoClaseInstrumento  
  LEFT JOIN   
     VectorPrecio VP ON VP.CodigoMnemonico = DRL.CodigoNemonico AND VP.Fecha = DRL.FechaCorte  
     AND VP.EntidadExt = dbo.RetornarSecuenciaFuenteVP(DRL.CodigoPortafolioSBS,DRL.CodigoNemonico,DRL.FechaCorte)  
  WHERE    
    CASE WHEN TipoDistribucion = 'L' THEN FechaEntrega   
      ELSE FechaIDI END = (CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(DRL.CodigoPortafolioSBS)   
           ELSE @p_Fecha END) 
    AND  DRL.Situacion = 'A'   
    AND  DRL.CodigoPortafolioSBS = (CASE WHEN ISNULL(@p_Fondo,'') = '' THEN DRL.CodigoPortafolioSBS ELSE @p_Fondo END) 
																													   
    AND  DRL.Estado IS NULL  
    AND CL.Categoria = (CASE WHEN ISNULL(@p_tipoInstrumento,'')= '' THEN CL.Categoria ELSE  @p_tipoInstrumento END) 
  --//**//**//***/**  
  UPDATE   
    #DRLConfirmacion   
  SET   
    MontoOperacion = Round(MontoOperacion,7)   
  WHERE   
    TipoOperacion = 'Liberada'       
  --//**//**//***/**  
    
  SELECT   
    P.CodigoPortafolioSBS AS 'CodigoPortafolio',  
    P.Descripcion AS 'Fondo',  
    LTRIM(OI.CodigoMnemonico) + '-' + CASE WHEN OI.TipoFondo in('CC_CNC','CC_SNC') THEN ISNULL(UPPER(TF.Nombre),'') ELSE ISNULL(TE.Descripcion,'') END 
																				   + '-' + ISNULL(CI.Descripcion,'') AS 'Descripcion',  
    CASE WHEN OI.CodigoOperacion = '3' THEN ISNULL(OI.MontoNominalOPeracion, 0)   
      WHEN OI.CodigoOperacion IN ('36','37') THEN ISNULL(DRL.MontoNominalLocal,0)  
      ELSE ISNULL(OI.MontoOperacion, '0.0000000')  END AS 'MontoOperacion',  
    CI.Descripcion AS 'TipoOrden',  
    OI.CodigoOrden AS 'NumeroTransaccion',  
    CASE WHEN OI.CodigoOperacion IN (65,66) AND OI.Delibery='N' THEN 'Pre-Liquidacion ' + OP.Descripcion   
      ELSE ISNULL(OP.Descripcion,'') END AS 'TipoOperacion',  
    CI.Categoria,  
    SUBSTRING(CONVERT(VARCHAR,OI.FechaOperacion),7,2) + '/' + SUBSTRING(CONVERT(VARCHAR,OI.FechaOperacion),5,2) + '/' + SUBSTRING(CONVERT(VARCHAR,OI.FechaOperacion),1,4) AS 'FechaOperacion',        
    OI.Estado,  
    CASE WHEN OI.CodigoOperacion IN ('4','35','39','36','38','37','22','23','24','25') THEN OI.FechaOperacion   
      ELSE NULL END AS 'FechaVencimiento',  
    CASE WHEN OI.CodigoOperacion IN ('4','35', '36', '38', '39','37','22','23','24','25') THEN OI.MontoOperacion   
      ELSE 0 END AS 'ValorNominaLocalCupon',  
    CASE WHEN OI.CodigoOperacion IN ('35','22','24') THEN 'COI'   
      WHEN OI.CodigoOperacion = '36' THEN 'DRL'   
      WHEN OI.CodigoOperacion = '37' THEN 'DRL'  
      WHEN (OI.CodigoOperacion IN ('38','39') AND ((SELECT COUNT(1)   
												   FROM Valores   
												   WHERE CodigoNemonico = OI.CodigoMNemonico AND CodigoTipoInstrumentoSBS = '08' AND  
												   CodigoEmisor = 'US-T' AND CodigoTipoCupon = '3' ) = 0)
			OR OI.CodigoOperacion IN ('23','25')) THEN 'COA'   
      ELSE 'OI' END AS 'TipoConfirmacion',   
    CONVERT(VARCHAR(50),DRL.IdentIFicador)  AS 'Secuencial',  
    OI.CodigoSBS AS 'CodigoSBS',  
    OI.CodigoOperacion AS 'CodigoOperacion',  
    '' CodigoTipoCupon,  
    '' CodigoTercero,  
    '' CodigoTipoTitulo,  
    0 TasaCupon,  
    CONVERT(NUMERIC, ISNULL((SELECT VALOR+200   
           FROM ParametrosGenerales   
           WHERE ClasIFicacion = 'ORDTIPOPE' AND Nombre = OI.CodigoOperacion), OI.CodigoOperacion+210)) AS orden,  
    ISNULL(OI.TasaPorcentaje,0) 'TasaPorcentaje',  
    ISNULL(TC.Nombre,'') 'Nombre',  --> '(DP) - Tipo Tasa',  
    ISNULL(OI.Plazo,0) 'Plazo',        --> 'Plazo',   
    ISNULL(OI.CodigoMoneda,'') 'CodigoMoneda',    --> 'Moneda',  
    ISNULL(OI.MontoNetoOperacion,0) 'MontoNetoOperacion', --> 'Monto al Vencimiento',  
    ISNULL(TE.Descripcion,'') 'Contraparte',       --> 'Contraparte'  
    CASE WHEN OI.CategoriaInstrumento ='FD' THEN ISNULL(OI.MontoCancelar,0) ELSE ISNULL(OI.MontoNominalOrdenado,0) END 'MontoNominalOrdenado',   --> 'Monto Nominal',   
    ISNULL(OI.InteresCorridoNegociacion,0) 'InteresCorridoNegociacion', --> 'Interés Corrido',  
    ISNULL(OI.CantidadOperacion,0) 'CantidadOperacion',     --> 'Cantidad',  
    SUBSTRING(CONVERT(VARCHAR,ISNULL(OI.FechaLiquidacion,'000000')),7,2) + '/' + SUBSTRING(CONVERT(VARCHAR,ISNULL(OI.FechaLiquidacion,'000000')),5,2) + '/' 
																			   + SUBSTRING(CONVERT(VARCHAR,ISNULL(OI.FechaLiquidacion,'000000')),1,4) AS 'FechaLiquidacion',  
    CASE WHEN CI.Categoria <> 'PC' THEN ISNULL(OI.Precio,0) ELSE ISNULL(OI.PrecioNegociacionLimpio,0) END 'Precio',        --> 'Precio',   
    CASE WHEN CI.Categoria = 'OR' THEN ISNULL(COR.ComisionAICompra,0)+ISNULL(COR.ComisionAIVenta,0)+ISNULL(COR.ImpuestoCompra,0)+  
               ISNULL(COR.ImpuestoVenta,0)+ISNULL(COR.RestoComisionCompra,0)+ISNULL(COR.RestoComisionVenta,0)   
        ELSE ISNULL(OI.TotalComisiones,0) END   'TotalComisiones',    --> 'Comisiones',  
    ISNULL(OI.TipoCambioFuturo,0) 'TipoCambioFuturo',   --> 'Tipo Cambio Forward',   
    ISNULL(OI.TipoCambioSpot,0) 'TipoCambioSpot',    --> 'Tipo Cambio Spot',  
    ISNULL(OI.CodigoISIN,'-') 'CodigoISIN',      --> 'Ingreso Código ISIN',  
    ISNULL(OI.CodigoMnemonico,'-') 'CodigoMnemonico',     --> 'Acción en Garantía',  
    CAST([dbo].[FormatearNumero](ISNULL(OI.MontoOrigen,0),2) AS VARCHAR(20)) + ' ' + ISNULL(OI.CodigoMoneda,'') 'MontoOrigen',   --> 'Monto Negociado',   
    CAST([dbo].[FormatearNumero](ISNULL(OI.MontoDestino,0),2) AS VARCHAR(20)) + ' ' + ISNULL(OI.CodigoMonedaDestino,'') 'MontoDestino', --> 'Monto Final',   
    ISNULL(OI.TipoCambio,0) 'TipoCambio',            --> 'Tipo de Cambio',  
    ISNULL(PL.Descripcion,'-') 'Bolsa',       --> 'Bolsa',  
    ISNULL(OI.CantidadOrdenado,0) 'CantidadOrdenado'   --> 'Cuotas',  
  FROM   
    OrdenInversion OI  
  INNER JOIN   
     Portafolio P ON OI.CodigoPortafolioSBS = P.CodigoPortafolioSBS  
  LEFT JOIN   
     Terceros TE ON OI.CodigoTercero = TE.CodigoTercero  
  LEFT JOIN   
     Moneda MO ON OI.CodigoMoneda = MO.CodigoMoneda  
  LEFT JOIN   
     ClaseInstrumento CI ON CI.Categoria = OI.CategoriaInstrumento  
  LEFT JOIN   
     Operacion OP ON OP.CodigoOperacion = OI.CodigoOperacion  
  LEFT JOIN   
     DividendosRebatesLiberadas DRL on drl.TipoDistribucion + cast(IDENTIFICADOR as varchar)= oi.codigoorden AND drl.Situacion = 'A'  
  LEFT JOIN   
     ParametrosGenerales TF ON OI.TipoFondo = TF.Valor AND TF.ClasIFicacion = 'TipoFondoI'  
  LEFT JOIN   
     ParametrosGenerales TC ON OI.CodigoTipoCupon = TC.Valor AND TC.ClasIFicacion = 'TipoTasaI'  
  LEFT JOIN  
     Plaza PL ON PL.CodigoPlaza = OI.CodigoPlaza  
  LEFT JOIN   
     ComisionesOR COR ON COR.CodigoOrden = OI.CodigoOrden  
  WHERE   
    OI.Estado = 'E-EJE'   
    AND FechaOperacion = (CASE WHEN RTRIM(LTRIM(@p_Fondo)) ='' THEN dbo.fn_SIT_Retornar_Portafolio_FechaConsitucion(OI.CodigoPortafolioSBS) ELSE @p_Fecha END) -- RCE | Se cambia la fecha de operación de ingreso cuando el tipo de fondo 
																																							    -- @p_fondo='' por la fecha de constitución de portafolio  
    AND OI.CodigoPortafolioSBS = (CASE WHEN ISNULL(@p_Fondo,'') = '' THEN OI.CodigoPortafolioSBS ELSE @p_Fondo END) -- RCE | Se modifica cáculo de tal manera que cuando se seleccione TODOS muestre todos los registros disponibles | 02072018  
    AND OI.CodigoOrden LIKE (CASE WHEN LEN(@p_NumeroOrden) = 0 THEN OI.CodigoOrden ELSE @p_NumeroOrden + '%' END)  
    AND CI.Categoria = (CASE WHEN ISNULL(@p_tipoInstrumento,'')= '' THEN CI.Categoria  ELSE  @p_tipoInstrumento END) -- RCE | Nuevo Parámetro de búsqueda | 02072018  
    
  UNION ALL  
  SELECT   
    CodigoPortafolio,  
    Fondo,  
    Descripcion,  
    MontoOperacion,  
    TipoOrden,  
    NumeroTransaccion,  
    TipoOperacion,  
    Categoria,  
    FechaOperacion,  
    Estado,  
    FechaVencimiento,  
    ValorNominaLocalCupon,  
    TipoConfirmacion,  
    Secuencial,  
    CodigoSBS,  
    CodigoOperacion,  
    '' CodigoTipoCupon,  
    '' CodigoTercero,  
    '' CodigoTipoTitulo,  
    0 TasaCupon,  
    CONVERT(NUMERIC, ISNULL((SELECT valor+200   
           FROM ParametrosGenerales   
           WHERE ClasIFicacion = 'ORDTIPOPE' AND Nombre = CodigoOperacion), CodigoOperacion+210)) AS ORDEN,  
    0 'TasaPorcentaje',  
    '' 'Nombre',          --> '(DP) - Tipo Tasa',  
    0 'Plazo',           --> 'Plazo',   
    ISNULL(CodigoMoneda,'') 'CodigoMoneda',    --> 'Moneda',  
    0 'MontoNetoOperacion',        --> 'Monto al Vencimiento',  
    '' 'Contraparte',         --> 'Contraparte'  
    0 'MontoNominalOrdenado',       --> 'Monto Nominal',   
    0 'InteresCorridoNegociacion',      --> 'Interés Corrido',  
    0 'CantidadOperacion',        --> 'Cantidad',  
    '0' 'FechaLiquidacion',        --> 'Fecha de Liquidación'  
    0 'Precio',           --> 'Precio',   
    0 'TotalComisiones',        --> 'Comisiones',  
    0 'TipoCambioFuturo',        --> 'Tipo Cambio Forward',   
    0 'TipoCambioSpot',         --> 'Tipo Cambio Spot',  
    ISNULL(CodigoISIN,'-') 'CodigoISIN',    --> 'Ingreso Código ISIN',  
    ISNULL(CodigoMnemonico,'-') 'CodigoMnemonico',  --> 'Acción en Garantía',  
    '' 'MontoOrigen',         --> 'Monto Negociado',   
    '' 'MontoDestino',         --> 'Monto Final',   
    0 'TipoCambio',         --> 'Tipo de Cambio',  
    '-' 'Bolsa',          --> 'Bolsa',  
    0 'CantidadOrdenado'        --> 'Cuotas',  
  FROM   
    #DRLConfirmacion   
  WHERE   
    MontoOperacion > 0  
  ORDER BY   
    Fondo, Categoria, ORDEN  
    
  DROP TABLE #DRLConfirmacion      
 END  
 

GO

GRANT EXECUTE ON [dbo].[OrdenInversion_ListarOIEjecutadasConfirmacion] TO [rol_sit_fondos] AS [dbo]
GO
PRINT '[dbo].[sp_SIT_GeneraVencimientosBono_Swap]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[sp_SIT_GeneraVencimientosBono_Swap]    Script Date: 04/11/2019 20:56:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_GeneraVencimientosBono_Swap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_GeneraVencimientosBono_Swap]
GO

----------------------------------------------------------------------------------------------------------------------------------------      
-- Objetivo: Genera los vencimientos de bonos SWAP.
----------------------------------------------------------------------------------------------------------------------------------------      
-- Fecha de Modificación: 19/12/2018      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11687    
-- Descripción del cambio: Modificación de SP para realizar la generación de los vctos y amortizaciones de los bonos swap.
---------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha de Modificación: 05/04/2019      
-- Modificado por: Ricardo Colonia     
-- Nro. Orden de Trabajo: 11851   
-- Descripción del cambio: Se modifica la fecha de operación generada en el OI por fecha de vencimiento cupón.
---------------------------------------------------------------------------------------------------------------------------------------- 
--exec sp_SIT_GeneraVencimientosBono_Swap 20190220,20190219,'109'  
CREATE PROC [dbo].[sp_SIT_GeneraVencimientosBono_Swap]  
	@p_FechaApertura NUMERIC(8),  
	@p_FechaOperacion NUMERIC(8),
	@p_CodigoPortafolioSBS VARCHAR(10),
	@p_CodigoOperacion VARCHAR(4)
AS 
BEGIN  
 DECLARE 
	   @Correlativo INT, 
	   @MontoInteres NUMERIC(22,7),
	   @MontoAmortizacion NUMERIC(22,7),
	   @intCurrentMax INT, 
	   @CodigoPortafolio VARCHAR(20), 
	   @FechaLiquidacion NUMERIC(8), 
	   @MontoNominal NUMERIC(22,7),  
	   @TipoCambio NUMERIC(22,7), 
	   @CodigoMoneda VARCHAR(10), 
	   @CodigoIsin VARCHAR(12), 
	   @Tasa NUMERIC(22,7), 
	   @TasaOriginal NUMERIC(22,7),  
	   @CodigoOrden VARCHAR(12), 
	   @CodigoOrdenOP1 VARCHAR(12),
	   @CodigoOrdenOP2 VARCHAR(12),
	   @Existe CHAR(1),  
	   @FechaOperacion NUMERIC(8), 
	   @CodigoTercero VARCHAR(12),
	   @GeneradoInteres CHAR(1),
	   @GeneradoAmortizar CHAR(1),
	   @fechaActual DECIMAL (8,0) = (CONVERT(CHAR(10),GETDATE(),112)),
	   @horaActual VARCHAR(10) = (CONVERT(CHAR(10),GETDATE(),108))
	   
 SET @FechaLiquidacion = @p_FechaApertura  
 
-- EXEC Cuponera_Bono_Swap_ActualizarTasaFlotante @p_FechaApertura, @p_CodigoPortafolioSBS
 
 DECLARE 
	   CursorCuponeraLEG2 
						CURSOR FOR      
	                        SELECT 
								CBS.Correlativo, 
								OI.CodigoOrden,
								OI.CodigoPortafolioSBS,
								CBS.MontoInteres,
								CBS.MontoAmortizacion,
								OI.MontoNetoOperacion,
								OI.TipoCambioSpot,
								OID.CodigoMonedaLeg2,
								OI.CodigoISIN,
								CBS.TasaCupon + CBS.TasaSpread,
								CBS.FechaFin, 
								OI.CodigoTercero, 
								CBS.GeneradoInteres,
								CBS.GeneradoAmortizar
							FROM 
								Cuponera_Bono_Swap  CBS  
							JOIN 
							    OrdenInversion OI ON OI.CodigoOrden = CBS.CodigoOrden  
							JOIN
								OrdenInversion_DetalleSWAP OID ON OID.CodigoOrden = OI.CodigoOrden
																  AND OID.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
							--JOIN 
							--    Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS  
							WHERE 
								CBS.FechaFin > @p_FechaOperacion 
								AND CBS.FechaFin <= @p_FechaApertura  
								AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
					OPEN 
						CursorCuponeraLEG2  
					FETCH NEXT FROM 
						CursorCuponeraLEG2 
					INTO 
					    @Correlativo,
					    @CodigoOrden,
					    @CodigoPortafolio,
					    @MontoInteres,
					    @MontoAmortizacion,
					    @MontoNominal,
					    @TipoCambio,
					    @CodigoMoneda,
					    @CodigoIsin,
					    @Tasa,
					    @FechaOperacion,
					    @CodigoTercero,
					    @GeneradoInteres,
					    @GeneradoAmortizar
			 WHILE @@FETCH_STATUS = 0   
			 BEGIN   
			     SELECT 
					@Existe = COUNT(1) 
				 FROM 
					OrdenInversion 
				 WHERE 
					OrdenGenera = @CodigoOrden 
					AND CodigoPortafolioSBS = @CodigoPortafolio 
					AND Estado <> 'E-ELI' AND Situacion = 'A'  
					
				 IF @Existe = 0 OR @GeneradoInteres = '0' OR  @GeneradoAmortizar = '0' 
				 BEGIN  
				   --Moneda BONOS LEG 2
				  -- SELECT TOP 1 
						--@CodigoMonedaOrigen = V.CodigoMoneda 
				  -- FROM 
					 --   Bono_Swap BS  
				  -- JOIN 
				  --      Valores V ON V.CodigoISIN = BS.CodigoIsin  
				  -- WHERE 
						--CodigoOrden = @CodigoOrden  
						
				   -- Generación de Vcto de Interes LEG 2
				  IF @MontoInteres > 0 AND @GeneradoInteres = '0' AND @p_CodigoOperacion IN ('','22') 
				  BEGIN  
					 SELECT 
						@intCurrentMax = MAX(CONVERT(INT, CASE WHEN ISNUMERIC(CodigoOrden) = 1 THEN CodigoOrden ELSE '000000' END))
					 FROM 
						OrdenInversion  
						
					 SELECT 
						@CodigoOrdenOP1 = RIGHT(REPLACE(STR(@intCurrentMax + 1),' ','0'),6)  
						
					 INSERT INTO 
					     OrdenInversion(
										CodigoPortafolioSBS, 
										CodigoOrden, 
										FechaOperacion,
										FechaLiquidacion, 
										FechaContrato, 
										MontoNominalOperacion,  
										CodigoTipoCupon, 
										MontoOperacion, 
										TipoCambio,
										CodigoMoneda, 
										CodigoMonedaDestino, 
										MontoNetoOperacion,
										Estado, 
										Situacion, 
										CodigoISIN,
										CodigoSBS, 
										CodigoTercero,
										CodigoOperacion,
										CodigoMnemonico, 
										CodigoTipoTitulo, 
										CategoriaInstrumento,
										Negociacion, 
										TasaPorcentaje, 
										CantidadOperacion, 
										HoraOperacion,
										FechaCreacion,
										HoraCreacion,
										TipoFondo, 
										Ficticia,
										OrdenGenera 
									    )  
					SELECT  
						 @CodigoPortafolio, 
						 @CodigoOrdenOP1 , 
						 @FechaOperacion,
						 @FechaLiquidacion,
						 @FechaLiquidacion,
						 @MontoNominal,  
						 '2',
						 @MontoInteres,
						 @TipoCambio,
						 @CodigoMoneda,
						 '',
						 @MontoInteres,
						 'E-EJE',
						 'A',
						 @CodigoIsin,
						 '',
						 @CodigoTercero,
						 '22',
						 'SWAP',
						 '',
						 'BS',
						 '',
						 @Tasa,
						 0,
						 @horaActual,
						 @fechaActual,
					     @horaActual,
						 '',
						 'N', 
						 @CodigoOrden
						  
					 --Confirma Operacion de Vcto Interes LEG 2 
					 EXEC OrdenInversion_ConfirmarOI @CodigoOrdenOP1, @CodigoPortafolio, @CodigoOrdenOP1,'' 
					  
					 UPDATE 
						Cuponera_Bono_Swap 
					 SET 
						GeneradoInteres = '1' 
					 WHERE 
						Correlativo = @Correlativo  
						AND CodigoOrden = @CodigoOrden
				  END    
				   -- Generación de Amortización de Capital LEG 2 
				  IF @MontoAmortizacion > 0 AND @GeneradoAmortizar = '0' AND @p_CodigoOperacion IN ('','23') 
				  BEGIN  
					 SELECT 
						 @intCurrentMax = MAX(CONVERT(INT, CASE WHEN ISNUMERIC(CodigoOrden) = 1 THEN CodigoOrden ELSE '000000' END)) 
					 FROM 
						 OrdenInversion  
						 
					 SELECT 
					     @CodigoOrdenOP2 = RIGHT(REPLACE(STR(@intCurrentMax + 1),' ','0'),6) 
					      
					INSERT INTO 
						 OrdenInversion(
						                CodigoPortafolioSBS, 
						                CodigoOrden, 
						                FechaOperacion,
						                FechaLiquidacion, 
						                FechaContrato, 
						                MontoNominalOperacion,
						                CodigoTipoCupon, 
						                MontoOperacion, 
						                TipoCambio,
						                CodigoMoneda,
						                CodigoMonedaDestino, 
						                MontoNetoOperacion,
						                Estado, 
						                Situacion,
						                CodigoISIN,  
						                CodigoSBS, 
						                CodigoTercero, 
						                CodigoOperacion,
						                CodigoMnemonico, 
						                CodigoTipoTitulo, 
						                CategoriaInstrumento,
						                Negociacion, 
						                TasaPorcentaje, 
						                CantidadOperacion,  
						                HoraOperacion,
						                FechaCreacion, 
						                HoraCreacion,
						                TipoFondo, 
						                Ficticia, 
						                OrdenGenera 
						                )  
					SELECT  
						@CodigoPortafolio, 
						@CodigoOrdenOP2, 
						@FechaOperacion,
						@FechaLiquidacion,
						@FechaLiquidacion,
						@MontoNominal,  
						'2',
						@MontoAmortizacion,
						@TipoCambio,
						@CodigoMoneda,
						'',
						@MontoAmortizacion,
						'E-EJE',
						'A',
						@CodigoIsin,
						'',
						@CodigoTercero,
						'23',
						'SWAP',
						'',
						'BS',
						'',
						@Tasa,
						0,
						@horaActual,
						@fechaActual,
					    @horaActual,
						'',
						'N',
						@CodigoOrden  
					 --Confirma Operacion  
					EXEC OrdenInversion_ConfirmarOI @CodigoOrdenOP2, @CodigoPortafolio, @CodigoOrdenOP2,''  
					
					UPDATE 
						Cuponera_Bono_Swap 
					SET 
						GeneradoAmortizar = '1' 
					WHERE 
						Correlativo = @Correlativo  
						AND CodigoOrden = @CodigoOrden
				  END  
			  END  
				 FETCH NEXT FROM 
					CursorCuponeraLEG2 
				 INTO 
					@Correlativo,
					@CodigoOrden,
					@CodigoPortafolio,
					@MontoInteres,
					@MontoAmortizacion,
					@MontoNominal,
					@TipoCambio,
					@CodigoMoneda,
					@CodigoIsin,
					@Tasa,
					@FechaOperacion,
					@CodigoTercero,
					@GeneradoInteres,
					@GeneradoAmortizar 
			END  
		CLOSE CursorCuponeraLEG2  
		DEALLOCATE CursorCuponeraLEG2  
	
DECLARE 
	   CursorCuponeraLEG1 
						CURSOR FOR      
	                        SELECT 
								CBS.Correlativo, 
								OI.CodigoOrden,
								OI.CodigoPortafolioSBS,
								CBS.MontoInteresOriginal,
								CBS.MontoAmortizacionOriginal,
								OI.MontoOperacion,
								OI.TipoCambioSpot,
								OID.CodigoMonedaLeg1,
								OI.CodigoISIN,
								CBS.TasaCuponOriginal + CBS.TasaSpreadOriginal,
								CBS.FechaFinOriginal, 
								OI.CodigoTercero,
								CBS.GeneradoInteresOriginal,
								CBS.GeneradoAmortizarOriginal  
							FROM 
								Cuponera_Bono_Swap  CBS  
							JOIN 
							    OrdenInversion OI ON OI.CodigoOrden = CBS.CodigoOrden  
							JOIN
								OrdenInversion_DetalleSWAP OID ON OID.CodigoOrden = OI.CodigoOrden
																  AND OID.CodigoPortafolioSBS = OI.CodigoPortafolioSBS
							--JOIN 
							--    Portafolio P ON P.CodigoPortafolioSBS = OI.CodigoPortafolioSBS  
							WHERE 
								CBS.FechaFinOriginal > @p_FechaOperacion 
								AND CBS.FechaFinOriginal <= @p_FechaApertura  
								AND OI.CodigoPortafolioSBS = @p_CodigoPortafolioSBS
					OPEN 
						CursorCuponeraLEG1  
					FETCH NEXT FROM 
						CursorCuponeraLEG1 
					INTO 
					    @Correlativo,
					    @CodigoOrden,
					    @CodigoPortafolio,
					    @MontoInteres,
					    @MontoAmortizacion,
					    @MontoNominal,
					    @TipoCambio,
					    @CodigoMoneda,
					    @CodigoIsin,
					    @Tasa,
					    @FechaOperacion,
					    @CodigoTercero,
					    @GeneradoInteres,
						@GeneradoAmortizar 
			 WHILE @@FETCH_STATUS = 0   
			 BEGIN   
			     SELECT 
					@Existe = COUNT(1) 
				 FROM 
					OrdenInversion 
				 WHERE 
					OrdenGenera = @CodigoOrden 
					AND CodigoPortafolioSBS = @CodigoPortafolio 
					AND Estado <> 'E-ELI' AND Situacion = 'A'  
					
				 IF @Existe = 0 OR @GeneradoInteres = '0' OR  @GeneradoAmortizar = '0' 
				 BEGIN  
				   --Moneda BONOS LEG 1
				  -- SELECT TOP 1 
						--@CodigoMonedaOrigen = V.CodigoMoneda 
				  -- FROM 
					 --   Bono_Swap BS  
				  -- JOIN 
				  --      Valores V ON V.CodigoISIN = BS.CodigoIsin  
				  -- WHERE 
						--CodigoOrden = @CodigoOrden  
						
				   -- Generación de Vcto de Interes LEG 1
				  IF @MontoInteres > 0 AND @GeneradoInteres = '0' AND @p_CodigoOperacion IN ('','24') 
				  BEGIN  
					SELECT 
						@intCurrentMax = MAX(CONVERT(INT, CASE WHEN ISNUMERIC(CodigoOrden) = 1 THEN CodigoOrden ELSE '000000' END)) 
					FROM 
						OrdenInversion  
					SELECT 
						@CodigoOrdenOP1 = RIGHT(REPLACE(STR(@intCurrentMax + 1),' ','0'),6)
						  
					INSERT INTO 
						OrdenInversion(
									   CodigoPortafolioSBS,
									   CodigoOrden, 
									   FechaOperacion,
									   FechaLiquidacion, 
									   FechaContrato, 
									   MontoNominalOperacion, 
									   CodigoTipoCupon, 
									   MontoOperacion, 
									   TipoCambio,
									   CodigoMoneda, 
									   CodigoMonedaDestino,
									   MontoNetoOperacion,
									   Estado,
									   Situacion,
									   CodigoISIN,
									   CodigoSBS,
									   CodigoTercero, 
									   CodigoOperacion,
									   CodigoMnemonico,
									   CodigoTipoTitulo, 
									   CategoriaInstrumento,
									   Negociacion, 
									   TasaPorcentaje, 
									   CantidadOperacion, 
									   HoraOperacion,
									   FechaCreacion, 
									   HoraCreacion,
									   TipoFondo, 
									   Ficticia,
									   OrdenGenera 
									   )  
					SELECT  
						@CodigoPortafolio,
						@CodigoOrdenOP1, 
						@FechaOperacion,
						@FechaLiquidacion,
						@FechaLiquidacion,
						@MontoNominal,
						'2',
						@MontoInteres,
						@TipoCambio,
						@CodigoMoneda,
						'',
						@MontoInteres,
						'E-EJE',
						'A',
						@CodigoIsin,
						'',
						@CodigoTercero,
						'24',
						'SWAP',
						'',
						'BS',
						'',
						@Tasa,
						0,
						@horaActual,
						@fechaActual,
					    @horaActual,
						'',
						'N'
						,@CodigoOrden  
						  
					 --Confirma Operacion de Vcto Interes LEG 1 
					 EXEC OrdenInversion_ConfirmarOI @CodigoOrdenOP1, @CodigoPortafolio, @CodigoOrdenOP1,''  
					 
					 UPDATE 
						Cuponera_Bono_Swap 
					 SET 
						GeneradoInteresOriginal = '1' 
					 WHERE 
						Correlativo = @Correlativo  
						AND CodigoOrden = @CodigoOrden
						
				  END    
				   -- Generación de Amortización de Capital LEG 1 
				  IF @MontoAmortizacion > 0 AND @GeneradoAmortizar = '0' AND @p_CodigoOperacion IN ('','25')  
				  BEGIN  
					SELECT 
						@intCurrentMax = MAX(CONVERT(INT, CASE WHEN ISNUMERIC(CodigoOrden) = 1 THEN CodigoOrden ELSE '000000' END)) 
					FROM 
						OrdenInversion  
						
					SELECT 
						@CodigoOrdenOP2 = RIGHT(REPLACE(STR(@intCurrentMax + 1),' ','0'),6)  
						
					INSERT INTO 
						OrdenInversion(
									   CodigoPortafolioSBS, 
									   CodigoOrden, 
									   FechaOperacion,
									   FechaLiquidacion, 
									   FechaContrato, 
									   MontoNominalOperacion,
									   CodigoTipoCupon, 
									   MontoOperacion,
									   TipoCambio,
									   CodigoMoneda, 
									   CodigoMonedaDestino, 
									   MontoNetoOperacion,
									   Estado, 
									   Situacion, 
									   CodigoISIN, 
									   CodigoSBS, 
									   CodigoTercero, 
									   CodigoOperacion,
									   CodigoMnemonico, 
									   CodigoTipoTitulo, 
									   CategoriaInstrumento,
									   Negociacion, 
									   TasaPorcentaje, 
									   CantidadOperacion, 
									   HoraOperacion,
									   FechaCreacion, 
									   HoraCreacion,
									   TipoFondo, 
									   Ficticia,
									   OrdenGenera 
									   )  
					SELECT  
						@CodigoPortafolio, 
						@CodigoOrdenOP2, 
						@FechaOperacion,
						@FechaLiquidacion,
						@FechaLiquidacion,
						@MontoNominal,
						'2',
						@MontoAmortizacion,
						@TipoCambio,
						@CodigoMoneda,
						'',
						@MontoAmortizacion,
						'E-EJE',
						'A',
						@CodigoIsin,
						'',
						@CodigoTercero,
						'25',
						'SWAP',
						'',
						'BS',
						'',
						@Tasa,
						0,
						@horaActual,
						@fechaActual,
					    @horaActual,
						'',
						'N',
						@CodigoOrden  
						
					 --Confirma Operacion de amortización de swap bonos
					EXEC OrdenInversion_ConfirmarOI @CodigoOrdenOP2, @CodigoPortafolio, @CodigoOrdenOP2,'' 
					 
					UPDATE 
						Cuponera_Bono_Swap 
					SET 
						GeneradoAmortizarOriginal = '1' 
					WHERE 
						Correlativo = @Correlativo  
						AND CodigoOrden = @CodigoOrden
				  END  
			  END  
				 FETCH NEXT FROM 
					CursorCuponeraLEG1 
				 INTO 
					@Correlativo,
					@CodigoOrden,
					@CodigoPortafolio,
					@MontoInteres,
					@MontoAmortizacion,
					@MontoNominal,
					@TipoCambio,
					@CodigoMoneda,
					@CodigoIsin,
					@Tasa,
					@FechaOperacion,
					@CodigoTercero,
					@GeneradoInteres,
					@GeneradoAmortizar 
			END  
		CLOSE CursorCuponeraLEG1  
		DEALLOCATE CursorCuponeraLEG1

END  

GO

GRANT EXECUTE ON [dbo].[sp_SIT_GeneraVencimientosBono_Swap] TO [rol_sit_fondos] AS [dbo]
GO



PRINT '[dbo].[ObtenerReporteVL]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[ObtenerReporteVL]    Script Date: 08/11/2019 20:56:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerReporteVL]') AND type in (N'P', N'PC'))
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
-----------------------------------------------------------------------------------------------------------
-- Fecha Modificación: 12/04/2019    
-- Modificado por: Ernesto Galarza    
-- Nro. Orden de Trabajo: 11851   
-- Descripcion del cambio: Se modifico el redondeo, en monto nominal de 2 decimales a 6
---------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[ObtenerReporteVL] @Fecha numeric(8), @p_Privados Numeric(1)
AS
BEGIN
	DECLARE @Correlativo INT
	SELECT @Correlativo = Correlativo FROM CorrelativoVL WHERE Fecha = @Fecha 
	SELECT RVL.TipoRegistro,RVL.Administradora,CASE WHEN RVL.Fondo = '' THEN P.Descripcion  ELSE RVL.Fondo  END AS Fondo ,RVL.Fecha,RVL.TipoCodigoValor,RVL.CodigoValor,
	RVL.IdentificadorOperacion,RVL.FormaValorizacion,ROUND(RVL.MontoNominal,6) MontoNominal ,
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


PRINT '[dbo].[sp_SIT_FormaValorizacion1]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[sp_SIT_FormaValorizacion1]    Script Date: 08/11/2019 20:56:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_FormaValorizacion1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_FormaValorizacion1]
GO


------------------------------------------------------------------------------------------------------------------------------------- 
--Objetivo: Guarda los registros de reporte VL para Certificado de dep├│sito, Papeles comerciales, 
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
--	Descripcion del cambio: C├ílculo de amortizaci├│n
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 25/09/2017
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 10813
--	Descripcion del cambio: C├ílculo de amortizaci├│n
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
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecuci├│n del tipo de cambio
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 31/08/2018
--	Modificado por: Ian Pastor Mendoza
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecuci├│n del VECTOR PRECIO
------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha modificacion: 16/11/2018  
-- Modificado por: Ricardo Colonia  
-- Descripcion del cambio: Agregar funcionalidad de secuencia de ejecuci├│n del tipo de cambio en fn_TipoCambioVL
-------------------------------------------------------------------------------------------------------------------------------------  
--	Fecha modificacion: 15/01/2019
--	Modificado por: Ernesto Galarza
--	Nro. Orden de Trabajo: 11636
--	Descripcion del cambio: Se modifico para que contemple las ordenes que pertecen a la clase facturas negociables
------------------------------------------------------------------------------------------------------------------------------------- 
--	Fecha modificacion: 13/03/2019
--	Modificado por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11908
--	Descripcion del cambio: Se elimina restricci├│n de generaci├│n de interesescorridocompra cuando la orden es CD
------------------------------------------------------------------------------------------------------------------------------------- 
-- Fecha Modificación: 12/04/2019    
-- Modificado por: Ernesto Galarza    
-- Nro. Orden de Trabajo: 11851   
-- Descripcion del cambio: Se modifico el redondeo, en monto nominal de 2 decimales a 6
---------------------------------------------------------------------------------------------------------------------  
CREATE PROC [dbo].[sp_SIT_FormaValorizacion1]
(
	@p_Fecha NUMERIC(8)
	,@p_CodigoPortafolioSBS VARCHAR(10)
)
AS
BEGIN
	--Forma valorizacion 1, Certificado de dep├│sito, Papeles comerciales, Acciones y Bonos	
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
		OT10813 - El c├ílculo del valor nominal de bonos es incorrecto. El c├ílculo real es las unidades disponibles por
				  por el valor unitario y por sus amortizaciones pendientes.
		MontoNominal =	CASE WHEN  CI.Categoria = 'AC' OR  CI.Categoria = 'FM' OR CI.Categoria = 'FI' THEN ROUND(CTV.Cantidad,2) 
							WHEN CI.Categoria = 'WA' THEN (CASE WHEN M.TipoCalculo = 'D' THEN V.ValorNominal * VTCo.ValorPrimario ELSE V.ValorNominal / VTCo.ValorPrimario END) --9683
							--WHEN (SELECT COUNT(1) FROM ParametrosGenerales WHERE Clasificacion = 'INST_AMORT' AND Valor = V.CodigoNemonico) > 0 THEN (CTV.ValorNominalMonedaOrigen * (dbo.CalcularSumaAmortizacionPendiente(ctv.FechaValoracion,V.CodigoNemonico)/100) * dbo.Reto
rnarTipoCambioT_1(p.CodigoMoneda,'REAL',ctv.FechaValoracion) )
							WHEN (SELECT COUNT(1) FROM ParametrosGenerales WHERE Clasificacion = 'INST_AMORT' AND Valor = V.CodigoNemonico) > 0 THEN (CTV.ValorNominalMonedaOrigen * (dbo.CalcularSumaAmortizacionPendiente(ctv.FechaValoracion,V.CodigoNemonico)/100) )
						ELSE ROUND(CTV.ValorNominalMonedaOrigen  - ISNULL(dbo.NominalLiberado(P.CodigoPortafolioSBS ,V.CodigoISIN,@p_Fecha,'1'),0),2) END, ----9
		*/
		MontoNominal =	CASE WHEN  CI.Categoria = 'AC' OR  CI.Categoria = 'FM' OR CI.Categoria = 'FI' THEN ROUND(CTV.Cantidad,6) 
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


PRINT '[dbo].[sp_SIT_ins_PrecioValorCuota]'
USE [SIT-FONDOS]
GO

/****** Object:  StoredProcedure [dbo].[sp_SIT_ins_PrecioValorCuota]    Script Date: 04/12/2019 16:00:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SIT_ins_PrecioValorCuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SIT_ins_PrecioValorCuota]
GO


-----------------------------------------------------------------------------------------------------------
--Objetivo: Insertar los valores cuotas calculados en le tabla de precios cuando valoriza.
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/01/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11636
--	Descripci├│n del cambio: Creaci├│n
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 23/03/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11908
--	Descripci├│n del cambio: Se cambia el c├ílculo del monto nominal teniendo en cuenta las amortizaciones 
--							en el precio devengado.
-----------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 03/04/2019
--	Craedo por: Ricardo Colonia
--	Nro. Orden de Trabajo: 11851
--	Descripcion del cambio: Se cambia el monto nominal desde la misma fecha de amortización.
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
	
	-- Generaci┬¥n de Precios para Instrumentos con Flag Precio Devengado
	DECLARE
		@tmpInstrumentosDevengados TABLE(
										  codigoMnemonico VARCHAR(20),
										  montoNominal DECIMAL(22,7),
										  interes DECIMAL(22,7),
										  cantidad DECIMAL(22,7),
										  codigoTipoCupon VARCHAR(4),
										  codigoIsin VARCHAR(20)
										)
	--DECLARE
	--		@p_CodigoPortafolioSBS VARCHAR(12)='47' , 
	--	@p_FechaProceso NUMERIC(8)=20190314
	DECLARE
		@FechaDiaSiguiente DATETIME = CONVERT(DATETIME, CONVERT(CHAR(8), @p_FechaProceso)),
		@FechaT1 DECIMAL(8)
	
		SET @FechaDiaSiguiente = DATEADD(DAY,1, @FechaDiaSiguiente)
		SET @FechaT1 =(SELECT YEAR(@FechaDiaSiguiente) * 10000 + MONTH(@FechaDiaSiguiente) * 100 + DAY(@FechaDiaSiguiente))	
		
	INSERT INTO 
		@tmpInstrumentosDevengados 
	SELECT 
		VL.CodigoNemonico,
	--	ISNULL(VL.MontoNominal,0),
		ISNULL(CTV.Cantidad * VA.ValorUnitario * (dbo.CalcularSumaAmortizacionPendiente(@FechaT1,VA.CodigoNemonico)/100),0),
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

PRINT '[dbo].[DividendosRebatesLiberadas_ConfirmarVencimiento]'
USE [SIT-FONDOS]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DividendosRebatesLiberadas_ConfirmarVencimiento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DividendosRebatesLiberadas_ConfirmarVencimiento]
GO

----------------------------------------------------------------------------------------------------------------------
--Objetivo: Reporte de Composicion de Cartera por Instrumento Empresa
----------------------------------------------------------------------------------------------------------------------
--	Fecha Creacion: 16/08/2018
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11512
--	Descripcion del cambio: Reemplazar la función "dbo.retornarPrecioVector" por "dbo.RetornarVectorPrecioT_1"
----------------------------------------------------------------------------------------------------------------------
-- Fecha Creacion: 11/08/2018  
-- Creado por: Ricardo Colonia  
-- Nro. Orden de Trabajo: 11547  
-- Descripción del cambio: Se considera el máximo número operación de CuentasPorCobrarPagar para la inserción
----------------------------------------------------------------------------------------------------------------------  
--	Fecha Creacion: 16/08/2018
--	Creado por: Ian Pastor M.
--	Nro. Orden de Trabajo: 11590
--	Descripcion del cambio: Agregar funcionalidad de secuencia de ejecución del VECTOR PRECIO
----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DividendosRebatesLiberadas_ConfirmarVencimiento](
	@p_CodigoPortafolioSBS VARCHAR(20),
	@p_CodigoNemonico VARCHAR(15),
	@p_FechaVencimiento NUMERIC(8),
	@p_MontoNominalLocal NUMERIC(22,7),
	@p_Identificador VARCHAR(50),
	@p_UsuarioModificacion VARCHAR(15),
	@p_FechaModificacion NUMERIC(8),
	@p_HoraModificacion VARCHAR(10),
	@p_Host   VARCHAR(20)
)
AS
BEGIN
	DECLARE @CodigoMoneda VARCHAR(10), @DescripcionValor VARCHAR(50), @Tipo VARCHAR(1),@CodigoOrden NVARCHAR(12),
	        @CodigoTercero VARCHAR(11),@CodigoMercado VARCHAR(3), @FechaEntrega  NUMERIC(8), @Factor NUMERIC(22,7),
	        @CodigoCustodio VARCHAR(12), @FechaCorte NUMERIC(8), @FechaOperacion NUMERIC(8), @VectorPrecioVal VARCHAR(4)
	
	SET @VectorPrecioVal = (dbo.RetornarSecuenciaFuenteVP(@p_CodigoPortafolioSBS,@p_CodigoNemonico,@p_FechaVencimiento))
	
	IF ISNULL(@VectorPrecioVal,'') = ''
	BEGIN
		RAISERROR('No se encuentra definido el precio vector del portafolio. Aségure de asignarle un valor en el Mantenimiento de Portafolios',16,1)
		RETURN
	END
	
	UPDATE DividendosRebatesLiberadas 
	   SET MontoNominalLocal = @p_MontoNominalLocal,
	       Estado = 'E-CON',
	       @Tipo = TipoDistribucion,
	       @CodigoMoneda = CodigoMoneda,
	       @FechaEntrega = FechaEntrega,
	       @Factor = Factor,
	       @FechaCorte = FechaCorte,
	       @FechaOperacion = FechaIDI
	WHERE Identificador = CONVERT(NUMERIC(9), @p_Identificador ) 
	SET @CodigoOrden = @Tipo + @p_Identificador
	
	--> Las Liberadas no generan CxCxP
	IF @Tipo='L' 
	BEGIN
		--> Actualiza los Saldos de Cartera
		IF EXISTS(SELECT * from saldoscarteratitulo where CodigoMnemonico=@p_CodigoNemonico and CodigoPortafolioSBS = @p_CodigoPortafolioSBS and Fechasaldo=@p_FechaVencimiento)
			UPDATE SaldosCarteraTitulo set SaldoUnidadesLiberadas=@p_MontoNominalLocal 
			where CodigoMnemonico=@p_CodigoNemonico and CodigoPortafolioSBS = @p_CodigoPortafolioSBS and Fechasaldo=@p_FechaVencimiento
		ELSE
			INSERT INTO SaldosCarteratitulo 
			VALUES(@p_CodigoPortafolioSBS,1,@p_CodigoNemonico,@p_FechaVencimiento,0,0,0,@p_MontoNominalLocal,0,0,@p_UsuarioModificacion, @p_FechaModificacion,@p_HoraModificacion,@p_UsuarioModificacion, @p_FechaModificacion,@p_HoraModificacion,@p_Host,0)
		--> CodigoOperacion '37' Dividendos de Acciones
		--> Si existe OI, la marca como Confirmada sino crea nueva
		IF EXISTS(SELECT 1 from OrdenInversion where CodigoOrden = @CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND CodigoOperacion = '37' )
			UPDATE OrdenInversion SET Estado = 'E-CON' WHERE CodigoOrden = @CodigoOrden 
		ELSE
			INSERT INTO OrdenInversion (CodigoPortafolioSBS, CodigoOrden, FechaOperacion, FechaLiquidacion, FechaContrato,
			MontoNominalOperacion, CodigoTipoCupon, MontoOperacion, TipoCambio, CodigoMoneda, CodigoMonedaDestino, 
			MontoNetoOperacion, Estado, Situacion, CodigoISIN, CodigoSBS, CodigoTercero, CodigoOperacion, CodigoMnemonico,
			CodigoTipoTitulo, CategoriaInstrumento, Negociacion, TasaPorcentaje, VPNLocal, Precio, CantidadOperacion,
			FechaCreacion, UsuarioCreacion, HoraCreacion, Host, TipoFondo, Ficticia)
			SELECT @p_CodigoPortafolioSBS, @CodigoOrden, @FechaOperacion, @FechaEntrega, @p_FechaVencimiento,
			@p_MontoNominalLocal, '', @p_MontoNominalLocal, 1, @CodigoMoneda, @CodigoMoneda, @p_MontoNominalLocal,
			'E-CON', 'A', v.CodigoISIN, v.CodigoSBS, e.CodigoTercero, '37', v.CodigoNemonico, v.CodigoTipoTitulo,
			ci.Categoria,'N',  0,  0,dbo.RetornarVectorPrecioT_1(@p_CodigoNemonico,@VectorPrecioVal,@p_FechaVencimiento), @p_MontoNominalLocal,@p_FechaModificacion,@p_UsuarioModificacion,
			@p_HoraModificacion,@p_Host,'', ''
			FROM Valores v
			INNER JOIN TipoInstrumento ti ON v.CodigoTipoInstrumentoSBS = ti.CodigoTipoInstrumentoSBS
			INNER JOIN ClaseInstrumento ci ON ti.CodigoClaseInstrumento = ci.CodigoClaseInstrumento
			INNER JOIN Entidad e ON v.CodigoEmisor = e.CodigoEntidad
			WHERE CodigoNemonico = @p_CodigoNemonico
		--> Actualiza los Saldos de Custodio   
		--LETV 20090525 La logica esta en GeneraSaldosCustodioPorNemonico
		SELECT @CodigoTercero = e.codigotercero from valores va
		INNER JOIN entidad e on va.CodigoEmisor = e.codigoentidad
		where va.situacion = 'A'  and va.codigonemonico =@p_CodigoNemonico
		SELECT @CodigoCustodio = dbo.ObtenerCustodio(@p_CodigoPortafolioSBS,@p_CodigoNemonico,@CodigoTercero)
		  EXEC GeneraSaldosCustodioPorNemonico @p_FechaVencimiento ,@p_CodigoNemonico,@CodigoCustodio,@p_CodigoPortafolioSBS,@p_UsuarioModificacion,@p_FechaModificacion,@p_HoraModificacion,@p_Host
	END
	ELSE
	BEGIN
		--> Los Dividendos en efectivo NO actualizan los Saldos de Cartera
		--> Obtiene datos 
		SELECT @CodigoTercero = e.CodigoTercero,
		       @CodigoMercado = e.codigoMercado,
		       @DescripcionValor = v.Descripcion
		FROM Valores v 
		INNER JOIN Entidad e ON E.CodigoEntidad = V.CodigoEmisor 
		WHERE CodigoNemonico = @p_CodigoNemonico
		--> Si existe OI, la marca como Confirmada sino crea nueva
		IF EXISTS(SELECT * from OrdenInversion where CodigoOrden = @CodigoOrden AND CodigoPortafolioSBS = @p_CodigoPortafolioSBS AND CodigoOperacion = '36' )
			UPDATE OrdenInversion SET Estado = 'E-CON' WHERE CodigoOrden = @CodigoOrden
		ELSE
		BEGIN
			--CA
			DECLARE @Saldo NUMERIC(22,7)
			SELECT @Saldo = SALDODISPONIBLE + SALDOPROCESOCOMPRA - SALDOPROCESOVENTA FROM SaldosCarteraTitulo WHERE CodigoMnemonico = @p_CodigoNemonico 
			AND FECHASALDO = @FechaCorte AND CODIGOPORTAFOLIOSBS = @p_CodigoPortafolioSBS 
			INSERT INTO OrdenInversion(CodigoPortafolioSBS,  CodigoOrden,  FechaOperacion,FechaLiquidacion,  FechaContrato,  MontoNominalOperacion,
			CodigoTipoCupon,  MontoOperacion,  TipoCambio,CodigoMoneda,  CodigoMonedaDestino,  MontoNetoOperacion, 
			Estado,  Situacion,  CodigoISIN,CodigoSBS,  CodigoTercero,  CodigoOperacion,CodigoMnemonico,  CodigoTipoTitulo,  CategoriaInstrumento,
			Negociacion,  TasaPorcentaje,  Precio,  FechaCreacion,UsuarioCreacion,HoraCreacion,Host,TipoFondo, Ficticia, CantidadOperacion ,CantidadOrdenado 	)
			SELECT @p_CodigoPortafolioSBS,  @CodigoOrden,  @p_FechaVencimiento,@FechaEntrega,  @p_FechaVencimiento,  0, '',  @p_MontoNominalLocal,  1, 
			@CodigoMoneda,  @CodigoMoneda,  @p_MontoNominalLocal,'E-CON',  'A',  v.CodigoISIN, v.CodigoSBS,  e.CodigoTercero, '36', 
			v.CodigoNemonico, v.CodigoTipoTitulo, ci.Categoria,'N',  0,  @Factor,@p_FechaModificacion,@p_UsuarioModificacion,@p_HoraModificacion,@p_Host ,'', '',
			@Saldo,@Saldo
			FROM Valores v 
			INNER JOIN TipoInstrumento ti ON v.CodigoTipoInstrumentoSBS = ti.CodigoTipoInstrumentoSBS
			INNER JOIN ClaseInstrumento ci ON ti.CodigoClaseInstrumento = ci.CodigoClaseInstrumento
			INNER JOIN Entidad e ON v.CodigoEmisor = e.CodigoEntidad
			WHERE CodigoNemonico =  @p_CodigoNemonico
		END
		--> Agrega Nueva CxCxP. Operacion '36' Dividendo en Efectivo
		DECLARE  @numeroCustodios int,@MontoNomimalDividido NUMERIC(22,7),@NumeroCuotasUnidades decimal,@CONST_CodigoOrden VARCHAR(12),
		@CodigoMercadoCustodio int
		SET @CONST_CodigoOrden = @CodigoOrden
		SELECT @numeroCustodios=count(CodigoCustodio) FROM CustodioValores WHERE CodigoNemonico = @p_CodigoNemonico AND  CodigoPortaFolioSBS = @p_CodigoPortafolioSBS
		Declare @LiquidaFechaAnt VARCHAR(1),@UsuarioLiqFecAnt VARCHAR(15)
		if (SELECT COUNT(1) from tmpOrdenLiquidaFechaAnt where NumeroOperacion = @CONST_CodigoOrden and CodigoPortafolioSBS = @p_CodigoPortafolioSBS) > 0
		BEGIN
			set @LiquidaFechaAnt = '1'
			set @UsuarioLiqFecAnt = @p_UsuarioModificacion
		END
		ELSE
		BEGIN
			set @LiquidaFechaAnt = NULL
			set @UsuarioLiqFecAnt = NULL
		END
		IF @numeroCustodios >1
		BEGIN 
			DECLARE NumeroCustodios_cursor CURSOR FOR 
			SELECT CodigoCustodio FROM CustodioValores WHERE CodigoNemonico = @p_CodigoNemonico AND  CodigoPortaFolioSBS = @p_CodigoPortafolioSBS
			OPEN NumeroCustodios_cursor
			FETCH NEXT FROM NumeroCustodios_cursor INTO @CodigoCustodio
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @CodigoOrden = ''
				SELECT  @CodigoOrden = Max(Convert(decimal ,Substring(NumeroOperacion,2,10))) + 1 FROM CuentasPorCobrarPagar  WHERE LEFT(NumeroOperacion,1) = 'D'
				SET @CodigoOrden = @Tipo + @CodigoOrden
				SELECT  @FechaCorte=fechaCorte FROM DividendosRebatesLiberadas WHERE Identificador=@p_Identificador
				SET @NumeroCuotasUnidades = dbo.GetSaldoDisponibleValorCustodio(@p_CodigoPortafolioSBS,@p_CodigoNemonico,@FechaCorte,@CodigoCustodio)   
			    SET @MontoNomimalDividido =  @NumeroCuotasUnidades*@factor
				IF @MontoNomimalDividido > 0
				BEGIN 
					SET @MontoNomimalDividido = @p_MontoNominalLocal
					INSERT INTO CuentasPorCobrarPagar(CodigoPortafolioSBS,  NumeroOperacion, CodigoOperacion,Referencia, CodigoOrden, Importe,FechaIngreso,
					CodigoClaseCuenta, NumeroCuenta,CodigoMoneda, CodigoMercado, FechaOperacion,FechaPago, TipoMovimiento, Egreso,Situacion, UsuarioCreacion, 
					HoraCreacion, FechaCreacion, UsuarioModificacion, Host, FechaModificacion, HoraModificacion, CodigoTercero,Estado, FechaVencimiento, NumeroAsiento, 
					FechaAsiento, Identificador, CodigoNemonico,LiquidaFechaAnt,UsuarioLiqFecAnt)
					VALUES(@p_CodigoPortafolioSBS,  @CodigoOrden,'36',@DescripcionValor,  @CONST_CodigoOrden,  @MontoNomimalDividido,@p_FechaVencimiento,NULL,NULL,
					case @CodigoMoneda when 'VAC' then 'NSOL' else @CodigoMoneda end, case @CodigoCustodio WHEN 'BBH' THEN 2 WHEN 'CAVALI' THEN 1 END, @FechaEntrega,
					NULL,  'A',  'N','A',  NULL,  NULL,NULL,  NULL,  NULL,NULL,  NULL,  @CodigoTercero,NULL,  @FechaEntrega,   NULL,NULL,@p_Identificador,@p_CodigoNemonico,
					@LiquidaFechaAnt,@UsuarioLiqFecAnt)
				END
				FETCH NEXT FROM NumeroCustodios_cursor INTO @CodigoCustodio   
			END
			CLOSE NumeroCustodios_cursor 
			DEALLOCATE NumeroCustodios_cursor 
		END
		ELSE
		BEGIN
		        SET @CodigoOrden = ''  
		        SELECT  @CodigoOrden = Max(Convert(decimal ,Substring(NumeroOperacion,2,10))) + 1 FROM CuentasPorCobrarPagar  WHERE LEFT(NumeroOperacion,1) = 'D'  
		        SET @CodigoOrden = @Tipo + @CodigoOrden  
			INSERT INTO CuentasPorCobrarPagar(CodigoPortafolioSBS,  NumeroOperacion, CodigoOperacion,Referencia,
			CodigoOrden, Importe,FechaIngreso, CodigoClaseCuenta,NumeroCuenta,CodigoMoneda, CodigoMercado,
			FechaOperacion,FechaPago, TipoMovimiento, Egreso,Situacion, UsuarioCreacion, HoraCreacion,
			FechaCreacion,UsuarioModificacion, Host,FechaModificacion, HoraModificacion, CodigoTercero,
			Estado, FechaVencimiento, NumeroAsiento,FechaAsiento, Identificador,
			CodigoNemonico,LiquidaFechaAnt,UsuarioLiqFecAnt)
			VALUES(@p_CodigoPortafolioSBS, @CodigoOrden, '36',@DescripcionValor, @CONST_CodigoOrden,
			@p_MontoNominalLocal,@p_FechaVencimiento,NULL,NULL,
			case @CodigoMoneda when 'VAC' then 'NSOL' else @CodigoMoneda end,
			 @CodigoMercado, @FechaEntrega,NULL, 'A','N','A', NULL,  NULL,
			NULL,  NULL,  NULL,NULL,  NULL,  @CodigoTercero,NULL, @FechaEntrega,NULL,NULL, @p_Identificador,  @p_CodigoNemonico,@LiquidaFechaAnt,@UsuarioLiqFecAnt)
		END
	END
END

GO

GRANT EXECUTE ON [dbo].[DividendosRebatesLiberadas_ConfirmarVencimiento] TO [rol_sit_fondos] AS [dbo]
GO





PRINT 'FIN --- > SECCIÓN DE SP'

IF @@ERROR <> 0 BEGIN
    ROLLBACK TRANSACTION __Transaction_SP
    PRINT 'ERROR: ROLLBACK SP'
END
ELSE
    COMMIT TRANSACTION __Transaction_SP
GO 

USE [SIT-FONDOS]
GO

BEGIN TRANSACTION __Transaction_Log_2
		
		PRINT '---------- INICIO UPDATE TABLES ----------'
		DECLARE @CANT INT = 0,
				@I INT = 1,
				@nombreTabla VARCHAR(50) = ''
				
		CREATE TABLE #tablaNemonico (
									  nemonicoNew VARCHAR(15),
									  nemonicoOld VARCHAR(15)
									 )
									 
		DECLARE @tablaListaCodNemonico TABLE (
											  id INT IDENTITY(1,1),  
											  nombreTabla VARCHAR(50)
											 )
		
		DECLARE @tablaListaCodMnemonico TABLE (
											  id INT IDENTITY(1,1),  
											  nombreTabla VARCHAR(50)
											 )
		PRINT 'LISTA DE MNEMONICOS ACTUALIZAR'	
		PRINT '------------------------------'								 
		INSERT INTO 
			#tablaNemonico (nemonicoNew,nemonicoOld)
		VALUES
			('AAXJ US','AAXJ '),
			('DXJ US','DXJ'),
			('ECH US','ECH'),
			('EDEN US','EDEN'),
			('EEM US','EEM'),
			('EIDO US','EIDO'),
			('EMLC US','EMLC'),
			('EPHE US','EPHE'),
			('EPOL US','EPOL'),
			('EPU US','EPU'),
			('EWA US','EWA'),
			('EWC US','EWC'),
			('EWD US','EWD'),
			('EWH US','EWH'),
			('EWI US','EWI'),
			('EWJ US','EWJ'),
			('EWL US','EWL'),
			('EWM US','EWM'),
			('EWN US','EWN'),
			('EWQ US','EWQ'),
			('EWS US','EWS'),
			('EWT US','EWT'),
			('EWU US','EWU'),
			('EWW US','EWW'),
			('EWY US','EWY'),
			('EWZ US','EWZ'),
			('EZA US','EZA'),
			('EZU US','EZU'),
			('GXG US','GXG'),
			('HEWJ US','HEWJ'),
			('IEI US','IEI'),
			('IEMG US','IEMG'),
			('IEV US','IEV'),
			('IGSEUIU LX','IGSEUIU'),
			('ILF US','ILF'),
			('INDA US','INDA'),
			('IVV US','IVV'),
			('MCHI US','MCHI'),
			('SHY US','SHY'),
			('SLQD US','SLQD'),
			('THD US','THD'),
			('TUR US','TUR'),
			('VGK US','VGK'),
			('VOO US','VOO'),
			('VWO US','VWO'),
			('XLB US','XLB'),
			('XLE US','XLE'),
			('XLF US','XLF'),
			('XLI US','XLI'),
			('XLK US','XLK'),
			('XLP US','XLP'),
			('XLRE US','XLRE'),
			('XLU US','XLU'),
			('XLY US','XLY'),
			('IEF US','IEF')  --> Ingresado el 11/02/19


	
		PRINT 'LISTA DE TABLAS ACTUALIZAR CON COLUMNA: CodigoNemonico'	
		PRINT '------------------------------------------------------'
		INSERT INTO 
			@tablaListaCodNemonico (nombreTabla)
		VALUES 
			('Valores'),
			('Bono_Swap'),
			('CalculoRebates'),
			('CodigoValor'),
			('CuentasPorCobrarPagar'),
			('CuentasPorCobrarPagarTemp'),
			('CuponeraNormal'),
			('CuponeraPagos'),
			('DistribucionLib'),
			('CustodioValores'),
			('DividendosRebatesLiberadas'),
			('Emisiones_Bloomberg'),
			('GrupoPorNemonico'),
			('InstrumentosEstructurados'),
			('LimiteValores'),
			('PatrimonioFideicomisoDetalle'),
			('PrevOrdenInversion'),
			('ReporteVL'),
			('tmp_CuentasPorCobrarPagar'),
			('ValorizacionAmortizada'),
			('VectorPrecioPIP')
			
		PRINT 'LISTA DE TABLAS ACTUALIZAR CON COLUMNA: CodigoMnemonico'
		PRINT '-------------------------------------------------------'
		INSERT INTO 
			@tablaListaCodMnemonico (nombreTabla)
		VALUES 
			('AumentoCapitalDetalle'),
			('BCRSeriados'),
			('CarteraTitulo'),
			('CarteraTituloValoracion'),
			('CriterioLiquidezAcciones'),
			('CustodioAjuste'),
			('CustodioKardex'),
			('CustodioSaldo'),
			('CustodioSaldoBBH'),
			('CustodioSaldoTMP'),
			('Factor'),
			('OrdenInversion'),
			('SaldosCarteraTitulo'),
			('TasaEncaje'),
			('ValoresTemporal'),
			('VectorPrecio')

		PRINT '-> UPDATE TABLE [PatrimonioTercero] CONSTRAINT [FK_PatrimonioTercero_Valores] ON DELETE CASCADE ON UPDATE CASCADE'
		IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatrimonioTercero_Valores]') AND parent_object_id = OBJECT_ID(N'[dbo].[PatrimonioTercero]'))
			ALTER TABLE [dbo].[PatrimonioTercero] DROP CONSTRAINT [FK_PatrimonioTercero_Valores]
		ALTER TABLE [dbo].[PatrimonioTercero] WITH CHECK ADD CONSTRAINT [FK_PatrimonioTercero_Valores] FOREIGN KEY([CodigoMnemonico])
		REFERENCES [dbo].[Valores] ([CodigoNemonico]) ON DELETE CASCADE ON UPDATE CASCADE
		
		PRINT '-> UPDATE TABLE [TipoPatrimonioValor] CONSTRAINT [FK_PatrimonioTercero_Valores] ON DELETE CASCADE ON UPDATE CASCADE'
		IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TipoPatrimonioValor_Valores]') AND parent_object_id = OBJECT_ID(N'[dbo].[TipoPatrimonioValor]'))
			ALTER TABLE [dbo].[TipoPatrimonioValor] DROP CONSTRAINT [FK_TipoPatrimonioValor_Valores]
		ALTER TABLE [dbo].[TipoPatrimonioValor]  WITH NOCHECK ADD CONSTRAINT [FK_TipoPatrimonioValor_Valores] FOREIGN KEY([CodigoMnemonico])
		REFERENCES [dbo].[Valores] ([CodigoNemonico]) ON DELETE CASCADE ON UPDATE CASCADE
		
		SET @CANT = (SELECT COUNT(*) FROM @tablaListaCodNemonico)
		PRINT 'TABLAS CON COLUMNAS: CodigoNemonico'
		PRINT '-----------------------------------'
		WHILE @I <= @CANT
		BEGIN
			SET @nombreTabla = (SELECT nombreTabla 
								FROM @tablaListaCodNemonico
								WHERE ID = @I)
			PRINT '-> ' + CAST(@I AS VARCHAR(2)) + ' - UPDATE TABLE ' + @nombreTabla
			EXEC (
				  'UPDATE ' + 
						@nombreTabla + 
				  ' SET ' +
					    'CodigoNemonico = TN.nemonicoNew ' + 
				  ' FROM ' +
						@nombreTabla + ' A ' +
				  ' JOIN ' +
						'#tablaNemonico TN ON A.CodigoNemonico = TN.nemonicoOld'
				 )
			SET @I = @I + 1
		END
		
		PRINT 'TABLAS CON COLUMNAS: CodigoMnemonico'
		PRINT '------------------------------------'
		SET @CANT = (SELECT COUNT(*) FROM @tablaListaCodMnemonico)
		SET @I = 1
		
		WHILE @I <= @CANT
		BEGIN
			SET @nombreTabla = (SELECT nombreTabla 
								FROM @tablaListaCodMnemonico
								WHERE ID = @I)
			PRINT '-> ' + CAST(@I AS VARCHAR(2)) + ' - UPDATE TABLE ' + @nombreTabla
			EXEC ('UPDATE ' + 
						@nombreTabla + 
				  ' SET ' +
					    'CodigoMnemonico = TN.nemonicoNew ' + 
				  ' FROM ' +
						@nombreTabla + ' A ' +
				  ' JOIN ' +
						'#tablaNemonico TN ON A.CodigoMnemonico = TN.nemonicoOld'
				 )
			SET @I = @I + 1
		END
	
		PRINT '-> UPDATE TABLE [PatrimonioTercero] CONSTRAINT [FK_PatrimonioTercero_Valores]'
		IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatrimonioTercero_Valores]') AND parent_object_id = OBJECT_ID(N'[dbo].[PatrimonioTercero]'))
			ALTER TABLE [dbo].[PatrimonioTercero] DROP CONSTRAINT [FK_PatrimonioTercero_Valores]
		ALTER TABLE [dbo].[PatrimonioTercero] WITH CHECK ADD CONSTRAINT [FK_PatrimonioTercero_Valores] FOREIGN KEY([CodigoMnemonico])
		REFERENCES [dbo].[Valores] ([CodigoNemonico])
		
		PRINT '-> UPDATE TABLE [TipoPatrimonioValor] CONSTRAINT [FK_PatrimonioTercero_Valores]'
		IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TipoPatrimonioValor_Valores]') AND parent_object_id = OBJECT_ID(N'[dbo].[TipoPatrimonioValor]'))
			ALTER TABLE [dbo].[TipoPatrimonioValor] DROP CONSTRAINT [FK_TipoPatrimonioValor_Valores]
		ALTER TABLE [dbo].[TipoPatrimonioValor]  WITH NOCHECK ADD CONSTRAINT [FK_TipoPatrimonioValor_Valores] FOREIGN KEY([CodigoMnemonico])
		REFERENCES [dbo].[Valores] ([CodigoNemonico])
		
		DROP TABLE #tablaNemonico		
		PRINT '---------- FIN UPDATE TABLES ----------'
		
IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log_2
ELSE
    COMMIT TRANSACTION __Transaction_Log_2
GO 