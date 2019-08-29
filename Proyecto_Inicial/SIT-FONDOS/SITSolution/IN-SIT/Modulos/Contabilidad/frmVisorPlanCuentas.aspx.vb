Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Contabilidad_frmVisorPlanCuentas
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim strUsuario, strNombre, strCodigoFondo, strDescripcionFondo As String
            strNombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & strNombre & "'")(0)(1), String)
            strCodigoFondo = Session("CodigoFondo")
            strDescripcionFondo = Session("descripcionFondo")
            oReport.Load(Server.MapPath("PlanCuentas.rpt"))
            Dim dsAux As New dsPlanCuentas
            Dim ds As DataSet = New PlanDeCuentasBM().SeleccionarPorFiltro(strCodigoFondo, 0, DatosRequest)
            Dim drAux As dsPlanCuentas.PlanCuentasRow
            For Each dr As DataRow In ds.Tables(0).Rows
                drAux = dsAux.PlanCuentas.NewPlanCuentasRow()
                drAux("CuentaContable") = dr("CuentaContable")
                drAux("DescripcionCuenta") = dr("DescripcionCuenta")
                drAux("CodigoContablePU") = dr("CodigoContablePU")
                dsAux.PlanCuentas.Rows.Add(drAux)
            Next
            oReport.SetDataSource(dsAux)
            oReport.SetParameterValue("@Usuario", strUsuario)
            oReport.SetParameterValue("@Fondo", strDescripcionFondo)
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            Me.CrystalReportViewer1.ReportSource = oReport
        Catch ex As Exception
            AlertaJS("Ocurrió un error al generar el reporte")
        End Try
    End Sub
    Protected Sub Modulos_Contabilidad_frmVisorPlanCuentas_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class