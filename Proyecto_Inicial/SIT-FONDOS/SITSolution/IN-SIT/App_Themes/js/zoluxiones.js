//Variables Globales
var ERR_CAMPO_OBLIGATORIO = "Los siguientes campos son obligatorios:\n";
var ERR_CONFIRMACION_PASSWORD = "El Password no coincide con lo ingresado en la confirmación.\n\n";
var popUpObj;
//Jquery
$(document).ready(InicializarPagina);
function formatNumber(num, prefix) {
    prefix = prefix || ''; num += '';
    var splitStr = num.split('.');
    var splitLeft = splitStr[0];
    var splitRight = splitStr.length > 1 ? '.' + splitStr[1] : '';
    var regx = /(\d+)(\d{3})/;
    while (regx.test(splitLeft)) { splitLeft = splitLeft.replace(regx, '$1' + ',' + '$2'); }
    return prefix + splitLeft + splitRight;
}
function AsignarNumeroFormateado(caja) {
    $("#" + caja).val(formatNumber($("#" + caja).val()));
}
function soloNumeros(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 8) { return true; }
    patron = /^[-]?[0-9]*\.?[0-9]*$/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}
function pageLoad(sender, args) {
    if (args.get_isPartialLoad()) {
        InicializarPagina();
    }
}
function InicializarPagina() {
    $('.date').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es',
        weekStart: 1
    });
    $('.Numbox-paste').NumBox({ type: "decimal" });
    $('.Numbox-7').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999999999.9999999
    });
    $('.Numbox-2_10').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 10,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999.9999999999
    });
    $('.Numbox-7_22_2').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 22,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 9999999.9999999999999999999999
    });
    $('.-Numbox-7').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: -99999999999999999999.9999999,
        max: 99999999999999999999.9999999
    });
    $('.Numbox-7_12').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999999999999.9999999
    });

    $('.Numbox-7_22').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 9999999999999999999999.9999999
    });

    $('.Numbox-2_2').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 2,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99.99
    });
    $('.Numbox-7_31').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999999999999.9999999
    });
    $('.Numbox-0_20').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999999999
    });

    $('.Numbox-7_32').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999999999999999999999999.9999999
    });
    $('.Numbox-0_12').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999999999999
    });
    $('.Numbox-0_15').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999999999999999
    });
    $('.Numbox-0_9').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999999999
    });
    $('.Numbox-0_5').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999
    });
    $('.Numbox-0_3').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 999
    });
    $('.Numbox-0_2').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99
    });
    $('.Numbox-5').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 5,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999.99999
    });
    $('.Numbox-3').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 3,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 9999999999999999.999
    });
    $('.Numbox-2').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 2,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999999.99
    });
    $('.Numbox-0').NumBox(
    {
        type: "integer",
        fixedLength: !1,
        places: 0,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999999999
    });
//    INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega clase para Amortización de 0 a 100 | 23/05/18
    $('.Numbox-0_100').NumBox({
        type: "decimal",
        fixedLength: !1,
        places: 7,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 100
    });
//    FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega clase para Amortización de 0 a 100 | 23/05/18
}
function EsHoraValida(timeStr) {
    var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))/;
    var matchArray = timeStr.match(timePat);
    if (matchArray == null) {
        alertify.alert("Formato de Hora Incorrecto");
    }
    hour = matchArray[1];
    minute = matchArray[2];
    second = matchArray[4];

    if (second == "") { second = null; }

    if (hour < 0 || hour > 23) {
        return false;
    }

    if (minute < 0 || minute > 59) {
        return false;
    }
    if (second != null && (second < 0 || second > 59)) {
        return false;
    }
    return true;
}

// Modal PopUp
function HideModalDiv() {
    $("divBackground").remove();
}
function OnUnload() {
    if (popUpObj != undefined) {
        if (false == popUpObj.closed) {
            popUpObj.close();
        }
    }
    if (window.opener != null && !window.opener.closed) {
        var target = window.opener.document.getElementById('_target');
        if (target != null) {
            if (target.value == '') {
                window.opener.document.forms[0].submit();
            }
            else {
                window.opener.document.getElementById(target.value).click();
            }
            window.opener.HideModalDiv();
        }
    }
}

function showModalDialog(url, width, height, target) {
    var posicion_x = (screen.width / 2) - (width / 2);
    var posicion_y = (screen.height / 2) - (height / 2);
    $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
    document.getElementById('_target').value = target;

    popUpObj = window.open(url, "ModalPopUp", "toolbar=no,scrollbars=yes,location=no,statusbar=no,menubar=no,resizable=yes,width=" + width + ",height=" + height + ",left = " + posicion_x + ",top=" + posicion_y)
    popUpObj.focus();
    return false;
}

function showWindow(url, width, height) {
    var posicion_x = (screen.width / 2) - (width / 2);
    var posicion_y = (screen.height / 2) - (height / 2);

    window.open(url, "ModalPopUp", "toolbar=no,scrollbars=yes,location=no,statusbar=no,menubar=no,resizable=yes,width=" + width + ",height=" + height + ",left = " + posicion_x + ",top=" + posicion_y)
    return false;
}
window.onunload = OnUnload;

function ShowProgress() {
    setTimeout(function () {
        var modal = $('<div />');
        modal.addClass("modal");
        $('body').append(modal);
        var loading = $(".loading");
        loading.show();
        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
        loading.css({ top: top, left: left });
    }, 200);
}

//INCIO | CDA | eGalarza --- Metodos para la grafica de valor cuota
function LoadChart(portafolio, serie, fecha) {
    var _periodoGrafico = $("[id*=ddlPeriodoGrafico]").val()
    var param = { codigoPortafolio: portafolio, fechaOperacion: fecha, serie: serie, tipoPeriodo: _periodoGrafico };

    $.ajax({
        type: "POST",
        url: "../../MetodosWeb.aspx/getRangos",
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (objDatosGrafico) {
            debugger
            $("#dvChart").html("");

            var configuracion = objDatosGrafico;
            var el = document.createElement('canvas');
            $("#dvChart")[0].appendChild(el);
            var ctx = el.getContext('2d');

            var configuracion = getConfiguracionGraficaValorCuota(objDatosGrafico);

            if (objDatosGrafico.d.variacionDatos != null) {
                $("#txtVCarteraPrecio").val(objDatosGrafico.d.variacionDatos.carteraPrecio);
                $("#txtVCarteraTipoCambio").val(objDatosGrafico.d.variacionDatos.carteraTipoCambio);
                $("#txtVDerivados").val(objDatosGrafico.d.variacionDatos.derivados);
                $("#txtVCuentasCajaTipoCambio").val(objDatosGrafico.d.variacionDatos.cuentasPorCobrarCaja);
                $("#txtVCuentasPorCobrarTipoCambio").val(objDatosGrafico.d.variacionDatos.cuentasPorCobrarTipoCambio);
                $("#txtVCuentasPorPagarTipoCambio").val(objDatosGrafico.d.variacionDatos.cuentasPorPagarTipoCambio);
                $("#txtVCuentasPorCobrarPrecio").val(objDatosGrafico.d.variacionDatos.cuentasPorCobrarPrecio);
                $("#txtVCuentasPorPagarPrecio").val(objDatosGrafico.d.variacionDatos.cuentasPorPagarPrecio);
                $("#txtVPorcentageVariacionEstimado").val(objDatosGrafico.d.variacionDatos.porcentageVariacionEstimado);
                $("#txtVTotalRentabilidadInversiones").val(objDatosGrafico.d.variacionDatos.totalRentabilidadInversiones);
            }
            var urlImagen = "../../App_Themes/img/icons/{0}.png";
         urlImagen=   urlImagen.replace('{0}', objDatosGrafico.d.imagen);
            $('#imgVSemaforo').attr('src', urlImagen);

            $("#lblVFechaOperacion").text(objDatosGrafico.d.fechaOperacion);
            $("#lblVLeyendaVariacion").text(objDatosGrafico.d.leyendaVariacion);
            new Chart(ctx, configuracion);
        },
        failure: function (response) {
            alert('There was an error.');
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('ERROR!!!.');
        }
    });
}

function getConfiguracionGraficaValorCuota(obj) {
    debugger
    var configuracionGrafica = { type: 'line',
        data: { labels: obj.d.rangoX,
            datasets: [{
                label: obj.d.portafolio,
                borderColor: "#3e95cd",
                backgroundColor: "#3e95cd",
                data: obj.d.rangoY,
                fill: false,
                lineTension: 0
            }
         ]
        }, options: {
            legend: {
                display: false
            }, responsive: true, title: { display: false, text: 'Variacion histórica de valor cuota' },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Periodo'
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: '% Var'
                    }
                }]
            },
            pan: {
                // Boolean to enable panning
                enabled: true,

                // Panning directions. Remove the appropriate direction to disable 
                // Eg. 'y' would only allow panning in the y direction
                mode: 'x',
                speed: 700
            },
            zoom: {
                // Boolean to enable zooming
                enabled: true,

                // Zooming directions. Remove the appropriate direction to disable 
                // Eg. 'y' would only allow zooming in the y direction
                mode: 'x',
                speed: 700
            }
        }
    }

    return configuracionGrafica;
}


function AbrirModalGrafica() {
    $("#PopUpGrafico").show();
    CargarGrafica();
    return false;
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
//FIN | CDA | eGalarza --- Metodos para la grafica de valor cuota