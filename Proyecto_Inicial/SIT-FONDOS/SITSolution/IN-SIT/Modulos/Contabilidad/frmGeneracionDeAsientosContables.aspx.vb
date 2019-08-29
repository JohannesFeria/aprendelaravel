Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Partial Class Modulos_Contabilidad_frmGeneracionDeAsientosContables
    Inherits BasePage
    Public Shared Function MensajeConfirmacionNumAsientos(ByVal listaNumAsientos_RT As ArrayList, ByVal listaNumAsientos_IN As ArrayList, ByVal ComisionSAFM As Boolean) As String
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String
        Mensaje = "Se generaron : " & listaNumAsientos_IN(0) & " Asientos en Inversiones </br>" + _
        "Se generaron : " & listaNumAsientos_RT(0) & " Asientos en Valorizacion"
        If ComisionSAFM Then
            Mensaje = Mensaje + "</br> Se genero : Un Asiento de comision SAFM "
        End If
        Return Mensaje
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
                ibProcesar.Attributes.Add("onclick", "return ValidarDatos();")
                ddlPortafolio_SelectedIndexChanged(Me, EventArgs.Empty)
                BuscarRuta()
                tbRuta.Enabled = True
            End If
            EvaluarProcesar()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub EvaluarProcesar()
        If Request.Form("hdProcesar") = "1" Then
            hdProcesar.Value = ""
            'GenerarAsiento() OT10783 - La generación de asientos se produce en el evento Procesar y no en el evento Load
        End If
    End Sub
    Private Sub GenerarAsiento()
        Dim oAsientoContable As New AsientoContableBM
        Dim FechaProceso As Decimal
        FechaProceso = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        Dim listaNumAsientos_RT As ArrayList, listaNumAsientos_IN As ArrayList, Comision As Boolean
        listaNumAsientos_RT = oAsientoContable.GenerarAsientoContable(ddlPortafolio.SelectedValue, FechaProceso, "VC", DatosRequest)
        listaNumAsientos_IN = oAsientoContable.GenerarAsientoContable(ddlPortafolio.SelectedValue, FechaProceso, "CVI", DatosRequest)
        'Si el fondo tiene series genera una asiento para cada una de las series
        Dim DT As DataTable = New PortafolioBM().ListarSeries(ddlPortafolio.SelectedValue)
        If DT.Rows.Count = 0 Then
            Comision = oAsientoContable.GenerarAsientoContableSAFM(ddlPortafolio.SelectedValue, "", "4", FechaProceso, DatosRequest)
        Else
            For Each DR In DT.Rows
                Comision = oAsientoContable.GenerarAsientoContableSAFM(ddlPortafolio.SelectedValue, DR("CodigoSerie"), "4", FechaProceso, DatosRequest)
            Next
        End If
        GenerarArchivo("RT", "VC")
        GenerarArchivo("IN", "CVI")
        GenerarArchivo("COM", "CO")
        AlertaJS(MensajeConfirmacionNumAsientos(listaNumAsientos_RT, listaNumAsientos_IN, Comision))
    End Sub
    Private Sub GenerarArchivo(ByVal TipoLote As String, ByVal CodigoTipoLote As String, Optional ByVal pTipoProceso As String = "")
        Dim oAsientoContable As New AsientoContableBM
        Dim dsArchivo As DataSet
        Dim registro As String = ""
        Dim PrefijoArchivo As String = "Lote"
        Dim sFileName As String = ""
        Dim sRutaFile As String = ""
        Dim sFecha As String = ""
        Dim FechaProceso As Decimal
        Dim errorGenerar As Boolean = False
        Dim i As Integer
        Dim extension As String = ViewState("Extension")
        Dim longitudRegistro As Integer = ViewState("LongitudRegistro")
        Dim auxformato As Decimal = 0.0
        errorGenerar = False
        FechaProceso = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        dsArchivo = oAsientoContable.AsientoContable_Interface(ddlPortafolio.SelectedValue, FechaProceso, CodigoTipoLote)
        If dsArchivo.Tables(0).Rows.Count > 0 Then
            sFecha = Convert.ToDateTime(tbFechaOperacion.Text).ToString("yyyyMMdd")
            sFileName = PrefijoArchivo & pTipoProceso & "_" & TipoLote & "_" & ddlPortafolio.SelectedItem.Text & "_" & sFecha & "." & extension
            sRutaFile = tbRuta.Text & sFileName
            Dim tw As TextWriter
            If (File.Exists(sRutaFile)) Then File.Delete(sRutaFile)
            tw = New StreamWriter(sRutaFile, True, Encoding.GetEncoding(1258))
            i = 0
            For i = 0 To dsArchivo.Tables(0).Rows.Count - 1
                registro = ""
                registro += dsArchivo.Tables(0).Rows(i).Item("PerVou").ToString().Trim().PadRight(4)
                registro += dsArchivo.Tables(0).Rows(i).Item("CodEmp").ToString().Trim().PadLeft(2)
                If dsArchivo.Tables(0).Rows(i).Item("CodLib").ToString.Trim = "CO" Then
                    registro += "AD"
                Else
                    registro += dsArchivo.Tables(0).Rows(i).Item("CodLib").ToString().Trim().PadRight(2)
                End If
                registro += dsArchivo.Tables(0).Rows(i).Item("NroVou").ToString().Trim().PadRight(5)
                registro += dsArchivo.Tables(0).Rows(i).Item("CorVou").ToString().Trim().PadRight(5)
                registro += dsArchivo.Tables(0).Rows(i).Item("FecVou").ToString().Trim().PadRight(8)
                registro += dsArchivo.Tables(0).Rows(i).Item("AsiVou").ToString().Trim().PadRight(5)
                registro += dsArchivo.Tables(0).Rows(i).Item("CtaVou").ToString().Trim().PadRight(15)
                registro += dsArchivo.Tables(0).Rows(i).Item("CenCos").ToString().Trim().PadRight(12)
                If dsArchivo.Tables(0).Rows(i).Item("CodLib").ToString.Trim = "CO" Then
                    If dsArchivo.Tables(0).Rows(i).Item("Debe").ToString.Trim <> Convert.ToDouble("0.0000000") Then
                        registro += ("").ToString.Trim.PadRight(18)
                    Else
                        registro += dsArchivo.Tables(0).Rows(i).Item("CtaCte").ToString().Trim().PadRight(16)
                        registro += dsArchivo.Tables(0).Rows(i).Item("TipCta").ToString().Trim().PadRight(2)
                    End If
                Else
                    registro += dsArchivo.Tables(0).Rows(i).Item("CtaCte").ToString().Trim().PadRight(16)
                    registro += dsArchivo.Tables(0).Rows(i).Item("TipCta").ToString().Trim().PadRight(2)
                End If
                registro += dsArchivo.Tables(0).Rows(i).Item("TipDoc").ToString().Trim().PadRight(2)
                registro += dsArchivo.Tables(0).Rows(i).Item("Nrodoc").ToString().Trim().PadRight(12)
                registro += dsArchivo.Tables(0).Rows(i).Item("FecVen").ToString().Trim().PadRight(8)
                If pTipoProceso = String.Empty Then
                    If dsArchivo.Tables(0).Rows(i).Item("Debe").ToString.Trim = "" Then
                        registro += auxformato.ToString("0.00").PadLeft(13)
                    Else
                        registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Debe")).ToString("0.00").PadLeft(13)
                    End If
                    If dsArchivo.Tables(0).Rows(i).Item("Haber").ToString.Trim = "" Then
                        registro += auxformato.ToString("0.00").PadLeft(13)
                    Else
                        registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Haber")).ToString("0.00").PadLeft(13)
                    End If
                Else
                    registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Haber")).ToString("0.00").PadLeft(13)
                    registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Debe")).ToString("0.00").PadLeft(13)
                End If
                registro += dsArchivo.Tables(0).Rows(i).Item("Moneda").ToString().PadRight(1)
                registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Tipcam")).ToString("0.0000").PadLeft(13)
                registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("ValOtraMon")).ToString("0.00").PadLeft(13)
                registro += dsArchivo.Tables(0).Rows(i).Item("CenGes").ToString().PadRight(6)
                registro += dsArchivo.Tables(0).Rows(i).Item("NombreTercero").ToString().PadRight(50)
                registro += dsArchivo.Tables(0).Rows(i).Item("Glosa").ToString().PadRight(80)
                If (dsArchivo.Tables(0).Rows(i).Item("DescripcionArchivo").ToString().Trim().Length > 50) Then
                    registro += dsArchivo.Tables(0).Rows(i).Item("DescripcionArchivo").ToString().Trim().Substring(1, 50)
                Else
                    registro += dsArchivo.Tables(0).Rows(i).Item("DescripcionArchivo").ToString().Trim().PadRight(50)
                End If
                registro += dsArchivo.Tables(0).Rows(i).Item("Per_Trans").ToString().PadRight(6)
                If (registro.Length = longitudRegistro) Then
                    tw.WriteLine(registro)
                Else
                    errorGenerar = True
                    Exit For
                End If
            Next
            tw.Close()
            If Not errorGenerar Then
                lblLog.Visible = True
                lblLog.Text = "Se ha generado correctamente el archivo Interfaz " + sFileName
            Else
                lblLog.Visible = True
                lblLog.Text = "Error al generar el archivo Interfaz " + sFileName
            End If
        Else
            lblLog.Visible = True
            lblLog.Text = "No existen datos para el archivo Interfaz de lote."
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub
    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.Items.Clear()
        ddlPortafolio.DataSource = dsPortafolio
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
    End Sub
    Private Sub ibProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibProcesar.Click
        Try
            If Request.Form("hdProcesar") = String.Empty Then
                Dim oAsientoContable As New AsientoContableBM
                Dim FechaProceso As Decimal
                FechaProceso = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
                If FechaProceso <> UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue) Then
                    AlertaJS(ObtenerMensaje("ALERT125"))
                    Exit Sub
                End If
                If Not Validar_VectorTipoCambio(FechaProceso) Then
                    AlertaJS("Debe ingresar Tipo de cambio para las monedas vigentes.")
                    Exit Sub
                End If
                Dim strCodigoMercado As String = ""
                Dim ds_In As DataSet = oAsientoContable.SeleccionarPorFiltroRevision(FechaProceso, ddlPortafolio.SelectedValue, "", "I", strCodigoMercado, DatosRequest)
                Dim ds_Rt As DataSet = oAsientoContable.SeleccionarPorFiltroRevision(FechaProceso, ddlPortafolio.SelectedValue, "", "V", strCodigoMercado, DatosRequest)
                If ds_In.Tables(0).Rows.Count = 0 And ds_Rt.Tables(0).Rows.Count = 0 Then
                    GenerarAsiento()
                Else
                    Dim Mensaje As String = ObtenerMensaje("CONF30")
                    'EjecutarJS("if(confirm('" & Mensaje & "')){document.getElementById('hdProcesar').value='1';document.forms['form1'].submit();}")
                    'OT10783 - Creación de mensaje de confirmación
                    EjecutarJS("GenerarAsientos_Confirmacion('" & Mensaje & "');")
                End If
            Else
                GenerarAsiento()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Function Validar_VectorTipoCambio(ByVal pfecha As Decimal) As Boolean
        Dim oBMVectorTipoCambio As New VectorTipoCambioBM
        Dim nValor As Integer
        nValor = oBMVectorTipoCambio.Validar_VectorTipoCambio(pfecha, DatosRequest)
        If nValor > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
#Region "/* Metodos Personalizados */"
    Private Sub BuscarRuta()
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim dtArchivoPlano As New DataSet
        dtArchivoPlano = oArchivoPlanoBM.Seleccionar(COD_ARCHIVO_LOTES, MyBase.DatosRequest())
        ViewState("Extension") = dtArchivoPlano.Tables(0).Rows(0).Item(3).ToString()
        ViewState("LongitudRegistro") = dtArchivoPlano.Tables(0).Rows(0).Item(7).ToString()
        tbRuta.Text = dtArchivoPlano.Tables(0).Rows(0).Item(4)
    End Sub
#End Region
    Protected Sub btnCierre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCierre.Click
        Try
            Dim oFeriadoBM As New FeriadoBM
            Dim objBM As New CuentasPorCobrarBM
            Dim FechaNueva As Date = Convert.ToDateTime(tbFechaOperacion.Text).AddDays(1)
            Dim FechaNuevaDec As Decimal
            FechaNuevaDec = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
            Dim DecFechaProceso As Decimal = CType(Date.Parse(tbFechaOperacion.Text).ToString("yyyyMMdd"), Decimal)
            Dim lote As String = objBM.ValidaLotesCuadradosParaCierre(DecFechaProceso, ddlPortafolio.SelectedValue, DatosRequest)
            Dim Mensaje As String = ""
            'OT 10031 27/02/2017 - Carlos Espejo
            'Descripcion: El mensaje se muestras pero perimite cerrar el fondo
            If Not lote.Equals("") Then
                Mensaje = " Pero existe un descuadre en el lote de \'" & lote & "\'."
            End If
            Dim oreporte As DataSet = objBM.OperacionesNoContabilizadas(DecFechaProceso, ddlPortafolio.SelectedValue, "", DatosRequest)
            If oreporte.Tables(0).Rows.Count > 0 Then
                AlertaJS("No es posible Cerrar porque existen documentos sin Contabilizar.")
                Session("ReporteContabilidad_Fondo") = ddlPortafolio.SelectedValue
                EjecutarJS(UIUtility.MostrarPopUp("frmVisorOperacionesNoContabilizadas.aspx?vfechaproceso=" + tbFechaOperacion.Text.Trim() + "&vfondo=" + ddlPortafolio.SelectedValue + "&vegreso=" + "&vdescripcionFondo=" + ddlPortafolio.SelectedItem.Text, "no", 800, 600, 40, 150, "no", "yes", "yes", "yes"), False)
                Exit Sub
            End If
            Dim oPortafolio As New PortafolioBM
            oPortafolio.ModificarCierreContable(ddlPortafolio.SelectedValue, FechaNuevaDec, DatosRequest)
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(FechaNuevaDec)
            AlertaJS("El cierre se realizó satisfactoriamente." + Mensaje)
            'OT 10031 Fin
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub btnRevertirCierre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRevertirCierre.Click
        Try
            Dim oFeriadoBM As New FeriadoBM
            Dim oPortafolio As New PortafolioBM
            Dim objBM As New CuentasPorCobrarBM
            Dim FechaNueva As Date = Convert.ToDateTime(tbFechaOperacion.Text).AddDays(-1)
            Dim FechaNuevaDec As Decimal
            If oFeriadoBM.BuscarPorFecha(CType(FechaNueva.ToString("yyyyMMdd"), Decimal), VALIDAFERIADO) = True Then
                While oFeriadoBM.BuscarPorFecha(CType(FechaNueva.ToString("yyyyMMdd"), Decimal), VALIDAFERIADO)
                    FechaNueva = FechaNueva.AddDays(-1)
                End While
            End If
            FechaNuevaDec = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
            oPortafolio.ModificarCierreContable(ddlPortafolio.SelectedValue, FechaNuevaDec, DatosRequest)
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(FechaNuevaDec)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim opcion As String
            Dim dtConsulta As New DataTable
            opcion = Me.RbReportes.SelectedValue
            Session("titulo") = opcion
            Session("ReporteContabilidad_Fondo") = Nothing
            Session("FechaOperacionInicio") = Nothing
            Session("FechaOperacionFin") = Nothing
            Session("FechaOperacionInicio") = tbFechaOperacion.Text.Trim()
            Session("FechaOperacionFin") = tbFechaOperacion.Text.Trim()
            Session("ReporteContabilidad_Fondo") = ddlPortafolio.SelectedValue
            Session("ReporteContabilidad_DescripcionFondo") = ddlPortafolio.SelectedItem.Text.Trim
            Session("CodigoMercado") = ""
            If opcion = "" Then
                AlertaJS("Debe seleccionar un Reporte")
            Else
                EjecutarJS(UIUtility.MostrarPopUp("Reportes/frmVisorAsientosContables.aspx", "no", 800, 600, 40, 150, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
End Class