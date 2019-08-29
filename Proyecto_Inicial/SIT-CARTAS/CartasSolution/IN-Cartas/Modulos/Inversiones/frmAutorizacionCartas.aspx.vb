Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Text
Imports System.Collections
Imports Cartas.BusinessEntities
Imports Cartas.BusinessLayer
Imports System.Data
Imports System.Threading
Imports System.Diagnostics
  
Partial Class Modulos_Tesoreria_OperacionesCaja_frmAutorizacionCartas
    Inherits BasePage

    Const CONST_ARCHIVO_SIN_FIRMA As String = "sin_firma.png"
    Private rutaCartas As String = String.Empty
    Private _strcartas As String = String.Empty
    Dim arrayCodigos As New ArrayList
    Dim oOperacionesCajaBM As New OperacionesCajaBM
    Dim lOperaciones As SeleccionCartaBEList
    'OT 10150 24/03/2017 - Carlos Espejo
    'Descripcion: Se declara la variable a nivel de Clase
    Dim RV As ReportViewer
    'OT 10150 Fin
    'OT 12012 
    Dim ListCodigoOrden As String = String.Empty




#Region "Eventos de la Pagina"
    Sub ClaveFirmante()
        If tbCodAprob.Visible Then
            tbCodAprob.Text = oOperacionesCajaBM.ObtenerClaveFirmantes(Usuario, UIUtility.ConvertirFechaaDecimal(tbFecha.Text))
            If tbCodAprob.Text = "" Then
                lblMensajeClave.Text = "No se ha generado codigo de firmante para esta fecha."
            Else
                lblMensajeClave.Text = ""
            End If
        End If
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Function GeneraReporte(ByVal CodigoOperacionCaja As String, ByVal CodigoOperacion As String, ByVal CodigoModelo As String) As String

        'CRumiche (2019-07-01): Tomamos de parametría la ruta de la carpeta de las imagenes (firmas)
        Dim listaParams As TablaGeneralBEList = (New ParametrosGeneralesBM()).Listar(ParametrosSIT.RUTA_FIRMA_CARTA, DatosRequest)
        If listaParams.Count = 0 Then Throw New Exception("No se encontró el parámetro RUTA_FIRMA_CARTA")
        Dim folderImagenes As String = listaParams(0).Comentario
        If Not folderImagenes.EndsWith("\") Then folderImagenes += "\"

        Dim bytes As Byte()
        Dim rds As ReportDataSource = Nothing
        Dim rds2 As ReportDataSource = Nothing
        Dim dt As DataTable = Nothing
        Dim EsAmpliacionBCR As Boolean = False
        Dim Ruta As String
        'OT 10150 24/03/2017 - Carlos Espejo
        'Descripcion: Se iniciaza cada vez que se llama la procedimiento
        Using RV As New ReportViewer
            'OT 10150 Fin

            'Orden de Inversion Forward
            If (CodigoOperacion = "93" And CodigoModelo = "OFWC") Or (CodigoOperacion = "94" And CodigoModelo = "OFWV") Then
                dt = oOperacionesCajaBM.Cartas_Operacion_Forward(CodigoOperacionCaja)
                rds = New ReportDataSource("DSOperacionForward", dt)

            End If
            'Vecimiento de Orden de Inversion Forward
            If (CodigoOperacion = "93" And CodigoModelo = "FWVC") Or (CodigoOperacion = "94" And CodigoModelo = "FWVV") Then
                dt = oOperacionesCajaBM.Cartas_Operacion_ForwardVcto(CodigoOperacionCaja)
                rds = New ReportDataSource("DSOperacionForwardVcto", dt)

            End If

            If CodigoOperacion = "65" Or CodigoOperacion = "66" Then
                dt = oOperacionesCajaBM.Cartas_Operacion_Cambio(CodigoOperacionCaja)
                rds = New ReportDataSource("DataSet1", dt)

            End If

            If CodigoOperacion = "101" Or (CodigoOperacion = "4" And CodigoModelo = "OPR1") Then
                dt = oOperacionesCajaBM.Cartas_Operacion_Reporte(CodigoOperacionCaja)
                rds = New ReportDataSource("DSOperacionReporte", dt)

            End If

            'Compra/venta Bonos Nacionales/Extranjeros
            If (CodigoModelo = "BO01") Then
                dt = oOperacionesCajaBM.Cartas_Operacion_Bono(CodigoOperacionCaja)
                rds = New ReportDataSource("DSCompraVentaBonos", dt)

            End If

            'Egreso/Ingreso tranferencias al exterior
            If (CodigoOperacion = "63" And CodigoModelo = "TE01") Or (CodigoOperacion = "64" And CodigoModelo = "TE02") Then
                dt = oOperacionesCajaBM.Cartas_Operacion_TransferenciaExterior(CodigoOperacionCaja)
                rds = New ReportDataSource("DSTransferenciaExterior", dt)

            End If

            If CodigoOperacion = "3" Or (CodigoOperacion = "4" And CodigoModelo <> "OPR1") Then
                If oOperacionesCajaBM.EsRenovacion(CodigoOperacionCaja) = "1" Then
                    If oOperacionesCajaBM.AmpliacionBCR(CodigoOperacionCaja) Then
                        EsAmpliacionBCR = True
                    End If
                    Dim dtConstitucion As DataTable = oOperacionesCajaBM.Cartas_Deposito_Renovacion(CodigoOperacionCaja, "1")
                    Dim dtCancelacion As DataTable = oOperacionesCajaBM.Cartas_Deposito_Renovacion(CodigoOperacionCaja, "2")
                    rds = New ReportDataSource("DSCancelacion", dtCancelacion)
                    rds2 = New ReportDataSource("DSConstitucion", dtConstitucion)
                    'Para las firmas
                    dt = dtConstitucion
                ElseIf oOperacionesCajaBM.EsRenovacion(CodigoOperacionCaja) = "2" Then
                    Dim dtConstitucion As DataTable = oOperacionesCajaBM.ImpresionCarta_Constitucion_CancelacionDPZ(CodigoOperacionCaja)
                    Dim dtCancelacion As DataTable = oOperacionesCajaBM.ImpresionCarta_Transferencias(oOperacionesCajaBM.CodigoBCR(CodigoOperacionCaja))
                    rds = New ReportDataSource("DSTransferencia", dtCancelacion)
                    rds2 = New ReportDataSource("DSConstitucion", dtConstitucion)
                    'Para las firmas
                    dt = dtConstitucion

                Else
                    dt = oOperacionesCajaBM.ImpresionCarta_Constitucion_CancelacionDPZ(CodigoOperacionCaja)
                    rds = New ReportDataSource("DSConstitucion_VencimientoDPZ", dt)
                End If
                'modificacion codigos 63 y 64
            ElseIf (CodigoOperacion = "63" And CodigoModelo <> "TE01") Or (CodigoOperacion = "64" And CodigoModelo <> "TE02") Or CodigoOperacion = "BCRI" Or CodigoOperacion = "BCRE" Then
                dt = oOperacionesCajaBM.ImpresionCarta_Transferencias(CodigoOperacionCaja)
                rds = New ReportDataSource("DataSet1", dt)
            End If
            If EsAmpliacionBCR Then
                RV.LocalReport.ReportPath = "Modulos\ModelosCarta\rpt_PlantillaCartas05.rdlc"
            Else
                RV.LocalReport.ReportPath = oOperacionesCajaBM.Ruta_Carta(CodigoOperacion, CodigoModelo)
            End If
            RV.LocalReport.EnableExternalImages = True

            'Firmas
            Dim PathFirma1 = "", PathFirma2 As String = ""
            Dim objAprobadorCarta As New AprobadorCartaBM

            'Para la firma 01
            Dim codigoUsuario As String = dt.Rows(0)("CodigoUsuarioF1").ToString()
            Dim listaAprob As AprobadorCartaBEList = objAprobadorCarta.SeleccionarPorFiltro(codigoUsuario, "", "")
            If listaAprob.Count > 0 Then PathFirma1 = listaAprob(0).Firma

            If Not File.Exists(PathFirma1) Then PathFirma1 = folderImagenes & Path.GetFileName(PathFirma1)
            If Not File.Exists(PathFirma1) Then PathFirma1 = folderImagenes & CONST_ARCHIVO_SIN_FIRMA
            PathFirma1 = New Uri(PathFirma1).AbsoluteUri

            'Para la firma 02
            codigoUsuario = dt.Rows(0)("CodigoUsuarioF2").ToString()
            listaAprob = objAprobadorCarta.SeleccionarPorFiltro(codigoUsuario, "", "")
            If listaAprob.Count > 0 Then PathFirma2 = listaAprob(0).Firma

            If Not File.Exists(PathFirma2) Then PathFirma2 = folderImagenes & Path.GetFileName(PathFirma2)
            If Not File.Exists(PathFirma2) Then PathFirma2 = folderImagenes & CONST_ARCHIVO_SIN_FIRMA
            PathFirma2 = New Uri(PathFirma2).AbsoluteUri


            Dim Firma1 As New ReportParameter("Firma1", PathFirma1)
            RV.LocalReport.SetParameters(Firma1)
            Dim Firma2 As New ReportParameter("Firma2", PathFirma2)
            RV.LocalReport.SetParameters(Firma2)
            'Reporte
            If Not rds Is Nothing Then
                RV.LocalReport.DataSources.Add(rds)
            End If
            If Not rds2 Is Nothing Then
                RV.LocalReport.DataSources.Add(rds2)
            End If
            bytes = RV.LocalReport.Render("pdf")
            Ruta = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest) + CodigoOperacionCaja + ".pdf"
            System.IO.File.WriteAllBytes(Ruta, bytes)
            GC.SuppressFinalize(Me)
        End Using

        Return Ruta
    End Function
    Private Function GenerarReporteComVenAcc(ByVal strCodigoOrden As String, ByVal strCodigoOperacion As String, ByVal strModeloCarta As String) As String

        'CRumiche (2019-07-01): Tomamos de parametría la ruta de la carpeta de las imagenes (firmas)
        Dim listaParams As TablaGeneralBEList = (New ParametrosGeneralesBM()).Listar(ParametrosSIT.RUTA_FIRMA_CARTA, DatosRequest)
        If listaParams.Count = 0 Then Throw New Exception("No se encontró el parámetro RUTA_FIRMA_CARTA")
        Dim folderImagenes As String = listaParams(0).Comentario
        If Not folderImagenes.EndsWith("\") Then folderImagenes += "\"

        GenerarReporteComVenAcc = ""
        Dim bytes As Byte()
        Dim Ruta As String
        Using RV As New ReportViewer
            Dim ds As DataSet
            RV.LocalReport.ReportPath = oOperacionesCajaBM.Ruta_Carta(strCodigoOperacion, strModeloCarta)
            ds = oOperacionesCajaBM.ImpresionCarta_CompraVentaAcciones(strCodigoOrden, strModeloCarta)
            For i = 0 To ds.Tables.Count - 1
                RV.LocalReport.DataSources.Add(GetMyDataTable("dsCompraVentaNacional", ds.Tables(i), i)) 'i+1
            Next
            RV.LocalReport.EnableExternalImages = True

            'Firmas
            Dim PathFirma1 = "", PathFirma2 As String = ""
            Dim objAprobadorCarta As New AprobadorCartaBM

            'Para la firma 01
            Dim codigoUsuario As String = ds.Tables(0).Rows(0)("CodigoUsuarioF1").ToString()
            Dim listaAprob As AprobadorCartaBEList = objAprobadorCarta.SeleccionarPorFiltro(codigoUsuario, "", "")
            If listaAprob.Count > 0 Then PathFirma1 = listaAprob(0).Firma

            If Not File.Exists(PathFirma1) Then PathFirma1 = folderImagenes & Path.GetFileName(PathFirma1)
            If Not File.Exists(PathFirma1) Then PathFirma1 = folderImagenes & CONST_ARCHIVO_SIN_FIRMA
            PathFirma1 = New Uri(PathFirma1).AbsoluteUri

            'Para la firma 02
            codigoUsuario = ds.Tables(0).Rows(0)("CodigoUsuarioF2").ToString()
            listaAprob = objAprobadorCarta.SeleccionarPorFiltro(codigoUsuario, "", "")
            If listaAprob.Count > 0 Then PathFirma2 = listaAprob(0).Firma

            If Not File.Exists(PathFirma2) Then PathFirma2 = folderImagenes & Path.GetFileName(PathFirma2)
            If Not File.Exists(PathFirma2) Then PathFirma2 = folderImagenes & CONST_ARCHIVO_SIN_FIRMA
            PathFirma2 = New Uri(PathFirma2).AbsoluteUri

            'Parametro Operación
            Dim dtOperacion As DataTable = Nothing
            dtOperacion = ds.Tables(0).DefaultView.ToTable(True, "Caso") '1
            Dim _paramOperacion As String = String.Empty
            If dtOperacion IsNot Nothing Then
                _paramOperacion = dtOperacion.Rows(0)("Caso").ToString()
            End If
            Dim Firma1 As New ReportParameter("Firma1", PathFirma1)
            RV.LocalReport.SetParameters(Firma1)
            Dim Firma2 As New ReportParameter("Firma2", PathFirma2)
            RV.LocalReport.SetParameters(Firma2)
            Dim paramOperacion As New ReportParameter("Operacion", _paramOperacion)
            RV.LocalReport.SetParameters(paramOperacion)

            bytes = RV.LocalReport.Render("pdf")
            Ruta = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest) + DateTime.Today.ToString("yyyyMMdd") + DateTime.Now.ToString("HHMMss") + ".pdf"
            System.IO.File.WriteAllBytes(Ruta, bytes)
            GC.SuppressFinalize(Me)
        End Using
        GenerarReporteComVenAcc = Ruta
    End Function
    Private Sub GenerarVista(Optional ByVal arrayCodigos As ArrayList = Nothing)
        Dim oOperacionCaja As New OperacionesCajaBM
        Dim path As String
        Dim chk As System.Web.UI.WebControls.CheckBox
        Dim windowsCartas As New System.Text.StringBuilder
        Dim veces As Integer = 0
        Dim lbCodigo, lbCodigoOperacion, lbModeloCarta, lblNumeroCuenta As Label, lblTipo As Label
        Dim dtComVenAcc As DataTable = Nothing
        Dim strArr() As String
        CreateTableComVenAcc(dtComVenAcc)
        Try
            'Actaliza algunos datos de las constitucuiones de DPZ a las cancelaciones para la impresion de cartas.
            oOperacionCaja.ActualizaDatosCancelacionesDPZ(UIUtility.ConvertirFechaaDecimal(tbFecha.Text))
            For Each item As GridViewRow In dgLista.Rows
                If (item.RowType = DataControlRowType.DataRow) Then
                    If (item.FindControl("chkSelect") Is Nothing) = False Then
                        chk = CType(item.FindControl("chkSelect"), WebControls.CheckBox)
                        If (chk.Checked = True) Then
                            lbCodigo = CType(item.FindControl("lbCodigo"), Label)
                            lbCodigoOperacion = CType(item.FindControl("lblCodigoOperacion"), Label)
                            lbModeloCarta = CType(item.FindControl("lblCodigoModeloCarta"), Label)
                            lblNumeroCuenta = CType(item.FindControl("lblNumeroCuenta"), Label)
                            lblTipo = CType(item.FindControl("lblTipo"), Label)
                            'Acciones | Operaciones de Reporte
                            If lbModeloCarta.Text = "CVA1" Or lbModeloCarta.Text = "CVA2" Or lbModeloCarta.Text = "OIAC" Or
                                lbModeloCarta.Text = "VNOR" Then
                                lbCodigo.Text = item.Cells(8).Text.Trim.Replace("&nbsp;", "")
                                dtComVenAcc.Rows.Add(lbCodigo.Text, lbCodigoOperacion.Text, lbModeloCarta.Text, lblTipo.Text)
                            Else
                                path = GeneraReporte(lbCodigo.Text, lbCodigoOperacion.Text, lbModeloCarta.Text)
                                windowsCartas.Append(path & "&")
                            End If
                            veces = veces + 1
                        End If
                        End If
                    End If
            Next
            If dtComVenAcc.Rows.Count > 0 Then
                For Each drCartas As DataRow In dtComVenAcc.Rows
                    If drCartas("ModeloCarta") = "VNOR" Then
                        path = GenerarReporteOperacionReporte(drCartas("CodigoOrden").Replace(";", ","), drCartas("CodigoOperacion"), drCartas("ModeloCarta"))
                        windowsCartas.Append(path & "&")
                    Else
                        path = GenerarReporteComVenAcc(drCartas("CodigoOrden").Replace(";", ","), drCartas("CodigoOperacion"), drCartas("ModeloCarta"))
                        windowsCartas.Append(path & "&")
                    End If
                Next
            End If

            If veces > 0 Then
                rutaCartas = windowsCartas.ToString()
                If (rutaCartas.Length > 0) Then
                    rutaCartas = rutaCartas.Remove(rutaCartas.Length - 1, 1)
                    If (veces > 1) Then
                        rutaCartas = HelpCarta.CrearMultiCartaPDF(rutaCartas)
                    End If
                End If
                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "Cartas_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".PDF")
                Response.WriteFile(rutaCartas)
                Response.End()
            Else
                AlertaJS("Seleccionar al menos una carta para imprimir.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub btnVista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVista.Click
        GenerarVista()
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
       Buscar()
    End Sub
    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            Dim oOperacionCaja As New OperacionesCajaBM
            'OT 10025 21/02/2017 - Carlos Espejo
            'Descripcion: Nuevo campo para generacion de claves en un solo proceso (FechaConsulta)
            'Mejora en el proceso de generacion de la aprobacion
            Dim dtClaves As TablaGeneralBEList = New AprobadorCartaBM().GeneraClaves(6, True, UIUtility.ConvertirFechaaDecimal(tbFecha.Text))
            Dim i As Integer = 0
            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As System.Web.UI.WebControls.CheckBox = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                    If Not chkSelect Is Nothing Then
                        Dim lbEstadoCarta As System.Web.UI.WebControls.Label = CType(fila.FindControl("lbEstadoCarta"), System.Web.UI.WebControls.Label)
                        If lbEstadoCarta.Text = "1" Then
                            If chkSelect.Checked = True Then
                                Dim lbCodigo As System.Web.UI.WebControls.Label = CType(fila.FindControl("lbCodigo"), System.Web.UI.WebControls.Label)
                                Dim lbFondo As System.Web.UI.WebControls.Label = CType(fila.FindControl("lbFondo"), System.Web.UI.WebControls.Label)
                                Dim lbModeloCarta As Label = CType(fila.FindControl("lblCodigoModeloCarta"), Label)
                                If lbModeloCarta.Text = "CVA1" Or lbModeloCarta.Text = "CVA2" Or lbModeloCarta.Text = "OIAC" Then
                                    lbCodigo.Text = fila.Cells(8).Text.Trim.Replace("&nbsp;", "")
                                    Dim codigoOrden() As String
                                    codigoOrden = lbCodigo.Text.Split(";")
                                    For j As Integer = 0 To codigoOrden.Length - 1
                                        If codigoOrden(j) <> String.Empty Then
                                            oOperacionCaja.AprobarOperacionCaja(codigoOrden(j), lbFondo.Text, DatosRequest)
                                        End If
                                    Next
                                Else
                                    oOperacionCaja.AprobarOperacionCaja(lbCodigo.Text, lbFondo.Text, DatosRequest)
                                End If
                                i = i + 1
                            End If
                        Else
                            chkSelect.Checked = False
                        End If
                    End If
                End If
            Next
            If i = 0 Then
                AlertaJS("No se ha seleccionado registros validos para la aprobación.")
            Else
                EnviarClaveFirma("", "")
                CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                AlertaJS("Se ha realizado la aprobacion de " & i & " Carta(s)")
            End If
            'OT 10025 Fin
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try
    End Sub
    Private Sub btnFirmar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirmar.Click
        'OT 10119 16/03/2017 - Carlos Espejo
        'Descripcion: Si el campo firmante esta vacio no se puede firmar
        If tbCodAprob.Text.Trim <> "" Then
            'OT 10119 Fin
            Try
                Dim cont As Integer = 0
                Dim oOperacionesCaja As New OperacionesCajaBM
                Dim chkSelect As System.Web.UI.WebControls.CheckBox
                Dim lbCodigo As System.Web.UI.WebControls.Label
                Dim lbFondo As System.Web.UI.WebControls.Label
                Dim lbModeloCarta As Label
                Dim codigoOrden As String = ""
                Dim mensaje As String = ""
                Dim usuarioAprobador As String = ""
                Dim swChecked As Integer = 0
                For Each fila As GridViewRow In dgLista.Rows
                    If fila.RowType = DataControlRowType.DataRow Then
                        chkSelect = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                        lbCodigo = CType(fila.FindControl("lbCodigo"), System.Web.UI.WebControls.Label)
                        lbFondo = CType(fila.FindControl("lbFondo"), System.Web.UI.WebControls.Label)
                        lbModeloCarta = CType(fila.FindControl("lblCodigoModeloCarta"), Label)
                        If chkSelect.Checked Then
                            swChecked = 1
                            Dim resultado As Boolean
                            If lbModeloCarta.Text = "CVA1" Or lbModeloCarta.Text = "CVA2" Or lbModeloCarta.Text = "OIAC" Then
                                lbCodigo.Text = fila.Cells(8).Text.Trim.Replace("&nbsp;", "")
                                Dim sw As Int16 = 0
                                Dim codigoOrdenAux() As String
                                codigoOrdenAux = lbCodigo.Text.Split(";")
                                For j As Integer = 0 To codigoOrdenAux.Length - 1
                                    If codigoOrdenAux(j) <> String.Empty Then
                                        resultado = oOperacionesCaja.FirmarCarta(codigoOrdenAux(j), lbFondo.Text, Usuario, tbCodAprob.Text.ToString().ToUpper, DatosRequest)
                                        If resultado Then
                                            codigoOrden = codigoOrden & codigoOrdenAux(j) & "|"
                                            sw = 1
                                        End If
                                    End If
                                Next
                                If sw = 1 Then
                                    cont = cont + 1
                                End If
                            Else
                                resultado = oOperacionesCaja.FirmarCarta(lbCodigo.Text, lbFondo.Text, Usuario, tbCodAprob.Text.ToString().ToUpper, DatosRequest)
                                If resultado Then
                                    cont = cont + 1
                                    codigoOrden = codigoOrden & lbCodigo.Text.Trim & "|"
                                    'OT10986 - 06/12/2017 - Ian Pastor M. Se comentó las validaciones porque no son correctas. Estas deben de realizarse al finalizar el bucle
                                    'Else                                'If cont > 0 Then
                                    '    AlertaJS("Se ha realizado la firma de " & cont & " Carta(s). <br />" & _
                                    '             " Las demas cartas no cumplen los requisitos.")
                                    'Else
                                    '    AlertaJS("Ninguna carta ha sido firmada, debido a que no cumplen los requisitos.")
                                    'End If
                                End If
                            End If
                        End If
                    End If
                Next
                If swChecked > 0 Then 'OT10986 - 06/12/2017 - Ian Pastor M. Verifica que se haya seleccionado uno o más registros de la grilla para su firma
                    If cont > 0 Then
                        CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                        AlertaJS("Se ha realizado la firma de " & cont & " Carta(s)")
                        usuarioAprobador = obtenerUsuarioAprobador()
                        Me.lblMensaje.Visible = True
                        If usuarioAprobador <> "" Then
                            mensaje = mensajeCartasFirmadas(codigoOrden)
                            UIUtility.EnviarMail(usuarioAprobador, "", "SIT - Cartas Firmadas", mensaje, DatosRequest)
                            Me.lblMensaje.Text = "Se ha enviado un correo de aviso a los Usuarios Aprobadores"
                        Else
                            Me.lblMensaje.Text = "Ha ocurrido un error en el envío de correo a los Usuarios Aprobadores"
                        End If
                    End If
                Else
                    AlertaJS("Debe seleccionar algún registro!")
                End If
            Catch ex As Exception
                AlertaJS(Replace(ex.Message.ToString(), "'", ""))
            End Try
        End If
    End Sub
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim lbDetalleOperaciones As LinkButton
            Dim hdCantidadOperacion As HtmlInputHidden
            lbDetalleOperaciones = CType(e.Row.FindControl("lbDetalleOperaciones"), LinkButton)
            hdCantidadOperacion = CType(e.Row.FindControl("hdCantidadOperacion"), HtmlInputHidden)
            lbDetalleOperaciones.Text = hdCantidadOperacion.Value
        End If
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim objObservacion As New ModeloCartaBM
        Try
            Dim index As Integer = -1
            If e.CommandName = "MostrarDetalle_Operaciones" Then

                hdnCodigoAgrupacion.Value = ""


                Dim ds As New DataSet

                Dim objListaCartas As SeleccionCartaBEList
                index = Integer.Parse(e.CommandArgument.ToString)
                Dim codigoPortafolio As String = String.Empty
                Dim fecha As Decimal = 0
                Dim codigoOperacionCaja As String = String.Empty
                Dim codigoCartaAgrupado As Integer = 0
                Dim codigoAgrupacionObservacion As Integer = 0
                Dim Observacion As String = ""

                codigoPortafolio = dgLista.Rows(index).Cells(13).Text.Trim.Replace("&nbsp;", "")
                fecha = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
                codigoOperacionCaja = dgLista.Rows(index).Cells(8).Text.Trim.Replace("&nbsp;", "")
                codigoCartaAgrupado = Integer.Parse(dgLista.Rows(index).Cells(14).Text.Trim.Replace("&nbsp;", ""))

                'Nuevo
                If (String.IsNullOrEmpty(dgLista.Rows(index).Cells(16).Text.Trim.Replace("&nbsp;", ""))) Then
                    codigoAgrupacionObservacion = 0
                Else
                    codigoAgrupacionObservacion = Integer.Parse(dgLista.Rows(index).Cells(16).Text.Trim.Replace("&nbsp;", ""))
                End If
                'Nuevo
                hdnCodigoAgrupacion.Value = codigoAgrupacionObservacion.ToString()
                'hdnCodigoAgrupacion.Value = "1"
                Dim CodigoModelo As String
                CodigoModelo = dgLista.Rows(index).Cells(15).Text.Trim.Replace("&nbsp;", "")

                Dim arrayCodigoOperacionCaja() As String
                arrayCodigoOperacionCaja = codigoOperacionCaja.Split(";")
                If arrayCodigoOperacionCaja.Length > 1 Then
                    objListaCartas = oOperacionesCajaBM.SeleccionCartas("", codigoPortafolio, "", "", fecha, ddlEstado.SelectedValue, "", codigoCartaAgrupado, "N")
                Else
                    objListaCartas = oOperacionesCajaBM.SeleccionCartas("", codigoPortafolio, "", "", fecha, ddlEstado.SelectedValue, arrayCodigoOperacionCaja(0), codigoCartaAgrupado, "N")
                End If

                If (CodigoModelo = "CVA1" Or CodigoModelo = "CVA2" Or CodigoModelo = "OIAC") Then

                    EjecutarJS("MuestraObservacion();")
                Else

                    EjecutarJS("OcultaObservacion();")
                End If


                'Obterner la observacion
                ds = objObservacion.ObtenerObservacionCarta(Convert.ToInt32(hdnCodigoAgrupacion.Value))
                If (ds.Tables.Count > 0) Then
                    If (ds.Tables(0).Rows.Count > 0) Then
                        Observacion = ds.Tables(0).Rows(0)(0)
                    Else
                        Observacion = ""
                    End If
                Else
                    Observacion = ""
                End If

                EjecutarJS("EliminarDetalleOperaciones();")
                EjecutarJS("agregarCabeceraTabla();")

                For Each objCarta As SeleccionCartaBE In objListaCartas
                    EjecutarJS(String.Format("agregarFilaDetalleOperaciones('{0}', '{1}', '{2}', '{3}');", objCarta.DescripcionOperacion, objCarta.NumeroOrden, objCarta.DescripcionIntermediario, _
                                             IIf(objCarta.CodigoOperacion = "2", "(" & Format(CDec(objCarta.Importe), "##,##0.00") & ")", Format(CDec(objCarta.Importe), "##,##0.00"))))
                Next
                EjecutarJS("mostrarDetalleOperaciones();")
                EjecutarJS(String.Format("MostrarObservacion('{0}');", Observacion))
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex()
        lOperaciones = DirectCast(ViewState("Operaciones"), SeleccionCartaBEList)
        dgLista.DataSource = lOperaciones
        dgLista.DataBind()
    End Sub
    Public Sub dgLista_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim chkTemp As CheckBox = CType(sender, CheckBox)
            Dim dgi As GridViewRow
            Dim i As Int32 = 0
            Dim valor As String = String.Empty
            dgi = CType(chkTemp.Parent.Parent, GridViewRow)
            i = dgLista.PageSize * dgLista.PageIndex + dgi.RowIndex
            lOperaciones = DirectCast(ViewState("Operaciones"), SeleccionCartaBEList)
            lOperaciones(i).check = chkTemp.Checked
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    Protected Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        'OT 10144 20/03/2017 - Carlos Espejo
        'Descripcion: Se agrega el dispose para eliminar de memoria el objeto reportviewer
        'OT 10150 24/03/2017 - Carlos Espejo
        'Descripcion: Solo cuando la variable tengo se halla usado se realiza el dispose
        If Not RV Is Nothing Then
            RV.Dispose()
        End If
        LiberarMemoria()
        'OT 10150 Fin
        'OT 10144 Fin
    End Sub
#End Region
#Region "Funciones Personalizas"
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        ddlPortafolio.Items.Clear()
        Dim oPortafolio As New PortafolioBM
        Dim lPortafolio As PortafolioBEList = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        For Each ePortafolio As PortafolioBE In lPortafolio
            Dim Li As New ListItem
            Li.Value = ePortafolio.CodigoPortafolioSBS
            Li.Text = ePortafolio.Descripcion
            Li.Selected = False
            ddlPortafolio.Items.Add(Li)
        Next
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        ddlMercado.Items.Clear()
        Dim oMercado As New MercadoBM
        Dim lMercado As MercadoBEList = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        UIUtility.InsertarElementoSeleccion(ddlMercado)
        For Each eMercado As MercadoBE In lMercado
            Dim Li As New ListItem
            Li.Value = eMercado.CodigoMercado
            Li.Text = eMercado.Descripcion
            ddlMercado.Items.Add(Li)
        Next
        ddlMercado.Enabled = enabled
    End Sub
    Private Sub CargarBanco(ByVal CodigoMercado As String, Optional ByVal enabled As Boolean = True)
        ddlBanco.Items.Clear()
        Dim oBanco As New TercerosBM
        Dim lBanco As TerceroBEList = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio("", ddlPortafolio.SelectedValue)
        UIUtility.InsertarElementoSeleccion(ddlBanco)
        For Each eBanco As TerceroBE In lBanco
            Dim Li As New ListItem
            Li.Value = eBanco.CodigoTercero
            Li.Text = eBanco.Descripcion
            ddlBanco.Items.Add(Li)
        Next
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarIntermediario(ByVal enabled As Boolean)
        ddlIntermediario.Items.Clear()
        Dim oIntermediario As New TercerosBM
        Dim lIntermediario As TerceroBEList = oIntermediario.SeleccionarPorFiltro(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "")
        UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        For Each eIntermediario As TerceroBE In lIntermediario
            Dim Li As New ListItem
            Li.Value = eIntermediario.CodigoTercero
            Li.Text = eIntermediario.Descripcion
            ddlIntermediario.Items.Add(Li)
        Next
        ddlIntermediario.Enabled = enabled
    End Sub
    Private Sub CargarEstadoCarta(ByVal enabled As Boolean)
        ddlEstado.Items.Clear()
        Dim lEstados As TablaGeneralBEList = New ParametrosGeneralesBM().Listar(ParametrosSIT.ESTADO_CARTA, DatosRequest)
        UIUtility.InsertarElementoSeleccion(ddlEstado)
        For Each eEstados As TablaGeneralBE In lEstados
            Dim Li As New ListItem
            Li.Value = eEstados.Codigo
            Li.Text = eEstados.Comentario
            ddlEstado.Items.Add(Li)
        Next
        ddlEstado.Enabled = enabled
    End Sub
    Private Sub EstablecerFecha()
        tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
    End Sub
    Private Sub HabilitaControles(Optional ByVal tipo As String = "")
        CargarEstadoCarta(True)
        If tipo = ParametrosSIT.ADM_CARTA Then
            btnAprobar.Visible = True
            lbCodAprob.Visible = False
            tbCodAprob.Visible = False
            btnFirmar.Visible = False
            ddlEstado.SelectedValue = ParametrosSIT.ESTADO_CARTA_PND
        Else
            btnAprobar.Visible = False
            lbCodAprob.Visible = True
            tbCodAprob.Visible = True
            btnFirmar.Visible = True
            ddlEstado.Enabled = True
            ddlEstado.SelectedValue = ParametrosSIT.ESTADO_CARTA_APR
        End If
    End Sub
    Private Sub CargarPagina()
        btnFirmar.Attributes.Add("onClick", "if (ValidarIngresoFirmas()){this.disabled = true;btnVista.disabled = true;this.value = 'en proceso...';}")
        btnAprobar.Attributes.Add("onClick", "this.disabled = true; btnVista.disabled = true; this.value = 'En proceso...'; ")
        HabilitaControles()
        Dim codigoUsuario As String = ""
        Dim lAdministrador As AprobadorCartaBEList = New AprobadorCartaBM().SeleccionarPorFiltro("", ParametrosSIT.ADM_CARTA, ParametrosSIT.ESTADO_ACTIVO)
        For Each eAdministrador As AprobadorCartaBE In lAdministrador
            If eAdministrador.CodigoInterno.ToUpper = Usuario.ToUpper Then
                HabilitaControles(ParametrosSIT.ADM_CARTA)
                Exit For
            End If
        Next
        CargarPortafolio()
        CargarMercado()
        CargarBanco(ddlMercado.SelectedValue, True)
        CargarIntermediario(True)
        tbFecha.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
        ClaveFirmante()
        ViewState("codigoMercado") = ddlMercado.SelectedValue
        ViewState("codigoPortafolio") = ddlPortafolio.SelectedValue
        ViewState("CodigoTercero") = ddlIntermediario.SelectedValue
        ViewState("codigoTerceroBanco") = ddlBanco.SelectedValue
        ViewState("fechaLiquidacion") = UIUtility.ConvertirFechaaDecimal(IIf(tbFecha.Text = "", "0", tbFecha.Text))
        ViewState("estadoCarta") = ddlEstado.SelectedValue
    End Sub
    Private Sub CargarGrilla(Optional ByVal fechaLiquidacion As Decimal = 0, Optional ByVal codigoMercado As String = "", Optional ByVal codigoPortafolio As String = "", _
    Optional ByVal codigoTercero As String = "", Optional ByVal codigoTerceroBanco As String = "", Optional ByVal estadoCarta As String = "", _
    Optional ByVal codigoOperacionCaja As String = "")
        lOperaciones = New OperacionesCajaBM().SeleccionCartas(codigoMercado, _
        codigoPortafolio, codigoTercero, codigoTerceroBanco, fechaLiquidacion, estadoCarta, codigoOperacionCaja, 1)
        ViewState("Operaciones") = lOperaciones
        dgLista.DataSource = lOperaciones
        dgLista.DataBind()
        NroCarta.Text = UIUtility.MostrarResultadoBusqueda(lOperaciones.Count)
    End Sub
    Private Sub EnviarClaveFirma(ByVal rutaArchivo As String, ByVal rutaCartas As String)
        Dim lClaveFirmantes As New TablaGeneralBEList
        Dim codigoUsuario As String
        Dim clave As String
        Dim mensaje As String
        Dim toUser As String
        lClaveFirmantes = New ParametrosGeneralesBM().ListarClaveFirmantesCartas(UIUtility.ConvertirFechaaDecimal(tbFecha.Text))
        For Each eClaveFirmantes As TablaGeneralBE In lClaveFirmantes
            codigoUsuario = eClaveFirmantes.Codigo
            clave = eClaveFirmantes.Valor
            mensaje = HelpCorreo.MensajeNotificacionClave(codigoUsuario, tbFecha.Text, clave)
            toUser = New PersonalBM().SeleccionarMail(codigoUsuario.ToString())
            UIUtility.EnviarMail(toUser, "", "Notificación de clave - Firma de Cartas", mensaje, DatosRequest, rutaArchivo)
        Next
    End Sub
    Private Function mensajeCartasFirmadas(codigoOrden As String) As String
        Dim dt As DataTable
        Dim bmOperacionesCaja As New OperacionesCajaBM
        Dim bmPersonal As New PersonalBM
        Dim mensaje As New StringBuilder
        Dim arrayCodigo As Array
        codigoOrden = codigoOrden.Substring(0, codigoOrden.Length - 1)
        arrayCodigo = codigoOrden.Split("|")
        With mensaje
            .Append("<table>")
            .Append("<tr><td>")
            .Append("El usuario " & bmPersonal.SeleccionarPorCodigoInterno(ObtenerValorRequest(DatosRequest, "Usuario"), DatosRequest) & " ha firmado las siguientes cartas")
            .Append("</td></tr>")
            .Append("</table>")
            .Append("<br/>")
            'Cabecera Tabla
            .Append("<table  border='1' cellpadding='2'>")
            .Append("<tr align='center'>")
            .Append("<td>PORTAFOLIO</td>")
            .Append("<td>C&Oacute;DIGO OPERACI&Oacute;N CAJA</td>")
            .Append("<td>OPERACI&Oacute;N</td>")
            .Append("<td>IMPORTE</td>")
            .Append("<td>MONEDA</td>")
            .Append("<td>VBADMIN</td>")
            .Append("<td>VBGERF1</td>")
            .Append("<td>VBGERF2</td>")
            .Append("</tr>")
            'Contenido Tabla
            For i = 0 To arrayCodigo.Length - 1
                If arrayCodigo(i) <> "" Then
                    dt = bmOperacionesCaja.SeleccionarAutorizacionCartas(String.Empty, String.Empty, String.Empty, String.Empty, ViewState("fechaLiquidacion"), False, String.Empty, arrayCodigo(i), DatosRequest).Tables(0)
                    If dt.Rows.Count > 0 Then
                        .Append("<tr>")
                        .Append("<td>" & dt.Rows(0)("DescripcionPortafolio").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("CodigoOperacionCaja").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("DescripcionOperacion").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("Importe").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("CodigoMoneda").ToString.Trim & "</td>")
                        .Append("<td>" & bmPersonal.SeleccionarPorCodigoInterno(dt.Rows(0)("VBADMINUSR").ToString.Trim, DatosRequest) & "</td>")
                        .Append("<td>" & bmPersonal.SeleccionarPorCodigoInterno(dt.Rows(0)("VBGERF1USR").ToString.Trim, DatosRequest) & "</td>")
                        .Append("<td>" & bmPersonal.SeleccionarPorCodigoInterno(dt.Rows(0)("VBGERF2USR").ToString.Trim, DatosRequest) & "</td>")
                        .Append("</tr>")
                    End If
                End If
            Next
            .Append("</table>")
            .Append("<br/>")
            'Fin Contenido Tabla
            .Append("<table>")
            .Append("<tr><td>AFP Integra</td></tr>")
            .Append("<tr><td>Grupo SURA</td></tr>")
            .Append("</table>")
        End With
        Return mensaje.ToString
    End Function
    Private Function obtenerUsuarioAprobador() As String
        Dim lAprobadorCarta As AprobadorCartaBEList
        Dim bmAprobadorCarta As New AprobadorCartaBM
        Dim usuario As String = ""
        lAprobadorCarta = bmAprobadorCarta.SeleccionarPorFiltro("", "1", "A")
        For Each eAprobadorCarta In lAprobadorCarta
            usuario = usuario & eAprobadorCarta.email_trabajo & ";"
        Next
        Return usuario
    End Function
    Public Function ObtenerValorRequest(ByVal dataRequest As DataSet, ByVal nombre As String) As String
        Dim columnName As String = dataRequest.Tables(0).Columns(0).ColumnName
        Return CType(dataRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
    End Function
#End Region
    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr, ByVal min As Int32, ByVal max As Int32) As Boolean
    Public Sub LiberarMemoria()
        Try
            Dim memoria As Process
            memoria = Process.GetCurrentProcess()
            SetProcessWorkingSetSize(memoria.Handle, -1, -1)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Private Sub CreateTableComVenAcc(ByRef dt As DataTable)
        dt = New DataTable
        dt.Columns.Add("CodigoOrden", GetType(String))
        dt.Columns.Add("CodigoOperacion", GetType(String))
        dt.Columns.Add("ModeloCarta", GetType(String))
        dt.Columns.Add("Tipo", GetType(String))
        'dt.Columns.Add("NumeroCuenta", GetType(String))
    End Sub
    Private Function GetMyDataTable(ByVal dataSourceName As String, ByVal dt As DataTable, Optional i As Integer = 0) As ReportDataSource
        If i = 0 Then
            GetMyDataTable = New ReportDataSource(dataSourceName, dt)
        Else
            GetMyDataTable = New ReportDataSource(dataSourceName & i.ToString(), dt)
        End If
    End Function

    Private Function GenerarReporteOperacionReporte(ByVal strCodigoOrden As String, ByVal strCodigoOperacion As String, ByVal strModeloCarta As String) As String
        GenerarReporteOperacionReporte = ""
        Dim bytes As Byte()
        Dim Ruta As String
        Using RV As New ReportViewer
            Dim ds As DataSet
            RV.LocalReport.ReportPath = oOperacionesCajaBM.Ruta_Carta(strCodigoOperacion, strModeloCarta)
            ds = oOperacionesCajaBM.ImpresionCarta_OperacionReporte(strCodigoOrden)
            For i = 0 To ds.Tables.Count - 1
                RV.LocalReport.DataSources.Add(GetMyDataTable("dsOperacionReporte", ds.Tables(i), i)) 'i+1
            Next
            RV.LocalReport.EnableExternalImages = True
            'Firmas
            Dim PathFirma1, PathFirma2 As String
            If ds.Tables(0).Rows(0)("NombreUsuarioF1") = "" Then
                PathFirma1 = New Uri(Server.MapPath("~/Imagenes/sin_firma.jpg")).AbsoluteUri
            Else
                PathFirma1 = New Uri(Server.MapPath("~/Imagenes/" + ds.Tables(0).Rows(0)("CodigoUsuarioF1") + ".jpg")).AbsoluteUri
            End If
            If ds.Tables(0).Rows(0)("NombreUsuarioF2") = "" Then
                PathFirma2 = New Uri(Server.MapPath("~/Imagenes/sin_firma.jpg")).AbsoluteUri
            Else
                PathFirma2 = New Uri(Server.MapPath("~/Imagenes/" + ds.Tables(0).Rows(0)("CodigoUsuarioF2") + ".jpg")).AbsoluteUri
            End If
            'Parametro Operación
            Dim dtOperacion As DataTable = Nothing
            dtOperacion = ds.Tables(0).DefaultView.ToTable(True, "Tipo") '1
            Dim _paramOperacion As String = String.Empty
            If dtOperacion IsNot Nothing Then
                _paramOperacion = dtOperacion.Rows(0)("Tipo").ToString()
            End If
            Dim Firma1 As New ReportParameter("Firma1", PathFirma1)
            RV.LocalReport.SetParameters(Firma1)
            Dim Firma2 As New ReportParameter("Firma2", PathFirma2)
            RV.LocalReport.SetParameters(Firma2)
            Dim paramOperacion As New ReportParameter("Operacion", _paramOperacion)
            RV.LocalReport.SetParameters(paramOperacion)

            bytes = RV.LocalReport.Render("pdf")
            Ruta = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest) + DateTime.Today.ToString("yyyyMMdd") + DateTime.Now.ToString("HHMMss") + ".pdf"
            System.IO.File.WriteAllBytes(Ruta, bytes)
            GC.SuppressFinalize(Me)
        End Using
        GenerarReporteOperacionReporte = Ruta
    End Function

    Protected Sub btnGrabarObservacion_Click(sender As Object, e As System.EventArgs) Handles btnGrabarObservacion.Click
        'valida
        Dim objBL As New ModeloCartaBM
        Dim observacion As String
        Dim CodigoAgrupacion As String
        Try
            observacion = hdnObservacion.Value
            CodigoAgrupacion = hdnCodigoAgrupacion.Value
            objBL.GrabarObservacionCarta(Convert.ToInt32(CodigoAgrupacion), observacion, DatosRequest)
            AlertaJS("Se guardó correctamente.")
            EjecutarJS("CerrarModal();")
            Buscar()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try

    End Sub
    Public Sub Buscar()

        Try
            ViewState("codigoMercado") = ddlMercado.SelectedValue
            ViewState("codigoPortafolio") = ddlPortafolio.SelectedValue
            ViewState("codigoTercero") = ddlIntermediario.SelectedValue
            ViewState("codigoTerceroBanco") = ddlBanco.SelectedValue
            ViewState("fechaLiquidacion") = UIUtility.ConvertirFechaaDecimal(IIf(tbFecha.Text = "", "0", tbFecha.Text))
            ViewState("estadoCarta") = ddlEstado.SelectedValue
            ClaveFirmante()
            CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try

    End Sub
End Class