Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmTiposInstrumentos
    Inherits BasePage

    Private Const S_CuentaBCR As String = "CuentasBCR"
    Private Const VIEW_IndiceFila As String = "IndiceFila"
    Private valorGrilla As String = ""


#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la pagina")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaTiposInstrumentos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Seleccionar */ "

#End Region

#Region " /* Funciones Insertar */ "

    Private Sub Insertar()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oTipoInstrumentoCuentaBCRBE As New TipoInstrumentoCuentaBCRBE
        oTipoInstrumentoBE = crearObjeto()
        oTipoInstrumentoCuentaBCRBE = crearCuentasBCR()
        oTipoInstrumentoBM.Insertar(oTipoInstrumentoBE, oTipoInstrumentoCuentaBCRBE, DatosRequest)
        Me.AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub
#End Region

#Region " /* Funciones Modificar */"

    Private Sub Modificar()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oTipoInstrumentoCuentaBCRBE As New TipoInstrumentoCuentaBCRBE
        oTipoInstrumentoBE = crearObjeto()
        oTipoInstrumentoCuentaBCRBE = crearCuentasBCR()
        oTipoInstrumentoBM.Modificar(oTipoInstrumentoBE, oTipoInstrumentoCuentaBCRBE, DatosRequest)
        Me.AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

#End Region

#Region " /* Funciones Eliminar */"

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            btnModificar.Visible = False
            btnCancelar.Visible = False
            If Not Request.QueryString("cod") Is Nothing Then
                tbCodigo.Enabled = False
                Me.hd.Value = Request.QueryString("cod")
                valorGrilla = hd.Value
                CargarRegistro(hd.Value)
                CargarGrilla(valorGrilla)
            Else
                valorGrilla = "-1"
                CargarGrilla(valorGrilla)
                tbCodigo.Enabled = True
                Me.hd.Value = String.Empty
            End If
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim arraySesiones As String() = New String(2) {}
            arraySesiones = DirectCast(Session("SS_DatosModal"), String())
            Me.tbCodigoEntidad.Text = arraySesiones(0)
            Session.Remove("SS_DatosModal")
        End If
    End Sub

    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If ddlTipoValoracion.SelectedIndex <= 0 Then
            AlertaJS("Debe seleccionar un tipo de Valorizacion.")
            Exit Sub
        End If
        If dgCuenta.Rows.Count <= 0 Then
            AlertaJS("Debe ingresar una cuenta contable")
            Exit Sub
        End If
        If Me.hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = verificarExistencia()
            If blnExisteEntidad Then
                Me.AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                If verificarExistenciaSBS_Nuevo() = True Then
                    AlertaJS("CONF39")
                    Exit Sub
                End If
                Insertar()
            End If
        Else
            Modificar()
        End If
    End Sub

    Private Function verificarExistenciaSBS_Nuevo() As Boolean
        Dim oTI As New TipoInstrumentoBM
        Return oTI.VerificarExistenciaSBS_Nuevo(Me.txtCodigoSBS.Text, DatosRequest)
    End Function

    Private Function verificarExistenciaSBS_Modificar() As Boolean
        Dim oTI As New TipoInstrumentoBM
        Return oTI.VerificarExistenciaSBS_Modificar(Me.txtCodigoSBS.Text, tbCodigo.Text, DatosRequest)
    End Function

    Private Sub CargarRegistro(ByVal Codigo As String)
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As TipoInstrumentoBE.TipoInstrumentoRow
        oTipoInstrumentoBE = oTipoInstrumentoBM.Seleccionar(Codigo, DatosRequest)
        oRow = DirectCast(oTipoInstrumentoBE.TipoInstrumento.Rows(0), TipoInstrumentoBE.TipoInstrumentoRow)
        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        Me.tbPlazoLiquidacion.Text = oRow.PlazoLiquidacion.ToString().Trim.ToUpper
        Try
            Me.ddlClaseInstrumento.SelectedValue = oRow.CodigoClaseInstrumento.ToString()
        Catch ex As Exception
            Me.ddlClaseInstrumento.SelectedIndex = 0
        End Try

        Try
            Me.ddlTipoRenta.SelectedValue = oRow.CodigoRenta.ToString()
        Catch ex As Exception
            Me.ddlTipoRenta.SelectedIndex = 0
        End Try

        Try
            Me.ddlTipoTasa.SelectedValue = oRow.TipoTasa.ToString()
        Catch ex As Exception
            Me.ddlTipoTasa.SelectedIndex = 0
        End Try

        Me.tbDescripcion.Text = oRow.Descripcion.ToString().Trim.ToUpper
        Try
            Me.ddlTipoValoracion.SelectedValue = oRow.CodigoTipoValorizacion
        Catch ex As Exception
            Me.ddlTipoValoracion.SelectedIndex = 0
        End Try

        Me.txtTasaEncaje.Text = oRow.TasaEncaje.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        Me.txtCodigoSBS.Text = oRow.CodigoTipoInstrumentoSBS
        Me.tbCodigo.Text = oRow.CodigoTipoInstrumento
        Me.hd.Value = oRow.CodigoTipoInstrumento.ToString()
        Me.txtPND.Text = oRow.PND.ToString().Replace(UIUtility.DecimalSeparator(), ".") 'JRM 20100428
        Me.txtPNND.Text = oRow.PNND.ToString().Replace(UIUtility.DecimalSeparator(), ".") 'JRM 20100428

        'ini HDG OT 58687 20100303
        Try
            Me.txtGrupoRiesgo.Text = oRow.GrupoRiesgo.ToString()
        Catch ex As Exception
            Me.txtGrupoRiesgo.Text = ""
        End Try
        'fin HDG OT 58687 20100303

        ddlMetodoCalculoRenta.SelectedValue = oRow.MetodoCalculoRenta 'RGF 20101229 OT 61897
    End Sub

    Private Function crearObjeto() As TipoInstrumentoBE

        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As TipoInstrumentoBE.TipoInstrumentoRow

        oRow = DirectCast(oTipoInstrumentoBE.TipoInstrumento.NewRow(), TipoInstrumentoBE.TipoInstrumentoRow)

        oRow.CodigoTipoInstrumentoSBS = Me.txtCodigoSBS.Text
        oRow.CodigoClaseInstrumento = Me.ddlClaseInstrumento.SelectedValue()
        oRow.CodigoRenta = Me.ddlTipoRenta.SelectedValue()
        oRow.TipoTasa = Me.ddlTipoTasa.SelectedValue()
        oRow.PlazoLiquidacion = Me.tbPlazoLiquidacion.Text.ToString.Trim.ToUpper
        oRow.Descripcion = Me.tbDescripcion.Text.ToString.Trim.ToUpper
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.TasaEncaje = Me.txtTasaEncaje.Text '.Replace(".", UIUtility.DecimalSeparator())
        oRow.CodigoTipoValorizacion = Me.ddlTipoValoracion.SelectedValue
        oRow.GrupoRiesgo = Me.txtGrupoRiesgo.Text   'HDG OT 58687 20100302
        oRow.PND = Me.txtPND.Text 'JRM 20100428
        oRow.PNND = Me.txtPNND.Text 'JRM 20100428
        If Not Me.hd.Value.Equals(String.Empty) Then
            oRow.CodigoTipoInstrumento = hd.Value
        Else
            oRow.CodigoTipoInstrumento = Me.tbCodigo.Text.Trim.ToUpper
        End If

        oRow.MetodoCalculoRenta = ddlMetodoCalculoRenta.SelectedValue 'RGF 20101229 OT 61897

        oTipoInstrumentoBE.TipoInstrumento.AddTipoInstrumentoRow(oRow)
        oTipoInstrumentoBE.TipoInstrumento.AcceptChanges()

        Return oTipoInstrumentoBE

    End Function

    Private Sub CargarCombos()

        Dim tablaTipoRenta As New Data.DataTable
        Dim tablaClaseInstrumento As New Data.DataTable
        Dim tablaSituacion As New DataTable
        Dim tablaTipoTasa As New DataTable
        Dim tablaCuenta As New DataTable
        Dim dtTipoValoracion As DataTable
        Dim dtMetodoCalculoRenta As DataTable 'RGF 20101229 OT 61897

        Dim oParametrosGenerales As New ParametrosGeneralesBM

        Dim oTipoRentaBM As New TipoRentaBM
        Dim oTipoValoracionBM As New TipoValorizacionBM
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        tablaTipoTasa = oParametrosGenerales.ListarTipoTasa(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlTipoTasa, tablaTipoTasa, "Valor", "Nombre", True)

        tablaClaseInstrumento = oClaseInstrumentoBM.Listar(DatosRequest).Tables(0)
        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlClaseInstrumento, tablaClaseInstrumento, "CodigoClaseInstrumento", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)

        dtTipoValoracion = oTipoValoracionBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoValoracion, dtTipoValoracion, "CodigoTipoValorizacion", "Descripcion", True)

        tablaCuenta = oParametrosGenerales.Listar("CuentaBCR", Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlCuentaContable, tablaCuenta, "Comentario", "Valor", True)
        'HelpCombo.LlenarComboBox(Me.ddlCuentaContable, tablaCuenta, "Nombre", "Valor", True)

        'RGF 20101229 OT 61897
        dtMetodoCalculoRenta = oParametrosGenerales.Listar("METO_CALC", Me.DatosRequest)
        HelpCombo.LlenarComboBox(ddlMetodoCalculoRenta, dtMetodoCalculoRenta, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarCampos()

        Me.tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbPlazoLiquidacion.Text = Constantes.M_STR_TEXTO_INICIAL
        txtPND.Text = String.Empty
        txtPNND.Text = String.Empty
        'RGF 20080827
        'Me.ddlClaseInstrumento.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
        ddlClaseInstrumento.SelectedIndex = 0
        'Me.ddlTipoRenta.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
        ddlTipoRenta.SelectedIndex = 0
        'Me.ddlTipoTasa.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
        ddlTipoTasa.SelectedIndex = 0
        ddlMetodoCalculoRenta.SelectedIndex = 0 'RGF 20101229 OT 61897
        Me.txtTasaEncaje.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedValue = "A"
        Me.txtCodigoSBS.Text = Constantes.M_STR_TEXTO_INICIAL
        valorGrilla = "-1"
        CargarGrilla(valorGrilla)
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoInstrumentosBM As New TipoInstrumentoBM
        Dim oTipoInstrumentosBE As New TipoInstrumentoBE
        oTipoInstrumentosBE = oTipoInstrumentosBM.Seleccionar(Me.tbCodigo.Text, DatosRequest)
        If oTipoInstrumentosBE.TipoInstrumento.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            If (ValidarCuentaContable() = False) Then
                MensajeCuentaContable()
                Exit Sub
            End If
            Dim dt As DataTable
            Dim oRow As DataRow
            dt = CType(Session(S_CuentaBCR), DataTable)
            oRow = dt.NewRow()
            oRow.Item("CodigoTipoInstrumentoCuentaBCR") = -1
            oRow.Item("CodigoTipoInstrumento") = tbCodigo.Text
            oRow.Item("CodigoTipoInstrumentoSBS") = txtCodigoSBS.Text
            oRow.Item("CodigoEntidad") = tbCodigoEntidad.Text
            oRow.Item("CuentaContable") = ddlCuentaContable.SelectedItem.Text 'RGF 20090911
            'oRow.Item("CuentaContable") = ddlCuentaContable.SelectedValue
            oRow.Item("NombreCuenta") = txtNombreCuenta.Text

            dt.Rows.Add(oRow)
            Session(S_CuentaBCR) = dt
            dgCuenta.DataSource = dt
            dgCuenta.DataBind()
            LimpiarCamposDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al agregar el registro")
        End Try
    End Sub

    Private Sub CargarGrilla(ByVal CodigoInstrumento As String)
        Dim oTipoInstrumentoCuentaBCRBM As New TipoInstrumentoCuentaBCRBM
        Session(S_CuentaBCR) = oTipoInstrumentoCuentaBCRBM.SeleccionarPorFiltro(CodigoInstrumento)
        dgCuenta.DataSource = CType(Session(S_CuentaBCR), DataTable)
        dgCuenta.DataBind()
    End Sub

    Private Sub LimpiarCamposDetalle()
        tbCodigoEntidad.Text = ""
        ddlCuentaContable.SelectedValue = ""
        txtNombreCuenta.Text = ""
    End Sub

    Protected Sub dgCuenta_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgCuenta.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("imbEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgCuenta_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCuenta.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar" : EliminarCuenta(CStr(e.CommandArgument))
                Case "Select"
                    Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                    ModificarCuenta(Row) 'es modificar'
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgCuenta_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCuenta.PageIndexChanging
        Try
            dgCuenta.PageIndex = e.NewPageIndex
            CargarGrilla(valorGrilla)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try    
    End Sub

    Private Sub EliminarCuenta(ByVal CodigoEntidad As String)
        Dim dt As DataTable
        Dim oRow As DataRow

        dt = CType(Session(S_CuentaBCR), DataTable)
        For Each oRow In dt.Rows
            If oRow.Item("CodigoEntidad") = CodigoEntidad Then
                dt.Rows.Remove(oRow)
                Exit For
            End If
        Next

        LimpiarCamposDetalle()
        tbCodigoEntidad.ReadOnly = False
        lkbBuscarEntidad.Visible = True
        btnAgregar.Visible = True
        btnModificar.Visible = False
        btnCancelar.Visible = False
        dgCuenta.DataSource = dt
        dgCuenta.DataBind()
    End Sub

    Private Sub ModificarCuenta(ByVal row As GridViewRow)
        Dim dt As DataTable
        dt = CType(Session(S_CuentaBCR), DataTable)
        tbCodigoEntidad.Text = dt.Rows(row.RowIndex)("CodigoEntidad")
        ddlCuentaContable.SelectedValue = dt.Rows(row.RowIndex)("NombreCuenta") 'RGF 20090911
        'ddlCuentaContable.SelectedValue = dt.Rows(item.DataSetIndex)("CuentaContable")
        txtNombreCuenta.Text = dt.Rows(row.RowIndex)("NombreCuenta")
        ViewState(VIEW_IndiceFila) = row.RowIndex

        tbCodigoEntidad.ReadOnly = True
        lkbBuscarEntidad.Visible = False
        btnAgregar.Visible = False
        btnModificar.Visible = True
        btnCancelar.Visible = True

        'divBtnAgregar.Style("Display") = "none"
        'divBtnModificar.Style("Display") = "block"
        'divBtnCancelar.Style("Display") = "block"
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Dim dt As DataTable
        Dim oRow As DataRow
        dt = CType(Session(S_CuentaBCR), DataTable)
        For Each oRow In dt.Rows
            If oRow.Item("CodigoEntidad") = tbCodigoEntidad.Text Then
                oRow.Item("CodigoEntidad") = tbCodigoEntidad.Text
                oRow.Item("CuentaContable") = ddlCuentaContable.SelectedItem.Text 'RGF 20090911
                'oRow.Item("CuentaContable") = ddlCuentaContable.SelectedValue
                oRow.Item("NombreCuenta") = txtNombreCuenta.Text
                Exit For
            End If
        Next

        Session(S_CuentaBCR) = dt
        dgCuenta.DataSource = dt
        dgCuenta.DataBind()
        dgCuenta.SelectedIndex = -1
        LimpiarCamposDetalle()

        tbCodigoEntidad.ReadOnly = False
        lkbBuscarEntidad.Visible = True
        btnAgregar.Visible = True
        btnModificar.Visible = False
        btnCancelar.Visible = False
        'AlertaJS("Se modificó los datos correctamente")
    End Sub

    Private Function ValidarCuentaContable() As Boolean
        Dim resultado As Boolean = True
        Dim dt As DataTable
        Dim oRow As DataRow
        dt = CType(Session(S_CuentaBCR), DataTable)
        For Each oRow In dt.Rows
            'If oRow.Item("CodigoEntidad") = tbCodigoEntidad.Text Or oRow.Item("CuentaContable") = ddlCuentaContable.SelectedItem.Text Then
            If oRow.Item("CodigoEntidad") = tbCodigoEntidad.Text Then
                resultado = False
                Exit For
            End If
        Next
        Return resultado
    End Function

    Private Sub MensajeCuentaContable()
        AlertaJS("La Entidad ya existe.")
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        btnAgregar.Visible = True
        btnModificar.Visible = False
        btnCancelar.Visible = False
        LimpiarCamposDetalle()
        dgCuenta.SelectedIndex = -1
    End Sub

    Private Function crearCuentasBCR() As TipoInstrumentoCuentaBCRBE
        Dim oTipoInstrumentoCuentaBCR As New TipoInstrumentoCuentaBCRBE
        If Not Session(S_CuentaBCR) Is Nothing Then

            Dim oTipoInstrumentoCuentaBCRRow As TipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBERow
            Dim oRow As DataRow
            Dim dt As New DataTable
            dt = CType(Session(S_CuentaBCR), DataTable)

            For Each oRow In dt.Rows
                oTipoInstrumentoCuentaBCRRow = DirectCast(oTipoInstrumentoCuentaBCR.TipoInstrumentoCuentaBCRBE.NewTipoInstrumentoCuentaBCRBERow, TipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBERow)
                oTipoInstrumentoCuentaBCRRow.Item("CodigoTipoInstrumentoCuentaBCR") = oRow.Item("CodigoTipoInstrumentoCuentaBCR")
                oTipoInstrumentoCuentaBCRRow.Item("CodigoTipoInstrumento") = oRow.Item("CodigoTipoInstrumento")
                oTipoInstrumentoCuentaBCRRow.Item("CodigoTipoInstrumentoSBS") = txtCodigoSBS.Text
                oTipoInstrumentoCuentaBCRRow.Item("CodigoEntidad") = oRow.Item("CodigoEntidad")
                oTipoInstrumentoCuentaBCRRow.Item("CuentaContable") = oRow.Item("CuentaContable")
                'oTipoInstrumentoCuentaBCRRow.Item("NombreCuenta") = oRow.Item("NombreCuenta") 'RGF 20090911
                oTipoInstrumentoCuentaBCRRow.Item("CodigoTipoInstrumentoCuentaBCR") = oRow.Item("CodigoTipoInstrumentoCuentaBCR")
                oTipoInstrumentoCuentaBCR.TipoInstrumentoCuentaBCRBE.AddTipoInstrumentoCuentaBCRBERow(oTipoInstrumentoCuentaBCRRow)
                oTipoInstrumentoCuentaBCR.AcceptChanges()
            Next
        End If
        Return oTipoInstrumentoCuentaBCR
    End Function

    Private Sub ddlCuentaContable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCuentaContable.SelectedIndexChanged
        Try
            txtNombreCuenta.Text = ddlCuentaContable.SelectedValue
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar la Cuenta Contable")
        End Try
    End Sub

End Class
