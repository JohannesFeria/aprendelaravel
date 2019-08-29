<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frm_ModificacionIsinNemonico.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frm_ModificacionIsinNemonico" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modificicacion de Emision</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <script type="text/javascript">
        function showModal_Nemonico() {
            var isin = $('#txtISIN').val();
            var mnemonico = $('#txtMnemonico').val();
            return showModalDialog('../../../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=&vMnemonico=' + mnemonico, '800', '600', '');
        }
        function limpiar_Nemonico(){
            $('#txtISIN').val('');
            $('#txtMnemonico').val('');
            return false;
        }        
    </script>
</head>
<body>
<form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">        
        <header><h2>Modificación de Emision</h2></header>
        <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <fieldset>
            <legend>Datos</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tipo de Actualización</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoAct" runat="server" AutoPostBack ="true"  >
                                <asp:ListItem Text ="Nemonico" Value = "1" ></asp:ListItem>
                                <asp:ListItem Text ="Isin" Value = "2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Código ISIN</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtISIN" Width="150px" MaxLength ="12" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" MaxLength ="100" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModal_Nemonico()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lbkLimpiarControl" OnClientClick="return limpiar_Nemonico()"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnGenera" runat="server" Enabled ="false" Text="Realizar Modificación" />
        </div>
        </ContentTemplate> 
        </asp:UpdatePanel>
    </div>
</form>
</body>
</html>