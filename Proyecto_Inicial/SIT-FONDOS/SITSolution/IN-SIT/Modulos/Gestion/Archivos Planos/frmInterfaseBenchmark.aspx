<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfaseBenchmark.aspx.vb"
    Inherits="Modulos_Gestion_Archivos_Planos_frmInterfaseBenchmark" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>InterfaseBenchmark</title>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function iptRuta_onclick() {

        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Importar Benchmark Indicadores
            </h2>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlFondo" runat="server" Width="130px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Ruta</label>
                        <div class="col-sm-7">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                                class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                                data-size="sm" size="150" onclick="return iptRuta_onclick()">
                                <%--accept=".xls,.xlsx"--%>
                            <%--onclick="return iptRuta_onclick()"--%>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <asp:Label ID="msgError" runat="server" Visible="False"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
