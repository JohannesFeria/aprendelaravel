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
    $('.Numbox-5').NumBox(
    {
        type: "decimal",
        fixedLength: !1,
        places: 5,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 99999999999999.99999
    });

    $('.Numbox-3').NumBox(
    {
        type: "decimal",
        fixedLength: !1,
        places: 3,
        grouping: 3,
        separator: ",",
        symbol: "",
        min: 0,
        max: 9999999999999999.999
    });

    $('.Numbox-2').NumBox(
    {
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
}


function EsHoraValida(timeStr) {
    var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))/;
    var matchArray = timeStr.match(timePat);
    if (matchArray == null) {
        alert("Formato de Hora Incorrecto");
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