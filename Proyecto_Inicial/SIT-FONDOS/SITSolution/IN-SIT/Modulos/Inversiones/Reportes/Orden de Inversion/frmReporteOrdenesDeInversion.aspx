<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteOrdenesDeInversion.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Orden_de_Inversion_frmReporteOrdenesDeInversion" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de Operaciones Ejecutadas</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
    <div class="container-fluid">
    <header><h2>Reporte de Operaciones Ejecutadas</h2></header>
    <fieldset>
    <legend>Filtros</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Inicio</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Fecha de Inicio" ControlToValidate="tbFechaInicio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Fin</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Fecha de Fin" ControlToValidate="tbFechaFin">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <fieldset>
    <legend>Selección del Reporte</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9">
                    <asp:radiobuttonlist id="rbtReporte" runat="server" Width="360px" CssClass="stlCajaTexto" RepeatColumns="2" AutoPostBack="True">
						<asp:ListItem Value="Renta Fija">Renta Fija</asp:ListItem>
						<asp:ListItem Value="Renta Variable">Renta Variable</asp:ListItem>
						<asp:ListItem Value="Divisas">Divisas</asp:ListItem>
						<%--<asp:ListItem Value="Por gestor">Por gestor</asp:ListItem>
						<asp:ListItem Value="Por Correlativo">Por Correlativo</asp:ListItem>
						<asp:ListItem Value="Cash Call">Cash Call</asp:ListItem>--%>
					</asp:radiobuttonlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align:right;">
        <asp:Button ID="ibVerFirmas" runat="server" Text="Visualizar Firmas" Visible="false" />
        <asp:Button ID="btnExportar" runat="server" Text="Exportar" Visible="False"/>
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir"  CausesValidation="False" />
    </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
