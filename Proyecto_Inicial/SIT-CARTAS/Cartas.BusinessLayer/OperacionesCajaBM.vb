Imports System
Imports System.Data
Imports System.Data.Common
Imports Cartas.BusinessEntities
Imports Cartas.DataAccessLayer
Public Class OperacionesCajaBM
    Public Sub New()
    End Sub
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se agrega la fecha de operacion al filtro
    Public Function ObtenerClaveFirmantes(ByVal codigoInterno As String, ByVal FechaOperacion As Decimal) As String
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.ObtenerClaveFirmantes(codigoInterno, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminarClavesFirmantesCartas(ByVal codigoOperacionCaja As String) As Boolean
        Try
            Dim oOperacionesCajaDAM As New OperacionesCajaDAM
            Return oOperacionesCajaDAM.EliminarClavesFirmantesCartas(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10025 27/02/2017 - Carlos Espejo
    'Descripcion: Se agrega la CodigoPortafolio,CodigoInterno  al filtro
    Public Function FirmarCarta(ByVal codigoOperacionCaja As String, ByVal CodigoPortafolio As String, ByVal CodigoInterno As String, ByVal claveFirma As String, _
    ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.FirmarCarta(codigoOperacionCaja, CodigoPortafolio, CodigoInterno, claveFirma, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
     ByVal fecha As Decimal, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal Vista As Integer) As SeleccionCartaBEList
        Dim operaciones As New OperacionesCajaDAM
        Dim objListAux As SeleccionCartaBEList = Nothing
        Dim objListAuxAgrupados As SeleccionCartaBEList = Nothing
        Dim objListCarta As SeleccionCartaBEList = Nothing
        Dim objSelCartaBE As SeleccionCartaBE = Nothing
        Try
            'Activar cuando se de el visto de bueno de la carta de Compra/Venta de acciones
            'operaciones.AgruparOperaciones_ComVenAcc(codigoPortafolio, fecha)
            objListAux = operaciones.SeleccionCartas(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, estado, codigoOperacionCaja, 0, "N")
            objListAuxAgrupados = operaciones.SeleccionCartas(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, estado, codigoOperacionCaja, 0, "S")

            objListCarta = New SeleccionCartaBEList
            Dim _CodigoCartaAgrupado As Integer = 0
            Dim Monto As Decimal
            For Each objCarta As SeleccionCartaBE In objListAuxAgrupados

                If objCarta.CodigoCartaAgrupado <> _CodigoCartaAgrupado And objCarta.CodigoCartaAgrupado <> 0 Then
                    objSelCartaBE = New SeleccionCartaBE
                    _CodigoCartaAgrupado = objCarta.CodigoCartaAgrupado
                    objSelCartaBE.DescripcionPortafolio = objCarta.DescripcionPortafolio
                    objSelCartaBE.DescripcionOperacion = objCarta.DescripcionOperacion
                    objSelCartaBE.ModeloCarta = objCarta.ModeloCarta
                    objSelCartaBE.DescripcionIntermediario = objCarta.Banco
                    objSelCartaBE.CodigoMoneda = objCarta.CodigoMoneda
                    Monto = 0
                    For Each objListOrden As SeleccionCartaBE In objListAux.FindAll(Function(x) x.CodigoCartaAgrupado = _CodigoCartaAgrupado)
                        objSelCartaBE.NumeroOrden = objSelCartaBE.NumeroOrden & objListOrden.NumeroOrden & ";"
                        Monto += IIf(objListOrden.CodigoOperacion = "1", objListOrden.Importe, objListOrden.Importe * -1)
                    Next
                    objSelCartaBE.Importe = IIf(Vista = 1, IIf(Monto < 0, Monto * -1, Monto), Monto)
                    objSelCartaBE.CantidadOperacion = objListAux.FindAll(Function(x) x.CodigoCartaAgrupado = _CodigoCartaAgrupado).Count
                    objSelCartaBE.VBADMIN = objCarta.VBADMIN
                    objSelCartaBE.VBGERF1 = objCarta.VBGERF1
                    objSelCartaBE.VBGERF2 = objCarta.VBGERF2
                    objSelCartaBE.CodigoOperacion = objCarta.CodigoOperacion
                    objSelCartaBE.CodigoOperacionCaja = objCarta.CodigoOperacionCaja
                    objSelCartaBE.EstadoCarta = objCarta.EstadoCarta
                    objSelCartaBE.CodigoModelo = objCarta.CodigoModelo
                    objSelCartaBE.CodigoPortafolioSBS = objCarta.CodigoPortafolioSBS
                    objSelCartaBE.CorrelativoCartas = objCarta.CorrelativoCartas
                    objSelCartaBE.NumeroCuenta = objCarta.NumeroCuenta
                    objSelCartaBE.CodigoCartaAgrupado = objCarta.CodigoCartaAgrupado
                    objSelCartaBE.Tipo = objCarta.Tipo
                    objSelCartaBE.CodigoAgrupado = objCarta.CodigoAgrupado
                    objListCarta.Add(objSelCartaBE)
                ElseIf objCarta.CodigoCartaAgrupado = 0 Then

                    _CodigoCartaAgrupado = objCarta.CodigoCartaAgrupado
                    objSelCartaBE = New SeleccionCartaBE
                    objSelCartaBE.DescripcionPortafolio = objCarta.DescripcionPortafolio
                    objSelCartaBE.DescripcionOperacion = objCarta.DescripcionOperacion
                    objSelCartaBE.ModeloCarta = objCarta.ModeloCarta
                    'objSelCartaBE.DescripcionIntermediario = objCarta.DescripcionIntermediario
                    objSelCartaBE.DescripcionIntermediario = objCarta.Banco
                    objSelCartaBE.CodigoMoneda = objCarta.CodigoMoneda
                    objSelCartaBE.Importe = objCarta.Importe
                    objSelCartaBE.NumeroOrden = objCarta.NumeroOrden
                    objSelCartaBE.CantidadOperacion = 1
                    objSelCartaBE.VBADMIN = objCarta.VBADMIN
                    objSelCartaBE.VBGERF1 = objCarta.VBGERF1
                    objSelCartaBE.VBGERF2 = objCarta.VBGERF2
                    objSelCartaBE.CodigoOperacion = objCarta.CodigoOperacion
                    objSelCartaBE.CodigoOperacionCaja = objCarta.CodigoOperacionCaja
                    objSelCartaBE.EstadoCarta = objCarta.EstadoCarta
                    objSelCartaBE.CodigoModelo = objCarta.CodigoModelo
                    objSelCartaBE.CodigoPortafolioSBS = objCarta.CodigoPortafolioSBS
                    objSelCartaBE.CorrelativoCartas = objCarta.CorrelativoCartas
                    objSelCartaBE.NumeroCuenta = objCarta.NumeroCuenta
                    objSelCartaBE.CodigoCartaAgrupado = objCarta.CodigoCartaAgrupado
                    objSelCartaBE.CodigoAgrupado = objCarta.CodigoAgrupado
                    objListCarta.Add(objSelCartaBE)
                End If
            Next
            Return objListCarta
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
     ByVal fecha As Decimal, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal codigoCartaAgrupado As Integer, ByVal Resumen As String) As SeleccionCartaBEList
        Dim operaciones As New OperacionesCajaDAM
        Try
            Return operaciones.SeleccionCartas(codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, estado, codigoOperacionCaja, codigoCartaAgrupado, Resumen)
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
    Public Function ReporteAprobacionOperacionesCaja(ByVal proceso As String, ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oOperacionCaja As New OperacionesCajaDAM
            Return oOperacionCaja.ReporteAprobacionOperacionesCaja(proceso, codigoMercado, codigoPortafolio, codigoTercero, codigoTerceroBanco, fecha, abono, estado, codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
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
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se agrega el fondo al filtro
    Public Sub AprobarOperacionCaja(ByVal codigoOperacionCaja As String, ByVal CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            oOperacionesCaja.AprobarOperacionCaja(codigoOperacionCaja, CodigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function GenerarClaveFirmantesCarta(ByVal codigoOperacionCaja As String, ByVal codigoInterno As String, ByVal claveFirma As String, _
    ByVal rutaArchivo As String, ByVal indReporte As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.GenerarClaveFirmantesCarta(codigoOperacionCaja, codigoInterno, claveFirma, rutaArchivo, indReporte, dataRequest)
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
    Public Function Ruta_Carta(ByVal CodigoOperacion As String, ByVal CodigoModelo As String) As String
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Ruta_Carta(CodigoOperacion, CodigoModelo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImpresionCarta_Constitucion_CancelacionDPZ(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.ImpresionCarta_Constitucion_CancelacionDPZ(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EsRenovacion(ByVal codigoOperacionCaja As String) As String
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.EsRenovacion(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Cartas_Deposito_Renovacion(ByVal codigoOperacionCaja As String, ByVal p_Constitucion As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Deposito_Renovacion(codigoOperacionCaja, p_Constitucion)
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
    Public Function CodigoBCR(ByVal codigoOperacionCaja As String) As String
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.CodigoBCR(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function AmpliacionBCR(ByVal codigoOperacionCaja As String) As String
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.AmpliacionBCR(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImpresionCarta_ComVenAcc(ByVal strCodigoOrden As String,
                                             ByVal strCodigoOperacion As String,
                                             ByVal strModeloCarta As String) As DataSet
        Try
            ImpresionCarta_ComVenAcc = Nothing
            Dim codigoOrden() As String
            codigoOrden = strCodigoOrden.Split(";")
            If codigoOrden.Length > 0 Then
                Dim strXML As String = "<ComVenAcc>"
                For i As Integer = 0 To codigoOrden.Length - 1
                    If codigoOrden(i) <> String.Empty Then
                        strXML &= "<DetComVenAcc>"
                        strXML &= "<CodigoOrden>" & codigoOrden(i) & "</CodigoOrden>"
                        strXML &= "</DetComVenAcc>"
                    End If
                Next
                strXML &= "</ComVenAcc>"
                Dim ObjOperacionesCajaDAM As New OperacionesCajaDAM

                ImpresionCarta_ComVenAcc = ObjOperacionesCajaDAM.ImpresionCarta_ComVenAcc(strXML, strCodigoOperacion, strModeloCarta)

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Cartas_Operacion_Cambio(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Operacion_Cambio(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Cartas_Operacion_Reporte(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Operacion_Reporte(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Cartas_Operacion_Forward(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Operacion_Forward(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Cartas_Operacion_ForwardVcto(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Operacion_ForwardVcto(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Bonos
    Public Function Cartas_Operacion_Bono(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Operacion_Bono(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImpresionCarta_CompraVentaAcciones(ByVal strCodigoOrden As String,
                                             ByVal strModeloCarta As String) As DataSet
        Try
            Dim ObjOperacionesCajaDAM As New OperacionesCajaDAM

            ImpresionCarta_CompraVentaAcciones = ObjOperacionesCajaDAM.ImpresionCarta_ComVenAccNacionales(strCodigoOrden, strModeloCarta)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Transferencias al exterior
    Public Function Cartas_Operacion_TransferenciaExterior(ByVal codigoOperacionCaja As String) As DataTable
        Try
            Dim oOperacionesCaja As New OperacionesCajaDAM
            Return oOperacionesCaja.Cartas_Operacion_TransferenciaExterior(codigoOperacionCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Operaciones de Reporte
    Public Function ImpresionCarta_OperacionReporte(ByVal strCodigoOrden As String) As DataSet
        Try
            Dim ObjOperacionesCajaDAM As New OperacionesCajaDAM

            ImpresionCarta_OperacionReporte = ObjOperacionesCajaDAM.ImpresionCarta_OperacionReporte(strCodigoOrden)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class