Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmLimiteTrading
    Inherits BasePage
    Private strOperacion As String
    Private decCodigoTrading As Decimal
    Private oLimiteTradingBM As New LimiteTradingBM
    Private oLimiteTradingBE As New LimiteTradingBE

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            strOperacion = Request.QueryString("ope")
            If strOperacion = "mod" Then
                decCodigoTrading = CType(Request.QueryString("cod"), Decimal)
            End If
            If Not Page.IsPostBack Then
                CargarPagina()
                If strOperacion = "mod" Then
                    CargarRegistro(decCodigoTrading)
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaLimiteTrading.aspx?CodigoRenta=" + ViewState("CodigoRenta") + "&CodigoGrupLimTrader=" + ViewState("CodigoGrupLimTrader") + "&TipoCargo=" + ViewState("TipoCargo") + "&Portafolio=" + ViewState("Portafolio"))
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim resultado As Boolean
            Dim oRow As LimiteTradingBE.LimiteTradingRow
            oRow = CType(oLimiteTradingBE.LimiteTrading.NewRow(), LimiteTradingBE.LimiteTradingRow)
            oLimiteTradingBM.InicializarLimiteTrading(oRow)
            oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
            oRow.CodigoGrupLimTrader = ddlGrupoLimite.SelectedValue
            oRow.TipoCargo = ddlTipoCargo.SelectedValue
            oRow.Porcentaje = CType(tbPorcentaje.Text, Decimal)
            oRow.Situacion = ESTADO_ACTIVO
            If strOperacion = "reg" Then
                oLimiteTradingBE.LimiteTrading.AddLimiteTradingRow(oRow)
                oLimiteTradingBE.AcceptChanges()
                resultado = oLimiteTradingBM.Insertar(oLimiteTradingBE, DatosRequest)
                If resultado = True Then
                    Me.AlertaJS("El registro se guardo satisfactoriamente! ")
                    LimpiarCampos()
                Else
                    Me.AlertaJS("El registro ingresado ya existe! ")
                End If
            Else
                If strOperacion = "mod" Then
                    oRow.CodigoTrading = CType(Request.QueryString("cod"), Decimal)
                    oLimiteTradingBE.LimiteTrading.AddLimiteTradingRow(oRow)
                    oLimiteTradingBE.AcceptChanges()
                    resultado = oLimiteTradingBM.Modificar(oLimiteTradingBE, DatosRequest)
                    If resultado = True Then
                        Me.AlertaJS("El registro se actualizado satisfactoriamente! ")
                    Else
                        Me.AlertaJS("Ingrese correctamente el registro!  ")
                    End If

                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub

    Private Sub ddlTipoRenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoRenta.SelectedIndexChanged
        Try
            ddlGrupoLimite.Items.Clear()
            CargarGrupoLimite(ddlTipoRenta.SelectedValue)
            If ddlTipoRenta.SelectedValue = "" Then
                ddlGrupoLimite.Enabled = False
                ddlGrupoLimite.SelectedIndex = 0
            Else
                ddlGrupoLimite.Enabled = True
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

    Private Sub CargarPagina()
        CargarCombos()
        ViewState("CodigoRenta") = IIf(Request.QueryString("CodigoRenta") Is Nothing, DDL_ITEM_SELECCIONE, Request.QueryString("CodigoRenta"))    'HDG OT 64291 20111202
        ViewState("TipoCargo") = IIf(Request.QueryString("TipoCargo") Is Nothing, "", Request.QueryString("TipoCargo"))
        ViewState("CodigoGrupLimTrader") = IIf(Request.QueryString("CodigoGrupLimTrader") Is Nothing, "", Request.QueryString("CodigoGrupLimTrader"))   'HDG OT 64291 20111202
        ViewState("Portafolio") = IIf(Request.QueryString("Portafolio") Is Nothing, "", Request.QueryString("Portafolio"))
    End Sub

    Private Sub CargarCombos()
        CargarPortafolioSBS()
        CargarGrupoLimite("")
        CargarTipoRenta()
        CargarTipoCargo()
    End Sub

    Private Sub CargarPortafolioSBS()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True)
        ddlPortafolio.SelectedIndex = 0
    End Sub

    Private Sub CargarGrupoLimite(ByVal TipoRenta As String)
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim dtGrupoLimite As New DataTable
        dtGrupoLimite = oGrupoLimiteTraderBM.ListarGrupoLimite(TipoRenta).Tables(0)
        HelpCombo.LlenarComboBox(ddlGrupoLimite, dtGrupoLimite, "CodigoGrupLimTrader", "Nombre", True)
        ddlGrupoLimite.SelectedIndex = 0
        ddlGrupoLimite.AutoPostBack = True
    End Sub

    Private Sub CargarTipoRenta()
        Dim oTipoRentaBM As New TipoRentaBM
        Dim dtTipoRenta As New DataTable
        dtTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, dtTipoRenta, "CodigoRenta", "Descripcion", False)
        ddlTipoRenta.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, DDL_ITEM_SELECCIONE))
        ddlTipoRenta.SelectedValue = TR_DERIVADOS
        ddlTipoRenta.Items.RemoveAt(ddlTipoRenta.SelectedIndex)
        ddlTipoRenta.SelectedIndex = 0

        ddlTipoRenta.AutoPostBack = True
    End Sub

    Private Sub CargarTipoCargo()
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim dtTipoCargo As New DataTable
        dtTipoCargo = oRolAprobadoresTraderBM.SeleccionarPorFiltro("", ESTADO_ACTIVO).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoCargo, dtTipoCargo, "TipoCargo", "Descripcion", True)
        ddlTipoCargo.SelectedIndex = 0
    End Sub

    Private Sub CargarRegistro(ByVal codigoTrading As Decimal)
        Dim oRow As LimiteTradingBE.LimiteTradingRow

        oLimiteTradingBE = oLimiteTradingBM.Seleccionar(codigoTrading, DatosRequest)
        oRow = CType(oLimiteTradingBE.Tables(0).Rows(0), LimiteTradingBE.LimiteTradingRow)

        ddlPortafolio.SelectedValue = oRow.CodigoPortafolioSBS
        ddlTipoCargo.SelectedValue = oRow.TipoCargo
        ddlTipoRenta.SelectedValue = Request.QueryString("tipo_renta")
        ddlGrupoLimite.Items.Clear()
        CargarGrupoLimite(Request.QueryString("tipo_renta"))
        ddlGrupoLimite.Enabled = True
        ddlGrupoLimite.SelectedValue = oRow.CodigoGrupLimTrader
        tbPorcentaje.Text = oRow.Porcentaje.ToString("#0.00")

    End Sub

    Private Sub LimpiarCampos()
        ddlTipoCargo.SelectedIndex = 0
        ddlPortafolio.SelectedIndex = 0
        ddlGrupoLimite.SelectedIndex = 0
        ddlTipoRenta.SelectedIndex = 0
        tbPorcentaje.Text = 0
    End Sub

End Class
