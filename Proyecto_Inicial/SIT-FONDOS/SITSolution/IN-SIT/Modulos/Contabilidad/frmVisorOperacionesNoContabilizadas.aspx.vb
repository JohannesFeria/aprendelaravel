Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Contabilidad_frmVisorOperacionesNoContabilizadas
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim StrEgreso, StrUsuario, StrNombre, StrFondo, strDescripcionFondo As String
            Dim DatFechaProceso As Date
            Dim DecFechaProceso As Decimal
            DatFechaProceso = Request.QueryString("vfechaproceso")
            StrFondo = Session("ReporteContabilidad_Fondo")
            strDescripcionFondo = Request.QueryString("vdescripcionFondo")
            StrEgreso = Request.QueryString("vegreso")
            StrNombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            StrUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
            DecFechaProceso = CType(DatFechaProceso.ToString("yyyyMMdd"), Decimal)
            Dim drfila As DataRow            
            oReport.Load(Server.MapPath("ReporteOperacionesNoContabilizadas.rpt"))
            Dim dsReporte As New OperacionesNoContabilizadas
            Dim oreporte As DataSet
            oreporte = New CuentasPorCobrarBM().OperacionesNoContabilizadas(DecFechaProceso, StrFondo, StrEgreso, DatosRequest)
            For Each dr As DataRow In oreporte.Tables(0).Rows
                drfila = dsReporte.Tables(0).NewRow()
                drfila("Matriz") = dr("Matriz")
                drfila("TipoOperacion") = dr("TipoOperacion")
                drfila("Operacion") = dr("Operacion")
                drfila("FormaPago") = dr("FormaPago")
                drfila("Moneda") = dr("Moneda")
                drfila("ClaseInstrumento") = dr("ClaseInstrumento")
                drfila("TipoInstrumento") = dr("TipoInstrumento")
                drfila("SectorEconomico") = dr("SectorEconomico")
                drfila("NroRegistro") = dr("NroRegistro")
                drfila("Descripcion") = dr("Descripcion")
                drfila("MontoTotal") = dr("MontoTotal")
                drfila("UsuarioCreacion") = dr("UsuarioCreacion")
                drfila("NumeroCuenta") = dr("NumeroCuenta")
                dsReporte.Tables(0).Rows.Add(drfila)
            Next
            oReport.SetDataSource(dsReporte)
            oReport.SetParameterValue("@FechaProceso", DatFechaProceso.ToShortDateString())
            oReport.SetParameterValue("@Fondo", strDescripcionFondo)
            oReport.SetParameterValue("@Usuario", StrUsuario)
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            CrystalReportViewer1.ReportSource = oReport
        Catch ex As Exception
            AlertaJS("Ocurrió un error al imprimir - " & ex.Message)
        End Try
    End Sub
    Protected Sub Modulos_Contabilidad_frmVisorOperacionesNoContabilizadas_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class