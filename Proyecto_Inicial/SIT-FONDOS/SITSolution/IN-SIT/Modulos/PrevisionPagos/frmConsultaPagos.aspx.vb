Imports Sit.BusinessEntities
Imports SIT.BusinessLayer
Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Threading
Imports UIUtility

Public Class Modulos_PrevisionPagos_frmConsultaPagos
    Inherits BasePage

    'Dim IdUsuario As String = Usuario.ToString

    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBox(ddl, tablaListaParametria, "Valor", "Descripcion", True)
    End Sub

    Private Sub LlenarGrillaDetalle_Export()
        Dim Fondo0, Fondo1, Fondo2, Fondo3, FondoAdm, Total As Double

        Dim strTipoOperacion, strEstado As String
        If ddlTipoOperacion.SelectedIndex <= 0 Then
            strTipoOperacion = "0"
        Else
            strTipoOperacion = ddlTipoOperacion.SelectedValue.Trim
        End If
        If ddlEstado.SelectedIndex <= 0 Then
            strEstado = "0"
        Else
            strEstado = ddlEstado.SelectedValue.Trim
        End If

        Dim oDataSet As DataSet = PrevisionPagoDetalleBM.ListarPrevisionPagoDetalleToExport(UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text.Trim), strTipoOperacion, strEstado)
        Dim oRow As DataRow = oDataSet.Tables(0).NewRow

        For Each row As DataRow In oDataSet.Tables(0).Rows

            Fondo0 = Fondo0 + Convert.ToDouble(row("Fondo0").ToString)
            Fondo1 = Fondo1 + Convert.ToDouble(row("Fondo1").ToString)
            Fondo2 = Fondo2 + Convert.ToDouble(row("Fondo2").ToString)
            Fondo3 = Fondo3 + Convert.ToDouble(row("Fondo3").ToString)
            FondoAdm = FondoAdm + Convert.ToDouble(row("FondoAdm").ToString)
            Total = Total + Convert.ToDouble(row("Total").ToString)
        Next

        oRow("Banco") = "TOTALES:"
        oRow("Fondo0") = String.Format("{0:0.00}", Fondo0)
        oRow("Fondo1") = String.Format("{0:0.00}", Fondo1)
        oRow("Fondo2") = String.Format("{0:0.00}", Fondo2)
        oRow("Fondo3") = String.Format("{0:0.00}", Fondo3)
        oRow("FondoAdm") = String.Format("{0:0.00}", FondoAdm)
        oRow("Total") = String.Format("{0:0.00}", Total)

        oDataSet.Tables(0).Rows.Add(oRow)

        Dim myDS As New Data.DataSet("Provision")
        Dim myCustomers As Data.DataTable = myDS.Tables.Add("ProvisionDetalle")

        With myCustomers
            .Columns.Add("Banco", Type.GetType("System.String"))
            .Columns.Add("IdEntidad1", Type.GetType("System.String"))
            .Columns.Add("Fondo1", Type.GetType("System.String"))
            .Columns.Add("IdEntidad2", Type.GetType("System.String"))
            .Columns.Add("Fondo2", Type.GetType("System.String"))
            .Columns.Add("IdEntidad3", Type.GetType("System.String"))
            .Columns.Add("Fondo3", Type.GetType("System.String"))
            .Columns.Add("IdEntidadADM", Type.GetType("System.String"))
            .Columns.Add("FondoAdm", Type.GetType("System.String"))
            .Columns.Add("IdEntidad0", Type.GetType("System.String"))
            .Columns.Add("Fondo0", Type.GetType("System.String"))
            .Columns.Add("Total", Type.GetType("System.String"))
        End With

        Dim myDr As Data.DataRow

        For Each row As DataRow In oDataSet.Tables(0).Rows
            myDr = myCustomers.NewRow()
            myDr("Banco") = row("Banco").ToString
            myDr("IdEntidad1") = row("IdEntidad1").ToString
            myDr("Fondo1") = IIf(row("Fondo1").ToString = "0.00", "", row("Fondo1").ToString)
            myDr("IdEntidad2") = row("IdEntidad2").ToString
            myDr("Fondo2") = IIf(row("Fondo2").ToString = "0.00", "", row("Fondo2").ToString)
            myDr("IdEntidad3") = row("IdEntidad3").ToString
            myDr("Fondo3") = IIf(row("Fondo3").ToString = "0.00", "", row("Fondo3").ToString)
            myDr("IdEntidadADM") = row("IdEntidadADM").ToString
            myDr("FondoAdm") = IIf(row("FondoAdm").ToString = "0.00", "", row("FondoAdm").ToString)
            myDr("IdEntidad0") = row("IdEntidad0").ToString
            myDr("Fondo0") = IIf(row("Fondo0").ToString = "0.00", "", row("Fondo0").ToString)
            myDr("Total") = IIf(row("Total").ToString = "0.00", "", row("Total").ToString)

            myCustomers.Rows.Add(myDr)
        Next

        gvDetallePagoToExport.DataSource = myCustomers 'oDataSet
        gvDetallePagoToExport.DataBind()

    End Sub

    Private Sub LlenarGrillaDetalle()

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-PE")

        Dim strTipoOperacion, strEstado As String

        If ddlTipoOperacion.SelectedIndex <= 0 Then
            strTipoOperacion = "0"
        Else
            strTipoOperacion = ddlTipoOperacion.SelectedValue.Trim
        End If
        If ddlEstado.SelectedIndex <= 0 Then
            strEstado = "0"
        Else
            strEstado = ddlEstado.SelectedValue.Trim
        End If

        gvPagos.DataSource = Nothing
        gvPagos.DataBind()

        Dim oDataSet As DataSet = PrevisionPagoDetalleBM.ListarPrevisionPagoDetalle(UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text.Trim), strTipoOperacion, strEstado)
        If oDataSet.Tables(0).Rows.Count > 0 Then

            Dim oRow As DataRow = oDataSet.Tables(0).NewRow
            Dim Ingreso0, Egreso0, Ingreso1, Egreso1, Ingreso2, Egreso2, Ingreso3, Egreso3, IngresoAdm, EgresoAdm, Total As Double

            For Each row As DataRow In oDataSet.Tables(0).Rows
                Ingreso0 = Ingreso0 + Convert.ToDouble(row("Ingreso0").ToString)
                Egreso0 = Egreso0 + Convert.ToDouble(row("Egreso0").ToString)
                Ingreso1 = Ingreso1 + Convert.ToDouble(row("Ingreso1").ToString)
                Egreso1 = Egreso1 + Convert.ToDouble(row("Egreso1").ToString)
                Ingreso2 = Ingreso2 + Convert.ToDouble(row("Ingreso2").ToString)
                Egreso2 = Egreso2 + Convert.ToDouble(row("Egreso2").ToString)
                Ingreso3 = Ingreso3 + Convert.ToDouble(row("Ingreso3").ToString)
                Egreso3 = Egreso3 + Convert.ToDouble(row("Egreso3").ToString)
                IngresoAdm = IngresoAdm + Convert.ToDouble(row("IngresoADM").ToString)
                EgresoAdm = EgresoAdm + Convert.ToDouble(row("EgresoADM").ToString)
                Total = Total + Convert.ToDouble(row("Total").ToString)
            Next

            oRow("Ingreso0") = String.Format("{0:0.00}", Ingreso0)
            oRow("Egreso0") = String.Format("{0:0.00}", Egreso0)
            oRow("Ingreso1") = String.Format("{0:0.00}", Ingreso1)
            oRow("Egreso1") = String.Format("{0:0.00}", Egreso1)
            oRow("Ingreso2") = String.Format("{0:0.00}", Ingreso2)
            oRow("Egreso2") = String.Format("{0:0.00}", Egreso2)
            oRow("Ingreso3") = String.Format("{0:0.00}", Ingreso3)
            oRow("Egreso3") = String.Format("{0:0.00}", Egreso3)
            oRow("IngresoADM") = String.Format("{0:0.00}", IngresoAdm)
            oRow("EgresoADM") = String.Format("{0:0.00}", EgresoAdm)
            oRow("Total") = String.Format("{0:0.00}", Total)

            oDataSet.Tables(0).Rows.Add(oRow)

            gvPagosCab.Visible = True
            gvPagos.DataSource = oDataSet
            gvPagos.DataBind()

            Dim nFilas As Integer = gvPagos.Rows.Count - 1
            Dim nColumnas As Integer = gvPagos.Columns.Count - 1

            For i As Integer = 0 To gvPagos.Rows.Count - 1
                gvPagos.Rows(i).Cells(nColumnas).BackColor = Color.Yellow
            Next

            For i As Integer = 0 To gvPagos.Columns.Count - 1
                gvPagos.Rows(nFilas).Cells(i).BackColor = Color.Yellow
            Next

            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(oDataSet.Tables(0).Rows.Count - 1) + "');")

        End If
    End Sub

    Private Sub CargarControles()
        CargarCombos(ddlTipoOperacion, 4)
        CargarCombos(ddlEstado, 5)
        txtFechaPago.Text = String.Empty
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CargarControles()
            Me.ddlEstado.SelectedValue = "APR"
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            'If txtFechaPago.Text = String.Empty Then
            '    RequiredFieldValidator1.Visible = True
            '    RequiredFieldValidator1.Enabled = True
            '    RequiredFieldValidator1.Text = "(*)"
            '    RequiredFieldValidator1.ErrorMessage = "(*)"
            'Else
            '    RequiredFieldValidator1.Visible = False
            If Me.ddlEstado.SelectedValue = "" Then
                AlertaJS("Tiene que seleccionar un Estado")
                Exit Sub
            End If
            LlenarGrillaDetalle()
            'End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportar.Click
        Try
            If Me.ddlEstado.SelectedValue = "" Then
                AlertaJS("Tiene que seleccionar un Estado")
                Exit Sub
            End If

            If gvPagos.Rows.Count > 0 Then
                LlenarGrillaDetalle_Export()

                Dim pathImagen As String
                Dim _dt As New System.Data.DataTable()
                _dt = PrevisionParametriaBM.ListarParametria(14).Tables(0)
                pathImagen = _dt.Rows(0)("Descripcion").ToString() 'Server.MapPath("../../App_Themes/img/integra.png")

                Dim sb As New StringBuilder()
                Dim sw As New StringWriter(sb)
                Dim htw As New HtmlTextWriter(sw)

                Dim page As New Page()
                Dim form As New HtmlForm()
                Dim HtmlBody As String

                HtmlBody = "<div><table>" &
                            "<tr>" &
                            "<td></td><td></td><td></td><td></td><td></td><td></td><td></td>" &
                            "<td>" &
                            "<br>Fecha Emision:</br>" &
                            "<br>Hora Emision:</br>" &
                            "<br>Usuario:</br>" &
                            "</td>" &
                            "<td>" &
                            "<br style=" + "float: right;" + ">&nbsp;&nbsp;" + System.DateTime.Now.ToString("dd/MM/yyyy") + "</br>" &
                            "<br style=" + "float: right;" + ">&nbsp;&nbsp;" + System.DateTime.Now.ToString("hh:mm:ss tt") + "</br>" &
                            "<br style=" + "float: right;" + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Usuario + "</br>" &
                            "</td>" &
                            "</tr>" &
                            "</table></div>"

                sb.Append(HtmlBody)

                'sb.Append("<div>")
                'sb.Append("<tr>")
                'sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td>")
                'sb.Append("<b>Fecha Emision: " + System.DateTime.Now.ToString("dd/MM/yyyy") + "</b>")
                'sb.Append("<b>Hora Emision: " + System.DateTime.Now.ToString("hh:mm:ss tt") + "</b>")
                'sb.Append("<b>Usuario: " + IdUsuario.ToString + "</b>")
                'sb.Append("</td>")
                'sb.Append("</tr>")
                'sb.Append("</div>")


                ' Deshabilitar la validación de eventos, sólo asp.net 2
                page.EnableEventValidation = False

                ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
                page.DesignerInitialize()

                page.Controls.Add(form)
                form.Controls.Add(gvDetallePagoToExport)

                page.RenderControl(htw)

                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("Content-Disposition", "attachment;filename=Export.xls")
                Response.Charset = "UTF-8"
                Response.ContentEncoding = Encoding.[Default]
                'Dim fi As System.IO.FileInfo
                'fi = New System.IO.FileInfo(Server.MapPath("../../Common/Imagenes/logo.gif"))

                'Dim style As String = "<style> td { mso-number-format:" + "0\.0000000" + "; } </style> "
                Dim style As String = "<style> td { mso-number-format:" + "\@" + "; } </style> "
                'Dim tag As String = "<img src='" & fi.ToString & "' width='220' eight='100'/>"
                Dim tag As String = "<img src='" & pathImagen.ToString & "' width='220' eight='100'/>"
                Response.Write(tag)
                Response.Write(style.ToString())
                Response.Write(sb.ToString())
                Response.[End]()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub gvPagos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPagos.RowDataBound
        Select Case e.Row.RowType
            Case DataControlRowType.Header

                'Agrupando las dos primeras columnas  (col=1, col=2)
                e.Row.Cells(1).ColumnSpan = 2
                'e.Row.Cells(1).Text = "Fondo 0"
                e.Row.Cells(1).Text = "<table width=" + "100%" + " ><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 0</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(2).Visible = False

                'Agrupando las dos ultimas columnas (col=3 y col=4)
                e.Row.Cells(3).ColumnSpan = 2
                'e.Row.Cells(3).Text = "Fondo 1"
                e.Row.Cells(3).Text = "<table width=" + "100%" + " ><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 1</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(4).Visible = False

                'Agrupando las dos primeras columnas  (col=8, col=9)
                e.Row.Cells(5).ColumnSpan = 2
                'e.Row.Cells(5).Text = "Fondo 2"
                e.Row.Cells(5).Text = "<table width=" + "100%" + " ><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 2</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(6).Visible = False

                'Agrupando las dos primeras columnas  (col=10, col=11)
                e.Row.Cells(7).ColumnSpan = 2
                'e.Row.Cells(7).Text = "Fondo 3"
                e.Row.Cells(7).Text = "<table width=" + "100%" + " ><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 3</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(8).Visible = False

                'Agrupando las dos primeras columnas  (col=12, col=13)
                e.Row.Cells(9).ColumnSpan = 2
                'e.Row.Cells(12).Text = "Fondo Adm"
                e.Row.Cells(9).Text = "<table width=" + "100%" + " ><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo Adm</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(10).Visible = False

        End Select
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

End Class
