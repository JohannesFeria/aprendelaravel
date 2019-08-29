Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Contabilidad_frmAprobacionSolicitudReversion
    Inherits BasePage
#Region "/* Enumerados */"

    Public Enum TIPO_ACCION As Byte
        [NUEVO] = 1
        [MODIFICAR] = 2
    End Enum
#End Region

    Private Property VistaDetalleSolicitudReversion() As SolicitudReversionBE
        Get
            Return DirectCast(ViewState("SolicitudReversion"), SolicitudReversionBE)
        End Get
        Set(ByVal Value As SolicitudReversionBE)
            ViewState("SolicitudReversion") = Value
        End Set
    End Property
    Dim cbSelect As CheckBox
    Dim hdCodigoEstado As HiddenField
    Private Property IdRegistro As Int16
    Private oSolicitudReversionBM As New SolicitudReversionBM
    Dim oSolicitudReversionBE As New SolicitudReversionBE


#Region " /* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()

                If Not Request.QueryString("cod") Is Nothing Then
                    'modificar
                    hd.Value = Request.QueryString("cod")
                Else
                    'crear
                    hd.Value = String.Empty
                    VistaDetalleSolicitudReversion = New SolicitudReversionBE
                    cargarRegistro()
                End If
                CargarGrilla()
            End If

            Dim intIndice As Integer
            intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página / " + ex.Message.ToString)
        End Try
    End Sub

    Private Sub CargarCombos()
        Dim tablaEstadoRegistro As New DataTable

        tablaEstadoRegistro = oSolicitudReversionBM.ObtenerEstadoAprobacionRegistroSolicitudReversion()
        HelpCombo.LlenarComboBox(ddlEstadoRegistro, tablaEstadoRegistro, "LLAVE_TABLA", "DESCRIPCION_LARGA", True, "TODOS")
    End Sub
#End Region

    Private Function crearObjeto() As SolicitudReversionBE

        Dim oRow As SolicitudReversionBE.SolicitudReversionRow

        Dim intIndice As Integer

        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        If intIndice > 0 Then
            oSolicitudReversionBE = VistaDetalleSolicitudReversion
            oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(intIndice), SolicitudReversionBE.SolicitudReversionRow)
        Else
            oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.NewSolicitudReversionRow(), SolicitudReversionBE.SolicitudReversionRow)
        End If

        oRow.FlagAprobado = "P"
        oRow.Situacion = "A"

        If ViewState("IndiceSeleccionado") Is Nothing Then
            oSolicitudReversionBE.SolicitudReversion.AddSolicitudReversionRow(oRow)
        End If
        oSolicitudReversionBE.SolicitudReversion.AcceptChanges()

        Return oSolicitudReversionBE

    End Function

    Private Sub cargarRegistro()

        oSolicitudReversionBE = oSolicitudReversionBM.SeleccionarSolicitudReversion(String.Empty, DatosRequest)

        VistaDetalleSolicitudReversion = oSolicitudReversionBE
    End Sub

    Private Sub EditarGrilla(ByVal id As Int16, ByVal index As Int16)

        oSolicitudReversionBE = VistaDetalleSolicitudReversion

        ViewState("IndiceSeleccionado") = index
    End Sub

    Private Sub Modificar()

        oSolicitudReversionBE = crearObjeto()

        Dim intIndice As Integer
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))

        oSolicitudReversionBM.Modificar(oSolicitudReversionBE, intIndice, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)

        cargarRegistro()

        ViewState("IndiceSeleccionado") = Nothing
    End Sub

    Private Sub EliminarFilaSolicitudReversion(ByVal id As Int16, ByVal index As Int16)

        Dim oRow As SolicitudReversionBE.SolicitudReversionRow

        ViewState("IndiceSeleccionado") = index

        oSolicitudReversionBE = VistaDetalleSolicitudReversion
        oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(index), SolicitudReversionBE.SolicitudReversionRow)

        oRow.BeginEdit()
        oRow.Situacion = "I"

        oRow.EndEdit()
        oRow.AcceptChanges()
        oSolicitudReversionBE.SolicitudReversion.AcceptChanges()

        VistaDetalleSolicitudReversion = oSolicitudReversionBE

        oSolicitudReversionBM.Eliminar(oSolicitudReversionBE, index, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ELIMINAR_ENTIDAD)

        cargarRegistro()
        ViewState("IndiceSeleccionado") = Nothing

    End Sub

    Private Sub CargarGrilla()
        dgLista.DataSource = VistaDetalleSolicitudReversion
        dgLista.DataBind()
        'EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Protected Sub btnAprobar_Click(sender As Object, e As System.EventArgs) Handles btnAprobar.Click

        Dim nSeleccionadas As Int64 = 0
        Dim AproboSolicitud As Boolean = False
        nSeleccionadas = retornarNumSeleccionados(dgLista, "chkOrden")

        Dim iCont As Int64 = dgLista.Rows.Count - 1
        Dim usuarioLogeado As String = UIUtility.ObtenerValorRequest(DatosRequest, "Usuario")

        Dim idSolicitudReversion As Integer

        'VERIFICAR el codigo de dominio
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As DataTable
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIO_APRUEBA_SOLICITUD_REVERSION, "", "", "", DatosRequest)

        Dim codigoDominioConfigurado As String = ""
        For Each fila As DataRow In dt.Rows
            codigoDominioConfigurado = fila("Valor")
        Next

        Dim flagUsuarioConPermisos As Boolean
        flagUsuarioConPermisos = False
        If codigoDominioConfigurado = usuarioLogeado Then
            flagUsuarioConPermisos = True
        End If

        If flagUsuarioConPermisos = False Then
            AlertaJS(Constantes.M_STR_MENSAJE_USUARIO_NO_TIENE_PERMISOS)
            Return
        End If


        If nSeleccionadas = 0 Then
            AlertaJS("Debe seleccionar al menos una solicitud.")
        End If

        Dim chk As CheckBox

        While iCont >= 0
            If dgLista.Rows(iCont).FindControl("chkOrden").GetType Is GetType(CheckBox) Then
                chk = CType(dgLista.Rows(iCont).FindControl("chkOrden"), CheckBox)
                If chk.Checked = True Then

                    Dim oRow As SolicitudReversionBE.SolicitudReversionRow
                    oSolicitudReversionBE = VistaDetalleSolicitudReversion
                    oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(iCont), SolicitudReversionBE.SolicitudReversionRow)
                    'oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(iContListadoTotal), SolicitudReversionBE.SolicitudReversionRow)
                    idSolicitudReversion = oRow.ID

                    Dim resultadoComparacionRegistroAprobado As Integer
                    resultadoComparacionRegistroAprobado = String.Compare(oRow.FlagAprobado, "P")

                    If resultadoComparacionRegistroAprobado = 0 Then
                        'procesar con la aprobacion del registro


                        oSolicitudReversionBM.Aprobar(idSolicitudReversion, "A", String.Empty, DatosRequest)
                        AproboSolicitud = True


                        ViewState("IndiceSeleccionado") = Nothing

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim nomFondo As String = ""
                        nomFondo = dgLista.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NOMBRE_FONDO", dgLista)).Text()
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim toUser As String = ""
                        Dim dtDes As DataTable
                        dtDes = oParametrosGeneralesBM.SeleccionarPorFiltro(APROBACION_SOLICITUD_REVERSION_EMAIL, "", "", "", DatosRequest)
                        For Each fila As DataRow In dtDes.Rows
                            toUser = toUser + fila("Valor") + ";"
                        Next

                        Dim asunto As String = ""
                        asunto = "Aprobación de Reversión del VC Fondo "

                        asunto = asunto + nomFondo + " Fecha: Del " + oRow.FechaInicial + " al " + oRow.FechaFinal
                        Dim message As String = ""

                        Dim resultadoComparacion As Integer
                        resultadoComparacion = String.Compare(oRow.FlagAfectaValorCuota, "S")

                        If resultadoComparacion = 0 Then
                            message = "Se comunica que se ha aprobado una solicitud de reversión que afecta el valor cuota. "
                        Else
                            message = "Se comunica que se ha aprobado una solicitud de reversión que no afecta el valor cuota. "
                        End If

                        Dim rootPath As String = Request.Url.Host

                        Dim detalle As String = ""
                        detalle += "<li>" & "<b>Motivo:</b> " & oRow.Motivo & "</li>"
                        detalle += "<li>" & "<b>Responsable:</b> " & oRow.Responsable & "</li>"
                        detalle += "<li>" & "<b>Acción Temporal:</b> " & oRow.AccionTemporal & "</li>"
                        detalle += "<li>" & "<b>Solución Definitiva Sugerida:</b> " & oRow.SolucionDefinitivaSugerida & "</li>"

                        If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                            EnviarCorreoNotificacion(asunto, message, detalle)
                        End If

                    End If

                End If
            End If
            iCont = iCont - 1

        End While

        If AproboSolicitud = True Then
            AlertaJS("Se aprobaron las solicitudes seleccionadas.")
        End If


        cargarRegistro()
        CargarGrilla()

    End Sub

    Private Function retornarNumSeleccionados(ByVal grilla As GridView, ByVal chkNombre As String) As Integer
        Dim iCont As Int64 = grilla.Rows.Count - 1
        Dim chk As CheckBox
        retornarNumSeleccionados = 0
        While iCont >= 0 And retornarNumSeleccionados < 1
            If grilla.Rows(iCont).FindControl(chkNombre).GetType Is GetType(CheckBox) Then
                chk = CType(grilla.Rows(iCont).FindControl(chkNombre), CheckBox)
                If chk.Checked = True Then
                    retornarNumSeleccionados += 1
                End If
            End If
            iCont = iCont - 1
        End While
        Return retornarNumSeleccionados
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

    Private Sub EnviarCorreoNotificacion(ByVal accionTitulo As String, ByVal accionDescripcion As String, ByVal descripcion As String)
        Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String)
        dict.Add("@Detalles", descripcion)
        dict.Add("@AccionTitulo", accionTitulo)
        dict.Add("@AccionDescripcion", accionDescripcion)

        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtPara As DataTable = oParametrosGeneralesBM.SeleccionarPorFiltro("APROBACION_SOLICITUD_REVERSION_EMAIL", "PARA", "", "", DatosRequest)
        Dim dtCopia As DataTable = oParametrosGeneralesBM.SeleccionarPorFiltro("APROBACION_SOLICITUD_REVERSION_EMAIL", "COPIA", "", "", DatosRequest)
        Dim paraCorreo As String = ""
        Dim copiaCorreo As String = ""
        If dtPara.Rows.Count > 0 Then
            paraCorreo = dtPara.Rows(0)("Valor").ToString()
        End If

        If dtCopia.Rows.Count > 0 Then
            copiaCorreo = dtCopia.Rows(0)("Valor").ToString()
        End If

        Dim archivoCodigo As String
        archivoCodigo = "030"

        If paraCorreo <> "" Then
            UIUtility.EnviarMailPlantilla_X_ArchivoCodigo(paraCorreo, copiaCorreo, accionTitulo, "SolicitudReversion.html", dict, archivoCodigo, DatosRequest)
        End If

    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Dim estadoRegistro As String = ""
        Dim fechaIni As String = ""

        estadoRegistro = ddlEstadoRegistro.SelectedValue
        fechaIni = UIUtility.ConvertirFechaaDecimal(tbFechaInicial.Text)

        If fechaIni = 0 Then
            fechaIni = ""
        End If

        Dim oSolicitudReversion As SolicitudReversionBE

        oSolicitudReversion = oSolicitudReversionBM.Seleccionar(estadoRegistro, fechaIni, DatosRequest)
        VistaDetalleSolicitudReversion = oSolicitudReversion

        CargarGrilla()

    End Sub

    Protected Sub PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            cbSelect = CType(e.Row.FindControl("chkOrden"), CheckBox)
            hdCodigoEstado = CType(e.Row.FindControl("hdCodigoEstado"), HiddenField)

            If hdCodigoEstado.Value = "P" Then
                cbSelect.Enabled = True
            Else
                cbSelect.Enabled = False
            End If
        End If
    End Sub

    Protected Sub btnRechazarReversion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRechazarReversion.Click
        Dim nSeleccionadas As Int64 = 0
        Dim desproboSolicitud As Boolean = False
        nSeleccionadas = retornarNumSeleccionados(dgLista, "chkOrden")

        Dim iCont As Int64 = dgLista.Rows.Count - 1
        Dim usuarioLogeado As String = UIUtility.ObtenerValorRequest(DatosRequest, "Usuario")

        Dim idSolicitudReversion As Integer

        'VERIFICAR el codigo de dominio
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As DataTable
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIO_APRUEBA_SOLICITUD_REVERSION, "", "", "", DatosRequest)

        Dim codigoDominioConfigurado As String = ""
        For Each fila As DataRow In dt.Rows
            codigoDominioConfigurado = fila("Valor")
        Next

        Dim flagUsuarioConPermisos As Boolean
        flagUsuarioConPermisos = False
        If codigoDominioConfigurado = usuarioLogeado Then
            flagUsuarioConPermisos = True
        End If

        If flagUsuarioConPermisos = False Then
            AlertaJS(Constantes.M_STR_MENSAJE_USUARIO_NO_TIENE_PERMISOS)
            Return
        End If


        If nSeleccionadas = 0 Then
            AlertaJS("Debe seleccionar al menos una solicitud.")
        End If

        Dim chk As CheckBox

        While iCont >= 0
            If dgLista.Rows(iCont).FindControl("chkOrden").GetType Is GetType(CheckBox) Then
                chk = CType(dgLista.Rows(iCont).FindControl("chkOrden"), CheckBox)
                If chk.Checked = True Then

                    Dim oRow As SolicitudReversionBE.SolicitudReversionRow
                    oSolicitudReversionBE = VistaDetalleSolicitudReversion
                    oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(iCont), SolicitudReversionBE.SolicitudReversionRow)
                    'oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(iContListadoTotal), SolicitudReversionBE.SolicitudReversionRow)
                    idSolicitudReversion = oRow.ID

                    Dim resultadoComparacionRegistroAprobado As Integer
                    resultadoComparacionRegistroAprobado = String.Compare(oRow.FlagAprobado, "P")

                    If resultadoComparacionRegistroAprobado = 0 Then
                        'procesar con la aprobacion del registro

                        If hdMotivoRechazo.Value.Trim.ToString.Length > 500 Then
                            hdMotivoRechazo.Value = hdMotivoRechazo.Value.Trim.ToString.Substring(0, 500)
                        End If

                        oSolicitudReversionBM.Aprobar(idSolicitudReversion, "R", hdMotivoRechazo.Value.Trim, DatosRequest)
                        desproboSolicitud = True


                        ViewState("IndiceSeleccionado") = Nothing

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim nomFondo As String = ""
                        nomFondo = dgLista.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NOMBRE_FONDO", dgLista)).Text()
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim toUser As String = ""
                        Dim dtDes As DataTable
                        dtDes = oParametrosGeneralesBM.SeleccionarPorFiltro(APROBACION_SOLICITUD_REVERSION_EMAIL, "", "", "", DatosRequest)
                        For Each fila As DataRow In dtDes.Rows
                            toUser = toUser + fila("Valor") + ";"
                        Next

                        Dim asunto As String = ""
                        asunto = "Rechazo de Reversión del VC Fondo "

                        asunto = asunto + nomFondo + " Fecha: Del " + oRow.FechaInicial + " al " + oRow.FechaFinal
                        Dim message As String = ""

                        Dim resultadoComparacion As Integer
                        resultadoComparacion = String.Compare(oRow.FlagAfectaValorCuota, "S")

                        If resultadoComparacion = 0 Then
                            message = "Se comunica que se ha rechazado una solicitud de reversión que afecta el valor cuota. "
                        Else
                            message = "Se comunica que se ha rechazado una solicitud de reversión que no afecta el valor cuota. "
                        End If

                        Dim rootPath As String = Request.Url.Host

                        Dim detalle As String = ""
                        detalle += "<li>" & "<b>Motivo:</b> " & oRow.Motivo & "</li>"
                        detalle += "<li>" & "<b>Responsable:</b> " & oRow.Responsable & "</li>"
                        detalle += "<li>" & "<b>Acción Temporal:</b> " & oRow.AccionTemporal & "</li>"
                        detalle += "<li>" & "<b>Solución Definitiva Sugerida:</b> " & oRow.SolucionDefinitivaSugerida & "</li>"
                        detalle += "<li>" & "<b style='color: Red'>Motivo de Rechazo:</b> " & hdMotivoRechazo.Value.Trim & "</li>"

                        If rootPath.ToUpper.Trim <> "LOCALHOST" Then
                            EnviarCorreoNotificacion(asunto, message, detalle)
                        End If

                    End If

                End If
            End If
            iCont = iCont - 1

        End While

        If desproboSolicitud = True Then
            AlertaJS("Se rechazaron las solicitudes seleccionadas.")
        End If


        cargarRegistro()
        CargarGrilla()
    End Sub
End Class

