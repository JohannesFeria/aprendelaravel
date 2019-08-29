'Creado por: HDG OT 62087 Nro3-R09 20110110
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class IndicadorBenchmarkBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Sub Actualizar(ByVal objIndicadorBenchmarkBE As IndicadorBenchmarkBE.IndicadorBenchmarkRow, ByVal dataRequest As DataSet)
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objIndicadorBenchmarkBE, dataRequest}

        Try
            Dim daIndicadorBenchmark As New IndicadorBenchmarkDAM
            daIndicadorBenchmark.Actualizar(objIndicadorBenchmarkBE, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores)
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
    End Sub
End Class
