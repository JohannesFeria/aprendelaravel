Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmSectorEmpresarial
    Inherits BasePage

#Region "/* Metodos de Pagina /*"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oSectorEmpresarialBM As New SectorEmpresarialBM
        Dim oSectorEmpresarialBE As New SectorEmpresarialBE
        'Si no tiene nada es porque es un nuevo registro
        Try
            oSectorEmpresarialBE = crearObjeto()
            If Me.hd.Value = "" Then
                If verificarExistenciaSectorEmpresarial() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oSectorEmpresarialBM.Insertar(oSectorEmpresarialBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oSectorEmpresarialBM.Modificar(oSectorEmpresarialBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaSectoresEmpresariales.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas"
    Public Sub cargarRegistro(ByVal codigo As String)
        Dim oSectorEmpresarialBM As New SectorEmpresarialBM
        Dim oSectorEmpresarial As New SectorEmpresarialBE
        oSectorEmpresarial = oSectorEmpresarialBM.SeleccionarPorFiltro(codigo, "", "", datosRequest)
        txtCodigo.Enabled = False

        Me.hd.Value = CType(oSectorEmpresarial.SectorEmpresarial.Rows(0), SectorEmpresarialBE.SectorEmpresarialRow).CodigoSectorEmpresarial.ToString()
        Me.txtCodigo.Text = CType(oSectorEmpresarial.SectorEmpresarial.Rows(0), SectorEmpresarialBE.SectorEmpresarialRow).CodigoSectorEmpresarial.ToString()
        Me.txtNombre.Text = CType(oSectorEmpresarial.SectorEmpresarial.Rows(0), SectorEmpresarialBE.SectorEmpresarialRow).Descripcion.ToString()
        Me.ddlSituacion.SelectedValue = CType(oSectorEmpresarial.SectorEmpresarial.Rows(0), SectorEmpresarialBE.SectorEmpresarialRow).Situacion.ToString()
    End Sub

    Public Function crearObjeto() As SectorEmpresarialBE
        Dim oSectorEmpresarialBE As New SectorEmpresarialBE
        Dim oRow As SectorEmpresarialBE.SectorEmpresarialRow
        oRow = CType(oSectorEmpresarialBE.SectorEmpresarial.NewSectorEmpresarialRow(), SectorEmpresarialBE.SectorEmpresarialRow)

        oRow.CodigoSectorEmpresarial = txtCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = Me.txtNombre.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        IIf(hd.Value <> "", oRow.CodigoSectorEmpresarial = hd.Value, oRow.CodigoSectorEmpresarial = Me.txtCodigo.Text.Trim)

        oSectorEmpresarialBE.SectorEmpresarial.AddSectorEmpresarialRow(oRow)
        oSectorEmpresarialBE.SectorEmpresarial.AcceptChanges()

        Return oSectorEmpresarialBE

    End Function

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Sub LimpiarCampos()
        Me.txtCodigo.Text = ""
        Me.txtNombre.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function verificarExistenciaSectorEmpresarial() As Boolean
        Dim oSectorEmpresarialBM As New SectorEmpresarialBM
        Dim oSectorEmpresarialBE As New SectorEmpresarialBE
        oSectorEmpresarialBE = oSectorEmpresarialBM.Seleccionar(Me.txtCodigo.Text.Trim, DatosRequest)
        If oSectorEmpresarialBE.SectorEmpresarial.Rows.Count > 0 Then
            verificarExistenciaSectorEmpresarial = True
        Else
            verificarExistenciaSectorEmpresarial = False
        End If
    End Function
#End Region
End Class
