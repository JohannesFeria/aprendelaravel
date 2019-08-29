Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
'Imports System.Globalization
Imports SistemaProcesosBL

Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmSeriesPortafolio
    Inherits BasePage

    Dim oUtil As New UtilDM
    Private RutaDestino As String = String.Empty
    Dim objValorCuotaBE As New ValorCuotaBE
    Dim ValorCuota As New ValorCuotaBM
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    'INICIO | ZOLUXIONES | DACV | SPRINT III - 20180806
    Dim moneda As String = String.Empty
    'FIN | ZOLUXIONES | DACV | SPRINT III - 20180806

    Function TruncateDecimal(ByVal value As Decimal, ByVal precision As Integer) As Decimal
        Dim stepper As Decimal = Math.Pow(10, precision)
        Dim tmp As Decimal = Math.Truncate(stepper * value)
        Return tmp / stepper
    End Function

    'INICIO | ZOLUXIONES | DACV | SPRINT III - 20180806
    Private Function TipoCambio() As DataTable
        Dim vector As New VectorTipoCambioBM
        Dim moneda As String = String.Empty
        Dim dtResult As New DataTable
        Dim tipoCambioFuente As String = IIf(getTipoNegocio() = "MANDA", "SBS", "REAL")

        dtResult = vector.Listar(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString, tipoCambioFuente, "", New DataSet()).Tables(0)
        If dtResult.Rows.Count = 0 Then Throw New System.Exception("No se ha cargado el tipo de cambio " + IIf(tipoCambioFuente = "REAL", "PIP", "SBS") + " del día " + tbFechaOperacion.Text)
        Return dtResult
    End Function

    Private Function ListaDistrucionFlujo() As DataTable
        Dim valorCuota As New ValorCuotaBM
        Dim dtResult As New DataTable
        dtResult = valorCuota.ListarDistribucionFlujo(New DataSet()).Tables(0)
        Return dtResult
    End Function

    Private Function TipoCambioAnterior() As DataTable
        Dim vector As New VectorTipoCambioBM
        Dim moneda As String = String.Empty
        Dim dtResult As New DataTable
        Dim fecha As Date = Convert.ToDateTime(tbFechaOperacion.Text)
        fecha = fecha.AddDays(-1)
        Dim tipoCambioFuente As String = IIf(getTipoNegocio() = "MANDA", "SBS", "REAL")

        dtResult = vector.Listar(UIUtility.ConvertirFechaaDecimal(fecha.ToString("dd/MM/yyyy")).ToString, tipoCambioFuente, "", New DataSet()).Tables(0)
        If dtResult.Rows.Count = 0 Then Throw New System.Exception("No se ha cargado el tipo de cambio " + IIf(tipoCambioFuente = "REAL", "PIP", "SBS") + " del día " + fecha.ToString("dd/MM/yyyy").ToString)
        Return dtResult
    End Function
    'FIN | ZOLUXIONES | DACV | SPRINT III - 20180806

    Sub CargaHiddenCheckFondo()
        Dim dtListaFlujo As DataTable = ListaDistrucionFlujo()
        Dim dtListaSeleccionada As DataRow() = dtListaFlujo.Select("ID = " + hdCodigoPortafolioSisOpe.Value)
        If (dtListaSeleccionada.Count > 0) Then
            hdCheckOperaciones.Value = "1"
        Else
            hdCheckOperaciones.Value = "0"
        End If
    End Sub


    Function getTipoNegocio() As String
        Dim resultado As String = ""
        Dim oPortafolioBM As New PortafolioBM
        Dim dtPortafolio As DataTable = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, DatosRequest).Tables(0)

        If dtPortafolio.Rows.Count > 0 Then
            resultado = dtPortafolio.Rows(0)("TipoNegocio")
        End If


        Return resultado
    End Function

    Sub CargaPantalla()
        If ddlPortafolio.SelectedValue = 0 Then
            AlertaJS("Debe seleccionar un Portafolio")
        Else
            Dim alerta As String = String.Empty
            Dim oPortafolioBE As PortafolioBE
            Dim oPortafolioBM As New PortafolioBM
            Dim oCarteraTituloValoracionBM As New CarteraTituloValoracionBM
            Dim oRow As PortafolioBE.PortafolioRow
            'OT 9851 27/01/2017 - Carlos Espejo
            'Descripcion: Si es seriado el fondo se habilita la caja de Otras CXP.
            'OT 10238 08/05/2017 - Carlos Espejo
            'Descripcion: Lista Inconsistencia de valorización

            'validar cajas
            Dim validacionCajas As String = String.Empty
            validacionCajas = UIUtility.ValidarCajas(ddlPortafolio.SelectedValue, "", UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))

            Dim dt As DataTable = oPortafolioBM.InconsisteciasValorizacion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            If dt.Rows.Count > 0 Then
                Dim strMensaje As New StringBuilder
                strMensaje.Append("<table border = ""1""; style=""width:100%; text-align: center;font-weight: normal;"">")
                strMensaje.Append("<tr><td colspan =""4""><b>Inconsistencias en la valorización</b></td></tr>")
                strMensaje.Append("<tr><td style=""width:20% ;""><b>Nemonico</b></td><td style=""width:30%;""><b>Instrumento</b></td>" + _
                "<td style=""width:30%;""><b>Instumento</b></td><td style=""width:20%;""><b>VPN</b></td></tr>")
                For Each dr As DataRow In dt.Rows
                    strMensaje.Append("<tr><td>" + dr("Nemonico") + "</td><td>" + dr("ISINSBS") + "</td><td>" + dr("TipoInstrumentoSBS") + "</td><td>" + dr("VpnOriginal").ToString + "</td></tr>")
                Next
                strMensaje.Append("</table>")
                'AlertaJS(strMensaje.ToString())
                alerta = strMensaje.ToString()
            End If
            'OT 10238 Fin
            MostrarOcultarFormulario(True)
            oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
            oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            tbCXPotras.ReadOnly = True
            txtChequePendiente.ReadOnly = False
            txtRescatePendiente.ReadOnly = False
            hdSeriado.Value = oRow.PorSerie
            hdCuotasLiberadas.Value = oRow.CuotasLiberadas
            hdCodigoPortafolioSisOpe.Value = oRow.CodigoPortafolioSisOpe 'OT10965 - 30/11/2017 - Ian Pastor M. (Obtener codigo de portafolio de operaciones)
            txtComisionPortafolio.Value = oRow.PorcentajeComision 'Obtener Porcentaje de comision del portafolio para realizar las operaciones                
            hdCPPadreSisOpe.Value = oRow.CPPadreSisOpe
            'Cambiar el nombre de los valores por portafolio - OT 12003 - Junior Huallullo 03/06/2019
            If (oRow.TipoNegocio = "MANDA") Then
                lblAportes.InnerText = "Aportes Mandato"
                lblRescates.InnerText = "Rescates Mandato"
                lblSuscripcion.InnerText = "Aporte Mandato:"
            Else
                lblAportes.InnerText = "Aportes Valores"
                lblRescates.InnerText = "Rescates Valores"
                lblSuscripcion.InnerText = "Suscripción:"
            End If
            'If oRow.VectorPrecioVal.Equals("") Then
            '    AlertaJS("No se encuentra definido el precio vector del portafolio. Aségure de asignarle un valor en el Mantenimiento de Portafolios")
            '    Exit Sub
            'End If
            If validacionCajas <> String.Empty Then
                AlertaJS(validacionCajas)
                '  If hdCuotasLiberadas.Value = "0" Then
                MostrarOcultarFormulario(False)
                Exit Sub
                'End If
            End If
            'OT10927 - 22/11/2017 - Ian Pastor M. Descripción: Valida si existe valorización a la fecha. Si existe, para los portafolios que liberen dividendos
            'no se muestran los controles.
            'OT10927 - 22/11/2017 - Ian Pastor M. Descripción: Valida si existe valorización a la fecha. Si existe, para los portafolios que liberen dividendos
            'no se muestran los controles.
            If oPortafolioBM.ValidarValorizacion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)) = False Then
                AlertaJS("El Portafolio no ha sido valorizado para esta fecha. Sólo se mostrarán los datos para los portafolios que distribuyan montos liberados")
                MostrarOcultarFormulario(False)
                If hdCuotasLiberadas.Value = "0" Then
                    HabilitarBotonDistribucion()
                End If
                Exit Sub
            End If
            'OT10927 - Fin
            'OT 9851 Fin
            HabilitarEscenario(True, oRow.PorSerie)
            CargarValoresCuota()
            CargarValoresIngresados()
            HabilitarControles()
            HabilitarControlesCalculoValorCuota(oRow.TipoCalculoValorCuota)
            If oRow.PorSerie.Equals("S") Then
                dgArchivo.DataSource = oPortafolioBM.Portafolio_Series_Cuotas(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
                dgArchivo.DataBind()
            End If
            btnProcesar.Visible = True
            btnCarga.Visible = True
            cbHabilitarCarga.Visible = True
            'Habilitar carga automática
            If cbHabilitarCarga.Checked Then
                btnCarga.Enabled = False
                iptRuta.Attributes.Add("disabled", "true")
            End If
            'OT10686 - Inicio 10/08/2017. La grilla del ingreso de series tiene que tener la opción a consultar para días anteriores.
            'No se puede editar el valor cuota para fechas anteriores

            'INICIO | PROYECTO SIT | ZOLUXIONES | CRumiche | 2018-08-17
            'Consultamos si ya existe un proceso de Calculo de Valor Cuota en Operaciones (Para la fecha y fondo determinado)

            Dim mostrarFrmSoloLectura As Boolean = False

            If hdCodigoPortafolioSisOpe.Value.Trim.Length > 0 Then ' Si no tenemos el CODIGO DE FONDO OPERACIONES no podemos realizar la ejecucion en OPE
                Dim objOperaciones As New PrecierreBO
                Dim rowVCuotaHistorico As DataRow = Nothing
                If oRow.PorSerie.Equals("S") Then
                    Dim DtValoresSerie As DataTable
                    Dim oPortafolio As New PortafolioBM

                    DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie(ddlPortafolio.SelectedValue)
                    If DtValoresSerie.Rows.Count > 0 Then
                        For Each fila As DataRow In DtValoresSerie.Rows
                            rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(fila("CodigoPortafolioSO"), UIUtility.ConvertirStringaFecha(tbFechaOperacion.Text))
                            If rowVCuotaHistorico IsNot Nothing Then Exit For
                        Next
                    End If
                Else
                    rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(hdCodigoPortafolioSisOpe.Value, UIUtility.ConvertirStringaFecha(tbFechaOperacion.Text))
                End If
                mostrarFrmSoloLectura = (rowVCuotaHistorico IsNot Nothing)

                If oRow.PorSerie.Equals("S") Then
                    btnEjecutarCierreOpe.Visible = False
                    btnSoloLecturaCierreOpe.Visible = False
                Else
                    btnEjecutarCierreOpe.Visible = Not mostrarFrmSoloLectura
                    btnSoloLecturaCierreOpe.Visible = mostrarFrmSoloLectura
                End If

            Else 'Metodo tradicional para determinar el formlario en solo lectura
                If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) < Decimal.Parse(oCarteraTituloValoracionBM.ObtenerFechaValoracion(ddlPortafolio.SelectedValue, "N", "2")) Then
                    mostrarFrmSoloLectura = True
                End If

                btnEjecutarCierreOpe.Visible = False
                btnSoloLecturaCierreOpe.Visible = False
            End If
            MostrarFormularioSoloLectura(mostrarFrmSoloLectura)
            'FIN | PROYECTO SIT | ZOLUXIONES | CRumiche | 2018-08-17
            'OT12126 | rcolonia | Zoluxiones | Se actualiza la fecha de Comision SAFM si está grabado en BD
            If Not CInt(ViewState("ComisionSAFMAnterior")) = 0 And Not btnEjecutarCierreOpe.Visible Then
                txtComisionSAFMAnterior.Text = UIUtility.ConvertirFechaaString(CDec(ViewState("ComisionSAFMAnterior")))
            End If
            'OT12126 - Fin
            'OT10686 - Fin
            If alerta.Length > 0 And Not mostrarFrmSoloLectura Then
                AlertaJS(alerta)
            End If
        End If

    End Sub
    Private Sub DetallePrecierre()
        Try
            Dim cuenta As New CuentaEconomicaBM()
            Dim portafolio As New PortafolioBM
            Dim dt As New DataTable
            Dim fila As DataRow
            saldoIni = 0
            saldoFin = 0
            Dim fechaAyer As Date = Convert.ToDateTime(tbFechaOperacion.Text)
            fechaAyer = fechaAyer.AddDays(-1)

            dt.Columns.Add(New DataColumn("Banco", GetType(String)))
            dt.Columns.Add(New DataColumn("SaldoInicial", GetType(String)))
            dt.Columns.Add(New DataColumn("SaldoLibro", GetType(String)))

            Dim moneda As String = portafolio.PortafolioSelectById(Me.ddlPortafolio.SelectedValue)(0)("CodigoMoneda")

            Dim dtCambioAyer As DataTable = TipoCambioAnterior()

            Dim dtValorMonedaAnterior = dtCambioAyer.Select("CodigoMoneda = 'DOL'").CopyToDataTable
            Dim valorDolaresAnterior As Decimal = Convert.ToDecimal(dtValorMonedaAnterior(0)(4))
            Dim valorSolesAnterior As Decimal = Convert.ToDecimal(dtValorMonedaAnterior(0)(5))

            Dim dtCambio As DataTable = TipoCambio()
            Dim dtValorMoneda = dtCambio.Select("CodigoMoneda = 'DOL'").CopyToDataTable
            Dim valorDolares As Decimal = Convert.ToDecimal(dtValorMoneda(0)(4))
            Dim valorSoles As Decimal = Convert.ToDecimal(dtValorMoneda(0)(5))



            Dim dtRecaudado As DataTable = cuenta.SeleccionarCuentaEconomica(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), "", "", "", "10")
            If dtRecaudado IsNot Nothing And dtRecaudado.Rows.Count > 0 Then
                fila = dt.NewRow
                fila(0) = "RECAUDO"
                fila(1) = "Saldo inicial (día " & fechaAyer.ToShortDateString & ")"
                fila(2) = "Saldo Libro Banco"
                dt.Rows.Add(fila)

                For i = 0 To dtRecaudado.Rows.Count - 1
                    fila = dt.NewRow
                    fila(0) = dtRecaudado.Rows(i)(2) & " (" & dtRecaudado.Rows(i)(6) & ")"
                    If moneda.Equals("NSOL") Then
                        If dtRecaudado.Rows(i)(5) = "DOL" Then
                            fila(1) = dtRecaudado.Rows(i)(7) * valorSolesAnterior
                            fila(2) = dtRecaudado.Rows(i)(8) * valorSoles
                        Else
                            fila(1) = dtRecaudado.Rows(i)(7)
                            fila(2) = dtRecaudado.Rows(i)(8)
                        End If
                    Else
                        If dtRecaudado.Rows(i)(5) = "NSOL" Then
                            fila(1) = dtRecaudado.Rows(i)(7) / valorDolaresAnterior
                            fila(2) = dtRecaudado.Rows(i)(8) / valorDolares
                        Else
                            fila(1) = dtRecaudado.Rows(i)(7)
                            fila(2) = dtRecaudado.Rows(i)(8)
                        End If
                    End If

                    saldoIni += Convert.ToDecimal(fila(1))
                    saldoFin += Convert.ToDecimal(fila(2))
                    dt.Rows.Add(fila)
                Next

            End If

            Dim dtInversiones As DataTable = cuenta.SeleccionarCuentaEconomica(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), "", "", "", "20")
            If dtInversiones IsNot Nothing And dtInversiones.Rows.Count > 0 Then
                fila = dt.NewRow
                fila(0) = "INVERSIONES"
                fila(1) = "Saldo inicial (día " & fechaAyer.ToShortDateString & ")"
                fila(2) = "Saldo Libro Banco"
                dt.Rows.Add(fila)

                For i = 0 To dtInversiones.Rows.Count - 1
                    fila = dt.NewRow
                    fila(0) = dtInversiones.Rows(i)(2) & " (" & dtInversiones.Rows(i)(6) & ")"
                    If moneda.Equals("NSOL") Then
                        If dtInversiones.Rows(i)(5) = "DOL" Then
                            fila(1) = dtInversiones.Rows(i)(7) * valorSolesAnterior
                            fila(2) = dtInversiones.Rows(i)(8) * valorSoles
                        Else
                            fila(1) = dtInversiones.Rows(i)(7)
                            fila(2) = dtInversiones.Rows(i)(8)
                        End If
                    Else
                        If dtInversiones.Rows(i)(5) = "NSOL" Then
                            fila(1) = dtInversiones.Rows(i)(7) / valorDolaresAnterior
                            fila(2) = dtInversiones.Rows(i)(8) / valorDolares
                        Else
                            fila(1) = dtInversiones.Rows(i)(7)
                            fila(2) = dtInversiones.Rows(i)(8)
                        End If
                    End If
                    saldoIni += Convert.ToDecimal(fila(1))
                    saldoFin += Convert.ToDecimal(fila(2))
                    dt.Rows.Add(fila)
                Next
            End If

            fila = dt.NewRow
            fila(0) = ""
            fila(1) = ""
            fila(2) = ""
            dt.Rows.Add(fila)

            fila = dt.NewRow
            fila(0) = "TOTALES"
            fila(1) = saldoIni
            fila(2) = saldoFin
            dt.Rows.Add(fila)

            Me.gvDesagradoCaja.DataSource = dt
            Me.gvDesagradoCaja.DataBind()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try
    End Sub
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                Call CargaPortafolio(ddlPortafolio)
                Session("llamadoAgregarSeries") = "NO"
                Session("ActualizacionAC_VC") = Nothing
                '*******************************************************************************************************
                'OT10916 - 06/11/2017 - Ian Pastor M. La fecha por default debe es la máxima fecha del portafolio
                'y no la fecha de negociación del portafolio MILA.
                '*******************************************************************************************************
                'Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
                'Dim strFecha As String
                'strFecha = oCarteraTituloValoracion.ObtenerFechaValoracion("1", "N", True)
                'If strFecha Is DBNull.Value Or strFecha = "" Then
                '    Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura("1"))
                'Else
                '    Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(strFecha))
                'End If
            Else
                If Session("llamadoAgregarSeries") = "SI" Then
                    Session("llamadoAgregarSeries") = "NO"
                    If CType(Session("ActualizacionAC_VC"), Boolean) Then
                        Session("ActualizacionAC_VC") = Nothing
                        btnProcesar_Click(Nothing, Nothing)
                        btnAceptar_Click(Nothing, Nothing)
                    Else
                        CargaPantalla()
                        DetallePrecierre()
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema")
        End Try
    End Sub
    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista, 0)
        objportafolio = Nothing
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se captura el el BorderColor inicial del campo Inversiones [T] Subtotal | 09/08/18 
        ViewState("tbInversionesSubTotalBorderColor") = tbInversionesSubTotal.BorderColor
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se captura el el BorderColor inicial del campo Inversiones [T] Subtotal | 09/08/18 
    End Sub
    Private Sub MostrarOcultarFormulario(ByVal Estado As String)
        FormInversiones.Visible = Estado
        FormPrecierre.Visible = Estado
        FormCierre.Visible = Estado
        FormSeries.Visible = Estado
        btnProcesar.Visible = Estado
        btnImprimir.Visible = Estado
        btnEjecutarCierreOpe.Visible = Estado
        btnSoloLecturaCierreOpe.Visible = Estado
        btnAceptar.Visible = Estado
    End Sub
    Private Sub HabilitarEscenario(ByVal Estado As Boolean, ByVal TipoFondo As String)
        FormInversiones.Visible = Estado
        FormPrecierre.Visible = Estado
        If TipoFondo.Equals("S") Then
            FormSeries.Visible = Estado
            FilaPrecierre.Visible = Not Estado
            tbValValoresPrecierre.Visible = Not Estado
            cuotapre.Visible = Not Estado
            FormCierre.Visible = Not Estado
            pncom.Visible = False
        Else
            FormCierre.Visible = Estado
            FormSeries.Visible = Not Estado
            tbValValoresPrecierre.Visible = Estado
            cuotapre.Visible = Estado
            pncom.Visible = True
            FilaPrecierre.Visible = Estado
        End If
    End Sub
    'Protected Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
    '    Try
    '        CargaPantalla()
    '        DetallePrecierre()
    '    Catch ex As Exception
    '        AlertaJS(Replace(ex.Message.ToString(), "'", ""))
    '    End Try
    'End Sub
    Private Sub HabilitarControles()
        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) = Decimal.Parse(HdFechaCreacionFondo.Value) Then
            tbCXCtitulo.ReadOnly = False
            txtCXCPrecierre.ReadOnly = False
            tbCXPtitulo.ReadOnly = False
            txtCXPPrecierre.ReadOnly = False
            tbValPatriPrecierre.ReadOnly = False
            tbComiSAFM.ReadOnly = False
            tbValPatriPrecierre2.ReadOnly = False
            tbValValoresPrecierre.ReadOnly = False
            tbCajaCierre.ReadOnly = False
            tbAporteCuota.ReadOnly = False
            tbRescatesCuota.ReadOnly = False
            tbCXCtituloCierre.ReadOnly = False
            tbCXPtituloCierre.ReadOnly = False
            tbValPatCierreCuota.ReadOnly = False
            tbValPatCierreValores.ReadOnly = False
            tbValCuotaCierreCuota.ReadOnly = False
            tbValCuotaCierreValores.ReadOnly = False
            tbValValoresPrecierre.ReadOnly = False
            tbCXPotrasCierre.ReadOnly = False
            tbValCuotaCierreCuota.ReadOnly = False
            tbValCuotaCierreValores.ReadOnly = False
            btnAceptar.Visible = True
            btnProcesar.Visible = False
        Else
            tbCXCtitulo.ReadOnly = True
            txtCXCPrecierre.ReadOnly = True
            tbCXPtitulo.ReadOnly = True
            txtCXPPrecierre.ReadOnly = True
            tbValPatriPrecierre.ReadOnly = True
            tbComiSAFM.ReadOnly = True
            tbValPatriPrecierre2.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbCajaCierre.ReadOnly = True
            tbAporteCuota.ReadOnly = True
            tbRescatesCuota.ReadOnly = True
            tbCXCtituloCierre.ReadOnly = True
            tbCXPtituloCierre.ReadOnly = True
            tbValPatCierreCuota.ReadOnly = True
            tbValPatCierreValores.ReadOnly = True
            tbValCuotaCierreCuota.ReadOnly = True
            tbValCuotaCierreValores.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbCXPotrasCierre.ReadOnly = True
            tbValCuotaCierreCuota.ReadOnly = True
            tbValCuotaCierreValores.ReadOnly = True
            btnAceptar.Visible = False
            btnProcesar.Visible = True
        End If
        HabilitarBotonDistribucion()
    End Sub
    Private Sub CargarValoresCuota()
        Dim DTValorCuota As DataTable
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se crea dataset para capturar el total de valorzación del reporte composición cartera| 09/08/18 
        Dim dsReporteComposicionCartera As DataSet
        Dim totalValorizacion As Object
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se crea dataset para capturar el total de valorzación del reporte composición cartera| 09/08/18 

        DTValorCuota = ValorCuota.CalcularValoresCuota(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        tbInversionesT1.Text = DTValorCuota.Rows(0)("InversionesT1")
        tbComprasT.Text = DTValorCuota.Rows(0)("ComprasT")
        tbVentasVenciT.Text = DTValorCuota.Rows(0)("VentasyVencimientosT")
        tbRentabilidadT.Text = DTValorCuota.Rows(0)("RentabilidadT")
        tbValoriForwardT.Text = DTValorCuota.Rows(0)("ValoracionForwards")
        tbInversionesSubTotal.Text = DTValorCuota.Rows(0)("InversionesSubTotal")
        tbValoriSwapT.Text = DTValorCuota.Rows(0)("ValorizacionSwaps")
        tbCXCtitulo.Text = DTValorCuota.Rows(0)("CXCVentaTitulo")
        tbCXPtitulo.Text = DTValorCuota.Rows(0)("CXPCompraTitulo")
        Dim tipoNegocio As String = ""
        tipoNegocio = DTValorCuota.Rows(0)("TipoNegocio")

        'INICIO | ZOLUXIONES | Rcolonia | Aumento de Capital - Se recupera nuevo campo Total Interes Aumento de Capital | 21/09/18 
        txtInteresesAumentoCapital.Text = DTValorCuota.Rows(0)("totalInteresAumentoCapital")
        'FIN | ZOLUXIONES | Rcolonia | Aumento de Capital - Se recupera nuevo campo Total Interes Aumento de Capital | 21/09/18 

        '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-07-17 | Aplicación de Compra de Divisas con Liquidación Post Cierre
        'El requerimiento tomará efecto a partir de la fecha indicada por el usuario, Ejemplo: 2018-08-01
        Dim fechaInicioLiquidacionesPost As New DateTime(2018, 8, 15)
        If fechaInicioLiquidacionesPost <= Date.Now Then
            tbCXCtitulo.Text = CDec(tbCXCtitulo.Text) + DTValorCuota.Rows(0)("CxC_DivisasLiquidacionPost")
            tbCXPtitulo.Text = CDec(tbCXPtitulo.Text) + DTValorCuota.Rows(0)("CxP_DivisasLiquidacionPost")
        End If
        '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-07-17 | Aplicación de Compra de Divisas con Liquidación Post Cierre

        tbValCuotaPrecierre.Text = DTValorCuota.Rows(0)("CuotaPreCierreAnterior")
        HdFechaCreacionFondo.Value = DTValorCuota.Rows(0)("FechaCreacionFondo")
        tbCajaPrecierre.Text = DTValorCuota.Rows(0)("CajaPrecierre")
        hdCajaPrecierre.Value = DTValorCuota.Rows(0)("CajaPrecierre")
        'tbAporteValores.Text = DTValorCuota.Rows(0)("Suscripcion") 'aporte.
        'tbAporteValores.Text = ObtenerAporteValores() '1


        Dim oPagoFechaComisionBM As New PagoFechaComisionBM
        Dim dtFechaCobro As DataTable = oPagoFechaComisionBM.ObtenerFechaComision(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlPortafolio.SelectedValue)
        If dtFechaCobro.Rows.Count > 0 Then
            txtComisionSAFMAnterior.Text = dtFechaCobro(0)("FechaComisionCadena")
            txtComisionSAFMAnterior.ReadOnly = True
            dvDiv321.Attributes.Add("class", "input-append")
            If dtFechaCobro(0)("FechaComisionCadena") = "" Then
                If Not CInt(DTValorCuota.Rows(0)("FechaInicioMes")) = 0 Then
                    txtComisionSAFMAnterior.Text = UIUtility.ConvertirFechaaString(CDec(DTValorCuota.Rows(0)("FechaInicioMes")))

                    txtComisionSAFMAnterior.ReadOnly = False
                    dvDiv321.Attributes.Add("class", "input-append date")
                Else
                    txtComisionSAFMAnterior.Text = ""
                End If
            End If
        Else
            If Not CInt(DTValorCuota.Rows(0)("FechaInicioMes")) = 0 Then
                txtComisionSAFMAnterior.Text = UIUtility.ConvertirFechaaString(CDec(DTValorCuota.Rows(0)("FechaInicioMes")))
            Else
                txtComisionSAFMAnterior.Text = ""
            End If

        End If


        If hdSeriado.Value = "S" Then
            txtVCA.Text = "0"
        Else
            txtVCA.Text = DTValorCuota.Rows(0)("VCAnterior")
        End If
        txtigv.Text = ParametrosSIT.IGV
        txtporcom.Text = Math.Round(Decimal.Parse(txtComisionPortafolio.Value), 2).ToString + " %"
        'OT10916 - 26/10/2017 - Ian Pastor M. Obtiene el monto total de dividendos que aún no liquidan
        If hdCuotasLiberadas.Value = "1" Then
            txtMontoDividendosPrecierre.Text = DTValorCuota.Rows(0)("CXCVentaTituloDividendos")
        Else
            txtMontoDividendosPrecierre.Text = "0"
            tbCXCtitulo.Text = Decimal.Parse(tbCXCtitulo.Text) + Decimal.Parse(DTValorCuota.Rows(0)("CXCVentaTituloDividendos"))
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se setea en campo "Otras CxC" el movimiento de caja para operaciones OP0089 (Nuevo Concepto) y si encuentra valor se lista los valores ingresados por caja | 07/06/18 
        dgOtrasCxC.DataSource = Nothing
        dgOtrasCxC.DataBind()
        tbCXCotras.Text = DTValorCuota.Rows(0)("montoCxC")
        If Decimal.Parse(tbCXCotras.Text) > 0 Then
            dgOtrasCxC.DataSource = ValorCuota.ValorCuota_ObtenerImporteCaja_CxC(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlPortafolio.SelectedValue)
            dgOtrasCxC.DataBind()
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se setea en campo "Otras CxC" el movimiento de caja para operaciones OP0089 (Nuevo Concepto) y si encuentra valor se lista los valores ingresados por caja | 07/06/18 

        If tipoNegocio = "MANDA" Then
            imgtbInversionesT1.Visible = False
            tbInversionesSubTotal.BorderColor = CType(ViewState("tbInversionesSubTotalBorderColor"), Drawing.Color)
        Else

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se obtiene total de campo Valorización de reporte composición cartera en moneda de portafolio | 09/08/18 
            dsReporteComposicionCartera = New ReporteGestionBM().ComposicionCartera(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), "REAL", ddlPortafolio.SelectedValue, DatosRequest)
            totalValorizacion = dsReporteComposicionCartera.Tables(0).Compute("SUM([Valoración Moneda Portafolio])", String.Empty)
            If totalValorizacion.ToString = String.Empty Then totalValorizacion = "0"
            txtTotalComposicionCartera.Text = IIf(IsNumeric(totalValorizacion), Math.Round(Decimal.Parse(totalValorizacion.ToString), 7), "0")
            txtDiferenciaComposicionCartera.Text = Decimal.Parse(txtTotalComposicionCartera.Text) - IIf(IsNumeric(tbInversionesSubTotal.Text), Decimal.Parse(tbInversionesSubTotal.Text), 0)
            If Decimal.Parse(txtDiferenciaComposicionCartera.Text) < -1 Or Decimal.Parse(txtDiferenciaComposicionCartera.Text) > 1 Then
                tbInversionesSubTotal.BorderColor = Drawing.Color.Red
                imgtbInversionesT1.Visible = True
            Else
                imgtbInversionesT1.Visible = False
                tbInversionesSubTotal.BorderColor = CType(ViewState("tbInversionesSubTotalBorderColor"), Drawing.Color)
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se obtiene total de campo Valorización de reporte composición cartera en moneda de portafolio | 09/08/18 

        End If

    End Sub

    'INICIO | ZOLUXIONES | DACV | SPRINT III - 20180617
    Private diferencia As Decimal
    Private saldoIni, saldoFin As Decimal
    'FIN | ZOLUXIONES | DACV | SPRINT III - 20180617

    Private Sub CargarValoresIngresados()
        Dim oValorCuotaBE As New ValorCuotaBE
        oValorCuotaBE = ValorCuota.SeleccionarValorCuota(ddlPortafolio.SelectedValue, "", UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        If Not oValorCuotaBE.Tables(0).Rows.Count = 0 Then
            Dim oRowS As ValorCuotaBE.ValorCuotaSerieRow
            oRowS = DirectCast(oValorCuotaBE.ValorCuotaSerie.Rows(0), ValorCuotaBE.ValorCuotaSerieRow)
            tbCXCotras.Text = oRowS("OtrasCXC")
            tbCXPotras.Text = oRowS("OtrasCXP")
            tbOtrosGastos.Text = oRowS("OtrosGastos")
            tbOtrosIngresos.Text = oRowS("OtrosIngresos")
            tbValPatriPrecierre.Text = oRowS("ValPatriPreCierre1")
            tbComiSAFM.Text = oRowS("ComisionSAFM")
            tbValPatriPrecierre2.Text = oRowS("ValPatriPreCierre2")
            tbValValoresPrecierre.Text = oRowS("ValCuotaPreCierreVal")
            tbAporteCuota.Text = oRowS("AportesCuotas")
            tbRescatesCuota.Text = oRowS("RescateCuotas")
            tbRescatesValores.Text = oRowS("RescateValores")
            tbCajaCierre.Text = oRowS("Caja")
            tbCXCtituloCierre.Text = oRowS("CXCVentaTituloCierre")
            tbCXCotrasCierre.Text = oRowS("OtrosCXCCierre")
            tbCXPtituloCierre.Text = oRowS("CXPCompraTituloCierre")
            tbCXPotrasCierre.Text = oRowS("OtrasCXPCierre")
            tbOtrosGastosCierre.Text = oRowS("OtrosGastosCierre")
            tbOtrosIngresosCierre.Text = oRowS("OtrosIngresosCierre")
            tbValPatCierreCuota.Text = oRowS("ValPatriCierreCuota")
            tbValPatCierreValores.Text = oRowS("ValPatriCierreValores")
            tbValCuotaCierreCuota.Text = oRowS("ValCuotaCierre")
            tbValCuotaCierreValores.Text = oRowS("ValCuotaValoresCierre")
            txtCXCPrecierre.Text = oRowS("CXCPreCierre")
            txtCXPPrecierre.Text = oRowS("CXPPreCierre")
            tbCXCCierre.Text = oRowS("CXCCierre")
            tbCXPCierre.Text = oRowS("CXPCierre")
            txtChequePendiente.Text = oRowS("ChequePendiente")
            txtRescatePendiente.Text = oRowS("RescatePendiente")
            txtDevolucionComisionUnificada.Text = oRowS("DevolucionComisionUnificada")
            'txtRescatePendiente.Text = ObtenerRescatesPendientes()
            tbAporteValores.Text = oRowS("AportesValores")
            If hdSeriado.Value = "S" Then
                txtVCDiferencia.Text = "0"
            Else
                'INICIO | ZOLUXIONES | DACV | SPRINT III - 20180617
                diferencia = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(txtVCA.Text)
                'FIN | ZOLUXIONES | DACV | SPRINT III - 20180617
                txtVCDiferencia.Text = Math.Round(diferencia / IIf(Decimal.Parse(tbValCuotaCierreValores.Text) = 0, 1, Decimal.Parse(tbValCuotaCierreValores.Text)) * 100, 7)
            End If
            'OT 9981 15/02/2017 - Carlos Espejo
            'Descripcion: Se cambia el valor del campo ComisionSAFMAnterior

            ViewState("ComisionSAFMAnterior") = oRowS("ComisionSAFMAnterior")


            'If Not CInt(oRowS("ComisionSAFMAnterior")) = 0 And Not btnEjecutarCierreOpe.Visible Then
            '    txtComisionSAFMAnterior.Text = UIUtility.ConvertirFechaaString(CDec(oRowS("ComisionSAFMAnterior")))
            'End If
            txtAjustesCXP.Text = oRowS("AjustesCXP")
            If Decimal.Parse(IIf(tbValCuotaPrecierre.Text.Trim = "", 0, tbValCuotaPrecierre.Text.Trim)) = 0 Then
                tbValCuotaPrecierre.Text = oRowS("ValCuotaPreCierre")
            End If
            Dim dtOtrasCXP = ValorCuota.OtrasCXP(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
            Decimal.Parse(txtChequePendiente.Text), Decimal.Parse(txtRescatePendiente.Text), UIUtility.ConvertirFechaaDecimal(txtComisionSAFMAnterior.Text), Decimal.Parse(txtDevolucionComisionUnificada.Text))
            txtComisionSAFM.Text = Convert.ToDecimal(dtOtrasCXP.Rows(0)("ComisionSAFM")) - Convert.ToDecimal(oRowS("DevolucionComisionUnificada"))
            txtCajaRecaudo.Text = dtOtrasCXP.Rows(0)("CajaRecaudo")
            txtSuscripcion.Text = dtOtrasCXP.Rows(0)("Suscripcion")
            txtRescateP.Text = dtOtrasCXP.Rows(0)("RescatePendiente")
            txtChequeP.Text = dtOtrasCXP.Rows(0)("ChequePendiente")
            'INICIO | ZOLUXIONES | rcolonia | Se recupera la distribución programada de intereses | 02-11-2018
            txtDistribucionProgramada.Text = dtOtrasCXP.Rows(0)("totalInteresDistribucion")
            'FIN | ZOLUXIONES | rcolonia | Se recupera la distribución programada de intereses  | 02-11-2018
            'OT 9981 Fin
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se recupera nuevo campo AjustesCXC | 16/07/18 
            txtAjustesCXC.Text = oRowS("AjustesCXC")
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se recupera nuevo campo AjustesCXC | 16/07/18 
            '  txtDevolucionComisionUnificada.Text = ObtenerDevolucionComisionUnificada()

            txtOtrasCxCPreCierre.Text = oRowS("OtrasCxCPreCierre")
            txtComisionUnificadaCuota.Text = oRowS("ComisionUnificadaCuota")
            txtComisionUnificadaMandato.Text = oRowS("ComisionUnificadaMandato")
            txtDevolucionComisionDiaria.Text = oRowS("DevolucionComisionDiaria")
            'txtAporteMandato.Text = oRowS("AporteMandato")
            'txtRetiroMandato.Text = oRowS("RetiroMandato")
        Else
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se comenta campo ya que en procedimiento CargarValoresCuota se fija valor | 07/06/18 
            'tbCXCotras.Text = 0
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se comenta campo ya que en procedimiento CargarValoresCuota se fija valor | 07/06/18 
            tbCXPotras.Text = 0
            tbOtrosGastos.Text = 0
            tbOtrosIngresos.Text = 0
            tbValPatriPrecierre.Text = 0
            tbComiSAFM.Text = 0
            tbValPatriPrecierre2.Text = 0
            tbValValoresPrecierre.Text = 0
            tbAporteCuota.Text = 0
            tbRescatesCuota.Text = 0
            tbRescatesValores.Text = 0
            tbCajaCierre.Text = 0
            tbCXCtituloCierre.Text = 0
            tbCXCotrasCierre.Text = 0
            tbCXPtituloCierre.Text = 0
            tbCXPotrasCierre.Text = 0
            tbOtrosGastosCierre.Text = 0
            tbOtrosIngresosCierre.Text = 0
            tbValPatCierreCuota.Text = 0
            tbValPatCierreValores.Text = 0
            tbValCuotaCierreCuota.Text = 0
            tbValCuotaCierreValores.Text = 0
            txtCXCPrecierre.Text = 0
            txtCXPPrecierre.Text = 0
            tbCXCCierre.Text = 0
            tbCXPCierre.Text = 0
            txtChequePendiente.Text = 0
            txtRescatePendiente.Text = 0
            'txtRescatePendiente.Text = ObtenerRescatesPendientes()
            txtAjustesCXP.Text = 0
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se setea txtAjustesCXC cuando no se tiene registro ingresado | 16/07/18 
            txtAjustesCXC.Text = 0
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se setea txtAjustesCXC cuando no se tiene registro ingresado | 16/07/18 
            tbCXCotras.Text = 0
            tbAporteValores.Text = 0
            txtVCDiferencia.Text = 0
            txtChequeP.Text = 0
            txtRescateP.Text = 0
            txtSuscripcion.Text = 0
            txtCajaRecaudo.Text = 0
            txtComisionSAFM.Text = 0
            txtDistribucionProgramada.Text = 0
            txtDevolucionComisionUnificada.Text = 0
            txtOtrasCxCPreCierre.Text = 0
            txtComisionUnificadaCuota.Text = 0
            txtComisionUnificadaMandato.Text = 0
            txtDevolucionComisionDiaria.Text = 0
            'txtAporteMandato.Text = 0
            'txtRetiroMandato.Text = 0
        End If
        'OT10986 - 15/12/2017 - Ian Pastor M. Agregar valor inicial de rescate de valores
        'tbRescatesValores.Text = ObtenerRescateValores()
    End Sub
    Private Sub CargarObjetoCabecera(ByVal rowValorCuota As ValorCuotaBE.ValorCuotaRow, Optional ByVal CodigoSerie As String = "")
        rowValorCuota.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        rowValorCuota.CodigoSerie = CodigoSerie
        rowValorCuota.FechaProceso = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        rowValorCuota.InversionesT1 = IIf(tbInversionesT1.Text = "", 0, tbInversionesT1.Text)
        rowValorCuota.VentasVencimientos = IIf(tbVentasVenciT.Text = "", 0, tbVentasVenciT.Text)
        rowValorCuota.Rentabilidad = IIf(tbRentabilidadT.Text = "", 0, tbRentabilidadT.Text)
        rowValorCuota.ValForwards = IIf(tbValoriForwardT.Text = "", 0, tbValoriForwardT.Text)
        rowValorCuota.ValSwaps = IIf(tbValoriSwapT.Text = "", 0, tbValoriSwapT.Text)
        rowValorCuota.InversionesSubTotal = IIf(tbInversionesSubTotal.Text = "", 0, tbInversionesSubTotal.Text)
    End Sub
    Private Sub CargarObjetoDetalle(ByVal rowDetalleCuota As ValorCuotaBE.ValorCuotaSerieRow)
        rowDetalleCuota.CajaPreCierre = IIf(tbCajaPrecierre.Text = "", 0, tbCajaPrecierre.Text)
        rowDetalleCuota.CXCVentaTitulo = IIf(tbCXCtitulo.Text = "", 0, tbCXCtitulo.Text)
        rowDetalleCuota.OtrasCXC = IIf(tbCXCotras.Text = "", 0, tbCXCotras.Text)
        rowDetalleCuota.CXCPreCierre = IIf(txtCXCPrecierre.Text = "", 0, txtCXCPrecierre.Text)
        rowDetalleCuota.CXPCompraTitulo = IIf(tbCXPtitulo.Text = "", 0, tbCXPtitulo.Text)
        rowDetalleCuota.OtrasCXP = IIf(tbCXPotras.Text = "", 0, tbCXPotras.Text)
        rowDetalleCuota.CXPPreCierre = IIf(txtCXPPrecierre.Text = "", 0, txtCXPPrecierre.Text)
        rowDetalleCuota.OtrosGastos = IIf(tbOtrosGastos.Text = "", 0, tbOtrosGastos.Text)
        rowDetalleCuota.OtrosIngresos = IIf(tbOtrosIngresos.Text = "", 0, tbOtrosIngresos.Text)
        rowDetalleCuota.ValPatriPreCierre1 = IIf(tbValPatriPrecierre.Text = "", 0, tbValPatriPrecierre.Text)
        rowDetalleCuota.ComisionSAFM = IIf(tbComiSAFM.Text = "", 0, tbComiSAFM.Text)
        rowDetalleCuota.ValPatriPreCierre2 = IIf(tbValPatriPrecierre2.Text = "", 0, tbValPatriPrecierre2.Text)
        rowDetalleCuota.ValCuotaPreCierre = IIf(tbValCuotaPrecierre.Text = "", 0, tbValCuotaPrecierre.Text)
        rowDetalleCuota.ValCuotaPreCierreVal = IIf(tbValValoresPrecierre.Text = "", 0, tbValValoresPrecierre.Text)
        rowDetalleCuota.AportesCuotas = IIf(tbAporteCuota.Text = "", 0, tbAporteCuota.Text)
        rowDetalleCuota.AportesValores = IIf(tbAporteValores.Text = "", 0, tbAporteValores.Text)
        rowDetalleCuota.RescateCuotas = IIf(tbRescatesCuota.Text = "", 0, tbRescatesCuota.Text)
        rowDetalleCuota.RescateValores = IIf(tbRescatesValores.Text = "", 0, tbRescatesValores.Text)
        rowDetalleCuota.Caja = IIf(tbCajaCierre.Text = "", 0, tbCajaCierre.Text)
        rowDetalleCuota.CXCVentaTituloCierre = IIf(tbCXCtituloCierre.Text = "", 0, tbCXCtituloCierre.Text)
        rowDetalleCuota.OtrosCXCCierre = IIf(tbCXCotrasCierre.Text = "", 0, tbCXCotrasCierre.Text)
        rowDetalleCuota.CXCCierre = IIf(tbCXCCierre.Text = "", 0, tbCXCCierre.Text)
        rowDetalleCuota.CXPCompraTituloCierre = IIf(tbCXPtituloCierre.Text = "", 0, tbCXPtituloCierre.Text)
        rowDetalleCuota.OtrasCXPCierre = IIf(tbCXPotrasCierre.Text = "", 0, tbCXPotrasCierre.Text)
        rowDetalleCuota.CXPCierre = IIf(tbCXPCierre.Text = "", 0, tbCXPCierre.Text)
        rowDetalleCuota.OtrosGastosCierre = IIf(tbOtrosGastosCierre.Text = "", 0, tbOtrosGastosCierre.Text)
        rowDetalleCuota.OtrosIngresosCierre = IIf(tbOtrosIngresosCierre.Text = "", 0, tbOtrosIngresosCierre.Text)
        rowDetalleCuota.ValPatriCierreCuota = IIf(tbValPatCierreCuota.Text = "", 0, tbValPatCierreCuota.Text)
        rowDetalleCuota.ValPatriCierreValores = IIf(tbValPatCierreValores.Text = "", 0, tbValPatCierreValores.Text)
        rowDetalleCuota.ValCuotaCierre = IIf(tbValCuotaCierreCuota.Text = "", 0, tbValCuotaCierreCuota.Text)
        rowDetalleCuota.ValCuotaValoresCierre = IIf(tbValCuotaCierreValores.Text = "", 0, tbValCuotaCierreValores.Text)
        rowDetalleCuota.OtrasCXCExclusivos = 0
        rowDetalleCuota.OtrasCXPExclusivos = 0
        rowDetalleCuota.OtrosGastosExclusivos = 0
        rowDetalleCuota.OtrosIngresosExclusivos = 0
        rowDetalleCuota.OtrosCXCExclusivoCierre = 0
        rowDetalleCuota.OtrasCXPExclusivoCierre = 0
        rowDetalleCuota.OtrosGastosExclusivosCierre = 0
        rowDetalleCuota.OtrosIngresosExclusivosCierre = 0
        rowDetalleCuota.InversionesSubTotalSerie = 0
        rowDetalleCuota.ValCuotaPreCierreAnt = 0
        'OT 9851 27/01/2017 - Carlos Espejo
        'Descripcion: Campos nuevos para el calculo de Otras CXP
        rowDetalleCuota.RescatePendiente = IIf(txtRescatePendiente.Text = "", 0, txtRescatePendiente.Text)
        rowDetalleCuota.ChequePendiente = IIf(txtChequePendiente.Text = "", 0, txtChequePendiente.Text)
        'OT 9981 15/02/2017 - Carlos Espejo
        'Descripcion: Se cambia el valor del campo ComisionSAFMAnterior
        rowDetalleCuota.ComisionSAFMAnterior = UIUtility.ConvertirFechaaDecimal(txtComisionSAFMAnterior.Text)
        rowDetalleCuota.AjustesCXP = IIf(txtAjustesCXP.Text = "", 0, txtAjustesCXP.Text)
        'OT 9981 Fin
        'OT 9851 Fin
        rowDetalleCuota.CXCVentaTituloDividendos = IIf(txtMontoDividendosPrecierre.Text = "", 0, txtMontoDividendosPrecierre.Text)
        'OT10883 - 08/11/2017 - Ian Pastor M. Nuevos campos agregados
        rowDetalleCuota.AportesLiberadas = 0
        rowDetalleCuota.RetencionPendiente = 0
        'OT10883 - Fin
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se carga valor de nuevo campo AjustesCXC | 16/07/18 
        rowDetalleCuota.AjustesCXC = IIf(txtAjustesCXC.Text = String.Empty, "0", txtAjustesCXC.Text)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se carga valor de nuevo campo AjustesCXC | 16/07/18 
        rowDetalleCuota.DevolucionComisionUnificada = IIf(txtDevolucionComisionUnificada.Text = String.Empty, "0", txtDevolucionComisionUnificada.Text)
        rowDetalleCuota.OtrasCxCPreCierre = IIf(txtOtrasCxCPreCierre.Text = String.Empty, "0", txtOtrasCxCPreCierre.Text)
        rowDetalleCuota.ComisionUnificadaCuota = IIf(txtComisionUnificadaCuota.Text = String.Empty, "0", txtComisionUnificadaCuota.Text)
        rowDetalleCuota.ComisionUnificadaMandato = IIf(txtComisionUnificadaMandato.Text = String.Empty, "0", txtComisionUnificadaMandato.Text)
        'rowDetalleCuota.AporteMandato = IIf(hdAporteMandato.Value = String.Empty, "0", hdAporteMandato.Value)
        'rowDetalleCuota.RetiroMandato = IIf(hdRetiroMandato.Value = String.Empty, "0", hdRetiroMandato.Value)        
        rowDetalleCuota.DevolucionComisionDiaria = IIf(txtDevolucionComisionDiaria.Text = "", 0, txtDevolucionComisionDiaria.Text)


        Dim oPortafolioBE As New PortafolioBE
        Dim oPortafolioBM As New PortafolioBM
        Dim oRowPortafolioBE As PortafolioBE.PortafolioRow
        oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
        oRowPortafolioBE = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)

        If oRowPortafolioBE.PorSerie = "S" Then
            rowDetalleCuota.DevolucionComisionDiaria = 0
        Else
            rowDetalleCuota.DevolucionComisionDiaria = IIf(txtDevolucionComisionDiaria.Text = "", 0, txtDevolucionComisionDiaria.Text)
        End If

    End Sub
    Private Function ValidarCuotaPreCierre() As Boolean
        'En el ambiente de desarrollo no se consulta el precierre
        ValidarCuotaPreCierre = False
        Dim ExistePrecierre As Integer
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        Dim oPortafolioBE As PortafolioBE
        Dim oPortafolioBM As New PortafolioBM
        Dim oRow As PortafolioBE.PortafolioRow
        oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
        oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
        ExistePrecierre = oCarteraTituloValoracion.Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(CDate(tbFechaOperacion.Text), CDate(tbFechaOperacion.Text), _
            IIf(oRow.CodigoPortafolioSisOpe = "", 0, oRow.CodigoPortafolioSisOpe))
        If ExistePrecierre > 0 Then
            Dim dt As DataTable = oCarteraTituloValoracion.ObtenerValorCuotaPreCierreOperaciones(tbFechaOperacion.Text, IIf(oRow.CodigoPortafolioSisOpe = "", 0, oRow.CodigoPortafolioSisOpe))
            If dt.Rows.Count > 0 Then
                If CType(tbValValoresPrecierre.Text, Decimal) <> CType(dt.Rows(0)("VALOR_CUOTA"), Decimal) Then
                    ValidarCuotaPreCierre = True
                End If
            End If
        End If
    End Function
    Private Sub HabilitarControlesCalculoValorCuota(ByVal TipoCalculoValorCuota As String)
        Select Case TipoCalculoValorCuota
            Case "01"
                tbInversionesSubTotal.ReadOnly = False
                tbCajaPrecierre.ReadOnly = False
            Case "02"
                tbInversionesSubTotal.ReadOnly = True
                tbCajaPrecierre.ReadOnly = True
        End Select
    End Sub
    Private Sub MostrarFormularioSoloLectura(ByVal Lectura As Boolean)
        If Lectura Then
            txtVCA.ReadOnly = True
            txtVCDiferencia.ReadOnly = True
            txtComisionSAFMAnterior.ReadOnly = True
            dvDiv321.Attributes.Add("class", "input-append")
            tbInversionesT1.ReadOnly = True
            tbComprasT.ReadOnly = True
            tbVentasVenciT.ReadOnly = True
            tbRentabilidadT.ReadOnly = True
            tbValoriForwardT.ReadOnly = True
            tbValoriSwapT.ReadOnly = True
            tbInversionesSubTotal.ReadOnly = True
            txtigv.ReadOnly = True
            txtporcom.ReadOnly = True
            tbCajaPrecierre.ReadOnly = True
            tbCXCtitulo.ReadOnly = True
            tbCXCotras.ReadOnly = True
            txtCXCPrecierre.ReadOnly = True
            txtChequePendiente.ReadOnly = True
            txtRescatePendiente.ReadOnly = True
            txtAjustesCXP.ReadOnly = True
            tbCXPtitulo.ReadOnly = True
            tbCXPotras.ReadOnly = True
            txtCXPPrecierre.ReadOnly = True
            tbOtrosGastos.ReadOnly = True
            tbOtrosIngresos.ReadOnly = True
            tbValPatriPrecierre.ReadOnly = True
            tbComiSAFM.ReadOnly = True
            tbValPatriPrecierre2.ReadOnly = True
            tbValCuotaPrecierre.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbCajaCierre.ReadOnly = True
            tbAporteCuota.ReadOnly = True
            tbAporteValores.ReadOnly = True
            tbRescatesCuota.ReadOnly = True
            tbRescatesValores.ReadOnly = True
            tbCXCtituloCierre.ReadOnly = True
            tbCXCotrasCierre.ReadOnly = True
            tbCXCCierre.ReadOnly = True
            tbCXPtituloCierre.ReadOnly = True
            tbCXPotrasCierre.ReadOnly = True
            tbCXPCierre.ReadOnly = True
            tbOtrosGastosCierre.ReadOnly = True
            tbOtrosIngresosCierre.ReadOnly = True
            tbValPatCierreCuota.ReadOnly = True
            tbValPatCierreValores.ReadOnly = True
            tbValCuotaCierreCuota.ReadOnly = True
            tbValCuotaCierreValores.ReadOnly = True
            txtComisionSAFM.ReadOnly = True
            txtCajaRecaudo.ReadOnly = True
            txtSuscripcion.ReadOnly = True
            txtChequeP.ReadOnly = True
            txtRescateP.ReadOnly = True
            txtDistribucionProgramada.ReadOnly = True
            btnCarga.Visible = Not True
            cbHabilitarCarga.Visible = Not True
            btnAceptar.Visible = Not True
            btnProcesar.Visible = Not True
            'INICIO | ZOLUXIONES | Rcolonia | Aumento de Capital | 21/09/18 
            txtInteresesAumentoCapital.ReadOnly = True
            'FIN | ZOLUXIONES | Rcolonia | Aumento de Capital | 21/09/18 
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se continúa lógica de habilitación de campos según procedimiento para nuevo campo AjustesCXC | 16/07/18 
            txtAjustesCXC.ReadOnly = True
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se continúa lógica de habilitación de campos según procedimiento para nuevo campo AjustesCXC | 16/07/18 
            txtDevolucionComisionUnificada.ReadOnly = True

        Else
            '  txtComisionSAFMAnterior.ReadOnly = False
            tbCXCotras.ReadOnly = False
            txtChequePendiente.ReadOnly = False
            txtRescatePendiente.ReadOnly = False
            txtAjustesCXP.ReadOnly = False
            tbOtrosGastos.ReadOnly = False
            tbOtrosIngresos.ReadOnly = False
            tbValCuotaPrecierre.ReadOnly = False
            tbRescatesValores.ReadOnly = False
            tbOtrosGastosCierre.ReadOnly = False
            tbOtrosIngresosCierre.ReadOnly = False
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se continúa lógica de habilitación de campos según procedimiento para nuevo campo AjustesCXC | 16/07/18 
            txtAjustesCXC.ReadOnly = False
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se continúa lógica de habilitación de campos según procedimiento para nuevo campo AjustesCXC | 16/07/18 

        End If
    End Sub
    Private Sub HabilitarBotonDistribucion()
        Dim objPortafolioBM As New PortafolioBM
        Dim objPortafolioBE As New PortafolioBE
        objPortafolioBE = objPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, DatosRequest)
        If objPortafolioBE.Tables(0).Rows(0)("CuotasLiberadas") = "1" Then
            btnVerDistribucion.Visible = True
            ConsultarDistribucionLib()
        Else
            btnVerDistribucion.Visible = False
        End If
        ViewState("CuotasLiberadas") = objPortafolioBE.Tables(0).Rows(0)("CuotasLiberadas")
    End Sub
    Protected Sub dgArchivo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgArchivo.RowCommand
        If e.CommandName.Equals("Modificar") Then
            Dim cadena As String = e.CommandArgument.ToString()
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim rowValorCuota As ValorCuotaBE.ValorCuotaRow
            Dim dblPorcentaje As Decimal
            Dim hdCheckOperaciones As String
            Dim DtValoresSerie As DataTable
            Dim oPortafolio As New PortafolioBM
            Dim CodigoPortafolioSeriadoSO As String = String.Empty
            dblPorcentaje = Decimal.Parse(CType(gvr.FindControl("tbPorcentaje"), TextBox).Text)
            rowValorCuota = CType(objValorCuotaBE.ValorCuota.NewRow(), ValorCuotaBE.ValorCuotaRow)
            CargarObjetoCabecera(rowValorCuota, cadena(0))
            Session("DatosCabecera") = rowValorCuota
            DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie(ddlPortafolio.SelectedValue)
            If DtValoresSerie.Rows.Count > 0 Then
                Dim DevolucionDiaria As Decimal = ObtenerDevolucionDiariaOperaciones()
                txtDevolucionComisionDiaria.Text = DevolucionDiaria
                CodigoPortafolioSeriadoSO = DtValoresSerie.Select("CodigoSerie='" & cadena & "'")(0)("CodigoPortafolioSO")
            End If
            'Busqueda Flujo Distribucion - Checkbox  JH  02/04/2019'
            Dim dtListaFlujo As DataTable = ListaDistrucionFlujo()
            Dim dtListaSeleccionada As DataRow() = dtListaFlujo.Select("ID = " + CodigoPortafolioSeriadoSO)
            If (dtListaSeleccionada.Count > 0) Then
                hdCheckOperaciones = "1"
            Else
                hdCheckOperaciones = "0"
            End If

            Dim strURL As String = "frmAgregarSeries.aspx?NroReg=" & cadena & "&Serie=" & cadena & "&Portafolio=" & ddlPortafolio.SelectedValue & _
                "&Fecha=" & tbFechaOperacion.Text & "&Porcentaje=" & CType(gvr.FindControl("tbPorcentaje"), TextBox).Text & _
                "&DetPorta=" & ddlPortafolio.SelectedItem.Text & "&CuotasLiberadas=" & ViewState("CuotasLiberadas") & "&CodigoPortafolioSeriadoSO=" & CodigoPortafolioSeriadoSO & "&hidCheck=" & hdCheckOperaciones & "&DevolucionComisionDiaria=" & txtDevolucionComisionDiaria.Text
            ' Session("llamadoAgregarSeries") = True
            EjecutarJS("showModalDialog('" & strURL & "', '1200', '600', '');") 'OT10795 - 20/10/2017 - Ian Pastor M. Se muestra la ventana sin importar el valor del porcentaje
        End If
    End Sub

    Protected Sub btnCarga_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCarga.Click
        Try
            Dim ValidaPortafolio As Boolean = True
            Dim ValidaSeries As Boolean = False
            Dim ValidaFecha As Boolean = True
            If iptRuta.Value.Equals("") Then
                AlertaJS("Debe seleccionar un archivo.")
                Exit Sub
            End If
            Dim oArchivoPlanoBE As New DataSet
            Dim oArchivoPlanoBM As New ArchivoPlanoBM
            oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("005", MyBase.DatosRequest())
            RutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
            If (Not Directory.Exists(RutaDestino)) Then
                AlertaJS("No existe la ruta destino.")
                Exit Sub
            End If
            Dim fInfo As New FileInfo(iptRuta.Value)
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)
            Dim strFile As String = File.ReadAllText(RutaDestino & "\" & fInfo.Name)
            strFile = Regex.Replace(strFile, "\n$", "") 'Elimina la ultima linea si es vacia
            strFile = Regex.Replace(strFile, "\n\r", "")  'Elimina las lineas vacias que no son ni la primera ni la ultima
            File.WriteAllText(RutaDestino & "\" & fInfo.Name, strFile)
            Dim objReader As New StreamReader(RutaDestino & "\" & fInfo.Name)
            Dim sLine As String = ""
            Dim arrText As New ArrayList()
            Do
                sLine = objReader.ReadLine()
                If Not sLine Is Nothing Then
                    arrText.Add(sLine)
                End If
            Loop Until sLine Is Nothing
            objReader.Close()
            'Validamos que el portafolio seleccionado sea el correcto.
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")

                If Not Fila(0).Equals(ddlPortafolio.SelectedItem.Text) Then
                    ValidaPortafolio = False
                    Exit For
                End If
            Next
            If Not ValidaPortafolio Then
                AlertaJS("El archivo a cargar no pertenece al Portafolio seleccionado.")
                Exit Sub
            End If
            'Validamos que la fecha del archivo se lgual a la fecha de Proceso
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")
                If Not Fila(1).Equals(tbFechaOperacion.Text) Then
                    ValidaFecha = False
                    Exit For
                End If
            Next
            If Not ValidaFecha Then
                AlertaJS("El archivo no corresponde a la Fecha de Proceso.")
                Exit Sub
            End If
            'validamos que la configuración de series este bien parametrizado
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")
                ValidaSeries = False
                For Each row As GridViewRow In dgArchivo.Rows
                    If row.Cells(1).Text.ToUpper.Equals(Fila(2).ToUpper) Then
                        ValidaSeries = True
                    End If
                Next
                If ValidaSeries = False Then
                    Exit For
                End If
            Next
            If Not ValidaSeries Then
                AlertaJS("El archivo a cargar no concuerda con la parametria de Series para este portafolio.")
                Exit Sub
            End If
            'Eliminamos los porcentajes antiguos antes de insertar los nuevos.
            Dim objportafolio As New PortafolioBM
            objportafolio.Eliminar_PorcentajeSeries(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            'Insertamos los porcentajes de las series
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")
                objportafolio = New PortafolioBM
                objportafolio.Insertar_PorcentajeSeries(ddlPortafolio.SelectedValue, Fila(2), UIUtility.ConvertirFechaaDecimal(Fila(1)), Fila(3), Me.DatosRequest)
            Next
            dgArchivo.DataSource = Nothing
            dgArchivo.DataBind()
            Dim oPortafolioBM As New PortafolioBM
            dgArchivo.DataSource = oPortafolioBM.Portafolio_Series_Cuotas(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            dgArchivo.DataBind()
            For Each row As GridViewRow In dgArchivo.Rows
                CType(row.FindControl("btnModificar"), ImageButton).Enabled = True
            Next
            AlertaJS("La carga se realizó con éxito.")
        Catch ex As Exception
            AlertaJS("Hubo un error al realizar la carga.")
        End Try
    End Sub

    Private Function validarFondosDevolucionOperacion(ByVal dtFondosOperacionesCuotas As DataTable, ByVal dtFondosSitComisiones As DataTable, ByVal oPortafolioBM As PortafolioBM) As String
        Dim resultado As String = String.Empty
        Dim objOperaciones As New PrecierreBO
        '  Dim rowVCuotaHistorico As DataRow = Nothing
        '  Dim listaFondosSit As New List(Of String)

        Dim listaFondosOperaciones = (From dr In dtFondosOperacionesCuotas
                      Select dr("ID_SERIE_A")
                      ).ToList()

        'Todos los fondos cerrados
        Dim listaFondosSit = (From dr In dtFondosSitComisiones
                              Where objOperaciones.ObtenerDetalleValorCuotaCerrado(IIf(dr("CodigoPortafolioSisOpe").trim = String.Empty, 0, dr("CodigoPortafolioSisOpe")), UIUtility.ConvertirStringaFecha(tbFechaOperacion.Text)) IsNot Nothing
                              Select dr("CodigoPortafolioSisOpe")
              ).ToList()


        'For Each row In dtFondosSitComisiones.Rows
        '    rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(row("CodigoPortafolioSisOpe"), UIUtility.ConvertirStringaFecha(tbFechaOperacion.Text))
        '    If rowVCuotaHistorico IsNot Nothing Then
        '        listaFondosSit.Add(row("CodigoPortafolioSisOpe"))
        '    End If
        'Next

        ' Dim filtradoFondos = listaFondosSit.Where(Function(s As String) s.Contains(listaFondosOperaciones.Where(Function(o As String) o.Equals(s)))).ToList()
        Dim resultadoAux As String = ""
        Dim dtFondos As DataTable = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)

        For j = 0 To listaFondosOperaciones.Count - 1

            If listaFondosSit.Where(Function(s As String) s.Equals(listaFondosOperaciones(j))).Count = 0 Then
                If hdCodigoPortafolioSisOpe.Value <> listaFondosOperaciones(j) Then

                    Dim fondoFaltaValorar As String = (From dr In dtFondos
                                           Where dr("CodigoPortafolioSisOpe").Equals(listaFondosOperaciones(j))
                      Select dr("Descripcion")
                     ).FirstOrDefault()
                    resultadoAux = resultadoAux + "<br>- " + fondoFaltaValorar
                End If

            End If
        Next

        If resultadoAux <> "" Then
            resultado = "Cálculo de devolución de comisiones. Se debe realizar valor cuota para los fondos:" + resultadoAux
        End If


        Return resultado
    End Function
    Private Function ObtenerDevolucionDiariaOperaciones() As Decimal
        Try
            Dim dtFondosDevolucionOperacion As DataTable = ValorCuota.ListarOperacionDevolucionDiaria(Convert.ToDateTime(tbFechaOperacion.Text))
            Dim dtFondosSerieNombres As DataTable = ValorCuota.ListarFondosConNombreSerie(ddlPortafolio.SelectedValue)

            Dim MonedaFondo As String = String.Empty

            Dim valorSoles As Decimal = 0
            Dim valorDolares As Decimal = 0
            For i = 0 To dtFondosSerieNombres.Rows.Count - 1
                MonedaFondo = dtFondosSerieNombres.Rows(i)("CodigoMoneda").ToString()
                For j = 0 To dtFondosDevolucionOperacion.Rows.Count - 1
                    If dtFondosSerieNombres.Rows(i)("NombreFondoSerie").ToString().Trim() = dtFondosDevolucionOperacion.Rows(j)("SERIE").ToString().Trim() Then

                        Dim objSol As String = dtFondosDevolucionOperacion.Rows(j)("COMISION_SERIE_SOL").ToString()
                        Dim objDol As String = dtFondosDevolucionOperacion.Rows(j)("COMISION_SERIE_DOL").ToString()

                        If objSol <> "" Then
                            valorSoles = valorSoles + Convert.ToDecimal(objSol)
                        End If

                        If objDol <> "" Then
                            valorDolares = valorDolares + Convert.ToDecimal(objDol)
                        End If


                        Exit For
                    End If
                Next
            Next

            If MonedaFondo = "NSOL" Then
                Return valorSoles
            Else
                Return valorDolares
            End If
        Catch ex As Exception
            Return 0
        End Try

    End Function
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim valorTipoCambio As String = txtTipoCambioOperacion.Text

            If txtTipoCambioOperacion.Text = String.Empty Then
                AlertaJS("Hubo un problema al tipo de cambio")
                Exit Sub
            End If


            'OT 9851 17/02/2017 - Carlos Espejo
            'Descripcion: Validacion para la fecha anterior
            If txtComisionSAFMAnterior.Text = "" Then
                AlertaJS("Se debe ingresar una fecha la comision SAFM anterior", "('#txtComisionSAFMAnterior').focus();")
                Exit Sub
            End If
            'OT 9851 Fin

            Dim oPortafolioBM As New PortafolioBM


            'Ernesto Galarza - OT12028 - Proceso de validacion de la devolucion de comision
            '=======================================================================================
            If tbFechaOperacion.Text <> "" Then

                'Si no esta en valores se hace el proceso de devolucion
                Dim existeNegociacionEnOtroFondo As String = ValorCuota.ValidarNegociacionFondosEnOtros(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))(0)(0)
                If existeNegociacionEnOtroFondo <> "" Then
                    Dim dtFondosOperacionesCuotas As DataTable = ValorCuota.ListarFondosComisionUnificadaOperaciones(Convert.ToDateTime(tbFechaOperacion.Text).AddDays(-2)).Tables(0)
                    Dim dtFondosSitComisiones As DataTable = ValorCuota.ListarFondosComisionUnificadaSit(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)).Tables(0)
                    If dtFondosOperacionesCuotas.Select("ID_FONDO IN (" + existeNegociacionEnOtroFondo + ")").Count() > 0 Then
                        dtFondosOperacionesCuotas = dtFondosOperacionesCuotas.Select("ID_FONDO IN (" + existeNegociacionEnOtroFondo + ")").CopyToDataTable()
                    Else
                        dtFondosOperacionesCuotas = dtFondosOperacionesCuotas.Clone()
                    End If

                    If dtFondosOperacionesCuotas.Rows.Count = 0 Then
                        AlertaJS("No se ha generado ningún cierre en operaciones para calcular la devolución de comisión.")
                        Exit Sub
                    End If

                    Dim listaFondosOperaciones = (From dr In dtFondosOperacionesCuotas
                              Where dr("ID_SERIE_A") = hdCodigoPortafolioSisOpe.Value
                              ).ToList()

                    Dim listaFondosSit = (From dr In dtFondosSitComisiones
                             Where dr("CodigoPortafolioSisOpe") = hdCodigoPortafolioSisOpe.Value
                             ).ToList()

                    Dim mensajeValidacionDevolucionOperacion = validarFondosDevolucionOperacion(dtFondosOperacionesCuotas, dtFondosSitComisiones, oPortafolioBM)

                    If mensajeValidacionDevolucionOperacion <> "" Then
                        AlertaJS(mensajeValidacionDevolucionOperacion)
                        Exit Sub
                    End If

                    If listaFondosSit.Count = 0 And listaFondosOperaciones.Count > 0 Then
                        'no hace nada
                    Else
                        dtFondosOperacionesCuotas = ValorCuota.ListarFondosComisionUnificadaOperaciones(Convert.ToDateTime(tbFechaOperacion.Text).AddDays(-1)).Tables(0)
                        dtFondosOperacionesCuotas = dtFondosOperacionesCuotas.Select("ID_FONDO IN (" + existeNegociacionEnOtroFondo + ")").CopyToDataTable()
                        If ValorCuota.GenerarDevolucionOperacion(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text, Convert.ToDecimal(txtTipoCambioOperacion.Text), dtFondosOperacionesCuotas, dtFondosSitComisiones, Me.DatosRequest) = False Then
                            AlertaJS("No pudo generar la devolución de comisión en el proceso de operaciones.")
                            Exit Sub
                        End If
                    End If

                End If

                Dim DevolucionDiaria As Decimal = ObtenerDevolucionDiariaOperaciones()
                txtDevolucionComisionDiaria.Text = DevolucionDiaria

            End If

            '=======================================================================================
            CargarValoresAutomaticos()
            If cbHabilitarCarga.Checked Then
                CargarPorcentajeSeries()
            End If
            'OT 9851 17/02/2017 - Carlos Espejo
            'Descripcion: Validacion para la fecha anterior
            If txtComisionSAFMAnterior.Text = "" Then
                AlertaJS("Se debe ingresar una fecha la comision SAFM anterior", "('#txtComisionSAFMAnterior').focus();")
                Exit Sub
            End If
            'OT 9851 Fin
            Dim objPortafolioPCBM As New PortafolioPorcentajeComisionBM
            Dim listPortafolioPCBE As List(Of PortafolioPorcentajeComisionBE)
            Dim valPatriPreCierre, saldoPatriPreCierre, comisionActual, comisionAcumulada, margenValorCuota, suscripcionIni As Decimal
            Dim cantComi, i As Int32
            Dim existeSaldo = True
            Dim oRowPortafolioBE As PortafolioBE.PortafolioRow
            Dim oPortafolioBE As PortafolioBE

            Session("totalPrecierre") = Nothing
            oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
            oRowPortafolioBE = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            listPortafolioPCBE = objPortafolioPCBM.Listar(ddlPortafolio.SelectedValue)
            'Calculo Valor Patrimonio precierre1
            Dim InversionesSubtotal As String = Replace(IIf(tbInversionesSubTotal.Text.Trim = "", "0", tbInversionesSubTotal.Text.Trim), ",", "")
            Dim CajaPrecierre As String = Replace(IIf(tbCajaPrecierre.Text.Trim = "", "0", tbCajaPrecierre.Text.Trim), ",", "")
            Dim CXCTitulo As String = Replace(IIf(tbCXCtitulo.Text.Trim = "", "0", tbCXCtitulo.Text.Trim), ",", "")
            Dim CXCOtras As String = Replace(IIf(tbCXCotras.Text.Trim = "", "0", tbCXCotras.Text.Trim), ",", "")
            Dim CXPTitulo As String = Replace(IIf(tbCXPtitulo.Text.Trim = "", "0", tbCXPtitulo.Text.Trim), ",", "")
            Dim OtrosGastos As String = Replace(IIf(tbOtrosGastos.Text.Trim = "", "0", tbOtrosGastos.Text.Trim), ",", "")
            Dim OtrosIngresos As String = Replace(IIf(tbOtrosIngresos.Text.Trim = "", "0", tbOtrosIngresos.Text.Trim), ",", "")
            Dim AporteValores As String = Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", "")
            Dim ValCuotaPrecierre As String = Replace(IIf(tbValCuotaPrecierre.Text.Trim = "", "0", tbValCuotaPrecierre.Text.Trim), ",", "")
            Dim RescatesValores As String = Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")
            Dim OtrosGastosCierre As String = Replace(IIf(tbOtrosGastosCierre.Text.Trim = "", "0", tbOtrosGastosCierre.Text.Trim), ",", "")
            Dim OtrosIngresosCierre As String = Replace(IIf(tbOtrosIngresosCierre.Text.Trim = "", "0", tbOtrosIngresosCierre.Text.Trim), ",", "")
            Dim ChequePendiente As String = Replace(IIf(txtChequePendiente.Text.Trim = "", "0", txtChequePendiente.Text.Trim), ",", "")
            Dim RescatePendiente As String = Replace(IIf(txtRescatePendiente.Text.Trim = "", "0", txtRescatePendiente.Text.Trim), ",", "")
            Dim VCAnterior As String = Replace(IIf(txtVCA.Text.Trim = "", "0", txtVCA.Text.Trim), ",", "")
            Dim AjustesCXP As String = Replace(IIf(txtAjustesCXP.Text = "", 0, txtAjustesCXP.Text), ",", "")
            Dim DevolucionComisionUnificada As String = Replace(IIf(txtDevolucionComisionUnificada.Text.Trim = String.Empty, "0", txtDevolucionComisionUnificada.Text.Trim), ",", "")
            Dim CXPOtras As Decimal = 0
            Dim CxCOtrasPreCierre As Decimal = 0
            CxCOtrasPreCierre = Math.Round(Decimal.Parse(txtMontoDividendosPrecierre.Text), 7)
            txtOtrasCxCPreCierre.Text = CxCOtrasPreCierre
            txtComisionUnificadaCuota.Text = Replace(IIf(txtComisionUnificadaCuota.Text = "", 0, txtComisionUnificadaCuota.Text), ",", "")
            txtComisionUnificadaMandato.Text = Replace(IIf(txtComisionUnificadaMandato.Text = "", 0, txtComisionUnificadaMandato.Text), ",", "")
            'txtAporteMandato.Text = Replace(IIf(txtAporteMandato.Text = "", 0, txtAporteMandato.Text), ",", "")
            'txtRetiroMandato.Text = Replace(IIf(txtRetiroMandato.Text = "", 0, txtRetiroMandato.Text), ",", "")
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se obtiene de nuevo campo AjustesCXC para incluirlo en el procesamiento del CXC Precierre | 16/07/18 
            Dim AjustesCXC As String = Replace(IIf(txtAjustesCXC.Text = String.Empty, "0", txtAjustesCXC.Text), ",", "")
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se obtiene de nuevo campo AjustesCXC para incluirlo en el procesamiento del CXC Precierre | 16/07/18 

            Dim ComisionSAFSisOpe As Decimal = ObtenerComisionSAFSisOpe()

            'OT 9851 27/01/2017 - Carlos Espejo
            'Descripcion: Si es seriado el fondo se calcula Otras CXP.
            'La caja se ve afectada por los cheques pendietes
            If oRowPortafolioBE.TipoCalculoValorCuota = "02" Then
                CajaPrecierre = Decimal.Parse(hdCajaPrecierre.Value) + Decimal.Parse(ChequePendiente)
            Else
                CajaPrecierre = Decimal.Parse(tbCajaPrecierre.Text) + Decimal.Parse(ChequePendiente)
            End If

            tbCajaPrecierre.Text = CajaPrecierre
            'OT 9981 15/02/2017 - Carlos Espejo
            'Descripcion: Se cambia el uso de ComisionSAFMAnterior

            '*********************************************************************************************************************************
            'Portafolio FIR no hace el calculo
            '*********************************************************************************************************************************
            If oRowPortafolioBE.TipoCalculoValorCuota = "02" Then
                Dim dtOtrasCXP = ValorCuota.OtrasCXP(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                Decimal.Parse(ChequePendiente), Decimal.Parse(RescatePendiente), UIUtility.ConvertirFechaaDecimal(txtComisionSAFMAnterior.Text), Decimal.Parse(txtDevolucionComisionUnificada.Text))
                CXPOtras = dtOtrasCXP.Rows(0)("OtrasCXP")
                txtComisionSAFM.Text = Decimal.Parse(dtOtrasCXP.Rows(0)("ComisionSAFM")) - Decimal.Parse(txtDevolucionComisionUnificada.Text)
                txtCajaRecaudo.Text = dtOtrasCXP.Rows(0)("CajaRecaudo")
                txtSuscripcion.Text = dtOtrasCXP.Rows(0)("Suscripcion")
                txtRescateP.Text = dtOtrasCXP.Rows(0)("RescatePendiente")
                txtChequeP.Text = dtOtrasCXP.Rows(0)("ChequePendiente")
                'INICIO | ZOLUXIONES | rcolonia | Se recupera la distribución programada de intereses | 02-11-2018
                txtDistribucionProgramada.Text = dtOtrasCXP.Rows(0)("totalInteresDistribucion")
                'FIN | ZOLUXIONES | rcolonia | Se recupera la distribución programada de intereses  | 02-11-2018
            Else
                CXPOtras = Decimal.Parse(tbCXPotras.Text)
            End If

            'OT 9981 Fin
            'OT 9851 Fin
            'Calculo
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se incluye el nuevo campo AjustesCXC en el cáculo ValPatriPrecierre | 07/06/18 
            tbValPatriPrecierre.Text =
            Decimal.Parse(InversionesSubtotal) + Decimal.Parse(CajaPrecierre) + Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) -
            Decimal.Parse(CXPTitulo) - Decimal.Parse(CXPOtras) - Decimal.Parse(AjustesCXP) - Decimal.Parse(OtrosGastos) +
            Decimal.Parse(OtrosIngresos) +
            Decimal.Parse(AjustesCXC) +
            Decimal.Parse(CxCOtrasPreCierre)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se incluye el nuevo campo AjustesCXC en el cáculo ValPatriPrecierre | 07/06/18 
            'Comision Variable
            margenValorCuota = oRowPortafolioBE.TopeValorCuota
            suscripcionIni = oRowPortafolioBE.MontoSuscripcionInicial
            'Calculo Nuevo Comision SAFM
            valPatriPreCierre = Decimal.Parse(tbValPatriPrecierre.Text.Trim)
            If CType(oRowPortafolioBE.FlagComisionVariable, Boolean) And (valPatriPreCierre > margenValorCuota Or CType(oRowPortafolioBE.FlagComisionSuscripInicial, Boolean)) Then
                valPatriPreCierre = suscripcionIni
            End If
            saldoPatriPreCierre = valPatriPreCierre
            existeSaldo = If(saldoPatriPreCierre > 0, True, False)
            cantComi = listPortafolioPCBE.Count
            i = 0
            Do While cantComi > 0 And existeSaldo And i < cantComi
                Dim obj As PortafolioPorcentajeComisionBE = listPortafolioPCBE(i)
                If saldoPatriPreCierre > obj.ValorMargenMaximo Then
                    comisionActual = ((obj.ValorPorcentajeComision / 100) / 360) * (1 + ParametrosSIT.IGV) * obj.ValorMargenMaximo
                    saldoPatriPreCierre = valPatriPreCierre - obj.ValorMargenMaximo
                Else
                    comisionActual = ((obj.ValorPorcentajeComision / 100) / 360) * (1 + ParametrosSIT.IGV) * saldoPatriPreCierre
                    existeSaldo = False
                End If

                comisionAcumulada += comisionActual
                i += 1
            Loop

            If oRowPortafolioBE.TipoComision = "2" Then
                Dim fechaFinMes As Date
                fechaFinMes = DateSerial(Now.Year, Now.Month + 1, 1 - 1)
                If fechaFinMes = Now.Date Then 'Obtener el ultimo dia del mes para comparar
                    If oRowPortafolioBE.TipoNegocio = "MANDA" Then
                        txtComisionUnificadaMandato.Text = comisionAcumulada - ComisionSAFSisOpe
                        tbComiSAFM.Text = 0
                    Else
                        txtComisionUnificadaMandato.Text = 0
                        tbComiSAFM.Text = comisionAcumulada - ComisionSAFSisOpe - Decimal.Parse(txtDevolucionComisionDiaria.Text)
                    End If
                End If
            Else
                If oRowPortafolioBE.TipoNegocio = "MANDA" Then
                    txtComisionUnificadaMandato.Text = comisionAcumulada - ComisionSAFSisOpe
                    tbComiSAFM.Text = 0
                Else
                    tbComiSAFM.Text = comisionAcumulada - ComisionSAFSisOpe - Decimal.Parse(txtDevolucionComisionDiaria.Text)
                    txtComisionUnificadaMandato.Text = 0
                End If
            End If
            'Agregado de Comision a Comision Unificada Mandato - OT12003 Junior Huallullo P.


            If oRowPortafolioBE.TipoNegocio = "MANDA" Then
                tbValPatriPrecierre2.Text = (Decimal.Parse(tbValPatriPrecierre.Text.Trim))
            Else
                If oRowPortafolioBE.TipoComision = "2" Then
                    Dim fechaFinMes As Date
                    fechaFinMes = DateSerial(Now.Year, Now.Month + 1, 1 - 1)
                    If fechaFinMes = Now.Date Then 'Obtener el ultimo dia del mes para comparar
                        tbValPatriPrecierre2.Text = (Decimal.Parse(tbValPatriPrecierre.Text.Trim) - comisionAcumulada)
                    Else
                        tbValPatriPrecierre2.Text = (Decimal.Parse(tbValPatriPrecierre.Text.Trim))
                    End If
                Else
                    tbValPatriPrecierre2.Text = (Decimal.Parse(tbValPatriPrecierre.Text.Trim) - comisionAcumulada)
                End If
            End If
            margenValorCuota = oRowPortafolioBE.TopeValorCuota
            suscripcionIni = oRowPortafolioBE.MontoSuscripcionInicial
            'Monto Valor Cuota Precierre
            If Decimal.Parse(ValCuotaPrecierre) <> 0 Then
                tbValValoresPrecierre.Text = Decimal.Parse(tbValPatriPrecierre2.Text.Trim) / Decimal.Parse(ValCuotaPrecierre)
                RescatesValores = ObtenerRescateValores() 'OT10965 - 29/11/2017 - Ian Pastor M.
                tbRescatesValores.Text = RescatesValores
            End If
            If CDec(Replace(IIf(tbValValoresPrecierre.Text.Trim = "", "0", tbValValoresPrecierre.Text.Trim), ",", "")) <> 0 Then
                'Calcular Aportes Cuotas
                tbAporteCuota.Text = Decimal.Parse(AporteValores) / Decimal.Parse(tbValValoresPrecierre.Text.Trim)
                'Calcular Rescate Cuos
                tbRescatesCuota.Text = Decimal.Parse(RescatesValores) / Decimal.Parse(tbValValoresPrecierre.Text.Trim)
                'Calcular Comision Unificada Cuotas - OT12003 - 03/06/2019 - Junior Huallullo P.
                txtComisionUnificadaCuota.Text = Decimal.Parse(txtComisionUnificadaMandato.Text) / Decimal.Parse(tbValValoresPrecierre.Text.Trim)
            Else
                tbAporteCuota.Text = 0
                tbRescatesCuota.Text = 0
                txtComisionUnificadaCuota.Text = 0
            End If
            'Asignando Caja Cierre de "Caja Precierre"
            tbCajaCierre.Text = Decimal.Parse(CajaPrecierre)
            'CALCULAR EL VALOR PATRIMONIO CIERRE
            If oRowPortafolioBE.TipoNegocio = "MANDA" Then
                Dim comUniCuoMan As Decimal = 0
                Dim aporteMandato As Decimal = 0
                Dim retiroMandato As Decimal = 0
                If Decimal.Parse(tbValValoresPrecierre.Text.Trim) <> 0 Then
                    comUniCuoMan = Decimal.Parse(txtComisionUnificadaMandato.Text) / Decimal.Parse(tbValValoresPrecierre.Text.Trim)
                    aporteMandato = Decimal.Parse(tbAporteValores.Text) / Decimal.Parse(tbValValoresPrecierre.Text.Trim)
                    retiroMandato = Decimal.Parse(tbRescatesValores.Text) / Decimal.Parse(tbValValoresPrecierre.Text.Trim)
                End If
                tbValPatCierreCuota.Text = Decimal.Parse(ValCuotaPrecierre) + Decimal.Parse(tbAporteCuota.Text.Trim) - Decimal.Parse(tbRescatesCuota.Text.Trim) - Decimal.Parse(txtComisionUnificadaCuota.Text)
                tbValPatCierreValores.Text = Decimal.Parse(tbValPatriPrecierre2.Text.Trim) _
                    + Decimal.Parse(OtrosIngresosCierre) - Decimal.Parse(OtrosGastosCierre) - Decimal.Parse(txtComisionUnificadaMandato.Text) - Decimal.Parse(tbRescatesValores.Text) + Decimal.Parse(tbAporteValores.Text)
            Else
                tbValPatCierreCuota.Text = Decimal.Parse(ValCuotaPrecierre) + Decimal.Parse(tbAporteCuota.Text.Trim) - Decimal.Parse(tbRescatesCuota.Text.Trim)
                tbValPatCierreValores.Text = Decimal.Parse(tbValPatriPrecierre2.Text.Trim) + Decimal.Parse(AporteValores) - Decimal.Parse(RescatesValores) _
                    + Decimal.Parse(OtrosIngresosCierre) - Decimal.Parse(OtrosGastosCierre)
            End If

            'EL VALOR CUOTA CIERRE
            tbValCuotaCierreCuota.Text = Decimal.Parse(tbValPatCierreCuota.Text.Trim)
            If CDec(Replace(IIf(tbValCuotaCierreCuota.Text.Trim = "", "0", tbValCuotaCierreCuota.Text.Trim), ",", "")) <> 0 Then
                tbValCuotaCierreValores.Text = Decimal.Parse(tbValPatCierreValores.Text.Trim) / Decimal.Parse(tbValCuotaCierreCuota.Text.Trim)
            Else
                tbValCuotaCierreValores.Text = 0
            End If
            'Páginas modificadas: ParametrosSIT.vb y tambien capas
            tbCXPotrasCierre.Text = Decimal.Parse(CXPOtras) + Decimal.Parse(AjustesCXP) + IIf(oRowPortafolioBE.TipoNegocio = "MANDA", Decimal.Parse(txtComisionUnificadaMandato.Text), Decimal.Parse(tbComiSAFM.Text)) + Decimal.Parse(RescatesValores) - Decimal.Parse(AporteValores)
            Try
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se incluye el nuevo campo AjustesCXC en el cáculo txtCXCPrecierre | 07/06/18 
                'txtCXCPrecierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) + Decimal.Parse(txtMontoDividendosPrecierre.Text) + Decimal.Parse(AjustesCXC) + Decimal.Parse(DevolucionComisionUnificada), 7)
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se incluye el nuevo campo AjustesCXC en el cáculo txtCXCPrecierre | 07/06/18
                txtCXCPrecierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) + Decimal.Parse(AjustesCXC) + Decimal.Parse(CxCOtrasPreCierre), 7)
            Catch ex As Exception
                txtCXCPrecierre.Text = "0"
            End Try
            Try
                txtCXPPrecierre.Text = Math.Round(Decimal.Parse(CXPTitulo) + Decimal.Parse(CXPOtras) + Decimal.Parse(AjustesCXP), 7).ToString
            Catch ex As Exception
                txtCXPPrecierre.Text = "0"
            End Try
            'Cierre Titulo
            tbCXCtituloCierre.Text = tbCXCtitulo.Text
            tbCXPtituloCierre.Text = tbCXPtitulo.Text
            tbCXCotrasCierre.Text = Math.Round(Decimal.Parse(CXCOtras), 7)
            'Cierre Totales CXC y CXP
            Try
                tbCXCCierre.Text = Math.Round(Decimal.Parse(tbCXCtituloCierre.Text) + Decimal.Parse(tbCXCotrasCierre.Text) + Decimal.Parse(AjustesCXC) + Decimal.Parse(CxCOtrasPreCierre), 7).ToString
            Catch ex As Exception
                tbCXCCierre.Text = "0"
            End Try
            Try
                tbCXPCierre.Text = Math.Round(Decimal.Parse(tbCXPtituloCierre.Text) + Decimal.Parse(tbCXPotrasCierre.Text), 7).ToString
            Catch ex As Exception
                tbCXPCierre.Text = "0"
            End Try
            btnAceptar.Visible = True
            'Ajustes
            tbValCuotaCierreValores.Text = Decimal.Parse(tbValCuotaCierreValores.Text)
            tbCXPotras.Text = CXPOtras
            'Diferencia entre el valor cuota del dia anterior y el calculado Hoy

            'INICIO | ZOLUXIONES | DACV | SPRINT III - 20180617
            diferencia = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(txtVCA.Text)
            txtVCDiferencia.Text = Math.Round(diferencia / IIf(Decimal.Parse(tbValCuotaCierreValores.Text) = 0, 1, Decimal.Parse(tbValCuotaCierreValores.Text)) * 100, 7)
            'txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(VCAnterior)

            'FIN | ZOLUXIONES | DACV | SPRINT III - 20180617


            If oRowPortafolioBE.PorSerie = "S" Then
                tbComiSAFM.Text = "0"
                txtDevolucionComisionDiaria.Text = "0"

            End If

        Catch ex As Exception
            AlertaJS("Ocurrió un error al procesar los datos: " & Replace(ex.Message, "'", ""))
        Finally
            DetallePrecierre()
        End Try
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oValorCuotaBE As New ValorCuotaBE
            Dim rowValorCuota As ValorCuotaBE.ValorCuotaRow
            Dim rowDetalleCuota As ValorCuotaBE.ValorCuotaSerieRow
            rowValorCuota = CType(oValorCuotaBE.ValorCuota.NewRow(), ValorCuotaBE.ValorCuotaRow)
            rowDetalleCuota = CType(oValorCuotaBE.ValorCuotaSerie.NewRow(), ValorCuotaBE.ValorCuotaSerieRow)
            CargarObjetoDetalle(rowDetalleCuota)
            CargarObjetoCabecera(rowValorCuota)

            If Not ValidarCuotaPreCierre() Then
                oValorCuotaBE.ValorCuota.AddValorCuotaRow(rowValorCuota)
                oValorCuotaBE.ValorCuotaSerie.AddValorCuotaSerieRow(rowDetalleCuota)
                oValorCuotaBE.AcceptChanges()
                ValorCuota.Insertar_ValorCuota(oValorCuotaBE, DatosRequest)
                ValorCuota.PrecioValorCuota(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), DatosRequest)
                AlertaJS("Registro guardado correctamente.")
            Else
                AlertaJS("Existe un precierre generado desde el sistema de Operaciones, no se puede modificar la informacion presentada.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            LimpiarControlesOcultos()
        End Try
    End Sub
    Protected Sub btnRefrescar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefrescar.Click
        Try
            listButtons.Attributes.Add("style", "display:inline")
            CargaPantalla()
            DetallePrecierre()
            If getTipoNegocio() = "FONDO" Then
                CargaHiddenCheckFondo()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim dtTabla As New DataTable
            dtTabla = ValorCuota.ReporteValorCuota(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text)
            dtTabla.TableName = "dsValorCuota"
            If dtTabla.Rows.Count > 0 Then
                oReport.Load(Server.MapPath("Reportes/rptValorCuota.rpt"))
                oReport.SetDataSource(dtTabla)
                oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                oReport.SetParameterValue("SubtototalInv", tbInversionesSubTotal.Text)
                oReport.SetParameterValue("IGV", txtigv.Text)
                oReport.SetParameterValue("Comision", txtporcom.Text)
                oReport.SetParameterValue("Usuario", Usuario)
                oReport.SetParameterValue("ComprasT", tbComprasT.Text)
                Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
                Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                exportOpts.ExportFormatOptions = pdfOpts
                oReport.ExportToHttpResponse(exportOpts, Response, True, "ValorCuota")
            Else
                AlertaJS("No se ha registrado el calculo de Valor Cuota para este dia y portafolio.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub form1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
    'OT10916 - 31/10/2017 - Ian Pastor Mendoza. Desc: Lógica para obtener los dividendos decretados por custodia
    Protected Sub btnVerDistribucion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerDistribucion.Click
        Try
            ConsultarDistribucionLib() 'OT10981 - 05/12/2017 - Hanz Cocchi. Consultar previamente si existe distribución
            Dim mensaje As String = String.Empty
            mensaje = VerDistribucionLiberadas()
            AlertaJS2(mensaje, "GrabarDistribucionLib();") 'OT10927 - 21/11/2017 - Hanz Cocchi. Mensaje de distribución del portafolio
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub cbHabilitarCarga_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbHabilitarCarga.CheckedChanged
        btnCarga.Enabled = IIf(cbHabilitarCarga.Checked = True, False, True)
        If cbHabilitarCarga.Checked Then
            iptRuta.Attributes.Add("disabled", "true")
        Else
            iptRuta.Attributes.Remove("disabled")
        End If
    End Sub
    Private Function VerDistribucionLiberadas() As String
        VerDistribucionLiberadas = String.Empty
        Dim oPortafolioBM As New PortafolioBM
        Dim dtPorcentaje As DataTable
        Dim fechaAnterior As String
        fechaAnterior = fnFechaAnteriorSinFeriado(tbFechaOperacion.Text)
        dtPorcentaje = oPortafolioBM.Portafolio_Series_Cuotas(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(fechaAnterior))
        Dim sumPorcentaje As Decimal = ExistePorcentajeSeries(dtPorcentaje)
        If sumPorcentaje = 0 Then
            Throw New System.Exception("No existen porcentajes series cargados a la fecha.")
        End If
        Dim objDividendosBM As New DividendosRebatesLiberadasBM
        Dim dt As DataTable
        dt = objDividendosBM.DividendosRebatesLiberadas_ObtenerMontoTotalDividendos(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        'If dt.Rows(0)("MontoTotalOperacion") <> 0 Then
        If dt.Rows.Count > 0 Then
            VerDistribucionLiberadas = CreateMensajeDistribucion(dt, dtPorcentaje)
        Else
            Throw New System.Exception("No existen dividendos liberados a la fecha.")
        End If
    End Function
    Private Function CreateMensajeDistribucion(ByVal dt As DataTable, ByVal dtPorcentaje As DataTable) As String
        CreateMensajeDistribucion = String.Empty
        Dim mensaje As New StringBuilder
        Dim porcentaje As Decimal
        Dim MontoOperacion As Decimal
        Dim MontoTotalOperacion As Decimal
        mensaje.Append("<table border=""0"" style=""width: 100%; text-align: left; font-weight: normal;"">")
        For Each dr As DataRow In dt.Rows
            MontoOperacion = Decimal.Parse(dr("MontoOperacion"))
            mensaje.Append("<tr>")
            mensaje.Append("<td colspan=""2"">" & dr("CodigoNemonico") & "</td>")
            mensaje.Append("<td style=""text-align:right;"">" & Format(MontoOperacion, "##,##0.00") & "</td>")
            mensaje.Append("</tr>")

            MontoTotalOperacion = MontoTotalOperacion + MontoOperacion
        Next
        mensaje.Append("<tr><td colspan=""3"">-------------------------------------------------------------------------</td></tr>")
        mensaje.Append("<tr><td colspan=""2"">Total Dividendos:</td><td style=""text-align:right;"">" & Format(MontoTotalOperacion, "##,##0.00") & "</td></tr>")

        mensaje.Append("<tr><td colspan=""3"" style=""text-align:center;"">")

        If Convert.ToInt16(hdCantRegDistribLib.Value) > 0 Then
            mensaje.Append("<input type=""button"" value=""Exportar rentabilidad"" onClick=""ExportarArchivo();"" class=""btn btn-integra"" style=""font-size:10px; width:120px;"" /></td></tr>")
        Else
            mensaje.Append("<input type=""button"" value=""Exportar rentabilidad"" onClick=""ExportarArchivo();"" disabled=""disabled"" class=""btn btn-integra"" style=""font-size:10px; width:120px;"" /></td></tr>")
        End If

        mensaje.Append("<tr><td colspan=""3"">&nbsp;</td></tr>")
        mensaje.Append("<tr><td colspan=""3"">-------------------------------------------------------------------------</td></tr>")
        For Each drPorcentaje As DataRow In dtPorcentaje.Rows
            porcentaje = Decimal.Parse(drPorcentaje("Porcentaje"))
            mensaje.Append("<tr><td>" & drPorcentaje("Nombre") & "</td><td style=""text-align:right;"">" & Format(porcentaje, "##,##0.0000000") & "%" & "</td><td style=""text-align:right;"">" & Format((MontoTotalOperacion * (porcentaje / 100)), "##,##0.0000000") & "</td></tr>")
        Next
        mensaje.Append("</table>")

        CreateMensajeDistribucion = mensaje.ToString()
    End Function
    Private Function ExistePorcentajeSeries(ByVal p_dtPorcentaje As DataTable) As Decimal
        ExistePorcentajeSeries = 0
        Dim porcentaje As Decimal = 0
        For Each dr As DataRow In p_dtPorcentaje.Rows
            porcentaje = porcentaje + Decimal.Parse(dr("Porcentaje"))
        Next
        ExistePorcentajeSeries = porcentaje
    End Function
    'OT10916 - Fin
    'OT10927 - 21/11/2017 - Hanz Cocchi. Exportar rentabilidad
    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Dim dt As DataTable
        dt = New ReporteGestionBM().reporteRentabilidad_Flujos(ddlPortafolio.SelectedValue, ConvertirFechaaDecimal(tbFechaOperacion.Text))
        copiaNotepad("Rentabilidad", dt)
    End Sub
    'OT10927 - 21/11/2017 - Hanz Cocchi. Grabar distribución de dividendos
    Protected Sub btnGrabarDistribucionLib_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabarDistribucionLib.Click
        Dim fechaAnterior As String
        fechaAnterior = fnFechaAnteriorSinFeriado(tbFechaOperacion.Text)

        Dim oDistribucion As DistribucionLibBE
        Dim dt As DataTable = New DividendosRebatesLiberadasBM().DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado_Flujos(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))

        'If dt.Rows.Count > 0 Then
        Dim rptaElim As Boolean = New DistribucionLibBM().Eliminar(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        'End If

        For Each fila As DataRow In dt.Rows
            If fila("CodigoSerie") = "SERB" Then
                oDistribucion = New DistribucionLibBE

                oDistribucion.CodigoPortafolioSBS = fila("CodigoPortafolioSBS")
                oDistribucion.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
                'oDistribucion.CodigoNemonico = dt.Rows(0)("CodigoMnemonico")
                oDistribucion.CodigoNemonico = fila("CodigoMnemonico") 'OT10981 - 05/12/2017 - Hanz Cocchi. Grabar Nemónico
                oDistribucion.Serie = fila("CodigoSerie")
                oDistribucion.Monto = (fila("MontoOperacion") * (fila("Porcentaje") / 100))

                Dim rpta As Boolean = New DistribucionLibBM().Insertar(oDistribucion, Me.DatosRequest)
            End If
        Next
        ConsultarDistribucionLib()
    End Sub
    Private Sub ConsultarDistribucionLib()
        Dim dt As DataTable = New DividendosRebatesLiberadasBM().ConsultarDistribucionLib(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If Not dt Is Nothing Then
            If dt.Rows.Count > 0 Then
                hdCantRegDistribLib.Value = dt.Rows(0)(0).ToString()
            Else
                hdCantRegDistribLib.Value = 0
            End If
        Else
            hdCantRegDistribLib.Value = 0
        End If
    End Sub
    'OT10927 - Fin
    'OT10927 - 21/11/2017 - Hanz Cocchi. Creación del archivo txt para el reporte de rentabilidad
    Private Sub copiaNotepad(ByVal Archivo As String, ByVal dt As DataTable, Optional ByVal dtPortafolio As DataTable = Nothing)
        Dim sRutaArchivo As String
        Dim nombreArchivo As String
        Dim sufijo As String
        nombreArchivo = Archivo & "_" & ddlPortafolio.SelectedItem.Text & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".txt"
        sRutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
        sufijo = Request.QueryString("RPT")
        escribirArchivo(sRutaArchivo, dt)
        System.GC.Collect()
        Response.Clear()
        Response.ContentType = "application/txt"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
        Response.WriteFile(sRutaArchivo)
        Response.End()
    End Sub
    Private Sub escribirArchivo(ByVal rutaArchivo As String, ByVal dt As DataTable)
        Dim lineaArchivo As String
        Dim escritura As New IO.StreamWriter(rutaArchivo)
        escritura.Flush()
        For i = 0 To dt.Rows.Count - 1
            lineaArchivo = ""
            For j = 0 To dt.Columns.Count - 1
                'Remueve Acentos
                lineaArchivo = UIUtility.RemoveDiacritics(lineaArchivo & dt.Rows(i)(j).ToString.Trim & ";")
            Next
            lineaArchivo = lineaArchivo.Remove(lineaArchivo.Length - 1, 1)
            escritura.WriteLine(lineaArchivo)
        Next
        escritura.Close()
    End Sub
    'OT10927 - Fin
    Private Function ObtenerRescateValores() As Decimal
        ObtenerRescateValores = 0
        Dim dtRescateValores As DataTable
        Try
            dtRescateValores = ValorCuota.ValorCuota_ObtenerRescateValores(hdCodigoPortafolioSisOpe.Value, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), "RES")
        Catch ex As Exception
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'OT10989 - 11/12/2017 - Ian Pastor M. Devuelve el valor que esté en la caja de texto.
            Exit Function
        End Try
        Dim valorCuotaDia As String = Replace(IIf(tbValValoresPrecierre.Text.Trim = "", "0", tbValValoresPrecierre.Text.Trim), ",", "")
        If Decimal.Parse(valorCuotaDia) = 0 Then
            ObtenerRescateValores = 0
            Exit Function
        End If
        Dim rescateCuotas As Decimal = 0
        Dim rescateValores As Decimal = 0
        If dtRescateValores.Rows.Count > 0 Then
            If Not IsDBNull(dtRescateValores.Rows(0)("VALOR1")) And Not IsDBNull(dtRescateValores.Rows(0)("VALOR2")) Then
                rescateCuotas = dtRescateValores.Rows(0)("VALOR2") * Decimal.Parse(valorCuotaDia)
                rescateValores = Math.Round(dtRescateValores.Rows(0)("VALOR1") + rescateCuotas, 7)
            End If
        End If
        If rescateValores > 0 Then 'OT10986 - 15/12/2017 - Ian Pastor M. Si el rescate valor es cero, se mantiene el valor de la caja de texto.
            ObtenerRescateValores = rescateValores
        Else
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'OT10989 - 11/12/2017 - Ian Pastor M. Devuelve el valor que esté en la caja de texto.
        End If
    End Function
    'OT11008 - 19/01/2018 - Ian Pastor M.
    'Descripción: Obtiene los rescates pendientes del sistema de operaciones
    Private Function ObtenerRescatesPendientes() As Decimal
        ObtenerRescatesPendientes = 0
        Dim montoRescatePND As Decimal = 0
        Try
            montoRescatePND = ValorCuota.ValorCuota_ObtenerRescatesPendientes(ddlPortafolio.SelectedValue, hdCodigoPortafolioSisOpe.Value, _
                                                                                 UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), hdSeriado.Value)
        Catch ex As Exception
            ObtenerRescatesPendientes = Decimal.Parse(Replace(IIf(txtRescatePendiente.Text.Trim = "", "0", txtRescatePendiente.Text.Trim), ",", ""))
            Exit Function
        End Try
        If montoRescatePND > 0 Then
            ObtenerRescatesPendientes = montoRescatePND
        Else
            ObtenerRescatesPendientes = Decimal.Parse(Replace(IIf(txtRescatePendiente.Text.Trim = "", "0", txtRescatePendiente.Text.Trim), ",", ""))
        End If
    End Function
    'OT11008 - Fin
    Private Sub CargarPorcentajeSeries()
        Dim bol As Boolean = ValorCuota.ValorCuota_ObtenerPorcentajeSeries(hdCPPadreSisOpe.Value, tbFechaOperacion.Text, ddlPortafolio.SelectedValue, ddlPortafolio.SelectedItem.Text, DatosRequest)
        If bol Then
            dgArchivo.DataSource = Nothing
            dgArchivo.DataBind()
            Dim oPortafolioBM As New PortafolioBM
            dgArchivo.DataSource = oPortafolioBM.Portafolio_Series_Cuotas(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            dgArchivo.DataBind()
            For Each row As GridViewRow In dgArchivo.Rows
                CType(row.FindControl("btnModificar"), ImageButton).Enabled = True
            Next
            'AlertaJS("La carga se realizó con éxito.")
        End If
    End Sub
    'OT11192 - 12/03/2018 - Ian Pastor M.
    'Descripción: Obtiene el monto de aporte de valores del sistema de operaciones
    Private Function ObtenerAporteValores() As Decimal
        ObtenerAporteValores = 0
        Dim montoAporteValores As Decimal = 0
        Try
            montoAporteValores = ValorCuota.ObtenerAporteValoresSisOpe(hdCodigoPortafolioSisOpe.Value, tbFechaOperacion.Text)
        Catch ex As Exception
            ObtenerAporteValores = Decimal.Parse(Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", ""))
            Exit Function
        End Try
        If montoAporteValores > 0 Then
            ObtenerAporteValores = montoAporteValores
        Else
            ObtenerAporteValores = Decimal.Parse(Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", ""))
        End If
    End Function
    'OT11192 - Fin
    Private Function ObtenerChequePendiente() As Decimal
        ObtenerChequePendiente = 0
        Dim montoChequePendiente As Decimal = 0
        Try
            montoChequePendiente = ValorCuota.ObtenerChequePendienteSisOpe(ddlPortafolio.SelectedValue, hdCodigoPortafolioSisOpe.Value, _
                                                                           hdSeriado.Value)
        Catch ex As Exception
            ObtenerChequePendiente = Decimal.Parse(Replace(IIf(txtChequePendiente.Text.Trim = "", "0", txtChequePendiente.Text.Trim), ",", ""))
            Exit Function
        End Try
        If montoChequePendiente > 0 Then
            ObtenerChequePendiente = montoChequePendiente
        Else
            ObtenerChequePendiente = Decimal.Parse(Replace(IIf(txtChequePendiente.Text.Trim = "", "0", txtChequePendiente.Text.Trim), ",", ""))
        End If
    End Function
    Private Function ObtenerDevolucionComisionUnificada(Optional ByVal fechaInicio As String = "", Optional ByVal fechaFin As String = "") As Decimal
        ObtenerDevolucionComisionUnificada = 0
        Dim montoDevolucionComisionUnificada As Decimal = 0D
        If fechaInicio = "" Then
            fechaInicio = txtComisionSAFMAnterior.Text
        End If

        If fechaFin = "" Then
            fechaFin = tbFechaOperacion.Text
        End If

        Try
            montoDevolucionComisionUnificada = ValorCuota.ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe(fechaInicio, _
                                                                                                              fechaFin, _
                                                                                                              ddlPortafolio.SelectedItem.ToString)
            Return montoDevolucionComisionUnificada
        Catch ex As Exception
            ObtenerDevolucionComisionUnificada = Decimal.Parse(Replace(IIf(txtDevolucionComisionUnificada.Text.Trim = String.Empty, "0", txtDevolucionComisionUnificada.Text.Trim), ",", ""))
            Exit Function
        End Try
        'If (montoDevolucionComisionUnificada > 0 And hdCambioOtrasCxC.Value = "") Then
        '    ObtenerDevolucionComisionUnificada = montoDevolucionComisionUnificada
        'Else
        '    If hdCambioOtrasCxC.Value = "" Then
        '        ObtenerDevolucionComisionUnificada = Decimal.Parse(Replace(IIf(txtDevolucionComisionUnificada.Text.Trim = String.Empty, "0", txtDevolucionComisionUnificada.Text.Trim), ",", ""))
        '    Else
        '        ObtenerDevolucionComisionUnificada = Decimal.Parse(Replace(IIf(hdDevolucionComisionUnificada.Value.Trim = String.Empty, "0", hdDevolucionComisionUnificada.Value.Trim), ",", ""))
        '    End If
        'End If
    End Function
    Private Function ObtenerComisionSAFSisOpe() As Decimal
        ObtenerComisionSAFSisOpe = 0
        Try
            ObtenerComisionSAFSisOpe = ValorCuota.ObtenerComisionSAFSisOpe(ddlPortafolio.SelectedValue, hdCodigoPortafolioSisOpe.Value, _
                                                                           hdSeriado.Value, tbFechaOperacion.Text)
        Catch ex As Exception
            ObtenerComisionSAFSisOpe = 0
            Exit Function
        End Try
    End Function
    Private Sub CargarValoresAutomaticos()
        tbAporteValores.Text = ObtenerAporteValores()
        txtRescatePendiente.Text = ObtenerRescatesPendientes()
        tbRescatesValores.Text = ObtenerRescateValores()
        txtChequePendiente.Text = ObtenerChequePendiente() 'OT11339 - Ian Pastor M. 31/05/2018 - Obtener cheque pendiente.
        txtDevolucionComisionUnificada.Text = ObtenerDevolucionComisionUnificada() 'Nuevo Campo para obtener desde operaciones la devolución de comisión unificada.
    End Sub

    Private banco, ini, fin As String
    Protected Sub gvDesagradoCaja_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDesagradoCaja.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            banco = e.Row.Cells(0).Text

            If banco.Trim.Equals("RECAUDO") Or banco.Trim.Equals("INVERSIONES") Then
                e.Row.CssClass = "caberaGrilla"
            ElseIf banco.Trim.Equals("TOTALES") Then
                e.Row.Font.Bold = True
            End If

            ini = e.Row.Cells(1).Text

            If ini.Contains("Saldo") = False And ini <> "&nbsp;" Then
                e.Row.Cells(1).Text = String.Format("{0:0,0.0000000}", Convert.ToDecimal(ini))
            End If

            fin = e.Row.Cells(2).Text
            If fin.Contains("Saldo") = False And fin <> "&nbsp;" Then
                e.Row.Cells(2).Text = String.Format("{0:0,0.0000000}", Convert.ToDecimal(fin))
            End If

        End If

    End Sub

    Protected Sub txtDevolucionComisionUnificada_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDevolucionComisionUnificada.TextChanged
        Dim CXCTitulo As String = Replace(IIf(tbCXCtitulo.Text.Trim = String.Empty, "0", tbCXCtitulo.Text.Trim), ",", "")
        Dim CXCOtras As String = Replace(IIf(tbCXCotras.Text.Trim = String.Empty, "0", tbCXCotras.Text.Trim), ",", "")
        Dim AjustesCXC As String = Replace(IIf(txtAjustesCXC.Text = String.Empty, "0", txtAjustesCXC.Text), ",", "")
        Dim DevolucionComisionUnificada As String = Replace(IIf(txtDevolucionComisionUnificada.Text.Trim = String.Empty, "0", txtDevolucionComisionUnificada.Text.Trim), ",", "")

        txtCXCPrecierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) + Decimal.Parse(txtMontoDividendosPrecierre.Text) + Decimal.Parse(AjustesCXC) + Decimal.Parse(DevolucionComisionUnificada), 7)
    End Sub

    Private Sub LimpiarControlesOcultos()
        hdCambioOtrasCxC.Value = String.Empty
        hdOtrasCxCPreCierre.Value = String.Empty
        hdDevolucionComisionUnificada.Value = String.Empty
        'hdAporteMandato.Value = String.Empty
        'hdRetiroMandato.Value = String.Empty
    End Sub
    Protected Sub btnProcesarAuxiliar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesarAuxiliar.Click
        btnProcesar_Click(Nothing, Nothing)
    End Sub
End Class