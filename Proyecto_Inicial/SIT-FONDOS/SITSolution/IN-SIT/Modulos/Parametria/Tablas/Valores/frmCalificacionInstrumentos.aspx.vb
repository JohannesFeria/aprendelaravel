Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Web
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmCalificacionInstrumentos
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not Session("CodigoCalificacion") Is Nothing Then
                    tbCodigo.Enabled = False
                    hd.Value = CType(Session("CodigoCalificacion"), String)
                    CargarRegistro(hd.Value)
                Else
                    tbCodigo.Enabled = True
                    hd.Value = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oCalificacionInstrumentoBM As New CalificacionInstrumentoBM
        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE

        Try
            oCalificacionInstrumentoBE = crearObjeto()
            If (hd.Value = "") Then
                If verificarExistencia() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oCalificacionInstrumentoBM.Insertar(oCalificacionInstrumentoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oCalificacionInstrumentoBM.Modificar(oCalificacionInstrumentoBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaCalificacionInstrumentos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oCalificacionInstrumentoBM As New CalificacionInstrumentoBM
        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        Dim oRow As CalificacionInstrumentoBE.CalificacionInstrumentoRow

        oCalificacionInstrumentoBE = oCalificacionInstrumentoBM.Seleccionar(Codigo, DatosRequest)

        oRow = DirectCast(oCalificacionInstrumentoBE.CalificacionInstrumento.Rows(0), CalificacionInstrumentoBE.CalificacionInstrumentoRow)

        ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        ddlPlazo.SelectedValue = oRow.Plazo.ToString()
        tbDescripcion.Text = oRow.Descripcion.ToString().ToUpper.Trim
        tbCodigo.Text = oRow.CodigoCalificacion.ToString()
        txtPrioridad.Text = oRow.Prioridad
        hd.Value = oRow.CodigoCalificacion.ToString()
    End Sub

    Public Function crearObjeto() As CalificacionInstrumentoBE
        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        Dim oRow As CalificacionInstrumentoBE.CalificacionInstrumentoRow
        oRow = CType(oCalificacionInstrumentoBE.CalificacionInstrumento.NewRow(), CalificacionInstrumentoBE.CalificacionInstrumentoRow)

        oRow.CodigoCalificacion = tbCodigo.Text.Trim
        oRow.Maduracion = ""
        oRow.Situacion = ddlSituacion.SelectedValue()
        oRow.Plazo = ddlPlazo.SelectedValue
        oRow.Descripcion = tbDescripcion.Text.ToString.ToUpper.Trim
        oRow.Prioridad = txtPrioridad.Text.Replace(".", UIUtility.DecimalSeparator())
        IIf(hd.Value <> "", oRow.CodigoCalificacion = hd.Value, oRow.CodigoCalificacion = tbCodigo.Text.Trim)

        oCalificacionInstrumentoBE.CalificacionInstrumento.AddCalificacionInstrumentoRow(oRow)
        oCalificacionInstrumentoBE.CalificacionInstrumento.AcceptChanges()
        Return oCalificacionInstrumentoBE
    End Function

    Public Sub CargarCombos()
        Dim tablaPlazo As New Data.DataTable
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaPlazo = oParametrosGenerales.ListarCalifInstr(DatosRequest)
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)

        HelpCombo.LlenarComboBox(ddlPlazo, tablaPlazo, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarCampos()
        tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        txtPrioridad.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlPlazo.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oCalificacionInstrumentoBM As New CalificacionInstrumentoBM
        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE

        oCalificacionInstrumentoBE = oCalificacionInstrumentoBM.Seleccionar(tbCodigo.Text.Trim, DatosRequest)

        Return oCalificacionInstrumentoBE.CalificacionInstrumento.Rows.Count > 0
    End Function

#End Region

End Class
