Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmTitulosAsociadosCustodios
    Inherits BasePage
    Dim rep As New ReportDocument
#Region "Variables"
    Dim oUtil As New UtilDM
#End Region
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim oTitAsocCus As New TitAsoCu
            Dim sFechaSaldo As String = Request.QueryString("FechaSaldo")
            Dim sPortafolioCodigo As String = Request.QueryString("PortafolioCodigo")
            Dim sCodigoISIN As String = Request.QueryString("CodigoISIN")
            Dim sCodigoCustodio As String = Request.QueryString("CodigoCustodio")
            Dim sTipoInstrumento As String = Request.QueryString("TipoInstrumento")
            Dim sTipoRenta As String = Request.QueryString("TipoRenta")
            Dim sCodigoMnemonico As String = Request.QueryString("CodigoMnemonico")
            Dim sCodigoMoneda As String = Request.QueryString("CodigoMoneda")
            Dim sFechaOperacion As String = oUtil.RetornarFechaSistema()            
            rep.Load(Server.MapPath("TitulosCustodios.rpt"))
            Dim oTitAsocCusTMP As DataSet = New CustodioBM().ListarTitulosAsociadosCustodiosC2(sFechaSaldo, sPortafolioCodigo, sCodigoISIN, sCodigoCustodio, sTipoInstrumento, sTipoRenta, sCodigoMnemonico, sCodigoMoneda, DatosRequest)
            CopiarTabla(oTitAsocCusTMP.Tables(0), oTitAsocCus.TitulosAsociadosCustodios)
            rep.SetDataSource(oTitAsocCus)
            crTitulosAsociados.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree
            crTitulosAsociados.RenderingDPI = 120
            crTitulosAsociados.ReportSource = rep
            rep.SetParameterValue("Usuario", MyBase.Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep.SetParameterValue("RutaLogo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmTitulosAsociadosCustodios_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class