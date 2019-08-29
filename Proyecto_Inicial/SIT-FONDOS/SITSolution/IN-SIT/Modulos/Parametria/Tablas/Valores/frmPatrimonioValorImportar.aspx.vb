
Imports SIT.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections

Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.Marshal
Imports System
Imports System.IO

Partial Class Modulos_Parametria_Tablas_Valores_frmPatrimonioValorImportar
    Inherits BasePage

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then

            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPatrimonioValor.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            'Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("RUTA_TEMP", Nothing)).Rows(0)("Valor")
            If Not iptRuta.Value.ToString.Trim.Equals("") Then
                CargarArchivoOrigen(strRuta)
                AlertaJS("La carga se realizó correctamente")
            Else
                AlertaJS("Especifique la ruta de un archivo")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Procesar: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

#End Region

#Region "/* Métodos Personalizados */"

    Private Sub ActualizarPatrimonioValorPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oPatrimonioValorBM As New PatrimonioValorBM
            If data.Columns.Count = 6 Then
                If oPatrimonioValorBM.ActualizarPatrimonioValorPorExcel(data, DatosRequest, strmensaje) Then
                    'strmensaje &= "Los datos de " & strReferencia & " cargaron correctamente\n"
                Else
                    strmensaje &= "Los datos de " & strReferencia & " son inconsistentes \n"
                End If
            Else
                strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada\n"
            End If
        End If
    End Sub

    Private Sub CargarArchivo(ByVal strRuta As String)

        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim strMensaje As String = ""

        Try
            'oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()

            'oCmd.CommandText = "SELECT [Tipo Instrumento], [Mnemonico], [Patrimonio], [Fecha Vigencia], [Situacion] FROM [PatrimonioValor$] "
            oCmd.CommandText = "SELECT [Tipo Instrumento], [Mnemonico], [Patrimonio], [Fecha Vigencia], [Nueva fecha de vigencia], [Situacion] FROM [PatrimonioValor$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "PatrimonioValor")
            ActualizarPatrimonioValorPorExcel(oDs.Tables(0), "PatrimonioValor", strMensaje)
            AlertaJS(strMensaje)
            oConn.Close()
        Catch ex As OleDbException
            Select Case ex.ErrorCode
                Case -2147467259
                    AlertaJS("El excel en este momento se encuentra abierto")
                Case -2147217865
                    AlertaJS("Error al leer el archivo Excel")
                Case Else
                    AlertaJS("Ocurrió un error no esperado. Revisar el archivo Excel")
            End Select
            oConn.Close()
        Catch ex As Exception
            'AlertaJS("Error al leer el archivo Excel")
            oConn.Close()
            Throw ex
        End Try
    End Sub

    Private Sub LeerExcel(ByVal strRuta As String)
        Try
            'Cargar datos de las Emisiones
            Dim dtEmision As DataTable = Nothing
            dtEmision = InstanciarTableEmision(dtEmision)
            CargarEmision(strRuta, dtEmision)

            'Cargar datos de los emisores
            Dim dtEmisor As DataTable = Nothing
            dtEmisor = InstanciarTableEmisor(dtEmisor)
            CargarEmisor(strRuta, dtEmisor)

            'Guardar registros
            Dim objPatValBM As New PatrimonioValorBM
            objPatValBM.GuardarRegistros(dtEmision, dtEmisor, DatosRequest)

            'CargarArchivo(strRuta)
            'loadData(strRuta, "EMISION", "A2:C1500", dtPatVal)
            'loadData(strRuta, "EMISOR", "A2:D1500", dtPatEmi)
        Catch ex As Exception
            'AlertaJS(ex.ToString)
            Throw ex
        End Try
    End Sub

    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)

            'PRIMERO SE SUBE EL ARCHIVO
            'If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            If Dir(strRuta & "\" & fInfo.Name) <> "" Then
                If fInfo.Extension = ".xls" Or fInfo.Extension = ".xlsx" Then
                    Try
                        LeerExcel(strRuta & "\" & fInfo.Name)
                    Catch ex As Exception
                        'AlertaJS(ex.ToString)
                        Throw ex
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
                        'strmensaje = "<script language='JavaScript'> alert('Error al leer el archivo')</script>"
                        'Page.RegisterStartupScript("Mensaje", strmensaje)
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        'AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                        Throw New Exception("La ruta no es válida. Especificar ruta de un archivo")
                    Else
                        File.Delete(strRuta & "\" & fInfo.Name)
                        AlertaJS("El tipo de archivo no es válido")
                        Throw New Exception("El tipo de archivo no es válido")
                    End If
                End If
            Else
                File.Delete(strRuta & "\" & fInfo.Name)
                'AlertaJS("El archivo no existe")
                Throw New Exception("El archivo no existe")
            End If
        Catch ex As Exception
            'AlertaJS(ex.ToString)
            Throw ex
        End Try
    End Sub

    Private Sub loadData(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dtResult As DataTable)
        Try
            If System.IO.File.Exists(sFileName) Then
                Using cn As New OleDbConnection
                    Dim sCs As String = String.Empty
                    If System.Configuration.ConfigurationManager.AppSettings("Ambiente").ToString() = "Desarrollo" Then
                        sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Excel 8.0;"
                    Else
                        sCs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"
                    End If
                    cn.ConnectionString = sCs
                    Dim sql As String = "select * from " & "[" & sSheetName & "$" & sRange & "]"
                    Dim objDataAdapter As New OleDbDataAdapter(sql, cn)
                    'dtResult = New DataTable
                    objDataAdapter.Fill(dtResult)
                End Using
            End If
        Catch ex As Exception
            'Return "Hubo un error en la obtención de data del archivo/ " + ex.Message.ToString()
            Throw New Exception("Hubo un error en la obtención de data del archivo/ " + Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Function InstanciarTableEmision(ByRef dt As DataTable) As DataTable
        dt = New DataTable
        dt.Columns.Add("CodigoNemonico", GetType(String))
        dt.Columns.Add("Valor", GetType(Decimal))
        dt.Columns.Add("Fecha", GetType(Date))
        Return dt
    End Function

    Private Function InstanciarTableEmisor(ByRef dt As DataTable) As DataTable
        dt = New DataTable
        dt.Columns.Add("CodigoEntidad", GetType(String))
        dt.Columns.Add("TipoValor", GetType(String))
        dt.Columns.Add("Valor", GetType(Decimal))
        dt.Columns.Add("Fecha", GetType(Date))
        Return dt
    End Function

    Private Function CargarEmision(ByVal routeFile As String, ByVal dt As DataTable) As DataTable
        CargarEmision = Nothing
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oSheets As Excel.Sheets
        Dim oCells As Excel.Range
        Dim hojaEnBlanco As Boolean = True
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            oBooks = xlApp.Workbooks
            oBooks.Open(routeFile)
            xlLibro = oBooks.Item(1)
            oSheets = xlLibro.Worksheets
            xlHoja = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = xlHoja.Cells

            Dim fila As Integer = 0
            Dim ultimaFila As Integer = oCells.Columns("A:A").Range("A1500").End(Excel.XlDirection.xlUp).Row()

            Dim codigoNemonico As String = String.Empty
            Dim valor As Decimal = 0
            Dim fecha As Date = Nothing
            For fila = 2 To ultimaFila
                codigoNemonico = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(1), fila)).Value, String) = String.Empty, "", CType(xlHoja.Range(String.Concat(LetraColumna(1), fila)).Value, String))
                valor = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(2), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(2), fila)).Value, Decimal))
                fecha = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(3), fila)).Value, String) = String.Empty, "01/01/0001", CType(xlHoja.Range(String.Concat(LetraColumna(3), fila)).Value, Date))
                'If codigoNemonico = "" Then
                '    Throw New System.Exception("No se encuentra definido el nemónico en la hoja " & xlHoja.Name & " y celda: " & String.Concat(LetraColumna(1), fila))
                'End If
                'If fecha.ToString("yyyyMMdd") = "00010101" Then
                '    Throw New System.Exception("No se encuentra definido la fecha en la hoja " & xlHoja.Name & " y celda: " & String.Concat(LetraColumna(3), fila))
                'End If
                If codigoNemonico <> String.Empty Then
                    dt.Rows.Add(codigoNemonico, valor, fecha)
                End If
            Next
            CargarEmision = dt
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
    End Function

    Private Function CargarEmisor(ByVal routeFile As String, ByVal dt As DataTable) As DataTable
        CargarEmisor = Nothing
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oSheets As Excel.Sheets
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            oBooks = xlApp.Workbooks
            oBooks.Open(routeFile)
            xlLibro = oBooks.Item(1)
            oSheets = xlLibro.Worksheets
            xlHoja = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = xlHoja.Cells

            Dim fila As Integer = 0
            Dim ultimaFila As Integer = oCells.Columns("A:A").Range("A1500").End(Excel.XlDirection.xlUp).Row()

            Dim codigoEntidad As String = String.Empty
            Dim tipoValor As String = String.Empty
            Dim valor As Decimal = 0
            Dim fecha As Date = Nothing
            For fila = 2 To ultimaFila
                codigoEntidad = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(1), fila)).Value, String) = String.Empty, "", CType(xlHoja.Range(String.Concat(LetraColumna(1), fila)).Value, String))
                tipoValor = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(2), fila)).Value, String) = String.Empty, "", CType(xlHoja.Range(String.Concat(LetraColumna(2), fila)).Value, String))
                valor = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(3), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(3), fila)).Value, Decimal))
                fecha = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(4), fila)).Value, String) = String.Empty, "01/01/0001", CType(xlHoja.Range(String.Concat(LetraColumna(4), fila)).Value, Date))
                'If codigoEntidad = "" Then
                '    Throw New System.Exception("No se encuentra definido la entidad en la hoja " & xlHoja.Name & " y celda: " & String.Concat(LetraColumna(1), fila))
                'End If
                'If tipoValor = "" Then
                '    Throw New System.Exception("No se encuentra definido la entidad en la hoja " & xlHoja.Name & " y celda: " & String.Concat(LetraColumna(2), fila))
                'End If
                'If fecha.ToString("yyyyMMdd") = "00010101" Then
                '    Throw New System.Exception("No se encuentra definido la fecha en la hoja " & xlHoja.Name & "y celda: " & String.Concat(LetraColumna(4), fila))
                'End If
                If codigoEntidad <> String.Empty And tipoValor <> String.Empty Then
                    dt.Rows.Add(codigoEntidad, tipoValor, valor, fecha)
                End If
            Next
            CargarEmisor = dt
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

    End Function

    Public Shared Function LetraColumna(ByVal numeroColumna As Integer) As String
        Dim VectorLetra() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}
        Return VectorLetra(numeroColumna)
    End Function

#End Region

End Class
