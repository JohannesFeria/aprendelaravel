Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT
Imports SistemaProcesosBL

Partial Class Modulos_Contabilidad_frmSolicitudReversion
    Inherits BasePage
#Region "/* Enumerados */"

    Public Enum TIPO_ACCION As Byte
        [NUEVO] = 1
        [MODIFICAR] = 2
    End Enum

#End Region
    Private oSolicitudReversionBE As New SolicitudReversionBE
    Private oSolicitudReversionBM As New SolicitudReversionBM

    Private Property VistaDetalleSolicitudReversion() As SolicitudReversionBE
        Get
            Return DirectCast(ViewState("SolicitudReversion"), SolicitudReversionBE)
        End Get
        Set(ByVal Value As SolicitudReversionBE)
            ViewState("SolicitudReversion") = Value
        End Set
    End Property

    Private Property IdRegistro As Int16


#Region " /* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            End If

            Dim intIndice As Integer
            intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
            If intIndice = 0 Then
                btnAgregarDetalle.Visible = True
            Else
                btnAgregarDetalle.Visible = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        Dim objReversion As New ReversionBL(Usuario.ToString.Trim)
        Dim idFondo As String = ddlFondo.SelectedValue.Trim
        If idFondo <> String.Empty Then
            Dim fechaReversion As String = objReversion.ObtenerFechaReversion(Convert.ToInt32(idFondo))
            If fechaReversion = String.Empty Then
                AlertaJS("El fondo no se puede revertir. No existe precierre para el fondo seleccionado.")
            Else
                tbFechaFinal.Text = Convert.ToDateTime(fechaReversion).ToShortDateString()
                tbFechaInicial.Text = Convert.ToDateTime(fechaReversion).ToShortDateString()
            End If

        End If
    End Sub
    Private Sub CargarCombos()
        Dim tablaFondo As New DataTable
        Dim oFondo As New FondoBM
        tablaFondo = oFondo.ListarPrecierreFondos()
        HelpCombo.LlenarComboBox(ddlFondo, tablaFondo, "ID_FONDO", "NOMBRE", True, "")

        Dim tablaArea As New DataTable
        Dim oSolicitudReversion As New SolicitudReversionBM
        tablaArea = oSolicitudReversion.ObtenerAreaUsuarioSisOpe()
        HelpCombo.LlenarComboBox(ddlArea, tablaArea, "LLAVE_TABLA", "DESCRIPCION_LARGA", True, "")

    End Sub
#End Region

    Protected Sub btnAgregarDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarDetalle.Click
        If Validar() Then
            Insertar()
        End If
    End Sub

    Private Sub Insertar()

        oSolicitudReversionBE = crearObjeto()
        oSolicitudReversionBM.Insertar(oSolicitudReversionBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)

        cargarRegistro()

        Dim toUser As String = ""
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As DataTable
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(SOLICITUD_REVERSION_EMAIL, "", "", "", DatosRequest)
        For Each fila As DataRow In dt.Rows
            toUser = toUser + fila("Valor") + ";"
        Next

        Dim asunto As String = ""
        asunto = "Solicitud de Reversión del VC Fondo "
        asunto = asunto + ddlFondo.SelectedItem.Text + " Fecha: Del " + tbFechaInicial.Text + " al " + tbFechaFinal.Text
        Dim message As String = ""

        Dim afectaValorCuota As String = ""
        afectaValorCuota = Me.RbAfectaValorCuota.SelectedValue

        Dim resultadoComparacion As Integer
        resultadoComparacion = String.Compare(afectaValorCuota, "S")

        If resultadoComparacion = 0 Then
            message = "Se comunica que se ha ingresado una solicitud de reversión que afecta  el valor cuota. "
        Else
            message = "Se comunica que se ha ingresado una solicitud de reversión que no afecta  el valor cuota. "
        End If

        Dim rootPath As String = Request.Url.Host

        Dim detalle As String = ""
        Dim oRow As SolicitudReversionBE.SolicitudReversionRow
        oRow = DirectCast(oSolicitudReversionBE.SolicitudReversion.Rows(0), SolicitudReversionBE.SolicitudReversionRow)

        detalle += "<li>" & "<b>Motivo:</b> " & oRow.Motivo & "</li>"
        detalle += "<li>" & "<b>Responsable:</b> " & oRow.Responsable & "</li>"
        detalle += "<li>" & "<b>Acción Temporal:</b> " & oRow.AccionTemporal & "</li>"
        detalle += "<li>" & "<b>Solución Definitiva Sugerida:</b> " & oRow.SolucionDefinitivaSugerida & "</li>"

        If rootPath.ToUpper.Trim <> "LOCALHOST" Then
            EnviarCorreoNotificacion(asunto, message, detalle)
        End If

        LimpiarCampos()

    End Sub

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

        oRow.IdFondo = ddlFondo.SelectedValue
        oRow.FechaInicial = UIUtility.ConvertirFechaaDecimal(tbFechaInicial.Text)
        oRow.FechaFinal = UIUtility.ConvertirFechaaDecimal(tbFechaFinal.Text)
        oRow.codigoArea = ddlArea.SelectedValue

        If txtMotivo.Text.Trim.ToString.Length > 250 Then
            oRow.Motivo = txtMotivo.Text.Trim.ToString.Substring(0, 250)
        Else
            oRow.Motivo = txtMotivo.Text.Trim.ToString
        End If

        If txtResponsable.Text.Trim.ToString.Length > 500 Then
            oRow.Responsable = txtResponsable.Text.Trim.ToString.Substring(0, 500)
        Else
            oRow.Responsable = txtResponsable.Text.Trim.ToString
        End If

        If txtAccionTemporal.Text.Trim.ToString.Length > 500 Then
            oRow.AccionTemporal = txtAccionTemporal.Text.Trim.ToString.Substring(0, 500)
        Else
            oRow.AccionTemporal = txtAccionTemporal.Text.Trim.ToString
        End If

        If txtSolucionDefinitivaSugerida.Text.Trim.ToString.Length > 500 Then
            oRow.SolucionDefinitivaSugerida = txtSolucionDefinitivaSugerida.Text.Trim.ToString.Substring(0, 500)
        Else
            oRow.SolucionDefinitivaSugerida = txtSolucionDefinitivaSugerida.Text.Trim.ToString
        End If

        oRow.FlagAfectaValorCuota = Me.RbAfectaValorCuota.SelectedValue
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

        ddlFondo.SelectedValue = oSolicitudReversionBE.SolicitudReversion(index)("IdFondo").ToString()
        ddlArea.SelectedValue = oSolicitudReversionBE.SolicitudReversion(index)("codigoArea").ToString()

        tbFechaInicial.Text = oSolicitudReversionBE.SolicitudReversion(index)("FechaInicial").ToString()
        tbFechaFinal.Text = oSolicitudReversionBE.SolicitudReversion(index)("FechaFinal").ToString()
        txtMotivo.Text = oSolicitudReversionBE.SolicitudReversion(index)("Motivo").ToString()
        txtResponsable.Text = oSolicitudReversionBE.SolicitudReversion(index)("Responsable").ToString()
        txtAccionTemporal.Text = oSolicitudReversionBE.SolicitudReversion(index)("AccionTemporal").ToString()
        txtSolucionDefinitivaSugerida.Text = oSolicitudReversionBE.SolicitudReversion(index)("SolucionDefinitivaSugerida").ToString()
        RbAfectaValorCuota.SelectedValue = oSolicitudReversionBE.SolicitudReversion(index)("FlagAfectaValorCuota").ToString()

        ViewState("IndiceSeleccionado") = index

        btnAgregarDetalle.Visible = False

    End Sub

    Private Sub Modificar()
        oSolicitudReversionBE = crearObjeto()

        Dim intIndice As Integer
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))

        oSolicitudReversionBM.Modificar(oSolicitudReversionBE, intIndice, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)

        cargarRegistro()
        LimpiarCampos()

        ViewState("IndiceSeleccionado") = Nothing

        btnAgregarDetalle.Visible = True

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

    Private Sub LimpiarCampos()
        ddlFondo.SelectedValue = ""
        ddlArea.SelectedValue = ""
        tbFechaInicial.Text = ""
        tbFechaFinal.Text = ""
        txtMotivo.Text = ""
        txtResponsable.Text = ""
        txtAccionTemporal.Text = ""
        txtSolucionDefinitivaSugerida.Text = ""
        RbAfectaValorCuota.SelectedIndex = -1
    End Sub

    Private Function Validar() As Boolean
        Dim strMsjCampOblig As String = String.Empty
        Dim tipoTitulo As String = String.Empty
        Dim resultadoValidacion As Boolean = True

        Dim strMsjValidacionFechas As String = String.Empty
        strMsjValidacionFechas = ValidarFechas()

        If strMsjValidacionFechas.Length > 0 Then
            resultadoValidacion = False
            strMsjCampOblig = strMsjCampOblig + strMsjValidacionFechas
        Else
            Dim arrayMotivo() As String = txtMotivo.Text.Split(" ")
            If (txtMotivo.Text.Length < 10 Or arrayMotivo.Length <= 2) Then
                resultadoValidacion = False
                strMsjCampOblig = "Debe indicar mayor detalle en el motivo de la reversión."
            End If
        End If
        If Not resultadoValidacion Then
            AlertaJS(strMsjCampOblig)
        End If

        Return resultadoValidacion

    End Function

    Private Sub EnviarCorreoNotificacion(ByVal accionTitulo As String, ByVal accionDescripcion As String, ByVal descripcion As String)
        Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String)
        dict.Add("@Detalles", descripcion)
        dict.Add("@AccionTitulo", accionTitulo)
        dict.Add("@AccionDescripcion", accionDescripcion)

        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        Dim dtPara As DataTable = oParametrosGeneralesBM.SeleccionarPorFiltro("SOLICITUD_REVERSION_EMAIL", "PARA", "", "", DatosRequest)
        Dim dtCopia As DataTable = oParametrosGeneralesBM.SeleccionarPorFiltro("SOLICITUD_REVERSION_EMAIL", "COPIA", "", "", DatosRequest)

        Dim paraCorreo As String = ""
        Dim copiaCorreo As String = ""

        If dtPara.Rows.Count > 0 Then
            paraCorreo = dtPara.Rows(0)("Valor").ToString()
        End If

        If dtCopia.Rows.Count > 0 Then
            copiaCorreo = dtCopia.Rows(0)("Valor").ToString()
        End If

        Dim archivoCodigo As String
        archivoCodigo = "029"

        If paraCorreo <> "" Then
            UIUtility.EnviarMailPlantilla_X_ArchivoCodigo(paraCorreo, copiaCorreo, accionTitulo, "SolicitudReversion.html", dict, archivoCodigo, DatosRequest)
        End If

    End Sub

    Private Function ValidarFechas() As String
        Dim fecIni As Int64 = 0
        Dim fecFin As Int64 = 0

        If tbFechaInicial.Text.Length > 0 Then
            fecIni = UIUtility.ConvertirFechaaDecimal(tbFechaInicial.Text)
        End If

        If tbFechaFinal.Text.Length > 0 Then
            fecFin = UIUtility.ConvertirFechaaDecimal(tbFechaFinal.Text)
        End If

        Dim strMsjCampOblig As String = ""

        If tbFechaInicial.Text.Length > 0 And tbFechaFinal.Text.Length > 0 Then
            If fecIni > fecFin Then
                strMsjCampOblig = "La fecha final debe ser mayor o igual que la fecha inicial. <br />"
            End If
        End If

        Return strMsjCampOblig

    End Function


End Class
