<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoPrecios.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmIngresoPrecios" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Ingreso de Precios</title>
    <script language="javascript" type="text/javascript">
        function ShowModalPopupNemonico() {
            var isin = document.getElementById('txtISIN').value;
            var sbs = document.getElementById('txtSBS').value;
            var nemonico = document.getElementById('txtMnemonico').value;
            return showModalDialog('../Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + nemonico, '800', '600', ''); 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Ingreso de Precios</h2></header>
    <fieldset>
    <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">C&oacutedigo Mnem&oacutenico</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" CssClass="input-medium" />
                                <asp:LinkButton ID="btnbuscar" runat="server" OnClientClick="return ShowModalPopupNemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:RequiredFieldValidator ID="rfvNemonico" ErrorMessage="Código Nemónico" ControlToValidate="txtMnemonico"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">C&oacutedigo SBS</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtSBS" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Código SBS" ControlToValidate="txtSBS"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                        </div>
                    </div>
                </div>
            </div>
           <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">C&oacutedigo ISIN</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtISIN" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Código ISIN" ControlToValidate="txtISIN"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Fecha Operación" ControlToValidate="tbFechaOperacion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Precio Limpio</label>
                            <div class="col-sm-9">                                
                                <asp:TextBox runat="server" ID="txtPrecioT_1" CssClass="Numbox-7" />                                                                    
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Precio Limpio" ControlToValidate="txtPrecioT_1"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                            </div>
                    </div>
               </div>
               <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Precio Sucio</label>
                            <div class="col-sm-9">      
                                <asp:TextBox runat="server" ID="txtPrecioSucio" CssClass="Numbox-7" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Precio Sucio" ControlToValidate="txtPrecioSucio"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                            </div>
                    </div>
               </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Porc. Precio Limpio</label>
                            <div class="col-sm-9">                                
                                <asp:TextBox runat="server" ID="txtPorcPrecioLimpio" CssClass="Numbox-7" />                                                                    
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Porc. Precio Limpio" ControlToValidate="txtPorcPrecioLimpio"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                            </div>
                    </div>
               </div>
               <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Porc. Precio Sucio</label>
                            <div class="col-sm-9">      
                                <asp:TextBox runat="server" ID="txtPorcPrecioSucio" CssClass="Numbox-7" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="Porc. Precio Sucio" ControlToValidate="txtPorcPrecioSucio"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                            </div>
                    </div>
               </div>
            </div>
            <div class="row">
               <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fuente Vec. Prec.</label>
                        <div class="col-sm-9">                                
                            <asp:DropDownList ID="ddlFuenteVecPre" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Fuente Vector Precio" ControlToValidate="ddlFuenteVecPre"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgIngresoPrecios" />
                        </div>
                    </div>
               </div>
               <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"></label>
                        <div class="col-sm-9">
                        </div>
                    </div>
               </div>
            </div>
        </fieldset>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="vgIngresoPrecios" />
            <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="false" />
        </div>
        <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
            HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgIngresoPrecios" />
    </div>
    </form>
</body>
</html>