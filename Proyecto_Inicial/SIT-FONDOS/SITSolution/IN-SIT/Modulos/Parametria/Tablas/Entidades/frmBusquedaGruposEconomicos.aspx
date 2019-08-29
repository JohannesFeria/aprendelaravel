<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaGruposEconomicos.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Entidades_frmBusquedaGruposEconomicos" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Intermediario Contacto</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                Grupos Económicos</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-2 control-label">
                            Código</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="tbCodigo" runat="server" MaxLength="4" Width="80px"></asp:TextBox></div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-2 control-label">
                            Descripción</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="tbDescripcion" runat="server" Width="360px" MaxLength="50"></asp:TextBox></div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-2 control-label">
                            Situación</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-9">
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <div class="col-sm-12">
                    <asp:Label ID="lbContador" runat="server"></asp:Label></div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoGrupoEconomico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoGrupoEconomico") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoGrupoEconomico" HeaderText="C&#243;digo">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
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
            <div class="form-group">
                <div class="col-md-1">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Visible="false" /></div>
                <div class="col-md-8">
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
