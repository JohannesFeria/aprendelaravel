Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports System.Text
Imports System.Data

Partial Class Modulos_Gestion_frmReporteLimitesInstrumento
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        Dim fecha As String
        Dim ds As DataTable

        btnImprimir.Attributes.Add("onclick", "return confirm('Es necesario haber generado de los límites el Consolidado de Posición de todos los fondos, antes de mostrar el reporte. ¿Desea generar el reporte?');")
        If Not Page.IsPostBack Then
            CargarCombos()
            ds = oCarteraTituloValoracion.UltimaValoracion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), DatosRequest).Tables(0)
            fecha = ds.Rows(0).Item(0)
            tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(Convert.ToInt32(fecha))
        End If
    End Sub

    Public Sub CargarCombos()
        Dim tablaTipoInstrumento As New Data.DataTable
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        tablaTipoInstrumento = oTipoInstrumentoBM.Listar(DatosRequest).Tables(0)
        Dim rows As DataRow()
        Dim dtNew As DataTable = tablaTipoInstrumento.Clone()
        rows = tablaTipoInstrumento.Select("CodigoTipoInstrumento <> 'FORW' and CodigoTipoInstrumento <> 'INES'")
        For Each dr As DataRow In rows
            dtNew.ImportRow(dr)
        Next
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoInstrumento, dtNew, "CodigoTipoInstrumentoSBS", "CodigoMasDescripcion", True)
        ddlTipoInstrumento.Items.Insert(0, New ListItem("--Seleccione--", ""))

        Dim oTipoRentaBM As New TipoRentaBM
        Dim DtTablaTipoRenta As New DataTable
        DtTablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        DtTablaTipoRenta.DefaultView.RowFilter = "CodigoRenta<=2"
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True)
        ddlTipoRenta.Items.Insert(0, New ListItem("--Seleccione--", ""))

        Dim tablaDerivadas As New Data.DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        tablaDerivadas = oParametrosGeneralesBM.ListarDerivadasLimiteInstrumento().Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlDerivados, tablaDerivadas, "Valor", "Nombre", True)
        ddlDerivados.Items.Insert(0, New ListItem("--Seleccione--", ""))
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim dt As DataTable = DatosRequest.Tables(0)
            Dim sUsuario As String = CType(dt.Select(dt.Columns(0).ColumnName & "='Usuario'")(0)(1), String)
            Dim sTipoRenta As String = ""
            Dim sTipoInst As String = ""
            Dim sDerivados As String = ""
            Dim sOrigen As String = ""
            Dim sDestino As String = ""
            Dim sFechaValora As String = ""

            sTipoRenta = IIf(ddlTipoRenta.SelectedValue.ToString = "Todos", "", ddlTipoRenta.SelectedValue.ToString)
            sTipoInst = IIf(ddlTipoInstrumento.SelectedValue.ToString = "Todos", "", ddlTipoInstrumento.SelectedValue.ToString)
            sDerivados = IIf(ddlDerivados.SelectedValue.ToString = "Todos", "T", ddlDerivados.SelectedValue.ToString)
            sOrigen = RutaPlantillas() & "\PlantillaConLim.xls"
            sDestino = New ParametrosGeneralesBM().Listar("RUTALIM", DatosRequest).Rows(0)("Valor") & "ReporteResulLimitesInstrumento_" & System.DateTime.Now.ToString("yyyyMMdd_hhmmss") & ".xls"
            sFechaValora = UIUtility.ConvertirFechaaDecimal(Me.tbFechaValoracion.Text)

            Dim Variable As String = "CodigoISIN,CodigoSBS,CodigoNemonico,TipoRenta,Sinonimo,Derivados,RutaOrigen,RutaDestino,FechaValora,Usuario,TipoReporte,CodigoLimite"    'HDG OT 63063 R02 20110503
            Dim Parametros As String = tbCodigoIsin.Text + "," + tbCodigoSBS.Text + "," + tbMnemonico.Text + "," + sTipoRenta + "," + sTipoInst + "," + sDerivados + "," + sOrigen + "," + sDestino + "," + sFechaValora + "," + sUsuario + "," + "I" + ","    'HDG OT 63063 R02 20110503
            Dim obj As New JobBM

            Dim rpta As String = obj.EjecutarJob("DTS_SIT_ProcesarRepControlLimitesInstrumento", "Procesa Reporte de Control de Limites por Instrumento basandose en los parametros enviados", Variable, Parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
            lblTime.Text = rpta


        Catch ex As Exception
            lblTime.Text = "El Proceso no fue enviado por un error"
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
End Class
