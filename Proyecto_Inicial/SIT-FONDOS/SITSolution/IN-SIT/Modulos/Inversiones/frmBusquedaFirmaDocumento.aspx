<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaFirmaDocumento.aspx.vb"
    Inherits="Modulos_Inversiones_frmBusquedaFirmaDocumento" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Firma de Documentos</title>
    <script type="text/javascript">
        function showPopupOrdenInversion() {
            window.showModalDialog('frmPopupBusquedaOI.aspx', '', 'dialogHeight:550px;dialogWidth:789px;status:no;unadorned:yes;help:No');
        }
        function SelectAll(divName, CheckBoxControl) {
            $('#' + divName + ' :checkbox').each(function () {
                if (!$(this).attr('disabled')) {
                    $(this).prop('checked', $(CheckBoxControl).prop('checked'));
                }
            });
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
                        Firma de Documentos
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Reporte</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlReporte" Width="220px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label id="lblTipoReporte" runat="server" class="col-sm-4 control-label">
                            Categor&iacute;a Reporte</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCategoria" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cargo Firmante</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCargoFirmante" Width="220px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="trPortafolio" runat="server">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="trCodigoOrden" runat="server">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Orden</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoOrden" Width="150px" />
                                <asp:LinkButton Text="" ID="lkbCodigoPopUp" runat="server" OnClientClick="showPopupOrdenInversion()">
                                <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operaci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Estado
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlEstado" Width="120px" />
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
        <div id="divBandeja1" runat="server" class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista1">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkSeleccionTodo1" onclick="SelectAll('divBandeja1',this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSeleccion1" runat="server" />
                            <input id="hdCodFirmaDocumento1" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.CodFirmaDocumento") %>' />
                            <input id="hdCodCargoFirmante1" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.CodCargoFirmante") %>' />
                            <input id="hdCodCategReporte1" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.CodCategReporte") %>' />
                            <input id="hdFechaDocumento1" runat="server" type="hidden" value='<%# DataBinder.Eval(Container,"DataItem.FechaDocumento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />
                    <asp:BoundField DataField="FechaOperacion" HeaderText="Fec. Ope." />
                    <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fec. de Liq." />
                    <asp:BoundField DataField="Plazo" HeaderText="Ndías" />
                    <asp:BoundField DataField="Intermediario" HeaderText="Intermediario" />
                    <asp:BoundField DataField="Tasa" HeaderText="Tasa%" DataFormatString="{0:#,##0.0000}" />
                    <asp:BoundField DataField="TipoCambio" HeaderText="Tip.Cam." DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="Operacion" HeaderText="Operaci&#243;n" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="CodigoOrden" HeaderText="Num.Ope." />
                    <asp:BoundField DataField="DescCargoFirmante" HeaderText="Cargo Firmante" />
                    <asp:BoundField DataField="DescEstado" HeaderText="Estado" />
                    <asp:BoundField DataField="CantFirmas" HeaderText="Cant. Firmas" />
                </Columns>
            </asp:GridView>
        </div>
        <div id="divBandeja2" runat="server" class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista2">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkSeleccionTodo2" onclick="SelectAll('divBandeja2',this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSeleccion2" runat="server" />
                            <input id="hdCodFirmaDocumento2" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.CodFirmaDocumento") %>' />
                            <input id="hdCodCargoFirmante2" runat="server" type="hidden" value='<%# DataBinder.Eval(Container,"DataItem.CodCargoFirmante") %>' />
                            <input id="hdCodCategReporte2" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.CodCategReporte") %>' />
                            <input id="hdFechaDocumento2" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.FechaDocumento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FrmFechaDocumento" HeaderText="Fecha" />
                    <asp:BoundField DataField="DescCategReporte" HeaderText="Categoria" />
                    <asp:BoundField DataField="DescCargoFirmante" HeaderText="Cargo Firmante" />
                    <asp:BoundField DataField="DescEstado" HeaderText="Estado" />
                    <asp:BoundField DataField="CantFirmas" HeaderText="Cant. Firmas" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Clave</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="tbClave" Width="120px" />
                    </div>
                </div>
            </div>
            <div class="col-md-3" style="text-align: right;">
                <asp:Button Text="Firmar" runat="server" ID="btnFirmar" />
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
