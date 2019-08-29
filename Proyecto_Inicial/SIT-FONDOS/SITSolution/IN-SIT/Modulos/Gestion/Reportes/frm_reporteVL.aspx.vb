Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessLayer
Imports System.IO
Partial Class Modulos_Gestion_Reportes_frm_reporteVL
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oValorBM As New ValoresBM
    Dim oCarteraTituloValoracionBM As New CarteraTituloValoracionBM

    Sub GenerarReporteXLS_V2(ByVal Privados As Integer)
        'OT10689 - Inicio. Kill process excel
        Dim can As Integer = 0
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim sTemplate As String, sNomFile As String
        Dim sFecha As String, sHora As String, sRutaTemp As String
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim sFile As String = ""
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            sFecha = String.Format("{0:yyyyMMdd}", DateTime.Today)
            sHora = String.Format("{0:HHMMss}", DateTime.Now)
            sRutaTemp = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
            sNomFile = "ON_" & Usuario.ToString() & sFecha & sHora & ".xls"
            sFile = sRutaTemp & sNomFile
            sTemplate = RutaPlantillas() & "\" & "Plantilla_VL.xls"
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            Dim DT As DataTable = New CarteraTituloValoracionBM().ReporteVLObtener(UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text), Privados)
            If Not (DT Is Nothing) Then
                Dim ncol As Integer = 7
                oCells(4, 4) = "Fecha : " + tbFechaIni.Text
                oCells(4, 5) = "Número Registros : " + DT.Rows.Count.ToString
                For Each dr As DataRow In DT.Rows
                    oSheet.Range(ncol.ToString() + ":" + ncol.ToString()).Copy()
                    oSheet.Range((ncol + 1).ToString() + ":" + (ncol + 1).ToString()).PasteSpecial(Excel.XlPasteType.xlPasteAll)
                    oCells(ncol, 1) = IIf(dr("TipoRegistro").Equals(DBNull.Value), "", dr("TipoRegistro"))
                    oCells(ncol, 2) = IIf(dr("Administradora").Equals(DBNull.Value), "", dr("Administradora"))
                    oCells(ncol, 3) = IIf(dr("Fondo").Equals(DBNull.Value), "", dr("Fondo"))
                    oCells(ncol, 4) = IIf(dr("Fecha").Equals(DBNull.Value), "", UIUtility.ConvertirDecimalAStringFormatoFecha(dr("Fecha")))
                    oCells(ncol, 5) = IIf(dr("TipoCodigoValor").Equals(DBNull.Value), "", dr("TipoCodigoValor"))
                    oCells(ncol, 6) = IIf(dr("CodigoValor").Equals(DBNull.Value), "", dr("CodigoValor"))
                    oCells(ncol, 7) = IIf(dr("IdentificadorOperacion").Equals(0), "", dr("IdentificadorOperacion"))
                    oCells(ncol, 8) = IIf(dr("FormaValorizacion").Equals(DBNull.Value), "", dr("FormaValorizacion"))
                    oCells(ncol, 9) = IIf(dr("MontoNominal").Equals(DBNull.Value), "", dr("MontoNominal"))
                    oCells(ncol, 10) = IIf(dr("PrecioTasa").Equals(DBNull.Value), "", dr("PrecioTasa"))
                    oCells(ncol, 11) = IIf(dr("TipoCambio").Equals(DBNull.Value), "", dr("TipoCambio"))
                    oCells(ncol, 12) = IIf(dr("MontoFinal").Equals(DBNull.Value), "", dr("MontoFinal"))
                    oCells(ncol, 13) = IIf(dr("MontoInversion").Equals(DBNull.Value), "", dr("MontoInversion"))
                    oCells(ncol, 14) = UIUtility.ConvertirDecimalAStringFormatoFecha(dr("FechaOperacion"))
                    oCells(ncol, 15) = UIUtility.ConvertirDecimalAStringFormatoFecha(dr("FechaInicioPagaIntereses"))
                    oCells(ncol, 16) = UIUtility.ConvertirDecimalAStringFormatoFecha(dr("FechaVencimiento"))
                    oCells(ncol, 17) = IIf(dr("InteresesCorrido").Equals(DBNull.Value), "", dr("InteresesCorrido"))
                    oCells(ncol, 18) = IIf(dr("InteresesGanado").Equals(DBNull.Value), "", dr("InteresesGanado"))
                    oCells(ncol, 19) = IIf(dr("Ganancia_Perdida").Equals(DBNull.Value), "", dr("Ganancia_Perdida"))
                    oCells(ncol, 20) = IIf(dr("Valorizacion").Equals(DBNull.Value), "", dr("Valorizacion"))
                    oCells(ncol, 21) = IIf(dr("TipoInstrumento").Equals(DBNull.Value), "", dr("TipoInstrumento"))
                    oCells(ncol, 22) = IIf(dr("Clasificacion").Equals(DBNull.Value), "", dr("Clasificacion"))
                    oCells(ncol, 23) = IIf(dr("ComisionContado").Equals(DBNull.Value), "", dr("ComisionContado"))
                    oCells(ncol, 24) = IIf(dr("Comisionplazo").Equals(DBNull.Value), "", dr("Comisionplazo"))
                    oCells(ncol, 25) = IIf(dr("TIR").Equals(DBNull.Value), "", dr("TIR"))
                    oCells(ncol, 26) = IIf(dr("Duracion").Equals(DBNull.Value), "", dr("Duracion"))
                    oCells(ncol, 27) = IIf(dr("CodigoNemonico").Equals(DBNull.Value), "", dr("CodigoNemonico"))
                    oCells(ncol, 28) = IIf(dr("DescripcionTipoInstrumento").Equals(DBNull.Value), "", dr("DescripcionTipoInstrumento"))
                    oCells(ncol, 29) = IIf(dr("Moneda").Equals(DBNull.Value), "", dr("Moneda"))

                    'If (Not dr("MontoNominal").Equals(DBNull.Value) Or Not dr("MontoNominal").Equals("0") Or Not dr("PrecioTasa").Equals(DBNull.Value) Or Not dr("PrecioTasa").Equals("0")) Then
                    If (dr("TipoInstrumento") = "0") Then
                        If (dr("Calculo100") = "SI") Then
                            oSheet.Range("AE" & ncol).FormulaR1C1 = "=+RC[-22]*RC[-21]/100"
                        Else
                            oSheet.Range("AE" & ncol).FormulaR1C1 = "=+RC[-22]*RC[-21]"
                        End If
                        oSheet.Range("AF" & ncol).FormulaR1C1 = "=+RC[-1]*RC[-21]-RC[-12]"
                    End If

                    ncol += 1
                    can = ncol
                Next
                oSheet.SaveAs(sFile)
                oBook.Save()
                oBook.Close()
                If sFile <> "" Then
                    Response.Clear()
                    Response.ContentType = "application/xls"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
                    Response.WriteFile(sFile)
                    Response.End()
                End If
            Else
                AlertaJS("No se encontraron registros para la fecha indicada.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Function FormatoFechaddmmyyyy(ByVal Fecha As String) As String
        Try
            If Not (Fecha.Equals(DBNull.Value) Or Fecha = "" Or Fecha = "0") Then
                Dim Nfecha As String
                Dim Fdate As Date = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Fecha))
                Nfecha = Right("00" + Fdate.Day.ToString(), 2) + Right("00" + Fdate.Month.ToString(), 2) + Fdate.Year.ToString
                Return Nfecha
            Else
                Return Space(8)
            End If
        Catch ex As Exception
            Return Space(8)
        End Try
    End Function
    Sub Append(ByVal SB As StringBuilder, ByVal TipoRegistro As String, ByVal Administradora As String, ByVal Fondo As String, ByVal Fecha As String,
               ByVal TipoCodigoValor As String, ByVal CodigoValor As String, ByVal IdentificadorOperacion As String, ByVal FormaValorizacion As String,
               ByVal MontoNominal As String, ByVal PrecioTasa As String, ByVal TipoCambio As String, ByVal MontoFinal As String, ByVal MontoInversion As String,
               ByVal FechaOperacion As String, ByVal FechaInicioPagaInteres As String, ByVal FechaVencimiento As String, ByVal InteresCorrido As String,
               ByVal Interesesganados As String, ByVal GOP As String, ByVal Valorizacion As String, ByVal TipoInstrumento As String, ByVal Clasificacion As String,
               ByVal ComisionContado As String, ByVal Comisionplazo As String, ByVal TIR As String, ByVal Duracion As String)
        'En el caso de que interes ganado sea negativo
        If CDec(Interesesganados) < 1 Then
            GOP = (CDec(GOP) + CDec(Interesesganados)).ToString
            Interesesganados = "0"
        End If
        SB.AppendLine(TipoRegistro + Administradora + Fondo + Fecha + TipoCodigoValor + Left(CodigoValor + Space(12), 12) +
        Left(IdentificadorOperacion + Space(15), 15) + FormaValorizacion +
        Right("00000000000000" + Math.Round(CDec(MontoNominal), 2).ToString.Replace(".", ""), 14) +
        Right("00000000000000" + Math.Round(CDec(PrecioTasa), 6).ToString.Replace(".", ""), 14) +
        IIf(TipoCambio = "1", "0000100000000", Right("0000000000000" + Math.Round(CDec(TipoCambio), 8).ToString.Replace(".", ""), 13)) +
        Right("00000000000000" + Math.Round(CDec(MontoFinal), 2).ToString.Replace(".", ""), 14) +
        Right("00000000000000" + Math.Round(CDec(MontoInversion), 2).ToString.Replace(".", ""), 14) +
        FormatoFechaddmmyyyy(FechaOperacion) +
        FormatoFechaddmmyyyy(FechaInicioPagaInteres) +
        FormatoFechaddmmyyyy(FechaVencimiento) +
        Right("00000000000000" + Math.Round(CDec(InteresCorrido), 2).ToString.Replace(".", ""), 14) +
        Right("00000000000000" + Math.Round(CDec(Interesesganados), 2).ToString.Replace(".", ""), 14) +
        IIf(CDec(GOP) < 0, "-", "+") + Right("00000000000000" + Math.Round(CDec(GOP), 2).ToString.Replace(".", "").Replace("+", "").Replace("-", ""), 14) +
        IIf(CDec(Valorizacion) < 0, "-", "+") + Right("00000000000000" + Math.Round(CDec(Valorizacion), 2).ToString.Replace(".", "").Replace("+", "").Replace("-", ""), 14) +
        TipoInstrumento +
        Left(Clasificacion.Replace("(FF)", "").Replace("(I)", "").Trim + Space(8), 8) +
        Right("0000000000" + Math.Round(CDec(ComisionContado), 2).ToString.Replace(".", ""), 10) +
        Right("0000000000" + Math.Round(CDec(Comisionplazo), 2).ToString.Replace(".", ""), 10) +
        Right("00000000000000" + Math.Round(CDec(TIR), 6).ToString.Replace(".", ""), 14) +
        Right("00000000000000" + Math.Round(CDec(Duracion), 6).ToString.Replace(".", ""), 14))
    End Sub
    Sub GenerarReporteTXT_V2(ByVal Privados As Integer)
        Dim sRutaTemp, sFile, sNomFile, sFecha, sHora As String
        Dim DT As DataTable = New CarteraTituloValoracionBM().ReporteVLObtener(UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text), Privados)
        If DT.Rows.Count = 0 Then
            AlertaJS("No se encontraron registros para la fecha indicada.")
        Else
            Dim DFecha As Date = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(DT.Rows(0)("Fecha").ToString())))
            Dim Fecha As String = DFecha.ToShortDateString.Replace("/", "")
            sRutaTemp = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
            sFecha = String.Format("{0:yyyyMMdd}", DateTime.Today)
            sHora = String.Format("{0:HHMMss}", DateTime.Now)
            sNomFile = "VL_" & Right("00" + DFecha.Day.ToString, 2) & Right("00" + DFecha.Month.ToString, 2) & ".M19"
            sFile = sRutaTemp & sNomFile
            File.WriteAllText(sFile, "")
            Dim sw As New System.IO.StreamWriter(sFile)
            Dim SB As New StringBuilder
            Dim Cabecera As String = "CM00019VL" + Fecha + Left(DT.Rows(0)("Correlativo").ToString() + "000000", 6) + DT.Rows.Count.ToString()
            SB.AppendLine(Cabecera)
            For Each dr As DataRow In DT.Rows
                Dim GOP As Decimal = CDec(IIf(dr("Valorizacion").Equals(DBNull.Value), 0, dr("Valorizacion"))) -
                        CDec(IIf(dr("MontoInversion").Equals(DBNull.Value), 0, dr("MontoInversion"))) -
                        CDec(IIf(dr("InteresesCorrido").Equals(DBNull.Value), 0, dr("InteresesCorrido"))) -
                        CDec(IIf(dr("InteresesGanado").Equals(DBNull.Value), 0, dr("InteresesGanado")))
                Append(SB, IIf(dr("TipoRegistro").Equals(DBNull.Value), "", dr("TipoRegistro").ToString()), _
                IIf(dr("Administradora").Equals(DBNull.Value), "", dr("Administradora").ToString()), _
                IIf(dr("Fondo").Equals(DBNull.Value), "", dr("Fondo").ToString()), _
                Fecha,
                IIf(dr("TipoCodigoValor").Equals(DBNull.Value), "", dr("TipoCodigoValor")), _
                IIf(dr("CodigoValor").Equals(DBNull.Value), "", dr("CodigoValor")), _
                IIf(dr("IdentificadorOperacion").Equals(DBNull.Value), "", dr("IdentificadorOperacion").ToString()), _
                IIf(dr("FormaValorizacion").Equals(DBNull.Value), "", dr("FormaValorizacion").ToString()), _
                IIf(dr("MontoNominal").Equals(DBNull.Value), "", dr("MontoNominal").ToString()), _
                IIf(dr("PrecioTasa").Equals(DBNull.Value), "", dr("PrecioTasa").ToString()), _
                IIf(dr("TipoCambio").Equals(DBNull.Value), "", dr("TipoCambio").ToString()), _
                IIf(dr("MontoFinal").Equals(DBNull.Value), "", dr("MontoFinal").ToString()), _
                IIf(dr("MontoInversion").Equals(DBNull.Value), "", dr("MontoInversion").ToString()), _
                IIf(dr("FechaOperacion").Equals(DBNull.Value), "", dr("FechaOperacion")), _
                IIf(dr("FechaInicioPagaIntereses").Equals(DBNull.Value), "", dr("FechaInicioPagaIntereses")), _
                IIf(dr("FechaVencimiento").Equals(DBNull.Value), "", dr("FechaVencimiento")), _
                IIf(dr("InteresesCorrido").Equals(DBNull.Value), "", dr("InteresesCorrido").ToString()), _
                IIf(dr("InteresesGanado").Equals(DBNull.Value), "", dr("InteresesGanado").ToString()), GOP.ToString(), _
                IIf(dr("Valorizacion").Equals(DBNull.Value), "", dr("Valorizacion").ToString()), _
                IIf(dr("TipoInstrumento").Equals(DBNull.Value), "", dr("TipoInstrumento")), _
                IIf(dr("Clasificacion").Equals(DBNull.Value), "", dr("Clasificacion")), _
                IIf(dr("ComisionContado").Equals(DBNull.Value), "", dr("ComisionContado").ToString()), _
                IIf(dr("Comisionplazo").Equals(DBNull.Value), "", dr("Comisionplazo").ToString()), _
                IIf(dr("TipoRegistro").Equals(DBNull.Value), "", dr("TIR").ToString()), _
                IIf(dr("Duracion").Equals(DBNull.Value), "", dr("Duracion").ToString()))

            Next
            sw.Write(SB.ToString())
            sw.Close()
            Response.Clear()
            Response.ContentType = "application/txt"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
            Response.WriteFile(sFile)
            Response.End()
        End If
    End Sub
    Function ValidarFondos(ByVal Fecha As Decimal, ByVal Privado As String) As Boolean
        Dim DT As DataTable = oCarteraTituloValoracionBM.ValidadFondos(Fecha, Privado)
        Dim strMensaje As New StringBuilder
        If DT.Rows.Count > 0 Then
            strMensaje.Append("<table style=""width: 100%; text-align: left; font-weight: normal;"">")
            strMensaje.Append("<tr><td style=""width: 100%;"">No se han valorizado los siguientes Fondos:</td></tr>")
            strMensaje.Append("</table>")
            strMensaje.Append("<hr>")
            strMensaje.Append("<table style=""width: 100%; text-align: left; font-weight: normal;"">")
            For Each dr As DataRow In DT.Rows
                strMensaje.Append("<tr><td style=""width: 100%;"">" + dr(0).ToString + "</td></tr>")
            Next
            strMensaje.Append("</table>")
            AlertaJS(strMensaje.ToString())
            Return False
        End If
        Return True
    End Function
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            If ddltipo.SelectedValue = "0" Then
                GenerarReporteXLS_V2(0)
            ElseIf ddltipo.SelectedValue = "1" Then
                GenerarReporteTXT_V2(0)
            End If
        Catch ex As Exception
            AlertaJS("Ha ocurrido un error: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnImpPrivados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImpPrivados.Click
        Try
            If ddltipo.SelectedValue = "0" Then
                GenerarReporteXLS_V2(1)
            ElseIf ddltipo.SelectedValue = "1" Then
                GenerarReporteTXT_V2(1)
            End If
        Catch ex As Exception
            AlertaJS("Ha ocurrido un error: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
End Class