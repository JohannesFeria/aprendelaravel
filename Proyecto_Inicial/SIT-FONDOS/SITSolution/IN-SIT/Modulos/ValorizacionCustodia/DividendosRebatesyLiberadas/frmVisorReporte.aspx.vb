Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmVisorReporte
    Inherits BasePage
    Dim oReport As New ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim CodigoPortafolioSBS As String
            Dim FechaInicio, FechaFin As Decimal
            CodigoPortafolioSBS = Request.QueryString("CodigoPortafolioSBS")
            FechaInicio = Request.QueryString("FechaInicio")
            FechaFin = Request.QueryString("FechaFin")
            Dim nombre As String = "Usuario"
            Dim sUsuario As String
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            sUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)            
            oReport.Load(Server.MapPath("RptDivRepLib.rpt"))
            Dim oBM As New ReporteGestionBM
            Dim dt As DataTable
            Dim drfila As DataRow
            Dim dsReporte As New DsDivRebLib
            Dim cls As New PortafolioBM
            Dim obj As PortafolioBE
            Dim ds As New DataSet
            obj = cls.Seleccionar(CodigoPortafolioSBS, ds)
            dt = oBM.DividendosRebatesLiberadas(CodigoPortafolioSBS, FechaInicio, FechaFin).Tables(0)
            For Each dr As DataRow In dt.Rows
                drfila = dsReporte.Tables(0).NewRow()
                drfila("Tipo") = dr("Tipo")
                drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                drfila("CodigoNemonico") = dr("CodigoNemonico")
                drfila("FechaCorte") = dr("FechaCorte")
                drfila("FechaIDI") = dr("FechaIDI")
                If Not dr("FechaPago") Is DBNull.Value Then
                    drfila("FechaPago") = dr("FechaPago")
                Else
                    drfila("FechaPago") = ""
                End If
                drfila("Unidades") = dr("Unidades")
                drfila("Factor") = dr("Factor")
                drfila("CodigoMoneda") = dr("CodigoMoneda")
                drfila("Importe") = dr("Importe")
                drfila("CodigoCustodio") = dr("CodigoCustodio")
                dsReporte.Tables(0).Rows.Add(drfila)
            Next
            oReport.SetDataSource(dsReporte)
            oReport.SetParameterValue("@Fondo", obj.Tables(0).Rows(0)("Descripcion").ToString)
            oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(FechaInicio))
            oReport.SetParameterValue("@FechaFin", UIUtility.ConvertirFechaaString(FechaFin))
            oReport.SetParameterValue("@Usuario", sUsuario)
            oReport.SetParameterValue("RutaLogo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            Me.CrystalReportViewer1.ReportSource = oReport
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar el Reporte")
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmVisorReporte_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class