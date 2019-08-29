Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Partial Class Modulos_Gestion_Reportes_frmSeguimientoForwards
    Inherits BasePage

#Region "Variables"
    Dim oReportesGestionBM As New ReporteGestionBM
    'Dim oUtil As New UtilDM
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub DescargaGrillaLista()
        Dim dtTabla As New DataTable

        dgLista.DataSource = Nothing


        dtTabla.Columns.Add("FechaVencimiento")
        dtTabla.Columns.Add("FechaIDI")
        dtTabla.Columns.Add("CodigoPortafolioSBS")
        dtTabla.Columns.Add("CodigoMonedaOrigen")
        dtTabla.Columns.Add("CodigoSBS")
        dtTabla.Columns.Add("Movimiento")
        dtTabla.Columns.Add("Operacion")
        dtTabla.Columns.Add("PrecioTrans")
        dtTabla.Columns.Add("PlazoVencimiento")
        dtTabla.Columns.Add("Indicador")
        dtTabla.Columns.Add("Consecutivo")


        dgLista.DataSource = dtTabla
        dgLista.DataBind()


    End Sub

    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        drlista.DataSource = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoPortafolio" : drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
    End Sub

    Private Function CargaSeguimientoFordWards(ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String, ByVal FechaVencimiento As Decimal) As Boolean
        Dim result As Boolean
        Try
            Dim oSegForwardsTable As DataTable = oReportesGestionBM.SeguimientoForwards(sCodigoPortafolioSBS, sCodigoMoneda, FechaVencimiento, DatosRequest).Tables(0)
            If oSegForwardsTable.Rows.Count > 0 Then
                dgLista.DataSource = oSegForwardsTable
                dgLista.DataBind()

                result = True
            Else
                result = False
            End If
            lbContador.Text = "Registros encontrados: " + dgLista.Rows.Count.ToString()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
        Return result
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                UIUtility.CargarMonedaOI(dllMoneda)
                Call CargaPortafolio(ddlPortafolio)
                ddlPortafolio.SelectedIndex = 1
                tbFechaInformacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
                Call DescargaGrillaLista()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Dim sCodigoPortafolioSBS As String = ddlPortafolio.SelectedValue.ToString
        Dim sCodigoMoneda As String = dllMoneda.SelectedValue.ToString
        Dim sFechaCorte As String = tbFechaInformacion.Text
        sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2)

        If Not CargaSeguimientoFordWards(sCodigoPortafolioSBS, sCodigoMoneda, sFechaCorte) Then
            Call DescargaGrillaLista()
            AlertaJS(ObtenerMensaje("ALERT171"))
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try

            LlenarSesionContextInfo
            Dim sCodigoPortafolioSBS As String = ddlPortafolio.SelectedValue.ToString
            Dim sCodigoMoneda As String = dllMoneda.SelectedValue.ToString
            Dim sFechaInformacion As String = tbFechaInformacion.Text
            sFechaInformacion = sFechaInformacion.Substring(6, 4) & sFechaInformacion.Substring(3, 2) & sFechaInformacion.Substring(0, 2)

            If dgLista.Rows.Count = 0 Then
                AlertaJS(ObtenerMensaje("ALERT172"))
            Else
                EjecutarJS(UIUtility.MostrarPopUp("frmVisorGestion.aspx?pportafolio=" & sCodigoPortafolioSBS & "&pFechaIni=" & sFechaInformacion & "&pReporte=" & "RDSDF", "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al enviar los datos a imprimir")
        End Try
    End Sub

    Sub LlenarSesionContextInfo()
        Dim tablaParametros As New Hashtable
        Session.Remove("context_info")
        If (ddlPortafolio.SelectedIndex >= 0) Then
            tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
            tablaParametros("Portafolio") = ddlPortafolio.SelectedItem.Text
        End If

        ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
        Session("context_info") = tablaParametros
    End Sub


    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Session.Remove("context_info")
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        If ddlPortafolio.SelectedIndex <> 0 Then
            tbFechaInformacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
        End If
        Call DescargaGrillaLista()
    End Sub

    Private Sub dllMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dllMoneda.SelectedIndexChanged
        Call DescargaGrillaLista()
    End Sub


    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim fecha As Decimal
            Dim codigoPortafolio As String
            If (e.CommandName = "_Edit") Then
                fecha = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInformacion.Text)
                codigoPortafolio = Me.ddlPortafolio.SelectedValue
                Response.Redirect("frmDetalleForward.aspx?cp=" + codigoPortafolio + "&pd=" + fecha.ToString() + "&id=" + e.CommandArgument)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub
End Class
