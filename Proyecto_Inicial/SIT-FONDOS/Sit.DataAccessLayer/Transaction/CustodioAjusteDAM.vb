Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class CustodioAjusteDAM

    Private sqlCommand As String = ""
    Private oCustodioAJusteRow As CustodioAjusteBE.CustodioAjusteRow
    Private oCustodioKardexRow As CustodioKardexBE.CustodioKardexRow

    Public Function Listar(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal CodigoSBS As String, ByVal TipoMovimiento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CustodioAjuste_Seleccionar")

        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
        db.AddInParameter(dbCommand, "@TipoMovimiento", DbType.String, TipoMovimiento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Insertar(ByVal obA As CustodioAjusteBE, ByVal obK As CustodioKardexBE, ByVal nFechaPortafolio As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "custodioAjusteKardex_Insertar"
        Dim dbCommand As dbCommand = db.GetStoredProcCommand(sqlCommand)
        oCustodioAJusteRow = CType(obA.CustodioAjuste.Rows(0), CustodioAjusteBE.CustodioAjusteRow)
        oCustodioKardexRow = CType(obK.CustodioKardex.Rows(0), CustodioKardexBE.CustodioKardexRow)


        db.AddInParameter(dbCommand, "@FechaPortafolio", DbType.String, nFechaPortafolio)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, oCustodioAJusteRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@NumeroTitulo", DbType.Decimal, oCustodioAJusteRow.NumeroTitulo)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oCustodioAJusteRow.CodigoISIN)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, oCustodioAJusteRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@FechaAjuste", DbType.Decimal, oCustodioAJusteRow.FechaAjuste)
        db.AddInParameter(dbCommand, "@HoraAjuste", DbType.String, oCustodioAJusteRow.HoraAjuste)
        db.AddInParameter(dbCommand, "@NumeroOperacion", DbType.Decimal, oCustodioAJusteRow.NumeroOperacion)
        db.AddInParameter(dbCommand, "@NumeroMovimiento", DbType.Decimal, oCustodioKardexRow.NumeroMovimiento)
        db.AddInParameter(dbCommand, "@FechaMovimiento", DbType.Decimal, oCustodioKardexRow.FechaMovimiento)
        db.AddInParameter(dbCommand, "@HoraMovimiento", DbType.String, oCustodioKardexRow.HoraMovimiento)
        db.AddInParameter(dbCommand, "@TituloNumeroLocalizacion", DbType.String, oCustodioKardexRow.TituloNumeroLocalizacion)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, oCustodioKardexRow.CodigoCustodio)
        db.AddInParameter(dbCommand, "@CtaDepositaria", DbType.String, oCustodioKardexRow.CtaDepositaria)
        db.AddInParameter(dbCommand, "@CantidadUnidades", DbType.Currency, oCustodioAJusteRow.CantidadUnidades)
        db.AddInParameter(dbCommand, "@ValorOrigenMovimiento", DbType.Currency, oCustodioKardexRow.ValorOrigenMovimiento)
        db.AddInParameter(dbCommand, "@ValorLocalMovimiento", DbType.Currency, oCustodioKardexRow.ValorLocalMovimiento)
        db.AddInParameter(dbCommand, "@TipoMovimiento", DbType.String, oCustodioAJusteRow.TipoMovimiento)
        db.AddInParameter(dbCommand, "@NumeroLamina", DbType.String, oCustodioKardexRow.NumeroLamina)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoST", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function InsertarTransferencia(ByVal obA As CustodioAjusteBE, ByVal obK As CustodioKardexBE, ByVal nFechaPortafolio As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CustodioAjusteKardexTransferencia_Insertar"
        Dim dbCommand As dbCommand = db.GetStoredProcCommand(sqlCommand)
        oCustodioAJusteRow = CType(obA.CustodioAjuste.Rows(0), CustodioAjusteBE.CustodioAjusteRow)
        oCustodioKardexRow = CType(obK.CustodioKardex.Rows(0), CustodioKardexBE.CustodioKardexRow)

        db.AddInParameter(dbCommand, "@FechaPortafolio", DbType.String, nFechaPortafolio)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, oCustodioAJusteRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@NumeroTitulo", DbType.Decimal, oCustodioAJusteRow.NumeroTitulo)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oCustodioAJusteRow.CodigoISIN)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, oCustodioAJusteRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@FechaAjuste", DbType.Decimal, oCustodioAJusteRow.FechaAjuste)
        db.AddInParameter(dbCommand, "@HoraAjuste", DbType.String, oCustodioAJusteRow.HoraAjuste)
        db.AddInParameter(dbCommand, "@NumeroOperacion", DbType.Decimal, oCustodioAJusteRow.NumeroOperacion)
        db.AddInParameter(dbCommand, "@NumeroMovimiento", DbType.Decimal, oCustodioKardexRow.NumeroMovimiento)
        db.AddInParameter(dbCommand, "@FechaMovimiento", DbType.Decimal, oCustodioKardexRow.FechaMovimiento)
        db.AddInParameter(dbCommand, "@HoraMovimiento", DbType.String, oCustodioKardexRow.HoraMovimiento)
        db.AddInParameter(dbCommand, "@TituloNumeroLocalizacion", DbType.String, oCustodioKardexRow.TituloNumeroLocalizacion)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, oCustodioKardexRow.CodigoCustodio)
        db.AddInParameter(dbCommand, "@CodigoCustodioFinal", DbType.String, oCustodioKardexRow.CodigoCustodioFinal)
        db.AddInParameter(dbCommand, "@CtaDepositaria", DbType.String, oCustodioKardexRow.CtaDepositaria)
        db.AddInParameter(dbCommand, "@CantidadUnidades", DbType.Currency, oCustodioAJusteRow.CantidadUnidades)
        db.AddInParameter(dbCommand, "@ValorOrigenMovimiento", DbType.Currency, oCustodioKardexRow.ValorOrigenMovimiento)
        db.AddInParameter(dbCommand, "@ValorLocalMovimiento", DbType.Currency, oCustodioKardexRow.ValorLocalMovimiento)
        db.AddInParameter(dbCommand, "@TipoMovimiento", DbType.String, oCustodioAJusteRow.TipoMovimiento)
        db.AddInParameter(dbCommand, "@NumeroLamina", DbType.String, oCustodioKardexRow.NumeroLamina)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoST", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

End Class
