Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Partial Class Modulos_Inversiones_AsignacionFondos_frmIngresoCriteriosAsignacion
    Inherits BasePage
    Dim codigoMultifondo As String = New ParametrosGeneralesBM().SeleccionarPorFiltro(FECHA_FONDO, FECHA_NEGOCIO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
#Region " /* Declaración Variables */ "

    Dim m_Fondo As String = "HO-FONDO1,HO-FONDO2,HO-FONDO3" '#ERROR#
    Dim m_MsgOK As String = "Se guardó la Asignación exitosamente."
    Dim m_MsgKO As String = "Ha ocurrido un error guardando la Asignación."
    Dim m_MsgIO As String = "Se ha asignado y generado las ordenes de inversion exitosamente."

    Dim asignacionOK As Boolean = True

#End Region

#Region " /* Propiedades */ "

    Private Property CodigoPreOrden() As String
        Get
            If Not (ViewState("CodigoPreOrden") Is Nothing) Then Return ViewState("CodigoPreOrden").ToString()
        End Get
        Set(ByVal Value As String)
            ViewState("CodigoPreOrden") = Value
        End Set
    End Property

    Private Property PrecioUnitario() As Decimal
        Get
            If Not (ViewState("PrecioUnitario") Is Nothing) Then Return Convert.ToDecimal(ViewState("PrecioUnitario"))
        End Get
        Set(ByVal Value As Decimal)
            ViewState("PrecioUnitario") = Value
        End Set
    End Property

    Private Property DatosPreOrden() As DataSet
        Get
            If Not (ViewState("DatosPreOrden") Is Nothing) Then Return CType(ViewState("DatosPreOrden"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            ViewState("DatosPreOrden") = Value
        End Set
    End Property

    Private Property PreOrdenes() As DataTable
        Get
            If Not (ViewState("PreOrdenes") Is Nothing) Then Return CType(ViewState("PreOrdenes"), DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("PreOrdenes") = Value
        End Set
    End Property

    Private Property Asignaciones() As DataTable
        Get
            If Not (ViewState("Asignaciones") Is Nothing) Then Return CType(ViewState("Asignaciones"), DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("Asignaciones") = Value
        End Set
    End Property

    Private Property [NominalTotal]() As Decimal
        Get
            If Not (ViewState("[NominalTotal]") Is Nothing) Then Return Convert.ToDecimal(ViewState("[NominalTotal]"))
        End Get
        Set(ByVal Value As Decimal)
            ViewState("[NominalTotal]") = Value
        End Set
    End Property

    Private Property [UnidadesTotal]() As Decimal
        Get
            If Not (ViewState("[UnidadesTotal]") Is Nothing) Then Return Convert.ToInt32(ViewState("[UnidadesTotal]"))
        End Get
        Set(ByVal Value As Decimal)
            ViewState("[UnidadesTotal]") = Value
        End Set
    End Property

    Private Property [CodigoPreOrdenAsignacion]() As String
        Get
            If Not (ViewState("[CodigoPreOrdenAsignacion]") Is Nothing) Then Return ViewState("[CodigoPreOrdenAsignacion]").ToString()
        End Get
        Set(ByVal Value As String)
            ViewState("[CodigoPreOrdenAsignacion]") = Value
        End Set
    End Property

#End Region

#Region " /* Métodos y Funcio6nes Personalizados */ "

    Private Function PageInit() As Boolean
        Try
            LoadGeneral()
            LoadDatosPreOrden()
            LoadPreOrdenes(CodigoPreOrden)
            HabilitarControles(False)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Function

    Private Function LoadGeneral()
        CodigoPreOrden = Request.QueryString("vCodigoOrden")
        If Request.QueryString("vCategoria") = "FD" Or Request.QueryString("vCategoria") = "CV" Then
            HelpCombo.LlenarComboBox(ddlTipoAsignacion, New ParametrosGeneralesBM().ListarTipoAsignacionPorImporte(DatosRequest), "Valor", "Nombre", True)
        Else
            HelpCombo.LlenarComboBox(ddlTipoAsignacion, New ParametrosGeneralesBM().ListarTipoAsignacion(DatosRequest), "Valor", "Nombre", True)
        End If
        'FIN CMB OT 61566 20101209
        ddlTipoAsignacion.SelectedIndex = 2
        [NominalTotal] = 0
        [UnidadesTotal] = 0
        ViewState("Carga-Preordenes") = False

    End Function

    Private Function LoadDatosPreOrden()

        Dim unidadesCal As Decimal = 0
        Dim unidadesOri As Decimal = 0
        Dim nominal As Decimal = 0
        Dim preOrdenTable As DataTable

        preOrdenTable = DirectCast(New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(CodigoPreOrden, codigoMultifondo, DatosRequest, PORTAFOLIO_MULTIFONDOS).OrdenPreOrdenInversion, DataTable)
        DatosPreOrden = preOrdenTable.DataSet
        Session("EmisorInt") = ""   'HDG 20120113

        With DatosPreOrden.Tables(0)

            lbCodigoOrden.Text = .Rows(0)("CodigoOrden").ToString()
            lbCodigoISIN.Text = .Rows(0)("CodigoISIN").ToString()
            lbMnemonico.Text = .Rows(0)("CodigoMnemonico").ToString()
            'INI CMB OT 61566 20101209
            If lbMnemonico.Text = "FORWARD" Then
                nominal = Convert.ToDecimal(.Rows(0)("MontoCancelar").ToString())
            Else
                nominal = Convert.ToDecimal(.Rows(0)("MontoNominalOperacion").ToString())
            End If
            Session("EmisorInt") = .Rows(0)("CodigoTercero").ToString() 'HDG 20120113
            'FIN CMB OT 61566 20101209
            unidadesOri = Convert.ToDecimal(.Rows(0)("CantidadOperacion").ToString())
            'nominal = Convert.ToDecimal(.Rows(0)("MontoNominalOperacion").ToString()) 'CMB OT 61566 20101209
            PrecioUnitario = New ValoresBM().BuscarPrecioUnitario(.Rows(0)("CodigoMnemonico").ToString(), DatosRequest)
            unidadesCal = IIf(unidadesOri <> 0, unidadesOri, nominal / PrecioUnitario)

            lbMontoNominal.Text = nominal.ToString()
            lbUnidades.Text = unidadesCal.ToString()

            lbNominalTotal.Text = nominal.ToString()
            lbUnidadesTotal.Text = unidadesCal.ToString()

            [UnidadesTotal] = unidadesCal
            [NominalTotal] = nominal
        End With
    End Function

    Private Function LoadPreOrdenes(ByVal codPreOrden As String)


        Dim categoria, fechaOperacion, nemonico As String
        Dim flag As Boolean = False
        Dim n As Int32 = 0
        Dim i As Int32 = 0

        With DatosPreOrden.Tables(0)

            categoria = .Rows(0)("CategoriaInstrumento").ToString()
            fechaOperacion = .Rows(0)("FechaOperacion").ToString()
            nemonico = .Rows(0)("CodigoMnemonico").ToString()

        End With

        PreOrdenes = New OrdenPreOrdenInversionBM().ListarPreOrdenesParaAsignacion(fechaOperacion, categoria, nemonico, DatosRequest).Tables(0)

        dgPreOrdenes.DataSource = PreOrdenes
        dgPreOrdenes.DataBind()

        n = PreOrdenes.Rows.Count

        For i = 0 To n - 1

            With dgPreOrdenes.Rows(i)
                If codPreOrden = .Cells(1).Text Then
                    CType(.FindControl("chkOrden"), CheckBox).Checked = True
                    flag = False
                    hdCodigoOperacion.Value = .Cells(11).Text 'RGF 20090318
                End If
            End With

        Next

        lbContador.Text = "Pre-Ordenes de Inversión Vigentes: " + IIf(PreOrdenes.Rows.Count < 10, "0" + PreOrdenes.Rows.Count.ToString(), PreOrdenes.Rows.Count.ToString())

        ViewState("Carga-Preordenes") = flag

        AsignarPreOrdenesSeleccionadas()
        LoadAsignacionPreOrdenes(CodigoPreOrden)

        LoadAsignacionPreOrdenes(UnidadesTotal)



    End Function

    Private Function LoadAsignacionPreOrdenes(ByVal codPreOrden As String) As Boolean

        Dim n As Int32 = 0
        Dim datos As DataTable

        datos = New OrdenInversionWorkFlowBM().BuscarAsignacion(codPreOrden, DatosRequest)

        n = IIf(Not datos Is Nothing, datos.Rows.Count, 0)

        If n > 0 Then

            dgLista.DataSource = datos
            dgLista.DataBind()

            ddlTipoAsignacion.SelectedValue = datos.Rows(0)("TipoAsignacion").ToString()

            Dim i As Int32 = 0
            Dim j As Int32 = 0

            j = dgLista.Rows.Count

            For i = 0 To j - 1

                Dim tbUnid As TextBox = CType(dgLista.Rows(i).FindControl("tbUnidades"), TextBox)
                Dim tbAsign As TextBox = CType(dgLista.Rows(i).FindControl("tbAsignacion"), TextBox)

                tbAsign.Text = dgLista.Rows(i).Cells(1).Text
                tbUnid.Text = dgLista.Rows(i).Cells(3).Text

                If ddlTipoAsignacion.SelectedValue.ToString() = "U" Or ddlTipoAsignacion.SelectedValue.ToString() = "I" Then 'CMB OT 61566 20101209 --Or ddlTipoAsignacion.SelectedValue.ToString() = "I"
                    tbAsign.Enabled = False
                    tbUnid.Enabled = True
                Else
                    tbAsign.Enabled = True
                    tbUnid.Enabled = False
                End If

            Next



        End If

    End Function

    Private Function LoadAsignacionPreOrdenes(ByVal unidadesTotal As Decimal) As Boolean
        Dim tbUnid As TextBox
        Dim tbAsign As TextBox
        Dim i As Int32 = 0
        Dim n As Int32 = 0

        'RGF 20081229 Los DPZ no tienen CodigoISIN
        'If lbCodigoISIN.Text.Trim <> "" And lbMnemonico.Text.Trim <> "" And lbUnidades.Text.Trim <> "" Then
        If (lbCodigoISIN.Text.Trim <> "" Or lbMnemonico.Text.StartsWith("DPZ") Or lbMnemonico.Text = "DIVISA" Or lbMnemonico.Text = "FORWARD") And lbMnemonico.Text.Trim <> "" And lbUnidades.Text.Trim <> "" Then 'CMB OT 61566 20101209 --Or lbMnemonico.Text = "FORWARD"

            dgLista.DataSource = ProcesarCalculoAsignacion(unidadesTotal)
            dgLista.DataBind()

            n = dgLista.Rows.Count

            For i = 0 To n - 1
                tbUnid = CType(dgLista.Rows(i).FindControl("tbUnidades"), TextBox)
                tbAsign = CType(dgLista.Rows(i).FindControl("tbAsignacion"), TextBox)
                'tbAsign.Text = "33.33"
                'tbUnid.Text = dgLista.Rows(i).Cells(3).Text
                If ddlTipoAsignacion.SelectedValue.ToString() = "U" Or ddlTipoAsignacion.SelectedValue.ToString = "I" Then 'CMB OT 61566 20101209 --Or ddlTipoAsignacion.SelectedValue.ToString = "I"
                    tbAsign.Enabled = False
                    tbUnid.Enabled = True
                Else
                    tbAsign.Enabled = True
                    tbUnid.Enabled = False
                End If
            Next
        End If

    End Function

    Private Function ReprocesarCalculoAsignacionDetalle(ByVal unidadesTotal As Decimal) As Boolean
        Dim tbUnid As TextBox
        Dim tbAsign As TextBox
        Dim lblPrecio As Label
        Dim chkPreOrden As CheckBox
        Dim lblImporte As Label
        Dim lblCantidad As Label

        Dim i As Int32 = 0
        Dim n As Int32 = 0
        Dim cantidad As Decimal = 0.0
        Dim sum As Decimal = 0.0
        Dim cantidadPreorden As Decimal
        Dim dtResumnesAsignacion As DataTable
        Dim drDetalle As DataRow
        Dim dgItem As DataGridItem
        Dim dgItemPreOrden As DataGridItem
        Dim subTotal As Decimal
        If lbCodigoISIN.Text.Trim = "" Or lbMnemonico.Text.Trim = "" Or lbUnidades.Text.Trim = "" Then
            ReprocesarCalculoAsignacionDetalle = False
            Exit Function
        End If


        Dim campos As String() = {"CodigoAsignacion", "CodigoAsignacionPreOrden", "FechaAsignacion", "CodigoPortafolioSBS", "UnidadesPropuesto", "UnidadesReal", "PorcentajePropuesto", "Precio", "MontoOperacion", "TipoAsignacion", "Estado", "Situacion"}
        Dim tipos As String() = {"String", "String", "String", "String", "String", "String", "String", "String", "String", "String", "String", "String"}



        dtResumnesAsignacion = UIUtility.GetStructureTablebase(campos)

        For Each dgItem In dgLista.Rows
            tbUnid = CType(dgItem.FindControl("tbUnidades"), TextBox)
            tbAsign = CType(dgItem.FindControl("tbAsignacion"), TextBox)

            If tbAsign.Text = "" Then
                tbAsign.Text = "0.0"
            End If
            If tbUnid.Text = "" Then
                tbUnid.Text = "0.0"
            End If
            cantidad = Convert.ToDecimal(tbUnid.Text)
            For Each dgItemPreOrden In dgPreOrdenes.Rows
                chkPreOrden = CType(dgItemPreOrden.FindControl("chkOrden"), CheckBox)
                lblCantidad = CType(dgItemPreOrden.FindControl("lblCantidad"), Label)
                lblImporte = CType(dgItemPreOrden.FindControl("lblImporte"), Label)
                If chkPreOrden.Checked = True Then
                    cantidadPreorden = Convert.ToDecimal(lblCantidad.Text)

                    drDetalle = dtResumnesAsignacion.NewRow()
                    drDetalle("CodigoPortafolioSBS") = dgItem.Cells(0).Text
                    drDetalle("CodigoAsignacionPreOrden") = dgItemPreOrden.Cells(1).Text

                    If (unidadesTotal = 0) Then
                        drDetalle("UnidadesPropuesto") = "0.0"
                    Else
                        subTotal = (cantidad / unidadesTotal) * cantidadPreorden
                        drDetalle("UnidadesPropuesto") = subTotal.ToString("N2")
                    End If
                    If (cantidad <> 0) Then
                        subTotal = (((cantidad / unidadesTotal) * cantidadPreorden) / cantidad) * 100
                        drDetalle("PorcentajePropuesto") = subTotal.ToString("N2")
                    Else
                        drDetalle("PorcentajePropuesto") = "0.0"
                    End If


                    drDetalle("Precio") = lblImporte.Text
                    drDetalle("MontoOperacion") = Convert.ToDecimal(drDetalle("Precio")) * Convert.ToDecimal(drDetalle("UnidadesPropuesto"))
                    dtResumnesAsignacion.Rows.Add(drDetalle)
                End If
            Next
        Next
        dtResumnesAsignacion.AcceptChanges()
        Dim dr As DataRow
        i = 0
        dgvDetalle.DataSource = dtResumnesAsignacion
        dgvDetalle.DataBind()
    End Function
    Private Function ReprocesarCalculoAsignacion(ByVal unidadesTotal As Decimal) As Boolean
        Dim tbUnid As TextBox
        Dim tbAsign As TextBox
        Dim i As Int32 = 0
        Dim n As Int32 = 0
        Dim dlUnidades As Decimal = 0.0
        Dim sum As Decimal = 0.0
        Dim dtResumnesAsignacion As DataTable
        Dim dgItem As DataGridItem

        If lbMnemonico.Text.Trim = "" Or lbUnidades.Text.Trim = "" Then
            ReprocesarCalculoAsignacion = False
            Exit Function
        End If

        If (Session("ResumenAsignacion") Is Nothing) Then
            dtResumnesAsignacion = ProcesarCalculoAsignacion(unidadesTotal)
        Else
            dtResumnesAsignacion = CType(Session("ResumenAsignacion"), DataTable)
        End If

        For Each dgItem In dgLista.Rows
            tbUnid = CType(dgItem.FindControl("tbUnidades"), TextBox)
            tbAsign = CType(dgItem.FindControl("tbAsignacion"), TextBox)
            If tbAsign.Text = "" Then
                tbAsign.Text = "0.0"
            End If
            dtResumnesAsignacion.Rows(dgItem.ItemIndex)("PorcentajePropuesto") = tbAsign.Text
            If tbUnid.Text = "" Then
                tbUnid.Text = "0.0"
            End If
            dtResumnesAsignacion.Rows(dgItem.ItemIndex)("UnidadesPropuesto") = tbUnid.Text
        Next
        dtResumnesAsignacion.AcceptChanges()
        Dim dr As DataRow
        i = 0
        For Each dr In dtResumnesAsignacion.Rows
            If ddlTipoAsignacion.SelectedValue.ToString() = "U" Or ddlTipoAsignacion.SelectedValue.ToString() = "I" Then 'CMB OT 61566 20101209 --Or ddlTipoAsignacion.SelectedValue.ToString() = "I"
                If i = 2 Then
                    dlUnidades = 100 - sum
                Else
                    dlUnidades = (Convert.ToDecimal(dr("UnidadesPropuesto")) / unidadesTotal) * 100
                    sum = sum + dlUnidades
                End If
                dr("PorcentajePropuesto") = Format(dlUnidades, "######.00")
            Else
                If i = 2 Then
                    dlUnidades = Format(unidadesTotal - sum, "######.00")
                Else
                    dlUnidades = (unidadesTotal * Convert.ToDecimal(dr("PorcentajePropuesto"))) / 100
                    sum = sum + dlUnidades
                End If
                dr("UnidadesPropuesto") = Format(dlUnidades, "######.00")
            End If
            i = i + 1
        Next
        dgLista.DataSource = dtResumnesAsignacion
        dgLista.DataBind()
        n = dgLista.Rows.Count

        For i = 0 To n - 1
            tbUnid = CType(dgLista.Rows(i).FindControl("tbUnidades"), TextBox)
            tbAsign = CType(dgLista.Rows(i).FindControl("tbAsignacion"), TextBox)
            If ddlTipoAsignacion.SelectedValue.ToString() = "U" Or ddlTipoAsignacion.SelectedValue.ToString() = "I" Then
                tbAsign.Enabled = False
                tbUnid.Enabled = True
            Else
                tbAsign.Enabled = True
                tbUnid.Enabled = False
            End If
        Next
    End Function

    Private Function AsignarPreOrdenesSeleccionadas() As Boolean

        Dim unidades As Int32 = 0
        Dim nominal As Decimal = 0

        Dim Impuestos As Decimal = 0.0
        Dim Precio As Decimal = 0.0

        Dim IsRentaFija As Boolean = False

        Dim i As Int32 = 0
        Dim n As Int32 = 0

        '------
        Dim contPOI As Integer = 0
        Dim intermediario As String = ""
        Dim strMensaje As String = ""
        '-------
        asignacionOK = True

        unidades = Convert.ToDecimal(DatosPreOrden.Tables(0).Rows(0).Item(DatosPreOrden.Tables(0).Columns.IndexOf("CantidadOperacion")))
        'INI CMB OT 61566 20101209
        If lbMnemonico.Text = "FORWARD" Then
            nominal = Convert.ToDecimal(DatosPreOrden.Tables(0).Rows(0)("MontoCancelar").ToString())
        Else
            nominal = Convert.ToDecimal(DatosPreOrden.Tables(0).Rows(0)("MontoNominalOperacion").ToString())
        End If
        'FIN CMB OT 61566 20101209
        If nominal <> 0 And nominal Then IsRentaFija = True Else IsRentaFija = False

        nominal = 0
        unidades = 0

        [NominalTotal] = 0
        [UnidadesTotal] = 0

        n = dgPreOrdenes.Rows.Count

        'verificar intermediarios iguales
        For i = 0 To n - 1
            With dgPreOrdenes.Rows(i)

                Dim chkPreOrden As CheckBox = CType(.FindControl("chkOrden"), CheckBox)

                If chkPreOrden.Checked = True Then
                    contPOI = contPOI + 1
                    If (contPOI = 1) Then
                        intermediario = .Cells(5).Text.ToString()
                        strMensaje = strMensaje + "\t -" + .Cells(1).Text.ToString() + " - " + .Cells(5).Text.ToString() + "\n"
                    Else
                        If (intermediario <> .Cells(5).Text.ToString()) Then
                            asignacionOK = False
                        End If
                        strMensaje = strMensaje + "\t -" + .Cells(1).Text.ToString() + " - " + .Cells(5).Text.ToString() + "\n"
                    End If
                End If
            End With
        Next

        If Not (asignacionOK) Then
            AlertaJS("Debe seleccionar Pre-Ordenes con el mismo Intermediario.\n" + strMensaje)
            Exit Function
        End If

        Dim lblImporte As Label
        Dim lblCantidad As Label
        Dim c As Integer
        Dim precioDocumento As Decimal
        Dim cantidadDocumento As Decimal
        Dim importeDocumento As Decimal
        Dim totalCantidad As Decimal
        precioDocumento = 0
        cantidadDocumento = 0
        importeDocumento = 0
        totalCantidad = 0
        For i = 0 To n - 1
            With dgPreOrdenes.Rows(i)
                Dim chkPreOrden As CheckBox = CType(.FindControl("chkOrden"), CheckBox)
                If chkPreOrden.Checked = True Then
                    If IsRentaFija Then
                        nominal = Convert.ToDecimal(.Cells(16).Text.ToString())
                        [NominalTotal] = [NominalTotal] + nominal
                        If [PrecioUnitario] <> 0 Then
                            [UnidadesTotal] = [UnidadesTotal] + (nominal / PrecioUnitario)
                        End If
                    Else
                        'INI CMB OT 61566 20101123
                        If .Cells(17).Text <> "&nbsp;" Then
                            unidades = Convert.ToInt32(Math.Round(Convert.ToDecimal(.Cells(17).Text.ToString())))
                        Else
                            unidades = 0
                        End If
                        'FIN CMB OT 61566 20101123
                        [UnidadesTotal] = [UnidadesTotal] + unidades
                    End If


                    If Not (.FindControl("lblImporte") Is Nothing) Then
                        lblImporte = CType(.FindControl("lblImporte"), Label)
                        importeDocumento = Convert.ToDecimal(lblImporte.Text)
                    End If
                    If Not (.FindControl("lblCantidad") Is Nothing) Then
                        lblCantidad = CType(.FindControl("lblCantidad"), Label)
                        If Not lblCantidad.Text.Equals("") Then 'RGF 20081229
                            cantidadDocumento = Convert.ToDecimal(lblCantidad.Text)
                            totalCantidad = totalCantidad + cantidadDocumento
                        End If
                    End If
                    If (cantidadDocumento > 0) Then
                        precioDocumento = precioDocumento + (importeDocumento / cantidadDocumento)
                    End If
                    c = c + 1
                End If
            End With
        Next

        'Se asigna el precio promedio
        If (c <> 0) Then
            PrecioUnitario = Decimal.Round(precioDocumento / c, 5)
        Else
            PrecioUnitario = 0
        End If
        lbNominalTotal.Text = [NominalTotal].ToString()

        'INI CMB OT 61566 20101209
        If lbMnemonico.Text = "FORWARD" Then
            UnidadesTotal = [UnidadesTotal]
            PrecioUnitario = 1
            lbUnidadesTotal.Text = [NominalTotal].ToString()
            lbEtiqueta.InnerText = "Importe: "
        Else
            UnidadesTotal = totalCantidad
            lbUnidadesTotal.Text = totalCantidad.ToString()
            lbEtiqueta.InnerText = "Unidades: "
            If lbMnemonico.Text = "DIVISA" Then
                lbEtiqueta.InnerText = "Importe: "
            End If
        End If
        If Convert.ToBoolean(ViewState("Carga-Preordenes")) = False Then

            'LoadAsignacionPreOrdenes([UnidadesTotal])
        End If
    End Function

    Private Function ModificarTipoAsignacion() As Boolean
        Dim i As Int32 = 0
        Dim n As Int32 = 0

        n = dgLista.Rows.Count

        If ddlTipoAsignacion.SelectedIndex <> 0 Then

            For i = 0 To n - 1

                With dgLista.Rows(i)
                    Dim tbUnid As TextBox = CType(.FindControl("tbUnidades"), TextBox)
                    Dim tbAsign As TextBox = CType(.FindControl("tbAsignacion"), TextBox)

                    If ddlTipoAsignacion.SelectedValue = "U" Or ddlTipoAsignacion.SelectedValue = "I" Then
                        tbAsign.Enabled = False
                        tbUnid.Enabled = True
                    Else
                        tbAsign.Enabled = True
                        tbUnid.Enabled = False
                    End If

                    tbUnid.Text = .Cells(3).Text
                    tbAsign.Text = .Cells(1).Text
                End With
            Next
        End If
    End Function

    Private Function ProcesarCalculoAsignacion(ByVal unidadesTotal As Decimal) As DataTable

        Dim dt As DataTable
        Dim dr As DataRow
        Dim dlUnidades As Decimal = 0.0

        Dim sum As Decimal = 0.0
        Dim campos As String() = {"CodigoAsignacion", "CodigoAsignacionPreOrden", "FechaAsignacion", "CodigoPortafolioSBS", "UnidadesPropuesto", "UnidadesReal", "PorcentajePropuesto", "PorcentajeReal", "Temporal", "TipoAsignacion", "Estado", "Situacion"}
        Dim tipos As String() = {"String", "String", "String", "String", "String", "String", "String", "String", "String", "String", "String", "String"}

        Dim i As Integer

        dt = UIUtility.GetStructureTablebase(campos)

        For i = 0 To 2

            dr = dt.NewRow()
            dr("CodigoPortafolioSBS") = m_Fondo.Split(",").GetValue(i)
            dr("PorcentajePropuesto") = "33.33"

            If unidadesTotal Mod 3 = 0 Then
                dr("UnidadesPropuesto") = Format(unidadesTotal / 3, "######.00")
            Else
                If i = 2 Then
                    dlUnidades = Format(unidadesTotal - sum, "######.00")
                Else
                    dlUnidades = Math.Round(unidadesTotal / 3)
                    sum = sum + dlUnidades
                End If
                dr("UnidadesPropuesto") = Format(dlUnidades, "######.00")
            End If

            dt.Rows.Add(dr)

        Next
        Session("ResumenAsignacion") = dt
        Return dt

    End Function

    Private Function GuardarAsignaciones(ByVal EsTemporal As Boolean) As Boolean

        Dim resultAsigDel As Boolean = False
        Dim resultAsigIns As Boolean = False
        Dim resultAsigAgrupDel As Boolean = False
        Dim resultAsigAgrupIns As Boolean = False

        Dim i As Int32 = 0
        Dim n As Int32 = 0
        Dim m As Int32 = 0

        Dim tipAsignacion As String = String.Empty
        Dim codPortafolio As String = String.Empty
        Dim codPreOrden As String = String.Empty
        Dim codigoAsigPreOrden As String = String.Empty
        Dim codigoAsigPreOrdenNuevo As String = String.Empty
        Dim porcPropuesto As Decimal = 0
        Dim porcReal As Decimal = 0
        Dim unidPropuesto As Decimal = 0
        Dim unidReal As Decimal = 0

        Dim workFlow As New OrdenInversionWorkFlowBM
        Dim mensaje As String = String.Empty

        n = dgPreOrdenes.Rows.Count
        m = dgLista.Rows.Count

        resultAsigAgrupDel = workFlow.EliminarAsignacion(CodigoPreOrden, DatosRequest)
        codigoAsigPreOrden = 0

        If resultAsigAgrupDel Then

            For i = 0 To n - 1

                With dgPreOrdenes.Rows(i)

                    Dim chkPreOrden As CheckBox = CType(.FindControl("chkOrden"), CheckBox)

                    If chkPreOrden.Checked = True Then

                        resultAsigAgrupIns = workFlow.IngresarAsignacionAgrupacion(IIf(EsTemporal = True, "S", "N"), .Cells(1).Text, codigoAsigPreOrden, codigoAsigPreOrdenNuevo, DatosRequest)
                        codigoAsigPreOrden = IIf(codigoAsigPreOrden <> codigoAsigPreOrdenNuevo, codigoAsigPreOrdenNuevo, codigoAsigPreOrden)

                        [CodigoPreOrdenAsignacion] = codigoAsigPreOrden

                    End If

                End With

            Next

            If resultAsigAgrupIns Then

                For i = 0 To m - 1

                    With dgLista.Rows(i)

                        Dim [Porcentaje] As TextBox = CType(.FindControl("tbAsignacion"), TextBox)
                        Dim [Unidades] As TextBox = CType(.FindControl("tbUnidades"), TextBox)

                        tipAsignacion = ddlTipoAsignacion.SelectedValue.ToString()
                        codPortafolio = .Cells(0).Text
                        codPreOrden = CodigoPreOrden
                        porcPropuesto = Convert.ToDecimal(.Cells(1).Text.ToString())
                        porcReal = Convert.ToDecimal([Porcentaje].Text.ToString())
                        unidPropuesto = Convert.ToDecimal(.Cells(3).Text.ToString())
                        unidReal = Convert.ToDecimal([Unidades].Text.ToString())

                        If [Porcentaje].Text.Trim() <> "" And [Unidades].Text.Trim() <> "" Then

                            resultAsigIns = New OrdenInversionWorkFlowBM().IngresarAsignacion(IIf(EsTemporal = True, "S", "N"), codPortafolio, [CodigoPreOrdenAsignacion], unidPropuesto, unidReal, porcPropuesto, porcReal, tipAsignacion, DatosRequest)

                        End If
                    End With
                Next
            End If
        End If

        If resultAsigAgrupIns = True And resultAsigIns = True Then
            mensaje = m_MsgOK
            HabilitarControles(False)
        Else
            mensaje = m_MsgKO
            HabilitarControles(True)
        End If

        AlertaJS(mensaje)
    End Function
    Private Function GenerarAsignacionesDetalladas()
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM

        Dim i As Int32 = 0
        Dim n As Int32 = 0
        Dim j As Int32 = 0

        Dim resultB As Boolean = False
        Dim resultA As Boolean = False

        Dim mensaje As New System.Text.StringBuilder

        Dim codigoOI As String
        Dim codigoOIarr As New ArrayList
        Dim porcReal As Decimal = 0
        Dim cantidad As Decimal = 0
        Dim precio As Decimal

        Dim item As DataGridItem
        Dim portafolio As String
        Dim numeroPreorden As String

        For Each item In dgvDetalle.Rows
            cantidad = Convert.ToDecimal(item.Cells(2).Text)
            precio = Convert.ToDecimal(item.Cells(3).Text)
            portafolio = item.Cells(1).Text
            numeroPreorden = item.Cells(0).Text
            porcReal = Convert.ToDecimal(item.Cells(5).Text)
            Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
            oOrdenInversionBE = CrearObjetoOI(portafolio, cantidad, porcReal, precio)
            codigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, "", "", DatosRequest)
            UIUtility.CalculaImpuestosComisiones(portafolio, codigoOI, lbCodigoOrden.Text, cantidad * precio, DatosRequest)
            If ViewState("estadoOI") = "E-EXC" Or ViewState("estadoOI") = "E-EBL" Then
                Dim toUser As String = ""
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                Dim dt As DataTable
                dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                For Each fila As DataRow In dt.Rows
                    toUser = toUser + fila("Valor") + ";"
                Next
                Dim oOperacionBM As New OperacionBM
                Dim oOperacionBE As New OperacionBE
                Dim strOperacion As String = ""
                Dim strOrden As String = UIUtility.ObtenerDescripcionClaseInstrumento(ViewState("CategoriaInstrumento").ToString())
                oOperacionBE = oOperacionBM.Seleccionar(hdCodigoOperacion.Value.ToString(), DatosRequest)
                strOperacion = oOperacionBE.Tables(0).Rows(0)("Descripcion").ToString()
                UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(codigoOI, portafolio, DatosRequest), DatosRequest) 'CMB OT 62254 20110418
            End If
            'FIN CMB OT 61566 20101228
            codigoOIarr.Add(portafolio + "?" + codigoOI)
            GenerarLlamadoGeneral(codigoOI, oOrdenInversionBE)
        Next

        'Actualizar Estados de PreOrdenes 
        n = dgPreOrdenes.Rows.Count

        For i = 0 To n - 1
            With dgPreOrdenes.Rows(i)
                Dim chkPreOrden As CheckBox = CType(.FindControl("chkOrden"), CheckBox)
                If chkPreOrden.Checked = True Then
                    resultB = oOrdenInversionWorkFlowBM.ModificarEstadoAsignacionPreOrden(.Cells(1).Text.ToString(), DatosRequest)
                End If
            End With
        Next

        For i = 0 To codigoOIarr.Count - 1

            Dim dato As String() = codigoOIarr(i).ToString.Split("?")
            portafolio = dato(0)
            Dim codigoOrden As String = dato(1)
            numeroPreorden = dgvDetalle.Rows(i).Cells(0).Text
            resultA = oOrdenInversionWorkFlowBM.ModificarCodigoAsignacionOI(codigoOrden, numeroPreorden, DatosRequest)
            mensaje.Append("\t- " + portafolio + " -> " + codigoOrden + "\n")

        Next

        AlertaJS(m_MsgIO + " Ordenes Generadas: " + "\n" + mensaje.ToString())
        PageInit()
        HabilitarControles(False)
        EjecutaLlamado()
    End Function
    Private Function GenerarAsignacionesFinales()

        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM

        Dim i As Int32 = 0
        Dim n As Int32 = 0
        Dim j As Int32 = 0

        Dim resultB As Boolean = False
        Dim resultA As Boolean = False

        Dim categoria As String = String.Empty
        Dim codigoMoneda As String = String.Empty
        Dim operacion As String = String.Empty
        Dim codigoPreOrdenAsig As String = String.Empty
        Dim mensaje As New System.Text.StringBuilder

        Dim codigoOI As String
        Dim codigoOIarr As New ArrayList
        Dim porcReal As Decimal = 0
        Dim unidReal As Decimal = 0
        Dim precio As Decimal
        With DatosPreOrden.Tables(0)

            categoria = .Rows(0)("CategoriaInstrumento").ToString()
            codigoMoneda = .Rows(0)("CodigoMoneda").ToString()

        End With

        codigoPreOrdenAsig = [CodigoPreOrdenAsignacion]
        If (ddlModo.SelectedValue = "G") Then
            'Generar Ordenes Inversion
            For i = 0 To 2
                With dgLista.Rows(i)
                    Dim [Porcentaje] As TextBox = CType(.FindControl("tbAsignacion"), TextBox)
                    Dim [Unidades] As TextBox = CType(.FindControl("tbUnidades"), TextBox)
                    porcReal = Convert.ToDecimal([Porcentaje].Text.ToString())
                    unidReal = Convert.ToDecimal([Unidades].Text.ToString())
                    If (unidReal <> 0) And (porcReal <> 0) Then
                        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
                        oOrdenInversionBE = CrearObjetoOI(m_Fondo.Split(",").GetValue(i), unidReal.ToString(), porcReal.ToString(), PrecioUnitario)
                        codigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, "", "", DatosRequest)
                        UIUtility.CalculaImpuestosComisiones(m_Fondo.Split(",").GetValue(i), codigoOI, lbCodigoOrden.Text, unidReal * PrecioUnitario, DatosRequest)
                        'INI CMB OT 61566 20101228
                        If ViewState("estadoOI") = "E-EXC" Or ViewState("estadoOI" & m_Fondo.Split(",").GetValue(i).Substring(m_Fondo.Split(",").GetValue(i).Length - 1)) = "E-EXC" Or ViewState("estadoOI") = "E-EBL" Then 'CMB OT 62254 20110418   'HDG INC 62817	20110419
                            'Dim fromUser As String = Usuario.ToString.Trim & "@afphorizonte.com.pe"
                            Dim toUser As String = ""
                            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                            Dim dt As DataTable
                            dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                            For Each fila As DataRow In dt.Rows
                                toUser = toUser + fila("Valor") + ";"
                            Next
                            Dim oOperacionBM As New OperacionBM
                            Dim oOperacionBE As New OperacionBE
                            Dim strOperacion As String = ""
                            Dim strOrden As String = UIUtility.ObtenerDescripcionClaseInstrumento(ViewState("CategoriaInstrumento").ToString())
                            oOperacionBE = oOperacionBM.Seleccionar(hdCodigoOperacion.Value.ToString(), DatosRequest)
                            strOperacion = oOperacionBE.Tables(0).Rows(0)("Descripcion").ToString()
                            UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(codigoOI, "HO-FONDO" & i + 1, DatosRequest), DatosRequest) 'CMB OT 62254 20110418
                        End If
                        'FIN CMB OT 61566 20101228
                        codigoOIarr.Add(m_Fondo.Split(",").GetValue(i) + "?" + codigoOI)
                        GenerarLlamadoGeneral(codigoOI, oOrdenInversionBE)
                    End If
                End With
            Next
        Else
            Dim item As DataGridItem
            Dim portafolio As String
            Dim numeroPreorden As String
            For Each item In dgvDetalle.Rows
                unidReal = Convert.ToDecimal(item.Cells(2).Text)
                precio = Convert.ToDecimal(item.Cells(3).Text)
                portafolio = item.Cells(3).Text
                numeroPreorden = item.Cells(0).Text
            Next
        End If

        'Actualizar Estados de PreOrdenes 
        n = dgPreOrdenes.Rows.Count

        For i = 0 To n - 1

            With dgPreOrdenes.Rows(i)

                Dim chkPreOrden As CheckBox = CType(.FindControl("chkOrden"), CheckBox)

                If chkPreOrden.Checked = True Then

                    resultB = oOrdenInversionWorkFlowBM.ModificarEstadoAsignacionPreOrden(.Cells(1).Text.ToString(), DatosRequest)

                End If

            End With

        Next


        'Actualizar Códigos de Asignación en Ordenes de Inversión

        For i = 0 To codigoOIarr.Count - 1

            Dim dato As String() = codigoOIarr(i).ToString.Split("?")
            Dim portafolio As String = dato(0)
            Dim codigoOrden As String = dato(1)

            resultA = oOrdenInversionWorkFlowBM.ModificarCodigoAsignacionOI(codigoOrden, [CodigoPreOrdenAsignacion], DatosRequest)
            mensaje.Append("\t- " + portafolio + " -> " + codigoOrden + "\n")

        Next

        AlertaJS(m_MsgIO + " Ordenes Generadas: " + "\n" + mensaje.ToString())
        PageInit()
        HabilitarControles(False)
        EjecutaLlamado()
    End Function
    'CMB OT 61566 20101228
    Private Sub CalcularLimiteExcesos(ByVal codOperacion As String, ByVal codNemonico As String, ByVal cantOperacion As Decimal, ByVal codPortafolio As String)
        Dim oLimiteEvaluacion As New LimiteEvaluacionBM
        Dim dsAux As New DataSet
        Dim codigoOperacion As String = codOperacion
        Dim codigoNemonico As String = codNemonico
        Dim cantidadOperacion As Decimal = cantOperacion
        Dim codigoPortafolio As String = codPortafolio

        dsAux = oLimiteEvaluacion.ListarExcesosLimitesOnLine(codigoNemonico, cantidadOperacion, codigoPortafolio, codigoOperacion, DatosRequest, Session("EmisorInt"))  'HDG 20120113

        ViewState("estadoOI") = ""

        If Not (dsAux Is Nothing) Then
            If (dsAux.Tables.Count > 0) Then
                If (dsAux.Tables(0).Rows.Count > 0) Then
                    ViewState("estadoOI") = "E-EXC"
                End If
            End If
        End If
    End Sub

    Private Function HabilitarControles(ByVal enabled As Boolean) As Boolean
        btnAceptar.Enabled = enabled
    End Function

#End Region

#Region " /* Métodos y Eventos de Página */ "


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            PageInit()
            CargarTablaLlamado()
        End If
    End Sub

    Private Sub CargarTablaLlamado()
        ViewState("Llamado") = Nothing
        Dim dtAux As New DataTable
        dtAux.Columns.Add("codigo")
        dtAux.Columns.Add("portafolio")
        dtAux.Columns.Add("clase")
        dtAux.Columns.Add("claseDetalle")
        dtAux.Columns.Add("operacion")
        dtAux.Columns.Add("moneda")
        dtAux.Columns.Add("mnemonico")
        ViewState("Llamado") = dtAux
    End Sub

    Private Sub ddlTipoAsignacion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoAsignacion.SelectedIndexChanged
        ReprocesarCalculoAsignacion(UnidadesTotal)
    End Sub
    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            ActualizaDistribucion()
            If ValidaSaldos() Then
                GuardarAsignaciones(True)
                HabilitarControles(True)
                LimitesOnline()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If ddlModo.SelectedValue = "G" Then
                GenerarAsignacionesFinales()
            Else
                GenerarAsignacionesDetalladas()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        If ValidaSaldos() Then
            HabilitarControles(True)
            LimitesOnline()
        End If
    End Sub

    Private Function ValidaSaldos() As Boolean
        Dim dgItem As DataGridItem
        Dim tbUnid As TextBox

        If hdCodigoOperacion.Value.Equals("2") Then
            Dim dt As DataTable = New OrdenPreOrdenInversionBM().ObtenerSaldosCustodio(lbMnemonico.Text, DatosRequest).Tables(0)
            For Each dgItem In dgLista.Rows
                tbUnid = CType(dgItem.FindControl("tbUnidades"), TextBox)
                If tbUnid.Text > dt.Rows(0)("saldoF" + Right(dgItem.Cells(0).Text, 1)) Then
                    AlertaJS("No existe el saldo suficiente para asignar al fondo " + dgItem.Cells(0).Text)
                    Return False
                End If
            Next
        End If

        Return True
    End Function

    Private Sub ActualizaDistribucion()
        Dim dgItem As DataGridItem
        Dim tbUnid, tbPorc As TextBox

        For Each dgItem In dgLista.Rows
            tbPorc = CType(dgItem.FindControl("tbAsignacion"), TextBox)
            tbUnid = CType(dgItem.FindControl("tbUnidades"), TextBox)
            If ddlTipoAsignacion.SelectedValue = "P" Then
                tbUnid.Text = tbPorc.Text * lbUnidadesTotal.Text / 100
            Else
                tbPorc.Text = tbUnid.Text / lbUnidadesTotal.Text * 100
            End If
        Next
    End Sub
#End Region

#Region " /* (ANTIGUO) Métodos Personalizados (ANTIGUO) */ "

    Public Function CrearObjetoOI(ByVal strCodigoPortafolioSBS As String, ByVal dclCantidadOperacion As Decimal, ByVal dclPorcentajeAsignacion As Decimal, ByVal precio As Decimal) As OrdenPreOrdenInversionBE

        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM

        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)

        With oRow

            With DatosPreOrden.Tables(0)
                oRow.CodigoPreOrden = ""
                oRow.CodigoPortafolioSBS = strCodigoPortafolioSBS

                If ViewState("estadoOI") = "" Then
                    oRow.Estado = IIf(IsNothing(ViewState("estadoOI" & strCodigoPortafolioSBS.Substring(strCodigoPortafolioSBS.Length - 1))), "", ViewState("estadoOI" & strCodigoPortafolioSBS.Substring(strCodigoPortafolioSBS.Length - 1)))  'HDG INC 63319	20110606
                Else
                    oRow.Estado = ViewState("estadoOI")
                End If

                oRow.CodigoOperacion = .Rows(0).Item(.Columns.IndexOf("CodigoOperacion"))
                oRow.CodigoMoneda = .Rows(0).Item(.Columns.IndexOf("CodigoMoneda"))
                oRow.CodigoISIN = .Rows(0).Item(.Columns.IndexOf("CodigoISIN"))
                oRow.CodigoMnemonico = .Rows(0).Item(.Columns.IndexOf("CodigoMnemonico"))
                oRow.CodigoSBS = .Rows(0).Item(.Columns.IndexOf("CodigoSBS"))
                oRow.CodigoTercero = .Rows(0).Item(.Columns.IndexOf("CodigoTercero"))
                oRow.CodigoContacto = .Rows(0).Item(.Columns.IndexOf("CodigoContacto"))
                oRow.FechaOperacion = .Rows(0).Item(.Columns.IndexOf("FechaOperacion"))
                oRow.FechaLiquidacion = .Rows(0).Item(.Columns.IndexOf("FechaLiquidacion"))
                oRow.TasaCastigo = .Rows(0).Item(.Columns.IndexOf("TasaCastigo"))
                oRow.YTM = .Rows(0).Item(.Columns.IndexOf("YTM"))
                oRow.PrecioCalculado = .Rows(0).Item(.Columns.IndexOf("PrecioCalculado"))
                oRow.PrecioNegociacionLimpio = .Rows(0).Item(.Columns.IndexOf("PrecioCalculado"))
                oRow.PrecioNegociacionSucio = .Rows(0).Item(.Columns.IndexOf("PrecioCalculado"))
                oRow.PrecioPromedio = .Rows(0).Item(.Columns.IndexOf("PrecioCalculado"))
                oRow.PTasa = .Rows(0).Item(.Columns.IndexOf("PrecioCalculado"))
                If (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "DP") Then
                    oRow.MontoNominalOperacion = dclCantidadOperacion
                    oRow.MontoNominalOrdenado = dclCantidadOperacion
                Else
                    If (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "FD") Then
                        oRow.MontoNominalOrdenado = .Rows(0).Item(.Columns.IndexOf("MontoNominalOrdenado"))
                        oRow.MontoNominalOperacion = .Rows(0).Item(.Columns.IndexOf("MontoNominalOperacion"))
                    Else
                        oRow.MontoNominalOperacion = (dclCantidadOperacion * precio)
                        oRow.MontoNominalOrdenado = (dclCantidadOperacion * precio)
                    End If
                End If

                oRow.Precio = precio
                oRow.TipoFondo = .Rows(0).Item(.Columns.IndexOf("TipoFondo"))
                oRow.FechaTrato = .Rows(0).Item(.Columns.IndexOf("FechaTrato"))

                If (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "FD") Then
                    oRow.CantidadOrdenado = .Rows(0).Item(.Columns.IndexOf("CantidadOrdenado"))
                    oRow.CantidadOperacion = .Rows(0).Item(.Columns.IndexOf("CantidadOperacion"))
                    oRow.MontoOperacion = .Rows(0).Item(.Columns.IndexOf("TipoCambioFuturo")) * (dclCantidadOperacion * precio)
                Else
                    oRow.CantidadOrdenado = dclCantidadOperacion.ToString.Replace(UIUtility.DecimalSeparator, ".")
                    oRow.CantidadOperacion = dclCantidadOperacion.ToString.Replace(UIUtility.DecimalSeparator, ".")
                    oRow.MontoOperacion = (dclCantidadOperacion * precio)
                End If
                'FIN CMB OT 61566 20101213
                oRow.TotalComisiones = (.Rows(0).Item(.Columns.IndexOf("TotalComisiones")) * (dclPorcentajeAsignacion / 100)).ToString.Replace(UIUtility.DecimalSeparator, ".")
                oRow.PrecioPromedio = .Rows(0).Item(.Columns.IndexOf("PrecioPromedio")).ToString.Replace(UIUtility.DecimalSeparator, ".")

                'INI CMB OT 61566 20101213
                If (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "FD") Then
                    oRow.MontoNetoOperacion = .Rows(0).Item(.Columns.IndexOf("MontoNetoOperacion"))
                Else
                    oRow.MontoNetoOperacion = (oRow.MontoOperacion + oRow.TotalComisiones).ToString.Replace(UIUtility.DecimalSeparator, ".")
                End If
                'FIN CMB OT 61566 20101213

                oRow.Situacion = "A"
                oRow.Observacion = .Rows(0).Item(.Columns.IndexOf("Observacion"))
                oRow.HoraOperacion = .Rows(0).Item(.Columns.IndexOf("HoraOperacion"))
                oRow.CategoriaInstrumento = .Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento"))

                oRow.FechaContrato = .Rows(0).Item(.Columns.IndexOf("FechaContrato"))
                oRow.CodigoTipoCupon = .Rows(0).Item(.Columns.IndexOf("CodigoTipoCupon"))
                oRow.Plazo = .Rows(0).Item(.Columns.IndexOf("Plazo"))
                oRow.FechaPago = .Rows(0).Item(.Columns.IndexOf("FechaPago"))
                oRow.CodigoTipoTitulo = .Rows(0).Item(.Columns.IndexOf("CodigoTipoTitulo"))
                oRow.TasaPorcentaje = .Rows(0).Item(.Columns.IndexOf("TasaPorcentaje"))

                'LETV 20090310
                oRow.Plaza = .Rows(0).Item(.Columns.IndexOf("Plaza"))

                'RGF 20090210
                If (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "CV") Or (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "FD") Then 'CMB OT 61566 20101213 --Or (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "FD")
                    oRow.MontoOrigen = dclCantidadOperacion
                    oRow.CodigoPreOrden = CodigoPreOrden
                End If

                'CMB OT 61566 20101213
                If (.Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")) = "FD") Then
                    oRow.TipoCambioSpot = .Rows(0).Item(.Columns.IndexOf("TipoCambioSpot"))
                    oRow.TipoCambioFuturo = .Rows(0).Item(.Columns.IndexOf("TipoCambioFuturo"))
                    oRow.CodigoMonedaDestino = .Rows(0).Item(.Columns.IndexOf("CodigoMonedaDestino"))
                    oRow.CodigoMonedaOrigen = .Rows(0).Item(.Columns.IndexOf("CodigoMonedaOrigen"))
                    oRow.Delibery = .Rows(0).Item(.Columns.IndexOf("Delibery"))
                    oRow.CodigoMotivo = .Rows(0).Item(.Columns.IndexOf("CodigoMotivo"))
                    oRow.MontoCancelar = dclCantidadOperacion * precio
                    'CALCULO DEL DIFERENCIAL
                    Dim diferencial As Decimal
                    Dim objFormulasOI As New OrdenInversionFormulasBM
                    diferencial = objFormulasOI.CalularDiferencial(Convert.ToDecimal(.Rows(0).Item(.Columns.IndexOf("TipoCambioFuturo"))), Convert.ToDecimal(.Rows(0).Item(.Columns.IndexOf("TipoCambioSpot"))), .Rows(0).Item(.Columns.IndexOf("FechaContrato")), .Rows(0).Item(.Columns.IndexOf("FechaOperacion")), DatosRequest)
                    oRow.Diferencial = diferencial
                End If
                oRow.GrupoIntermediario = .Rows(0).Item(.Columns.IndexOf("GrupoIntermediario")) 'HDG 20120116
                oRow.TipoTramo = .Rows(0).Item(.Columns.IndexOf("TipoTramo")) 'HDG 20120116
                oRow.HoraEjecucion = .Rows(0).Item(.Columns.IndexOf("HoraEjecucion")) 'HDG 20120116
            End With

        End With

        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()

        Return oOrdenInversionBE

    End Function

    Private Sub CargarTipoAsignacion()
        HelpCombo.LlenarComboBox(ddlTipoAsignacion, New ParametrosGeneralesBM().ListarTipoAsignacion(DatosRequest), "Valor", "Nombre", True)
    End Sub

    Public Function Valida() As Boolean

        Dim i As Integer
        Dim TotPor As Decimal = 0.0
        Dim TotUnd As Decimal = 0.0

        For i = 0 To 2
            Dim tbUnid As TextBox = CType(dgLista.Rows(i).FindControl("tbUnidades"), TextBox)
            Dim tbAsign As TextBox = CType(dgLista.Rows(i).FindControl("tbAsignacion"), TextBox)
            If tbUnid.Text.Trim = "" Or tbAsign.Text.Trim = "" Then
                tbUnid.Text = 0
                tbAsign.Text = 0
            End If
            TotPor = Format(TotPor + tbAsign.Text, "#######.00")
            TotUnd = Format(TotUnd + tbUnid.Text, "#######.00")
        Next

        If ddlTipoAsignacion.SelectedValue = "U" And TotUnd <> lbUnidades.Text Then
            AlertaJS("Las unidades deben coincidir con las ingresadas")
            Return True
        Else
            If ddlTipoAsignacion.SelectedValue = "P" And (Math.Round(TotPor)) <> 100 Then
                AlertaJS("Las porcentajes no deben excederse")
                Return True
            End If
        End If
    End Function

    Public Function LimpiarCampos() As Boolean
        Dim i As Integer

        For i = 0 To 2
            Dim tbUnid As TextBox = CType(dgLista.Rows(i).FindControl("tbUnidades"), TextBox)
            Dim tbAsign As TextBox = CType(dgLista.Rows(i).FindControl("tbAsignacion"), TextBox)

            Select Case ddlTipoAsignacion.SelectedValue
                Case "U"
                    tbUnid.Text = ""
                Case "I"
                    tbUnid.Text = ""
                Case "P"
                    tbAsign.Text = ""
                Case Else
                    tbUnid.Text = dgLista.Rows(i).Cells(3).Text
                    tbAsign.Text = dgLista.Rows(i).Cells(1).Text
            End Select
        Next
    End Function

    'DB 20090507
    Private Sub GenerarLlamadoGeneral(ByVal codigo As String, ByVal objBE As OrdenPreOrdenInversionBE)
        Dim strClase As String = ""
        Dim objCI As New ClaseInstrumentoBM
        Dim objCI_BE As ClaseInstrumentoBE = objCI.Listar(DatosRequest)

        strClase = UIUtility.ObtenerDescripcionClaseInstrumento(CType(objBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow).CategoriaInstrumento)
        Dim strPortafolio As String = CType(objBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow).CodigoPortafolioSBS
        Dim strOperacion As String = CType(objBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow).CodigoOperacion
        strOperacion = New OperacionBM().Seleccionar(strOperacion, DatosRequest).Tables(0).Rows(0)("Descripcion") 'RGF 20090702
        Dim strMoneda As String = CType(objBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow).CodigoMoneda
        Dim strMnemonico As String = CType(objBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow).CodigoMnemonico

        Dim drAux As DataRow = CType(ViewState("Llamado"), DataTable).NewRow
        drAux("codigo") = codigo
        drAux("portafolio") = strPortafolio
        drAux("clase") = CType(objBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow).CategoriaInstrumento
        drAux("claseDetalle") = strClase
        drAux("operacion") = strOperacion
        drAux("moneda") = strMoneda
        drAux("mnemonico") = strMnemonico
        CType(ViewState("Llamado"), DataTable).Rows.Add(drAux)
        'GenerarLlamado(codigo, strPortafolio, strClase, strOperacion, strMoneda, strMnemonico)
    End Sub

    'DB 20090507
    Private Sub EjecutaLlamado()
        Dim dtAux As DataTable = CType(ViewState("Llamado"), DataTable)
        Dim i As Integer
        Dim strAccion As String = "ASIGNAR"
        Session("dtdatosoperacion") = Nothing
        With dtAux
            For i = 0 To .Rows.Count - 1
                GenerarLlamado(.Rows(i)("codigo"), .Rows(i)("portafolio"), .Rows(i)("claseDetalle"), .Rows(i)("operacion"), .Rows(i)("moneda"), .Rows(i)("mnemonico"))
            Next
        End With
    End Sub

    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/VisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "No", "Yes", "Yes"), False)
    End Sub

    Public Sub GenerarOI(ByVal url As String)
        EjecutarJS(UIUtility.MostrarPopUp(url, "10", 1000, 650, 0, 0, "No", "No", "Yes", "Yes"), False)
    End Sub

#End Region

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSalir.Click
        Dim url_Anterior As String
        url_Anterior = CType(Session("URL_Anterior"), String)
        Session("URL_Anterior") = Nothing
        If (Not url_Anterior Is Nothing And (url_Anterior.Length > 0)) Then
            Response.Redirect(url_Anterior)
        End If
    End Sub

    Private Sub ddlModo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModo.SelectedIndexChanged
        AsignarPreOrdenesSeleccionadas()
        ReprocesarCalculoAsignacion(UnidadesTotal)

        If (ddlModo.SelectedValue = "D") Then
            AsignarPreOrdenesSeleccionadas()
            ReprocesarCalculoAsignacionDetalle(UnidadesTotal)

        Else
            dgvDetalle.DataSource = Nothing
            dgvDetalle.DataBind()

        End If
        If asignacionOK Then
            HabilitarControles(False)
        End If

    End Sub

    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/VisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub

    'ini HDG INC 62817	20110419
    Private Sub LimitesOnline()
        Dim i As Int32 = 0
        Dim item As DataGridItem
        Dim oLimiteEvaluacion As New LimiteEvaluacionBM
        Dim dsAux As New DataSet
        Dim codigoOperacion As String = ""
        Dim codigoNemonico As String = ""
        Dim cantidadOperacion As Decimal = 0
        Dim codigoPortafolio As String = ""
        Dim cantidadTotOperacion As Decimal = 0
        Dim Guid As String = System.Guid.NewGuid.ToString()
        ViewState("estadoOI") = ""
        ViewState("CategoriaInstrumento") = ""

        With DatosPreOrden.Tables(0)
            codigoNemonico = .Rows(0).Item(.Columns.IndexOf("CodigoMnemonico"))
            cantidadTotOperacion = lbUnidadesTotal.Text.ToString.Replace(UIUtility.DecimalSeparator, ".")
            codigoOperacion = .Rows(0).Item(.Columns.IndexOf("CodigoOperacion"))
            Guid = System.Guid.NewGuid.ToString()

            dsAux = oLimiteEvaluacion.ListarExcesosLimitesOnLine(codigoNemonico, cantidadTotOperacion, codigoMultifondo, codigoOperacion, DatosRequest, Session("EmisorInt"))   'HDG 20120113
            If Not (dsAux Is Nothing) Then
                If (dsAux.Tables.Count > 0) Then
                    If (dsAux.Tables(0).Rows.Count > 0) Then
                        Session("Instrumento") = codigoNemonico
                        Session(Guid) = dsAux
                        Session("dsExcesoLimites") = dsAux
                        ViewState("estadoOI") = "E-EXC" 'CMB OT 61566 20101228
                        ViewState("CategoriaInstrumento") = .Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")).ToString()
                        EjecutarJS(UIUtility.MostrarPopUp("../InstrumentosNegociados/ConsultaLimitesInstrumento.aspx?GUID=" + Guid, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                    End If
                End If
            Else
                If (ddlModo.SelectedValue = "G") Then
                    For i = 0 To 2
                        Dim [Unidades] As TextBox = CType(dgLista.Rows(i).FindControl("tbUnidades"), TextBox)
                        codigoPortafolio = m_Fondo.Split(",").GetValue(i)
                        cantidadOperacion = Convert.ToDecimal([Unidades].Text.ToString()).ToString.Replace(UIUtility.DecimalSeparator, ".")
                        Guid = System.Guid.NewGuid.ToString()

                        dsAux = oLimiteEvaluacion.ListarExcesosLimitesOnLine(codigoNemonico, cantidadOperacion, codigoPortafolio, codigoOperacion, DatosRequest, Session("EmisorInt"))  'HDG 20120113
                        If Not (dsAux Is Nothing) Then
                            If (dsAux.Tables.Count > 0) Then
                                If (dsAux.Tables(0).Rows.Count > 0) Then
                                    Session("Instrumento") = codigoNemonico
                                    Session(Guid) = dsAux
                                    Session("dsExcesoLimites") = dsAux
                                    ViewState("estadoOI") = "E-EXC"
                                    ViewState("CategoriaInstrumento") = .Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")).ToString()
                                    EjecutarJS(UIUtility.MostrarPopUp("../InstrumentosNegociados/frmConsultaLimitesInstrumento.aspx?GUID=" + Guid, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                                End If
                            End If
                        End If
                    Next
                Else
                    For Each item In dgvDetalle.Rows
                        codigoPortafolio = item.Cells(1).Text
                        cantidadOperacion = Convert.ToDecimal(item.Cells(2).Text).ToString.Replace(UIUtility.DecimalSeparator, ".")
                        Guid = System.Guid.NewGuid.ToString()

                        dsAux = oLimiteEvaluacion.ListarExcesosLimitesOnLine(codigoNemonico, cantidadOperacion, codigoPortafolio, codigoOperacion, DatosRequest, Session("EmisorInt"))  'HDG 20120113
                        If Not (dsAux Is Nothing) Then
                            If (dsAux.Tables.Count > 0) Then
                                If (dsAux.Tables(0).Rows.Count > 0) Then
                                    Session("Instrumento") = codigoNemonico
                                    Session(Guid) = dsAux
                                    Session("dsExcesoLimites") = dsAux
                                    ViewState("estadoOI" & codigoPortafolio.Substring(codigoPortafolio.Length - 1)) = "E-EXC"
                                    ViewState("CategoriaInstrumento") = .Rows(0).Item(.Columns.IndexOf("CategoriaInstrumento")).ToString()
                                    EjecutarJS(UIUtility.MostrarPopUp("../InstrumentosNegociados/frmConsultaLimitesInstrumento.aspx?GUID=" + Guid, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        End With
    End Sub
    'fin HDG INC 62817	20110419

    Protected Sub dgPreOrdenes_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgPreOrdenes.RowDataBound
        Dim lblImporte As Label
        Dim lblCantidad As Label
        Dim dr As DataRowView
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            dr = CType(e.Row.DataItem, DataRowView)
            If Not (e.Row.FindControl("lblImporte") Is Nothing) Then
                lblImporte = CType(e.Row.FindControl("lblImporte"), Label)
                lblImporte.Text = Convert.ToString(dr("MontoOperacion"))
            End If
            If Not (e.Row.FindControl("lblCantidad") Is Nothing) Then
                lblCantidad = CType(e.Row.FindControl("lblCantidad"), Label)
                lblCantidad.Text = Convert.ToString(dr("CantidadOperacion"))
            End If
        End If
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim tbAsignacion As TextBox
        Dim tbUnidades As TextBox
        Dim dr As DataRowView
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            dr = CType(e.Row.DataItem, DataRowView)
            If Not (e.Row.FindControl("tbAsignacion") Is Nothing) Then
                tbAsignacion = CType(e.Row.FindControl("tbAsignacion"), TextBox)
                tbAsignacion.Text = Convert.ToString(dr("PorcentajePropuesto"))
            End If
            If Not (e.Row.FindControl("tbUnidades") Is Nothing) Then
                tbUnidades = CType(e.Row.FindControl("tbUnidades"), TextBox)
                tbUnidades.Text = Convert.ToString(dr("UnidadesPropuesto"))
            End If
        End If
    End Sub
End Class
