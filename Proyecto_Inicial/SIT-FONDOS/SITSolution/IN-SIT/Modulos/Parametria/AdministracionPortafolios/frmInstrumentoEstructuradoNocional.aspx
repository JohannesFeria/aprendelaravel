<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInstrumentoEstructuradoNocional.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmInstrumentoEstructuradoNocional" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Intrumento Estructurado Nocional</title>
    <script type="text/javascript">
        function confirm_delete() {
            if (confirm("Esta seguro de eliminar el registro?") == true)
                return true;
            else
                return false;
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Intrumento Estructurado Nocional</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nocional
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtNocional" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Nocional" ControlToValidate="txtNocional"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Moneda" ControlToValidate="ddlMoneda" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                    <asp:Button Text="Modificar" runat="server" ID="btnModificar" />
                    <asp:Button Text="Cancelar" runat="server" ID="btnCancelar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla-small">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="GridSmall" ID="dgIENocional">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Select"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />
                            <asp:BoundField DataField="Nocional" HeaderText="Nocional" />
                            <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnModificar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            </div>
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
