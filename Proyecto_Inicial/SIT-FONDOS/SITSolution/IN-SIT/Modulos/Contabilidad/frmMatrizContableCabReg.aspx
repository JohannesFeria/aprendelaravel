<%@ Page Language="VB" AutoEventWireup="false" 
    CodeFile="frmMatrizContableCabReg.aspx.vb" Inherits="Modulos_Contabilidad_frmMatrizContableCabReg" %>

<!DOCTYPE html>

<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server" id="head1">
    <title>Mantenimiento Cabecera Matriz Contable</title>
    <script type="text/javascript">
        $(function () {
            $('#btnAceptar').click(function () {
                if ($('#ddlMatriz').val() == '') {
                    alert('Debe seleccionar una matriz.');
                    return false;
                }
                else if ($('#ddlMoneda').val() == '') {
                    alert('Debe seleccionar una moneda.');
                    return false;
                }
                else if ($('#ddlTipoInstrumento').val() == '' && $('#ddlOperacion').val() != '92') {
                    alert('Debe seleccionar un tipo de instrumento.');
                    return false;
                }
                else if ($('#ddlOperacion').val() == '' && !($('#ddlMatriz').val() == '1')) {
                    alert('Debe seleccionar una operacion.');
                    return false;
                }

            });

            $('#btnAgregar').click(function () {
                if ($('#ddlMatriz').val() == '') {
                    alert('Debe seleccionar una matriz.');
                    return false;
                }
                else if ($('#tbNumeroCuentaContable').val() == '') {
                    alert('Debe ingresar una cuenta contable.');
                    return false;
                }
                else if ($('#tbAplicar').val() == '') {
                    alert('Debe ingresar aplicar.');
                    return false;
                }
                else if ($('#ddlDebeHaber').val() == '') {
                    alert('Debe seleccionar Debe/Haber.');
                    return false;
                }
                else if ($('#tbGlosa').val() == '') {
                    alert('Debe seleccionar una glosa.');
                    return false;
                }
                //OT10783 - Validación tipo cuenta
                else if ($('#ddlTipoCuenta').val() == '') {
                    alert('Debe seleccionar un tipo de cuenta.');
                    return false;
                }
                //OT10783 - Fin

                var existeRegistro = false;
                $("#<%=dgLista.ClientID%> tr:has(td)").each(function () {
                    var cellCuenta = $(this).find("td:eq(3)");
                    var cellSentido = $(this).find("td:eq(4)");
                    var cellTipoCuenta = $(this).find("td:eq(6)");
                    if (cellCuenta.html() == $('#tbNumeroCuentaContable').val() && cellSentido.html() == $('#ddlDebeHaber').val() && cellTipoCuenta.html() == $('#ddlTipoCuenta').val()) {
                        existeRegistro = true;
                    }
                });

                if (existeRegistro) {
                    alert('Cuenta Contable ya existe.');
                    return false;
                }
                $('#btnAgregar').prop('value', 'Agregar');
            });

            $("input[id$=ibModificar]").on("click", function () {
                $('#btnAgregar').prop('value', 'Modificar');
            });

        });

        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(jsFunctions);
        }

        function jsFunctions() {
            $('#btnAceptar').click(function () {
                if ($('#ddlMatriz').val() == '') {
                    alert('Debe seleccionar una matriz.');
                    return false;
                } 
                else if ($('#ddlMoneda').val() == '') {
                    alert('Debe seleccionar una moneda.');
                    return false;
                }
                else if ($('#ddlTipoInstrumento').val() == '' && $('#ddlOperacion').val() != '92') {
                    alert('Debe seleccionar un tipo de instrumento.');
                    return false;
                }
                else if ($('#ddlOperacion').val() == '' && !($('#ddlMatriz').val() == '1')) {
                    alert('Debe seleccionar una operacion.');
                    return false;
                }
                
            });

            $('#btnAgregar').click(function () {
                if ($('#ddlMatriz').val() == '') {
                    alert('Debe seleccionar una matriz.');
                    return false;
                }
                else if ($('#tbNumeroCuentaContable').val() == '') {
                    alert('Debe ingresar una cuenta contable.');
                    return false;
                }
                else if ($('#tbAplicar').val() == '') {
                    alert('Debe ingresar aplicar.');
                    return false;
                }
                else if ($('#ddlDebeHaber').val() == '') {
                    alert('Debe seleccionar Debe/Haber.');
                    return false;
                }
                else if ($('#tbGlosa').val() == '') {
                    alert('Debe seleccionar una glosa.');
                    return false;
                }
                //OT10783 - Validación tipo cuenta
                else if ($('#ddlTipoCuenta').val() == '') {
                    alert('Debe seleccionar un tipo de cuenta.');
                    return false;
                }
                //OT10783 - Fin

                var existeRegistro = false;
                $("#<%=dgLista.ClientID%> tr:has(td)").each(function () {
                    var cellCuenta = $(this).find("td:eq(3)");
                    var cellSentido = $(this).find("td:eq(4)");
                    var cellTipoCuenta = $(this).find("td:eq(6)"); //OT10783 - Obtener tipo de cuenta
                    if (cellCuenta.html() == $('#tbNumeroCuentaContable').val() && cellSentido.html() == $('#ddlDebeHaber').val() && cellTipoCuenta.html() == $('#ddlTipoCuenta').val()) {
                        existeRegistro = true;
                    }
                });

                if (existeRegistro) {
                    alert('Cuenta Contable ya existe.');
                    return false;
                }
                $('#btnAgregar').prop('value', 'Agregar');
            });

            $("input[id$=ibModificar]").on("click", function () {
                $('#btnAgregar').prop('value', 'Modificar');
            });
        }

      
    </script>
</head>
<body onload="load();">
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID ="smLocal">
    </asp:ScriptManager>
        <div class="container-fluid">
            <header>
                <div class="row">
                    <div class="col-md-6">
                        <h2>
                            Mantenimiento Cabecera Matriz Contable
                        </h2>
                    </div>
                </div>
            </header>
            <fieldset>
                <legend>Datos Generales Cabecera</legend>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                C&oacute;digo Cabecera</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="tbCodigo" runat="server" CssClass="form-control" Enabled="false"
                                            MaxLength="18" onkeypress="return soloNumeros(event)"></asp:TextBox>
                            </div>
                            <div class="col-sm-5"></div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Situaci&oacute;n</label>
                            <div class="col-sm-6">
                                <asp:DropDownList ID="ddlSituacion" runat="server" AutoPostBack="False" >
                                    <asp:ListItem Value="A" Text="Activo" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="I" Text="Inactivo"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2"></div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Matriz</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlFondo" runat="server" AutoPostBack="False" Visible="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMatriz" runat="server" AutoPostBack="False" >
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlMoneda" runat="server" >
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Instrumento</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlTipoInstrumento" runat="server" >
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operaci&oacute;n</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlOperacion" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
            </div>           
            </fieldset>
            <br />
            <header>
                <div class="row">
                    <div class="col-md-6">
                        <h2>
                            Detalle Matriz Contable
                        </h2>
                    </div>
                </div>
            </header>
            <br />
            <fieldset>
                <legend>Datos Detalle Matriz</legend>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Label ID="lblNumeroCuenta" runat="server">Cuenta Contable</asp:Label></label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="tbNumeroCuentaContable" runat="server" MaxLength="20" CssClass="form-control"
                                        onkeypress="return soloNumeros(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Debe/Haber</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlDebeHaber" runat="server" Width="130px">
                                        <asp:ListItem Value="" Text="--SELECCIONE--"></asp:ListItem>
                                        <asp:ListItem Value="D" Text="Debe"></asp:ListItem>
                                        <asp:ListItem Value="H" Text="Haber"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Cuenta</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlTipoCuenta" runat="server" Width="130px">
                                        <asp:ListItem Value="" Text="--SELECCIONE--"></asp:ListItem>
                                        <asp:ListItem Value="Importe" Text="Importe"></asp:ListItem>
                                        <asp:ListItem Value="Ganancia" Text="Ganancia"></asp:ListItem>
                                        <asp:ListItem Value="Perdida" Text="Perdida"></asp:ListItem>
                                        <asp:ListItem Value="ValorCausado Local" Text="ValorCausado Local"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                        </div>
                        <div class="col-md-4">
                        </div>
                        <div class="col-md-4" style="text-align: right;">
                            <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                            <asp:HiddenField ID="hfIndexRow" runat="server" />
                            <asp:HiddenField ID="hd" runat="server" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click"  />
                </Triggers>
                </asp:UpdatePanel>
            </fieldset>
            <br />
            <div class="grilla">
                <asp:UpdatePanel runat="server" ID="UpdPanelDetalleMatriz">
                    <ContentTemplate>
                        <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                            <Columns>
                                <asp:TemplateField HeaderText="Modificar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibElimina" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                            OnClientClick="return confirm('¿Desea eliminar el registro seleccionado?')">
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CodigoCabeceraMatriz" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                </asp:BoundField>
                                <asp:BoundField DataField="NumeroCuentaContable" HeaderText="Cuenta Contable"></asp:BoundField>
                                <asp:BoundField DataField="DebeHaber" HeaderText="Debe/Haber"></asp:BoundField>
                                <asp:BoundField DataField="Glosa" HeaderText="Glosa"></asp:BoundField>
                                <asp:BoundField DataField="Aplicar" HeaderText="Aplicar"></asp:BoundField>
                                <asp:BoundField DataField="Secuencia" HeaderText="Secuencia"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <header></header>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="False" />
            </div>
        </div>
    </form>
</body>
</html>
