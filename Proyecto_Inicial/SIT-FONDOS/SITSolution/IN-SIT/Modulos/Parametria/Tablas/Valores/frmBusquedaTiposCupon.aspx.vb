﻿Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaTiposCupon
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
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
            Response.Redirect("frmTiposCupon.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar a la página")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
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

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Response.Redirect("frmTiposCupon.aspx?cod=" & codigo)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar el registro")
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTipoCuponBM As New TipoCuponBM
            Dim codigo As String = e.CommandArgument
            oTipoCuponBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar el registro")
        End Try
    End Sub

    Private Sub CargarDatosGrilla()
        dgLista.PageIndex = 0
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oTipoCuponBM As New TipoCuponBM
        Dim codigo As String
        Dim descripcion As String
        Dim situacion As String

        codigo = tbCodigo.Text.ToUpper.Trim
        descripcion = tbDescripcion.Text.ToUpper.Trim
        situacion = ddlSituacion.SelectedValue
        If situacion = "Todos" Then
            situacion = ""
        End If
        Dim dtblDatos As DataTable = oTipoCuponBM.SeleccionarPorFiltros(codigo, descripcion, situacion, DatosRequest).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

#End Region

End Class