Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Tesoreria_frmTipoOperaciones
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    tbCodigo.Enabled = False
                    Me.hd.Value = Request.QueryString("cod")
                    CargarRegistro(hd.Value)
                Else
                    tbCodigo.Enabled = True
                    Me.hd.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try

    End Sub

    Protected Sub ibAceptar_Click(sender As Object, e As System.EventArgs) Handles ibAceptar.Click
        Try
            Dim oTipoOperacionBM As New TipoOperacionBM
            Dim oTipoOperacionBE As New TipoOperacionBE
            Dim blnExisteEntidad As Boolean
            If Me.hd.Value.Equals(String.Empty) Then
                blnExisteEntidad = verificarExistencia()
                If Not blnExisteEntidad Then
                    oTipoOperacionBE = crearObjeto()
                    oTipoOperacionBM.Insertar(oTipoOperacionBE, DatosRequest)
                    AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
                    tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
                    tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
                Else
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                End If
            Else
                oTipoOperacionBE = crearObjeto()
                oTipoOperacionBM.Modificar(oTipoOperacionBE, DatosRequest)
                AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Protected Sub ibSalir_Click(sender As Object, e As System.EventArgs) Handles ibSalir.Click
        Try
            Response.Redirect("frmBusquedaTipoOperaciones.aspx")
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal Codigo As String)

        Dim oTipoOperacionBM As New TipoOperacionBM
        Dim oTipoOperacionBE As New TipoOperacionBE

        oTipoOperacionBE = oTipoOperacionBM.Seleccionar(Codigo, DatosRequest)

        Me.hd.Value = DirectCast(oTipoOperacionBE.TipoOperacion.Rows(0), TipoOperacionBE.TipoOperacionRow).CodigoTipoOperacion.ToString()
        Me.tbCodigo.Text = DirectCast(oTipoOperacionBE.TipoOperacion.Rows(0), TipoOperacionBE.TipoOperacionRow).CodigoTipoOperacion.ToString()
        Me.tbDescripcion.Text() = DirectCast(oTipoOperacionBE.TipoOperacion.Rows(0), TipoOperacionBE.TipoOperacionRow).Descripcion.ToString().Trim.ToUpper
        Me.ddlSituacion.SelectedValue = DirectCast(oTipoOperacionBE.TipoOperacion.Rows(0), TipoOperacionBE.TipoOperacionRow).Situacion.ToString()

    End Sub

    Public Function crearObjeto() As TipoOperacionBE
        Dim oTipoOperacionBE As New TipoOperacionBE
        Dim oRow As TipoOperacionBE.TipoOperacionRow

        oRow = DirectCast(oTipoOperacionBE.TipoOperacion.NewRow(), TipoOperacionBE.TipoOperacionRow)

        oRow.Descripcion = Me.tbDescripcion.Text.Trim().ToUpper
        oRow.Situacion = Me.ddlSituacion.SelectedValue.ToString()

        If Not Me.hd.Value.Equals(String.Empty) Then
            oRow.CodigoTipoOperacion = hd.Value
        Else
            oRow.CodigoTipoOperacion = Me.tbCodigo.Text.Trim().ToUpper
        End If

        oTipoOperacionBE.TipoOperacion.AddTipoOperacionRow(oRow)
        oTipoOperacionBE.TipoOperacion.AcceptChanges()

        Return oTipoOperacionBE

    End Function

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoOperacionBM As New TipoOperacionBM
        Dim oTipoOperacionBE As New TipoOperacionBE
        oTipoOperacionBE = oTipoOperacionBM.Seleccionar(Me.tbCodigo.Text, DatosRequest)
        Return oTipoOperacionBE.TipoOperacion.Rows.Count > 0
    End Function

#End Region

End Class
