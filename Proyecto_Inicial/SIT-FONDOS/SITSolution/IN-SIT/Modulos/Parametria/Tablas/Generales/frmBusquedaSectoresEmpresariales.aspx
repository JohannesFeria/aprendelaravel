<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaSectoresEmpresariales.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaSectoresEmpresariales" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Sector Empresarial</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Sector Empresarial</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Código</label>
                <div class="col-md-9"><asp:TextBox id="txtCodigo" runat="server" Width="64px" MaxLength="9" /></div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Descripción</label>
                <div class="col-md-9">
                    <asp:TextBox id="txtNombre" runat="server" Width="488px" MaxLength="40"></asp:TextBox>
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
    <div class="row">
        <asp:Label id="lblContador" runat="server"></asp:Label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="dgLista" runat="server" Width="100%" GridLines="None" CellPadding="1"
            AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" OnCommand="Modificar" SkinID="imgEdit"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoSectorEmpresarial") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" OnCommand="Eliminar" SkinID="imgDelete"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoSectorEmpresarial") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoSectorEmpresarial" HeaderText="C&#243;digo"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n"></asp:BoundField>
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
