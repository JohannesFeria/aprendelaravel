<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaMonedas.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmMonedas" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Monedas</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid" >
    <header>
        <h2>Monedas</h2>
    </header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
        <div class="form-group">
        <label class="col-md-3 control-label">Código</label>
        <div class="col-md-9">
            <asp:TextBox id="txtCodigo" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
        </div>
        </div>
        </div>
        <div class="col-md-6">
        <div class="form-group">
        <label class="col-md-3 control-label">Código ISO</label>
        <div class="col-md-9">
            <asp:TextBox ID="txtCodigoIso" Runat="server" CssClass="stlCajaTexto" MaxLength="3"></asp:TextBox>
        </div>
        </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
        <div class="form-group">
        <label class="col-md-3 control-label">Descripción</label>
        <div class="col-md-9">
            <asp:TextBox id="txtDescripcion" runat="server" Width="360px" MaxLength="50"></asp:TextBox>
        </div>
        </div>
        </div>
        <div class="col-md-6">
        <div class="form-group">
        <label class="col-md-3 control-label">Sinónimo ISO</label>
        <div class="col-md-9">
            <asp:TextBox ID="txtSinonimoIso" Runat="server" MaxLength="3"></asp:TextBox>
        </div>
        </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
        <div class="form-group">
        <label class="col-md-3 control-label">Situación</label>
        <div class="col-md-9">
            <asp:DropDownList id="ddlSituacion" runat="server" Width="115px"></asp:DropDownList>
        </div>
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
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMoneda") %>'>
                            </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">                    
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMoneda") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoMoneda" HeaderText="C&#243;digo"></asp:BoundField>
                <asp:BoundField DataField="CodigoMonedaSBS" HeaderText="C&#243;digo SBS"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="NombreTipoCalculo" HeaderText="Tipo de C&#225;lculo"></asp:BoundField>
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
