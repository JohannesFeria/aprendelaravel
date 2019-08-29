<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaModalidadesPago.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Tesoreria_Default" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Modalidad de Pago</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>Modalidad de Pago</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Descripción</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="tbDescripcion" runat="server" Width="440"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <lable class="col-md-3 control-label">Situación</lable>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="120px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-9">
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />

        <asp:UpdatePanel runat="server" ID="UP1">
        <ContentTemplate>
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <asp:Label ID="lbContador" runat="server"></asp:Label>
            </div>
        </fieldset>
        <br />
        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoModalidadPago") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoModalidadPago") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoModalidadPago" HeaderText="C&#243;digo "></asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                    <asp:BoundField Visible="False" DataField="Naturaleza" HeaderText="Naturaleza"></asp:BoundField>
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
        </asp:UpdatePanel>
        <header></header>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <div class="col-md-3">
                        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Visible="false" />
                    </div>
                    <div class="col-md-9">
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <div class="col-md-7">
                    </div>
                    <div class="col-md-5" style="text-align: right;">
                        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
                        <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
