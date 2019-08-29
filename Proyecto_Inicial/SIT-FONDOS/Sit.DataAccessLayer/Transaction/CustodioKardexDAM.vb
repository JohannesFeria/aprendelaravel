Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class CustodioKardexDAM

    Private sqlCommand As String = ""
    Private oCustodioKardexRow As CustodioKardexBE.CustodioKardexRow

    Public Function Insertar(ByVal ob As CustodioKardexBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "custodioKardex_Insertar"
        Dim dbCommand As dbCommand = db.GetStoredProcCommand(sqlCommand)
        oCustodioKardexRow = CType(ob.CustodioKardex.Rows(0), CustodioKardexBE.CustodioKardexRow)

        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, oCustodioKardexRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@NumeroTitulo", DbType.Decimal, oCustodioKardexRow.NumeroTitulo)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oCustodioKardexRow.CodigoISIN)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, oCustodioKardexRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@NumeroOperacion", DbType.Decimal, oCustodioKardexRow.NumeroOperacion)
        db.AddInParameter(dbCommand, "@NumeroMovimiento", DbType.Decimal, oCustodioKardexRow.NumeroMovimiento)
        db.AddInParameter(dbCommand, "@FechaMovimiento", DbType.Decimal, oCustodioKardexRow.FechaMovimiento)
        db.AddInParameter(dbCommand, "@HoraMovimiento", DbType.String, oCustodioKardexRow.HoraMovimiento)
        db.AddInParameter(dbCommand, "@TituloNumeroLocalizacion", DbType.String, oCustodioKardexRow.TituloNumeroLocalizacion)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, oCustodioKardexRow.CodigoCustodio)
        db.AddInParameter(dbCommand, "@CtaDepositaria", DbType.String, oCustodioKardexRow.CtaDepositaria)
        db.AddInParameter(dbCommand, "@CantidadUnidades", DbType.Currency, oCustodioKardexRow.CantidadUnidades)
        db.AddInParameter(dbCommand, "@ValorOrigenMovimiento", DbType.Currency, oCustodioKardexRow.ValorOrigenMovimiento)
        db.AddInParameter(dbCommand, "@ValorLocalMovimiento", DbType.Currency, oCustodioKardexRow.ValorLocalMovimiento)
        db.AddInParameter(dbCommand, "@TipoMovimiento", DbType.String, oCustodioKardexRow.TipoMovimiento)
        db.AddInParameter(dbCommand, "@NumeroLamina", DbType.String, oCustodioKardexRow.NumeroLamina)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, oCustodioKardexRow.UsuarioCreacion)
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, oCustodioKardexRow.FechaCreacion)
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, oCustodioKardexRow.HoraCreacion)
        db.AddInParameter(dbCommand, "@Host", DbType.String, oCustodioKardexRow.Host)

        db.ExecuteNonQuery(dbCommand)

    End Function

End Class
