<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaTipoCambio.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaTipoCambio" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Tipo de Cambio</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Tipo de Cambio</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Divisa</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlMonedaOrigen" runat="server" Width="180px" ></asp:dropdownlist>
                </div>
            </div>            
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Moneda</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlMonedaDestino" runat="server" Width="180px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Tipo</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlTipo" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>            
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Situación</label>
                <div class="col-md-5">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
                <div class="col-md-3" style="Text-align: right;">
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
        <asp:label id="lblContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <asp:GridView ID="dgLista" runat="server" GridLines="None" CellPadding="1"
            AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>                        
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit"
                                OnCommand="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTipoCambio") %>'>
                            </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete"
                            OnCommand="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTipoCambio") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField Visible="False" DataField="CodigoTipoCambio" HeaderText="C&#243;digo"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoMonedaOrigen" HeaderText="CodigoMonedaOrigen"></asp:BoundField>
                <asp:BoundField DataField="DescripcionMonedaOrigen" HeaderText="Divisa"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoMonedaDestino" HeaderText="CodigoMonedaDestino"></asp:BoundField>
                <asp:BoundField DataField="DescripcionMonedaDestino" HeaderText="Moneda"></asp:BoundField>
                <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="Observaciones" HeaderText="Observaciones"></asp:BoundField>
            </Columns>
        </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
        </asp:UpdatePanel>        
    </div>
    <br />
    <header></header>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
