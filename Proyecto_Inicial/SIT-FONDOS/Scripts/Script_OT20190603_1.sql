----------------------------------------------------------------------------------------------------------------------------------------
--Objetivo: ACTUALIZAR MONTO DE LIQUIDACIÓN FACTURA NEGOCIABLE
----------------------------------------------------------------------------------------------------------------------------------------
-- Fecha de Creación		: 03/06/2019
-- Modificado por			: Ian Pastor M.
-- Nro. Orden de Trabajo	: 20190603
-- Descripción				: -
----------------------------------------------------------------------------------------------------------------------------------------
USE [SIT-FONDOS]
GO
BEGIN TRANSACTION __Transaction_Log

update OrdenInversion 
set MontoNetoOperacion=7913.94 
where CodigoOrden = '066253'
go

update CuentasPorCobrarPagar
set Importe =7913.94 
where CodigoOrden = '066253'
go

IF @@ERROR <> 0
    ROLLBACK TRANSACTION __Transaction_Log
ELSE
    COMMIT TRANSACTION __Transaction_Log
GO 