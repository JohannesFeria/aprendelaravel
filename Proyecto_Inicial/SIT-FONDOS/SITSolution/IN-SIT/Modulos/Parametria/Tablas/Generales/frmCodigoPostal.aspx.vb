Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmCodigoPostal
    Inherits BasePage

#Region "*/ Metodos de Pagina */"
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
        Dim oCodigoPostalBM As New CodigoPostalBM
        Dim oCodigoPostalBE As New CodigoPostalBE

        'Si no tiene nada es porque es un nuevo registro
        Try
            oCodigoPostalBE = crearObjeto()
            If hd.Value = "" Then
                If verificarExistenciaCodigoPostal() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If

                oCodigoPostalBM.Insertar(oCodigoPostalBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oCodigoPostalBM.Modificar(oCodigoPostalBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaCodigosPostales.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancenlar la operación")
        End Try
    End Sub
#End Region

#Region "*/ Funciones Personalizadas */"
    Public Sub cargarRegistro(ByVal codigo As String)
        Dim oCodigoPostalBM As New CodigoPostalBM
        Dim oCodigoPostal As New CodigoPostalBE
        oCodigoPostal = oCodigoPostalBM.SeleccionarPorFiltro(codigo, "", "", DatosRequest)
        tbCodigo.Enabled = False

        hd.Value = CType(oCodigoPostal.CodigoPostal.Rows(0), CodigoPostalBE.CodigoPostalRow).CodigoPostal.ToString()
        tbCodigo.Text = CType(oCodigoPostal.CodigoPostal.Rows(0), CodigoPostalBE.CodigoPostalRow).CodigoPostal.ToString()
        tbDescripcion.Text = CType(oCodigoPostal.CodigoPostal.Rows(0), CodigoPostalBE.CodigoPostalRow).Descripcion.ToString()
        ddlSituacion.SelectedValue = CType(oCodigoPostal.CodigoPostal.Rows(0), CodigoPostalBE.CodigoPostalRow).Situacion.ToString()
    End Sub

    Public Function crearObjeto() As CodigoPostalBE
        Dim oCodigoPostalBE As New CodigoPostalBE
        Dim oRow As CodigoPostalBE.CodigoPostalRow
        oRow = CType(oCodigoPostalBE.CodigoPostal.NewCodigoPostalRow(), CodigoPostalBE.CodigoPostalRow)

        oRow.CodigoPostal = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = tbDescripcion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = ddlSituacion.SelectedValue

        IIf(hd.Value <> "", oRow.CodigoPostal = hd.Value, oRow.CodigoPostal = tbCodigo.Text.Trim)

        oCodigoPostalBE.CodigoPostal.AddCodigoPostalRow(oRow)
        oCodigoPostalBE.CodigoPostal.AcceptChanges()

        Return oCodigoPostalBE
    End Function

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Public Sub LimpiarCampos()
        tbCodigo.Text = ""
        tbDescripcion.Text = ""
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function verificarExistenciaCodigoPostal() As Boolean
        Dim oCodigoPostalBM As New CodigoPostalBM
        Dim oCodigoPostalBE As New CodigoPostalBE
        oCodigoPostalBE = oCodigoPostalBM.SeleccionarPorFiltro(tbCodigo.Text.Trim, "", "", DatosRequest)
        If oCodigoPostalBE.CodigoPostal.Rows.Count > 0 Then
            verificarExistenciaCodigoPostal = True
        Else
            verificarExistenciaCodigoPostal = False
        End If
    End Function
#End Region
End Class
