<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaMedioTransmision.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaMedioTransmision" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Medio de Transmisión</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Medio de Transmisión</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Tipo Renta</label>
                <div class="col-md-9">
                    <asp:dropdownlist style="Z-INDEX: 0" id="ddlTipoRenta" runat="server" Width="160px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Nombre</label>
                <div class="col-md-9">
                    <asp:textbox id="txtNombre" Width="250px" CssClass="stlCajaTexto" Runat="server" style="Z-INDEX: 0"></asp:textbox>
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
    <div class="row">
        <asp:label id="lblContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <ContentTemplate>
        <asp:GridView ID="dgLista" runat="server" Width="100%" AutoGenerateColumns="False" SkinID="Grid"
            CellPadding="1" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEditar" runat="server" SkinID="imgEdit"
                            CommandName="Editar" AlternateText="Editar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Valor") %>'>
                        </asp:ImageButton>&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" 
                            CommandName="Eliminar" AlternateText="Eliminar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Valor") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                <asp:BoundField DataField="TipoRenta" HeaderText="Tipo Renta"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>        
    </div>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
