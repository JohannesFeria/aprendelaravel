Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

Public Class TipoCambioDI_DAM
    Private oTipoCambioRow As TipoCambioDI_BE.TipoCambioDIRow

    Public Function SeleccionarPorFiltros(ByVal codigoTipoCambio As String, ByVal codigoMonedaOrigen As String, ByVal codigoMonedaDestino As String, ByVal tipo As String, ByVal situacion As String, ByVal DataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioDI_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, codigoTipoCambio)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaOrigen", DbType.String, codigoMonedaOrigen)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, codigoMonedaDestino)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, tipo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Insertar(ByVal ob As TipoCambioDI_BE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioDI_Insertar")

        Dim Codigo As String = String.Empty

        oTipoCambioRow = CType(ob.TipoCambioDI.Rows(0), TipoCambioDI_BE.TipoCambioDIRow)

        db.AddInParameter(dbCommand, "@p_CodigoMonedaOrigen", DbType.String, oTipoCambioRow.CodigoMonedaOrigen)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, oTipoCambioRow.CodigoMonedaDestino)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, oTipoCambioRow.Tipo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoCambioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oTipoCambioRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal ob As TipoCambioDI_BE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioDI_Modificar")

        oTipoCambioRow = CType(ob.TipoCambioDI.Rows(0), TipoCambioDI_BE.TipoCambioDIRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, oTipoCambioRow.CodigoTipoCambio)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaOrigen", DbType.String, oTipoCambioRow.CodigoMonedaOrigen)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, oTipoCambioRow.CodigoMonedaDestino)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, oTipoCambioRow.Tipo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoCambioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oTipoCambioRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoTipoCambioDI As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioDI_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, codigoTipoCambioDI)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
End Class
