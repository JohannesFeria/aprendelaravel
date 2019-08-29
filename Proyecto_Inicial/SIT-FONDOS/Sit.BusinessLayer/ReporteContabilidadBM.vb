Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ReporteContabilidadBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function CobranzaCancelacionInversiones(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal param4 As String, ByVal dataRequest As DataSet, Optional ByVal StrCodigoMercado As String = "") As ReportesContabilidadBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oReportesContabilidadBE As ReportesContabilidadBE
            oReportesContabilidadBE = New ReportesContabilidadDAM().CobranzaCancelacionInversiones(param1, param2, param3, param4, dataRequest, StrCodigoMercado)
            RegistrarAuditora(parameters)
            Return oReportesContabilidadBE
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ValorizacionCartera(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReportesContabilidadBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oReportesContabilidadBE As ReportesContabilidadBE
            oReportesContabilidadBE = New ReportesContabilidadDAM().ValorizacionCartera(param1, param2, param3, dataRequest)
            RegistrarAuditora(parameters)
            Return oReportesContabilidadBE
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
    Public Function CompraVentaOperacionesInversion(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReportesContabilidadBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oReportesContabilidadBE As ReportesContabilidadBE
            oReportesContabilidadBE = New ReportesContabilidadDAM().CompraVentaOperacionesInversion(param1, param2, param3, dataRequest)
            RegistrarAuditora(parameters)
            Return oReportesContabilidadBE
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
    Public Function CompraVentaOperacionesInversionCabecera(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReporteContabilidadCabeceraBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oReportesContabilidadBE As ReporteContabilidadCabeceraBE
            oReportesContabilidadBE = New ReportesContabilidadDAM().CompraVentaOperacionesInversionCabecera(param1, param2, param3, dataRequest)
            RegistrarAuditora(parameters)
            Return oReportesContabilidadBE
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
    Public Function Administradora(ByVal param1 As Decimal, ByVal param2 As String, ByVal param3 As Decimal, ByVal dataRequest As DataSet) As ReportesContabilidadBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oReportesContabilidadBE As ReportesContabilidadBE
            oReportesContabilidadBE = New ReportesContabilidadDAM().Administradora(param1, param2, param3, dataRequest)
            RegistrarAuditora(parameters)
            Return oReportesContabilidadBE
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
    Public Function Resumen_Envio_PU_ADM(ByVal param1 As String, _
                                         ByVal param2 As Decimal, _
                                         ByVal param3 As Decimal, _
                                         ByVal dataRequest As DataSet) As ReportesResumenEnvioPUADM
        Dim oReportesResumenEnvioPUADM As ReportesResumenEnvioPUADM
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            oReportesResumenEnvioPUADM = New ReportesContabilidadDAM().Resumen_Envio_PU_ADM(param1, param2, param3, dataRequest)
            RegistrarAuditora(parameters)
            Return oReportesResumenEnvioPUADM
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
    Public Function Resumen_Envio_PU_FONDO(ByVal param1 As String, _
                                           ByVal param2 As Decimal, _
                                           ByVal param3 As Decimal, _
                                           ByVal dataRequest As DataSet) As ReportesResumenEnvioPUADM
        Dim oReportesResumenEnvioPUADM As ReportesResumenEnvioPUADM
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            oReportesResumenEnvioPUADM = New ReportesContabilidadDAM().Resumen_Envio_PU_FONDO(param1, param2, param3, dataRequest)
            RegistrarAuditora(parameters)
            Return oReportesResumenEnvioPUADM
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
    Public Function LotesResumenClaseInstrumento(ByVal CodigoPortafolioSBS As String, Fecha As Decimal) As DataTable
        Try
            Dim oReportes As New ReportesContabilidadDAM
            Return oReportes.LotesResumenClaseInstrumento(CodigoPortafolioSBS, Fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class