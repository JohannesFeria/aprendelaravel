Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaImpuestosComisiones
    Inherits BasePage
#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not Session("Descripcion") Is Nothing Then
                    tbDescripcion.Text = Session("Descripcion")
                    ddlBolsa.SelectedValue = Session("Bolsa")
                    ddlTipoRenta.SelectedValue = Session("TipoRenta")
                    ddlSituacion.SelectedValue = Session("Situacion")
                    Session.Remove("Descripcion")
                    Session.Remove("Bolsa")
                    Session.Remove("TipoRenta")
                    Session.Remove("Situacion")
                End If
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmImpuestosyComisiones.aspx", False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar a la página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
        End Try
    End Sub
#End Region
#Region " /* Funciones Seleccionar */ "

#End Region
#Region " /* Funciones Insertar */ "

#End Region
#Region "/* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim strCadena, StrCodComision, StrCodMercado, StrCodTipoRenta As String
            strCadena = e.CommandArgument
            StrCodComision = strCadena.Split(",").GetValue(0).ToString
            StrCodMercado = strCadena.Split(",").GetValue(1).ToString
            StrCodTipoRenta = strCadena.Split(",").GetValue(2).ToString
            Session("Descripcion") = tbDescripcion.Text
            Session("Bolsa") = ddlBolsa.SelectedValue
            Session("TipoRenta") = ddlTipoRenta.SelectedValue
            Session("Situacion") = ddlSituacion.SelectedValue
            Response.Redirect("frmImpuestosyComisiones.aspx?cCom=" & StrCodComision & "&cMer=" & StrCodMercado & "&cRenta=" & StrCodTipoRenta)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
#End Region
#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim strCadena, StrCodComision, StrCodMercado, StrCodTipoRenta As String
            Dim oImpuestosComisionesBM As New ImpuestosComisionesBM

            strCadena = e.CommandArgument

            StrCodComision = strCadena.Split(",").GetValue(0).ToString
            StrCodMercado = strCadena.Split(",").GetValue(1).ToString
            StrCodTipoRenta = strCadena.Split(",").GetValue(2).ToString

            oImpuestosComisionesBM.Eliminar(StrCodComision, StrCodMercado, StrCodTipoRenta, DatosRequest)

            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar el registro")
        End Try
    End Sub
#End Region
#Region " /* Funciones Personalizadas*/"
    Private Sub CargarGrilla()
        Dim oImpuestosComisionesBM As New ImpuestosComisionesBM
        Dim situacion As String
        Dim mercado As String
        Dim tipoRenta As String
        Dim descripcion As String
        mercado = Me.ddlBolsa.SelectedValue
        tipoRenta = Me.ddlTipoRenta.SelectedValue
        situacion = Me.ddlSituacion.SelectedValue
        descripcion = Me.tbDescripcion.Text.ToUpper.Trim
        If tipoRenta.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            tipoRenta = String.Empty
        End If
        If situacion.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            situacion = String.Empty
        End If
        If mercado.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            mercado = String.Empty
        End If
        Dim dtblDatos As DataTable = oImpuestosComisionesBM.SeleccionarPorFiltro(descripcion, tipoRenta, mercado, situacion, DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub
    Public Sub CargarCombos()
        Dim tablaTipoRenta As New Data.DataTable
        Dim tablaMercado As New Data.DataTable
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oMercadoBM As New MercadoBM
        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        tablaMercado = oMercadoBM.Listar(DatosRequest).Tables(0)
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)
        Dim oPlazaBM As New PlazaBM
        ddlBolsa.DataSource = oPlazaBM.Listar(Nothing)
        ddlBolsa.DataTextField = "Descripcion"
        ddlBolsa.DataValueField = "CodigoPlaza"
        ddlBolsa.DataBind()
        ddlBolsa.Items.Insert(0, New ListItem("Todos"))
    End Sub
    Private Sub CargarDatosGrilla()
        Try
            CargarGrilla()
            If Me.dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString)
        End Try
    End Sub
#End Region
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar los datos en la Grilla")
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la páginación")
        End Try
    End Sub
End Class