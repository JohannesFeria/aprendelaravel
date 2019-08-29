Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Gestion_Reportes_frmVisorGestionIDI
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim usuario, nombre, fechaVectorPrecio As String
        fechaVectorPrecio = Request.QueryString("pFechaVectorPrecio")
        nombre = "Usuario"
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
        Dim dsconsulta As DataTable = CType(Session("dsGestionIDI"), DataTable)
        oReport.Load(Server.MapPath("../Archivos Planos/NoVector.rpt"))
        oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & fechaVectorPrecio & "'"
        oReport.SetDataSource(dsconsulta)
        oReport.SetParameterValue("@Usuario", usuario)
        oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Me.crGestion.ReportSource = oReport
    End Sub
    Protected Sub Modulos_Gestion_Reportes_frmVisorGestionIDI_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class