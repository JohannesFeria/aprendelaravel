Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class SaldoNoAdministradoDAM
    Public Function InsertarRegistrosExcel(ByVal ob As SaldoNoAdministradoBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SaldoNoAdministrado_InsertarExcel")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, ob.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ob.Fecha)
        db.AddInParameter(dbCommand, "@p_CodigoTerceroFinanciero", DbType.String, ob.CodigoTerceroFinanciero)
        db.AddInParameter(dbCommand, "@p_TipoCuenta", DbType.String, ob.TipoCuenta)
        db.AddInParameter(dbCommand, "@p_Saldo", DbType.Decimal, ob.Saldo)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, ob.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    'Public Function SeleccionarPorFiltro(ByVal CodigoSaldoNoAdministrado As String, ByVal Tercero As String, ByVal Fecha As Decimal, ByVal TerceroFinanciero As String, ByVal TipoCuenta As String, ByVal Moneda As String, ByVal TipoConsulta As String, ByVal dataRequest As DataSet) As DataSet
    Public Function SeleccionarPorFiltro(ByVal CodigoSaldoNoAdministrado As String, ByVal Mandato As String, ByVal Fecha As Decimal, ByVal TerceroFinanciero As String, ByVal TipoCuenta As String, ByVal Moneda As String, ByVal TipoConsulta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SP_SaldoNoAdministrado_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoSaldoNoAdministrado", DbType.String, CodigoSaldoNoAdministrado)
        db.AddInParameter(dbCommand, "@P_CodigoTercero", DbType.String, Mandato)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@P_CodigoTerceroFinanciero", DbType.String, TerceroFinanciero)
        db.AddInParameter(dbCommand, "@p_TipoCuenta", DbType.String, TipoCuenta)
        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, Moneda)
        db.AddInParameter(dbCommand, "@p_TipoConsulta", DbType.String, TipoConsulta)
        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function Eliminar(ByVal CodigoSaldoNoAdministrado As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SaldoNoAdministrado_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoSaldoNoAdministrado", DbType.String, CodigoSaldoNoAdministrado)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    Public Function Insertar(ByVal ob As SaldoNoAdministradoBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SaldoNoAdministrado_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, ob.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ob.Fecha)
        db.AddInParameter(dbCommand, "@p_CodigoTerceroFinanciero", DbType.String, ob.CodigoTerceroFinanciero)
        db.AddInParameter(dbCommand, "@p_TipoCuenta", DbType.String, ob.TipoCuenta)
        db.AddInParameter(dbCommand, "@p_Saldo", DbType.Decimal, ob.Saldo)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, ob.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, ob.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As SaldoNoAdministradoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SaldoNoAdministrado_Modificar")

        'oObligacionTecnicaRow = CType(ob.ObligacionTecnica.Rows(0), ObligacionTecnicaBE.ObligacionTecnicaRow)
        db.AddInParameter(dbCommand, "@p_CodigoSaldoNoAdministrado", DbType.String, ob.CodigoSaldoNoAdministrado)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, ob.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ob.Fecha)
        db.AddInParameter(dbCommand, "@p_CodigoTerceroFinanciero", DbType.String, ob.CodigoTerceroFinanciero)
        db.AddInParameter(dbCommand, "@p_TipoCuenta", DbType.String, ob.TipoCuenta)
        db.AddInParameter(dbCommand, "@p_Saldo", DbType.String, ob.Saldo)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, ob.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, ob.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function DesactivarRegistrosExcel(ByVal dataRequest As DataSet, ByRef strMensaje As String, ByVal strMes As String, ByVal strAnhio As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommandInactivar As DbCommand = db.GetStoredProcCommand("sp_SaldoNoAdministrado_DesactivarRegistros")

            db.AddInParameter(dbCommandInactivar, "@p_Mes", DbType.String, strMes)
            db.AddInParameter(dbCommandInactivar, "@p_Anhio", DbType.String, strAnhio)
            db.AddInParameter(dbCommandInactivar, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommandInactivar, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommandInactivar, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommandInactivar, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommandInactivar)

        Catch ex As Exception
            strMensaje = "error al desactivar los registros existentes"
        End Try


    End Function
End Class
