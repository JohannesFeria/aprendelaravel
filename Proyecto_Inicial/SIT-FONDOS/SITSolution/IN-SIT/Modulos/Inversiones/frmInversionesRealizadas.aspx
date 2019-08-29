<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInversionesRealizadas.aspx.vb"
    Inherits="Modulos_Inversiones_frmInversionesRealizadas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self">
    <title>Cuentas Econ&oacute;micas</title>
    <script type="text/javascript">
        function OnSelectOrden(chk) {
            if (chk.checked) {
                color = chk.parentElement.parentElement.style.backgroundColor;
                chk.parentElement.parentElement.style.backgroundColor = "LemonChiffon";
            }
            else {
                chk.parentElement.parentElement.style.backgroundColor = color;
            }
        }

        function Close() {
            window.close();
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-sm-6">
                    <h2>
                        <asp:label text="" runat="server" id="lblTitulo" /></h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fondo
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblFondo" Width="150px" ReadOnly="true" />
                            <asp:HiddenField runat="server" ID="hdCodFondo" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblMoneda" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label ID="lbContador" runat="server"></asp:Label>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="updgrilla" runat="server">
                <ContentTemplate>
        <div class="grilla">
            
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgCheck" CommandName="Seleccionar"
                                        CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoOrden" HeaderText="Codigo Orden" />
                            <asp:BoundField DataField="CodigoISIN" HeaderText="Codigo ISIN" />
                            <asp:BoundField DataField="CodigoMnemonico" HeaderText="Codigo Mnemonico" />
                            <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS" />
                            <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                            <asp:BoundField DataField="CodigoEmisor" HeaderText="Emisor" />
                            <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operacion" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="Operacion" HeaderText="Operacion" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="CodigoMoneda"
                                HeaderText="CodigoMoneda" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="CodigoOperacion"
                                HeaderText="CodigoOperacion" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="FechaOperacion"
                                HeaderText="FechaOperacion" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="HoraOperacion"
                                HeaderText="HoraOperacion" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="TipoValor"
                                HeaderText="TipoValor" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="Precio"
                                HeaderText="Precio" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="PTasa"
                                HeaderText="PTasa" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="MontoNominalOrdenado"
                                HeaderText="MontoNominalOrdenado" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="MontoNominalOperacion"
                                HeaderText="MontoNominalOperacion" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="FechaUltimoCupon"
                                HeaderText="FechaUltimoCupon" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="FechaProximoCupon"
                                HeaderText="FechaProximoCupon" />
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="FechaVencimiento"
                                HeaderText="FechaVencimiento" />
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="Portafolio"
                                HeaderText="FechaVencimiento" />
                        </Columns>
                    </asp:GridView>
                <asp:HiddenField runat="server" ID="lblParametros" />
        </div>
        <br />
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Operaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbFechaOperacion" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Monto Operaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbMontoOperacion" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Hora Negociaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbHoraNegociacion" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Monto Nominal</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbMontoNominal" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Precio</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbPrecio" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Ultimo Cup&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbFechaUltimo" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Tasa</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbTasa" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Pr&oacute;ximo Cup&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbFechaProximo" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Tipo Valor</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbTipoValor" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Vencimiento</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lbFechaVencimiento" Width="150px" ReadOnly="true" />
                        <asp:Label Text="" runat="server" ID="lbParametros" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="RowEliminacion" runat="server">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Motivo de Eliminaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="150px" />
                        <asp:RequiredFieldValidator ValidationGroup="vgEliminar" ErrorMessage="Motivo de Eliminaci&oacute;n"
                            ControlToValidate="ddlMotivoCambio" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Comentarios Eliminaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtComentarios" CssClass="mayusculas" Width="150px"
                            TextMode="MultiLine" Rows="5" />
                        <asp:RequiredFieldValidator ValidationGroup="vgEliminar" ErrorMessage="Comentarios Eliminaci&oacute;n"
                            ControlToValidate="txtComentarios" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        </ContentTemplate>
            </asp:UpdatePanel>
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" ValidationGroup="vgEliminar" />
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
        </div>
    </div>
    <input type="hidden" runat="server" id="hdnCategoriaInstrumento" name="hdnCategoriaInstrumento">
    
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="vgEliminar" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
