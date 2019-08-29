Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ParametriaReportesBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function ReporteCotizacionVAC(ByVal CodigoIndicador As String, ByVal fechainicial As Decimal, ByVal fechafinal As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoIndicador, fechainicial, fechafinal, dataRequest}
        Dim ds As DataSet
        Try
            Dim oReporte As New ParametriaReportesDAM
            ds = oReporte.ReporteCotizacionVAC(CodigoIndicador, fechainicial, fechafinal)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ds
    End Function
    Public Function ReporteCuponera(ByVal CodigoMnemonico As String, ByVal TipoCuponera As String, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoMnemonico, datarequest}
        Dim ds As DataSet
        Try
            Dim oReporte As New ParametriaReportesDAM
            ds = oReporte.ReporteCuponera(CodigoMnemonico, TipoCuponera)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ds
    End Function
    Public Function SeleccionarMnemonicoPorTipoRentaFija() As DataSet
         Dim ds As DataSet
        Try
            Dim oReporte As New ParametriaReportesDAM
            ds = oReporte.SeleccionarMnemonicoPorTipoRentaFija()


        Catch ex As Exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ds
    End Function

    Public Function SeleccionarMnemonicoPorTipoCuponera(ByVal TipoCuponera As String) As DataSet
        Dim ds As DataSet
        Try
            Dim oReporte As New ParametriaReportesDAM
            ds = oReporte.SeleccionarMnemonicoPorTipoCuponera(TipoCuponera)

        Catch ex As Exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ds
    End Function
End Class
