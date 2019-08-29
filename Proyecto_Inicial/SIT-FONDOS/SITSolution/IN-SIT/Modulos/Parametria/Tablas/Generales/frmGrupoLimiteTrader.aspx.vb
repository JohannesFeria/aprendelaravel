Option Explicit On
Option Strict Off

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmGrupoLimiteTrader
    Inherits BasePage

#Region "/* Propiedades */"

    Private Property VistaDetalle() As GrupoLimiteTraderDetalleBE
        Get
            Return DirectCast(ViewState("GrupoLimiteTraderDetalle"), GrupoLimiteTraderDetalleBE)
        End Get
        Set(ByVal Value As GrupoLimiteTraderDetalleBE)
            ViewState("GrupoLimiteTraderDetalle") = Value
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
            Dim strValor As String = ""
            strValor = IIf(ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI, ddlTipoInstrumento.SelectedValue, tbNemonico.Text)

            If Not VerificarGrupoLimiteTraderDetalle(Val(hd.Value), strValor) Then
                AlertaJS("Existe Tipo Instrumento o Nemonico, para este Grupo Límite")
                Exit Sub
            End If
            Agregar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al agregar el detalle")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornarDetalle.Click
        Try
            Response.Redirect("frmBusquedaGrupoLimiteTrader.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
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
        Dim blnExisteGrupoLimite As Boolean
        Dim blnDataCorrecta As Boolean

        If hd.Value.Equals(String.Empty) Then

            blnExisteGrupoLimite = ExisteGrupoLimite()

            If blnExisteGrupoLimite Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                blnDataCorrecta = ValidarDataEntry()
                If blnDataCorrecta Then
                    Insertar()
                    LimpiarControlesCabecera()
                    LimpiarControlesDetalle()
                    Cancelar()
                Else
                    AlertaJS("Debe ingresar al menos un registro en el detalle.")
                End If
            End If
        Else
            Modificar()
        End If
    End Sub

    Private Sub Insertar()
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim oGrupoLimiteTraderBE As New GrupoLimiteTraderBE

        oGrupoLimiteTraderBE = crearObjeto()
        oGrupoLimiteTraderBM.Insertar(oGrupoLimiteTraderBE, VistaDetalle, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
    End Sub

    Private Sub Modificar()
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim oGrupoLimiteTraderBE As New GrupoLimiteTraderBE

        oGrupoLimiteTraderBE = crearObjeto()
        oGrupoLimiteTraderBM.Modificar(oGrupoLimiteTraderBE, VistaDetalle, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub


    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cod") Is Nothing Then
                hd.Value = Request.QueryString("cod")
                cargarRegistro(hd.Value)
            Else
                hd.Value = String.Empty
                divDetalle.Visible = False
            End If
        End If
    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim oGrupoLimiteTraderBE As New GrupoLimiteTraderBE
        Dim oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE

        Dim oRow As GrupoLimiteTraderBE.GrupoLimiteTraderRow

        oGrupoLimiteTraderBE = oGrupoLimiteTraderBM.Seleccionar(codigo)

        oRow = DirectCast(oGrupoLimiteTraderBE.GrupoLimiteTrader.Rows(0), GrupoLimiteTraderBE.GrupoLimiteTraderRow)

        hd.Value = oRow.CodigoGrupLimTrader.ToString()
        tbDescripcion.Text = oRow.Nombre.ToString()
        ddlTipoRenta.SelectedValue = oRow.CodigoRenta.ToString()
        ddlTipoInst.SelectedValue = oRow.Tipo.ToString()
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()

        If ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI Then
            trNemonico.Style.Item("display") = "none"
            trTipo.Style.Item("display") = ""
        Else
            trNemonico.Style.Item("display") = ""
            trTipo.Style.Item("display") = "none"
        End If

        oGrupoLimiteTraderDetalleBE = oGrupoLimiteTraderBM.SeleccionarPorFiltroDetalle(codigo, ddlTipoInst.SelectedValue)

        VistaDetalle = oGrupoLimiteTraderDetalleBE
        CargarGrilla()

        divDetalle.Visible = True

        btnMostrarDetalle.Visible = False
        btnAgregarDetalle.Visible = False
        btnModificarDetalle.Visible = False

        DeshabilitarControlesCabecera(2)
        btnAgregarDetalle.Visible = True
    End Sub

    Private Function crearObjeto() As GrupoLimiteTraderBE

        Dim oGrupoLimiteTraderBE As New GrupoLimiteTraderBE
        Dim oRow As GrupoLimiteTraderBE.GrupoLimiteTraderRow

        oRow = DirectCast(oGrupoLimiteTraderBE.GrupoLimiteTrader.NewGrupoLimiteTraderRow(), GrupoLimiteTraderBE.GrupoLimiteTraderRow)

        oRow.CodigoGrupLimTrader = Val(hd.Value)
        oRow.Nombre = tbDescripcion.Text.Trim
        oRow.Tipo = ddlTipoInst.SelectedValue
        oRow.CodigoRenta = ddlTipoRenta.SelectedValue
        oRow.Situacion = ddlSituacion.SelectedValue

        oGrupoLimiteTraderBE.GrupoLimiteTrader.AddGrupoLimiteTraderRow(oRow)
        oGrupoLimiteTraderBE.GrupoLimiteTrader.AcceptChanges()

        Return oGrupoLimiteTraderBE

    End Function

    Private Sub CargarGrilla()
        dgLista.DataSource = VistaDetalle
        dgLista.DataBind()
    End Sub

    Private Sub CargarCombos()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oTipoInstrumento As New TipoInstrumentoBM
        Dim oTipoRentaBM As New TipoRentaBM
        Dim dt As New DataTable

        dt = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", False)

        HelpCombo.LlenarComboBox(ddlSituacionDet, dt, "Valor", "Nombre", False)

        dt = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, dt, "CodigoRenta", "Descripcion", True)
        ddlTipoRenta.SelectedValue = TR_DERIVADOS
        ddlTipoRenta.Items.RemoveAt(ddlTipoRenta.SelectedIndex)
        ddlTipoRenta.SelectedIndex = 0

        dt = oParametrosGenerales.Listar(TIPO_GRUPO_TRADING, DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoInst, dt, "Valor", "Nombre", True)
        ddlTipoInst.SelectedIndex = 0

        dt = oTipoInstrumento.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoInstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
    End Sub

    Private Sub LimpiarControlesCabecera()

        tbDescripcion.Text = String.Empty
        ddlTipoInst.SelectedIndex = 0
        ddlTipoRenta.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0

    End Sub

    Private Sub LimpiarControlesDetalle()
        tbNemonico.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlTipoInstrumento.SelectedIndex = 0
        ddlSituacionDet.SelectedIndex = 0
    End Sub

    Private Function ExisteGrupoLimite() As Boolean

        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim oGrupoLimiteTraderBE As New GrupoLimiteTraderBE

        oGrupoLimiteTraderBE = oGrupoLimiteTraderBM.Seleccionar(Val(hd.Value.Trim))

        Return oGrupoLimiteTraderBE.GrupoLimiteTrader.Rows.Count > 0

    End Function

    Private Function ValidarDataEntry() As Boolean

        Return VistaDetalle.GrupoLimiteTraderDetalle.Rows.Count > 0

    End Function

    Private Sub InsertarFilaGrupoLimiteTraderDetalle()
        Dim oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE
        Dim oRow As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow
        Dim strValor As String = IIf(ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI, ddlTipoInstrumento.SelectedValue, tbNemonico.Text.ToUpper.Trim.ToString)

        oGrupoLimiteTraderDetalleBE = VistaDetalle
        If oGrupoLimiteTraderDetalleBE Is Nothing Then oGrupoLimiteTraderDetalleBE = New GrupoLimiteTraderDetalleBE

        If oGrupoLimiteTraderDetalleBE.Tables(0).Select("Valor = '" & strValor & "'").Length = 0 Then
            oRow = oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.NewGrupoLimiteTraderDetalleRow()
            oRow.CodigoGrupLimTrader = Val(hd.Value)
            oRow.Valor = strValor
            oRow.Situacion = ddlSituacionDet.SelectedValue
            oRow.NombreSituacion = ddlSituacionDet.SelectedItem.Text
            oRow.Instrumento = IIf(ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI, ddlTipoInstrumento.SelectedItem.Text, tbNemonico.Text.ToUpper.Trim.ToString)
            oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.AddGrupoLimiteTraderDetalleRow(oRow)
            oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.AcceptChanges()

            VistaDetalle = oGrupoLimiteTraderDetalleBE
        End If
    End Sub

    Private Sub ModificarFilaGrupoLimiteTraderDetalle()

        Dim oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE
        Dim oRow As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow

        Dim intIndice As Integer

        oGrupoLimiteTraderDetalleBE = VistaDetalle

        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))

        oRow = DirectCast(oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows(intIndice), GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow)

        oRow.BeginEdit()
        oRow.CodigoGrupLimTrader = hd.Value
        oRow.Valor = IIf(ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI, ddlTipoInstrumento.SelectedValue, tbNemonico.Text.ToUpper.Trim.ToString)
        oRow.Situacion = ddlSituacionDet.SelectedValue
        oRow.NombreSituacion = ddlSituacionDet.SelectedItem.Text
        oRow.Instrumento = IIf(ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI, ddlTipoInstrumento.SelectedItem.Text, tbNemonico.Text.ToUpper.Trim.ToString)

        oRow.EndEdit()
        oRow.AcceptChanges()
        oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.AcceptChanges()

        VistaDetalle = oGrupoLimiteTraderDetalleBE

    End Sub

    Private Sub DeshabilitarControlesCabecera(ByVal opcion As Integer)
        If opcion = 1 Then 'ingreso
            tbDescripcion.Enabled = False
            ddlTipoRenta.Enabled = False
            ddlTipoInst.Enabled = False
            ddlSituacion.Enabled = False
        ElseIf opcion = 2 Then 'modificacion
            tbDescripcion.Enabled = True
            ddlTipoRenta.Enabled = True
            ddlTipoInst.Enabled = False
            ddlSituacion.Enabled = True
        End If

    End Sub

    Private Sub HabilitarControlesCabecera()

        tbDescripcion.Enabled = True
        ddlTipoInst.Enabled = True
        ddlTipoRenta.Enabled = True
        ddlSituacion.Enabled = True

    End Sub

    Private Sub DeshabilitarControlesDetalle()
        tbNemonico.Enabled = False
        ddlTipoInstrumento.Enabled = False
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
        tbNemonico.Enabled = True
        ddlTipoInstrumento.Enabled = True
        ddlSituacionDet.Enabled = True
    End Sub

    Private Sub Ingresar()
        Dim blnExisteGrupoLimite As Boolean
        blnExisteGrupoLimite = ExisteGrupoLimite()

        If Not blnExisteGrupoLimite Then
            divDetalle.Visible = True
            btnMostrarDetalle.Visible = False
            btnModificarDetalle.Visible = False
            DeshabilitarControlesCabecera(1)
            HabilitarControlesDetalle()
        Else
            AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
        End If
    End Sub

    Private Sub Agregar()
        InsertarFilaGrupoLimiteTraderDetalle()
        CargarGrilla()
        LimpiarControlesDetalle()
    End Sub

    Private Sub ModificarDetalle()
        ModificarFilaGrupoLimiteTraderDetalle()
        CargarGrilla()
        LimpiarControlesDetalle()

        btnModificarDetalle.Visible = False
        btnAgregarDetalle.Visible = True
        HabilitarControlesDetalle()

        If hd.Value.Equals(String.Empty) Then
            btnAgregarDetalle.Visible = True
        End If
    End Sub

    Private Sub EditarGrilla(ByVal index As Integer)
        Dim oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE

        oGrupoLimiteTraderDetalleBE = VistaDetalle
        btnModificarDetalle.Enabled = True

        If ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI Then
            ddlTipoInstrumento.SelectedValue = oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows(index)("Valor").ToString()
        Else
            tbNemonico.Text = oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows(index)("Valor").ToString()
        End If

        ddlSituacionDet.SelectedValue = oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows(index)("Situacion").ToString()

        ViewState("IndiceSeleccionado") = index

        btnModificarDetalle.Visible = True
        btnAgregarDetalle.Visible = False
    End Sub

    Private Sub EliminarFilaGrupoLimiteTraderDetalle(ByVal index As Integer)

        Dim oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE
        Dim oRow As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow

        oGrupoLimiteTraderDetalleBE = VistaDetalle
        oRow = DirectCast(oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows(index), GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow)

        oRow.BeginEdit()
        ddlSituacionDet.SelectedValue = ESTADO_INACTIVO
        oRow.Situacion = ESTADO_INACTIVO
        oRow.NombreSituacion = ddlSituacionDet.SelectedItem.Text
        oRow.EndEdit()
        oRow.AcceptChanges()
        oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.AcceptChanges()
        ddlSituacionDet.SelectedIndex = 0

        VistaDetalle = oGrupoLimiteTraderDetalleBE

        CargarGrilla()
    End Sub


    Private Sub Cancelar()

        Dim oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE

        oGrupoLimiteTraderDetalleBE = VistaDetalle

        oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Clear()
        oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.AcceptChanges()

        VistaDetalle = oGrupoLimiteTraderDetalleBE

        CargarGrilla()

        divDetalle.Visible = False

        btnMostrarDetalle.Visible = True

        HabilitarControlesCabecera()

    End Sub

    Private Function VerificarGrupoLimiteTraderDetalle(ByVal strCodigoGrupLimTrader As String, ByVal strValor As String) As Boolean
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim LngContar As Long
        LngContar = oGrupoLimiteTraderBM.SeleccionarDetalle(strCodigoGrupLimTrader, strValor).Tables(0).Rows.Count
        If LngContar > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
#End Region

    Private Sub ddlTipoInst_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoInst.SelectedIndexChanged
        Try
            If ddlTipoInst.SelectedValue = TIPO_GRUPO_TRADING_TI Then
                trNemonico.Style.Item("display") = "none"
                trTipo.Style.Item("display") = ""
            Else
                trNemonico.Style.Item("display") = ""
                trTipo.Style.Item("display") = "none"
            End If
            Dim oGrupoLimiteTraderDetalleBE As New GrupoLimiteTraderDetalleBE
            oGrupoLimiteTraderDetalleBE.Clear()
            VistaDetalle = oGrupoLimiteTraderDetalleBE
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Tipo de Instrumento")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            ActualizarIndiceGrilla(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName.Equals("Modificar") Then
                EditarGrilla(CInt(e.CommandArgument.ToString.Split("|")(1)))
            End If
            If e.CommandName.Equals("Eliminar") Then
                EliminarFilaGrupoLimiteTraderDetalle(CInt(e.CommandArgument.ToString.Split("|")(1)))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub
End Class
