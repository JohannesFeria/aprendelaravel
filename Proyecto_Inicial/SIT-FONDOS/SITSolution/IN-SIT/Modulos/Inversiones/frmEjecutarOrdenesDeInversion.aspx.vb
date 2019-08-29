Imports ParametrosSIT
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text

Partial Class Modulos_Inversiones_frmEjecutarOrdenesDeInversion
    Inherits BasePage
    Private campos() As String = {"Fondo", "Descripcion", "Moneda", "Monto"}
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String"}

#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then

                Dim FechaOperacion As Decimal
                Dim NroOrden As String
                Dim Portafolio As String
                If Not Request.QueryString("vFechaOperacion") Is Nothing Then
                    If Request.QueryString("vFechaOperacion").Length = 8 Then
                        FechaOperacion = Convert.ToDecimal(Request.QueryString("vFechaOperacion"))
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(FechaOperacion)
                    Else
                        FechaOperacion = UIUtility.ConvertirFechaaDecimal(Convert.ToString(Request.QueryString("vFechaOperacion")))
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(FechaOperacion))
                    End If
                Else
                    FechaOperacion = 0
                End If
                If Not Request.QueryString("vNroOrden") Is Nothing Then
                    NroOrden = Request.QueryString("vNroOrden")
                    Me.txtNroOrdenOPE.Text = NroOrden
                Else
                    NroOrden = ""
                End If
                If Not Request.QueryString("vPortafolio") Is Nothing Then
                    Portafolio = Request.QueryString("vPortafolio")
                    Me.ddlFondoOPE.SelectedValue = Portafolio
                Else
                    Portafolio = ""
                End If
                CargarGrillaOIEjecutadas(Portafolio, NroOrden, FechaOperacion)
                CargarGrillaOIPorEjecutar(Portafolio, NroOrden, FechaOperacion)
                ConsultarPaginasPorOI()
                CargarCombos()
                ViewState("Indica") = 0
                ViewState("Fondo") = ""
                ViewState("NroOrden") = ""
            Else
                If Not Session("Ejecutado") Is Nothing Then
                    If Session("Ejecutado") = 1 Then
                        CargarGrillaOIEjecutadas(ViewState("Fondo"), ViewState("NroOrden"), ViewState("Fecha"))
                        CargarGrillaOIPorEjecutar(ViewState("Fondo"), ViewState("NroOrden"), ViewState("Fecha"))
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la Página")
        End Try        
    End Sub

    Protected Sub dgListaOE_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOE.PageIndexChanging
        Try
            dgListaOE.SelectedIndex = -1
            dgListaOE.PageIndex = e.NewPageIndex

            Dim fondo As String = ViewState("Fondo")
            Dim nroOrden As String = ViewState("NroOrden")
            Dim fecha As String = ViewState("Fecha")

            CargarGrillaOIEjecutadas(fondo, nroOrden, fecha)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Protected Sub dgListaOPE_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOPE.PageIndexChanging
        Try
            dgListaOPE.SelectedIndex = -1
            dgListaOPE.PageIndex = e.NewPageIndex

            Dim fondo As String = ViewState("Fondo")
            Dim nroOrden As String = ViewState("NroOrden")
            Dim fecha As String = ViewState("Fecha")

            CargarGrillaOIPorEjecutar(fondo, nroOrden, fecha)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgListaOE_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOE.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                dgListaOE.SelectedIndex = Row.RowIndex
                dgListaOPE.SelectedIndex = -1
                Dim i As Integer = Row.RowIndex
                Me.lbCodigoISIN.Text = HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(9).Text)
                Me.lbTipoOrden.Text = HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(10).Text)
                Me.lbNroTransaccion.Text = HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(3).Text)
                Me.lbTipoOperacion.Text = HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(4).Text)
                lbParametros.Text = ""
                Me.btnEjecutar.Attributes.Remove("onClick")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgListaOE.PageIndex = 0
            dgListaOPE.PageIndex = 0
            Dim arrFecha() As String
            Dim dato As DateTime
            dato = Convert.ToDateTime(Me.tbFechaOperacion.Text.Trim())
            arrFecha = Me.tbFechaOperacion.Text.Trim().Split("/")

            Dim nroOrden As String = txtNroOrdenOPE.Text.Trim
            Dim fondo As String = IIf(ddlFondoOPE.SelectedIndex = 0, "", ddlFondoOPE.SelectedValue)
            Dim fecha As String = arrFecha(2) + arrFecha(1) + arrFecha(0)

            ViewState("Fondo") = fondo
            ViewState("NroOrden") = nroOrden
            ViewState("Fecha") = fecha

            CargarGrillaOIPorEjecutar(fondo, nroOrden, fecha)
            CargarGrillaOIEjecutadas(fondo, nroOrden, fecha)

            Session.Remove("Ejecutado")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnEjecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEjecutar.Click
        Try
            Dim oOrdenInversionWorkFlowBM As OrdenInversionWorkFlowBM
            Dim iNumber As Integer = NumeroOrdenesSeleccionadas()
            Dim oOrdenInversionBE As OrdenPreOrdenInversionBE
            Dim oOrdenInversionBM As OrdenPreOrdenInversionBM
            Dim oOIFormulas As OrdenInversionFormulasBM
            If iNumber > 0 Then
                If iNumber = 1 Then
                    If lbParametros.Text.ToString <> "" Then
                        Session("Ejecutado") = 1
                        Ir("EO", lbNroTransaccion.Text, lbParametros.Text.Split(",").GetValue(0), lbParametros.Text.Split(",").GetValue(1))
                        Me.btnEjecutar.Attributes.Remove("onClick")
                    Else
                        AlertaJS("Debe seleccionar un Registro")
                    End If
                Else
                    For iCont As Integer = 0 To dgListaOPE.Rows.Count - 1
                        If dgListaOPE.Rows(iCont).FindControl("chkSelect").GetType Is GetType(CheckBox) Then
                            Dim chk As CheckBox = CType(dgListaOPE.Rows(iCont).FindControl("chkSelect"), CheckBox)
                            If chk.Checked = True Then
                                Dim CategoriaInstrumento As String = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(14).Text)
                                Dim CodigoOrden As String = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(5).Text)
                                Dim Fondo As String = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(4).Text)
                                Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
                                Dim ValorCustodio As String = ""
                                Dim objContacto As ContactoBM
                                Dim hdPagina As String = "EO"
                                Dim hdSaldo As Decimal = 0
                                Dim NPoliza As String = ""
                                Dim dsValor As New DataSet
                                Dim drValor As DataRow
                                Dim dtAux As DataTable
                                Try
                                    oOrdenInversionWorkFlowBM = New OrdenInversionWorkFlowBM
                                    oOrdenInversionBM = New OrdenPreOrdenInversionBM
                                    oOrdenInversionBE = New OrdenPreOrdenInversionBE
                                    oOIFormulas = New OrdenInversionFormulasBM
                                    objContacto = New ContactoBM
                                    'Obtiene ordenes de inversión
                                    oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Fondo, DatosRequest, PORTAFOLIO_MULTIFONDOS)
                                    'Primera fila de OI
                                    oRow = oOrdenInversionBE.Tables(0).Rows(0)
                                    'Obtiene caracteristicas valor
                                    dsValor = oOIFormulas.SeleccionarCaracValor_Acciones(Fondo, oRow.CodigoMnemonico, DatosRequest)
                                    If dsValor.Tables(0).Rows.Count > 0 Then
                                        drValor = dsValor.Tables(0).Rows(0)
                                        'Datos de tercero
                                        dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
                                        If dtAux.Rows.Count > 0 Then
                                            'Categoria instrumento
                                            oOrdenInversionBE.Tables(0).Rows(0).Item("CategoriaInstrumento") = CategoriaInstrumento
                                            'Indica cambio
                                            oOrdenInversionBE.Tables(0).Rows(0).Item("IndicaCambio") = "1"
                                            'Situacíón
                                            oOrdenInversionBE.Tables(0).Rows(0).Item("Situacion") = "A"
                                            'Carga impuestos de comisiones
                                            UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, CodigoOrden, Fondo)
                                            'Custodio
                                            ValorCustodio = UIUtility.ObtieneCustodiosOI(CodigoOrden, Fondo, DatosRequest, dtAux.Rows(0)("CodigoCustodio"), hdSaldo)
                                            '*-------------------------Ejecutar Operación------------------------------------------------------------------*'                                    
                                            oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina, ValorCustodio, DatosRequest)
                                            NPoliza = oRow.NumeroPoliza.ToString()
                                            oOrdenInversionWorkFlowBM.EjecutarOI(CodigoOrden, Fondo, NPoliza, DatosRequest)
                                            Me.btnEjecutar.Attributes.Remove("onClick")
                                            '*-------------------------------------------------------------------------------------------------------------*'
                                        Else
                                            AlertaJS(ObtenerMensaje("CONF29"))
                                        End If
                                    Else
                                        AlertaJS("No se encontro caracteristicas valor con el fondo: " & Fondo)
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                Finally
                                    oOrdenInversionWorkFlowBM = Nothing
                                    oOrdenInversionBM = Nothing
                                    oOrdenInversionBE = Nothing
                                    oOIFormulas = Nothing
                                    objContacto = Nothing
                                    dsValor = Nothing
                                    GC.Collect()
                                End Try
                            End If
                        End If
                    Next iCont
                    btnBuscar_Click(sender, e)
                    AlertaJS("Se ejecutarón las ordenes correctamente...!")


                End If
            Else
                AlertaJS("Debe seleccionar un Registro...!")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

    Private Sub ddlFondoOPE_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondoOPE.SelectedIndexChanged
        Try
            If (Me.ddlFondoOPE.SelectedIndex <> 0) Then
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(Me.ddlFondoOPE.SelectedValue.ToString))
            ElseIf (Me.ddlFondoOPE.SelectedIndex <> 0) Then
                Me.tbFechaOperacion.Text = ""
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try       
    End Sub

    Public Sub dgListaOPE_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)        
        Try
            Dim chkTemp As CheckBox = CType(sender, CheckBox)
            Dim dgi As GridViewRow
            dgi = CType(chkTemp.Parent.Parent, GridViewRow)
            If chkTemp.Checked = True Then
                'Obtiene fila de Grilla
                Dim i As Integer = dgi.RowIndex
                Me.lbCodigoISIN.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(i).Cells(12).Text)
                Me.lbTipoOrden.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(i).Cells(13).Text)
                Me.lbNroTransaccion.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(i).Cells(5).Text)
                Me.lbTipoOperacion.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(i).Cells(6).Text)
                Me.lbParametros.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(i).Cells(4).Text) & "," & HttpUtility.HtmlDecode(dgListaOPE.Rows(i).Cells(14).Text)
                Me.btnEjecutar.Attributes.Add("onClick", "javascript:return Ejecutar();")
            Else
                Dim bSeleccionadas As Boolean = False, iCont As Int64 = dgListaOPE.Rows.Count - 1
                While iCont >= 0
                    If dgListaOPE.Rows(iCont).FindControl("chkSelect").GetType Is GetType(CheckBox) Then
                        Dim chk As CheckBox = CType(dgListaOPE.Rows(iCont).FindControl("chkSelect"), CheckBox)
                        If chk.Checked = True Then
                            bSeleccionadas = True
                            Exit While
                        End If
                    End If
                    iCont = iCont - 1
                End While
                If bSeleccionadas = True Then
                    Me.lbCodigoISIN.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(12).Text)
                    Me.lbTipoOrden.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(13).Text)
                    Me.lbNroTransaccion.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(5).Text)
                    Me.lbTipoOperacion.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(6).Text)
                    Me.lbParametros.Text = HttpUtility.HtmlDecode(dgListaOPE.Rows(iCont).Cells(4).Text & "," & dgListaOPE.Rows(iCont).Cells(14).Text)
                    Me.btnEjecutar.Attributes.Add("onClick", "javascript:return Ejecutar();")
                Else
                    Me.lbNroTransaccion.Text = ""
                    Me.lbTipoOperacion.Text = ""
                    Me.lbParametros.Text = ""
                    Me.lbCodigoISIN.Text = ""
                    Me.lbTipoOrden.Text = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
#End Region
#Region " /* Métodos Personalizados */ "
    Public Sub CargarCombos()
        Dim tablaClaseInstrumento As New DataTable
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim oPortafolioBM As New PortafolioBM
        tablaClaseInstrumento = oClaseInstrumentoBM.Listar(Me.DatosRequest).Tables(0)
        ddlFondoOPE.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondoOPE.DataValueField = "CodigoPortafolio"
        ddlFondoOPE.DataTextField = "Descripcion"
        ddlFondoOPE.DataBind()
        ddlFondoOPE.Items.Insert(0, New ListItem("--Seleccione--", ""))
    End Sub

    Private Sub CargarGrillaOIEjecutadas(ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As Decimal)
        Dim tablaOIEjecutadas As New Data.DataTable
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        tablaOIEjecutadas = oOrdenesInversionBM.ListarOIEjecutadas(DatosRequest, fondo, nroOrden, fecha)
        ViewState("OIEjecutadas") = tablaOIEjecutadas
        dgListaOE.DataSource = tablaOIEjecutadas
        dgListaOE.DataBind()
    End Sub

    Private Sub CargarGrillaOIPorEjecutar(ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As String)
        Dim tablaOIPorEjecutar As New Data.DataTable
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        tablaOIPorEjecutar = oOrdenesInversionBM.ListarOIPorEjecutar(DatosRequest, fondo, nroOrden, fecha)
        ViewState("OIPorEjecutar") = tablaOIPorEjecutar
        dgListaOPE.DataSource = tablaOIPorEjecutar
        dgListaOPE.DataBind()
    End Sub
    Private Sub Ir(ByVal ejecucion As String, ByVal orden As String, ByVal fondo As String, ByVal clasificacion As String)
        Dim StrURL As String
        StrURL = Pagina(clasificacion).Replace("OI1", ejecucion).Replace("NO1", orden).Replace("FO1", fondo).Replace("CL1", clasificacion).Replace("%", "&")
        ShowDialogPopup(StrURL)
    End Sub
    Private Sub ShowDialogPopup(ByVal StrURL As String)
        Dim FechaOperacion As String
        Dim NroOrden As String
        Dim Portafolio As String
        FechaOperacion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text).ToString
        NroOrden = Me.txtNroOrdenOPE.Text
        If (Me.ddlFondoOPE.SelectedIndex = 0) Then
            Portafolio = ""
        Else
            Portafolio = Me.ddlFondoOPE.SelectedValue.ToString
        End If

        EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '');")
        EjecutarJS("window.location.href = 'frmEjecutarOrdenesDeInversion.aspx?vFechaOperacion='" & FechaOperacion & "'&vNroOrden='" & NroOrden & "'&vPortafolio='" & Portafolio & ";")

        'Dim script As New StringBuilder
        'With script
        '    .Append("<script>")
        '    .Append("function PopupBuscador(FechaOperacion, NroOrden, Portafolio)")
        '    .Append("{")
        '    .Append("   var argument = new Object(FechaOperacion, NroOrden, Portafolio);")
        '    .Append("   window.showModalDialog('" + StrURL + "', argument, 'dialogHeight:600px; dialogWidth:950px; dialogLeft:125px;');")
        '    .Append("   window.location.href = 'frmEjecutarOrdenesDeInversion.aspx?vFechaOperacion='+FechaOperacion+'&vNroOrden='+NroOrden+'&vPortafolio='+Portafolio;")
        '    .Append("}")
        '    .Append("PopupBuscador('" + FechaOperacion + "','" + NroOrden + "','" + Portafolio + "');")
        '    .Append("</script>")
        'End With
        'EjecutarJS(script.ToString(), False)
    End Sub

    Private Function Pagina(ByVal clasificacion As String) As String

        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)

    End Function

    Private Function ConsultarPaginasPorOI() As Boolean

        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\Configuracion\TEjecucionOI.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Function NumeroOrdenesSeleccionadas() As Integer
        Dim iNumber As Integer = 0
        Try
            For iCont As Integer = 0 To dgListaOPE.Rows.Count - 1
                If dgListaOPE.Rows(iCont).FindControl("chkSelect").GetType Is GetType(CheckBox) Then
                    Dim chk As CheckBox = CType(dgListaOPE.Rows(iCont).FindControl("chkSelect"), CheckBox)
                    If chk.Checked = True Then
                        iNumber = iNumber + 1
                    End If
                End If
            Next iCont
        Catch ex As Exception
            Throw ex
        End Try
        Return iNumber
    End Function

#End Region
End Class
