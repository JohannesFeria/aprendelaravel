<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmParamCalculoRebates.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmParamCalculoRebates" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Cálculo de Rebates</title>
    <script type="text/javascript">
        function ShowPopup() {
            return showModalDialog('../../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Valores', '1200', '600', '');    
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Cálculo de Rebates</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnémonico</label>
                <div class="col-sm-9 ">                                
                    <div class="input-append">
                    <asp:TextBox runat="server" ID="tbNemonico" CssClass="input-medium" />
                    <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" CausesValidation="false" OnClientClick="return ShowPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>                    
                </div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="Mnémonico" ControlToValidate="tbNemonico" 
                    ValidationGroup="btnIngresar">(*)</asp:RequiredFieldValidator>
                </div>                
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Días de Calculo</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbDias" runat="server" MaxLength="50" Width="88px" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="Días de Calculo" ControlToValidate="tbDias" 
                    ValidationGroup="btnIngresar">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Rebate General (%)</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbRebate" runat="server" MaxLength="50" Width="172px" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ErrorMessage="Rebate General (%)" ControlToValidate="tbRebate" 
                    ValidationGroup="btnIngresar">(*)</asp:RequiredFieldValidator>
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
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="140px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9">
                    <asp:CheckBox ID="chkRango" runat="server" Text="Por rango de inversion" />
                    <asp:CheckBox ID="chkSumatoriafondos" runat="server" Text="Sumatoria de Fondos" />
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" 
                ValidationGroup="btnIngresar" />
        </div>        
    </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Detalle rango de Inversion</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Importe Inicial</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbImpInicial" runat="server" Width="160px" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ErrorMessage="Importe Inicial" ControlToValidate="tbImpInicial" 
                    ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Importe Final</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbImpFinal" runat="server" Width="160px" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ErrorMessage="Importe Final" ControlToValidate="tbImpFinal" 
                    ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Rebate (%)</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbPorRebate" runat="server" Width="160px" MaxLength="4" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ErrorMessage="Rebate (%)" ControlToValidate="tbPorRebate" 
                    ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situacion (%)</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlSituacionDet" runat="server" Width="140px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:label id="Label1" runat="server" Visible="False" Text=""></asp:label>
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" 
                ValidationGroup="btnAgregar" />
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" 
                ValidationGroup="btnAgregar" />
        </div>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">                    
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit"
                                    CommandName="Modificar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ImporteIni")&","&DataBinder.Eval(Container, "DataItem.ImporteFin")&","&DataBinder.Eval(Container, "DataItem.PorcRebate") &","&DataBinder.Eval(Container, "DataItem.Situacion")&","&DataBinder.Eval(Container, "DataItem.CodigoCalculoRebateDet")%>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete"
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ImporteIni")&amp;","&amp;DataBinder.Eval(Container, "DataItem.ImporteFin")&amp;","&amp;DataBinder.Eval(Container, "DataItem.PorcRebate") &amp;","&amp;DataBinder.Eval(Container, "DataItem.Situacion")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoCalculoRebateDet")%>'
                                    CommandName="Eliminar"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ImporteIni" HeaderText="ImporteInicial"></asp:BoundField>
                        <asp:BoundField DataField="ImporteFin" HeaderText="ImporteFinal"></asp:BoundField>
                        <asp:BoundField DataField="PorcRebate" HeaderText="% Rebate"></asp:BoundField>
                        <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                        <asp:BoundField Visible="False" DataField="CodigoCalculoRebateDet" HeaderText="CodigoCalculoRebateDet"></asp:BoundField>
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
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button id="btnAceptar" runat="server" Text="Aceptar"/>
        <asp:Button id="btnRetornar" runat="server" Text="Retornar"/>
        <asp:HiddenField ID="hd" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" ValidationGroup="btnIngresar"
        HeaderText="Los siguientes campos son obligatorios:" />
        <asp:ValidationSummary runat="server" ID="ValidationSummary1" ShowMessageBox="true" ShowSummary="false" ValidationGroup="btnAgregar"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
