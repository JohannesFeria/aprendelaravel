<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaLimiteParametria.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmBusquedaLimiteParametria" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form  class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Parametria de Limites</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Tipo de Grupo</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" Width="290px" ID="ddltipogrupo" >
                                <asp:ListItem Value="">--Seleccione--</asp:ListItem>
                                <asp:ListItem Value="0">Grupo por tipo de Moneda.</asp:ListItem>
                                <asp:ListItem Value="1">Grupo por Clase de Instrumento.</asp:ListItem>
                                <asp:ListItem Value="2">Grupo por Entidad.</asp:ListItem>
                                <asp:ListItem Value="3">Grupo por Derivados.</asp:ListItem>
                                <asp:ListItem Value="4">Grupo por Nemonico.</asp:ListItem>
                                <asp:ListItem Value="5">Grupo por Calificacion.</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Codigo del Grupo</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtCodigo" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Nombre del Grupo</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtdescripcion" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnbuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="upgrilla" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                        <asp:TemplateField ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar" 
                                CommandArgument="<%#  CType(Container, GridViewRow).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:BoundField DataField="Codigo" HeaderText="Código" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="Situacion" HeaderText="Situacion" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
