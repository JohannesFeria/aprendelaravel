Imports Microsoft.VisualBasic
Imports System.Data
Imports Sit.BusinessLayer

Public Class GenerarReporteRenta

    Public Shared Function GenerarReporteRentaVariablePDF(ByVal strCategoriaReporte As String, ByVal user As String, ByVal request As DataSet) As String
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasRV.rpt"
        oReport.Load(fileReport)
        Dim dsAux As DataSet
        Dim dsReporteOperacionesMasivasRV As New DsReporteOperacionesMasivasRV
        Dim decNProceso As Decimal = 0  'HDG OT 67554 duplicado
        decNProceso = oPrevOrdenInversion.InsertarProcesoMasivo(user) 'HDG OT 67554 duplicado
        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_RENTA_VARIABLE, fechaNegocio, request, "", strCategoriaReporte, decNProceso)  'HDG OT 67554 duplicado
        oPrevOrdenInversion.EliminarProcesoMasivo(decNProceso)  'HDG OT 67554 duplicado
        CopyTable(dsAux.Tables(0), dsReporteOperacionesMasivasRV.RegistroPrevio)
        'Firmas
        Dim drFirma As DsReporteOperacionesMasivasRV.FirmaRow
        Dim drRutaFirma As DataRow
        drRutaFirma = dsAux.Tables(1).Rows(0)
        drFirma = CType(dsReporteOperacionesMasivasRV.Firma.NewFirmaRow(), DsReporteOperacionesMasivasRV.FirmaRow)
        drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(0), String)))
        drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(1), String)))
        drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(2), String)))
        drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(3), String)))
        drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(4), String)))
        drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(5), String)))
        drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(6), String)))
        drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(7), String)))
        drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(8), String)))
        drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(9), String)))
        drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(10), String)))
        drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(11), String)))
        dsReporteOperacionesMasivasRV.Firma.AddFirmaRow(drFirma)
        dsReporteOperacionesMasivasRV.Firma.AcceptChanges()

        dsReporteOperacionesMasivasRV.Merge(dsReporteOperacionesMasivasRV, False, System.Data.MissingSchemaAction.Ignore)

        oReport.SetDataSource(dsReporteOperacionesMasivasRV)
        oReport.SetParameterValue("@Usuario", user)
        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))

        Dim rutaArchivo As String = ""

        If Not (oReport Is Nothing) Then
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", request).Rows(0)("Valor") & "RV_" & user.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
        End If

        Return rutaArchivo
    End Function

    Public Shared Function GenerarReporteRentaVariableAFPDF(ByVal strCategoriaReporte As String, ByVal user As String, ByVal request As DataSet) As String
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasRV2.rpt"
        oReport.Load(fileReport)
        Dim dsAux As DataSet
        Dim dsReporteOperacionesMasivasRV2 As New DsReporteOperacionesMasivasRV2
        Dim decNProceso As Decimal = 0  'HDG OT 67554 duplicado
        decNProceso = oPrevOrdenInversion.InsertarProcesoMasivo(user) 'HDG OT 67554 duplicado
        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_RENTA_VARIABLE, fechaNegocio, request, "", strCategoriaReporte, decNProceso)  'HDG OT 67554 duplicado
        oPrevOrdenInversion.EliminarProcesoMasivo(decNProceso)  'HDG OT 67554 duplicado
        CopyTable(dsAux.Tables(0), dsReporteOperacionesMasivasRV2.RegistroPrevio)
        'Firmas
        Dim drFirma As DsReporteOperacionesMasivasRV2.FirmaRow
        Dim drRutaFirma As DataRow
        drRutaFirma = dsAux.Tables(1).Rows(0)
        drFirma = CType(dsReporteOperacionesMasivasRV2.Firma.NewFirmaRow, DsReporteOperacionesMasivasRV2.FirmaRow)
        drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(0), String)))
        drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(1), String)))
        drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(2), String)))
        drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(3), String)))
        drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(4), String)))
        drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(5), String)))
        drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(6), String)))
        drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(7), String)))
        drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(8), String)))
        drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(9), String)))
        drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(10), String)))
        drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(11), String)))
        dsReporteOperacionesMasivasRV2.Firma.AddFirmaRow(drFirma)
        dsReporteOperacionesMasivasRV2.Firma.AcceptChanges()

        dsReporteOperacionesMasivasRV2.Merge(dsReporteOperacionesMasivasRV2, False, System.Data.MissingSchemaAction.Ignore)

        oReport.SetDataSource(dsReporteOperacionesMasivasRV2)
        oReport.OpenSubreport("rptReporteOperacionesMasivasRV2SR").SetDataSource(dsReporteOperacionesMasivasRV2)
        oReport.SetParameterValue("@Usuario", user)
        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))

        Dim rutaArchivo As String = ""

        If Not (oReport Is Nothing) Then
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", request).Rows(0)("Valor") & "RV2_" & user.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
        End If

        Return rutaArchivo
    End Function

    Public Shared Function GenerarReporteRentaFijaPDF(ByVal strCategoriaReporte As String, ByVal user As String, ByVal request As DataSet) As String
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasRF.rpt"
        oReport.Load(fileReport)
        Dim dsAux As DataSet
        Dim dsReporteOperacionesMasivasRF As New DsReporteOperacionesMasivasRF
        Dim decNProceso As Decimal = 0  'HDG OT 67554 duplicado
        decNProceso = oPrevOrdenInversion.InsertarProcesoMasivo(user) 'HDG OT 67554 duplicado
        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_RENTA_FIJA, fechaNegocio, request, "", strCategoriaReporte, decNProceso)  'HDG OT 67554 duplicado
        oPrevOrdenInversion.EliminarProcesoMasivo(decNProceso)  'HDG OT 67554 duplicado
        CopyTable(dsAux.Tables(0), dsReporteOperacionesMasivasRF.RegistroPrevio)
        'Firmas
        Dim drFirma As DsReporteOperacionesMasivasRF.FirmaRow
        Dim drRutaFirma As DataRow
        drRutaFirma = dsAux.Tables(1).Rows(0)
        drFirma = CType(dsReporteOperacionesMasivasRF.Firma.NewFirmaRow(), DsReporteOperacionesMasivasRF.FirmaRow)
        drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(0), String)))
        drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(1), String)))
        drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(2), String)))
        drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(3), String)))
        drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(4), String)))
        drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(5), String)))
        drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(6), String)))
        drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(7), String)))
        drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(8), String)))
        drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(9), String)))
        drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(10), String)))
        drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(11), String)))
        drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(12), String)))
        drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(13), String)))
        drFirma.Firma15 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(14), String)))
        drFirma.Firma16 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(15), String)))
        drFirma.Firma17 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(16), String)))
        drFirma.Firma18 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(17), String)))
        drFirma.Firma19 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(18), String)))
        drFirma.Firma20 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(19), String)))
        dsReporteOperacionesMasivasRF.Firma.AddFirmaRow(drFirma)
        dsReporteOperacionesMasivasRF.Firma.AcceptChanges()

        dsReporteOperacionesMasivasRF.Merge(dsReporteOperacionesMasivasRF, False, System.Data.MissingSchemaAction.Ignore)

        oReport.SetDataSource(dsReporteOperacionesMasivasRF)
        oReport.SetParameterValue("@Usuario", user)
        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))

        Dim rutaArchivo As String = ""

        If Not (oReport Is Nothing) Then
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", request).Rows(0)("Valor") & "RF_" & user.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
        End If
        Return rutaArchivo
    End Function

    Public Shared Function GenerarReporteFXPDF(ByVal strCategoriaReporte As String, ByVal user As String, ByVal request As DataSet) As String
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasFX.rpt"
        oReport.Load(fileReport)
        Dim dsAux As DataSet
        Dim dsReporteOperacionesMasivasFX As New DsReporteOperacionesMasivasFX
        Dim decNProceso As Decimal = 0
        decNProceso = oPrevOrdenInversion.InsertarProcesoMasivo(user)
        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_DERIVADOS, fechaNegocio, request, "", strCategoriaReporte, decNProceso)  'HDG OT 67554 duplicado
        oPrevOrdenInversion.EliminarProcesoMasivo(decNProceso)
        CopyTable(dsAux.Tables(0), dsReporteOperacionesMasivasFX.RegistroPrevio)
        'Firmas
        Dim drFirma As DsReporteOperacionesMasivasFX.FirmaRow
        Dim drRutaFirma As DataRow
        drRutaFirma = dsAux.Tables(1).Rows(0)
        drFirma = CType(dsReporteOperacionesMasivasFX.Firma.NewFirmaRow(), DsReporteOperacionesMasivasFX.FirmaRow)
        drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(0), String)))
        drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(1), String)))
        drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(2), String)))
        drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(3), String)))
        drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(4), String)))
        drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(5), String)))
        drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(6), String)))
        drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(7), String)))
        drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(8), String)))
        drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(9), String)))
        drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(10), String)))
        drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(11), String)))
        drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(12), String)))
        drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(13), String)))
        drFirma.Firma15 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(14), String)))
        drFirma.Firma16 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(15), String)))
        drFirma.Firma17 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(16), String)))
        drFirma.Firma18 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(17), String)))
        drFirma.Firma19 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(18), String)))
        drFirma.Firma20 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(19), String)))
        dsReporteOperacionesMasivasFX.Firma.AddFirmaRow(drFirma)
        dsReporteOperacionesMasivasFX.Firma.AcceptChanges()

        dsReporteOperacionesMasivasFX.Merge(dsReporteOperacionesMasivasFX, False, System.Data.MissingSchemaAction.Ignore)

        oReport.SetDataSource(dsReporteOperacionesMasivasFX)
        oReport.SetParameterValue("@Usuario", user)
        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))

        Dim rutaArchivo As String = ""

        If Not (oReport Is Nothing) Then
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", request).Rows(0)("Valor") & "FX_" & user.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
        End If

        Return rutaArchivo
    End Function

    Private Shared Sub CopyTable(ByRef dtSource As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtSource.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
            End Try
        Next
    End Sub
End Class
