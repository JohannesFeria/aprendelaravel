<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSwapPorcentaje.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmSwapPorcentaje" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>SWAP</title>
     <style type="text/css">
         .colorBackGround
         {
             background-color: #8080801f;
         }
         .label_Leg
         {
             font-weight: bold;
         }
     </style>
    <script type="text/javascript">
        function Validar() {
            return true;
        }
        function ResaltarColor(obj, flag) {
        if (flag == true)
            document.getElementById(obj).style.borderColor = "#FF0000";
        else if(flag == false)
            document.getElementById(obj).style.borderColor = "#CCCCCC";
        }
        $(document).ready(function () {
            $("[id*='ddlMonedaLeg']").change(function () {
                var mon1 = $("#ddlMonedaLeg1").val() == "" ? "-" : $("#ddlMonedaLeg1").val();
                var mon2 = $("#ddlMonedaLeg2").val() == "" ? "-" : $("#ddlMonedaLeg2").val();
                $("[id$='lblParTC']").text(mon1 + "/" + mon2);
                if (mon1 == mon2) {
                    $("#txtTipoCambio").prop("disabled", true);
                    $("#txtTipoCambio").val("1");
                } else {
                    var estado = ObtenerParametroValor('estado');
                    if (estado != "E-ELI" && estado != "E-CON") $("#txtTipoCambio").prop("disabled", false);

                }
            });
            //            $("[id$='ddlTasaInteresVariableOrigen']").change(function () {
            //                var tasa = $("#ddlTasaInteresVariableOrigen").val() == "" ? "-" : $("#ddlTasaInteresVariableOrigen  option:selected").text();
            //                if (tasa.length > 1) {
            //                    var libor = tasa.split("|");
            //                    $("[id$='lblTasaFlotanteOrigen']").text(libor[1].trim());
            //                } else {
            //                    $("[id$='lblTasaFlotanteOrigen']").text("-");
            //                }
            //            });


//            $("[id$='txtTasaLiborOrigen']").change(function () {
//                 $("#btnAceptar").attr("disabled", true);
//             });

//             $("[id$='txtTasaLibor']").change(function () {
//                 $("#btnAceptar").attr("disabled", true);
//             });

            //            $("[id$='ddlTasaInteresVariable']").change(function () {
            //                var tasa = $("#ddlTasaInteresVariable").val() == "" ? "-" : $("#ddlTasaInteresVariable  option:selected").text();
            //                if (tasa.length > 1) {
            //                    var libor = tasa.split("|");
            //                    $("[id$='lblTasaFlotante']").text(libor[1].trim());
            //                } else {
            //                    $("[id$='lblTasaFlotante']").text("-");
            //                }
            //            });
            var mon1 = $("#ddlMonedaLeg1").val() == "" ? "-" : $("#ddlMonedaLeg1").val();
            var mon2 = $("#ddlMonedaLeg2").val() == "" ? "-" : $("#ddlMonedaLeg2").val();
            $("[id$='lblParTC']").text(mon1 + "/" + mon2);
            if (mon1 == mon2) {
                $("#txtTipoCambio").prop("disabled", true);
                $("#txtTipoCambio").val("1");
            } else {
                var estado = ObtenerParametroValor('estado');
                if (estado != "E-ELI" && estado != "E-CON") $("#txtTipoCambio").prop("disabled", false);
            }

            //            var tasa = $("#ddlTasaInteresVariableOrigen").val() == "" ? "-" : $("#ddlTasaInteresVariableOrigen option:selected").text();
            //            if (tasa.length > 1) {
            //                var libor = tasa.split("|");
            //                $("[id$='lblTasaFlotanteOrigen']").text(libor[1].trim());
            //            } else {
            //                $("[id$='lblTasaFlotanteOrigen']").text("-");
            //            }

            //            tasa = $("#ddlTasaInteresVariable").val() == "" ? "-" : $("#ddlTasaInteresVariable option:selected").text();
            //            if (tasa.length > 1) {
            //                libor = tasa.split("|");
            //                $("[id$='lblTasaFlotante']").text(libor[1].trim());
            //            } else {
            //                $("[id$='lblTasaFlotante']").text("-");
            //            }


            function ObtenerParametroValor(param) {
                var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < url.length; i++) {
                    var urlparam = url[i].split('=');
                    if (urlparam[0] == param) {
                        return urlparam[1];
                    }
                }
            }
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
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
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" Text="SWAP" runat="server" /></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h2>
                        <asp:Label ID="lblAccion" Text="" runat="server" />
                    </h2>
                </div>
            </div>
        </header>
        <br />
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label runat="server" id="lblFondo" class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="180px" AutoPostBack="true"
                                Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label runat="server" class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoOperacion" Width="180px" Enabled="false"
                                AutoPostBack="true">
                                <asp:ListItem Value="1" Text="Compra" />
                                <asp:ListItem Value="2" Text="Venta" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label runat="server" class="col-sm-4 control-label">
                            Tipo Cuponera</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoCuponera" Width="180px" Enabled="false"
                                AutoPostBack="true">
                                <asp:ListItem Value="0" Text="Manual" />
                                <asp:ListItem Value="1" Text="Automática" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset id="fsEmisionesSWAP" runat="server">
            <legend>Emisiones SWAP</legend>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código ISIN</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" onkeypress="Javascript:Numero();" ID="txtISIN" Width="150px"
                                MaxLength="12" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnemónico</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Unidades</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtUnidades" Width="150px" Enabled="false" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" Enabled="False" />
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregar" Enabled="False" />
                </div>
            </div>
            <div class="grilla">
                <asp:GridView runat="server" ID="gvBonos" SkinID="Grid_AllowPaging_NO">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Correlativo") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="25px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Correlativo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoIsin" HeaderText="Isin" />
                        <asp:BoundField DataField="CodigoNemonico" HeaderText="Nemonico" />
                        <asp:BoundField DataField="Unidades" DataFormatString="{0:#,##0.00}" HeaderText="Unidades"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Nominal" DataFormatString="{0:#,##0.00}" HeaderText="Nominal"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vencimiento" ItemStyle-HorizontalAlign="Center" />
                        <%-- <asp:BoundField DataField="PrecioVenta" DataFormatString="{0:#,##0.00}" HeaderText="Precio Venta" />--%>
                        <asp:TemplateField HeaderText="Importe Venta" ItemStyle-Width="120px">
                            <ItemTemplate>
                                <div class="input-append">
                                    <asp:TextBox ID="txtPrecioVenta" runat="server" CssClass="Numbox-7" Width="120px"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "PrecioVenta") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos SWAP</legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operacion</label>
                        <div class="col-sm-7">
                            <div id="imgFechaOperacion" runat="server" class="input-append">
                                <asp:TextBox ID="tbFechaOperacion" runat="server" SkinID="Date" Enabled="false" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código ISIN</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" onkeypress="Javascript:Numero();" ID="txtISINOperacion"
                                Width="180px" MaxLength="12" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Grupo I.</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlGrupoInt" Width="180px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Intermediario</label>
                        <div class="col-sm-7">
                            <asp:UpdatePanel ID="up_Intermediario" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="ddlIntermediario" Width="246px" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlGrupoInt" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-sm-5 colorBackGround" style="width:36%">
                    <div class="form-group">
                        <label class="col-sm-6 control-label label_Leg">
                            LEG 1</label>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 6%">
                </div>
                <div class="col-sm-5 colorBackGround" style="width:36%">
                    <div class="form-group">
                        <label class="col-sm-6 control-label label_Leg">
                            LEG 2</label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4" style="width: 35.33%">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nominal Inicial</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtMontoNominalOriginal" runat="server" CssClass="Numbox-7" Enabled="false"
                                Text="0" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 6%">
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Nominal Final</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMontoNominal" runat="server" CssClass="Numbox-7" Enabled="false"
                                        Text="0" Width="180px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3" style="text-align: right; width: 200px;">
                            <asp:Button ID="btnProcesar" runat="server" Enabled="false" Text="Procesar" />
                        </div>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div id="blurEspera" />
                                <div id="progressEspera">
                                    <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMonedaLeg1" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMonedaLeg2" Width="180px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-7">
                            <div id="Div1" runat="server" class="input-append date">
                                <asp:TextBox ID="txtfecIniCuponOrigen" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-7">
                            <div id="Div3" runat="server" class="input-append date">
                                <asp:TextBox ID="txtfecIniCupon" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Fin</label>
                        <div class="col-sm-7">
                            <div id="Div4" runat="server" class="input-append date">
                                <asp:TextBox ID="txtfecFinCuponOrigen" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Fin</label>
                        <div class="col-sm-7">
                            <div id="Div2" runat="server" class="input-append date">
                                <asp:TextBox ID="txtfecFinCupon" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Cambio SPOT</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtTipoCambio" runat="server" CssClass="Numbox-7" Enabled="false"
                                Width="90px" />
                            <label id="lblParTC" class="control-label" style="width: 90px;">
                                -/-</label>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="updtLibor" runat="server">
            <ContentTemplate>

            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Interés %</label>
                        <div class="col-sm-7" style="width:200px;">
                            <asp:TextBox ID="txtTasaOriginal" runat="server" CssClass="Numbox-7" Enabled="false"
                                Width="90px" />
                            <asp:DropDownList runat="server" ID="ddlTasaInteresVariableOrigen" Width="90px" Enabled="False" AutoPostBack="true" />
                        </div>
                       
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Interés %</label>
                        <div class="col-sm-7" style="width: 200px;">
                            <asp:TextBox ID="txtTasa" runat="server" CssClass="Numbox-7" Enabled="false" Width="90px"  />
                            <asp:DropDownList runat="server" ID="ddlTasaInteresVariable" Width="90px" AutoPostBack="true"  />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">                    
                         <div class="col-sm-4"> 
                          <asp:Panel runat="server" ID="pnlTasaLiborOrigen" Visible="False">             
                           <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tasa Libor</label>
                            <div class="col-sm-7" style="width:200px;">
                              <asp:DropDownList runat="server" ID="ddlTasaLiborOrigen" Width="90px"  AutoPostBack="true"/>
                                <asp:TextBox ID="txtTasaLiborOrigen" runat="server" CssClass="Numbox-7" 
                                    Width="90px" AutoPostBack="true" />                          
                            </div>
                         </div>
                        </asp:Panel>
                        </div>                    
                
                <div class="col-sm-1" style="width: 8%">
                </div>
               
                     <div class="col-sm-4">
                      <asp:Panel runat="server" ID="pnlTasaLibor" Visible="False" >
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tasa Libor</label>
                                <div class="col-sm-7" style="width:200px;">
                                  <asp:DropDownList runat="server" ID="ddlTasaLibor" Width="90px"  AutoPostBack="true" />
                                    <asp:TextBox ID="txtTasaLibor" runat="server" CssClass="Numbox-7"
                                        Width="90px" AutoPostBack="true"/>
                          
                                </div>
                            </div>
                        </asp:Panel>
                        </div>
               
                        
            </div>
            
            </ContentTemplate>
            </asp:UpdatePanel>

            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Periodicidad</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPeriodicidadOriginal" runat="server" Width="183px" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Periodicidad</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPeriodicidad" runat="server" Width="183px" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Amortización</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlAmortizacionOriginal" runat="server" Width="183px" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Amortización</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlAmortizacion" runat="server" Width="183px" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlBaseDiasOrigen" runat="server" Width="90px" Enabled="false" />
                            <asp:DropDownList ID="ddlBaseMesOrigen" runat="server" Width="90px" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-1" style="width: 8%">
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlBaseDias" runat="server" Width="90px" Enabled="false" />
                            <asp:DropDownList ID="ddlBaseMes" runat="server" Width="90px" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>Cuponera SWAP</legend>
                    <div class="grilla" style="overflow: scroll;">
                        <asp:GridView ID="dgLista" runat="server" SkinID="Grid_AllowPaging_NO">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25px" HeaderStyle-CssClass="hidden">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Consecutivo") %>'
                                            CommandName="Eliminar" SkinID="imgDelete" />
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" CssClass="hidden" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acción" ItemStyle-Width="25px" >
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnModificar" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Consecutivo") %>'
                                            CommandName="Modificar" SkinID="imgEdit" ToolTip="Modificar Cupon" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Consecutivo" HeaderText="Nro." ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="FechaIniOriginal" HeaderText="Fecha Inicio" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Fecha Fin" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <div class="input-append date" id="divFechaTerminoOriginal" runat="server">
                                            <asp:TextBox ID="tbFechaTerminoOriginal" Font-Size="11px" runat="server" Style="width: 75px;"
                                                placeholder="dd/MM/yyyy" data-mask="99/99/9999" Text='<%# DataBinder.Eval(Container.DataItem, "FechaFinOriginal") %>' />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                               <%-- <asp:BoundField DataField="FechaFinOriginal" HeaderText="Fecha Fin" ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:BoundField DataField="DifDiasOriginal" HeaderText="Dif. Días" ItemStyle-HorizontalAlign="Center"  />
                                
                                 <asp:TemplateField HeaderText="Fecha Libor" ItemStyle-Width="25px" Visible="false">
                                    <ItemTemplate>
                                        <div class="input-append date" id="divFechaLiborOriginal" runat="server">
                                            <asp:TextBox ID="tbFechaLiborOriginal" Font-Size="11px" runat="server" Style="width: 75px;"
                                                placeholder="dd/MM/yyyy" data-mask="99/99/9999" Text='<%# DataBinder.Eval(Container.DataItem, "FechaLiborOriginal") %>' />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>


                                <asp:BoundField DataField="NominalRestanteOriginal" DataFormatString="{0:#,##0.00}"
                                    ItemStyle-HorizontalAlign="Right" HeaderText="Saldo Nominal" />
                                <asp:BoundField DataField="MontoAmortizacionOriginal" DataFormatString="{0:#,##0.00}"
                                    ItemStyle-HorizontalAlign="Right" HeaderText="Flujo Amortización" HeaderStyle-Width="80px" />
                                <%--           <asp:TemplateField HeaderText="Factor Amortización %">
                                    <ItemTemplate>
                                        <div class="input-append">
                                            <asp:TextBox ID="txtAmortizacion" runat="server" CssClass="Numbox-7" Text='<%# DataBinder.Eval(Container.DataItem, "Amortizac") %>'
                                                Width="90px" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="AmortizacOriginal" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                                    HeaderText="Factor Amortización %" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="MontoInteresOriginal" DataFormatString="{0:#,##0.00}"
                                    ItemStyle-HorizontalAlign="Right" HeaderText="Flujo Interés" />
                                <asp:BoundField DataField="TotalFlujoOriginal" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                                    HeaderText="Total Flujo" />
                                <asp:BoundField DataField="FechaIni" HeaderText="Fecha Ini" ItemStyle-HorizontalAlign="Center" />
                                <%--   <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:TemplateField HeaderText="Fecha Fin" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <div class="input-append date" id="divFechaTermino" runat="server">
                                            <asp:TextBox ID="tbFechaTermino" Font-Size="11px" runat="server" Style="width: 75px;"
                                                placeholder="dd/MM/yyyy" data-mask="99/99/9999" Text='<%# DataBinder.Eval(Container.DataItem, "FechaFin") %>' />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="DifDias" HeaderText="Dif. Días" ItemStyle-HorizontalAlign="Center" />
                                  <asp:TemplateField HeaderText="Fecha Libor" ItemStyle-Width="25px" Visible="false">
                                    <ItemTemplate>
                                        <div class="input-append date" id="divFechaLibor" runat="server">
                                            <asp:TextBox ID="tbFechaLibor" Font-Size="11px" runat="server" Style="width: 75px;"
                                                placeholder="dd/MM/yyyy" data-mask="99/99/9999" Text='<%# DataBinder.Eval(Container.DataItem, "FechaLibor") %>' />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="NominalRestante" DataFormatString="{0:#,##0.00}" HeaderText="Saldo Nominal"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="MontoAmortizacion" DataFormatString="{0:#,##0.00}" HeaderText="Flujo Amortización"
                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="MontoInteres" DataFormatString="{0:#,##0.00}" HeaderText="Flujo Interés"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="TotalFlujo" DataFormatString="{0:#,##0.00}" HeaderText="Total Flujo"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="TCFlujo" DataFormatString="{0:#,##0.0000}" HeaderText="TC Flujo"
                                    ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdPagina" runat="server" />
        <asp:HiddenField ID="hdCodigoOrden" runat="server" />
        <asp:HiddenField ID="hdMoneda" runat="server" />
        <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
        <br />
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                   <asp:UpdatePanel ID="upnl1" runat="server">
                     <ContentTemplate>
                <asp:Button ID="btnRetornar" runat="server" Enabled="false" Text="Cancelar" />
                <asp:Button ID="btnAceptar" runat="server" Enabled="false" Text="Aceptar" UseSubmitBehavior="False"/>
                   </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <fieldset class="hidden">
            <legend>Campos Retirados </legend>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Forma de Cambio</label>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlFormaCambio" runat="server" Width="100px" Enabled="false">
                            <asp:ListItem Text="Directo" Value="1" />
                            <asp:ListItem Text="Indirecto" Value="2" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
       </form>
</body>
</html>