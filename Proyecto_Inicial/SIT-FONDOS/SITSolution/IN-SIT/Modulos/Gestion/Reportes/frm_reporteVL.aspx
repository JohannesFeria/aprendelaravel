<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frm_reporteVL.aspx.vb" Inherits="Modulos_Gestion_Reportes_frm_reporteVL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Reporte VL</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos del Reporte</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaIni" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddltipo" runat="server">
                                <asp:ListItem Value="0">Excel</asp:ListItem>
                                <asp:ListItem Value="1">TXT</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <br />
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Imprimir Privados" runat="server" ID="btnImpPrivados" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>