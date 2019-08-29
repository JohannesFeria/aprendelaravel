<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCotizacionVAC.aspx.vb"
    Inherits="Modulos_Parametria_CotizacionVAC_frmCotizacionVAC" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Indicadores</title>
    <script type="text/javascript">
        function ConfirmarCancelar() {
            return confirm('Seguro de cancelar el ingreso del detalle?');
        }
         //INICIO | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
        function ActualizarCodigoLibor() {
            var estructuraNombre = "XM - LIBOR - Y"
            var estructuraCodigo  = "SW-XML-Y"
            var fuenteLibor = $("#<%=ddlFuenteLibor.ClientID%>").val();
            var mesLibor = $("#<%=txtMesLibor.ClientID%>").val();

            estructuraNombre = estructuraNombre.replace('Y', fuenteLibor).replace('X', mesLibor);
            estructuraCodigo = estructuraCodigo.replace('Y', fuenteLibor.substring(0,1)).replace('X', mesLibor);
          $("#<%=txtCodigoIndicador.ClientID%>").val(estructuraCodigo);
          $("#<%=txtNombreIndicador.ClientID%>").val(estructuraNombre);

      }
      //FIN | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
        function ValidaBusqueda() {
            var isValid = false;
            isValid = Page_ClientValidate('vgCabecera');
            if (isValid) {
                isValid = Page_ClientValidate('vgBusquedaDetalle');
            }
            return isValid;
        }

        
        //   'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se crea validación de fecha de búsqueda para enviar campo vacío | 23/07/18
        $(document).ready(function () {

            //INICIO | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
            $("#<%=txtMesLibor.ClientID%>").change(function () {
                ActualizarCodigoLibor();
            });

            $("#<%=ddlFuenteLibor.ClientID%>").change(function () {
                ActualizarCodigoLibor();
            });
            //FIN | CDA | Ernesto Galarza | OT 11966 - Agregar controles para registrar indicadores para tasa libor
            $('#chkFechaBusqueda').change(function () {
                var $input = $(this);
                var validador = document.getElementById("<%=rvFechaBusqueda.ClientID%>");
                if ($input.is(':checked')) {
                    $('#divFechaBuscar').addClass('input-append date');
                    $('#tbFechaControl').attr('readonly', false);
                    $('#rvFechaBusqueda').attr('enabled', true);
                } else {
                    $('#divFechaBuscar').addClass('input-append');
                    $('#tbFechaControl').attr('readonly', true);
                    $('#tbFechaControl').val('');
                    $('#rvFechaBusqueda').attr('enabled', false);
                }
                ValidatorEnable(validador, $input.is(':checked'));
            });
        });
        //   'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se crea validación de fecha de búsqueda para enviar campo vacío | 23/07/18 
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"><h2>Mantenimiento de Indicadores</h2></div></div></header>
        <fieldset>
            <legend>Datos Generales</legend>

            
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tasa Libor</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlMostrarLibor" Width="120px" 
                                AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row">
                         <asp:Panel runat="server" ID="pnlLiborFuente" Visible="False"> 
                         <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Fuente</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList runat="server" ID="ddlFuenteLibor" Width="120px" />
                                </div>
                            </div>
                         </div>

                         <div class="col-md-6">
                                 <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Mes Libor</label>
                                    <div class="col-sm-9">
                                       <asp:TextBox runat="server" ID="txtMesLibor" min="1" type="number" CssClass="NumBox"  Width="120px"/>
                                    </div>
                                </div>

                         </div>
                         </asp:Panel>

                    </div>
           
                  
                
                    
                </div>


            </div>
        

            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            C&oacute;digo Indicador</label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="txtCodigoIndicador" Width="120px" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="C&oacute;digo Indicador"
                                ControlToValidate="txtCodigoIndicador" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Nombre Indicador</label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="txtNombreIndicador" Width="320px" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="Nombre Indicador"
                                ControlToValidate="txtNombreIndicador" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Manejo Periodo</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlManejaPeriodo" Width="120px" AutoPostBack="false" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="Manejo Periodo"
                                ControlToValidate="ddlManejaPeriodo" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tipo de Indicador</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="dllTipoIndicador" Width="120px" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="Tipo de Indicador"
                                ControlToValidate="dllTipoIndicador" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        
        </fieldset>
        <br />
        <fieldset>
            <legend>Buscar Registro</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                       <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se modifica campo de búsqueda para ingresar campo vacío | 23/07/18 --%>
                        <label class="col-sm-2 control-label">
                        </label>
                        <div class="col-sm-8">
                            <asp:CheckBox runat="server" Checked="true" ID="chkFechaBusqueda" Text="Fecha"/>
                            <div class="input-append date" id="divFechaBuscar">
                                <asp:TextBox runat="server" ID="tbFechaControl" SkinID="Date" ValidationGroup="vgBusquedaDetalle" />
                                <span runat="server" id="imgCalendar" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha" ControlToValidate="tbFechaControl" ID="rvFechaBusqueda"
                                ValidationGroup="vgBusquedaDetalle" runat="server" />
                        </div>
                        <%-- 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se modifica campo de búsqueda para ingresar campo vacío | 23/07/18 --%>
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscarDetalle" OnClientClick="return ValidaBusqueda()"
                        ValidationGroup="vgBusquedaDetalle" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla-small">
            <asp:GridView runat="server" SkinID="GridSmall" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Secuencia") & "|" & CType(Container, GridViewRow).RowIndex %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Secuencia") & "|" & CType(Container, GridViewRow).RowIndex %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Portafolio" />
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DiasPeriodo" HeaderText="D&iacute;as Periodo" />
                    <asp:BoundField DataField="Valor" HeaderText="Valor" />
                    <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se modifica <legend> para dar título a sección de modificar registro | 23/07/18 --%>
            <legend>
                <asp:Label ID="lblAddModRegistro" Text="Agregar Registro" runat="server"></asp:Label>
            </legend>
            <%-- 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se modifica <legend> para dar título a sección de modificar registro | 23/07/18 --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtValorCotizacion" CssClass="Numbox-7" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtValorCotizacion"
                                ErrorMessage="Valor" ValidationGroup="vgAgregarDetalle" /></div>
                    </div>
                </div>
                <div class="col-md-4" id="divDiasPeriodoPortafolio" runat="server">
                    <div class="form-group">
                        <asp:Label Text="" runat="server" ID="lblDiasPeriodo" CssClass="col-sm-4 control-label" />
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolioSBS" />
                            <asp:TextBox runat="server" ID="txtDiasPeriodo" CssClass="Numbox-0_9" />
                            <asp:RequiredFieldValidator ID="rvManejaPeriodo" ErrorMessage="" ControlToValidate="ddlPortafolioSBS"
                                ValidationGroup="vgAgregarDetalle" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacionDetalle" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Valor</label>
                        <div class="col-sm-8">
                            <div class="input-append date" id="divFechaValor" runat="server">
                                <asp:TextBox runat="server" ID="tbFechaValor" SkinID="Date" ValidationGroup="vgAgregarDetalle"/>
                                <span runat="server" id="imgFechaValor" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbFechaValor" ErrorMessage="Fecha Valor"
                                ValidationGroup="vgAgregarDetalle" /></div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregarDetalle" ValidationGroup="vgAgregarDetalle" />
                    <asp:Button Text="Modificar" runat="server" ID="btnModificarDetalle" ValidationGroup="vgAgregarDetalle" />
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Grabar" runat="server" ID="btnAceptar" 
                    ValidationGroup="vgCabecera" style="height: 26px" />
                <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdCodigo" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgCabecera" />
    <asp:ValidationSummary runat="server" ID="vsBusquedaDetalle" ShowMessageBox="true"
        ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgBusquedaDetalle" />
    <asp:ValidationSummary runat="server" ID="vsAgregarDetalle" ShowMessageBox="true"
        ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgAgregarDetalle" />
    </form>
</body>
</html>
