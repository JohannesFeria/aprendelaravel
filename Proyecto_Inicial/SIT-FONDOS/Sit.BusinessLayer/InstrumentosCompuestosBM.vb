Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class InstrumentosCompuestosBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Function SeleccionarInstrumentosCompuestos(ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dataRequest}
        Dim oBEinst As InstrumentosCompuestosBE
        Try
            oBEinst = New InstrumentosCompuestosDAM().SeleccionarInstrumentosCompuestos(CodigoNemonico, dataRequest)
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

    Public Function IngresarModificarInstrumentosCompuestos(ByVal CodigoNemonico As String, ByVal dtDetalles As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dtDetalles, dataRequest}
        Dim blnResultado As Boolean = False
        Try
            blnResultado = New InstrumentosCompuestosDAM().IngresarModificarInstrumentosCompuestos(CodigoNemonico, dtDetalles, dataRequest)
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
        Dim oInsDAM As New InstrumentosCompuestosDAM
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

End Class
