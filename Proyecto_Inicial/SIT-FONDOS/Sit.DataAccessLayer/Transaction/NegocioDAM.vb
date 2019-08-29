Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class NegocioDAM

    Private sqlCommand As String = ""
    Private oNegocioRow As NegocioBE.NegocioRow

    Public Sub New()

    End Sub
#Region "Funciones Seleccionar"
    Public Function SeleccionarPorFiltro(ByVal codigoNegocio As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As NegocioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Negocio_SeleccionarPorFiltro"
        Dim dbCommand As dbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New NegocioBE
        db.LoadDataSet(dbCommand, objeto, "Negocio")
        Return objeto
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Negocio_Listar")
            Dim objeto As New DataSet
            db.LoadDataSet(dbCommand, objeto, "Negocio")
            Return objeto
        End Using
    End Function

#End Region

    Public Function Insertar(ByVal ob As NegocioBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Negocio_Insertar")
        oNegocioRow = CType(ob.Negocio.Rows(0), NegocioBE.NegocioRow)

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oNegocioRow.CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oNegocioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oNegocioRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oNegocioRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Modificar(ByVal ob As NegocioBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Negocio_Modificar")
        oNegocioRow = CType(ob.Negocio.Rows(0), NegocioBE.NegocioRow)

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oNegocioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oNegocioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oNegocioRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oNegocioRow.CodigoNegocio)

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoNegocio As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Negocio_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

End Class

