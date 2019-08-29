Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Inversiones_Reportes_Orden_de_Inversion_frmVisorReporteOperacionesEjecutadas
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim titulo, usuario, nombre As String
        Dim sFechaIni, sFechaFin As Date
        Dim dFechaIni, dFechaFin As Decimal
        Dim sPortafolio As String = ""
        Dim nomPortafolio As String = ""
        sFechaIni = Request.QueryString("FInicio")
        sFechaFin = Request.QueryString("FFin")
        sPortafolio = Request.QueryString("pPortafolio")
        nomPortafolio = Request.QueryString("nomPortafolio")
        'If sPortafolio.Equals("--Seleccione--") Then
        If sPortafolio Is Nothing Then
            sPortafolio = ""
        End If
        '=======================================================
        '-- Cargamos infirmación del Contexto (si existe) - CRumiche
        Dim nombrePortafolio As String = ""

        If Session("context_info") IsNot Nothing Then
            If TypeOf Session("context_info") Is Hashtable Then 'Si se desea pasar otro tipo de objeto se debe validar de esta manera - CRumiche
                Dim htInfo As Hashtable = Session("context_info")

                If htInfo.Contains("Portafolio") Then nombrePortafolio = htInfo("Portafolio").ToString
                '// Obtener más valores aquí de la manera mostrada
            End If
        End If
        '=======================================================
        Session.Remove("context_info")
        titulo = Session("titulo")
        nombre = "Usuario"
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
        Select Case titulo
            Case "Renta Fija"
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                oReport.Load(Server.MapPath("RentaFija.rpt"))
                Dim dsAux As New RentaFijaBE
                Dim dsReporteRentaFija As New DsRentaFija
                dsAux = New ReportesInversionesBM().SeleccionarOperacionesRentaFija(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                CopiarTabla(dsAux.ReporteOperacionRentaFija, dsReporteRentaFija.ReporteOperacionRentaFija)
                Dim vFirma As Decimal = 0
                If Not Request.QueryString("VFirma") Is Nothing Then
                    If Request.QueryString("VFirma") = "1" Then
                        vFirma = 1
                    End If
                End If
                Dim dsAux2 As DataSet
                dsAux2 = New ReportesInversionesBM().ReporteOperacionRentaFija(dFechaIni, sPortafolio, vFirma, DatosRequest)
                Dim drFirma As DsRentaFija.FirmaRow
                Dim dr As DataRow
                For Each dr In dsAux2.Tables(1).Rows()
                    drFirma = CType(dsReporteRentaFija.Firma.NewFirmaRow(), DsRentaFija.FirmaRow)
                    drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(0), String)))
                    drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(1), String)))
                    drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(2), String)))
                    drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(3), String)))
                    drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(4), String)))
                    drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(5), String)))
                    drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(6), String)))
                    drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(7), String)))
                    drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(8), String)))
                    drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(9), String)))
                    drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(10), String)))
                    drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(11), String)))
                    drFirma.Portafolio = dr(12).ToString()
                    dsReporteRentaFija.Firma.AddFirmaRow(drFirma)
                Next
                dsReporteRentaFija.Firma.AcceptChanges()
                dsReporteRentaFija.Merge(dsReporteRentaFija, False, System.Data.MissingSchemaAction.Ignore)
                oReport.SetDataSource(dsReporteRentaFija)
                oReport.SetParameterValue("@FechaInicio", "Reporte de Operaciones de Renta Fija de " & sFechaIni.ToShortDateString() & "al " & sFechaFin.ToShortDateString())
                oReport.SetParameterValue("@Usuario", usuario)
                oReport.SetParameterValue("@Portafolio", IIf(sPortafolio = "", "TODOS", nomPortafolio))
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                CrystalReportViewer1.DisplayGroupTree = False
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Renta Variable"
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                oReport.Load(Server.MapPath("RentaVariable.rpt"))
                Dim dsAux As New RentaVariableBE
                Dim DsReporteRentaVariable As New DsRentaVariable
                dsAux = New ReportesInversionesBM().SeleccionarOperacionesRentaVariable(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                CopiarTabla(dsAux.ReporteOperacionRentaVariable, DsReporteRentaVariable.ReporteOperacionRentaVariable)
                Dim vFirma As Decimal = 0
                If Not Request.QueryString("VFirma") Is Nothing Then
                    vFirma = 1
                End If
                Dim dsAux2 As DataSet
                dsAux2 = New ReportesInversionesBM().ReporteOperacionRentaVariable(dFechaIni, sPortafolio, vFirma, DatosRequest)
                Dim drFirma As DsRentaVariable.FirmaRow
                Dim dr As DataRow
                For Each dr In dsAux2.Tables(1).Rows()
                    drFirma = CType(DsReporteRentaVariable.Firma.NewFirmaRow(), DsRentaVariable.FirmaRow)
                    drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(0), String)))
                    drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(1), String)))
                    drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(2), String)))
                    drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(3), String)))
                    drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(4), String)))
                    drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(5), String)))
                    drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(6), String)))
                    drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(7), String)))
                    drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(8), String)))
                    drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(9), String)))
                    drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(10), String)))
                    drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(11), String)))
                    drFirma.Portafolio = dr(12).ToString()
                    DsReporteRentaVariable.Firma.AddFirmaRow(drFirma)
                Next
                DsReporteRentaVariable.Firma.AcceptChanges()
                DsReporteRentaVariable.Merge(DsReporteRentaVariable, False, System.Data.MissingSchemaAction.Ignore)
                oReport.SetDataSource(DsReporteRentaVariable)
                oReport.SetParameterValue("@FechaInicio", "Reporte de Operaciones de Renta Variable de " & sFechaIni.ToShortDateString() & "al " & sFechaFin.ToShortDateString())
                oReport.SetParameterValue("@Usuario", usuario)
                oReport.SetParameterValue("Fondo", IIf(sPortafolio.Equals(""), "TODOS", nomPortafolio))
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Divisas"
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                oReport.Load(Server.MapPath("Divisas.rpt"))
                Dim dsAux As New DivisasBE
                Dim DsReporteDivisas As New DsDivisas
                dsAux = New ReportesInversionesBM().SeleccionarOperacionesDivisas(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                CopiarTabla(dsAux.ReporteOperacionDivisa, DsReporteDivisas.ReporteOperacionDivisa)
                Dim vFirma As Decimal = 0
                If Not Request.QueryString("VFirma") Is Nothing Then
                    vFirma = 1
                End If
                Dim dsAux2 As DataSet
                dsAux2 = New ReportesInversionesBM().ReporteOperacionDivisa(dFechaIni, sPortafolio, vFirma, DatosRequest)
                Dim drFirma As DsDivisas.FirmaRow
                Dim dr As DataRow
                For Each dr In dsAux2.Tables(1).Rows()
                    drFirma = CType(DsReporteDivisas.Firma.NewFirmaRow(), DsDivisas.FirmaRow)
                    drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(0), String)))
                    drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(1), String)))
                    drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(2), String)))
                    drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(3), String)))
                    drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(4), String)))
                    drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(5), String)))
                    drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(6), String)))
                    drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(7), String)))
                    drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(8), String)))
                    drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(9), String)))
                    drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(10), String)))
                    drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr(11), String)))
                    drFirma.Portafilio = dr(12).ToString()
                    DsReporteDivisas.Firma.AddFirmaRow(drFirma)
                Next
                DsReporteDivisas.Firma.AcceptChanges()
                DsReporteDivisas.Merge(DsReporteDivisas, False, System.Data.MissingSchemaAction.Ignore)
                oReport.SetDataSource(DsReporteDivisas)
                oReport.SetParameterValue("@FechaInicio", sFechaIni.ToShortDateString())
                oReport.SetParameterValue("@FechaFin", sFechaFin.ToShortDateString())
                oReport.SetParameterValue("@Usuario", usuario)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Por Correlativo"
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                oReport.Load(Server.MapPath("PorCorrelativo.rpt"))
                Dim dsAux As New PorCorrelativoBE
                Dim dsPorCorrelativo As New DsPorCorrelativo
                dsAux = New ReportesInversionesBM().SeleccionarPorCorrelativo(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                CopiarTabla(dsAux.PorCorrelativo, dsPorCorrelativo.PorCorrelativo)
                dsPorCorrelativo.Merge(dsPorCorrelativo, False, System.Data.MissingSchemaAction.Ignore)
                oReport.SetDataSource(dsPorCorrelativo)
                oReport.SetParameterValue("@FechaInicio", sFechaIni.ToShortDateString())
                oReport.SetParameterValue("@FechaFin", sFechaFin.ToShortDateString())
                oReport.SetParameterValue("@Usuario", usuario)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Por gestor"
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                oReport.Load(Server.MapPath("RptPorGestor.rpt"))
                Dim dsPorGestor As New DataSet
                Dim DsReportePorGestor As New DsPorGestor
                Dim dt As DataRow
                Dim dt2 As DataRow
                dsPorGestor = New ReportesInversionesBM().SeleccionarPorGestor(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                For Each dt In dsPorGestor.Tables(0).Rows()
                    dt2 = DsReportePorGestor.Tables(0).NewRow
                    dt2("CodigoOrden") = dt("CodigoOrden")
                    dt2("Fecha") = dt("Fecha")
                    dt2("Operacion") = dt("Operacion")
                    dt2("Moneda") = dt("Moneda")
                    dt2("CodigoTipoTitulo") = dt("CodigoTipoTitulo")
                    dt2("CodigoSBS") = dt("CodigoSBS")
                    dt2("Contraparte") = dt("Contraparte")
                    dt2("Emisor") = dt("Emisor")
                    dt2("HoraOperacion") = dt("HoraOperacion")
                    dt2("PTasa") = dt("PTasa")
                    dt2("YTM") = dt("YTM")
                    dt2("MontoNominal") = dt("MontoNominal")
                    dt2("MontoOperacion") = dt("MontoOperacion")
                    dt2("FechaLiquidacion") = dt("FechaLiquidacion")
                    dt2("Portafolio") = dt("Portafolio")
                    dt2("Gestor") = dt("Gestor")
                    DsReportePorGestor.Tables(0).Rows.Add(dt2)
                Next
                oReport.SetDataSource(DsReportePorGestor)
                oReport.SetParameterValue("RangoFechas", "Reporte de Operaciones Por Gestor de " & sFechaIni.ToShortDateString() & " Al " & sFechaFin.ToShortDateString())
                oReport.SetParameterValue("Usuario", usuario)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Por Gestor2" 'Reporte de OPERACIONES POR GESTOR ' Evaluado x CRumiche
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                Dim ds As DataSet, dsTipado As New DsPorGestor
                If Not Page.IsPostBack Then
                    Dim repBM As New ReportesInversionesBM
                    ds = repBM.SeleccionarPorGestor(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                    Session("dsConsulta") = ds
                Else
                    ds = CType(Session("dsConsulta"), DataSet)
                End If
                If ds.Tables.Count > 0 Then dsTipado.PorGestor.Merge(ds.Tables(0))
                oReport.Load(Server.MapPath("RptPorGestorHora.rpt"))
                oReport.SetDataSource(dsTipado)
                oReport.SetParameterValue("RangoFechas", "Reporte de Operaciones Por Gestor de " & sFechaIni.ToShortDateString() & " Al " & sFechaFin.ToShortDateString())
                oReport.SetParameterValue("Usuario", usuario)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Cash Call"
                dFechaIni = CType(sFechaIni.ToString("yyyyMMdd"), Decimal)
                dFechaFin = CType(sFechaFin.ToString("yyyyMMdd"), Decimal)
                oReport.Load(Server.MapPath("rptCashCall.rpt"))
                Dim dsAux As DataSet
                Dim dsCashCall As New DsCashCall
                dsAux = New ReportesInversionesBM().SeleccionarOperacionesCashCall(dFechaIni, dFechaFin, sPortafolio, DatosRequest)
                CopiarTabla(dsAux.Tables(0), dsCashCall.ReporteCashCall)
                dsCashCall.Merge(dsCashCall, False, System.Data.MissingSchemaAction.Ignore)
                oReport.SetDataSource(dsCashCall)
                oReport.SetParameterValue("@FechaInicio", sFechaIni.ToShortDateString())
                oReport.SetParameterValue("@FechaFin", sFechaFin.ToShortDateString())
                oReport.SetParameterValue("@Usuario", usuario)
                oReport.SetParameterValue("@Portafolio", IIf(sPortafolio = "", "TODOS", nomPortafolio))
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
        End Select
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(ex.Message.ToString)
            End Try
        Next
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_Orden_de_Inversion_frmVisorReporteOperacionesEjecutadas_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class