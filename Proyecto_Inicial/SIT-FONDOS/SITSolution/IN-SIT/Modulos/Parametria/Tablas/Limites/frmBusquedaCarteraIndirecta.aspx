<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaCarteraIndirecta.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmBusquedaCarteraIndirecta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mantenimiento Cartera Indirecta</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <header><h2>Mantenimiento Cartera Indirecta</h2></header>
            <br />

            <fieldset>
                <legend>Búsqueda</legend>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Grupo Econ&oacute;mico</label>
                            <div class="col-sm-9">
                                <asp:TextBox runat="server" ID="txtGrupoEconomico" CssClass="input-large" width="300px"/>
                            </div>
                        </div>
                    </div>  
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Fondo</label>
                            <div class="col-sm-9">
                                <asp:TextBox runat="server" ID="txtFondo" CssClass="input-large" width="300px"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Emisor</label>
                            <div class="col-sm-9">
                                <asp:TextBox runat="server" ID="txtEmisor" CssClass="input-large" width="300px"/>
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

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="grilla">
                        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoCarteraIndirecta") %>'>                            
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoCarteraIndirecta") %>'>
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DesPortafolio" HeaderText="Fondo"></asp:BoundField>
                                <asp:BoundField DataField="DesGrupoEconomico" HeaderText="Grupo Económico"></asp:BoundField>
                                <asp:BoundField DataField="DesEmisor" HeaderText="Emisor"></asp:BoundField>
                                <asp:BoundField DataField="CodigoEntidad" HeaderText="Código Entidad"></asp:BoundField>
                                <asp:BoundField DataField="DesActividadEconomica" HeaderText="Actividad Económica"></asp:BoundField>
                                <asp:BoundField DataField="DesPais" HeaderText="País"></asp:BoundField>
                                <asp:BoundField DataField="DesRating" HeaderText="Rating"></asp:BoundField>
                                <asp:BoundField DataField="Posicion" HeaderText="Posición"></asp:BoundField>
                                <asp:BoundField DataField="Patrimonio" HeaderText="Patrimonio Cierre"></asp:BoundField>
                                <asp:BoundField DataField="Participacion" HeaderText="Participación (%)"></asp:BoundField>
                                <asp:BoundField DataField="FechaFormat" HeaderText="Fecha"></asp:BoundField>
                                <asp:BoundField DataField="DesSituacion" HeaderText="Estado"></asp:BoundField>

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
