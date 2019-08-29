<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHipervalorizador.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmHipervalorizador" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>HiperValorizador</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-sm-6">
                    <h2>
                        HiperValorizador</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnem&oacute;nico
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblCodigoNemonico" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6" style="text-align: center">
                    <h4>
                        CARACTERISTICAS</h4>
                </div>
                <div class="col-sm-6" style="text-align: center">
                    <h4>
                        NEGOCIACI&Oacute;N</h4>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Principal</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPrincipal" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Compra</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaCompra" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Principal Ajustado</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPrincipalAjustado" ReadOnly="true" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Precio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPrecio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Rendimiento</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbRendimiento" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Rendimiento</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbRendimientoNegociacion" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base (d&iacute;as)</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbBaseDias" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Treasuries</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTreasuries" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base TIR</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbBaseTIR" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base Cup&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlBaseCupon" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            VAC Emisi&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbVACEmision" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tasa Cupón</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTasaCupon" Width="150px" AutoPostBack="True">
                                <asp:ListItem Value="1">Nominal</asp:ListItem>
                                <asp:ListItem Value="2">Efectiva</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            VAC Actual</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbVACActual" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Para TIR</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlParaTIR" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Ultimo Cup&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblUltimoCupon" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valores Ajustados</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlValoresAjustados" Width="150px" AutoPostBack="True">
                                <asp:ListItem Value="1">Con Valor Ajustado</asp:ListItem>
                                <asp:ListItem Value="2">Sin Valor Ajustado</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Siguiente Cup&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblSiguienteCupon" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Emisi&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblFechaEmision" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="Evento" HeaderText="Evento" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Cupon" HeaderText="Cup&#243;n" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="TIR" HeaderText="TIR(C)" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="VA" HeaderText="VA (C)" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="Dias" HeaderText="D&#237;as" DataFormatString="{0:#,##0}" />
                    <asp:BoundField DataField="Amortiza" HeaderText="Amortiza" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="AmortAcum" HeaderText="Amort. Acum" DataFormatString="{0:#,##0.00}" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor Actual</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbValorActual" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            + Intereses</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbMasIntereses" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor Total</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbValorTotal" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <asp:Button Text="Imprimir" runat="server" ID="btnExportar" />
                    <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
                </div>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField runat="server" ID="hdnTipoCuponera" />
    <asp:HiddenField runat="server" ID="hdnUltimoCuponValor" />
    <asp:HiddenField runat="server" ID="hdnSiguienteCuponValor" />
    </form>
</body>
</html>
