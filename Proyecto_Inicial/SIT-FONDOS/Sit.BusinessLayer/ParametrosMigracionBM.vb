Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class ParametrosMigracionBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function SeleccionarPorFiltro(ByVal codigoParametro As String, ByVal secuencial As Decimal, ByVal nombre As String, ByVal valor As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoParametro, secuencial, nombre, valor, situacion, dataRequest}
        Dim oParametrosMigracionBE As New DataSet
        Dim oParametrosMigracionDAM As New ParametrosMigracionDAM
        Try
            RegistrarAuditora(parameters)
            oParametrosMigracionBE = oParametrosMigracionDAM.SeleccionarPorFiltro(codigoParametro, secuencial, nombre, valor, situacion)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oParametrosMigracionBE
    End Function
End Class
