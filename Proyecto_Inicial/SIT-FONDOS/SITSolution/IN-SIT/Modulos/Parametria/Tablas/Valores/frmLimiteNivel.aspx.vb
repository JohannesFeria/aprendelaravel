Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Collections
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Parametria_Tablas_Valores_frmLimiteNivel
    Inherits BasePage
#Region " Código generado por el Diseñador de Web Forms "
    Protected DtDetalle As New DataTable
    Protected ListaCaracteristicas As New Hashtable
    Protected ListaDetalleNivelLimite As New Hashtable
    Protected WithEvents divPorcentajeValores As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents TD1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents TD2 As System.Web.UI.HtmlControls.HtmlTableCell
    Private designerPlaceholderDeclaration As System.Object
#End Region
#Region " /* Declaración de Variables */ "
    Private campos() As String = {"CodigoNivelLimite", "CodigoLimiteCaracteristica", "CodigoCaracteristica", "Secuencial", "DescripcionCaracteristica", "FlagTipoPorcentaje", "ValorPorcentaje", "Situacion", "ValorPorcentajeM", "TieneLimiteNivel", "LimiteNivelMin", "LimiteNivelMax", "TieneLimiteFijoEnDetalle", "LimiteFijoEnDetalleMin", "LimiteFijoEnDetalleMax"}   'HDG OT 65023 20120522
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String", "System.Decimal", "System.String", "System.Decimal", "System.String", "System.Decimal", "System.Decimal", "System.String", "System.Decimal", "System.Decimal"}   'HDG OT 65023 20120522
    Dim oLimiteBM As New LimiteBM
    Dim oGrupoInstrumentoBM As New GrupoInstrumentoBM
    Private camposDetalleNivelLimite() As String = {"Selected", "CodigoNivelLimite", "CodigoCaracteristica", "NombreVista", "ClaseNormativa", "ValorCaracteristica", "DescripcionValorCaracteristica", "ValorPorcentaje", "Situacion", "ValorPorcentajeM", "ValorEspecifico", "CodigoCaracteristicaRelacion", "CodigoRelacion", "CodigoClaseActivoLimite"}  'GT 20181113
    Private tiposDetalleNivelLimite() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String", "System.String", "System.Decimal", "System.String", "System.Decimal", "System.String", "System.String", "System.String", "System.String"}    'GT 20181113
    Private CodigoGrupoInstrumento As String
    Private DescripcionGrupoInstrumento As String
    Dim CodigoLimite As String
    Dim claseLimite As String
    Dim TopeLimite As String
    Dim vCodigoCaracteristica As String
    Private CamposCondicion() As String = {"CodigoNivelLimite", "Secuencial", "Condicion", "PorcentajeMin", "PorcentajeMax"}   'GT 16/11/2018
    Private TipoCondicion() As String = {"System.String", "System.String", "System.String", "System.Decimal", "System.Decimal"}   'GT 16/11/2018

#End Region
#Region "/*Metodos Pagina*/"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CodigoLimite = Request.QueryString("CodigoLimite")
            CodigoGrupoInstrumento = Request.QueryString("codGrupo")
            claseLimite = Request.QueryString("claseLimite")
            TopeLimite = Request.QueryString("TopeLimite")
            If Not Page.IsPostBack Then
                btnModificar.Visible = False
                Dim oParametrosGenerales As New ParametrosGeneralesBM
                Dim CodigoPortafolio As String = ""
                Dim oPortafolioBM As New PortafolioBM
                Dim dt As DataTable = oPortafolioBM.PortafolioCodigoListar("", Constantes.M_STR_CONDICIONAL_NO)
                HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
                If CodigoLimite = "" Then
                    Session("filaseleccionada") = ddlPortafolio.SelectedValue
                Else
                    ddlPortafolio.SelectedValue = Session("filaseleccionada")
                End If
                ViewState("CaracteristicaSeleccionada") = ddlPortafolio.SelectedValue
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim Index As Integer = Row.RowIndex
            Dim numFila As Integer = Index
            If CType(e.CommandSource, ImageButton).CommandName = "Modificar" Then
                pnlPortafolio.Visible = False
                Dim OrdenFila As String
                Dim StrValor As String
                OrdenFila = Row.Cells(5).Text
                ViewState("Secuencial") = OrdenFila
                Session("FilaSeleccionadaNivel") = numFila
                DtDetalle = ViewState("NivelDetalle")
                StrValor = HttpUtility.HtmlDecode(dgLista.Rows.Item(numFila).Cells(8).Text).Trim
                If StrValor <> "-1" And StrValor <> "&nbsp;" Then
                    Me.tbValor.Text = StrValor
                Else
                    Me.tbValor.Text = ""
                End If
                Me.tbOrdenNivel.Text = OrdenFila 'CA
                ddlCaracteristica.SelectedValue = Right("00" & dgLista.Rows.Item(numFila).Cells(4).Text, 2)

                Dim viOrdenFila As Integer = 0
                Integer.TryParse(OrdenFila, viOrdenFila)

                lblCaracteristicaAnterior.Visible = False
                ddlCaracteristicaAnterior.Visible = False
                If (viOrdenFila = 3) Then
                    lblCaracteristicaAnterior.Visible = True
                    ddlCaracteristicaAnterior.Visible = True
                    CargarCaracteristicaAnterior(dgLista.Rows.Item(numFila - 1).Cells(4).Text)
                    ddlCaracteristicaAnterior.SelectedValue = "1"
                End If

                If dgLista.Rows.Item(numFila).Cells(7).Text.ToUpper = "GENERAL" Or dgLista.Rows.Item(numFila).Cells(7).Text.ToUpper = "G" Then
                    Me.rblOpcion.SelectedValue = "G"
                    Me.divPorcentaje_Valores.Attributes.Add("class", "hidden")
                    Me.tdCentro.Attributes.Add("class", "hidden")
                    Me.tdPorcentajeValor.Visible = True
                    Me.lblValor.Visible = True
                    Me.tbValor.Visible = True
                    Me.tdIzqPorcentajeValor.Visible = True
                Else
                    If dgLista.Rows.Item(numFila).Cells(7).Text.ToUpper = "AGRUPACION" Or dgLista.Rows.Item(numFila).Cells(7).Text.ToUpper = "A" Then
                        Me.rblOpcion.SelectedValue = "A"
                    End If
                    If dgLista.Rows.Item(numFila).Cells(7).Text.ToUpper = "DETALLE" Or dgLista.Rows.Item(numFila).Cells(7).Text.ToUpper = "D" Then
                        Me.rblOpcion.SelectedValue = "D"
                        hdCodigoLimiteNivel.Value = DtDetalle.Rows(numFila)(0)
                        chkLimiteNIvel.Checked = IIf(DtDetalle.Rows(numFila)(9).Equals("S"), True, False)
                        txtLimiteNIvelMin.Text = DtDetalle.Rows(numFila)(10)
                        txtLimiteNivelMax.Text = DtDetalle.Rows(numFila)(11)
                        chkLimiteEnDetalle.Checked = IIf(DtDetalle.Rows(numFila)(12).Equals("S"), True, False)
                        txtLImiteEnDetalleMin.Text = DtDetalle.Rows(numFila)(13)
                        txtLImiteEnDetalleMax.Text = DtDetalle.Rows(numFila)(14)

                        Session("dtblCondicion") = oLimiteBM.Seleccionar_Condicion_LimiteNIvel(DtDetalle.Rows(numFila)(0))
                    End If
                    Me.divPorcentaje_Valores.Attributes.Remove("class")
                    Me.tdCentro.Attributes.Remove("class")
                    Me.tdPorcentajeValor.Visible = False
                    Me.lblValor.Visible = False
                    Me.tbValor.Visible = False
                    Me.tdIzqPorcentajeValor.Visible = False
                    CargarGrillaValores(Me.rblOpcion.SelectedValue.ToString)
                End If
                If dgLista.Rows.Item(numFila).Cells(9).Text = "ACTIVO" Then
                    Me.ddlSituacion.SelectedValue = "A"
                Else
                    Me.ddlSituacion.SelectedValue = "I"
                End If
                Me.btnAgregar.TabIndex = 1
                btnAgregar.Visible = False
                btnModificar.Visible = True
            ElseIf (e.CommandName = "Eliminar") Then
                Dim dtAux As DataTable
                dtAux = CType(ViewState("NivelDetalle"), DataTable)
                dtAux.Rows.RemoveAt(numFila)
                ViewState("NivelDetalle") = dtAux
                Me.dgLista.DataSource = dtAux
                Me.dgLista.DataBind()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la operación de la Grilla")
        End Try
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            AgregarNivel()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregrar")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim Indice As Integer
            DtDetalle = ViewState("NivelDetalle")
            If Not (Me.DtDetalle Is Nothing) Then
                Try
                    Indice = CInt(ddlPortafolio.SelectedValue)
                    ListaCaracteristicas = CType(Session("ArregloCaracteristicas"), Hashtable)
                    If ListaCaracteristicas(Indice) Is Nothing Then
                        ListaCaracteristicas.Add(Indice, DtDetalle)
                    Else
                        Dim dtTemp As DataTable = ListaCaracteristicas.Item(Indice)
                        Dim drTemp As DataRow
                        For Each drTemp In dtTemp.Rows
                            If DtDetalle.Select("CodigoNivelLimite = '" & drTemp("CodigoNivelLimite") & "'").Length <= 0 Then
                                drTemp("situacion") = "INACTIVO"
                            End If
                        Next
                        ListaCaracteristicas.Item(Indice) = dtTemp
                    End If
                    AlertaJS("Se asignaron los niveles correctamente", "window.close();")
                Catch ex As Exception
                    AlertaJS(ex.ToString)
                End Try
            Else
                AlertaJS("Debe Ingresar mínimo un Nivel")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub rblOpcion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblOpcion.SelectedIndexChanged
        Try
            If rblOpcion.SelectedValue.ToString = "G" Then
                Me.divPorcentaje_Valores.Attributes.Add("class", "hidden")
                Me.tdCentro.Attributes.Add("class", "hidden")
                Me.lblValor.Visible = True
                Me.tbValor.Visible = True
                Me.tdPorcentajeValor.Visible = True
                tdIzqPorcentajeValor.Visible = True
            Else
                Me.btnAgregar.Attributes.Remove("onclick")
                CodigoLimite = Request.QueryString("CodigoLimite")
                CodigoGrupoInstrumento = Request.QueryString("codGrupo")
                btnModificar.Visible = False
                btnAgregar.Visible = True
                CargarGrillaValores(rblOpcion.SelectedValue.ToString)
                chkTodos.Attributes.Add("onclick", "javascript:TodosOnClick(" & dgListaValores.ClientID & ");")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Protected Sub dgListaValores_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgListaValores.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim valor As String = ""
                Dim txt As TextBox
                valor = CType(e.Row.FindControl("hdValorPorcentaje"), HiddenField).Value
                txt = CType(e.Row.FindControl("txtValor"), TextBox)
                txt.ReadOnly = True
                'Maximo
                If (ValidarNumero(valor, 2)) Then
                    CType(e.Row.Cells(13).Controls(1), TextBox).Text = valor
                    CType(e.Row.Cells(13).Controls(1), TextBox).ReadOnly = False
                End If

                Dim estado As String
                estado = e.Row.Cells(1).Text
                If estado = "S" Then
                    CType(e.Row.Cells(0).Controls(1), CheckBox).Checked = True
                End If

                CType(e.Row.Cells(8).Controls(1), CheckBox).Checked = IIf(e.Row.Cells(14).Text.Equals("S"), True, False)


                If e.Row.Cells(14).Text.Equals("S") And chkLimiteEnDetalle.Checked Then
                    CType(e.Row.Cells(13).Controls(1), TextBox).Enabled = False
                    CType(e.Row.Cells(10).Controls(1), TextBox).Enabled = False
                End If


                If TopeLimite = TIPO_LIMITE_BANDAS Then
                    Dim valorM As String
                    valorM = e.Row.Cells(12).Text
                    'Minimo
                    If (ValidarNumero(valorM, 2)) Then
                        CType(e.Row.Cells(10).Controls(1), TextBox).Text = valorM
                        CType(e.Row.Cells(10).Controls(1), TextBox).ReadOnly = False
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim valor As String
                valor = e.Row.Cells(7).Text
                If (valor = "-1" Or valor = "-1.0000000") Then
                    e.Row.Cells(7).Text = ""
                End If


                If e.Row.Cells(10).Text = "S" Then
                    e.Row.Cells(10).Text = "SI"
                Else
                    e.Row.Cells(10).Text = "NO"
                End If

            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Private Sub ddlCaracteristica_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCaracteristica.SelectedIndexChanged
        Try
            If (ddlCaracteristica.SelectedIndex = -1) Then
                Return
            End If
            If (ddlCaracteristica.SelectedValue = "00") Then
                rblOpcion.Items(1).Attributes.Add("disabled", "true")
                rblOpcion.Items(1).Attributes.CssStyle.Add("disabled", "true")
                rblOpcion.Items(2).Attributes.CssStyle.Add("disabled", "true")
                rblOpcion.Items(2).Attributes.Add("disabled", "true")
            Else
            End If
            If rblOpcion.SelectedValue.ToString = "G" Then
                Me.divPorcentaje_Valores.Attributes.Add("class", "hidden")
                Me.tdCentro.Attributes.Add("class", "hidden")
                '----------------------------------
                Me.lblValor.Visible = True
                Me.tbValor.Visible = True
                Me.tdPorcentajeValor.Visible = True

                tdIzqPorcentajeValor.Visible = True
            Else
                If (ddlCaracteristica.SelectedValue <> "00") Then
                    Me.btnAgregar.Attributes.Remove("onclick")
                End If
                CargarGrillaValores(rblOpcion.SelectedValue.ToString)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            LimpiarCampos()
            ViewState("CaracteristicaSeleccionada") = ddlPortafolio.SelectedValue
            Session("filaseleccionada") = ddlPortafolio.SelectedValue
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Try
            pnlPortafolio.Visible = True
            AgregarNivel()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar")
        End Try
    End Sub
    Protected Sub dgListaValores_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaValores.PageIndexChanging
        Try
            dgListaValores.PageIndex = e.NewPageIndex
            CargarGrillaValores(Me.rblOpcion.SelectedValue.ToString)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
#End Region
#Region "/*Funciones Personalizadas*/"

    Private Function VerificarOrdenNivelLimite(ByVal DtDetalle As DataTable) As Boolean
        Dim ArregloOk(DtDetalle.Rows.Count) As Boolean
        Dim i As Integer = 0
        While (i < ArregloOk.Length)
            ArregloOk(i) = False
            i = i + 1
        End While

        For Each row As DataRow In DtDetalle.Rows
            Dim Secuencial As Integer
            Secuencial = Convert.ToInt16(row.Item("Secuencial").ToString())
            If Secuencial < ArregloOk.Length Then
                ArregloOk(Secuencial) = True
            End If
        Next
        i = 1
        While (i < ArregloOk.Length)
            If (ArregloOk(i) = False) Then
                Return False
            End If
            i = i + 1
        End While
        Return True
    End Function

    Public Sub CargarPagina()
        Dim Indice As Integer
        Dim DtTablaDetalle As DataTable
        If claseLimite = "SF" Then
            pnlPortafolio.Visible = False
        Else
            pnlPortafolio.Visible = True
        End If
        Dim dt As DataTable = New GrupoInstrumentoBM().SeleccionarPorFiltro(CodigoGrupoInstrumento, "", "", DatosRequest).Tables(0)
        If dt.Rows.Count > 0 Then
            DescripcionGrupoInstrumento = dt.Rows(0)("Descripcion")
        Else
            DescripcionGrupoInstrumento = "Todos"
        End If
        txtGrupoInstrumento.Text = DescripcionGrupoInstrumento
        CargarCombos()
        Dim dtblGenerico As DataTable = UIUtility.GetStructureTablebase(campos, tipos)
        Dim dtblCondicion As DataTable = UIUtility.GetStructureTablebase(CamposCondicion, TipoCondicion)
        Session("dtblCondicion") = dtblCondicion
        dgLista.DataSource = dtblGenerico : dgLista.DataBind()
        Dim dtDetalleNivelLimite As DataTable = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)
        Session("FilaSeleccionadaNivel") = -1
        ViewState("Secuencial") = -1
        Session("NivelDetalle") = Nothing
        ViewState("NivelDetalle") = Nothing
        ListaCaracteristicas = CType(Session("ArregloCaracteristicas"), Hashtable)
        ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
        Indice = Session("FilaSeleccionada")

        If Not (ListaCaracteristicas(Indice) Is Nothing) Then
            DtTablaDetalle = ListaCaracteristicas(Indice)
            ViewState("NivelDetalle") = DtTablaDetalle
            ValidaFormularioMultinivel(DtTablaDetalle)
            CargarRegistro(DtTablaDetalle)
        Else
            rblOpcion.Enabled = False
            tbOrdenNivel.Text = "1"

        End If


        Me.divPorcentaje_Valores.Attributes.Add("class", "hidden")
        Me.tdCentro.Attributes.Add("class", "hidden")

        txtLimiteNivelMax.Enabled = False
        txtLimiteNIvelMin.Enabled = False
        txtLImiteEnDetalleMin.Enabled = False
        txtLImiteEnDetalleMax.Enabled = False

        'ca
        If TopeLimite = TIPO_LIMITE_BANDAS Then
            tbValorMin.Visible = True
            lbMin.Visible = True
            lbMax.Visible = True
            dgListaValores.Columns(10).Visible = True
        Else
            tbValorMin.Visible = False
            lbMin.Visible = False
            lbMax.Visible = False
            dgListaValores.Columns(10).Visible = False
        End If
    End Sub

    Public Sub ValidaFormularioMultinivel(ByVal DtTablaDetalle As DataTable)
        Dim vSecuencial As Integer,
            vTotal As Integer

        'Deshabilitamos el txt Orden Nivel
        tbOrdenNivel.Enabled = False

        If (DtTablaDetalle.Rows.Count = 0) Then
            rblOpcion.Enabled = False
            tbOrdenNivel.Text = "1"
        Else
            rblOpcion.Enabled = True
            If (DtTablaDetalle.Rows.Count > 0) Then
                vTotal = DtTablaDetalle.Rows.Count
                vSecuencial = DtTablaDetalle.Rows(vTotal - 1)("Secuencial")
                vCodigoCaracteristica = DtTablaDetalle.Rows(vTotal - 1)("CodigoCaracteristica")
                'Seteamos un Orden por Nivel, por defecto
                tbOrdenNivel.Text = vSecuencial + 1

                'Validamos si hay mas de 2 niveles - para crear una relacion del nivel anterior
                If (DtTablaDetalle.Rows.Count = 2) Then

                    lblCaracteristicaAnterior.Visible = True
                    ddlCaracteristicaAnterior.Visible = True
                    CargarCaracteristicaAnterior(vCodigoCaracteristica)
                End If

            End If
        End If

    End Sub

    Public Sub CargarCaracteristicaAnterior(ByVal CodigoCaracteristica As String)

        Dim TablaCaracteristica As New DataTable
        Dim objCaracteristicaGrupo As New CaracteristicaGrupoBM

        TablaCaracteristica = objCaracteristicaGrupo.ListarVistaCodigoCaracteristica(CodigoCaracteristica)
        HelpCombo.LlenarComboBox(Me.ddlCaracteristicaAnterior, TablaCaracteristica, "Codigo", "Descripcion", False)

    End Sub

    Public Sub LimpiarCampos()
        'Esconder el div de detalle nivel limite
        divPorcentaje_Valores.Attributes.Add("class", "hidden")
        tdCentro.Attributes.Add("class", "hidden")
        lblValor.Visible = True
        tbValor.Visible = True
        tdPorcentajeValor.Visible = True
        tdIzqPorcentajeValor.Visible = True
        'Continua con el resto de campos
        ddlCaracteristica.SelectedIndex = 0
        tbOrdenNivel.Text = ""
        tbValor.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedIndex = 0
        rblOpcion.SelectedIndex = 0

        Dim dt As DataTable
        dt = ViewState("NivelDetalle")
        ValidaFormularioMultinivel(dt)
    End Sub
    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim tablaCaracteristica As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaCaracteristica = oGrupoInstrumentoBM.SeleccionarCaracteristicaGrupo("", "", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlCaracteristica, tablaCaracteristica, "CodigoCaracteristica", "Descripcion", False)
        ddlCaracteristica.Items.Insert(0, New ListItem("Todo", "00"))
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub
    Public Sub CargarRegistro(ByVal Tabla As DataTable)
        DtDetalle = Tabla
        Dim i As Integer
        Dim valor, valorf As String
        Dim codGrupo As String
        codGrupo = Request.QueryString("codGrupo")
        If DtDetalle.Rows.Count = 0 Then
            divDetalle.Attributes.Add("class", "hidden")
        Else
            ViewState("NivelDetalle") = DtDetalle
            Me.dgLista.DataSource = DtDetalle
            Me.dgLista.DataBind()
            For i = 0 To Me.dgLista.Rows.Count - 1
                valor = dgLista.Rows(i).Cells(7).Text.ToString
                valorf = valor.Replace(",", ".")
                dgLista.Rows(i).Cells(7).Text = valorf
                'General o Agrupacion
                If (dgLista.Rows(i).Cells(6).Text.ToString = "G") Then
                    dgLista.Rows(i).Cells(6).Text = "GENERAL"
                ElseIf (dgLista.Rows(i).Cells(6).Text.ToString = "A") Then
                    dgLista.Rows(i).Cells(6).Text = "AGRUPACION"
                ElseIf (dgLista.Rows(i).Cells(6).Text.ToString = "D") Then
                    dgLista.Rows(i).Cells(6).Text = "DETALLE"
                End If
            Next
        End If
    End Sub
    Public Sub InicializarListaDetalleNivelLimite()
        Me.ListaDetalleNivelLimite = New Hashtable
        ListaDetalleNivelLimite.Clear()
    End Sub
    Private Sub EliminarDetalleNivelLimite()
        Dim claveDetalleNivelLimite As String
        Dim dtDetalleNivelLimite As New DataTable
        Dim ordenFila = ViewState("Secuencial")
        Dim filaCaracteristica As String = ViewState("CaracteristicaSeleccionada")
        claveDetalleNivelLimite = filaCaracteristica + "," + ordenFila.ToString()
        If Not Session("DetalleNivelLimite") Is Nothing Then
            ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
            If Not ListaDetalleNivelLimite(claveDetalleNivelLimite) Is Nothing Then
                Dim dtt As DataTable
                dtt = ListaDetalleNivelLimite(claveDetalleNivelLimite)
                oLimiteBM.EliminarDetalleNivelLimite(dtt, DatosRequest)
                ListaDetalleNivelLimite(claveDetalleNivelLimite) = Nothing
                Session("DetalleNivelLimite") = ListaDetalleNivelLimite
            End If
        End If
    End Sub
    Private Sub ModificarDetalleNivelLimite()
        Dim claveDetalleNivelLimite As String
        Dim dtDetalleNivelLimite As New DataTable
        Dim ordenFila = ViewState("Secuencial")
        Dim row As DataRow
        Dim filaCaracteristica As String = ViewState("CaracteristicaSeleccionada")
        claveDetalleNivelLimite = filaCaracteristica + "," + ordenFila.ToString() + "," & ddlCaracteristica.SelectedValue
        dtDetalleNivelLimite = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)
        For Each fila As GridViewRow In dgListaValores.Rows
            If CType(fila.Cells(0).Controls(1), CheckBox).Checked = True Then
                row = dtDetalleNivelLimite.NewRow
                row.Item("Selected") = "S"
                row.Item("CodigoNivelLimite") = fila.Cells(2).Text
                row.Item("CodigoCaracteristica") = fila.Cells(3).Text
                row.Item("NombreVista") = fila.Cells(4).Text
                row.Item("ClaseNormativa") = HttpUtility.HtmlDecode(fila.Cells(5).Text).Trim
                row.Item("ValorCaracteristica") = fila.Cells(6).Text
                row.Item("DescripcionValorCaracteristica") = fila.Cells(7).Text
                'CA
                If (CType(fila.Cells(13).Controls(1), TextBox).Text <> "") Then
                    row.Item("ValorPorcentaje") = CType(fila.Cells(13).Controls(1), TextBox).Text
                End If
                If (CType(fila.Cells(10).Controls(1), TextBox).Text <> "") Then
                    row.Item("ValorPorcentajeM") = CType(fila.Cells(10).Controls(1), TextBox).Text
                End If

                If CType(fila.Cells(8).Controls(1), CheckBox).Checked = True Then
                    row.Item("ValorEspecifico") = "S"
                Else
                    row.Item("ValorEspecifico") = "N"
                End If

                row.Item("CodigoCaracteristicaRelacion") = IIf(tbOrdenNivel.Text = "3", vCodigoCaracteristica, "")
                row.Item("CodigoRelacion") = IIf(tbOrdenNivel.Text = "3", ddlCaracteristicaAnterior.SelectedValue.ToString(), "")

                dtDetalleNivelLimite.Rows.Add(row)
            End If
        Next

        If (ddlCaracteristicaAnterior.Visible = True) Then
            If Not Session("DetalleNivelLimite") Is Nothing Then
                Dim dt As New DataTable
                Dim ListaDetalle As New Hashtable
                ListaDetalle = CType(Session("DetalleNivelLimite"), Hashtable)
                dt = ListaDetalle(claveDetalleNivelLimite)
                For Each det In dtDetalleNivelLimite.Rows
                    For Each f In dt.Rows
                        If (f("ValorCaracteristica").ToString.Equals(det("ValorCaracteristica"))) Then
                            f("ValorPorcentaje") = det("ValorPorcentaje")
                            f("ValorEspecifico") = det("ValorEspecifico")
                        End If
                    Next
                Next
                ListaDetalleNivelLimite = Session("DetalleNivelLimite")
            End If

        Else
            If Not Session("DetalleNivelLimite") Is Nothing Then
                ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
            Else
                InicializarListaDetalleNivelLimite()
            End If
            If Not ListaDetalleNivelLimite(claveDetalleNivelLimite) Is Nothing Then
                ListaDetalleNivelLimite(claveDetalleNivelLimite) = dtDetalleNivelLimite
            Else
                ListaDetalleNivelLimite.Add(claveDetalleNivelLimite, dtDetalleNivelLimite)
            End If
        End If

        Session("DetalleNivelLimite") = ListaDetalleNivelLimite
    End Sub
    Private Sub InsertarDetalleNivelLimite(ByVal orden As Integer)
        Dim txtValor As TextBox
        Dim txtValorM As TextBox
        Dim claveDetalleNivelLimite As String
        Dim dtDetalleNivelLimite As New DataTable
        Dim row As DataRow
        Dim filaCaracteristica As String = ViewState("CaracteristicaSeleccionada")
        claveDetalleNivelLimite = filaCaracteristica + "," + orden.ToString() + "," + ddlCaracteristica.SelectedValue
        dtDetalleNivelLimite = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)
        For Each fila As GridViewRow In dgListaValores.Rows
            If CType(fila.Cells(0).Controls(1), CheckBox).Checked = True Then
                row = dtDetalleNivelLimite.NewRow
                row.Item("Selected") = "S"
                row.Item("CodigoNivelLimite") = fila.Cells(2).Text
                row.Item("CodigoCaracteristica") = fila.Cells(3).Text
                row.Item("NombreVista") = fila.Cells(4).Text
                row.Item("ClaseNormativa") = HttpUtility.HtmlDecode(fila.Cells(5).Text)
                row.Item("ValorCaracteristica") = fila.Cells(6).Text
                row.Item("DescripcionValorCaracteristica") = fila.Cells(7).Text
                If Not (fila.FindControl("txtValor") Is Nothing) Then
                    txtValor = CType(fila.FindControl("txtValor"), TextBox)
                    If (txtValor.Text.Trim() <> "") Then
                        row.Item("ValorPorcentaje") = txtValor.Text
                    ElseIf (txtTodos.Text <> "") Then
                        row.Item("ValorPorcentaje") = txtTodos.Text
                    End If
                End If
                If Not (CType(fila.FindControl("txtValoM"), TextBox) Is Nothing) Then
                    txtValorM = CType(fila.FindControl("txtValoM"), TextBox)
                    If (txtValorM.Text.Trim() <> "") Then
                        row.Item("ValorPorcentajeM") = txtValorM.Text
                    Else
                        row.Item("ValorPorcentajeM") = -1
                    End If
                End If
                row.Item("CodigoCaracteristicaRelacion") = IIf(tbOrdenNivel.Text = "3", vCodigoCaracteristica, "")
                row.Item("CodigoRelacion") = IIf(tbOrdenNivel.Text = "3", ddlCaracteristicaAnterior.SelectedValue.ToString(), "")

                dtDetalleNivelLimite.Rows.Add(row)
            End If
        Next
        If Not (Session("DetalleNivelLimite") Is Nothing) Then
            ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
        Else
            Me.ListaDetalleNivelLimite = New Hashtable
            InicializarListaDetalleNivelLimite()
        End If
        If Not ListaDetalleNivelLimite(claveDetalleNivelLimite) Is Nothing Then
            ListaDetalleNivelLimite(claveDetalleNivelLimite) = dtDetalleNivelLimite
        Else
            ListaDetalleNivelLimite.Add(claveDetalleNivelLimite, dtDetalleNivelLimite)
        End If
        Session("DetalleNivelLimite") = ListaDetalleNivelLimite
    End Sub
    Public Sub InsertarFila()
        Dim dtAux As New DataTable
        Dim fila As DataRow
        Dim StrValor As String
        Dim Indice As Integer
        If Not ViewState("NivelDetalle") Is Nothing Then
            dtAux = ViewState("NivelDetalle")
        Else
            dtAux = UIUtility.GetStructureTablebase(campos, tipos)
        End If
        fila = dtAux.NewRow()
        If dtAux.Rows.Count > 0 Then
            fila.Item("CodigoLimiteCaracteristica") = dtAux.Rows(0)("CodigoLimiteCaracteristica")
        Else
            fila.Item("CodigoLimiteCaracteristica") = "Nuevo"
        End If
        fila.Item("CodigoNivelLimite") = "Nuevo"
        fila.Item("Secuencial") = Me.tbOrdenNivel.Text
        fila.Item("CodigoCaracteristica") = ddlCaracteristica.SelectedValue
        fila.Item("DescripcionCaracteristica") = ddlCaracteristica.SelectedItem.Text
        If Me.rblOpcion.SelectedValue = "G" Then
            fila.Item("FlagTipoPorcentaje") = "GENERAL"
            StrValor = Me.tbValor.Text.Trim()
            If StrValor = "" Then
                fila.Item("ValorPorcentaje") = -1
            Else
                fila.Item("ValorPorcentaje") = Convert.ToDecimal(StrValor.Replace(".", UIUtility.DecimalSeparator))
            End If
        Else
            If Me.rblOpcion.SelectedValue = "A" Then
                fila.Item("FlagTipoPorcentaje") = "AGRUPACION"
                fila.Item("ValorPorcentaje") = -1
            ElseIf Me.rblOpcion.SelectedValue = "D" Then
                fila.Item("FlagTipoPorcentaje") = "DETALLE"
                fila.Item("ValorPorcentaje") = -1
            End If
            InsertarDetalleNivelLimite(Me.tbOrdenNivel.Text)
        End If
        If Me.ddlSituacion.SelectedValue = "A" Then
            fila.Item("Situacion") = "ACTIVO"
        Else
            fila.Item("Situacion") = "INACTIVO"
        End If
        dtAux.Rows.Add(fila)
        Indice = Session("filaseleccionada")
        CType(Session("ArregloCaracteristicas"), Hashtable)(Indice) = dtAux
        CargarRegistro(dtAux)
        ViewState("NivelDetalle") = dtAux
    End Sub
    Public Sub ModificarFila()
        Dim numFila As Integer
        Dim ordenFila As String
        Dim estado As String
        Dim StrValor As String
        Dim Indice As Integer
        numFila = CType(Session("FilaSeleccionadaNivel"), Integer)
        ordenFila = ViewState("Secuencial")
        If (numFila <> -1) Then
            Me.DtDetalle = ViewState("NivelDetalle")
            Me.DtDetalle.Rows(numFila).Item("Secuencial") = Me.tbOrdenNivel.Text
            Me.DtDetalle.Rows(numFila).Item("CodigoCaracteristica") = ddlCaracteristica.SelectedValue
            Me.DtDetalle.Rows(numFila).Item("DescripcionCaracteristica") = ddlCaracteristica.SelectedItem.Text
            Me.DtDetalle.Rows(numFila).Item("CodigoRelacion") = IIf((ddlCaracteristicaAnterior.Visible = True), ddlCaracteristicaAnterior.SelectedValue, "0")

            If Me.rblOpcion.SelectedValue = "G" Then
                Me.DtDetalle.Rows(numFila).Item("FlagTipoPorcentaje") = "GENERAL"
                StrValor = Me.tbValor.Text.Trim()
                If StrValor = "" Then
                    Me.DtDetalle.Rows(numFila).Item("ValorPorcentaje") = -1
                Else
                    Me.DtDetalle.Rows(numFila).Item("ValorPorcentaje") = Convert.ToDecimal(StrValor.Replace(".", UIUtility.DecimalSeparator))
                End If
                EliminarDetalleNivelLimite()
            Else
                If Me.rblOpcion.SelectedValue = "A" Then
                    Me.DtDetalle.Rows(numFila).Item("FlagTipoPorcentaje") = "AGRUPACION"
                    Me.DtDetalle.Rows(numFila).Item("ValorPorcentaje") = -1
                ElseIf Me.rblOpcion.SelectedValue = "D" Then
                    Me.DtDetalle.Rows(numFila).Item("FlagTipoPorcentaje") = "DETALLE"
                    Me.DtDetalle.Rows(numFila).Item("ValorPorcentaje") = -1

                    If chkLimiteNIvel.Checked Then
                        Me.DtDetalle.Rows(numFila).Item("TieneLimiteNivel") = "S"
                    Else
                        Me.DtDetalle.Rows(numFila).Item("TieneLimiteNivel") = "N"
                    End If

                    Me.DtDetalle.Rows(numFila).Item("LimiteNivelMin") = txtLimiteNIvelMin.Text
                    Me.DtDetalle.Rows(numFila).Item("LimiteNivelMax") = txtLimiteNivelMax.Text

                    If chkLimiteEnDetalle.Checked Then
                        Me.DtDetalle.Rows(numFila).Item("TieneLimiteFijoEnDetalle") = "S"
                    Else
                        Me.DtDetalle.Rows(numFila).Item("TieneLimiteFijoEnDetalle") = "N"
                    End If

                    Me.DtDetalle.Rows(numFila).Item("LimiteFijoEnDetalleMin") = txtLImiteEnDetalleMin.Text
                    Me.DtDetalle.Rows(numFila).Item("LimiteFijoEnDetalleMax") = txtLImiteEnDetalleMax.Text

                End If
                ModificarDetalleNivelLimite()
            End If
            If Me.ddlSituacion.SelectedValue = "A" Then
                estado = "ACTIVO"
            Else
                estado = "INACTIVO"
            End If
            Me.DtDetalle.Rows(numFila).Item("Situacion") = estado
            DtDetalle.DefaultView.Sort = "Secuencial"
            ViewState("NivelDetalle") = DtDetalle.DefaultView
            Indice = Session("filaseleccionada")
            CType(Session("ArregloCaracteristicas"), Hashtable)(Indice) = DtDetalle
            If Not (ddlCaracteristicaAnterior.Visible = True) Then
                CargarRegistro(DtDetalle)
                LimpiarCampos()
                Session("FilaSeleccionadaNivel") = -1
                ViewState("Secuencial") = -1
            End If
        End If
    End Sub
    Public Function VerificarValor() As String
        Dim msg As String = ""
        Dim strMensajeError As String = ""
        Dim valor As Decimal
        valor = Convert.ToDecimal(Me.tbValor.Text)
        If valor > 100 Then
            msg += "El valor ingresado no debe ser mayor al 100% </br>"
        Else
            If valor = 0 Then
                msg += "El valor ingresado no puede ser 0 </br>"
            End If
        End If
        If (msg <> "") Then
            strMensajeError = msg + "</br>"
            Return strMensajeError
        Else
            Return ""
        End If
    End Function
    Public Function ValidarNumero(ByVal Cadena() As Char, ByVal Tipo As Byte) As Boolean
        Dim Estado As Boolean
        Dim CadBase As String = "0123456789"
        Dim posic, Largo, Ind As Integer
        Dim Eval As Char

        If Tipo = 2 Then
            CadBase = CadBase + "."
        End If
        If Tipo = 3 Then
            CadBase = CadBase + "/"
        End If
        Estado = True
        Largo = Len(Cadena)
        Ind = 0
        Do While Ind < Largo
            Eval = Cadena(Ind)
            posic = InStr(CadBase, Eval)
            If posic = 0 Then
                Estado = False
                Ind = Largo
            End If
            Ind = Ind + 1
        Loop
        Return Estado
    End Function
    Public Function ValidarCadena(ByVal Cadena() As Char) As Boolean
        Dim Estado As Boolean
        Dim CadBase As String = "@" + Chr(34)
        Dim posic, Largo, Ind As Integer
        Dim Eval As Char
        Estado = True
        Largo = Len(Cadena)
        Ind = 0
        Do While Ind < Largo
            Eval = Cadena(Ind)
            posic = InStr(CadBase, Eval)
            If posic > 0 Then
                Estado = False
                Ind = Largo
            End If
            Ind = Ind + 1
        Loop
        Return Estado
    End Function
    Public Function ValidarCamposDetalleImporte() As String
        Dim Mensaje As String = ""
        If Me.rblOpcion.SelectedIndex = 0 Then
            If (Me.tbValor.Text <> "") Then
                If (ValidarNumero(Me.tbValor.Text.Trim(), 2)) Then
                    If Convert.ToDecimal(Me.tbValor.Text.Trim()) > 100 Or Convert.ToDecimal(Me.tbValor.Text.Trim()) < 0 Then
                        Mensaje = "-Valor Porcentaje (Entre 0 y 100%)"
                    Else
                        Mensaje = ""
                    End If
                Else
                    Mensaje = "-Valor Porcentaje (Numérico)"
                End If
            End If
        ElseIf Me.rblOpcion.SelectedIndex = 1 Then
            Dim dtAux As New DataTable
            Dim cont As Integer = 0
            If Not (ViewState("DetalleNivelLimite") Is Nothing) Then
                dtAux = ViewState("DetalleNivelLimite")
                For Each row As DataRow In dtAux.Rows
                    If (row.Item("ValorPorcentaje").ToString <> "") Then
                        If (ValidarNumero(row.Item("ValorPorcentaje").ToString, 2)) Then
                            If Convert.ToDecimal(row.Item("ValorPorcentaje").ToString) > 100 Or Convert.ToDecimal(row.Item("ValorPorcentaje").ToString) <= 0 Then
                                Mensaje = Mensaje + "</br>-Valor Porcentaje Fila " + cont + ", (Entra 0 y 100%)"
                            Else
                                Mensaje = ""
                            End If
                        Else
                            Mensaje = Mensaje + "</br>-Valor Porcentaje Fila " + cont + ", (Numérico)"
                        End If
                    End If
                    cont = cont + 1
                Next
            End If
        End If
        Return Mensaje
    End Function
    Private Sub CargarGrillaValores(ByVal FlagTipoPorcentaje As String)
        Dim oGrupoInstrumentoBM As New GrupoInstrumentoBM
        Dim dtDetalleNivelLimite As New DataTable
        Dim dtDetalleNivelLimiteValida As New DataTable
        Dim dtCaracteristicasGrupo As New DataTable
        Dim OrdenFila As String
        Dim dtDetalle As New DataTable
        Dim dtValores As New DataTable
        Dim dtValoresGrupo As New DataTable
        Dim row As DataRow
        Dim i As Integer = 0
        Dim codGrupo As String
        Dim numFila As Integer
        Dim claveDetalleNivelLimite As String
        dtDetalle = ViewState("NivelDetalle")
        numFila = Session("FilaSeleccionadaNivel")
        OrdenFila = ViewState("Secuencial")
        codGrupo = CodigoGrupoInstrumento
        Dim CodigoCaracteristica As String = IIf(ddlCaracteristica.SelectedIndex = 0, "", ddlCaracteristica.SelectedValue)
        dtCaracteristicasGrupo = oGrupoInstrumentoBM.SeleccionarCodigoCaracteristicasGrupoNivel(codGrupo, FlagTipoPorcentaje, CodigoCaracteristica, DatosRequest).Tables(0)
        'Activar el espacio de la grilla
        Me.divPorcentaje_Valores.Attributes.Remove("class")
        Me.tdCentro.Attributes.Remove("class")
        Me.tdPorcentajeValor.Visible = False
        Me.lblValor.Visible = False
        Me.tdIzqPorcentajeValor.Visible = False
        Dim ValidaData As String = "S"

        If Not (Session("DetalleNivelLimite") Is Nothing) And numFila <> -1 Then
            ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
            Dim filaCaracteristica As String = ViewState("CaracteristicaSeleccionada")
            claveDetalleNivelLimite = filaCaracteristica + "," + OrdenFila.ToString() + "," + ddlCaracteristica.SelectedValue
            dtDetalleNivelLimite = ListaDetalleNivelLimite(claveDetalleNivelLimite)
            If CodigoGrupoInstrumento = codGrupo And Not dtDetalleNivelLimite Is Nothing Then
                If dtDetalle.Rows(numFila).Item("FlagTipoPorcentaje").ToString.Substring(0, 1) = FlagTipoPorcentaje Then
                    If FlagTipoPorcentaje = "D" Then
                        For Each fila As DataRow In dtCaracteristicasGrupo.Rows
                            If dtDetalleNivelLimite.Select("ValorCaracteristica = '" & fila("ValorCaracteristica").ToString & "'").Length = 0 Then
                                row = dtDetalleNivelLimite.NewRow
                                row.Item("Selected") = "N"
                                row.Item("CodigoCaracteristica") = fila.Item("CodigoCaracteristica").ToString
                                row.Item("NombreVista") = fila.Item("Vista").ToString
                                row.Item("ClaseNormativa") = fila.Item("ClaseNormativa").ToString
                                row.Item("ValorCaracteristica") = fila.Item("ValorCaracteristica").ToString
                                row.Item("CodigoCaracteristicaRelacion") = ""
                                row.Item("CodigoRelacion") = ""
                                'row.Item("ValorEspecifico") = fila.Item("ValorEspecifico").ToString
                                dtDetalleNivelLimite.Rows.Add(row)
                            End If
                        Next
                        For Each fila As DataRow In dtDetalleNivelLimite.Rows
                            fila.Item("DescripcionValorCaracteristica") = oGrupoInstrumentoBM.SeleccionarDescripcionValoresPorValorVista(fila.Item("ValorCaracteristica").ToString, fila.Item("NombreVista").ToString, DatosRequest).ToString
                        Next
                    End If
                Else
                    dtDetalleNivelLimite = New DataTable
                    dtDetalleNivelLimite = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)
                    For Each fila As DataRow In dtCaracteristicasGrupo.Rows
                        row = dtDetalleNivelLimite.NewRow
                        row.Item("Selected") = "N"
                        row.Item("CodigoCaracteristica") = fila.Item("CodigoCaracteristica").ToString
                        row.Item("NombreVista") = fila.Item("Vista").ToString
                        row.Item("ClaseNormativa") = fila.Item("ClaseNormativa").ToString
                        row.Item("ValorCaracteristica") = fila.Item("ValorCaracteristica").ToString
                        row.Item("CodigoCaracteristicaRelacion") = ""
                        row.Item("CodigoRelacion") = ""
                        If CType(fila.Item("CodigoCaracteristica"), String) = "04" And CType(fila.Item("CodigoGrupoInstrumento"), String) = "GI005" Then
                            row.Item("DescripcionValorCaracteristica") = fila.Item("DescripcionValorCaracteristica").ToString
                            row.Item("ValorPorcentaje") = CType(IIf(fila.Item("ValorPorcentaje").ToString = "", -1, fila.Item("ValorPorcentaje").ToString), Decimal)
                        Else
                            row.Item("DescripcionValorCaracteristica") = oGrupoInstrumentoBM.SeleccionarDescripcionValoresPorValorVista(fila.Item("ValorCaracteristica").ToString, fila.Item("Vista").ToString, DatosRequest)
                        End If

                        row.Item("ValorEspecifico") = fila.Item("ValorEspecifico").ToString

                        dtDetalleNivelLimite.Rows.Add(row)
                    Next
                End If
            Else
                dtDetalleNivelLimite = New DataTable
                dtDetalleNivelLimite = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)
                For Each fila As DataRow In dtCaracteristicasGrupo.Rows
                    row = dtDetalleNivelLimite.NewRow
                    row.Item("CodigoNivelLimite") = "Nuevo"
                    row.Item("Selected") = "N"
                    row.Item("CodigoCaracteristica") = fila.Item("CodigoCaracteristica").ToString
                    row.Item("NombreVista") = fila.Item("Vista").ToString
                    row.Item("ClaseNormativa") = fila.Item("ClaseNormativa").ToString
                    row.Item("ValorCaracteristica") = fila.Item("ValorCaracteristica").ToString
                    row.Item("DescripcionValorCaracteristica") = oGrupoInstrumentoBM.SeleccionarDescripcionValoresPorValorVista(fila.Item("ValorCaracteristica").ToString, fila.Item("Vista").ToString, DatosRequest)
                    row.Item("CodigoCaracteristicaRelacion") = ""
                    row.Item("CodigoRelacion") = ""
                    'row.Item("ValorEspecifico") = fila.Item("ValorEspecifico").ToString
                    dtDetalleNivelLimite.Rows.Add(row)
                Next
            End If
        Else
            dtDetalleNivelLimite = New DataTable
            dtDetalleNivelLimite = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)
            For Each fila As DataRow In dtCaracteristicasGrupo.Rows
                row = dtDetalleNivelLimite.NewRow
                row.Item("CodigoNivelLimite") = "Nuevo"
                row.Item("Selected") = "N"
                row.Item("CodigoCaracteristica") = fila.Item("CodigoCaracteristica").ToString
                row.Item("NombreVista") = fila.Item("Vista").ToString
                row.Item("ClaseNormativa") = fila.Item("ClaseNormativa").ToString
                row.Item("ValorCaracteristica") = fila.Item("ValorCaracteristica").ToString
                row.Item("DescripcionValorCaracteristica") = oGrupoInstrumentoBM.SeleccionarDescripcionValoresPorValorVista(fila.Item("ValorCaracteristica").ToString, fila.Item("Vista").ToString, DatosRequest)
                row.Item("CodigoCaracteristicaRelacion") = ""
                row.Item("CodigoRelacion") = ""
                'row.Item("ValorEspecifico") = fila.Item("ValorEspecifico").ToString
                dtDetalleNivelLimite.Rows.Add(row)
            Next
        End If
        If FlagTipoPorcentaje = "D" Then
            dgListaValores.Columns(5).ItemStyle.CssClass = ""
            dgListaValores.Columns(5).HeaderStyle.CssClass = ""
            dgListaValores.Columns(7).ItemStyle.CssClass = ""
            dgListaValores.Columns(7).HeaderStyle.CssClass = ""
        Else
            dgListaValores.Columns(5).ItemStyle.CssClass = ""
            dgListaValores.Columns(5).HeaderStyle.CssClass = ""
            dgListaValores.Columns(7).ItemStyle.CssClass = "hidden"
            dgListaValores.Columns(7).HeaderStyle.CssClass = "hidden"
        End If

        If (ddlCaracteristica.SelectedValue.ToString.Equals("66")) Then
            Dim objCaracteristicaGrupo As New CaracteristicaGrupoBM
            Dim dtVista As DataTable
            dtVista = objCaracteristicaGrupo.ListarVistaCodigoCaracteristica(ddlCaracteristica.SelectedValue.ToString())
            For Each fila As DataRow In dtVista.Rows
                If (ddlCaracteristicaAnterior.SelectedValue = fila.Item(2).ToString()) Then

                    For Each det As DataRow In dtDetalleNivelLimite.Rows
                        If (det.Item(5) = fila.Item(0)) Then
                            det.Item("CodigoClaseActivoLimite") = fila.Item(2).ToString
                        End If
                    Next
                End If
            Next
            If (dtDetalleNivelLimite.Select("CodigoClaseActivoLimite = '" & ddlCaracteristicaAnterior.SelectedValue.ToString() & "'").Length = 0) Then
                'dtDetalleNivelLimite.Rows.Clear()
                ValidaData = "N"
                dtDetalleNivelLimiteValida = New DataTable
                dtDetalleNivelLimiteValida = UIUtility.GetStructureTablebase(camposDetalleNivelLimite, tiposDetalleNivelLimite)

            Else
                dtDetalleNivelLimite = dtDetalleNivelLimite.Select("CodigoClaseActivoLimite = '" & ddlCaracteristicaAnterior.SelectedValue.ToString() & "'").CopyToDataTable()
            End If


        End If
        Dim dvValores As DataView

        If (ValidaData.Equals("N")) Then
            dvValores = New DataView(dtDetalleNivelLimiteValida)
        Else
            dvValores = New DataView(dtDetalleNivelLimite)
            If CodigoCaracteristica = "23" Then
                dvValores.Sort = "ClaseNormativa desc, DescripcionValorCaracteristica"
            Else
                dvValores.Sort = "DescripcionValorCaracteristica"
            End If
        End If

        Dim view As DataView = DirectCast(dvValores, DataView)
        view.Sort = "ValorPorcentaje DESC, DescripcionValorCaracteristica"

        dgListaValores.DataSource = view.ToTable()
        dgListaValores.DataBind()
    End Sub
    Private Sub AgregarNivel()
        Dim Mensaje As String
        Dim dtDetalle As DataTable
        Dim OrdenAux As Integer
        dtDetalle = ViewState("NivelDetalle")
        divDetalle.Attributes.Remove("class")
        If (Me.rblOpcion.SelectedValue = "A" Or Me.rblOpcion.SelectedValue = "D") And chkTodos.Checked Then
            If (txtTodos.Text = "") Then
                AlertaJS("Ingreso un valor de % para todos los demas.")
                Exit Sub
            End If
        End If
        If (ViewState("Secuencial") <> -1) And (btnAgregar.TabIndex = 1) Then
            Mensaje = ValidarCamposDetalleImporte()
            If (Mensaje <> "") Then
                AlertaJS("Error en los siguientes campos: </br>" + Mensaje)
                Exit Sub
            End If
            If Not dtDetalle Is Nothing Then
                OrdenAux = dtDetalle.Rows.Count
                If (Convert.ToInt16(Me.tbOrdenNivel.Text) > OrdenAux) Then
                    AlertaJS("Orden Nivel no válido")
                    Exit Sub
                End If
            Else
                OrdenAux = Convert.ToInt16(Me.tbOrdenNivel.Text)
                If OrdenAux <> 1 Then
                    AlertaJS("Orden Nivel no válido")
                    Exit Sub
                End If
            End If
            ModificarFila()
            If Not (ddlCaracteristicaAnterior.Visible = True) Then 'Nivel 3 - Sigue modificando 
                LimpiarCampos()
            End If

            Me.btnAgregar.TabIndex = 0
            Me.btnModificar.Visible = False
            btnAgregar.Visible = True
        Else
            Mensaje = ValidarCamposDetalleImporte()
            If (Mensaje <> "") Then
                AlertaJS("confirm('" + Mensaje + "')")
                Exit Sub
            End If
            InsertarFila()
            LimpiarCampos()
        End If
    End Sub

    'Public Function FiltraGrilla(ByVal dtTable As DataTable)
    '    If (ddlCaracteristicaAnterior.Visible) Then
    '        dtTable.Select()

    '    End If
    'End Function

#End Region

    Protected Sub chkLimiteNIvel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLimiteNIvel.CheckedChanged

        If chkLimiteNIvel.Checked Then
            txtLimiteNIvelMin.Enabled = True
            txtLimiteNivelMax.Enabled = True
        Else
            txtLimiteNIvelMin.Enabled = False
            txtLimiteNIvelMin.Text = String.Empty
            txtLimiteNivelMax.Enabled = False
            txtLimiteNivelMax.Text = String.Empty
        End If

    End Sub

    Protected Sub chkLimiteEnDetalle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLimiteEnDetalle.CheckedChanged

        Dim Valor As Boolean = True

        If chkLimiteEnDetalle.Checked Then
            txtLImiteEnDetalleMin.Enabled = True
            txtLImiteEnDetalleMax.Enabled = True
            Valor = False
        Else
            txtLImiteEnDetalleMin.Enabled = False
            txtLImiteEnDetalleMin.Text = String.Empty
            txtLImiteEnDetalleMax.Enabled = False
            txtLImiteEnDetalleMax.Text = String.Empty


        End If

        For Each fila As GridViewRow In dgListaValores.Rows
            Dim tbValorMin As TextBox = CType(fila.FindControl("txtValor"), TextBox)
            Dim tbValorMax As TextBox = CType(fila.FindControl("txtValorM"), TextBox)

            tbValorMax.Enabled = Valor
            tbValorMin.Enabled = Valor
        Next
    End Sub

    Protected Sub txtLImiteEnDetalleMin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLImiteEnDetalleMin.TextChanged
        For Each fila As GridViewRow In dgListaValores.Rows
            If Not CType(fila.FindControl("chkValorEspecifico"), CheckBox).Checked Then
                Dim tbValorMin As TextBox = CType(fila.FindControl("txtValor"), TextBox)
                tbValorMin.Text = txtLImiteEnDetalleMin.Text
            End If
        Next
    End Sub

    Protected Sub txtLImiteEnDetalleMax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLImiteEnDetalleMax.TextChanged
        For Each fila As GridViewRow In dgListaValores.Rows
            If Not CType(fila.FindControl("chkValorEspecifico"), CheckBox).Checked Then
                Dim tbValorMax As TextBox = CType(fila.FindControl("txtValorM"), TextBox)
                tbValorMax.Text = txtLImiteEnDetalleMax.Text
            End If
        Next
    End Sub

    Protected Sub chkValorFijo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Tablas As DataTable = dgListaValores.DataSource
        Dim rowIndex As Integer = DirectCast(DirectCast(sender, System.Web.UI.WebControls.CheckBox).DataItemContainer, System.Web.UI.WebControls.GridViewRow).RowIndex

        Dim tbValorMin As TextBox = CType(dgListaValores.Rows(rowIndex).FindControl("txtValor"), TextBox)
        Dim tbValorMax As TextBox = CType(dgListaValores.Rows(rowIndex).FindControl("txtValorM"), TextBox)

        tbValorMin.Enabled = DirectCast(sender, System.Web.UI.WebControls.CheckBox).Checked
        tbValorMax.Enabled = DirectCast(sender, System.Web.UI.WebControls.CheckBox).Checked

        tbValorMin.ReadOnly = Not DirectCast(sender, System.Web.UI.WebControls.CheckBox).Checked
        tbValorMax.ReadOnly = Not DirectCast(sender, System.Web.UI.WebControls.CheckBox).Checked
    End Sub

    'Protected Sub dgListaValores_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaValores.RowCommand
    '    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | GTC | 12/11/2018
    '    Dim gvr As GridViewRow = Nothing3
    '    gvr = CType(CType(e.CommandSource, CheckBox).NamingContainer, GridViewRow)

    '    If e.CommandName = "ValorEspecifico" Then
    '        Dim chkValorEspecifico As CheckBox = CType(gvr.FindControl("chkValorEspecifico"), CheckBox)
    '        Dim tbValorMin As TextBox = CType(gvr.FindControl("txtValor"), TextBox)
    '        Dim tbValorMax As TextBox = CType(gvr.FindControl("txtValorM"), TextBox)

    '        If chkValorEspecifico.Checked Then
    '            tbValorMin.Enabled = True
    '            tbValorMax.Enabled = True
    '        Else
    '            tbValorMin.Enabled = False
    '            tbValorMax.Enabled = False
    '        End If

    '    End If
    '    'FIN | PROYECTO FONDOS II - ZOLUXIONES | GTC | 12/11/2018
    'End Sub

    Protected Sub btnAgregarCondicion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarCondicion.Click

        If txtCampoCon.Text.Equals("") Then
            AlertaJS("Ingrese el Campo de la Condición")
            Exit Sub
        End If

        If txtValorCon.Text.Equals("") Then
            AlertaJS("Ingrese el Valor de la Condición")
            Exit Sub
        End If

        Dim strCondicion As String = ViewState("Condicion")
        strCondicion = lblCondicion.Text

        If strCondicion.Equals("") Then
            strCondicion = strCondicion + " " + txtCampoCon.Text + " = '" + txtValorCon.Text + "'"
        Else
            strCondicion = strCondicion + " AND " + txtCampoCon.Text + " = '" + txtValorCon.Text + "'"
        End If

        txtCampoCon.Text = ""
        txtValorCon.Text = ""
        lblCondicion.Text = strCondicion

        ViewState("Condicion") = strCondicion
    End Sub

    Protected Sub btnGrabarCondicion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabarCondicion.Click


        If txtValorMaxCon.Text < 0 Then
            AlertaJS("Debe ingresar un Valor Maximo mayor a 0")
            Exit Sub
        End If

        If lblCondicion.Text.Equals("") Then
            AlertaJS("Debe de ingresar una condición.")
            Exit Sub
        End If

        Dim dtblCondicion As DataTable = Session("dtblCondicion")
        Dim rowCondicion As DataRow = dtblCondicion.NewRow

        rowCondicion("CodigoNivelLimite") = hdCodigoLimiteNivel.Value
        rowCondicion("Secuencial") = dtblCondicion.Rows.Count + 1
        rowCondicion("Condicion") = ViewState("Condicion")
        rowCondicion("PorcentajeMin") = IIf(txtValorMinCon.Text.Equals(""), 0, txtValorMinCon.Text)
        rowCondicion("PorcentajeMax") = IIf(txtValorMaxCon.Text.Equals(""), 0, txtValorMaxCon.Text)

        dtblCondicion.Rows.Add(rowCondicion)

        Session("dtblCondicion") = dtblCondicion

        lblCondicion.Text = ""
        txtValorMaxCon.Text = ""
        txtValorMinCon.Text = ""

        dgGrillaCondicion.DataSource = dtblCondicion
        dgGrillaCondicion.DataBind()
    End Sub

    Protected Sub ddlCaracteristicaAnterior_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCaracteristicaAnterior.SelectedIndexChanged
        CargarGrillaValores(Me.rblOpcion.SelectedValue.ToString)
    End Sub
End Class