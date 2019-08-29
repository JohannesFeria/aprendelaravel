Imports System.IO
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Parametria_Tablas_Generales_frmAprobadorCarta
    Inherits BasePage

    Const CONST_ARCHIVO_SIN_FIRMA As String = "sin_firma.png"
    Dim oAprobadorCartaBM As New AprobadorCartaBM
    Dim oPersonalBM As New PersonalBM

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoUsuario.Text = CType(Session("SS_DatosModal")(0), String)
                tbNombreUsuario.Text = CType(Session("SS_DatosModal")(1), String)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    'OT11112 - 05/02/2018 - Ian Pastor M.
    'Descripción: Refactorizar código al momento de guardar y modificar
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oAprobadorCartaBE As AprobadorCartaBE

            oAprobadorCartaBE = CreateObjectAprobadorCarta()
            If Not Request.QueryString("cod") Is Nothing Then
                oAprobadorCartaBM.Modificar(oAprobadorCartaBE, DatosRequest)
                Me.AlertaJS("Los cambios se han realizado satisfactoriamente!")
            Else
                oAprobadorCartaBM.Insertar(oAprobadorCartaBE, DatosRequest)
                Me.AlertaJS("Se ha registrado satisfactoriamente!")
            End If

            EjecutarJS("showFirma();")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Call Retornar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

    Protected Sub ddlRol_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlRol.SelectedIndexChanged
        Try
            HabilitarControlesRol(ddlRol.SelectedValue)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub Retornar()
        Response.Redirect("frmBusquedaAprobadorCarta.aspx?codInterno=" & Request.QueryString("codInterno") & "&rol=" & Request.QueryString("rol") & "&situacion=" & Request.QueryString("situacion"))
    End Sub

    Private Sub CargarPagina()
        CargarCombos()
        ViewState("VS_FolderFirmas") = ObtenerFolderFirmas()

        Dim archivoImgLocal As String = CONST_ARCHIVO_SIN_FIRMA

        If Not Request.QueryString("cod") Is Nothing Then
            HabilitarControles(False)
            Dim dt As DataTable
            dt = oAprobadorCartaBM.SeleccionarPorFiltro(Request.QueryString("cod"), "", "", DatosRequest).Tables(0)
            tbCodigoUsuario.Text = dt.Rows(0)("CodigoInterno").ToString()
            tbNombreUsuario.Text = dt.Rows(0)("Nombre").ToString()
            ddlRol.SelectedValue = dt.Rows(0)("Rol").ToString()
            ddlSituacion.SelectedValue = dt.Rows(0)("Situacion").ToString()
            ddlTipoFirma.SelectedValue = dt.Rows(0)("TipoFirmante").ToString()

            If dt.Rows(0)("Rol") = ParametrosSIT.FRM_CARTA Then
                'CRumiche (2019-07-04): Realizamos la descarga de la imagen de Firma desde el FileServer (Solamente a nivel de Bytes)             
                ViewState("VS_ArchivoImgServer") = ObtenerDataImgDesdeFileServer(dt.Rows(0)("Firma").ToString)
            End If
            EjecutarJS("showFirma();")
        Else
            HabilitarControles(True)
            lkbBuscarUsuario.Enabled = True
            lkbBuscarUsuario.Attributes.Add("onclick", "javascript:return showPopupUsuarios();")
        End If

        HabilitarControlesRol(ddlRol.SelectedValue)
    End Sub

    Private Sub CargarCombos()
        CargarRol()
        CargarSituacion()
        CargarTipoFirma()
    End Sub

    Private Sub CargarRol()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.ROL_CARTA, DatosRequest)
        HelpCombo.LlenarComboBox(ddlRol, dt, "Valor", "Nombre", False)
        ddlRol.SelectedValue = ParametrosSIT.ADM_CARTA
    End Sub

    Private Sub CargarSituacion()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.SITUACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", False)
        ddlSituacion.SelectedValue = ParametrosSIT.ESTADO_ACTIVO
    End Sub

    Private Sub CargarTipoFirma()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.TIPOFIRMA, DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoFirma, dt, "Valor", "Nombre", True)
        ddlTipoFirma.SelectedValue = ParametrosSIT.TIPOFIRMA
    End Sub

    Private Sub HabilitarControles(ByVal habilita As Boolean)
        tbCodigoUsuario.Enabled = habilita
        lkbBuscarUsuario.Enabled = habilita
    End Sub

    Private Sub LimpiarCampos()
        tbCodigoUsuario.Text = ""
        tbNombreUsuario.Text = ""
        ddlRol.ClearSelection()
        ddlSituacion.ClearSelection()
    End Sub

    Private Sub HistorialCambios(ByVal pProceso As String, ByVal pRutaFirmas As String)
        Dim oHistorialCambiosRow As HistorialCambiosBE.HistorialCambiosRow
        Dim oHistorialCambiosBE As New HistorialCambiosBE
        Dim oHistorialCambiosBM As New HistorialCambiosBM
        oHistorialCambiosRow = CType(oHistorialCambiosBE.HistorialCambios.NewRow(), HistorialCambiosBE.HistorialCambiosRow)
        oHistorialCambiosBM.InicializarHistorialCambios(oHistorialCambiosRow)
        oHistorialCambiosRow.Aplicativo = ConfigurationManager.AppSettings("CODIGO_SISTEMA")
        oHistorialCambiosRow.Funcionalidad = "Aprobador Carta - " & pProceso
        oHistorialCambiosRow.idRegistro = tbCodigoUsuario.Text
        oHistorialCambiosRow.SituacionAnterior = ddlSituacion.SelectedValue
        oHistorialCambiosRow.SituacionActual = ddlSituacion.SelectedValue
        oHistorialCambiosRow.Rol_RutaFirma = ddlRol.SelectedValue.Trim & pRutaFirmas.Trim
        oHistorialCambiosBE.HistorialCambios.Rows.Add(oHistorialCambiosRow)
        oHistorialCambiosBE.HistorialCambios.AcceptChanges()
        oHistorialCambiosBM.Insertar(oHistorialCambiosBE, DatosRequest)
    End Sub

    Private Function CreateObjectAprobadorCarta() As AprobadorCartaBE
        If ddlRol.SelectedValue = "2" Then
            If ddlTipoFirma.SelectedValue.Equals("") Then
                Throw New System.Exception("Seleccione el tipo firmante")
            End If
        End If

        Dim rutaImagenFirma As String = ViewState("VS_ArchivoImgServer")
        If files.Value <> "" Then

            If File.Exists(rutaImagenFirma) And Not rutaImagenFirma.EndsWith(CONST_ARCHIVO_SIN_FIRMA) Then File.Delete(rutaImagenFirma)
            rutaImagenFirma = String.Format("{0}{1}{2}", ViewState("VS_FolderFirmas"), tbCodigoUsuario.Text, Path.GetExtension(files.PostedFile.FileName))

            'CRumiche (2019-07-08): Guardamos el archivo directamente en el FileServer
            Using fileStream As FileStream = File.Create(rutaImagenFirma)
                files.PostedFile.InputStream.Seek(0, SeekOrigin.Begin)
                files.PostedFile.InputStream.CopyTo(fileStream)
                fileStream.Close()
            End Using
            'CRumiche (2019-07-04): Realizamos la descarga de la imagen de Firma desde el FileServer (Solamente a nivel de Bytes) 
            ObtenerDataImgDesdeFileServer(rutaImagenFirma)
        End If

        Dim oAprobadorCartaRow As AprobadorCartaBE.AprobadorCartaRow
        Dim oAprobadorCartaBE As New AprobadorCartaBE
        oAprobadorCartaRow = CType(oAprobadorCartaBE.AprobadorCarta.NewRow(), AprobadorCartaBE.AprobadorCartaRow)
        oAprobadorCartaBM.InicializarAprobadorCarta(oAprobadorCartaRow)
        oAprobadorCartaRow.CodigoInterno = tbCodigoUsuario.Text
        oAprobadorCartaRow.Rol = ddlRol.SelectedValue
        oAprobadorCartaRow.Situacion = ddlSituacion.SelectedValue
        oAprobadorCartaRow.Firma = rutaImagenFirma
        oAprobadorCartaRow.TipoFirmante = ddlTipoFirma.SelectedValue
        oAprobadorCartaBE.AprobadorCarta.Rows.Add(oAprobadorCartaRow)
        oAprobadorCartaBE.AprobadorCarta.AcceptChanges()

        Return oAprobadorCartaBE
    End Function

    Private Sub HabilitarControlesRol(ByVal rol As String)
        ddlTipoFirma.Enabled = IIf(rol = "2", True, False)
        If rol = "2" Then
            files.Attributes.Remove("disabled")

        Else
            files.Attributes.Add("disabled", "true")
            ddlTipoFirma.SelectedValue = ""
        End If
    End Sub

    Function ObtenerFolderFirmas() As String
        Dim dt As DataTable = New ParametrosGeneralesBM().Listar(ParametrosSIT.RUTA_FIRMA_CARTA, DatosRequest)
        Dim folderImgFirmas As String = dt.Rows(0)("Comentario").ToString()
        If Not Directory.Exists(folderImgFirmas) Then Throw New Exception(String.Format("No existe la carpeta de imagenes de firmas configurada en la variable {0}: {1}", ParametrosSIT.RUTA_FIRMA_CARTA, folderImgFirmas))
        If Not folderImgFirmas.EndsWith("\") Then folderImgFirmas += "\"

        Return folderImgFirmas
    End Function

    Function CopiarImgFirmaDesdeLocalHaciaFileServer(ByVal nombreArchivoSinExtension As String) As String
        If Directory.Exists(ViewState("VS_FolderFirmas")) And Not nombreArchivoSinExtension.Equals("sin_firma") Then
            Dim archivoImgLocal As String = ViewState("VS_ArchivoImgLocal")
            Dim pathImgLocal As String = Server.MapPath("~\Imagenes\") & archivoImgLocal
            Dim pathImgFileServer As String = ViewState("VS_FolderFirmas") & nombreArchivoSinExtension & Path.GetExtension(archivoImgLocal)

            File.Copy(pathImgLocal, pathImgFileServer, True)
            Return pathImgFileServer
        End If

        Return ""
    End Function

    Function ObtenerDataImgDesdeFileServer(ByVal rutaImgFileServer As String) As String
        Dim rutaValidaEnServer As String = rutaImgFileServer

        'Lo siguiente lo realizamos por si el archivo no esté ubicado en la ruta compartida VS_FolderFirmas
        If Not File.Exists(rutaValidaEnServer) Then rutaValidaEnServer = ViewState("VS_FolderFirmas") & Path.GetFileName(rutaImgFileServer)

        If File.Exists(rutaValidaEnServer) Then
            Dim fileInfo As New FileInfo(rutaValidaEnServer)
            If fileInfo.IsReadOnly Then fileInfo.IsReadOnly = False

            'CRumiche (2019-07-04): Realizamos la descarga Solamente a nivel de Bytes
            Dim bytes() As Byte = File.ReadAllBytes(rutaValidaEnServer)
            hdPathFirma.Value = "data:image/jpg;base64," & Convert.ToBase64String(CType(bytes, Byte()))
        Else
            hdPathFirma.Value = ""
        End If

        Return rutaValidaEnServer
    End Function

End Class
