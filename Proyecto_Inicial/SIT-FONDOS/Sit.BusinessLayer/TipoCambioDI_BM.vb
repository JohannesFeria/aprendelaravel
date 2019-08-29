Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy

Public Class TipoCambioDI_BM
    Inherits InvokerCOM

    Public Function SeleccionarPorFiltros(ByVal codigoTipoCambio As String, ByVal codigoMonedaOrigen As String, ByVal codigoMonedaDestino As String, ByVal tipo As String, ByVal situacion As String, ByVal DataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(DataRequest)
        Dim parameters As Object() = {codigoTipoCambio, codigoMonedaOrigen, codigoMonedaDestino, tipo, situacion, DataRequest}
        Dim objDS As DataSet
        Try
            objDS = New TipoCambioDI_DAM().SeleccionarPorFiltros(codigoTipoCambio, codigoMonedaOrigen, codigoMonedaDestino, tipo, situacion, DataRequest)
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
        Return objDS
    End Function

    Public Function Insertar(ByVal ob As TipoCambioDI_BE, ByVal dataRequest As DataSet) As Boolean
        Dim respuesta As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {ob, dataRequest}
        Try
            Dim oTipoCambioDAM As New TipoCambioDI_DAM
            respuesta = oTipoCambioDAM.Insertar(ob, dataRequest)
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
        Return respuesta
    End Function

    Public Function Modificar(ByVal ob As TipoCambioDI_BE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {ob, dataRequest}
        Try
            Dim oTipoCambioDAM As New TipoCambioDI_DAM

            actualizado = oTipoCambioDAM.Modificar(ob, dataRequest)
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
        Return actualizado
    End Function

    Public Function Eliminar(ByVal codigoTipoCambioDI As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoCambioDI, dataRequest}
        Try
            Dim oTipoCambioDAM As New TipoCambioDI_DAM
            eliminado = oTipoCambioDAM.Eliminar(codigoTipoCambioDI, dataRequest)
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
        Return eliminado
    End Function

#Region " /* Funciones Personalizadas*/"
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region
End Class
