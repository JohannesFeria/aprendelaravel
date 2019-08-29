<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPatrimonioEmisor.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmPatrimonioEmisor" %>

<!DOCTYPE html>

<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function showModalEmisor() {
            $('#hdTipoModal').val('EMI');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Patrimonio Emisor</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Emisor</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbEmisor" Width="80px" ReadOnly="true" />
                                <asp:LinkButton ID="lbkModalEmisor" OnClientClick="return showModalEmisor()" runat="server">
                                    <span runat="server" id="imbEmisor" class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbEmisorDesc" Width="220px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Valor
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="txtValor" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tipo Valor</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlTipoValor" Width="200px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnCancelar" />
            <asp:HiddenField ID="hdTipoModal" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
