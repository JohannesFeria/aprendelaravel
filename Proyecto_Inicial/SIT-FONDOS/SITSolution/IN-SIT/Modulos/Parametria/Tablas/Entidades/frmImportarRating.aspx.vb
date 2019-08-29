''''' CREACION: BPesantes 29-11-2016

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb
Imports Microsoft.Office
Imports System.Runtime.InteropServices

Partial Class Modulos_Parametria_Tablas_Entidades_frmImportarRating
    Inherits BasePage
#Region "Variables"
    Dim oUtil As New UtilDM
    Dim sFileName As String
    Dim oRatingBM As New RatingBM
    Dim oRatingBE As New RatingBE
    Dim contadorFilasProcesadas As Integer = 0
    Dim cantRegistros As Integer = 0
#End Region
    Private RutaDestino As String = String.Empty
    Private Const NombreHoja As String = "Hoja1"
    Private Const RangoDatos As String = "A1:C3000"
    Private Const ExtensionValida As String = ".xls,.xlsx,"  'CRumiche | 2018-10-15 | Para acptar xls y xlsx

    Private Const TipoArchivoTercero As String = "TER"
    Private Const TipoArchivoEmision As String = "EMI"


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombo()
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Confirmacion();")
            Me.tbFechaProceso.Text = oUtil.RetornarFechaSistema
            Me.tbFechaProceso.Text = Now.ToString("dd/MM/yyyy")

            If Request.QueryString("frmPadre") IsNot Nothing Then
                btnSalir.Text = "Retornar"
            End If
        End If
    End Sub

    Public Sub CargarCombo()
        Dim dtTipoArchivo As New DataTable
        Dim dr As DataRow
        dtTipoArchivo.Columns.Add("CodigoTipoArchivo", GetType(String))
        dtTipoArchivo.Columns.Add("Descripcion", GetType(String))

        dr = dtTipoArchivo.NewRow()
        dr("CodigoTipoArchivo") = "TODOS"
        dr("Descripcion") = "Todos"
        dtTipoArchivo.Rows.Add(dr)

        dr = dtTipoArchivo.NewRow()
        dr("CodigoTipoArchivo") = TipoArchivoEmision
        dr("Descripcion") = "Emisión"
        dtTipoArchivo.Rows.Add(dr)

        dr = dtTipoArchivo.NewRow()
        dr("CodigoTipoArchivo") = TipoArchivoTercero
        dr("Descripcion") = "Tercero"
        dtTipoArchivo.Rows.Add(dr)

        HelpCombo.LlenarComboBox(ddlTipoArchivo, dtTipoArchivo, "CodigoTipoArchivo", "Descripcion", False)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        If btnSalir.Text.Equals("Retornar") Then
            If Request.QueryString("frmPadre").Equals("frmHistoriaRating") Then
                Response.Redirect("../../../Riesgos/frmHistoriaRating.aspx")
            End If
        Else
            Response.Redirect("../../../../frmDefault.aspx")
        End If
    End Sub

    Private Sub ProcesarArchivoEmision()
        'Dim codigosMnemonicosDuplicados As IEnumerable(Of Object) = Nothing
        'Dim codigosMnemonicosConCero As IEnumerable(Of Object) = Nothing

        'If Not valExistenDuplicadosPrimeraColumna(codigosMnemonicosDuplicados) Then
        '    Dim strCodMnemonicoDuplicados As String = String.Join("<br />", codigosMnemonicosDuplicados.Select(Function(d) d))
        '    AlertaJS("No se puede procesar el archivo. Existen instrumentos duplicados. <br />Códigos ISIN: <br />" & strCodMnemonicoDuplicados)
        'ElseIf Not ValValoresEmision(codigosMnemonicosConCero) Then
        '    Dim strCodMnemonicoConCero As String = String.Join("<br />", codigosMnemonicosConCero.Select(Function(d) d))
        '    AlertaJS("No se puede procesar el archivo. Existen registros sin Rating. <br />Códigos ISIN: <br />" & strCodMnemonicoConCero)
        'Else
        '    CargarMasivos(ddlTipoArchivo.SelectedValue)
        'End If

        ''CargarRatingDesdeArchivo(ddlTipoArchivo.SelectedValue)
        ''AlertaJS("Archivo cargado correctamente.")
    End Sub

    Private Sub ProcesarArchivoTercero()
        Dim codigosTercerosDuplicados As IEnumerable(Of Object) = Nothing
        Dim codigosTercerosConCero As IEnumerable(Of Object) = Nothing

        If Not valExistenDuplicadosPrimeraColumna(codigosTercerosDuplicados) Then
            Dim strCodTerceroDuplicados As String = String.Join("<br />", codigosTercerosDuplicados.Select(Function(d) d))
            AlertaJS("No se puede procesar el archivo. Existen terceros duplicados. <br />Códigos de Tercero: <br />" & strCodTerceroDuplicados)
        ElseIf Not ValValoresTercero(codigosTercerosConCero) Then
            Dim strCodTerceroConCero As String = String.Join("<br />", codigosTercerosConCero.Select(Function(d) d))
            AlertaJS("No se puede procesar el archivo. Existen registros sin Rating/Rating Interno. <br />Códigos de Tercero: <br />" & strCodTerceroConCero)
        Else
            CargarMasivos(ddlTipoArchivo.SelectedValue)
        End If
        AlertaJS("Archivo cargado correctamente.")
    End Sub

    Sub CargarRatingDesdeArchivo(ByVal tipoArchivo As String)
        Dim fechaProceso As Decimal
        fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))
        Dim pathExcel As String = hfRutaDestino.Value & System.Guid.NewGuid().ToString().Substring(0, 8) & "_" & iptRuta.Value
        Dim mensajePersonalizado As String = ""
        iptRuta.PostedFile.SaveAs(pathExcel)

        Dim listaTiposCarga As New List(Of String)
        If tipoArchivo.Equals("EMI") Or tipoArchivo.Equals("TODOS") Then listaTiposCarga.Add("EMI")
        If tipoArchivo.Equals("TER") Or tipoArchivo.Equals("TODOS") Then listaTiposCarga.Add("TER")

        Dim dtRating As RatingBE = ExcelHaciaDataTable(pathExcel, listaTiposCarga)

        'For Each tipoCarga As String In listaTiposCarga
        '    oRatingBM.Borrar_Rating(fechaProceso, tipoCarga, MyBase.DatosRequest())
        'Next

        For Each row As RatingBE.RegistroRatingRow In dtRating.RegistroRating
            'row.Fecha = fechaProceso
            oRatingBM.Insertar(row, MyBase.DatosRequest())
        Next
    End Sub

    Private Sub CargarMasivos(Optional ByVal tipoArchivo As String = TipoArchivoTercero)
        Dim dtRating As New DataTable
        Dim fechaProceso As Decimal

        msgError.Visible = False
        msgTotalRegistrosProcesados.Visible = False

        sFileName = Myfile.Text
        fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))


        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value

            If fInfo.Extension = ExtensionValida Then
                iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)
                If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                    Try
                        CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtRating)
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    Finally
                        File.Delete(RutaDestino & "\" & fInfo.Name)
                    End Try
                Else
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End If
            Else
                If fInfo.Extension.Equals("") Then
                    AlertaJS("La extensión del archivo no es válida.")
                Else
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                    AlertaJS("La extensión del archivo no es válida.")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString)
        End Try
        fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))

        oRatingBM.Borrar_Rating(fechaProceso, tipoArchivo, MyBase.DatosRequest())

        Dim mensajePersonalizado As String = ""
        Dim banderaProcesamientoDetenido = InsertarRating(dtRating, fechaProceso, tipoArchivo, mensajePersonalizado)
        gvResumenDataCargada.PageIndex = 0
        CargarGrilla()

        If banderaProcesamientoDetenido Then
            Dim messageT As String = "Error al procesar. Se procesaron " & contadorFilasProcesadas & " sobre un total de " & cantRegistros & "."
            messageT = messageT & mensajePersonalizado
            AlertaJS(messageT)
        Else
            If contadorFilasProcesadas <> cantRegistros Then
                AlertaJS("Se procesaron " & contadorFilasProcesadas & " sobre un total de " & cantRegistros)
            Else
                AlertaJS("El Archivo Plano ha sido importado correctamente")
            End If
        End If
    End Sub

    Private Sub ProcesarArchivo()
        Try
            Dim tipoArchivoValue As String = ddlTipoArchivo.SelectedValue
            Select Case tipoArchivoValue
                Case TipoArchivoEmision
                    ProcesarArchivoEmision()
                Case TipoArchivoTercero
                    ProcesarArchivoTercero()
            End Select
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub

    ' Validacion de los campos obligatorios: Fecha Proceso, Tipo Archivo y Archivo.
    Private Function IsValidoCamposObligatorios(ByRef msjValidacion As String) As Boolean
        ' Validacion de Fecha de Proceso
        Dim fechaProceso As String = CStr(UIUtility.ConvertirFechaaDecimal(tbFechaProceso.Text))
        If String.IsNullOrEmpty(fechaProceso) Then
            msjValidacion = "La Fecha de Proceso no es válida. Revisar la Fecha de Proceso."
            Return False
        End If

        Try
            DateTime.ParseExact(fechaProceso, "yyyyMMdd", Nothing)
        Catch ex As Exception
            msjValidacion = "La Fecha de Proceso no es válida. Revisar la Fecha de Proceso."
            Return False
        End Try

        ' Validacion de Tipo Archivo
        Dim tipoArchivoValue As String = ddlTipoArchivo.SelectedValue
        If String.IsNullOrEmpty(tipoArchivoValue) Then
            msjValidacion = "El Tipo de Archivo no es válido. Revisar el Tipo de Archivo."
            Return False
        End If

        ' Validacion de Archivo.
        Dim fileNameClient As String = iptRuta.PostedFile.FileName
        If String.IsNullOrEmpty(fileNameClient) Then
            msjValidacion = "El nombre del archivo no es válido. Revisar el Nombre del Archivo."
            Return False
        End If

        msjValidacion = ""
        Return True
    End Function

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim msjValidacion As String = ""

        Try
            If Not IsValidoCamposObligatorios(msjValidacion) Then
                AlertaJS(msjValidacion)
            ElseIf Not ValExtensionArchivoValido() Then
                AlertaJS("La extensión del archivo no es válido")
            Else
                CargarRuta()
                RutaDestino = hfRutaDestino.Value

                If RutaDestino = "" Then
                    AlertaJS("No se ha especificado la ruta destino del archivo.")
                Else
                    'ProcesarArchivo()
                    CargarRatingDesdeArchivo(ddlTipoArchivo.SelectedValue)
                    AlertaJS("Archivo cargado correctamente")
                    'CargarGrillaFiltro()
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", "").Replace(Environment.NewLine, ""))
        End Try
    End Sub

    Private Sub CargarRuta()
        RutaDestino = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")

        If (Not Directory.Exists(RutaDestino)) Then
            AlertaJS("No existe la ruta destino.")
            Exit Sub
        End If

        hfRutaDestino.Value = RutaDestino
        btnProcesar.Enabled = True
    End Sub

#Region "Carga de Archivos Excel"

    Public Shared Function ifNull(ByVal o As Object, ByVal defaultValue As Object) As Object
        If o Is Nothing Then
            Return defaultValue
        End If

        If TypeOf o Is Date Then
            Return CDate(o).ToString("yyyyMMdd")
        End If

        Return o
    End Function

    Function ExcelHaciaDataTable(ByVal pathArchivo As String, ByVal listaTiposCarga As List(Of String)) As RatingBE
        Dim oRatingBE As New RatingBE
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application = Nothing
        Try
            Const _MAX_FILAS_EXCEL As Integer = 5000
            'Const _FILA_INICIAL_LECTURA As Integer = 3
            Const _FILA_INICIAL_LECTURA As Integer = 2
            '--Emisores y emosires
            Dim ColExcel_Fecha As Integer = 1 'Ubicación en el EXCEL


            'Emisiones
            Dim ColExcel_Tipo_Negocio As Integer = 2 'Ubicación en el EXCEL
            Dim ColExcel_CodigoISIN As Integer = 3 'Ubicación en el EXCEL
            'Dim ColExcel_Nemonico As Integer = 4 'Ubicación en el EXCEL
            ' Dim ColExcel_CodigoSBS As Integer = 5 'Ubicación en el EXCEL
            Dim ColExcel_Rating_V As Integer = 6 'Ubicación en el EXCEL
            Dim ColExcel_Clasificadora As Integer = 7 'Ubicación en el EXCEL
            Dim ColExcel_FechaClasificacion As Integer = 8 'Ubicación en el EXCEL

            ' Indices de Columnas en el Excel para EMISORES
            Dim ColExcel_CodigoTercero As Integer = 2 'Ubicación en el EXCEL
            Dim ColExcel_Rating As Integer = 3 'Ubicación en el EXCEL
            Dim ColExcel_RatingInterno As Integer = 4 'Ubicación en el EXCEL
            Dim ColExcel_RatingFF As Integer = 5 'Ubicación en el EXCEL
            Dim ColExcel_LineaPlazo As Integer = 6 'Ubicación en el EXCEL

            ' Indices de Columnas en el Excel para VALORES (EMISIONES)
            'Dim ColExcel_CodigoISIN As Integer = 2 'Ubicación en el EXCEL
            Dim hojaLectura As Integer

            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = ObjCom.ObjetoAplication

            Dim oBooks As Excel.Workbooks = xlApp.Workbooks
            oBooks.Open(pathArchivo, [ReadOnly]:=True, IgnoreReadOnlyRecommended:=True)

            Dim xlLibro As Excel.Workbook = oBooks.Item(1)
            Dim oSheets As Excel.Sheets = xlLibro.Worksheets

            For Each tipoCarga As String In listaTiposCarga

                If tipoCarga.Equals("EMI") Then
                    'ColExcel_Rating = 5
                    hojaLectura = 1 'VALORES (EMISIONES)                
                Else
                    'ColExcel_Rating = 3
                    hojaLectura = 2 'EMISORES
                End If

                Dim hojaConfig As Excel.Worksheet = oSheets.Item(hojaLectura)
                Dim filaActual As Integer = 0

                While filaActual < _MAX_FILAS_EXCEL
                    Dim row As RatingBE.RegistroRatingRow = oRatingBE.RegistroRating.NewRegistroRatingRow()

                    row.Tipo = tipoCarga
                    row.Fecha = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Fecha).Value(), 0)

                    If tipoCarga.Equals("EMI") Then 'VALORES (EMISIONES)                         
                        row.TipoNegocio = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Tipo_Negocio).Value(), "")
                        row.Codigo = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoISIN).Value(), "")
                        'row.Nemonico = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Nemonico).Value(), "")
                        'row.CodigoSBS = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoSBS).Value(), "")
                        row.Rating = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Rating_V).Value(), "")
                        row.Clasificadora = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Clasificadora).Value(), "")
                        row.FechaClasificacion = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_FechaClasificacion).Value(), 0)
                    Else 'EMISORES o TERCEROS
                        row.Codigo = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoTercero).Value(), "")
                        row.Rating = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Rating).Value(), "")
                        row.RatingInterno = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_RatingInterno).Value(), "")
                        row.RatingFF = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_RatingFF).Value(), "")
                        row.LineaPlazo = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_LineaPlazo).Value(), "")
                    End If

                    If (row.Rating <> "") Then
                        row.Rating = ifNull(ObtenerCodigoRatingxNombre(Convert.ToString(row.Rating.ToString())), "")
                    End If
                    If (row.RatingInterno <> "") Then
                        row.RatingInterno = ifNull(ObtenerCodigoRatingxNombre(Convert.ToString(row.RatingInterno.ToString())), "")
                    End If
                    If (row.RatingFF <> "") Then
                        row.RatingFF = ifNull(ObtenerCodigoRatingxNombre(Convert.ToString(row.RatingFF.ToString())), "")
                    End If


                    If Not row.Fecha.Equals(0) And Not row.Codigo.Equals("") Then
                        oRatingBE.RegistroRating.AddRegistroRatingRow(row)
                    Else
                        Exit While
                    End If

                    filaActual += 1
                End While
            Next
        Catch ex As Exception
            If xlApp IsNot Nothing Then
                xlApp.Quit()
                Marshal.ReleaseComObject(xlApp)
            End If

            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()

            Throw ex
        Finally
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try

        Return oRatingBE
    End Function

#End Region


    Private Function InsertarRating(ByVal dtRating As DataTable, ByVal fechaProceso As Decimal, ByVal tipoArchivo As String _
                                              , ByRef mensajePersonalizado As String) As Boolean
        Dim errorVacio As Boolean = False
        Dim banderaProcesamientoDetenido As Boolean = False
        cantRegistros = dtRating.Rows.Count
        contadorFilasProcesadas = 0

        If dtRating.Rows.Count > 0 Then
            oRatingBE = New RatingBE
            For index = 0 To dtRating.Rows.Count - 1
                Try
                    Dim oRating As RatingBE.RegistroRatingRow = _
                        oRatingBE.RegistroRating.NewRegistroRatingRow()

                    If dtRating.Rows(index)(0) Is DBNull.Value Then
                        banderaProcesamientoDetenido = True
                        Exit For
                    End If

                    oRating.Codigo = dtRating.Rows(index)(0).ToString()
                    oRating.Rating = dtRating.Rows(index)(1).ToString
                    oRating.RatingInterno = dtRating.Rows(index)(2).ToString

                    ' ''oRating.RatingFF = dtRating.Rows(index)(1).ToString
                    ' ''oRating.LineaPlazo = dtRating.Rows(index)(1).ToString

                    oRating.Fecha = fechaProceso
                    oRating.Tipo = tipoArchivo

                    oRatingBM.Insertar(oRating, MyBase.DatosRequest())
                    oRatingBE.RegistroRating.AddRegistroRatingRow(oRating)
                    If contadorFilasProcesadas = 80 Then
                        contadorFilasProcesadas = contadorFilasProcesadas + 1
                    Else
                        contadorFilasProcesadas = contadorFilasProcesadas + 1
                    End If

                Catch ex As Exception
                    If ex.Message.ToUpper.Contains("FOREIGN KEY") Then
                        mensajePersonalizado = "<br />El Código " & If(tipoArchivo = TipoArchivoEmision, "ISIN ", "de Tercero") &
                        dtRating.Rows(index)(0).ToString() & " no existe."
                    End If

                    banderaProcesamientoDetenido = True
                    Exit For
                End Try
            Next
        Else
            errorVacio = True
        End If

        If errorVacio Then
            msgError.Visible = True
            msgError.Text = "No hay datos en el Archivo Plano..."
        End If
        'If Not errorVacio Then
        '    AlertaJS("El Archivo Plano ha sido importado correctamente")
        'End If
        Return banderaProcesamientoDetenido
    End Function

    Private Function ObtenerValorValidoCampoNoObligatorioDecimal(ByVal campo As Object) As Decimal
        If campo Is DBNull.Value Then
            Return 0
        End If

        If Not IsNumeric(campo) Then
            Return 0
        End If

        Try
            Dim valueDecimal As Decimal = Convert.ToDecimal(campo)
            If valueDecimal < 0 Then
                Return 0
            Else
                Return valueDecimal
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Sub CargarRango(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dt As DataTable)
        Try
            If System.IO.File.Exists(sFileName) Then
                Dim objDataSet As System.Data.DataSet
                Dim objDataAdapter As System.Data.OleDb.OleDbDataAdapter
                Dim sCs As String = ""
                If System.Configuration.ConfigurationManager.AppSettings("Ambiente").ToString() = "Desarrollo" Then
                    sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Excel 8.0;"
                Else
                    sCs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"
                End If
                Dim objOleConnection As System.Data.OleDb.OleDbConnection
                objOleConnection = New System.Data.OleDb.OleDbConnection(sCs)
                Dim sSql As String = "select * from " & "[" & sSheetName & "$" & sRange & "]"
                objDataAdapter = New System.Data.OleDb.OleDbDataAdapter(sSql, objOleConnection)
                objDataSet = New System.Data.DataSet
                objDataAdapter.Fill(objDataSet)
                dt = objDataSet.Tables(0)
                objOleConnection.Close()
            Else
                AlertaJS("No se ha encontrado el archivo: " & sFileName)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Function validar() As Boolean
        Dim fecha As String = CStr(UIUtility.ConvertirFechaaDecimal(tbFechaProceso.Text))
        If String.IsNullOrEmpty(fecha) Then
            Return False
        End If
        Try
            DateTime.ParseExact(fecha, "yyyyMMdd", Nothing)
        Catch ex As Exception
            Return False
        End Try

        'fecha = fecha.Substring(4, 4)

        Dim fileNameClient As String = iptRuta.PostedFile.FileName
        If String.IsNullOrEmpty(fileNameClient) Then
            Return False
        End If

        Return True
    End Function

    Private Sub CargarGrilla()
        Dim fechaProceso As Decimal = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))
        Dim tipoArchivo As String = ddlTipoArchivo.SelectedValue
        Dim dtblDatos As DataTable = oRatingBM.SeleccionarPorFecha(fechaProceso, tipoArchivo, MyBase.DatosRequest())

        msgTotalRegistrosProcesados.Text = "Total de registros: " & dtblDatos.Rows.Count
        msgTotalRegistrosProcesados.Visible = True
        gvResumenDataCargada.Visible = True
        gvResumenDataCargada.DataSource = dtblDatos
        gvResumenDataCargada.DataBind()
    End Sub

    Protected Sub gvResumenDataCargada_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResumenDataCargada.PageIndexChanging
        Try
            gvResumenDataCargada.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Private Sub CargarGrillaFiltro()
        Dim tipoArchivo = ddlTipoArchivo.SelectedValue
        If String.IsNullOrEmpty(tipoArchivo) Then
            msgError.Visible = False
            msgTotalRegistrosProcesados.Visible = False
            gvResumenDataCargada.DataSource = Nothing
            gvResumenDataCargada.Visible = False

            Exit Sub
        End If

        Dim fechaProceso As Decimal
        Try
            fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))
        Catch ex As Exception
            msgError.Visible = False
            msgTotalRegistrosProcesados.Visible = False
            gvResumenDataCargada.DataSource = Nothing
            gvResumenDataCargada.Visible = False

            Exit Sub
        End Try

        Dim dtblDatos As DataTable = oRatingBM.SeleccionarPorFecha(fechaProceso, tipoArchivo, MyBase.DatosRequest())

        If dtblDatos.Rows.Count > 0 Then
            msgError.Visible = False
            msgError.Text = ""
            msgTotalRegistrosProcesados.Visible = True
            msgTotalRegistrosProcesados.Text = "Total de registros: " & dtblDatos.Rows.Count
            gvResumenDataCargada.DataSource = dtblDatos
            gvResumenDataCargada.DataBind()
            gvResumenDataCargada.Visible = True
        Else
            msgError.Visible = False
            msgTotalRegistrosProcesados.Visible = False
            gvResumenDataCargada.DataSource = Nothing
            gvResumenDataCargada.Visible = False
        End If
    End Sub

    Protected Sub ddlTipoArchivo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoArchivo.SelectedIndexChanged
        'CargarGrillaFiltro()
    End Sub

    Protected Sub tbFechaProceso_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaProceso.TextChanged
        'CargarGrillaFiltro()
    End Sub

    Private Function valExistenDuplicadosPrimeraColumna(ByRef codigosPrimeraColumnaDuplicados As IEnumerable(Of Object)) As Boolean
        Dim dtPatrimonioTercero As New DataTable
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)

            If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                Try
                    CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtPatrimonioTercero)
                Catch ex As Exception
                    Return True
                Finally
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End Try
            Else
                File.Delete(RutaDestino & "\" & fInfo.Name)
            End If
        Catch ex As Exception
            Return True
        End Try

        Dim duplicados = dtPatrimonioTercero.AsEnumerable().Where(Function(r) r(0) IsNot DBNull.Value).GroupBy(Function(p) p(0)).Where(Function(gr) gr.Count > 1).ToList()
        If duplicados IsNot Nothing And duplicados.Count > 0 Then
            codigosPrimeraColumnaDuplicados = duplicados.SelectMany(Function(e) e).Select(Function(e) e.ItemArray(0)).Distinct()
            Return False
        End If

        Return True
    End Function

    Private Function ValValoresEmision(ByRef codigosTercerosConCero As IEnumerable(Of Object)) As Boolean
        Dim dtPatrimonioTercero As New DataTable
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)

            If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                Try
                    CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtPatrimonioTercero)
                Catch ex As Exception
                    Return True
                Finally
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End Try
            Else
                File.Delete(RutaDestino & "\" & fInfo.Name)
            End If
        Catch ex As Exception
            Return True
        End Try

        For Each row As DataRow In dtPatrimonioTercero.Rows
            If row(1) Is DBNull.Value Or row(1) Is String.Empty Then row(1) = ""
        Next

        Dim fnRegistrosConCero = Function(r As DataRow)
                                     Return r(1) = ""
                                 End Function
        Dim registrosConCero As Generic.List(Of DataRow) = dtPatrimonioTercero.AsEnumerable().Where(fnRegistrosConCero).ToList()
        If registrosConCero IsNot Nothing And registrosConCero.Count > 0 Then
            codigosTercerosConCero = registrosConCero.Select(Function(e) e.ItemArray(0)).Distinct()
            Return False
        End If

        Return True
    End Function

    Private Function ValValoresTercero(ByRef codigosTercerosConCero As IEnumerable(Of Object)) As Boolean
        Dim dtPatrimonioTercero As New DataTable
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)

            If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                Try
                    CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtPatrimonioTercero)
                Catch ex As Exception
                    Return True
                Finally
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End Try
            Else
                File.Delete(RutaDestino & "\" & fInfo.Name)
            End If
        Catch ex As Exception
            Return True
        End Try

        For Each row As DataRow In dtPatrimonioTercero.Rows
            If row(1) Is DBNull.Value Or row(1) Is String.Empty Then row(1) = ""
            If row(2) Is DBNull.Value Or row(1) Is String.Empty Then row(2) = ""
        Next

        Dim fnRegistrosConCero = Function(r As DataRow)
                                     Return r(1) = "" Or r(2) = ""
                                 End Function
        Dim registrosConCero As Generic.List(Of DataRow) = dtPatrimonioTercero.AsEnumerable().Where(fnRegistrosConCero).ToList()
        If registrosConCero IsNot Nothing And registrosConCero.Count > 0 Then
            codigosTercerosConCero = registrosConCero.Select(Function(e) e.ItemArray(0)).Distinct()
            Return False
        End If

        Return True
    End Function

    Private Function ValExtensionArchivoValido() As Boolean
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value

            If ExtensionValida.Contains(fInfo.Extension & ",") Then 'CRumiche | 2018-10-15 | Para acptar xls y xlsx
                Return True
            Else
                If fInfo.Extension.Equals("") Then
                    Return False
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ObtenerCodigoRatingxNombre(ByVal Rating As String) As String
        Dim strValor As String
        Try

            Dim tb As New DataTable
            tb = oRatingBM.ObtenerCodigoRatingxNombre(Rating)

            strValor = tb(0)("Valor")


        Catch ex As Exception

            strValor = ""
            Return ex.Message

        End Try
        Return strValor

    End Function

End Class