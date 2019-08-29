Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class CuentasPorCobrarDAM
	
		Public Sub New()

		End Sub

    Public Function Insertar(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal egreso As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.Rows(0)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Ingresar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, cuentaPorCobrar.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, cuentaPorCobrar.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, cuentaPorCobrar.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, cuentaPorCobrar.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, cuentaPorCobrar.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, cuentaPorCobrar.Referencia)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, cuentaPorCobrar.Importe)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, cuentaPorCobrar.FechaOperacion)
        db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, cuentaPorCobrar.FechaIngreso)
        db.AddInParameter(dbCommand, "@p_egreso", DbType.String, egreso)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Sub Anular(ByVal CodigoCuenta As String, ByVal CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Anular")
        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, CodigoCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function Extornar(ByVal CodigoCuenta As String, ByVal CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Extornar")
        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, CodigoCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Sub ModificarFechaVencimiento(ByVal CodigoCuenta As String, ByVal CodigoPortafolio As String, ByVal fechaVencimiento As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_ModificarFecha")
            db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, CodigoCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, fechaVencimiento)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ObtenerNumeroCartaPortafolio(ByVal sPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ObtenerNumeroCarta_Seleccionar")

        db.AddInParameter(dbCommand, "@CodigoPortafolio", DbType.String, sPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Sub Liquidar(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal sCodigoContacto As String, ByVal dataRequest As DataSet, _
      ByVal bancoOrigen As String, ByVal bancoDestino As String, ByVal numeroCuentaDestino As String, ByVal Agrupado As String, ByVal ObservacionCarta As String, ByVal CodigoRelacion As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Liquidar")
            Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
            db.AddInParameter(dbCommand, "@p_NroOperacion", DbType.String, opCaja.NumeroOperacion)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, opCaja.FechaPago)
            db.AddInParameter(dbCommand, "@p_OperacionCaja", DbType.Int32, opCaja.CodigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
            db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, sNumeroCarta)
            db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
            db.AddInParameter(dbCommand, "@nvcBancoOrigen", DbType.String, bancoOrigen)
            db.AddInParameter(dbCommand, "@nvcBancoDestino", DbType.String, bancoDestino)
            db.AddInParameter(dbCommand, "@nvcNumeroCuentaDestino", DbType.String, numeroCuentaDestino)
            db.AddInParameter(dbCommand, "@nvcCodigoContacto", DbType.String, sCodigoContacto)
            db.AddInParameter(dbCommand, "@p_BancoMatrizOrigen", DbType.String, opCaja.BancoMatrizOrigen)
            db.AddInParameter(dbCommand, "@p_BancoMatrizDestino", DbType.String, opCaja.BancoMatrizDestino)
            db.AddInParameter(dbCommand, "@p_CodigoContactoIntermediario", DbType.String, opCaja.CodigoContactoIntermediario)
            db.AddInParameter(dbCommand, "@p_agrupado", DbType.String, Agrupado)
            db.AddInParameter(dbCommand, "@p_TasaImpuesto", DbType.Decimal, IIf(opCaja.TasaImpuesto = 0, DBNull.Value, opCaja.TasaImpuesto))
            db.AddInParameter(dbCommand, "@p_CodigoTerceroDestino", DbType.String, opCaja.CodigoTerceroDestino)
            db.AddInParameter(dbCommand, "@p_ObservacionCarta", DbType.String, ObservacionCarta)
            db.AddInParameter(dbCommand, "@p_ObservacionCartaDestino", DbType.String, IIf(String.IsNullOrEmpty(opCaja.ObservacionCartaDestino) = True, "", opCaja.ObservacionCartaDestino))
            db.AddInParameter(dbCommand, "@p_BancoGlosaDestino", DbType.String, opCaja.BancoGlosaDestino)
            db.AddInParameter(dbCommand, "@p_CodigoRelacion", DbType.String, CodigoRelacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub LiquidarNPH(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal dataRequest As DataSet, _
      ByVal bancoOrigen As String, ByVal bancoDestino As String, ByVal numeroCuentaDestino As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_LiquidarNPH")
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        db.AddInParameter(dbCommand, "@p_NroOperacion", DbType.String, opCaja.NumeroOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, opCaja.NumeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, opCaja.FechaPago)
        db.AddInParameter(dbCommand, "@p_OperacionCaja", DbType.Int32, opCaja.CodigoOperacionCaja)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, sNumeroCarta)
        db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
        db.AddInParameter(dbCommand, "@nvcBancoOrigen", DbType.String, bancoOrigen)
        db.AddInParameter(dbCommand, "@nvcBancoDestino", DbType.String, bancoDestino)
        db.AddInParameter(dbCommand, "@nvcNumeroCuentaDestino", DbType.String, numeroCuentaDestino)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub LiquidarDivisas(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal sCodigoContacto As String, ByVal Agrupado As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_LiquidarDivisas")
            Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
            db.AddInParameter(dbCommand, "@p_NroOperacion", DbType.String, opCaja.NumeroOperacion)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, opCaja.FechaPago)
            db.AddInParameter(dbCommand, "@p_OperacionCaja", DbType.Int32, opCaja.CodigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
            db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, sNumeroCarta)
            db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
            db.AddInParameter(dbCommand, "@nvcCodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@nvcCodigoContacto", DbType.String, sCodigoContacto)
            db.AddInParameter(dbCommand, "@p_BancoMatrizOrigen", DbType.String, opCaja.BancoMatrizOrigen)
            db.AddInParameter(dbCommand, "@p_BancoMatrizDestino", DbType.String, opCaja.BancoMatrizDestino)
            db.AddInParameter(dbCommand, "@p_CodigoContactoIntermediario", DbType.String, opCaja.CodigoContactoIntermediario)
            db.AddInParameter(dbCommand, "@p_agrupado", DbType.String, Agrupado)
            db.AddInParameter(dbCommand, "@p_ObservacionCartaDestino", DbType.String, IIf(String.IsNullOrEmpty(opCaja.ObservacionCartaDestino) = True, "", opCaja.ObservacionCartaDestino))
            db.AddInParameter(dbCommand, "@p_BancoGlosaDestino", DbType.String, opCaja.BancoGlosaDestino)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub LiquidarDivisasNPH(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_LiquidarDivisasNPH")
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        db.AddInParameter(dbCommand, "@p_NroOperacion", DbType.String, opCaja.NumeroOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, opCaja.NumeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, opCaja.FechaPago)
        db.AddInParameter(dbCommand, "@p_OperacionCaja", DbType.Int32, opCaja.CodigoOperacionCaja)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, sNumeroCarta)
        db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
        db.AddInParameter(dbCommand, "@nvcCodigoMoneda", DbType.String, opCaja.CodigoMoneda)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function SeleccionarPorFiltro(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal LiquidaFechaAnt As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, cuentaPorCobrar.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, cuentaPorCobrar.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, cuentaPorCobrar.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, cuentaPorCobrar.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, cuentaPorCobrar.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, "")
            db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, cuentaPorCobrar.Egreso)
            db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
            db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
            db.AddInParameter(dbCommand, "@p_LiquidaFechaAnt", DbType.String, LiquidaFechaAnt)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarPorFiltro = ds
            End Using
        End Using
    End Function
    Public Function SeleccionarAnularPorFiltro(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal LiquidaFechaAnt As String) As DataSet 'HDG OT 64767 20120222
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.Rows(0)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrarAnular_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, cuentaPorCobrar.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, cuentaPorCobrar.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, cuentaPorCobrar.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, cuentaPorCobrar.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, cuentaPorCobrar.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, cuentaPorCobrar.Egreso)
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_LiquidaFechaAnt", DbType.String, LiquidaFechaAnt)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarVencimientos(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal codigoClaseInstrumento As String, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.Rows(0)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarVencimientos2")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, cuentaPorCobrar.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, cuentaPorCobrar.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, cuentaPorCobrar.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, cuentaPorCobrar.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, cuentaPorCobrar.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarVencimientos2(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal codigoClaseInstrumento As String, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.Rows(0)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_CuentasPorCobrar_ReporteVencimientos")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, cuentaPorCobrar.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, cuentaPorCobrar.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, cuentaPorCobrar.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, cuentaPorCobrar.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, cuentaPorCobrar.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Sub IngresarPagoParcial(ByVal nroOperacion As String, ByVal numeroCuenta As String, ByVal codigoPortafolio As String, _
    ByVal importe As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Liquidar")
        db.AddInParameter(dbCommand, "@p_NroOperacion", DbType.String, nroOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    ''' <summary>
    ''' Selecciona un solo expediente de CuentasPorCobrar tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoCuenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    ''' <summary>
    ''' Selecciona expedientes de CuentasPorCobrar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorCodigoClaseCuenta")
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    ''' <summary>
    ''' Selecciona expedientes de CuentasPorCobrar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMercado"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorCobrar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoOrden"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoOrden(ByVal codigoOrden As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorCodigoOrden")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorCobrar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorCobrar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorCobrar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_SeleccionarPorCodigoPortafolio")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de CuentasPorCobrar tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Limite1() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrarPagar_Limite1")

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function OperacionesNoContabilizadas(ByVal fechaAsiento As Decimal, ByVal fondo As String, ByVal egreso As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrarPagar_OperacionesNoContabilizadas")
        dbCommand.CommandTimeout = 1020  'HDG 20110831
        db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fechaAsiento)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, fondo)
        db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, egreso)

        'Dim dsOperacionesNoContabilizadas As New DataSet
        'db.LoadDataSet(dbCommand, dsOperacionesNoContabilizadas, "OperacionesNoContabilizadas")
        'Return dsOperacionesNoContabilizadas
        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en CuentasPorCobrar tabla.
    ''' <summary>
    ''' <param name="codigoCuenta"></param>
    ''' <param name="referencia"></param>
    ''' <param name="importe"></param>
    ''' <param name="fechaIngreso"></param>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <param name="numeroCuenta"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <param name="fechaOperacion"></param>
    ''' <param name="situacion"></param>
    ''' <param name="fechaPago"></param>
    ''' <param name="tipoMovimiento"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="host"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="codigoOrden"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoCuenta As String, ByVal referencia As String, ByVal importe As Decimal, ByVal fechaIngreso As Decimal, ByVal codigoClaseCuenta As String, ByVal numeroCuenta As Decimal, ByVal codigoMoneda As String, ByVal codigoPortafolio As String, ByVal codigoMercado As String, ByVal codigoTipoOperacion As String, ByVal fechaOperacion As Decimal, ByVal situacion As String, ByVal fechaPago As Decimal, ByVal tipoMovimiento As String, ByVal usuarioCreacion As String, ByVal horaCreacion As String, ByVal fechaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal host As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoOrden As String, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)
        db.AddInParameter(dbCommand, "@p_Referencia", DbType.String, referencia)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_FechaIngreso", DbType.Decimal, fechaIngreso)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, fechaPago)
        db.AddInParameter(dbCommand, "@p_TipoMovimiento", DbType.String, tipoMovimiento)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoCuenta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_EliminarPorCodigoClaseCuenta")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_EliminarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoOrden(ByVal codigoOrden As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_EliminarPorCodigoOrden")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_EliminarPorCodigoTipoOperacion")
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_EliminarPorCodigoMoneda")
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    ''' <summary>
    ''' Elimina un expediente de CuentasPorCobrar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorCobrar_EliminarPorCodigoPortafolio")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ValidaLotesCuadradosParaCierre(ByVal fechaAsiento As Decimal, ByVal fondo As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CierreContable_ValidaLotes")
        dbCommand.CommandTimeout = 1020  'HDG 20110831
        db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fechaAsiento)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, fondo)
        db.AddParameter(dbCommand, "@lote", DbType.Int16, ParameterDirection.ReturnValue, "", DataRowVersion.Default, 0)    'HDG 20120927
        db.ExecuteNonQuery(dbCommand)
        Dim lote As Integer = db.GetParameterValue(dbCommand, "@lote")
        'Dim lote As Integer = db.ExecuteScalar(dbCommand)
        Select Case lote
            Case 0
                Return ""
            Case 1
                Return "Valorizacion"
            Case 2
                Return "C/V de Inversiones"
            Case 3
                Return "Cobranza y Cancelación"
        End Select

        Return lote
    End Function
    Public Function ObtenerEstadoOperacion(ByVal codigoOrden As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_ObtenerEstado")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddOutParameter(dbCommand, "@p_Estado", DbType.String, 5)
        db.ExecuteNonQuery(dbCommand)
        Return db.GetParameterValue(dbCommand, "@p_Estado")
    End Function
    Public Sub ActualizaCuentaMatriz(CodigoOperacionCaja As String, ByVal CodigoPortafolioSBS As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizaCuentaMatriz")
        db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se implementa proceso de automatización para suscripción de fondos CAPESTRen T + 1 | 01/08/18 
    Public Function SuscripcionFondos_Automatico(ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String, ByVal numeroCuenta As String, ByVal importe As Decimal, ByVal codigoTerceroOrigen As String, ByVal codigoClaseCuenta As String, ByVal fechaOperacion As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_SuscripcionFondos_Automatico")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_CodigoTerceroOrigen", DbType.String, codigoTerceroOrigen)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fechaOperacion)
        db.AddOutParameter(dbCommand, "@p_Result", DbType.Int32, 0)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Result"), Integer)
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se implementa proceso de automatización para suscripción de fondos CAPESTRen T + 1 | 01/08/18 
End Class