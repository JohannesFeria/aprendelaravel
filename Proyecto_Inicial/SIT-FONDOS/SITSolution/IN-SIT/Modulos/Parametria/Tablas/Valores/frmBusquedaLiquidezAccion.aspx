<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaLiquidezAccion.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaLiquidezAccion" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Acciones seg&uacute;n Liquidez</title>
    <script type="text/javascript">
        function showPopupMnemonico() {
            $('#hdTipoModal').val('MNE');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');    

        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Liquidez Acciones</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"> C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                            <asp:TextBox runat="server" ID="tbCodigoMnemonico"  MaxLength="15"/>
                            <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
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
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMnemonico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMnemonico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoMnemonico" HeaderText="Codigo Mnemonico" />
                            <asp:BoundField DataField="DescripcionMnemonico" HeaderText="Descripcion Mnemonico" />
                            <asp:BoundField DataField="CriterioLiquidez" HeaderText="Criterio Liquidez" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="Situacion" HeaderText="Situacion" />
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
                <asp:HiddenField ID="hdTipoModal" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
