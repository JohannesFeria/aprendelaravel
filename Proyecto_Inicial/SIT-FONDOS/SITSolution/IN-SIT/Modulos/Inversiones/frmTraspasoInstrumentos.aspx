<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTraspasoInstrumentos.aspx.vb" Inherits="Modulos_Inversiones_frmTraspasoInstrumentos" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Traspaso de Instrumentos</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Traspaso de Instrumentos</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Clase de Instrumento</label>
                        <div class="col-sm-8">
                            <asp:dropdownlist id="ddlTipoOrden" runat="server" Width="260px" ></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label"  Visible="False" >
                            <asp:Label ID="lblOperacion" runat="server" Text="Operación" Visible ="false" />Fecha Operacion
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="150px" Visible="False" />
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio Origen</label>
                        <div class="col-sm-8">
                            <asp:dropdownlist id="ddlFondoOrigen" runat="server" Width="150px" ></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label"  Visible="False" >Fecha Destino</label>
                        <div class="col-sm-7">
                            <asp:dropdownlist id="ddlFondoDestino" runat="server" Width="150px" ></asp:dropdownlist>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Mnemónico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbMnemonico" />
                                <asp:LinkButton ID="ibBuscar" runat="server" ><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Código ISIN</label>
                        <div class="col-sm-7">
                            <asp:textbox id="tbISIN" runat="server" Width="120px" MaxLength="12" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código SBS</label>
                        <div class="col-sm-8">
                            <asp:textbox id="tbSBS" runat="server" Width="120px" MaxLength="12" ForeColor="Black" />
						    <INPUT id="hdCustodio" style="WIDTH: 1px; HEIGHT: 3px" type="hidden" name="hdCustodio" runat="server">
                            <INPUT id="hdSaldo" style="WIDTH: 1px; HEIGHT: 3px" type="hidden" name="hdSaldo" runat="server">
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Moneda</label>
                        <div class="col-sm-7">
                            <asp:textbox id="lblMoneda" ReadOnly="true"  runat="server" Width="120px" MaxLength="12" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <INPUT id="txtFechaPortafolioOrigen"  type="hidden" size="12" name="txtFechaPortafolioOrigen" runat="server">
        <INPUT id="txtFechaPortafolioDestino" type="hidden" size="12" name="txtFechaPortafolioDestino" runat="server">
        <br />
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="ibAceptar" CausesValidation="false" />
                <asp:Button Text="Salir" runat="server" ID="ibSalir" CausesValidation="false" />
            </div>
            <asp:Button ID="btnpopup" runat="server" Text=""  CssClass ="hidden" />
        </div>
    </div>
    </form>
</body>
</html>
