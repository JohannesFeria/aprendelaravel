Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PrevisionPagoDetalleDAM

    Public Shared Function ListarPrevisionPagoDetalle(ByVal FechaPago As String, ByVal IdTipoOperacion As String, ByVal IdEstado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ConsultarPagos")
        db.AddInParameter(dbCommand, "@FechaPago", DbType.String, FechaPago)
        db.AddInParameter(dbCommand, "@IdTipoOperacion", DbType.String, IdTipoOperacion)
        db.AddInParameter(dbCommand, "@IdEstado", DbType.String, IdEstado)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ListarPrevisionPagoDetalleToExport(ByVal FechaPago As String, ByVal IdTipoOperacion As String, ByVal IdEstado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ConsultarPagos_Export")
        db.AddInParameter(dbCommand, "@FechaPago", DbType.String, FechaPago)
        db.AddInParameter(dbCommand, "@IdTipoOperacion", DbType.String, IdTipoOperacion)
        db.AddInParameter(dbCommand, "@IdEstado", DbType.String, IdEstado)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ReportePrevisionPagoPorDetalle(ByVal sFechaInicio As String, ByVal sFechaFin As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ReportePorDetalle")
        db.AddInParameter(dbCommand, "@FechaInicio", DbType.String, sFechaInicio)
        db.AddInParameter(dbCommand, "@FechaFinal", DbType.String, sFechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function PagoDetallePorCuentaCte(ByVal Fecha As String, ByVal CuentaCte As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_PagoDetallePorCuentaCte")
        db.AddInParameter(dbCommand, "@CuentaCte", DbType.String, CuentaCte)
        db.AddInParameter(dbCommand, "@FechaPago", DbType.String, Fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ListarCuentaCtePorIdFondo(ByVal IdFondo As String, ByVal Fecha As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ListaCuentaCtePorIdFondo")
        db.AddInParameter(dbCommand, "@IdFondo", DbType.String, IdFondo)
        db.AddInParameter(dbCommand, "@Fecha", DbType.String, Fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ListarOperacionxUsuario(ByVal CodUsuario As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ListarOperacionxUsuario")
        db.AddInParameter(dbCommand, "@CodUsuario", DbType.String, CodUsuario)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ConsultaEntidad_x_TipoMoneda(ByVal IdMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ConsultaEntidad_x_TipoMoneda")
        db.AddInParameter(dbCommand, "@p_IdMonera", DbType.String, IdMoneda)
        Return db.ExecuteDataSet(dbCommand)
    End Function
End Class
