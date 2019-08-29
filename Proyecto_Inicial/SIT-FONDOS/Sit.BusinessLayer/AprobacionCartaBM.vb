Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class AprobacionCartaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoModelo As String) As DataSet

    End Function

    Public Function SeleccionarPorCodigoModelo(ByVal codigoModelo As String) As DataSet


    End Function

    Public Function Listar() As DataSet

    End Function
#End Region

    Public Function Insertar(ByVal oAprobacionCarta As AprobacionCartaBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oAprobacionCarta, dataRequest}
        Try
            Dim oAprobacionCartaDAM As New AprobacionCartaDAM
            oAprobacionCartaDAM.Insertar(oAprobacionCarta, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oAprobacionCarta As AprobacionCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oAprobacionCarta, dataRequest}
        Try
            Dim oAprobacionCartaDAM As New AprobacionCartaDAM
            actualizado = oAprobacionCartaDAM.Modificar(oAprobacionCarta, dataRequest)
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

    Public Function Eliminar(ByVal codigoAprobacionCarta As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoAprobacionCarta, dataRequest}
        Try
            Dim oAprobacionCartaDAM As New AprobacionCartaDAM
            eliminado = oAprobacionCartaDAM.Eliminar(codigoAprobacionCarta, dataRequest)
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

End Class

