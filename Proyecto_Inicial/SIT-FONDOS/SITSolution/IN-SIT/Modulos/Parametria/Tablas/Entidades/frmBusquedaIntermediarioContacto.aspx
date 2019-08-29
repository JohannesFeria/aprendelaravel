<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaIntermediarioContacto.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Entidades_frmBusquedaIntermediarioContacto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Intermediario Contacto</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <div class="row">
                <h2>
                    Intermediario Contacto</h2>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Contacto</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlContacto" runat="server" Width="232px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Tercero</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlTercero" runat="server" Width="416px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Situcación</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:Label ID="lbContador" runat="server"></asp:Label>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" OnCommand="Modificar" SkinID="imgEdit"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTercero") & "," & DataBinder.Eval(Container.DataItem, "CodigoContacto") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEliminar" runat="server" OnCommand="Eliminar" SkinID="imgDelete"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTercero") & "," & DataBinder.Eval(Container.DataItem, "CodigoContacto") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NombreContacto" HeaderText="Contacto">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoContacto" HeaderText="Tipo Contacto">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreTercero" HeaderText="Tercero">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
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
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="false"/>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
