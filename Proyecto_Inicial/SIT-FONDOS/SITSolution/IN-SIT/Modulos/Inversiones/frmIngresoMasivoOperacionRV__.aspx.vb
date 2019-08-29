Imports System.Runtime.InteropServices.Marshal
Imports ParametrosSIT
Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports System.IO
Imports Microsoft.Office
Imports System.Threading
Imports System.Globalization
Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionRV__
    Inherits BasePage
#Region "Rutinas"
    Private Sub GenerarReporteRentaVariable()
        Dim oldCulture As CultureInfo
        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
        Try
            Dim sFile As String, sTemplate As String
            Dim dtRentaVariable As New DataTable
            Dim dtResumen As New DataTable
            Dim dtResumen2 As New DataTable
            Dim dtResumenSBS As New DataTable
            Dim dtPatrimFondos As New DataTable
            Dim oDs As New DataSet
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            oDs = oPrevOrdenInversionBM.GenerarReporte(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            dtRentaVariable = oDs.Tables(0)
            dtResumen = oDs.Tables(2)
            dtResumen2 = oDs.Tables(3)
            dtResumenSBS = oDs.Tables(4)
            dtPatrimFondos = oDs.Tables(5)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'CMB OT 62087 20110427 REQ 6
            Dim n As Integer
            Dim n2 As Long
            Dim dr As DataRow
            Dim i As Integer = 0
            Dim m As Integer = 0
            Dim j As Integer = 0
            Dim k As Integer = 0
            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet, oSheetSBS As Excel.Worksheet
            Dim oCells As Excel.Range
            If File.Exists(sFile) Then
                File.Delete(sFile)
            End If
            sTemplate = RutaPlantillas() & "\" & "PlantillaPrevOrdenInversionRV.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(2, 17) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Merge()
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Font.Name = "Arial"
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Font.Bold = True
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Font.Size = 13
            oSheet.SaveAs(sFile)
            n = 8
            Dim nCorrela As Decimal = 0
            Dim nCorrela2 As Decimal = 0
            Dim correlativo As Integer = 0
            Dim cantidadOperacionHoja1 As Decimal = 0
            Dim totalOperacionHoja1 As Decimal = 0
            Dim codigoPrevOrdenHoja1 As Integer = 0
            'Hoja 1
            For Each dr In dtRentaVariable.Rows
                If Integer.Parse(dr("CodigoPrevOrden").ToString.Trim) <> codigoPrevOrdenHoja1 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 2) = dr("Correlativo")
                    codigoPrevOrdenHoja1 = Integer.Parse(dr("CodigoPrevOrden").ToString.Trim)
                    oCells(n, 3) = dr("HoraOperacion")
                    oCells(n, 4) = dr("UsuarioCreacion")
                    oCells(n, 5) = dr("CodigoNemonico")
                    oCells(n, 6) = dr("Instrumento")
                    oCells(n, 7) = dr("Operacion")
                    oCells(n, 8) = dr("Cantidad")
                    oCells(n, 8).NumberFormat = "###,###,##0"
                    oCells(n, 9) = dr("Precio")
                    oCells(n, 9).NumberFormat = "###,###,##0.0000"
                    oCells(n, 10) = dr("Total")
                    oCells(n, 10).NumberFormat = "###,###,##0.00"
                    oCells(n, 11) = dr("Condicion")
                    oCells(n, 12) = dr("Intermediario")
                    oCells(n, 13) = dr("Plaza")
                    oCells(n, 14) = "Fondo " & Replace(dr("TipoFondo").ToString.Trim, "IN-FONDO", "")
                    oCells(n, 15) = dr("IntervaloPrecio")
                    oCells(n, 15).NumberFormat = "###,###,##0.0000000"
                    cantidadOperacionHoja1 = 0
                    totalOperacionHoja1 = 0
                    If dr("CantidadOperacion").ToString.Trim <> "" And dr("CodigoNemonico") <> "" Then
                        cantidadOperacionHoja1 = cantidadOperacionHoja1 + Decimal.Parse(dr("CantidadOperacion"))
                    End If

                    If dr("TotalOperacionRV").ToString.Trim <> "" And dr("CodigoNemonico") <> "" Then
                        totalOperacionHoja1 = totalOperacionHoja1 + Decimal.Parse(dr("TotalOperacionRV"))
                    End If
                    oCells(n, 16) = cantidadOperacionHoja1
                    oCells(n, 16).NumberFormat = "###,###,##0"
                    oCells(n, 17) = dr("PrecioOperacion")
                    oCells(n, 17).NumberFormat = "###,###,##0.0000"
                    oCells(n, 18) = totalOperacionHoja1
                    oCells(n, 18).NumberFormat = "###,###,##0.00"
                    oSheet.Columns("E:E").EntireColumn.AutoFit()
                    oSheet.Columns("H:M").EntireColumn.AutoFit()
                    oSheet.Columns("O:R").EntireColumn.AutoFit()
                    n = n + 1
                End If
                If Integer.Parse(dr("CodigoPrevOrden").ToString.Trim) = codigoPrevOrdenHoja1 And dr("CodigoNemonico") = "" Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 16) = dr("CantidadOperacion")
                    oCells(n, 16).NumberFormat = "###,###,##0"
                    oCells(n, 17) = dr("PrecioOperacion")
                    oCells(n, 17).NumberFormat = "###,###,##0.0000"
                    oCells(n, 18) = dr("TotalOperacionRV")
                    oCells(n, 18).NumberFormat = "###,###,##0.00"
                    n = n + 1
                End If
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            'REPORTE Hoja 2 Registro de Órdenes
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(6, 9) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 10
            Dim codigoPrevOrdenHoja2 As Integer = 0
            Dim correlativoHoja2 As Integer = 0
            Dim swFormato As Integer = 0
            m = 0
            For i = 0 To dtRentaVariable.Rows.Count - 1
                If n >= 29 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                End If
                If dtRentaVariable.Rows(i)("Correlativo") <> correlativoHoja2 And dtRentaVariable.Rows(i)("CodigoPrevOrden") <> codigoPrevOrdenHoja2 Then
                    oCells(n, 2) = dtRentaVariable.Rows(i)("Operacion")
                    oCells(n, 3) = dtRentaVariable.Rows(i)("Instrumento")
                    oCells(n, 5) = dtRentaVariable.Rows(i)("CodigoNemonico")
                    oCells(n, 6) = dtRentaVariable.Rows(i)("HoraOperacion")
                    oCells(n, 7) = dtRentaVariable.Rows(i)("CantidadOperacion")
                    oCells(n, 8) = dtRentaVariable.Rows(j)("DescripcionFondo").ToString.Trim
                    codigoPrevOrdenHoja2 = Integer.Parse(dtRentaVariable.Rows(i)("CodigoPrevOrden").ToString.Trim)
                    oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 8).Interior.Color = RGB(204, 255, 204)
                    swFormato = 1
                    n = n + 1
                    For k = 0 To dtRentaVariable.Rows.Count - 1
                        If dtRentaVariable.Rows(k)("CodigoPrevOrden") = dtRentaVariable.Rows(i)("CodigoPrevOrden") And dtRentaVariable.Rows(k)("CodigoPortafolio") <> "" Then
                            If dtRentaVariable.Rows(k)("CantidadOperacion") <> "" Then
                                If k > m Then
                                    m = k
                                    oCells(n - 1, 7) = dtRentaVariable.Rows(k)("CantidadOperacion")
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If
            Next
            oSheet.Columns("D:E").EntireColumn.AutoFit()
            If n >= 30 Then
                oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            End If
            'REPORTE Hoja 3
            oSheet = CType(oSheets.Item(3), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(6, 9) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 9
            Dim NemonicoIntermediarioHoja3 As String = ""
            For Each dr In dtResumen.Rows
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n + 1 & ":" & n + 1).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                oCells(n, 2) = dr("CodigoNemonico")
                oCells(n, 3) = dr("Operacion")
                oCells(n, 4) = dr("Intermediario")
                oCells(n, 5) = dr("HoraOperacion")
                oCells(n, 6) = dr("PrecioPromedio")
                oCells(n, 6).NumberFormat = "###,###,##0.0000"
                oCells(n, 7) = dr("DescripcionFondo").ToString().Trim()
                oCells(n, 8) = dr("Cantidad").ToString.Trim
                oCells(n, 8).NumberFormat = "###,###,##0.0000"
                'Agregando color
                oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                oCells(n, 4).Interior.Color = RGB(204, 255, 204)
                oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                oCells(n, 8).Interior.Color = RGB(204, 255, 204)
                n = n + 1
            Next
            'REPORTE DE LA SBS Hoja 4
            oSheetSBS = CType(oSheets.Item(4), Excel.Worksheet)
            oCells = oSheetSBS.Cells
            oCells(6, 13) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheetSBS.SaveAs(sFile)
            n = 10
            For Each dr In dtResumenSBS.Rows
                n2 = n + 1
                oSheetSBS.Rows(n & ":" & n).Copy()
                oSheetSBS.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheetSBS.Application.CutCopyMode = False
                oCells(n, 2) = dr("Afp")
                oCells(n, 3) = dr("Fondo").ToString.Trim
                oCells(n, 4) = dr("Nemonico")
                oCells(n, 5) = dr("HoraOrden")
                oCells(n, 6) = dr("HoraOperacion")
                oCells(n, 7) = dr("Movimiento")
                oCells(n, 8) = dr("Cantidad")
                oCells(n, 8).NumberFormat = "###,###,##0"
                oCells(n, 9) = dr("Precio")
                oCells(n, 9).NumberFormat = "###,###,##0.0000"
                oCells(n, 10) = dr("Intermediario")
                oCells(n, 11) = dr("Resultado")
                oCells(n, 12) = dr("NumeroOperacion")
                oCells(n, 13) = dr("MedioTransmision")
                n = n + 1
            Next
            oSheetSBS.Columns("D:D").EntireColumn.AutoFit()
            oSheetSBS.Columns("H:J").EntireColumn.AutoFit()
            oSheetSBS.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            'REPORTE Hoja 5 Resumen
            oSheet = CType(oSheets.Item(5), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(5, 14) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 10
            Dim MontoAsigna As Decimal
            Dim nemonicoHoja5 As String = ""
            Dim aux As String = ""
            Dim dtportafolioHoja5 As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            For Each dr In dtResumen2.Rows
                If dr("NemonicoIntermediario").ToString.Trim <> nemonicoHoja5 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 2) = dr("CodigoNemonico")
                    oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 4).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                    nemonicoHoja5 = dr("NemonicoIntermediario").ToString.Trim
                    n = n + 1
                End If
                If dr("TotalOperacionRV") > 0 Then
                    oCells(n, 15) = dr("CantidadOperacion") 'O
                    oCells(n, 16) = dr("TotalOperacionRV") 'P
                    oCells(n, 17) = dr("Asignacion") 'Q
                    oCells(n, 17).font.color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White)
                    oCells(n, 20) = dr("CodigoOperacion") 'T
                    oCells(n, 20).font.color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White)
                    oCells(n, 21) = ObtenerMontoPatrimonio(dtPatrimFondos, dr("CodigoPortafolio").ToString) 'dtPatrimFondos.Rows(Integer.Parse(dr("CodigoPortafolio").ToString.Trim))("TotalPatrim") 'U
                    oCells(n, 21).font.color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White)
                    oCells(n, 3) = dr("DescripcionFondo").ToString.Trim() '"IN-FONDO" & dr("CodigoPortafolio").ToString.Trim ZX 20140926
                    oCells(n, 4) = "=IF(" & dr("Fondo") & "=0,"""",IF($T" & n & "=""Venta"",-1,1) * " & dr("Fondo") & ")"
                    oCells(n, 5) = "=IF(" & Decimal.Parse(dr("Asignacion").ToString.Trim) & ">=1," & Decimal.Parse(dr("TotalOperacionRV").ToString.Trim) & "/" & Integer.Parse(dr("CantidadOperacion").ToString.Trim) & "*" & "$D" & n & ",IF(ISERROR(" & Decimal.Parse(dr("TotalOperacionRV").ToString.Trim) & "*" & Decimal.Parse(dr("Asignacion").ToString.Trim) & "),""""," & Decimal.Parse(dr("TotalOperacionRV").ToString.Trim) & "*" & Decimal.Parse(dr("Asignacion").ToString.Trim) & "))"
                    oCells(n, 6) = "=IF($T" & n & "=" & """Venta""" & ",IF(ISERROR($P" & n & "/$U" & n & "),"""",-$P" & n & "/$U" & n & "),IF(ISERROR($P" & n & "/$U" & n & "),"""",$P" & n & "/$U" & n & "))"
                    oCells(n, 6).NumberFormat = "0.00%"
                    oCells(n, 7) = "=IF(ISERROR(E" & n & "/" & "$U" & n & "),"""",E" & n & "/" & "$U" & n & ")"
                    oCells(n, 7).NumberFormat = "0.00%"
                    MontoAsigna = 0
                    For Each draux In dtResumen2.Rows
                        If draux("NemonicoIntermediario").ToString.Trim = nemonicoHoja5 Then
                            MontoAsigna = MontoAsigna + CDec(draux("MAsignaF").ToString.Trim)
                        End If
                    Next
                    If MontoAsigna > 0 Then
                        oCells(n, 5) = "=IF(" & Decimal.Parse(dr("MAsignaF").ToString.Trim) & "=0,0,IF($T" & n & "=""Venta"",-1,1) * " & Decimal.Parse(dr("MAsignaF").ToString.Trim) & ")"
                    End If
                End If
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            m = 10
            'Portafolios
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 9) = dtportafolioHoja5.Rows(i)("Descripcion").ToString.Trim
                oCells(m + i, 9).Interior.Color = RGB(204, 255, 204)
            Next
            'Numero de acciones
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 10) = "=IF($I" & m + i & "="""","""",SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$D" & m & ":$D" & n - 1 & "))"
            Next
            'Monto Asignado(Nuevos soles)
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 11) = "=SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$E" & m & ":$E" & n - 1 & ")"
            Next
            'Puntos Básicos Equivalentes
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 12) = "=SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$F" & m & ":$F" & n - 1 & ")"
                oCells(m + i, 12).NumberFormat = "0.00%"
            Next
            'Puntos Básicos Asignados
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 13) = "=SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$G" & m & ":$G" & n - 1 & ")"
                oCells(m + i, 13).NumberFormat = "0.00%"
            Next
            oBook.Save()
            oBook.Close()
            oExcel.Quit()
            ReleaseComObject(oCells)
            ReleaseComObject(oSheet)
            ReleaseComObject(oSheets) : ReleaseComObject(oBook)
            ReleaseComObject(oBooks) : ReleaseComObject(oExcel)
            oExcel = Nothing : oBooks = Nothing : oBook = Nothing
            oSheets = Nothing : oSheet = Nothing : oCells = Nothing
            System.GC.Collect()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "FV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
        Thread.CurrentThread.CurrentCulture = oldCulture
    End Sub
    Public Function ObtenerMontoPatrimonio(ByVal dtPatrimonio As DataTable, ByVal codFondo As String) As String
        Dim monto As String = "0"
        For Each dr As DataRow In dtPatrimonio.Rows
            If dr("CodigoPortafolio").ToString = codFondo Then
                monto = dr("TotalPatrim").ToString
                Exit For
            End If
        Next
        Return monto
    End Function
    Public Sub EjecutarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodPrevOrden As String = "", Optional ByRef bolUpdGrilla As Boolean = False, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0)
        Dim objBM As New LimiteBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As New DataSet
        Dim dtOrdenInversion As New DataTable
        Dim bolGeneraOrden As Boolean = False
        Dim i As Long
        Dim arrCodPrevOrden As Array
        objBM.RegistrarOrdenesPreviasSeleccionadas(strTipoRenta, decNProceso)
        ds = oPrevOrdenInversionBM.GenerarOrdenInversion_Sura(strTipoRenta, decFechaOperacion, DatosRequest, claseInstrumento, decNProceso)
        If ds.Tables(0).Rows.Count <= 0 Then
            bolUpdGrilla = True
        Else
            If ds.Tables(0).Rows(0)(0).ToString.Trim = LIQUIDADO Then
                AlertaJS("Alguna operación ejecutada similar para una agrupación esta liquidada.\nDebe extornar la operación liquidada antes de ejecutar una agrupación similar.")
            Else
                Dim script As New StringBuilder
                dtOrdenInversion = ds.Tables(0)
                bolGeneraOrden = CType(ds.Tables(1).Rows(0)("GeneraOrden"), Boolean)

                arrCodPrevOrden = strCodPrevOrden.Split("|")
                For i = 0 To arrCodPrevOrden.Length - 1
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "1")
                Next

                If bolGeneraOrden Then
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
                        Dim Parametros As String = Usuario + "," + ParametrosSIT.EJECUCION_PREVOI + "," + decNProceso.ToString
                        Dim obj As New JobBM
                        Session("dtOrdenInversion") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "', '650', '450','" & btnBuscar.ClientID & "');")
                    End If
                Else
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "', '650', '450','" & btnBuscar.ClientID & "');")
                    End If
                End If
            End If
        End If
        If bolGeneraOrden = False Then
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        End If
    End Sub
    Public Sub VerificaExcesoLimites(ByVal tipoRenta As String, Optional ByVal decNProceso As Decimal = 0)
        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
        Dim parametros As String = Usuario + "," + ParametrosSIT.VALIDACION_PREVOI + "," + decNProceso.ToString
        Dim objBM As New LimiteBM
        objBM.RegistrarOrdenesPreviasSeleccionadas(tipoRenta, decNProceso)
        Dim obj As New JobBM
        'Verificar realizar prueba Job ipastor
        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_VerificaExcesoLimitesEnLinea" & System.DateTime.Now.ToString("_hhmmss"), "Verifica exceso de limites en linea, considerando el neteo de operaciones", Variable, parametros, "", "", ConfigurationManager.AppSettings(SERVIDORETL))
        'RGF 20110318
        AlertaJS(mensaje)
    End Sub
    Private Sub llenarFilaVacia(ByRef table As DataSet)
        Dim row As DataRow = table.Tables(0).NewRow()
        For Each item As DataColumn In table.Tables(0).Columns
            Select Case item.DataType
                Case GetType(String)
                    row(item.ColumnName) = String.Empty
                Case GetType(Decimal)
                    row(item.ColumnName) = 0.0
            End Select
        Next
        table.Tables(0).Rows.Add(row)
    End Sub
    Private Function ValidarIntermediario(ByVal strCodigoTercero As String) As Boolean
        Dim oTercerosBM As New TercerosBM
        Dim oTercerosBE As New TercerosBE
        Dim bolResult As Boolean = False
        oTercerosBE = oTercerosBM.Seleccionar(strCodigoTercero, DatosRequest)
        If oTercerosBE.Tables(0).Rows.Count > 0 Then
            If ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO = CType(oTercerosBE.Tables(0).Rows(0)("ClasificacionTercero"), String) Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function
    Private Function ValidarNemonico(ByVal strNemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim bolResult As Boolean = False
        Dim strTipoRenta As String = ""

        strTipoRenta = oValoresBM.SeleccionarTipoRentaPorCodigoNemonico(strNemonico)
        If strTipoRenta <> "" Then
            If ParametrosSIT.TIPO_RENTA_VARIABLE.ToString.Replace("_", " ") = strTipoRenta Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function
    Private Sub HabilitaControles(ByVal habilita As Boolean, Optional ByVal AntesApertura As Boolean = False)   'HDG OT 64480 20120120
        ibGrabar.Enabled = habilita
        ibValidar.Enabled = habilita
        ibValidarTrader.Enabled = habilita
        ibAprobar.Enabled = habilita
        Datagrid1.Enabled = habilita
        If AntesApertura Then
            ibGrabar.Enabled = True
            Datagrid1.Enabled = True
        End If
    End Sub
    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, _
    Optional ByVal strCodigoClaseInstrumento As String = "", _
    Optional ByVal strCodigoTipoInstrumentoSBS As String = "", _
    Optional ByVal strCodigoNemonico As String = "", _
    Optional ByVal strOperador As String = "", _
    Optional ByVal strEstado As String = "")
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As DataSet
        ds = oPrevOrdenInversionBM.SeleccionarPorFiltro(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado, DatosRequest)
        If ds.Tables(0).Rows.Count = 0 Then
            llenarFilaVacia(ds)
            Datagrid1.DataSource = ds
            Datagrid1.DataBind()
            Datagrid1.Rows(0).Visible = False
        Else
            Datagrid1.DataSource = ds
            Datagrid1.DataBind()
        End If
    End Sub
    Private Sub CargarEstados()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGeneralesBM.Listar(ParametrosSIT.ESTADO_PREV_OI, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, dt, "Valor", "Nombre", True)
        ddlEstado.SelectedIndex = 0
    End Sub

    Private Sub CargarOperadores()
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim dt As New DataTable
        dt = oPrevOrdenInversionBM.SeleccionarOperadores(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlOperador, dt, "UsuarioCreacion", "UsuarioCreacion", True)
        ddlOperador.SelectedIndex = 0
    End Sub

    Private Sub CargaClaseInstrumento()
        'CLASE INSTRUMENTO
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim dsClaseInstrumento As DataSet
        dsClaseInstrumento = oClaseInstrumentoBM.SeleccionarClaseInstrumentoPorTipoRenta(ParametrosSIT.TR_RENTA_VARIABLE, DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Codigo", "Descripcion", True)
        ddlClaseInstrumento.SelectedIndex = 0
    End Sub

    Private Sub CargarTipoInstrumento(ByVal codigoClaseInstrumento As String)
        'TIPO INSTRUMENTO
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim dt As New DataTable
        dt = oTipoInstrumentoBM.SeleccionarPorFiltro(codigoClaseInstrumento, "", ParametrosSIT.TR_RENTA_VARIABLE, "A", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        ddlTipoInstrumento.SelectedIndex = 0
    End Sub
    Private Sub CargarCombos()
        CargarOperadores()
        CargaClaseInstrumento()
        CargarTipoInstrumento("")
        CargarEstados()
    End Sub
    Private Sub CargarPagina()
        CargarCombos()
        hdFechaNegocio.Value = UIUtility.ObtenerFechaMaximaNegocio()
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(hdFechaNegocio.Value)
        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
        ViewState("strOperador") = ddlOperador.SelectedValue
        ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
        ViewState("strEstado") = ddlEstado.SelectedValue
        Dim dtCondicion As New DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtCondicion = oParametrosGeneralesBM.ListarCondicionPrevOI().Tables(0)
        Session("dtCondicion") = dtCondicion
        Dim dtOperacion As New DataTable
        Dim oOperacionBM As New OperacionBM
        dtOperacion = oOperacionBM.SeleccionarporClaseinstrumento("OperacionOI", ParametrosSIT.ESTADO_ACTIVO).Tables(0)
        Session("dtOperacion") = dtOperacion
        Dim dtPlazaN As New DataTable
        Dim oPlazaBM As New PlazaBM
        dtPlazaN = oPlazaBM.ListarxOrden(DatosRequest).Tables(0) 'CMB OT 64034 20111202
        Session("dtPlazaN") = dtPlazaN
        Dim dtTipoFondo As DataTable
        dtTipoFondo = New ParametrosGeneralesBM().ListarFondosInversion(DatosRequest, "M")
        Session("dtTipoFondo") = dtTipoFondo
        Dim dtTipoTramo As DataTable
        dtTipoTramo = New ParametrosGeneralesBM().Listar("TIPOTRAMO", Me.DatosRequest)
        Session("dtTipoTramo") = dtTipoTramo
        Dim dtMedioTrans As New DataTable   'HDG OT 64291 20111024
        dtMedioTrans = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_VARIABLE).Tables(0)
        Session("dtMedioTrans") = dtMedioTrans
        CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"))
        hdPuedeNegociar.Value = New PersonalBM().VerificaPermisoNegociacion(Session("Login"))
        If hdPuedeNegociar.Value = "0" Then
            HabilitaControles(False)
        End If
    End Sub

    Private Sub removerSessiones()
        Dim lbCodigoPrevOrden As Label
        Dim i As Integer
        For i = 0 To Datagrid1.Rows.Count - 1
            lbCodigoPrevOrden = CType(Datagrid1.Rows(i).FindControl("lbCodigoPrevOrden"), Label)
            Session.Remove("dtDetalleInversiones" & lbCodigoPrevOrden.Text.Trim)
        Next
    End Sub

#End Region
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
                removerSessiones()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case _PopUp.Value
                    Case "B"
                        tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0)
                    Case "GM"
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbNemonico"), TextBox).Text = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbInstrumento"), TextBox).Text = CType(Session("SS_DatosModal"), String())(2)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdClaseInstrumento"), HtmlInputHidden).Value = String.Empty
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdNemonico"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                    Case "FM"
                        CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.FooterRow.FindControl("tbInstrumentoF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(2)
                        CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden).Value = String.Empty
                        hdnOperador.Value = CType(Session("SS_DatosModal"), String())(0)
                    Case "GT"
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbIntermediario"), TextBox).Text = CType(Session("SS_DatosModal"), String())(1)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdIntermediario"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdDescTercero"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(1)
                        EjecutarJS("cambio(" + _ControlID.Value + ")")
                    Case "FT"
                        CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(1)
                        CargarDatosContactoF(CType(Session("SS_DatosModal"), String())(0))
                        hdnIntermediario.Value = CType(Session("SS_DatosModal"), String())(1)
                End Select
                _RowIndex.Value = String.Empty
                _ControlID.Value = String.Empty
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim decFechaActual As Decimal = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMdd"))

            If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
                HabilitaControles(True)
            ElseIf decFechaOperacion > hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then    'HDG OT 64480 20120120
                HabilitaControles(False)
            Else
                HabilitaControles(False)
            End If

            ViewState("decFechaOperacion") = decFechaOperacion
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strCodigoNemonico As String = tbCodigoMnemonico.Text
            Dim strEstado As String = ddlEstado.SelectedValue
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
            ViewState("strEstado") = ddlEstado.SelectedValue
            CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub Datagrid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Datagrid1.RowCommand
        Try
            If e.CommandName = "Add" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
                Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
                Dim ddlOperacionF As DropDownList
                Dim tbCantidadF As TextBox
                Dim tbPrecioF As TextBox
                Dim ddlCondicionF As DropDownList
                Dim ddlPlazaNF As DropDownList
                Dim tbIntervaloPrecioF As TextBox
                Dim tbCantidadOperacionF As TextBox
                Dim tbPrecioOperacionF As TextBox
                Dim chkPorcentajeF As CheckBox
                Dim porcentajeF As String
                Dim dtDetalleInversiones As DataTable
                Dim ddlPortafolio As DropDownList
                Dim strMensaje As String = ""
                Dim tbNemonicoF As TextBox
                Dim tbIntermediarioF As TextBox
                Dim hdIntermediarioF As HtmlControls.HtmlInputHidden
                Dim bolValidaCampos As Boolean = False
                oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                tbIntermediarioF = CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox)
                hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlControls.HtmlInputHidden)
                ddlOperacionF = CType(Datagrid1.FooterRow.FindControl("ddlOperacionF"), DropDownList)
                tbCantidadF = CType(Datagrid1.FooterRow.FindControl("tbCantidadF"), TextBox)
                tbPrecioF = CType(Datagrid1.FooterRow.FindControl("tbPrecioF"), TextBox)
                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlPlazaNF = CType(Datagrid1.FooterRow.FindControl("ddlPlazaNF"), DropDownList)
                tbIntervaloPrecioF = CType(Datagrid1.FooterRow.FindControl("tbIntervaloPrecioF"), TextBox)
                tbCantidadOperacionF = CType(Datagrid1.FooterRow.FindControl("tbCantidadOperacionF"), TextBox)
                tbPrecioOperacionF = CType(Datagrid1.FooterRow.FindControl("tbPrecioOperacionF"), TextBox)
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)
                ddlPortafolio = CType(Datagrid1.FooterRow.FindControl("ddlfondosF"), DropDownList)
                porcentajeF = "N"
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                Dim ddlContactoF As DropDownList
                Dim tbHoraF As TextBox
                Dim ddlMedioTransF As DropDownList
                Dim tbHoraEjeF As TextBox
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                ddlContactoF = CType(Datagrid1.FooterRow.FindControl("ddlContactoF"), DropDownList)
                tbHoraF = CType(Datagrid1.FooterRow.FindControl("tbHoraF"), TextBox)
                ddlMedioTransF = CType(Datagrid1.FooterRow.FindControl("ddlMedioTransF"), DropDownList)
                tbHoraEjeF = CType(Datagrid1.FooterRow.FindControl("tbHoraEjeF"), TextBox)
                If Datagrid1.FooterRow.RowType = ListItemType.Footer Then
                    If tbCantidadF.Text <> "" And _
                        tbPrecioF.Text <> "" And _
                        tbPrecioOperacionF.Text <> "" And _
                        tbCantidadOperacionF.Text <> "" And _
                        tbNemonicoF.Text <> "" And _
                        hdIntermediarioF.Value.ToString <> "" And _
                        Not ((hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) And _
                        Not (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) Then  'HDG OT 64034 04 20111111
                        If IsNumeric(tbCantidadF.Text) And _
                            IsNumeric(tbPrecioF.Text) And _
                            IsNumeric(tbCantidadOperacionF.Text) And _
                            IsNumeric(tbPrecioOperacionF.Text) Then
                            If ValidarNemonico(tbNemonicoF.Text) And ValidarIntermediario(hdIntermediarioF.Value.ToString) Then
                                bolValidaCampos = True
                            End If
                        End If
                    Else
                        If tbNemonicoF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Nemónico. \n"
                        End If
                        If tbCantidadF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Cant. Instrumento. \n"
                        End If
                        If tbPrecioF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Precio. \n"
                        End If
                        If hdIntermediarioF.Value.ToString() = "" Then
                            strMensaje = strMensaje + "- Ingrese Intermediario. \n"
                        End If
                        If tbCantidadOperacionF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Cant. Instrumento Ejecución. \n"
                        End If
                        If tbPrecioOperacionF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Precio Ejecución. \n"
                        End If
                        If (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                            strMensaje = strMensaje + "- Seleccione Tipo. \n"
                        End If
                        If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                            strMensaje = strMensaje + "- Seleccione Tipo Tramo. \n"
                        End If
                    End If
                    If bolValidaCampos = True Then
                        oRow.FechaOperacion = ViewState("decFechaOperacion")
                        oRow.HoraOperacion = tbHoraF.Text
                        oRow.CodigoNemonico = tbNemonicoF.Text
                        oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                        oRow.Cantidad = CType(tbCantidadF.Text, Decimal)
                        oRow.Precio = CType(tbPrecioF.Text, Decimal)
                        oRow.TipoCondicion = ddlCondicionF.SelectedValue
                        oRow.CodigoTercero = hdIntermediarioF.Value
                        oRow.CodigoPlaza = ddlPlazaNF.SelectedValue
                        oRow.IntervaloPrecio = CType(IIf(tbIntervaloPrecioF.Text = "", -1, tbIntervaloPrecioF.Text), Decimal)
                        oRow.CantidadOperacion = CType(tbCantidadOperacionF.Text, Decimal)
                        oRow.PrecioOperacion = CType(tbPrecioOperacionF.Text, Decimal)
                        oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                        oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                        oRow.CodigoContacto = ddlContactoF.SelectedValue
                        oRow.TipoFondo = IIf(ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoFondoF.SelectedValue)
                        oRow.TipoTramo = IIf(ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoTramoF.SelectedValue)
                        oRow.MedioNegociacion = IIf(ddlMedioTransF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlMedioTransF.SelectedValue)
                        oRow.HoraEjecucion = tbHoraEjeF.Text
                        oRow.Porcentaje = porcentajeF
                        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                        oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                        oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                        oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_RENTA_VARIABLE.ToString(), DatosRequest, dtDetalleInversiones)
                        CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                    Else
                        If Not ValidarNemonico(tbNemonicoF.Text) Then
                            strMensaje = "- El nemonico ingresado es invalido!"
                        End If
                        If Not ValidarIntermediario(hdIntermediarioF.Value) Then
                            strMensaje = strMensaje + "\n - El intermediario ingresado es invalido!"
                        End If
                        If strMensaje <> "" Then
                            AlertaJS(strMensaje)
                        End If
                    End If
                Else
                    If strMensaje <> "" Then
                        AlertaJS(strMensaje)
                    Else
                        AlertaJS("Ingrese correctamente el registro!")
                    End If
                End If
            End If
            If e.CommandName = "_Delete" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim script As String = ""
                If gvr.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Then
                    Dim dtValidaExtorno As New DataTable
                    dtValidaExtorno = oPrevOrdenInversionBM.ValidaExtorno(decCodigoPrevOrden, DatosRequest).Tables(0)
                    If dtValidaExtorno.Rows.Count > 0 Then
                        script = "Para realizar el extorno, debe revertir las siguientes operaciones: \n"
                        For Each fila As DataRow In dtValidaExtorno.Rows
                            script = script & " - " & fila("CodigoOrden").ToString() & ", " & fila("CodigoPortafolioSBS") & ", " & fila("Estado").ToString() & "\n"
                        Next
                        AlertaJS(script)
                    Else
                        EjecutarJS("showModalDialog('frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" + TR_RENTA_VARIABLE.ToString() + "&codigo=" + decCodigoPrevOrden.ToString() + "', '650', '450','" & btnBuscar.ClientID & "');")
                    End If
                ElseIf gvr.Cells(2).Text = PREV_OI_APROBADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                    CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                    CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                End If
            End If
            If e.CommandName = "Select" Then
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
                Dim script As String = ""
                EjecutarJS(UIUtility.MostrarPopUp("frmSubNivelesRentaVariable.aspx?Codigo=" & decCodigoPrevOrden.ToString(), "SubNivelesRVL", 650, 450, 90, 65, "no", "no", "yes", "yes"), False)
            End If
            If e.CommandName = "Item" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
                Dim hdIntermediario As HtmlControls.HtmlInputHidden
                Dim tbNemonico As TextBox
                hdIntermediario = CType(gvr.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
                If hdIntermediario.Value.Trim <> "" Then
                    Dim ddlContacto As DropDownList
                    Dim objContacto As New ContactoBM
                    Dim dtContacto As DataSet
                    ddlContacto = CType(gvr.FindControl("ddlContacto"), DropDownList)
                    dtContacto = objContacto.ListarContactoPorTerceros(hdIntermediario.Value)
                    If dtContacto.Tables.Count > 0 Then
                        ddlContacto = CType(gvr.FindControl("ddlContacto"), DropDownList)
                        HelpCombo.LlenarComboBox(ddlContacto, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
                        ddlContacto.SelectedValue = ""
                    Else
                        ddlContacto.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                    End If
                End If
                tbNemonico = CType(gvr.FindControl("tbNemonico"), TextBox)
                Dim hdNemonico As HtmlControls.HtmlInputHidden
                hdNemonico = CType(gvr.FindControl("hdNemonico"), HtmlControls.HtmlInputHidden)
                If hdNemonico.Value <> "" Then
                    tbNemonico.Text = hdNemonico.Value
                End If
                Dim hdDescTercero As HtmlControls.HtmlInputHidden
                Dim tbIntermediario As TextBox
                hdDescTercero = CType(gvr.FindControl("hdDescTercero"), HtmlControls.HtmlInputHidden)
                tbIntermediario = CType(gvr.FindControl("tbIntermediario"), TextBox)
                If hdDescTercero.Value <> "" Then
                    tbIntermediario.Text = hdDescTercero.Value
                End If
                If tbNemonico.Text.Trim <> "" Then
                    Dim ddlTipoFondo As DropDownList
                    Dim ddlTipoTramo As DropDownList
                    Dim tbHora As TextBox
                    Dim tbInstrumento As TextBox
                    Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
                    tbInstrumento = CType(gvr.FindControl("tbInstrumento"), TextBox)
                    hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                    If hdClaseInstrumento.Value.Trim = "" Then
                        hdClaseInstrumento.Value = tbInstrumento.Text.Trim.Substring(0, 2)
                        tbInstrumento.Text = tbInstrumento.Text.Trim.Substring(2)
                    End If
                    ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                    ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                    If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondo.Enabled = True
                        If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                            ddlTipoTramo.Enabled = True
                        Else
                            ddlTipoTramo.Enabled = False
                            ddlTipoTramo.SelectedIndex = 0
                        End If
                    Else
                        ddlTipoFondo.Enabled = False
                        ddlTipoTramo.Enabled = False
                        ddlTipoFondo.SelectedIndex = 0
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                    tbHora = CType(gvr.FindControl("tbHora"), TextBox)
                    tbHora.Text = Now.ToString("hh:mm:ss")
                End If

            End If
            If e.CommandName = "TipoFondo" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    Else
                        ddlTipoTramo.Enabled = False
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "Footer" Then
                Dim hdIntermediarioF As HtmlControls.HtmlInputHidden
                Dim tbNemonicoF As TextBox
                Dim tbIntermediarioF As TextBox
                hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlControls.HtmlInputHidden)
                tbIntermediarioF = CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox)
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                tbNemonicoF.Text = Me.hdnOperador.Value
                If hdIntermediarioF.Value.Trim <> "" Then
                    tbIntermediarioF.Text = hdnIntermediario.Value
                    CargarDatosContactoF(hdIntermediarioF.Value)
                End If

                If tbNemonicoF.Text.Trim <> "" Then
                    Dim ddlTipoFondoF As DropDownList
                    Dim ddlTipoTramoF As DropDownList
                    Dim tbHoraF As TextBox
                    Dim tbHoraEjeF As TextBox
                    Dim tbInstrumentoF As TextBox
                    Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
                    tbInstrumentoF = CType(Datagrid1.FooterRow.FindControl("tbInstrumentoF"), TextBox)
                    hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                    If hdClaseInstrumentoF.Value.Trim = "" Then
                        hdClaseInstrumentoF.Value = tbInstrumentoF.Text.Trim.Substring(0, 2)
                        tbInstrumentoF.Text = tbInstrumentoF.Text.Trim.Substring(2)
                    End If
                    ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                    ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                    If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondoF.Enabled = True
                        If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                            ddlTipoTramoF.Enabled = True
                        Else
                            ddlTipoTramoF.Enabled = False
                            ddlTipoTramoF.SelectedIndex = 0
                        End If
                    Else
                        ddlTipoFondoF.Enabled = False
                        ddlTipoTramoF.Enabled = False
                        ddlTipoFondoF.SelectedIndex = 0
                        ddlTipoTramoF.SelectedIndex = 0
                    End If
                    tbHoraF = CType(Datagrid1.FooterRow.FindControl("tbHoraF"), TextBox)
                    tbHoraF.Text = New UtilDM().RetornarHoraSistema
                    tbHoraEjeF = CType(Datagrid1.FooterRow.FindControl("tbHoraEjeF"), TextBox)
                    tbHoraEjeF.Text = New UtilDM().RetornarHoraSistema
                End If
            End If
            If e.CommandName = "TipoFondoF" Then
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden

                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramoF.Enabled = True
                    Else
                        ddlTipoTramoF.Enabled = False
                        ddlTipoTramoF.SelectedIndex = 0
                    End If
                End If
            End If
            'CA
            If e.CommandName = "Condicion" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim ddlCondicion As DropDownList
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
                Dim hdTotal As HtmlControls.HtmlInputHidden
                Dim tbTotal As TextBox
                tbTotal = CType(gvr.FindControl("tbTotal"), TextBox)
                hdTotal = CType(gvr.FindControl("hdTotal"), HtmlControls.HtmlInputHidden)
                If hdTotal.Value <> "" Then
                    tbTotal.Text = hdTotal.Value
                End If
                ddlCondicion = CType(gvr.FindControl("ddlCondicion"), DropDownList)
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoTramo.Enabled = True
                    ddlTipoFondo.SelectedValue = TIPOFONDO_ETF
                    If ddlCondicion.SelectedValue = PRINCIPAL_TRADE Then
                        ddlTipoTramo.SelectedValue = TIPO_TRAMO_PRINCIPAL
                    Else
                        ddlTipoTramo.SelectedValue = TIPO_TRAMO_AGENCIA
                    End If
                End If
            End If
            If e.CommandName = "CondicionF" Then
                Dim ddlCondicionF As DropDownList
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
                Dim hdTotalF As HtmlControls.HtmlInputHidden
                Dim tbTotalF As TextBox
                tbTotalF = CType(Datagrid1.FooterRow.FindControl("tbTotalF"), TextBox)
                hdTotalF = CType(Datagrid1.FooterRow.FindControl("hdTotalF"), HtmlControls.HtmlInputHidden)
                If hdTotalF.Value <> "" Then
                    tbTotalF.Text = hdTotalF.Value
                End If
                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoTramoF.Enabled = True
                    ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF
                    If ddlCondicionF.SelectedValue = ParametrosSIT.PRINCIPAL_TRADE Then
                        ddlTipoTramoF.SelectedValue = ParametrosSIT.TIPO_TRAMO_PRINCIPAL
                    Else
                        ddlTipoTramoF.SelectedValue = ParametrosSIT.TIPO_TRAMO_AGENCIA
                    End If
                End If
            End If
            If e.CommandName = "asignarfondo" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim chkPorcentaje As CheckBox
                Dim porcentaje As String
                chkPorcentaje = CType(gvr.FindControl("chkPorcentaje"), CheckBox)
                If chkPorcentaje.Checked Then
                    porcentaje = "S"
                Else
                    porcentaje = "N"
                End If
                EjecutarJS(UIUtility.MostrarPopUp("frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentaje, "", 650, 450, 90, 65, "no", "no", "yes", "yes"), False)
            End If
            If e.CommandName = "asignarfondoF" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim chkPorcentajeF As CheckBox
                Dim porcentajeF As String
                chkPorcentajeF = CType(gvr.FindControl("chkPorcentajeF"), CheckBox)
                If chkPorcentajeF.Checked Then
                    porcentajeF = "S"
                Else
                    porcentajeF = "N"
                End If
                EjecutarJS(UIUtility.MostrarPopUp("frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentajeF, "", 650, 450, 90, 65, "no", "no", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub

    Public Sub CargarDatosContactoF(ByVal Intermediario As String)
        Dim ddlContactoF As DropDownList
        Dim objContacto As New ContactoBM
        Dim dtContacto As DataSet

        ddlContactoF = CType(Datagrid1.FooterRow.FindControl("ddlContactoF"), DropDownList)
        dtContacto = objContacto.ListarContactoPorTerceros(Intermediario)
        If dtContacto.Tables.Count > 0 Then
            ddlContactoF = CType(Datagrid1.FooterRow.FindControl("ddlContactoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlContactoF, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
            ddlContactoF.SelectedValue = ""
        Else
            ddlContactoF.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, ""))
        End If
    End Sub

    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        Try
            If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                Dim ddlCondicion As DropDownList
                Dim ddlOperacion As DropDownList
                Dim ddlPlazaN As DropDownList
                Dim ibBNemonico As ImageButton
                Dim lbTipoCondicion2 As Label
                Dim lbOperacion As Label
                Dim lbPlazaN As Label
                Dim ddlContacto As DropDownList
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim ddlfondos As DropDownList
                Dim hdIntermediario As HtmlControls.HtmlInputHidden
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
                Dim lbContacto As Label
                Dim lbTipoFondo As Label
                Dim lbTipoTramo As Label
                Dim ddlMedioTrans As DropDownList
                Dim lbMedioTrans As Label
                Dim hdOperacionTrz As HtmlControls.HtmlInputHidden  'HDG OT 67627 20130531
                ibBNemonico = CType(e.Row.FindControl("ibBNemonico"), ImageButton)
                ibBNemonico.Attributes.Add("onclick", "javascript:showPopupMnemonicoGrilla(this);")
                lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
                ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                HelpCombo.LlenarComboBox(ddlOperacion, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
                ddlOperacion.SelectedValue = lbOperacion.Text
                hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)    'HDG OT 67627 20130531
                hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text   'HDG OT 67627 20130531
                lbTipoCondicion2 = CType(e.Row.FindControl("lbTipoCondicion2"), Label)
                ddlCondicion = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
                HelpCombo.LlenarComboBox(ddlCondicion, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
                ddlCondicion.SelectedValue = lbTipoCondicion2.Text
                ddlCondicion.Attributes.Add("onchange", "javascript:HabilitaCondicion(this);")

                lbPlazaN = CType(e.Row.FindControl("lbPlazaN"), Label)
                ddlPlazaN = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
                HelpCombo.LlenarComboBox(ddlPlazaN, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
                ddlPlazaN.SelectedValue = lbPlazaN.Text

                hdIntermediario = CType(e.Row.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
                ddlContacto = CType(e.Row.FindControl("ddlContacto"), DropDownList)
                If hdIntermediario.Value.Trim <> "" Then
                    Dim objContacto As New ContactoBM
                    Dim dtContacto As DataSet

                    dtContacto = objContacto.ListarContactoPorTerceros(hdIntermediario.Value)
                    If dtContacto.Tables.Count > 0 Then
                        lbContacto = CType(e.Row.FindControl("lbContacto"), Label)
                        ddlContacto = CType(e.Row.FindControl("ddlContacto"), DropDownList)
                        HelpCombo.LlenarComboBox(ddlContacto, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
                        ddlContacto.SelectedValue = lbContacto.Text
                    Else
                        ddlContacto.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                    End If
                Else
                    ddlContacto.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                End If
                lbTipoFondo = CType(e.Row.FindControl("lbTipoFondo"), Label)
                ddlTipoFondo = CType(e.Row.FindControl("ddlTipoFondo"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoFondo, CType(Session("dtTipoFondo"), DataTable), "Valor", "Nombre", False)
                ddlTipoFondo.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
                ddlTipoFondo.SelectedValue = IIf(lbTipoFondo.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbTipoFondo.Text)
                lbTipoTramo = CType(e.Row.FindControl("lbTipoTramo"), Label)
                ddlTipoTramo = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoTramo, CType(Session("dtTipoTramo"), DataTable), "Valor", "Nombre", False)
                ddlTipoTramo.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
                ddlTipoTramo.SelectedValue = IIf(lbTipoTramo.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbTipoTramo.Text)
                ddlfondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
                CargaPortafolio(ddlfondos)
                Dim hdPortafolioSel As HtmlControls.HtmlInputHidden = CType(e.Row.FindControl("hdPortafolioSel"), HtmlControls.HtmlInputHidden)
                ddlfondos.SelectedValue = hdPortafolioSel.Value
                ddlTipoFondo.Attributes.Add("onchange", "javascript:HabilitaTipoTramo(this);")
                hdClaseInstrumento = CType(e.Row.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoFondo.Enabled = True
                    If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    End If
                End If
                lbMedioTrans = CType(e.Row.FindControl("lbMedioTrans"), Label)
                ddlMedioTrans = CType(e.Row.FindControl("ddlMedioTrans"), DropDownList)
                HelpCombo.LlenarComboBox(ddlMedioTrans, CType(Session("dtMedioTrans"), DataTable), "Valor", "Nombre", False)
                ddlMedioTrans.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
                ddlMedioTrans.SelectedValue = IIf(lbMedioTrans.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbMedioTrans.Text)
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    ddlCondicion.Enabled = False
                    ddlOperacion.Enabled = False
                    ddlPlazaN.Enabled = False
                    ddlContacto.Enabled = False
                    ddlTipoFondo.Enabled = False
                    ddlTipoTramo.Enabled = False
                    ddlfondos.Enabled = False
                    ddlMedioTrans.Enabled = False
                    ibBNemonico.Enabled = False
                    Dim tbCantidad As TextBox
                    Dim tbPrecio As TextBox
                    Dim tbIntervaloPrecio As TextBox
                    Dim tbCantidadOperacion As TextBox
                    Dim tbPrecioOperacion As TextBox
                    Dim tbTotalOperacion As TextBox
                    Dim tbIntermediario As TextBox
                    Dim tbNemonico As TextBox
                    Dim chkSelect As CheckBox
                    Dim chkPorcentaje As CheckBox
                    Dim hdPorcentaje As HtmlControls.HtmlInputHidden
                    Dim Imagebutton1 As ImageButton
                    Dim tbHora As TextBox
                    Dim tbHoraEje As TextBox
                    Dim tbTotal As TextBox
                    Imagebutton1 = CType(e.Row.FindControl("Imagebutton1"), ImageButton)
                    tbCantidad = CType(e.Row.FindControl("tbCantidad"), TextBox)
                    tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                    tbIntervaloPrecio = CType(e.Row.FindControl("tbIntervaloPrecio"), TextBox)
                    tbCantidadOperacion = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
                    tbPrecioOperacion = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
                    tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                    tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                    tbNemonico = CType(e.Row.FindControl("tbNemonico"), TextBox)
                    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                    hdPorcentaje = CType(e.Row.FindControl("hdPorcentaje"), HtmlControls.HtmlInputHidden)
                    chkPorcentaje = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
                    tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                    tbHoraEje = CType(e.Row.FindControl("tbHoraEje"), TextBox)
                    tbTotal = CType(e.Row.FindControl("tbTotal"), TextBox)
                    tbCantidad.Enabled = False
                    tbPrecio.Enabled = False
                    tbIntervaloPrecio.Enabled = False
                    tbCantidadOperacion.Enabled = False
                    tbPrecioOperacion.Enabled = False
                    tbTotalOperacion.Enabled = False
                    tbIntermediario.Enabled = False
                    tbNemonico.Enabled = False
                    chkSelect.Enabled = False
                    If hdPorcentaje.Value = "S" Then
                        chkPorcentaje.Checked = True
                    Else
                        chkPorcentaje.Checked = False
                    End If
                    Imagebutton1.Enabled = False
                    tbHora.Enabled = False
                    tbHoraEje.Enabled = False
                    tbTotal.Enabled = False
                    If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                        e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Then
                        chkSelect.Enabled = True
                    End If
                    If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                        Imagebutton1.Enabled = True
                    End If
                End If
                Dim tbHora2 As TextBox
                Dim tbNemonico2 As TextBox
                Dim ddlOperacion2 As DropDownList
                Dim tbCantidad2 As TextBox
                Dim tbPrecio2 As TextBox
                Dim tbTotal2 As TextBox
                Dim tbIntermediario2 As TextBox
                Dim ddlContacto2 As DropDownList
                Dim ddlMedioTrans2 As DropDownList
                Dim ddlTipoTramo2 As DropDownList
                Dim ddlPlazaN2 As DropDownList
                Dim tbIntervaloPrecio2 As TextBox
                Dim tbHoraEje2 As TextBox
                Dim tbCantidadOperacion2 As TextBox
                Dim tbPrecioOperacion2 As TextBox
                Dim tbTotalOperacion2 As TextBox
                tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
                tbNemonico2 = CType(e.Row.FindControl("tbNemonico"), TextBox)
                ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                tbCantidad2 = CType(e.Row.FindControl("tbCantidad"), TextBox)
                tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
                tbTotal2 = CType(e.Row.FindControl("tbTotal"), TextBox)
                tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                ddlContacto2 = CType(e.Row.FindControl("ddlContacto"), DropDownList)
                ddlMedioTrans2 = CType(e.Row.FindControl("ddlMedioTrans"), DropDownList)
                ddlTipoTramo2 = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
                ddlPlazaN2 = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
                tbIntervaloPrecio2 = CType(e.Row.FindControl("tbIntervaloPrecio"), TextBox)
                tbHoraEje2 = CType(e.Row.FindControl("tbHoraEje"), TextBox)
                tbCantidadOperacion2 = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
                tbPrecioOperacion2 = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
                tbTotalOperacion2 = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                tbHora2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbNemonico2.Attributes.Add("onpropertychange", "javascript:cambio(this);")
                ddlOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbCantidad2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbPrecio2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTotal2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbIntermediario2.Attributes.Add("onpropertychange", "javascript:cambio(this);")
                ddlContacto2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMedioTrans2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlTipoTramo2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlPlazaN2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbIntervaloPrecio2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbHoraEje2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbCantidadOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbPrecioOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTotalOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
            End If
            If e.Row.RowType = ListItemType.Footer Then
                Dim ddlCondicionF As DropDownList
                Dim ddlOperacionF As DropDownList
                Dim ddlPlazaNF As DropDownList
                Dim tbOperadorF As TextBox
                Dim ddlfondos As DropDownList
                Dim ddlContactoF As DropDownList
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                Dim ddlMedioTransF As DropDownList
                tbOperadorF = CType(e.Row.FindControl("tbOperadorF"), TextBox)
                tbOperadorF.Text = Usuario.ToString.Trim
                ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
                ddlOperacionF.SelectedIndex = 0
                ddlCondicionF = CType(e.Row.FindControl("ddlCondicionF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlCondicionF, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
                ddlCondicionF.SelectedIndex = 0
                ddlCondicionF.Attributes.Add("onchange", "javascript:HabilitaCondicionF(this);")
                ddlPlazaNF = CType(e.Row.FindControl("ddlPlazaNF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlPlazaNF, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
                ddlPlazaNF.SelectedIndex = 0
                ddlContactoF = CType(e.Row.FindControl("ddlContactoF"), DropDownList)
                ddlContactoF.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                ddlContactoF.SelectedIndex = 0
                ddlTipoFondoF = CType(e.Row.FindControl("ddlTipoFondoF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoFondoF, CType(Session("dtTipoFondo"), DataTable), "Valor", "Nombre", False)
                ddlTipoFondoF.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
                ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF
                ddlTipoTramoF = CType(e.Row.FindControl("ddlTipoTramoF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoTramoF, CType(Session("dtTipoTramo"), DataTable), "Valor", "Nombre", False)
                ddlTipoTramoF.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
                ddlTipoTramoF.SelectedValue = ParametrosSIT.TIPO_TRAMO_PRINCIPAL
                ddlfondos = CType(e.Row.FindControl("ddlfondosF"), DropDownList)
                CargaPortafolio(ddlfondos)
                ddlTipoFondoF.Attributes.Add("onchange", "javascript:HabilitaTipoTramoF(this);")
                ddlMedioTransF = CType(e.Row.FindControl("ddlMedioTransF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlMedioTransF, CType(Session("dtMedioTrans"), DataTable), "Valor", "Nombre", False)
                ddlMedioTransF.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
                ddlMedioTransF.SelectedValue = ParametrosSIT.MEDIO_TRANS_TELF
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub
    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        objportafolio = Nothing
    End Sub
    Public Function instanciarTabla(ByVal tabla As DataTable) As DataTable
        tabla = New DataTable
        tabla.Columns.Add("CodigoPrevOrden")
        tabla.Columns.Add("CodigoPortafolio")
        tabla.Columns.Add("Asignacion")
        tabla.Columns.Add("Cambio")
        Return tabla
    End Function
    Protected Sub ibGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibGrabar.Click
        Try
            Dim sw As Integer = 0
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim bolValidaCampos As Boolean = False
            Dim lbCodigoPrevOrden As Label
            Dim tbHora As TextBox
            Dim tbNemonico As TextBox
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim ddlOperacion As DropDownList
            Dim tbCantidad As TextBox
            Dim tbPrecio As TextBox
            Dim ddlCondicion As DropDownList
            Dim ddlPlazaN As DropDownList
            Dim ddlPortafolio As DropDownList
            Dim tbIntervaloPrecio As TextBox
            Dim tbCantidadOperacion As TextBox
            Dim tbPrecioOperacion As TextBox
            Dim chkPorcentaje As CheckBox
            Dim porcentaje As String
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
            Dim ddlTipoFondo As DropDownList
            Dim ddlTipoTramo As DropDownList
            Dim ddlContacto As DropDownList
            Dim NroOper As String = ""
            Dim strMensaje As String = ""
            Dim ddlMedioTrans As DropDownList
            Dim tbHoraEje As TextBox
            Dim hdCambio As HtmlControls.HtmlInputHidden
            Dim oTrazabilidadOperacionBE As New TrazabilidadOperacionBE
            Dim hdCambioTraza As HtmlControls.HtmlInputHidden
            Dim hdCambioTrazaFondo As HtmlControls.HtmlInputHidden
            Dim hdNemonicoTrz As HtmlControls.HtmlInputHidden
            Dim hdOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdCantidadTrz As HtmlControls.HtmlInputHidden
            Dim hdPrecioTrz As HtmlControls.HtmlInputHidden
            Dim hdIntermediarioTrz As HtmlControls.HtmlInputHidden
            Dim hdCantidadOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdPrecioOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdFondo1Trz As HtmlControls.HtmlInputHidden
            Dim hdFondo2Trz As HtmlControls.HtmlInputHidden
            Dim hdFondo3Trz As HtmlControls.HtmlInputHidden
            Dim tbIntermediario As TextBox
            Dim strCambioTrazaFondo As String = ""
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            For Each fila As GridViewRow In Datagrid1.Rows
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                hdCambio = CType(fila.FindControl("hdCambio"), HtmlControls.HtmlInputHidden)
                hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlControls.HtmlInputHidden)
                hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlControls.HtmlInputHidden)
                chkPorcentaje = CType(fila.FindControl("chkPorcentaje"), CheckBox)
                porcentaje = "N"
                If fila.Cells(2).Text = ParametrosSIT.PREV_OI_INGRESADO Or fila.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    If (Not lbCodigoPrevOrden Is Nothing And hdCambio.Value = "1") Then
                        hdCambio.Value = ""
                        oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                        tbHora = CType(fila.FindControl("tbHora"), TextBox)
                        tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                        hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
                        ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                        tbCantidad = CType(fila.FindControl("tbCantidad"), TextBox)
                        tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                        ddlCondicion = CType(fila.FindControl("ddlCondicion"), DropDownList)
                        ddlPlazaN = CType(fila.FindControl("ddlPlazaN"), DropDownList)
                        tbIntervaloPrecio = CType(fila.FindControl("tbIntervaloPrecio"), TextBox)
                        tbCantidadOperacion = CType(fila.FindControl("tbCantidadOperacion"), TextBox)
                        tbPrecioOperacion = CType(fila.FindControl("tbPrecioOperacion"), TextBox)
                        ddlPortafolio = CType(fila.FindControl("ddlfondos"), DropDownList)
                        hdClaseInstrumento = CType(fila.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                        ddlTipoFondo = CType(fila.FindControl("ddlTipoFondo"), DropDownList)
                        ddlTipoTramo = CType(fila.FindControl("ddlTipoTramo"), DropDownList)
                        ddlContacto = CType(fila.FindControl("ddlContacto"), DropDownList)
                        NroOper = fila.Cells(1).Text
                        ddlMedioTrans = CType(fila.FindControl("ddlMedioTrans"), DropDownList)
                        tbHoraEje = CType(fila.FindControl("tbHoraEje"), TextBox)
                        bolValidaCampos = False
                        If tbCantidad.Text <> "" And _
                            tbPrecio.Text <> "" And _
                            tbPrecioOperacion.Text <> "" And _
                            tbCantidadOperacion.Text <> "" And _
                            tbNemonico.Text <> "" And _
                            hdIntermediario.Value.ToString <> "" And _
                            Not ((hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) And _
                            Not (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) Then   'HDG OT 64034 04 20111111
                            If ValidarNemonico(tbNemonico.Text) And ValidarIntermediario(hdIntermediario.Value.ToString) Then
                                If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And _
                                    fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                    fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                    bolValidaCampos = True
                                End If
                            End If
                        Else
                            If (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                                strMensaje = strMensaje + "- Seleccione Tipo. \n"
                            End If
                            If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                                strMensaje = strMensaje + "- Seleccione Tipo Tramo. \n"
                            End If
                        End If
                        If bolValidaCampos = True Then
                            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                            oRow.CodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oRow.HoraOperacion = tbHora.Text
                            oRow.CodigoNemonico = tbNemonico.Text
                            oRow.CodigoOperacion = ddlOperacion.SelectedValue
                            oRow.Cantidad = CType(tbCantidad.Text, Decimal)
                            oRow.Precio = CType(tbPrecio.Text, Decimal)
                            oRow.TipoCondicion = ddlCondicion.SelectedValue
                            oRow.CodigoTercero = hdIntermediario.Value
                            oRow.CodigoPlaza = ddlPlazaN.SelectedValue
                            oRow.IntervaloPrecio = CType(IIf(tbIntervaloPrecio.Text = "", -1, tbIntervaloPrecio.Text), Decimal)
                            oRow.CantidadOperacion = CType(tbCantidadOperacion.Text, Decimal)
                            oRow.PrecioOperacion = CType(tbPrecioOperacion.Text, Decimal)
                            oRow.CodigoContacto = ddlContacto.SelectedValue
                            oRow.TipoFondo = IIf(ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoFondo.SelectedValue)
                            oRow.TipoTramo = IIf(ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoTramo.SelectedValue)
                            oRow.MedioNegociacion = IIf(ddlMedioTrans.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlMedioTrans.SelectedValue)
                            oRow.HoraEjecucion = tbHoraEje.Text
                            oRow.Porcentaje = porcentaje
                            oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            If hdCambioTraza.Value = "1" Then
                                hdCambioTraza.Value = ""
                                hdNemonicoTrz = CType(fila.FindControl("hdNemonicoTrz"), HtmlControls.HtmlInputHidden)
                                hdOperacionTrz = CType(fila.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)
                                hdCantidadTrz = CType(fila.FindControl("hdCantidadTrz"), HtmlControls.HtmlInputHidden)
                                hdPrecioTrz = CType(fila.FindControl("hdPrecioTrz"), HtmlControls.HtmlInputHidden)
                                hdIntermediarioTrz = CType(fila.FindControl("hdIntermediarioTrz"), HtmlControls.HtmlInputHidden)
                                hdCantidadOperacionTrz = CType(fila.FindControl("hdCantidadOperacionTrz"), HtmlControls.HtmlInputHidden)
                                hdPrecioOperacionTrz = CType(fila.FindControl("hdPrecioOperacionTrz"), HtmlControls.HtmlInputHidden)
                                hdFondo1Trz = CType(fila.FindControl("hdFondo1Trz"), HtmlControls.HtmlInputHidden)
                                hdFondo2Trz = CType(fila.FindControl("hdFondo2Trz"), HtmlControls.HtmlInputHidden)
                                hdFondo3Trz = CType(fila.FindControl("hdFondo3Trz"), HtmlControls.HtmlInputHidden)
                                tbIntermediario = CType(fila.FindControl("tbIntermediario"), TextBox)
                                If hdCambioTrazaFondo.Value = "1" Then
                                    strCambioTrazaFondo = "1"
                                End If
                            End If
                        Else
                            If strMensaje <> "" Then
                                AlertaJS("Nro. Operación " + NroOper + ":\n" + strMensaje)
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next
            If oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count > 0 Then
                oPrevOrdenInversionBM.Modificar(oPrevOrdenInversionBE, DatosRequest)
                oRow = Nothing
                CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                AlertaJS("Operacion correcta.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibValidar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibValidar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim lbCodigoPrevOrden As Label
            Dim count As Integer = 0
            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login"))
            For Each fila As GridViewRow In Datagrid1.Rows
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                If Not lbCodigoPrevOrden Is Nothing Then
                    'PROCESAR VALIDACION
                    '-------------------------------------------
                    Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim decCodigoPrevOrden As Decimal

                    If chkSelect.Checked = True Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                        count = count + 1
                    End If
                End If
            Next
            'SE PROCEDE A VALIDAR
            If count > 0 Then
                VerificaExcesoLimites(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), decNProceso)
                Dim dt As New DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso).Tables(0)
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_RENTA_VARIABLE.ToString(), "ValidacionExcesos", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibAprobar.Click
        Try
            'EJECUCION DE ORDENES DE INVERSION
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim strCodPrevOrden As String = ""
            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = "APR" Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                        count = count + 1
                        strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                    End If
                End If
            Next
            strCodPrevOrden = strCodPrevOrden.Substring(0, strCodPrevOrden.Length - 1)
            If count > 0 Then
                Dim bolUpdGrilla As Boolean = False
                'rossini
                EjecutarOrdenInversion(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, bolUpdGrilla, , decNProceso)
                If bolUpdGrilla Then
                    CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"))
                End If
            Else
                AlertaJS("Seleccione el registro a ejecutar!")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If

        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibExportar.Click
        Try
            GenerarReporteRentaVariable()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibValidarTrader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibValidarTrader.Click
        Try
            'VALIDACION DE EXCESOS POR TRADER
            'crear variables globales
            Dim dtValidaTrader As New DataTable
            Dim oLimiteTradingBM As New LimiteTradingBM
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0
            'inserto el proceso masivo
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            'Se declara un contador
            Dim count As Decimal = 0
            'Se recorre un bucle en la grilla
            For Each fila As GridViewRow In Datagrid1.Rows
                'If fila.ItemType = ListItemType.Item Or fila.ItemType = ListItemType.AlternatingItem Then
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = "ING" Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        'Ejecutar limites trader
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                        count = count + 1
                    End If
                End If
            Next
            'Si la cantidad es mayor que cero
            If count > 0 Then
                'Valida excesos límite trader
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), Usuario, DatosRequest, , decNProceso).Tables(0)
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    Dim strURL As String = "frmValidacionExcesosTrader.aspx?TipoRenta=" & ParametrosSIT.TR_RENTA_VARIABLE.ToString() & "&nProc=" & decNProceso.ToString()
                    EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "');")
                Else
                    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub Datagrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Datagrid1.PageIndexChanging
        Datagrid1.PageIndex = e.NewPageIndex
        CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"))
    End Sub
End Class