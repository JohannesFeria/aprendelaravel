Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ReportesInversionesBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub



    Public Function SeleccionarOperacionesRentaFija(ByVal param1 As String, ByVal param2 As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As RentaFijaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim oReporteOperacionRentaFijaBE As RentaFijaBE

            oReporteOperacionRentaFijaBE = New ReporteInversionesDAM().SeleccionarOperacionRentaFija(param1, param2, portafolio, dataRequest)
            RegistrarAuditora(parameters)
            Return oReporteOperacionRentaFijaBE
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

    'CMB OT 65473 20120919
    Public Function ReporteOperacionRentaFija(ByVal param1 As Decimal, ByVal portafolio As String, ByVal flag As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim dsAux As DataSet = New ReporteInversionesDAM().ReporteOperacionRentaFija(param1, portafolio, flag, dataRequest)
            RegistrarAuditora(parameters)
            Return dsAux
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

    Public Function SeleccionarPorGestor_RentaFija(ByVal param1 As String, ByVal param2 As String, ByVal dataRequest As DataSet) As RentaFijaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim oReporteOperacionRentaFijaBE As RentaFijaBE

            oReporteOperacionRentaFijaBE = New ReporteInversionesDAM().SeleccionarPorGestor_RentaFija(param1, param2, dataRequest)
            RegistrarAuditora(parameters)
            Return oReporteOperacionRentaFijaBE
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
    'CMB OT 65473 20120919
    Public Function ReporteOperacionRentaVariable(ByVal param1 As Decimal, ByVal portafolio As String, ByVal flag As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim dsAux As DataSet = New ReporteInversionesDAM().ReporteOperacionRentaVariable(param1, portafolio, flag, dataRequest)
            RegistrarAuditora(parameters)
            Return dsAux
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

    Public Function SeleccionarOperacionesRentaVariable(ByVal param1 As String, ByVal param2 As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As RentaVariableBE
        Try
            Dim oRentaVariableBE As RentaVariableBE
            oRentaVariableBE = New ReporteInversionesDAM().SeleccionarOperacionRentaVariable(param1, param2, portafolio, dataRequest)
            Return oRentaVariableBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorGestor_RentaVariable(ByVal param1 As String, ByVal param2 As String, ByVal dataRequest As DataSet) As RentaVariableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim oRentaVariableBE As RentaVariableBE

            oRentaVariableBE = New ReporteInversionesDAM().SeleccionarPorGestor_RentaVariable(param1, param2, dataRequest)
            RegistrarAuditora(parameters)
            Return oRentaVariableBE
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

    'CMB OT 65473 20120919
    Public Function ReporteOperacionDivisa(ByVal param1 As Decimal, ByVal portafolio As String, ByVal flag As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim dsAux As DataSet = New ReporteInversionesDAM().ReporteOperacionDivisa(param1, portafolio, flag, dataRequest)
            RegistrarAuditora(parameters)
            Return dsAux
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

    Public Function SeleccionarOperacionesDivisas(ByVal param1 As String, ByVal param2 As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DivisasBE
        Try
            Dim oDivisasBE As DivisasBE
            oDivisasBE = New ReporteInversionesDAM().SeleccionarOperacionDivisa(param1, param2, portafolio, dataRequest)
            Return oDivisasBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorGestor_Divisas(ByVal param1 As String, ByVal param2 As String, ByVal dataRequest As DataSet) As DivisasBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim oDivisasBE As DivisasBE

            oDivisasBE = New ReporteInversionesDAM().SeleccionarPorGestor_Divisa(param1, param2, dataRequest)
            RegistrarAuditora(parameters)
            Return oDivisasBE
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

    Public Function SeleccionarPorGestor(ByVal param1 As String, ByVal param2 As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim dsgestor As DataSet
        Try
            RegistrarAuditora(parameters)
            Dim oReporteInversionesDAM As New ReporteInversionesDAM
            dsgestor = oReporteInversionesDAM.SeleccionarPorGestor(param1, param2, portafolio, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsgestor
    End Function
    Public Function SeleccionarPorCorrelativo(ByVal param1 As String, ByVal param2 As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As PorCorrelativoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Dim oPorCorrelativoBE As PorCorrelativoBE

            oPorCorrelativoBE = New ReporteInversionesDAM().SeleccionarPorCorrelativo(param1, param2, portafolio, dataRequest)
            RegistrarAuditora(parameters)
            Return oPorCorrelativoBE
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

    'CMB OT 64769 20120329
    Public Function SeleccionarOperacionesCashCall(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fechaInicio, fechaFin, portafolio, dataRequest}
        Try
            Dim ds As DataSet
            ds = New ReporteInversionesDAM().SeleccionarOperacionesCashCall(fechaInicio, fechaFin, portafolio, dataRequest)
            Return ds
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

    '============================================================================
    ' CREADO POR  : Zoluxiones Consulting S.A.C (JVC)
    ' DESCRIPCIÓN : Obtiene datos de contratos de forwards
    ' FECHA DE CREACIÓN : 06/04/2009
    ' PARÁMETROS ENTRADA: p_CodigoPortafolioSBS: Código de portafolio SBS
    '                     p_CodigoMoneda	   : Moneda negociada
    '                     p_CodigoMonedaDestino: Moneda destino
    '	                  p_fechainicio        : fecha de inicio
    '                     p_fechafin           : fecha de fin
    '============================================================================
    Public Function InventarioForward(ByVal p_CodigoPortafolioSBS As String, _
                                      ByVal p_CodigoMoneda As String, _
                                      ByVal p_CodigoMonDes As String, _
                                      ByVal p_fechainicio As Decimal, _
                                      ByVal p_fechafin As Decimal) As DataTable
        Dim oReporteInversionesDAM As ReporteInversionesDAM
        Try
            oReporteInversionesDAM = New ReporteInversionesDAM
            Return oReporteInversionesDAM.InventarioForward(p_CodigoPortafolioSBS, p_CodigoMoneda, p_CodigoMonDes, p_fechainicio, p_fechafin)
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    ' OT 61609 REQ 37 20101119 PLD
    Public Function ImpuestoRenta(ByVal p_fechainicio As Decimal) As DataSet

        Dim dsgestor As DataSet
        Try
            dsgestor = New ReporteInversionesDAM().ImpuestoRenta(p_fechainicio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsgestor

    End Function
    Public Function SeleccionarOperacionRentaFijaExcel(ByVal param1 As String, ByVal param2 As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim ds As DataSet
            ds = New ReporteInversionesDAM().SeleccionarOperacionRentaFijaExcel(param1, param2, portafolio, dataRequest)
            Return ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
