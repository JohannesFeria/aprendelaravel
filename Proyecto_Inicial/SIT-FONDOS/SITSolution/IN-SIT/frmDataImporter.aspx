<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDataImporter.aspx.cs" Inherits="frmDataImporter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">

    <title></title>

  <style type="text/css"> 
        .win01 {
            border: solid 1px gray;
            background-color: #80808052;
            position: absolute;
            z-index: 10;
            width: 100%;
            height: 100%;
            text-align: center;     
        }        
        .cont01 {
            border: solid 1px gray;
            background-color: white;
            display: inline-block;
            
            margin-top: 50px;
            padding: 10px 20px; 
            border-radius: 5px;
            
            width: 700px;      
        }

        .cont01 input[type=text]
        {
            background-color: White;
            cursor: default;
        }

        .cont01 div[class=row]
        {
            margin-top: 5px;
        }
        
        .tab01 {
            width: 100%;
        }
        .tab01>tbody>tr>td {
            padding: 2px 5px;
            border: Solid 1px gray;
        }        
        .span01 {
            font-size: 17px;
        }

        .tooltip>div{
            /*background-color: gray;*/
        }

    </style>

    <script type="text/javascript">

        $(document).ready(function () {        

//            $("#btnLeerExcel").click(function () {
//                LeerExcel(true);
//                return false;
//            });
        });

        /*CRumiche: idFondoOpe=ID Fondo en BD Operaciones*/
        function LeerExcel(soloLectura) {
//            var _idFondoOpe = 2;
//            var _codUsuario = "P500625"; // Probando con el usuario LISETH

            //            var param = { idFondoOpe: _idFondoOpe, codUsuario: _codUsuario };

            var param = { p: "" };
            var _url = "frmDataImporter.aspx/Get_CargarExcelMaestro";

            $.ajax({
                url: _url,
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (response) {

                    alert("Archivo Cargado");
                    // Para manejar los datos obtenidos correctamente
                    if (typeof response.d == "object") {
                        
                    }
                    else {

                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
            });
        }

        $('form').live("submit", function () {
            //$('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            ShowProgress();
        });

    </script>


</head>
<body style="background-image: url('fondo1.png')" >

    <form id="form1" runat="server" >
<%--    <div>    
        <asp:Button ID="btnLeerExcel" runat="server" Text="Leer Excel" />
    </div>
--%>

    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"><h2>Importar Portafolio</h2></div></div></header>
        <fieldset>
            <legend>Cargar el Archivo</legend>

            <div class="row">
                <div class="col-md-9">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Archivo (Basado en la Plantilla de Datos)</label>
                        <div class="col-sm-8">                       
                            <asp:FileUpload class="filestyle" id="fupArchivo" runat="server" accept=".xls,.xlsx"
                                            data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <asp:Button Text="Procesar Archivo" runat="server" ID="brnProcesar" 
                            OnClientClick="return ValidaBusqueda()" onclick="brnProcesar_Click" />
                    </div>                 
                </div>
            </div
            
            <div class="row">
                <div class="col-md-11">
                    <div class="form-group">                            
                        <label class="col-sm-12 control-label" >Resultados del procesamiento</label>
                        <div class="col-sm-12">                                
                            <textarea id="pop1_areaErrores" runat="server" style="width:90%; height: 70px;" readonly="readonly"  ></textarea>
                        </div>
                    </div>
                </div>
            </div>        
                                 
        </fieldset>
        <div class="loading" align="center">
            <img src="App_Themes/img/icons/loading.gif" />
        </div>
    </div>

    </form>
</body>
</html>
