<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="false" CodeFile="frmRegistrarPagoFechaComision.aspx.vb" Inherits="Modulos_Inversiones_frmRegistrarPagoFechaComision" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  
    
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts")%>    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Registrar Fecha para Pago Comision</title>
    <style type="text/css">
        .divGrilla {
            height: 200px;
            border: solid 1px #706f6f;
            overflow-y: auto;
            margin-bottom: 15px;
        }
        
        .divGrilla2 {
            height: 200px;
            border: solid 1px #706f6f;
            overflow-y: auto;
            margin-bottom: 15px;
        }
        .winMensajes
        {
            border: solid 1px #706f6f;
            padding: 5px;
            overflow-y: auto;
            margin-bottom: 15px;            
            }

        .ocultarCol {
            display: none;
        }
    </style>

      <script type="text/javascript">

        function ObtenerPrefijoControlEnGrilla(controlEnGrilla) {            
            var idControl = $(controlEnGrilla).attr('id').toString();
            var nombreEnArray = idControl.split('_');
            var prefijoControl = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';            

            return prefijoControl.toString();
        }

        function blmostrocult(div) {
            if (div == "divGrillaComisiones") {
                if (document.getElementById(div).style.display == "none") document.getElementById(div).style.display = "block";
                else document.getElementById(div).style.display = "none";
            }
            else {
                if (document.getElementById(div).style.height == '20px') {
                    document.getElementById(div).style.height = '200px';
                } else {
                    document.getElementById(div).style.height = '20px';
                }
            }
            return false;
        }

        function CargarNumeroDeCuenta(codigoFondo, codigoMoneda, codigoBanco,prefijo) {
            debugger;
            var param = { codigoFondo: codigoFondo, codigoMoneda: codigoMoneda, codigoBanco: codigoBanco };
            $.ajax({
                url: "../../MetodosWeb.aspx/CargarNumeroDeCuentas",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (data) {
                    $("#" + prefijo + "ddlNumeroDeCuenta").html('');
                    $("#" + prefijo + "ddlNumeroDeCuenta").append('<option value="">--SELECCIONE--</option>');
                    for (var i = 0; i < data.d.length; i++) {
                        $("#" + prefijo + "ddlNumeroDeCuenta").append('<option value="' + data.d[i].id + '">' + data.d[i].descripcion + '</option>');
                    }
                 }, error: function (XMLHttpRequest, textStatus, errorThrown) { /*alert(textStatus);*/ }
            });
        }

        $(document).ready(function () {

//            $("[id$='ddlBanco']").change(function () {

//                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
//                $("#" + prefijoControlEnGrilla + "hdCodigoBanco").val($(this).val());

//                var fondo = $("#" + prefijoControlEnGrilla + "hdCodigoFondo").val();
//                var moneda = $("#" + prefijoControlEnGrilla + "hdCodigoMoneda").val();
//                CargarNumeroDeCuenta(fondo, moneda, $(this).val(), prefijoControlEnGrilla);

//            });

        });

        function CambiarBanco(combo) {
            debugger;
            var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(combo);
            $("#" + prefijoControlEnGrilla + "hdCodigoBanco").val($(combo).val());

            var fondo = $("#" + prefijoControlEnGrilla + "hdCodigoFondo").val();
            var moneda = $("#" + prefijoControlEnGrilla + "hdCodigoMoneda").val();
            CargarNumeroDeCuenta(fondo, moneda, $(combo).val(), prefijoControlEnGrilla);

        }
         </script>

</head>

<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> 
    
      <script language="javascript" type="text/javascript">
          Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(
	        function () {
	            if (document.getElementById) {
	                var progress = document.getElementById('progressEspera');
	                var blur = document.getElementById('blurEspera');
	                var altoPage = document.documentElement.scrollHeight;
	                progress.style.width = '300px';
	                progress.style.height = '300px';
	                blur.style.height = '1200px';
	                //     progress.style.top = altoPage / 3 - progress.style.height.replace('px', '') / 2 + 'px';
	                progress.style.top = '300px'
	                progress.style.left = document.body.offsetWidth / 2 - progress.style.width.replace('px', '') / 2 + 'px';
	            }
	        }
            )

    </script> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
        <div class="container-fluid" id="divContainer">
            <header><h2>Solicitud de Pago de Comisión</h2></header>
            <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-md-4">
                           <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Fecha Corte</label>
                                <div class="col-sm-5">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="txtFechaCobro" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                       </div>
                        <div class="col-md-8" style="text-align: right;">
                        
                           <div class="form-group">
                           <asp:Button id="btnBuscar" runat="server" Text="Buscar"/>
                           </div>
                        </div>
                    
                    </div>
                </fieldset>

                
                <br />
                <fieldset>
                    <legend>Fondos
                        <a onclick="return blmostrocult('divGrillaOrdenesEjecutada');" style="cursor: hand;
                            cursor: pointer; position: absolute; right: 20px;">[+/-] Expandir / Contraer
                        </a></legend>
                    <div class="grilla divGrilla" style="overflow: scroll;" id="divGrillaOrdenesEjecutada">
                        <asp:GridView ID="gvFondos" runat="server" SkinID="Grid_Paging_No" >
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                                   
                                    <ItemTemplate>

                                        <asp:CheckBox ID="chkSelectPE" runat="server"
                                            ></asp:CheckBox>
                                            <asp:HiddenField  ID="hdCodigoFondo"  Value='<%# Eval("CodigoPortafolioSBS") %>' runat="server" />
                                             <asp:HiddenField  ID="hdCodigoMoneda"  Value='<%# Eval("CodigoMoneda") %>' runat="server" />
                                             <asp:HiddenField  ID="hdCodigoEstado"  Value='<%# Eval("CodigoEstado") %>' runat="server" />
                                              <asp:HiddenField  ID="hdNombreFondo"  Value='<%# Eval("Portafolio") %>' runat="server" />
                                              <asp:HiddenField ID="hdNumeroDeCuenta" runat="server" Value='<%# Eval("NumeroDeCuenta") %>' />
                                              <asp:HiddenField ID="hdFechaInicio" runat="server" Value='<%# Eval("FechaInicio") %>' />
                                              <asp:HiddenField ID="hdFechaFin" runat="server" Value='<%# Eval("FechaFin") %>' />
                                              <asp:HiddenField ID="hdCodigoPortafolioSisOpe" runat="server" Value='<%# Eval("CodigoPortafolioSisOpe") %>' />                                             
                                              
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolio" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Portafolio" HeaderText="Portafolio"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                 <asp:BoundField DataField="Periodo" HeaderText="Periodo" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                              
                               <asp:TemplateField HeaderText="Banco" ItemStyle-HorizontalAlign="Center" >
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdCodigoBanco" runat="server" Value='<%# Eval("CodigoBanco") %>' />
                                        <asp:DropDownList ID="ddlBanco" OnSelectedIndexChanged="ddlBanco_SelectedIndexChanged" runat="server" Width="250px" AutoPostBack="true" />                               
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cuenta" ItemStyle-HorizontalAlign="Center" >
                                    <ItemTemplate>
                                        
                                        <asp:DropDownList ID="ddlNumeroDeCuenta" runat="server" Width="300px" />                               
                                    </ItemTemplate>
                                </asp:TemplateField>                              
                               <asp:BoundField DataField="CodigoMoneda" HeaderText="Mon"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                               <asp:BoundField DataField="Comision" HeaderText="Comisión" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                               
                               <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio"  HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="FechaCaja" HeaderText="FechaFin" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="FechaCajaCadena" HeaderText="FechaFin" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="NombreMoneda" HeaderText="NombreMoneda" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="Saldo" HeaderText="Saldo" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="CodigoEstado" HeaderText="CodigoEstado" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolioSBS" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="CodigoPortafolioSisOpe" HeaderText="CodigoPortafolioSisOpe" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               
                             <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crean los nuevos campos dinámicos para mostrar en grilla | 04/07/18 --%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="row" id="divBotonAgregar" runat="server" style="text-align: right;">
             
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                    </div>
                </fieldset>

                <br />
                <fieldset>
                    <legend>Fondos Seleccionados
                        <a onclick="return blmostrocult('div1');" style="cursor: hand;
                            cursor: pointer; position: absolute; right: 20px;">[+/-] Expandir / Contraer
                        </a></legend>
                    <div class="grilla divGrilla2" style="overflow: scroll;" id="div1">
                        <asp:GridView ID="gvFondosPendientes" runat="server" SkinID="Grid_Paging_No" >
                            <Columns>
                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibBorrarOE" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                            ToolTip="Eliminar"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolio" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Portafolio" HeaderText="Portafolio"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                 <asp:BoundField DataField="Periodo" HeaderText="Periodo" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                              
                             <asp:BoundField DataField="NombreBanco" HeaderText="Banco"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                             <asp:BoundField DataField="NumeroDeCuenta" HeaderText="Número de cuenta" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                              <asp:BoundField DataField="NombreNumeroDeCuenta" HeaderText="Número de cuenta"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                          
                               <asp:BoundField DataField="CodigoMoneda" HeaderText="Mon"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                             
                               <asp:BoundField DataField="Comision" HeaderText="Comisión" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                 <asp:BoundField DataField="UsuarioSolicitud" HeaderText="Operador"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                     <asp:BoundField DataField="NombreEstado" HeaderText="Estado"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                         

                               <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio"  HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="NombreMoneda" HeaderText="NombreMoneda" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="Saldo" HeaderText="Saldo" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="CodigoEstado" HeaderText="CodigoEstado" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="CodigoBanco" HeaderText="CodigoBanco" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                              <asp:BoundField DataField="FechaCaja" HeaderText="FechaFin" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                               <asp:BoundField DataField="FechaCajaCadena" HeaderText="FechaFin" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                              
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="row" id="div2" runat="server" style="text-align: right;">
             
                        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar"/>
                    </div>
                    <div style="display:none">
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
                    <asp:Button ID="btnBuscarAuxiliar" runat="server" Text="buscarAuxiliar"  />
                    <asp:Label ID="lblFondoEliminar" runat="server" text=""></asp:Label>
                    <asp:Label ID="lblIdentificador" runat="server" text=""></asp:Label>
                    <asp:HiddenField runat="server" ID="hdRptaConfirmar" />
                    </div>
                    <asp:HiddenField ID="hdEliminarConfirmar" runat="server" />
                </fieldset>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id="blurEspera" />
                    <div id="progressEspera">
                        <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
