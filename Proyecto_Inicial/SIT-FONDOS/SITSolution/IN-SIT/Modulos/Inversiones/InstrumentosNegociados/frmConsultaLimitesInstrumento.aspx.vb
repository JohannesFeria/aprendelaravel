Imports System.Data

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmConsultaLimitesInstrumento
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            Dim dstLimites As DataSet
            Dim strInstrumento As String = String.Empty

            Try
                Dim GUID As String = Request.QueryString("GUID")
                dstLimites = CType(Session(GUID), DataSet)
                strInstrumento = Session("Instrumento")
                If dstLimites Is Nothing And strInstrumento = "" Then
                    Retornar()
                Else
                    lblInstrumento.Text = strInstrumento
                    CargarLimitesEvaluados(dstLimites)
                End If
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

    Private Function CargarLimitesEvaluados(ByVal dstLimitesEvaluados As DataSet) As System.Boolean
        dgLista.DataSource = dstLimitesEvaluados.Tables(0)
        dgLista.DataBind()
        Return True
    End Function

    Private Sub Imprimir()
        EjecutarJS("window.print();")
    End Sub

    Private Sub Retornar()
        EjecutarJS("window.close();")
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Call Imprimir()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Call Retornar()
    End Sub

End Class
