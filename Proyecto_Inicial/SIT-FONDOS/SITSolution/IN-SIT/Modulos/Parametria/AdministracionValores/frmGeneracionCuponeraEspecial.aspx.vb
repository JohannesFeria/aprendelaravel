Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Parametria_AdministracionValores_frmGeneracionCuponeraEspecial
    Inherits BasePage
    Dim strFlagAmortizacion As String
    Dim decTasaCupon As String
    Dim decTasaSpread As String
    Dim decBaseCupon As String
    Dim periodicidad As String
    Dim numCupones As String
    Dim codPeriodicidad As String

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnGenerarCuponera.Attributes.Add("onclick", "javascript:return Validar();")

        If Not (Page.IsPostBack) Then
            CargarCombo()
            pnlCupon.Visible = False
            btnAgregar.Visible = False
            tbCodigoNemonico.Text = Request.QueryString("vMnemo")
            tbCodigoIsin.Text = Request.QueryString("vISIN")
            tbFechaEmision.Text = Request.QueryString("vFechaE")
            tbFechaVencimiento.Text = Request.QueryString("vFechaV")
            tbFechaPrimer.Text = Request.QueryString("vFechaP")
            tbTasaSpread.Text = Request.QueryString("vTasaSpread")
            hdFechaInicial.Value = UIUtility.ConvertirFechaaDecimal(Request.QueryString("vFechaP"))
            hdAmort.Value = Request.QueryString("vFlag")
            decTasaCupon = Request.QueryString("vTasaC").Replace(".", UIUtility.DecimalSeparator)
            hdBase.Value = Request.QueryString("vBaseC").Replace(".", UIUtility.DecimalSeparator)
            periodicidad = Request.QueryString("vPeriod")
            codPeriodicidad = Request.QueryString("vCodPeriod")

            ddlPeriodicidad.SelectedValue = codPeriodicidad
            tbTasaEncaje.Text = decTasaCupon.Replace(UIUtility.DecimalSeparator, ".")

            If Session("accionValor") = "INGRESAR" Then
                If Not Session("cuponeraEspecial") Is Nothing Then
                    dgLista.DataSource = CType(Session("cuponeraEspecial"), DataTable)
                    dgLista.DataBind()
                    ActualizaCuponeraEspecial()
                    tbDe.Text = Convert.ToInt32(dgLista.Rows(dgLista.Rows.Count - 1).Cells(2).Text) + 1
                End If
            ElseIf Session("accionValor") = "MODIFICAR" Then
                If Session("cuponeraEspecial") Is Nothing Then
                    cargarCuponeraEspecial()
                Else
                    dgLista.DataSource = CType(Session("cuponeraEspecial"), DataTable)
                    dgLista.DataBind()
                    ActualizaCuponeraEspecial()
                End If
                If dgLista.Rows.Count > 0 Then
                    tbDe.Text = Convert.ToInt32(dgLista.Rows(dgLista.Rows.Count - 1).Cells(2).Text) + 1
                End If
            End If

            If Request.QueryString("vReadOnly") = "YES" Then
                bloqueatodo()
            End If
            Dim dtTemporal As New DataTable
            If Not Session("cuponeraEspecial") Is Nothing Then
                dtTemporal = CType(Session("cuponeraEspecial"), DataTable).Copy
                Session("Temporal") = dtTemporal
            Else
                Session("Temporal") = Nothing
            End If
        End If
    End Sub

    Private Sub bloqueatodo()
        tbCodigoNemonico.ReadOnly = True
        tbCodigoIsin.ReadOnly = True
        tbFechaEmision.ReadOnly = True
        tbFechaVencimiento.ReadOnly = True
        tbFechaPrimer.ReadOnly = True
        tbA.ReadOnly = True
        tbDe.ReadOnly = True
        tbTasaEncaje.ReadOnly = True
        ddlPeriodicidad.Enabled = False
        btnGenerarCuponera.Visible = False
        btnAceptar.Visible = False
    End Sub

    Private Sub cargarCuponeraEspecial()
        Dim oCuponeraBM As New CuponeraBM
        Dim dtAux As DataTable
        dtAux = oCuponeraBM.LeerCuponeraEspecial(tbCodigoNemonico.Text, DatosRequest).Tables(0)
        dgLista.DataSource = dtAux
        dgLista.DataBind()
        Session("cuponeraEspecial") = dtAux
        ActualizaCuponeraEspecial()
    End Sub

    Private Sub CargarCombo()
        Dim DtTablaPeriodicidad As DataTable
        Dim dtTablaAmortizacionVenc As DataTable
        Dim oPeriodicidadBM As New PeriodicidadBM
        Dim oPG As New ParametrosGeneralesBM
        DtTablaPeriodicidad = oPeriodicidadBM.Listar(DatosRequest).Tables(0)
        dtTablaAmortizacionVenc = oPG.ListarAmortizacionVencimiento(DatosRequest)
        HelpCombo.LlenarComboBox(ddlPeriodicidad, DtTablaPeriodicidad, "CodigoPeriodicidad", "Descripcion", True)
        Session("periodicidad") = DtTablaPeriodicidad
        If dtTablaAmortizacionVenc.Rows.Count > 0 Then
            hdAmortVenc.Value = dtTablaAmortizacionVenc.Rows(0)("Valor")
        Else
            hdAmortVenc.Value = "-1"
        End If
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Session("cuponeraEspecial") = Session("Temporal")
        EjecutarJS("window.close();")
    End Sub

    Private Sub btnGenerarCuponera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarCuponera.Click
        numCupones = Convert.ToInt32(tbA.Text) - Convert.ToInt32(tbDe.Text) + 1
        If numCupones > 0 Then
            GeneraCuponeraEspecial()
        Else
            AlertaJS(ObtenerMensaje("CONF35"))
        End If
    End Sub

    Private Sub GeneraCuponeraEspecial()
        Dim oCuponera As New CuponeraBM
        Dim i As Integer
        Dim oDT As New DataTable
        periodicidad = obtenerNroDiasPeriodicidad(ddlPeriodicidad.SelectedValue)
        decTasaCupon = tbTasaEncaje.Text.Replace(".", UIUtility.DecimalSeparator)
        Dim decTotal As String
        decTotal = Request.QueryString("vIndicador").ToString

        decTasaSpread = Convert.ToString(Convert.ToDecimal(tbTasaSpread.Text.Replace(".", UIUtility.DecimalSeparator)) + Convert.ToDecimal(decTotal))
        oDT = oCuponera.GenerarCuponeraEspecial(hdAmort.Value, numCupones, Convert.ToDecimal(hdFechaInicial.Value), decTasaCupon, hdBase.Value, periodicidad, decTasaSpread, DatosRequest).Tables(0)
        agregarTablaGeneral(oDT)
        limpiarCampo()
        oDT = CType(Session("cuponeraEspecial"), DataTable)
        If oDT.Rows.Count > 0 Then
            tbDe.Text = Convert.ToInt32(oDT.Rows(oDT.Rows.Count - 1)("consecutivo")) + 1
        End If
    End Sub

    Private Sub agregarTablaGeneral(ByVal oDT As DataTable)
        Dim oDTaux As New DataTable
        Dim i As Integer
        If Session("cuponeraEspecial") Is Nothing Then
            oDTaux = oDT
            hdFechaInicial.Value = oDT.Rows(oDT.Rows.Count - 1)("FechaFin")
        Else
            oDTaux = CType(Session("cuponeraEspecial"), DataTable)
            If oDTaux.Rows.Count > 0 Then
                Dim intConsMax As Integer = oDTaux.Rows(oDTaux.Rows.Count - 1)("consecutivo")
                For i = 0 To oDT.Rows.Count - 1
                    oDT.Rows(i)("consecutivo") = intConsMax + i + 1
                    oDTaux.ImportRow(oDT.Rows(i))
                    If i = oDT.Rows.Count - 1 Then
                        hdFechaInicial.Value = oDT.Rows(i)("FechaFin")
                    End If
                Next
            Else
                oDTaux = oDT
                hdFechaInicial.Value = oDT.Rows(oDT.Rows.Count - 1)("FechaFin")
            End If
        End If
        If oDTaux.Rows.Count = 0 Then
            Exit Sub
        End If
        Dim nroFilas As Integer = oDTaux.Rows.Count
        Dim decAmortiza As Decimal = Math.Round(100 / nroFilas, 7)
        If hdAmort.Value = hdAmortVenc.Value Then 'AL VENCIMIENTO
            For i = 0 To oDTaux.Rows.Count - 2
                oDTaux.Rows(i)("Amortizac") = "0.0000000"
            Next
            oDTaux.Rows(oDTaux.Rows.Count - 1)("Amortizac") = "100.0000000"
        Else    'CADA CUPON
            Dim decSubTotalAmortiza As Decimal = 0.0
            For i = 0 To oDTaux.Rows.Count - 2
                oDTaux.Rows(i)("Amortizac") = decAmortiza
                decSubTotalAmortiza = decSubTotalAmortiza + decAmortiza
            Next
            oDTaux.Rows(oDTaux.Rows.Count - 1)("Amortizac") = 100 - decSubTotalAmortiza
        End If
        dgLista.DataSource = oDTaux
        dgLista.DataBind()
        Session("cuponeraEspecial") = oDTaux
        ActualizaCuponeraEspecial()
    End Sub

    Private Sub ActualizaCuponeraEspecial()
        Dim i As Integer
        Try
            For i = 0 To dgLista.Rows.Count - 1
                dgLista.Rows(i).Cells(3).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dgLista.Rows(i).Cells(3).Text))
                dgLista.Rows(i).Cells(4).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dgLista.Rows(i).Cells(4).Text))
                dgLista.Rows(i).Cells(5).Text = CType(dgLista.Rows(i).Cells(5).Text, String).Replace(UIUtility.DecimalSeparator, ".")
                dgLista.Rows(i).Cells(7).Text = CType(dgLista.Rows(i).Cells(7).Text, String).Replace(UIUtility.DecimalSeparator, ".")
                dgLista.Rows(i).Cells(8).Text = CType(dgLista.Rows(i).Cells(8).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub limpiarCampo()
        tbA.Text = ""
        tbTasaEncaje.Text = ""
        ddlPeriodicidad.SelectedIndex = 0
        tbTasaSpread.Text = ""
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAceptar.Click
        EjecutarJS("window.close();")
    End Sub

    Private Function obtenerNroDiasPeriodicidad(ByVal strValor As String) As Integer
        Dim dtPeriodicidad As DataTable
        dtPeriodicidad = Session("periodicidad")
        Dim i As Integer
        For i = 0 To dtPeriodicidad.Rows.Count - 1
            If (dtPeriodicidad.Rows(i)("CodigoPeriodicidad") = strValor) Then
                Return CType(dtPeriodicidad.Rows(i)("DiasPeriodo"), Integer)
            End If
        Next
    End Function

#End Region

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim consecutivo As String = e.CommandArgument
        Dim i As Integer
        For i = 0 To dgLista.Rows.Count - 1
            If dgLista.Rows(i).Cells(2).Text = e.CommandArgument Then
                pnlCupon.Visible = True
                tbFechaInicio.Text = dgLista.Rows(i).Cells(3).Text
                tbFechaTermino.Text = dgLista.Rows(i).Cells(4).Text
                tbAmortizacion.Text = dgLista.Rows(i).Cells(5).Text
                tbDifDias.Text = dgLista.Rows(i).Cells(6).Text
                tbTasaCupon.Text = dgLista.Rows(i).Cells(7).Text
                tbBase.Text = dgLista.Rows(i).Cells(8).Text
                tbDiasPago.Text = dgLista.Rows(i).Cells(9).Text
                hdConsecutivo.Value = e.CommandArgument
                hdFechaIni.Value = tbFechaInicio.Text
                hdFechaFin.Value = tbFechaTermino.Text
                btnAgregar.Visible = True
                Exit For
            End If
        Next
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim dtAuxCuponEliminado As DataTable = CType(Session("cuponEspecial_Eliminados"), DataTable)
        Dim drAuxCuponEli As DataRow = dtAuxCuponEliminado.NewRow
        drAuxCuponEli(0) = tbCodigoNemonico.Text
        drAuxCuponEli(1) = e.CommandArgument
        dtAuxCuponEliminado.Rows.Add(drAuxCuponEli)
        Session("cuponEspecial_Eliminados") = dtAuxCuponEliminado
        Dim i As Integer
        Dim dtAux As DataTable = CType(Session("cuponeraEspecial"), DataTable)
        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("consecutivo") = e.CommandArgument Then
                dtAux.Rows.RemoveAt(i)
                Exit For
            End If
        Next
        dtAux = ActualizarAmortizacion_100(dtAux)
        dgLista.DataSource = dtAux
        dgLista.DataBind()
        Session("cuponeraEspecial") = dtAux
        ActualizaCuponeraEspecial()
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAgregar.Click
        Dim dtAux As DataTable = CType(Session("cuponeraEspecial"), DataTable)
        Dim i As Integer
        Dim decContador As Decimal = 0.0
        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("consecutivo") <> hdConsecutivo.Value Then
                decContador = decContador + Convert.ToDecimal(dtAux.Rows(i)("Amortizac"))
            End If
        Next
        If decContador + Convert.ToDecimal(tbAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)) > 100 Then
            AlertaJS(ObtenerMensaje("CONF33"))
            Exit Sub
        End If
        For i = 0 To dgLista.Rows.Count - 1
            If dgLista.Rows(i).Cells(2).Text = hdConsecutivo.Value Then
                dgLista.Rows(i).Cells(3).Text = tbFechaInicio.Text
                dgLista.Rows(i).Cells(4).Text = tbFechaTermino.Text
                dgLista.Rows(i).Cells(5).Text = Format(Convert.ToDecimal(tbAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.0000000")
                dgLista.Rows(i).Cells(6).Text = tbDifDias.Text
                dgLista.Rows(i).Cells(7).Text = Format(Convert.ToDecimal(tbTasaCupon.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.000000000000")
                dgLista.Rows(i).Cells(8).Text = tbBase.Text
                dgLista.Rows(i).Cells(9).Text = tbDiasPago.Text
                Exit For
            End If
        Next

        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("consecutivo") = hdConsecutivo.Value Then
                dtAux.Rows(i)("FechaIni") = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
                dtAux.Rows(i)("FechaFin") = UIUtility.ConvertirFechaaDecimal(tbFechaTermino.Text)
                dtAux.Rows(i)("Amortizac") = Format(Convert.ToDecimal(tbAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.0000000") 'tbAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)
                dtAux.Rows(i)("DifDias") = tbDifDias.Text.Replace(".", UIUtility.DecimalSeparator)
                dtAux.Rows(i)("TasaCupon") = Format(Convert.ToDecimal(tbTasaCupon.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.000000000000") 'tbTasaCupon.Text.Replace(".", UIUtility.DecimalSeparator)
                dtAux.Rows(i)("BaseCupon") = tbBase.Text.Replace(".", UIUtility.DecimalSeparator)
                dtAux.Rows(i)("DiasPago") = tbDiasPago.Text.Replace(".", UIUtility.DecimalSeparator)
                Exit For
            End If
        Next
        Session("cuponeraEspecial") = dtAux
        pnlCupon.Visible = False
        If hdFechaIni.Value <> tbFechaInicio.Text Then
            ActualizarFechasCuponeras("I", tbFechaInicio.Text)
        End If
        If hdFechaFin.Value <> tbFechaTermino.Text Then
            ActualizarFechasCuponeras("F", tbFechaTermino.Text)
        End If
        ActualizarAmortizacion()
        ActualizaCuponeraEspecial()
        LimpiarCampos()
        btnAgregar.Visible = False
    End Sub

    Private Sub ActualizarAmortizacion()
        Dim i As Integer
        Dim dtAux As DataTable = CType(Session("cuponeraEspecial"), DataTable)
        Dim amortSuma As Decimal = 0
        Dim amortActual As Decimal = 0
        Dim indiceActual As Integer = -1

        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("consecutivo") < hdConsecutivo.Value Then
                amortSuma = amortSuma + Convert.ToDecimal(dtAux.Rows(i)("Amortizac"))
            ElseIf dtAux.Rows(i)("consecutivo") = hdConsecutivo.Value Then
                amortActual = Convert.ToDecimal(dtAux.Rows(i)("Amortizac"))
                amortSuma = amortSuma + amortActual
                indiceActual = i
            End If
        Next
        If amortSuma < 100 Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") > hdConsecutivo.Value Then
                    If amortSuma + amortActual <= 100 Then
                        dtAux.Rows(i)("Amortizac") = Format(amortActual, "##0.0000000")
                        amortSuma = amortSuma + amortActual
                    Else
                        dtAux.Rows(i)("Amortizac") = Format(100 - amortSuma, "##0.0000000")
                    End If
                End If
            Next
        ElseIf amortSuma > 100 Then
            dtAux.Rows(indiceActual)("Amortizac") = Format(amortActual - amortSuma + 100, "##0.0000000")
        ElseIf amortSuma = 100 Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") > hdConsecutivo.Value Then
                    dtAux.Rows(i)("Amortizac") = Format(0, "##0.0000000")
                End If
            Next

        End If
        amortSuma = 0
        dtAux = ActualizarAmortizacion_100(dtAux)

        Session("cuponeraEspecial") = dtAux
        dgLista.DataSource = dtAux
        dgLista.DataBind()
    End Sub

    Private Function ActualizarAmortizacion_100(ByVal dtAux100 As DataTable) As DataTable
        Dim i As Integer
        Dim amortSuma As Decimal = 0

        For i = 0 To dtAux100.Rows.Count - 1
            amortSuma = amortSuma + dtAux100.Rows(i)("Amortizac")
        Next
        If amortSuma <> 100 Then
            dtAux100.Rows(dtAux100.Rows.Count - 1)("Amortizac") = Format(Convert.ToDecimal(dtAux100.Rows(dtAux100.Rows.Count - 1)("Amortizac")) + 100 - amortSuma, "##0.0000000")
        End If
        Return dtAux100
    End Function

    Private Sub ActualizarFechasCuponeras(ByVal flag As String, ByVal fecha As String)
        Dim i As Integer
        Dim difDias As Integer = 0
        Dim dtAux As DataTable = CType(Session("cuponeraEspecial"), DataTable)
        i = hdConsecutivo.Value - 1

        If flag = "I" Then  'INICIO
            While i > 0
                i = i - 1
                difDias = CType(dtAux.Rows(i)("DifDias"), Integer)
                dtAux.Rows(i)("FechaFin") = UIUtility.ConvertirFechaaDecimal(fecha)
                If i > 0 Then
                    dtAux.Rows(i)("FechaIni") = UIUtility.ConvertirFechaaDecimal(ObtieneFecha(fecha, difDias, "-"))
                End If
                fecha = UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaIni"))
            End While
        ElseIf flag = "F" Then  'FIN
            While i < dtAux.Rows.Count - 1
                i = i + 1
                difDias = CType(dtAux.Rows(i)("DifDias"), Integer)
                dtAux.Rows(i)("FechaIni") = UIUtility.ConvertirFechaaDecimal(fecha)
                If i < dtAux.Rows.Count - 1 Then
                    dtAux.Rows(i)("FechaFin") = UIUtility.ConvertirFechaaDecimal(ObtieneFecha(fecha, difDias, "+"))
                End If
                fecha = UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaFin"))
            End While
        End If
        Session("cuponeraEspecial") = dtAux
        dgLista.DataSource = dtAux
        dgLista.DataBind()
        'ActualizaCuponeraEspecial()
    End Sub

    Private Function ObtieneFecha(ByVal fecha As String, ByVal difDias As Integer, ByVal accion As String) As String
        Dim dtFecha As Date = CType(fecha, Date)
        Dim fechaNueva As String = ""
        If accion = "-" Then    'DISMINUYE FECHA
            fechaNueva = DateAdd(DateInterval.Day, -difDias, dtFecha).ToShortDateString
        ElseIf accion = "+" Then    'AUMENTA FECHA
            fechaNueva = DateAdd(DateInterval.Day, difDias, dtFecha).ToShortDateString
        End If
        Return fechaNueva
    End Function

    Private Sub LimpiarCampos()
        tbFechaInicio.Text = ""
        tbFechaTermino.Text = ""
        tbAmortizacion.Text = ""
        tbDifDias.Text = ""
        tbTasaCupon.Text = ""
        tbBase.Text = ""
        tbDiasPago.Text = ""
        hdConsecutivo.Value = ""
        tbTasaSpread.Text = ""
    End Sub
End Class
