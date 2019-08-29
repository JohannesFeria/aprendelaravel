'Creado por: HDG OT 64480 20120119
Option Explicit On
Option Strict Off

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmRolAprobadoresTrader
    Inherits BasePage


#Region "/* Propiedades */"

    Private Property VistaDetalle() As AprobadorTraderBE
        Get
            Return DirectCast(ViewState("AprobadorTrader"), AprobadorTraderBE)
        End Get
        Set(ByVal Value As AprobadorTraderBE)
            ViewState("AprobadorTrader") = Value
        End Set
    End Property

#End Region

#Region " /* Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnModificarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificarDetalle.Click
        Try
            ModificarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el detalle")
        End Try
    End Sub

    Private Sub btnAgregarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarDetalle.Click
        Try
            Dim strCodUsuario As String = ""
            strCodUsuario = hdCodUsuario.Value
            If Not VerificarAprobadorTrader(Val(hd.Value), strCodUsuario, ddlTipoRenta.SelectedValue) Then
                AlertaJS("Existe un Usuario Aprobador registrado con las mismas características")
                Exit Sub
            End If
            Agregar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al agregar el detalle: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaRolAprobadoresTrader.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Private Sub btnMostrarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMostrarDetalle.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al mostrar el detalle")
        End Try
    End Sub

#End Region

#Region "*/ Funciones Personalizadas */"

    Private Sub Aceptar()
        Dim blnExisteRolTrader As Boolean
        Dim blnDataCorrecta As Boolean
        If hd.Value.Equals(String.Empty) Then
            blnExisteRolTrader = ExisteRolTrader()
            If blnExisteRolTrader Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                blnDataCorrecta = ValidarDataEntry()
                If blnDataCorrecta Then
                    blnDataCorrecta = ValidarAprobadorPrincipal()
                    If blnDataCorrecta Then
                        Insertar()
                        LimpiarControlesCabecera()
                        LimpiarControlesDetalle()
                        Cancelar()
                    Else
                        AlertaJS("Debe ingresar al menos un Usuario Aprobador Principal.")
                    End If
                Else
                    AlertaJS("Debe ingresar al menos un registro en el detalle.")
                End If
            End If
        Else
            Modificar()
        End If
    End Sub

    Private Sub Insertar()
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim oRolTraderBE As New RolTraderBE

        oRolTraderBE = crearObjeto()
        oRolAprobadoresTraderBM.Insertar(oRolTraderBE, VistaDetalle, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
    End Sub

    Private Sub Modificar()
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim oRolTraderBE As New RolTraderBE
        oRolTraderBE = crearObjeto()
        oRolAprobadoresTraderBM.Modificar(oRolTraderBE, VistaDetalle, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Request.QueryString("cod") Is Nothing Then
                hd.Value = String.Empty
                'divDetalle.Visible = False
            Else
                hd.Value = Request.QueryString("cod")
                cargarRegistro(hd.Value)
            End If
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim arraySesiones As String() = New String(5) {}
            arraySesiones = DirectCast(Session("SS_DatosModal"), String())
            hdCodUsuario.Value = arraySesiones(0)
            tbNombre.Text = arraySesiones(1)
            hdCodInterno.Value = arraySesiones(4)
            Session.Remove("SS_DatosModal")
        End If
    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim oRolTraderBE As New RolTraderBE
        Dim oAprobadorTraderBE As AprobadorTraderBE
        Dim oRow As RolTraderBE.RolTraderRow

        oRolTraderBE = oRolAprobadoresTraderBM.Seleccionar(codigo)
        oRow = DirectCast(oRolTraderBE.RolTrader.Rows(0), RolTraderBE.RolTraderRow)

        hd.Value = oRow.CodigoRolTrader.ToString()
        tbDescripcion.Text = oRow.Descripcion.ToString()
        tbCodigo.Text = oRow.TipoCargo.ToString()
        tbCantidadP.Text = oRow.CantidadPrincipal.ToString()
        tbCantidadA.Text = oRow.CantidadAlterno.ToString()
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()

        oAprobadorTraderBE = oRolAprobadoresTraderBM.SeleccionarPorFiltroDetalle(codigo)
        VistaDetalle = oAprobadorTraderBE
        CargarGrilla()

        'divDetalle.Visible = True
        btnMostrarDetalle.Visible = False
        btnModificarDetalle.Visible = False
        btnAgregarDetalle.Visible = True
        DeshabilitarControlesCabecera(True)
    End Sub

    Private Function crearObjeto() As RolTraderBE
        Dim oRolTraderBE As New RolTraderBE
        Dim oRow As RolTraderBE.RolTraderRow

        oRow = DirectCast(oRolTraderBE.RolTrader.NewRolTraderRow(), RolTraderBE.RolTraderRow)

        oRow.CodigoRolTrader = Val(hd.Value)
        oRow.TipoCargo = tbCodigo.Text
        oRow.Descripcion = tbDescripcion.Text.Trim
        oRow.CantidadPrincipal = tbCantidadP.Text
        oRow.CantidadAlterno = tbCantidadA.Text
        oRow.Situacion = ddlSituacion.SelectedValue

        oRolTraderBE.RolTrader.AddRolTraderRow(oRow)
        oRolTraderBE.RolTrader.AcceptChanges()

        Return oRolTraderBE
    End Function

    Private Sub CargarGrilla()
        dgLista.DataSource = VistaDetalle
        dgLista.DataBind()
    End Sub

    Private Sub CargarCombos()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oTipoRentaBM As New TipoRentaBM 'HDG OT 64926 20120320
        Dim dt As New DataTable

        dt = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlSituacionDet, dt, "Valor", "Nombre", False)

        dt = oParametrosGenerales.Listar(TIPO_GRUPO_APROBADOR, DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoGrupo, dt, "Valor", "Nombre", False)
        ddlTipoGrupo.SelectedIndex = 1

        'ini HDG OT 64926 20120320
        dt = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, dt, "CodigoRenta", "Descripcion", True)
        ddlTipoRenta.SelectedValue = TR_DERIVADOS
        ddlTipoRenta.Items.RemoveAt(ddlTipoRenta.SelectedIndex)
        ddlTipoRenta.SelectedIndex = 0
        'fin HDG OT 64926 20120320
    End Sub

    Private Sub LimpiarControlesCabecera()
        tbCodigo.Text = String.Empty
        tbDescripcion.Text = String.Empty
        tbCantidadP.Text = String.Empty
        tbCantidadA.Text = String.Empty
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub LimpiarControlesDetalle()
        tbNombre.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlTipoGrupo.SelectedValue = TIPO_GRUPO_APROBADOR_PRINC
        chkAprobador.Checked = False
        ddlTipoRenta.SelectedValue = ""  'HDG OT 64926 20120320
        ddlSituacionDet.SelectedIndex = 0
    End Sub

    Private Function ExisteRolTrader() As Boolean
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim oRolTraderBE As New RolTraderBE

        oRolTraderBE = oRolAprobadoresTraderBM.Seleccionar(Val(hd.Value.Trim))

        Return oRolTraderBE.RolTrader.Rows.Count > 0
    End Function

    Private Function ValidarAprobadorPrincipal() As Boolean
        Dim oAprobadorTraderBE As AprobadorTraderBE

        oAprobadorTraderBE = VistaDetalle
        Return oAprobadorTraderBE.Tables(0).Select("Tipo = '" & TIPO_GRUPO_APROBADOR_PRINC & "'").Length > 0
    End Function

    Private Function ValidarDataEntry() As Boolean
        Return VistaDetalle.AprobadorTrader.Rows.Count > 0
    End Function

    Private Sub InsertarFilaAprobadorTrader()
        Dim oAprobadorTraderBE As AprobadorTraderBE
        Dim oRow As AprobadorTraderBE.AprobadorTraderRow
        Dim strCodigoUsuario As String = hdCodUsuario.Value

        oAprobadorTraderBE = VistaDetalle
        If oAprobadorTraderBE Is Nothing Then oAprobadorTraderBE = New AprobadorTraderBE

        'If oAprobadorTraderBE.Tables(0).Select("CodigoUsuario = '" & strCodigoUsuario & "'").Length = 0 Then
        oRow = oAprobadorTraderBE.AprobadorTrader.NewAprobadorTraderRow()
        oRow.CodigoRolTrader = Val(hd.Value)
        oRow.CodigoUsuario = strCodigoUsuario
        oRow.CodigoInterno = hdCodInterno.Value
        oRow.NombreUsuario = tbNombre.Text
        oRow.Tipo = ddlTipoGrupo.SelectedValue
        oRow.NombreTipo = ddlTipoGrupo.SelectedItem.Text
        oRow.Aprobador = IIf(chkAprobador.Checked, "1", "0")
        oRow.IndAprobador = IIf(chkAprobador.Checked, "SI", "")
        oRow.Situacion = ddlSituacionDet.SelectedValue
        oRow.NombreSituacion = ddlSituacionDet.SelectedItem.Text
        oRow.CodigoRenta = ddlTipoRenta.SelectedValue    'HDG OT 64926 20120320
        oAprobadorTraderBE.AprobadorTrader.AddAprobadorTraderRow(oRow)
        oAprobadorTraderBE.AprobadorTrader.AcceptChanges()

        VistaDetalle = oAprobadorTraderBE
        'Else
        '    AlertaJS("Existe un Usuario Aprobador ya ingresado")
        'End If
    End Sub

    Private Sub ModificarFilaAprobadorTrader()
        Dim oAprobadorTraderBE As AprobadorTraderBE
        Dim oRow As AprobadorTraderBE.AprobadorTraderRow
        Dim intIndice As Integer

        oAprobadorTraderBE = VistaDetalle
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        oRow = DirectCast(oAprobadorTraderBE.AprobadorTrader.Rows(intIndice), AprobadorTraderBE.AprobadorTraderRow)

        oRow.BeginEdit()
        oRow.CodigoRolTrader = Val(hd.Value)
        oRow.CodigoUsuario = hdCodUsuario.Value
        oRow.CodigoInterno = hdCodInterno.Value
        oRow.NombreUsuario = tbNombre.Text
        oRow.Tipo = ddlTipoGrupo.SelectedValue
        oRow.NombreTipo = ddlTipoGrupo.SelectedItem.Text
        oRow.Aprobador = IIf(chkAprobador.Checked, "1", "0")
        oRow.IndAprobador = IIf(chkAprobador.Checked, "SI", "")
        oRow.Situacion = ddlSituacionDet.SelectedValue
        oRow.NombreSituacion = ddlSituacionDet.SelectedItem.Text
        oRow.CodigoRenta = ddlTipoRenta.SelectedValue
        oRow.EndEdit()
        oRow.AcceptChanges()
        oAprobadorTraderBE.AprobadorTrader.AcceptChanges()

        VistaDetalle = oAprobadorTraderBE
    End Sub

    Private Sub DeshabilitarControlesCabecera(ByVal pEnable As Boolean)
        tbCodigo.Enabled = pEnable
        tbDescripcion.Enabled = pEnable
        tbCantidadP.Enabled = pEnable
        tbCantidadA.Enabled = pEnable
        ddlSituacion.Enabled = pEnable
    End Sub

    Private Sub HabilitarControlesCabecera()
        tbCodigo.Enabled = True
        tbDescripcion.Enabled = True
        tbCantidadP.Enabled = True
        tbCantidadA.Enabled = True
        ddlSituacion.Enabled = True
    End Sub

    Private Sub DeshabilitarControlesDetalle()
        tbNombre.Enabled = False
        ddlTipoGrupo.Enabled = False
        chkAprobador.Enabled = False
        ddlSituacionDet.Enabled = False
    End Sub

    Private Sub ActualizarIndiceGrilla(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        If Not Request.QueryString("cod") Is Nothing Then
            CargarGrilla()
        Else
            hd.Value = String.Empty
        End If
    End Sub

    Private Sub HabilitarControlesDetalle()
        tbNombre.Enabled = True
        ddlTipoGrupo.Enabled = True
        chkAprobador.Enabled = True
        ddlSituacionDet.Enabled = True
    End Sub

    Private Sub Ingresar()
        Dim blnExisteRolTrader As Boolean

        blnExisteRolTrader = ExisteRolTrader()

        If Not blnExisteRolTrader Then
            'divDetalle.Visible = True
            btnMostrarDetalle.Visible = False
            btnModificarDetalle.Visible = False
            DeshabilitarControlesCabecera(False)
            HabilitarControlesDetalle()
        Else
            AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
        End If
    End Sub

    Private Sub Agregar()
        InsertarFilaAprobadorTrader()
        CargarGrilla()
        LimpiarControlesDetalle()
    End Sub

    Private Sub ModificarDetalle()
        ModificarFilaAprobadorTrader()
        CargarGrilla()
        LimpiarControlesDetalle()

        btnModificarDetalle.Visible = False
        btnAgregarDetalle.Visible = True
        HabilitarControlesDetalle()
        If hd.Value.Equals(String.Empty) Then
            btnAgregarDetalle.Visible = True
        End If
    End Sub

    Private Sub EditarGrilla(ByVal indice As Integer)
        Dim oAprobadorTraderBE As AprobadorTraderBE

        oAprobadorTraderBE = VistaDetalle
        ViewState("IndiceSeleccionado") = indice

        tbNombre.Text = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("NombreUsuario").ToString()
        ddlTipoGrupo.SelectedValue = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("Tipo").ToString()
        chkAprobador.Checked = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("Aprobador").ToString()
        ddlSituacionDet.SelectedValue = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("Situacion").ToString()
        ddlTipoRenta.SelectedValue = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("CodigoRenta").ToString()   'HDG OT 64926 20120320
        hdCodUsuario.Value = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("CodigoUsuario").ToString()
        hdCodInterno.Value = oAprobadorTraderBE.AprobadorTrader.Rows(indice)("CodigoInterno").ToString()

        If ddlTipoGrupo.SelectedValue = TIPO_GRUPO_APROBADOR_PRINC Then
            chkAprobador.Enabled = True
        Else
            chkAprobador.Enabled = False
        End If

        btnModificarDetalle.Visible = True
        btnAgregarDetalle.Visible = False
    End Sub

    Private Sub EliminarFilaAprobadorTrader(ByVal indice As Integer)
        Dim oAprobadorTraderBE As AprobadorTraderBE
        Dim oRow As AprobadorTraderBE.AprobadorTraderRow

        oAprobadorTraderBE = VistaDetalle

        oRow = DirectCast(oAprobadorTraderBE.AprobadorTrader.Rows(indice), AprobadorTraderBE.AprobadorTraderRow)
        oRow.BeginEdit()
        ddlSituacionDet.SelectedValue = ESTADO_INACTIVO
        oRow.Situacion = ESTADO_INACTIVO
        oRow.NombreSituacion = ddlSituacionDet.SelectedItem.Text
        oRow.EndEdit()
        oRow.AcceptChanges()
        oAprobadorTraderBE.AprobadorTrader.AcceptChanges()
        ddlSituacionDet.SelectedIndex = 0

        VistaDetalle = oAprobadorTraderBE
        CargarGrilla()
    End Sub

    Private Sub Cancelar()
        Dim oAprobadorTraderBE As AprobadorTraderBE

        oAprobadorTraderBE = VistaDetalle
        oAprobadorTraderBE.AprobadorTrader.Clear()
        oAprobadorTraderBE.AprobadorTrader.AcceptChanges()
        VistaDetalle = oAprobadorTraderBE

        CargarGrilla()

        'divDetalle.Visible = False
        btnMostrarDetalle.Visible = True
        HabilitarControlesCabecera()
    End Sub

    Private Function VerificarAprobadorTrader(ByVal strCodigoRolTrader As String, ByVal strCodigoUsuario As String, ByVal strCodigoRenta As String) As Boolean
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim LngContar As Long
        LngContar = oRolAprobadoresTraderBM.SeleccionarDetalle(strCodigoRolTrader, strCodigoUsuario, strCodigoRenta).Tables(0).Rows.Count
        If LngContar > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

#End Region

    Private Sub dgLista_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        ActualizarIndiceGrilla(e.NewPageIndex)
    End Sub

    Private Sub ddlTipoGrupo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoGrupo.SelectedIndexChanged
        Try
            If ddlTipoGrupo.SelectedValue = TIPO_GRUPO_APROBADOR_PRINC Then
                chkAprobador.Enabled = True
            Else
                chkAprobador.Enabled = False
                chkAprobador.Checked = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Tipo de Aprobador")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName.Equals("Modificar") Then
                EditarGrilla(e.CommandArgument.ToString().Split("|")(1))
            End If

            If e.CommandName.Equals("Eliminar") Then
                EliminarFilaAprobadorTrader(e.CommandArgument.ToString().Split("|")(1))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub
End Class
