Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System
Imports System.IO

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfasesVAX
    Inherits BasePage


#Region "Variables"
    Dim portafolio As String
    Dim fechaVAX As String
    Dim tipoInterface As String
    Dim oArchivoPlanoBM As New ArchivoPlanoBM
    Dim oArchivoPlanoBE As New DataSet
    Dim oArchivoPlanoEstructuraBM As New ArchivoPlanoEstructuraBM
    Dim oArchivoPlanoEstructuraBE As New DataSet
    Dim oArchivosVAXBCOSBM As New ArchivosVAXBCOSBM
    Dim oArchivosVAXBCOSBE As New ArchivosVAXBCOSBE
    Dim oArchivosVAXBMIDASBM As New ArchivoVAXBMIDASBM
    Dim oArchivosVAXBMIDASBE As New ArchivosVAXBMIDASBE
    Dim oConceptoIdiBM As New ConceptoIdiBM
    Dim oConceptoIdiBE As New DataSet
    Dim strmensaje As String
    Dim sFileName As String

    Dim errorNoExiste As Boolean = False
#End Region

    Private Sub CargarRutaBCOS(ByVal rutaPart As String, ByVal portafolio As String, ByVal fVax As String)
        Dim prefijo As String = "BSIT"
        sFileName = rutaPart + portafolio + "\" + prefijo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        If (File.Exists(sFileName)) Then
            'Inserta en funcion a la estructura del la tabla del Archivo Plano
            Myfile.Text = sFileName
        Else
            AlertaJS("No existe Archivo Plano.")
        End If
        Myfile.Text = sFileName

        'Seleccionar por el tipo de portafolio
        'Select Case (portafolio)
        '    Case "HO-FONDO1"
        '        sFileName = rutaPart + portafolio.Trim() + "\" + prefijo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        '    Case "HO-FONDO2"
        '        sFileName = rutaPart + portafolio.Trim() + "\" + prefijo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        '    Case "HO-FONDO3"
        '        sFileName = rutaPart + portafolio.Trim() + "\" + prefijo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        '    Case "HOCARTADM"
        '        sFileName = rutaPart + "HO-CARTADM\" + prefijo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        'End Select
        'Myfile.Text = sFileName
    End Sub

    Private Sub CargarCombos()
        Dim dtPortafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub CargarRutaBMIDAS(ByVal rutaPart As String, ByVal portafolio As String, ByVal fVax As String)
        Dim prefijoNombreArchivo As String = "SALSIT" 'Antes BMIDAS
        sFileName = rutaPart + portafolio.Trim() + "\" + prefijoNombreArchivo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        If (File.Exists(sFileName)) Then
            'Inserta en funcion a la estructura del la tabla del Archivo Plano
            Myfile.Text = sFileName
        Else
            AlertaJS("No existe Archivo Plano.")
        End If

        Myfile.Text = sFileName
        'Select Case (portafolio)
        '    Case "HO-FONDO1"
        '        'RGF 20080625
        '        sFileName = rutaPart + portafolio.Trim() + "\" + prefijoNombreArchivo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        '    Case "HO-FONDO2"
        '        sFileName = rutaPart + portafolio.Trim() + "\" + prefijoNombreArchivo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        '    Case "HO-FONDO3"
        '        'RGF 20080625
        '        sFileName = rutaPart + portafolio.Trim() + "\" + prefijoNombreArchivo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        '    Case "HOCARTADM"
        '        sFileName = rutaPart + "HO-CARTADM\" + prefijoNombreArchivo + fVax + "." + oArchivoPlanoBE.Tables(0).Rows(0).Item(3)
        '        If (File.Exists(sFileName)) Then
        '            'Inserta en funcion a la estructura del la tabla del Archivo Plano
        '            Myfile.Text = sFileName
        '        Else
        '            AlertaJS("No existe Archivo Plano.")
        '        End If
        'End Select
        'Myfile.Text = sFileName
    End Sub
    Private Sub CargarRuta(ByVal tI As String, ByVal portafolio As String, ByVal fVax As String)
        'Seleccionar por tipo de Interfaz
        Select Case (tI)
            Case "BCOS"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("007", MyBase.DatosRequest())
                sFileName = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
                CargarRutaBCOS(sFileName, portafolio, fVax)
            Case "BMIDAS"
                oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("008", MyBase.DatosRequest())
                sFileName = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
                CargarRutaBMIDAS(sFileName, portafolio, fVax)
        End Select


    End Sub
    Private Sub CargarBCOS()
        Dim errorVacio As Boolean = False
        Dim dtNoExiste As New DataTable
        Dim drNoExisteRow As DataRow
        dtNoExiste.Columns.Add("CodigoVAXBCOS")
        errorNoExiste = False
        msgError.Visible = False
        dgNoExiste.Visible = False
        Dim tr As TextReader
        Dim fechaBCOS As Decimal
        Dim valorBCOS As Decimal
        Dim codigoIndicador As String
        Dim colPosIniValorEnt, colLongValorEnt, colPosIniValorDec, colLongValorDec, colPosIniCodigoConcepto, colLongCodigoConcepto As Integer
        Dim codigoConcepto As String
        sFileName = Myfile.Text
        If (File.Exists(sFileName) = False) Then
            AlertaJS("No existe Archivo Plano.")
            Return
        End If
        tr = New StreamReader(sFileName)
        'Recupera la estructura del archivo
        oArchivoPlanoEstructuraBE = oArchivoPlanoEstructuraBM.Listar("007", MyBase.DatosRequest())
        fechaBCOS = Convert.ToDecimal(tbFechaVAX.Text.Substring(6, 4) + tbFechaVAX.Text.Substring(3, 2) + tbFechaVAX.Text.Substring(0, 2))


        Dim sContent As String = tr.ReadLine()
        If (sContent = "") Then
            errorVacio = True
        Else

            While (sContent <> "")
                Dim oArchivosVAXBCOSRow As ArchivosVAXBCOSBE.ArchivosVAXBCOSRow
                Dim oIndicadorBM As New IndicadorBM
                colPosIniCodigoConcepto = Convert.ToInt16(oArchivoPlanoEstructuraBE.Tables(0).Rows(0).Item(4))
                colLongCodigoConcepto = Convert.ToInt16(oArchivoPlanoEstructuraBE.Tables(0).Rows(0).Item(5))
                codigoConcepto = sContent.Substring(colPosIniCodigoConcepto, colLongCodigoConcepto)
                While (codigoConcepto.Substring(codigoConcepto.Length - 1, 1) = "0")
                    codigoConcepto = codigoConcepto.Substring(0, codigoConcepto.Length - 1)
                End While
                oConceptoIdiBE = oConceptoIdiBM.SeleccionarPorCodConcepto(codigoConcepto, MyBase.DatosRequest())
                If (oConceptoIdiBE.Tables(0).Rows.Count() <> 0) Then

                    codigoIndicador = oConceptoIdiBE.Tables(0).Rows(0).Item(1)

                    If oIndicadorBM.SeleccionarPorFiltro(codigoIndicador, "", "", 0, 0, 0, "", "", "", DatosRequest).Indicador.Rows.Count = 0 Then
                        AlertaJS("Debe crear el indicador " & codigoIndicador & " para poder continuar.")
                        Exit Sub
                    End If

                    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                    If oParametrosGeneralesBM.SeleccionarPorFiltro("VAXBCOS", codigoIndicador, "", "", DatosRequest).Rows.Count = 0 Then
                        oParametrosGeneralesBM.Insertar("VAXBCOS", codigoIndicador, "", "", DatosRequest)
                    End If
                    colPosIniValorEnt = Convert.ToInt16(oArchivoPlanoEstructuraBE.Tables(0).Rows(1).Item(4))
                    colLongValorEnt = Convert.ToInt16(oArchivoPlanoEstructuraBE.Tables(0).Rows(1).Item(5))
                    colLongValorDec = Convert.ToInt16(oArchivoPlanoEstructuraBE.Tables(0).Rows(2).Item(5))

                    If sContent.Substring(colPosIniValorEnt, 1).Equals("-") Then
                        valorBCOS = Convert.ToDecimal(sContent.Substring(colPosIniValorEnt, colLongValorEnt + colLongValorDec))
                    Else
                        colPosIniValorDec = Convert.ToInt16(oArchivoPlanoEstructuraBE.Tables(0).Rows(2).Item(4))
                        valorBCOS = Convert.ToDecimal(sContent.Substring(colPosIniValorEnt, colLongValorEnt)) + Convert.ToDecimal(sContent.Substring(colPosIniValorDec, colLongValorDec))
                    End If

                    oArchivosVAXBCOSRow = oArchivosVAXBCOSBE.ArchivosVAXBCOS.NewArchivosVAXBCOSRow
                    oArchivosVAXBCOSRow.CodigoIndicador = codigoIndicador
                    oArchivosVAXBCOSRow.PortafolioSBS = ddlPortafolio.SelectedValue()
                    oArchivosVAXBCOSRow.Fecha = fechaBCOS
                    oArchivosVAXBCOSRow.Valor = valorBCOS

                    oArchivosVAXBCOSBM.Insertar(oArchivosVAXBCOSRow, MyBase.DatosRequest())
                Else
                    drNoExisteRow = dtNoExiste.NewRow
                    drNoExisteRow.Item(0) = codigoConcepto
                    dtNoExiste.Rows.Add(drNoExisteRow)
                    errorNoExiste = True
                End If
                sContent = tr.ReadLine()
            End While
        End If

        If (errorNoExiste) Then
            msgError.Visible = True
            msgError.Text = "Error... al importar los siguientes codigos de Concepto"
            dgNoExiste.Visible = True
            dgNoExiste.DataSource = dtNoExiste
            dgNoExiste.DataBind()
        ElseIf errorVacio Then
            msgError.Visible = True
            msgError.Text = "No hay datos en el Archivo Plano..."
        Else
            AlertaJS("El Archivo ha sido importado correctamente")
        End If

        tr.Close()
    End Sub

    Private Sub CargarBMIDAS()
        Dim errorVacio As Boolean = False
        Dim dtNoExiste As New DataTable
        dtNoExiste.Columns.Add("CodigoVAXBMIDAS")
        errorNoExiste = False
        msgError.Visible = False
        dgNoExiste.Visible = False
        Dim tr As TextReader
        Dim fechaCargaBMIDAS As Decimal

        Dim ArrayBMIDAS(11) As String
        Dim separador As String

        'Label1.Text = Myfile.Text
        sFileName = Myfile.Text
        If (File.Exists(sFileName) = False) Then
            AlertaJS("No existe Archivo Plano.")
            Return
        End If
        tr = New StreamReader(sFileName)
        'Recupera el Archivo Plano
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("008", MyBase.DatosRequest())
        'Cargar Separador
        separador = oArchivoPlanoBE.Tables(0).Rows(0).Item(6)
        'Fecha Tipo de Cambio
        fechaCargaBMIDAS = Convert.ToDecimal(tbFechaVAX.Text.Substring(6, 4) + tbFechaVAX.Text.Substring(3, 2) + tbFechaVAX.Text.Substring(0, 2))
        Dim sContent As String = tr.ReadLine()
        If (sContent = "") Then
            errorVacio = True
        Else
            While (sContent <> "")
                Dim oArchivosVAXBMIDASRow As ArchivosVAXBMIDASBE.ArchivosVAXBMIDASRow
                oArchivosVAXBMIDASRow = oArchivosVAXBMIDASBE.ArchivosVAXBMIDAS.NewArchivosVAXBMIDASRow()
                ArrayBMIDAS = Split(sContent, separador, -1)
                oArchivosVAXBMIDASRow.FechaCarga = fechaCargaBMIDAS
                If ArrayBMIDAS.Length <= 0 Then
                    oArchivosVAXBMIDASRow.CodigoPortafolio = -1
                Else
                    oArchivosVAXBMIDASRow.CodigoPortafolio = ArrayBMIDAS(0)
                End If
                If ArrayBMIDAS.Length <= 1 Then
                    oArchivosVAXBMIDASRow.NombreBanco = -1
                Else
                    oArchivosVAXBMIDASRow.NombreBanco = ArrayBMIDAS(1)
                End If

                If ArrayBMIDAS.Length <= 2 Then
                    oArchivosVAXBMIDASRow.CodigoMoneda = -1
                Else
                    oArchivosVAXBMIDASRow.CodigoMoneda = ArrayBMIDAS(2)
                End If
                If ArrayBMIDAS.Length <= 3 Then
                    oArchivosVAXBMIDASRow.Saldo = -1
                Else
                    oArchivosVAXBMIDASRow.Saldo = Convert.ToDecimal(ArrayBMIDAS(3))
                End If

                oArchivosVAXBMIDASRow.SaldoSoles = -1
                oArchivosVAXBMIDASRow.SaldoDolares = -1
                oArchivosVAXBMIDASRow.SaldoEuros = -1
                oArchivosVAXBMIDASRow.SaldoYenes = -1
                oArchivosVAXBMIDASRow.SaldoLibras = -1
                oArchivosVAXBMIDASRow.SaldoDolCan = -1
                oArchivosVAXBMIDASRow.SaldoBRL = -1
                oArchivosVAXBMIDASRow.SaldoMxN = -1

                oArchivosVAXBMIDASBM.Insertar(oArchivosVAXBMIDASRow, MyBase.DatosRequest())

                sContent = tr.ReadLine()
            End While
        End If
        If (errorNoExiste) Then
            msgError.Visible = True
            msgError.Text = "Error... al importar los siguientes codigos de Concepto"
            dgNoExiste.Visible = True
            dgNoExiste.DataSource = dtNoExiste
            dgNoExiste.DataBind()
        ElseIf errorVacio Then
            msgError.Visible = True
            msgError.Text = "No hay datos en el Archivo Plano..."
        Else
            AlertaJS("El Archivo ha sido importado correctamente")
        End If

        tr.Close()

    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        If Myfile.Text = "" Then
            AlertaJS("Especifique la ruta del archivo.")
        Else
            If (ddlInterface.SelectedValue() = "BCOS") Then
                CargarBCOS()
            Else
                If (ddlInterface.SelectedValue() = "BMIDAS") Then
                    CargarBMIDAS()
                End If
            End If
        End If
    End Sub

    Private Sub BuscarRuta()
        'portafolio = ddlPortafolio.SelectedValue()
        portafolio = ddlPortafolio.SelectedItem.Text.Trim
        fechaVAX = tbFechaVAX.Text
        tipoInterface = ddlInterface.SelectedValue()
        Myfile.Text = ""
        If ((fechaVAX = "") Or (fechaVAX = "--Seleccione--") Or (portafolio = "") Or (portafolio = "--Seleccione--") Or (tipoInterface = "") Or (tipoInterface = "--Seleccione--")) Then
            AlertaJS("Especifique la fecha y/o el tipo de la Interface")
        Else
            fechaVAX = fechaVAX.Substring(6, 4) + fechaVAX.Substring(3, 2) + fechaVAX.Substring(0, 2)
            CargarRuta(tipoInterface, portafolio, fechaVAX)
        End If
    End Sub

    Private Sub btn_Salir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Salir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim decFechaInicio As Decimal

        LlenarSesionContextInfo()
        If (Me.tbFechaVAX.Text.Length > 0) Then
            decFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVAX.Text)
        Else
            AlertaJS("Debe ingresar una fecha.")
            Exit Sub
        End If
        If (Me.ddlInterface.SelectedIndex = 0) Then
            AlertaJS("Debe seleccionar una Interface.")
            Exit Sub
        End If

        If (Me.ddlInterface.SelectedValue = "BMIDAS") Then
            EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorGestion.aspx?pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pMercado=" + "" + "&pFechaIni=" + Convert.ToString(decFechaInicio) + "&pFechaFin=" + "" + "&pReporte=" + "BMIDAS", "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
        Else
            EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorGestion.aspx?pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pMercado=" + "" + "&pFechaIni=" + Convert.ToString(decFechaInicio) + "&pFechaFin=" + "" + "&pReporte=" + "BCOS", "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
        End If


    End Sub

    Sub LlenarSesionContextInfo()
        Dim tablaParametros As New Hashtable

        If (ddlPortafolio.SelectedIndex >= 0) Then
            tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
            tablaParametros("Portafolio") = ddlPortafolio.SelectedItem.Text
        End If

        ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
        Session("context_info") = tablaParametros
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Me.Myfile.Text = ""
        Me.ddlInterface.SelectedIndex = 0
        EstablecerFecha()
    End Sub
    Private Sub EstablecerFecha()
        If (ddlPortafolio.SelectedIndex = -1) Then
            Return
        End If

        tbFechaVAX.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If (Page.IsPostBack = False) Then
                CargarCombos()
                EstablecerFecha()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub ddlInterface_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlInterface.SelectedIndexChanged
        If (ddlInterface.SelectedIndex = -1) Then
            Return
        End If
        BuscarRuta()
    End Sub


End Class
