<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRentabilidadEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmRentabilidadEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head id="Head1" runat="server">
    <title>Reporte de Rentabilidad de Encaje</title>
    <script type="text/javascript">
        function showPopupMnemonico() {
            return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoAprob', '800', '600', '');             
        }
        function ShowPopupImprimir(vFFin) {
            var vFondo = $('#ddlFondo').val();
            var vFInicio = $('#tbFechaInicio').val();
            var vMnem = $('#tbNemonico').val();
            //alert(vFondo); alert(vFInicio); alert(vFFin); alert(vMnem);
            return showModalDialog('frmVisorEncaje.aspx?pReporte=REN_ENCAJE&pportafolio=' + vFondo + '&pFechaIni=' + vFInicio + '&pFechaFin=' + vFFin + '&pNemonico=' + vMnem, '800', '600', '');                       
        }
        function DownloadFile(url) {
            $(document).ready(function () {
                //window.open(url);
                //window.showModalDialog(url);
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Reporte de Rentabilidad de Encaje</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlFondo" tabIndex="4" runat="server" AutoPostBack="True" Width="183px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Mnemónico</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbNemonico" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" CausesValidation="false" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
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
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">
                    <asp:CheckBox style="Z-INDEX: 0" id="chkIndAnio" runat="server" AutoPostBack="True" Text="Saldos al Año:"></asp:CheckBox>
                </label>
                <div class="col-sm-9">
                    <asp:dropdownlist style="Z-INDEX: 0" id="ddlAnio" tabIndex="4" runat="server" Width="70px" AutoPostBack="True" Enabled="False"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Ruta Excel</label>
                <div class="col-sm-9">
                    <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                        class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                        data-size="sm">
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Label style="Z-INDEX: 102; POSITION: absolute; TOP: 224px; LEFT: 16px" id="lblTime" runat="server"></asp:Label>
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
        <asp:Button ID="btnDetallePorInstrumento" runat="server" Text="Detalle por Instrumento" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
