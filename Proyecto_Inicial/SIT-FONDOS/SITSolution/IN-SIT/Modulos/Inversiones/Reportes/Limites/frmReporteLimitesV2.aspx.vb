Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports ParametrosSIT
Imports System.Data
Partial Class Modulos_Inversiones_Reportes_Limites_ReporteLimitesV2
    Inherits BasePage
    Dim oLimiteBM As New LimiteBM    
#Region " /* Eventos de la Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then                
                CargarCombos()                
                tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio)
            End If
            If Not (ViewState("ProcesarEnPageLoad") Is Nothing) AndAlso ViewState("ProcesarEnPageLoad") = True Then
                Me.procesar()
                ViewState("ProcesarEnPageLoad") = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Me.hdProcesar.Value = "0"
            If Me.ddlReporteLimite.SelectedIndex = 0 Then
                Me.AlertaJS("Debe Seleccionar un Reporte Limite.")
                Exit Sub
            End If
            If Me.ddlLimiteCaracteristica.SelectedIndex <= 0 Then
                Me.AlertaJS("Debe Seleccionar un Limite Caracteristica.")
                Exit Sub
            End If
            'Los limites ESTIMADOS siempre se vuelven a procesar ya que no se guardan en una BD
            If rblEscenario.SelectedValue = "REAL" Then
                'verificar que no se haya hecho la consulta previamente
                Dim existenCalculos As Boolean = New ReporteLimitesBM().TieneCalculoExistente(Me.ddlReporteLimite.SelectedValue, Me.ddlLimiteCaracteristica.SelectedValue.Split("-")(0).ToString(), UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text), "REAL", DatosRequest)
                If Not existenCalculos Then
                    Me.hdProcesar.Value = "1"
                    Me.procesar()
                Else
                    ViewState("ProcesarEnPageLoad") = True
                    EjecutarJS("PreguntarSiProcesamosNuevamente('Ya existe data. ¿Desea volver a procesar la consulta?')")
                End If
            Else
                Me.hdProcesar.Value = "1"
                Me.procesar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
    Private Sub ddlReporteLimite_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlReporteLimite.SelectedIndexChanged
        CargaLimitePortafolio()
    End Sub
#End Region
#Region " /* Métodos Personalizados */ "
    Sub CargaLimitePortafolio()
        Try
            Me.lblLimiteCaracteristica.Visible = True
            Me.ddlLimiteCaracteristica.Visible = True
            Dim dtSituacion As DataTable = New LimiteBM().SeleccionarCaracteristicasCompuestas(ddlReporteLimite.SelectedValue.ToString, DatosRequest)
            HelpCombo.LlenarComboBox(Me.ddlLimiteCaracteristica, dtSituacion, "codigoLimiteCaracteristica", "DescripcionCompuesta", True)
            chkDetallePorFondo.Visible = False ' IIf(ddlReporteLimite.SelectedValue = "16", True, False)
            ddlLimiteCaracteristica.Enabled = True
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Public Sub CargarCombos()
        Dim dtSituacion As DataTable = New LimiteBM().SeleccionarPorFiltro("", "", "A", DatosRequest).Tables(0) 'RGF 20090424
        HelpCombo.LlenarComboBox(Me.ddlReporteLimite, dtSituacion, "codigoLimite", "Descripcion", True)
        If ddlReporteLimite.SelectedValue <> "" Then
            CargaLimitePortafolio()
        End If
        'Ocultamos el label y combo LimiteCaracteristica
        'Me.ddlLimiteCaracteristica.Visible = False : Me.lblLimiteCaracteristica.Visible = False
    End Sub
    Private Sub procesar()
        Dim DetalladoPorFondo As String = "N", codigoPortafolio As String = Me.ddlLimiteCaracteristica.SelectedValue.Split("-")(1).ToString()
        Dim codigoMultifondo As String = String.Empty, limPortafolio As String = String.Empty
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet
        dsPortafolio = oPortafolio.Seleccionar(codigoPortafolio, Nothing)
        Session("pCodLimite") = ddlReporteLimite.SelectedValue
        Session("pCodLimiteCaracteristica") = Me.ddlLimiteCaracteristica.SelectedValue.Split("-")(0).ToString()
        Session("pFechaOperacion") = Me.tbFechaInicio.Text.Trim()
        Session("pDetalladoPorFondo") = DetalladoPorFondo
        Session("pEscenario") = rblEscenario.SelectedValue
        Session("pProcesarLimite") = Me.hdProcesar.Value
        Session("pPortafolio") = codigoPortafolio
        EjecutarJS("showModalDialog('frmVisorReporteLimitesV2.aspx?', '1124', '600','');")
    End Sub
    Private Function ExisteTipoCambioSpot() As Boolean
        Dim oTipoCambio As New TipoCambioBM
        Return oTipoCambio.ExisteTipoCambio("Spot", "DOL", UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
    End Function
#End Region
End Class