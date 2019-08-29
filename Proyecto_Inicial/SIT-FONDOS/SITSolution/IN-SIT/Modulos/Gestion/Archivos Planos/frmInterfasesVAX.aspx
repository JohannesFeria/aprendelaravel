<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfasesVAX.aspx.vb"
    Inherits="Modulos_Gestion_Archivos_Planos_frmInterfasesVAX" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>InterfasesVAX</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                Importar VAX
            </h2>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                        <div class="col-sm-9">
                            <%--<asp:DropDownList ID="ddlPortafolio" AutoPostBack="True" runat="server"
                                Width="136px">
                                <asp:ListItem Value="HO-FONDO1">HO-FONDO1</asp:ListItem>
                                <asp:ListItem Value="HO-FONDO2">HO-FONDO2</asp:ListItem>
                                <asp:ListItem Value="HO-FONDO3">HO-FONDO3</asp:ListItem>
                            </asp:DropDownList>--%>
                            <asp:DropDownList ID="ddlPortafolio" AutoPostBack="True" runat="server" Width="136px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha Interfase</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVAX" SkinID="Date" />
                                <span class="add-on" id="imgFechaVAX"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Interfase</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlInterface" AutoPostBack="True" runat="server"
                                Width="136px">
                                <asp:ListItem Value="--Seleccione--">--Seleccione--</asp:ListItem>
                                <asp:ListItem Value="BCOS">BCOS</asp:ListItem>
                                <asp:ListItem Value="BMIDAS">BMIDAS</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Ruta</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="Myfile" runat="server" Width="577px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="btn_Salir" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <asp:Label ID="msgError" runat="server" CssClass="stlPaginaTexto" Visible="False"></asp:Label>
        </div>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgNoExiste">
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="txtValidacion" runat="server" />
    </form>
</body>
</html>
