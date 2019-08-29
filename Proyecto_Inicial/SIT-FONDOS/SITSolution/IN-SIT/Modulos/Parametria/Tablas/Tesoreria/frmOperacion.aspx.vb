Option Explicit On
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Entidades_frmOperacion
    Inherits BasePage
    Dim oClaseCuenta As New ClaseCuentaBM
#Region "/* Eventos de la Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarClaseCuenta()

                Dim oOperacionBM As New OperacionBM

                tbCodigo.Text = oOperacionBM.OperacionCodAuto()

                If Not Request.QueryString("codigo") Is Nothing Then
                    hdnCodigo.Value = Request.QueryString("codigo")
                    cargarRegistro(hdnCodigo.Value)
                Else
                    hdnCodigo.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If ddlClaseCuenta.SelectedValue = "" Then
                AlertaJS("Seleccione la clase de cuenta para continuar.")
            Else
                Aceptar()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaOperacion.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub
#End Region
#Region "*/ Funciones Personalizadas */"
    Private Sub CargarClaseCuenta()
        HelpCombo.LlenarComboBox(ddlClaseCuenta, oClaseCuenta.Listar().Tables(0), "CodigoClaseCuenta", "Descripcion", True, "--Seleccione--")
    End Sub
    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If hdnCodigo.Value.Equals(String.Empty) Then
            blnExisteEntidad = Me.ExisteEntidad()
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                Insertar()
            End If
        Else
            Modificar()
        End If
    End Sub
    Private Sub Insertar()
        Dim oOperacionBE As OperacionBE
        Dim oOperacionBM As New OperacionBM
        oOperacionBE = ObtenerInstancia()
        oOperacionBM.Insertar(oOperacionBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
        tbCodigo.Text = oOperacionBM.OperacionCodAuto()
    End Sub
    Private Sub Modificar()
        Dim oOperacionBE As OperacionBE
        Dim oOperacionBM As New OperacionBM
        oOperacionBE = ObtenerInstancia()
        oOperacionBM.Modificar(oOperacionBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub
    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oOperacionBE As OperacionBE
        Dim oOperacionBM As New OperacionBM
        Dim oRow As OperacionBE.OperacionRow
        oOperacionBE = oOperacionBM.Seleccionar(codigo, DatosRequest)
        tbCodigo.Enabled = False
        oRow = DirectCast(oOperacionBE.Operacion.Rows(0), OperacionBE.OperacionRow)
        hdnCodigo.Value = oRow.CodigoOperacion
        tbCodigo.Text = oRow.CodigoOperacion
        ddlTipoOperacion.SelectedValue = oRow.CodigoTipoOperacion
        tbDescripcion.Text = oRow.Descripcion
        ddlSituacion.SelectedValue = oRow.Situacion
        ddlClaseCuenta.SelectedValue = oRow.CodigoClaseCuenta
    End Sub
    Private Function ObtenerInstancia() As OperacionBE
        Dim oOperacionBE As New OperacionBE
        Dim oRow As OperacionBE.OperacionRow
        oRow = DirectCast(oOperacionBE.Operacion.NewOperacionRow(), OperacionBE.OperacionRow)
        oRow.CodigoOperacion = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = Me.tbDescripcion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.CodigoTipoOperacion = Me.ddlTipoOperacion.SelectedValue
        oRow.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
        IIf(Not hdnCodigo.Value.Equals(String.Empty), oRow.CodigoOperacion = hdnCodigo.Value, oRow.CodigoOperacion = Me.tbCodigo.Text.Trim)
        oOperacionBE.Operacion.AddOperacionRow(oRow)
        oOperacionBE.Operacion.AcceptChanges()
        Return oOperacionBE
    End Function
    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        Dim dtTipoOperacion As DataTable
        Dim oTipoOperacion As New TipoOperacionBM
        dtTipoOperacion = oTipoOperacion.Listar("A").TipoOperacion
        HelpCombo.LlenarComboBox(Me.ddlTipoOperacion, dtTipoOperacion, "CodigoTipoOperacion", "Descripcion", True)
    End Sub
    Private Sub LimpiarCampos()
        tbCodigo.Text = String.Empty
        tbDescripcion.Text = String.Empty
        ddlTipoOperacion.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oOperacionBE As OperacionBE
        Dim oOperacionBM As New OperacionBM
        oOperacionBE = oOperacionBM.Seleccionar(Me.tbCodigo.Text, Me.DatosRequest)
        Return oOperacionBE.Operacion.Rows.Count > 0
    End Function
#End Region
End Class