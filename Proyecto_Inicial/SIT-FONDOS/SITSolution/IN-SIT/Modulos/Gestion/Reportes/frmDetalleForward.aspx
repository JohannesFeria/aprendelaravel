<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDetalleForward.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmDetalleForward" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento de Forward</title>
    <script type="text/javascript">
        function showModal() {
            window.showModalDialog('frmBuscaValor.aspx?sbs=' + img.innerText, '', 'dialogHeight:550px;dialogWidth:789px;status:no;unadorned:yes;help:No');
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
                        Mantenimiento de Forward</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodigoSBS" Width="150px" />
                                <asp:LinkButton ID="lkbShowModal" OnClientClick="showModal()" runat="server">
                                <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo SBS" ControlToValidate="txtCodigoSBS"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Indicador Movimiento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlIndicador" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Forward</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipo" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Monto Forward</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtMonto" CssClass="Numbox-7" Width="150px" /><asp:RequiredFieldValidator
                                ErrorMessage="Monto Forward" ControlToValidate="txtMonto" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Precio Transacci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtPrecio" CssClass="Numbox-7" Width="150px" /><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" ErrorMessage="Precio Transacci&oacute;n" ControlToValidate="txtPrecio"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Vencimiento</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaVencimiento" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Plazo Vencimiento</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtPlazoVencimiento" Width="150px" /><asp:RequiredFieldValidator
                                ErrorMessage="Plazo Vencimiento" ControlToValidate="txtPlazoVencimiento" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Modalidad</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlModalidad" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Cambio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtTipoCambio" Width="150px" /><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" ErrorMessage="Tipo de Cambio" ControlToValidate="txtTipoCambio"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Indicador Caja</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlIndicadorCaja" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Plaza de Negociaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtPlaza" Width="150px" /><asp:RequiredFieldValidator
                                ErrorMessage="Plaza de Negociaci&oacute;n" ControlToValidate="txtPlaza" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-sm-6">
            </div>
            <div class="col-sm-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="txtConsecutivo" />
    <asp:HiddenField runat="server" ID="txtPortafolio" />
    <asp:HiddenField runat="server" ID="txtFechaOperacion" />
    <asp:ValidationSummary runat="server" ID="vsPagina" ShowMessageBox="true" ShowSummary="false" />
    </form>
</body>
</html>
