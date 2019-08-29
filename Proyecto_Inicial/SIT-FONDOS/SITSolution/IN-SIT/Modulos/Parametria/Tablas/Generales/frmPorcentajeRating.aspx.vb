Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmPorcentajeRating
    Inherits BasePage

#Region "Eventos de la pagina"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbRating.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                hdCodRating.Value = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Guardar()
            Me.AlertaJS("Se guardo satisfactoriamente")
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPorcentajeRating.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

#End Region

#Region "Metodos Personalizados"

    Private Sub CargarPagina()
        ViewState("accion") = "1"
        HabilitarControles(True)
        CargarTipoInv()
        CargarPeriodoInv()
        CargarPortafolio()
        CargarSituacion()
        If Not (Request.QueryString("rating") Is Nothing And _
            Request.QueryString("categinver") Is Nothing And _
            Request.QueryString("portafolio") Is Nothing) Then
            ViewState("rating") = Request.QueryString("rating").ToString()
            ViewState("categinver") = Request.QueryString("categinver").ToString()
            ViewState("portafolio") = Request.QueryString("portafolio").ToString()
            ViewState("grupo") = Request.QueryString("grupo").ToString()  'HDG 20121002 rating

            Dim oPorcentajeNivelRatingBM As New PorcentajeNivelRatingBM
            Dim oPorcentajeNivelRatingBE As New PorcentajeNivelRatingBE
            Dim oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow
            Dim oDt As DataTable
            oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.NewRow(), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)
            oPorcentajeNivelRatingBM.InicializarPorcentajeNivelRating(oRow, DatosRequest)
            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
            Dim dtParametro As DataTable
            dtParametro = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.RATING, String.Empty, ViewState("rating"), String.Empty, DatosRequest)
            oRow.ValorCaracteristica = Convert.ToString(dtParametro.Rows(0)("Nombre"))
            oRow.CategInver = ViewState("categinver")
            oRow.CodigoPortafolioSBS = ViewState("portafolio")
            oRow.GrupoRating = ViewState("grupo")   'HDG 20121002 rating
            oPorcentajeNivelRatingBE.PorcentajeNivelRating.AddPorcentajeNivelRatingRow(oRow)
            oPorcentajeNivelRatingBE.PorcentajeNivelRating.AcceptChanges()
            oDt = oPorcentajeNivelRatingBM.SeleccionarPorFiltro(oPorcentajeNivelRatingBE, DatosRequest).Tables(0)

            If oDt.Rows.Count > 0 Then
                tbRating.Text = Convert.ToString(oDt.Rows(0)("DescRating"))
                hdCodRating.Value = Convert.ToString(oDt.Rows(0)("CodigoRating"))
                ddlTipoInv.SelectedValue = Convert.ToString(oDt.Rows(0)("CategInver")).Substring(0, 1)
                ddlPeriodoInv.SelectedValue = Convert.ToString(oDt.Rows(0)("CategInver")).Substring(1, 1)
                ddlPortafolio.SelectedValue = Convert.ToString(oDt.Rows(0)("CodigoPortafolioSBS"))
                tbPorcentaje.Text = String.Format("{0:###.00}", oDt.Rows(0)("ValorPorcentaje"))
                ddlSituacion.SelectedValue = Left(Convert.ToString(oDt.Rows(0)("Situacion")), 1)
                tbGrupoRating.Text = Convert.ToString(oDt.Rows(0)("GrupoRating"))    'HDG 20121002 rating

                HabilitarControles(False)
                ViewState("accion") = "2"
            Else
                LimpiarControles()
            End If
        End If
    End Sub

    Private Sub CargarTipoInv()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oDt As DataTable
        Dim drArray() As DataRow
        oDt = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.TIPO_INVERSION, String.Empty, String.Empty, String.Empty, DatosRequest)
        Dim dtResult As DataTable = oDt.Clone()
        drArray = oDt.Select(Nothing, "Valor", DataViewRowState.CurrentRows)
        For Each dr As DataRow In drArray
            dtResult.ImportRow(dr)
        Next
        dtResult.AcceptChanges()
        HelpCombo.LlenarComboBox(ddlTipoInv, dtResult, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarPeriodoInv()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oDt As DataTable
        Dim drArray() As DataRow
        oDt = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.PERIODO_INVERSION, String.Empty, String.Empty, String.Empty, DatosRequest)
        Dim dtResult As DataTable = oDt.Clone()
        drArray = oDt.Select(Nothing, "Valor", DataViewRowState.CurrentRows)
        For Each dr As DataRow In drArray
            dtResult.ImportRow(dr)
        Next
        dtResult.AcceptChanges()
        HelpCombo.LlenarComboBox(ddlPeriodoInv, dtResult, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        ddlPortafolio.DataSource = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        Dim lt As New ListItem("--Seleccione--", "")
        ddlPortafolio.Items.Insert(0, lt)
    End Sub

    Private Sub CargarSituacion()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oDt As DataTable
        oDt = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.SITUACION, String.Empty, String.Empty, String.Empty, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, oDt, "Valor", "Nombre", False)
        ddlSituacion.SelectedIndex = -1
    End Sub

   Private Sub Guardar()
        Dim oPorcentajeNivelRatingBM As New PorcentajeNivelRatingBM
        Dim oPorcentajeNivelRatingBE As New PorcentajeNivelRatingBE
        Dim oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow
        oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.NewRow(), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)
        oPorcentajeNivelRatingBM.InicializarPorcentajeNivelRating(oRow, DatosRequest)

        oRow.ValorCaracteristica = hdCodRating.Value
        oRow.CategInver = ddlTipoInv.SelectedItem.Value & ddlPeriodoInv.SelectedItem.Value
        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedItem.Value
        oRow.Situacion = ddlSituacion.SelectedValue
        oRow.ValorPorcentaje = Convert.ToDecimal(tbPorcentaje.Text)
        oRow.GrupoRating = tbGrupoRating.Text   'HDG 20121002 rating

        oPorcentajeNivelRatingBE.PorcentajeNivelRating.AddPorcentajeNivelRatingRow(oRow)
        oPorcentajeNivelRatingBE.PorcentajeNivelRating.AcceptChanges()

        If ViewState("accion") = "1" Then
            If oPorcentajeNivelRatingBM.Insertar(oPorcentajeNivelRatingBE, DatosRequest) Then
                LimpiarControles()
                AlertaJS("Registro ingresado satisfactoriamente!")
            End If
        ElseIf ViewState("accion") = "2" Then
            If oPorcentajeNivelRatingBM.Modificar(oPorcentajeNivelRatingBE, DatosRequest) Then
                LimpiarControles()
                lkbBuscarRating.Visible = True
                AlertaJS("Cambio actualizado satisfactoriamente")
            End If
        End If

    End Sub

    Private Sub LimpiarControles()
        tbPorcentaje.Text = ""
        tbRating.Text = ""
        hdCodRating.Value = ""
        ddlPeriodoInv.SelectedIndex = -1
        ddlTipoInv.SelectedIndex = -1
        ddlPortafolio.SelectedIndex = -1
        tbGrupoRating.Text = "" 'HDG 20121002 rating
    End Sub

    Private Sub HabilitarControles(ByVal habilita As Boolean)
        ddlPeriodoInv.Enabled = habilita
        ddlTipoInv.Enabled = habilita
        ddlPortafolio.Enabled = habilita
        lkbBuscarRating.Visible = habilita
    End Sub

#End Region

End Class
