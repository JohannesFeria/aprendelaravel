<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaValores.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_AdministracionValores_frmBusquedaValores" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Administraci&oacute;n de Valores</title>
    <script type="text/javascript">
        function showModal() {
            return showModalDialog('../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');    
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Administraci&oacute;n de Valores</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Isin</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigoIsin" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigoSBS" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbMnemonico" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Renta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Emisor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbEmisor" />
                                <asp:LinkButton runat="server" ID="lkbShowModal" OnClientClick="return showModal();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="150px" />
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
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNemonico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNemonico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoISIN" HeaderText="C&#243;digo ISIN" />
                            <asp:BoundField DataField="CodigoNemonico" HeaderText="C&#243;digo Mnemonico" />
                            <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS" />
                            <asp:BoundField DataField="Agrupacion" HeaderText="Agrupaci&#243;n" />
                            <asp:BoundField DataField="Emisor" HeaderText="Emisor" />
                            <asp:BoundField DataField="TipoRenta" HeaderText="Tipo Renta" />
                            <asp:BoundField DataField="Mercado" HeaderText="Mercado" />
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="TipoTitulo" HeaderText="TipoT&#237;tulo" />
                            <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
