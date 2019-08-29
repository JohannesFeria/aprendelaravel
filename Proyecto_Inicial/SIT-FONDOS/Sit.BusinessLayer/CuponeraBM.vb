Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Imports Sit.BusinessEntities
Public Class CuponeraBM
    ' Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorOrdenInversion(ByVal strCodigoMnemonico As String, ByVal dataRequest As DataSet) As DataTable

        Dim oCuponeraDAM As New CuponeraDAM
        Try
            Return New CuponeraDAM().SeleccionarPorOrdenInversion(strCodigoMnemonico, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarVencimientos(ByVal codigoNemonico As String, ByVal valorNominal As Decimal, _
        ByVal fecha As String, ByVal codigoPortafolio As String, ByRef saldo As Decimal, _
        ByVal codigoOrden As String, ByRef codigoISIN As String, ByRef codigoSBS As String, _
        ByRef fechaVencimiento As Decimal, ByVal dataRequest As DataSet, secuencial As String) As DataSet
     
        Dim oCuponeraDAM As New CuponeraDAM
        Try

            Return New CuponeraDAM().SeleccionarVencimientos(codigoNemonico, valorNominal, fecha, codigoPortafolio, _
                saldo, codigoOrden, codigoISIN, codigoSBS, fechaVencimiento, secuencial)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarAmortizaciones(ByVal codigoOrden As String, ByVal codigoNemonico As String, _
        ByVal valorNominal As Decimal, ByVal fecha As String, ByVal codigoPortafolio As String, _
        ByRef cantidadOperacion As Decimal, ByRef codigoISIN As String, ByRef codigoSBS As String, _
        ByRef fechaVencimiento As Decimal, ByVal dataRequest As DataSet, Secuencial As String) As DataSet

        Dim oCuponeraDAM As New CuponeraDAM
        Try
            Return New CuponeraDAM().SeleccionarAmortizaciones(codigoOrden, codigoNemonico, valorNominal, fecha, _
            codigoPortafolio, cantidadOperacion, codigoISIN, codigoSBS, fechaVencimiento, Secuencial)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CuponeraTieneCuponVencido(ByVal codigoOrden As String, ByVal dataRequest As DataSet) As Boolean

        Dim oCuponeraDAM As New CuponeraDAM
        Try
            Return New CuponeraDAM().CuponeraTieneCuponVencido(codigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorOrdenInversionVPN(ByVal codigoMnemonico As String, ByVal GUID As String, ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As String, ByVal dataRequest As DataSet) As DataTable
        Dim oCuponeraDAM As New CuponeraDAM
        Try
            Return New CuponeraDAM().SeleccionarPorOrdenInversionVPN(codigoMnemonico, GUID, codigoOrden, codigoPortafolioSBS, fechaOperacion, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GenerarCuponeraNormal(ByVal strCodigoAmortiza As String, ByVal strFechaEmision As String, ByVal strFechaVcto As String, ByVal strFechaPriCupon As String, ByVal decTasaCupon As String, ByVal decBaseCupon As String, ByVal intPeriodicidad As String, ByVal decTasaSpread As String, ByVal numDias As String, ByVal dataRequest As DataSet) As DataSet
        Dim cuponera As DataSet
        Try
            cuponera = New CuponeraDAM().GenerarCuponeraNormal(strCodigoAmortiza, strFechaEmision, strFechaVcto, strFechaPriCupon, decTasaCupon, decBaseCupon, intPeriodicidad, decTasaSpread, numDias)
        Catch ex As Exception
            Throw ex
        End Try
        Return cuponera
    End Function

    'Nueva Cuponera
    Public Function CalcularCuponeraNormal(ByVal codigoNemonico As String, _
                                           ByVal existeCuponera As Boolean, _
                                           ByVal codigoPeriodicidad As String, _
                                           ByVal valorNominal As Decimal, _
                                           ByVal codigoTipoAmortizacion As String, _
                                           ByVal strFechaEmision As String, _
                                           ByVal strFechaVencimiento As String, _
                                           ByVal strFechaPrimerCupon As String, _
                                           ByVal decTasaCupon As String, _
                                           ByVal decBaseCupon As String, _
                                           ByVal decTasaSpread As String, _
                                           ByVal numDias As String, _
                                           ByVal FlagSinCalcular As String, _
                                           ByVal dataRequest As DataSet) As DataSet
        Dim cuponera As DataSet
        Try
            cuponera = New CuponeraDAM().CalcularCuponeraNormal(codigoNemonico, _
                                                                existeCuponera, _
                                                                codigoPeriodicidad, _
                                                                valorNominal, _
                                                                codigoTipoAmortizacion, _
                                                                strFechaEmision, _
                                                                strFechaVencimiento, _
                                                                strFechaPrimerCupon, _
                                                                decTasaCupon, _
                                                                decBaseCupon, _
                                                                decTasaSpread, _
                                                                numDias, _
                                                                FlagSinCalcular)
        Catch ex As Exception
            Throw ex
        End Try
        Return cuponera
    End Function

    Public Function Obtener_CuponActual(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim cuponera As DataSet
        Try
            cuponera = New CuponeraDAM().Obtener_CuponActual(codigoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
        Return cuponera
    End Function

    Public Function ObtenerCuponera(ByVal CodigoTipoAmortizacionOriginal As String, ByVal fechaEmisionOriginal As Decimal, ByVal fechaVctoOriginal As Decimal, _
    ByVal fechaPriCuponOriginal As Decimal, ByVal CodigoPeriodicidadOriginal As String, ByVal tasaCuponOriginal As Decimal, ByVal baseCuponOriginal As String, _
    ByVal tasaSpreadOriginal As Decimal, ByVal numeroDiasOriginal As String, ByVal MontoNominalOriginal As Decimal, ByVal CadenaNemonico As String, ByVal ImporteVentaTotal As Decimal, _
    ByVal tipoCuponera As String, ByVal CodigoTipoAmortizacion As String, ByVal fechaEmision As Decimal, ByVal fechaVcto As Decimal, ByVal fechaPriCupon As Decimal, _
    ByVal CodigoPeriodicidad As String, ByVal tasaCupon As Decimal, ByVal baseCupon As String, ByVal tasaSpread As Decimal, ByVal numeroDias As String, ByVal MontoNominal As Decimal, _
    ByVal fechaOperacion As Decimal, ByVal diaTOriginal As Decimal, ByVal diaT As Decimal, ByVal portafolio As String) As DataTable
        Dim cuponera As DataTable
        Try
            cuponera = New CuponeraDAM().ObtenerCuponera(CodigoTipoAmortizacionOriginal, _
                                                         fechaEmisionOriginal, _
                                                         fechaVctoOriginal, _
                                                         fechaPriCuponOriginal, _
                                                         CodigoPeriodicidadOriginal, _
                                                         tasaCuponOriginal, _
                                                         baseCuponOriginal, _
                                                         tasaSpreadOriginal, _
                                                         numeroDiasOriginal, _
                                                         MontoNominalOriginal, _
                                                         CadenaNemonico, _
                                                         ImporteVentaTotal, _
                                                         tipoCuponera, _
                                                         CodigoTipoAmortizacion, _
                                                         fechaEmision, _
                                                         fechaVcto, _
                                                         fechaPriCupon, _
                                                         CodigoPeriodicidad, _
                                                         tasaCupon, _
                                                         baseCupon, _
                                                         tasaSpread, _
                                                         numeroDias, _
                                                         MontoNominal,
                                                         fechaOperacion,
                                                         diaTOriginal,
                                                         diaT,
                                                         portafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return cuponera
    End Function
    Public Function GenerarCuponeraEspecial(ByVal strFlagAmortiza As String, ByVal strNroCupones As String, ByVal strFechaPriCupon As String, ByVal decTasaCupon As String, ByVal decBaseCupon As String, ByVal intPeriodicidad As String, ByVal decTasaSpread As String, ByVal dataRequest As DataSet) As DataSet
        Dim cuponera As DataSet

        Try
            cuponera = New CuponeraDAM().GenerarCuponeraEspecial(strFlagAmortiza, strNroCupones, strFechaPriCupon, decTasaCupon, decBaseCupon, intPeriodicidad, decTasaSpread)

        Catch ex As Exception
            Throw ex
        End Try
        Return cuponera
    End Function
    Public Function RegistrarCuponeraNormal(ByVal strNemonico As String, ByVal dtAux As DataTable, ByVal dataRequest As DataSet, _
                                            ByVal tipoTasaVariable As String, ByVal tasaVariable As Decimal, ByVal periodicidadTasaVariable As Integer) As Boolean

        Dim oCuponeraBE As CuponeraNormalBE
        Dim oCuponera As CuponeraDAM
        Dim cuponReferencial As Integer = 0
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            For i = 0 To dtAux.Rows.Count - 1
                oCuponeraBE = crearObjeto_CuponeraNormal(strNemonico, dtAux.Rows(i), i + 1)
                oCuponera = New CuponeraDAM
                oCuponera.RegistrarCuponeraNormal(oCuponeraBE, dtAux.Rows(i)("AmortizacConsolidado"), dataRequest)
                If cuponReferencial = 0 And tipoTasaVariable <> String.Empty Then cuponReferencial = IIf(cuponReferencial = 0 And dtAux.Rows(i)("Estado").ToString = "1", CInt(dtAux.Rows(i)("Consecutivo")), 0)
            Next

            If tipoTasaVariable <> String.Empty Then
                oCuponera.Valores_ActualizarTasaVariable(strNemonico, tipoTasaVariable, tasaVariable, periodicidadTasaVariable, cuponReferencial)
            End If


            blnResul = True
        Catch ex As Exception
            Throw ex
        End Try
        Return blnResul
    End Function
    Public Function EliminarCuponeraNormal(ByVal strNemonico As String, ByVal dataRequest As DataSet) As Boolean

        Dim oCuponera As New CuponeraDAM
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            oCuponera.EliminarCuponeraNormal(strNemonico, dataRequest)
            blnResul = True
        Catch ex As Exception
        End Try
        Return blnResul
    End Function
    Public Function EliminarCuponeraNormal_Cupon(ByVal dtAux As DataTable, ByVal dataRequest As DataSet) As Boolean

        Dim oCuponera As New CuponeraDAM
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            For i = 0 To dtAux.Rows.Count - 1
                oCuponera.EliminarCuponeraNormal_Cupon(dtAux.Rows(i)(0), dtAux.Rows(i)(1), dataRequest)
            Next

            blnResul = True
        Catch ex As Exception
            Throw ex
        End Try
        Return blnResul
    End Function
    Private Function crearObjeto_CuponeraNormal(ByVal strNemonico As String, ByVal oDR As DataRow, ByVal numRow As Integer) As CuponeraNormalBE
        Dim oCuponNormalBE As New CuponeraNormalBE
        Dim oRow As CuponeraNormalBE.CuponeraNormalRow
        oRow = CType(oCuponNormalBE.CuponeraNormal.NewRow(), CuponeraNormalBE.CuponeraNormalRow)
        oRow.CodigoNemonico = strNemonico
        oRow.Secuencia = numRow
        oRow.FechaInicio = oDR("FechaIni") 'ConvertirFechaaDecimal(oDR("FechaIni"))
        oRow.FechaTermino = oDR("FechaFin") 'ConvertirFechaaDecimal(oDR("FechaFin"))
        oRow.DiferenciaDias = oDR("DifDias")
        oRow.Amortizacion = oDR("Amortizac")
        oRow.TasaCupon = oDR("TasaCupon")
        oRow.TasaVariable = IIf(oDR("TasaVariable") Is DBNull.Value, 0, oDR("TasaVariable"))
        oRow.Base = oDR("BaseCupon")
        oRow.DiasPago = oDR("DiasPago")
        oRow.Situacion = "A"
        oCuponNormalBE.CuponeraNormal.AddCuponeraNormalRow(oRow)
        oCuponNormalBE.CuponeraNormal.AcceptChanges()
        Return oCuponNormalBE
    End Function
    Public Function RegistrarCuponeraEspecial(ByVal strNemonico As String, ByVal dtAux As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim oCuponeraBE As CuponeraEspecialBE
        Dim oCuponera As CuponeraDAM
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            For i = 0 To dtAux.Rows.Count - 1
                oCuponeraBE = crearObjeto_CuponeraEspecial(strNemonico, dtAux.Rows(i), i + 1)
                oCuponera = New CuponeraDAM
                oCuponera.RegistrarCuponeraEspecial(oCuponeraBE, dataRequest)
            Next
            blnResul = True
        Catch ex As Exception
            Throw ex
        End Try
        Return blnResul
    End Function
    Private Function crearObjeto_CuponeraEspecial(ByVal strNemonico As String, ByVal oDR As DataRow, ByVal numRow As Integer) As CuponeraEspecialBE
        Dim oCuponEspecialBE As New CuponeraEspecialBE
        Dim oRow As CuponeraEspecialBE.CuponeraEspecialRow
        oRow = CType(oCuponEspecialBE.CuponeraEspecial.NewRow(), CuponeraEspecialBE.CuponeraEspecialRow)
        oRow.CodigoNemonico = strNemonico
        oRow.Secuencia = numRow
        oRow.FechaInicio = oDR("FechaIni") 'ConvertirFechaaDecimal(oDR("FechaIni"))
        oRow.FechaTermino = oDR("FechaFin") 'ConvertirFechaaDecimal(oDR("FechaFin"))
        oRow.DiferenciaDias = oDR("DifDias")
        oRow.Amortizacion = oDR("Amortizac")
        oRow.TasaCupon = oDR("TasaCupon")
        oRow.Base = oDR("BaseCupon")
        oRow.DiasPago = oDR("DiasPago")
        oRow.Situacion = "A"
        oCuponEspecialBE.CuponeraEspecial.AddCuponeraEspecialRow(oRow)
        oCuponEspecialBE.CuponeraEspecial.AcceptChanges()
        Return oCuponEspecialBE
    End Function
    Public Function EliminarCuponeraEspecial(ByVal strNemonico As String, ByVal dataRequest As DataSet) As Boolean

        Dim oCuponera As New CuponeraDAM
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            oCuponera.EliminarCuponeraEspecial(strNemonico, dataRequest)
            blnResul = True
        Catch ex As Exception
            Throw ex
        End Try
        Return blnResul
    End Function
    Public Function EliminarCuponeraEspecial_Cupon(ByVal dtAux As DataTable, ByVal dataRequest As DataSet) As Boolean

        Dim oCuponera As New CuponeraDAM
        Dim blnResul As Boolean = False
        Dim i As Integer
        Try
            For i = 0 To dtAux.Rows.Count - 1
                oCuponera.EliminarCuponeraEspecial_Cupon(dtAux.Rows(i)(0), dtAux.Rows(i)(1), dataRequest)
            Next

            blnResul = True
        Catch ex As Exception
            Throw ex
        End Try
        Return blnResul
    End Function
    Public Function LeerCuponeraEspecial(ByVal strNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim cuponera As DataSet
        Try
            cuponera = New CuponeraDAM().LeerCuponeraEspecial(strNemonico)
        Catch ex As Exception

            Throw ex
        End Try
        Return cuponera
    End Function
    Public Function LeerCuponeraNormal(ByVal strNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim cuponera As DataSet

        Try
            cuponera = New CuponeraDAM().LeerCuponeraNormal(strNemonico)
        Catch ex As Exception
            Throw ex
        End Try
        Return cuponera
    End Function
    Public Function InsertarCuponeraOI(ByVal flag As Integer, ByVal GUID As String, ByVal temporal As Boolean, ByVal strCodigoOrden As String, ByVal strCodigoPortafolioSBS As String, ByVal dclFechaInicio As Decimal, ByVal dclFechaFin As Decimal, ByVal dclAmortizacion As Decimal, ByVal dclValorNominalOrigen As Decimal, ByVal strModalidad As String, ByVal strPeriodicidad As String, ByVal strRecalcula As String, ByVal dclTasaNominal As Decimal, ByVal dclVPN As Decimal, ByVal dclMontoNominal As Decimal, ByVal tipoTasa As String, ByVal diferenciaDias As Decimal, ByVal periodicidad As Decimal, ByVal base As Decimal, ByVal codigoTipoAmortizacion As String, ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim cuponera As DataSet
        Dim oCuponera As New CuponeraDAM
        Try
            oCuponera.InsertarCuponeraOI(flag, GUID, temporal, strCodigoOrden, strCodigoPortafolioSBS, dclFechaInicio, dclFechaFin, dclAmortizacion, dclValorNominalOrigen, strModalidad, strPeriodicidad, strRecalcula, dclTasaNominal, dclVPN, dclMontoNominal, tipoTasa, diferenciaDias, periodicidad, base, codigoTipoAmortizacion, codigoNemonico, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminarCuponeraOI(ByVal GUID As String, ByVal IsTemporal As String, ByVal strCodigoOrden As String, ByVal strCodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim cuponera As DataSet

        Dim oCuponera As New CuponeraDAM
        Try
            oCuponera.EliminarCuponeraOI(GUID, IsTemporal, strCodigoOrden, strCodigoPortafolioSBS, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConfirmarVencimientoCuponeraOI(ByVal portafolio As String, ByVal codigoNemonico As String, ByVal fechaVencimiento As Decimal, ByVal montoNominalLocal As Decimal, ByVal secuencial As String, ByVal fechaIDI As Decimal, ByVal fechaPago As Decimal, ByVal ordenInversion As String, ByVal monedaDestino As String, ByVal montoOrigen As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim cuponera As DataSet

        Dim oCuponera As New CuponeraDAM
        Dim rpta As Boolean = False
        Try
            rpta = oCuponera.ConfirmarVencimientoCuponeraOI(portafolio, codigoNemonico, fechaVencimiento, montoNominalLocal, secuencial, fechaIDI, fechaPago, ordenInversion, monedaDestino, montoOrigen, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function
    Public Function SecuenciaCuponera(ByVal CodigoOrden As String, ByVal Portafolio As String) As String
        Try
            Return New CuponeraDAM().SecuenciaCuponera(CodigoOrden, Portafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCuponesPorVencer() As DataTable
        Dim oCuponeraDAM As New CuponeraDAM
        Try
            Return New CuponeraDAM().ListarCuponesPorVencer()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CuponeraNormal_ObtenerPorcentajeParticipacion(ByVal CodigoNemonico As String, ByVal FechaIni As Decimal, ByVal FechaFin As Decimal, _
                                                                  ByVal Consecutivo As Integer, ByVal Estado As Integer, ByVal MontoNominalTotal As Decimal, _
                                                                  ByVal TasaCupon As Decimal, ByVal DifDias As Decimal, ByVal BaseCupon As Integer, _
                                                                  ByVal Amortizac As Decimal, ByVal SumaMontoAmortizacion As Decimal) As DataTable
        Dim oCuponeraDAM As New CuponeraDAM
        Try
            Return New CuponeraDAM().CuponeraNormal_ObtenerPorcentajeParticipacion(CodigoNemonico, _
                                                                                   FechaIni, _
                                                                                   FechaFin, _
                                                                                   Consecutivo, _
                                                                                   Estado, _
                                                                                   MontoNominalTotal, _
                                                                                   TasaCupon, _
                                                                                   DifDias, _
                                                                                   BaseCupon, _
                                                                                   Amortizac, _
                                                                                   SumaMontoAmortizacion)


        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class