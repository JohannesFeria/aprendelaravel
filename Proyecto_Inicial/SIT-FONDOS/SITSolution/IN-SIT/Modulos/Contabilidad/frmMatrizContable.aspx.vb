Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports UIUtility
Partial Class Modulos_Contabilidad_frmMatrizContable
    Inherits BasePage
#Region "Variables"
    Dim oCabeceraMatrizContableBM As New CabeceraMatrizContableBM
    Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
    Dim oMatrizContableBM As New MatrizContableBM
    Dim oMatrizContableBE As New MatrizContableBE
    Private dtTblCabecera As New DataTable
    Private dtTblDetalle As New DataTable
    Dim strCodigoMatriz As String
#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                InicializarCabecera()
                CargarCombos()
                If (TryCast(Request.QueryString("retornar"), String) = "1") Then
                    CargarFiltroBusquedaM()
                End If
                Session("FiltroBusquedaM") = Nothing
                Session("FilaSeleccionada") = -1
                Session("FilaSeleccionadaCabecera") = -1
                Session("GrillaCabeceraMatriz") = Nothing
            End If
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; al cargar la p&aacute;gina")
        End Try
    End Sub
    Private Sub ibtnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibtnBuscar.Click
        Try
            Me.dgCabecera.PageIndex = 0
            CargarGrilla()
            ViewState("EstadoDetalle") = "Seleccionar"
            ViewState("EstadoCabecera") = ""
            ViewState("CabeceraSeleccionada") = Nothing
            Session("FilaSeleccionada") = -1
            Session("FilaSeleccionadaCabecera") = -1
            Session("GrillaCabeceraMatriz") = Nothing
            hdCodigoDetalleMatriz.Value = ""
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Session("FiltroBusquedaM") = CrearFiltroBusquedaM()
            Response.Redirect("frmMatrizContableCabReg.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codCabecera As Int32 = e.CommandArgument
            oCabeceraMatrizContableBM.Eliminar(codCabecera, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub
    Protected Sub ibtnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnIngresar.Click
        Try
            Session("FiltroBusquedaM") = CrearFiltroBusquedaM()
            Response.Redirect("frmMatrizContableCabReg.aspx?")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
    Protected Sub ibtnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub
    Protected Sub ibtnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnImprimir.Click
        Try
            Dim fondo As String = ddlFondo.SelectedValue
            Dim matriz As String = IIf(ddlMatriz.SelectedIndex = 0, "0", ddlMatriz.SelectedValue)
            Dim codigoMoneda As String = IIf(ddlMoneda.SelectedIndex = 0, "", ddlMoneda.SelectedValue)
            Dim codigoClaseInstrumento As String = String.Empty
            Dim codigoOperacion As String = IIf(ddlOperacion.SelectedIndex = 0, "", ddlOperacion.SelectedValue)
            Dim codigoTipoInstrumento As String = IIf(ddlTipoInstrumento.SelectedIndex = 0, "", ddlTipoInstrumento.SelectedValue)
            Dim codigoModalidadPago As String = String.Empty
            Dim codigoSectorEmpresarial As String = String.Empty
            EjecutarJS(UIUtility.MostrarPopUp("frmVisorMatrizContable.aspx?pFondo=" + fondo + "&pMatriz=" + matriz + "&pCodigoMoneda=" + codigoMoneda + "&pCodigoOperacion=" + codigoOperacion + "&pCodigoTipoInstrumento=" + codigoTipoInstrumento + "&pCodigoClaseInstrumento=" + codigoClaseInstrumento + "&pCodigoModalidadPago=" + codigoModalidadPago + "&pCodigoSectorEmpresarial=" + codigoSectorEmpresarial, "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al enviar los datos al imprimir")
        End Try
    End Sub
    Protected Sub dgCabecera_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCabecera.PageIndexChanging
        Try
            dgCabecera.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
#End Region
#Region "Metodos Privados"
    Private Sub InicializarCabecera()
        Dim dt As DataTable
        dt = New DataTable
        dt.Columns.Add(New DataColumn("MatrizContable", GetType(String)))
        dt.Columns.Add(New DataColumn("CodigoPortafolio", GetType(String)))
        dt.Columns.Add(New DataColumn("Fondo", GetType(String)))
        dt.Columns.Add(New DataColumn("Moneda", GetType(String)))
        dt.Columns.Add(New DataColumn("Operacion", GetType(String)))
        dt.Columns.Add(New DataColumn("ClaseInstrumento", GetType(String)))
        dt.Columns.Add(New DataColumn("ModalidadPago", GetType(String)))
        dt.Columns.Add(New DataColumn("TipoInstrumento", GetType(String)))
        dt.Columns.Add(New DataColumn("SectorEmpresarial", GetType(String)))
        dt.Columns.Add(New DataColumn("NumeroCuentaIngreso", GetType(String)))
        dt.GetChanges()
        Me.dgCabecera.DataSource = dt
        Me.dgCabecera.DataBind()
    End Sub
    Private Sub CargarCombos()
        'Portafolio
        Dim tblPortafolio As New Data.DataTable
        Dim oPortafolio As New PortafolioBM
        tblPortafolio = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, tblPortafolio, "CodigoPortafolio", "Descripcion", True)
        'Operacion
        Dim tblOperacion As New Data.DataTable
        Dim oOperacion As New OperacionBM
        tblOperacion = oOperacion.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlOperacion, tblOperacion, "CodigoOperacion", "Descripcion", True)
        'TipoInstrumento
        Dim tblTipoInstrumento As New Data.DataTable
        Dim oTipoInstrumento As New TipoInstrumentoBM
        tblTipoInstrumento = oTipoInstrumento.Listar(DatosRequest).Tables(0)
        tblTipoInstrumento.Rows.Add("DIVISA", "DIVISA", "DIVISAS", "3", "", 0, "", "", "", "", "", 0, "", "", "", "", "DIVISA - DIVISA")
        HelpCombo.LlenarComboBox(Me.ddlTipoInstrumento, tblTipoInstrumento, "CodigoTipoInstrumento", "CodigoMasDescripcion", True)
        'Moneda
        Dim tblMoneda As New Data.DataTable
        Dim oMoneda As New MonedaBM
        tblMoneda = oMoneda.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMoneda, tblMoneda, "CodigoMoneda", "Descripcion", True)
        'Matriz
        Dim oMatrizContableBM As New MatrizContableBM
        Dim dtblDatos As DataTable = oMatrizContableBM.SeleccionarPorFiltros("", "A", "0").Tables(0)
        HelpCombo.LlenarComboBox(ddlMatriz, dtblDatos, "CodigoMatrizContable", "Descripcion", True)
    End Sub
    Private Sub CargarGrilla()
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = oCabeceraMatrizContableBE.CabeceraMatrizContable.NewCabeceraMatrizContableRow()
        oRow = CrearFiltroBusquedaM()
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AddCabeceraMatrizContableRow(oRow)
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AcceptChanges()
        InicializarCabecera()
        dtTblCabecera = oCabeceraMatrizContableBM.SeleccionarPorFiltro(oCabeceraMatrizContableBE, "0").Tables(0).Copy
        Session("GrillaCabeceraMatriz") = dtTblCabecera
        Me.dgCabecera.DataSource = dtTblCabecera
        Me.dgCabecera.DataBind()
        dgCabecera.SelectedIndex = -1
        ViewState("CabeceraSeleccionada") = Nothing
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtTblCabecera.Rows.Count) + "');")
    End Sub
    Private Function CrearFiltroBusquedaM() As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = oCabeceraMatrizContableBE.CabeceraMatrizContable.NewCabeceraMatrizContableRow()
        oRow.CodigoMoneda = ddlMoneda.SelectedValue
        oRow.CodigoClaseInstrumento = String.Empty
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoTipoInstrumento = ddlTipoInstrumento.SelectedValue
        oRow.CodigoModalidadPago = String.Empty
        oRow.CodigoSectorEmpresarial = String.Empty
        oRow.CodigoPortafolioSBS = If(Not ddlFondo.SelectedValue = "", ddlFondo.SelectedValue, "0")
        oRow.CodigoMatrizContable = ddlMatriz.SelectedValue
        oRow.NumeroCuentaIngreso = String.Empty
        oRow.CodigoSBSBanco = String.Empty
        oRow.Situacion = ddlSituacion.SelectedValue
        oRow.CodigoCabeceraMatriz = If(tbCodigoCabeceraM.Text.Trim().Equals(String.Empty), "0", tbCodigoCabeceraM.Text.Trim())
        oRow.CodigoNegocio = ""
        oRow.CodigoSerie = ""
        Return oRow
    End Function
    Private Sub CargarFiltroBusquedaM()
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = DirectCast(Session("FiltroBusquedaM"), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        SeleccionarOpcionCombo(Me.ddlFondo, oRow.CodigoPortafolioSBS)
        SeleccionarOpcionCombo(Me.ddlMatriz, oRow.CodigoMatrizContable)
        SeleccionarOpcionCombo(Me.ddlTipoInstrumento, oRow.CodigoTipoInstrumento)
        SeleccionarOpcionCombo(Me.ddlOperacion, oRow.CodigoOperacion)
        SeleccionarOpcionCombo(Me.ddlMoneda, oRow.CodigoMoneda)
        SeleccionarOpcionCombo(Me.ddlSituacion, oRow.Situacion)
        tbCodigoCabeceraM.Text = oRow.CodigoCabeceraMatriz
        CargarGrilla()
    End Sub
    Private Sub SeleccionarOpcionCombo(ByVal combo As Control, ByVal valorSeleccionar As Object)
        Select Case combo.GetType().ToString()
            Case "System.Web.UI.WebControls.DropDownList"
                Dim ddlCombo As DropDownList = (CType(combo, DropDownList))
                If Not ddlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)) Is Nothing Then
                    Dim indice As Integer = ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)))
                    ddlCombo.SelectedIndex = indice
                End If
                Exit Sub
            Case "System.Web.UI.HtmlControls.HtmlSelect"
                Dim htmlCombo As HtmlSelect = (CType(combo, HtmlSelect))
                If Not htmlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)) Is Nothing Then
                    Dim indice As Integer = htmlCombo.Items.IndexOf(htmlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)))
                    htmlCombo.SelectedIndex = indice
                End If
                Exit Sub
            Case Else
                Return
        End Select
    End Sub
#End Region
End Class