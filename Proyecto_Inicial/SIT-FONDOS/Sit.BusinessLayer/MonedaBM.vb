Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

    ''' Clase para el acceso de los datos para Moneda tabla.
    Public  Class MonedaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoMoneda As String, ByVal descripcion As String, ByVal situacion As String, ByVal codigoIso As String, ByVal sinonimoIso As String, ByVal dataRequest As DataSet) As MonedaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMoneda, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MonedaDAM().SeleccionarPorFiltro(codigoMoneda, descripcion, situacion, codigoIso, sinonimoIso, dataRequest)
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
    Public Function SeleccionarPorCodigoSBS(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoSBS, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MonedaDAM().SeleccionarPorCodigoSBS(codigoSBS, dataRequest)
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

    Public Function Listar(Optional ByVal situacion As String = "") As DataSet
        Try
            Return New MonedaDAM().Listar(situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetMonedaMercadoPortafolio(ByVal codigoMercado As String, ByVal codigoPortafolio As String) As DataSet
        Try
            Return New MonedaDAM().GetMonedaMercadoPortafolio(codigoMercado, codigoPortafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As MonedaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MonedaDAM().Listar(dataRequest)
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


    Public Function ObtenerCodigoMonedaxSinonimo(ByVal SinonimoMoneda As String) As DataTable

        Dim dt As DataTable
        Try
            Dim oMonedaDAM As New MonedaDAM
            dt = oMonedaDAM.ObtenerCodigoMonedaxSinonimo(SinonimoMoneda)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dt
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oMoneda As MonedaBE, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMoneda, dataRequest}
        Try
            Dim oMonedaDAM As New MonedaDAM
            oMonedaDAM.Insertar(oMoneda, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

#Region " /* Funciones Modificar */ "
    Public Function Modificar(ByVal oMoneda As MonedaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMoneda, dataRequest}
        Try
            Dim oMonedaDAM As New MonedaDAM
            actualizado = oMonedaDAM.Modificar(oMoneda, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'RegistrarAuditora(parameters, ex)
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            '    Throw
            'End If
        End Try
        Return actualizado
    End Function
#End Region

#Region " /* Funciones Eliminar */ "
    Public Function Eliminar(ByVal codigoMoneda As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMoneda, dataRequest}
        Try
            Dim oMonedaDAM As New MonedaDAM
            eliminado = oMonedaDAM.Eliminar(codigoMoneda, dataRequest)
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
#End Region

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

