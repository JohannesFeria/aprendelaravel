Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Inversiones_Reportes_Orden_de_Inversion_frmReporteOI_Estados
    Inherits BasePage
#Region " /* Eventos de la Página */ "
    Dim oPortafolioBM As New PortafolioBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Dim oBM As New UtilDM
                Me.tbFecha.Text = oBM.RetornarFechaSistema()
                Me.tbFechaFin.Text = oBM.RetornarFechaSistema()
                Me.CargarPortafolio()
                LoadEstadoOrdenes()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim decFecha As Decimal = 0
            Dim decFechaFin As Decimal = 0
            Dim vEstado As String = ""
            Dim Estado As String = ""
            Dim regSBS As String = ""
            Dim liqAntFon As String = ""
            decFecha = UIUtility.ConvertirFechaaDecimal(Me.tbFecha.Text)
            decFechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
            If decFecha > 0 And decFechaFin > 0 Then
                Estado = IIf(ddlEstado.SelectedValue = "E-APR", ddlAprobados.SelectedValue.ToString, ddlEstado.SelectedValue.ToString)
                vEstado = IIf(ddlEstado.SelectedValue = "E-APR", ddlEstado.SelectedItem.ToString & " por exceso de " & ddlAprobados.SelectedItem.ToString, ddlEstado.SelectedItem.ToString)
                If (chkRegulaSBS.Checked) Then
                    regSBS = "S"
                    vEstado += " con Regularización SBS"
                End If
                If (chkLiqAntFon.Checked) Then
                    liqAntFon = LIQUIDA_FECHA_ANT_FONDO
                    vEstado += " Liquidadas con Fecha Anterior a la Apertura de Portafolio"
                End If

                Dim Reporte As String = "frmVisorReporteOrdenesInversion.aspx?vPortafolio=" & ddlPortafolio.SelectedValue & "&vFecha=" & decFecha.ToString() & "&vFechaFin=" & decFechaFin.ToString() & "&estado=" & Estado & "&vEstado=" & vEstado & "&vRegSBS=" & regSBS & "&vliqAntFon=" & liqAntFon & "&vNomPortafolio=" & ddlPortafolio.SelectedItem.Text

                EjecutarJS("showModalDialog('" & Reporte & "', '800', '600','');")
                'EjecutarJS("ShowPopup('" + Me.ddlPortafolio.SelectedValue + "','" + decFecha.ToString() + "','" + decFechaFin.ToString() + "','" + Estado + "','" + vEstado + "','" + regSBS + "','" + liqAntFon + "','" + ddlPortafolio.SelectedItem.Text + "');")
            Else
                AlertaJS(ObtenerMensaje("CONF36"))
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF36"))
        End Try
    End Sub

    Private Sub ddlEstado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEstado.SelectedIndexChanged
        Try
            If ddlEstado.SelectedValue = "E-APR" Then
                lblExceso.Attributes.Add("class", "col-sm-4 control-label")
                Me.ddlAprobados.Visible = True
            Else
                lblExceso.Attributes.Add("class", "col-sm-4 control-label hidden")
                Me.ddlAprobados.Visible = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

#End Region

#Region " /* Métodos de la Página */ "

    Private Sub CargarPortafolio()
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Items.Insert(0, New ListItem("--Seleccione--", ""))
    End Sub

    Private Sub LoadEstadoOrdenes()
        Dim ordenInveriones As New OrdenPreOrdenInversionBM
        Try
            Dim dsOrdenesInversion As DataTable = ordenInveriones.GetEstadoOrdenes(Me.DatosRequest)
            HelpCombo.LlenarComboBox(ddlEstado, dsOrdenesInversion, "CodigoEstado", "DescripcionEstado", True, "-- Todos --")
            Me.ddlEstado.Items.Insert(1, New ListItem("CONFIRMADA-ELIMINADA-MODIFICADA", "E-CONELIMOD"))
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF36"))
        End Try
    End Sub

#End Region

End Class
