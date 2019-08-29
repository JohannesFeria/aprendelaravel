Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.IO
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Contabilidad_frmCierreyPreCierreContable
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPortafolio()
            lblFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
            Dim fecha As String = IIf(Me.lblFechaOperacion.Text.Length = 0, Date.Today.ToShortDateString, lblFechaOperacion.Text)
            ObtenerFechaActual(fecha)
            BuscarRuta()
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub btnCierre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCierre.Click
        Try
            If (validarFechaAperturaContable(VALIDAFERIADO)) Then
                AlertaJS("La Fecha de apertura no puede ser Sabado, Domingo o Feriado.")
                Exit Sub
            End If
            Dim objBM As New CuentasPorCobrarBM
            Dim DecFechaProceso As Decimal = CType(Date.Parse(lblFechaOperacion.Text).ToString("yyyyMMdd"), Decimal)
            Dim lote As String = objBM.ValidaLotesCuadradosParaCierre(DecFechaProceso, ddlPortafolio.SelectedValue, DatosRequest)
            If Not lote.Equals("") Then
                AlertaJS("El lote de \'" & lote & "\' debe estar cuadrado para proceder con el cierre.")
                Exit Sub
            End If
            Dim oreporte As DataSet = objBM.OperacionesNoContabilizadas(DecFechaProceso, ddlPortafolio.SelectedValue, "", DatosRequest)
            If oreporte.Tables(0).Rows.Count > 0 Then
                AlertaJS("No es posible Cerrar porque existen documentos sin Contabilizar.")

                Session("ReporteContabilidad_Fondo") = ddlPortafolio.SelectedValue
                EjecutarJS(UIUtility.MostrarPopUp("frmVisorOperacionesNoContabilizadas.aspx?vfechaproceso=" + lblFechaOperacion.Text.Trim() + "&vfondo=" + ddlPortafolio.SelectedValue + "&vegreso=" + "&vdescripcionFondo=" + ddlPortafolio.SelectedItem.Text, "no", 800, 600, 40, 150, "no", "yes", "yes", "yes"), False)
                Exit Sub
            End If
            Dim oPortafolio As New PortafolioBM
            oPortafolio.ModificarCierreContable(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), DatosRequest)
            AlertaJS("El cierre se realizó satisfactoriamente.")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        lblFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
        Dim fecha As String = IIf(Me.lblFechaOperacion.Text.Length = 0, Date.Today.ToShortDateString, lblFechaOperacion.Text)
        Me.ObtenerFechaActual(fecha)
        Me.lblError.Text = ""
    End Sub
    Private Function validarFechaAperturaContable(Optional ByVal sInd As String = "") As Boolean
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        Dim EsFeriado As Boolean
        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) <= UIUtility.ConvertirFechaaDecimal(lblFechaOperacion.Text) Then
            Return True
        End If
        fechaAnterior = Convert.ToDateTime(Me.tbFechaOperacion.Text)
        fechaNueva = fechaAnterior
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        'If fechaNueva.DayOfWeek = DayOfWeek.Sunday Then
        '    Return True
        'End If

        'If fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
        '    Return True
        'Else
        EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, sInd)
        If EsFeriado = True Then
            Return True
        End If
        'End If
        Return False
    End Function
    Private Sub ObtenerFechaActual(ByVal fechaAnt As String)
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        'Dim EsFeriado As Boolean
        fechaAnterior = Convert.ToDateTime(fechaAnt)
        fechaNueva = fechaAnterior.AddDays(1)
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        'If fechaNueva.DayOfWeek = DayOfWeek.Sunday Then
        '    fechaNueva = fechaNueva.AddDays(1)
        'End If
        'If fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
        '    While fechaNueva.DayOfWeek = DayOfWeek.Saturday
        '        fechaNueva = fechaNueva.AddDays(2)
        '        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        '        While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
        '            fechaNueva = fechaNueva.AddDays(1)
        '            Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        '        End While
        '    End While
        'Else
        '    EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
        '    If EsFeriado = True Then
        '        While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
        '            fechaNueva = fechaNueva.AddDays(1)
        '            Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        '            While fechaNueva.DayOfWeek = DayOfWeek.Saturday
        '                fechaNueva = fechaNueva.AddDays(2)
        '                Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        '            End While
        '        End While
        '    End If
        'End If
        Me.tbFechaOperacion.Text = fechaNueva.ToShortDateString
    End Sub
    Private Sub BuscarRuta()
        Dim dtConsulta As DataTable
        dtConsulta = New ParametrosGeneralesBM().SeleccionarPorFiltro("RISTRACONT", "", "", "", DatosRequest)
        If dtConsulta.Rows.Count > 0 Then
            tbRuta.Text = CType(dtConsulta.Rows(0).Item("Valor"), String)
        End If
    End Sub
End Class