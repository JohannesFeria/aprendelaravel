<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteDiarioOperaciones.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmReporteDiarioOperaciones" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reportes Diarios de Operaciones</title>
</head>
<body>
    <form id="form1" runat="server">
<div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header><h2>Reportes Diarios de Operaciones</h2></header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="220px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Operacion</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Reportes</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlreporte" runat="server" AutoPostBack="True" Width="220px" >
                            <asp:ListItem Value ='1' Text = 'Depositos'></asp:ListItem>
                            <asp:ListItem Value ='2' Text = 'Forwad'></asp:ListItem>
                            <asp:ListItem Value ='3' Text = 'Operaciones de Reporte'></asp:ListItem>
                            <asp:ListItem Value ='4' Text = 'Tenencias'></asp:ListItem>
                            <asp:ListItem Value ='5' Text = 'Dividendos'></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate> 
            </asp:UpdatePanel> 
        </fieldset>
        <br /><hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte" />
            <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>
