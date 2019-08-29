<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAutorizacionCartas.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmAutorizacionCartas" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server"> <title>Autorizaci&oacute;n</title> 
<script type ="text/javascript" language="javascript">
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
	</script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="UPCuerpo" runat="server" UpdateMode ="Conditional">
    <ContentTemplate>
    <div class="container-fluid">
        <header>
        <div class="row"><div class="col-md-6"><h2>Autorizaci&oacute;n de Cartas</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Mercado</label>
                        <div class="col-sm-8"><asp:DropDownList runat="server" ID="ddlMercado" Width="150px" /></div>


                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Banco</label>
                        <div class="col-sm-8"><asp:DropDownList runat="server" ID="ddlBanco" Width="280px" /></div>


                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Liquidaci&oacute;n</label>
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
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8"><asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" /></div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Intermediario</label>
                        <div class="col-sm-8"><asp:DropDownList runat="server" ID="ddlIntermediario" Width="280px" /></div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Estado Carta</label>
                        <div class="col-sm-8"><asp:DropDownList runat="server" ID="ddlEstado" Width="150px" /></div>
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
            <asp:Label Text="" runat="server" ID="NroCarta" />
        </fieldset>
        <br />
        <div class="grilla" style="border: 1px !important; height: 250px !important;">
            <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate><input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox"></HeaderTemplate>
                        <ItemTemplate >
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            <asp:Label ID="lbCodigo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacionCaja") %>'></asp:Label>
                            <asp:Label ID="lbEstadoCarta" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.EstadoCarta") %>'></asp:Label>
                            <asp:Label ID="lblCodigoOperacion" runat="server" style="display:none" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operaci&#243;n" />
                    <asp:BoundField DataField="ModeloCarta" HeaderText="Modelo Carta" />
                    <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Banco" />
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Importe" HeaderText="Monto" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="NumeroOrden" HeaderText="Codigo Orden" />
                    <asp:BoundField DataField="VBADMIN" HeaderText="V. B. Admin" />
                    <asp:BoundField DataField="VBGERF1" HeaderText="V. B. GER 1" />
                    <asp:BoundField DataField="VBGERF2" HeaderText="V. B. GER 2" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label id="lbCodAprob" runat="server" class="col-sm-6 control-label">
                        Cod. Firmante</label>
                    <div class="col-sm-6"><asp:TextBox runat="server" ID="tbCodAprob" CssClass="mayusculas" Width="70px" /></div>
                </div>
            </div>
            <div class="col-md-12" style="text-align: right;">
                <samp style="float:left;" ><asp:Button Text="Aprobar" runat="server" ID="btnAprobar" Visible="false" OnClientClick="return MostrarMensaje()"/></samp>
                <asp:Button Text="Firmar" runat="server" ID="btnFirmar" Visible="false" />
                <asp:Button Text="Vista" runat="server" ID="btnVista" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <asp:Label ID="lblMensaje" runat="server" Text="" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID ="btnVista" />
    </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>