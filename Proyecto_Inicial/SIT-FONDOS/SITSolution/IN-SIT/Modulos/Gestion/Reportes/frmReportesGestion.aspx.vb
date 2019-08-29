Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data

Partial Class Modulos_Gestion_Reportes_frmReportesGestion
    Inherits BasePage

#Region "Variables"
    Dim oUtil As New UtilDM
#End Region
#Region "/* Metodos Personalizados */"
    Private Sub VisualizarInstrumento(ByVal situacion As Boolean)
        If situacion Then
            divInstrumento.Attributes.Add("class", "col-md-4")
        Else
            divInstrumento.Attributes.Add("class", "col-md-4 hidden")
        End If
    End Sub
    Private Sub VisualizarPortafolio(ByVal situacion As Boolean)
        If situacion Then
            divPortafolio.Attributes.Add("class", "col-md-4")
        Else
            divPortafolio.Attributes.Add("class", "col-md-4 hidden")
        End If
    End Sub
    Private Sub VisualizarFechaInicio(ByVal situacion As Boolean)
        If situacion Then
            divFechaInicio.Attributes.Add("class", "col-md-4")
        Else
            divFechaInicio.Attributes.Add("class", "col-md-4 hidden")
        End If
    End Sub
    Private Sub VisualizarFechaFin(ByVal situacion As Boolean)
        If situacion Then
            divFechaFin.Attributes.Add("class", "col-md-4")
        Else
            divFechaFin.Attributes.Add("class", "col-md-4 hidden")
        End If
    End Sub

    Private Sub VisualizarMercado(ByVal situacion As Boolean)
        If situacion Then
            divMercado.Attributes.Add("class", "col-md-4")
        Else
            divMercado.Attributes.Add("class", "col-md-4 hidden")
        End If
    End Sub

    Private Sub VisualizarPrimeraFila(ByVal situacion As Boolean)
        If situacion Then
            divPrimeraFila.Attributes.Add("class", "row")
        Else
            divPrimeraFila.Attributes.Add("class", "row hidden")
        End If
    End Sub

    Private Sub InhabilitarFechaFin(ByVal bValor As Boolean)
        If bValor Then
            ImgFIN.Attributes.Add("class", "input-append date")
            tbFechaFin.ReadOnly = False
        Else
            ImgFIN.Attributes.Add("class", "input-append")
            tbFechaFin.ReadOnly = True
        End If
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim vReporte As String
        Dim database As DataTable
        Dim FechaValoracion As String
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        If Not Page.IsPostBack Then
            Try
                CargarCombos()
                database = oCarteraTituloValoracion.UltimaValoracion(UIUtility.ConvertirFechaaDecimal(oUtil.RetornarFechaSistema), Me.DatosRequest).Tables(0)
                FechaValoracion = UIUtility.ConvertirFechaaString(database.Rows(0).Item(0))
                tbFechaInicio.Text = oUtil.RetornarFechaSistema
                tbFechaFin.Text = oUtil.RetornarFechaSistema

                'divFechaFin.Visible = False
                'ddlFondo.Visible = False
                'lblPortafolio.Visible = False
                ''Agregado por Yanina Perez 20080622
                'ddlInstrumento.Visible = False
                'lblInstrumento.Visible = False

                divInstrumento.Attributes.Add("class", "col-md-4 hidden")
                divFechaFin.Attributes.Add("class", "col-md-4 hidden")
                divPortafolio.Attributes.Add("class", "col-md-4 hidden")

                ImgFIN.Attributes.Remove("class")
                ImgFIN.Attributes.Add("class", "input-append")


                vReporte = Request.QueryString("rpt")
                hdReporte.Value = vReporte

                Select Case vReporte
                    Case "BCH" 'REPORTE DE BENCHMARKING
                        lNombreReporte.Text = "BENCHMARKING"
                    Case "CCM" 'COMP. CARTERA POR MONEDA    
                        lNombreReporte.Text = "COMP. CARTERA POR MONEDA"
                        VisualizarFechaInicio(True)
                        VisualizarPortafolio(True)
                        VisualizarMercado(True)
                        VisualizarFechaFin(False)
                        lFechaInicio.InnerText = "Fecha"
                        Me.tbFechaInicio.Text = FechaValoracion
                    Case "DCD" 'DURACION DE CARTERA  DETALLE          
                        lNombreReporte.Text = "DURACION DE CARTERA DETALLE"
                        lFechaInicio.InnerText = "Fecha"
                        VisualizarMercado(False)
                        VisualizarPortafolio(True)
                        tbFechaInicio.Text = FechaValoracion
                    Case "DC" 'DURACION DE CARTERA RESUMEN            
                        lNombreReporte.Text = "DURACION DE CARTERA RESUMEN"
                        lFechaInicio.InnerText = "Fecha"
                        VisualizarPrimeraFila(False)
                        tbFechaInicio.Text = FechaValoracion
                    Case "CCE" 'COMP. CARTERA POR EMISOR
                        lFechaInicio.InnerText = "Fecha Valoración"
                        lNombreReporte.Text = "COMP. CARTERA POR EMISOR"
                        VisualizarPortafolio(True)
                        ddlFondo.Enabled = False
                        tbFechaInicio.Text = FechaValoracion

                    Case "CCS" 'COMP. CARTERA POR SECTOR     
                        lFechaInicio.InnerText = "Fecha Valoración"
                        lNombreReporte.Text = "COMP. CARTERA POR SECTOR"
                        tbFechaInicio.Text = FechaValoracion
                        VisualizarPortafolio(False)
                    Case "CCP1" 'COMP. CARTERA POR PLAZO RESUMEN  
                        lFechaInicio.InnerText = "Fecha Valoración"
                        lNombreReporte.Text = "COMP. CARTERA POR PLAZO RESUMEN"
                        VisualizarPortafolio(True)
                        VisualizarMercado(False)
                        tbFechaInicio.Text = FechaValoracion

                    Case "CCPD" 'COMP. CARTERA POR PLAZO DETALLE
                        lFechaInicio.InnerText = "Fecha Valoración"
                        lNombreReporte.Text = "COMP. CARTERA POR PLAZO DETALLE"
                        VisualizarPortafolio(True)
                        VisualizarMercado(False)
                        tbFechaInicio.Text = FechaValoracion

                    Case "CCR" 'COMP. CARTERA POR RIESGO DETALLE
                        lNombreReporte.Text = "COMP. POR CATEGORIA DE RIESGO"
                        lFechaInicio.InnerText = "Fecha: "
                        VisualizarPortafolio(True)
                        VisualizarFechaInicio(True)
                        VisualizarFechaFin(False)
                        VisualizarMercado(False)
                        tbFechaInicio.Text = FechaValoracion
                    Case "CCRR" 'COMP. CARTERA POR RIESGO RESUMEN
                        lNombreReporte.Text = "RESUMEN DE COMP. POR CATEGORIA DE RIESGO"
                        lFechaInicio.InnerText = "Fecha: "
                        tbFechaInicio.Text = FechaValoracion
                        VisualizarPortafolio(False)
                        VisualizarFechaInicio(True)
                        VisualizarFechaFin(False)
                        VisualizarMercado(True)
                    Case "CCTR" 'COMP. CARTERA POR TIPO DE RENTA
                        lFechaInicio.InnerText = "Fecha Valoración"
                        lNombreReporte.Text = "COMP. CARTERA POR TIPO DE RENTA"
                        tbFechaInicio.Text = FechaValoracion
                        VisualizarMercado(False)
                        VisualizarPortafolio(False)
                        ddlFondo.Enabled = False
                    Case "CCEX" 'COMP. CARTERA EXTERIOR
                        Me.lNombreReporte.Text = "COMP. CARTERA EXTERIOR"
                        lFechaInicio.InnerText = "Fecha"
                        tbFechaInicio.Text = FechaValoracion
                        VisualizarPrimeraFila(False)
                        VisualizarFechaInicio(True)
                    Case "CCIE" 'COMP. CARTERA INSRUMENTO EMPRESA
                        lNombreReporte.Text = "Comp. Cartera por Instrumento-Empresa"
                        lFechaInicio.InnerText = "Fecha"
                        VisualizarPortafolio(True)
                        VisualizarFechaInicio(True)
                        VisualizarMercado(False)
                        tbFechaInicio.Text = FechaValoracion
                    Case "CCCI" 'COMP. CARTERA POR CATEGORIA DE INSTRUMENTO   
                        lNombreReporte.Text = "COMP. CARTERA POR CATEGORIA DE INSTRUMENTO"
                        lFechaInicio.InnerText = "Fecha Valoración"
                        tbFechaInicio.Text = FechaValoracion
                        VisualizarPortafolio(True)
                        ddlFondo.Enabled = False
                    Case "SBCR" 'STOCK CD DEL BCR               
                        lNombreReporte.Text = "STOCK CD DEL BCR"
                        lFechaInicio.InnerText = "Fecha"
                        VisualizarPortafolio(True)
                    Case "SF" 'STOCK FORDWARDS                  
                        lNombreReporte.Text = "STOCK FORDWARDS"
                        lFechaInicio.InnerText = "Fecha"
                        VisualizarPortafolio(True)
                    Case "FC" 'FLUJO DE CAJA                    
                        lNombreReporte.Text = "FLUJO DE CAJA"
                        ddlFondo.Visible = True
                        lblPortafolio.Visible = True
                        lFechaInicio.InnerText = "Fecha"
                    Case "RU" 'REPORTE DE UTILIDAD             
                        Me.lNombreReporte.Text = "REPORTE DE UTILIDAD"
                        Me.ddlFondo.Visible = True
                        Me.lblPortafolio.Visible = True
                        Me.lFechaInicio.InnerText = "Fecha Inicio"
                        Me.lFechaFin.Visible = True
                        Me.lFechaFin.InnerText = "Fecha Fin"
                        Me.tbFechaFin.Visible = True
                    Case "SAFP" 'LISTA DE PRECIOS SAFP          
                        Me.lNombreReporte.Text = "LISTA DE PRECIOS SAFP"
                        VisualizarPortafolio(False)
                        VisualizarMercado(False)
                        VisualizarFechaFin(True)
                        InhabilitarFechaFin(False)
                        Me.ddlFondo.Enabled = False
                        Me.lFechaInicio.InnerText = "Fecha"
                        'Agregado por Yanina Perez 20080622
                        VisualizarInstrumento(True)
                    Case "SIPE" 'SALDOS DE INSTRUMENTOS POR EMPRESA  
                        'RGF 20080715 se cambio el titulo del encabezado
                        'Me.lNombreReporte.Text = "SALDOS DE INSTRUMENTOS POR EMPRESA"
                        Me.lNombreReporte.Text = "AUXILIAR BCR"
                        VisualizarPortafolio(True)
                        Me.lFechaInicio.InnerText = "Fecha"
                    Case "RDSDF" 'REPORTE DE STOCKS DE FORWARDS
                        Me.lNombreReporte.Text = "STOCKS DE FORWARDS"
                        VisualizarPortafolio(True)
                        Me.lFechaInicio.InnerText = "Fecha"
                    Case "RDSDCDD" 'REPORTE DE STOCKS DE CERTIFICADOS DE DEPOSITO
                        Me.lNombreReporte.Text = "STOCKS DE CERTIFICADOS DE DEPOSITO"
                        VisualizarPortafolio(True)
                        Me.lFechaInicio.InnerText = "Fecha"
                    Case "REPCOMPB" 'REPORTE DE COMPOSICION DE BENCHMARK
                        Me.lNombreReporte.Text = "COMPOSICION DE BENCHMARK"
                        Me.lFechaInicio.InnerText = "Fecha"
                    Case "RDU" 'REPORTE DE UTILIDAD
                        lNombreReporte.Text = "UTILIDAD"
                        lFechaInicio.InnerText = "Fecha Inicio"
                        VisualizarMercado(False)
                        VisualizarPortafolio(True)
                        VisualizarFechaInicio(True)
                        VisualizarFechaFin(True)
                    Case "RFC" 'REPORTE DE FLUJO DE CAJA
                        Me.VisualizarPortafolio(True)
                        Me.VisualizarFechaInicio(True)
                        Me.lNombreReporte.Text = "FLUJO DE CAJA"
                        Me.lFechaInicio.InnerText = "Fecha"
                        Me.tbFechaInicio.Text = Date.Today.ToShortDateString
                        Me.tbFechaInicio.Enabled = True
                        ImgINI.Attributes.Remove("class")
                        ImgINI.Attributes.Add("class", "input-append date")

                        'Agregado por Yanina Pérez 20080612
                    Case "SAFPM" 'LISTA DE PRECIOS SAFP          
                        Me.lNombreReporte.Text = "LISTA DE TIPOS DE CAMBIO SAFP"
                        VisualizarPortafolio(False)
                        VisualizarFechaFin(False)
                        InhabilitarFechaFin(False)
                        Me.ddlFondo.Enabled = False
                        Me.lFechaInicio.InnerText = "Fecha"

                    Case "GESHORA"
                        Me.lNombreReporte.Text = "OPERACIONES POR GESTOR"
                        VisualizarFechaInicio(True)
                        VisualizarPortafolio(True)
                        VisualizarFechaFin(True)
                        InhabilitarFechaFin(True)
                        Me.lFechaInicio.InnerText = "Fecha Inicio"

                        tbFechaInicio.Text = CType(oUtil.RetornarFechaSistema, Date)

                        Dim fechaSisTexto = oUtil.RetornarFechaSistema.Replace("/", "")
                        Dim fechaSis As Date = New Date(Right(fechaSisTexto, 4), fechaSisTexto.Substring(2, 2), Left(fechaSisTexto, 2))

                        tbFechaInicio.Text = fechaSis.AddMonths(-1).ToString("dd/MM/yyyy")
                End Select
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

    Sub LlenarSesionContextInfo()
        Dim tablaParametros As New Hashtable

        If (ddlFondo.SelectedIndex >= 0) Then
            tablaParametros("codPortafolio") = ddlFondo.SelectedValue
            tablaParametros("Portafolio") = ddlFondo.SelectedItem.Text
        End If

        ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
        Session("context_info") = tablaParametros
    End Sub

    Private Sub ibVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click

        Me.LlenarSesionContextInfo()

        Dim decfechainicio, decfechafin As Decimal
        decfechainicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
        decfechafin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)

        Dim vReporte As String = Request.QueryString("rpt")

        If (vReporte = "CCR" Or vReporte = "CCR" Or vReporte = "DCD" Or vReporte = "CCPD" Or vReporte = "CCP" Or vReporte = "RDU" Or vReporte = "SIPE" Or vReporte = "GESHORA" Or vReporte = "CCM" Or vReporte = "CCIE" Or vReporte = "RDSDCDD" Or vReporte = "RDSDF") Then
            If Me.ddlFondo.SelectedIndex = 0 Then
                AlertaJS("Debe seleccionar un Portafolio.")
                Exit Sub
            End If
        End If

        Dim script As String = ""

        Dim paramPortafolio As String = "?pPortafolio=" & IIf(Not Me.ddlFondo.Visible, "", Me.ddlFondo.SelectedValue())
        Dim paramMercado As String = "&pMercado=" & IIf(Not Me.ddlMercado.Visible, "", Me.ddlMercado.SelectedValue())
        Dim paramInstrumento As String = "&pInstrumento=" & IIf(Not Me.ddlInstrumento.Visible, "", Me.ddlInstrumento.SelectedValue())

        If vReporte = "GESHORA" Then
            Session("titulo") = "Por Gestor2"
            script = "../../Inversiones/Reportes/Orden de Inversion/frmVisorReporteOperacionesEjecutadas.aspx" + paramPortafolio + "&Finicio=" + Me.tbFechaInicio.Text.Trim() + "&FFin=" + Me.tbFechaFin.Text.Trim()
            EjecutarJS(UIUtility.MostrarPopUp(script, "no", 800, 600, 5, 5, "no", "yes", "yes", "yes"), False)
            Exit Sub
        Else
            script = "frmVisorGestion.aspx" + paramPortafolio + paramMercado + paramInstrumento
        End If

        EjecutarJS(UIUtility.MostrarPopUp(script + "&pFechaIni=" + Convert.ToString(decfechainicio) + "&pFechaFin=" + Convert.ToString(decfechafin) + "&pReporte=" + Me.hdReporte.Value, "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Session.Remove("context_info")
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Public Sub CargarCombos()
        Dim oMercadoBM As New MercadoBM
        Dim DtablaMercado As DataTable
        Dim dtInstrumentos As DataTable

        Dim dtPortafolios As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlFondo, dtPortafolios, "CodigoPortafolio", "Descripcion", True)

        DtablaMercado = oMercadoBM.ListarActivos(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlMercado, DtablaMercado, "CodigoMercado", "Descripcion", True)

        dtInstrumentos = ObtenerTipoInstrumentos()
        ddlInstrumento.Items.Clear()
        ddlInstrumento.DataSource = dtInstrumentos
        ddlInstrumento.DataValueField = "CODIGO"
        ddlInstrumento.DataTextField = "DESC"
        ddlInstrumento.DataBind()
    End Sub
    Private Function ObtenerTipoInstrumentos() As DataTable
        Dim dtTipos As New DataTable
        Dim dcCodigo As New DataColumn
        Dim dcTipo As New DataColumn

        dcCodigo.ColumnName = "CODIGO"
        dcCodigo.DataType = System.Type.GetType("System.String")
        dcCodigo.Caption = "CODIGO"

        dcTipo.ColumnName = "DESC"
        dcTipo.DataType = System.Type.GetType("System.String")
        dcTipo.Caption = "DESC"

        dtTipos.Columns.Add(dcCodigo)
        dtTipos.Columns.Add(dcTipo)

        Dim drTipo As DataRow
        drTipo = dtTipos.NewRow
        drTipo("CODIGO") = "0"
        drTipo("DESC") = "--Todo--"
        dtTipos.Rows.Add(drTipo)

        drTipo = dtTipos.NewRow
        drTipo("CODIGO") = "AFP"
        drTipo("DESC") = "Negociadas"
        dtTipos.Rows.Add(drTipo)

        drTipo = dtTipos.NewRow
        drTipo("CODIGO") = "SBS"
        drTipo("DESC") = "No Negociadas"
        dtTipos.Rows.Add(drTipo)
        Return dtTipos
    End Function

End Class
