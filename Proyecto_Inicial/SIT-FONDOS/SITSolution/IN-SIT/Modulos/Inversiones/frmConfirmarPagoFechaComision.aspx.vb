Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports SistemaProcesosBL
Imports UIUtility
Imports ParametrosSIT
Partial Class Modulos_Inversiones_frmConfirmarPagoFechaComision
    Inherits BasePage

    Dim portafolioBM As New PortafolioBM
    Dim ibBorrar As ImageButton
    Dim ddlBanco, ddlNumeroDeCuenta As DropDownList
    Dim pagoFechaComisionBM As New PagoFechaComisionBM
    Dim parametroGeneralBM As New ParametrosGeneralesBM
    Dim codigoMonedaAdministradora As String = "NSOL"
    Dim codigoFondoAdministradora As String = "23"
    Dim cbSelect As CheckBox
    Dim hdCodigoFondo, hdCodigoMoneda, hdCodigoEstado, hdCodigoBanco, hdNumeroDeCuenta As HiddenField

#Region "Variables en Sesion"
    Public Property dtFondosSesion() As DataTable
        Get
            If Session("dtPagosComisionCustodioSesion") Is Nothing Then
                Return New DataTable
            Else
                Return CType(Session("dtPagosComisionCustodioSesion"), DataTable)
            End If
        End Get
        Set(ByVal value As DataTable)
            Session("dtPagosComisionCustodioSesion") = value
        End Set
    End Property
#End Region
   
#Region "Metodos"
    Private Sub CargarPagina()
        txtFechaCorte.Text = Date.Now.ToString("dd/MM/yyyy")
        btnConfirmar.Enabled = False
        Dim dt As DataTable = portafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, "TODOS")
        Dim dtBanco As DataTable = pagoFechaComisionBM.ListarBancos(codigoFondoAdministradora, codigoMonedaAdministradora)
        HelpCombo.LlenarComboBox(ddlBancoAdministradora, dtBanco, "CodigoBanco", "Descripcion", True)
        If ddlBancoAdministradora.SelectedValue = "" Then
            ddlNumeroDeCuentaAdministradora.Items.Insert(0, New System.Web.UI.WebControls.ListItem("--SELECCIONE--", ""))
        End If
        Buscar()

    End Sub
    Private Sub Buscar()
        Dim dtFondos As DataTable = pagoFechaComisionBM.ListarPortafoliosCustodio(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaCorte.Text))

        Dim totalIngresados = (From dr In dtFondos
                                Where (dr("CodigoEstado") = "ING")
                                     Select dr
                       )
        btnConfirmar.Enabled = False
        If totalIngresados IsNot Nothing Then
            If totalIngresados.Count() > 0 Then
                btnConfirmar.Enabled = True
            End If
        End If

        dtFondosSesion = dtFondos
        gvFondos.DataSource = dtFondosSesion
        gvFondos.DataBind()
    End Sub



    Private Function obtenerIndiceColumna_Grilla(ByVal nomCol As String, ByVal grilla As GridView) As Integer
        Dim columna As DataControlFieldCollection = grilla.Columns
        Dim indiceCol As Integer = -1
        For Each celda As DataControlField In columna
            If TypeOf celda Is System.Web.UI.WebControls.BoundField Then
                If CType(celda, BoundField).DataField = nomCol Then
                    indiceCol = columna.IndexOf(celda)
                    Exit For
                End If
            End If
        Next
        Return indiceCol
    End Function

    Private Function retornarNumSeleccionados(ByVal grilla As GridView, ByVal chkNombre As String) As Integer
        Dim iCont As Int64 = grilla.Rows.Count - 1
        Dim chk As CheckBox
        retornarNumSeleccionados = 0
        While iCont >= 0 And retornarNumSeleccionados < 1
            If grilla.Rows(iCont).FindControl(chkNombre).GetType Is GetType(CheckBox) Then
                chk = CType(grilla.Rows(iCont).FindControl(chkNombre), CheckBox)
                If chk.Checked = True Then
                    retornarNumSeleccionados += 1
                End If
            End If
            iCont = iCont - 1
        End While
        Return retornarNumSeleccionados
    End Function
    Private Sub EnviarCorreoNotificacion(ByVal accionTitulo As String, ByVal accionDescripcion As String, ByVal descripcion As String, ByVal accion As String)
        Try
            Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String)
            dict.Add("@Fondos", descripcion)
            dict.Add("@AccionTitulo", accionTitulo)
            dict.Add("@AccionDescripcion", accionDescripcion)
            Dim paraCorreo As String = parametroGeneralBM.SeleccionarPorFiltro("FECHACOBRO_EMAIL_" + accion, "", "PARA", "", DatosRequest).Rows(0)("Nombre")
            Dim copiaCorreo As String = parametroGeneralBM.SeleccionarPorFiltro("FECHACOBRO_EMAIL_" + accion, "", "COPIA", "", DatosRequest).Rows(0)("Nombre")
            UIUtility.EnviarMailPlantilla(paraCorreo, copiaCorreo, accionTitulo & " de pago de comisión", "PagoComision.html", dict, DatosRequest)
        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error en el EnviarCorreoNotificacion/ " + ex.Message.ToString, "'", String.Empty))
        End Try

    End Sub

#End Region

#Region "Eventos de la pagina"
    Protected Sub btnConfirmar_Click(sender As Object, e As System.EventArgs) Handles btnConfirmar.Click
        'Seleccionar 

        Try

            If hdRptaConfirmar.Value = "NO" Then
                ConfirmarJS("¿Desea confirmar los pagos de comisión seleccionados?", "document.getElementById('hdRptaConfirmar').value = 'SI';document.getElementById('btnConfirmar').click(); ")
                Exit Sub
            Else
                hdRptaConfirmar.Value = "NO"
                Dim rootPath As String = Request.Url.Host
                Dim combosObligatorios As String = ""
                Dim mensajeCamposObligatorios As String = ""
                If ddlBancoAdministradora.SelectedValue = "" Then
                    mensajeCamposObligatorios = mensajeCamposObligatorios + "<br>- Banco"
                End If
                If ddlNumeroDeCuentaAdministradora.SelectedValue = "" Then
                    mensajeCamposObligatorios = mensajeCamposObligatorios + "<br>- Numero de Cuenta"
                End If

                If mensajeCamposObligatorios <> "" Then
                    mensajeCamposObligatorios = "Debe seleccionar: " + mensajeCamposObligatorios
                    AlertaJS(mensajeCamposObligatorios)
                    Exit Sub
                End If

                Dim nSeleccionadas As Int64 = 0, iCont As Int64 = gvFondos.Rows.Count - 1
                Dim chk As CheckBox
                Dim hdFilaEstado As HiddenField
                Dim ddlFilaBanco As DropDownList
                Dim ddlFilaNumeroCuenta As DropDownList
                Dim fondosConfirmadosCorreo As String = ""
                Dim mensajeError As String = String.Empty
                nSeleccionadas = retornarNumSeleccionados(gvFondos, "chkSelectPE")
                If nSeleccionadas > 0 Then
                    Dim listaFechas As New List(Of PagoFechaComisionBE)
                    While iCont >= 0
                        If gvFondos.Rows(iCont).FindControl("chkSelectPE").GetType Is GetType(CheckBox) Then
                            chk = CType(gvFondos.Rows(iCont).FindControl("chkSelectPE"), CheckBox)
                            hdFilaEstado = CType(gvFondos.Rows(iCont).FindControl("hdCodigoEstado"), HiddenField)
                            If chk.Checked Then
                                If hdFilaEstado.Value = "ING" Then

                                    ddlFilaBanco = CType(gvFondos.Rows(iCont).FindControl("ddlBanco"), DropDownList)
                                    ddlFilaNumeroCuenta = CType(gvFondos.Rows(iCont).FindControl("ddlNumeroDeCuenta"), DropDownList)

                                    Dim codigoPortafolio As String = gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondos)).Text.Trim()
                                    Dim fechaCajaCadena As String = gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("FechaSolicitudCadena", gvFondos)).Text.Trim()
                                    ' Validar que exista un VC al T - 1 para poder generar el pago de comisión
                                    Dim rowVCuotaHistorico As DataRow = Nothing
                                    Dim objOperaciones As New PrecierreBO
                                    Dim oPortafolioBE As PortafolioBE
                                    Dim oRow As PortafolioBE.PortafolioRow
                                    Dim oPortafolioBM As New PortafolioBM
                                    oPortafolioBE = oPortafolioBM.Seleccionar(codigoPortafolio, Me.DatosRequest)
                                    oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)

                                    If oRow.PorSerie.Equals("S") Then
                                        Dim DtValoresSerie As DataTable
                                        Dim oPortafolio As New PortafolioBM

                                        DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie(codigoPortafolio)
                                        If DtValoresSerie.Rows.Count > 0 Then
                                            For Each fila As DataRow In DtValoresSerie.Rows
                                                rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(fila("CodigoPortafolioSO"), _
                                                                                                                    Convert.ToDateTime(fechaCajaCadena).AddDays(-1))
                                                If rowVCuotaHistorico Is Nothing Then Exit For
                                            Next
                                        End If
                                    Else
                                        If oRow.CodigoPortafolioSisOpe.Trim <> String.Empty Then
                                            rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(oRow.CodigoPortafolioSisOpe, _
                                                                                                                Convert.ToDateTime(fechaCajaCadena).AddDays(-1))
                                        End If
                                    End If

                                    Dim nombrePortafolio As String = gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("Portafolio", gvFondos)).Text.Trim()
                                    If rowVCuotaHistorico Is Nothing Then
                                        AlertaJS("No se puede confirmar el pago de comisión del portafolio: " + nombrePortafolio + _
                                                 ", primero se debe cerrar el Valor Cuota del día: " + Convert.ToDateTime(fechaCajaCadena).AddDays(-1).Date.ToShortDateString)
                                        Exit Sub
                                    End If


                                    Dim identificador As Int32 = Convert.ToInt32(gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("Id", gvFondos)).Text.Trim())
                                    Dim codigoMoneda As String = gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", gvFondos)).Text.Trim()
                                    Dim periodo As String = gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("Periodo", gvFondos)).Text.Trim()

                                    'Dim FechaCaja As Decimal = Decimal.Parse(gvFondos.Rows(iCont).Cells(obtenerIndiceColumna_Grilla("FechaCaja", gvFondos)).Text.Trim())
                                    Dim obj As New PagoFechaComisionBE
                                    obj.Id = identificador
                                    obj.Estado = "CON"
                                    obj.NumeroCuenta = ddlFilaNumeroCuenta.SelectedValue
                                    obj.CodigoBanco = ddlFilaBanco.SelectedValue
                                    obj.CodigoBancoAdministradora = ddlBancoAdministradora.SelectedValue
                                    obj.NumeroCuentaAdministradora = ddlNumeroDeCuentaAdministradora.SelectedValue
                                    obj.CodigoPortafolioSBS = codigoPortafolio
                                    obj.FechaConfirmacion = UIUtility.ConvertirFechaaDecimal(Date.Now.ToString("dd/MM/yyyy"))
                                    Dim objBd As New PagoFechaComisionBE(pagoFechaComisionBM.Seleccionar(obj.Id, codigoPortafolio, ddlFilaNumeroCuenta.SelectedValue))
                                    If objBd.Id <> 0 Then
                                        obj.CodigoMoneda = objBd.CodigoMoneda
                                        obj.ComisionAcumulada = objBd.ComisionAcumulada
                                        obj.SaldoDisponible = objBd.SaldoOnline
                                        obj.FechaPago = objBd.FechaCajaOperaciones
                                        'fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & nombrePortafolio & " para el periodo  " & periodo & "  por el monto " & codigoMoneda & " " & Format(Convert.ToDecimal(CType(obj.ComisionAcumulada, String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.0000") & "</li>"
                                        fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & nombrePortafolio & " | Periodo: " & periodo & " | Monto: " & codigoMoneda & " " & Format(Convert.ToDecimal(CType(obj.ComisionAcumulada, String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.00") & " | Liquidación: " & fechaCajaCadena & " </li>"
                                        'fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & filaPadre("Portafolio").ToString() & " | Periodo: " & filaPadre("Periodo").ToString() & " | Monto: " & filaPadre("CodigoMoneda").ToString() & " " & Format(Convert.ToDecimal(CType(filaPadre("Comision"), String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.00") & " | Liquidación: " & filaPadre("FechaCajaCadena") & " </li>"

                                    End If

                                    If objBd.SaldoOnline <= objBd.ComisionAcumulada Then
                                        obj.TieneError = True
                                        mensajeError = mensajeError + "<br>-" + nombrePortafolio + ". Saldo:" + objBd.SaldoOnline.ToString() + " | Comision " + objBd.ComisionAcumulada.ToString()
                                    End If
                                    listaFechas.Add(obj)
                                End If
                            End If
                        End If
                        iCont = iCont - 1
                    End While

                    If mensajeError <> "" Then
                        mensajeError = "Existe una diferencia en el saldo de las cuentas de los fondos: " + mensajeError
                        AlertaJS(mensajeError)
                        Exit Sub
                    End If


                    If listaFechas.Count > 0 Then
                        pagoFechaComisionBM.Confirmar(listaFechas, DatosRequest)
                        'Realizar confirmacion                       
                        AlertaJS("Se realizó la confirmación de los pagos de comisión seleccionados.")
                        Buscar()
                        If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                            EnviarCorreoNotificacion("Confirmación", "confirmado", fondosConfirmadosCorreo, "CONFIRMAR")
                        End If


                    End If

                Else
                    AlertaJS("Seleccione al menos un pago de comisión para confirmar.")
                End If

            End If




        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error en la Confirmación/ " + ex.Message.ToString, "'", String.Empty))
        End Try

    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As System.EventArgs) Handles btnEliminar.Click

        Try

            If hdEliminarConfirmar.Value = "SI" Then
                hdEliminarConfirmar.Value = "NO"
                Dim rootPath As String = Request.Url.Host
                Dim filaPadre As DataRow = (From dr In dtFondosSesion
                                  Where (dr("Id") = lblIdentificador.Text)
                                       Select dr
                         ).FirstOrDefault()

                If filaPadre IsNot Nothing Then
                    Dim fondosConfirmadosCorreo As String = ""
                    'fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & filaPadre("Portafolio").ToString() & " para el periodo  " & filaPadre("Periodo").ToString() & "  por el monto " & filaPadre("CodigoMoneda").ToString() & " " & Format(Convert.ToDecimal(CType(filaPadre("ComisionAcumulada"), String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.0000") & "</li>"
                    fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & filaPadre("Portafolio").ToString() & " | Periodo: " & filaPadre("Periodo").ToString() & " | Monto: " & filaPadre("CodigoMoneda").ToString() & " " & Format(Convert.ToDecimal(CType(filaPadre("ComisionAcumulada"), String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.00") & " | Liquidación: " & filaPadre("FechaSolicitudCadena") & " </li>"

                    'Estaba pendiente
                    If filaPadre("CodigoEstado") = "ING" Then

                        pagoFechaComisionBM.Eliminar(Convert.ToInt32(filaPadre("Id")))

                        If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                            EnviarCorreoNotificacion("Eliminación de solicitud", "eliminado", fondosConfirmadosCorreo, "ELIMINAR")
                        End If


                    Else
                        'Estado Confirmado
                        filaPadre("CodigoEstado") = "ING"

                        Dim listaRegistros As New List(Of PagoFechaComisionBE)
                        Dim obj As New PagoFechaComisionBE
                        obj.Id = Convert.ToInt32(filaPadre("Id"))
                        obj.Estado = "ING"
                        obj.CodigoPortafolioSBS = filaPadre("CodigoPortafolioSBS")

                        Dim mensajeConfirmacion As String = pagoFechaComisionBM.EliminarConfirmado(obj.Id, obj.CodigoPortafolioSBS, DatosRequest)

                        If mensajeConfirmacion = "" Then

                            If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                                EnviarCorreoNotificacion("Eliminación de confirmación", "desconfirmado", fondosConfirmadosCorreo, "ELIMINAR")
                            End If
                            AlertaJS("Se reversó el pago de comisión del portafolio seleccionado a estado ingresado")

                        Else
                            AlertaJS(mensajeConfirmacion)
                        End If
                    End If
                    Buscar()
                End If
            End If

        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error en la Eliminación/ " + ex.Message.ToString, "'", String.Empty))
        End Try
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Buscar()
    End Sub

    Protected Sub gvFondos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFondos.RowCommand
        Dim iCont As Int64 = gvFondos.Rows.Count - 1
        Dim Row As GridViewRow
        Dim i As Int32 = 0
        If e.CommandName = "Eliminar" Then
            Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            i = Row.RowIndex
            Dim nombreFondoSeleccionado As String = gvFondos.Rows(i).Cells(obtenerIndiceColumna_Grilla("Portafolio", gvFondos)).Text
            Dim codigoFondoSeleccionado As String = gvFondos.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondos)).Text
            Dim identificadorSeleccionado As String = gvFondos.Rows(i).Cells(obtenerIndiceColumna_Grilla("Id", gvFondos)).Text
            lblFondoEliminar.Text = codigoFondoSeleccionado
            lblIdentificador.Text = identificadorSeleccionado
            ConfirmarJS("¿Está seguro de eliminar la fecha de cobro para el fondo " + nombreFondoSeleccionado + "?", "document.getElementById('hdEliminarConfirmar').value = 'SI'; document.getElementById('btnEliminar').click(); ")

        End If
    End Sub
    Protected Sub gvFondos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFondos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ddlBanco = CType(e.Row.FindControl("ddlBanco"), DropDownList)
            hdCodigoEstado = CType(e.Row.FindControl("hdCodigoEstado"), HiddenField)
            hdCodigoBanco = CType(e.Row.FindControl("hdCodigoBanco"), HiddenField)
            ddlNumeroDeCuenta = CType(e.Row.FindControl("ddlNumeroDeCuenta"), DropDownList)
            hdNumeroDeCuenta = CType(e.Row.FindControl("hdNumeroDeCuenta"), HiddenField)
            cbSelect = CType(e.Row.FindControl("chkSelectPE"), CheckBox)
            ibBorrar = CType(e.Row.FindControl("ibBorrar"), ImageButton)

            hdCodigoFondo = CType(e.Row.FindControl("hdCodigoFondo"), HiddenField)
            hdCodigoMoneda = CType(e.Row.FindControl("hdCodigoMoneda"), HiddenField)
            Dim dtBanco As DataTable = pagoFechaComisionBM.ListarBancos(hdCodigoFondo.Value, hdCodigoMoneda.Value)

            HelpCombo.LlenarComboBox(ddlBanco, dtBanco, "CodigoBanco", "Descripcion", False)
            ddlBanco.Attributes.Add("Fondo", hdCodigoFondo.Value)
            ddlBanco.Attributes.Add("Moneda", hdCodigoMoneda.Value)
            ddlBanco.Attributes.Add("Indice", e.Row.RowIndex)

            ddlBanco.SelectedValue = hdCodigoBanco.Value

            Dim dtNumeroDeCuenta As DataTable = pagoFechaComisionBM.ListarNumeroDeCuentas(hdCodigoFondo.Value, hdCodigoMoneda.Value, hdCodigoBanco.Value)
            HelpCombo.LlenarComboBox(ddlNumeroDeCuenta, dtNumeroDeCuenta, "NumeroCuenta", "NombreNumeroDeCuenta", False)
            ddlNumeroDeCuenta.SelectedValue = hdNumeroDeCuenta.Value

            If hdCodigoEstado.Value = "CON" Then
                cbSelect.Checked = True
                cbSelect.Enabled = False
                ddlBanco.Enabled = False
                ddlNumeroDeCuenta.Enabled = False
                'ibBorrar.Visible = False
            End If

        End If
    End Sub

    Protected Sub ddlBancoAdministradora_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBancoAdministradora.SelectedIndexChanged
        If ddlBancoAdministradora.SelectedValue = "" Then
            ddlNumeroDeCuentaAdministradora.Items.Clear()
            ddlNumeroDeCuentaAdministradora.Items.Insert(0, New System.Web.UI.WebControls.ListItem("--SELECCIONE--", ""))

        Else

            Dim dtNumeroDeCuenta As DataTable = pagoFechaComisionBM.ListarNumeroDeCuentas(codigoFondoAdministradora, codigoMonedaAdministradora, ddlBancoAdministradora.SelectedValue)
            HelpCombo.LlenarComboBox(ddlNumeroDeCuentaAdministradora, dtNumeroDeCuenta, "NumeroCuenta", "NombreNumeroDeCuenta", True)
            ddlNumeroDeCuentaAdministradora.Focus()
        End If



    End Sub
    Public Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            Dim ddlFilaNumeroDeCuenta As DropDownList
            Dim ddlFilaBanco As DropDownList = CType(sender, DropDownList)
            Dim indice As Integer = ddlFilaBanco.Attributes("Indice")
            Dim monedaFila As String = ddlFilaBanco.Attributes("Moneda")
            Dim fondoFila As String = ddlFilaBanco.Attributes("Fondo")

            ddlFilaNumeroDeCuenta = CType(gvFondos.Rows(indice).FindControl("ddlNumeroDeCuenta"), DropDownList)
            If ddlFilaBanco.SelectedValue = "" Then
                ddlFilaNumeroDeCuenta.Items.Clear()
                ddlFilaNumeroDeCuenta.Items.Insert(0, New System.Web.UI.WebControls.ListItem("--SELECCIONE--", ""))
                ddlFilaNumeroDeCuenta.DataBind()
                ddlFilaBanco.Focus()
            Else
                Dim dtNumeroDeCuenta As DataTable = pagoFechaComisionBM.ListarNumeroDeCuentas(fondoFila, monedaFila, ddlFilaBanco.SelectedValue)
                HelpCombo.LlenarComboBox(ddlFilaNumeroDeCuenta, dtNumeroDeCuenta, "NumeroCuenta", "NombreNumeroDeCuenta", False)
                ddlFilaNumeroDeCuenta.Focus()

            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Page.IsPostBack Then
            Session("dtPagosComisionCustodioSesion") = Nothing
            hdRptaConfirmar.Value = "NO"
            CargarPagina()
        End If
    End Sub

#End Region
End Class
