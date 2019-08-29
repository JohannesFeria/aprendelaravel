Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmIntermediarioContacto
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.btnAceptar.Attributes.Add("onclick", "javascript:return Validar();")
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("codigo") = Nothing) Then
                    Me.hdCodigo.Value = Request.QueryString("codigo")
                    CargarControles(Me.hdCodigo.Value)
                Else
                    Me.hdCodigo.Value = String.Empty
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
            Response.Redirect("frmBusquedaIntermediarioContacto.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean

        If Me.hdCodigo.Value.Equals(String.Empty) Then
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

    Private Function ExisteEntidad() As Boolean

        Dim oIntermediarioContactoBM As New IntermediarioContactoBM
        Dim oIntermediarioContactoBE As IntermediarioContactoBE

        oIntermediarioContactoBE = oIntermediarioContactoBM.ExistenciaIntermediarioContacto(Me.ddlTercero.SelectedValue, Me.ddlContacto.SelectedValue, Me.DatosRequest)

        Return oIntermediarioContactoBE.IntermediarioContacto.Rows.Count > 0

    End Function

    Private Sub Insertar()

        Dim oIntermediarioContactoBM As New IntermediarioContactoBM
        Dim oIntermediarioContactoBE As IntermediarioContactoBE

        oIntermediarioContactoBE = Me.ObtenerInstancia()

        oIntermediarioContactoBM.Insertar(oIntermediarioContactoBE, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)

        LimpiarCampos()

    End Sub

    Private Sub Modificar()
        Dim oIntermediarioContactoBM As New IntermediarioContactoBM
        Dim oIntermediarioContactoBE As IntermediarioContactoBE

        oIntermediarioContactoBE = Me.ObtenerInstancia()

        oIntermediarioContactoBM.Modificar(oIntermediarioContactoBE, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)        
    End Sub

    Private Sub CargarControles(ByVal codigo As String)

        Dim oIntermediarioContactoBM As New IntermediarioContactoBM
        Dim oIntermediarioContactoBE As IntermediarioContactoBE

        Dim oRow As IntermediarioContactoBE.IntermediarioContactoRow

        Dim strCodigoTercero, strCodigoContacto As String

        strCodigoTercero = codigo.Split(","c)(0)
        strCodigoContacto = codigo.Split(","c)(1)

        oIntermediarioContactoBE = oIntermediarioContactoBM.SeleccionarPorFiltro(strCodigoTercero, strCodigoContacto, String.Empty, Me.DatosRequest)

        oRow = DirectCast(oIntermediarioContactoBE.IntermediarioContacto.Rows(0), IntermediarioContactoBE.IntermediarioContactoRow)

        Me.hdCodigo.Value = String.Format("{0},{1}", oRow.CodigoTercero, oRow.CodigoContacto)

        Me.ddlContacto.SelectedValue = oRow.CodigoContacto
        Me.ddlTercero.SelectedValue = oRow.CodigoTercero
        Me.tbAnexo1.Text = oRow.Anexo1
        Me.tbAnexo2.Text = oRow.Anexo2
        Me.tbAnexo3.Text = oRow.Anexo3
        Me.tbTelefono1.Text = oRow.Telefono1
        Me.tbTelefono2.Text = oRow.Telefono2
        Me.tbTelefono3.Text = oRow.Telefono3
        Me.ddlSituacion.SelectedValue = oRow.Situacion

        Me.ddlTercero.Enabled = False
        Me.ddlContacto.Enabled = False

    End Sub

    Private Function ObtenerInstancia() As IntermediarioContactoBE

        Dim oIntermediarioContactoBE As New IntermediarioContactoBE

        Dim oRow As IntermediarioContactoBE.IntermediarioContactoRow

        oRow = oIntermediarioContactoBE.IntermediarioContacto.NewIntermediarioContactoRow()
        oRow.CodigoTercero = Me.ddlTercero.SelectedValue
        oRow.CodigoContacto = Me.ddlContacto.SelectedValue
        oRow.Anexo1 = Me.tbAnexo1.Text
        oRow.Anexo2 = Me.tbAnexo2.Text
        oRow.Anexo3 = Me.tbAnexo3.Text
        oRow.Telefono1 = Me.tbTelefono1.Text
        oRow.Telefono2 = Me.tbTelefono2.Text
        oRow.Telefono3 = Me.tbTelefono3.Text
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        oIntermediarioContactoBE.IntermediarioContacto.AddIntermediarioContactoRow(oRow)

        oIntermediarioContactoBE.IntermediarioContacto.AcceptChanges()

        Return oIntermediarioContactoBE

    End Function

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

        Dim dtContacto As DataTable
        Dim oContactoBM As New ContactoBM

        dtContacto = oContactoBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlContacto, dtContacto, "CodigoContacto", "Descripcion", True)

        Dim dtTerceros As DataTable
        Dim oTerceroBM As New TercerosBM

        dtTerceros = oTerceroBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTercero, dtTerceros, "CodigoTercero", "Descripcion", True)

    End Sub

    Public Sub LimpiarCampos()

        Me.tbAnexo1.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbAnexo2.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbAnexo3.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbTelefono1.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbTelefono2.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbTelefono3.Text = Constantes.M_STR_TEXTO_INICIAL

        Me.ddlContacto.SelectedIndex = 0
        Me.ddlTercero.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0

    End Sub

#End Region

End Class
