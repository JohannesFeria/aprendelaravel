<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmNivelesCobertura.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Limites_frmNivelesCobertura" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Niveles de Cobertura</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Mantenimiento de Niveles de Cobertura</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tercero
                        </label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlTercero" Width="320px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tercero" ControlToValidate="ddlTercero"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <br />

            <div class="row">
                <!--<div class="col-md-12">-->
                <div class="grilla-small">
                    <asp:GridView ID="dgthreshold" runat="server" SkinID="GridSmall" AutoGenerateColumns="False"
                        PageSize="5" DataKeyNames="codigoportafolio">
                        <Columns>
                            <asp:BoundField DataField="codigoportafolio" HeaderText="Codigo Portafolio" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                            <%--<asp:BoundField DataField="Threshold" HeaderText="Threshold" />--%>
                            <asp:TemplateField HeaderText="Threshold">
                                <ItemTemplate>
                                    <asp:TextBox onkeypress="Javascript:Numero();" ID="txtThreshold" runat="server"
                                        Width="160px" Text='<%#DataBinder.Eval(Container.DataItem, "Threshold")%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!--</div>-->
            </div>

            <div class="row">
                <!--<div class="col-md-12">-->
                <div class="grilla-small">
                    <asp:GridView ID="dgmtafondo" runat="server" SkinID="GridSmall" AutoGenerateColumns="False"
                        PageSize="5">
                        <Columns>
                            <asp:BoundField DataField="codigoportafolio" HeaderText="Codigo Portafolio" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                            <%--<asp:BoundField DataField="MTA" HeaderText="MTA" />--%>
                            <asp:TemplateField HeaderText="MTA">
                                <ItemTemplate>
                                    <asp:TextBox onkeypress="Javascript:Numero();" ID="txtMTA" runat="server"
                                        Width="160px" Text='<%#DataBinder.Eval(Container.DataItem, "MTA")%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!--</div>-->
            </div>







            <%--<div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Threshold Fondo 1
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbThreshold1" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            MTA Fondo 1
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbMTA1" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Threshold Fondo 2
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbThreshold2" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            MTA Fondo 2
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbMTA2" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Threshold Fondo 3
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbThreshold3" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            MTA Fondo 3
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbMTA3" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Threshold Contraparte
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbThresholdC" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            MTA Fondo 3
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbMTAC" />
                        </div>
                    </div>
                </div>
            </div>--%>





            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" 
                    CausesValidation="False" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdCodigo" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
