<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfases.aspx.vb" Inherits="Modulos_Gestion_Archivos_Planos_frmInterfases" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Generar Archivos VAX</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                Generar para VAX
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
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="216px" CssClass="stlCajaTexto"
                                AutoPostBack="True">
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
                            <asp:DropDownList ID="ddlInterface" runat="server" Width="216px" CssClass="stlCajaTexto"
                                AutoPostBack="True">
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
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="imbSalir" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <asp:Label ID="msgError1" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError2" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError3" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError4" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError5" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="msgError6" runat="server" Visible="False"></asp:Label>
        </div>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgNoExiste">
            </asp:GridView>
        </div>
    </div>
    <%--<asp:HiddenField ID="txtValidacion" runat="server" />--%>
    </form>
</body>
</html>
