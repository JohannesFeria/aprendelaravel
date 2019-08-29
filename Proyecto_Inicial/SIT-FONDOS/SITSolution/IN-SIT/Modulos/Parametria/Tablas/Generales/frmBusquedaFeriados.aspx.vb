Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaFeriados
    Inherits BasePage

#Region " */ Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try            
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
            End If            
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Me.dgLista.PageIndex = 0
            CargarGrilla()
            If Me.dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Protected Sub ibIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibIngresar.Click
        Try
            Response.Redirect("frmFeriado.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Protected Sub ibCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            Dim imgEliminar As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                imgEliminar = DirectCast(e.Row.FindControl("ibEliminar"), ImageButton)
                imgEliminar.Attributes.Add("onclick", ConfirmJS("¿Confirmar la eliminación del registro?"))
                e.Row.Cells(2).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(e.Row.Cells(2).Text))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la carga de datos de la Grilla")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmFeriado.aspx?cod=" & e.CommandArgument.Split(",").GetValue(0).ToString() & "&merca=" & e.CommandArgument.Split(",").GetValue(1).ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oFeriadoBM As New FeriadoBM
            Dim codigo As String = e.CommandArgument.Split(",").GetValue(0).ToString()
            Dim mercado As String = e.CommandArgument.Split(",").GetValue(1).ToString()
            oFeriadoBM.Eliminar(codigo, mercado, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub
#End Region

#Region " */ Funciones Personalizadas */"

    Private Sub CargarGrilla()
        Dim oFeriadoBM As New FeriadoBM
        Dim strSituacion As String
        Dim decAnio As Decimal
        If Me.ddlSituacion.SelectedItem.Text.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            strSituacion = String.Empty
        Else
            strSituacion = Me.ddlSituacion.SelectedValue
        End If
        If Me.txtAnio.Text.Equals(String.Empty) Then
            decAnio = 0
        Else
            decAnio = Me.txtAnio.Text
        End If
        Dim dtblDatos As New DataTable
        dtblDatos = oFeriadoBM.SeleccionarPorFiltro(decAnio, strSituacion, DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()

        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos.Rows.Count) + "');")

    End Sub

    Public Sub CargarCombos()

        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

        Me.txtAnio.Text = DateTime.Now.Year

    End Sub

    Public Sub LimpiarConsulta()

        Me.txtAnio.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0

    End Sub

#End Region
   
End Class
