Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ReporteGestionDAM
    Public Sub New()

    End Sub

    Public Sub InsertForward(ByVal codigoPortafolio As String, ByVal fechaProceso As Int32, _
            ByVal codigoSBS As String, ByVal moneda As String, ByVal tipoMov As String, ByVal indicadorForward As String, _
            ByVal monto As Decimal, ByVal precio As Decimal, ByVal fechaVencimiento As Integer, ByVal plazo As Integer, _
            ByVal modalidad As String, ByVal tipoCambio As Decimal, ByVal indicadorCaja As String, ByVal plaza As String, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("usp_InsertAnexoIDI9")

        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@numFechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@nvcCodigoSBS", DbType.String, codigoSBS)
        db.AddInParameter(dbCommand, "@crhHoraNegociacion", DbType.String, "")
        db.AddInParameter(dbCommand, "@crhCodigoMonedaVenta", DbType.String, moneda)
        db.AddInParameter(dbCommand, "@crhMovimiento", DbType.String, tipoMov)
        db.AddInParameter(dbCommand, "@crhIndicadorForward", DbType.String, indicadorForward)
        db.AddInParameter(dbCommand, "@numMontoForward", DbType.Decimal, monto)
        db.AddInParameter(dbCommand, "@numPrecioTransaccion", DbType.Decimal, precio)
        db.AddInParameter(dbCommand, "@numFechaVencimiento", DbType.Int32, fechaVencimiento)
        db.AddInParameter(dbCommand, "@numPlazoVencimiento", DbType.Int32, plazo)
        db.AddInParameter(dbCommand, "@crhModalidad", DbType.String, modalidad)
        db.AddInParameter(dbCommand, "@numTipoCambio", DbType.Decimal, tipoCambio)
        db.AddInParameter(dbCommand, "@crhIndicadorCaja", DbType.String, indicadorCaja)
        db.AddInParameter(dbCommand, "@crhPlaza", DbType.String, plaza)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)


    End Sub
    Public Sub UpdateForward(ByVal codigoPortafolio As String, ByVal fechaProceso As Int32, ByVal consecutivo As Integer, _
            ByVal codigoSBS As String, ByVal moneda As String, ByVal tipoMov As String, ByVal indicadorForward As String, _
            ByVal monto As Decimal, ByVal precio As Decimal, ByVal fechaVencimiento As Integer, ByVal plazo As Integer, _
            ByVal modalidad As String, ByVal tipoCambio As Decimal, ByVal indicadorCaja As String, ByVal plaza As String, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("usp_UpdateAnexoIDI9")

        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@numFechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@numConsecutivo", DbType.Int32, consecutivo)
        db.AddInParameter(dbCommand, "@nvcCodigoSBS", DbType.String, codigoSBS)
        db.AddInParameter(dbCommand, "@crhHoraNegociacion", DbType.String, "")
        db.AddInParameter(dbCommand, "@crhCodigoMonedaVenta", DbType.String, moneda)
        db.AddInParameter(dbCommand, "@crhMovimiento", DbType.String, tipoMov)
        db.AddInParameter(dbCommand, "@crhIndicadorForward", DbType.String, indicadorForward)
        db.AddInParameter(dbCommand, "@numMontoForward", DbType.Decimal, monto)
        db.AddInParameter(dbCommand, "@numPrecioTransaccion", DbType.Decimal, precio)
        db.AddInParameter(dbCommand, "@numFechaVencimiento", DbType.Int32, fechaVencimiento)
        db.AddInParameter(dbCommand, "@numPlazoVencimiento", DbType.Int32, plazo)
        db.AddInParameter(dbCommand, "@crhModalidad", DbType.String, modalidad)
        db.AddInParameter(dbCommand, "@numTipoCambio", DbType.Decimal, tipoCambio)
        db.AddInParameter(dbCommand, "@crhIndicadorCaja", DbType.String, indicadorCaja)
        db.AddInParameter(dbCommand, "@crhPlaza", DbType.String, plaza)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)


    End Sub
    Public Function GetForward(ByVal codigoPortafolio As String, ByVal fechaProceso As Int32, ByVal consecutivo As Integer) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("usp_GetAnexoIDI9")

        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@numFechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@numConsecutivo", DbType.Int32, consecutivo)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte.Tables(0)

    End Function
    Public Function SeguimientoForwards(ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String, ByVal FechaVencimiento As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("SeguimientoForwards_Listar")

        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)
        db.AddInParameter(dbCommand, "@FechaVencimiento", DbType.Decimal, FechaVencimiento)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function VerificaInformacionAnexo(ByVal CodigoPortafolioSBS As String, ByVal FechaProceso As Decimal, ByVal NroAnexo As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VerificaInformacionAnexo_Listar")

        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@FechaProceso", DbType.Decimal, FechaProceso)
        db.AddInParameter(dbCommand, "@NroAnexo", DbType.String, NroAnexo)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function GeneraInformacionAnexo(ByVal CodigoPortafolioSBS As String, ByVal FechaProceso As Decimal, ByVal NroAnexo As String, ByVal Reproceso As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GeneracionInformediario_Listar")

        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@FechaProceso", DbType.Decimal, FechaProceso)
        db.AddInParameter(dbCommand, "@NroAnexo", DbType.String, NroAnexo)
        db.AddInParameter(dbCommand, "@Reproceso", DbType.String, Reproceso)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ReporteAnexoIDI3A(ByVal fechaIni As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteAnexoIDI3A")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function ReporteAnexoIDI3B(ByVal fechaIni As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteAnexoIDI3B")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function ReporteAnexoIDI6(ByVal fechaIni As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteAnexoIDI6")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function ReporteAnexoIDI8(ByVal fechaIni As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteAnexoIDI8")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function ReporteAnexoIDI7(ByVal fechaIni As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteAnexoIDI7")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function ReporteAnexoIDI9(ByVal fechaIni As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteAnexoIDI9")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function ListaTipoCambio(ByVal fechaCarga As Decimal, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioHorizontalTMP_Seleccionar")
        dbCommand.CommandTimeout = 1020  'HDG 20110831

        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, fechaCarga)
        db.AddInParameter(dbCommand, "@Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function ListaPrecios(ByVal fechaCarga As Decimal, ByVal tipoSocio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteListaPrecios")
        dbCommand.CommandTimeout = 1020  'HDG 20110831
        db.AddInParameter(dbCommand, "@p_fechaCarga", DbType.Decimal, fechaCarga)
        db.AddInParameter(dbCommand, "@p_socio", DbType.String, tipoSocio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function ListaPrecios2(ByVal fechaCarga As Decimal, ByVal tipoSocio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteListaPrecios2")
        dbCommand.CommandTimeout = 1020 'RGF 20081223    'HDG 20110831

        db.AddInParameter(dbCommand, "@p_fechaCarga", DbType.Decimal, fechaCarga)
        db.AddInParameter(dbCommand, "@p_socio", DbType.String, tipoSocio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    'FQS 20110526 OT63063 REQ 17
    Public Function ListaPrecios3(ByVal fechaCarga As Decimal, ByVal tipoSocio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_VectorVariacion")
        dbCommand.CommandTimeout = 1020  'HDG 20110831

        db.AddInParameter(dbCommand, "@p_fechaCarga", DbType.Decimal, fechaCarga)
        db.AddInParameter(dbCommand, "@p_socio", DbType.String, tipoSocio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function SaldoInstrumentos(ByVal fechaValoracion As Decimal, ByVal portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_SaldosInstrumentoEmpresa")

        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaValoracion)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'RGF 20090108 Ya existe un metodo q invoca a este SP
    'Public Function Utilidad(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal Mercado As String) As DataSet

    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteUtilidad")

    '    db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
    '    db.AddInParameter(dbCommand, "@p_fechaFin", DbType.Decimal, fechaFin)
    '    db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

    '    Dim oReporte As New DataSet
    '    db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
    '    Return oReporte

    'End Function

    Public Function FlujoCaja(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal Mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteFlujoCaja")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_fechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function StockForwards(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal Mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteStockForwards")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_fechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function StockBCR(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal Mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteStockBCR")

        db.AddInParameter(dbCommand, "@p_fechaIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_fechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function DuracionCarteraDetalle(ByVal CodigoPortafolioSBS As String, ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_DetalleDuraciones")
        Dim objeto As New DataSet

        db.AddInParameter(dbCommand, "@FechaValoracion", DbType.Decimal, fechaValoracion)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.LoadDataSet(dbCommand, objeto, "ReporteGestion")
        Return objeto
    End Function
    Public Function DuracionCarteraResumen(ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_DetalleResumenDuraciones")
        Dim objeto As New DataSet

        db.AddInParameter(dbCommand, "@FechaValoracion", DbType.Decimal, fechaValoracion)
        db.LoadDataSet(dbCommand, objeto, "ReporteGestion")
        Return objeto
    End Function
    Public Function CarteraTipoRenta(ByVal fechaValorizacion As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraTipoRenta")

        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function

    Public Function CarteraEmisor(ByVal fechaValorizacion As Decimal, ByVal Mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraEmisor")

        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, Mercado)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function CarteraMoneda(ByVal fechaValorizacion As Decimal, ByVal Portafolio As String, ByVal Mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraMoneda")

        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_portafolio", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_mercado", DbType.String, Mercado)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function CarteraCategoriaInstrumento(ByVal fechaValorizacion As Decimal, ByVal Mercado As String, ByVal Portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraCategoriaInstrumento")

        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, Mercado)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function CarteraSectorEmpresarial(ByVal fechaValorizacion As Decimal, ByVal Mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraSector")

        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, Mercado)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function CarteraPlazoDetalle(ByVal fechaValorizacion As Decimal, ByVal Portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraPlazoDetalle")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, Portafolio)


        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function CarteraPlazoResumen(ByVal fechaValorizacion As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraPlazoResumen")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function CarteraRiesgoDetalle(ByVal fechaValorizacion As Decimal, ByVal Portafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraRiesgoDetalle")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)


        Dim oReporte As New DataSet
        'db.ExecuteDataSet(dbCommand, oReporte, "ReporteGestion")
        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function CarteraRiesgoResumen(ByVal fechaValorizacion As Decimal, ByVal mercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraRiesgoResumen")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)
        db.AddInParameter(dbCommand, "@p_mercado", DbType.String, mercado)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte

    End Function
    Public Function CarteraExterior(ByVal fechaValorizacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CarteraExterior")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, fechaValorizacion)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function GenerarReporteForwards(ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_StockForwards")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, Portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function GenerarReporteCertificadosDeposito(ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_StockCertificadosDeposito")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, Portafolio)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function GenerarReporteCompBenchmark(ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompBenchmarkSBS")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaValoracion", DbType.Decimal, fechaFin)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function

    Public Function GenerarReporteUtilidad(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteUtilidad")
        dbCommand.CommandTimeout = 300 'RGF 20090116

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function

    Public Function GenerarReporteEncaje(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal Nemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteUtilidad_Encaje")
        dbCommand.CommandTimeout = 300 'RGF 20090116

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, Nemonico)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function

    'RGF 20080627
    Public Function GenerarReporteComposicionCarteraInstrumentoEmpresa(ByVal fecha As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_CompCarteraInstrumentoEmpresa")

        db.AddInParameter(dbCommand, "@p_portafolio", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        dbCommand.CommandTimeout = 600 'OAB 20100106
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestionCCIE")
        Return oReporte
    End Function

    Public Function GenerarReporteFlujoCaja(ByVal fecha As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteFlujoCaja")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaActual", DbType.Decimal, fecha)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function GenerarReporteBcos(ByVal fecha As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteBcos")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function GenerarReporteBmidas(ByVal fecha As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Gestion_Reportes_ReporteBmidas")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function ConsultarVecTipoCambio_Por_Fecha(ByVal sFecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_ConsultarFecha")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, sFecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ComposicionCartera(ByVal fecha As Decimal, ByVal Escenario As String, CodigoPortafolioSBS As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_Gestion_CompCartera")
        dbCommand.CommandTimeout = 1020  'HDG 20110912
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_escenario", DbType.String, Escenario)
        db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function MonedaGestion(ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Gestion_Moneda")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function CarteraUnidades(ByVal fecha As Decimal, ByVal tipo As String, ByVal CodigoMercado As String, ByVal TipoRenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Gestion_CarteraUnidades")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, tipo) 'RGF 20081119
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado) 'LETV 20090105
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, TipoRenta) 'LETV 20090105
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ReporteExterior(ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Reporte_Exterior")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function CarteraFondo3(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_Gestion_CarteraFondo3")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function CarteraOperacion(ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Gestion_Operaciones")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function CarteraEngrapado(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_Gestion_Engrapado")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function UnidadesxFecha(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Gestion_ComposicionxFondo")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function CarteraComPraVenta(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal TipoInstrumento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Gestion_CompraVenta")
        dbCommand.CommandTimeout = 1020  'HDG 20110912
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoInstrumento", DbType.String, IIf(TipoInstrumento = "Todos", "", TipoInstrumento))
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function VectorSerie(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal TipoInstrumento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_Gestion_VectorSerie")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoInstrumento", DbType.String, IIf(TipoInstrumento = "Todos", "", TipoInstrumento))
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarConstitucionForwards(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ConstitucionForwards_Listar")
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin) 'OAB 20091007
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteConstitucionForwards")
        Return oReporte
    End Function
    Public Function DividendosRebatesLiberadas(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_DividendosRebatesLiberadas")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteDividendosRebatesLiberadas")
        Return oReporte
    End Function
    Public Function RentabilidadFondoEncaje(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_UtilidadFondoEncaje")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteDividendosRebatesLiberadas")
        Return oReporte
    End Function
    Public Function LiquidacionComisionesAgentes(ByVal CodigoIntermediario As String, _
                                                 ByVal FechaInicio As Decimal, _
                                                 ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Gestion_Reportes_ComisionesAgentes")
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, CodigoIntermediario)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteLiquidacionComisionesAgentes")
        Return oReporte
    End Function
    Public Function ConversionAcciones(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal CodigoIntermediaro As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_ConversionAcciones")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, CodigoIntermediaro)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteConversionAcciones")
        Return oReporte
    End Function
    Public Function PosicionMoneda(ByVal Fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_PosicionMoneda")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "PosicionMoneda")
        Return oReporte
    End Function
    Public Function PosicionMonedaCajaLocal(ByVal Fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_PosicionMonedaCajaLocal")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "CajaLocal")
        Return oReporte.Tables(0)
    End Function
    Public Function PosicionMonedaForward(ByVal Fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_PosicionMonedaForward")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "CajaLocal")
        Return oReporte.Tables(0)
    End Function
    Public Function OperacionInversion(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_OperacionInversion")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.String, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "OperacionInversion")
        Return oReporte
    End Function
    Public Function OperacionInversionPatrimonio(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_OperacionInversionPatrimonio")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.String, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "OperacionInversionPatrimonio")
        Return oReporte.Tables(0)
    End Function
    Public Function ReporteResumenRentabilidadEncajeTotalInstrumentos(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteResumenRentabilidadEncajeTotalInstrumentos")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function LineasCreditoxEmisor(ByVal Fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteLineasCreditoxEmisor")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "LineasCredito")
        Return oReporte
    End Function
    Public Function DetallePosicionBancos(ByVal CodigoPortafolio As String, ByVal CodigoIntermediario As String, _
                                                 ByVal FechaInicio As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_DetallePosicionBancoxTipoInst")
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Emisor", DbType.String, CodigoIntermediario)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "DetallePosicionBancos")
        Return oReporte
    End Function
    Public Function DetInteresesDividendos(ByVal CodigoPortafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteInteresesyDividendos")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "InteresesDividendos")
        Return oReporte
    End Function
    Public Function RentabilidadEncajeFondo(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerRentabFondoEncajexFondo")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "ReporteGestion")
        Return oReporte
    End Function
    Public Function LineasContraparte(ByVal Fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteLineasContraparte")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "LineasContraparte")
        Return oReporte
    End Function
    Public Function LineasSettlement(ByVal Fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteLineasSettlement")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "LineasSettlement")
        Return oReporte
    End Function
    Public Function Anexo_Swaps_RenovacionFWD(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_AnexoI_II_Swaps_RenovacionFWD")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "AnexoI_II_Swaps_RenovacionFWD")
        Return oReporte
    End Function
    Public Function reporteRentabilidad(codigoPortafolio As String, fechaOperacion As Decimal, estado As String) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_Sit_Rpt_Rentabilidad")
        comando.CommandTimeout = 1020
        Try
            db.AddInParameter(comando, "@p_codigoPortafolioSbs", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(comando, "@p_estado", DbType.String, estado)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function
    'OT10927 - 21/11/2017 - Hanz Cocchi. Generación de reporte de flujos de rentabilidad
    Public Function reporteRentabilidad_Flujos(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using comando As DbCommand = db.GetStoredProcCommand("Pr_Sit_Rpt_Rentabilidad_Flujos")
            comando.CommandTimeout = 1020
            db.AddInParameter(comando, "@p_codigoPortafolioSbs", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(comando, "@p_estado", DbType.String, "A")
            Using ds As DataSet = db.ExecuteDataSet(comando)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT10927 - Fin
    Public Function listarReporteValorCuota(codigoPortafolio As String, fechaInicial As Decimal, fechaFinal As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_Sit_listarValorCuota")
        Try
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fechaInicial", DbType.Decimal, fechaInicial)
            db.AddInParameter(comando, "@p_fechaFinal", DbType.Decimal, fechaFinal)

            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function listarReporteValorCuotaTotalPorFondo(fechaInicial As Decimal, fechaFinal As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_Sit_listarValorCuotaTotalPorFondo")
        Try
            db.AddInParameter(comando, "@p_fechaInicial", DbType.Decimal, fechaInicial)
            db.AddInParameter(comando, "@p_fechaFinal", DbType.Decimal, fechaFinal)

            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function listarReporteValorCuotaTotalPorFondoSeriado(ByVal codigoPortafolio As String, ByVal fechaInicial As Decimal, ByVal fechaFinal As Decimal) As DataSet
        Dim dt As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_Sit_listarValorCuotaTotalPorFondoSeriado")
        Try
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fechaInicial", DbType.Decimal, fechaInicial)
            db.AddInParameter(comando, "@p_fechaFinal", DbType.Decimal, fechaFinal)

            dt = db.ExecuteDataSet(comando)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function listarReporteCustodiaDPZ(codigoPortafolio As String, fecha As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_SitFondos_ReporteCustodiaDPZ")
        Try
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fecha", DbType.Decimal, fecha)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function listarReporteCustodiaForward(codigoPortafolio As String, fecha As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_SitFondos_ReporteCustodiaForward")
        Try
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fecha", DbType.Decimal, fecha)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function listarReporteCustodiaOpReporte(codigoPortafolio As String, fecha As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_SitFondos_ReporteCustodiaOpReporte")
        Try
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fecha", DbType.Decimal, fecha)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function listarReporteCustodiaTenencia(codigoPortafolio As String, fecha As Decimal)
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("Pr_SitFondos_ReporteCustodiaTenencias")
        Try
            db.AddInParameter(comando, "@p_codigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(comando, "@p_fecha", DbType.Decimal, fecha)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function ListarReporteValorCuotaLimite(ByVal CodigoPortafolioSBS As String, ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_LIS_ValorCuota")
        Try
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(comando, "@p_FechaInicial", DbType.Decimal, FechaInicial)
            db.AddInParameter(comando, "@p_FechaFinal", DbType.Decimal, FechaFinal)

            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function ListarReporteValorCuotaLimiteMandatos(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_LIS_ValorCuota_Mandatos")
        Try
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(comando, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)

            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function


    Public Function ListarCarteraFondo(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal, ByVal Escenario As String) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_LIS_CarteraFondo")
        Try
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(comando, "@p_FechaValoracion", DbType.Decimal, FechaValoracion)
            db.AddInParameter(comando, "@p_Escenario", DbType.String, Escenario)

            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function ListarCarteraFondoForward(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_LIS_CarteraFondoForward")
        Try
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(comando, "@p_FechaValoracion", DbType.Decimal, FechaValoracion)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function ListarOption(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_LIS_Option")
        Try
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(comando, "@p_FechaValoracion", DbType.Decimal, FechaValoracion)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function ListarTipoCambio(ByVal Fecha As Decimal) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim comando As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_LIS_TipoCambio")
        Try
            db.AddInParameter(comando, "@p_Fecha", DbType.Decimal, Fecha)
            dt = db.ExecuteDataSet(comando).Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If (comando IsNot Nothing) Then
                comando.Dispose()
                db = Nothing
            End If
        End Try
        Return dt
    End Function

    Public Function Reporte_VectorVariacion(ByVal FechaHoy As Decimal, ByVal FechaAyer As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_VectorVariacion")
            dbCommand.CommandTimeout = 1215
            db.AddInParameter(dbCommand, "@p_fechaHoy", DbType.Decimal, FechaHoy)
            db.AddInParameter(dbCommand, "@p_fechaAyer", DbType.Decimal, FechaAyer)
            Using oReporte As New DataSet
                db.LoadDataSet(dbCommand, oReporte, "VectorVariacion")
                Return oReporte
            End Using
        End Using
    End Function

    Public Function Reporte_ValorCuotaVariacion(ByVal FechaInicial As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_VectorVariacionPorcentual")
            dbCommand.CommandTimeout = 1215
            db.AddInParameter(dbCommand, "@p_fechaInicial", DbType.Decimal, FechaInicial)
            Dim oReporte As New DataSet
            db.LoadDataSet(dbCommand, oReporte, "VectorVariacion")
            Return oReporte
        End Using
    End Function

    Public Function Reporte_ValorCuotaVariacionFormula(ByVal MesVariacion As Integer, ByVal Alerta As Integer, ByVal FechaInicial As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_VectorVariacionPorcentual_Formula")
            dbCommand.CommandTimeout = 1215
            db.AddInParameter(dbCommand, "@p_fechaInicial", DbType.Decimal, FechaInicial)
            db.AddInParameter(dbCommand, "@p_MesesVariacion", DbType.Int32, MesVariacion)
            db.AddInParameter(dbCommand, "@p_Alerta", DbType.Int32, Alerta)
            Dim oReporte As New DataSet
            db.LoadDataSet(dbCommand, oReporte, "VectorVariacion")
            Return oReporte
        End Using
    End Function

    Public Function Reporte_Dividendo(ByVal CodigoPortafolio As Integer, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Lis_ReporteDividendo")
            dbCommand.CommandTimeout = 1215
            db.AddInParameter(dbCommand, "@P_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@P_FechaInicio", DbType.Decimal, FechaInicio)
            db.AddInParameter(dbCommand, "@P_FechaFin", DbType.Decimal, FechaFin)
            Dim oReporte As New DataSet
            db.LoadDataSet(dbCommand, oReporte, "Dividendo")
            Return oReporte
        End Using
    End Function
    'OT11004 - 21/12/2017 - Ian Pastor M.
    'Descripción: Obtiene las inversiones con sus comisiones
    Public Function ReporteComisionInvesiones(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Decimal, ByVal p_FechaFin As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_rpt_ReporteComisionInversiones")
            DbCommand.CommandTimeout = 1215
            db.AddInParameter(DbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(DbCommand, "@p_FechaInicio", DbType.Decimal, p_FechaInicio)
            db.AddInParameter(DbCommand, "@p_FechaFin", DbType.Decimal, p_FechaFin)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                ReporteComisionInvesiones = ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11004 - Fin
    Public Function ReporteConsolidado(ByVal p_CodigoPortafolio As String, ByVal p_Fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("ReporteGestion_ReporteConsolidado")
            DbCommand.CommandTimeout = 1215
            db.AddInParameter(DbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(DbCommand, "@p_FechaProceso", DbType.Decimal, p_Fecha)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                ReporteConsolidado = ds
            End Using
        End Using
    End Function
    Public Function ReporteOperacionesVencimientosOTC_Mandatos(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ReporteGestion_OperacionesVencimientosOTC")
            dbCommand.CommandTimeout = 1215
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Int32, p_FechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Int32, p_FechaFin)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ReporteOperacionesVencimientosOTC_Mandatos = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ReporteVencimientoOperaciones(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ReporteGestion_VencimientosOperaciones")
            dbCommand.CommandTimeout = 1215
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Int32, p_FechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Int32, p_FechaFin)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ReporteVencimientoOperaciones = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ListarSwap(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using comando As DbCommand = db.GetStoredProcCommand("SP_SIT_LIS_Swap")
            db.AddInParameter(comando, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(comando, "@p_FechaValoracion", DbType.Decimal, FechaValoracion)
            ListarSwap = db.ExecuteDataSet(comando).Tables(0)
        End Using
    End Function
End Class