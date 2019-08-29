Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Entidades_frmTerceros
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
#Region " Web Form Designer Generated Code "
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    End Sub
    Protected WithEvents txtCodigoTipoDocumento As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCodigoSectorEmpresarial As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPais As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCustodio As System.Web.UI.WebControls.Label
    Private designerPlaceholderDeclaration As System.Object
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region
#Region "/* Declaracion de Variables */"
    Public Enum CLASE_ENTIDAD As Byte
        [EMISOR] = 0
        [ENTIDAD_VINCULADA] = 1
        [ENTIDAD_VIGILADA] = 2
        [BORKER] = 3
        [AVAL] = 4
        [CUSTODIO] = 5
        [RIESGO_CERO] = 6
        [ENTIDAD_FINANCIERA] = 7
        [COMISIONISTA] = 8
        [ENTIDAD_REGULADORA] = 9
    End Enum
#End Region
#Region "/* Propiedades */"
    Private Property VistaDetalleCuentas() As DataSet
        Get
            Return ViewState("Cuentas")
        End Get
        Set(ByVal Value As DataSet)
            ViewState("Cuentas") = Value
        End Set
    End Property
#End Region
#Region " */ Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.lblCustodio = New System.Web.UI.WebControls.Label
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub ddlClasificacionTercero_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClasificacionTercero.SelectedIndexChanged
        Try
            CambiarEstadoCustodio()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Selecionar")
        End Try
    End Sub
    Private Sub ddlPais_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            If ddlPais.SelectedItem.Text.ToString.ToUpper.Trim = "PERÚ" Or ddlPais.SelectedItem.Text.ToString.ToUpper.Trim = "PERU" Then
                ddlCodigoPostal.Enabled = True
            Else
                ddlCodigoPostal.SelectedIndex = 0
                ddlCodigoPostal.Enabled = False
            End If

            divFortalezaFinanciera.Visible = (ddlTipoTercero.SelectedValue = "ENFI" And ddlPais.SelectedValue = "604")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub

    Private Sub ddlTipoTercero_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoTercero.SelectedIndexChanged
        Try
            divFortalezaFinanciera.Visible = (ddlTipoTercero.SelectedValue = "ENFI" And ddlPais.SelectedValue = "604")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer = 0
            Dim secuencial As Integer = 0
            Dim liqAutomatica As String = ""
            If e.CommandName.Equals("Modificar") Then
                index = Integer.Parse(e.CommandArgument.ToString())
                ViewState("IndiceSeleccionado") = index
                Dim auxSecuencial As String = dgLista.Rows(index).Cells(17).Text.Trim.Replace("&nbsp;", "")
                secuencial = Integer.Parse(IIf(auxSecuencial = "", "0", auxSecuencial).ToString())
                hdSecuencial.Value = secuencial
                ddlBanco.SelectedValue = dgLista.Rows(index).Cells(3).Text.Trim.Replace("&nbsp;", "")
                liqAutomatica = dgLista.Rows(index).Cells(14).Text.Trim.Replace("&nbsp;", "")
                cbLiqAutomatica.Checked = IIf(liqAutomatica = "S", True, False)
                txtNumeroCuenta.Text = dgLista.Rows(index).Cells(5).Text.Trim.Replace("&nbsp;", "")
                txtCuentaInterBancario.Text = dgLista.Rows(index).Cells(6).Text.Trim.Replace("&nbsp;", "")
                ddlMercadoTercero.SelectedValue = dgLista.Rows(index).Cells(10).Text.Trim.Replace("&nbsp;", "")
                ddlPortafolio.SelectedValue = dgLista.Rows(index).Cells(8).Text.Trim.Replace("&nbsp;", "")
                ddlMonedaTercero.SelectedValue = dgLista.Rows(index).Cells(12).Text.Trim.Replace("&nbsp;", "")
                ddlSituacionDet.SelectedValue = dgLista.Rows(index).Cells(15).Text.Trim.Replace("&nbsp;", "")
                If liqAutomatica = "S" Then
                    txtNumeroCuenta.Enabled = False
                    txtCuentaInterBancario.Enabled = False
                    ddlMercadoTercero.Enabled = False
                    ddlPortafolio.Enabled = False
                End If
                ibnAgregarDetalle.Text = "Modificar"
                'EditarGrilla(e.CommandArgument.ToString().Split("|")(1))
            End If
            If e.CommandName.Equals("Eliminar") Then
                index = Integer.Parse(e.CommandArgument.ToString())
                ViewState("IndiceSeleccionado") = index
                EliminarFilaCuenta()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la operación de la Grilla")
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            ActualizarIndiceGrilla(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ibEliminar As ImageButton
                ibEliminar = DirectCast(e.Row.FindControl("ibnEliminar"), ImageButton)
                ibEliminar.Attributes.Add("onclick", ConfirmJS("¿Confirmar la eliminación del registro?"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Protected Sub ibRetornarDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibRetornarDetalle.Click
        Try
            Response.Redirect("frmBusquedaTerceros.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar Detalle")
        End Try
    End Sub
    Protected Sub ibnAgregarDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibnAgregarDetalle.Click
        Try
            Me.Agregar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar Detalle: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub ibnModificarDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibnModificarDetalle.Click
        Try
            Me.ModificarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Detalle")
        End Try
    End Sub
    Protected Sub ibnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub ibnEliminar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton1.Click  'CMB SE CAMBIO ESTA LINEA PORQUE GENERA ERROR
        Try
            Dim oEntidadBM As New EntidadBM
            Dim eliminado As Boolean = False
            Dim codigoEntidad As String = String.Empty
            Dim existeCodEntidad As Boolean = False
            Dim dtblDatos As DataTable
            codigoEntidad = txtCodigoEntidad.Text
            If codigoEntidad.ToString() <> String.Empty Then
                dtblDatos = oEntidadBM.SeleccionarPorFiltro(codigoEntidad, "", "", "", "", "", "", "", DatosRequest).Tables(0)
                If dtblDatos.Rows.Count > 0 Then
                    If codigoEntidad <> String.Empty Then
                        eliminado = oEntidadBM.Eliminar_Registro_Fisico(codigoEntidad, Me.DatosRequest)
                    End If
                Else
                    AlertaJS("No existe entidad para eliminar.")
                End If
            Else
                AlertaJS("No existe entidad para eliminar.") : Exit Sub
            End If
            If eliminado = True Then
                LimpiarRegistrosEntidad()
                txtCodigoEntidad.Enabled = True
                AlertaJS("La entidad: " + codigoEntidad + " se ha eliminado correctamente")
            End If
        Catch exSQL As SqlClient.SqlException
            AlertaJS("La entidad no se puede eliminar debido a que se encuentra relacionada a otras tablas.")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar")
        End Try
    End Sub
    Private Sub chlClase_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chlClase.SelectedIndexChanged
        Try
            If (chlClase.Items(CLASE_ENTIDAD.BORKER).Selected = True) Then
                Me.txtCodigoBroker.Enabled = True
            Else
                Me.txtCodigoBroker.Text = ""
                Me.txtCodigoBroker.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Protected Sub cbLiqAutomatica_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLiqAutomatica.CheckedChanged
        If cbLiqAutomatica.Checked Then
            txtNumeroCuenta.Enabled = False
            txtNumeroCuenta.Text = String.Empty
            txtCuentaInterBancario.Enabled = False
            txtCuentaInterBancario.Text = String.Empty
            ddlPortafolio.Enabled = False
            ddlPortafolio.SelectedValue = ""
            ddlMercadoTercero.Enabled = False
            ddlMercadoTercero.SelectedValue = ""
        Else
            txtNumeroCuenta.Enabled = True
            txtCuentaInterBancario.Enabled = True
            ddlPortafolio.Enabled = True
            ddlMercadoTercero.Enabled = True
        End If
    End Sub
    Protected Sub chkFondoVinculado_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFondoVinculado.CheckedChanged
        If chkFondoVinculado.Checked = True Then
            lbFondoVinculado.Visible = True
        Else
            lbFondoVinculado.Visible = False
        End If
    End Sub
#End Region

#Region "*/ Funciones Personalizadas */"
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cod") Is Nothing Then
                hd.Value = Request.QueryString("cod")
                CargarControles(hd.Value)
            Else
                hd.Value = String.Empty
                CargaGrilla(hd.Value)
                If ddlClasificacionTercero.SelectedValue.ToString = "I" Then
                    Me.ddlCustodio.Visible = True
                    Me.lblCustodio.Visible = True
                Else
                    Me.ddlCustodio.Visible = False
                    Me.lblCustodio.Visible = False
                End If
                If ddlPais.SelectedItem.Text.ToString.ToUpper.Trim = "PERÚ" Or ddlPais.SelectedItem.Text.ToString.ToUpper.Trim = "PERU" Then
                    ddlCodigoPostal.Enabled = True
                Else
                    ddlCodigoPostal.SelectedIndex = 0
                    ddlCodigoPostal.Enabled = False
                End If
            End If
            If chkFondoVinculado.Checked = False Then
                lbFondoVinculado.Visible = False
            Else
                lbFondoVinculado.Visible = True
            End If
        Else
            If ddlClasificacionTercero.SelectedValue.ToString = "I" Then
                Me.ddlCustodio.Visible = True
                Me.lblCustodio.Visible = True
            Else
                Me.ddlCustodio.Visible = False
                Me.lblCustodio.Visible = False
            End If

            If ddlPais.SelectedItem.Text.ToString.ToUpper.Trim = "PERÚ" Or ddlPais.SelectedItem.Text.ToString.ToUpper.Trim = "PERU" Then
                ddlCodigoPostal.Enabled = True
            Else
                ddlCodigoPostal.SelectedIndex = 0
                ddlCodigoPostal.Enabled = False
            End If
        End If

        ddlTipoTercero_SelectedIndexChanged(Nothing, Nothing)
    End Sub
    Private Sub Aceptar()
        Dim blnExisteEntidad, blnExisteCodigoSBS As Boolean
        If Me.hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = Me.ExisteEntidad()
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                blnExisteCodigoSBS = ExisteCodigoSBS(Me.txtCodigoEntidad.Text, Me.txtCodigoSBS.Text, "S")
                If blnExisteCodigoSBS Then
                    AlertaJS("El Codigo SBS asociado ya existe")
                Else
                    Insertar()
                End If
            End If
        Else
            blnExisteCodigoSBS = ExisteCodigoSBS(Me.txtCodigoEntidad.Text, Me.txtCodigoSBS.Text, "N")
            If blnExisteCodigoSBS Then
                AlertaJS("El Codigo SBS asociado ya existe")
            ElseIf ddlSituacion.SelectedValue = "I" Then
                Dim objTercero As New TercerosBM
                If objTercero.VerificarExisteTerceroNegociado(txtCodigoTercero.Text.Trim) Then
                    Throw New System.Exception("El tercero ya ha sido utilizado en la negociación de operaciones de inversión. No se puede eliminar.")
                End If
            Else
                Modificar()
            End If
        End If
    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oTerceroBM As New TercerosBM
        Dim oTerceroBE As TercerosBE
        oTerceroBE = oTerceroBM.Seleccionar(Me.txtCodigoTercero.Text, Me.DatosRequest)
        Return oTerceroBE.Terceros.Rows.Count > 0
    End Function
    Private Function ExisteEntidad_ID() As Boolean
        Dim oEntidadBM As New EntidadBM
        Dim oEntidadBE As EntidadBE
        oEntidadBE = oEntidadBM.Seleccionar(Me.txtCodigoEntidad.Text, Me.DatosRequest)
        Return oEntidadBE.Entidad.Rows.Count > 0
    End Function
    Private Function VerificarCuentaTerceros(ByVal strNumeroCuenta As String, ByVal strCodigoTercero As String, ByVal strCodigoPortafolioSBS As String, ByVal strBanco As String, ByVal strLiqAut As String) As Boolean
        Dim oCuentaTercerosBM As New CuentaTercerosBM
        Dim LngContarCuentaTerceros As Long
        LngContarCuentaTerceros = oCuentaTercerosBM.VerificarCuentaTerceros(strNumeroCuenta, strCodigoTercero, strCodigoPortafolioSBS, strBanco, strLiqAut)
        If LngContarCuentaTerceros > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub Insertar()
        Dim oTercerosBM As New TercerosBM
        Dim oEntidadBM As New EntidadBM
        Dim oTercerosBE As New TercerosBE
        Dim oEntidadBE As New EntidadBE
        Dim oCuentasTecerosBE As New CuentaTercerosBE
        Dim oListaFondoCliente As ListFondoCliente
        oTercerosBE = Me.ObtenerInstanciaTercero()
        oCuentasTecerosBE = Me.ObtenerInstanciaDetalle()
        oEntidadBE = Me.ObtenerInstanciaEntidad()
        oListaFondoCliente = Me.ObtenerInstanciaListaFondoCliente()
        oTercerosBM.Insertar(oTercerosBE, oCuentasTecerosBE, oListaFondoCliente, DatosRequest)
        oEntidadBM.Insertar(oEntidadBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub
    Private Sub Modificar()
        Dim oTercerosBM As New TercerosBM
        Dim oEntidadBM As New EntidadBM
        Dim oTercerosBE As New TercerosBE
        Dim oEntidadBE As New EntidadBE
        Dim oCuentasTecerosBE As New CuentaTercerosBE
        Dim oListaFondoCliente As ListFondoCliente
        oTercerosBE = Me.ObtenerInstanciaTercero()
        oCuentasTecerosBE = Me.ObtenerInstanciaDetalle()
        oListaFondoCliente = Me.ObtenerInstanciaListaFondoCliente()
        oTercerosBM.Modificar(oTercerosBE, oCuentasTecerosBE, oListaFondoCliente, DatosRequest)
        '------------------------------------------------------------------------
        Dim ds As DataSet
        Dim codigoTercero As String = Me.txtCodigoTercero.Text.ToString()
        Dim codEntidad As String = String.Empty
        Dim blnExisteEntidad As Boolean
        ds = oEntidadBM.SeleccionarPorCodigoTercero(codigoTercero, Me.DatosRequest)
        oEntidadBE = Me.ObtenerInstanciaEntidad()
        If ds.Tables(0).Rows.Count > 0 Then
            If ds.Tables(0).Rows(0)(0).ToString() <> "" Then
                codEntidad = ds.Tables(0).Rows(0)(0).ToString()
                oEntidadBM.Modificar(oEntidadBE, txtSinonimo.Text.ToString().Trim(), DatosRequest)
            Else
                blnExisteEntidad = Me.ExisteEntidad_ID()
                If blnExisteEntidad Then
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_ID_EXISTE)
                    Exit Sub
                Else
                    oEntidadBM.Insertar(oEntidadBE, DatosRequest)
                End If
            End If
            txtCodigoEntidad.Enabled = False
        End If
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub
    Private Sub CargarControles(ByVal codigo As String)
        Dim oTercerosBM As New TercerosBM
        Dim oTerceros As New TercerosBE
        Dim oRow As TercerosBE.TercerosRow
        oTerceros = oTercerosBM.Seleccionar(codigo, DatosRequest)
        oRow = DirectCast(oTerceros.Terceros.Rows(0), TercerosBE.TercerosRow)
        Me.txtCodigoTercero.Enabled = False
        Me.hd.Value = oRow.CodigoTercero
        Me.txtCodigoTercero.Text = oRow.CodigoTercero
        Me.tbDescripcion.Text = oRow.Descripcion
        Me.tbDireccion.Text = oRow.Direccion
        Me.tbCodigoEmision.Text = oRow.CodigoEmision
        Try
            Me.ddlTipoDocumento.SelectedValue = oRow.CodigoTipoDocumento
        Catch ex As Exception
            Me.ddlTipoDocumento.SelectedIndex = 0
        End Try
        Try
            Me.ddlPais.SelectedValue = oRow.CodigoPais
        Catch ex As Exception
            Me.ddlPais.SelectedIndex = 0
        End Try
        Try
            Me.ddlTipoTercero.SelectedValue = oRow.TipoTercero
        Catch ex As Exception
            Me.ddlTipoTercero.SelectedIndex = 0
        End Try
        Try
            Me.ddlCodigoPostal.SelectedValue = oRow.CodigoPostal
        Catch ex As Exception
            Me.ddlCodigoPostal.SelectedIndex = 0
        End Try
        Try
            Me.ddlSectorEmpresarial.SelectedValue = oRow.CodigoSectorEmpresarial
        Catch ex As Exception
            Me.ddlSectorEmpresarial.SelectedIndex = 0
        End Try
        Try
            Me.cboRating.SelectedValue = oRow.Rating
        Catch ex As Exception
            Me.cboRating.SelectedIndex = 0
        End Try
        Try
            Me.ddlRatingInterno.SelectedValue = oRow.RatingInterno
        Catch ex As Exception
            Me.ddlRatingInterno.SelectedIndex = 0
        End Try
        Try
            Me.ddlRatingFF.SelectedValue = oRow.RatingFF
        Catch ex As Exception
            Me.ddlRatingFF.SelectedIndex = 0
        End Try
        Try
            Me.ddlBeneficiario.SelectedValue = oRow.Beneficiario
        Catch ex As Exception
            Me.ddlBeneficiario.SelectedIndex = 0
        End Try
        Me.ddlSituacion.SelectedValue = oRow.Situacion
        Try
            Me.ddlClasificacionTercero.SelectedValue = oRow.ClasificacionTercero
        Catch ex As Exception
            Me.ddlClasificacionTercero.SelectedIndex = 0
        End Try
        Try
            Me.ddlCustodio.SelectedValue = oRow.CodigoCustodio
        Catch ex As Exception
            Me.ddlCustodio.SelectedIndex = 0
        End Try
        Try
            tbCodigoDocumento.Text = oRow.CodigoDocumento
        Catch ex As Exception
            tbCodigoDocumento.Text = ""
        End Try
        If oRow.ClasificacionTercero.Equals(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO) Then
            If Not ddlCustodio.SelectedValue.Equals(oRow.CodigoCustodio) Then
                AlertaJS(ObtenerMensaje("ALERT145"))
            Else
                Me.ddlCustodio.SelectedValue = oRow.CodigoCustodio
            End If
            Me.lblCustodio.Visible = True
            Me.ddlCustodio.Visible = True
            Me.ddlPortafolio.Visible = True
            Me.dgLista.Columns(8).Visible = True
        Else
            Me.ddlCustodio.Visible = False
            Me.lblCustodio.Visible = False
            Me.ddlPortafolio.Visible = False
            Me.dgLista.Columns(8).Visible = False
        End If
        If oRow.IsSectorGIGSNull = True Then Me.ddlSectorGigs.SelectedValue = "" Else Me.ddlSectorGigs.SelectedValue = oRow.SectorGIGS
        CargaGrilla(codigo)
        Dim oEntidadBE As EntidadBE
        Dim oEntidadBM As New EntidadBM
        Dim oRowE As EntidadBE.EntidadRow
        Dim ds As DataSet
        Dim codEntidad As String = String.Empty
        ds = oEntidadBM.SeleccionarPorCodigoTercero(codigo, Me.DatosRequest)
        If ds.Tables(0).Rows.Count > 0 Then
            codEntidad = ds.Tables(0).Rows(0)(0).ToString()
            oEntidadBE = oEntidadBM.Seleccionar(codEntidad, Me.DatosRequest)
            If oEntidadBE.Tables(0).Rows.Count > 0 Then
                oRowE = oEntidadBE.Entidad(0)
                Me.txtCodigoEntidad.Enabled = False
                Me.txtCodigoEntidad.Text = codEntidad
                Me.txtCodigoSBS.Text = IIf(oRowE.IsCodigoSBSNull(), "", oRowE.CodigoSBS)
                Try
                    Me.txtSinonimo.Text = oRowE.Sinonimo
                Catch ex As Exception
                    Me.txtSinonimo.Text = ""
                End Try
                If Not (oRowE.IsUnidadesEmitidasNull()) Then
                    Me.tbUnidadesEmitidas.Text = Format(CType(oRowE.UnidadesEmitidas, Decimal), "##,##0.0000000")
                End If
                Try
                    Me.txtCodigoBroker.Text = oRowE.CodigoBroker
                Catch ex As Exception
                    Me.txtCodigoBroker.Text = ""
                End Try
                If oRowE.CodigoTipoEntidad = "" Then
                    Me.ddlTipoEntidad.SelectedIndex = 0
                Else
                    Me.ddlTipoEntidad.SelectedValue = oRowE.CodigoTipoEntidad
                End If
                If oRowE.CodigoPortafolio = "" Then
                    Me.ddlPortafolioENT.SelectedIndex = 0
                Else
                    Me.ddlPortafolioENT.SelectedValue = oRowE.CodigoPortafolio
                End If
                Try
                    Me.ddlGrupoEconomico.SelectedValue = IIf(oRowE.IsCodigoGrupoEconomicoNull(), "--Seleccione--", oRowE.CodigoGrupoEconomico)
                Catch ex As Exception
                    Me.ddlGrupoEconomico.SelectedIndex = 0
                End Try
                Try
                    Me.ddlMercado.SelectedValue = oRowE.CodigoMercado
                Catch ex As Exception
                    Me.ddlMercado.SelectedIndex = 0
                End Try
                Try
                    Me.ddlSituacion.SelectedValue = oRow.Situacion
                Catch ex As Exception
                    Me.ddlSituacion.SelectedIndex = 0
                End Try
                If Not (oRowE.IsEntidadAvalNull()) And oRowE.EntidadAval.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.AVAL).Selected = True
                End If
                If Not (oRowE.IsEntidadBrokerNull()) And oRowE.EntidadBroker.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.BORKER).Selected = True
                    Me.txtCodigoBroker.Enabled = True
                Else
                    Me.txtCodigoBroker.Text = ""
                    Me.txtCodigoBroker.Enabled = False
                End If
                If Not (oRowE.IsEntidadComisionistaNull()) And oRowE.EntidadComisionista.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.COMISIONISTA).Selected = True
                End If
                If Not (oRowE.IsEntidadCustodioNull()) And oRowE.EntidadCustodio.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.CUSTODIO).Selected = True
                End If
                If Not (oRowE.IsEntidadEmisoraNull()) And oRowE.EntidadEmisora.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.EMISOR).Selected = True
                    Me.tbFactorCastigo.Enabled = True
                    Me.tbFactorCastigo.ReadOnly = False
                    If Not oRowE.IsFactorCastigoNull() Then
                        Me.tbFactorCastigo.Text = oRowE.FactorCastigo
                    End If
                End If
                If Not (oRow.IsEntidadFinancieraNull()) And oRowE.EntidadFinanciera.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_FINANCIERA).Selected = True
                End If
                If Not (oRowE.IsEntidadReguladoraNull()) And oRowE.EntidadReguladora.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_REGULADORA).Selected = True
                End If
                If Not (oRowE.IsEntidadRiesgoceroNull()) And oRowE.EntidadRiesgocero.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.RIESGO_CERO).Selected = True
                End If
                If Not (oRowE.IsEntidadVigiladaNull()) And oRowE.EntidadVigilada.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_VIGILADA).Selected = True
                End If
                If Not (oRowE.IsEntidadVinculadaNull()) And oRowE.EntidadVinculada.Equals(Constantes.M_STR_CONDICIONAL_SI) Then
                    Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_VINCULADA).Selected = True
                End If
            End If
        End If
        CargarControlTercero_FondoCliente(codigo)
    End Sub
    Private Function ObtenerInstanciaTercero() As TercerosBE
        Dim oTercerosBE As New TercerosBE
        Dim oRow As TercerosBE.TercerosRow
        oRow = DirectCast(oTercerosBE.Terceros.NewTercerosRow(), TercerosBE.TercerosRow)
        oRow.CodigoTercero = Me.txtCodigoTercero.Text
        oRow.Descripcion = tbDescripcion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Direccion = Me.tbDireccion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.CodigoTipoDocumento = Me.ddlTipoDocumento.SelectedValue
        oRow.CodigoPais = Me.ddlPais.SelectedValue
        oRow.CodigoPostal = IIf(Me.ddlCodigoPostal.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), "", Me.ddlCodigoPostal.SelectedValue).ToString()
        oRow.CodigoSectorEmpresarial = ddlSectorEmpresarial.SelectedValue
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.ClasificacionTercero = Me.ddlClasificacionTercero.SelectedValue
        oRow.CodigoCustodio = IIf(Me.ddlCustodio.SelectedItem.Text.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlCustodio.SelectedValue).ToString()
        oRow.TipoTercero = Me.ddlTipoTercero.SelectedValue
        oRow.Beneficiario = IIf(Me.ddlBeneficiario.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), "", Me.ddlBeneficiario.SelectedValue).ToString()
        oRow.Rating = Me.cboRating.SelectedValue
        oRow.RatingInterno = Me.ddlRatingInterno.SelectedValue
        oRow.RatingFF = Me.ddlRatingFF.SelectedValue ' Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-05 | Rating

        oRow.CodigoEmision = Me.tbCodigoEmision.Text
        oTercerosBE.Terceros.AddTercerosRow(oRow)
        oTercerosBE.Terceros.AcceptChanges()
        oRow.CodigoDocumento = tbCodigoDocumento.Text
        oRow.SectorGIGS = ddlSectorGigs.SelectedValue
        Return oTercerosBE
    End Function
    Private Function ObtenerInstanciaEntidad() As EntidadBE
        Dim oEntidadBE As New EntidadBE
        Dim oRow As EntidadBE.EntidadRow
        oRow = oEntidadBE.Entidad.NewEntidadRow()
        oRow.CodigoEntidad = Me.txtCodigoEntidad.Text
        oRow.CodigoTercero = Me.txtCodigoTercero.Text
        oRow.CodigoSBS = Me.txtCodigoSBS.Text
        oRow.CodigoTipoEntidad = Me.ddlTipoEntidad.SelectedValue
        If (Me.ddlGrupoEconomico.SelectedValue = "--Seleccione--") Then
            oRow.CodigoGrupoEconomico = ""
        Else
            oRow.CodigoGrupoEconomico = Me.ddlGrupoEconomico.SelectedValue
        End If

        If (Me.ddlMercado.SelectedValue = "--Seleccione--") Then
            oRow.CodigoMercado = ""
        Else
            oRow.CodigoMercado = Me.ddlMercado.SelectedValue
        End If
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.EntidadEmisora = IIf(Me.chlClase.Items(CLASE_ENTIDAD.EMISOR).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadCustodio = IIf(Me.chlClase.Items(CLASE_ENTIDAD.CUSTODIO).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadVinculada = IIf(Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_VINCULADA).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadRiesgocero = IIf(Me.chlClase.Items(CLASE_ENTIDAD.RIESGO_CERO).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadVigilada = IIf(Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_VIGILADA).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadFinanciera = IIf(Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_FINANCIERA).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadBroker = IIf(Me.chlClase.Items(CLASE_ENTIDAD.BORKER).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadComisionista = IIf(Me.chlClase.Items(CLASE_ENTIDAD.COMISIONISTA).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadAval = IIf(Me.chlClase.Items(CLASE_ENTIDAD.AVAL).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.EntidadReguladora = IIf(Me.chlClase.Items(CLASE_ENTIDAD.ENTIDAD_REGULADORA).Selected, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO).ToString()
        oRow.FactorCastigo = IIf(Me.chlClase.Items(CLASE_ENTIDAD.EMISOR).Selected, Me.tbFactorCastigo.Text, -1)
        oRow.Sinonimo = Me.txtSinonimo.Text
        oRow.CodigoBroker = Me.txtCodigoBroker.Text
        If (Me.ddlPortafolioENT.SelectedValue = "--Seleccione--") Then
            oRow.CodigoPortafolio = ""
        Else
            oRow.CodigoPortafolio = Me.ddlPortafolioENT.SelectedValue
        End If
        If Microsoft.VisualBasic.IsNumeric(tbUnidadesEmitidas.Text.Trim) Then
            oRow.UnidadesEmitidas = Me.tbUnidadesEmitidas.Text.Trim.Replace(".", UIUtility.DecimalSeparator)
        Else
            oRow.UnidadesEmitidas = Nothing
        End If
        oEntidadBE.Entidad.AddEntidadRow(oRow)
        oEntidadBE.Entidad.AcceptChanges()
        Return oEntidadBE
    End Function
    Private Function ObtenerInstanciaDetalle() As CuentaTercerosBE
        Dim oCuentaTercerosBE As New CuentaTercerosBE
        Dim oRow As CuentaTercerosBE.CuentaTercerosRow
        If Not VistaDetalleCuentas Is Nothing Then
            Dim dtCuentasTercero As New DataSet
            Dim i As Integer
            dtCuentasTercero = VistaDetalleCuentas
            If Not dtCuentasTercero Is Nothing Then
                For i = 0 To dtCuentasTercero.Tables(0).Rows.Count - 1
                    oRow = DirectCast(oCuentaTercerosBE.CuentaTerceros.NewCuentaTercerosRow(), CuentaTercerosBE.CuentaTercerosRow)
                    oRow.Item("Secuencial") = 0
                    oRow.Item("NumeroCuenta") = dtCuentasTercero.Tables(0).Rows(i)("NumeroCuenta").ToString()
                    oRow.Item("CodigoMoneda") = IIf(dtCuentasTercero.Tables(0).Rows(i)("CodigoMoneda").ToString().Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, dtCuentasTercero.Tables(0).Rows(i)("CodigoMoneda").ToString())
                    oRow.Item("CodigoClaseCuenta") = ""
                    oRow.Item("CuentaInterBancario") = dtCuentasTercero.Tables(0).Rows(i)("CuentaInterBancario").ToString()
                    oRow.Item("CodigoMercado") = IIf(dtCuentasTercero.Tables(0).Rows(i)("CodigoMercado").ToString().Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, dtCuentasTercero.Tables(0).Rows(i)("CodigoMercado").ToString())
                    oRow.Item("EntidadFinanciera") = IIf(dtCuentasTercero.Tables(0).Rows(i)("EntidadFinanciera").ToString().Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, dtCuentasTercero.Tables(0).Rows(i)("EntidadFinanciera").ToString())
                    oRow.Item("Situacion") = dtCuentasTercero.Tables(0).Rows(i)("NombreSituacion").ToString()
                    oRow.Item("CodigoPortafolioSBS") = IIf(dtCuentasTercero.Tables(0).Rows(i)("CodigoPortafolioSBS").ToString().Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, dtCuentasTercero.Tables(0).Rows(i)("CodigoPortafolioSBS").ToString())
                    oRow.Item("CodigoTercero") = dtCuentasTercero.Tables(0).Rows(i)("CodigoTercero").ToString()
                    oRow.Item("LiquidacionAutomatica") = dtCuentasTercero.Tables(0).Rows(i)("LiquidacionAutomatica").ToString()
                    oCuentaTercerosBE.CuentaTerceros.AddCuentaTercerosRow(oRow)
                    oCuentaTercerosBE.CuentaTerceros.AcceptChanges()
                Next
            End If
        End If
        Return oCuentaTercerosBE
    End Function
    Private Sub CargarCombos()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim tablaSituacion As New DataTable
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        Dim tablaTipoDocumento As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaTipoDocumento = oTipoDocumento.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoDocumento, tablaTipoDocumento, "CodigoTipoDocumento", "Descripcion", True)
        Dim tablaPais As New Data.DataTable
        Dim oPais As New PaisBM
        tablaPais = oPais.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlPais, tablaPais, "CodigoPais", "Descripcion", True)
        Dim tablaCodigoPostal As New Data.DataTable
        Dim oCodigoPostal As New CodigoPostalBM
        tablaCodigoPostal = oCodigoPostal.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlCodigoPostal, tablaCodigoPostal, "CodigoPostal", "Descripcion", True)
        Dim tablaSectorEmpresarial As New Data.DataTable
        Dim oSectorEmpresarial As New SectorEmpresarialBM
        tablaSectorEmpresarial = oSectorEmpresarial.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlSectorEmpresarial, tablaSectorEmpresarial, "CodigoSectorEmpresarial", "Descripcion", True)
        Dim dtTipoTercero As DataTable
        Dim oTipoTerceroBM As New TipoTerceroBM
        dtTipoTercero = oTipoTerceroBM.Listar(Me.DatosRequest).TipoTercero
        HelpCombo.LlenarComboBox(Me.ddlTipoTercero, dtTipoTercero, "CodigoTipoTercero", "Descripcion", True)
        Dim dtParametrosGenerales As DataTable
        dtParametrosGenerales = oParametrosGenerales.Listar("CTercero", Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlClasificacionTercero, dtParametrosGenerales, "Valor", "Nombre", True)
        Dim oCustodioBM As New CustodioBM
        Dim dtCustodio As DataTable
        dtCustodio = oCustodioBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlCustodio, dtCustodio, "CodigoCustodio", "Descripcion", False)
        Dim dtBanco As DataTable
        Dim oEntidadBM As New EntidadBM
        dtBanco = oEntidadBM.ListarEntidadFinanciera(Me.DatosRequest).Entidad
        HelpCombo.LlenarComboBox(Me.ddlBanco, dtBanco, "CodigoEntidad", "NombreCompleto", True)
        Dim dtBeneficiario As DataTable
        Dim oContactoBM As New ContactoBM
        dtBeneficiario = oContactoBM.ListarActivos(Me.DatosRequest).Contacto
        HelpCombo.LlenarComboBox(Me.ddlBeneficiario, dtBeneficiario, "CodigoContacto", "Descripcion", True)
        Dim dtMercado As DataTable
        Dim oMercadoBM As New MercadoBM
        dtMercado = oMercadoBM.ListarActivos(Me.DatosRequest).Mercado
        HelpCombo.LlenarComboBox(Me.ddlMercadoTercero, dtMercado, "CodigoMercado", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlMercado, dtMercado, "CodigoMercado", "Descripcion", True)
        Dim dtPortafolio As DataTable = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlPortafolioENT, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
        Dim dtMoneda As DataTable
        Dim oMonedaBM As New MonedaBM
        dtMoneda = oMonedaBM.Listar(DatosRequest).Moneda
        HelpCombo.LlenarComboBox(Me.ddlMonedaTercero, dtMoneda, "CodigoMoneda", "CodigoMoneda", True)
        Dim tablaSituacion1 As New DataTable
        Dim oParametrosGenerales1 As New ParametrosGeneralesBM
        tablaSituacion1 = oParametrosGenerales1.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacionDet, tablaSituacion1, "Valor", "Nombre", False)
        Dim dtTipoEntidad As DataTable
        Dim oTipoEntidadBM As New TipoEntidadBM
        dtTipoEntidad = oTipoEntidadBM.Listar(Me.DatosRequest).TipoEntidad
        HelpCombo.LlenarComboBox(Me.ddlTipoEntidad, dtTipoEntidad, "CodigoTipoEntidad", "Descripcion", True)

        Dim dtGrupoEconomico As DataTable
        Dim oGrupoEconomicoBM As New GrupoEconomicoBM
        dtGrupoEconomico = oGrupoEconomicoBM.Listar(Me.DatosRequest).GrupoEconomico
        HelpCombo.LlenarComboBox(Me.ddlGrupoEconomico, dtGrupoEconomico, "CodigoGrupoEconomico", "Descripcion", True)

        '' FIN MPENAL - 21/09/16
        Dim dtFondoVinculado As DataTable = New PortafolioBM().PortafolioListar("", "", "A", "S")
        HelpCombo.LlenarListBox(lbFondoVinculado, dtFondoVinculado, "CodPorTerCli", "DescripcionTerceroCliente", False)
        'INI MPENAL - 21/09/16

        ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-23 | Carga Rating
        Dim oRating As New ParametrosGeneralesBM
        'Obtenemos la tabla completa Rating 
        Dim dtRating As DataTable = oParametrosGenerales.Listar("RATING", DatosRequest)

        Dim rowRating() As DataRow = dtRating.Select("Comentario in ('DLP_EXT','TODOS')")
        HelpCombo.LlenarComboBox(Me.cboRating, rowRating.CopyToDataTable(), "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(Me.ddlRatingInterno, rowRating.CopyToDataTable(), "Valor", "Nombre", False)

        rowRating = dtRating.Select("Comentario in ('ESF_LOC','TODOS')")
        HelpCombo.LlenarComboBox(Me.ddlRatingFF, rowRating.CopyToDataTable(), "Valor", "Nombre", False)
        ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-23 | Carga Rating
        Dim dt As DataTable = Nothing
        dt = oParametrosGenerales.SeleccionarPorFiltro("SECTOR_GIGS", "", "", "", DatosRequest)
        HelpCombo.LlenarComboBox(ddlSectorGigs, dt, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarCampos()
        Me.txtCodigoTercero.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbDireccion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlCodigoPostal.SelectedIndex = 0
        Me.ddlPais.SelectedIndex = 0
        Me.ddlSectorEmpresarial.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
        Me.ddlTipoDocumento.SelectedIndex = 0
        Me.ddlBeneficiario.SelectedIndex = 0
        Me.ddlTipoTercero.SelectedIndex = 0
        Me.ddlClasificacionTercero.SelectedIndex = 0
        Me.tbCodigoEmision.Text = ""
        tbCodigoDocumento.Text = Constantes.M_STR_TEXTO_INICIAL
        LimpiarControlesDetalle()
        LimpiarRegistrosEntidad()
        LimpiarFondosVinculados()
    End Sub
    Private Sub LimpiarRegistrosEntidad()
        Me.txtCodigoEntidad.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtCodigoSBS.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlTipoEntidad.SelectedIndex = 0
        Me.ddlGrupoEconomico.SelectedIndex = 0
        Me.ddlMercado.SelectedIndex = 0
        Me.txtSinonimo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlPortafolioENT.SelectedIndex = 0
        Me.tbUnidadesEmitidas.Text = Constantes.M_STR_TEXTO_INICIAL
        For i As Int32 = 0 To chlClase.Items.Count - 1
            Me.chlClase.Items(i).Selected = False
        Next
    End Sub
    Private Sub CambiarEstadoCustodio()
        If Me.ddlClasificacionTercero.SelectedValue.Equals(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO) Then
            Me.ddlCustodio.Visible = True
            Me.lblCustodio.Visible = True
            Me.ddlPortafolio.SelectedIndex = 0
            Me.ddlPortafolio.Visible = True
            Me.dgLista.Columns(8).Visible = True
        Else
            Me.ddlCustodio.Visible = False
            Me.lblCustodio.Visible = False
            Me.ddlPortafolio.SelectedValue = ParametrosSIT.PORTAFOLIO_MULTIFONDOS
            Me.ddlPortafolio.Visible = False
            Me.dgLista.Columns(8).Visible = False
        End If
    End Sub
    Private Sub CargaGrilla(ByVal codigo As String)
        Dim oCuentaTercerosBM As New CuentaTercerosBM
        Dim dtCustodio As DataSet
        dtCustodio = oCuentaTercerosBM.SeleccionarCuentaTerceros(codigo, "")
        Me.VistaDetalleCuentas = dtCustodio
        Me.dgLista.DataSource = Me.VistaDetalleCuentas
        Me.dgLista.DataBind()
        Me.ibnModificarDetalle.Visible = False
    End Sub
    Private Sub EditarGrilla(ByVal index As Int16)
        Dim oCuentasTerceros As DataTable
        oCuentasTerceros = Me.VistaDetalleCuentas.Tables(0)
        Me.ibnModificarDetalle.Enabled = True
        If oCuentasTerceros.Rows(index)("EntidadFinanciera").ToString() = String.Empty Then
            Me.ddlBanco.SelectedIndex = 0
        Else
            If (Me.ddlBanco.Items.FindByValue(oCuentasTerceros.Rows(index)("EntidadFinanciera").ToString()) Is Nothing) Then
                Me.ddlBanco.SelectedIndex = 0
            Else
                Me.ddlBanco.SelectedValue = oCuentasTerceros.Rows(index)("EntidadFinanciera").ToString()
            End If
        End If
        Session("Secuencial_CuentaT") = oCuentasTerceros.Rows(index)("Secuencial").ToString()
        txtNumeroCuenta.Text = oCuentasTerceros.Rows(index)("NumeroCuenta").ToString()
        txtCuentaInterBancario.Text = oCuentasTerceros.Rows(index)("CuentaInterBancario").ToString()
        Try
            Me.ddlMercadoTercero.SelectedValue = oCuentasTerceros.Rows(index)("CodigoMercado").ToString()
        Catch ex As Exception
            Me.ddlMercadoTercero.SelectedIndex = 0
        End Try

        If oCuentasTerceros.Rows(index)("CodigoPortafolioSBS").ToString() = "" Then
            Me.ddlPortafolio.SelectedIndex = 0
        Else
            Me.ddlPortafolio.SelectedValue = oCuentasTerceros.Rows(index)("CodigoPortafolioSBS").ToString()
        End If
        Try
            Me.ddlMonedaTercero.SelectedValue = oCuentasTerceros.Rows(index)("CodigoMoneda").ToString()
        Catch ex As Exception
            Me.ddlMonedaTercero.SelectedIndex = 0
        End Try

        If oCuentasTerceros.Rows(index)("Situacion").ToString() = "" Then
            Me.ddlSituacionDet.SelectedIndex = 0
        Else
            Me.ddlSituacionDet.SelectedValue = oCuentasTerceros.Rows(index)("Situacion").ToString()
        End If
        DesHabilitarControlesDetalle()
        ViewState("IndiceSeleccionado") = index
        Me.ibnModificarDetalle.Visible = True
        Me.ibnAgregarDetalle.Visible = False
    End Sub
    Private Sub Agregar()
        Dim bolExiste As Boolean = False
        If ibnAgregarDetalle.Text = "Agregar" Then
            If cbLiqAutomatica.Checked Then
                bolExiste = VerificarCuentaTercerosLiqAutomatica(ddlBanco.SelectedValue, txtCodigoTercero.Text, ddlMonedaTercero.SelectedValue, "S")
                If bolExiste Then
                    AlertaJS("Ya existe la cuenta con el banco " & ddlBanco.SelectedItem.Text & " y moneda " & ddlMonedaTercero.SelectedItem.Text)
                    Exit Sub
                End If
                InsertarFilaCuentas()
                LimpiarControlesDetalle()
                'HabilitarControlesDetalle()
            Else
                If Me.txtNumeroCuenta.Text <> String.Empty Then
                    If ddlPortafolio.SelectedValue <> String.Empty Then
                        If txtCodigoTercero.Text <> String.Empty Then
                            If Not VerificarCuentaTerceros(txtNumeroCuenta.Text, txtCodigoTercero.Text, Me.ddlPortafolio.SelectedValue, ddlBanco.SelectedValue, "N") Then
                                AlertaJS("Ya existe la cuenta")
                                Exit Sub
                            End If
                            InsertarFilaCuentas()
                            LimpiarControlesDetalle()
                        Else
                            AlertaJS("Ingresar código de tercero")
                            Exit Sub
                        End If
                    Else
                        AlertaJS("Seleccionar un portafolio")
                        Exit Sub
                    End If
                Else
                    AlertaJS("Ingresar número de cuenta")
                    Exit Sub
                End If
            End If
        Else
            If cbLiqAutomatica.Checked Then
                'bolExiste = VerificarCuentaTercerosLiqAutomatica(ddlBanco.SelectedValue, txtCodigoTercero.Text, ddlMonedaTercero.SelectedValue, "S")
                'If bolExiste Then
                '    AlertaJS("Ya existe la cuenta con el banco " & ddlBanco.SelectedItem.Text & " y moneda " & ddlMonedaTercero.SelectedItem.Text)
                '    Exit Sub
                'End If
                ModificarFilaCuentas()
                LimpiarControlesDetalle()
                'HabilitarControlesDetalle()
                ibnAgregarDetalle.Text = "Agregar"
            Else
                If Me.txtNumeroCuenta.Text <> String.Empty Then
                    If ddlPortafolio.SelectedValue <> String.Empty Then
                        If txtCodigoTercero.Text <> String.Empty Then
                            'If Not VerificarCuentaTerceros(txtNumeroCuenta.Text, txtCodigoTercero.Text, Me.ddlPortafolio.SelectedValue, ddlBanco.SelectedValue) Then
                            '    AlertaJS("Ya existe la cuenta")
                            '    Exit Sub
                            'End If
                            ModificarFilaCuentas()
                            LimpiarControlesDetalle()
                            ibnAgregarDetalle.Text = "Agregar"
                        Else
                            AlertaJS("Ingresar código de tercero")
                        End If
                    Else
                        AlertaJS("Seleccionar un portafolio")
                    End If
                Else
                    AlertaJS("Ingresar número de cuenta")
                End If
            End If
        End If
    End Sub
    Private Sub InsertarFilaCuentas()
        Dim oCuentas As DataSet
        Dim oRow As DataRow
        oCuentas = Me.VistaDetalleCuentas
        If (Me.ddlPortafolio.SelectedValue = ParametrosSIT.PORTAFOLIO_MULTIFONDOS) Then
            oRow = oCuentas.Tables(0).NewRow()
            oRow.Item("CodigoTercero") = txtCodigoTercero.Text
            oRow.Item("EntidadFinanciera") = IIf(Me.ddlBanco.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlBanco.SelectedValue)
            oRow.Item("DesBanco") = IIf(Me.ddlBanco.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlBanco.SelectedValue)
            If cbLiqAutomatica.Checked Then
                oRow.Item("NumeroCuenta") = "Cta. del Fondo"
                oRow.Item("CuentaInterBancario") = "Cta. del Fondo"
            Else
                oRow.Item("NumeroCuenta") = txtNumeroCuenta.Text
                oRow.Item("CuentaInterBancario") = txtCuentaInterBancario.Text
            End If
            oRow.Item("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
            oRow.Item("DesPortafolio") = IIf(ddlPortafolio.SelectedValue = "", String.Empty, ddlPortafolio.SelectedItem.Text)
            oRow.Item("CodigoMercado") = IIf(Me.ddlMercadoTercero.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlMercadoTercero.SelectedValue)
            'oRow.Item("DesMercado") = IIf(Me.ddlMercadoTercero.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlMercadoTercero.SelectedItem.Text)
            oRow.Item("DesMercado") = IIf(Me.ddlMercadoTercero.SelectedValue = "", String.Empty, Me.ddlMercadoTercero.SelectedItem.Text)
            oRow.Item("CodigoMoneda") = IIf(Me.ddlMonedaTercero.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlMonedaTercero.SelectedValue)
            oRow.Item("DesMoneda") = IIf(Me.ddlMonedaTercero.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, Me.ddlMonedaTercero.SelectedItem.Text)
            oRow.Item("NombreSituacion") = IIf(Me.ddlSituacionDet.SelectedValue.Equals("A"), "Activo", "Inactivo")
            oRow.Item("Situacion") = Me.ddlSituacionDet.SelectedValue
            oRow.Item("LiquidacionAutomatica") = IIf(cbLiqAutomatica.Checked = True, "S", "N").ToString()
            oCuentas.Tables(0).Rows.Add(oRow)
        Else
            oRow = oCuentas.Tables(0).NewRow()
            oRow.Item("CodigoTercero") = txtCodigoTercero.Text
            oRow.Item("EntidadFinanciera") = IIf(Me.ddlBanco.SelectedIndex = 0, String.Empty, Me.ddlBanco.SelectedValue)
            oRow.Item("DesBanco") = IIf(Me.ddlBanco.SelectedIndex = 0, String.Empty, Me.ddlBanco.SelectedValue)
            If cbLiqAutomatica.Checked Then
                oRow.Item("NumeroCuenta") = "Cta. del Fondo"
                oRow.Item("CuentaInterBancario") = "Cta. del Fondo"
            Else
                oRow.Item("NumeroCuenta") = txtNumeroCuenta.Text
                oRow.Item("CuentaInterBancario") = txtCuentaInterBancario.Text
            End If
            oRow.Item("CodigoPortafolioSBS") = IIf(Me.ddlPortafolio.SelectedIndex = 0, String.Empty, Me.ddlPortafolio.SelectedValue)
            oRow.Item("DesPortafolio") = IIf(Me.ddlPortafolio.SelectedIndex = 0, String.Empty, Me.ddlPortafolio.SelectedItem.Text)
            oRow.Item("CodigoMercado") = IIf(Me.ddlMercadoTercero.SelectedIndex = 0, String.Empty, Me.ddlMercadoTercero.SelectedValue)
            oRow.Item("DesMercado") = IIf(Me.ddlMercadoTercero.SelectedIndex = 0, String.Empty, Me.ddlMercadoTercero.SelectedItem.Text)
            oRow.Item("CodigoMoneda") = IIf(Me.ddlMonedaTercero.SelectedIndex = 0, String.Empty, Me.ddlMonedaTercero.SelectedValue)
            oRow.Item("DesMoneda") = IIf(Me.ddlMonedaTercero.SelectedIndex = 0, String.Empty, Me.ddlMonedaTercero.SelectedItem.Text)
            oRow.Item("NombreSituacion") = IIf(Me.ddlSituacionDet.SelectedValue.Equals("A"), "Activo", "Inactivo")
            oRow.Item("Situacion") = Me.ddlSituacionDet.SelectedValue
            oRow.Item("LiquidacionAutomatica") = IIf(cbLiqAutomatica.Checked = True, "S", "N").ToString()
            oCuentas.Tables(0).Rows.Add(oRow)
        End If
        Me.VistaDetalleCuentas = oCuentas
        Me.dgLista.DataSource = Me.VistaDetalleCuentas
        Me.dgLista.DataBind()
    End Sub
    Private Sub ModificarDetalle()
        ModificarFilaCuentas()
        LimpiarControlesDetalle()
        HabilitarControlesDetalle()
        Me.ibnModificarDetalle.Visible = False
        Me.ibnAgregarDetalle.Visible = True
        If Me.hd.Value.Equals(String.Empty) Then
            Me.ibnAgregarDetalle.Visible = True
        End If
    End Sub
    Private Sub ModificarFilaCuentas()
        Dim intIndice As Integer
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        'intIndice = Integer.Parse(hdSecuencial.Value)
        Dim oCuentas As DataSet
        Dim oRow As DataRow
        oCuentas = Me.VistaDetalleCuentas
        oRow = oCuentas.Tables(0).Rows(intIndice)
        oRow.BeginEdit()
        oRow.Item("Secuencial") = IIf(Session("Secuencial_CuentaT") = "", 0, Session("Secuencial_CuentaT"))
        oRow.Item("CodigoTercero") = txtCodigoTercero.Text
        oRow.Item("EntidadFinanciera") = IIf(Me.ddlBanco.SelectedIndex = 0, String.Empty, Me.ddlBanco.SelectedValue)
        oRow.Item("DesBanco") = IIf(Me.ddlBanco.SelectedIndex = 0, String.Empty, Me.ddlBanco.SelectedItem)
        oRow.Item("NumeroCuenta") = txtNumeroCuenta.Text
        oRow.Item("CuentaInterBancario") = txtCuentaInterBancario.Text
        oRow.Item("CodigoPortafolioSBS") = IIf(Me.ddlPortafolio.SelectedIndex = 0, String.Empty, Me.ddlPortafolio.SelectedValue)
        oRow.Item("DesPortafolio") = IIf(Me.ddlPortafolio.SelectedIndex = 0, String.Empty, Me.ddlPortafolio.SelectedItem.Text)
        oRow.Item("CodigoMercado") = IIf(Me.ddlMercadoTercero.SelectedIndex = 0, String.Empty, Me.ddlMercadoTercero.SelectedValue)
        oRow.Item("DesMercado") = IIf(Me.ddlMercadoTercero.SelectedIndex = 0, String.Empty, Me.ddlMercadoTercero.SelectedItem.Text)
        oRow.Item("CodigoMoneda") = IIf(Me.ddlMonedaTercero.SelectedIndex = 0, String.Empty, Me.ddlMonedaTercero.SelectedValue)
        oRow.Item("DesMoneda") = IIf(Me.ddlMonedaTercero.SelectedIndex = 0, String.Empty, Me.ddlMonedaTercero.SelectedItem.Text)
        oRow.Item("Situacion") = Me.ddlSituacionDet.SelectedValue
        oRow.Item("NombreSituacion") = IIf(Me.ddlSituacionDet.SelectedValue.Equals("A"), "Activo", "Inactivo")
        oRow.Item("LiquidacionAutomatica") = IIf(cbLiqAutomatica.Checked = True, "S", "N").ToString()
        oRow.EndEdit()
        Me.VistaDetalleCuentas = oCuentas
        Me.dgLista.DataSource = Me.VistaDetalleCuentas
        Me.dgLista.DataBind()
    End Sub
    Private Sub EliminarFilaCuenta()
        Dim intIndice As Integer
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        'intIndice = Integer.Parse(hdSecuencial.Value)
        Dim oCuentas As DataSet
        Dim oRow As DataRow
        oCuentas = Me.VistaDetalleCuentas
        oRow = oCuentas.Tables(0).Rows(intIndice)
        oCuentas.Tables(0).Rows.Remove(oRow)
        Me.VistaDetalleCuentas = oCuentas
        Me.dgLista.DataSource = Me.VistaDetalleCuentas
        Me.dgLista.DataBind()
    End Sub
    Private Sub Eliminar(ByVal secuencial As Integer)
        Dim oCuentaTercerosBM As New CuentaTercerosBM
        oCuentaTercerosBM.Eliminar(secuencial)
        CargaGrilla(txtCodigoTercero.Text)
    End Sub
    Private Sub HabilitarControlesDetalle()
        ddlPortafolio.Enabled = True
    End Sub
    Private Sub DesHabilitarControlesDetalle()
        ddlPortafolio.Enabled = False
    End Sub
    Private Sub LimpiarControlesDetalle()
        Me.ddlBanco.SelectedIndex = 0
        txtNumeroCuenta.Text = ""
        txtCuentaInterBancario.Text = ""
        Me.ddlPortafolio.SelectedIndex = 0
        Me.ddlMercadoTercero.SelectedIndex = 0
        Me.ddlMonedaTercero.SelectedIndex = 0
        Me.ddlSituacionDet.SelectedIndex = 0
        Me.cbLiqAutomatica.Checked = False
        txtNumeroCuenta.Enabled = True
        txtCuentaInterBancario.Enabled = True
        ddlPortafolio.Enabled = True
        ddlMercadoTercero.Enabled = True
        dgLista.DataSource = Nothing
    End Sub
    Private Sub ActualizarIndiceGrilla(ByVal nuevoIndice As Integer)
        If Not Request.QueryString("cod") Is Nothing Then
            CargaGrilla(hd.Value)
        Else
            hd.Value = String.Empty
        End If
    End Sub
    Private Function ExisteCodigoSBS(ByVal CodigoEntidad As String, ByVal CodigoSBS As String, ByVal FlagIngreso As String) As Boolean
        If CodigoSBS.Trim.Equals("") Then
            Return False
        End If
        Dim oEntidadBE As EntidadBE
        Dim oEntidadBM As New EntidadBM
        oEntidadBE = oEntidadBM.ExisteCodigoSBS(CodigoEntidad, CodigoSBS, FlagIngreso, DatosRequest)
        Return oEntidadBE.Entidad.Rows.Count > 0
    End Function
    Private Function VerificarCuentaTercerosLiqAutomatica(ByVal banco As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, ByVal liqAutomatica As String) As Boolean
        VerificarCuentaTercerosLiqAutomatica = False
        Dim oCuentaTercerosBM As New CuentaTercerosBM
        Dim dt As DataTable
        dt = oCuentaTercerosBM.Seleccionar(banco, txtCodigoTercero.Text, codigoMoneda, liqAutomatica)
        If (dt IsNot Nothing) Then
            If dt.Rows.Count > 0 Then
                VerificarCuentaTercerosLiqAutomatica = True
            End If
        End If
    End Function
    Private Sub ModificarCuentaTercero(ByVal secuencial As Integer)
        Dim oCuentasTecerosBE As New CuentaTercerosBE
        Dim oCuentaTercerosBM As New CuentaTercerosBM
        Dim oRow As CuentaTercerosBE.CuentaTercerosRow
        oRow = DirectCast(oCuentasTecerosBE.CuentaTerceros.NewCuentaTercerosRow(), CuentaTercerosBE.CuentaTercerosRow)
        oRow.Secuencial = secuencial
        oRow.EntidadFinanciera = ddlBanco.SelectedValue
        oRow.LiquidacionAutomatica = IIf(cbLiqAutomatica.Checked = True, "S", "N").ToString()
        oRow.NumeroCuenta = txtNumeroCuenta.Text.Trim
        oRow.CuentaInterBancario = txtCuentaInterBancario.Text.Trim
        oRow.CodigoMercado = ddlMercadoTercero.SelectedValue
        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        oRow.CodigoMoneda = ddlMonedaTercero.SelectedValue
        oRow.Situacion = ddlSituacionDet.SelectedValue
        oRow.CodigoTercero = txtCodigoTercero.Text
        oCuentasTecerosBE.CuentaTerceros.AddCuentaTercerosRow(oRow)
        oCuentasTecerosBE.CuentaTerceros.AcceptChanges()
        oCuentaTercerosBM.Modificar(oCuentasTecerosBE, DatosRequest)
        CargaGrilla(txtCodigoTercero.Text)
        LimpiarControlesDetalle()
    End Sub
    Private Function ObtenerInstanciaListaFondoCliente() As ListFondoCliente
        Dim objLFC As Terceros_FondoClienteBE
        Dim objListFCAux As List(Of Terceros_FondoClienteBE) = New List(Of Terceros_FondoClienteBE)
        Dim objListFC As New ListFondoCliente
        Dim codTerPor() As String = Nothing
        For Each item As ListItem In lbFondoVinculado.Items
            If item.Selected Then
                objLFC = New Terceros_FondoClienteBE
                objLFC.CodigoTercero = txtCodigoTercero.Text.Trim
                codTerPor = item.Value.Split(",")
                'objLFC.CodigoPortafolio = item.Value
                objLFC.CodigoPortafolio = codTerPor(0)
                objLFC.CodigoTerceroCliente = codTerPor(1)
                objListFCAux.Add(objLFC)
            End If
        Next
        objListFC.objListFondoCliente = objListFCAux
        Return objListFC
    End Function
    Private Sub LimpiarFondosVinculados()
        chkFondoVinculado.Checked = False
        For Each item As ListItem In lbFondoVinculado.Items
            If item.Selected Then
                item.Selected = False
            End If
        Next
    End Sub
    Private Sub CargarControlTercero_FondoCliente(ByVal p_CodigoTercero As String)
        Dim oTerceroBM As New TercerosBM
        Dim objTerceroFC As New Terceros_FondoClienteBE
        objTerceroFC.CodigoTercero = p_CodigoTercero
        objTerceroFC.CodigoPortafolio = ""
        objTerceroFC.Id = 0
        Dim dtFondoCliente As DataTable = oTerceroBM.SeleccionarTercero_FondoCliente(objTerceroFC)
        If dtFondoCliente IsNot Nothing Then
            If dtFondoCliente.Rows.Count > 0 Then
                chkFondoVinculado.Checked = True
                For Each item As ListItem In lbFondoVinculado.Items
                    For Each dtRow As DataRow In dtFondoCliente.Rows
                        If (item.Value = dtRow("CodigoPortafolio")) Then
                            item.Selected = True
                            Exit For
                        End If
                    Next
                Next
            End If
        End If
    End Sub
#End Region

End Class