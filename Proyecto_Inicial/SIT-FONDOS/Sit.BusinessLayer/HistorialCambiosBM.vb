'Creado por: HDG OT 64016 20111017
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class HistorialCambiosBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Sub InicializarHistorialCambios(ByRef oRow As HistorialCambiosBE.HistorialCambiosRow)
        Try
            Dim oHistorialCambiosDAM As New HistorialCambiosDAM
            oHistorialCambiosDAM.InicializarHistorialCambios(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Public Function Insertar(ByVal oHistorialCambios As HistorialCambiosBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oHistorialCambios, dataRequest}
        Try
            Dim oHistorialCambiosDAM As New HistorialCambiosDAM
            oHistorialCambiosDAM.Insertar(oHistorialCambios, dataRequest)
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
    End Function

End Class
