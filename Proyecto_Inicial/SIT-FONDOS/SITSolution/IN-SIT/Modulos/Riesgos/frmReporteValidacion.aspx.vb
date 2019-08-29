Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class Modulos_Riesgos_frmReporteValidacion
    Inherits BasePage
    Dim oUtil As New UtilDM
    Private RutaDestino As String = String.Empty
    Dim Riesgos As New RiesgosBM
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                UIUtility.InsertarElementoSeleccion(ddlValidacion, 2, "Validación 2")
                UIUtility.InsertarElementoSeleccion(ddlValidacion, 1, "Validación 1")
                UIUtility.InsertarElementoSeleccion(ddlValidacion, 0)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema")
        End Try
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            If ddlValidacion.SelectedValue = 0 Then
                AlertaJS("Debe seleccionar una Validación")
            Else
                If ddlValidacion.SelectedValue = 1 Then
                    Call Validacion01()
                End If
                If ddlValidacion.SelectedValue = 2 Then
                    Call Validacion02()
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema")
        End Try
    End Sub

    Private Sub Validacion01()
        Try
            Dim dtTabla As New DataTable
            dtTabla = Riesgos.ReporteValidacionRating(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text)
            dtTabla.TableName = "DsReporteValidacionRating"
            If dtTabla.Rows.Count > 0 Then
                oReport.Load(Server.MapPath("Reportes/rptReporteValidacionRating.rpt"))
                oReport.SetDataSource(dtTabla)
                oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                oReport.SetParameterValue("Usuario", Usuario)
                Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
                'Dim excelOpts As New CrystalDecisions.Shared.ExcelFormatOptions
                'exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel
                'exportOpts.ExportFormatOptions = excelOpts
                Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                exportOpts.ExportFormatOptions = pdfOpts
                oReport.ExportToHttpResponse(exportOpts, Response, True, "ReporteValidacionRating")
            Else
                AlertaJS("No existe información a reportar.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub Validacion02()
        Try
            Dim dtTabla As New DataTable
            dtTabla = Riesgos.ReporteValidacionFondos(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text)
            dtTabla.TableName = "DsReporteValidacionFondos"
            If dtTabla.Rows.Count > 0 Then
                oReport.Load(Server.MapPath("Reportes/rptReporteValidacionFondos.rpt"))
                oReport.SetDataSource(dtTabla)
                oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                oReport.SetParameterValue("Usuario", Usuario)
                Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
                Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                exportOpts.ExportFormatOptions = pdfOpts
                oReport.ExportToHttpResponse(exportOpts, Response, True, "ReporteValidacionFondos")
            Else
                AlertaJS("No existe información a reportar.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub form1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
