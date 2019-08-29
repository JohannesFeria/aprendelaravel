Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Public Class OrdenInversionFormulasBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function SeleccionarCaracValor_Acciones(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_Acciones(strCodigoValor)
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
    Public Function SeleccionarCaracValor_Acciones(ByVal portafolio As String, ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_Acciones(portafolio, strCodigoValor)
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
    Public Function SeleccionarCaracValor_Bonos(ByVal strCodigoValor As String, ByVal strFondo As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_Bonos(strCodigoValor, strFondo)
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
    'OT 10090 - 21/03/2017 - Carlos Espejo
    'Descripcion: Se elimian el parametro obsoleto
    Public Function SeleccionarCaracValor_CertificadoDeposito(ByVal strCodigoValor As String, ByVal strFondo As String, ByVal strCodigoOrden As String) As DataSet
        Try
            Dim oCaracValor As DataSet
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_CertificadoDeposito(strCodigoValor, strFondo, strCodigoOrden)
            Return oCaracValor
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCaracValor_CertificadoSuscripcion(ByVal CodigoPortafolioSBS As String, ByVal strCodigoValor As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_CertificadoSuscripcion(CodigoPortafolioSBS, strCodigoValor)
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
    Public Function SeleccionarCaracValor_InstCoberturados(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_InstCoberturados(strCodigoValor)
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
    Public Function SeleccionarCaracValor_InstEstructurados(ByVal strCodigoValor As String, ByVal fondo As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_InstEstructurados(strCodigoValor, fondo)
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
    Public Function SeleccionarCaracValor_OperacionesReporte(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_OperacionesReporte(strCodigoValor)
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
    Public Function SeleccionarCaracValor_DepositoPlazos(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_DepositoPlazos(strCodigoValor)
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
    Public Function SeleccionarCaracValor_ForwardDivisas(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_ForwardDivisas(strCodigoValor)
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
    Public Function SeleccionarCaracValor_Futuros(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_Futuros(strCodigoValor)
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
    Public Function SeleccionarCaracValor_LetrasHipotecarias(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_LetrasHipotecarias(strCodigoValor)
        Catch ex As Exception
            Throw ex
        End Try
        Return oCaracValor
    End Function

    Public Function SeleccionarCaracValor_LetrasHipotecarias(ByVal CodigoNemonico As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New OrdenInversionFormulasDAM().SeleccionarCaracValor_LetrasHipotecarias(CodigoNemonico, codigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarCaracValor_OrdenesFondo(ByVal strCodigoValor As String, ByVal strFondo As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_OrdenesFondo(strCodigoValor, strFondo)
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
    Public Function SeleccionarCaracValor_Pagares(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_Pagares(strCodigoValor)
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
    Public Function SeleccionarCaracValor_PapelesComerciales(ByVal strCodigoValor As String, ByVal strFondo As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionFormulasDAM().SeleccionarCaracValor_PapelesComerciales(strCodigoValor, strFondo)
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
    Public Function CalcularInteresesCorridos(ByVal guid As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal,
    ByVal tasainteres As Decimal, ByVal TipoTasa As String, ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {guid, codigonemonico, fechaoperacion, valornominal, tasainteres, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularInteresesCorridos(guid, codigonemonico, fechaoperacion, valornominal, tasainteres, TipoTasa)
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
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function CalcularPrecioLimpioN(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal, ByVal tasainteres As Decimal,
    ByVal TipoTasa As String) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalcularPrecioLimpioN(codigonemonico, fechaoperacion, valornominal, tasainteres, TipoTasa)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CalcularTIR(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal guid As String, ByVal valornominal As Decimal,
    ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {codigonemonico, fechaoperacion, guid, valornominal, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularTIR(codigonemonico, fechaoperacion, guid, valornominal)
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
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro datarequest
    Public Function CalcularVPN(ByVal manejaflujo As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal guid As String,
    ByVal valornominal As Decimal, ByVal tasanegociacion As Decimal, ByVal tipotasa As String) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalcularVPN(manejaflujo, codigonemonico, fechaoperacion, guid, valornominal, tasanegociacion, tipotasa)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CalcularPrecioBono(ByVal CodigoPortafolioSBS As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, _
    ByVal FechaLiquidacion As Decimal, ByVal TasaAnual As Decimal, MontoNominal As Decimal, TipoTasa As String, Operacion As String) As DataTable
        Try
            Return New OrdenInversionFormulasDAM().CalcularPrecioBono(CodigoPortafolioSBS, codigonemonico, fechaoperacion, FechaLiquidacion, TasaAnual, MontoNominal, TipoTasa, Operacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 9856 - 24/01/2017 - Carlos Espejo
    'Descripcion: Forma de calculo para CD a la par
    Public Function CalcularPrecioBono_ALAPAR(ByVal CodigoPortafolioSBS As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, _
    ByVal FechaLiquidacion As Decimal, ByVal TasaAnual As Decimal, MontoNominal As Decimal, TipoTasa As String) As DataTable
        Try
            Return New OrdenInversionFormulasDAM().CalcularPrecioBono_ALAPAR(CodigoPortafolioSBS, codigonemonico, fechaoperacion, FechaLiquidacion, _
            TasaAnual, MontoNominal, TipoTasa)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CalcularVPN2(ByVal manejaflujo As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal guid As String, _
    ByVal valornominal As Decimal, ByVal tasanegociacion As Decimal, ByVal tipotasa As String, ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {manejaflujo, codigonemonico, fechaoperacion, guid, valornominal, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularVPN2(manejaflujo, codigonemonico, fechaoperacion, guid, valornominal, tasanegociacion, tipotasa)
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
    Public Function CalcularPrecioLimpio(ByVal ValorPresenteNegociacion As Decimal, ByVal ValorPresenteCupon As Decimal, ByVal Interescorrido As Decimal,
    ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {ValorPresenteNegociacion, Interescorrido, ValorPresenteCupon, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularPrecioLimpio(ValorPresenteNegociacion, ValorPresenteCupon, Interescorrido)
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
    Public Function CalcularPrecioLimpio2(ByVal MontoNegociacion As Decimal, ByVal MontoNominal As Decimal, ByVal interescorrido As Decimal,
    ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {MontoNegociacion, MontoNominal, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularPrecioLimpio2(MontoNegociacion, MontoNominal, interescorrido)
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
    Public Function CalcularPrecioSucio(ByVal ValorPresenteNegociacion As Decimal, ByVal ValorPresenteCupon As Decimal, ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {ValorPresenteNegociacion, ValorPresenteCupon, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularPrecioSucio(ValorPresenteNegociacion, ValorPresenteCupon)
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
    Public Function CalcularPrecioSucio3(ByVal MontoOperacion As Decimal, ByVal MontoNominal As Decimal, ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {MontoOperacion, MontoNominal, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularPrecioSucio3(MontoOperacion, MontoNominal)
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
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro datarequest
    Public Function CalcularNumeroPapeles(ByVal MontoNominalOperacion As Decimal, ByVal montoNominalunidades As Decimal) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalcularNumeroPapeles(MontoNominalOperacion, montoNominalunidades)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CalcularMontoOperacion2(ByVal guid As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal,
    ByVal YMT As Decimal, ByVal tipotasa As String, ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {guid, codigonemonico, fechaoperacion, valornominal, YMT, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularMontoOperacion2(guid, codigonemonico, fechaoperacion, valornominal, YMT, tipotasa)
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
    Public Function CalcularDuracionOI(ByVal guid As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal,
    ByVal YMT As Decimal, ByVal tipotasa As String, ByVal datarequest As DataSet) As Decimal
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {guid, codigonemonico, fechaoperacion, valornominal, YMT, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalcularDuracionOI(guid, codigonemonico, fechaoperacion, valornominal, YMT, tipotasa)
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
    Public Function CalularDiferencial(ByVal tcvcto As Decimal, ByVal tcspot As Decimal, ByVal fechavcto As Decimal, ByVal fechaconst As Decimal,
    ByVal datarequest As DataSet)
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {tcvcto, tcspot, fechavcto, fechaconst, datarequest}
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().CalularDiferencial(tcvcto, tcspot, fechavcto, fechaconst)
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
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function CalcularInteresesCorridos2(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal, ByVal YMT As Decimal,
    ByVal tipotasa As String, Optional ByVal FlagOperacionReporte As Integer = 0) As Decimal
        Try
            Dim oCaracValor As Decimal
            oCaracValor = New OrdenInversionFormulasDAM().CalcularInteresesCorridos(codigonemonico, fechaoperacion, valornominal, YMT, tipotasa, FlagOperacionReporte)
            Return oCaracValor
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function OntenerMontoCupon(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal) As Decimal
        Dim oCaracValor As Decimal
        Try
            oCaracValor = New OrdenInversionFormulasDAM().OntenerMontoCupon(codigonemonico, fechaoperacion, valornominal)
        Catch ex As Exception
            Throw ex
        End Try
        Return oCaracValor
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function calcularTasanegociacion(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal montonominal As Decimal, ByVal tipotasa As String,
    ByVal montooperacion As Decimal) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().calcularTasanegociacion(codigonemonico, fechaoperacion, montonominal, tipotasa, montooperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function calcularTasanegociacionPrecio(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal montonominal As Decimal, _
    ByVal tipotasa As String, ByVal preciolimpio As Decimal) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().calcularTasanegociacionPrecio(codigonemonico, fechaoperacion, montonominal, tipotasa, preciolimpio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function CalularValorOperacionDPCD(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal ValorNominal As Decimal, ByVal Tasa As Decimal,
    ByVal nemonico As String) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalularValorOperacionDPCD(FechaInicio, FechaFin, ValorNominal, Tasa, nemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro  datarequest
    Public Function CalularValorOperacionDPCDPrecio(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal ValorNominal As Decimal, ByVal Precio As Decimal,
    ByVal nemonico As String) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalularValorOperacionDPCDPrecio(FechaInicio, FechaFin, ValorNominal, Precio, nemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function CalcularMontoNominalVAC(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalcularMontoNominalVAC(codigonemonico, fechaoperacion, valornominal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID Y datarequest
    Public Function CalculoNroPapelesBonoAmortPrin(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal montooperacion As Decimal,
    ByVal valorunidades As Decimal) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().CalculoNroPapelesBonoAmortPrin(codigonemonico, fechaoperacion, montooperacion, valorunidades)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function RestriccionExcesosBroker(ByVal fechaoperacion As Decimal, ByVal codigotercero As String, ByVal montooperacion As Decimal,
    ByVal codigonemonico As String) As String
        Dim parameters As Object() = {fechaoperacion, codigotercero, montooperacion}
        Dim oCaracValor As String
        Try
            oCaracValor = New OrdenInversionFormulasDAM().RestriccionExcesosBroker(fechaoperacion, codigotercero, montooperacion, codigonemonico)
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
    Public Function ValidarTresuryADescuento(ByVal codigonemonico As String) As String
        Dim parameters As Object() = {codigonemonico}
        Dim oCaracValor As String
        Try
            oCaracValor = New OrdenInversionFormulasDAM().ValidarTresuryADescuento(codigonemonico)
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
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista mercado por nemonico
    Public Function ValorMercado(ByVal codigoMnemonico As String) As Decimal
        Try
            Return New OrdenInversionFormulasDAM().ValorMercado(codigoMnemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class