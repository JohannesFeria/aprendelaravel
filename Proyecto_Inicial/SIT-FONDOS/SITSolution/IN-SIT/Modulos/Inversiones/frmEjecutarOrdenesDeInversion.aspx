<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmEjecutarOrdenesDeInversion.aspx.vb" Inherits="Modulos_Inversiones_frmEjecutarOrdenesDeInversion" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Ejecutar() {
            var strNroOrden = "";

            strNroOrden = document.getElementById("lbNroTransaccion").innerText;

            if (strNroOrden != "") {
                if (confirm("¿Desea ejecutar la(s) orden(es) seleccionada(s)?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('dgListaOPE') > -1)) {
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
						(document.forms[0].elements[i].name.indexOf('dgListaOPE') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
   <asp:UpdatePanel ID="upcamb" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div class="container-fluid">
    <header><h2>Ejecutar Ordenes de Inversión</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fondo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlFondoOPE" runat="server" AutoPostBack="True" Width="140px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-3 control-label">Nro. Orden</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtNroOrdenOPE" runat="server" Width="140px" ></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-6 control-label">Fecha de Operación</label>
                <div class="col-sm-6">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3" style="text-align: right;">           
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <div class="row" style="text-align: center;">
        Ordenes Por Ejecutar
    </div>
    <div class="grilla">
        <asp:GridView ID="dgListaOPE" runat="server" AutoGenerateColumns="False" SkinID="Grid" >            
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="dgListaOPE_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Hora" HeaderText="Hora"></asp:BoundField>
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="FechaOperacion" HeaderText="Fecha Operaci&#243;n"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="Fondo" HeaderText="Fondo"></asp:BoundField>
                <asp:BoundField DataField="NumeroTransaccion" HeaderText="Nro Orden"></asp:BoundField>
                <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operacion"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="Estado" HeaderText="Estado"></asp:BoundField>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                <asp:BoundField DataField="Precio" HeaderText="Precio"></asp:BoundField>
                <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="ISIN" HeaderText="ISIN"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="TipoOrden" HeaderText="TipoOrden"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="Categoria" HeaderText="Categoria"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div class="row" style="text-align: center;">
        Ordenes Ejecutadas
    </div>
    <div class="grilla">
        <asp:GridView ID="dgListaOE" runat="server" AutoGenerateColumns="False" SkinID="Grid">            
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgCheck"
                            CommandName="Seleccionar"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operaci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Fondo" HeaderText="Fondo"></asp:BoundField>
                <asp:BoundField DataField="NumeroTransaccion" HeaderText="Nro Orden"></asp:BoundField>
                <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operacion"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="ISIN" HeaderText="ISIN"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="TipoOrden" HeaderText="TipoOrden"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="Categoria" HeaderText="Categoria"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <fieldset>
    <legend>Detalle de Ejecución de Ordenes de Inversión</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código ISIN</label>
                <div class="col-sm-9">                    
                    <asp:TextBox ID="lbCodigoISIN" runat="server" Text="" Width="120px" ReadOnly="true"></asp:TextBox>                    
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Nro. Transacción</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="lbNroTransaccion" runat="server" Text="" Width="96px" ReadOnly="true" ></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Operación</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="lbTipoOperacion" runat="server" Text="" Width="120px" ReadOnly="true" ></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9">
                    <asp:label id="lbParametros" runat="server" CssClass="stlEtiqueta" Visible="False"></asp:label>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Orden</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="lbTipoOrden" runat="server" Text="" ReadOnly="true" Width="279px"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9">
                    
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnEjecutar" runat="server" Text="Ejecutar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgLista" runat="server" Visible="False">            
            <Columns>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="codigoComision1" HeaderText="C&#243;digo Impuesto/Comisi&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisi&#243;n"></asp:BoundField>
                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisi&#243;n"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="strValorCalculadoComision1" HeaderText="Comisi&#243;n"></asp:BoundField>
                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                    <ItemTemplate>
                        <asp:TextBox ID="txtValorComision1" runat="server" CssClass="stlCajaTextoNumero"
                            Width="200px" MaxLength="23"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="ValorOcultoComision1" HeaderText="ValorOcultoComision1"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="codigoComision2" HeaderText="C&#243;digo Impuesto/Comisi&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisi&#243;n"></asp:BoundField>
                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisi&#243;n"></asp:BoundField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="strValorCalculadoComision2" HeaderText="Comisi&#243;n"></asp:BoundField>
                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                    <ItemTemplate>
                        <asp:TextBox ID="txtValorComision2" runat="server" CssClass="stlCajaTextoNumero"
                            Width="200px" MaxLength="23"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" DataField="ValorOcultoComision2" HeaderText="ValorOcultoComision2"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>