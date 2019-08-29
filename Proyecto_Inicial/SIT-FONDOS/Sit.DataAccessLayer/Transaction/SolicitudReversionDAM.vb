Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class SolicitudReversionDAM
    Public Function ObtenerAreaUsuarioSisOpe(ByVal p_CodigoTabla As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_TABLA_GENERAL")
            db.AddInParameter(dbCommand, "@codigoTabla", DbType.String, p_CodigoTabla)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    'oSolicitudReversionDAM.Seleccionar(estadoRegistro, fechaIni, fechaFin, dataRequest)
    'Public Function Seleccionar(ByVal estadoRegistro As String, ByVal fechaIni As String, ByVal fechaFin As String, ByVal dataRequest As DataSet) As SolicitudReversionBE
    Public Function Seleccionar(ByVal estadoRegistro As String, ByVal fechaIni As String, ByVal dataRequest As DataSet) As SolicitudReversionBE
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SolicitudReversion_Seleccionar")
        Dim oSolicitudesReversion As New SolicitudReversionBE

        db.AddInParameter(dbCommand, "@p_FlagAprobado", DbType.String, estadoRegistro)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fechaIni)

        db.LoadDataSet(dbCommand, oSolicitudesReversion, "SolicitudReversion")

        Return oSolicitudesReversion

    End Function

    Public Function Insertar(ByVal oSolicitudReversionBE As SolicitudReversionBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SolicitudReversion_Insertar")
        Dim oRow As SolicitudReversionBE.SolicitudReversionRow

        oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(0), SolicitudReversionBE.SolicitudReversionRow)

        db.AddInParameter(dbCommand, "@p_IdFondo", DbType.String, oRow.IdFondo)
        db.AddInParameter(dbCommand, "@p_FechaInicial", DbType.Decimal, oRow.FechaInicial)
        db.AddInParameter(dbCommand, "@p_FechaFinal", DbType.Decimal, oRow.FechaFinal)
        db.AddInParameter(dbCommand, "@p_Motivo", DbType.String, oRow.Motivo)
        db.AddInParameter(dbCommand, "@p_codigoArea", DbType.String, oRow.codigoArea)
        db.AddInParameter(dbCommand, "@p_Responsable", DbType.String, oRow.Responsable)
        db.AddInParameter(dbCommand, "@p_AccionTemporal", DbType.String, oRow.AccionTemporal)
        db.AddInParameter(dbCommand, "@p_SolucionDefinitivaSugerida", DbType.String, oRow.SolucionDefinitivaSugerida)
        db.AddInParameter(dbCommand, "@p_FlagAfectaValorCuota", DbType.String, oRow.FlagAfectaValorCuota)
        db.AddInParameter(dbCommand, "@p_FlagAprobado", DbType.String, oRow.FlagAprobado)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Modificar(ByVal oSolicitudReversionBE As SolicitudReversionBE, ByVal intIndice As Integer, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SolicitudReversion_Modificar")
        Dim oRow As SolicitudReversionBE.SolicitudReversionRow

        oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(intIndice), SolicitudReversionBE.SolicitudReversionRow)

        db.AddInParameter(dbCommand, "@p_Id", DbType.String, oRow.ID)
        db.AddInParameter(dbCommand, "@p_IdFondo", DbType.String, oRow.IdFondo)
        db.AddInParameter(dbCommand, "@p_FechaInicial", DbType.Decimal, oRow.FechaInicial)
        db.AddInParameter(dbCommand, "@p_FechaFinal", DbType.Decimal, oRow.FechaFinal)
        db.AddInParameter(dbCommand, "@p_Motivo", DbType.String, oRow.Motivo)
        db.AddInParameter(dbCommand, "@p_codigoArea", DbType.String, oRow.codigoArea)
        db.AddInParameter(dbCommand, "@p_Responsable", DbType.String, oRow.Responsable)
        db.AddInParameter(dbCommand, "@p_AccionTemporal", DbType.String, oRow.AccionTemporal)
        db.AddInParameter(dbCommand, "@p_SolucionDefinitivaSugerida", DbType.String, oRow.SolucionDefinitivaSugerida)
        db.AddInParameter(dbCommand, "@p_FlagAfectaValorCuota", DbType.String, oRow.FlagAfectaValorCuota)
        db.AddInParameter(dbCommand, "@p_FlagAprobado", DbType.String, oRow.FlagAprobado)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal oSolicitudReversionBE As SolicitudReversionBE, ByVal intIndice As Integer, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SolicitudReversion_Eliminar")
        Dim oRow As SolicitudReversionBE.SolicitudReversionRow

        oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(intIndice), SolicitudReversionBE.SolicitudReversionRow)

        db.AddInParameter(dbCommand, "@p_Id", DbType.String, oRow.ID)
        'db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    'oSolicitudReversionDAM.Eliminar(id, dataRequest)
    Public Function Aprobar(ByVal id As Integer, ByVal estado As String, ByVal motivoRechazo As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SolicitudReversion_Aprobar")
        Dim oRow As SolicitudReversionBE.SolicitudReversionRow

        db.AddInParameter(dbCommand, "@p_Id", DbType.String, id)
        db.AddInParameter(dbCommand, "@p_FlagEstado", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_MotivoRechazo", DbType.String, motivoRechazo)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function SeleccionarSolicitudReversion(ByVal estado As String, ByVal dataRequest As DataSet) As SolicitudReversionBE
        'Dim oCuentas As New CustodioCuentaDepositariaBE
        Dim oSolicitudesReversion As New SolicitudReversionBE

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SolicitudReversion_Listar")

        db.AddInParameter(dbCommand, "@p_FlagEstado", DbType.String, estado)

        db.LoadDataSet(dbCommand, oSolicitudesReversion, "SolicitudReversion")

        Return oSolicitudesReversion

    End Function
End Class
