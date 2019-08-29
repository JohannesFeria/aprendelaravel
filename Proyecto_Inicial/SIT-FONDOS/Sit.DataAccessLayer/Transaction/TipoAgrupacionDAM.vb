Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class TipoAgrupacionDAM
    Private sqlCommand As String = ""
    Private oRow As TipoAgrupacionBE.TipoAgrupacionRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltros(ByVal situacion As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As TipoAmortizacionBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_SeleccionarPorFiltro")
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)

        db.LoadDataSet(dbCommand, oTipoAmortizacionBE, "TipoAmortizacion")

        Return oTipoAmortizacionBE

    End Function

    Public Function Seleccionar(ByVal codigoTipoAmortizacion As String, ByVal dataRequest As DataSet) As TipoAmortizacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, codigoTipoAmortizacion)
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        db.LoadDataSet(dbCommand, oTipoAmortizacionBE, "TipoAmortizacion")
        Return oTipoAmortizacionBE

    End Function
#End Region
End Class
