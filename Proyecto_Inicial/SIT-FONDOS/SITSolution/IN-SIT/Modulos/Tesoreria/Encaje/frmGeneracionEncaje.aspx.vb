Imports System.Web
Imports System.Security
Imports System.Reflection
Imports System.Diagnostics
Imports System.Configuration
Imports System.Text
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmGeneracionEncaje
    Inherits BasePage
    Public GobiernoCentral As String
    Public BancoCentral As String
    Public DiasAPromediar As String
    Public PorcentajeLimite As String
    Public DiasAlVencimiento As String
    Public ValorFondo As String
    Public IndicadorFondo As Decimal
    Public EncajeMantenido As String
    Public IndicadorEncaje As Decimal
    Public TotalActivos As String
    Public IndicadorTotalActivos As Decimal
    Public ValorNumeroCuotasT As Decimal
    Public IndicadorNumeroCuotasT As String
    Public ValorCuotaT As Decimal
    Public IndicadorValorCuota As String
    Public ValorNumeroCuotasT1 As Decimal
    Public IndicadorNumeroCuotas As String
    Public ValorCuotaT1 As Decimal
    Public ValorComprasT As Decimal
    Public IndicadorCompras As String
    Public ValorVentasT As Decimal
    Public IndicadorVentas As String
    Public fechaanterior As Decimal


#Region "Variables"

    Dim oArchivosVaxBCOSBM As New ArchivosVAXBCOSBM
    Dim oArchivosVaxBCOSBE As New DataSet
    Dim oParametrosMigracionBM As New ParametrosMigracionBM
    Dim oParametrosMigracionBE As New DataSet
    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
    Dim strMensajeObli As String = ""
    Dim oencajeBM As New EncajeBM

#End Region

#Region "/*Metodos Personalizados*/"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            Dim dtPortafolio As DataTable
            Dim oPortafolioBM As New PortafolioBM
            dtPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(Me.ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
        End If
    End Sub

    Private Function RecuperarDatosRequerido() As Boolean
        Dim okParametros As Boolean = True
        Dim okDatos As Boolean = True
        Dim DecFechaProceso As Decimal
        Dim portafolio As String
        Dim strMensaje As String = ""
        Dim objutilbm As New UtilDM
        strMensaje = "<script language='JavaScript'> alert('Los siguienes campos son obligatorios: \n"
        If (Me.txtFechaProceso.Text = "") Then
            DecFechaProceso = 0
            strMensaje = strMensaje + "-Fecha Proceso\n"
            okParametros = False
        Else
            'RGF 20090218 Ocurrio un problema por el feriado del 16/02/09
            DecFechaProceso = objutilbm.RetornarFechaValoradaAnterior(UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), ddlPortafolio.SelectedValue)
            'DecFechaProceso = objutilbm.RetornarFechaUtilAnterior(UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text))
        End If
        If (Me.ddlPortafolio.SelectedItem.Text = "--Seleccione--") Then
            portafolio = ""
            strMensaje = strMensaje + "-Portafolio"
            okParametros = False
        Else
            portafolio = ddlPortafolio.SelectedValue
        End If
        okDatos = okParametros
        If (okParametros = True) Then
            strMensajeObli = "<script language='JavaScript'> alert('No estan disponibles los siguientes campos: \n"
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 0, "", "", "", DatosRequest)
            'Gobierno Central
            GobiernoCentral = oParametrosGeneralesBM.ListarEncaje(DatosRequest).Rows(1).Item("Valor").ToString()
            'Banco Central
            BancoCentral = oParametrosGeneralesBM.ListarEncaje(DatosRequest).Rows(0).Item("Valor").ToString()
            'DiasAPromediar
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 5, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                DiasAPromediar = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                DiasAPromediar = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Dias a Promediar \n"
            End If
            'PorcentajeLimite
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 4, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                PorcentajeLimite = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                PorcentajeLimite = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Porcentaje Limite sobre Emision \n"
            End If
            'DiasAlVencimiento
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 1, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                DiasAlVencimiento = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                DiasAlVencimiento = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Dias al Vencimiento minimo paa encaje \n"
            End If
            'ValorFondo
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 6, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                ValorFondo = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                ValorFondo = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Fondo \n"
            End If
            'IndicadorFondo
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(ValorFondo, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                IndicadorFondo = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(ValorFondo, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                IndicadorFondo = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Fondo \n"
            End If
            'EncajeMantenido
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 7, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                EncajeMantenido = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                EncajeMantenido = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-EncajeMantenido \n"
            End If
            'IndicadorEncaje
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(EncajeMantenido, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                IndicadorEncaje = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(EncajeMantenido, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                IndicadorEncaje = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Encaje Mantenido  \n"
            End If
            'TotalActivos
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 8, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                TotalActivos = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                TotalActivos = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-TotalActivos \n"
            End If
            'IndicadorTotalActivos
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(TotalActivos, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                IndicadorTotalActivos = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(TotalActivos, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                IndicadorTotalActivos = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador de Total Activos \n"
            End If
            If (okDatos = False) Then
                strMensajeObli = strMensajeObli + "')</script>"
                EjecutarJS(strMensajeObli, False)
            End If
        Else
            strMensaje = strMensaje + "');</script>"
            EjecutarJS(strMensaje, False)
        End If
        Return okDatos
    End Function

    Private Function RecuperarDatosRentabilidad() As Boolean
        Dim okParametros As Boolean = True
        Dim okDatos As Boolean = True
        Dim DecFechaProceso As Decimal
        Dim portafolio As String
        Dim strMensaje As String = ""
        Dim objutilbm As New UtilDM
        strMensaje = "<script language='JavaScript'> alert('Los siguienes campos son obligatorios: \n"
        If (Me.txtFechaProceso.Text = "") Then
            DecFechaProceso = 0
            strMensaje = strMensaje + "-Fecha Proceso\n"
            okParametros = False
        Else
            ' DecFechaProceso = UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text)
            'RGF 20090218 Ocurrio un problema por el feriado del 16/02/09
            DecFechaProceso = objutilbm.RetornarFechaValoradaAnterior(UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), ddlPortafolio.SelectedValue)
            'DecFechaProceso = objutilbm.RetornarFechaUtilAnterior(UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text))
        End If
        If (Me.ddlPortafolio.SelectedItem.Text = "--Seleccione--") Then
            portafolio = ""
            strMensaje = strMensaje + "-Portafolio"
            okParametros = False
        Else
            portafolio = ddlPortafolio.SelectedValue
        End If
        okDatos = okParametros
        If (okParametros = True) Then
            strMensajeObli = "<script language='JavaScript'> alert('No estan disponibles los siguientes campos: \n"
            '----------- 
            'Indicador NumeroCuotas a T (ENCMCU)
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 3, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                IndicadorNumeroCuotas = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                IndicadorNumeroCuotas = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador de Numero de Cuota \n"
            End If
            'Valor Numero Cuotas a T
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorNumeroCuotas, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                ValorNumeroCuotasT = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorNumeroCuotas, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                ValorNumeroCuotasT = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Numero de cuota \n"
            End If
            '----------
            '----------- 
            'Indicador Valorcuota a T (VALCUO)
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 2, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                IndicadorValorCuota = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                IndicadorValorCuota = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador Valor Cuota \n"
            End If
            'Valor Valorcuota  a T
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorValorCuota, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                ValorCuotaT = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorValorCuota, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                ValorCuotaT = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Valor Cuota \n"
            End If
            '----------
            '----------- 
            'Indicador Compras T (ENCSOC)
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 10, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                IndicadorCompras = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                IndicadorCompras = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador Soles Compras \n"
            End If
            'Valor compras  a T
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorCompras, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                ValorComprasT = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorCompras, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                ValorComprasT = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Valor en soles de compras \n"
            End If
            '----------
            '----------- 
            'Indicador Ventas T (ENCSOV)
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 9, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                IndicadorVentas = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                IndicadorVentas = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador Soles Ventas \n"
            End If
            'Valor compras  a T
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorVentas, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                ValorVentasT = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorVentas, portafolio, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                ValorVentasT = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Valor en soles de Ventas \n"
            End If
            '----------
            '----------- 
            'Indicador NumeroCuotas a T-1 (ENCMCU)
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 3, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                IndicadorNumeroCuotas = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                IndicadorNumeroCuotas = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador de Numero de Cuota \n"
            End If
            'Valor Numero Cuotas a T-1
            fechaanterior = oencajeBM.ObtenerFechaT1Indicadores(IndicadorNumeroCuotas, portafolio, DecFechaProceso, DatosRequest)
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorNumeroCuotas, portafolio, fechaanterior, DatosRequest).Tables(0).Rows.Count <> 0) Then
                ValorNumeroCuotasT1 = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorNumeroCuotas, portafolio, fechaanterior, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                ValorNumeroCuotasT1 = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Numero de cuota \n"
            End If
            '----------
            '----------- 
            'Indicador Valorcuota a T -1(VALCUO)
            oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 2, "", "", "", DatosRequest)
            If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                IndicadorValorCuota = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
            Else
                IndicadorValorCuota = ""
                okDatos = False
                strMensajeObli = strMensajeObli + "-Indicador Valor Cuota \n"
            End If
            'Valor Valorcuota  a T-1
            fechaanterior = oencajeBM.ObtenerFechaT1Indicadores(IndicadorValorCuota, portafolio, DecFechaProceso, DatosRequest)
            If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorValorCuota, portafolio, fechaanterior, DatosRequest).Tables(0).Rows.Count <> 0) Then
                ValorCuotaT1 = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(IndicadorValorCuota, portafolio, fechaanterior, DatosRequest).Tables(0).Rows(0).Item("Valor"))
            Else
                ValorCuotaT1 = -1
                okDatos = False
                strMensajeObli = strMensajeObli + "-Valor de Indicador de Valor Cuota \n"
            End If
            '----------
            If (okDatos = False) Then
                strMensajeObli = strMensajeObli + "')</script>"
                EjecutarJS(strMensajeObli)
            End If
        Else
            strMensaje = strMensaje + "');</script>"
            EjecutarJS(strMensaje)
        End If
        Return okDatos
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim Mensaje As String = ""
        If txtFechaProceso.Text = "" Then
            Mensaje += " - Fecha de Proceso"
        End If
        If ddlPortafolio.SelectedValue = "" Then
            Mensaje += " - Portafolio"
        End If
        If Not Mensaje = "" Then
            AlertaJS("Los  siguientes campos son obligatorios: <br/>" + Mensaje)
            Exit Sub
        End If
        Dim objencajeBM As New EncajeBM
        Dim objValorizacionBM As New CarteraTituloValoracionBM
        Dim resultado As Integer

        If (objValorizacionBM.Validar(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text)) = False) Then
            AlertaJS("No existe valoracion para la fecha seleccionada")
            Exit Sub
        End If
        If Me.rblOpcion.SelectedValue = "0" Then
            If objencajeBM.ExisteEncajePeru(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), DatosRequest) = 0 Then
                If (RecuperarDatosRequerido() = True) Then
                    Try
                        resultado = objencajeBM.CalculoencajePeru2(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), GobiernoCentral, BancoCentral, Convert.ToInt32(DiasAPromediar), Convert.ToInt32(PorcentajeLimite), Convert.ToInt32(DiasAlVencimiento), ValorFondo, IndicadorFondo, EncajeMantenido, IndicadorEncaje, TotalActivos, IndicadorTotalActivos, DatosRequest)
                        If resultado = 1 Then
                            AlertaJS(ObtenerMensaje("ALERT15"))
                        Else
                            AlertaJS("Faltan Datos en el Proceso de Encaje")
                            'Page.RegisterStartupScript("JScript", UIUtility.MostrarPopUp("frmVisorErrorEncaje.aspx?pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pFecha=" + txtFechaProceso.Text, "no", 800, 789, 10, 10, "no", "yes", "yes", "yes"))                            
                            EjecutarJS("ShowModal();")
                        End If
                    Catch ex As Exception
                        AlertaJS(ex.Message.ToString)
                    End Try
                End If
            Else
                AlertaJS(ObtenerMensaje("ALERT144"))
            End If
        Else
            If objencajeBM.ExisteEncajePeru(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), DatosRequest) = 1 Then
                If (RecuperarDatosRentabilidad() = True) Then
                    objencajeBM.ActualizarRentabilidad(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), ValorNumeroCuotasT, ValorCuotaT, ValorNumeroCuotasT1, ValorCuotaT1, ValorComprasT, ValorVentasT, DatosRequest)
                    AlertaJS(ObtenerMensaje("ALERT163"))
                End If
            Else
                AlertaJS(ObtenerMensaje("ALERT162"))
            End If
        End If
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Dim objencajeBM As New EncajeBM
            Dim FechaUltimoEncaje As Decimal
            FechaUltimoEncaje = objencajeBM.ObtenerFechaUltimoEncaje(Me.ddlPortafolio.SelectedValue, 0, DatosRequest)
            If FechaUltimoEncaje <> 0 Then
                If Me.rblOpcion.SelectedValue = "0" Then
                    Me.txtFechaProceso.Text = UIUtility.ConvertirFechaaString(objencajeBM.ProximaFechaEncaje(FechaUltimoEncaje, DatosRequest))
                Else
                    Me.txtFechaProceso.Text = UIUtility.ConvertirFechaaString(FechaUltimoEncaje)
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub rblOpcion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblOpcion.SelectedIndexChanged
        Try
            If Me.ddlPortafolio.SelectedValue <> "--Seleccione--" Then
                Dim objencajeBM As New EncajeBM
                Dim FechaUltimoEncaje As Decimal
                FechaUltimoEncaje = objencajeBM.ObtenerFechaUltimoEncaje(Me.ddlPortafolio.SelectedValue, 0, DatosRequest)
                If FechaUltimoEncaje <> 0 Then
                    If Me.rblOpcion.SelectedValue = "0" Then
                        Me.txtFechaProceso.Text = UIUtility.ConvertirFechaaString(objencajeBM.ProximaFechaEncaje(FechaUltimoEncaje, DatosRequest))
                    Else
                        Me.txtFechaProceso.Text = UIUtility.ConvertirFechaaString(FechaUltimoEncaje)
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

End Class
