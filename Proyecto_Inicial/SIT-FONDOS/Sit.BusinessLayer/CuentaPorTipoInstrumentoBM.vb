Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class CuentaPorTipoInstrumentoBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "


    Public Function SeleccionarPorFiltro(ByVal codigoInstrumentoSBS As String, ByVal codigoMoneda As String, ByVal grupoContable As String, ByVal situacion As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As CuentasPorTipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInstrumentoSBS, codigoMoneda, grupoContable, situacion, dataRequest}
        Try

            Return New CuentaPorTipoInstrumentoDAM().SeleccionarPorFiltro(codigoInstrumentoSBS, codigoMoneda, grupoContable, situacion, portafolio, dataRequest)
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


    Public Function Seleccionar(ByVal Secuencial As Int32, ByVal dataRequest As DataSet) As CuentasPorTipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {Secuencial, dataRequest}
        Try

            Return New CuentaPorTipoInstrumentoDAM().Seleccionar(Secuencial, dataRequest)
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

    'RGF 20090113
    Public Function Listar(ByVal portafolio As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {portafolio, dataRequest}
        Try

            Return New CuentaPorTipoInstrumentoDAM().Listar(portafolio)
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
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oCuentaPorTipoInstrumentoBE As CuentasPorTipoInstrumentoBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCuentaPorTipoInstrumentoBE, dataRequest}
        Try
            Dim oCuentaPorTipoInstrumentoDAM As New CuentaPorTipoInstrumentoDAM

            codigo = oCuentaPorTipoInstrumentoDAM.Insertar(oCuentaPorTipoInstrumentoBE, dataRequest)
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

        Return codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oCuentaPorTipoInstrumentoBE As CuentasPorTipoInstrumentoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCuentaPorTipoInstrumentoBE, dataRequest}
        Try
            Dim oCuentaPorTipoInstrumentoDAM As New CuentaPorTipoInstrumentoDAM

            actualizado = oCuentaPorTipoInstrumentoDAM.Modificar(oCuentaPorTipoInstrumentoBE, dataRequest)
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

    Public Function Eliminar(ByVal secuencial As Int32, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {secuencial, dataRequest}
        Try
            Dim oCuentaPorTipoInstrumentoDAM As New CuentaPorTipoInstrumentoDAM

            eliminado = oCuentaPorTipoInstrumentoDAM.Eliminar(secuencial, dataRequest)
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
