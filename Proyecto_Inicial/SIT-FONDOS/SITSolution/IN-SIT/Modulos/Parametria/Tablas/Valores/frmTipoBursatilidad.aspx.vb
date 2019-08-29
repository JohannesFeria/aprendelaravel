Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmTipoBursatilidad
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    tbCodigo.Enabled = False
                    hd.Value = Request.QueryString("cod")
                    CargarRegistro(hd.Value)
                Else
                    tbCodigo.Enabled = False
                    hd.Value = ""
                    CargarSecuencial()
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oTipoBursatilidadBM As New TipoBursatilidadBM
        Dim oTipoBursatilidadBE As New TipoBursatilidadBE
        Try
            oTipoBursatilidadBE = crearObjeto()
            If hd.Value.Equals("") Then
                If Not verificarExistencia() Then
                    oTipoBursatilidadBM.Insertar(oTipoBursatilidadBE, DatosRequest)
                    AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
                    LimpiarCampos()
                    CargarSecuencial()
                Else
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                End If
            End If
            If Not tbCodigo.Enabled Then
                oTipoBursatilidadBM.Modificar(oTipoBursatilidadBE, DatosRequest)
                AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaTipoBursatilidad.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oTipoBursatilidadBM As New TipoBursatilidadBM
        Dim oTipoBursatilidadBE As New TipoBursatilidadBE

        oTipoBursatilidadBE = oTipoBursatilidadBM.Seleccionar(Codigo, DatosRequest)

        ddlSituacion.SelectedValue = CType(oTipoBursatilidadBE.TipoBursatilidad.Rows(0), TipoBursatilidadBE.TipoBursatilidadRow).Situacion.ToString()
        tbDescripcion.Text = CType(oTipoBursatilidadBE.TipoBursatilidad.Rows(0), TipoBursatilidadBE.TipoBursatilidadRow).Descripcion.ToString().Trim.ToUpper
        tbCodigo.Text = CType(oTipoBursatilidadBE.TipoBursatilidad.Rows(0), TipoBursatilidadBE.TipoBursatilidadRow).CodigoBursatilidad.ToString()
        hd.Value = CType(oTipoBursatilidadBE.TipoBursatilidad.Rows(0), TipoBursatilidadBE.TipoBursatilidadRow).CodigoBursatilidad.ToString()
    End Sub

    Public Function CargarSecuencial() As Boolean
        Try
            tbCodigo.Text = New ParametrosGeneralesBM().ObtenerProximoSecuencial(ParametrosSIT.TABLAS_TBL_T04, DatosRequest).ToString()
        Catch ex As Exception
            tbCodigo.Text = ""
        End Try
    End Function

    Public Function crearObjeto() As TipoBursatilidadBE
        Dim oTipoBursatilidadBE As New TipoBursatilidadBE
        Dim oRow As TipoBursatilidadBE.TipoBursatilidadRow
        oRow = CType(oTipoBursatilidadBE.TipoBursatilidad.NewRow(), TipoBursatilidadBE.TipoBursatilidadRow)
        oRow.Descripcion = tbDescripcion.Text.ToString().Trim.ToUpper
        oRow.Situacion = ddlSituacion.SelectedValue
        If hd.Value <> "" Then
            oRow.CodigoBursatilidad = hd.Value
        Else
            oRow.CodigoBursatilidad = tbCodigo.Text.Trim.ToUpper
        End If
        oTipoBursatilidadBE.TipoBursatilidad.AddTipoBursatilidadRow(oRow)
        oTipoBursatilidadBE.TipoBursatilidad.AcceptChanges()
        Return oTipoBursatilidadBE
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
        ddlSituacion.SelectedValue = "A"
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoBursatilidadBM As New TipoBursatilidadBM
        Dim oTipoBursatilidadBE As New TipoBursatilidadBE
        oTipoBursatilidadBE = oTipoBursatilidadBM.Seleccionar(tbCodigo.Text, DatosRequest)
        If oTipoBursatilidadBE.TipoBursatilidad.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class