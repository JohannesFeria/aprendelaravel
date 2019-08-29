Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Partial Class Modulos_Contabilidad_frmBusquedaAplicacion
    Inherits BasePage

#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            Dim Aplicacion As String
            Aplicacion = Request.QueryString("pAplicacion")
            hd.Value = Aplicacion
            ObtenerDatosBusqueda()
        End If

    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        ActualizarIndice(e.NewPageIndex)
    End Sub

    Protected Sub dgLista_RowDataBound1(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff';")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5f5f5';this.style.cursor='hand'")
        End If
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        ObtenerDatosBusqueda()
    End Sub

#End Region


#Region " /* Funciones Personalizadas*/"

    Private Function ObtenerDatosBusqueda() As Boolean
        Dim dsConsulta As New DsAplicacion
        Dim drconsulta As DsAplicacion.AplicacionRow
        Dim dtTabla As New DataTable
        Dim strCodigo As String

        strCodigo = Me.hd.Value
        dtTabla = New MatrizContableBM().Listar(DatosRequest).Tables(0)
        Dim tabla As String = New MatrizContableBM().Listar(DatosRequest).Tables(0).Select("CodigoMatrizContable = " & strCodigo)(0)("TablaMatriz")

        Dim ds As New DataSet
        Select Case tabla
            Case "I"
                ds = New CuentasPorCobrarPagarBE
            Case "T"
                ds = New OperacionCajaBE
            Case "V"
                ds = New CarteraTituloValoracionBE
        End Select

        For Each dc As DataColumn In ds.Tables(0).Columns
            Dim tipo As String
            tipo = dc.DataType.ToString
            If tipo = "System.Decimal" And Convert.ToString(dc.ColumnName.ToUpper).Substring(0, 3).ToUpper <> "FEC" Then
                drconsulta = dsConsulta.Tables(0).NewRow()
                drconsulta("Campo") = dc.ColumnName.ToUpper
                dsConsulta.Tables(0).Rows.Add(drconsulta)
            End If
        Next

        Dim dtblDatos As DataTable = dsConsulta.Tables(0)
        dgLista.DataSource = dtblDatos : Me.dgLista.DataBind()

        Return True

    End Function

    Private Sub ReturnArgumentShowDialogPopup(ByVal campo As String)
        If Not Session("SS_DatosModal") Is Nothing Then
            Session.Remove("SS_DatosModal")
        End If

        Dim arraySesiones As String() = New String(1) {}
        arraySesiones(0) = campo

        Session("SS_DatosModal") = arraySesiones
        EjecutarJS("window.close();")
    End Sub

    Public Sub Seleccionar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        ReturnArgumentShowDialogPopup(strcadena)
    End Sub


#End Region

End Class
