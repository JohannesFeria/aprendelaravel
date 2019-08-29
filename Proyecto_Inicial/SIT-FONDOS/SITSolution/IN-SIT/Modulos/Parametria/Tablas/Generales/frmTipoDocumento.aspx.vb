Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmTipoDocumento
    Inherits BasePage

#Region " /* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oTipoDocumentoBM As New TipoDocumentoBM
        Dim oTipoDocumentoBE As New TipoDocumentoBE

        Try
            oTipoDocumentoBE = crearObjeto()
            If hd.Value = "" Then
                If verificarExistenciaTipoDocumento() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If

                oTipoDocumentoBM.Insertar(oTipoDocumentoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oTipoDocumentoBM.Modificar(oTipoDocumentoBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaTipoDocumentos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas */"
    Public Sub LimpiarCampos()
        tbCodigo.Text = ""
        tbDescripcion.Text = ""
        tbLongitudMax.Text = ""
        ddlTipoPersona.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
    End Sub

    Public Sub cargarRegistro(ByVal codigo As String)

        Dim oTipoDocumentoBM As New TipoDocumentoBM
        Dim oTipoDocumento As New TipoDocumentoBE

        Dim oRow As TipoDocumentoBE.TipoDocumentoRow

        oTipoDocumento = oTipoDocumentoBM.SeleccionarPorFiltro(codigo, "", "", DatosRequest)

        oRow = DirectCast(oTipoDocumento.TipoDocumento.Rows(0), TipoDocumentoBE.TipoDocumentoRow)

        tbCodigo.Enabled = False

        hd.Value = oRow.CodigoTipoDocumento
        tbCodigo.Text = oRow.CodigoTipoDocumento
        tbDescripcion.Text = oRow.Descripcion
        tbLongitudMax.Text = oRow.LongitudMaxima.ToString()
        If Not oRow.IsDigitoChekeoNull() Then

            rdbChekeo.SelectedValue = oRow.DigitoChekeo

        End If
        ddlTipoPersona.SelectedValue = oRow.TipoPersona
        ddlSituacion.SelectedValue = oRow.Situacion

    End Sub
    Public Function crearObjeto() As TipoDocumentoBE
        Dim oTipoDocumentoBE As New TipoDocumentoBE
        Dim oRow As TipoDocumentoBE.TipoDocumentoRow
        oRow = CType(oTipoDocumentoBE.TipoDocumento.NewTipoDocumentoRow(), TipoDocumentoBE.TipoDocumentoRow)

        oRow.CodigoTipoDocumento = tbCodigo.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Descripcion = tbDescripcion.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.LongitudMaxima = tbLongitudMax.Text.ToString.Trim
        oRow.DigitoChekeo = rdbChekeo.SelectedValue
        oRow.TipoPersona = ddlTipoPersona.SelectedValue
        oRow.Situacion = ddlSituacion.SelectedValue

        IIf(hd.Value <> "", oRow.CodigoTipoDocumento = hd.Value, oRow.CodigoTipoDocumento = tbCodigo.Text.Trim)

        oTipoDocumentoBE.TipoDocumento.AddTipoDocumentoRow(oRow)
        oTipoDocumentoBE.TipoDocumento.AcceptChanges()

        Return oTipoDocumentoBE

    End Function
    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim tablaTipoPersona As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        tablaTipoPersona = oParametrosGenerales.ListarTipoPersona(DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoPersona, tablaTipoPersona, "Valor", "Nombre", True)
    End Sub

    Private Function verificarExistenciaTipoDocumento() As Boolean
        Dim oTipoDocumentoBM As New TipoDocumentoBM
        Dim oTipoDocumentoBE As New TipoDocumentoBE
        oTipoDocumentoBE = oTipoDocumentoBM.SeleccionarPorFiltro(tbCodigo.Text.Trim, "", "", DatosRequest)
        If oTipoDocumentoBE.TipoDocumento.Rows.Count > 0 Then
            verificarExistenciaTipoDocumento = True
        Else
            verificarExistenciaTipoDocumento = False
        End If
    End Function
#End Region

End Class