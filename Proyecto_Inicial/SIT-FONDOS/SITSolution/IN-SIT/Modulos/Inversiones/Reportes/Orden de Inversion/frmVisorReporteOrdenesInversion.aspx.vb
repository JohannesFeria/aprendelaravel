Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Inversiones_Reportes_Orden_de_Inversion_frmfrmVisorReporteOrdenesInversion
    Inherits BasePage
    Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fecha As Decimal = Convert.ToDecimal(Request.QueryString("vFecha"))
        Dim fechaFin As Decimal = Convert.ToDecimal(Request.QueryString("vFechaFin"))
        Dim codPortafolio As String = Convert.ToString(Request.QueryString("vPortafolio"))
        Dim nomPortafolio As String = Convert.ToString(Request.QueryString("vNomPortafolio"))
        Dim vEstado As String = Convert.ToString(Request.QueryString("vEstado"))
        Dim RegSBS As String = Convert.ToString(Request.QueryString("vRegSBS"))
        Dim liqAntFon As String = Convert.ToString(Request.QueryString("vliqAntFon"))
        Dim estado As String = String.Empty
        If Not (Request.QueryString("estado") Is Nothing) Then
            estado = Convert.ToString(Request.QueryString("estado"))
        End If
        If codPortafolio = "--Seleccione--" Then
            codPortafolio = ""
        End If
        Dim oOI As New OrdenPreOrdenInversionBM
        Dim i As Integer = 1
        Dim dsAux As New DataSet
        Dim dsOIReporte As New DsOrdenInversion
        Dim estatus As String
        If estado = "E-CONELIMOD" Then
            estatus = "E-CON"
            dsAux = oOI.ListarOrdenesInversionPorFecha(fecha, fechaFin, codPortafolio, estatus, RegSBS, liqAntFon, DatosRequest)
            i = dsAux.Tables(0).Rows.Count
            estatus = "E-ELI"
            Dim dsAux2 As New DataSet
            dsAux2 = oOI.ListarOrdenesInversionPorFecha(fecha, fechaFin, codPortafolio, estatus, RegSBS, liqAntFon, DatosRequest)
            dsAux.Merge(dsAux2, True, MissingSchemaAction.Ignore)
            i = dsAux.Tables(0).Rows.Count
            estatus = "E-MOD"
            dsAux2 = oOI.ListarOrdenesInversionPorFecha(fecha, fechaFin, codPortafolio, estatus, RegSBS, liqAntFon, DatosRequest)
            dsAux.Merge(dsAux2, True, MissingSchemaAction.Ignore)
            i = dsAux.Tables(0).Rows.Count
        Else
            dsAux = oOI.ListarOrdenesInversionPorFecha(fecha, fechaFin, codPortafolio, estado, RegSBS, liqAntFon, DatosRequest)
        End If
        For i = 0 To dsAux.Tables(0).Rows.Count - 1
            dsAux.Tables(0).Rows(i)("numero") = i + 1
        Next
        CopiarTabla(dsAux.Tables(0), dsOIReporte.OrdenInversionReporte)
        dsOIReporte.Merge(dsOIReporte, False, System.Data.MissingSchemaAction.Ignore)
        Dim nombre, usuario As String
        nombre = "Usuario"
        Dim oStream As New System.IO.MemoryStream        
        Try
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
            Cro.Load(Server.MapPath("RelacionOI_Estado.rpt"))
            Cro.SetDataSource(dsOIReporte)
            Cro.SetParameterValue("@Usuario", usuario)
            Cro.SetParameterValue("@p_Portafolio", IIf(codPortafolio = "", "TODOS", nomPortafolio))
            Cro.SetParameterValue("@p_Fecha", UIUtility.ConvertirFechaaString(fecha))
            Cro.SetParameterValue("@p_Estado", vEstado)
            Cro.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            Me.CrystalReportViewer1.ReportSource = Cro
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_Orden_de_Inversion_frmfrmVisorReporteOrdenesInversion_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Cro.Close()
        Cro.Dispose()
    End Sub
End Class