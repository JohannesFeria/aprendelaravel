Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data
Imports UIUtility

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaPatrimonioFideicomiso
    Inherits BasePage
    Private CodigoFideicomiso As String

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmPatrimonioFideicomiso.aspx?ope=reg")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Dim oPatrimonioFideicomisoBM As New PatrimonioFideicomisoBM
            Dim dt As DataTable, dr As DataRow, ary() As Object
            Dim sb As New System.Text.StringBuilder, sf As String
            Dim iRow As Integer, iCol As Integer
            Dim strNombreReporte As String
            Dim situacion As String

            strNombreReporte = "PatrimonioFideicomiso_" & Date.Now.ToShortDateString().Replace("/", "_") & ".txt"
            Response.AddHeader("Content-Disposition", "attachment; filename= " & strNombreReporte)
            Response.ContentType = "application/vnd.ms-text"
            Response.Charset = ""
            situacion = IIf(ddlSituacion.SelectedIndex = 0, "", ddlSituacion.SelectedValue)
            dt = oPatrimonioFideicomisoBM.SeleccionarPorFiltroExportar("", tbDescripcion.Text, situacion, DatosRequest)

            sf = ""
            For iCol = 0 To dt.Columns.Count - 1
                sf += Chr(124).ToString + dt.Columns(iCol).ToString
            Next
            sb.Append(sf.Substring(1) + vbCrLf)

            For iRow = 0 To dt.Rows.Count - 1
                dr = dt.Rows.Item(iRow)
                ary = dr.ItemArray
                sf = ""
                For iCol = 0 To UBound(ary)
                    sf += Chr(124).ToString + ary(iCol).ToString
                Next
                sb.Append(sf.Substring(1) + vbCrLf)
            Next
            Response.Write(sb.ToString)
            Response.End()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar")
        End Try        
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmPatrimonioFideicomisoImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            dgLista.DataSource = CType(ViewState("dtPatrimonioFideicomiso"), PatrimonioFideicomisoBE)
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error el la Paginación")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If (e.CommandName = "Eliminar" Or e.CommandName = "Modificar") Then
                CodigoFideicomiso = CType(dgLista.Rows(CInt(e.CommandArgument)).FindControl("_CodigoPatrimonioFideicomiso"), HiddenField).Value
            End If
            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oPatrimonioFideicomisoBM As New PatrimonioFideicomisoBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oPatrimonioFideicomisoBM.Eliminar(CodigoFideicomiso, DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString())
                    End Try
                Case "Modificar"
                    Response.Redirect("frmPatrimonioFideicomiso.aspx?ope=mod&codigo=" + CodigoFideicomiso)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(7).Text = "A") Then
                    e.Row.Cells(7).Text = "Activo"
                ElseIf (e.Row.Cells(7).Text = "I") Then
                    e.Row.Cells(7).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oPatrimonioFideicomisoBM As New PatrimonioFideicomisoBM
        Dim ds As PatrimonioFideicomisoBE
        Dim situacion As String
        situacion = IIf(ddlSituacion.SelectedIndex = 0, "", ddlSituacion.SelectedValue)
        ds = oPatrimonioFideicomisoBM.SeleccionarPorFiltro("", tbDescripcion.Text, situacion, DatosRequest)
        ViewState("dtPatrimonioFideicomiso") = ds
        dgLista.DataSource = ds
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(ds.Tables(0)) + "');")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        tbDescripcion.Text = ""
        ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

End Class
