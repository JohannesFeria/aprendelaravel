Imports Sit.BusinessLayer
Imports Sit.DataAccessLayer
Imports System.Data
Imports System.IO

Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmVisualizacionValorizacion
    Inherits BasePage

    Private url As String
    Private portBM As New PortafolioBM
    Private dt_VL, dt_Interes, dt_Ganancia, dt_Inversion, dt_Valorizacion As New DataTable
    Private total As Integer
    Private ds As New DataSet
    Private lstPortafolio As String, fechaOperacion As String = String.Empty
    Private lt As Literal

    Private Sub Cargar_VL()
        gvPxq.DataSource = dt_VL
        gvPxq.DataBind()
        total = gvPxq.Rows.Count
        If total = 0 Then
            Me.li01.Visible = False
            Me.tabs01.Visible = False
        End If

    End Sub
    Private Sub Cargar_Interes()
        gvInteresNegativo.DataSource = dt_Interes
        gvInteresNegativo.DataBind()
        total = gvInteresNegativo.Rows.Count
        If total = 0 Then
            Me.li02.Visible = False
            Me.tabs02.Visible = False
        End If
    End Sub
    Private Sub Cargar_Ganancia()
        gvVariacion.DataSource = dt_Ganancia
        gvVariacion.DataBind()
        total = gvVariacion.Rows.Count
        If total = 0 Then
            Me.li03.Visible = False
            Me.tabs03.Visible = False
        End If
    End Sub
    Private Sub Cargar_Inversion()
        gvInversionNula.DataSource = dt_Inversion
        gvInversionNula.DataBind()
        total = gvInversionNula.Rows.Count
        If total = 0 Then
            Me.li04.Visible = False
            Me.tabs04.Visible = False
        End If
    End Sub
    Private Sub Cargar_Valorizacion()
        gvValorizacionNula.DataSource = dt_Valorizacion
        gvValorizacionNula.DataBind()
        total = gvValorizacionNula.Rows.Count
        If total = 0 Then
            Me.li05.Visible = False
            Me.tabs05.Visible = False
        End If
    End Sub

    Private dsComplemento As DataSet
    Private filas() As DataRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lstPortafolio = Request.QueryString("lstPortafolio")
        fechaOperacion = Convert.ToDecimal(Request.QueryString("fechaOperacion"))

        dsComplemento = Session("dsComplemento")

        dt_VL = dsComplemento.Tables(1)

        If dt_VL.Rows.Count > 0 Then
            filas = dt_VL.Select("codigoPortafolioSBS IN (" & lstPortafolio & ")")
            If filas.Count > 0 Then
                dt_VL = filas.CopyToDataTable()
                dt_VL.Columns.RemoveAt(0)
            End If
        End If

        dt_Interes = dsComplemento.Tables(2)
        If dt_Interes.Rows.Count > 0 Then
            filas = dt_Interes.Select("codigoPortafolioSBS IN (" & lstPortafolio & ")")
            If filas.Count > 0 Then
                dt_Interes = filas.CopyToDataTable()
                dt_Interes.Columns.RemoveAt(0)
            End If
        End If

        dt_Ganancia = dsComplemento.Tables(3)
        If dt_Ganancia.Rows.Count > 0 Then
            filas = dt_Ganancia.Select("codigoPortafolioSBS IN (" & lstPortafolio & ")")
            If filas.Count > 0 Then
                dt_Ganancia = filas.CopyToDataTable()
                dt_Ganancia.Columns.RemoveAt(0)
            End If
        End If

        dt_Inversion = dsComplemento.Tables(4)
        If dt_Inversion.Rows.Count > 0 Then
            filas = dt_Inversion.Select("codigoPortafolioSBS IN (" & lstPortafolio & ")")
            If filas.Count > 0 Then
                dt_Inversion = filas.CopyToDataTable()
                dt_Inversion.Columns.RemoveAt(0)
            End If
        End If

        dt_Valorizacion = dsComplemento.Tables(5)
        If dt_Valorizacion.Rows.Count > 0 Then
            filas = dt_Valorizacion.Select("codigoPortafolioSBS IN (" & lstPortafolio & ")")
            If filas.Count > 0 Then
                dt_Valorizacion = filas.CopyToDataTable()
                dt_Valorizacion.Columns.RemoveAt(0)
            End If
        End If

        Cargar_VL()
        Cargar_Interes()
        Cargar_Ganancia()
        Cargar_Inversion()
        Cargar_Valorizacion()


    End Sub

    Private Function CargarGanancia(ByVal ganancia As String, ByVal fechaOperacion As Decimal) As DataTable
        Dim dtHoy As New DataTable
        dtHoy = dt_Ganancia

        Dim fechaAyer As DateTime = Convert.ToDateTime(UIUtility.ConvertirDecimalAStringFormatoFecha(fechaOperacion))
        fechaAyer = fechaAyer.AddDays(-1)
        Dim dtAyer As New DataTable
        dtAyer = portBM.CargarDatos_InversionAnterior(ganancia, UIUtility.ConvertirFechaaDecimal(fechaAyer.ToShortDateString))


        For hoy = 0 To dtHoy.Rows.Count - 1
            For ayer = 0 To dtAyer.Rows.Count - 1

                If dtHoy.Rows(hoy).Item("Fondo") = dtAyer.Rows(ayer).Item("Fondo") And dtHoy.Rows(hoy).Item("CodigoValor") = dtAyer.Rows(ayer).Item("CodigoValor") Then
                    Dim row As DataRow = dtHoy.Rows(hoy)
                    row.BeginEdit()
                    row("Ganacia_perdida_T_1") = Convert.ToDecimal(dtHoy.Rows(hoy)("Ganancia_Perdida")) - Convert.ToDecimal(dtAyer.Rows(ayer)("Ganancia_Perdida"))
                    row.EndEdit()
                Else
                    Dim row As DataRow = dtHoy.Rows(hoy)
                    row.BeginEdit()
                    row("Ganacia_perdida_T_1") = DBNull.Value
                    row.EndEdit()
                End If

            Next
        Next

        Return dtHoy
    End Function

    Private sScript As StringBuilder
    Protected Sub gvPxq_OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvPxq.PageIndex = e.NewPageIndex
        Cargar_VL()
    End Sub


    Protected Sub gvInteresNegativo_OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvInteresNegativo.PageIndex = e.NewPageIndex
        Cargar_Interes()
    End Sub

    Protected Sub gvVariacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVariacion.PageIndexChanging
        gvVariacion.PageIndex = e.NewPageIndex
        Cargar_Ganancia()
    End Sub

    Protected Sub gvInversionNula_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInversionNula.PageIndexChanging
        gvInversionNula.PageIndex = e.NewPageIndex
        Cargar_Inversion()
    End Sub

    Protected Sub gvValorizacionNula_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvValorizacionNula.PageIndexChanging
        gvValorizacionNula.PageIndex = e.NewPageIndex
        Cargar_Valorizacion()
    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        GenerarArchivoExcel(fechaOperacion, "PlantillaAnomalias.xls")

    End Sub

    Sub DatosExcel(ByVal fecha As String, ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(3, 4) = fecha
        For Each dr In dt.Rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
        Next
    End Sub

    Function RutaPlantillas() As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("018", New DataSet())
        Return oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
    End Function

    Private Sub GenerarArchivoExcel(ByVal fecha As String, ByVal nombreArchivo As String)

        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
        oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
        fecha = UIUtility.ConvertirDecimalAStringFormatoFecha(Convert.ToDecimal(fecha))
        Dim sFile As String, sTemplate As String
        sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", New DataSet).Rows(0)("Valor") & "COM_" & Usuario & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
        Dim Indice As Integer = 3
        If File.Exists(sFile) Then File.Delete(sFile)
        sTemplate = RutaPlantillas() & "\" & nombreArchivo
        oExcel.Visible = False : oExcel.DisplayAlerts = False
        oBooks = oExcel.Workbooks
        oBooks.Open(sTemplate)

        'Libro 01
        oBook = oBooks.Item(1)
        oSheets = oBook.Worksheets
        oSheet = CType(oSheets.Item(1), Excel.Worksheet)
        oCells = oSheet.Cells
        oSheet.SaveAs(sFile)

        DatosExcel(fecha, dt_VL, 6, oSheet, oCells)
        oExcel.Cells.EntireColumn.AutoFit()

        'Libro 02
        oBook = oBooks.Item(1)
        oSheets = oBook.Worksheets
        oSheet = CType(oSheets.Item(2), Excel.Worksheet)
        oCells = oSheet.Cells
        oSheet.SaveAs(sFile)
        DatosExcel(fecha, dt_Interes, 6, oSheet, oCells)
        oExcel.Cells.EntireColumn.AutoFit()

        'Libro 03
        oBook = oBooks.Item(1)
        oSheets = oBook.Worksheets
        oSheet = CType(oSheets.Item(3), Excel.Worksheet)
        oCells = oSheet.Cells
        oSheet.SaveAs(sFile)
        dt_Ganancia = CargarGanancia(lstPortafolio, fechaOperacion)
        DatosExcel(fecha, dt_Ganancia, 6, oSheet, oCells)
        oExcel.Cells.EntireColumn.AutoFit()

        'Libro 04
        oBook = oBooks.Item(1)
        oSheets = oBook.Worksheets
        oSheet = CType(oSheets.Item(4), Excel.Worksheet)
        oCells = oSheet.Cells
        oSheet.SaveAs(sFile)
        DatosExcel(fecha, dt_Inversion, 6, oSheet, oCells)
        oExcel.Cells.EntireColumn.AutoFit()

        'Libro 05
        oBook = oBooks.Item(1)
        oSheets = oBook.Worksheets
        oSheet = CType(oSheets.Item(5), Excel.Worksheet)
        oCells = oSheet.Cells
        oSheet.SaveAs(sFile)
        DatosExcel(fecha, dt_Valorizacion, 6, oSheet, oCells)
        oExcel.Cells.EntireColumn.AutoFit()

        oBook.Save()
        oBook.Close()
        HttpContext.Current.Response.ContentType = "application/xls"
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "COM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
        HttpContext.Current.Response.WriteFile(sFile)
        HttpContext.Current.Response.End()
        ''Else
        ''    AlertaJS("No existen registros que mostrar para esta fecha y portafolio.")

    End Sub
    Private diferencia As Decimal

    Protected Sub gvPxq_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPxq.PreRender

    End Sub

    Private contador As Integer
    Protected Sub gvPxq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPxq.RowDataBound

    End Sub

    Protected Sub Modulos_ValorizacionCustodia_Valorizacion_frmVisualizacionValorizacion_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Protected Sub Modulos_ValorizacionCustodia_Valorizacion_frmVisualizacionValorizacion_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
