Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmDivRebLibImpCR
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load        
        Dim oDivRebLibImp As New DivRebLibImp
        Dim sCodigoSBS As String = Request.QueryString("CodigoSBS")
        Dim nGrupoIdentificador As Long = Request.QueryString("GrupoIdentificador")
        Dim sTipoDistribucion As String = Request.QueryString("TipoDistribucion")
        Dim sMultifondo As String = Request.QueryString("Multifondo")
        Select Case sTipoDistribucion.ToUpper
            Case "D"
                rep.Load(Server.MapPath("DivRebLibImpCry_Div.rpt"))
            Case "R"
                rep.Load(Server.MapPath("DivRebLibImpCry_Reb.rpt"))
            Case "L"
                rep.Load(Server.MapPath("DivRebLibImpCry_Lib.rpt"))
        End Select
        Dim oDivRebLibBE As DataSet = New DividendosRebatesLiberadasBM().SeleccionarImpresion(sCodigoSBS, nGrupoIdentificador, "A", sMultifondo, DatosRequest)
        CopiarTabla(oDivRebLibBE.Tables(0), oDivRebLibImp.DivRebLibImp)
        rep.SetDataSource(oDivRebLibImp)
        crDivRebLibImp.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree
        crDivRebLibImp.RenderingDPI = 120
        crDivRebLibImp.ReportSource = rep
        crDivRebLibImp.ReportSource = rep
        rep.SetParameterValue("Usuario", Usuario)
        rep.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmDivRebLibImpCR_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class