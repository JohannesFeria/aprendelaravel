Imports System.Data
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Partial Class Modulos_Inversiones_Reportes_Orden_de_Inversion_frmVisorReporteLimitesPorInstrumento
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim strFecha As String = Request.QueryString("Fecha")
            Dim strPortafolio As String = Request.QueryString("Portafolio")
            Dim strEscenario As String = Request.QueryString("Escenario")
            Dim strValorNivel As String = Request.QueryString("ValorNivel")
            Dim decFecha As Decimal
            decFecha = UIUtility.ConvertirFechaaDecimal(strFecha)
            Dim dsAux As DataSet
            Dim dtAux As DataTable
            Dim oReporteLimitesBM As New ReporteLimitesBM
            dsAux = oReporteLimitesBM.Seleccionar_ReporteLimitesPorInstrumento(strPortafolio, decFecha, "REAL", strValorNivel, DatosRequest)
            If Not dsAux Is Nothing Then
                dtAux = dsAux.Tables(0)
                If dtAux.Rows.Count > 0 Then
                    Dim dsReporteLimitePorInstrumento As New DsReporteLimitesPorInstrumento
                    CopiarTabla(dsAux.Tables(0), dsReporteLimitePorInstrumento.ReporteLimitesPorInstrumento)
                    dsReporteLimitePorInstrumento.Merge(dsReporteLimitePorInstrumento, False, System.Data.MissingSchemaAction.Ignore)
                    Dim nombre, usuario As String
                    nombre = "Usuario"
                    Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
                    usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)                    
                    oReport.Load(Server.MapPath("LimitesPorInstrumento.rpt"))
                    oReport.SetDataSource(dsReporteLimitePorInstrumento)
                    oReport.SetParameterValue("Portafolio", strPortafolio)
                    oReport.SetParameterValue("FechaProceso", strFecha)
                    oReport.SetParameterValue("Escenario", strEscenario)
                    oReport.SetParameterValue("Usuario", usuario)
                    oReport.SetParameterValue("RutaLogo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crvInversion.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree
                    crvInversion.RenderingDPI = 120
                    crvInversion.ReportSource = oReport
                Else
                    EjecutarJS("alert('No existen limites vinculados al instrumento');window.close();")
                End If
            Else
                EjecutarJS("alert('No existen limites vinculados al instrumento');window.close();")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_Orden_de_Inversion_frmVisorReporteLimitesPorInstrumento_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class