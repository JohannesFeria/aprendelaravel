Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Generales_frmMonedas
    Inherits BasePage
#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaMonedas.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub
#End Region
#Region " /* Metodos Personalizados */ "
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cod") Is Nothing Then
                hd.Value = Request.QueryString("cod")
                cargarRegistro(hd.Value)
            Else
                hd.Value = String.Empty
            End If
        End If
    End Sub
    Private Sub Insertar()
        Dim oMonedaBM As New MonedaBM
        Dim oMonedaBE As New MonedaBE
        oMonedaBE = crearObjeto()
        oMonedaBM.Insertar(oMonedaBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub
    Private Sub Modificar()
        Dim oMonedaBM As New MonedaBM
        Dim oMonedaBE As New MonedaBE
        oMonedaBE = crearObjeto()
        oMonedaBM.Modificar(oMonedaBE, DatosRequest)
        AlertaJS("Los datos fueron modificados correctamente")
    End Sub
    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If Me.hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = verificarExistenciaMoneda()
            If blnExisteEntidad Then
                Me.AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            ElseIf ddlmoneda.SelectedValue = "" Then
                AlertaJS("Seleccione una moneda de Tipo de Cambio")
            Else
                If VerificarExistenciaMonedaSBS() Then
                    AlertaJS(ObtenerMensaje("ALERT157"))
                Else
                    Insertar()
                End If
            End If
        Else
            Modificar()
        End If
    End Sub
    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oMonedaBM As New MonedaBM
        Dim oMoneda As New MonedaBE
        oMoneda = oMonedaBM.SeleccionarPorFiltro(codigo, String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest)
        Me.txtCodigo.Enabled = False
        Me.hd.Value = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).CodigoMoneda.ToString()
        Me.txtCodigo.Text = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).CodigoMoneda.ToString()
        Me.txtCodigoSBS.Text = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).CodigoMonedaSBS.ToString()
        Me.txtDescripcion.Text = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).Descripcion.ToString()
        Me.txtSimbolo.Text = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).Simbolo.ToString()
        Me.ddlTipoCalculo.SelectedValue = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).TipoCalculo.ToString()
        Me.ddlSituacion.SelectedValue = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).Situacion.ToString()
        Me.txtCodigoIso.Text = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).CodigoISO.ToString()
        Me.txtSinonimoIso.Text = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).SinonimoISO.ToString()
        ddlmoneda.SelectedValue = DirectCast(oMoneda.Moneda.Rows(0), MonedaBE.MonedaRow).CodMonedaTipoCambio.ToString()
    End Sub
    Private Function crearObjeto() As MonedaBE
        Dim oMonedaBE As New MonedaBE
        Dim oRow As MonedaBE.MonedaRow
        oRow = DirectCast(oMonedaBE.Moneda.NewMonedaRow(), MonedaBE.MonedaRow)
        oRow.CodigoMoneda = txtCodigo.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.CodigoMonedaSBS = txtCodigoSBS.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Descripcion = Me.txtDescripcion.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Simbolo = txtSimbolo.Text.ToString.ToUpper.ToUpper.TrimStart.TrimEnd
        oRow.TipoCalculo = ddlTipoCalculo.SelectedValue
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.CodigoISO = Me.txtCodigoIso.Text
        oRow.SinonimoISO = Me.txtSinonimoIso.Text
        oRow.CodMonedaTipoCambio = ddlmoneda.SelectedValue
        IIf(Not hd.Value.Equals(String.Empty), oRow.CodigoMoneda = hd.Value, oRow.CodigoMoneda = Me.txtCodigo.Text.Trim)
        oMonedaBE.Moneda.AddMonedaRow(oRow)
        oMonedaBE.Moneda.AcceptChanges()
        Return oMonedaBE
    End Function
    Public Sub CargarCombos()
        Dim oMonedaBM As New MonedaBM
        Dim tablaSituacion As New DataTable
        Dim tablaTipoCalculo As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlmoneda, oMonedaBM.Listar("A").Tables(0), "CodigoMoneda", "Descripcion", True)
        ddlmoneda.SelectedValue = "NSOL"
        tablaTipoCalculo = oParametrosGenerales.Listar("BaseCalc", DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlTipoCalculo, tablaTipoCalculo, "Valor", "Nombre", True)
    End Sub
    Private Sub LimpiarCampos()
        Me.txtCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtCodigoSBS.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtSimbolo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0
        Me.ddlTipoCalculo.SelectedIndex = 0
    End Sub
    Private Function verificarExistenciaMoneda() As Boolean
        Dim oMonedaBM As New MonedaBM
        Dim oMonedaBE As New MonedaBE
        oMonedaBE = oMonedaBM.SeleccionarPorFiltro(Me.txtCodigo.Text.Trim, String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest)
        Return oMonedaBE.Moneda.Rows.Count > 0
    End Function
    Private Function VerificarExistenciaMonedaSBS() As Boolean
        Dim oMonedaBM As New MonedaBM
        Dim oMonedaBE As New DataSet
        oMonedaBE = oMonedaBM.SeleccionarPorCodigoSBS(txtCodigoSBS.Text.Trim, DatosRequest)
        Return oMonedaBE.Tables(0).Rows.Count > 0
    End Function
#End Region
End Class