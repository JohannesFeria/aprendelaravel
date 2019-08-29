Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmConsultaDividendos
    Inherits BasePage

#Region "Variables"
    Dim oDividendosRebatesLiberadas As New DividendosRebatesLiberadasBM
    Dim oUtil As New UtilDM
    Dim oUIUtil As New UIUtility
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Function Validar() As Boolean
        Dim sFechaInicio As String = tbFechaInicio.Text.Trim
        Dim sFechaFin As String = tbFechaFin.Text.Trim

        If sFechaInicio.Trim.Length < 10 Then
            AlertaJS(ObtenerMensaje("ALERT46"))
            Return False
            Exit Function
        ElseIf sFechaFin.Trim.Length < 10 Then
            AlertaJS(ObtenerMensaje("ALERT47"))
            Return False
            Exit Function
        ElseIf sFechaInicio.Trim.Length = 0 Then
            AlertaJS(ObtenerMensaje("ALERT20"))
            Return False
            Exit Function
        ElseIf sFechaFin.Trim.Length = 0 Then
            AlertaJS(ObtenerMensaje("ALERT19"))
            Return False
            Exit Function
        End If

        If sFechaInicio.Trim <> "" And sFechaFin.Trim <> "" Then
            If sFechaInicio.Substring(6, 4) & sFechaInicio.Substring(3, 2) & sFechaInicio.Substring(0, 2) > _
                sFechaFin.Substring(6, 4) & sFechaFin.Substring(3, 2) & sFechaFin.Substring(0, 2) Then
                AlertaJS(ObtenerMensaje("ALERT48"))
                Return False
                Exit Function
            End If
        End If

        If sFechaInicio.Trim <> "" Then
            If Not IsDate(sFechaInicio) Then
                AlertaJS(ObtenerMensaje("ALERT44"))
                Return False
                Exit Function
            End If
        End If

        If sFechaFin.Trim <> "" Then
            If Not IsDate(sFechaFin) Then
                AlertaJS(ObtenerMensaje("ALERT45"))
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                FechaPortafolio()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        tablaPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS) 'oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlFondo, tablaPortafolio, "CodigoPortafolio", "Descripcion", False)
    End Sub

    Private Sub btnGenerarReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarReporte.Click
        Try
            Dim sFechaInicio As String = tbFechaInicio.Text.Trim
            Dim sFechaFin As String = tbFechaFin.Text.Trim
            sFechaInicio = UIUtility.ConvertirFechaaDecimal(sFechaInicio)
            sFechaFin = UIUtility.ConvertirFechaaDecimal(sFechaFin)
            If Validar() Then
                Dim strurl As String = String.Format("frmVisorReporte.aspx?CodigoPortafolioSBS={0}&FechaInicio={1}&FechaFin={2}", ddlFondo.SelectedValue, sFechaInicio, sFechaFin)
                EjecutarJS("showWindow('" & strurl & "', '800', '600');")
                'EjecutarJS(UIUtility.MostrarPopUp(String.Format("frmVisorReporte.aspx?CodigoPortafolioSBS={0}&FechaInicio={1}&FechaFin={2}", ddlFondo.SelectedValue, sFechaInicio, sFechaFin), "no", 1010, 670, 0, 0, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Generar el Reporte")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
        End Try
    End Sub

    Private Sub FechaPortafolio()
        Dim fechaFin As Decimal = UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue)
        Dim temp As String = fechaFin.ToString.Substring(0, 6) & "01"
        Dim FechaInicio As Decimal = CDec(temp)
        tbFechaFin.Text = UIUtility.ConvertirFechaaString(fechaFin)
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(FechaInicio)
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        Try
            FechaPortafolio()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar el Fondo")
        End Try
    End Sub

End Class