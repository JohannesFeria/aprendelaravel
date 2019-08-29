<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaLocacionBBH.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaLocacionBBH" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Locaciones BBH</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Locaciones BBH</h2></header>
    <br />
    <fieldset>
        <legend></legend>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Location</label>
                    <div class="col-md-9">
                        <asp:textbox id="tbDescripcion" runat="server" MaxLength="30" Width="267px"></asp:textbox>
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
                        <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
                    </div>
                </div>
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClientClick="confirmModal();" />
            </div>
        </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
        <asp:label id="lblContador" runat="server"></asp:label>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <ContentTemplate>
        <asp:GridView ID="dgLista" runat="server" Width="100%" GridLines="None" CellPadding="1"
            AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoLocacion") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoLocacion") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Mercado" HeaderText="Mercado"></asp:BoundField>
                <asp:BoundField DataField="SOD_Name" HeaderText="Location"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
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
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>    
    </form>
</body>
</html>
