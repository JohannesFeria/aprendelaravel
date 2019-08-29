Imports Sit.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Data
Imports UIUtility

Public Class Modulos_PrevisionPagos_frmPagos
    Inherits BasePage

    'Dim IdUsuario As String = Usuario.ToString
   
    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBox(ddl, tablaListaParametria, "Valor", "Descripcion", True)
    End Sub

    Private Sub CargarControles()
        CargarCombos(ddlTipoOperacion, 4)
        CargarCombos(ddlEstado, 5)
        txtFechaPago.Text = String.Empty
    End Sub

    Private Sub BuscarPagos()
        Try
            Dim valorFechaPago, valorIdTipoOperacion, valorIsEstado As String
            valorFechaPago = IIf(txtFechaPago.Text.Trim = String.Empty, "0", Convert.ToString(UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text.Trim)))
            valorIdTipoOperacion = IIf(ddlTipoOperacion.SelectedIndex <= 0, "0", ddlTipoOperacion.SelectedValue.Trim)
            valorIsEstado = IIf(ddlEstado.SelectedIndex <= 0, "0", ddlEstado.SelectedValue.Trim)

            Dim dtblDatos As New DataSet
            dtblDatos = PrevisionPagoBM.ListarPrevisionPago(valorFechaPago, valorIdTipoOperacion, valorIsEstado, Usuario)
            gvPagos.DataSource = dtblDatos
            gvPagos.DataBind()
            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")

            For i As Integer = 0 To gvPagos.Rows.Count - 1
                Dim img1 As ImageButton = DirectCast(gvPagos.Rows(i).FindControl("ibEliminar"), ImageButton)
                If gvPagos.Rows(i).Cells(4).Text = "ANULADO" Then
                    img1.Enabled = False
                End If
            Next
        Catch ex As Exception
            AlertaJS(ex.ToString)
        End Try        
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Session("sCodigoPago") = Nothing
            CargarControles()
        End If
    End Sub

    Protected Sub ibIngresar_Click(sender As Object, e As System.EventArgs) Handles ibIngresar.Click
        Response.Redirect("frmPagosDetalle.aspx")
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        BuscarPagos()
    End Sub

    Protected Sub gvPagos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPagos.PageIndexChanging
        gvPagos.PageIndex = e.NewPageIndex
        BuscarPagos()
    End Sub

    Protected Sub gvPagos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPagos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ibt1 As ImageButton = e.Row.FindControl("ibModificar")
                Dim ibt2 As ImageButton = e.Row.FindControl("ibEliminar")
                ibt1.CommandArgument = e.Row.RowIndex
                ibt2.CommandArgument = e.Row.RowIndex

                'Cambios

                Dim Fecha As Decimal = 0
                Dim FechaActual As Decimal = 0

                Fecha = UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text)
                FechaActual = UIUtility.ConvertirFechaaDecimal(DateTime.Now.ToString.Substring(0, 10))

                Dim ObtenerCierre As New DataSet
                ObtenerCierre = PrevisionCierreBM.ObtenerPrevisionCierre()
                Dim HoraCierre As String = ObtenerCierre.Tables(0).Rows(0)(0)

                If Fecha = 0 Then
                    ibt1.Enabled = False
                    ibt2.Enabled = False
                Else

                    If Fecha < FechaActual Then
                        ibt1.Enabled = False
                        ibt2.Enabled = False
                    Else

                        If Fecha > FechaActual Then
                            ibt1.Enabled = True
                            ibt2.Enabled = True
                        Else

                            If Fecha = FechaActual Then
                                Dim HoraActual As Integer = 0
                                Dim MinutosActual As Integer = 0

                                Dim HoraCierres As Integer = 0
                                Dim MinutosCierre As Integer = 0

                                HoraActual = CInt(DateTime.Now.Hour.ToString())
                                MinutosActual = CInt(DateTime.Now.Minute.ToString())
                                HoraCierres = CInt(HoraCierre.ToString.Substring(0, 2))
                                MinutosCierre = CInt(HoraCierre.ToString.Substring(3, 2))

                                If MinutosCierre = 0 Then
                                    MinutosCierre = 60
                                End If

                                If HoraActual > HoraCierres Then
                                    ibt1.Enabled = False
                                    ibt2.Enabled = False
                                Else
                                    If HoraActual = HoraCierres And MinutosActual > MinutosCierre Then
                                        ibt1.Enabled = False
                                        ibt2.Enabled = False
                                    Else
                                        ibt1.Enabled = True
                                        ibt2.Enabled = True
                                    End If
                                End If

                            Else

                            End If
                        End If
                    End If
                End If

                Dim hdapro As HtmlInputHidden = e.Row.FindControl("hdEstAprobacion")

                If hdapro.Value = "1" Then
                    ibt1.Enabled = False
                    ibt2.Enabled = False
                End If

            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub gvPagos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPagos.RowCommand
        Try

            Dim index As String
            'Condición para ver cuando el paginado envía los valores "First" o "Last"
            If e.CommandArgument = "First" Then
                index = "1"
            ElseIf e.CommandArgument = "Last" Then
                Dim auxIndex As Integer = gvPagos.Rows.Count / 10
                index = auxIndex
            Else
                index = e.CommandArgument
            End If

            Dim row As GridViewRow = gvPagos.Rows(index)

            If e.CommandName = "Modificar" Then
                Session("sCodigoPago") = gvPagos.DataKeys(index)("CodigoPago")
                Session("sEstado") = row.Cells(4).Text.Trim
                Response.Redirect("frmPagosDetalle.aspx")
            End If
            If e.CommandName = "Eliminar" Then
                If PrevisionPagoBM.EliminarPrevisionPago(gvPagos.DataKeys(index)("CodigoPago"), Usuario) = True Then
                    'Session("sCodigoPago").ToString
                    BuscarPagos()
                    'Response.Write("<script>window.alert('Registro anulado.')</script>")
                Else
                    AlertaJS("Error al registrar")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub ibCancelar_Click(sender As Object, e As System.EventArgs) Handles ibCancelar.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub
End Class
