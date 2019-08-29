Option Explicit On
Option Strict Off
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Parametria_CotizacionVAC_frmCotizacionVAC
    Inherits BasePage
#Region "/* Variables */"
    Dim oIndicadorBM As New IndicadorBM
    Dim EstructuraCodigoLibor As String = "SW-{0}ML-{1}"
    Dim EstructuraNombreLibor As String = "{0}M - LIBOR - {1}"
#End Region
#Region "/* Propiedades */"
    Private Property VistaDetalleCotizacion() As CotizacionVACBE
        Get
            Return DirectCast(ViewState("Cotizacion"), CotizacionVACBE)
        End Get
        Set(ByVal Value As CotizacionVACBE)
            ViewState("Cotizacion") = Value
        End Set
    End Property
#End Region
#Region "/* Eventos de la Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                btnModificarDetalle.Visible = False
                'btnAgregarDetalle.Visible = False
                ViewState("IndiceSeleccionado") = Nothing
                If Not Request.QueryString("codigo") Is Nothing Then
                    hdCodigo.Value = Request.QueryString("codigo")
                    CargarControles(hdCodigo.Value)
                    BuscarDetalle(0)
                    HabilitarControlesBusqueda()
                Else
                    hdCodigo.Value = String.Empty
                    VistaDetalleCotizacion = New CotizacionVACBE
                    DeshabilitarControlesDetalle()
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnAgregarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarDetalle.Click
        Try
            If dllTipoIndicador.SelectedValue = "" Then
                AlertaJS("Seleccione el tipo de indicador para continuar.")
                Exit Sub
            End If
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
            If tbFechaValor.Text = String.Empty Then
                AlertaJS("Ingrese la fecha para continuar.")
                Exit Sub
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
            Dim blnExisteRegistro As Boolean
            blnExisteRegistro = ExisteRegistro()
            If blnExisteRegistro Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                Exit Sub
            End If
            Agregar()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnModificarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificarDetalle.Click
        Try
            If Not ddlPortafolioSBS.SelectedValue = "--SELECCIONE--" Then
                Dim blnExisteRegistro As Boolean
                blnExisteRegistro = ExisteRegistro()
                If blnExisteRegistro Then
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                    Exit Sub
                End If
                Modificar()
            Else
                AlertaJS("Seleccione un portafolio.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - se inhabilita condicional por fecha | 23/07/18 
            'If tbFechaValor.Text = String.Empty Then
            '    AlertaJS("Ingrese la fecha de control.")
            'Else
            'Validar que no exista

            EstablecerCodigoLibor()
            If txtCodigoIndicador.Text.Trim() <> String.Empty And Request.QueryString("codigo") Is Nothing Then
                Dim oIndicadorBM As New IndicadorBM
                Dim oIndicadorBE As New IndicadorBE
                oIndicadorBE = oIndicadorBM.Seleccionar(txtCodigoIndicador.Text, DatosRequest)
                If oIndicadorBE.Tables(0).Rows.Count > 0 Then
                    AlertaJS("Ya existe un indicador con el codigo ingresado")
                    Exit Sub
                End If
            End If
           
            Aceptar()
            BuscarDetalle(0)
            HabilitarControlesBusqueda()
            'End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - se inhabilita condicional por fecha  | 23/07/18 
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaCotizacionVAC.aspx")
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Private Sub btnBuscarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarDetalle.Click
        Try
            EstablecerCodigoLibor()
            BuscarDetalle(0)
            HabilitarControlesBusqueda()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer
            limpiarGrilla(dgLista)
            If e.CommandName.Equals("Modificar") Or e.CommandName.Equals("Eliminar") Then
                index = CInt(e.CommandArgument.ToString().Split("|")(1))
                dgLista.Rows.Item(index).BackColor = System.Drawing.Color.LemonChiffon
            End If
            If e.CommandName.Equals("Modificar") Then
                EditarGrilla(index)
            ElseIf e.CommandName.Equals("Eliminar") Then
                EliminarFilaCotizacion(index)
            End If
            EstablecerCodigoLibor()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(2).Text = UIUtility.ConvertirFechaaString(e.Row.Cells(2).Text)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
#End Region
#Region "/* Metodos Personalizados */"
    Private Function ExisteRegistro() As Boolean
        Dim seleccion As String
        seleccion = ddlPortafolioSBS.SelectedValue.ToString
        If (seleccion = "--Seleccione--") Then
            seleccion = "&nbsp;"
        End If
        If Not ViewState("IndiceSeleccionado") Is Nothing Then ' modificar
            For Each item As GridViewRow In dgLista.Rows
                If tbFechaControl.Text = item.Cells(2).Text And item.RowIndex <> ViewState("IndiceSeleccionado") Then
                    Return True
                End If
            Next
        Else
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
            For Each item As GridViewRow In dgLista.Rows
                If tbFechaValor.Text = item.Cells(2).Text Then
                    Return True
                End If
            Next
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
        End If
        Return False
    End Function
    Private Function ValidarDataEntry() As Boolean
        Return VistaDetalleCotizacion.CotizacionVAC.Rows.Count > 0
    End Function
    Private Sub CargarCombos()
        Dim dtManejaPeriodo, dtSituacion As DataTable
        Dim dtTipoIndicador As DataTable
        Dim dtFuenteLibor As DataTable
        Dim oIndicadorBM As New IndicadorBM
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oPortafolioBM As New PortafolioBM
        ddlPortafolioSBS.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolioSBS.DataValueField = "CodigoPortafolio"
        ddlPortafolioSBS.DataTextField = "Descripcion"
        ddlPortafolioSBS.DataBind()
        ddlPortafolioSBS.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        dtSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dtSituacion, "Valor", "Nombre", False)
        dtManejaPeriodo = oParametrosGenerales.Listar("Desicion", DatosRequest)
        HelpCombo.LlenarComboBox(ddlManejaPeriodo, dtManejaPeriodo, "Valor", "Nombre", True)
        dtTipoIndicador = oParametrosGenerales.Listar("TipoIndica", DatosRequest)
        HelpCombo.LlenarComboBox(dllTipoIndicador, dtTipoIndicador, "Valor", "Nombre", True)
        dtSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacionDetalle, dtSituacion, "Valor", "Nombre", False)

        HelpCombo.LlenarComboBox(ddlMostrarLibor, dtManejaPeriodo, "Valor", "Nombre", False)
        ddlMostrarLibor.SelectedValue = "N"
        dtFuenteLibor = oParametrosGenerales.Listar("FUENTE_LIBOR", DatosRequest)
        HelpCombo.LlenarComboBox(ddlFuenteLibor, dtFuenteLibor, "Valor", "Nombre", False)
        txtMesLibor.Text = 1
    End Sub
    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If hdCodigo.Value.Equals(String.Empty) Then
            blnExisteEntidad = ExisteEntidad()
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                InsertarIndicador()
                ModificarValorIndicador()
                AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
                hdCodigo.Value = txtCodigoIndicador.Text
            End If
        Else
            ModificarIndicador()
            ModificarValorIndicador()
            AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
        End If
    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oCotizacionVACBE As CotizacionVACBE
        Dim oCotizacionVACBM As New CotizacionVACBM
        Dim decFecha As Decimal
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
        decFecha = UIUtility.ConvertirFechaaDecimal(tbFechaValor.Text)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
        oCotizacionVACBE = oCotizacionVACBM.SeleccionarPorFiltro(decFecha, txtCodigoIndicador.Text, DatosRequest)
        Return oCotizacionVACBE.CotizacionVAC.Rows.Count > 0
    End Function
    Private Sub InsertarIndicador()
        Dim oIndicadorBE As New IndicadorBE
        Dim oIndicadorBM As New IndicadorBM
        Dim row As IndicadorBE.IndicadorRow
        row = oIndicadorBE.Indicador.NewIndicadorRow

        'INCIO | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
        Dim codigoIndicadorLibor As String = EstructuraCodigoLibor
        Dim nombreIndicadorLibor As String = EstructuraNombreLibor
        If ddlMostrarLibor.SelectedValue = "N" Then
            row("CodigoIndicador") = txtCodigoIndicador.Text
            row("NombreIndicador") = txtNombreIndicador.Text
            row("FuenteLibor") = String.Empty
            row("MesLibor") = 0
        Else
            codigoIndicadorLibor = String.Format(codigoIndicadorLibor, txtMesLibor.Text.Trim(), ddlFuenteLibor.SelectedValue.Substring(0, 1))
            nombreIndicadorLibor = String.Format(nombreIndicadorLibor, txtMesLibor.Text.Trim(), ddlFuenteLibor.SelectedValue)

            row("CodigoIndicador") = codigoIndicadorLibor
            row("NombreIndicador") = nombreIndicadorLibor

            row("FuenteLibor") = ddlFuenteLibor.SelectedValue
            row("MesLibor") = Convert.ToInt32(txtMesLibor.Text)
            txtCodigoIndicador.Text = codigoIndicadorLibor
            txtNombreIndicador.Text = nombreIndicadorLibor
        End If

        row("MostrarLibor") = ddlMostrarLibor.SelectedValue
        'FIN | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor


        row("Situacion") = ddlSituacionDetalle.SelectedValue.ToString() 'ddlSituacion.SelectedValue.ToString() Antes modificado por LC
        row("ManejaPeriodo") = ddlManejaPeriodo.SelectedValue.ToString
        row("TipoIndicador") = dllTipoIndicador.SelectedValue.ToString
        oIndicadorBE.Indicador.AddIndicadorRow(row)
        oIndicadorBE.Indicador.AcceptChanges()
        oIndicadorBM.Insertar(oIndicadorBE, DatosRequest)
    End Sub
    Private Sub InsertarValorIndicador()
        Dim oCotizacionVACBE As CotizacionVACBE
        Dim oCotizacionVACBM As New CotizacionVACBM
        oCotizacionVACBE = DirectCast(ViewState("Cotizacion"), CotizacionVACBE)
        oCotizacionVACBM.Insertar(oCotizacionVACBE, DatosRequest)
    End Sub
    Private Sub ModificarIndicador()
        Dim oIndicadorBE As New IndicadorBE
        Dim oIndicadorBM As New IndicadorBM
        Dim row As IndicadorBE.IndicadorRow
        row = oIndicadorBE.Indicador.NewIndicadorRow
        row("CodigoIndicador") = txtCodigoIndicador.Text
        row("NombreIndicador") = txtNombreIndicador.Text
        row("Situacion") = ddlSituacion.SelectedValue.ToString
        row("ManejaPeriodo") = ddlManejaPeriodo.SelectedValue.ToString
        row("TipoIndicador") = dllTipoIndicador.SelectedValue.ToString
        oIndicadorBE.Indicador.AddIndicadorRow(row)
        oIndicadorBE.Indicador.AcceptChanges()
        oIndicadorBM.Modificar(oIndicadorBE, DatosRequest)
    End Sub
    Private Sub ModificarValorIndicador()
        Dim oCotizacionVACBE As CotizacionVACBE
        Dim oCotizacionVACBM As New CotizacionVACBM
        If Not ViewState("Cotizacion") Is Nothing Then
            oCotizacionVACBE = DirectCast(ViewState("Cotizacion"), CotizacionVACBE)
            oCotizacionVACBM.Modificar(txtCodigoIndicador.Text, oCotizacionVACBE, DatosRequest)
        End If
    End Sub
    Private Sub LimpiarControlesDetalle()
        txtDiasPeriodo.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedIndex = 0
        txtValorCotizacion.Text = Constantes.M_STR_TEXTO_INICIAL
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Duplicar habilitación de campo "Valor" en nuevo campo "Fecha Valor"| 23/07/18 
        tbFechaValor.Text = Constantes.M_STR_TEXTO_INICIAL
        divFechaValor.Attributes.Add("class", "input-append date")
        tbFechaValor.ReadOnly = False
        chkFechaBusqueda.Checked = True
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Duplicar habilitación de campo "Valor" en nuevo campo "Fecha Valor"| 23/07/18 
    End Sub
    Private Sub LimpiarControlesCaberera()
        tbFechaControl.Text = Constantes.M_STR_TEXTO_INICIAL
        txtCodigoIndicador.Text = Constantes.M_STR_TEXTO_INICIAL
        txtNombreIndicador.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedIndex = 0
        ddlManejaPeriodo.SelectedIndex = 0
    End Sub
    Private Sub InsertarFilaCotizacion()
        Dim oCotizacionVACBE As CotizacionVACBE
        Dim oRow As CotizacionVACBE.CotizacionVACRow
        oCotizacionVACBE = VistaDetalleCotizacion
        oRow = oCotizacionVACBE.CotizacionVAC.NewCotizacionVACRow()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
        oRow.Fecha = UIUtility.ConvertirFechaaDecimal(tbFechaValor.Text)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - se cambia el campo fecha por "Fecha Valor" | 23/07/18 
        oRow.CodigoIndicador = "Nuevo"
        Dim manejaPeriodo As String
        manejaPeriodo = ddlManejaPeriodo.SelectedValue.ToString

        If manejaPeriodo = "S" Then
            oRow.DiasPeriodo = txtDiasPeriodo.Text.Replace(".", UIUtility.DecimalSeparator())
            oRow.CodigoPortafolioSBS = ""
        Else
            oRow.DiasPeriodo = -1
            oRow.CodigoPortafolioSBS = ddlPortafolioSBS.SelectedValue.ToString
            oRow.Descripcion = ddlPortafolioSBS.SelectedItem.Text
            If oRow.CodigoPortafolioSBS = "--Seleccione--" Then
                oRow.CodigoPortafolioSBS = ""
            End If
        End If
        oRow.Situacion = ddlSituacionDetalle.SelectedValue
        oRow.NombreSituacion = ddlSituacionDetalle.SelectedItem.Text
        oRow.Valor = txtValorCotizacion.Text.Replace(".", UIUtility.DecimalSeparator())
        oCotizacionVACBE.CotizacionVAC.AddCotizacionVACRow(oRow)
        oCotizacionVACBE.CotizacionVAC.AcceptChanges()
        VistaDetalleCotizacion = oCotizacionVACBE
    End Sub
    Private Sub ModificarFilaCotizacion()
        Dim oCotizacionVACBE As CotizacionVACBE
        Dim oRow As CotizacionVACBE.CotizacionVACRow
        Dim intIndice As Integer
        oCotizacionVACBE = VistaDetalleCotizacion
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        oRow = DirectCast(oCotizacionVACBE.CotizacionVAC.Rows(intIndice), CotizacionVACBE.CotizacionVACRow)
        oRow.CodigoIndicador = txtCodigoIndicador.Text
        Dim manejaPeriodo As String
        manejaPeriodo = ddlManejaPeriodo.SelectedValue.ToString
        oRow.BeginEdit()
        If manejaPeriodo = "S" Then
            oRow.DiasPeriodo = txtDiasPeriodo.Text.Replace(".", UIUtility.DecimalSeparator())
            oRow.CodigoPortafolioSBS = ""
        Else
            oRow.DiasPeriodo = -1
            oRow.CodigoPortafolioSBS = ddlPortafolioSBS.SelectedValue.ToString
            oRow.Descripcion = ddlPortafolioSBS.SelectedItem.Text
        End If
        oRow.Valor = txtValorCotizacion.Text.Replace(".", UIUtility.DecimalSeparator())
        oRow.Situacion = ddlSituacionDetalle.SelectedValue  'ddlSituacion.SelectedValue Antes Modificado por LC
        oRow.NombreSituacion = ddlSituacionDetalle.SelectedItem.Text   'ddlSituacion.SelectedItem.Text Antes Modificado por LC
        oRow.EndEdit()
        oCotizacionVACBE.CotizacionVAC.AcceptChanges()
        VistaDetalleCotizacion = oCotizacionVACBE
    End Sub
    Private Sub EliminarFilaCotizacion(ByVal index As Integer)
        Dim oCotizacionVACBE As CotizacionVACBE
        Dim oRow As CotizacionVACBE.CotizacionVACRow
        oCotizacionVACBE = DirectCast(ViewState("Cotizacion"), CotizacionVACBE)
        oRow = DirectCast(oCotizacionVACBE.CotizacionVAC.Rows(index), CotizacionVACBE.CotizacionVACRow)
        oRow.BeginEdit()
        oRow.Situacion = ddlSituacion.Items(1).Value
        oRow.NombreSituacion = ddlSituacion.Items(1).Text
        oRow.EndEdit()
        oCotizacionVACBE.CotizacionVAC.AcceptChanges()
        VistaDetalleCotizacion = oCotizacionVACBE
        CargarGrilla()
    End Sub
    Private Sub DeshabilitarControlesBusqueda()
        tbFechaControl.Enabled = False
        imgCalendar.Visible = False
    End Sub
    Private Sub DeshabilitarControlesCabecera()
        txtCodigoIndicador.Enabled = False
        txtNombreIndicador.Enabled = False
        ddlManejaPeriodo.Enabled = False
        ddlSituacion.Enabled = False
        ddlFuenteLibor.Enabled = False
        txtMesLibor.Enabled = False
        ddlMostrarLibor.Enabled = False
    End Sub
    Private Sub HabilitarControlesBusqueda()
        tbFechaControl.Enabled = True
        imgCalendar.Visible = True
    End Sub
    Private Sub DeshabilitarControlesDetalle()
        If (ddlManejaPeriodo.SelectedValue.ToString = "S") Then
            txtDiasPeriodo.Enabled = False
        ElseIf (ddlManejaPeriodo.SelectedValue.ToString = "N") Then
            ddlPortafolioSBS.Enabled = False
        Else
            txtDiasPeriodo.Visible = False
            ddlPortafolioSBS.Visible = False
            lblDiasPeriodo.Visible = False
        End If
        'ddlSituacionDetalle.Enabled = False
        'txtValorCotizacion.Enabled = False
    End Sub
    Private Sub HabilitarControlesDetalle()
        If (ddlManejaPeriodo.SelectedValue.ToString = "S") Then
            txtDiasPeriodo.Enabled = True
        ElseIf (ddlManejaPeriodo.SelectedValue.ToString = "N") Then
            ddlPortafolioSBS.Enabled = True
        Else
            txtDiasPeriodo.Visible = True
            lblDiasPeriodo.Visible = True
        End If
        ddlSituacionDetalle.Enabled = True
        txtValorCotizacion.Enabled = True
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Duplicar habilitación de campo "Valor" en nuevo campo "Fecha Valor"| 23/07/18 
        tbFechaValor.Enabled = True
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Duplicar habilitación de campo "Valor" en nuevo campo "Fecha Valor"| 23/07/18 
    End Sub
    Private Sub CargarGrilla()
        dgLista.DataSource = VistaDetalleCotizacion
        dgLista.DataBind()
    End Sub
    Private Sub Ingresar()
        Dim blnExisteEntidad As Boolean
        blnExisteEntidad = ExisteEntidad()
        If Not blnExisteEntidad Then
            btnModificarDetalle.Visible = False
            HabilitarControlesDetalle()
        Else
            AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
        End If
    End Sub
    Private Sub Cancelar()
        Dim oCotizacionVACBE As CotizacionVACBE
        oCotizacionVACBE = VistaDetalleCotizacion
        oCotizacionVACBE.CotizacionVAC.Clear()
        oCotizacionVACBE.CotizacionVAC.AcceptChanges()
        VistaDetalleCotizacion = oCotizacionVACBE
        CargarGrilla()
    End Sub
    Private Sub Agregar()
        InsertarFilaCotizacion()
        CargarGrilla()
        LimpiarControlesDetalle()
        btnModificarDetalle.Visible = False
    End Sub
    Private Sub Modificar()
        ModificarFilaCotizacion()
        CargarGrilla()
        LimpiarControlesDetalle()
        ViewState("IndiceSeleccionado") = Nothing
        btnModificarDetalle.Visible = False
        btnAgregarDetalle.Visible = True
    End Sub
    Private Sub CargarControles(ByVal cadenaQueryString As String)
        Dim oIndicadorBM As New IndicadorBM
        Dim oIndicadorBE As New IndicadorBE
        Dim strCodigoIndicador As String
        strCodigoIndicador = cadenaQueryString.Split(","c)(1)
        oIndicadorBE = oIndicadorBM.Seleccionar(strCodigoIndicador, DatosRequest)
        Dim oRow As IndicadorBE.IndicadorRow
        oRow = DirectCast(oIndicadorBE.Indicador.Rows(0), IndicadorBE.IndicadorRow)
        txtCodigoIndicador.Text = oRow.CodigoIndicador.ToString
        txtNombreIndicador.Text = oRow.NombreIndicador.ToString
        ddlSituacion.SelectedValue = oRow.Situacion.ToString

        ddlMostrarLibor.SelectedValue = oRow.MostrarLibor

        If (oRow.MostrarLibor = "S") Then
            pnlLiborFuente.Visible = True
            ddlFuenteLibor.SelectedValue = oRow.FuenteLibor
            txtMesLibor.Text = oRow.MesLibor
        Else
            pnlLiborFuente.Visible = False

        End If

        Try
            ddlManejaPeriodo.SelectedValue = oRow.ManejaPeriodo.ToString
            ControlarManejaPeriodo(oRow.ManejaPeriodo.ToString)
        Catch ex As Exception
            ddlManejaPeriodo.SelectedIndex = 0
            ControlarManejaPeriodo("S")
        End Try
        Try
            dllTipoIndicador.SelectedValue = oRow.TipoIndicador.ToString
        Catch ex As Exception
            dllTipoIndicador.SelectedIndex = 0
        End Try
    End Sub
    Private Sub EditarGrilla(ByVal index As Integer)
        Dim oCotizacionVACBE As CotizacionVACBE
        oCotizacionVACBE = VistaDetalleCotizacion
        btnModificarDetalle.Enabled = True
        If ddlManejaPeriodo.SelectedValue.ToString = "S" Then
            txtDiasPeriodo.Text = oCotizacionVACBE.CotizacionVAC.Rows(index)("DiasPeriodo").ToString()
        ElseIf ddlManejaPeriodo.SelectedValue.ToString = "N" Then
            ddlPortafolioSBS.SelectedValue = oCotizacionVACBE.CotizacionVAC.Rows(index)("CodigoPortafolioSBS").ToString()
        End If
        txtValorCotizacion.Text = oCotizacionVACBE.CotizacionVAC.Rows(index)("Valor").ToString().Replace(UIUtility.DecimalSeparator(), ".")
        ddlSituacionDetalle.SelectedValue = oCotizacionVACBE.CotizacionVAC.Rows(index)("Situacion").ToString()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se recupera fecha valor en nuevo campo | 23/07/18 
        tbFechaValor.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(oCotizacionVACBE.CotizacionVAC.Rows(index)("Fecha"))
        divFechaValor.Attributes.Add("class", "input-append")
        tbFechaValor.ReadOnly = True
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se recupera fecha valor en nuevo campo | 23/07/18 
        HabilitarControlesDetalle()
        ViewState("IndiceSeleccionado") = index
        btnModificarDetalle.Visible = True
        btnAgregarDetalle.Visible = False
        lblAddModRegistro.Text = "Modificar Registro"
    End Sub
    Private Sub ControlarManejaPeriodo(ByVal manejaPeriodo As String)
        rvManejaPeriodo.Enabled = True
        If manejaPeriodo = "S" Then
            lblDiasPeriodo.Visible = True
            lblDiasPeriodo.Text = "Dias Periodo :"
            ddlPortafolioSBS.Visible = False
            txtDiasPeriodo.Visible = True
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se cambia búsqueda de columna por nombre de DataField | 23/07/18 
            MostrarColumna_Grilla(dgLista, obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", dgLista), "hidden")
            MostrarColumna_Grilla(dgLista, obtenerIndiceColumna_Grilla("DiasPeriodo", dgLista), String.Empty)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se cambia búsqueda de columna por DataField | 23/07/18 
            rvManejaPeriodo.ControlToValidate = "txtDiasPeriodo"
            rvManejaPeriodo.ErrorMessage = "Dias Periodo"
        ElseIf (manejaPeriodo = "N") Then
            lblDiasPeriodo.Visible = True
            lblDiasPeriodo.Text = "Portafolio :"
            ddlPortafolioSBS.Visible = True
            txtDiasPeriodo.Visible = False
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se cambia búsqueda de columna por nombre de DataField | 23/07/18 
            MostrarColumna_Grilla(dgLista, obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", dgLista), "hidden")
            MostrarColumna_Grilla(dgLista, obtenerIndiceColumna_Grilla("DiasPeriodo", dgLista), String.Empty)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se cambia búsqueda de columna por nombre de DataField | 23/07/18 

            rvManejaPeriodo.ControlToValidate = "ddlPortafolioSBS"
            rvManejaPeriodo.ErrorMessage = "Portafolio"
            If Not txtCodigoIndicador.Text.Trim().Equals("FACVAC") Then
                rvManejaPeriodo.Enabled = False
            End If
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se oculta la columna "Portafolio" y "Dias de periodo" de grilla cuando filtro se "FACVAC" | 23/07/18 
        If txtCodigoIndicador.Text.Trim().Equals("FACVAC") Then
            MostrarColumna_Grilla(dgLista, obtenerIndiceColumna_Grilla("Descripcion", dgLista), "hidden")
            MostrarColumna_Grilla(dgLista, obtenerIndiceColumna_Grilla("DiasPeriodo", dgLista), "hidden")
            divDiasPeriodoPortafolio.Attributes.Add("style", "display:none")
            rvManejaPeriodo.Enabled = False
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se oculta la columna "Portafolio" y "Dias de periodo" de grilla cuando filtro se "FACVAC" | 23/07/18 

        'INICIO | CDA | Ernesto Galarza| OT 11966 - Agregar controles para registrar indicadores para tasa libor
        EstablecerCodigoLibor()
        'FIN | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
    End Sub
    Private Sub EstablecerCodigoLibor()
        If Request.QueryString("codigo") Is Nothing Then
            If ddlMostrarLibor.SelectedValue = "S" Then
                txtCodigoIndicador.Text = String.Format(EstructuraCodigoLibor, txtMesLibor.Text.Trim(), ddlFuenteLibor.SelectedValue.Substring(0, 1))
                txtNombreIndicador.Text = String.Format(EstructuraNombreLibor, txtMesLibor.Text.Trim(), ddlFuenteLibor.SelectedValue)
            End If
        End If
    End Sub

    Private Sub BuscarDetalle(ByVal tipoCarga As Int16)
        Dim oCotizacionVACBM As New CotizacionVACBM
        Dim oCotizacionVACBE As New CotizacionVACBE
        Dim decFecha As Decimal
        Dim strCodigoIndicador As String
        Dim cadenaQueryString As String
        If Not hdCodigo.Value Is Nothing And hdCodigo.Value <> "" Then
            cadenaQueryString = hdCodigo.Value
            strCodigoIndicador = cadenaQueryString.Split(","c)(1)
        Else
            strCodigoIndicador = txtCodigoIndicador.Text
        End If
        decFecha = UIUtility.ConvertirFechaaDecimal(tbFechaControl.Text)
        'NOTA:
        '-----
        'Tipo 0 => Carga Detalle Indicadores por Filtro Fecha y Codigo Indicador
        'Tipo 1 => Carga Detalle Indicadores por Filtro Ultima Fecha con datos e Indicador
        Select Case tipoCarga
            Case 0
                oCotizacionVACBE = oCotizacionVACBM.Seleccionar(decFecha, strCodigoIndicador, DatosRequest)
            Case 1
                oCotizacionVACBE = oCotizacionVACBM.SeleccionarPorUltimaFecha(strCodigoIndicador, DatosRequest)
                Dim fec As String
                If oCotizacionVACBE.Tables(0).Rows.Count > 0 Then
                    fec = oCotizacionVACBE.Tables(0).Rows(0)(0).ToString()
                    fec = UIUtility.ConvertirFechaaString(Convert.ToDecimal(fec))
                    tbFechaControl.Text = fec
                    tbFechaControl.Enabled = True
                End If
        End Select
        VistaDetalleCotizacion = oCotizacionVACBE
        CargarGrilla()
        ControlarManejaPeriodo(ddlManejaPeriodo.SelectedValue.ToString)
        btnAgregarDetalle.Visible = True
        DeshabilitarControlesBusqueda()
        DeshabilitarControlesCabecera()
        HabilitarControlesDetalle()
        LimpiarControlesDetalle()
    End Sub

    Private Sub limpiarGrilla(ByVal grilla As GridView)
        Dim i As Integer
        For i = 0 To grilla.Rows.Count - 1
            grilla.Rows.Item(i).BackColor = Drawing.Color.White
        Next
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Obtener índice de columna de acuerdo al nombre de la columna en grilla | 23/07/18 
    Private Function obtenerIndiceColumna_Grilla(ByVal nomCol As String, ByVal grilla As GridView) As Integer
        Dim columna As DataControlFieldCollection = grilla.Columns
        Dim indiceCol As Integer = -1
        For Each celda As DataControlField In columna
            If TypeOf celda Is System.Web.UI.WebControls.BoundField Then
                If CType(celda, BoundField).DataField = nomCol Then
                    indiceCol = columna.IndexOf(celda)
                    Exit For
                End If
            End If
        Next
        Return indiceCol
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener índice de columna de acuerdo al nombre de la columna en grilla | 23/07/18 

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Mostrar Columnas de acuerdo a requerimiento | 23/07/18 
    Private Sub MostrarColumna_Grilla(ByVal grilla As GridView, ByVal ColActual As Integer, ByVal opcionMostrar As String)
        grilla.Columns(ColActual).ItemStyle.CssClass = opcionMostrar
        grilla.Columns(ColActual).HeaderStyle.CssClass = opcionMostrar
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Mostrar Columnas de acuerdo a requerimiento | 23/07/18 

#End Region
    Protected Sub dllTipoIndicador_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dllTipoIndicador.SelectedIndexChanged
        If dllTipoIndicador.SelectedValue = "E" Then
            ControlarManejaPeriodo("S")
        Else
            ControlarManejaPeriodo("N")
        End If
    End Sub
    'INICIO | CDA | Ernesto Galarza| OT 11966 - Agregar controles para registrar indicadores para tasa libor
    Protected Sub ddlMostrarLibor_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMostrarLibor.SelectedIndexChanged

        If Request.QueryString("codigo") Is Nothing Then
            If ddlMostrarLibor.SelectedValue = "S" Then
                pnlLiborFuente.Visible = True
                txtNombreIndicador.Enabled = False
                txtCodigoIndicador.Enabled = False

                ddlFuenteLibor.SelectedValue = "PIP"
                txtMesLibor.Text = "1"

                txtCodigoIndicador.Text = String.Format(EstructuraCodigoLibor, txtMesLibor.Text.Trim(), ddlFuenteLibor.SelectedValue.Substring(0, 1))
                txtNombreIndicador.Text = String.Format(EstructuraNombreLibor, txtMesLibor.Text.Trim(), ddlFuenteLibor.SelectedValue)

            Else
                pnlLiborFuente.Visible = False
                txtNombreIndicador.Enabled = True
                txtCodigoIndicador.Enabled = True

                txtCodigoIndicador.Text = ""
                txtNombreIndicador.Text = ""
            End If
        End If
        
    End Sub
    'FIN | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
End Class