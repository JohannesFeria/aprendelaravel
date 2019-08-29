Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ReporteGestionBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function InsertForward(ByVal codigoPortafolio As String, ByVal fechaProceso As Int32, _
                ByVal codigoSBS As String, ByVal moneda As String, ByVal tipoMov As String, ByVal indicadorForward As String, _
                ByVal monto As Decimal, ByVal precio As Decimal, ByVal fechaVencimiento As Integer, ByVal plazo As Integer, _
                ByVal modalidad As String, ByVal tipoCambio As Decimal, ByVal indicadorCaja As String, ByVal plaza As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim bEjecucion As Boolean
        Try
            Dim objreportegestion As New ReporteGestionDAM
            objreportegestion.InsertForward(codigoPortafolio, fechaProceso, codigoSBS, moneda, _
                tipoMov, indicadorForward, monto, precio, fechaVencimiento, plazo, modalidad, _
                tipoCambio, indicadorCaja, plaza, dataRequest)
            bEjecucion = True
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

        Return bEjecucion

    End Function
    Public Function UpdateForward(ByVal codigoPortafolio As String, ByVal fechaProceso As Int32, ByVal consecutivo As Integer, _
            ByVal codigoSBS As String, ByVal moneda As String, ByVal tipoMov As String, ByVal indicadorForward As String, _
            ByVal monto As Decimal, ByVal precio As Decimal, ByVal fechaVencimiento As Integer, ByVal plazo As Integer, _
            ByVal modalidad As String, ByVal tipoCambio As Decimal, ByVal indicadorCaja As String, ByVal plaza As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim bEjecucion As Boolean
        Try
            Dim objreportegestion As New ReporteGestionDAM
            objreportegestion.UpdateForward(codigoPortafolio, fechaProceso, consecutivo, codigoSBS, moneda, _
                tipoMov, indicadorForward, monto, precio, fechaVencimiento, plazo, modalidad, _
                tipoCambio, indicadorCaja, plaza, dataRequest)
            bEjecucion = True
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

        Return bEjecucion
    End Function
    Public Function GetForward(ByVal codigoPortafolio As String, ByVal fechaProceso As Int32, ByVal consecutivo As Integer, ByVal dataRequest As DataSet) As DataTable

        Dim oReporte As New DataTable
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.GetForward(codigoPortafolio, fechaProceso, consecutivo)
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
        Return oReporte
    End Function
    Public Function SeguimientoForwards(ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String, ByVal FechaVencimiento As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oReporte As New DataSet
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.SeguimientoForwards(sCodigoPortafolioSBS, sCodigoMoneda, FechaVencimiento)
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
        Return oReporte
    End Function

    Public Function VerificaInformacionAnexo(ByVal CodigoPortafolioSBS As String, ByVal FechaProceso As Decimal, ByVal NroAnexo As String, ByVal dataRequest As DataSet) As DataSet
        Dim oReporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.VerificaInformacionAnexo(CodigoPortafolioSBS, FechaProceso, NroAnexo)
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
        Return oReporte
    End Function

    Public Function GeneraInformacionAnexo(ByVal CodigoPortafolioSBS As String, ByVal FechaProceso As Decimal, ByVal NroAnexo As String, ByVal Reproceso As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim bEjecucion As Boolean
        Try
            Dim objreportegestion As New ReporteGestionDAM
            bEjecucion = objreportegestion.GeneraInformacionAnexo(CodigoPortafolioSBS, FechaProceso, NroAnexo, Reproceso, dataRequest)
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

        Return bEjecucion
    End Function

    Public Function ReporteAnexoIDI3A(ByVal fechaIni As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteAnexoIDI3A(fechaIni, portafolio)
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
        Return oreporte
    End Function
    Public Function ReporteAnexoIDI3B(ByVal fechaIni As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteAnexoIDI3B(fechaIni, portafolio)
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
        Return oreporte
    End Function
    Public Function ReporteAnexoIDI6(ByVal fechaIni As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteAnexoIDI6(fechaIni, portafolio)
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
        Return oreporte
    End Function
    Public Function ReporteAnexoIDI8(ByVal fechaIni As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteAnexoIDI8(fechaIni, portafolio)
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
        Return oreporte
    End Function
    Public Function ReporteAnexoIDI7(ByVal fechaIni As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteAnexoIDI7(fechaIni, portafolio)
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
        Return oreporte
    End Function
    Public Function ReporteAnexoIDI9(ByVal fechaIni As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteAnexoIDI9(fechaIni, portafolio)
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
        Return oreporte
    End Function


    Public Function SaldosInstrumentosPorEmpresa(ByVal fechaValoracion As Decimal, ByVal portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.SaldoInstrumentos(fechaValoracion, portafolio)
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
        Return oreporte
    End Function

    Public Function ListaTipoCambio(ByVal fechaCarga As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ListaTipoCambio(fechaCarga, dataRequest)
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
        Return oreporte
    End Function

    Public Function ListaPrecios(ByVal fechaCarga As Decimal, ByVal tipoSocio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ListaPrecios(fechaCarga, tipoSocio)
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
        Return oreporte
    End Function
    Public Function ListaPrecios2(ByVal fechaCarga As Decimal, ByVal tipoSocio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ListaPrecios2(fechaCarga, tipoSocio)
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
        Return oreporte
    End Function
    Public Function ListaPrecios3(ByVal fechaCarga As Decimal, ByVal tipoSocio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ListaPrecios3(fechaCarga, tipoSocio)
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
        Return oreporte
    End Function
    Public Function FlujoCaja(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.FlujoCaja(fechainicio, fechafin, portafolio, Mercado)
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
        Return oreporte
    End Function

    Public Function StockForwards(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.StockForwards(fechainicio, fechafin, portafolio, Mercado)
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
        Return oreporte
    End Function

    Public Function StockBCR(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.StockBCR(fechainicio, fechafin, portafolio, Mercado)
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
        Return oreporte
    End Function
    Public Function DuracionCarteraDetalle(ByVal portafolio As String, ByVal fechaValoracion As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.DuracionCarteraDetalle(portafolio, fechaValoracion, datarequest)
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
        Return oreporte
    End Function
    Public Function DuracionCarteraResumen(ByVal fechaValoracion As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.DuracionCarteraResumen(fechaValoracion, datarequest)
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
        Return oreporte
    End Function

    'RGF 20090108 Ya existe un metodo q invoca a este SP
    'Public Function Utilidad(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
    '    Dim oreporte As New DataSet
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
    '    Dim parameters As Object() = {datarequest}
    '    Try
    '        Dim objreportegestion As New ReporteGestionDAM
    '        oreporte = objreportegestion.Utilidad(fechainicio, fechafin, portafolio, Mercado)
    '        RegistrarAuditora(parameters)
    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return oreporte
    'End Function

    Public Function CarteraMoneda(ByVal fechainicio As Decimal, ByVal portafolio As String, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraMoneda(fechainicio, portafolio, Mercado)
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
        Return oreporte
    End Function
    Public Function CarteraCategoriaInstrumento(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraCategoriaInstrumento(fechainicio, Mercado, portafolio)
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
        Return oreporte
    End Function
    Public Function CarteraSector(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraSectorEmpresarial(fechainicio, Mercado)
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
        Return oreporte
    End Function
    Public Function CarteraEmisor(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraEmisor(fechainicio, Mercado)
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
        Return oreporte
    End Function
    Public Function CarteraTipoRenta(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraTipoRenta(fechainicio)
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
        Return oreporte
    End Function
    Public Function CarteraPlazoDetalle(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraPlazoDetalle(fechainicio, portafolio)
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
        Return oreporte
    End Function
    Public Function CarteraPlazoResumen(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal Mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraPlazoResumen(fechainicio)
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
        Return oreporte
    End Function
    Public Function CarteraRiesgoDetalle(ByVal portafolio As String, ByVal fechaValoracion As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraRiesgoDetalle(fechaValoracion, portafolio)
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
        Return oreporte
    End Function
    Public Function CarteraRiesgoResumen(ByVal fechaValoracion As Decimal, ByVal mercado As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraRiesgoResumen(fechaValoracion, mercado)
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
        Return oreporte
    End Function
    Public Function CarteraExterior(ByVal fechaValoracion As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraExterior(fechaValoracion)
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
        Return oreporte
    End Function
    Public Function GenerarReporteForwards(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteForwards(fecha, Portafolio)
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
        Return oreporte
    End Function
    Public Function GenerarReporteCertificadosDeposito(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteCertificadosDeposito(fecha, Portafolio)
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
        Return oreporte
    End Function
    Public Function GenerarReporteCompBenchmark(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteCompBenchmark(fecha, Portafolio)
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
        Return oreporte
    End Function

    Public Function GenerarReporteUtilidad(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteUtilidad(fechaInicio, fechaFin, Portafolio)
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
        Return oreporte
    End Function

    Public Function GenerarReporteEncaje(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal Nemonico As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteEncaje(fechaInicio, fechaFin, Portafolio, Nemonico)
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
        Return oreporte
    End Function

    'RGF 20080627
    Public Function GenerarReporteComposicionCarteraInstrumentoEmpresa(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteComposicionCarteraInstrumentoEmpresa(fecha, Portafolio)
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
        Return oreporte
    End Function

    Public Function GenerarReporteFlujoCaja(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteFlujoCaja(fecha, Portafolio)
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
        Return oreporte
    End Function


    Public Function GenerarReporteBcos(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteBcos(fecha, Portafolio)
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
        Return oreporte
    End Function


    Public Function GenerarReporteBmidas(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.GenerarReporteBmidas(fecha, Portafolio)
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
        Return oreporte
    End Function
    'Agregado por Yanina Pérez 20080612
    Public Function ConsultarVecTipoCambio_Por_Fecha(ByVal sFecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ConsultarVecTipoCambio_Por_Fecha(sFecha)
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
        Return oreporte
    End Function
    Public Function ComposicionCartera(ByVal sFecha As Decimal, ByVal Escenario As String, CodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ComposicionCartera(sFecha, Escenario, CodigoPortafolioSBS)
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
        Return oreporte
    End Function
    Public Function MonedaGestion(ByVal sFecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.MonedaGestion(sFecha)
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
        Return oreporte
    End Function
    Public Function CarteraUnidades(ByVal sFecha As Decimal, ByVal tipo As String, ByVal dataRequest As DataSet, ByVal CodigoMercado As String, ByVal TipoRenta As String) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraUnidades(sFecha, tipo, CodigoMercado, TipoRenta)
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
        Return oreporte
    End Function

    'RGF 20090107
    Public Function ReporteExterior(ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteExterior(fecha)
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
        Return oreporte
    End Function

    Public Function CarteraFondo3(ByVal sFechaInicio As Decimal, ByVal sFechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraFondo3(sFechaInicio, sFechaFin)
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
        Return oreporte
    End Function

    Public Function CarteraOperacion(ByVal sFecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraOperacion(sFecha)
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
        Return oreporte
    End Function

    Public Function CarteraEngrapado(ByVal sFechaInicio As Decimal, ByVal sFechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraEngrapado(sFechaInicio, sFechaFin)
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
        Return oreporte
    End Function
    Public Function UnidadesxFecha(ByVal sFechaInicio As Decimal, ByVal sFechaFin As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.UnidadesxFecha(sFechaInicio, sFechaFin, portafolio)
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
        Return oreporte
    End Function
    Public Function CompraVenta(ByVal sFechaInicio As Decimal, ByVal sFechaFin As Decimal, ByVal TipoInstrumento As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.CarteraComPraVenta(sFechaInicio, sFechaFin, TipoInstrumento)
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
        Return oreporte
    End Function

    Public Function VectorSerie(ByVal sFechaInicio As Decimal, ByVal sFechaFin As Decimal, ByVal TipoInstrumento As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.VectorSerie(sFechaInicio, sFechaFin, TipoInstrumento)
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
        Return oreporte
    End Function

    'RGF 20081105
    Public Function ListarConstitucionForwards(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ListarConstitucionForwards(fechaIni, fechaFin, Portafolio) 'OAB 20091007
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
        Return oreporte
    End Function
    Public Function DividendosRebatesLiberadas(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim oreporte As New DataSet
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.DividendosRebatesLiberadas(CodigoPortafolioSBS, FechaInicio, FechaFin)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function
    Public Function RentabilidadFondoEncaje(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim oreporte As New DataSet
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.RentabilidadFondoEncaje(CodigoPortafolioSBS, FechaInicio, FechaFin)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function
    Public Function LiquidacionComisionesAgentes(ByVal CodigoIntermediario As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.LiquidacionComisionesAgentes(CodigoIntermediario, FechaInicio, FechaFin)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    Public Function ConversionAcciones(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal CodigoIntermediaro As String) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.ConversionAcciones(CodigoPortafolioSBS, FechaOperacion, CodigoIntermediaro)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    Public Function PosicionMoneda(ByVal Fecha As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.PosicionMoneda(Fecha)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    'LETV 20091103
    Public Function PosicionMonedaCajaLocal(ByVal Fecha As Decimal) As DataTable
        Dim oReporte As New DataTable
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.PosicionMonedaCajaLocal(Fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    'LETV 20091103
    Public Function PosicionMonedaForward(ByVal Fecha As Decimal) As DataTable
        Dim oReporte As New DataTable
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.PosicionMonedaForward(Fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'LETV 20091216
    Public Function OperacionInversion(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.OperacionInversion(FechaInicio, FechaFin)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'LETV 20091217
    Public Function OperacionInversionPatrimonio(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataTable
        Dim oReporte As New DataTable
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.OperacionInversionPatrimonio(FechaInicio, FechaFin)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'RGF 20101124 OT 61609
    Public Function ReporteResumenRentabilidadEncajeTotalInstrumentos(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.ReporteResumenRentabilidadEncajeTotalInstrumentos(fechaInicio, fechaFin, Portafolio)
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
        Return oreporte
    End Function

    'HDG OT 62087 Nro5-R10 20110120
    Public Function LineasCreditoxEmisor(ByVal Fecha As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.LineasCreditoxEmisor(Fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'HDG OT 65018 20120418
    Public Function DetallePosicionBancos(ByVal CodigoPortafolio As String, ByVal CodigoIntermediario As String, _
                                                 ByVal FechaInicio As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.DetallePosicionBancos(CodigoPortafolio, CodigoIntermediario, FechaInicio)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'HDG OT 65195 20120515
    Public Function DetInteresesDividendos(ByVal CodigoPortafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim oReporteGestionDAM As New ReporteGestionDAM
            oReporte = oReporteGestionDAM.DetInteresesDividendos(CodigoPortafolio, FechaInicio, FechaFin)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'HDG OT 65195 20120515
    Public Function RentabilidadEncajeFondo(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            Dim objreportegestion As New ReporteGestionDAM
            oreporte = objreportegestion.RentabilidadEncajeFondo(fechaInicio, fechaFin, Portafolio)
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
        Return oreporte
    End Function

    'CMB OT 67168 20130430
    Public Function LineasContraparte(ByVal Fecha As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.LineasContraparte(Fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'CMB OT 67168 20130430
    Public Function LineasSettlement(ByVal Fecha As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReporteGestion As New ReporteGestionDAM
            oReporte = objReporteGestion.LineasSettlement(Fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function

    'HDG OT 67944 20130807
    Public Function Anexo_Swaps_RenovacionFWD(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim objReportegestion As New ReporteGestionDAM
            oReporte = objReportegestion.Anexo_Swaps_RenovacionFWD(fechaIni, fechaFin, Portafolio)
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    Public Function reporteRentabilidad(codigoPortafolio As String, fechaOperacion As Decimal, estado As String) As DataTable
        Try
            Dim daReporteRentabilidad As New ReporteGestionDAM
            Return daReporteRentabilidad.reporteRentabilidad(codigoPortafolio, fechaOperacion, estado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10927 - 21/11/2017 - Hanz Cocchi. Generación del reporte de rentabilidad de flujos
    Public Function reporteRentabilidad_Flujos(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal) As DataTable
        Try
            Dim daReporteRentabilidad As New ReporteGestionDAM
            Return daReporteRentabilidad.reporteRentabilidad_Flujos(codigoPortafolio, fechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10927 - Fin
    Public Function listarReporteValorCuota(codigoPortafolio As String, fechaInicial As Decimal, fechaFinal As Decimal) As DataTable
        Try
            Dim obValorCuota As New ReporteGestionDAM
            Return obValorCuota.listarReporteValorCuota(codigoPortafolio, fechaInicial, fechaFinal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function listarReporteValorCuotaTotalPorFondo(fechaInicial As Decimal, fechaFinal As Decimal) As DataTable
        Try
            Dim obValorCuota As New ReporteGestionDAM
            Return obValorCuota.listarReporteValorCuotaTotalPorFondo(fechaInicial, fechaFinal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function listarReporteValorCuotaTotalPorFondoSeriado(ByVal codigoPortafolio As String, ByVal fechaInicial As Decimal, ByVal fechaFinal As Decimal) As DataSet
        Try
            Dim obValorCuota As New ReporteGestionDAM
            Return obValorCuota.listarReporteValorCuotaTotalPorFondoSeriado(codigoPortafolio, fechaInicial, fechaFinal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function listarReporteCustodiaDPZ(codigoPortafolio As String, fecha As Decimal) As DataTable
        Try
            Dim objReporteCustodia As New ReporteGestionDAM
            Return objReporteCustodia.listarReporteCustodiaDPZ(codigoPortafolio, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function listarReporteCustodiaForward(codigoPortafolio As String, fecha As Decimal) As DataTable
        Try
            Dim objReporteCustodia As New ReporteGestionDAM
            Return objReporteCustodia.listarReporteCustodiaForward(codigoPortafolio, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function listarReporteCustodiaOpReporte(codigoPortafolio As String, fecha As Decimal) As DataTable
        Try
            Dim objReporteCustodia As New ReporteGestionDAM
            Return objReporteCustodia.listarReporteCustodiaOpReporte(codigoPortafolio, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function listarReporteCustodiaTenencia(codigoPortafolio As String, fecha As Decimal)
        Try
            Dim objReporteCustodia As New ReporteGestionDAM
            Return objReporteCustodia.listarReporteCustodiaTenencia(codigoPortafolio, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarReporteValorCuotaLimite(ByVal CodigoPortafolioSBS As String, ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal) As DataTable
        Try
            Dim obValorCuota As New ReporteGestionDAM
            Return obValorCuota.ListarReporteValorCuotaLimite(CodigoPortafolioSBS, FechaInicial, FechaFinal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarReporteValorCuotaLimiteMandatos(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim obValorCuota As New ReporteGestionDAM
            Return obValorCuota.ListarReporteValorCuotaLimiteMandatos(CodigoPortafolioSBS, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCarteraFondo(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal, ByVal Escenario As String) As DataTable
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.ListarCarteraFondo(CodigoPortafolioSBS, FechaValoracion, Escenario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarOption(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As DataTable
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.ListarOption(CodigoPortafolioSBS, FechaValoracion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCarteraFondoForward(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As DataTable
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.ListarCarteraFondoForward(CodigoPortafolioSBS, FechaValoracion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarTipoCambio(ByVal Fecha As Decimal) As DataTable
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.ListarTipoCambio(Fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Reporte_VectorVariacion(ByVal FechaHoy As Decimal, ByVal FechaAyer As Decimal) As DataSet
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.Reporte_VectorVariacion(FechaHoy, FechaAyer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Reporte_ValorCuotaVariacion(ByVal FechaInicial As Decimal) As DataSet
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.Reporte_ValorCuotaVariacion(FechaInicial)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Reporte_ValorCuotaVariacionFormula(ByVal MesVariacion As Integer, ByVal Alerta As Integer, ByVal FechaInicial As Decimal) As DataSet
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.Reporte_ValorCuotaVariacionFormula(MesVariacion, Alerta, FechaInicial)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Reporte_Dividendo(ByVal CodigoPortafolio As Integer, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.Reporte_Dividendo(CodigoPortafolio, FechaInicio, FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11004 - 21/12/2017 - Ian Pastor M.
    'Descripción: Obtiene las inversiones con sus comisiones
    Public Function ReporteComisionInvesiones(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Decimal, ByVal p_FechaFin As Decimal) As DataTable
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.ReporteComisionInvesiones(p_CodigoPortafolio, p_FechaInicio, p_FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11004 - Fin
    Public Function ReporteConsolidado(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As Decimal) As DataSet
        Try
            Dim objReportesGestionDAM As New ReporteGestionDAM
            Return objReportesGestionDAM.ReporteConsolidado(p_CodigoPortafolio, p_Fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ReporteOperacionesVencimientosOTC_Mandatos(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As DataTable
        Try
            Dim objReportesGestionDAM As New ReporteGestionDAM
            Return objReportesGestionDAM.ReporteOperacionesVencimientosOTC_Mandatos(p_CodigoPortafolio, p_FechaInicio, p_FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ReporteVencimientoOperaciones(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As DataTable
        Try
            Dim objReportesGestionDAM As New ReporteGestionDAM
            Return objReportesGestionDAM.ReporteVencimientoOperaciones(p_CodigoPortafolio, p_FechaInicio, p_FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarSwap(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As DataTable
        Try
            Dim objReporteGestionDAM As New ReporteGestionDAM
            Return objReporteGestionDAM.ListarSwap(CodigoPortafolioSBS, FechaValoracion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class

