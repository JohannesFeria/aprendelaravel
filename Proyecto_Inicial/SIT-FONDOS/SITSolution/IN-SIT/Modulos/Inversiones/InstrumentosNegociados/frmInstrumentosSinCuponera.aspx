<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInstrumentosSinCuponera.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmInstrumentosSinCuponera" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Confirmación de Instrumentos sin cuponera</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Código Nemónico</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtCodigoNemonico" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Código SBS</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtCodigoSBS" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Código ISIN</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtCodigoIsin" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Operación</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaOperacion" Width="100px" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Liquidación</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox ID="txtFechaLiquidacion" runat="server" SkinID="Date"></asp:TextBox>
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox ID="txtFechaVencimiento" runat="server" SkinID="Date"></asp:TextBox>
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Número de Unidades</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtNumeroUnidades" runat="server" CssClass="Numbox-7"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Monto Operación</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtMontoOperacion" runat="server" CssClass="Numbox-7"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header></header>
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row" style="text-align: right;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
