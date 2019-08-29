<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaPatrimonioFideicomiso.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaPatrimonioFideicomiso" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Patrimonio Fideicomiso</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Patrimonio Fideicomiso</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Patrimonio Fideicomiso</label>
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
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
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
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="_CodigoPatrimonioFideicomiso" Value="<%# Bind('CodigoPatrimonioFideicomiso') %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Patrimonio Fideicomiso" />
                            <asp:BoundField DataField="TotalActivo" HeaderText="Total Activo" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="TotalPasivo" HeaderText="Total Pasivo" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="Patrimonio" HeaderText="Patrimonio" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="FechaVigencia" HeaderText="Fecha Vigencia" />
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
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
            <asp:Button Text="Importar" runat="server" ID="btnImportar" />
            <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
            <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
    </div>
    </form>
</body>
</html>
