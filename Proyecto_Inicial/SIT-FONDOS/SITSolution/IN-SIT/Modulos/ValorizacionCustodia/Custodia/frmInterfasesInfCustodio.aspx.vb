Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System
Imports System.IO
Imports ParametrosSIT
Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmInterfasesInfCustodio
    Inherits BasePage
#Region "Variables"
    Dim oCustodioArchivoBE As New CustodioArchivoBE
    Dim oCustodioArchivoBM As New CustodioArchivoBM
    '------------------------------------------------------------
    Dim objCustodio As New CustodioBM
    Dim objPortafolioDirectorio As New PortafolioDirectorioBM
    '------------------------------------------------------------
    Dim objArchivoPlanoBM As New ArchivoPlanoBM
    Dim objArchivoPlanoBE As New DataSet
    '------------------------------------------------------------
    Dim objArchivoPlanoEstructuraBM As New ArchivoPlanoEstructuraBM
    Dim objArchivoPlanoEstructuraBE As New DataSet
    '------------------------------------------------------------
    Dim sArchivoCodigo As String, sArchivoNombre As String
    Dim sArchivoDescripcion As String, sArchivoExtension As String
    Dim sArchivoUbicacion As String, sArchivoTipoSeparador As String
    Dim sArchivoSeparador As String, nArchivoLongitudRegistro As Decimal
    Dim sArchivoSeparadorDecimales As String
    Dim iArchivoFechaCortePosicion As Integer
    Dim iArchivoFechaCorteLongitud As Integer
    '------------------------------------------------------------
    Dim sArchivo As String
    Dim sFechaCorte As String

    Dim oUtil As New UtilDM
    Dim oUIUtil As New UIUtility

#End Region
#Region " /* Métodos de la Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Not Page.IsPostBack Then
                'DescargaGrillaLista()
                tbFechaInformacion.Text = oUtil.RetornarFechaSistema
                tbFechaRegistro.Text = oUtil.RetornarFechaSistema
                CargaCustodios()
                CargaPortafolio(ddlPortafolio)
                LimpiarTablaCarga()
                CargarGrilla()
                tbFechaInformacion.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
                btnImportar.Attributes.Add("onclick", "javascript:return Confirmacion();")
            Else
                ViewState("vsFechaInformacion") = tbFechaInformacion.Text
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub
    Private Sub ddlReporte_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlReporte.SelectedIndexChanged
        Try
            ddlPortafolio.SelectedIndex = 0
            Call RecuperaCustodioDirectorio()
            CreaRuta()

            'OT 10883 20/10/2017 Hanz Cocchi: Se agrega validación para deshabilitar el combo de Portafolios cuando el Custodio sea BBH. Solicitado MMARALLANO
            If ddlReporte.SelectedValue = "BBH" Then
                ddlPortafolio.Enabled = False
            Else
                ddlPortafolio.Enabled = True
            End If
            'OT 10883 FIN
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Custodio")
        End Try
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Call RecuperaPortafolioDirectorio()
            CreaRuta()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Portafolio")
        End Try
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            If Validar() Then
                Dim NomFile As String = iptRuta.Value
                RecuperaCustodioDirectorio()
                RecuperaPortafolioDirectorio()
                CreaRuta()      'OT 10883 20/10/2017 Hanz Cocchi
                iptRuta.PostedFile.SaveAs(Myfile.Value & "\" & NomFile)
                Dim dt As DataTable = Nothing
                'OT 10883 20/10/2017 Hanz Cocchi: Se agrega condición para llamar al nuevo proceso CargarArchivoBBH para que no interfiera con la lógica de CargarArchivo
                Select Case ddlReporte.SelectedValue
                    Case "CAVALI"
                        dt = CargarArchivo(NomFile)  'OT 10883: Linea de código original
                    Case "BBH"
                        dt = CargarArchivoBBH(NomFile)
                End Select
                'OT 10883 FIN
                CargarGrilla()
                If dt IsNot Nothing Then
                    If dt.Rows.Count > 0 Then
                        Dim mensaje As String = MensajeCuentasDepositariasFaltantes(dt)
                        AlertaJS(mensaje)
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Dim sFechaCorte As String
            sFechaCorte = tbFechaInformacion.Text
            sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2)
            If Validar() Then
                If dgArchivo.Rows.Count <= 0 Then
                    AlertaJS(ObtenerMensaje("ALERT61"))
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
            Call ImportarInformacion()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oUtil = Nothing
            oUIUtil = Nothing
            oCustodioArchivoBE.Dispose()
            oCustodioArchivoBM = Nothing
            objCustodio = Nothing
            objPortafolioDirectorio = Nothing
            objArchivoPlanoBE.Dispose()
            objArchivoPlanoEstructuraBM = Nothing
            objArchivoPlanoEstructuraBE.Dispose()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgArchivo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgArchivo.PageIndexChanging
        Try
            ActualizarIndice(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
#End Region
    Sub CreaRuta()
        'OT 10883 18/10/2017 Hanz Cocchi: Se agrega validación a CAVALI, el código dentro de la condicional ya existía previamente
        If ddlReporte.SelectedValue = "CAVALI" Then
            If Not (ddlReporte.SelectedValue = "" And ddlPortafolio.SelectedValue = "" And tbFechaInformacion.Text = "") Then
                If Not File.Exists(Myfile.Value) Then
                    System.IO.Directory.CreateDirectory(Myfile.Value)
                End If
            End If
        End If
        'OT 10883 18/10/2017 Hanz Cocchi: Se agrega validación para crear directorio cuando Custodio es BBH ya que no se selecciona el Portafolio, no se conoce hasta que el archivo sea cargado
        If ddlReporte.SelectedValue = "BBH" Then
            If Not (ddlReporte.SelectedValue = "" And tbFechaInformacion.Text = "") Then
                If Not File.Exists(Myfile.Value) Then
                    System.IO.Directory.CreateDirectory(Myfile.Value)
                End If
            End If
        End If
        'OT 10883 FIN
    End Sub
#Region " /* Funciones Personalizadas*/"
    Private Function Validar() As Boolean
        If tbFechaInformacion.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT55"))
            Return False
            Exit Function
        ElseIf ddlReporte.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT56"))
            Return False
            Exit Function
        ElseIf (ddlPortafolio.SelectedValue.Trim = "") And ddlReporte.SelectedValue.ToUpper.Trim = "CAVALI" Then    'OT 10883 18/10/2017 Hanz Cocchi: La validación inicial era <> "CAVALI", se cambia para exigir al custodio CAVALI que seleccione un portafolio pero no a BBH ya que no requiere
            AlertaJS(ObtenerMensaje("ALERT57"))
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub LimpiaControles()
        Call LimpiarTablaCarga()
        Call CargarGrilla()
        tbFechaRegistro.Text = oUtil.RetornarFechaSistema
        tbFechaInformacion.Text = oUtil.RetornarFechaSistema
        Myfile.Value = ""
    End Sub
    Private Sub LimpiarTablaCarga()
        Dim objInformacionCustodio As New CustodioArchivoBM
        Call objInformacionCustodio.Eliminar(DatosRequest)
        dgArchivo.DataSource = Nothing
    End Sub
    Private Sub CargarGrilla()
        Dim oInfCusDS As New DataTable
        Dim objInformacionCustodio As New CustodioArchivoBM
        oInfCusDS = objInformacionCustodio.Listar(DatosRequest).Tables(0)
        dgArchivo.DataSource = oInfCusDS
        dgArchivo.DataBind()
    End Sub
    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
    End Sub
    Private Sub CargaCustodios()
        ddlReporte.DataTextField = "Descripcion"
        ddlReporte.DataValueField = "CodigoCustodio"
        ddlReporte.DataSource = objCustodio.Listar(DatosRequest)
        ddlReporte.DataBind()
        UIUtility.InsertarElementoSeleccion(ddlReporte)
    End Sub
    Private Function CargarArchivo(ByVal NomFile As String) As DataTable
        CargarArchivo = Nothing
        Dim fichero As String = Myfile.Value & "\" & NomFile
        Dim sFechaCorte As String
        Dim file_num As Integer = FreeFile()
        Dim Linea As String
        Dim iFila As Integer
        Dim iColumnaOrden As Decimal, sColumnaNombre As String, sColumnaDescripcion As String
        Dim iColumnaPosicionInicial As Decimal, iColumnaLongitud As Decimal, sColumnaTipoDato As String
        Dim codtitular As String, nomtitular As String, isin As String, descripcion As String, tipointermediario As String,
        codIntermediario As String, DescripcionIntermediario As String, SaldoContable As String, saldoDisponible As String,
        SaldoOtros As String
        Dim sCustodioArchivo(-1) As String
        Dim dtCarga As DataTable = Nothing
        objArchivoPlanoEstructuraBE = objArchivoPlanoEstructuraBM.Listar(ViewState("vsArchivoCodigo"), DatosRequest)
        FileOpen(file_num, fichero, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
        Do While Not EOF(file_num)
            Linea = LineInput(file_num)
            If Linea.Trim <> "" Then
                For iFila = 0 To objArchivoPlanoEstructuraBE.Tables(0).Rows.Count - 1
                    iColumnaOrden = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaOrden")
                    sColumnaNombre = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaNombre")
                    sColumnaDescripcion = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaDescripcion")
                    iColumnaPosicionInicial = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaPosicionInicial")
                    iColumnaLongitud = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaLongitud")
                    sColumnaTipoDato = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaTipoDato")
                    Select Case iColumnaOrden
                        Case 1
                            If iColumnaLongitud <> 0 Then
                                codtitular = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                codtitular = "***"
                            End If
                        Case 2
                            If iColumnaLongitud <> 0 Then
                                nomtitular = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                nomtitular = "***"
                            End If
                        Case 3
                            If iColumnaLongitud <> 0 Then
                                isin = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                isin = "***"
                            End If
                        Case 4
                            If iColumnaLongitud <> 0 Then
                                descripcion = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                descripcion = "***"
                            End If
                        Case 5
                            If iColumnaLongitud <> 0 Then
                                tipointermediario = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                tipointermediario = "***"
                            End If
                        Case 6
                            If iColumnaLongitud <> 0 Then
                                codIntermediario = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                codIntermediario = "***"
                            End If
                        Case 7
                            If iColumnaLongitud <> 0 Then
                                DescripcionIntermediario = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                DescripcionIntermediario = "***"
                            End If
                        Case 8
                            If iColumnaLongitud <> 0 Then
                                SaldoContable = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                SaldoContable = "***"
                            End If
                        Case 9
                            If iColumnaLongitud <> 0 Then
                                saldoDisponible = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                saldoDisponible = "***"
                            End If
                        Case 10
                            If iColumnaLongitud <> 0 Then
                                SaldoOtros = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                SaldoOtros = "***"
                            End If
                    End Select
                Next
                ReDim Preserve sCustodioArchivo(UBound(sCustodioArchivo) + 1)
                sCustodioArchivo(UBound(sCustodioArchivo)) = codtitular & "@@" & nomtitular & "@@" & isin & "@@" & descripcion & "@@" & tipointermediario & "@@" & _
                codIntermediario & "@@" & DescripcionIntermediario & "@@" & SaldoContable & "@@" & saldoDisponible & "@@" & SaldoOtros & "@*@"

                'sCustodioArchivo = sCustodioArchivo & codtitular & "@@" & nomtitular & "@@" & isin & "@@" & descripcion & "@@" & tipointermediario & "@@" & _
                'codIntermediario & "@@" & DescripcionIntermediario & "@@" & SaldoContable & "@@" & saldoDisponible & "@@" & SaldoOtros & "@*@"

                sFechaCorte = tbFechaInformacion.Text
                sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2) & " 00:00:00"
            End If
        Loop

        'Dividir el total de la información en archivos de tamaño de 40 registros. Esto se hace porque la programación en la base de datos
        'solo soporta información de hasta 8000 caracteres.
        Dim custodioCargaInfo(-1) As String
        If sCustodioArchivo.Length > 0 Then
            Dim numeroArchivo As Integer = DividirArchivo(sCustodioArchivo.Length, Constantes.NUMERO_CARGA_REGISTROS_CUSTODIO)
            Dim numeroRegistroFaltantes As Integer = 0
            Dim contadorRegistros As Integer = 0
            Dim contador As Integer = 0
            For j = 1 To numeroArchivo
                numeroRegistroFaltantes = sCustodioArchivo.Length - (j * Constantes.NUMERO_CARGA_REGISTROS_CUSTODIO)
                ReDim Preserve custodioCargaInfo(UBound(custodioCargaInfo) + 1)
                If numeroRegistroFaltantes >= 0 Then
                    For i As Integer = contadorRegistros To (j * Constantes.NUMERO_CARGA_REGISTROS_CUSTODIO) - 1
                        custodioCargaInfo(UBound(custodioCargaInfo)) += sCustodioArchivo(i)
                        contador += 1
                    Next
                    contadorRegistros = contador
                Else
                    For i As Integer = contadorRegistros To sCustodioArchivo.Length - 1
                        custodioCargaInfo(UBound(custodioCargaInfo)) += sCustodioArchivo(i)
                        contador += 1
                    Next
                End If
            Next
            'Ingresar la inforamción a la base de datos
            If custodioCargaInfo.Length > 0 Then
                dtCarga = TableRequest(dtCarga)
                Dim dt As DataTable = Nothing
                For Each stringItem As String In custodioCargaInfo
                    dt = oCustodioArchivoBM.CargarArchivo(ddlReporte.SelectedValue.ToString, stringItem, ddlPortafolio.SelectedValue, sFechaCorte, DatosRequest)
                    If dt.Rows.Count > 0 Then
                        For Each dtRow As DataRow In dt.Rows
                            dtCarga.Rows.Add(dtRow("Id"), dtRow("CodigoCustodio"), dtRow("CuentaDepositaria"))
                        Next
                    End If
                Next
            End If
        End If
        FileClose(file_num)
        CargarArchivo = dtCarga
    End Function
    ''' <summary>
    ''' Carga los datos del archivo de texto a la tabla temporal de forma similar al método CargarArchivo pero orientado solo al Custodio BBH 
    ''' </summary>
    ''' <param name="NomFile">Nombre del archivo seleccionado en la página</param>
    ''' <remarks></remarks>
    Private Function CargarArchivoBBH(ByVal NomFile As String) As DataTable
        'OT 10883 18/10/2017 Hanz Cocchi
        'Se agrega esta función para realizar la carga del archivo para custodio BBH
        CargarArchivoBBH = Nothing
        Dim fichero As String = Myfile.Value & "\" & NomFile
        Dim sFechaCorte As String
        Dim file_num As Integer = FreeFile()
        Dim Linea As String
        Dim iFila As Integer
        Dim iColumnaOrden As Decimal, sColumnaNombre As String, sColumnaDescripcion As String
        Dim iColumnaPosicionInicial As Decimal, iColumnaLongitud As Decimal, sColumnaTipoDato As String
        Dim codtitular As String, nomtitular As String, isin As String, descripcion As String, tipointermediario As String,
        codIntermediario As String, DescripcionIntermediario As String, SaldoContable As String, saldoDisponible As String,
        SaldoOtros As String, cuentaDepositaria As String
        Dim sCustodioArchivo(-1) As String
        Dim dtCarga As DataTable = Nothing
        objArchivoPlanoEstructuraBE = objArchivoPlanoEstructuraBM.Listar(ViewState("vsArchivoCodigo"), DatosRequest)
        FileOpen(file_num, fichero, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
        Do While Not EOF(file_num)
            Linea = LineInput(file_num)
            If Linea.Trim <> "" Then
                For iFila = 0 To objArchivoPlanoEstructuraBE.Tables(0).Rows.Count - 1
                    iColumnaOrden = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaOrden")
                    sColumnaNombre = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaNombre")
                    sColumnaDescripcion = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaDescripcion")
                    iColumnaPosicionInicial = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaPosicionInicial")
                    iColumnaLongitud = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaLongitud")
                    sColumnaTipoDato = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaTipoDato")

                    codtitular = "***"
                    nomtitular = "***"
                    DescripcionIntermediario = "***"

                    Select Case iColumnaOrden
                        Case 1
                            If iColumnaLongitud <> 0 Then
                                tipointermediario = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                tipointermediario = "***"
                            End If
                        Case 2
                            If iColumnaLongitud <> 0 Then
                                codIntermediario = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                codIntermediario = "***"
                            End If
                        Case 3
                            If iColumnaLongitud <> 0 Then
                                isin = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud)).Replace("-", "")
                            Else
                                isin = "***"
                            End If
                        Case 4
                            If iColumnaLongitud <> 0 Then
                                descripcion = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                descripcion = "***"
                            End If
                        Case 5
                            If iColumnaLongitud <> 0 Then
                                cuentaDepositaria = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                cuentaDepositaria = "***"
                            End If
                        Case 8
                            If iColumnaLongitud <> 0 Then
                                SaldoContable = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                SaldoContable = "***"
                            End If
                        Case 9
                            If iColumnaLongitud <> 0 Then
                                saldoDisponible = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                saldoDisponible = "***"
                            End If
                        Case 10
                            If iColumnaLongitud <> 0 Then
                                SaldoOtros = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            Else
                                SaldoOtros = "***"
                            End If
                    End Select
                Next
                ReDim Preserve sCustodioArchivo(UBound(sCustodioArchivo) + 1)
                sCustodioArchivo(UBound(sCustodioArchivo)) = codtitular & "@@" & nomtitular & "@@" & isin & "@@" & descripcion & "@@" & tipointermediario & "@@" & _
                codIntermediario & "@@" & DescripcionIntermediario & "@@" & SaldoContable & "@@" & saldoDisponible & "@@" & SaldoOtros & "@@" & cuentaDepositaria & "@*@"

                'sCustodioArchivo = sCustodioArchivo & codtitular & "@@" & nomtitular & "@@" & isin & "@@" & descripcion & "@@" & tipointermediario & "@@" & _
                'codIntermediario & "@@" & DescripcionIntermediario & "@@" & SaldoContable & "@@" & saldoDisponible & "@@" & SaldoOtros & "@@" & cuentaDepositaria & "@*@"

                sFechaCorte = tbFechaInformacion.Text
                sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2) & " 00:00:00"
            End If
        Loop

        'Dividir el total de la información en archivos de tamaño de 40 registros. Esto se hace porque la programación en la base de datos
        'solo soporta información de hasta 8000 caracteres.
        Dim custodioCargaInfo(-1) As String
        If sCustodioArchivo.Length > 0 Then
            Dim numeroArchivo As Integer = DividirArchivo(sCustodioArchivo.Length, Constantes.NUMERO_CARGA_REGISTROS_CUSTODIO)
            Dim numeroRegistroFaltantes As Integer = 0
            Dim contadorRegistros As Integer = 0
            Dim contador As Integer = 0
            For j = 1 To numeroArchivo
                numeroRegistroFaltantes = sCustodioArchivo.Length - (j * Constantes.NUMERO_CARGA_REGISTROS_CUSTODIO)
                ReDim Preserve custodioCargaInfo(UBound(custodioCargaInfo) + 1)
                If numeroRegistroFaltantes >= 0 Then
                    For i As Integer = contadorRegistros To (j * Constantes.NUMERO_CARGA_REGISTROS_CUSTODIO) - 1
                        custodioCargaInfo(UBound(custodioCargaInfo)) += sCustodioArchivo(i)
                        contador += 1
                    Next
                    contadorRegistros = contador
                Else
                    For i As Integer = contadorRegistros To sCustodioArchivo.Length - 1
                        custodioCargaInfo(UBound(custodioCargaInfo)) += sCustodioArchivo(i)
                        contador += 1
                    Next
                End If
            Next

            'Ingresar la inforamción a la base de datos
            If custodioCargaInfo.Length > 0 Then
                dtCarga = TableRequest(dtCarga)
                Dim dt As DataTable = Nothing
                For Each stringItem As String In custodioCargaInfo
                    dt = oCustodioArchivoBM.CargarArchivo(ddlReporte.SelectedValue.ToString, stringItem, ddlPortafolio.SelectedValue, sFechaCorte, DatosRequest)
                    If dt.Rows.Count > 0 Then
                        For Each dtRow As DataRow In dt.Rows
                            dtCarga.Rows.Add(dtRow("Id"), dtRow("CodigoCustodio"), dtRow("CuentaDepositaria"))
                        Next
                    End If
                Next
            End If
        End If
        FileClose(file_num)
        'OT 10883 FIN
        'Return ds
        CargarArchivoBBH = dtCarga
    End Function
    Private Function crearObjetoOI(ByVal CodigoCustodio As String, ByVal CuentaDepositariaCustodio As String, _
                                  ByVal CodigoPortafolio As String, ByVal CodigoISIN As String, _
                                  ByVal DescripcionTitulo As String, ByVal TipoIntermediario As String, _
                                  ByVal CodigoIntermediario As String, ByVal ValorNominal As String, _
                                  ByVal SaldoContable As String, ByVal SaldoDisponible As String, _
                                  ByVal Dato1 As String, ByVal Diferencia As String, _
                                  ByVal UsuarioCreacion As String, ByVal FechaCreacion As String, _
                                  ByVal HoraCreacion As String) As CustodioArchivoBE
        Dim oCustodioArchivoBE As New CustodioArchivoBE
        Dim oRow As CustodioArchivoBE.CustodioArchivoRow
        oRow = CType(oCustodioArchivoBE.CustodioArchivo.NewRow(), CustodioArchivoBE.CustodioArchivoRow)
        oRow.CodigoCustodio() = CodigoCustodio
        oRow.CuentaDepositariaCustodio() = CuentaDepositariaCustodio
        oRow.CodigoPortafolio() = CodigoPortafolio
        oRow.CodigoISIN() = CodigoISIN
        oRow.DescripcionTitulo() = DescripcionTitulo
        oRow.TipoIntermediario() = TipoIntermediario
        oRow.CodigoIntermediario() = CodigoIntermediario
        oRow.ValorNominal() = ValorNominal
        oRow.SaldoContable() = SaldoContable
        oRow.SaldoDisponible() = SaldoDisponible
        oRow.Dato1() = Dato1
        oRow.Diferencia() = Diferencia
        oRow.UsuarioCreacion() = UsuarioCreacion
        oRow.FechaCreacion() = FechaCreacion
        oRow.HoraCreacion() = HoraCreacion
        oCustodioArchivoBE.CustodioArchivo.AddCustodioArchivoRow(oRow)
        oCustodioArchivoBE.CustodioArchivo.AcceptChanges()
        Return oCustodioArchivoBE
    End Function
    Private Sub RecuperaCustodioDirectorio()
        Dim scodigoCustodio As String = ddlReporte.SelectedValue.ToString
        Call LimpiaControles()
        tbFechaInformacion.Text = ViewState("vsFechaInformacion")
        Dim oDatCustodio As DataSet = objCustodio.SeleccionarCustodioArchivoPlano(scodigoCustodio, DatosRequest)
        If oDatCustodio.Tables(0).Rows.Count <= 0 Then
            AlertaJS("No esta definido un tipo de archivo y ruta para la carga de Información de este custodio.")
            Exit Sub
        End If
        sArchivoCodigo = oDatCustodio.Tables(0).Rows(0).Item("ArchivoCodigo")
        objArchivoPlanoBE = objArchivoPlanoBM.Seleccionar(sArchivoCodigo, DatosRequest)
        sArchivoNombre = objArchivoPlanoBE.Tables(0).Rows(0).Item("ArchivoNombre")
        sArchivoDescripcion = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoDescripcion")
        sArchivoExtension = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoExtension")
        sArchivoUbicacion = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoUbicacion")
        sArchivoTipoSeparador = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoTipoSeparador")
        sArchivoSeparador = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoSeparador")
        nArchivoLongitudRegistro = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoLongitudRegistro")
        sArchivoSeparadorDecimales = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoSeparadorDecimales")
        iArchivoFechaCortePosicion = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoFechaCortePosicion")
        iArchivoFechaCorteLongitud = objArchivoPlanoBE.Tables(0).Rows(0)("ArchivoFechaCorteLongitud")
        ViewState("vsArchivoCodigo") = sArchivoCodigo
        ViewState("vsArchivoNombre") = sArchivoNombre
        ViewState("vsArchivoDescripcion") = sArchivoDescripcion
        ViewState("vsArchivoExtension") = sArchivoExtension
        ViewState("vsArchivoUbicacion") = sArchivoUbicacion
        ViewState("vsArchivoTipoSeparador") = sArchivoTipoSeparador
        ViewState("vsArchivoSeparador") = sArchivoSeparador
        ViewState("vsArchivoLongitudRegistro") = nArchivoLongitudRegistro
        ViewState("vsArchivoSeparadorDecimales") = sArchivoSeparadorDecimales
        ViewState("vsArchivoFechaCortePosicion") = iArchivoFechaCortePosicion
        ViewState("vsArchivoFechaCorteLongitud") = iArchivoFechaCorteLongitud
        Me.Myfile.Value = sArchivoUbicacion
    End Sub
    Private Sub RecuperaPortafolioDirectorio()
        Dim sPortafolioCodigo As String = ddlPortafolio.SelectedValue.ToString
        If ddlReporte.SelectedValue.ToUpper.Trim = "CAVALI" Then
            If sPortafolioCodigo.Trim = "" Then sPortafolioCodigo = "X"
        End If
        Dim sPortafolioDirectorio As String = String.Empty
        Dim oDatPortafolioDirectorio As DataSet = objPortafolioDirectorio.Seleccionar(sPortafolioCodigo, DatosRequest)
        Dim sAnioMes As String = tbFechaInformacion.Text
        Call LimpiaControles()
        tbFechaInformacion.Text = ViewState("vsFechaInformacion")
        If ddlReporte.SelectedValue.ToUpper.Trim = "CAVALI" Then
            If oDatPortafolioDirectorio.Tables(0).Rows.Count <= 0 Then
                AlertaJS("No esta definido un Directorio de trabajo para este portafolio, Consulte con el area de sistemas.")
                Exit Sub
            End If
            sPortafolioDirectorio = oDatPortafolioDirectorio.Tables(0).Rows(0).Item("PortafolioDirectorio")
        End If
        Me.Myfile.Value = ViewState("vsArchivoUbicacion") & sPortafolioDirectorio & "\" & sAnioMes.Substring(6, 4) & sAnioMes.Substring(3, 2)
    End Sub
    Private Sub ImportarInformacion()
        Dim sFechaCorte As String
        sFechaCorte = tbFechaInformacion.Text
        sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2)
        Dim bImportarInfCustodio As Boolean = oCustodioArchivoBM.ImportarArchivo(ddlReporte.SelectedValue.ToString, ddlPortafolio.SelectedValue.ToString, CLng(sFechaCorte), 1, DatosRequest)
        If bImportarInfCustodio Then
            AlertaJS(ObtenerMensaje("ALERT59"))
        Else
            AlertaJS(ObtenerMensaje("ALERT101"))
        End If
    End Sub
    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgArchivo.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub
    Private Function MensajeCuentasDepositariasFaltantes(ByVal p_Dt As DataTable) As String
        Dim mensaje As New StringBuilder
        mensaje.Append("Las siguientes cuentas depositarias no están registrados en el sistema " & _
                       "y no se cargará su información. De acuerdo al detalle, en el archivo de carga identificar el portafolio " & _
                       "al que corresponde y registrar la información en la opción Administración de Portafolios")
        mensaje.Append("<table border=""1"" style=""width: 100%; text-align: left; font-weight: normal;"">")
        mensaje.Append("<tr><th>Custodio</th><th>Cuenta depositaria</th></tr>")
        For Each dr As DataRow In p_Dt.Rows
            mensaje.Append("<tr><td>" & dr("CodigoCustodio") & "</td><td>" & dr("CuentaDepositaria") & "</td></tr>")
        Next
        mensaje.Append("</table>")
        Return mensaje.ToString()
    End Function
    Private Function DividirArchivo(ByVal sizeFile As Integer, ByVal divisor As Integer) As Integer
        Dim parteEntera As Integer = sizeFile / divisor
        Dim parteDecimal As Decimal = (sizeFile / divisor) - parteEntera
        If parteDecimal > 0 Then
            parteEntera = parteEntera + 1
        End If
        DividirArchivo = parteEntera
    End Function
    Private Function TableRequest(ByRef dt As DataTable) As DataTable
        dt = New DataTable
        dt.Columns.Add("Id", GetType(Integer))
        dt.Columns.Add("CodigoCustodio", GetType(String))
        dt.Columns.Add("CuentaDepositaria", GetType(String))
        TableRequest = dt
    End Function
#End Region
End Class