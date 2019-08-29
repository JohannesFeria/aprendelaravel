<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGenerarMemo.aspx.vb" Inherits="Modulos_PrevisionPagos_frmGenerarMemo" %>

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
                    <h2>
                        <asp:Label ID="lbTitulo" runat="server" Text="Generar Memo" />
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </h2>
                </div>
            </div>
        </header>

        
        <fieldset>
            <legend>Datos&#32;Generales</legend>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                         Fecha Registro</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>    
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
                        Tipo Reporte</label>
                        <div class="col-md-7">
                            <asp:DropDownList ID="ddlTipoReporte" runat="server" AutoPostBack="True"></asp:DropDownList>
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
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server"></asp:DropDownList>
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
        
        <br />
        <br />

        <%--<div class="grilla">
        </div>--%>
        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <%--<asp:UpdatePanel runat="server" ID="updExtraer">
                    <ContentTemplate>--%>
                        &nbsp;
                        <asp:Button ID="btGenerar" runat="server" Width="72px" Text="Generar" CausesValidation="False"
                            CssClass="button"></asp:Button>&nbsp;<asp:Button ID="btnParametria" runat="server"
                                CausesValidation="False" Text="Parametria" Width="100px" CssClass="button" />
                        &nbsp;<asp:Button ID="bSalir" runat="server" Width="72px" Text="Retornar" CausesValidation="False"
                            CssClass="button" Visible="false"></asp:Button>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>

    </div>
    </form>
</body>
</html>

