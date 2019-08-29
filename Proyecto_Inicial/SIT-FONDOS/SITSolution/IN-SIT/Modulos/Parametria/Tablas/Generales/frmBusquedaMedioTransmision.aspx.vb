Imports SIT.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaMedioTransmision
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load        
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception            
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("nombre") = txtNombre.Text
            CargarGrilla(ViewState("nombre"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmMedioTransmision.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub CargarPagina()
        ViewState("nombre") = ""
        CargarTipoRenta()
        CargarGrilla()
    End Sub

    Public Sub CargarTipoRenta()
        Dim tablaTipoRenta As New Data.DataTable
        Dim oTipoRentaBM As New TipoRentaBM

        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)
    End Sub

    Private Sub CargarGrilla(Optional ByVal nombre As String = "")
        Dim tipoRenta As String
        tipoRenta = IIf(ddlTipoRenta.SelectedValue = "Todos", "", ddlTipoRenta.SelectedValue)        
        Dim dtsDatos As DataSet = New ParametrosGeneralesBM().SeleccionarMedioTransmision(nombre, "", tipoRenta, DatosRequest)
        dgLista.DataSource = dtsDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lblContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtsDatos.Tables(0)) + "');")
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Response.Redirect("frmMedioTransmision.aspx?cod=" & e.CommandArgument)
            End If
            If e.CommandName = "Eliminar" Then
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                oParametrosGeneralesBM.Eliminar(ParametrosSIT.MEDIO_TRANSMISION, e.CommandArgument, DatosRequest)
                CargarGrilla(ViewState("nombre"))
                AlertaJS("Los cambios se han realizado satisfactoriamente!")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

End Class
