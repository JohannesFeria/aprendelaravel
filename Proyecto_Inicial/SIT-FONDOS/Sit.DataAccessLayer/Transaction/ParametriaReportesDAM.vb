Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ParametriaReportesDAM
    Public Sub New()

    End Sub
    Public Function ReporteCotizacionVAC(ByVal CodigoIndicador As String, ByVal fechainicial As Decimal, ByVal fechafinal As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Parametria_ReporteCotizacionVac")
        Dim oPaisBE As New DataSet

        db.AddInParameter(dbCommand, "@p_codigoIndicador", DbType.String, CodigoIndicador)
        db.AddInParameter(dbCommand, "@p_fecha_inicial", DbType.Decimal, fechainicial)
        db.AddInParameter(dbCommand, "@p_fecha_final", DbType.Decimal, fechafinal)

        db.LoadDataSet(dbCommand, oPaisBE, "CotizacionVAC")

        Return oPaisBE

    End Function

    Public Function ReporteCuponera(ByVal CodigoMnemonico As String, ByVal TipoCuponera As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand
        If TipoCuponera = 0 Then
            dbCommand = db.GetStoredProcCommand("ReporteCuponeraNormal")
        Else
            dbCommand = db.GetStoredProcCommand("ReporteCuponeraEspecial")
        End If

        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, CodigoMnemonico)
        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function SeleccionarMnemonicoPorTipoRentaFija() As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand

        dbCommand = db.GetStoredProcCommand("SeleccionarMnemonico_PorTipoRentaFija")
        Dim oPaisBE As New DataSet
        db.LoadDataSet(dbCommand, oPaisBE, "Mnemonico")

        Return oPaisBE

    End Function
    Public Function SeleccionarMnemonicoPorTipoCuponera(ByVal TipoCuponera As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand

        dbCommand = db.GetStoredProcCommand("SeleccionarMnemonico_PorTipoCuponera")
        db.AddInParameter(dbCommand, "@p_tipoCuponera", DbType.String, TipoCuponera)

        Dim oPaisBE As New DataSet
        db.LoadDataSet(dbCommand, oPaisBE, "Mnemonico")

        Return oPaisBE

    End Function
End Class
