Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data

Partial Class Modulos_Gestion_Reportes_frmReporteGipsa
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ParametrosSIT.PORTAFOLIO_MULTIFONDOS))
            CargarPortafolio(True)
        End If
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ds, dsTabla, dsEncaje, dsEmisor As DataSet
        Dim fecha As Integer
        Try
            If Me.tbFecha.Text = "" Then
                fecha = 0
            Else
                fecha = UIUtility.ConvertirFechaaDecimal(Me.tbFecha.Text)
            End If

            ds = New ReporteGipsaBM().ReporteGIPSA(fecha, Me.ddlPortafolio.SelectedValue, DatosRequest)
            dsEmisor = New ReporteGipsaBM().ReporteGIPSAEmisor(fecha, Me.ddlPortafolio.SelectedValue, DatosRequest)
            dsTabla = New ReporteGipsaBM().TablaReporteGipsa(fecha, Me.ddlPortafolio.SelectedValue, DatosRequest)
            dsEncaje = New ReporteGipsaBM().EncajeReporteGipsa(DatosRequest)
            Copia("Datos Gipsa", "T Especiales", "Encaje", CType(ds.Tables(0), DataTable), CType(dsTabla.Tables(0), DataTable), CType(dsEncaje.Tables(0), DataTable), CType(dsEmisor.Tables(0), DataTable))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Public Sub Copia(ByVal Archivo As String, ByVal ArchivoTabla As String, ByVal ArchivoEncaje As String, ByVal dt As DataTable, ByVal dtTabla As DataTable, ByVal dtEncaje As DataTable, ByVal dtEmisor As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim nombreArchivo As String = Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\PlantillaGipsa.xls"

            oExcel.Visible = False : oExcel.DisplayAlerts = False

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(3), Excel.Worksheet)
            oSheet.Name = ArchivoEncaje
            oCells = oSheet.Cells
            DumpDataEncaje(dtEncaje, oCells) 'Fill in the data
            oSheet.SaveAs(sFile)

            'Pagina Tabla
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oSheet.Name = ArchivoTabla
            oCells = oSheet.Cells
            DumpDataTabla(dtTabla, oCells) 'Fill in the data
            oSheet.SaveAs(sFile) 'Save in a temporary file
            'oBook.Close()

            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            DumpData(dt, oCells) 'Fill in the data
            DumpDataEmisor(dtEmisor, oCells) 'Fill in the data
            oSheet.SaveAs(sFile) 'Save in a temporary file
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
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

    Private Sub DumpData(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow, ary() As Object
        Dim iRow, iCol As Integer

        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray
            For iCol = 0 To UBound(ary)
                oCells(iRow + 7, iCol + 2) = ary(iCol).ToString
            Next
        Next
    End Sub

    Private Sub DumpDataEmisor(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow, ary() As Object
        Dim iRow, iCol As Integer

        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray
            For iCol = 0 To UBound(ary)
                oCells(iRow + 6, 79) = ary(iCol).ToString
            Next
        Next
    End Sub

    Private Sub DumpDataTabla(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow, ary() As Object
        Dim iRow, iCol As Integer

        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray
            For iCol = 0 To UBound(ary)
                oCells(iRow + 7, iCol + 2) = ary(iCol).ToString
            Next
        Next
    End Sub

    Private Sub DumpDataEncaje(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow, ary() As Object
        Dim iRow, iCol As Integer

        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray
            For iCol = 0 To UBound(ary)
                oCells(iRow + 5, iCol + 2) = ary(iCol).ToString
            Next
        Next
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
    End Sub

End Class
