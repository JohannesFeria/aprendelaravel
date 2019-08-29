<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaTipoInstrumento.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmBusquedaTipoInstrumento" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>HiperValorizador</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-sm-6"><h2>Tipo de Instrumento</h2></div></div></header>
        <fieldset>
            <legend>Tipo de Instrumento</legend>
            <div class="row">
                <div class="col-sm-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtCodigo" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtDescripcion" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="dgLista" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" OnCommand="Seleccionar" SkinID="imgCheck"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoTipoInstrumentoSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Descripcion")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoClaseInstrumento") %>'
                                        CausesValidation="False"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoTipoInstrumentoSBS" HeaderText="C&#243;digo" />
                            <asp:BoundField DataField="Descripcion" ItemStyle-HorizontalAlign="Left" HeaderText="Descripci&#243;n" />
                            <asp:BoundField DataField="CodigoClaseInstrumento" Visible="False" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            
        </div>
    </div>
    </form>
</body>
</html>
