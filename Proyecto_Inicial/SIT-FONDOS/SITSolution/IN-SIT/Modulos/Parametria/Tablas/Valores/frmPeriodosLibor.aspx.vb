Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmPeriodosLibor
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    Me.hd.Value = Request.QueryString("cod")
                    CargarRegistro(hd.Value)
                    tbCodigo.Enabled = False
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
        Try
            Dim oPeriodoLiborBM As New PeriodoLiborBM
            Dim oPeriodoLiborBE As New PeriodoLiborBE
            If (Me.hd.Value = "") Then
                If verificarExistencia() = False Then
                    oPeriodoLiborBE = crearObjeto()
                    oPeriodoLiborBM.Insertar(oPeriodoLiborBE, DatosRequest)
                    AlertaJS("Los datos fueron grabados correctamente")
                    LimpiarCampos()
                    CargarSecuencial()
                Else
                    AlertaJS("Este registro ya existe")
                End If
            Else
                oPeriodoLiborBE = crearObjeto()
                oPeriodoLiborBM.Modificar(oPeriodoLiborBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPeriodosLibor.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oPeriodoLiborBM As New PeriodoLiborBM
        Dim oPeriodoLiborBE As New PeriodoLiborBE
        oPeriodoLiborBE = oPeriodoLiborBM.Seleccionar(Codigo, DatosRequest)
        Me.ddlSituacion.SelectedValue = CType(oPeriodoLiborBE.PeriodoLibor.Rows(0), PeriodoLiborBE.PeriodoLiborRow).Situacion.ToString()
        Me.tbDescripcion.Text = CType(oPeriodoLiborBE.PeriodoLibor.Rows(0), PeriodoLiborBE.PeriodoLiborRow).Descripcion.ToString().Trim.ToUpper
        Me.tbCodigo.Text = CType(oPeriodoLiborBE.PeriodoLibor.Rows(0), PeriodoLiborBE.PeriodoLiborRow).CodigoPeriodoLibor
        Me.hd.Value = CType(oPeriodoLiborBE.PeriodoLibor.Rows(0), PeriodoLiborBE.PeriodoLiborRow).CodigoPeriodoLibor.ToString()
    End Sub

    Public Function CargarSecuencial() As Boolean
        tbCodigo.Text = New ParametrosGeneralesBM().ObtenerProximoSecuencial(ParametrosSIT.TABLAS_TBL_T05, DatosRequest).ToString()
    End Function

    Public Function crearObjeto() As PeriodoLiborBE
        Dim oPeriodoLiborBE As New PeriodoLiborBE
        Dim oRow As PeriodoLiborBE.PeriodoLiborRow
        oRow = CType(oPeriodoLiborBE.PeriodoLibor.NewRow(), PeriodoLiborBE.PeriodoLiborRow)

        oRow.CodigoPeriodoLibor = Me.tbCodigo.Text.ToString
        oRow.Descripcion = Me.tbDescripcion.Text.ToString.Trim.ToUpper
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        If Me.hd.Value <> "" Then
            oRow.CodigoPeriodoLibor = hd.Value
        Else
            oRow.CodigoPeriodoLibor = Me.tbCodigo.Text.Trim.ToUpper
        End If
        oPeriodoLiborBE.PeriodoLibor.AddPeriodoLiborRow(oRow)
        oPeriodoLiborBE.PeriodoLibor.AcceptChanges()
        Return oPeriodoLiborBE
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
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oPeriodoLiborBM As New PeriodoLiborBM
        Dim oPeriodoLiborBE As New PeriodoLiborBE
        oPeriodoLiborBE = oPeriodoLiborBM.Seleccionar(Me.tbCodigo.Text, DatosRequest)
        If oPeriodoLiborBE.PeriodoLibor.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class
