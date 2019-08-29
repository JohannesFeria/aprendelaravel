<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportePromedioTasas.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmReportePromedioTasas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Promedio de tasas</title>
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
                        Promedio de tasas
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label id="Label1" runat="server" class="col-sm-4 control-label">
                            Año</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlAnio" runat="server" Width="150px" />
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
    <asp:HiddenField runat="server" ID="hiContD" />
    <asp:HiddenField runat="server" ID="hiContS" />
    </form>
</body>
</html>
