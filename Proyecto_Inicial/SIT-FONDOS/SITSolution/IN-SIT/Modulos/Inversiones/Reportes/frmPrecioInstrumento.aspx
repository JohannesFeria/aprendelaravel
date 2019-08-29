<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPrecioInstrumento.aspx.vb" Inherits="Modulos_Inversiones_Reportes_frmPrecioInstrumento" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Precio de Instrumentos</title>
    <script type="text/javascript">
        function showModalNem() {
            $('#hdBusqueda').val("Nemonico");
            var isin = $('#txtCodIsin').val().trim(); var sbs = '';
            var mnemonico = $('#txtnemonico').val().trim();
            return showModalDialog('../../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Precio Instrumento</h2></div></div>
             <fieldset>
                <legend>Datos de Busqueda</legend>
                <asp:UpdatePanel ID="upControles" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Código Nemonico</label>
                                <div class="col-sm-8">
                                    <div class="input-append">
                                        <asp:TextBox runat="server" ID="txtnemonico"  MaxLength ="12" />
                                        <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModalNem()">
                                        <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Código ISIN</label>
                                <div class="col-sm-8">
                                    <div class="input-append">
                                        <asp:TextBox runat="server" ID="txtCodIsin"  MaxLength ="12"  />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID ="btnAgregar" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Inicial</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                    <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha de Final</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                    <span class="add-on" id="Span1"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>                
                <div class="col-md-2">
                    <div style="text-align: right;">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                    </div>
                </div>
            </div>
                <div class="row">
                    <div class="col-md-12" style="text-align: right;">
                        <asp:Button Text="Generar" runat="server" ID="btnAceptar" />
                        <asp:Button Text="Limpiar" runat="server" ID="btnRegresar" />
                    </div>
                </div>
                <br/>
                <div class="row">
                    <div class="col-md-12">
                        <div class="Grilla">
                            <asp:UpdatePanel ID="udpReporte" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnBorrar" runat="server" Text="Borrar" />
                                    <asp:ListBox ID="lbxValor" runat="server" Width="100%" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID ="btnAgregar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </fieldset>
        </header>
    </div>
    </form>
</body>
</html>