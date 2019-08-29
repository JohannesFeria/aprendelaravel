Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System
Imports System.IO

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfases
    Inherits BasePage


#Region "/*Variables*/"
    Dim oArchivoPlanoBM As New ArchivoPlanoBM
    Dim oArchivoPlanoBE As New DataSet
    Dim oVaxRegauxBM As New VaxRegauxBM
    Dim oVaxRegauxBE As New DataSet
    Dim oVaxRegedBM As New VaxRegedBM
    Dim oVaxRegedBE As New DataSet
    Dim oVaxTitcusBM As New VaxTitcusBM
    Dim oVaxTitcusBE As New DataSet
    Dim oVaxCajaSITBM As New VaxCajaSITBM
    Dim oVaxCajaSITBE As New DataSet
    Dim oVaxInterfazBM As New VaxInterfazBM
    Dim oVaxCustmanBE As New DataSet
    Dim oVaxDvdosBE As New DataSet
    Dim oVaxInfodiaBE As New DataSet
    Dim oVaxTransacBE As New DataSet
    Dim oVaxComisionBE As New DataSet
    Dim oVaxAuxbcrBE As New DataSet
    Dim strmensaje As String
    Dim sFileName As String
    Dim portafolio As String
    Dim fondo As String
    Dim fechaVax As String
    Dim ext As String
    Dim errorGenerar As Boolean = False
    Dim escribir As Boolean = True
    Protected WithEvents btnImprimir As System.Web.UI.WebControls.ImageButton
#End Region

#Region "/*Metodos Personalizados*/"
    Private Sub CargarCombos()

        Dim dtPortafolio As DataTable
        Dim dtParametros As DataTable

        Dim oPortafolioBM As New PortafolioBM
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        dtPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", False)
        dtParametros = oParametrosGeneralesBM.Listar("GenerarVAX", Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlInterface, dtParametros, "Valor", "Comentario", True)
        ddlInterface.Items.Insert(1, New ListItem("AUXBCR, REGDET, REGAUX", "AUXDETAUX"))

    End Sub

    Private Sub GenerarCustman()
        errorGenerar = False
        Dim i As Integer
        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        oVaxCustmanBE = oVaxInterfazBM.SeleccionarVaxCustman(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
        extension = ViewState("Extension")

        If oVaxCustmanBE.Tables(0).Rows.Count > 0 And tbFechaVAX.Text <> "" Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "CUSM_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            'Obtener el detalle del archivo
            i = 0
            While (i < oVaxCustmanBE.Tables(0).Rows.Count())
                registro = ""

                'TotalFisicoFondo
                arrCadenas = Split(oVaxCustmanBE.Tables(0).Rows(i).Item("TotalFisicoFondo").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(10, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec
                'TotalFisicoEncaje
                arrCadenas = Split(oVaxCustmanBE.Tables(0).Rows(i).Item("TotalFisicoEncaje").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(10, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec
                'TotalAnotCuentaFondo
                arrCadenas = Split(oVaxCustmanBE.Tables(0).Rows(i).Item("TotalAnotCuentaFondo").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(10, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec
                'TotalAnotCuentaEncaje 
                arrCadenas = Split(oVaxCustmanBE.Tables(0).Rows(i).Item("TotalAnotCuentaEncaje").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                If arrCadenas.Length = 2 Then

                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'Escribir Detalle
                If (registro.Length = 73) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el CUSM_" + FechaArchivoVax + "." + extension
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo CUSM_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError1.Visible = True
            msgError1.Text = "No existen datos para el CUSM"
        End If

    End Sub

    Private Sub GenerarDvdos()
        errorGenerar = False

        Dim i As Integer
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        oVaxDvdosBE = oVaxInterfazBM.SeleccionarVaxDvdos(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
        extension = ViewState("Extension")

        If tbFechaVAX.Text <> "" Then

            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "DVDOS_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "DVDOS_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            'Obtener el detalle del archivo
            If oVaxDvdosBE.Tables(0).Rows.Count <= 0 Then
                tw.WriteLine("                              00000000000000000") 'Igual que en MIDAS
            Else
                i = 0
                While (i < oVaxDvdosBE.Tables(0).Rows.Count())
                    registro = ""

                    registro += Left(oVaxDvdosBE.Tables(0).Rows(i).Item("CuentaContable").ToString().PadRight(14, " "), 14)
                    registro += oVaxDvdosBE.Tables(0).Rows(i).Item("SinonCodInstrumento").ToString().PadRight(10, " ")
                    registro += oVaxDvdosBE.Tables(0).Rows(i).Item("SinonCodEmisor").ToString().PadRight(10, " ")
                    registro += Format(oVaxDvdosBE.Tables(0).Rows(i).Item("ValorCuponRecup"), "0000000000.0000000")

                    'Escribir Detalle
                    If (registro.Length = 52) Then
                        tw.WriteLine(registro)
                        i = i + 1
                    Else
                        errorGenerar = True
                        Exit While
                    End If
                End While
            End If

            tw.Close()

            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el DVDOS_" + FechaArchivoVax + "." + extension
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo DVDOS_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError1.Visible = True
            msgError1.Text = "Fecha Invalida" 'RGF 20080704
        End If

    End Sub


    Private Sub GenerarInfodia()
        errorGenerar = False

        Dim i As Integer
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        oVaxInfodiaBE = oVaxInterfazBM.SeleccionarVaxInfodia(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
        extension = ViewState("Extension")

        If oVaxInfodiaBE.Tables(0).Rows.Count > 0 And tbFechaVAX.Text <> "" Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "INFO_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "INFO_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            'Obtener el detalle del archivo
            i = 0
            While (i < oVaxInfodiaBE.Tables(0).Rows.Count())
                registro = ""
                registro = oVaxInfodiaBE.Tables(0).Rows(i).Item("FechaInfDiario").ToString()
                registro += oVaxInfodiaBE.Tables(0).Rows(i).Item("NumItemSBS").ToString().PadLeft(14, "0")
                registro += Format(oVaxInfodiaBE.Tables(0).Rows(i).Item("MontoSolesItem"), "00000000000.0000000")
                registro += "00000000000.0000000"

                'Escribir Detalle
                If (registro.Length = 60) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError2.Visible = True
                msgError2.Text = "Se ha generado correctamente el INFO_" + FechaArchivoVax + "." + extension
            Else
                msgError2.Visible = True
                msgError2.Text = "Error al generar el archivo INFO_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError2.Visible = True
            msgError2.Text = "No existen datos para el INFO"
        End If

    End Sub


    Private Sub GenerarAuxbcr()

        errorGenerar = False

        Dim i As Integer
        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim cadena As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        Try

            oVaxAuxbcrBE = oVaxInterfazBM.SeleccionarVaxAuxbcr(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
            extension = ViewState("Extension")

            If oVaxAuxbcrBE.Tables(0).Rows.Count > 0 And tbFechaVAX.Text <> "" Then
                FechaProceso = Me.tbFechaVAX.Text
                FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
                sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "AUXBCR_" + FechaArchivoVax + "." + extension

                Dim tw As TextWriter
                tw = New StreamWriter(sFileName)

                'Obtener el detalle del archivo
                i = 0
                While (i < oVaxAuxbcrBE.Tables(0).Rows.Count())
                    registro = ""

                    'CodigoCuentaBCR
                    cadena = oVaxAuxbcrBE.Tables(0).Rows(i).Item("CodigoCuentaBCR").ToString().PadRight(10, " ")
                    registro = registro + cadena

                    'TotalMontoDolares
                    arrCadenas = Split(oVaxAuxbcrBE.Tables(0).Rows(i).Item("TotalMontoDolares").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(10, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(7, "0")
                    Else
                        parteDec = parteDecAux.PadRight(7, "0")
                    End If
                    registro = registro + parteEnt + "." + parteDec

                    'TotalMontoSoles
                    arrCadenas = Split(oVaxAuxbcrBE.Tables(0).Rows(i).Item("TotalMontoSoles").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(10, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(7, "0")
                    Else
                        parteDec = parteDecAux.PadRight(7, "0")
                    End If
                    registro = registro + parteEnt + "." + parteDec

                    'Escribir Detalle
                    If (registro.Length = 46) Then
                        tw.WriteLine(registro)
                        i = i + 1
                    Else
                        errorGenerar = True
                        Exit While
                    End If
                End While

                tw.Close()

                If Not errorGenerar Then
                    msgError1.Visible = True
                    msgError1.Text = "Se ha generado correctamente el AUXBCR_" + FechaArchivoVax + "." + extension
                Else
                    msgError1.Visible = True
                    msgError1.Text = "Error al generar el archivo AUXBCR_" + FechaArchivoVax + "." + extension
                End If
            Else
                msgError1.Visible = True
                msgError1.Text = "No existen datos para el AUXBCR"
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub


    Private Sub GenerarTransac()
        errorGenerar = False

        Dim i As Integer
        Dim Orden As String
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String = ViewState("Extension")
        Dim registro As String = ""
        Dim tw As TextWriter = Nothing

        Try

            oVaxTransacBE = oVaxInterfazBM.SeleccionarVaxTransac(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), datosrequest)
            If (oVaxTransacBE.Tables.Count = 0) Then
                msgError3.Text = "No existen datos para el TRAN"
                Exit Try
            End If
            If oVaxTransacBE.Tables(0).Rows.Count > 0 And tbFechaVAX.Text <> "" Then
                FechaProceso = Me.tbFechaVAX.Text
                FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
                sFileName = Me.rutaArchivoVax(Me.MyFile.Text, Me.ddlPortafolio.SelectedValue) + "TRAN_" + FechaArchivoVax + "." + extension

                tw = New StreamWriter(sFileName)

                'Obtener el detalle del archivo
                i = 0
                While (i < oVaxTransacBE.Tables(0).Rows.Count())
                    registro = ""
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("FechaOperacion").ToString()
                    registro += Left(oVaxTransacBE.Tables(0).Rows(i).Item("CuentaContable").ToString(), 14).PadRight(14, " ")
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("TipoOperacion").ToString()
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("SignoValorOperacion").ToString()
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("ValorOperacion"), "00000000000.00")
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("EquivalenciaUnid"), "0000000000.00")
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("TipoCambioSBS"), "0000.0000")

                    Orden = oVaxTransacBE.Tables(0).Rows(i).Item("NumeroOperacion").ToString().PadLeft(7, "0")
                    If Val(Mid(Orden, 2, 1)) = 0 Then Orden = Left(Orden, 1) + "0" + Right(Orden, 5)
                    registro += Orden
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("Portafolio").ToString()
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("SinonCodEmisor").ToString().PadRight(10, " ")
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("SinonCodInstrumento").ToString().PadRight(10, " ")
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("SerieEmision").ToString().PadRight(13, " ")
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("CodigoMonedaSBS").ToString()
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("FechaEmiInstrumento").ToString()
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("FechaVencInstrumento").ToString()
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("NominalOperacion"), "00000000000.00")
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("TipoCambioTransac"), "000000000.0000")
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("CodigoAgenteBolsa").ToString().PadRight(10, " ")
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("PrecioOperacion"), "000000000.0000000")
                    registro += oVaxTransacBE.Tables(0).Rows(i).Item("NumeroPoliza").ToString().PadRight(15, " ")
                    registro += Format(oVaxTransacBE.Tables(0).Rows(i).Item("PrecioEmisionSBS"), "000000000.0000000")

                    If (registro.Length = 205) Then
                        tw.WriteLine(registro)
                        i = i + 1
                    Else
                        errorGenerar = True
                        Exit While
                    End If

                End While

                tw.Close()

                If Not errorGenerar Then
                    msgError3.Visible = True
                    msgError3.Text = "Se ha generado correctamente el TRAN_" + FechaArchivoVax + "." + extension
                Else
                    msgError3.Visible = True
                    msgError3.Text = "Error al generar el archivo TRAN_" + FechaArchivoVax + "." + extension
                End If
            Else
                msgError3.Visible = True
                msgError3.Text = "No existen datos para el TRAN"
            End If

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        Finally
            If Not tw Is Nothing Then
                tw.Close()
            End If
        End Try

    End Sub


    Private Sub GenerarComision()
        errorGenerar = False

        Dim i As Integer
        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim cadena As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        oVaxComisionBE = oVaxInterfazBM.SeleccionarVaxComision(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
        extension = ViewState("Extension")

        If oVaxComisionBE.Tables(0).Rows.Count > 0 And tbFechaVAX.Text <> "" Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "COMISI_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "COMISI_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            i = 0
            While (i < oVaxComisionBE.Tables(0).Rows.Count())
                registro = ""

                'FechaOperacion
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("FechaOperacion").ToString()
                registro = registro + cadena

                'Portafolio
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("Portafolio").ToString()
                registro = registro + cadena

                'SinonCodEmisor
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("SinonCodEmisor").ToString().PadRight(10, " ")
                registro = registro + cadena

                'SinonCodInstrumento
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("SinonCodInstrumento").ToString().PadRight(10, " ")
                registro = registro + cadena

                'SerieEmision
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("SerieEmision").ToString().PadRight(10, " ")
                registro = registro + cadena

                'CodigoMonedaSBS
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("CodigoMonedaSBS").ToString()
                registro = registro + cadena

                'NumeroCarta
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("NumeroCarta").ToString().PadLeft(8, "0")
                registro = registro + cadena

                'NumeroPoliza
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("NumeroPoliza").ToString().PadRight(15, " ")
                registro = registro + cadena

                'MontoComisionSAB
                arrCadenas = Split(oVaxComisionBE.Tables(0).Rows(i).Item("MontoComisionSAB").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(6, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(2, "0")
                Else
                    parteDec = parteDecAux.PadRight(2, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'MontoAporteBVL
                arrCadenas = Split(oVaxComisionBE.Tables(0).Rows(i).Item("MontoAporteBVL").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(6, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(2, "0")
                Else
                    parteDec = parteDecAux.PadRight(2, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'MontoCAVALI
                arrCadenas = Split(oVaxComisionBE.Tables(0).Rows(i).Item("MontoCAVALI").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(6, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(2, "0")
                Else
                    parteDec = parteDecAux.PadRight(2, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'MontoFondoLiquidacion
                arrCadenas = Split(oVaxComisionBE.Tables(0).Rows(i).Item("MontoFondoLiquidacion").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(6, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(2, "0")
                Else
                    parteDec = parteDecAux.PadRight(2, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'MontoIGV
                arrCadenas = Split(oVaxComisionBE.Tables(0).Rows(i).Item("MontoIGV").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(6, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(2, "0")
                Else
                    parteDec = parteDecAux.PadRight(2, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'MontoCONASE
                arrCadenas = Split(oVaxComisionBE.Tables(0).Rows(i).Item("MontoCONASE").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(6, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(2, "0")
                Else
                    parteDec = parteDecAux.PadRight(2, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'IndicadorInverLocal
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("IndicadorInverLocal").ToString()
                registro = registro + cadena

                'CodigoContraparte
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("CodigoContraparte").ToString().PadRight(10, " ")
                registro = registro + cadena

                'RucContraparte
                cadena = oVaxComisionBE.Tables(0).Rows(i).Item("RucContraparte").ToString().PadLeft(16, "0")
                registro = registro + cadena

                If (registro.Length = 144) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el COMISI_" + FechaArchivoVax + "." + extension
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo COMISI_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError1.Visible = True
            msgError1.Text = "No existen datos para el COMISI"
        End If

    End Sub


    Private Sub GenerarRegAux()
        errorGenerar = False

        Dim i As Integer
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String = ViewState("Extension")
        Dim registro As String = ""

        oVaxRegauxBE = oVaxRegauxBM.SeleccionarPorCartera(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)

        If (oVaxRegauxBE.Tables(0).Rows.Count() <> 0) And (Me.tbFechaVAX.Text <> "") Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.MyFile.Text, Me.ddlPortafolio.SelectedValue) + "REGAUX_" + FechaArchivoVax + "." + extension
            sFileName = Path.Combine(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim(), "REGAUX_" + FechaArchivoVax + "." + extension)

            

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            i = 0
            While (i < oVaxRegauxBE.Tables(0).Rows.Count())
                registro = ""
                registro += oVaxRegauxBE.Tables(0).Rows(i).Item("Portafolio").ToString()
                registro += Left(oVaxRegauxBE.Tables(0).Rows(i).Item("CuentaContable").ToString().PadRight(14, " "), 14)
                registro += oVaxRegauxBE.Tables(0).Rows(i).Item("SinonCodigoInstrumento").ToString().PadRight(10, " ")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("SaldoAnteriorMonedaLocal"), "0000000000.0000000")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("TotalCompras"), "0000000000.0000000")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("TotalVentas"), "0000000000.0000000")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("TotalVencimientos"), "0000000000.0000000")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("TotalCupones"), "0000000000.0000000")
                registro += oVaxRegauxBE.Tables(0).Rows(i).Item("SignoRentabilidad").ToString()
                registro += Format(Math.Abs(oVaxRegauxBE.Tables(0).Rows(i).Item("TotalRentabilidad")), "000000000.0000000")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("SaldoDelDia1"), "0000000000.0000000")
                registro += Format(oVaxRegauxBE.Tables(0).Rows(i).Item("SaldoDelDia2"), "0000000000.0000000")

                If (registro.Length = 169) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError3.Visible = True
                msgError3.Text = "Se ha generado correctamente el REGAUX_" + FechaArchivoVax + "." + extension
            Else
                msgError3.Visible = True
                msgError3.Text = "Error al generar el archivo REGAUX_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError3.Visible = True
            msgError3.Text = "No existen datos para el REGAUX"
        End If

    End Sub


    Private Sub GenerarRegDet()
        errorGenerar = False

        Dim i As Integer
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String = ViewState("Extension")
        Dim registro As String = ""

        oVaxRegedBE = oVaxRegedBM.SeleccionarPorCartera(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)

        If (oVaxRegedBE.Tables(0).Rows.Count() <> 0) And (Me.tbFechaVAX.Text <> "") Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "REGDET_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "REGDET_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            i = 0
            While (i < oVaxRegedBE.Tables(0).Rows.Count())
                registro = ""
                registro += oVaxRegedBE.Tables(0).Rows(i).Item("Portafolio").ToString()
                registro += Left(oVaxRegedBE.Tables(0).Rows(i).Item("CuentaContable").ToString().PadRight(14, " "), 14)
                registro += oVaxRegedBE.Tables(0).Rows(i).Item("SinonCodigoInstrumento").ToString().PadRight(10, " ")
                registro += oVaxRegedBE.Tables(0).Rows(i).Item("SinonCodigoEmisor").ToString().PadRight(10, " ")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("SaldoAnteriorMonedaLocal"), "0000000000.0000000")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("TotalCompras"), "0000000000.0000000")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("TotalVentas"), "0000000000.0000000")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("TotalVencimientos"), "0000000000.0000000")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("TotalCupones"), "0000000000.0000000")
                registro += oVaxRegedBE.Tables(0).Rows(i).Item("SignoRentabilidad").ToString()
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("TotalRentabilidad"), "0000000000.0000000")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("SaldoDelDia1"), "0000000000.0000000")
                registro += Format(oVaxRegedBE.Tables(0).Rows(i).Item("SaldoDelDia2"), "0000000000.0000000")
                registro += oVaxRegedBE.Tables(0).Rows(i).Item("IndicadorCustodio").ToString()

                If (registro.Length = 181) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError2.Visible = True
                msgError2.Text = "Se ha generado correctamente el REGDET_" + FechaArchivoVax + "." + extension
            Else
                msgError2.Visible = True
                msgError2.Text = "Error al generar el archivo REGDET_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError2.Visible = True
            msgError2.Text = "No existen datos para el REGDET"
        End If

    End Sub

    Private Sub GenerarCajaSIT()
        errorGenerar = False

        Dim i As Integer
        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        oVaxCajaSITBE = oVaxCajaSITBM.SeleccionarPorCartera(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), datosrequest)
        extension = ViewState("Extension")

        If (oVaxCajaSITBE.Tables(0).Rows.Count() <> 0) And (Me.tbFechaVAX.Text <> "") Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.MyFile.Text, Me.ddlPortafolio.SelectedValue) + "CAJA_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "CAJA_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)
            i = 0
            While (i < oVaxCajaSITBE.Tables(0).Rows.Count())
                'fechaoperacion
                registro = oVaxCajaSITBE.Tables(0).Rows(i).Item("fechaoperacion").ToString().PadRight(8, " ")
                
                'FechaLiquidacion
                registro += oVaxCajaSITBE.Tables(0).Rows(i).Item("FechaLiquidacion").ToString().PadRight(8, " ")

                'Numerocuenta
                registro += oVaxCajaSITBE.Tables(0).Rows(i).Item("Numerocuenta").ToString().PadRight(20, " ")

                'CodigoMoneda
                registro += oVaxCajaSITBE.Tables(0).Rows(i).Item("CodigoMoneda").ToString().PadRight(1, " ")

                'CodigoOperacion
                registro += oVaxCajaSITBE.Tables(0).Rows(i).Item("CodigoOperacion").ToString().PadRight(6, " ")

                'CodigoTerceroOrigen
                registro += oVaxCajaSITBE.Tables(0).Rows(i).Item("CodigoTerceroOrigen").ToString().PadRight(11, " ")

                'Poliza
                Dim poliza As String = ""
                poliza = oVaxCajaSITBE.Tables(0).Rows(i).Item("Poliza").ToString
                If poliza.Length > 15 Then
                    poliza = poliza.Substring(0, 15)
                End If
                registro += poliza.ToString().PadRight(15, " ")
                
                'Importe
                arrCadenas = Split(oVaxCajaSITBE.Tables(0).Rows(i).Item("Importe").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro += parteEnt + "." + parteDec

                'Instrumento
                registro += oVaxCajaSITBE.Tables(0).Rows(i).Item("Instrumento").ToString().PadRight(10, " ")

                'Entidad
                registro += Left(oVaxCajaSITBE.Tables(0).Rows(i).Item("Entidad").ToString().PadRight(20, " "), 20)

                If (registro.Length = 118) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el CAJA_" + FechaArchivoVax + "." + extension
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo CAJA_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError1.Visible = True
            msgError1.Text = "No existen datos para la CAJA SIT"
        End If
    End Sub

    Private Sub GenerarTicus()
        errorGenerar = False
        Dim i As Integer

        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""

        oVaxTitcusBE = oVaxTitcusBM.SeleccionarPorCartera(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
        extension = ViewState("Extension")

        If (oVaxTitcusBE.Tables(0).Rows.Count() <> 0) And (Me.tbFechaVAX.Text <> "") Then
            FechaProceso = Me.tbFechaVAX.Text
            FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
            'sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "TICUS_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "TICUS_" + FechaArchivoVax + "." + extension

            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)
            i = 0
            While (i < oVaxTitcusBE.Tables(0).Rows.Count())
                'Portafolio
                registro = oVaxTitcusBE.Tables(0).Rows(i).Item("Portafolio").ToString()

                'SinonInstrumento
                registro += oVaxTitcusBE.Tables(0).Rows(i).Item("SinonInstrumento").ToString().PadRight(3, " ")

                'SinonCodigoEmisor
                registro += oVaxTitcusBE.Tables(0).Rows(i).Item("SinonCodigoEmisor").ToString().PadRight(10, " ")

                'SerieEmision
                registro += oVaxTitcusBE.Tables(0).Rows(i).Item("SerieEmision").ToString().PadRight(5, " ")

                'SimboloMoneda
                registro += oVaxTitcusBE.Tables(0).Rows(i).Item("SimboloMoneda").ToString().PadRight(3, " ")

                'ValorNominal
                arrCadenas = Split(oVaxTitcusBE.Tables(0).Rows(i).Item("ValorNominal").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'ValorizacionDelDia1
                arrCadenas = Split(oVaxTitcusBE.Tables(0).Rows(i).Item("ValorizacionDelDia1").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                'ValorizacionDelDia2
                arrCadenas = Split(oVaxTitcusBE.Tables(0).Rows(i).Item("ValorizacionDelDia2").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                If arrCadenas.Length = 2 Then
                    parteDec = arrCadenas(1).PadRight(7, "0")
                Else
                    parteDec = parteDecAux.PadRight(7, "0")
                End If
                registro = registro + parteEnt + "." + parteDec

                If (registro.Length = 79) Then
                    tw.WriteLine(registro)
                    i = i + 1
                Else
                    errorGenerar = True
                    Exit While
                End If
            End While

            tw.Close()

            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el TICUS_" + FechaArchivoVax + "." + extension
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo TICUS_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError1.Visible = True
            msgError1.Text = "No existen datos para el TICUS"
        End If

    End Sub
    Private Sub GeneraCCP()
        errorGenerar = False

        Dim i As Integer
        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim cadena As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        Dim extension As String
        Dim registro As String = ""
        Dim codigoMoneda As String

        Try
            oVaxTitcusBE = oVaxTitcusBM.GetCuentasPorCobrarPagarToVAX(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaVAX.Text), DatosRequest)
            Session("dtCCP") = oVaxTitcusBE.Tables(0)

            extension = ViewState("Extension")

            If (oVaxTitcusBE.Tables(0).Rows.Count() <> 0) And (Me.tbFechaVAX.Text <> "") Then
                FechaProceso = Me.tbFechaVAX.Text
                FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)
                'sFileName = Me.rutaArchivoVax(Me.MyFile.Text, Me.ddlPortafolio.SelectedValue) + "CCP_" + FechaArchivoVax + "." + extension
                sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "CCP_" + FechaArchivoVax + "." + extension

                Dim tw As TextWriter
                tw = New StreamWriter(sFileName)

                i = 0
                While (i < oVaxTitcusBE.Tables(0).Rows.Count())
                    registro = ""

                    'TipoRegistro
                    cadena = oVaxTitcusBE.Tables(0).Rows(i).Item("TipoRegistro").ToString()
                    registro = registro + cadena

                    'FechaOperacion
                    cadena = Convert.ToString(oVaxTitcusBE.Tables(0).Rows(i).Item("FechaOperacion"))
                    registro = registro + cadena

                    'CodigoTercero
                    cadena = oVaxTitcusBE.Tables(0).Rows(i).Item("CodigoTercero").ToString().PadRight(5, " ")
                    registro = registro + cadena

                    'Importe (Moneda local)
                    arrCadenas = Split(oVaxTitcusBE.Tables(0).Rows(i).Item("Importe").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = Left(arrCadenas(1).PadRight(2, "0"), 2)
                    Else
                        parteDec = Left(parteDecAux.PadRight(2, "0"), 2)
                    End If
                    registro = registro + parteEnt + "." + parteDec

                    'FechaLiquidacion
                    cadena = Convert.ToString(oVaxTitcusBE.Tables(0).Rows(i).Item("FechaIngreso")).ToString().PadRight(8, " ")
                    registro = registro + cadena

                    'CodigoMonedaSBS
                    codigoMoneda = oVaxTitcusBE.Tables(0).Rows(i).Item("CodigoMonedaSBS").ToString()

                    'Monto en moneda Origen (Moneda Extranjera)
                    If codigoMoneda.Equals("1") Then
                        'Si la moneda origen esta en soles, no se debe mostrar Moneda Extranjera
                        registro = registro + "              "
                    Else
                        arrCadenas = Split(oVaxTitcusBE.Tables(0).Rows(i).Item("MontoOrigen").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                        parteEnt = arrCadenas(0).Replace("-", "").PadLeft(11, "0")
                        If arrCadenas.Length = 2 Then
                            parteDec = Left(arrCadenas(1).PadRight(2, "0"), 2)
                        Else
                            parteDec = Left(parteDecAux.PadRight(2, "0"), 2)
                        End If
                        registro = registro + parteEnt + "." + parteDec
                    End If

                    'CodigoMonedaSBS
                    cadena = oVaxTitcusBE.Tables(0).Rows(i).Item("CodigoMonedaSBS").ToString()
                    registro = registro + cadena

                    'Tipo Cambio
                    arrCadenas = Split(oVaxTitcusBE.Tables(0).Rows(i).Item("TipoCambio").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(4, "0")

                    If arrCadenas.Length = 2 Then
                        parteDec = Left(arrCadenas(1).PadRight(7, "0"), 7)
                    Else
                        parteDec = Left(parteDecAux.PadRight(7, "0"), 7)
                    End If
                    registro = registro + parteEnt + "." + parteDec

                    'Codigo Operacion
                    cadena = oVaxTitcusBE.Tables(0).Rows(i).Item("CodigoOperacion").ToString().PadLeft(2, "0")
                    registro = registro + cadena

                    'Codigo SBS
                    cadena = oVaxTitcusBE.Tables(0).Rows(i).Item("CodigoSBS").ToString().PadRight(12, " ")

                    registro = registro + cadena

                    tw.WriteLine(registro)
                    i = i + 1

                End While
                tw.Flush()
                tw.Close()

                If Not errorGenerar Then
                    msgError1.Visible = True
                    msgError1.Text = "Se ha generado correctamente el CCP_" + FechaArchivoVax + "." + extension
                Else
                    msgError1.Visible = True
                    msgError1.Text = "Error al generar el archivo CCP_" + FechaArchivoVax + "." + extension
                End If
            Else
                msgError1.Visible = True
                msgError1.Text = "No existen datos para el CCP"
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarRuta()
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("012", MyBase.DatosRequest())
        ViewState("Extension") = oArchivoPlanoBE.Tables(0).Rows(0).Item(3).ToString()
        MyFile.Text = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
    End Sub


#End Region
#Region "/*Metodos de la Pagina*/"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                EstablecerFecha()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub


    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim decFechaInicio As Decimal

        Try
            Me.LimpiarMensajes()
            Select Case Me.ddlInterface.SelectedValue
                Case "AUXDETAUX"
                    Me.GenerarAuxbcr()
                    Me.GenerarRegDet()
                    Me.GenerarRegAux()

                    decFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVAX.Text)
                    EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorGestion2.aspx?pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pMercado=" + "" + "&pFechaIni=" + Convert.ToString(decFechaInicio) + "&pFechaFin=" + "" + "&pReporte=" + "AUXDETAUX", "si", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)

                Case "Grupo1"
                    Me.GenerarRegDet()
                    Me.GenerarTransac()

                    decFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVAX.Text)
                    EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorGestion.aspx?pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pMercado=" + "" + "&pFechaIni=" + Convert.ToString(decFechaInicio) + "&pFechaFin=" + "" + "&pReporte=" + "Grupo1Det", "si", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)

                Case "REGAUX"
                    Me.GenerarRegAux()
                    btnImprimir_Click(btnImprimir, New ImageClickEventArgs(0, 0))
                    decFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVAX.Text)
                Case "Grupo2"
                    Me.GenerarDvdos()
                    Me.GenerarInfodia()
                Case "TICUS"
                    Me.GenerarTicus()
                Case "CUSM"
                    Me.GenerarCustman()
                Case "COMIS"
                    Me.GenerarComision()
                Case "CAJAINV"
                    'no hay
                Case "AUXBCR"
                    Me.GenerarAuxbcr()
                Case "CCP"
                    Me.GeneraCCP()
                    btnImprimir_Click(btnImprimir, New ImageClickEventArgs(0, 0))
                Case "CAJA_SIT"
                    Me.GenerarCajaSIT()
                Case "CCP_FORW"
                    GenerarCPP_Forward()
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al procesar los datos")
        End Try
        

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim decFechaInicio As Decimal
            decFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVAX.Text)
            EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorGestion.aspx?pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pMercado=" + "" + "&pFechaIni=" + Convert.ToString(decFechaInicio) + "&pFechaFin=" + "" + "&pReporte=" + Me.ddlInterface.SelectedValue, "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub LimpiarMensajes()
        Me.msgError1.Text = ""
        Me.msgError1.Visible = False
        Me.msgError2.Text = ""
        Me.msgError2.Visible = False
        Me.msgError3.Text = ""
        Me.msgError3.Visible = False
        Me.msgError4.Text = ""
        Me.msgError4.Visible = False
        Me.msgError5.Text = ""
        Me.msgError5.Visible = False
        Me.msgError6.Text = ""
        Me.msgError6.Visible = False

    End Sub


    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Me.LimpiarMensajes()
        ddlInterface.SelectedIndex = 0
        MyFile.Text = ""
        EstablecerFecha()
    End Sub
#End Region


    Private Sub imbSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imbSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub ddlInterface_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlInterface.SelectedIndexChanged
        Me.LimpiarMensajes()
        CargarRuta()
    End Sub

    Private Function rutaArchivoVax(ByVal rutaDestino As String, ByVal carpetaFondo As String) As String
        Return rutaDestino + "\" + carpetaFondo + "\"
    End Function
    Private Sub EstablecerFecha()
        If (ddlPortafolio.SelectedIndex = -1) Then
            Return
        End If
        tbFechaVAX.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
    End Sub
    Private Sub GenerarCPP_Forward()

        Dim extension As String = "prn"
        Dim FechaProceso As Date
        Dim FechaArchivoVax As String
        FechaProceso = Me.tbFechaVAX.Text
        FechaArchivoVax = CType(FechaProceso.ToString("yyMMdd"), String)

        Dim dtForward As DataTable
        dtForward = oVaxTitcusBM.Forward_GenerarArchivoCPP(Me.ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.tbFechaVAX.Text))
        If dtForward.Rows.Count Then


            'sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedValue) + "FORW_" + FechaArchivoVax + "." + extension
            sFileName = Me.rutaArchivoVax(Me.Myfile.Text, Me.ddlPortafolio.SelectedItem.Text.Trim) + "FORW_" + FechaArchivoVax + "." + extension
            Dim tw As TextWriter
            tw = New StreamWriter(sFileName)

            For Each row As DataRow In dtForward.Rows
                tw.WriteLine(row(0))
            Next
            tw.Flush()
            tw.Close()

            If Not errorGenerar Then
                msgError1.Visible = True
                msgError1.Text = "Se ha generado correctamente el FORW_" + FechaArchivoVax + "." + extension
            Else
                msgError1.Visible = True
                msgError1.Text = "Error al generar el archivo FORW_" + FechaArchivoVax + "." + extension
            End If
        Else
            msgError1.Visible = True
            msgError1.Text = "No existen datos para el FORW"
        End If
    End Sub

End Class
