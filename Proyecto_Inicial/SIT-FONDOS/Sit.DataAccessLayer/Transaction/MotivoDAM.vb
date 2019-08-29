Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class MotivoDAM

    Private sqlCommand As String = ""

    Public Sub New()

    End Sub

    Public Function Seleccionar(ByVal codigoMotivo As String) As MotivoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Motivo_Seleccionar")
        Dim oMotivoBE As New MotivoBE

        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, codigoMotivo)

        db.LoadDataSet(dbCommand, oMotivoBE, "Motivo")

        Return oMotivoBE

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoMotivo As String, ByVal descripcion As String, ByVal situacion As String) As MotivoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Motivo_SeleccionarPorFiltro")
        Dim oMotivoBE As New MotivoBE

        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, codigoMotivo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oMotivoBE, "Motivo")

        Return oMotivoBE

    End Function

    Public Function Listar(ByVal dataRequest) As MotivoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim objeto As New MotivoBE
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Motivo_Listar")

        db.LoadDataSet(dbCommand, objeto, "Motivo")

        Return objeto

    End Function

    Public Function Insertar(ByVal oMotivoBE As MotivoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Motivo_Insertar")

        Dim oMotivoRow As MotivoBE.MotivoRow

        oMotivoRow = DirectCast(oMotivoBE.Motivo.Rows(0), MotivoBE.MotivoRow)

        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, oMotivoRow.CodigoMotivo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMotivoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMotivoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Eliminar(ByVal codigoMotivo As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Motivo_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, codigoMotivo)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Modificar(ByVal oMotivoBE As MotivoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Motivo_Modificar")

        Dim oMotivoRow As MotivoBE.MotivoRow

        oMotivoRow = DirectCast(oMotivoBE.Motivo.Rows(0), MotivoBE.MotivoRow)

        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, oMotivoRow.CodigoMotivo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMotivoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMotivoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

End Class

