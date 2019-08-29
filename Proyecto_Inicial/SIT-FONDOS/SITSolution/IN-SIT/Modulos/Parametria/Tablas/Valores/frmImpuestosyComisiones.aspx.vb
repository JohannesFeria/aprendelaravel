Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Valores_frmImpuestosyComisiones
    Inherits BasePage
#Region "/* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            MyBase.AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaImpuestosComisiones.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try
    End Sub
#End Region
#Region " /* Funciones Insertar */ "
    Private Sub Insertar()
        Dim oImpuestosComisionesBM As New ImpuestosComisionesBM
        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        oImpuestosComisionesBE = crearObjeto()
        oImpuestosComisionesBM.Insertar(oImpuestosComisionesBE, DatosRequest)
        Me.AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub
#End Region
#Region " /* Funciones Modificar */"
    Private Sub Modificar()
        Dim oImpuestosComisionesBM As New ImpuestosComisionesBM
        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        oImpuestosComisionesBE = crearObjeto()
        oImpuestosComisionesBM.Modificar(oImpuestosComisionesBE, DatosRequest)
        Me.AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub
#End Region
#Region "/* Funciones Personalizadas */"
    Private Function ExisteEntidad() As Boolean
        Dim oImpuestosComisionesBM As New ImpuestosComisionesBM
        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        oImpuestosComisionesBE = oImpuestosComisionesBM.Seleccionar(Me.tbCodigo.Text.Trim, Me.ddlBolsa.SelectedValue, Me.ddlTipoRenta.SelectedValue, Me.DatosRequest)
        Return oImpuestosComisionesBE.ImpuestosComisiones.Rows.Count > 0
    End Function
    Private Sub CargarRegistro(ByVal codigoComision As String, ByVal codigoMercado As String, ByVal codigoRenta As String)
        Dim oImpuestosComisionesBM As New ImpuestosComisionesBM
        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        Dim oRow As ImpuestosComisionesBE.ImpuestosComisionesRow
        oImpuestosComisionesBE = oImpuestosComisionesBM.Seleccionar(codigoComision, codigoMercado, codigoRenta, DatosRequest)
        oRow = DirectCast(oImpuestosComisionesBE.ImpuestosComisiones.Rows(0), ImpuestosComisionesBE.ImpuestosComisionesRow)
        If oRow.CodigoPlaza.ToString <> "" Then
            ddlBolsa.SelectedValue = oRow.CodigoPlaza
        Else
            ddlBolsa.SelectedIndex = 0
        End If
        If oRow.CodigoMoneda.ToString <> "" Then
            ddlMonedaValorFijo.SelectedValue = oRow.CodigoMoneda
        Else
            ddlMonedaValorFijo.SelectedIndex = 0
        End If
        If oRow.Situacion.ToString <> "" Then
            ddlSituacion.SelectedValue = oRow.Situacion
        Else
            ddlSituacion.SelectedIndex = 0
        End If
        If oRow.CodigoRenta.ToString <> "" Then
            ddlTipoRenta.SelectedIndex = ddlTipoRenta.Items.IndexOf(ddlTipoRenta.Items.FindByValue(oRow.CodigoRenta.ToString()))
        Else
            ddlTipoRenta.SelectedIndex = 0
        End If
        If oRow.CodigoTarifa.ToString <> "" Then
            ddlTipoTarifa.SelectedIndex = ddlTipoTarifa.Items.IndexOf(ddlTipoTarifa.Items.FindByValue(oRow.CodigoTarifa.ToString()))
        Else
            ddlTipoTarifa.SelectedIndex = 0
        End If
        If oRow.Indicador.ToString <> "" Then
            ddlIndicador.SelectedIndex = ddlIndicador.Items.IndexOf(ddlIndicador.Items.FindByValue(oRow.Indicador.ToString()))
        Else
            ddlIndicador.SelectedIndex = 0
        End If
        Me.tbDescripcion.Text = oRow.Descripcion.ToString().ToUpper.Trim
        Me.tbCodigo.Text = oRow.CodigoComision.ToString()
        Me.tbValor.Text = oRow.ValorComision.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        Me.hd.Value = oRow.CodigoComision.ToString()
        Me.hdMercado.Value = oRow.CodigoMercado.ToString()
        Me.hdRenta.Value = oRow.CodigoRenta.ToString()
        Me.chkGeneraImpuestos.Checked = oRow.GeneraImpuestos
    End Sub
    Private Function crearObjeto() As ImpuestosComisionesBE
        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        Dim oRow As ImpuestosComisionesBE.ImpuestosComisionesRow
        oRow = DirectCast(oImpuestosComisionesBE.ImpuestosComisiones.NewRow(), ImpuestosComisionesBE.ImpuestosComisionesRow)
        oRow.Situacion = Me.ddlSituacion.SelectedValue()
        oRow.CodigoTarifa = Me.ddlTipoTarifa.SelectedValue()
        oRow.BaseCalculo = 0 'Me.txtBaseCalculo.Text.Replace(".", UIUtility.DecimalSeparator())
        oRow.Descripcion = Me.tbDescripcion.Text.ToString.Trim.ToUpper
        oRow.ValorComision = Me.tbValor.Text.Replace(",", "").Replace(".", UIUtility.DecimalSeparator())
        oRow.Indicador = ddlIndicador.SelectedValue.ToString
        If Not Me.hd.Value.Equals(String.Empty) Then
            oRow.CodigoComision = hd.Value
        Else
            oRow.CodigoComision = tbCodigo.Text.ToString.Trim.ToUpper
        End If
        oRow.CodigoMercado = ddlBolsa.SelectedValue
        oRow.CodigoRenta = ddlTipoRenta.SelectedValue.ToUpper
        oRow.CodigoMoneda = ddlMonedaValorFijo.SelectedValue
        oRow.GeneraImpuestos = chkGeneraImpuestos.Checked
        oImpuestosComisionesBE.ImpuestosComisiones.AddImpuestosComisionesRow(oRow)
        oImpuestosComisionesBE.ImpuestosComisiones.AcceptChanges()
        Return oImpuestosComisionesBE
    End Function
    Private Sub CargarCombos()
        Dim tablaTipoRenta As Data.DataTable
        Dim tablaMercado As Data.DataTable
        Dim tablaSituacion As DataTable
        Dim tablaTarifa As DataTable
        Dim tablaBase As DataTable
        Dim tablaBolsa As DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oMercadoBM As New MercadoBM
        Dim oPlazaBM As New PlazaBM
        Dim oMonedaBM As New MonedaBM

        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        tablaMercado = oMercadoBM.Listar(DatosRequest).Tables(0)
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaTarifa = oParametrosGenerales.ListarTipoTarifa(DatosRequest)
        tablaBase = oParametrosGenerales.ListarBaseImp(DatosRequest)
        tablaBolsa = oPlazaBM.Listar(Nothing).Tables(0)
        Dim DtTablaMoneda As DataTable = oMonedaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(Me.ddlBolsa, tablaBolsa, "CodigoPlaza", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlTipoTarifa, tablaTarifa, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlIndicador, tablaBase, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlMonedaValorFijo, DtTablaMoneda, "CodigoMoneda", "CodigoMoneda", True)
    End Sub
    Private Sub LimpiarCampos()
        Me.tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlTipoRenta.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
        Me.tbValor.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlBolsa.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
        Me.ddlIndicador.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
        Me.ddlTipoTarifa.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO        
        Me.ddlSituacion.SelectedValue = "A"
        Me.ddlMonedaValorFijo.SelectedValue = Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO
    End Sub
    Private Sub CargarPagina()
        Dim strCodigoComision, strCodMercado, StrCodRenta As String
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cCom") Is Nothing Then
                Me.hd.Value = Request.QueryString("cCom")
                Me.hdMercado.Value = Request.QueryString("cMer")
                Me.hdRenta.Value = Request.QueryString("cRenta")
                strCodigoComision = Request.QueryString("cCom")
                strCodMercado = Request.QueryString("cMer")
                StrCodRenta = Request.QueryString("cRenta")
                CargarRegistro(strCodigoComision, strCodMercado, StrCodRenta)
                Me.tbCodigo.Enabled = False
                Me.ddlBolsa.Enabled = False
                Me.ddlTipoRenta.Enabled = False
            Else
                Me.tbCodigo.Enabled = True
                Me.ddlBolsa.Enabled = True
                Me.ddlTipoRenta.Enabled = True
                Me.hd.Value = String.Empty
                Me.hdMercado.Value = String.Empty
                Me.hdRenta.Value = String.Empty
            End If
        End If
    End Sub
    Private Sub Aceptar()
        Dim blnExisteItem As Boolean
        If (Me.hd.Value.Equals(String.Empty)) Then
            blnExisteItem = ExisteEntidad()
            If Not blnExisteItem Then
                Insertar()
            Else
                Me.AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            End If
        Else
            Modificar()
        End If
    End Sub
#End Region
End Class