Imports Sit.BusinessLayer
Imports System.Web.UI.WebControls
Imports System.Data
Imports UIUtility

Public Class Modulos_PrevisionPagos_frmRegPagos
    Inherits BasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            bBoton.Visible = False
            bAnular.Visible = False
            CargarCombos(ddlTipoOperacion, 4)

            Dim ObtenerCierre As New DataSet
            ObtenerCierre = PrevisionCierreBM.ObtenerPrevisionCierre()
            lblHoraCierre.Text = ObtenerCierre.Tables(0).Rows(1)(0)
            Dim Hora As String = ""
            Hora = ObtenerCierre.Tables(0).Rows(1)(0)
        End If
    End Sub

    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddl, tablaListaParametria, "Valor", "Descripcion", True)
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim Fecha As Decimal = 0
            Fecha = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)

            If Fecha = 0 Then
                AlertaJS("Debe ingresar la fecha de pago.")
            Else
                CargarGrilla()
            End If

        Catch ex As Exception
            AlertaJS("Debe ingresar la fecha de pago.")
        End Try
    End Sub

    Protected Sub bBoton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bBoton.Click
        Dim Fecha As Decimal = 0
        Dim FechaActual As Decimal = 0
        Try
            Fecha = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            FechaActual = UIUtility.ConvertirFechaaDecimal(DateTime.Now.ToString.Substring(0, 10))

            If Fecha = 0 Then
                AlertaJS("Debe ingresar la fecha de pago.")
            Else
                If Fecha < FechaActual Then
                    AlertaJS("La fecha ingresada debe ser actual o superior.")
                Else
                    If Fecha > FechaActual Then
                        'Codigo Aprobacion y Anulacion
                        Dim i As Integer = 0
                        Dim Est As String = ""

                        For Each row As GridViewRow In gvPagos.Rows
                            Dim im1 As Image = CType(row.FindControl("ImgPendiente"), Image)
                            Dim im2 As Image = CType(row.FindControl("ImgAprobado"), Image)
                            Dim im3 As Image = CType(row.FindControl("ImgAnulado"), Image)


                            If im2.Visible = True Or im3.Visible = True Then

                            Else

                                Dim chk As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                                If chk.Checked = True Then
                                    Dim Codigo As String = row.Cells(2).Text

                                    If PrevisionAprobacionBM.ActualizarAprobacion(Codigo, Usuario, "APR") = True Then
                                        AlertaJS("Aprobación realizada correctamente")
                                    Else
                                        AlertaJS("Ha ocurrido un error al momento de realizar la aprobación")
                                    End If
                                End If
                            End If
                        Next

                    Else
                        If Fecha = FechaActual Then
                            Dim HoraActual As Integer = 0
                            Dim MinutosActual As Integer = 0

                            Dim HoraCierre As Integer = 0
                            Dim MinutosCierre As Integer = 0



                            HoraActual = CInt(DateTime.Now.Hour.ToString())
                            MinutosActual = CInt(DateTime.Now.Minute.ToString())
                            HoraCierre = CInt(lblHoraCierre.Text.Substring(0, 2))
                            MinutosCierre = CInt(lblHoraCierre.Text.Substring(3, 2))

                            If MinutosCierre = 0 Then
                                MinutosCierre = 60
                            End If

                            If HoraActual > HoraCierre Then
                                AlertaJS("Se ha superado la Hora de Cierre.")
                            Else
                                If HoraActual = HoraCierre And MinutosActual > MinutosCierre Then
                                    AlertaJS("Se ha superado la Hora de Cierre.")
                                Else

                                    'Codigo Aprobacion y Anulacion
                                    Dim i As Integer = 0
                                    Dim Est As String = ""

                                    For Each row As GridViewRow In gvPagos.Rows
                                        Dim im1 As Image = CType(row.FindControl("ImgPendiente"), Image)
                                        Dim im2 As Image = CType(row.FindControl("ImgAprobado"), Image)
                                        Dim im3 As Image = CType(row.FindControl("ImgAnulado"), Image)


                                        If im2.Visible = True Or im3.Visible = True Then

                                        Else

                                            Dim chk As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                                            If chk.Checked = True Then
                                                Dim Codigo As String = row.Cells(2).Text

                                                If PrevisionAprobacionBM.ActualizarAprobacion(Codigo, Usuario, "APR") = True Then
                                                    AlertaJS("Aprobación realizada correctamente")
                                                Else
                                                    AlertaJS("Ha ocurrido un error al momento de realizar la aprobación")
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Else

                        End If
                    End If
                End If
            End If

            CargarGrilla()

        Catch ex As Exception
            AlertaJS("Debe ingresar la fecha de pago.")
        End Try

    End Sub

    Protected Sub bAnular_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bAnular.Click
        Dim Fecha As Decimal = 0
        Dim FechaActual As Decimal = 0
        Try
            Fecha = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            FechaActual = UIUtility.ConvertirFechaaDecimal(DateTime.Now.ToString.Substring(0, 10))

            If Fecha = 0 Then
                AlertaJS("Debe ingresar la fecha de pago.")
            Else
                If Fecha < FechaActual Then
                    AlertaJS("La fecha ingresada debe ser actual o superior.")
                Else
                    If Fecha > FechaActual Then
                        'Codigo Aprobacion y Anulacion
                        Dim i As Integer = 0
                        Dim Est As String = ""

                        For Each row As GridViewRow In gvPagos.Rows
                            Dim im1 As Image = CType(row.FindControl("ImgPendiente"), Image)
                            Dim im2 As Image = CType(row.FindControl("ImgAprobado"), Image)
                            Dim im3 As Image = CType(row.FindControl("ImgAnulado"), Image)


                            If im3.Visible = True Then

                            Else
                                Dim chk As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                                If chk.Checked = True Then
                                    Dim Codigo As String = row.Cells(2).Text

                                    If PrevisionAprobacionBM.ActualizarAprobacion(Codigo, Usuario, "ANU") Then
                                        AlertaJS("Anulación realizada correctamente")
                                    Else
                                        AlertaJS("Ha ocurrido un error al momento de anular el pago")
                                    End If

                                End If
                            End If
                        Next
                    Else
                        If Fecha = FechaActual Then
                            Dim HoraActual As Integer = 0
                            Dim MinutosActual As Integer = 0

                            Dim HoraCierre As Integer = 0
                            Dim MinutosCierre As Integer = 0

                            HoraActual = CInt(DateTime.Now.Hour.ToString())
                            MinutosActual = CInt(DateTime.Now.Minute.ToString())
                            HoraCierre = CInt(lblHoraCierre.Text.Substring(0, 2))
                            MinutosCierre = CInt(lblHoraCierre.Text.Substring(3, 2))

                            If MinutosCierre = 0 Then
                                MinutosCierre = 60
                            End If

                            If HoraActual > HoraCierre Then
                                AlertaJS("Se ha superado la Hora de Cierre.")
                            Else
                                If HoraActual = HoraCierre And MinutosActual > MinutosCierre Then
                                    AlertaJS("Se ha superado la Hora de Cierre.")
                                Else
                                    'Codigo Aprobacion y Anulacion
                                    Dim i As Integer = 0
                                    Dim Est As String = ""

                                    For Each row As GridViewRow In gvPagos.Rows
                                        Dim im1 As Image = CType(row.FindControl("ImgPendiente"), Image)
                                        Dim im2 As Image = CType(row.FindControl("ImgAprobado"), Image)
                                        Dim im3 As Image = CType(row.FindControl("ImgAnulado"), Image)


                                        If im3.Visible = True Then

                                        Else
                                            Dim chk As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                                            If chk.Checked = True Then
                                                Dim Codigo As String = row.Cells(2).Text


                                                If PrevisionAprobacionBM.ActualizarAprobacion(Codigo, Usuario, "ANU") = True Then
                                                    AlertaJS("Anulación realizada correctamente")
                                                Else
                                                    AlertaJS("Ha ocurrido un error al momento de anular el pago")
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Else

                        End If
                    End If
                End If
            End If
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Debe ingresar la fecha de pago.")
        End Try
    End Sub

    Private Sub CargarGrilla()
        Dim IdTipoOperacion As String = ""
        IdTipoOperacion = IIf(ddlTipoOperacion.SelectedIndex = 0, "T", ddlTipoOperacion.SelectedValue)
        Dim dtTabla As Data.DataTable
        dtTabla = PrevisionAprobacionBM.ListarPagosAprobar(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), Usuario, IdTipoOperacion).Tables(0)
        If dtTabla.Rows.Count > 0 Then
            gvPagos.DataSource = dtTabla
            bBoton.Visible = True
            bAnular.Visible = True
        Else
            bBoton.Visible = False
            bAnular.Visible = False
        End If
        gvPagos.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtTabla) + "');")
    End Sub

    Protected Sub gvPagos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPagos.PageIndexChanging
        gvPagos.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub


    Protected Sub gvPagos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPagos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)

                Dim Estado As String = rowView.Row.Item(9).ToString

                Dim im1 As Image = e.Row.FindControl("ImgPendiente")
                Dim im2 As Image = e.Row.FindControl("ImgAprobado")
                Dim im3 As Image = e.Row.FindControl("ImgAnulado")

                If Estado = "PENDIENTE" Then
                    im1.Visible = True
                    im2.Visible = False
                    im3.Visible = False
                End If

                If Estado = "APROBADO" Then
                    im1.Visible = False
                    im2.Visible = True
                    im3.Visible = False
                End If

                If Estado = "ANULADO" Then
                    im1.Visible = False
                    im2.Visible = False
                    im3.Visible = True
                End If

            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

End Class