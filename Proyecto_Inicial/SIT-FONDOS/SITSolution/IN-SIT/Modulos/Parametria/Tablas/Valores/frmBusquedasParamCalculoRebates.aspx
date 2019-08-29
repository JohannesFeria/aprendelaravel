<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedasParamCalculoRebates.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedasParamCalculoRebates" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Cálculo Rebates</title>
    <script language="javascript">        
        function ShowPopup() {            
            return showModalDialog('../../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Valores', '800', '600', ''); 
        }
		</script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Cálculo Rebates</h2></header>
        <br />
        <fieldset>
        <legend>Datos Generales</legend>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Mnemónico</label>
                    <div class="col-sm-9">
                        <div class="input-append">
                            <asp:TextBox runat="server" ID="tbNemonico" CssClass="input-medium" />
                            <asp:LinkButton ID="lkbBiscarMnemónico" runat="server" CausesValidation="false" OnClientClick="return ShowPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
            </div>
        </div>
        </fieldset>
        <br />
        <fieldset>
        <legend>Resultados de la Búsqueda</legend>
        <div class="row">
            <asp:Label ID="lblContador" runat="server"></asp:Label>
        </div>
        </fieldset>
        <br />
        <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoNemonico")&","&DataBinder.Eval(Container, "DataItem.Situacion")%>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoNemonico")&","&DataBinder.Eval(Container, "DataItem.Situacion")%>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoNemonico" HeaderText="Instrumento"></asp:BoundField>
                    <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
        </asp:UpdatePanel>
        </div>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>
