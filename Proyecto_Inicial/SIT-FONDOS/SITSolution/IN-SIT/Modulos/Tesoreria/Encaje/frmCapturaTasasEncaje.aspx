<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCapturaTasasEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmCapturaTasasEncaje" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Tasas de Encaje</title>
    <script type="text/javascript">
        function showPopupMnemonico() {
            $('#hdTipoBusqueda').val('M');
            return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '800', '600', '');                      
        }
        function ShowPopup() {
            $('#hdTipoBusqueda').val('E');
            return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '800', '600', '');             
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Tasas de Encaje</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Calificación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlCalificacion" runat="server" Width="160px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Vigencia desde</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbVigenciaDesde" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tasas Vigentes al día</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtVigenciaA_Control" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Emisor</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbemisor" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarEmisor" runat="server" OnClientClick="return ShowPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnemónico </label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbNemonico" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
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
        <asp:label id="lbContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />    
    <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server" >
            <ContentTemplate>
                <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                    CommandName="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SecuenciaTasa") %>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SecuenciaTasa") %>'
                                    CommandName="Eliminar"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField Visible="False" DataField="SecuenciaTasa" HeaderText="SecuenciaTasa">
                        </asp:BoundField>
                        <asp:BoundField DataField="codigoMnemonico" HeaderText="Codigo Mnemonico"></asp:BoundField>
                        <asp:BoundField DataField="CodigoEntidad" HeaderText="Emisor"></asp:BoundField>
                        <asp:BoundField DataField="CodigoCalificacion" HeaderText="Calificacion"></asp:BoundField>
                        <asp:BoundField DataField="ValorTasaEncaje" HeaderText="Tasa" DataFormatString="{0:#,##0.0000000}">
                        </asp:BoundField>
                        <asp:BoundField DataField="Situacion" HeaderText="Estado"></asp:BoundField>
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
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    <asp:HiddenField ID="hdEmisor" runat="server" />
    <asp:HiddenField ID="hdTipoBusqueda" runat="server" />
    <br />
    </form>
</body>
</html>