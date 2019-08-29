<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaTiposValorizacion.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaTiposValorizacion" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Bcr Seriados y Unicos</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="Container-fluid">
    <header><h2>Bcr Seriados y Unicos</h2></header>
    <fieldset>
        <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Nombre de Cuenta</label>            
                <div class="col-sm-9">
                <asp:textbox id="tbDescripcion" runat="server" Width="400px" MaxLength="50"></asp:textbox>
            </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo BCR</label>            
                <div class="col-sm-9">
                <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" AutoPostBack="True">
					<asp:ListItem Value="Unico">Bcr Unicos</asp:ListItem>
					<asp:ListItem Value="Seriado">BCR Seriado</asp:ListItem>
				</asp:dropdownlist>
            </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align:right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
        <asp:label id="lbContador" runat="server"></asp:label>
    </fieldset>
    <br />

        <asp:UpdatePanel ID="upBusqueda" runat="server">
            <ContentTemplate>
                <div class="grilla">
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" Visible="False"
                        SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="ModificarBCRSeriado"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMNemonico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="EliminarBCRSeriado"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMNemonico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoMnemonico" HeaderText="C&#243;digo Mnemonico"></asp:BoundField>
                            <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable"></asp:BoundField>
                            <asp:BoundField DataField="NombreCuenta" HeaderText="Nombre Cuenta"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="dgBCRUnico" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="ModificarBCRUnico"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoSBS") %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminarUnicos" runat="server" SkinID="imgDelete" OnCommand="EliminarBCRUnico"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoSBS") %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoTipoTitulo" HeaderText="C&#243;digo Tipo T&#237;tulo">
                            </asp:BoundField>
                            <asp:BoundField DataField="CodigoEntidad" HeaderText="Codigo Entidad"></asp:BoundField>
                            <asp:BoundField DataField="CodigoMoneda" HeaderText="C&#243;digo Moneda"></asp:BoundField>
                            <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS"></asp:BoundField>
                            <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable"></asp:BoundField>
                            <asp:BoundField DataField="NombreCuenta" HeaderText="Nombre Cuenta"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ddlSituacion" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

    
    <br />
    <header></header>
    <div class="row" style="text-align: right">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
