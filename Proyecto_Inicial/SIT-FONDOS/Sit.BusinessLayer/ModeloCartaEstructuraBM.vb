Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ModeloCartaEstructuraBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoModelo As String, ByVal dataRequest As DataSet) As Sit.BusinessEntities.ModeloCartaEstructuraBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoModelo, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ModeloCartaEstructuraDAM().SeleccionarPorFiltro(codigoModelo, dataRequest)
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

    Public Function Listar(ByVal dataRequest As DataSet) As Sit.BusinessEntities.ModeloCartaEstructuraBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ModeloCartaEstructuraDAM().Listar()

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


#End Region

    Public Function Insertar(ByVal oModeloCartaEstructura As Sit.BusinessEntities.ModeloCartaEstructuraBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModeloCartaEstructura, dataRequest}
        Try
            Dim oModeloCartaEstructuraDAM As New ModeloCartaEstructuraDAM
            oModeloCartaEstructuraDAM.Insertar(oModeloCartaEstructura, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function Modificar(ByVal oModeloCartaEstructura As Sit.BusinessEntities.ModeloCartaEstructuraBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModeloCartaEstructura, dataRequest}
        Try
            Dim oModeloCartaEstructuraDAM As New ModeloCartaEstructuraDAM
            actualizado = oModeloCartaEstructuraDAM.Modificar(oModeloCartaEstructura, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function
    Public Function Eliminar(ByVal oModeloCartaEstructura As Sit.BusinessEntities.ModeloCartaEstructuraBE, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModeloCartaEstructura, dataRequest}
        Try
            Dim oModeloCartaEstructuraDAM As New ModeloCartaEstructuraDAM
            eliminado = oModeloCartaEstructuraDAM.Eliminar(oModeloCartaEstructura, dataRequest)
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