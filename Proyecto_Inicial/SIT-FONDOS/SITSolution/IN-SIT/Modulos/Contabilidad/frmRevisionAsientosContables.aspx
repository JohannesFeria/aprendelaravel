<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRevisionAsientosContables.aspx.vb"
    Inherits="Modulos_Contabilidad_frmRevisionAsientosContables" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Revisión de Asientos Contables</title>
    <script type="text/javascript">
        var strMensajeError = "";
        var strError = "";

        function ValidaFechas() {
            var strMsjFechas = "";

            if (document.getElementById("<%= txtFechaOperacion.ClientID %>").value == "") {
                strMsjFechas += "-Fecha Control:" + strError
            }

            if (strMsjFechas != "") {
                strMensajeError += "Formato de Fecha Incorrecto: DD/MM/YYYY\n" + strMsjFechas + "\n";
                return false;
            }
            else {
                return true;
            }
        }

        function ValidaCamposCabecera() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%= txtFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha\n"

            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Portafolio\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }

        function ValidaCamposDetalle() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Moneda\n"
            if (document.getElementById("<%= ddlDebeHaber.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Debe/Haber\n"
            if (document.getElementById("<%= txtGlosa.ClientID %>").value == "")
                strMsjCampOblig += "\t-Glosa\n"
            if (document.getElementById("<%= txtImporteSoles.ClientID %>").value == "")
                strMsjCampOblig += "\t-Importe Soles\n"
            if (document.getElementById("<%= txtImporteOrigen.ClientID %>").value == "")
                strMsjCampOblig += "\t-Importe Origen\n"
            if (document.getElementById("<%= txtNroCuenta.ClientID %>").value == "")
                strMsjCampOblig += "\t-Glosa\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }

        function ValidarCabecera() {
            strMensajeError = "";
            if (ValidaCamposCabecera() && ValidaFechas()) {
                return true;
            }
            else {
                alertify.alert('<b>' + HttpUtility.HtmlEncode(strMensajeError) + '</b>');
                return false;
            }
        }

        function ValidarDetalle() {
            strMensajeError = "";
            if (ValidaCamposDetalle()) {
                return true;
            }
            else {
                alertify.alert('<b>' + HttpUtility.HtmlEncode(strMensajeError) + '</b>');
                return false;
            }
        }

        function ConfirmarCancelar() {
            return confirm('Seguro de cancelar el ingreso del detalle?');
        }

        function ActualizaImporte() {
            if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "NSOL") {
                document.getElementById("<%= txtImporteSoles.ClientID %>").value = document.getElementById("<%= txtImporteOrigen.ClientID %>").value
            }
        }

        function showCuentaContable(numCuenta, portafolio) {
            document.getElementById('hdModal').value = "1";
            return showModalDialog('frmPopupCuentaContable.aspx?vIsin=' + numCuenta + '&vPortafolio=' + portafolio, '800', '600', '');
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
                        Asientos Contables
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondo" runat="server" Width="125px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaOperacion" SkinID="Date" />
                                <span class="add-on" id="imgCalendar" runat="server"><i class="awe-calendar"></i>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Lote
                        </label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddlTipoLote" runat="server" Width="255px" AutoPostBack="True">
                                <asp:ListItem Value="I">Compra Venta de Inversiones</asp:ListItem>
                                <asp:ListItem Value="V">Valorización de la Cartera</asp:ListItem>
                                <asp:ListItem Value="T">Cobranza y Cancelación de la Inversión</asp:ListItem>
                                <%--<asp:ListItem Value="P">PROVISIÓN DE POLIZAS AGENTES DE BOLSA</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="ibListar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla" style="height: 270px;" id="divBusqueda" runat="server">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField HeaderText="Modificar">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgCheck" CommandName="Modificar"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS"
                        HeaderText="CodigoPortafolioSBS"></asp:BoundField>
                    <asp:BoundField DataField="NumeroAsiento" HeaderText="Número Asiento"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Secuencia"
                        HeaderText="Secuencia"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="FechaAsiento"
                        HeaderText="Fecha Asiento"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMatrizContable"
                        HeaderText="CodigoMatrizContable"></asp:BoundField>
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda"></asp:BoundField>
                    <asp:BoundField DataField="Glosa" HeaderText="Glosa"></asp:BoundField>
                    <asp:BoundField DataField="DebeHaber" HeaderText="Debe/Haber"></asp:BoundField>
                    <asp:BoundField DataField="CuentaContable" HeaderText="Número de Cuenta"></asp:BoundField>
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}"
                        HtmlEncodeFormatString="false"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Total Debe</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtTobDeb" runat="server" Width="150px" MaxLength="10" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Total Haber</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtTotHab" runat="server" Width="150px" MaxLength="10" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Total Diferencia</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtTotDif" runat="server" Width="150px" MaxLength="10" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <fieldset>
            <legend>Selección de Lote</legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Matriz</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlMatriz" runat="server" Width="162px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Número Asiento</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlNumeroAsiento" TabIndex="1" runat="server" Width="115px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Debe/Haber</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlDebeHaber" TabIndex="2" runat="server" Width="130px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Nro.Cuenta Contable</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox ID="txtNroCuenta" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                                <asp:LinkButton Text="" ID="imbNroCuenta" runat="server">
                                    <span class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="col-sm-12 control-label">
                            <asp:Label ID="lblNroCuentaContable" runat="server" Width="100px"></asp:Label></label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Glosa</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtGlosa" TabIndex="5" runat="server" Width="500px" CssClass="stlCajaTexto"
                                MaxLength="200"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Importe</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtImporteOrigen" onblur="ActualizaImporte();" runat="server" Width="100px"
                                CssClass="stlCajaTexto"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda Origen</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMoneda" runat="server" Width="130px" CssClass="stlCajaTexto">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Importe (soles)</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtImporteSoles" TabIndex="8" runat="server" Width="100px" CssClass="stlCajaTexto"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button Text="Calcular" runat="server" ID="ibCalcular" />
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="ibIngresar" />
            <asp:Button Text="Imprimir" runat="server" ID="ibImprimir" />
            <asp:Button Text="Aceptar" runat="server" ID="ibAceptar" />
            <asp:Button Text="Salir" runat="server" ID="ibSalir" />
        </div>
    </div>
    <asp:HiddenField ID="hdProcesar" runat="server" />
    <asp:HiddenField ID="hdModal" runat="server" />
    <asp:Button ID="btModal" runat="server" />
    </form>
</body>
</html>
