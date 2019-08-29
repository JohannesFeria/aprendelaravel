<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAprobacionAltaInstrumento.aspx.vb"
    Inherits="Modulos_Riesgos_frmAprobacionAltaInstrumento" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>REPORTE AUTORIZACIÓN RIESGOS</title>
    <script type="text/javascript">
        function ShowPopup(pCodSIN, pPeriodo, pBase, pTasa, pTasaS, pIndica, pFlag, pFechaP, pFechaV, pMnemo, pFechaE, pNumDias) {
            return showWindow('../Parametria/AdministracionValores/frmGeneracionCuponeraNormal.aspx?vISIN=' + pCodSIN + '&vPeriod=' + pPeriodo + '&vBaseC=' + pBase + '&vTasaC=' + pTasa + '&vTasaSpread=' + pTasaS + '&vIndicador=' + pIndica + '&vFlag=' + pFlag + '&vFechaP=' + pFechaP + '&vFechaV=' + pFechaV + '&vMnemo=' + pMnemo + '&vFechaE=' + pFechaE + '&vReadOnly=YES&vEstado=SI&vApInst=SI&vNumeroDias=' + pNumDias, '1200', '600'); 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <h2>
                REPORTE AUTORIZACIÓN RIESGOS</h2>
        </header>
        <fieldset>
            <legend>General</legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Renta</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblTipoRenta" runat="server" Width="466px" align="right" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código Instrumento</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblCodInstrumento" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nombre Instrumento</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblNomInstrumento" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de alta del producto</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblFechaAlta" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblCodigoISIN" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblCodigoSBS" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código Sinónimo</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblCodigoSin" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descrición Sinónimo</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblDescSin" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Plaza del instrumento</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblPlaza" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Número de Unidades</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblNumUnidad" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor Unitario</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblValUnitario" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor Efectivo Colocado</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblValEfectivo" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trMargenInicial" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Margen Inicial</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblMargenInicial" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trMargenMnto" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Margen Mantenimiento</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblMargenMnto" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trContractSize" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Contract Size</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblContractSize" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trTC" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo de Cupón</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblTipoCupon" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trFecEmi" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Emisión</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblFechaEmision" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div >
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Vencimiento</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtfecven" runat="server" Width="466px" ReadOnly="True" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trFecPCup" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Fecha Primer Cupón</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblFechaPriCupon" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trTA" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo de Amortización</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblTipoAmorti" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trPer" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Periodicidad</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblPeriodicidad" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trBC" runat="server">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Base cupon</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lblBaseCupon" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4" style="text-align: right;">                        
                        <asp:Button ID="btnCuponera" runat="server" Text="Cuponera" CausesValidation="False" Visible="false"  />                     
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Requisitos</legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha de Aprobación CNIAF/Marco de crédito</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaAprobar" SkinID="Date"></asp:TextBox>
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha de Aprobación CNIAF/Marco de crédito"
                                ControlToValidate="txtFechaAprobar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Observaciones</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtObs" runat="server" Width="466px" MaxLength="150" Height="42px"
                                TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            <strong>Información Límites:</strong></label>
                        <div class="col-sm-9">
                        </div>
                    </div>
                </div>
            </div>
            <div id="trCantMontCir" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Cantidad y monto en circulación</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblCantMonto" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trNomEmi" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Nombre Emisor</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblEmisor" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trContrapar" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Nombre Contraparte</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblContraparte" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trNotional" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Nocional</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblNocional" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trMonNotional" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Moneda Notional</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblMonedaNocional" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trInfFin" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                <strong>Información financiera:</strong></label>
                            <div class="col-sm-9">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trActivo" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                            </label>
                            <div class="col-sm-9">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">
                                        Activo</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="lblActivo" runat="server" Width="125px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trPasivo" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                            </label>
                            <div class="col-sm-9">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">
                                        Pasivo</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="lblPasivo" runat="server" Width="125px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trPatri" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                            </label>
                            <div class="col-sm-9">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">
                                        Patrimonio</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="lblPatrimonio" runat="server" Width="125px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trGrpEco" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Grupo Económico</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblGrupoEconomico" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trNivLiq" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Nivel de Liquidez</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblNivelLiquidez" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trTipFac" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Tipo Factor</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblTipoFactor" runat="server" Width="466px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trValFac" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Valor Factor</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="lblFactor" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="text-align: right;">
                <asp:HiddenField ID="hdnCod" runat="server" />
                <asp:HiddenField ID="hdnOp" runat="server" />
                <asp:HiddenField ID="hdnTR" runat="server" />
                <asp:HiddenField ID="hdnDiasPer" runat="server" />
                <asp:HiddenField ID="hdnTCupon" runat="server" />
                <asp:HiddenField ID="hdnTSpread" runat="server" />
                <asp:HiddenField ID="hdnValInd" runat="server" />
                <asp:HiddenField ID="hdnCTAmort" runat="server" />
                <asp:HiddenField ID="hdnFecVenc" runat="server" />
                <asp:HiddenField ID="hdnBCuponDias" runat="server" />
                <asp:HiddenField ID="hdnCTipCupon" runat="server" />
                <asp:HiddenField ID="hdnCClaInst" runat="server" />
                <asp:HiddenField ID="hdnCTipInstSBS" runat="server" />
                <asp:HiddenField ID="hdnCTer" runat="server" />
                <asp:Button ID="btnAprobarRiesgo" runat="server" Text="Aprobación de Riesgos" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="False" />
            </div>
        </fieldset>
        <br />
    </div>
    </form>
</body>
</html>
