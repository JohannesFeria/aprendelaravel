Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities


    Public  Class NegocioBM
    Inherits InvokerCOM
        Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoNegocio As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As NegocioBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNegocio, descripcion, situacion, dataRequest}
        Dim oNegocioDAM As New NegocioDAM
        Dim oNegocioBE As NegocioBE

        Try

            oNegocioBE = oNegocioDAM.SeleccionarPorFiltro(codigoNegocio, descripcion, situacion, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return oNegocioBE

    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim obj As New DataSet
        Try
            obj = New NegocioDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oNegocio As NegocioBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oNegocio, dataRequest}
        Try
            Dim oNegocioDAM As New NegocioDAM
            oNegocioDAM.Insertar(oNegocio, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            '    Throw
            'End If
        End Try
    End Function
#End Region

#Region " /* Funciones Modificar */ "
    Public Function Modificar(ByVal oNegocio As NegocioBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oNegocio, dataRequest}
        Try
            Dim oNegocioDAM As New NegocioDAM
            actualizado = oNegocioDAM.Modificar(oNegocio, dataRequest)
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

    Public Function Eliminar(ByVal codigoTipoRenta As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoRenta, dataRequest}
        Try
            Dim oTipoRentaDAM As New TipoRentaDAM
            eliminado = oTipoRentaDAM.Eliminar(codigoTipoRenta, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            'Throw
            'End If
        End Try
        Return eliminado
    End Function

    Public Function Eliminar(ByVal codigoNegocio As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

        Dim oNegocioDAM As New NegocioDAM
        Dim parameters As Object() = {codigoNegocio, dataRequest}

        Try

            eliminado = oNegocioDAM.Eliminar(codigoNegocio, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            'Throw
            'End If
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

