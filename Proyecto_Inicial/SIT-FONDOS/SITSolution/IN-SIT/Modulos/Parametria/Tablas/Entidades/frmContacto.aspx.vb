Option Explicit On
Option Strict On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmContacto
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not Request.QueryString("codigo") Is Nothing Then
                    hdnCodigo.Value = Request.QueryString("codigo")
                    cargarRegistro(hdnCodigo.Value)
                Else
                    hdnCodigo.Value = String.Empty
                    tbCodigo.Enabled = False
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaContacto.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

#End Region

#Region "*/ Funciones Personalizadas */"

    Private Sub Aceptar()
        Dim blnExisteEntidad As New Boolean
        If Me.hdnCodigo.Value.Equals(String.Empty) Then
            Insertar()
        Else
            Modificar()
        End If
    End Sub

    Private Sub Insertar()
        Dim oContactoBE As ContactoBE
        Dim oContectoBM As New ContactoBM

        oContactoBE = ObtenerInstancia()
        oContectoBM.Insertar(oContactoBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

    Private Sub Modificar()
        Dim oContactoBE As ContactoBE
        Dim oContectoBM As New ContactoBM

        oContactoBE = ObtenerInstancia()
        oContectoBM.Modificar(oContactoBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oContactoBE As ContactoBE
        Dim oContectoBM As New ContactoBM
        Dim oRow As ContactoBE.ContactoRow

        oContactoBE = oContectoBM.Seleccionar(codigo, DatosRequest)
        tbCodigo.Enabled = False
        oRow = DirectCast(oContactoBE.Contacto.Rows(0), ContactoBE.ContactoRow)

        hdnCodigo.Value = oRow.CodigoContacto.ToString()
        tbCodigo.Text = oRow.CodigoContacto.ToString()
        txtObservaciones.Text = oRow.Observaciones
        tbDescripcion.Text = oRow.Descripcion.ToString()
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        ddlTipoContacto.SelectedValue = oRow.TipoContacto.ToString()
    End Sub

    Private Function ObtenerInstancia() As ContactoBE
        Dim oContactoBE As New ContactoBE
        Dim oRow As ContactoBE.ContactoRow

        oRow = DirectCast(oContactoBE.Contacto.NewContactoRow(), ContactoBE.ContactoRow)

        oRow.CodigoContacto = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = Me.tbDescripcion.Text.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.TipoContacto = Me.ddlTipoContacto.SelectedValue
        oRow.Observaciones = Me.txtObservaciones.Text

        IIf(Not hdnCodigo.Value.Equals(String.Empty), oRow.CodigoContacto = hdnCodigo.Value, oRow.CodigoContacto = Me.tbCodigo.Text.Trim)

        oContactoBE.Contacto.AddContactoRow(oRow)
        oContactoBE.Contacto.AcceptChanges()
        Return oContactoBE
    End Function

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        ddlTipoContacto.Items.Insert(0, New ListItem("Inversión", "I"))
        ddlTipoContacto.Items.Insert(1, New ListItem("Tesorería", "T"))
    End Sub

    Private Sub LimpiarCampos()
        tbCodigo.Text = String.Empty
        tbDescripcion.Text = String.Empty
        txtObservaciones.Text = String.Empty
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function ExisteEntidad() As Boolean
        Dim oContactoBE As ContactoBE
        Dim oContectoBM As New ContactoBM

        oContactoBE = oContectoBM.Seleccionar(Me.tbCodigo.Text.Trim, DatosRequest)
        Return oContactoBE.Contacto.Rows.Count > 0
    End Function

#End Region

End Class