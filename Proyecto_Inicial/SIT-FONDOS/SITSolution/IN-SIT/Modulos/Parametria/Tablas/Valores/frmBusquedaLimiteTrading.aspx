<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaLimiteTrading.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaLimiteTrading" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Limites Trading</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Limites Trading</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta" runat="server" AutoPostBack="True" Width="160px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Cargo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoCargo" runat="server" Width="220px"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Grupo Límite</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlGrupoLimite" runat="server" Width="220px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" Width="140px" Runat="server"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
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
        <asp:label id="lbContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" SkinID="imgEdit"
                            CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoTrading") %>'
                            CommandName="Editar"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton2" runat="server" SkinID="imgDelete"
                            CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoTrading") %>'
                            CommandName="Eliminar"></asp:ImageButton>
                        <asp:Label ID="lbTipoRenta" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoRenta") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Renta" HeaderText="Tipo Renta"></asp:BoundField>
                <asp:BoundField DataField="NombreGrupoLimite" HeaderText="Grupo Límite"></asp:BoundField>
                <asp:BoundField DataField="TipoCargo" HeaderText="Cargo"></asp:BoundField>
                <asp:BoundField DataField="DescripcionPortafolioSBS" HeaderText="Portafolio"></asp:BoundField>
                <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje %" DataFormatString="{0:###,##0.00}"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <br />
    </div>
    </form>
</body>
</html>
