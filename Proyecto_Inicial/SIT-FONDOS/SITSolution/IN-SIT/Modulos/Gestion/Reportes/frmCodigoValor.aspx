<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCodigoValor.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmCodigoValor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Código Valor</title>
        <script type="text/javascript">
            function showModal() {
                return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');
            }
        </script>
        <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
        <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Codigo Valor</h2></div>
            </div>
        </header>
        </div>
        <fieldset>
        <legend>Datos de Busqueda</legend>
            <div class="row">
                 <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código Nemonico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlnemonico" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Emisor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtEmisor" ReadOnly ="true"  />
                                <asp:LinkButton runat="server" ID="lkbShowModal" OnClientClick="return showModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                 <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Moneda</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlmoneda" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código Valor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtcodigovalor" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                 <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Situacion</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlsituacion" runat="server" >
                                    <asp:ListItem Value="">--Todos--</asp:ListItem>
                                    <asp:ListItem Value="A">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <fieldset>
                    <legend>Resultados de la B&uacute;squeda</legend>
                    <asp:Label Text="" runat="server" ID="lbContador" />
                </fieldset>
                <br />
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate><asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate><asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete"  CommandName ="Eliminar" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoValor" HeaderText="C&#243;digo Valor" />
                            <asp:BoundField DataField="CodigoNemonico" HeaderText="C&#243;digo Mnemonico" />
                            <asp:BoundField DataField="Sinonimo" HeaderText="Emisor" />
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="DSituacion" HeaderText="Situacion" />
                            <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:HiddenField ID="hdemisor" runat="server" />
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </form>
</body>
</html>
