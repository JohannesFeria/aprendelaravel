<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmEvidenciaLoteClaseInstrumento.aspx.vb" Inherits="Modulos_Contabilidad_Reportes_frmEvidenciaLoteClaseInstrumento" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Resumen Lotes por Clase Instrumento</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Resumen Lotes por Clase Instrumento</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fondo</label>
                        <div class="col-sm-7"><asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true" /></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Fecha" ControlToValidate="tbFechaInicio" runat="server" />
                        </div>
                    </div>
                </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6"><asp:Label Text="" runat="server" ID="lblError" /></div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnVista" />
                <asp:Button Text="Salir" runat="server" ID="btnTerminar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <br />
    <asp:Literal Text="" runat="server" ID="ltrLog" />
    <asp:ValidationSummary ID="vsResumenError" runat="server" ShowMessageBox="True" ShowSummary="False"
    HeaderText="Los siguientes campos son obligatorios:"></asp:ValidationSummary>
    </form>
</body>
</html>