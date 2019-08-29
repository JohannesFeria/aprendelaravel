<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaNegocios.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaNegocios" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Negocio</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Negocio</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Codigo</label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="4" Width="56px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Descripción</label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="400px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situación</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlSituacion" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="text-align: right;">
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
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNegocio") %>'
                                        OnCommand="Modificar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNegocio") %>'
                                        OnCommand="Eliminar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoNegocio" HeaderText="C&#243;digo" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n" />
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
                <asp:Button Text="Salir" runat="server" ID="ibCancelar" CausesValidation="false" />
            </div>
        </div>
        <br />
    </div>
    </form>
</body>
</html>
