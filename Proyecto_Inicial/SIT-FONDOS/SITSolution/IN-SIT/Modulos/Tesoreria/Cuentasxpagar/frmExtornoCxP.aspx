<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoCxP.aspx.vb" Inherits="Modulos_Tesoreria_Cuentasxpagar_frmExtornoCxP" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Anulaci&oacute;n de Cuentas por Pagar</title>
    <script type="text/javascript">
        var color;
        function SelectedRow(chk) {
            if (chk.checked) {
                color = chk.parentElement.parentElement.style.backgroundColor;
                chk.parentElement.parentElement.style.backgroundColor = "LemonChiffon";
            }else { chk.parentElement.parentElement.style.backgroundColor = color; }
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="UP" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <header>
                <div class="row"><div class="col-md-6"><h2>Anulaci&oacute;n de Cuentas por Pagar</h2></div></div>
            </header>
            <fieldset>
                <legend></legend>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Mercado</label>
                            <div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Portafolio</label>
                            <div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Intermediario</label>
                            <div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlIntermediario" Width="280px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Moneda</label>
                            <div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlMoneda" Width="280px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Operaci&oacute;n</label>
                            <div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlOperacion" Width="280px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Inicio Vencimiento</label>
                            <div class="col-sm-7">
                                <div class="input-append">
                                    <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" ReadOnly ="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" style="text-align: right;">
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
                <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chbConfirmar" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="NroOperacion" />
                        <asp:BoundField DataField="FechaNegociacion" HeaderText="Fec. Negoc." />
                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Fec. Vcto" />
                        <asp:BoundField DataField="NroOperacion" HeaderText="Nro. Operaci&#243;n" />
                        <asp:BoundField DataField="Referencia" HeaderText="Descripci&#243;n" />
                        <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" />
                        <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado" />
                        <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                        <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda" />
                        <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Intermediario" />
                        <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operaci&#243;n" />
                        <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Mov." />
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS" />
                        <asp:BoundField DataField="DescripcionEstado" HeaderText="Estado" />
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Estado" />
                        <asp:BoundField DataField="CodigoRenta" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    </Columns>
                </asp:GridView>
            </div>
            <header>
            </header>
            <div class="row">
                <div class="col-md-6">
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button Text="Anular" runat="server" ID="btnExtornar" />
                    <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                </div>
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    
    </form>
</body>
</html>
