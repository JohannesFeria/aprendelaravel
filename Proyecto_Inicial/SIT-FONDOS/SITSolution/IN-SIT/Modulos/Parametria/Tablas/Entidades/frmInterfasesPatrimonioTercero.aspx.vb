''''' CREACION: MPENAL - 09/09/2016

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb
Imports Microsoft.Office

Partial Class Modulos_Parametria_Tablas_Entidades_frmInterfasesPatrimonioTercero
    Inherits BasePage
#Region "Variables"
    Dim oUtil As New UtilDM
    Dim sFileName As String
    Dim oPatrimonioTerceroBM As New PatrimonioTerceroBM
    Dim oPatrimonioTerceroBE As New PatrimonioTerceroBE
    Dim contadorFilasProcesadas As Integer = 0
    Dim cantRegistros As Integer = 0
#End Region
    Private RutaDestino As String = String.Empty
    Private Const NombreHoja As String = "Hoja1"
    Private Const RangoDatos As String = "A1:G3000"
    Private Const ExtensionValida As String = ".xls"

    Private Const TipoArchivoTercero As String = "TER"
    Private Const TipoArchivoEmision As String = "EMI"


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombo()
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Confirmacion();")
            Me.tbFechaProceso.Text = oUtil.RetornarFechaSistema
        End If
    End Sub

    Public Sub CargarCombo()
        Dim dtTipoArchivo As New DataTable
        Dim dr As DataRow
        dtTipoArchivo.Columns.Add("CodigoTipoArchivo", GetType(String))
        dtTipoArchivo.Columns.Add("Descripcion", GetType(String))

        dr = dtTipoArchivo.NewRow()
        dr("CodigoTipoArchivo") = TipoArchivoEmision
        dr("Descripcion") = "Emisión"
        dtTipoArchivo.Rows.Add(dr)

        dr = dtTipoArchivo.NewRow()
        dr("CodigoTipoArchivo") = TipoArchivoTercero
        dr("Descripcion") = "Tercero"
        dtTipoArchivo.Rows.Add(dr)

        HelpCombo.LlenarComboBox(ddlTipoArchivo, dtTipoArchivo, "CodigoTipoArchivo", "Descripcion", False)
    End Sub

    Private Sub Imagebutton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Imagebutton3.Click
        Response.Redirect("../../../../frmDefault.aspx")
    End Sub

    Private Sub ProcesarArchivoEmision()
        Dim codigosMnemonicosDuplicados As IEnumerable(Of Object) = Nothing
        Dim codigosMnemonicosConCero As IEnumerable(Of Object) = Nothing

        If Not valExistenDuplicadosPrimeraColumna(codigosMnemonicosDuplicados) Then
            Dim strCodMnemonicoDuplicados As String = String.Join("<br />", codigosMnemonicosDuplicados.Select(Function(d) d))
            AlertaJS("No se puede procesar el archivo. Existen instrumentos duplicados. <br />Códigos Mnemonicos: <br />" & strCodMnemonicoDuplicados)
        ElseIf Not ValValoresCeroEnFila(codigosMnemonicosConCero) Then
            Dim strCodMnemonicoConCero As String = String.Join("<br />", codigosMnemonicosConCero.Select(Function(d) d))
            AlertaJS("No se puede procesar el archivo. Existen registros con valores en cero. <br />Códigos Mnemonicos: <br />" & strCodMnemonicoConCero)
        Else
            CargarMasivos(ddlTipoArchivo.SelectedValue)
        End If
        AlertaJS("Archivo cargado correctamente.")
    End Sub

    Private Sub ProcesarArchivoTercero()
        Dim codigosTercerosDuplicados As IEnumerable(Of Object) = Nothing
        Dim codigosTercerosConCero As IEnumerable(Of Object) = Nothing

        If Not valExistenDuplicadosPrimeraColumna(codigosTercerosDuplicados) Then
            Dim strCodTerceroDuplicados As String = String.Join("<br />", codigosTercerosDuplicados.Select(Function(d) d))
            AlertaJS("No se puede procesar el archivo. Existen terceros duplicados. <br />Códigos de Tercero: <br />" & strCodTerceroDuplicados)
        ElseIf Not ValValoresCeroEnFila(codigosTercerosConCero) Then
            Dim strCodTerceroConCero As String = String.Join("<br />", codigosTercerosConCero.Select(Function(d) d))
            AlertaJS("No se puede procesar el archivo. Existen registros con valores en cero. <br />Códigos de Tercero: <br />" & strCodTerceroConCero)
        Else
            CargarMasivos(ddlTipoArchivo.SelectedValue)
        End If
        AlertaJS("Archivo cargado correctamente.")
    End Sub

    Private Sub CargarMasivos(Optional ByVal tipoArchivo As String = TipoArchivoTercero)
        Dim dtPatrimonioTercero As New DataTable
        Dim fechaProceso As Decimal

        msgError.Visible = False
        msgTotalRegistrosProcesados.Visible = False

        sFileName = Myfile.Text
        fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value

            If fInfo.Extension = ExtensionValida Then
                iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)
                If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                    Try
                        CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtPatrimonioTercero)
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    Finally
                        File.Delete(RutaDestino & "\" & fInfo.Name)
                    End Try
                Else
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End If
            Else
                If fInfo.Extension.Equals("") Then
                    AlertaJS("La extensión del archivo no es válida.")
                Else
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                    AlertaJS("La extensión del archivo no es válida.")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString)
        End Try
        fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))

        oPatrimonioTerceroBM.Borrar_PatrimonioTercero(fechaProceso, tipoArchivo)
        Dim mensajePersonalizado As String = ""
        Dim banderaProcesamientoDetenido = InsertarPatrimoniosTercero(dtPatrimonioTercero, fechaProceso, tipoArchivo, mensajePersonalizado)
        gvResumenDataCargada.PageIndex = 0
        CargarGrilla()

        If banderaProcesamientoDetenido Then
            Dim messageT As String = "Error al procesar. Se procesaron " & contadorFilasProcesadas & " sobre un total de " & cantRegistros & "."
            messageT = messageT & mensajePersonalizado
            AlertaJS(messageT)
        Else
            If contadorFilasProcesadas <> cantRegistros Then
                AlertaJS("Se procesaron " & contadorFilasProcesadas & " sobre un total de " & cantRegistros)
            Else
                AlertaJS("El Archivo Plano ha sido importado correctamente")
            End If
        End If
    End Sub

    Private Sub ProcesarArchivo()
        Try
            Dim tipoArchivoValue As String = ddlTipoArchivo.SelectedValue
            Select Case tipoArchivoValue
                Case TipoArchivoEmision
                    ProcesarArchivoEmision()
                Case TipoArchivoTercero
                    ProcesarArchivoTercero()
            End Select
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub

    ' Validacion de los campos obligatorios: Fecha Proceso, Tipo Archivo y Archivo.
    Private Function IsValidoCamposObligatorios(ByRef msjValidacion As String) As Boolean
        ' Validacion de Fecha de Proceso
        Dim fechaProceso As String = CStr(UIUtility.ConvertirFechaaDecimal(tbFechaProceso.Text))
        If String.IsNullOrEmpty(fechaProceso) Then
            msjValidacion = "La Fecha de Proceso no es válida. Revisar la Fecha de Proceso."
            Return False
        End If

        Try
            DateTime.ParseExact(fechaProceso, "yyyyMMdd", Nothing)
        Catch ex As Exception
            msjValidacion = "La Fecha de Proceso no es válida. Revisar la Fecha de Proceso."
            Return False
        End Try

        ' Validacion de Tipo Archivo
        Dim tipoArchivoValue As String = ddlTipoArchivo.SelectedValue
        If String.IsNullOrEmpty(tipoArchivoValue) Then
            msjValidacion = "El Tipo de Archivo no es válido. Revisar el Tipo de Archivo."
            Return False
        End If

        ' Validacion de Archivo.
        Dim fileNameClient As String = iptRuta.PostedFile.FileName
        If String.IsNullOrEmpty(fileNameClient) Then
            msjValidacion = "El nombre del archivo no es válido. Revisar el Nombre del Archivo."
            Return False
        End If

        msjValidacion = ""
        Return True
    End Function

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
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
                    ProcesarArchivo()
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub

    Private Sub CargarRuta()
        RutaDestino = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        If (Not Directory.Exists(RutaDestino)) Then
            AlertaJS("No existe la ruta destino.")
            Exit Sub
        End If

        hfRutaDestino.Value = RutaDestino
        btnProcesar.Enabled = True
    End Sub

    Private Function InsertarPatrimoniosTercero(ByVal dtPatrimonioTercero As DataTable, ByVal fechaProceso As Decimal, ByVal tipoArchivo As String _
                                              , ByRef mensajePersonalizado As String) As Boolean
        Dim errorVacio As Boolean = False
        Dim banderaProcesamientoDetenido As Boolean = False
        cantRegistros = dtPatrimonioTercero.Rows.Count
        contadorFilasProcesadas = 0

        If dtPatrimonioTercero.Rows.Count > 0 Then
            oPatrimonioTerceroBE = New PatrimonioTerceroBE
            For index = 0 To dtPatrimonioTercero.Rows.Count - 1
                Try
                    Dim oPatrimonioTercero As PatrimonioTerceroBE.PatrimonioTerceroRow = _
                        oPatrimonioTerceroBE.PatrimonioTercero.NewPatrimonioTerceroRow()

                    If dtPatrimonioTercero.Rows(index)(0) Is DBNull.Value Then
                        banderaProcesamientoDetenido = True
                        Exit For
                    End If

                    If tipoArchivo = TipoArchivoEmision Then
                        oPatrimonioTercero.CodigoMnemonico = dtPatrimonioTercero.Rows(index)(0).ToString()
                    Else
                        oPatrimonioTercero.CodigoTercero = dtPatrimonioTercero.Rows(index)(0).ToString()
                    End If

                    oPatrimonioTercero.ActivoMN = ObtenerValorValidoCampoNoObligatorioDecimal(dtPatrimonioTercero.Rows(index)(1))
                    oPatrimonioTercero.PasivoMN = ObtenerValorValidoCampoNoObligatorioDecimal(dtPatrimonioTercero.Rows(index)(2))
                    oPatrimonioTercero.PatrimonioMN = ObtenerValorValidoCampoNoObligatorioDecimal(dtPatrimonioTercero.Rows(index)(3))
                    oPatrimonioTercero.ActivoME = ObtenerValorValidoCampoNoObligatorioDecimal(dtPatrimonioTercero.Rows(index)(4))
                    oPatrimonioTercero.PasivoME = ObtenerValorValidoCampoNoObligatorioDecimal(dtPatrimonioTercero.Rows(index)(5))
                    oPatrimonioTercero.PatrimonioME = ObtenerValorValidoCampoNoObligatorioDecimal(dtPatrimonioTercero.Rows(index)(6))
                    oPatrimonioTercero.TipoInformacion = tipoArchivo

                    oPatrimonioTercero.Fecha = fechaProceso

                    oPatrimonioTerceroBM.Insertar(oPatrimonioTercero, MyBase.DatosRequest())
                    oPatrimonioTerceroBE.PatrimonioTercero.AddPatrimonioTerceroRow(oPatrimonioTercero)
                    contadorFilasProcesadas = contadorFilasProcesadas + 1
                Catch ex As Exception
                    If ex.Message.ToUpper.Contains("FOREIGN KEY") Then

                        mensajePersonalizado = "<br />El Código " & If(tipoArchivo = TipoArchivoEmision, "Mnemónico ", "de Tercero") &
                            dtPatrimonioTercero.Rows(index)(0).ToString() & " no existe."
                    End If

                    banderaProcesamientoDetenido = True
                    Exit For
                End Try
            Next
        Else
            errorVacio = True
        End If

        If errorVacio Then
            msgError.Visible = True
            msgError.Text = "No hay datos en el Archivo Plano..."
        End If
        'If Not errorVacio Then
        '    AlertaJS("El Archivo Plano ha sido importado correctamente")
        'End If
        Return banderaProcesamientoDetenido
    End Function

    Private Function ObtenerValorValidoCampoNoObligatorioDecimal(ByVal campo As Object) As Decimal
        If campo Is DBNull.Value Then
            Return 0
        End If

        If Not IsNumeric(campo) Then
            Return 0
        End If

        Try
            Dim valueDecimal As Decimal = Convert.ToDecimal(campo)
            If valueDecimal < 0 Then
                Return 0
            Else
                Return valueDecimal
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Sub CargarRango(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dt As DataTable)
        Try
            If System.IO.File.Exists(sFileName) Then
                Dim objDataSet As System.Data.DataSet
                Dim objDataAdapter As System.Data.OleDb.OleDbDataAdapter
                Dim sCs As String = ""
                If System.Configuration.ConfigurationManager.AppSettings("Ambiente").ToString() = "Desarrollo" Then
                    sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Excel 8.0;"
                Else
                    sCs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"
                End If
                Dim objOleConnection As System.Data.OleDb.OleDbConnection
                objOleConnection = New System.Data.OleDb.OleDbConnection(sCs)
                Dim sSql As String = "select * from " & "[" & sSheetName & "$" & sRange & "]"
                objDataAdapter = New System.Data.OleDb.OleDbDataAdapter(sSql, objOleConnection)
                objDataSet = New System.Data.DataSet
                objDataAdapter.Fill(objDataSet)
                dt = objDataSet.Tables(0)
                objOleConnection.Close()
            Else
                AlertaJS("No se ha encontrado el archivo: " & sFileName)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Function validar() As Boolean
        Dim fecha As String = CStr(UIUtility.ConvertirFechaaDecimal(tbFechaProceso.Text))
        If String.IsNullOrEmpty(fecha) Then
            Return False
        End If
        Try
            DateTime.ParseExact(fecha, "yyyyMMdd", Nothing)
        Catch ex As Exception
            Return False
        End Try

        'fecha = fecha.Substring(4, 4)

        Dim fileNameClient As String = iptRuta.PostedFile.FileName
        If String.IsNullOrEmpty(fileNameClient) Then
            Return False
        End If

        Return True
    End Function

    Private Sub CargarGrilla()
        Dim fechaProceso As Decimal = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))
        Dim tipoArchivo As String = ddlTipoArchivo.SelectedValue
        Dim dtblDatos As DataTable = oPatrimonioTerceroBM.SeleccionarPorFecha(fechaProceso, tipoArchivo).Tables(0)

        msgTotalRegistrosProcesados.Text = "Total de registros: " & dtblDatos.Rows.Count
        msgTotalRegistrosProcesados.Visible = True
        gvResumenDataCargada.Visible = True
        gvResumenDataCargada.DataSource = dtblDatos
        gvResumenDataCargada.DataBind()
    End Sub

    Protected Sub gvResumenDataCargada_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResumenDataCargada.PageIndexChanging
        Try
            gvResumenDataCargada.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Private Sub CargarGrillaFiltro()
        Dim tipoArchivo = ddlTipoArchivo.SelectedValue
        If String.IsNullOrEmpty(tipoArchivo) Then
            msgError.Visible = False
            msgTotalRegistrosProcesados.Visible = False
            gvResumenDataCargada.DataSource = Nothing
            gvResumenDataCargada.Visible = False

            Exit Sub
        End If

        Dim fechaProceso As Decimal
        Try
            fechaProceso = Convert.ToDecimal(tbFechaProceso.Text.Substring(6, 4) + tbFechaProceso.Text.Substring(3, 2) + tbFechaProceso.Text.Substring(0, 2))
        Catch ex As Exception
            'Dim dateNow As String = DateTime.Now.ToString("dd/MM/yyyy")
            'fechaProceso = Convert.ToDecimal(dateNow.Substring(6, 4) + dateNow.Substring(3, 2) + dateNow.Substring(0, 2))
            'tbFechaProceso.Text = dateNow
            msgError.Visible = False
            msgTotalRegistrosProcesados.Visible = False
            gvResumenDataCargada.DataSource = Nothing
            gvResumenDataCargada.Visible = False

            Exit Sub
        End Try

        Dim dtblDatos As DataTable = oPatrimonioTerceroBM.SeleccionarPorFecha(fechaProceso, tipoArchivo).Tables(0)

        If dtblDatos.Rows.Count > 0 Then
            msgError.Visible = False
            msgError.Text = ""
            msgTotalRegistrosProcesados.Visible = True
            msgTotalRegistrosProcesados.Text = "Total de registros: " & dtblDatos.Rows.Count
            gvResumenDataCargada.DataSource = dtblDatos
            gvResumenDataCargada.DataBind()
            gvResumenDataCargada.Visible = True
        Else
            msgError.Visible = False
            msgTotalRegistrosProcesados.Visible = False
            gvResumenDataCargada.DataSource = Nothing
            gvResumenDataCargada.Visible = False
        End If
    End Sub

    Protected Sub ddlTipoArchivo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoArchivo.SelectedIndexChanged
        CargarGrillaFiltro()
    End Sub

    Protected Sub tbFechaProceso_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaProceso.TextChanged
        CargarGrillaFiltro()
    End Sub

    Private Function valExistenDuplicadosPrimeraColumna(ByRef codigosPrimeraColumnaDuplicados As IEnumerable(Of Object)) As Boolean
        Dim dtPatrimonioTercero As New DataTable
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)

            If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                Try
                    CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtPatrimonioTercero)
                Catch ex As Exception
                    Return True
                Finally
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End Try
            Else
                File.Delete(RutaDestino & "\" & fInfo.Name)
            End If
        Catch ex As Exception
            Return True
        End Try

        Dim duplicados = dtPatrimonioTercero.AsEnumerable().Where(Function(r) r(0) IsNot DBNull.Value).GroupBy(Function(p) p(0)).Where(Function(gr) gr.Count > 1).ToList()
        If duplicados IsNot Nothing And duplicados.Count > 0 Then
            codigosPrimeraColumnaDuplicados = duplicados.SelectMany(Function(e) e).Select(Function(e) e.ItemArray(0)).Distinct()
            Return False
        End If

        Return True
    End Function

    Private Function ValValoresCeroEnFila(ByRef codigosTercerosConCero As IEnumerable(Of Object)) As Boolean
        Dim dtPatrimonioTercero As New DataTable
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)

            If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                Try
                    CargarRango(RutaDestino & "\" & fInfo.Name, NombreHoja, RangoDatos, dtPatrimonioTercero)
                Catch ex As Exception
                    Return True
                Finally
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End Try
            Else
                File.Delete(RutaDestino & "\" & fInfo.Name)
            End If
        Catch ex As Exception
            Return True
        End Try

        For Each row As DataRow In dtPatrimonioTercero.Rows
            If row(1) Is DBNull.Value Then row(1) = 0
            If row(2) Is DBNull.Value Then row(2) = 0
            If row(3) Is DBNull.Value Then row(3) = 0
            If row(4) Is DBNull.Value Then row(4) = 0
            If row(5) Is DBNull.Value Then row(5) = 0
            If row(6) Is DBNull.Value Then row(6) = 0
        Next

        Dim fnRegistrosConCero = Function(r As DataRow)
                                     Return r(1) = 0 And r(2) = 0 And r(3) = 0 And r(4) = 0 And r(5) = 0 And r(6) = 0
                                 End Function
        Dim registrosConCero As Generic.List(Of DataRow) = dtPatrimonioTercero.AsEnumerable().Where(fnRegistrosConCero).ToList()
        If registrosConCero IsNot Nothing And registrosConCero.Count > 0 Then
            codigosTercerosConCero = registrosConCero.Select(Function(e) e.ItemArray(0)).Distinct()
            Return False
        End If

        Return True
    End Function

    Private Function ValExtensionArchivoValido() As Boolean
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            If fInfo.Extension = ExtensionValida Then
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
End Class