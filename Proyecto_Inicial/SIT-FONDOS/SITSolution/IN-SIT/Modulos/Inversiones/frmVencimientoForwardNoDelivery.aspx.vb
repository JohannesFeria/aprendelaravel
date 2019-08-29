Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Inversiones_frmVencimientoForwardNoDelivery
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Try
                CargarCombos()
                ibProcesar.Attributes.Add("onClick", "javascript:return Confirmar();")
                tbFechaVencimientoDesde.Text = DateTime.Now.ToString("dd/MM/yyyy")
                tbFechaVencimientoHasta.Text = DateTime.Now.ToString("dd/MM/yyyy")
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

    Public Sub CargarCombos()
        ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondo.DataValueField = "CodigoPortafolio"
        ddlFondo.DataTextField = "Descripcion"
        ddlFondo.DataBind()
        ddlFondo.Items.Insert(0, New ListItem("--Seleccione--", ""))
        UIUtility.CargarMonedaOI(ddlMonedaNegociada)
        UIUtility.CargarMonedaOI(ddlMonedaDestino)
        Dim DtablaMercado As New DataTable
        Dim oMercadoBM As New MercadoBM
        DtablaMercado = oMercadoBM.ListarActivos(DatosRequest).Tables(0)
        DtablaMercado.DefaultView.Sort = "CodigoMercado"
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlMercado, DtablaMercado, "CodigoMercado", "Descripcion", True)
    End Sub
    Private Sub ibBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibBuscar.Click
        Try
            If tbFechaVencimientoDesde.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar una fecha inicial.")
                Exit Sub
            End If
            If tbFechaVencimientoHasta.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar una fecha final.")
                Exit Sub
            End If
            dgLista.PageIndex = 0
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub CargarGrilla()
        Dim strPortafolio As String = IIf(ddlFondo.SelectedValue.ToString = "Todos", "", ddlFondo.SelectedValue.ToString)
        Dim strMercado As String = IIf(ddlMercado.SelectedValue.ToString = "Todos", "", ddlMercado.SelectedValue.ToString)
        Dim dsOperaciones As DataSet = New OrdenPreOrdenInversionBM().ListarVencimientoForwardNoDelivery(strPortafolio, UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoDesde.Text), UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoHasta.Text), ddlMonedaNegociada.SelectedValue, ddlMonedaDestino.SelectedValue, strMercado, rbnCalculo.SelectedValue, DatosRequest)
        lblCantidad.Text = String.Format("({0})", dsOperaciones.Tables(0).Rows.Count)
        dgLista.DataSource = dsOperaciones
        dgLista.DataBind()
        dgLista.SelectedIndex = -1
    End Sub
    Private Sub ibProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibProcesar.Click
        Dim chk As CheckBox
        Dim objB As New OrdenPreOrdenInversionBM
        If txtFixing.Text.Trim.Equals("") Then
            AlertaJS("Debe ingresar el fixing.")
            Exit Sub
        End If
        Try
            Dim nCont As Long = 0
            For Each item As GridViewRow In dgLista.Rows
                chk = CType(item.FindControl("chkSeleccion"), CheckBox)
                If chk.Checked Then
                    If item.Cells(13).Text.Trim = "L" Then
                        nCont = nCont + 1
                    Else
                        objB.ConfirmarVencimientoForwardNoDelivery(item.Cells(0).Text, item.Cells(12).Text, txtFixing.Text, DatosRequest)
                    End If
                End If
            Next
            If nCont > 0 Then
                AlertaJS("Algunas operaciones de Forward no se procesaron por que ya estaban liquidadas.\nDebe reversar la liquidación para permitir procesar la pre-liquidación.")
            End If
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
End Class
