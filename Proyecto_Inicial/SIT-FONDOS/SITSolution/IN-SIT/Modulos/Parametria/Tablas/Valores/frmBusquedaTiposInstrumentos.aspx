<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaTiposInstrumentos.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaTiposInstrumentos" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Tipo de Instrumentos</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Tipo de Instrumentos</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Clase de Instrumento</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlClaseInstrumento" runat="server" Width="288px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta" runat="server" Width="288px"></asp:dropdownlist>
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
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px"></asp:dropdownlist>
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
        <asp:Label id="lbContador" runat="server"></asp:Label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTipoInstrumento") %>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTipoInstrumento") %>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CodigoTipoInstrumento" HeaderText="C&#243;digo"></asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                        <asp:BoundField DataField="CodigoTipoInstrumentoSBS" HeaderText="C&#243;digo SBS">
                        </asp:BoundField>
                        <asp:BoundField DataField="NombreClaseInstrumento" HeaderText="Clase Instrumento">
                        </asp:BoundField>
                        <asp:BoundField DataField="NombreTipoTasa" HeaderText="Tipo de Tasa"></asp:BoundField>
                        <asp:BoundField DataField="PlazoLiquidacion" HeaderText="Plazo Liquidaci&#243;n">
                        </asp:BoundField>
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
    </div>
    </form>
</body>
</html>
