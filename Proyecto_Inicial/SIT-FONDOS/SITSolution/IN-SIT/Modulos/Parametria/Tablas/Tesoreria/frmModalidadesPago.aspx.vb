Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Tesoreria_frmModalidadesPago
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not (Request.QueryString("cod") = Nothing) Then
                'Llega un registro para edición
                txtCodigo.Enabled = False
                hd.Value = Request.QueryString("cod")
                CargarRegistro(hd.Value)
            Else
                txtCodigo.Enabled = True
                hd.Value = String.Empty
            End If

        End If
    End Sub

#Region " /* Metodos de Pagina */ "
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oModalidadPagoBM As New ModalidadPagoBM
        Dim oModalidadPagoBE As New ModalidadPagoBE
        If (hd.Value = "") Then
            'Si no tiene nada es porque es un nuevo registro
            Try
                If (txtCodigo.Text = String.Empty Or txtDescripcion.Text = String.Empty) Then
                    AlertaJS("Debe llenar todos los campos")
                Else
                    If verificarExistencia() = False Then
                        oModalidadPagoBE = crearObjeto()
                        oModalidadPagoBM.Insertar(oModalidadPagoBE, DatosRequest)
                        AlertaJS("Los datos fueron grabados correctamente")
                        LimpiarCampos()
                    Else
                        AlertaJS("Este registro ya existe")
                    End If
                End If
            Catch ex As Exception
                AlertaJS(ex.Message)
            End Try
        Else
            Try
                oModalidadPagoBE = crearObjeto()
                oModalidadPagoBM.Modificar(oModalidadPagoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            Catch ex As Exception
                AlertaJS(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaModalidadesPago.aspx")
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub LimpiarCampos()
        txtCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        txtDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedValue = "A"
    End Sub

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oModalidadPagoBM As New ModalidadPagoBM
        Dim oModalidadPagoBE As New ModalidadPagoBE

        oModalidadPagoBE = oModalidadPagoBM.Seleccionar(Codigo, DatosRequest)

        hd.Value = DirectCast(oModalidadPagoBE.ModalidadPago.Rows(0), ModalidadPagoBE.ModalidadPagoRow).CodigoModalidadPago
        txtCodigo.Text = DirectCast(oModalidadPagoBE.ModalidadPago.Rows(0), ModalidadPagoBE.ModalidadPagoRow).CodigoModalidadPago
        txtDescripcion.Text() = DirectCast(oModalidadPagoBE.ModalidadPago.Rows(0), ModalidadPagoBE.ModalidadPagoRow).Descripcion.ToString().Trim.ToUpper
        ddlSituacion.SelectedValue = DirectCast(oModalidadPagoBE.ModalidadPago.Rows(0), ModalidadPagoBE.ModalidadPagoRow).Situacion
    End Sub

    Public Function crearObjeto() As ModalidadPagoBE
        Dim oModalidadPagoBE As New ModalidadPagoBE
        Dim oRow As ModalidadPagoBE.ModalidadPagoRow
        oRow = CType(oModalidadPagoBE.ModalidadPago.NewRow(), ModalidadPagoBE.ModalidadPagoRow)

        oRow.Descripcion = Me.txtDescripcion.Text.Trim.ToUpper
        oRow.Naturaleza = ""
        oRow.Situacion = Me.ddlSituacion.SelectedValue.ToString()

        If Not Me.hd.Value.Equals(String.Empty) Then
            oRow.CodigoModalidadPago = hd.Value
        Else
            oRow.CodigoModalidadPago = Me.txtCodigo.Text.Trim.ToUpper
        End If

        oModalidadPagoBE.ModalidadPago.AddModalidadPagoRow(oRow)
        oModalidadPagoBE.ModalidadPago.AcceptChanges()

        Return oModalidadPagoBE
    End Function

    Private Sub CargarCombos()
        Dim tablaNaturaleza As New Data.DataTable
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oModalidadPagoBM As New ModalidadPagoBM
        Dim oModalidadPagoBE As New ModalidadPagoBE
        oModalidadPagoBE = oModalidadPagoBM.Seleccionar(Me.txtCodigo.Text, DatosRequest)
        If oModalidadPagoBE.ModalidadPago.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class
