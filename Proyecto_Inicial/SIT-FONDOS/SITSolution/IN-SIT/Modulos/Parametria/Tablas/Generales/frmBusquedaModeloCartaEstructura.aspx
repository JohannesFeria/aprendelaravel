<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaModeloCartaEstructura.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaModeloCartaEstructura" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Modelo de Carta Estructura</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Modelo de Carta Estructura</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="4" Width="56px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
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
            <asp:UpdatePanel ID="upGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar">
								<HeaderStyle Width="80px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" OnCommand="Modificar" SkinID="imgEdit"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoModelo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "NombreCampo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "OrigenCampo")&amp;","&amp;Container.DataItemIndex %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
								<HeaderStyle Width="80px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" OnCommand="Eliminar" SkinID="imgDelete"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoModelo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "NombreCampo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "OrigenCampo")&amp;","&amp;Container.DataItemIndex %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NombreCampo" HeaderText="Nombre" />
                            <asp:BoundField DataField="OrigenCampo" HeaderText="Origen" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <asp:label id="lblNombre" runat="server" Visible="False">Nombre</asp:label></label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtNombre" runat="server" Width="112px" Visible="False"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <asp:label id="lblOrigen" runat="server" Visible="False">Origen</asp:label></label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtOrigen" runat="server" Width="104px" Visible="False"></asp:TextBox>
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
                    <asp:Button Text="Aceptar" runat="server" ID="ibtnAceptar" Visible="False" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Ingresar" runat="server" ID="ibIngresar" Height="26px" />
                <asp:Button Text="Retornar" runat="server" ID="ibCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdOperacion" />
    <asp:HiddenField runat="server" ID="hdCodigoModelo" />
    <asp:HiddenField runat="server" ID="hdNombre" />
    <asp:HiddenField runat="server" ID="hdOrigen" />
    </form>
</body>
</html>
