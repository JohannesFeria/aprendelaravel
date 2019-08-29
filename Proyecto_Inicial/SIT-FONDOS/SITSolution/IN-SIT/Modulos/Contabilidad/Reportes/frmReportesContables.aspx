<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportesContables.aspx.vb" Inherits="Modulos_Contabilidad_frmReportesContables" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reportes Contables</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Reportes Contables</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fondo</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div id="divMercado" runat="server" class="hidden">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Mercado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha" ControlToValidate="tbFechaInicio"
                                runat="server" />
                        </div>
                    </div>
                    <div id="divFechaFin" runat="server" class="hidden">
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Fecha de Fin</label>
                            <div class="col-sm-7">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                                <asp:RequiredFieldValidator ErrorMessage="Fecha de Fin" ControlToValidate="tbFechaFin"
                                    runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Selecci&oacute;n de Reporte</legend>
            <div class="row">
                <asp:RadioButtonList ID="RbReportes" runat="server" Width="100%" AutoPostBack="True">
                    <asp:ListItem Value="CVI">Compra Venta de Inversiones</asp:ListItem>
                    <asp:ListItem Value="VC">Valorizacion de la Cartera</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <asp:Label Text="" runat="server" ID="lblError" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnVista" />
                <asp:Button Text="Salir" runat="server" ID="btnTerminar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <br />
    <asp:Literal Text="" runat="server" ID="ltrLog" />
    <asp:ValidationSummary ID="vsResumenError" runat="server" ShowMessageBox="True" ShowSummary="False"
        HeaderText="Los siguientes campos son obligatorios:"></asp:ValidationSummary>
    </form>
</body>
</html>