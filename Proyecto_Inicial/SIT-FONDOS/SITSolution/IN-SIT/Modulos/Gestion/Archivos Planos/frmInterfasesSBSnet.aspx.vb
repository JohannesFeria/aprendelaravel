Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb
Imports Microsoft.Office
Partial Class Modulos_Gestion_Archivos_Planos_frmInterfasesSBSnet
    Inherits BasePage

    Private dtArchivosPip As DataTable
    Private dtImportarSbs As DataTable = New ParametrosGeneralesBM().Listar("ImporSBS", Nothing)
    Private tCaja As TextBox
    Private ddlCombo As DropDownList
    Private hdOculto As HiddenField
    Private fpCargar As FileUpload
    Private fecha As String = String.Empty, tipoIf As String = String.Empty, tipoExtension As String = String.Empty, archivo As String = String.Empty, rutaDestino As String = String.Empty, msg As String = String.Empty, fechaInterface As String, nombreInterface As String
    Private blob() As Byte
    Private msgGral As StringBuilder
    Private oArchivoPlanoBM As New ArchivoPlanoBM
    Private oArchivoPlanoBE As New DataSet
    Private dtsbs As New DataTable
    Private fechaD As Decimal = 0
    Private oVectorTipoCambioBM As VectorTipoCambioBM
    Private oVectorTipoCambioBE As New VectorTipoCambio
    Private oVectorPrecioSBSBM As New VectorPrecioSBSBM
    Private oVectorPrecioBM As New VectorPrecioBM
    Private OVectorPiPBE As New VectorPrecioPIP
    Private oVectorPrecioBE As New VectorPrecioBE
    Private oVectorPrecioSBSBE As New VectorPrecioSBSBE
    Private oVectorFordWardBE As New VectorForwardSBSBE
    Private oVectorFordWardBM As New VectorForwardSBSBM
    Private oIndicadorBM As New IndicadorBM
    Private listaErrores As New Hashtable

    Private Function Configurar() As DataTable
        dtArchivosPip = New DataTable
        Dim dcId As New DataColumn("Id", GetType(Integer))
        dcId.AutoIncrement = True
        dcId.AutoIncrementSeed = 1
        dcId.AutoIncrementStep = 1
        dtArchivosPip.Columns.Add(dcId)
        dtArchivosPip.Columns.Add(New DataColumn("Fecha", GetType(String)))
        dtArchivosPip.Columns.Add(New DataColumn("Interfaz", GetType(String)))
        dtArchivosPip.Columns.Add(New DataColumn("CodInterfaz", GetType(String)))
        dtArchivosPip.Columns.Add(New DataColumn("Nombre", GetType(String)))
        dtArchivosPip.Columns.Add(New DataColumn("Archivo", GetType(Byte())))
        dtArchivosPip.Columns.Add(New DataColumn("Extension", GetType(String)))
        Return dtArchivosPip
    End Function

    Private Function AgregarFila(ByVal _fecha As String, ByVal _interfaz As String, ByVal _codInterfaz As String,
                                 ByVal _nombre As String, ByVal _archivo As Byte(), ByVal _extension As String) As DataTable
        Dim dr As DataRow = dtArchivosPip.NewRow
        dr(1) = _fecha
        dr(2) = _interfaz
        dr(3) = _codInterfaz
        dr(4) = _nombre
        dr(5) = _archivo
        dr(6) = _extension
        dtArchivosPip.Rows.Add(dr)
        Return dtArchivosPip
    End Function
    Private Function ValidarArchvivoExcelLibor(ByVal sFileName As String, ByVal fuente As String) As String
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
            xlApp.Visible = False : xlApp.DisplayAlerts = False
            oBooks = xlApp.Workbooks
            oBooks.Open(sFileName)
            xlLibro = oBooks.Item(1)
            oSheets = xlLibro.Worksheets
            xlHoja = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = xlHoja.Cells
            Select Case fuente
                Case "TL"
                    xlHoja.Name = "Hoja1"
                    xlHoja.Range("B2").Value = "Valor"
                Case "TB"
                    ' Ninguno 
            End Select
       
            xlLibro.Save()
            xlLibro.Close()
            Return (String.Empty)
        Catch ex As Exception
            Return Replace("Hubo un error en la validación del archivo Excel de Tasa Libor / " + ex.Message.ToString(), "'", String.Empty)
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

    Private Function loadData(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dtResult As DataTable) As String
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
                    dtResult = New DataTable
                    objDataAdapter.Fill(dtResult)
                End Using
            End If
            Return String.Empty
        Catch ex As Exception
            Return "Hubo un error en la obtención de data del archivo/ " + ex.Message.ToString()
        End Try
    End Function

    Private Function guardarArchivo(ByVal archivo() As Byte, ByVal nombre As String) As Boolean
        Try
            Dim rutaGuardar As String = Path.Combine(rutaDestino, nombre)
            File.WriteAllBytes(rutaGuardar, archivo)
            Return File.Exists(rutaGuardar)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function cargarRuta(ByVal _tipoInterfaz As String, ByVal _fechaInterfaz As String, ByVal _FechaMostrar As String) As String
        msg = String.Empty
        Select Case _tipoInterfaz.Trim
            Case "Tipo de Cambio"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("005", MyBase.DatosRequest())
                rutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
                If (Not Directory.Exists(rutaDestino)) Then
                    msg = "No existe la ruta destino."
                End If
            Case "Vector de Precio", "VW", "VP_SBS"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("006", MyBase.DatosRequest())
                rutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
                If (Not Directory.Exists(rutaDestino)) Then
                    msg = "No existe la ruta destino."
                End If
            Case "VPF"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("019", MyBase.DatosRequest())
                rutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
                If (Not Directory.Exists(rutaDestino)) Then
                    msg = "No existe la ruta destino."
                End If
            Case "VTF"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("022", MyBase.DatosRequest())
                rutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4) + "VTF1" + _fechaInterfaz + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
                If (Not File.Exists(rutaDestino)) Then
                    msg = "No existe la ruta destino."
                End If
            Case "Elex"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("023", MyBase.DatosRequest())
                rutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4) + "ELEX" + _fechaInterfaz + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
            Case "VTC_SBS"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("027", MyBase.DatosRequest())
                rutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
                If (Not Directory.Exists(rutaDestino)) Then
                    msg = "No existe la ruta destino."
                End If
            Case "VF", "TL", "TB"
                rutaDestino = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
                If (Not Directory.Exists(rutaDestino)) Then
                    msg = "No existe la ruta destino."
                End If
        End Select
        Return msg
    End Function
    Private Function vectorPrecioConstructor(ByRef dtVectorPrecio As DataTable) As String
        Dim objVectorPrecioPIP As VectorPrecioPIP.VectorPrecioPIPRow
        Try
            For Each DR As DataRow In dtVectorPrecio.Rows
                objVectorPrecioPIP = OVectorPiPBE._VectorPrecioPIP.NewVectorPrecioPIPRow()
                objVectorPrecioPIP.Fecha = IIf(DR("Fecha") Is DBNull.Value, "0", DR("Fecha"))
                objVectorPrecioPIP.Hora = IIf(DR("Hora") Is DBNull.Value, "", DR("Hora"))
                objVectorPrecioPIP.CodigoNemonico = IIf(DR("Nemónico") Is DBNull.Value, "", DR("Nemónico"))
                objVectorPrecioPIP.CodigoISIN = IIf(DR("ISIN") Is DBNull.Value, "", DR("ISIN"))
                objVectorPrecioPIP.DescripcionEmisor = IIf(DR("Emisor") Is DBNull.Value, "", DR("Emisor"))
                objVectorPrecioPIP.Moneda = IIf(DR("Moneda") Is DBNull.Value, "", DR("Moneda"))
                objVectorPrecioPIP.TipoTasa = IIf(DR("Tipo de Tasa") Is DBNull.Value, "", DR("Tipo de Tasa"))
                objVectorPrecioPIP.TasaCupon = IIf(DR("Tasa de Cupón") Is DBNull.Value, "0", DR("Tasa de Cupón"))
                objVectorPrecioPIP.FechaVencimiento = IIf(DR("Fecha de Vencimiento") Is DBNull.Value, "0", DR("Fecha de Vencimiento"))
                objVectorPrecioPIP.ClasificacionRiesgo = IIf(DR("Clasificación de Riesgo") Is DBNull.Value, "", DR("Clasificación de Riesgo"))
                objVectorPrecioPIP.ValorNominal = IIf(DR("Valor Nominal") Is DBNull.Value, "0", DR("Valor Nominal"))
                objVectorPrecioPIP.PorcPrecioLimpio = IIf(DR("Precio Limpio %") Is DBNull.Value, "0", DR("Precio Limpio %"))
                objVectorPrecioPIP.TIR = IIf(DR("TIR") Is DBNull.Value, "0", DR("TIR"))
                objVectorPrecioPIP.PorcPrecioSucio = IIf(DR("Precio Sucio %") Is DBNull.Value, "0", DR("Precio Sucio %"))
                objVectorPrecioPIP.SobreTasa = IIf(DR("Sobretasa") Is DBNull.Value, "0", DR("Sobretasa"))
                objVectorPrecioPIP.PrecioLimpio = IIf(DR("Precio Limpio") Is DBNull.Value, "0", DR("Precio Limpio"))
                objVectorPrecioPIP.PrecioSucio = IIf(DR("Precio Sucio") Is DBNull.Value, "0", DR("Precio Sucio"))
                objVectorPrecioPIP.IC = IIf(DR("IC") Is DBNull.Value, "0", DR("IC"))
                objVectorPrecioPIP.Duracion = IIf(DR("Duración") Is DBNull.Value, "0", DR("Duración"))
                objVectorPrecioPIP.DetalleMoneda = IIf(DR("Detalle Moneda") Is DBNull.Value, "0", DR("Detalle Moneda"))
                objVectorPrecioPIP.FechaCarga = ""
                objVectorPrecioPIP.Escenario = "REAL"
                objVectorPrecioPIP.UsuarioCreacion = ""
                objVectorPrecioPIP.HoraCreacion = ""
                objVectorPrecioPIP.Manual = "N"
                oVectorPrecioSBSBM.VectorPrecioPIP(objVectorPrecioPIP, DatosRequest)
                oVectorPrecioSBSBM.Update_RatingValores(IIf(DR("Clasificación de Riesgo") Is DBNull.Value, "", DR("Clasificación de Riesgo")), IIf(DR("ISIN") Is DBNull.Value, "", DR("ISIN")))
            Next
            dtVectorPrecio.Columns.Remove("Fecha")
            dtVectorPrecio.Columns.Remove("Hora")
            dtVectorPrecio.Columns.Remove("Emisor")
            dtVectorPrecio.Columns.Remove("Moneda")
            dtVectorPrecio.Columns.Remove("Tipo de Tasa")
            dtVectorPrecio.Columns.Remove("Tasa de Cupón")
            dtVectorPrecio.Columns.Remove("Fecha de Vencimiento")
            dtVectorPrecio.Columns.Remove("Valor Nominal")
            dtVectorPrecio.Columns.Remove("TIR")
            dtVectorPrecio.Columns.Remove("Sobretasa")
            dtVectorPrecio.Columns.Remove("IC")
            dtVectorPrecio.Columns.Remove("Detalle Moneda")
            Return String.Empty
        Catch ex As Exception
            Return "Hubo un error en la obtención de data del archivo/ " + ex.Message.ToString()
        End Try
    End Function
    Private Function VectorPrecioForwardsConstructor(ByRef dtVectorPrecioFW As DataTable) As String
        Try
            dtVectorPrecioFW.Columns.Remove("Fecha")
            dtVectorPrecioFW.Columns.Remove("Hora")
            dtVectorPrecioFW.Columns.Remove("Nemónico")
            dtVectorPrecioFW.Columns.Remove("Emisor")
            dtVectorPrecioFW.Columns.Remove("Contraparte")
            dtVectorPrecioFW.Columns.Remove("Tipo de Operación")
            dtVectorPrecioFW.Columns.Remove("Subyacente")
            dtVectorPrecioFW.Columns.Remove("Fecha de Emisión")
            dtVectorPrecioFW.Columns.Remove("Fecha de Vencimiento")
            Return String.Empty
        Catch ex As Exception
            Return "Hubo un error en la inserción de data del archivo/ " + ex.Message.ToString()
        End Try
    End Function
    Private Function InsertarTipoCambio(ByVal dtTipoCambio As DataTable, ByVal fechaTipoCambio As Decimal, ByVal entidadExt As String) As String
        Dim errorVacio As Boolean = False
        'Dim dtNoExiste As New DataTable
        Dim valorTipoCambio As Decimal
        Dim codigoMonedaSBS As String
        'dtNoExiste.Columns.Add("CodigoMonedaSBS")
        If dtTipoCambio.Rows.Count > 0 Then
            If dtTipoCambio.Rows(0)("Fecha de Vencimiento").ToString() <> String.Empty Then Return "Formato no corresponde al tipo de Intertaz."
            Try
                For index = 0 To dtTipoCambio.Rows.Count - 1
                    codigoMonedaSBS = dtTipoCambio.Rows(index)("Nemónico")
                    Dim oVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow
                    oVectorTipoCambio = oVectorTipoCambioBE.VectorTipoCambio.NewVectorTipoCambioRow()
                    valorTipoCambio = dtTipoCambio.Rows(index)("Precio Limpio")
                    oVectorTipoCambio.Fecha = fechaTipoCambio
                    oVectorTipoCambio.Valor = valorTipoCambio
                    oVectorTipoCambio.CodigoMoneda = codigoMonedaSBS
                    oVectorTipoCambio.EntidadExt = entidadExt
                    oVectorTipoCambioBM.Insertar(oVectorTipoCambio, MyBase.DatosRequest())
                Next
                oVectorTipoCambioBM.Actualiza_ValorCambioMoneda(fechaTipoCambio, entidadExt)
                oVectorTipoCambioBM.Copiar_TipoCambio(fechaTipoCambio, entidadExt)
                Return String.Empty
            Catch ex As Exception
                Return "Hubo un error en la inserción de data del archivo/ " + ex.Message.ToString()
            End Try
        Else
            Return "No hay datos en el Archivos Plano..."
        End If
    End Function
    Private Function InsertarTasaLibor(ByVal dtTasaLibor As DataTable, ByVal fechaTasaLibor As Decimal) As String
        Dim errorVacio As Boolean = False
        Dim valorCurva As Decimal = 0, plazoCurva As Integer = 0, codigoIndicador As String = String.Empty
        Dim dtIndicador As DataTable = oIndicadorBM.Indicador_SeleccionarIndicadorLibor("SW", "P")
        Dim drTasaLibor() As DataRow

        If dtTasaLibor.Rows.Count > 0 Then
            If dtTasaLibor.Rows(0)("Curva").ToString() = String.Empty Then Return "Formato no corresponde al tipo de Intertaz."
            Try
                If dtIndicador.Rows.Count > 0 Then
                    For index = 0 To dtIndicador.Rows.Count - 1
                        drTasaLibor = dtTasaLibor.Select("Curva = '" & dtIndicador.Rows(index)("DiasLibor").ToString() & "'")
                        plazoCurva = CInt(drTasaLibor(0)("Curva").ToString)
                        valorCurva = CDec(drTasaLibor(0)("Valor").ToString)
                        codigoIndicador = dtIndicador.Rows(index)("CodigoIndicador").ToString
                        oVectorTipoCambioBM.TasaLibor_InsertarVectorCarga_PIP(plazoCurva, valorCurva, fechaTasaLibor, codigoIndicador, MyBase.DatosRequest())
                    Next
                Else
                    Return "No se tiene definido ningún Indicador de Tasa Libor."
                End If
                Return String.Empty
            Catch ex As Exception
                Return "Hubo un error en la inserción de data del archivo/ " + ex.Message.ToString()
            End Try
        Else
            Return "No hay datos en el Archivos Plano..."
        End If
    End Function

    Public Class IndicadorLibor
        Public Fecha As Decimal
        Public Tasa As Decimal
        Public Mes As Integer
        Public Codigo As String
        Public NombreColumna As String
    End Class

    Private Function InsertarTasaLiborBloomberg(dtTasaLibor As DataTable, fechaD As Decimal) As String
        Dim dtIndicador As DataTable = oIndicadorBM.Indicador_SeleccionarIndicadorLibor("SW", "B")
        Dim estaVacio As Boolean = False
        Dim drTasaLibor() As DataRow
        Dim drIndicador() As DataRow
        Dim listaLibor As New List(Of IndicadorLibor)
        Dim strQuery As String = "TickerBBG_FECHA='" & UIUtility.ConvertirDecimalAStringFormatoFecha(fechaD) & "'"
        If dtTasaLibor.Rows.Count > 0 Then

            If dtTasaLibor.Columns(0).DataType.Name.ToUpper = "DATETIME" Then
                dtTasaLibor.Columns.Add("TickerBBG_FECHA")
                For Each row In dtTasaLibor.Rows
                    row("TickerBBG_FECHA") = String.Format("{0:dd/MM/yyyy}", row("Ticker BBG"))
                Next
            Else
                dtTasaLibor.Columns(0).ColumnName = "TickerBBG_FECHA"
            End If

            drTasaLibor = dtTasaLibor.Select(strQuery)

            If drTasaLibor.Length > 0 Then

                'verificar las columnas 
                If dtTasaLibor.Columns.Count > 0 Then
                    For ind = 1 To dtTasaLibor.Columns.Count - 1
                        Dim columnaLibor As String = dtTasaLibor.Columns(ind).ColumnName
                        If columnaLibor <> "TickerBBG_FECHA" Then
                            Dim objLibor As New IndicadorLibor
                            objLibor.Fecha = fechaD
                            objLibor.NombreColumna = dtTasaLibor.Columns(ind).ColumnName
                            objLibor.Mes = Convert.ToInt32(dtTasaLibor.Columns(ind).ColumnName.Substring(4, 2))
                            objLibor.Tasa = CDec(drTasaLibor(0)(ind).ToString)
                            drIndicador = dtIndicador.Select("MesLibor='" & objLibor.Mes.ToString() & "'")
                            If drIndicador.Length Then
                                objLibor.Codigo = drIndicador(0)("CodigoIndicador").ToString()
                                listaLibor.Add(objLibor)
                            End If
                        End If
                    Next

                    If listaLibor.Count = 0 Then
                        Return "No existe un código de identificador de la tasa libor, para los valores que desea subir."
                    End If

                    For i = 0 To listaLibor.Count - 1
                        oVectorTipoCambioBM.TasaLibor_InsertarVectorCarga_PIP(listaLibor(i).Mes, listaLibor(i).Tasa, fechaD, listaLibor(i).Codigo, MyBase.DatosRequest())
                    Next

                Else
                    Return "El archivo no contiene columnas"
                End If

            Else
                Return "No existe tasa para la fecha que se desea cargar"
            End If
        Else
            Return "No hay datos en el Archivos Plano..."
        End If
        Return String.Empty
    End Function
    Private Function InsertarVectorPrecio(ByVal dtVectorPrecio As DataTable, ByVal fechaProceso As Decimal) As String
        Dim errorVacio As Boolean = False
        Dim dtNoExiste As New DataTable
        Dim drNoExisteRow As DataRow
        dtNoExiste.Columns.Add("CodigoISIN")
        dtNoExiste.Columns.Add("Nemonico")
        dtNoExiste.Columns.Add("PrecioLimpio")
        dtNoExiste.Columns.Add("PrecioSUcio")
        If dtVectorPrecio.Rows.Count > 0 Then
            oVectorPrecioBM.BorrarVectorPrecio(fechaProceso, "N", "REAL")
            Dim dtISIN_NEMONICOS As DataTable = oVectorPrecioBM.ListarValores_ISIN()
            Try
                For ind = 0 To dtVectorPrecio.Rows.Count - 1
                    Dim foundRows() As DataRow
                    foundRows = dtISIN_NEMONICOS.Select("codigoisin = '" & dtVectorPrecio.Rows(ind)("ISIN").ToString() & "'")
                    If foundRows.Length > 0 Then
                      
                        Dim beVectorPrecio As VectorPrecioBE.VectorPrecioRow
                        beVectorPrecio = oVectorPrecioBE.VectorPrecio.NewVectorPrecioRow()
                        beVectorPrecio.CodigoMnemonico = dtVectorPrecio.Rows(ind)("Nemónico")
                        'If CInt(IIf(dtVectorPrecio.Rows(ind)("Precio Sucio") = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Sucio"))) = 0 Then
                        '    beVectorPrecio.Valor = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Limpio") = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Limpio")))
                        'Else
                        '    beVectorPrecio.Valor = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Sucio") = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Sucio")))
                        'End If
                        'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
                        If foundRows(0).ItemArray(3).ToString.Contains("100") Or foundRows(0).ItemArray(3).ToString.Contains("101") Then
                            dtVectorPrecio.Rows(ind)("Precio Limpio") = (IIf(dtVectorPrecio.Rows(ind)("Precio Limpio") = String.Empty, 0, Decimal.Parse(dtVectorPrecio.Rows(ind)("Precio Limpio"))) * 10).ToString
                            dtVectorPrecio.Rows(ind)("Precio Sucio") = (IIf(dtVectorPrecio.Rows(ind)("Precio Sucio") = String.Empty, 0, Decimal.Parse(dtVectorPrecio.Rows(ind)("Precio Sucio"))) * 10).ToString
                        End If
                        beVectorPrecio.Valor = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Limpio") = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Limpio")))
                        beVectorPrecio.Fecha = fechaProceso
                        beVectorPrecio.EntidadExt = "REAL"
                        beVectorPrecio.Manual = "N"
                        beVectorPrecio.CodigoISIN = dtVectorPrecio.Rows(ind)("ISIN").ToString()
                        beVectorPrecio.HoraCreacion = IIf(dtVectorPrecio.Rows(ind)("Hora").ToString() Is DBNull.Value, "", dtVectorPrecio.Rows(ind)("Hora").ToString())
                        beVectorPrecio.DescripcionEmisor = IIf(dtVectorPrecio.Rows(ind)("Emisor").ToString() Is DBNull.Value, "", dtVectorPrecio.Rows(ind)("Emisor").ToString())
                        beVectorPrecio.CodigoMoneda = IIf(dtVectorPrecio.Rows(ind)("Moneda").ToString() Is DBNull.Value, "", dtVectorPrecio.Rows(ind)("Moneda").ToString())
                        beVectorPrecio.TipoTasa = IIf(dtVectorPrecio.Rows(ind)("Tipo de Tasa").ToString() Is DBNull.Value, "", dtVectorPrecio.Rows(ind)("Tipo de Tasa").ToString())
                        beVectorPrecio.TasaCupon = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Tasa de Cupón").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Tasa de Cupón")))
                        beVectorPrecio.FechaVencimiento = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Fecha de Vencimiento").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Fecha de Vencimiento")))
                        beVectorPrecio.ClasificacionRiesgo = IIf(dtVectorPrecio.Rows(ind)("Clasificación de Riesgo").ToString() Is DBNull.Value, "", dtVectorPrecio.Rows(ind)("Clasificación de Riesgo").ToString)
                        beVectorPrecio.ValorNominal = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Valor Nominal").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Valor Nominal")))
                        beVectorPrecio.PorcPrecioLimpio = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Limpio %").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Limpio %")))
                        beVectorPrecio.TIR = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("TIR").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("TIR")))
                        beVectorPrecio.PorcPrecioSucio = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Sucio %").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Sucio %")))
                        beVectorPrecio.SobreTasa = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Sobretasa").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Sobretasa")))
                        beVectorPrecio.PrecioLimpio = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Limpio").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Limpio")))
                        beVectorPrecio.PrecioSucio = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Precio Sucio").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Precio Sucio")))
                        beVectorPrecio.IC = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("IC").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("IC")))
                        beVectorPrecio.Duracion = Decimal.Parse(IIf(dtVectorPrecio.Rows(ind)("Duración").ToString() = String.Empty, 0, dtVectorPrecio.Rows(ind)("Duración")))
                        beVectorPrecio.DetalleMoneda = IIf(dtVectorPrecio.Rows(ind)("Detalle Moneda").ToString() Is DBNull.Value, "", dtVectorPrecio.Rows(ind)("Detalle Moneda").ToString())
                        oVectorPrecioBM.Insertar(beVectorPrecio, MyBase.DatosRequest())
                    Else
                        drNoExisteRow = dtNoExiste.NewRow
                        drNoExisteRow("CodigoISIN") = dtVectorPrecio.Rows(ind)("ISIN")
                        drNoExisteRow("Nemonico") = dtVectorPrecio.Rows(ind)("Nemónico")
                        drNoExisteRow("PrecioLimpio") = dtVectorPrecio.Rows(ind)("Precio Limpio")
                        drNoExisteRow("PrecioSUcio") = dtVectorPrecio.Rows(ind)("Precio Sucio")
                        dtNoExiste.Rows.Add(drNoExisteRow)
                    End If
                Next
                Return String.Empty
            Catch ex As Exception
                Return "Hubo un error en la obtención de data del archivo/ " + ex.Message.ToString()
            End Try
        Else
            Return "No hay datos en el Archivos Plano..."
        End If
    End Function

    Private Function InsertarVectorPrecioFW(ByVal dtVectorPrecioFW As DataTable, ByVal fechaProceso As Decimal) As String
        Dim ds As DataSet
        Dim sbMensaje As StringBuilder

        If dtVectorPrecioFW.Rows.Count > 0 Then
            Try
                For ind = 0 To dtVectorPrecioFW.Rows.Count - 1
                    Dim beVectorFodward As VectorForwardSBSBE.VectorForwardSBSRow
                    beVectorFodward = oVectorFordWardBE.VectorForwardSBS.NewVectorForwardSBSRow()
                    beVectorFodward.NumeroPoliza = dtVectorPrecioFW.Rows(ind)("ISIN")
                    beVectorFodward.Fecha = fechaProceso
                    beVectorFodward.PrecioForward = dtVectorPrecioFW.Rows(ind)("Strike Price")
                    beVectorFodward.MtmDestino = dtVectorPrecioFW.Rows(ind)("Valor de Operación")
                    beVectorFodward.Mtm = dtVectorPrecioFW.Rows(ind)("Valor de Operación")
                    beVectorFodward.PrecioVector = dtVectorPrecioFW.Rows(ind)("Valor de Operación")
                    oVectorFordWardBM.Insertar(beVectorFodward, MyBase.DatosRequest())
                Next
            Catch ex As Exception
                oVectorFordWardBM.EliminatmpVectorForwardSBS()
                Return "Hubo un error en la obtención de data del archivo/ " + ex.Message.ToString()
            End Try
        Else
            Return "No hay datos en el Archivo Plano..."
        End If
        Try
            ds = oVectorFordWardBM.CargarPrecioFWD(fechaProceso)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count = dtVectorPrecioFW.Rows.Count Then
                    Return "Primero se debe cargar el vector TIPO CAMBIO del día: " & UIUtility.ConvertirFechaaString(fechaProceso)
                Else
                    sbMensaje = CrearMensaje(ds)
                    Return sbMensaje.ToString()
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            Return "Hubo un error en la CargarPrecioFWD/" + ex.Message.ToString()
        Finally
            oVectorFordWardBM.EliminatmpVectorForwardSBS()
        End Try
    End Function

    Private Function InsertarTasaInteresForward(ByVal rutaDestino As String, ByVal fecha As Decimal) As String
        Dim oConn As New OleDbConnection
        Try
            oConn = New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oCmd1 As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDa1 As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim oDs1 As New DataSet
            Dim dtTasaInteres As New DataTable
            Dim dtPrecioForward As New DataTable
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaDestino & "; Extended Properties= Excel 8.0;HDR=Yes;IMEX=1;"
            oConn.Open()
            oCmd.CommandText = "SELECT moneda,plazo,activa,pasiva FROM [TASA_INTERES$] where len(moneda)>0"
            oCmd.Connection = oConn
            oCmd1.CommandText = "SELECT REF, [precio forward] FROM [PRECIO_FORWARD$] where len(REF) > 0 "
            oCmd1.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "TasaInteres")
            oDa1.SelectCommand = oCmd1
            oDa1.Fill(oDs1, "PrecioForward")
            dtTasaInteres = oDs.Tables(0)
            dtPrecioForward = oDs1.Tables(0)
            oConn.Close()
            If dtTasaInteres.Rows.Count = 1 And dtPrecioForward.Rows.Count = 1 Then
                Return "El Archivo no tiene registros."
            Else
                Try
                    Dim oBM As New VectorPrecioBM
                    oBM.InsertarTasaInteresPrecioForward(fecha, dtTasaInteres, dtPrecioForward, Me.DatosRequest)
                    Return String.Empty
                Catch ex As Exception
                    Return "Hubo un error en la inserción de data del archivo/ " + ex.Message.ToString()
                End Try
            End If
        Catch ex As OleDbException
            If ex.ErrorCode = -2147467259 Then
                'OT10689 - Inicio. Se crea un mensaje de error y se lanza al TryCash principal.
                Return "El excel se encuentra abierto o\nEl nombre de la pestaña no es el correcto."
            End If
        Catch ex As Exception
            'OT10689 - Inicio. Se lanza el mensaje de error al TryCash principal.
            Return ex.Message.ToString
        Finally
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
            End If
        End Try
        Return String.Empty
    End Function

    Private Function InsertarArchivoOperacionesBVL(ByVal rutaDestino As String, ByVal fecha As Decimal) As String
        Dim oConn As New OleDbConnection
        Try
            oConn = New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim dtOperacionBVL As New DataTable
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaDestino & "; Extended Properties= Excel 8.0;HDR=Yes;IMEX=1;"
            oConn.Open()
            oCmd.CommandText = "SELECT Valor,SUM(Cantidad) as Cantidad FROM [ELEX$] where Cantidad>0 group by valor"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "ELEX")
            dtOperacionBVL = oDs.Tables(0)
            oConn.Close()
            If dtOperacionBVL.Rows.Count = 1 And dtOperacionBVL.Rows.Count = 1 Then
                Return "El Archivo no tiene registros"
            Else
                Try
                    Dim oBM As New VectorPrecioBM
                    oBM.InsertarOperacionesBVL(fecha, dtOperacionBVL, Me.DatosRequest)
                    Return String.Empty
                Catch ex As Exception
                    Return "Hubo un error en la inserción de data del archivo/ " + ex.Message.ToString()
                End Try
            End If
        Catch ex As OleDbException
            If ex.ErrorCode = -2147467259 Then
                'OT10689 - Inicio. Se crea un mensaje de error y se lanza al TryCash principal.
                Return "El nombre la Pestaña debe ser ELEX."
            End If
        Catch ex As Exception
            'OT10689 - Inicio. Se lanza el mensaje de error al TryCash principal.
            Return ex.Message.ToString()
        Finally
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
            End If
        End Try
        Return String.Empty
    End Function

    Private Function InsertarArchivoWarrant(ByVal dtVectorWarrant As DataTable, ByVal fechaProceso As Decimal) As String
        If dtVectorWarrant.Rows.Count > 0 Then
            Try
                For ind = 0 To dtVectorWarrant.Rows.Count - 1
                    If Not IsDBNull(dtVectorWarrant.Rows(ind)(2)) Then
                        oVectorPrecioSBSBM.InsertarVectorWarrant(dtVectorWarrant.Rows(ind)(2), dtVectorWarrant.Rows(ind)(5), _
                        fechaProceso, dtVectorWarrant.Rows(ind)(8), dtVectorWarrant.Rows(ind)(12), _
                        dtVectorWarrant.Rows(ind)(13), DatosRequest)
                    End If
                Next
                Return String.Empty
            Catch ex As Exception
                Return "Hubo un error en la inserción de data del archivo/ " + ex.Message.ToString()
            End Try
        Else
            Return "No hay datos en el Archivos Plano..."
        End If
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Page.IsPostBack = False Then

                If Session("dtArchivosPip") IsNot Nothing Then
                    Session("dtArchivosPip") = Nothing
                End If
                dtArchivosPip = Configurar()
                Me.gvPip.DataSource = AgregarFila("-1", "-1", "-1", "-1", Nothing, "-1")
                Me.gvPip.DataBind()
                btnProcesar.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar el formulario")
        End Try
    End Sub

    Private Function crearArchivoExcel(ByVal archivo() As Byte, ByVal nombre As String) As String
        Try
            Dim rutaDestinoTemporal As String = rutaDestino
            Dim fechaCadena As String = Date.Now.ToString("yyyyMMdd_Hmmss")

            Dim archivoRutaFinal As String = rutaDestinoTemporal & "\" & nombre & "_" & ++Date.Now.Hour + Date.Now.Minute + Date.Now.Second
            File.WriteAllBytes(rutaDestinoTemporal & "\" & nombre, archivo)

            Dim ObjCom As UIUtility.COMObjectAplication = Nothing
            Dim xlApp As Excel.Application
            Dim oBooks As Excel.Workbooks
            Dim oOrdenInversion As New OrdenPreOrdenInversionBM
            Dim oOperacion As New OperacionBM
            Dim dtOperacioneEPU As New DataTable
            Dim newFileName As String = "EXCEL_CARTERA_" & fechaCadena & ".xls"

            If File.Exists(rutaDestinoTemporal & "\" & nombre) Then
                Try
                    ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
                    xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
                    oBooks = xlApp.Workbooks
                    oBooks.OpenText(rutaDestinoTemporal & "\" & nombre, DataType:=Excel.XlTextParsingType.xlDelimited, Tab:=True)
                    xlApp.DisplayAlerts = False

                    Dim hoja As Excel.Worksheet
                    hoja = xlApp.Worksheets(1)
                    hoja.Name = "DATOS"

                    ' Save the file as XLS file
                    xlApp.ActiveWorkbook.SaveAs(Filename:=rutaDestino & "\" & newFileName, FileFormat:=Excel.XlFileFormat.xlExcel5, CreateBackup:=False)

                    ' Close the workbook
                    xlApp.ActiveWorkbook.Close(SaveChanges:=True)
                    oBooks.Close()
                    ' Turn the messages back on
                    xlApp.DisplayAlerts = True

                    ' Quit from Excel
                    xlApp.Quit()

                    ' Kill the variable
                    xlApp = Nothing

                    '    xlApp.Quit()
                    If ObjCom IsNot Nothing Then
                        ObjCom.terminarProceso()
                    End If
                    File.Delete(rutaDestinoTemporal & "\" & nombre)
                Catch ex As Exception
                    Return String.Empty
                End Try

            End If

            Return newFileName
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function obtenerCodigoMoneda(ByVal pCodigoMonedaISO As String) As String
        Dim resultado As String
        resultado = IIf("PEN".Equals(pCodigoMonedaISO), "1", "2")
        Return resultado

    End Function

    Private Function InsertarVectorFactoring(ByVal dtVectorFactoring As DataTable, ByVal fechaProceso As Decimal) As String
        Dim errorVacio As Boolean = False
        Dim dtNoExiste As New DataTable
        Dim drNoExisteRow As DataRow

        If dtVectorFactoring.Rows.Count = 0 Then
            Return "Archivos Plano. Cantidad registros: " + dtVectorFactoring.Rows.Count
        End If

        dtNoExiste.Columns.Add("CodigoISIN")
        dtNoExiste.Columns.Add("Nemonico")
        dtNoExiste.Columns.Add("PrecioLimpio")
        dtNoExiste.Columns.Add("PrecioSUcio")
        Dim listaEstados = (From dr In dtVectorFactoring
                           Select dr("Serie")
                           ).ToList()

        Dim dtValida As DataTable

        If (listaEstados.Count > 0) Then
            dtValida = oVectorPrecioBM.ListarValores_Facturas(String.Join(",", listaEstados.ToArray()))
        Else
            dtValida = New DataTable
        End If
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM


        If dtValida.Rows.Count > 0 Then
            oVectorPrecioBM.BorrarVectorPrecio(fechaProceso, "N", "SAB")

            Try
                For ind = 0 To dtValida.Rows.Count - 1
                    Dim foundRows() As DataRow
                    foundRows = dtVectorFactoring.Select("Serie = '" & dtValida.Rows(ind)("CodigoFactura").ToString() & "'")
                    If foundRows.Length > 0 Then
                        Dim beVectorPrecio As VectorPrecioBE.VectorPrecioRow
                        beVectorPrecio = oVectorPrecioBE.VectorPrecio.NewVectorPrecioRow()
                        beVectorPrecio.CodigoMnemonico = dtValida.Rows(ind)("CodigoNemonico")
                        beVectorPrecio.Valor = 0
                        beVectorPrecio.Fecha = fechaProceso
                        beVectorPrecio.EntidadExt = "SAB"
                        beVectorPrecio.Manual = "N"
                        beVectorPrecio.CodigoISIN = dtValida.Rows(ind)("CodigoISIN")
                        beVectorPrecio.HoraCreacion = New Date().ToLongTimeString()
                        beVectorPrecio.DescripcionEmisor = "SAB SURA"
                        beVectorPrecio.CodigoMoneda = obtenerCodigoMoneda(foundRows(0)("Mon"))
                        beVectorPrecio.TipoTasa = ""
                        beVectorPrecio.TasaCupon = 2
                        beVectorPrecio.FechaVencimiento = fechaProceso
                        beVectorPrecio.ClasificacionRiesgo = ""
                        beVectorPrecio.ValorNominal = foundRows(0)("Monto Disponible")
                        beVectorPrecio.PorcPrecioLimpio = foundRows(0)("PrMer")
                        beVectorPrecio.TIR = 0
                        beVectorPrecio.PorcPrecioSucio = foundRows(0)("PrMer")
                        beVectorPrecio.SobreTasa = 0
                        beVectorPrecio.PrecioLimpio = foundRows(0)("PrMer")
                        beVectorPrecio.PrecioSucio = foundRows(0)("PrMer")
                        beVectorPrecio.IC = 0
                        beVectorPrecio.Duracion = 0
                        beVectorPrecio.DetalleMoneda = ""
                        oOrdenPreOrdenInversionBM.ActualizaPrecioLimpioSucio(beVectorPrecio.CodigoMnemonico, beVectorPrecio.PrecioLimpio)
                        oVectorPrecioBM.Insertar(beVectorPrecio, MyBase.DatosRequest())

                    Else
                        drNoExisteRow = dtNoExiste.NewRow
                        drNoExisteRow("CodigoISIN") = dtValida.Rows(ind)("CodigoFactura")
                        drNoExisteRow("Nemonico") = dtVectorFactoring.Rows(ind)("Nemónico")
                        drNoExisteRow("PrecioLimpio") = dtVectorFactoring.Rows(ind)("Precio Limpio")
                        drNoExisteRow("PrecioSUcio") = dtVectorFactoring.Rows(ind)("Precio Sucio")
                        dtNoExiste.Rows.Add(drNoExisteRow)
                    End If

                Next
                Return String.Empty
            Catch ex As Exception
                Return "Hubo un error en la obtención de data del archivo/ " + ex.Message.ToString()
            End Try
        End If

    End Function


    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        oVectorTipoCambioBM = New VectorTipoCambioBM
        msgGral = New StringBuilder
        Dim mensajeDetalle As String = String.Empty
        Dim p As Integer = 0
        msgError.Text = "<hr/><b><i><u>LOG DE PROCESAMIENTO:</b></i></u><br/><br/>"
        Try
            If Session("dtArchivosPip") IsNot Nothing Then
                dtArchivosPip = Session("dtArchivosPip")
                For i = 0 To dtArchivosPip.Rows.Count - 1

                    fecha = dtArchivosPip(i)("Fecha").ToString.Trim
                    tipoIf = dtArchivosPip(i)("CodInterfaz").ToString.Trim
                    nombreInterface = dtArchivosPip(i)("Interfaz").ToString.Trim
                    tipoExtension = dtArchivosPip(i)("Extension").ToString.Trim
                    If tipoExtension <> "" Then
                        tipoExtension = tipoExtension.ToLower()
                    End If
                    archivo = dtArchivosPip(i)("Nombre").ToString.Trim
                    blob = CType(dtArchivosPip(i)("Archivo"), Byte())
                    fechaInterface = fecha.Substring(3, 2) + fecha.Substring(0, 2)
                    msg = cargarRuta(tipoIf, fechaInterface, fecha)
                    mensajeDetalle = "Fecha: <b>" + fecha + "</b> - Interfaz: <b>" + nombreInterface + "</b>"
                    If tipoExtension.Equals(".xls") Or tipoExtension.Equals(".xlsx") Or tipoExtension.Equals(".xlsm") Then
                        If msg.Length = 0 Then
                            If guardarArchivo(blob, archivo) Then
                                fechaD = UIUtility.ConvertirFechaaDecimal(fecha)
                                Select Case tipoIf
                                    Case "Tipo de Cambio"
                                        msg = loadData(rutaDestino & "\" & archivo, "Hoja1", "A2:P130", dtsbs)
                                        If msg = String.Empty Then
                                            oVectorTipoCambioBM.EliminarTipoCambioReal_SBS(fechaD, "REAL")
                                            msg = InsertarTipoCambio(dtsbs, fechaD, "REAL")
                                        End If
                                    Case "Vector de Precio"
                                        msg = loadData(rutaDestino & "\" & archivo, "Hoja1", "A2:T1500", dtsbs)
                                        If msg = String.Empty Then
                                            'oVectorPrecioSBSBM.delVectorPrecioPIP(fechaD, "N", "REAL")
                                            'msg = vectorPrecioConstructor(dtsbs)
                                            'If msg = String.Empty Then msg = InsertarVectorPrecio(dtsbs, fechaD)
                                            msg = InsertarVectorPrecio(dtsbs, fechaD)
                                        End If
                                    Case "VF"
                                        '==== VECTOR FACTORING | CDA | Ernesto Galarza | 2019-01-16 | Carga de archivo excel cartera que se utiliza en facturas negociables
                                        Dim nombreArchivoExcel As String = crearArchivoExcel(blob, archivo)
                                        If (nombreArchivoExcel = String.Empty) Then
                                            msg = "Hubo un problema al procesar el archivo. Comunicarse con el administrador."
                                        Else
                                            msg = loadData(rutaDestino & "\" & nombreArchivoExcel, "DATOS", "A2:AF1500", dtsbs)
                                            If msg = String.Empty Then
                                                If File.Exists(rutaDestino & "\" & nombreArchivoExcel) Then
                                                    File.Delete(rutaDestino & "\" & nombreArchivoExcel)
                                                End If
                                                msg = InsertarVectorFactoring(dtsbs, fechaD)
                                            End If
                                        End If
                                    Case "VPF"
                                        msg = loadData(rutaDestino & "\" & archivo, "Hoja1", "A2:L1500", dtsbs)
                                        If msg = String.Empty Then
                                            msg = VectorPrecioForwardsConstructor(dtsbs)
                                            If msg = String.Empty Then msg = InsertarVectorPrecioFW(dtsbs, fechaD)
                                        End If
                                    Case "VTF"
                                        msg = InsertarTasaInteresForward(rutaDestino, fecha)
                                    Case "Elex"
                                        msg = InsertarArchivoOperacionesBVL(rutaDestino, fecha)
                                    Case "VW"
                                        msg = loadData(rutaDestino & "\" & archivo, "Hoja1", "A2:W1500", dtsbs)
                                        If msg = String.Empty Then
                                            oVectorPrecioSBSBM.BorrarVectorWarrant(fechaD)
                                            msg = InsertarArchivoWarrant(dtsbs, fechaD)
                                        End If
                                    Case "VTC_SBS"
                                        msg = loadVTC_SBS(rutaDestino & "\" & archivo, fechaD)
                                    Case "VP_SBS"
                                        msg = loadVP_SBS(rutaDestino & "\" & archivo, fechaD)
                                    Case "TL"
                                        msg = ValidarArchvivoExcelLibor(rutaDestino & "\" & archivo, tipoIf)
                                        If msg = String.Empty Then
                                            msg = loadData(rutaDestino & "\" & archivo, "Hoja1", "A2:B12000", dtsbs)
                                            If msg = String.Empty Then
                                                oVectorTipoCambioBM.TasaLibor_EliminarVectorCarga_PIP(fechaD, "L")
                                                msg = InsertarTasaLibor(dtsbs, fechaD)
                                            End If
                                        End If
                                    Case "TB"
                                        msg = ValidarArchvivoExcelLibor(rutaDestino & "\" & archivo, tipoIf)
                                        If msg = String.Empty Then
                                            msg = loadData(rutaDestino & "\" & archivo, "Valores", "A1:E100", dtsbs)
                                            If msg = String.Empty Then
                                                oVectorTipoCambioBM.TasaLibor_EliminarVectorCarga_PIP(fechaD, "B")
                                                msg = InsertarTasaLiborBloomberg(dtsbs, fechaD)
                                            End If

                                        End If



                                End Select
                                If msg = String.Empty Then
                                    msgGral.Append(mensajeDetalle + "<p style='color:#0000FF';><b>El archivo '" & archivo & "' se cargó correctamente.</b></p><br/>")
                                    p += 1
                                    Continue For
                                Else
                                    msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>" + msg & "</b></p><br/>")
                                End If
                            Else
                                msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>El archivo " & archivo & " no se llegó a cargar correctamente.</b></p><br/>")
                            End If
                        Else
                            msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>" + msg & "</b></p><br/>")
                        End If
                    Else
                        msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>El archivo " & archivo & " no tiene extensión válida(.xls)</b></p><br/>")
                    End If
                    listaErrores.Add(i.ToString, String.Empty)
                Next
            Else
                msgGral.Append("No se tiene ningún archivo para importar.")
            End If

        Catch ex As Exception
            msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>Ocurrió un error al realizar el proceso/ " + ex.Message.ToString() + "<br/> -->>>  [Proceso Abortado] <<<--</b></p>")
        Finally
            AlertaJS("<p align=left>- Se ejecutaron " + p.ToString() + " carga(s) correctamente.<br/>- Hubieron " + (dtArchivosPip.Rows.Count - p).ToString() + " carga(s) con errores.</p>")
            msgError.Text += msgGral.ToString
            Me.gvPip.DataSource = Session("dtArchivosPip")
            Me.gvPip.DataBind()
        End Try
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        dtArchivosPip = Configurar()
        Session("dtArchivosPip") = Nothing
        Me.gvPip.DataSource = AgregarFila("-1", "-1", "-1", "-1", Nothing, "-1")
        Me.gvPip.DataBind()
        btnProcesar.Enabled = False
    End Sub
    Private Sub Imagebutton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Imagebutton3.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Protected Sub gvPip_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPip.RowDataBound
        Dim xError As DictionaryEntry
        Dim txtRutaError As TextBox
        Dim divFechas As HtmlGenericControl
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                tCaja = CType(e.Row.FindControl("tbFechaInterface"), TextBox)
                hdOculto = CType(e.Row.FindControl("hdInterface"), HiddenField)
                ddlCombo = CType(e.Row.FindControl("ddlInterface"), DropDownList)
                divFechas = CType(e.Row.FindControl("divFecha"), HtmlGenericControl)

                If tCaja.Text.Trim.Equals("-1") Then
                    e.Row.Visible = False
                End If

                HelpCombo.LlenarComboBox(ddlCombo, dtImportarSbs, "Valor", "Nombre", True)
                ddlCombo.SelectedValue = hdOculto.Value

                ddlCombo.Enabled = False
                tCaja.Enabled = False
                divFechas.Attributes.Add("class", "input-append")

                If listaErrores.Count > 0 Then
                    For Each xError In listaErrores
                        If xError.Key = e.Row.DataItemIndex.ToString Then
                            txtRutaError = CType(e.Row.FindControl("txtFile"), TextBox)
                            txtRutaError.BorderColor = Drawing.Color.Red
                        End If
                    Next
                End If
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                ddlCombo = CType(e.Row.FindControl("ddlInterfaceF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlCombo, dtImportarSbs, "Valor", "Nombre", True)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar el listado de la grilla/ " + ex.Message.ToString())
        End Try
    End Sub

    Protected Sub gvPip_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvPip.RowDeleting
        Try
            Dim fila As GridViewRow = CType(gvPip.Rows(e.RowIndex), GridViewRow)
            Dim ltId As Literal = CType(fila.FindControl("ltId"), Literal)
            Dim Id As Integer = ltId.Text
            dtArchivosPip = Session("dtArchivosPip")

            For i = 0 To dtArchivosPip.Rows.Count - 1
                If dtArchivosPip.Rows(i)("Id") = Id Then
                    dtArchivosPip.Rows.RemoveAt(i)
                    Session("dtArchivosPip") = dtArchivosPip
                    If dtArchivosPip.Rows.Count = 0 Then
                        dtArchivosPip = Configurar()
                        Session("dtArchivosPip") = Nothing
                        Me.gvPip.DataSource = AgregarFila("-1", "-1", "-1", "-1", Nothing, "-1")
                        btnProcesar.Enabled = False
                    Else
                        Me.gvPip.DataSource = dtArchivosPip
                        btnProcesar.Enabled = True
                    End If
                    Me.gvPip.DataBind()
                    Exit For
                End If
            Next
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la eliminación de la fila/ " + ex.Message.ToString())
        End Try
    End Sub

    Protected Sub gvPip_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPip.RowCommand
        Try
            If e.CommandName = "Add" Then

                tCaja = CType(gvPip.FooterRow.FindControl("tbFechaInterfaceF"), TextBox)
                If tCaja.Text.Length <= 0 Then
                    AlertaJS("Ingresar Fecha Interfaz.")
                    Exit Sub
                End If

                ddlCombo = CType(gvPip.FooterRow.FindControl("ddlInterfaceF"), DropDownList)
                If ddlCombo.SelectedItem.Text = "--SELECCIONE--" Then
                    AlertaJS("Seleccionar tipo de Interfaz.")
                    Exit Sub
                End If

                fpCargar = CType(gvPip.FooterRow.FindControl("fpRutaF"), FileUpload)
                If fpCargar.FileBytes.Length <= 0 Then
                    AlertaJS("Seleccionar archivo.")
                    Exit Sub
                End If

                For i = 0 To gvPip.Rows.Count - 1
                    If tCaja.Text = CType(gvPip.Rows(i).FindControl("tbFechaInterface"), TextBox).Text And ddlCombo.SelectedValue = CType(gvPip.Rows(i).FindControl("ddlInterface"), DropDownList).SelectedValue Then
                        AlertaJS("<p align=left>Verificar, el vector ya se encuentra en la grilla de carga:<br><br>- Fecha: " + tCaja.Text + "<br>- Interfaz: " + ddlCombo.SelectedItem.Text + "</p>")
                        Exit Sub
                    End If
                Next

                If Session("dtArchivosPip") IsNot Nothing Then
                    dtArchivosPip = Session("dtArchivosPip")
                Else
                    dtArchivosPip = Configurar()
                End If

                dtArchivosPip = AgregarFila(tCaja.Text, ddlCombo.SelectedItem.Text, ddlCombo.SelectedValue, fpCargar.FileName, fpCargar.FileBytes, Path.GetExtension(fpCargar.FileName))
                Session("dtArchivosPip") = dtArchivosPip
                If dtArchivosPip.Rows.Count > 0 Then btnProcesar.Enabled = True
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en agregar la fila/ " + ex.Message.ToString())
        Finally
            If Session("dtArchivosPip") IsNot Nothing Then
                dtArchivosPip = Session("dtArchivosPip")
            Else
                dtArchivosPip = Configurar()
                Session("dtArchivosPip") = Nothing
                Me.gvPip.DataSource = AgregarFila("-1", "-1", "-1", "-1", Nothing, "-1")
            End If

            Me.gvPip.DataSource = dtArchivosPip
            Me.gvPip.DataBind()
        End Try
    End Sub

    'OT10709. Creación del mensaje de alerta si existen registros con valores cero o NULL
    Private Function CrearMensaje(ByVal ds As DataSet) As StringBuilder
        CrearMensaje = Nothing
        Dim mensaje As New StringBuilder
        mensaje.Append("Existen datos incongruentes en la carga del vector precio forward, Verificar los siguientes registros del archivo que intenta cargar:")
        mensaje.Append("<table border=""1"" style=""width: 100%; text-align: left; font-weight: normal;"">")
        mensaje.Append("<tr><th>Forward Póliza</th><th>Fecha</th><th>Mtm Destino</th><th>Mtm Local</th></tr>")
        For Each dr As DataRow In ds.Tables(0).Rows
            mensaje.Append("<tr><td>" & dr("NumeroPoliza") & "</td><td>" & dr("Fecha") & "</td><td>" & dr("MtmUSD") & "</td><td>" & dr("MtmPEN") & "</td></tr>")
        Next
        mensaje.Append("</table>")
        CrearMensaje = mensaje
    End Function
    'OT10709 Fin

    Private Function loadVTC_SBS(ByVal fileName As String, ByVal fechaOperacion As Decimal) As String
        loadVTC_SBS = String.Empty
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oSheets As Excel.Sheets
        Dim oCells As Excel.Range
        Try
            oVectorTipoCambioBM.EliminarTipoCambioReal_SBS(fechaD, "SBS")

            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            oBooks = xlApp.Workbooks
            oBooks.Open(fileName)
            xlLibro = oBooks.Item(1)
            oSheets = xlLibro.Worksheets
            xlHoja = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = xlHoja.Cells

            Dim fila As Integer = 5

            'Cargar Moneda Dólar
            Dim oVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow
            oVectorTipoCambio = oVectorTipoCambioBE.VectorTipoCambio.NewVectorTipoCambioRow()
            oVectorTipoCambio.CodigoMoneda = "DOL"
            oVectorTipoCambio.EntidadExt = "SBS"
            oVectorTipoCambio.Fecha = fechaOperacion
            oVectorTipoCambio.Valor = CType(xlHoja.Range(String.Concat(LetraColumna(4), fila)).Value, Decimal)
            oVectorTipoCambio.ValorPrimario = CType(xlHoja.Range(String.Concat(LetraColumna(4), fila)).Value, Decimal)
            oVectorTipoCambio.Manual = "N"
            oVectorTipoCambioBM.Insertar(oVectorTipoCambio, MyBase.DatosRequest())

            'Cargar Moneda Nuevo Sol
            oVectorTipoCambio = oVectorTipoCambioBE.VectorTipoCambio.NewVectorTipoCambioRow()
            oVectorTipoCambio.CodigoMoneda = "NSOL"
            oVectorTipoCambio.EntidadExt = "SBS"
            oVectorTipoCambio.Fecha = fechaOperacion
            oVectorTipoCambio.Valor = 1
            oVectorTipoCambio.ValorPrimario = 1
            oVectorTipoCambio.Manual = "N"
            oVectorTipoCambioBM.Insertar(oVectorTipoCambio, MyBase.DatosRequest())

            oVectorTipoCambioBM.Actualiza_ValorCambioMoneda(fechaD, "SBS")
            oVectorTipoCambioBM.Copiar_TipoCambio(fechaD, "SBS")
        Catch ex As Exception
            loadVTC_SBS = "Hubo un error en la obtención de data del archivo/ " & ex.Message.ToString
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

    Private Function loadVP_SBS(ByVal fileName As String, ByVal fechaOperacion As Decimal) As String
        loadVP_SBS = String.Empty
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim xlLibro As Excel.Workbook
        Dim xlHoja As New Excel.Worksheet
        Dim oSheets As Excel.Sheets
        Dim oCells As Excel.Range
        Dim precioSucio As Decimal = 0D, precioLimpio As Decimal = 0D
        Try
            oVectorPrecioBM.BorrarVectorPrecio(fechaOperacion, "N", "SBS")
            Dim dtISIN_NEMONICOS As DataTable = oVectorPrecioBM.ListarValores_ISIN()

            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            oBooks = xlApp.Workbooks
            oBooks.Open(fileName)
            xlLibro = oBooks.Item(1)
            oSheets = xlLibro.Worksheets
            xlHoja = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = xlHoja.Cells

            Dim fila As Integer = 0
            Dim ultimaFila As Integer = oCells.Columns("A:A").Range("A1000").End(Excel.XlDirection.xlUp).Row()

            For fila = 3 To ultimaFila
                Dim foundRows() As DataRow
                foundRows = dtISIN_NEMONICOS.Select("codigoisin = '" & CType(xlHoja.Range(String.Concat(LetraColumna(2), fila)).Value, String) & "'")
                If foundRows.Length > 0 Then
                    Dim beVectorPrecio As VectorPrecioBE.VectorPrecioRow
                    'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
                    precioLimpio = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(9), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(9), fila)).Value, Decimal)) * _
                                   IIf(foundRows(0).ItemArray(3).ToString.Contains("100") Or foundRows(0).ItemArray(3).ToString.Contains("101"), 10, 1)
                    precioSucio = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(10), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(10), fila)).Value, Decimal)) * _
                                  IIf(foundRows(0).ItemArray(3).ToString.Contains("100") Or foundRows(0).ItemArray(3).ToString.Contains("101"), 10, 1)

                    beVectorPrecio = oVectorPrecioBE.VectorPrecio.NewVectorPrecioRow()
                    beVectorPrecio.CodigoMnemonico = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(1), fila)).Value, String) = String.Empty, "", CType(xlHoja.Range(String.Concat(LetraColumna(1), fila)).Value, String))
                    beVectorPrecio.Valor = precioLimpio
                    beVectorPrecio.Fecha = fechaOperacion
                    beVectorPrecio.EntidadExt = "SBS"
                    beVectorPrecio.Manual = "N"
                    beVectorPrecio.CodigoISIN = CType(xlHoja.Range(String.Concat(LetraColumna(2), fila)).Value, String)
                    beVectorPrecio.HoraCreacion = ""
                    beVectorPrecio.DescripcionEmisor = CType(xlHoja.Range(String.Concat(LetraColumna(3), fila)).Value, String)
                    beVectorPrecio.CodigoMoneda = CType(xlHoja.Range(String.Concat(LetraColumna(4), fila)).Value, String)
                    beVectorPrecio.TipoTasa = ""
                    beVectorPrecio.TasaCupon = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(14), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(14), fila)).Value, Decimal))
                    If IsDate(CType(xlHoja.Range(String.Concat(LetraColumna(13), fila)).Value, Date)) Then
                        beVectorPrecio.FechaVencimiento = CType(CType(xlHoja.Range(String.Concat(LetraColumna(13), fila)).Value, Date).ToString("yyyyMMdd"), Decimal)
                    End If
                    beVectorPrecio.ClasificacionRiesgo = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(17), fila)).Value, String) = String.Empty, "", CType(xlHoja.Range(String.Concat(LetraColumna(17), fila)).Value, String))
                    beVectorPrecio.ValorNominal = 0
                    beVectorPrecio.PorcPrecioLimpio = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(5), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(5), fila)).Value, Decimal))
                    beVectorPrecio.TIR = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(6), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(6), fila)).Value, Decimal))
                    beVectorPrecio.SobreTasa = 0
                    beVectorPrecio.PrecioLimpio = precioLimpio
                    beVectorPrecio.PrecioSucio = precioSucio
                    beVectorPrecio.IC = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(11), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(11), fila)).Value, Decimal))
                    beVectorPrecio.Duracion = IIf(CType(xlHoja.Range(String.Concat(LetraColumna(20), fila)).Value, String) = String.Empty, 0, CType(xlHoja.Range(String.Concat(LetraColumna(20), fila)).Value, Decimal))
                    Try
                        beVectorPrecio.PorcPrecioSucio = beVectorPrecio.PrecioSucio / beVectorPrecio.PrecioLimpio * beVectorPrecio.PorcPrecioLimpio
                    Catch ex As Exception
                        beVectorPrecio.PorcPrecioSucio = 0
                    End Try
                    beVectorPrecio.DetalleMoneda = ""
                    oVectorPrecioBM.Insertar(beVectorPrecio, MyBase.DatosRequest())
                End If
            Next
        Catch ex As Exception
            loadVP_SBS = "Hubo un error en la obtención de data del archivo/ " & ex.Message.ToString
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


End Class
