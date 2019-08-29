Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data
Imports UIUtility

Partial Class Modulos_Contabilidad_frmPopupCuentaContable
    Inherits BasePage


    Dim oPlanDeCuentasBM As PlanDeCuentasBM
    Dim oPlanDeCuentasBE As DataSet

#Region " /* Eventos de Página */ "

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        ActualizarIndice(e.NewPageIndex)
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff';")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5f5f5';this.style.cursor='hand'")
        End If
    End Sub

    Protected Sub ibBuscar_Click(sender As Object, e As System.EventArgs) Handles ibBuscar.Click
        Buscar()
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub ReturnArgumentShowDialogPopup(ByVal cuentaContable As String, ByVal descripcion As String)
        If Not Session("SS_DatosModal") Is Nothing Then
            Session.Remove("SS_DatosModal")
        End If

        Dim arraySesiones As String() = New String(2) {}
        arraySesiones(0) = cuentaContable
        arraySesiones(1) = descripcion

        Session("SS_DatosModal") = arraySesiones
        EjecutarJS("window.close();")
    End Sub

    Public Sub Seleccionar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(0), strcadena.Split(",").GetValue(1))
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        Me.dgLista.PageIndex = nuevoIndice
        Buscar()
    End Sub

    Private Sub Buscar()
        Dim oPlanCuentaBM As New PlanDeCuentasBM
        Dim portafolio As String = Request.QueryString("vPortafolio")

        Dim dtblDatos As DataTable = oPlanCuentaBM.Buscar(txtCuentaContable.Text, txtDescripcion.Text, portafolio, MyBase.DatosRequest()).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

#End Region

End Class
