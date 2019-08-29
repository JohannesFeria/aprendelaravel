Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class ParametrosGeneralesBM
    'Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal dataRequest As DataSet)
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            oParametrosGeneralesDAM.Insertar(clasificacion, nombre, valor, comentario, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal dataRequest As DataSet) As DataTable
        Dim tbl As New DataTable
        Try
            tbl = New ParametrosGeneralesDAM().SeleccionarPorFiltro(clasificacion, nombre, valor, comentario, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return tbl
    End Function

    Public Function ListarCalifInstr(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarCalifInstr(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBaseTir(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarBaseTir(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBaseTirNDias(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarBaseTirNDias(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBaseCupon(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarBaseCupon(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarDefaultTipoContabilidad(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarDefaultTipoContabilidad(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarDefaultCentroCosto(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarDefaultCentroCosto(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBaseCuponNDias(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarBaseCuponNDias(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarFondosInversion(ByVal dataRequest As DataSet, Optional ByVal ordenFondo As String = "") As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarFondosInversion(dataRequest, ordenFondo).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarSituacion(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarSituacion(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Listar_TipoFactor(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().Listar_TipoFactor(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarRutaBloomberg(ByVal dataRequest As DataSet) As DataTable
        Dim Tabla As New DataTable
        Try
            Tabla = New ParametrosGeneralesDAM().ListarRutaBloomberg(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return Tabla
    End Function
    Public Function ListarInterfaseBloomberg(ByVal dataRequest As DataSet) As DataTable
        Dim Tabla As New DataTable
        Try
            Tabla = New ParametrosGeneralesDAM().ListarInterfaseBloomberg(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return Tabla
    End Function
    Public Function ListarSecuencial(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarSecuencial(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarRelacion(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarRelacion(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTablaInstrumento(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTablaInstrumento(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCampoInstrumento(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarCampoInstrumento(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarPosicion(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarPosicion(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCriterioTabla(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarCriterioTabla(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCriterioCampo(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarCriterioCampo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarNaturaleza(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarNaturaleza(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoTasa(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoTasa(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarMaduracion(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarMaduracion(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarPeriodos(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarPeriodos(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBaseCalculo(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarBaseCalculo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoPrecio(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoPrecio(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoTarifa(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoTarifa(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoCalculo(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoCalculo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarUnidadPosicion(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarUnidadPosicion(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarValorBase(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarValorBase(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarAplicarCastigo(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarAplicarCastigo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoFactor(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoFactor(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarVencimiento(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarVencimiento(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarSaldoBanco(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarSaldoBanco(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoPersona(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoPersona(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarRutaParametria(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarRutaParametria(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoIntermediario(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoIntermediario(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarEntidadesExternas(ByVal Clasificacion As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New ParametrosGeneralesDAM().ListarEntidadesExternas(Clasificacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarParametros(ByVal Clasificacion As String, _
                                    ByVal Nombre As String, _
                                    ByVal Valor As String, _
                                    ByVal Comentario As String, _
                                    ByVal dataRequest As DataSet) As DataSet
        Try
            Return New ParametrosGeneralesDAM().ListarParametros(Clasificacion, Nombre, Valor, Comentario, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarRating(ByVal Nombre As String, _
                                    ByVal Valor As String, _
                                    ByVal Comentario As String, _
                                    ByVal Local As String,
                                    ByVal dataRequest As DataSet) As DataSet
        Try
            Return New ParametrosGeneralesDAM().ListarRating(Nombre, Valor, Comentario, Local, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarAgrupacion(ByVal dataRequest As DataSet) As DataTable
        Try
            Dim DtTabla As DataTable
            DtTabla = New ParametrosGeneralesDAM().ListarAgrupacion(dataRequest).Tables(0)
            Return DtTabla
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal Clasificacion As String, ByVal dataRequest As DataSet) As DataTable
        Dim DtTabla As DataTable
        Try
            Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
            DtTabla = oParametrosGeneralesDAM.Listar(Clasificacion).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return DtTabla
    End Function
    Public Function ListarCompraVentaME_Mnemonico(ByVal dataRequest As DataSet) As DataTable
        Dim DtTabla As DataTable
        Try
            DtTabla = New ParametrosGeneralesDAM().ListarCompraVentaME_Mnemonico().Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return DtTabla
    End Function

    Public Function ListarFisDes(ByVal dataRequest As DataSet) As DataTable
        Try
            Dim DtTabla As DataTable
            'DtTabla = New ParametrosGeneralesDAM().ListarFisDes(dataRequest).Tables(0)
            Return DtTabla
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarTipoContabilidad(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoContabilidad(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarMatrizContable(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarMatrizContable(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarOrdenFondo(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarOrdenFondo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarClaseLimite(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarClaseLimite(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoLimite(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoLimite(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTopeLimite(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTopeLimite(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarTipoContabilidad(ByVal strCodigo As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().SeleccionarTipoContabilidad(strCodigo, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoAsignacion(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoAsignacion(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTipoAsignacionPorImporte(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarTipoAsignacionPorImporte(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarMatriz(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            oParametrosGeneralesBE = oParametrosGeneralesDAM.ListarMatriz(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return oParametrosGeneralesBE
    End Function
    Public Function ListarEncaje(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            oParametrosGeneralesBE = oParametrosGeneralesDAM.ListarEncaje(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return oParametrosGeneralesBE
    End Function
    Public Function ListarEstadoOI(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            Return New ParametrosGeneralesDAM().ListarEstadosOI(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBaseImp(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            Return New ParametrosGeneralesDAM().ListarBaseImp(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarAmortizacionVencimiento(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ParametrosGeneralesDAM().ListarAmortizacionVencimiento(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarGrupoNormativo(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As New DataTable
        Try
            dt = New ParametrosGeneralesDAM().ListarGrupoNormativo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function ListarGrupoContable(ByVal CodigoTipoInstrumentoSBS As String, ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As New DataTable
        Try
            dt = New ParametrosGeneralesDAM().ListarGrupoContable(CodigoTipoInstrumentoSBS, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function ListarEntidadVinculadaGrupoEconomico(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As New DataTable
        Try
            dt = New ParametrosGeneralesDAM().ListarEntidadVinculadaGrupoEconomico(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function ListarEntidadVinculadaEmisor(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As New DataTable
        Try
            dt = New ParametrosGeneralesDAM().ListarEntidadVinculadaEmisor(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function ObtenerRutaModeloCarta(ByVal dataRequest As DataSet) As DataTable
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As New DataTable
        Try
            dt = New ParametrosGeneralesDAM().ObtenerRutaModeloCarta(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function ListarRutaGeneracionCartas(ByVal datarequest As DataSet) As String
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As String
        Try
            dt = oParametrosGeneralesDAM.ListarRutaGeneracionCartas()
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function ObtenerProximoSecuencial(ByVal tabla As String, ByVal datarequest As DataSet) As String
        Dim oParametrosGeneralesBE As New DataTable
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Dim dt As String
        Try
            dt = oParametrosGeneralesDAM.ObtenerProximoSecuencial(tabla)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function ListarGrupoIntermediario() As DataSet
        Dim dsAux As DataSet
        Try
            Dim oParamGenDAM As New ParametrosGeneralesDAM
            dsAux = oParamGenDAM.ListarGrupoIntermediario()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsAux
    End Function

    Public Function ListarCondicionPrevOI() As DataSet
        Dim dsAux As DataSet
        Try
            Dim oParamGenDAM As New ParametrosGeneralesDAM
            dsAux = oParamGenDAM.ListarCondicionPrevOI()
        Catch ex As Exception
            Throw ex
        End Try
        Return dsAux
    End Function

    Public Function ListarMedioNegociacionPrevOI(ByVal TipoRenta As String) As DataSet
        Dim dsAux As DataSet
        Try
            Dim oParamGenDAM As New ParametrosGeneralesDAM
            dsAux = oParamGenDAM.ListarMedioNegociacionPrevOI(TipoRenta)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsAux
    End Function

    Public Function ListarDerivadasLimiteInstrumento() As DataSet
        Dim dsAux As DataSet
        Try
            Dim oParamGenDAM As New ParametrosGeneralesDAM
            dsAux = oParamGenDAM.ListarDerivadasLimitesInstrumentos()
        Catch ex As Exception
            Throw ex
        End Try
        Return dsAux
    End Function

    'CMB OT 63063 20110419 REQ 15
    'Public Function ListarAprobadorCarta(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As DataSet
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {codigoInterno, dataRequest}
    '    Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
    '    Try
    '        RegistrarAuditora(parameters)
    '        Return oParametrosGeneralesDAM.ListarAprobadorCarta(codigoInterno)
    '    Catch ex As Exception
    '        RegistrarAuditora(parameters, ex)
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function

    'CMB OT 63063 20110504 REQ 15
    'Public Function ListarFirmantesCartas(ByVal dataRequest As DataSet) As DataSet
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {dataRequest}
    '    Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
    '    Try
    '        RegistrarAuditora(parameters)
    '        Return oParametrosGeneralesDAM.ListarFirmantesCartas()
    '    Catch ex As Exception
    '        RegistrarAuditora(parameters, ex)
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function

    'CMB OT 63063 20110504 REQ 15
    Public Function ListarClaveFirmantesCartas(ByVal dataRequest As DataSet) As DataSet
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            Return oParametrosGeneralesDAM.ListarClaveFirmantesCartas()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarSubLista(ByVal clasificacion As String, ByVal valor2 As String, ByVal dataRequest As DataSet) As DataTable
        Dim dtTabla As DataTable
        Try
            Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
            dtTabla = oParametrosGeneralesDAM.ListarSubLista(clasificacion, valor2).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return dtTabla
    End Function

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa función para capturar valores de tabla Parametros Generales | 17/05/18 
    Public Function ListarTipoRentaRiesgo(ByVal dataRequest As DataSet) As DataTable
        Try
            Dim DtTabla As DataTable
            DtTabla = New ParametrosGeneralesDAM().ListarTipoRentaRiesgo(dataRequest).Tables(0)
            Return DtTabla
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa función para capturar valores de tabla Parametros Generales | 17/05/18 
#End Region

#Region " - Funciones personalizadas - "
    'OAB 20091109
    'Public Function InsertarRating(ByVal nombre As String, ByVal comentario As String, ByVal dataRequest As DataSet) As Integer    'HDG OT 62087 Nro14-R23 20110223
    Public Function InsertarRating(ByVal nombre As String, ByVal comentario As String, ByVal Factor As String, ByVal dataRequest As DataSet) As Integer    'HDG OT 62087 Nro14-R23 20110223
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            Return oParametrosGeneralesDAM.InsertarRating(nombre, comentario, Factor, dataRequest)  'HDG OT 62087 Nro14-R23 20110223
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OAB 20091105
    Public Function EliminarRating(ByVal valor As String, ByVal dataRequest As DataSet) As DataSet
        Dim dsAux As DataSet
        Try
            Dim oParamGenDAM As New ParametrosGeneralesDAM
            dsAux = oParamGenDAM.EliminarRating(valor, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsAux
    End Function

    'OAB 20091005
    'HDG OT 62087 Nro14-R23 20110223
    'Public Function ModificarRating(ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal dataRequest As DataSet) As Integer
    Public Function ModificarRating(ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal factor As String, ByVal dataRequest As DataSet) As Integer
        Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
        Try
            'HDG OT 62087 Nro14-R23 20110223
            'Return oParametrosGeneralesDAM.ModificarRating(nombre, valor, comentario, dataRequest)
            Return oParametrosGeneralesDAM.ModificarRating(nombre, valor, comentario, factor, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Se implementó para que se pueda modificar el rating a traves de un archivo excel
    Public Function ActualizarRatingPorExcel(ByVal dtRating As DataTable, ByVal tipo As Integer, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim strCodigo As Boolean = False
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            strCodigo = oParametrosGenerales.ActualizarRatingPorExcel(dtRating, tipo, dataRequest, strmensaje)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function Eliminar(ByVal clasificacion As String, ByVal valor As String, ByVal dataRequest As DataSet) As Boolean
        Dim strCodigo As Boolean = False
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            strCodigo = oParametrosGenerales.Eliminar(clasificacion, valor)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarMotivoExtorno(ByVal Nombre As String, ByVal Valor As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            Dim dsAux As DataSet
            dsAux = oParametrosGenerales.SeleccionarMotivoExtorno(Nombre, Valor)
            Return dsAux
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertarMotivoExtorno(ByVal nombre As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            bolResult = oParametrosGenerales.InsertarMotivoExtorno(nombre, descripcion)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Actualizar(ByVal clasificacion As String, ByVal valor As String, ByVal nombre As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            bolResult = oParametrosGenerales.Actualizar(clasificacion, valor, nombre, descripcion)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarMedioTransmision(ByVal Nombre As String, ByVal Valor As String, ByVal tiporenta As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            Dim dsAux As DataSet
            dsAux = oParametrosGenerales.SeleccionarMedioTransmision(Nombre, Valor, tiporenta)
            Return dsAux
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertarMedioTransmision(ByVal codigo As String, ByVal descripcion As String, ByVal tiporenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            bolResult = oParametrosGenerales.InsertarMedioTransmision(codigo, descripcion, tiporenta)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ActualizarMedioTransmision(ByVal codigo As String, ByVal descripcion As String, ByVal tiporenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            bolResult = oParametrosGenerales.ActualizarMedioTransmision(codigo, descripcion, tiporenta)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarCategReporte(ByVal codCategReporte As String, ByVal codReporte As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesDAM
            Dim dsAux As DataSet
            dsAux = oParametrosGenerales.SeleccionarCategReporte(codCategReporte, codReporte)
            Return dsAux
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
End Class