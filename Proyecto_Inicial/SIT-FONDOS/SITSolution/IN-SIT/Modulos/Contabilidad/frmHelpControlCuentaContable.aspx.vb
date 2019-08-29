Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Contabilidad_frmHelpControlCuentaContable
    Inherits BasePage

    Dim oPlanDeCuentasBM As PlanDeCuentasBM
    Dim oPlanDeCuentasBE As DataSet

#Region "/* Metodos Personalizados */"

    Private Sub Buscar()

        Dim dtResultado As New DataTable
        Dim oPlanCuentaBM As New PlanDeCuentasBM
        Dim portafolio As String = Request.QueryString("tlbPortafolio")

        dtResultado = oPlanCuentaBM.Buscar(txtCuentaContable.Text, txtDescripcion.Text, portafolio, MyBase.DatosRequest()).Tables(0)

        Me.dgLista_Modal.DataSource = dtResultado
        Me.dgLista_Modal.DataBind()

    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Private Sub ibBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibBuscar.Click
        Try
            Me.dgLista_Modal.PageIndex = 0
            Buscar()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista_Modal.PageIndexChanging
        Me.dgLista_Modal.PageIndex = e.NewPageIndex
        Buscar()
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista_Modal.RowCommand
        Select Case e.CommandName
            Case "Seleccionar"
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                With dgLista_Modal.Rows(row.RowIndex)
                    ReturnArgumentShowDialogPopup(.Cells(2).Text, .Cells(3).Text)
                End With
        End Select
    End Sub

    Private Sub ReturnArgumentShowDialogPopup(ByVal cuentaContable As String, ByVal descripcionCuenta As String)
        If Not Session("SS_DatosModal") Is Nothing Then
            Session.Remove("SS_DatosModal")
        End If

        Dim arraySesiones As String() = New String(2) {}
        arraySesiones(0) = cuentaContable
        arraySesiones(1) = descripcionCuenta

        Session("SS_DatosModal") = arraySesiones
        EjecutarJS("window.close();")
    End Sub

#End Region

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
