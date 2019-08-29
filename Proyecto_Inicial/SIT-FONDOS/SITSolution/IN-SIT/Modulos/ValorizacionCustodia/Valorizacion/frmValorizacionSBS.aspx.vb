Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Linq.Expressions
Imports ParametrosSIT
Imports Sit.BusinessLayer.MotorInversiones

Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmValorizacionSBS
    Inherits BasePage

    Private oCarteraTituloValoracion As New CarteraTituloValoracionBM
    Private dtResultado As DataTable
    Private row As DataRow
    Private chkSelect As CheckBox
    Private img As Image
    Private hd As HiddenField
    Private hdCodigoPortafolio As HiddenField
    Private strRuta As String
    Private oValor As ValoresBM
    Private tipoValorizacion As String = String.Empty
    Private Portafolio As New PortafolioBM
    Private intresultado, cantidadreg As Integer
    Private lista As List(Of Portafolio)
    Private FechaApertura As Decimal
    Private msg, url, entExtPrecio, entExtTipoCambio As String
    Private idFondo As String
    Private valor As Decimal
    Private lstVisor As List(Of PortafolioVisor)
    Private ListaId As List(Of String)

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

        Dim ItemSeleccion As List(Of Portafolio)
        Dim CheckBoxArray As ArrayList


        If ViewState("ItemSeleccion") IsNot Nothing Then
            ItemSeleccion = DirectCast(ViewState("ItemSeleccion"), List(Of Portafolio))
        Else
            ItemSeleccion = New List(Of Portafolio)
        End If

        If ViewState("CheckBoxArray") IsNot Nothing Then
            CheckBoxArray = DirectCast(ViewState("CheckBoxArray"), ArrayList)
        Else
            CheckBoxArray = New ArrayList()
        End If

        If Not Page.IsPostBack Then
            ' INICIO --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
            Dim dtParametrosGenarales As DataTable = oParametrosGeneralesBM.Listar(TIPO_NEGOCIO, Me.DatosRequest)
            HelpCombo.LlenarComboBox(Me.ddlTipoNegocio, dtParametrosGenarales, "Valor", "Nombre", False)
            Me.ddlTipoNegocio.SelectedValue = "FONDO"
            ' FIN --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio

            Situacion()
            CargarDatosEstimacion()
        End If

        Dim CheckBoxIndex As Integer
        Dim CheckAllWasChecked As Boolean = False

        If gvresultado.HeaderRow IsNot Nothing Then
            Dim chkAll As CheckBox = DirectCast(gvresultado.HeaderRow.Cells(0).FindControl("SelectAllCheckBox"), CheckBox)
            Dim checkAllIndex As String = "SelectAllCheckBox-" & gvresultado.PageIndex
            If chkAll.Checked Then
                If CheckBoxArray.IndexOf(checkAllIndex) = -1 Then
                    CheckBoxArray.Add(checkAllIndex)
                End If
            Else
                If CheckBoxArray.IndexOf(checkAllIndex) <> -1 Then
                    CheckBoxArray.Remove(checkAllIndex)
                    CheckAllWasChecked = True
                End If
            End If
        End If

        Dim item As Portafolio
        For i As Integer = 0 To gvresultado.Rows.Count - 1
            If gvresultado.Rows(i).RowType = DataControlRowType.DataRow Then
                item = New Portafolio

                Dim chk As CheckBox = DirectCast(gvresultado.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                CheckBoxIndex = gvresultado.PageSize * gvresultado.PageIndex + (i + 1)
                If chk.Checked Then
                    If CheckBoxArray.IndexOf(CheckBoxIndex) = -1 And Not CheckAllWasChecked Then
                        CheckBoxArray.Add(CheckBoxIndex)

                        item = New Portafolio
                        item.Fila = CheckBoxIndex
                        hd = CType(gvresultado.Rows(i).Cells(0).FindControl("hdCodPortafolio"), HiddenField)
                        item.CodPortafolio = hd.Value
                        item.NomPortafolio = gvresultado.Rows(i).Cells(1).Text
                        hd = CType(gvresultado.Rows(i).Cells(0).FindControl("hdFecApertura"), HiddenField)
                        item.FechaApertura = Convert.ToDecimal(hd.Value)
                        ItemSeleccion.Add(item)

                    End If
                Else
                    If CheckBoxArray.IndexOf(CheckBoxIndex) <> -1 Or CheckAllWasChecked Then
                        CheckBoxArray.Remove(CheckBoxIndex)

                        item = New Portafolio
                        item.Fila = CheckBoxIndex
                        hd = CType(gvresultado.Rows(i).Cells(0).FindControl("hdCodPortafolio"), HiddenField)
                        item.CodPortafolio = hd.Value
                        item.NomPortafolio = gvresultado.Rows(i).Cells(1).Text
                        hd = CType(gvresultado.Rows(i).Cells(0).FindControl("hdFecApertura"), HiddenField)
                        item.FechaApertura = Convert.ToDecimal(hd.Value)
                        ItemSeleccion.Remove(item)

                    End If
                End If
            End If
        Next


        ViewState("CheckBoxArray") = CheckBoxArray
        ViewState("ItemSeleccion") = ItemSeleccion
    End Sub
    Private Sub CargarDatosEstimacion()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dt As DataTable = oParametrosGenerales.Listar("EntidadExt", DatosRequest)
        ddlPrecioEstimado.DataSource = dt
        ddlPrecioEstimado.DataTextField = "Nombre"
        ddlPrecioEstimado.DataValueField = "Valor"
        ddlPrecioEstimado.DataBind()
        ddlTipoCambioEstimado.DataSource = dt
        ddlTipoCambioEstimado.DataTextField = "Nombre"
        ddlTipoCambioEstimado.DataValueField = "Valor"
        ddlTipoCambioEstimado.DataBind()
    End Sub
    Private Function ObtenerPortafolio() As List(Of Portafolio)
        lista = New List(Of Portafolio)
        Dim portafolio As Portafolio
        For Each row As GridViewRow In Me.gvresultado.Rows
            chkSelect = CType(row.FindControl("chkSelect"), CheckBox)

            If chkSelect.Checked Then
                portafolio = New Portafolio
                hd = CType(row.FindControl("hdCodPortafolio"), HiddenField)
                portafolio.CodPortafolio = hd.Value
                portafolio.NomPortafolio = row.Cells(1).Text
                hd = CType(row.FindControl("hdFecApertura"), HiddenField)
                portafolio.FechaApertura = Convert.ToDecimal(hd.Value)
                lista.Add(portafolio)
            End If
        Next
        Return lista
    End Function
    Private Sub GrillaCheck(ByVal estado As Boolean, ByVal portafolio As Portafolio)

        For Each row As GridViewRow In Me.gvresultado.Rows
            chkSelect = CType(row.FindControl("chkSelect"), CheckBox)
            hd = CType(row.Cells(0).FindControl("hdCodPortafolio"), HiddenField)

            If hd.Value.Trim().Equals(portafolio.CodPortafolio) Then
                chkSelect.Checked = estado
            End If

        Next

        ViewState("ItemSeleccion") = New List(Of Portafolio)
        ViewState("CheckBoxArray") = New ArrayList()
    End Sub
    Private Sub ProcesarTipo_V(ByVal list As List(Of Portafolio), ByVal fechaOperacion As Decimal)

        For index = 0 To list.Count - 1
            If UIUtility.ConvertirFechaaDecimal(fechaOperacion) > list(index).FechaApertura Then
                AlertaJS("La fecha de operacion debe ser menor a la fecha de apertura actual.")
                Exit Sub
            End If
            If oValor.ValoracionValidarOperaciones(list(index).CodPortafolio, fechaOperacion) > 0 Then
                url = "../../Parametria/frmVisorOrdenesFaltantes.aspx?pfondo=" + list(index).CodPortafolio + "&pFecha=" + fechaOperacion + "&tipoOperacion=VAL"
                EjecutarJS("showWindow('" & url & "', '800', '600');")
                Exit Sub
            End If
            cantidadreg = oValor.ExisteValoracion(list(index).CodPortafolio, fechaOperacion)
            If cantidadreg > 0 Then
                AlertaJS("Ya existe una valorización para esta fecha, debe extornarla para crear una nueva.")
                Exit Sub
            End If

            entExtPrecio = ddlPrecioEstimado.SelectedItem.Text
            entExtTipoCambio = ddlTipoCambioEstimado.SelectedItem.Text

            If oValor.ValidarDRL_ParaValoracionEstimada(list(index).CodPortafolio, fechaOperacion) = 1 Then
                url = "../../Parametria/frmVisorOrdenesFaltantes.aspx?pfondo=" + list(index).CodPortafolio + "&pFecha=" + fechaOperacion + "&tipoOperacion=VALEST"
                EjecutarJS("showWindow('" & url & "', '800', '600');")
                Exit Sub
            End If
            oValor.EliminarValoracionCartera("E", list(index).CodPortafolio, fechaOperacion)

            intresultado = oValor.GenerarValorizacionCartera(list(index).CodPortafolio, fechaOperacion, entExtPrecio, entExtTipoCambio, tipoValorizacion, DatosRequest)
            If intresultado <> 2 Then
                AlertaJS("Ocurrio un error en el Proceso de Valoración")
            Else
                If oValor.VerificaNemonicosValorizacion > 0 Then
                    AlertaJS("Algunos Valores no tienen precio para la fecha que se esta Valorizando")
                    url = "Reportes/frmVisorErrorValoracion.aspx?pportafolio=" + list(index).CodPortafolio + "&pFecha=" + fechaOperacion
                    EjecutarJS("showWindow('" & url & "', '800', '600');")
                Else
                    oValor.CalculaMontoInversion(fechaOperacion, list(index).CodPortafolio)
                    oCarteraTituloValoracion.ReporteVLGenerar(fechaOperacion, list(index).CodPortafolio)
                    'EjecutarJS("window.showModalDialog('frmDiferenciaValorCuota.aspx?Fecha=" & tbFechaOperacion.Text & "','650','450','');")
                End If
            End If

            Dim decFecha As Decimal = oCarteraTituloValoracion.ObtenerFechaValoracion(list(index).CodPortafolio)
            Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(decFecha)

        Next
        AlertaJS("Se proceso correctamente")
    End Sub
    Private Sub ProcesarTipo_R(ByVal list As List(Of Portafolio), ByVal fechaOperacion As Decimal)

        Dim siguiente As Boolean
        For index = 0 To list.Count - 1

            siguiente = True

            If fechaOperacion > list(index).FechaApertura Then
                btnProcesar.Enabled = True
                AlertaJS("La fecha de operacion debe ser menor a la fecha de apertura actual.")
                GrillaCheck(False, list(index))
                siguiente = False

            End If

            If oValor.ValoracionValidarOperaciones(list(index).CodPortafolio, fechaOperacion) > 0 Then
                btnProcesar.Enabled = True
                url = "../../Parametria/frmVisorOrdenesFaltantes.aspx?pfondo=" & list(index).CodPortafolio + "&pFecha=" & fechaOperacion.ToString() & "&tipoOperacion=VAL"
                EjecutarJS("showWindow('" & url & "', '800', '600');")
                GrillaCheck(False, list(index))
                siguiente = False
            End If

            cantidadreg = oValor.ExisteValoracion(list(index).CodPortafolio, fechaOperacion)
            If cantidadreg > 0 Then
                AlertaJS("El Portafolio " & list(index).NomPortafolio & " tiene una valorización para esta fecha, debe extornarla para crear una nueva.")
                GrillaCheck(False, list(index))
                siguiente = False
            End If


            If siguiente Then
                entExtPrecio = "REAL"
                entExtTipoCambio = "REAL"
                tipoValorizacion = rblTipoValorizacion.SelectedValue
                intresultado = oValor.GenerarValorizacionCartera(list(index).CodPortafolio, fechaOperacion, entExtPrecio, entExtTipoCambio, tipoValorizacion, DatosRequest)

                If intresultado <> 2 Then
                    btnProcesar.Enabled = True
                    AlertaJS("Ocurrio un error en el Proceso de Valoración")
                    GrillaCheck(False, list(index))
                Else
                    'If oValor.VerificaNemonicosValorizacion > 0 Then
                    '    btnProcesar.Enabled = True
                    '    AlertaJS("Algunos Valores no tienen precio para la fecha que se esta Valorizando")
                    '    url = "Reportes/frmVisorErrorValoracion.aspx?pportafolio=" & list(index).CodPortafolio & "&pFecha=" + fechaOperacion.ToString()
                    '    EjecutarJS("showWindow('" & url & "', '800', '600');")
                    '    GrillaCheck(False, list(index))
                    '    siguiente = False
                    'Else
                    oValor.CalculaMontoInversion(fechaOperacion, list(index).CodPortafolio)
                    oCarteraTituloValoracion.ReporteVLGenerar(fechaOperacion, list(index).CodPortafolio)
                    'EjecutarJS("window.showModalDialog('frmDiferenciaValorCuota.aspx?Fecha=" & tbFechaOperacion.Text & "','650','450','');")
                    GrillaCheck(False, list(index))
                    'End If
                End If

                If ddlTipoNegocio.SelectedValue = "MANDA" Then
                    Dim incValorizacion As New List(Of KeyValuePair(Of String, String)) 'Incidentes del proceso
                    ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-31 | Generar la Valorización Amorizada
                    incValorizacion = GenerarValorizacionAmortizada(list(index).CodPortafolio, DateTime.ParseExact(fechaOperacion, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
                    ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-31 | Generar la Valorización Amorizada

                    '==== INICIO | PROYECTO FONDOS-MANDATOS - ZOLUXIONES | CRumiche | 2019-01-07 | Lista de mensajes al realizr Valorización Amortizada 
                    If incValorizacion.Count > 0 Then
                        For Each claveValor As KeyValuePair(Of String, String) In incValorizacion
                            'cuentasConcat = cuentasConcat & IIf(cuentasConcat.Length = 0, "", ", ") & dr("NumeroCuenta")

                            EjecutarJS(String.Format("agregarFilaMensajes('{0}', '{1}');", claveValor.Key, claveValor.Value.Replace("'", "")))
                        Next
                        EjecutarJS("mostrarPanelMensajes();")
                    End If
                    '==== FIN | PROYECTO FONDOS-MANDATOS - ZOLUXIONES | CRumiche | 2019-01-07 | Lista de mensajes al realizr Valorización Amortizada 
                End If

            End If
            list(index).Estado = siguiente
            list(index).FechaOperacion = fechaOperacion
        Next

        CargarDatosComplementarios(list)
        btnVer.Visible = True
        'hdProcesado.Value = "1"

    End Sub
    Private dsComplemento As DataSet
    Private Sub CargarDatosComplementarios(ByVal list As List(Of Portafolio))
        Dim cod As String = String.Empty
        Dim Fecha As String = String.Empty

        dsComplemento = New DataSet
        dsComplemento = Portafolio.CargarDatosComplementarios(ListadoPortafolio(list), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))


        If Session("dsComplemento") IsNot Nothing Then
            Session("dsComplemento") = Nothing
        End If


        Session("dsComplemento") = dsComplemento

        Dim resultado As DataTable = dsComplemento.Tables(0)
        dtResultado = Session("dtResultado")

        If resultado IsNot Nothing Then
            If resultado.Rows.Count > 0 Then
                For i = 0 To resultado.Rows.Count - 1

                    For index = 0 To dtResultado.Rows.Count - 1

                        If dtResultado(index)("codigoPortafolio").ToString() = resultado(i)("Portafolio").ToString() Then
                            dtResultado(index).BeginEdit()
                            dtResultado(index)("vl") = resultado.Rows(i)(1).ToString
                            dtResultado(index)("interesNegativo") = resultado.Rows(i)(2).ToString
                            dtResultado(index)("valorizacionGanancia") = resultado.Rows(i)(3).ToString
                            dtResultado(index)("invercionNula") = resultado.Rows(i)(4).ToString
                            dtResultado(index)("valorizacionNula") = resultado.Rows(i)(5).ToString
                            dtResultado(index).BeginEdit()
                        End If
                    Next

                Next
            End If
        End If


        Session("dtResultado") = dtResultado


        If resultado.Rows.Count > 0 Then

            If resultado.Rows(0)(0) <> "0" Then
                If dsComplemento.Tables(1).Rows.Count > 0 Or dsComplemento.Tables(2).Rows.Count > 0 Or
                    dsComplemento.Tables(3).Rows.Count > 0 Or dsComplemento.Tables(4).Rows.Count > 0 Or
                    dsComplemento.Tables(5).Rows.Count > 0 Then
                    AlertaJS("Se procesaron con anomalías")
                Else
                    AlertaJS("Se proceso correctamente")
                End If
            End If

        End If

        CargarInformacion()

        ViewState("ItemSeleccion") = Nothing
    End Sub
    Private Sub ProcesarTipo_C(ByVal list As List(Of Portafolio), ByVal fechaOperacion As Decimal)

        For index = 0 To list.Count - 1
            If oValor.VerificarTasasCurva(UIUtility.ConvertirFechaaDecimal(fechaOperacion)) = 0 Then
                AlertaJS("No existen valores de indicadores para T-1")
            Else
                oValor.EliminarValoracionCartera("C", list(index).CodPortafolio, UIUtility.ConvertirFechaaDecimal(fechaOperacion))
                intresultado = oValor.GenerarValorizacionCurvaCuponCero(list(index).CodPortafolio, UIUtility.ConvertirFechaaDecimal(fechaOperacion), DatosRequest)
                If intresultado <> 2 Then
                    AlertaJS("Ocurrio un error en el Proceso de Valoración de Curva Cupon Cero")
                Else
                    AlertaJS(ObtenerMensaje("ALERT168"))
                End If
            End If
        Next


    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            If hdOcultar.Value.Equals("1") Then
                oValor = New ValoresBM
                Dim fechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)

                If ViewState("ItemSeleccion") IsNot Nothing Then
                    lista = DirectCast(ViewState("ItemSeleccion"), List(Of Portafolio))
                End If
                If lista IsNot Nothing And lista.Count > 0 Then
                    entExtPrecio = String.Empty
                    entExtTipoCambio = String.Empty
                    Select Case rblTipoValorizacion.SelectedValue
                        Case "C"
                            ProcesarTipo_C(lista, fechaOperacion)
                        Case "V"
                            ProcesarTipo_V(lista, fechaOperacion)
                        Case "R"
                            ProcesarTipo_R(lista, fechaOperacion)
                    End Select
                End If

            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub Situacion()
        dtResultado = New DataTable
        dtResultado.Columns.Add(New DataColumn("Nombre", GetType(System.String)))
        dtResultado.Columns.Add(New DataColumn("Valor", GetType(System.Int32)))

        row = dtResultado.NewRow
        row("Nombre") = "Todos"
        row("Valor") = "-1"
        dtResultado.Rows.Add(row)

        row = dtResultado.NewRow
        row("Nombre") = "Por Valorar"
        row("Valor") = "0"
        dtResultado.Rows.Add(row)

        row = dtResultado.NewRow
        row("Nombre") = "Valorado"
        row("Valor") = "1"
        dtResultado.Rows.Add(row)



        Me.ddlSituacion.DataSource = dtResultado
        Me.ddlSituacion.DataTextField = "Nombre"
        Me.ddlSituacion.DataValueField = "Valor"
        Me.ddlSituacion.DataBind()

        Me.ddlSituacion.SelectedIndex = 0
    End Sub
    Private Sub CargarSeleccion()
        Dim CheckBoxArray As ArrayList
        If ViewState("CheckBoxArray") IsNot Nothing Then
            CheckBoxArray = DirectCast(ViewState("CheckBoxArray"), ArrayList)
        Else
            CheckBoxArray = New ArrayList()
        End If
        Dim CheckBoxIndex As Integer
        Dim CheckAllWasChecked As Boolean = False

        For i As Integer = 0 To gvresultado.Rows.Count - 1
            If gvresultado.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim chk As CheckBox = _
                 DirectCast(gvresultado.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                CheckBoxIndex = gvresultado.PageSize * gvresultado.PageIndex + (i + 1)
                If chk.Checked Then
                    If CheckBoxArray.IndexOf(CheckBoxIndex) = -1 And _
                        Not CheckAllWasChecked Then
                        CheckBoxArray.Add(CheckBoxIndex)
                    End If
                Else
                    If CheckBoxArray.IndexOf(CheckBoxIndex) <> -1 Or _
                        CheckAllWasChecked Then
                        CheckBoxArray.Remove(CheckBoxIndex)
                    End If
                End If
            End If
        Next

        ViewState("CheckBoxArray") = CheckBoxArray
    End Sub
    Private msgResult As String = String.Empty
    Private Sub CargarInformacion()

        If Session("dtResultado") IsNot Nothing Then
            dtResultado = Session("dtResultado")

            If dtResultado.Columns.Count = 6 Then
                dtResultado.Columns.Add(New DataColumn("vl", GetType(System.String)))
                dtResultado.Columns.Add(New DataColumn("interesNegativo", GetType(System.String)))
                dtResultado.Columns.Add(New DataColumn("valorizacionGanancia", GetType(System.String)))
                dtResultado.Columns.Add(New DataColumn("invercionNula", GetType(System.String)))
                dtResultado.Columns.Add(New DataColumn("valorizacionNula", GetType(System.String)))
            End If


            Me.gvresultado.DataSource = dtResultado
            Me.gvresultado.DataBind()


            If ddlSituacion.SelectedValue.Trim.Equals("0") Then
                msgResult = "No hay Portafolios por valorizar en esta fecha."
            ElseIf ddlSituacion.SelectedValue.Trim.Equals("1") Then
                msgResult = "No hay ningún Portafolio valorado para esta fecha."
            Else
                msgResult = "No se cargaron los vectores para esa fecha."
            End If


            If dtResultado.Rows.Count <= 0 Then
                AlertaJS(msgResult)
            End If

        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        'Dim codigoPortafolio As String = "8"
        'Dim fechaValorizacion As DateTime = New DateTime(2018, 8, 27) 'INGRESAR ESTA FECHA DESDE EL FORMULARIO
        'GenerarValorizacionAmortizada(codigoPortafolio, fechaValorizacion)
        'Exit Sub
        Try
            Dim fecha As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim estado As String = ddlSituacion.SelectedValue
            Dim tipoNegocio As String = ddlTipoNegocio.SelectedValue

            Dim cantidad As Integer = Portafolio.VerificarVectorPrecio(fecha).Rows(0)(0)
            If cantidad = 0 Then
                AlertaJS("Falta cargar el Vector Precio para esta fecha")
                Me.gvresultado.DataSource = dtResultado
                Me.gvresultado.DataBind()
            Else
                dtResultado = Portafolio.CargarValorizador(fecha, tipoNegocio, estado)
                ViewState("ItemSeleccion") = Nothing
                If Session("dtResultado") IsNot Nothing Then
                    Session("dtResultado") = Nothing
                End If
                Session("dtResultado") = dtResultado
                CargarInformacion()
                msgValoresPrecio()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
#Region "Valorizacion Amortizada"

    ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-15 | Generar la Valorización Amortizada 

    ''' <summary>
    ''' Genera la VALORIZCIÓN AMORTIZADA tanto para: A) TIR DE COMPRA, B) TIR ACTUAL SBS - Razonable, y C) Cantidad en Stock descontada x VENTAS
    ''' </summary>
    Function GenerarValorizacionAmortizada(ByVal codigoPortafolio As String, ByVal fechaValorizacion As DateTime) As List(Of KeyValuePair(Of String, String))
        Dim incidentes As New List(Of KeyValuePair(Of String, String)) 'Incidentes del proceso

        Dim aplicacionTasa As TipoAplicacionTasa = TipoAplicacionTasa.EFECTIVA 'EL USUARIO DEBERIA PODER ELEGIR LA FORMA DE CALCULO
        Dim val As New ValorizacionAmortizadaBM

        Dim ds As DataSet = val.ComprasReferidasAlStock(codigoPortafolio, CDec(fechaValorizacion.ToString("yyyyMMdd")))
        Dim dtOrdenes As DataTable = ds.Tables("Ordenes")
        Dim dtCuponeras As DataTable = ds.Tables("Cuponeras")
        Dim dtCuponesActuales As DataTable

        Dim dtValorizacionResul As New ValorizacionAmortizadaBE.ValorizacionAmortizadaDataTable
        Dim rowActual As ValorizacionAmortizadaBE.ValorizacionAmortizadaRow

        For Each row As DataRow In dtOrdenes.Rows
            Try
                If (row("TipoRenta").ToString() = TR_RENTA_VARIABLE.ToString()) Then

                    'Dim strnull As String
                    'strnull = String.Empty
                    'Dim cupones As DataRow() = dtCuponeras.Select("CodigoNemonico = '" & row("CodigoNemonico").ToString & "'")
                    'dtCuponesActuales = cupones.CopyToDataTable()
                    'Dim vacEmision As Decimal = 1, vacEvaluacion As Decimal = 1
                    'Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(row("BaseCuponAnual"))
                    'Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(row("BaseCuponMensual"))

                    'Dim EsCuponADescuento As Boolean = row("codigoTipoCupon").ToString.Equals("3") ' Es cupón A DESCUENTO solo si CodigoTipoCupon = 3 
                    'Dim esValorExtranjero As Boolean = row("CodigoMercado").ToString.Equals("2")


                    ''Pasamos a calcular la NEGOCIACION (VALORIZACION A TIR DE COMPRA)
                    'Dim YTM_COMPRA_NETA As Decimal = CDec(row("TIR_Neta")) ' UTILIZAMOS EL TIR DE COMPRA NETA
                    'If YTM_COMPRA_NETA = 0 Then YTM_COMPRA_NETA = CDec(row("TasaCupon"))

                    rowActual = dtValorizacionResul.NewValorizacionAmortizadaRow
                    rowActual.CodigoOrden = row("CodigoOrden").ToString
                    rowActual.CodigoNemonico = row("CodigoNemonico").ToString
                    rowActual.CantidadOperacion = CDec(row("CantidadOperacion"))
                    rowActual.CantidadEnStock = CDec(row("CantidadEnStock"))

                    If row("PorcentagePrecioLimpio") IsNot Nothing Then
                        rowActual.PRELIM_InteresCorrido = 0
                        'prelimNegociacion.PrecioLimpio = CDec(row("PorcentagePrecioLimpio")) / 100 'Por ser porcentaje
                        'prelimNegociacion.PrecioSucio = CDec(row("PorcentagePrecioSucio")) / 100 'Por ser porcentaje
                        ' prelimNegociacion.ValorActual = (prelimNegociacion.PrecioLimpio * prelimNegociacion.ValorNominal) + prelimNegociacion.InteresCorrido


                        rowActual.PRELIM_PrecioLimpio = CDec(row("PorcentagePrecioLimpio"))
                        rowActual.PRELIM_PrecioSucio = CDec(row("PorcentagePrecioLimpio"))
                        rowActual.PRELIM_ValorNominal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                        rowActual.PRELIM_CantidadOperacion = CDec(row("CantidadEnStock"))

                        'prelimNegociacion.CalcularDatosDelFlujoDeCuponesBasadoEnPrecioLimpio()
                        rowActual.PRELIM_ValorActual = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                        rowActual.PRELIM_ValorPrincipal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))

                        rowActual.PRELIM_TIR = 0
                        'If prelimNegociacion.CuponVigente IsNot Nothing Then
                        rowActual.PRELIM_FechaFinCuponActual = "0"
                        'prelimNegociacion.CalcularDatosDelFlujoDeCuponesBasadoEnTIR()
                        rowActual.PRELIM_MontoCuponActual = "0"
                        rowActual.PRELIM_SaldoNominalVigente = "0"
                        rowActual.PRELIM_PagoCuponVigente = "0"
                        'End If

                    End If

                    'TIRCOM

                    rowActual.TIRCOM_ValorActual = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.TIRCOM_InteresCorrido = 0
                    rowActual.TIRCOM_ValorPrincipal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.TIRCOM_PrecioLimpio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.TIRCOM_PrecioSucio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.TIRCOM_TIR = 0
                    rowActual.TIRCOM_ValorNominal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.TIRCOM_CantidadOperacion = CDec(row("CantidadEnStock"))

                    'If valCompra.CuponVigente IsNot Nothing Then
                    'rowActual.TIRCOM_FechaFinCuponActual = Nothing
                    'rowActual.TIRCOM_MontoCuponActual = Nothing
                    'rowActual.TIRCOM_SaldoNominalVigente = Nothing
                    'rowActual.TIRCOM_PagoCuponVigente = Nothing
                    'End If

                    rowActual.TIRRAZ_ValorActual = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.TIRRAZ_InteresCorrido = 0
                    rowActual.TIRRAZ_ValorPrincipal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.TIRRAZ_PrecioLimpio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.TIRRAZ_PrecioSucio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.TIRRAZ_TIR = 0
                    rowActual.TIRRAZ_ValorNominal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.TIRRAZ_CantidadOperacion = CDec(row("CantidadEnStock"))

                    'If valRazonable.CuponVigente IsNot Nothing Then
                    '    rowActual.TIRRAZ_FechaFinCuponActual = ""
                    '    rowActual.TIRRAZ_MontoCuponActual = ""
                    '    rowActual.TIRRAZ_SaldoNominalVigente = ""
                    '    rowActual.TIRRAZ_PagoCuponVigente = ""
                    'End If

                    rowActual.VTA_ValorActual = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.VTA_InteresCorrido = 0
                    rowActual.VTA_ValorPrincipal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.VTA_PrecioLimpio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.VTA_PrecioSucio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.VTA_TIR = 0
                    rowActual.VTA_ValorNominal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.VTA_CantidadOperacion = CDec(row("CantidadEnStock"))

                    'If valVenta.CuponVigente IsNot Nothing Then
                    '    rowActual.VTA_FechaFinCuponActual = ""
                    '    rowActual.VTA_MontoCuponActual = ""
                    '    rowActual.VTA_SaldoNominalVigente = ""
                    '    rowActual.VTA_PagoCuponVigente = ""
                    'End If

                    rowActual.NEG_ValorActual = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.NEG_InteresCorrido = 0
                    rowActual.NEG_ValorPrincipal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.NEG_PrecioLimpio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.NEG_PrecioSucio = CDec(row("PorcentagePrecioLimpio"))
                    rowActual.NEG_TIR = 0
                    rowActual.NEG_ValorNominal = (CDec(row("CantidadEnStock")) * CDec(row("PorcentagePrecioLimpio")))
                    rowActual.NEG_CantidadOperacion = CDec(row("CantidadEnStock"))

                    'If valNeg.CuponVigente IsNot Nothing Then
                    '    rowActual.NEG_FechaFinCuponActual = ""
                    '    rowActual.NEG_MontoCuponActual = ""
                    '    rowActual.NEG_SaldoNominalVigente = ""
                    '    rowActual.NEG_PagoCuponVigente = ""
                    'End If

                Else

                    Dim cupones As DataRow() = dtCuponeras.Select("CodigoNemonico = '" & row("CodigoNemonico").ToString & "'")
                    Dim probando As String = ""
                    'If row("CodigoNemonico").ToString = "SB12AGO20" Then
                    'If row("CodigoNemonico").ToString = "EDP5BC11A" Then
                    '    probando = "nuevo"
                    '  End If

                    'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
                    Dim dsValor As DataSet = New PrevOrdenInversionBM().SeleccionarCaracValor(row("CodigoNemonico").ToString, CDec(fechaValorizacion.ToString("yyyyMMdd")))
                    If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Bono")

                    Dim dtCaracValor As DataTable = dsValor.Tables(0)
                    Dim rowValor As DataRow = dtCaracValor.Rows(0)

                    Dim esCalculoTBill As Boolean = rowValor("CodigoTipoInstrumentoSBS").ToString.Equals("100")


                    dtCuponesActuales = cupones.CopyToDataTable()

                    Dim vacEmision As Decimal = 1, vacEvaluacion As Decimal = 1
                    'If rowValor("CodigoMoneda").ToString.Equals("VAC") Then
                    '    ' Obtencion de valores VAC para los Bonos que aplique
                    '    UIUtility.ObtenerValoresVAC(rowValor("FechaEmision"),
                    '                      UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text),
                    '                      vacEmision,
                    '                      vacEvaluacion,
                    '                      DatosRequest)
                    'End If

                    Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(row("BaseCuponAnual"))
                    Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(row("BaseCuponMensual"))

                    Dim EsCuponADescuento As Boolean = row("codigoTipoCupon").ToString.Equals("3") ' Es cupón A DESCUENTO solo si CodigoTipoCupon = 3 
                    Dim esValorExtranjero As Boolean = row("CodigoMercado").ToString.Equals("2")


                    'Pasamos a calcular la NEGOCIACION (VALORIZACION A TIR DE COMPRA)
                    Dim YTM_COMPRA_NETA As Decimal = CDec(row("TIR_Neta")) ' UTILIZAMOS EL TIR DE COMPRA NETA
                    If YTM_COMPRA_NETA = 0 Then YTM_COMPRA_NETA = CDec(row("TasaCupon"))

                    Dim valCompra As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(dtCuponesActuales,
                                                                        CDec(row("TasaCupon")),
                                                                        CInt(row("DiasPeriodicidad")),
                                                                        CDec(row("ValorUnitario")),
                                                                        CDec(row("CantidadEnStock")),
                                                                        fechaValorizacion,
                                                                        YTM_COMPRA_NETA,
                                                                        baseMensual,
                                                                        baseAnual,
                                                                        EsCuponADescuento,
                                                                        esValorExtranjero,
                                                                        aplicacionTasa,
                                                                        vacEmision,
                                                                        vacEvaluacion,
                                                                        esCalculoTBill)


                    'Pasamos a calcular la NEGOCIACION (VALORIZACION A VALOR RAZONABLE)
                    Dim YTM_SBS As Decimal = CDec(row("TIR_SBS")) ' ESTE DATO DEBERIA OBTENERLO DESDE LA CARGA DE VECTOR PRECIO SBS
                    If YTM_SBS = 0 Then YTM_SBS = CDec(row("TasaCupon"))

                    If YTM_SBS < 0 Then
                        incidentes.Add(New KeyValuePair(Of String, String)(row("CodigoNemonico"), "ALERTA: El YTM utilizado para el cálculo es negativo"))
                    End If

                    Dim valRazonable As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(dtCuponesActuales,
                                                                        CDec(row("TasaCupon")),
                                                                        CInt(row("DiasPeriodicidad")),
                                                                        CDec(row("ValorUnitario")),
                                                                        CDec(row("CantidadEnStock")),
                                                                        fechaValorizacion,
                                                                        YTM_SBS,
                                                                        baseMensual,
                                                                        baseAnual,
                                                                        EsCuponADescuento,
                                                                        esValorExtranjero,
                                                                        aplicacionTasa,
                                                                        vacEmision,
                                                                        vacEvaluacion,
                                                                        esCalculoTBill)

                    'Pasamos a calcular la NEGOCIACION (VALORIZACION A VALOR RAZONABLE)
                    Dim YTM_VTA As Decimal = YTM_SBS

                    Dim valVenta As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(dtCuponesActuales,
                                                                        CDec(row("TasaCupon")),
                                                                        CInt(row("DiasPeriodicidad")),
                                                                        CDec(row("ValorUnitario")),
                                                                        CDec(row("CantidadEnStock")),
                                                                        fechaValorizacion,
                                                                        YTM_VTA,
                                                                        baseMensual,
                                                                        baseAnual,
                                                                        EsCuponADescuento,
                                                                        esValorExtranjero,
                                                                        aplicacionTasa,
                                                                        vacEmision,
                                                                        vacEvaluacion,
                                                                        esCalculoTBill)

                    'Pasamos a calcular la NEGOCIACION (VALORIZACION A VALOR RAZONABLE)
                    Dim YTM_COMPRA_NEG As Decimal = CDec(row("TIR_COMPRA")) ' UTILIZAMOS EL TIR DE COMPRA NEGOCIACION
                    If YTM_COMPRA_NEG = 0 Then YTM_COMPRA_NEG = CDec(row("TasaCupon"))

                    Dim valNeg As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(dtCuponesActuales,
                                                                        CDec(row("TasaCupon")),
                                                                        CInt(row("DiasPeriodicidad")),
                                                                        CDec(row("ValorUnitario")),
                                                                        CDec(row("CantidadEnStock")),
                                                                        fechaValorizacion,
                                                                        YTM_COMPRA_NEG,
                                                                        baseMensual,
                                                                        baseAnual,
                                                                        EsCuponADescuento,
                                                                        esValorExtranjero,
                                                                        aplicacionTasa,
                                                                        vacEmision,
                                                                        vacEvaluacion,
                                                                        esCalculoTBill)

                    ' // -- ==============================================================

                    Dim prelimNegociacion As NegociacionRentaFija = valNeg.Clone()

                    rowActual = dtValorizacionResul.NewValorizacionAmortizadaRow
                    rowActual.CodigoOrden = row("CodigoOrden").ToString
                    rowActual.CodigoNemonico = row("CodigoNemonico").ToString
                    rowActual.CantidadOperacion = CDec(row("CantidadOperacion"))
                    rowActual.CantidadEnStock = CDec(row("CantidadEnStock"))

                    If row("PorcentagePrecioLimpio") IsNot Nothing Then
                        rowActual.PRELIM_InteresCorrido = prelimNegociacion.InteresCorrido
                        prelimNegociacion.PrecioLimpio = CDec(row("PorcentagePrecioLimpio")) / 100 'Por ser porcentaje
                        prelimNegociacion.PrecioSucio = CDec(row("PorcentagePrecioSucio")) / 100 'Por ser porcentaje
                        ' prelimNegociacion.ValorActual = (prelimNegociacion.PrecioLimpio * prelimNegociacion.ValorNominal) + prelimNegociacion.InteresCorrido


                        rowActual.PRELIM_PrecioLimpio = prelimNegociacion.PrecioLimpio * 100
                        rowActual.PRELIM_PrecioSucio = prelimNegociacion.PrecioSucio * 100
                        rowActual.PRELIM_ValorNominal = prelimNegociacion.ValorNominal
                        rowActual.PRELIM_CantidadOperacion = prelimNegociacion.CantidadUnidadesNegociadas

                        prelimNegociacion.CalcularDatosDelFlujoDeCuponesBasadoEnPrecioLimpio()
                        rowActual.PRELIM_ValorActual = prelimNegociacion.ValorActual
                        rowActual.PRELIM_ValorPrincipal = prelimNegociacion.ValorPrincipal

                        rowActual.PRELIM_TIR = prelimNegociacion.YTM * 100
                        If prelimNegociacion.CuponVigente IsNot Nothing Then
                            rowActual.PRELIM_FechaFinCuponActual = prelimNegociacion.CuponVigente.FechaFin.ToString("yyyyMMdd")
                            prelimNegociacion.CalcularDatosDelFlujoDeCuponesBasadoEnTIR()
                            rowActual.PRELIM_MontoCuponActual = prelimNegociacion.CuponVigente.FlujoDescuento
                            rowActual.PRELIM_SaldoNominalVigente = prelimNegociacion.CuponVigente.SaldoNominalInicial
                            rowActual.PRELIM_PagoCuponVigente = prelimNegociacion.CuponVigente.PagoCupon
                        End If

                    End If


                    rowActual.TIRCOM_ValorActual = valCompra.ValorActual
                    rowActual.TIRCOM_InteresCorrido = valCompra.InteresCorrido
                    rowActual.TIRCOM_ValorPrincipal = valCompra.ValorPrincipal
                    rowActual.TIRCOM_PrecioLimpio = valCompra.PrecioLimpio * 100 'Por ser porcentaje
                    rowActual.TIRCOM_PrecioSucio = valCompra.PrecioSucio * 100 'Por ser porcentaje
                    rowActual.TIRCOM_TIR = valCompra.YTM * 100 'Por ser porcentaje
                    rowActual.TIRCOM_ValorNominal = valCompra.ValorNominal
                    rowActual.TIRCOM_CantidadOperacion = valCompra.CantidadUnidadesNegociadas

                    If valCompra.CuponVigente IsNot Nothing Then
                        rowActual.TIRCOM_FechaFinCuponActual = valCompra.CuponVigente.FechaFin.ToString("yyyyMMdd")
                        rowActual.TIRCOM_MontoCuponActual = valCompra.CuponVigente.FlujoDescuento
                        rowActual.TIRCOM_SaldoNominalVigente = valCompra.CuponVigente.SaldoNominalInicial
                        rowActual.TIRCOM_PagoCuponVigente = valCompra.CuponVigente.PagoCupon
                    End If

                    rowActual.TIRRAZ_ValorActual = valRazonable.ValorActual
                    rowActual.TIRRAZ_InteresCorrido = valRazonable.InteresCorrido
                    rowActual.TIRRAZ_ValorPrincipal = valRazonable.ValorPrincipal
                    rowActual.TIRRAZ_PrecioLimpio = valRazonable.PrecioLimpio * 100 'Por ser porcentaje
                    rowActual.TIRRAZ_PrecioSucio = valRazonable.PrecioSucio * 100 'Por ser porcentaje
                    rowActual.TIRRAZ_TIR = valRazonable.YTM * 100 'Por ser porcentaje
                    rowActual.TIRRAZ_ValorNominal = valRazonable.ValorNominal
                    rowActual.TIRRAZ_CantidadOperacion = valRazonable.CantidadUnidadesNegociadas

                    If valRazonable.CuponVigente IsNot Nothing Then
                        rowActual.TIRRAZ_FechaFinCuponActual = valRazonable.CuponVigente.FechaFin.ToString("yyyyMMdd")
                        rowActual.TIRRAZ_MontoCuponActual = valRazonable.CuponVigente.FlujoDescuento
                        rowActual.TIRRAZ_SaldoNominalVigente = valRazonable.CuponVigente.SaldoNominalInicial
                        rowActual.TIRRAZ_PagoCuponVigente = valRazonable.CuponVigente.PagoCupon
                    End If

                    rowActual.VTA_ValorActual = valVenta.ValorActual
                    rowActual.VTA_InteresCorrido = valVenta.InteresCorrido
                    rowActual.VTA_ValorPrincipal = valVenta.ValorPrincipal
                    rowActual.VTA_PrecioLimpio = valVenta.PrecioLimpio * 100 'Por ser porcentaje
                    rowActual.VTA_PrecioSucio = valVenta.PrecioSucio * 100 'Por ser porcentaje
                    rowActual.VTA_TIR = valVenta.YTM * 100 'Por ser porcentaje
                    rowActual.VTA_ValorNominal = valVenta.ValorNominal
                    rowActual.VTA_CantidadOperacion = valVenta.CantidadUnidadesNegociadas

                    If valVenta.CuponVigente IsNot Nothing Then
                        rowActual.VTA_FechaFinCuponActual = valVenta.CuponVigente.FechaFin.ToString("yyyyMMdd")
                        rowActual.VTA_MontoCuponActual = valVenta.CuponVigente.FlujoDescuento
                        rowActual.VTA_SaldoNominalVigente = valVenta.CuponVigente.SaldoNominalInicial
                        rowActual.VTA_PagoCuponVigente = valVenta.CuponVigente.PagoCupon
                    End If

                    rowActual.NEG_ValorActual = valNeg.ValorActual
                    rowActual.NEG_InteresCorrido = valNeg.InteresCorrido
                    rowActual.NEG_ValorPrincipal = valNeg.ValorPrincipal
                    rowActual.NEG_PrecioLimpio = valNeg.PrecioLimpio * 100 'Por ser porcentaje
                    rowActual.NEG_PrecioSucio = valNeg.PrecioSucio * 100 'Por ser porcentaje
                    rowActual.NEG_TIR = valNeg.YTM * 100 'Por ser porcentaje
                    rowActual.NEG_ValorNominal = valNeg.ValorNominal
                    rowActual.NEG_CantidadOperacion = valNeg.CantidadUnidadesNegociadas

                    If valNeg.CuponVigente IsNot Nothing Then
                        rowActual.NEG_FechaFinCuponActual = valNeg.CuponVigente.FechaFin.ToString("yyyyMMdd")
                        rowActual.NEG_MontoCuponActual = valNeg.CuponVigente.FlujoDescuento
                        rowActual.NEG_SaldoNominalVigente = valNeg.CuponVigente.SaldoNominalInicial
                        rowActual.NEG_PagoCuponVigente = valNeg.CuponVigente.PagoCupon
                    End If

                End If
                dtValorizacionResul.AddValorizacionAmortizadaRow(rowActual)
            Catch ex As Exception
                incidentes.Add(New KeyValuePair(Of String, String)(row("CodigoNemonico"), "ERROR: " & ex.Message))
            End Try
        Next

        Dim oValorizacionBM As New ValorizacionAmortizadaBM
        oValorizacionBM.GuardarValorizacion(codigoPortafolio, fechaValorizacion.ToString("yyyyMMdd"), dtValorizacionResul, Session("UInfo_CodUsuario"))

        Return incidentes
    End Function
    ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-15 | Generar la Valorización Amortizada 


#End Region

    Private Sub msgValoresPrecio()
        Dim contador As Integer = 0
        For Each row As GridViewRow In Me.gvresultado.Rows
            hd = CType(row.FindControl("hdPip"), HiddenField)

            If hd.Value.Equals("0") Then
                contador += 1
            End If
        Next
        If contador > 0 Then
            AlertaJS("Algunos Valores no tienen precio para la fecha que se esta Valorizando")
        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvresultado.PageIndex = e.NewPageIndex
        CargarInformacion()
        gvresultado.DataBind()
        ItemSeleccionado()
    End Sub
    Private Sub ItemSeleccionado()
        If ViewState("CheckBoxArray") IsNot Nothing Then
            Dim CheckBoxArray As ArrayList = _
            DirectCast(ViewState("CheckBoxArray"), ArrayList)
            Dim checkAllIndex As String = "chkAll-" & gvresultado.PageIndex

            If CheckBoxArray.IndexOf(checkAllIndex) <> -1 Then
                Dim chkAll As CheckBox = _
                DirectCast(gvresultado.HeaderRow.Cells(0).FindControl("SelectAllCheckBox"), CheckBox)
                chkAll.Checked = True
            End If
            For i As Integer = 0 To gvresultado.Rows.Count - 1
                If gvresultado.Rows(i).RowType = DataControlRowType.DataRow Then
                    If CheckBoxArray.IndexOf(checkAllIndex) <> -1 Then
                        Dim chk As CheckBox = _
                        DirectCast(gvresultado.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                        chk.Checked = True
                        'gvresultado.Rows(i).Attributes.Add("style", "background-color:aqua")
                    Else
                        Dim CheckBoxIndex As Integer = gvresultado.PageSize * (gvresultado.PageIndex) + (i + 1)
                        If CheckBoxArray.IndexOf(CheckBoxIndex) <> -1 Then
                            Dim chk As CheckBox = _
                            DirectCast(gvresultado.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                            chk.Checked = True
                            'gvresultado.Rows(i).Attributes.Add("style", "background-color:aqua")
                        End If
                    End If
                End If
            Next
        End If
    End Sub
    Protected Sub gvresultado_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvresultado.RowCreated

        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGrid As GridView = sender
            Dim HeaderGridRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim HeaderCell As New TableCell
            HeaderCell.Text = ""
            HeaderCell.ColumnSpan = 2
            HeaderCell.BorderColor = Drawing.Color.Black
            HeaderCell.VerticalAlign = VerticalAlign.Top
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "Datos Input"
            HeaderCell.ColumnSpan = 2
            HeaderCell.BorderColor = Drawing.Color.Black
            HeaderCell.VerticalAlign = VerticalAlign.Top
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "Datos Salida"
            HeaderCell.ColumnSpan = 6
            HeaderCell.BorderColor = Drawing.Color.Black
            HeaderCell.VerticalAlign = VerticalAlign.Top
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)


            gvresultado.Controls(0).Controls.AddAt(0, HeaderGridRow)

        End If

    End Sub
    Private Function ObtenerPIP(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            If hdInput.Value = 1 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            ElseIf hdInput.Value = 2 Then
                strRuta = "~\App_Themes\img\icons\advertencia.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function
    Private Function ObtenerImagen(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            If hdInput.Value > 0 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function
    Private Function ObtenerPxQ(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            valor = Convert.ToDecimal(hdInput.Value)
            If valor = 0 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function
    Private Function ObtenerInteresNegativo(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            valor = Convert.ToDecimal(hdInput.Value)
            If valor = 0 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function
    Private Function ObtenerGanancia(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            valor = Convert.ToDecimal(hdInput.Value)
            If valor = 0 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function
    Private Function ObtenerInversionNula(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            valor = Convert.ToDecimal(hdInput.Value)
            If valor = 0 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function
    Private Function ObtenerValorizacionNula(ByVal hdInput As HiddenField) As String
        strRuta = String.Empty
        If hdInput.Value.Length > 0 Then
            valor = Convert.ToDecimal(hdInput.Value)
            If valor = 0 Then
                strRuta = "~\App_Themes\img\icons\check.png"
            Else
                strRuta = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If
        Return strRuta
    End Function

    Private flag As Integer

    Protected Sub gvresultado_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvresultado.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)

            hdCodigoPortafolio = CType(e.Row.FindControl("hdCodPortafolio"), HiddenField)

            hd = CType(e.Row.FindControl("hdPip"), HiddenField)
            'If hd.Value = 0 Or hd.Value = 2 Then
            '    chkSelect.Enabled = False
            'End If

            img = CType(e.Row.FindControl("imgPip"), Image)
            img.ImageUrl = ObtenerPIP(hd)

            If hd.Value.Trim().Equals("0") Or hd.Value.Trim().Equals("2") Then
                chkSelect.Enabled = False
                img.CssClass = "selector"
                img.Attributes.Add("onclick", IIf(hd.Value.Trim().Equals("0"), "javascript:valorizar(" & hdCodigoPortafolio.Value & ");", _
                                                                               "javascript:MostrarAlertaFechaUltimaValorizacion(" & hdCodigoPortafolio.Value & ");"))
            End If

            hd = CType(e.Row.FindControl("hdCaja"), HiddenField)
            img = CType(e.Row.FindControl("imgCaja"), Image)
            img.ImageUrl = ObtenerImagen(hd)

            If hd.Value = 0 Then
                chkSelect.Enabled = False
                img.CssClass = "selector"
                img.Attributes.Add("onclick", "javascript:MostrarAlertaFechaCajas(" & hdCodigoPortafolio.Value & ");")
            End If

            hd = CType(e.Row.FindControl("hdVL"), HiddenField)
            img = CType(e.Row.FindControl("imgVL"), Image)
            img.ImageUrl = ObtenerPxQ(hd)
            img.AlternateText = hd.Value

            hd = CType(e.Row.FindControl("hdInteres"), HiddenField)
            img = CType(e.Row.FindControl("imgInteres"), Image)
            img.ImageUrl = ObtenerInteresNegativo(hd)

            hd = CType(e.Row.FindControl("hdVariacion"), HiddenField)
            img = CType(e.Row.FindControl("imgVariacion"), Image)
            img.ImageUrl = ObtenerGanancia(hd)

            hd = CType(e.Row.FindControl("hdInversion"), HiddenField)
            img = CType(e.Row.FindControl("imgInversion"), Image)
            img.ImageUrl = ObtenerInversionNula(hd)

            hd = CType(e.Row.FindControl("hdValorizacion"), HiddenField)
            img = CType(e.Row.FindControl("imgValorizacion"), Image)
            img.ImageUrl = ObtenerValorizacionNula(hd)

            hd = CType(e.Row.FindControl("hdEstado"), HiddenField)

            If hd.Value.Trim().Equals("1") Then
                chkSelect.Enabled = False
            End If

        End If

        If e.Row.RowType = DataControlRowType.Header Then
            Dim portafolio As String = Me.ddlSituacion.SelectedValue.Trim

            If portafolio.Equals("1") Then
                Dim check As CheckBox = CType(e.Row.FindControl("SelectAllCheckBox"), CheckBox)
                check.Enabled = False
            End If

        End If



    End Sub
    Protected Sub gvresultado_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvresultado.RowDeleted

    End Sub
    Protected Sub btnVer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVer.Click

        If ViewState("ItemSeleccion") IsNot Nothing Then
            lstVisor = ObtenerVisorItem()
        End If


        If lstVisor.Count > 0 Then
            If Cont_Anomalias(lstVisor) > 0 Then
                url = "frmVisualizacionValorizacion.aspx?lstPortafolio=" & ListadoPortafolioVer(lstVisor) & "&fechaOperacion=" & UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
                EjecutarJS("showWindow('" & url & "', '800', '600');")
            ElseIf Cont_Anomalias(lstVisor) = -1 Then
                AlertaJS("Falta procesar el Portafolio seleccionado")
            Else
                AlertaJS("No hay anomalías")
            End If
        Else
            AlertaJS("Seleccione un Portafolio ya valorizado.")
        End If

    End Sub

    Private dt_VL, dt_Interes, dt_Ganancia, dt_Inversion, dt_Valorizacion As New DataTable
    Private filas() As DataRow
    Private ContadorAnomalias As Integer

    Private Function Cont_Anomalias(ByVal lstVisor As List(Of PortafolioVisor)) As Integer

        Dim lstPortafolio As String = ListadoPortafolioVer(lstVisor)

        dsComplemento = Session("dsComplemento")

        Dim filas() As DataRow = dsComplemento.Tables(0).Select("Portafolio IN (" & lstPortafolio & ")")
        Dim dtContador As DataTable
        If filas.Count > 0 Then
            dtContador = filas.CopyToDataTable

            For index = 0 To dtContador.Rows.Count - 1
                ContadorAnomalias += Convert.ToInt32(dtContador(index)(1))
                ContadorAnomalias += Convert.ToInt32(dtContador(index)(2))
                ContadorAnomalias += Convert.ToInt32(dtContador(index)(3))
                ContadorAnomalias += Convert.ToInt32(dtContador(index)(4))
                ContadorAnomalias += Convert.ToInt32(dtContador(index)(5))
            Next
        Else
            Return -1
        End If
        Return ContadorAnomalias
    End Function
    Function Obtener_Vl(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty
        ListaId = New List(Of String)

        For index = 0 To lstFondo.Count - 1
            If lstFondo(index).VL = 1 Then
                ListaId.Add(lstFondo(index).CodPortafolio)
            End If
        Next

        For index = 0 To ListaId.Count - 1

            If index <> 0 Then
                If index < ListaId.Count Then
                    idFondo += ","
                End If
            End If

            idFondo += ListaId(index)

        Next

        Return idFondo
    End Function
    Function Obtener_Interes(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty
        ListaId = New List(Of String)

        For index = 0 To lstFondo.Count - 1
            If lstFondo(index).Interes = 1 Then
                ListaId.Add(lstFondo(index).CodPortafolio)
            End If
        Next

        For index = 0 To ListaId.Count - 1
            If index <> 0 Then
                If index < ListaId.Count Then
                    idFondo += ","
                End If
            End If

            idFondo += ListaId(index)
        Next
        Return idFondo
    End Function
    Function Obtener_Ganancia(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty

        ListaId = New List(Of String)

        For index = 0 To lstFondo.Count - 1
            If lstFondo(index).Ganancia = 1 Then
                ListaId.Add(lstFondo(index).CodPortafolio)
            End If
        Next

        For index = 0 To ListaId.Count - 1
            If index <> 0 Then
                If index < ListaId.Count Then
                    idFondo += ","
                End If
            End If

            idFondo += ListaId(index)
        Next
        Return idFondo
    End Function
    Function Obtener_Inversion(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty

        ListaId = New List(Of String)

        For index = 0 To lstFondo.Count - 1
            If lstFondo(index).Inversion = 1 Then
                ListaId.Add(lstFondo(index).CodPortafolio)
            End If
        Next

        For index = 0 To ListaId.Count - 1
            If index <> 0 Then
                If index < ListaId.Count Then
                    idFondo += ","
                End If
            End If

            idFondo += ListaId(index)
        Next
        Return idFondo
    End Function
    Function Obtener_Valorizacion(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty
        ListaId = New List(Of String)

        For index = 0 To lstFondo.Count - 1
            If lstFondo(index).Valorizacion = 1 Then
                ListaId.Add(lstFondo(index).CodPortafolio)
            End If
        Next

        For index = 0 To ListaId.Count - 1
            If index <> 0 Then
                If index < ListaId.Count Then
                    idFondo += ","
                End If
            End If

            idFondo += ListaId(index)
        Next

        Return idFondo
    End Function
    Private Function ObtenerVisorItem() As List(Of PortafolioVisor)
        lstVisor = New List(Of PortafolioVisor)
        Dim visor As PortafolioVisor
        For Each row As GridViewRow In Me.gvresultado.Rows

            chkSelect = CType(row.FindControl("chkSelect"), CheckBox)

            If chkSelect.Checked Then

                visor = New PortafolioVisor
                hd = CType(row.FindControl("hdCodPortafolio"), HiddenField)
                visor.CodPortafolio = hd.Value
                visor.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)

                img = CType(row.FindControl("imgVL"), Image)
                If img.ImageUrl.Equals("~\App_Themes\img\icons\glyphicons_bin.png") Then
                    visor.VL = 1
                End If

                img = CType(row.FindControl("imgInteres"), Image)
                If img.ImageUrl.Equals("~\App_Themes\img\icons\glyphicons_bin.png") Then
                    visor.Interes = 1
                End If

                img = CType(row.FindControl("imgVariacion"), Image)
                If img.ImageUrl.Equals("~\App_Themes\img\icons\glyphicons_bin.png") Then
                    visor.Ganancia = 1
                End If

                img = CType(row.FindControl("imgInversion"), Image)
                If img.ImageUrl.Equals("~\App_Themes\img\icons\glyphicons_bin.png") Then
                    visor.Inversion = 1
                End If

                img = CType(row.FindControl("imgValorizacion"), Image)
                If img.ImageUrl.Equals("~\App_Themes\img\icons\glyphicons_bin.png") Then
                    visor.Valorizacion = 1
                End If

                lstVisor.Add(visor)
            End If


        Next
        Return lstVisor
    End Function
    Private Function ListadoPortafolio(ByVal lstFondo As List(Of Portafolio)) As String
        idFondo = String.Empty

        For index = 0 To lstFondo.Count - 1
            If lstFondo(index).Estado Then
                idFondo += lstFondo(index).CodPortafolio


                If index < lstFondo.Count - 1 Then
                    idFondo += ","
                End If
            End If
        Next
        Return idFondo
    End Function
    Private Function ListadoPortafolioVer(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty

        For index = 0 To lstFondo.Count - 1

            idFondo += lstFondo(index).CodPortafolio


            If index < lstFondo.Count - 1 Then
                idFondo += ","
            End If
        Next
        Return idFondo
    End Function
    Private Function ObtenerFondo(ByVal lstFondo As List(Of Portafolio)) As String
        idFondo = String.Empty

        For index = 0 To lstFondo.Count - 1

            If index <> 0 Then
                If index < lstFondo.Count - 1 Then
                    idFondo += ","
                End If
            End If

            idFondo += lstFondo(index).CodPortafolio

        Next
        Return idFondo
    End Function
    Private Function ObtenerFecha(ByVal lstFondo As List(Of Portafolio)) As String
        idFondo = String.Empty

        For index = 0 To lstFondo.Count - 1
            If index <> 0 Then
                If index < lstFondo.Count - 1 Then
                    idFondo += ","
                End If
            End If

            idFondo += lstFondo(index).FechaOperacion.ToString

        Next
        Return idFondo
    End Function
    Private Function ObtenerPortafolio(ByVal lstFondo As List(Of PortafolioVisor)) As String
        idFondo = String.Empty

        For index = 0 To lstFondo.Count - 1

            If index <> 0 Then
                If index < lstFondo.Count Then
                    idFondo += ","
                End If
            End If

            idFondo += lstFondo(index).CodPortafolio

        Next
        Return idFondo
    End Function
End Class