Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text

Partial Class Modulos_Inversiones_ExcesosLimite_frmExcesosxBroker
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrillaEx()
                CargarGrillaAprob()
                ConsultarPaginasPorOI()
                ContadorInicial()
            End If
            btnEliminar.Attributes.Add("onClick", "javascript:return Confirmar();")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Public Sub ContadorInicial()
        If dgListaCE.Rows.Count = 0 Then
            AlertaJS("No existen Registros para mostrar")
            Exit Sub
        End If
    End Sub
    Public Sub CargarCombos()
        Dim dt As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        dt = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondoOA, dt, "CodigoPortafolio", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlFondoOE, dt, "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub CargarGrillaEx()
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenesInversionBM.ListarOIExcedidasBroker(DatosRequest)
        dgListaCE.DataSource = dtblDatos
        dgListaCE.DataBind()
        ViewState("OIExcedidas") = dtblDatos
    End Sub

    Private Sub CargarGrillaAprob()
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ListarOIAprobadasExcesoBroker(DatosRequest)
        Me.dgListaOA.DataSource = dtblDatos
        Me.dgListaOA.DataBind()

        ViewState("OIAprobadas") = dtblDatos
    End Sub
    Public Sub Seleccionar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim StrNumeroOrden, StrCadena, StrISIN, StrFondo, StrTipoOrden, StrCategoria, StrTipoOperacion, StrEstado As String

        StrCadena = e.CommandArgument
        StrNumeroOrden = StrCadena.Split(",").GetValue(0).ToString
        StrFondo = StrCadena.Split(",").GetValue(1).ToString
        StrISIN = StrCadena.Split(",").GetValue(2).ToString
        StrTipoOrden = StrCadena.Split(",").GetValue(3).ToString
        StrTipoOperacion = StrCadena.Split(",").GetValue(4).ToString
        StrCategoria = StrCadena.Split(",").GetValue(5).ToString
        StrEstado = StrCadena.Split(",").GetValue(6).ToString

        lblCodigoISIN.Text = StrISIN
        lblNroTransaccion.Text = StrNumeroOrden
        lblTipoOperacion.Text = StrTipoOperacion
        lblTipoOrden.Text = StrTipoOrden
        lCategoria.Text = StrCategoria
        lFondo.Text = StrFondo
        txtEstado.Value = StrEstado
    End Sub

    Private Sub LimpiarControles()
        lblCodigoISIN.Text = String.Empty
        lblNroTransaccion.Text = String.Empty
        lblTipoOperacion.Text = String.Empty
        lblTipoOrden.Text = String.Empty
        lCategoria.Text = String.Empty
        lFondo.Text = String.Empty
        txtEstado.Value = String.Empty
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub Ir(ByVal clasificacion As String, ByVal StrNOrden As String, ByVal StrFondo As String)
        Dim StrURL As String
        StrURL = Pagina(clasificacion).Replace("#", StrFondo).Replace("%", "&").Replace("@", StrNOrden)
        ShowDialogPopup("..\" & StrURL)
    End Sub
    Private Function Pagina(ByVal clasificacion As String) As String
        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)
    End Function
    Private Sub ConsultarPaginasPorOI()
        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\..\Configuracion\TOExcedida.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnAprobar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        ContadorInicial()
        If lCategoria.Text.ToString <> "" Then
            Try
                oOrdenInversionWorkFlowBM.AprobarExcesoBrokerOI(Me.lblNroTransaccion.Text, Me.lFondo.Text, Me.txtEstado.Value, Me.DatosRequest)
                CargarGrillaEx()
                CargarGrillaAprob()
                dgListaCE.SelectedIndex = -1
                LimpiarControles()
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Else
            AlertaJS("Debe seleccionar un Registro")
        End If
    End Sub

    Private Sub ShowDialogPopup(ByVal StrURL As String)
        EjecutarJS("showModalDialog('" & StrURL & "', '800', '600', '');")
        'EjecutarJS("window.showModalDialog('" + StrURL + "','','dialogHeight:550px;dialogWidth:789px;status:no;unadorned:yes;help:No');")
    End Sub

    Private Sub btnConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        If lCategoria.Text.ToString <> "" Then
            Ir(lCategoria.Text, Me.lblNroTransaccion.Text, Me.lFondo.Text)
        Else
            AlertaJS("Debe seleccionar un Registro")
        End If
    End Sub

    Private Sub btnBuscarOE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarOE.Click
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

    Private Sub btnBuscarOA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarOA.Click
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

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
        ContadorInicial()
        If lCategoria.Text.ToString <> "" Then
            Try
                oOrdenInversionBM.EliminarOI(Me.lblNroTransaccion.Text, Me.lFondo.Text, "16", DatosRequest)
                CargarGrillaEx()
                CargarGrillaAprob()
                AlertaJS("El Registro ha sido eliminado satisfactoriamente")
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Else
            AlertaJS("Debe seleccionar un Registro")
        End If
    End Sub

    Protected Sub dgListaCE_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaCE.PageIndexChanging
        dgListaCE.SelectedIndex = -1
        dgListaCE.PageIndex = e.NewPageIndex
        CargarGrillaEx()
    End Sub

    Protected Sub dgListaCE_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaCE.RowCommand
        If e.CommandName = "Seleccionar" Then
            dgListaCE.SelectedIndex = CInt(e.CommandArgument.ToString().Split(",")(7))
            dgListaOA.SelectedIndex = -1
            btnAprobar.Attributes.Add("onClick", "javascript:return confirm('¿Desea la aprobar del Nro. de Orden?');")
        End If
    End Sub

    Protected Sub dgListaOA_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOA.PageIndexChanging
        dgListaOA.SelectedIndex = -1
        dgListaOA.PageIndex = e.NewPageIndex
        CargarGrillaAprob()
    End Sub

    Protected Sub dgListaOA_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOA.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim cadena As String() = e.CommandArgument.ToString.Split(",")
            Dim index As Integer = cadena(4)
            dgListaOA.SelectedIndex = index
            dgListaCE.SelectedIndex = -1
            lblNroTransaccion.Text = cadena(0)
            lblTipoOperacion.Text = cadena(1)
            lblCodigoISIN.Text = cadena(2)
            lblTipoOrden.Text = cadena(3)
            lCategoria.Text = ""
            lFondo.Text = ""
            btnAprobar.Attributes.Remove("onClick")
        End If
    End Sub
End Class
