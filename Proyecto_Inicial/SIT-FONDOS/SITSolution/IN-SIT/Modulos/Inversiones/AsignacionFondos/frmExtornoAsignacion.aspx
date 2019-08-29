<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoAsignacion.aspx.vb"
    Inherits="Modulos_Inversiones_AsignacionFondos_frmExtornoAsignacion" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Extorno de &Oacute;rdenes Asignadas</title>
    <script type="text/javascript">
        var strMensajeError = "";

        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%=tbMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "\t-Mnemonico\n"
            if (document.getElementById("<%=tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Reporte\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                alert(strMensajeError);
                return false;
            }
            {
                return true;
            }
        }

        function ValidarRegistro() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%=lblCodigoMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "\t-Debe seleccionar un Registro\n"

            if (strMsjCampOblig != "") {
                strMensajeError += strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }

        function ValidarSeleccion() {
            strMensajeError = "";
            if (ValidarRegistro()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Extorno de &Oacute;rdenes Asignadas</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Código ISIN</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="tbISIN" runat="server" MaxLength="12" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnemónico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbMnemonico" />
                                <asp:LinkButton runat="server" ID="ibAyuda"> <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha Operación</label>
                        <div class="col-sm-4">
                            <div class="input-append">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="120px" />
                                    <span id="img1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" align="right">
                    <asp:Button ID="ibBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionarPE" runat="server" CommandName="Seleccionar" SkinID="imgCheck" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="Instrumento"></asp:BoundField>
                    <asp:BoundField DataField="HO-FONDO1" HeaderText="# Orden de Inversi&#243;n F1">
                    </asp:BoundField>
                    <asp:BoundField DataField="HO-FONDO2" HeaderText="# Orden de Inversi&#243;n F2">
                    </asp:BoundField>
                    <asp:BoundField DataField="HO-FONDO3" HeaderText="# Orden de Inversi&#243;n F3">
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoPreOrden" HeaderText="CodigoPreorden"></asp:BoundField>
                    <asp:BoundField DataField="Monto" HeaderText="Monto"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend>Detalle de Extorno de Ordenes de Inversión</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código Mnemonico</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblCodigoMnemonico" ReadOnly="true" runat="server" MaxLength="12"
                                Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Nro. Orden HO-FONDO3</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lbFondo3" ReadOnly="true" runat="server" MaxLength="12" Width="96px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro.Orden HO-FONDO1</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblFondo1" ReadOnly="true" runat="server" MaxLength="12" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Codigo PreOrden</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblCodigoPreOrden" ReadOnly="true" runat="server" MaxLength="12"
                                Width="96px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro.Orden HO-FONDO2</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblFondo2" ReadOnly="true" runat="server" MaxLength="12" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-3">
                <asp:Button runat="server" ID="ibConsultar" Text="Consultar" />
            </div>
            <div class="col-md-9" style="text-align: right;">
                <asp:Button runat="server" ID="ibExtornar" Text="Extornar" />
                <asp:Button runat="server" ID="ibSalir" Text="Salir" />
            </div>
            <asp:Button ID="btnpopup" runat="server" Text="Button" CssClass="hidden" />
        </div>
    </div>
    </form>
</body>
</html>
