<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoMasivoOperacionFT.aspx.vb"
    Inherits="Modulos_Inversiones_frmIngresoMasivoOperacionFT" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Operaci&oacute;n Futuros</title>
    <script language="javascript" type="text/javascript">
        function cambio(myElem) {
            var elem = myElem.id.split('_');
            var elem2 = elem;
            var elem3 = elem;
            var elem4 = elem;
            elem = elem[0] + '_' + elem[2] + '_' + 'hdCambio';
            elem2 = elem2[0] + '_' + elem2[2] + '_' + 'hdCambioTraza';
            elem4 = elem4[0] + '_' + elem4[2] + '_' + 'hdCambioTrazaFondo';
            document.getElementById(elem).value = "1";
            if ((elem3[3] == "tbNemonico") || (elem3[3] == "ddlOperacion") || (elem3[3] == "tbCantidad") || (elem3[3] == "tbPrecio") || (elem3[3] == "tbIntermediario") ||
                    (elem3[3] == "tbCantidadOperacion") || (elem3[3] == "tbPrecioOperacion") || (elem3[3] == "tbFondo1") || (elem3[3] == "tbFondo3") || (elem3[3] == "tbFondo3"))
                document.getElementById(elem2).value = "1";
            if ((elem3[3] == "tbFondo1") || (elem3[3] == "tbFondo3") || (elem3[3] == "tbFondo3"))
                document.getElementById(elem4).value = "1";
        }
        function showPopupMnemonico() {
            window.showModalDialog('../Parametria/HelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoFuturo', DatosDialog, 'dialogHeight:620px;dialogWidth:500px;status:no;unadorned:yes;help:No');
            document.getElementById('tbCodigoMnemonico').innerText = DatosDialog.Codigo;
        }

        function showPopupMnemonicoGrilla(myElem) {
            var DatosDialog = new Object;
            DatosDialog.Codigo = "";
            DatosDialog.Descripcion = "";
            DatosDialog.contractZise = "";


            window.showModalDialog('../Parametria/HelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoFuturo', DatosDialog, 'dialogHeight:620px;dialogWidth:500px;status:no;unadorned:yes;help:No');

            var elem = myElem.id.split('_');
            var elem2 = elem;
            var elem3 = elem;
            var elem4 = elem; //Migracion CMB 20120809
            var elem5 = elem; //JHC REQ 66056: Implementacion Futuros
            elem = elem[0] + '_' + elem[2] + '_' + 'tbNemonico';
            elem2 = elem2[0] + '_' + elem2[2] + '_' + 'tbInstrumento';
            elem3 = elem3[0] + '_' + elem3[2] + '_' + 'hdClaseInstrumento';
            elem4 = elem4[0] + '_' + elem4[2] + '_' + 'hdNemonico'; //Migracion CMB 20120809
            elem5 = elem5[0] + '_' + elem5[2] + '_' + 'hdContractZise'; //JHC REQ 66056: Implementacion Futuros

            document.getElementById(elem).innerText = DatosDialog.Codigo;
            document.getElementById(elem2).innerText = DatosDialog.CodigoSBS;
            document.getElementById(elem4).value = DatosDialog.Codigo; //Migracion CMB 20120809
            document.getElementById(elem5).value = DatosDialog.contractZise; //JHC REQ 66056: Implementacion Futuros
            document.getElementById(elem3).value = '';
        }

        function showPopupMnemonicoGrillaF(myElem) {
            var DatosDialog = new Object;
            DatosDialog.Codigo = "";
            DatosDialog.Descripcion = "";
            DatosDialog.contractZise = ""; //JHC REQ 66056: Implementacion Futuros

            window.showModalDialog('../Parametria/HelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoFuturo', DatosDialog, 'dialogHeight:620px;dialogWidth:500px;status:no;unadorned:yes;help:No');

            var elem = myElem.id.split('_');
            var elem2 = elem;
            var elem3 = elem;
            elem = elem[0] + '_' + elem[2] + '_' + 'tbNemonicoF';
            elem2 = elem2[0] + '_' + elem2[2] + '_' + 'hdContractZiseF'; //JHC REQ 66056: Implementacion Futuros
            elem3 = elem3[0] + '_' + elem3[2] + '_' + 'hdClaseInstrumentoF';

            document.getElementById('hdnOperador').value = DatosDialog.Codigo
            document.getElementById(elem).value = DatosDialog.Codigo;
            document.getElementById(elem2).value = DatosDialog.contractZise; //JHC REQ 66056: Implementacion Futuros
            document.getElementById(elem3).value = '';
        }

        function ShowPopupTercerosGrilla(myElem) {
            var DatosDialog = new Object;
            DatosDialog.Codigo = "";
            DatosDialog.Descripcion = "";

            window.showModalDialog('../Parametria/HelpControlParametria.aspx?tlbBusqueda=Terceros', DatosDialog, 'dialogHeight:620px;dialogWidth:500px;status:no;unadorned:yes;help:No');

            var elem = myElem.id.split('_');
            var elem2 = elem;
            var elem3 = elem; //Migracion CMB 20120809
            var elem4 = elem; //Migracion CMB 20120809
            elem = elem[0] + '_' + elem[2] + '_' + 'tbIntermediario';
            elem2 = elem2[0] + '_' + elem2[2] + '_' + 'hdIntermediario';
            elem3 = elem3[0] + '_' + elem3[2] + '_' + 'hdDescTercero'; //Migracion CMB 20120809

            document.getElementById(elem).innerText = DatosDialog.Descripcion;
            document.getElementById(elem2).value = DatosDialog.Codigo;
            document.getElementById(elem3).value = DatosDialog.Descripcion; //Migracion CMB 20120809
            cambio(myElem)
        }

        function ShowPopupTercerosGrillaF(myElem) {
            var DatosDialog = new Object;
            DatosDialog.Codigo = "";
            DatosDialog.Descripcion = "";

            window.showModalDialog('../Parametria/HelpControlParametria.aspx?tlbBusqueda=Terceros', DatosDialog, 'dialogHeight:620px;dialogWidth:500px;status:no;unadorned:yes;help:No');

            var elem = myElem.id.split('_');
            var elem2 = elem;

            elem = elem[0] + '_' + elem[2] + '_' + 'tbIntermediarioF';
            elem2 = elem2[0] + '_' + elem2[2] + '_' + 'hdIntermediarioF';
            document.getElementById('hdnIntermediario').value = DatosDialog.Descripcion;
            document.getElementById(elem).innerText = DatosDialog.Descripcion;
            document.getElementById(elem2).value = DatosDialog.Codigo;

        }
        function Salir() {
            location.href = "../../Bienvenida.aspx";
        }
        function precio_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[2] + '_' + 'tbCantidad';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotal';

            var precio = document.getElementById(myElem.id).value;
            var cantidad = String(document.getElementById(elem).value);
            cantidad = cantidad.replace(/,/g, "");

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            formatTotal(document.getElementById(resultado));
        }

        function precioF_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var elemCE = elem;
            var elemPE = elem;
            var resultado = myElem.id.split('_');
            var resultadoE = resultado;
            elem = elem[0] + '_' + elem[2] + '_' + 'tbCantidadF';
            elemCE = elemCE[0] + '_' + elemCE[2] + '_' + 'tbCantidadOperacionF';
            elemPE = elemPE[0] + '_' + elemPE[2] + '_' + 'tbPrecioOperacionF';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotalF';
            resultadoE = resultadoE[0] + '_' + resultadoE[2] + '_' + 'tbTotalOperacionF';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = String(document.getElementById(elem).value);
            cantidad = cantidad.replace(/,/g, "")

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById('hdnTotalF').value = total
                document.getElementById(resultado).value = total;
                document.getElementById(resultadoE).value = total;
                document.getElementById(elemCE).value = cantidad;
                document.getElementById(elemPE).value = precio;
            }
            else {
                document.getElementById(resultado).value = 0;
                document.getElementById(resultadoE).value = 0;
            }
            formatTotal(document.getElementById(resultado));
            formatTotal(document.getElementById(resultadoE));
        }

        function precio_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[2] + '_' + 'tbCantidadOperacion';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotalOperacion';

            var precio = document.getElementById(myElem.id).value;
            var cantidad = String(document.getElementById(elem).value);
            cantidad = cantidad.replace(/,/g, "");

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            formatTotal(document.getElementById(resultado));
        }

        function precioF_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[2] + '_' + 'tbCantidadOperacionF';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotalOperacionF';

            var precio = document.getElementById(myElem.id).value;
            var cantidad = String(document.getElementById(elem).value);
            cantidad = cantidad.replace(/,/g, "")

            if (cantidad != "" && precio != "") {

                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
            }

            else {

                document.getElementById(resultado).value = 0;
            }

            formatTotal(document.getElementById(resultado));
        }

        function cantidad_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            var hdconZise = myElem.id.split('_');
            var tbtotExpo = myElem.id.split('_');
            elem = elem[0] + '_' + elem[2] + '_' + 'tbPrecio';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotal';
            hdconZise = hdconZise[0] + '_' + hdconZise[2] + '_' + 'hdContractZise';
            tbtotExpo = tbtotExpo[0] + '_' + tbtotExpo[2] + '_' + 'tbTotalExposicion';
            var cantidad = String(document.getElementById(myElem.id).value);
            var precio = document.getElementById(elem).value;
            var conZise = document.getElementById(hdconZise).value;
            cantidad = cantidad.replace(/,/g, "");
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            if (cantidad != "" && conZise != "") {
                var totExpo = precio * conZise;
                totExpo = totExpo.toFixed(2);
                document.getElementById(tbtotExpo).value = totExpo;
            }
            else {
                document.getElementById(tbtotExpo).value = 0;
            }
            formatTotal(document.getElementById(resultado));
        }

        function cantidadF_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var elemPE = elem;
            var elemCE = elem;
            var resultado = myElem.id.split('_');
            var resultadoE = resultado;
            var resCont = resultado; //JHC REQ 66056: Implementacion Futuros
            var eleExpo = elem; //JHC REQ 66056: Implementacion Futuros
            var totalExpo = 0; //JHC REQ 66056: Implementacion Futuros
            elem = elem[0] + '_' + elem[2] + '_' + 'tbPrecioF';
            elemPE = elemPE[0] + '_' + elemPE[2] + '_' + 'tbPrecioOperacionF';
            elemCE = elemCE[0] + '_' + elemCE[2] + '_' + 'tbCantidadOperacionF';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotalF';
            resultadoE = resultadoE[0] + '_' + resultadoE[2] + '_' + 'tbTotalOperacionF';
            resCont = resCont[0] + '_' + resCont[2] + '_' + 'hdContractZiseF'; //JHC REQ 66056: Implementacion Futuros
            eleExpo = eleExpo[0] + '_' + eleExpo[2] + '_' + 'tbTotalExposicionF'; //JHC REQ 66056: Implementacion Futuros        
            var cantidad = String(document.getElementById(myElem.id).value);
            var precio = document.getElementById(elem).value;
            var expo = document.getElementById(resCont).value.replace(',', '');
            cantidad = cantidad.replace(/,/g, "");
            if (cantidad != "" && expo != "") { //JHC REQ 66056: Implementacion Futuros
                totalExpo = cantidad * expo; //JHC REQ 66056: Implementacion Futuros
                document.getElementById(eleExpo).value = totalExpo; //JHC REQ 66056: Implementacion Futuros
            } //JHC REQ 66056: Implementacion Futuros
            else {
                document.getElementById(eleExpo).value = 0; //JHC REQ 66056: Implementacion Futuros
            }
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
                document.getElementById(resultadoE).value = total;
                document.getElementById(elemPE).value = precio;
                document.getElementById(elemCE).value = cantidad;
            }
            else {
                document.getElementById(resultado).value = 0;
                document.getElementById(resultadoE).value = 0;
            }
            formatTotal(document.getElementById(resultado));
            formatTotal(document.getElementById(resultadoE));
            formatTotal(document.getElementById(resCont)); //JHC REQ 66056: Implementacion Futuros
        }

        function cantidad_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            var resCont = elem; //JHC REQ 66056: Implementacion Futuros
            var eleExpo = elem; //JHC REQ 66056: Implementacion Futuros
            var totalExpo = 0; //JHC REQ 66056: Implementacion Futuros
            elem = elem[0] + '_' + elem[2] + '_' + 'tbPrecioOperacion';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotalOperacion';
            resCont = resCont[0] + '_' + resCont[2] + '_' + 'hdContractZiseF'; //JHC REQ 66056: Implementacion Futuros
            eleExpo = eleExpo[0] + '_' + eleExpo[2] + '_' + 'tbTotalExposicionF'; //JHC REQ 66056: Implementacion Futuros
            var cantidad = String(document.getElementById(myElem.id).value);
            var precio = document.getElementById(elem).value;
            var expo = document.getElementById(resCont).value.replace(',', '');
            cantidad = cantidad.replace(/,/g, "");
            if (cantidad != "" && expo != "") { //JHC REQ 66056: Implementacion Futuros
                var totalExpo = cantidad * expo; //JHC REQ 66056: Implementacion Futuros
                document.getElementById(eleExpo).value = totalExpo; //JHC REQ 66056: Implementacion Futuros
            }
            else {
                document.getElementById(eleExpo).value = 0; //JHC REQ 66056: Implementacion Futuros
            }
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            formatTotal(document.getElementById(resultado));
            formatTotal(document.getElementById(resCont)); //JHC REQ 66056: Implementacion Futuros
        }

        function cantidadF_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            var resCont = elem; //JHC REQ 66056: Implementacion Futuros
            var totalExpo = 0; //JHC REQ 66056: Implementacion Futuros
            var eleExpo = elem; //JHC REQ 66056: Implementacion Futuros
            elem = elem[0] + '_' + elem[2] + '_' + 'tbPrecioOperacionF';
            resultado = resultado[0] + '_' + resultado[2] + '_' + 'tbTotalOperacionF';
            resCont = resCont[0] + '_' + resCont[2] + '_' + 'hdContractZiseF'; //JHC REQ 66056: Implementacion Futuros
            eleExpo = eleExpo[0] + '_' + eleExpo[2] + '_' + 'tbTotalExposicionF'; //JHC REQ 66056: Implementacion Futuros   
            var cantidad = String(document.getElementById(myElem.id).value);
            var precio = document.getElementById(elem).value;
            var expo = document.getElementById(resCont).value.replace(',', '');
            cantidad = cantidad.replace(/,/g, "");
            if (cantidad != "" && expo != "") { //JHC REQ 66056: Implementacion Futuros
                var totalExpo = cantidad * expo; //JHC REQ 66056: Implementacion Futuros
                document.getElementById(eleExpo).value = totalExpo; //JHC REQ 66056: Implementacion Futuros
            }
            else {
                document.getElementById(eleExpo).value = 0; //JHC REQ 66056: Implementacion Futuros
            }
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                total = total.toFixed(2);
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            formatTotal(document.getElementById(resultado));
            formatTotal(document.getElementById(resCont)); //JHC REQ 66056: Implementacion Futuros
        }

        function formatCurrencyAccionesOperacion(myElem) {
            var num = document.getElementById(myElem.id).value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '00'
                var tmp2 = tmp1.substr(0, 2);

                num = num.toString().replace(/$|,/g, '');

                if (isNaN(num))
                    num = "0";

                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();

                if (cents < 10) {
                    cents = "0" + cents + '00';
                    cents = cents.substr(0, 2);
                }
                else
                { cents = tmp2; }

                if (pos1 == -1) {
                    tmp2 = '00';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);
                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);

            }

            return false;

        }
        function formatTotal(myElem) {
            var num = myElem.value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '00'
                var tmp2 = tmp1.substr(0, 2);

                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '00';
                    cents = cents.substr(0, 2);
                }
                else
                { cents = tmp2; }

                if (pos1 == -1) {
                    tmp2 = '00';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                myElem.value = (((sign) ? '' : '-') + num + '.' + tmp2);
                myElem.value = (((sign) ? '' : '-') + num + '.' + tmp2);

            }
            return false;

        }

        function formatCurrencyAcciones(myElem) {
            var num = document.getElementById(myElem.id).value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000000'
                var tmp2 = tmp1.substr(0, 7);

                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
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

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));


                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;

        }

        function formatCurrencyPrecio(myElem) {
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


        function SelectAll(CheckBoxControl) {
            var i;
            if (CheckBoxControl.checked == true) {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
		(document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
		(document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function BorrarHiddens() {
            document.getElementById('hdnTotalF').value = "";
            document.getElementById('hdnIntermediario').value = "";
            document.getElementById('hdnOperador').value = "";
        }

        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#ibExportar").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Registro Masivo de Operaciones Futuros</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" />
                                <asp:LinkButton ID="lkbMnemonico" runat="server" OnClientClick="showPopupMnemonico()"><span class="add-on"><i
                                    class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Operador</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlOperador" Width="150px  " />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseInstrumento" Width="280px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Estado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlEstado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <asp:Button Text="Grabar" runat="server" ID="btnGrabar" />
            <asp:Button Text="Validar" runat="server" ID="btnValidar" />
            <asp:Button Text="Validar Exc. Trader" runat="server" ID="btnValidarTrader" />
            <asp:Button Text="Ejecutar" runat="server" ID="btnAprobar" />
            <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
            <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
        </div>
        <br />
        <div class="grilla-footer">
            <asp:GridView runat="server" SkinID="GridFooter" ID="Datagrid1">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <HeaderTemplate>
                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            <asp:ImageButton ID="Imagebutton1" runat="server" SkinID="imgDelete" CommandName="_Delete"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") %>'
                                AlternateText="Eliminar"></asp:ImageButton>
                            <asp:Label ID="lbCodigoPrevOrden" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") %>'
                                Visible="False"></asp:Label>
                            <asp:Label ID="lbClase" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Clase") %>'
                                Visible="False"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:ImageButton ID="ImageButton2" runat="server" SkinID="imgAdd" CommandName="_Add"
                                AlternateText="Agregar"></asp:ImageButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Correlativo" HeaderText="N&#176;" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Hora Orden">
                        <ItemTemplate>
                            <asp:TextBox ID="tbHora" runat="server" Width="57px" Text='<%# DataBinder.Eval(Container, "DataItem.HoraOperacion") %>'
                                MaxLength="8">
                            </asp:TextBox>
                            <input id="hdCambio" type="hidden" name="hdCambio" runat="server" />
                            <input id="hdCambioTraza" type="hidden" name="hdCambioTraza" runat="server" />
                            <input id="hdCambioTrazaFondo" type="hidden" name="hdCambioTrazaFondo" runat="server" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbHoraF" Width="57px" runat="server" MaxLength="8"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Operador">
                        <ItemTemplate>
                            <asp:TextBox ID="tbOperador" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.UsuarioCreacion") %>'
                                Enabled="False">
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbOperadorF" Width="70px" runat="server" Enabled="False"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nem&#243;nico">
                        <ItemTemplate>
                            <asp:TextBox ID="tbNemonico" Width="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CodigoNemonico") %>'
                                ReadOnly="True">
                            </asp:TextBox>&nbsp;
                            <asp:ImageButton Style="z-index: 0" ID="ibBNemonico" runat="server" CommandName="Item"
                                SkinID="imgSearch"></asp:ImageButton>
                            <input type="hidden" id="hdNemonico" runat="server" />
                            <input id="hdClaseInstrumento" type="hidden" name="hdClaseInstrumentoF" runat="server" />
                            <input id="hdContractZise" type="hidden" name="hdClaseInstrumento" runat="server"
                                value='<%# DataBinder.Eval(Container,"DataItem.ContractSize") %>' />
                            <input type="hidden" id="hdNemonicoTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoNemonico") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbNemonicoF" Width="90px" runat="server" ReadOnly="True"></asp:TextBox>&nbsp;
                            <asp:ImageButton Style="z-index: 0" ID="ibBNemonicoF" runat="server" CommandName="Footer"
                                SkinID="imgSearch"></asp:ImageButton>
                            <input id="hdClaseInstrumentoF" type="hidden" name="hdClaseInstrumentoF" runat="server" />
                            <input id="hdContractZiseF" type="hidden" name="hdContractZiseF" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vencimiento">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlVencimientoMes" runat="server" Width="100px">
                            </asp:DropDownList>
                            <asp:Label ID="lbVencimientoMes" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VencimientoMes") %>'
                                Visible="False">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlVencimientoMesF" runat="server" Width="100px">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Año">
                        <ItemTemplate>
                            <asp:TextBox ID="tbVencimientoAno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VencimientoAno") %>'
                                Style="text-align: right; width: 50px;">
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbVencimientoAnoF" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VencimientoAno") %>'
                                Style="text-align: right; width: 50px;">
                            </asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Operaci&#243;n">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlOperacion" runat="server" Width="100px">
                            </asp:DropDownList>
                            <asp:Label ID="lbOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>'
                                Visible="False">
                            </asp:Label>
                            <input type="hidden" id="hdOperacionTrz" runat="server" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlOperacionF" runat="server" Width="100px">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cant. Instrumento">
                        <ItemTemplate>
                            <asp:TextBox ID="tbCantidad" runat="server" Text='<%# String.Format("{0:##,##0}",DataBinder.Eval(Container, "DataItem.Cantidad")) %>'
                                onblur='javascript:cantidad_CalcularMonto(this);' Style="text-align: right">
                            </asp:TextBox>
                            <input type="hidden" id="hdCantidadTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Cantidad") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbCantidadF" runat="server" onblur='javascript:cantidadF_CalcularMonto(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio">
                        <ItemTemplate>
                            <asp:TextBox ID="tbPrecio" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Precio") %>'
                                onblur='javascript:precio_CalcularMonto(this);'>
                            </asp:TextBox>
                            <input type="hidden" id="hdPrecioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Precio") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbPrecioF" runat="server" onblur='javascript:precioF_CalcularMonto(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Orden">
                        <ItemTemplate>
                            <asp:TextBox ID="tbTotal" runat="server" Text='<%# String.Format("{0:##,##0.00}",DataBinder.Eval(Container, "DataItem.Total")) %>'
                                ReadOnly="True" Style="text-align: right">
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbTotalF" runat="server" ReadOnly="True" onblur='formatCurrencyAccionesOperacion(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Exposición">
                        <ItemTemplate>
                            <asp:TextBox ID="tbTotalExposicion" runat="server" Text='<%# String.Format("{0:##,##0.00}",DataBinder.Eval(Container, "DataItem.TotalExposicion")) %>'
                                Style="text-align: right">
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbTotalExposicionF" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Condici&#243;n">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlCondicion" runat="server" Width="120px">
                            </asp:DropDownList>
                            <asp:Label ID="lbTipoCondicion2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoCondicion") %>'
                                Visible="False">
                            </asp:Label>
                            <input id="hdTotal" type="hidden" name="hdTotal" runat="server" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlCondicionF" runat="server" Width="120px">
                            </asp:DropDownList>
                            <input id="hdTotalF" type="hidden" name="hdTotalF" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Intermediario">
                        <ItemTemplate>
                            <asp:TextBox ID="tbIntermediario" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>'
                                ReadOnly="True">
                            </asp:TextBox>&nbsp;
                            <asp:ImageButton ID="ibBIntermediario" runat="server" CommandName="Item" ImageUrl="../../Common/Imagenes/ico_Show.gif">
                            </asp:ImageButton>
                            <input id="hdIntermediario" value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>'
                                type="hidden" name="hdIntermediario" runat="server" />
                            <input type="hidden" id="hdDescTercero" runat="server" />
                            <input type="hidden" id="hdIntermediarioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbIntermediarioF" Width="200px" runat="server" ReadOnly="True"></asp:TextBox>&nbsp;
                            <asp:ImageButton ID="ibBIntermediarioF" runat="server" CommandName="Footer" ImageUrl="../../Common/Imagenes/ico_Show.gif"
                                Width="16px"></asp:ImageButton>
                            <input id="hdIntermediarioF" type="hidden" name="hdIntermediarioF" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contacto">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlContacto" runat="server" Width="150px">
                            </asp:DropDownList>
                            <asp:Label ID="lbContacto" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoContacto") %>'
                                Visible="False">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlContactoF" runat="server" Width="150px">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mercado">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlPlazaN" runat="server" Width="120px">
                            </asp:DropDownList>
                            <asp:Label ID="lbPlazaN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPlaza") %>'
                                Visible="False">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlPlazaNF" runat="server" Width="120px">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Vencimiento">
                        <ItemTemplate>
                            <asp:TextBox ID="tbFechaLiquidacion" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.FechaLiquidacion") %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbFechaLiquidacionF" runat="server" Width="70px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hora Ejecución">
                        <ItemTemplate>
                            <asp:TextBox ID="tbHoraEje" runat="server" Width="57px" Text='<%# DataBinder.Eval(Container, "DataItem.HoraEjecucion") %>'
                                MaxLength="8">
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbHoraEjeF" Width="57px" runat="server" MaxLength="8"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cant. Instrumento Ejecuci&#243;n">
                        <ItemTemplate>
                            <asp:TextBox ID="tbCantidadOperacion" runat="server" Text='<%# String.Format("{0:##,##0}",DataBinder.Eval(Container, "DataItem.CantidadOperacion")) %>'
                                onblur='javascript:cantidad_CalcularMontoOperacion(this);' Style="text-align: right">
                            </asp:TextBox>
                            <input type="hidden" id="hdCantidadOperacionTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CantidadOperacion") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbCantidadOperacionF" runat="server" onblur='javascript:cantidadF_CalcularMontoOperacion(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio Ejecuci&#243;n">
                        <ItemTemplate>
                            <asp:TextBox ID="tbPrecioOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PrecioOperacion") %>'
                                onblur='javascript:precio_CalcularMontoOperacion(this); formatCurrencyPrecio(this);'>
                            </asp:TextBox>
                            <input type="hidden" id="hdPrecioOperacionTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.PrecioOperacion") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbPrecioOperacionF" runat="server" onblur='javascript:precioF_CalcularMontoOperacion(this); formatCurrencyPrecio(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Ejecuci&#243;n">
                        <ItemTemplate>
                            <asp:TextBox ID="tbTotalOperacion" runat="server" Text='<%# String.Format("{0:##,##0.00}",DataBinder.Eval(Container, "DataItem.TotalOperacionRV")) %>'
                                Style="text-align: right">
                            </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbTotalOperacionF" runat="server" onblur='formatCurrencyAccionesOperacion(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fondo 1">
                        <ItemTemplate>
                            <asp:TextBox ID="tbFondo1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AsignacionF1") %>'
                                onblur='formatCurrencyAccionesOperacion(this);'>
                            </asp:TextBox>
                            <input type="hidden" id="hdFondo1Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.AsignacionF1") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbFondo1F" runat="server" onblur='formatCurrencyAccionesOperacion(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fondo 2">
                        <ItemTemplate>
                            <asp:TextBox ID="tbFondo2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AsignacionF2") %>'
                                onblur='formatCurrencyAccionesOperacion(this);'>
                            </asp:TextBox>
                            <input type="hidden" id="hdFondo2Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.AsignacionF2") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbFondo2F" runat="server" onblur='formatCurrencyAccionesOperacion(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fondo 3">
                        <ItemTemplate>
                            <asp:TextBox ID="tbFondo3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AsignacionF3") %>'
                                onblur='formatCurrencyAccionesOperacion(this);'>
                            </asp:TextBox>
                            <input type="hidden" id="hdFondo3Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.AsignacionF3") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbFondo3F" runat="server" onblur='formatCurrencyAccionesOperacion(this);'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Salir" runat="server" ID="btnSalir" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdFechaNegocio" />
    <asp:HiddenField runat="server" ID="hdPuedeNegociar" />
    <asp:HiddenField runat="server" ID="hdnIntermediario" />
    <asp:HiddenField runat="server" ID="hdnOperador" />
    <asp:HiddenField runat="server" ID="_PopUp" />
    <asp:HiddenField runat="server" ID="_RowIndex" />
    <asp:HiddenField runat="server" ID="_ControlID" />
    <asp:HiddenField runat="server" ID="hdnTotalF" />
    </form>
</body>
</html>
