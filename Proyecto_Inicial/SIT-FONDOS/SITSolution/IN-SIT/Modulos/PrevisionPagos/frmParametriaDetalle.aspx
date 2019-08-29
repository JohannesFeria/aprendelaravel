<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmParametriaDetalle.aspx.vb" Inherits="Modulos_PrevisionPagos_frmParametriaDetalle" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Feriados</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:HiddenField runat="server" ID="hdValor" Value="" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6" style="width:100%;">
                    <h2>Mantenimiento de Parámetros Generales - <asp:Label id="lblCabeceraParam" runat="server" /></h2> 
                </div>
            </div>
        </header>
        <asp:UpdatePanel runat="server" ID="up" UpdateMode="Always">
            <ContentTemplate>
                    <fieldset>
                        <legend>Datos&#32;Generales</legend>
                        <div class="row">
                            <div class="col-md-5">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Descripción</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox runat="server" ID="tbDescripcion" CssClass="stlCajaTexto" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfDescripcion" ErrorMessage="Descripción"
                                            Text="(*)" Display="Static" ControlToValidate="tbDescripcion"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Valor</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="tbValor" runat="server" CssClass="stlCajaTexto"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfValor" ErrorMessage="Valor" Text="(*)"
                                            Display="Static" ControlToValidate="tbValor"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2" style="text-align: right;">
                                <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                            </div>
                        </div>
                    </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="pnlFormulario" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgDetalle" OnRowCommand="dgDetalle_RowCommand"
                        OnRowDataBound="dgDetalle_RowDataBound" DataKeyNames="Valor">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar">
                                <HeaderStyle Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        AlternateText="Modificar" CausesValidation="false"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
                                <HeaderStyle Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        AlternateText="Eliminar" CausesValidation="false"></asp:ImageButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Valor" HeaderText="Valor">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <div class="row" style="text-align: right;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:Button ID="btnAceptar" runat="server" Width="72px" Text="Aceptar" CausesValidation="False"
                        CssClass="button"></asp:Button>
                    &nbsp;<asp:Button ID="bSalir" runat="server" Width="72px" Text="Retornar" CausesValidation="False"
                        CssClass="button"></asp:Button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
    </div>
    <asp:HiddenField runat="server" ID="hdCodigo" Value="" />
    <asp:ValidationSummary runat="server" ID="vsValidacion" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios :" />
    </form>
</body>
</html>



