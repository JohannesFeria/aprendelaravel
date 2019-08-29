Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports System.Text
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Gestion_frmReporteLimitesporTipo
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        Dim fecha As String
        Dim ds As DataTable

        Me.btnImprimir.Attributes.Add("onclick", "javascript:return confirm('Es necesario haber generado de los límites el Consolidado de Posición de todos los fondos,\nantes de mostrar el reporte. ¿Desea generar el reporte?');")
        If Not Page.IsPostBack Then
            CargarCombos()
            ds = oCarteraTituloValoracion.UltimaValoracion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), Me.DatosRequest).Tables(0)
            fecha = ds.Rows(0).Item(0)
            tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(Convert.ToInt32(fecha))
        End If
    End Sub

    Public Sub CargarCombos()
        Dim dtTipoReporte As DataTable
        dtTipoReporte = New ParametrosGeneralesBM().Listar(TIPOS_REP_CONTROL_LIMITES, DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoReporte, dtTipoReporte, "Valor", "Nombre", False)

        Dim dtLimite As DataTable = New LimiteBM().SeleccionarPorFiltro("", "", "A", datosrequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlLimites, dtLimite, "codigoLimite", "Descripcion", True)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim dt As DataTable = DatosRequest.Tables(0)
            Dim sUsuario As String = CType(dt.Select(dt.Columns(0).ColumnName & "='Usuario'")(0)(1), String)
            Dim sTipoReporte As String = ""
            Dim sLimite As String = ""
            Dim sOrigen As String = ""
            Dim sDestino As String = ""
            Dim sFechaValora As String = ""

            sTipoReporte = ddlTipoReporte.SelectedValue
            sLimite = IIf(ddlLimites.SelectedValue.ToString = "Todos", "", ddlLimites.SelectedValue.ToString)
            sOrigen = RutaPlantillas() & "\PlantillaConLim" & sTipoReporte & ".xls"
            sDestino = New ParametrosGeneralesBM().Listar("RUTALIM", DatosRequest).Rows(0)("Valor") & "ReporteLimites" & IIf(sTipoReporte = "TI", "xTipoInstrumento", "xTipoRenta") & "_" & System.DateTime.Now.ToString("yyyyMMdd_hhmmss") & ".xls"
            sFechaValora = UIUtility.ConvertirFechaaDecimal(Me.tbFechaValoracion.Text)

            Dim Variable As String = "CodigoISIN,CodigoSBS,CodigoNemonico,TipoRenta,Sinonimo,Derivados,RutaOrigen,RutaDestino,FechaValora,Usuario,TipoReporte,CodigoLimite"
            Dim Parametros As String = "" + "," + "" + "," + "" + "," + "" + "," + "" + "," + "" + "," + sOrigen + "," + sDestino + "," + sFechaValora + "," + sUsuario + "," + sTipoReporte + "," + sLimite
            Dim obj As New JobBM

            Dim rpta As String = obj.EjecutarJob("DTS_SIT_ProcesarRepControlLimitesInstrumento", "Procesa Reporte de Control de Limites " & IIf(sTipoReporte = "TI", "por Tipo de Instrumento", "por Tipo de Renta") & " basandose en los parametros enviados", Variable, Parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
            Me.lblTime.Text = rpta

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

End Class
