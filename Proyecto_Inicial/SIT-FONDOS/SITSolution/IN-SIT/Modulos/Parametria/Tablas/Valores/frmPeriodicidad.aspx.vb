Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmPeriodicidad
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
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaPeriocidad.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oPeriodicidadBM As New PeriodicidadBM
        Dim oPeriodicidadBE As New PeriodicidadBE
        Dim dias As Integer
        oPeriodicidadBE = oPeriodicidadBM.Seleccionar(Codigo, DatosRequest)
        If CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow).IsDiasPeriodoNull = False Then
            dias = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow).DiasPeriodo
        Else
            dias = 0
        End If
        txtDias.Text = dias.ToString
        ddlSituacion.SelectedValue = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow).Situacion.ToString()
        tbDescripcion.Text = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow).Descripcion.ToString().Trim.ToUpper
        tbCodigo.Text = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow).CodigoPeriodicidad.ToString()
        hd.Value = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow).CodigoPeriodicidad.ToString()
    End Sub

    Public Function CargarSecuencial() As Boolean
        Try
            tbCodigo.Text = New ParametrosGeneralesBM().ObtenerProximoSecuencial(ParametrosSIT.TABLAS_TBL_T06, DatosRequest).ToString()
        Catch ex As Exception
            tbCodigo.Text = ""
        End Try
        Return Nothing
    End Function

    Public Function crearObjeto() As PeriodicidadBE
        Dim oPeriodicidadBE As New PeriodicidadBE
        Dim oRow As PeriodicidadBE.PeriodicidadRow
        oRow = CType(oPeriodicidadBE.Periodicidad.NewRow(), PeriodicidadBE.PeriodicidadRow)

        oRow.DiasPeriodo = CInt(txtDias.Text.ToString)
        oRow.Descripcion = tbDescripcion.Text.ToString.Trim.ToUpper
        oRow.Situacion = ddlSituacion.SelectedValue

        If hd.Value <> "" Then
            oRow.CodigoPeriodicidad = hd.Value
        Else
            oRow.CodigoPeriodicidad = tbCodigo.Text.Trim.ToUpper
        End If

        oPeriodicidadBE.Periodicidad.AddPeriodicidadRow(oRow)
        oPeriodicidadBE.Periodicidad.AcceptChanges()

        Return oPeriodicidadBE
    End Function

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim tablaPeriodos As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaPeriodos = oParametrosGenerales.ListarPeriodos(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Public Sub LimpiarCampos()
        tbCodigo.Text = ""
        tbDescripcion.Text = ""
        txtDias.Text = ""
        ddlSituacion.SelectedValue = "A"
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oPeriodicidadBM As New PeriodicidadBM
        Dim oPeriodicidadBE As New PeriodicidadBE
        oPeriodicidadBE = oPeriodicidadBM.Seleccionar(tbCodigo.Text, DatosRequest)
        If oPeriodicidadBE.Periodicidad.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
#End Region

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oPeriodicidadBM As New PeriodicidadBM
        Dim oPeriodicidadBE As New PeriodicidadBE
        If (hd.Value = "") Then
            Try
                If verificarExistencia() = False Then
                    oPeriodicidadBE = crearObjeto()
                    oPeriodicidadBM.Insertar(oPeriodicidadBE, DatosRequest)
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
                oPeriodicidadBE = crearObjeto()
                oPeriodicidadBM.Modificar(oPeriodicidadBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            Catch ex As Exception
                AlertaJS("Ocurrió un error al Modificar los datos")
            End Try
        End If
    End Sub
End Class
