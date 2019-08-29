<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmGeneracionEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Cálculo de Encaje</title>
    <script type="text/javascript">
        function ShowModal() {
            var vPorta = $('#ddlPortafolio').val();
            var vFecha = $('#txtFechaProceso').val();
            //alert(vPorta);
            //alert(vFecha);
            return showModalDialog('frmVisorErrorEncaje.aspx?pportafolio=' + vPorta + '&pFecha=' + vFecha, '800', '600', '');                
        }
        function Confirmacion() {
            if (confirm('¿Está seguro de generar el encaje?')==false){
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Cálculo de Encaje</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div  class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="136px" AutoPostBack="True"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Proceso</label>
                <div  class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaProceso" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Fecha de Proceso" ControlToValidate="txtFechaProceso">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 contaol-label"></label>
                <div class="col-sm-9">
                    <asp:RadioButtonList id="rblOpcion" runat="server" Width="330px" 
                        AutoPostBack="True">
					<asp:ListItem Value="0" Selected="True">Encaje Requerido</asp:ListItem>
					<asp:ListItem Value="1">Rentabilidad Encaje</asp:ListItem>
				</asp:RadioButtonList>
                </div>                
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnProcesar" runat="server" text="Procesar" OnClientClick="return Confirmacion();" />
        <asp:Button ID="btnSalir" runat="server" text="Salir" CausesValidation="false" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
