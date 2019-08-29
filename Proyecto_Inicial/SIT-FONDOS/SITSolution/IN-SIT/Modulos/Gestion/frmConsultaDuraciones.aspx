<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaDuraciones.aspx.vb"
    Inherits="Modulos_Gestion_frmConsultaDuraciones" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Consulta de Duraciones</title>
    <script type="text/javascript">
//        function showModal() {
//            var isin = $('#tbCodigoIsin').val();
//            var mnemonico = $('#tbCodigoMnemonico').val();
//            var sbs = $('#tbCodigoSBS').val();
//            window.showModalDialog('../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '', 'dialogHeight:550px;dialogWidth:789px;status:no;unadorned:yes;help:No');
//        }            
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Consulta de Duraciones</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaValoracion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigoIsin" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigoSBS" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Mnemonico
                        </label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width="150px" />
                                <asp:LinkButton ID="lkbMnemonico" runat="server">
                            <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="tbDescripcionInstrumento" Width="250px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnListar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="CodigoPortafolio" HeaderText="CodigoPortafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="Emisi&oacute;n" />
                    <asp:BoundField DataField="NumeroTitulo" HeaderText="N&uacute;mero T&iacute;tulo" />
                    <asp:BoundField DataField="Instrumento" HeaderText="Instrumento" />
                    <asp:BoundField DataField="Duracion" HeaderText="Duraci&oacute;n" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnpopup" />
        </div>
    </div>
    </form>
</body>
</html>
