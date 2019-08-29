Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
''' <summary>
''' Clase para el acceso de los datos para OrdenInversion tabla.
''' </summary>
Public Class OrdenPreOrdenInversionDAM
#Region "Variables"
    Dim DECIMAL_NULO As Decimal = 0.0
    Private oOrdenInversionRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    Private oTmpOperacionesEPUDetRow As TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow
    Private oTmpOperacionesEPURow As TmpOperacionesEPUBE.TmpOperacionesEPURow
    Private oTmpResumenOperacionesEPURow As TmpResumenOperacionesEPUBE.TmpResumenOperacionesEPURow
    Private Structure Procedimiento
        Public Const Reporte_InventarioForward_Fecha_Rango As String = "Reporte_InventarioForward_Fecha_Rango"
        Public Const Reporte_Gestion_CompCartera_Fecha_Rango As String = "Reporte_Gestion_CompCartera_Fecha_Rango"
        Public Const Reporte_VectorTipoCambio_Fecha_Rango As String = "Reporte_VectorTipoCambio_Fecha_Rango"
    End Structure
    Private Structure Parametro
        Public Const p_CodigoPortafolioSBS As String = "@p_CodigoPortafolioSBS"
        Public Const p_escenario As String = "@p_escenario"
        Public Const p_FechaDesde As String = "@p_FechaDesde"
        Public Const p_FechaHasta As String = "@p_FechaHasta"
    End Structure
#End Region
    Public Sub New()
    End Sub
    Private Sub AgregarParametros(ByRef db As Database, ByVal dbCommand As DbCommand, ByVal dataRequest As DataSet)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonicoReporte", DbType.String, IIf(oOrdenInversionRow.CodigoMnemonicoReporte = "", DBNull.Value, oOrdenInversionRow.CodigoMnemonicoReporte))
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, IIf(oOrdenInversionRow.FechaOperacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.FechaOperacion))
        db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, IIf(oOrdenInversionRow.FechaLiquidacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.FechaLiquidacion))
        db.AddInParameter(dbCommand, "@p_MontoNominalOrdenado", DbType.Decimal, IIf(oOrdenInversionRow.MontoNominalOrdenado = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoNominalOrdenado))
        db.AddInParameter(dbCommand, "@p_MontoNominalOperacion", DbType.Decimal, IIf(oOrdenInversionRow.MontoNominalOperacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoNominalOperacion))
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, IIf(oOrdenInversionRow.CodigoTipoCupon = "", DBNull.Value, oOrdenInversionRow.CodigoTipoCupon))
        db.AddInParameter(dbCommand, "@p_YTM", DbType.Decimal, IIf(oOrdenInversionRow.YTM = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.YTM))
        db.AddInParameter(dbCommand, "@p_PrecioNegociacionLimpio", DbType.Decimal, IIf(oOrdenInversionRow.PrecioNegociacionLimpio = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PrecioNegociacionLimpio))
        db.AddInParameter(dbCommand, "@p_PrecioNegociacionSucio", DbType.Decimal, IIf(oOrdenInversionRow.PrecioNegociacionSucio = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PrecioNegociacionSucio))
        db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, IIf(oOrdenInversionRow.MontoOperacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoOperacion))
        db.AddInParameter(dbCommand, "@p_Observacion", DbType.String, IIf(oOrdenInversionRow.Observacion = "", DBNull.Value, oOrdenInversionRow.Observacion))
        db.AddInParameter(dbCommand, "@p_Plazo", DbType.Decimal, IIf(oOrdenInversionRow.Plazo = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.Plazo))
        db.AddInParameter(dbCommand, "@p_PTasa", DbType.Decimal, IIf(oOrdenInversionRow.PTasa = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PTasa))
        db.AddInParameter(dbCommand, "@p_InteresAcumulado", DbType.Decimal, IIf(oOrdenInversionRow.InteresAcumulado = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.InteresAcumulado))
        db.AddInParameter(dbCommand, "@p_InteresCastigado", DbType.Decimal, IIf(oOrdenInversionRow.InteresCastigado = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.InteresCastigado))
        db.AddInParameter(dbCommand, "@p_TasaCastigo", DbType.Decimal, IIf(oOrdenInversionRow.TasaCastigo = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.TasaCastigo))
        db.AddInParameter(dbCommand, "@p_MontoPreCancelar", DbType.Decimal, IIf(oOrdenInversionRow.MontoPreCancelar = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoPreCancelar))
        db.AddInParameter(dbCommand, "@p_InteresCorridoNegociacion", DbType.Decimal, IIf(oOrdenInversionRow.InteresCorridoNegociacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.InteresCorridoNegociacion))
        db.AddInParameter(dbCommand, "@p_PorcentajeAcciones", DbType.Decimal, IIf(oOrdenInversionRow.PorcentajeAcciones = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PorcentajeAcciones))
        db.AddInParameter(dbCommand, "@p_PorcentajeDolares", DbType.Decimal, IIf(oOrdenInversionRow.PorcentajeDolares = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PorcentajeDolares))
        db.AddInParameter(dbCommand, "@p_PorcentajeBonos", DbType.Decimal, IIf(oOrdenInversionRow.PorcentajeBonos = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PorcentajeBonos))
        db.AddInParameter(dbCommand, "@p_CantidadValor", DbType.Decimal, IIf(oOrdenInversionRow.CantidadValor = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.CantidadValor))
        db.AddInParameter(dbCommand, "@p_MontoContado", DbType.Decimal, IIf(oOrdenInversionRow.MontoContado = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoContado))
        db.AddInParameter(dbCommand, "@p_MontoPlazo", DbType.Decimal, IIf(oOrdenInversionRow.MontoPlazo = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoPlazo))
        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oOrdenInversionRow.Precio = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.Precio))
        db.AddInParameter(dbCommand, "@p_TipoCambio", DbType.Decimal, IIf(oOrdenInversionRow.TipoCambio = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.TipoCambio))
        db.AddInParameter(dbCommand, "@p_MontoOrigen", DbType.Decimal, IIf(oOrdenInversionRow.MontoOrigen = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoOrigen))
        db.AddInParameter(dbCommand, "@p_MontoDestino", DbType.Decimal, IIf(oOrdenInversionRow.MontoDestino = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoDestino))
        db.AddInParameter(dbCommand, "@p_TipoCobertura", DbType.String, IIf(oOrdenInversionRow.TipoCobertura = "", DBNull.Value, oOrdenInversionRow.TipoCobertura))
        db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, IIf(oOrdenInversionRow.FechaPago = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.FechaPago))
        db.AddInParameter(dbCommand, "@p_ContadoSoles", DbType.Decimal, IIf(oOrdenInversionRow.ContadoSoles = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.ContadoSoles))
        db.AddInParameter(dbCommand, "@p_TipoCambioSpot", DbType.Decimal, IIf(oOrdenInversionRow.TipoCambioSpot = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.TipoCambioSpot))
        db.AddInParameter(dbCommand, "@p_ContadoDolares", DbType.Decimal, IIf(oOrdenInversionRow.ContadoDolares = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.ContadoDolares))
        db.AddInParameter(dbCommand, "@p_PlazoSoles", DbType.Decimal, IIf(oOrdenInversionRow.PlazoSoles = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PlazoSoles))
        db.AddInParameter(dbCommand, "@p_TipoCambioForw", DbType.Decimal, IIf(oOrdenInversionRow.TipoCambioForw = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.TipoCambioForw))
        db.AddInParameter(dbCommand, "@p_PlazoDolares", DbType.Decimal, IIf(oOrdenInversionRow.PlazoDolares = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PlazoDolares))
        db.AddInParameter(dbCommand, "@p_PorcentajeRendimiento", DbType.Decimal, IIf(oOrdenInversionRow.PorcentajeRendimiento = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PorcentajeRendimiento))
        db.AddInParameter(dbCommand, "@p_TipoCambioFuturo", DbType.Decimal, IIf(oOrdenInversionRow.TipoCambioFuturo = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.TipoCambioFuturo))
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, IIf(oOrdenInversionRow.CodigoMoneda = "", DBNull.Value, oOrdenInversionRow.CodigoMoneda))
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, IIf(oOrdenInversionRow.CodigoMonedaDestino = "", DBNull.Value, oOrdenInversionRow.CodigoMonedaDestino))
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, IIf(oOrdenInversionRow.CodigoPreOrden = "", DBNull.Value, oOrdenInversionRow.CodigoPreOrden))
        db.AddInParameter(dbCommand, "@p_TotalComisiones", DbType.Decimal, IIf(oOrdenInversionRow.TotalComisiones = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.TotalComisiones))
        db.AddInParameter(dbCommand, "@p_PrecioPromedio", DbType.Decimal, IIf(oOrdenInversionRow.PrecioPromedio = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.PrecioPromedio))
        db.AddInParameter(dbCommand, "@p_MontoNetoOperacion", DbType.Decimal, IIf(oOrdenInversionRow.MontoNetoOperacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.MontoNetoOperacion))
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, IIf(oOrdenInversionRow.Estado = "", DBNull.Value, oOrdenInversionRow.Estado))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, IIf(oOrdenInversionRow.Situacion = "", DBNull.Value, oOrdenInversionRow.Situacion))
        db.AddInParameter(dbCommand, "@p_Delibery", DbType.String, IIf(oOrdenInversionRow.Delibery = "", DBNull.Value, oOrdenInversionRow.Delibery))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, IIf(oOrdenInversionRow.CodigoTercero = "", DBNull.Value, oOrdenInversionRow.CodigoTercero))
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, IIf(oOrdenInversionRow.CodigoPortafolioSBS = "", DBNull.Value, oOrdenInversionRow.CodigoPortafolioSBS))
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, IIf(oOrdenInversionRow.CodigoContacto = "", DBNull.Value, oOrdenInversionRow.CodigoContacto))
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, IIf(oOrdenInversionRow.CodigoOperacion = "", DBNull.Value, oOrdenInversionRow.CodigoOperacion))
        db.AddInParameter(dbCommand, "@p_CantidadOrdenado", DbType.Decimal, IIf(oOrdenInversionRow.CantidadOrdenado = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.CantidadOrdenado))
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, IIf(oOrdenInversionRow.CantidadOperacion = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.CantidadOperacion))
        db.AddInParameter(dbCommand, "@p_NumeroPoliza", DbType.String, IIf(oOrdenInversionRow.NumeroPoliza = "", DBNull.Value, oOrdenInversionRow.NumeroPoliza))
        db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, IIf(oOrdenInversionRow.CodigoUsuario = "", DBNull.Value, oOrdenInversionRow.CodigoUsuario))
        db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, IIf(oOrdenInversionRow.HoraOperacion = "", DBNull.Value, oOrdenInversionRow.HoraOperacion))
        db.AddInParameter(dbCommand, "@p_CodigoGestor", DbType.String, IIf(oOrdenInversionRow.CodigoGestor = "", DBNull.Value, oOrdenInversionRow.CodigoGestor))
        db.AddInParameter(dbCommand, "@p_CodigoMonedaOrigen", DbType.String, IIf(oOrdenInversionRow.CodigoMonedaOrigen = "", DBNull.Value, oOrdenInversionRow.CodigoMonedaOrigen))
        db.AddInParameter(dbCommand, "@p_Diferencial", DbType.Decimal, IIf(oOrdenInversionRow.Diferencial = DECIMAL_NULO, DBNull.Value, oOrdenInversionRow.Diferencial))
        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, IIf(oOrdenInversionRow.CodigoMotivo = "", DBNull.Value, oOrdenInversionRow.CodigoMotivo))
        db.AddInParameter(dbCommand, "@p_MontoCancelar", DbType.Decimal, IIf(oOrdenInversionRow.MontoCancelar = 0, DBNull.Value, oOrdenInversionRow.MontoCancelar))
        db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, IIf(oOrdenInversionRow.CodigoTipoTitulo = "", DBNull.Value, oOrdenInversionRow.CodigoTipoTitulo))
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, IIf(oOrdenInversionRow.CategoriaInstrumento = "", DBNull.Value, oOrdenInversionRow.CategoriaInstrumento))
        db.AddInParameter(dbCommand, "@p_FechaContrato", DbType.Decimal, IIf(oOrdenInversionRow.FechaContrato = 0, DBNull.Value, oOrdenInversionRow.FechaContrato))
        db.AddInParameter(dbCommand, "@p_OR_CantidadRV", DbType.Decimal, IIf(oOrdenInversionRow.OR_CantidadRV = 0, DBNull.Value, oOrdenInversionRow.OR_CantidadRV))
        db.AddInParameter(dbCommand, "@p_OR_CantidadNominal", DbType.Decimal, IIf(oOrdenInversionRow.OR_CantidadNominal = 0, DBNull.Value, oOrdenInversionRow.OR_CantidadNominal))
        db.AddInParameter(dbCommand, "@p_OR_CantidadRF", DbType.Decimal, IIf(oOrdenInversionRow.OR_CantidadRF = 0, DBNull.Value, oOrdenInversionRow.OR_CantidadRF))
        db.AddInParameter(dbCommand, "@p_TasaPorcentaje", DbType.Decimal, IIf(oOrdenInversionRow.TasaPorcentaje = 0, DBNull.Value, oOrdenInversionRow.TasaPorcentaje))
        db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, IIf(oOrdenInversionRow.TipoFondo = "", DBNull.Value, oOrdenInversionRow.TipoFondo))
        db.AddInParameter(dbCommand, "@p_FechaTrato", DbType.Decimal, IIf(oOrdenInversionRow.FechaTrato = 0, DBNull.Value, oOrdenInversionRow.FechaTrato))
        db.AddInParameter(dbCommand, "@p_IsTemporal", DbType.String, IIf(oOrdenInversionRow.IsTemporal = "", DBNull.Value, oOrdenInversionRow.IsTemporal))
        db.AddInParameter(dbCommand, "@p_PrecioCalculado", DbType.Decimal, IIf(oOrdenInversionRow.PrecioCalculado = 0, DBNull.Value, oOrdenInversionRow.PrecioCalculado))
        db.AddInParameter(dbCommand, "@p_InteresCorrido", DbType.Decimal, IIf(oOrdenInversionRow.InteresCorrido = 0, DBNull.Value, oOrdenInversionRow.InteresCorrido))
        db.AddInParameter(dbCommand, "@p_Fixing", DbType.Decimal, IIf(oOrdenInversionRow.Fixing = 0, DBNull.Value, oOrdenInversionRow.Fixing))
        db.AddInParameter(dbCommand, "@p_GrupoIntermediario", DbType.String, IIf(oOrdenInversionRow.GrupoIntermediario = "", DBNull.Value, oOrdenInversionRow.GrupoIntermediario))
        'RGF 20090116
        If oOrdenInversionRow.IsAfectaFlujoCajaNull Then
            db.AddInParameter(dbCommand, "@p_AfectaFlujoCaja", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_AfectaFlujoCaja", DbType.String, oOrdenInversionRow.AfectaFlujoCaja)
        End If

        'LETV 20090310
        If oOrdenInversionRow.IsPlazaNull Then
            db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, oOrdenInversionRow.Plaza)
        End If

        'RGF 20090327
        If oOrdenInversionRow.IsMontoPrimaNull Then
            db.AddInParameter(dbCommand, "@p_MontoPrima", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_MontoPrima", DbType.Decimal, oOrdenInversionRow.MontoPrima)
        End If

        'LETV 20090401
        If oOrdenInversionRow.IsTipoTramoNull Then
            db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oOrdenInversionRow.TipoTramo)
        End If

        'HDG OT 61566 Nro5-R12 20101122
        If oOrdenInversionRow.IsRenovacionNull Then
            db.AddInParameter(dbCommand, "@p_Renovacion", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_Renovacion", DbType.String, IIf(oOrdenInversionRow.Renovacion = "", DBNull.Value, oOrdenInversionRow.Renovacion))
        End If

        'HDG OT 61573 20101125
        If oOrdenInversionRow.IsTipoMonedaForwNull Then
            db.AddInParameter(dbCommand, "@p_TipoMonedaForw", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TipoMonedaForw", DbType.String, IIf(oOrdenInversionRow.TipoMonedaForw = "", DBNull.Value, oOrdenInversionRow.TipoMonedaForw))
        End If

        'HDG OT 62255 20110214
        If oOrdenInversionRow.IsRegulaSBSNull Then
            db.AddInParameter(dbCommand, "@p_RegulaSBS", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_RegulaSBS", DbType.String, IIf(oOrdenInversionRow.RegulaSBS = "", DBNull.Value, oOrdenInversionRow.RegulaSBS))
        End If

        'ini HDG OT 64291 20111128
        If oOrdenInversionRow.IsMedioNegociacionNull Then
            db.AddInParameter(dbCommand, "@p_MedioNegociacion", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_MedioNegociacion", DbType.String, IIf(oOrdenInversionRow.MedioNegociacion = "", DBNull.Value, oOrdenInversionRow.MedioNegociacion))
        End If

        If oOrdenInversionRow.IsHoraEjecucionNull Then
            db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, IIf(oOrdenInversionRow.HoraEjecucion = "", DBNull.Value, oOrdenInversionRow.HoraEjecucion))
        End If
        'ini HDG OT 64291 20111128
        'INI JHC REQ 66056: Implementacion Futuros
        If oOrdenInversionRow.IsTipoCondicionNull Then
            db.AddInParameter(dbCommand, "@p_TipoCondicion", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TipoCondicion", DbType.String, oOrdenInversionRow.TipoCondicion)
        End If
        If oOrdenInversionRow.IsVencimientoAnoNull Then
            db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, oOrdenInversionRow.VencimientoAno)
        End If
        If oOrdenInversionRow.IsVencimientoMesNull Then
            db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, oOrdenInversionRow.VencimientoMes)
        End If
        If oOrdenInversionRow.IsTipoValorizacionNull Then
            db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, oOrdenInversionRow.TipoValorizacion)
        End If

        db.AddInParameter(dbCommand, "@p_TirNeta", DbType.Decimal, oOrdenInversionRow.TirNeta)
        'FIN JHC REQ 66056: Implementacion Futuros

        If oOrdenInversionRow.IsObservacionCartaNull Then
            db.AddInParameter(dbCommand, "@p_ObservacionCarta", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_ObservacionCarta", DbType.String, oOrdenInversionRow.ObservacionCarta)
        End If

    End Sub
    Public Function InsertarOI(ByVal objOI As OrdenPreOrdenInversionBE, ByVal strPagina As String, ByVal dataRequest As DataSet) As String
        Dim strCodigoOI As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_InsertarOI")
        oOrdenInversionRow = CType(objOI.OrdenPreOrdenInversion.Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        AgregarParametros(db, dbCommand, dataRequest)

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, IIf(oOrdenInversionRow.CodigoISIN = String.Empty, DBNull.Value, oOrdenInversionRow.CodigoISIN))
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, IIf(oOrdenInversionRow.CodigoSBS = String.Empty, DBNull.Value, oOrdenInversionRow.CodigoSBS))
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, IIf(oOrdenInversionRow.CodigoMnemonico = String.Empty, DBNull.Value, oOrdenInversionRow.CodigoMnemonico))
        db.AddInParameter(dbCommand, "@p_EventoFuturo", DbType.Decimal, oOrdenInversionRow.EventoFuturo) 'CMB OT 64769 20120320
        db.AddInParameter(dbCommand, "@p_Pagina", DbType.String, strPagina)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        'LETV 20090305
        If oOrdenInversionRow.IsOrdenGeneraNull Then
            db.AddInParameter(dbCommand, "@p_OrdenGenera", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_OrdenGenera", DbType.String, oOrdenInversionRow.OrdenGenera)
        End If
        'LETV 20090706
        db.AddInParameter(dbCommand, "@p_Ficticia", DbType.String, IIf(oOrdenInversionRow.Ficticia = String.Empty, DBNull.Value, oOrdenInversionRow.Ficticia))
        strCodigoOI = db.ExecuteScalar(dbCommand)
        Return strCodigoOI
    End Function
    Public Sub ModificarOI(ByVal objOI As OrdenPreOrdenInversionBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ModificarOI")
            oOrdenInversionRow = CType(objOI.OrdenPreOrdenInversion.Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
            AgregarParametros(db, dbCommand, dataRequest)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, oOrdenInversionRow.CodigoOrden)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            'RGF 20080929
            If oOrdenInversionRow.IsCodigoMotivoCambioNull Then
                db.AddInParameter(dbCommand, "@p_CodigoMotivoCambio", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_CodigoMotivoCambio", DbType.String, oOrdenInversionRow.CodigoMotivoCambio)
            End If
            If oOrdenInversionRow.IsIndicaCambioNull Then
                db.AddInParameter(dbCommand, "@p_IndicaCambio", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_IndicaCambio", DbType.String, oOrdenInversionRow.IndicaCambio)
            End If
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function EliminarOI(ByVal codigoOrden As String, ByVal strFondo As String, ByVal CodigoMotivoCambio As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarOI")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_PortafolioSBS", DbType.String, strFondo)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        'RGF 20080929
        db.AddInParameter(dbCommand, "@p_CodigoMotivoCambio", DbType.String, CodigoMotivoCambio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'creado por DB 02/03/2009
    Public Sub CorreccionDepositoPlazo(ByVal strCodigoOrden As String, ByVal strCodigoFondo As String, ByVal strDiasPlazo As String, ByVal decTasaPorc As Decimal, ByVal strCodigoSBS As String, _
        ByVal decMontoOperacion As Decimal, ByVal strCodigoTipoCupon As String, ByVal decFechaContrato As Decimal, ByVal dataRequest As DataSet, ByVal CodigoTercero As String,
        ByVal CodigoNemonico As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CorreccionDepositosPlazo_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoFondo)
        db.AddInParameter(dbCommand, "@p_DiasPlazo", DbType.String, strDiasPlazo)
        db.AddInParameter(dbCommand, "@p_TasaPorc", DbType.Decimal, decTasaPorc)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, strCodigoSBS)
        db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.String, decMontoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, strCodigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_FechaContrato", DbType.Decimal, decFechaContrato)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@P_CodigoNemonico", DbType.String, CodigoNemonico)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function ExtornaOrdenInversionConfirmada(ByVal codigoPortafolio As String, ByVal numeroOrden As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_ExtornaOIConfirmada")
        db.AddInParameter(dbCommand, "@nvcCodigoOrden", DbType.String, numeroOrden)
        db.AddInParameter(dbCommand, "@nvcPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega parámetro de salida para comprobar estado del procesamiento del SP usp_ExtornaOIConfirmada | 11/07/18 
        db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
        db.ExecuteNonQuery(dbCommand)

        Return CType(db.GetParameterValue(dbCommand, "@p_Result"), Integer)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega parámetro de salida para comprobar estado del procesamiento del SP usp_ExtornaOIConfirmada | 11/07/18 
    End Function
    Public Function ActualizarCustodioCarteraKardex(ByVal strPagina As String, ByVal strCodigoOI As String, ByVal strCodigoCustodio As String, ByVal decMonto As Decimal, ByVal strAccion As String, ByVal strCategoriaInstrumento As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ActualizaCustodioCarteraKardex")

        db.AddInParameter(dbCommand, "@p_CodigoOrdenPreOrden", DbType.String, strCodigoOI)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, strCodigoCustodio)
        db.AddInParameter(dbCommand, "@p_NumUnidades", DbType.Decimal, decMonto)
        db.AddInParameter(dbCommand, "@p_ClasificacionInstrumento", DbType.String, strCategoriaInstrumento)
        db.AddInParameter(dbCommand, "@p_Accion", DbType.String, strAccion)
        db.AddInParameter(dbCommand, "@p_Pagina", DbType.String, strPagina)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function GetItemInstrumentoEstructurado(ByVal codigoPortafolio As String, ByVal codigoCustodio As String, _
        ByVal codigoNemonico As String, ByVal fecha As Integer) As DataSet

        Dim ds As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetItemsInstrumentoEstructurado")


        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@nvcCodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@nvcCodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@nvcFecha", DbType.Int32, fecha)

        ds = db.ExecuteDataSet(dbCommand)
        Return ds
    End Function
    Public Function ListarOrdenesInversionPorCodigoOrden(ByVal codigoOrden As String, ByVal fondo As String, ByVal codPortafolioMultifondo As String, Optional ByVal LlamadoForward As String = "N") As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        Dim dsOI As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarPorCodigoOrden")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        db.AddInParameter(dbCommand, "@p_LlamadoForward", DbType.String, LlamadoForward)
        dsOI = db.ExecuteDataSet(dbCommand)
        For Each dr As DataRow In dsOI.Tables(0).Rows
            oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
            InicializarOrdenInversion(oRow)
            'If fondo <> codPortafolioMultifondo Then
            oRow.CodigoOrden = IIf(dr("CodigoOrden") Is DBNull.Value, "", dr("CodigoOrden"))
            'Else
            'oRow.CodigoOrden = IIf(dr("CodigoPreOrden") Is DBNull.Value, "", dr("CodigoPreOrden"))
            'End If
            oRow.FechaOperacion = IIf(dr("FechaOperacion") Is DBNull.Value, 0, dr("FechaOperacion"))
            oRow.FechaLiquidacion = IIf(dr("FechaLiquidacion") Is DBNull.Value, 0, dr("FechaLiquidacion"))
            oRow.MontoNominalOrdenado = IIf(dr("MontoNominalOrdenado") Is DBNull.Value, 0, dr("MontoNominalOrdenado"))
            oRow.MontoNominalOperacion = IIf(dr("MontoNominalOperacion") Is DBNull.Value, 0, dr("MontoNominalOperacion"))
            oRow.CodigoTipoCupon = IIf(dr("CodigoTipoCupon") Is DBNull.Value, "", dr("CodigoTipoCupon"))
            oRow.YTM = IIf(dr("YTM") Is DBNull.Value, 0, dr("YTM"))
            oRow.PrecioNegociacionLimpio = IIf(dr("PrecioNegociacionLimpio") Is DBNull.Value, 0, dr("PrecioNegociacionLimpio"))
            oRow.PrecioNegociacionSucio = IIf(dr("PrecioNegociacionSucio") Is DBNull.Value, 0, dr("PrecioNegociacionSucio"))
            oRow.MontoOperacion = IIf(dr("MontoOperacion") Is DBNull.Value, 0, dr("MontoOperacion"))
            oRow.Observacion = IIf(dr("Observacion") Is DBNull.Value, "", dr("Observacion"))
            oRow.Plazo = IIf(dr("Plazo") Is DBNull.Value, 0, dr("Plazo"))
            oRow.PTasa = IIf(dr("PTasa") Is DBNull.Value, 0, dr("Ptasa"))
            oRow.InteresAcumulado = IIf(dr("InteresAcumulado") Is DBNull.Value, 0, dr("InteresAcumulado"))
            oRow.InteresCastigado = IIf(dr("InteresCastigado") Is DBNull.Value, 0, dr("InteresCastigado"))
            oRow.TasaCastigo = IIf(dr("TasaCastigo") Is DBNull.Value, 0, dr("TasaCastigo"))
            oRow.MontoPreCancelar = IIf(dr("MontoPreCancelar") Is DBNull.Value, 0, dr("MontoPreCancelar"))
            oRow.InteresCorridoNegociacion = IIf(dr("InteresCorridoNegociacion") Is DBNull.Value, 0, dr("InteresCorridoNegociacion"))
            oRow.PorcentajeAcciones = IIf(dr("PorcentajeAcciones") Is DBNull.Value, 0, dr("PorcentajeAcciones"))
            oRow.PorcentajeDolares = IIf(dr("PorcentajeDolares") Is DBNull.Value, 0, dr("PorcentajeDolares"))
            oRow.PorcentajeBonos = IIf(dr("PorcentajeBonos") Is DBNull.Value, 0, dr("PorcentajeBonos"))
            oRow.CantidadValor = IIf(dr("CantidadValor") Is DBNull.Value, 0, dr("CantidadValor"))
            oRow.MontoContado = IIf(dr("MontoContado") Is DBNull.Value, 0, dr("MontoContado"))
            oRow.MontoPlazo = IIf(dr("MontoPlazo") Is DBNull.Value, 0, dr("MontoPlazo"))
            oRow.Precio = IIf(dr("Precio") Is DBNull.Value, 0, dr("Precio"))
            oRow.TipoCambio = IIf(dr("TipoCambio") Is DBNull.Value, 0, dr("TipoCambio"))
            oRow.MontoOrigen = IIf(dr("MontoOrigen") Is DBNull.Value, 0, dr("MontoOrigen"))
            oRow.MontoDestino = IIf(dr("MontoDestino") Is DBNull.Value, 0, dr("MontoDestino"))
            oRow.TipoCobertura = IIf(dr("TipoCobertura") Is DBNull.Value, "", dr("TipoCobertura"))
            oRow.FechaPago = IIf(dr("FechaPago") Is DBNull.Value, 0, dr("FechaPago"))
            oRow.ContadoSoles = IIf(dr("ContadoSoles") Is DBNull.Value, 0, dr("ContadoSoles"))
            oRow.TipoCambioSpot = IIf(dr("TipoCambioSpot") Is DBNull.Value, 0, dr("TipoCambioSpot"))
            oRow.ContadoDolares = IIf(dr("ContadoDolares") Is DBNull.Value, 0, dr("ContadoDolares"))
            oRow.PlazoSoles = IIf(dr("PlazoSoles") Is DBNull.Value, 0, dr("PlazoSoles"))
            oRow.TipoCambioForw = IIf(dr("TipoCambioForw") Is DBNull.Value, 0, dr("TipoCambioForw"))
            oRow.PlazoDolares = IIf(dr("PlazoDolares") Is DBNull.Value, 0, dr("PlazoDolares"))
            oRow.PorcentajeRendimiento = IIf(dr("PorcentajeRendimiento") Is DBNull.Value, 0, dr("PorcentajeRendimiento"))
            oRow.TipoCambioFuturo = IIf(dr("TipoCambioFuturo") Is DBNull.Value, 0, dr("TipoCambioFuturo"))
            oRow.CodigoMoneda = IIf(dr("CodigoMoneda") Is DBNull.Value, "", dr("CodigoMoneda"))
            oRow.CodigoMonedaDestino = IIf(dr("CodigoMonedaDestino") Is DBNull.Value, "", dr("CodigoMonedaDestino"))
            oRow.CodigoPreOrden = IIf(dr("CodigoPreOrden") Is DBNull.Value, "", dr("CodigoPreOrden"))
            oRow.TotalComisiones = IIf(dr("TotalComisiones") Is DBNull.Value, 0, dr("TotalComisiones"))
            oRow.PrecioPromedio = IIf(dr("PrecioPromedio") Is DBNull.Value, 0, dr("PrecioPromedio"))
            oRow.MontoNetoOperacion = IIf(dr("MontoNetoOperacion") Is DBNull.Value, 0, dr("MontoNetoOperacion"))
            oRow.Estado = IIf(dr("Estado") Is DBNull.Value, "", dr("Estado"))
            oRow.Delibery = IIf(dr("Delibery") Is DBNull.Value, "", dr("Delibery"))
            oRow.CodigoISIN = IIf(dr("CodigoISIN") Is DBNull.Value, "", dr("CodigoISIN"))
            oRow.CodigoTercero = IIf(dr("CodigoTercero") Is DBNull.Value, "", dr("CodigoTercero"))
            oRow.AfectaFlujoCaja = IIf(dr("AfectaFlujoCaja") Is DBNull.Value, "", dr("AfectaFlujoCaja")) 'RGF 20090116
            oRow.CodigoContacto = IIf(dr("CodigoContacto") Is DBNull.Value, "", dr("CodigoContacto"))
            oRow.CodigoPortafolioSBS = IIf(dr("CodigoPortafolioSBS") Is DBNull.Value, "", dr("CodigoPortafolioSBS"))
            oRow.CodigoMnemonico = IIf(dr("CodigoMnemonico") Is DBNull.Value, "", dr("CodigoMnemonico"))
            oRow.CodigoOperacion = IIf(dr("CodigoOperacion") Is DBNull.Value, "", dr("CodigoOperacion"))
            oRow.CodigoSBS = IIf(dr("CodigoSBS") Is DBNull.Value, "", dr("CodigoSBS"))
            oRow.CantidadOperacion = IIf(dr("CantidadOperacion") Is DBNull.Value, 0, dr("CantidadOperacion"))
            oRow.CantidadOrdenado = IIf(dr("CantidadOrdenado") Is DBNull.Value, 0, dr("CantidadOrdenado"))
            oRow.NumeroPoliza = IIf(dr("NumeroPoliza") Is DBNull.Value, "", dr("NumeroPoliza"))
            oRow.CodigoUsuario = IIf(dr("CodigoUsuario") Is DBNull.Value, "", dr("CodigoUsuario"))
            oRow.HoraOperacion = IIf(dr("HoraOperacion") Is DBNull.Value, "", dr("HoraOperacion"))
            oRow.CodigoGestor = IIf(dr("CodigoGestor") Is DBNull.Value, "", dr("CodigoGestor"))
            oRow.CodigoMonedaOrigen = IIf(dr("CodigoMonedaOrigen") Is DBNull.Value, "", dr("CodigoMonedaOrigen"))
            oRow.Diferencial = IIf(dr("Diferencial") Is DBNull.Value, 0, dr("Diferencial"))
            oRow.CodigoMotivo = IIf(dr("CodigoMotivo") Is DBNull.Value, "", dr("CodigoMotivo"))
            oRow.MontoCancelar = IIf(dr("MontoCancelar") Is DBNull.Value, 0, dr("MontoCancelar"))
            oRow.CodigoTipoTitulo = IIf(dr("CodigoTipoTitulo") Is DBNull.Value, "", dr("CodigoTipoTitulo"))
            oRow.CategoriaInstrumento = IIf(dr("CategoriaInstrumento") Is DBNull.Value, "", dr("CategoriaInstrumento"))
            oRow.CodigoMnemonicoReporte = IIf(dr("CodigoMnemonicoReporte") Is DBNull.Value, "", dr("CodigoMnemonicoReporte"))
            oRow.FechaContrato = IIf(dr("FechaContrato") Is DBNull.Value, 0, dr("FechaContrato"))
            oRow.OR_CantidadRV = IIf(dr("OR_CantidadRV") Is DBNull.Value, 0, dr("OR_CantidadRV"))
            oRow.OR_CantidadNominal = IIf(dr("OR_CantidadNominal") Is DBNull.Value, 0, dr("OR_CantidadNominal"))
            oRow.OR_CantidadRF = IIf(dr("OR_CantidadRF") Is DBNull.Value, 0, dr("OR_CantidadRF"))
            oRow.TasaPorcentaje = IIf(dr("TasaPorcentaje") Is DBNull.Value, 0, dr("TasaPorcentaje"))
            oRow.FechaTrato = IIf(dr("FechaTrato") Is DBNull.Value, 0, dr("FechaTrato"))
            oRow.TipoFondo = IIf(dr("TipoFondo") Is DBNull.Value, "", dr("TipoFondo"))
            oRow.IsTemporal = IIf(dr("IsTemporal") Is DBNull.Value, "", dr("IsTemporal"))
            oRow.PrecioCalculado = IIf(dr("PrecioCalculado") Is DBNull.Value, 0, dr("PrecioCalculado"))
            oRow.InteresCorrido = IIf(dr("InteresCorrido") Is DBNull.Value, 0, dr("InteresCorrido"))
            oRow.Plaza = IIf(dr("CodigoPlaza") Is DBNull.Value, "", dr("CodigoPlaza"))
            oRow.Fixing = IIf(dr("Fixing") Is DBNull.Value, 0, dr("Fixing"))
            oRow.GrupoIntermediario = IIf(dr("GrupoIntermediario") Is DBNull.Value, "", dr("GrupoIntermediario"))
            oRow.MontoPrima = IIf(dr("MontoPrima") Is DBNull.Value, 0, dr("MontoPrima")) 'RGF 20090401
            oRow.TipoTramo = IIf(dr("TipoTramo") Is DBNull.Value, "", dr("TipoTramo")) 'LETV 20090402
            oRow.CodigoMotivoCambio = IIf(dr("CodigoMotivoCambio") Is DBNull.Value, "", dr("CodigoMotivoCambio")) 'LETV 20090706
            oRow.Ficticia = IIf(dr("Ficticia") Is DBNull.Value, "", dr("Ficticia")) 'LETV 20090706
            oRow.RegulaSBS = IIf(dr("RegulaSBS") Is DBNull.Value, "", dr("RegulaSBS"))
            oRow.EventoFuturo = IIf(dr("EventoFuturo") Is DBNull.Value, 0, dr("EventoFuturo")) 'CMB OT 64769 20120321
            'ini HDG 20120110
            If dr("Fixing").ToString = "" Then
                oRow.Fixing = 0
            Else
                oRow.Fixing = dr("Fixing")
            End If
            If dr("Renovacion").ToString = "" Then
                oRow.Renovacion = ""
            Else
                oRow.Renovacion = dr("Renovacion")
            End If
            If dr("TipoMonedaForw").ToString = "" Then
                oRow.TipoMonedaForw = ""
            Else
                oRow.TipoMonedaForw = dr("TipoMonedaForw")
            End If
            If dr("HoraEjecucion").ToString = "" Then
                oRow.HoraEjecucion = ""
            Else
                oRow.HoraEjecucion = dr("HoraEjecucion")
            End If
            If dr("MedioNegociacion").ToString = "" Then
                oRow.MedioNegociacion = ""
            Else
                oRow.MedioNegociacion = dr("MedioNegociacion")
            End If
            'fin HDG 20120110

            'INI JHC REQ 66056: Implementacion Futuros
            If dr("VencimientoAno").ToString = "" Then
                oRow.VencimientoAno = ""
            Else
                oRow.VencimientoAno = dr("VencimientoAno")
            End If
            If dr("VencimientoMes").ToString = "" Then
                oRow.VencimientoMes = ""
            Else
                oRow.VencimientoMes = dr("VencimientoMes")
            End If
            If dr("TipoCondicion").ToString = "" Then
                oRow.TipoCondicion = ""
            Else
                oRow.TipoCondicion = dr("TipoCondicion")
            End If
            'FIN JHC REQ 66056: Implementacion Futuros

            ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-09-19 | Nuevos Campos
            oRow.TipoValorizacion = IIf(dr("TipoValorizacion") Is DBNull.Value, "", dr("TipoValorizacion").ToString())
            oRow.TirNeta = IIf(dr("TirNeta") Is DBNull.Value, 0, dr("TirNeta"))
            ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-09-19 | Nuevos Campos

            If dr("ObservacionCarta").ToString = "" Then
                oRow.ObservacionCarta = ""
            Else
                oRow.ObservacionCarta = dr("ObservacionCarta")
            End If

            oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
            oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Next
        Return oOrdenInversionBE
    End Function

    Public Function ListarOrdenesInversionPorCodigoOrden(ByVal codigoOrden As String, ByVal codigoPortafolio As String) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarPorCodigoOrden")
        Try
            db.AddInParameter(comando, "@p_CodigoOrden", DbType.String, codigoOrden)
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            db = Nothing
            Throw
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function ListarOrdenesCabecera(ByVal CategoriaInstrumento As String, ByVal FechaOperacion As Decimal, ByVal CodigoSBS As String, ByVal CodigoISIN As String, ByVal CodigoMnemonico As String, ByVal Portafolio As String, ByVal Operacion As String, ByVal Moneda As String, ByVal accion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenesInversion_BuscarNegociacion")
        db.AddInParameter(dbCommand, "@p_Accion", DbType.String, accion)
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, CategoriaInstrumento)
        db.AddInParameter(dbCommand, "@p_FechaNegociacion", DbType.String, FechaOperacion)
        If CodigoSBS.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, CodigoSBS)
        End If
        If CodigoISIN.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, CodigoISIN)
        End If
        If CodigoMnemonico.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, CodigoMnemonico)
        End If
        If Portafolio.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, Portafolio)
        End If
        If Operacion.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoOperacion", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoOperacion", DbType.String, Operacion)
        End If
        If Moneda.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, Moneda)
        End If

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarPreOrdenesCabecera(ByVal CategoriaInstrumento As String, ByVal FechaOperacion As Decimal, ByVal CodigoSBS As String, ByVal CodigoISIN As String, ByVal CodigoMnemonico As String, ByVal Portafolio As String, ByVal Operacion As String, ByVal Moneda As String, ByVal accion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenesInversion_BuscarNegociacion")
        db.AddInParameter(dbCommand, "@p_Accion", DbType.String, accion)
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, CategoriaInstrumento)
        db.AddInParameter(dbCommand, "@p_FechaNegociacion", DbType.String, FechaOperacion)
        If CodigoSBS.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, CodigoSBS)
        End If
        If CodigoISIN.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, CodigoISIN)
        End If
        If CodigoMnemonico.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, CodigoMnemonico)
        End If
        If Portafolio.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, Portafolio)
        End If
        If Operacion.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_codigoOperacion", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_codigoOperacion", DbType.String, Operacion)
        End If
        If Moneda.Trim = "" Then
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, String.Empty)
        Else
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, Moneda)
        End If

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarPreOrdenesParaAsignacion(ByVal FechaOperacion As Decimal, ByVal CategoriaInstrumento As String, ByVal CodigoMnemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenesInversion_BuscarParaAsignacion")

        db.AddInParameter(dbCommand, "@p_FechaNegociacion", DbType.String, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, CategoriaInstrumento)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarOIEjecutadas(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIEjecutadas")
        dbCommand.CommandTimeout = 1020  'HDG 20110905
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@p_NumeroOrden", DbType.String, nroOrden)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarOIPorEjecutar(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIPorEjecutar")
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@p_NumeroOrden", DbType.String, nroOrden)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fechaOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenAcciones(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenAcciones")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenAsignacion(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenAsignacion")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenBonos(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenBonos")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenCompraVenta(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenCompraVenta")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenForwardDivisas(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenForwardDivisas")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenLetrasHipotecarias(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenLetrasHipotecarias")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenOperacionesReporte(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenOperacionesReporte")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenOrdenesFondo(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenOrdenesFondo")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ResumenPreOrdenDepositoPlazos(ByVal CodigoPreOrden As String, ByVal Clase As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ResumenPreOrdenDepositoPlazos")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, CodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
    Public Function ListarOIConfirmadas(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As Decimal, ByVal tipoInstrumento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIConfirmadas")
        dbCommand.CommandTimeout = 1020  'HDG 20110905
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@p_NumeroOrden", DbType.String, nroOrden)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_tipoInstrumento", DbType.String, tipoInstrumento)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
    Public Function GetComisionesOrdenInversionByPoliza(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal fechaOperacion As String, ByVal numeroPoliza As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetImpuestosComisionesByNumeroPoliza")
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@intFechaOperacion", DbType.String, fechaOperacion)
        db.AddInParameter(dbCommand, "@nvcNumeroPoliza", DbType.String, numeroPoliza)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function GetOrdenesByNumeroPoliza(ByVal codigoFondo As String, ByVal fechaOperacion As String, ByVal numeroPoliza As String, ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetOrdenesByNumeroPoliza")
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@intFechaOperacion", DbType.String, fechaOperacion)
        db.AddInParameter(dbCommand, "@nvcNumeroPoliza", DbType.String, numeroPoliza)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Sub UpdateImpuestosComisionesOrdenPreOrden(ByVal codigoFondo As String, ByVal codigoOrdenPreOrden As String, ByVal codigoComision As String, ByVal importeComision As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_UpdateImpuestosComisionesOrdenPreOrden")
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@nvcCodigoOrdenPreOrden", DbType.String, codigoOrdenPreOrden)
        db.AddInParameter(dbCommand, "@nvcCodigoComision", DbType.String, codigoComision)
        db.AddInParameter(dbCommand, "@decValorComision", DbType.Decimal, importeComision)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function ListarOIEjecutadasExtornadas(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIEjecutadasExtornadas")
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@p_NumeroOrden", DbType.String, nroOrden)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarOIAsignadas(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIAsignadas")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarOIAsignadas(ByVal StrCodigoMnemonico As String, ByVal StrCodiIsin As String, ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIAsignadas")
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, StrCodiIsin)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, StrCodigoMnemonico)

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, Fecha)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarOIExcedidas(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIExcedidas")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarOIAprobadas(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIAprobadas")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 60022 20100813
    Public Function ListarOIAprobadasExcesoBroker(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarOIAprobadasExcesoBroker")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 60022 20100714
    Public Function ListarOIExcedidasBroker(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarOIExcedidasBroker")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 62254 20110415
    Public Function ListarExcesoPorBroker(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarExcesoPorBroker_OrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de OrdenInversion tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoOperacion(ByVal codigoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoEstadoOrden"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoEstadoOrden(ByVal codigoEstadoOrden As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoEstadoOrden")

        db.AddInParameter(dbCommand, "@p_CodigoEstadoOrden", DbType.String, codigoEstadoOrden)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMonedaDestino"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMonedaDestino(ByVal codigoMonedaDestino As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoMonedaDestino")

        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, codigoMonedaDestino)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoIntermediario"></param>
    ''' <param name="codigoContacto"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoIntermediario_CodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoCupon"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoCupon(ByVal codigoTipoCupon As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoTipoCupon")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, codigoTipoCupon)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OrdenInversion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de OrdenInversion tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function


    Public Function EliminarPorCodigoOperacion(ByVal codigoOperacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoEstadoOrden(ByVal codigoEstadoOrden As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoEstadoOrden")

        db.AddInParameter(dbCommand, "@p_CodigoEstadoOrden", DbType.String, codigoEstadoOrden)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMonedaDestino(ByVal codigoMonedaDestino As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoMonedaDestino")

        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, codigoMonedaDestino)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoIntermediario_CodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoCupon(ByVal codigoTipoCupon As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoTipoCupon")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, codigoTipoCupon)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'Public Function ListarOrdenesInversionPorFecha(ByVal fecha As Decimal, ByVal portafolio As String) As DataSet
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversion_PorFechaReporte")

    '    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
    '    db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)

    '    Return db.ExecuteDataSet(dbCommand)
    'End Function

    'RGF 20080929
    Public Function ListarOrdenesInversionPorFecha(ByVal fecha As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal estado As String, ByVal RegSBS As String, ByVal liqAntFon As String) As DataSet 'HDG OT 64767 20120222
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_PorFechaReporte")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@nvcEstado", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_RegSBS", DbType.String, RegSBS) 'HDG OT 62255 20110214
        db.AddInParameter(dbCommand, "@p_liqAntFon", DbType.String, liqAntFon) 'HDG OT 64767 20120222
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'CMB OT 65473 20120626 Se agrego un parametro opcional CodigoOrden
    Public Function ConsultaOrdenesPreOrdenes(ByVal fechainicio As Decimal, _
                                                ByVal fechafin As Decimal, ByVal codigooperacion As String, _
                                                ByVal codigotipoinstrumento As String, ByVal nemonico As String, _
                                                ByVal isin As String, ByVal sbs As String, ByVal tiporenta As String, _
                                                ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_consultarOrdenesPreordenes")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechainicio)
        db.AddInParameter(dbCommand, "@p_fechafin", DbType.Decimal, fechafin)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigooperacion)
        db.AddInParameter(dbCommand, "@p_CodigotipoInstrumentoSBS", DbType.String, codigotipoinstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, nemonico)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, isin)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, sbs)
        db.AddInParameter(dbCommand, "@p_codigotipoRenta", DbType.String, tiporenta)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ConsultaOrdenesPreOrdenes(ByVal fechainicio As Decimal, _
                                                    ByVal fechafin As Decimal, ByVal codigooperacion As String, _
                                                    ByVal codigotipoinstrumento As String, ByVal nemonico As String, _
                                                    ByVal isin As String, ByVal sbs As String, ByVal tiporenta As String, _
                                                    ByVal portafolio As String, _
                                                    ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_consultarOrdenesPreordenes")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechainicio)
        db.AddInParameter(dbCommand, "@p_fechafin", DbType.Decimal, fechafin)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigooperacion)
        db.AddInParameter(dbCommand, "@p_CodigotipoInstrumentoSBS", DbType.String, codigotipoinstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, nemonico)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, isin)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, sbs)
        db.AddInParameter(dbCommand, "@p_codigotipoRenta", DbType.String, tiporenta)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_codigoOrden", DbType.String, codigoOrden)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ConsultaKardex(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_consultarInstrumentosCArtera")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechainicio)
        db.AddInParameter(dbCommand, "@p_Fechafin", DbType.Decimal, fechafin)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, nemonico)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, isin)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, sbs)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ConsultaOperacionesRealizadas(ByVal portafolio As String, ByVal fechainicio As Decimal, ByVal fechafin As Decimal, _
    ByVal nemonico As String, ByVal isin As String, ByVal sbs As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_consultarOperacionesRealizadas")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechainicio)
        db.AddInParameter(dbCommand, "@p_Fechafin", DbType.Decimal, fechafin)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, nemonico)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, sbs)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, isin)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ConsultaHistoricaPreOrdenes(ByVal Portafolio As String, ByVal fechaBusqueda As Decimal, ByVal codigooperacion As String, ByVal nemonico As String, ByVal isin As String, ByVal sbs As String, ByVal estado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_consultaHistoricaPreordenes")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaBusqueda", DbType.Decimal, fechaBusqueda)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigooperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, nemonico)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, isin)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, sbs)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Elimina un expediente de OrdenInversion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        'Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversion_EliminarPorCodigoISIN_CodigoNemonico")

        'db.AddInParameter(dbCommand, "@p_fechaOperacion", DbType.String, codigoISIN)
        'db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, codigoNemonico)

        'db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function RetornarFechaVencimiento(ByVal fechaOperacion As Decimal, ByVal CodigoMnemonico As String, _
        ByVal portafolio As String, ByVal codigoTercero As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_RetornarFechaVencimiento")
            db.AddInParameter(dbCommand, "@p_fechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_codigoMnemonico ", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            Return db.ExecuteScalar(dbCommand)
        End Using
    End Function
    Public Function RetornarClaseInstrumento(ByVal ClaseInstrumento As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenInversion_RetornarClaseInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, ClaseInstrumento)
        Return db.ExecuteScalar(dbCommand)
    End Function
    Public Sub InicializarOrdenInversion(ByRef oRowOI As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)

        oRowOI.CodigoOrden = ""
        oRowOI.FechaOperacion = DECIMAL_NULO
        oRowOI.FechaLiquidacion = DECIMAL_NULO
        oRowOI.MontoNominalOrdenado = DECIMAL_NULO
        oRowOI.MontoNominalOperacion = DECIMAL_NULO
        oRowOI.CodigoTipoCupon = ""
        oRowOI.YTM = DECIMAL_NULO
        oRowOI.PrecioNegociacionLimpio = DECIMAL_NULO
        oRowOI.PrecioNegociacionSucio = DECIMAL_NULO
        oRowOI.MontoOperacion = DECIMAL_NULO
        oRowOI.Observacion = ""
        oRowOI.Plazo = DECIMAL_NULO
        oRowOI.PTasa = DECIMAL_NULO
        oRowOI.InteresAcumulado = DECIMAL_NULO
        oRowOI.InteresCastigado = DECIMAL_NULO
        oRowOI.TasaCastigo = DECIMAL_NULO
        oRowOI.MontoPreCancelar = DECIMAL_NULO
        oRowOI.InteresCorridoNegociacion = DECIMAL_NULO
        oRowOI.PorcentajeAcciones = DECIMAL_NULO
        oRowOI.PorcentajeDolares = DECIMAL_NULO
        oRowOI.PorcentajeBonos = DECIMAL_NULO
        oRowOI.CantidadValor = DECIMAL_NULO
        oRowOI.MontoContado = DECIMAL_NULO
        oRowOI.MontoPlazo = DECIMAL_NULO
        oRowOI.Precio = DECIMAL_NULO
        oRowOI.TipoCambio = DECIMAL_NULO
        oRowOI.MontoOrigen = DECIMAL_NULO
        oRowOI.MontoDestino = DECIMAL_NULO
        oRowOI.TipoCobertura = ""
        oRowOI.FechaPago = DECIMAL_NULO
        oRowOI.ContadoSoles = DECIMAL_NULO
        oRowOI.TipoCambioSpot = DECIMAL_NULO
        oRowOI.ContadoDolares = DECIMAL_NULO
        oRowOI.PlazoSoles = DECIMAL_NULO
        oRowOI.TipoCambioForw = DECIMAL_NULO
        oRowOI.PlazoDolares = DECIMAL_NULO
        oRowOI.PorcentajeRendimiento = DECIMAL_NULO
        oRowOI.TipoCambioFuturo = DECIMAL_NULO
        oRowOI.CodigoMoneda = ""
        oRowOI.CodigoMonedaDestino = ""
        oRowOI.CodigoPreOrden = ""
        oRowOI.TotalComisiones = DECIMAL_NULO
        oRowOI.PrecioPromedio = DECIMAL_NULO
        oRowOI.MontoNetoOperacion = DECIMAL_NULO
        oRowOI.Estado = ""
        oRowOI.Situacion = ""
        oRowOI.Delibery = ""
        oRowOI.CodigoISIN = ""
        oRowOI.CodigoSBS = ""
        oRowOI.CodigoTercero = ""
        oRowOI.CodigoPortafolioSBS = ""
        oRowOI.CodigoMnemonico = ""
        oRowOI.CodigoContacto = ""
        oRowOI.CodigoOperacion = ""
        oRowOI.CantidadOrdenado = DECIMAL_NULO
        oRowOI.CantidadOperacion = DECIMAL_NULO
        oRowOI.NumeroPoliza = ""
        oRowOI.CodigoUsuario = ""
        oRowOI.HoraOperacion = ""
        oRowOI.CodigoGestor = ""
        oRowOI.CodigoMonedaOrigen = ""
        oRowOI.Diferencial = DECIMAL_NULO
        oRowOI.CodigoMotivo = ""
        oRowOI.MontoCancelar = DECIMAL_NULO
        oRowOI.CodigoTipoTitulo = ""
        oRowOI.CategoriaInstrumento = ""
        oRowOI.CodigoMnemonicoReporte = ""
        oRowOI.FechaContrato = DECIMAL_NULO
        oRowOI.OR_CantidadRF = DECIMAL_NULO
        oRowOI.OR_CantidadNominal = DECIMAL_NULO
        oRowOI.OR_CantidadRV = DECIMAL_NULO
        oRowOI.TasaPorcentaje = DECIMAL_NULO
        oRowOI.FechaTrato = DECIMAL_NULO
        oRowOI.TipoFondo = ""
        oRowOI.IsTemporal = ""
        oRowOI.PrecioCalculado = DECIMAL_NULO
        oRowOI.InteresCorrido = DECIMAL_NULO
        oRowOI.OrdenGenera = ""
        oRowOI.Fixing = DECIMAL_NULO
        oRowOI.GrupoIntermediario = ""
        oRowOI.FechaIDI = DECIMAL_NULO 'DB 20090506
        oRowOI.Ficticia = ""
        oRowOI.Renovacion = ""  'HDG OT 61566 Nro5-R12 20101122
        oRowOI.TipoMonedaForw = ""  'HDG OT 61573 20101125
        oRowOI.EventoFuturo = 0 'CMB OT 64769 20120320
    End Sub

    Public Function ConsultaCertificados(ByVal intIndicador As Integer, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ConsultaCertificados")
        db.AddInParameter(dbCommand, "@p_Indica", DbType.Int16, intIndicador)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ObtenerValorCustodioOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal codigoCustodio As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioOI_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        Return Convert.ToDecimal(db.ExecuteDataSet(dbCommand).Tables(0).Rows(0)(0))
    End Function

    Public Function InsertarCustodioOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal codigoCustodio As String, ByVal valorAsignacion As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioOI_Insertar")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@p_ValorAsignacion", DbType.String, valorAsignacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ModificarCustodioOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal codigoCustodio As String, ByVal valorAsignacion As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioOI_Modificar")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@p_ValorAsignacion", DbType.String, valorAsignacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarCustodioOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal codigoCustodio As String, ByVal valorAsignacion As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioOI_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function SeleccionarPorFiltro_ValorCustodioOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal situacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioOI_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function OtroSeleccionarPorFiltro_ValorCustodioOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioOI_OtroSeleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function GetEstadoOrdenes() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetEstadoOI")
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    Public Function validarPolizaExistencia(ByVal varCodigoOrden As String, ByVal vartbNPoliza As String, ByVal varddlOperacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ValidarPolizaExistencia")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, varCodigoOrden)
        db.AddInParameter(dbCommand, "@p_NPoliza", DbType.String, vartbNPoliza)
        db.AddInParameter(dbCommand, "@p_Operacion", DbType.String, varddlOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function verValorizadoExistencia(ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As Decimal, ByVal NumeroOrden As String, ByRef Liquidado As Boolean, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ValorizadoExistencia")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_Operacion", DbType.String, NumeroOrden)
        db.AddOutParameter(dbCommand, "@p_Liquidado", DbType.String, 1)
        verValorizadoExistencia = db.ExecuteDataSet(dbCommand)
        Liquidado = CType(db.GetParameterValue(dbCommand, "@p_Liquidado"), Boolean)
    End Function
    Public Function ListarVencimientosFuturos(ByVal dataRequest As DataSet, ByVal fondo As String, ByVal fecha As Decimal, ByVal fechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VencimientosFuturos_Listar")
        dbCommand.CommandTimeout = 1020  'HDG 20110831
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, fondo)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ExistenciaOperacionCaja(ByVal dataRequest As DataSet, ByVal codigoOperacion As String, ByVal fechaOperacion As String, _
    ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OIOperacionCaja_Existencia")
        Dim oOrdenPreOrdenInversionBE As New OrdenPreOrdenInversionBE
        db.AddInParameter(dbCommand, "@p_Operacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, Convert.ToDecimal(fechaOperacion))
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarForwardsCartas(ByVal fechaOperacion As Decimal, ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarForwardsCartas")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarVencimientoForwardNoDelivery(ByVal portafolio As String, ByVal fechaVencimientoDesde As Decimal, ByVal fechaVencimientoHasta As Decimal, ByVal codigoMoneda As String, ByVal codigoMonedaDestino As String, ByVal codigoMercado As String, ByVal Calculo As Decimal) As DataSet 'HDG OT 63063 R09 20110616
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VencimientoForwardND_Listar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaVencimientoDesde", DbType.Decimal, fechaVencimientoDesde)
        db.AddInParameter(dbCommand, "@p_FechaVencimientoHasta", DbType.Decimal, fechaVencimientoHasta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, codigoMonedaDestino)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_Calculo", DbType.Decimal, Calculo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarVencimientoForward(ByVal portafolio As String, ByVal fechaProceso As Decimal, ByVal fechaVencimientoDesde As Decimal, ByVal fechaVencimientoHasta As Decimal, ByVal codigoMoneda As String, ByVal codigoMonedaDestino As String, ByVal Estado As Decimal, ByVal tRangoFecha As Decimal, ByVal codigoMercado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VencimientoForward_Listar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_FechaVencimientoDesde", DbType.Decimal, fechaVencimientoDesde)
        db.AddInParameter(dbCommand, "@p_FechaVencimientoHasta", DbType.Decimal, fechaVencimientoHasta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, codigoMonedaDestino)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.Decimal, Estado)
        db.AddInParameter(dbCommand, "@p_RangoFecha", DbType.Decimal, tRangoFecha)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 63375 20110627
    Public Function InventarioForward(ByVal portafolio As String, ByVal fechaProceso As Decimal, ByVal fechaVencimientoDesde As Decimal, ByVal fechaVencimientoHasta As Decimal, ByVal codigoMoneda As String, ByVal codigoMonedaDestino As String, ByVal Estado As Decimal, ByVal tRangoFecha As Decimal, ByVal codigoMercado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_InventarioForward")
        dbCommand.CommandTimeout = 1020  'HDG 20120105
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaVencimientoDesde)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaVencimientoHasta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, codigoMonedaDestino)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.Decimal, Estado)
        db.AddInParameter(dbCommand, "@p_RangoFecha", DbType.Decimal, tRangoFecha)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'LETV 20090225
    Public Function ModificarNumeroContratoForward(ByVal portafolio As String, _
                                                   ByVal codigoOrden As String, _
                                                   ByVal numeroContrato As String, _
                                                   ByVal Mtm As Decimal, _
                                                   ByVal MtmDestino As Decimal, _
                                                   ByVal PrecioVector As Decimal, _
                                                   ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VencimientoForward_ModificarNumeroContrato")
        dbCommand.CommandTimeout = 1020  'HDG 20120105
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_NumeroContrato", DbType.String, numeroContrato)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'JVC 20090406 VECTOR PRECIO FORWARDS
        db.AddInParameter(dbCommand, "@p_PrecioVector", DbType.Decimal, PrecioVector)
        db.AddInParameter(dbCommand, "@p_Mtm", DbType.Decimal, Mtm)
        db.AddInParameter(dbCommand, "@p_MtmDestino", DbType.Decimal, MtmDestino)
        'fin
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'RGF 20090209
    Public Function ConfirmarVencimientoForwardNoDelivery(ByVal portafolio As String, ByVal codigoOrden As String, ByVal fixing As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VencimientoForwardND_ConfirmarOrden")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_Fixing", DbType.Decimal, fixing)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'RGF 20090316
    Public Function ObtenerSaldosCustodio(ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Custodio_ObtenerSaldos")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'DB 20090506
    Public Function SeleccionarOI_FechaIDI(ByVal fondo As String, ByVal fecha As Decimal, ByVal nemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_SeleccionarFechaIDI")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, nemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'DB 20090506
    Public Function ActualizarFechaIDI(ByVal codigoOrden As String, ByVal portafolio As String, ByVal fechaIDI As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ActualizarFechaIDI")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_FechaIDI", DbType.Decimal, fechaIDI)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''''HDG OT 59298 20100422
    '''Public Sub ValidarPrecio(ByVal strClaseInstrumento As String, ByVal strOperacion As String, ByVal strMnemonico As String, ByVal strIntermediario As String, ByVal strFondo As String, ByRef decPrecio1 As Decimal, ByRef decPrecio2 As Decimal _
    ''', Optional ByVal codigoMoneda As String = "", Optional ByVal CodigoMonedaDestino As String = "")
    '''    Dim db As Database = DatabaseFactory.CreateDatabase()
    '''    Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversion_ValidarPrecio")
    '''    db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, strClaseInstrumento)
    '''    db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, strOperacion)
    '''    db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strMnemonico)
    '''    db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, strIntermediario)
    '''    db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strFondo)
    '''    dbCommand.AddParameter("@p_Precio1", DbType.Double, 13, ParameterDirection.InputOutput, True, 27, 12, "", DataRowVersion.Default, decPrecio1)
    '''    dbCommand.AddParameter("@p_Precio2", DbType.Double, 13, ParameterDirection.InputOutput, True, 27, 12, "", DataRowVersion.Default, decPrecio2)
    '''    'db.AddOutParameter(dbCommand, "@p_Precio1", DbType.Double, 13)
    '''    'db.AddOutParameter(dbCommand, "@p_Precio2", DbType.Double, 13)

    '''    db.ExecuteNonQuery(dbCommand)

    '''    decPrecio1 = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_Precio1"))
    '''    decPrecio2 = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_Precio2"))

    '''End Sub

    'HDG OT 60072 20100615
    'Public Sub FechaModificarEliminarOI(ByVal Portafolio As String, ByVal CodigoOrden As String, ByVal Fecha As Integer, ByVal tProc As String)
    Public Sub FechaModificarEliminarOI(ByVal Portafolio As String, ByVal CodigoOrden As String, ByVal Fecha As Integer, ByVal tProc As String, ByVal Comentario As String, ByVal dataRequest As DataSet)   'HDG OT 60882 20100915
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_FechaEliminarModificarOI")

        db.AddInParameter(dbCommand, "@p_PortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_UsuarioModificaOI", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario")) 'HDG OT 60882 20100915
        db.AddInParameter(dbCommand, "@p_Comentarios", DbType.String, Comentario) 'HDG OT 60882 20100915
        db.AddInParameter(dbCommand, "@p_TProceso", DbType.String, tProc)

        db.ExecuteNonQuery(dbCommand)
    End Sub

    'HDG OT 61566 Nro5-R12 20101122
    Public Sub OrdenSimultaneoSwapOI(ByVal strCodigoOrden As String, ByVal strCodigoOrdenSim As String, ByVal strPrior As String, ByVal strFondo As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_OrdenSimultaneoSwap")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenSim", DbType.String, strCodigoOrdenSim)
        db.AddInParameter(dbCommand, "@p_Prior", DbType.String, strPrior)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strFondo)

        db.ExecuteNonQuery(dbCommand)
    End Sub

    'HDG OT 61566 Nro5-R12 20101122
    Public Function ListarOrdenesInversionSwap(ByVal codigoOrden As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_OrdenInversion_NroOrdenesSwap")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)

        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    'HDG OT 61573 20101125
    'Public Function SeleccionarTipoMonedaxMotivoForw(ByVal codigoMotivo As String) As DataTable   'HDG OT 62325 20110323
    Public Function SeleccionarTipoMonedaxMotivoForw(ByVal codigoMotivo As String, ByVal codigoMoneda As String) As DataTable   'HDG OT 62325 20110323
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_TipoMonedaxMotivoForw_Seleccionar")

        db.AddInParameter(dbCommand, "@CodigoMotivo", DbType.String, codigoMotivo)
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, codigoMoneda)   'HDG OT 62325 20110323

        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    'HDG OT 62087 Nro14-R23 20110315
    Public Function ValidarLineaNegociacionDPZ(ByVal Mnemonico As String, ByVal Fecha As Decimal, ByVal Portafolio As String, ByVal CodigoTercero As String, ByVal MontoOperacion As Decimal, ByVal CodigoOrden As String) As Boolean   'HDG OT 62087 Nro14-R23 20110511
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Validar_LineaNegociacionDPZ")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, Mnemonico)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, MontoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)   'HDG OT 62087 Nro14-R23 20110511

        Return db.ExecuteScalar(dbCommand)
    End Function

    ''CMB OT 62087 20110316 Nro 8
    'Public Function ResumenCajaPorMonedas(ByVal fechaLiquidacion As Decimal) As DataSet
    '    Dim db As Database = DatabaseFactory.CreateDatabase

    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ResumenCaja_OrdenInversion")
    '    dbCommand.CommandTimeout = 600 'CMB 20110614 OT 63063 REQ 10 
    '    db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, fechaLiquidacion)

    '    Return db.ExecuteDataSet(dbCommand)
    'End Function

    'CMB OT 62254 20110324
    Public Function ObtenerUsuario(ByVal strCodigoOrden As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerUsuario_OrdenInversion")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        Return db.ExecuteDataSet(dbCommand).Tables(0).Rows(0)("UsuarioCreacion").ToString()
    End Function

    'HDG OT 63063 R04 20110523
    Public Function SeleccionarOrdenExcepPorFiltro(ByVal strTipoRenta As String, ByVal strPortafolio As String, ByVal decFechaInicio As Decimal, ByVal decFechaFin As Decimal, ByVal strTipoOperacion As String, ByVal strCodigoTipoInstrumentoSBS As String, ByVal strExclusion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarOI_LimiteExclusion")
        db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, decFechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, decFechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, strTipoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, strCodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_CodigoExclusion", DbType.String, strExclusion)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 63063 R04 20110526
    Public Sub ExcepcionLimiteNegociacion(ByVal strCodigoOrden As String, ByVal strCodigoExcepcion As String, ByVal decCantidad As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ExcepcionLimiteNegociacion")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoExcepcion", DbType.String, strCodigoExcepcion)
        db.AddInParameter(dbCommand, "@p_CantidadExcepcion", DbType.Decimal, decCantidad)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Sub

    'CMB OT 63063 20110602 REQ 06
    Public Sub InicializarOperacionesEPU(ByRef oRow As TmpOperacionesEPUBE.TmpOperacionesEPURow)
        oRow.TrdDate = DECIMAL_NULO
        oRow.SetDate = DECIMAL_NULO
        oRow.NumberUnits = DECIMAL_NULO
        oRow.SharesPerUnit = DECIMAL_NULO
        oRow.EstimatedCash = DECIMAL_NULO
        oRow.FinalCash = DECIMAL_NULO
        oRow.CashInLieu = DECIMAL_NULO
        oRow.CommissionRate = DECIMAL_NULO
        oRow.CreationCost = DECIMAL_NULO
    End Sub

    'CMB OT 63063 20110622 REQ 06
    Public Sub InicializarOperacionesEPUDet(ByRef oRow As TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow)
        oRow.Id = DECIMAL_NULO
        oRow.Cntry = ""
        oRow.Acct = ""
        oRow.AcctDescription = ""
        oRow.Ric = ""
        oRow.Bloomberg = ""
        oRow.Sedol = ""
        oRow.ISIN = ""
        oRow.CompanyName = ""
        oRow.ByS = ""
        oRow.ExecutedShares = DECIMAL_NULO
        oRow.ExecutedPrices = DECIMAL_NULO
        oRow.NetComm = DECIMAL_NULO
        oRow.NetTax = DECIMAL_NULO
        oRow.Currency = ""
        oRow.ResidualQty = DECIMAL_NULO
        oRow.CodigoOrden = ""
        oRow.CodigoPortafolioSBS = ""
        oRow.CodigoTercero = ""
        oRow.TipoOperacion = ""
        oRow.TipoCambio = DECIMAL_NULO
    End Sub

    'CMB OT 63063 20110624 REQ 06
    Public Sub InicializarResumenOperacionesEPU(ByRef oRow As TmpResumenOperacionesEPUBE.TmpResumenOperacionesEPURow)
        oRow.Currency = ""
        oRow.USDAmount = DECIMAL_NULO
        oRow.FXRate = DECIMAL_NULO
        oRow.FXCross = DECIMAL_NULO
        oRow.LocalAmount = DECIMAL_NULO
    End Sub

    'CMB OT 63063 20110622 REQ 06
    Public Function EliminarOperacionesEPUDet() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Inicializar_TmpOperacionesEPUDet")
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110621 REQ 06
    Public Function InsertarOperacionesEPU(ByVal oTmpOperacionesEPUBE As TmpOperacionesEPUBE) As Boolean
        oTmpOperacionesEPURow = CType(oTmpOperacionesEPUBE.TmpOperacionesEPU.Rows(0), TmpOperacionesEPUBE.TmpOperacionesEPURow)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_TmpOperacionesEPU")
        db.AddInParameter(dbCommand, "@p_TrdDate", DbType.Decimal, oTmpOperacionesEPURow.TrdDate)
        db.AddInParameter(dbCommand, "@p_SetDate", DbType.Decimal, oTmpOperacionesEPURow.SetDate)
        db.AddInParameter(dbCommand, "@p_NumberUnits", DbType.Decimal, oTmpOperacionesEPURow.NumberUnits)
        db.AddInParameter(dbCommand, "@p_SharesPerUnit", DbType.Decimal, oTmpOperacionesEPURow.SharesPerUnit)
        db.AddInParameter(dbCommand, "@p_EstimatedCash", DbType.Decimal, oTmpOperacionesEPURow.EstimatedCash)
        db.AddInParameter(dbCommand, "@p_FinalCash", DbType.Decimal, oTmpOperacionesEPURow.FinalCash)
        db.AddInParameter(dbCommand, "@p_CashInLieu", DbType.Decimal, oTmpOperacionesEPURow.CashInLieu)
        db.AddInParameter(dbCommand, "@p_CommissionRate", DbType.Decimal, oTmpOperacionesEPURow.CommissionRate)
        db.AddInParameter(dbCommand, "@p_CreationCost", DbType.Decimal, oTmpOperacionesEPURow.CreationCost)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110622 REQ 06
    Public Function InsertarOperacionesEPUDet(ByVal oTmpOperacionesEPUDetBE As TmpOperacionesEPUDetBE) As Boolean
        Dim i As Integer = 0
        For i = 0 To oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_TmpOperacionesEPUDet")
            oTmpOperacionesEPUDetRow = CType(oTmpOperacionesEPUDetBE.Tables(0).Rows(i), TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow)
            db.AddInParameter(dbCommand, "@p_Cntry", DbType.String, oTmpOperacionesEPUDetRow.Cntry)
            db.AddInParameter(dbCommand, "@p_Acct", DbType.String, oTmpOperacionesEPUDetRow.Acct)
            db.AddInParameter(dbCommand, "@p_AcctDescription", DbType.String, IIf(oTmpOperacionesEPUDetRow.AcctDescription = "", DBNull.Value, oTmpOperacionesEPUDetRow.AcctDescription))
            db.AddInParameter(dbCommand, "@p_Ric", DbType.String, oTmpOperacionesEPUDetRow.Ric)
            db.AddInParameter(dbCommand, "@p_Bloomberg", DbType.String, oTmpOperacionesEPUDetRow.Bloomberg)
            db.AddInParameter(dbCommand, "@p_Sedol", DbType.String, oTmpOperacionesEPUDetRow.Sedol)
            db.AddInParameter(dbCommand, "@p_ISIN", DbType.String, oTmpOperacionesEPUDetRow.ISIN)
            db.AddInParameter(dbCommand, "@p_CompanyName", DbType.String, oTmpOperacionesEPUDetRow.CompanyName)
            db.AddInParameter(dbCommand, "@p_ByS", DbType.String, oTmpOperacionesEPUDetRow.ByS)
            db.AddInParameter(dbCommand, "@p_ExecutedShares", DbType.Decimal, oTmpOperacionesEPUDetRow.ExecutedShares)
            db.AddInParameter(dbCommand, "@p_ExecutedPrices", DbType.Decimal, oTmpOperacionesEPUDetRow.ExecutedPrices)
            db.AddInParameter(dbCommand, "@p_NetComm", DbType.Decimal, oTmpOperacionesEPUDetRow.NetComm)
            db.AddInParameter(dbCommand, "@p_NetTax", DbType.Decimal, oTmpOperacionesEPUDetRow.NetTax)
            db.AddInParameter(dbCommand, "@p_Currency", DbType.String, oTmpOperacionesEPUDetRow.Currency)
            db.AddInParameter(dbCommand, "@p_ResidualQty", DbType.Decimal, oTmpOperacionesEPUDetRow.ResidualQty)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oTmpOperacionesEPUDetRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_TipoOperacion", DbType.String, oTmpOperacionesEPUDetRow.TipoOperacion)
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function

    'CMB OT 63063 20110624 REQ 06
    Public Function InsertarResumenOperacionesEPU(ByVal oTmpResumenOperacionesEPUBE As TmpResumenOperacionesEPUBE) As Boolean
        Dim i As Integer = 0
        For i = 0 To oTmpResumenOperacionesEPUBE.TmpResumenOperacionesEPU.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_TmpResumenOperacionesEPU")
            oTmpResumenOperacionesEPURow = CType(oTmpResumenOperacionesEPUBE.TmpResumenOperacionesEPU.Rows(i), TmpResumenOperacionesEPUBE.TmpResumenOperacionesEPURow)
            db.AddInParameter(dbCommand, "@p_Currency", DbType.String, oTmpResumenOperacionesEPURow.Currency)
            db.AddInParameter(dbCommand, "@p_LocalAmount", DbType.Decimal, oTmpResumenOperacionesEPURow.LocalAmount)
            db.AddInParameter(dbCommand, "@p_FXRate", DbType.Decimal, oTmpResumenOperacionesEPURow.FXRate)
            db.AddInParameter(dbCommand, "@p_USDAmount", DbType.Decimal, oTmpResumenOperacionesEPURow.USDAmount)
            db.AddInParameter(dbCommand, "@p_FXCross", DbType.String, oTmpResumenOperacionesEPURow.FXCross)
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function

    'CMB OT 63063 20110624 REQ 06
    Public Function EliminarResumenOperacionesEPU() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Inicializar_TmpResumenOperacionesEPU")
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110622 REQ 06
    Public Function ActualizarOperacionesEPUDet(ByVal Id As Int32, ByVal portafolio As String, ByVal codigoTercero As String, ByVal tipoCambio As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_TmpOperacionesEPUDet")
        db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, Id)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_TipoCambio", DbType.Decimal, tipoCambio)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110602 REQ 06
    Public Function GenerarOperacionesEPU(ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_GenerarOI_TmpOperacionesEPU")
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110603 REQ 06
    Public Function SeleccionarPrevOperacionesEPU() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Seleccionar_TmpOperacionesEPU")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Seleccion una serie de listado para Generar el Reporte de Operaciones EPU .
    ''' <summary>
    ''' <param></param>
    ''' <returns>OperacionContableBE</returns>

    'GTC OT 63063 20110627 REQ 06
    Public Function GenerarReporteOperacionesPU() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteOperacionesEPU")
        Dim objeto As New DataSet
        objeto = db.ExecuteDataSet(dbCommand)
        Return objeto
    End Function

    'CMB OT 64292 20111123
    Public Function GenerarReporteDeFallasOI(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal opcion As Decimal, ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteDeFallas_OrdenInversion")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Opcion", DbType.Decimal, opcion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 64291 20111202
    Public Function ValidacionPuntual_LimitesTrading(ByVal codigoNemonico As String, ByVal fechaOperacion As Decimal, ByVal portafolio As String, ByVal montoNegociado As Decimal, ByVal codigoMoneda As String, ByVal usuarioTrader As String, Optional ByVal CategoriaInstrumento As String = "") As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ValidacionPuntual_LimitesTrading")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_MontoNegociado", DbType.Decimal, montoNegociado)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, CategoriaInstrumento)
        db.AddInParameter(dbCommand, "@p_UsuarioTrader", DbType.String, usuarioTrader)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 65473 20120921
    Public Function ObtenerFirmasLlamadoOI(ByVal codigoOrden As String, ByVal fecha As Decimal, ByVal flagMostrarFirma As Boolean) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerFirmas_LlamadoOrdenInversion")
        db.AddInParameter(dbCommand, "@p_codigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_flagMostrarFirma", DbType.Decimal, Convert.ToDecimal(flagMostrarFirma))
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 66471 20121227
    Public Function ReporteVerificacionFirmas(ByVal fecha As Decimal, ByVal codCargoFirmante As Decimal, ByVal codigoUsuario As String, _
                                              ByVal codigoOrden As String, ByVal estFirmaD As String, ByVal codPortafolioSBS As String, _
                                              ByVal codigoOperacion As String, ByVal codigoMercado As String, ByVal codCategReporte As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_RptVerificacionFirmas_OrdenInversion")
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_codCargoFirmante", DbType.Decimal, codCargoFirmante)
        db.AddInParameter(dbCommand, "@p_codigoUsuario", DbType.String, codigoUsuario)
        db.AddInParameter(dbCommand, "@p_codigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_estFirmaD", DbType.String, estFirmaD)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_codCategReporte", DbType.String, codCategReporte)

        Return db.ExecuteDataSet(dbCommand)
    End Function

#Region "66056 - Modificacion: JZAVALA"

    ''' <summary>66056 - JZAVALA.
    ''' METODO PARA CARGAR EL REPORTE CONSOLIDADO EN CRYSTAL REPORT.
    ''' </summary>
    ''' <param name="strCodigoPrevOrden"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ListarDatosOperacionPorCodigoPrevOrdenInversion(ByVal strCodigoPrevOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_DatosOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoPrevOrden)

        Return db.ExecuteDataSet(dbCommand)
    End Function

#End Region

    'HDG OT 67944 20130705
    Public Function EliminarTablaTmpNemonicosFondoRenta() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Inicializar_tmpInstrumentosCartera")
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'HDG OT 67944 20130705
    Public Function ActualizarInstrumentosPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sCodigoSBS As String
        Dim sFondo As String
        Dim nCantidad As Decimal
        Dim sAfpDestino As String
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_ActualizatmpInstrumentosCarteraxExcel")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String)
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_AFPDestino", DbType.String)

        For Each filaLinea As DataRow In dtDetalle.Rows
            sFondo = filaLinea(0).ToString().Trim()
            sCodigoSBS = filaLinea(1).ToString().Trim()
            nCantidad = Val(filaLinea(2).ToString().Trim())
            sAfpDestino = filaLinea(3).ToString().Trim()

            db.SetParameterValue(dbCommand, "@p_CodigoPortafolioSBS", sFondo)
            db.SetParameterValue(dbCommand, "@p_CodigoSBS", sCodigoSBS)
            db.SetParameterValue(dbCommand, "@p_Cantidad", nCantidad)
            db.SetParameterValue(dbCommand, "@p_AFPDestino", sAfpDestino)
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function

    'HDG OT 67944 20130705
    Public Function CarteraTituloValoracionFinal(ByVal FechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ValorizacionCartera_GenerarFinal")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "CarteraTituloValoracionFinal")
        Return oReporte
    End Function

    'HDG OT 67944 20130705
    Public Function EnvioInfoCarteraAFP(ByVal FechaOperacion As Decimal, ByVal Fondo As String, ByVal AFPDestino As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_EnvioInfoCarteraAFP")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, Fondo)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_AFPDestino", DbType.String, AFPDestino)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "EnvioInfoCarteraAFP")
        Return oReporte
    End Function

    'HDG OT 67944 20130705
    Public Function EnvioConstitucionForwards(ByVal FechaOperacion As Decimal, ByVal Fondo As String, ByVal AFPDestino As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_EnvioConstitucionForwards")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, Fondo)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_AFPDestino", DbType.String, AFPDestino)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "EnvioConstitucionForwards")
        Return oReporte
    End Function

    'HDG OT 67944 20130705
    Public Function ParticionCartera(ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ParticionCartera")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Reporte_ReportesDiariosOperaciones(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal TipoConsulta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ReporteDiarioOperaciones")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@P_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@P_Fecha", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@P_TipoConsulta", DbType.String, TipoConsulta)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "RegistroInversiones")
        Return oReporte.Tables(0)
    End Function
    Public Function Reporte_RegistroInversiones(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal CodigoPortafolioSBS As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_RegistroInversiones")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFinal", DbType.Decimal, FechaFin)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.Decimal, CodigoPortafolioSBS)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "RegistroInversiones")
        Return oReporte
    End Function
    'Operaciones de Reporte
    Public Function Seleccionar_ComisionesOR(ByVal CodigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oReporteComi As New DataSet
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Seleccionar_ComisionesOR")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@P_CodigoOrden", DbType.String, CodigoOrden)
        db.LoadDataSet(dbCommand, oReporteComi, "ORComision")
        Return oReporteComi
    End Function
    Public Sub Insertar_ComisionesOR(ByVal CodigoOrden As String, ByVal ImpuestoCompra As Decimal, ByVal ImpuestoVenta As Decimal, ByVal ComisionAIVenta As Decimal,
    ByVal ComisionAICompra As Decimal, ByVal RestoComisionCompra As Decimal, ByVal RestoComisionVenta As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Insertar_ComisionesOR")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@P_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@P_ImpuestoCompra", DbType.Decimal, ImpuestoCompra)
        db.AddInParameter(dbCommand, "@P_ImpuestoVenta", DbType.Decimal, ImpuestoVenta)
        db.AddInParameter(dbCommand, "@P_ComisionAIVenta", DbType.Decimal, ComisionAIVenta)
        db.AddInParameter(dbCommand, "@P_ComisionAICompra", DbType.Decimal, ComisionAICompra)
        db.AddInParameter(dbCommand, "@P_RestoComisionCompra", DbType.Decimal, RestoComisionCompra)
        db.AddInParameter(dbCommand, "@P_RestoComisionVenta", DbType.Decimal, RestoComisionVenta)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub Modificar_ComisionesOR(ByVal CodigoOrden As String, ByVal ImpuestoCompra As Decimal, ByVal ImpuestoVenta As Decimal, ByVal ComisionAIVenta As Decimal,
    ByVal ComisionAICompra As Decimal, ByVal RestoComisionCompra As Decimal, ByVal RestoComisionVenta As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Modificar_ComisionesOR")
        db.AddInParameter(dbCommand, "@P_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@P_ImpuestoCompra", DbType.Decimal, ImpuestoCompra)
        db.AddInParameter(dbCommand, "@P_ImpuestoVenta", DbType.Decimal, ImpuestoVenta)
        db.AddInParameter(dbCommand, "@P_ComisionAIVenta", DbType.Decimal, ComisionAIVenta)
        db.AddInParameter(dbCommand, "@P_ComisionAICompra", DbType.Decimal, ComisionAICompra)
        db.AddInParameter(dbCommand, "@P_RestoComisionCompra", DbType.Decimal, RestoComisionCompra)
        db.AddInParameter(dbCommand, "@P_RestoComisionVenta", DbType.Decimal, RestoComisionVenta)
        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Function confirmarInstrumentosSinCuponera(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Dim rpta As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_SitFondos_ConfirmarOrdenesSinCuponera")
        Try
            db.AddInParameter(comando, "@p_codigoOrden", DbType.String, codigoOrden)
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_usuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(comando, "@p_fechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(comando, "@p_horaModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(comando, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(comando)
            rpta = True
        Catch ex As Exception
            rpta = False
            Throw
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return rpta
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
                                                          ByVal fechaHasta As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(Procedimiento.Reporte_InventarioForward_Fecha_Rango)

        db.AddInParameter(dbCommand, Parametro.p_CodigoPortafolioSBS, DbType.String, portafolio)
        db.AddInParameter(dbCommand, Parametro.p_FechaDesde, DbType.String, fechaDesde)
        db.AddInParameter(dbCommand, Parametro.p_FechaHasta, DbType.Decimal, fechaHasta)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    ''' <summary>
    ''' Genera la data del reporte tipo de cambio
    ''' </summary>
    ''' <param name="fechaDesde"></param>
    ''' <param name="fechaHasta"></param>
    ''' <remarks>
    ''' 2015-12-03        Herbert Mendoza              creacion
    ''' </remarks>
    Public Function Reporte_VectorTipoCambio_Fecha_Rango(ByVal fechaDesde As Decimal, ByVal fechaHasta As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(Procedimiento.Reporte_VectorTipoCambio_Fecha_Rango)
        db.AddInParameter(dbCommand, Parametro.p_FechaDesde, DbType.String, fechaDesde)
        db.AddInParameter(dbCommand, Parametro.p_FechaHasta, DbType.Decimal, fechaHasta)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Reporte_Gestion_MarkToMarkedFW_Fecha_Rango(ByVal Fecha As Decimal, ByVal Escenario As String, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.Reporte_Gestion_MarkToMarkedFW_Fecha_Rango")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, Escenario)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ReporteVencimientosCuponesOrdenes(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal TipoOperacion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_ReporteVencimientos")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_TipoOperacion", DbType.String, TipoOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "RegistroInversiones")
        Return oReporte.Tables(0)
    End Function
    Public Sub ActualizaVencimientosDPZ(ByVal CodigoPortafolioSBS As String, ByVal CodigoOrdenConstitucion As String, ByVal CodigoOrdenVencimiento As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizaVencimientosDPZ")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenConstitucion", DbType.String, CodigoOrdenConstitucion)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenVencimiento", DbType.String, CodigoOrdenVencimiento)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function TransferenciaBCR(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_TransferenciaBCR")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim oTransferenciaBCR As New DataSet
        db.LoadDataSet(dbCommand, oTransferenciaBCR, "TransferenciaBCRDia")
        Return oTransferenciaBCR.Tables(0)
    End Function
    Public Function VencimientosdelDiaDPZ(ByVal CodigoPortafolioSBS As String, ByVal CodigoTercero As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.sp_SIT_VencimientosdelDiaDPZ")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "VencimientosDPZDia")
        Return oReporte.Tables(0)
    End Function
    Public Function ConstitucionesdelDiaDPZ(ByVal CodigoPortafolioSBS As String, ByVal CodigoTercero As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.sp_SIT_ConstitucionesdelDiaDPZ")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ConstitucionesDPZDia")
        Return oReporte.Tables(0)
    End Function
    Public Function ReporteRegistroInversiones(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, ByVal FechaFinal As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ReporteRegistroInversiones")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFinal", DbType.Decimal, FechaFinal)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteRegistroInversiones")
        Return oReporte.Tables(0)
    End Function
    Public Function ResumenPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ResumenPortafolio")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ResumenPortafolio")
        Return oReporte
    End Function

    'INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-05 | Rating
    Public Function SeleccionaRatingTerceroHistoria(ByVal CodigoTercero As String, ByVal fechaInicio As Integer, ByVal fechaFin As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SeleccionaRatingTerceroHistoria")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_FechaIni", DbType.Int32, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Int32, fechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "SeleccionaRatingTerceroHistoria")
        Return oReporte.Tables(0)
    End Function

    Public Function SeleccionaRatingValorHistoria(ByVal CodigoIsin As String, ByVal fechaInicio As Integer, ByVal fechaFin As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SeleccionaRatingValorHistoria")
        db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
        db.AddInParameter(dbCommand, "@p_FechaIni", DbType.Int32, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Int32, fechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "SeleccionaRatingValorHistoria")
        Return oReporte.Tables(0)
    End Function
    'INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-05 | Rating

    Public Function BorrarCuponera_Bono_Swap(ByVal CodigoOrden As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_BorrarCuponera_Bono_Swap")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertaCuponera_Bono_Swap(ByVal Correlativo As Integer, ByVal CodigoOrden As String, ByVal FechaIniOriginal As Decimal, ByVal FechaFinOriginal As Decimal, ByVal DifDiasOriginal As Decimal, _
                                              ByVal AmortizacOriginal As Decimal, ByVal TasaCuponOriginal As Decimal, ByVal BaseCuponOriginal As String, ByVal DiasPagoOriginal As String, ByVal fechaRealInicialOriginal As Decimal, _
                                              ByVal fechaRealFinalOriginal As Decimal, ByVal AmortizacConsolidadoOriginal As Decimal, ByVal MontoInteresOriginal As Decimal, ByVal MontoAmortizacionOriginal As Decimal, _
                                              ByVal NominalRestanteOriginal As Decimal, ByVal TasaSpreadOriginal As Decimal, ByVal FechaIni As Decimal, ByVal FechaFin As Decimal, ByVal DifDias As Decimal, ByVal Amortizac As Decimal, _
                                              ByVal TasaCupon As Decimal, ByVal BaseCupon As String, ByVal DiasPago As String, ByVal fechaRealInicial As Decimal, ByVal fechaRealFinal As Decimal, ByVal AmortizacConsolidado As Decimal, _
                                              ByVal MontoInteres As Decimal, ByVal MontoAmortizacion As Decimal, ByVal NominalRestante As Decimal, ByVal TasaSpread As Decimal, ByVal FechaLiborOriginal As Decimal, ByVal FechaLibor As Decimal)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_InsertaCuponera_Bono_Swap")
            db.AddInParameter(dbCommand, "@p_Correlativo", DbType.Int32, Correlativo)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddInParameter(dbCommand, "@p_FechaIniOriginal", DbType.Decimal, FechaIniOriginal)
            db.AddInParameter(dbCommand, "@p_FechaFinOriginal", DbType.Decimal, FechaFinOriginal)
            db.AddInParameter(dbCommand, "@p_DifDiasOriginal", DbType.Decimal, DifDiasOriginal)
            db.AddInParameter(dbCommand, "@p_AmortizacOriginal", DbType.Decimal, AmortizacOriginal)
            db.AddInParameter(dbCommand, "@p_TasaCuponOriginal", DbType.Decimal, TasaCuponOriginal)
            db.AddInParameter(dbCommand, "@p_BaseCuponOriginal", DbType.String, BaseCuponOriginal)
            db.AddInParameter(dbCommand, "@p_DiasPagoOriginal", DbType.String, DiasPagoOriginal)
            db.AddInParameter(dbCommand, "@p_fechaRealInicialOriginal", DbType.Decimal, fechaRealInicialOriginal)
            db.AddInParameter(dbCommand, "@p_fechaRealFinalOriginal", DbType.Decimal, fechaRealFinalOriginal)
            db.AddInParameter(dbCommand, "@p_AmortizacConsolidadoOriginal", DbType.Decimal, AmortizacConsolidadoOriginal)
            db.AddInParameter(dbCommand, "@p_MontoInteresOriginal", DbType.Decimal, MontoInteresOriginal)
            db.AddInParameter(dbCommand, "@p_MontoAmortizacionOriginal", DbType.Decimal, MontoAmortizacionOriginal)
            db.AddInParameter(dbCommand, "@p_NominalRestanteOriginal", DbType.Decimal, NominalRestanteOriginal)
            db.AddInParameter(dbCommand, "@p_TasaSpreadOriginal", DbType.Decimal, TasaSpreadOriginal)
            db.AddInParameter(dbCommand, "@p_FechaIni", DbType.Decimal, FechaIni)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
            db.AddInParameter(dbCommand, "@p_DifDias", DbType.Decimal, DifDias)
            db.AddInParameter(dbCommand, "@p_Amortizac", DbType.Decimal, Amortizac)
            db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, TasaCupon)
            db.AddInParameter(dbCommand, "@p_BaseCupon", DbType.String, BaseCupon)
            db.AddInParameter(dbCommand, "@p_DiasPago", DbType.String, DiasPago)
            db.AddInParameter(dbCommand, "@p_fechaRealInicial", DbType.Decimal, fechaRealInicial)
            db.AddInParameter(dbCommand, "@p_fechaRealFinal", DbType.Decimal, fechaRealFinal)
            db.AddInParameter(dbCommand, "@p_AmortizacConsolidado", DbType.Decimal, AmortizacConsolidado)
            db.AddInParameter(dbCommand, "@p_MontoInteres", DbType.Decimal, MontoInteres)
            db.AddInParameter(dbCommand, "@p_MontoAmortizacion", DbType.Decimal, MontoAmortizacion)
            db.AddInParameter(dbCommand, "@p_NominalRestante", DbType.Decimal, NominalRestante)
            db.AddInParameter(dbCommand, "@p_TasaSpread", DbType.Decimal, TasaSpread)

            db.AddInParameter(dbCommand, "@p_FechaLiborOriginal", DbType.Decimal, If(FechaLiborOriginal = 0, DBNull.Value, FechaLiborOriginal))
            db.AddInParameter(dbCommand, "@p_FechaLibor", DbType.Decimal, If(FechaLibor = 0, DBNull.Value, FechaLibor))
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BorrarBono_Swap(ByVal CodigoOrden As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_BorrarBono_Swap")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertaBono_Swap(ByVal CodigoOrden As String, ByVal CodigoIsin As String, ByVal CodigoNemonico As String, ByVal Nominal As Decimal, ByVal Unidades As Decimal, ByVal FechaVencimiento As Decimal, ByVal importeVenta As Decimal)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_InsertaBono_Swap")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_Nominal", DbType.Decimal, Nominal)
            db.AddInParameter(dbCommand, "@p_Unidades", DbType.Decimal, Unidades)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, FechaVencimiento)
            db.AddInParameter(dbCommand, "@p_ImporteVenta", DbType.Decimal, importeVenta)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConsultaOrdenSwapBono(ByVal CodigoPortafolio As String, ByVal FechaOperacion As Decimal, ByVal CodigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ConsultaOrdenSwapBono")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            Return db.ExecuteDataSet(dbCommand)
        End Using
    End Function
    Public Function ConsultaBono_Swap(ByVal CodigoOrden As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ConsultaBono_Swap")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "BonoSwap")
        Return oReporte.Tables(0)
    End Function
    Public Function ConsultaCuponera_Bono_Swap(ByVal CodigoOrden As String, ByVal OrdenGenera As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ConsultaCuponera_Bono_Swap")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_OrdenGenera", DbType.String, OrdenGenera)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "CuponeraSwap")
        Return oReporte.Tables(0)
    End Function
    Public Sub AnulaOrdenInversion(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("sp_SIT_AnularOrdenInversion")
        Try
            db.AddInParameter(comando, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_CodigoOrden", DbType.String, codigoOrden)
            db.AddInParameter(comando, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(comando, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(comando)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub ConfirmaOrdenInversion(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal CodigoIsin As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("sp_SIT_ConfirmaOrdenInversion")
        Try
            db.AddInParameter(comando, "@p_CodigoOrden", DbType.String, codigoOrden)
            db.AddInParameter(comando, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.AddInParameter(comando, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(comando, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(comando)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Function ValidaISIN(ByVal CodigoIsin As String, ByVal CodigoOrden As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ValidaISIN")
        db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddOutParameter(dbCommand, "@p_Existe", DbType.Decimal, 1)
        db.ExecuteDataSet(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Existe"), Integer)
    End Function
    Public Function ConsultaOrden(ByVal CodigoPortafolio As String, ByVal FechaOperacion As Decimal, ByVal CodigoOrden As String, ByVal CodigoOperacion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ConsultaOrden")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, CodigoOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "Orden")
        Return oReporte.Tables(0)
    End Function
    Public Function Insertar_DPZRenovacionCabecera(ByVal CodigoOrden As String, ByVal CodigoOperacion As String, ByVal CodigoModelo As String, ByVal FechaRelacion As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_DPZRenovacionCabecera")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, CodigoModelo)
        db.AddInParameter(dbCommand, "@p_FechaRelacion", DbType.Decimal, FechaRelacion)
        db.AddOutParameter(dbCommand, "@p_CodigoCabecera", DbType.Int32, 100)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_CodigoCabecera"), Integer)
    End Function
    Public Sub Insertar_DPZRenovacionDetalle(ByVal CodigoCabecera As Integer, ByVal CodigoOrden As String, ByVal CodigoOperacion As String, ByVal FechaRelacion As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_DPZRenovacionDetalle")
        db.AddInParameter(dbCommand, "@p_CodigoCabecera", DbType.Int32, CodigoCabecera)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_FechaRelacion", DbType.Decimal, FechaRelacion)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub Borrar_DPZRenovacion(ByVal CodigoOrden As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_DPZRenovacion")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function Codigo_DPZRenovacionCabecera(ByVal CodigoOrden As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Codigo_DPZRenovacionCabecera")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddOutParameter(dbCommand, "@p_CodigoCabecera", DbType.Int32, 100)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_CodigoCabecera"), Integer)
    End Function

    Public Sub Libera_Renovacion(ByVal CodigoOrden As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Libera_Renovacion")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    'OT 10238 - 07/04/2017 - Carlos Espejo
    'Descripcion: Funcion para validar Codigo Valor
    Public Function ValidarCodigoValor(ByVal CodigoNemonico As String, ByVal CodigoTercero As String, ByVal CodigoTipoCupon As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Validacion_CodigoValor")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, CodigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddOutParameter(dbCommand, "@p_Cantidad", DbType.Int32, 1)
        db.ExecuteNonQuery(dbCommand)
        Return (CType(db.GetParameterValue(dbCommand, "@p_Cantidad"), Integer))
    End Function
    'OT 10238 - 07/04/2017 - Carlos Espejo
    'Descripcion: Funcion para validar Codigo Valor
    Public Sub RecalculaMontoInversion(ByVal CodigoIsin As String, ByVal CodigoPortafolioSBS As String, ByVal FechaOperacionActual As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_RecalculaMontoInversion")
        db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacionActual", DbType.Decimal, FechaOperacionActual)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
    Public Function ListarOIEjecutadasConfirmacion(ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal nroOrden As String, ByVal fechaOperacion As Decimal, ByVal tipoInstrumento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ListarOIEjecutadasConfirmacion")
        dbCommand.CommandTimeout = 1020  'HDG 20110905
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, codigoFondo)
        db.AddInParameter(dbCommand, "@p_NumeroOrden", DbType.String, nroOrden)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_tipoInstrumento", DbType.String, tipoInstrumento)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda tipoInstrumento | 02/07/18 
#Region "SWAP"
    Public Sub InsertarOI_DetalleSwap(ByVal objOI As OrdenInversion_DetalleSWAPBE)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_InsertarOI_DetalleSWAP")
        AgregarParametros_detalleSWAP(db, dbCommand, objOI)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub ModificarOI_DetalleSwap(ByVal objOI As OrdenInversion_DetalleSWAPBE)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ModificarOI_DetalleSWAP")
        AgregarParametros_detalleSWAP(db, dbCommand, objOI)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub OrdenInversion_BorrarOI_Error_SWAP(ByVal CodigoPortafolio As String, ByVal CodigoOrden As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_BorrarOI_Error_SWAP")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.ExecuteNonQuery(dbCommand)
    End Sub

#Region " /* Funciones Personalizadas*/"
    Private Sub AgregarParametros_detalleSWAP(ByRef db As Database, ByVal dbCommand As DbCommand, ByVal objOI As OrdenInversion_DetalleSWAPBE)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, objOI.CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, objOI.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaIniLeg1", DbType.Decimal, objOI.FechaIniLeg1)
        db.AddInParameter(dbCommand, "@p_FechaFinLeg1", DbType.Decimal, objOI.FechaFinLeg1)
        db.AddInParameter(dbCommand, "@p_FechaIniLeg2", DbType.Decimal, objOI.FechaIniLeg2)
        db.AddInParameter(dbCommand, "@p_FechaFinLeg2", DbType.Decimal, objOI.FechaFinLeg2)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaLeg1", DbType.String, objOI.CodigoMonedaLeg1)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaLeg2", DbType.String, objOI.CodigoMonedaLeg2)
        db.AddInParameter(dbCommand, "@p_TipoCambioSpot", DbType.Decimal, objOI.TipoCambioSpot)
        db.AddInParameter(dbCommand, "@p_TasaInteresLeg1", DbType.Decimal, objOI.TasaInteresLeg1)
        db.AddInParameter(dbCommand, "@p_TasaFlotanteLeg1", DbType.String, objOI.TasaFlotanteLeg1)
        db.AddInParameter(dbCommand, "@p_TasaInteresLeg2", DbType.Decimal, objOI.TasaInteresLeg2)
        db.AddInParameter(dbCommand, "@p_TasaFlotanteLeg2", DbType.String, objOI.TasaFlotanteLeg2)
        db.AddInParameter(dbCommand, "@p_PeriodicidadLeg1", DbType.String, objOI.PeriodicidadLeg1)
        db.AddInParameter(dbCommand, "@p_PeriodicidadLeg2", DbType.String, objOI.PeriodicidadLeg2)
        db.AddInParameter(dbCommand, "@p_AmortizacionLeg1", DbType.String, objOI.AmortizacionLeg1)
        db.AddInParameter(dbCommand, "@p_AmortizacionLeg2", DbType.String, objOI.AmortizacionLeg2)
        db.AddInParameter(dbCommand, "@p_BaseDiasLeg1", DbType.String, objOI.BaseDiasLeg1)
        db.AddInParameter(dbCommand, "@p_BaseAniosLeg1", DbType.String, objOI.BaseAniosLeg1)
        db.AddInParameter(dbCommand, "@p_BaseDiasLeg2", DbType.String, objOI.BaseDiasLeg2)
        db.AddInParameter(dbCommand, "@p_BaseAniosLeg2", DbType.String, objOI.BaseAniosLeg2)

        db.AddInParameter(dbCommand, "@p_DiaTLeg1", DbType.Int32, If(objOI.DiatLeg1 > 0, DBNull.Value, objOI.DiatLeg1))
        db.AddInParameter(dbCommand, "@p_DiaTLeg2", DbType.Int32, If(objOI.DiaTLeg2 > 0, DBNull.Value, objOI.DiaTLeg2))

        db.AddInParameter(dbCommand, "@p_TasaLiborLeg1", DbType.Decimal, objOI.TasaLiborLeg1)
        db.AddInParameter(dbCommand, "@p_TasaLiborLeg2", DbType.Decimal, objOI.TasaLiborLeg2)

    End Sub
    Public Sub InicializarOrdenInversion_DetalleSWAP(ByRef oRowSWAP As OrdenInversion_DetalleSWAPBE)
        oRowSWAP.CodigoOrden = String.Empty
        oRowSWAP.CodigoPortafolioSBS = String.Empty
        oRowSWAP.FechaIniLeg1 = DECIMAL_NULO
        oRowSWAP.FechaFinLeg1 = DECIMAL_NULO
        oRowSWAP.FechaIniLeg2 = DECIMAL_NULO
        oRowSWAP.FechaFinLeg2 = DECIMAL_NULO
        oRowSWAP.TipoCambioSpot = DECIMAL_NULO
        oRowSWAP.TasaInteresLeg1 = DECIMAL_NULO
        oRowSWAP.TasaFlotanteLeg1 = String.Empty
        oRowSWAP.TasaInteresLeg2 = DECIMAL_NULO
        oRowSWAP.TasaFlotanteLeg2 = String.Empty
        oRowSWAP.PeriodicidadLeg1 = DECIMAL_NULO
        oRowSWAP.AmortizacionLeg1 = DECIMAL_NULO
        oRowSWAP.AmortizacionLeg2 = DECIMAL_NULO
        oRowSWAP.BaseDiasLeg1 = String.Empty
        oRowSWAP.BaseAniosLeg1 = String.Empty
        oRowSWAP.BaseDiasLeg2 = String.Empty
        oRowSWAP.BaseAniosLeg2 = String.Empty
        oRowSWAP.DiaTLeg1 = 0
        oRowSWAP.DiaTLeg2 = 0
    End Sub

#End Region
    'INICIO | ZOLUXIONES | RCOLONIA | OT11698 - OC10284 | Obtener las unidades negociadas del día | 06/12/18 
    Public Function ObtenerUnidadesNegociadasDiaT(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal CodigoMnemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ObtenerUnidadesNegociadasDiaT")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            Return db.ExecuteDataSet(dbCommand)
        End Using
    End Function
    'FIN | ZOLUXIONES | RCOLONIA | OT11698 - OC10284 | Obtener las unidades negociadas del día | 06/12/18 
#End Region
    Public Function SeleccionarOrden_OrdenPreOrden(ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Seleccionar_CodigoOrdenyPreOrden")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Sub ActualizaPrecioLimpioSucio(ByVal pCodigoNemonico As String, ByVal pPrecio As Decimal)
        Dim strCodigoOI As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_Actualizar_PrecioLimpioSucio")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, pCodigoNemonico)
        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, pPrecio)

        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Function ConstitucionesdelDiaDPZRenovacion(ByVal CodigoPortafolioSBS As String, ByVal CodigoMnemonico As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.sp_SIT_ConstitucionesdelDiaDPZRenovacion")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ConstitucionesDPZDiaRenovacion")
        Return oReporte.Tables(0)
    End Function

    Public Function VencimientoRenovacion(ByVal CodigoPortafolioSBS As String, ByVal CodigoTercero As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.sp_SIT_Lista_Vencimientos_OR")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "VencimientosRenovacion")
        Return oReporte.Tables(0)
    End Function

    Public Function ListarOIConfirmadasAgrupados(ByVal codigoFondo As String, ByVal fechaOperacion As Integer, ByVal codigoMercado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_AccionesConfirmadas")
            dbCommand.CommandTimeout = 1020  'HDG 20110905
            db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, codigoFondo)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Int32, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, codigoMercado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function AgruparDesagruparAccionesOI(ByVal CodigoOrden As String, ByVal Opcion As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_AgrupadoAccionesOI")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_Opcion", DbType.String, Opcion)
        db.AddOutParameter(dbCommand, "@p_Valida", DbType.String, 200)
        db.ExecuteNonQuery(dbCommand)

        Return CType(db.GetParameterValue(dbCommand, "@p_Valida"), String)
    End Function

    Public Function ValidaOI_Agrupada(ByVal CodigoOrden As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ValidaAgrupado")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddOutParameter(dbCommand, "@p_Valida", DbType.Int32, 10)
        db.ExecuteNonQuery(dbCommand)

        Return CType(db.GetParameterValue(dbCommand, "@p_Valida"), Integer)
    End Function

End Class
