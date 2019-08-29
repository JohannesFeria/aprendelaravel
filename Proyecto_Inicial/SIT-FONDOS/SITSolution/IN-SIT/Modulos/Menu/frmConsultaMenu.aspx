<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaMenu.aspx.vb" Inherits="Modulos_Menu_frmConsultaMenu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento Rol</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Mantenimiento Menu</h2>
                </div>
            </div>
        </header>

        <div class="Contenedor">
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nombre Menu</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbNombreMenu" />
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
        </div>

        <br />

        <div class="Contenedor">
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label runat="server" ID="lbContador" />
        </fieldset>
        </div>

        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                        CommandArgument='<%#Container.DataItemIndex%>'>
                                    </asp:ImageButton>
                                    <asp:HiddenField ID="hfCodAplicativo" runat="server" Value='<%# Eval("id") %>' />
                                    <asp:HiddenField ID="hfEstado" runat="server" Value='<%# Eval("ESTADO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%#Container.DataItemIndex%>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:BoundField DataField="name" HeaderText="Nombre Menu" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="file" HeaderText="Ruta" />
                            <asp:BoundField DataField="DESC_ESTADO" HeaderText="Situaci&#243;n" />
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
                <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
