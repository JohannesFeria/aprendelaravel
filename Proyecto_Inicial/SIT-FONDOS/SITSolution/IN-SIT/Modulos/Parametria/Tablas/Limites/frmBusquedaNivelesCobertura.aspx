<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaNivelesCobertura.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Limites_frmBusquedaNivelesCobertura" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Niveles de Cobertura</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Niveles de Cobertura</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-1 control-label">
                            Tercero</label>
                        <div class="col-sm-11">
                            <asp:DropDownList runat="server" Width="290px" ID="ddlTercero" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-9">
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
                            <asp:TemplateField ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" OnCommand="Modificar" SkinID="imgEdit"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTercero") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTercero") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoEntidad" HeaderText="Código Entidad" />
                            <asp:BoundField DataField="CodigoTercero" HeaderText="Código Tercero" />
                            <asp:BoundField DataField="NombreTercero" HeaderText="Tercero" />
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" />
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
