<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTiposInstrumentos.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmTiposInstrumentos" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Tipo de Instrumentos</title>
    <script type="text/javascript">
        function showPopupEntidad() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');               
        }
        function formatCurrencyValorNominal(num) {
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
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                txtTasaEncaje.value = (((sign) ? '' : '-') + num + '.' + cents);
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Mnatenimiento de Tipo de Instrumentos</h2></header>
        <br />
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Tipo Instrumento</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbCodigo" runat="server" Width="96px" MaxLength="12"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Código Tipo Instrumento" ControlToValidate="tbCodigo" 
                                ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ErrorMessage="Código Tipo Instrumento" ControlToValidate="tbCodigo" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Encaje</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtTasaEncaje" onblur="Javascript:formatCurrencyValorNominal(txtTasaEncaje.value); return false;"
                                Width="100px" MaxLength="22" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                ErrorMessage="Tasa Encaje" ControlToValidate="txtTasaEncaje" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Tipo Instrumento SBS</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtCodigoSBS" runat="server" Width="96px" MaxLength="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ErrorMessage="Código Tipo Instrumento SBS" ControlToValidate="txtCodigoSBS" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tipo de Tasa</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoTasa" runat="server" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Descripción</label>
                        <div class="col-sm-7">
                            <asp:textbox id="tbDescripcion" runat="server" Width="300px" MaxLength="50" TextMode="MultiLine" Height="32px"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                ErrorMessage="Descripción" ControlToValidate="tbDescripcion" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tipo Valorización</label>
                        <div class="col-sm-7">
                            <asp:dropdownlist id="ddlTipoValoracion" runat="server" Width="150px"></asp:dropdownlist>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                ErrorMessage="Tipo Valorización" ControlToValidate="ddlTipoValoracion" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tipo de Renta</label>
                        <div class="col-sm-7">
                            <asp:dropdownlist id="ddlTipoRenta" runat="server"  Width="152px"></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Situación</label>
                        <div class="col-sm-7">
                            <asp:dropdownlist id="ddlSituacion" runat="server" Width="150px"></asp:dropdownlist>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Clase de Instrumento</label>
                        <div class="col-sm-7">
                            <asp:dropdownlist id="ddlClaseInstrumento" runat="server" Width="256px"></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Grupo Riesgo</label>
                        <div class="col-sm-7">
                            <asp:textbox id="txtGrupoRiesgo" runat="server" MaxLength="10" Width="100px" ></asp:textbox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">PND</label>
                        <div class="col-sm-7">
                            <asp:textbox id="txtPND" runat="server"  Width="96px"  CssClass="Numbox-2_2" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                                ErrorMessage="PND" ControlToValidate="txtPND" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">PNND</label>
                        <div class="col-sm-7">
                            <asp:textbox id="txtPNND" runat="server" Width="96px" CssClass="Numbox-2_2" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                                ErrorMessage="PNND" ControlToValidate="txtPNND" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Plazo de Liquidación</label>
                        <div class="col-sm-7">
                            <asp:textbox id="tbPlazoLiquidacion" runat="server" Width="40px" MaxLength="2"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                ErrorMessage="Plazo Liquidación" ControlToValidate="tbPlazoLiquidacion" 
                                ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Método Calculo Renta</label>
                        <div class="col-sm-7">
                            <asp:dropdownlist id="ddlMetodoCalculoRenta" runat="server" Width="200px" ></asp:dropdownlist>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header><h2>Cuentas BCR</h2></header>
        <br />
        <fieldset>
        <legend></legend>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-5 control-label">Código Entidad</label>
                    <div class="col-sm-7">
                        <div class="input-append">
                            <asp:TextBox runat="server" ID="tbCodigoEntidad" CssClass="input-medium" 
                                ReadOnly="True" />
                            <asp:LinkButton ID="lkbBuscarEntidad" runat="server" CausesValidation="false" OnClientClick="return showPopupEntidad();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                                ErrorMessage="Código Entidad" ControlToValidate="tbCodigoEntidad" 
                                ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-5 control-label">Cuenta Contable</label>
                    <div class="col-sm-7">
                        <asp:dropdownlist id="ddlCuentaContable" runat="server" Width="200px" AutoPostBack="True"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="Cuenta Contable" ControlToValidate="ddlCuentaContable" 
                                ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-5 control-label">Nombre Cuenta</label>
                    <div class="col-sm-7">
                        <asp:textbox id="txtNombreCuenta" runat="server" Width="392px" ReadOnly="True"></asp:textbox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ErrorMessage="Nombre Cuenta" ControlToValidate="txtNombreCuenta" 
                                ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6" style="text-align: right;">
                <div id="divBtnAgregar" runat="server"></div>
                <div id="divBtnModificar" runat="server"></div>
                <div id="divBtnCancelar" runat="server"></div>
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" ValidationGroup="btnAgregar" />                               
                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" ValidationGroup="btnAgregar" />                                
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" />                
            </div>
        </div>
        </fieldset>
        <br />        
        <div class="grilla">
        <asp:GridView ID="dgCuenta" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbModificar" runat="server" SkinID="imgEdit" CommandName="Select"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoEntidad") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoEntidad") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoEntidad" HeaderText="C&#243;digo Entidad"></asp:BoundField>
                            <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta"></asp:BoundField>
                            <asp:BoundField DataField="NombreCuenta" HeaderText="Descripcion"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
            <asp:UpdatePanel ID="UP1" runat="server">
                <ContentTemplate>
                    
                </ContentTemplate>
                <Triggers>
                   
                </Triggers>
            </asp:UpdatePanel>      
        </div>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                ValidationGroup="btnAceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
                CausesValidation="False" />
                <asp:HiddenField ID="hd" runat="server" />
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="btnAgregar"/>
    <asp:ValidationSummary runat="server" ID="ValidationSummary1" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="btnAceptar"/>
    </form>
</body>
</html>







