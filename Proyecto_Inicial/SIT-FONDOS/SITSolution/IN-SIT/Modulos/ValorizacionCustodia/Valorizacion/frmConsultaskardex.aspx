<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaskardex.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmConsultaskardex" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Kardex</title>
    <script type="text/javascript">
        function showPopup(pPorta, pInstru, pMnemo, pIsin, pSbs, pFInicio, pFFin, pTReport) {
            return showModalDialog('Reportes/frmVkardex.aspx?pportafolio=' + pPorta + '&pTipoInstrumento=' + pInstru + '&pmnemonico=' + pMnemo + '&pisin=' + pIsin + '&psbs=' + pSbs + '&pFechaIni=' + pFInicio + '&pFechaFin=' + pFFin + '&pReporte=' + pTReport, '800', '600', '');             
        }

        function showPupupNemonico() {
            var Isin = document.getElementById('txtISIN').value;
            var Nemonico = document.getElementById('txtMnemonico').value;
            var Sbs = document.getElementById('txtsbs').value;

            return showModalDialog('frmBusquedaInstrumentos.aspx?vIsin=' + Isin + '&vSbs=' + Sbs + '&vMnemonico=' + Nemonico, '800', '600', '');             
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Kardex</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de instrumento</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoInstrumento" runat="server" Width="300px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlFondo" runat="server" Width="150px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnemónico</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="txtMnemonico" CssClass="mayusculas" />
                        <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" CausesValidation="false" OnClientClick="return showPupupNemonico();" ><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Inicio</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Fecha Inicio" ControlToValidate="tbFechaInicio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Fin</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <!--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Fecha Fin" ControlToValidate="tbFechaFin">(*)</asp:RequiredFieldValidator>-->
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código ISIN</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtISIN" runat="server" CssClass="mayusculas" ></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código SBS</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtsbs" runat="server" CssClass="mayusculas" ></asp:textbox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Reporte</label>
                <div class="col-sm-9">
                    <asp:radiobuttonlist id="rbtReporte" runat="server" Width="400px" 
                        RepeatColumns="2" RepeatDirection="Horizontal" >
						<asp:ListItem Value="IC" Selected="True">Instrumentos en Cartera</asp:ListItem>
						<asp:ListItem Value="OR">Operaciones Realizadas</asp:ListItem>
					</asp:radiobuttonlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" 
            CausesValidation="False" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>