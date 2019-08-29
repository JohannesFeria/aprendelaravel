<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmValorizacionSBS.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmValorizacionSBS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Valorización</title>

       <style type="text/css"> 
        .divGrilla {
            height: 420px;
            border: solid 1px #706f6f;
            overflow-y: auto;
            margin-bottom: 15px;
        }

        .ColumnsGrilla>tbody>tr>td>img {
            color: blue;
            height: 18px;
            width: 18px;
            margin: 2px 2px;
        }
        
        .selector
        {
            cursor: pointer;
        }
        
        .winMensajes
        {
            border: solid 1px #706f6f;
            padding: 5px;
            overflow-y: auto;
            margin-bottom: 15px;            
            }
        .winMensajes > .cabecera
        {
            font-size: 14px;
            }
        .winMensajes > .cabecera > .cerrar
        {
            width: 100px;
            }        
        .winMensajes > .cabecera > .titulo
        {
            margin-left: 10px;
            font-weight: bold;
            color: #824224;
            }                 
        .winMensajes > .grilla
        {
            border: solid 1px #706f6f
            }
        .winMensajes > .grilla > thead > tr > td
        {
            text-align: center;
            height: 22px;
            }
        .winMensajes > .grilla > tbody > tr
        {
            border-top: solid 1px #706f6f;
            }             
        .winMensajes > .grilla > tbody > tr > td
        {
            height: 22px;
            }              
    </style>
    <script type="text/javascript">
        function valorizar(idPortafolio) {
            var fechaOperacion = $('#tbFechaOperacion').val()
            var param = { IdPortafolio: idPortafolio, FechaOperacion: fechaOperacion };
            $.ajax({
                url: "../../../MetodosWeb.aspx/VectorPrecioPIP",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (data) {
                
                    if (data.d.length > 0) {

                        var res = data.d.split('-');
                        if (res.length > 1) {

                            var url = '';
                            url = "Reportes/frmVisorErrorValoracion.aspx?pportafolio=" + res[0] + "&pFecha=" + res[1];
                            showWindow(url, '800', '600');
                        }
                    }
                    else {
                        alertify.alert('<b>No hay precios cargados para este día en este Portafolio.</b>');
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        }
        function MostrarAlertaFechaUltimaValorizacion(idPortafolio) {
            var param = {idPortafolio:idPortafolio};
            $.ajax({
                url: "../../../MetodosWeb.aspx/GetFechaMaximaValorizacion",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (data) {
                    if (data.d.length > 0) {
                        alertify.alert('<b>No se puede valorizar el portafolio seleccionado, porque la fecha de la última valorización es: ' + data.d.toString() + ' .</b>');
                    }
                    else {
                        alertify.alert('<b>No se puede valorizar el portafolio seleccionado, porque no se pudo obtener la fecha de la última valorización.</b>');
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        }
        function MostrarAlertaFechaCajas(idPortafolio) {
            var fechaOperacion = $('#tbFechaOperacion').val()
            var param = { idPortafolio: idPortafolio, FechaOperacion: fechaOperacion };
            $.ajax({
                url: "../../../MetodosWeb.aspx/GetValidacionFechaCierreCajas",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (data) {
                    if (data.d.length > 0) {
                        alertify.alert('<b>' + data.d.toString() + '</b>');
                    }
                    else {
                        alertify.alert('<b>Hubo un error en la obtención de fecha de cierre de cajas.</b>');
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        }
        /* ==== INICIO | PROYECTO FONDOS-MANDATOS - ZOLUXIONES | CRumiche | 2019-01-07 | Lista de mensajes al realizr Valorización Amortizada */
        function agregarFilaMensajes(nemonico, mensaje) {
            $('#tablaMensajes > tbody:last-child').append('<tr><td style="text-align: center">' + nemonico + '</td><td>' + mensaje + '</td></tr>');
        }
        function mostrarPanelMensajes() {
            $("#winMensajes01").show();
        }
        /* ==== FIN | PROYECTO FONDOS-MANDATOS - ZOLUXIONES | CRumiche | 2019-01-07 | Lista de mensajes al realizr Valorización Amortizada */

        $(document).ready(function () {
            $("#winMensajes_btnCerrar").click(function () { $("#winMensajes01").hide(); return false; });

            $('#btnBuscar').on('click', function (e) {
                if ($('#tbFechaOperacion').val().length != 10) {
                    alertify.alert('Ingrese Fecha para realizar la búsqueda');
                    return false;
                }

                $('#btnProcesar').removeAttr('disabled');
                $('#hdOcultar').val('0');

                $("#popup01").show(); // Ricardo
            });

            $('#btnProcesar').on('click', function (e) {
                $("#popup01").show();
            });

            $('#btnVer').on('click', function (e) {
                $("#popup01").show();
            });

            ocultar();
            ActivarProcesar();

            $('#ddlSituacion').on('click', function (e) {
                var item = $(this).val();

                if (item == 1) {
                    $('#btnProcesar').hide();
                    $('#btnVer').hide();
                } else {
                    $('#btnProcesar').show();
                    $('#btnVer').show();
                }


            });

            $("input[type='radio']").on('click', function (e) {
                switch ($(this).val()) {
                    case 'V':
                        mostrar();
                        break;
                    case 'R':
                        ocultar();
                        break;
                }
            });

            function mostrar() {
                $('#pnlValEstimada').show();
            }

            function ocultar() {
                $('#pnlValEstimada').hide();
            }



            DesactivarHeader();
        });

        function ActivarBoton(chkControl) {
         
                var isChecked = chkControl.checked;
                if (isChecked) {
//                    chkControl.parentElement.parentElement.style.backgroundColor = '#E3D829';
//                    chkControl.parentElement.parentElement.style.color = 'white';
                   
                } else {
//                    chkControl.parentElement.parentElement.style.backgroundColor = 'white';
//                    chkControl.parentElement.parentElement.style.color = 'black';
                }

                ActivarProcesar();
            }
            function ColorGrilla(estado) {
                var tabla = document.getElementById('gvresultado');
                if (estado) {
                    for (var i = 0; i < tabla.rows.length; i++) {
                        switch (i) {
                            case 0:
                                break;
                            case 1:
                                break;
                            case 12:
                                break;
                            default:
//                                tabla.rows[i].style.backgroundColor = '#E3D829';
//                                tabla.rows[i].style.color = 'white';
                        }
                    }
                } else {
                    for (var i = 0; i < tabla.rows.length; i++) {
                        switch (i) {
                            case 0:
                                break;
                            case 1:
                                break;
                            case 12:
                                break;
                            default:
//                                tabla.rows[i].style.backgroundColor = 'white';
//                                tabla.rows[i].style.color = 'black';
                        }
                    }
                }

            }
            function ActivarProcesar() {
                var contador = 0;
                for (var i = 0; i < document.forms[0].elements.length; i++) {
                    if (document.forms[0].elements[i].type == 'checkbox') {
                        if (document.forms[0].elements[i].checked == true) {
                            contador += 1;
                        }
                    }
                }
  
                if (contador > 0 && $('#hdProcesado').val() != 1) {
                    $('#btnProcesar').removeAttr('disabled');
                    $('#hdOcultar').val('1');
                } else {
                    $('#btnProcesar').attr('disabled', 'disabled');
                    $('#hdOcultar').val('0');
                }
            }

            function DesactivarHeader() {

                var i;
                var contador = 0;

                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('gvresultado') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                          
                                if (document.forms[0].elements[i].disabled != 'disabled') {
                                    contador += 1;
                                }
                            }
                        }
                    }
                }

                if (contador == 1) {
                    $('#gvresultado_ctl02_SelectAllCheckBox').attr("disabled", true);
                }

            }

        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('gvresultado') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                                
                                document.forms[0].elements[i].checked = true;
                                

                            }
                        }
                    }
                }

                ColorGrilla(true);

            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('gvresultado') > -1)) {
                        if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                            document.forms[0].elements[i].checked = false;
//                            document.forms[0].elements[i].style.backgroundColor = 'white';
//                            document.forms[0].elements[i].style.color = 'black';
                        }
                    }
                }
                ColorGrilla(false);
            }

            ActivarProcesar();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="position:absolute; width: 100%;" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">

             <div class="row">
                <div class="col-md-6"><h2>Valorización</h2></div>
            </div>
    <hr />
    <fieldset>
    <legend></legend>
    <div class="row">

        <div class="col-md-3">
            <div class="form-group">
             
                <label class="col-sm-5 control-label">Fecha Operación</label>
                <div class="col-sm-7">

                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" 
                            ValidationGroup="Buscar" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Fecha Operación" ControlToValidate="tbFechaOperacion" 
                        ValidationGroup="Buscar">(*)</asp:RequiredFieldValidator>--%>
                </div>
          
            </div>
        </div>

        <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">Situación</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlSituacion" style = "width: 100px;" />
                    </div>

                    <br />
                    <br />

                    <label class="col-sm-4 control-label">Tipo Negocio</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlTipoNegocio" runat="server" style = "width: 90%;"></asp:DropDownList>
                    </div>

                </div>
        </div>

        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-12 control-label" style="text-align: center; padding-left: 30px;" >Tipo de Valorización</label>

                <br />
                <br />

                <div class="col-sm-12" style="text-align: center;" >
                    <asp:radiobuttonlist id="rblTipoValorizacion" runat="server" Width="100%" RepeatDirection="Vertical">
						<asp:ListItem Value="R" Selected="True" >Valorizaci&#243;n Normativa</asp:ListItem>
						<asp:ListItem Value="V">Valorizaci&#243;n Estimada</asp:ListItem>
					</asp:radiobuttonlist>
                </div>
            </div>
        </div>

        <div class="col-md-2">
                  <div class="form-group">
                  <div class="col-md-8" style="text-align: right;">
                  <asp:Button ID="btnBuscar" Text = "Buscar" CssClass="btn btn-integra" 
                          runat="server" ValidationGroup="Buscar" />
                  </div>
                  </div>
        </div>

    </div>
    </fieldset>
    <br />
     <div class="row" id="pnlValEstimada">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Precio</label>
                    <div class="col-sm-9">
                        <asp:DropDownList id="ddlPrecioEstimado" runat="server" ></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Tipo de Cambio</label>
                    <div class="col-sm-9">
                        <asp:DropDownList id="ddlTipoCambioEstimado" runat="server" ></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

    <%-- ==== INICIO | PROYECTO FONDOS-MANDATOS - ZOLUXIONES | CRumiche | 2019-01-07 | Lista de mensajes al realizr Valorización Amortizada --%>
    <div id="winMensajes01" class="winMensajes" style="display:none">
        <div class="cabecera" >
            <input id="winMensajes_btnCerrar" type="submit" value="Cerrar [ x ]"  class="btn btn-integra cerrar"  />
            <span class="titulo">Valorización Amortizada: Alertas y Errores al procesar los movimientos</span><br /><br />
        </div>       
        <table id="tablaMensajes" class="grilla">
            <thead>
                <tr style="background-color: #f7b08f; font-weight:bold">
                    <td style="width: 150px">Nemonico</td>
                    <td>Error / Alerta</td>
                </tr>            
            </thead>
            <tbody>                          
            </tbody>
        </table>
    </div>
    <%-- ==== FIN | PROYECTO FONDOS-MANDATOS - ZOLUXIONES | CRumiche | 2019-01-07 | Lista de mensajes al realizr Valorización Amortizada --%>

        <div class="grilla divGrilla">
                <asp:GridView ID="gvresultado" runat="server" SkinID="Grid_Paging_No" 
                    AutoGenerateColumns="False">
                     <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                     <asp:CheckBox ID="SelectAllCheckBox" onclick="SelectAll(this)" runat="server"></asp:CheckBox>
                                </HeaderTemplate>
                                 <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" onclick="ActivarBoton(this)" runat="server"></asp:CheckBox>
                                    <asp:HiddenField ID="hdCodPortafolio" Value='<%# Eval("codigoPortafolio") %>' runat = "server" />
                                    <asp:HiddenField ID="hdFecApertura" Value='<%# Eval("FechaConstitucion") %>' runat = "server" />
                                     <asp:HiddenField ID="hdEstado" Value='<%# Eval("estado") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="25px" />
                             </asp:TemplateField>
                            <asp:BoundField DataField="Fondo" HeaderText="Portafolio" >
                            <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Archivos PIP">
                                <ItemTemplate>
                                    <asp:Image ID="imgPip" runat="server"  />
                                    <asp:HiddenField ID="hdPip" runat="server" Value='<%# Eval("archivosPIP") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cierre Caja">
                                <ItemTemplate>
                                    <asp:Image ID="imgCaja" runat="server" />
                                    <asp:HiddenField ID="hdCaja" runat="server" Value='<%# Eval("cierreCaja") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Diferencia PXQ" >
                                <ItemTemplate>
                                    <asp:Image ID="imgVL" runat="server" Width="15px" />
                                    <asp:HiddenField ID="hdVL" runat="server" Value='<%# Eval("vl") %>'></asp:HiddenField>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interés Ganado" >
                                  <ItemTemplate>
                                  <asp:Image ID="imgInteres" runat="server" Width="15px" />
                                   <asp:HiddenField ID="hdInteres" runat="server" Value='<%# Eval("interesNegativo") %>'></asp:HiddenField>
                                </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Variación Ganancia/Pérdida" >
                                <ItemTemplate>
                                    <asp:Image ID="imgVariacion" runat="server" Width="15px" />
                                    <asp:HiddenField ID="hdVariacion" runat="server" Value='<%# Eval("valorizacionGanancia") %>'></asp:HiddenField>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inversión Nula" >
                                  <ItemTemplate>
                                    <asp:Image ID="imgInversion" runat="server" Width="15px" />
                                    <asp:HiddenField ID="hdInversion" runat="server" Value='<%# Eval("invercionNula") %>'></asp:HiddenField>
                                </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="valorización Nula" >
                                 <ItemTemplate>
                                 <asp:Image ID="imgValorizacion" runat="server" Width="15px" />
                                    <asp:HiddenField ID="hdValorizacion" runat="server" Value='<%# Eval("valorizacionNula") %>'></asp:HiddenField>
                                </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                    </Columns>
                </asp:GridView>                
      </div>

    <div class="row" style="text-align: right;">
      <asp:Button ID="btnVer" Text = "Visualizar" CssClass="btn btn-integra" Visible="false" runat="server" />
      <asp:Button ID="btnProcesar" Text = "Procesar" CssClass="btn btn-integra" Enabled="false" runat="server" />
      <asp:HiddenField ID="hdOcultar" Value ="0" runat="server" />
       <asp:HiddenField ID="hdProcesado" Value ="0" runat="server" />
  
      <asp:Button ID="btnSalir" Text = "Salir" CssClass="btn btn-integra" runat="server" />
      </div>
    <div class="row" style="text-align: right;">
        <%--        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" 
            CausesValidation="False" />--%>
    </div>
    
    </div>
    
    </form>

    <div id="popup01" class="winBloqueador" style="display: none">
        <div class="winBloqueador-inner">
          <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..."  style="height: 70px;"/>
        </div>
    </div>
</body>
</html>
