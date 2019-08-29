Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Tesoreria_Encaje_frmVisorErrorEncaje
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim vFondo, vFecha, usuario, nombre As String
        vFondo = Request.QueryString("pportafolio")
        vFecha = Request.QueryString("pFecha")
        nombre = "Usuario"
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
        Dim dtconsultanemonico As New DataTable
        Dim dsconsulta As New dsNemonicoEncaje
        Dim drconsulta As DataRow        
        oReport.Load(Server.MapPath("RptErrorEncaje.rpt"))
        Dim oEncajeBM As New EncajeBM
        dtconsultanemonico = oEncajeBM.ObtenerNemonicosError.Tables(0)
        For Each drv As DataRow In dtconsultanemonico.Rows
            drconsulta = dsconsulta.Tables(0).NewRow()
            drconsulta("Secuencia") = drv("Secuencia")
            drconsulta("CodigoNemonico") = drv("CodigoMNemonico")
            dsconsulta.Tables(0).Rows.Add(drconsulta)
        Next
        dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
        oReport.SetDataSource(dsconsulta)
        oReport.SetParameterValue("@Fecha", vFecha)
        oReport.SetParameterValue("@Usuario", usuario)
        oReport.SetParameterValue("@Portafolio", vFondo)
        oReport.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
        Me.CrNemonico.ReportSource = oReport
    End Sub
    Protected Sub Modulos_Tesoreria_Encaje_frmVisorErrorEncaje_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class