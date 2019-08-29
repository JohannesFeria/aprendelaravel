Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports System.Runtime.InteropServices

Partial Class Modulos_Parametria_Tablas_Limites_frmCarteraIndirectaImportar
    Inherits BasePage

    Private Const ExtensionValida As String = ".xls,.xlsx,"
    Private RutaDestino As String = String.Empty


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaCarteraIndirecta.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click
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

        Dim lsCarteraIndirecta As List(Of CarteraIndirectaBE) = ExcelHaciaDataTable(pathExcel, fechaProceso)

        Dim Lista1 As List(Of CarteraIndirectaBE)
        Dim Lista2 As List(Of CarteraIndirectaBE)
        Dim ColA1 As String, ColA2 As String ', ColA3 As String
        Dim ColB1 As String, ColB2 As String ', ColB3 As String
        Dim nCon As Long

        Lista1 = lsCarteraIndirecta
        Lista2 = lsCarteraIndirecta
        'Dim dt1 As DataTable, dt2 As DataTable
        'Dim sCod1 As String, sCod2 As String, nCon As Long
        'dt1 = oDs.Tables(0)
        'dt2 = oDs.Tables(0)
        For Each Ent1 As CarteraIndirectaBE In Lista1

            ColA1 = Ent1.CodigoPortafolio.ToString().Trim()
            ColA2 = Ent1.CodigoEntidad.ToString().Trim()
            nCon = 0
            For Each Ent2 As CarteraIndirectaBE In Lista2
                ColB1 = Ent2.CodigoPortafolio.ToString().Trim()
                ColB2 = Ent2.CodigoEntidad.ToString().Trim()
                If (ColA1 = ColB1 And ColA2 = ColB2) Then
                    nCon += 1
                End If
            Next
            If nCon > 1 Then
                AlertaJS("No se ha podido cargar el archivo. Existe duplicidad de la combinacion Fondo + Codigo Entidad.")
                'oConn.Close()
                Exit Sub
            End If
        Next

        InsertarDeExcel(fechaProceso, lsCarteraIndirecta, MyBase.DatosRequest())

    End Sub

    Function ExcelHaciaDataTable(ByVal pathArchivo As String, ByVal FechaProceso As Decimal) As List(Of CarteraIndirectaBE)

        Dim oListCarteraIndirectaBE As New List(Of CarteraIndirectaBE)
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application = Nothing

        Try
            Const _MAX_FILAS_EXCEL As Integer = 5000

            Const _FILA_INICIAL_LECTURA As Integer = 2

            Dim ColExcel_Fondo As Integer = 1 'Ubicación en el EXCEL
            Dim ColExcel_GrupoEconomico As Integer = 2 'Ubicación en el EXCEL
            Dim ColExcel_Emisor As Integer = 3 'Ubicación en el EXCEL
            Dim ColExcel_CodigoEntidad As Integer = 4 'Ubicación en el EXCEL
            Dim ColExcel_CodigoActividadEconomica As Integer = 5 'Ubicación en el EXCEL
            Dim ColExcel_CodigoPais As Integer = 6 'Ubicación en el EXCEL
            Dim ColExcel_Rating As Integer = 7 'Ubicación en el EXCEL
            Dim ColExcel_Posicion As Integer = 8 'Ubicación en el EXCEL
            Dim ColExcel_Patrimonio As Integer = 9 'Ubicación en el EXCEL
            Dim ColExcel_Participacion As Integer = 10 'Ubicación en el EXCEL
            'Dim ColExcel_Fecha As Integer = 11 'Ubicación en el EXCEL
            'Dim ColExcel_Situacion As Integer = 11 'Ubicación en el EXCEL

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
                Dim oCarteraIndirectaBE As New CarteraIndirectaBE

                oCarteraIndirectaBE.CodigoPortafolio = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Fondo).Value(), "")
                oCarteraIndirectaBE.CodigoGrupoEconomico = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_GrupoEconomico).Value(), "")
                oCarteraIndirectaBE.CodigoEntidad = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoEntidad).Value(), "")
                oCarteraIndirectaBE.CodigoActividadEconomica = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoActividadEconomica).Value(), "")
                oCarteraIndirectaBE.CodigoPais = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoPais).Value(), "")
                oCarteraIndirectaBE.Rating = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Rating).Value(), "")
                oCarteraIndirectaBE.Posicion = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Posicion).Value(), 0)
                oCarteraIndirectaBE.Patrimonio = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Patrimonio).Value(), 0)
                oCarteraIndirectaBE.Participacion = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Participacion).Value(), 0)
                'oCarteraIndirectaBE.Fecha = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Fecha).Value(), 0)
                oCarteraIndirectaBE.Fecha = FechaProceso
                'oCarteraIndirectaBE.Situacion = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Situacion).Value(), 0)

                oCarteraIndirectaBE.Rating = ifNull(ObtenerCodigoRatingxNombre(Convert.ToString(oCarteraIndirectaBE.Rating.ToString())), "")


                If Not oCarteraIndirectaBE.Fecha.Equals(0) And Not oCarteraIndirectaBE.CodigoPortafolio.Equals("") And Not oCarteraIndirectaBE.CodigoEntidad.Equals("") Then
                    oListCarteraIndirectaBE.Add(oCarteraIndirectaBE)
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

        Return oListCarteraIndirectaBE
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

    Public Sub InsertarDeExcel(ByVal fechaProceso As Decimal, ByVal ListadoCarteraIndirecta As List(Of CarteraIndirectaBE), ByVal DatosRequest As DataSet)

        Dim oCarteraIndirectaBM As New CarteraIndirectaBM
        Dim strMensaje As String = ""

        oCarteraIndirectaBM.DesactivarRegistrosExcel(fechaProceso, DatosRequest, strMensaje)

        If (String.IsNullOrEmpty(strMensaje)) Then

            For Each row As CarteraIndirectaBE In ListadoCarteraIndirecta
                'row.Fecha = fechaProceso
                oCarteraIndirectaBM.InsertarRegistrosExcel(row, MyBase.DatosRequest())
            Next

        End If



    End Sub

    Private Function ObtenerCodigoRatingxNombre(ByVal Rating As String) As String
        Dim strValor As String
        Dim oRatingBM As New RatingBM
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
