Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Gestion_Reportes_frmVisorGestion2
    Inherits BasePage

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim vReporte, vFondo, usuario, nombre, vMercado, FechaFin As String
        Dim vfechainicio, vfechafin As Decimal
        Dim vInstrumento As String

        vReporte = Request.QueryString("pReporte")
        Dim ruta As String
        ruta = CargarRuta()

        Try
            vInstrumento = IIf(Request.QueryString("pInstrumento") = "--Seleccione--", "", Request.QueryString("pInstrumento"))
            vFondo = IIf(Request.QueryString("pportafolio") = "--Seleccione--", "", Request.QueryString("pportafolio"))
            vfechainicio = Convert.ToDecimal(Request.QueryString("pFechaIni"))
            FechaFin = IIf(Request.QueryString("pFechaFin") Is Nothing, "", Request.QueryString("pFechaFin"))
            If FechaFin <> "" Then
                vfechafin = Convert.ToDecimal(FechaFin)
            End If

            vMercado = IIf(Request.QueryString("pMercado") = "Todos", "", Request.QueryString("pMercado"))

            nombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)

            Select Case vReporte

                Case "AUXDETAUX"
                    Dim archivosEntrada As String = ""
                    Dim archivoSalida As String
                    Dim dtConsulta As New DataTable
                    Dim ds As New RegDetContable
                    Dim dr As RegDetContable.DetalladoRow
                    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    Dim regDet As New VaxRegedBM
                    dtConsulta = regDet.SeleccionarPorCartera(vFondo, vfechainicio, DatosRequest).Tables(0)
                    For Each drv As DataRow In dtConsulta.Rows
                        dr = ds.Tables(0).NewRow()
                        dr("Portafolio") = drv("Portafolio")
                        dr("CuentaContable") = drv("CuentaContable")
                        dr("SinonimoCodigoInstrumento") = drv("SinonCodigoInstrumento")
                        dr("SinonimoCodigoEmisor") = drv("SinonCodigoEmisor")
                        dr("SaldoAnteriorMonedaLocal") = drv("SaldoAnteriorMonedaLocal")
                        dr("TotalCompras") = drv("TotalCompras")
                        dr("TotalVentas") = drv("TotalVentas")
                        dr("TotalVencimientos") = drv("TotalVencimientos")
                        dr("TotalCupones") = drv("TotalCupones")
                        dr("IndicadorCustodio") = drv("IndicadorCustodio")
                        ds.Tables(0).Rows.Add(dr)
                    Next
                    oReport.Load(Server.MapPath("RegDet.rpt"))
                    oReport.SetDataSource(ds)
                    oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & UIUtility.ConvertirFechaaString(vfechainicio) & "'"
                    oReport.DataDefinition.FormulaFields("Portafolio").Text = "'" & vFondo & "'"
                    oReport.DataDefinition.FormulaFields("Usuario").Text = "'" & usuario & "'"
                    oReport.SetDataSource(ds)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Dim rutaArchivo As String
                    rutaArchivo = ruta + "tmpRegDet.pdf"
                    archivosEntrada = archivosEntrada & rutaArchivo & "!"
                    oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)

                    Dim ds2 As New RegAuxContable
                    Dim dr2 As RegAuxContable.AuxiliarRow
                    Dim regAux As New VaxRegauxBM
                    dtConsulta = regAux.SeleccionarPorCartera(vFondo, vfechainicio, DatosRequest).Tables(0)
                    For Each drv As DataRow In dtConsulta.Rows
                        dr2 = ds2.Tables(0).NewRow()
                        dr2("Portafolio") = drv("portafolio")
                        dr2("CuentaContable") = drv("cuentacontable")
                        dr2("SinonimoCodigoInstrumento") = drv("sinoncodigoinstrumento")

                        dr2("SaldoAnteriorMonedaLocal") = Format(Convert.ToDecimal(drv("saldoanteriormonedalocal")), "###,##0.0000000")
                        dr2("SumaSaldoAnteriorMonedaLocal") = Format(Convert.ToDecimal(drv("SumaSaldoAnteriorMonedaLocal")), "###,##0.0000000")
                        dr2("GlobalSaldoAnteriorMonedaLocal") = Format(Convert.ToDecimal(drv("GlobalSaldoAnteriorMonedaLocal")), "###,##0.0000000")

                        dr2("TotalCompras") = Format(Convert.ToDecimal(drv("totalcompras")), "###,##0.0000000")
                        dr2("SumaTotalCompras") = Format(Convert.ToDecimal(drv("SumaTotalCompras")), "###,##0.0000000")
                        dr2("GlobalTotalCompras") = Format(Convert.ToDecimal(drv("GlobalTotalCompras")), "###,##0.0000000")

                        dr2("TotalVentas") = Format(Convert.ToDecimal(drv("totalventas")), "###,##0.0000000")
                        dr2("SumaTotalVentas") = Format(Convert.ToDecimal(drv("SumaTotalVentas")), "###,##0.0000000")
                        dr2("GlobalTotalVentas") = Format(Convert.ToDecimal(drv("GlobalTotalVentas")), "###,##0.0000000")

                        dr2("TotalVencimientos") = Format(Convert.ToDecimal(drv("totalvencimientos")), "###,##0.0000000")
                        dr2("SumaTotalVencimientos") = Format(Convert.ToDecimal(drv("SumaTotalVencimientos")), "###,##0.0000000")
                        dr2("GlobalTotalVencimientos") = Format(Convert.ToDecimal(drv("GlobalTotalVencimientos")), "###,##0.0000000")

                        dr2("TotalCupones") = Format(Convert.ToDecimal(drv("totalcupones")), "###,##0.0000000")
                        dr2("SumaTotalCupones") = Format(Convert.ToDecimal(drv("SumaTotalCupones")), "###,##0.0000000")
                        dr2("GlobalTotalCupones") = Format(Convert.ToDecimal(drv("GlobalTotalCupones")), "###,##0.0000000")

                        dr2("TotalRentabilidad") = Format(Convert.ToDecimal(drv("totalrentabilidad")), "###,##0.0000000")
                        dr2("SumaTotalRentabilidad") = Format(Convert.ToDecimal(drv("SumaTotalRentabilidad")), "###,##0.0000000")
                        dr2("GlobalTotalRentabilidad") = Format(Convert.ToDecimal(drv("GlobalTotalRentabilidad")), "###,##0.0000000")

                        dr2("SaldoDelDia1") = Format(Convert.ToDecimal(drv("saldodeldia1")), "###,##0.0000000")
                        dr2("SumaSaldoDelDia1") = Format(Convert.ToDecimal(drv("SumaSaldoDelDia1")), "###,##0.0000000")
                        dr2("GlobalSumaSaldoDelDia1") = Format(Convert.ToDecimal(drv("GlobalSaldoDelDia1")), "###,##0.0000000")

                        dr2("SaldoDelDia2") = Format(Convert.ToDecimal(drv("saldodeldia2")), "###,##0.0000000")
                        dr2("SumaSaldoDelDia2") = Format(Convert.ToDecimal(drv("SumaSaldoDelDia2")), "###,##0.0000000")
                        dr2("GlobalSaldoDelDia2") = Format(Convert.ToDecimal(drv("GlobalSaldoDelDia2")), "###,##0.0000000")

                        ds2.Tables(0).Rows.Add(dr2)
                    Next
                    oReport.Load(Server.MapPath("RegAux.rpt"))
                    oReport.SetDataSource(ds2)
                    oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & UIUtility.ConvertirFechaaString(vfechainicio) & "'"
                    oReport.DataDefinition.FormulaFields("Portafolio").Text = "'" & vFondo & "'"
                    oReport.DataDefinition.FormulaFields("Usuario").Text = "'" & usuario & "'"
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    rutaArchivo = ruta + "tmpRegAux.pdf"
                    archivosEntrada = archivosEntrada & rutaArchivo & "!"
                    oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)

                    Dim ds3 As New RegAuxContable
                    Dim dr3 As RegAuxContable.AuxiliarRow
                    Dim AuxCbr As New VaxInterfazBM

                    dtConsulta = AuxCbr.SeleccionarVaxAuxbcr(vFondo, vfechainicio, DatosRequest).Tables(0)
                    For Each drv As DataRow In dtConsulta.Rows
                        dr3 = ds3.Tables(0).NewRow()
                        dr3("SinonimoCodigoInstrumento") = drv("CodigoCuentaBCR")
                        dr3("TotalCompras") = Format(Convert.ToDecimal(drv("TotalMontoDolares")), "###,##0.0000000")
                        dr3("TotalVentas") = Format(Convert.ToDecimal(drv("TotalMontoSoles")), "###,##0.0000000")
                        ds3.Tables(0).Rows.Add(dr3)
                    Next
                    oReport.Load(Server.MapPath("AuxBcr.rpt"))
                    oReport.SetDataSource(ds3)
                    oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & UIUtility.ConvertirFechaaString(vfechainicio) & "'"
                    oReport.DataDefinition.FormulaFields("Portafolio").Text = "'" & vFondo & "'"
                    oReport.DataDefinition.FormulaFields("Usuario").Text = "'" & usuario & "'"
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    rutaArchivo = ruta + "tmpAuxBcr.pdf"
                    archivosEntrada = archivosEntrada & rutaArchivo
                    oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
                    archivoSalida = ruta + "AUXDETAUX.pdf"
                    MergePDF(archivosEntrada, archivoSalida)

                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + archivoSalida)
                    Response.WriteFile(archivoSalida)
                    Response.End()
            End Select
        Catch ex As Exception
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
        For i = 0 To archivoEntrada.Length - 1
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

End Class
