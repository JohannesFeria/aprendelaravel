<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAutorizacionCartas.aspx.vb"
    Inherits="Modulos_Tesoreria_OperacionesCaja_frmAutorizacionCartas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Autorizaci&oacute;n</title>
    <style type="text/css">
        .win01
        {
            border: solid 1px gray;
            background-color: #80808052;
            position: absolute;
            z-index: 10;
            width: 100%;
            height: 100%;
            text-align: center;
        }
        .cont01
        {
            border: solid 1px gray;
            background-color: white;
            display: inline-block;
            margin-top: 120px;
            padding: 10px 20px;
            border-radius: 5px;
        }
        
        .tab01
        {
            width: 100%;
        }
        .tab01 > tbody > tr > td
        {
            padding: 2px 5px;
            border: Solid 1px gray;
        }
        .span01
        {
            font-size: 17px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ValidarSeleccion(emision) {
            if (document.getElementById("hdNumeroCarta").value == '') { alertify.alert('Seleccione una carta.'); return false; }
        }
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        if (document.forms[0].elements[i].disabled != true || $("#ddlEstado").val() == '3') {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            } else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
        function SeleccionarOperacionesCaja() {
            var i;
            var count;
            count = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
			(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                    if (document.forms[0].elements[i].disabled != true) {
                        if (document.forms[0].elements[i].checked) {
                            count = count + 1;
                        }
                    }
                }
            }
            if (count > 0) { return true; }
            else { return false; }
        }
        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= tbCodAprob.ClientID %>").value == "")
                strMsjCampOblig += "\t-Codigo Firmante\n"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            } { return true; }
        }
        function ValidarIngresoFirmas() {
            strMensajeError = "";
            if (ValidaCamposObligatorios()) {
                if (SeleccionarOperacionesCaja()) {
                    return true;
                }
                else {
                    alertify.alert('Debe seleccionar algún registro! ');
                    return false;
                }
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function MostrarMensaje() {
            var resultado;
            resultado = 0;
            resultado = ValidarCartasSeleccionadas();
            if (resultado > 0) {
                var men;
                men = confirm('¿ Está seguro de realizar la aprobación de ' + resultado + ' Carta(s) ?');
                if (men) { return true; }
                else { return false; }
            }
        }
        function ValidarCartasSeleccionadas() {
            var i;
            var contador = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                    if (document.forms[0].elements[i].checked == true) { contador = contador + 1; }
                }
            }
            return contador;
        }
        function ValidarAprobacion() {
            if (SeleccionarOperacionesCaja()) { return true; }
            else {
                alertify.alert('Debe seleccionar algún registro! ');
                return false;
            }
        }

        function agregarFilaDetalleOperaciones(Operacion, CodigoOrden, Intermediario, Monto) {
            $('#tablaCuentas > tbody:last-child').append('<tr><td>' + Operacion + '</td><td>' + CodigoOrden + '</td><td>' + Intermediario + '</td><td style="text-align: right;">' + Monto + '</td></tr>');
        }
        function agregarCabeceraTabla() {
            $('#tablaCuentas').append('<tr style="background-color: #e3d829; font-weight:bold"><td>Tipo Operación</td><td>Codigo Orden</td><td>Intermediario</td><td>Monto</td></tr>')
        }
        function EliminarDetalleOperaciones() {
            $('#tablaCuentas > tbody').html("");
        }

        function MuestraObservacion() {
            document.getElementById("IdObservacionMultiple").style.display = 'block';
            document.getElementById("btnGrabarobs").style.display = 'block';
        }

        function OcultaObservacion() {
            document.getElementById("IdObservacionMultiple").style.display = 'none';
            document.getElementById("btnGrabarobs").style.display = 'none';
        }

        function GrabarObservacion() {
            var observacion =  document.getElementById('txtObservacionAccion').value;
            document.getElementById('hdnObservacion').value = observacion;
            document.getElementById("<%= btnGrabarObservacion.ClientID %>").click();
        }
        function mostrarDetalleOperaciones() {
            $("#popup01").show();
        }

        function MostrarObservacion(Observacion) {
            document.getElementById('txtObservacionAccion').value = Observacion;
        }

        $(document).ready(function () {
            $("#Popup01_btnCerrar").click(function () { $("#popup01").hide(); });
        });

        function CerrarModal() {
            $("#popup01").hide();
        }
    </script>
</head>
<body>
    <div id="popup01" class="win01" style="display: none">
        <div class="cont01">
            <span class="span01">Detalle de Operación</span><br />
            <br />
            <table id="tablaCuentas" class="tab01">
                <%--<tr style="background-color: #e3d829; font-weight:bold">
                    <td>Tipo Operación</td>
                    <td>Codigo Orden</td>
                    <td>Intermediario</td>
                    <td>Monto</td>
                </tr>--%>
            </table>
            <br />
            <div id="IdObservacionMultiple">
                <div class="form-group">
                    <div class="col-md-3">
                        Observación: 
                    </div>
                    <div class="col-md-9">
                        <textarea id="txtObservacionAccion" class="md-textarea form-control" ></textarea>
                    </div>
                </div>
            </div>
            <div class="row col-md-12">
            <br />
                <div class="col-md-6 text-right">
                    <input type="submit" value="Salir" id="Popup01_btnCerrar" class="btn btn-integra"
                        style="min-width: 80px; text-align: center; width: auto;" />
                </div>
                <div class="col-md-6  text-right">
                    <input type="submit" value="Grabar" id="btnGrabarobs" class="btn btn-integra"
                        onclick="GrabarObservacion();" style="min-width: 80px; text-align: center; width: auto;" />
                </div>
            </div>
        </div>
    </div>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>        
        <div class="row"><div class="col-md-6"><h2>Autorizaci&oacute;n de Cartas</h2></div></div>
        </header>
        <asp:UpdatePanel ID="UPCuerpo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-md-3">
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
                                <label class="col-sm-3 control-label">
                                    Banco</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlBanco" Width="250px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Liquidaci&oacute;n</label>
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
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Portafolio</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Intermediario</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlIntermediario" Width="250px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Estado Carta</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlEstado" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="text-align: right;">
                            <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Resultados de la B&uacute;squeda</legend>
                    <div class="row">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label id="lbCodAprob" runat="server" class="col-sm-6 control-label">
                                    Cod. Firmante</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="tbCodAprob" CssClass="mayusculas" Width="70px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:Label ID="lblMensajeClave" runat="server" Text="" class="col-sm-10 control-label"
                                    Style="color: Red;" />
                            </div>
                        </div>
                        <div class="col-md-6" style="text-align: right;">
                            <asp:Button Text="Aprobar" runat="server" ID="btnAprobar" UseSubmitBehavior="false" />
                            <asp:Button Text="Firmar" runat="server" ID="btnFirmar" Visible="false" UseSubmitBehavior="false" />
                            <asp:Button Text="Vista" runat="server" ID="btnVista" UseSubmitBehavior="false" />
                            <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblMensaje" runat="server" Text="" Visible="false" />
                            <asp:Label Text="" runat="server" ID="NroCarta" />
                        </div>
                    </div>
                </fieldset>
                <div class="grilla" style="border: 1px !important; height: 400px !important;">
                    <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgLista" Style="font-size: 12px;">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox"></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.check") %>'
                                        OnCheckedChanged="dgLista_CheckedChanged"></asp:CheckBox>
                                    <asp:Label ID="lbCodigo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacionCaja") %>' />
                                    <asp:Label ID="lbFondo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPortafolioSBS") %>' />
                                    <asp:Label ID="lbEstadoCarta" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.EstadoCarta") %>' />
                                    <asp:Label ID="lblCodigoOperacion" runat="server" Style="display: none" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>' />
                                    <asp:Label ID="lblCodigoModeloCarta" runat="server" Style="display: none" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoModelo") %>' />
                                    <asp:Label ID="lblNumeroCuenta" runat="server" Style="display: none" Text='<%# DataBinder.Eval(Container, "DataItem.NumeroCuenta") %>' />
                                    <asp:Label ID="lblTipo" runat="server" Style="display: none" Text='<%# DataBinder.Eval(Container, "DataItem.Tipo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CorrelativoCartas" HeaderText="Orden" />
                            <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                            <asp:BoundField DataField="ModeloCarta" HeaderText="Modelo Carta" />
                            <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Banco" />
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="Número Cuenta" />
                            <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="Importe" HeaderText="Monto" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="NumeroOrden" HeaderText="Codigo Orden" HeaderStyle-CssClass="hidden"
                                ItemStyle-CssClass="hidden" />
                            <asp:TemplateField HeaderText="Cant. Órdenes" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetalleOperaciones" runat="server" Text="" Style="font-size: 15px;
                                        font-weight: bold;" CommandName="MostrarDetalle_Operaciones" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"></asp:LinkButton>
                                    <input type="hidden" id="hdCantidadOperacion" runat="server" value='<%# Eval("CantidadOperacion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="VBADMIN" HeaderText="Aprobador" />
                            <asp:BoundField DataField="VBGERF1" HeaderText="Firma 1" />
                            <asp:BoundField DataField="VBGERF2" HeaderText="Firma 2" />
                            <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolioSBS"
                                HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                            <asp:BoundField DataField="CodigoCartaAgrupado" HeaderText="CodigoCartaAgrupado"
                                HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                            <asp:BoundField DataField="CodigoModelo" HeaderText="CodigoModelo" HeaderStyle-CssClass="hidden"
                                ItemStyle-CssClass="hidden" />
                            <asp:BoundField DataField="CodigoAgrupado" HeaderText="CodigoAgrupado" HeaderStyle-CssClass="hidden"
                                ItemStyle-CssClass="hidden" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div  style="visibility: hidden">
                    <asp:HiddenField ID="hdnObservacion" runat="server" />
                    <asp:HiddenField ID="hdnCodigoAgrupacion" runat="server" />
                    <asp:Button ID="btnGrabarObservacion" runat="server" Text="Grabar" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnVista" />
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
