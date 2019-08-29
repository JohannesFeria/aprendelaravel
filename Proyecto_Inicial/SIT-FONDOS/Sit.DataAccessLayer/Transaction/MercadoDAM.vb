Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class MercadoDAM

    Private sqlCommand As String = ""
    Private oMercadoRow As MercadoBE.MercadoRow
    Public Sub New()

    End Sub

    Public Function Insertar(ByVal ob As MercadoBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mercado_Insertar")
        oMercadoRow = CType(ob.Mercado.Rows(0), MercadoBE.MercadoRow)

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oMercadoRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMercadoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMercadoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoMercado As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As MercadoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mercado_SeleccionarPorFiltro")
        Dim objeto As New MercadoBE

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, objeto, "Mercado")

        Return objeto

    End Function

    Public Function Listar(ByVal dataRequest As DataSet, ByVal situacion As String) As MercadoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Mercado_Listar")
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        Dim objeto As New MercadoBE

        db.LoadDataSet(dbCommand, objeto, "Mercado")

        Return objeto

    End Function

    Public Function ListarActivos(ByVal dataRequest As DataSet) As MercadoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mercado_Listar_Activos")
        Dim objeto As New MercadoBE

        db.LoadDataSet(dbCommand, objeto, "Mercado")

        Return objeto

    End Function



    Public Function Modificar(ByVal ob As MercadoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mercado_Modificar")
        oMercadoRow = CType(ob.Mercado.Rows(0), MercadoBE.MercadoRow)

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oMercadoRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMercadoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMercadoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoMercado As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mercado_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

End Class

