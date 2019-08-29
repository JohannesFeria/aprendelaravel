Imports System.IO
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Parametria_Tablas_Generales_frmVacaciones
    Inherits BasePage

    Dim oMovimientoPersonalBM As New MovimientoPersonalBM
    Dim oPersonalBM As New PersonalBM

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoUsuario.Text = CType(Session("SS_DatosModal")(0), String)
                tbNombreUsuario.Text = CType(Session("SS_DatosModal")(1), String)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim dt As New DataTable
            Dim rutaFirmas As String
            Dim rutaImagenFirma As String 'HDG OT 64016 20111017
            Dim oMovimientoPersonalRow As MovimientoPersonalBE.MovimientoPersonalRow
            Dim oMovimientoPersonalBE As New MovimientoPersonalBE
            dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.RUTA_FIRMA_CARTA, DatosRequest)
            rutaFirmas = CType(dt.Rows(0)("Comentario"), String)

            
            If Not Request.QueryString("cod") Is Nothing Then
                oMovimientoPersonalRow = CType(oMovimientoPersonalBE.MovimientoPersonal.NewRow(), MovimientoPersonalBE.MovimientoPersonalRow)
                oMovimientoPersonalBM.InicializarMovimientoPersonal(oMovimientoPersonalRow)

                oMovimientoPersonalRow.CodigoInterno = tbCodigoUsuario.Text
                oMovimientoPersonalRow.FechaInicio = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
                oMovimientoPersonalRow.FechaFin = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
                oMovimientoPersonalRow.CodigoUsuario = tbCodigoUsuario.Text
                oMovimientoPersonalRow.Estado = ddlSituacion.SelectedValue

                oMovimientoPersonalBE.MovimientoPersonal.Rows.Add(oMovimientoPersonalRow)
                oMovimientoPersonalBE.MovimientoPersonal.AcceptChanges()
                oMovimientoPersonalBM.Modificar(oMovimientoPersonalBE, DatosRequest)
                Me.AlertaJS("Los cambios se han realizado satisfactoriamente!")
            Else
                oMovimientoPersonalRow = CType(oMovimientoPersonalBE.MovimientoPersonal.NewRow(), MovimientoPersonalBE.MovimientoPersonalRow)
                oMovimientoPersonalBM.InicializarMovimientoPersonal(oMovimientoPersonalRow)

                oMovimientoPersonalRow.CodigoInterno = tbCodigoUsuario.Text
                oMovimientoPersonalRow.FechaInicio = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
                oMovimientoPersonalRow.FechaFin = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
                oMovimientoPersonalRow.CodigoUsuario = tbCodigoUsuario.Text
                oMovimientoPersonalRow.Estado = ddlSituacion.SelectedValue

                oMovimientoPersonalBE.MovimientoPersonal.Rows.Add(oMovimientoPersonalRow)
                oMovimientoPersonalBE.MovimientoPersonal.AcceptChanges()
                If oMovimientoPersonalBM.Insertar(oMovimientoPersonalBE, DatosRequest) Then
                    Me.AlertaJS("Se ha registrado satisfactoriamente!")
                Else
                    Me.AlertaJS("Ya existe este Personal registrado.!")
                End If

                LimpiarCampos()
            End If

        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Call Retornar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

    Private Sub Retornar()
        Response.Redirect("frmBusquedaVacaciones.aspx?codInterno=" & Request.QueryString("codInterno") & "&situacion=" & Request.QueryString("situacion"))
    End Sub

    Private Sub CargarPagina()

        CargarCombos()
        If Not Request.QueryString("cod") Is Nothing Then
            HabilitarControles(False)
            Dim dt As DataTable
            dt = oMovimientoPersonalBM.SeleccionarPorFiltro(Request.QueryString("cod"), "", DatosRequest).Tables(0)
            tbCodigoUsuario.Text = CType(dt.Rows(0)("CodigoInterno"), String)
            tbNombreUsuario.Text = CType(dt.Rows(0)("Nombre"), String)
            tbFechaInicio.Text = CType(dt.Rows(0)("FecINI"), String)
            tbFechaFin.Text = CType(dt.Rows(0)("FecFIN"), String)
            ddlSituacion.SelectedValue = CType(dt.Rows(0)("Estado"), String)
        Else
            HabilitarControles(True)
            HabilitaFirma(False)
            lkbBuscarUsuario.Enabled = True
            lkbBuscarUsuario.Attributes.Add("onclick", "javascript:return showPopupUsuarios();")
        End If
    End Sub

    Private Sub CargarCombos()
        CargarSituacion()
    End Sub

    Private Sub CargarSituacion()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.SITUACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", False)
        ddlSituacion.SelectedValue = ParametrosSIT.ESTADO_ACTIVO
    End Sub

    Private Sub HabilitarControles(ByVal habilita As Boolean)
        tbCodigoUsuario.Enabled = habilita
        lkbBuscarUsuario.Enabled = habilita
    End Sub

    Private Sub LimpiarCampos()
        tbCodigoUsuario.Text = ""
        tbNombreUsuario.Text = ""
        tbFechaFin.Text = ""
        tbFechaInicio.Text = ""
        'ddlRol.ClearSelection()
        ddlSituacion.ClearSelection()
        HabilitaFirma(False)
    End Sub

    Private Sub HabilitaFirma(ByVal habilita As Boolean)

    End Sub

End Class
