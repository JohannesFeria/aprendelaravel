Imports System
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Collections.Generic

Public Class MnRolDAM
    Inherits BaseDAM

    Private sqlCommand As String = ""
    'Private oMercadoRow As MercadoBE.MercadoRow

    Public Function Listar(rolBE As MnRolBE, ByVal dataRequest As DataSet) As List(Of MnRolBE)

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Sel_Rol_Aplicativo")

        db.AddInParameter(dbCommand, "@v_Nombre_Rol", DbType.String, rolBE.NOMBRE_ROL)
        db.AddInParameter(dbCommand, "@v_Estado", DbType.String, rolBE.ESTADO)
        db.AddInParameter(dbCommand, "@v_CodAplicativo", DbType.String, rolBE.CODIGO_APLICATIVO)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return ConvertToList(Of MnRolBE)(dt)

    End Function

    Public Function Insertar(ByVal rolBE As MnRolBE, ByVal dataRequest As DataSet) As Boolean 'List(Of MnRolBE)

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Ins_Rol")

        db.AddInParameter(dbCommand, "@v_CODIGO_APLICATIVO", DbType.String, rolBE.CODIGO_APLICATIVO)
        db.AddInParameter(dbCommand, "@v_NOMBRE_ROL", DbType.String, rolBE.NOMBRE_ROL)
        db.AddInParameter(dbCommand, "@v_ESTADO", DbType.String, rolBE.ESTADO)
        db.AddInParameter(dbCommand, "@v_FECHA_AUDITORIA", DbType.Int32, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@v_HORA_AUDITORIA", DbType.Int32, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")).Replace(":", ""))
        db.AddInParameter(dbCommand, "@v_USUARIO_AUDITORIA", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))

        'dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Actualizar(ByVal rolBE As MnRolBE, ByVal dataRequest As DataSet) As Boolean 'List(Of MnRolBE)

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Upd_Rol")

        db.AddInParameter(dbCommand, "@v_CODIGO", DbType.String, rolBE.CODIGO_ROL)
        db.AddInParameter(dbCommand, "@v_CODIGO_APLICATIVO", DbType.String, rolBE.CODIGO_APLICATIVO)
        db.AddInParameter(dbCommand, "@v_NOMBRE_ROL", DbType.String, rolBE.NOMBRE_ROL)
        db.AddInParameter(dbCommand, "@v_ESTADO", DbType.String, rolBE.ESTADO)
        db.AddInParameter(dbCommand, "@v_FECHA_AUDITORIA", DbType.Int32, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@v_HORA_AUDITORIA", DbType.Int32, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")).Replace(":", ""))
        db.AddInParameter(dbCommand, "@v_USUARIO_AUDITORIA", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))

        'dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return db.ExecuteNonQuery(dbCommand)

    End Function



End Class
