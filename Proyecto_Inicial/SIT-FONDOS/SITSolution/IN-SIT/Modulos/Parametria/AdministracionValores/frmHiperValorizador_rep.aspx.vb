Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_AdministracionValores_frmHiperValorizador_rep
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtHiperValorizar As New DataTable
        dtHiperValorizar = CType(Session("tabla"), DataTable)
        rep.Load(Server.MapPath("HiperValorizadorRep.rpt"))
        rep.SetDataSource(dtHiperValorizar)
        CrystalReportViewer1.ReportSource = rep
    End Sub
    Protected Sub Modulos_Parametria_AdministracionValores_frmHiperValorizador_rep_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class