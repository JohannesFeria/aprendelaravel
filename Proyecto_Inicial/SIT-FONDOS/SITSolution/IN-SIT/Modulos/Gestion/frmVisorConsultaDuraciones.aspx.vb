Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Gestion_frmVisorConsultaDuraciones
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim StrMnemonico, StrUsuario, StrNombre, StrFondo, StrDescripcionFondo As String
            Dim DatFechaValoracion As Date
            Dim DecFechaValoracion As Decimal
            DatFechaValoracion = Request.QueryString("vfechaValoracion")
            StrFondo = Request.QueryString("vfondo")
            StrMnemonico = Request.QueryString("vcodigomnemonico")
            StrNombre = "Usuario"
            StrDescripcionFondo = ""
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            StrUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
            DecFechaValoracion = CType(DatFechaValoracion.ToString("yyyyMMdd"), Decimal)
            Dim drfila As DataRow            
            oReport.Load(Server.MapPath("ConsultaDuraciones.rpt"))
            Dim dsReporte As New dsReporteDuraciones2
            Dim oreporte As DataSet
            If (Not Page.IsPostBack) Then
                oreporte = New CarteraTituloValoracionBM().SeleccionarDuraciones(StrFondo, StrMnemonico, DecFechaValoracion, DatosRequest)
                StrDescripcionFondo = oreporte.Tables(0).Rows(0)("Fondo").ToString.Trim
                Session("dtConsulta") = oreporte
            Else
                oreporte = CType(Session("dtConsulta"), DataSet)
                StrDescripcionFondo = oreporte.Tables(0).Rows(0)("Fondo").ToString.Trim
            End If
            For Each dr As DataRow In oreporte.Tables(0).Rows
                drfila = dsReporte.Tables(0).NewRow()
                drfila("Fondo") = dr("Fondo")
                drfila("CodigoMnemonico") = dr("CodigoMnemonico")
                drfila("NumeroTitulo") = dr("NumeroTitulo")
                drfila("Instrumento") = dr("Instrumento")
                drfila("Duracion") = dr("Duracion")
                dsReporte.Tables(0).Rows.Add(drfila)
            Next
            oReport.SetDataSource(dsReporte)
            oReport.SetParameterValue("@FechaValoracion", DatFechaValoracion.ToShortDateString())
            oReport.SetParameterValue("@Fondo", StrDescripcionFondo)
            oReport.SetParameterValue("@Mnemonico", StrMnemonico)
            oReport.SetParameterValue("@Usuario", StrUsuario)
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            CrystalReportViewer1.ReportSource = oReport
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar el reporte")
        End Try
    End Sub
    Protected Sub Modulos_Gestion_frmVisorConsultaDuraciones_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class