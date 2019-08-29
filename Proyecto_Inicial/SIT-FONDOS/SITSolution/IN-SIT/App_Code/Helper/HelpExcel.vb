Imports Microsoft.VisualBasic
Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports Microsoft.Office.Core

Public Class HelpExcel
    Public xlApp As Object
    Public ProcId As Integer
    Dim bp As New BasePage

    Public Sub Kill()
        Dim Process1() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses()
        xlApp = CreateObject("Excel.Application")  'HDG 20120918
        Dim Process2() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses()
        '
        Dim ct1, ct2 As Integer
        Dim blnUnique As Boolean
        For ct1 = 0 To Process2.GetUpperBound(0)
            If Process2(ct1).ProcessName = "EXCEL" Then
                blnUnique = True
                For ct2 = 0 To Process1.GetUpperBound(0)
                    If Process1(ct2).ProcessName = "EXCEL" Then
                        If Process2(ct1).Id = Process1(ct2).Id Then
                            blnUnique = False
                            Exit For
                        End If
                    End If
                Next
                If blnUnique = True Then
                    ProcId = Process2(ct1).Id
                    Exit For
                End If
            End If
        Next
    End Sub

    Public Sub Kill2()
        Dim Process1() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses()
        Dim ct1 As Integer

        For ct1 = 0 To Process1.GetUpperBound(0)
            If Process1(ct1).ProcessName = "EXCEL" Then
                System.Diagnostics.Process.GetProcessById(Process1(ct1).Id).Kill()
            End If
        Next
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub LeerExcelyActualizarOI(ByVal archivo_incoming As String, ByVal DatosRequest As DataSet, Optional ByVal sFecha As String = "")
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oOIDatOpeDAM As OrdenInversionDatosOperacionBM
        Dim _Validar As Boolean = False
        Dim _NombreCampo As String = ""
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oSheets As Excel.Sheets
        Dim oCells As Excel.Range
        Dim dtOrdInv As DataTable
        Dim _Ref As String = ""
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim oVectorForwardSBS As New VectorForwardSBSBM
            Dim oVectorForwardSBSBE As New VectorForwardSBSBE
            Dim oVectorSwapBM As New VectorSwapBM
            Dim oVectorSwapBE As New VectorSwapBE
            Dim oRow As VectorForwardSBSBE.VectorForwardSBSRow

            Dim decFecha As Decimal = CDec(sFecha.Substring(6, 4) & sFecha.Substring(3, 2) & sFecha.Substring(0, 2))

            oOIDatOpeDAM = New OrdenInversionDatosOperacionBM
            'Libros
            oBooks = xlApp.Workbooks
            xlLibro = oBooks.Open(archivo_incoming)
            'Hojas
            oSheets = xlLibro.Worksheets
            'Hoja
            xlHoja = xlLibro.Worksheets("FORWARDS")
            oCells = xlHoja.Cells

            Dim decSPOT As Decimal = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(10), 14)).Value)

            Dim lngUltimaFila As Int64 = oCells.Columns("A:A").Range("A65536").End(Excel.XlDirection.xlUp).Row()
            Dim iCont As Int64
            Dim _PrecioForwards As Decimal = 0
            Dim _Mtm As Decimal = 0
            Dim _MtmDestino As Decimal = 0
            Dim _MtmT As Decimal = 0
            Dim _MtmDestinoT As Decimal = 0
            Dim _PrecioVector As Decimal = 0
            Dim _NocionalF As Decimal = 0

            For iCont = 1 To lngUltimaFila
                'Buscamos la columna REF
                If xlHoja.Range(String.Concat(LetraColumna(1), iCont)).Value = "REF" Then
                    iCont = iCont + 1
                    While Convert.ToString(xlHoja.Range(String.Concat(LetraColumna(1), iCont)).Value) <> "" And iCont <= lngUltimaFila
                        'Referencia de OI
                        _Ref = Convert.ToString(xlHoja.Range(String.Concat(LetraColumna(1), iCont)).Value)
                        _NombreCampo = ""
                        _PrecioVector = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(12), iCont)).Value)

                        Try
                            'Orden de inversión de # póliza
                            dtOrdInv = oOIDatOpeDAM.ObtenerDatoOperacion_PorPoliza(_Ref)

                            If dtOrdInv.Rows.Count > 0 Then
                                'Validar el Némonico
                                If dtOrdInv.Rows(0)("CodigoMnemonico") = "FORWARD" Then
                                    'Validar que sea de FONDOS(Fondo1, Fondo2, Fondo3)
                                    If dtOrdInv.Rows(0)("CodigoPortafolioSBS") = "HO-FONDO1" Or dtOrdInv.Rows(0)("CodigoPortafolioSBS") = "HO-FONDO2" Or dtOrdInv.Rows(0)("CodigoPortafolioSBS") = "HO-FONDO3" Then
                                        If Mid(Convert.ToString(xlHoja.Range(String.Concat(LetraColumna(5), iCont)).Value), 1, 10) = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtOrdInv.Rows(0)("FechaContrato"))) Then
                                            If xlHoja.Range(String.Concat(LetraColumna(7), iCont)).Value = Convert.ToDecimal(dtOrdInv.Rows(0)("TipoCambioFuturo")) Then
                                                _Validar = True
                                            Else
                                                _NombreCampo = "STRIKE"
                                            End If
                                        Else
                                            _NombreCampo = "F.VENCIMIENTO"
                                        End If

                                        If _Validar = True Then
                                            _PrecioForwards = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(9), iCont)).Value)
                                            _Mtm = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(10), iCont)).Value)
                                            _MtmDestino = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(11), iCont)).Value)
                                            _NocionalF = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(8), iCont)).Value)

                                            _MtmT = Val(dtOrdInv.Rows(0)("MontoCancelar")) * _Mtm / _NocionalF
                                            _MtmDestinoT = Val(dtOrdInv.Rows(0)("MontoCancelar")) * _MtmDestino / _NocionalF
                                            xlHoja.Cells(iCont, 8) = dtOrdInv.Rows(0)("MontoCancelar")
                                            xlHoja.Cells(iCont, 10) = _MtmT
                                            xlHoja.Cells(iCont, 11) = _MtmDestinoT
                                            xlHoja.Cells(iCont, 10).NumberFormat = "###,###,##0.0000000"
                                            xlHoja.Cells(iCont, 11).NumberFormat = "###,###,##0.0000000"

                                            oRow = oVectorForwardSBSBE.VectorForwardSBS.NewVectorForwardSBSRow
                                            oRow.Ref = _Ref
                                            oRow.NumeroPoliza = dtOrdInv.Rows(0)("NumeroPoliza")
                                            oRow.Fecha = decFecha
                                            oRow.Mtm = _MtmT
                                            oRow.MtmDestino = _MtmDestinoT
                                            oRow.PrecioVector = _PrecioVector
                                            oRow.PrecioForward = _PrecioForwards
                                            oVectorForwardSBSBE.VectorForwardSBS.AddVectorForwardSBSRow(oRow)
                                            oVectorForwardSBSBE.VectorForwardSBS.AcceptChanges()
                                        Else
                                            bp.AlertaJS("Para la REF " + _Ref + " se encontro una \n" + "diferencia de datos en el campo \n" + _NombreCampo)
                                            Exit Sub
                                        End If
                                    Else
                                        bp.AlertaJS("Para la REF " + _Ref + " no es de ninguno de los 3 fondos")
                                        Exit Sub
                                    End If
                                Else
                                    bp.AlertaJS("Para la REF " + _Ref + " no es de orden de inversion de Forward")
                                    Exit Sub
                                End If
                            End If
                        Catch ex As Exception
                            Throw ex
                        Finally
                            dtOrdInv = Nothing
                        End Try
                        iCont = iCont + 1
                    End While
                End If
            Next iCont
            xlHoja.Columns(LetraColumna(10) & ":" & LetraColumna(11)).EntireColumn.AutoFit()    'HDG INC 66139	20120927

            xlLibro.Save()
            xlLibro.Close(SaveChanges:=False)

            If Not sFecha.Equals("") Then

                Dim oVectorTipoCambioBM As New VectorTipoCambioBM
                Dim oVectorTipoCambioBE As New VectorTipoCambio
                oVectorTipoCambioBE = CrearObjetoTipoCambioSBS("DOL", _
                                                        sFecha.Substring(6, 4) & sFecha.Substring(3, 2) & sFecha.Substring(0, 2), _
                                                        "SPOT", _
                                                        decSPOT)

                oVectorTipoCambioBM.InsertarTipoCambioSPOT(oVectorTipoCambioBE, DatosRequest)
            End If

            oVectorForwardSBS.EliminatmpVectorForwardSBS()
            Dim intNroFilas, intIndice As Integer
            intNroFilas = oVectorForwardSBSBE.VectorForwardSBS.Rows.Count
            For intIndice = 0 To intNroFilas - 1
                oRow = DirectCast(oVectorForwardSBSBE.VectorForwardSBS.Rows(intIndice), VectorForwardSBSBE.VectorForwardSBSRow)

                oVectorForwardSBS.Insertar(oRow, DatosRequest)
            Next
            oVectorForwardSBS.CargarPrecioFWD(decFecha)

            bp.AlertaJS("Se importó correctamente el precio de \n" & Space(18) & "Forwards y Swaps")
            bp.EjecutarJS("window.open('" & archivo_incoming.Replace("\", "\\") & "')")
        Catch ex As Exception
            Throw ex
        Finally
            xlApp.Quit()
            ReleaseComObject(xlApp)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub

    Public Sub CargarArchivoVectorPrecioNAV(ByVal sFileName As String, ByVal DatosRequest As DataSet, Optional ByVal sFecha As String = "")
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oSheets As Excel.Sheets = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim oInterfaseBloombergBM As New InterfaseBloombergBM
            Dim oVectorPrecioBloombergNavBE As New VectorPrecioBloombergNavBE
            Dim oRow As VectorPrecioBloombergNavBE.VectorPrecioBloombergNavRow

            Dim decFecha As Decimal = UIUtility.ConvertirFechaaDecimal(sFecha)

            'Libros
            oBooks = xlApp.Workbooks
            '1. Abrir el archivo Excel (archivo en otra carpeta)
            xlLibro = oBooks.Open(sFileName, True, True, , "")
            'Hojas
            oSheets = xlLibro.Worksheets
            'Hoja
            xlHoja = xlLibro.Worksheets("Hoja1")
            'Celdas
            oCells = xlHoja.Cells

            Dim lngUltimaFila As Int64 = 0
            Dim lngUltimaCol As Int64 = 0S
            Dim iContF As Int64
            Dim iContC As Int64
            Dim _FechaCarga As Decimal = 0
            Dim _CodigoISIN(1) As String
            Dim sCodigoISIN As String = ""

            oCells.Range("A3").Select()
            lngUltimaFila = oCells.Application.Selection.End(Excel.XlDirection.xlDown).Row()

            Dim nPos As Long = 0
            For iContC = 1 To 255
                sCodigoISIN = Convert.ToString(xlHoja.Range(String.Concat(LetraColumna(iContC), 1)).Value)
                If sCodigoISIN = String.Empty Then
                    nPos = nPos + 1
                    If nPos >= 3 Then
                        Exit For
                    End If
                Else
                    nPos = 0
                End If
            Next
            lngUltimaCol = iContC - 3
            iContC = 0
            nPos = 0

            '2. Leemos el archivo excel
            For iContC = 1 To lngUltimaCol Step 3
                nPos = nPos + 1
                sCodigoISIN = Convert.ToString(xlHoja.Range(String.Concat(LetraColumna(iContC), 1)).Value)
                _CodigoISIN(nPos - 1) = sCodigoISIN
                ReDim Preserve _CodigoISIN(_CodigoISIN.GetUpperBound(0) + 1)
            Next

            For iContF = 3 To lngUltimaFila
                'Buscamos la columna REF
                If xlHoja.Range(String.Concat(LetraColumna(1), iContF)).Value.ToString <> "" Then
                    Try
                        _FechaCarga = Convert.ToDecimal(Convert.ToDateTime(xlHoja.Range(String.Concat(LetraColumna(1), iContF)).Value).ToString("yyyyMMdd"))
                        nPos = 0
                        For iContC = 1 To lngUltimaCol Step 3
                            nPos = nPos + 1
                            oRow = oVectorPrecioBloombergNavBE.VectorPrecioBloombergNav.NewVectorPrecioBloombergNavRow
                            oRow.FechaCarga = _FechaCarga
                            oRow.CodigoISIN = _CodigoISIN(nPos - 1)
                            oRow.Precio = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(iContC + 1), iContF)).Value)
                            oVectorPrecioBloombergNavBE.VectorPrecioBloombergNav.AddVectorPrecioBloombergNavRow(oRow)
                            oVectorPrecioBloombergNavBE.VectorPrecioBloombergNav.AcceptChanges()
                        Next
                    Catch ex As Exception
                        Throw ex
                    Finally
                    End Try
                End If
            Next iContF

            xlLibro.Close(SaveChanges:=False)
            'Actualizar datos de la OI
            oInterfaseBloombergBM.InsertarPrecioNav(oVectorPrecioBloombergNavBE, DatosRequest)

            'Mostrar alerta para importación
            Dim bp As New BasePage
            bp.AlertaJS("Se importó correctamente\n" + Space(7) + "Los Precios NAV.") 'HDG 20120723
            'Cerramos el archivo Excel
        Catch ex As Exception
            Throw ex
        Finally
            xlApp.Quit()
            ReleaseComObject(xlApp)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub

    Public Sub LeerActualizarExcel(ByVal archivo_carga As String, ByVal Fondo As String, ByVal DatosRequest As DataSet)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oCells As Excel.Range = Nothing
        Dim _Ref As String = ""
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            'Libros
            oBooks = xlApp.Workbooks
            '1. Abrir el archivo Excel (archivo en otra carpeta)
            xlLibro = oBooks.Open(archivo_carga, True, True, , "")
            'Hoja
            'xlHoja = xlLibro.Worksheets(Fondo.Substring(3))
            xlHoja = xlLibro.Worksheets("Fondo" & Fondo)
            'Celdas
            oCells = xlHoja.Cells

            'Ultima fila
            Dim lngUltimaFila As Int64 = oCells.Columns("B:B").Range("B65536").End(Excel.XlDirection.xlUp).Row()
            Dim iCont As Int64

            Dim oIndicadorBenchmarkBM As New IndicadorBenchmarkBM
            Dim beIndicadorBenchmarkBE As New IndicadorBenchmarkBE
            Dim oRow As IndicadorBenchmarkBE.IndicadorBenchmarkRow

            '2. Leemos el archivo excel
            For iCont = 3 To lngUltimaFila
                'Buscamos la columna REF
                If Convert.ToString(xlHoja.Range(String.Concat(LetraColumna(4), iCont)).Value) <> "" And iCont <= lngUltimaFila Then
                    Try
                        oRow = CType(beIndicadorBenchmarkBE.IndicadorBenchmark.NewRow(), IndicadorBenchmarkBE.IndicadorBenchmarkRow)
                        _Ref = "00" & Convert.ToString((iCont - 2).ToString)
                        _Ref = _Ref.Substring(_Ref.Length - 2)

                        Dim _PorcInverObj As Decimal = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(5), iCont)).Value)
                        Dim _PorcInverMax As Decimal = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(6), iCont)).Value)
                        Dim _PorcInverMin As Decimal = Convert.ToDecimal(xlHoja.Range(String.Concat(LetraColumna(7), iCont)).Value)

                        oRow.CodigoIndicadorRent = _Ref
                        oRow.CodigoPortafolioSBS = Fondo
                        oRow.PorcInverObj = _PorcInverObj * 100
                        oRow.PorcInverMax = _PorcInverMax * 100
                        oRow.PorcInverMin = _PorcInverMin * 100
                        oRow.CodigoSubGrupo = _Ref
                        oIndicadorBenchmarkBM.Actualizar(oRow, DatosRequest)
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            Next iCont
            'Mostrar alerta para importación
            Dim bp As New BasePage
            bp.AlertaJS("Se importaron correctamente los Indicadores Benchmark")
            'Cerramos el archivo Excel
            xlLibro.Save()
            xlLibro.Close(SaveChanges:=False)
        Catch ex As Exception
            Throw ex
        Finally
            xlApp.Quit()
            ReleaseComObject(xlApp)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub

    Public Sub GenerarReporteOperacionesEPU(ByVal Usuario As String, ByVal DatosRequest As DataSet)

        Dim sFile As String, sTemplate As String
        Dim dsReporteOperacionesPU As New DataSet

        dsReporteOperacionesPU = New OrdenPreOrdenInversionBM().GenerarReporteOperacionesPU()

        Dim dtCabecera As New DataTable
        Dim dtConfirmCash As New DataTable
        Dim dtConfirmCC As New DataTable
        Dim dtExecutions As New DataTable
        Dim dtPeruCrosses As New DataTable
        Dim dtUSCrosses As New DataTable

        dtCabecera = dsReporteOperacionesPU.Tables(0)
        dtConfirmCash = dsReporteOperacionesPU.Tables(1)
        dtConfirmCC = dsReporteOperacionesPU.Tables(2)
        dtExecutions = dsReporteOperacionesPU.Tables(3)
        dtPeruCrosses = dsReporteOperacionesPU.Tables(4)
        dtUSCrosses = dsReporteOperacionesPU.Tables(5)


        Dim oDs As New DataSet

        sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "EPU_" & Usuario & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"

        Dim n As Integer
        Dim dr As DataRow
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range

        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            If File.Exists(sFile) Then File.Delete(sFile)

            sTemplate = RutaPlantillas(DatosRequest) & "\" & "PlantillaOperacionesEPU.xls"

            oExcel.Visible = False : oExcel.DisplayAlerts = False

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)

            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            oSheet.SaveAs(sFile)

            oCells(12, 2) = dsReporteOperacionesPU.Tables(6).Rows(0)(0)

            For Each dr In dtCabecera.Rows
                oCells(2, 3) = dr("TrdDate")
                oCells(2, 4) = dr("SetDate")
                oCells(4, 2) = dr("NumberUnits")
                oCells(4, 2) = dr("SharesPerUnit")
                oCells(5, 2) = dr("EstimatedCash")
                oCells(6, 2) = dr("FinalCash")
                oCells(7, 2) = dr("CashInLieu")
                oCells(8, 2) = dr("CommissionRate")
                oCells(12, 3) = dr("CreationCost")
            Next

            oCells(18, 1) = "CASH"

            n = 19

            For Each dr In dtConfirmCash.Rows
                oCells(n, 1) = dr("Currency")
                oCells(n, 2) = dr("Local")
                oCells(n, 3) = dr("VALUEDATE")
                oCells(n, 4) = dr("FXRATE")
                oCells(n, 5) = dr("USDAmount")
                oCells(n, 6) = dr("FXCross")

                n = n + 1
            Next

            n = n + 1

            oCells(n, 1) = "C*C"

            oSheet.Range(oCells(18, 1), oCells(n, 1)).Font.Bold = True
            n = n + 1

            For Each dr In dtConfirmCC.Rows
                oCells(n, 1) = dr("Currency")
                oCells(n, 2) = dr("Local")
                oCells(n, 3) = dr("VALUEDATE")
                oCells(n, 4) = dr("FXRATE")
                oCells(n, 5) = dr("USDAmount")
                oCells(n, 6) = dr("FXCross")

                n = n + 1
            Next

            oCells(n, 5) = "=SUMA(E19:E" & (n - 1) & ")"

            Dim oSheetEXE As Excel.Worksheet
            'REPORTE DE EXECUTIONS
            oSheetEXE = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheetEXE.Cells
            oSheetEXE.SaveAs(sFile)

            n = 10

            Dim Currency, Indicador As String

            Currency = ""
            Indicador = ""

            Dim K As Integer = 1
            Dim Contador As Integer = 1

            For Each dr In dtExecutions.Rows

                If K <> 1 Then
                    If Currency = dr("CURRENCY") Then
                        If Indicador <> dr("INDICADOR") Then


                            If Currency = "USD" Then
                                If Contador = 1 Then
                                    oCells(n, 15) = "=(O" & (n - 1) & ")"
                                Else
                                    oCells(n, 15) = "=SUMA(O" & (n - 1) & ":E" & (n - Contador) & ")"
                                End If
                            End If

                            If Currency = "PEN" And Indicador = "USD" Then
                                oCells(n, 11) = "PEN"
                                oCells(n, 12) = dsReporteOperacionesPU.Tables(7).Rows(0)(0)
                                n = n + 1
                                oCells(n, 11) = "TC"
                                oCells(n, 12) = dsReporteOperacionesPU.Tables(7).Rows(0)(1)
                                n = n + 1
                                oCells(n, 11) = "USD"
                                oCells(n, 12) = "=L" & (n - 2) & "/L" & (n - 1)

                                oSheet.Range(String.Concat(LetraColumna(11), n.ToString()), String.Concat(LetraColumna(12), n.ToString())).Font.Bold = True
                            End If
                            n = n + 2
                            Contador = 1
                        Else
                            Contador = Contador + 1
                        End If
                    Else
                        n = n + 1
                        Contador = 1
                    End If
                End If

                oCells(n, 1) = dr("CNTRY")
                oCells(n, 2) = dr("ACCT")
                oCells(n, 3) = dr("TRDATE")
                oCells(n, 4) = dr("RIC")
                oCells(n, 5) = dr("BLOOMBERG")
                oCells(n, 6) = dr("SEDOL")
                oCells(n, 7) = dr("ISIN")
                oCells(n, 8) = dr("COMPANYNAME")
                oCells(n, 9) = dr("BYS")
                oCells(n, 10) = dr("ExecutedShares")
                oCells(n, 11) = dr("ExecutedPrices")
                oCells(n, 12) = dr("EXCUTEGROSS")
                oCells(n, 13) = dr("NETCOMM")
                oCells(n, 14) = dr("NETTAX")
                oCells(n, 15) = dr("EXCUTENETLOCAL")
                oCells(n, 16) = dr("SETDATE")
                oCells(n, 17) = dr("CURRENCY")
                oCells(n, 18) = dr("RESIDUALQTY")
                oCells(n, 19) = dr("INDICADOR")

                Currency = dr("CURRENCY")
                Indicador = dr("INDICADOR")

                n = n + 1
                K = K + 1
            Next

            oCells(n + 1, 11) = "CASH EPU"
            oCells(n + 1, 12) = dsReporteOperacionesPU.Tables(7).Rows(0)(3)

            Dim oSheetPERU As Excel.Worksheet
            oSheetPERU = CType(oSheets.Item(3), Excel.Worksheet)
            oCells = oSheetPERU.Cells
            oSheetPERU.SaveAs(sFile)

            n = 10

            For Each dr In dtPeruCrosses.Rows
                oCells(n, 1) = dr("Cntry")
                oCells(n, 2) = dr("Acct")
                oCells(n, 3) = dr("AcctDescription")
                oCells(n, 4) = dr("TRDDATE")
                oCells(n, 5) = dr("Ric")
                oCells(n, 6) = dr("Bloomberg")
                oCells(n, 7) = dr("Sedol")
                oCells(n, 8) = dr("ISIN")
                oCells(n, 9) = dr("CompanyName")
                oCells(n, 10) = dr("ByS")
                oCells(n, 11) = dr("ExecutedShares")
                oCells(n, 12) = dr("ExecutedPrices")
                oCells(n, 13) = dr("EXCUTEGROSS")
                oCells(n, 14) = dr("NETCOMM")
                oCells(n, 15) = dr("NETTAX")
                oCells(n, 16) = dr("EXCUTENETLOCAL")
                oCells(n, 17) = dr("SETDATE")
                oCells(n, 18) = dr("CURRENCY")
                oCells(n, 19) = dr("RESIDUALQTY")
                n = n + 1
            Next

            Dim oSheetUS As Excel.Worksheet
            oSheetUS = CType(oSheets.Item(4), Excel.Worksheet)
            oCells = oSheetUS.Cells
            oSheetUS.SaveAs(sFile)

            n = 10

            For Each dr In dtUSCrosses.Rows
                oCells(n, 1) = dr("Cntry")
                oCells(n, 2) = dr("Acct")
                oCells(n, 3) = dr("AcctDescription")
                oCells(n, 4) = dr("TRDDATE")
                oCells(n, 5) = dr("Ric")
                oCells(n, 6) = dr("Bloomberg")
                oCells(n, 7) = dr("Sedol")
                oCells(n, 8) = dr("ISIN")
                oCells(n, 9) = dr("CompanyName")
                oCells(n, 10) = dr("ByS")
                oCells(n, 11) = dr("ExecutedShares")
                oCells(n, 12) = dr("ExecutedPrices")
                oCells(n, 13) = dr("EXCUTEGROSS")
                oCells(n, 14) = dr("NETCOMM")
                oCells(n, 15) = dr("NETTAX")
                oCells(n, 16) = dr("EXCUTENETLOCAL")
                oCells(n, 17) = dr("SETDATE")
                oCells(n, 18) = dr("CURRENCY")
                oCells(n, 19) = dr("RESIDUALQTY")
                n = n + 1
            Next
            oBook.Save()
            oBook.Close()

            Dim bp As New BasePage
            bp.EjecutarJS("window.open('" & sFile.Replace("\", "\\") & "')")
        Catch ex As Exception
            Throw ex
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

    Public Sub LeerExcelyRegistrarOperacionesEPU(ByVal strRuta As String, ByVal portafolio As String, ByRef strMensaje As String, ByVal DatosRequest As DataSet)
        'OT10689 - Inicio. Kill process excel
        Dim _validar As Boolean = False
        Dim _nombreCampo As String = ""
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As Excel.Worksheet
        Dim oSheets As Excel.Sheets
        Dim oCells As Excel.Range
        Dim oOrdenInversion As New OrdenPreOrdenInversionBM
        Dim oOperacion As New OperacionBM
        Dim dtOperacioneEPU As New DataTable
        Dim _Ref As String = ""

        Dim oTmpOperacionesEPUDetRow As TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow
        Dim oTmpOperacionesEPUDetBE As TmpOperacionesEPUDetBE
        Dim oTmpOperacionesEPURow As TmpOperacionesEPUBE.TmpOperacionesEPURow
        Dim oTmpOperacionesEPUBE As TmpOperacionesEPUBE
        Dim oTmpResumenOperacionesEPURow As TmpResumenOperacionesEPUBE.TmpResumenOperacionesEPURow
        Dim oTmpResumenOperacionesEPUBE As TmpResumenOperacionesEPUBE
        Dim n As Integer
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            oBooks = xlApp.Workbooks
            xlLibro = oBooks.Open(strRuta)
            oSheets = xlLibro.Worksheets

            xlHoja = xlLibro.Worksheets("Confirm")
            oCells = xlHoja.Cells

            oTmpOperacionesEPUBE = New TmpOperacionesEPUBE
            oTmpOperacionesEPURow = CType(oTmpOperacionesEPUBE.TmpOperacionesEPU.NewRow(), TmpOperacionesEPUBE.TmpOperacionesEPURow)
            oOrdenInversion.InicializarOperacionesEPU(oTmpOperacionesEPURow, DatosRequest)

            oTmpOperacionesEPURow.TrdDate = UIUtility.ConvertirFechaaDecimal(CType(xlHoja.Range(String.Concat(LetraColumna(3), 2)).Value, String))
            oTmpOperacionesEPURow.SetDate = UIUtility.ConvertirFechaaDecimal(CType(xlHoja.Range(String.Concat(LetraColumna(4), 2)).Value, String))
            oTmpOperacionesEPURow.NumberUnits = Val(xlHoja.Range(String.Concat(LetraColumna(2), 4)).Value)
            oTmpOperacionesEPURow.SharesPerUnit = Val(xlHoja.Range(String.Concat(LetraColumna(2), 5)).Value)
            oTmpOperacionesEPURow.EstimatedCash = Val(xlHoja.Range(String.Concat(LetraColumna(2), 6)).Value)
            oTmpOperacionesEPURow.FinalCash = Val(xlHoja.Range(String.Concat(LetraColumna(2), 7)).Value)
            oTmpOperacionesEPURow.CashInLieu = Val(xlHoja.Range(String.Concat(LetraColumna(2), 8)).Value)
            oTmpOperacionesEPURow.CommissionRate = Val(xlHoja.Range(String.Concat(LetraColumna(2), 9)).Value)
            oTmpOperacionesEPURow.CreationCost = Val(xlHoja.Range(String.Concat(LetraColumna(3), 12)).Value)
            oTmpOperacionesEPUBE.TmpOperacionesEPU.AddTmpOperacionesEPURow(oTmpOperacionesEPURow)
            oTmpOperacionesEPUBE.AcceptChanges()
            oOrdenInversion.InsertarOperacionesEPU(oTmpOperacionesEPUBE, DatosRequest)

            oOrdenInversion.EliminarResumenOperacionesEPU(DatosRequest)
            oTmpResumenOperacionesEPUBE = New TmpResumenOperacionesEPUBE

            n = 17
            While CType(xlHoja.Range(String.Concat(LetraColumna(1), n)).Value, String) <> ""
                oTmpResumenOperacionesEPURow = CType(oTmpResumenOperacionesEPUBE.TmpResumenOperacionesEPU.NewRow(), TmpResumenOperacionesEPUBE.TmpResumenOperacionesEPURow)
                oOrdenInversion.InicializarResumenOperacionesEPU(oTmpResumenOperacionesEPURow, DatosRequest)
                oTmpResumenOperacionesEPURow.Currency = CType(xlHoja.Range(String.Concat(LetraColumna(1), n)).Value, String)
                oTmpResumenOperacionesEPURow.LocalAmount = Val(xlHoja.Range(String.Concat(LetraColumna(2), n)).Value)
                oTmpResumenOperacionesEPURow.FXRate = Val(xlHoja.Range(String.Concat(LetraColumna(4), n)).Value)
                oTmpResumenOperacionesEPURow.USDAmount = Val(xlHoja.Range(String.Concat(LetraColumna(5), n)).Value)
                oTmpResumenOperacionesEPURow.FXCross = CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String)
                oTmpResumenOperacionesEPUBE.TmpResumenOperacionesEPU.AddTmpResumenOperacionesEPURow(oTmpResumenOperacionesEPURow)
                oTmpResumenOperacionesEPUBE.AcceptChanges()
                n = n + 1
            End While
            oOrdenInversion.InsertarResumenOperacionesEPU(oTmpResumenOperacionesEPUBE, DatosRequest)

            oOrdenInversion.EliminarOperacionesEPUDet(DatosRequest)

            oTmpOperacionesEPUDetBE = New TmpOperacionesEPUDetBE
            xlHoja = xlLibro.Worksheets("Peru Crosses")
            oCells = xlHoja.Cells

            n = 10
            While CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String) <> ""
                oTmpOperacionesEPUDetRow = CType(oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.NewRow(), TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow)
                oOrdenInversion.InicializarOperacionesEPUDet(oTmpOperacionesEPUDetRow, DatosRequest)
                oTmpOperacionesEPUDetRow.Cntry = CType(xlHoja.Range(String.Concat(LetraColumna(1), n)).Value, String)
                oTmpOperacionesEPUDetRow.Acct = CType(xlHoja.Range(String.Concat(LetraColumna(2), n)).Value, String)
                oTmpOperacionesEPUDetRow.AcctDescription = CType(xlHoja.Range(String.Concat(LetraColumna(3), n)).Value, String)
                oTmpOperacionesEPUDetRow.Ric = CType(xlHoja.Range(String.Concat(LetraColumna(5), n)).Value, String)
                oTmpOperacionesEPUDetRow.Bloomberg = CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String)
                oTmpOperacionesEPUDetRow.Sedol = CType(xlHoja.Range(String.Concat(LetraColumna(7), n)).Value, String)
                oTmpOperacionesEPUDetRow.ISIN = CType(xlHoja.Range(String.Concat(LetraColumna(8), n)).Value, String)
                oTmpOperacionesEPUDetRow.CompanyName = CType(xlHoja.Range(String.Concat(LetraColumna(9), n)).Value, String)
                oTmpOperacionesEPUDetRow.ByS = CType(xlHoja.Range(String.Concat(LetraColumna(10), n)).Value, String)
                oTmpOperacionesEPUDetRow.ExecutedShares = Val(xlHoja.Range(String.Concat(LetraColumna(11), n)).Value)
                oTmpOperacionesEPUDetRow.ExecutedPrices = Val(xlHoja.Range(String.Concat(LetraColumna(12), n)).Value)
                oTmpOperacionesEPUDetRow.NetComm = Val(xlHoja.Range(String.Concat(LetraColumna(14), n)).Value)
                oTmpOperacionesEPUDetRow.NetTax = Val(xlHoja.Range(String.Concat(LetraColumna(15), n)).Value)
                oTmpOperacionesEPUDetRow.Currency = CType(xlHoja.Range(String.Concat(LetraColumna(18), n)).Value, String)
                oTmpOperacionesEPUDetRow.ResidualQty = Val(xlHoja.Range(String.Concat(LetraColumna(19), n)).Value)
                oTmpOperacionesEPUDetRow.TipoOperacion = ParametrosSIT.EPU_PERU_CROSSES
                oTmpOperacionesEPUDetRow.CodigoPortafolioSBS = portafolio
                oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.AddTmpOperacionesEPUDetRow(oTmpOperacionesEPUDetRow)
                oTmpOperacionesEPUBE.AcceptChanges()
                n = n + 1
            End While
            oOrdenInversion.InsertarOperacionesEPUDet(oTmpOperacionesEPUDetBE, DatosRequest)

            oTmpOperacionesEPUDetBE = New TmpOperacionesEPUDetBE
            xlHoja = xlLibro.Worksheets("US Crosses")
            oCells = xlHoja.Cells
            n = 10
            While CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String) <> ""
                oTmpOperacionesEPUDetRow = CType(oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.NewRow(), TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow)
                oOrdenInversion.InicializarOperacionesEPUDet(oTmpOperacionesEPUDetRow, DatosRequest)
                oTmpOperacionesEPUDetRow.Cntry = CType(xlHoja.Range(String.Concat(LetraColumna(1), n)).Value, String)
                oTmpOperacionesEPUDetRow.Acct = CType(xlHoja.Range(String.Concat(LetraColumna(2), n)).Value, String)
                oTmpOperacionesEPUDetRow.AcctDescription = CType(xlHoja.Range(String.Concat(LetraColumna(3), n)).Value, String)
                oTmpOperacionesEPUDetRow.Ric = CType(xlHoja.Range(String.Concat(LetraColumna(5), n)).Value, String)
                oTmpOperacionesEPUDetRow.Bloomberg = CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String)
                oTmpOperacionesEPUDetRow.Sedol = CType(xlHoja.Range(String.Concat(LetraColumna(7), n)).Value, String)
                oTmpOperacionesEPUDetRow.ISIN = CType(xlHoja.Range(String.Concat(LetraColumna(8), n)).Value, String)
                oTmpOperacionesEPUDetRow.CompanyName = CType(xlHoja.Range(String.Concat(LetraColumna(9), n)).Value, String)
                oTmpOperacionesEPUDetRow.ByS = CType(xlHoja.Range(String.Concat(LetraColumna(10), n)).Value, String)
                oTmpOperacionesEPUDetRow.ExecutedShares = Val(xlHoja.Range(String.Concat(LetraColumna(11), n)).Value)
                oTmpOperacionesEPUDetRow.ExecutedPrices = Val(xlHoja.Range(String.Concat(LetraColumna(12), n)).Value)
                oTmpOperacionesEPUDetRow.NetComm = Val(xlHoja.Range(String.Concat(LetraColumna(14), n)).Value)
                oTmpOperacionesEPUDetRow.NetTax = Val(xlHoja.Range(String.Concat(LetraColumna(15), n)).Value)
                oTmpOperacionesEPUDetRow.Currency = CType(xlHoja.Range(String.Concat(LetraColumna(18), n)).Value, String)
                oTmpOperacionesEPUDetRow.ResidualQty = Val(xlHoja.Range(String.Concat(LetraColumna(19), n)).Value)
                oTmpOperacionesEPUDetRow.TipoOperacion = ParametrosSIT.EPU_US_CROSSES
                oTmpOperacionesEPUDetRow.CodigoPortafolioSBS = portafolio
                oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.AddTmpOperacionesEPUDetRow(oTmpOperacionesEPUDetRow)
                oTmpOperacionesEPUBE.AcceptChanges()
                n = n + 1
            End While
            oOrdenInversion.InsertarOperacionesEPUDet(oTmpOperacionesEPUDetBE, DatosRequest)

            oTmpOperacionesEPUDetBE = New TmpOperacionesEPUDetBE
            xlHoja = xlLibro.Worksheets("Executions")
            oCells = xlHoja.Cells
            n = 10
            While CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String) <> ""
                oTmpOperacionesEPUDetRow = CType(oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.NewRow(), TmpOperacionesEPUDetBE.TmpOperacionesEPUDetRow)
                oOrdenInversion.InicializarOperacionesEPUDet(oTmpOperacionesEPUDetRow, DatosRequest)
                oTmpOperacionesEPUDetRow.Cntry = CType(xlHoja.Range(String.Concat(LetraColumna(1), n)).Value, String)
                oTmpOperacionesEPUDetRow.Acct = CType(xlHoja.Range(String.Concat(LetraColumna(2), n)).Value, String)
                oTmpOperacionesEPUDetRow.Ric = CType(xlHoja.Range(String.Concat(LetraColumna(4), n)).Value, String)
                oTmpOperacionesEPUDetRow.Bloomberg = CType(xlHoja.Range(String.Concat(LetraColumna(5), n)).Value, String)
                oTmpOperacionesEPUDetRow.Sedol = CType(xlHoja.Range(String.Concat(LetraColumna(6), n)).Value, String)
                oTmpOperacionesEPUDetRow.ISIN = CType(xlHoja.Range(String.Concat(LetraColumna(7), n)).Value, String)
                oTmpOperacionesEPUDetRow.CompanyName = CType(xlHoja.Range(String.Concat(LetraColumna(8), n)).Value, String)
                oTmpOperacionesEPUDetRow.ByS = CType(xlHoja.Range(String.Concat(LetraColumna(9), n)).Value, String)
                oTmpOperacionesEPUDetRow.ExecutedShares = Val(xlHoja.Range(String.Concat(LetraColumna(10), n)).Value)
                oTmpOperacionesEPUDetRow.ExecutedPrices = Val(xlHoja.Range(String.Concat(LetraColumna(11), n)).Value)
                oTmpOperacionesEPUDetRow.NetComm = Val(xlHoja.Range(String.Concat(LetraColumna(13), n)).Value)
                oTmpOperacionesEPUDetRow.NetTax = Val(xlHoja.Range(String.Concat(LetraColumna(14), n)).Value)
                oTmpOperacionesEPUDetRow.Currency = CType(xlHoja.Range(String.Concat(LetraColumna(17), n)).Value, String)
                oTmpOperacionesEPUDetRow.ResidualQty = Val(xlHoja.Range(String.Concat(LetraColumna(18), n)).Value)
                oTmpOperacionesEPUDetRow.TipoOperacion = ParametrosSIT.EPU_CASH
                oTmpOperacionesEPUDetBE.TmpOperacionesEPUDet.AddTmpOperacionesEPUDetRow(oTmpOperacionesEPUDetRow)
                oTmpOperacionesEPUBE.AcceptChanges()
                n = n + 1
            End While
            oOrdenInversion.InsertarOperacionesEPUDet(oTmpOperacionesEPUDetBE, DatosRequest)
            xlLibro.Save()
            xlLibro.Close()
            oBooks.Close()
        Catch ex As Exception
            Throw ex
        Finally
            xlApp.Quit()
            ReleaseComObject(xlApp)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub


    Protected Function RutaPlantillas(DatosRequest As DataSet) As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("018", DatosRequest)
        Return oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
    End Function

    Private Function CrearObjetoTipoCambioSBS( _
                                                ByVal CodigoMoneda As String, _
                                                ByVal Fecha As Decimal, _
                                                ByVal EntidadExt As String, _
                                                ByVal ValorPrimario As Decimal) As VectorTipoCambio

        Dim oVectorTipoCambioBE As New VectorTipoCambio
        Dim oRow As VectorTipoCambio.VectorTipoCambioRow

        oRow = CType(oVectorTipoCambioBE.VectorTipoCambio.NewRow(), VectorTipoCambio.VectorTipoCambioRow)

        oRow.CodigoMoneda() = CodigoMoneda
        oRow.Fecha() = Fecha
        oRow.EntidadExt() = EntidadExt
        oRow.ValorPrimario() = ValorPrimario

        oVectorTipoCambioBE.VectorTipoCambio.AddVectorTipoCambioRow(oRow)
        oVectorTipoCambioBE.VectorTipoCambio.AcceptChanges()

        Return oVectorTipoCambioBE
    End Function

    Public Shared Function LetraColumna(ByVal numeroColumna As Integer) As String
        Dim VectorLetra() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}
        Return VectorLetra(numeroColumna)
    End Function
End Class