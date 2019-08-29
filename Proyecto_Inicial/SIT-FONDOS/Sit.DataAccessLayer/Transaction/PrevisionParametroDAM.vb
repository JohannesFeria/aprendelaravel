Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Sit.BusinessEntities

Public Class PrevisionParametroDAM

    Private dbBase As SqlDatabase
    Private dbCommand As DbCommand
    Private sqlCommand As String = ""

    Public Sub New()
    End Sub

    Public Function Listar(ByVal strParametro As String) As DataSet
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.sp_PROV_sel_ParametroListar")
        dbBase.AddInParameter(cmd, "@p_Parametro", DbType.String, strParametro)
        Return dbBase.ExecuteDataSet(cmd)
    End Function
    Public Function Eliminar(ByVal strParametro As String) As Boolean
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.sp_PROV_del_ParametroEliminar")
        dbBase.AddInParameter(cmd, "@p_IdTabla", DbType.String, strParametro)
        dbBase.ExecuteNonQuery(cmd)
        Return True
    End Function
    Public Function Seleccionar(ByVal strParametro As String) As DataSet
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.sp_PROV_sel_ParametroSeleccionarPorCodigo")
        dbBase.AddInParameter(cmd, "@p_IdTabla", DbType.String, strParametro)
        Return dbBase.ExecuteDataSet(cmd)
    End Function

    Public Function Insertar(ByVal IdTabla As Integer, ByVal Descripcion As String, ByVal Valor As String) As String
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.sp_PROV_ins_ParametroInsertar")
        dbBase.AddInParameter(cmd, "@p_IdTabla", DbType.Int32, IdTabla)
        dbBase.AddInParameter(cmd, "@p_Descripcion", DbType.String, Descripcion)
        dbBase.AddInParameter(cmd, "@p_Valor", DbType.String, Valor)
        Dim Codigo As String = dbBase.ExecuteScalar(cmd)
        Return Codigo
    End Function
    Public Function SeleccionarPorCodigos(ByVal strParametro As String) As DataSet
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.Parametro_SeleccionarPorCodigos")
        dbBase.AddInParameter(cmd, "@p_Valor", DbType.String, strParametro)
        Return dbBase.ExecuteDataSet(cmd)
    End Function

    Public Function SeleccionarDescripcion(ByVal iIdTabla As Integer, ByVal Valor As String) As String
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.Parametro_SeleccionarDescripcion")
        dbBase.AddInParameter(cmd, "@p_IdTabla", DbType.Int32, iIdTabla)
        dbBase.AddInParameter(cmd, "@p_Valor", DbType.String, Valor)
        Return dbBase.ExecuteScalar(cmd).ToString()
    End Function
    Public Function SeleccionarPorCodigos2(ByVal strParametro As String) As DataSet
        dbBase = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim cmd As DbCommand = dbBase.GetStoredProcCommand("prov.Parametro_SeleccionarPorCodigos2")
        dbBase.AddInParameter(cmd, "@p_Valor", DbType.String, strParametro)
        Return dbBase.ExecuteDataSet(cmd)
    End Function
End Class

