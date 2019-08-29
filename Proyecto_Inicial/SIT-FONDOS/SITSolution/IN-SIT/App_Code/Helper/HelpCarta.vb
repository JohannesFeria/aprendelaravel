Imports Microsoft.VisualBasic
Imports System.Data
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Diagnostics
Imports iTextSharp.text.pdf
Public Class HelpCarta
#Region "Cartas"
    Public Shared Sub GenerarCartas(ByRef dsOperacionesCaja As DataSet, ByVal request As DataSet)
        Dim _strcartas As String
        _strcartas = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(request)
        For Each oper As DataRow In dsOperacionesCaja.Tables(0).Copy().Rows
            Dim op As DataRow = dsOperacionesCaja.Tables(0).Select("NumeroCarta='" & oper("NumeroCarta") & "'")(0)
            If oper("NumeroCarta") = "" Then
                dsOperacionesCaja.Tables(0).Rows.Remove(op)
            Else
                If Not CrearCarta(oper, request) Then
                    dsOperacionesCaja.Tables(0).Rows.Remove(op)
                End If
            End If
        Next
    End Sub
    Private Shared Function CrearCarta(ByVal row As DataRow, ByVal request As DataSet) As Boolean
        If row("ArchivoPlantilla") = "" Then Return False
        If Not File.Exists(row("ArchivoPlantilla")) Then Return False
        Dim _strcartas As String
        _strcartas = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(request)
        Dim appWord As Word.Application
        Dim aWordDocument As Word.Document
        Dim mensaje As String = ""
        Dim elog As New System.Diagnostics.EventLog
        elog.Log = "Application"
        elog.Source = "CartasSIT"
        Try
            mensaje = "antes de instanciar WORD"
            appWord = New Word.Application
            appWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone
            mensaje = mensaje & " ++ WORD instanciado"
            Dim variable As Boolean = False, nombreVar As String = "", remplazo As String
            aWordDocument = appWord.Documents.Open(Convert.ToString(row("ArchivoPlantilla")))
            If File.Exists(_strcartas & "\" & row("NumeroCarta") & ".doc") Then
                File.Delete(_strcartas & "\" & row("NumeroCarta") & ".doc")
            End If
            aWordDocument.SaveAs(_strcartas & "\" & Convert.ToString(row("NumeroCarta")) & ".doc")
            mensaje = mensaje & " ++ Agrego plantilla"
            ReemplazarCadena("[Fecha de Operación]", UIUtility.ConvertirFechaaString(row("FechaOperacion")), appWord)
            mensaje = mensaje & " ++ Reemplazo cadena"
            Dim dsCartaEstructura As DataSet = New ModeloCartaBM().SeleccionarCartaEstructuraPorModelo(row("CodigoModelo"))
            mensaje = mensaje & " ++ Consulta OK"
            For Each etiqueta As DataRow In dsCartaEstructura.Tables(0).Rows
                If (row.Table.Columns.Contains(etiqueta("OrigenCampo"))) Then
                    If IsDBNull(row(Trim(etiqueta("OrigenCampo")))) Then
                        remplazo = ""
                    Else
                        remplazo = row(Trim(etiqueta("OrigenCampo")))
                    End If
                    ReemplazarCadena(etiqueta("NombreCampo"), remplazo, appWord)
                Else
                    Console.WriteLine("No existe campo")
                End If
            Next
            mensaje = mensaje & " ++ Elimino carta existente"
            aWordDocument.Save()
            mensaje = mensaje & " ++ Guardo OK"
            aWordDocument.Close()
            appWord.Quit()
            mensaje = mensaje & " ++ Fin Word"
            Return True
        Catch ex As Exception
            elog.WriteEntry("ERROR2: " & mensaje & " ++ messageError = " & ex.Message & " ++ Stack = " & ex.StackTrace)
            elog.Close()
            Throw ex
        End Try
        Return False
    End Function
    Public Shared Function CrearCartaPDF(ByVal drValores As DataRow, ByVal request As DataSet, Optional ByVal path As String = "") As Boolean
        Dim _strcartas As String
        _strcartas = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(request)
        If drValores("ArchivoPlantilla") = "" Then Return False
        Try
            Dim strRutaHTML = drValores("ArchivoPlantilla").Replace("dot", "html")
            Dim strRutaPDF As String
            If Not String.IsNullOrEmpty(path) Then
                strRutaPDF = path
            Else
                strRutaPDF = _strcartas & "\" & drValores("NumeroCarta") & ".pdf"
            End If
            If Not File.Exists(strRutaHTML) Then Return False
            If File.Exists(strRutaPDF) Then File.Delete(strRutaPDF)
            UIUtility.CrearArchivoPDF(CrearTablaValores(drValores), strRutaHTML, strRutaPDF)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function
    Public Shared Function CrearMultiCarta(ByVal cartas As String) As String
        Dim path As String
        Dim nombreNuevoArchivo As String = ""
        Dim dir As String = ""
        Dim appWord As Word.Application
        Dim aWordDocument As Word.Document
        Try
            appWord = New Word.Application
            appWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone
            Dim variable As Boolean = False
            Dim nombreVar As String = ""
            aWordDocument = appWord.Documents.Add(, , , )
            Dim arrCartas As String() = cartas.Split(New Char() {"&"})
            If arrCartas.Length > 0 Then
                dir = System.IO.Path.GetDirectoryName(arrCartas(0))
            End If
            nombreNuevoArchivo = dir & "\" & System.Guid.NewGuid().ToString() & ".doc"
            aWordDocument.SaveAs(nombreNuevoArchivo)
            For Each savedDoc As String In arrCartas
                path = savedDoc
                If File.Exists(path) Then
                    appWord.Selection.InsertFile(path, , , )
                    appWord.Selection.InsertBreak(Word.WdBreakType.wdPageBreak)
                End If
            Next
            aWordDocument.Save()
            aWordDocument.Close()
            appWord.Quit()
            Return nombreNuevoArchivo
        Catch ex As Exception
            If File.Exists(nombreNuevoArchivo) Then
                File.Delete(nombreNuevoArchivo)
            End If
            Throw ex
            Return String.Empty
        End Try
        Return nombreNuevoArchivo
    End Function
    Public Shared Function CrearMultiCartaPDF(ByVal strCartas As String) As String
        Dim strDirecctorio As String = ""
        Dim strNombreArchivo As String
        Dim arrCartas As String()
        Dim oPdfReader As PdfReader
        Dim oPdfWriter As PdfWriter
        Dim oDocument As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
        Dim oPdfImportedPage As PdfImportedPage
        Try
            arrCartas = strCartas.Split(New Char() {"&"})
            If arrCartas.Length > 0 Then strDirecctorio = System.IO.Path.GetDirectoryName(arrCartas(0))
            strNombreArchivo = strDirecctorio & "\" & System.Guid.NewGuid().ToString() & ".pdf"
            oPdfWriter = PdfWriter.GetInstance(oDocument, New FileStream(strNombreArchivo, FileMode.Create))
            oDocument.Open()
            For Each strCartaPDF As String In arrCartas
                If File.Exists(strCartaPDF) Then
                    oPdfReader = New PdfReader(strCartaPDF)
                    For x As Integer = 1 To oPdfReader.NumberOfPages
                        oDocument.NewPage()
                        oPdfImportedPage = oPdfWriter.GetImportedPage(oPdfReader, x)

                        If oPdfReader.GetPageRotation(x) = 90 Or oPdfReader.GetPageRotation(x) = 270 Then
                            oPdfWriter.DirectContent.AddTemplate(oPdfImportedPage, 0, -1.0F, 1.0F, 0, 0, oPdfReader.GetPageSizeWithRotation(x).Height)
                        Else
                            oPdfWriter.DirectContent.AddTemplate(oPdfImportedPage, 1.0F, 0, 0, 1.0F, 0, 0)
                        End If
                    Next
                    oPdfReader.Close()
                End If
            Next
            oDocument.Close()
            oPdfWriter.Close()
            Return strNombreArchivo
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Funciones"
    Private Shared Sub ReemplazarCadena(ByVal texto As String, ByVal reemplazo As String, ByRef appWord As Word.Application)
        appWord.Selection.Find.ClearFormatting()
        appWord.Selection.Find.Replacement.ClearFormatting()
        appWord.Selection.MoveStart()
        With appWord.Selection.Find
            .Text = texto
            .Replacement.Text = reemplazo
            .Forward = True
            .Wrap = Word.WdFindWrap.wdFindContinue
            .Format = False
            .MatchCase = False
            .MatchWholeWord = False
            .MatchWildcards = False
            .MatchSoundsLike = False
            .MatchAllWordForms = False
        End With
        appWord.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)
    End Sub
    Private Shared Function CrearTablaValores(ByRef drDocuemnto As DataRow) As DataTable
        Dim dtValores As New DataTable
        Dim drValor As DataRow
        dtValores.Columns.Add("Find", GetType(String))
        dtValores.Columns.Add("New", GetType(String))
        Dim dsEstructuraCarta As DataTable = New ModeloCartaBM().SeleccionarCartaEstructuraPorModelo(drDocuemnto("CodigoModelo")).Tables(0)
        For Each drEtiqueta As DataRow In dsEstructuraCarta.Rows
            drValor = dtValores.NewRow
            If (drDocuemnto.Table.Columns.Contains(drEtiqueta("OrigenCampo"))) Then
                If Not IsDBNull(drDocuemnto(Trim(drEtiqueta("OrigenCampo")))) Then
                    drValor("Find") = drEtiqueta("NombreCampo")
                    drValor("New") = drDocuemnto(drEtiqueta("OrigenCampo"))
                    dtValores.Rows.Add(drValor)
                End If
            Else
                If (Convert.ToString(drEtiqueta("OrigenCampo")).Trim().Equals("Logo")) Then
                    drValor("Find") = drEtiqueta("NombreCampo")
                    drValor("New") = ""
                    dtValores.Rows.Add(drValor)
                End If
            End If
        Next
        Return dtValores
    End Function
#End Region
End Class