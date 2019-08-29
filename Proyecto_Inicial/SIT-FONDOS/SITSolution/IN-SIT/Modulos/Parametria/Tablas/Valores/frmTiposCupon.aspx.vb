Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmTiposCupon
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
        Try
            Dim oTipoCuponBM As New TipoCuponBM
            Dim oTipoCuponBE As New TipoCuponBE
            If (hd.Value = "") Then
                If verificarExistencia() = False Then
                    oTipoCuponBE = crearObjeto()
                    oTipoCuponBM.Insertar(oTipoCuponBE, DatosRequest)
                    AlertaJS("Los datos fueron grabados correctamente")
                    LimpiarCampos()
                    CargarSecuencial()
                Else
                    AlertaJS("Este registro ya existe")
                End If
            Else
                oTipoCuponBE = crearObjeto()
                oTipoCuponBM.Modificar(oTipoCuponBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaTiposCupon.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oTipoCuponBM As New TipoCuponBM
        Dim oTipoCuponBE As New TipoCuponBE
        oTipoCuponBE = oTipoCuponBM.Seleccionar(Codigo, DatosRequest)
        ddlSituacion.SelectedValue = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow).Situacion.ToString()
        tbDescripcion.Text = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow).Descripcion.ToString().Trim.ToUpper
        tbObservaciones.Text = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow).Observaciones.ToString().Trim.ToUpper
        tbCodigo.Text = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow).CodigoTipoCupon.ToString()
        hd.Value = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow).CodigoTipoCupon.ToString()
    End Sub

    Public Function CargarSecuencial() As Boolean
        Try
            tbCodigo.Text = New ParametrosGeneralesBM().ObtenerProximoSecuencial(ParametrosSIT.TABLAS_TBL_T02, DatosRequest).ToString()
        Catch ex As Exception
            tbCodigo.Text = ""
        End Try
    End Function

    Public Function crearObjeto() As TipoCuponBE
        Dim oTipoCuponBE As New TipoCuponBE
        Dim oRow As TipoCuponBE.TipoCuponRow
        oRow = CType(oTipoCuponBE.TipoCupon.NewRow(), TipoCuponBE.TipoCuponRow)
        oRow.Observaciones = tbObservaciones.Text.ToString()
        oRow.Descripcion = tbDescripcion.Text.ToString()
        oRow.Situacion = ddlSituacion.SelectedValue
        If hd.Value <> "" Then
            oRow.CodigoTipoCupon = hd.Value
        Else
            oRow.CodigoTipoCupon = tbCodigo.Text.Trim.ToUpper
        End If
        oTipoCuponBE.TipoCupon.AddTipoCuponRow(oRow)
        oTipoCuponBE.TipoCupon.AcceptChanges()
        Return oTipoCuponBE
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
        tbObservaciones.Text = ""
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoCuponBM As New TipoCuponBM
        Dim oTipoCuponBE As New TipoCuponBE
        oTipoCuponBE = oTipoCuponBM.Seleccionar(tbCodigo.Text, DatosRequest)
        If oTipoCuponBE.TipoCupon.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class