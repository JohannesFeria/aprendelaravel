Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports CrystalDecisions.Shared
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Inversiones_frmBusquedaFirmaDocumento
    Inherits BasePage

#Region "Variables"
    Dim oParametrosGenerales As New ParametrosGeneralesBM
    Dim oCargoFirmante As New CargoFirmanteBM
    Dim oFirmaDocumento As New FirmaDocumentoBM
    Dim oFirmaDocumentoDet As New FirmaDocumentoDetBM
    Dim objutil As New UtilDM
    Dim login As String = ""
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoOrden.Text = CType(Session("SS_DatosModal"), String())(0)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

#Region "Eventos de la Pagina"
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        CargarGrilla()
        If ddlReporte.SelectedValue = ParametrosSIT.REPORTE_LLAMADO_OI Then
            If dgLista1.Rows.Count > 0 Then
                GenerarReporteVerificacion(ddlReporte.SelectedValue)
            End If
        Else
            If dgLista2.Rows.Count > 0 Then
                GenerarReporteVerificacion(ddlReporte.SelectedValue)
            Else
                If ddlCategoria.SelectedValue = ParametrosSIT._REPORTE_OPE_EJECUTADAS.RentaFija Or ddlCategoria.SelectedValue = ParametrosSIT._REPORTE_OPE_EJECUTADAS.Divisas Then
                    AlertaJS("Todos los llamados relacionados al reporte deben estar firmados\ncon el cargo V B Liquidación Tesorería (Administrador).")
                End If
            End If
        End If
    End Sub

    Protected Sub btnFirmar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirmar.Click
        Dim clave As String = ""
        login = IIf(Session("Login") Is Nothing, "", Session("Login"))
        clave = tbClave.Text.ToString()
        If VerificaPermisoFirma(clave) = False Then
            AlertaJS("Usted no tiene permisos para realizar esta operación!")
            Exit Sub
        End If

        Dim codReporte As String = IIf(ViewState("codReporte") Is Nothing, "", ViewState("codReporte"))
        Dim cantDocumentos As Decimal = 0
        Dim Mensaje As String = ""
        Dim ordenInversion As String = ""
        If Not String.IsNullOrEmpty(codReporte) Then
            Dim codFirmaDocumento As Decimal = -1
            Dim validaFirma As Decimal = 0
            If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
                For Each fila As DataGridItem In dgLista1.Rows
                    If fila.ItemType = ListItemType.Item Or fila.ItemType = ListItemType.AlternatingItem Then
                        Dim chkSeleccion1 As CheckBox = CType(fila.FindControl("chkSeleccion1"), CheckBox)
                        Dim hdCodFirmaDocumento1 As HtmlInputHidden = CType(fila.FindControl("hdCodFirmaDocumento1"), HtmlInputHidden)
                        If chkSeleccion1.Checked Then
                            codFirmaDocumento = Convert.ToDecimal(hdCodFirmaDocumento1.Value)
                            oFirmaDocumentoDet.Insertar(codFirmaDocumento, login, validaFirma, DatosRequest)
                            If validaFirma = 2 Then
                                ordenInversion = ordenInversion & ", " & fila.Cells(11).Text
                            Else
                                If Convert.ToBoolean(validaFirma) Then
                                    cantDocumentos = cantDocumentos + 1
                                End If
                            End If
                        End If
                    End If
                Next
            Else
                For Each fila As DataGridItem In dgLista2.Rows
                    If fila.ItemType = ListItemType.Item Or fila.ItemType = ListItemType.AlternatingItem Then
                        Dim chkSeleccion2 As CheckBox = CType(fila.FindControl("chkSeleccion2"), CheckBox)
                        Dim hdCodFirmaDocumento2 As HtmlInputHidden = CType(fila.FindControl("hdCodFirmaDocumento2"), HtmlInputHidden)
                        If chkSeleccion2.Checked Then
                            codFirmaDocumento = Convert.ToDecimal(hdCodFirmaDocumento2.Value)
                            oFirmaDocumentoDet.Insertar(codFirmaDocumento, login, validaFirma, DatosRequest)
                            If Convert.ToBoolean(validaFirma) Then
                                cantDocumentos = cantDocumentos + 1
                            End If
                        End If
                    End If
                Next
            End If
            CargarGrilla()
            tbClave.Text = ""
            Mensaje = "Se ha realizado la firma de " + Convert.ToString(cantDocumentos) + " documento(s).\n"
            If ordenInversion.Length > 0 And codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
                ordenInversion = ordenInversion.Substring(2, ordenInversion.Length - 2)
                Mensaje = Mensaje & "La(s) Orden(es) " & ordenInversion & ", no se ha(n) firmado porque\nfalta la firma de " & FIRMA_ADMINISTRADOR & " (Administrador)."
            End If
            AlertaJS(Mensaje)
        End If
    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim clave As String = ""
        login = IIf(Session("Login") Is Nothing, "", Session("Login")) 'Se obtiene el usuario de login
        clave = tbClave.Text.ToString()
        If VerificaPermisoFirma(clave) = False Then
            AlertaJS("Usted no tiene permisos para realizar esta operación!")
            Exit Sub
        End If

        Dim codReporte As String = IIf(ViewState("codReporte") Is Nothing, "", ViewState("codReporte"))
        If Not String.IsNullOrEmpty(codReporte) Then
            Dim codFirmaDocumento As Decimal = -1
            If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
                For Each fila As DataGridItem In dgLista1.Rows
                    If fila.ItemType = ListItemType.Item Or fila.ItemType = ListItemType.AlternatingItem Then
                        Dim chkSeleccion1 As CheckBox = CType(fila.FindControl("chkSeleccion1"), CheckBox)
                        Dim hdCodFirmaDocumento1 As HtmlInputHidden = CType(fila.FindControl("hdCodFirmaDocumento1"), HtmlInputHidden)
                        If chkSeleccion1.Checked Then
                            codFirmaDocumento = Convert.ToDecimal(hdCodFirmaDocumento1.Value)
                            oFirmaDocumentoDet.Eliminar(codFirmaDocumento, login, DatosRequest)
                        End If
                    End If
                Next
            Else
                For Each fila As DataGridItem In dgLista2.Rows
                    If fila.ItemType = ListItemType.Item Or fila.ItemType = ListItemType.AlternatingItem Then
                        Dim chkSeleccion2 As CheckBox = CType(fila.FindControl("chkSeleccion2"), CheckBox)
                        Dim hdCodFirmaDocumento2 As HtmlInputHidden = CType(fila.FindControl("hdCodFirmaDocumento2"), HtmlInputHidden)
                        If chkSeleccion2.Checked Then
                            codFirmaDocumento = Convert.ToDecimal(hdCodFirmaDocumento2.Value)
                            oFirmaDocumentoDet.Eliminar(codFirmaDocumento, login, DatosRequest)
                        End If
                    End If
                Next
            End If
            CargarGrilla()
            tbClave.Text = ""
            AlertaJS("Se ha realizado los cambios satisfactoriamente")
        End If
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Protected Sub ddlReporte_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlReporte.SelectedIndexChanged
        Dim codReporte As String
        Dim codCategReporte As String
        codReporte = ddlReporte.SelectedValue
        CargarCategoria(codReporte)
        codCategReporte = ddlCategoria.SelectedValue
        CargarCargoFirmante(codReporte, codCategReporte)

        trCodigoOrden.Visible = False
        trPortafolio.Visible = False
        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            trCodigoOrden.Visible = True
            lblTipoReporte.InnerText = "Reporte de :"
        Else
            If codReporte = ParametrosSIT.REPORTE_OPE_EJE Then
                trPortafolio.Visible = True
                CargarPortafolio()
            End If
            lblTipoReporte.InnerText = "Categoría Reporte :"
        End If

    End Sub

    Protected Sub ddlCategoria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCategoria.SelectedIndexChanged
        Dim codReporte As String
        Dim codCategReporte As String
        codReporte = ddlReporte.SelectedValue
        codCategReporte = ddlCategoria.SelectedValue
        If codReporte <> ParametrosSIT.REPORTE_LLAMADO_OI Then
            CargarCargoFirmante(codReporte, codCategReporte)
            ddlCargoFirmante.Enabled = True
            If ddlCategoria.SelectedValue = String.Empty Then
                ddlCargoFirmante.Enabled = False
            End If
        End If
    End Sub
#End Region

#Region "Metodos Personalizados"
    Private Sub CargarPagina()
        login = IIf(Session("Login") Is Nothing, "", Session("Login"))

        Dim codReporte As String
        Dim codCategReporte As String
        trCodigoOrden.Visible = False
        trPortafolio.Visible = False

        CargarFecha()
        CargarReporte()
        CargarPortafolio()
        codReporte = ddlReporte.SelectedValue

        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            ddlCargoFirmante.Enabled = True
            trCodigoOrden.Visible = True
            lblTipoReporte.InnerText = "Reporte de :"
        End If

        If codReporte = ParametrosSIT.REPORTE_OPE_EJE Then
            trPortafolio.Visible = True
            lblTipoReporte.InnerText = "Categoría Reporte :"
        End If

        CargarCategoria(codReporte)
        codCategReporte = ddlCategoria.SelectedValue
        CargarCargoFirmante(codReporte, codCategReporte)
        CargarEstado(codReporte)
        CargarMercado()
        CargarOperacion()
        'btnBuscar.Attributes.Add("onclick", "javascript:showPopupOrdenInversion();") #Miguel
        HabilitarBandeja(codReporte)
    End Sub

    Private Sub CargarReporte()
        Dim oDt As New DataTable
        oDt = oParametrosGenerales.SeleccionarPorFiltro(ParametrosSIT.REPORTE_FIRMA, String.Empty, String.Empty, String.Empty, DatosRequest)
        HelpCombo.LlenarComboBox(ddlReporte, oDt, "Valor", "Nombre", False)
        ddlReporte.SelectedIndex = -1
    End Sub

    Private Sub CargarCategoria(ByVal codReporte As String)
        Dim oDt As New DataTable
        oDt = oParametrosGenerales.SeleccionarCategReporte(String.Empty, codReporte, DatosRequest).Tables(0)
        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            HelpCombo.LlenarComboBox(ddlCategoria, oDt, "Valor", "Categoria", True, "TODOS")
        Else
            HelpCombo.LlenarComboBox(ddlCategoria, oDt, "Valor", "Categoria", False)
        End If
        ddlCategoria.SelectedIndex = -1
    End Sub

    Private Sub CargarCargoFirmante(ByVal codReporte As String, ByVal codCategReporte As String)
        Dim oDt As New DataTable
        Dim oCargoFirmanteBE As New CargoFirmanteBE
        Dim oCargoFirmanteRow As CargoFirmanteBE.CargoFirmanteRow
        oCargoFirmanteRow = CType(oCargoFirmanteBE.CargoFirmante.NewRow(), CargoFirmanteBE.CargoFirmanteRow)
        oCargoFirmante.InicializarCargoFirmante(oCargoFirmanteRow)
        oCargoFirmanteRow.CodReporte = codReporte
        oCargoFirmanteRow.CodCategReporte = codCategReporte
        oCargoFirmanteRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oCargoFirmanteBE.CargoFirmante.AddCargoFirmanteRow(oCargoFirmanteRow)
        oCargoFirmanteBE.AcceptChanges()
        oDt = oCargoFirmante.SeleccionarPorFiltro(oCargoFirmanteBE, True, DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlCargoFirmante, oDt, "CodCargoFirmante", "Nombre", True, "TODOS")

        Dim codigoInterno As String
        Dim oAprobadorDocumento As New AprobadorDocumentoBM
        Dim oAprobadorDocumentoBE As New AprobadorDocumentoBE
        Dim oAprobadorDocumentoRow As AprobadorDocumentoBE.AprobadorDocumentoRow
        Dim bAdministrador As Boolean = False
        Dim bFirmante As Boolean = False

        ddlCargoFirmante.SelectedIndex = -1
        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            login = IIf(Session("Login") Is Nothing, "", Session("Login")) 'Se obtiene el usuario de login
            codigoInterno = New PersonalBM().SeleccionarCodigoInterno(login, DatosRequest).ToString()
            oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.NewRow(), AprobadorDocumentoBE.AprobadorDocumentoRow)
            oAprobadorDocumento.InicializarAprobadorDocumento(oAprobadorDocumentoRow)
            oAprobadorDocumentoRow.CodigoInterno = codigoInterno
            oAprobadorDocumentoBE.AprobadorDocumento.AddAprobadorDocumentoRow(oAprobadorDocumentoRow)
            oAprobadorDocumentoBE.AcceptChanges()
            oDt = oAprobadorDocumento.SeleccionarPorFiltro(oAprobadorDocumentoBE, DatosRequest).Tables(0)
            If oDt.Rows.Count > 0 Then
                bAdministrador = CType(oDt.Rows(0)("Administrador"), Boolean)
                bFirmante = CType(oDt.Rows(0)("Firmante"), Boolean)
            End If
            If bAdministrador Then
                ddlCargoFirmante.SelectedValue = ParametrosSIT.CARGO_FIRMANTE_VB_LIQTES
            ElseIf bFirmante Then
                ddlCargoFirmante.SelectedValue = ParametrosSIT.CARGO_FIRMANTE_VB_GERGESINV
            End If
        End If
    End Sub

    Private Sub CargarFecha()
        tbFecha.Text = objutil.RetornarFechaNegocio
    End Sub

    Private Sub HabilitarBandeja(ByVal codReporte As String)
        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            divBandeja1.Visible = True
            divBandeja2.Visible = False

            dgLista2.DataSource = Nothing
            dgLista2.DataBind()
        Else
            divBandeja1.Visible = False
            divBandeja2.Visible = True

            dgLista1.DataSource = Nothing
            dgLista1.DataBind()
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oDt As New DataTable
        Dim fecha As Decimal
        Dim codReporte As String
        Dim codCategReporte As String
        Dim codCargoFirmante As String
        Dim decCargoFirmante As Decimal
        Dim codigoOrden As String = String.Empty
        Dim portafolio As String = String.Empty
        Dim mercado As String = String.Empty
        Dim operacion As String = String.Empty
        Dim estado As String = String.Empty

        fecha = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
        codReporte = ddlReporte.SelectedValue
        codCategReporte = ddlCategoria.SelectedValue
        codCargoFirmante = ddlCargoFirmante.SelectedValue

        If codCargoFirmante = "" Then
            decCargoFirmante = -1
        Else
            decCargoFirmante = CType(codCargoFirmante, Decimal)
        End If

        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            codigoOrden = tbCodigoOrden.Text
            mercado = ddlMercado.SelectedValue
            operacion = ddlOperacion.SelectedValue
        End If

        If codReporte = ParametrosSIT.REPORTE_OPE_EJE Then
            portafolio = ddlPortafolio.SelectedValue
        End If
        estado = ddlEstado.SelectedValue

        oDt = oFirmaDocumento.SeleccionarPorFiltro(fecha, codReporte, codCategReporte, _
                                                   decCargoFirmante, codigoOrden, portafolio, estado, mercado, operacion, DatosRequest).Tables(0)

        If codReporte = ParametrosSIT.REPORTE_LLAMADO_OI Then
            dgLista1.DataSource = oDt
            dgLista1.DataBind()
        Else
            dgLista2.DataSource = oDt
            dgLista2.DataBind()
        End If
        HabilitarBandeja(codReporte)
        ViewState("codReporte") = codReporte
        Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(oDt)
    End Sub

    Private Sub CargarPortafolio()
        Dim dsPortafolio As DataSet
        Dim oPortafolioBM As New PortafolioBM
        dsPortafolio = oPortafolioBM.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
        Dim dtPortafolio As DataTable = dsPortafolio.Tables(0)
        dtPortafolio.DefaultView.RowFilter = "CodigoPortafolioSBS<>'MULTIFONDO'"
        If ddlReporte.SelectedValue = ParametrosSIT.REPORTE_OPE_EJE Then
            HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolio.DefaultView.Table, "CodigoPortafolioSBS", "Descripcion", True, "TODOS")
        End If
    End Sub

    Private Sub CargarEstado(ByVal codReporte As String)
        Dim oDt As New DataTable
        oDt = oParametrosGenerales.SeleccionarPorFiltro(ParametrosSIT.ESTADO_FIRMA_DOC, String.Empty, String.Empty, String.Empty, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, oDt, "Valor", "Nombre", True, "TODOS")
        ddlEstado.SelectedValue = ParametrosSIT.ESTADO_FIRMA_DOC_PF
    End Sub

    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.Listar(DatosRequest)
            ddlMercado.Items.Clear()
            ddlMercado.DataSource = dsMercado
            ddlMercado.DataValueField = "CodigoMercado"
            ddlMercado.DataTextField = "Descripcion"
            ddlMercado.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMercado, "", "--TODOS--")
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado, "", "--TODOS--")
        End If
        ddlMercado.Enabled = enabled
        ddlMercado.SelectedIndex = -1
    End Sub

    Private Sub CargarOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsOperacion As DataSet = oOperacion.ListaOperacionLlamado()
            ddlOperacion.Items.Clear()
            ddlOperacion.DataSource = dsOperacion
            ddlOperacion.DataValueField = "CodigoOperacion"
            ddlOperacion.DataTextField = "Descripcion"
            ddlOperacion.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlOperacion, "", "--TODOS--")
        Else
            ddlOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacion, "", "--TODOS--")
        End If
        ddlOperacion.Enabled = enabled
    End Sub

    Private Function VerificaPermisoFirma(ByVal clave As String) As Boolean
        Dim validaUsuario As Decimal

        validaUsuario = oFirmaDocumento.VerificaPermisoFirma(login, clave, DatosRequest)

        Return Convert.ToBoolean(validaUsuario)
    End Function

    Private Sub GenerarReporteVerificacion(ByVal codReporte As String)
        Dim opcion As String
        Dim portafolio As String
        Dim rutaArchivo As String = ""
        Select Case codReporte
            Case ParametrosSIT.REPORTE_LLAMADO_OI
                Dim codCargoFirmante As String = ddlCargoFirmante.SelectedValue
                Dim codPortafolioSBS As String = ddlPortafolio.SelectedValue
                Dim codigoOrden As String = tbCodigoOrden.Text
                Dim codigoMercado As String = ddlMercado.SelectedValue
                Dim codigoOperacion As String = ddlOperacion.SelectedValue
                Dim estadoFirma As String = ddlEstado.SelectedValue
                Dim fechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
                Dim codigoUsuario As String = Usuario.ToString()
                Dim decCargoFirmante As Decimal = Convert.ToDecimal(IIf(codCargoFirmante.Equals(String.Empty), -1, codCargoFirmante))
                Dim codCategReporte As String = ddlCategoria.SelectedValue
                EjecutarJS(UIUtility.MostrarPopUp("Reportes/frmVisorVerificacionFirmasOI.aspx?vCodCargoFirmante=" & decCargoFirmante.ToString() _
                                                                                            & "&vFechaOperacion=" & fechaOperacion.ToString() _
                                                                                            & "&vCodigoUsuario=" & codigoUsuario _
                                                                                            & "&vCodPortafolioSBS=" & codPortafolioSBS _
                                                                                            & "&vCodigoMercado=" & codigoMercado _
                                                                                            & "&vCodigoOperacion=" & codigoOperacion _
                                                                                            & "&vCodigoOrden=" & codigoOrden _
                                                                                            & "&vEstadoFirma=" & estadoFirma _
                                                                                            & "&vCodCategReporte=" & codCategReporte, "no", 1010, 670, 0, 0, "no", "yes", "yes", "yes"), False)
            Case ParametrosSIT.REPORTE_OPE_EJE
                opcion = ddlCategoria.SelectedItem.Text
                portafolio = Me.ddlPortafolio.SelectedValue
                Session("titulo") = opcion

                Dim fechaNegocio As String = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio("MULTIFONDO"))
                EjecutarJS(UIUtility.MostrarPopUp("Reportes/Orden de Inversion/frmVisorReporteOperacionesEjecutadas.aspx?Finicio=" + fechaNegocio + "&FFin=" + fechaNegocio + "&Portafolio=" + portafolio + "&VFirma=1", "no", 800, 600, 5, 5, "no", "yes", "yes", "yes"), False)

            Case ParametrosSIT.REPORTE_OPE_MASIVAS
                opcion = ddlCategoria.SelectedValue

                Select Case opcion
                    Case ParametrosSIT._REPORTE_OPE_MASIVAS.RentaVariable
                        rutaArchivo = GenerarReporteRenta.GenerarReporteRentaVariablePDF(ParametrosSIT._REPORTE_OPE_MASIVAS.RentaVariable, Usuario, DatosRequest)
                    Case ParametrosSIT._REPORTE_OPE_MASIVAS.AsignaFondos
                        rutaArchivo = GenerarReporteRenta.GenerarReporteRentaVariableAFPDF(ParametrosSIT._REPORTE_OPE_MASIVAS.AsignaFondos, Usuario, DatosRequest)
                    Case ParametrosSIT._REPORTE_OPE_MASIVAS.RentaFija
                        rutaArchivo = GenerarReporteRenta.GenerarReporteRentaFijaPDF(ParametrosSIT._REPORTE_OPE_MASIVAS.RentaFija, Usuario, DatosRequest)
                    Case ParametrosSIT._REPORTE_OPE_MASIVAS.OperacionesFx
                        rutaArchivo = GenerarReporteRenta.GenerarReporteFXPDF(ParametrosSIT._REPORTE_OPE_MASIVAS.OperacionesFx, Usuario, DatosRequest)
                End Select

                If rutaArchivo <> "" Then
                    If IO.File.Exists(rutaArchivo) Then
                        EjecutarJS("<script language='javascript'>window.open('frmVisorIngresoMasivoOI.aspx?rutaReporte=" + rutaArchivo.Replace("\", "\\").Replace("/", "//") + "');</script>", False)
                    Else
                        AlertaJS("Error: El reporte no ha sido generado satisfactoriamente")
                    End If
                End If
        End Select

    End Sub

#End Region

End Class
