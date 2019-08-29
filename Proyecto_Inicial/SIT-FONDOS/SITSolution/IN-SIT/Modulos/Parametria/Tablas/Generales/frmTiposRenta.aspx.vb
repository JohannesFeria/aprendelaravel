Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmTiposRenta
    Inherits BasePage

#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    CargarRegistro(hd.Value)
                Else
                    hd.Value = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oTipoRentaBE As New TipoRentaBE
        Try
            oTipoRentaBE = crearObjeto()

            If Me.hd.Value = "" Then
                If verificarExistenciaTipoRenta() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If

                oTipoRentaBM.Insertar(oTipoRentaBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oTipoRentaBM.Modificar(oTipoRentaBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
            'Me.ibAceptar.Visible = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos en la Grilla")
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaTipoRenta.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub
#End Region

#Region " /* Metodos Personalizados */ "
    Public Sub CargarRegistro(ByVal codigo As String)
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oTipoRenta As New TipoRentaBE
        oTipoRenta = oTipoRentaBM.SeleccionarPorFiltro(codigo, "", "", DatosRequest)
        Me.tbCodigo.Enabled = False

        Me.hd.Value = CType(oTipoRenta.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow).CodigoRenta.ToString()
        Me.tbCodigo.Text = CType(oTipoRenta.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow).CodigoRenta.ToString()
        Me.tbDescripcion.Text = CType(oTipoRenta.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow).Descripcion.ToString()
        Me.ddlSituacion.SelectedValue = CType(oTipoRenta.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow).Situacion.ToString()
        'ini HDG OT 62087 Nro14-R23 20110223
        If CType(oTipoRenta.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow).IsTipoFactorNull Then
            Me.ddlTipoFactor.SelectedValue = ""
        Else
            Me.ddlTipoFactor.SelectedValue = CType(oTipoRenta.TipoRenta.Rows(0), TipoRentaBE.TipoRentaRow).TipoFactor.ToString()
        End If
        'fin HDG OT 62087 Nro14-R23 20110223
    End Sub
    Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim tablaTipoFactor As New DataTable    'HDG OT 62087 Nro14-R23 20110223
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        tablaTipoFactor = oParametrosGenerales.Listar_TipoFactor(DatosRequest)   'HDG OT 62087 Nro14-R23 20110223
        HelpCombo.LlenarComboBox(Me.ddlTipoFactor, tablaTipoFactor, "Valor", "Nombre", True)  'HDG OT 62087 Nro14-R23 20110223
    End Sub
    Sub LimpiarCampos()
        Me.tbCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
        Me.ddlTipoFactor.SelectedIndex = 0   'HDG OT 62087 Nro14-R23 20110223
    End Sub
    Public Function crearObjeto() As TipoRentaBE
        Dim oTipoRentaBE As New TipoRentaBE
        Dim oRow As TipoRentaBE.TipoRentaRow
        oRow = CType(oTipoRentaBE.TipoRenta.NewTipoRentaRow(), TipoRentaBE.TipoRentaRow)

        oRow.CodigoRenta = tbCodigo.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Descripcion = Me.tbDescripcion.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        'HDG OT 62087 Nro14-R23 20110223
        oRow.TipoFactor = IIf(Me.ddlTipoFactor.SelectedValue.ToString.Equals("--Seleccione--"), "", Me.ddlTipoFactor.SelectedValue.ToString)

        IIf(hd.Value <> "", oRow.CodigoRenta = hd.Value, oRow.CodigoRenta = Me.tbCodigo.Text.Trim)

        oTipoRentaBE.TipoRenta.AddTipoRentaRow(oRow)
        oTipoRentaBE.TipoRenta.AcceptChanges()

        Return oTipoRentaBE
    End Function
    Private Function verificarExistenciaTipoRenta() As Boolean
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oTipoRentaBE As New TipoRentaBE
        oTipoRentaBE = oTipoRentaBM.SeleccionarPorFiltro(Me.tbCodigo.Text.Trim, "", "", DatosRequest)
        If oTipoRentaBE.TipoRenta.Rows.Count > 0 Then
            verificarExistenciaTipoRenta = True
        Else
            verificarExistenciaTipoRenta = False
        End If
    End Function
#End Region
End Class
