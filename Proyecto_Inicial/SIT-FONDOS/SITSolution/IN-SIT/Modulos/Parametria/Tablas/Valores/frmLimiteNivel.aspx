<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLimiteNivel.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmLimiteNivel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Límite</title>
    <script type="text/javascript">
        function Cerrar() { window.close(); }
        function marcar(obj) {
            alertify.alert(obj.parentElement);
            alertify.alert(obj);
            var txt = null;
            if (obj.parentElement.parentElement.cells(3) == null) //RGF 20081118 salia error para limite 18
                txt = obj.parentElement.parentElement.cells(2).childNodes(0);
            else
                txt = obj.parentElement.parentElement.cells(3).childNodes(0);
            if (obj.checked) { txt.disabled = false; }
            else {
                txt.value = "";
                txt.disabled = true;
            }
        }
        function TodosOnClick(obj) {
            var row = null;
            var chk = null;
            var txt = null;
            var i = 1;
            if (obj == null)
                return;
            for (i = 1; i <= obj.rows.length - 1; i++) {
                row = obj.rows(i);
                chk = row.cells(0).childNodes(0);
                txt = row.cells(3).childNodes(0);
                if (chk.checked == true) {

                }
                else {
                    txt.value = "";
                }
            }
        }
        function ActivateTextBox(chk) {
            var Controlchk = chk.id;
            var Change = Controlchk.replace('Checkbox1', 'txtValor')
            if (document.getElementById(Controlchk).checked == true) {
                document.getElementById(Change).readOnly = false;
            } else {
                document.getElementById(Change).readOnly = true;
            }
        }

        function RegistrarValor(valor) {
            var control = valor.id;
            document.getElementById(control).innerHTML = valor.value;
        }

        function RegistrarCelda(filaSel, valor) {
            var fila = filaSel;
            var grd = document.getElementById('<%= dgListaValores.ClientID %>');
            grd.rows[fila].cells[10].childNodes[0].value = valor.value;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uppagina" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <header>
            <h2>Características - Nivel</h2>
        </header>
                <asp:Panel ID="pnlPortafolio" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                    Portafolio</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="200px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <div class="grilla">
                    <div id="divDetalle" runat="server">
                        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                            CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNivelLimite") %>'>
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                            CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNivelLimite") %>'>
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoNivelLimite"
                                    HeaderText="Código Nivel"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoLimiteCaracteristica"
                                    HeaderText="Código Limite Caracteristica"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoCaracteristica"
                                    HeaderText="Código Grupo Instrumento"></asp:BoundField>
                                <asp:BoundField DataField="Secuencial" HeaderText="Orden Nivel"></asp:BoundField>
                                <asp:BoundField DataField="DescripcionCaracteristica" HeaderText="Caracteristica">
                                </asp:BoundField>
                                <asp:BoundField DataField="FlagTipoPorcentaje" HeaderText="Porcentaje"></asp:BoundField>
                                <asp:BoundField DataField="ValorPorcentaje" HeaderText="Valor"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Situacion"
                                    HeaderText="Situación"></asp:BoundField>
                                <asp:BoundField DataField="TieneLimiteNivel" HeaderText="Limite Nivel"></asp:BoundField>
                                <asp:BoundField DataField="LimiteNivelMin" HeaderText="Limite Nivel Min."></asp:BoundField>
                                <asp:BoundField DataField="LimiteNivelMax" HeaderText="Limite Nivel Max"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <br />
                <fieldset>
                    <legend>Nivel</legend>
                    <asp:HiddenField ID="hdCodigoLimiteNivel" runat="server" Value="" />
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                    Orden Nivel</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tbOrdenNivel" runat="server" Width="109px" MaxLength="50" CssClass="Numbox-0_2" />
                                    <strong>
                                        <asp:RequiredFieldValidator ID="rfvOrdenNivel" runat="server" ErrorMessage="Orden Nivel"
                                            ControlToValidate="tbOrdenNivel" ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                    Universo de Límites</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtGrupoInstrumento" runat="server" Width="250px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                    Característica</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlCaracteristica" runat="server" AutoPostBack="True" Width="250px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:CheckBox ID="chkLimiteNIvel" AutoPostBack="true" runat="server" Text=" "></asp:CheckBox>
                                <asp:Label ID="lblLimiteNivel" runat="server">Tiene Limite Nivel Característica(%)</asp:Label>
                                <asp:Label ID="Label1" runat="server">Min.&nbsp;</asp:Label>
                                <asp:TextBox Style="z-index: 0" ID="txtLimiteNIvelMin" runat="server" Width="109px"
                                    MaxLength="50" CssClass="Numbox-7" />
                                <asp:Label ID="Label2" runat="server">Max.&nbsp;</asp:Label>
                                <asp:TextBox ID="txtLimiteNivelMax" runat="server" Width="109px" MaxLength="50" Style="z-index: 0"
                                    CssClass="Numbox-7" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                    Opción Nivel</label>
                                <div class="col-md-8">
                                    <asp:RadioButtonList ID="rblOpcion" runat="server" AutoPostBack="True" Width="300px">
                                        <asp:ListItem Value="G" Selected="True">Porcentaje General</asp:ListItem>
                                        <asp:ListItem Value="A">Porcentaje Por Agrupación</asp:ListItem>
                                        <asp:ListItem Value="D">Porcentaje por Detalle de Agrupación</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                  <asp:Label runat="server" ID="lblCaracteristicaAnterior" Visible="false" >Caracteristica Anterior</asp:Label>    
                                  </label>
                                <div class="col-md-8">
                                    <asp:DropDownList Id="ddlCaracteristicaAnterior" runat="server" Width="250px" Visible="false" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="grilla">
                        <div id="tdCentro" runat="server">
                            <div class="grilla-small">
                                <div id="divPorcentaje_Valores" runat="server">
                                    <asp:CheckBox ID="chkLimiteEnDetalle" AutoPostBack="true" runat="server" Text=" ">
                                    </asp:CheckBox>
                                    <asp:Label ID="Label3" runat="server">Tiene Limite Fijo en Detalle Característica (%)</asp:Label>
                                    <asp:Label ID="Label4" runat="server">Min.&nbsp;</asp:Label>
                                    <asp:TextBox Style="z-index: 0" ID="txtLImiteEnDetalleMin" AutoPostBack="true" runat="server"
                                        Width="109px" MaxLength="50" CssClass="Numbox-7" />
                                    <asp:Label ID="Label5" runat="server">Max.&nbsp;</asp:Label>
                                    <asp:TextBox ID="txtLImiteEnDetalleMax" runat="server" Width="109px" MaxLength="50"
                                        Style="z-index: 0" CssClass="Numbox-7" AutoPostBack="True" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="dgListaValores" runat="server" SkinID="Grid_AllowPaging_NO" Style="margin-top: 0px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Checkbox1" ToolTip="Permite que este detalle de caracteristica sea considerada a la hora de procesar el limite"
                                                        runat="server" Text="" OnClick="ActivateTextBox(this);" />
                                                    <asp:HiddenField ID="hdValorPorcentaje" runat="server" Value="<%# Bind('ValorPorcentaje') %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Selected"
                                                HeaderText="Selected"></asp:BoundField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoNivelLimite"
                                                HeaderText="CodigoNivelLimite"></asp:BoundField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoCaracteristica"
                                                HeaderText="CodigoCaracteristica"></asp:BoundField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="NombreVista"
                                                HeaderText="NombreVista"></asp:BoundField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ClaseNormativa"
                                                HeaderText="Clase Normativa"></asp:BoundField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorCaracteristica"
                                                HeaderText="ValorCaracteristica"></asp:BoundField>
                                            <asp:BoundField DataField="DescripcionValorCaracteristica" HeaderText="Descripción">
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkValorEspecifico" AutoPostBack="true" ToolTip="Permite que este detalle de caracteristica tenga valor especifico"
                                                        runat="server" Text="" OnCheckedChanged="chkValorFijo_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorPorcentaje"
                                                HeaderText="ValorPorcentaje"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Valor Minimo (%)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtValor" Width="100px" ReadOnly="true" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Situacion"
                                                HeaderText="Situación"></asp:BoundField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorPorcentajeM"
                                                HeaderText="ValorPorcentajeM"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Valor Maximo (%)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtValorM" Width="100px" ReadOnly="true" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorEspecifico"
                                                HeaderText="Selected"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <asp:CheckBox ID="chkTodos" runat="server" Text=" "></asp:CheckBox><asp:Label ID="lblTodos"
                                runat="server">Todos los demas:</asp:Label><asp:TextBox ID="txtTodos" runat="server"></asp:TextBox>
                            <br />
                            <br />
                            <fieldset>
                                <legend>Condiciones de Nivel</legend>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group" style="padding: 5px;">
                                            <asp:Label ID="Label6" runat="server">Campo Vista</asp:Label>
                                            <asp:TextBox Style="z-index: 0" ID="txtCampoCon" runat="server" Width="109px" MaxLength="50" />
                                            <asp:Label ID="Label8" runat="server">= Valor Condición</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtValorCon" runat="server" Width="109px" MaxLength="250" Style="z-index: 0" />
                                            <asp:Button ID="btnAgregarCondicion" runat="server" Text="Agregar Condición" />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            Condición:
                                            <asp:Label ID="lblCondicion" runat="server" Text="" />
                                            <br />
                                            <br />
                                            <asp:Label ID="Label7" runat="server">Valor Min. (%)&nbsp;</asp:Label>
                                            <asp:TextBox Style="z-index: 0" ID="txtValorMinCon" runat="server" Width="109px"
                                                MaxLength="50" CssClass="Numbox-7" />
                                            <asp:Label ID="Label9" runat="server">Valor Max. (%)&nbsp;</asp:Label>
                                            <asp:TextBox ID="txtValorMaxCon" runat="server" Width="109px" MaxLength="50" Style="z-index: 0"
                                                CssClass="Numbox-7" />
                                            <asp:Button ID="btnGrabarCondicion" runat="server" Text="Grabar Condición" />
                                            <br />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                                <div class="grilla-small">
                                    <div id="GrillaCondicion" runat="server">
                                        <asp:GridView ID="dgGrillaCondicion" runat="server" SkinID="Grid_AllowPaging_NO" Style="margin-top: 0px">
                                            <Columns>                                                
                                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoNivelLimite"
                                                    HeaderText="CodigoNivelLimite"></asp:BoundField>
                                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Secuencial"
                                                    HeaderText="Secuencial"></asp:BoundField>                                                
                                                <asp:BoundField DataField="Condicion" HeaderText="Condición">
                                                </asp:BoundField>     
                                                <asp:BoundField DataField="PorcentajeMin" HeaderText="Valor Min. %">
                                                </asp:BoundField>    
                                                <asp:BoundField DataField="PorcentajeMax" HeaderText="Valor Max. %">
                                                </asp:BoundField>                                               
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div id="tdIzqPorcentajeValor" runat="server">
                                    <label class="col-md-4 control-label">
                                        <asp:Label ID="lblValor" runat="server">Valor (%)</asp:Label></label>
                                </div>
                                <div id="tdPorcentajeValor" runat="server">
                                    <div class="col-md-8">
                                        <asp:Label ID="lbMin" runat="server">Min.&nbsp;</asp:Label><asp:TextBox Style="z-index: 0"
                                            ID="tbValorMin" runat="server" Width="109px" MaxLength="50" Visible="False" CssClass="Numbox-7" />
                                        <asp:Label ID="lbMax" runat="server">Max.&nbsp;</asp:Label><asp:TextBox ID="tbValor"
                                            runat="server" Width="109px" MaxLength="50" Style="z-index: 0" CssClass="Numbox-7" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-md-4 control-label">
                                </label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px" Visible="False">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: right">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
                    </div>
                </fieldset>
                <header>
        </header>
                <div class="row" style="text-align: right;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CausesValidation="False" />
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="False" OnClientClick="Cerrar();" />
                </div>
            </div>
            <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
                HeaderText="Los siguientes campos son obligatorios:" />
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlCaracteristicaAnterior" EventName ="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
