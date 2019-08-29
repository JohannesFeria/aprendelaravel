Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities




    ''' Clase para el acceso de los datos para Encaje tabla.
    Public  Class EncajeBM
    Inherits InvokerCOM
        Public Sub New()

        End Sub

    Public Function ListarParametros(ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Dim dsparametros As DataSet
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.ListarParametros()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function SeleccionarParametro(ByVal secuencial As String, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {secuencial, datarequest}
        Dim dsparametros As DataSet
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.SeleccionarParametro(secuencial)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function ModificarParametro(ByVal secuencial As String, ByVal nombre As String, ByVal valor As String, ByVal datarequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {secuencial, nombre, valor, datarequest}
        Dim dsparametros As String
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.ModificarParametro(secuencial, nombre, valor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal Fecha As Date, ByVal dataRequest As DataSet) As EncajeBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, Fecha, dataRequest}
        Dim oEncajeBE As EncajeBE


        Try

            Dim oEncaje As New EncajeDAM
            oEncajeBE = oEncaje.SeleccionarPorFiltro(codigoPortafolio, Fecha, dataRequest)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oEncajeBE

    End Function
    Public Function CalculoencajePeru(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal gobiernocentral As String, ByVal bancocentral As String, ByVal diaspromediar As Decimal, ByVal porcentajelimite As Decimal, ByVal diasvencimiento As Decimal, ByVal indicadorvalorfondo As String, ByVal valorindicadorfondo As Decimal, ByVal indicadorencajemantenido As String, ByVal valorindicadorencajemantenido As Decimal, ByVal indicadortotalactivos As String, ByVal valorindicadortotalactivos As Decimal, ByVal datarequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fechaproceso, gobiernocentral, bancocentral, diaspromediar, porcentajelimite, diasvencimiento, indicadorvalorfondo, valorindicadorfondo, indicadorencajemantenido, valorindicadorencajemantenido, indicadortotalactivos, valorindicadortotalactivos, datarequest}
        Dim dsparametros As Integer
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.CalculoencajePeru(portafolio, fechaproceso, gobiernocentral, bancocentral, diaspromediar, porcentajelimite, diasvencimiento, indicadorvalorfondo, valorindicadorfondo, indicadorencajemantenido, valorindicadorencajemantenido, indicadortotalactivos, valorindicadortotalactivos, datarequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function CalculoencajePeru2(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal gobiernocentral As String, ByVal bancocentral As String, ByVal diaspromediar As Decimal, ByVal porcentajelimite As Decimal, ByVal diasvencimiento As Decimal, ByVal indicadorvalorfondo As String, ByVal valorindicadorfondo As Decimal, ByVal indicadorencajemantenido As String, ByVal valorindicadorencajemantenido As Decimal, ByVal indicadortotalactivos As String, ByVal valorindicadortotalactivos As Decimal, ByVal datarequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fechaproceso, gobiernocentral, bancocentral, diaspromediar, porcentajelimite, diasvencimiento, indicadorvalorfondo, valorindicadorfondo, indicadorencajemantenido, valorindicadorencajemantenido, indicadortotalactivos, valorindicadortotalactivos, datarequest}
        Dim dsparametros As Integer
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.CalculoencajePeru2(portafolio, fechaproceso, gobiernocentral, bancocentral, diaspromediar, porcentajelimite, diasvencimiento, indicadorvalorfondo, valorindicadorfondo, indicadorencajemantenido, valorindicadorencajemantenido, indicadortotalactivos, valorindicadortotalactivos, datarequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function ExtornoEncajePeru(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal datarequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fechaproceso, datarequest}
        Dim dsparametros As Integer
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.ExtornoEncajePeru(portafolio, fechaproceso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function ObtenerNemonicosError() As DataSet
        Dim oDS As DataSet
        Try
            Dim oEncaje As New EncajeDAM
            oDS = oEncaje.ObtenerNemonicosError()
        Catch ex As Exception
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function ExisteEncajePeru(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal datarequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fechaproceso, datarequest}
        Dim dsparametros As Integer
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.ExisteEncajePeru(portafolio, fechaproceso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function

    Public Function ObtenerFechaUltimoEncaje(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal datarequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fechaproceso, datarequest}
        Dim dsparametros As Decimal
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.ObtenerFechaUltimoEncaje(portafolio, fechaproceso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Function ObtenerFechaT1Indicadores(ByVal codigoindicador As String, ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal datarequest As DataSet) As Decimal
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fechaproceso, datarequest}
        Dim dsparametros As Decimal
        Try
            Dim oEncaje As New EncajeDAM
            dsparametros = oEncaje.ObtenerFechaT1Indicadores(codigoindicador, portafolio, fechaproceso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsparametros
    End Function
    Public Sub ActualizarRentabilidad(ByVal portafolio As String, ByVal fecha As Decimal, ByVal numerocuotast As Decimal, ByVal valorcuotat As Decimal, ByVal numerocuotast1 As Decimal, ByVal valorcuotat1 As Decimal, ByVal compras As Decimal, ByVal ventas As Decimal, ByVal datarequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {portafolio, fecha, datarequest}

        Try
            Dim oEncaje As New EncajeDAM
            oEncaje.ActualizarRentabilidad(portafolio, fecha, numerocuotast, valorcuotat, numerocuotast1, valorcuotat1, compras, ventas)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Function ProximaFechaEncaje(ByVal FechaProceso As Decimal, ByVal datarequest As DataSet) As Decimal
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {FechaProceso, datarequest}
        Dim fecha As Decimal
        Try
            Dim oEncaje As New EncajeDAM
            fecha = oEncaje.ProximaFechaEncaje(FechaProceso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return fecha
    End Function
End Class

