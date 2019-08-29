<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGrupoPorTipoMoneda.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmGrupoPorTipoMoneda" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Grupo por tipo de moneda</title>
</head>
<body>
    <form id="form1" class="form-horizontal" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Grupo Tipo de Moneda</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Grupo</label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtgrupomoneda" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n Grupo</label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtDescripcion" Width ="350px" MaxLength ="100" />
                        </div>
                    </div>
                </div>
                </div>
                 <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" >
                                <asp:ListItem Value="A">Activo</asp:ListItem>
                                <asp:ListItem Value="I">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Opciones de Agrupacion</label>
                        <div class="col-sm-4">
                            <asp:CheckBoxList ID="chklOpciones" runat="server">
                                <asp:ListItem Value="1">Aplica Moneda Portafolio</asp:ListItem>
                                <asp:ListItem Value="2">Aplica Forward</asp:ListItem>
                                <asp:ListItem Value="3">Aplica Cartera</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CssClass="disabled" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
