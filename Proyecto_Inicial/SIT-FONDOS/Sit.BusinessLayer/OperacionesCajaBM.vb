Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Imports System.Transactions
'OT10749 - Refactorizar código de la página
Public Class OperacionesCajaBM
    'Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function Insertar(ByVal dsOperacionCaja As OperacionCajaBE, ByVal dataRequest As DataSet)
        Try
            Dim daOperacionCaja As New OperacionesCajaDAM
            daOperacionCaja.Insertar(dsOperacionCaja, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Insertar_FechaOperacion(ByVal dsOperacionCaja As OperacionCajaBE, ByVal dataRequest As DataSet)
        Try
            Dim daOperacionCaja As New OperacionesCajaDAM
            daOperacionCaja.Insertar_FechaOperacion(dsOperacionCaja, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal dsOperacionCaja As OperacionCajaBE, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.SeleccionarPorFiltro(dsOperacionCaja, fechaInicio, fechaFin)
        Catch ex As Exception
            Throw ex
        End Try
        Return oOperacionCaja
    End Function
    Public Function SeleccionarPorFiltro3(ByVal dsOperacionCaja As OperacionCajaBE, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.SeleccionarPorFiltro3(dsOperacionCaja, fechaInicio, fechaFin)
        Catch ex As Exception
            Throw ex
        End Try
        Return oOperacionCaja
    End Function
    Public Function SeleccionarAutorizacionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.SeleccionarAutorizacionCartas(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, abono, estado, codigoOperacionCaja)
            Return oOperacionCaja
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCartasFirmadas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.SeleccionarCartasFirmadas(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, abono, estado, codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
        Return oOperacionCaja
    End Function
    Public Function ReporteInventarioCartas(ByVal fechaInicio As Decimal, ByVal fechaFinal As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.ReporteInventarioCartas(fechaInicio, fechaFinal)
        Catch ex As Exception
            Throw ex
        End Try
        Return oOperacionCaja
    End Function
    Public Function SeleccionarExtornos(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal motivo As String, ByVal estado As String, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.SeleccionarExtornos(fechaInicio, fechaFin, motivo, estado)
            Return oOperacionCaja
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ReporteExtornos(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal motivo As String, ByVal estado As String, ByVal dataRequest As DataSet) As DataSet
        Dim oOperacionCaja As DataSet
        Dim operaciones As New OperacionesCajaDAM
        Try
            oOperacionCaja = operaciones.ReporteExtornos(fechaInicio, fechaFin, motivo, estado)
            Return oOperacionCaja
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, ByVal fecha As Decimal, ByVal generar As Boolean, ByVal abono As Boolean, ByVal impreso As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oOperacionCaja As DataSet = New OperacionesCajaDAM().SeleccionarPorFiltro2(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, generar, abono, impreso)
            Return oOperacionCaja
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ModificarEstadoCarta(ByVal NumeroCarta As String, ByVal Estado As String, ByVal TipoEmision As String, ByVal abono As Boolean, ByVal dataRequest As DataSet)
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            oOperacionesCaja.ModificarEstadoCarta(NumeroCarta, Estado, TipoEmision, abono, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub AprobarCarta(ByVal NumeroCarta As String, ByVal Usuario As String, ByVal abono As Boolean, ByVal dataRequest As DataSet)
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            oOperacionesCaja.AprobarCarta(NumeroCarta, Usuario, dataRequest, abono)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function AprobarOperacionCaja(ByVal codigoOperacionCaja As String, ByVal dataRequest As DataSet, Optional ByVal indAprobar As String = "") As Boolean    'HDG 20120330
        Dim bolResult As Boolean = False
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            bolResult = oOperacionesCaja.AprobarOperacionCaja(codigoOperacionCaja, dataRequest, indAprobar) 'HDG 20120330
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FirmarCarta(ByVal codigoOperacionCaja As String, ByVal claveFirma As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            bolResult = oOperacionesCaja.FirmarCarta(codigoOperacionCaja, claveFirma, dataRequest)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ActualizarEstadoImpresion(ByVal codigoImpresion As Decimal, ByVal estado As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            bolResult = oOperacionesCaja.ActualizarEstadoImpresion(codigoImpresion, estado, dataRequest)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CalcularRangosInicialFinalCartas(ByVal cantidad As Decimal, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.CalcularRangosInicialFinalCartas(cantidad)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertarRangoImpresionCartas(ByVal rangoInicial As Decimal, ByVal rangoFinal As Decimal, ByVal cantidad As Decimal, ByRef codigoRango As Decimal, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.InsertarRangoImpresionCartas(rangoInicial, rangoFinal, cantidad, codigoRango, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImprimirCarta(ByVal codigoImpresion As Decimal, ByVal codigoCarta As Decimal, ByVal codigoRango As Decimal, ByVal dataRequest As DataSet, Optional ByVal EstadoCarta As String = "") As Boolean 'HDG 20120120
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.ImprimirCarta(codigoImpresion, codigoCarta, codigoRango, dataRequest, EstadoCarta)  'HDG 20120120
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GenerarClaveFirmantesCarta(ByVal codigoOperacionCaja As String, ByVal codigoInterno As String, ByVal claveFirma As String, ByVal rutaArchivo As String, ByVal indReporte As String, ByVal dataRequest As DataSet) As Boolean  'HDG OT 64016 20111021
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.GenerarClaveFirmantesCarta(codigoOperacionCaja, codigoInterno, claveFirma, rutaArchivo, indReporte, dataRequest) 'HDG OT 64016 20111021
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertarClaveFirmantes(ByVal codigoInterno As String, ByVal claveFirma As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.InsertarClaveFirmantes(codigoInterno, claveFirma, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerClaveFirmantes(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.ObtenerClaveFirmantes(codigoInterno, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExtornarOperacionesCaja(ByVal codigoExtorno As Decimal, ByVal accion As Boolean, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.ExtornarOperacionesCaja(codigoExtorno, accion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertarOperacionesCajaExt(ByVal codigoOperacionCaja As String, ByVal motivo As String, ByVal observacion As String, ByVal dataRequest As DataSet, ByVal liqAntFondo As Integer, ByVal codPortafolio As String) As Boolean
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.InsertarOperacionesCajaExt(codigoOperacionCaja, motivo, observacion, dataRequest, liqAntFondo, codPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InicializarAprobacionOperacionesCaja(ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oOperacionCaja As New OperacionesCajaDAM
            oOperacionCaja.InicializarAprobacionOperacionesCaja()
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function
    Public Function ReporteAprobacionOperacionesCaja(ByVal proceso As String, ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal dataRequest As DataSet) As DataSet  'HDG 20111128
        Try
            Dim oOperacionCaja As New OperacionesCajaDAM
            Return oOperacionCaja.ReporteAprobacionOperacionesCaja(proceso, codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, abono, estado, codigoOperacionCaja)  'HDG 20111128
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub InsertarTransferenciaInterna(ByVal dsOperacionCaja As OperacionCajaBE, TipoTransferencia As String, TranFictizia As String, ByVal dataRequest As DataSet)
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            oOperacionesCaja.InsertarTransferenciaInterna(dsOperacionCaja, TipoTransferencia, TranFictizia, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ValidarTransferenciaClaseCuenta(ByVal portafolioOrigen As String, ByVal potafolioDestino As String, ByVal numeroCuentaOrigen As String, ByVal numeroCuentaDestino As String) As Integer
        Try
            Return New OperacionesCajaDAM().ValidarTransferenciaClaseCuenta(portafolioOrigen, potafolioDestino, numeroCuentaOrigen, numeroCuentaDestino)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCartas(ByVal dsOperacionCaja As OperacionCajaBE, ByVal abono As Boolean, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim dsCartas As DataSet = New OperacionesCajaDAM().SeleccionarCartas(dsOperacionCaja, abono)
            Return dsCartas
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Limite1(ByVal dataRequest As DataSet) As DataSet
        Dim dsOperacionesCaja As DataSet
        Try
            dsOperacionesCaja = New OperacionesCajaDAM().Limite1()
        Catch ex As Exception
            Throw ex
        End Try
        Return dsOperacionesCaja
    End Function
    Public Function EliminarClavesFirmantesCartas(ByVal codigoOperacionCaja As String) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oOperacionesCajaDAM As New OperacionesCajaDAM
            eliminado = oOperacionesCajaDAM.EliminarClavesFirmantesCartas(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    Public Function Listar_MonedaBanco(ByVal CodigoPortafolioSBS As String, CodigoEntidad As String) As DataTable
        Try
            Dim oOperacionesCajaDAM As New OperacionesCajaDAM
            Return oOperacionesCajaDAM.Listar_MonedaBanco(CodigoPortafolioSBS, CodigoEntidad)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
     ByVal fecha As Decimal, ByVal estado As String, ByVal codigoOperacionCaja As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.SeleccionCartas(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, estado, codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImpresionCarta_Transferencias(ByVal codigoOperacionCaja As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.ImpresionCarta_Transferencias(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImpresionCarta_CancelacionDPZ(ByVal codigoOperacionCaja As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.ImpresionCarta_CancelacionDPZ(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImpresionCarta_ConstitucionDPZ(ByVal codigoOperacionCaja As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.ImpresionCarta_ConstitucionDPZ(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ActualizaDatosCancelacionesDPZ(ByVal FechaOperacion As Decimal)
        Dim operaciones As New OperacionesCajaDAM
        Try
            operaciones.ActualizaDatosCancelacionesDPZ(FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function SaldoBancario(ByVal FechaOperacion As Decimal, CodigoClaseCuenta As String, CodigoPortafolioSBS As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.SaldoBancario(FechaOperacion, CodigoClaseCuenta, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaldoBancarioOperaciones(ByVal FechaOperacion As Decimal, CodigoPortafolioSBS As String, NumeroCuenta As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.SaldoBancarioOperaciones(FechaOperacion, CodigoPortafolioSBS, NumeroCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaldoBancarioOperaciones(ByVal CodigoOperacionCaja As String, CodigoPortafolioSBS As String) As Decimal
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.ObtienCodigoExtorno(CodigoOperacionCaja, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GeneraInversiones(CodigoPortafolio As String, ByVal FechaOperacion As Decimal, NumeroCuenta As String, ByVal dataRequest As DataSet) As Integer
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return (operaciones.GeneraInversiones(CodigoPortafolio, FechaOperacion, NumeroCuenta, dataRequest))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ActualizaOperacionCaja(CodigoPortafolio As String, ByVal Importe As Decimal, CodigoOperacionCaja As String, ByVal dataRequest As DataSet)
        Dim operaciones As New OperacionesCajaDAM
        Try
            operaciones.ActualizaOperacionCaja(CodigoPortafolio, Importe, CodigoOperacionCaja, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub RegularizarIntradia(CodigoPortafolio As String, CodigoOperacionCaja As String, ByVal dataRequest As DataSet)
        Dim operaciones As New OperacionesCajaDAM
        Try
            operaciones.RegularizarIntradia(CodigoPortafolio, CodigoOperacionCaja, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function SeleccionBancos(CodigoClaseCuenta As String, CodigoPortafolioSBS As String) As DataTable
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.SeleccionBancos(CodigoClaseCuenta, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CuentaEconomica_SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, ByVal CodigoEntidad As String, _
    ByVal codigoMoneda As String) As DataTable
        Try
            Return New OperacionesCajaDAM().CuentaEconomica_SeleccionarPorFiltro(codigoPortafolio, codigoClaseCuenta, CodigoEntidad, codigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub InsertarCajaRecaudoPendiente(NumeroCuenta As String, MontoPendiente As Decimal, FechaOperacion As Decimal, FechaVencimiento As Decimal,
    dataRequest As DataSet)
        Try
            Dim oCuentaEconomica As New OperacionesCajaDAM
            oCuentaEconomica.InsertarCajaRecaudoPendiente(NumeroCuenta, MontoPendiente, FechaOperacion, FechaVencimiento, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub EliminarPendiente(Correlativo As Integer, FechaOperacion As Decimal, dataRequest As DataSet)
        Try
            Dim oCuentaEconomica As New OperacionesCajaDAM
            oCuentaEconomica.EliminarPendiente(Correlativo, FechaOperacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub ActualizarCajaRecaudoPendiente(Correlativo As Integer, NumeroCuenta As String, MontoPendiente As Decimal, FechaOperacion As Decimal, FechaVencimiento As Decimal,
    dataRequest As DataSet)
        Try
            Dim oCuentaEconomica As New OperacionesCajaDAM
            oCuentaEconomica.ActualizarCajaRecaudoPendiente(Correlativo, NumeroCuenta, MontoPendiente, FechaOperacion, FechaVencimiento, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ValidaNumeroCuenta(NumeroCuenta As String, FechaOperacion As Decimal) As Integer
        Try
            Dim oCuentaEconomica As New OperacionesCajaDAM
            Return oCuentaEconomica.ValidaNumeroCuenta(NumeroCuenta, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCajaRecaudoPendiente(ByVal codigoPortafolio As String, ByVal CodigoEntidad As String, ByVal CodigoMoneda As String, _
    ByVal NumeroCuenta As String) As DataTable
        Try
            Return New OperacionesCajaDAM().SeleccionarCajaRecaudoPendiente(codigoPortafolio, CodigoEntidad, CodigoMoneda, NumeroCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CajaRecaudoPendiente(ByVal Correlativo As Integer) As DataTable
        Try
            Return New OperacionesCajaDAM().CajaRecaudoPendiente(Correlativo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar_MonedaBanco_Clase(ByVal CodigoPortafolioSBS As String, CodigoEntidad As String, ClaseCuenta As String) As DataTable
        Try
            Return New OperacionesCajaDAM().Listar_MonedaBanco_Clase(CodigoPortafolioSBS, CodigoEntidad, ClaseCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Ruta_Carta(ByVal CodigoOperacion As String, CodigoModelo As String) As String
        Try
            Return New OperacionesCajaDAM().Ruta_Carta(CodigoOperacion, CodigoModelo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10749 - Refactorizar código de la página
    Public Sub ExtornarOperacionCaja(ByVal p_CodigoOperacionCaja As String, ByVal p_CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Try
            Using Transaction As New TransactionScope
                Dim ObjOperacionesCajaDAM As New OperacionesCajaDAM
                Dim CodigoExtorno As String
                ObjOperacionesCajaDAM.InsertarOperacionesCajaExt(p_CodigoOperacionCaja, "1", "", dataRequest, 0, p_CodigoPortafolio)
                CodigoExtorno = ObjOperacionesCajaDAM.ObtienCodigoExtorno(p_CodigoOperacionCaja, p_CodigoPortafolio)
                ObjOperacionesCajaDAM.ExtornarOperacionesCaja(CodigoExtorno, True, dataRequest)
                Transaction.Complete()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT10749 - Fin
    'OT11008 - 05/01/2018 - Ian Pastor M.
    'Descripción: Obtiene los rescates y retenciones del sistema de operaciones
    Public Function ObtenerMovimientosRescateyRetenciones(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As String, _
                                                     ByVal p_NumeroCuenta As String, ByVal p_CodigoEntidad As String, _
                                                     ByVal p_CodigoMoneda As String, ByVal p_ClaseCuenta As String, _
                                                     ByVal p_CodigoMercado As String, ByVal p_dataRequest As DataSet) As Boolean
        Try
            Dim sw As Boolean = False
            Dim objPortafolioBM As New PortafolioBM
            Dim objPortafolioBE As PortafolioBE
            Dim objPortafolioRow As PortafolioBE.PortafolioRow
            objPortafolioBE = objPortafolioBM.Seleccionar(p_CodigoPortafolio, p_dataRequest)
            objPortafolioRow = CType(objPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            Dim objOperacionesCajaDAM As New OperacionesCajaDAM
            Dim dt() As DataRow = Nothing

            'OT11192 - 09/03/2018 - Ian Pastor M.
            'Descripción: Asignar el código de moneda SOL del sistema de operaciones para encontrar los rescates
            Dim codigoMonedaSisOpe As String = IIf(p_CodigoMoneda = "NSOL", "SOL", p_CodigoMoneda).ToString()

            'OT11157 - 20/02/2018 - Ian Pastor M.
            'Verificar si el portafolio es seriado
            If objPortafolioRow.PorSerie = "N" Then
                dt = objOperacionesCajaDAM.ObtenerMovimientosRescateyRetenciones(objPortafolioRow.CodigoPortafolioSisOpe, p_Fecha) _
                                  .Select("DESCRIPCION_CORTA='" & p_CodigoEntidad & "' AND CODIGO_MONEDA='" & codigoMonedaSisOpe & "'")
            Else
                'Si es seriado se obtienen los rescates de sus series
                Dim dtRescatesAux(-1) As DataRow
                Dim DtValoresSerie As DataTable
                Dim dtRowsAux() As DataRow
                DtValoresSerie = objPortafolioBM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
                For Each dtRowPortafolio As DataRow In DtValoresSerie.Rows
                    dtRowsAux = Nothing
                    dtRowsAux = objOperacionesCajaDAM.ObtenerMovimientosRescateyRetenciones(Decimal.Parse(dtRowPortafolio("CodigoPortafolioSO")), p_Fecha) _
                                  .Select("DESCRIPCION_CORTA='" & p_CodigoEntidad & "' AND CODIGO_MONEDA='" & codigoMonedaSisOpe & "'")
                    For Each dtRow2 As DataRow In dtRowsAux
                        ReDim Preserve dtRescatesAux(UBound(dtRescatesAux) + 1)
                        dtRescatesAux(UBound(dtRescatesAux)) = dtRow2
                    Next
                Next
                If dtRescatesAux.Length > 0 Then dt = dtRescatesAux
            End If
            'OT11157 - Fin
            Using Transaction As New TransactionScope
                Dim codigoOperacionCaja As String = String.Empty
                Dim objEventoAutomaticoBE As EventosAutomaticosBE
                Dim objEventoAutomaticoDAM As New EventosAutomaticosDAM
                Dim objOperacionesCajaBM As New OperacionesCajaBM
                Dim dtOperaciones() As DataRow = objOperacionesCajaDAM.SaldoBancarioOperaciones(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_CodigoPortafolio, p_NumeroCuenta) _
                                                 .Select("CodigoOperacion='107'")

                'Extornar Operaciones Cargadas anteriormente
                For Each dtRowOpe As DataRow In dtOperaciones
                    objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, dtRowOpe("CodigoOperacionCaja").ToString(), "", "", "")
                    If objEventoAutomaticoDAM.ExisteOperacion(objEventoAutomaticoBE) Then
                        objOperacionesCajaBM.ExtornarOperacionCaja(dtRowOpe("CodigoOperacionCaja").ToString(), p_CodigoPortafolio, p_dataRequest)
                    End If
                Next

                Dim objOCBE As OperacionCajaBE
                If (dt IsNot Nothing) Then
                    'Ingresar operaciones
                    For Each dtRow As DataRow In dt
                        'Ingreso Rescate
                        If Not IsDBNull(dtRow("neto")) Then
                            If Decimal.Parse(dtRow("neto")) > 0 Then
                                objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                   p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, Decimal.Parse(dtRow("neto")), "107")
                                codigoOperacionCaja = String.Empty
                                codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                                objEventoAutomaticoBE = Nothing
                                objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "S", "107", "")
                                objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                                sw = True
                            End If
                        End If
                    Next
                End If
                Transaction.Complete()
            End Using
            ObtenerMovimientosRescateyRetenciones = sw
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function CrearObjetoOperacionesCajaBE(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As String, _
                                                     ByVal p_NumeroCuenta As String, ByVal p_CodigoEntidad As String, _
                                                     ByVal p_CodigoMoneda As String, ByVal p_ClaseCuenta As String, _
                                                     ByVal p_CodigoMercado As String, ByVal p_importe As Decimal, _
                                                     ByVal p_CodigoOperacion As String) As OperacionCajaBE
        Using objOCBE As New OperacionCajaBE
            Dim objOperacionCajaBE As OperacionCajaBE.OperacionCajaRow = objOCBE.OperacionCaja.NewOperacionCajaRow
            objOperacionCajaBE.CodigoMercado = p_CodigoMercado
            objOperacionCajaBE.CodigoPortafolioSBS = p_CodigoPortafolio
            objOperacionCajaBE.CodigoClaseCuenta = p_ClaseCuenta
            objOperacionCajaBE.NumeroCuenta = p_NumeroCuenta
            objOperacionCajaBE.CodigoModalidadPago = "CPAG"
            objOperacionCajaBE.CodigoTerceroDestino = ""
            objOperacionCajaBE.CodigoTerceroOrigen = p_CodigoEntidad
            objOperacionCajaBE.NumeroCuentaDestino = ""
            objOperacionCajaBE.CodigoOperacion = p_CodigoOperacion
            objOperacionCajaBE.Referencia = ""
            objOperacionCajaBE.CodigoMoneda = p_CodigoMoneda
            objOperacionCajaBE.Importe = p_importe
            objOperacionCajaBE.CodigoModelo = "SC01"
            objOperacionCajaBE.CodigoOperacionCaja = ""
            objOperacionCajaBE.FechaPago = UtilDM.ConvertirFechaaDecimal(p_Fecha)
            objOCBE.OperacionCaja.AddOperacionCajaRow(objOperacionCajaBE)
            Return objOCBE
        End Using
    End Function
    Private Function CrearObjetoEventoAutomatico(ByVal p_CodigoPortafolio As String, ByVal p_CodigoOperacionCaja As String, _
                                                 ByVal p_Egreso As String, ByVal p_CodigoOperacion As String, ByVal p_FlagCorte As String)
        Dim objEventoAutomaticoBE As New EventosAutomaticosBE
        objEventoAutomaticoBE.CodigoPortafolioSBS = p_CodigoPortafolio
        objEventoAutomaticoBE.NumeroOperacion = p_CodigoOperacionCaja
        objEventoAutomaticoBE.CodigoOperacion = p_CodigoOperacion
        objEventoAutomaticoBE.Egreso = p_Egreso
        objEventoAutomaticoBE.FlagCorte = p_FlagCorte
        Return objEventoAutomaticoBE
    End Function
    Public Function ObtenerMovimientosComisionesSisOpe(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As String, _
                                                     ByVal p_NumeroCuenta As String, ByVal p_CodigoEntidad As String, _
                                                     ByVal p_CodigoMoneda As String, ByVal p_ClaseCuenta As String, _
                                                     ByVal p_CodigoMercado As String, ByVal p_dataRequest As DataSet) As Boolean
        Try
            Dim sw As Boolean = False
            Dim objPortafolioBM As New PortafolioBM
            Dim objPortafolioBE As PortafolioBE
            Dim objPortafolioRow As PortafolioBE.PortafolioRow
            objPortafolioBE = objPortafolioBM.Seleccionar(p_CodigoPortafolio, p_dataRequest)
            objPortafolioRow = CType(objPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            Dim objOperacionesCajaDAM As New OperacionesCajaDAM
            Dim dt() As DataRow

            'OT11103 - 02/02/2018 - Ian Pastor M.
            'Descripción: Asignar el código de moneda SOL del sistema de operaciones para encontrar las retenciones y comisiones
            Dim codigoMonedaSisOpe As String = IIf(p_CodigoMoneda = "NSOL", "SOL", p_CodigoMoneda).ToString()

            'OT11157 - 20/02/2018 - Ian Pastor M.
            'Verificar si el portafolio es seriado
            If objPortafolioRow.PorSerie = "N" Then
                dt = objOperacionesCajaDAM.ObtenerMovimientosRescateyRetenciones(objPortafolioRow.CodigoPortafolioSisOpe, p_Fecha) _
                                  .Select("CODIGO_MONEDA='" & codigoMonedaSisOpe & "'")
            Else
                'Si es seriado se obtienen los rescates de sus series
                Dim dtRescatesAux(-1) As DataRow
                Dim DtValoresSerie As DataTable
                Dim dtRowsAux() As DataRow
                DtValoresSerie = objPortafolioBM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
                For Each dtRowPortafolio As DataRow In DtValoresSerie.Rows
                    dtRowsAux = Nothing
                    dtRowsAux = objOperacionesCajaDAM.ObtenerMovimientosRescateyRetenciones(Decimal.Parse(dtRowPortafolio("CodigoPortafolioSO")), p_Fecha) _
                                  .Select("CODIGO_MONEDA='" & codigoMonedaSisOpe & "'")
                    For Each dtRow2 As DataRow In dtRowsAux
                        ReDim Preserve dtRescatesAux(UBound(dtRescatesAux) + 1)
                        dtRescatesAux(UBound(dtRescatesAux)) = dtRow2
                    Next
                Next
                If dtRescatesAux.Length > 0 Then dt = dtRescatesAux
            End If
            'OT11157 - Fin

            Using transaction As New TransactionScope
                Dim objOperacionesCajaBM As New OperacionesCajaBM
                Dim objEventoAutomaticoBE As EventosAutomaticosBE
                Dim objEventoAutomaticoDAM As New EventosAutomaticosDAM
                Dim dtOperaciones() As DataRow = objOperacionesCajaDAM.SaldoBancarioOperaciones(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_CodigoPortafolio, p_NumeroCuenta) _
                                                 .Select("CodigoOperacion in ('CSAF','IMPU')")

                'Extornar Operaciones Cargadas anteriormente
                For Each dtRow As DataRow In dtOperaciones
                    objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, dtRow("CodigoOperacionCaja").ToString(), "", "", "")
                    If objEventoAutomaticoDAM.ExisteOperacion(objEventoAutomaticoBE) Then
                        objOperacionesCajaBM.ExtornarOperacionCaja(dtRow("CodigoOperacionCaja").ToString(), p_CodigoPortafolio, p_dataRequest)
                    End If
                Next

                If (dt IsNot Nothing) Then
                    If dt.Length > 0 Then
                        Dim comision As Decimal = 0
                        Dim retencion As Decimal = 0
                        Dim objOCBE As OperacionCajaBE
                        Dim codigoOperacionCaja As String = String.Empty
                        For Each dtRow As DataRow In dt
                            comision = comision + Decimal.Parse(IIf(Not IsDBNull(dtRow("COMISION_MONTO")), dtRow("COMISION_MONTO"), 0).ToString()) + _
                                Decimal.Parse(IIf(Not IsDBNull(dtRow("IMPUESTO_MONTO")), dtRow("IMPUESTO_MONTO"), 0).ToString())
                            retencion = retencion + Decimal.Parse(IIf(Not IsDBNull(dtRow("RETENCION_MONTO")), dtRow("RETENCION_MONTO"), 0).ToString())
                        Next
                        If comision > 0 Then
                            objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                       p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, comision, "CSAF")
                            codigoOperacionCaja = String.Empty
                            codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                            objEventoAutomaticoBE = Nothing
                            objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "S", "CSAF", "")
                            objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                            sw = True
                        End If
                        If retencion > 0 Then
                            objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                       p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, retencion, "IMPU")
                            codigoOperacionCaja = String.Empty
                            codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                            objEventoAutomaticoBE = Nothing
                            objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "S", "IMPU", "")
                            objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                            sw = True
                        End If
                    End If
                End If
                transaction.Complete()
            End Using
            ObtenerMovimientosComisionesSisOpe = sw
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11008 - Fin

    'OT11237 - 15/03/2018 - Ian Pastor M.
    'Descripción: Obtiene las suscripciones del sistema de operaciones
    Public Function ObtenerSuscripcionesSisOpe(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As String, ByVal p_ClaseCuenta As String, ByVal p_dataRequest As DataSet) As Boolean
        Dim sw As Boolean = False
        Dim bolOpeIng As Boolean = False 'Verifica si se ha registrado operaciones de rescate
        Dim objOperacionesCajaDAM As New OperacionesCajaDAM
        Dim objPortafolioBM As New PortafolioBM
        Dim objPortafolioBE As PortafolioBE
        Dim objPortafolioRow As PortafolioBE.PortafolioRow
        objPortafolioBE = objPortafolioBM.Seleccionar(p_CodigoPortafolio, p_dataRequest)
        objPortafolioRow = CType(objPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
        Dim dt As DataTable 'Tabla para obtener las suscripciones del sistema de operaciones

        'Verificar si el portafolio es seriado
        If objPortafolioRow.PorSerie = "N" Then
            dt = objOperacionesCajaDAM.ObtenerSuscripcionesSisOpe(Decimal.Parse(objPortafolioRow.CodigoPortafolioSisOpe), p_Fecha)
            Using tran As New TransactionScope
                Dim dtSaldoBancario As DataTable = objOperacionesCajaDAM.SaldoBancario(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_ClaseCuenta, p_CodigoPortafolio)
                For Each dtRow As DataRow In dtSaldoBancario.Rows
                    sw = IngresarSuscripcionesSisOpe(p_CodigoPortafolio, p_Fecha, dtRow("NumeroCuenta"), dtRow("CodigoEntidad"), dtRow("CodigoMoneda"), p_ClaseCuenta, dtRow("CodigoMercado"), dt, "", p_dataRequest)
                    If sw Then
                        bolOpeIng = sw
                    End If
                Next
                tran.Complete()
            End Using
        Else
            'Para los portafolios seriados se obtiene cada uno de sus codigosportafolios y se obtiene sus suscripciones
            'Dim dtSuscripcionesAux(-1) As DataRow
            Dim DtValoresSerie As DataTable
            'Dim dtAux As DataTable
            DtValoresSerie = objPortafolioBM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
            For Each dtRowPortafolio As DataRow In DtValoresSerie.Rows
                dt = Nothing
                dt = objOperacionesCajaDAM.ObtenerSuscripcionesSisOpe(Decimal.Parse(dtRowPortafolio("CodigoPortafolioSO")), p_Fecha)
                Using tran As New TransactionScope
                    Dim dtSaldoBancario As DataTable = objOperacionesCajaDAM.SaldoBancario(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_ClaseCuenta, p_CodigoPortafolio)
                    For Each dtRow As DataRow In dtSaldoBancario.Rows
                        sw = IngresarSuscripcionesSisOpe(p_CodigoPortafolio, p_Fecha, dtRow("NumeroCuenta"), dtRow("CodigoEntidad"), dtRow("CodigoMoneda"), p_ClaseCuenta, dtRow("CodigoMercado"), dt, dtRowPortafolio("CodigoSerie"), p_dataRequest)
                        If sw Then
                            bolOpeIng = sw
                        End If
                    Next
                    tran.Complete()
                End Using
            Next
        End If
        ObtenerSuscripcionesSisOpe = bolOpeIng
    End Function

    Public Function IngresarSuscripcionesSisOpe(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As String, _
                                                     ByVal p_NumeroCuenta As String, ByVal p_CodigoEntidad As String, _
                                                     ByVal p_CodigoMoneda As String, ByVal p_ClaseCuenta As String, _
                                                     ByVal p_CodigoMercado As String, ByVal p_DtOperacionesSisOpe As DataTable, _
                                                     ByVal p_CodigoSerie As String, ByVal p_dataRequest As DataSet) As Boolean
        Try
            Dim sw As Boolean = False
            Dim objOperacionesCajaDAM As New OperacionesCajaDAM
            Dim dt() As DataRow = Nothing
            Dim codigoMonedaSisOpe As String = IIf(p_CodigoMoneda = "NSOL", "SOL", p_CodigoMoneda).ToString()

            'FLAG_CORTE='N' => Se pueden borrar las operaciones y volverlas a ingresar
            'FLAG_CORTE='S' => No se pueden volver a ingresar las operaciones de suscripción al SIT
            dt = p_DtOperacionesSisOpe.Select("CODIGO_MONEDA='" & codigoMonedaSisOpe & "' AND VIA_PAGO_SIT='" & p_CodigoEntidad & "'")

            Dim objOperacionesCajaBM As New OperacionesCajaBM
            Dim objEventoAutomaticoBE As EventosAutomaticosBE
            Dim objEventoAutomaticoDAM As New EventosAutomaticosDAM

            If (dt IsNot Nothing) Then
                If dt.Length > 0 Then
                    Dim montoRecaudo_N As Decimal = 0
                    Dim montoRecaudo_S As Decimal = 0
                    Dim montoRecaudoIntradia_N As Decimal = 0
                    Dim montoRecaudoIntradia_S As Decimal = 0
                    Dim objOCBE As OperacionCajaBE
                    Dim codigoOperacionCaja As String = String.Empty
                    For Each dtRow As DataRow In dt
                        If (dtRow("FLAG_CORTE") = "N") Then
                            montoRecaudo_N = montoRecaudo_N + Decimal.Parse(IIf(Not IsDBNull(dtRow("MONTO_SUS")), dtRow("MONTO_SUS"), 0).ToString())
                            montoRecaudoIntradia_N = montoRecaudoIntradia_N + Decimal.Parse(IIf(Not IsDBNull(dtRow("MONTO_SUS_INTRADIA")), dtRow("MONTO_SUS_INTRADIA"), 0).ToString())
                        Else
                            montoRecaudo_S = montoRecaudo_S + Decimal.Parse(IIf(Not IsDBNull(dtRow("MONTO_SUS")), dtRow("MONTO_SUS"), 0).ToString())
                            montoRecaudoIntradia_S = montoRecaudoIntradia_S + Decimal.Parse(IIf(Not IsDBNull(dtRow("MONTO_SUS_INTRADIA")), dtRow("MONTO_SUS_INTRADIA"), 0).ToString())
                        End If
                    Next
                    If montoRecaudo_N > 0 Then
                        Dim codigoOperacion As String = String.Empty
                        If p_CodigoSerie = String.Empty Then
                            codigoOperacion = "7"
                        Else
                            If p_CodigoSerie = "SERA" Then
                                codigoOperacion = "RECA"
                            Else
                                If p_CodigoSerie = "SERB" Then
                                    codigoOperacion = "RECB"
                                End If
                            End If
                        End If

                        Dim dtOperaciones() As DataRow = objOperacionesCajaDAM.SaldoBancarioOperaciones(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_CodigoPortafolio, p_NumeroCuenta) _
                                             .Select("CodigoOperacion in ('" & codigoOperacion & "')")

                        'Extornar Operaciones Cargadas anteriormente
                        For Each dtRow As DataRow In dtOperaciones
                            objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, dtRow("CodigoOperacionCaja").ToString(), "", "", "N")
                            If objEventoAutomaticoDAM.ExisteOperacion(objEventoAutomaticoBE) Then
                                objOperacionesCajaBM.ExtornarOperacionCaja(dtRow("CodigoOperacionCaja").ToString(), p_CodigoPortafolio, p_dataRequest)
                            End If
                        Next

                        objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                   p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, montoRecaudo_N, codigoOperacion)
                        codigoOperacionCaja = String.Empty
                        codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                        objEventoAutomaticoBE = Nothing
                        objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "N", codigoOperacion, "N")
                        objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                        sw = True
                    End If
                    If montoRecaudo_S > 0 Then
                        Dim codigoOperacion As String = String.Empty
                        If p_CodigoSerie = String.Empty Then
                            codigoOperacion = "7"
                        Else
                            If p_CodigoSerie = "SERA" Then
                                codigoOperacion = "RECA"
                            Else
                                If p_CodigoSerie = "SERB" Then
                                    codigoOperacion = "RECB"
                                End If
                            End If
                        End If

                        'Dim dtOperaciones() As DataRow = objOperacionesCajaDAM.SaldoBancarioOperaciones(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_CodigoPortafolio, p_NumeroCuenta) _
                        '                     .Select("CodigoOperacion in ('" & codigoOperacion & "')")

                        ''Extornar Operaciones Cargadas anteriormente
                        'For Each dtRow As DataRow In dtOperaciones
                        '    objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, dtRow("CodigoOperacionCaja").ToString(), "", "", "S")
                        '    If objEventoAutomaticoDAM.ExisteOperacion(objEventoAutomaticoBE) Then
                        '        objOperacionesCajaBM.ExtornarOperacionCaja(dtRow("CodigoOperacionCaja").ToString(), p_CodigoPortafolio, p_dataRequest)
                        '    End If
                        'Next

                        objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                   p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, montoRecaudo_S, codigoOperacion)
                        codigoOperacionCaja = String.Empty
                        codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                        objEventoAutomaticoBE = Nothing
                        objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "N", codigoOperacion, "S")
                        objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                        sw = True
                    End If
                    If montoRecaudoIntradia_N > 0 Then
                        Dim dtOperaciones() As DataRow = objOperacionesCajaDAM.SaldoBancarioOperaciones(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_CodigoPortafolio, p_NumeroCuenta) _
                                             .Select("CodigoOperacion in ('8')")

                        'Extornar Operaciones Cargadas anteriormente
                        For Each dtRow As DataRow In dtOperaciones
                            objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, dtRow("CodigoOperacionCaja").ToString(), "", "", "N")
                            If objEventoAutomaticoDAM.ExisteOperacion(objEventoAutomaticoBE) Then
                                objOperacionesCajaBM.ExtornarOperacionCaja(dtRow("CodigoOperacionCaja").ToString(), p_CodigoPortafolio, p_dataRequest)
                            End If
                        Next

                        objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                   p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, montoRecaudoIntradia_N, "8")
                        codigoOperacionCaja = String.Empty
                        codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                        objEventoAutomaticoBE = Nothing
                        objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "N", "8", "N")
                        objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                        sw = True
                    End If
                    If montoRecaudoIntradia_S > 0 Then
                        'Dim dtOperaciones() As DataRow = objOperacionesCajaDAM.SaldoBancarioOperaciones(UtilDM.ConvertirFechaaDecimal(p_Fecha), p_CodigoPortafolio, p_NumeroCuenta) _
                        '                     .Select("CodigoOperacion in ('8')")

                        ''Extornar Operaciones Cargadas anteriormente
                        'For Each dtRow As DataRow In dtOperaciones
                        '    objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, dtRow("CodigoOperacionCaja").ToString(), "", "", "S")
                        '    If objEventoAutomaticoDAM.ExisteOperacion(objEventoAutomaticoBE) Then
                        '        objOperacionesCajaBM.ExtornarOperacionCaja(dtRow("CodigoOperacionCaja").ToString(), p_CodigoPortafolio, p_dataRequest)
                        '    End If
                        'Next

                        objOCBE = CrearObjetoOperacionesCajaBE(p_CodigoPortafolio, p_Fecha, p_NumeroCuenta, p_CodigoEntidad, _
                                                                   p_CodigoMoneda, p_ClaseCuenta, p_CodigoMercado, montoRecaudoIntradia_S, "8")
                        codigoOperacionCaja = String.Empty
                        codigoOperacionCaja = objOperacionesCajaDAM.InsertarOperacion(objOCBE, p_dataRequest)
                        objEventoAutomaticoBE = Nothing
                        objEventoAutomaticoBE = CrearObjetoEventoAutomatico(p_CodigoPortafolio, codigoOperacionCaja, "N", "8", "S")
                        objEventoAutomaticoDAM.Insertar(objEventoAutomaticoBE, p_dataRequest)
                        sw = True
                    End If
                End If
            End If
            IngresarSuscripcionesSisOpe = sw
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11237 - Fin

End Class