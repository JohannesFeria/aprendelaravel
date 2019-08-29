Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class MovimientoPersonalDAM
    Private sqlCommand As String = ""
    Dim DECIMAL_NULO As Decimal = -1
    Public oMovimientoPersonalRow As MovimientoPersonalBE.MovimientoPersonalRow
    Public Sub New()

    End Sub

    Public Sub InicializarMovimientoPersonal(ByRef oRow As MovimientoPersonalBE.MovimientoPersonalRow)
        oRow.CodigoInterno = ""
        oRow.Estado = ""
        oRow.FechaFin = DECIMAL_NULO
        oRow.FechaInicio = DECIMAL_NULO
        oRow.NumeroMovimiento = DECIMAL_NULO
        oRow.CodigoUsuario = ""
    End Sub

    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_MovimientoPersonal")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Modificar(ByVal oMovimientoPersonalBE As MovimientoPersonalBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        oMovimientoPersonalRow = CType(oMovimientoPersonalBE.MovimientoPersonal.Rows(0), MovimientoPersonalBE.MovimientoPersonalRow)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_MovimientoPersonal")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oMovimientoPersonalRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, oMovimientoPersonalRow.FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, oMovimientoPersonalRow.FechaFin)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMovimientoPersonalRow.Estado)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Insertar(ByVal oMovimientoPersonalBE As MovimientoPersonalBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        oMovimientoPersonalRow = CType(oMovimientoPersonalBE.MovimientoPersonal.Rows(0), MovimientoPersonalBE.MovimientoPersonalRow)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_MovimientoPersonal")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oMovimientoPersonalRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, oMovimientoPersonalRow.FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, oMovimientoPersonalRow.FechaFin)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMovimientoPersonalRow.Estado)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
    End Function
End Class
