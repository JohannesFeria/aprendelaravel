<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInstrumentosEstructurados.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmInstrumentosEstructurados" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Agrupaci&oacute;n de Instrumentos Estructurados</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Agrupaci&oacute;n de Instrumentos Estructurados</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Isin
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lCodigoIsin" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            C&oacute;digo Mnem&oacute;nico
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="lCodigoNemo" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Renta Fija %
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbRF" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Renta Variable %
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbRV" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Derivado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoDerivado" Width="200px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Emisor</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlEmisor" Width="200px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo de Instrumento
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="150px" AutoPostBack="True" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo de Instrumento" ControlToValidate="ddlTipoInstrumento"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Emisi&oacute;n
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlEmision" Width="150px" AutoPostBack="True" />
                            <asp:RequiredFieldValidator ErrorMessage="Emisi&oacute;n" ControlToValidate="ddlEmision"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        </label>
                        <div class="col-sm-7">
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Factor
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCantidad" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Cantidad" ControlToValidate="tbCantidad"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Moneda Prima
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMonedaPrima" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla-small">
            <asp:GridView runat="server" SkinID="GridSmall" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Identificador") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Identificador") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField Visible="False" DataField="CodigoTipoInstrumento" HeaderText="Codigo Tipo Instrumento" />
                    <asp:BoundField DataField="DescripcionTipoInstrumento" HeaderText="Tipo Instrumento" />
                    <asp:BoundField Visible="False" DataField="CodigoNemonicoAsociado" HeaderText="Codigo Nemonico Asociado" />
                    <asp:BoundField DataField="Emision" HeaderText="Emision" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Factor" />
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" />
                    <asp:BoundField Visible="False" DataField="Identificador" HeaderText="Identificador" />
                    <asp:BoundField Visible="False" DataField="MonedaPrima" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hdNumeroUnidades" type="hidden" runat="server">
    <input id="hdPortafolio" type="hidden" runat="server">
    <input id="hdFechaEmision" type="hidden" runat="server">
    <input id="hdValorUnitario" name="hdValorUnitario" runat="server">
    <input id="hdMnemo" type="hidden" name="hdMnemo" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
