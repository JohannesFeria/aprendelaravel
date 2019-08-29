Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class MonedaDAM
    Private sqlCommand As String = ""
    Private oMonedaRow As MonedaBE.MonedaRow
    Public Sub New()
    End Sub
    Public Function SeleccionarPorFiltro(ByVal codigoMoneda As String, ByVal descripcion As String, ByVal situacion As String, ByVal codigoIso As String, ByVal sinonimoIso As String, ByVal dataRequest As DataSet) As MonedaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim objeto As New MonedaBE
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Moneda_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_CodigoIso", DbType.String, codigoIso)
        db.AddInParameter(dbCommand, "@p_SinonimoIso", DbType.String, sinonimoIso)
        db.LoadDataSet(dbCommand, objeto, "Moneda")
        Return objeto
    End Function
    Public Function SeleccionarPorCodigoSBS(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim objeto As New DataSet
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Moneda_SeleccionarPorCodigoSBS")
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, codigoSBS)
        db.LoadDataSet(dbCommand, objeto, "Moneda")
        Return objeto
    End Function
    Public Function Listar(Optional ByVal situacion As String = "") As DataSet
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("Moneda_Listar")
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
                Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                    Return ds
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetMonedaMercadoPortafolio(ByVal codigoMercado As String, ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetMonedaMercadoPortafolio")
        db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolio", DbType.String, codigoPortafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Listar(ByVal dataRequest) As MonedaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim objeto As New MonedaBE
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Moneda_Listar")
        db.LoadDataSet(dbCommand, objeto, "Moneda")
        Return objeto
    End Function
    Public Function Insertar(ByVal ob As MonedaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Moneda_Insertar"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        oMonedaRow = CType(ob.Moneda.Rows(0), MonedaBE.MonedaRow)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oMonedaRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaSBS", DbType.String, oMonedaRow.CodigoMonedaSBS)
        db.AddInParameter(dbCommand, "@p_Simbolo", DbType.String, oMonedaRow.Simbolo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMonedaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_TipoCalculo", DbType.String, oMonedaRow.TipoCalculo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMonedaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoIso", DbType.String, oMonedaRow.CodigoISO)
        db.AddInParameter(dbCommand, "@p_SinonimoIso", DbType.String, oMonedaRow.SinonimoISO)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@P_CodMonedaTipoCambio", DbType.String, oMonedaRow.CodMonedaTipoCambio)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function Modificar(ByVal ob As MonedaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Moneda_Modificar")
        oMonedaRow = CType(ob.Moneda.Rows(0), MonedaBE.MonedaRow)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oMonedaRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaSBS", DbType.String, oMonedaRow.CodigoMonedaSBS)
        db.AddInParameter(dbCommand, "@p_Simbolo", DbType.String, oMonedaRow.Simbolo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMonedaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_TipoCalculo", DbType.String, oMonedaRow.TipoCalculo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMonedaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoIso", DbType.String, oMonedaRow.CodigoISO)
        db.AddInParameter(dbCommand, "@p_SinonimoIso", DbType.String, oMonedaRow.SinonimoISO)
        db.AddInParameter(dbCommand, "@P_CodMonedaTipoCambio", DbType.String, oMonedaRow.CodMonedaTipoCambio)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Eliminar(ByVal codigoMoneda As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Moneda_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Function ObtenerCodigoMonedaxSinonimo(ByVal SinonimoMoneda As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ObtenerCodigoMoneda")

        db.AddInParameter(dbCommand, "@SinonimoMoneda", DbType.String, SinonimoMoneda)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "Moneda")
        Return ds.Tables("Moneda")

    End Function
End Class