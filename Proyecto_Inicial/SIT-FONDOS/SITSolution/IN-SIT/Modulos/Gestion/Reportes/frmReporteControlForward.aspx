﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteControlForward.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmReporteControlForward" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Control de Forward</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Control de Forward</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
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
    </div>
    <asp:HiddenField runat="server" id="hidB" />
    <asp:HiddenField runat="server" id="hidFN" />
    <asp:HiddenField runat="server" id="hidF" />
    </form>
</body>
</html>
