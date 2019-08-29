Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmConsultaMovimientosBancarios
    Inherits BasePage
#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarMercado(True)
                CargarBanco(ddlMercado.SelectedValue, True)
                CargarPortafolio(True)
                CargarClaseCuenta(True)
                CargarDetallePortafolio("", ddlMercado.SelectedValue, "", "", ddlPortafolio.SelectedValue)
                CargarOperacion(True)
                CargarTipoOperacion()
                CargarMoneda(True)
                tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                tbFechaFin.Text = tbFechaInicio.Text
                divExtornoOperacionesCaja.Visible = False
                CargarMotivo(True)
                VerificarConsulta()
            End If
            btnExtornar.Attributes.Add("onclick", "return ValidarOperaciones()&&confirm('" & New UtilDM().RetornarMensajeConfirmacion("CONF19") & "');")
            btnSaldos.Attributes.Add("onclick", "return ValidarOperaciones();")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub VerificarConsulta()
        If Not Request.QueryString("nroCuenta") Is Nothing Then
            Dim sCodigoMoneda As String = ""
            Dim sCodigoBanco As String = ""
            Dim codMercado As String = Request.QueryString("codMercado")
            Dim codPortafolio As String = Request.QueryString("codPortafolio")
            Dim nroCuenta As String = Request.QueryString("nroCuenta")
            Dim fecha As String = Request.QueryString("fecha")
            Dim clase As String = Request.QueryString("ClaseCuenta")
            CargarDetallePortafolio("", codMercado, clase, "", codPortafolio, True)
            ObtenerMoneda(nroCuenta, codPortafolio, sCodigoBanco, sCodigoMoneda)
            If Not ddlMoneda.Items.FindByValue(sCodigoMoneda) Is Nothing Then
                ddlMoneda.SelectedValue = sCodigoMoneda
            End If
            If Not ddlBanco.Items.FindByValue(sCodigoBanco) Is Nothing Then
                ddlBanco.SelectedValue = sCodigoBanco
            End If
            ddlMercado.SelectedValue = codMercado
            ddlPortafolio.SelectedValue = codPortafolio
            If ddlNroCuenta.Items.Count > 0 Then
                ddlNroCuenta.SelectedValue = codPortafolio.Trim & "," & nroCuenta.Trim
            End If
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(Decimal.Parse(fecha))
            tbFechaFin.Text = UIUtility.ConvertirFechaaString(Decimal.Parse(fecha))
            ddlClaseCuenta.SelectedValue = clase
            hdCodigoOperacionCaja.Value = ""
            btnSalir.Attributes.Add("onclick", "return Retornar();")
            btnSalir.Text = "Retornar"
            btnSaldos.Visible = False
            CargarGrilla()
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        CargarBanco(ddlMercado.SelectedValue, True)
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlMercado.SelectedValue, ddlClaseCuenta.SelectedValue, ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlMercado.SelectedValue, ddlClaseCuenta.SelectedValue, ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
        EstablecerFecha()
    End Sub
    Private Sub EstablecerFecha()
        If (ddlPortafolio.SelectedIndex <= 0) Then
            Return
        End If
        Me.tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        hdFechaNegocio.Value = tbFechaInicio.Text
    End Sub
    Private Sub ddlClaseCuenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClaseCuenta.SelectedIndexChanged
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlMercado.SelectedValue, ddlClaseCuenta.SelectedValue, ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
        CargarOperacion(True)
    End Sub
    Sub CargaMonedaBanco()
        If Not ddlBanco.SelectedValue = "" Then
            Dim oOperaCaja As New OperacionesCajaBM
            HelpCombo.LlenarComboBox(ddlMoneda, oOperaCaja.Listar_MonedaBanco(ddlPortafolio.SelectedValue, ddlBanco.SelectedValue), "CodigoMoneda", "Descripcion", True)
        End If
    End Sub
    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlMercado.SelectedValue, ddlClaseCuenta.SelectedValue, ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
        CargaMonedaBanco()
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        Me.CargarClaseCuenta(True)
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlMercado.SelectedValue, ddlClaseCuenta.SelectedValue, ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text) Then
            AlertaJS(ObtenerMensaje("ALERT48"))
            Exit Sub
        End If
        CargarGrilla()
        hdCodigoOperacionCaja.Value = ""
    End Sub
    Private Function OperacionCajaSeleccionarPorFiltro(ByVal dsOperacionCaja As OperacionCajaBE, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet
        Return New OperacionesCajaBM().SeleccionarPorFiltro(dsOperacionCaja, fechaInicio, fechaFin, DatosRequest)
    End Function
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
    Private Sub btnExtornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtornar.Click
        If hdCodigoOperacionCaja.Value = "" Then
            AlertaJS("Seleccione un movimiento")
        Else
            Try
                HabilitaControles(False)
            Catch ex As Exception
                AlertaJS(Replace(ex.Message, "'", ""))
            End Try
        End If
    End Sub
    Private Sub btnSaldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaldos.Click
        If Not MostrarSaldos() Then
            AlertaJS(ObtenerMensaje("ALERT137"))
            Exit Sub
        End If
    End Sub
#End Region
#Region "Cargar Datos"
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            HelpCombo.LlenarComboBox(ddlMercado, dsMercado.Tables(0), "CodigoMercado", "Descripcion", True)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            ddlPortafolio.DataSource = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarTipoOperacion()
        Dim obj As New TipoOperacionBM
        Dim ds As DataSet = obj.Listar("A")
        HelpCombo.LlenarComboBox(ddlTipoOperacion, ds.Tables(0), "CodigoTipoOperacion", "Descripcion", False)
        UIUtility.InsertarElementoSeleccion(ddlTipoOperacion, "")
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dt As DataTable = oClaseCuenta.Listar().Tables(0)
            HelpCombo.LlenarComboBox(ddlClaseCuenta, dt, "CodigoClaseCuenta", "Descripcion", True, "SELECCIONE")
            ddlClaseCuenta.SelectedValue = "20"
        Else
            ddlClaseCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        End If
        ddlClaseCuenta.Enabled = enabled
    End Sub
    Private Function Distinct(ByVal dt As DataTable, ByVal columName As String) As DataTable
        Dim dr As DataRow
        Dim value As String
        Dim dtResult As DataTable
        dtResult = dt.Clone()
        If (dt.Rows.Count > 0) Then
            value = Convert.ToString(dt.Rows(0)(columName))
            dtResult.LoadDataRow(dt.Rows(0).ItemArray(), True)
            For Each dr In dt.Rows
                If String.Equals(value, Convert.ToString(dr(columName))) = False Then
                    value = Convert.ToString(dr(columName))
                    dtResult.LoadDataRow(dr.ItemArray(), True)
                End If
            Next
        End If
        Return dtResult
    End Function
    Private Sub CargarBanco(ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(codMercado, ddlPortafolio.SelectedValue, "")
            Dim dtBancos As DataTable = Distinct(dsBanco.Tables(0), "Descripcion")
            Me.Session().Add("Bancos", dtBancos)
            HelpCombo.LlenarComboBox(ddlBanco, dtBancos, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsOperacion As DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion(ddlTipoOperacion.SelectedValue, "", ddlClaseCuenta.SelectedValue)
            HelpCombo.LlenarComboBox(ddlOperacion, dsOperacion.Tables(0), "CodigoOperacion", "Descripcion", True, "SELECCIONE")
        Else
            ddlOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        End If
        ddlOperacion.Enabled = enabled
    End Sub
    Private Sub CargarDetallePortafolio(ByVal codTercero As String, ByVal codMercado As String, ByVal codClaseCuenta As String, ByVal codMoneda As String, ByVal codPortafolio As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dsCuentaEconomica As DataSet = oCuentaEconomica.SeleccionarPorFiltro(codPortafolio, codClaseCuenta, codTercero, codMoneda, codMercado, DatosRequest)
            ddlNroCuenta.Items.Clear()
            ddlNroCuenta.DataSource = dsCuentaEconomica.Tables(1)
            ddlNroCuenta.DataValueField = "CodigoCuenta"
            ddlNroCuenta.DataTextField = "NumeroCuenta"
            ddlNroCuenta.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        ddlNroCuenta.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub CargarMotivo(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim dtMotivo As DataTable = New ParametrosGeneralesBM().Listar(ParametrosSIT.MOTIVO_EXTORNO, DatosRequest)
            HelpCombo.LlenarComboBox(ddlMotivo, dtMotivo, "Valor", "Nombre", False)
        Else
            ddlMotivo.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMotivo)
        End If
        ddlMotivo.Enabled = enabled
    End Sub
    Private Sub CargarGrilla()
        Dim dsOpCaja As New OperacionCajaBE
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
        Dim fechaIni As Decimal = 0
        Dim fechaFin As Decimal = 0
        If tbFechaInicio.Text <> "" Then
            fechaIni = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        End If
        If tbFechaFin.Text <> "" Then
            fechaFin = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
        End If
        opCaja.CodigoMercado = ddlMercado.SelectedValue
        opCaja.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        opCaja.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
        opCaja.CodigoMoneda = ddlMoneda.SelectedValue
        opCaja.CodigoTerceroOrigen = ddlBanco.SelectedValue
        opCaja.NumeroCuenta = ddlNroCuenta.SelectedValue
        opCaja.CodigoOperacion = ddlOperacion.SelectedValue
        opCaja.CodigoTipoOperacion = ddlTipoOperacion.SelectedValue
        opCaja.CodigoOperacionCaja = ""
        dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
        dgLista.SelectedIndex = -1
        Dim operacionesCaja As New OperacionesCajaBM
        Dim ds As New DataSet
        ds = operacionesCaja.SeleccionarPorFiltro(dsOpCaja, fechaIni, fechaFin, DatosRequest)
        ds.Tables(0).DefaultView.Sort = "TipoOperacion DESC"
        dgLista.DataSource = ds.Tables(0).DefaultView
        dgLista.DataBind()
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(ds.Tables(0).Rows.Count) + "')")
    End Sub
#End Region
#Region "Metodos de Control"
    Private Sub ValidarLista(ByVal ddl As DropDownList, ByVal lista As String)
        Dim i As Integer = ddl.Items.Count - 1
        While (i >= 0)
            If lista.IndexOf(ddl.Items(i).Value) = -1 Then
                ddl.Items.RemoveAt(i)
            End If
            i = i - 1
        End While
    End Sub
    Private Function MostrarSaldos() As Boolean
        Dim sCodigoPortafolio As String = ddlPortafolio.SelectedValue
        Dim sNumeroCuenta As String = ddlNroCuenta.SelectedItem.Text.Trim
        Dim s_namePortafolio As String = ddlPortafolio.SelectedItem.Text
        For Each row As GridViewRow In dgLista.Rows
            If row.Cells(1).Text = hdCodigoOperacionCaja.Value Then
                sCodigoPortafolio = row.Cells(5).Text
                sNumeroCuenta = row.Cells(13).Text
                s_namePortafolio = row.Cells(6).Text
                Exit For
            End If
        Next
        Dim strMensaje As String
        If sCodigoPortafolio.Trim() = "" Then
            AlertaJS(ObtenerMensaje("ALERT134"))
            Return False
            Exit Function
        ElseIf sNumeroCuenta.Trim() = "" Then
            AlertaJS(ObtenerMensaje("ALERT135"))
            Return False
            Exit Function
        End If
        Dim oSaldoBancario As New SaldosBancariosBM
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        roCuentaEconomica.CodigoPortafolioSBS = sCodigoPortafolio
        roCuentaEconomica.NumeroCuenta = IIf(sNumeroCuenta = "--SELECCIONE--", "", sNumeroCuenta)
        roCuentaEconomica.CodigoMoneda = ""
        roCuentaEconomica.CodigoClaseCuenta = ""
        roCuentaEconomica.CodigoTercero = ""
        roCuentaEconomica.FechaCreacion = 0
        roCuentaEconomica.CodigoMercado = ""
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim oSBDS As DataSet = oSaldoBancario.SeleccionarPorFiltro(dsCuentaEconomica, DatosRequest)
        If oSBDS.Tables(0).Rows.Count <= 0 Then
            Return False
            Exit Function
        End If
        If oSBDS.Tables(0).Rows.Count > 0 Then
            Dim sSaldoContable As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoContableInicial")
            Dim sSaldoDisponible As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoDisponibleInicial")
            Dim sClaseCuenta As String = oSBDS.Tables(0).Rows(0).Item("ClaseDescripcion")
            Dim sMoneda As String = oSBDS.Tables(0).Rows(0).Item("DescripcionMoneda")
            strMensaje = "Portafolio:\t" & s_namePortafolio & " \n"
            strMensaje = strMensaje & "Cuenta:\t\t" & sNumeroCuenta & "\n"
            strMensaje = strMensaje & "_______________________________________" & " \n" & " \n"
            strMensaje = strMensaje & "Saldo Contable:\t" & String.Format("{0:#,##0.0000000}", sSaldoContable) & " \n"
            strMensaje = strMensaje & "Saldo Disponible:\t" & String.Format("{0:#,##0.0000000}", sSaldoDisponible) & " \n"
            strMensaje = strMensaje & "Clase Cuenta:\t" & sClaseCuenta & " \n"
            strMensaje = strMensaje & "Moneda:\t\t" & sMoneda & " \n" & " \n"
            strMensaje = strMensaje & "___________________º___________________" & " \n" & " \n"
            Call AlertaJS(strMensaje)
        End If
        Return True
    End Function
    Private Sub ObtenerMoneda(ByVal numeroCuenta As String, ByVal codigoPortafolio As String, ByRef banco As String, ByRef moneda As String)
        Dim oCuentaEconomica As New CuentaEconomicaBM
        Dim row As DataRow = oCuentaEconomica.SeleccionarPorFiltro(codigoPortafolio, "", "", "", "", DatosRequest).Tables(1).Select("NumeroCuenta='" & numeroCuenta & "'")(0)
        moneda = row("CodigoMoneda")
        banco = row("CodigoTercero")
    End Sub
    Private Sub HabilitaControles(ByVal visible As Boolean)
        btnBuscar.Visible = visible
        btnSaldos.Visible = visible
        btnImprimir.Visible = visible
        tbComentario.Text = ""
        If visible = True Then
            dgLista.Enabled = True
            divExtornoOperacionesCaja.Visible = False
        Else
            dgLista.Enabled = False
            divExtornoOperacionesCaja.Visible = True
        End If
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim tablaParametros As New Hashtable
        tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
        tablaParametros("codMercado") = ddlMercado.SelectedValue
        tablaParametros("codMoneda") = ddlMoneda.SelectedValue
        tablaParametros("codOperacion") = ddlOperacion.SelectedValue
        tablaParametros("codBanco") = ddlBanco.SelectedValue
        tablaParametros("codClaseCuenta") = ddlClaseCuenta.SelectedValue
        tablaParametros("Portafolio") = IIf(ddlPortafolio.SelectedIndex = 0, "", ddlPortafolio.SelectedItem.Text)
        tablaParametros("Mercado") = IIf(ddlMercado.SelectedIndex = 0, "", ddlMercado.SelectedItem.Text)
        tablaParametros("Moneda") = IIf(ddlMoneda.SelectedIndex = 0, "", ddlMoneda.SelectedItem.Text)
        tablaParametros("Operacion") = IIf(ddlOperacion.SelectedIndex = 0, "", ddlOperacion.SelectedItem.Text)
        tablaParametros("Banco") = IIf(ddlBanco.SelectedIndex = 0, "", ddlBanco.SelectedItem.Text)
        tablaParametros("ClaseCuenta") = IIf(ddlClaseCuenta.SelectedIndex = 0, "", ddlClaseCuenta.SelectedItem.Text)
        tablaParametros("codTipoOperacion") = ddlTipoOperacion.SelectedValue
        Session("ParametrosReporteMovimientos") = tablaParametros
        EjecutarJS("PopUpReportes('../Reportes/frmReporte.aspx?ClaseReporte=MovimientosTotales&FechaInicio=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text) & "&FechaFin=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text) & "')")
    End Sub
    Private Sub btnAceptarExt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarExt.Click
        Try
            Dim oOperacionesCaja As New OperacionesCajaBM
            oOperacionesCaja.InsertarOperacionesCajaExt(hdCodigoOperacionCaja.Value, ddlMotivo.SelectedValue, tbComentario.Text, DatosRequest, Math.Abs(Convert.ToInt32(chkLiqAntFon.Checked)), hdCodigoPortafolioSBS.Value)      'HDG OT 64767 20120222
            HabilitaControles(True)
            AlertaJS("La siguiente operacion ha sido enviada al administrador para su aprobación! ")
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnCancelarExt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelarExt.Click
        Try
            HabilitaControles(True)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim fechaLiquidacion As Decimal
            hdCodigoOperacionCaja.Value = e.CommandArgument
            hdCodigoPortafolioSBS.Value = CType(Row.FindControl("hdCodigoPortafolioSBS"), HiddenField).Value
            fechaLiquidacion = UIUtility.ConvertirFechaaDecimal(CType(Row.FindControl("hdFechaLiquidacion"), HiddenField).Value)
            If fechaLiquidacion >= UIUtility.ConvertirFechaaDecimal(hdFechaNegocio.Value) Then
                btnExtornar.Visible = True
            Else
                btnExtornar.Visible = False
            End If
            dgLista.SelectedIndex = Row.RowIndex
        End If
    End Sub
    Protected Sub ddlTipoOperacion_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoOperacion.SelectedIndexChanged
        CargarOperacion(True)
    End Sub
End Class