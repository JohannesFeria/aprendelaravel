Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_EntidadExcesos
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim arraySesiones As String() = New String(2) {}
                arraySesiones = DirectCast(Session("SS_DatosModal"), String())
                Me.hdCodigoEntidad.Value = arraySesiones(0)
                Me.hdDescripcion.Value = arraySesiones(1)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Protected Sub ibAceptar_Click(sender As Object, e As System.EventArgs) Handles ibAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Protected Sub ibCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibCancelar.Click
        Try
            Response.Redirect("frmBusquedaEntidadExcesos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

    Private Sub lkbBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lkbBuscar.Click
        Try
            If Me.hdEstadoBusqueda.Value = 0 Then
                ShowDialogPopupValores()
                hdEstadoBusqueda.Value = 1
            ElseIf hdEstadoBusqueda.Value = 1 Then
                txtCodigoEntidad.Text = Me.hdCodigoEntidad.Value
                tbDescripcion.Text = Me.hdDescripcion.Value
                Me.hdEstadoBusqueda.Value = 0
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al mostrar Popup")
        End Try        
    End Sub

#End Region

#Region "*/ Funciones Personalizadas */"

    Private Sub Aceptar()

        Dim blnExisteEntidad As Boolean
        'If Me.hdCodigoEntidad.Value.Equals(String.Empty) Then
        If Request.QueryString("codigo") Is Nothing Then
            blnExisteEntidad = Me.ExisteExcesoBroker()
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                Insertar()
            End If
        Else
            Modificar()
        End If

    End Sub

    Private Sub Insertar()

        Dim oEntidadExcesosBE As EntidadExcesosBE
        Dim oEntidadBM As New EntidadBM

        oEntidadExcesosBE = ObtenerInstancia()

        oEntidadBM.InsertarExcesosBroker(oEntidadExcesosBE, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)

        LimpiarCampos()

    End Sub

    Private Sub Modificar()

        Dim oEntidadExcesosBE As EntidadExcesosBE
        Dim oEntidadBM As New EntidadBM

        oEntidadExcesosBE = ObtenerInstancia()

        oEntidadBM.ModificarExcesosBroker(oEntidadExcesosBE, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)

    End Sub

    Private Sub CargarPagina()

        'Me.ibAceptar.Attributes.Add("onclick", "javascript:return Validar();")

        If Not Page.IsPostBack Then
            Me.hdEstadoBusqueda.Value = 0
            CargarCombos()

            If Not Request.QueryString("codigo") Is Nothing Then
                hdCodigoEntidad.Value = Request.QueryString("codigo")
                cargarRegistro(hdCodigoEntidad.Value)
                txtCodigoEntidad.Enabled = False
            Else
                hdCodigoEntidad.Value = String.Empty
                lkbBuscar.Visible = True
            End If
        End If

    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)

        Dim oEntidadBM As New EntidadBM

        Dim dt As DataTable = oEntidadBM.SeleccionarExcesosBroker(codigo, "", "")
        txtCodigoEntidad.Enabled = False

        Me.hdCodigoEntidad.Value = dt.Rows(0)("CodigoEntidad")
        Me.txtCodigoEntidad.Text = dt.Rows(0)("CodigoEntidad")
        Me.tbDescripcion.Text = dt.Rows(0)("descripcion")
        Me.tbMontoExceso.Text = Format(dt.Rows(0)("montorestriccion"), "##,##0.0000000")
        Try
            Me.ddlSituacion.SelectedValue = dt.Rows(0)("Situacion")
        Catch ex As Exception
            Me.ddlSituacion.SelectedIndex = 0
        End Try
    End Sub

    Private Function ObtenerInstancia() As EntidadExcesosBE

        Dim oEntidadExcesosBE As New EntidadExcesosBE
        Dim oRow As EntidadExcesosBE.EntidadExcesosRow

        oRow = DirectCast(oEntidadExcesosBE.EntidadExcesos.NewEntidadExcesosRow(), EntidadExcesosBE.EntidadExcesosRow)

        oRow.CodigoEntidad = txtCodigoEntidad.Text.ToUpper.Trim.ToString
        oRow.MontoRestriccion = Me.tbMontoExceso.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue

       oEntidadExcesosBE.EntidadExcesos.AddEntidadExcesosRow(oRow)
        oEntidadExcesosBE.EntidadExcesos.AcceptChanges()

        Return oEntidadExcesosBE

    End Function

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

    End Sub

    Private Sub LimpiarCampos()

        Me.txtCodigoEntidad.Text = String.Empty
        Me.tbDescripcion.Text = String.Empty
        Me.tbMontoExceso.Text = String.Empty
        Me.ddlSituacion.SelectedIndex = 0

    End Sub

    Private Function ExisteExcesoBroker() As Boolean

        Dim oEntidadBM As New EntidadBM

        Dim dt As DataTable = oEntidadBM.SeleccionarExcesosBroker(Me.txtCodigoEntidad.Text, "", "")

        Return dt.Rows.Count > 0

    End Function

    Private Sub ShowDialogPopupValores()

        Dim strurl As String = "../../frmHelpControlParametria.aspx?tlbBusqueda=Broker"
        EjecutarJS("showModalDialog('" & strurl & "', '800', '600', '" & lkbBuscar.ClientID & "');")

    End Sub

#End Region

End Class
