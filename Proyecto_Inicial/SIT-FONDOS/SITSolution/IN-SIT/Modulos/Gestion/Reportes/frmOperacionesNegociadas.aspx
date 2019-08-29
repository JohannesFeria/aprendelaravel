<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacionesNegociadas.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmOperacionesNegociadas" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reporte Operaciones Negociadas por Trader</title>
    <script language="javascript">
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }
        $(document).ready(function () {
            $("#ibExportar").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Reporte Operaciones Negociadas por Trader</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="input-append date">
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Fin</label>
                        <div class="input-append date">
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Renta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operadores</label>
                        <div class="col-sm-8">
                            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSeleccion" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField Visible="True" DataField="CodigoUsuario" HeaderText="Código Usuario" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
                <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
        </div>
    </div>
    </form>
</body>
</html>
