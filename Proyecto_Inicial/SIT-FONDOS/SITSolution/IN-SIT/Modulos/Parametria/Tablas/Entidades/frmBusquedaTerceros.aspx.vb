Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Partial Class Modulos_Parametria_Tablas_Entidades_frmBusquedaTerceros
    Inherits BasePage
#Region " */ Metodos de Pagina */"
    Private Sub MostrarMensajeConfirmacion(ByVal oDataGridItem As GridViewRow)
        Dim ibEliminar As ImageButton
        ibEliminar = DirectCast(oDataGridItem.FindControl("ibnEliminar"), ImageButton)
        Dim men As String = "javascript:return confirm('" & Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD & "');"
        ibEliminar.Attributes.Add("onclick", men)
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
                Buscar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmTerceros.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim oTerceroBM As New TercerosBM
            Dim dt As New DataTable
            dt = oTerceroBM.ListarTerceroReporte()
            Copia("Tercero", dt)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try
    End Sub
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmTerceros.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Registro")
        End Try
    End Sub
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTercerosBM As New TercerosBM
            Dim codigoTercero As String = e.CommandArgument.ToString()
            Dim ds As DataSet
            Dim oEntidadBM As New EntidadBM
            Dim codigoEntidad As String = String.Empty
            'OT-11033 - 03/01/2017 - Ian Pastor M.
            'Descripcion: Verifica si existen terceros negociados (Ejecutados o confirmados)
            If oTercerosBM.VerificarExisteTerceroNegociado(codigoTercero) Then
                AlertaJS("El tercero ya ha sido utilizado en la negociación de operaciones de inversión. No se puede eliminar.")
                Exit Sub
            End If
            ds = oEntidadBM.SeleccionarPorCodigoTercero(codigoTercero, Me.DatosRequest)
            If ds.Tables(0).Rows.Count > 0 Then
                codigoEntidad = ds.Tables(0).Rows(0)(0).ToString()
                Try
                    oEntidadBM.Eliminar(codigoEntidad, Me.DatosRequest)
                Catch ex As Exception
                    AlertaJS(ex.Message) : CargarGrilla()
                    Exit Sub
                End Try
                Try
                    oTercerosBM.Eliminar(codigoTercero, DatosRequest)
                Catch ex As Exception
                    AlertaJS(ex.Message) : CargarGrilla()
                    Exit Sub
                End Try
            End If
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim imgEliminar As ImageButton
                imgEliminar = DirectCast(e.Row.FindControl("ibnEliminar"), ImageButton)
                imgEliminar.Attributes.Add("onclick", ConfirmJS("¿Confirmar la eliminación del registro?"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
#End Region
#Region " /* Funciones Personalizadas*/"
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
        End If
    End Sub
    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub
    Private Sub CargarGrilla()
        Dim oTercerosBM As New TercerosBM
        Dim strCodigoTipoDocumento, strCodigoSectorEmpresarial, strCodigoClasificacion, strSituacion As String
        strCodigoTipoDocumento = ""
        strCodigoSectorEmpresarial = IIf(ddlSectorEmpresarial.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSectorEmpresarial.SelectedValue).ToString()
        strCodigoClasificacion = IIf(Me.ddlClasificacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlClasificacion.SelectedValue).ToString()
        strSituacion = IIf(ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oTercerosBM.SeleccionarPorFiltro(txtCodigoTercero.Text, strCodigoClasificacion, tbDescripcion.Text.TrimStart.TrimEnd.ToString(), strCodigoTipoDocumento, strCodigoSectorEmpresarial, strSituacion, DatosRequest).Terceros()
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub
    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        Dim tablaSectorEmpresarial As New DataTable
        Dim oSectorEmpresarial As New SectorEmpresarialBM
        tablaSectorEmpresarial = oSectorEmpresarial.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSectorEmpresarial, tablaSectorEmpresarial, "CodigoSectorEmpresarial", "Descripcion", True)
        Dim dtClasificacion As DataTable
        dtClasificacion = oParametrosGenerales.Listar("CTercero", DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlClasificacion, dtClasificacion, "Valor", "Nombre", True)
    End Sub
    Public Sub Copia(ByVal Archivo As String, ByVal dt As DataTable)
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
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor").ToString & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\Plantilla" & Archivo & ".xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            DumpData(dt, oCells)
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Dim filepath As String = sFile.Replace("\", "\\")
            Dim filename As String = System.IO.Path.GetFileName(filepath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
            Response.Flush()
            Response.WriteFile(filepath)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString, "'", ""))
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
        Dim iRow As Integer, iCol As Integer
        For iCol = 0 To dt.Columns.Count - 1
            oCells(1, iCol + 1) = dt.Columns(iCol).ToString
        Next
        'Output Data
        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray
            For iCol = 0 To UBound(ary)
                oCells(iRow + 2, iCol + 1) = ary(iCol).ToString
            Next
        Next
    End Sub
#End Region
End Class