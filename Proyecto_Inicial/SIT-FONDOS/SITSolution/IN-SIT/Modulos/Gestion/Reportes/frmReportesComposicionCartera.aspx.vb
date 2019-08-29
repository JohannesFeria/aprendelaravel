Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Text
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Gestion_Reportes_frmReportesComposicionCartera
    Inherits BasePage


#Region "Variables"
    Dim oAnexoIDI3ABM As New AnexoIDI3ABM
    Dim oAnexoIDI3ABE As New DataSet
    Dim oAnexoIDI3BBM As New AnexoIDI3BBM
    Dim oAnexoIDI3BBE As New DataSet
    Dim oAnexoIDI6BM As New AnexoIDI6BM
    Dim oAnexoIDI6BE As New DataSet
    Dim oAnexoIDI7BM As New AnexoIDI7BM
    Dim oAnexoIDI7BE As New DataSet
    Dim oAnexoIDI8BM As New AnexoIDI8BM
    Dim oAnexoIDI8BE As New DataSet
    Dim oAnexoIDI9BM As New AnexoIDI9BM
    Dim oAnexoIDI9BE As New DataSet
    Dim oTerceroBM As New TercerosBM
    Dim oTerceroBE As New DataSet
    Dim oTerceroBE1 As New TercerosBE
    Dim oMonedaBM As New MonedaBM
    Dim oMonedaBE As New DataSet
    Dim oArchivoPlanoBM As New ArchivoPlanoBM
    Dim oArchivoPlanoBE As New DataSet
    Dim strmensaje As String
    Dim sFileName As String
    Dim portafolio As String
    Dim fondo As String
    Dim fechaSucave As String
    Dim longPortafolio As String
    Dim errorGenerar As Boolean = False
    Dim escribir As Boolean = True
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ibVista.Attributes.Add("onclick", "javascript:return Validar();")
        If Not Page.IsPostBack Then
            UIUtility.CargarPortafoliosOI(ddlPortafolio, Constantes.M_STR_CONDICIONAL_NO)
            CargarRuta()
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
            ViewState("vsFechaPortafolio") = tbFechaInicio.Text
        End If
    End Sub
    Private Sub CargarRuta()
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("009", MyBase.DatosRequest())
        Myfile.Text = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
    End Sub

    Private Sub ibVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibVista.Click
        Try
            Dim sOpcion As String = RbReportes.SelectedValue
            Dim oReportesGestionBM As New ReporteGestionBM
            Dim sFechaInicio As String = tbFechaInicio.Text.Trim
            Dim sPortafolio As String = ddlPortafolio.SelectedValue.Trim
            Dim sMensaje As String = "Ocurrió un error al reprocesar la información para el anexo "

            If RbReportes.SelectedIndex = -1 Then
                AlertaJS(ObtenerMensaje("ALERT173"))
                Exit Sub
            End If

            If (txtReprocesar.Value = "1") Or (txtValidacion.Value = "0") Then
                sFechaInicio = sFechaInicio.Substring(6, 4) & sFechaInicio.Substring(3, 2) & sFechaInicio.Substring(0, 2)
                If Me.RbReportes.SelectedValue <> Constantes.M_STR_TEXTO_TODOS Then
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, sOpcion, "1", DatosRequest) Then
                        AlertaJS(ObtenerMensaje("ALERT161"))
                        Exit Sub
                    End If
                End If
            End If

            'Session("ReporteContabilidad_Fondo") = ddlPortafolio.SelectedValue
            Session("ReporteContabilidad_Fondo") = ddlPortafolio.SelectedItem.Text.Trim
            Select Case Me.RbReportes.SelectedValue
                Case Constantes.M_STR_TEXTO_TODOS
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, ANEXO_A3A_03, "1", DatosRequest) Then
                        AlertaJS(sMensaje & ANEXO_A3A_03 & ".")
                        Exit Sub
                    End If
                    GenerarAnexo03()
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, ANEXO_A3B_04, "1", DatosRequest) Then
                        AlertaJS(sMensaje & ANEXO_A3B_04 & ".")
                        Exit Sub
                    End If
                    GenerarAnexo04()
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, ANEXO_A6_08, "1", DatosRequest) Then
                        AlertaJS(sMensaje & ANEXO_A6_08 & ".")
                        Exit Sub
                    End If
                    GenerarAnexo08()
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, ANEXO_A7_09, "1", DatosRequest) Then
                        AlertaJS(sMensaje & ANEXO_A7_09 & ".")
                        Exit Sub
                    End If
                    GenerarAnexo09()
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, ANEXO_A8_10, "1", DatosRequest) Then
                        AlertaJS(sMensaje & ANEXO_A8_10 & ".")
                        Exit Sub
                    End If
                    GenerarAnexo10()
                    If Not oReportesGestionBM.GeneraInformacionAnexo(sPortafolio, sFechaInicio, ANEXO_A9_11, "1", DatosRequest) Then
                        AlertaJS(sMensaje & ANEXO_A9_11 & ".")
                        Exit Sub
                    End If
                    GenerarAnexo11()
                Case ANEXO_A3A_03
                    GenerarAnexo03()
                Case ANEXO_A3B_04
                    GenerarAnexo04()
                Case ANEXO_A6_08
                    GenerarAnexo08()
                Case ANEXO_A7_09
                    GenerarAnexo09()
                Case ANEXO_A8_10
                    GenerarAnexo10()
                Case ANEXO_A9_11
                    GenerarAnexo11()
            End Select
            EjecutarJS(UIUtility.MostrarPopUp("frmVisorReportesCartera.aspx?vopcion=" & sOpcion & "&vfechainicio=" & tbFechaInicio.Text.Trim(), "no", 1010, 670, 0, 0, "no", "yes", "yes", "yes"), False)
            RbReportes.SelectedIndex = -1
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try


    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Function Validacion(Optional ByVal bTotal As Boolean = False) As Boolean
        Dim sFechaInicio As String = tbFechaInicio.Text.Trim

        If ddlPortafolio.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT159"))
            Return False
            Exit Function
        ElseIf sFechaInicio.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT160"))
            RbReportes.Items(RbReportes.SelectedIndex).Selected = False
            Return False
            Exit Function
        End If

        Return True
    End Function

    Private Sub RbReportes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RbReportes.SelectedIndexChanged
        txtReprocesar.Value = "0"
        txtValidacion.Value = "0"
        If Not Validacion() Then
            Exit Sub
        Else
            Dim sOpcion As String = RbReportes.SelectedValue

            If VerificaPreCarga() Then
                txtValidacion.Value = "0"   'No Existe Data
            Else
                txtValidacion.Value = "1"   'Si Existe Data
            End If
        End If
    End Sub

    Private Function VerificaPreCarga() As Boolean
        Dim sFechaInicio As String = tbFechaInicio.Text.Trim
        Dim sPortafolio As String = ddlPortafolio.SelectedValue.Trim
        Dim sOpcion As String = RbReportes.SelectedValue
        Dim oReporteDS As DataSet

        If (sOpcion = Constantes.M_STR_TEXTO_TODOS) Then
            Return True
        End If

        sFechaInicio = sFechaInicio.Substring(6, 4) & sFechaInicio.Substring(3, 2) & sFechaInicio.Substring(0, 2)
        oReporteDS = New ReporteGestionBM().VerificaInformacionAnexo(sPortafolio, sFechaInicio, sOpcion, DatosRequest)

        If oReporteDS.Tables(0).Rows.Count > 0 Then
            Return False    'Si Existe Data
        Else
            Return True     'No Existe Data
        End If

        oReporteDS = Nothing

    End Function

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
        ViewState("vsFechaPortafolio") = tbFechaInicio.Text
        txtReprocesar.Value = "0"
        txtValidacion.Value = "0"
        If RbReportes.SelectedIndex <> -1 Then
            RbReportes.Items(RbReportes.SelectedIndex).Selected = False
        End If
    End Sub
    Private Function ConvertiraCantidad(ByVal value As String, ByVal len As Integer, ByVal fill As Boolean, ByVal lenDecimales As Integer, ByVal removePuntoDecimal As Boolean, ByVal fillChar As String) As String
        Dim newValue As String = String.Empty
        Dim result As String = String.Empty

        newValue = value
        Dim valueString As String = value
        Dim parteEntera As String = String.Empty
        Dim parteDecimal As String = String.Empty

        If Convert.ToDecimal(value) <> 0 Then
            If removePuntoDecimal Then
                parteEntera = valueString.Substring(0, valueString.LastIndexOf("."))
                parteDecimal = valueString.Substring(valueString.LastIndexOf(".") + 1, lenDecimales)
            Else
                parteEntera = valueString
            End If
            If fill Then
                parteEntera = parteEntera.PadLeft(len - lenDecimales, Convert.ToChar(fillChar))
            End If
            result = String.Concat(parteEntera, parteDecimal)
        Else
            result = newValue.PadLeft(len, Convert.ToChar(fillChar))
        End If
        Return result
    End Function

#Region "Anexos IDI"
    Private Sub GenerarAnexo03()
        errorGenerar = False
        msgError1.Visible = False
        msgError1.Text = ""
        Dim i As Integer

        Dim cabecera As String
        Dim detalle As String
        Dim codigoFila As String
        Dim codigoInstrumento As String
        Dim horaDeNegociacion As String
        Dim unidadesT As String
        Dim precioT As String
        Dim nroSecuencia As String
        Dim indicadorMov As String
        Dim fechaLiquidacion As String
        Dim plazoLiquidacion As String
        Dim intermediario As String
        Dim indicadorCaja As String
        Dim plazaNegociacion As String
        Dim cadenaVacia As String = ""
        Dim codigoTercero As String
        Dim fechaReporte As String
        Dim tw As TextWriter
        Dim totalRegistros As Integer = 0
        Dim existeFile As Boolean = False

        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaReporte = tbFechaInicio.Text.Substring(6, 4) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        fechaSucave = tbFechaInicio.Text.Substring(8, 2) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        longPortafolio = portafolio.Length - 1
        fondo = portafolio.Substring(longPortafolio, 1)
        'Determinar el Archivo
        sFileName = Myfile.Text + "03" + fechaSucave + ".72" + fondo

        oAnexoIDI3ABE = oAnexoIDI3ABM.SeleccionarPortafolioFecha(portafolio, fechaReporte, MyBase.DatosRequest())
        totalRegistros = oAnexoIDI3ABE.Tables(0).Rows.Count

        If (ddlPortafolio.SelectedValue() <> "") Then

            If (File.Exists(sFileName)) = False Then
                existeFile = False
            End If

            oTerceroBE = oTerceroBM.SeleccionarPorFiltro("", "", portafolio, "", "", "", MyBase.DatosRequest())
            'Extraer valores para el detalle
            If (oTerceroBE.Tables(0).Rows.Count() <> 0) Then
                codigoTercero = oTerceroBE.Tables(0).Rows(0).Item(0)
            Else
                errorGenerar = True
            End If

            If totalRegistros > 0 Then
                fechaReporte = oAnexoIDI3ABE.Tables(0).Rows(0).Item(1)
            End If

            tw = New StreamWriter(sFileName)
            cabecera = "072" + fondo + "03" + "00241" + fechaReporte + "017000000000000000"
            tw.WriteLine(cabecera)

            If oAnexoIDI3ABE.Tables(0).Rows.Count = 0 Then
                cabecera = "0000010000000000000000000000000000000000000000000000000000000000000000000000000000000000"
                'Escribir la cabecera
                tw.WriteLine(cabecera)
            End If

            'Obtener el detalle del archivo
            i = 0
            While (i < oAnexoIDI3ABE.Tables(0).Rows.Count())
                'Codigo de fila
                codigoFila = oAnexoIDI3ABE.Tables(0).Rows(i).Item(2)
                detalle = codigoFila.PadLeft(6, "0")

                'Codigo de Instrumento
                codigoInstrumento = oAnexoIDI3ABE.Tables(0).Rows(i).Item(3)
                detalle = detalle + codigoInstrumento

                horaDeNegociacion = oAnexoIDI3ABE.Tables(0).Rows(i).Item(5)
                If (horaDeNegociacion.Length >= 8) Then
                    horaDeNegociacion = horaDeNegociacion.Substring(0, 2) + horaDeNegociacion.Substring(3, 2) + horaDeNegociacion.Substring(6, 2) 'sin los dos puntos
                Else
                    horaDeNegociacion = "000000"
                End If

                detalle = detalle + horaDeNegociacion

                nroSecuencia = oAnexoIDI3ABE.Tables(0).Rows(i).Item(6)
                detalle = detalle + nroSecuencia.PadLeft(4, "0")

                indicadorMov = oAnexoIDI3ABE.Tables(0).Rows(i).Item(7)
                detalle = detalle + indicadorMov


                unidadesT = oAnexoIDI3ABE.Tables(0).Rows(i).Item(8)
                detalle = detalle + ConvertiraCantidad(unidadesT, 18, True, 7, True, "0")

                precioT = oAnexoIDI3ABE.Tables(0).Rows(i).Item(9)
                detalle = detalle + ConvertiraCantidad(precioT, 18, True, 7, True, "0")

                'Fecha de Liquidacion
                fechaLiquidacion = oAnexoIDI3ABE.Tables(0).Rows(i).Item(10)
                detalle = detalle + fechaLiquidacion

                'Plazo de Liquidacion
                plazoLiquidacion = oAnexoIDI3ABE.Tables(0).Rows(i).Item(11)
                detalle = detalle + plazoLiquidacion.PadLeft(8, "0")

                'Intermediario
                intermediario = oAnexoIDI3ABE.Tables(0).Rows(i).Item(12)
                detalle = detalle + intermediario.PadLeft(4, " ")

                'Indicador de Caja
                indicadorCaja = oAnexoIDI3ABE.Tables(0).Rows(i).Item(13)
                detalle = detalle + indicadorCaja.PadLeft(1, " ")

                'Plaza de Negociacion
                plazaNegociacion = oAnexoIDI3ABE.Tables(0).Rows(i).Item(15)
                detalle = detalle + plazaNegociacion.PadLeft(2, " ")

                'Escribir Detalle
                tw.WriteLine(detalle)
                i = i + 1
            End While
            tw.Close()
            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el archivo 03" + fechaSucave + ".72" + fondo
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo 03" + fechaSucave + ".72" + fondo
            End If
            'End If

        Else
            msgError1.Visible = True

            If totalRegistros = 0 Then
                msgError1.Text = "No existe datos en el anexo: " + RbReportes.SelectedItem.Text + " para los parametros ingresados."
            ElseIf existeFile = False Then
                msgError1.Text = "La ruta destino es incorrecta"
            Else
                msgError1.Text = "Ha ocurido un error generando el reporte"
            End If
        End If
    End Sub

    Private Sub GenerarAnexo04()
        msgError2.Visible = False
        msgError2.Text = ""
        Dim i As Integer
        Dim cabecera As String
        Dim detalle As String
        Dim codigoFila As String
        Dim codigoInstrumento As String
        Dim horaDeNegociacion As String
        Dim indicadorMov As String
        Dim fechaEmision As String
        Dim fechaVencimiento As String
        Dim valorinicialT As String
        Dim ValorVencimientoT As String
        Dim tipoTasa As String
        Dim TasaInteresAnualT As String
        Dim plazoLiquidacion As String
        Dim baseAnual As String
        Dim metodoValorizacion As String
        Dim intermediario As String
        Dim indicadorCaja As String
        Dim plazaNegociacion As String
        Dim codigoTercero As String
        Dim fechaReporte As String
        Dim tw As TextWriter
        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaReporte = tbFechaInicio.Text.Substring(6, 4) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        fechaSucave = tbFechaInicio.Text.Substring(8, 2) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        longPortafolio = portafolio.Length - 1
        fondo = portafolio.Substring(longPortafolio, 1)

        'Determinar el Archivo
        sFileName = Myfile.Text + "04" + fechaSucave + ".72" + fondo
        
        Dim totalRegistros As Integer = 0
        Dim existeFile As Boolean = False

        oAnexoIDI3BBE = oAnexoIDI3BBM.SeleccionarPortafolioFecha(portafolio, fechaReporte, MyBase.DatosRequest())

        totalRegistros = oAnexoIDI3BBE.Tables(0).Rows.Count()

        If (ddlPortafolio.SelectedValue() <> "") Then
            oTerceroBE = oTerceroBM.SeleccionarPorFiltro("", "", portafolio, "", "", "", MyBase.DatosRequest())
            If (File.Exists(sFileName)) = False Then
                existeFile = False
            End If

            'Extraer valores para el detalle
            If (oTerceroBE.Tables(0).Rows.Count() <> 0) Then
                codigoTercero = oTerceroBE.Tables(0).Rows(0).Item(0)
            Else
                errorGenerar = True
            End If

            If (totalRegistros > 0) Then
                fechaReporte = oAnexoIDI3BBE.Tables(0).Rows(0).Item(1)
            End If

            tw = New StreamWriter(sFileName)
            cabecera = "072" + fondo + "04" + "00241" + fechaReporte + "017000000000000000"

            'Escribir la cabecera
            tw.WriteLine(cabecera)

            'Obtener el detalle del archivo
            i = 0
            While (i < oAnexoIDI3BBE.Tables(0).Rows.Count())
                'Codigo de fila
                codigoFila = oAnexoIDI3BBE.Tables(0).Rows(i).Item(2)
                detalle = codigoFila.PadLeft(6, "0")

                'Codigo de Instrumento
                codigoInstrumento = oAnexoIDI3BBE.Tables(0).Rows(i).Item(3)
                detalle = detalle + codigoInstrumento

                'Hora de negociacion
                horaDeNegociacion = oAnexoIDI3BBE.Tables(0).Rows(i).Item(5)
                horaDeNegociacion = horaDeNegociacion.Substring(0, 2) + horaDeNegociacion.Substring(3, 2) + horaDeNegociacion.Substring(6, 2) 'sin los dos puntos
                detalle = detalle + horaDeNegociacion

                'Indicador de Movimiento
                indicadorMov = oAnexoIDI3BBE.Tables(0).Rows(i).Item(6)
                detalle = detalle + indicadorMov.PadLeft(1, " ")

                'Fecha Emision 
                fechaEmision = oAnexoIDI3BBE.Tables(0).Rows(i).Item(7)
                detalle = detalle + fechaEmision.PadLeft(8, " ")

                'Fecha Vencimiento
                If Not oAnexoIDI3BBE.Tables(0).Rows(i).Item(8) Is DBNull.Value Then
                    fechaVencimiento = oAnexoIDI3BBE.Tables(0).Rows(i).Item(8)
                    detalle = detalle + fechaVencimiento.PadLeft(8, " ")
                End If
                
                'valor Inicial
                valorinicialT = oAnexoIDI3BBE.Tables(0).Rows(i).Item(9)
                detalle = detalle + ConvertiraCantidad(valorinicialT, 18, True, 7, True, "0")

                'valor Vencimiento
                ValorVencimientoT = oAnexoIDI3BBE.Tables(0).Rows(i).Item(10)

                detalle = detalle + ConvertiraCantidad(ValorVencimientoT, 18, True, 7, True, "0")
                
                'tipo de Tasa
                tipoTasa = oAnexoIDI3BBE.Tables(0).Rows(i).Item(11)

                'Para el MIDAS Tasa efectiva=1, Tasa Nominal=2
                'mientras q para el SIT es al revés: Tasa efectiva=2, Tasa Nominal=1
                If tipoTasa.Equals("1") Then
                    tipoTasa = "2"
                ElseIf tipoTasa.Equals("2") Then
                    tipoTasa = "1"
                End If

                tipoTasa = tipoTasa.PadLeft(2, "0")
                detalle = detalle + tipoTasa

                'tasa de interes anual
                TasaInteresAnualT = oAnexoIDI3BBE.Tables(0).Rows(i).Item(12)
                detalle = detalle + ConvertiraCantidad(TasaInteresAnualT, 11, True, 9, True, "0")
                
                'Plazo Liquidacion
                plazoLiquidacion = oAnexoIDI3BBE.Tables(0).Rows(i).Item(13)
                plazoLiquidacion = plazoLiquidacion.PadLeft(8, "0")
                detalle = detalle + plazoLiquidacion

                'Base Anual
                baseAnual = oAnexoIDI3BBE.Tables(0).Rows(i).Item(14)
                detalle = detalle + baseAnual.ToString.PadLeft(3, " ")
                
                'Metodo de Valorizacion
                metodoValorizacion = oAnexoIDI3BBE.Tables(0).Rows(i).Item(15)
                detalle = detalle + metodoValorizacion.PadLeft(2, " ")

                'Intermediario
                intermediario = oAnexoIDI3BBE.Tables(0).Rows(i).Item(16)
                detalle = detalle + Left(intermediario, 4)

                'Indicador de Caja
                indicadorCaja = oAnexoIDI3BBE.Tables(0).Rows(i).Item(17)
                detalle = detalle + indicadorCaja.PadLeft(1, " ")

                'Plaza de Negociacion
                plazaNegociacion = oAnexoIDI3BBE.Tables(0).Rows(i).Item(18)
                detalle = detalle + plazaNegociacion.PadLeft(2, " ")

                'Escribir Detalle
                tw.WriteLine(detalle)

                i = i + 1
            End While
            tw.Close()
            If Not errorGenerar Then
                msgError2.Visible = True
                msgError2.Text = "Se ha generado correctamente el archivo 04" + fechaSucave + ".72" + fondo
            Else
                msgError2.Visible = True
                msgError2.Text = "Error al generar el archivo 04" + fechaSucave + ".72" + fondo
            End If
        Else
            msgError2.Visible = True

            If totalRegistros = 0 Then
                msgError1.Text = "No existe datos en el anexo: " + RbReportes.SelectedItem.Text + " para los parametros ingresados."
            ElseIf existeFile = False Then
                msgError1.Text = "La ruta destino es incorrecta"
            Else
                msgError1.Text = "Ha ocurido un error generando el reporte"
            End If
        End If


    End Sub
    Private Sub GenerarAnexo08()
        msgError3.Visible = False
        msgError3.Text = ""

        Dim i As Integer
        Dim cabecera As String
        Dim detalle As String
        Dim codigoFila As String
        Dim codigoInstrumento As String
        Dim indicadorMov As String
        Dim fechaLiquidacion As String
        Dim plazoLiquidacion As String
        Dim tenenciaT As String
        Dim FactorT As String
        Dim totalmonorigenT As String
        Dim codigoMoneda As String
        Dim codigoMonedaSBS As String = ""
        Dim indicadorCaja As String
        Dim totalMonlocalT As String
        Dim existeFile As Boolean = False
        Dim totalRegistros As Integer = 0
        Dim codigoTercero As String
        Dim fechaReporte As String

        'Portafolio y Fondo
        Dim tw As TextWriter
        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaReporte = tbFechaInicio.Text.Substring(6, 4) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        fechaSucave = tbFechaInicio.Text.Substring(8, 2) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        longPortafolio = portafolio.Length - 1
        fondo = portafolio.Substring(longPortafolio, 1)
        'Determinar el Archivo
        sFileName = Myfile.Text + "08" + fechaSucave + ".72" + fondo

        oAnexoIDI6BE = oAnexoIDI6BM.SeleccionarPortafolioFecha(portafolio, fechaReporte, MyBase.DatosRequest())

        totalRegistros = oAnexoIDI6BE.Tables(0).Rows.Count()

        If (ddlPortafolio.SelectedValue() <> "") Then
            oTerceroBE = oTerceroBM.SeleccionarPorFiltro("", "", portafolio, "", "", "", MyBase.DatosRequest())

            If (File.Exists(sFileName)) = False Then
                existeFile = False
            End If

            'Extraer valores para el detalle
            If (oTerceroBE.Tables(0).Rows.Count <> 0) Then
                codigoTercero = oTerceroBE.Tables(0).Rows(0).Item(0)
            Else
                errorGenerar = True
            End If

            If (totalRegistros > 0) Then
                fechaReporte = oAnexoIDI6BE.Tables(0).Rows(0).Item(1)
            End If

            tw = New StreamWriter(sFileName)
            cabecera = "072" + fondo + "08" + "00241" + fechaReporte + "017000000000000000"

            'Escribir la cabecera
            tw.WriteLine(cabecera)

            'Obtener el detalle del archivo
            i = 0
            While (i < oAnexoIDI6BE.Tables(0).Rows.Count())
                'Codigo de fila
                codigoFila = oAnexoIDI6BE.Tables(0).Rows(i).Item(2)
                detalle = codigoFila.PadLeft(6, "0")

                'Codigo de Instrumento
                codigoInstrumento = oAnexoIDI6BE.Tables(0).Rows(i).Item(3)
                detalle = detalle + codigoInstrumento

                'Indicador de Movimiento
                indicadorMov = oAnexoIDI6BE.Tables(0).Rows(i).Item(4)
                detalle = detalle + indicadorMov

                'Fecha de Liquidacion
                fechaLiquidacion = oAnexoIDI6BE.Tables(0).Rows(i).Item(5)
                detalle = detalle + fechaLiquidacion

                'Plazo Liquidacion
                plazoLiquidacion = oAnexoIDI6BE.Tables(0).Rows(i).Item(6)
                plazoLiquidacion = plazoLiquidacion.PadLeft(8, "0")
                detalle = detalle + plazoLiquidacion
                
                'tenencia anterior
                tenenciaT = oAnexoIDI6BE.Tables(0).Rows(i).Item(7)
                detalle = detalle + ConvertiraCantidad(tenenciaT, 18, True, 7, True, "0")

                'Factor
                FactorT = oAnexoIDI6BE.Tables(0).Rows(i).Item(8)
                detalle = detalle + ConvertiraCantidad(FactorT, 18, True, 15, True, "0")
                
                'Codigo de Moneda
                codigoMoneda = oAnexoIDI6BE.Tables(0).Rows(i).Item(9)

                'extraer de la tabla Moneda
                oMonedaBE = oMonedaBM.SeleccionarPorFiltro(codigoMoneda, "", "", "", "", MyBase.DatosRequest())
                If (oMonedaBE.Tables(0).Rows.Count <> 0) Then
                    codigoMonedaSBS = oMonedaBE.Tables(0).Rows(0).Item("CodigoMonedaSBS")
                Else
                    errorGenerar = True
                End If
                detalle = detalle + codigoMonedaSBS.PadLeft(1, " ")

                'Moneda Original
                totalmonorigenT = oAnexoIDI6BE.Tables(0).Rows(i).Item(10)
                detalle = detalle + ConvertiraCantidad(totalmonorigenT, 18, True, 7, True, "0")
                
                'Indicador de Caja
                indicadorCaja = oAnexoIDI6BE.Tables(0).Rows(i).Item(11)
                detalle = detalle + indicadorCaja.PadLeft(1, " ")
                
                'Moneda Local
                totalMonlocalT = oAnexoIDI6BE.Tables(0).Rows(i).Item(12)
                detalle = detalle + ConvertiraCantidad(totalMonlocalT, 18, True, 7, True, "0")

                'Escribir Detalle
                tw.WriteLine(detalle)

                i = i + 1
            End While
            tw.Close()
            If (Not errorGenerar) Then
                msgError3.Visible = True
                msgError3.Text = "Se ha generado correctamente el archivo 08" + fechaSucave + ".72" + fondo
            Else
                msgError3.Visible = True
                msgError3.Text = "Error al generar el archivo 08" + fechaSucave + ".72" + fondo
            End If
        Else
            msgError3.Visible = True

            If totalRegistros = 0 Then
                msgError1.Text = "No existe datos en el anexo: " + RbReportes.SelectedItem.Text + " para los parametros ingresados."
            ElseIf existeFile = False Then
                msgError1.Text = "La ruta destino es incorrecta"
            Else
                msgError1.Text = "Ha ocurido un error generando el reporte"
            End If
        End If

    End Sub
    Private Sub GenerarAnexo09()
        msgError4.Visible = False
        msgError4.Text = ""

        Dim i As Integer
        Dim cabecera As String
        Dim detalle As String
        Dim codigoFila As String
        Dim horaNegociacion As String
        Dim codigoMonedaCompra As String
        Dim monedaCompraSBS As String = ""
        Dim montoCompraT As String
        Dim codigoMonedaVenta As String
        Dim monedaVentaSBS As String = ""
        Dim montoVentaT As String
        Dim TipoCambioT As String
        Dim plazoLiquidacion As String
        Dim fechaLiquidacion As String
        Dim indicadorCaja As String
        Dim plazaNegociacion As String
        Dim codigoTercero As String
        Dim fechaReporte As String

        Dim tw As TextWriter
        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaReporte = tbFechaInicio.Text.Substring(6, 4) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        fechaSucave = tbFechaInicio.Text.Substring(8, 2) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        longPortafolio = portafolio.Length - 1
        fondo = portafolio.Substring(longPortafolio, 1)
        'Determinar el Archivo
        sFileName = Myfile.Text + "09" + fechaSucave + ".72" + fondo
        
        Dim existeFile As Boolean = False
        Dim totalRegistros As Integer = 0

        oAnexoIDI7BE = oAnexoIDI7BM.SeleccionarPortafolioFecha(portafolio, fechaReporte, MyBase.DatosRequest())

        totalRegistros = oAnexoIDI7BE.Tables(0).Rows.Count()

        If (ddlPortafolio.SelectedValue() <> "") Then
            oTerceroBE = oTerceroBM.SeleccionarPorFiltro("", "", portafolio, "", "", "", MyBase.DatosRequest())

            If (File.Exists(sFileName)) = False Then

                existeFile = False

            End If

            'Extraer valores para el detalle
            If (oTerceroBE.Tables(0).Rows.Count <> 0) Then
                codigoTercero = oTerceroBE.Tables(0).Rows(0).Item(0)
            Else
                errorGenerar = True
            End If

            If (totalRegistros > 0) Then
                fechaReporte = oAnexoIDI7BE.Tables(0).Rows(0).Item(1)
            End If

            tw = New StreamWriter(sFileName)
            cabecera = "072" + fondo + "09" + "00241" + fechaReporte + "017000000000000000"

            'Escribir la cabecera
            tw.WriteLine(cabecera)

            'Obtener el detalle del archivo
            i = 0
            While (i < oAnexoIDI7BE.Tables(0).Rows.Count())
                'Codigo de fila
                codigoFila = oAnexoIDI7BE.Tables(0).Rows(i).Item(2)
                detalle = codigoFila.PadLeft(6, "0")

                'Codigo de Emisor
                codigoTercero = oAnexoIDI7BE.Tables(0).Rows(i).Item(3)
                detalle = detalle + codigoTercero.PadLeft(4, " ")

                'Hora de Negociacion
                horaNegociacion = oAnexoIDI7BE.Tables(0).Rows(i).Item(5)
                horaNegociacion = horaNegociacion.Substring(0, 2) + horaNegociacion.Substring(3, 2) + horaNegociacion.Substring(6, 2) 'sin los dos puntos
                detalle = detalle + horaNegociacion

                'Codigo de Moneda
                codigoMonedaCompra = oAnexoIDI7BE.Tables(0).Rows(i).Item(6)

                oMonedaBE = oMonedaBM.SeleccionarPorFiltro(codigoMonedaCompra, "", "", "", "", MyBase.DatosRequest())
                If (oMonedaBE.Tables(0).Rows.Count <> 0) Then
                    monedaCompraSBS = oMonedaBE.Tables(0).Rows(0).Item("CodigoMonedaSBS")
                Else
                    errorGenerar = True
                End If
                detalle = detalle + monedaCompraSBS.PadLeft(1, " ")
                'ListBox1.Items.Add(monedaCompraSBS)

                'Monto Compra
                montoCompraT = oAnexoIDI7BE.Tables(0).Rows(i).Item(7)
                detalle = detalle + ConvertiraCantidad(montoCompraT, 18, True, 7, True, "0")
                'ListBox1.Items.Add(montoCompraEnt + montoCompraDec)

                'Moneda Venta
                'Codigo de Moneda
                codigoMonedaVenta = oAnexoIDI7BE.Tables(0).Rows(i).Item(8)
                'extraer de la tabla Moneda
                oMonedaBE = oMonedaBM.SeleccionarPorFiltro(codigoMonedaVenta, "", "", "", "", MyBase.DatosRequest())
                If (oMonedaBE.Tables(0).Rows.Count <> 0) Then
                    monedaVentaSBS = oMonedaBE.Tables(0).Rows(0).Item("CodigoMonedaSBS")
                Else
                    errorGenerar = True
                End If
                detalle = detalle + monedaVentaSBS.PadLeft(1, " ")

                'Monto Venta
                montoVentaT = oAnexoIDI7BE.Tables(0).Rows(i).Item(9)
                detalle = detalle + ConvertiraCantidad(montoVentaT, 18, True, 7, True, "0")
                
                'Tipo de Cambio
                TipoCambioT = oAnexoIDI7BE.Tables(0).Rows(i).Item(10)
                detalle = detalle + ConvertiraCantidad(TipoCambioT, 18, True, 7, True, "0")

                'Fecha Liquidacion
                fechaLiquidacion = oAnexoIDI7BE.Tables(0).Rows(i).Item(11)
                detalle = detalle + fechaLiquidacion
                
                'Plazo Liquidacion
                plazoLiquidacion = oAnexoIDI7BE.Tables(0).Rows(i).Item(13)
                plazoLiquidacion = plazoLiquidacion.PadLeft(8, "0")
                detalle = detalle + plazoLiquidacion
                
                'Indicador de Caja
                indicadorCaja = oAnexoIDI7BE.Tables(0).Rows(i).Item(12)
                detalle = detalle + indicadorCaja.PadLeft(1, " ")
                
                'Plaza Negociacion
                plazaNegociacion = oAnexoIDI7BE.Tables(0).Rows(i).Item(14)
                detalle = detalle + plazaNegociacion.PadLeft(2, " ")
                
                'Escribir Detalle
                tw.WriteLine(detalle)

                i = i + 1
            End While
            tw.Close()
            If Not errorGenerar Then
                msgError4.Visible = True
                msgError4.Text = "Se ha generado correctamente el archivo 09" + fechaSucave + ".72" + fondo
            Else
                msgError4.Visible = True
                msgError4.Text = "Error al generar el archivo 09" + fechaSucave + ".72" + fondo
            End If
        Else
            msgError4.Visible = True
            If totalRegistros = 0 Then
                msgError1.Text = "No existe datos en el anexo: " + RbReportes.SelectedItem.Text + " para los parametros ingresados."
            ElseIf existeFile = False Then
                msgError1.Text = "La ruta destino es incorrecta"
            Else
                msgError1.Text = "Ha ocurido un error generando el reporte"
            End If
        End If
    End Sub
    Private Sub GenerarAnexo10()
        msgError5.Visible = False
        msgError5.Text = ""

        'Variables auxiliares
        Dim i As Integer

        Dim cabecera As String
        Dim detalle As String
        Dim codigoFila As String
        Dim codigoInstrumento As String
        Dim cantidadInstrumentosT As String
        Dim instrumentosTransitoT As String
        Dim totalT As String
        Dim entidadGuardia As String
        Dim ValorizacionT As String
        Dim codigoTercero As String
        Dim fechaReporte As String

        'Portafolio y Fondo
        Dim tw As TextWriter
        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaReporte = tbFechaInicio.Text.Substring(6, 4) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        fechaSucave = tbFechaInicio.Text.Substring(8, 2) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        longPortafolio = portafolio.Length - 1
        fondo = portafolio.Substring(longPortafolio, 1)
        sFileName = Myfile.Text + "10" + fechaSucave + ".72" + fondo

        Dim existeFile As Boolean = False
        Dim totalRegistros As Integer = 0

        oAnexoIDI8BE = oAnexoIDI8BM.SeleccionarPortafolioFecha(portafolio, fechaReporte, MyBase.DatosRequest())

        totalRegistros = oAnexoIDI8BE.Tables(0).Rows.Count()

        If (ddlPortafolio.SelectedValue() <> "") Then
            oTerceroBE = oTerceroBM.SeleccionarPorFiltro("", "", portafolio, "", "", "", MyBase.DatosRequest())

            If (File.Exists(sFileName)) = False Then
                existeFile = False
            End If

            'Extraer valores para el detalle
            If (oTerceroBE.Tables(0).Rows.Count <> 0) Then
                codigoTercero = oTerceroBE.Tables(0).Rows(0).Item(0)
            Else
                errorGenerar = True
            End If

            If (totalRegistros > 0) Then
                fechaReporte = oAnexoIDI8BE.Tables(0).Rows(0).Item(1)
            End If

            tw = New StreamWriter(sFileName)
            cabecera = "072" + fondo + "10" + "00241" + fechaReporte + "017000000000000000"

            'Escribir la cabecera
            tw.WriteLine(cabecera)

            'Obtener el detalle del archivo
            i = 0
            While (i < oAnexoIDI8BE.Tables(0).Rows.Count())
                'Codigo de fila
                codigoFila = oAnexoIDI8BE.Tables(0).Rows(i).Item(2)
                detalle = codigoFila.PadLeft(6, "0")

                'Codigo Instrumento
                If IsDBNull(oAnexoIDI8BE.Tables(0).Rows(i).Item(3)) = True Then
                    codigoInstrumento = ""
                Else
                    codigoInstrumento = oAnexoIDI8BE.Tables(0).Rows(i).Item(3)
                End If
                detalle = detalle + codigoInstrumento

                'Cantidad de Instrumentos
                cantidadInstrumentosT = oAnexoIDI8BE.Tables(0).Rows(i).Item(4)
                detalle = detalle + ConvertiraCantidad(cantidadInstrumentosT, 18, True, 7, True, "0")

                'Instrumento en Transito
                instrumentosTransitoT = oAnexoIDI8BE.Tables(0).Rows(i).Item(5)
                detalle = detalle + ConvertiraCantidad(instrumentosTransitoT, 18, True, 7, True, "0")

                'Total
                totalT = oAnexoIDI8BE.Tables(0).Rows(i).Item(6)
                detalle = detalle + ConvertiraCantidad(totalT, 18, True, 7, True, "0")

                'Entidad de Guardia Fisica
                If IsDBNull(oAnexoIDI8BE.Tables(0).Rows(i).Item(8)) = True Then
                    entidadGuardia = ""
                Else
                    entidadGuardia = oAnexoIDI8BE.Tables(0).Rows(i).Item(8)
                End If
                detalle = detalle + entidadGuardia

                'Valorizacion
                ValorizacionT = oAnexoIDI8BE.Tables(0).Rows(i).Item(7)
                detalle = detalle + ConvertiraCantidad(ValorizacionT, 18, True, 7, True, "0")
                
                'Escribir Detalle
                tw.WriteLine(detalle)

                i = i + 1
            End While
            tw.Close()
            If (Not errorGenerar) Then
                msgError5.Visible = True
                msgError5.Text = "Se ha generado correctamente el archivo 10" + fechaSucave + ".72" + fondo
            Else
                msgError5.Visible = True
                msgError5.Text = "Error al generar el archivo 10" + fechaSucave + ".72" + fondo
            End If
        Else
            msgError5.Visible = True

            If totalRegistros = 0 Then
                msgError1.Text = "No existe datos en el anexo: " + RbReportes.SelectedItem.Text + " para los parametros ingresados."
            ElseIf existeFile = False Then
                msgError1.Text = "La ruta destino es incorrecta"
            Else
                msgError1.Text = "Ha ocurido un error generando el reporte"
            End If
        End If

    End Sub
    Private Sub GenerarAnexo11()
        msgError6.Visible = False
        msgError6.Text = ""

        Dim i As Integer
        Dim cabecera As String
        Dim detalle As String
        Dim codigoFila As String
        Dim codigoInstrumento As String
        Dim horaNegociacion As String
        Dim codigoMonedaVenta As String
        Dim codigoMonedaVentaSBS As String = ""
        Dim indicadorMov As String
        Dim indicadorForward As String
        Dim montoForwardT As String
        Dim precioTransaccionT As String
        Dim tipoCambioT As String
        Dim fechaVencimiento As String
        Dim plazoVencimiento As String
        Dim modalidad As String
        Dim indicadorCaja As String
        Dim plazaNegociacion As String

        Dim codigoTercero As String
        Dim fechaReporte As String

        Dim tw As TextWriter
        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaReporte = tbFechaInicio.Text.Substring(6, 4) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        fechaSucave = tbFechaInicio.Text.Substring(8, 2) + tbFechaInicio.Text.Substring(3, 2) + tbFechaInicio.Text.Substring(0, 2)
        longPortafolio = portafolio.Length - 1
        fondo = portafolio.Substring(longPortafolio, 1)

        'Determinar el Archivo
        sFileName = Myfile.Text + "11" + fechaSucave + ".72" + fondo

        Dim existeFile As Boolean = False
        Dim totalRegistros As Integer = 0

        oAnexoIDI9BE = oAnexoIDI9BM.SeleccionarPortafolioFecha(portafolio, fechaReporte, MyBase.DatosRequest())
        totalRegistros = oAnexoIDI9BE.Tables(0).Rows.Count()

        If (ddlPortafolio.SelectedValue() <> "") Then

            If (File.Exists(sFileName)) = False Then
                existeFile = False
            End If

            File.Delete(sFileName)

            oTerceroBE = oTerceroBM.SeleccionarPorFiltro("", "", portafolio, "", "", "", MyBase.DatosRequest())
            'Extraer valores para el detalle
            If (oTerceroBE.Tables(0).Rows.Count <> 0) Then
                codigoTercero = oTerceroBE.Tables(0).Rows(0).Item(0)
            Else
                errorGenerar = True
            End If
            If (totalRegistros > 0) Then
                fechaReporte = oAnexoIDI9BE.Tables(0).Rows(0).Item(1)
            End If

            tw = New StreamWriter(sFileName)
            cabecera = "072" + fondo + "11" + "00241" + fechaReporte + "017000000000000000"
            'Escribir la cabecera
            tw.WriteLine(cabecera)
            'Obtener el detalle del archivo
            i = 0
            While (i < oAnexoIDI9BE.Tables(0).Rows.Count())
                'Codigo de fila
                codigoFila = oAnexoIDI9BE.Tables(0).Rows(i).Item(2)
                detalle = codigoFila.PadLeft(6, "0")

                'Codigo Instrumento
                If IsDBNull(oAnexoIDI9BE.Tables(0).Rows(i).Item(3)) = True Then
                    codigoInstrumento = ""
                Else
                    codigoInstrumento = oAnexoIDI9BE.Tables(0).Rows(i).Item(3)
                End If
                detalle = detalle + codigoInstrumento

                'Hora Negociacion
                horaNegociacion = oAnexoIDI9BE.Tables(0).Rows(i).Item(4)
                horaNegociacion = horaNegociacion.Substring(0, 2) + horaNegociacion.Substring(3, 2) + horaNegociacion.Substring(6, 2) 'sin los dos puntos
                detalle = detalle + horaNegociacion

                'Codigo de Moneda
                codigoMonedaVenta = oAnexoIDI9BE.Tables(0).Rows(i).Item(5)

                'extraer de la tabla Moneda
                oMonedaBE = oMonedaBM.SeleccionarPorFiltro(codigoMonedaVenta, "", "", "", "", MyBase.DatosRequest())
                If (oMonedaBE.Tables(0).Rows.Count <> 0) Then
                    codigoMonedaVentaSBS = oMonedaBE.Tables(0).Rows(0).Item("CodigoMonedaSBS")
                Else
                    errorGenerar = True
                End If
                detalle = detalle + codigoMonedaVentaSBS.PadLeft(1, " ")
                
                'Indicador de Movimiento
                indicadorMov = oAnexoIDI9BE.Tables(0).Rows(i).Item(6)
                detalle = detalle + indicadorMov.PadLeft(1, " ")

                'Indicador de Tipo de Forward
                indicadorForward = oAnexoIDI9BE.Tables(0).Rows(i).Item(7)
                detalle = detalle + indicadorForward.PadLeft(1, " ")

                'Monto Forward
                If IsDBNull(oAnexoIDI9BE.Tables(0).Rows(i).Item(8)) = True Then
                    montoForwardT = "0.00"
                Else
                    montoForwardT = oAnexoIDI9BE.Tables(0).Rows(i).Item(8)
                End If
                detalle = detalle + ConvertiraCantidad(montoForwardT, 18, True, 7, True, "0")

                'Precio Transaccion
                If IsDBNull(oAnexoIDI9BE.Tables(0).Rows(i).Item(9)) = True Then
                    precioTransaccionT = "0.00"
                Else
                    precioTransaccionT = oAnexoIDI9BE.Tables(0).Rows(i).Item(9)
                End If
                
                'Fecha Vencimiento
                fechaVencimiento = oAnexoIDI9BE.Tables(0).Rows(i).Item(10)
                If fechaReporte = fechaVencimiento Then
                    precioTransaccionT = "1.0000000"
                End If

                detalle = detalle + ConvertiraCantidad(precioTransaccionT, 18, True, 7, True, "0")

                'Fecha Vencimiento
                detalle = detalle + fechaVencimiento.PadLeft(8, " ")

                'Plazo Vencimiento
                plazoVencimiento = oAnexoIDI9BE.Tables(0).Rows(i).Item(11)
                plazoVencimiento = plazoVencimiento.PadLeft(8, "0")
                detalle = detalle + plazoVencimiento

                'Modalidad
                modalidad = oAnexoIDI9BE.Tables(0).Rows(i).Item(12)
                detalle = detalle + modalidad.PadLeft(1, " ")

                'TipoCambio
                tipoCambioT = oAnexoIDI9BE.Tables(0).Rows(i).Item(13)
                detalle = detalle + ConvertiraCantidad(tipoCambioT, 18, True, 7, True, "0")

                'Indicador Caja
                indicadorCaja = oAnexoIDI9BE.Tables(0).Rows(i).Item(14)
                detalle = detalle + indicadorCaja.PadLeft(1, " ")

                'plaza Negociacion
                plazaNegociacion = oAnexoIDI9BE.Tables(0).Rows(0).Item(15)
                detalle = detalle + plazaNegociacion.PadLeft(2, " ")

                'Escribir Detalle
                tw.WriteLine(detalle)

                i = i + 1
            End While
            tw.Close()
            If Not errorGenerar Then
                msgError6.Visible = True
                msgError6.Text = "Se ha generado correctamente el archivo 11" + fechaSucave + ".72" + fondo
            Else
                msgError6.Visible = True
                msgError6.Text = "Error al Generar el archivo 11" + fechaSucave + ".72" + fondo
            End If
        Else
            msgError6.Visible = True
            If totalRegistros = 0 Then
                msgError1.Text = "No existe datos en el anexo: " + RbReportes.SelectedItem.Text + " para los parametros ingresados."
            ElseIf existeFile = False Then
                msgError1.Text = "La ruta destino es incorrecta"
            Else
                msgError1.Text = "Ha ocurido un error generando el reporte"
            End If
        End If

    End Sub
#End Region

End Class
