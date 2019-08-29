Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Partial Class Modulos_Parametria_Tablas_Valores_frmLimites
    Inherits BasePage
#Region " Código generado por el Diseñador de Web Forms "
    Protected DtDetalle As New DataTable
    Protected DtPortafolios As New DataTable
    Protected DtNegocios As New DataTable
    Protected DtDetalleEliminado As New DataTable
    Protected ListaCaracteristicas As New Hashtable
    Protected ListaDetalleNivelLimite As New Hashtable
    Protected ListaPortafolioSituacion As New Hashtable
    Private designerPlaceholderDeclaration As System.Object
#End Region
#Region " /* Declaración de Variables */ "
    Private campos() As String = {"CodigoLimiteCaracteristica", "CodigoLimite", "Tipo", "Situacion"}
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String"}
    Private camposListBox() As String = {"Codigo", "Descripcion"}
    Private tiposListBox() As String = {"System.String", "System.String"}
    Private camposDetalleNivelLimite() As String = {"Selected", "CodigoNivelLimite", "CodigoCaracteristica", "NombreVista", "ValorCaracteristica", "DescripcionValorCaracteristica", "ValorPorcentaje", "Situacion"}
    Private tiposDetalleNivelLimite() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String", "System.Decimal", "System.String"}
    Dim strmensaje As String = ""
    Dim dtDetalleNivel As New DataTable
    Private ddlCombo As DropDownList
    Private hdOculto As HiddenField
    Private dtSituacionPortafolio As DataTable = New ParametrosGeneralesBM().ListarSituacion(Nothing)
#End Region

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oLimiteBM As New LimiteBM
        If Not Page.IsPostBack Then
            Try
                'CargarLoading("btnAceptar")
                CargarCombos()
                CargaRadioButtonGroups() ' MPENAL - 22/09/16
                Dim dtblGenerico As DataTable = UIUtility.GetStructureTablebase(campos, tipos)
                dgLista.DataSource = dtblGenerico : dgLista.DataBind()
                Me.DtDetalleEliminado = DtDetalle.Clone
                Session("ArregloCaracteristicas") = Nothing
                Session("DetalleNivelLimite") = Nothing
                ViewState("CaracteristicasDetalle") = Nothing
                If Not (Request.QueryString("cod") = Nothing) Then
                    Me.hd.Value = Request.QueryString("cod")
                    Me.tbCodigo.ReadOnly = True
                    Me.btnExportar.Visible = True
                    CargarRegistro(Me.hd.Value.Trim())
                    dgLiquidez.Visible = False
                Else
                    Me.rdbCuadrar.SelectedValue = 0 ' MPENAL - 22/09/16
                    DtDetalle = oLimiteBM.SeleccionarCaracteristicas("", DatosRequest)
                    ViewState("CaracteristicasDetalle") = DtDetalle
                    Me.hd.Value = ""
                    Me.txtValorPorcPorAgrup.Attributes.Add("readonly", "readonly") '' MPENAL - 19/09/16
                End If
                If Not ViewState("NivelDetalle") = Nothing Then
                    dtDetalleNivel = ViewState("NivelDetalle")
                End If
            Catch ex As Exception
                AlertaJS("Ocurrió un error al cargar la página")
            End Try
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            '' INI MPENAL - 19/09/16
            Dim valorPorc As Decimal = Convert.ToDecimal(Me.txtValorPorcPorAgrup.Text)
            If Me.chbValorPorcentajePorAgrup.Checked And valorPorc = 0 Then
                AlertaJS("El valor de % Agrupador es obligatorio", "$('#txtValorPorcPorAgrup').focus();")
                Me.txtValorPorcPorAgrup.Attributes.Remove("readonly")
                Return
            Else
                Me.txtValorPorcPorAgrup.Attributes.Add("readonly", "readonly")
            End If
            '' FIN MPENAL - 19/09/16

            Dim oLimiteBM As New LimiteBM
            Dim oLimiteBE As New LimiteBE
            Dim blnExisteItem As Boolean
            Dim Portafolio As New PortafolioBM
            Me.ListaCaracteristicas = CType(Session("ArregloCaracteristicas"), Hashtable)
            Me.ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
            For Each dtrow As GridViewRow In dgLista.Rows
                Me.ListaPortafolioSituacion.Add(dtrow.Cells(6).Text, CType(dtrow.FindControl("ddlSituacionPortafolio"), DropDownList).SelectedValue)
            Next
            DtDetalle = oLimiteBM.SeleccionarCaracteristicas("", DatosRequest)
            Dim drCaracteristica As DataRow
            Dim Total As Integer
            If IsNothing(ListaCaracteristicas) Then
                Total = 0
            Else
                Total = ListaCaracteristicas.Count
            End If
            Dim array(Total - 1) As Object
            Dim oListaValidadorLimiteDetalle As ListValidadorLimite
            If (ListaCaracteristicas IsNot Nothing) Then

                ListaCaracteristicas.CopyTo(array, 0)

            End If
            For Each elemento As DictionaryEntry In array
                If Not elemento.Key = 0 Then
                    drCaracteristica = DtDetalle.NewRow
                    Dim dtTemp As DataTable = ListaCaracteristicas.Item(elemento.Key)
                    drCaracteristica.Item("CodigoLimiteCaracteristica") = dtTemp(0)("CodigoLimiteCaracteristica")
                    drCaracteristica.Item("CodigoLimite") = tbCodigo.Text
                    drCaracteristica.Item("Tipo") = ddlTipoLimite.SelectedValue
                    drCaracteristica.Item("CodigoPortafolioSBS") = elemento.Key
                    'drCaracteristica.Item("Situacion") = ddlSituacion.SelectedItem.Text
                    drCaracteristica.Item("Situacion") = ListaPortafolioSituacion.Item(IIf(IsNumeric(elemento.Key), elemento.Key.ToString, elemento.Key))
                    DtDetalle.Rows.Add(drCaracteristica)
                End If
            Next
            If Not (Me.DtDetalle Is Nothing) Then
                Dim dtblCondiciones As DataTable = Session("dtblCondicion")
                If dtblCondiciones Is Nothing Then dtblCondiciones = New DataTable

                If (Me.hd.Value = "") Then
                    'Si no tiene nada es porque es un nuevo registro
                    Dim StrId As String = String.Empty

                    blnExisteItem = ExisteEntidad()
                    If Not blnExisteItem Then
                        oLimiteBE = crearObjeto()
                        oListaValidadorLimiteDetalle = Me.ObtenerInstanciaValidadorLimite()
                        StrId = oLimiteBM.Insertar(oLimiteBE, oListaValidadorLimiteDetalle, DatosRequest)
                        oLimiteBM.InsertarCaracteristicas(tbCodigo.Text, ddlSituacion.SelectedValue.ToString, DtDetalle, ListaCaracteristicas, ListaDetalleNivelLimite, DatosRequest, PORTAFOLIO_MULTIFONDOS, dtblCondiciones)
                        ModificarPorcentajeCercaLimite()
                        AlertaJS("Los Datos Fueron Grabados Correctamente")
                        LimpiarCampos()
                    Else
                        AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                    End If
                Else
                    oLimiteBE = crearObjeto()
                    oListaValidadorLimiteDetalle = Me.ObtenerInstanciaValidadorLimite()
                    oLimiteBM.Modificar(oLimiteBE, oListaValidadorLimiteDetalle, DatosRequest)
                    Dim CodigoRelacion As String
                    oLimiteBM.ModificarCaracteristicas(Me.hd.Value, ddlSituacion.SelectedValue.ToString, DtDetalle, ListaCaracteristicas, ListaDetalleNivelLimite, DatosRequest, PORTAFOLIO_MULTIFONDOS, dtblCondiciones, CodigoRelacion)
                    ModificarPorcentajeCercaLimite()
                    AlertaJS("Los Datos Fueron Modificados Correctamente")
                    CargarRegistro(Me.hd.Value.Trim())
                End If
            Else
                EjecutarJS("CloseProgress();")
                AlertaJS("Debe Ingresar Mínimo una Característica")
            End If
        Catch ex As Exception
            EjecutarJS("CloseProgress();")
            AlertaJS("Ocurrió un error al Aceptar : " + Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaLimites.aspx")
        Catch ex As Exception
            EjecutarJS("CloseProgress();")
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            '' INI MPENAL - 19/09/16
            If Me.chbValorPorcentajePorAgrup.Checked Then
                Me.txtValorPorcPorAgrup.Attributes.Remove("readonly")
            Else
                Me.txtValorPorcPorAgrup.Attributes.Add("readonly", "readonly")
            End If
            '' FIN MPENAL - 19/09/16

            Session("FilaSeleccionada") = 1
            Dim grupoInstrumento, claseLimite As String
            Me.DtDetalle = ViewState("CaracteristicasDetalle")
            If Not (Session("ArregloCaracteristicas") Is Nothing) Then
                Me.ListaCaracteristicas = CType(Session("ArregloCaracteristicas"), Hashtable)
            Else
                Me.ListaCaracteristicas = New Hashtable
                InicializarLista()
            End If
            Session("ArregloCaracteristicas") = ListaCaracteristicas
            claseLimite = ddlclaselimite.SelectedValue
            Dim CodigoLimite As String = ""
            If Not (Request.QueryString("cod") = Nothing) Then
                CodigoLimite = Request.QueryString("cod")
            End If
            grupoInstrumento = ddlPosicion.SelectedValue.ToString
            ddlclaselimite.Enabled = False
            Dim TopeLimite As String = ""
            TopeLimite = ddlTopeLimite.SelectedValue
            EjecutarJS(UIUtility.MostrarPopUp("frmLimiteNivel.aspx?CodigoLimite=" + CodigoLimite + "&claseLimite=" + claseLimite + "&codGrupo=" + grupoInstrumento + "&TopeLimite=" + TopeLimite, "no", 750, 650, 0, 0, "no", "yes", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If CType(e.CommandSource, ImageButton).CommandName = "Modificar" Then
                Dim CodigoLimiteCaracteristica, grupoInstrumento, tipo, portafolio As String
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim numFila As Integer = Row.RowIndex
                DtDetalle = ViewState("CaracteristicasDetalle")
                CodigoLimiteCaracteristica = DtDetalle.Rows(numFila).Item("CodigoLimiteCaracteristica").ToString
                tipo = dgLista.Rows(numFila).Cells(4).Text
                portafolio = dgLista.Rows(numFila).Cells(5).Text
                grupoInstrumento = ddlPosicion.SelectedValue.ToString
                Session("FilaSeleccionada") = DtDetalle.Rows(numFila).Item("CodigoPortafolioSBS").ToString
                Dim TopeLimite As String = ddlTopeLimite.SelectedValue
                EjecutarJS(UIUtility.MostrarPopUp("frmLimiteNivel.aspx?CodigoLimite=" + tbCodigo.Text + "&CodigoLimiteCaracteristica=" + CodigoLimiteCaracteristica + "&tipo=" + tipo + "&portafolio=" + portafolio + "&TopeLimite=" + TopeLimite + "&codGrupo=" + grupoInstrumento, "no", 750, 650, 0, 0, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la operación de la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If (e.Row.Cells(4).Text = "I") Then
                    e.Row.Cells(4).Text = "Interno"
                ElseIf (e.Row.Cells(4).Text = "L") Then
                    e.Row.Cells(4).Text = "Segun Ley"
                End If

                If (e.Row.Cells(5).Text.ToUpper = "MIN") Then
                    e.Row.Cells(5).Text = "Minimo"
                ElseIf (e.Row.Cells(5).Text.ToUpper = "MAX") Then
                    e.Row.Cells(5).Text = "Maximo"
                End If
                hdOculto = CType(e.Row.FindControl("hdSituacion"), HiddenField)
                ddlCombo = CType(e.Row.FindControl("ddlSituacionPortafolio"), DropDownList)
                HelpCombo.LlenarComboBox(Me.ddlCombo, dtSituacionPortafolio, "Valor", "Nombre", False)
                ddlCombo.SelectedValue = IIf(UCase(hdOculto.Value) = "ACTIVO", "A", "I")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub

    Private Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            EjecutarJS("CloseProgress();")
            GenerarReporteEstructuraLimites()
        Catch ex As Exception
            EjecutarJS("CloseProgress();")
            Me.AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub chkLimiteVinculado_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLimiteVinculado.CheckedChanged
        Try
            If chkLimiteVinculado.Checked Then
                txtPorcentajeVinculado.ReadOnly = False
            Else
                txtPorcentajeVinculado.ReadOnly = True
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub


#End Region

#Region " /* Funciones Personalizadas */"
    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim tablaTipoCalculo As New DataTable
        Dim tablaUnidadPosicion As New DataTable
        Dim tablaValorBase As New DataTable
        Dim tablaClaseLimite As New DataTable
        Dim tablaAplicarCastigo As New DataTable
        Dim tablaTipoFactor As New DataTable
        Dim tablaSaldoBanco As New DataTable
        Dim tablaPortafolio As New DataTable
        Dim tablaNegocio As New DataTable
        Dim tablaTipoLimite As New DataTable
        Dim tablaTopeLimite As New DataTable
        Dim tablaPosicion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oPortafolioBM As New PortafolioBM
        Dim oNegocioBM As New NegocioBM
        Dim oGrupoInstrumentoBM As New GrupoInstrumentoBM
        Dim tablaValidadorLimite As New DataTable
        Dim oParamGBM As New ParametrosGeneralesBM
        Dim vClasificacion As String = "TipoCuenta"
        Dim TablaTipoCuenta As DataTable = oParamGBM.Listar(vClasificacion, Nothing)

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaTipoCalculo = oParametrosGenerales.ListarTipoCalculo(DatosRequest)
        tablaUnidadPosicion = oParametrosGenerales.ListarUnidadPosicion(DatosRequest)
        tablaValorBase = oParametrosGenerales.ListarValorBase(DatosRequest)
        tablaClaseLimite = oParametrosGenerales.ListarClaseLimite(DatosRequest)
        tablaAplicarCastigo = oParametrosGenerales.ListarAplicarCastigo(DatosRequest)
        tablaTipoFactor = oParametrosGenerales.ListarTipoFactor(DatosRequest)
        tablaSaldoBanco = oParametrosGenerales.ListarSaldoBanco(DatosRequest)
        tablaTipoLimite = oParametrosGenerales.ListarTipoLimite(DatosRequest)
        tablaTopeLimite = oParametrosGenerales.ListarTopeLimite(DatosRequest)
        tablaPortafolio = oPortafolioBM.Listar(DatosRequest, "").Tables(0)
        tablaNegocio = oNegocioBM.Listar(DatosRequest).Tables(0)
        tablaPosicion = oGrupoInstrumentoBM.SeleccionarPorFiltro("", "", "", DatosRequest).Tables(0)
        tablaValidadorLimite = oParametrosGenerales.Listar("ValLimite", DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(Me.ddlcastigo, tablaAplicarCastigo, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlclaselimite, tablaClaseLimite, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlfactor, tablaTipoFactor, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlSaldobanco, tablaSaldoBanco, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlunidadposicion, tablaUnidadPosicion, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlvalorbase, tablaValorBase, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddltipocalculo, tablaTipoCalculo, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlTipoLimite, tablaTipoLimite, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlTopeLimite, tablaTopeLimite, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlPosicion, tablaPosicion, "CodigoGrupoInstrumento", "Descripcion", True)
        ddlPosicion.Items.Insert(1, New ListItem("Todos", "GI000"))
        HelpCombo.LlenarListBox(lbValidadores, tablaValidadorLimite, "Valor", "Nombre", False)
        HelpCombo.LlenarListBox(lbTipoCuenta, TablaTipoCuenta, "Valor", "Comentario", False)

    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oLimiteBM As New LimiteBM
        Dim oLimiteBE As New LimiteBE
        oLimiteBE = oLimiteBM.Seleccionar(Me.tbCodigo.Text.Trim(), Me.DatosRequest)
        Return oLimiteBE.Limite.Rows.Count > 0
    End Function

    ' INI MPENAL - 22/09/16
    Private Sub CargaRadioButtonGroups()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtDatosCuadrar As DataTable = oParametrosGenerales.SeleccionarPorFiltro("L_Cuadrar", "", "", "A", Me.DatosRequest)
        If dtDatosCuadrar IsNot Nothing And dtDatosCuadrar.Rows.Count > 0 Then
            For Each drow As DataRow In dtDatosCuadrar.AsEnumerable().OrderBy(Function(r) r.Field(Of String)("Valor")).CopyToDataTable().Rows
                Me.rdbCuadrar.Items.Add(New ListItem(drow("Nombre"), drow("Valor")))
            Next
        End If
    End Sub
    ' FIN MPENAL - 22/09/16

    Private Sub ActualizarListBox(ByVal lbxOrigen As ListBox, ByVal lbxSeleccion As ListBox)
        Dim j As Integer
        Dim dtAux As New DataTable
        Dim row As DataRow
        dtAux = UIUtility.GetStructureTablebase(camposListBox, tiposListBox)
        j = 0
        While (j < lbxOrigen.Items.Count)
            If (lbxSeleccion.Items.Contains(lbxOrigen.Items.Item(j)) = False) Then
                row = dtAux.NewRow
                row.Item("Codigo") = lbxOrigen.Items.Item(j).Value
                row.Item("Descripcion") = lbxOrigen.Items.Item(j).Text
                dtAux.Rows.Add(row)
            End If
            j = j + 1
        End While
        lbxOrigen.Items.Clear()
        HelpCombo.LlenarListBox(lbxOrigen, dtAux, "Codigo", "Descripcion", False)
    End Sub
    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim DtNivel As New DataTable
        Dim oLimiteBM As New LimiteBM
        Dim oLimiteBE As New LimiteBE
        Dim oLimiteCaracteristicaBE As New LimiteCaracteristicasBE
        Dim oRow As LimiteBE.LimiteRow
        Dim StrNulo As New SqlString
        oLimiteBE = oLimiteBM.Seleccionar(Codigo, DatosRequest)
        DtNegocios = oLimiteBM.SeleccionarNegocios(Codigo, DatosRequest)
        DtDetalle = SeleccionarCaracteristicas(Codigo)
        ViewState("CaracteristicasDetalle") = DtDetalle
        oRow = DirectCast(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
        Me.tbNombreLimite.Text = oRow.NombreLimite.ToString()
        '''' INI MPENAL - 13/09/16
        Me.chbValorPorcentajePorAgrup.Checked = oRow.IsAgrupadoPorcentaje
        Me.txtValorPorcPorAgrup.Text = oRow.ValorAgrupadoPorcentaje
        If oRow.IsAgrupadoPorcentaje Then
            'Me.txtValorPorcPorAgrup.ReadOnly = False
            Me.txtValorPorcPorAgrup.Attributes.Remove("readonly")
        Else
            'Me.txtValorPorcPorAgrup.ReadOnly = True
            Me.txtValorPorcPorAgrup.Attributes.Add("readonly", "readonly")
        End If
        '''' FIN MPENAL - 13/09/16
        Me.rdbCuadrar.SelectedValue = oRow.Cuadrar  ' MPENAL - 22/09/16

        If oRow.MarketShare > 0 Then
            tbMarketShare.Text = oRow.MarketShare
        End If
        If oRow.AplicarCastigo.Equals(String.Empty) Then
            Me.ddlcastigo.SelectedIndex = 0
        Else
            Me.ddlcastigo.SelectedValue = oRow.AplicarCastigo
        End If
        If oRow.Tope.Equals(String.Empty) Then
            Me.ddlTopeLimite.SelectedIndex = 0
        Else
            Me.ddlTopeLimite.SelectedValue = oRow.Tope
        End If
        If oRow.ClaseLimite.Equals(String.Empty) Then
            Me.ddlclaselimite.SelectedIndex = 0
        Else
            Me.ddlclaselimite.SelectedValue = oRow.ClaseLimite
        End If
        If (oRow.TipoFactor.Equals(String.Empty)) Then
            Me.ddlfactor.SelectedIndex = 0
        Else
            Me.ddlfactor.SelectedValue = oRow.TipoFactor
        End If
        If (oRow.UnidadPosicion.Equals(String.Empty)) Then
            Me.ddlunidadposicion.SelectedIndex = 0
        Else
            Me.ddlunidadposicion.SelectedValue = oRow.UnidadPosicion
        End If
        If (oRow.ValorBase.Equals(String.Empty)) Then
            Me.ddlvalorbase.SelectedIndex = 0
        Else
            Me.ddlvalorbase.SelectedValue = oRow.ValorBase
        End If
        If (oRow.SaldoBanco.Equals(String.Empty)) Then
            Me.ddlSaldobanco.SelectedIndex = 0
        Else
            Me.ddlSaldobanco.SelectedValue = oRow.SaldoBanco
        End If
        If (oRow.TipoCalculo.Equals(String.Empty)) Then
            Me.ddltipocalculo.SelectedIndex = 0
        Else
            Me.ddltipocalculo.SelectedValue = oRow.TipoCalculo
        End If
        If (oRow.Posicion.Equals(String.Empty)) Then
            Me.ddlPosicion.SelectedIndex = 0
        Else
            Me.ddlPosicion.SelectedValue = oRow.Posicion
        End If
        If oRow.TipoLimite.Equals(String.Empty) Then
            Me.ddlTipoLimite.SelectedIndex = 0
        Else
            Me.ddlTipoLimite.SelectedValue = oRow.TipoLimite
        End If
        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        Me.hd.Value = oRow.CodigoLimite.ToString()
        Me.tbCodigo.Text = oRow.CodigoLimite.ToString()
        If DtDetalle.Rows.Count = 0 Then
        Else
            ViewState("CaracteristicasDetalle") = DtDetalle
            Me.dgLista.DataSource = DtDetalle
            Me.dgLista.DataBind()
        End If

        Dim cont As Integer
        cont = 0
        Me.ListaCaracteristicas = New Hashtable
        InicializarLista()
        For Each filaLinea As DataRow In DtDetalle.Rows
            DtNivel = oLimiteBM.SeleccionarCaracteristicasNiveles(filaLinea.Item("CodigoLimiteCaracteristica").ToString, DatosRequest)
            If DtNivel.Rows.Count <> 0 Then
                ListaCaracteristicas.Add(Convert.ToInt32(filaLinea("CodigoPortafolioSBS")), DtNivel)
                Session("ArregloCaracteristicas") = ListaCaracteristicas
                Dim dtDetalleNivelLimite As New DataTable
                Dim dtDetalleCaracteristicas As New DataTable
                Dim iDetalle As Integer = 0
                Dim claveDetalleNivelLimite As String
                For Each row As DataRow In DtNivel.Rows
                    If (row.Item("FlagTipoPorcentaje").ToString <> "G") Then
                        dtDetalleNivelLimite = oLimiteBM.SeleccionarCaracteristicasDetalleNiveles(row.Item("CodigoNivelLimite").ToString, row.Item("FlagTipoPorcentaje").ToString, DatosRequest)
                        If dtDetalleNivelLimite.Rows.Count <> 0 Then
                            If Not (Session("DetalleNivelLimite") Is Nothing) Then
                                ListaDetalleNivelLimite = CType(Session("DetalleNivelLimite"), Hashtable)
                            Else
                                Me.ListaDetalleNivelLimite = New Hashtable
                                InicializarListaDetalleNivelLimite()
                            End If
                            claveDetalleNivelLimite = filaLinea("CodigoPortafolioSBS").ToString() & "," & row.Item("Secuencial").ToString() & "," & row("CodigoCaracteristica")
                            ListaDetalleNivelLimite.Add(claveDetalleNivelLimite, dtDetalleNivelLimite)
                            Session("DetalleNivelLimite") = ListaDetalleNivelLimite
                        End If
                        iDetalle = iDetalle + 1
                    End If
                Next
            End If
            cont += 1
        Next
        If oRow.IsLimiteVinculado = "S" Then
            chkLimiteVinculado.Checked = True
            txtPorcentajeVinculado.ReadOnly = False
        Else
            chkLimiteVinculado.Checked = False
            txtPorcentajeVinculado.ReadOnly = True
        End If

        If oRow.AplicaForward = "S" Then
            chkAplicaForward.Checked = True
        Else
            chkAplicaForward.Checked = False
        End If

        If oRow.TieneCastigo = "S" Then
            chkCastigoRating.Checked = True
            txtCastigoRating.ReadOnly = False
        Else
            chkCastigoRating.Checked = False
            txtCastigoRating.ReadOnly = True
        End If

        txtPorcentajeVinculado.Text = oRow.PorcLimiteVinculado
        txtCastigoRating.Text = oRow.CastigoRating
        CargarValidadorLimite(Me.tbCodigo.Text)
    End Sub
    Private Function Descripcion(ByVal tipo As String) As String
        Dim sDescripcion As String = ""
        If (tipo.ToUpper = "I") Then
            sDescripcion = "Interno"
        ElseIf (tipo.ToUpper = "L") Then
            sDescripcion = "Segun Ley"
        End If
        Return sDescripcion
    End Function
    Public Function VerificarNiveles(ByVal DtDetalle As DataTable, ByVal Lista As Hashtable) As String
        Dim msg As String = ""
        Dim strMensajeError As String = ""
        Dim Fila As Integer
        Dim dtNivel As DataTable
        If Not (Session("ArregloCaracteristicas") Is Nothing) Then
            For x As Int32 = 0 To DtDetalle.Rows.Count - 1
                dtNivel = CType(Lista(x), DataTable)
                If dtNivel Is Nothing Then
                    Fila = x + 1
                    msg += "\t-Fila " & Fila & "\n"
                End If
            Next
            If (msg <> "") Then
                strMensajeError = "Falta Ingresar Nivel(es) a la(s) Característica(s) de la : \n " + msg + "\n"
                Return strMensajeError
            Else
                Return ""
            End If
        Else
            strMensajeError = "Falta Ingresar Niveles a las Características\n "
            Return strMensajeError
        End If
    End Function
    Public Function VerificarFilaDistinta(ByVal StrTipoLim As String, ByVal StrTopeLim As String) As Boolean
        Dim StrTipo, StrTope As String
        Dim BlnEsta As Boolean = False
        Dim numFila, cont As Integer
        numFila = CType(Session("FilaSeleccionada"), Integer)
        cont = 0
        For Each filaLinea As DataRow In DtDetalle.Rows
            If numFila = cont Then
                StrTipo = filaLinea("TipoLimite").ToString().Trim()
                StrTope = filaLinea("TopeLimite").ToString().Trim()
                If (StrTipo = StrTipoLim) And (StrTope = StrTopeLim) Then
                    BlnEsta = False
                    Exit For
                End If
            Else
                StrTipo = filaLinea("TipoLimite").ToString().Trim()
                StrTope = filaLinea("TopeLimite").ToString().Trim()

                If (StrTipo = StrTipoLim) And (StrTope = StrTopeLim) Then
                    BlnEsta = True
                    Exit For

                End If
            End If
            cont += 1
        Next

        If BlnEsta Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub InicializarDetalle()
        Me.DtDetalle = New DataTable
        Me.DtDetalle.Columns.Add("CodigoLimiteCaracteristica", GetType(String))
        Me.DtDetalle.Columns.Add("codigoLimite", GetType(String))
        Me.DtDetalle.Columns.Add("Tipo", GetType(String))
        Me.DtDetalle.Columns.Add("Tope", GetType(String))
        Me.DtDetalle.Columns.Add("CodigoPortafolioSBS", GetType(String))
        Me.DtDetalle.Columns.Add("Situacion", GetType(String))
        Me.DtDetalle.GetChanges()
        Me.dgLista.DataSource = DtDetalle
        Me.dgLista.DataBind()
    End Sub
    Public Sub InicializarLista()
        Me.ListaCaracteristicas = New Hashtable
        ListaCaracteristicas.Clear()
        Session("DetalleNivelLimite") = Nothing
    End Sub
    Public Sub InicializarListaDetalleNivelLimite()
        Me.ListaDetalleNivelLimite = New Hashtable
        ListaDetalleNivelLimite.Clear()
    End Sub
    Public Sub LimpiarCamposCaracteristicas()
        Me.ddlTipoLimite.SelectedIndex = 0
    End Sub
    Public Sub LimpiarCampos()
        Me.ddlTipoLimite.SelectedValue = ""
        Me.ddlSituacion.SelectedIndex = 0
        Me.tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbNombreLimite.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.chbValorPorcentajePorAgrup.Checked = False   '' MPENAL - 13/09/16
        Me.txtValorPorcPorAgrup.Text = ""               '' MPENAL - 13/09/16
        'RGF 20080819
        Me.tbMarketShare.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlcastigo.SelectedIndex = 0
        Me.ddlclaselimite.SelectedValue = ""
        'Me.txtFactorCastigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlfactor.SelectedValue = ""
        Me.ddlSaldobanco.SelectedValue = ""
        Me.ddltipocalculo.SelectedValue = ""
        Me.ddlTopeLimite.SelectedValue = ""
        Me.ddlunidadposicion.SelectedValue = ""
        Me.ddlvalorbase.SelectedValue = ""
        Me.ddlPosicion.SelectedValue = ""
        'Me.lbxSeleccionPortafolio.Items.Clear()
        InicializarDetalle()
        InicializarLista()
        InicializarListaDetalleNivelLimite()
        Session("FilaSeleccionada") = -1
        Session("GrillaDetalle") = Nothing
        Session("dtDetalleEliminar") = Nothing
        Session("ArregloCaracteristicas") = Nothing
        DtDetalle = New LimiteBM().SeleccionarCaracteristicas("", DatosRequest) 'oLimiteBM.SeleccionarCaracteristicas("", DatosRequest)
        ViewState("CaracteristicasDetalle") = DtDetalle
        Me.hd.Value = ""
        ViewState("NivelDetalle") = Nothing
        dgCercaLimite.DataSource = Nothing
        dgCercaLimite.DataBind()
    End Sub
    Private Function crearObjeto() As LimiteBE
        Dim oLimiteBE As New LimiteBE
        Dim oRow As LimiteBE.LimiteRow
        oRow = DirectCast(oLimiteBE.Limite.NewRow(), LimiteBE.LimiteRow)
        oRow.CodigoLimite = Me.tbCodigo.Text.Trim()
        oRow.NombreLimite = Me.tbNombreLimite.Text.ToUpper
        If Microsoft.VisualBasic.IsNumeric(tbMarketShare.Text.Trim) Then
            oRow.MarketShare = tbMarketShare.Text
        Else
            oRow.MarketShare = Nothing
        End If
        oRow.TipoCalculo = Me.ddltipocalculo.SelectedValue()
        oRow.UnidadPosicion = Me.ddlunidadposicion.SelectedValue()
        oRow.ValorBase = Me.ddlvalorbase.SelectedValue()
        oRow.ClaseLimite = Me.ddlclaselimite.SelectedValue()
        oRow.TipoLimite = ddlTipoLimite.SelectedValue
        oRow.CodigoPortafolio = ""
        oRow.AplicarCastigo = Me.ddlcastigo.SelectedValue()
        If Me.ddlfactor.SelectedIndex = 0 Then
            oRow.TipoFactor = ""
        Else
            oRow.TipoFactor = Me.ddlfactor.SelectedValue()
        End If
        oRow.SaldoBanco = Me.ddlSaldobanco.SelectedValue()
        oRow.Posicion = Me.ddlPosicion.SelectedValue()
        oRow.Situacion = Me.ddlSituacion.SelectedValue()
        oRow.Tope = Me.ddlTopeLimite.SelectedValue()
        oRow.IsAgrupadoPorcentaje = Me.chbValorPorcentajePorAgrup.Checked   '' MPENAL - 13/09/16
        oRow.ValorAgrupadoPorcentaje = System.Convert.ToDecimal(IIf(String.IsNullOrEmpty(txtValorPorcPorAgrup.Text), 0, txtValorPorcPorAgrup.Text))            '' MPENAL - 13/09/16
        oRow.ValorAgrupadoPorcentaje = IIf(oRow.IsAgrupadoPorcentaje, oRow.ValorAgrupadoPorcentaje, 0) '' MPENAL - 13/09/16
        oRow.Cuadrar = Me.rdbCuadrar.SelectedValue '' MPENAL - 22/09/16
        If chkLimiteVinculado.Checked Then
            oRow.PorcLimiteVinculado = Decimal.Parse(txtPorcentajeVinculado.Text)
            oRow.IsLimiteVinculado = "S"
        Else
            oRow.PorcLimiteVinculado = Decimal.Parse(txtPorcentajeVinculado.Text)
            oRow.IsLimiteVinculado = "N"
        End If

        If chkCastigoRating.Checked Then
            oRow.CastigoRating = Decimal.Parse(txtCastigoRating.Text)
            oRow.TieneCastigo = "S"
        Else
            oRow.CastigoRating = Decimal.Parse(txtCastigoRating.Text)
            oRow.TieneCastigo = "N"
        End If

        If chkAplicaForward.Checked Then
            oRow.AplicaForward = "S"
        Else
            oRow.AplicaForward = "N"
        End If

        oLimiteBE.Limite.AddLimiteRow(oRow)
        oLimiteBE.Limite.AcceptChanges()
        Return oLimiteBE
    End Function
    Private Sub ModificarPorcentajeCercaLimite()
        Try
            Dim oLimite As New LimiteBM
            Dim dtPortafolio As DataTable = New PortafolioBM().Listar(Nothing, "").Tables(0)
            Dim txtPorcentaje As TextBox
            For i = 0 To dgCercaLimite.Rows.Count - 1
                For j = 0 To dtPortafolio.Rows.Count - 1
                    If dtPortafolio.Rows(j)("Descripcion").ToString.Trim = dgCercaLimite.Rows(i).Cells(0).Text.Trim Then
                        txtPorcentaje = CType(dgCercaLimite.Rows(i).FindControl("txtPorcentaje"), TextBox)
                        'oLimite.ModificarPorcentajeCercaLimite(Me.tbCodigo.Text, dgCercaLimite.Rows(i).Cells(0).Text, CDec(txtPorcentaje.Text))
                        oLimite.ModificarPorcentajeCercaLimite(Me.tbCodigo.Text, dtPortafolio.Rows(j)("CodigoPortafolioSBS").ToString.Trim, CDec(txtPorcentaje.Text))
                    End If
                Next
            Next
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Private Function SeleccionarCaracteristicas(ByVal Codigo As String) As DataTable
        Dim oLimiteBM As New LimiteBM

        Dim dt As DataTable
        dt = oLimiteBM.SeleccionarCaracteristicas(Codigo, Me.DatosRequest)

        '' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-15 | Se quitó el uso de Hashtable pues genera problemas 
        ''      con el UNIQUE, en su lugar ahora se trabaja con la misma tabla de caracteristicas
        'Dim ListaCercaAlLimite As New Hashtable
        'For Each row As DataRow In dt.Rows
        '    ListaCercaAlLimite.Add(row("CodigoPortafolioSBS") & " - " & row("Descripcion"), row("PorcentajeCercaLimite"))
        'Next
        'dgCercaLimite.DataSource = ListaCercaAlLimite

        dgCercaLimite.DataSource = dt
        '' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-15

        dgCercaLimite.DataBind()
        Return dt
    End Function
    Private Function CargarFondosCercaAlLimite(ByVal TipoFondo As String) As Hashtable
        Dim ht As New Hashtable
        If TipoFondo = "F" Then
            'DtDetalle = SeleccionarCaracteristicas(Codigo)
            If Request.QueryString("cod") Is Nothing Then
                Dim Portafolio As New PortafolioBM
                Dim p As New DataTable
                p = Portafolio.PortafolioCodigoListar(ParametrosSIT.PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
                For i = 0 To p.Rows.Count - 1
                    ht.Add(p.Rows(i)(1).ToString.Trim, 1)
                Next
            Else
                DtDetalle = SeleccionarCaracteristicas(Request.QueryString("cod"))
                For i = 0 To DtDetalle.Rows.Count - 1
                    ht.Add(DtDetalle.Rows(i)(4).ToString.Trim, DtDetalle.Rows(i)("PorcentajeCercaLimite").ToString.Trim)
                Next
            End If
        Else
            ht.Add("MULTIFONDO", 1)
        End If
        Return ht
    End Function
    Private Sub GenerarReporteEstructuraLimites()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet, oSheetSBS As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtDatosGenerales As New DataTable
            Dim dtCaracteristicas As New DataTable
            Dim ds As New DataSet
            Dim oLimiteBM As New LimiteBM
            ds = oLimiteBM.ReporteEstructuraLimites(Request.QueryString("cod"), DatosRequest)
            dtDatosGenerales = ds.Tables(0)
            dtCaracteristicas = ds.Tables(1)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "PlantillaEstructuraLimites.xls"
            Dim n As Integer
            Dim dr As DataRow
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaEstructuraLimites.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oSheet.SaveAs(sFile)
            dr = dtDatosGenerales.Rows(0)
            oCells(4, 3) = dr("CodigoLimite")
            oCells(5, 3) = dr("Tipo")
            oCells(6, 3) = dr("NombreLimite")
            oCells(7, 3) = dr("UnidadPosicion")
            oCells(8, 3) = dr("TipoRenta")
            oCells(9, 3) = dr("ValorBase")
            oCells(10, 3) = dr("ClaseLimite")
            oCells(11, 3) = dr("Mercado")
            oCells(12, 3) = dr("GrupoInstrumento")
            oCells(13, 3) = dr("Situación")
            'Caracteristicas - Nivel
            oSheetSBS = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheetSBS.Cells
            oSheetSBS.SaveAs(sFile)
            n = 5
            For Each dr In dtCaracteristicas.Rows
                oCells(n, 2) = dr("OrdenNivel")
                oCells(n, 3) = dr("Portafolio")
                oCells(n, 4) = dr("Caracteristica")
                oCells(n, 5) = dr("FlagTipoPorcentaje")
                oCells(n, 6) = dr("Valor")
                oCells(n, 7) = dr("Porcentaje")
                n = n + 1
            Next
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & System.IO.Path.GetFileName(sFile))
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            Throw ex
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Public Sub CargarLiquidez(ByVal Codigo As String)
        Dim DtLiquidez As New DataTable
        Dim oLimiteBM As New LimiteBM
        DtLiquidez = oLimiteBM.ObtenerLiquidezLimite(Codigo)
        dgLiquidez.DataSource = DtLiquidez
        dgLiquidez.DataBind()
    End Sub
    Public Sub ActualizarLiquidacion(ByVal CodigoLimite As String)
        Dim oLimite As New LimiteBM
        Dim txtPorcentaje As TextBox
        Dim count As Integer = 0
        Dim PorcentajeIL As String = ""
        Dim PorcentajeLL As String = ""
        Dim PorcentajeSL As String = ""
        For Each dgRow As DataGridItem In Me.dgLiquidez.Rows
            count = count + 1
            txtPorcentaje = CType(dgRow.FindControl("txtPorcentaje0"), TextBox)
            If count = 1 Then
                PorcentajeIL = txtPorcentaje.Text
            Else
                If count = 2 Then
                    PorcentajeLL = txtPorcentaje.Text
                Else
                    PorcentajeSL = txtPorcentaje.Text
                End If
            End If
        Next
        oLimite.ActualizarParametriaLiquidez(CodigoLimite, PorcentajeIL, PorcentajeLL, PorcentajeSL)
        oLimite.ActualizarPorcentajeLiquidez(CodigoLimite)
    End Sub
    Private Function ObtenerInstanciaValidadorLimite() As ListValidadorLimite
        Dim objLDL As ValidadorLimiteDetalleBE
        Dim objListDLAux As List(Of ValidadorLimiteDetalleBE) = New List(Of ValidadorLimiteDetalleBE)
        Dim objListDL As New ListValidadorLimite
        For Each item As ListItem In lbValidadores.Items
            If item.Selected Then
                objLDL = New ValidadorLimiteDetalleBE
                objLDL.CodigoLimite = tbCodigo.Text.Trim
                objLDL.CodigoValidador = item.Value
                objLDL.Tipo = "1"
                objListDLAux.Add(objLDL)
            End If
        Next
        For Each item As ListItem In lbTipoCuenta.Items
            If item.Selected Then
                objLDL = New ValidadorLimiteDetalleBE
                objLDL.CodigoLimite = tbCodigo.Text.Trim
                objLDL.CodigoValidador = item.Value
                objLDL.Tipo = "2"
                objListDLAux.Add(objLDL)
            End If
        Next

        objListDL.objListValidadorLimite = objListDLAux
        Return objListDL
    End Function
    
    Private Sub CargarValidadorLimite(ByVal p_CodigoLimite As String)
        Dim oLimiteBM As New LimiteBM
        Dim objLimiteVLD As New ValidadorLimiteDetalleBE
        objLimiteVLD.CodigoLimite = p_CodigoLimite
        objLimiteVLD.CodigoValidador = String.Empty
        objLimiteVLD.Id = 0
        Dim dtValidadorLimiteDetalle As DataTable = oLimiteBM.SeleccionarValidadorLimiteDetalle(objLimiteVLD)
        If dtValidadorLimiteDetalle IsNot Nothing Then
            If dtValidadorLimiteDetalle.Rows.Count > 0 Then

                For Each item As ListItem In lbValidadores.Items
                    For Each dtRow As DataRow In dtValidadorLimiteDetalle.Rows
                        If (item.Value = dtRow("CodigoValidador") And dtRow("Tipo") = "1") Then
                            item.Selected = True
                            Exit For
                        End If
                    Next
                Next

                For Each item As ListItem In lbTipoCuenta.Items
                    For Each dtRow As DataRow In dtValidadorLimiteDetalle.Rows
                        If (item.Value = dtRow("CodigoValidador") And dtRow("Tipo") = "2") Then
                            item.Selected = True
                            Exit For
                        End If
                    Next
                Next

            End If
        End If
    End Sub
#Region "/* Agregar Caracteristica*/"

    Private Function ExisteCaracteristica(ByVal tipo As String, ByVal tope As String) As Boolean
        Return False
    End Function

    Private Function ExisteCaracteristicaModificar(ByVal tipo As String, ByVal indice As Integer) As Boolean
        Return False
    End Function

    Private Function AgregarCaracteristica(ByVal CodigoPortafolio As String) As Integer
        Dim dtAux As DataTable
        Dim drCaracteristica As DataRow
        dtAux = ViewState("CaracteristicasDetalle")
        If dtAux.Rows.Count > 0 Then
            Return 0
        End If
        Dim indice As Integer = -1
        If (Not ExisteCaracteristica(ddlTipoLimite.SelectedValue.ToString, ddlTopeLimite.SelectedValue.ToString)) Then
            Select Case ddlclaselimite.SelectedValue
                Case "F"
                    Dim Portafolio As New PortafolioBM
                    Dim dt As New DataTable
                    dt = Portafolio.PortafolioCodigoListar(ParametrosSIT.PORTAFOLIO_MULTIFONDOS)
                    For Each dr As DataRow In dt.Rows
                        drCaracteristica = dtAux.NewRow
                        drCaracteristica.Item("CodigoLimiteCaracteristica") = "Nuevo"
                        drCaracteristica.Item("CodigoLimite") = tbCodigo.Text
                        drCaracteristica.Item("Tipo") = ddlTipoLimite.SelectedValue
                        drCaracteristica.Item("CodigoPortafolioSBS") = dr("CodigoPortafolio")
                        drCaracteristica.Item("Situacion") = ddlSituacion.SelectedItem.Text
                        dtAux.Rows.Add(drCaracteristica)
                    Next
                Case "SF"
                    drCaracteristica = dtAux.NewRow
                    drCaracteristica.Item("CodigoLimiteCaracteristica") = "Nuevo"
                    drCaracteristica.Item("CodigoLimite") = tbCodigo.Text
                    drCaracteristica.Item("Tipo") = ddlTipoLimite.SelectedValue
                    drCaracteristica.Item("CodigoPortafolioSBS") = CodigoPortafolio
                    drCaracteristica.Item("Situacion") = ddlSituacion.SelectedItem.Text
                    dtAux.Rows.Add(drCaracteristica)
                Case Else
                    drCaracteristica = dtAux.NewRow
                    drCaracteristica.Item("CodigoLimiteCaracteristica") = "Nuevo"
                    drCaracteristica.Item("CodigoLimite") = tbCodigo.Text
                    drCaracteristica.Item("Tipo") = ddlTipoLimite.SelectedValue
                    drCaracteristica.Item("CodigoPortafolioSBS") = CodigoPortafolio
                    drCaracteristica.Item("Situacion") = ddlSituacion.SelectedItem.Text
                    dtAux.Rows.Add(drCaracteristica)
            End Select
            indice = 0
        Else
            AlertaJS("Ya existe el Tipo de Limite.")
        End If
        dgLista.DataSource = dtAux
        dgLista.DataBind()
        ViewState("CaracteristicasDetalle") = dtAux
        Return indice
    End Function

#End Region
#Region "/* Modificar Caracteristica*/"
    Public Sub ModificarCaracteristica()
        Dim numFila As Integer
        Dim dtAux As New DataTable
        numFila = CType(Session("FilaSeleccionada"), Integer)
        If (numFila <> -1) Then
            dtAux = ViewState("CaracteristicasDetalle")
            If (Not ExisteCaracteristicaModificar(ddlTipoLimite.SelectedValue.ToString, numFila)) Then
                dtAux.Rows(numFila).Item("CodigoLimite") = tbCodigo.Text
                dtAux.Rows(numFila).Item("Tipo") = ddlTipoLimite.SelectedValue
                dtAux.Rows(numFila).Item("CodigoPortafolioSBS") = ""
                dtAux.Rows(numFila).Item("Situacion") = ddlSituacion.SelectedItem.Text
            Else
                AlertaJS("Ya existe el Tipo de Limite")
            End If
            dgLista.DataSource = dtAux
            dgLista.DataBind()
            ViewState("CaracteristicasDetalle") = dtAux
            Session("FilaSeleccionada") = -1
        End If
    End Sub
#End Region
#Region "/* Verificar Valor*/"
    Public Function ValidarNumero(ByVal Cadena() As Char, ByVal Tipo As Byte) As Boolean
        ' Tipo = 1 --> Entero
        ' Tipo = 2 --> Float
        ' Tipo = 3 --> Fecha

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
                Exit Do
            End If
            Ind = Ind + 1
        Loop
        Return Estado
    End Function
#End Region

 
#End Region

End Class
