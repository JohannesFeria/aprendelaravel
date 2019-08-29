<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGenerarReporteConstForward.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmGenerarReporteConstForward" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>GenerarReporteConstForward</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <header><h2>Constitución de Forwards</h2></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="220px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMercado" runat="server" AutoPostBack="True" Width="150px">
                                <asp:ListItem>-- Todos --</asp:ListItem>
                                <asp:ListItem Value="1">Local</asp:ListItem>
                                <asp:ListItem Value="2">Extranjero</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. de Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlNroCuenta" runat="server" Width="150px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:dropdownlist id="ddlMoneda" runat="server" AutoPostBack="True" Width="220px"></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cuentas por</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="150px">
                                <asp:ListItem>-- Todos --</asp:ListItem>
                                <asp:ListItem Value="N">Cobrar</asp:ListItem>
                                <asp:ListItem Value="S">Pagar</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on" id="imgFechaFin"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>    
    </ContentTemplate>
    </asp:UpdatePanel>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnVista" runat="server" Text="Imprimir" />
            <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
    
    </div>

    </form>
</body>
</html>
