Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class VaxCajaSITBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function SeleccionarPorCartera(ByVal cartera As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxCajaSIT As New DataSet
        Try
            oDSVaxCajaSIT = New VaxCajaSITDAM().SeleccionarPorCartera(cartera, fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSVaxCajaSIT
    End Function
End Class
