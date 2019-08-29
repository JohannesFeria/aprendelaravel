Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmResultadoEncaje
    Inherits BasePage


#Region "/*Variables*/"

    Dim oEncajeDetalleBE As New EncajeDetalleBE
    Dim oEncajeDetalleBM As New EncajeDetalleBM

#End Region

#Region "/* Metodos de la pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Session("dsEncajeDetalle") = Nothing
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try            
            If ddlPortafolio.SelectedValue = "" Or Me.txtFechaProceso.Text = "" Then
                AlertaJS("Faltan ingresar parámetros")
            Else
                Me.dgLista.PageIndex = 0
                CargarGrilla()
                If Me.dgLista.Rows.Count = 0 Then
                    AlertaJS("No se encontraron Registros")
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try        
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Me.txtFechaProceso.Text = UIUtility.ConvertirFechaaString(New EncajeBM().ObtenerFechaUltimoEncaje(Me.ddlPortafolio.SelectedValue, 0, DatosRequest))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim decfechaProceso As String = UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text)
            EjecutarJS("ShowModal(" + decfechaProceso + ");")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
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

#Region "/* Metodos Personalizados */"

    Private Sub CargarCombos()
        Dim DtTablaPortafolio As DataTable
        Dim oPortafolioBM As New PortafolioBM
        DtTablaPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, DtTablaPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub CargarGrilla()
        Dim dtblDatos As DataTable = oEncajeDetalleBM.ResultadosEncaje(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text), Me.DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()        
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos.Rows.Count) + "')")
    End Sub

#End Region

End Class