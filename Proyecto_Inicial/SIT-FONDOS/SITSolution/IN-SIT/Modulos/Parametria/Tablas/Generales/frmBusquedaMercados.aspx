<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaMercados.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaMercados" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Mercados</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
    <header>
        <h2>Mercados</h2>
    </header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
               <label class="col-md-3 control-label">Código</label>
               <div class="col-md-9">
                <asp:TextBox id="txtCodigo" runat="server" Width="32px" MaxLength="3"></asp:TextBox>            
               </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
               <label class="col-md-3 control-label">Descripción</label>
               <div class="col-md-9">
               <asp:TextBox id="txtDescripcion" runat="server" Width="152px" MaxLength="10"></asp:TextBox>         
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
               <asp:DropDownList id="ddlSituacion" runat="server" Width="115px" ></asp:DropDownList>
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
            <asp:GridView ID="dgLista" runat="server" SkinID="Grid" GridLines="None" CellPadding="1" AutoGenerateColumns="False">            
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">                    
                    <ItemTemplate>								
                        <asp:ImageButton id=ibModificar runat="server" OnCommand="Modificar" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMercado") %>'></asp:ImageButton>
					</ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">                    
                    <ItemTemplate>
						<asp:ImageButton id=ibEliminar runat="server"  OnCommand="Eliminar" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoMercado") %>'></asp:ImageButton>
					</ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoMercado" HeaderText="C&#243;digo"></asp:BoundField>
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
