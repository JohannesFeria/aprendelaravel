<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportesCalculoInteres.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmReportesCalculoInteres" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Calculo de Intereses</title>
    <script type="text/javascript">

        function Validar() {
            d = document.getElementById("ddlMes");

            if (d.value == "0") {
                alertify.alert("Seleccione un mes");
                return false;
            }
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Calculo de Intereses
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label id="lblFechaInicio" runat="server" class="col-sm-4 control-label">
                            Mes</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMes" runat="server" Width="150px">
                                <asp:ListItem Value="0">--Seleccionar--</asp:ListItem>
                                <asp:ListItem Value="01">Enero</asp:ListItem>
                                <asp:ListItem Value="02">Febrero</asp:ListItem>
                                <asp:ListItem Value="03">Marzo</asp:ListItem>
                                <asp:ListItem Value="04">Abril</asp:ListItem>
                                <asp:ListItem Value="05">Mayo</asp:ListItem>
                                <asp:ListItem Value="06">Junio</asp:ListItem>
                                <asp:ListItem Value="07">Julio</asp:ListItem>
                                <asp:ListItem Value="08">Agosto</asp:ListItem>
                                <asp:ListItem Value="09">Setiembre</asp:ListItem>
                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label id="Label1" runat="server" class="col-sm-4 control-label">
                            Año</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="dllAnio" runat="server" Width="150px" />
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
    <asp:HiddenField runat="server" ID="hiRow" />
    <asp:HiddenField runat="server" ID="hiRowDolares" />
    <asp:HiddenField runat="server" ID="hidFM" />
    <asp:HiddenField runat="server" ID="hidFMncuota" />
    <asp:HiddenField runat="server" ID="hidInteres1" />
    <asp:HiddenField runat="server" ID="hidInteres2" />
    <asp:HiddenField runat="server" ID="hidSumaD" />
    <asp:HiddenField runat="server" ID="hidSumaF" />
    <asp:HiddenField runat="server" ID="hidSumaH" />
    <asp:HiddenField runat="server" ID="hidSumaJ" />
    </form>
</body>
</html>
