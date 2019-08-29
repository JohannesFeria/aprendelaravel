Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class TipoRentaDAM

    Private sqlCommand As String = ""
    Private oTipoRentaRow As TipoRentaBE.TipoRentaRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoTipoRenta As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoRentaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoRenta_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoTipoRenta)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New TipoRentaBE
        db.LoadDataSet(dbCommand, objeto, "TipoRenta")
        Return objeto

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As TipoRentaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoRenta_Listar")

        Dim objeto As New TipoRentaBE
        db.LoadDataSet(dbCommand, objeto, "TipoRenta")
        Return objeto

    End Function

    'HDG OT 62087 Nro14-R23 20110223
    Public Function SeleccionarTipoFactor(ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_TipoRenta_SeleccionaTipoFactor")

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoTipoRenta)
        Return db.ExecuteDataSet(dbCommand)

    End Function

#End Region

    Public Function Insertar(ByVal ob As TipoRentaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoRenta_Insertar")

        oTipoRentaRow = CType(ob.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oTipoRentaRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoRentaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoRentaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, oTipoRentaRow.TipoFactor)   'HDG OT 62087 Nro14-R23 20110223

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Modificar(ByVal ob As TipoRentaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoRenta_Modificar")

        oTipoRentaRow = CType(ob.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoRentaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoRentaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, oTipoRentaRow.TipoFactor)   'HDG OT 62087 Nro14-R23 20110223

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oTipoRentaRow.CodigoRenta)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoRenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoRenta_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoRenta)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

End Class

