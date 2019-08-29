Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class CalificacionInstrumentoBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal plazo As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, plazo, situacion, dataRequest}
        Try
            Dim oCalificacionInstrumentoBE As CalificacionInstrumentoBE
            oCalificacionInstrumentoBE = New CalificacionInstrumentoDAM().SeleccionarPorFiltro(descripcion, plazo, situacion, dataRequest)
            RegistrarAuditora(parameters)
            Return oCalificacionInstrumentoBE
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
    Public Function SeleccionarPorFiltro(ByVal codigoCalificacionInstrumento As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCalificacionInstrumento, descripcion, dataRequest}
        Try

            Return New CalificacionInstrumentoDAM().SeleccionarPorFiltro(codigoCalificacionInstrumento, descripcion, dataRequest)
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
    End Function
    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal tipoRenta As String, ByVal situacion As String, ByVal maduracion As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, tipoRenta, maduracion, situacion, dataRequest}
        Try
            Dim oCalificacionInstrumentoBE As CalificacionInstrumentoBE
            oCalificacionInstrumentoBE = New CalificacionInstrumentoDAM().SeleccionarPorFiltro(descripcion, tipoRenta, situacion, maduracion, dataRequest)
            RegistrarAuditora(parameters)
            Return oCalificacionInstrumentoBE
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

    Public Function Seleccionar(ByVal codigoCalificacionInstrumento As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCalificacionInstrumento, dataRequest}
        Try

            Return New CalificacionInstrumentoDAM().Seleccionar(codigoCalificacionInstrumento, dataRequest)
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
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New CalificacionInstrumentoDAM().Listar(dataRequest)
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
    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oCalificacionInstrumentoBE As CalificacionInstrumentoBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCalificacionInstrumentoBE, dataRequest}
        Try
            Dim oCalificacionInstrumentoDAM As New CalificacionInstrumentoDAM

            codigo = oCalificacionInstrumentoDAM.Insertar(oCalificacionInstrumentoBE, dataRequest)
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

        Return codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oCalificacionInstrumentoBE As CalificacionInstrumentoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCalificacionInstrumentoBE, dataRequest}
        Try
            Dim oCalificacionInstrumentoDAM As New CalificacionInstrumentoDAM

            actualizado = oCalificacionInstrumentoDAM.Modificar(oCalificacionInstrumentoBE, dataRequest)
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

        Return actualizado

    End Function

#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoCalificacionInstrumento As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCalificacionInstrumento, dataRequest}
        Try
            Dim oCalificacionInstrumentoDAM As New CalificacionInstrumentoDAM

            eliminado = oCalificacionInstrumentoDAM.Eliminar(codigoCalificacionInstrumento, dataRequest)
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
        Return eliminado
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region



   
End Class

