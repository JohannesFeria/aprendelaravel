Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaPorcentajeRating
    Inherits BasePage

#Region "Eventos de la pagina"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbRating.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
	    CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try   
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmPorcentajeRating.aspx")
        Catch ex As Exception            
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim parameters() As String
            If e.CommandName.ToString() = "Modificar" Then
                parameters = Convert.ToString(e.CommandArgument).Split(";")
                Dim parCadenaUrl As New StringBuilder
                parCadenaUrl.Append("rating=" & parameters(0).ToString() & "&")
                parCadenaUrl.Append("categinver=" & parameters(1).ToString() & "&")
                parCadenaUrl.Append("portafolio=" & parameters(2).ToString() & "&") 'HDG 20121002 rating
                parCadenaUrl.Append("grupo=" & parameters(3).ToString())    'HDG 20121002 rating

                Response.Redirect("frmPorcentajeRating.aspx?" & parCadenaUrl.ToString())
            End If
            If e.CommandName.ToString() = "Eliminar" Then
                parameters = Convert.ToString(e.CommandArgument).Split(";")

                Dim oPorcentajeNivelRatingBE As New PorcentajeNivelRatingBE
                Dim oPorcentajeNivelRatingBM As New PorcentajeNivelRatingBM
                Dim oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow
                oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.NewRow(), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)
                oPorcentajeNivelRatingBM.InicializarPorcentajeNivelRating(oRow, DatosRequest)
                oRow.ValorCaracteristica = parameters(0).ToString()
                oRow.CategInver = parameters(1).ToString()
                oRow.CodigoPortafolioSBS = parameters(2).ToString()
                oRow.GrupoRating = parameters(3).ToString() 'HDG 20121002 rating
                oPorcentajeNivelRatingBE.PorcentajeNivelRating.AddPorcentajeNivelRatingRow(oRow)
                oPorcentajeNivelRatingBE.AcceptChanges()
                oPorcentajeNivelRatingBM.Eliminar(oPorcentajeNivelRatingBE, DatosRequest)

                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region "Metodos personalizados"

    Private Sub CargarPagina()
        CargarPortafolio()
        CargarCategoriaInv()
        CargarSituacion()
        CargarGrilla()
    End Sub

    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        ddlPortafolio.DataSource = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        Dim lt As New ListItem("--TODOS--", "")
        ddlPortafolio.Items.Insert(0, lt)
    End Sub

    Private Sub CargarCategoriaInv()
        Dim oPorcentajeRating As New PorcentajeNivelRatingBM
        Dim dsCategInver As DataSet = oPorcentajeRating.SeleccionarCategoriaInversiones(DatosRequest)
        HelpCombo.LlenarComboBox(ddlCategoria, dsCategInver.Tables(0), "Codigo", "Descripcion", True, "TODOS")
    End Sub

    Private Sub CargarGrilla()
        Dim oPorcentajeRating As New PorcentajeNivelRatingBM
        Dim dsPorcentaje As DataSet
        Dim oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow
        Dim oPorcentajeNivelRatingBE As New PorcentajeNivelRatingBE
        Dim nTotalReg As Integer
        oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.NewRow(), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)
        oPorcentajeRating.InicializarPorcentajeNivelRating(oRow, DatosRequest)
        oRow.CategInver = ddlCategoria.SelectedValue
        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        oRow.ValorCaracteristica = tbRating.Text
        oRow.GrupoRating = ""
        oRow.Situacion = ddlSituacion.SelectedValue

        oPorcentajeNivelRatingBE.PorcentajeNivelRating.AddPorcentajeNivelRatingRow(oRow)
        oPorcentajeNivelRatingBE.PorcentajeNivelRating.AcceptChanges()

        dsPorcentaje = oPorcentajeRating.SeleccionarPorFiltro(oPorcentajeNivelRatingBE, DatosRequest)
        nTotalReg = dsPorcentaje.Tables(0).Rows.Count
        lbContador.Text = "Registros encontrados: " + nTotalReg.ToString()
        dgLista.DataSource = dsPorcentaje
        dgLista.DataBind()
    End Sub

    Private Sub CargarSituacion()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oDt As New DataTable
        oDt = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.SITUACION, String.Empty, String.Empty, String.Empty, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, oDt, "Valor", "Nombre", True, "TODOS")
        ddlSituacion.SelectedIndex = -1
    End Sub

#End Region

End Class
