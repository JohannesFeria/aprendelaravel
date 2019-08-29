Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Partial Class Modulos_Riesgos_frmAprobacionAltaInstrumento
    Inherits BasePage
#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            If Not Page.IsPostBack Then
                Me.hdnCod.Value = Request.QueryString("Cod")
                Me.hdnOp.Value = Request.QueryString("Op")
                Me.trMargenInicial.Visible = False
                Me.trMargenMnto.Visible = False
                Me.trContractSize.Visible = False
                If Not (Request.QueryString("Cod") = Nothing) Then
                    Me.lblCodInstrumento.Text = Me.hdnCod.Value
                    CargarRegistro(Me.hdnCod.Value)
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub btnAprobarRiesgo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobarRiesgo.Click
        Try
            Dim oValoresBM As New ValoresBM
            If (hdnOp.Value = "A") Then
                oValoresBM.Aprobacion_InstrumentosRiesgo(Me.lblCodInstrumento.Text.ToString, UIUtility.ConvertirFechaaDecimal(Me.txtFechaAprobar.Text), Me.txtObs.Text, "A", DatosRequest)
                ReturnArgumentShowDialogPopup()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aprobar")
        End Try
    End Sub

    Sub LlenarSesionContextInfo()
        Dim tablaParametros As New Hashtable

        tablaParametros("Eliminar") = "SI"

        ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
        Session("context_eliminar") = tablaParametros
    End Sub


    Private Sub btnCuponera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCuponera.Click
        Me.LlenarSesionContextInfo()

        EjecutarJS("ShowPopup('" + Me.lblCodigoISIN.Text.Trim.ToUpper + "','" + Me.hdnDiasPer.Value.ToString + "','" + Me.lblBaseCupon.Text + "','" + Me.hdnTCupon.Value + "','" + Me.hdnTSpread.Value + "','" + Me.hdnValInd.Value + "','" + Me.hdnCTAmort.Value + "','" + Me.lblFechaPriCupon.Text + "','" + Me.hdnFecVenc.Value + "','" + Me.lblCodInstrumento.Text.Trim.ToUpper + "','" + Me.lblFechaEmision.Text + "','" + Me.hdnBCuponDias.Value.Trim + "');")
    End Sub

#End Region
#Region " /* Funciones Personalizadas*/"

    Public Sub CargarRegistro(ByVal CodigoMnemonico As String)
        Dim oValoresBM As New ValoresBM
        Dim drValor As DataRow

        Try
            drValor = oValoresBM.Reporte_AutorizacionRiesgo(CodigoMnemonico).Rows(0)

            hdnTR.Value = drValor("CodTR").ToString
            lblTipoRenta.Text = drValor("TipoRenta").ToString
            lblNomInstrumento.Text = drValor("Descripcion").ToString
            lblFechaAlta.Text = UIUtility.ConvertirFechaaString(drValor("FechaAlta").ToString)
            txtFechaAprobar.Text = Me.lblFechaAlta.Text
            lblCodigoISIN.Text = drValor("CodigoISIN").ToString
            lblCodigoSBS.Text = drValor("CodigoSBS").ToString
            lblCodigoSin.Text = drValor("Sinonimo").ToString
            lblDescSin.Text = drValor("DescSinonimo").ToString
            lblPlaza.Text = drValor("Plaza").ToString
            lblNumUnidad.Text = Format(CType(drValor("NumeroUnidades").ToString, Decimal), "##,##0.0000000")
            lblValUnitario.Text = Format(CType(drValor("ValorUnitario").ToString, Decimal), "##,##0.0000000")
            lblValEfectivo.Text = Format(CType(drValor("ValorEfectivoColocado").ToString, Decimal), "##,##0.0000000")
            lblTipoCupon.Text = drValor("TipoCupon").ToString
            lblFechaEmision.Text = UIUtility.ConvertirFechaaString(drValor("FechaEmision").ToString)
            lblFechaPriCupon.Text = UIUtility.ConvertirFechaaString(drValor("FechaPrimerCupon").ToString)
            lblTipoAmorti.Text = drValor("TipoAmortizacion").ToString
            lblPeriodicidad.Text = drValor("Periodicidad").ToString
            lblBaseCupon.Text = drValor("BaseCupon").ToString
            txtObs.Text = drValor("Obs").ToString
            lblEmisor.Text = drValor("NomEmisor").ToString
            lblNocional.Text = drValor("Nocional").ToString
            lblMonedaNocional.Text = drValor("MonedaN").ToString
            lblPatrimonio.Text = Format(CType(drValor("Patrimonio").ToString, Decimal), "##,##0.0000000")
            lblActivo.Text = Format(CType(drValor("Activo").ToString, Decimal), "##,##0.0000000")
            lblPasivo.Text = Format(CType(drValor("Pasivo").ToString, Decimal), "##,##0.0000000")
            lblGrupoEconomico.Text = drValor("GrupoEconomico").ToString
            lblNivelLiquidez.Text = drValor("CriterioLiquidez").ToString
            lblTipoFactor.Text = drValor("TipoFactor").ToString
            lblFactor.Text = drValor("Factor").ToString.Replace(UIUtility.DecimalSeparator, ".")
            lblCantMonto.Text = Format(CType(drValor("ValorEfectivoColocado").ToString, Decimal), "##,##0.0000000")
            txtfecven.Text = UIUtility.ConvertirFechaaString(drValor("FechaVencimiento").ToString)
            If Me.hdnTR.Value = "1" Then
                hdnDiasPer.Value = drValor("DiasPeriodo").ToString
                hdnTCupon.Value = drValor("TasaCupon").ToString
                hdnTSpread.Value = drValor("TasaSpread").ToString
                hdnValInd.Value = drValor("ValorIndicador").ToString
                hdnCTAmort.Value = drValor("CodigoTipoAmortizacion").ToString
                hdnFecVenc.Value = UIUtility.ConvertirFechaaString(drValor("FechaVencimiento").ToString)
                hdnBCuponDias.Value = drValor("BaseCuponDias").ToString
                hdnCTipCupon.Value = drValor("CodigoTipoCupon").ToString
                hdnCClaInst.Value = drValor("CodigoClaseInstrumento").ToString
                hdnCTipInstSBS.Value = drValor("CodigoTipoInstrumentoSBS").ToString
                hdnCTer.Value = drValor("CodigoTercero").ToString
                If (hdnCClaInst.Value = "3" Or _
                    hdnCClaInst.Value = "5" Or _
                    hdnCClaInst.Value = "7" Or _
                    (hdnCClaInst.Value = "10" And hdnCTipInstSBS.Value.Trim.Equals("19")) Or _
                    (Trim(hdnCTipInstSBS.Value) = "08" And Trim(hdnCTer.Value) = "4000" And hdnCTipCupon.Value = "3")) Then
                    btnCuponera.Visible = False
                Else
                    If (hdnCClaInst.Value = "1" Or _
                        hdnCClaInst.Value = "11" Or _
                        (hdnCClaInst.Value = "10" And hdnCTipInstSBS.Value.Trim.Equals("09")) Or _
                        hdnCTipInstSBS.Value.Trim.Equals("57")) Then
                        btnCuponera.Visible = False
                    End If
                End If
            End If
            If Me.hdnTR.Value = ParametrosSIT.TR_DERIVADOS Then
                If drValor("CodigoTipoInstrumentoSBS").ToString() = ParametrosSIT.CODIGOSBS_FUTUROS Then
                    Me.trMargenInicial.Visible = True
                    Me.trMargenMnto.Visible = True
                    Me.trContractSize.Visible = True
                    Me.lblMargenInicial.Text = String.Format("{0:###,##0.0000000}", drValor("MargenInicial"))
                    Me.lblMargenMnto.Text = String.Format("{0:###,##0.0000000}", drValor("MargenMantenimiento"))
                    Me.lblContractSize.Text = String.Format("{0:###,##0.0000000}", drValor("ContractSize"))
                End If
            End If
            HabilitaCampos()
            HabilitaFilas()
            If Me.hdnOp.Value = "A" Then
                HabilitaControles(True)
            Else
                HabilitaControles(False)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub HabilitaFilas()
        If Me.hdnTR.Value = "1" Then
            trTC.Style.Item("display") = ""
            trFecEmi.Style.Item("display") = ""
            trFecPCup.Style.Item("display") = ""
            trTA.Style.Item("display") = ""
            trPer.Style.Item("display") = ""
            trBC.Style.Item("display") = ""
        Else
            trTC.Style.Item("display") = "none"
            trFecEmi.Style.Item("display") = "none"
            trFecPCup.Style.Item("display") = "none"
            trTA.Style.Item("display") = "none"
            trPer.Style.Item("display") = "none"
            trBC.Style.Item("display") = "none"
        End If
    End Sub

    Private Sub HabilitaControles(ByVal estado As Boolean)
        txtFechaAprobar.ReadOnly = Not estado
        txtObs.ReadOnly = Not estado
        btnAprobarRiesgo.Visible = estado
    End Sub

    Private Sub HabilitaCampos()
        trCantMontCir.Style.Item("display") = ""
        trNomEmi.Style.Item("display") = ""
        trContrapar.Style.Item("display") = ""
        trNotional.Style.Item("display") = ""
        trMonNotional.Style.Item("display") = ""
        trInfFin.Style.Item("display") = ""
        trActivo.Style.Item("display") = ""
        trPasivo.Style.Item("display") = ""
        trPatri.Style.Item("display") = ""
        trGrpEco.Style.Item("display") = ""
        trNivLiq.Style.Item("display") = ""
        trTipFac.Style.Item("display") = ""
        trValFac.Style.Item("display") = ""

        Select Case Me.lblCodigoSin.Text
            Case "ACC", "ACT", "ACX", "VRA"
                trContrapar.Style.Item("display") = "none"
                trNotional.Style.Item("display") = "none"
                trMonNotional.Style.Item("display") = "none"
            Case "CDBCR", "DPZ", "CDLP"
                trCantMontCir.Style.Item("display") = "none"
                trContrapar.Style.Item("display") = "none"
                trNotional.Style.Item("display") = "none"
                trMonNotional.Style.Item("display") = "none"
                trInfFin.Style.Item("display") = "none"
                trActivo.Style.Item("display") = "none"
                trPasivo.Style.Item("display") = "none"
                trPatri.Style.Item("display") = "none"
                trNivLiq.Style.Item("display") = "none"
            Case "BAF", "BEP", "BHSF", "BNP", "BSF", "BTH", "BGC", "OSF", "OSX", "TCDC", "TDNF", "TDORINL", "TEX"
                trContrapar.Style.Item("display") = "none"
                trNivLiq.Style.Item("display") = "none"
            Case "FIV", "FMX"
                trCantMontCir.Style.Item("display") = "none"
                trContrapar.Style.Item("display") = "none"
                trNotional.Style.Item("display") = "none"
                trMonNotional.Style.Item("display") = "none"
                trActivo.Style.Item("display") = "none"
                trPasivo.Style.Item("display") = "none"
                trGrpEco.Style.Item("display") = "none"
                trNivLiq.Style.Item("display") = "none"
                trTipFac.Style.Item("display") = "none"
                trValFac.Style.Item("display") = "none"
            Case Else
                trCantMontCir.Style.Item("display") = "none"
                trNomEmi.Style.Item("display") = "none"
                trInfFin.Style.Item("display") = "none"
                trActivo.Style.Item("display") = "none"
                trPasivo.Style.Item("display") = "none"
                trPatri.Style.Item("display") = "none"
                trGrpEco.Style.Item("display") = "none"
                trNivLiq.Style.Item("display") = "none"
                trTipFac.Style.Item("display") = "none"
                trValFac.Style.Item("display") = "none"
        End Select
    End Sub

    Private Sub ReturnArgumentShowDialogPopup()
        AlertaJS("Se aprobó el alta del instrumento correctamente", "window.close();")
    End Sub

#End Region
End Class