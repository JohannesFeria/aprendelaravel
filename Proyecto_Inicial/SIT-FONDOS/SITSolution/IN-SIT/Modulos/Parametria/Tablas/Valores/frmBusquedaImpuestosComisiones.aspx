<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaImpuestosComisiones.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaImpuestosComisiones" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Impuestos y Comisiones</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Impuestos y Comisiones</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Descripción</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbDescripcion" runat="server" MaxLength="50" Width="272px"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Bolsa</label>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlBolsa" Width="180px" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <%--<div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mercado</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlMercado" runat="server" Width="272px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>--%>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta" runat="server" Width="272px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px"></asp:dropdownlist>
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
        <asp:Label id="lbContador" runat="server"></asp:Label>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UP1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoComision")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoPlaza")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoTipoRenta")%>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoComision")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoPlaza")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoTipoRenta")%>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoComision" HeaderText="C&#243;digo"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Bolsa" HeaderText="Bolsa"></asp:BoundField>
                <asp:BoundField DataField="NombreRenta" HeaderText="Tipo Renta"></asp:BoundField>
                <asp:BoundField DataField="CodigoTarifa" HeaderText="Base C&#225;lculo"></asp:BoundField>
                <asp:BoundField DataField="ValorComision" HeaderText="Valor"></asp:BoundField>
                <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoTipoRenta" HeaderText="CodigoRenta">
                </asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoMercado" HeaderText="CodigoMercado">
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>        
    </div>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
