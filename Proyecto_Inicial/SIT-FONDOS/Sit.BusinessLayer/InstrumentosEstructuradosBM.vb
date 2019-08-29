Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class InstrumentosEstructuradosBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Function SeleccionarInstrumentosEstructurados(ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As InstrumentosEstructuradosBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dataRequest}
        Dim oBEinst As InstrumentosEstructuradosBE
        Try
            oBEinst = New InstrumentosEstructuradosDAM().SeleccionarInstrumentosEstructurados(CodigoNemonico, dataRequest)
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
        Return oBEinst
    End Function

    Public Function IngresarModificarInstrumentosEstructurados(ByVal CodigoNemonico As String, ByVal dtDetalles As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dtDetalles, dataRequest}
        Dim blnResultado As Boolean = False
        Try
            blnResultado = New InstrumentosEstructuradosDAM().IngresarModificarInstrumentosEstructurados(CodigoNemonico, dtDetalles, dataRequest)
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
        Return blnResultado
    End Function

    Public Function Eliminar(ByVal strNemonico As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strNemonico}
        Dim oInsDAM As New InstrumentosEstructuradosDAM
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            oInsDAM.Eliminar(strNemonico, dataRequest)
            RegistrarAuditora(parameters)
            blnResul = True
        Catch ex As Exception
            RegistrarAuditora(parameters)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return blnResul
    End Function

    'RGF 20090331
    Public Function ListarUnidadesBloqueadas(ByVal codigoNemonico As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataTable
        Dim oLimite As New DataTable
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {codigoNemonico, portafolio, dataRequest}
        Try
            Dim oLimiteDAM As New InstrumentosEstructuradosDAM
            oLimite = oLimiteDAM.ListarUnidadesBloqueadas(codigoNemonico, portafolio, dataRequest)

            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oLimite
    End Function

    'LETV 20091109
    Public Function ListarInstrumentoEstructuradoNocional(ByVal codigoNemonico As String) As DataTable

        ' Dim parameters As Object() = {CodigoNemonico, dataRequest}
        Dim oBEinst As DataTable
        Try
            oBEinst = New InstrumentosEstructuradosDAM().ListarInstrumentoEstructuradoNocional(codigoNemonico)
            ' RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            '   RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oBEinst
    End Function

    'LETV 20091110
    Public Function InsertarInstrumentoEstructuradosNocional(ByVal oListaIENocional As DataTable) As Boolean
        Dim resultado As Boolean
        Try
            resultado = New InstrumentosEstructuradosDAM().InsertarInstrumentoEstructuradosNocional(oListaIENocional)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function

    'LETV 20091110
    Public Function ModificarInstrumentoEstructuradosNocional(ByVal oListaIENocional As DataTable) As Boolean
        Dim resultado As Boolean
        Try
            resultado = New InstrumentosEstructuradosDAM().ModificarInstrumentoEstructuradosNocional(oListaIENocional)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function
End Class
