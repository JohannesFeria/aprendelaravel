<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteValidacion.aspx.vb" Inherits="Modulos_Riesgos_frmReporteValidacion" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head runat="server">
    <title>Reporte de Validación</title>
</head>
<body>    
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><h2>Reporte de Validaci&oacute;n de Riesgos</h2></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha Operaci&oacute;n</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>                    
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlValidacion" runat="server" Width="120px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlValidacion" InitialValue="0" runat="server" ErrorMessage="Seleccione Tipo"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align:left;">                      
                            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir"/>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
    </div>
    </form>
</body>
</html>
