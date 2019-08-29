<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaPortafolio.aspx.vb" Inherits="Modulos_Parametria_AdministracionPortafolios_frmBusquedaPortafolio" %>

<!DOCTYPE html >

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Administración Portafolios</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Administración Portafolios</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Descripción</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtDescripcion" runat="server" MaxLength="40" Width="248px"></asp:textbox>
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
                    <asp:DropDownList id="ddlSituacion" runat="server" ></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnBuscar" runat="server" text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
    <div class="row">
        <asp:Label id="lbContador" runat="server"></asp:Label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UP1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:GridView ID="dgLista" runat="server"  AutoGenerateColumns="False" SkinID="Grid">           
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                    <ItemTemplate>
                            <asp:ImageButton ID="btnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") %>'>
                            </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                             CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="C&#243;digo Portafolio SBS"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion"></asp:BoundField>
                <asp:BoundField DataField="fechaConstitucion" HeaderText="Fec. Constit."></asp:BoundField>
                <asp:BoundField DataField="FechaNegocio" HeaderText="Fecha de Negocio"></asp:BoundField>
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
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <br />
    </div>
    </form>
</body>
</html>
