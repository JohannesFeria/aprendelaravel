Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmBusquedaTerceroMandato
    Inherits BasePage

#Region "Variables"


#End Region

#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarSituacion()
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
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

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'Dim ibnSeleccionar As ImageButton
                'ibnSeleccionar = DirectCast(e.Row.FindControl("ibnSeleccionar"), ImageButton)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Metodos"
    Public Sub Seleccionar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim StrCadena As String
        StrCadena = e.CommandArgument
        Session("ClienteMandato") = StrCadena
        CerrarModal()
    End Sub
    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub CargarGrilla()

        Dim oTercerosBM As New TercerosBM
        Dim strSituacion As String
        strSituacion = IIf(ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oTercerosBM.ListarClientesMandato(tbDescripcion.Text.TrimStart.TrimEnd.ToString(), strSituacion, txtCodigoTercero.Text).Terceros
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()

    End Sub

    Private Sub CargarSituacion()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

    End Sub

    Private Sub CerrarModal()
        EjecutarJS("window.close();")
    End Sub

#End Region

End Class
