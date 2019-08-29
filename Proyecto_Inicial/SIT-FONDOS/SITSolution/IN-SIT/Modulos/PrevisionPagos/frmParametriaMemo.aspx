<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmParametriaMemo.aspx.vb" Inherits="Modulos_PrevisionPagos_frmParametriaMemo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Feriados</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManagerLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Parametría Memo</h2>
                </div>
            </div>
        </header>
                
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-gruop">
                        <label class="col-sm-5 control-label">
                        Tipo Reporte</label>
                        <div class="col-md-7">
                            <asp:DropDownList ID="ddlTipoReporte" runat="server" AutoPostBack="true"></asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="Tipo Reporte" ControlToValidate="ddlTipoReporte"
                            Text="(*)" runat="server" ID="rfTipoReporte" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Tipo Operación</label>
                        <div class="col-md-7">
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server" AutoPostBack = "true" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        A</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbDescripcionA" runat="server" MaxLength="50">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n A" ControlToValidate="tbDescripcionA"
                             Text="(*)" runat="server" ID="rfDescripcionA" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        De</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbDescripcionDe" runat="server" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n De" ControlToValidate="tbDescripcionDe"
                                Text="(*)" runat="server" ID="rfDescripcionDe" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Referencia</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbReferencia" runat="server" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Referencia" ControlToValidate="tbReferencia"
                             Text="(*)" runat="server" ID="rfReferencia" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Contenido</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbContenido" runat="server" MaxLength="1000" Width="300px" Height="115px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Contenido" ControlToValidate="tbContenido"
                                Text="(*)" runat="server" ID="rfContenido" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Despedida</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbDespedida" runat="server" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Despedida" ControlToValidate="tbDespedida"
                                Text="(*)" runat="server" ID="rfDespedida" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Usuario Firma</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbUsuarioFirma" runat="server" MaxLength="80"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Usuario Firma" ControlToValidate="tbUsuarioFirma"
                                Text="(*)" runat="server" ID="rfUsuarioFirma" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Area Usuario Firma</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbAreaUsuarioFirma" runat="server" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Area Usuario Firma" ControlToValidate="tbAreaUsuarioFirma"
                                Text="(*)" runat="server" ID="rfAreaUsuarioFirma" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Iniciales Documentador</label>
                        <div class="col-md-7">
                            <asp:TextBox ID="tbInicialesDocumentador" runat="server" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

        </fieldset>
        </div>

        <br />
        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:UpdatePanel runat="server" ID="updExtraer">
                    <ContentTemplate>
                        &nbsp;
                        <asp:Button ID="btnAceptar" runat="server" Width="72px" Text="Aceptar">
                        </asp:Button>&nbsp;
                        <asp:Button ID="bSalir" runat="server" Width="72px" Text="Retornar" CausesValidation="False">
                        </asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="col-md-6">
            <asp:ValidationSummary runat="server" ID="vsFormulario" ShowMessageBox="true" HeaderText="Los siguientes campos son obligatorios:"
            ShowSummary="false" />
        </div>
    </div>
    </form>
</body>
</html>
