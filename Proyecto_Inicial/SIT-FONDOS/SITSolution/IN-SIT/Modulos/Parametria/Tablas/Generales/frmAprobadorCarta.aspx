<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAprobadorCarta.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmAprobadorCarta" %>

<!DOCTYPE html >
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Aprobador Carta</title>
    <style>
        .thumb
        {
            height:120px;
            width:150px;
            border:0px;
            margin:10px 5px 0 0;
        }
        
    </style>
    <script type="text/javascript">
        function showPopupUsuarios() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Personal', '1200', '600', '');
        }

        $(document).ready(function () {
        });

        function showFirma() {
            $("#divFirma").html("");
            var img = $("<img>").attr({ alt: "Firma Usuario", height: '120px', width: '150px', style: 'border: solid 1px #9ecff9', src: $("#hdPathFirma").val() });
            $("#divFirma").append(img);
        }

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
                        Aprobador Carta</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            C&oacute;digo Usuario</label>
                        <div class="col-sm-10">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoUsuario" Width="120px" />
                                <asp:LinkButton ID="lkbBuscarUsuario" runat="server" CausesValidation="false" Enabled="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbNombreUsuario" Width="280px" Enabled="false" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Usuario" ControlToValidate="tbCodigoUsuario"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Rol</label>
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlRol" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <%--INICIO OT10795--%>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Tipo Firma
                        </label>
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlTipoFirma" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <%--FIN OT10795--%>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Firma
                        </label>
                        <div class="col-sm-10">
                            <%--<input id="iptRuta" runat="server" name="iptRuta" type="file" accept="image/*" class="filestyle" data-buttonname="btn-primary"
                                data-buttontext="Seleccionar" data-size="sm" size="80">--%>
                            <input type="file" id="files" name="files[]" runat="server" title="" style="width:auto;" />
                            <output id="list"></output>
                            <script>
                                function archivo(evt) {
                                    var files = evt.target.files; // FileList object
                                    // Obtenemos la imagen del campo "file".
                                    for (var i = 0, f; f = files[i]; i++) {
                                        //Solo admitimos imágenes.
                                        if (!f.type.match('image.*')) {
                                            continue;
                                        }
                                        var reader = new FileReader();
                                        reader.onload = (function (theFile) {
                                            return function (e) {
                                                // Insertamos la imagen
                                                $("#divFirma").html("");
                                                document.getElementById("list").innerHTML = ['<img class="thumb" src="', e.target.result, '" title="', escape(theFile.name), '"/>'].join('');
                                            };
                                        })(f);
                                        reader.readAsDataURL(f);
                                    }
                                }
                                document.getElementById('files').addEventListener('change', archivo, false);
                            </script>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label"></label>
                        <div id="divFirma" class="col-sm-10">
                        <%--<img alt="Firma Usuario" height="120px" width="150px" src="">--%>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
        </div>
    </div>
    <%--<input type="hidden" runat="server" id="hdFirma">--%>
    <input type="hidden" runat="server" id="hdPathFirma">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
