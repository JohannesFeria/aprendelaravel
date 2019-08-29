Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosUnificados
    Inherits BasePage
    Dim rep As New ReportDocument
    Dim rep2 As New ReportDocument
    Dim rep3 As New ReportDocument
    Dim rep4 As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Function CargarRuta() As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("012", MyBase.DatosRequest())
        Return (oArchivoPlanoBE.Tables(0).Rows(0).Item(4))
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim ruta As String
                ruta = CargarRuta()
                Dim archivosEntrada As String = String.Empty
                Dim archivoSalida As String                
                Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
                Dim sPortafolioCodigo As String = Request.QueryString("sPortafolioCodigo")
                Dim sCodigoCustodio As String = Request.QueryString("sCodigoCustodio")
                Dim sNombreCustodio As String = Request.QueryString("sNombreCustodio")
                Dim sFechaOperacion As String = nFechaOperacion
                sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)
                Dim archivo As DataSet = New ArchivoPlanoBM().Seleccionar("025", Nothing)
                'Dim rutaArchivo As String = "\\zxdes23\Archivos\ArchivosPlanos\Tesoreria\NoReg.pdf" '#ERROR#
                'Llamada a la ruta de archivo desde parametría
                Dim rutaArchivo As String = archivo.Tables(0).Rows(0)("ArchivoUbicacion").ToString.Trim() & archivo.Tables(0).Rows(0)("ArchivoNombre").ToString.Trim() & "." & archivo.Tables(0).Rows(0)("ArchivoExtension").ToString.Trim()
                'Obtención de fondos
                Dim portafolio As New PortafolioBM
                Dim dtPortafolio As DataTable = portafolio.PortafolioCodigoListar(ParametrosSIT.PORTAFOLIO_MULTIFONDOS)
                'Dim tPortafolio(3) As String
                Dim i As Byte
                rep.Load(Server.MapPath("InstrumentosNoRegistradosUnif.rpt"))
                rep2.Load(Server.MapPath("InstrumentosConciliadosUnif.rpt"))
                rep3.Load(Server.MapPath("DiferenciasUnif.rpt"))
                rep4.Load(Server.MapPath("DiferenciasVariosUnif.rpt"))
                For i = 0 To dtPortafolio.Rows.Count - 1
                    sPortafolioCodigo = dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim
                    Dim oInstNoreg As New InstNoreg
                    Dim tableNoRegis As DataTable
                    Dim oInstNoregTMP As DataSet
                    sCodigoCustodio = "CAVALI" '#ERROR#
                    oInstNoregTMP = New CustodioArchivoBM().InstrumentosNoRegistrados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableNoRegis = oInstNoregTMP.Tables(0)
                    sCodigoCustodio = "BBH" '#ERROR#
                    oInstNoregTMP = New CustodioArchivoBM().InstrumentosNoRegistrados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableNoRegis.Merge(oInstNoregTMP.Tables(0))
                    CopiarTabla(tableNoRegis, oInstNoreg.InstrumentosNoRegistrados)
                    If tableNoRegis.Rows.Count > 0 Then
                        rep.SetDataSource(oInstNoreg)
                        rep.SetParameterValue("Usuario", Usuario)
                        rep.SetParameterValue("FechaOperacion", sFechaOperacion)
                        rep.SetParameterValue("Portafolio", sPortafolioCodigo)
                        rep.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio)
                        rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
                        'rutaArchivo = ruta + tPortafolio(i - 1) + "_noreg.pdf"
                        rutaArchivo = ruta + dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim + "_noreg.pdf"
                        archivosEntrada = archivosEntrada & rutaArchivo & "!"
                        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
                    End If
                    Dim oInstCons As New InstCons
                    Dim tableCons As DataTable
                    Dim oInstConsTMP As DataSet
                    sCodigoCustodio = "CAVALI" '#ERROR#
                    oInstConsTMP = New CustodioArchivoBM().InstrumentosConciliados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableCons = oInstConsTMP.Tables(0)
                    sCodigoCustodio = "BBH" '#ERROR#
                    oInstConsTMP = New CustodioArchivoBM().InstrumentosConciliados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableCons.Merge(oInstConsTMP.Tables(0))
                    sCodigoCustodio = "BCRPLP" '#ERROR#
                    oInstConsTMP = New CustodioArchivoBM().InstrumentosConciliados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableCons.Merge(oInstConsTMP.Tables(0))
                    CopiarTabla(tableCons, oInstCons.InstrumentosConciliados)
                    If tableCons.Rows.Count > 0 Then
                        rep2.SetDataSource(oInstCons)
                        rep2.SetParameterValue("Usuario", MyBase.Usuario)
                        rep2.SetParameterValue("FechaOperacion", sFechaOperacion)
                        rep2.SetParameterValue("Portafolio", sPortafolioCodigo)
                        rep2.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio)
                        rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
                        'rutaArchivo = ruta + tPortafolio(i - 1) + "_concil.pdf"
                        rutaArchivo = ruta + dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim + "_concil.pdf"
                        archivosEntrada = archivosEntrada & rutaArchivo & "!"
                        rep2.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
                    End If
                    Dim tableDife As DataTable
                    Dim oInstDifeTMP As DataSet
                    Dim oInstDife As New InstDife
                    sCodigoCustodio = "CAVALI" '#ERROR#
                    oInstDifeTMP = New CustodioArchivoBM().InstrumentosDiferencias(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableDife = oInstDifeTMP.Tables(0)
                    sCodigoCustodio = "BBH" '#ERROR#
                    oInstDifeTMP = New CustodioArchivoBM().InstrumentosDiferencias(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
                    tableDife.Merge(oInstDifeTMP.Tables(0))
                    CopiarTabla(tableDife, oInstDife.InstrumentosDiferencias)
                    If tableDife.Rows.Count > 0 Then
                        rep3.SetDataSource(oInstDife)
                        rep3.SetParameterValue("Usuario", MyBase.Usuario)
                        rep3.SetParameterValue("FechaOperacion", sFechaOperacion)
                        rep3.SetParameterValue("Portafolio", sPortafolioCodigo)
                        rep3.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio)
                        rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
                        'rutaArchivo = ruta + tPortafolio(i - 1) + "_difere.pdf"
                        rutaArchivo = ruta + dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim + "_difere.pdf"
                        archivosEntrada = archivosEntrada & rutaArchivo & "!"
                        rep3.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
                    End If
                    Dim dtConsulta As New DataTable
                    Dim dsconsulta As New InstDifVarios
                    Dim drconsulta As DataRow
                    sCodigoCustodio = ""
                    dtConsulta = New CustodioArchivoBM().InstrumentosDiferenciasVarios(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest).Tables(0)
                    If dtConsulta.Rows.Count > 0 Then
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
                        rep4.SetDataSource(dsconsulta)
                        rep4.SetParameterValue("FechaOperacion", sFechaOperacion)
                        rep4.SetParameterValue("Usuario", Usuario)
                        rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
                        'rutaArchivo = ruta + tPortafolio(i - 1) + "_difeVar.pdf"
                        rutaArchivo = ruta + dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim + "_difeVar.pdf"
                        archivosEntrada = archivosEntrada & rutaArchivo & "!"
                        rep4.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
                    End If
                Next
                archivoSalida = ruta + "InstrumentosUnificados.pdf"
                MergePDF(archivosEntrada, archivoSalida)
                lblrutaArchivo.Text = archivoSalida
                lblrutaArchivo.Visible = True
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + archivoSalida)
                Response.WriteFile(archivoSalida)
                Response.End()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página de impresión")
        End Try
    End Sub
    Private Sub MergePDF(ByVal archivosEntrada As String, ByVal archivoSalida As String)
        Dim archivoEntrada() = archivosEntrada.Split("!")
        Dim i As Byte
        Dim MemStream As New System.IO.MemoryStream
        Dim doc As New iTextSharp.text.Document(PageSize.A4, 33, 0, 5, 0)
        Dim reader As iTextSharp.text.pdf.PdfReader
        Dim numberOfPages As Integer
        Dim currentPageNumber As Integer
        Dim PdfPath As String = archivoSalida
        Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, New FileStream(PdfPath, FileMode.Create))
        doc.Open()
        Dim cb As iTextSharp.text.pdf.PdfContentByte = writer.DirectContent
        Dim page As iTextSharp.text.pdf.PdfImportedPage
        Dim rotation As Integer
        Dim filename As String
        For i = 0 To archivoEntrada.Length - 2
            filename = archivoEntrada(i)
            reader = New PdfReader(filename)
            numberOfPages = reader.NumberOfPages
            currentPageNumber = 0
            Do While (currentPageNumber < numberOfPages)
                currentPageNumber += 1
                doc.SetPageSize(reader.GetPageSizeWithRotation(currentPageNumber))
                doc.NewPage()
                page = writer.GetImportedPage(reader, currentPageNumber)
                rotation = reader.GetPageRotation(currentPageNumber)
                If (rotation = 90) Or (rotation = 270) Then
                    cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(currentPageNumber).Height)
                Else
                    cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                End If
            Loop
        Next
        doc.Close()
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosUnificados_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
        rep2.Close()
        rep2.Dispose()
        rep3.Close()
        rep3.Dispose()
        rep4.Close()
        rep4.Dispose()
    End Sub
End Class