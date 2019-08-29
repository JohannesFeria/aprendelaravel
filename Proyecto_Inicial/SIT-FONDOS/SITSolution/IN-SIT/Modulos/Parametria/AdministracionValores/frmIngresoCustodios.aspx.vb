Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Parametria_AdministracionValores_frmIngresoCustodios
    Inherits BasePage
    Dim dtDetalleCustodios As DataTable
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
                    If Not Session("TablaDetalle") Is Nothing Then
                        If CType(Session("TablaDetalle"), DataTable).Rows.Count > 0 Then
                            dgLista.DataSource = CType(Session("TablaDetalle"), DataTable)
                            dgLista.DataBind()
                        End If
                    Else
                        Session("TablaDetalle") = Nothing
                        GenerarTabla()
                    End If
                ElseIf Session("accionValor") = "MODIFICAR" Then
                    If Not Session("TablaDetalle") Is Nothing Then
                        If CType(Session("TablaDetalle"), DataTable).Rows.Count > 0 Then
                            dgLista.DataSource = CType(Session("TablaDetalle"), DataTable)
                            dgLista.DataBind()
                        End If
                    Else
                        Session("TablaDetalle") = Nothing
                        GenerarTabla()
                        CargarRegistro(lCodigoNemo.Text)
                    End If
                End If
                If Request.QueryString("vReadOnly") = "YES" Then
                    bloqueatodo()
                End If
                Dim dtTemporal As New DataTable
                If Not Session("TablaDetalle") Is Nothing Then
                    dtTemporal = CType(Session("TablaDetalle"), DataTable).Copy
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
        ddlSituacion.Enabled = False
        btnAgregar.Visible = False
        btnAceptar.Visible = False
        dgLista.Columns(0).Visible = False
        btnEliminar.Visible = False
        ddlPortafolio.Enabled = False
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim dtDetalle As DataTable
        dtDetalle = CType(Session("TablaDetalle"), DataTable)
        If (dtDetalle.Rows.Count > 0) Then
            EjecutarJS("window.close();")
            Session("Custodios") = True
        Else
            Session("Custodios") = False
            AlertaJS("Debe Ingresar mínimo un Custodio")
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            If Session("TablaDetalle") IsNot Nothing Then
                If CType(Session("TablaDetalle"), DataTable).Rows.Count > 0 Then
                    Session("Custodios") = True
                    EjecutarJS("window.close()")
                Else
                    Session("Custodios") = False
                    AlertaJS("Debe Ingresar mínimo un Custodio", "window.close()")
                End If
                Session("TablaDetalle") = Session("Temporal")
            Else
                EjecutarJS("window.close()")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            If ddlPortafolio.SelectedValue = "" Then
                AlertaJS("Seleccione un Portafolio para continuar.")
            ElseIf ddlCustodio.SelectedValue = "" Then
                AlertaJS("Seleccione un Custodio para continuar.")
            Else
                If CType(Session("FilaSeleccionada"), Integer) <> -1 Then
                    ModificarFila()
                Else
                    InsertarFila()
                End If
            End If
            LimpiarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub
    Private Sub LimpiarDetalle()
        ddlCustodio.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
        ddlCustodio.Enabled = True
        ddlSituacion.Enabled = True
    End Sub
#End Region
#Region " /* Funciones Personalizadas */"
    Public Sub CargarRegistro(ByVal CodigoNemo As String)
        Dim oValoresBM As New ValoresBM
        Dim dtDetalle As DataTable
        Dim i As Integer
        dtDetalle = oValoresBM.SeleccionarDetalleCustodios(CodigoNemo, DatosRequest)
        If dtDetalle.Rows.Count > 0 Then
            For i = 0 To dtDetalle.Rows.Count - 1
                dtDetalle.Rows(i)("Identificador") = i
            Next
            Session("Identificador") = dtDetalle.Rows.Count - 1
            Session("TablaDetalle") = dtDetalle
            dgLista.DataSource = dtDetalle
            dgLista.DataBind()
        End If
    End Sub
    Public Sub CargarCombos()
        Dim DtTablaCustodios As New DataTable
        Dim DtTablaCuentaDepositaria As New DataTable
        Dim DtTablaSituacion As New DataTable
        Dim DtPortafolio As DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oCustodioBM As New CustodioBM
        Dim oPortafolio As New PortafolioBM
        DtPortafolio = oPortafolio.PortafolioCodigoListar(ParametrosSIT.PORTAFOLIO_MULTIFONDOS)
        DtTablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        DtTablaCustodios = oCustodioBM.Listar(DatosRequest).Tables(0)
        DtTablaCuentaDepositaria = oCustodioBM.ListarCuentasDepositariasPorCustodio(DatosRequest).Tables(0)
        Session("CuentaDepositaria") = DtTablaCuentaDepositaria
        HelpCombo.LlenarComboBox(ddlSituacion, DtTablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlCustodio, DtTablaCustodios, "CodigoCustodio", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlPortafolio, DtPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub
    Public Sub InsertarFila()
        Dim dtDetalle As New DataTable,drFila As DataRow,oValoresBM As New ValoresBM,dtAuxPortafolio As DataTable
        Dim i As Integer
        dtAuxPortafolio = oValoresBM.Custodio_ListarCuentasDepositariasPorCustodio(ddlCustodio.SelectedValue, DatosRequest).Tables(0)
        If dtAuxPortafolio Is Nothing Then
            AlertaJS(ObtenerMensaje("CONF34"))
            Exit Sub
        End If
        If dtAuxPortafolio.Rows.Count = 0 Then
            AlertaJS(ObtenerMensaje("CONF34"))
            Exit Sub
        End If
        dtDetalle = CType(Session("TablaDetalle"), DataTable)
        drFila = dtDetalle.NewRow
        drFila("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
        drFila("DescripcionSBS") = ddlPortafolio.SelectedItem.Text
        drFila("CodigoCustodio") = ddlCustodio.SelectedValue
        drFila("Descripcion") = ddlCustodio.Items(ddlCustodio.SelectedIndex).Text
        If ddlSituacion.SelectedValue = "A" Then
            drFila("Situacion") = "ACTIVO"
        Else
            drFila("Situacion") = "INACTIVO"
        End If
        drFila("CodigoCuentaDepositaria") = ""
        drFila("CuentaDepositaria") = ""
        drFila("Identificador") = CType(Session("Identificador"), Integer)

        If VerificarFila(ddlPortafolio.SelectedValue, ddlCustodio.SelectedValue, "") Then
            AlertaJS("Ya ingresó el portafolio para este Custodio")
        Else
            dtDetalle.Rows.Add(drFila)
            Session("TablaDetalle") = dtDetalle
            Session("Identificador") = CType(Session("Identificador"), Integer) + 1
            dgLista.DataSource = dtDetalle
            dgLista.DataBind()
        End If
    End Sub
    Public Function VerificarFila(ByVal strCodigoSBS As String, ByVal StrCodigoCustodio As String, ByVal strCodigoCuentaDepositaria As String) As Boolean
        Dim BlnEsta As Boolean = False
        Dim dtDetalle As DataTable
        Dim i As Integer
        dtDetalle = CType(Session("TablaDetalle"), DataTable)
        For i = 0 To dtDetalle.Rows.Count - 1
            If CType(dtDetalle.Rows(i)("CodigoPortafolioSBS"), String) = strCodigoSBS And CType(dtDetalle.Rows(i)("CodigoCustodio"), String) = StrCodigoCustodio Then
                BlnEsta = True
                Exit For
            End If
        Next
        Return BlnEsta
    End Function
    Public Function VerificarFilaModificar(ByVal strCodigoSBS As String, ByVal StrCodigoCustodio As String, ByVal strCodigoCuentaDepositaria As String, ByVal intNroFila As Integer) As Boolean
        Dim BlnEsta As Boolean = False
        Dim dtDetalle As DataTable
        Dim i As Integer
        dtDetalle = CType(Session("TablaDetalle"), DataTable)
        For i = 0 To dtDetalle.Rows.Count - 1
            If CType(dtDetalle.Rows(i)("CodigoPortafolioSBS"), String) = strCodigoSBS And CType(dtDetalle.Rows(i)("CodigoCustodio"), String) = StrCodigoCustodio And i <> intNroFila Then
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
        dtAuxDetalle = CType(Session("TablaDetalle"), DataTable)
        numSeleccionado = CType(Session("FilaSeleccionada"), Integer)
        If (numSeleccionado <> -1) Then
            For i = 0 To dtAuxDetalle.Rows.Count - 1
                If CType(dtAuxDetalle.Rows(i)("Identificador"), Integer) = numSeleccionado Then
                    If VerificarFilaModificar(ddlPortafolio.SelectedValue, ddlCustodio.SelectedValue, "", i) Then
                        AlertaJS("Ya ingresó el Custodio")
                    Else
                        dtAuxDetalle.Rows(i)("DescripcionSBS") = ""
                        dtAuxDetalle.Rows(i)("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
                        dtAuxDetalle.Rows(i)("CodigoCustodio") = ddlCustodio.SelectedValue
                        dtAuxDetalle.Rows(i)("Descripcion") = ddlCustodio.SelectedItem.Text
                        dtAuxDetalle.Rows(i)("DescripcionSBS") = ddlPortafolio.SelectedItem.Text
                        dtAuxDetalle.Rows(i)("CodigoCuentaDepositaria") = ""
                        dtAuxDetalle.Rows(i)("CuentaDepositaria") = ""
                        dtAuxDetalle.Rows(i)("Identificador") = dtAuxDetalle.Rows.Count + 1
                        If ddlSituacion.SelectedValue = "A" Then
                            dtAuxDetalle.Rows(i)("Situacion") = "ACTIVO"
                        Else
                            dtAuxDetalle.Rows(i)("Situacion") = "INACTIVO"
                        End If
                        dgLista.DataSource = dtAuxDetalle
                        dgLista.DataBind()
                        Session("TablaDetalle") = dtAuxDetalle
                        Session("FilaSeleccionada") = -1
                        btnAgregar.Text = "Agregar"
                        ddlPortafolio.Enabled = True
                    End If
                End If
            Next
        End If
    End Sub
#End Region
    Private Sub ddlCustodio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCustodio.SelectedIndexChanged
        Try
            CargarCuentaDepositaria()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub
    Private Sub CargarCuentaDepositaria()
        Dim dtCuentaDepositaria As DataTable
        Dim dtAux As New DataTable
        Dim i As Integer
        dtCuentaDepositaria = Session("CuentaDepositaria")
        dtAux = dtCuentaDepositaria.Clone
        For i = 0 To dtCuentaDepositaria.Rows.Count - 1
            If CType(dtCuentaDepositaria.Rows(i)("CodigoCustodio"), String) = ddlCustodio.SelectedValue Then
                dtAux.ImportRow(dtCuentaDepositaria.Rows(i))
            End If
        Next
    End Sub
    Private Sub GenerarTabla()
        dtDetalleCustodios = New DataTable
        dtDetalleCustodios.Columns.Add("CodigoCustodio")
        dtDetalleCustodios.Columns.Add("Descripcion")
        dtDetalleCustodios.Columns.Add("CodigoCuentaDepositaria")
        dtDetalleCustodios.Columns.Add("CuentaDepositaria")
        dtDetalleCustodios.Columns.Add("CodigoPortafolioSBS")
        dtDetalleCustodios.Columns.Add("DescripcionSBS")
        dtDetalleCustodios.Columns.Add("Situacion")
        dtDetalleCustodios.Columns.Add("Identificador")
        Session("TablaDetalle") = dtDetalleCustodios
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            Dim oValoresBM As New ValoresBM
            Session("TablaDetalle") = oValoresBM.SeleccionarDetalleCustodios(lCodigoNemo.Text, DatosRequest)
            For i = 0 To CType(Session("TablaDetalle"), DataTable).Rows.Count - 1
                CType(Session("TablaDetalle"), DataTable).Rows(i)("Identificador") = i
            Next
            Session("FilaSeleccionada") = -1
            dgLista.DataSource = CType(Session("TablaDetalle"), DataTable)
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
            dtAuxDetalle = CType(Session("TablaDetalle"), DataTable)
            btnAgregar.Text = "Grabar"
            Session("FilaSeleccionada") = CType(e.CommandArgument, Integer)
            intNroFila = CType(e.CommandArgument, Integer)
            For i = 0 To dtAuxDetalle.Rows.Count - 1
                If dtAuxDetalle.Rows(i)("Identificador") = intNroFila Then
                    If CType(e.CommandSource, ImageButton).CommandName = "Modificar" Then
                        lblPortafolio.Visible = True
                        ddlPortafolio.Visible = True
                        ddlPortafolio.SelectedValue = CType(dtAuxDetalle.Rows(i)("CodigoPortafolioSBS"), String)
                        ddlCustodio.SelectedValue = CType(dtAuxDetalle.Rows(i)("CodigoCustodio"), String)
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
            Session("TablaDetalle") = dtAuxDetalle
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub
End Class