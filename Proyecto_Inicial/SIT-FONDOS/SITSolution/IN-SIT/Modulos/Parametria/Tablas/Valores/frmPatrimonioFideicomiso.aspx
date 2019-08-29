<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPatrimonioFideicomiso.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmPatrimonioFideicomiso" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Patrimonio Fideicomiso</title>
    <script type="text/javascript">

        function showPopup() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Valores', '1200', '600', '');  
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Mantenimiento de Patrimonio Fideicomiso</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="380px" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                CssClass="validator" ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Total Activo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbTotalActivo" Width="150px"  CssClass ="Numbox-7" />
                            <asp:RequiredFieldValidator ErrorMessage="Total Activo" ControlToValidate="tbTotalActivo"
                                CssClass="validator" ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Total Pasivo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbTotalPasivo" Width="150px" CssClass="Numbox-7" />
                            <asp:RequiredFieldValidator ErrorMessage="Total Pasivo" ControlToValidate="tbTotalPasivo"
                                CssClass="validator" ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Patrimonio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPatrimonio" Width="150px" CssClass="Numbox-7" />
                            <asp:RequiredFieldValidator ErrorMessage="Patrimonio" ControlToValidate="tbPatrimonio"
                                CssClass="validator" ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Vigencia</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVigencia" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha Vigencia" ControlToValidate="tbFechaVigencia"
                                CssClass="validator" ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Factor Riesgo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtFactorRiesgo" Width="150px"  CssClass="Numbox-7" /><asp:RequiredFieldValidator
                                ErrorMessage="Factor Riesgo" ControlToValidate="txtFactorRiesgo" ValidationGroup="vgCabecera"
                                CssClass="validator" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Factor Liquidez</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtFactorLiquidez" Width="150px" CssClass ="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                            <asp:HiddenField runat="server" ID="_CodigoPatrimonioFideicomisoDetalle" Value="<%# Bind('CodigoPatrimonioFideicomisoDetalle') %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField Visible="False" DataField="CodigoPatrimonioFideicomisoDetalle" HeaderText="CodigoPatrimonioFideicomisoDetalle" />
                    <asp:BoundField DataField="Emisor" HeaderText="Emisor" />
                    <asp:BoundField DataField="CodigoIsin" HeaderText="C&#243;digo Isin" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n" />
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="C&#243;digo Mnem&#243;nico" />
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonicoDetalle" Width="120px" />
                                <asp:LinkButton runat="server" ID="lkbModalMnemonico" OnClientClick="return showPopup()"
                                    CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Mnem&oacute;nico" ControlToValidate="tbCodigoMnemonicoDetalle"
                                ValidationGroup="vgDetalle" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <label class="col-sm-5 control-label">
                        Descripci&oacute;n</label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="tbDescripcionDetalle" MaxLength ="50" Width="320px" style="text-transform:uppercase"  />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" Width="120px" ID="ddlSituacionDetalle" />
                            <asp:RequiredFieldValidator ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacionDetalle"
                                ValidationGroup="vgDetalle" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregarDetalle" ValidationGroup="vgDetalle" />
                    <asp:Button Text="Modificar" runat="server" ID="btnModificarDetalle" ValidationGroup="vgDetalle" />
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnIngresar" ValidationGroup="vgCabecera" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <input id="hdCodigoPatrimonioFideicomiso" type="hidden" name="hdCodigoPatrimonioFideicomiso"
        runat="server">
    <input id="hdCodigoPatrimonioFideicomisoDetalle" type="hidden" name="hdCodigoPatrimonioFideicomisoDetalle"
        runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ValidationGroup="vgCabecera"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    <asp:ValidationSummary runat="server" ID="vsDetalle" ValidationGroup="vgDetalle"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
