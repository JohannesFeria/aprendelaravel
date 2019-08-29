\<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfaseBloomberg.aspx.vb"
    Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmInterfaseBloomberg" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>InterfaseBloomberg</title>
    <script type="text/javascript">

        var strMensajeError = "";
        var strError = "";

        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%=ddlInterface.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Interfase\n"

            if (document.getElementById("<%=tbFechaInterface.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Interfase\n"
            if (document.getElementById("<%=Myfile.ClientID %>").value == "")
                strMsjCampOblig += "\t-Ruta\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }


        function Validar() {
            strMensajeError = "";
            if (ValidaCamposObligatorios()) {
                return true;
            }
            else {
                alert(strMensajeError);
                return false;
            }
        }

        function Confirmacion() {
            var control;
            control = document.getElementById("<%= txtValidacion.ClientID %>");
            var Flag = control.value;
            if (Flag == '1') {
                var conf = window.confirm('¿El archivo ya ha sido cargado. Desea cargarlo nuevamente ?');
                if (conf == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Interfase Bloomberg
            </h2>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha Interfase</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInterface" SkinID="Date" />
                                <span class="add-on" id="imgFechaVAX"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Interfase</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlInterface" runat="server" Width="200px" AutoPostBack="True">
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
                        <div class="col-sm-9">
                            <asp:TextBox ID="Myfile" runat="server" Width="680px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="ibCargar" runat="server" Text="Procesar" />
            <asp:Button ID="ibSalir" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <asp:Label ID="msgError" runat="server" Visible="False"></asp:Label>
        </div>
        <div class="grilla" id="divDetalle" runat="server">
            <asp:GridView runat="server" SkinID="Grid" ID="dgNoExiste">
                <Columns>
                    <asp:BoundField DataField="CodigoISIN" HeaderText="Código ISIN"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:GridView runat="server" SkinID="Grid" ID="dgNoRegistrados">
                <Columns>
                    <asp:BoundField DataField="CodigoSBS" HeaderText="Código SBS"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="txtValidacion" runat="server" />
    </form>
</body>
</html>
