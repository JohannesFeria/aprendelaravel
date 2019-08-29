'Creado por: HDG OT 64480 20120119
Option Strict On
Option Explicit On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaRolAprobadoresTrader
    Inherits BasePage


#Region " /* Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnActualizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        Try
            Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
            Dim obj As New JobBM
            Dim Variable As String = ""
            Dim Parametros As String = ""
            Dim Resultado As String
            Dim Fecha As Decimal
            Fecha = oRolAprobadoresTraderBM.ObtieneFechaActualizacionMovPersonal()
            Variable = "TmpFecha"
            Parametros = Fecha.ToString
            Resultado = obj.EjecutarJob("DTS_SIT_CargaMovimientoPersonal", "Procesa la carga actualización de movimientopersonal", Variable, Parametros, "", "", ConfigurationManager.AppSettings(SERVIDORETL))
            AlertaJS("Trabajo DTS_SIT_CargaMovimientoPersonal en ejecucion.")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al actualizar la operación")
        End Try
    End Sub

    Private Sub dgLista_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        ActualizarIndice(e.NewPageIndex)
    End Sub

#End Region

#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmRolAprobadoresTrader.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Eliminar */"

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
            Dim decCodigo As Decimal

            decCodigo = CDec(e.CommandArgument.ToString())

            oRolAprobadoresTraderBM.Eliminar(decCodigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            Buscar()
        End If
    End Sub

    Private Sub Ingresar()
        Response.Redirect("frmRolAprobadoresTrader.aspx")
    End Sub

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()

        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub

    Private Sub CargarGrilla()
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim strSituacion As String
        Dim dtblDatos As DataTable

        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        dtblDatos = oRolAprobadoresTraderBM.SeleccionarPorFiltro(tbDescripcion.Text.TrimStart.TrimEnd, strSituacion).Tables(0)

        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        Me.lbContador.Text = MostrarResultadoBusqueda(dtblDatos)
    End Sub

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Public Sub LimpiarConsulta()
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub MostrarMensajeConfirmacion(ByVal oDataGridItem As GridViewRow)
        Dim imgEliminar As ImageButton

        If oDataGridItem.RowType = DataControlRowType.DataRow Then
            imgEliminar = DirectCast(oDataGridItem.FindControl("ibEliminar"), ImageButton)
            Dim valor As String = "¿Confirmar la eliminación del registro?"
            imgEliminar.Attributes.Add("onclick", ConfirmJS(valor))
        End If
    End Sub

#End Region

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            MostrarMensajeConfirmacion(e.Row)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar los datos en la Grilla")
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub
End Class
