Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Partial Class Modulos_Parametria_frmCronogramaPagos
    Inherits BasePage
    Private dtCronogramaPago As DataTable
    Private hdCodigoPortafolioSBS As HiddenField
    Private hdAccion As HiddenField
    Private tbFechaPago As TextBox
    Private ibEliminar As ImageButton
    Private divFecha As HtmlControl
    Private btnDetalle As HtmlControl
    Private oCronogramaPagosBM As New CronogramaPagosBM
    Private oRowCP As New CronogramaPagosBE(String.Empty, String.Empty, String.Empty)
    Private dtblDatos As DataTable

#Region "/* Métodos de la Página */"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                EstablecerFecha()
                ActivarGrabar(False)
                dgDetalleInstrumento.DataSource = Nothing
                dgDetalleInstrumento.DataBind()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página / " + ex.Message.ToString)
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir.")
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            If ddlFondo.SelectedIndex <= 0 Then
                AlertaJS("Debe seleccionar un fondo.")
                Exit Sub
            End If
            If tbFechaFin.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar una fecha.")
                Exit Sub
            End If
            dgCronogramaPagos.PageIndex = 0
            ViewState("Indica") = 1
            CargarGrilla()
            ActivarGrabar(False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub
    Protected Sub dgCronogramaPagos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCronogramaPagos.RowCommand
        Try
            Dim index As Int32 = 0
            If e.CommandName = "Add" Then
                tbFechaPago = CType(dgCronogramaPagos.FooterRow.FindControl("tbFechaPagoF"), TextBox)
                If tbFechaPago.Text.Length <= 0 Then
                    AlertaJS("Falta ingresar la fecha de pago.")
                    Exit Sub
                End If
                If ViewState("fechaNegocio") > UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text) Then
                    AlertaJS("La fecha de pago no puede ser menor a la fecha de negocio del portafolio.")
                    Exit Sub
                End If
                dtblDatos = oCronogramaPagosBM.CronogramaPagos_ListarbyRangoFechaPortafolio(ddlFondo.SelectedValue, ViewState("fechaNegocio"), UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text))
                For i = 0 To dtblDatos.Rows.Count - 1
                    If tbFechaPago.Text = dtblDatos.Rows(i).Item("fechaCronogramaPagos").ToString Then
                        AlertaJS("Verificar, la fecha de pago ya se encuentra registrada.")
                        Exit Sub
                    End If
                Next
                If ViewState("CronogramaPagos") IsNot Nothing Then
                    dtCronogramaPago = ViewState("CronogramaPagos")
                Else
                    dtCronogramaPago = Configurar()
                End If
                dtCronogramaPago = AgregarFila(1, ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text), tbFechaPago.Text, "N")
                ViewState("CronogramaPagos") = dtCronogramaPago
                ActivarGrabar(True)
                If dtCronogramaPago.Rows.Count > 0 Then btnGrabar.Enabled = True
            ElseIf e.CommandName = "Del" Then
                Dim Row As GridViewRow
                Dim i As Integer
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                tbFechaPago = CType(dgCronogramaPagos.Rows(i).FindControl("tbFechaPago"), TextBox)
                divFecha = CType(dgCronogramaPagos.Rows(i).FindControl("divFecha"), HtmlControl)
                ibEliminar = CType(dgCronogramaPagos.Rows(i).FindControl("imgDelete"), ImageButton)
                btnDetalle = CType(dgCronogramaPagos.Rows(i).FindControl("btnVerDetalle"), HtmlControl)
                tbFechaPago.Enabled = False
                btnDetalle.Disabled = True
                divFecha.Attributes.Add("class", "input-append")
                ibEliminar.Visible = False
                CType(ViewState("CronogramaPagos"), DataTable).Rows(i).Item(4) = "D"
                ActivarGrabar(True)
            End If
        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error en los registros del cronograma de pagos. / " + ex.Message.ToString, "'", String.Empty))
        Finally
            If ViewState("CronogramaPagos") IsNot Nothing Then
                If CType(ViewState("CronogramaPagos"), DataTable).Rows.Count > 0 Then
                    dtCronogramaPago = ViewState("CronogramaPagos")
                    Me.dgCronogramaPagos.DataSource = dtCronogramaPago
                Else
                    dtCronogramaPago = Configurar()
                    Me.dgCronogramaPagos.DataSource = AgregarFila(0, "-1", 0, String.Empty, String.Empty)
                End If
            Else
                dtCronogramaPago = Configurar()
                ViewState("CronogramaPagos") = Nothing
                Me.dgCronogramaPagos.DataSource = AgregarFila(0, "-1", 0, String.Empty, String.Empty)
            End If
                Me.dgCronogramaPagos.DataBind()
        End Try
    End Sub
    Protected Sub dgCronogramaPagos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgCronogramaPagos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                hdCodigoPortafolioSBS = CType(e.Row.FindControl("hdCodigoPortafolio"), HiddenField)
                tbFechaPago = CType(e.Row.FindControl("tbFechaPago"), TextBox)
                divFecha = CType(e.Row.FindControl("divFecha"), HtmlControl)
                ibEliminar = CType(e.Row.FindControl("imgDelete"), ImageButton)
                hdAccion = CType(e.Row.FindControl("hdAccion"), HiddenField)
                btnDetalle = CType(e.Row.FindControl("btnVerDetalle"), HtmlControl)

                e.Row.Visible = Not hdCodigoPortafolioSBS.Value.Trim.Equals("-1")
                tbFechaPago.Enabled = Not (ViewState("fechaNegocio") > UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text) Or hdAccion.Value = "D")
                ibEliminar.Visible = (tbFechaPago.Enabled)
                If Not tbFechaPago.Enabled Then divFecha.Attributes.Add("class", "input-append")
                btnDetalle.Disabled = IIf(hdAccion.Value = "D" Or hdAccion.Value = "N" Or hdAccion.Value = "E", True, False)
            End If
            If e.Row.RowType = DataControlRowType.Footer Then

            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar el listado de las fechas de pago / " + ex.Message.ToString())
        End Try
    End Sub
    Protected Sub dgCronogramaPagos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgCronogramaPagos.SelectedIndexChanged
        Try
            If ddlFondo.SelectedIndex > 0 Then
                tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue))
            Else
                tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ParametrosSIT.PORTAFOLIO_MULTIFONDOS))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            If CType(ViewState("CronogramaPagos"), DataTable).Rows.Count > 0 Then
                For i = 0 To dgCronogramaPagos.Rows.Count - 1
                    crearObjetoCronogramaPagos(i)
                    If oRowCP.Estado = "N" Then 'Nuevo Registro.
                        oCronogramaPagosBM.CronogramaPagos_Insertar(oRowCP, DatosRequest)
                    ElseIf oRowCP.Estado = "E" Or oRowCP.Estado = "D" Then 'Registro E = modificado / D = eliminado.
                        oCronogramaPagosBM.CronogramaPagos_Modificar(oRowCP, DatosRequest)
                    End If
                Next
            Else

            End If
            AlertaJS("Se generó el cronograma de pagos para el fondo: " & ddlFondo.SelectedItem.ToString())
            CargarGrilla()
            ActivarGrabar(False)
        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error al grabar la información / " + ex.Message.ToString, "'", String.Empty))
        End Try
    End Sub
    Protected Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        EstablecerFecha()
        CargarGrilla()
    End Sub
    Protected Sub tbFechaPago_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim cont As Integer
            For Each row As GridViewRow In dgCronogramaPagos.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    tbFechaPago = CType(row.Cells(1).FindControl("tbFechaPago"), TextBox)
                    If tbFechaPago.ClientID = CType(sender, TextBox).ClientID Then
                        Dim tbFechaPagoBusqueda As TextBox
                        If UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text) And _
                            UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text) <= UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text) Then
                            If ViewState("fechaNegocio") > UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text) Then
                                AlertaJS("La fecha de pago no puede ser menor a la fecha de negocio del portafolio.")
                                Exit Sub
                            End If
                            For i = 0 To dgCronogramaPagos.Rows.Count - 1
                                tbFechaPagoBusqueda = CType(dgCronogramaPagos.Rows(i).FindControl("tbFechaPago"), TextBox)
                                If tbFechaPago.ClientID <> tbFechaPagoBusqueda.ClientID Then
                                    If tbFechaPago.Text = tbFechaPagoBusqueda.Text Then
                                        AlertaJS("Verificar, la fecha de pago ya se encuentra registrada.")
                                        Exit Sub
                                    End If
                                End If
                            Next
                            hdAccion = CType(row.Cells(1).FindControl("hdAccion"), HiddenField)
                            If hdAccion.Value.ToUpper = "R" Or hdAccion.Value.ToUpper = "E" Then
                                CType(ViewState("CronogramaPagos"), DataTable).Rows(cont).Item(4) = "E"
                                CType(ViewState("CronogramaPagos"), DataTable).Rows(cont).Item(3) = tbFechaPago.Text
                                ActivarGrabar(True)
                            End If
                        Else
                            AlertaJS("Verificar, sólo se puede modificar la fecha de pago con valores dentro del rango de fechas seleccionado de búsqueda.")
                        End If
                    End If
                End If
                cont += 1
            Next

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", String.Empty))
        Finally
            dgCronogramaPagos.DataSource = ViewState("CronogramaPagos")
            dgCronogramaPagos.DataBind()
        End Try

    End Sub
#End Region
#Region "/* Métodos Personalizados */"
    Public Sub CargarCombos()
        UIUtility.CargarPortafoliosOI(ddlFondo)
    End Sub
    Private Sub CargarGrilla()
        dtblDatos = oCronogramaPagosBM.CronogramaPagos_ListarbyRangoFechaPortafolio(ddlFondo.SelectedValue, _
                                                                                                     UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text), _
                                                                                                     UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
        If Not (dtblDatos Is Nothing) Then
            Me.lblCantidad.Text = String.Format("({0})", dtblDatos.Rows.Count)
            ViewState("CronogramaPagos") = dtblDatos
        Else
            Me.lblCantidad.Text = String.Format("({0})", 0)
        End If
        If dtblDatos.Rows.Count = 0 Then
            dtCronogramaPago = Configurar()
            dtblDatos = AgregarFila(0, "-1", 0, String.Empty, String.Empty)
        End If

        dgCronogramaPagos.DataSource = dtblDatos
        dgCronogramaPagos.DataBind()
    End Sub
    Sub EstablecerFecha()
        If ddlFondo.SelectedValue <> String.Empty Then
            tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondo.SelectedValue))
        Else
            tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        End If
        ViewState("fechaNegocio") = UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text)
        tbFechaFin.Text = UIUtility.ConvertirFechaaString(CType(Convert.ToDateTime(tbFechaIni.Text).AddDays(360).ToString("yyyyMMdd"), Decimal))
    End Sub
    Private Function Configurar() As DataTable
        dtCronogramaPago = New DataTable
        dtCronogramaPago.Columns.Add(New DataColumn("idCronogramaPagos", GetType(Integer)))
        dtCronogramaPago.Columns.Add(New DataColumn("CodigoPortafolioSBS", GetType(String)))
        dtCronogramaPago.Columns.Add(New DataColumn("FechaPagos", GetType(Decimal)))
        dtCronogramaPago.Columns.Add(New DataColumn("fechaCronogramaPagos", GetType(String)))
        dtCronogramaPago.Columns.Add(New DataColumn("Accion", GetType(String)))
        Return dtCronogramaPago
    End Function
    Private Function AgregarFila(ByVal _idCronogramaPagos As Integer, ByVal _codigoPortafolioSBS As String, ByVal _FechaDecimal As Decimal, ByVal _fecha As String, ByVal _Accion As String) As DataTable
        Dim dr As DataRow = dtCronogramaPago.NewRow
        dr(0) = _idCronogramaPagos
        dr(1) = _codigoPortafolioSBS
        dr(2) = _FechaDecimal
        dr(3) = _fecha
        dr(4) = _Accion
        dtCronogramaPago.Rows.Add(dr)
        Return dtCronogramaPago
    End Function
    Private Sub crearObjetoCronogramaPagos(ByVal indice As Integer)
        oCronogramaPagosBM.InicializarCronogramaPagos(oRowCP)
        oRowCP.IDCronogramaPagos = CType(dgCronogramaPagos.Rows(indice).FindControl("hdIdCronogramaPagos"), HiddenField).Value
        oRowCP.FechaCronogramaPagos = UIUtility.ConvertirFechaaDecimal(CType(dgCronogramaPagos.Rows(indice).FindControl("tbFechaPago"), TextBox).Text)
        oRowCP.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRowCP.Estado = CType(dgCronogramaPagos.Rows(indice).FindControl("hdAccion"), HiddenField).Value
    End Sub
    Private Sub ActivarGrabar(ByVal estado As Boolean)
        lblFlagCambios.Visible = estado
        btnPrevGrabar.Disabled = Not estado
    End Sub
#End Region
End Class