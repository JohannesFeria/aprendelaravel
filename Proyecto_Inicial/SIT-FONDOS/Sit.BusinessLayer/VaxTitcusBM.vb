Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class VaxTitcusBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function SeleccionarPorCartera(ByVal cartera As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxTitcus As New DataSet
        Try
            oDSVaxTitcus = New VaxTitcusDAM().SeleccionarPorCartera(cartera, fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSVaxTitcus
    End Function
    Public Function GetCuentasPorCobrarPagarToVAX(ByVal portafolio As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {portafolio}
        Dim oDSVaxTitcus As New DataSet
        Try
            oDSVaxTitcus = New VaxTitcusDAM().GetCuentasPorCobrarPagarToVAX(portafolio, fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSVaxTitcus
    End Function
    Public Function Forward_GenerarArchivoCPP(ByVal portafolio As String, ByVal fecha As Decimal) As DataTable

        Dim parameters As Object() = {portafolio}
        Dim oDSVaxTitcus As New DataTable
        Try
            oDSVaxTitcus = New VaxTitcusDAM().Forward_GenerarArchivoCPP(portafolio, fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSVaxTitcus
    End Function
End Class
