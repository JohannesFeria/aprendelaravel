<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCorreccionDepositoPlazo.aspx.vb" Inherits="Modulos_Inversiones_frmCorreccionDepositoPlazo" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Correci&oacute;n Dep&oacute;sitos a Plazo</title>
    <script type="text/javascript">
        function showModalDeposito() {
            var portafolio = document.getElementById('ddlFondo').value;
            var nroOrden = document.getElementById('hdNroOrden').value;
            var tipoTitulo = document.getElementById('hdTipoTitulo').value;
            var codigoOperacion = document.getElementById('hdCodigoOperacion').value;
            if (portafolio != '' && nroOrden != '' && nroOrden != '' && tipoTitulo != '' && codigoOperacion != '') {
                return showModalDialog('InstrumentosNegociados/frmDepositoPlazos.aspx?PTFondo=' + portafolio + '&CodigoOrden=' + nroOrden + '&PTNeg=CDP&CodigoTipoTitulo=' + tipoTitulo + '&PTOperacion=' + codigoOperacion, '1200', '600', '');
            }
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"> <h2> Correci&oacute;n Dep&oacute;sitos a Plazo</h2></div></div></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fondo</label>
                        <div class="col-sm-8"><asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true" /></div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date"  />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscarOPE" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionarOPE" runat="server" SkinID="imgCheck" CommandName="Select"
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NroOrden" HeaderText="Nro Orden" />
                    <asp:BoundField DataField="Intermediario" HeaderText="Intermediario" />
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" />
                    <asp:BoundField DataField="Plazo" HeaderText="Plazo" />
                    <asp:BoundField DataField="Tasa" HeaderText="Tasa (%)" />
                    <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS" />
                    <asp:BoundField DataField="CodigoTipoTitulo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoOperacion" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Modificar" runat="server" ID="btnEjecutar" OnClientClick="return showModalDeposito();" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hdNroOrden" type="hidden" name="hdNroOrden" runat="server">
    <input id="hdTipoTitulo" type="hidden" name="hdTipoTitulo" runat="server">
    <asp:HiddenField ID="hdCodigoOperacion" runat="server"/>
    </form>
</body>
</html>