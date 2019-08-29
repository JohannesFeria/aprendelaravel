<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSeriesPortafolioFirbi.aspx.vb" Inherits="Modulos_ValorCuota_frmSeriesPortafolioFirbi" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Valores Portafolio</title>
</head>
  <script type="text/javascript" >
      //OT10927 - 22/11/2017 - Ian Pastor M. Modal detalle de CxC Venta de título
      $(function () {

          $('#tbFechaOperacion').change(function () {
              $('#listButtons').hide();
          });


          $("#datosOtrasCXP").dialog({ resizable: false, autoOpen: false,
              buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
              hide: { effect: "explode", duration: 1000 }
          });
          //Elimina el botón "X" del modal
          $("#datosOtrasCXP").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
          $("#imgDetalle").click(function () {
              $("#datosOtrasCXP").dialog('open');
              $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
          });
      });
      //OT10927 - Fin

      //OT10927 - 22/11/2017 - Hanz Cocchi. Exportar archivo plano de rentabilidad
      function ExportarArchivo() {
          $("#btnExportar").click();
      }

      function GrabarDistribucionLib() {
          //var vCantRegDistribLib = $("#hdCantRegDistribLib").val();
          //if (vCantRegDistribLib == 0) {
          //if (confirm("¿Desea registrar los datos?")) {
          $("#btnGrabarDistribucionLib").click();
          alert("Los datos se guardaron correctamente.");
          //}
          //}
      }
      //OT10927 - Fin
  </script>
   <style type="text/css">
    .ocultarCol {
            display: none;
        }
        
         
        #btnRefrescar, #btnRefrescar:hover
        {
            background-image: url(../../App_Themes/img/refresh01.png);
            background-size: contain;
            background-repeat: no-repeat;
            background-position-x: center;
            background-position-y: center;
            min-width: 50px !important;
            margin-right: 35px;
        }
    </style>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><h2>Calculo Valor Fondo Manual - FIRBI</h2></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">Fecha Información</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>                    
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlPortafolio" AutoPostBack="true" runat="server" Width="120px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlPortafolio" InitialValue="0" runat="server" ErrorMessage="Seleccione Portafolio"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align:left;">
                            <asp:Button ID="btnInicializar" runat="server" Text="Inicializar" Visible="false" UseSubmitBehavior="false"/>
                            <asp:Button ID="btnRefrescar" runat="server" Text="" UseSubmitBehavior="false" />
                             <span ID="listButtons" style="display:none;" runat="server">
                                <asp:Button ID="btnProcesar" runat="server" Text="Procesar" Visible="false" UseSubmitBehavior = "false" />
                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Visible="false" UseSubmitBehavior="false" />
                                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Visible="false" UseSubmitBehavior="false" />
                                <asp:Button ID="btnVerDistribucion" runat="server" Text="Ver Distribución" Visible="false" UseSubmitBehavior="false" />
                                <span style="display:none;">
                                    <asp:Button runat="server" ID="btnExportar" Text="Exportar" UseSubmitBehavior="false" />
                                    <asp:Button runat="server" ID="btnGrabarDistribucionLib" Text="Grabar Distribucion" UseSubmitBehavior="false" />
                                </span>
                            </span>
                             
                            
                        </div>
                    </div>
                </div>
            </div>
            
        </fieldset>
        <br />
        <fieldset id="FormInversiones" runat="server" visible="false">
            <legend>Inversiones</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Valor Cuota Anterior</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtVCA" ReadOnly ="true" Width="150px" />
                        </div>
                    </div>                    
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Diferencia Valor Cuota</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtVCDiferencia" ReadOnly ="true" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Comision SAFM Anterior</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" Enabled="false" ID="txtComisionSAFMAnterior" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Inversiones [t-1]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbInversionesT1" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Compras [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbComprasT" ReadOnly = "true"  CssClass="Numbox-7"   runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Ventas y Venci. [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbVentasVenciT" ReadOnly = "true"  CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Rentabilidad [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbRentabilidadT" ReadOnly = "true" CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Val. Forwards [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValoriForwardT" ReadOnly = "true" CssClass="Numbox-7"   runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><b>Inversiones [T] Subtotal</b> </label>
                        <div class="col-sm-8">
                            <%--<asp:TextBox ID="tbInversionesSubTotal" ReadOnly = "true" CssClass="Numbox-7"  runat="server" />--%>
                            <asp:TextBox ID="tbInversionesSubTotal" CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pncom" runat="server" >
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><b>I.G.V.</b> </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtigv" ReadOnly = "true" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><b>Porcentaje de comisión</b> </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtporcom" ReadOnly = "true" CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
                </asp:Panel>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormPrecierre" runat="server" visible="false">
            <legend>Valor Cuota Precierre</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Caja [Precierre]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCajaPrecierre" CssClass="Numbox-7" runat="server"  />
                            <%--<asp:TextBox ID="TextBox1" CssClass="Numbox-7" runat="server" ReadOnly ="true"  />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">CxC Monto Liberada(Precierre)</label>
                        <div class="col-sm-8">
                            <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosPrecierre"--%>
                            <asp:TextBox ID="txtMontoDividendosPrecierre" CssClass="Numbox-7" runat="server" ReadOnly ="true"  />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">CXC Venta de título</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCtitulo" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Otras CXC</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCotras" onkeypress="return soloNumeros(event)" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">CXC(Precierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCXCPrecierre" CssClass="Numbox-7" runat="server"  ReadOnly = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Cheque Pendiente</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtChequePendiente"  onkeypress="return soloNumeros(event)" runat="server"  />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Rescate Pendiente</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtRescatePendiente" onkeypress="return soloNumeros(event)" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Ajustes CXP</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtAjustesCXP" onkeypress="return soloNumeros(event)" runat="server" Text ="0" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">CXP Compra de título</label>
                        <div class="col-sm-8">
                            <%--<asp:TextBox ID="tbCXPtitulo" ReadOnly = "true" CssClass="Numbox-7"   runat="server" />--%>
                            <asp:TextBox ID="tbCXPtitulo" CssClass="Numbox-7"   runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">Otras CXP</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXPotras" CssClass="Numbox-7" runat="server" ReadOnly ="true" Width="180px"  />
                            <img alt="Detalle" id="imgDetalle" src="../../App_Themes/img/icons/tree_list.png" style="cursor:pointer; display: none" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group campo-omitido">
                        <label class="col-sm-4 control-label">CXP(Precierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCXPPrecierre" CssClass="Numbox-7" runat="server" ReadOnly = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otros Gastos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosGastos" CssClass="Numbox-7 NumBox" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otros Ingresos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosIngresos" CssClass="Numbox-7 NumBox"  runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="FilaPrecierre" runat="server">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">V. Pat. Precierre 1</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatriPrecierre" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Comisión SAFM</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbComiSAFM" ReadOnly = "true" onkeypress="return soloNumeros(event)"  runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">V. Pat. Precierre 2</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatriPrecierre2" ReadOnly = "true"  CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><b>V. Cuota Precierre(Cuotas) </b></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValCuotaPrecierre" CssClass="Numbox-7 NumBox" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" runat ="server" id="cuotapre" >
                    <div class="form-group">
                        <label class="col-sm-4 control-label">   <b>V. Cuota Precierre(Valores) </b></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValValoresPrecierre" Style="font-weight: bold; background-color: #e3e3e3;" ReadOnly = "true" CssClass="Numbox-7"   runat="server"/>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormCierre" runat="server" visible="false">
            <legend>Valor Cuota Cierre</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Caja</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCajaCierre" CssClass="Numbox-7" runat="server" ReadOnly = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Aportes Cuotas</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbAporteCuota" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Aportes Valores</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbAporteValores" CssClass="Numbox-7" runat="server" ReadOnly ="true"  />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Rescates Cuotas</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbRescatesCuota" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Rescates Valores</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbRescatesValores" onkeypress="return soloNumeros(event)"  runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">CXC Venta de título</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCtituloCierre" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otras CXC</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCotrasCierre" ReadOnly = "true"  CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">CXC(Cierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCCierre" CssClass="Numbox-7" runat="server" ReadOnly = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">CXP Compra de título</label>
                        <div class="col-sm-8">
                            <%--<asp:TextBox ID="tbCXPtituloCierre" ReadOnly = "true" CssClass="Numbox-7" runat="server" />--%>
                            <asp:TextBox ID="tbCXPtituloCierre" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otras CXP</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXPotrasCierre" CssClass="Numbox-7" runat="server" ReadOnly = "true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">CXP(Cierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXPCierre" CssClass="Numbox-7" runat="server" ReadOnly = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otros Gastos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosGastosCierre" onkeypress="return soloNumeros(event)"  runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otros Ingresos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosIngresosCierre" onkeypress="return soloNumeros(event)" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">V. Pat. Cierre (Cuota)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatCierreCuota" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">V. Pat. Cierre (Valor)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatCierreValores" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><b>V. Cuota Cierre (Cuota) </b></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValCuotaCierreCuota" Style="font-weight: bold; background-color: #e3e3e3;" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><b>V. Cuota Cierre (Valor) </b></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValCuotaCierreValores" Style="font-weight: bold; background-color: #e3e3e3;" ReadOnly = "true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormSeries" runat="server" visible="false">
            <legend>Series</legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><b>Cargar Porcentaje Series </b></label>
                        <div class="col-sm-9">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".txt" class="filestyle" data-buttonname="btn-primary" 
                            data-buttontext="Seleccionar" data-size="sm" style="width:300px;" >
                            <asp:HiddenField ID="hfRutaDestino" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align:right;">
                    <asp:CheckBox ID="cbHabilitarCarga" runat="server" Text="Habilitar carga automática" Checked="true" AutoPostBack="true" />
                    <asp:Button ID="btnCarga" runat="server" Text="Cargar Archivo" UseSubmitBehavior="false" />
                </div>
            </div>
            <br />
            <div class="grilla" style="height: 200px; width: 100%; overflow: auto;">
                <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgArchivo">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                   CommandArgument='<%# String.Format("{0}", DataBinder.Eval(Container.DataItem, "CodigoSerie")) %>' >                                    
                                </asp:ImageButton> 
                                <asp:HiddenField ID="hdDiferenciaMensaje" runat="server" Value='<%# Eval("DiferenciaMensaje") %>' />
                                <asp:HiddenField ID="hdFlagDiferencia" runat="server" Value='<%# Eval("FlagDiferencia") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CodigoSerie" HeaderText="Codigo Serie" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre Serie" />
                        <asp:BoundField DataField="CuotasPrecierre" HeaderText="Cuotas Precierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="ValorCero" HeaderText="Valores Precierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="ValoresPrecierre" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"  HeaderText="Valores Precierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="CuotasCierre" HeaderText="Cuotas Cierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="ValorCero" HeaderText="Valores Cierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="ValoresCierre" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"  HeaderText="Valores Cierre" DataFormatString="{0:0.0000000}" />                      
                        <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" DataFormatString="{0:0.0000000}" />
                        <asp:TemplateField HeaderText="Porcentaje Serie" ItemStyle-Width="25px" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPorcentaje" onkeypress="return soloNumeros(event)" Text='<%# DataBinder.Eval(Container.DataItem, "Porcentaje")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <div id="divFondo" style="width:100%; height:100%; position:absolute; top:0; left:0; display:none; background-color:#7D7D7D; opacity:0.7"></div>
        <div id="datosOtrasCXP" title ="Detalle del calculo de Otras CXP" >
            <div class="form-horizontal">
                <div class="container-fluid" >
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size:12px;">
                                <label class="col-sm-6 control-label"><b>Comision SAFM Acumulada</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtComisionSAFM" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label"><b>Caja de Recaudo:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtCajaRecaudo" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label"><b>Suscripción:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtSuscripcion" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label"><b>Cheque Pendiente:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtChequeP" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label"><b>Rescate Pendiente:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtRescateP" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input id="txtComisionPortafolio" type="text" runat="server" class="hidden" />
    <input id="HdFechaCreacionFondo" type="text" runat="server" class="hidden" />
    <input id="hdSeriado" type="text" runat="server" class="hidden" />
    <input id="hdCajaPrecierre" type="text" runat="server" class="hidden" />
    <input id="hdCuotasLiberadas" type="text" runat="server" class="hidden" />
    <input id="hdCodigoPortafolioSisOpe" type="text" runat="server" class="hidden" />
    <%--OT10927 - 22/11/2017 - Hanz Cocchi. Variable que permite guardar la cantidad de registros de operaciones liberadas--%>
    <input id="hdCantRegDistribLib" type="text" runat="server" class="hidden" value="0" />
    <%--OT10927 - Fin--%>    <%--OT11169 - 15/02/2018 - Ian Pastor M. Variable que guarda el codigo de portafolio padre del Sistema de Operaciones--%>
    <input id="hdCPPadreSisOpe" type="hidden" runat="server" />
    <%--OT11169 - Fin--%>
    <br />
    </form>
    <span id="spanMarca"></span>
</body>
</html>
