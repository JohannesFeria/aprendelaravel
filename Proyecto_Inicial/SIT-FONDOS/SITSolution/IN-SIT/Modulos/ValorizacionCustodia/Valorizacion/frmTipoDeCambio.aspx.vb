Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports Sit.BusinessLayer
Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmTipoDeCambio
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim oVectorTipoCambioBM As New VectorTipoCambioBM            
            Dim oTipoCambio As New TipoCamb
            Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
            Dim sFechaOperacion As String = nFechaOperacion
            Dim sFuenteInformacion As String = Request.QueryString("sFuenteInformacion")
            sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)
            rep.Load(Server.MapPath("TipoCambioRP.rpt"))
            Dim oTipoCambioTMP As DataSet = New VectorTipoCambioBM().Listar(nFechaOperacion, sFuenteInformacion, "", DatosRequest)
            CopiarTabla(oTipoCambioTMP.Tables(0), oTipoCambio.TipoCambio)
            rep.SetDataSource(oTipoCambio)
            crTipoDeCambio.ReportSource = rep
            rep.SetParameterValue("Usuario", MyBase.Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep.SetParameterValue("FuenteInformacion", "Reporte de Tipo de Cambio - " & sFuenteInformacion)
            rep.SetParameterValue("rutaLogo", ConfigurationManager.AppSettings("RUTA_LOGO"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Protected Sub form1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class