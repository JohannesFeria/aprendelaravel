Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System
Imports System.IO

Partial Class Modulos_ValorizacionCustodia_Custodia_frmInterfasesInfoDivRebLib
    Inherits BasePage

#Region "Variables"
    'TablaIntermedia--------------------------------------------
    Dim objEntidadExtArchivoPlano As New EntidadExternaArchivoPlanoBM
    'Combos------------------------------------------------------
    Dim objParametrosGenerales As New ParametrosGeneralesBM
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
    Dim sFechaInformacion As String

    Dim oUtil As New UtilDM
    Dim oUIUtil As New UIUtility

#End Region

#Region "/* Métodos de la Página*/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                DescargaGrillaLista()
                tbFechaRegistro.Text = oUtil.RetornarFechaSistema
                tbFechaInformacion.Text = oUtil.RetornarFechaSistema
                CargaEntidadExterna()
                LimpiarTablaCarga()
                CargarGrilla()
                btnImportar.Attributes.Add("onclick", "javascript:return Confirmacion();")
            Else
                ViewState("vsFechaInformacion") = tbFechaInformacion.Text
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try       
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            If Validar() Then
                Call CargarListaArchivos()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try        
    End Sub

    Private Sub ddlEntidadExterna_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlEntidadExterna.SelectedIndexChanged
        Try
            Dim sEntidadExternaCodigo As String = ddlEntidadExterna.SelectedValue.ToString
            ViewState("vsEntidadExternaCodigo") = ddlEntidadExterna.SelectedValue.ToString
            Dim oDatEntidadExtArchivoPlano As DataSet = objEntidadExtArchivoPlano.Seleccionar(sEntidadExternaCodigo, DatosRequest)
            Call LimpiaControles()
            tbFechaInformacion.Text = ViewState("vsFechaInformacion")
            If oDatEntidadExtArchivoPlano.Tables(0).Rows.Count <= 0 Then
                AlertaJS("No esta definido un tipo de archivo y ruta para la carga de Información de este custodio.")
                Exit Sub
            End If
            sArchivoCodigo = oDatEntidadExtArchivoPlano.Tables(0).Rows(0).Item("ArchivoCodigo")

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

            Myfile.Text = sArchivoUbicacion
            Dim sAnioMes As String = tbFechaInformacion.Text
            Myfile.Text = ViewState("vsArchivoUbicacion") & "\" & sAnioMes.Substring(6, 4) & sAnioMes.Substring(3, 2)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Dim sFechaInformacion As String
            sFechaInformacion = tbFechaInformacion.Text
            sFechaInformacion = sFechaInformacion.Substring(6, 4) & sFechaInformacion.Substring(3, 2) & sFechaInformacion.Substring(0, 2)

            If Validar() Then
                If dgDirtectorioContenido.Rows.Count <= 0 Then
                    AlertaJS(ObtenerMensaje("ALERT62"))
                    Exit Sub
                Else
                    If dgDirtectorioContenido.Rows(0).Cells(1).Text = "&nbsp;" Then
                        AlertaJS(ObtenerMensaje("ALERT62"))
                        Exit Sub
                    Else
                        If dgArchivo.Rows.Count <= 0 Then
                            AlertaJS(ObtenerMensaje("ALERT61"))
                            Exit Sub
                        End If
                    End If
                End If
            Else
                Exit Sub
            End If

            Dim bImportarInfDivLib As Boolean = objEntidadExtArchivoPlano.ImportarArchivo(ddlEntidadExterna.SelectedValue.ToString, CLng(sFechaInformacion), 1, DatosRequest)
            If bImportarInfDivLib Then
                AlertaJS(ObtenerMensaje("ALERT22"))
                Call LimpiaControles()
                ddlEntidadExterna.SelectedIndex = 0
            Else
                AlertaJS(ObtenerMensaje("ALERT104"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try        
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oUtil = Nothing
            oUIUtil = Nothing
            objEntidadExtArchivoPlano = Nothing
            objParametrosGenerales = Nothing
            objArchivoPlanoBM = Nothing
            objArchivoPlanoBE = Nothing
            objArchivoPlanoEstructuraBM = Nothing
            objArchivoPlanoEstructuraBE = Nothing
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try        
    End Sub

    Protected Sub dgArchivo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgArchivo.PageIndexChanging
        Try
            dgArchivo.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Protected Sub dgDirtectorioContenido_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDirtectorioContenido.RowCommand
        Try
            If e.CommandName = "VerContenido" Then
                Dim index As Integer = CInt(e.CommandArgument)
                sArchivo = dgDirtectorioContenido.Rows(index).Cells(1).Text()
                ViewState("sArchivo") = sArchivo
                sFechaInformacion = sArchivo.Substring(ViewState("vsArchivoFechaCortePosicion"), ViewState("vsArchivoFechaCorteLongitud"))

                Select Case ViewState("vsEntidadExternaCodigo")
                    Case "1"   'Bloomberg
                        tbFechaInformacion.Text = sFechaInformacion.Substring(0, 2).ToString & "/" & sFechaInformacion.Substring(2, 2).ToString & "/" & sFechaInformacion.Substring(4, 4).ToString
                        Call LimpiarTablaCarga()
                        Call CargarArchivoIndicadorBloomberg()
                        Call CargarGrilla()
                    Case "2"   'BVL
                        tbFechaInformacion.Text = sFechaInformacion.Substring(0, 2).ToString & "/" & sFechaInformacion.Substring(2, 2).ToString & "/20" & sFechaInformacion.Substring(4, 2).ToString
                        LimpiarTablaCarga()
                        CargarArchivo()
                        CargarGrilla()
                    Case "3"   'SBS
                End Select

                If VerificaPreCarga() Then
                    txtValidacion.Value = "0"
                Else
                    txtValidacion.Value = "1"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Function Validar() As Boolean
        If tbFechaInformacion.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT55"))
            Return False
            Exit Function
        ElseIf ddlEntidadExterna.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT60"))
            Return False
            Exit Function
        End If
        Return True
    End Function

    Private Sub CargaEntidadExterna()
        ddlEntidadExterna.DataTextField = "Nombre"
        ddlEntidadExterna.DataValueField = "Valor"
        ddlEntidadExterna.DataSource = objParametrosGenerales.ListarEntidadesExternas("EntidadExt", DatosRequest)
        ddlEntidadExterna.DataBind()
        UIUtility.InsertarElementoSeleccion(ddlEntidadExterna)
    End Sub

    Private Sub LimpiaControles()
        Call LimpiarTablaCarga()
        Call CargarGrilla()
        Call DescargaGrillaLista()
        tbFechaRegistro.Text = oUtil.RetornarFechaSistema
        tbFechaInformacion.Text = oUtil.RetornarFechaSistema
        Myfile.Text = ""
    End Sub

    Private Sub LimpiarTablaCarga()
        Dim objEntidadExtInformacion As New EntidadExternaArchivoPlanoBM
        Call objEntidadExtInformacion.Eliminar(DatosRequest)
        dgArchivo.DataSource = Nothing
    End Sub

    Private Sub DescargaGrillaLista()
        Dim dtTabla As New DataTable

        dgDirtectorioContenido.DataSource = Nothing
        dtTabla.Columns.Add("Nombre Archivo")
        dtTabla.Columns.Add("Tamaño")
        dtTabla.Columns.Add("Fecha")
        dtTabla.Columns.Add("Hora")

        Dim row As DataRow = dtTabla.NewRow()
        dtTabla.Rows.Add(row)

        dgDirtectorioContenido.DataSource = dtTabla
        dgDirtectorioContenido.DataBind()
        dgDirtectorioContenido.Columns(0).Visible = False
    End Sub

    Private Sub CargarGrilla()
        Dim objInformacionEntidad As New EntidadExternaArchivoPlanoBM
        Dim oDTInfEnt As DataTable = objInformacionEntidad.Listar(ddlEntidadExterna.SelectedValue.ToString, DatosRequest).Tables(0)

        dgArchivo.DataSource = oDTInfEnt
        dgArchivo.DataBind()
    End Sub

    Private Sub CargarArchivo()
        'Dim fichero As String = Myfile.Text
        Dim fichero As String = Myfile.Text & "\" & sArchivo
        Dim sFechaInformacion As String
        ' Get an available file number.
        Dim file_num As Integer = FreeFile()

        Dim Linea As String
        Dim IcontLinea As Integer

        Dim strCodigoOI As String
        Dim iFila As Integer

        Dim iColumnaOrden As Decimal, sColumnaNombre As String, sColumnaDescripcion As String
        Dim iColumnaPosicionInicial As Decimal, iColumnaLongitud As Decimal, sColumnaTipoDato As String

        Dim sCodigoMnemonico As String, sFactor As String, sConceptoEjercicio As String
        Dim sFechaAcuerdo As String, sFechaCorte As String, sFechaRegistro As String
        Dim sFechaEntrega As String

        Dim sEntidadArchivo As String

        objArchivoPlanoEstructuraBE = objArchivoPlanoEstructuraBM.Listar(viewstate("vsArchivoCodigo"), DatosRequest)

        FileOpen(file_num, fichero, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)

        IcontLinea = 1
        ' Read the file's lines.
        Do While Not EOF(file_num)
            ' Read a line.
            Linea = LineInput(file_num)
            IcontLinea = IcontLinea + 1

            For iFila = 0 To objArchivoPlanoEstructuraBE.Tables(0).Rows.Count - 1

                iColumnaOrden = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaOrden")
                sColumnaNombre = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaNombre")
                sColumnaDescripcion = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaDescripcion")
                iColumnaPosicionInicial = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaPosicionInicial")
                iColumnaLongitud = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaLongitud")
                sColumnaTipoDato = objArchivoPlanoEstructuraBE.Tables(0).Rows(iFila)("ColumnaTipoDato")

                'ColumnaOrden         ColumnaNombre        ColumnaPosicionInicial ColumnaLongitud      
                '-------------------- -------------------- ---------------------- -------------------- 
                '1                    Código Mnemonico     0                      8
                '2                    Factor               8                      22
                '3                    Concepto Ejercicio   30                     19
                '4                    Fecha de Acuerdo     49                     11
                '5                    Fecha de Corte       60                     11
                '6                    Fecha de Registro    71                     11
                '7                    Fecha de Entrega     82                     11

                Select Case iColumnaOrden
                    Case 1
                        If iColumnaLongitud <> 0 Then
                            sCodigoMnemonico = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                        Else
                            sCodigoMnemonico = "***"
                        End If
                    Case 2
                        If iColumnaLongitud <> 0 Then
                            sFactor = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                        Else
                            sFactor = "***"
                        End If
                    Case 3
                        If iColumnaLongitud <> 0 Then
                            sConceptoEjercicio = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                        Else
                            sConceptoEjercicio = "***"
                        End If
                    Case 4
                        If iColumnaLongitud <> 0 Then
                            sFechaAcuerdo = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            If Not IsDate(sFechaAcuerdo) Then
                                sFechaAcuerdo = "***"
                            End If
                        Else
                            sFechaAcuerdo = "***"
                        End If
                    Case 5
                        If iColumnaLongitud <> 0 Then
                            sFechaCorte = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            If Not IsDate(sFechaCorte) Then
                                sFechaCorte = "***"
                            End If
                        Else
                            sFechaCorte = "***"
                        End If
                    Case 6
                        If iColumnaLongitud <> 0 Then
                            sFechaRegistro = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            If Not IsDate(sFechaRegistro) Then
                                sFechaRegistro = "***"
                            End If
                        Else
                            sFechaRegistro = "***"
                        End If
                    Case 7
                        If iColumnaLongitud <> 0 Then
                            sFechaEntrega = Trim(Linea.PadRight(iColumnaLongitud, " ").Substring(iColumnaPosicionInicial, iColumnaLongitud))
                            If Not IsDate(sFechaEntrega) Then
                                sFechaEntrega = "***"
                            End If
                        Else
                            sFechaEntrega = "***"
                        End If
                End Select
            Next

            sEntidadArchivo = sEntidadArchivo & sCodigoMnemonico & "@@" & sFactor & "@@" & _
                                sConceptoEjercicio & "@@" & sFechaAcuerdo & "@@" & sFechaCorte & "@@" & _
                                sFechaRegistro & "@@" & sFechaEntrega & "@*@"

            sFechaInformacion = tbFechaInformacion.Text
            sFechaInformacion = sFechaInformacion.Substring(6, 4) & sFechaInformacion.Substring(3, 2) & sFechaInformacion.Substring(0, 2)
            ' & " 00:00:00"

            If IcontLinea = 25 Then
                strCodigoOI = objEntidadExtArchivoPlano.CargarArchivo(ddlEntidadExterna.SelectedValue.ToString, sEntidadArchivo, sFechaInformacion, DatosRequest)
                sEntidadArchivo = ""
                IcontLinea = 0
            End If
        Loop
        IcontLinea = IcontLinea + 1
        If sEntidadArchivo.Trim <> "" Then
            strCodigoOI = objEntidadExtArchivoPlano.CargarArchivo(ddlEntidadExterna.SelectedValue.ToString, sEntidadArchivo, sFechaInformacion, DatosRequest)
        End If

        'Close the file.
        FileClose(file_num)

    End Sub

    Private Sub CargarListaArchivos()
        Dim archivo, carpeta As String 'para el nombre de archivos y carpetas
        Dim sArchivos(), sCarpetas() As String 'array con los nombres de archivos y directorios
        Dim carpetaInfo As DirectoryInfo 'objeto para extraer propiedades de las carpetas
        Dim archivoInfo As FileInfo 'objeto para extraer propiedades de los archivos
        Dim a, b, i As Integer 'para llevar la cuenta del nº de archivos

        'ruta relativa al directorio cuyos archivos se van a listar
        Dim ruta2 As String
        'asignación del texto del TextBox
        ruta2 = Myfile.Text
        'cadena que almacena la ruta dentro del servidor web
        'Dim ruta As String = Server.MapPath(ruta2)
        Dim dtTabla As New DataTable

        dtTabla.Columns.Add("Nombre Archivo")
        dtTabla.Columns.Add("Tamaño")
        dtTabla.Columns.Add("Fecha")
        dtTabla.Columns.Add("Hora")

        Try
            'el objeto Response permite interaccionar servidor con cliente,
            'su método Write envía resultados HTTP al navegador web cliente.
            'Aquí lo utilizamos para ir enviando código HTML que va formateando una tabla
            'cuyas celdas se irán rellenando con texto y resultados de la ejecución de código VB

            'array con los nombres de archivo en el directorio actual
            sArchivos = Directory.GetFiles(ruta2)
            sCarpetas = Directory.GetDirectories(ruta2)

            'número de archivos en el directorio
            i = sArchivos.Length
            'crear la tabla y la cabecera con títulos de columnas

            'sección que lista los archivos que cuelgan directamente del directorio actual
            'condiciones: listar sólo en carpetas con al menos 1 archivo

            'Obtener lista de archivos contenidos en el directorio actual
            For Each archivo In sArchivos
                archivoInfo = New FileInfo(archivo)
                Dim row As DataRow = dtTabla.NewRow()
                row(0) = archivoInfo.Name  'Nombre Archivo
                row(1) = archivoInfo.Length.ToString("#,#") & " bytes"   'Tamaño
                row(2) = archivoInfo.CreationTime.ToShortDateString  'Fecha
                row(3) = archivoInfo.CreationTime.ToLongTimeString  'Hora

                dtTabla.Rows.Add(row)

            Next

            dgDirtectorioContenido.Columns(0).Visible = True
            dgDirtectorioContenido.DataSource = dtTabla
            dgDirtectorioContenido.DataBind()

        Catch pollo As Exception
            ''finalizar la tabla con el Nº total de archivos listados
            ''sólo si el TextBox contiene alguna ruta

        End Try


    End Sub

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

    Private Sub CargarArchivoIndicadorBloomberg()
        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim sFechaInformacion As Date
        Dim blnExisteItem As Boolean
        Dim fichero As String = Myfile.Text & "\" & sArchivo
        Dim strmensaje As String

        Try
            Dim oConn As New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim DtBloomberg As New DataTable

            oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & fichero & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            'oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & fichero & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()
            oCmd.CommandText = "SELECT * FROM [DIV-LIB BLOOMBERG$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            'Llenar el DataSet

            oDa.Fill(oDs, "ListaPrecios")
            DtBloomberg = oDs.Tables(0)
            sFechaInformacion = tbFechaInformacion.Text

            If DtBloomberg.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else

                If Not blnExisteItem Then
                    objEntidadExtArchivoPlano.InsertarDivLibBloomberg(sFechaInformacion, DtBloomberg, Me.DatosRequest)
                Else
                    oInterfaseBloombergBM.EliminarIndBloomberg(sFechaInformacion, Me.DatosRequest)

                    oInterfaseBloombergBM.InsertarIndicadorBloomberg(sFechaInformacion, DtBloomberg, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                End If

            End If

            oConn.Close()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Function VerificaPreCarga() As Boolean
        Dim sFechaInformacion As String
        sFechaInformacion = tbFechaInformacion.Text
        sFechaInformacion = sFechaInformacion.Substring(6, 4) & sFechaInformacion.Substring(3, 2) & sFechaInformacion.Substring(0, 2)
        Dim oEntidadExtInformacion As DataSet = objEntidadExtArchivoPlano.VerificaPreCarga(ddlEntidadExterna.SelectedValue.ToString, CLng(sFechaInformacion), DatosRequest)
        If oEntidadExtInformacion.Tables(0).Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

#End Region

End Class
