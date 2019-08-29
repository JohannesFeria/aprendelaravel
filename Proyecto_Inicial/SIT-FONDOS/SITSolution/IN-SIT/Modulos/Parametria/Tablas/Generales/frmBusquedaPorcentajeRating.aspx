<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaPorcentajeRating.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaPorcentajeRating" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Porcentajes por Rating</title>
    <script type="text/javascript">
        function showPopup() {
            return showModalDialog('../../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Rating', '1200', '600', '');    
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Porcentajes por Rating</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Rating</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbRating" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarRating" runat="server" OnClientClick="return showPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Categoria</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlCategoria" runat="server" Width="340px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="160px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situacion</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="160px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />

    <asp:UpdatePanel ID="UP1" runat="server">
    <ContentTemplate>

    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
        <asp:label id="lbContador" runat="server"></asp:label>
    </fieldset>
    <br />
    <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEditar"  runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoRating") & ";" & DataBinder.Eval(Container, "DataItem.CategInver") & ";" & DataBinder.Eval(Container, "DataItem.CodigoPortafolioSBS") & ";" & DataBinder.Eval(Container, "DataItem.GrupoRating") %>'>
                            </asp:ImageButton>&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                AlternateText="Eliminar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoRating") & ";" & DataBinder.Eval(Container, "DataItem.CategInver") & ";" & DataBinder.Eval(Container, "DataItem.CodigoPortafolioSBS") & ";" & DataBinder.Eval(Container, "DataItem.GrupoRating") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescRating" HeaderText="Rating"></asp:BoundField>
                    <asp:BoundField DataField="DescCategInver" HeaderText="Categoria Inversi&#242;n">
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Código Portafolio" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Portafolio" />
                    <asp:BoundField DataField="ValorPorcentaje" HeaderText="Porcentaje" DataFormatString="{0:##0.00}">
                    </asp:BoundField>
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&#242;n"></asp:BoundField>
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
    <br />
    </div>
    </form>
</body>
</html>
