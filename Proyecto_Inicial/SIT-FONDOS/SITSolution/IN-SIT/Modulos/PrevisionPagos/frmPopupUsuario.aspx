<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPopupUsuario.aspx.vb" Inherits="Modulos_PrevisionPagos_frmPopupUsuario" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Usuarios</title>

    <script type="text/javascript">
        function GetRowValue(val, val1) {
            window.opener.document.getElementById("lbCodigoUsuario").value = val;
            window.opener.document.getElementById("tbUsuario").value = val1;
            window.opener.document.getElementById("_CodigoUsuario").value = val;
            window.opener.document.getElementById("_NomUsuario").value = val1;
            window.close();
        }
    </script>

</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <%--<h2>Registro Pagos</h2>--%>
                </div>
            </div>
        </header>

        
        <fieldset>
            <legend>Criterio de B&uacute;squeda de Usuarios</legend>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Codigo Usuario</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbCodUsuario_popup" runat="server" MaxLength="18"/>
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
                             Primer Nombre</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbNombre_popup" runat="server" MaxLength="15" />
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
                             Primer Apellido</label>
                        <div class="col-sm-7">
                             <asp:TextBox ID="tbApePat_popup" runat="server" MaxLength="15" />
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
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button ID="btnBuscarPersonal" runat="server" Text="Buscar" CausesValidation="false" />
                </div>
            </div>

        </fieldset>
        

        <br />

        
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador_popup" />
        </fieldset>
        

        <br />

        <div class="grilla">
          
                <asp:GridView ID="gvUsuarios_popup" runat="server" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="Seleccionar">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSeleccionar" SkinID="imgCheck" CommandName="Seleccionar" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoInterno")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoUsuario")&amp;","&amp;DataBinder.Eval(Container, "DataItem.NumeroDocumento")&amp;","&amp;DataBinder.Eval(Container, "DataItem.NombreCompleto")%>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoUsuario" HeaderText="C&#243;digo Usuario">
                                <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumeroDocumento" HeaderText="N&uacute;ero Documento" 
                                Visible="False">
                                <ItemStyle HorizontalAlign="Center" CssClass="stlPaginaTexto2" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre Completo">
                                <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" />
                  </asp:GridView>
              
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:Button Text="Cancelar" runat="server" ID="btnCancelarM" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
