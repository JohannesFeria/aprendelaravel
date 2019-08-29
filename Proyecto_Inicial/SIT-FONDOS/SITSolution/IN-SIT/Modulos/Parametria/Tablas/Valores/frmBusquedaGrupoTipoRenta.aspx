<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaGrupoTipoRenta.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaGrupoTipoRenta" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Grupo Por Tipo Renta</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Grupo Por Tipo Renta</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbGrupoInstrumento" runat="server" MaxLength="50" Width="147px"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Descripción</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbDescripcion" runat="server" MaxLength="50" Width="234px"></asp:TextBox>
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
                    <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px"></asp:DropDownList>
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
        <asp:label id="lbContador" runat="server"></asp:label>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GrupoInstrumento") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GrupoInstrumento") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="GrupoInstrumento" HeaderText="Codigo"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
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
    <br />
    </div>
    </form>
</body>
</html>
