Imports SIT.BusinessEntities
Imports SIT.BusinessLayer
Imports System.IO
Imports System.Text
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Public Class Modulos_PrevisionPagos_frmGenerarMemo
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTipoReporte()
            CargarTipoOperacion()

            ddlTipoReporte.SelectedIndex = 0
            ddlTipoOperacion.SelectedIndex = 0

            Label1.Text = "-" + Usuario
        End If
    End Sub
    Protected Sub btnParametria_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnParametria.Click
        Response.Redirect("frmParametriaMemo.aspx")
    End Sub
    Private Sub CargarTipoReporte()
        Dim objBM As New PrevisionMemoBM()
        Dim ds As New DataSet()
        ds = objBM.SeleccionarTipoReportexUsuario("11", Usuario) ' Tabla "USUARIO MEMO GENERAL" en PARAMETRIA
        HelpCombo.LlenarComboBox(ddlTipoReporte, ds.Tables(0), "Valor", "descripcion", True)
    End Sub
    Private Sub CargarTipoOperacion()
        Dim objBM As New PrevisionMemoBM()
        Dim ds As New DataSet()
        ds = objBM.SeleccionarTipoOperacionxUsuario("4", Usuario)
        HelpCombo.LlenarComboBox(ddlTipoOperacion, ds.Tables(0), "Valor", "descripcion", True)
    End Sub
    Protected Sub bSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bSalir.Click
        Response.Redirect("../../Bienvenida.aspx", False)
    End Sub
    Protected Sub ddlTipoReporte_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoReporte.SelectedIndexChanged
        If ddlTipoReporte.SelectedValue.ToString() = "TIP1" Then
            ddlTipoOperacion.Enabled = True
        Else
            ddlTipoOperacion.SelectedIndex = 0
            ddlTipoOperacion.Enabled = False
        End If
    End Sub
    Protected Sub btGenerar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btGenerar.Click
        Dim objBM As New PrevisionMemoBM()
        Dim ds As New DataSet()
        If ddlTipoReporte.SelectedValue.ToString() = "TIP1" Then
            If ddlTipoOperacion.SelectedIndex = 0 Then
                AlertaJS("Seleccione un Tipo de Operación.")
            Else
                If tbFechaInicio.Text = "" Then
                    AlertaJS("Por favor ingrese la Fecha de Pago.")
                Else
                    ds = objBM.SeleccionarReporteTipoOperacion(ddlTipoOperacion.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
                    If ds.Tables(0).Rows.Count > 0 And ds.Tables(1).Rows.Count > 0 Then
                        Dim TipoReporte As String = ""
                        Dim TipoOperacion As String = ""
                        Dim Fechas As String = ""
                        TipoReporte = "TIP1"
                        TipoOperacion = ddlTipoOperacion.SelectedValue.ToString()
                        Fechas = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text).ToString()
                        'Reporte 1
                        If TipoReporte = "TIP1" Then
                            Dim oobjBM As New PrevisionMemoBM()
                            Dim dss As New DataSet()
                            dss = oobjBM.SeleccionarReporteTipoOperacion(TipoOperacion, CDec(Fechas))
                            Dim dsReporte As New dsReporteOperacion
                            Dim drfila As DataRow                            
                            oReport.Load(Server.MapPath("MemoTipoOperacion.rpt"))
                            For Each dr As DataRow In dss.Tables(1).Rows
                                drfila = dsReporte.Tables(0).NewRow()
                                drfila("Banco") = dr("Banco")
                                drfila("CuentaCargo") = dr("CuentaCar")
                                drfila("CuentaDestino") = dr("CuentaDes")
                                drfila("Moneda") = dr("Moneda")
                                drfila("Monto") = dr("Monto")
                                dsReporte.Tables(0).Rows.Add(drfila)
                            Next
                            'Fecha
                            Dim mes As Integer = DateTime.Now.Month
                            Dim dia As Integer = DateTime.Now.Day
                            Dim anio As Integer = DateTime.Now.Year
                            Dim fecha As String = ""
                            Select Case mes
                                Case 1
                                    fecha = dia.ToString() + " de Enero de " + anio.ToString()
                                Case 2
                                    fecha = dia.ToString() + " de Febrero de " + anio.ToString()
                                Case 3
                                    fecha = dia.ToString() + " de Marzo de " + anio.ToString()
                                Case 4
                                    fecha = dia.ToString() + " de Abril de " + anio.ToString()
                                Case 5
                                    fecha = dia.ToString() + " de Mayo de " + anio.ToString()
                                Case 6
                                    fecha = dia.ToString() + " de Junio de " + anio.ToString()
                                Case 7
                                    fecha = dia.ToString() + " de Julio de " + anio.ToString()
                                Case 8
                                    fecha = dia.ToString() + " de Agosto de " + anio.ToString()
                                Case 9
                                    fecha = dia.ToString() + " de Septiembre de " + anio.ToString()
                                Case 10
                                    fecha = dia.ToString() + " de Octubre de " + anio.ToString()
                                Case 11
                                    fecha = dia.ToString() + " de Noviembre de " + anio.ToString()
                                Case Else
                                    fecha = dia.ToString() + " de Diciembre de " + anio.ToString()
                            End Select
                            oReport.SetDataSource(dsReporte)
                            oReport.SetParameterValue("@Fecha", fecha)
                            oReport.SetParameterValue("@A", dss.Tables(0).Rows(0)("DescripcionA").ToString())
                            oReport.SetParameterValue("@De", dss.Tables(0).Rows(0)("DescripcionDe").ToString())
                            oReport.SetParameterValue("@Referencia", dss.Tables(0).Rows(0)("Referencia").ToString())
                            oReport.SetParameterValue("@Contenido", dss.Tables(0).Rows(0)("Contenido").ToString())
                            oReport.SetParameterValue("@Despedida", dss.Tables(0).Rows(0)("Despedida").ToString())
                            oReport.SetParameterValue("@UsuarioFirma", dss.Tables(0).Rows(0)("UsuarioFirma").ToString())
                            oReport.SetParameterValue("@AreaUsuarioFirma", dss.Tables(0).Rows(0)("AreaUsuarioFirma").ToString())
                            oReport.SetParameterValue("@InicialesDocumentador", dss.Tables(0).Rows(0)("InicialesDocumentador").ToString())
                            Dim rptStream As New System.IO.MemoryStream
                            rptStream = CType(oReport.ExportToStream(CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat), System.IO.MemoryStream)
                            'Limpiamos la memoria
                            Response.Clear()
                            Response.Buffer = True
                            'Le indicamos el tipo de documento que vamos a exportar
                            Response.ContentType = "application/pdf"
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + "Memo.pdf")
                            'Se escribe el archivo
                            Response.BinaryWrite(rptStream.ToArray())
                            Response.End()
                        End If
                    Else
                        AlertaJS("No hay Información para Generar el Memo.")
                    End If
                End If
            End If
        Else
            If ddlTipoReporte.SelectedValue.ToString() = "TIP2" Then
                If tbFechaInicio.Text = "" Then
                    AlertaJS("Por favor ingrese la Fecha de Pago.")
                Else
                    ds = objBM.SeleccionarReporteGeneral(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
                    If ds.Tables(0).Rows.Count > 0 And ds.Tables(1).Rows.Count > 0 Then
                        Dim TipoReporte As String = ""
                        Dim TipoOperacion As String = ""
                        Dim Fechas As String = ""
                        TipoReporte = "TIP2"
                        TipoOperacion = ddlTipoOperacion.SelectedValue.ToString()
                        Fechas = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text).ToString()
                        Dim oobjBM As New PrevisionMemoBM()
                        Dim dss As New DataSet()
                        dss = oobjBM.SeleccionarReporteGeneral(CDec(Fechas))
                        Dim dsReporte As New dsReporteGeneral
                        Dim drfila As DataRow
                        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                        oReport.Load(Server.MapPath("MemoGeneral.rpt"))
                        For Each dr As DataRow In dss.Tables(1).Rows
                            drfila = dsReporte.Tables(0).NewRow()
                            drfila("Banco") = dr("Banco")
                            drfila("CuentaCargo") = dr("CuentaCar")
                            drfila("CuentaDestino") = dr("CuentaDes")
                            drfila("Moneda") = IIf(dr("Moneda").ToString.Substring(0, 1) = "D", "$.", "S/.")
                            drfila("Monto") = dr("Monto")
                            dsReporte.Tables(0).Rows.Add(drfila)
                        Next
                        'Fecha
                        Dim mes As Integer = DateTime.Now.Month
                        Dim dia As Integer = DateTime.Now.Day
                        Dim anio As Integer = DateTime.Now.Year
                        Dim fecha As String = ""
                        Select Case mes
                            Case 1
                                fecha = dia.ToString() + " de Enero de " + anio.ToString()
                            Case 2
                                fecha = dia.ToString() + " de Febrero de " + anio.ToString()
                            Case 3
                                fecha = dia.ToString() + " de Marzo de " + anio.ToString()
                            Case 4
                                fecha = dia.ToString() + " de Abril de " + anio.ToString()
                            Case 5
                                fecha = dia.ToString() + " de Mayo de " + anio.ToString()
                            Case 6
                                fecha = dia.ToString() + " de Junio de " + anio.ToString()
                            Case 7
                                fecha = dia.ToString() + " de Julio de " + anio.ToString()
                            Case 8
                                fecha = dia.ToString() + " de Agosto de " + anio.ToString()
                            Case 9
                                fecha = dia.ToString() + " de Septiembre de " + anio.ToString()
                            Case 10
                                fecha = dia.ToString() + " de Octubre de " + anio.ToString()
                            Case 11
                                fecha = dia.ToString() + " de Noviembre de " + anio.ToString()
                            Case Else
                                fecha = dia.ToString() + " de Diciembre de " + anio.ToString()
                        End Select
                        oReport.SetDataSource(dsReporte)
                        oReport.SetParameterValue("@Fecha", fecha)
                        oReport.SetParameterValue("@A", dss.Tables(0).Rows(0)("DescripcionA").ToString())
                        oReport.SetParameterValue("@De", dss.Tables(0).Rows(0)("DescripcionDe").ToString())
                        oReport.SetParameterValue("@Referencia", dss.Tables(0).Rows(0)("Referencia").ToString())
                        oReport.SetParameterValue("@Contenido", dss.Tables(0).Rows(0)("Contenido").ToString())
                        oReport.SetParameterValue("@Despedida", dss.Tables(0).Rows(0)("Despedida").ToString())
                        oReport.SetParameterValue("@UsuarioFirma", dss.Tables(0).Rows(0)("UsuarioFirma").ToString())
                        oReport.SetParameterValue("@AreaUsuarioFirma", dss.Tables(0).Rows(0)("AreaUsuarioFirma").ToString())
                        oReport.SetParameterValue("@InicialesDocumentador", dss.Tables(0).Rows(0)("InicialesDocumentador").ToString())
                        Dim rptStream As New System.IO.MemoryStream
                        rptStream = CType(oReport.ExportToStream(CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat), System.IO.MemoryStream)
                        'Limpiamos la memoria
                        Response.Clear()
                        Response.Buffer = True
                        'Le indicamos el tipo de documento que vamos a exportar
                        Response.ContentType = "application/pdf"
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + "Memo.pdf")
                        'Se escribe el archivo
                        Response.BinaryWrite(rptStream.ToArray())
                        Response.End()
                    Else
                        AlertaJS("No hay Información para Generar el Memo.")
                    End If
                End If
            Else
                AlertaJS("Seleccione un Tipo de Memo.")
            End If
        End If
    End Sub
    Protected Sub Alerta(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
    End Sub
    Protected Sub Modulos_PrevisionPagos_frmGenerarMemo_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class