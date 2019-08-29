<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteConsolidadoPosicionV2.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmReporteConsolidadoPosicionV2" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Consolidado de Posición de Fondos</title>
    <script type="text/javascript">
        function ShowPopup(pFecha, pPorta, pEsce) {
            return showModalDialog('frmVisorReporteConsolidadoPosiciones.aspx?pFecha=' + pFecha + '&pPortafolio=' + pPorta + '&pEscenario=' + pEsce, '800', '600', '');   
        }
        function ShowPopupPDF() {
            return showModalDialog('frmVisorReporteLimitePDF.aspx', '800', '600', '');   
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Consolidado de Posición de Fondos</h2></header>
    <fieldset>
    <legend>Filtros</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Operación</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Fecha de Operación" ControlToValidate="tbFechaInicio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Reportes</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoReporte" runat="server" Width="215px" Visible="False">
						<asp:ListItem Value="EXCESO">Consolidado Exceso L&#237;mite</asp:ListItem>
						<asp:ListItem Value="CONSOLIDADO">Resumen Consolidado</asp:ListItem>
					</asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="170px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9">
                    <asp:radiobuttonlist id="rblEscenario" runat="server" CssClass="stlCajaTexto" RepeatDirection="Horizontal" AutoPostBack="True">
						<asp:ListItem Value="REAL" Selected="True">Reales</asp:ListItem>
						<asp:ListItem Value="ESTIMADO">Estimado</asp:ListItem>
					</asp:radiobuttonlist>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" >
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label id="lblTime" runat="server">Tiempo</asp:Label>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">            
            <asp:Button ID="btnImprimirResumen" runat="server" Text="Imprimir Resumen" Visible="False" />
            <asp:Button ID="btnImprimirPDF" runat="server" Text="Imprimir en PDF" />
            <asp:Button ID="btnGenerarExcel" runat="server" Text="Generar Excel" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" 
                CausesValidation="False" />
        </div>
    </div>
    </div>
    </form>
</body>
</html>
