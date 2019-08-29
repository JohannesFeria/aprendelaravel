Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text

Partial Class Modulos_Inversiones_frmModificacionFechaIDI
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
                LimpiarCampos()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        dgLista.PageIndex = 0
        CargarGrilla()
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Dim objBM As New OrdenPreOrdenInversionBM
        Dim resul As Boolean = objBM.ActualizarFechaIDI(txtCodigoOrdenAux.Text, hdCodigoPortafolioSBS.Value, UIUtility.ConvertirFechaaDecimal(txtFechaIDIAux.Text), DatosRequest)
        If resul Then
            AlertaJS("Se actualizó la Fecha IDI exitosamente")
            CargarGrilla()
        Else
            AlertaJS("Hubo un error al actualizar la Fecha IDI")
        End If
        LimpiarCampos()
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim i As Integer = 0
        If e.CommandName = "Select" Then
            Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)            
            i = Row.RowIndex
            txtFechaOperacionAux.Text = dgLista.Rows(i).Cells(1).Text()
            txtFechaIDIAux.Text = IIf(dgLista.Rows(i).Cells(2).Text() = "&nbsp;", txtFechaOperacionAux.Text, dgLista.Rows(i).Cells(2).Text())
            txtMnemonicoAux.Text = dgLista.Rows(i).Cells(3).Text()
            txtCodigoOrdenAux.Text = dgLista.Rows(i).Cells(4).Text()
            hdCodigoPortafolioSBS.Value = dgLista.Rows(i).Cells(6).Text()
        End If
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        LlenaGrilla()
    End Sub

    Private Sub LimpiarCampos()
        txtCodigoOrdenAux.Text = ""
        txtFechaIDIAux.Text = ""
        txtFechaOperacionAux.Text = ""
        txtMnemonicoAux.Text = ""
        hdCodigoPortafolioSBS.Value = ""
    End Sub

    Private Sub CargarGrilla()
        Dim objBM As New OrdenPreOrdenInversionBM
        Dim dsAux As DataSet = objBM.SeleccionarOI_FechaIDI(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFecha.Text), txtMnemonico.Text, datosrequest)

        Session("dsDatosFechas") = dsAux
        LlenaGrilla()
    End Sub

    Private Sub LlenaGrilla()
        Dim dsAux As DataSet = Session("dsDatosFechas")
        dgLista.DataSource = dsAux
        dgLista.DataBind()
    End Sub

    Private Sub CargarCombos()
        Dim oPortafolioBM As New PortafolioBM
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0), "CodigoPortafolioSBS", "Descripcion", True)
    End Sub

End Class
