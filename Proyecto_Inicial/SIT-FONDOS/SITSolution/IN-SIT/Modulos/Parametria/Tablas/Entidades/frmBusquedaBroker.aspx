<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaBroker.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Entidades_frmBusquedaBroker" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Broker / Comisiones</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="smBrokerComisiones" />
    <div class="container-fluid">
        <header>
            <h2>
                Broker / Comisiones</h2>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Código Entidad</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtCodigoEntidad" runat="server" MaxLength="4" Width="64px"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Descripción</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="30" Width="267px"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Situación</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tipo de Tramo</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlTipoTramo" runat="server" Width="115px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-9">
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                    </div>
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
            <asp:UpdatePanel ID="upLista" runat="server">
                <ContentTemplate>

                <asp:GridView ID="dgLista" runat="server" SkinID="Grid" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Tramo") %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Tramo") %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoEntidad" HeaderText="C&#243;digo Entidad"></asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"></asp:BoundField>
                    <asp:BoundField DataField="TipoTramo" HeaderText="Tipo Tramo"></asp:BoundField>
                    <asp:BoundField DataField="Tramo" HeaderText="Tramo"></asp:BoundField>
                    <asp:BoundField DataField="BandaInferior" HeaderText="BI" DataFormatString="{0:#,##0}">
                    </asp:BoundField>
                    <asp:BoundField DataField="BandaSuperior" HeaderText="BS" DataFormatString="{0:#,##0}">
                    </asp:BoundField>
                    <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:#,##0.0000}">
                    </asp:BoundField>
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
                    <asp:BoundField Visible="False" DataField="TipoCosto" HeaderText="TipoCosto"></asp:BoundField>
                </Columns>
                </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="false" />            
        </div>
        <br />
    </div>
    </form>
</body>
</html>
