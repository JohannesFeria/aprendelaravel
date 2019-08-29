Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class MovimientoPersonalBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Sub InicializarMovimientoPersonal(ByRef oRow As MovimientoPersonalBE.MovimientoPersonalRow)
        Try
            Dim oMovimientoPersonalDAMAs As New MovimientoPersonalDAM
            oMovimientoPersonalDAMAs.InicializarMovimientoPersonal(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInterno, situacion, dataRequest}
        Try
            Dim oMovimientoPersonalDAMAs As New MovimientoPersonalDAM
            Dim dsAux As DataSet
            dsAux = oMovimientoPersonalDAMAs.SeleccionarPorFiltro(codigoInterno, situacion, dataRequest)
            RegistrarAuditora(parameters)
            Return dsAux
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oMovimientoPersonalBE As MovimientoPersonalBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMovimientoPersonalBE, dataRequest}
        Try
            Dim oMovimientoPersonalDAM As New MovimientoPersonalDAM
            bolResult = oMovimientoPersonalDAM.Modificar(oMovimientoPersonalBE, dataRequest)
            RegistrarAuditora(parameters)
            Return bolResult
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Insertar(ByVal oMovimientoPersonalBE As MovimientoPersonalBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMovimientoPersonalBE, dataRequest}
        Try
            Dim oMovimientoPersonalDAM As New MovimientoPersonalDAM
            bolResult = oMovimientoPersonalDAM.Insertar(oMovimientoPersonalBE, dataRequest)
            RegistrarAuditora(parameters)
            Return bolResult
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class
