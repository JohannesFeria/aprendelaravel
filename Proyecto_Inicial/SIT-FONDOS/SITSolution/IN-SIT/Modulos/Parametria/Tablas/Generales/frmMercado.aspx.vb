Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmMercado
    Inherits BasePage

#Region " /* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not (Request.QueryString("cod") = Nothing) Then
                hd.Value = Request.QueryString("cod")
                cargarRegistro(hd.Value)
            Else
                hd.Value = ""
            End If
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oMercadoBM As New MercadoBM
        Dim oMercadoBE As New MercadoBE

        'Si no tiene nada es porque es un nuevo registro
        Try
            oMercadoBE = crearObjeto()
            If Me.hd.Value = "" Then
                If verificarExistenciaMercado() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If

                oMercadoBM.Insertar(oMercadoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oMercadoBM.Modificar(oMercadoBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
            'Me.ibAceptar.Visible = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaMercados.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas"
    Public Sub cargarRegistro(ByVal codigo As String)
        Dim oMercadoBM As New MercadoBM
        Dim oMercado As New MercadoBE
        oMercado = oMercadoBM.SeleccionarPorFiltro(codigo, "", "", datosRequest)
        txtCodigo.Enabled = False

        Me.hd.Value = CType(oMercado.Mercado.Rows(0), MercadoBE.MercadoRow).CodigoMercado.ToString()
        Me.txtCodigo.Text = CType(oMercado.Mercado.Rows(0), MercadoBE.MercadoRow).CodigoMercado.ToString()
        Me.tbDescripcion.Text = CType(oMercado.Mercado.Rows(0), MercadoBE.MercadoRow).Descripcion.ToString()
        Me.ddlSituacion.SelectedValue = CType(oMercado.Mercado.Rows(0), MercadoBE.MercadoRow).Situacion.ToString()

    End Sub
    Public Function crearObjeto() As MercadoBE
        Dim oMercadoBE As New MercadoBE
        Dim oRow As MercadoBE.MercadoRow
        oRow = CType(oMercadoBE.Mercado.NewMercadoRow(), MercadoBE.MercadoRow)

        oRow.CodigoMercado = txtCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = Me.tbDescripcion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        IIf(hd.Value <> "", oRow.CodigoMercado = hd.Value, oRow.CodigoMercado = Me.txtCodigo.Text.Trim)

        oMercadoBE.Mercado.AddMercadoRow(oRow)
        oMercadoBE.Mercado.AcceptChanges()
        Return oMercadoBE

    End Function
    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub
    Sub LimpiarCampos()
        Me.txtCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
    End Sub
    Public Function verificarExistenciaMercado()
        Dim oMercadoBM As New MercadoBM
        Dim oMercadoBE As New MercadoBE
        oMercadoBE = oMercadoBM.SeleccionarPorFiltro(Me.txtCodigo.Text.Trim, "", "", DatosRequest)
        If oMercadoBE.Mercado.Rows.Count > 0 Then
            verificarExistenciaMercado = True
        Else
            verificarExistenciaMercado = False
        End If
    End Function
#End Region

End Class
