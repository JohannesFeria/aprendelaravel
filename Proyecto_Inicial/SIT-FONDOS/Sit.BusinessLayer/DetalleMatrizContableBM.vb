Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class DetalleMatrizContableBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function Seleccionar(ByVal strCodigoMatrizContable As String, ByVal dataRequest As DataSet) As DetalleMatrizContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoMatrizContable, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New DetalleMatrizContableDAM().Seleccionar(strCodigoMatrizContable, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarReporteMatrizContable(ByVal strFondo As String, ByVal strMatriz As String, ByVal CodigoMoneda As String, ByVal CodigoClaseInstrumento As String, ByVal CodigoOperacion As String, ByVal CodigoTipoInstrumento As String, ByVal CodigoModalidadPago As String, ByVal CodigoSectorEmpresarial As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strFondo, strMatriz, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New DetalleMatrizContableDAM().SeleccionarReporteMatrizContable(strFondo, strMatriz, CodigoMoneda, CodigoClaseInstrumento, CodigoOperacion, CodigoTipoInstrumento, CodigoModalidadPago, CodigoSectorEmpresarial)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Insertar(ByVal oDetalleMatrizContable As DetalleMatrizContableBE, ByVal Portafolio As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oDetalleMatrizContable, dataRequest}
        Try
            Dim oDetalleMatrizContableDAM As New DetalleMatrizContableDAM
            oDetalleMatrizContableDAM.Insertar(oDetalleMatrizContable, Portafolio, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oDetalleMatrizContable As DetalleMatrizContableBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oDetalleMatrizContable, dataRequest}
        Try
            Dim oDetalleMatrizContableDAM As New DetalleMatrizContableDAM
            oDetalleMatrizContableDAM.Modificar(oDetalleMatrizContable, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Listar() As DataSet

    End Function

    Public Function Eliminar(ByVal ob As DetalleMatrizContableBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {ob, dataRequest}
        Try
            Dim oDetalleMatrizContableDAM As New DetalleMatrizContableDAM
            oDetalleMatrizContableDAM.Eliminar(ob, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Duplicar(ByVal strCodigoDetalleMatriz As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoDetalleMatriz, dataRequest}
        Try
            Dim oDetalleMatrizContableDAM As New DetalleMatrizContableDAM
            oDetalleMatrizContableDAM.Duplicar(strCodigoDetalleMatriz, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'HSP 20151015
    Public Function Eliminar(ByVal sCodigoCabeceraMatriz As String) As Boolean
        Dim esElimado As Boolean = False
        Try
            Dim oDetalleMatrizContableDAM As New DetalleMatrizContableDAM
            esElimado = oDetalleMatrizContableDAM.EliminarDetalle(sCodigoCabeceraMatriz)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return esElimado
    End Function

End Class

