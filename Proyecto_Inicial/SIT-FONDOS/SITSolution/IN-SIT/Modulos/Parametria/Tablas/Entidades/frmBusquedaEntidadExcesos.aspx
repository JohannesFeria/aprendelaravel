<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaEntidadExcesos.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmBusquedaEntidadExcesos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Brokers/Excesos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Brokers/Excesos</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Codig&oacute; Entidad </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigoEntidad" MaxLength="4" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>           
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripci&oacute;n </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDescripcion" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>           
             <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlSituacion" />
                        </div>
                    </div>
                </div>
                 <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="ibBuscar" />
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
                             <asp:TemplateField ItemStyle-Width="25" >
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoEntidad")  %>'
                                        OnCommand="Modificar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25" >
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoEntidad")  %>'
                                        OnCommand="Eliminar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoEntidad" HeaderText="C&#243;digo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" ItemStyle-HorizontalAlign="Left" />                           
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>                               
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibBuscar" EventName="Click" />
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

