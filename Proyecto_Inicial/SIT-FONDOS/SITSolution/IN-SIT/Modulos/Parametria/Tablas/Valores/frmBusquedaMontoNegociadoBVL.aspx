<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaMontoNegociadoBVL.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaMontoNegociadoBVL" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Monto Negociado BVL</title>
    <script type="text/javascript">
        function showPopupTipoMnemonico() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=TipoInstrumento', '1200', '600', '');    
        }

        function formatCurrency(cajatexto) {

            var num = "";
            switch (cajatexto) {
                case "tbMontoEfectivo":
                    num = tbMontoEfectivo.value; break;
                case "tbPrecio":
                    num = tbPrecio.value; break;
                case "tbCantidad":
                    num = tbCantidad.value; break;
                case "tbNumeroOperacion":
                    num = tbNumeroOperacion.value; break;
            }

            num = num.toString().replace(/$|,/g, '');
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

                switch (cajatexto) {
                    case "tbMontoEfectivo":
                        tbMontoEfectivo.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbPrecio":
                        tbPrecio.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbCantidad":
                        tbCantidad.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbNumeroOperacion":
                        tbNumeroOperacion.value = (((sign) ? '' : '-') + num); break;
                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Monto Negociado BVL</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Operación</label>
                <div class="col-sm-9">
                <div class="input-append date">
                    <asp:TextBox runat="server" ID="tbFechaVigencia" SkinID="Date" />
                    <span class="add-on"><i class="awe-calendar"></i></span>
                </div>
            </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class ="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Número Operación</label>
                <div class="col-sm-9">
                <asp:textbox id="tbNumeroOperacion" onblur="Javascript:formatCurrency(tbNumeroOperacion.id); return false;" runat="server" MaxLength="50" Width="73px" ></asp:textbox>
            </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnemónico</label>
                <div class="col-sm-9">
                <div class="input-append">
                    <asp:TextBox runat="server" ID="tbCodigoMnemonico" CssClass="input-medium" />
                    <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" OnClientClick="return showPopupTipoMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                </div>
            </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situación</label>
                <div class="col-sm-9">
                <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px"></asp:dropdownlist>
            </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align:right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
    <div class="row">
        <asp:label id="lbContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="grilla">
        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid"
            GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NumeroOperacion") %>'>                            
                        </asp:ImageButton>
                        <asp:HiddenField ID="hdFechaOperacion" runat="server" value="<%# Bind('FechaOperacion') %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NumeroOperacion") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="strFechaOperacion" HeaderText="Fecha Operacion"></asp:BoundField>
                <asp:BoundField DataField="NumeroOperacion" HeaderText="Numero Operacion"></asp:BoundField>
                <asp:BoundField DataField="DescripcionMnemonico" HeaderText="Mnemonico"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="FechaOperacion" HeaderText="FechaOperacion"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoMnemonico" HeaderText="Mnemonico"></asp:BoundField>
            </Columns>
        </asp:GridView>
        </div>
    </ContentTemplate>

    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    </Triggers>

    </asp:UpdatePanel>        

    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <br />
    </div>
    </form>
</body>
</html>
