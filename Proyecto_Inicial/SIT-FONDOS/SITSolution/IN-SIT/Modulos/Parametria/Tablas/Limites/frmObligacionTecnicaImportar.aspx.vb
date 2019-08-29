Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.Marshal
Imports System
Imports System.IO
Imports System.Runtime.InteropServices

Partial Class Modulos_Parametria_Tablas_Limites_frmObligacionTecnicaImportar
    Inherits BasePage

    Private Const ExtensionValida As String = ".xls,.xlsx,"
    Private RutaDestino As String = String.Empty
    Private FechaObligacionTecnica As Decimal

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaObligacionTecnica.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click
        'Try
        '    Dim strRuta As String = (New ParametrosGeneralesBM().Listar("RUTA_TEMP", Nothing)).Rows(0)("Valor")
        '    If Not iptRuta.Value.ToString.Trim.Equals("") Then
        '        CargarArchivoOrigen(strRuta)
        '    Else
        '        AlertaJS("Especifique la ruta de un archivo")
        '    End If
        'Catch ex As Exception
        '    AlertaJS("Ocurrió un error al Procesar")
        'End Try
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
                    CargarDesdeArchivo()
                    AlertaJS("Archivo cargado correctamente")
                    'CargarGrillaFiltro()
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", "").Replace(Environment.NewLine, ""))
        End Try
    End Sub

    'Private Sub CargarArchivoOrigen(ByVal strRuta As String)



    '    'Try
    '    '    Dim fInfo As New FileInfo(iptRuta.Value)

    '    '    'PRIMERO SE SUBE EL ARCHIVO
    '    '    'Dim pathExcel As String = strRuta & "\" & iptRuta.Value
    '    '    'If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
    '    '    iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)

    '    '    'SE VERIFICA Q EXISTA EL ARCHIVO
    '    '    If Dir(strRuta & "\" & fInfo.Name) <> "" Then
    '    '        If fInfo.Extension = ".xls" Or fInfo.Extension = ".xlsx" Then
    '    '            Try
    '    '                LeerExcel(strRuta & "\" & fInfo.Name)
    '    '            Catch ex As Exception
    '    '                AlertaJS(ex.Message)
    '    '            Finally
    '    '                File.Delete(strRuta & "\" & fInfo.Name)
    '    '            End Try
    '    '        Else
    '    '            If fInfo.Extension.Equals("") Then
    '    '                AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
    '    '            Else
    '    '                File.Delete(strRuta & "\" & fInfo.Name)
    '    '                AlertaJS("El tipo de archivo no es válido")
    '    '            End If
    '    '        End If
    '    '    Else
    '    '        File.Delete(strRuta & "\" & fInfo.Name)
    '    '        AlertaJS("El archivo no existe")
    '    '    End If
    '    'Catch ex As Exception
    '    '    AlertaJS(ex.ToString())
    '    'End Try
    'End Sub

    Private Function IsValidoCamposObligatorios(ByRef msjValidacion As String) As Boolean
        ' Validacion de Fecha de Proceso
        Dim fechaProceso As String = CStr(UIUtility.ConvertirFechaaDecimal(txtFecha.Text))
        If String.IsNullOrEmpty(fechaProceso) Then
            msjValidacion = "La Fecha no es válida. Revisar la Fecha."
            Return False
        End If

        Try
            DateTime.ParseExact(fechaProceso, "yyyyMMdd", Nothing)
        Catch ex As Exception
            msjValidacion = "La Fecha no es válida. Revisar la Fecha."
            Return False
        End Try

        ' Validacion de Archivo.
        Dim fileNameClient As String = iptRuta.PostedFile.FileName
        If String.IsNullOrEmpty(fileNameClient) Then
            msjValidacion = "El nombre del archivo no es válido. Revisar el Nombre del Archivo."
            Return False
        End If

        msjValidacion = ""
        Return True
    End Function

    Private Function ValExtensionArchivoValido() As Boolean
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)

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

    Private Sub CargarRuta()
        RutaDestino = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")

        If (Not Directory.Exists(RutaDestino)) Then
            AlertaJS("No existe la ruta destino.")
            Exit Sub
        End If

        hfRutaDestino.Value = RutaDestino
        btnProcesar.Enabled = True
    End Sub

    Private Sub CargarDesdeArchivo()
        'Dim oCarteraIndirectaBM As CarteraIndirectaBM
        Dim fechaProceso As Decimal
        'fechaProceso = Convert.ToDecimal(txtFecha.Text.Substring(6, 4) + txtFecha.Text.Substring(3, 2) + txtFecha.Text.Substring(0, 2))
        fechaProceso = CStr(UIUtility.ConvertirFechaaDecimal(txtFecha.Text))
        Dim pathExcel As String = hfRutaDestino.Value & System.Guid.NewGuid().ToString().Substring(0, 8) & "_" & iptRuta.Value
        Dim mensajePersonalizado As String = ""
        iptRuta.PostedFile.SaveAs(pathExcel)

        Dim dtObligacionTecnica As ObligacionTecnicaBE = ExcelHaciaDataTable(pathExcel, fechaProceso)
        'Dim lsCarteraIndirecta As List(Of CarteraIndirectaBE) = ExcelHaciaDataTable(pathExcel, fechaProceso)

        Dim dtA1 As ObligacionTecnicaBE
        Dim dtA2 As ObligacionTecnicaBE
        Dim Col1 As String, Col2 As String
        Dim xcon As Long

        dtA1 = dtObligacionTecnica
        dtA2 = dtObligacionTecnica

        For Each row As ObligacionTecnicaBE.ObligacionTecnicaRow In dtA1.ObligacionTecnica
            Col1 = row.CodigoPortafolioSBS.ToString().Trim()
            xcon = 0
            For Each row1 As ObligacionTecnicaBE.ObligacionTecnicaRow In dtA2.ObligacionTecnica
                Col2 = row1.CodigoPortafolioSBS.ToString().Trim()
                If (Col1 = Col2) Then
                    xcon += 1
                End If
            Next
            If xcon > 1 Then
                AlertaJS("No se ha podido cargar el archivo. Existe códigos de portafolio duplicados")
                'oConn.Close()
                Exit Sub
            End If
        Next

        InsertarDeExcel(fechaProceso, dtObligacionTecnica, MyBase.DatosRequest())

    End Sub

    Function ExcelHaciaDataTable(ByVal pathArchivo As String, ByVal FechaProceso As Decimal) As ObligacionTecnicaBE

        'Dim oListCarteraIndirectaBE As New List(Of CarteraIndirectaBE)
        Dim oObligacionTecnicaBE As New ObligacionTecnicaBE
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application = Nothing

        Try
            Const _MAX_FILAS_EXCEL As Integer = 5000

            Const _FILA_INICIAL_LECTURA As Integer = 2

            Dim ColExcel_Portafolio As Integer = 1 'Ubicación en el EXCEL
            Dim ColExcel_DesPortafolio As Integer = 2 'Ubicación en el EXCEL
            Dim ColExcel_Monto As Integer = 3 'Ubicación en el EXCEL


            Dim hojaLectura As Integer

            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = ObjCom.ObjetoAplication

            Dim oBooks As Excel.Workbooks = xlApp.Workbooks
            oBooks.Open(pathArchivo, [ReadOnly]:=True, IgnoreReadOnlyRecommended:=True)

            Dim xlLibro As Excel.Workbook = oBooks.Item(1)
            Dim oSheets As Excel.Sheets = xlLibro.Worksheets

            'ColExcel_Rating = 5
            hojaLectura = 1 'VALORES (EMISIONES)                

            Dim hojaConfig As Excel.Worksheet = oSheets.Item(hojaLectura)
            Dim filaActual As Integer = 0

            While filaActual < _MAX_FILAS_EXCEL

                'Dim row As RatingBE.RegistroRatingRow = oRatingBE.RegistroRating.NewRegistroRatingRow()
                'Dim oCarteraIndirectaBE As New CarteraIndirectaBE
                Dim row As ObligacionTecnicaBE.ObligacionTecnicaRow = oObligacionTecnicaBE.ObligacionTecnica.NewObligacionTecnicaRow()
                row.CodigoPortafolioSBS = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Portafolio).Value(), "")
                row.Descripcion = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_DesPortafolio).Value(), "")
                row.Monto = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Monto).Value(), 0)
                row.Fecha = FechaProceso

                If Not row.CodigoPortafolioSBS.Equals("") Then
                    oObligacionTecnicaBE.ObligacionTecnica.AddObligacionTecnicaRow(row)
                Else
                    Exit While
                End If

                    filaActual += 1
            End While

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

        Return oObligacionTecnicaBE
    End Function

    Public Shared Function ifNull(ByVal o As Object, ByVal defaultValue As Object) As Object
        If o Is Nothing Then
            Return defaultValue
        End If

        If TypeOf o Is Date Then
            Return CDate(o).ToString("yyyyMMdd")
        End If

        Return o
    End Function

    Public Sub InsertarDeExcel(ByVal fechaProceso As Decimal, ByVal Ob As ObligacionTecnicaBE, ByVal DatosRequest As DataSet)

        Dim oObligacionTecnicaBM As New ObligacionTecnicaBM
        Dim strMensaje As String = ""

        oObligacionTecnicaBM.DesactivarRegistrosExcel(fechaProceso, DatosRequest, strMensaje)

        If (String.IsNullOrEmpty(strMensaje)) Then

            For Each row As ObligacionTecnicaBE.ObligacionTecnicaRow In Ob.ObligacionTecnica
                'row.Fecha = fechaProceso
                oObligacionTecnicaBM.InsertarRegistrosExcel(row, MyBase.DatosRequest())
            Next

        End If
    End Sub
    'eliminar

    'Private Sub LeerExcel(ByVal strRuta As String)
    '    Try
    '        CargarArchivo(strRuta)
    '    Catch ex As OleDbException
    '        AlertaJS("Error al leer el archivo Excel")
    '    Catch ex As Exception
    '        AlertaJS(ex.Message)
    '    End Try
    'End Sub

    'Private Sub CargarArchivo(ByVal strRuta As String)

    '    Dim oConn As New OleDbConnection
    '    Dim oCmd As New OleDbCommand
    '    Dim oDa As New OleDbDataAdapter
    '    Dim oDs As New DataSet
    '    Dim strMensaje As String = ""

    '    Try
    '        'oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
    '        oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
    '        oConn.Open()

    '        oCmd.CommandText = "SELECT DISTINCT [CodigoPortafolio], [Monto] FROM [ObligacionTecnica$] "
    '        oCmd.Connection = oConn
    '        oDa.SelectCommand = oCmd
    '        oDa.Fill(oDs, "ObligacionTecnica")

    '        Dim dt1 As DataTable, dt2 As DataTable
    '        Dim sCod1 As String, sCod2 As String, nCon As Long
    '        dt1 = oDs.Tables(0)
    '        dt2 = oDs.Tables(0)

    '        For Each filaLinea1 As DataRow In dt1.Rows
    '            sCod1 = filaLinea1(0).ToString().Trim()
    '            nCon = 0
    '            For Each filaLinea2 As DataRow In dt2.Rows
    '                sCod2 = filaLinea2(0).ToString().Trim()
    '                If sCod1 = sCod2 Then
    '                    nCon += 1
    '                End If
    '            Next
    '            If nCon > 1 Then
    '                AlertaJS("No se ha podido cargar el archivo.")
    '                oConn.Close()
    '                Exit Sub
    '            End If
    '        Next

    '        FechaObligacionTecnica = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)

    '        InsertarObligacionesTecnicasPorExcel(FechaObligacionTecnica, oDs.Tables(0), "PatrimonioFideicomiso", strMensaje)
    '        AlertaJS(strMensaje)
    '        oConn.Close()
    '    Catch ex As OleDbException
    '        Select Case ex.ErrorCode
    '            Case -2147467259
    '                AlertaJS("El excel en este momento se encuentra abierto")
    '            Case -2147217865
    '                AlertaJS("Error al leer el archivo Excel")
    '            Case Else
    '                AlertaJS("Ocurrió un error no esperado. Revisar el archivo Excel")
    '        End Select
    '        oConn.Close()

    '    Catch ex As Exception
    '        AlertaJS("Error al leer el archivo Excel")
    '        oConn.Close()
    '    End Try
    'End Sub

    'Private Sub InsertarObligacionesTecnicasPorExcel(ByVal FechaObligacionTecnica As Decimal, ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
    '    If data.Rows.Count = 0 Then
    '        strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
    '    Else
    '        Dim oObligacionTecnicaBM As New ObligacionTecnicaBM
    '        If data.Columns.Count = 2 Then
    '            If oObligacionTecnicaBM.InsertarObligacionesTecnicasPorExcel(FechaObligacionTecnica, data, DatosRequest, strmensaje) Then

    '            Else
    '                strmensaje &= "Los datos de " & strReferencia & " son inconsistentes \n"
    '            End If
    '        Else
    '            strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada\n"
    '        End If
    '    End If
    'End Sub
End Class
