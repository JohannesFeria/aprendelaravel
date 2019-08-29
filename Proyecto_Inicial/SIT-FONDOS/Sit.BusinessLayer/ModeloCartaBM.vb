Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ModeloCartaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoModelo As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ModeloCartaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoModelo, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ModeloCartaDAM().SeleccionarPorFiltro(codigoModelo, descripcion, situacion, dataRequest)
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

    Public Function SeleccionarPorOperacion(ByVal codigoOperacion As String, ByVal dataRequest As DataSet) As ModeloCartaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOperacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ModeloCartaDAM().SeleccionarPorOperacion(codigoOperacion, dataRequest)
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

    'Public Function SeleccionarPorOperacion2(ByVal codigoOperacion As String, ByVal dataRequest As DataSet) As ModeloCartaBE
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {codigoOperacion, dataRequest}
    '    Try
    '        RegistrarAuditora(parameters)
    '        Return New ModeloCartaDAM().SeleccionarPorOperacion2(codigoOperacion, dataRequest)
    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function

    Public Function Listar(ByVal dataRequest As DataSet) As ModeloCartaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ModeloCartaDAM().Listar()
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
    Public Function ListarDescripcionConcatenada(ByVal dataRequest As DataSet) As ModeloCartaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ModeloCartaDAM().ListarDescripcionConcatenada()
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

    Public Function SeleccionarCartaEstructuraPorModelo(ByVal codigoModelo As String) As DataSet
        Try
            Return New ModeloCartaDAM().SeleccionarCartaEstructuraPorModelo(codigoModelo)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

    Public Function Insertar(ByVal oModeloCarta As ModeloCartaBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModeloCarta, dataRequest}
        Try
            Dim oModeloCartaDAM As New ModeloCartaDAM
            oModeloCartaDAM.Insertar(oModeloCarta, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function Modificar(ByVal oModeloCarta As ModeloCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModeloCarta, dataRequest}
        Try
            Dim oModeloCartaDAM As New ModeloCartaDAM
            actualizado = oModeloCartaDAM.Modificar(oModeloCarta, dataRequest)
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
    Public Function Eliminar(ByVal codigoModelo As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoModelo, dataRequest}
        Try
            Dim oModeloCartaDAM As New ModeloCartaDAM
            eliminado = oModeloCartaDAM.Eliminar(codigoModelo, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarPorCodigoRuta(ByVal codigoRuta As String)

    End Function
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
End Class

