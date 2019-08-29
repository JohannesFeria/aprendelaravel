<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPagosDetalle.aspx.vb"
    Inherits="Modulos_PrevisionPagos_frmPagosDetalle" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento Pago</title>
</head>
<body>

    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                Mantenimiento Pago</h2>
        </header>
        
            <fieldset>
                <legend></legend>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Fecha Pago</label>
                            <div class="col-md-9">
                                <div class="input-append">
                                    <div class="input-append date">
                                        <div class="col-md-8">
                                            <asp:UpdatePanel ID="up" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:TextBox runat="server" ID="txtFechaPago" SkinID="Date" AutoPostBack="true" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-2">
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                <span style="font-weight: bold; color: Red">Hora Cierre
                                    <asp:Label ID="lblHoraCierre" runat="server" Text="Label"></asp:Label></span></label>
                            <div class="col-md-9">
                                <span style="font-weight: bold; color: Red"></span>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Tipo Operaci&oacute;n</label>
                            <div class="col-md-9">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="280px" CssClass="stlCajaTexto"></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Moneda</label>
                            <div class="col-md-9">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlMoneda" runat="server" Width="280px" CssClass="stlCajaTexto"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Importe</label>
                            <div class="col-md-9">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtImporte" runat="server" Width="190px" MaxLength="25"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Cuenta Origen</label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlCtaOrigen" runat="server" Width="280px" CssClass="stlCajaTexto"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Banco Origen</label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlBancoOrigen" runat="server" Width="280px" CssClass="stlCajaTexto">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Cuenta Destino</label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlCtaDestino" runat="server" Width="280px" CssClass="stlCajaTexto"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Banco Destino</label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlBancoDestino" runat="server" Width="280px" CssClass="stlCajaTexto">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        
        <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="divPanelInferior" runat="server">
                    <div class="Contenedor">
                        <fieldset>
                            <legend></legend>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Estado</label>
                                        <div class="col-md-9">
                                            <div class="input-append">
                                                <asp:DropDownList ID="ddlEstado" runat="server" Width="150px" CssClass="stlCajaTexto">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                        </label>
                                        <div class="col-md-9">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Usuario Provisi&oacute;n</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtUsuarioProvision" runat="server" Width="280px" ReadOnly="True"
                                                CssClass="stlCajaTexto"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Fecha Provisi&oacute;n</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtFechaProvision" runat="server" Width="280px" ReadOnly="True"
                                                CssClass="stlCajaTexto"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Usuario Aprobaci&oacute;n</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtUsuarioAprobacion" runat="server" Width="280px" ReadOnly="True"
                                                CssClass="stlCajaTexto"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Fecha Aprobaci&oacute;n</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtFechaAprobacion" runat="server" Width="280px" ReadOnly="True"
                                                CssClass="stlCajaTexto"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Usuario Anulaci&oacute;n</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtUsuarioAnulacion" runat="server" Width="280px" ReadOnly="True"
                                                CssClass="stlCajaTexto"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">
                                            Fecha Anulaci&oacute;n</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtFechaAnulacion" runat="server" Width="280px" ReadOnly="True"
                                                CssClass="stlCajaTexto"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGrabar" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-5">
                <div class="form-group" style="float: right;">
                    <asp:Button Text="Aceptar" runat="server" ID="btnGrabar" ValidationGroup="vgDetalle" />
                    <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="false" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
    </div>
    </form>
</body>
</html>
