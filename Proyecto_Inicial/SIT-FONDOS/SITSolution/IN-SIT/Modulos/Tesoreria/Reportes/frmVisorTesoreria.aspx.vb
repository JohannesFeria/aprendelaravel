Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Tesoreria_Reportes_frmVisorTesoreria
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strParametros, strUsuario, strNombre As String        
        strNombre = "Usuario"
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        strUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & strNombre & "'")(0)(1), String)
        If Request.QueryString("FechaProceso") <> "" Then
            Dim dtTablaListaCargados As New DataTable
            Dim dtTablaListaNoCargados As New DataTable
            Dim oSaldosCargados As New SaldosCargadosBE
            Dim oSaldosNoCargados As New SaldosNoCargadosBE
            dtTablaListaCargados = CType(Session("GrillaListaCargados"), DataTable)
            dtTablaListaNoCargados = CType(Session("GrillaListaNoCargados"), DataTable)
            CopiarTabla(dtTablaListaCargados, oSaldosCargados.SaldosCargados)
            CopiarTabla(dtTablaListaNoCargados, oSaldosNoCargados.SaldosNoCargados)
            oReport.Load(Server.MapPath("CapturaDeSaldosBancarios.rpt"))
            oReport.OpenSubreport("SaldosCargados").SetDataSource(oSaldosCargados)
            oReport.OpenSubreport("SaldosNoCargados").SetDataSource(oSaldosNoCargados)
            oReport.DataDefinition.FormulaFields("Usuario").Text = "'" & strUsuario & "'"
            oReport.SetParameterValue("@FechaProceso", Request.QueryString("FechaProceso"))
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Else
            Dim dtTabla As New DataTable
            Dim Liquidaciones As Array
            If Request.QueryString("LiquidacionesCobrar") <> "" Then
                dtTabla = CType(Session("GrillaCuentasCobrar"), DataTable)
                strParametros = Request.QueryString("LiquidacionesCobrar")
                Liquidaciones = strParametros.Split(",")
                oReport.Load(Server.MapPath("LiquidacionesCobrar.rpt"))
            Else
                If Request.QueryString("LiquidacionesPagar") <> "" Then
                    dtTabla = CType(Session("GrillaCuentasPagar"), DataTable)
                    strParametros = Request.QueryString("LiquidacionesPagar")
                    Liquidaciones = strParametros.Split(",")
                    oReport.Load(Server.MapPath("LiquidacionesPagar.rpt"))
                End If
            End If
            Dim oLiquidacionesBE As New LiquidacionesBE
            CopiarTabla(dtTabla, oLiquidacionesBE.LiquidacionesTesoreria)
            oReport.SetDataSource(oLiquidacionesBE)
            oReport.SetParameterValue("@Usuario", strUsuario)
            oReport.SetParameterValue("@Fechas", Liquidaciones(0) & " Al " & Liquidaciones(1))
            oReport.SetParameterValue("@Portafolio", IIf(Liquidaciones(2) = "--SELECCIONE--", "Todos", Liquidaciones(2)))
            oReport.SetParameterValue("@Mercado", IIf(Liquidaciones(3) = "--SELECCIONE--", "Todos", Liquidaciones(3)))
            oReport.SetParameterValue("@Intermediario", IIf(Liquidaciones(5) = "--SELECCIONE--", "Todos", Liquidaciones(5)))
            oReport.SetParameterValue("@Moneda", Liquidaciones(4))
            oReport.SetParameterValue("@TipoOperacion", IIf(Liquidaciones(6) = "--SELECCIONE--", "Todos", Liquidaciones(6)))
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        End If
        CrystalReportViewer1.ReportSource = oReport
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        If (dtOrigen Is Nothing) Then
            Return
        End If
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub vistaLiquidacionesCobrar()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        oReport.Load(Server.MapPath("LiquidacionesCobrar.rpt"))
        CrystalReportViewer1.ReportSource = oReport
    End Sub
    Protected Sub Modulos_Tesoreria_Reportes_frmVisorTesoreria_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class