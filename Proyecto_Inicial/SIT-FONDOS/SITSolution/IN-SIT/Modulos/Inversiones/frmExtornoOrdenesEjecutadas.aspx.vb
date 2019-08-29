Imports ParametrosSIT
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_frmExtornoOrdenesEjecutadas
    Inherits BasePage
#Region "Rutinas"
    Private Sub ShowDialogPopup(ByVal StrURL As String)
        Dim FechaOperacion As String
        Dim NroOrden As String
        Dim Portafolio As String
        FechaOperacion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text).ToString
        NroOrden = Me.txtNroOrdenOE.Text
        If (Me.ddlFondoOE.SelectedIndex = 0) Then
            Portafolio = ""
        Else
            Portafolio = Me.ddlFondoOE.SelectedValue.ToString
        End If

        

        Dim script As New StringBuilder

        EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '');")
        'EjecutarJS("window.location.href = 'frmExtornoOrdenesEjecutadas.aspx?vFechaOperacion='" & FechaOperacion & "'&vNroOrden='" & NroOrden & "'&vPortafolio='" & Portafolio & ";")

        'With script
        '    .Append("<script>")
        '    .Append("function PopupBuscador(FechaOperacion, NroOrden, Portafolio)")
        '    .Append("{")
        '    .Append("   var argument = new Object(FechaOperacion, NroOrden, Portafolio);")
        '    .Append("   window.showModalDialog('" + StrURL + "', argument, 'dialogHeight:600px; dialogWidth:950px; dialogLeft:125px;');")
        '    .Append("   window.location.href = 'frmExtornoOrdenesEjecutadas.aspx?vFechaOperacion='+FechaOperacion+'&vNroOrden='+NroOrden+'&vPortafolio='+Portafolio;")
        '    .Append("}")
        '    .Append("PopupBuscador('" + FechaOperacion + "','" + NroOrden + "','" + Portafolio + "');")
        '    .Append("</script>")
        'End With
        'EjecutarJS(script.ToString(), False)
    End Sub
    Private Function Pagina(ByVal clasificacion As String) As String
        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)
    End Function
    Private Sub Ir(ByVal ejecucion As String, ByVal orden As String, ByVal fondo As String, ByVal clasificacion As String)
        Dim StrURL As String
        StrURL = Pagina(clasificacion).Replace("OI1", ejecucion).Replace("NO1", orden).Replace("FO1", fondo).Replace("CL1", clasificacion).Replace("%", "&")
        ShowDialogPopup(StrURL)
    End Sub
    Protected Sub CrearMensajeStartupScript(ByVal mensaje As String)
        EjecutarJS("<script language=""JavaScript""> alert('" & mensaje & "')</script>", False)
    End Sub
    Private Function ExisteOperacion() As Boolean
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        'RGF 20090428 se estaba perdiendo la fecha con el postback
        Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ExistenciaOperacionCaja(lbNroTransaccion.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), lbParametros.Text.Split(",").GetValue(0), Me.DatosRequest)
        If dtblDatos.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function ContadorInicio() As Boolean
        If dgListaOE.Rows.Count = 0 Then
            AlertaJS("No existen Registros para mostrar")
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub ConsultarPaginasPorOI()
        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\Configuracion\TEjecucionOI.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim tablaClaseInstrumento As New DataTable
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim oPortafolioBM As New PortafolioBM
        tablaClaseInstrumento = oClaseInstrumentoBM.Listar(Me.DatosRequest).Tables(0)
        tablaPortafolio = oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0)
        ddlFondoOE.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondoOE.DataValueField = "CodigoPortafolio"
        ddlFondoOE.DataTextField = "Descripcion"
        ddlFondoOE.DataBind()
        ddlFondoOE.Items.Insert(0, New ListItem("--Seleccione--", ""))
    End Sub
    Private Sub CargarGrillaOIEjecutadasExtornadas(ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As Decimal)
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenesInversionBM.ListarOIEjecutadasExtornadas(DatosRequest, fondo, nroOrden, fecha)
        dgListaOX.DataSource = dtblDatos
        dgListaOX.DataBind()
        'Guarda Tabla en ViewState
        ViewState("OIConfirmadas") = dtblDatos
    End Sub
    Private Sub CargarGrillaOIEjecutadas(ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As String)
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenesInversionBM.ListarOIEjecutadas(DatosRequest, fondo, nroOrden, fecha)
        dgListaOE.DataSource = dtblDatos
        dgListaOE.DataBind()
        'Guarda Tabla en ViewState
        ViewState("OIEjecutadas") = dtblDatos
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim FechaOperacion As Decimal
                Dim NroOrden As String
                Dim Portafolio As String
                If Not Request.QueryString("vFechaOperacion") Is Nothing Then
                    Try
                        FechaOperacion = Convert.ToString(Request.QueryString("vFechaOperacion"))
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(FechaOperacion))
                    Catch ex As Exception
                        FechaOperacion = 0
                    End Try
                Else
                    FechaOperacion = 0
                End If
                If Not Request.QueryString("vNroOrden") Is Nothing Then
                    NroOrden = Request.QueryString("vNroOrden")
                    Me.txtNroOrdenOE.Text = NroOrden
                Else
                    NroOrden = ""
                End If
                If Not Request.QueryString("vPortafolio") Is Nothing Then
                    Portafolio = Request.QueryString("vPortafolio")
                    Me.ddlFondoOE.SelectedValue = Portafolio
                Else
                    Portafolio = ""
                End If
                CargarGrillaOIEjecutadas(Portafolio, NroOrden, FechaOperacion)
                CargarGrillaOIEjecutadasExtornadas(Portafolio, NroOrden, FechaOperacion)
                CargarCombos()
                ConsultarPaginasPorOI()
                ViewState("Indica") = 0
                ViewState("Fondo") = ""
                ViewState("NroOrden") = ""
                ViewState("Indica") = 0
            Else
                If Not Session("Extornado") Is Nothing Then
                    If Session("Extornado") = 1 Then
                        CargarGrillaOIEjecutadas(ViewState("Fondo"), ViewState("NroOrden"), ViewState("Fecha"))
                        CargarGrillaOIEjecutadasExtornadas(ViewState("Fondo"), ViewState("NroOrden"), ViewState("Fecha"))
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Redirect("~/frmError")
            'AlertaJS("Ocurrió un error al cargar la página")
        End Try
        
    End Sub
    Protected Sub ddlFondoOE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondoOE.SelectedIndexChanged
        If (Me.ddlFondoOE.SelectedIndex <> 0) Then
            Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(Me.ddlFondoOE.SelectedValue.ToString))
        Else
            Me.tbFechaOperacion.Text = ""
        End If
    End Sub
    Protected Sub dgListaOE_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOE.PageIndexChanging
        Try
            dgListaOE.PageIndex = e.NewPageIndex
            CargarGrillaOIEjecutadas(ViewState("Fondo"), ViewState("NroOrden"), ViewState("Fecha"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el paginado")
        End Try
    End Sub
    Protected Sub dgListaOE_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOE.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            If e.CommandName = "Seleccionar" Then
                dgListaOE.SelectedIndex = gvr.RowIndex ' e.Item.ItemIndex
                dgListaOX.SelectedIndex = -1
                Dim intA As Integer = gvr.RowIndex
                Me.lbCodigoISIN.Text = HttpUtility.HtmlDecode(dgListaOE.Rows.Item(intA).Cells(10).Text)
                Me.lbTipoOrden.Text = HttpUtility.HtmlDecode(dgListaOE.Rows.Item(intA).Cells(5).Text)
                Me.lbNroTransaccion.Text = HttpUtility.HtmlDecode(dgListaOE.Rows.Item(intA).Cells(4).Text)
                Me.lbTipoOperacion.Text = HttpUtility.HtmlDecode(dgListaOE.Rows.Item(intA).Cells(11).Text)
                Me.lbParametros.Text = HttpUtility.HtmlDecode(dgListaOE.Rows.Item(intA).Cells(2).Text) & "," & HttpUtility.HtmlDecode(dgListaOE.Rows.Item(intA).Cells(12).Text)
                Me.ibExtornar.Attributes.Add("onClick", "javascript:return Extornar();")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub dgListaOX_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOX.PageIndexChanging
        Try
            dgListaOX.PageIndex = e.NewPageIndex
            CargarGrillaOIEjecutadasExtornadas(ViewState("Fondo"), ViewState("NroOrden"), ViewState("Fecha"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el paginado")
        End Try
    End Sub
    Protected Sub dgListaOX_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOX.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim intA As Integer = gvr.RowIndex
            dgListaOX.SelectedIndex = gvr.RowIndex
            dgListaOE.SelectedIndex = -1
            Me.lbCodigoISIN.Text = HttpUtility.HtmlDecode(dgListaOX.Rows.Item(intA).Cells(9).Text)
            Me.lbTipoOrden.Text = HttpUtility.HtmlDecode(dgListaOX.Rows.Item(intA).Cells(4).Text)
            Me.lbNroTransaccion.Text = HttpUtility.HtmlDecode(dgListaOX.Rows.Item(intA).Cells(3).Text)
            Me.lbTipoOperacion.Text = HttpUtility.HtmlDecode(dgListaOX.Rows.Item(intA).Cells(10).Text)
            Me.lbParametros.Text = ""
            ibExtornar.Attributes.Remove("onClick")
        End If
    End Sub
    Protected Sub ibBuscarOE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibBuscarOE.Click
        Dim arrFecha() As String
        Try
            Dim dato As DateTime
            dato = Convert.ToDateTime(Me.tbFechaOperacion.Text.Trim())
            arrFecha = Me.tbFechaOperacion.Text.Trim().Split("/")
        Catch ex As Exception
            AlertaJS("El Formato de la fecha es incorrecto.")
            Exit Sub
        End Try
        Dim nroOrden As String = txtNroOrdenOE.Text.Trim
        Dim fondo As String = IIf(ddlFondoOE.SelectedIndex = 0, "", ddlFondoOE.SelectedValue)
        Dim fecha As String = arrFecha(2) + arrFecha(1) + arrFecha(0)
        ViewState("Fondo") = fondo
        ViewState("NroOrden") = nroOrden
        ViewState("Fecha") = fecha
        CargarGrillaOIEjecutadas(fondo, nroOrden, fecha)
        CargarGrillaOIEjecutadasExtornadas(fondo, nroOrden, fecha)
        Me.lbCodigoISIN.Text = ""
        Me.lbNroTransaccion.Text = ""
        Me.lbParametros.Text = ""
        Me.lbTipoOperacion.Text = ""
        Me.lbTipoOrden.Text = ""
        Session.Remove("Extornado")
    End Sub
    Protected Sub ibExtornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibExtornar.Click
        Dim blnExisteOperacion As Boolean
        Try
            If ContadorInicio() Then
                If lbParametros.Text.ToString <> "" Then
                    blnExisteOperacion = Me.ExisteOperacion()
                    If blnExisteOperacion Then
                        Me.CrearMensajeStartupScript(Constantes.M_STR_MENSAJE_OPERACION_EXISTE)
                    Else
                        Session("Extornado") = 1
                        Ir("XO", lbNroTransaccion.Text, lbParametros.Text.Split(",").GetValue(0), lbParametros.Text.Split(",").GetValue(1))
                    End If
                Else
                    AlertaJS("Debe seleccionar un Registro")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub
End Class
