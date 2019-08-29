<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmFeriado.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmFeriado" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento Feriado</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Mantenimiento Feriado</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaControl" SkinID="Date" />
                                    <span runat="server" id="spCalendar" class="add-on"><i class="awe-calendar"></i>
                                    </span>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvFechaControl" ErrorMessage="Fecha" ControlToValidate="tbFechaControl"
                                    runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situación</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="152px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSituacion" ErrorMessage="Situación" ControlToValidate="ddlSituacion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Mercado</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlMercado" runat="server" Width="152px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvMercado" ErrorMessage="Mercado" ControlToValidate="ddlMercado"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                </div>
                <div class="col-sm-6">
                    <div class="form-group" style="float: right;">
                        <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" ValidationGroup="vgDetalle" />
                        <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </fieldset>
        <asp:HiddenField ID="hd" runat="server" />
        <asp:HiddenField ID="hdMerca" runat="server" />
        <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
            HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgDetalle" />
    </div>
    </form>
</body>
</html>
