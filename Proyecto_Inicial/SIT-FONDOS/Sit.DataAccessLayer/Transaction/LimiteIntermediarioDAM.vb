'Creado por: HDG OT 64926 20120320
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class LimiteIntermediarioDAM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oLimiteIntermediarioBE As LimiteIntermediarioBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_LimiteIntermediario_Insertar")
        Dim oRow As LimiteIntermediarioBE.LimiteIntermediarioRow

        oRow = DirectCast(oLimiteIntermediarioBE.LimiteIntermediario.Rows(0), LimiteIntermediarioBE.LimiteIntermediarioRow)

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, oRow.Porcentaje)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal situacion As String) As LimiteIntermediarioBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_LimiteIntermediario_SeleccionarPorFiltro")
        Dim oLimiteIntermediarioBE As New LimiteIntermediarioBE

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oLimiteIntermediarioBE, "LimiteIntermediario")

        Return oLimiteIntermediarioBE

    End Function

    Public Function Seleccionar(ByVal CodigoLimInter As String) As LimiteIntermediarioBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_LimiteIntermediario_Seleccionar")
        Dim oLimiteIntermediarioBE As New LimiteIntermediarioBE

        db.AddInParameter(dbCommand, "@p_CodigoLimInter", DbType.Decimal, CodigoLimInter)

        db.LoadDataSet(dbCommand, oLimiteIntermediarioBE, "LimiteIntermediario")

        Return oLimiteIntermediarioBE

    End Function

    Public Function Modificar(ByVal oLimiteIntermediarioBE As LimiteIntermediarioBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_LimiteIntermediario_Modificar")
        Dim oRow As LimiteIntermediarioBE.LimiteIntermediarioRow

        oRow = DirectCast(oLimiteIntermediarioBE.LimiteIntermediario.Rows(0), LimiteIntermediarioBE.LimiteIntermediarioRow)

        db.AddInParameter(dbCommand, "@p_CodigoLimInter", DbType.Decimal, oRow.CodigoLimInter)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, oRow.Porcentaje)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal CodigoLimInter As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_LimiteIntermediario_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoLimInter", DbType.String, CodigoLimInter)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function GenerarReporteOperacionesNegociadas(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal tipoRenta As String, ByVal codigoUsuario As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteLimitesOperacionxTrader")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, tipoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, codigoUsuario)

        Return db.ExecuteDataSet(dbCommand)
    End Function
End Class

