Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports SistemaProcesosBL

Partial Class Modulos_Inversiones_frmRegistrarPagoFechaComision
    Inherits BasePage
    Dim pagoFechaComisionBM As New PagoFechaComisionBM
    Dim parametroGeneralBM As New ParametrosGeneralesBM
    Dim ddlBanco, ddlNumeroDeCuenta As DropDownList
    Dim hdCodigoFondo, hdCodigoMoneda, hdCodigoEstado, hdCodigoBanco, hdNumeroDeCuenta As HiddenField
    Dim cbSelect As CheckBox

#Region "Variables en Sesion"
    Public Property dtFondosSesion() As DataTable
        Get
            If Session("dtPagosComisionSesion") Is Nothing Then
                Return New DataTable
            Else
                Return CType(Session("dtPagosComisionSesion"), DataTable)
            End If
        End Get
        Set(ByVal value As DataTable)
            Session("dtPagosComisionSesion") = value
        End Set
    End Property

#End Region
    
#Region "Metodos"
    Function MensajeValidacionAgregar(ByVal bBanco As Boolean, ByVal sBanco As String, ByVal bNumeroCuenta As Boolean, ByVal sNumeroCuenta As String, ByVal bSaldo As Boolean, ByVal sSaldo As String, ByVal bComision As Boolean, ByVal sComision As String, ByVal bValorCuotaOperacion As Boolean, ByVal sValorCuotaOperacion As String) As String
        Dim mensajeValidacion As String
        mensajeValidacion = ""

        If bBanco = True Then
            mensajeValidacion = mensajeValidacion + "Seleccionar banco para: "
            mensajeValidacion = mensajeValidacion + sBanco
        End If

        If bNumeroCuenta = True Then
            If mensajeValidacion <> "" Then
                mensajeValidacion = mensajeValidacion + "<br>"
            End If

            mensajeValidacion = mensajeValidacion + "Seleccionar número de cuenta: "
            mensajeValidacion = mensajeValidacion + sNumeroCuenta
        End If

        If bSaldo = True Then
            If mensajeValidacion <> "" Then
                mensajeValidacion = sSaldo + "<br>"
            End If

            mensajeValidacion = mensajeValidacion + "No existe saldo suficiente para el pago de las cuentas seleccionadas para los fondos: "
            mensajeValidacion = mensajeValidacion + sSaldo
        End If

        If bComision = True Then
            If mensajeValidacion <> "" Then
                mensajeValidacion = mensajeValidacion + "<br>"
            End If

            mensajeValidacion = mensajeValidacion + "La comision de los fondos debe ser mayor a 0. Los siguientes fondos tienen comision 0: "
            mensajeValidacion = mensajeValidacion + sComision
        End If

        If bValorCuotaOperacion = True Then
            If mensajeValidacion <> "" Then
                mensajeValidacion = mensajeValidacion + "<br>"
            End If

            mensajeValidacion = mensajeValidacion + "Se necesita realizar el proceso de valor cuota en los fondos: "
            mensajeValidacion = mensajeValidacion + sValorCuotaOperacion
        End If


        Return mensajeValidacion
    End Function
    Function ObtenerDescripcionEstado(ByVal CodigoEstado As String) As String

        If ViewState("EstadosFechaCobro") Is Nothing Then
            ViewState("EstadosFechaCobro") = parametroGeneralBM.Listar("FECHACOBRO_ESTADO", DatosRequest)
        End If

        Dim dtEstadosFechaCobro As DataTable = (ViewState("EstadosFechaCobro"))

        Dim nombre As String = (From dr In dtEstadosFechaCobro
                       Where dr("Valor") = CodigoEstado
                       Select dr("Nombre")
                       ).FirstOrDefault()
        Return nombre
    End Function
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
    Private Function crearFilaSeleccionada(ByVal filaOrigen As DataRow, ByVal filaDestino As DataRow) As DataRow

        Dim cantidadColumnas As Integer = filaOrigen.Table.Columns.Count

        For i = 0 To cantidadColumnas - 1
            filaDestino(i) = filaOrigen(i)
        Next


        Return filaDestino
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

    Private Sub displayBotones(ByVal divNombre As System.Web.UI.HtmlControls.HtmlGenericControl, ByVal opcion As String)
        divNombre.Attributes.Add("style", "text-align:right;display:" + opcion)
    End Sub
    Private Sub CargarFondosPendientes(ByVal dtFondos As DataTable)
        Dim dtAux As DataTable = dtFondos.Clone()
        dtAux.Clear()

        If dtFondos.Select("CodigoEstado NOT IN ('DIS')").Count() > 0 Then
            dtAux = dtFondos.Select("CodigoEstado NOT IN ('DIS')").CopyToDataTable()
        Else
            dtAux = dtFondos.Clone()
        End If

        gvFondosPendientes.DataSource = dtAux
        gvFondosPendientes.DataBind()
        BloquearFondosSeleccionados()

    End Sub
    Private Sub BloquearFondosSeleccionados()
        Dim totalFondosSeleccionados = gvFondosPendientes.Rows.Count


        'recorro los seleccionados

        'Bloquear este fondo en el padre
        For j = 0 To gvFondos.Rows.Count - 1
            Dim codigoPortafolio As String = gvFondos.Rows(j).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondos)).Text.Trim()
            Dim fechaIncio As Decimal = Convert.ToDecimal(gvFondos.Rows(j).Cells(obtenerIndiceColumna_Grilla("FechaInicio", gvFondos)).Text.Trim())
            Dim fechaFin As Decimal = Convert.ToDecimal(gvFondos.Rows(j).Cells(obtenerIndiceColumna_Grilla("FechaFin", gvFondos)).Text.Trim())
            Dim diferencia As Decimal = fechaFin - fechaIncio
            Dim checkPadre As CheckBox = CType(gvFondos.Rows(j).FindControl("chkSelectPE"), CheckBox)
            Dim ddlBancoPadre As DropDownList = CType(gvFondos.Rows(j).FindControl("ddlBanco"), DropDownList)
            Dim ddlNumeroDeCuentaPadre As DropDownList = CType(gvFondos.Rows(j).FindControl("ddlNumeroDeCuenta"), DropDownList)
            If (diferencia < 1) Then
                checkPadre.Checked = False
                checkPadre.Enabled = False
                ddlBancoPadre.Enabled = False
                ddlNumeroDeCuentaPadre.Enabled = False

            End If
            For i = 0 To gvFondosPendientes.Rows.Count - 1
                Dim codigoPortafolioSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondosPendientes)).Text.Trim()


                If codigoPortafolio = codigoPortafolioSeleccionado Then
                    checkPadre.Checked = True
                    checkPadre.Enabled = False
                    ddlBancoPadre.Enabled = False
                    ddlNumeroDeCuentaPadre.Enabled = False

                End If
            Next
        Next


        ''recorro los seleccionados
        'For i = 0 To gvFondosPendientes.Rows.Count - 1
        '    Dim codigoPortafolioSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondosPendientes)).Text.Trim()

        '    'Bloquear este fondo en el padre
        '    For j = 0 To gvFondos.Rows.Count - 1
        '        Dim codigoPortafolio As String = gvFondos.Rows(j).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondos)).Text.Trim()
        '        Dim fechaIncio As Decimal = Convert.ToDecimal(gvFondos.Rows(j).Cells(obtenerIndiceColumna_Grilla("FechaInicio", gvFondos)).Text.Trim())
        '        Dim fechaFin As Decimal = Convert.ToDecimal(gvFondos.Rows(j).Cells(obtenerIndiceColumna_Grilla("FechaFin", gvFondos)).Text.Trim())
        '        Dim diferencia As Decimal = fechaFin - fechaIncio
        '        If codigoPortafolio = codigoPortafolioSeleccionado Or (diferencia < 1) Then
        '            Dim checkPadre As CheckBox = CType(gvFondos.Rows(j).FindControl("chkSelectPE"), CheckBox)
        '            Dim ddlBancoPadre As DropDownList = CType(gvFondos.Rows(j).FindControl("ddlBanco"), DropDownList)
        '            Dim ddlNumeroDeCuentaPadre As DropDownList = CType(gvFondos.Rows(j).FindControl("ddlNumeroDeCuenta"), DropDownList)
        '            checkPadre.Checked = True
        '            checkPadre.Enabled = False
        '            ddlBancoPadre.Enabled = False
        '            ddlNumeroDeCuentaPadre.Enabled = False

        '        End If
        '    Next
        'Next
    End Sub
    Private Sub EnviarCorreoNotificacion(ByVal accionTitulo As String, ByVal accionDescripcion As String, ByVal descripcion As String, ByVal accion As String)
        Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String)
        dict.Add("@Fondos", descripcion)
        dict.Add("@AccionTitulo", accionTitulo)
        dict.Add("@AccionDescripcion", accionDescripcion)
        Dim paraCorreo As String = parametroGeneralBM.SeleccionarPorFiltro("FECHACOBRO_EMAIL_" + accion, "", "PARA", "", DatosRequest).Rows(0)("Nombre")
        Dim copiaCorreo As String = parametroGeneralBM.SeleccionarPorFiltro("FECHACOBRO_EMAIL_" + accion, "", "COPIA", "", DatosRequest).Rows(0)("Nombre")
        UIUtility.EnviarMailPlantilla(paraCorreo, copiaCorreo, accionTitulo & " de pago de comisión", "PagoComision.html", dict, DatosRequest)
    End Sub
    Private Sub Buscar(Optional ByVal dtPendientes As DataTable = Nothing)
        If Session("FechaCorteBuscar") = Nothing Then
            Session("FechaCorteBuscar") = UIUtility.ConvertirFechaaDecimal(txtFechaCobro.Text)
        End If

        Dim dtFondos As DataTable = pagoFechaComisionBM.ListarPortafolios(Convert.ToDecimal(Session("FechaCorteBuscar")))
        For Each row As DataRow In dtFondos.Rows
            row("Comision") = Decimal.Parse(row("Comision")) - ObtenerDevolucionComisionUnificada(UIUtility.ConvertirDecimalAStringFormatoFecha(row("FechaInicio")), _
                                                                                                  UIUtility.ConvertirDecimalAStringFormatoFecha(row("FechaFin")), _
                                                                                                  row("Portafolio"))

        Next
        Me.dtFondosSesion = dtFondos

        If Me.dtFondosSesion.Rows.Count = 0 Then

        End If

        If dtPendientes IsNot Nothing Then
            'Actualizar el tabla seleccionados sesion
            Dim totalRecorrer = dtPendientes.Rows.Count - 1

            For i = 0 To totalRecorrer
                Dim filaPadre As DataRow = (From dr In dtFondosSesion
                            Where (dr("CodigoPortafolioSBS") = dtPendientes(i)("CodigoPortafolioSBS"))
                                 Select dr
                   ).FirstOrDefault()
                filaPadre("CodigoEstado") = dtPendientes(i)("CodigoEstado")
                filaPadre("CodigoEstado") = dtPendientes(i)("CodigoEstado")
                filaPadre("NombreEstado") = dtPendientes(i)("NombreEstado")
                filaPadre("CodigoBanco") = dtPendientes(i)("CodigoBanco")
                filaPadre("NumeroDeCuenta") = dtPendientes(i)("NumeroDeCuenta")
                filaPadre("UsuarioSolicitud") = dtPendientes(i)("UsuarioSolicitud")
                filaPadre("NombreBanco") = dtPendientes(i)("NombreBanco")
                filaPadre("NombreNumeroDeCuenta") = dtPendientes(i)("NombreNumeroDeCuenta")
            Next
        End If


        gvFondos.DataSource = dtFondosSesion
        gvFondos.DataBind()
        CargarFondosPendientes(dtFondos)
    End Sub
    Private Sub CargarPagina()
        txtFechaCobro.Text = Date.Now.ToString("dd/MM/yyyy")
        Buscar()
        BloquearFondosSeleccionados()
    End Sub
    Private Function ObtenerDevolucionComisionUnificada(ByVal fechaInicio As String, ByVal fechaFin As String, ByVal nombrePortafolioSBS As String) As Decimal
        Dim ValorCuota As New ValorCuotaBM
        ObtenerDevolucionComisionUnificada = 0
        Dim montoDevolucionComisionUnificada As Decimal = 0D

        Try
            Return ValorCuota.ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe(fechaInicio, _
                                                                                  fechaFin, _
                                                                                  nombrePortafolioSBS)
        Catch ex As Exception
            Return 0
        End Try

    End Function
#End Region

#Region "Eventos de la pagina"
    Protected Sub gvFondosPendientes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFondosPendientes.RowCommand

        Dim iCont As Int64 = gvFondosPendientes.Rows.Count - 1
        Dim Row As GridViewRow
        Dim i As Int32 = 0
        If e.CommandName = "Eliminar" Then
            Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            i = Row.RowIndex
            Dim nombreFondoSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Portafolio", gvFondosPendientes)).Text
            Dim codigoFondoSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondosPendientes)).Text
            Dim identificadorSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Id", gvFondosPendientes)).Text
            lblFondoEliminar.Text = codigoFondoSeleccionado
            lblIdentificador.Text = identificadorSeleccionado
            ConfirmarJS("¿Está seguro de eliminar el pago de la comisión para el portafolio " + nombreFondoSeleccionado + "?", "document.getElementById('hdEliminarConfirmar').value = 'SI'; document.getElementById('btnEliminar').click(); ")

        End If
    End Sub
    Protected Sub gvFondos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFondos.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ddlBanco = CType(e.Row.FindControl("ddlBanco"), DropDownList)
            hdCodigoFondo = CType(e.Row.FindControl("hdCodigoFondo"), HiddenField)
            hdCodigoMoneda = CType(e.Row.FindControl("hdCodigoMoneda"), HiddenField)
            cbSelect = CType(e.Row.FindControl("chkSelectPE"), CheckBox)
            hdCodigoEstado = CType(e.Row.FindControl("hdCodigoEstado"), HiddenField)
            ddlNumeroDeCuenta = CType(e.Row.FindControl("ddlNumeroDeCuenta"), DropDownList)
            hdCodigoBanco = CType(e.Row.FindControl("hdCodigoBanco"), HiddenField)
            hdNumeroDeCuenta = CType(e.Row.FindControl("hdNumeroDeCuenta"), HiddenField)
            Dim dtBanco As DataTable = pagoFechaComisionBM.ListarBancos(hdCodigoFondo.Value, hdCodigoMoneda.Value)

            HelpCombo.LlenarComboBox(ddlBanco, dtBanco, "CodigoBanco", "Descripcion", True)
            ddlBanco.Attributes.Add("Fondo", hdCodigoFondo.Value)
            ddlBanco.Attributes.Add("Moneda", hdCodigoMoneda.Value)
            ddlBanco.Attributes.Add("Indice", e.Row.RowIndex)

            If ddlBanco.SelectedValue = "" Then
                ddlNumeroDeCuenta.Items.Insert(0, New System.Web.UI.WebControls.ListItem("--SELECCIONE--", ""))


            End If
            If hdCodigoBanco.Value <> "" Then
                ddlBanco.SelectedValue = hdCodigoBanco.Value
            End If

            If hdNumeroDeCuenta.Value <> "" Then
                Dim dtNumeroDeCuenta As DataTable = pagoFechaComisionBM.ListarNumeroDeCuentas(hdCodigoFondo.Value, hdCodigoMoneda.Value, ddlBanco.SelectedValue)
                HelpCombo.LlenarComboBox(ddlNumeroDeCuenta, dtNumeroDeCuenta, "NumeroCuenta", "NombreNumeroDeCuenta", True)
                ddlNumeroDeCuenta.SelectedValue = hdNumeroDeCuenta.Value
            End If

            If hdCodigoEstado.Value <> "DIS" Then
                ddlBanco.SelectedValue = hdCodigoBanco.Value
                ddlNumeroDeCuenta.SelectedValue = hdNumeroDeCuenta.Value
            End If

        End If
    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As System.EventArgs) Handles btnAgregar.Click
        Dim nSeleccionadas As Int64 = 0, iCont As Int64 = gvFondos.Rows.Count - 1
        Dim chk As CheckBox
        Dim hdCodigoFondo, hdNombreFondo, hdFechaInicioFila, hdFechaFinFila As HiddenField
        Dim ddlBancoGrilla As DropDownList
        Dim ddlNumeroDeCuentaGrilla As DropDownList
        Dim mensajeValidacion, faltaBancos, faltaNumeroCuentas, faltaSaldo, faltaComision, faltaValorCuotaOperacion As String

        mensajeValidacion = String.Empty
        faltaBancos = String.Empty
        faltaNumeroCuentas = String.Empty
        faltaSaldo = String.Empty
        faltaComision = String.Empty
        faltaValorCuotaOperacion = String.Empty
      
        Dim faltaSeleccionarBanco, faltaSeleccionarNumeroDeCuenta, faltaSeleccionarSaldo, faltaSeleccionarComision, faltaSeleccionarValorCuotaOperacion As Boolean
        faltaSeleccionarNumeroDeCuenta = False
        faltaSeleccionarBanco = False
        faltaSeleccionarSaldo = False
        faltaSeleccionarComision = False
        faltaSeleccionarValorCuotaOperacion = False
        nSeleccionadas = retornarNumSeleccionados(gvFondos, "chkSelectPE")
        If nSeleccionadas > 0 Then

            Dim dtFondosSeleccionados As DataTable = dtFondosSesion.Copy()
            dtFondosSeleccionados.Rows.Clear()

            While iCont >= 0
                Dim objOperaciones As New PrecierreBO
                If gvFondos.Rows(iCont).FindControl("chkSelectPE").GetType Is GetType(CheckBox) Then
                    chk = CType(gvFondos.Rows(iCont).FindControl("chkSelectPE"), CheckBox)
                    hdCodigoFondo = CType(gvFondos.Rows(iCont).FindControl("hdCodigoFondo"), HiddenField)
                    hdNombreFondo = CType(gvFondos.Rows(iCont).FindControl("hdNombreFondo"), HiddenField)
                    hdFechaInicioFila = CType(gvFondos.Rows(iCont).FindControl("hdFechaInicio"), HiddenField)
                    hdFechaFinFila = CType(gvFondos.Rows(iCont).FindControl("hdFechaFin"), HiddenField)
                    Dim diferencia As Decimal = Decimal.Parse(hdFechaFinFila.Value) - Decimal.Parse(hdFechaInicioFila.Value)
                    If chk.Checked = True And diferencia > 0 Then
                        Dim filaFondoSeleccionado As DataRow = (From dr In dtFondosSesion
                       Where dr("CodigoPortafolioSBS") = hdCodigoFondo.Value Select dr
                       ).FirstOrDefault()

                        'validar si ha seleccionado banco y numero de cuenta
                        ddlBancoGrilla = CType(gvFondos.Rows(iCont).FindControl("ddlBanco"), DropDownList)
                        ddlNumeroDeCuentaGrilla = CType(gvFondos.Rows(iCont).FindControl("ddlNumeroDeCuenta"), DropDownList)
                        If (ddlBancoGrilla.SelectedValue = "" And filaFondoSeleccionado("CodigoEstado") <> "PEN") Then
                            faltaSeleccionarBanco = True
                            faltaBancos = faltaBancos + "<br> -" + hdNombreFondo.Value
                        End If

                        'Validacion por Comision igual 0
                        If Decimal.Parse(filaFondoSeleccionado("Comision")) = 0 Then
                            faltaComision = faltaComision + "<br> -" + hdNombreFondo.Value
                            faltaSeleccionarComision = True
                        End If

                        'Validar si se realizo valor cuota en operaciones
                        Dim oPortafolioBE As PortafolioBE
                        Dim oRow As PortafolioBE.PortafolioRow
                        Dim oPortafolioBM As New PortafolioBM
                        Dim rowVCuotaHistorico As DataRow = Nothing
                        oPortafolioBE = oPortafolioBM.Seleccionar(filaFondoSeleccionado("CodigoPortafolioSBS"), Me.DatosRequest)
                        oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
                        If oRow.PorSerie.Equals("S") Then
                            Dim DtValoresSerie As DataTable
                            Dim oPortafolio As New PortafolioBM

                            DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie(filaFondoSeleccionado("CodigoPortafolioSBS"))
                            If DtValoresSerie.Rows.Count > 0 Then
                                For Each fila As DataRow In DtValoresSerie.Rows
                                    rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(fila("CodigoPortafolioSO"), Convert.ToDateTime(UIUtility.ConvertirFechaaString(Decimal.Parse(hdFechaFinFila.Value))))
                                    If rowVCuotaHistorico IsNot Nothing Then Exit For
                                Next
                            End If
                        Else
                            If oRow.CodigoPortafolioSisOpe.Trim <> String.Empty Then
                                rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(oRow.CodigoPortafolioSisOpe, Convert.ToDateTime(UIUtility.ConvertirFechaaString(Decimal.Parse(hdFechaFinFila.Value))))
                            End If
                        End If

                        If rowVCuotaHistorico Is Nothing Then
                            faltaValorCuotaOperacion = faltaValorCuotaOperacion + "<br> -" + hdNombreFondo.Value
                            faltaSeleccionarValorCuotaOperacion = True
                        End If

                        Dim filaNueva As DataRow = crearFilaSeleccionada(filaFondoSeleccionado, dtFondosSeleccionados.NewRow)
                        filaNueva("UsuarioSolicitud") = UIUtility.ObtenerValorRequest(DatosRequest, "Usuario")
                        If filaFondoSeleccionado("CodigoEstado") = "DIS" Then
                            filaNueva("NombreEstado") = ObtenerDescripcionEstado("PEN")
                            filaNueva("CodigoEstado") = "PEN"
                        End If
                        filaNueva("NombreBanco") = ddlBancoGrilla.SelectedItem.Text
                        filaNueva("CodigoBanco") = ddlBancoGrilla.SelectedItem.Value
                        filaNueva("NumeroDeCuenta") = ddlNumeroDeCuentaGrilla.SelectedItem.Value
                        filaNueva("NombreNumeroDeCuenta") = ddlNumeroDeCuentaGrilla.SelectedItem.Text
                        dtFondosSeleccionados.Rows.Add(filaNueva)

                        If (ddlNumeroDeCuentaGrilla.SelectedValue = "" And filaFondoSeleccionado("CodigoEstado") <> "PEN") Then
                            faltaSeleccionarNumeroDeCuenta = True
                            faltaNumeroCuentas = faltaNumeroCuentas + "<br> -" + hdNombreFondo.Value
                        Else
                            Dim saldo As String() = ddlNumeroDeCuentaGrilla.SelectedItem.Text.Split("|")
                            If Decimal.Parse(filaNueva("Comision")) > Decimal.Parse(saldo(1).Trim()) Then
                                faltaSaldo = faltaSaldo + "<br> -" + hdNombreFondo.Value
                                faltaSeleccionarSaldo = True
                            End If
                        End If
                    End If

                End If
                iCont = iCont - 1
            End While

            mensajeValidacion = MensajeValidacionAgregar(faltaSeleccionarBanco, faltaBancos, faltaSeleccionarNumeroDeCuenta, faltaNumeroCuentas, faltaSeleccionarSaldo, faltaSaldo, faltaSeleccionarComision, faltaComision, faltaSeleccionarValorCuotaOperacion, faltaValorCuotaOperacion)
          
            'Validar los combos seleccionados
            If mensajeValidacion = String.Empty Then
                dtFondosSeleccionados.DefaultView.Sort = "CodigoEstado DESC"
                gvFondosPendientes.DataSource = dtFondosSeleccionados
                gvFondosPendientes.DataBind()
                Dim totalSeleccionados As Int32 = (dtFondosSeleccionados.Rows.Count - 1)

                'Actualizar el tabla seleccionados sesion
                For i = 0 To totalSeleccionados
                    Dim filaPadre As DataRow = (From dr In dtFondosSesion
                                Where (dr("CodigoPortafolioSBS") = dtFondosSeleccionados(i)("CodigoPortafolioSBS"))
                                     Select dr
                       ).FirstOrDefault()
                    filaPadre("CodigoEstado") = dtFondosSeleccionados(i)("CodigoEstado")
                    filaPadre("NombreEstado") = dtFondosSeleccionados(i)("NombreEstado")
                    filaPadre("CodigoBanco") = dtFondosSeleccionados(i)("CodigoBanco")
                    filaPadre("NumeroDeCuenta") = dtFondosSeleccionados(i)("NumeroDeCuenta")
                    filaPadre("UsuarioSolicitud") = dtFondosSeleccionados(i)("UsuarioSolicitud")
                    filaPadre("NombreBanco") = dtFondosSeleccionados(i)("NombreBanco")
                    filaPadre("NombreNumeroDeCuenta") = dtFondosSeleccionados(i)("NombreNumeroDeCuenta")
                Next

                BloquearFondosSeleccionados()
            Else
                AlertaJS(mensajeValidacion)
            End If
        Else
            AlertaJS("Seleccione al menos un fondo")
            Exit Sub

        End If
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Dim rootPath As String = Request.Url.Host
            If hdRptaConfirmar.Value = "NO" Then
                ConfirmarJS("¿Desea Ingresar el pago de comisiones de estado pendiente?", "document.getElementById('hdRptaConfirmar').value = 'SI';document.getElementById('btnIngresar').click(); ")
                Exit Sub
            Else
                hdRptaConfirmar.Value = "NO"
                Dim iCont = gvFondosPendientes.Rows.Count - 1
                Dim fondosIngresadosCorreo As String = ""
                Dim dtAuxiliar As DataTable = dtFondosSesion

                If gvFondosPendientes.Rows.Count = 0 Then
                    AlertaJS("No existe pago de comisiones pendientes.")
                    Exit Sub
                End If
                Dim listaFechas As New List(Of PagoFechaComisionBE)
                For i = 0 To gvFondosPendientes.Rows.Count - 1
                    Dim codigoPortafolioSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondosPendientes)).Text.Trim()
                    Dim codigoEstadoSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoEstado", gvFondosPendientes)).Text.Trim()
                    Dim identificadorSeleccionado As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Id", gvFondosPendientes)).Text.Trim()
                    Dim nombrePortafolio As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Portafolio", gvFondosPendientes)).Text.Trim()
                    Dim periodo As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Periodo", gvFondosPendientes)).Text.Trim()
                    Dim codigoMoneda As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", gvFondosPendientes)).Text.Trim()
                    If codigoEstadoSeleccionado = "PEN" Then
                        Dim obj As New PagoFechaComisionBE
                        obj.CodigoPortafolioSBS = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolioSBS", gvFondosPendientes)).Text.Trim()
                        obj.Estado = "ING"
                        obj.NumeroCuenta = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroDeCuenta", gvFondosPendientes)).Text.Trim()
                        obj.CodigoBanco = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoBanco", gvFondosPendientes)).Text.Trim()
                        obj.UsuarioSolicitud = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("UsuarioSolicitud", gvFondosPendientes)).Text.Trim()
                        obj.FechaInicio = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaInicio", gvFondosPendientes)).Text.Trim()
                        obj.FechaFin = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaFin", gvFondosPendientes)).Text.Trim()
                        obj.ComisionAcumulada = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Comision", gvFondosPendientes)).Text.Trim()
                        obj.SaldoDisponible = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("Saldo", gvFondosPendientes)).Text.Trim()
                        'obj.FechaSolicitud = UIUtility.ConvertirFechaaDecimal(Date.Now.ToString("dd/MM/yyyy"))
                        obj.FechaCajaOperaciones = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaCaja", gvFondosPendientes)).Text.Trim()
                        Dim fechaCajaCadena As String = gvFondosPendientes.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaCajaCadena", gvFondosPendientes)).Text.Trim()
                        'fondosIngresadosCorreo = fondosIngresadosCorreo + "<li>" & nombrePortafolio & " para el periodo  " & periodo & "  por el monto " & codigoMoneda & " " & Format(Convert.ToDecimal(CType(obj.ComisionAcumulada, String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.0000") & "</li>"
                        fondosIngresadosCorreo = fondosIngresadosCorreo + "<li>" & nombrePortafolio & " | Periodo: " & periodo & " | Monto: " & codigoMoneda & " " & Format(Convert.ToDecimal(CType(obj.ComisionAcumulada, String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.00") & " | Liquidación: " & fechaCajaCadena & " </li>"
                        'CORTOPD | Periodo: 01/06/2019 – 30/06/2019 | Monto: DOL 48,971.00 |Liquidación: 05/07/2019

                        listaFechas.Add(obj)
                    End If
                Next


                pagoFechaComisionBM.Insertar(listaFechas, DatosRequest)
                Buscar()
                AlertaJS("Se ingresó correctamente el pago de comisión para su confirmación.")

                If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                    EnviarCorreoNotificacion("Solicitud", "solicitado", fondosIngresadosCorreo, "INGRESAR")
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If hdEliminarConfirmar.Value = "SI" Then
            Dim rootPath As String = Request.Url.Host
            'Eliminacion Pendiente
            Dim dtSesion As DataTable = Me.dtFondosSesion

            Dim filaPadre As DataRow = (From dr In dtFondosSesion
                                Where (dr("CodigoPortafolioSBS") = lblFondoEliminar.Text)
                                     Select dr
                       ).FirstOrDefault()

            If filaPadre IsNot Nothing Then

                Dim fondosConfirmadosCorreo As String = ""
                'fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & filaPadre("Portafolio").ToString() & " para el periodo  " & filaPadre("Periodo").ToString() & "  por el monto " & filaPadre("CodigoMoneda").ToString() & " " & Format(Convert.ToDecimal(CType(filaPadre("Comision"), String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.0000") & "</li>"
                ' fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & filaPadre("Portafolio").ToString() & " para el periodo  " & filaPadre("Periodo").ToString() & "  por el monto " & filaPadre("CodigoMoneda").ToString() & " " & Format(Convert.ToDecimal(CType(filaPadre("Comision"), String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.00") & "</li>"
                fondosConfirmadosCorreo = fondosConfirmadosCorreo + "<li>" & filaPadre("Portafolio").ToString() & " | Periodo: " & filaPadre("Periodo").ToString() & " | Monto: " & filaPadre("CodigoMoneda").ToString() & " " & Format(Convert.ToDecimal(CType(filaPadre("Comision"), String).Replace(UIUtility.DecimalSeparator, ".")), "###,###,##0.00") & " | Liquidación: " & filaPadre("FechaCajaCadena") & " </li>"


                'Estaba pendiente
                If filaPadre("CodigoEstado") = "ING" Then
                    pagoFechaComisionBM.Eliminar(Convert.ToInt32(filaPadre("Id")))

                    If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                        EnviarCorreoNotificacion("Eliminación de solicitud", "eliminado", fondosConfirmadosCorreo, "ELIMINAR")
                    End If
                End If

                filaPadre("CodigoEstado") = "DIS"


                Dim dtNuevaSeleccion As DataTable

                If dtFondosSesion.Select("CodigoEstado NOT IN ('DIS')").Count() > 0 Then
                    dtNuevaSeleccion = dtFondosSesion.Select("CodigoEstado NOT IN ('DIS')").CopyToDataTable()
                Else
                    dtNuevaSeleccion = dtFondosSesion.Clone()
                End If

                Buscar(dtNuevaSeleccion)
                'gvFondosPendientes.DataSource = dtNuevaSeleccion
                'gvFondosPendientes.DataBind()
                BloquearFondosSeleccionados()
            End If


            hdEliminarConfirmar.Value = "NO"
        End If
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        'Validar si hay registros pendientes

        'Buscando en estado Pendiente
        Dim dtSesion As DataTable = Me.dtFondosSesion
       
        Dim listaSeleccionados = (From dr In dtFondosSesion
                            Where (dr("CodigoEstado") = "PEN")
                                 Select dr("CodigoEstado")
                   ).ToList()

        If listaSeleccionados.Count > 0 Then
            ConfirmarJS("¿Existen fondos pendientes de ingresar. Desea cambiar la fecha de corte?", "document.getElementById('btnBuscarAuxiliar').click(); ")
            Exit Sub
        Else
            Session("FechaCorteBuscar") = UIUtility.ConvertirFechaaDecimal(txtFechaCobro.Text)

            Buscar()
        End If

    End Sub
    Protected Sub btnBuscarAuxiliar_Click(sender As Object, e As System.EventArgs) Handles btnBuscarAuxiliar.Click
        Session("FechaCorteBuscar") = UIUtility.ConvertirFechaaDecimal(txtFechaCobro.Text)
        Buscar()
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Page.IsPostBack Then
            hdRptaConfirmar.Value = "NO"
            Session("dtPagosComisionSesion") = Nothing
            Session("FechaCorteBuscar") = Nothing
            CargarPagina()
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
                HelpCombo.LlenarComboBox(ddlFilaNumeroDeCuenta, dtNumeroDeCuenta, "NumeroCuenta", "NombreNumeroDeCuenta", True)
                ddlFilaNumeroDeCuenta.Focus()

            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub


#End Region

End Class
