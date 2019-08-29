Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_frmTraspasoInstrumentos
    Inherits BasePage
#Region "Rutinas"
    Private Sub CargarMoneda()
        Dim oValoresBM As New ValoresBM
        Dim dsValorBono As New DataSet
        Dim drValorBono As DataRow
        dsValorBono = oValoresBM.SeleccionarBono(Me.tbMnemonico.Text, datosrequest)
        If dsValorBono.Tables(0).Rows.Count > 0 Then
            drValorBono = dsValorBono.Tables(0).NewRow
            drValorBono = dsValorBono.Tables(0).Rows(0)
            Me.lblMoneda.Text = CType(drValorBono("Moneda"), String)
        End If
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String, ByVal categoria As String)
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("function PopupBuscador(isin, mnemonico, sbs, fondo, operacion, categoria)")
            .Append("{")
            .Append("   var argument = new Object();")
            .Append("   argument.ISIN = '';")
            .Append("   argument.MNEMONICO = '';")
            .Append("   argument.SBS = '';")
            .Append("   window.showModalDialog('InstrumentosNegociados/frmBuscarValor.aspx?vISIN=' + isin + '&vSBS=' + sbs + '&vMnemonico=' + mnemonico + '&vFondo=' + fondo + '&vOperacion=' + operacion + '&vCategoria=' + categoria , argument, 'dialogHeight:530px; dialogWidth:1180px; dialogLeft:150px;'); document.getElementById('btnpopup').click();")
            .Append("   document.getElementById('" + tbISIN.ClientID + "').innerText = argument.ISIN;")
            .Append("   document.getElementById('" + tbMnemonico.ClientID + "').innerText = argument.MNEMONICO;")
            .Append("   document.getElementById('" + tbSBS.ClientID + "').innerText = argument.SBS;")
            .Append("   document.getElementById('" + hdCustodio.ClientID + "').value = argument.CUSTODIO;")
            .Append("   document.getElementById('" + hdSaldo.ClientID + "').value = argument.SALDO;")
            .Append("   return false;")
            .Append("}")
            .Append("PopupBuscador('" + isin + "','" + mnemonico + "','" + sbs + "','" + fondo + "','" + operacion + "','" + categoria + "');")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString(), False)
    End Sub
    Private Function Pagina(ByVal clasificacion As String) As String
        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)
    End Function
    Private Sub Ir(ByVal clasificacion As String, ByVal StrMnemonico As String, ByVal StrOperacion As String, ByVal StrFondoO As String, ByVal StrFondoD As String, ByVal StrISIN As String, ByVal StrSBS As String, ByVal StrMoneda As String, ByVal custodio As String, ByVal saldo As String)
        Dim strUrl As String = Pagina(clasificacion).Replace("@", StrMnemonico).Replace("%", "&").Replace("¿", StrOperacion).Replace("#", StrFondoO).Replace("$", StrFondoD).Replace("+", StrISIN).Replace("SBS1", StrSBS).Replace("MN!", StrMoneda).Replace("XC", custodio).Replace("XS", saldo)
        strUrl = strUrl + "&fechaOperacion=" + UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text).ToString()
        Response.Redirect(strUrl, False)
    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oValoresBM As New ValoresBM
        Dim oValoresBE As New DataTable
        oValoresBE = oValoresBM.SeleccionarInstrumento(String.Empty, Me.tbMnemonico.Text.Trim(), Me.DatosRequest).Tables(0)
        Return oValoresBE.Rows.Count > 0
    End Function
    Public Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim tablaClaseInstrumento As New DataTable
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim oPortafolioBM As New PortafolioBM

        tablaClaseInstrumento = oClaseInstrumentoBM.Listar(Me.DatosRequest).Tables(0)
        tablaPortafolio = oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0)
        UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
        HelpCombo.LlenarComboBox(Me.ddlTipoOrden, tablaClaseInstrumento, "Categoria", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlFondoOrigen, tablaPortafolio, "CodigoPortafolioSBS", "Descripcion", True)
        HelpCombo.LlenarComboBox(Me.ddlFondoDestino, tablaPortafolio, "CodigoPortafolioSBS", "Descripcion", True)
    End Sub
    Private Sub ConsultarPaginasPorOI()
        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\Configuracion\TTraspasoI.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.ibAceptar.Attributes.Add("onclick", "javascript:return Validar();")
        If Not Page.IsPostBack = True Then
            Session("Busqueda") = 0
            Try
                CargarCombos()
                ConsultarPaginasPorOI()
            Catch ex As Exception
                AlertaJS(ex.Message)
            End Try
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            tbMnemonico.Text = CType(Session("SS_DatosModal"), String())(0)
            Session.Remove("SS_DatosModal")
        End If
    End Sub
    Protected Sub ddlFondoDestino_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFondoDestino.SelectedIndexChanged
        If ddlFondoDestino.SelectedIndex <> -1 And ddlFondoDestino.SelectedIndex <> 0 Then
            txtFechaPortafolioDestino.Value = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondoDestino.SelectedValue))
        End If
    End Sub
    Protected Sub ddlFondoOrigen_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFondoOrigen.SelectedIndexChanged
        If ddlFondoOrigen.SelectedIndex <> -1 And ddlFondoOrigen.SelectedIndex <> 0 Then
            txtFechaPortafolioOrigen.Value = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondoOrigen.SelectedValue))
            Me.tbFechaOperacion.Text = txtFechaPortafolioOrigen.Value
        End If
    End Sub
    Protected Sub ibAceptar_Click(sender As Object, e As System.EventArgs) Handles ibAceptar.Click
        If Me.ddlFondoOrigen.SelectedValue = Me.ddlFondoDestino.SelectedValue Then
            AlertaJS("El Portafolio Origen no debe ser igual al Portafolio Destino")
            Exit Sub
        End If
        Dim blnExisteItem As Boolean
        If Me.tbISIN.Text.Trim = "" Or Me.tbMnemonico.Text.Trim = "" Or Me.tbSBS.Text.Trim = "" Then
            AlertaJS("Debe ingresar la Orden de Inversion")
            Exit Sub
        End If
        Try
            Dim StrId As String = String.Empty
            blnExisteItem = ExisteEntidad()
            If Not blnExisteItem Then
                AlertaJS("No existe el Instrumento")
            Else
                Ir(Me.ddlTipoOrden.SelectedValue, Me.tbMnemonico.Text.Trim, Me.ddlOperacion.SelectedValue, Me.ddlFondoOrigen.SelectedValue, Me.ddlFondoDestino.SelectedValue, Me.tbISIN.Text, Me.tbSBS.Text, Me.lblMoneda.Text, Me.hdCustodio.Value, Me.hdSaldo.Value)
                Dim oValoresBM As New ValoresBM
                Session("MonedaOrigen") = oValoresBM.Seleccionar(Me.tbMnemonico.Text, DatosRequest).Valor.Item(0).CodigoMoneda.ToString
            End If
        Catch ex As Exception
            'Las excepciones deben ser enviadas a la clase base con el método ManejarError,esta clase se encarga de mostrar los mensajes correspondientes
            AlertaJS(ex.Message)
        End Try
    End Sub
    
    Protected Sub ibSalir_Click(sender As Object, e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

    Protected Sub ibBuscar_Click(sender As Object, e As System.EventArgs) Handles ibBuscar.Click
        Dim strCodigoOperacion As String = String.Empty
        Dim strClaseInstrumento As String = String.Empty
        If Me.ddlTipoOrden.SelectedValue = "" Then
            AlertaJS("Debe Seleccionar el Tipo de Orden")
        Else
            'Seleccionar Clase Insrumento a Negociar
            '----------------------------------------------------------------
            strClaseInstrumento = ddlTipoOrden.SelectedValue
            'Seleccionar Combo para Operacion de Traspaso
            '-----------------------------------------------------------------
            ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionPorTraspaso(Constantes.M_TRASPASO_EGRESO)
            'Seleccionar Codigo Operacion para Búsqeuda de instrumentos
            '-----------------------------------------------------------------
            Select Case strClaseInstrumento
                Case "BO", "FI", "LH", "FM", "FD", "IE", "CS", "CD", "OR", "PA", "AC", "CV", "IC"
                    strCodigoOperacion = "1"
                    '1. Todas Las Ordenes (Excepto Depositos a Plazos)
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoOperacionTIngreso() Then
                        strCodigoOperacion = UIUtility.ObtenerCodigoOperacionCompra()
                    ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoOperacionTEgreso() Then
                        strCodigoOperacion = UIUtility.ObtenerCodigoOperacionVenta()
                    End If
                Case "DP"
                    '2. Ordenes de Depósitos a Plazos
                    strCodigoOperacion = ""
            End Select
            ShowDialogPopupValores(tbISIN.Text.Trim.ToUpper, tbSBS.Text.Trim.ToUpper, tbMnemonico.Text.Trim.ToUpper, ddlFondoOrigen.SelectedValue, strCodigoOperacion, ddlTipoOrden.SelectedValue)
            Session("Busqueda") = 2
        End If
    End Sub
End Class