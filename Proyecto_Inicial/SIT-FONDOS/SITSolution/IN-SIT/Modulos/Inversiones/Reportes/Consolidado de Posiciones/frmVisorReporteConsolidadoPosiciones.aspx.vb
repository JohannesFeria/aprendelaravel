Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Web.Mail
Imports System.Data
Partial Class Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmVisorReporteConsolidadoPosiciones
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                GeneraLimitesExcedidos()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub GeneraLimitesExcedidos()
        Dim dsOrigen As New DataSet
        Dim Portafolio As String = Request.QueryString("pPortafolio")
        Dim FechaLimite As String = Request.QueryString("pFecha")
        Dim Escenario As String = Request.QueryString("pEscenario") 'RGF 20090924
        dsOrigen = New ReporteLimitesBM().Seleccionar_ConsolidadoLimitesExcedidos(UIUtility.ConvertirFechaaDecimal(FechaLimite), Portafolio, Escenario, DatosRequest) 'RGF 20090924
        Dim dtConsulta As New DataTable
        Dim ds As New DsReporteConsolidadoPosicion
        Dim dr As DsReporteConsolidadoPosicion.ReporteConsolidadoPosicionRow        
        dtConsulta = dsOrigen.Tables(0)
        If dtConsulta.Rows.Count > 0 Then
            For Each drv As DataRow In dtConsulta.Rows
                dr = ds.Tables(0).NewRow()
                dr("CodigoLimite") = drv("CodigoLimite")
                dr("NombreLimite") = drv("NombreLimite")
                dr("DescripcionNivel") = drv("DescripcionNivel")
                dr("ValorPorcentaje") = drv("ValorPorcentaje")
                dr("Posicion") = drv("Posicion")
                dr("Margen") = drv("Margen")
                dr("Secuencial") = drv("Secuencial")
                dr("Alerta") = drv("Alerta") 'RGF 20090203
                dr("Fondo") = drv("Fondo") 'HDG OT 61566 Nro3 20101029
                ds.Tables(0).Rows.Add(dr)
            Next
            oReport.Load(Server.MapPath("LimitesExcedidos.rpt"))
            oReport.SetDataSource(ds)
            oReport.SetParameterValue("FechaProceso", FechaLimite)
            oReport.SetParameterValue("Fondo", Portafolio)
            oReport.SetParameterValue("Usuario", Usuario)
            oReport.SetParameterValue("Escenario", Escenario) 'RGF 20090924
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            Dim rptStream As New System.IO.MemoryStream
            'se envia el reporte el stram y le indicamos el metodo de escritura o tipo de documento
            rptStream = CType(oReport.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat), System.IO.MemoryStream)
            'Limpiamos la memoria
            Response.Clear()
            Response.Buffer = True
            'Le indicamos el tipo de documento que vamos a exportar
            Response.ContentType = "application/pdf"
            'Automaticamente se descarga el archivo
            'Response.AddHeader("Content-Disposition", "attachment;filename=" + "Llamado.pdf")
            Response.AddHeader("Content-Disposition", "inline;filename=" + "LimitesExcedidos.pdf")
            'Se escribe el archivo
            Response.BinaryWrite(rptStream.ToArray())
            Response.End()
        Else
            AlertaJS("No se encontraron registros.", "window.close();")
        End If
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmVisorReporteConsolidadoPosiciones_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.close()
        oReport.Dispose()
    End Sub
End Class