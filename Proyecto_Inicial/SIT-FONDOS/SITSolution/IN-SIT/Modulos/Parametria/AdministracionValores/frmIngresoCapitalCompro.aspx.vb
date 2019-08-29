'Creado por: HDG Fondo0 20140801
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Parametria_AdministracionValores_frmIngresoCapitalCompro
    Inherits BasePage

    Dim dtDetalleCapitalCompro As DataTable

#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                Session("FilaSeleccionada") = -1
                Session("Identificador") = 0
                lCodigoIsin.Text = Request.QueryString("vISIN")
                lCodigoNemo.Text = Request.QueryString("vMnemonico")
                If Session("accionValor") = "INGRESAR" Then
                    If Not Session("TablaDetalleCapComp") Is Nothing Then
                        If CType(Session("TablaDetalleCapComp"), DataTable).Rows.Count > 0 Then
                            dgLista.DataSource = CType(Session("TablaDetalleCapComp"), DataTable)
                            dgLista.DataBind()
                        End If
                    Else
                        Session("TablaDetalleCapComp") = Nothing
                        GenerarTabla()
                    End If
                ElseIf Session("accionValor") = "MODIFICAR" Then
                    If Not Session("TablaDetalleCapComp") Is Nothing Then
                        If CType(Session("TablaDetalleCapComp"), DataTable).Rows.Count > 0 Then
                            dgLista.DataSource = CType(Session("TablaDetalleCapComp"), DataTable)
                            dgLista.DataBind()
                        End If
                    Else
                        Session("TablaDetalleCapComp") = Nothing
                        GenerarTabla()
                        CargarRegistro(lCodigoNemo.Text)
                    End If
                End If
                If Request.QueryString("vReadOnly") = "YES" Then
                    bloqueatodo()
                End If
                Dim dtTemporal As New DataTable
                If Not Session("TablaDetalleCapComp") Is Nothing Then
                    dtTemporal = CType(Session("TablaDetalleCapComp"), DataTable).Copy
                    Session("Temporal") = dtTemporal
                Else
                    Session("Temporal") = Nothing
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub

    Private Sub bloqueatodo()
        ddlPortafolio.Enabled = False
        tbCapitalCompro.Enabled = False
        ddlSituacion.Enabled = False
        btnAgregar.Visible = False
        btnAceptar.Visible = False
        dgLista.Columns(0).Visible = False
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim dtDetalle As DataTable
        dtDetalle = CType(Session("TablaDetalleCapComp"), DataTable)
        If (dtDetalle.Rows.Count > 0) Then
            Try
                If Session("accionValor") = "INGRESAR" Then
                    EjecutarJS("window.close();")
                ElseIf Session("accionValor") = "MODIFICAR" Then
                    EjecutarJS("window.close();")
                End If
                Session("CapitalCompro") = True
            Catch ex As Exception
                'Las excepciones deben ser enviadas a la clase base con el método AlertaJS,esta clase se encarga de mostrar los mensajes correspondientes
                AlertaJS(ex.Message.ToString())
            End Try
        Else
            Session("CapitalCompro") = False
            AlertaJS("Debe Ingresar mínimo un Custodio")
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            If CType(Session("TablaDetalleCapComp"), DataTable).Rows.Count > 0 Then
                Session("CapitalCompro") = True
                EjecutarJS("window.close()")
            Else
                Session("CapitalCompro") = False
                AlertaJS("Debe Ingresar mínimo un Capital Comprometido", "window.close()")
            End If
            Session("TablaDetalleCapComp") = Session("Temporal")
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            If CType(Session("FilaSeleccionada"), Integer) <> -1 Then
                ModificarFila()
            Else
                InsertarFila()
            End If
            LimpiarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub

    Private Sub LimpiarDetalle()
        ddlPortafolio.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
        ddlPortafolio.Enabled = True
        ddlSituacion.Enabled = True
        tbCapitalCompro.Text = ""
    End Sub

#End Region

#Region " /* Funciones Personalizadas */"
    Public Sub CargarRegistro(ByVal CodigoNemo As String)
        Dim oValoresBM As New ValoresBM
        Dim dtDetalle As DataTable
        Dim i As Integer
        dtDetalle = oValoresBM.SeleccionarDetalleCapitalCompro(CodigoNemo, DatosRequest)
        If dtDetalle.Rows.Count > 0 Then
            For i = 0 To dtDetalle.Rows.Count - 1
                dtDetalle.Rows(i)("Identificador") = i
            Next
            Session("Identificador") = dtDetalle.Rows.Count - 1
            Session("TablaDetalleCapComp") = dtDetalle
            dgLista.DataSource = dtDetalle
            dgLista.DataBind()
        End If
    End Sub

    Public Sub CargarCombos()
        Dim DtTablaSituacion As New DataTable
        Dim DtPortafolio As DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oPortafolio As New PortafolioBM
        Dim codigoPortafolio As String = ""

        'codigoPortafolio = New ParametrosGeneralesBM().SeleccionarPorFiltro(GRUPO_FONDO, MULTIFONDO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
        DtPortafolio = oPortafolio.PortafolioCodigoListar(codigoPortafolio)
        DtTablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, DtTablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlPortafolio, DtPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub

    Public Sub InsertarFila()
        Dim dtDetalle As New DataTable
        Dim drFila As DataRow
        Dim oValoresBM As New ValoresBM

        dtDetalle = CType(Session("TablaDetalleCapComp"), DataTable)
        drFila = dtDetalle.NewRow
        drFila("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
        drFila("Descripcion") = ddlPortafolio.SelectedItem.Text
        drFila("CapitalCompro") = tbCapitalCompro.Text
        If ddlSituacion.SelectedValue = "A" Then
            drFila("Situacion") = "ACTIVO"
        Else
            drFila("Situacion") = "INACTIVO"
        End If
        drFila("Identificador") = CType(Session("Identificador"), Integer)

        If VerificarFila(ddlPortafolio.SelectedValue) Then
            AlertaJS("Ya ingresó el Portafolio")
        Else
            dtDetalle.Rows.Add(drFila)
            Session("TablaDetalleCapComp") = dtDetalle
            Session("Identificador") = CType(Session("Identificador"), Integer) + 1
            dgLista.DataSource = dtDetalle
            dgLista.DataBind()
        End If

    End Sub

    Public Function VerificarFila(ByVal strCodigoSBS As String) As Boolean
        Dim BlnEsta As Boolean = False
        Dim dtDetalle As DataTable
        Dim i As Integer
        dtDetalle = CType(Session("TablaDetalleCapComp"), DataTable)
        For i = 0 To dtDetalle.Rows.Count - 1
            If CType(dtDetalle.Rows(i)("CodigoPortafolioSBS"), String) = strCodigoSBS Then
                BlnEsta = True
                Exit For
            End If
        Next
        Return BlnEsta
    End Function

    Public Function VerificarFilaModificar(ByVal strCodigoSBS As String, ByVal intNroFila As Integer) As Boolean
        Dim BlnEsta As Boolean = False
        Dim dtDetalle As DataTable
        Dim i As Integer
        dtDetalle = CType(Session("TablaDetalleCapComp"), DataTable)
        For i = 0 To dtDetalle.Rows.Count - 1
            If CType(dtDetalle.Rows(i)("CodigoPortafolioSBS"), String) = strCodigoSBS And i <> intNroFila Then
                BlnEsta = True
                Exit For
            End If
        Next
        Return BlnEsta
    End Function

    Public Sub ModificarFila()
        Dim numSeleccionado As Integer
        Dim i As Integer
        Dim dtAuxDetalle As DataTable
        dtAuxDetalle = CType(Session("TablaDetalleCapComp"), DataTable)

        numSeleccionado = CType(Session("FilaSeleccionada"), Integer)
        If (numSeleccionado <> -1) Then
            For i = 0 To dtAuxDetalle.Rows.Count - 1
                If CType(dtAuxDetalle.Rows(i)("Identificador"), Integer) = numSeleccionado Then
                    If VerificarFilaModificar(ddlPortafolio.SelectedValue, i) Then
                        AlertaJS("Ya ingresó el Portafolio")
                    Else
                        dtAuxDetalle.Rows(i)("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
                        dtAuxDetalle.Rows(i)("Descripcion") = ddlPortafolio.SelectedItem.Text
                        dtAuxDetalle.Rows(i)("CapitalCompro") = tbCapitalCompro.Text
                        If ddlSituacion.SelectedValue = "A" Then
                            dtAuxDetalle.Rows(i)("Situacion") = "ACTIVO"
                        Else
                            dtAuxDetalle.Rows(i)("Situacion") = "INACTIVO"
                        End If
                        dgLista.DataSource = dtAuxDetalle
                        dgLista.DataBind()
                        Session("TablaDetalleCapComp") = dtAuxDetalle
                        Session("FilaSeleccionada") = -1
                        btnAgregar.Text = "Agregar"
                    End If
                    'lblPortafolio.Visible = False
                    'ddlPortafolio.Visible = False
                End If
            Next
        End If
    End Sub

#End Region

    Private Sub GenerarTabla()
        dtDetalleCapitalCompro = New DataTable
        dtDetalleCapitalCompro.Columns.Add("CodigoPortafolioSBS")
        dtDetalleCapitalCompro.Columns.Add("Descripcion")
        dtDetalleCapitalCompro.Columns.Add("CapitalCompro")
        dtDetalleCapitalCompro.Columns.Add("Situacion")
        dtDetalleCapitalCompro.Columns.Add("Identificador")
        Session("TablaDetalleCapComp") = dtDetalleCapitalCompro
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            Dim oValoresBM As New ValoresBM
            Session("TablaDetalleCapComp") = oValoresBM.SeleccionarDetalleCapitalCompro(lCodigoNemo.Text, DatosRequest)
            dgLista.DataSource = CType(Session("TablaDetalleCapComp"), DataTable)
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub


    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim dtAuxDetalle As DataTable
            Dim intNroFila = -1
            Dim i As Integer
            dtAuxDetalle = CType(Session("TablaDetalleCapComp"), DataTable)
            btnAgregar.Text = "Modificar"
            Session("FilaSeleccionada") = CType(e.CommandArgument, Integer)
            intNroFila = CType(e.CommandArgument, Integer)
            For i = 0 To dtAuxDetalle.Rows.Count - 1
                If dtAuxDetalle.Rows(i)("Identificador") = intNroFila Then
                    If CType(e.CommandSource, ImageButton).CommandName = "Modificar" Then
                        'lblPortafolio.Visible = True
                        'ddlPortafolio.Visible = True
                        ddlPortafolio.SelectedValue = CType(dtAuxDetalle.Rows(i)("CodigoPortafolioSBS"), String)
                        tbCapitalCompro.Text = CType(dtAuxDetalle.Rows(i)("CapitalCompro"), String)

                        If CType(dtAuxDetalle.Rows(i)("Situacion"), String) = "ACTIVO" Then
                            ddlSituacion.SelectedValue = "A"
                        Else
                            ddlSituacion.SelectedValue = "I"
                        End If
                        ddlPortafolio.Enabled = False
                        ddlSituacion.Enabled = True
                    End If
                    Exit For
                End If
            Next
            dgLista.DataSource = dtAuxDetalle
            dgLista.DataBind()
            Session("TablaDetalleCapComp") = dtAuxDetalle
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub
End Class
