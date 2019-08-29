<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaObligacionTecnica.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmBusquedaObligacionTecnica" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mantenimiento Obligación Técnica</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
             <header><h2>Mantenimiento Obligación Técnica</h2></header>
            <br />

            <fieldset>
                <legend>Búsqueda</legend>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Descripción Fondo</label>
                            <div class="col-sm-9">
                                <asp:TextBox runat="server" ID="txtDescripcion" CssClass="input-large" />
                            </div>
                        </div>
                    </div>  
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Fecha</label>
                            <div class="col-sm-9">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2" style="text-align:right;">
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

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="grilla">
                        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoObligacionTecnica") %>'>                            
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoObligacionTecnica") %>'>
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                                <asp:BoundField DataField="FechaFormato" HeaderText="Fecha"></asp:BoundField>
                                <asp:BoundField DataField="Monto" HeaderText="Monto"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <br />

            <header></header>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnImportar" runat="server" Text="Importar" />
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" />
            </div>
            <br />
        </div>
    </form>
</body>
</html>
