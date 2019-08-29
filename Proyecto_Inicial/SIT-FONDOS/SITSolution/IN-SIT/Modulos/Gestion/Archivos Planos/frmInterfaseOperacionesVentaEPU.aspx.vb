Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseOperacionesVentaEPU
    Inherits BasePage


    Private oTmpOperacionesEPUDetRow As TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow
    Private oTmpOperacionesEPUDetBE As TmpOperacionesEPUDetBE
    Private oTmpOperacionesEPURow As TmpOperacionesEPUBE.TmpOperacionesEPURow
    Private oTmpOperacionesEPUBE As TmpOperacionesEPUBE
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim oOrdenInversion As New OrdenPreOrdenInversionBM
            Dim id As Integer
            Dim portafolio As String
            Dim tipoCambio As Decimal
            Dim codigoTercero As String
            Dim cont As Decimal
            If dgLista.Rows.Count > 0 Then
                For Each fila As DataGridItem In dgLista.Rows
                    If fila.ItemType = ListItemType.Item Or fila.ItemType = ListItemType.AlternatingItem Then
                        Dim hdCodigo As HiddenField
                        Dim ddlPortafolioF As DropDownList
                        Dim tbTipoCambio As TextBox
                        Dim hdIntermediario As HiddenField

                        hdCodigo = CType(fila.FindControl("hdCodigo"), HiddenField)
                        ddlPortafolioF = CType(fila.FindControl("ddlPortafolioF"), DropDownList)
                        tbTipoCambio = CType(fila.FindControl("tbTipoCambio"), TextBox)
                        hdIntermediario = CType(fila.FindControl("hdIntermediario"), HiddenField)

                        id = CType(hdCodigo.Value, Integer)
                        portafolio = ddlPortafolioF.SelectedValue
                        tipoCambio = Val(tbTipoCambio.Text)
                        codigoTercero = CType(hdIntermediario.Value, String)

                        If tipoCambio <> 0 And _
                         codigoTercero <> "" And _
                          portafolio <> "" Then
                            oOrdenInversion.ActualizarOperacionesEPUDet(id, portafolio, codigoTercero, tipoCambio, DatosRequest)
                            cont = cont + 1
                        End If
                    End If
                Next
                If cont = dgLista.Rows.Count Then
                    Dim he As New HelpExcel
                    oOrdenInversion.GenerarOperacionesEPU(DatosRequest)
                    dgLista.DataSource = Nothing
                    dgLista.DataBind()
                    dgLista.Visible = False
                    CargarGrillaOrdenes()
                    he.GenerarReporteOperacionesEPU(Usuario, DatosRequest)
                    AlertaJS("Proceso realizado satisfactoriamente!")
                Else
                    AlertaJS("Error al procesar, verifique que los datos sean correctos! ")
                End If
            Else
                AlertaJS("No hay datos para procesar!")
            End If

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarGrillaOrdenes()
        dgListaOrdenes.Visible = True
        dgListaOrdenes.DataSource = New OrdenPreOrdenInversionBM().SeleccionarPrevOperacionesEPU(DatosRequest).Tables(1)
        dgListaOrdenes.DataBind()
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub btnImportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")
            Dim portafolio As String = ddlPortafolio.SelectedValue
            ViewState("codigoPortafolioSBS") = portafolio
            If Not iptRuta.Value.ToString.Trim.Equals("") Then
                Dim oOrdenInversion As New OrdenPreOrdenInversionBM
                If CargarArchivoOrigen(strRuta) Then
                    Dim dsOperacionesEPU As DataSet = oOrdenInversion.SeleccionarPrevOperacionesEPU(DatosRequest)
                    If dsOperacionesEPU.Tables(0).Rows.Count > 0 Then
                        CargarPortafolio()
                        CargarGrilla(dsOperacionesEPU.Tables(0))
                        dgListaOrdenes.Visible = False
                        dgListaOrdenes.DataSource = Nothing
                        dgListaOrdenes.DataBind()
                        ViewState("codigoMoneda") = Nothing
                        ViewState("codigoOrden") = Nothing
                        ViewState("codigoNemonico") = Nothing
                        ViewState("portafolio") = Nothing
                        ViewState("codigoSBS") = Nothing
                        ViewState("codigoISIN") = Nothing
                    End If
                End If
            Else
                AlertaJS("Especifique la ruta de un archivo")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarPagina()
        'btnImportar.Attributes.Add("onclick", BusyBox1.ShowFunctionCall)
        'btnProcesar.Attributes.Add("onclick", Busybox2.ShowFunctionCall)
        Session("dtPortafolio") = Nothing
        CargarPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, Session("dtPortafolio"), "CodigoPortafolio", "Descripcion", True)
        ddlPortafolio.SelectedIndex = 1
    End Sub

    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        'Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        Dim dtPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        Session("dtPortafolio") = dtPortafolio
    End Sub

    Private Sub CargarGrilla(ByVal dtOperacionesEPU As DataTable)
        dgLista.Visible = True
        dgLista.DataSource = dtOperacionesEPU
        dgLista.DataBind()
    End Sub

    Private Function CargarArchivoOrigen(ByVal strRuta As String) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            'PRIMERO SE SUBE EL ARCHIVO
            If File.Exists(strRuta & "\" & fInfo.Name) Then File.Delete(strRuta & "\" & fInfo.Name)
            iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            If Dir(strRuta & "\" & fInfo.Name) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        bolResult = True
                        LeerExcel(strRuta & "\" & fInfo.Name)
                    Catch ex As Exception
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                    Else
                        File.Delete(strRuta & "\" & fInfo.Name)
                        AlertaJS("El tipo de archivo no es válido")
                    End If
                End If
            Else
                File.Delete(strRuta & "\" & fInfo.Name)
                AlertaJS("El archivo no existe")
            End If
            Return bolResult
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
        Return bolResult
    End Function

    Private Sub LeerExcel(ByVal strRuta As String)
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarArchivo(ByVal strRuta As String)
        Dim oDs As New DataSet
        Dim strMensaje As String = ""
        Try
            Dim he As New HelpExcel
            he.LeerExcelyRegistrarOperacionesEPU(strRuta, ViewState("codigoPortafolioSBS"), strMensaje, DatosRequest)
        Catch ex As Exception
            AlertaJS("Error al leer el archivo Excel")
        End Try
    End Sub

    Protected Sub dgListaOrdenes_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOrdenes.RowCommand
        If e.CommandName = "Select" Then
            Dim arguments() As String = CType(e.CommandArgument, String).Split(",")
            Dim codigoOrden As String = CType(arguments(0), String)
            Dim codigoNemonico As String = CType(arguments(1), String)
            Dim codigoISIN As String = CType(arguments(2), String)
            Dim codigoSBS As String = CType(arguments(3), String)
            Dim portafolio As String = CType(arguments(4), String)
            Dim codigoMoneda As String = CType(arguments(5), String)

            ViewState("codigoOrden") = codigoOrden
            ViewState("codigoNemonico") = codigoNemonico
            ViewState("codigoISIN") = codigoISIN
            ViewState("codigoSBS") = codigoSBS
            ViewState("portafolio") = portafolio
            ViewState("codigoMoneda") = codigoMoneda
        End If
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlPortafolioF As DropDownList
                Dim lbPortafolioF As Label
                Dim ibBIntermediario As ImageButton

                ddlPortafolioF = CType(e.Row.FindControl("ddlPortafolioF"), DropDownList)
                lbPortafolioF = CType(e.Row.FindControl("lbPortafolioF"), Label)
                ibBIntermediario = CType(e.Row.FindControl("ibBIntermediario"), ImageButton)

                ibBIntermediario.Attributes.Add("onclick", "javascript:ShowPopupTercerosGrilla(this);")
                HelpCombo.LlenarComboBox(ddlPortafolioF, CType(Session("dtPortafolio"), DataTable), "CodigoPortafolioSBS", "Descripcion", False)
                ddlPortafolioF.SelectedValue = lbPortafolioF.Text
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        If ViewState("codigoMoneda") Is Nothing And _
                ViewState("codigoOrden") Is Nothing And _
                ViewState("codigoNemonico") Is Nothing And _
                ViewState("portafolio") Is Nothing And _
                ViewState("codigoSBS") Is Nothing And _
                ViewState("codigoISIN") Is Nothing Then
            AlertaJS("Debe seleccionar un registro!")
        Else
            Dim descOperacion As String = CType(New OperacionBM().Seleccionar("2", DatosRequest).Tables(0).Rows(0)("Descripcion"), String)
            GenerarLlamado(ViewState("codigoOrden"), ViewState("portafolio"), "ACCIONES", descOperacion, ViewState("codigoMoneda"), ViewState("codigoISIN"), ViewState("codigoSBS"), ViewState("codigoNemonico"))
        End If
    End Sub

    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../../Inversiones/Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub

End Class
