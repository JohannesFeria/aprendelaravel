//OT 10090 - 26/07/2017 - Carlos Espejo
//Codigo para busqueda con texto en masivo
$(document).ready(function () {
    var Fila = $("#Datagrid1 tr").length;
    var IndiceCaja = 0
    var IndiceBlucle = 0
    for (i = 2; i < Fila; i++) {
        if (i.toString().length == 1) { IndiceBlucle = "0" + i.toString(); } else { IndiceBlucle = i; }
        //Como no se puede indentificar el indice en el evento autocomplete, se usa un control comodin cuando se digita algo nuevo
        $("#Datagrid1_ctl" + IndiceBlucle + "_tbNemonico").on('input', function (e) {
            $("#hdNemonicoBusqueda").val($(this).val());
            IndiceCaja = $(this).parents("tr").index() + 1;
            if (IndiceCaja.toString().length == 1) { IndiceCaja = "0" + IndiceCaja.toString(); }
        });
        $("#Datagrid1_ctl" + IndiceBlucle + "_tbIntermediario").on('input', function (e) {
            $("#hdIntermediarioBusqueda").val($(this).val());
            IndiceCaja = $(this).parents("tr").index() + 1;
            if (IndiceCaja.toString().length == 1) { IndiceCaja = "0" + IndiceCaja.toString(); }
        });

        //        $("#Datagrid1_ctl" + IndiceBlucle + "_tbNemonico").autocomplete({
        //            source: function (request, response) {
        //                var param = { Nemonico: $("#Datagrid1_ctl" + IndiceBlucle + "_tbNemonico").val(), FechaOperacion: $("#tbFechaOperacion").val(), TipoRenta: $("#hdTipoRenta").val() };
        //                $.ajax({
        //                    url: "../../MetodosWeb.aspx/GetNemonico",
        //                    data: JSON.stringify(param),
        //                    dataType: "json",
        //                    type: "POST",
        //                    contentType: "application/json; charset=utf-8;",
        //                    dataFilter: function (data) { return data; },
        //                    success: function (data) {
        //                        $("#Datagrid1_ctl" + IndiceBlucle + "_hdClaseInstrumento").val("");
        //                        response($.map(data.d, function (item) {
        //                            return {
        //                                key: item.Categoria,
        //                                value: item.Nemonico
        //                            }
        //                        }))
        //                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
        //                });
        //            }, select: function (event, ui) {
        //                if (ui.item) {
        //                    $("#Datagrid1_ctl" + IndiceBlucle + "_hdClaseInstrumento").val(ui.item.key);
        //                }
        //            }, change: function (event) {
        //                // INICIO | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Lanzmos el evento change de la caja de texto
        //                $("#Datagrid1_ctl" + IndiceBlucle + "_tbNemonico").trigger("change");
        //                // FIN | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Lanzmos el evento change de la caja de texto
        //            },
        //            minLength: 2
        //        });

        //OT10709 - Se corrigió script que no obtenía el código del intermediario
        $("#Datagrid1_ctl" + IndiceBlucle + "_tbIntermediario").autocomplete({
            source: function (request, response) {
                var param = { Descripcion: $("#hdIntermediarioBusqueda").val() };
                $.ajax({
                    url: "../../MetodosWeb.aspx/GetIntermediario",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8;",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        $("#Datagrid1_ctl" + IndiceCaja + "_hdIntermediario").val("");
                        response($.map(data.d, function (item) {
                            return {
                                key: item.CodigoTercero,
                                value: item.Descripcion
                            }
                        }))
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
                });
            }, select: function (event, ui) {
                if (ui.item) {
                    $("#Datagrid1_ctl" + IndiceCaja + "_hdIntermediario").val(ui.item.key);
                }
            },
            minLength: 2
        }); //OT10709 - Fin
    }
    if ($("#hdGrillaRegistros").val() == "0") { Fila = "03" } else {
        if (Fila.toString().length == 1) { Fila = "0" + Fila.toString(); }
    }
    //Terminar Bucle // Ultima fila de la Tabla

    //    $("#Datagrid1_ctl" + Fila + "_tbNemonicoF").autocomplete({
    //        source: function (request, response) {

    //            var param = { Nemonico: $("#Datagrid1_ctl" + Fila + "_tbNemonicoF").val(), FechaOperacion: $("#tbFechaOperacion").val(), TipoRenta: $("#hdTipoRenta").val() };
    //            $.ajax({
    //                url: "../../MetodosWeb.aspx/GetNemonico",
    //                data: JSON.stringify(param),
    //                dataType: "json",
    //                type: "POST",
    //                contentType: "application/json; charset=utf-8;",
    //                dataFilter: function (data) { return data; },
    //                success: function (data) {
    //                    $("#Datagrid1_ctl" + Fila + "_hdClaseInstrumentoF").val("");
    //                    response($.map(data.d, function (item) {
    //                        return {
    //                            key: item.Categoria,
    //                            value: item.Nemonico
    //                        }
    //                    }))
    //                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
    //            });
    //        }, select: function (event, ui) {
    //            if (ui.item) {
    //                $("#Datagrid1_ctl" + Fila + "_hdClaseInstrumentoF").val(ui.item.key);
    //            }
    //        }, change: function (event) {
    //            // INICIO | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Lanzmos el evento change de la caja de texto
    //            $("#Datagrid1_ctl" + Fila + "_tbNemonicoF").trigger("change");
    //            // FIN | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Lanzmos el evento change de la caja de texto
    //        },
    //        minLength: 2
    //    });

    $("#Datagrid1_ctl" + Fila + "_tbIntermediarioF").autocomplete({
        source: function (request, response) {
            var param = { Descripcion: $("#Datagrid1_ctl" + Fila + "_tbIntermediarioF").val() };
            $.ajax({
                url: "../../MetodosWeb.aspx/GetIntermediario",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    $("#Datagrid1_ctl" + Fila + "_hdIntermediarioF").val("");
                    response($.map(data.d, function (item) {
                        return {
                            key: item.CodigoTercero,
                            value: item.Descripcion
                        }
                    }))
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        }, select: function (event, ui) {
            if (ui.item) {
                $("#Datagrid1_ctl" + Fila + "_hdIntermediarioF").val(ui.item.key);
            }
        },
        minLength: 2
    });
    //Busqueda de filtro grilla
    $("#tbCodigoMnemonico").autocomplete({
        source: function (request, response) {
            var param = { Nemonico: $("#tbCodigoMnemonico").val(), FechaOperacion: $("#tbFechaOperacion").val(), TipoRenta: $("#hdTipoRenta").val() };
            $.ajax({
                url: "../../MetodosWeb.aspx/GetNemonico",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            key: item.Categoria,
                            value: item.Nemonico
                        }
                    }))
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        },
        minLength: 2
    });


    // INICIO | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Configuración del Autocompletar para controles de búsqueda NEMONICO
    $("[id$='tbNemonicoF']").each(function () {

        var nombreTextboxNemonico = $(this).attr('id').toString();
        var nombreEnArray = nombreTextboxNemonico.split('_');
        var prefijoControlEnGrilla = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';

        configurarAutoCompletarEnTextBoxNemonico(nombreTextboxNemonico, prefijoControlEnGrilla + "hdClaseInstrumentoF");
    });
    $("[id$='tbNemonico']").each(function () {

        var nombreTextboxNemonico = $(this).attr('id').toString();
        var nombreEnArray = nombreTextboxNemonico.split('_');
        var prefijoControlEnGrilla = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';

        configurarAutoCompletarEnTextBoxNemonico(nombreTextboxNemonico, prefijoControlEnGrilla + "hdClaseInstrumento");
    });
    // FIN | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Configuración del Autocompletar para controles de búsqueda NEMONICO


});

// INICIO | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Configuración del Autocompletar para controles de búsqueda NEMONICO
// Método reutilizable
function configurarAutoCompletarEnTextBoxNemonico(nombreTextboxNemonico, nombreHiddenClaseInstrumento) {

    $("#" + nombreTextboxNemonico).autocomplete({
        source: function (request, response) {
            var param = { Nemonico: $("#" + nombreTextboxNemonico).val(), FechaOperacion: $("#tbFechaOperacion").val(), TipoRenta: $("#hdTipoRenta").val() };
            $.ajax({
                url: "../../MetodosWeb.aspx/GetNemonico",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    $("#" + nombreHiddenClaseInstrumento).val("");
                    response($.map(data.d, function (item) {
                        return {
                            key: item.Categoria,
                            value: item.Nemonico
                        }
                    }))
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        }, select: function (event, ui) {
            if (ui.item) {
                $("#" + nombreHiddenClaseInstrumento).val(ui.item.key);
                $("#" + nombreTextboxNemonico).val(ui.item.value);
                $("#" + nombreTextboxNemonico).trigger("change");
            }
        }, change: function (event) {
            $("#" + nombreTextboxNemonico).trigger("change"); // CRumiche Debido a que no lanzaba el evento                
        },
        minLength: 2
    });
}
// FIN | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-07-31 | Configuración del Autocompletar para controles de búsqueda NEMONICO

// ==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | Actualizar Combo Portafolios cuando cambia un Nemonico
// Método reutilizable:Esto ayuda a evitar el método mouseover (javascript) en ddlPortafolio que se hace en cada página
function actualizarComboPortafolioCuandoCambioNemonico(nombreTextboxNemonico, nombreComboPortafolio, callBack) {

    var codNemonico = $("#" + nombreTextboxNemonico).val().toUpperCase();
    $("#" + nombreTextboxNemonico).val(codNemonico); /*Para aprovechar el UPPER*/

    if (codNemonico.length > 0) {
        var param = { CodigoNemonico: codNemonico };
        $.ajax({
            url: "../../MetodosWeb.aspx/GetPortafolioCodigoListarByNemonico",
            data: JSON.stringify(param),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8;",
            success: function (data) {

                $("#" + nombreComboPortafolio).html('');
                $("#" + nombreComboPortafolio).append('<option value="0">--Seleccione--</option>');
                for (var i = 0; i < data.d.length; i++) {
                    $("#" + nombreComboPortafolio).append('<option value="' + data.d[i].id + '">' + data.d[i].descripcion + '</option>');
                }

                //callBack();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
        });
    }
}
// ==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | Actualizar Combo Portafolios cuando cambia un Nemonico

// ==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-10-04 | Seleccionar los check de cada fila en una grilla cualquier
function seleccionarChecksEnGrilla(controlCheckCabecera, nombreGrilla, nombreCheckFila) {
    var chekeado = $(controlCheckCabecera).is(":checked");

    $("#" + nombreGrilla + " [id$='" + nombreCheckFila + "']").each(function () {
        if (!$(this).is(':disabled')) { 
            if (chekeado)
                $(this).attr("checked", "checked");
            else
                $(this).removeAttr('checked');
        }
    });
}

/*Evaluar cuantos check de una grilla estan checkeados*/
function contarChecksSeleccionadosEnGrilla(nombreGrilla, nombreCheckFila) {
    var cont = 0;
    $("#" + nombreGrilla + " [id$='" + nombreCheckFila + "']").each(function () {
        if ($(this).is(":checked"))
            cont++;
    });
    return cont;
}
// ==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-10-04 | Seleccionar los check de cada fila en una grilla