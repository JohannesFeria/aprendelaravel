Imports Sit.BusinessLayer
Imports Sit.BusinessEntities

Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports System.Runtime.InteropServices

Partial Class Modulos_ValorizacionCustodia_Custodia_frmRegistroDivRebLibImportar
    Inherits BasePage

    Private Const ExtensionValida As String = ".xls,.xlsx,"
    Private RutaDestino As String = String.Empty
    Dim oValoresBM As New ValoresBM
    Dim oDividendosRebatesLiberadasBM As New DividendosRebatesLiberadasBM
    Dim objPortafolio As New PortafolioBM


    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click
        Dim msjValidacion As String = ""
        Try
            dgLista.DataSource = Nothing
            dgLista.DataBind()

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
                    'AlertaJS("Archivo cargado correctamente")
                    'CargarGrillaFiltro()
                End If
            End If
        Catch ex As Exception
            dgLista.DataSource = ""
            dgLista.DataBind()
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
        Dim lsDividendosRebatesLiberadasBEFinal As New DividendosRebatesLiberadasBE
        Dim pathExcel As String = hfRutaDestino.Value & System.Guid.NewGuid().ToString().Substring(0, 8) & "_" & iptRuta.Value
        Dim mensajePersonalizado As String = ""
        Dim Lista1 As DividendosRebatesLiberadasBE

        Try
            iptRuta.PostedFile.SaveAs(pathExcel)

            Dim lsDividendosRebatesLiberadasBE As DividendosRebatesLiberadasBE = ExcelHaciaDataTable(pathExcel)

            Lista1 = lsDividendosRebatesLiberadasBE

            For Each Ent1 As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow In Lista1.DividendosRebatesLiberadas
                Dim strMensaje As String = "", strEstado As String = "Cargado"
                If (ValidarFecha(Ent1, strMensaje)) Then
                    If (ValidarUnidades(Ent1, strMensaje)) Then
                        If (validarLiberadas(Ent1, strMensaje)) Then
                            Ent1.Situacion = "Cargado"
                            Ent1.Mensaje = strMensaje
                        Else
                            Ent1.Situacion = "Error"
                            Ent1.Mensaje = strMensaje
                        End If
                    Else
                        Ent1.Situacion = "Error"
                        Ent1.Mensaje = strMensaje
                    End If
                Else
                    Ent1.Situacion = "Error"
                    Ent1.Mensaje = strMensaje
                End If

            Next

            InsertarDeExcel(Lista1, MyBase.DatosRequest())

            dgLista.DataSource = Lista1.DividendosRebatesLiberadas.DataSet.Tables(0)
            dgLista.DataBind()

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Function ExcelHaciaDataTable(ByVal pathArchivo As String) As DividendosRebatesLiberadasBE

        Dim oDividendosRebatesLiberadasBE As New DividendosRebatesLiberadasBE
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application = Nothing

        Try
            Const _MAX_FILAS_EXCEL As Integer = 5000

            Const _FILA_INICIAL_LECTURA As Integer = 2

            Dim ColExcel_TipoDistribucion As Integer = 1 'Ubicación en el EXCEL
            Dim ColExcel_CodigoPortafolio As Integer = 2 'Ubicación en el EXCEL
            Dim ColExcel_CodigoNemonico As Integer = 3 'Ubicación en el EXCEL
            Dim ColExcel_CodigoISIN As Integer = 4 'Ubicación en el EXCEL
            Dim ColExcel_CodigoMoneda As Integer = 5 'Ubicación en el EXCEL
            Dim ColExcel_Unidades As Integer = 6 'Ubicación en el EXCEL
            Dim ColExcel_Factor As Integer = 7 'Ubicación en el EXCEL
            Dim ColExcel_FechaCorte As Integer = 8 'Ubicación en el EXCEL
            Dim ColExcel_FechaOperacion As Integer = 9 'Ubicación en el EXCEL
            Dim ColExcel_FechaEntrega As Integer = 10 'Ubicación en el EXCEL


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
            Dim filaActual As Integer = 0, ContadorFila As Integer = 2

            While filaActual < _MAX_FILAS_EXCEL

                Dim strEstado As String = "", strMensaje As String = ""

                Dim row As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow = oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.NewDividendosRebatesLiberadasRow()

                row.TipoDistribucion = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_TipoDistribucion).Value(), "")
                'row.CodigoPortafolioSBS = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoPortafolio).Value(), "")
                row.DescripcionPortafolio = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoPortafolio).Value(), "")
                row.CodigoNemonico = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoNemonico).Value(), "")
                row.CodigoISIN = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoISIN).Value(), "")
                row.CodigoMoneda = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_CodigoMoneda).Value(), "")
                row.Unidades = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Unidades).Value(), 0)
                row.Factor = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_Factor).Value(), 0)
                row.FechaCorte = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_FechaCorte).Value(), 0)
                row.FechaIDI = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_FechaOperacion).Value(), 0)
                row.FechaEntrega = ifNull(hojaConfig.Cells(_FILA_INICIAL_LECTURA + filaActual, ColExcel_FechaEntrega).Value(), 0)
                row.Identificador = ContadorFila
                'Obtener SBS
                If (row.CodigoISIN <> "" And row.CodigoNemonico <> "") Then
                    Dim oValoresBE As DataSet = oValoresBM.SeleccionarInstrumento(row.CodigoISIN, row.CodigoNemonico, DatosRequest)
                    If oValoresBE.Tables(0).Rows.Count > 0 Then
                        row.CodigoSBS = oValoresBE.Tables(0).Rows(0).ItemArray(2)
                    Else
                        row.CodigoSBS = ""
                    End If
                Else
                    row.CodigoSBS = ""
                End If
               
                'Fin

                'Obtener Portafolio
                If (row.DescripcionPortafolio <> "") Then
                    row.CodigoPortafolioSBS = objPortafolio.PortafolioPorDescripcion(row.DescripcionPortafolio)
                End If
                'Fin


                If (Validacion(row, strMensaje)) Then
                    row.Situacion = "Cargado"
                    row.Mensaje = strMensaje
                Else
                    row.Situacion = "Error"
                    row.Mensaje = strMensaje
                End If

                If (Not row.TipoDistribucion.Equals("") And Not row.DescripcionPortafolio.Equals("") And Not row.CodigoNemonico.Equals("") _
                   And Not row.CodigoISIN.Equals("") And Not row.CodigoMoneda.Equals("") And Not row.Unidades <= 0 And Not row.Factor <= 0 _
                   And Not row.FechaCorte = 0 And Not row.FechaIDI = 0 And Not row.FechaEntrega = 0) Then
                    oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.AddDividendosRebatesLiberadasRow(row)
                Else
                    Exit While
                End If

                ContadorFila += 1
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

        Return oDividendosRebatesLiberadasBE
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

    Public Function Validacion(ByVal row As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow, ByRef strMensajeError As String) As Boolean

        Dim blnResultado As Boolean = True

        If (row.CodigoISIN = "") Then
            strMensajeError = "El código ISIN no es valido o no existe."
        ElseIf (row.CodigoNemonico = "") Then
            strMensajeError = "El código Mnemónico no es valido o no existe."
        ElseIf (row.CodigoSBS = "") Then
            strMensajeError = "El código SBS no es valido."
        ElseIf (row.CodigoPortafolioSBS = "") Then
            strMensajeError = "El portafolio no es valido."
        ElseIf (row.CodigoMoneda = "") Then
            strMensajeError = "La moneda no es valida."
        ElseIf (row.FechaCorte = 0) Then
            strMensajeError = "La fecha corte no es valida."
        ElseIf (row.FechaIDI = 0) Then
            strMensajeError = "La fecha IDI no es valida."
        ElseIf (row.FechaEntrega = 0) Then
            strMensajeError = "La fecha entrega no es valida."
        ElseIf (row.FechaEntrega < row.FechaCorte) Then
            strMensajeError = "La Fecha de Entrega no puede ser menor a la Fecha de Corte."
        ElseIf (row.FechaIDI < row.FechaCorte) Then
            strMensajeError = "La Fecha IDI no puede ser menor a la Fecha de Corte."
        ElseIf (row.Factor = 0) Then
            strMensajeError = "Debe ingresar el Factor correspondiente."
        ElseIf (row.TipoDistribucion = "") Then
            strMensajeError = "Debe ingresar el Tipo de distribución."
        End If

        If (String.IsNullOrEmpty(strMensajeError)) Then
            blnResultado = True
            strMensajeError = ""
        Else
            blnResultado = False
        End If

        Return blnResultado
    End Function

    Public Function ValidarFecha(ByVal row As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow, ByRef strmensajeError As String) As Boolean
        Dim blnResultado As Boolean = True
        Dim str As String = "", strMercado As String = "0"
        Dim dsFechas As PortafolioBE, objPortafolio As New PortafolioBM, objferiadoBM As New FeriadoBM, oValoresMonedaBE As ValoresBE
        Dim drFechas As DataRow
        strmensajeError = ""
        dsFechas = objPortafolio.Seleccionar(row.CodigoPortafolioSBS.ToString(), DatosRequest)
        oValoresMonedaBE = New ValoresBM().Seleccionar(row.CodigoNemonico, DatosRequest)
        If (oValoresMonedaBE.Valor.Rows.Count > 0) Then
            strMercado = oValoresMonedaBE.Valor.Rows(0)("CodigoMercado")
        End If

        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaValoracion As Decimal = CType(drFechas("FechaValoracion"), Decimal)
            If (dblFechaConstitucion <> row.FechaIDI) Then
                blnResultado = False
                strmensajeError = "La fecha de operacion no debe ser diferente a la fecha de proceso del portafolio"
            ElseIf (dblFechaValoracion >= row.FechaEntrega) Then
                blnResultado = False
                strmensajeError = "Este portafolio ya se encuentra valorizado para esta fecha."
            End If
        End If
        If (objferiadoBM.Feriado_ValidarFecha(UIUtility.ConvertirFechaaDecimal(row.FechaIDI), strMercado)) = True Then
            blnResultado = False
            strmensajeError = "La fecha de operación no puede ser día feriado."
        End If
        Return blnResultado
    End Function

    Public Function ValidarUnidades(ByVal row As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow, ByRef strMensajeError As String) As Boolean

        Dim dcSaldo As Decimal = 0
        Dim blnResultado As Boolean = True

        dcSaldo = oDividendosRebatesLiberadasBM.ObtenerSaldo_NemonicoPortafolioFecha(row.CodigoNemonico, row.FechaCorte, row.CodigoPortafolioSBS)
        If (dcSaldo <> row.Unidades) Then
            strMensajeError = "El saldo cargado es diferente al saldo de cartera."
            blnResultado = False
        Else
            strMensajeError = ""
            blnResultado = True
        End If

        Return blnResultado
    End Function

    Public Function validarLiberadas(ByVal Ent1 As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow, ByRef strmensajeError As String) As Boolean
        Dim blnResultado = True
        If (Ent1.TipoDistribucion = "L") Then
            Ent1.FechaIDI = Ent1.FechaEntrega
            If oDividendosRebatesLiberadasBM.NemonicoFechaidi_Contar(Ent1.CodigoNemonico.Trim(), Ent1.FechaIDI.ToString()) <> "0" Then
                If Convert.ToDouble(oDividendosRebatesLiberadasBM.NemonicoFechaidi_Factor(Ent1.CodigoNemonico.Trim(), Ent1.FechaIDI.ToString())) <> Ent1.Factor Then
                    If oDividendosRebatesLiberadasBM.NemonicoFechaidi_VerificaActual(Ent1.CodigoNemonico.Trim(), Ent1.FechaIDI.ToString()) = "1" Then
                        If oDividendosRebatesLiberadasBM.NemonicoFechaidi_NumeroUnidades(Ent1.CodigoNemonico.Trim(), Ent1.FechaIDI.ToString()) <> "" Then
                            blnResultado = True
                            strmensajeError = ""
                        Else
                            strmensajeError = "Falta información para recalcular el (Número de Unidades) en Valores."
                            blnResultado = False
                        End If
                    Else
                        strmensajeError = "No se puede recalcular el (Número de Unidades) en Valores."
                        blnResultado = False
                    End If
                End If
            End If
        End If
        Return blnResultado
    End Function


    Public Sub InsertarDeExcel(ByVal Ob As DividendosRebatesLiberadasBE, ByVal DatosRequest As DataSet)

        'Dim oDividendosRebatesLiberadasBM As New DividendosRebatesLiberadasBM
        Dim strMensaje As String = ""

        'oDividendosRebatesLiberadasBM.DesactivarRegistrosExcel(DatosRequest, strMensaje)
        Dim CountA As Integer = 0, CountE As Integer = 0
        Try
            'If (String.IsNullOrEmpty(strMensaje)) Then
            For Each row As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow In Ob.DividendosRebatesLiberadas
                Dim dcmIndicador = 0
                Dim dcmGrupoIndicador = 0
                '  Dim objBE As new DividendosRebatesLiberadasBE
                Dim objent As New DividendosRebatesLiberadasBE()

                Dim ObjBE As New DividendosRebatesLiberadasBE()
                ObjBE = CrearObjetoDividendosLiberadasRebates(row)

                If (row.Situacion = "Cargado") Then
                    oDividendosRebatesLiberadasBM.Insertar(ObjBE, dcmIndicador, dcmGrupoIndicador, MyBase.DatosRequest())
                    CountA = CountA + 1
                Else
                    CountE = CountE + 1
                End If
            Next
            'End If

            AlertaJS("Registrados Cargados " + CountA.ToString() + ", registros no cargardos " + CountE.ToString() + ".")
        Catch ex As Exception
            Throw ex
        End Try
        
    End Sub

    Private Function CrearObjetoDividendosLiberadasRebates(ByVal row As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow) As DividendosRebatesLiberadasBE
        Dim oDividendosRebatesLiberadasBE As New DividendosRebatesLiberadasBE
        Dim oRow As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow
        oRow = CType(oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.NewRow(), DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow)
        oRow.CodigoSBS() = row.CodigoSBS
        'oRow.Identificador() = row.Identificador
        oRow.CodigoISIN() = row.CodigoISIN
        oRow.CodigoNemonico() = row.CodigoNemonico
        'oRow.CodigoISIN() = row.CodigoISIN
        oRow.FechaIDI() = row.FechaIDI
        oRow.Factor() = row.Factor
        oRow.FechaCorte() = row.FechaCorte
        oRow.FechaEntrega() = row.FechaEntrega
        oRow.TipoDistribucion() = row.TipoDistribucion
        oRow.CodigoMoneda() = row.CodigoMoneda
        oRow.CodigoPortafolioSBS() = row.CodigoPortafolioSBS
        oRow.Mensaje() = row.Mensaje
        oRow.Situacion() = row.Situacion
        oRow.Unidades() = row.Unidades
        oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.AddDividendosRebatesLiberadasRow(oRow)
        oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.AcceptChanges()
        Return oDividendosRebatesLiberadasBE
    End Function
End Class
