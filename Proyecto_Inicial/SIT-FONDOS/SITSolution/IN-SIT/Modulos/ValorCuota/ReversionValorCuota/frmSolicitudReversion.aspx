<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSolicitudReversion.aspx.vb" Inherits="Modulos_Contabilidad_frmSolicitudReversion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
  <script type="text/javascript">
      window.alert = function (txt, title) {
          if (title === undefined) title = "Default Name";
          alertify.alert(txt);
      };
</script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Solicitud de Reversión</h2>
                </div>
            </div>
        </header>
        <div runat="server" id="divDetalle">
            <fieldset>
                <legend>Detalle</legend>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Fondo</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true" />
                                <asp:RequiredFieldValidator ID="rvfFondo" runat="server" ErrorMessage="Fondo <br>" ForeColor="Red"
                                    ControlToValidate="ddlFondo">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Área</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlArea" Width="150px" />
                                <asp:RequiredFieldValidator ID="rvfArea" runat="server" ErrorMessage="Cargo <br>" ForeColor="Red"
                                    ControlToValidate="ddlArea">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Fecha Inicial</label>
                            <div class="col-sm-8">
                                <div class="input-append">
                                    <div id="Div2" runat="server" class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaInicial" SkinID="Date" Width="100px" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>                                        
                                    </div>
                                </div>                                
                                          <asp:RequiredFieldValidator ID="rfvFechaInicial" runat="server" 
                                  ErrorMessage="Fecha Inicial <br>" ForeColor="Red" ControlToValidate="tbFechaInicial">(*)</asp:RequiredFieldValidator>             
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Fecha Final</label>
                            <div class="col-sm-8">
                                <div class="input-append">
                                    <div id="imgFechaContrato" runat="server" class="input-append">
                                        <asp:TextBox runat="server" ID="tbFechaFinal" SkinID="Date" Width="100px" ReadOnly="true"/>
                                        <span class="add-on"><i class="awe-calendar"></i></span>                                   
                                    </div>
                                </div>
                                <asp:RequiredFieldValidator ID="rvfFechaFinal" runat="server" 
                              ErrorMessage="Fecha Final <br>" ForeColor="Red" ControlToValidate="tbFechaFinal">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Motivo</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtMotivo" TextMode="MultiLine" Rows="6" Width="320px" Height="100px" MaxLength = "250"/>
                              <asp:RequiredFieldValidator ID="rfvMotivo" runat="server" 
                            ErrorMessage="Motivo <br>" ForeColor="Red" ControlToValidate="txtMotivo">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Responsable</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtResponsable" TextMode="MultiLine" Rows="6" Width="320px" Height="30px" MaxLength = "500"/>
                                 <asp:RequiredFieldValidator ID="rvfResponsable" runat="server" 
                            ErrorMessage="Responsable <br>" ForeColor="Red" ControlToValidate="txtResponsable">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Acción Temporal</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtAccionTemporal" TextMode="MultiLine" Rows="6" Width="320px" MaxLength = "500"/>
                                   <asp:RequiredFieldValidator ID="rvfAccionTemporal" runat="server" 
                            ErrorMessage="Acción Temporal <br>" ForeColor="Red" ControlToValidate="txtAccionTemporal">(*)</asp:RequiredFieldValidator>
                            
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Solución Definitiva Sugerida</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtSolucionDefinitivaSugerida" TextMode="MultiLine" Rows="6" Width="320px" MaxLength = "500"/>
                                 <asp:RequiredFieldValidator ID="rvfSolucionDefinitivaSugerida" runat="server" 
                            ErrorMessage="Solución Definitiva Sugerida <br>" ForeColor="Red" ControlToValidate="txtSolucionDefinitivaSugerida">(*)</asp:RequiredFieldValidator>
                            
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Afecta Valor Cuota</label>
                            <div class="col-sm-8">
                                <asp:RadioButtonList ID="RbAfectaValorCuota" runat="server" Width="100%" CssClass="stlCajaTexto">
                                    <asp:ListItem Value="S">Sí</asp:ListItem>
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                </asp:RadioButtonList>
                                 <asp:RequiredFieldValidator ID="rvfAfectaValorCuota" runat="server" 
                            ErrorMessage="Afecta Valor Cuota" ForeColor="Red" ControlToValidate="RbAfectaValorCuota">(*)</asp:RequiredFieldValidator>
                            
                            </div>
                        </div>
                    </div>	
                </div>
                <div class="row">
                        <div class="row" style="text-align: right; height: 70px; padding-top: 30px;">
                            <asp:Button Text="Agregar" runat="server" ID="btnAgregarDetalle" />&#32;
                        </div>
                </div>
            </fieldset>
            <br/>
            <br />
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" 
        HeaderText="Los siguientes campos son obligatorios: <br>" />
    </form>
</body>
</html>
