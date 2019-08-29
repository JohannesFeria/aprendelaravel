Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Valorizacion_Reportes_frmPrecios
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try            
            Dim oPrecio As New DsRptPrecios
            Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
            Dim nFechaOperacionFin As Long = Request.QueryString("nFechaOperacionFin")
            Dim sTipoInst As String = Request.QueryString("sTipoInstrumento")
            Dim sCodMnemo As String = Request.QueryString("sCodigoMnemonico")
            Dim sCodIsin As String = Request.QueryString("sCodigoIsin")
            Dim sEntExt As String = Request.QueryString("sEntidad")
            Dim sFechaOperacion As String = nFechaOperacion
            sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)
            rep.Load(Server.MapPath("RptPrecios.rpt"))
            Dim oPrecioTMP As New DataSet
            'OT 10238 - 18/04/2017 - Carlos Espejo
            'Descripcion: Se quita el parametro TipoInstrumento
            oPrecioTMP = New VectorPrecioBM().ListarRango(nFechaOperacion, nFechaOperacionFin, sCodMnemo, sCodIsin, sEntExt, DatosRequest)
            'OT 10238 Fin
            CopiarTabla(oPrecioTMP.Tables(0), oPrecio.Precio)
            rep.SetDataSource(oPrecio)
            crPrecios.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree
            crPrecios.RenderingDPI = 120
            crPrecios.ReportSource = rep
            rep.SetParameterValue("Usuario", MyBase.Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep.SetParameterValue("FuenteInformacion", "Reporte de Precios " & sEntExt)
            rep.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Generar el Reporte de Precios")
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Valorizacion_Reportes_frmPrecios_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class