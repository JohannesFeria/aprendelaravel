<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionCuponeraEspecial.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmGeneracionCuponeraEspecial" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Cuponera Especial</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2> 
                        Cuponera Especial</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            C&oacute;digo Mnem&oacute;nico
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigoNemonico" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            C&oacute;digo Isin
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigoIsin" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Emisi&oacute;n</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaEmision" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimiento" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Primer Cup&oacute;n</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaPrimer" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Cupones</label>
                        <div class="col-sm-7">
                            De
                            <asp:TextBox runat="server" ID="tbDe" Width="40px" />
                            <asp:RequiredFieldValidator ErrorMessage="Rango De" ControlToValidate="tbDe" runat="server" />&nbsp;A
                            <asp:TextBox runat="server" ID="tbA" Width="40px" />
                            <asp:RequiredFieldValidator ErrorMessage="Rango A" ControlToValidate="tbA" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Periodicidad</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlPeriodicidad" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Periodicidad" ControlToValidate="ddlPeriodicidad"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Cup&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbTasaEncaje" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tasa Cup&oacute;n" ControlToValidate="tbTasaEncaje"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Spread</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbTasaSpread" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tasa Spread" ControlToValidate="tbTasaSpread"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-2" style="text-align: right;">
                    <asp:Button Text="Generar Cuponera" runat="server" ID="btnGenerarCuponera" CausesValidation="true" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "consecutivo") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <a onclick="return Confirmar()" href="javascript:;">
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "consecutivo") %>'
                                    OnCommand="Eliminar"></asp:ImageButton>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="consecutivo" HeaderText="Nro. Cup&#243;n" />
                    <asp:BoundField DataField="FechaIni" HeaderText="Fecha Inicio" />
                    <asp:BoundField DataField="FechaFin" HeaderText="Fecha Termino" />
                    <asp:BoundField DataField="Amortizac" HeaderText="Amortizaci&oacute;n" />
                    <asp:BoundField DataField="DifDias" HeaderText="Dif. Dias" />
                    <asp:BoundField DataField="TasaCupon" HeaderText="Tasa Cup&#243;n" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="BaseCupon" HeaderText="Base" DataFormatString="{0:0}" />
                    <asp:BoundField DataField="DiasPago" HeaderText="Dias Pago" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset runat="server" id="pnlCupon">
            <legend>Modificar Cup&oacute;n</legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Inicio
                        </label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" AutoPostBack="true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Termino
                        </label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaTermino" SkinID="Date" AutoPostBack="true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Amortizaci&oacute;n
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbAmortizacion" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Dif. D&iacute;as
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDifDias" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Cup&oacute;n
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbTasaCupon" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Base
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbBase" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            D&iacute;as Pago
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDiasPago" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                </div>
                <div class="col-sm-4">
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Modificar" runat="server" ID="btnAgregar" CausesValidation="false" />
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" CausesValidation="false" />
                <asp:Button Text="Salir" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hdConsecutivo" type="hidden" name="hdConsecutivo" runat="server">
    <input id="hdFechaIni" type="hidden" name="hdFechaIni" runat="server">
    <input id="hdFechaFin" type="hidden" name="hdFechaFin" runat="server">
    <input id="hdAmortVenc" type="hidden" name="hdAmortVenc" runat="server">
    <input id="hdBase" type="hidden" name="hdBase" runat="server">
    <input id="hdAmort" type="hidden" name="hdAmort" runat="server">
    <input id="hdFechaInicial" type="hidden" name="hdFechaInicial" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
