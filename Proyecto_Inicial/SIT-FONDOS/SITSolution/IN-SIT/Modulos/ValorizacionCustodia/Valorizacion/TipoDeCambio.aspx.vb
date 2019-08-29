Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Reportes_TipoDeCambio
    Inherits BasePage

    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página

        Dim oVectorTipoCambioBM As New VectorTipoCambioBM

        If Not Page.IsPostBack Then
            btnCancelar.Attributes.Add("onclick", "return CloseWindow();")
        End If
        Dim rep As New ReportDocument

        Dim oTipoCambio As New TipoCamb
        Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
        Dim sFechaOperacion As String = nFechaOperacion
        Dim sFuenteInformacion As String = Request.QueryString("sFuenteInformacion")
        sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)

        rep.Load(Server.MapPath("TipoCambioRP.rpt"))
        Dim oTipoCambioTMP As DataSet = New VectorTipoCambioBM().Listar(nFechaOperacion, sFuenteInformacion, "", DatosRequest)
        CopiarTabla(oTipoCambioTMP.Tables(0), oTipoCambio.TipoCambio)
        rep.SetDataSource(oTipoCambio)

        crTipoDeCambio.DisplayGroupTree = False
        crTipoDeCambio.ReportSource = rep
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("FechaOperacion", sFechaOperacion)
        rep.SetParameterValue("FuenteInformacion", "Reporte de Tipo de Cambio - " & sFuenteInformacion)

    End Sub
End Class
