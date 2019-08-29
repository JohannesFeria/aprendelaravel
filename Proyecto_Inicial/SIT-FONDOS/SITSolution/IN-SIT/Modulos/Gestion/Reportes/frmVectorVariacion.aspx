<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVectorVariacion.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmVectorVariacion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2 runat="server" id="lblTitulo">
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">

                <div id="divFechaDsc1" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="lblFechaDsc1" runat="server" class="col-sm-3 control-label" />
                        <div class="col-sm-7">
                            <div id="divFechaValoracion1" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaValoracion" SkinID="Date" Width="150px" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>                       
                </div>


                <div id="divFechaDsc2" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="lblFechaDsc2" runat="server" class="col-sm-5 control-label">
                        </label>
                        <div class="col-sm-7">
                            <div id="divFechaValoracion2" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaValoracion2" SkinID="Date" Width="150px" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                           
                        </div>
                    </div>
                </div>
              
            </div>
       
      
        </fieldset>
        <br />

        <header>
        </header>

        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>

    <input id="hdborrar" type="hidden" />


    </form>


</body>
</html>
