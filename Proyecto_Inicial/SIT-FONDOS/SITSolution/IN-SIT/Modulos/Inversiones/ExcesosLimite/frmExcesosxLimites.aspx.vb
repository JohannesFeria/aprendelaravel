Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Imports ParametrosSIT
Partial Class Modulos_Inversiones_ExcesosLimite_frmExcesosxLimites
    Inherits BasePage
#Region "Rutinas"
    Private Function MensajedeAprobacion() As String 'CMB OT 61566 20101207
        Dim mensaje As New StringBuilder
        Dim nroOrden As String = lblNroTransaccion.Text
        Dim fondo As String = lFondo.Text
        Dim operacion As String = lblTipoOperacion.Text
        Dim orden As String = lblTipoOrden.Text.Trim
        Dim codISIN As String = lblCodigoISIN.Text.Trim
        Dim codNem As String = lDescripcion.Text.Trim.Split("-").GetValue(0)
        Dim fecha As String = DateTime.Today.ToString("dd/MM/yyyy")
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='560' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>La siguiente orden ha sido aprobada el " & fecha & "</td></tr>") 'CMB OT 61566 20101207
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td width='35%'>Numero de Orden:</td>")
            .Append("<td colspan='2' width='65%'>" & nroOrden & "</td></tr>")
            .Append("<tr><td width='35%'>Fondo:</td><td colspan='2' width='65%'>" & fondo & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Operaci&oacute;n:</td><td colspan='2' width='65%'>" & operacion & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Orden: </td><td colspan='2' width='65%'>" & orden & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo ISIN:</td><td colspan='2' width='65%'>" & codISIN & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo Nem&oacute;nico:</td><td colspan='2' width='65%'>" & codNem & "</td></tr>")
            .Append("<tr><td colspan='3' height='8'></td></tr>")
            .Append("<tr><td colspan='3'><strong>AFP Integra</strong></td></tr>")
            .Append("<tr><td colspan='3'><strong>Grupo Integra</strong></td></tr></table>")
        End With
        Return mensaje.ToString
    End Function
    Public Sub ContadorInicial()
        If dgListaCE.Rows.Count = 0 Then
            AlertaJS("No existen Registros para mostrar")
            Exit Sub
        End If
    End Sub
    Public Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim tablaClaseInstrumento As New DataTable
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim oPortafolioBM As New PortafolioBM
        Dim codigoPortafolio As String = ""
        'codigoPortafolio = New ParametrosGeneralesBM().SeleccionarPorFiltro(GRUPO_FONDO, MULTIFONDO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
        tablaClaseInstrumento = oClaseInstrumentoBM.Listar(Me.DatosRequest).Tables(0)
        tablaPortafolio = oPortafolioBM.PortafolioCodigoListar(codigoPortafolio)
        HelpCombo.LlenarComboBox(Me.ddlFondoOA, tablaPortafolio, "CodigoPortafolio", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlFondoOE, tablaPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub
    Public Sub Seleccionar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim StrNumeroOrden, StrCadena, StrISIN, StrCodigoPortafolio, StrFondo, StrTipoOrden, StrCategoria, StrTipoOperacion, StrDescripcion As String
        StrCadena = e.CommandArgument
        StrNumeroOrden = StrCadena.Split(",").GetValue(0).ToString
        StrCodigoPortafolio = StrCadena.Split(",").GetValue(1).ToString
        StrFondo = StrCadena.Split(",").GetValue(2).ToString
        StrISIN = StrCadena.Split(",").GetValue(3).ToString
        StrTipoOrden = StrCadena.Split(",").GetValue(4).ToString
        StrTipoOperacion = StrCadena.Split(",").GetValue(5).ToString
        StrCategoria = StrCadena.Split(",").GetValue(6).ToString
        StrDescripcion = StrCadena.Split(",").GetValue(7).ToString

        Me.lblCodigoISIN.Text = StrISIN
        Me.lblNroTransaccion.Text = StrNumeroOrden
        Me.lblTipoOperacion.Text = StrTipoOperacion
        Me.lblTipoOrden.Text = StrTipoOrden
        Me.lCategoria.Text = StrCategoria
        Me.lFondo.Text = StrCodigoPortafolio
        Me.lDescripcion.Text = StrDescripcion
    End Sub
    Private Sub CargarGrillaEx()
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenesInversionBM.ListarOIExcedidas(DatosRequest)
        dgListaCE.DataSource = dtblDatos
        dgListaCE.DataBind()
        'Guarda Tabla en ViewState
        ViewState("OIExcedidas") = dtblDatos
    End Sub
    Private Sub CargarGrillaAprob()
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ListarOIAprobadas(DatosRequest)
        Me.dgListaOA.DataSource = dtblDatos
        Me.dgListaOA.DataBind()
        'Guarda Tabla en ViewState
        ViewState("OIAprobadas") = dtblDatos
    End Sub
    Private Function ShowDialogPopup(ByVal StrURL As String) As Boolean

        EjecutarJS("showModalDialog('" & StrURL & "', '800', '600', '');")
        'Dim script As New StringBuilder
        'With script
        '    .Append("<script>")
        '    .Append("function PopupBuscador()")
        '    .Append("{")
        '    .Append("   var argument = new Object();")
        '    .Append("   window.showModalDialog('" + StrURL + "', argument, 'dialogHeight:600px; dialogWidth:800px; dialogLeft:150px;');")
        '    .Append("   return false;")
        '    .Append("}")
        '    .Append("PopupBuscador();")
        '    .Append("</script>")
        'End With
        'EjecutarJS(script.ToString(), False)
    End Function
    Private Function Pagina(ByVal clasificacion As String) As String
        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)
    End Function
    Private Function ConsultarPaginasPorOI() As Boolean

        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\..\Configuracion\TOExcedida.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrillaEx()
                CargarGrillaAprob()
                ConsultarPaginasPorOI()
                ContadorInicial()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub dgListaOA_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgListaOA.PageIndexChanging
        dgListaOA.SelectedIndex = -1
        dgListaOA.PageIndex = e.NewPageIndex
        CargarGrillaAprob()
    End Sub

    Protected Sub dgListaOA_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgListaOA.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            dgListaOA.SelectedIndex = gvr.RowIndex
            dgListaCE.SelectedIndex = -1
            Me.ibAprobar.Attributes.Remove("onClick")
        End If
    End Sub

    Protected Sub ibBuscarOE_Click(sender As Object, e As EventArgs) Handles ibBuscarOE.Click
        CargarGrillaEx()

        Dim dtblDatos As DataTable = CType(ViewState("OIExcedidas"), DataTable)
        Dim nroOrden As String = txtNroOrdenOE.Text.Trim
        Dim fondo As String = ddlFondoOE.SelectedValue
        Dim dvwVista As DataView

        dvwVista = dtblDatos.DefaultView

        'No selecciona ninguna opcion
        If (nroOrden = String.Empty Or nroOrden = "") And (ddlFondoOE.SelectedIndex = 0) Then
            dgListaCE.DataSource = dvwVista
            dgListaCE.DataBind()
        Else
            'Selecciona Nro ORden
            If (nroOrden <> String.Empty Or nroOrden = "") And (ddlFondoOE.SelectedIndex = 0) Then
                dvwVista.RowFilter = "NumeroTransaccion = '" + nroOrden + "'"
                dgListaCE.DataSource = dvwVista
                dgListaCE.DataBind()
            Else
                'Selecciona Fondo
                If (nroOrden = String.Empty Or nroOrden = "") And (ddlFondoOE.SelectedIndex <> 0) Then
                    dvwVista.RowFilter = "Fondo = '" + fondo + "'"
                    dgListaCE.DataSource = dvwVista
                    dgListaCE.DataBind()
                Else
                    'Selecciona Fondo y NroOrden
                    dvwVista.RowFilter = "Fondo = '" + fondo + "' and NumeroTransaccion='" + nroOrden + "'"
                    dgListaCE.DataSource = dvwVista
                    dgListaCE.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub ibBuscarOA_Click(sender As Object, e As EventArgs) Handles ibBuscarOA.Click
        Dim dtblDatos As DataTable = CType(ViewState("OIAprobadas"), DataTable)
        Dim nroOrden As String = txtNroOrdenOC.Text.Trim
        Dim fondo As String = ddlFondoOA.SelectedValue
        Dim dvwVista As DataView

        dvwVista = dtblDatos.DefaultView

        'No selecciona ninguna opcion
        If (nroOrden = String.Empty Or nroOrden = "") And (ddlFondoOA.SelectedIndex = 0) Then
            dgListaOA.DataSource = dvwVista
            dgListaOA.DataBind()
        Else
            'Selecciona Nro ORden
            If (nroOrden <> String.Empty Or nroOrden = "") And (ddlFondoOA.SelectedIndex = 0) Then
                dvwVista.RowFilter = "NumeroTransaccion = '" + nroOrden + "'"
                dgListaOA.DataSource = dvwVista
                dgListaOA.DataBind()
            Else
                'Selecciona Fondo
                If (nroOrden = String.Empty Or nroOrden = "") And (ddlFondoOA.SelectedIndex <> 0) Then
                    dvwVista.RowFilter = "Fondo = '" + fondo + "'"
                    dgListaOA.DataSource = dvwVista
                    dgListaOA.DataBind()
                Else
                    'Selecciona Fondo y NroOrden
                    dvwVista.RowFilter = "Fondo = '" + fondo + "' and NumeroTransaccion='" + nroOrden + "'"
                    dgListaOA.DataSource = dvwVista
                    dgListaOA.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub dgListaCE_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgListaCE.PageIndexChanging
        Try
            dgListaCE.SelectedIndex = -1
            dgListaCE.PageIndex = e.NewPageIndex
            CargarGrillaEx()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la paginación")
        End Try
    End Sub
    Protected Sub dgListaCE_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgListaCE.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                dgListaCE.SelectedIndex = gvr.RowIndex
                dgListaOA.SelectedIndex = -1
                Me.ibAprobar.Attributes.Add("onClick", "javascript:return confirm('¿Desea la aprobar del Nro. de Orden?');")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub

    Protected Sub ibAprobar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibAprobar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM

        ContadorInicial()
        If lCategoria.Text.ToString <> "" Then
            'Dim StrUrl As String
            'StrUrl = "AutorizarExceso.aspx?cOrden=" & Me.lblNroTransaccion.Text & "&Fondo=" & Me.lFondo.Text
            'ShowDialogPopup(StrUrl)
            Try
                'INI CMB OT 61566 20101201
                'oOrdenInversionWorkFlowBM.AprobarOI(Me.lblNroTransaccion.Text, Me.lFondo.Text, Me.DatosRequest)
                If oOrdenInversionWorkFlowBM.AprobarOI(Me.lblNroTransaccion.Text, Me.lFondo.Text, Me.DatosRequest) Then

                    Dim toUser As String = ""
                    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                    Dim dt As DataTable
                    dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEII, "", "", "", DatosRequest)
                    For Each fila As DataRow In dt.Rows
                        toUser = toUser + fila("Valor").ToString() + ";"
                    Next
                    'INI CMB OT 62254 20110324
                    Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
                    Dim strNroTran As String = Me.lblNroTransaccion.Text.ToString
                    Dim strUsuarioOI As String = ""
                    strUsuarioOI = oOrdenPreOrdenInversionBM.ObtenerUsuario(strNroTran, DatosRequest)
                    strUsuarioOI = New PersonalBM().SeleccionarMail(strUsuarioOI)
                    If strUsuarioOI <> "" Then
                        toUser = toUser + strUsuarioOI + ";"
                    End If
                    'FIN CMB OT 62254 20110324
                    UIUtility.EnviarMail(toUser, "", "Aprobación de Orden excedido por Limites de Inversión", MensajedeAprobacion(), Me.DatosRequest) 'CMB OT 61566 20101207 'CMB 20110518
                End If
                'FIN CMB OT 61566 20101201

                CargarGrillaEx()
                CargarGrillaAprob() 'HDG OT 61166 20100920

            Catch ex As Exception
                AlertaJS(ex.Message)
            End Try
        Else
            AlertaJS("Debe seleccionar un Registro")
        End If
    End Sub

    Protected Sub ibSalir_Click(sender As Object, e As EventArgs) Handles ibSalir.Click
        Response.Redirect("~/frmDefault.aspx", False)
    End Sub
End Class
