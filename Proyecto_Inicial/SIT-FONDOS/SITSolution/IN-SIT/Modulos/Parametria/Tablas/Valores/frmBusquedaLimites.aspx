<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaLimites.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaLimites" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Límites</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Límites</h2></header>
    
    <fieldset>
            <legend style="border:1px solid gray;">Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Código Límite</label>
                        <div class="col-md-9">
                            <asp:TextBox id="txtCodigo" runat="server" Width="115px" MaxLength="5"></asp:textbox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6"></div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Nombre Límite</label>
                        <div class="col-md-9">
                            <asp:TextBox id="txtNombreLimite" runat="server" Width="310px" MaxLength="50"></asp:textbox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6"></div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Situación</label>
                        <div class="col-md-9">
                            <asp:DropDownList id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="dgLista" runat="server" Width="100%" AutoGenerateColumns="False"
            CellPadding="1" SkinID="Grid" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoLimite") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoLimite") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoLimite" HeaderText="C&#243;digo L&#237;mite"></asp:BoundField>
                <asp:BoundField DataField="NombreLimite" HeaderText="L&#237;mite"></asp:BoundField>
                <asp:BoundField DataField="TipoCalculo" HeaderText="Tipo C&#225;lculo"></asp:BoundField>
                <asp:BoundField DataField="UnidadPosicion" HeaderText="Unidad Posici&#243;n"></asp:BoundField>
                <asp:BoundField DataField="ValorBase" HeaderText="Valor Base"></asp:BoundField>
                <asp:BoundField DataField="ClaseLimite" HeaderText="Clase L&#237;mite"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>        
    </div>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>