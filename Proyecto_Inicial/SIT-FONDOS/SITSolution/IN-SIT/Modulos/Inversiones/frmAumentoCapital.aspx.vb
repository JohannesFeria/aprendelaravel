Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Partial Class Modulos_Inversiones_frmAumentoCapital
    Inherits BasePage
    Dim oAumentoCapitalBM As New AumentoCapitalBM
    Dim oRowAC As New AumentoCapitalBE
    Dim accion As String

#Region "Eventos de Página"
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                LimpiarSesiones()
                CargarPaginaInicio()
                UIUtility.CargarPortafoliosbyAumentoCapital(ddlPortafolio)
                ddlPortafolio.SelectedValue = String.Empty
            End If
        Catch ex As Exception
            AlertaJS("Error al cargar el formulario / " + ex.Message.ToString)
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            HabilitaDeshabilitaBotonesInicio(True)
            CargarDatosAumentoCapital()
            'If txtEstado.Text = "PENDIENTE" Then
            '    HabilitaDeshabilitaDatosOperacion(True)
            '    ddlPortafolio.Enabled = False
            '    If ViewState("EstadoPantalla") = "Eliminar" Then
            '        HabilitaDeshabilitaDatosOperacion(False)
            '        HabilitaDeshabilitaBotonesInicio(True)

            '    End If
            'Else
            HabilitaDeshabilitaDatosOperacion(False)

            'ddlPortafolio.Enabled = True
            'End If
            lblFondo.Text = ddlPortafolio.SelectedItem.ToString
            lblFecha.Text = txtFechaPago.Text
        Catch ex As Exception
            AlertaJS("Error al buscar el Aumento de Capital / " + ex.Message.ToString)
        End Try
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            ViewState("EstadoPantalla") = "Ingresar"
            lblAccion.Text = ViewState("EstadoPantalla").ToString
            CargarPaginaAccion()
            CargarPaginaIngresar()
        Catch ex As Exception
            AlertaJS("Error al ingresar el Aumento de Capital / " + ex.Message.ToString)
        End Try
    End Sub
    'Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificar.Click
    '    Try
    '        ViewState("EstadoPantalla") = "Modificar"
    '        lblAccion.Text = ViewState("EstadoPantalla").ToString
    '        CargarPaginaAccion()
    '    Catch ex As Exception
    '        AlertaJS("Error al modificar el Aumento de Capital / " + ex.Message.ToString)
    '    End Try
    'End Sub
    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            ViewState("EstadoPantalla") = "Eliminar"
            lblAccion.Text = ViewState("EstadoPantalla").ToString
            CargarPaginaAccion()
        Catch ex As Exception
            AlertaJS("Error al eliminar el Aumento de Capital / " + ex.Message.ToString)
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            Dim result As Integer
            If ValidarCamposbyGrabar() Then
                If ViewState("EstadoPantalla") = "Ingresar" Then
                    result = InsertarAumentoCapital()
                ElseIf ViewState("EstadoPantalla") = "Modificar" Then
                    result = ModificarAumentoCapital()
                ElseIf ViewState("EstadoPantalla") = "Eliminar" Then
                    result = EliminarAumentoCapital()
                End If
                If result <> 0 Then
                    AlertaJS("Hubo un Error en el procesamiento del Aumento de Capital / Code Error: " + result.ToString)
                Else
                    AlertaJS("Se " + accion + " correctamente el Aumento de Capital.")
                    CargarPaginaInicio()
                End If
            End If

        Catch ex As Exception
            AlertaJS("Error al guardar el Aumento de Capital / " + ex.Message.ToString)
        End Try

    End Sub
    Protected Sub btnCalcular_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalcular.Click
        Try
            HabilitaDeshabilitaBotonesInicio(False)
            If ValidarCamposbyGrabar() Then
                cargarDistribucion("BO", ddlPortafolio.SelectedValue, ConvertirFechaaDecimal(txtFechaPago.Text))
                lblFondo.Text = ddlPortafolio.SelectedItem.ToString
                lblFecha.Text = txtFechaPago.Text
                txtFechaRegistro.Text = DateTime.Now.ToString("dd/MM/yyyy")
                HabilitaDeshabilitaBotonesInicio(True)
            End If
        Catch ex As Exception
            AlertaJS("Error al calcular / " + ex.Message.ToString)
        End Try
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text))
            If cantidadreg > 0 Then
                AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                ddlPortafolio.SelectedIndex = 0
                Exit Sub
            End If
            If ViewState("EstadoPantalla") = "Ingresar" Then
                cantidadreg = New AumentoCapitalBM().AumentoCapital_ExistePendientebyFechaPortafolio(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe un Aumento de Capital con fecha mayor o igual a: " + txtFechaPago.Text)
                    ddlPortafolio.SelectedIndex = 0
                    Exit Sub
                End If
            End If
            Dim dt As DataTable = New PortafolioBM().PortafolioSelectById(ddlPortafolio.SelectedValue)
            Dim fecha As Decimal = 0

            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    fecha = CDec(dt.Rows(0)("FechaNegocio").ToString())
                    ViewState("FechaNegocio") = UIUtility.ConvertirFechaaString(fecha)
                Else
                    ViewState("FechaNegocio") = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Error al seleccionar el portafolio / " + ex.Message.ToString)
        End Try
    End Sub
#End Region

#Region "Funciones"
    Private Sub LimpiarSesiones()

    End Sub

    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacion(False)
        HabilitaDeshabilitaBotonesInicio(False)
        LimpiarDatosOperacion()
    End Sub

    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlPortafolio.Enabled = estado
        txtFechaPago.Enabled = estado
        If estado Then
            imgFechaPago.Attributes.Add("class", "input-append date")
            txtFechaPago.Text = DateTime.Now.ToString("dd/MM/yyyy")
        Else
            imgFechaPago.Attributes.Add("class", "input-append")
        End If
        btnBuscar.Visible = estado
    End Sub

    Private Sub HabilitaDeshabilitaDatosOperacion(ByVal estado As Boolean)
        'txtImporte.Enabled = estado
        btnCalcular.Visible = estado
    End Sub

    Private Sub HabilitaDeshabilitaBotonesInicio(ByVal estado As Boolean)
        btnPrevGrabar.Disabled = Not estado
    End Sub

    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(True)
        btnBuscar.Visible = True
        btnBuscar.Enabled = True
        hdHoraOperacion.Value = New UtilDM().RetornarHoraSistema
    End Sub

    Private Sub LimpiarDatosOperacion()
        If ddlPortafolio.Items.Count > 0 Then ddlPortafolio.SelectedValue = String.Empty
        txtFechaPago.Text = String.Empty
        'txtImporte.Text = 0
        txtTotalIC.Text = 0
        txtFechaRegistro.Text = String.Empty
        '    txtEstado.Text = String.Empty
        Me.gvDistribucion.DataSource = Nothing
        Me.gvDistribucion.DataBind()
        lblFondo.Text = String.Empty
        lblFecha.Text = String.Empty
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaCalcular()
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacion(True)
    End Sub
    Private Sub CargarPaginaCalcular()
        btnCalcular.Visible = True
    End Sub
    Private Sub cargarDistribucion(ByVal CategoriaInstrumento As String, ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal)
        Me.gvDistribucion.DataSource = Nothing
        Me.gvDistribucion.DataBind()
        Dim dtDistribucion As DataTable = oAumentoCapitalBM.AumentoCapital_CalcularDistribucion(CategoriaInstrumento, CodigoPortafolioSBS, FechaAumentoCapital)
        txtTotalIC.Text = dtDistribucion.Compute("SUM([InteresCorrido])", String.Empty).ToString
        gvDistribucion.DataSource = dtDistribucion
        gvDistribucion.DataBind()
    End Sub
    Private Function crearObjetoAumentoCapital() As AumentoCapitalBE

        oAumentoCapitalBM.InicializarAumentoCapital(oRowAC)

        oRowAC.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        oRowAC.FechaAumentoCapital = ConvertirFechaaDecimal(txtFechaPago.Text)
        'oRowAC.Importe = txtImporte.Text
        oRowAC.Estado = IIf(ViewState("EstadoPantalla") = "Ingresar", "P", "E")
        oRowAC.HoraModificacion = New UtilDM().RetornarHoraSistema

        Return oRowAC
    End Function

    Public Function InsertarAumentoCapital() As Integer
        oRowAC = crearObjetoAumentoCapital()
        accion = "ingresó"
        Return oAumentoCapitalBM.AumentoCapital_Insertar(oRowAC, DatosRequest)
    End Function
    Private Function ModificarAumentoCapital() As Integer
        oRowAC = crearObjetoAumentoCapital()
        accion = "modificó"
        Return oAumentoCapitalBM.AumentoCapital_Modificar(oRowAC, ConvertirFechaaDecimal(ViewState("FechaAumentoCapitalOriginal").ToString), DatosRequest)
    End Function
    Private Function EliminarAumentoCapital() As Integer
        accion = "eliminó"
        oRowAC = crearObjetoAumentoCapital()
        Return oAumentoCapitalBM.AumentoCapital_Eliminar(oRowAC, DatosRequest)
    End Function
    Private Sub CargarDatosAumentoCapital()
        Dim dsResult As DataSet
        Dim cantidadreg As Integer
        If Me.ddlPortafolio.SelectedValue = String.Empty Then
            AlertaJS("Falta seleccionar un Portafolio.")
            ddlPortafolio.Focus()
            Exit Sub
        End If

        Me.ddlPortafolio.Enabled = True
        HabilitaDeshabilitaBotonesInicio(False)

        gvDistribucion.DataSource = Nothing
        gvDistribucion.DataBind()
        'txtImporte.Text = String.Empty
        txtFechaRegistro.Text = String.Empty
        'txtImporte.Text = "0"
        txtTotalIC.Text = "0"
        'txtEstado.Text = String.Empty

        cantidadreg = New AumentoCapitalBM().AumentoCapital_ExisteGeneradaOI(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaPago.Text))
        dsResult = oAumentoCapitalBM.AumentoCapital_ListarbyFechaPortafolio(Me.ddlPortafolio.SelectedValue, ConvertirFechaaDecimal(Me.txtFechaPago.Text))

        If dsResult.Tables(2).Rows.Count > 0 And ViewState("EstadoPantalla") = "Eliminar" Then
            For Each row As DataRow In dsResult.Tables(2).Rows
                AlertaJS("Existe un Aumento de Capital con <br/>Fecha: " + CStr(row("FechaAumentoCapital")) + " y estado: " + CStr(row("Estado")) + ",<br/>por lo que no se puede eliminar en la fecha seleccionada.")
            Next
        ElseIf cantidadreg > 0 And ViewState("EstadoPantalla") = "Eliminar" Then
            AlertaJS("No se puede eliminar, por que existen " + cantidadreg.ToString + " órdenes generadas de los instrumentos del aumento de capital.")
        ElseIf dsResult.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In dsResult.Tables(0).Rows
                'txtImporte.Text = CStr(row("Importe"))
                txtFechaRegistro.Text = CStr(row("FechaRegistro"))
                'txtEstado.Text = CStr(row("Estado"))
                ViewState("FechaAumentoCapitalOriginal") = txtFechaPago.Text
            Next

            txtTotalIC.Text = dsResult.Tables(1).Compute("SUM([InteresCorrido])", String.Empty).ToString
            gvDistribucion.DataSource = dsResult.Tables(1)
            gvDistribucion.DataBind()
            Me.ddlPortafolio.Enabled = False
            HabilitaDeshabilitaBotonesInicio(True)
        Else
            AlertaJS("No existe Aumento de Capital para la fecha y portafolio seleccionado.")
        End If

    End Sub
    Private Function ValidarCamposbyGrabar() As Boolean
        If ddlPortafolio.SelectedValue = String.Empty Then
            AlertaJS("Falta seleccionar un Portafolio.")
            ddlPortafolio.Focus()
            Return 0
        End If
        If txtFechaPago.Text.Trim = String.Empty Then
            AlertaJS("Falta seleccionar la fecha de Aumento de Capital.")
            Return 0
        End If
        If ValidarHora(Me.hdHoraOperacion.Value) = False Then
            AlertaJS(ObtenerMensaje("CONF22"))
            Return 0
        End If
        If ViewState("FechaNegocio") Is Nothing Or ViewState("FechaNegocio") = String.Empty Then
            AlertaJS("Hubo un error obteniendo la Fecha de Negocio, vuela a seleccionar el portafolio.")
            ddlPortafolio.SelectedValue = String.Empty
            Return 0
        End If
        If ConvertirFechaaDecimal(txtFechaPago.Text) < ConvertirFechaaDecimal(ViewState("FechaNegocio")) Then
            AlertaJS("La fecha de Aumento de Capital no puede ser menor a la Fecha de Negocio del Portafolio.")
            txtFechaPago.Focus()
            Return 0
        End If

        'If (feriado.VerificaDia(ConvertirFechaaDecimal(txtFechaPago.Text), Session("Mercado")) = False) Then
        '    AlertaJS("Fecha de Vencimiento no es valida.")
        '    Exit Sub
        'End If

        'If txtImporte.Text <= 0 Then
        '    AlertaJS("El importe del Aumento de Capital debe ser mayor a Cero.")
        '    txtImporte.Focus()
        '    Return 0
        'End If

        Dim cantidadreg As Integer = New AumentoCapitalBM().AumentoCapital_ExistePendientebyFechaPortafolio(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text))
        If ViewState("EstadoPantalla") = "Modificar" Then
            If cantidadreg > 1 And txtFechaPago.Text <> ViewState("FechaAumentoCapitalOriginal") Then
                AlertaJS("Ya existe un Aumento de Capital con fecha mayor o igual a: " + txtFechaPago.Text)
                txtFechaPago.Text = String.Empty
                Return 0
            End If
        ElseIf ViewState("EstadoPantalla") = "Ingresar" Then
            If cantidadreg > 0 Then
                AlertaJS("Ya existe un Aumento de Capital con fecha mayor o igual a: " + txtFechaPago.Text)
                ddlPortafolio.SelectedIndex = 0
                Return 0
            End If
        End If
        Return 1
    End Function
#End Region

End Class
