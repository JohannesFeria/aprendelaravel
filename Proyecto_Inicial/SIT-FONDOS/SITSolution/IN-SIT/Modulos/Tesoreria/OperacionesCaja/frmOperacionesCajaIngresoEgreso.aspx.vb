Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports SistemaProcesosBL
Imports UIUtility
Partial Class Modulos_Tesoreria_OperacionesCaja_frmOperacionesCajaIngresoEgreso
    Inherits BasePage
    Dim oOperacionCaja As New OperacionesCajaBM
    Dim oTipoPeracionBM As New TipoOperacionBM
    Dim oPeracionBM As New OperacionBM
    Dim oSaldoBancario As New SaldosBancariosBM
    Dim oPortafolio As New PortafolioBM
    Dim oPagoFechaComision As New PagoFechaComisionBM
    Private Sub llenarFilaVacia(ByRef table As DataTable)
        Dim row As DataRow = table.NewRow()
        For Each item As DataColumn In table.Columns
            Select Case item.DataType
                Case GetType(String)
                    row(item.ColumnName) = String.Empty
                Case GetType(Decimal)
                    row(item.ColumnName) = 0.0
            End Select
        Next
        table.Rows.Add(row)
    End Sub
    Private Sub EstablecerFecha()
        lblFechaPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue, DatosRequest))
    End Sub
    'OT10749 - Los saldos bancarios sólo deben actualizarce cuando haya un ingreso o actualización de una operación de caja
    Sub CargaSaldo()
        'oSaldoBancario.ActualizaSaldosBancarios(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text))
        Dim dtSaldos As datatable = oOperacionCaja.SaldoBancario(UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text), ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
        If dtSaldos.Rows.Count = 0 Then
            EjecutarJS("$('.confirmReversa').attr('disabled', true);")
            EjecutarJS("$('.confirm').attr('disabled', true);")
        Else
            EjecutarJS("$('.confirmReversa').attr('disabled', false);")
            EjecutarJS("$('.confirm').attr('disabled', false);")
        End If
        GVSaldo.DataSource = oOperacionCaja.SaldoBancario(UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text), ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
        GVSaldo.DataBind()
        lblFondo.Text = ddlPortafolio.SelectedItem.Text
        lblFecha.Text = lblFechaPago.Text
    End Sub
    'OT10749 - Fin
    Sub CargaOperaciones(NumeroCuenta As String)
        Dim DTOperacion As DataTable
        DTOperacion = oOperacionCaja.SaldoBancarioOperaciones(UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text), _
                ddlPortafolio.SelectedValue, NumeroCuenta)
        If DTOperacion.Rows.Count = 0 Then
            llenarFilaVacia(DTOperacion)
        End If
        GVOperaciones.DataSource = DTOperacion
        GVOperaciones.DataBind()
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dt As DataTable = oClaseCuenta.Listar().Tables(0)
            HelpCombo.LlenarComboBox(ddlClase, dt, "CodigoClaseCuenta", "Descripcion", False, "SELECCIONE")
            ddlClase.SelectedValue = "10"
        Else
            ddlClase.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClase)
        End If
        ddlClase.Enabled = enabled
    End Sub
    Private Sub CargaOperacioneFooter(CodigoTipoOperacion As String, ddlOperacionF As DropDownList)
        Dim DTOperaciones As DataTable = oPeracionBM.SeleccionarPorCodigoTipoOperacion(CodigoTipoOperacion, "", ddlClase.SelectedValue).Tables(0)
        HelpCombo.LlenarComboBox(ddlOperacionF, DTOperaciones, "CodigoOperacion", "Descripcion", True)
    End Sub
    'OT10749 - Los saldos bancarios sólo deben actualizarce cuando haya un ingreso o actualización de una operación de caja
    Private Sub ActualizarSaldos(ByVal p_CodigoPortafolio As String, ByVal p_FechaSaldo As Decimal)
        oSaldoBancario.ActualizaSaldosBancarios(p_CodigoPortafolio, p_FechaSaldo)
    End Sub
    'OT10749 - Fin
    Protected Sub lblFechaPago_TextChanged(sender As Object, e As System.EventArgs) Handles lblFechaPago.TextChanged
        Try
            pnSaldo.Visible = False
            If lblFechaPago.Text = String.Empty Then
                Exit Sub
            End If
            Dim FechaPortafolio As Decimal, FechaCajaIndividual As Decimal
            Dim FechaPago As Decimal = UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text)

            FechaCajaIndividual = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue)
            FechaPortafolio = UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue)

            If FechaPago > FechaPortafolio And Not ddlClase.SelectedValue.Contains("10") Then
                AlertaJS("La fecha ingresada no puede ser mayor a la fecha de apertura del portafolio o feriados.")
                lblFechaPago.Text = UIUtility.ConvertirFechaaString(FechaCajaIndividual)
            ElseIf FechaPago > FechaCajaIndividual Then
                AlertaJS("La fecha ingresada no puede ser mayor a la fecha actual de cierre de la caja (" + UIUtility.ConvertirFechaaString(FechaCajaIndividual) + ")")
                lblFechaPago.Text = UIUtility.ConvertirFechaaString(FechaCajaIndividual)
            ElseIf FechaPago < FechaCajaIndividual Then
                AlertaJS("La fecha ingresada no puede ser menor a la fecha actual de cierre de la caja (" + UIUtility.ConvertirFechaaString(FechaCajaIndividual) + ")")
                lblFechaPago.Text = UIUtility.ConvertirFechaaString(FechaCajaIndividual)
            Else
                Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                    lblFechaPago.Text = UIUtility.ConvertirFechaaString(FechaCajaIndividual)
                End If

            End If

            CargaSaldo()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    'OT10749 - Refactorizar codigo
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                CargarClaseCuenta()
                EstablecerFecha()
                CargaSaldo()
                CargarControles()
                InicializarBotones()
                Dim dtOperacion As New DataTable
                Dim dtTipoOperacion As New DataTable
                dtOperacion = oPeracionBM.Listar(DatosRequest).Tables(0)
                dtTipoOperacion = oTipoPeracionBM.Listar("A").Tables(0)
                ViewState("dtOperacion") = dtOperacion
                ViewState("dtTipoOperacion") = dtTipoOperacion
                lblFondo.Text = ddlPortafolio.SelectedItem.Text
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            EstablecerFecha()
            CargaSaldo()
            CargaOperaciones("")
            HDNumeroCuenta.Value = ""
            pnSaldo.Visible = False
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub ddlClase_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlClase.SelectedIndexChanged
        Try
            EstablecerFecha()
            CargaSaldo()
            CargaOperaciones("")
            HDNumeroCuenta.Value = ""
            pnSaldo.Visible = False
            If ddlClase.SelectedValue = "10" Then
                btnProceso.Visible = True
            Else
                btnProceso.Visible = False
            End If
            CargarControles() ''OT11192 - 08/03/2018 - Ian Pastor M. Habilitar los botones solo para las cuentas de inversión
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    'OT10749 - Fin
    Protected Sub GVSaldo_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVSaldo.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim NumeroCuenta As String = e.CommandArgument
                CargaOperaciones(NumeroCuenta)
                HDNumeroCuenta.Value = NumeroCuenta
                HDCodigoEntidad.Value = HttpUtility.HtmlDecode(gvr.Cells(1).Text)
                HDCodigoMercado.Value = HttpUtility.HtmlDecode(gvr.Cells(2).Text)
                HDCodigoMoneda.Value = HttpUtility.HtmlDecode(gvr.Cells(3).Text)
                pnSaldo.Visible = True
                lblBanco.Text = HttpUtility.HtmlDecode(gvr.Cells(4).Text)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub GVOperaciones_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVOperaciones.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim FechaPortafolio As Decimal = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue)
            Dim ejecutar As Integer
            Dim lbloperacion As Label

            If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) <> FechaPortafolio And ddlClase.SelectedValue = "10" Then
                AlertaJS("Solo se puede modificar en la fecha actual del portafolio.")
                Exit Sub
            End If

            'Agregado JH - Validacion Fecha Caja con clase Inversion para agregar movimientos
            If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) <> FechaPortafolio And ddlClase.SelectedValue = "20" Then
                AlertaJS("Solo se puede modificar, agregar o eliminar operaciones en la fecha de Caja asignada. Fecha de Caja : " + UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue)))
                Exit Sub
            End If

            If e.CommandName = "Add" Then
                Dim dsOpCaja As New OperacionCajaBE
                Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                Dim ddlOperacionF As DropDownList, ddlTipoOperacionF As DropDownList, TXTImporteF As TextBox
                ddlTipoOperacionF = CType(GVOperaciones.FooterRow.FindControl("ddltipooperacionF"), DropDownList)
                ddlOperacionF = CType(GVOperaciones.FooterRow.FindControl("ddlOperacionF"), DropDownList)
                TXTImporteF = CType(GVOperaciones.FooterRow.FindControl("txtImporteF"), TextBox)

                If TXTImporteF.Text = "" Then
                    AlertaJS("Debe ingresar un Importe para realizar la operación.")
                Else
                    opCaja.CodigoMercado = HDCodigoMercado.Value
                    opCaja.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                    opCaja.CodigoClaseCuenta = ddlClase.SelectedValue
                    opCaja.NumeroCuenta = HDNumeroCuenta.Value
                    If ddlTipoOperacionF.SelectedValue = "1" Then
                        opCaja.CodigoModalidadPago = "CPAG"
                    ElseIf ddlTipoOperacionF.SelectedValue = "2" Then
                        opCaja.CodigoModalidadPago = "CCOB"
                    End If
                    opCaja.CodigoTerceroDestino = ""
                    opCaja.CodigoTerceroOrigen = HDCodigoEntidad.Value
                    opCaja.NumeroCuentaDestino = ""
                    opCaja.CodigoOperacion = ddlOperacionF.SelectedValue
                    opCaja.Referencia = ""
                    opCaja.CodigoMoneda = HDCodigoMoneda.Value
                    opCaja.Importe = Replace(IIf(TXTImporteF.Text.Trim = "", "0", TXTImporteF.Text.Trim), ",", "")
                    opCaja.CodigoModelo = "SC01"
                    opCaja.CodigoOperacionCaja = ""
                    opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text)
                    dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                    If opCaja.CodigoClaseCuenta = "20" And opCaja.CodigoMercado = "1" And opCaja.CodigoModalidadPago = "CPAG" And opCaja.CodigoOperacion = "CSAF" Then
                        'validar si existe fechacomision en estado ingresado
                        If oPagoFechaComision.ValidarExistenciaIngresados(ddlPortafolio.SelectedValue).Rows.Count > 0 Then
                            AlertaJS("No se puede realizar el egreso de pago porque existen fechas de pago de comision en estado ingresado")
                            Exit Sub
                        End If
                        
                    End If

                    ejecutar = oOperacionCaja.Insertar_FechaOperacion(dsOpCaja, DatosRequest)
                    If ejecutar <> 0 Then
                        AlertaJS("Hubo un Error en el guardado del movimiento de caja / Code Error: " + ejecutar.ToString)
                    Else
                        If ddlOperacionF.SelectedValue = "OP0092" Then
                            AlertaJS("Se guardó correctamente el movimiento de caja.<BR>Se generaron las órdenes de inversión (Pendiente de confirmación) de los instrumentos correspondientes al Aumento de Capital.")
                        Else
                            AlertaJS("Se guardó correctamente el movimiento de caja.")
                        End If
                    End If
                End If
            ElseIf e.CommandName = "Modificar" Then
                Dim txtImporte As TextBox
                txtImporte = CType(gvr.FindControl("txtImporte"), TextBox)
                If txtImporte.Text = "" Then
                    AlertaJS("Debe ingresar un Importe para realizar la operación.")
                Else
                    oOperacionCaja.ActualizaOperacionCaja(ddlPortafolio.SelectedValue, CDec(txtImporte.Text), e.CommandArgument, DatosRequest)
                End If
                AlertaJS("Se modifico el monto de la operación correctamente.")
            ElseIf e.CommandName = "Eliminar" Then
                lbloperacion = CType(gvr.FindControl("lbloperacion"), Label)
                If HttpUtility.HtmlDecode(gvr.Cells(1).Text).Trim <> "" Then
                    AlertaJS("No se pueden eliminar órdenes que se han generado desde la negociación.")
                    Exit Sub
                End If
                ' INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital  | Eliminación del Aumento Capital para la nueva operación creada | 19092018

                If lbloperacion.Text = "OP0092" Then
                    Dim cantidadreg As Integer = New AumentoCapitalBM().AumentoCapital_ExisteGeneradaOI(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text))
                    If cantidadreg > 0 Then
                        AlertaJS("No se puede eliminar por que existen órdenes generadas de los instrumentos del aumento de capital.")
                        ddlPortafolio.SelectedIndex = 0
                        Exit Sub
                    End If
                End If
                ' FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital  | Eliminación del Aumento Capital para la nueva operación creada | 19092018
                Dim CodigoOperacionCaja As String
                CodigoOperacionCaja = e.CommandArgument
                'OT10749 - Se juntó los tres procesos en una sola transacción
                oOperacionCaja.ExtornarOperacionCaja(CodigoOperacionCaja, ddlPortafolio.SelectedValue, DatosRequest)
                ' INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital  | Eliminación del Aumento Capital para la nueva operación creada | 19092018
                If lbloperacion.Text = "OP0092" Then
                    Dim oRowAC As New AumentoCapitalBE
                    Dim oAumentoCapitalBM As New AumentoCapitalBM
                    Dim result As Integer
                    oAumentoCapitalBM.InicializarAumentoCapital(oRowAC)

                    oRowAC.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                    oRowAC.FechaAumentoCapital = ConvertirFechaaDecimal(lblFechaPago.Text)
                    oRowAC.Estado = "P"

                    result = oAumentoCapitalBM.AumentoCapital_Eliminar(oRowAC, DatosRequest)

                    If result <> 0 Then
                        AlertaJS("Hubo un Error en la eliminación del Aumento Capital / Code Error: " + result.ToString)
                    Else
                        AlertaJS("Se eliminó correctamente el Aumento de Capital.")
                    End If
                End If
                ' FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital  | Eliminación del Aumento Capital para la nueva operación creada | 19092018
                'OT10749 - Fin

            ElseIf e.CommandName = "Regularizar" Then
                oOperacionCaja.RegularizarIntradia(ddlPortafolio.SelectedValue, e.CommandArgument, DatosRequest)
            End If
                CargaOperaciones(HDNumeroCuenta.Value)
                CargaSaldo()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub GVOperaciones_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVOperaciones.RowDataBound
        Try
            If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                Dim ddlOperacion As DropDownList, ddlTipoOperacion As DropDownList, lbloperacion As Label, lbltipooperacion As Label, txtImporte As TextBox,
                    btnRegularizar As ImageButton
                ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                ddlTipoOperacion = CType(e.Row.FindControl("ddltipooperacion"), DropDownList)
                lbloperacion = CType(e.Row.FindControl("lbloperacion"), Label)
                lbltipooperacion = CType(e.Row.FindControl("lbltipooperacion"), Label)
                txtImporte = CType(e.Row.FindControl("txtImporte"), TextBox)
                btnRegularizar = CType(e.Row.FindControl("btnRegularizar"), ImageButton)
                HelpCombo.LlenarComboBox(ddlOperacion, CType(ViewState("dtOperacion"), Data.DataTable), "CodigoOperacion", "Descripcion", False)
                ddlOperacion.SelectedValue = lbloperacion.Text
                ddlOperacion.Enabled = False
                HelpCombo.LlenarComboBox(ddlTipoOperacion, CType(ViewState("dtTipoOperacion"), Data.DataTable), "CodigoTipoOperacion", "Descripcion", False)
                ddlTipoOperacion.SelectedValue = lbltipooperacion.Text
                ddlTipoOperacion.Enabled = False
                If txtImporte.Text = 0 Then
                    e.Row.Visible = False
                End If
                If ddlOperacion.SelectedValue = "8" Then
                    btnRegularizar.Visible = True
                End If
                ' INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital | Se inhabilita txtimporte cuando operación es aumento capital| 19092018
                txtImporte.Enabled = Not (lbloperacion.Text = "OP0092")
                ' FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital | Se inhabilita txtimporte cuando operación es aumento capital| 19092018
            ElseIf e.Row.RowType = ListItemType.Footer Then
                Dim ddlOperacionF As DropDownList, ddlTipoOperacionF As DropDownList
                ddlTipoOperacionF = CType(e.Row.FindControl("ddltipooperacionF"), DropDownList)
                ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoOperacionF, CType(ViewState("dtTipoOperacion"), Data.DataTable), "CodigoTipoOperacion", "Descripcion", True)
                CargaOperacioneFooter(ddlTipoOperacionF.SelectedValue, ddlOperacionF)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub ddltipooperacionF_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Try
            Dim ddlOperacionF As DropDownList, ddlTipoOperacionF As DropDownList
            ddlTipoOperacionF = CType(GVOperaciones.FooterRow.FindControl("ddltipooperacionF"), DropDownList)
            ddlOperacionF = CType(GVOperaciones.FooterRow.FindControl("ddlOperacionF"), DropDownList)
            CargaOperacioneFooter(ddlTipoOperacionF.SelectedValue, ddlOperacionF)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    ' INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital  | Obtener Aumento Capital para la nueva operación creada | 19092018
    Protected Sub ddlOperacionF_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Try
            Dim oAumentoCapitalBM As New AumentoCapitalBM
            Dim dsResult As DataSet
            Dim ddlOperacionF As DropDownList
            Dim txtImporteF As TextBox

            ddlOperacionF = CType(GVOperaciones.FooterRow.FindControl("ddlOperacionF"), DropDownList)
            txtImporteF = CType(GVOperaciones.FooterRow.FindControl("txtImporteF"), TextBox)

            txtImporteF.Enabled = True
            txtImporteF.Text = String.Empty

            If ddlOperacionF.SelectedValue = "OP0092" Then

                UIUtility.CargarPortafoliosbyAumentoCapital(ddlPortafolioAumentoCapitalTemp)
                If ddlPortafolioAumentoCapitalTemp.Items.FindByValue(ddlPortafolio.SelectedValue) Is Nothing Then
                    AlertaJS("El portafolio no está habilitado para realizar la operación de Aumento de Capital.")
                    ddlOperacionF.SelectedValue = String.Empty
                    Exit Sub
                End If

                dsResult = oAumentoCapitalBM.AumentoCapital_ListarbyFechaPortafolio(Me.ddlPortafolio.SelectedValue, ConvertirFechaaDecimal(Me.lblFechaPago.Text))
                If dsResult.Tables(0).Rows.Count = 0 Then
                    AlertaJS("No existe Aumento de Capital para la fecha y portafolio seleccionado.")
                    ddlOperacionF.SelectedValue = String.Empty
                    Exit Sub
                ElseIf dsResult.Tables(0).Select("Estado = 'PENDIENTE'").Length = 0 Then
                    AlertaJS("No existe Aumento de Capital para la fecha y portafolio seleccionado.")
                    ddlOperacionF.SelectedValue = String.Empty
                    Exit Sub
                End If

                For Each row As DataRow In dsResult.Tables(0).Rows
                    txtImporteF.Text = CStr(row("Importe"))
                Next
                txtImporteF.Enabled = False

            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    ' FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital  | Obtener Aumento Capital para la nueva operación creada | 19092018

    Protected Sub btnProceso_Click(sender As Object, e As System.EventArgs) Handles btnProceso.Click
        Try
            Dim FechaCajaIndividual As Decimal

            'OT12028 | Zoluxiones | rcolonia | Se valida la fecha independiente de caja recaudadora antes de obtener comisiones
            FechaCajaIndividual = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, "20")

            If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) > FechaCajaIndividual Then
                Throw New Exception("La fecha de la caja " + ddlClase.SelectedItem.ToString _
                                   & " debe ser menor o igual a la caja Inversion (" + UIUtility.ConvertirDecimalAStringFormatoFecha(FechaCajaIndividual) + ")")
            End If
            If HDNumeroCuenta.Value = "" Then
                AlertaJS("Seleccione una cuenta de la lista de Saldos.")
            Else
                If oOperacionCaja.GeneraInversiones(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text), HDNumeroCuenta.Value, DatosRequest) > 0 Then
                    CargaOperaciones(HDNumeroCuenta.Value)
                    CargaSaldo()
                    AlertaJS("Se generaron las inversiones.")
                Else
                    AlertaJS("No se encotraron inversiones para generar.")
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            'OT10749 - Terminado el proceso, se vuelve a colocar los valores del botón a su estado inicial
            btnProceso.Text = "Generar Inversiones"
            btnProceso.Enabled = True
        End Try
    End Sub
    Protected Sub btnCierre_Click(sender As Object, e As System.EventArgs) Handles btnCierre.Click
        Try
            Dim FechaActual As Decimal, FechaNueva As Decimal
            FechaActual = UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text)
            Dim fechaNuevaCadena As String = fnFechaNueva(lblFechaPago.Text).ToString("dd/MM/yyyy")
            FechaNueva = UIUtility.ConvertirFechaaDecimal(fechaNuevaCadena)

            Dim fechaFondo As Decimal = UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue)
            Dim CantidadRegistroPendientePagoComision As Integer = oPortafolio.CierreCajas_ValidarFechaIngresoPagoComision(ddlPortafolio.SelectedValue, ddlClase.SelectedValue, FechaActual)

            If FechaNueva > fechaFondo And Not ddlClase.SelectedValue.Contains("10") Then
                AlertaJS("No se puede realizar el cierre porque La nueva fecha de cierre (" + fechaNuevaCadena + ") es mayor a la fecha de apertura del portafolio")
                Exit Sub
            End If

            If CantidadRegistroPendientePagoComision > 0 Then
                AlertaJS("Existen (" + CantidadRegistroPendientePagoComision.ToString + ") Pago de Comisión pendiente de confirmación en el portafolio seleccionado.")
                Exit Sub
            End If


            lblFechaPago.Text = fnFechaNueva(lblFechaPago.Text).ToString("dd/MM/yyyy")
            oPortafolio.AperturaCajaRecaudo(ddlPortafolio.SelectedValue, ddlClase.SelectedValue, FechaNueva, DatosRequest)
            oPortafolio.GeneraSaldoBanco(FechaNueva, FechaActual, ddlPortafolio.SelectedValue, ddlClase.SelectedValue, DatosRequest)

            'oPortafolio.GeneraSaldoBanco(FechaNueva, FechaActual, ddlPortafolio.SelectedValue, "10", DatosRequest)
            'OT-10902 - 02/11/2017 - Ian Pastor. Actualizar saldos bancarios de la caja de inversiones
            'oPortafolio.GeneraSaldoBanco(FechaNueva, FechaActual, ddlPortafolio.SelectedValue, "20", DatosRequest)
            'OT10749 - Generar movimientos bancarios de ingreso y egreso (Si los hubiera) para calcular el nuevo saldo final
            'para la nueva fecha
            ActualizarSaldos(ddlPortafolio.SelectedValue, FechaNueva)
            'OT10749 - Fin
            CargaSaldo()
            CargaOperaciones("")
            HDNumeroCuenta.Value = ""
            pnSaldo.Visible = False
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub btnReversa_Click(sender As Object, e As System.EventArgs) Handles btnReversa.Click
        Try
            Dim FechaNueva As Decimal
            FechaNueva = UIUtility.ConvertirFechaaDecimal(fnFechaAnterior(lblFechaPago.Text).ToString("dd/MM/yyyy"))
            Dim objOperaciones As New PrecierreBO
            Dim rowVCuotaHistorico As DataRow = Nothing

            Dim oPortafolioBE As PortafolioBE
            Dim oRow As PortafolioBE.PortafolioRow
            Dim oPortafolioBM As New PortafolioBM
            oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
            oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            If oRow.PorSerie.Equals("S") Then
                Dim DtValoresSerie As DataTable
                Dim oPortafolio As New PortafolioBM

                DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie(ddlPortafolio.SelectedValue)
                If DtValoresSerie.Rows.Count > 0 Then
                    For Each fila As DataRow In DtValoresSerie.Rows
                        rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(fila("CodigoPortafolioSO"), Convert.ToDateTime(UIUtility.ConvertirFechaaString(FechaNueva)))
                        If rowVCuotaHistorico IsNot Nothing Then Exit For
                    Next
                End If
            Else
                If oRow.CodigoPortafolioSisOpe.Trim <> String.Empty Then
                    rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(oRow.CodigoPortafolioSisOpe, Convert.ToDateTime(UIUtility.ConvertirFechaaString(FechaNueva)))
                End If
            End If
          
            If rowVCuotaHistorico IsNot Nothing Then
                AlertaJS("Ya existe un cierre de valor cuota en operaciones para esta fecha, debe extornarla.")
                Exit Sub
            End If
            Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, FechaNueva)
            If cantidadreg > 0 Then
                AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                lblFechaPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue))
                Exit Sub
            End If
            lblFechaPago.Text = fnFechaAnterior(lblFechaPago.Text).ToString("dd/MM/yyyy")
            lblFecha.Text = lblFechaPago.Text
            oPortafolio.ReversaCajaRecaudo(ddlPortafolio.SelectedValue, ddlClase.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text))
            CargaSaldo()
            CargaOperaciones("")
            HDNumeroCuenta.Value = ""
            pnSaldo.Visible = False
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString, "'", ""))
        End Try
    End Sub
    'OT11008 - 23/01/2018 - Ian Pastor M.
    'Descripción: Obtener comisiones, rescates y retenciones del sistema de operaciones
    Protected Sub btnRescatarCuotas_Click(sender As Object, e As System.EventArgs) Handles btnRescatarCuotas.Click
        Try
            If ObtenerRescateyRetencionesDeOperaciones() Then
                CargaSaldo()
                CargaOperaciones("")
                AlertaJS("Se cargaron correctamente las operaciones")
            Else
                AlertaJS("No se encontraron rescates a la fecha " & lblFecha.Text & " para el portafolio " & ddlPortafolio.SelectedItem.Text)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            btnRescatarCuotas.Text = "Obtener rescates"
            btnRescatarCuotas.Enabled = True
        End Try
    End Sub

    Private Function ObtenerRescateyRetencionesDeOperaciones() As Boolean
        'OT11192 - 08/03/2018 - Ian Pastor M.
        'Descripción: Se obtienen las cuentas económicas de un portafolio y se recorre con un bucle para ingresar sus rescates
        Dim sw As Boolean = False
        Dim bolOpeIng As Boolean = False 'Verifica si se ha registrado operaciones de rescate
        Dim dt As DataTable = oOperacionCaja.SaldoBancario(UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text), ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
        Dim FechaCajaIndividual As Decimal

        'OT12028 | Zoluxiones | rcolonia | Se valida la fecha independiente de caja recaudadora antes de obtener comisiones
        FechaCajaIndividual = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, "10")

        If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) > FechaCajaIndividual Then
            Throw New Exception("La fecha de la caja " + ddlClase.SelectedItem.ToString _
                               & " debe ser menor o igual a la caja Recaudadora (" + UIUtility.ConvertirDecimalAStringFormatoFecha(FechaCajaIndividual) + ")")
        End If
        For Each dtRow As DataRow In dt.Rows
            sw = oOperacionCaja.ObtenerMovimientosRescateyRetenciones(ddlPortafolio.SelectedValue, lblFechaPago.Text, _
                                                             dtRow("NumeroCuenta"), dtRow("CodigoEntidad"), dtRow("CodigoMoneda"), _
                                                             ddlClase.SelectedValue, dtRow("CodigoMercado"), DatosRequest)
            If sw Then
                bolOpeIng = sw
            End If
        Next
        ObtenerRescateyRetencionesDeOperaciones = bolOpeIng
        'CargaOperaciones(HDNumeroCuenta.Value)
        'CargaSaldo()
        'OT11192 - Fin
    End Function

    Protected Sub btnObtenerComisiones_Click(sender As Object, e As System.EventArgs) Handles btnObtenerComisiones.Click
        Try
            If ObtenerComisiones() Then
                CargaSaldo()
                CargaOperaciones("")
                AlertaJS("Se cargaron correctamente las operaciones")
            Else
                AlertaJS("No se encontraron retenciones y comisiones a la fecha " & lblFecha.Text & " para el portafolio " & ddlPortafolio.SelectedItem.Text)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            btnObtenerComisiones.Text = "Obtener comisiones/retenciones"
            btnObtenerComisiones.Enabled = True
        End Try
    End Sub

    Private Function ObtenerComisiones() As Boolean
        'OT11192 - 08/03/2018 - Ian Pastor M.
        'Descripción: Se obtienen las cuentas económicas de un portafolio y se recorre con un bucle para ingresar sus rescates
        Dim sw As Boolean = False
        Dim bolOpeIng As Boolean = False 'Verifica si se ha registrado operaciones de rescate
        Dim objParametrosBM As New ParametrosGeneralesBM
        Dim dtCusBanc As DataTable = Nothing
        Dim FechaCajaIndividual As Decimal

        'OT12028 | Zoluxiones | rcolonia | Se valida la fecha independiente de caja recaudadora antes de obtener comisiones
        FechaCajaIndividual = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, "10")

        If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) > FechaCajaIndividual Then
            Throw New Exception("La fecha de la caja " + ddlClase.SelectedItem.ToString _
                               & " debe ser menor o igual a la caja Recaudadora (" + UIUtility.ConvertirDecimalAStringFormatoFecha(FechaCajaIndividual) + ")")
        End If
        dtCusBanc = objParametrosBM.Listar("BAN_CUST", DatosRequest)
        If dtCusBanc.Rows.Count = 0 Or dtCusBanc.Rows(0)("Valor2").ToString() = "" Then
            Throw New Exception("No se encuentra definido el custodio de la entidad bancaria de la empresa." _
                                & "Por favor comuníquese con sistemas para que lo registre en la parametría general")
        End If
        Dim dt() As DataRow = oOperacionCaja.SaldoBancario(UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text), _
                                                           ddlClase.SelectedValue, ddlPortafolio.SelectedValue). _
                                                       Select("CodigoEntidad='" & dtCusBanc.Rows(0)("Valor2").ToString() & "'")
        For Each dtRow As DataRow In dt
            sw = oOperacionCaja.ObtenerMovimientosComisionesSisOpe(ddlPortafolio.SelectedValue, lblFechaPago.Text, _
                                                             dtRow("NumeroCuenta"), dtRow("CodigoEntidad"), dtRow("CodigoMoneda"), _
                                                             ddlClase.SelectedValue, dtRow("CodigoMercado"), DatosRequest)
            If sw Then
                bolOpeIng = sw
            End If
        Next
        ObtenerComisiones = bolOpeIng
        'CargaOperaciones(HDNumeroCuenta.Value)
        'CargaSaldo()
        'OT11192 - Fin.
    End Function
    'OT11008 - Fin
    'OT11192 - 08/03/2018 - Ian Pastor M.
    'Descripción: Activa o inactiva los controles según la clase de cuenta que se seleccione
    Private Sub CargarControles()
        btnRescatarCuotas.Visible = IIf(ddlClase.SelectedValue = "20", True, False)
        btnObtenerComisiones.Visible = IIf(ddlClase.SelectedValue = "20", True, False)
        btnObtenerSuscripciones.Visible = IIf(ddlClase.SelectedValue = "10", True, False)
    End Sub
    'OT11192 - Fin

    'OT11237 - 15/03/2018 - Ian Pastor M.
    'Descripción: Inicializa los botones para que sólo se pueda dar click una vez al ejecutar un proceso
    Private Sub InicializarBotones()
        btnProceso.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnRescatarCuotas.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnObtenerComisiones.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnObtenerSuscripciones.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
    End Sub
    'OT11237 - Fin

    Protected Sub btnObtenerSuscripciones_Click(sender As Object, e As System.EventArgs) Handles btnObtenerSuscripciones.Click
        Try
            If (ObtenerSuscripciones()) Then
                CargaSaldo()
                CargaOperaciones("")
                AlertaJS("Se cargaron correctamente las operaciones")
            Else
                AlertaJS("No se encontraron suscripciones a la fecha " & lblFecha.Text & " para el portafolio " & ddlPortafolio.SelectedItem.Text)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            btnObtenerSuscripciones.Text = "Obtener suscripciones"
            btnObtenerSuscripciones.Enabled = True
        End Try
    End Sub

    'OT11237 - 15/03/2018 - Ian Pastor M.
    'Descripción: Obtiene las suscripciones del sistema de operaciones
    Private Function ObtenerSuscripciones() As Boolean
        Dim sw As Boolean = False
        Dim FechaCajaIndividual As Decimal

        'OT12028 | Zoluxiones | rcolonia | Se valida la fecha independiente de caja recaudadora antes de obtener comisiones
        FechaCajaIndividual = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, "20")

        'If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) > FechaCajaIndividual Then
        '    Throw New Exception("La fecha de la caja " + ddlClase.SelectedItem.ToString _
        '                       & " debe ser menor o igual a la caja Inversion (" + UIUtility.ConvertirDecimalAStringFormatoFecha(FechaCajaIndividual) + ")")
        'End If
        sw = oOperacionCaja.ObtenerSuscripcionesSisOpe(ddlPortafolio.SelectedValue, lblFechaPago.Text, ddlClase.SelectedValue, DatosRequest)
        ObtenerSuscripciones = sw
    End Function
    'OT11237 - Fin

End Class