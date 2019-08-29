<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCodigoValorMantenimiento.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmCodigoValorMantenimiento" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mantenimiento Código Valor</title>
    <script type="text/javascript">
        function showModal() {
            $('#hdBusqueda').val("Emisor");
            return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');
        }
        function showModalNem() {
            $('#hdBusqueda').val("Nemonico");
            var isin = $('#txtCodIsin').val().trim();var sbs = '';
            var mnemonico = $('#txtnemonico').val().trim();
            return showModalDialog('../../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');             
        }
    </script>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal" >
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Mantenimiento Código Valor</h2></div>
            </div>
        </header>
        </div>
    <asp:UpdatePanel ID="upgeneral" runat="server">
    <ContentTemplate>
    <fieldset>
        <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código Nemonico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtnemonico"  MaxLength ="12" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModalNem()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Código ISIN</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodIsin"  MaxLength ="12"  />
                            </div>
                        </div>
                    </div>
                </div>
                </div>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Emisor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtEmisor" ReadOnly ="true" Width="350px"  />
                                <asp:LinkButton runat="server" ID="lkbShowModal" OnClientClick="return showModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Opcion</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlopcion" runat="server" Width ="200px" AutoPostBack ="true">
                                    <asp:ListItem Text ="Titulo Unico" Value = "0" Selected ="True" />
                                    <asp:ListItem Text ="Titulo con Isin" Value = "1" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" runat = "server">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo de Instrumento SMV</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlTipoInstumentoSMV" runat="server" Width = "350PX" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" runat = "server" id="rowTipoTasa">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Tipo Tasa</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddltipoTasa" runat="server">
                                    <asp:ListItem  Text="Nominal" Value = "1"  />
                                    <asp:ListItem Text="Efectiva" Value = "2"  />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código Valor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtcodigovalor"  />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                 <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Situacion</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlestado" runat="server" >
                                    <asp:ListItem Value="A">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:HiddenField ID="hdemisor" runat="server" />
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Regresar" runat="server" ID="btnRegresar" CausesValidation="false" />
            </div>
        </div>
        <asp:HiddenField ID="hdBusqueda" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>