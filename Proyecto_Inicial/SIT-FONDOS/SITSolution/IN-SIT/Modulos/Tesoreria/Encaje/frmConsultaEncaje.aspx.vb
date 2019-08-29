Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmConsultaEncaje
    Inherits BasePage

#Region " /* Declaración de Variables */ "

    Private campos() As String = {"FechaEncaje", "ValorRequerido", "ValorMantenido", "DiferenciaEncaje", "Estado", "ValorRentabilidad"}
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String"}

#End Region

#Region " /* Métodos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                Dim dtblGenerico As DataTable = UIUtility.GetStructureTablebase(campos, tipos)
                dgLista.DataSource = dtblGenerico : dgLista.DataBind()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Me.dgLista.PageIndex = 0
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try        
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Me.tbFechaEncaje.Text = UIUtility.ConvertirFechaaString(New EncajeBM().ObtenerFechaUltimoEncaje(Me.ddlPortafolio.SelectedValue, 0, DatosRequest))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Métodos Personalizados */ "

    Private Sub CargarDatosGrilla()
        Try
            CargarGrilla()
            If Me.dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub CargarGrilla()
        Dim oEncajeBM As New EncajeBM
        Dim strPortafolio As String
        strPortafolio = Me.ddlPortafolio.SelectedValue
        Dim dtblDatos As DataTable = oEncajeBM.SeleccionarPorFiltro(strPortafolio, Me.tbFechaEncaje.Text, DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos.Rows.Count) + "')")
    End Sub

    Public Sub CargarCombos()
        Dim Dtablafondo As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        Dtablafondo = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, Dtablafondo, "CodigoPortafolio", "Descripcion", True)
    End Sub

#End Region

End Class