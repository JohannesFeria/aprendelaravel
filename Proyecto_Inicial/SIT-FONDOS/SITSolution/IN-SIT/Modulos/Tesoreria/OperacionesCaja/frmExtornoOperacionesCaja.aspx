<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoOperacionesCaja.aspx.vb"
    Inherits="Modulos_Tesoreria_OperacionesCaja_frmExtornoOperacionesCaja" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>PreLiquidacionDivisas</title>

		<script language="javascript">
		    function SelectAll(CheckBoxControl) {
		        if (CheckBoxControl.checked == true) {
		            var i;
		            for (i = 0; i < document.forms[0].elements.length; i++) {
		                if ((document.forms[0].elements[i].type == 'checkbox') &&
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
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
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
		                    document.forms[0].elements[i].checked = false;
		                }
		            }
		        }
		    }

		    function SeleccionarOperaciones() {
		        var i;
		        var count;
		        count = 0;
		        for (i = 0; i < document.forms[0].elements.length; i++) {
		            if ((document.forms[0].elements[i].type == 'checkbox') &&
			(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
		                if (document.forms[0].elements[i].disabled != true) {
		                    if (document.forms[0].elements[i].checked) {
		                        count = count + 1;
		                    }
		                }
		            }
		        }
		        if (count > 0) {
		            return true;
		        }
		        else {
		            alertify.alert("Debe seleccionar algún registro! ");
		            return false;
		        }
		    }

		    function ShowProgress() {
		        setTimeout(function () {
		            $('body').addClass("modal");
		            var loading = $(".loading");
		            loading.show();
		        }, 200);
		    }

		    $(document).ready(function () {
		        $("#btnAprobar").click(function () {
		            ShowProgress();
		        });
		        $("#btnDesaprobar").click(function () {
		            ShowProgress();
		        });
		        $("#btnImprimir").click(function () {
		            ShowProgress();
		        });
		    });
		</script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <asp:updatepanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
    <div class="container-fluid">
        <header>
            <h2>
                Extorno de Movimiento Caja
            </h2>
        </header>
        <br />
        <fieldset>
         <legend></legend>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Extorno Inicio</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                            <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Extorno Fin</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                            <span class="add-on" id="imgFechaFin"><i class="awe-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Motivo</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlMotivo" Width="280px" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Estado</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlEstado" runat="server" Width="120px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />            
        </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            <input type="hidden" id="hdCodigoExtorno" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoExtorno") %>'>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NumeroOperacion" HeaderText="Nro. Operación">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Estado" HeaderText="Estado">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaLiquidado" HeaderText="Fecha Liq.">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS"  HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden"  ></asp:BoundField>
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IntermediarioDescripcion" HeaderText="Intermediario">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" >
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operación">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Referencia" HeaderText="Descripción">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NumeroCuenta" HeaderText="Nro.Cuenta">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Movimiento">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UsuarioCreacion" HeaderText="Usuario">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MotivoExtorno" HeaderText="Motivo">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" />
            <asp:Button ID="btnDesaprobar" runat="server" Text="Desaprobar" />
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
    </div>
    </ContentTemplate>
    </asp:updatepanel>
    </form>
</body>

</html>
