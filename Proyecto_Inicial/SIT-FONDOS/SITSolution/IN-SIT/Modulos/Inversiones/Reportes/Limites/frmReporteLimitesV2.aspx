<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteLimitesV2.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Limites_ReporteLimitesV2" %>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de Límites</title>
    <script type="text/javascript">
        function ShowPopup() {
            window.showModalDialog('frmVisorReporteLimitesV2.aspx?', '', 'dialogHeight:600px;dialogWidth:769px;status:no;unadorned:yes;help:No');
        }
        function PreguntarSiProcesamosNuevamente(msjPregunta) {
            if (confirm(msjPregunta))
                document.getElementById('hdProcesar').value = '1';
            else
                document.getElementById('hdProcesar').value = '0';
            document.forms["form1"].submit();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="Form-Horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<div class="Container-Fluid" >
    <header><h2>Reporte de Límites</h2></header>
    <br />
    <asp:UpdatePanel ID="Up" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="Form-group">        
            <div class="col-md-6">
            <label class="col-sm-3 control-label">Fecha Límite</label>
            <div class="col-sm-9">
                <div class="input-append date">
                    <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                    <span class="add-on"><i class="awe-calendar"></i></span>
                </div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="Fecha Límite" ControlToValidate="tbFechaInicio">(*)</asp:RequiredFieldValidator>
            </div>
        </div>
            <div class="col-md-6"></div>
        </div>
    </div>
    <div class="row">
        <div class="Form-group">
            <div class="col-md-6">
            <label class="col-sm-3 control-label">Reporte Límite</label>
            <div class="col-sm-9">
                <asp:DropDownList id="ddlReporteLimite" runat="server" AutoPostBack="True" Width="380px" />
            </div>
        </div>
            <div class="col-md-6"></div>
        </div>
    </div>
    <div class="row">
        <div class="Form-group">
            <div class="col-md-6">
            <asp:Label ID="lblLimiteCaracteristica" runat="server" CssClass="col-sm-3 control-label">Límite Portafolio</asp:Label>
            <div class="col-sm-9">
                <asp:DropDownList id="ddlLimiteCaracteristica" runat="server"  Width="250px"/>
            </div>
        </div>
            <div class="col-md-6"></div>
        </div>
    </div>
    <div class="row">
        <div class="Form-group">
            <div class="col-md-6">
            <label class="col-sm-3 control-label"></label>
            <div class="col-sm-9">
                <asp:CheckBox id="chkDetallePorFondo" runat="server" Text="Ver detalle por fondos" Visible="False"></asp:CheckBox>
            </div>
        </div>
            <div class="col-md-6"></div>
        </div>
    </div>
    <div class="row">
        <div class="Form-group">
            <div class="col-md-6">
            <label class="col-sm-3 control-label"></label>
            <div class="col-sm-9">
                <asp:radiobuttonlist id="rblEscenario" runat="server" CssClass="stlCajaTexto" RepeatDirection="Horizontal">
					<asp:ListItem Value="REAL" Selected="True">Reales</asp:ListItem>
					<asp:ListItem Value="ESTIMADO">Estimado</asp:ListItem>
				</asp:radiobuttonlist>
            </div>
        </div>
            <div class="col-md-6"></div>
        </div>
    </div>
    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="row">
        <div class="form-group">
            <div class="col-sm-6"></div>
            <div class="col-sm-6" style="text-align: right;">
                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" 
                    CausesValidation="False" />
                <asp:HiddenField ID="hdProcesar" runat="server" />
            </div>
        </div>
    </div>
    </div>

    </form>
</body>
</html>
