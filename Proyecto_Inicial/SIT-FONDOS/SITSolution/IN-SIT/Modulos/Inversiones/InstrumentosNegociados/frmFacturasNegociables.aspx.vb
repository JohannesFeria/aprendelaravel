Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmFacturasNegociables
    Inherits BasePage
    Dim bmPortafolio As New PortafolioBM
    Dim formatoFechaExcel1 As String = "ddMMMyyyy"
    Dim formatoFechaExcel2 As String = "dd/MM/yyyy"
    Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
    Dim oEntidadBM As New EntidadBM
    Private dtArchivoExcel As New DataTable
    Private oValidarFicheroBE As New ValidarFicheroBE
    Private strRutaEstado As String
    Private imgEstado As Image
    Private oValoresBM As New ValoresBM
    Private hdEstado As HiddenField, hdIndice As HiddenField, hdExisteOrden As HiddenField
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oTercerosBM As New TercerosBM
    Dim oTerceros As New TercerosBE
    Private listaRegistrosExcel As New List(Of ValidarFicheroBE)
    Private archivo As String = String.Empty, rutaDestino As String = String.Empty, msg As String = String.Empty, nombreArchivoExcel As String = String.Empty, strColumnas As String = String.Empty

#Region "Variables en Sesion"
    Public Property AccionFichero() As String
        Get
            If Session("AccionFichero") Is Nothing Then
                Return String.empty
            Else
                Return CType(Session("AccionFichero"), String)
            End If
        End Get
        Set(ByVal value As String)
            Session("AccionFichero") = value
        End Set
    End Property

    Public Property ArchivoNombre() As String
        Get
            If Session("ArchivoNombre") Is Nothing Then
                Return String.empty
            Else
                Return CType(Session("ArchivoNombre"), String)
            End If
        End Get
        Set(ByVal value As String)
            Session("ArchivoNombre") = value
        End Set
    End Property

    Public Property ValidarFicheroBESesion() As ValidarFicheroBE
        Get
            If Session("ValidarFicheroBE") Is Nothing Then
                Return New ValidarFicheroBE
            Else
                Return CType(Session("ValidarFicheroBE"), ValidarFicheroBE)
            End If
        End Get
        Set(ByVal value As ValidarFicheroBE)
            Session("ValidarFicheroBE") = value
        End Set
    End Property

    Public Property ListaValidarFicheroBESesion() As List(Of ValidarFicheroBE)
        Get
            If Session("ListaValidarFicheroBE") Is Nothing Then
                Return New List(Of ValidarFicheroBE)
            Else
                Return CType(Session("ListaValidarFicheroBE"), List(Of ValidarFicheroBE))
            End If
        End Get
        Set(ByVal value As List(Of ValidarFicheroBE))
            Session("ListaValidarFicheroBE") = value
        End Set
    End Property

    Public Property dtArchivoFacturacionSesion() As DataTable
        Get
            If Session("dtArchivoFacturacion") Is Nothing Then
                Return New DataTable
            Else
                Return CType(Session("dtArchivoFacturacion"), DataTable)
            End If
        End Get
        Set(ByVal value As DataTable)
            Session("dtArchivoFacturacion") = value
        End Set
    End Property


#End Region

#Region "Metodos"

    Private Function CargaTablaInicial() As DataTable
        Dim dtExcel As New DataTable

        dtExcel.Columns.Add(New DataColumn("Serie", GetType(System.String)))
        dtExcel.Columns.Add(New DataColumn("Precio", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("Moneda", GetType(System.String)))
        dtExcel.Columns.Add(New DataColumn("FeOpe", GetType(System.DateTime)))
        dtExcel.Columns.Add(New DataColumn("FeComp", GetType(System.DateTime)))
        dtExcel.Columns.Add(New DataColumn("Fecha Venc Fac", GetType(System.DateTime)))
        dtExcel.Columns.Add(New DataColumn("SAB", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("BVL", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("FGB", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("SMV", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("CAV", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("IGV", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("Tasa Fac", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("VN Fac", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("Monto Neto", GetType(System.Decimal)))
        dtExcel.Columns.Add(New DataColumn("Precio Fac", GetType(System.Decimal)))
        Return dtExcel
    End Function

    Private Sub CargarPortafolio(Optional ByVal refresh As Boolean = False)
        Dim oPortafolioBM As New PortafolioBM

        If ViewState("Portafolios") Is Nothing Or refresh Then
            ViewState("Portafolios") = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        End If

        HelpCombo.LlenarComboBox(Me.ddlFondo, ViewState("Portafolios"), "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub CargarDatosPortafolio(ByVal Idportafolio As String)

        Dim dtPortafolio As DataTable
        dtPortafolio = bmPortafolio.PortafolioSelectById(Idportafolio)

        If (dtPortafolio.Rows.Count > 0) Then
            Dim drPortafolio As DataRow
            drPortafolio = dtPortafolio.Rows.Item(0)
            Dim fechaNegocio As Object = drPortafolio.Item("FechaNegocio")
            If Not fechaNegocio Is Nothing And Not fechaNegocio Is DBNull.Value Then

                Dim dtFecha As DateTime = DateTime.ParseExact(CStr(fechaNegocio), "yyyyMMdd", Nothing)
                txtFechaNegociacion.Text = dtFecha.ToString(Constantes.Pantalla.FormatoFecha.ddMMyyyy_01)
                ViewState("FechaApertura") = txtFechaNegociacion.Text

            Else
                txtFechaNegociacion.Text = String.Empty
            End If
        Else
            txtFechaNegociacion.Text = String.Empty
        End If

    End Sub

    Private Sub ImportarArchivoFileServer()
        Dim rutaArchivo As String = txtRutaFileServer.Text

    End Sub

    Private Function guardarArchivo(ByVal archivo() As Byte, ByVal nombre As String) As Boolean
        Try
            File.WriteAllBytes(rutaDestino & "\" & nombre, archivo)
            Return File.Exists(rutaDestino & "\" & nombre)



        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function crearArchivoExcel(ByVal archivo() As Byte, ByVal nombre As String) As String
        Try

            Dim fechaCadena As String = Date.Now.ToString("yyyyMMdd_Hmmss")

            Dim archivoRutaFinal As String = rutaDestino & "\" & nombre & "_" & ++Date.Now.Hour + Date.Now.Minute + Date.Now.Second
            File.WriteAllBytes(rutaDestino & "\" & nombre, archivo)

            'File.WriteAllBytes(archivoRutaFinal, archivo)

            Dim ObjCom As UIUtility.COMObjectAplication = Nothing
            Dim xlApp As Excel.Application
            Dim oBooks As Excel.Workbooks
            Dim oOrdenInversion As New OrdenPreOrdenInversionBM
            Dim oOperacion As New OperacionBM
            Dim dtOperacioneEPU As New DataTable

            Dim newFileName As String = "EXCEL_FACTURACION_" & fechaCadena & ".xls"

            If File.Exists(rutaDestino & "\" & nombre) Then

                Try
                    ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
                    xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
                    oBooks = xlApp.Workbooks
                    oBooks.OpenText(rutaDestino & "\" & nombre, DataType:=Excel.XlTextParsingType.xlDelimited, Tab:=True)
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
                    File.Delete(rutaDestino & "\" & nombre)

                    'oValidarFicheroBE = ValidarFicheroBESesion
                    'oValidarFicheroBE.NombreArchivo = newFileName
                    'ValidarFicheroBESesion = oValidarFicheroBE
                Catch ex As Exception
                    Return String.Empty
                End Try

            End If


            Return newFileName
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function getValidarFicheroBE() As ValidarFicheroBE
        If Session("ValidarFicheroBE") IsNot Nothing Then
            oValidarFicheroBE = Session("ValidarFicheroBE")
        Else
            oValidarFicheroBE = New ValidarFicheroBE
        End If
        Return oValidarFicheroBE
    End Function

    Private Function cargarRuta() As String


        msg = String.Empty
        rutaDestino = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        If (Not Directory.Exists(rutaDestino)) Then
            msg = "No existe la ruta destino."
        End If

        Return msg
    End Function

    Private Function actualizarContadores(ByVal dt As DataTable) As List(Of String)
        Dim listaEstados = (From dr In dt
                           Select dr("Estado")
                           ).ToList()





        lblTotal.Text = listaEstados.Count


        lblExitoso.Text = listaEstados.FindAll(Function(p) p = "3").Count
        lblAdvertencia.Text = listaEstados.FindAll(Function(p) p = "2").Count
        lblNoValidos.Text = listaEstados.FindAll(Function(p) p = "1").Count

    End Function

    Private Sub ImportarArchivoCarga()
        Dim strRutaArchivoFinal As String = String.Empty

        If fpRutaF.HasFile Then
            Dim archivo As Byte() = fpRutaF.FileBytes
            Dim nombreArchivo As String = fpRutaF.FileName
            Dim vColeccion() As String = nombreArchivo.Split(".")

            Dim nombreHoja = vColeccion(0)
            ArchivoNombre = nombreArchivo

            Dim nombreArchivoFinal As String = nombreHoja + "_" + Date.Now.ToString("yyyyMMdd_Hmmss") + "." + vColeccion(1)

            msg = cargarRuta()


            If msg <> String.Empty Then
                AlertaJS(msg)
                Return
            End If

            Try

                nombreArchivoExcel = crearArchivoExcel(archivo, nombreArchivoFinal)

                If nombreArchivoExcel <> String.Empty Then

                    dtArchivoExcel = dtArchivoFacturacionSesion
                    msg = loadExcel(rutaDestino & "\" & nombreArchivoExcel, "DATOS", "A2:AC100", dtArchivoExcel)

                    If File.Exists(rutaDestino & "\" & nombreArchivoExcel) Then
                        File.Delete(rutaDestino & "\" & nombreArchivoExcel)
                    End If

                    If msg <> String.Empty Then
                        Throw New System.Exception("Error Estructura")
                        Return
                    End If

                    If dtArchivoExcel.Rows.Count > 0 Then
                        dtArchivoExcel.Rows.Remove(dtArchivoExcel.Rows(0))
                    End If


                    'msg = loadArchivoPlano(rutaDestino & "\" & nombreArchivo, "POLIZA_AUX_TEXTO", "A2:AE100", dtArchivoExcel)
                    ' dtArchivoFacturacionSesion = dtArchivoExcel
                    dtArchivoFacturacionSesion = AgregarColumnaEstado(dtArchivoExcel)
                    AccionFichero = "IMPORTAR"
                    gvExcel.DataSource = dtArchivoFacturacionSesion
                    gvExcel.DataBind()

                    '  actualizarContadores(dtArchivoFacturacionSesion)
                    Dim listaEstados = (From dr In dtArchivoFacturacionSesion
                         Select dr("Estado")
                         ).ToList()

                    lblTotal.Text = dtArchivoFacturacionSesion.Rows.Count
                    lblAdvertencia.Text = "0"
                    lblExitoso.Text = "0"
                    lblNoValidos.Text = "0"
                    FormatoGrilla()



                End If

            Catch ex As Exception
                dtArchivoFacturacionSesion = CargaTablaInicial()
                gvExcel.DataSource = dtArchivoFacturacionSesion
                gvExcel.DataBind()
                actualizarContadores(dtArchivoFacturacionSesion)

                AlertaJS(String.Format("El archivo {0} no cuenta con la estructura de un archivo de POLIZA", nombreArchivo))
            End Try





        Else
            AlertaJS("No se ha seleccion ningún archivo")
        End If


    End Sub

    Public Shared Function ConvertToDate(ByVal dateString As String, ByRef result As DateTime) As Boolean
        Try

            'Here is the date format you desire to use
            Dim supportedFormats() As String = New String() {"dd/MM/yyyy"}

            'Now it will be converted to what the machine supports
            result = DateTime.ParseExact(dateString, supportedFormats, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ConvertirFecha1(ByVal dateString As String, ByRef result As String) As Boolean
        Try

            'Now it will be converted to what the machine supports
            result = DateTime.ParseExact(dateString, "ddMMMyyyy", CultureInfo.CreateSpecificCulture("es-US")).ToString(Constantes.Pantalla.FormatoFecha.ddMMyyyy_01)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ConvertirFecha2(ByVal dateString As String, ByRef result As String) As Boolean
        Try

            'Here is the date format you desire to use
            'Dim supportedFormats() As String = New String() {formato}

            'Now it will be converted to what the machine supports
            result = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-US")).ToString(Constantes.Pantalla.FormatoFecha.ddMMyyyy_01)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ConvertirFecha3(ByVal dateString As String, ByRef result As String) As Boolean
        Try

            'Here is the date format you desire to use
            'Dim supportedFormats() As String = New String() {formato}

            'Now it will be converted to what the machine supports
            result = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-US")).ToString(Constantes.Pantalla.FormatoFecha.ddMMyyyy_01)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function obtenerIndiceColumna_Grilla(ByVal nomCol As String, ByVal grilla As GridView) As Integer
        Dim columna As DataControlFieldCollection = grilla.Columns
        Dim indiceCol As Integer = -1
        For Each celda As DataControlField In columna
            If TypeOf celda Is System.Web.UI.WebControls.BoundField Then
                If CType(celda, BoundField).DataField = nomCol Then
                    indiceCol = columna.IndexOf(celda)
                    Exit For
                End If
            End If
        Next
        Return indiceCol
    End Function

    Private Sub FormatoGrilla()
        Dim i As Integer
        'Dim dtFecha As DateTime
        For i = 0 To gvExcel.Rows.Count - 1

            If CType(gvExcel.Rows(i).Cells(3).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                Dim FechaMostrar As String = String.Empty
                ConvertirFecha1(gvExcel.Rows(i).Cells(3).Text, FechaMostrar)
                If (FechaMostrar = String.Empty) Then
                    ConvertirFecha2(gvExcel.Rows(i).Cells(3).Text, FechaMostrar)
                End If
                gvExcel.Rows(i).Cells(3).Text = FechaMostrar
            End If
            If CType(gvExcel.Rows(i).Cells(4).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                Dim FechaMostrar As String = String.Empty
                ConvertirFecha1(gvExcel.Rows(i).Cells(4).Text, FechaMostrar)
                If (FechaMostrar = String.Empty) Then
                    ConvertirFecha2(gvExcel.Rows(i).Cells(4).Text, FechaMostrar)
                End If
                gvExcel.Rows(i).Cells(4).Text = FechaMostrar
            End If
            If CType(gvExcel.Rows(i).Cells(5).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                Dim FechaMostrar As String = String.Empty
                ConvertirFecha1(gvExcel.Rows(i).Cells(5).Text, FechaMostrar)
                If (FechaMostrar = String.Empty) Then
                    ConvertirFecha2(gvExcel.Rows(i).Cells(5).Text, FechaMostrar)
                End If
                gvExcel.Rows(i).Cells(5).Text = FechaMostrar
            End If


            If CType(gvExcel.Rows(i).Cells(6).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(6).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(6).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If
            If CType(gvExcel.Rows(i).Cells(7).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(7).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(7).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If
            If CType(gvExcel.Rows(i).Cells(8).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(8).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(8).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If
            If CType(gvExcel.Rows(i).Cells(9).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(9).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(9).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If
            If CType(gvExcel.Rows(i).Cells(10).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(11).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(10).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If
            If CType(gvExcel.Rows(i).Cells(11).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(11).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(11).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If
            If CType(gvExcel.Rows(i).Cells(12).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(12).Text = Format(Convert.ToDecimal(CType(gvExcel.Rows(i).Cells(12).Text, String).Replace(UIUtility.DecimalSeparator, ".")), "##0.0000000")
            End If

            If CType(gvExcel.Rows(i).Cells(13).Text, String).Replace("&nbsp;", "") <> String.Empty Then
                gvExcel.Rows(i).Cells(13).Text = CType(gvExcel.Rows(i).Cells(13).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            End If
            'gvExcel.Rows(i).Cells(2).Text = CType(gvExcel.Rows(i).Cells(2).Text, String).Replace(UIUtility.DecimalSeparator, ".")

        Next
    End Sub


    Private Function loadExcel(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dtResult As DataTable) As String
        Try
            If System.IO.File.Exists(sFileName) Then
                Using cn As New OleDbConnection
                    Dim sCs As String = String.Empty
                    If System.Configuration.ConfigurationManager.AppSettings("Ambiente").ToString() = "Desarrollo" Then
                        sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Excel 8.0;"
                    Else
                        sCs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"
                    End If
                    '  sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Excel 8.0;"

                    cn.ConnectionString = sCs
                    Dim sql As String = String.Empty
                    Dim campos As New StringBuilder

                    campos.Append("[Serie],")
                    campos.Append("[Cantidad],")
                    campos.Append("[Precio],")
                    campos.Append("[Monto Bruto],")
                    campos.Append("[%COM],")
                    campos.Append("[SAB],")
                    campos.Append("[BVL],")
                    campos.Append("[FGB],")
                    campos.Append("[SMV],")
                    campos.Append("[CAV],")
                    campos.Append("[IGV],")
                    campos.Append("[OTR],")
                    campos.Append("[Monto Neto],")
                    campos.Append("[Moneda],")
                    campos.Append("[Modalidad],")
                    campos.Append("[OFC],")
                    campos.Append("[Tasa Fac],")
                    campos.Append("[Precio Fac],")
                    campos.Append("FORMAT([FeComp], 'dd/MM/yyyy') as [FeComp], ")
                    campos.Append("FORMAT([FeOpe], 'dd/MM/yyyy') as [FeOpe], ")
                    campos.Append("FORMAT([Fecha Venc Fac], 'dd/MM/yyyy') as [Fecha Venc Fac], ")
                    campos.Append("[VN Fac]")

                    If sRange IsNot Nothing Then
                        sql = "select " & campos.ToString() & " from " & "[" & sSheetName & "$" & sRange & "]"
                    Else
                        sql = "select * from " & "[" & sSheetName & "$]"
                    End If

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

    Private Sub ProcesarArchivo()

    End Sub

    Private Function loadArchivoPlano(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dtResult As DataTable) As String
        Try
            If System.IO.File.Exists(sFileName) Then
                Using cn As New OleDbConnection
                    Dim sCs As String = String.Empty

                    sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Text;HDR=Yes;FMT=Delimited;Format=TabDelimited;"

                    cn.ConnectionString = sCs
                    Dim sql As String = String.Empty

                    sql = "select * from " & sSheetName

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

    Private Function obtenerCodigoMoneda(ByVal pCodigoMonedaISO As String) As String
        Dim resultado As String
        resultado = IIf(pCodigoMonedaISO = "PEN", "NSOL", "DOL")
        Return resultado

    End Function

    Private Function obtenerCodigoTercero(ByVal pSerie As String) As String
        Dim resultado As String

        Try
            Dim serie = pSerie.Split("-")
            Return serie(2)
        Catch ex As Exception
            Return resultado
        End Try


    End Function

    Private Function ValidarExistenciaTercero(ByVal pNumeroRuc As String) As String
        oTerceros = oTercerosBM.Seleccionar(pNumeroRuc, DatosRequest)
        Dim dt As DataTable
        dt = oEntidadBM.SeleccionarPorCodigoTercero(pNumeroRuc, DatosRequest).Tables(0)

        Return dt.Rows(0)(0)

    End Function

    Private Sub ValidarEstructuraArchivo()


        If ddlFondo.SelectedIndex <= 0 Then
            AlertaJS("Debe seleccionar un portafolio")
            Return

        End If

        Dim fechaAperturaFondo As Decimal

        Dim dtPortafolio As DataTable
        dtPortafolio = bmPortafolio.PortafolioSelectById(ddlFondo.SelectedValue)

        If (dtPortafolio.Rows.Count > 0) Then
            Dim drPortafolio As DataRow
            drPortafolio = dtPortafolio.Rows.Item(0)
            Dim fechaNegocio As Object = drPortafolio.Item("FechaNegocio")

            If Not fechaNegocio Is Nothing And Not fechaNegocio Is DBNull.Value Then
                   fechaAperturaFondo = fechaNegocio
            End If
        Else
            AlertaJS("El portafolio no cuenta con una fecha de negocio")
            Return
        End If


        Dim oMonedaBM As New MonedaBM
        Dim oMoneda As New MonedaBE
        Dim oMonedaDolares As New MonedaBE
        Dim oMonedaSoles As New MonedaBE
        Dim listaRegistrosExcel As New List(Of ValidarFicheroBE)

        ListaValidarFicheroBESesion = listaRegistrosExcel
        dtArchivoExcel = dtArchivoFacturacionSesion


        If dtArchivoExcel.Rows.Count > 0 Then
            For i As Integer = 0 To dtArchivoExcel.Rows.Count - 1

                Dim registroExcel As New ValidarFicheroBE
                Dim entPreOrden As New PrevOrdenInversionBE
                Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(entPreOrden.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                Dim fila As DataRow = dtArchivoExcel.Rows(i)
                Dim columnaNombre As String = String.Empty
                registroExcel.Indice = i
                Try
                    fila("Indice") = i
                    columnaNombre = "nombre"
                    If fila("Moneda") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Moneda")
                    Else
                        oRow.Moneda = obtenerCodigoMoneda(fila("Moneda"))
                        registroExcel.CodigoMoneda = oRow.Moneda
                    End If

                    columnaNombre = "Serie"
                    If fila("Serie") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Serie")
                    Else
                        registroExcel.CodigoFacturacion = fila("Serie").ToString()
                        oRow.CodigoTercero = obtenerCodigoTercero(registroExcel.CodigoFacturacion)

                        If oRow.CodigoTercero = String.Empty Then
                            registroExcel.CodigosValidaciones.Add(Constantes.ValidarExcel.FORMATO_CODIGO_SERIE)

                        Else
                            registroExcel.CodigoEntidad = ValidarExistenciaTercero(oRow.CodigoTercero)

                        End If

                    End If

                    columnaNombre = "Precio"
                    If fila("Precio Fac") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Precio Fac")
                    Else
                        oRow.Precio = fila("Precio Fac")
                    End If

                    columnaNombre = "Monto Nominal"
                    If fila("VN Fac") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("VN Fac")
                    Else
                        oRow.MontoNominal = fila("VN Fac")
                        oRow.Cantidad = oRow.MontoNominal
                        registroExcel.Cantidad = oRow.MontoNominal

                    End If

                    columnaNombre = "Monto Neto"
                    If fila("Monto Neto") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Monto Neto")
                    Else
                        registroExcel.MontoNeto = fila("Monto Neto")
                    End If

                    columnaNombre = "Monto Bruto"
                    If fila("Monto Bruto") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Monto Bruto")
                    Else
                        oRow.CantidadOperacion = fila("Monto Bruto")
                        oRow.MontoOperacion = fila("Monto Bruto")
                    End If

                    columnaNombre = "Tasa"
                    If fila("Tasa Fac") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Tasa Fac")
                    Else
                        oRow.Tasa = fila("Tasa Fac")
                        registroExcel.Tasa = oRow.Tasa
                    End If


                    columnaNombre = "Fecha Operación"
                    If fila("FeOpe") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("FeOpe")
                    Else
                        Dim fechaCadena = ObtenerFechaExcel(fila("FeOpe").ToString())
                        If fechaCadena <> String.Empty Then
                            oRow.FechaOperacion = Convert.ToDecimal(fechaCadena)

                            If (fechaAperturaFondo <> oRow.FechaOperacion) Then

                            End If

                            registroExcel.FechaEmision = oRow.FechaOperacion

                        Else
                            registroExcel.CamposValidar.Add("FeOpe")
                        End If


                    End If

                    columnaNombre = "Fecha Liquidación"
                    If fila("FeComp") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("FeComp")
                    Else
                        Dim fechaCadena = ObtenerFechaExcel(fila("FeComp").ToString())
                        If fechaCadena <> String.Empty Then
                            oRow.FechaLiquidacion = Convert.ToDecimal(fechaCadena)
                        Else
                            registroExcel.CamposValidar.Add("FeComp")
                        End If

                    End If

                    columnaNombre = "Fecha Vencimiento"
                    If fila("Fecha Venc Fac") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("Fecha Venc Fac")
                    Else

                        Dim fechaCadena = ObtenerFechaExcel(fila("Fecha Venc Fac").ToString())
                        If fechaCadena <> String.Empty Then
                            registroExcel.FechaVencimiento = Convert.ToDecimal(fechaCadena)
                        Else
                            registroExcel.CamposValidar.Add("Fecha Venc Fac")
                        End If


                    End If

                    registroExcel.ComisionValor = New ComisionValor

                    columnaNombre = "SAB"
                    If fila("SAB") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("SAB")
                    Else
                        registroExcel.ComisionValor.PCOMIS_SAB = fila("SAB")
                    End If

                    columnaNombre = "BVL"
                    If fila("BVL") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("BVL")
                    Else
                        registroExcel.ComisionValor.PCUOTA_BVL = fila("BVL")
                    End If

                    columnaNombre = "FGB"
                    If fila("FGB") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("FGB")
                    Else
                        registroExcel.ComisionValor.PFND_GARNTIA = fila("FGB")
                    End If

                    columnaNombre = "SMV"
                    If fila("SMV") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("SMV")
                    Else
                        registroExcel.ComisionValor.PCONT_CONASE = fila("SMV")
                    End If


                    If fila("CAV") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("CAV")
                    Else
                        registroExcel.ComisionValor.PCUOT_CAVLI = fila("CAV")
                    End If

                    If fila("IGV") Is DBNull.Value Then
                        registroExcel.CamposValidar.Add("IGV")
                    Else
                        registroExcel.ComisionValor.PIGV_TOT = fila("IGV")
                    End If


                    columnaNombre = String.Empty

                    'Valores por Defecto  
                    oRow.CodigoOperacion = "1"
                    oRow.MedioNegociacion = "E" 'Por defecto 'ELECTRONICO'
                    oRow.TipoFondo = "Normal" 'Por defecto 'NORMAL'
                    oRow.TipoTramo = "AGENCIA" 'Por defecto 'AGENCIA'
                    oRow.TipoCondicion = "AM" 'Por defecto 'A MERCADO'
                    oRow.Porcentaje = "N" 'N: No Porcentaje, solo Monto directo
                    oRow.Fixing = 0 'Por defecto 
                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                    oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                    oRow.IndPrecioTasa = "T"
                    oRow.TipoTasa = "2"
                    oRow.CodigoPlaza = "7" 'Por defecto 7:'LIMA' ---- ddlPlaza.SelectedValue
                    oRow.InteresCorrido = 0
                    oRow.TipoValorizacion = "VAL_RAZO"
                    oRow.Porcentaje = "S"
                Catch ex As Exception
                    ' registroExcel.CamposValidar.Add("Disp")
                    If columnaNombre <> String.Empty Then
                        registroExcel.CamposValidar.Add(columnaNombre)
                    End If


                End Try



                If registroExcel.CamposValidar.Count > 0 Then
                    registroExcel.ObjetoValido = False
                    'objeto con error (X)
                    fila("Estado") = "1"
                Else
                    If registroExcel.CodigoEntidad = String.Empty Then
                        registroExcel.CodigosValidaciones.Add(Constantes.ValidarExcel.NO_EXISTE_TERCERO)
                    End If
                    If ValidarExisteOrden(registroExcel.CodigoFacturacion) = True Then
                        registroExcel.CodigosValidaciones.Add(Constantes.ValidarExcel.EXISTE_ORDEN_BD)
                    End If

                    If ValidarFechaOperacion(oRow.FechaOperacion, fechaAperturaFondo) = False Then
                        registroExcel.CodigosValidaciones.Add(Constantes.ValidarExcel.FECHA_OPERACION_NO_VALIDO)
                    End If

                    If ExisteValoracion(fechaAperturaFondo) = True Then
                        registroExcel.CodigosValidaciones.Add(Constantes.ValidarExcel.EXISTE_VALORIZACION)
                    End If

                    If listaRegistrosExcel.FindAll(Function(p) p.CodigoFacturacion.Trim().Equals(registroExcel.CodigoFacturacion.Trim())).Count > 0 Then
                        registroExcel.CodigosValidaciones.Add(Constantes.ValidarExcel.EXISTE_ORDEN_EXCEL)
                    End If

                    If registroExcel.CodigosValidaciones.Count = 0 Then
                        'objeto valido (check)
                        fila("Estado") = "3"
                        registroExcel.ObjetoValido = True
                        'registroExcel.CodigoPortafolio = ddlFondo.SelectedValue
                        oRow.CodigoPortafolioSBS = registroExcel.CodigoPortafolio
                    Else
                        'validaciones (advertencia)
                        fila("Estado") = "2"
                    End If

                End If



                If registroExcel.ObjetoValido = False Then
                   If registroExcel.CamposValidar.Count > 0 Then
                        registroExcel.TipoError = "COLUMNA"
                    End If
                End If



                'Valores  del excel              
                entPreOrden.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                entPreOrden.PrevOrdenInversion.AcceptChanges()

                registroExcel.PreOrdenInversion = entPreOrden
                listaRegistrosExcel.Add(registroExcel)
            Next


            ListaValidarFicheroBESesion = listaRegistrosExcel

            Dim valorSesion As String = AccionFichero

            'Actualizar el datagrid
            dtArchivoExcel.DefaultView.Sort = "Estado DESC"
            dtArchivoFacturacionSesion = dtArchivoExcel
            valorSesion = "VALIDAR"
            AccionFichero = valorSesion
            gvExcel.DataSource = dtArchivoExcel
            gvExcel.DataBind()
            actualizarContadores(dtArchivoExcel)
            FormatoGrilla()

        Else
            AlertaJS("No existe ningun archivo cargado.")
        End If







    End Sub

    Private Function ObtenerFechaExcel(ByVal pFechaExcel As String) As String
        Try

            Dim fechaCadena As String = String.Empty
            Dim resultado As String = String.Empty
            ConvertirFecha1(pFechaExcel, fechaCadena)
            If (fechaCadena = String.Empty) Then
                ConvertirFecha2(pFechaExcel, fechaCadena)
            End If

            If (fechaCadena = String.Empty) Then
                ConvertirFecha3(pFechaExcel, fechaCadena)
            End If

            If (fechaCadena <> String.Empty) Then
                resultado = Convert.ToDateTime(fechaCadena).ToString("yyyyMMdd")
                'DateTime.ParseExact(resultado, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-US")).ToString("yyyyMMdd")
            End If

            'Dim fechaDate As DateTime = DateTime.ParseExact(pFechaExcel, "dd-MMM-yy", CultureInfo.CreateSpecificCulture("es-US"))
            Return resultado
        Catch ex As Exception
            Return ""
        End Try
        Return ""
    End Function

    Private Function ObtenerImagenEstado(ByVal hdInput As HiddenField, ByVal hdInputExisteOrden As HiddenField) As String
        strRutaEstado = String.Empty

        If hdInput.Value.Length > 0 Then
            If hdInput.Value = "3" Then
                strRutaEstado = "~\App_Themes\img\icons\check.png"
            ElseIf hdInput.Value = "2" Then
                strRutaEstado = "~\App_Themes\img\icons\advertencia.png"
            ElseIf hdInput.Value = "1" Then
                strRutaEstado = "~\App_Themes\img\icons\glyphicons_bin.png"
            End If
        End If

        Return strRutaEstado
    End Function

    Private Function ObtenerColumnasObservadas(ByVal hdInput As HiddenField) As String
        strColumnas = String.Empty

        listaRegistrosExcel = ListaValidarFicheroBESesion
        Dim oFilaRevisar = listaRegistrosExcel.Find(Function(p) p.Indice = hdInput.Value)
        strColumnas = String.Join(",", oFilaRevisar.CamposValidar)
        Return strColumnas
    End Function

    Private Function ObtenerObservaciones(ByVal hdInputIndice As HiddenField, ByVal hdInputEstado As HiddenField) As String
        strColumnas = String.Empty
        If ListaValidarFicheroBESesion.Count > 0 Then
            Dim oFilaRevisar = ListaValidarFicheroBESesion.Find(Function(p) p.Indice = hdInputIndice.Value)
            If "2".Equals(hdInputEstado.Value) Then
                'Adventencia. 
                strColumnas = String.Join(",", oFilaRevisar.CodigosValidaciones)

            Else
                'Columnas a corregir
                strColumnas = String.Join(",", oFilaRevisar.CamposValidar)

            End If
        End If
        Return strColumnas
    End Function

    Private Function ObtenerComentarioxCodigoValidacion(ByVal pCodigoValidacion)
        Dim Resultado As String = String.Empty

        If pCodigoValidacion.Equals(Constantes.ValidarExcel.CODIGO_EXISTE_ORDEN_BD) Then
            Return Constantes.ValidarExcel.EXISTE_ORDEN_BD
        End If

        If pCodigoValidacion.Equals(Constantes.ValidarExcel.CODIGO_EXISTE_ORDEN_EXCEL) Then
            Return Constantes.ValidarExcel.EXISTE_ORDEN_EXCEL
        End If

        If pCodigoValidacion.Equals(Constantes.ValidarExcel.CODIGO_NO_EXISTE_TERCERO) Then
            Return Constantes.ValidarExcel.NO_EXISTE_TERCERO
        End If

        Return Resultado
    End Function

    Private Function ActualizarEstadoExcel(dtExcel As DataTable, listaFichero As List(Of ValidarFicheroBE)) As DataTable



        Return dtExcel
    End Function

    Private Function AgregarColumnaEstado(dtExcel As DataTable) As DataTable

        If dtExcel.Columns.Count >= 0 Then
            'dtExcel.Columns.Add(New DataColumn("Estado", GetType(System.String)))
            Dim column As DataColumn
            column = New DataColumn
            With column
                .DataType = System.Type.GetType("System.String")
                .DefaultValue = "0"
                .Unique = False
                .ColumnName = "Estado"
            End With
            dtExcel.Columns.Add(column)
            dtExcel.Columns.Add(New DataColumn("Comentario", GetType(System.String)))
            'dtExcel.Columns.Add(New DataColumn("Validacion", GetType(System.String)))
            dtExcel.Columns.Add(New DataColumn("Indice", GetType(System.Int32)))
        End If
        Return dtExcel
    End Function

    Private Function ValidarExisteOrden(ByVal pCodigoFactura As String) As Boolean

        If String.IsNullOrEmpty(pCodigoFactura) = False Then
            'Revisar si existe la factura
            Dim oValorBE = oValoresBM.SeleccionarPorCodigoFactura(pCodigoFactura, String.Empty, DatosRequest)
            If oValorBE.Valor.Rows.Count > 0 Then
                Return True
            End If

        End If

        Return False
    End Function

    Private Function ValidarFechaOperacion(ByVal pFechaOperacion As Decimal, ByVal pFechaApertura As Decimal) As Boolean

        If pFechaOperacion = pFechaApertura Then
            Return True
        End If

        Return False
    End Function

    Private Function ExisteValoracion(ByVal pFechaApertura As Decimal) As Boolean

        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, pFechaApertura)
        If cantidadreg > 0 Then
            Return True
        End If

        Return False
    End Function




#End Region

#Region "Eventos de la pagina"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load


        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Try
            If Not IsPostBack Then
                Session("dtArchivoFacturacion") = Nothing
                Session("ListaRegistrosExcel") = Nothing
                Session("ListaValidarFicheroBE") = Nothing
                Session("ValidarFicheroBE") = Nothing
                Session("AccionFichero") = Nothing
                CargarPortafolio()
                Me.dtArchivoFacturacionSesion = CargaTablaInicial()
                gvExcel.DataSource = dtArchivoFacturacionSesion
                gvExcel.DataBind()
            Else
                If Not ViewState("FechaApertura") Is Nothing Then
                    txtFechaNegociacion.Text = ViewState("FechaApertura")
                End If

            End If


        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub


    Protected Sub gvExcel_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvExcel.PageIndexChanging

        gvExcel.PageIndex = e.NewPageIndex
        dtArchivoExcel = dtArchivoFacturacionSesion
        gvExcel.DataSource = dtArchivoExcel
        gvExcel.DataBind()
    End Sub

    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click

        If AccionFichero = String.Empty Then
            AlertaJS("No existe ningun archivo cargado.")

        ElseIf AccionFichero = "PROCESAR" Then
            AlertaJS("No existe ningun archivo cargado.")

        ElseIf AccionFichero = "IMPORTAR" Then
            AlertaJS("Se debe validar las operaciones antes de importarlas.")

        Else

            listaRegistrosExcel = ListaValidarFicheroBESesion
            Dim listaRegistrosValidos = listaRegistrosExcel.FindAll(Function(p) p.ObjetoValido = True)

            Dim valor As String = Constantes.ParametrosGenerales.RUTA_TEMP

            If listaRegistrosValidos.Count > 0 Then
                oPrevOrdenInversionBM.ProcesarFacturasNegociable(listaRegistrosValidos, ParametrosSIT.TR_RENTA_FIJA.ToString(), ddlFondo.SelectedValue, DatosRequest)
                dtArchivoExcel = dtArchivoFacturacionSesion
                dtArchivoExcel.Clear()
                'AccionFichero = "PROCESAR"
                AccionFichero = String.Empty
                dtArchivoFacturacionSesion = dtArchivoExcel
                gvExcel.DataSource = dtArchivoFacturacionSesion
                gvExcel.DataBind()
                AlertaJS("Se importó " + listaRegistrosValidos.Count.ToString() + " operaciones exitósamente.")
                actualizarContadores(dtArchivoExcel)
            Else
                AlertaJS("No existe algun registro Exitoso para procesar. Corregir las observaciones")
            End If

        End If



    End Sub

    Protected Sub ddlFondo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        Try
            If Not Me.ddlFondo.SelectedItem.Value = String.Empty Then
                CargarDatosPortafolio(Me.ddlFondo.SelectedItem.Value)
            Else
                txtFechaNegociacion.Text = String.Empty
                ViewState("Portafolios") = String.Empty
            End If





        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la fecha del portafolio: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub btnImportar_Click(sender As Object, e As System.EventArgs) Handles btnImportar.Click
        Try

            If rbtnFileServer.Checked Then
                ImportarArchivoFileServer()
            ElseIf rbtnWeb.Checked Then
                ImportarArchivoCarga()
            End If


        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la carga: " & Replace(ex.Message, "'", ""))
        End Try

    End Sub

    Protected Sub btnValidar_Click(sender As Object, e As System.EventArgs) Handles btnValidar.Click
        ValidarEstructuraArchivo()
    End Sub

    Protected Sub gvExcel_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExcel.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            hdEstado = CType(e.Row.FindControl("hdEstado"), HiddenField)
            hdIndice = CType(e.Row.FindControl("hdIndice"), HiddenField)
            hdExisteOrden = CType(e.Row.FindControl("hdExisteOrden"), HiddenField)

            imgEstado = CType(e.Row.FindControl("imgEstado"), Image)

            If hdEstado.Value.Trim().Equals("0") Then
                imgEstado.Visible = False
            End If

            imgEstado.ImageUrl = ObtenerImagenEstado(hdEstado, hdExisteOrden)
            If AccionFichero <> "IMPORTAR" And (hdEstado.Value.Trim().Equals("2") Or hdEstado.Value.Trim().Equals("1")) Then
                imgEstado.CssClass = "selector"
                Dim titulo As String = IIf(hdEstado.Value.Trim().Equals("2"), "No se puede procesar", "Columnas por corregir")
                imgEstado.Style.Add("cursor", "pointer")
                imgEstado.Attributes.Add("onclick", "javascript:agregarFilasModalColumnas('" & ObtenerObservaciones(hdIndice, hdEstado) & "','" + titulo + "');")

            End If
        End If


    End Sub

#End Region

End Class