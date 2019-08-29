<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGenerarReportes.aspx.vb" Inherits="Modulos_Parametria_Reportes_frmGenerarReportes" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reportes Composicion De Cartera</title>
    <script language="javascript" type="text/javascript">
        var strMensajeError = "";

        var strError = "";
        var strMensajeError = "";
        function CheckDateTimeFormat(pDateTime, pFormat) {
            strError = "";
            if (pDateTime == "" ||
      pFormat == "")
                return (true);

            var 
    Format = pFormat.toLowerCase(),
	DateTime = pDateTime.toLowerCase(),
    VerifyIndex = -1,
    ArrayFormats = new Array("am/pm", "dd", "d", "mm", "m", "yyyy", "yy", "hh", "h", "nn", "n", "ss", "s"),
	ArrayVerify = new Array(),
	i = 0, Pos, day = 1, month = 1, year = 2000, hour = 0, minute = 0, second = 0,
	ampm = "",
	FormatError = "",
	error = false;

            while (i < ArrayFormats.length) {
                Pos = Format.indexOf(ArrayFormats[i]);
                if (Pos > -1) {
                    ArrayVerify[++VerifyIndex] = ArrayFormats[i];
                    Format = Format.substring(0, Pos) + "~" + VerifyIndex + "~" +
	           Format.substring(Pos + ArrayFormats[i].length, Format.length);
                }
                else
                    i++;
            }

            IndexDT = 0;
            IndexFormat = 0;
            while ((IndexFormat < Format.length ||
          IndexDT < DateTime.length) &&
         !error) {
                if (IndexFormat >= Format.length ||
	    IndexDT >= DateTime.length) {
                    error = true;
                    FormatError = "\nDiferente Extensión.";
                }
                else if (Format.charAt(IndexFormat) != "~") {
                    error = Format.charAt(IndexFormat++) != DateTime.charAt(IndexDT++);
                    if (error)
                        FormatError = "\nCaracter " + IndexDT + " es inválido.";
                }
                else {
                    var 
	    Pos = Format.indexOf("~", IndexFormat + 1),
	    Index = parseInt(Format.substring(IndexFormat + 1, Pos), 10),
	    Text = ArrayVerify[Index],
		TextEval, number = "";

                    if (Text != "am/pm") {
                        if (Text != "yyyy") {
                            var c1, c2 = "";

                            c1 = DateTime.charAt(IndexDT);
                            if (IndexDT + 1 < DateTime.length &&
		      DateTime.charAt(IndexDT + 1) != Format.charAt(Pos + 1) &&
			  DateTime.charAt(IndexDT + 1) != " ")
                                c2 = DateTime.charAt(IndexDT + 1);

                            error = (isNaN(c1)) || (Text.length == 2 && IndexDT + 1 >= DateTime.length) ||
			      (Text.length == 2 && (c2 == "" || isNaN(c2)));

                            if (!error) {
                                number += c1;
                                if (c2 != "")
                                    number += c2;

                                if (Text == "d" || Text == "dd")
                                    day = parseInt(number, 10)
                                else if (Text == "m" || Text == "mm")
                                    month = parseInt(number, 10)
                                else if (Text == "yy")
                                    year = parseInt(number, 10)
                                else if (Text == "h" || Text == "hh")
                                    hour = parseInt(number, 10)
                                else if (Text == "n" || Text == "nn")
                                    minute = parseInt(number, 10)
                                else if (Text == "s" || Text == "ss")
                                    second = parseInt(number, 10)

                                IndexFormat = Pos + 1;
                                IndexDT += number.length;
                            }
                            else
                                FormatError = "\nValor de " + Text.toUpperCase() + " es inválido.";
                        }
                        else {
                            error = IndexDT + 3 >= DateTime.length ||
		          isNaN(DateTime.substring(IndexDT, IndexDT + 4));
                            if (!error) {
                                year = parseInt(DateTime.substring(IndexDT, IndexDT + 4), 10);
                                IndexFormat = Pos + 1;
                                IndexDT += 4;
                            }
                            else
                                FormatError = "\nValor de " + Text.toUpperCase() + " es inválido.";
                        }
                    }
                    else {
                        error = IndexDT + 1 >= DateTime.length ||
		        (DateTime.substring(IndexDT, IndexDT + 2) != "am" &&
				 DateTime.substring(IndexDT, IndexDT + 2) != "pm");

                        if (!error) {
                            ampm = DateTime.substring(IndexDT, IndexDT + 2);
                            IndexFormat = Pos + 1;
                            IndexDT += 2;
                        }
                        else
                            FormatError = "\nValor de " + Text.toUpperCase() + " es inválido.";
                    }
                }
            }

            if (!error) {
                if (month > 12 || month < 1)
                    FormatError = "\nMes fuera de Rango [1..12].";

                if (day < 1 || day > 31)
                    FormatError += "\nDia fuera de Rango [1..31].";

                if ((hour < 0 || hour > 23) && ampm == "")
                    FormatError += "\nHora fuera de Rango [0..23].";

                if (hour > 12 && ampm == "pm")
                    FormatError += "\nHora fuera de Rango  [0..12] for PM.";

                if (hour > 11 && ampm == "am")
                    FormatError += "\nHora fuera de Rango  [0..11] for AM.";

                if (minute < 0 || minute > 59)
                    FormatError += "\nMinuto fuera de Rango  [0..59].";

                if (second < 0 || second > 59)
                    FormatError += "\nSegundos fuera de Rango  [0..59].";

                if (month > 12 || month < 1 || day < 1 || day > 31 || hour < 0 ||
	    hour > 23 || (hour > 12 && ampm != "") || (hour > 11 && ampm == "am") ||
		minute < 0 || minute > 59 || second < 0 || second > 59)
                    error = true;

                if (!error)
                    switch (month) {
                    case 2:
                        if (Bisiesto(year)) {
                            if (day > 29) {
                                error = true;
                                FormatError += "\nDia fuera del rango [1..29] para el mes " + month +
			               " para el año bisiesto " + year + ".";
                            }
                        }
                        else if (day > 28) {
                            error = true;
                            FormatError += "\nDia fuera del rango [1..28] para el mes " + month + ".";
                        }
                        break;
                    case 4: case 6: case 9: case 11:
                        if (day > 30) {
                            error = true;
                            FormatError += "\nDia fuera del Rango [1..30] para el mes " + month + ".";
                        }
                        break;
                }
            }

            if (error) {
                //    alert("Formato de Fecha Inválido " + pFormat.toUpperCase() + "." + FormatError);
                //strError += "Formato de Fecha Inválido: " + pFormat.toUpperCase() + "." + FormatError + "\n";

                strError += FormatError + "\n";
                return (false);
            }
            else
                return (true);

        }

        function Bisiesto(year) {
            return (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        }

        function Trim(pString) {
            var i, resultado = "";
            pString = String(pString);
            if (pString.length > 0) {
                i = 0;
                while (pString.charAt(i) == " ") i++;
                resultado = pString.substring(i);

                i = resultado.length - 1;
                if (i > -1) {
                    while (resultado.charAt(i) == " ") i--;
                    resultado = resultado.substring(0, i + 1);
                }
            }
            return (resultado);
        }

        function ValidaFecha(control) {
            if (!CheckDateTimeFormat(control.value, 'dd/mm/yyyy')) {
                //control.value = "";
                return false;
            }
            return true;
        }


        function ValidaFechas() {
            var strMsjFechas = "";



            if (document.getElementById("<%= tbFechaInicio.ClientID %>").value != "") {
                if (!ValidaFecha(document.getElementById("<%= tbFechaInicio.ClientID %>")))
                    strMsjFechas += "-Fecha Inicio :" + strError

            }

            if (document.getElementById("<%= tbFechaFin.ClientID %>").value != "") {
                if (!ValidaFecha(document.getElementById("<%= tbFechaFin.ClientID %>")))
                    strMsjFechas += "-Fecha Fin :" + strError

            }


            if (strMsjFechas != "") {
                strMensajeError += "Formato de Fecha Incorrecto: DD/MM/YYYY\n" + strMsjFechas + "\n";
                return false;
            }
            else {
                return true;
            }
        }



        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%=tbFechaInicio.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Inicio\n"
            if (document.getElementById("<%=tbFechaFin.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Fin\n"


            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }


        function Validar() {
            strMensajeError = "";
            if (ValidaFechas()) {
                return true;
            }
            else {
                alert(strMensajeError);
                return false;
            }
        }

        function showPopupMnemonico() {
            return showModalDialog('../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Reportes de Parametría</h2></header>
    <fieldset>
    <legend>Selección de Reporte</legend>   
        <div class="row">
            <div class="col-md-9">
                <div>                
                    <div class="col-sm-9">
                	<asp:radiobuttonlist id="RbtnFiltro" runat="server" Width="535px" AutoPostBack="True"
								RepeatDirection="Horizontal" Font-Size="X-Small" Height="15px" BorderStyle="Inset" 
                            CssClass="Zoluxiones" BorderWidth="1px">
								<asp:ListItem Value="Indicadores" Selected="True">Indicadores</asp:ListItem>
								<asp:ListItem Value="Cuponera">Cuponera</asp:ListItem>
				    </asp:radiobuttonlist>
                    </div>    
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Indicador</label>
                        <div class="col-sm-9">
                        <asp:dropdownlist id="ddlIndicador" runat="server" Width="350px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Codigo Mnemónico</label>
                    <div class="col-sm-9">
                        <div class="input-append">
                            <asp:TextBox runat="server" ID="tbCodigoMnemonico" CssClass="input-medium" />
                            <asp:LinkButton ID="btnbuscar" runat="server" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha de Inicio</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha de Fin</label>
                            <div class="col-sm-9">
                                <div class="input-append date" style="text-align:justify ;">
                                    <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                    </div>
               </div>
       </div>

    </fieldset>
    <br />
    <header>
    </header>
        <div class="row"> 
            <div class="col-md-6">
            </div>
            <div class="col-md-6">
                <div class="form-group" style="float: right;">                    
                        <asp:Button Text="Imprimir" runat="server" ID="btnGenerar" />
                        <asp:Button Text="Salir" runat="server" ID="btnSalir" />                    
                </div>
            </div>             
        </div>
        <input id="hdnCodigo" type="hidden" name="hdnCodigo" runat="server" />
    </div>
    </form>
</body>
</html>
