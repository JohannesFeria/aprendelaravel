<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPlanDeCuentas.aspx.vb" Inherits="Modulos_Contabilidad_frmPlanDeCuentas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Firma de Documentos</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Plan de Cuentas
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <%--<div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Periodo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPeriodo" runat="server" AutoPostBack="True" 
                                Width="130px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondo" runat="server" AutoPostBack="True" 
                                Width="130px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="lblNombreFondo" runat="server" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla" style="height: 270px;">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="CuentaContable" Visible="True" ItemStyle-HorizontalAlign="Left"
                        HeaderText="Cuenta SBS"></asp:BoundField>
                    <asp:BoundField DataField="CodigoContablePU" Visible="True" ItemStyle-HorizontalAlign="Left"
                        HeaderText="Cuenta SUCAVE"></asp:BoundField>
                    <asp:BoundField DataField="DescripcionCuenta" ItemStyle-HorizontalAlign="Left" HeaderText="Descripción">
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Imprimir" runat="server" ID="ibImprimir" />
            <asp:Button Text="Salir" runat="server" ID="ibSalir" />
        </div>
        <asp:HiddenField ID="tbCuentaContable" runat="server" />
        <asp:HiddenField ID="tbDescripcionCuentaContable" runat="server" />
    </div>
    </form>
</body>
</html>

