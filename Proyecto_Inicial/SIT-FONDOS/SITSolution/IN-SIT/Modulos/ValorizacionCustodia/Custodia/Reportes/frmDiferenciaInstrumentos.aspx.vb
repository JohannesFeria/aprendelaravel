Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmDiferenciaInstrumentos
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
            Dim sPortafolioCodigo As String = Request.QueryString("sPortafolioCodigo")
            Dim sPortafolioDescripcion As String = Request.QueryString("sPortafolioDescripcion")
            Dim sCodigoCustodio As String = Request.QueryString("sCodigoCustodio")
            Dim sReporte As String = Request.QueryString("sRep")
            Dim sNombreCustodio As String = Request.QueryString("sNombreCustodio") 'RGF 20101103 INC 61546
            Dim sFechaOperacion As String = nFechaOperacion
            sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)
            'RGF 20081118 Reporte
            If sReporte.Equals("DiferenciasCartera_Custodia") Then
                Dim oInstDife As New InstDife
                rep.Load(Server.MapPath("DiferenciasCarteraCustodio.rpt"))
                Dim oInstDifeTMP As DataSet = New CustodioArchivoBM().InstrumentosDiferenciasCarteraCustodio(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio)
                Dim drconsulta As DataRow
                For Each drv As DataRow In oInstDifeTMP.Tables(0).Rows
                    drconsulta = oInstDife.Tables(0).NewRow()
                    drconsulta("Código Mnemónico") = drv("CodigoMnemonico")
                    drconsulta("Código ISIN") = drv("CodigoISIN")
                    drconsulta("Código SBS") = drv("CodigoSBS")
                    drconsulta("Fondo") = drv("Name_Portafolio")
                    drconsulta("Nombre Emisor") = drv("Emisor")
                    drconsulta("Sinónimo") = drv("CodigoTipoInstrumento")
                    drconsulta("Unidades") = Format(Convert.ToDecimal(drv("UnidadesCartera")), "###,##0.0000000")
                    drconsulta("Valor Custodio Und") = Format(Convert.ToDecimal(drv("UnidadesCustodia")), "###,##0.0000000")
                    oInstDife.Tables(0).Rows.Add(drconsulta)
                Next
                rep.SetDataSource(oInstDife)
                crDiferencias.ReportSource = rep
                rep.SetParameterValue("Usuario", Usuario)
                rep.SetParameterValue("Portafolio", sPortafolioDescripcion)
                rep.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio)
            ElseIf sCodigoCustodio = "" Then                
                Dim dtConsulta As New DataTable
                Dim dsconsulta As New InstDifVarios
                Dim drconsulta As DataRow
                rep.Load(Server.MapPath("DiferenciasVarios.rpt"))
                dtConsulta = New CustodioArchivoBM().InstrumentosDiferenciasVarios(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest).Tables(0)
                For Each drv As DataRow In dtConsulta.Rows
                    drconsulta = dsconsulta.Tables(0).NewRow()
                    drconsulta("CodigoMnemonico") = drv("CodigoMnemonico")
                    drconsulta("CodigoISIN") = drv("CodigoISIN")
                    drconsulta("CodigoSBS") = drv("CodigoSBS")
                    drconsulta("CodigoPortafolioSBS") = drv("Name_Portafolio")
                    drconsulta("CodigoCustodio") = drv("CodigoCustodio")
                    drconsulta("Emisor") = drv("Emisor")
                    drconsulta("CodigoTipoTitulo") = drv("CodigoTipoTitulo")
                    drconsulta("CodigoTipoInstrumento") = drv("CodigoTipoInstrumento")
                    drconsulta("Unidades") = Format(Convert.ToDecimal(drv("Unidades")), "###,##0.0000000")
                    drconsulta("Saldo") = Format(Convert.ToDecimal(drv("Saldo")), "###,##0.0000000")
                    drconsulta("Diferencia") = Format(Convert.ToDecimal(drv("Diferencia")), "###,##0.0000000")
                    drconsulta("Nro") = Format(Convert.ToDecimal(drv("TotalUnidades")), "###,##0.0000000")
                    drconsulta("Vpn") = Format(Convert.ToDecimal(drv("TotalSaldo")), "###,##0.0000000")
                    drconsulta("Total") = Format(Convert.ToDecimal(drv("TotalDiferencia")), "###,##0.0000000")
                    drconsulta("Valor") = drv("Valor")
                    dsconsulta.Tables(0).Rows.Add(drconsulta)
                Next
                dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                rep.SetDataSource(dsconsulta)
                crDiferencias.ReportSource = rep
                rep.SetParameterValue("FechaOperacion", sFechaOperacion)
                rep.SetParameterValue("Usuario", Usuario)
                rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
            Else
                Dim oInstDife As New InstDife
                rep.Load(Server.MapPath("Diferencias.rpt"))
                Dim oInstDifeTMP As DataSet = New CustodioArchivoBM().InstrumentosDiferencias(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                CopiarTabla(oInstDifeTMP.Tables(0), oInstDife.InstrumentosDiferencias)
                rep.SetDataSource(oInstDife)
                rep.SetParameterValue("Usuario", MyBase.Usuario)
                rep.SetParameterValue("FechaOperacion", sFechaOperacion)
                rep.SetParameterValue("Portafolio", sPortafolioDescripcion) 'RGF 20101103 INC 61546
                rep.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio) 'RGF 20101103 INC 61546
                rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
                crDiferencias.ReportSource = rep
            End If
            crDiferencias.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.GroupTree
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página de Impresión")
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmDiferenciaInstrumentos_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class