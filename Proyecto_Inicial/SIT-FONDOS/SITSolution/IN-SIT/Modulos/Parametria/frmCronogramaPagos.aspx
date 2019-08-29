<%@ Page Language = "VB" AutoEventWireup = "false" CodeFile = "frmCronogramaPagos.aspx.vb" Inherits = "Modulos_Parametria_frmCronogramaPagos" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts2")%>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos2") %>
<head runat="server">
    <title>Generación de Cronograma de Pagos </title>
    <style type="text/css">
        .cabeceraGrilla
        {
            color: #0039A6;
            background-color: #EFF4FA;
            font-family: Trebuchet MS;
            font-size: 11px;
            font-weight: bold;
            height: 23px;
        }
    </style>
</head>
<script type="text/javascript">
    $(function () {
        $(".grabar").confirm({
            text: "¿Desea Grabar el cronograma de Pagos?",
            title: "¿Desea Grabar el cronograma de Pagos?",
            confirm: function (button) {
                $("#btnGrabar").click();
            },
            cancel: function (button) {
                // nothing to do
            },
            confirmButton: "Aceptar",
            cancelButton: "Cancelar",
            post: true,

            confirmButtonClass: "btn btn-integra",
            cancelButtonClass: "btn btn-integra",
            dialogClass: "modal-dialog modal-lg"
        });

        $("#datosDetalleInstrumentos").dialog({ resizable: false, autoOpen: false,
            buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
            hide: { effect: "fade", duration: 500 }
        });
        $("#datosDetalleInstrumentos").dialog("option", "width", 420).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $(".VerDetalle").click(function () {
            $('#dgDetalleInstrumento').empty();
            var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
            Cargar_Data($("#" + prefijoControlEnGrilla + "tbFechaPago").val());
            $("#datosDetalleInstrumentos").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');

        });
        $("div>.ui-dialog-buttonset>:button").addClass("btn btn-integra");
        $(document).keyup(function () {
            if (event.which == 27) {
                $("#divBackground").remove();
            }
        });
        function Cargar_Data(fechaPago) {
            debugger;
            var param = { fondo: $("#ddlFondo").val(), fechaPago: fechaPago };
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: "../../MetodosWeb.aspx/GetDetalleInstrumentoCronogramaPagos",
                data: JSON.stringify(param),
                dataType: 'JSON',
                success: function (response) {
                    $('#dgDetalleInstrumento').append("<tr class='cabeceraGrilla' style='font-family:Trebuchet MS;font-size:11px;height:18px;'><th>Código Mnemónico</th><th>Tipo Pago</th><th>Fecha Liquidación</th></tr>")
                    for (var i = 0; i < response.d.length; i++) {
                        debugger;
                        $('#dgDetalleInstrumento').append("<tr class='nowrap' style='font-family:Trebuchet MS;font-size:11px;height:18px;'><td>" + response.d[i].codigoMnemonico + "</td><td style='text-align:center'>" + response.d[i].TipoPago + "</td><td style='text-align:center'>" + response.d[i].fechaLiquidacion + "</td></tr>")
                    };
                },
                error: function () {
                    alert("Error en la carga del detalle del cronograma de pagos por instrumento.");
                }
            });
            return false;
        };
        function ObtenerPrefijoControlEnGrilla(controlEnGrilla) {
            var idControl = $(controlEnGrilla).attr('id').toString();
            var nombreEnArray = idControl.split('_');
            var prefijoControl = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';
            return prefijoControl.toString();
        };
    });
    $('form').live("submit", function () {
        $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        ShowProgress();
    });


</script>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Generar Cronograma de Pagos</h2></header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fondo</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlFondo" runat="server" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Búsqueda</legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-3">
                            <div class="input-append date" style="text-align: left;">
                                <asp:TextBox runat="server" ID="tbFechaIni" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                        <label class="col-sm-2 control-label">
                            Fecha Final</label>
                        <div class="col-sm-3">
                            <div class="input-append date" style="text-align: left;">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <%--       <div class="col-md-5">
                    <div class="form-group">
                   
                    </div>
                </div>--%>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Cronograma de Pagos
                <asp:Label ID="lblCantidad" runat="server">(0)</asp:Label></legend>
            <div class="grilla" style="width: 300px">
                <asp:GridView ID="dgCronogramaPagos" runat="server" SkinID="GridFooter" DataKeyNames="CodigoPortafolioSBS,fechaCronogramaPagos">
                    <Columns>
                        <asp:TemplateField HeaderText="Fecha Pago">
                            <ItemTemplate>
                                <div class="input-append date" id="divFecha" runat="server">
                                    <asp:TextBox runat="server" Text='<%# Eval("fechaCronogramaPagos") %>' ID="tbFechaPago"
                                        SkinID="Date" AutoPostBack="true" OnTextChanged="tbFechaPago_TextChanged" />
                                    <span class="add-on" id="imgFechaPago"><i class="awe-calendar"></i></span>
                                </div>
                                <asp:HiddenField ID="hdIdCronogramaPagos" runat="server" Value='<%# Eval("idCronogramaPagos") %>' />
                                <asp:HiddenField ID="hdAccion" runat="server" Value='<%# Eval("Accion") %>' />
                                <asp:HiddenField ID="hdCodigoPortafolio" runat="server" Value='<%# Eval("CodigoPortafolioSBS") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaPagoF" SkinID="Date" />
                                    <span class="add-on" id="imgFechaPago"><i class="awe-calendar"></i></span>
                                </div>
                            </FooterTemplate>
                            <ControlStyle Width="80px" />
                            <FooterStyle Width="80px" />
                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Instrumentos" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <button class="VerDetalle btn btn-integra" type="button" id="btnVerDetalle" runat="server"
                                    title="Ver detalle por Instrumento." style="height: 22px; font-size: 12px;">
                                    Ver Detalle</button>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDelete" runat="server" SkinID="imgDelete" CommandName="Del"
                                    ToolTip="Eliminar Fecha." />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton ID="imgAdd" runat="server" SkinID="imgAdd" CommandName="Add" ToolTip="Agregar Fecha." />
                            </FooterTemplate>
                            <ControlStyle Width="50px" />
                            <ItemStyle Width="50px" />
                            <FooterStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div class="row">
                <div class="col-md-13" style="text-align: right;">
                    <button class="grabar btn btn-integra" type="button" id="btnPrevGrabar" runat="server">
                        Grabar</button>
                    <asp:Button Text="Salir" runat="server" ID="btnSalir" />
                </div>
            </div>
        </fieldset>
        <asp:Label ID="lblFlagCambios" Text="* Existen cambios sin grabar" runat="server"
            Font-Bold="false" ForeColor="Red" Font-Italic="true" Font-Size="X-Small" />
    </div>
    <div style="display: none">
        <asp:Button Text="" runat="server" ID="btnGrabar" />
    </div>
    <div id="datosDetalleInstrumentos" title="Detalle de Cronograma de Pagos por Instrumento">
        <div class="form-horizontal">
            <div class="container-fluid">
                <div class="grilla" style="height: 200px; width: 100%; overflow: auto;">
                    <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgDetalleInstrumento">
                        <Columns>
                            <asp:BoundField DataField="codigoMnemonico" HeaderText="Código Mnemónico" />
                            <asp:BoundField DataField="tipoPago" HeaderText="Tipo Pago" />
                            <asp:BoundField DataField="fechaLiquidacion" HeaderText="Fecha Liquidación" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="loading" align="center">
        <img src="../../App_Themes/img/icons/loading.gif" alt="Cargado..." />
    </div>
    </form>
</body>
</html>