<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteValorCuotaVariacion.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmReporteValorCuotaVariacion" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de Valor Cuota Variación</title>
</head>
<body>
    <form id="form1" runat="server">
 <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Reporte de Valor Cuota Variación</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos del Reporte</legend>
            <div class="row">
          
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Estadística</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlVariacion" Width="200px" runat="server" >
                                <asp:ListItem Value="12" Text="a 12 meses"></asp:ListItem>
                                <asp:ListItem Value="24" Text="a 24 meses"></asp:ListItem>
                                <asp:ListItem Value="36" Text="a 36 meses" Selected="True"></asp:ListItem>
                             </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Alerta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlAlerta" Width="200px" runat="server" >
                                    <asp:ListItem Value="1" Text="Z-score > 1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Z-score > 2" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Z-score > 3" ></asp:ListItem>
                             </asp:DropDownList>
                        </div>
                    </div>
                </div>


                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaIni" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
               
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <br />
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir Calculo" runat="server" ID="btnImprimirFormula" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>