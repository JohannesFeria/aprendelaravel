<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAgruparAcciones.aspx.vb"
    Inherits="Modulos_Inversiones_frmAgruparAcciones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts")%>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Agrupar Acciones</title>
    <script type="text/javascript">
        function SelectAll(CheckBoxControl, dgControl) {
            var i;
            if (CheckBoxControl.checked == true) {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf(dgControl) > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
                document.getElementById('hdnSelect').value = "1";
            }
            else {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf(dgControl) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
                document.getElementById('hdnSelect').value = "2";
            }
            document.getElementById('hdnGridViewSelect').value = dgControl;
            document.getElementById("<%= btnSeleccionar.ClientID %>").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid" id="divContainer">
                <header><h2>Agrupar Acciones</h2></header>
                <fieldset>
                    <legend>Filtro de búsqueda</legend>
                    <div class="row">
                        <div class="form-group col-md-3">
                            <label class="col-md-4 control-label">
                                Portafolio</label>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlFondoOE" class=" form-control" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                        <div class="form-group col-md-3">
                            <label class="col-md-6 control-label">
                                Fecha de Operación</label>
                            <div class="col-md-6 input-group date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" class="form-control" Height="30px"  />
                                <div class="input-group-addon">
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                            <%--<div class="col-md-8 input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" class=" form-control input-lg" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>--%>
                        </div>
                        <div class="form-group col-md-3">
                            <label class="col-md-4 control-label">
                                Mercado</label>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlMercado" class="form-control" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                        <div class="form-group col-md-3 text-right">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border">&Oacute;rdenes Confirmadas&nbsp; - Acciones<asp:Label
                        ID="lblCantidadOIC" runat="server">(0)</asp:Label>
                    </legend>
                    <div class="grilla" style="overflow: scroll;" id="divGrillaAccionesConfirmadas">
                        <asp:GridView ID="dgListaOC" runat="server" SkinID="Grid_PageSize_15" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <input onclick="SelectAll(this,'dgListaOC')" type="checkbox" name="SelectAllCheckBoxOC"
                                            id="SelectAllCheckBoxOC" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectOC" runat="server" OnCheckedChanged="dgListaOC_CheckedChanged"
                                            AutoPostBack="True"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operación" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liquidación" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="Portafolio" HeaderText="Portafolio" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoInstrumento" HeaderText="Tipo Instrumento" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoOrden" HeaderText="N° Orden" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="Operacion" HeaderText="Tipo Operación" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:#,##0.00}"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoAgrupado" HeaderText="Cod. Agrupación" ItemStyle-HorizontalAlign="Center">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoMercado" HeaderText="Cod. Mercado" ItemStyle-HorizontalAlign="Center" /><%--HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"--%>
                                
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hdnSelect" runat="server" />
                    </div>
                    <div class="form-group col-md-12">
                        <div class=" text-left col-md-6" runat="server">
                            <asp:Button ID="btnRegresar" class="form-control" runat="server" Text="Regresar" />
                        </div>
                        <div class=" text-right col-md-6" id="divAcciones" runat="server">
                            <asp:Button ID="btnAgrupar" class="form-control" runat="server" Text="Agrupar" />
                            <asp:Button ID="btnDesagrupar" class="form-control" runat="server" Text="Desagrupar" />
                        </div>
                        <div style="visibility: hidden">
                            <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" />
                        </div>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
