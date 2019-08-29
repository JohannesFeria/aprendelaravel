<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaFactorEmision.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaFactorEmision" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Factores por Emisi&oacute;n</title>
    <script type="text/javascript">
        function showMnemonicoModal() {
            $('#_TipoModal').val("M");
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');             
        }

        function showEmisorModal() {
            $('#_TipoModal').val("E");
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', ''); 
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
                        Factores por Emisi&oacute;n</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Factor</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoFactor" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Mnemonico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width="160px" />
                                <asp:LinkButton runat="server" ID="lkbMnemonicoModal" OnClientClick="return showMnemonicoModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Emisor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoEmisor" Width="160px" />
                                <asp:LinkButton runat="server" ID="lkbEmisorModal" OnClientClick="return showEmisorModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
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
            <asp:Label Text="" runat="server" ID="lblContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="updGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar" CommandArgument='<%# CType(Container, GridViewRow).RowIndex  %>'></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="_TipoFactor" Value="<%# Bind('TipoFactor') %>" />
                                    <asp:HiddenField runat="server" ID="_CodigoEntidad" Value="<%# Bind('Codigoentidad') %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar" CommandArgument='<%# CType(Container, GridViewRow).RowIndex%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DescripcionTipoFactor" HeaderText="Tipo Factor" />
                            <asp:BoundField DataField="CodigoMnemonico" HeaderText="C&#243;digo Mnemonico" />
                            <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS" />
                            <asp:BoundField DataField="CodigoISIN" HeaderText="Codigo ISIN" />
                            <asp:BoundField DataField="FloatOficioMultiple" HeaderText="Factor" />
                            <asp:BoundField DataField="Situacion" HeaderText="Inactivo" />
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
        <div class="row" style="text-align: right;">
            <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
            <asp:Button Text="Importar" runat="server" ID="btnImportar" />
            <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
            <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
        <br />
    </div>
    <asp:HiddenField runat="server" Value="" ID="_TipoModal" />
    </form>
</body>
</html>
