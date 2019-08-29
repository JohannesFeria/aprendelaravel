<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title></title>    
    <script type="text/javascript" >
        $(document).ready(function () {
            $("#btnBuscar").click(function () {
                var request = $.ajax({ type: "POST",
                    url: "http://sdpeapp00024:9080/fondosivr2/mostrarTelefono",
                    context: document.body,
                    data: '{"numeroDocumento":"' + $("#txtNumeroDocumento").val() + '"}',
                    contentType: "application/json",
                    dataType: "json",
                    success: function (w_response) {
                        var jsonData = JSON.stringify(w_response);
                        $("#txtCUC").val("");
                        $("#txtTelCasa").val("");
                        $("#txtTelOficina").val("");
                        $("#txtTelCelular").val("");
                        $.each($.parseJSON(jsonData), function (key, value) {
                            $("#txtCUC").val(value.cuc);
                            $("#txtTelCasa").val(value.telefonoCasa);
                            $("#txtTelOficina").val(value.telefonoOficina);
                            $("#txtTelCelular").val(value.telefonoCelular);
                        });
                    },
                    error: function () { alert('Error') }
                });
            })
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>REST</h2></header>
        <fieldset>
            <legend>Resultado</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Numero de Documento</label>
                        <div class="col-sm-9">
                            <input id="txtNumeroDocumento" type="text" />
                        </div>
                    </div>
                </div>
                <input id="btnBuscar" type="button" value="Buscar" />
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultado</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">CUC</label>
                        <div class="col-sm-9">
                            <input id="txtCUC" type="text" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Telefono Casa</label>
                        <div class="col-sm-9">
                            <input id="txtTelCasa" type="text" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Telefono Oficina</label>
                        <div class="col-sm-9">
                            <input id="txtTelOficina" type="text" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Telefono Celular</label>
                        <div class="col-sm-9">
                            <input id="txtTelCelular" type="text" />
                        </div>
                    </div>
                </div>
            </div>

        </fieldset>
    </div>
    </form>
</body>
</html>