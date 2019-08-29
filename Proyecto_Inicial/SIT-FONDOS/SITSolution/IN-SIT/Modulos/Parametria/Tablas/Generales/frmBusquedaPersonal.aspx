<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaPersonal.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaPersonal" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Monedas</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid" >
    <header>
        <h2>Personal</h2>
    </header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código Usuario</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtCodigoUsuario" MaxLength="10" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                </div>
            </div>
     <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nombre Completo</label>
                        <div class="col-sm-8">
                            <asp:TextBox id="txtNombreCompleto" runat="server" Width="360px" MaxLength="150"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
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
        <asp:GridView ID="dgLista" runat="server" Width="100%" GridLines="None" CellPadding="1" AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">                    
                    <ItemTemplate>                        
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoUsuario") %>'>
                            </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">                    
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoUsuario") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoUsuario" HeaderText="C&#243;digo Usuario"></asp:BoundField>
                <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre Completo"></asp:BoundField>                
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
    </div>
    </form>
</body>
</html>
