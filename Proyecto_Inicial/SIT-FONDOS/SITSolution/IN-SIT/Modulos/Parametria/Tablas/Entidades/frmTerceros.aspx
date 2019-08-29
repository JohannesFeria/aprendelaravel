<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTerceros.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmTerceros" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<script src="../../../../App_Themes/js/inputmask/jquery.maskedinput.min.js" type="text/javascript"></script>
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../../App_Themes/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="../../../../App_Themes/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <%--<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function soloLetras(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = " 0123456789abcdefghijklmnñopqrstuvwxyz";
            especiales = [8, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 37, 39, 46];
            if (letras.indexOf(tecla) == -1)
                return false;
        }

        function limpia() {
            var val = document.getElementById("miInput").value;
            var tam = val.length;
            for (i = 0; i < tam; i++) {
                if (!isNaN(val[i]))
                    document.getElementById("miInput").value = '';
            }
        }

        $(function () {
            $('[id*=lbFondoVinculado]').multiselect({
                includeSelectAllOption: true
            });


//            $("#ddlTipoTercero").change(function () {
//                if ($(this).val() == "ENFI" && $("#ddlPais").val() == "604")
//                    $("#divFortalezaFinanciera").show();
//                else
//                    $("#divFortalezaFinanciera").hide();
//            });

            // $("#ddlTipoTercero").trigger("change");
            //$("#ddlPais").trigger("change");


        });



    </script>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <%--<asp:UpdatePanel ID="upTerceros" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>
            <div class="container-fluid">
                <header>
            <div class="row">
                <div class="col-md-6"><h2>Mantenimiento de Terceros</h2></div>
            </div>
        </header>
                <fieldset>
                    <legend>Datos&#32;Generales - Terceros</legend>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    C&oacute;digo Tercero</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCodigoTercero" MaxLength="11" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Codigo Tercero"
                                        ControlToValidate="txtCodigoTercero" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Sector Empresarial:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlSectorEmpresarial" Width="300px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Sector Empresarial"
                                        ControlToValidate="ddlSectorEmpresarial" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Descripci&oacute;n:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="tbDescripcion" MaxLength="64" Width="300px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Descripcion"
                                        ControlToValidate="tbDescripcion" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Tercero:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlTipoTercero" Width="300px" AutoPostBack="true" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Tipo Tercero"
                                        ControlToValidate="ddlTipoTercero" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Direcci&oacute;n:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="tbDireccion" MaxLength="64" Width="300px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Direccion"
                                        ControlToValidate="tbDireccion" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Clasificaci&oacute;n Tercero:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlClasificacionTercero" Width="300px" AutoPostBack="true" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="Clasificacion Tercero"
                                        ControlToValidate="ddlClasificacionTercero" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Documento:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlTipoDocumento" Width="300px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Tipo Documento"
                                        ControlToValidate="ddlTipoDocumento" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Custodio:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlCustodio" Width="300px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Código Documento:</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="tbCodigoDocumento" MaxLength="15" Width="120px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Sector GIGS</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlSectorGigs" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Pa&iacute;s:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlPais" Width="300px" AutoPostBack="true" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Pais" ControlToValidate="ddlPais"
                                        runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Contacto:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlBeneficiario" Width="300px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    C&oacute;digo Postal:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlCodigoPostal" Width="300px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Situaci&oacute;n:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Forward:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCodigoEmision" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Rating:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="cboRating" Width="100px" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- INI MPENAL - 21/09/16 -->
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:CheckBox ID="chkFondoVinculado" Text="Vinculado" runat="server" class="col-sm-4 control-label" AutoPostBack="true" />
                                <div class="col-sm-8">
                                    <asp:ListBox ID="lbFondoVinculado" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Rating Interno:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlRatingInterno" Width="100px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ErrorMessage="Rating Interno"
                                        ControlToValidate="ddlRatingInterno" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- INI MPENAL - 21/09/16 -->
                    <div id="divFortalezaFinanciera" class="row" runat="server" visible="false" >
                        <div class="col-md-6">
                            <div class="form-group">


                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fortaleza Financiera:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlRatingFF" Width="100px" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- FIN MPENAL - 21/09/16 -->
                    </label>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Detalle Cuentas Bancarias - Terceros</legend>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Banco</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlBanco" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ErrorMessage="Banco" ControlToValidate="ddlBanco"
                                        runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle1" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Liq. Autómatica</label>
                                <div class="col-sm-8">
                                    <asp:CheckBox ID="cbLiqAutomatica" runat="server" AutoPostBack="true" />
                                    <input type="hidden" runat="server" id="hdSecuencial" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Nro Cuenta:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtNumeroCuenta" MaxLength="20" Width="200px" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" ErrorMessage="Nro Cuenta" ControlToValidate="txtNumeroCuenta"
                                    runat="server" Text="(*)" ValidationExpression="^([A-Z]){3}$" ValidationGroup="vgDetalle1" />--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Nro Cuenta Interbancaria:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCuentaInterBancario" MaxLength="24" Width="200px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Mercado:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlMercadoTercero" Width="200px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Portafolio :</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" ErrorMessage="Portafolio"
                                        ControlToValidate="ddlPortafolio" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle1" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Moneda:
                                </label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlMonedaTercero" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ErrorMessage="Moneda" ControlToValidate="ddlMonedaTercero"
                                        runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle1" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Situaci&oacute;n :</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlSituacionDet" Width="200px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                </label>
                                <div class="col-sm-8">
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                </label>
                                <div class="col-sm-7" align="right">
                                    <asp:Button Text="Agregar" runat="server" ID="ibnAgregarDetalle" ValidationGroup="vgDetalle1" />
                                    <asp:Button Text="Modificar" runat="server" ID="ibnModificarDetalle" ValidationGroup="vgDetalle1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <div class="GridSmall">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" SkinID="GridSmall" ID="dgLista">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                CommandName="Modificar"></asp:ImageButton></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                CommandName="Eliminar"></asp:ImageButton></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoTercero"
                                        HeaderText="CodigoTercero"></asp:BoundField>
                                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="EntidadFinanciera"
                                        HeaderText="EntidadFinanciera"></asp:BoundField>
                                    <asp:BoundField DataField="DesBanco" HeaderText="Banco"></asp:BoundField>
                                    <asp:BoundField DataField="NumeroCuenta" HeaderText="Nro. Cuenta"></asp:BoundField>
                                    <asp:BoundField DataField="CuentaInterBancario" HeaderText="Nro. Cuenta InterBancario">
                                    </asp:BoundField>
                                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS"
                                        HeaderText="CodigoPortafolioSBS"></asp:BoundField>
                                    <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DesPortafolio" HeaderText="Portafolio"></asp:BoundField>
                                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMercado"
                                        HeaderText="CodigoMercado"></asp:BoundField>
                                    <asp:BoundField DataField="DesMercado" HeaderText="Mercado"></asp:BoundField>
                                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMoneda"
                                        HeaderText="CodigoMoneda"></asp:BoundField>
                                    <asp:BoundField DataField="DesMoneda" HeaderText="Moneda"></asp:BoundField>
                                    <asp:BoundField DataField="LiquidacionAutomatica" HeaderText="Liq. Automatica"></asp:BoundField>
                                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Situacion"
                                        HeaderText="Situacion"></asp:BoundField>
                                    <asp:BoundField DataField="NombreSituacion" HeaderText="Situacion"></asp:BoundField>
                                    <asp:BoundField DataField="Secuencial" HeaderText="Secuencial" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <br />
                <br />
                <fieldset>
                    <legend>Datos Generales - Entidad</legend>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Código Entidad:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCodigoEntidad" MaxLength="4" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage="Codigo Entidad"
                                        ControlToValidate="txtCodigoEntidad" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Entidad:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlTipoEntidad" Width="300px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ErrorMessage="Tipo Entidad"
                                        ControlToValidate="ddlTipoEntidad" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Código SBS:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCodigoSBS" MaxLength="50" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Grupo Económico :</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlGrupoEconomico" Width="300px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Factor Castigo:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbFactorCastigo" CssClass="Numbox-7" runat="server" Enabled="False"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Mercado:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlMercado" Width="300px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Sinónimo:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtSinonimo" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ErrorMessage="Sinonimo"
                                        ControlToValidate="txtSinonimo" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Código Broker:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCodigoBroker" onkeypress="return soloLetras(event)"
                                        onblur="limpia()" MaxLength="50" Style="text-transform: uppercase;" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Unidades Emitidas:
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="tbUnidadesEmitidas" CssClass="Numbox-7" MaxLength="22" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Portafolio :</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlPortafolioENT" Width="200px" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" ErrorMessage="Portafolio"
                                        ControlToValidate="ddlPortafolioENT" runat="server" Text="(*)" CssClass="validator"
                                        ValidationGroup="vgDetalle1" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Clase - Entidad </legend>
                    <div class="row">
                        <asp:CheckBoxList ID="chlClase" CellPadding="1" CellSpacing="1" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="5" AutoPostBack="True">
                            <asp:ListItem Value="0">Emisor</asp:ListItem>
                            <asp:ListItem Value="1">Entidad Vinculada</asp:ListItem>
                            <asp:ListItem Value="2">Entidad Vigilada</asp:ListItem>
                            <asp:ListItem Value="3">Broker</asp:ListItem>
                            <asp:ListItem Value="4">Aval</asp:ListItem>
                            <asp:ListItem Value="5">Custodio</asp:ListItem>
                            <asp:ListItem Value="6">Riesgo Cero</asp:ListItem>
                            <asp:ListItem Value="7">Entidad Financiera</asp:ListItem>
                            <asp:ListItem Value="8">Comisionista</asp:ListItem>
                            <asp:ListItem Value="9">Entidad Reguladora</asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-sm-9">
                                    <asp:Button Text="Eliminar" runat="server" ID="Imagebutton1" /></div>
                                <div class="col-sm-2" align="right">
                                    <asp:Button Text="Aceptar" runat="server" ID="ibnAceptar" ValidationGroup="vgDetalle" />
                                    <asp:Button Text="Retornar" runat="server" ID="ibRetornarDetalle" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
            </fieldset> </div>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:HiddenField ID="hd" runat="server" />
    <asp:ValidationSummary runat="server" ID="vgDetalle" ValidationGroup="vgDetalle"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    <asp:ValidationSummary runat="server" ID="vgDetalle1" ValidationGroup="vgDetalle1"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
