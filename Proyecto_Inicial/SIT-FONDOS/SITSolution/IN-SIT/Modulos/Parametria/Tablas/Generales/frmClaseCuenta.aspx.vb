Option Explicit On
Option Strict On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmClaseCuentas
    Inherits BasePage

#Region "/* Metodos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaClaseCuentas.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

#End Region

#Region "/* Funciones Personalizadas */"

    Private Sub CargarRegistro(ByVal codigo As String)
        Dim oClaseCuentaBM As New ClaseCuentaBM
        Dim oClaseCuentaBE As ClaseCuentaBE
        Dim oRow As ClaseCuentaBE.ClaseCuentaRow

        oClaseCuentaBE = oClaseCuentaBM.Seleccionar(codigo, Me.DatosRequest)

        txtCodigo.Enabled = False

        oRow = DirectCast(oClaseCuentaBE.ClaseCuenta.Rows(0), ClaseCuentaBE.ClaseCuentaRow)

        hdnCodigoClaseCuenta.Value = oRow.CodigoClaseCuenta
        txtCodigo.Text = oRow.CodigoClaseCuenta
        txtDescripcion.Text = oRow.Descripcion
        ddlSituacion.SelectedValue = oRow.Situacion
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("codigo") Is Nothing Then
                hdnCodigoClaseCuenta.Value = Request.QueryString("codigo")
                CargarRegistro(hdnCodigoClaseCuenta.Value)
            Else
                hdnCodigoClaseCuenta.Value = String.Empty
            End If
        End If
    End Sub

    Private Function ObtenerInstancia() As ClaseCuentaBE
        Dim oClaseCuentaBE As New ClaseCuentaBE
        Dim oRow As ClaseCuentaBE.ClaseCuentaRow

        oRow = DirectCast(oClaseCuentaBE.ClaseCuenta.NewClaseCuentaRow(), ClaseCuentaBE.ClaseCuentaRow)

        oRow.CodigoClaseCuenta = Me.txtCodigo.Text
        oRow.Descripcion = Me.txtDescripcion.Text
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.Host = ""

        IIf(Not Me.hdnCodigoClaseCuenta.Value.Equals(String.Empty), oRow.CodigoClaseCuenta = Me.hdnCodigoClaseCuenta.Value, oRow.CodigoClaseCuenta = Me.txtCodigo.Text.Trim())

        oClaseCuentaBE.ClaseCuenta.AddClaseCuentaRow(oRow)
        oClaseCuentaBE.ClaseCuenta.AcceptChanges()

        Return oClaseCuentaBE
    End Function

    Private Sub CargarCombos()
        Dim dtSituacion As DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        dtSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, dtSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub LimpiarCampos()
        txtCodigo.Text = String.Empty
        txtDescripcion.Text = String.Empty
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function VerificarExisteEntidad() As Boolean
        Dim oClaseCuentaBM As New ClaseCuentaBM
        Dim oClaseCuentaBE As ClaseCuentaBE

        oClaseCuentaBE = oClaseCuentaBM.Seleccionar(Me.txtCodigo.Text, Me.DatosRequest)
        Return oClaseCuentaBE.ClaseCuenta.Rows.Count > 0
    End Function

    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean

        If hdnCodigoClaseCuenta.Value.Equals(String.Empty) Then
            blnExisteEntidad = VerificarExisteEntidad()

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
        Dim oClaseCuentaBE As ClaseCuentaBE
        Dim oClaseCuentaBM As New ClaseCuentaBM

        oClaseCuentaBE = ObtenerInstancia()
        oClaseCuentaBM.Insertar(oClaseCuentaBE, Me.DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

    Private Sub Modificar()
        Dim oClaseCuentaBE As ClaseCuentaBE
        Dim oClaseCuentaBM As New ClaseCuentaBM

        oClaseCuentaBE = ObtenerInstancia()
        oClaseCuentaBM.Modificar(oClaseCuentaBE, Me.DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

#End Region
End Class
