<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaFactor.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaFactor" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Factores por Emisor</title>
    <script type="text/javascript">
        function showPopupEntidad() {
            $('#hdTipoBusqueda').val('E');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');                
        }
        function showPopupMnemonico() {
            $('#hdTipoBusqueda').val('M');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');    
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h3>
                Factores por Emisor</h3>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tipo Factor</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlTipoFactor" runat="server" Width="145px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Código Mnemonico</label>
                        <div class="col-md-9">
                            <div class="input-append">
                                <asp:TextBox ID="tbCodigoMnemonico" runat="server" Width="148px" MaxLength="15"></asp:TextBox>
                                <asp:LinkButton ID="lkbMnemonico" runat="server" OnClientClick="return showPopupMnemonico();" ><span class="add-on"><i class="awe-search"></i>
                        </span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Código Entidad</label>
                        <div class="col-md-9">
                            <div class="input-append">
                                <asp:TextBox ID="tbCodigoEntidad" runat="server" Width="147px" MaxLength="50"></asp:TextBox>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return showPopupEntidad();"><span class="add-on"><i class="awe-search"></i>
                        </span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Situación</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="145px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <asp:Label ID="lbContador" runat="server"></asp:Label>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TipoFactor") %>'></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hdTipoFactor" value="<%# Bind('TipoFactor') %>" />
                                    <asp:HiddenField runat="server" ID="hdCodigoEntidad" value="<%# Bind('CodigoEntidad') %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TipoFactor") %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DescripcionTipoFactor" HeaderText="Tipo Factor"></asp:BoundField>
                            <asp:BoundField DataField="DescripcionMnemonico" HeaderText="Mnemonico"></asp:BoundField>
                            <asp:BoundField DataField="DescripcionEntidad" HeaderText="Entidad"></asp:BoundField>
                            <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoMnemonico" HeaderText="Codigo Mnemonico"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoEntidad" HeaderText="Codigo Entidad"></asp:BoundField>
                            <asp:BoundField DataField="TipoFactor" Visible="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
            <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
            <asp:Button ID="btnImportar" runat="server" Text="Importar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
            <asp:HiddenField ID="hdTipoBusqueda" runat="server" />            
        </div>
        <br />
    </div>
    </form>
</body>
</html>
