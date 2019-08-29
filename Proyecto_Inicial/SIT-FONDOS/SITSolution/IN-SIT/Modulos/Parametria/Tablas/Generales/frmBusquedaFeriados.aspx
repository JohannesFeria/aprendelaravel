<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaFeriados.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaFeriados" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Feriados</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Feriados</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Año</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtAnio" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-7">
                              <asp:DropDownList runat="server" ID="ddlSituacion" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                       
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                             <asp:TemplateField ItemStyle-Width="25">
                                <ItemTemplate>
            
                                    <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Fecha")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoMercado") %>'
                                        OnCommand="Modificar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Fecha")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoMercado") %>'
                                        OnCommand="Eliminar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NombreMercado" HeaderText="Mercado" />
                            <asp:BoundField DataField="CodigoMercado" HeaderText="CodigoMercado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" />                           
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>                               
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
              
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:Button Text="Ingresar" runat="server" ID="ibIngresar" Height="26px" />
                <asp:Button Text="Salir" runat="server" ID="ibCancelar" CausesValidation="false"/>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

