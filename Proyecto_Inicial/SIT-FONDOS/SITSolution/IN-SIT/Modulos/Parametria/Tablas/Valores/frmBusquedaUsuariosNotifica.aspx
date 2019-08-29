<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaUsuariosNotifica.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaUsuariosNotifica" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>B&uacute;squeda de Personal</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        B&uacute;squeda de Personal</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            C&oacute;digo Usuario</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodUsu" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Primer Nombre</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPriNom" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Primer Apellido</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPriApe" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Unidades/Puestos</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlUnidad" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <%--       <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />--%>
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" OnCommand="SeleccionarUsuario"
                                        SkinID="imgCheck" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoUsuario")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Nombre")&amp;","&amp;DataBinder.Eval(Container, "DataItem.NombreCentroCosto")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoCentroCosto")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoInterno") %>'
                                        CausesValidation="False"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoUsuario" HeaderText="C&#243;digo Usuario" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre Completo" />
                            <asp:BoundField DataField="NombreCentroCosto" HeaderText="Unidad/Puesto" />
                            <asp:BoundField Visible="False" DataField="CodigoCentroCosto" />
                            <asp:BoundField Visible="False" DataField="CodigoInterno" />
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
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
