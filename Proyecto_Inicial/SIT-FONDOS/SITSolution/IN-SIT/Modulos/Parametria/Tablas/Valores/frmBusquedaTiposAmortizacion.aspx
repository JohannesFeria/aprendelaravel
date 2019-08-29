<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaTiposAmortizacion.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaTiposAmortizacion" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Tipo de Amortización</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Tipo de Amortización</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Descripción</label>
            <div class="col-sm-9">
                <asp:TextBox id="tbDescripcion" runat="server" MaxLength="30"  Width="296px"></asp:TextBox>
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
                <asp:dropdownlist id="ddlSituacion" runat="server"  Width="115px"></asp:dropdownlist>
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
        <asp:Label id="lbContador" runat="server"></asp:Label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UP1" runat="server">
        <ContentTemplate>
            <asp:GridView id="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">						
						<Columns>
							<asp:TemplateField HeaderText="" ItemStyle-Width="25px">
								<ItemTemplate>
									<asp:ImageButton id="ibModificar" runat="server" SkinID="imgEdit"
                                    OnCommand="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTipoAmortizacion") %>'>
									</asp:ImageButton>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="" ItemStyle-Width="25px">
								<ItemTemplate>
									<asp:ImageButton id="ibEliminar" runat="server" SkinID="imgDelete"
                                    OnCommand="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTipoAmortizacion") %>'>
									</asp:ImageButton>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="CodigoTipoAmortizacion" HeaderText="C&#243;digo Tipo Amortizaci&#243;n"></asp:BoundField>
							<asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
							<asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
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
