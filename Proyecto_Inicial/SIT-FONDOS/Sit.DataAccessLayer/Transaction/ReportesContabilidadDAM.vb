Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ReportesContabilidadDAM
    Private sqlCommand As String = ""
    Public Sub New()
    End Sub
    Public Function CompraVentaOperacionesInversion(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReportesContabilidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_CompraVentaOperacionesInversion")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param3)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, param2)
        Dim oReporte As New ReportesContabilidadBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteContabilidad")
        Return oReporte
    End Function
    Public Function CompraVentaOperacionesInversionCabecera(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReporteContabilidadCabeceraBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_CompraVentaOperacionesInversion_Cabecera")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param3)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, param2)
        Dim oReporte As New ReporteContabilidadCabeceraBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteContabilidadCabecera")
        Return oReporte
    End Function
    Public Function ValorizacionCartera(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReportesContabilidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_ValorizacionCartera")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param3)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, param2)
        Dim oReporte As New ReportesContabilidadBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteContabilidad")
        Return oReporte
    End Function
    Public Function CobranzaCancelacionInversiones(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal param4 As String, ByVal dataRequest As DataSet, Optional ByVal StrCodigoMercado As String = "") As ReportesContabilidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_AsientosContablesPorTipo")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param3)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, param2)
        db.AddInParameter(dbCommand, "@p_TipoAsiento", DbType.String, param4)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, StrCodigoMercado)
        Dim oReporte As New ReportesContabilidadBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteContabilidad")
        Return oReporte
    End Function
    Public Function Administradora(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReportesContabilidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_Administradora")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param3)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, param2)
        Dim oReporte As New ReportesContabilidadBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteContabilidad")
        Return oReporte
    End Function
    Public Function Resumen_Envio_PU_ADM(ByVal param1 As String, _
                                         ByVal param2 As Decimal, _
                                         ByVal param3 As Decimal, _
                                         ByVal dataRequest As DataSet) As ReportesResumenEnvioPUADM
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_Resumen_Envio_PU_ADM")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, param1)
        db.AddInParameter(dbCommand, "@p_FechaAperturaDesde", DbType.Decimal, param2)
        db.AddInParameter(dbCommand, "@p_FechaAperturaHasta", DbType.Decimal, param3)
        Dim oReporte As New ReportesResumenEnvioPUADM
        db.LoadDataSet(dbCommand, oReporte, "ReportesResumenEnvioPUADM")
        Return oReporte
    End Function
    Public Function Resumen_Envio_PU_FONDO(ByVal param1 As String, _
                                           ByVal param2 As Decimal, _
                                           ByVal param3 As Decimal, _
                                           ByVal dataRequest As DataSet) As ReportesResumenEnvioPUADM
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Contabilidad_Reportes_Resumen_Envio_PU_FONDO")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, param1)
        db.AddInParameter(dbCommand, "@p_FechaAperturaDesde", DbType.Decimal, param2)
        db.AddInParameter(dbCommand, "@p_FechaAperturaHasta", DbType.Decimal, param3)
        Dim oReporte As New ReportesResumenEnvioPUADM
        db.LoadDataSet(dbCommand, oReporte, "ReportesResumenEnvioPUADM")
        Return oReporte
    End Function
    Public Function LotesResumenClaseInstrumento(ByVal CodigoPortafolioSBS As String, Fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_LotesResumenClaseInstrumento")
        db.AddInParameter(dbCommand, "@P_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@P_Fecha", DbType.Decimal, Fecha)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
End Class