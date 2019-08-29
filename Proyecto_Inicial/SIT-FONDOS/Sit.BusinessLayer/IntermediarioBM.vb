Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class IntermediarioBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub
#Region "*/ Funciones Seleccionar */"

    Public Function Seleccionar(ByVal codigoIntermediario As String) As DataSet

    End Function

    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

    End Function

    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet


    End Function

    Public Function SeleccionarPorCodigoCustodio(ByVal codigoCustodio As String) As DataSet


    End Function

    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet


    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As IntermediarioBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            'Return New MercadoDAM
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
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoIntermediario As String, _
  ByVal tipoIntermediario As String, _
  ByVal descripcion As String, _
  ByVal codigoMercado As String, _
  ByVal codigoTercero As String, _
  ByVal codigoPortafolio As String, _
  ByVal codigoMoneda As String, _
  ByVal numeroCuenta As String, _
  ByVal codigoCustodio As String, _
  ByVal situacion As String, _
  ByVal dataRequest As DataSet) As IntermediarioBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCustodio, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New IntermediarioDAM().SeleccionarPorFiltro(codigoIntermediario, tipoIntermediario, descripcion, codigoMercado, codigoTercero, codigoPortafolio, codigoMoneda, numeroCuenta, codigoCustodio, situacion, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

    Public Function Insertar(ByVal oIntermediario As IntermediarioBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oIntermediario, dataRequest}
        Try
            Dim oIntermediarioDAM As New IntermediarioDAM
            oIntermediarioDAM.Insertar(oIntermediario, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oIntermadiario As IntermediarioBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oIntermadiario, dataRequest}
        Try
            Dim oIntermediarioDAM As New IntermediarioDAM
            actualizado = oIntermediarioDAM.Modificar(oIntermadiario, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function Eliminar(ByVal codigoIntermediario As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoIntermediario, dataRequest}
        Try
            Dim oIntermediarioDAM As New IntermediarioDAM
            eliminado = oIntermediarioDAM.Eliminar(codigoIntermediario, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
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

