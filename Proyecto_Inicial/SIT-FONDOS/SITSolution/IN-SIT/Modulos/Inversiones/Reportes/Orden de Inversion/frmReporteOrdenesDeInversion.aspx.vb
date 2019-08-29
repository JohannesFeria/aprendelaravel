Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports System.Collections.Generic
Imports System.Text
Imports System.Data
Imports System.Threading
'Imports System.Globalization
Partial Class Modulos_Inversiones_Reportes_Orden_de_Inversion_frmReporteOrdenesDeInversion
    Inherits BasePage
    Dim objutil As New UtilDM
#Region " /* Eventos de la Página */ "
    Dim oPortafolioBM As New PortafolioBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Me.tbFechaFin.Text = objutil.RetornarFechaSistema()
                Me.tbFechaInicio.Text = objutil.RetornarFechaSistema()
                CargarPortafolio()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Sub LlenarSesionContextInfo()
        Dim tablaParametros As New Hashtable
        If (ddlPortafolio.SelectedIndex >= 0) Then
            tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
            tablaParametros("Portafolio") = ddlPortafolio.SelectedItem.Text
        End If
        ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
        Session("context_info") = tablaParametros
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim StrMensajeRangoValido As String
            StrMensajeRangoValido = validacionesRango()
            If (StrMensajeRangoValido <> "") Then
                AlertaJS("La fecha Final tiene que ser mayor a la fecha Inicial")
                Exit Sub
            End If
            Dim opcion As String
            opcion = Me.rbtReporte.SelectedValue
            Session("titulo") = opcion
            If opcion = "" Then
                AlertaJS("Debe seleccionar un Reporte")
            ElseIf opcion = "Impuesto a la Renta" Then
                ExportarExcel()
            Else
                LlenarSesionContextInfo()
                'EjecutarJS("ShowPopup(''" + Me.tbFechaInicio.Text.Trim() + "','" + Me.tbFechaFin.Text.Trim() + "','" + ddlPortafolio.SelectedValue + "','" + ddlPortafolio.SelectedItem.Text + "');")
                Dim Reporte As String = "frmVisorReporteOperacionesEjecutadas.aspx?Finicio=" & Me.tbFechaInicio.Text.Trim & "&FFin=" & Me.tbFechaFin.Text.Trim & "&pPortafolio=" & ddlPortafolio.SelectedValue & "&nomPortafolio=" & Me.ddlPortafolio.SelectedItem.Text.Trim
                EjecutarJS("showModalDialog('" & Reporte & "', '800', '600','');")
                'EjecutarJS(UIUtility.MostrarPopUp("frmVisorReporteOperacionesEjecutadas.aspx?Finicio=" & Me.tbFechaInicio.Text.Trim & "&FFin=" & Me.tbFechaFin.Text.Trim & "&pPortafolio=" & ddlPortafolio.SelectedValue & "&nomPortafolio=" & Me.ddlPortafolio.SelectedItem.Text.Trim, "", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            'OT11008 - 10/01/2018 - Ian Pastor M.
            'Descripción: Se optimizó y se quitó código inncesario
            Dim sPortafolio As String = IIf(ddlPortafolio.SelectedValue.ToLower.Equals("--seleccione--"), "", ddlPortafolio.SelectedValue)
            Dim opcion As String = ""
            opcion = Me.rbtReporte.SelectedValue
            If opcion = "Renta Variable" Then
                Dim dsAux As New RentaVariableBE
                dsAux = New ReportesInversionesBM().SeleccionarOperacionesRentaVariable(UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim()), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim()), sPortafolio, DatosRequest)
                Copia("RentaVariable", dsAux.Tables(0))
            ElseIf opcion = "Renta Fija" Then
                Dim dsRentaFija As DataSet
                dsRentaFija = New ReportesInversionesBM().SeleccionarOperacionRentaFijaExcel(UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim()), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim()), sPortafolio, DatosRequest)
                Copia("RentaFija", dsRentaFija.Tables(0))
            ElseIf opcion = "Divisas" Then
                Dim oDivisas As New DivisasBE
                oDivisas = New ReportesInversionesBM().SeleccionarOperacionesDivisas(UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim()), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim()), "x" & sPortafolio, DatosRequest)
                Copia("Divisas", oDivisas.Tables(0))
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub rbtReporte_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtReporte.SelectedIndexChanged
        Try
            If rbtReporte.SelectedValue = "Renta Variable" Or rbtReporte.SelectedValue = "Renta Fija" Or rbtReporte.SelectedValue = "Divisas" Then
                btnExportar.Visible = True
            Else
                btnExportar.Visible = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub

    Protected Sub ibVerFirmas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibVerFirmas.Click
        Try
            If ddlPortafolio.SelectedValue = "" Then
                Me.AlertaJS("Debe seleccionar el portafolio")
                Exit Sub
            End If
            If rbtReporte.SelectedValue = "" Then
                Me.AlertaJS("Debe seleccionar el reporte")
                Exit Sub
            End If
            Session("titulo") = Me.rbtReporte.SelectedValue
            Dim fechaNegocio As String = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(PORTAFOLIO_MULTIFONDOS))
            EjecutarJS("ShowPopup('" + fechaNegocio + "','" + fechaNegocio + "','" + ddlPortafolio.SelectedValue + "','" + ddlPortafolio.SelectedItem.Text + "');")
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub

#End Region

#Region " /* Eventos de la Página */ "
    Private Sub CargarPortafolio()
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Items.Insert(0, New ListItem("--Seleccione--", ""))
    End Sub

    Protected Function validacionesRango() As String
        Dim msg As String = ""
        Dim strMensajeError As String = ""
        If (Convert.ToDateTime(Me.tbFechaInicio.Text) > (Convert.ToDateTime(Me.tbFechaFin.Text))) Then
            msg += "\t-La fecha Final tiene que ser mayor a la fecha Inicial \n"
        End If
        If (msg <> "") Then

            strMensajeError = "Error de Rango de Fechas : \n " + msg + "\n"
            Return strMensajeError
        Else
            Return ""
        End If
    End Function

    Private Sub ExportarExcel()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim ds As DataSet
            Dim oReporte As New ReportesInversionesBM
            ds = oReporte.ImpuestoRenta(UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim()))
            Dim dtDPZ As DataTable = ds.Tables(0)
            Dim dtRentaFija As DataTable = ds.Tables(1)
            Dim dtRentaVariable As DataTable = ds.Tables(2)
            Dim dtEncaje As DataTable = ds.Tables(3)

            Dim sFile As String, sTemplate As String

            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "ReportePagoMensualImpuestoRenta_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"

            If File.Exists(sFile) Then File.Delete(sFile)

            sTemplate = RutaPlantillas() & "\" & "PlantillaImpuestoRenta.xls"

            oExcel.Visible = False : oExcel.DisplayAlerts = False
            Dim p As String = 0
            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheet.Cells

            oSheet.SaveAs(sFile)

            For i As Int32 = 0 To dtRentaFija.Rows.Count - 1
                oCells(i + 2, 1) = dtRentaFija.Rows(i)("CodigoPortafolioSBS") 'Portafolio
                oCells(i + 2, 2) = dtRentaFija.Rows(i)("Fecha") 'Fecha
                oCells(i + 2, 3) = dtRentaFija.Rows(i)("Operacion") 'Operacion
                oCells(i + 2, 4) = dtRentaFija.Rows(i)("Moneda") 'Moneda
                oCells(i + 2, 5) = dtRentaFija.Rows(i)("CodigoSBS") 'CodigoSBS
                oCells(i + 2, 6) = dtRentaFija.Rows(i)("Contraparte") 'Intermediario
                oCells(i + 2, 7) = dtRentaFija.Rows(i)("Emisor") 'Emisor
                oCells(i + 2, 8) = dtRentaFija.Rows(i)("CodigoNemonico") 'Nemonico
                oCells(i + 2, 9) = dtRentaFija.Rows(i)("CodigoOrden") 'N°
                oCells(i + 2, 10) = dtRentaFija.Rows(i)("YTM") 'TasaEfectiva
                oCells(i + 2, 11) = dtRentaFija.Rows(i)("MontoNominal") 'ValorNominal
                oCells(i + 2, 12) = dtRentaFija.Rows(i)("MontoOperacion") 'ValorOperacion
                oCells(i + 2, 13) = dtRentaFija.Rows(i)("FechaLiquidacion") 'FechaVencimiento
                oCells(i + 2, 14) = dtRentaFija.Rows(i)("TipoCambio") 'TipoCambio
                oCells(i + 2, 15) = dtRentaFija.Rows(i)("ValorSoles") 'ValorSoles
                oCells(i + 2, 16) = dtRentaFija.Rows(i)("Encaje") 'TasaEncaje
                oCells(i + 2, 17) = "=O" & (i + 2) & "*P" & (i + 2) & "/100" 'dtRentaFija.Rows(i)(17) 'ValorInteres
                p = i
            Next

            oBook.Save()

            p = 0
            oSheet = CType(oSheets.Item(3), Excel.Worksheet)
            oCells = oSheet.Cells

            For i As Int32 = 0 To dtRentaVariable.Rows.Count - 1
                oCells(i + 2, 1) = dtRentaVariable.Rows(i)("CodigoPortafolioSBS") 'Portafolio
                oCells(i + 2, 2) = dtRentaVariable.Rows(i)("Fecha") 'Fecha
                oCells(i + 2, 3) = dtRentaVariable.Rows(i)("Operacion") 'Operacion
                oCells(i + 2, 4) = dtRentaVariable.Rows(i)("Moneda") 'Moneda
                oCells(i + 2, 5) = dtRentaVariable.Rows(i)("Emisor") 'Emisor
                oCells(i + 2, 6) = dtRentaVariable.Rows(i)("CodigoNemonico") 'Nemonico
                oCells(i + 2, 7) = dtRentaVariable.Rows(i)("CodigoSBS") 'CodigoSBS
                oCells(i + 2, 8) = dtRentaVariable.Rows(i)("CantidadOperacion") 'Cantidad
                oCells(i + 2, 9) = dtRentaVariable.Rows(i)("Precio") 'Precio
                oCells(i + 2, 10) = dtRentaVariable.Rows(i)("MontoOperacion") 'MontoOperacion
                oCells(i + 2, 11) = dtRentaVariable.Rows(i)("TipoCambio") 'TipoCambio
                oCells(i + 2, 12) = dtRentaVariable.Rows(i)("ValorSoles") 'ValorSoles
                oCells(i + 2, 13) = dtRentaVariable.Rows(i)("Encaje") 'TasaEncaje  
                oCells(i + 2, 14) = "=L" & (i + 2) & "*M" & (i + 2) & "/100" 'dtRentaVariable.Rows(i)(15) 'ValorInteres 
                p = i
            Next

            oBook.Save()

            p = 0
            oSheet = CType(oSheets.Item(4), Excel.Worksheet)
            oCells = oSheet.Cells

            For i As Int32 = 0 To dtDPZ.Rows.Count - 1
                oCells(i + 2, 1) = dtDPZ.Rows(i)("CodigoPortafolioSBS") 'Portafolio
                oCells(i + 2, 2) = dtDPZ.Rows(i)("Fecha") 'Fecha
                oCells(i + 2, 3) = dtDPZ.Rows(i)("Operacion") 'Operacion
                oCells(i + 2, 4) = dtDPZ.Rows(i)("Moneda") 'Moneda
                oCells(i + 2, 5) = dtDPZ.Rows(i)("CodigoSBS") 'CodigoSBS
                oCells(i + 2, 6) = dtDPZ.Rows(i)("Contraparte") 'Intermediario
                oCells(i + 2, 7) = dtDPZ.Rows(i)("Emisor") 'Emisor
                oCells(i + 2, 8) = dtDPZ.Rows(i)("CodigoNemonico") 'Nemonico
                oCells(i + 2, 9) = dtDPZ.Rows(i)("CodigoOrden") 'N°
                oCells(i + 2, 10) = dtDPZ.Rows(i)("MontoNominal") 'ValorNominal
                oCells(i + 2, 11) = dtDPZ.Rows(i)("MontoOperacion") 'ValorOperacion
                oCells(i + 2, 12) = dtDPZ.Rows(i)("FechaLiquidacion") 'FechaVencimiento
                oCells(i + 2, 13) = dtDPZ.Rows(i)("TipoCambio") 'TipoCambio
                oCells(i + 2, 14) = dtDPZ.Rows(i)("ValorSoles") 'ValorSoles
                oCells(i + 2, 15) = dtDPZ.Rows(i)("Encaje") 'TasaEncaje
                oCells(i + 2, 16) = "=N" & (i + 2) & "*O" & (i + 2) & "/100" 'ValorInteres --
                p = i
            Next

            oBook.Save()

            p = 0

            oSheet = CType(oSheets.Item(5), Excel.Worksheet)
            oCells = oSheet.Cells

            For i As Int32 = 0 To dtEncaje.Rows.Count - 1
                oCells(i + 2, 1) = dtEncaje.Rows(i)("CodigoMnemonico") 'CodigoNemonico
                oCells(i + 2, 2) = dtEncaje.Rows(i)("CodigoSBS") 'CodigoObs
                oCells(i + 2, 3) = dtEncaje.Rows(i)("CodigoTipoInstrumento") 'CodigoTipoInstrumento
                oCells(i + 2, 4) = dtEncaje.Rows(i)("FechaEmision") 'FechaEmision
                oCells(i + 2, 5) = dtEncaje.Rows(i)("ValorTasaEncaje") 'ValorTasaEncaje
                oCells(i + 2, 6) = dtEncaje.Rows(i)("FechaVigencia") 'Situacion
                p = i
            Next

            oBook.Save()

            p = 0
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            Dim mes As String = CType(tbFechaInicio.Text, DateTime).ToString("m", Global.System.Globalization.CultureInfo.CurrentCulture).Substring(3).ToUpper
            oCells(1, 1) = "RESUMEN " + mes + " " + tbFechaInicio.Text.Substring(6, 4)

            Dim Suma As Decimal = 0
            Dim ValorVenta As Object
            Dim ValorVentaEncaje As Object

            For i As Int32 = 1 To 3
                'RENTA FIJA
                ValorVenta = dtRentaFija.Compute("sum(ValorSoles)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "'")
                If ValorVenta Is DBNull.Value Then ValorVenta = 0
                ValorVentaEncaje = dtRentaFija.Compute("sum(ValorEncaje)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "'")
                If ValorVentaEncaje Is DBNull.Value Then ValorVentaEncaje = 0
                oCells(i + 2, 3) = ValorVenta
                oCells(i + 2, 4) = ValorVentaEncaje

                'RENTA VARIABLE
                ValorVenta = dtRentaVariable.Compute("sum(ValorSoles)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "'")
                If ValorVenta Is DBNull.Value Then ValorVenta = 0
                ValorVentaEncaje = dtRentaVariable.Compute("sum(ValorEncaje)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "'")
                If ValorVentaEncaje Is DBNull.Value Then ValorVentaEncaje = 0
                oCells(i + 5, 3) = ValorVenta
                oCells(i + 5, 4) = ValorVentaEncaje

                'DPZ SOLES
                ValorVenta = dtDPZ.Compute("sum(ValorSoles)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "' and Moneda='NSOL'")
                If ValorVenta Is DBNull.Value Then ValorVenta = 0
                ValorVentaEncaje = dtDPZ.Compute("sum(ValorEncaje)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "' and Moneda='NSOL'")
                If ValorVentaEncaje Is DBNull.Value Then ValorVentaEncaje = 0
                oCells(i + 10, 3) = ValorVenta
                oCells(i + 10, 4) = ValorVentaEncaje

                'DPZ DOLARES
                ValorVenta = dtDPZ.Compute("sum(ValorSoles)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "' and Moneda='DOL'")
                If ValorVenta Is DBNull.Value Then ValorVenta = 0
                ValorVentaEncaje = dtDPZ.Compute("sum(ValorEncaje)", "CodigoPortafolioSBS = 'HO-FONDO" & i & "' and Moneda='DOL'")
                If ValorVentaEncaje Is DBNull.Value Then ValorVentaEncaje = 0
                oCells(i + 13, 3) = ValorVenta
                oCells(i + 13, 4) = ValorVentaEncaje
            Next
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
            Response.WriteFile(sFile)
            Response.End()
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

    Public Sub Copia(ByVal Archivo As String, ByVal dt As DataTable)
        'OT10689 - Inicio. Kill process excel
        'Dim oldCulture As CultureInfo
        Dim oldCulture As Global.System.Globalization.CultureInfo
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim sFile As String, sTemplate As String = String.Empty
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            oldCulture = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = New Global.System.Globalization.CultureInfo("en-US", False)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".xls"    'JHC REQ 66056 20130208
            If Archivo = "RentaVariable" Then
                sTemplate = RutaPlantillas() & "\PlantillaOperacionesRentaVariable.xls"
            ElseIf Archivo = "RentaFija" Then
                sTemplate = RutaPlantillas() & "\PlantillaOperacionesRentaFija.xls"
            ElseIf Archivo = "Divisas" Then
                sTemplate = RutaPlantillas() & "\PlantillaOperacionesDivisas.xls"
            End If
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            DumpData(OrdenarTabla(dt, Archivo), oCells) 'Fill in the data   
            oSheet.SaveAs(sFile) 'Save in a temporary file
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
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

    Private Function OrdenarTabla(ByVal dt As DataTable, ByVal Archivo As String) As DataTable
        Dim rows As DataRow()
        Dim dtNew As DataTable
        ' copy table structure  
        dtNew = dt.Clone()
        ' sort and filter data  

        If Archivo = "Divisas" Then
            rows = dt.Select("1=1", "Portafolio asc , Operacion Asc , Moneda asc")
        Else
            rows = dt.Select("1=1", "Portafolio asc , Operacion Asc , Moneda asc , CodigoNemonico asc")
        End If

        ' fill dtNew with selected rows
        For Each dr As DataRow In rows
            dtNew.ImportRow(dr)
        Next
        ' return filtered dt
        Return dtNew
    End Function

    Private Sub DumpData(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim iRow As Integer
        Dim ListaCantidad As New ArrayList
        Dim ListaMonto As New ArrayList
        Dim ListaPrecio As New ArrayList
        Dim ListaMontoMoneda As New ArrayList
        Dim Cantidad As Decimal
        Dim Monto As Decimal
        Dim MontoMoneda As Decimal
        Dim i As Integer = 1

        If rbtReporte.SelectedValue = "Renta Variable" Then
            For iRow = 0 To dt.Rows.Count - 1
                If Not IsDBNull(dt.Rows(iRow)("CantidadOperacion")) Then
                    Cantidad = Cantidad + Convert.ToDecimal(dt.Rows(iRow)("CantidadOperacion"))
                End If
                If Not IsDBNull(dt.Rows(iRow)("MontoOperacion")) Then
                    Monto = Monto + +Convert.ToDecimal(dt.Rows(iRow)("MontoOperacion"))
                    MontoMoneda = MontoMoneda + +Convert.ToDecimal(dt.Rows(iRow)("MontoOperacion"))
                End If

                If dt.Rows.Count = 1 Then
                    ListaCantidad.Add(Cantidad)
                    ListaMonto.Add(Monto)
                    If Cantidad = 0 Then
                        ListaPrecio.Add(0)
                    Else
                        ListaPrecio.Add(Monto / Cantidad)
                    End If
                    ListaMontoMoneda.Add(MontoMoneda)
                    Cantidad = 0
                    Monto = 0
                Else
                    If iRow = dt.Rows.Count - 1 Then
                        ListaCantidad.Add(Cantidad)
                        ListaMonto.Add(Monto)
                        ListaPrecio.Add(Monto / Cantidad)
                        ListaMontoMoneda.Add(MontoMoneda)
                        Cantidad = 0
                        Monto = 0
                    Else
                        If (dt.Rows(iRow)("CodigoNemonico") <> dt.Rows(iRow + 1)("CodigoNemonico")) Then
                            ListaCantidad.Add(Cantidad)
                            ListaMonto.Add(Monto)
                            ListaPrecio.Add(Monto / Cantidad)
                            Cantidad = 0
                            Monto = 0
                        End If
                        If (dt.Rows(iRow)("Moneda") <> dt.Rows(iRow + 1)("Moneda")) Then
                            ListaMontoMoneda.Add(MontoMoneda)
                            MontoMoneda = 0
                        End If

                    End If
                End If
            Next

            Dim j As Integer = 0
            Dim k As Integer = 0
            For iRow = 0 To dt.Rows.Count - 1
                oCells(i, 1) = dt.Rows(iRow)("Portafolio")
                oCells(i, 2) = "OPERACIONES DE RENTA VARIABLE DEL "
                oCells(i, 3) = tbFechaInicio.Text
                oCells(i, 4) = "AL"
                oCells(i, 5) = tbFechaFin.Text
                oCells(i, 6) = "MON."
                oCells(i, 7) = "EMISION"
                oCells(i, 8) = "SBS"
                oCells(i, 9) = "DIA"
                oCells(i, 10) = "N°"
                oCells(i, 11) = "CANTIDAD"
                oCells(i, 12) = "PRECIO"
                oCells(i, 13) = "MONTO"
                oCells(i, 14) = "S.A.B"
                oCells(i, 15) = "HORA"
                oCells(i, 16) = dt.Rows(iRow)("Operacion")
                oCells(i, 17) = String.Format("{0}({1})", dt.Rows(iRow)("DescripcionMoneda"), dt.Rows(iRow)("Moneda"))
                oCells(i, 18) = dt.Rows(iRow)("CodigoNemonico")
                oCells(i, 19) = dt.Rows(iRow)("CodigoSBS")
                If dt.Rows(iRow)("Fecha").ToString.Length < 3 Then
                    oCells(i, 20) = ""
                Else
                    oCells(i, 20) = CType(dt.Rows(iRow)("Fecha"), String).Substring(0, 2)
                End If
                oCells(i, 21) = dt.Rows(iRow)("CodigoOrden")
                oCells(i, 22) = dt.Rows(iRow)("CantidadOperacion")
                oCells(i, 23) = dt.Rows(iRow)("PrecioNegociacionLimpio")
                oCells(i, 24) = dt.Rows(iRow)("MontoOperacion")
                oCells(i, 25) = dt.Rows(iRow)("SAB")
                If dt.Rows(iRow)("Fecha").ToString.Length < 6 Then
                    oCells(i, 26) = ""
                Else
                    oCells(i, 26) = CType(dt.Rows(iRow)("HoraOperacion"), String).Substring(0, 5)
                End If
                oCells(i, 27) = "TOTAL EMISION:"
                If (dt.Rows.Count >= 2) Then
                    If (i = 1) Then
                        oCells(i, 28) = ListaCantidad(j)
                        oCells(i, 29) = ListaPrecio(j)
                        oCells(i, 30) = ListaMonto(j)
                        oCells(i, 33) = ListaMontoMoneda(k)
                    Else
                        If (dt.Rows(iRow)("CodigoNemonico") <> dt.Rows(iRow - 1)("CodigoNemonico")) Then
                            j = j + 1
                            oCells(i, 28) = ListaCantidad(j)
                            oCells(i, 29) = ListaPrecio(j)
                            oCells(i, 30) = ListaMonto(j)
                        Else
                            oCells(i, 28) = ListaCantidad(j)
                            oCells(i, 29) = ListaPrecio(j)
                            oCells(i, 30) = ListaMonto(j)
                        End If
                        If (dt.Rows(iRow)("Moneda") <> dt.Rows(iRow - 1)("Moneda")) Then
                            k = k + 1
                            oCells(i, 33) = ListaMontoMoneda(k)
                        Else
                            oCells(i, 33) = ListaMontoMoneda(k)
                        End If
                    End If
                Else
                    oCells(i, 28) = ListaCantidad(j)
                    oCells(i, 29) = ListaPrecio(j)
                    oCells(i, 30) = ListaMonto(j)
                End If
                oCells(i, 31) = "TOTAL"
                oCells(i, 32) = String.Format("{0}({1})", dt.Rows(iRow)("DescripcionMoneda"), dt.Rows(iRow)("Moneda"))
                oCells(i, 34) = "HUBIERON INVERSIONES EN EL EXTRANJERO   ( SI )  ( NO )"
                oCells(i, 35) = "OPERADOR DE RENTA VARIABLE"
                oCells(i, 36) = "CONTROL DE INVERSIONES"
                oCells(i, 37) = "GERENTE DE INVERSIONES"

                i = i + 1
            Next
        ElseIf rbtReporte.SelectedValue = "Renta Fija" Then

            i = 7
            oCells(4, 1) = oCells(4, 1).Value & tbFechaInicio.Text & " al " & tbFechaFin.Text
            oCells(2, 13) = Usuario
            oCells(3, 13) = Now.ToString("dd/MM/yyyy")
            For iRow = 0 To dt.Rows.Count - 1
                oCells(i, 1) = dt.Rows(iRow)("Portafolio")
                oCells(i, 2) = dt.Rows(iRow)("Fecha")
                oCells(i, 3) = dt.Rows(iRow)("Operacion")
                oCells(i, 4) = dt.Rows(iRow)("Moneda")
                oCells(i, 5) = dt.Rows(iRow)("CodigoTipoTitulo")
                oCells(i, 6) = "'" & dt.Rows(iRow)("CodigoSBS")
                oCells(i, 7) = dt.Rows(iRow)("Contraparte")
                oCells(i, 8) = dt.Rows(iRow)("Emisor")
                oCells(i, 9) = dt.Rows(iRow)("CodigoNemonico")
                oCells(i, 10) = dt.Rows(iRow)("MontoNominal")
                oCells(i, 11) = dt.Rows(iRow)("ValorUnitario")
                oCells(i, 12) = "=+J" & i & "/K" & i
                oCells(i, 13) = dt.Rows(iRow)("MontoOperacion")
                i = i + 1
            Next

        ElseIf rbtReporte.SelectedValue = "Divisas" Then

            Dim sPortafolio As String = ""
            Dim i2 As Long = 0

            oCells(4, 1) = oCells(4, 1).Value & tbFechaInicio.Text & " al " & tbFechaFin.Text
            oCells(2, 14) = Usuario
            oCells(3, 14) = Now.ToString("dd/MM/yyyy")
            i = 9
            For iRow = 0 To dt.Rows.Count - 1
                If dt.Rows(iRow)("Fecha").ToString <> "" Then
                    i2 = i + 1
                    If dt.Rows(iRow)("Portafolio").ToString <> sPortafolio Then
                        oCells.Rows((i + 1).ToString & ":" & (i + 2).ToString).Copy()
                        oCells.Rows((i2 + 2).ToString & ":" & (i2 + 3).ToString).Insert(Excel.XlDirection.xlDown)
                        oCells.Application.CutCopyMode = False
                        oCells(i + 1, 1) = dt.Rows(iRow)("Portafolio")
                        sPortafolio = dt.Rows(iRow)("Portafolio")
                        If i = 9 Then
                            i = i + 2
                        Else
                            oCells.Rows(i & ":" & i).Delete(Excel.XlDirection.xlUp)
                            i = i + 1
                        End If
                        i2 = i + 1
                    End If
                    oCells.Rows(i & ":" & i).Copy()
                    oCells.Rows(i2 & ":" & i2).Insert(Excel.XlDirection.xlDown)
                    oCells.Application.CutCopyMode = False
                    oCells(i, 2) = dt.Rows(iRow)("Mercado")
                    oCells(i, 3) = dt.Rows(iRow)("Fecha")
                    oCells(i, 4) = dt.Rows(iRow)("FechaLiquidacion")
                    If dt.Rows(iRow)("Operacion").ToString = "Compra Divisas" Then
                        oCells(i, 6) = dt.Rows(iRow)("MonedaOperacion")
                        oCells(i, 9) = dt.Rows(iRow)("Moneda")
                    Else
                        oCells(i, 6) = dt.Rows(iRow)("Moneda")
                        oCells(i, 9) = dt.Rows(iRow)("MonedaOperacion")
                    End If
                    oCells(i, 7) = dt.Rows(iRow)("MontoDivisa")
                    oCells(i, 10) = dt.Rows(iRow)("MontoOperacion")
                    oCells(i, 12) = dt.Rows(iRow)("TipoCambio")
                    oCells(i, 13) = dt.Rows(iRow)("Banco")
                    oCells(i, 14) = "'" & dt.Rows(iRow)("CodigoOrden")
                    i = i + 1
                End If
            Next
            oCells.Rows(i & ":" & i).Delete(Excel.XlDirection.xlUp)
            oCells.Rows(i & ":" & (i + 1).ToString).Delete(Excel.XlDirection.xlUp)
        End If
    End Sub

#End Region

End Class

