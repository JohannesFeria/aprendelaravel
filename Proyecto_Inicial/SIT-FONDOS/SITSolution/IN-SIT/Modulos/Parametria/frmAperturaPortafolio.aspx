<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAperturaPortafolio.aspx.vb" Inherits="Modulos_Parametria_frmAperturaPortafolio" %>

<!DOCTYPE html>

<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Apertura de Negociaci&oacute;n</title>

    <script type="text/javascript" src="App_Themes/js/jquery.js"></script>

    <style type="text/css"> 
        .win01 {
            border: solid 1px gray;
            background-color: #80808052;
            position: absolute;
            z-index: 8;
            width: 100%;
            height: 100%;
            text-align: center;     
        }        
        .cont01 {
            border: solid 1px gray;
            background-color: white;
            display: inline-block;
            
            margin-top: 120px;
            padding: 10px 20px; 
            border-radius: 5px;        
        }
        
        .tab01 {
            width: 100%;
        }
        .tab01>tbody>tr>td {
            padding: 2px 5px;
            border: Solid 1px gray;
        }        
        .span01 {
            font-size: 17px;
        }        

    </style>

    <script type="text/javascript">
        function MostrarConfirmacion(msj, cantidad, Cuentas) {
            var fecha = ""
            fecha = $('#tbFechaInicio').val();
            if ((msj == 'Sunday' || msj == 'Saturday') && cantidad == 0) {
                alertify.alert('La fecha ingresada es Sabado o Domingo.');
                return false;
            } 
            if (msj == 'Feriado' && cantidad == 0) {
                if (!confirm('La Fecha Ingresada ' + fecha + ' es Feriado .¿Desea aperturar?')) {
                    return false;
                }
                else { $('#hdValidarApertura').val("1"); }
            }

            // Despues de 
//            if (cantidad > 0) { 
//                if (msj == 'Feriado') 
//                    if (!confirm('La Fecha Ingresada ' + fecha + ' es Feriado.¿Desea aperturar?')) 
//                        return false; 

//                if (!confirm('Cuentas Sobregiradas ' + Cuentas + ' , Desea detener la Apertura?. En caso  desee continuar presione Cancelar para aperturar nueva fecha ' + fecha))
//                    $('#hdValidarApertura').val("1");
//                else 
//                    return false; 
//            }

            document.getElementById("<%= btnEjecutar.ClientID %>").click();
        }
        
        function agregarFilaCuentas(banco, cuenta, saldo) {
            $('#tablaCuentas > tbody:last-child').append('<tr><td>' + banco + '</td><td>' + cuenta + '</td><td style="text-align: right;">' + saldo + '</td></tr>');
        }
        function mostrarCuentasConIncidentes() {            
            $("#popup01").show();
        }
        $(document).ready(function () {
            $("#Popup01_btnCerrar").click(function () { $("#popup01").hide(); });

            $("#Popup01_btnContinuar").click(function () {
                $('#hdValidarApertura').val("1");
                $("#divBloqueo").show(); 
                document.getElementById("<%= btnEjecutar.ClientID %>").click();
             });
        });
    </script>
</head>
<body>
   <div id="divBloqueo" class="winBloqueador" style="display: none">
        <div class="winBloqueador-inner">
          <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..."  style="height: 90px;"/>
        </div>
    </div>
    <div id="popup01" class="win01" style="display:none">
        <div class="cont01">
            <span class="span01">Cuentas sobregiradas o con Saldo Cero</span><br /><br />
            
            <table id="tablaCuentas" class="tab01">
                <tr style="background-color: #e3d829; font-weight:bold">
                    <td>Banco</td>
                    <td>Nro. Cuenta</td>
                    <td>Saldo Banco</td>
                </tr>
            </table>
            <br />
            <span id="spanContinuar">¿Desea continuar con el proceso?</span><br />
            <br />
            <input type="submit" value="No" id="Popup01_btnCerrar" class="btn btn-integra" style="min-width: 80px; text-align: center; width:auto;" />
            <input type="submit" value="Sí, Deseo Continuar" id="Popup01_btnContinuar" class="btn btn-integra" style="min-width: 80px; text-align: center; width:auto; margin-left: 10px;" />
        </div>
    </div>

    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"> <h2>Apertura de Negociaci&oacute;n   </h2> </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="dlPortafolio" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Actual</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblFecha" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Nueva</label>
                        <div class="col-sm-8">
                            <div  id ="Div_fn" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" Enabled ="true"  />
                                <span class="add-on"><i class="awe-search"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha Nueva" ControlToValidate="tbFechaInicio" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Reproceso</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:CheckBox ID="chkreproceso" runat="server" AutoPostBack ="true"  />  
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
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar"/>
                <asp:Button Text="Salir" runat="server" ID="btnSalir" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-6" style="display: none">
                <asp:Button Text="Ejecutar" runat="server" ID="btnEjecutar" />
            </div>
        </div>
    </div>
    <input id="hdValidarApertura" type="hidden" name="hdValidarApertura" runat="server" />
   
    </form>
      
</body>
</html>
