Imports System.Data
Imports Sit.BusinessLayer
Imports UIUtility

Partial Class Modulos_Inversiones_ConsultasPreOrden_frmBusquedaValoresInstrumento
    Inherits BasePage

#Region "/* Metodos Personalizados */"

    Private Function BuscarValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, _
    ByVal correlativo As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal TipoOperacion As String, _
    ByVal TipoInstrumento As String, ByVal TipoRenta As String) As DataTable
        Return New ValoresBM().InstrumentosBuscarPorFiltroConsultarOrdenesPreordenes(isin, sbs, mnemonico, correlativo, FechaInicio, FechaFin, TipoOperacion, TipoInstrumento, TipoRenta, DatosRequest).Tables(0)
    End Function

    Private Function ObtenerDatosBusqueda() As Boolean
        Dim sbs As String = IIf(Request.QueryString("vSBS") Is Nothing, "", Request.QueryString("vSBS"))
        Dim isin As String = IIf(Request.QueryString("vISIN") Is Nothing, "", Request.QueryString("vISIN"))
        Dim mnemonico As String = IIf(Request.QueryString("vMnemonico") Is Nothing, "", Request.QueryString("vMnemonico"))
        Dim correlativo As String = IIf(Request.QueryString("vCorrelativo") Is Nothing, "", Request.QueryString("vCorrelativo"))
        Dim FechaInicio As String = IIf(Request.QueryString("vFechaInicio") Is Nothing, "", Request.QueryString("vFechaInicio"))
        Dim FechaFin As String = IIf(Request.QueryString("vFechaFin") Is Nothing, "", Request.QueryString("vFechaFin"))
        Dim TipoOperacion As String = IIf(Request.QueryString("vTipoOperacion") Is Nothing, "", Request.QueryString("vTipoOperacion"))
        Dim TipoInstrumento As String = IIf(Request.QueryString("vTipoInstrumento") Is Nothing, "", Request.QueryString("vTipoInstrumento"))
        Dim TipoRenta As String = IIf(Request.QueryString("vTipoRenta") Is Nothing, "", Request.QueryString("vTipoRenta"))
        Dim dtblDatos As DataTable = BuscarValores(isin, sbs, mnemonico, correlativo, UIUtility.ConvertirFechaaDecimal(FechaInicio), UIUtility.ConvertirFechaaDecimal(FechaFin), TipoOperacion, TipoInstrumento, TipoRenta)

        dgLista.DataSource = dtblDatos : Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos.Rows.Count) + "');")
        Return True
    End Function

    Public Sub SeleccionarISIN(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        Dim arraySesiones As String() = New String(6) {}
        arraySesiones(0) = strcadena.Split(",").GetValue(0)
        arraySesiones(1) = strcadena.Split(",").GetValue(1)
        arraySesiones(2) = strcadena.Split(",").GetValue(2)
        arraySesiones(3) = strcadena.Split(",").GetValue(3)
        Session("SS_DatosModal") = arraySesiones
        Dim script As New StringBuilder
        With script
            .Append("window.close();")

        End With
        EjecutarJS(script.ToString())
    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ObtenerDatosBusqueda()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            Me.dgLista.PageIndex = e.NewPageIndex
            ObtenerDatosBusqueda()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff';")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5f5f5';this.style.cursor='hand'")
        End If
    End Sub

    Protected Sub ibCancelar_Click(sender As Object, e As System.EventArgs) Handles ibCancelar.Click
        EjecutarJS("window.close();")
    End Sub

#End Region

End Class
