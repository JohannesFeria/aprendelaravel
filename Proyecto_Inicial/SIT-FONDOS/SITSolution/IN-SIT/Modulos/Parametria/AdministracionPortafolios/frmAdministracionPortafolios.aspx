<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAdministracionPortafolios.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionPortafolios_frmAdministracionPortafolios" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Administraci&oacute;n de Portafolios</title>
    <script type="text/javascript">

        $(function () {
            $(document).ready(function () {
                if ($("#ddlTipoNegocio").val() == "MANDA") {
                    $("#DivManda").css("display", "block")
                } else {
                    $("#DivManda").css("display", "none")
                }
                $("[id$='ddlTipoNegocio']").change(function () {
                    var tipoNegocio = $("#ddlTipoNegocio").val();

                    if (tipoNegocio.toString() == "MANDA") {
                        $("#ddlIndicador").prop("disabled", "disabled");
                        $("#ddlIndicador").val("N");
                        $("#DivManda").css("display", "block")
                    } else {
                        $("#ddlIndicador").removeAttr("disabled");
                        $("#ddlIndicador").val("");
                        $("#DivManda").css("display", "none")
                    }
                });
              
                $(".dropdown dt a").on('click', function () {
                    var ddl = $(this).parent().parent();
                    $(ddl).find("dd ul").slideToggle('fast');
                });

                $(".dropdown dd ul li a").on('click', function () {
                    var ddl = $(this).parent().parent();
                    $(ddl).find("dd ul").hide();
                });

                $('.items-ddl input[type="checkbox"]').on('click', function () {
                    // var title = $(this).closest('.items-ddl').find('input[type="checkbox"]').val(),
                    var valor = $(this).val() + ",";
                    var titulo = $(this).parent().text() + ",";
                    var ddl = $(this).closest('.dropdown');

                    if ($(this).is(':checked')) {
                        var html = '<span title="' + valor + '">' + titulo + '</span>';
                        $(ddl).find(".texto-ddl").append(html);
                        $(ddl).find(".hida").hide();
                        $(ddl).find(".texto-ddl").show();
                    } else {
                        $(ddl).find('span[title="' + valor + '"]').remove();
                        if ($(ddl).find(".texto-ddl").html() == "") {
                            $(ddl).find(".hida").show();
                            $(ddl).find(".texto-ddl").hide();
                        }
                    }
                });


                ddl_setSelectedValue("ddlOrdenVectorPrecio", $("#<%=hdnOrdenVectorPrecio.ClientID%>").val());
                ddl_setSelectedValue("ddlOrdenVectorTC", $("#<%=hdnOrdenVectorTC.ClientID%>").val());

                $("#btnAceptar").click(function () {
                    $("#<%=hdnOrdenVectorPrecio.ClientID%>").val(ddl_getSelectedValue("ddlOrdenVectorPrecio"));
                    $("#<%=hdnOrdenVectorTC.ClientID%>").val(ddl_getSelectedValue("ddlOrdenVectorTC"));
                    return true;
                });

                //            $("#btnPrueba").click(function () {
                //                alert(ddl_getSelectedValue("ddlVectoresTC"));
                //            });

                //            $("#btnPrueba2").click(function () {
                //                ddl_setSelectedValue("ddlVectoresTC", "B,");
                //            });
            });

            $(document).bind('click', function (e) {
                var $clicked = $(e.target);
                if (!$clicked.parents().hasClass("dropdown"))
                    $(".dropdown dd ul").hide();
            });

            function ddl_getSelectedValue(ddlID) {
                var resul = "";
                $("#" + ddlID).find("dt>a>p>span").each(function (index, a) {
                    resul = resul + $(this).attr("title");
                });
                return resul;
            }
            function ddl_setSelectedValue(ddlID, value) {
                var arr = value.split(',');
                for (var i = 0; i < arr.length; i++) {
                    $("#" + ddlID).find('input[value="' + arr[i] + '"]').each(function (index, a) {
                        //$(this).trigger('click'); // No funciona
                        //$(this).click(); // No funciona
                        $(this)[0].click(); // FUNCIONA
                    });
                }
            }

            if ($("#chkComisionVariable").prop('checked')) {
                $("#divComisionVariable").show();
            } else {
                $("#divComisionVariable").hide();
                $("#txtTopeValorCuota").val('');
                $("#txtMontoSuscripcionInicial").val('');
            }

            $("#chkComisionVariable").click(function () {
                if ($(this).prop('checked')) {
                    $("#divComisionVariable").show();
                } else {
                    $("#divComisionVariable").hide();
                    $("#txtTopeValorCuota").val('');
                    $("#txtMontoSuscripcionInicial").val('');
                }
            });

            $("#btnAgregarSeries").click(function () {
                var porcentajeSerie;
                porcentajeSerie = parseFloat($("#txtPorcentajeSerie").val().replace(/\,/g, '')).toFixed(7);
                if ($("#txtCodigoSerie").val() == "") {
                    alertify.alert("Debe ingresar Codigo Serie.");
                    return false;
                }
                else if ($("#txtNombreSerie").val() == "") {
                    alertify.alert("Debe ingresar Nombre Serie.");
                    return false;
                }
                //                else if (porcentajeSerie == 0) {
                //                    alertify.alert("Debe ingresar Porcentaje.");
                //                    return false;
                //                }
            });

            $("#btnAgregarPC").click(function () {
                var margenMin, margenMax, porcentaje;
                margenMin = parseFloat($("#txtMargenMin").val().replace(/\,/g, '')).toFixed(7);
                margenMax = parseFloat($("#txtMargenMax").val().replace(/\,/g, '')).toFixed(7);
                porcentaje = parseFloat($("#txtPorcentaje").val().replace(/\,/g, '')).toFixed(7);

                if (margenMax == 0) {
                    alertify.alert("Debe ingresar Margen Máximo");
                    return false;
                }
                else if (porcentaje == 0) {
                    alertify.alert("Debe ingresar Porcentaje C.");
                    return false;
                }

                else if (parseFloat(margenMin.toString()) >= parseFloat(margenMax.toString())) {
                    alertify.alert("Margen Mínimo debe ser menor que Margen Máximo.");
                    return false;
                }

                var existeIntervalo = false, existeMargen = false;
                $("#<%=dgPorcentajeComision.ClientID%> tr:has(td)").each(function () {
                    var min, max;
                    var min = $(this).find("td:eq(1)");
                    var max = $(this).find("td:eq(2)");
                    if ((parseFloat(min.html()) <= parseFloat(margenMin) && parseFloat(max.html()) >= parseFloat(margenMin)) ||
                        (parseFloat(min.html()) <= parseFloat(margenMax) && parseFloat(max.html()) >= parseFloat(margenMax))) {
                        existeMargen = true;
                    }
                    if (parseFloat(margenMin) <= parseFloat(min.html()) && parseFloat(margenMax) >= parseFloat(max.html())) {
                        existeIntervalo = true;
                    }
                });

                if (existeMargen) {
                    alertify.alert("Márgenes ya utilizados.");
                    return false;
                }
                if (existeIntervalo) {
                    alertify.alert("Intervalo de Margenes en uso.");
                    return false;
                }

            });

        });

        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(jsFunctions);
        }

        function jsFunctions() {

            if ($("#chkComisionVariable").prop('checked')) {
                $("#divComisionVariable").show();
            } else {
                $("#divComisionVariable").hide();
                $("#txtTopeValorCuota").val('');
                $("#txtMontoSuscripcionInicial").val('');
            }

            $("#chkComisionVariable").click(function () {
                if ($(this).prop('checked')) {
                    $("#divComisionVariable").show();
                } else {
                    $("#divComisionVariable").hide();
                    $("#txtTopeValorCuota").val('');
                    $("#txtMontoSuscripcionInicial").val('');
                }
            });

            $("#btnAgregarPC").click(function () {
                var margenMin, margenMax, porcentaje;
                margenMin = parseFloat($("#txtMargenMin").val().replace(/\,/g, '')).toFixed(7);
                margenMax = parseFloat($("#txtMargenMax").val().replace(/\,/g, '')).toFixed(7);
                porcentaje = parseFloat($("#txtPorcentaje").val().replace(/\,/g, '')).toFixed(7);

                if (margenMax == 0) {
                    alertify.alert("Debe ingresar Margen Máximo");
                    return false;
                }
                else if (porcentaje == 0) {
                    alertify.alert("Debe ingresar Porcentaje C.");
                    return false;
                }

                else if (parseFloat(margenMin.toString()) >= parseFloat(margenMax.toString())) {
                    alertify.alert("Margen Mínimo debe ser menor que Margen Máximo.");
                    return false;
                }

                var existeIntervalo = false, existeMargen = false;
                $("#<%=dgPorcentajeComision.ClientID%> tr:has(td)").each(function () {
                    var min, max;
                    var min = $(this).find("td:eq(1)");
                    var max = $(this).find("td:eq(2)");
                    if ((parseFloat(min.html()) <= parseFloat(margenMin) && parseFloat(max.html()) >= parseFloat(margenMin)) ||
                        (parseFloat(min.html()) <= parseFloat(margenMax) && parseFloat(max.html()) >= parseFloat(margenMax))) {
                        existeMargen = true;
                    }
                    if (parseFloat(margenMin) <= parseFloat(min.html()) && parseFloat(margenMax) >= parseFloat(max.html())) {
                        existeIntervalo = true;
                    }
                });

                if (existeMargen) {
                    alertify.alert("Márgenes ya utilizados.");
                    return false;
                }
                if (existeIntervalo) {
                    alertify.alert("Intervalo de Margenes en uso.");
                    return false;
                }

            });
        }

        // INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24 | Agregado del campo CLIENTE
        function showModal_Tercero() {
            $("#hdModal").val("_MODAL_TERCERO_");
            return showModalDialog('../frmHelpControlParametria.aspx?tlbBusqueda=Terceros&codSelec=' + $("#txtCodCliente").val(), '1200', '600', '');
        }
        function limpiarCampoCliente() {
            $('#txtCodCliente').val('');
            $('#txtDescCliente').val('');
            return false;
        }

        function Validadores() {
            $("[id$='ddlTipoNegocio']").change(function () {
                var ValorBase = $("#ddlTipoNegocio").val();
                if (ValorBase.toString() == "MANDA") {
                    $("#DivManda").css("display", "block")
                } else {

                    $("#DivManda").css("display", "none")
                }
            });
        };

        // FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24 | Agregado del campo CLIENTE
    </script>
    <style type="text/css">
        .dropdown
        {
        }
        
        .dropdown dd, .dropdown dt
        {
            margin: 0px;
            padding: 0px;
        }
        .dropdown ul
        {
            margin: -1px 0 0 0;
        }
        .dropdown dd
        {
            position: relative;
        }
        .dropdown a, .dropdown a:visited
        {
            color: #4c4444;
            text-decoration: none;
            outline: none; /* font-size: 12px; */
        }
        .dropdown dt a
        {
            background-color: #ffffff;
            display: block;
            padding: 1px 20px 1px 10px; /* min-height: 25px; */
            line-height: 20px;
            overflow: hidden;
            border: solid 1px #cccccc;
            width: 220px;
        }
        .dropdown dt a span, .texto-ddl span
        {
            cursor: pointer;
            display: inline-block;
            padding: 0 3px 2px 0;
        }
        .dropdown dd ul
        {
            background-color: #ffffff;
            border: solid 1px #cccccc;
            color: #2f2b2b;
            display: none;
            left: 0px;
            padding: 2px 15px 2px 5px;
            position: absolute;
            top: 2px;
            width: 280px;
            list-style: none; /* height: 100px; */
            overflow: auto;
        }
        .dropdown span.value
        {
            display: none;
        }
        .dropdown dd ul li a
        {
            padding: 5px;
            display: block;
        }
        .dropdown dd ul li a:hover
        {
            background-color: #fff;
        }
    </style>
</head>
<body onload="load();">
    <form id="form1" runat="server" class="form-horizontal">
    <asp:HiddenField ID="hdModal" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Administraci&oacute;n de Portafolios</h2></header>
        <br />
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Portafolio SBS</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="tbCodigoPortafolioSBS" runat="server" MaxLength="10" CssClass="form-control" />
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Código Portafolio SBS"
                                ControlToValidate="tbCodigoPortafolioSBS">(*)</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlSituacion" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="40" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Descripción"
                                ControlToValidate="txtDescripcion">(*)</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nombre Completo</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="tbNombreCompleto" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Nombre Completo"
                                ControlToValidate="tbNombreCompleto">(*)</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Constituci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaConstitucion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Fecha Constitución"
                                ControlToValidate="tbFechaConstitucion">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Contable</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaContable" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Fecha Contable"
                                ControlToValidate="tbFechaContable">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Negocio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlNegocio" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Negocio"
                                ControlToValidate="ddlNegocio">(*)</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlMoneda" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group" style="vertical-align: bottom;">
                        <label class="col-sm-4 control-label">
                            Indicador VL</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlIndicador" runat="server" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            N&uacute;mero de Cuota PreCierre</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="tbNroCuotaPreCierre" runat="server" CssClass="form-control Numbox-7"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group" style="vertical-align: bottom;">
                        <label class="col-sm-4 control-label">
                            Tipo Portafolio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlMultifondo" runat="server" AutoPostBack="True" Width="100%">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group" style="vertical-align: bottom;">
                        <label class="col-sm-4 control-label">
                            Valor Inicial Fondo</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="tbValorINIFondo" runat="server" CssClass="form-control Numbox-7"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Contabilidad</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="tbCodigoFondo" MaxLength="10" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Fondo-SMV</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="tbCodFondoMutuo" MaxLength="10" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base Contable</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtBaseContable" MaxLength="20" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Sistema de Operaciones</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtCodSO" MaxLength="10" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            R.U.C.</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtRuc" MaxLength="20" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Renta
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" AutoPostBack="False" />
                            <asp:Label ID="lbAlerta" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Porcentaje en Series</label>
                        <div class="col-sm-1">
                            <asp:CheckBox ID="chkSerie" AutoPostBack="true" runat="server" />
                        </div>
                        <div class="col-sm-6">
                            <label class="col-sm-7 control-label">
                                Cuotas Liberadas</label>
                            <div class="col-sm-1">
                                <asp:CheckBox ID="chkCuotasLiberadas" AutoPostBack="true" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Interface contable</label>
                        <div class="col-sm-1">
                            <asp:CheckBox ID="chkIContable" runat="server" AutoPostBack="true" Width="100%" />
                        </div>
                        <div class="col-sm-6">
                            <label class="col-sm-7 control-label">
                                Fondo Cliente</label>
                            <div class="col-sm-1">
                                <asp:CheckBox ID="chkFondoCliente" runat="server" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Comisi&oacute;n Variable</label>
                        <div class="col-sm-1">
                            <asp:CheckBox ID="chkComisionVariable" runat="server" />
                        </div>
                        <%--INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018--%>
                        <div class="col-sm-6">
                            <label class="col-sm-7 control-label">
                                Aumento de Capital</label>
                            <div class="col-sm-1">
                                <asp:CheckBox ID="chkAumentoCapital" runat="server" />
                            </div>
                        </div>
                        <%--FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018--%>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Comisi&oacute;n en base Suscrip. Ini.</label>
                        <div class="col-sm-1">
                            <asp:CheckBox ID="chkComisionSusIni" runat="server" Width="100%" />
                        </div>
                        <div class="col-sm-6" id="DivManda" style="display:none" >
                            <label class="col-sm-7 control-label">
                             Consolidado</label>
                            <div class="col-sm-1">
                                <asp:CheckBox ID="chkConsolidado" runat="server" AutoPostBack="false" Width="100%" />
                            </div>
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo calculo Valor Cuota
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoCalculoValorCuota" AutoPostBack="False" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valorización Mensual
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoValorizacion" AutoPostBack="False" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Codigo Portafolio Padre S.O.</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCPPadreSO" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Negocio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlTipoNegocio" runat="server" onchange="javascript:Validadores();"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                    </div>
                </div>
                <div id="divFondoCliente" class="col-md-6" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cliente</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <input id="txtCodCliente" type="text" runat="server" value="" readonly="readonly"
                                    style="width: 80px;" />
                                <asp:LinkButton runat="server" ID="lkbShowModal" OnClientClick="return showModal_Tercero();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" OnClientClick="return limpiarCampoCliente();" CausesValidation="False"
                                    UseSubmitBehavior="False"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="txtDescCliente" ReadOnly="true" Style="width: 180px;" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-md-offset-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Comision</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlTipoComision" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Vector Precio (Valorización)</label>
                        <div class="col-sm-8">
                            <asp:HiddenField ID="hdnOrdenVectorPrecio" runat="server" />
                            <dl id="ddlOrdenVectorPrecio" class="dropdown">
                                <dt><a href="#"><span class="hida">Seleccione</span>
                                    <p class="texto-ddl" style="margin: 0;">
                                    </p>
                                </a></dt>
                                <dd>
                                    <div class="items-ddl">
                                        <ul>
                                            <li>
                                                <input type="checkbox" value="REAL" />PIP</li>
                                            <li>
                                                <input type="checkbox" value="SBS" />SBS</li>
                                            <li>
                                                <input type="checkbox" value="MANU" />MANUAL</li>
                                            <li>
                                                <input type="checkbox" value="SAB" />SAB</li>
                                        </ul>
                                    </div>
                                </dd>
                            </dl>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Vector TC (Valorización)</label>
                        <div class="col-sm-8">
                            <asp:HiddenField ID="hdnOrdenVectorTC" runat="server" />
                            <dl id="ddlOrdenVectorTC" class="dropdown">
                                <dt><a href="#"><span class="hida">Seleccione</span>
                                    <p class="texto-ddl" style="margin: 0;">
                                    </p>
                                </a></dt>
                                <dd>
                                    <div class="items-ddl">
                                        <ul>
                                            <li>
                                                <input type="checkbox" value="REAL" />PIP</li>
                                            <li>
                                                <input type="checkbox" value="SBS" />SBS</li>
                                            <li>
                                                <input type="checkbox" value="MANU" />MANUAL</li>
                                            <li>
                                                <input type="checkbox" value="SAB" />SAB</li>
                                        </ul>
                                    </div>
                                </dd>
                            </dl>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div id="FilaSeries" runat="server">
            <fieldset>
                <legend>Series</legend>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                C&oacute;digo Serie</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCodigoSerie" runat="server" MaxLength="10" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Nombre Serie</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtNombreSerie" runat="server" MaxLength="50" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-1">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Porcentaje</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtPorcentajeSerie" runat="server" class="form-control Numbox-7"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Codigo Portafolio S.O.</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCodigoPortafolioSO" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-3" style="text-align: right">
                                <asp:Button ID="btnAgregarSeries" runat="server" Text="Agregar" />
                            </div>
                            <div class="col-sm-1">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-sm-12">
                            <div class="Grilla">
                                <asp:GridView runat="server" SkinID="Grid" ID="dgGrillaVCouta" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="EliminarSerie">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEliminarSerie" runat="server" SkinID="imgDelete" CommandName="EliminarSerie"
                                                    Style="text-align: center" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IdPortafolioSerie")  %>'
                                                    OnClientClick="return confirm('¿Desea eliminar el registro seleccionado?')" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CodigoSerie" HeaderText="Identificador Serie" />
                                        <asp:BoundField DataField="NombreSerie" HeaderText="Nombre Serie" />
                                        <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" />
                                        <asp:BoundField DataField="CodigoPortafolioSO" HeaderText="Portafolio S.O." />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <br />
        <fieldset>
            <legend>Custodios Asociados</legend>
            <asp:UpdatePanel ID="upcustodios" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Custodio</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlcustodio" runat="server" Width="100%" />
                                </div>
                                <div class="col-sm-3">
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group" style="vertical-align: bottom;">
                                <label class="col-sm-4 control-label">
                                    Cuenta Depositaria</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtcuentadepositaria" runat="server" class="form-control" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:Button ID="btnaddcustodio" runat="server" Text="Agregar Custodio" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="GVcustodio" runat="server" SkinID="Grid_AllowPaging_NO" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoCustodio" HeaderText="Codigo Custodio" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="CuentaDepositaria" HeaderText="Cuenta Depositaria" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <asp:Panel ID="tblDetalleGrpFondo" runat="server">
            <fieldset>
                <legend>Detalle Grupo Portafolio</legend>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label runat="server" id="lblPortafolio" class="col-sm-4 control-label">
                                Portafolio</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="col-sm-6" style="text-align: right;">
                            <asp:Button Text="Agregar" runat="server" ID="btAgregar" />
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <div class="Grilla">
                <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar">
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="C&#243;digo Portafolio" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Portafolio SBS" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        <br />
        <div id="divPorcentajeComision">
            <asp:UpdatePanel runat="server" ID="updPorcentajeComision">
                <ContentTemplate>
                    <asp:Panel ID="PNComisionVariable" runat="server">
                        <fieldset>
                            <legend>Porcentaje de Comisi&oacute;n</legend>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label">
                                            Margen M&iacute;nimo</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtMargenMin" runat="server" class="form-control Numbox-7"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label">
                                            Margen M&aacute;ximo</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtMargenMax" runat="server" class="form-control Numbox-7"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label">
                                            Porcentaje C.</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtPorcentaje" runat="server" class="form-control Numbox-7"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label">
                                        </label>
                                        <div class="col-sm-4">
                                        </div>
                                        <div class="col-sm-3" style="text-align: right">
                                            <asp:Button ID="btnAgregarPC" runat="server" Text="Agregar" />
                                        </div>
                                        <div class="col-sm-1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div>
                                        <div class="col-sm-12">
                                            <div class="Grilla">
                                                <asp:GridView runat="server" ID="dgPorcentajeComision" AutoGenerateColumns="false"
                                                    SkinID="Grid">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminarPC" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                                                    Style="text-align: center" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Secuencia")  %>'
                                                                    OnCommand="EliminarPC" OnClientClick="return confirm('¿Desea eliminar el registro seleccionado?')" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ValorMargenMinimo" HeaderText="Margen M&#237;nimo" DataFormatString="{0:#,##0.0000000}" />
                                                        <asp:BoundField DataField="ValorMargenMaximo" HeaderText="Margen M&#225;ximo" DataFormatString="{0:#,##0.0000000}" />
                                                        <asp:BoundField DataField="ValorPorcentajeComision" HeaderText="Porcentaje" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregarPC" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <div id="divComisionVariable" style="display: none;">
            <fieldset>
                <legend>Comisi&oacute;n Variable</legend>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Margen Valor Cuota</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtTopeValorCuota" runat="server" class="form-control Numbox-7"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Suscripci&oacute;n Inicial
                            </label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMontoSuscripcionInicial" runat="server" class="form-control Numbox-7"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="False" />
            <asp:HiddenField ID="hdnCodigo" runat="server" />
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
