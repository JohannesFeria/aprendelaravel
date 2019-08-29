<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportesComposicionCartera.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmReportesComposicionCartera" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reportes Composicion De Cartera</title>
    <script type="text/javascript">
        var strMensajeError = "";
        var strError = "";

        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%=tbFechaInicio.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha de Inicio\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }

            return true;
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

    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Reportes IDI
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
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="144px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha Interfase</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
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
                            Ruta</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="Myfile" runat="server" Width="680px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Selección de Reporte</legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:RadioButtonList ID="RbReportes" runat="server" Width="100%" AutoPostBack="True">
                                <asp:ListItem Value="A3A">Anexo III - A (03)</asp:ListItem>
                                <asp:ListItem Value="A3B">Anexo III - B (04)</asp:ListItem>
                                <asp:ListItem Value="A6">Anexo VI (08)</asp:ListItem>
                                <asp:ListItem Value="A7">Anexo VII (09)</asp:ListItem>
                                <asp:ListItem Value="A8">Anexo VIII (10)</asp:ListItem>
                                <asp:ListItem Value="A9">Anexo IX (11)</asp:ListItem>
                                <asp:ListItem>Todos</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="ibVista" runat="server" Text="Imprimir" style="height: 26px" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <asp:Label ID="msgError1" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError2" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError3" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError4" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError5" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError6" runat="server" Visible="False"></asp:Label>
        </div>
        <div class="grilla" id="divDetalle" runat="server">
            <asp:GridView runat="server" SkinID="Grid" ID="dgNoExiste">
                <Columns>
                    <asp:BoundField DataField="CodigoISIN" HeaderText="Código ISIN"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="txtValidacion" runat="server" />
    <asp:HiddenField ID="txtReprocesar" runat="server" Value="1" />
    </form>
</body>
</html>
