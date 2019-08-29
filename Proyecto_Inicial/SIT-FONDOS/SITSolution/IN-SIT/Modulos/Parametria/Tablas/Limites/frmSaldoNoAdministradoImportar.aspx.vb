Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports System.Runtime.InteropServices

Partial Class Modulos_Parametria_Tablas_Limites_frmSaldoNoAdministradoImportar
    Inherits BasePage

    Private Const ExtensionValida As String = ".xls,.xlsx,"
    Private RutaDestino As String = String.Empty

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub CargarCombos()
        LlenarMandato()
    End Sub
    Private Sub LlenarMandato()
        Dim dtMandatos As DataTable
        Dim oTercerosBM As New TercerosBM
        dtMandatos = oTercerosBM.ListarMandatos().Terceros
        HelpCombo.LlenarComboBox(Me.ddlMandato, dtMandatos, "CodigoTercero", "Descripcion", True)
    End Sub
    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaSaldoNoAdministrado.aspx")
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

                    'CargarGrillaFiltro()
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", "").Replace(Environment.NewLine, ""))
        End Try
    End Sub

    Private Function IsValidoCamposObligatorios(ByRef msjValidacion As String) As Boolean
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

            If ExtensionValida.Contains(fInfo.Extension & ",") Then
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

        Dim pathExcel As String = hfRutaDestino.Value & System.Guid.NewGuid().ToString().Substring(0, 8) & "_" & iptRuta.Value
        Dim mensajePersonalizado As String = ""
        iptRuta.PostedFile.SaveAs(pathExcel)

        Dim lsSaldosNoAdministrados As List(Of SaldoNoAdministradoBE) = ExcelHaciaDataTable(pathExcel)

        Dim Lista1 As List(Of SaldoNoAdministradoBE)
        Dim Lista2 As List(Of SaldoNoAdministradoBE)
        Dim colMes1 As String, ColAnhio1 As String
        Dim ColMes2 As String, Colanhio2 As String
        Dim nCon As Long

        Lista1 = lsSaldosNoAdministrados
        Lista2 = lsSaldosNoAdministrados


        For Each Ent1 As SaldoNoAdministradoBE In Lista1

            colMes1 = Ent1.Fecha.ToString().Trim().Substring(4, 2)
            ColAnhio1 = Ent1.Fecha.ToString().Trim().Substring(0, 4)
            nCon = 0
            For Each Ent2 As SaldoNoAdministradoBE In Lista2
                ColMes2 = Ent2.Fecha.ToString().Trim().Substring(4, 2)
                Colanhio2 = Ent2.Fecha.ToString().Trim().Substring(0, 4)

                If ((colMes1 <> ColMes2 And ColAnhio1 <> Colanhio2) Or (colMes1 <> ColMes2 And ColAnhio1 = Colanhio2) Or (colMes1 = ColMes2 And ColAnhio1 <> Colanhio2)) Then
                    nCon += 1
                End If
            Next
            If nCon > 1 Then
                AlertaJS("Existen fechas distintas revisar el archivo")
                'oConn.Close()
                Exit Sub
            End If
        Next

        InsertarDeExcel(lsSaldosNoAdministrados, MyBase.DatosRequest(), colMes1, ColAnhio1)

        AlertaJS("Archivo cargado correctamente")
    End Sub

    Function ExcelHaciaDataTable(ByVal pathArchivo As String) As List(Of SaldoNoAdministradoBE)

        Dim oListSaldoNoAdministradoBE As New List(Of SaldoNoAdministradoBE)
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application = Nothing

        Try
            Const _MAX_FILAS_EXCEL As Integer = 5000

            Const _FILA_INICIAL_LECTURA As Integer = 2

            Dim ColExcel_Tercero As Integer = 1 'Ubicación en el EXCEL
            Dim ColExcel_Fecha As Integer = 2 'Ubicación en el EXCEL
            Dim ColExcel_TerceroFinanciero As Integer = 3 'Ubicación en el EXCEL
            Dim ColExcel_TipoCuenta As Integer = 4 'Ubicación en el EXCEL
            Dim ColExcel_Saldo As Integer = 5 'Ubicación en el EXCEL
            Dim ColExcel_Moneda As Integer = 6 'Ubicación en el EXCEL

            Dim hojaLectura As Integer



            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = ObjCom.ObjetoAplication

            Dim oBooks As Excel.Workbooks = xlApp.Workbooks
            oBooks.Open(pathArchivo, [ReadOnly]:=True, IgnoreReadOnlyRecommended:=True)

            Dim xlLibro As Excel.Workbook = oBooks.Item(1)
            Dim oSheets As Excel.Sheets = xlLibro.Worksheets

            'ColExcel_Rating = 5
            hojaLectura = 1

            Dim hojaConfig As Excel.Worksheet = oSheets.Item(hojaLectura)
            Dim filaActual As Integer = 0

            While filaActual < _MAX_FILAS_EXCEL
                Dim strTercero(2) As String, strTecerFinanciero(2) As String
                'Dim row As RatingBE.RegistroRatingRow = oRatingBE.RegistroRating.NewRegistroRatingRow()
                Dim oSaldoNoAdministradoBE As New SaldoNoAdministradoBE

                strTercero = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Tercero).Value(), "").ToString().Split("-")
                oSaldoNoAdministradoBE.CodigoTercero = strTercero(0).ToString().Trim()
                'oSaldoNoAdministradoBE.CodigoTercero = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Tercero).Value(), "")
                oSaldoNoAdministradoBE.Fecha = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Fecha).Value(), 0)

                strTecerFinanciero = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_TerceroFinanciero).Value(), "").ToString().Split("-")
                oSaldoNoAdministradoBE.CodigoTerceroFinanciero = strTecerFinanciero(0).ToString().Trim()
                'oSaldoNoAdministradoBE.CodigoTerceroFinanciero = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_TerceroFinanciero).Value(), "")

                oSaldoNoAdministradoBE.TipoCuenta = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_TipoCuenta).Value(), "")
                oSaldoNoAdministradoBE.TipoCuenta = ifNull(ObtenerTipoCuenta(Convert.ToString(oSaldoNoAdministradoBE.TipoCuenta.ToString())), "")

                oSaldoNoAdministradoBE.Saldo = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Saldo).Value(), 0)
                oSaldoNoAdministradoBE.CodigoMoneda = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Moneda).Value(), "")
                oSaldoNoAdministradoBE.CodigoMoneda = ifNull(ObtenerCodigoMonedaxSinonimo(Convert.ToString(oSaldoNoAdministradoBE.CodigoMoneda.ToString())), "")


                If Not oSaldoNoAdministradoBE.CodigoTercero.Equals("") And Not oSaldoNoAdministradoBE.Fecha.Equals(0) And Not oSaldoNoAdministradoBE.CodigoTerceroFinanciero.Equals("") And Not oSaldoNoAdministradoBE.TipoCuenta.Equals("") And Not oSaldoNoAdministradoBE.CodigoMoneda.Equals("") Then
                    oSaldoNoAdministradoBE.Fecha = CStr(UIUtility.ConvertirFechaaDecimal(oSaldoNoAdministradoBE.Fecha))

                    'Dim ColA1(2) As String
                    Dim Mandato As String
                    Mandato = ddlMandato.SelectedValue
                    'ColA1 = oSaldoNoAdministradoBE.CodigoTercero.Split("-")

                    If (oSaldoNoAdministradoBE.CodigoTercero = Mandato) Then
                        oListSaldoNoAdministradoBE.Add(oSaldoNoAdministradoBE)
                    End If

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

        Return oListSaldoNoAdministradoBE
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

    Private Function ObtenerTipoCuenta(ByVal TipoCuenta As String) As String
        Dim strValor As String
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Try

            Dim tb As New DataTable
            tb = oParametrosGeneralesBM.SeleccionarPorFiltro("TipoCuenta", TipoCuenta, String.Empty, String.Empty, DatosRequest)

            strValor = tb(0)("Valor")


        Catch ex As Exception

            strValor = ""
            Return ex.Message

        End Try
        Return strValor

    End Function


    Private Function ObtenerCodigoMonedaxSinonimo(ByVal SinonimoMoneda As String) As String
        Dim strValor As String
        Dim oMonedaBM As New MonedaBM
        Try

            Dim tb As New DataTable
            tb = oMonedaBM.ObtenerCodigoMonedaxSinonimo(SinonimoMoneda)

            strValor = tb(0)("CodigoMoneda")


        Catch ex As Exception

            strValor = ""
            Return ex.Message

        End Try
        Return strValor

    End Function

    Public Sub InsertarDeExcel(ByVal ListadoSaldoNoAdministado As List(Of SaldoNoAdministradoBE), ByVal DatosRequest As DataSet, ByVal strMes As String, ByVal strAnhio As String)

        Dim oSaldoNoAdministradoBM As New SaldoNoAdministradoBM
        Dim strMensaje As String = ""

        oSaldoNoAdministradoBM.DesactivarRegistrosExcel(DatosRequest, strMensaje, strMes, strAnhio)

        If (String.IsNullOrEmpty(strMensaje)) Then

            For Each row As SaldoNoAdministradoBE In ListadoSaldoNoAdministado
                'row.Fecha = CStr(UIUtility.ConvertirFechaaDecimal(row.Fecha))
                oSaldoNoAdministradoBM.InsertarRegistrosExcel(row, MyBase.DatosRequest())
            Next

        End If



    End Sub
End Class
