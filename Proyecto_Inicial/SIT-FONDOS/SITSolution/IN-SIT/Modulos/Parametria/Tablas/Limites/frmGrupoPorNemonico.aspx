<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGrupoPorNemonico.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmGrupoPorNemonico" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Grupo por Nemonico</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Grupo por Nemonico</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-8 control-label">
                                    C&oacute;digo Grupo</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtcodigogrupo" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-8 control-label">
                                    Descripci&oacute;n Grupo</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtDescripcion" Width ="350px" MaxLength ="100" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-8 control-label">
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
                </div>
            </div>
            <div class="row">
            <asp:UpdatePanel ID="upItems" runat="server" UpdateMode ="Conditional" >
            <ContentTemplate>
            <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-5" style="text-align: center;">
                             Valores Disponibles
                        </div>
                        <div class="col-md-2" style="text-align: center;">
                        </div>
                        <div class="col-md-5" style="text-align: center;">
                            Valores Agregados
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5" style="text-align: right;">
                            <asp:TextBox ID="txtDesVal" runat="server" MaxLength ="100" Width="280px" />
                            <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5" style="text-align: right;">
                            <asp:ListBox ID="lbxValores" TabIndex="12" runat="server" Width="100%" Height="120px"
                                Style="background-image: none; background-color: #fff;"></asp:ListBox>
                        </div>
                        <div class="col-md-2" style="text-align: center;">
                            <div class="row">
                                <asp:Button ID="btnAgregarTodosCaracteristica" Text=">>" runat="server" SkinID="btnSmall" /></div>
                            <div class="row">
                                <asp:Button ID="btnAgregarCaracteristica" Text=">" runat="server" 
                                    SkinID="btnSmall" style="height: 26px" /></div>
                            <div class="row">
                                <asp:Button ID="btnDevolverCaracteristica" Text="<" runat="server" SkinID="btnSmall" /></div>
                            <div class="row">
                                <asp:Button ID="btnDevolverTodosCaracteristica" Text="<<" runat="server" SkinID="btnSmall" /></div>
                        </div>
                        <div class="col-md-5">
                            <asp:ListBox ID="lbxSeleccionValores" TabIndex="17" runat="server" Width="100%" Height="120px"
                                Style="background-image: none; background-color: #fff;"></asp:ListBox>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            </asp:UpdatePanel> 
            </div>
            <hr />
            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                    <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CssClass="disabled" CausesValidation="false" />
                </div>
            </div>
        </fieldset>
        <br />
    </div>
    </form>
</body>
</html>
