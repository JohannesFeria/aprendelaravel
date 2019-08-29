<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaPatrimonio.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Limites_frmBusquedaPatrimonio" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Patrimonio</title>

</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Patrimonio</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscarCabecera" />
                </div>
            </div>
        </fieldset>
        <br />

        <asp:UpdatePanel ID="upCabecera" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="grilla-small">
                <asp:GridView runat="server" ID="dgCabecera" SkinID="GridSmall" DataKeyNames="valor">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="Imagebutton3" runat="server" SkinID="imgCheck" CommandName="Select"
                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField Visible="False" DataField="Clasificacion" HeaderText="Clasificacion" />
                        <asp:BoundField Visible="False" DataField="valor" HeaderText="CodigoCategoriaIncDec" />
                        <asp:BoundField DataField="Nombre" HeaderText="Descripci&#243;n" />
                    </Columns>
                </asp:GridView>
                </div>

        
        <br />
        <div class="row">
            <div class="col-md-6">
                <h4>
                    Detalle Patrimonio</h4>
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Modificar" runat="server" ID="btnModificarCabecera" />
                <asp:Button Text="Cancelar" runat="server" ID="btnCancelarCabecera" />
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresarCabecera" />
            </div>
        </div>
        <br />

        <div class="grilla-small">
            <asp:GridView runat="server" ID="dgDetalle" SkinID="GridSmall" DataKeyNames="CodigoIncDec,CodigoPortafolioSBS">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="Imagebutton1" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEliminarDetalle" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField Visible="False" DataField="CodigoIncDec" HeaderText="Codigo" />
                    <asp:BoundField Visible="False" DataField="CodigoCategoriaIncDec" HeaderText="CodigoCategoriaIncDec" />
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Codigo Portafolio" Visible="false" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Fondo" />
                    <asp:BoundField DataField="Valor" HeaderText="Valor" />
                    <asp:BoundField DataField="TipoIngreso" HeaderText="Tipo Ingreso" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fondo</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Valor</label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="txtValor" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tipo Ingreso</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" Width="150px" ID="ddlTipoIngresoD">
                                <asp:ListItem Value="DISMINUCION">DISMINUCION</asp:ListItem>
                                <asp:ListItem Value="INCREMENTO">INCREMENTO</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Modificar" runat="server" ID="btnModificarDetalle" 
                        Width="79px" />
                    <asp:Button Text="Cancelar" runat="server" ID="btnCancelarDetalle" />
                    <asp:Button Text="Ingresar" runat="server" ID="btnIngresarDetalle" />
                </div>
            </div>
        </fieldset>

        <input id="hdCodigoCabecera" type="hidden" name="hdCodigoCabecera" runat="server">
        <input id="hdCodigoDetalle" type="hidden" name="hdCodigoDetalle" runat="server">
        </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscarCabecera" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModificarDetalle" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnIngresarDetalle" EventName="Click" />
            </Triggers>

        </asp:UpdatePanel>

        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" />
            </div>
        </div>
        <br />
    </div>    
    </form>
</body>
</html>
