Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVisorErrorValoracion
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
        Dim dsconsulta As New DsConsultaNemonico
        Dim drconsulta As DataRow
        Dim ruta As String = Server.MapPath("RptNemonicosPrecio.rpt")
        oReport.Load(ruta)
        Dim oValoresBM As New ValoresBM
        dtconsultanemonico = oValoresBM.ObtenerNemonicosError.Tables(0)
        For Each drv As DataRow In dtconsultanemonico.Rows
            drconsulta = dsconsulta.Tables(0).NewRow()

            drconsulta("Secuencia") = drv("Secuencia")
            drconsulta("CodigoNemonico") = drv("CodigoNemonico")

            dsconsulta.Tables(0).Rows.Add(drconsulta)
        Next
        dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
        oReport.SetDataSource(dsconsulta)
        oReport.SetParameterValue("@Fecha", vFecha)
        oReport.SetParameterValue("@Usuario", usuario)
        oReport.SetParameterValue("@Portafolio", vFondo)
        oReport.SetParameterValue("@RutaImagen", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Me.CrNemonico.ReportSource = oReport
        CrNemonico.DataBind()
    End Sub
    Protected Sub Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVisorErrorValoracion_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class