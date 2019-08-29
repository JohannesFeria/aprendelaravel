Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT
Imports SistemaProcesosBL
Imports SistemaProcesosTD.Constantes

Partial Class Modulos_Contabilidad_frmReversarValorCuota
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
    Private msgGral As StringBuilder
    Private msg As String = String.Empty, mensajeDetalle As String = String.Empty, msgErrorEmail As String = String.Empty


#Region " /* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Not Page.IsPostBack Then
                If Not Request.QueryString("cod") Is Nothing Then
                    'modificar
                    hd.Value = Request.QueryString("cod")
                Else
                    'crear
                    hd.Value = String.Empty
                    VistaDetalleSolicitudReversion = New SolicitudReversionBE
                    cargarRegistro()
                End If
                hdRptaConfirmar.Value = "NO"
                CargarGrilla()
            End If

            Dim intIndice As Integer
            intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página / " + ex.Message.ToString)
        End Try
    End Sub

#End Region

    Private Sub cargarRegistro()
        oSolicitudReversionBE = oSolicitudReversionBM.SeleccionarSolicitudReversion("A", DatosRequest)
        VistaDetalleSolicitudReversion = oSolicitudReversionBE
    End Sub

    Private Sub EditarGrilla(ByVal id As Int16, ByVal index As Int16)

        oSolicitudReversionBE = VistaDetalleSolicitudReversion

        ViewState("IndiceSeleccionado") = index
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

    Protected Sub PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim iCont As Int64 = dgLista.Rows.Count - 1
        Dim I As Int16 = 0
        Dim Row As GridViewRow
        If e.CommandName = "Seleccionar" Then
            I = dgLista.SelectedIndex
            If I >= 0 And I <= iCont Then
                dgLista.Rows.Item(I).BackColor = System.Drawing.Color.White
            End If
            Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            I = Row.RowIndex
            dgLista.SelectedIndex = Row.RowIndex
            dgLista.Rows.Item(I).BackColor = System.Drawing.Color.LemonChiffon
            ViewState("NombreFondo") = dgLista.Rows(I).Cells(obtenerIndiceColumna_Grilla("NOMBRE_FONDO", dgLista)).Text
            ViewState("FechaInicial") = dgLista.Rows(I).Cells(obtenerIndiceColumna_Grilla("FechaInicial", dgLista)).Text
            ViewState("FechaFinal") = dgLista.Rows(I).Cells(obtenerIndiceColumna_Grilla("FechaFinal", dgLista)).Text
            ViewState("motivo") = dgLista.Rows(I).Cells(obtenerIndiceColumna_Grilla("Motivo", dgLista)).Text
            ViewState("idFondo") = dgLista.Rows(I).Cells(obtenerIndiceColumna_Grilla("IdFondo", dgLista)).Text
            ViewState("idSolicitud") = dgLista.Rows(I).Cells(obtenerIndiceColumna_Grilla("ID", dgLista)).Text
            btnReversar.Enabled = True
        End If
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    cbSelect = CType(e.Row.FindControl("chkOrden"), CheckBox)
        '    hdCodigoEstado = CType(e.Row.FindControl("hdCodigoEstado"), HiddenField)

        '    If hdCodigoEstado.Value = "A" Or hdCodigoEstado.Value = "R" Then
        '        cbSelect.Enabled = False
        '    Else
        '        cbSelect.Enabled = True
        '    End If
        'End If
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Protected Sub btnReversar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReversar.Click

        Dim objReversion As New ReversionBL(Usuario.ToString.Trim)

        ViewState("FechaUltimaCierre") = Convert.ToDateTime(objReversion.ObtenerFechaReversion(Convert.ToInt32(ViewState("idFondo")))).ToShortDateString()
        Dim fechaFinalSolicitud As Date = UIUtility.ConvertirStringaFecha(ViewState("FechaFinal"))
        Dim fechaUltimoCierre As Date = UIUtility.ConvertirStringaFecha(ViewState("FechaUltimaCierre"))
        Dim fechaLimite As Date = UIUtility.ConvertirStringaFecha(ViewState("FechaInicial"))
        Dim fechaReversar As Date

        If fechaUltimoCierre <= fechaFinalSolicitud Then
            fechaReversar = fechaUltimoCierre
        Else
            AlertaJS("La fecha final de reversión (" + fechaFinalSolicitud.ToShortDateString() + ") no puede ser menor a la fecha actual de cierre del fondo (" + ViewState("FechaUltimaCierre") + ")")
            Return
        End If

        If fechaUltimoCierre < fechaLimite Then
            AlertaJS("La fecha Inicial de reversión (" + fechaLimite.ToShortDateString() + ") no puede ser mayor a la fecha actual de cierre del fondo (" + ViewState("FechaUltimaCierre") + ")")
            Return
        End If

        If hdRptaConfirmar.Value.ToUpper = "NO" Then
            ConfirmarJS("¿Está seguro de realizar la reversión? <p align=left>- Fondo: " + ViewState("NombreFondo") _
                        + "<br/>- Actual Fecha Cierre: " + ViewState("FechaUltimaCierre") _
                        + "<br/>- Nueva Fecha Cierre: " + ViewState("FechaInicial") + "</p>", _
                        "document.getElementById('hdRptaConfirmar').value = 'SI'; document.getElementById('btnReversar').click(); ")
        Else
            Try
                hdRptaConfirmar.Value = "NO"
                msgGral = New StringBuilder
                msgError.Text = "<hr/><b><i><u>LOG DE PROCESAMIENTO FONDO: " + ViewState("NombreFondo") + "</b></i></u><br/><br/>"
                Dim nombreUsuario As String = String.Empty

                Dim dtUsuario = objReversion.ObtenerUsuario(Usuario.ToString.Trim)
                Dim motivo As String = ViewState("motivo")
                Dim fondo As String = ViewState("NombreFondo")

                If (dtUsuario.Rows.Count > 0) Then
                    nombreUsuario = dtUsuario.Rows(0)("NOMBRE").ToString() + " " + dtUsuario.Rows(0)("APELLIDO_PATERNO").ToString() + " " + dtUsuario.Rows(0)("APELLIDO_MATERNO").ToString()
                End If

                If (ViewState("idFondo") = "0") Then
                    AlertaJS("Debe seleccionar un fondo para revertir.")
                    Return
                End If

                Dim arrayMotivo() As String = motivo.Split(" ")
                If (motivo.Length < 10 Or arrayMotivo.Length <= 2) Then
                    AlertaJS("Debe indicar mayor detalle en el motivo de la reversión.")
                    Return
                End If

                Dim idFondoEntero As Int32 = Convert.ToUInt32(ViewState("idFondo"))
                Dim enteroFecha As Int32 = 0

                While (fechaReversar >= fechaLimite)
                    msg = String.Empty
                    msgErrorEmail = String.Empty
                    mensajeDetalle = "Fecha: <b>" + fechaReversar.ToShortDateString() + "</b> "
                    enteroFecha = Convert.ToInt32(fechaReversar.ToString("yyyyMMdd"))

                    Dim dtSolicitudReversionAprobada As DataTable = objReversion.ObtenerSolicitudReversionAprobada(idFondoEntero, enteroFecha)
                    If (dtSolicitudReversionAprobada.Rows.Count <= 0) Then
                        msg = "El fondo no se puede revertir. No existe una solicitud de reversión"
                    Else
                        Try
                            objReversion.RevertirPrecierre(idFondoEntero, fechaReversar, Usuario.ToString.Trim, motivo)
                            Try
                                objReversion.ReversionAlertaActividadCliente(idFondoEntero, fechaReversar, Usuario.ToString.ToString)
                            Catch ex As Exception
                                msg = "- No se pudo Realizar el cambio de estado de la Alerta Actividad Cliente. Detalle: " + ex.Message.ToString + "<br>" + ex.StackTrace.ToString
                            End Try

                        Catch ex As Exception
                            msg = "- No se pudo revertir el fondo.<br>Detalle: " + ex.Message.ToString
                        End Try
                    End If
                    If msg = String.Empty Then
                        msgGral.Append(mensajeDetalle + "<p style='color:#0000FF';><b>El fondo se revirtió con éxito.</b></p><br/>")
                    Else
                        msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>" + msg + "</b></p><br/>")
                        Return
                    End If
                    fechaReversar = fechaReversar.AddDays(-1)

                End While

                Dim wsEmail As New FondosSuraWS_Email.EmailManager()
                Dim bo As New PrecierreBO(Usuario.ToString.Trim)
                Dim correoInfoPrecierre As DataTable = bo.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_INFO_PRECIERRE)
                Dim returnValue As Boolean, returnValuesSpecified As Boolean, errorCorreo As Boolean = False
                Dim mensajeAdicional As String = String.Empty

                For Each dr2 As DataRow In correoInfoPrecierre.Rows
                    Try
                        wsEmail.enviarEmail(dr2("DESCRIPCION_LARGA").ToString(), "Reversión de Precierre Fondo " + fondo + " Desde: " + fechaFinalSolicitud.ToShortDateString() + _
                                            "Hasta: " + fechaReversar.ToShortDateString(), _
                                            NotificarPrecierreReversado(fondo, fechaFinalSolicitud, fechaReversar, nombreUsuario, motivo), returnValue, returnValuesSpecified)
                    Catch ex As Exception
                        msgErrorEmail += " - No se pudo enviar el correo a " + dr2("DESCRIPCION_LARGA").ToString() + ". Detalle: " + ex.Message.ToString + "<br>" + ex.StackTrace.ToString
                        errorCorreo = True
                    End Try
                Next

                mensajeAdicional = IIf(errorCorreo, "<br>Se ha encontrado error en el envío de correos.<br>" + msgErrorEmail, String.Empty)

                If mensajeAdicional.Length > 0 Then
                    msgGral.Append("<p style='color:#FF0000';><b>" + mensajeAdicional + "</b></p><br/>")
                End If

            Catch ex As Exception
                msgGral.Append(mensajeDetalle + "<p style='color:#FF0000';><b>Ocurrió un error al realizar el proceso de reversión/ " + ex.Message.ToString() + "<br/> -->>>  [Proceso Abortado] <<<--</b></p>")
            Finally
                AlertaJS("<p align=left>- Se realizó el proceso de reversión del fondo seleccionado.<br>Verificar el Log de Procesamiento.</p>")
                msgError.Text += msgGral.ToString
                If fechaLimite > Convert.ToDateTime(objReversion.ObtenerFechaReversion(Convert.ToInt32(ViewState("idFondo")))) Then
                    oSolicitudReversionBM.Aprobar(CInt(ViewState("idSolicitud")), "E", String.Empty, DatosRequest)
                    AlertaJS("Se Ejecutó la solicitud de reversión del fondo seleccionado")
                End If
                btnReversar.Enabled = False
                cargarRegistro()
                CargarGrilla()
            End Try
        End If

    End Sub
    Private Function NotificarPrecierreReversado(ByVal fondo As String, ByVal fechaIni As DateTime, ByVal fechaFin As DateTime, ByVal usuario As String, ByVal motivo As String) As String
        Dim sb As StringBuilder = New StringBuilder("Se notifica que se ha ejecutado una reversión de precierre.<br>Fondo: ")
        sb.Append(fondo)
        sb.Append("<br>Desde : ")
        sb.Append(fechaFin.ToString("dd/MM/yyyy"))
        sb.Append("<br>Hasta : ")
        sb.Append(fechaIni.ToString("dd/MM/yyyy"))
        sb.Append("<br>Usuario: ")
        sb.Append(usuario)
        sb.Append("<br>Fecha y hora de ejecución: ")
        sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))
        sb.Append("<br>Motivo: ")
        sb.Append(motivo)
        sb.Append("<br><br>Atte.<br><br>")
        Return sb.ToString()
    End Function
End Class

