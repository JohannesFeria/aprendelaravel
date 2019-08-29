Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices.Marshal

Partial Class Modulos_Riesgos_frmExcepcionLimiteNegociacion
    Inherits BasePage

#Region " /* Eventos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            GridView1.PageIndex = e.NewPageIndex
            CargarGrilla("x")
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub ddlExcepcionN_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Try
            Dim ddlExcepcion As DropDownList = CType(sender, DropDownList)
            Dim gvr As GridViewRow = CType(CType(ddlExcepcion, DropDownList).NamingContainer, GridViewRow)
            Dim txtCantidad As TextBox = CType(gvr.FindControl("tbCantidadN"), TextBox)
            'txtCantidad.Attributes.Add("onkeypress", "IngresoNumeroDecimales();")
            If ddlExcepcion.SelectedValue <> "" Then
                txtCantidad.Enabled = True
            Else
                txtCantidad.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar la opción")
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlExcepcionN As DropDownList
                Dim lbExcepcionN As Label
                Dim tbCantidadN As TextBox

                lbExcepcionN = CType(e.Row.FindControl("lbExcepcionN"), Label)
                ddlExcepcionN = CType(e.Row.FindControl("ddlExcepcionN"), DropDownList)
                HelpCombo.LlenarComboBox(ddlExcepcionN, CType(Session("dtExclusionN"), DataTable), "Valor", "Nombre", True)
                ddlExcepcionN.SelectedValue = lbExcepcionN.Text
                ddlExcepcionN.AutoPostBack = True
                tbCantidadN = CType(e.Row.FindControl("tbCantidadN"), TextBox)
                'ddlExcepcionN.Attributes.Add("onchange", "javascript:HabilitaCampoCantidad(this);")

                tbCantidadN.Enabled = True
                If ddlExcepcionN.SelectedValue = "" Then
                    tbCantidadN.Enabled = False
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim strTipoRenta As String = ddlTipoRenta.SelectedValue
            Dim strPortafolio As String = ddlPortafolio.SelectedValue
            Dim decFechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            Dim decFechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            Dim strTipoOperacion As String = ddlTipoOperacion.SelectedValue
            Dim strCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue
            Dim strExclusion As String = ddlExclusion.SelectedValue

            ViewState("strTipoRenta") = strTipoRenta
            ViewState("strPortafolio") = strPortafolio
            ViewState("decFechaInicio") = decFechaInicio
            ViewState("decFechaFin") = decFechaFin
            ViewState("strTipoOperacion") = strTipoOperacion
            ViewState("strTipoInstrumento") = strCodigoTipoInstrumentoSBS
            ViewState("strExclusion") = strExclusion
            CargarGrilla(strTipoRenta, strPortafolio, decFechaInicio, decFechaFin, strTipoOperacion, strCodigoTipoInstrumentoSBS, strExclusion)
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
            Dim chkSelect As CheckBox
            Dim ddlExcepcionN As DropDownList
            Dim lbCodigoOrden As Label
            Dim tbCantidadN As TextBox
            Dim decCantidad As Decimal
            Dim decCantidadOrden As Decimal
            Dim bInd As Boolean = False

            Dim count As Decimal = 0
            For Each fila As GridViewRow In GridView1.Rows
                If fila.RowType = ListItemType.Item Or fila.RowType = ListItemType.AlternatingItem Then
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)

                    If chkSelect.Checked = True Then
                        ddlExcepcionN = CType(fila.FindControl("ddlExcepcionN"), DropDownList)
                        lbCodigoOrden = CType(fila.FindControl("lbCodigoOrden"), Label)
                        tbCantidadN = CType(fila.FindControl("tbCantidadN"), TextBox)
                        decCantidad = Val(tbCantidadN.Text.Trim)
                        decCantidadOrden = Val(fila.Cells(7).Text.ToString)

                        If decCantidadOrden < decCantidad Then
                            bInd = True
                            Exit For
                        End If
                        oOrdenPreOrdenInversionBM.ExcepcionLimiteNegociacion(fila.Cells(1).Text.Trim.ToString, ddlExcepcionN.SelectedValue, decCantidad, DatosRequest)
                        count = count + 1
                    End If
                End If
            Next
            If bInd = True Then
                AlertaJS("La cantidad de excepción debe ser menor o igual a la cantidad negociada")
            ElseIf count = 0 Then
                AlertaJS("Seleccione el registro")
            Else
                AlertaJS("La selección se regristró con éxito")
            End If

        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

#End Region

#Region " /* Métodos Personalizados */ "

    Private Sub CargarGrilla(ByVal strTipoRenta As String, _
        Optional ByVal strPortafolio As String = "", _
        Optional ByVal decFechaInicio As Decimal = 0, _
        Optional ByVal decFechaFin As Decimal = 0, _
        Optional ByVal strTipoOperacion As String = "", _
        Optional ByVal strCodigoTipoInstrumentoSBS As String = "", _
        Optional ByVal strExclusion As String = "")
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim ds As DataSet
        ds = oOrdenPreOrdenInversionBM.SeleccionarOrdenExcepPorFiltro(strTipoRenta, strPortafolio, decFechaInicio, decFechaFin, strTipoOperacion, strCodigoTipoInstrumentoSBS, strExclusion, DatosRequest)
        GridView1.DataSource = ds
        GridView1.DataBind()
    End Sub

    Private Sub CargarPagina()
        Dim oUtil As New UtilDM
        CargarCombos()
        ViewState("strTipoRenta") = ddlTipoRenta.SelectedValue
        ViewState("strPortafolio") = ddlPortafolio.SelectedValue
        ViewState("decFechaInicio") = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        ViewState("decFechaFin") = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
        ViewState("strTipoOperacion") = ddlTipoOperacion.SelectedValue
        ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
        ViewState("strExclusion") = ddlExclusion.SelectedValue
        Me.tbFechaInicio.Text = oUtil.RetornarFechaSistema
        Me.tbFechaFin.Text = oUtil.RetornarFechaSistema
        CargarGrilla("x")
    End Sub

    Private Sub CargarCombos()
        CargarTipoRenta()
        CargarPortafolio()
        CargarTipoOperacion()
        CargarTipoInstrumento("")
        CargaExclusion()
    End Sub

    Private Sub CargarTipoRenta()
        Dim oTipoRentaBM As New TipoRentaBM
        Dim dt As New DataTable
        dt = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoRenta, dt, "CodigoRenta", "Descripcion", True)
        ddlTipoRenta.SelectedIndex = 0
    End Sub

    Private Sub CargarPortafolio()
        Dim oPortafolioBM As New PortafolioBM
        Dim dt As New DataTable
        'dt = oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0)
        dt = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True)
        ddlPortafolio.SelectedIndex = 0
    End Sub

    Private Sub CargarTipoOperacion()
        Dim oOperacionBM As New OperacionBM
        Dim dt As New DataTable
        dt = oOperacionBM.Listar_ClaseInstrumento(datosrequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoOperacion, dt, "CodigoOperacion", "Descripcion", True)
        ddlTipoOperacion.SelectedIndex = 0
    End Sub

    Private Sub CargarTipoInstrumento(ByVal codigoClaseInstrumento As String)
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim dt As New DataTable
        dt = oTipoInstrumentoBM.SeleccionarPorFiltro(codigoClaseInstrumento, "", ParametrosSIT.TR_RENTA_VARIABLE, "A", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        ddlTipoInstrumento.SelectedIndex = 0
    End Sub

    Private Sub CargaExclusion()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGeneralesBM.Listar(ParametrosSIT.EXCEP_LIMITE_NEGOCIA, datosrequest)
        HelpCombo.LlenarComboBox(Me.ddlExclusion, dt, "Valor", "Nombre", True)
        Session("dtExclusionN") = dt
        ddlExclusion.SelectedIndex = 0
    End Sub

#End Region

    'Protected Sub chk_Item_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Item.CheckedChanged

    '    For Each fila As GridViewRow In GridView1.Rows
    '        If fila.RowType = ListItemType.Item Or fila.RowType = ListItemType.AlternatingItem Then
    '            CType(fila.FindControl("chkSelect"), CheckBox).Checked = True
    '        End If
    '    Next
    'End Sub
End Class