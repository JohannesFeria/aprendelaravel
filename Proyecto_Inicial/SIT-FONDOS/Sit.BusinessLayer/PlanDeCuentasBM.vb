Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PlanDeCuentasBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal StrCodigoPortafolioSBS As String, ByVal periodo As Integer, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoPortafolioSBS, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Dim oPlanDeCuentasDAM As New PlanDeCuentasDAM
            Return oPlanDeCuentasDAM.SeleccionarPorFiltro(StrCodigoPortafolioSBS, periodo, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Seleccionar(ByVal strCodigoPlanCuenta As String, ByVal dataRequest As DataSet) As PlanDeCuentasBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoPlanCuenta, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Dim oPlanDeCuentasDAM As New PlanDeCuentasDAM
            Return oPlanDeCuentasDAM.Seleccionar(strCodigoPlanCuenta, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Buscar(ByVal CuentaContable As String, ByVal DescripcionCuenta As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CuentaContable, DescripcionCuenta, dataRequest}
        Dim oPlanDeCuentasDAM As New PlanDeCuentasDAM
        Dim oPlanDeCuentasBE As New DataSet
        Try
            RegistrarAuditora(parameters)
            oPlanDeCuentasBE = oPlanDeCuentasDAM.Buscar(CuentaContable, DescripcionCuenta, portafolio, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oPlanDeCuentasBE
    End Function
End Class

