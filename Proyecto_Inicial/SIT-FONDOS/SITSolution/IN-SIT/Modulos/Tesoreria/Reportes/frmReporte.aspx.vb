Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Tesoreria_Reportes_frmReporte
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim reporte As String
            reporte = Request.QueryString("ClaseReporte")
            rep.Load(Server.MapPath(reporte & ".rpt"))
            Select Case (reporte)
                Case "ControlDeForwards" : reporteControlDeForwards()
                Case "MovimientosTotales" : reporteMovimientos()
                Case "ReporteEnvioCartas" : ReporteEnvioCartas()
                Case "SaldosNetos" : reporteSaldosNetos()
                Case "ReporteVencimientos" : reporteVencimientos()
                Case "ReporteMovimientosNeg" : ReporteMovimientosNeg()
                Case "ReporteFlujoEstimado" : ReporteFlujoEstimado()
                Case "DetalleSaldosBancarios" : reporteDetalleCuenta()
            End Select
            rep.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            CrystalReportViewer1.ReportSource = rep
            CrystalReportViewer1.BestFitPage = True
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
#Region "/* Funciones Personalizadas */"
    Private Sub reporteControlDeForwards()
    End Sub
    Private Sub reporteDetalleCuenta()
        Dim dsAux As New RepDetalleSaldos
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        Dim codPortafolio As String = ""
        Dim codClaseCuenta As String = ""
        Dim codBanco As String = ""
        Dim codMoneda As String = ""
        Dim fechaInicio As Decimal
        Dim portafolio As String = "Todos"
        Dim moneda As String = "Todos"
        Dim banco As String = "Todos"
        Dim clasecuenta As String = "Todos"
        Dim sFecha As String = ""
        Dim codMercado As String = ""
        Dim numerocuenta As String = ""

        Dim tablaParametros As Hashtable = Session("ParametrosReporteDetalleSaldos")
        If tablaParametros Is Nothing Then
            tablaParametros = New Hashtable
        End If

        If Not tablaParametros("codPortafolio") Is Nothing Then
            codPortafolio = tablaParametros("codPortafolio")
        End If
        If Not tablaParametros("codMoneda") Is Nothing Then
            codMoneda = tablaParametros("codMoneda")
        End If
        If Not tablaParametros("codBanco") Is Nothing Then
            codBanco = tablaParametros("codBanco")
        End If
        If Not tablaParametros("codClaseCuenta") Is Nothing Then
            codClaseCuenta = tablaParametros("codClaseCuenta")
        End If

        If Not tablaParametros("Portafolio") Is Nothing Then
            If tablaParametros("Portafolio") <> "" Then
                portafolio = tablaParametros("Portafolio")
            End If
        End If
        If Not tablaParametros("Banco") Is Nothing Then
            If tablaParametros("Banco") <> "" Then
                banco = tablaParametros("Banco")
            End If
        End If
        If Not tablaParametros("Moneda") Is Nothing Then
            If tablaParametros("Moneda") <> "" Then
                moneda = tablaParametros("Moneda")
            End If
        End If
        If Not tablaParametros("ClaseCuenta") Is Nothing Then
            If tablaParametros("ClaseCuenta") <> "" Then
                clasecuenta = tablaParametros("ClaseCuenta")
            End If
        End If

        If Not tablaParametros("Mercado") Is Nothing Then
            If tablaParametros("Mercado") <> "" Then
                codMercado = tablaParametros("codMercado")
            End If
        End If

        If Not tablaParametros("numerocuenta") Is Nothing Then
            If tablaParametros("numerocuenta") <> "" Then
                numerocuenta = tablaParametros("numerocuenta")
            End If
        End If

        fechaInicio = IIf(Request.QueryString("FechaInicio") Is Nothing, "0", Request.QueryString("FechaInicio"))
        roCuentaEconomica.CodigoPortafolioSBS = codPortafolio
        roCuentaEconomica.CodigoMoneda = codMoneda
        roCuentaEconomica.CodigoClaseCuenta = "" 'Modificado por LC 15082008
        roCuentaEconomica.CodigoTercero = codBanco
        roCuentaEconomica.FechaCreacion = fechaInicio
        roCuentaEconomica.CodigoMercado = codMercado
        roCuentaEconomica.NumeroCuenta = numerocuenta

        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)

        Dim rpt As Boolean
        Dim oSaldoBancario As New SaldosBancariosBM
        rpt = oSaldoBancario.ActualizaSaldosBancarios(codPortafolio, fechaInicio)

        Dim dsSaldos As DataSet = New SaldosBancariosBM().DetalleSaldosBancarios(dsCuentaEconomica, DatosRequest)
        CopiarTabla(dsSaldos.Tables(0), dsAux.DetalleSaldosBancarios)
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("FechaOperacion", IIf(fechaInicio = "0", Now.ToString("dd/MM/yyyy"), UIUtility.ConvertirFechaaString(fechaInicio)))

    End Sub
    Private Sub reporteSaldosNetos()
        Dim dsAux As New RepSaldosBancarios
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        Dim codPortafolio As String = ""
        Dim codClaseCuenta As String = ""
        Dim codBanco As String = ""
        Dim codMoneda As String = ""
        Dim fechaInicio As Decimal
        Dim portafolio As String = "Todos"
        Dim moneda As String = "Todos"
        Dim banco As String = "Todos"
        Dim clasecuenta As String = "Todos"
        Dim sFecha As String = ""
        Dim codMercado As String = ""

        Dim tablaParametros As Hashtable = Session("ParametrosReporteSaldos")
        If tablaParametros Is Nothing Then
            tablaParametros = New Hashtable
        End If

        If Not tablaParametros("codPortafolio") Is Nothing Then
            codPortafolio = tablaParametros("codPortafolio")
        End If
        If Not tablaParametros("codMoneda") Is Nothing Then
            codMoneda = tablaParametros("codMoneda")
        End If
        If Not tablaParametros("codBanco") Is Nothing Then
            codBanco = tablaParametros("codBanco")
        End If
        If Not tablaParametros("codClaseCuenta") Is Nothing Then
            codClaseCuenta = tablaParametros("codClaseCuenta")
        End If
        If Not tablaParametros("Portafolio") Is Nothing Then
            If tablaParametros("Portafolio") <> "" Then
                portafolio = tablaParametros("Portafolio")
            End If
        End If
        If Not tablaParametros("Banco") Is Nothing Then
            If tablaParametros("Banco") <> "" Then
                banco = tablaParametros("Banco")
            End If
        End If
        If Not tablaParametros("Moneda") Is Nothing Then
            If tablaParametros("Moneda") <> "" Then
                moneda = tablaParametros("Moneda")
            End If
        End If
        If Not tablaParametros("ClaseCuenta") Is Nothing Then
            If tablaParametros("ClaseCuenta") <> "" Then
                clasecuenta = tablaParametros("ClaseCuenta")
            End If
        End If
        If Not tablaParametros("Mercado") Is Nothing Then
            If tablaParametros("Mercado") <> "" Then
                codMercado = tablaParametros("codMercado")
            End If
        End If

        fechaInicio = IIf(Request.QueryString("FechaInicio") Is Nothing, "0", Request.QueryString("FechaInicio"))
        roCuentaEconomica.CodigoPortafolioSBS = codPortafolio
        roCuentaEconomica.CodigoMoneda = codMoneda
        roCuentaEconomica.CodigoClaseCuenta = codClaseCuenta
        roCuentaEconomica.CodigoTercero = codBanco
        roCuentaEconomica.FechaCreacion = fechaInicio
        roCuentaEconomica.CodigoMercado = codMercado


        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim dsSaldos As DataSet = New SaldosBancariosBM().SeleccionarPorFiltro(dsCuentaEconomica, DatosRequest)
        CopiarTabla(dsSaldos.Tables(0), dsAux.SaldosBancarios)
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("Portafolio", portafolio)
        rep.SetParameterValue("ClaseCuenta", clasecuenta)
        rep.SetParameterValue("Moneda", moneda)
        rep.SetParameterValue("Banco", banco)
        rep.SetParameterValue("FechaOperacion", IIf(fechaInicio = "0", Now.ToString("dd/MM/yyyy"), UIUtility.ConvertirFechaaString(fechaInicio)))
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub reporteVencimientos()
        Dim dsAux As New RepVencimientos
        Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
        Dim codPortafolio As String ' = UIUtility.ListaCadena(Portafolios) #ERROR#
        Dim codMoneda As String = ""
        Dim codIntermediario As String = ""
        Dim codOperacion As String = ""
        Dim codMercado As String '= UIUtility.ListaCadena(Mercados) #ERROR#
        Dim codClaseInstrumento As String = ""
        Dim fechaInicio As Decimal
        Dim fechaFin As Decimal
        Dim portafolio As String = "Todos"
        Dim moneda As String = "Todos"
        Dim mercado As String = "Todos"
        Dim claseInstrumento As String = "Todos"
        Dim Intermediario As String = "Todos"
        Dim operacion As String = "Todos"
        Dim tablaParametros As Hashtable = Session("ParametrosReporteVencimientos")
        If tablaParametros Is Nothing Then
            tablaParametros = New Hashtable
        End If
        If Not tablaParametros("codPortafolio") Is Nothing Then
            codPortafolio = tablaParametros("codPortafolio")
        End If
        If Not tablaParametros("codMercado") Is Nothing Then
            codMercado = tablaParametros("codMercado")
        End If
        If Not tablaParametros("codMoneda") Is Nothing Then
            codMoneda = tablaParametros("codMoneda")
        End If
        If Not tablaParametros("codOperacion") Is Nothing Then
            codOperacion = tablaParametros("codOperacion")
        End If
        If Not tablaParametros("codIntermediario") Is Nothing Then
            codIntermediario = tablaParametros("codIntermediario")
        End If
        If Not tablaParametros("codClaseInstrumento") Is Nothing Then
            codClaseInstrumento = tablaParametros("codClaseInstrumento")
        End If
        If Not tablaParametros("Portafolio") Is Nothing Then
            If tablaParametros("Portafolio") <> "" Then
                portafolio = tablaParametros("Portafolio")
            End If
        End If
        If Not tablaParametros("Mercado") Is Nothing Then
            If tablaParametros("Mercado") <> "" Then
                mercado = tablaParametros("Mercado")
            End If
        End If
        If Not tablaParametros("Moneda") Is Nothing Then
            If tablaParametros("Moneda") <> "" Then
                moneda = tablaParametros("Moneda")
            End If
        End If
        If Not tablaParametros("Operacion") Is Nothing Then
            If tablaParametros("Operacion") <> "" Then
                operacion = tablaParametros("Operacion")
            End If
        End If
        If Not tablaParametros("Intermediario") Is Nothing Then
            If tablaParametros("Intermediario") <> "" Then
                Intermediario = tablaParametros("Intermediario")
            End If
        End If
        If Not tablaParametros("ClaseInstrumento") Is Nothing Then
            If tablaParametros("ClaseInstrumento") <> "" Then
                claseInstrumento = tablaParametros("ClaseInstrumento")
            End If
        End If
        cuentaPorCobrar.CodigoMercado = codMercado
        cuentaPorCobrar.CodigoPortafolioSBS = codPortafolio
        cuentaPorCobrar.CodigoMoneda = codMoneda
        cuentaPorCobrar.CodigoTercero = codIntermediario
        cuentaPorCobrar.CodigoOperacion = codOperacion
        fechaInicio = IIf(Request.QueryString("FechaInicio") Is Nothing, "0", Request.QueryString("FechaInicio"))
        fechaFin = IIf(Request.QueryString("FechaFin") Is Nothing, "0", Request.QueryString("FechaFin"))
        cuentaPorCobrar.FechaOperacion = fechaInicio
        cuentaPorCobrar.FechaIngreso = fechaFin
        dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
        Dim dsCuentas As DataSet = New CuentasPorCobrarBM().SeleccionarVencimientos2(dsCuentasPorCobrar, codClaseInstrumento, fechaInicio, fechaFin, DatosRequest)
        CopiarTabla(dsCuentas.Tables(0), dsAux.Vencimientos)
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("Fecha_Ini", UIUtility.ConvertirFechaaString(fechaInicio))
        rep.SetParameterValue("Fecha_Fin", UIUtility.ConvertirFechaaString(fechaFin))
        rep.SetParameterValue("portafolio", portafolio)
        rep.SetParameterValue("mercado", mercado)
        rep.SetParameterValue("moneda", moneda)
        rep.SetParameterValue("Operacion", operacion)
    End Sub
    Private Sub reporteMovimientos()
        Dim dsAux As New RepMovimientos
        Dim codPortafolio As String
        Dim codMercado As String
        Dim codClaseCuenta As String = ""
        Dim codBanco As String = ""
        Dim codMoneda As String = ""
        Dim codOperacion As String = ""
        Dim codTipoOperacion As String = ""
        Dim fechaInicio As Decimal
        Dim fechaFin As Decimal
        Dim portafolio As String = "Todos"
        Dim mercado As String = "Todos"
        Dim moneda As String = "Todos"
        Dim banco As String = "Todos"
        Dim clasecuenta As String = "Todos"
        Dim operacion As String = "Todos"
        Dim tablaParametros As Hashtable = Session("ParametrosReporteMovimientos")
        If tablaParametros Is Nothing Then
            tablaParametros = New Hashtable
        End If
        If Not tablaParametros("codPortafolio") Is Nothing Then
            codPortafolio = tablaParametros("codPortafolio")
        End If
        If Not tablaParametros("codMercado") Is Nothing Then
            codMercado = tablaParametros("codMercado")
        End If
        If Not tablaParametros("codMoneda") Is Nothing Then
            codMoneda = tablaParametros("codMoneda")
        End If
        If Not tablaParametros("codOperacion") Is Nothing Then
            codOperacion = tablaParametros("codOperacion")
        End If
        If Not tablaParametros("codTipoOperacion") Is Nothing Then
            codTipoOperacion = tablaParametros("codTipoOperacion")
        End If
        If Not tablaParametros("codBanco") Is Nothing Then
            codBanco = tablaParametros("codBanco")
        End If
        If Not tablaParametros("codClaseCuenta") Is Nothing Then
            codClaseCuenta = tablaParametros("codClaseCuenta")
        End If

        If Not tablaParametros("Portafolio") Is Nothing Then
            If tablaParametros("Portafolio") <> "" Then
                portafolio = tablaParametros("Portafolio")
            End If
        End If
        If Not tablaParametros("Mercado") Is Nothing Then
            If tablaParametros("Mercado") <> "" Then
                mercado = tablaParametros("Mercado")
            End If
        End If
        If Not tablaParametros("Banco") Is Nothing Then
            If tablaParametros("Banco") <> "" Then
                banco = tablaParametros("Banco")
            End If
        End If
        If Not tablaParametros("Moneda") Is Nothing Then
            If tablaParametros("Moneda") <> "" Then
                moneda = tablaParametros("Moneda")
            End If
        End If
        If Not tablaParametros("Operacion") Is Nothing Then
            If tablaParametros("Operacion") <> "" Then
                operacion = tablaParametros("Operacion")
            End If
        End If
        If Not tablaParametros("ClaseCuenta") Is Nothing Then
            If tablaParametros("ClaseCuenta") <> "" Then
                clasecuenta = tablaParametros("ClaseCuenta")
            End If
        End If
        fechaInicio = IIf(Request.QueryString("FechaInicio") Is Nothing, "0", Request.QueryString("FechaInicio"))
        fechaFin = IIf(Request.QueryString("FechaFin") Is Nothing, "0", Request.QueryString("FechaFin"))
        Dim dsOpCaja As New OperacionCajaBE
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
        opCaja.CodigoMercado = codMercado
        opCaja.CodigoPortafolioSBS = codPortafolio
        opCaja.CodigoClaseCuenta = codClaseCuenta
        opCaja.CodigoMoneda = codMoneda
        opCaja.CodigoTerceroOrigen = codBanco
        opCaja.NumeroCuenta = ""
        opCaja.CodigoOperacion = codOperacion
        opCaja.CodigoTipoOperacion = codTipoOperacion 'RGF 20080826
        opCaja.CodigoOperacionCaja = ""
        dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
        Dim dsMovimientos As DataSet = New OperacionesCajaBM().SeleccionarPorFiltro3(dsOpCaja, fechaInicio, fechaFin, DatosRequest)
        CopiarTabla(dsMovimientos.Tables(0), dsAux.Movimientos)
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("FechaInicio", IIf(fechaInicio = "0", "", UIUtility.ConvertirFechaaString(fechaInicio)))
        rep.SetParameterValue("FechaFin", IIf(fechaInicio = "0", "", UIUtility.ConvertirFechaaString(fechaFin)))
    End Sub
    Private Sub ReporteEnvioCartas()
        Dim ds As DataSet = Session("ReporteEnvioCartas")
        Dim dsAux As New RepEnvioCartas
        CopiarTabla(ds.Tables(0), dsAux.EnvioCartas)
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Fecha", Now.ToString("dd/MM/yyyy"))
        rep.SetParameterValue("Hora", Now.ToString("HH:mm:ss"))
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        Dim titulo As String = "CARTAS"
        If Not Request.QueryString("Titulo") Is Nothing Then
            titulo = "ABONOS"
        End If
        rep.SetParameterValue("Titulo", titulo)
    End Sub
    Private Sub ReporteMovimientosNeg()
        Dim ds() As DataSet = Session("ReporteMovimientosNeg")
        Dim dsAuxLoc As New RepMovimientosNeg
        Dim dsAuxExt As New RepMovimientosNegE
        CopiarTabla(ds(0).Tables(0), dsAuxLoc.MovimientosNeg1)
        CopiarTabla(ds(0).Tables(0), dsAuxLoc.MovimientosNeg2)
        CopiarTabla(ds(1).Tables(0), dsAuxExt.MovimientosNeg1)
        CopiarTabla(ds(1).Tables(0), dsAuxExt.MovimientosNeg2)
        rep.OpenSubreport("Subreport1").SetDataSource(dsAuxLoc)
        rep.OpenSubreport("Subreport2").SetDataSource(dsAuxExt)
        rep.SetParameterValue("Usuario", Usuario)
        rep.SetParameterValue("Glosa1", Session("Glosa"))
        rep.SetParameterValue("Glosa2", Session("Glosa"))
    End Sub
    Private Sub ReporteFlujoEstimado()
        Dim saldosBancariosBM As New SaldosBancariosBM
        Dim ds As DataSet = saldosBancariosBM.SeleccionarFlujoEstimadoPorFiltro2(Session("Rep_CuentaEconomica"), UIUtility.ConvertirFechaaDecimal(Request.QueryString("FechaInicio")), UIUtility.ConvertirFechaaDecimal(Request.QueryString("FechaFin")), Request.QueryString("Periodo"), Request.QueryString("PorDivisas"), DatosRequest) 'Session("ReporteFlujoEstimado")
        Dim dsAux As New RepFlujoEstimado
        Dim intI As Integer
        Dim drAux As DataRow
        Dim FchInicio, FchFin, Rango As String
        FchInicio = IIf(Request.QueryString("FechaInicio") Is Nothing, "0", Request.QueryString("FechaInicio"))
        FchFin = IIf(Request.QueryString("FechaFin") Is Nothing, "0", Request.QueryString("FechaFin"))
        Rango = "Del  " & FchInicio & "  Al  " & FchFin
        Dim dr As DataRow
        Dim drNuevo As DataRow
        Dim dt As DataTable = ds.Tables(0).Clone
        For Each dr In ds.Tables(0).Rows
            If dr("Formula").ToString.EndsWith("-I") Then
                drNuevo = dt.NewRow
                drNuevo("IngresoValor") = dr("IngresoValor")
                drNuevo("EgresoValor") = dr("EgresoValor")
                drNuevo("CodigoPortafolio") = dr("CodigoPortafolio")
                drNuevo("FechaVencimiento") = dr("FechaVencimiento")
                drNuevo("DescripcionOperacion") = dr("DescripcionOperacion")
                drNuevo("NumeroOperacion") = dr("NumeroOperacion")
                drNuevo("SaldoPosterior") = ds.Tables(0).Select("Formula = '" & dr("Formula").ToString.Substring(0, 11) & "' AND CodigoMoneda = '" & dr("CodigoMoneda") & "'")(0)("SaldoAnterior")
                drNuevo("FechaOperacion") = dr("FechaOperacion")
                drNuevo("CodigoMoneda") = dr("CodigoMoneda")
                dt.Rows.Add(drNuevo)
            End If
        Next
        With dt
            For intI = 0 To .Rows.Count - 1
                drAux = dsAux.FlujoEstimado.NewFlujoEstimadoRow
                drAux("SaldoAnterior") = .Rows(intI)("SaldoAnterior")
                drAux("SaldoPosterior") = .Rows(intI)("SaldoPosterior")
                drAux("IngresoValor") = .Rows(intI)("IngresoValor")
                drAux("EgresoValor") = .Rows(intI)("EgresoValor")
                drAux("CodigoPortafolio") = .Rows(intI)("CodigoPortafolio")
                drAux("FechaVencimiento") = .Rows(intI)("FechaVencimiento")
                drAux("DescripcionOperacion") = .Rows(intI)("DescripcionOperacion")
                drAux("NumeroOperacion") = .Rows(intI)("NumeroOperacion")
                drAux("FechaOperacion") = .Rows(intI)("FechaOperacion")
                drAux("CodigoMoneda") = .Rows(intI)("CodigoMoneda")
                dsAux.FlujoEstimado.Rows.Add(drAux)
            Next
        End With
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Mercado", Request.QueryString("Mercado"))
        rep.SetParameterValue("Fecha", Now.ToString("dd/MM/yyyy"))
        rep.SetParameterValue("Hora", Now.ToString("HH:mm:ss"))
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("RangoFecha", Rango)
        rep.SetParameterValue("Moneda", IIf(Request.QueryString("Moneda") = "", "TODOS", Request.QueryString("Moneda")))
        rep.SetParameterValue("TipoFlujo", Request.QueryString("TipoFlujo"))

    End Sub
#End Region
    Private Sub Retornar()
        AlertaJS("window.close();")
    End Sub
    Protected Sub Modulos_Tesoreria_Reportes_frmReporte_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class