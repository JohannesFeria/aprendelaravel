Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Transactions

Public Class OrdenPreOrdenInversionBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Sub EliminarCustodioCarteraKardex(ByVal strPagina As String, ByVal strCodigoOI As String, ByVal strCodigoPortafolioSBS As String, ByVal strTipoAccion As String, ByVal strCategoriaInstrumento As String, ByVal datarequest As DataSet, ByVal codPortafolioMultifondo As String, Optional ByVal strOperacion As String = "")
        Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
        Dim dtAux As DataTable
        Dim strSeparador As String = "&"
        Dim strDatos As String = ""
        Dim i As Integer
        dtAux = daOrdenInversion.SeleccionarPorFiltro_ValorCustodioOI(strCodigoOI, strCodigoPortafolioSBS, "A").Tables(0)
        If dtAux.Rows.Count > 0 Then
            For i = 0 To dtAux.Rows.Count - 1
                If strDatos = "" Then
                    strDatos = dtAux.Rows(i)(0) + strSeparador + CType(dtAux.Rows(i)(1), String).Replace(",", ".")
                Else
                    strDatos = strDatos + strSeparador + dtAux.Rows(i)(0) + strSeparador + CType(dtAux.Rows(i)(1), String).Replace(",", ".")
                End If
            Next
            ObtieneCustodioCarteraKardex(strPagina, strDatos, strCodigoOI, strCodigoPortafolioSBS, "E", strCategoriaInstrumento, datarequest, codPortafolioMultifondo)
        End If
    End Sub
    Public Sub ObtieneCustodioCarteraKardex(ByVal strPagina As String, ByVal strCadena As String, ByVal strCodigoOI As String, ByVal strCodigoPortafolioSBS As String, ByVal strTipoAccion As String, ByVal strCategoriaInstrumento As String, ByVal datarequest As DataSet, ByVal codPortafolioMultifondo As String, Optional ByVal strOperacion As String = "")
        Dim strSeparador As String = "&"
        Dim strDatos() As String = strCadena.Split(strSeparador)
        Dim i As Integer
        Dim strCodigoCustodio As String
        Dim decMontoCustodio As Decimal
        Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
        ' 1:compra - 2:venta
        For i = 0 To strDatos.Length - 1
            strCodigoCustodio = strDatos(i)
            If strDatos(i + 1).ToString = "" Then
                decMontoCustodio = 0
            Else
                strDatos(i + 1) = strDatos(i + 1).Replace(",", "")
                strDatos(i + 1).Replace(".", ",")
                decMontoCustodio = Convert.ToDecimal(strDatos(i + 1))
            End If
            i = i + 1
            If strTipoAccion = "I" Then         'INGRESAR
                If strCategoriaInstrumento <> "DP" Then
                    If strOperacion = "1" Then
                        strOperacion = "C"
                    ElseIf (strOperacion = "2") Then
                        strOperacion = "V"
                    Else
                        strOperacion = "C"
                    End If
                Else
                    strOperacion = "C"
                End If
                daOrdenInversion.ActualizarCustodioCarteraKardex(strPagina, strCodigoOI, strCodigoCustodio, decMontoCustodio, strOperacion, strCategoriaInstrumento)
                Dim decValor As Decimal = 0
                decValor = daOrdenInversion.ObtenerValorCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio)
                If decValor = 0 Then
                    daOrdenInversion.InsertarCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio, decMontoCustodio, datarequest)
                Else
                    daOrdenInversion.ModificarCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio, decMontoCustodio + decValor, datarequest)
                End If
            ElseIf strTipoAccion = "M" Then     'MODIFICAR                
                If strCategoriaInstrumento <> "DP" Then
                    If strOperacion = "1" Then
                        strOperacion = "C"
                    ElseIf (strOperacion = "2") Then
                        strOperacion = "V"
                    Else
                        strOperacion = "C"
                    End If
                Else
                    strOperacion = "C"
                End If
                daOrdenInversion.ActualizarCustodioCarteraKardex(strPagina, strCodigoOI, strCodigoCustodio, decMontoCustodio, strOperacion, strCategoriaInstrumento)
                Dim decValor As Decimal = 0
                decValor = daOrdenInversion.ObtenerValorCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio)
                If decValor = 0 Then
                    daOrdenInversion.InsertarCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio, decMontoCustodio, datarequest)
                Else
                    If strCategoriaInstrumento = "OR" Or strCategoriaInstrumento = "FD" Then
                        daOrdenInversion.ModificarCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio, decMontoCustodio + decValor, datarequest)
                    Else
                        daOrdenInversion.ModificarCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio, decMontoCustodio, datarequest)
                    End If
                End If
            ElseIf strTipoAccion = "E" Then     'ELIMINAR
                If strCategoriaInstrumento <> "DP" And strCategoriaInstrumento <> "FD" And strCategoriaInstrumento <> "OR" Then
                    Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
                    oRow = daOrdenInversion.ListarOrdenesInversionPorCodigoOrden(strCodigoOI, strCodigoPortafolioSBS, codPortafolioMultifondo).Tables(0).Rows(0)
                    If oRow.CodigoOperacion = "1" Then  'SE REALIZA LO INVERSO
                        strOperacion = "V"
                    ElseIf (strOperacion = "2") Then
                        strOperacion = "C"
                    Else
                        strOperacion = "V"
                    End If
                Else
                    strOperacion = "V"
                End If
                daOrdenInversion.ActualizarCustodioCarteraKardex(strPagina, strCodigoOI, strCodigoCustodio, decMontoCustodio, strOperacion, strCategoriaInstrumento)
                daOrdenInversion.EliminarCustodioOI(strCodigoOI, strCodigoPortafolioSBS, strCodigoCustodio, decMontoCustodio, datarequest)
            End If
        Next
    End Sub
    Public Function ValidaCantidadIE(ByVal codigoPortafolio As String, ByVal codigoCustodio As String, _
        ByVal codigoNemonico As String, ByVal fecha As String, ByVal cantidad As Decimal) As Boolean
        Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
        Dim dtIE As DataTable
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim ok As Boolean
        Dim saldoActual As Decimal
        Dim unidadCantidad As Decimal
        Try
            ds = daOrdenInversion.GetItemInstrumentoEstructurado(codigoPortafolio, codigoCustodio, codigoNemonico, fecha)
            dtIE = ds.Tables(0)
            ok = False
            For Each dr In dtIE.Rows
                saldoActual = CType(dr("SaldoActual"), Decimal)
                unidadCantidad = CType(dr("Cantidad"), Decimal)
                If (unidadCantidad * cantidad) > saldoActual Then
                    ok = False
                    Exit For
                Else
                    ok = True
                End If
            Next
            'Verifica saldos en SaldosCarteraTitulo
            ok = False
            dtIE = ds.Tables(1)
            For Each dr In dtIE.Rows
                saldoActual = CType(dr("SaldoActual"), Decimal)
                unidadCantidad = CType(dr("Cantidad"), Decimal)
                If (unidadCantidad * cantidad) > saldoActual Then
                    ok = False
                    Exit For
                Else
                    ok = True
                End If
            Next
        Catch ex As Exception
            ok = False
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ok
    End Function
    Public Function InsertarOI(ByVal objOI As OrdenPreOrdenInversionBE, ByVal strPagina As String, ByVal strCustodios As String, ByVal dataRequest As DataSet) As String
        Dim strCodigoOI As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objOI, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            strCodigoOI = daOrdenInversion.InsertarOI(objOI, strPagina, dataRequest)
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
        Return strCodigoOI
    End Function
    Public Sub ModificarOI(ByVal objOI As OrdenPreOrdenInversionBE, ByVal strPagina As String, ByVal strCustodios As String, ByVal dataRequest As DataSet)
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.ModificarOI(objOI, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'RGF 20080929 se agrego el parametro: strCodigoMotivoCambio
    'Public Sub EliminarOI(ByVal strCodigoOrden As String, ByVal strFondo As String, ByVal strCustodios As String, ByVal strCategoriaInstrumento As String, ByVal dataRequest As DataSet)
    Public Sub EliminarOI(ByVal strCodigoOrden As String, ByVal strFondo As String, ByVal strCodigoMotivoCambio As String, ByVal dataRequest As DataSet)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {strCodigoOrden, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            Using transaction As New TransactionScope
                daOrdenInversion.EliminarOI(strCodigoOrden, strFondo, strCodigoMotivoCambio, dataRequest)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim dtPreOrden As DataTable = oPrevOrdenInversionBM.SeleccionarPorCodigoOrden(strCodigoOrden, dataRequest).Tables(0)

                If dtPreOrden.Rows.Count > 0 Then
                    Dim codigoPreOrden As String = dtPreOrden.Rows(0)("CodigoPrevOrden").ToString()
                    oPrevOrdenInversionBM.Extornar(codigoPreOrden, "8", "Se Elimina las preordenes asociadas a una orden.", dataRequest)
                End If
                RegistrarAuditora(parameters)
                transaction.Complete()
            End Using
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Sub CorreccionDepositoPlazo(ByVal strCodigoOrden As String, ByVal strCodigoFondo As String, ByVal strDiasPlazo As String, ByVal decTasaPorc As Decimal, ByVal strCodigoSBS As String, _
    ByVal decMontoOperacion As Decimal, ByVal strCodigoTipoCupon As String, ByVal decFechaContrato As Decimal, ByVal dataRequest As DataSet, ByVal CodigoTercero As String, _
    ByVal CodigoNemonico As String)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoOrden, strCodigoFondo, strDiasPlazo, decTasaPorc, strCodigoSBS, decMontoOperacion, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.CorreccionDepositoPlazo(strCodigoOrden, strCodigoFondo, strDiasPlazo, decTasaPorc, strCodigoSBS, decMontoOperacion, _
            strCodigoTipoCupon, decFechaContrato, dataRequest, CodigoTercero, CodigoNemonico)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se cambia retorno de función de boolean a integer | 11/07/18 
    Public Function ExtornaOrdenInversionConfirmada(ByVal codigoPortafolio As String, ByVal numeroOrden As String, ByVal dataRequest As DataSet) As Integer
        Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
        Dim parameters As Object() = {numeroOrden, dataRequest}
        Dim result As Integer
        Try

            result = daOrdenInversion.ExtornaOrdenInversionConfirmada(codigoPortafolio, numeroOrden, dataRequest)
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            'oPrevOrdenInversionBM.Extornar("", "8", "", dataRequest)
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
        Return result
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se cambia retorno de función de boolean a integer | 11/07/18 
    Public Function ObtenerValorCustodioOI(ByVal codigoOrden As String, ByVal fondo As String, ByVal codigoCustodio As String, ByVal datarequest As DataSet) As Decimal
        Dim decValor As Decimal = 0.0
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {codigoOrden, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            decValor = daOrdenInversion.ObtenerValorCustodioOI(codigoOrden, fondo, codigoCustodio)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return decValor
    End Function
    Public Function SeleccionarPorFiltro_CustodioOI(ByVal codigoOrden As String, ByVal fondo As String, ByVal datarequest As DataSet, ByVal situacion As String) As DataSet
        Dim oDS As DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {codigoOrden, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            oDS = daOrdenInversion.SeleccionarPorFiltro_ValorCustodioOI(codigoOrden, fondo, situacion)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return oDS
    End Function
    Public Function OtroSeleccionarPorFiltro_CustodioOI(ByVal codigoOrden As String, ByVal fondo As String, ByVal datarequest As DataSet) As DataSet
        Dim oDS As DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {codigoOrden, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            oDS = daOrdenInversion.OtroSeleccionarPorFiltro_ValorCustodioOI(codigoOrden, fondo)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return oDS
    End Function
    Public Function ListarOrdenesInversionPorCodigoOrden(ByVal codigoOrden As String, ByVal fondo As String, ByVal datarequest As DataSet, ByVal codPortafolioMultifondo As String, Optional ByVal LlamadoForward As String = "N") As OrdenPreOrdenInversionBE
        Dim objOIBE As New OrdenPreOrdenInversionBE
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            objOIBE = daOrdenInversion.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, codPortafolioMultifondo, LlamadoForward)
            Return objOIBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarOrdenesInversionPorCodigoOrden(ByVal codigoOrden As String, ByVal codigoPortafolio As String) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, codigoPortafolio)
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function ListarOrdenesInversionPorFecha(ByVal fecha As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal estado As String, ByVal RegSBS As String, ByVal liqAntFon As String, ByVal datarequest As DataSet) As DataSet  'HDG OT 64767 20120222
        Dim oDS As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {fecha, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            oDS = daOrdenInversion.ListarOrdenesInversionPorFecha(fecha, fechaFin, portafolio, estado, RegSBS, liqAntFon)   'HDG OT 64767 20120222
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return oDS
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
    Public Function ConsultaOrdenesPreOrdenes(ByVal correlativo As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal codigooperacion As String, ByVal codigotipoinstrumento As String, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String, ByVal tiporenta As String, ByVal portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim objOIBE As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {correlativo, fechainicio, fechafin, codigooperacion, codigotipoinstrumento, nemonico, isin, sbs, tiporenta, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            objOIBE = daOrdenInversion.ConsultaOrdenesPreOrdenes(correlativo, fechainicio, fechafin, codigooperacion, codigotipoinstrumento, nemonico, isin, sbs, tiporenta, portafolio)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return objOIBE
    End Function
    Public Function ConsultaOrdenesPreOrdenes(ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal codigooperacion As String, _
    ByVal codigotipoinstrumento As String, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String, ByVal tiporenta As String, ByVal portafolio As String, _
    ByVal codigoOrden As String, ByVal datarequest As DataSet) As DataSet
        Dim objOIBE As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {fechainicio, fechafin, codigooperacion, codigotipoinstrumento, nemonico, isin, sbs, tiporenta, portafolio, codigoOrden, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            objOIBE = daOrdenInversion.ConsultaOrdenesPreOrdenes(fechainicio, fechafin, codigooperacion, codigotipoinstrumento, nemonico, isin, sbs, tiporenta, portafolio, codigoOrden)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return objOIBE
    End Function
    Public Function ConsultaKardex(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String, ByVal datarequest As DataSet) As DataSet
        Dim objOIBE As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {portafolio, fechainicio, fechafin, nemonico, isin, sbs, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            objOIBE = daOrdenInversion.ConsultaKardex(portafolio, fechainicio, fechafin, nemonico, isin, sbs)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return objOIBE
    End Function
    Public Function ConsultaOperacionesRealizadas(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String, ByVal datarequest As DataSet) As DataSet   'HDG INC 64085	20111011
        Dim objOIBE As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {portafolio, fechainicio, nemonico, isin, sbs, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            objOIBE = daOrdenInversion.ConsultaOperacionesRealizadas(portafolio, fechainicio, fechafin, nemonico, isin, sbs) 'HDG INC 64085	20111011
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return objOIBE
    End Function
    Public Function ConsultaHistoricaPreOrdenes(ByVal Portafolio As String, ByVal fechaBusqueda As Decimal, ByVal codigooperacion As String, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String, ByVal estado As String, ByVal datarequest As DataSet) As DataTable
        Dim objOIBE As New DataTable
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {Portafolio, fechaBusqueda, codigooperacion, nemonico, isin, sbs, estado, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            objOIBE = daOrdenInversion.ConsultaHistoricaPreOrdenes(Portafolio, fechaBusqueda, codigooperacion, nemonico, isin, sbs, estado).Tables(0)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return objOIBE
    End Function
    Public Function ListarOrdenesCabecera(ByVal CategoriaInstrumento As String, ByVal FechaOperacion As Decimal, ByVal CodigoSBS As String, ByVal CodigoISIN As String, ByVal CodigoMnemonico As String, ByVal Portafolio As String, ByVal Operacion As String, ByVal Moneda As String, ByVal accion As String, ByVal dataRequest As DataSet) As DataSet
        Dim dsListaOI As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {FechaOperacion, CodigoSBS, CodigoISIN, CodigoMnemonico, Portafolio, Operacion, Moneda, accion, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            dsListaOI = daOrdenInversion.ListarOrdenesCabecera(CategoriaInstrumento, FechaOperacion, CodigoSBS, CodigoISIN, CodigoMnemonico, Portafolio, Operacion, Moneda, accion)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return dsListaOI
    End Function
    Public Function ListarPreOrdenesCabecera(ByVal CategoriaInstrumento As String, ByVal FechaOperacion As Decimal, ByVal CodigoSBS As String, ByVal CodigoISIN As String, ByVal CodigoMnemonico As String, ByVal Portafolio As String, ByVal Operacion As String, ByVal Moneda As String, ByVal accion As String, ByVal dataRequest As DataSet) As DataSet
        Dim dsListaOI As New DataSet
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {FechaOperacion, CodigoSBS, CodigoISIN, CodigoMnemonico, Portafolio, Operacion, Moneda, accion, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            dsListaOI = daOrdenInversion.ListarPreOrdenesCabecera(CategoriaInstrumento, FechaOperacion, CodigoSBS, CodigoISIN, CodigoMnemonico, Portafolio, Operacion, Moneda, accion)
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return dsListaOI
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
    Public Function ListarPreOrdenesParaAsignacion(ByVal FechaOperacion As Decimal, ByVal CategoriaInstrumento As String, ByVal CodigoMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim dsListaOI As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaOperacion, CategoriaInstrumento, CodigoMnemonico, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            dsListaOI = daOrdenInversion.ListarPreOrdenesParaAsignacion(FechaOperacion, CategoriaInstrumento, CodigoMnemonico)
            RegistrarAuditora(parameters)
            Return dsListaOI
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function RetornarFechaVencimiento(ByVal fechaOperacion As Decimal, ByVal CodigoMnemonico As String, _
        ByVal portafolio As String, ByVal codigoTercero As String) As String
        Dim strCodigoOI As String
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            strCodigoOI = daOrdenInversion.RetornarFechaVencimiento(fechaOperacion, CodigoMnemonico, portafolio, codigoTercero)
            Return strCodigoOI
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function RetornarClaseInstrumento(ByVal CodigoPreOrden As String, ByVal dataRequest As DataSet) As String
        Dim strClase As String
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            strClase = daOrdenInversion.RetornarClaseInstrumento(CodigoPreOrden)
            'Luego de terminar la ejecución de métodos(sin errores) 
            Return strClase
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Sub InicializarOrdenInversion(ByRef oRowOI As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.InicializarOrdenInversion(oRowOI)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Function ListarOIEjecutadas(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As Decimal) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIEjecutadas(dataRequest, codigoFondo, nroOrden, fechaOperacion).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ResumenPreOrdenAcciones(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenAcciones(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenAsignacion(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenAsignacion(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenDepositoPlazos(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenDepositoPlazos(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenOrdenesFondo(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenOrdenesFondo(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenOperacionesReporte(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenOperacionesReporte(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenLetrasHipotecarias(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenLetrasHipotecarias(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenForwardDivisas(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenForwardDivisas(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenCompraVenta(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenCompraVenta(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function
    Public Function ResumenPreOrdenBonos(ByVal CodigoPreOrden As String, ByVal Clase As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPreOrden, dataRequest}
        Dim DsResumen As DataTable
        Try
            DsResumen = New OrdenPreOrdenInversionDAM().ResumenPreOrdenBonos(CodigoPreOrden, Clase).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return DsResumen
    End Function

    Public Function ListarOIPorEjecutar(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As String) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIPorEjecutar(dataRequest, codigoFondo, nroOrden, fechaOperacion).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
    Public Function ListarOIConfirmadas(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fecha As Decimal, ByVal tipoInstrumento As String) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIConfirmadas(dataRequest, codigoFondo, nroOrden, fecha, tipoInstrumento).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
    Public Function GetComisionesOrdenInversionByPoliza(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal fechaOperacion As String, ByVal numeroPoliza As String) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().GetComisionesOrdenInversionByPoliza(dataRequest, codigoFondo, fechaOperacion, numeroPoliza).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function UpdateImpuestosComisionesOrdenPreOrden(ByVal codigoFondo As String, ByVal fechaOperacion As String, ByVal codigoComision As String, ByVal importeComision As Decimal, ByVal numeroPoliza As String, ByVal codigoOrden As String) As Boolean
        Dim ordenInversionDA As New OrdenPreOrdenInversionDAM
        Dim codigoOrdenPreOrden As String = String.Empty
        Dim currentImporteComision As Decimal = 0
        Dim tempCodigoComision As String = String.Empty
        Dim newValorComision As Decimal = 0
        Dim dtOrders As New DataTable
        Dim ok As Boolean = False
        If (numeroPoliza = "") Then
            Return ok
        End If
        Try
            dtOrders = ordenInversionDA.GetOrdenesByNumeroPoliza(codigoFondo, fechaOperacion, numeroPoliza, codigoOrden).Tables(0) 'Modificado por LC 25082008

            For Each dr As DataRow In dtOrders.Rows
                tempCodigoComision = Convert.ToString(dr("CodigoComision"))
                currentImporteComision = Convert.ToDecimal(dr("ValorComision"))
                newValorComision = Convert.ToDecimal(dr("ValorComision"))
                If String.Equals(tempCodigoComision, codigoComision) Then
                    codigoOrdenPreOrden = Convert.ToString(dr("CodigoOrden"))
                    If (importeComision > 0) Then
                        newValorComision = currentImporteComision - System.Math.Abs(importeComision)
                    ElseIf (importeComision <= 0) Then
                        newValorComision = currentImporteComision + System.Math.Abs(importeComision)
                    End If
                    Exit For
                End If
            Next
            If (codigoOrdenPreOrden.Equals(String.Empty) = False) Then
                ordenInversionDA.UpdateImpuestosComisionesOrdenPreOrden(codigoFondo, codigoOrdenPreOrden, codigoComision, newValorComision)
            End If
            ok = True
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ok
    End Function
    Public Function ListarOIEjecutadasExtornadas(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fecha As Decimal) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIEjecutadasExtornadas(dataRequest, codigoFondo, nroOrden, fecha).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarOIAsignadas(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIAsignadas(dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarOIAsignadas(ByVal StrCodigoMnemonico As String, ByVal StrCodiIsin As String, ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIAsignadas(StrCodigoMnemonico, StrCodiIsin, Fecha, dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarOIExcedidas(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIExcedidas(dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarOIAprobadas(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIAprobadas(dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'HDG OT 60022 20100813
    Public Function ListarOIAprobadasExcesoBroker(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIAprobadasExcesoBroker(dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'HDG OT 60022 20100714
    Public Function ListarOIExcedidasBroker(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIExcedidasBroker(dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'CMB OT 62254 20110415
    Public Function ListarExcesoPorBroker(ByVal codigoOrden As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarExcesoPorBroker(codigoOrden)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ConsultaCertificados(ByVal intIndica As Integer, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {intIndica, dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ConsultaCertificados(intIndica, dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function GetEstadoOrdenes(ByVal dataRequest As DataSet) As DataTable
        Dim parameters As Object() = {dataRequest}
        Dim ordenInversiones As New OrdenPreOrdenInversionDAM
        Dim dtEstados As New DataTable
        Try
            dtEstados = ordenInversiones.GetEstadoOrdenes()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dtEstados
    End Function
    '*****************
    Public Function validarPolizaExistencia(ByVal varCodigoOrden As String, ByVal vartbNPoliza As String, ByVal varddlOperacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {varCodigoOrden, vartbNPoliza, varddlOperacion, dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().validarPolizaExistencia(varCodigoOrden, vartbNPoliza, varddlOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function verValorizadoExistencia(ByVal varCodigoPortafolio As String, ByVal varfechaOperacion As Decimal, ByVal varNumeroOrden As String, ByRef Liquidado As Boolean, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {varCodigoPortafolio, varfechaOperacion, varNumeroOrden, dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().verValorizadoExistencia(varCodigoPortafolio, varfechaOperacion, varNumeroOrden, Liquidado, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'RGF 20080814
    Public Function ListarVencimientosFuturos(ByVal dataRequest As DataSet, ByVal fondo As String, ByVal fecha As Decimal, ByVal fechaOperacion As Decimal) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarVencimientosFuturos(dataRequest, fondo, fecha, fechaOperacion).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'LC 20080820 
    Public Function ExistenciaOperacionCaja(ByVal codigoOperacion As String, ByVal fechaOperacion As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New OrdenPreOrdenInversionDAM().ExistenciaOperacionCaja(dataRequest, codigoOperacion, fechaOperacion, portafolio).Tables(0)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    'RGF 20081110
    Public Function ListarForwardsCartas(ByVal fechaOperacion As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarForwardsCartas(fechaOperacion, portafolio)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'RGF 20081114
    Public Function ListarVencimientoForwardNoDelivery(ByVal portafolio As String, ByVal fechaVencimientoDesde As Decimal, ByVal fechaVencimientoHasta As Decimal, ByVal codigoMoneda As String, ByVal codigoMonedaDestino As String, ByVal codigoMercado As String, ByVal Calculo As Decimal, ByVal dataRequest As DataSet) As DataSet   'HDG OT 63063 R09 20110616
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarVencimientoForwardNoDelivery(portafolio, fechaVencimientoDesde, fechaVencimientoHasta, codigoMoneda, codigoMonedaDestino, codigoMercado, Calculo)  'HDG OT 63063 R09 20110616
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarVencimientoForward(ByVal portafolio As String, ByVal fechaProceso As Decimal, ByVal fechaVencimientoDesde As Decimal, ByVal fechaVencimientoHasta As Decimal, ByVal codigoMoneda As String, ByVal codigoMonedaDestino As String, ByVal Estado As Decimal, ByVal tRangoFecha As Decimal, ByVal codigoMercado As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarVencimientoForward(portafolio, fechaProceso, fechaVencimientoDesde, fechaVencimientoHasta, codigoMoneda, codigoMonedaDestino, Estado, tRangoFecha, codigoMercado)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'HDG OT 63375 20110627
    Public Function InventarioForward(ByVal portafolio As String, ByVal fechaProceso As Decimal, ByVal fechaVencimientoDesde As Decimal, ByVal fechaVencimientoHasta As Decimal, ByVal codigoMoneda As String, ByVal codigoMonedaDestino As String, ByVal Estado As Decimal, ByVal tRangoFecha As Decimal, ByVal codigoMercado As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().InventarioForward(portafolio, fechaProceso, fechaVencimientoDesde, fechaVencimientoHasta, codigoMoneda, codigoMonedaDestino, Estado, tRangoFecha, codigoMercado)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'LETV 20090225
    Public Function ModificarNumeroContratoForward(ByVal portafolio As String, ByVal codigoOrden As String, ByVal numeroContrato As String, _
                                                   ByVal Mtm As Decimal, _
                                                   ByVal MtmDestino As Decimal, _
                                                   ByVal PrecioVector As Decimal, _
                                                   ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ModificarNumeroContratoForward(portafolio, codigoOrden, numeroContrato, Mtm, MtmDestino, PrecioVector, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function


    'RGF 20090209
    Public Function ConfirmarVencimientoForwardNoDelivery(ByVal portafolio As String, ByVal codigoOrden As String, ByVal fixing As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ConfirmarVencimientoForwardNoDelivery(portafolio, codigoOrden, fixing, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'RGF 20090316
    Public Function ObtenerSaldosCustodio(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ObtenerSaldosCustodio(codigoNemonico)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'DB 20090506
    Public Function SeleccionarOI_FechaIDI(ByVal fondo As String, ByVal fecha As Decimal, ByVal codigoMnemonico As String, ByVal datarequest As DataSet) As DataSet
        Dim oDS As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {fondo, fecha, codigoMnemonico, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            oDS = daOrdenInversion.SeleccionarOI_FechaIDI(fondo, fecha, codigoMnemonico)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function

    'DB 20090506
    Public Function ActualizarFechaIDI(ByVal codigoOrden As String, ByVal portafolio As String, ByVal fechaIDI As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim resul As Boolean = False
        Dim parameters As Object() = {codigoOrden, portafolio, fechaIDI, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            resul = daOrdenInversion.ActualizarFechaIDI(codigoOrden, portafolio, fechaIDI, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resul
    End Function

    ''''HDG OT 59298 20100422
    '''Public Sub ValidarPrecio(ByVal strClaseInstrumento As String, ByVal strOperacion As String, ByVal strMnemonico As String, ByVal strIntermediario As String, ByVal strFondo As String, ByRef decPrecio1 As Decimal, ByRef decPrecio2 As Decimal _
    '''            , Optional ByVal codigoMoneda As String = "", Optional ByVal CodigoMonedaDestino As String = "")
    '''    Try
    '''        Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
    '''        daOrdenInversion.ValidarPrecio(strClaseInstrumento, strOperacion, strMnemonico, strIntermediario, strFondo, decPrecio1, decPrecio2, codigoMoneda, CodigoMonedaDestino)

    '''    Catch ex As Exception
    '''        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception

    '''        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '''        Dim rethrow As Boolean = True
    '''        If (rethrow) Then
    '''            Throw
    '''        End If
    '''    End Try
    '''End Sub

    'HDG OT 60072 20100615 
    'Public Sub FechaModificarEliminarOI(ByVal Portafolio As String, ByVal CodigoOrden As String, ByVal Fecha As Integer, ByVal tProc As String)
    Public Sub FechaModificarEliminarOI(ByVal Portafolio As String, ByVal CodigoOrden As String, ByVal Fecha As Integer, ByVal tProc As String, ByVal Comentario As String, ByVal dataRequest As DataSet) 'HDG OT 60882 20100915
        Dim parameters As Object() = {Portafolio, CodigoOrden, Fecha, tProc}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            'HDG OT 60882 20100915
            'daOrdenInversion.FechaModificarEliminarOI(Portafolio, CodigoOrden, Fecha, tProc)
            daOrdenInversion.FechaModificarEliminarOI(Portafolio, CodigoOrden, Fecha, tProc, Comentario, dataRequest)

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
    End Sub

    'HDG OT 61566 Nro5-R12 20101122
    Public Sub OrdenSimultaneoSwapOI(ByVal strCodigoOrden As String, ByVal strCodigoOrdenSim As String, ByVal strPrior As String, ByVal strFondo As String)
        Dim parameters As Object() = {strCodigoOrden, strCodigoOrdenSim, strPrior, strFondo}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.OrdenSimultaneoSwapOI(strCodigoOrden, strCodigoOrdenSim, strPrior, strFondo)
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
    End Sub

    'HDG OT 61566 Nro5-R12 20101122
    Public Function ListarOrdenesInversionSwap(ByVal codigoOrden As String, ByVal datarequest As DataSet) As DataTable
        Dim dt As DataTable
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {codigoOrden, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            dt = daOrdenInversion.ListarOrdenesInversionSwap(codigoOrden)
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return dt
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

    'HDG OT 61573 20101125
    'Public Function SeleccionarTipoMonedaxMotivoForw(ByVal codigoMotivo As String) As DataTable
    Public Function SeleccionarTipoMonedaxMotivoForw(ByVal codigoMotivo As String, ByVal codigoMoneda As String) As DataTable   'HDG OT 62325 20110323
        Dim dt As DataTable
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {codigoMotivo, codigoMoneda}   'HDG OT 62325 20110323
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            dt = daOrdenInversion.SeleccionarTipoMonedaxMotivoForw(codigoMotivo, codigoMoneda)   'HDG OT 62325 20110323
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return dt
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

    'HDG OT 62087 Nro14-R23 20110315
    Public Function ValidarLineaNegociacionDPZ(ByVal Mnemonico As String, ByVal Fecha As Decimal, ByVal Portafolio As String, ByVal CodigoTercero As String, ByVal MontoOperacion As Decimal, ByVal CodigoOrden As String) As Boolean
        Dim parameters As Object() = {Mnemonico}
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            RegistrarAuditora(parameters)
            Return oOrdenPreOrdenInversionDAM.ValidarLineaNegociacionDPZ(Mnemonico, Fecha, Portafolio, CodigoTercero, MontoOperacion, CodigoOrden)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    ''CMB OT 62087 20110316 Nro 8
    'Public Function ResumenCajaPorMonedas(ByVal fechaLiquidacion As Decimal, ByVal datosRequest As DataSet) As DataSet
    '    Dim ds As DataSet
    '    'Se agrupan los parámetros enviados en el mismo orden
    '    Dim parameters As Object() = {fechaLiquidacion, datosRequest}
    '    Try
    '        Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
    '        ds = daOrdenInversion.ResumenCajaPorMonedas(fechaLiquidacion)
    '        'Luego de terminar la ejecución de métodos(sin errores) 
    '        RegistrarAuditora(parameters)
    '        Return ds
    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function

    'CMB OT 62254 20110324
    Public Function ObtenerUsuario(ByVal strCodigoOrden As String, ByVal datosRequest As DataSet) As String
        'Se agrupan los parámetros enviados en el mismo orden
        Dim strUsuario As String = ""
        Dim parameters As Object() = {strCodigoOrden, datosRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            strUsuario = daOrdenInversion.ObtenerUsuario(strCodigoOrden)
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return strUsuario
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

    'HDG OT 63063 R04 20110523
    Public Function SeleccionarOrdenExcepPorFiltro(ByVal strTipoRenta As String, ByVal strPortafolio As String, ByVal decFechaInicio As String, ByVal decFechaFin As String, ByVal strTipoOperacion As String, ByVal strCodigoTipoInstrumentoSBS As String, ByVal strExclusion As String, ByVal datarequest As DataSet) As DataSet
        Dim oDS As DataSet
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {strTipoRenta, strPortafolio, decFechaInicio, decFechaFin, strCodigoTipoInstrumentoSBS, strTipoOperacion, strExclusion, datarequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            oDS = daOrdenInversion.SeleccionarOrdenExcepPorFiltro(strTipoRenta, strPortafolio, decFechaInicio, decFechaFin, strTipoOperacion, strCodigoTipoInstrumentoSBS, strExclusion, datarequest)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return oDS
    End Function

    'HDG OT 63063 R04 20110526
    Public Sub ExcepcionLimiteNegociacion(ByVal strCodigoOrden As String, ByVal strCodigoExcepcion As String, ByVal decCantidad As Decimal, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {strCodigoOrden, strCodigoExcepcion, decCantidad, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.ExcepcionLimiteNegociacion(strCodigoOrden, strCodigoExcepcion, decCantidad, dataRequest)

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
    End Sub

    'CMB OT 63063 20110622 REQ 06
    Public Sub InicializarOperacionesEPU(ByRef oRow As TmpOperacionesEPUBE.TmpOperacionesEPURow, ByVal dataRequest As DataSet)
        Dim parameters As Object() = {oRow, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.InicializarOperacionesEPU(oRow)
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
    End Sub

    'CMB OT 63063 20110622 REQ 06
    Public Sub InicializarOperacionesEPUDet(ByRef oRow As TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow, ByVal dataRequest As DataSet)
        Dim parameters As Object() = {oRow, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.InicializarOperacionesEPUDet(oRow)
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
    End Sub

    'CMB OT 63063 20110624 REQ 06
    Public Sub InicializarResumenOperacionesEPU(ByRef oRow As TmpResumenOperacionesEPUBE.TmpResumenOperacionesEPURow, ByVal dataRequest As DataSet)
        Dim parameters As Object() = {oRow, dataRequest}
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            daOrdenInversion.InicializarResumenOperacionesEPU(oRow)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    'CMB OT 63063 20110622 REQ 06
    Public Function EliminarOperacionesEPUDet(ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.EliminarOperacionesEPUDet()
            RegistrarAuditora(parameters)
            Return bolResult
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

    'CMB OT 63063 20110622 REQ 06
    Public Function InsertarOperacionesEPU(ByVal oTmpOperacionesEPUBE As TmpOperacionesEPUBE, ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {oTmpOperacionesEPUBE, dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.InsertarOperacionesEPU(oTmpOperacionesEPUBE)
            RegistrarAuditora(parameters)
            Return bolResult
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

    'CMB OT 63063 20110622 REQ 06
    Public Function InsertarOperacionesEPUDet(ByVal oTmpOperacionesEPUDetBE As TmpOperacionesEPUDetBE, ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {oTmpOperacionesEPUDetBE, dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.InsertarOperacionesEPUDet(oTmpOperacionesEPUDetBE)
            Return bolResult
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

    'CMB OT 63063 20110622 REQ 06
    Public Function InsertarResumenOperacionesEPU(ByVal oTmpResumenOperacionesEPUBE As TmpResumenOperacionesEPUBE, ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {oTmpResumenOperacionesEPUBE, dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.InsertarResumenOperacionesEPU(oTmpResumenOperacionesEPUBE)
            Return bolResult
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

    'CMB OT 63063 20110624 REQ 06
    Public Function EliminarResumenOperacionesEPU(ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.EliminarResumenOperacionesEPU()
            Return bolResult
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

    'CMB OT 63063 20110602 REQ 06
    Public Function ActualizarOperacionesEPUDet(ByVal Id As Int32, ByVal portafolio As String, ByVal codigoTercero As String, ByVal tipoCambio As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {Id, portafolio, codigoTercero, tipoCambio, dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.ActualizarOperacionesEPUDet(Id, portafolio, codigoTercero, tipoCambio)
            RegistrarAuditora(parameters)
            Return bolResult
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

    'CMB OT 63063 20110602 REQ 06
    Public Function GenerarOperacionesEPU(ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            bolResult = daOrdenInversion.GenerarOperacionesEPU(dataRequest)
            RegistrarAuditora(parameters)
            Return bolResult
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

    'CMB OT 63063 20110603 REQ 06
    Public Function SeleccionarPrevOperacionesEPU(ByVal dataRequest As DataSet) As DataSet
        Dim parameters As Object() = {dataRequest}
        Dim dsAux As DataSet
        Try
            Dim daOrdenInversion As New OrdenPreOrdenInversionDAM
            dsAux = daOrdenInversion.SeleccionarPrevOperacionesEPU()
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

    ''' <summary>
    ''' Lista todos los expedientes de OperacionContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As OperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New OperacionContableDAM().Listar(dataRequest)
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

    ''' <summary>
    ''' Seleccion una serie de listado para Generar el Reporte de Operaciones EPU .
    ''' <summary>
    ''' <param></param>
    ''' <returns>OperacionContableBE</returns>

    'GTC OT 63063 20110627 REQ 06
    Public Function GenerarReporteOperacionesPU() As DataSet
        Try

            Return New OrdenPreOrdenInversionDAM().GenerarReporteOperacionesPU()


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'CMB OT 64292 20111123
    Public Function GenerarReporteDeFallasOI(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal opcion As Decimal, ByVal portafolio As String) As DataSet
        Try
            Return New OrdenPreOrdenInversionDAM().GenerarReporteDeFallasOI(fechaInicio, fechaFin, opcion, portafolio)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'HDG OT 64291 20111202
    Public Function ValidacionPuntual_LimitesTrading(ByVal codigoNemonico As String, ByVal fechaOperacion As Decimal, ByVal portafolio As String, ByVal montoNegociado As Decimal, ByVal codigoMoneda As String, ByVal usuarioTrader As String, Optional ByVal CategoriaInstrumento As String = "") As DataSet
        Try
            Return New OrdenPreOrdenInversionDAM().ValidacionPuntual_LimitesTrading(codigoNemonico, fechaOperacion, portafolio, montoNegociado, codigoMoneda, usuarioTrader, CategoriaInstrumento)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'CMB OT 65473 20120921
    Public Function ObtenerFirmasLlamadoOI(ByVal codigoOrden As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet, Optional ByVal flagMostrarFirma As Boolean = True) As DataSet
        Try
            Return New OrdenPreOrdenInversionDAM().ObtenerFirmasLlamadoOI(codigoOrden, fecha, flagMostrarFirma)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'CMB OT 66471 20121227
    Public Function ReporteVerificacionFirmas(ByVal fecha As Decimal, ByVal codCargoFirmante As Decimal, ByVal codigoUsuario As String, _
                                              ByVal codigoOrden As String, ByVal estFirmaD As String, ByVal codPortafolioSBS As String, _
                                              ByVal codigoOperacion As String, ByVal codigoMercado As String, ByVal codCategReporte As String) As DataSet
        Try
            Return New OrdenPreOrdenInversionDAM().ReporteVerificacionFirmas(fecha, codCargoFirmante, codigoUsuario, codigoOrden, _
                                                                             estFirmaD, codPortafolioSBS, codigoOperacion, codigoMercado, _
                                                                             codCategReporte)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#Region "66056 - Modificacion: JZAVALA"

    Public Function ListarDatosOperacionPorCodigoPrevOrdenInversion(ByVal strCodigoPrevOrden As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoPrevOrden, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenPreOrdenInversionDAM().ListarDatosOperacionPorCodigoPrevOrdenInversion(strCodigoPrevOrden)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function

#End Region

    'HDG OT 67944 20130705
    Public Function EliminarTablaTmpNemonicosFondoRenta(ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            bolResult = oOrdenPreOrdenInversionDAM.EliminarTablaTmpNemonicosFondoRenta()
            RegistrarAuditora(parameters)
            Return bolResult
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

    'HDG OT 67944 20130705
    Public Function ActualizarInstrumentosPorExcel(ByVal data As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {data, dataRequest}

        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Codigo = oOrdenPreOrdenInversionDAM.ActualizarInstrumentosPorExcel(data, dataRequest, strmensaje)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    'HDG OT 67944 20130705
    Public Function CarteraTituloValoracionFinal(ByVal FechaOperacion As Decimal) As DataSet
        Dim bolResult As Boolean = False
        Dim oReporte As New DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.CarteraTituloValoracionFinal(FechaOperacion)
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

    'HDG OT 67944 20130705
    Public Function EnvioInfoCarteraAFP(ByVal FechaOperacion As Decimal, ByVal Fondo As String, ByVal AFPDestino As String) As DataSet
        Dim bolResult As Boolean = False
        Dim oReporte As New DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.EnvioInfoCarteraAFP(FechaOperacion, Fondo, AFPDestino)
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

    'HDG OT 67944 20130705
    Public Function EnvioConstitucionForwards(ByVal FechaOperacion As Decimal, ByVal Fondo As String, ByVal AFPDestino As String) As DataSet
        Dim bolResult As Boolean = False
        Dim oReporte As New DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.EnvioConstitucionForwards(FechaOperacion, Fondo, AFPDestino)
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

    'HDG OT 67944 20130705
    Public Function ParticionCartera(ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            bolResult = oOrdenPreOrdenInversionDAM.ParticionCartera(FechaOperacion, dataRequest)
            RegistrarAuditora(parameters)
            Return bolResult
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
    Public Function Reporte_RegistroInversiones(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal CodigoPortafolioSBS As String) As DataSet
        Dim bolResult As Boolean = False
        Dim oReporte As New DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.Reporte_RegistroInversiones(FechaInicio, FechaFin, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
        Return oReporte
    End Function
    Public Function Reporte_ReportesDiariosOperaciones(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal TipoConsulta As String) As DataTable
        Dim bolResult As Boolean = False
        Dim oReporte As New DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.Reporte_ReportesDiariosOperaciones(CodigoPortafolioSBS, FechaOperacion, TipoConsulta)
        Catch ex As Exception
            Throw ex
        End Try
        Return oReporte
    End Function
    'Operaciones de Reporte
    Public Function Seleccionar_ComisionesOR(ByVal CodigoOrden As String) As DataTable
        Try
            Dim oReporte As New DataSet
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.Seleccionar_ComisionesOR(CodigoOrden)
            Return oReporte.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Insertar_ComisionesOR(ByVal CodigoOrden As String, ByVal ImpuestoCompra As Decimal, ByVal ImpuestoVenta As Decimal, ByVal ComisionAIVenta As Decimal,
    ByVal ComisionAICompra As Decimal, ByVal RestoComisionCompra As Decimal, ByVal RestoComisionVenta As Decimal)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.Insertar_ComisionesOR(CodigoOrden, ImpuestoCompra, ImpuestoVenta, ComisionAIVenta, ComisionAICompra,
            RestoComisionCompra, RestoComisionVenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Modificar_ComisionesOR(ByVal CodigoOrden As String, ByVal ImpuestoCompra As Decimal, ByVal ImpuestoVenta As Decimal, ByVal ComisionAIVenta As Decimal,
    ByVal ComisionAICompra As Decimal, ByVal RestoComisionCompra As Decimal, ByVal RestoComisionVenta As Decimal)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.Modificar_ComisionesOR(CodigoOrden, ImpuestoCompra, ImpuestoVenta, ComisionAIVenta, ComisionAICompra,
            RestoComisionCompra, RestoComisionVenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function confirmarInstrumentosSinCuponera(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.confirmarInstrumentosSinCuponera(codigoOrden, codigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' Genera la data del reporte inventario Forward
    ''' </summary>
    ''' <param name="fechaDesde"></param>
    ''' <param name="fechaHasta"></param>
    ''' <remarks>
    ''' 2015-12-03        Herbert Mendoza              creacion
    ''' </remarks>
    Public Function Reporte_InventarioForward_Fecha_Rango(ByVal portafolio As String, _
                                                            ByVal fechaDesde As Decimal, _
                                                            ByVal fechaHasta As Decimal, _
                                                            ByVal dataRequest As DataSet) As DataSet
        Dim oReporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim damReporteCarteraForward As New OrdenPreOrdenInversionDAM()
        Try
            oReporte = damReporteCarteraForward.Reporte_InventarioForward_Fecha_Rango(portafolio, fechaDesde, fechaHasta)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    ''' <summary>
    ''' Genera la data del tipo de cambio
    ''' </summary>
    ''' <param name="fechaDesde"></param>
    ''' <param name="fechaHasta"></param>
    ''' <remarks>
    ''' 2015-12-03        Herbert Mendoza              creacion
    ''' </remarks>
    Public Function Reporte_VectorTipoCambio_Fecha_Rango(ByVal fechaDesde As Decimal, _
                                                         ByVal fechaHasta As Decimal, _
                                                         ByVal dataRequest As DataSet) As DataSet
        Dim oReporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim damReporteCarteraForward As New OrdenPreOrdenInversionDAM()
        Try
            oReporte = damReporteCarteraForward.Reporte_VectorTipoCambio_Fecha_Rango(fechaDesde, fechaHasta)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    Public Function Reporte_Gestion_MarkToMarkedFW_Fecha_Rango(ByVal Fecha As Decimal, _
                                                               ByVal Escenario As String, _
                                                               ByVal Portafolio As String, _
                                                               ByVal dataRequest As DataSet) As DataSet
        Dim oReporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim damReporteCarteraForward As New OrdenPreOrdenInversionDAM()
        Try
            oReporte = damReporteCarteraForward.Reporte_Gestion_MarkToMarkedFW_Fecha_Rango(Fecha, Escenario, Portafolio)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    Public Function ReporteVencimientosCuponesOrdenes(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal TipoOperacion As String) As DataTable
        Dim bolResult As Boolean = False
        Dim oReporte As New DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.ReporteVencimientosCuponesOrdenes(CodigoPortafolioSBS, FechaOperacion, TipoOperacion)
        Catch ex As Exception
            Throw ex
        End Try
        Return oReporte
    End Function
    Public Function VencimientosdelDiaDPZ(ByVal CodigoPortafolioSBS As String, ByVal CodigoTercero As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.VencimientosdelDiaDPZ(CodigoPortafolioSBS, CodigoTercero, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConstitucionesdelDiaDPZ(ByVal CodigoPortafolioSBS As String, ByVal CodigoTercero As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.ConstitucionesdelDiaDPZ(CodigoPortafolioSBS, CodigoTercero, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TransferenciaBCR(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.TransferenciaBCR(CodigoPortafolioSBS, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ActualizaVencimientosDPZ(ByVal CodigoPortafolioSBS As String, ByVal CodigoOrdenConstitucion As String, ByVal CodigoOrdenVencimiento As String)
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            ordenInversionDAM.ActualizaVencimientosDPZ(CodigoPortafolioSBS, CodigoOrdenConstitucion, CodigoOrdenVencimiento)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ReporteRegistroInversiones(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, ByVal FechaFinal As Decimal) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.ReporteRegistroInversiones(CodigoPortafolioSBS, FechaInicio, FechaFinal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ResumenPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataSet
        Dim bolResult As Boolean = False
        Dim oReporte As New DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.ResumenPortafolio(CodigoPortafolioSBS, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
        Return oReporte
    End Function
    Public Function SeleccionaRatingTerceroHistoria(ByVal CodigoTercero As String, ByVal fechaInicio As Integer, ByVal fechaFin As Integer) As DataTable
        Dim bolResult As Boolean = False
        Dim oReporte As New DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.SeleccionaRatingTerceroHistoria(CodigoTercero, fechaInicio, fechaFin)
        Catch ex As Exception
            Throw ex
        End Try
        Return oReporte
    End Function
    Public Function SeleccionaRatingValorHistoria(ByVal CodigoIsin As String, ByVal fechaInicio As Integer, ByVal fechaFin As Integer) As DataTable
        Dim bolResult As Boolean = False
        Dim oReporte As New DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oReporte = oOrdenPreOrdenInversionDAM.SeleccionaRatingValorHistoria(CodigoIsin, fechaInicio, fechaFin)
        Catch ex As Exception
            Throw ex
        End Try
        Return oReporte
    End Function
    Public Sub BorrarCuponera_Bono_Swap(ByVal CodigoOrden As String)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.BorrarCuponera_Bono_Swap(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function InsertaCuponera_Bono_Swap(ByVal Correlativo As Integer, ByVal CodigoOrden As String, ByVal FechaIniOriginal As Decimal, ByVal FechaFinOriginal As Decimal, ByVal DifDiasOriginal As Decimal, _
                                              ByVal AmortizacOriginal As Decimal, ByVal TasaCuponOriginal As Decimal, ByVal BaseCuponOriginal As String, ByVal DiasPagoOriginal As String, ByVal fechaRealInicialOriginal As Decimal, _
                                              ByVal fechaRealFinalOriginal As Decimal, ByVal AmortizacConsolidadoOriginal As Decimal, ByVal MontoInteresOriginal As Decimal, ByVal MontoAmortizacionOriginal As Decimal, _
                                              ByVal NominalRestanteOriginal As Decimal, ByVal TasaSpreadOriginal As Decimal, ByVal FechaIni As Decimal, ByVal FechaFin As Decimal, ByVal DifDias As Decimal, ByVal Amortizac As Decimal, _
                                              ByVal TasaCupon As Decimal, ByVal BaseCupon As String, ByVal DiasPago As String, ByVal fechaRealInicial As Decimal, ByVal fechaRealFinal As Decimal, ByVal AmortizacConsolidado As Decimal, _
                                              ByVal MontoInteres As Decimal, ByVal MontoAmortizacion As Decimal, ByVal NominalRestante As Decimal, ByVal TasaSpread As Decimal, ByVal FechaLiborOriginal As Decimal, ByVal FechaLibor As Decimal)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.InsertaCuponera_Bono_Swap(Correlativo, CodigoOrden, FechaIniOriginal, FechaFinOriginal, DifDiasOriginal, AmortizacOriginal, TasaCuponOriginal, BaseCuponOriginal, DiasPagoOriginal, _
                                                                 fechaRealInicialOriginal, fechaRealFinalOriginal, AmortizacConsolidadoOriginal, MontoInteresOriginal, MontoAmortizacionOriginal, NominalRestanteOriginal, TasaSpreadOriginal, _
                                                                 FechaIni, FechaFin, DifDias, Amortizac, TasaCupon, BaseCupon, DiasPago, fechaRealInicial, fechaRealFinal, AmortizacConsolidado, MontoInteres, MontoAmortizacion, NominalRestante, TasaSpread, FechaLiborOriginal, FechaLibor)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertaBono_Swap(ByVal CodigoOrden As String, ByVal CodigoIsin As String, ByVal CodigoNemonico As String, ByVal Nominal As Decimal, ByVal Unidades As Decimal, ByVal FechaVencimiento As Decimal, ByVal importeVenta As Decimal)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.InsertaBono_Swap(CodigoOrden, CodigoIsin, CodigoNemonico, Nominal, Unidades, FechaVencimiento, importeVenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub BorrarBono_Swap(ByVal CodigoOrden As String)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.BorrarBono_Swap(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ConsultaOrdenSwapBono(ByVal CodigoPortafolio As String, ByVal FechaOperacion As Decimal, ByVal CodigoOrden As String) As DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ConsultaOrdenSwapBono(CodigoPortafolio, FechaOperacion, CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConsultaBono_Swap(ByVal CodigoOrden As String) As DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ConsultaBono_Swap(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConsultaCuponera_Bono_Swap(ByVal CodigoOrden As String, ByVal OrdenGenera As String) As DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ConsultaCuponera_Bono_Swap(CodigoOrden, OrdenGenera)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub AnulaOrdenInversion(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.AnulaOrdenInversion(codigoOrden, codigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub ConfirmaOrdenInversion(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal CodigoIsin As String, ByVal dataRequest As DataSet)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.ConfirmaOrdenInversion(codigoOrden, codigoPortafolio, CodigoIsin, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ValidaISIN(ByVal CodigoIsin As String, ByVal CodigoOrden As String) As Integer
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ValidaISIN(CodigoIsin, CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConsultaOrden(ByVal CodigoPortafolio As String, ByVal FechaOperacion As Decimal, ByVal CodigoOrden As String, ByVal CodigoOperacion As String) As DataTable
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ConsultaOrden(CodigoPortafolio, FechaOperacion, CodigoOrden, CodigoOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Insertar_DPZRenovacionCabecera(ByVal CodigoOrden As String, ByVal CodigoOperacion As String, ByVal CodigoModelo As String, ByVal FechaRelacion As Decimal) As Integer
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.Insertar_DPZRenovacionCabecera(CodigoOrden, CodigoOperacion, CodigoModelo, FechaRelacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Insertar_DPZRenovacionDetalle(ByVal CodigoCabecera As Integer, ByVal CodigoOrden As String, ByVal CodigoOperacion As String, ByVal FechaRelacion As Decimal)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.Insertar_DPZRenovacionDetalle(CodigoCabecera, CodigoOrden, CodigoOperacion, FechaRelacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Borrar_DPZRenovacion(ByVal CodigoOrden As String)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.Borrar_DPZRenovacion(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Codigo_DPZRenovacionCabecera(ByVal CodigoOrden As String) As Integer
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.Codigo_DPZRenovacionCabecera(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Libera_Renovacion(ByVal CodigoOrden As String)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.Libera_Renovacion(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT 10238 - 07/04/2017 - Carlos Espejo
    'Descripcion: Funcion para validar Codigo Valor
    Public Function ValidarCodigoValor(ByVal CodigoNemonico As String, ByVal CodigoTercero As String, ByVal CodigoTipoCupon As String) As Integer
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ValidarCodigoValor(CodigoNemonico, CodigoTercero, CodigoTipoCupon)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10238 - 07/04/2017 - Carlos Espejo
    'Descripcion: Funcion para validar Codigo Valor
    Public Sub RecalculaMontoInversion(ByVal CodigoIsin As String, ByVal CodigoPortafolioSBS As String, ByVal FechaOperacionActual As Decimal)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.RecalculaMontoInversion(CodigoIsin, CodigoPortafolioSBS, FechaOperacionActual)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
    Public Function ListarOIEjecutadasConfirmacion(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As Decimal, ByVal tipoInstumento As String) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIEjecutadasConfirmacion(dataRequest, codigoFondo, nroOrden, fechaOperacion, tipoInstumento).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
#Region "SWAP"
    Public Sub InsertarOI_DetalleSwap(ByVal objOI As OrdenInversion_DetalleSWAPBE)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.InsertarOI_DetalleSwap(objOI)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub ModificarOI_DetalleSwap(ByVal objOI As OrdenInversion_DetalleSWAPBE)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.ModificarOI_DetalleSwap(objOI)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub OrdenInversion_BorrarOI_Error_SWAP(ByVal CodigoPortafolio As String, ByVal CodigoOrden As String)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.OrdenInversion_BorrarOI_Error_SWAP(CodigoPortafolio, CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub InicializarOrdenInversion_DetalleSWAP(ByRef oRowSWAP As OrdenInversion_DetalleSWAPBE)
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.InicializarOrdenInversion_DetalleSWAP(oRowSWAP)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
#End Region


    Public Function ObtenerUnidadesNegociadasDiaT(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal CodigoMnemonico As String) As DataSet
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Return oOrdenPreOrdenInversionDAM.ObtenerUnidadesNegociadasDiaT(CodigoPortafolioSBS, FechaOperacion, CodigoMnemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    'INICIO | CDA | Ernesto Galarza | Actualizacion del porcentage precio sucio y limpio en orden de inversion con el dato que viene del archivo excel de vector factoring | 16/01/19
    Public Sub ActualizaPrecioLimpioSucio(ByVal pCodigoNemonico As String, ByVal pPrecio As Decimal)
        Dim bolResult As Boolean = False
        Try
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            oOrdenPreOrdenInversionDAM.ActualizaPrecioLimpioSucio(pCodigoNemonico, pPrecio)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    'FIN | CDA | Ernesto Galarza |  Actualizacion del porcentage precio sucio y limpio en orden de inversion con el dato que viene del archivo excel de vector factoring | 16/01/19 

    Public Function ConstitucionesdelDiaDPZRenovacion(ByVal CodigoPortafolioSBS As String, ByVal CodigoMnemonico As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.ConstitucionesdelDiaDPZRenovacion(CodigoPortafolioSBS, CodigoMnemonico, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function VencimientoRenovacion(ByVal CodigoPortafolioSBS As String, ByVal CodigoTercero As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim ordenInversionDAM As New OrdenPreOrdenInversionDAM
            Return ordenInversionDAM.VencimientoRenovacion(CodigoPortafolioSBS, CodigoTercero, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarOIConfirmadasAgrupados(ByVal codigoFondo As String, ByVal fechaOperacion As Integer, ByVal codigoMercado As String) As DataTable
        Try
            Return New OrdenPreOrdenInversionDAM().ListarOIConfirmadasAgrupados(codigoFondo, fechaOperacion, codigoMercado).Tables(0)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function AgruparDesagruparAccionesOI(ByVal CodigoOrden As String, ByVal Opcion As String) As String
        Try
            Return New OrdenPreOrdenInversionDAM().AgruparDesagruparAccionesOI(CodigoOrden, Opcion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ValidaOI_Agrupada(ByVal CodigoOrden As String) As Integer
        Try
            Return New OrdenPreOrdenInversionDAM().ValidaOI_Agrupada(CodigoOrden)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class