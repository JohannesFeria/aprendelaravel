Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class SolicitudReversionBM
    Public Function ObtenerAreaUsuarioSisOpe() As DataTable
        Try
            'Return New SolicitudReversionDAM().ObtenerAreaUsuarioSisOpe().Tables(0)
            'Dim drViaPago() As DataRow = oCuentaEconomicaDAM.ObtenerViaPagoSisOpe("VIAPAGRES")
            Return New SolicitudReversionDAM().ObtenerAreaUsuarioSisOpe("AREUSU")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerEstadoAprobacionRegistroSolicitudReversion() As DataTable
        Try
            'Return New SolicitudReversionDAM().ObtenerAreaUsuarioSisOpe().Tables(0)
            'Dim drViaPago() As DataRow = oCuentaEconomicaDAM.ObtenerViaPagoSisOpe("VIAPAGRES")
            Return New SolicitudReversionDAM().ObtenerAreaUsuarioSisOpe("EST_SOL_REV")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'oSolicitudReversion = oSolicitudReversionBM.Seleccionar(estadoRegistro, fechaIni, fechaFin, DatosRequest)
    'Public Function Seleccionar(ByVal estadoRegistro As String, ByVal fechaIni As String, ByVal fechaFin As String, ByVal dataRequest As DataSet) As SolicitudReversionBE
    Public Function Seleccionar(ByVal estadoRegistro As String, ByVal fechaIni As String, ByVal dataRequest As DataSet) As SolicitudReversionBE
        Dim oSolicitudesReversion As SolicitudReversionBE
        Dim oSolicitudReversionDAM As New SolicitudReversionDAM

        Try
            'oSolicitudesReversion = oSolicitudReversionDAM.Seleccionar(estadoRegistro, fechaIni, fechaFin, dataRequest)
            oSolicitudesReversion = oSolicitudReversionDAM.Seleccionar(estadoRegistro, fechaIni, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return oSolicitudesReversion

    End Function

    Public Function Insertar(ByVal oSolicitudReversionBE As SolicitudReversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim oSolicitudReversionDAM As New SolicitudReversionDAM

        Try
            oSolicitudReversionDAM.Insertar(oSolicitudReversionBE, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function

    Public Function Modificar(ByVal oSolicitudReversionBE As SolicitudReversionBE, ByVal intIndice As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim oSolicitudReversionDAM As New SolicitudReversionDAM

        Try
            oSolicitudReversionDAM.Modificar(oSolicitudReversionBE, intIndice, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function

    Public Function Eliminar(ByVal oSolicitudReversionBE As SolicitudReversionBE, ByVal intIndice As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim oSolicitudReversionDAM As New SolicitudReversionDAM

        Try
            oSolicitudReversionDAM.Eliminar(oSolicitudReversionBE, intIndice, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function

    Public Function Aprobar(ByVal id As Integer, ByVal estado As String, ByVal motivoRechazo As String, ByVal dataRequest As DataSet) As Boolean
        Dim oSolicitudReversionDAM As New SolicitudReversionDAM

        Try
            oSolicitudReversionDAM.Aprobar(id, estado, motivoRechazo, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function

    Public Function SeleccionarSolicitudReversion(ByVal estado As String, ByVal dataRequest As DataSet) As SolicitudReversionBE
        Dim parameters As Object() = {dataRequest}

        Dim oSolicitudesReversion As SolicitudReversionBE
        Dim oSolicitudReversionDAM As New SolicitudReversionDAM

        Try

            'oCuentas = oCustodioDAM.SeleccionarCuentasDepositaria(codigoCustodio)
            'RegistrarAuditora(parameters)
            oSolicitudesReversion = oSolicitudReversionDAM.SeleccionarSolicitudReversion(estado, dataRequest)

        Catch ex As Exception

            'RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return oSolicitudesReversion

    End Function
End Class
