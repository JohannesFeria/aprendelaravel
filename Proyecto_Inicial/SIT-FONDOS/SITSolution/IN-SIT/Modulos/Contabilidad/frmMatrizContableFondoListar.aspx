<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMatrizContableFondoListar.aspx.vb" Inherits="Modulos_Contabilidad_frmMatrizContableFondoListar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Matriz Contable - Portafolio</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Matriz Contable - Negocio</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Negocio</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlnegocio" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Matriz</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlMatriz" runat="server" AutoPostBack="false" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Situación</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlSituacion" runat="server" AutoPostBack="False" >
                                <asp:ListItem Value="A" Text="Activo" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="I" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Serie</label>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlSerie" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código Cabecera M</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="tbCodigoCabeceraM" runat="server" MaxLength="10" CssClass ="form-control" onkeypress="return soloNumeros(event)" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="ibtnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla" style="height: 350px;">
            <asp:UpdatePanel runat="server" ID="UpdPanelMatriz">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgCabecera">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName ="Modificar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoCabeceraMatriz") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName ="Anular" 
                                    OnClientClick="return confirm('¿Desea eliminar el registro seleccionado?')"
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoCabeceraMatriz") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoCabeceraMatriz">
                            </asp:BoundField>
                            <asp:BoundField DataField="CodigoCabeceraMatriz" HeaderText="Código"></asp:BoundField>
                            <asp:BoundField DataField="Negocio" HeaderText="Negocio"></asp:BoundField>
                            <asp:BoundField DataField="MatrizContable" HeaderText="Matriz"></asp:BoundField>
                            <asp:BoundField DataField="CodigoSerie" HeaderText="Serie"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscar" EventName="Click"/>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="ibtnIngresar" />
        </div>
    </div>
    <asp:HiddenField ID="hdCodigoDetalleMatriz" runat="server" />
    <asp:HiddenField ID="tbPortafolio" runat="server" />
    <asp:HiddenField ID="hdModal" runat="server" />
    <asp:HiddenField ID="btModal" runat="server" />
    </form>
</body>
</html>