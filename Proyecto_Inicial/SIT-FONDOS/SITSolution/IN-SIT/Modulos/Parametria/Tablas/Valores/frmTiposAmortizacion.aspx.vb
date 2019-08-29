Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmTiposAmortizacion
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
                    tbCodigo.Enabled = False
                    Me.hd.Value = ""
                    CargarSecuencial()
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oTipoAmortizacionBM As New TipoAmortizacionBM
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        If (Me.hd.Value = "") Then
            Try
                If verificarExistencia() = False Then
                    oTipoAmortizacionBE = crearObjeto()
                    oTipoAmortizacionBM.Insertar(oTipoAmortizacionBE, DatosRequest)
                    AlertaJS("Los datos fueron grabados correctamente")
                    LimpiarCampos()
                Else
                    AlertaJS("Este registro ya existe")
                End If
            Catch ex As Exception
                AlertaJS("Ocurrió un error al Grabar los datos")
            End Try
        Else
            Try
                oTipoAmortizacionBE = crearObjeto()
                oTipoAmortizacionBM.Modificar(oTipoAmortizacionBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            Catch ex As Exception
                AlertaJS("Ocurrió un error al Modificar los datos")
            End Try
        End If
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaTiposAmortizacion.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Seleccionar */ "

#End Region

#Region " /* Funciones Insertar */ "

#End Region

#Region " /* Funciones Modificar */"

#End Region

#Region " /* Funciones Eliminar */"

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oTipoAmortizacionBM As New TipoAmortizacionBM
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        Dim cod As String
        Dim oRow As TipoAmortizacionBE.TipoAmortizacionRow
        oTipoAmortizacionBE = oTipoAmortizacionBM.Seleccionar(Codigo, DatosRequest)
        oRow = DirectCast(oTipoAmortizacionBE.TipoAmortizacion.Rows(0), TipoAmortizacionBE.TipoAmortizacionRow)
        Me.tbNumeroDias.Text = oRow.NumeroDias
        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        Me.tbDescripcion.Text = oRow.Descripcion.ToString().Trim.ToUpper
        cod = oRow.CodigoTipoAmortizacion.ToString()
        Me.tbCodigo.Text = cod
        Me.hd.Value = oRow.CodigoTipoAmortizacion.ToString()
    End Sub

    Public Function CargarSecuencial() As Boolean
        Try
            tbCodigo.Text = New ParametrosGeneralesBM().ObtenerProximoSecuencial(ParametrosSIT.TABLAS_TBL_T03, DatosRequest).ToString()
        Catch ex As Exception
            tbCodigo.Text = ""
        End Try
    End Function

    Public Function crearObjeto() As TipoAmortizacionBE
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        Dim oRow As TipoAmortizacionBE.TipoAmortizacionRow
        oRow = DirectCast(oTipoAmortizacionBE.TipoAmortizacion.NewRow(), TipoAmortizacionBE.TipoAmortizacionRow)
        oRow.CodigoTipoAmortizacion = Me.tbCodigo.Text
        oRow.Descripcion = Me.tbDescripcion.Text.ToString().Trim.ToUpper
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.NumeroDias = Me.tbNumeroDias.Text
        If Not Me.hd.Value.Equals(String.Empty) Then
            oRow.CodigoTipoAmortizacion = hd.Value
        Else
            oRow.CodigoTipoAmortizacion = Me.tbCodigo.Text.Trim.ToUpper
        End If
        oTipoAmortizacionBE.TipoAmortizacion.AddTipoAmortizacionRow(oRow)
        oTipoAmortizacionBE.TipoAmortizacion.AcceptChanges()
        Return oTipoAmortizacionBE
    End Function

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Public Sub LimpiarCampos()
        Me.tbCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        Me.ddlSituacion.SelectedValue = "A"
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoAmortizacionBM As New TipoAmortizacionBM
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        oTipoAmortizacionBE = oTipoAmortizacionBM.Seleccionar(Me.tbCodigo.Text, DatosRequest)
        If oTipoAmortizacionBE.TipoAmortizacion.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class
