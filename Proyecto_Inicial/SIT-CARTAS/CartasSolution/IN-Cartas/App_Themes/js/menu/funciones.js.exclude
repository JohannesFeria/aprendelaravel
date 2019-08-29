

function formatCurrency(myElem) {

    var num = document.getElementById(myElem.id).value;

    if (num != "") {

        var pos1 = num.toString().lastIndexOf('.');

        var pos2 = num.toString().substring(pos1 + 1);

        var tmp1 = pos2 + '0000000'

        var tmp2 = tmp1.substr(0, 7);

        var numoriginal = 0;

        var texto = '';

        var pos = 0;

        num = num.toString().replace(/$|,/g, '');

        if (isNaN(num))

            num = "0";

        sign = (num == (num = Math.abs(num)));

        texto = num + '';

        pos = texto.lastIndexOf('.');

        if (pos > 0)

        { numoriginal = texto.substring(0, pos) }

        else

        { numoriginal = num }

        num = Math.floor(num * 100 + 0.50000000001);

        cents = num % 100;

        num = Math.floor(num / 100).toString();

        if (cents < 10) {
            cents = "0" + cents + '0000000';

            cents = cents.substr(0, 7);

        }

        else

        { cents = tmp2; }



        if (pos1 == -1) {

            tmp2 = '0000000';

        }



        for (var i = 0; i < Math.floor((numoriginal.length - (1 + i)) / 3); i++)

            numoriginal = numoriginal.substring(0, numoriginal.length - (4 * i + 3)) + ',' +

                                                                       numoriginal.substring(numoriginal.length - (4 * i + 3));



        document.getElementById(myElem.id).value = (((sign) ? '' : '-') + numoriginal + '.' + tmp2);

    }

    return false;

}



function HoraValida(valor, alerta) {
    var er_fh = /^(1|01|2|02|3|03|4|04|5|05|6|06|7|07|8|08|9|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23)\:([0-5]0|[0-5][1-9])$/

    if (!(er_fh.test(valor))) {
        alert(alerta);
        return false
    }
    return true
}

function ValidaNumerico() {
    var re;
    var ch = String.fromCharCode(event.keyCode);
    if ((event.keyCode <= 57) && (event.keyCode >= 48)) {
        if (!event.shiftKey) { return; }
    };
    event.returnValue = false;
}

function ValidaAlfaNumerico() {
    var re;
    var ch = String.fromCharCode(event.keyCode);
    if ((event.keyCode <= 57) && (event.keyCode >= 48)) {
        if (!event.shiftKey) { return; }
    };
    if (event.keyCode == 32 || event.keyCode == 164 || event.keyCode == 165) {
        return;
    };
    if ((event.keyCode <= 90) && (event.keyCode >= 65)) {
        return;
    };
    if ((event.keyCode <= 122) && (event.keyCode >= 97)) {
        return;
    };
    event.returnValue = false;
}



function onKeyPressFecha(field) {
    KeyAscii = event.keyCode
    event.keyCode = 0
    if ((field.value.length == 1) || (field.value.length == 4)) {
        if (KeyAscii < 48 || KeyAscii > 57) {
            event.keyCode = 0
        }
        else {
            event.keyCode = 0
            field.value = field.value + String.fromCharCode(KeyAscii) + "/";
        }
    }
    else {
        if (KeyAscii < 48 || KeyAscii > 57) {
            event.keyCode = 0
        }
        else {
            event.keyCode = KeyAscii
        }
    }
}


function fechas(caja) {
    var a;
    var m;
    var d;

    if (caja) {
        borrar = caja;
        if ((caja.substr(2, 1) == "/") && (caja.substr(5, 1) == "/")) {
            for (i = 0; i < 10; i++) {
                if (((caja.substr(i, 1) < "0") || (caja.substr(i, 1) > "9")) && (i != 2) && (i != 5)) {
                    borrar = "";
                    break;
                }
            }

            if (borrar) {
                a = caja.substr(6, 4);
                m = caja.substr(3, 2);
                d = caja.substr(0, 2);

                if ((a < 1900) || (a > 2050) || (m < 1) || (m > 12) || (d < 1) || (d > 31)) {
                    borrar = "";
                }
                else {
                    if ((a % 4 != 0) && (m == 2) && (d > 28)) {
                        borrar = ""; // A?o no viciesto y es febrero y el dia es mayor a 28 
                    }
                    else {
                        if ((((m == 4) || (m == 6) || (m == 9) || (m == 11)) && (d > 30)) || ((m == 2) && (d > 29))) {
                            borrar = "";
                        }
                    }
                }
            }
        }
        else {
            return false;
        }
    }
    else {
        borrar = "";

    }

    if (borrar == "") {
        return false;
    }
    else {
        return true;
    }
}


function ValidaFecha(field) {
    if (field.value != "") {
        Fecha = field.value;
        if (!fechas(Fecha)) {
            alert("Fecha ingresada no posee el formato dd/mm/aaaa o rango valido");
            field.value = "";
            field.focus();
        } else {
            //fiel.focus();
        }
    }
}


function convertFormatDate(sDate, formatIn, formatOut) {
    var year = "";
    var month = "";
    var day = "";
    var pos = formatIn.indexOf("y");
    while (pos < formatIn.length && formatIn.charAt(pos) == "y") {
        year += sDate.charAt(pos);
        pos++;
    }
    var pos = formatIn.indexOf("m");
    while (pos < formatIn.length && formatIn.charAt(pos) == "m") {
        month += sDate.charAt(pos);
        pos++;
    }
    var pos = formatIn.indexOf("d");
    while (pos < formatIn.length && formatIn.charAt(pos) == "d") {
        day += sDate.charAt(pos);
        pos++;
    }
    var flength = formatOut.length;
    var y = 0;
    var m = 0;
    var d = 0;
    var result = "";
    for (var i = flength - 1; i >= 0; i--) {
        var fletra = formatOut.charAt(i);
        var letra;
        if (fletra == 'y') {
            letra = year.charAt(year.length - 1 - y);
            y++;
        } else if (fletra == 'm') {
            letra = month.charAt(month.length - 1 - m);
            m++;
        } else if (fletra == 'd') {
            letra = day.charAt(day.length - 1 - d);
            d++;
        } else {
            letra = fletra;
        }
        result = letra + result;
    }
    return result;
}

function FechaMenor(Fecha, FechaReferencia) { /// Fecha en formato "dd/mm/yyyy"
    /// FechaReferencia en formato "dd/mm/yyyy"
    var fecha = convertFormatDate(Fecha, "dd/mm/yyyy", "yyyymmdd");

    var fechaReferencia = convertFormatDate(FechaReferencia, "dd/mm/yyyy", "yyyymmdd");



    var fecha1 = new Date(fecha.substr(0, 4), parseFloat(fecha.substr(4, 2)) - 1, fecha.substr(6, 2));
    var fecha2 = new Date(fechaReferencia.substr(0, 4), parseFloat(fechaReferencia.substr(4, 2)) - 1, fechaReferencia.substr(6, 2));


    if (fecha1 > fecha2) {
        return true;
    }
    return false;

}


function FechaMayor(Fecha1, Fecha2) //Retorna si la primera Fecha1 es mayor o igual a la Fecha2
{
    var xMes = Fecha1.substring(3, 5);
    var xDia = Fecha1.substring(0, 2);
    var xAnio = Fecha1.substring(6, 10);
    var yMes = Fecha2.substring(3, 5);
    var yDia = Fecha2.substring(0, 2);
    var yAnio = Fecha2.substring(6, 10);
    if (xAnio > yAnio)
        return (true)
    else
        if (xAnio == yAnio) {
            if (xMes > yMes)
                return (true)
            else {
                if (xMes == yMes) {
                    if (xDia >= yDia)
                        return (true);
                    else
                        return (false);
                }
                else
                    return (false);
            }
        }
        else
            return (false);
}

function ColorearFila(obj) {
    obj = document.getElementById(obj);
    obj.style.backgroundColor = '#B0D8FF';
}

function RestaurarFila(obj, Tipo) {
    if (Tipo == 1) {
        obj = document.getElementById(obj);
        obj.style.backgroundColor = '#FEFEFE';
    } else {
        obj = document.getElementById(obj);
        obj.style.backgroundColor = '#F3F5FB';
    }
}


function OnKeyPressTextLetras() {
    var e_k = event.keyCode;
    if (e_k >= 97 && e_k <= 122 || e_k == 32 || e_k >= 65 && e_k <= 90 || e_k == 241 || e_k == 209) {
        event.returnValue = true; //97-122 minusculas, 65-90 Mayusculas, 32-espacioblanco, 241-ñ, 209-Ñ
    }
    else {
        event.returnValue = false;
    }
}





function OnKeyPressLetrasCaracteres() {
    //acentos
    //Á - e_k == 193
    //á - e_k == 225
    //É - e_k == 201
    //é - e_k == 233
    //Í - e_k == 205
    //í - e_k == 237
    //Ó - e_k == 211
    //ó - e_k == 243
    //Ú - e_k == 218
    //ú - e_k == 250

    var e_k = event.keyCode;
    if (e_k >= 97 && e_k <= 122 || e_k == 32 || e_k >= 65 && e_k <= 90 || e_k == 241 || e_k == 209 || e_k == 40 || e_k == 41 || e_k == 44 || e_k == 46 || e_k == 47 || e_k == 45 || e_k == 193 || e_k == 225 || e_k == 201 || e_k == 233 || e_k == 205 || e_k == 237 || e_k == 211 || e_k == 243 || e_k == 218 || e_k == 250) {
        event.returnValue = true; //97-122 minusculas, 65-90 Mayusculas, 32-espacioblanco, 241-ñ, 209-Ñ
    }
    else {
        event.returnValue = false;
    }
}

function OnKeyPressTextNumeros() {
    var e_k = event.keyCode;
    if (e_k >= 48 && e_k <= 57) //48-57 es 0 al 9
    {
        event.returnValue = true;
    }
    else {
        event.returnValue = false;
    }
}


function OnKeyPressTextLetrasConvierteMayusculas() {
    var e_k = event.keyCode;
    if (e_k >= 97 && e_k <= 122 || e_k == 32 || e_k >= 65 && e_k <= 90 || e_k == 241 || e_k == 209) {
        if (e_k >= 97 && e_k <= 122) {
            event.keyCode = event.keyCode - 32;
        }
        event.returnValue = true; //97-122 minusculas, 65-90 Mayusculas, 32-espacioblanco, 241-ñ, 209-Ñ
    }
    else {
        event.returnValue = false;
    }
}

function AceptaSoloNroDecimal() {
    if ((event.keyCode > 57 || event.keyCode < 48) && event.keyCode != 13 && event.keyCode != 46) {
        event.returnValue = false;
    }
}

function OnKeyPressTextDecimales(Text) {
    var e_k = event.keyCode;
    if (e_k >= 48 && e_k <= 57) //48-57 es 0 al 9
    {
        event.returnValue = true;
    }
    else {
        if (e_k == 46) {
            if (Text.value != "") {
                indicepunto = Text.value.indexOf('.');
                if ((indicepunto != -1))  //e_k=46 es el '.'
                {
                    event.returnValue = false;
                }
                else {
                    event.returnValue = true;
                }
            }
            else {
                event.returnValue = false;
            }
        }
        else {
            event.returnValue = false;
        }
    }
}

//function OnKeyPressTextDecimalesNegativos(Text)
//{
//	var e_k = event.keyCode;						
//	if (e_k >= 48 && e_k <= 57 || e_k==45 || e_k = 13) //48-57 es 0 al 9
//	{				
//		event.returnValue = true;
//	}
//	else
//	{	
//		if (e_k == 46) 
//		{						
//			if (Text.value != "")
//			{						
//				indicepunto = Text.value.indexOf('.');					
//				if ((indicepunto != -1))  //e_k=46 es el '.'
//				{					
//					event.returnValue = false;
//				}
//				else
//				{
//					event.returnValue = true;
//				}
//			}
//			else
//			{				
//				event.returnValue = false;				
//			}				
//		}
//		else
//		{
//			event.returnValue = false;
//		}			
//	}
//}

function OnKeyPressTextDecimalesconComa(Text) {

    var e_k = event.keyCode;


    if (e_k >= 48 && e_k <= 57) //48-57 es 0 al 9
    {
        event.returnValue = true;
    }
    else {
        if (e_k == 44) {
            if (Text.value != "") {
                indicepunto = Text.value.indexOf(',');
                if ((indicepunto != -1))  //e_k=44 es el ','
                {
                    event.returnValue = false;
                }
                else {
                    event.returnValue = true;
                }
            }
            else {
                event.returnValue = false;
            }
        }
        else {
            event.returnValue = false;
        }
    }
}


function OnKeyPressTextHora(Text) {
    var e_k = event.keyCode;
    if (e_k >= 48 && e_k <= 57) //48-57 es 0 al 9
    {
        event.returnValue = true;
    }
    else {
        if (e_k == 58) {
            if (Text.value != "") {
                indicepunto = Text.value.indexOf(':');
                if ((indicepunto != -1))  //e_k=46 es el '.'
                {
                    event.returnValue = false;
                }
                else {
                    event.returnValue = true;
                }
            }
            else {
                event.returnValue = false;
            }
        }
        else {
            event.returnValue = false;
        }
    }
}


function OnKeyPressTextLetrasNumeros(e) {
    var target = (e.target ? e.target : e.srcElement);
    var key = (e ? e.keyCode || e.which : window.event.keyCode);
    return (key <= 12 || (key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122) || (key == 32) || (key >= 164 && key <= 165));
}

function OnKeyPressTextNumerosLetras(CaracterPermitido) {
    var e_k = event.keyCode;
    if (e_k >= 48 && e_k <= 57 || e_k >= 97 && e_k <= 122 || e_k >= 65 && e_k <= 90 || e_k == 241 || e_k == 209) {
        event.returnValue = true;
    }
    else {
        if (String.fromCharCode(e_k) == CaracterPermitido) {
            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }
}

function OnKeyPressPlaca() {
    var e_k = event.keyCode;
    if ((e_k >= 48 && e_k <= 58) || (e_k >= 65 && e_k <= 90) || (e_k >= 97 && e_k <= 122) || (e_k == 45)) {
        if (e_k >= 97 && e_k <= 122) //Aquí se cambia automáticamente si es minúscula a MAYUSCULA
        {
            event.keyCode = event.keyCode - 32;
            event.returnValue = true;
        }
    }
    else {
        event.returnValue = false;
    }
}

function OnKeyPressTextNumerosBinarios(Text) {
    var e_k = event.keyCode;
    if (String.fromCharCode(e_k) == "0") {
        event.returnValue = true;
    }
    else {
        if (String.fromCharCode(e_k) == "1") {
            event.returnValue = true;
        }
        else
            event.returnValue = false;
    }
}

function ValidaDecimal(Text) {
    if (Text.value != "") {
        indicepunto = Text.value.indexOf('.');
        if (indicepunto != -1) {
            if (Text.value.length <= 2) {
                alert("El Formato Decimal ingresado es incorrecto. Ej.50.22");
                Text.focus();
                return false;
            }
            else {
                if (Text.value.length - 1 == indicepunto) //El Punto Esta al Final
                {
                    alert("El Formato Decimal ingresado es incorrecto. Ej.50.22");
                    Text.focus();
                    return false;
                }
                else {
                    if (parseFloat(Text.value) == 0) {
                        alert("El Valor ingresado no puede ser 0.");
                        Text.focus();
                        return false;
                    }
                }
            }

        }
        else {
            if (parseFloat(Text.value) == 0) {
                alert("El Valor ingresado no puede ser 0.");
                Text.focus();
                return false;
            }
        }
    }
}

function ValidaPorcentaje(Text) {
    if (Text.value != "") {
        indicepunto = Text.value.indexOf('.');
        if (indicepunto != -1) {
            if (Text.value.length <= 2) {
                alert("El Formato ingresado es incorrecto. Ej.50.22");
                Text.focus();
                return false;
            }
            else {
                if (Text.value.length - 1 == indicepunto) //El Punto Esta al Final
                {
                    alert("El Formato ingresado es incorrecto. Ej.50.22");
                    Text.focus();
                    return false;
                }
                else {
                    if (parseFloat(Text.value) == 0) {
                        alert("El Valor ingresado no puede ser 0.");
                        Text.focus();
                        return false;
                    }
                    if (parseFloat(Text.value) >= 100) {
                        alert("El Valor ingresado no puede ser mayor igual a 100.");
                        Text.focus();
                        return false;
                    }
                }
            }

        }
        else {
            if (parseFloat(Text.value) == 0) {
                alert("El Valor ingresado no puede ser 0.");
                Text.focus();
                return false;
            }
            if (parseFloat(Text.value) >= 100) {
                alert("El Valor ingresado no puede ser mayor igual a 100.");
                Text.focus();
                return false;
            }
        }
    }
}

function ValidaComa(Text) {
    if (Text.value != "") {
        indicepunto = Text.value.indexOf(',');
        if (indicepunto != -1) {
            if (Text.value.length <= 2) {
                alert("El Formato ingresado es incorrecto. Ej.1,4");
                Text.focus();
                return false;
            }
            else {
                if (Text.value.length - 1 == indicepunto) //La Coma esta al Final
                {
                    alert("El Formato ingresado es incorrecto. Ej.1,4");
                    Text.focus();
                    return false;
                }
                else {
                    if (parseFloat(Text.value) == 0) {
                        alert("El Formato ingresado es incorrecto. No debe ingresar cero. Ej.1,4");
                        Text.focus();
                        return false;
                    }
                    var matrizasientos;
                    matrizasientos = Text.value;
                    matrizasientos = matrizasientos.split(',')
                    if (matrizasientos[0] > matrizasientos[1]) {
                        alert("El Formato ingresado es incorrecto. El numero a la derecha de la coma debe ser mayor igual al izquierdo. Ej.1,4");
                        Text.focus();
                        return false;
                    }
                }
            }

        }
        else {
            if (parseFloat(Text.value) == 0) {
                alert("El Formato ingresado es incorrecto. No debe ingresar cero. Ej.1,4");
                Text.focus();
                return false;
            }
            else {
                alert("El Formato ingresado es incorrecto. Ej.1,4");
                Text.focus();
                return false;
            }
        }
    }
}


function ValidaText(Text, Mensaje) {
    if (Text.value.length == 0) {
        alert(Mensaje);
        Text.focus();
        return true //Longitud de campo 0
    }
    else {
        return false //Longitud de campo mas de 0
    }
}

function ConvierteMayuscula(Text) {
    Text.value = Text.value.toUpperCase();
}

function ValidaCombo(Combo, Mensaje) {
    if (Combo.value == 0) {
        alert(Mensaje);
        Combo.focus();
        return true;
    }
    else {
        return false;
    }
}

function ValidaNroCaracteres(Text, Cantidad) {
    var e_k = event.keyCode;
    if (Text.value.length > Cantidad - 1) {
        if ((e_k == 8) || (e_k >= 37 && e_k <= 40) || (e_k == 46))
            event.returnValue = true;
        else
            event.returnValue = false;
    }
}

function ValidaCantidadCaracteres(Text, Cantidad) {
    if (Text.value.length > Cantidad - 1) {
        Text.value = Text.value.substring(0, Cantidad - 1);
    }
}



function AbreVentana(Url) {
    window.open(Url, 'new', 'scrollbars=yes, resizeable= no, status=no, toolbar=no, top=200,width=550, location=no, left=250,height=330');
}


/*function FechaMayor(Fecha1,Fecha2)
{
if (Date.parseString(Fecha1).isAfter(Date.parseString(Fecha2))){ 
return true
}else
{
return false	
}

}*/


function FechaMayor(Obj1, Obj2) {
    String1 = Obj2;
    String2 = Obj1;

    // Si los dias y los meses llegan con un valor menor que 10 
    // Se concatena un 0 a cada valor dentro del string 
    if (String1.substring(1, 2) == "/") {
        String1 = "0" + String1
    }
    if (String1.substring(4, 5) == "/") {
        String1 = String1.substring(0, 3) + "0" + String1.substring(3, 9)
    }

    if (String2.substring(1, 2) == "/") {
        String2 = "0" + String2
    }
    if (String2.substring(4, 5) == "/") {
        String2 = String2.substring(0, 3) + "0" + String2.substring(3, 9)
    }

    dia1 = String1.substring(0, 2);
    mes1 = String1.substring(3, 5);
    anyo1 = String1.substring(6, 10);
    dia2 = String2.substring(0, 2);
    mes2 = String2.substring(3, 5);
    anyo2 = String2.substring(6, 10);


    if (dia1 == "08") // parseInt("08") == 10 base octogonal
        dia1 = "8";
    if (dia1 == '09') // parseInt("09") == 11 base octogonal
        dia1 = "9";
    if (mes1 == "08") // parseInt("08") == 10 base octogonal
        mes1 = "8";
    if (mes1 == "09") // parseInt("09") == 11 base octogonal
        mes1 = "9";
    if (dia2 == "08") // parseInt("08") == 10 base octogonal
        dia2 = "8";
    if (dia2 == '09') // parseInt("09") == 11 base octogonal
        dia2 = "9";
    if (mes2 == "08") // parseInt("08") == 10 base octogonal
        mes2 = "8";
    if (mes2 == "09") // parseInt("09") == 11 base octogonal
        mes2 = "9";

    dia1 = parseInt(dia1);
    dia2 = parseInt(dia2);
    mes1 = parseInt(mes1);
    mes2 = parseInt(mes2);
    anyo1 = parseInt(anyo1);
    anyo2 = parseInt(anyo2);

    if (anyo1 > anyo2) {
        return false;
    }

    if ((anyo1 == anyo2) && (mes1 > mes2)) {
        return false;
    }
    if ((anyo1 == anyo2) && (mes1 == mes2) && (dia1 > dia2)) {
        return false;
    }

    return true;
}


function SelectAllChkGrilla(NombreGrilla, NombreCheck, Inicio) {
    var i;
    var Check;
    oTabla = document.getElementById(NombreGrilla);
    total_registros = oTabla.rows.length;
    if (total_registros != 2) {
        var chequeadosdeschequeados;
        chequeadosdeschequeados = 0;
        var checksbloqueados;
        checksbloqueados = 0;
        //for (i=3;i<=total_registros; i++)		
        for (i = Inicio; i <= total_registros; i++) {
            Check = NombreGrilla + "__ctl" + i + "_" + NombreCheck
            if (document.getElementById(Check).disabled == true) {
                checksbloqueados = checksbloqueados + 1;
            }
            else {
                if (document.getElementById(Check).checked == false) {
                    chequeadosdeschequeados = chequeadosdeschequeados + 1;
                }
            }
        }
        //if (chequeadosdeschequeados==total_registros-2-checksbloqueados) //total de registros activos
        if (chequeadosdeschequeados == total_registros - checksbloqueados) {
            for (i = Inicio; i <= total_registros; i++) {
                Check = NombreGrilla + "__ctl" + i + "_" + NombreCheck
                if (document.getElementById(Check).disabled == false) {
                    document.getElementById(Check).checked = true;
                }
            }
        }
        else {
            for (i = Inicio; i <= total_registros; i++) {
                Check = NombreGrilla + "__ctl" + i + "_" + NombreCheck
                if (document.getElementById(Check).disabled == false) {
                    if (document.getElementById(Check).checked == true) {
                        document.getElementById(Check).checked = false
                    }
                    else {
                        document.getElementById(Check).checked = true
                    }
                }
            }
        }
        //ValueCheck=!ValueCheck;				
    }
}


/*function MostrarDiv(Obj,left,top,Mensaje)
{	
var Div=document.getElementById(Obj);
Div.style.display='block';
Div.style.visibility='visible';
Div.style.pixelLeft=left
Div.style.pixelTop=top	
Div.innerHTML=Mensaje
}*/

function MostrarDiv(Obj) {
    var Div = document.getElementById(Obj);
    Div.style.display = 'block';
    Div.style.visibility = 'visible';
}

function OcultarDiv(Obj) {
    var Div = document.getElementById(Obj);
    Div.style.display = 'none';
    Div.style.visibility = 'hidden'
}

function ColocarMensajeDiv(obj, Div, Mensaje) {
    var curleft = curtop = 0;
    obj = document.getElementById(obj);
    obj.focus();
    if (obj.offsetParent) {
        curleft = obj.offsetLeft
        curtop = obj.offsetTop
        while (obj = obj.offsetParent) {
            curleft += obj.offsetLeft
            curtop += obj.offsetTop
        }
    }

    MostrarDiv(Div, curleft, curtop + 15, Mensaje)
    setTimeout("OcultarDiv('" + Div + "')", 1750)
}

function RetornaFechaActual() {
    var fecha_actual = new Date()
    var Dia = fecha_actual.getDate();
    var Mes = fecha_actual.getMonth() + 1;
    var Anio = fecha_actual.getYear();
    if (Anio < 1900)
        Anio += 1900
    if (Mes < 10)
        Mes = "0" + Mes
    if (Dia < 10)
        Dia = "0" + Dia
    Fecha = Dia + "/" + Mes + "/" + Anio
    return Fecha;
}


function valida_correo(email) {
    regx = /^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$/;
    return regx.test(email);
}

function OnKeyPressTextNumerosTelef() {
    var e_k = event.keyCode;
    if ((e_k >= 48 && e_k <= 57) || (e_k >= 40 && e_k <= 41) || (e_k == 45)) //48-57 es 0 al 9, 40 y 41 es (),45 es -
    {
        event.returnValue = true;
    }
    else {
        event.returnValue = false;
    }
}

function AbrirVentanaPDF(url) {
    var vValorRetorno;

    if (url != null) {

        vValorRetorno = window.open(url, 'new', "");

    }
    if (vValorRetorno != null) {
        ActualizarCliente(vValorRetorno);
    }
}

//CMB INC 77 20111226
function SelectAllCheckBoxGridView(CheckBoxControl, GridViewElement) {
    if (CheckBoxControl.checked == true) {
        var i;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            if ((document.forms[0].elements[i].type == 'checkbox') &&
		        (document.forms[0].elements[i].name.indexOf(GridViewElement) > -1)) {
                if (document.forms[0].elements[i].disabled != true) {
                    document.forms[0].elements[i].checked = true;
                }
            }
        }
    }
    else {
        var i;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            if ((document.forms[0].elements[i].type == 'checkbox') &&
		        (document.forms[0].elements[i].name.indexOf(GridViewElement) > -1)) {
                document.forms[0].elements[i].checked = false;
            }
        }
    }
}

function addCommas(nStr) {
    var numerodecimales = 2;
    nStr += '';
    if (nStr.indexOf('.') == -1) {
        nStr += '.00';
    }
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + (x2 + '00').substring(0, numerodecimales + 1);
}


