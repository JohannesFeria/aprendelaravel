<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLimiteIntermediario.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmLimiteIntermediario" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>L&iacute;mites de Intermediario Negociaci&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        L&iacute;mites de Intermediario Negociaci&oacute;n</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Intermediario</label>
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlTercero" Width="250px" />
                            <asp:RequiredFieldValidator ErrorMessage="Intermediario" ControlToValidate="ddlTercero"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            % L&iacute;mite</label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="tbLimite" Width="120px"  CssClass="Numbox-7_12"/>
                            <asp:RequiredFieldValidator ErrorMessage="% L&iacute;mite" ControlToValidate="tbLimite"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacion"
                                runat="server" >(*)</asp:RequiredFieldValidator>
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
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdCodigo" />
    </form>    
</body>
</html>
