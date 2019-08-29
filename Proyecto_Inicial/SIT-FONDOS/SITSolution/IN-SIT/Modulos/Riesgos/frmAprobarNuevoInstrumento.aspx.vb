Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Riesgos_frmAprobarNuevoInstrumento
    Inherits BasePage
    Dim oValoresBM As New ValoresBM
    Dim strCodigoSBS As String
    Dim strCodigoIsin As String
    Dim strCodigoMnemonico As String
    Dim strTipoRenta As String
    Dim strMoneda As String
    Dim strEmisor As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            HabilitaBotones(False)
            If Not Page.IsPostBack Then
                CargarCombos()

                If Not Request.QueryString("vCSbs") Is Nothing Then
                    strCodigoSBS = Request.QueryString("vCSbs")
                    Me.tbCodigoSBS.Text = strCodigoSBS
                Else
                    strCodigoSBS = ""
                End If
                If Not Request.QueryString("vCIsin") Is Nothing Then
                    strCodigoIsin = Request.QueryString("vCIsin")
                    Me.tbCodigoIsin.Text = strCodigoIsin
                Else
                    strCodigoIsin = ""
                End If
                If Not Request.QueryString("vNemo") Is Nothing Then
                    strCodigoMnemonico = Request.QueryString("vNemo")
                    Me.tbMnemonico.Text = strCodigoMnemonico
                Else
                    strCodigoMnemonico = ""
                End If
                If Not Request.QueryString("vTR") Is Nothing Then
                    strTipoRenta = Request.QueryString("vTR")
                    Me.ddlTipoRenta.SelectedValue = IIf(strTipoRenta = "", "Todos", strTipoRenta)
                Else
                    strTipoRenta = ""
                End If
                If Not Request.QueryString("vMon") Is Nothing Then
                    strMoneda = Request.QueryString("vMon")
                    Me.ddlMoneda.SelectedValue = IIf(strMoneda = "", "Todos", strMoneda)
                Else
                    strMoneda = ""
                End If
                If Not Request.QueryString("vEmi") Is Nothing Then
                    strEmisor = Request.QueryString("vEmi")
                    Me.tbEmisor.Text = strEmisor
                Else
                    strEmisor = ""
                End If
                CargarGrillaPA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)

                If Not Request.QueryString("vCSbs2") Is Nothing Then
                    strCodigoSBS = Request.QueryString("vCSbs2")
                    Me.tbCodigoSBS2.Text = strCodigoSBS
                Else
                    strCodigoSBS = ""
                End If
                If Not Request.QueryString("vCIsin2") Is Nothing Then
                    strCodigoIsin = Request.QueryString("vCIsin2")
                    Me.tbCodigoIsin2.Text = strCodigoIsin
                Else
                    strCodigoIsin = ""
                End If
                If Not Request.QueryString("vNemo2") Is Nothing Then
                    strCodigoMnemonico = Request.QueryString("vNemo2")
                    Me.tbMnemonico2.Text = strCodigoMnemonico
                Else
                    strCodigoMnemonico = ""
                End If
                If Not Request.QueryString("vTR2") Is Nothing Then
                    strTipoRenta = Request.QueryString("vTR2")
                    Me.ddlTipoRenta2.SelectedValue = IIf(strTipoRenta = "", "Todos", strTipoRenta)
                Else
                    strTipoRenta = ""
                End If
                If Not Request.QueryString("vMon2") Is Nothing Then
                    strMoneda = Request.QueryString("vMon2")
                    Me.ddlMoneda2.SelectedValue = IIf(strMoneda = "", "Todos", strMoneda)
                Else
                    strMoneda = ""
                End If
                If Not Request.QueryString("vEmi2") Is Nothing Then
                    strEmisor = Request.QueryString("vEmi2")
                    Me.tbEmisor2.Text = strEmisor
                Else
                    strEmisor = ""
                End If
                CargarGrillaIA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
                If hdTipoBusqueda.Value = 1 Then
                    tbEmisor.Text = datosModal(0).ToString()
                ElseIf hdTipoBusqueda.Value = 2 Then
                    tbEmisor2.Text = datosModal(0).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            ContadorInicial()
            If txtCodPA.Text.ToString <> "" Then
                Ir(txtCodPA.Text, "A")
            Else
                AlertaJS("Debe seleccionar un Registro")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aprobar")
        End Try
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Try
            If txtCodIA.Text.ToString <> "" Then
                Ir(txtCodIA.Text, "C")
            Else
                AlertaJS("Debe seleccionar un Registro")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar")
        End Try
    End Sub

    Protected Sub dgListaIA_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaIA.PageIndexChanging
        Try
            dgListaIA.PageIndex = e.NewPageIndex
            strCodigoSBS = Me.tbCodigoSBS2.Text.ToString
            strCodigoIsin = Me.tbCodigoIsin2.Text.ToString
            strCodigoMnemonico = Me.tbMnemonico2.Text.ToString
            strMoneda = Me.ddlMoneda2.SelectedValue
            strTipoRenta = Me.ddlTipoRenta2.SelectedValue.ToString
            strEmisor = Me.tbEmisor2.Text
            CargarGrillaIA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Protected Sub dgListaIA_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaIA.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then                
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                dgListaIA.SelectedIndex = Row.RowIndex
                dgListaPA.SelectedIndex = -1
                Dim i As Integer = Row.RowIndex
                'Me.txtCodIA.Text = dgListaIA.Rows(i).Cells(2).Text
                Me.txtCodIA.Text = dgListaIA.Rows(i).Cells(3).Text
                Me.btnAprobar.Attributes.Remove("onClick")
                Me.btnDesaprobar.Attributes.Add("onClick", "javascript:return confirm('¿Desea desaprobar el alta del instrumento?');")
                HabilitaBotones(True)
            ElseIf e.CommandName = "DetalleIA" Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim codigoNemonico As String = String.Empty
                codigoNemonico = Row.Cells(3).Text
                EjecutarJS("window.showModalDialog('../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" & codigoNemonico & "&accionValor=Consulta_Aprobacion_Instrumento" & "','1028','780','');")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Página")
        End Try
    End Sub

    Protected Sub dgListaPA_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaPA.PageIndexChanging
        Try
            dgListaPA.PageIndex = e.NewPageIndex
            strCodigoSBS = Me.tbCodigoSBS.Text.ToString
            strCodigoIsin = Me.tbCodigoIsin.Text.ToString
            strCodigoMnemonico = Me.tbMnemonico.Text.ToString
            strTipoRenta = Me.ddlTipoRenta.SelectedValue.ToString
            strMoneda = Me.ddlMoneda.SelectedValue
            strEmisor = Me.tbEmisor.Text
            CargarGrillaPA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

    Protected Sub dgListaPA_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaPA.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                dgListaPA.SelectedIndex = Row.RowIndex
                dgListaIA.SelectedIndex = -1
                Dim i As Integer = Row.RowIndex
                'Me.txtCodPA.Text = dgListaPA.Rows(i).Cells(2).Text
                Me.txtCodPA.Text = dgListaPA.Rows(i).Cells(3).Text
                Me.btnAprobar.Attributes.Add("onClick", "javascript:return confirm('¿Desea aprobar el alta del instrumento?');")
                Me.btnDesaprobar.Attributes.Remove("onClick")
                HabilitaBotones(False)
            ElseIf e.CommandName = "DetallePA" Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim codigoNemonico As String = String.Empty
                codigoNemonico = Row.Cells(3).Text
                EjecutarJS("window.showModalDialog('../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" & codigoNemonico & "&accionValor=Consulta_Aprobacion_Instrumento" & "','1028','780','');")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            strCodigoSBS = Me.tbCodigoSBS.Text.ToString
            strCodigoIsin = Me.tbCodigoIsin.Text.ToString
            strCodigoMnemonico = Me.tbMnemonico.Text.ToString
            strTipoRenta = Me.ddlTipoRenta.SelectedValue.ToString
            strMoneda = Me.ddlMoneda.SelectedValue
            strEmisor = Me.tbEmisor.Text
            CargarGrillaPA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnBuscar2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar2.Click
        Try
            strCodigoSBS = Me.tbCodigoSBS2.Text.ToString
            strCodigoIsin = Me.tbCodigoIsin2.Text.ToString
            strCodigoMnemonico = Me.tbMnemonico2.Text.ToString
            strMoneda = Me.ddlMoneda2.SelectedValue
            strTipoRenta = Me.ddlTipoRenta2.SelectedValue.ToString
            strEmisor = Me.tbEmisor2.Text
            CargarGrillaIA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnDesaprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesaprobar.Click
        Try
            Dim dt As DataTable
            If txtCodIA.Text.ToString <> "" Then
                dt = oValoresBM.Aprobacion_InstrumentosRiesgo(Me.txtCodIA.Text.ToString, 0, "", "", DatosRequest).Tables(0)
                EnviarAlertaPorNuevoIntrumento(dt)
                strCodigoSBS = Me.tbCodigoSBS.Text.ToString
                strCodigoIsin = Me.tbCodigoIsin.Text.ToString
                strCodigoMnemonico = Me.tbMnemonico.Text.ToString
                strMoneda = Me.ddlMoneda.SelectedValue
                strTipoRenta = Me.ddlTipoRenta.SelectedValue.ToString
                strEmisor = Me.tbEmisor.Text
                CargarGrillaPA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)

                strCodigoSBS = Me.tbCodigoSBS2.Text.ToString
                strCodigoIsin = Me.tbCodigoIsin2.Text.ToString
                strCodigoMnemonico = Me.tbMnemonico2.Text.ToString
                strMoneda = Me.ddlMoneda2.SelectedValue
                strTipoRenta = Me.ddlTipoRenta2.SelectedValue.ToString
                strEmisor = Me.tbEmisor2.Text
                CargarGrillaIA(strCodigoSBS, strCodigoIsin, strCodigoMnemonico, strMoneda, strTipoRenta, strEmisor)
            Else
                AlertaJS("Debe seleccionar un Registro")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Desaprobar")
        End Try        
    End Sub

    Public Sub ContadorInicial()
        If dgListaPA.Rows.Count = 0 Then            
            AlertaJS("No existen Registros para mostrar")
            Exit Sub
        End If
    End Sub

    Public Sub HabilitaBotones(ByVal estado As Boolean)
        btnAprobar.Visible = Not estado
        btnDesaprobar.Visible = estado
        btnConsultar.Visible = estado
    End Sub

    Public Sub CargarCombos()
        Dim DtTablaTipoRenta As New DataTable
        Dim DtTablaMoneda As DataTable
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oMonedaBM As New MonedaBM
        DtTablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        DtTablaMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlMoneda, DtTablaMoneda, "CodigoMoneda", "CodigoMoneda", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta2, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlMoneda2, DtTablaMoneda, "CodigoMoneda", "CodigoMoneda", True)
    End Sub

    Private Sub CargarGrillaPA(ByVal p_CSbs As String, ByVal p_CIsin As String, ByVal p_CMnemo As String, ByVal p_Mon As String, ByVal p_TipRen As String, ByVal p_Emi As String)
        If p_Mon = "Todos" Then p_Mon = ""
        If p_TipRen = "Todos" Then p_TipRen = ""

        'Me.dgListaPA.PageIndex = 0
        Dim dtblDatos As DataTable = oValoresBM.SeleccionarPorFiltro3(p_CSbs, p_CIsin, p_CMnemo, p_Mon, p_TipRen, p_Emi, "", DatosRequest).Tables(0)
        Me.dgListaPA.DataSource = dtblDatos
        Me.dgListaPA.DataBind()

        If dgListaPA.Rows.Count = 0 Then            
            AlertaJS("No se encontraron registros")
        End If
    End Sub

    Private Sub CargarGrillaIA(ByVal p_CSbs As String, ByVal p_CIsin As String, ByVal p_CMnemo As String, ByVal p_Mon As String, ByVal p_TipRen As String, ByVal p_Emi As String)
        If p_Mon = "Todos" Then p_Mon = ""
        If p_TipRen = "Todos" Then p_TipRen = ""

        'Me.dgListaIA.PageIndex = 0
        Dim dtblDatos As DataTable = oValoresBM.SeleccionarPorFiltro3(p_CSbs, p_CIsin, p_CMnemo, p_Mon, p_TipRen, p_Emi, "A", DatosRequest).Tables(0)
        Me.dgListaIA.DataSource = dtblDatos
        Me.dgListaIA.DataBind()

        If dgListaIA.Rows.Count = 0 Then            
            AlertaJS("No se encontraron registros")
        End If
    End Sub

    Private Sub Ir(ByVal pCodigo As String, ByVal pOper As String)
        Dim StrURL As String
        StrURL = "frmAprobacionAltaInstrumento.aspx?Cod=" & pCodigo & "&Op=" & pOper

        EjecutarJS("showWindow('" & StrURL & "','1200', '600')")
        'ShowDialogPopup(StrURL)
    End Sub

    Private Sub ShowDialogPopup(ByVal StrURL As String)
        Dim strArg As String = ""
        strCodigoSBS = Me.tbCodigoSBS.Text.ToString
        strCodigoIsin = Me.tbCodigoIsin.Text.ToString
        strCodigoMnemonico = Me.tbMnemonico.Text.ToString
        strTipoRenta = Me.ddlTipoRenta.SelectedValue.ToString
        strMoneda = Me.ddlMoneda.SelectedValue
        strEmisor = Me.tbEmisor.Text
        If strMoneda = "Todos" Then strMoneda = ""
        If strTipoRenta = "Todos" Then strTipoRenta = ""

        strArg = strCodigoSBS + "','" + strCodigoIsin + "','" + strCodigoMnemonico + "','" + strTipoRenta + "','" + strMoneda + "','" + strEmisor

        strCodigoSBS = Me.tbCodigoSBS2.Text.ToString
        strCodigoIsin = Me.tbCodigoIsin2.Text.ToString
        strCodigoMnemonico = Me.tbMnemonico2.Text.ToString
        strTipoRenta = Me.ddlTipoRenta2.SelectedValue.ToString
        strMoneda = Me.ddlMoneda2.SelectedValue
        strEmisor = Me.tbEmisor2.Text
        If strMoneda = "Todos" Then strMoneda = ""
        If strTipoRenta = "Todos" Then strTipoRenta = ""

        strArg = strArg + "','" + strCodigoSBS + "','" + strCodigoIsin + "','" + strCodigoMnemonico + "','" + strTipoRenta + "','" + strMoneda + "','" + strEmisor

        EjecutarJS("showWindow('" & StrURL & "', '1200', '600', '');")

    End Sub

    Private Sub EnviarAlertaPorNuevoIntrumento(ByVal dt As DataTable)
        Dim destinatarios As String = ""

        Dim dte As DataTable
        dte = New ParametrosGeneralesBM().Listar(CORREOS_ALTA_INSTRUMENTOS, DatosRequest)
        For Each fila As DataRow In dte.Rows
            destinatarios = destinatarios + fila("Valor") + ";"
        Next

        Dim asunto As String = "SIT - DESAPROBACIÓN DE ALTA DE INSTRUMENTO: " & dt.Rows(0)("CodigoNemonico")

        Dim mensaje As String = "Se ha desaprobado el alta del nuevo instrumento registrado en el SIT.<br/>" & _
            "<br/>Código Nemónico: " & dt.Rows(0)("CodigoNemonico") & _
            "<br/>Tipo de Instrumento: " & dt.Rows(0)("CodigoTipoInstrumentoSBS") & " - " & dt.Rows(0)("DescTipoInstrumento") & _
            "<br/>Nombre del Instrumento: " & dt.Rows(0)("DescInstrumento") & _
            "<br/>Fecha de desaprobación del instrumento: " & Date.Now.ToString("dd/MM/yyyy") & _
            "<br/>Nombre emisor: " & dt.Rows(0)("DescEmisor") & _
            "<br/>Código ISIN: " & dt.Rows(0)("CodigoISIN") & _
            "<br/>Código SBS: " & dt.Rows(0)("CodigoSBS")

        Dim ret As Boolean = UIUtility.EnviarMail(destinatarios, "", asunto, mensaje, DatosRequest)
    End Sub

End Class
