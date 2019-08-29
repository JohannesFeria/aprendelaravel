Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_Cuentasxpagar_frmPagoParcial
    Inherits BasePage

    Private CodigoPortafolio As String
    Private CodigoMoneda As String
    Protected WithEvents hdImporteTotal As System.Web.UI.HtmlControls.HtmlInputHidden
    Private Importe As String

#Region "Eventos de Control"
    Private Function NombrePortafolio(ByVal codigoPortafolio As String) As String
        If codigoPortafolio.Trim().Length = 0 Then Return ""
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        Return dt.Select("CodigoPortafolio='" & codigoPortafolio & "'")(0)("Descripcion")
    End Function

    Private Function NombreMoneda(ByVal codigoMoneda As String) As String
        If codigoMoneda.Trim().Length = 0 Then Return ""
        Dim dsMoneda As DataSet = New MonedaBM().Listar()
        Return dsMoneda.Tables(0).Select("CodigoMoneda='" & codigoMoneda & "'")(0)("Descripcion")
    End Function

    Private Function CrearTabla() As DataTable
        Dim dtTabla As New DataTable
        dtTabla.Columns.Add("Indice")
        For i As Integer = 0 To 4
            Dim row As DataRow = dtTabla.NewRow()
            row(0) = i
            dtTabla.Rows.Add(row)
        Next
        Return dtTabla
    End Function

    Private Function ValidarDatosGrilla() As Boolean
        Dim valPorcentaje As Decimal
        Dim valImporte As Decimal
        SumarCampos(valPorcentaje, valImporte)
        If Not ValidarSeleccionCuenta() Then
            AlertaJS("Se debe seleccionar un número de cuenta")
            Return False
        End If
        If (valPorcentaje <> 100) Then
            AlertaJS("La suma de los porcentajes debe ser 100")
            Return False
        End If
        If (valImporte <> Decimal.Parse(txtImporteTotal.Text.Replace(".", UIUtility.DecimalSeparator))) Then
            AlertaJS("La suma de los importes debe ser igual al importe total (" & txtImporteTotal.Text & ")")
            Return False
        End If
        Return True
    End Function

    Private Sub SumarCampos(ByRef porcentaje As Decimal, ByRef importe As Decimal)
        Dim valPorcentaje As Decimal = 0
        Dim valImporte As Decimal = 0
        Dim txtPorcentaje As TextBox
        Dim txtImporte As TextBox
        Dim chbSeleccion As CheckBox
        For Each row As GridViewRow In dgLista.Rows
            chbSeleccion = row.Cells(1).Controls(1)
            If chbSeleccion.Checked Then
                txtPorcentaje = row.Cells(3).Controls(1)
                txtImporte = row.Cells(4).Controls(1)
                If txtPorcentaje.Text <> "" Then
                    valPorcentaje = valPorcentaje + Decimal.Parse(txtPorcentaje.Text.Replace(".", UIUtility.DecimalSeparator))
                    valImporte = valImporte + Decimal.Parse(txtImporte.Text.Replace(".", UIUtility.DecimalSeparator))
                End If
            End If
        Next
        porcentaje = valPorcentaje
        importe = valImporte
    End Sub

    Private Function ValidarSeleccionCuenta() As Boolean
        Dim ddlNroCuenta As DropDownList
        Dim chbSeleccion As CheckBox
        For Each row As GridViewRow In dgLista.Rows
            ddlNroCuenta = row.Cells(7).Controls(1)
            chbSeleccion = row.Cells(1).Controls(1)
            If chbSeleccion.Checked And ddlNroCuenta.SelectedValue = "" Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Sub ActualizarEstadoControles()
        For Each item As GridViewRow In dgLista.Rows
            Dim chbSeleccion As CheckBox = item.Cells(1).Controls(1)
            Dim ddlTipoPago As DropDownList = item.Cells(2).Controls(1)
            Dim ddlClaseCuenta As DropDownList = item.Cells(5).Controls(1)
            Dim ddlBanco As DropDownList = item.Cells(6).Controls(1)
            Dim ddlNroCuenta As DropDownList = item.Cells(7).Controls(1)
            Dim txtPorcentaje As TextBox = item.Cells(3).Controls(1)
            Dim txtImporte As TextBox = item.Cells(4).Controls(1)
            ddlTipoPago.Enabled = chbSeleccion.Checked
            ddlClaseCuenta.Enabled = chbSeleccion.Checked
            ddlBanco.Enabled = chbSeleccion.Checked
            ddlNroCuenta.Enabled = chbSeleccion.Checked
            txtPorcentaje.Enabled = chbSeleccion.Checked
            txtImporte.Enabled = chbSeleccion.Checked
        Next
    End Sub

#End Region

#Region "CargarDatos"
    Private Sub CargarClaseCuenta(ByVal ddlClase As DropDownList, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dt As DataTable = oClaseCuenta.Listar().Tables(0)
            HelpCombo.LlenarComboBox(ddlClase, dt, "CodigoClaseCuenta", "Descripcion", True, "SELECCIONE")
        Else
            ddlClase.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClase)
        End If
        ddlClase.Enabled = enabled

    End Sub

    Private Sub CargarBanco(ByVal ddlBanco As DropDownList, ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dt As DataTable = oBanco.SeleccionarBancoPorCodigoMercado(codMercado).Tables(0)
            HelpCombo.LlenarComboBox(ddlBanco, dt, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub

    Private Sub CargarDetallePortafolio(ByVal ddlNroCuenta As DropDownList, ByVal codTercero As String, ByVal CodigoClaseCuenta As String, ByVal CodigoPortafolio As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dt As DataTable = oCuentaEconomica.SeleccionarPorFiltro(CodigoPortafolio, CodigoClaseCuenta, codTercero, CodigoMoneda, "", DatosRequest).Tables(1)
            HelpCombo.LlenarComboBox(ddlNroCuenta, dt, "CodigoCuenta", "NumeroCuenta", True, "SELECCIONE")
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        ddlNroCuenta.Enabled = enabled
    End Sub

    Private Sub CargarTipoPago(ByVal ddlTipoPago As DropDownList, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oModalidadPago As New ModalidadPagoBM
            Dim dt As DataTable = oModalidadPago.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlTipoPago, dt, "codigoModalidadPago", "Descripcion", True, "SELECCIONE")
        Else
            ddlTipoPago.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoPago)
        End If
        ddlTipoPago.Enabled = enabled
    End Sub
#End Region

#Region "Eventos de la Página"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CodigoPortafolio = IIf(Request.QueryString.Item("codPortafolio") Is Nothing, "", Request.QueryString.Item("codPortafolio"))
        CodigoMoneda = IIf(Request.QueryString.Item("codMoneda") Is Nothing, "", Request.QueryString.Item("codMoneda"))
        Importe = IIf(Request.QueryString.Item("Importe") Is Nothing, "", Request.QueryString.Item("Importe"))
        btnAceptar.Attributes.Add("onclick", "return ValidarDatos();")
        If Not Page.IsPostBack Then
            viewstate("CodigoPortafolio") = CodigoPortafolio
            txtPortafolio.Text = NombrePortafolio(CodigoPortafolio)
            txtMoneda.Text = NombreMoneda(CodigoMoneda)
            txtImporteTotal.Text = Importe
            dgLista.DataSource = CrearTabla()
            dgLista.DataBind()
        Else
            ActualizarEstadoControles()
            CodigoPortafolio = viewstate("CodigoPortafolio")
        End If
    End Sub

    'Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect("~/frmDefault.aspx")
    'End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmLiquidaciones.aspx?NumeroOperacion=" & IIf(Request.QueryString.Item("NumeroOp") Is Nothing, "", Request.QueryString.Item("NumeroOp")))
    End Sub
#End Region

    Public Sub ddl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim selectedIndex As Integer = Integer.Parse(CType(sender, DropDownList).ClientID.Replace("dgLista_ctl", "").Replace("_ddlClaseCuenta", ""))
        Dim row As GridViewRow = dgLista.Rows(selectedIndex - 2)
        Dim ddlClaseCuenta As DropDownList = row.Cells(5).Controls(1)
        Dim ddlBanco As DropDownList = row.Cells(6).Controls(1)
        Dim ddlNroCuenta As DropDownList = row.Cells(7).Controls(1)
        CargarDetallePortafolio(ddlNroCuenta, ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, CodigoPortafolio)
    End Sub

    Public Sub ddl_SelectedIndexChanged1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim selectedIndex As Integer = Integer.Parse(CType(sender, DropDownList).ClientID.Replace("dgLista_ctl", "").Replace("_ddlBanco", ""))
        Dim row As GridViewRow = dgLista.Rows(selectedIndex - 2)
        Dim ddlClaseCuenta As DropDownList = row.Cells(5).Controls(1)
        Dim ddlBanco As DropDownList = row.Cells(6).Controls(1)
        Dim ddlNroCuenta As DropDownList = row.Cells(7).Controls(1)
        CargarDetallePortafolio(ddlNroCuenta, ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, CodigoPortafolio)
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oCuentasPorCobrar As New CuentasPorCobrarBM
            Dim NumeroOperacion As String = IIf(Request.QueryString.Item("NumeroOp") Is Nothing, "", Request.QueryString.Item("NumeroOp"))
            Dim chbSeleccion As CheckBox
            If (ValidarDatosGrilla()) Then
                For Each row As GridViewRow In dgLista.Rows
                    chbSeleccion = row.Cells(1).Controls(1)
                    If chbSeleccion.Checked Then
                        Dim ddlTipoPago As DropDownList = row.Cells(2).Controls(1)
                        Dim ddlNroCuenta As DropDownList = row.Cells(7).Controls(1)
                        Dim txtPorcentaje As TextBox = row.Cells(3).Controls(1)
                        Dim txtImporte As TextBox = row.Cells(4).Controls(1)
                        oCuentasPorCobrar.IngresarPagoParcial(NumeroOperacion, ddlNroCuenta.SelectedValue.Split(",")(1), ddlNroCuenta.SelectedValue.Split(",")(0), _
                                                              Decimal.Parse(txtImporte.Text.Replace(".", UIUtility.DecimalSeparator)), _
                                                              DatosRequest)
                    End If
                Next
                Dim dsOpCaja As New OperacionCajaBE
                Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                opCaja.BancoMatrizDestino = ""
                opCaja.BancoMatrizOrigen = ""
                opCaja.ObservacionCartaDestino = ""
                opCaja.NumeroOperacion = NumeroOperacion
                opCaja.NumeroCuenta = ""
                opCaja.CodigoPortafolioSBS = ""
                opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(txtFechaLiq.Text)
                opCaja.CodigoOperacionCaja = "0"
                opCaja.Importe = 0
                opCaja.CodigoModalidadPago = ""
                dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                oCuentasPorCobrar.Liquidar(dsOpCaja, "", "", DatosRequest, String.Empty, String.Empty, String.Empty, "", "", "")
                AlertaJS("La cuenta ha sido Liquidada")
                btnAceptar.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chbSeleccion As CheckBox = e.Row.Cells(1).Controls(1)
            Dim ddlTipoPago As DropDownList = e.Row.Cells(2).Controls(1)
            Dim ddlClaseCuenta As DropDownList = e.Row.Cells(5).Controls(1)
            Dim ddlBanco As DropDownList = e.Row.Cells(6).Controls(1)
            Dim ddlNroCuenta As DropDownList = e.Row.Cells(7).Controls(1)
            Dim txtPorcentaje As TextBox = e.Row.Cells(3).Controls(1)
            Dim txtImporte As TextBox = e.Row.Cells(4).Controls(1)
            CargarTipoPago(ddlTipoPago)
            CargarClaseCuenta(ddlClaseCuenta)
            CargarBanco(ddlBanco, "")
            CargarDetallePortafolio(ddlNroCuenta, "", "", CodigoPortafolio)
            txtPorcentaje.Attributes.Add("onblur", "ActualizarImporte(document.getElementById('" & txtImporte.ClientID & "'),this);")
            txtImporte.Attributes.Add("onblur", "ActualizarPorcentaje(document.getElementById('" & txtPorcentaje.ClientID & "'),this);")
            chbSeleccion.Attributes.Add("onclick", "ActualizarEstadoControles(document.getElementById('" & chbSeleccion.ClientID & "')," & _
                                                                             "document.getElementById('" & txtImporte.ClientID & "')," & _
                                                                             "document.getElementById('" & txtPorcentaje.ClientID & "')," & _
                                                                             "document.getElementById('" & ddlTipoPago.ClientID & "')," & _
                                                                             "document.getElementById('" & ddlClaseCuenta.ClientID & "')," & _
                                                                             "document.getElementById('" & ddlBanco.ClientID & "')," & _
                                                                             "document.getElementById('" & ddlNroCuenta.ClientID & "'))")
            ddlTipoPago.Enabled = False
            ddlClaseCuenta.Enabled = False
            ddlBanco.Enabled = False
            ddlNroCuenta.Enabled = False
            txtPorcentaje.Enabled = False
            txtImporte.Enabled = False
        End If
    End Sub
End Class
