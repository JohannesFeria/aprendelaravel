Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmBuscarValor
    Inherits BasePage
#Region " /* Eventos de Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If Not Page.IsPostBack Then
            dgLista.PageIndex = 0
            CargarGrilla()
            CargarCombos()
        End If
    End Sub
    Private Sub ibCancelar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ibCancelar.Click
        CloseWindow()
    End Sub
#End Region
#Region " /* Funciones Personalizadas*/"
    Private Sub CargarGrilla(Optional ByVal CodigoTipoInstrumentoSBS As String = "")
        Dim objValores As New ValoresBM
        Dim strSBS As String = IIf(Request.QueryString("vSBS") Is Nothing, "", Request.QueryString("vSBS"))
        Dim strISIN As String = IIf(Request.QueryString("vISIN") Is Nothing, "", Request.QueryString("vISIN"))
        Dim strMnemonico As String = IIf(Request.QueryString("vMnemonico") Is Nothing, "", Request.QueryString("vMnemonico"))
        Dim strCategoriaInstrumento As String = IIf(Request.QueryString("vCategoria") Is Nothing, "", Request.QueryString("vCategoria"))
        Dim strFondo As String = IIf(Request.QueryString("vFondo") Is Nothing, "", Request.QueryString("vFondo"))
        Dim cFondo As String = IIf(Request.QueryString("cFondo") Is Nothing, "", Request.QueryString("cFondo"))
        Me.Session("Portafolio") = cFondo
        Dim strOperacion As String = IIf(Request.QueryString("vOperacion") Is Nothing, "", Request.QueryString("vOperacion"))
        Dim strCodigoTipoInstrumentoSBS As String = IIf(Me.ddlTipoIntrumento.SelectedIndex = 0, "", Me.ddlTipoIntrumento.SelectedValue.ToString)
        Dim i As Integer
        Dim DSresultado = objValores.ListarPorFiltro(DatosRequest, strCategoriaInstrumento, strSBS, strISIN, strMnemonico, cFondo, strCodigoTipoInstrumentoSBS, strOperacion)
        If DSresultado.Tables.Count <> 0 Then
            Me.dgLista.DataSource = DSresultado.Tables(0)
            Me.dgLista.DataBind()
            If strOperacion = "1" Or strOperacion = "3" Then   'COMPRA
                dgLista.Columns(7).Visible = False
                dgLista.Columns(8).Visible = False
            ElseIf strOperacion = "2" Then  'VENTA
                dgLista.Columns(7).Visible = True
                dgLista.Columns(8).Visible = True
                For i = 0 To dgLista.Rows.Count - 1
                    dgLista.Rows(i).Cells(8).Text = Math.Round(Convert.ToDecimal(dgLista.Rows(i).Cells(8).Text), Constantes.M_INT_NRO_DECIMALES)
                Next
            End If
            If dgLista.Rows.Count = 0 Then
                If CodigoTipoInstrumentoSBS = "" Then
                    AlertaJS("No existen registros para mostrar", "window.close()")
                End If
            End If
            lbContador.Text = UIUtility.MostrarResultadoBusqueda(DSresultado.Tables(0).Rows.Count)
        Else
            AlertaJS("No existen registros para mostrar", "window.close()")
        End If
    End Sub
    Private Sub ReturnArgumentShowDialogPopup(ByVal isin As String, ByVal mnemonico As String, ByVal sbs As String, ByVal codigoCustodio As String, ByVal saldo As String, ByVal fondo As String, moneda As String)
        Session("Busqueda") = 1
        Dim arraySesiones As String() = New String(6) {}
        arraySesiones(0) = isin
        arraySesiones(1) = mnemonico
        arraySesiones(2) = sbs
        arraySesiones(3) = codigoCustodio
        arraySesiones(4) = saldo
        arraySesiones(5) = moneda
        arraySesiones(6) = Me.Session("Portafolio")
        Session("SS_DatosModal") = arraySesiones
        EjecutarJS("window.close();")
    End Sub
    Private Sub CloseWindow()
        Session("Busqueda") = 2
        EjecutarJS("window.close();")
    End Sub
    Public Sub SeleccionarISIN(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(0), strcadena.Split(",").GetValue(1), strcadena.Split(",").GetValue(2), strcadena.Split(",").GetValue(3), strcadena.Split(",").GetValue(4), strcadena.Split(",").GetValue(5), strcadena.Split(",").GetValue(6))
    End Sub
#End Region
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
    Public Sub CargarCombos()
        Dim strCategoriaInstrumento As String = IIf(Request.QueryString("vCategoria") Is Nothing, "", Request.QueryString("vCategoria"))
        Dim tablaTipoInstrumento As New Data.DataTable
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        tablaTipoInstrumento = oTipoInstrumentoBM.ListarTipoInstrumentoPorCategoria(strCategoriaInstrumento, Me.DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoIntrumento, tablaTipoInstrumento, "CodigoTipoInstrumentoSBS", "Descripcion", True)
    End Sub
    Private Sub ddlTipoIntrumento_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoIntrumento.SelectedIndexChanged
        dgLista.PageIndex = 0
        CargarGrilla(Me.ddlTipoIntrumento.SelectedValue.ToString)
    End Sub
End Class