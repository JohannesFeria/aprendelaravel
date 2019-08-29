<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDatosCarta.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmDatosCarta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <fieldset>
        <legend>Datos Carta</legend>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="form-group">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="col-md-4">
                                Tipo:</div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlTipoDatoCarta" runat="server" Style="width: 100%" AutoPostBack="true" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="col-md-4">
                                Valor:</div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlValorTipo" runat="server" Style="width: 100%" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="col-md-4">
                                Banco</div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlBancosDatoCarta" runat="server" Width="100%" AutoPostBack="true" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="col-md-4">
                                Cuenta:</div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlCuentasDatoCarta" runat="server" Width="100%" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTipoDatoCarta" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlBancosDatoCarta" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    <div class="col-md-12 ">
        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" />
    </div>
    </form>
</body>
</html>
