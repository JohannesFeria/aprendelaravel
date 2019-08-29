Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
''' <summary>
''' Clase para el acceso de los datos para PrevOrdenInversion tabla.
''' </summary>
Public Class PrevOrdenInversionDAM
    Private sqlCommand As String = ""
    Private oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
    Private oRowOp As PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow
    Dim DECIMAL_NULO As Decimal = -1
    Public Sub New()
    End Sub
#Region " /* Funciones No Transaccionales */ "
    Public Function SeleccionarPorFiltro(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strCodigoClaseInstrumento As String,
    ByVal strCodigoTipoInstrumentoSBS As String, ByVal strCodigoNemonico As String, ByVal strOperador As String, ByVal strEstado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarPorFiltro_sura")
            db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, strCodigoClaseInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, strCodigoTipoInstrumentoSBS)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodigoNemonico)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, strOperador)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, strEstado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarDetallePreOrdenInversion(ByVal codigoPreOrden As Integer, ByVal codigoportafolio As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_seleccionarDetalle")
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Int32, codigoPreOrden)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoportafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionarDetallePreOrdenInversion(ByVal codigoPreOrden As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_Portafolio_SURA")
            db.AddInParameter(dbCommand, "@CodigoProvOrden", DbType.Int32, codigoPreOrden)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltroFuturo(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strCodigoClaseInstrumento As String,
    ByVal strCodigoTipoInstrumentoSBS As String, ByVal strCodigoNemonico As String, ByVal strOperador As String, ByVal strEstado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_Futuro_SeleccionFiltro")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, strCodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, strCodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodigoNemonico)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, strOperador)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, strEstado)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarOperaciones(ByVal decCodigoPrevOrden As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarOperaciones")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorCodigoOrden(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarPorCodigoOrden")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarOperadores() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarOperadores")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarValidacionExcesos(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_ValidarExcesos_sura")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        dbCommand.CommandTimeout = 1200
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function GenerarReporte(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_GenerarReporte_sura")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        dbCommand.CommandTimeout = 1200
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarTemporal() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarTemporal")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function GenerarOrdenInversion_Sura(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet,
    ByVal claseInstrumento As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_PrevOrdenInversion_GenerarOI_Sura")
            db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_claseInstrumento", DbType.String, claseInstrumento)
            db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function GenerarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet,
    ByVal claseInstrumento As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_PrevOrdenInversion_GenerarOI")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_claseInstrumento", DbType.String, claseInstrumento)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor(ByVal strNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarCaracValor")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strNemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor(ByVal p_Nemonico As String, ByVal p_FechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarCaracValor")
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, p_Nemonico)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ValidarOperaciones(ByVal decCodigoPrevOrden As Decimal, ByVal decCantidadOperacion As Decimal, ByVal bolValida As Boolean) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ValidarOperaciones_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, decCantidadOperacion)
        db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolValida)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
    End Function
    Public Function ObtenerExcesosTrader(ByVal strTipoRenta As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerExcesosTrader_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ExcesosTrader(ByVal strCodigoGrupLimTrader As String, ByVal strTipoRenta As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ExcesosTrader_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, strCodigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ValidarAprobacion(ByVal decCodigoPrevOrden As Decimal) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ValidarAprobacion_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
        db.ExecuteDataSet(dbCommand)
        bolResult = CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
        Return bolResult
    End Function
    Public Function ObtenerEstadoPrevOrdenInversion(ByVal decCodigoPrevOrden As Decimal) As String 'Agregado por JH 25042019
        Dim strResult As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerEstado_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        strResult = CType(db.ExecuteScalar(dbCommand), String)
        Return strResult
    End Function
    Public Function SeleccionarPreExtorno(ByVal decCodigoPrevOrden As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPreExtorno_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ValidaExtorno(ByVal decCodigoPrevOrden As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ValidaExtorno_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarImprimir_PrevOrdenInversion(ByVal decCodigoPrevOrden As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarImprimir_PrevOrdenInversion")

        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function GenerarReporteConFirmas(ByVal tipoRenta As String, ByVal fechaOperacion As Decimal, Optional ByVal strClaseInstrumento As String = "",
    Optional ByVal strCategoriaReporte As String = "", Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteOperacionesMasivas_SeleccionarDocFirma")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, tipoRenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodCategReporte", DbType.String, strCategoriaReporte)
        db.AddInParameter(dbCommand, "@p_ClaseInstrumento", DbType.String, strClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region
#Region "66056 - Modificacion: JZAVALA"
    ''' <summary>66056 - JZAVALA.
    ''' REPORTE EXCEL CONSOLIDADO.
    ''' </summary>
    ''' <param name="strTipoRenta"></param>
    ''' <param name="decFechaOperacion"></param>
    ''' <param name="strCodigoClaseInstrumento"></param>
    ''' <param name="strCodigoTipoInstrumentoSBS"></param>
    ''' <param name="strCodigoNemonico"></param>
    ''' <param name="strOperador"></param>
    ''' <param name="strEstado"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerarReporteConsolidado(ByVal decFechaOperacion As Decimal, ByVal strCodigoClaseInstrumento As String, ByVal strCodigoTipoInstrumentoSBS As String,
    ByVal strCodigoNemonico As String, ByVal strOperador As String, ByVal strEstado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_GenerarReporteConsolidado")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    ''' <summary>66056 - JZAVALA.
    ''' REPORTE EXCEL FUTURO.
    ''' </summary>
    ''' <param name="fechaIni"></param>
    ''' <param name="fechaFin"></param>
    ''' <param name="Portafolio"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ListarOperacionesCajaFuturos(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, _
    ByVal strCodigoOperacion As String, ByVal strMoneda As String, ByVal strCodigoMercado As String, ByVal strNumeroCuenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_SeleccionarPorFiltroFuturo")
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, strCodigoOperacion)
        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, strMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, strCodigoMercado)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, strNumeroCuenta)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteFuturos")
        Return oReporte
    End Function
#End Region
#Region " /* Funciones Transaccionales */ "
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function Insertar(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal strTipoRenta As String, ByVal dataRequest As DataSet,
    ByVal dtdetalle As DataTable) As Boolean
        Insertar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_PrevOrdenInversion_Insertar_detalle")
            Dim i As Integer
            Dim detalleXml As String = ""
            detalleXml = "<Detalles>"
            For i = 0 To dtdetalle.Rows.Count - 1
                detalleXml &= "<Detalle>"
                detalleXml &= "<CodigoPortafolio>" & dtdetalle.Rows(i)("CodigoPortafolio").ToString.Trim & "</CodigoPortafolio>"
                detalleXml &= "<Asignacion>" & Decimal.Parse(dtdetalle.Rows(i)("Asignacion")).ToString.Trim & "</Asignacion>"
                detalleXml &= "</Detalle>"
            Next
            detalleXml &= "</Detalles>"
            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.Rows(0), PrevOrdenInversionBE.PrevOrdenInversionRow)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oRow.FechaOperacion)
            db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, oRow.HoraOperacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_AsignacionF1", DbType.Decimal, oRow.AsignacionF1)
            db.AddInParameter(dbCommand, "@p_AsignacionF2", DbType.Decimal, oRow.AsignacionF2)
            db.AddInParameter(dbCommand, "@p_AsignacionF3", DbType.Decimal, oRow.AsignacionF3)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRow.Estado)
            db.AddInParameter(dbCommand, "@p_TipoCondicion", DbType.String, IIf(oRow.TipoCondicion = "", DBNull.Value, oRow.TipoCondicion))
            db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, IIf(oRow.CodigoPlaza = "", DBNull.Value, oRow.CodigoPlaza))
            db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, IIf(oRow.Cantidad = -1, DBNull.Value, oRow.Cantidad))
            db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oRow.Precio = -1, DBNull.Value, oRow.Precio))
            db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, IIf(oRow.MontoNominal = -1, DBNull.Value, oRow.MontoNominal))
            db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, IIf(oRow.CantidadOperacion = -1, DBNull.Value, oRow.CantidadOperacion))
            db.AddInParameter(dbCommand, "@p_PrecioOperacion", DbType.Decimal, IIf(oRow.PrecioOperacion = -1, DBNull.Value, oRow.PrecioOperacion))
            db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, IIf(oRow.MontoOperacion = -1, DBNull.Value, oRow.MontoOperacion))
            db.AddInParameter(dbCommand, "@p_MedioNegociacion", DbType.String, IIf(oRow.MedioNegociacion = "", DBNull.Value, oRow.MedioNegociacion))
            db.AddInParameter(dbCommand, "@p_Tasa", DbType.String, IIf(oRow.Tasa = -1, DBNull.Value, oRow.Tasa))
            db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, IIf(oRow.FechaLiquidacion = -1, DBNull.Value, oRow.FechaLiquidacion))
            db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, IIf(oRow.TipoTasa = "", DBNull.Value, oRow.TipoTasa))
            db.AddInParameter(dbCommand, "@p_IndPrecioTasa", DbType.String, IIf(oRow.IndPrecioTasa = "", DBNull.Value, oRow.IndPrecioTasa))
            db.AddInParameter(dbCommand, "@p_IntervaloPrecio", DbType.Decimal, IIf(oRow.IntervaloPrecio = -1, DBNull.Value, oRow.IntervaloPrecio))
            db.AddInParameter(dbCommand, "@p_MonedaNegociada", DbType.String, IIf(oRow.MonedaNegociada = "", DBNull.Value, oRow.MonedaNegociada))
            db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, IIf(oRow.Moneda = "", DBNull.Value, oRow.Moneda))
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, IIf(oRow.CodigoTipoTitulo = "", DBNull.Value, oRow.CodigoTipoTitulo))
            db.AddInParameter(dbCommand, "@p_ModalidadForward", DbType.String, IIf(oRow.ModalidadForward = "", DBNull.Value, oRow.ModalidadForward))
            db.AddInParameter(dbCommand, "@p_PrecioFuturo", DbType.Decimal, IIf(oRow.PrecioFuturo = -1, DBNull.Value, oRow.PrecioFuturo))
            db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, IIf(oRow.CodigoMotivo = "", DBNull.Value, oRow.CodigoMotivo))
            db.AddInParameter(dbCommand, "@p_FechaContrato", DbType.Decimal, IIf(oRow.FechaContrato = -1, DBNull.Value, oRow.FechaContrato))
            db.AddInParameter(dbCommand, "@p_ClaseInstrumentoFx", DbType.String, IIf(oRow.ClaseInstrumentoFx = "", DBNull.Value, oRow.ClaseInstrumentoFx))
            db.AddInParameter(dbCommand, "@p_Fixing", DbType.Decimal, IIf(oRow.Fixing = -1, DBNull.Value, oRow.Fixing))
            If oRow.IsCodigoContactoNull Then
                db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
            End If
            If oRow.IsTipoFondoNull Then
                db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, oRow.TipoFondo)
            End If
            If oRow.IsTipoTramoNull Then
                db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oRow.TipoTramo)
            End If
            If oRow.IsHoraEjecucionNull Then
                db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, oRow.HoraEjecucion)
            End If
            If oRow.IsVencimientoAnoNull Then
                db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, oRow.VencimientoAno)
            End If
            If oRow.IsVencimientoMesNull Then
                db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, oRow.VencimientoMes)
            End If
            If oRow.IsTotalExposicionNull Then
                db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, oRow.TotalExposicion)
            End If
            db.AddInParameter(dbCommand, "@p_porcentaje", DbType.String, oRow.Porcentaje)
            db.AddInParameter(dbCommand, "@p_detalle", DbType.Xml, detalleXml)

            If oRow.IsInteresCorridoNull Then
                db.AddInParameter(dbCommand, "@p_InteresCorrido", DbType.Decimal, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_InteresCorrido", DbType.Decimal, oRow.InteresCorrido)
            End If

            If oRow.IsTipoValorizacionNull Then
                db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, oRow.TipoValorizacion)
            End If

            If oRow.IsFicticiaNull Then
                db.AddInParameter(dbCommand, "@p_Ficticia", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_Ficticia", DbType.String, oRow.Ficticia)
            End If

            If oRow.IsRegulaSBSNull Then
                db.AddInParameter(dbCommand, "@p_RegulaSBS", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_RegulaSBS", DbType.String, oRow.RegulaSBS)
            End If

            db.ExecuteNonQuery(dbCommand)
            Insertar = True
        End Using
    End Function
    'OT10795 - Fin
    Public Function Modificar_Sura(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal dataRequest As DataSet) As String
        Dim i As Integer = 0
        For i = 0 To oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_PrevOrdenInversion_Modificar_Sura")
            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.Rows(i), PrevOrdenInversionBE.PrevOrdenInversionRow)
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.String, oRow.CodigoPrevOrden)
            db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, oRow.HoraOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_TipoCondicion", DbType.String, IIf(oRow.TipoCondicion = "", DBNull.Value, oRow.TipoCondicion))
            db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, IIf(oRow.CodigoPlaza = "", DBNull.Value, oRow.CodigoPlaza))
            db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, IIf(oRow.Cantidad = -1, DBNull.Value, oRow.Cantidad))
            db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oRow.Precio = -1, DBNull.Value, oRow.Precio))
            db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, IIf(oRow.MontoNominal = -1, DBNull.Value, oRow.MontoNominal))
            db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, IIf(oRow.CantidadOperacion = -1, DBNull.Value, oRow.CantidadOperacion))
            db.AddInParameter(dbCommand, "@p_PrecioOperacion", DbType.Decimal, IIf(oRow.PrecioOperacion = -1, DBNull.Value, oRow.PrecioOperacion))
            db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, IIf(oRow.MontoOperacion = -1, DBNull.Value, oRow.MontoOperacion))
            db.AddInParameter(dbCommand, "@p_MedioNegociacion", DbType.String, IIf(oRow.MedioNegociacion = "", DBNull.Value, oRow.MedioNegociacion))
            db.AddInParameter(dbCommand, "@p_Tasa", DbType.String, IIf(oRow.Tasa = -1, DBNull.Value, oRow.Tasa))
            db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, IIf(oRow.FechaLiquidacion = -1, DBNull.Value, oRow.FechaLiquidacion))
            db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, IIf(oRow.TipoTasa = "", DBNull.Value, oRow.TipoTasa))
            db.AddInParameter(dbCommand, "@p_IntervaloPrecio", DbType.Decimal, IIf(oRow.IntervaloPrecio = -1, DBNull.Value, oRow.IntervaloPrecio))
            db.AddInParameter(dbCommand, "@p_MonedaNegociada", DbType.String, IIf(oRow.MonedaNegociada = "", DBNull.Value, oRow.MonedaNegociada))
            db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, IIf(oRow.Moneda = "", DBNull.Value, oRow.Moneda))
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, IIf(oRow.CodigoTipoTitulo = "", DBNull.Value, oRow.CodigoTipoTitulo))
            db.AddInParameter(dbCommand, "@p_ModalidadForward", DbType.String, IIf(oRow.ModalidadForward = "", DBNull.Value, oRow.ModalidadForward))
            db.AddInParameter(dbCommand, "@p_PrecioFuturo", DbType.Decimal, IIf(oRow.PrecioFuturo = -1, DBNull.Value, oRow.PrecioFuturo))
            db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, IIf(oRow.CodigoMotivo = "", DBNull.Value, oRow.CodigoMotivo))
            db.AddInParameter(dbCommand, "@p_FechaContrato", DbType.Decimal, IIf(oRow.FechaContrato = -1, DBNull.Value, oRow.FechaContrato))
            db.AddInParameter(dbCommand, "@p_ClaseInstrumentoFx", DbType.String, IIf(oRow.ClaseInstrumentoFx = "", DBNull.Value, oRow.ClaseInstrumentoFx))
            db.AddInParameter(dbCommand, "@p_IndPrecioTasa", DbType.String, IIf(oRow.IndPrecioTasa = "", DBNull.Value, oRow.IndPrecioTasa))
            db.AddInParameter(dbCommand, "@p_Fixing", DbType.Decimal, IIf(oRow.Fixing = -1, DBNull.Value, oRow.Fixing))
            If oRow.IsCodigoContactoNull Then
                db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
            End If
            If oRow.IsTipoFondoNull Then
                db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, oRow.TipoFondo)
            End If
            If oRow.IsTipoTramoNull Then
                db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oRow.TipoTramo)
            End If
            If oRow.IsHoraEjecucionNull Then
                db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, oRow.HoraEjecucion)
            End If
            If oRow.IsVencimientoAnoNull Then
                db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, oRow.VencimientoAno)
            End If
            If oRow.IsVencimientoMesNull Then
                db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, oRow.VencimientoMes)
            End If
            If oRow.IsTotalExposicionNull Then
                db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, oRow.TotalExposicion)
            End If
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function
    Public Function Insertar(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal strTipoRenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_PrevOrdenInversion_Insertar")
        oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.Rows(0), PrevOrdenInversionBE.PrevOrdenInversionRow)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oRow.FechaOperacion)
        db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, oRow.HoraOperacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_AsignacionF1", DbType.Decimal, oRow.AsignacionF1)
        db.AddInParameter(dbCommand, "@p_AsignacionF2", DbType.Decimal, oRow.AsignacionF2)
        db.AddInParameter(dbCommand, "@p_AsignacionF3", DbType.Decimal, oRow.AsignacionF3)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRow.Estado)
        db.AddInParameter(dbCommand, "@p_TipoCondicion", DbType.String, IIf(oRow.TipoCondicion = "", DBNull.Value, oRow.TipoCondicion))
        db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, IIf(oRow.CodigoPlaza = "", DBNull.Value, oRow.CodigoPlaza))
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, IIf(oRow.Cantidad = -1, DBNull.Value, oRow.Cantidad))
        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oRow.Precio = -1, DBNull.Value, oRow.Precio))
        db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, IIf(oRow.MontoNominal = -1, DBNull.Value, oRow.MontoNominal))
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, IIf(oRow.CantidadOperacion = -1, DBNull.Value, oRow.CantidadOperacion))
        db.AddInParameter(dbCommand, "@p_PrecioOperacion", DbType.Decimal, IIf(oRow.PrecioOperacion = -1, DBNull.Value, oRow.PrecioOperacion))
        db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, IIf(oRow.MontoOperacion = -1, DBNull.Value, oRow.MontoOperacion))
        db.AddInParameter(dbCommand, "@p_MedioNegociacion", DbType.String, IIf(oRow.MedioNegociacion = "", DBNull.Value, oRow.MedioNegociacion))
        db.AddInParameter(dbCommand, "@p_Tasa", DbType.String, IIf(oRow.Tasa = -1, DBNull.Value, oRow.Tasa))
        db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, IIf(oRow.FechaLiquidacion = -1, DBNull.Value, oRow.FechaLiquidacion))
        db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, IIf(oRow.TipoTasa = "", DBNull.Value, oRow.TipoTasa))
        db.AddInParameter(dbCommand, "@p_IndPrecioTasa", DbType.String, IIf(oRow.IndPrecioTasa = "", DBNull.Value, oRow.IndPrecioTasa))
        db.AddInParameter(dbCommand, "@p_IntervaloPrecio", DbType.Decimal, IIf(oRow.IntervaloPrecio = -1, DBNull.Value, oRow.IntervaloPrecio))
        db.AddInParameter(dbCommand, "@p_MonedaNegociada", DbType.String, IIf(oRow.MonedaNegociada = "", DBNull.Value, oRow.MonedaNegociada))
        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, IIf(oRow.Moneda = "", DBNull.Value, oRow.Moneda))
        db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, IIf(oRow.CodigoTipoTitulo = "", DBNull.Value, oRow.CodigoTipoTitulo))
        db.AddInParameter(dbCommand, "@p_ModalidadForward", DbType.String, IIf(oRow.ModalidadForward = "", DBNull.Value, oRow.ModalidadForward))
        db.AddInParameter(dbCommand, "@p_PrecioFuturo", DbType.Decimal, IIf(oRow.PrecioFuturo = -1, DBNull.Value, oRow.PrecioFuturo))
        db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, IIf(oRow.CodigoMotivo = "", DBNull.Value, oRow.CodigoMotivo))
        db.AddInParameter(dbCommand, "@p_FechaContrato", DbType.Decimal, IIf(oRow.FechaContrato = -1, DBNull.Value, oRow.FechaContrato))
        db.AddInParameter(dbCommand, "@p_ClaseInstrumentoFx", DbType.String, IIf(oRow.ClaseInstrumentoFx = "", DBNull.Value, oRow.ClaseInstrumentoFx))
        db.AddInParameter(dbCommand, "@p_Fixing", DbType.Decimal, IIf(oRow.Fixing = -1, DBNull.Value, oRow.Fixing)) 'CMB REQ 67089 20130319
        If oRow.IsCodigoContactoNull Then
            db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
        End If
        If oRow.IsTipoFondoNull Then
            db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, oRow.TipoFondo)
        End If
        If oRow.IsTipoTramoNull Then
            db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oRow.TipoTramo)
        End If
        If oRow.IsHoraEjecucionNull Then
            db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, oRow.HoraEjecucion)
        End If
        If oRow.IsVencimientoAnoNull Then
            db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, oRow.VencimientoAno)
        End If
        If oRow.IsVencimientoMesNull Then
            db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, oRow.VencimientoMes)
        End If
        If oRow.IsTotalExposicionNull Then
            db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, oRow.TotalExposicion)
        End If
        db.AddInParameter(dbCommand, "@p_porcentaje", DbType.String, oRow.Porcentaje)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function insertarDetalle(ByVal codprevorden As Integer, ByVal codigoportafolio As String, ByVal asignacion As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_insertarDetalle")
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Int32, codprevorden)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoportafolio)
            db.AddInParameter(dbCommand, "@p_Asignacion", DbType.Decimal, asignacion)
            Return db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function Modificar(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim i As Integer = 0
        For i = 0 To oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_PrevOrdenInversion_Modificar")
                oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.Rows(i), PrevOrdenInversionBE.PrevOrdenInversionRow)
                db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.String, oRow.CodigoPrevOrden)
                db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, oRow.HoraOperacion)
                db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
                db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
                db.AddInParameter(dbCommand, "@p_AsignacionF1", DbType.Decimal, oRow.AsignacionF1)
                db.AddInParameter(dbCommand, "@p_AsignacionF2", DbType.Decimal, oRow.AsignacionF2)
                db.AddInParameter(dbCommand, "@p_AsignacionF3", DbType.Decimal, oRow.AsignacionF3)
                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.AddInParameter(dbCommand, "@p_TipoCondicion", DbType.String, IIf(oRow.TipoCondicion = "", DBNull.Value, oRow.TipoCondicion))
                db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, IIf(oRow.CodigoPlaza = "", DBNull.Value, oRow.CodigoPlaza))
                db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, IIf(oRow.Cantidad = -1, DBNull.Value, oRow.Cantidad))
                db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oRow.Precio = -1, DBNull.Value, oRow.Precio))
                db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, IIf(oRow.MontoNominal = -1, DBNull.Value, oRow.MontoNominal))
                db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, IIf(oRow.CantidadOperacion = -1, DBNull.Value, oRow.CantidadOperacion))
                db.AddInParameter(dbCommand, "@p_PrecioOperacion", DbType.Decimal, IIf(oRow.PrecioOperacion = -1, DBNull.Value, oRow.PrecioOperacion))
                db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, IIf(oRow.MontoOperacion = -1, DBNull.Value, oRow.MontoOperacion))
                db.AddInParameter(dbCommand, "@p_MedioNegociacion", DbType.String, IIf(oRow.MedioNegociacion = "", DBNull.Value, oRow.MedioNegociacion))
                db.AddInParameter(dbCommand, "@p_Tasa", DbType.String, IIf(oRow.Tasa = -1, DBNull.Value, oRow.Tasa))
                db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, IIf(oRow.FechaLiquidacion = -1, DBNull.Value, oRow.FechaLiquidacion))
                db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, IIf(oRow.TipoTasa = "", DBNull.Value, oRow.TipoTasa))
                db.AddInParameter(dbCommand, "@p_IntervaloPrecio", DbType.Decimal, IIf(oRow.IntervaloPrecio = -1, DBNull.Value, oRow.IntervaloPrecio))
                db.AddInParameter(dbCommand, "@p_MonedaNegociada", DbType.String, IIf(oRow.MonedaNegociada = "", DBNull.Value, oRow.MonedaNegociada))
                db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, IIf(oRow.Moneda = "", DBNull.Value, oRow.Moneda))
                db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, IIf(oRow.CodigoTipoTitulo = "", DBNull.Value, oRow.CodigoTipoTitulo))
                db.AddInParameter(dbCommand, "@p_ModalidadForward", DbType.String, IIf(oRow.ModalidadForward = "", DBNull.Value, oRow.ModalidadForward))
                db.AddInParameter(dbCommand, "@p_PrecioFuturo", DbType.Decimal, IIf(oRow.PrecioFuturo = -1, DBNull.Value, oRow.PrecioFuturo))
                db.AddInParameter(dbCommand, "@p_CodigoMotivo", DbType.String, IIf(oRow.CodigoMotivo = "", DBNull.Value, oRow.CodigoMotivo))
                db.AddInParameter(dbCommand, "@p_FechaContrato", DbType.Decimal, IIf(oRow.FechaContrato = -1, DBNull.Value, oRow.FechaContrato))
                db.AddInParameter(dbCommand, "@p_ClaseInstrumentoFx", DbType.String, IIf(oRow.ClaseInstrumentoFx = "", DBNull.Value, oRow.ClaseInstrumentoFx))
                db.AddInParameter(dbCommand, "@p_IndPrecioTasa", DbType.String, IIf(oRow.IndPrecioTasa = "", DBNull.Value, oRow.IndPrecioTasa))
                db.AddInParameter(dbCommand, "@p_Fixing", DbType.Decimal, IIf(oRow.Fixing = -1, DBNull.Value, oRow.Fixing))
                db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, IIf(oRow.FechaOperacion = -1, DBNull.Value, oRow.FechaOperacion))

                If oRow.IsCodigoContactoNull Then
                    db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
                End If
                If oRow.IsTipoFondoNull Then
                    db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_TipoFondo", DbType.String, oRow.TipoFondo)
                End If
                If oRow.IsTipoTramoNull Then
                    db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oRow.TipoTramo)
                End If
                If oRow.IsHoraEjecucionNull Then
                    db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_HoraEjecucion", DbType.String, oRow.HoraEjecucion)
                End If
                If oRow.IsVencimientoAnoNull Then
                    db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_VencimientoAno", DbType.String, oRow.VencimientoAno)
                End If
                If oRow.IsVencimientoMesNull Then
                    db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_VencimientoMes", DbType.String, oRow.VencimientoMes)
                End If
                If oRow.IsTotalExposicionNull Then
                    db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_TotalExposicion", DbType.Decimal, oRow.TotalExposicion)
                End If
                db.AddInParameter(dbCommand, "@p_porcentaje", DbType.String, oRow.Porcentaje)

                If oRow.IsInteresCorridoNull Then
                    db.AddInParameter(dbCommand, "@p_InteresCorrido", DbType.Decimal, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_InteresCorrido", DbType.Decimal, oRow.InteresCorrido)
                End If

                If oRow.IsTipoValorizacionNull Then
                    db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, oRow.TipoValorizacion)
                End If

                db.ExecuteNonQuery(dbCommand)
            End Using
        Next
        Modificar = True
    End Function
    'OT10795 - Fin
    Public Function modificarDetalle(ByVal codigoprevorden As Integer, ByVal codigoportafolio As String, ByVal asignacion As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_actualizarDetalle")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Int32, codigoprevorden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoportafolio)
        db.AddInParameter(dbCommand, "@p_Asignacion", DbType.Decimal, asignacion)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Eliminar(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_PrevOrdenInversion_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function eliminarDetalle(ByVal codigoprevorden As Integer, ByVal codigoportafolio As String) As Boolean
        eliminarDetalle = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PrevOrdenInversion_eliminarDetalle")
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, codigoprevorden)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoportafolio)
            db.ExecuteNonQuery(dbCommand)
            eliminarDetalle = True
        End Using
    End Function
    'OT10795 - Fin
    Public Function InsertarOperacion(ByVal oPrevOrdenInversionDetalleBE As PrevOrdenInversionDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_PrevOrdenInversionDetalle_Insertar")
        oRowOp = CType(oPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.Rows(0), PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow)
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, oRowOp.CodigoPrevOrden)
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, oRowOp.CantidadOperacion)
        db.AddInParameter(dbCommand, "@p_PrecioOperacion", DbType.Decimal, oRowOp.PrecioOperacion)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ModificarOperacion(ByVal OPrevOrdenInversionDetalleBE As PrevOrdenInversionDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim i As Integer = 0
        For i = 0 To OPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_PrevOrdenInversionDetalle_Modificar")
            oRowOp = CType(OPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.Rows(i), PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow)
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrdenDet", DbType.Decimal, oRowOp.CodigoPrevOrdenDet)
            db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, oRowOp.CantidadOperacion)
            db.AddInParameter(dbCommand, "@p_PrecioOperacion", DbType.Decimal, oRowOp.PrecioOperacion)
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function
    Public Function EliminarOperacion(ByVal decCodigoPrevOrdenDet As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_PrevOrdenInversionDetalle_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrdenDet", DbType.Decimal, decCodigoPrevOrdenDet)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ProcesarEjecucion(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet, Optional ByVal decNProceso As Decimal = 0) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ProcesarEjecucion_PrevOrdenInversion")
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Sub InicializarPrevOrdenInversion(ByRef oRow As PrevOrdenInversionBE.PrevOrdenInversionRow)
        oRow.CodigoPrevOrden = DECIMAL_NULO
        oRow.Correlativo = DECIMAL_NULO
        oRow.FechaOperacion = DECIMAL_NULO
        oRow.HoraOperacion = ""
        oRow.UsuarioCreacion = ""
        oRow.CodigoNemonico = ""
        oRow.CodigoOperacion = ""
        oRow.Cantidad = DECIMAL_NULO
        oRow.Precio = DECIMAL_NULO
        oRow.TipoCondicion = ""
        oRow.CodigoTercero = ""
        oRow.CodigoPlaza = ""
        oRow.IntervaloPrecio = DECIMAL_NULO
        oRow.CantidadOperacion = DECIMAL_NULO
        oRow.PrecioOperacion = DECIMAL_NULO
        oRow.AsignacionF1 = DECIMAL_NULO
        oRow.AsignacionF2 = DECIMAL_NULO
        oRow.AsignacionF3 = DECIMAL_NULO
        oRow.MedioNegociacion = ""
        oRow.Tasa = DECIMAL_NULO
        oRow.Situacion = ""
        oRow.FechaCreacion = DECIMAL_NULO
        oRow.HoraCreacion = ""
        oRow.UsuarioModificacion = ""
        oRow.FechaModificacion = DECIMAL_NULO
        oRow.HoraModificacion = ""
        oRow.Host = ""
        oRow.Estado = ""
        oRow.FechaLiquidacion = DECIMAL_NULO
        oRow.TipoTasa = ""
        oRow.IndPrecioTasa = ""
        oRow.MontoOperacion = DECIMAL_NULO
        oRow.MontoNominal = DECIMAL_NULO
        oRow.MonedaNegociada = ""
        oRow.Moneda = ""
        oRow.CodigoTipoTitulo = ""
        oRow.ModalidadForward = ""
        oRow.PrecioFuturo = DECIMAL_NULO
        oRow.CodigoMotivo = ""
        oRow.FechaContrato = DECIMAL_NULO
        oRow.ClaseInstrumentoFx = ""
        oRow.Fixing = DECIMAL_NULO
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Funcion para aprobar limites trader
    Public Function AprobarNegociacionTrader(ByVal dataRequest As DataSet, ByVal tipoRenta As String, ByVal decNProceso As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_upd_AprobarNegociacionTrader")
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, tipoRenta)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Funcion para listar aprobadores segun grupo
    Public Function AprobarNegociacionTrader(ByVal CodigoGrupLimTrader As Integer, ByVal CodigoPortafolio As String, ByVal PorcentajeExcedido As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_AprobadoresTrader")
        db.AddInParameter(dbCommand, "@P_CodigoGrupLimTrader", DbType.Int32, CodigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_PorcentajeExcedido", DbType.Decimal, PorcentajeExcedido)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function AprobarNegociacion(ByVal dataRequest As DataSet, Optional ByVal usuarioAprob As String = "", Optional ByVal codigoPrevOrden As Decimal = Nothing,
    Optional ByVal tipoRenta As String = "", Optional ByVal decNProceso As Decimal = 0) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_AprobarNegociacion_PrevOrdenInversion")
        If usuarioAprob = "" And codigoPrevOrden = Nothing Then
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        Else
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioAprob)
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DateTime.Today))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DateTime.Now))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, "")
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, codigoPrevOrden)
            db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, tipoRenta)
        End If
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ProcesarSwapDivisa(ByVal decFechaOperacion As Decimal, ByVal decProceso As Decimal) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_VincularSwap_SwapDivisa")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        db.AddOutParameter(dbCommand, "@p_ValidaProceso", DbType.Boolean, bolResult)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decProceso)
        db.ExecuteNonQuery(dbCommand)
        bolResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaProceso"), Boolean)
        Return bolResult
    End Function
    Public Function Extornar(ByVal decCodigoPrevOrden As Decimal, ByVal strCodigoMotivo As String, ByVal strComentario As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Extornar_PrevOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        db.AddInParameter(dbCommand, "@p_CodigoMotivoCambio", DbType.String, strCodigoMotivo)
        db.AddInParameter(dbCommand, "@p_ComentariosE", DbType.String, strComentario)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DateTime.Today))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DateTime.Now))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_ValidaExtorno", DbType.Boolean, bolResult)
        db.ExecuteNonQuery(dbCommand)
        bolResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaExtorno"), Boolean)
        Return bolResult
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Sub ActualizaSeleccionPrevOrden(ByVal decCodigoPrevOrden As Decimal, ByVal Flag As String)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_PrevOrdenInversion_FlagSeleccion")
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
            db.AddInParameter(dbCommand, "@p_Flag", DbType.String, Flag)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT10795 - Fin
    Public Function DesAprobarNegociacion(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_PrevOrdenInversion_Desaprobar")
        db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, decCodigoPrevOrden)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function InsertarProcesoMasivo(ByVal pUsuario As String) As Decimal
        Dim NProceso As Decimal = 0
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_ProcesoMasivo_Insertar")
            db.AddInParameter(dbCommand, "@p_UsuarioProceso", DbType.String, pUsuario)
            db.AddOutParameter(dbCommand, "@p_NProceso", DbType.Decimal, 0)
            db.ExecuteDataSet(dbCommand)
            NProceso = CType(db.GetParameterValue(dbCommand, "@p_NProceso"), Decimal)
        End Using
        Return NProceso
    End Function
    Public Function EliminarProcesoMasivo(ByVal pNProceso As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_ProcesoMasivo_Eliminar")
            db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, pNProceso)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    'OT10795 - Fin
    Public Function TruncarProcesoMasivo() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ProcesoMasivo_Truncar")
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function InsertarTrazabilidad(ByVal oTrazabilidadOperacionBE As TrazabilidadOperacionBE, ByVal pProceso As String, ByVal dataRequest As DataSet) As Boolean
        Dim oRow As TrazabilidadOperacionBE.TrazabilidadOperacionRow
        Dim i As Integer = 0
        For i = 0 To oTrazabilidadOperacionBE.TrazabilidadOperacion.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_TrazabilidadOperaciones_Insertar")
            oRow = CType(oTrazabilidadOperacionBE.TrazabilidadOperacion.Rows(i), TrazabilidadOperacionBE.TrazabilidadOperacionRow)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oRow.FechaOperacion)
            db.AddInParameter(dbCommand, "@p_Correlativo", DbType.Decimal, oRow.Correlativo)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRow.Estado)
            db.AddInParameter(dbCommand, "@p_TipoOperacion", DbType.String, oRow.TipoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, oRow.CodigoPrevOrden)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, oRow.CodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, IIf(oRow.Cantidad = -1, DBNull.Value, oRow.Cantidad))
            db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oRow.Precio = -1, DBNull.Value, oRow.Precio))
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_CantidadEjecucion", DbType.Decimal, IIf(oRow.CantidadEjecucion = -1, DBNull.Value, oRow.CantidadEjecucion))
            db.AddInParameter(dbCommand, "@p_PrecioEjecucion", DbType.Decimal, IIf(oRow.PrecioEjecucion = -1, DBNull.Value, oRow.PrecioEjecucion))
            db.AddInParameter(dbCommand, "@p_AsignacionF1", DbType.Decimal, oRow.AsignacionF1)
            db.AddInParameter(dbCommand, "@p_AsignacionF2", DbType.Decimal, oRow.AsignacionF2)
            db.AddInParameter(dbCommand, "@p_AsignacionF3", DbType.Decimal, oRow.AsignacionF3)
            db.AddInParameter(dbCommand, "@p_ModoIngreso", DbType.String, oRow.ModoIngreso)
            db.AddInParameter(dbCommand, "@p_Proceso", DbType.String, pProceso)
            db.AddInParameter(dbCommand, "@p_MotivoCambio", DbType.String, oRow.MotivoCambio)
            db.AddInParameter(dbCommand, "@p_Comentarios", DbType.String, oRow.Comentarios)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function InsertarTrazabilidad_sura(ByVal oTrazabilidadOperacionBE As TrazabilidadOperacionBE, ByVal pProceso As String, ByVal dataRequest As DataSet) As Boolean
        InsertarTrazabilidad_sura = False
        Dim oRow As TrazabilidadOperacionBE.TrazabilidadOperacionRow
        Dim i As Integer = 0
        For i = 0 To oTrazabilidadOperacionBE.TrazabilidadOperacion.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_TrazabilidadOperaciones_Insertar_sura")
                oRow = CType(oTrazabilidadOperacionBE.TrazabilidadOperacion.Rows(i), TrazabilidadOperacionBE.TrazabilidadOperacionRow)
                db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oRow.FechaOperacion)
                db.AddInParameter(dbCommand, "@p_Correlativo", DbType.Decimal, oRow.Correlativo)
                db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRow.Estado)
                db.AddInParameter(dbCommand, "@p_TipoOperacion", DbType.String, oRow.TipoOperacion)
                db.AddInParameter(dbCommand, "@p_CodigoPrevOrden", DbType.Decimal, oRow.CodigoPrevOrden)
                db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, oRow.CodigoOrden)
                db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
                db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
                db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, IIf(oRow.Cantidad = -1, DBNull.Value, oRow.Cantidad))
                db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, IIf(oRow.Precio = -1, DBNull.Value, oRow.Precio))
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
                db.AddInParameter(dbCommand, "@p_CantidadEjecucion", DbType.Decimal, IIf(oRow.CantidadEjecucion = -1, DBNull.Value, oRow.CantidadEjecucion))
                db.AddInParameter(dbCommand, "@p_PrecioEjecucion", DbType.Decimal, IIf(oRow.PrecioEjecucion = -1, DBNull.Value, oRow.PrecioEjecucion))
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oRow.CodigoPortafolio)
                db.AddInParameter(dbCommand, "@p_Asignacion", DbType.Decimal, oRow.Asignacion)
                db.AddInParameter(dbCommand, "@p_ModoIngreso", DbType.String, oRow.ModoIngreso)
                db.AddInParameter(dbCommand, "@p_Proceso", DbType.String, pProceso)
                db.AddInParameter(dbCommand, "@p_MotivoCambio", DbType.String, oRow.MotivoCambio)
                db.AddInParameter(dbCommand, "@p_Comentarios", DbType.String, oRow.Comentarios)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            End Using
        Next
        InsertarTrazabilidad_sura = True
    End Function
    'OT10795 - Fin
    Public Function TrazabilidadOperaciones(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteTrazabilidadOperaciones")
        dbCommand.CommandTimeout = 1215
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.String, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "TrazabilidadOperaciones")
        Return oReporte
    End Function

    '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución
    Public Sub ActualizarTirNetaEnOrdenInversion(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal tirNeta As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarTirNeta")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_TirNeta", DbType.Decimal, tirNeta)

        db.ExecuteNonQuery(dbCommand)
    End Sub
    '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución

#End Region
End Class
