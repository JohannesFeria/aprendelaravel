Option Strict On
Option Explicit On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaCuentasEconomicas
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
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

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmCuentaEconomica.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarGrilla()
            If dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
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
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la operación en la Grilla")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmCuentaEconomica.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
    'OT10795 - Evento cambiar cuenta económica
    Public Sub CambiarCuentaEconomica(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmCambiarCuentaEconomica.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
    'OT10795 - Fin
#End Region

#Region " /* Funciones Eliminar */"

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oCuentaEconomicaBM As New CuentaEconomicaBM
            Dim strCodigoPortafolio, strCuentaContable, strNumeroCuenta As String
            strCodigoPortafolio = e.CommandArgument.ToString().Split(","c)(0)
            strCuentaContable = e.CommandArgument.ToString().Split(","c)(1)
            strNumeroCuenta = e.CommandArgument.ToString().Split(","c)(2)
            oCuentaEconomicaBM.Eliminar(strCodigoPortafolio, strCuentaContable, strNumeroCuenta, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"
    Private Sub CargarGrilla()
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Dim strCodigoPortafolio, strCodigoClaseCuenta, strCodigoTercero, strCodigoMoneda As String
        strCodigoPortafolio = IIf(ddlPortafolio.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlPortafolio.SelectedValue).ToString()
        strCodigoClaseCuenta = IIf(ddlClaseCuenta.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlClaseCuenta.SelectedValue).ToString()
        strCodigoTercero = IIf(ddlBanco.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlBanco.SelectedValue).ToString()
        strCodigoMoneda = IIf(ddlMoneda.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlMoneda.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oCuentaEconomicaBM.SeleccionarPorFiltroMant(strCodigoPortafolio, strCodigoClaseCuenta, strCodigoTercero, strCodigoMoneda, String.Empty, String.Empty, DatosRequest).CuentaEconomica
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Public Sub CargarCombos()
        Dim tablaTerceros As New Data.DataTable
        Dim oEntidadBM As New EntidadBM
        tablaTerceros = oEntidadBM.ListarEntidadFinanciera(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlBanco, tablaTerceros, "CodigoEntidad", "NombreCompleto", True)
        Dim tablaClaseCuenta As New Data.DataTable
        Dim oClaseCuenta As New ClaseCuentaBM
        tablaClaseCuenta = oClaseCuenta.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlClaseCuenta, tablaClaseCuenta, "CodigoClaseCuenta", "Descripcion", True)
        Dim tablaMoneda As New Data.DataTable
        Dim oMoneda As New MonedaBM
        tablaMoneda = oMoneda.Listar("A").Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlMoneda, tablaMoneda, "CodigoMoneda", "CodigoMoneda", True)
        ddlPortafolio.DataSource = oPortafolioBM.ObtenerDatosPortafolio(DatosRequest)
        ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        Dim Lit As New ListItem("Todos", "Todos")
        ddlPortafolio.Items.Insert(0, Lit)
    End Sub

    Public Sub LimpiarConsulta()
        ddlBanco.SelectedIndex = 0
        ddlClaseCuenta.SelectedIndex = 0
        ddlMoneda.SelectedIndex = 0
        ddlPortafolio.SelectedIndex = 0
    End Sub

#End Region

End Class
