Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Partial Class Modulos_Tesoreria_Reportes_frmReporteRegistroInversiones
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oOrdenPrevOrden As New OrdenPreOrdenInversionBM
    Dim contador As Integer
    Private Sub CargarPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        For Each dr In dt.Rows
            contador += 1
            oSheet.Rows(FilaInicial.ToString & ":" & FilaInicial.ToString).Copy()
            oSheet.Rows((FilaInicial + 1).ToString & ":" & (FilaInicial + 1).ToString).Insert(Excel.XlDirection.xlDown)
            oCells(FilaInicial, 2) = dr("CodigoOrden")
            oCells(FilaInicial, 3) = dr("Operacion")
            oCells(FilaInicial, 4) = dr("CodigoEntidad")
            oCells(FilaInicial, 5) = dr("CodigoISIN")
            oCells(FilaInicial, 6) = dr("CodigoTipoTitulo")
            oCells(FilaInicial, 7) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaOperacion")))
            oCells(FilaInicial, 8) = dr("CantidadOperacion")
            oCells(FilaInicial, 9) = dr("Precio")
            oCells(FilaInicial, 10) = dr("CodigoMoneda")
            oCells(FilaInicial, 11) = dr("MontoNominalOperacion")
            oCells(FilaInicial, 12) = dr("MontoOperacion")
            oCells(FilaInicial, 13) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaVencimiento")))
            oCells(FilaInicial, 14) = dr("Tercero")
            oCells(FilaInicial, 15) = dr("CodigoPlaza")
            oCells(FilaInicial, 16) = dr("Mecanismo")
            oCells(FilaInicial, 17) = dr("TasaPorcentaje")
            oCells(FilaInicial, 18) = dr("TasaEfectiva")
            oCells(FilaInicial, 19) = dr("TotalComisiones")
            oCells(FilaInicial, 20) = dr("RestoComisiones")
            oCells(FilaInicial, 21) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaLiquidacion")))
            oCells(FilaInicial, 22) = dr("SUBYACENTE")
            oCells(FilaInicial, 23) = dr("ValorEjecucion")
            oCells(FilaInicial, 24) = dr("Prima")
            oCells(FilaInicial, 25) = dr("CodigoSBS")
            FilaInicial += 1
        Next
    End Sub
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim sFile As String, sTemplate As String
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheetTemplate As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim Indice As Integer = 1
        Dim i As Int32 = 1
        Dim exitDo As Boolean = False
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RI_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaRegistroInversiones.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheetTemplate = CType(oSheets.Item(1), Excel.Worksheet)
            Do While i < ddlPortafolio.Items.Count
                contador = 0
                Dim item As ListItem
                item = ddlPortafolio.Items(i)
                If Not ddlPortafolio.SelectedValue.Equals("") Then
                    item = ddlPortafolio.SelectedItem
                    exitDo = True
                End If
                Dim dsRegInv As DataSet = oOrdenPrevOrden.Reporte_RegistroInversiones(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), item.Value)
                oSheet = oBook.Worksheets.Add(After:=oBook.Worksheets(i))
                oSheetTemplate.Range("A1:Z50").Copy(oSheet.Range("A1"))
                oSheet.Name = item.Text
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                oCells(7, 2) = "FONDO: " + dsRegInv.Tables(4).Rows(0)("NombreCompleto")
                oCells(8, 2) = "FECHA: " + tbFechaInicio.Text
                oCells(2, 14) = Now.ToLongDateString
                oCells(3, 14) = Usuario
                'Tabla 1 OPERACIONES AL CONTADO Y DEPÓSITOS
                Dim dtOpecontado As DataTable = dsRegInv.Tables(0)
                DatosExcel(dtOpecontado, 14, oSheet, oCells)
                'Tabla 2 OPERACIONES DE REPORTE Y OPERACIONES DE PACTO
                Dim dtOpeReporte As DataTable = dsRegInv.Tables(1)
                DatosExcel(dtOpeReporte, 18 + contador, oSheet, oCells)
                'Tabla 3 CUPONES Y DIVIDENDOS COBRADOS
                Dim dtCupoDivReb As DataTable = dsRegInv.Tables(2)
                DatosExcel(dtCupoDivReb, 22 + contador, oSheet, oCells)
                'Tabla 4 DERIVADOS
                Dim dtDerivados As DataTable = dsRegInv.Tables(3)
                DatosExcel(dtDerivados, 26 + contador, oSheet, oCells)
                'oExcel.Cells.EntireColumn.AutoFit()
                oSheet.SaveAs(sFile)
                i += 1
                If exitDo Then
                    Exit Do
                End If
            Loop
            oSheetTemplate.Delete()
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "RI_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack = False) Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            CargarPortafolio()
        End If
    End Sub
    Protected Sub btnGenera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenera.Click

        If (validarRangoFechaBusqueda("3")) Then
            GenerarReporte()
        End If

    End Sub

    Protected Sub tbFechaFin_TextChanged(sender As Object, e As System.EventArgs) Handles tbFechaFin.TextChanged
        validarRangoFechaBusqueda("2")
    End Sub

    Protected Sub tbFechaInicio_TextChanged(sender As Object, e As System.EventArgs) Handles tbFechaInicio.TextChanged
        validarRangoFechaBusqueda("1")
    End Sub

    Private Function validarRangoFechaBusqueda(ByVal tipo As String) As Boolean

        'Diferencia de dias
        Dim fechaInicio As String = tbFechaInicio.Text
        Dim fechaFin As String = tbFechaFin.Text
        Dim diferenciaDias As Integer

        If fechaInicio = String.Empty Then
            AlertaJS("Ingrese una fecha de inicio.")
            Return False
        End If

        If fechaFin = String.Empty Then
            AlertaJS("Ingrese una fecha de fin.")
            Return False
        End If

        Try
            diferenciaDias = UIUtility.ObtenerDiferenciaDias(fechaInicio, fechaFin)
            If diferenciaDias < 0 Then
                tbFechaFin.Text = tbFechaInicio.Text
                If tipo <> "1" Then
                    AlertaJS("La Fecha Final debe ser mayor o igual a la fecha de inicio.")
                End If

                Return False
            ElseIf diferenciaDias > 30 Then
                If tipo <> "1" Then
                    AlertaJS("El rango de la fecha de inicio y fecha fin no puede ser mayor a 30 dias.")
                End If

                tbFechaFin.Text = tbFechaInicio.Text
                Return False
            End If

        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error en el cambio de fecha de operación / " + ex.Message.ToString, "'", String.Empty))
            Return False
        End Try
        Return True
    End Function

End Class