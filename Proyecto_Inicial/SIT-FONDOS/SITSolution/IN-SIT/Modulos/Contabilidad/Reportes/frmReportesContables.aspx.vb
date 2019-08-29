Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Contabilidad_frmReportesContables
    Inherits BasePage
#Region "/* Metodos Personalizados */"
    Public Sub CargarMercado()
        Dim DtablaMercado As New DataTable
        Dim oMercadoBM As New MercadoBM
        Try
            DtablaMercado = oMercadoBM.ListarActivos(DatosRequest).Tables(0)
            DtablaMercado.DefaultView.Sort = "CodigoMercado"
            HelpCombo.LlenarComboBoxBusquedas(Me.ddlMercado, DtablaMercado, "CodigoMercado", "Descripcion", True)
        Finally
            oMercadoBM = Nothing
            GC.Collect()
        End Try
    End Sub
    Private Sub VisualizarMercado(ByVal situacion As Boolean)
        divMercado.Attributes.Remove("class")
        If situacion Then
            divMercado.Attributes.Add("class", "col-sm-4")
        Else
            divMercado.Attributes.Add("class", "hidden")
        End If
    End Sub
    Private Sub VisualizarFechaFin(ByVal situacion As Boolean)
        divFechaFin.Attributes.Remove("class")
        If situacion Then
            divFechaFin.Attributes.Add("class", "col-sm-4")
        Else
            divFechaFin.Attributes.Add("class", "hidden")
        End If
    End Sub
#End Region
#Region "/* Eventos del WebForm */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlFondo.SelectedValue))
            VisualizarFechaFin(False)
            ddlFondo_SelectedIndexChanged(Me, EventArgs.Empty)
            CargarMercado()
            VisualizarMercado(False)
        End If
    End Sub
    Private Sub CargarFiltros()
        Dim oPortafolioBE As DataTable
        Dim oPortafolioBM As New PortafolioBM
        oPortafolioBE = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlFondo, oPortafolioBE, "CodigoPortafolio", "Descripcion", False)
    End Sub
    Private Sub ibVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Dim StrMensajeRangoValido As String
        StrMensajeRangoValido = validacionesRango()
        If (StrMensajeRangoValido <> "") Then
            AlertaJS("La fecha Final tiene que ser mayor a la fecha Inicial")
            Exit Sub
        End If
        Dim opcion As String
        Dim dtConsulta As New DataTable
        If RbReportes.SelectedValue = "RPA" Then
            If ddlFondo.SelectedValue = "ADMINISTRA" Then
                dtConsulta = New ReporteContabilidadBM().Resumen_Envio_PU_ADM(Me.ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim), DatosRequest).Tables(0)
            Else
                dtConsulta = New ReporteContabilidadBM().Resumen_Envio_PU_FONDO(Me.ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim), DatosRequest).Tables(0)
            End If
            Session("dtConsultaRPA") = dtConsulta
            If dtConsulta.Rows.Count = 0 Then
                AlertaJS("No se encontraron datos para mostrar el reporte contable...!")
                Exit Sub
            End If
        End If
        'fin
        opcion = Me.RbReportes.SelectedValue
        Session("titulo") = opcion
        Session("ReporteContabilidad_Fondo") = Nothing
        Session("FechaOperacionInicio") = Nothing
        Session("FechaOperacionFin") = Nothing
        Session("FechaOperacionInicio") = Me.tbFechaInicio.Text.Trim()
        If RbReportes.SelectedValue = "RPA" Then
            Session("FechaOperacionFin") = Me.tbFechaFin.Text.Trim()
        Else
            Session("FechaOperacionFin") = Me.tbFechaInicio.Text.Trim()
        End If
        Session("ReporteContabilidad_Fondo") = Me.ddlFondo.SelectedValue
        Session("ReporteContabilidad_DescripcionFondo") = Me.ddlFondo.SelectedItem.Text.Trim
        Session("CodigoMercado") = ""
        If ddlFondo.SelectedValue = ParametrosSIT.PORTAFOLIO_ADMINISTRA And RbReportes.SelectedValue = "CCI" Then
            If ddlMercado.SelectedIndex > 0 Then
                Session("CodigoMercado") = Me.ddlMercado.SelectedValue
            End If
        End If
        If opcion = "" Then
            AlertaJS("Debe seleccionar un Reporte")
        Else
            EjecutarJS(UIUtility.MostrarPopUp("frmVisorAsientosContables.aspx", "no", 800, 600, 40, 150, "no", "yes", "yes", "yes"), False)
        End If
    End Sub
    Protected Function validacionesRango() As String
        Dim msg As String = ""
        Dim strMensajeError As String = ""
        If Convert.ToDateTime(Me.tbFechaInicio.Text) > Convert.ToDateTime(Me.tbFechaFin.Text) Then
            msg += "\t-La fecha Final tiene que ser mayor o igual a la fecha Inicial \n"
        End If
        If (msg <> "") Then
            strMensajeError = "Error de Rango de Fechas : \n " + msg + "\n"
            Return strMensajeError
        Else
            Return ""
        End If
    End Function
    Private Sub btnTerminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerminar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(Me.ddlFondo.SelectedValue))
        tbFechaFin.Text = Me.tbFechaInicio.Text
        VisualizarFechaFin(False)
        If ddlFondo.SelectedValue = "ADMINISTRA" Then
            RbReportes.Items(0).Text = "PROVISIÓN DE POLIZAS AGENTES DE BOLSA"
            RbReportes.Items(0).Value = "PP"
            RbReportes.Items.RemoveAt(1)
            If RbReportes.Items.Count = 3 Then
                RbReportes.Items(2).Text = "PROVISIÓN MENSUAL DE INTERESES"
                RbReportes.Items(2).Value = "PI"
            ElseIf RbReportes.Items.Count = 2 Then
                RbReportes.Items.Insert(2, New ListItem("PROVISIÓN MENSUAL DE INTERESES", "PI"))
            End If
            VisualizarMercado(True)
            If RbReportes.Items.Count = 3 Then
                RbReportes.Items.Insert(3, New ListItem("RESUMEN ENVIO PU ADM", "RPA"))
            End If
            'JVC 20090326 Reporte resumen de envio PU ADM
            If RbReportes.SelectedValue = "RPA" Then
                VisualizarFechaFin(True)
            End If
        Else
            RbReportes.Items(0).Text = "COMPRA VENTA DE INVERSIONES"
            RbReportes.Items(0).Value = "CVI"
            If RbReportes.Items(1).Value <> "VC" Then
                RbReportes.Items.Insert(1, New ListItem("VALORIZACION DE LA CARTERA", "VC"))
            End If
            'JVC 20090310 Generar asientos por cada mercado
            VisualizarMercado(False)
            'JVC 20090331 PROVISION MENSUAL DE INTERESES
            If RbReportes.Items.Count > 3 Then
                If RbReportes.Items(3).Value = "PI" Then
                    RbReportes.Items.RemoveAt(3)
                End If
            End If
            'JVC 20090326 Reporte resumen de envio PU FONDO
            If ddlFondo.SelectedValue = "HO-FONDO1" Or ddlFondo.SelectedValue = "HO-FONDO2" Or ddlFondo.SelectedValue = "HO-FONDO3" Then
                If RbReportes.Items.Count > 3 Then
                    RbReportes.Items(3).Text = "RESUMEN ENVIO PU FONDO"
                    RbReportes.Items(3).Value = "RPA"
                ElseIf RbReportes.Items.Count = 3 Then
                    RbReportes.Items.Insert(3, New ListItem("RESUMEN ENVIO PU FONDO", "RPA"))
                End If
                If RbReportes.SelectedValue = "RPA" Then
                    VisualizarFechaFin(True)
                End If
            Else
                'JVC 20090324 Reporte resumen de envio PU ADM
                If RbReportes.Items.Count = 4 Then
                    RbReportes.Items.RemoveAt(3)
                End If
            End If
        End If
    End Sub
    Private Sub RbReportes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RbReportes.SelectedIndexChanged
        VisualizarFechaFin(False)
        VisualizarMercado(False)
        If RbReportes.SelectedValue = "RPA" Then
            tbFechaFin.Text = tbFechaInicio.Text
            '#ERROR#
            If ddlFondo.SelectedValue = "ADMINISTRA" Or ddlFondo.SelectedValue = "HO-FONDO1" Or ddlFondo.SelectedValue = "HO-FONDO2" Or ddlFondo.SelectedValue = "HO-FONDO3" Then
                VisualizarFechaFin(True)
            End If
        Else
            If ddlFondo.SelectedValue = "ADMINISTRA" And RbReportes.SelectedValue = "CCI" Then
                VisualizarMercado(True)
            End If
        End If
    End Sub
#End Region
End Class