﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmManMenu.aspx.vb" Inherits="Modulos_Menu_frmManMenu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro Pagos</title>
    <link rel="stylesheet" href="../../App_Themes/css/menu/zTreeStyle.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/js/jquery.js"></script>
    <script type="text/javascript" src="../../App_Themes/js/menu/jquery.ztree.core-3.5.js"></script>
     <script type="text/javascript">
         var zNodes = null;
         var siguienteIndiceNodo = "1";

         $(document).ready(function () {
             LoadMenu();
             OcultarPanelNuevo();
         });

         function LoadMenu() {
             $.ajax({
                 type: "POST",
                 url: "frmManMenu.aspx/ListarMenu",
                 data: "{}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (response) {
                     zNodes = response.d;
                     $.fn.zTree.init($("#treeSIT"), setting, zNodes);
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("LoadMenuRol: " + XMLHttpRequest.toString() 
                                + "\n\nStatus: " + textStatus 
                                + "\n\nError: " + errorThrown);
                        }  
             });
         }

         var setting = {
             view: {
                 dblClickExpand: false,
                 showLine: false
             },
             data: {
                 simpleData: {
                     enable: true
                 }
             },
             callback: {
                 beforeClick: function (treeId, treeNode) {

                    $("#txt_CodOpcionMenu").val(treeNode.id);
                    $("#txt_CodAplicativo").val(treeNode._CodAplicativo); // ---
                    $("#txt_TituloOpcionMenu").val(treeNode.name);
                    $("#txt_Nivel").val(treeNode._Nivel); // ---
                    $("#txt_Orden").val(treeNode._Orden); // ---
                    $("#txt_Url").val(treeNode.file);
                    $("#txt_CodOpcionMenuPadre").val(treeNode.pId);

                    //$("#btn_Nuevo").attr("disable", "");
                    $("#btn_Guardar").removeAttr("disabled");
                    $("#btn_Nuevo").removeAttr("disabled");

                    OcultarPanelNuevo();

                     var zTree = $.fn.zTree.getZTreeObj("treeSIT");
                     if (treeNode.isParent) {
                         zTree.expandNode(treeNode);

                         return false;
                     } else {
                         //-- Link(treeNode.name, treeNode.id);
                         
                         /*demoIframe.attr("src", treeNode.file + ".html");*/
                         return true;
                     }
                 }
             }
         };

         function Link(strPagina, id) {
             document.getElementById('hMenu').value = id + "-" + strPagina;
         }

         function onClick(e, treeId, treeNode) {
             var zTree = $.fn.zTree.getZTreeObj("treeSIT");
             zTree.expandNode(treeNode);

         }

         function GuardarOpcion() {
             var datos = "{";
             datos += 'CodOpcionMenu: "' + $("#txt_CodOpcionMenu").val() + '",';
             datos += 'CodAplicativo: "' + $("#txt_CodAplicativo").val() + '",';
             datos += 'TituloOpcionMenu: "' + $("#txt_TituloOpcionMenu").val() + '",';
             datos += 'Nivel: "' + $("#txt_Nivel").val() + '",';
             datos += 'Orden: "' + $("#txt_Orden").val() + '",';
             datos += 'Url: "' + $("#txt_Url").val() + '",';
             datos += 'CodOpcionMenuPadre: "' + $("#txt_CodOpcionMenuPadre").val() + '"}';

             InsertarOpcionMenu("GuardarOpcion", datos);
             return false;
         }

         function InsertarSubOpcion() {
             var nivel = parseInt($("#txt_Nivel").val()) + 1;

             var datos = "{";
             datos += 'CodOpcionMenu: "' + "0" + '",';
             datos += 'CodAplicativo: "' + $("#txt_CodAplicativo").val() + '",';
             datos += 'TituloOpcionMenu: "' + $("#txt_h_TituloOpcionMenu").val() + '",';
             datos += 'Nivel: "' + nivel + '",';
             datos += 'Orden: "' + $("#txt_h_Orden").val() + '",';
             datos += 'Url: "' + $("#txt_h_Url").val() + '",';
             datos += 'CodOpcionMenuPadre: "' + $("#txt_CodOpcionMenu").val() + '"}';

             InsertarOpcionMenu("InsertarSubOpcion", datos);

             siguienteIndiceNodo = parseInt($("#txt_h_Orden").val()) + 1; // Para el Orden del siguiente NODO

             return false;
         }

         function InsertarOpcionMenu(sender, datos) {                      
             datos = datos.replace(/\\/g, "\\\\");

             $.ajax({
                 type: "POST",
                 url: "frmManMenu.aspx/InsertarOpcionMenu",
                 data: datos,
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (response) {
                     var result = response.d;

                     if (result.length == 0) {
                        OcultarPanelNuevo();
                        LoadMenu(); // Volvemos a cargar el Arbol
                     }
                     else
                         alert(sender + ": " + result); // Error
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(sender + ": " + XMLHttpRequest.toString() 
                                + "\n\nStatus: " + textStatus 
                                + "\n\nError: " + errorThrown);
                        }
             });
        }

        function MostrarPanelNuevo() {
            $("#txt_h_TituloOpcionMenu").val("");
            $("#txt_h_Orden").val(siguienteIndiceNodo);
            $("#txt_h_Url").val("");

            $("#pnl_SubOp").show();
            return false;
        }

        function OcultarPanelNuevo() {
            $("#pnl_SubOp").hide();
            return false;
        }

    </script>

</head>
<body>
    <form class="form-horizontal" id="form2" runat="server" action="frmManMenu.aspx">

    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Menu
                        <asp:Label ID="lblEdicion" runat="server"></asp:Label></h2>
                </div>
            </div>
        </header>

        <div class="Contenedor">
        <fieldset>
            <legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">                 
                        Menu General       
                       <label class="col-sm-6 control-label"></label>                        
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">                  
                    Menu de Rol      
                        <label class="col-sm-6 control-label"></label>                        
                    </div>
                </div>
            </div>
            </legend>
             <div class="row">

                <div class="col-md-6">
                    <div class="form-group">
                       <div class="content_wrap">                            
                            <div class="zTreeBackground left">
                                <ul id="treeSIT" class="ztree">
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-2">

                    <div  style="border: 1px Solid Blue; background-color: White; padding: 5px; width:370px" >

                        <table>
                            <tr>
                                <td>Cod Opción</td>
                                <td><input id="txt_CodOpcionMenu" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Cod Aplicación</td>
                                <td><input id="txt_CodAplicativo" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Título Opción</td>
                                <td><input id="txt_TituloOpcionMenu" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Nivel</td>
                                <td><input id="txt_Nivel" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Orden</td>
                                <td><input id="txt_Orden" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Url</td>
                                <td><input id="txt_Url" type="text" /></td>
                            </tr>
                            <tr>
                                <td style="width:150px">Cod Opcion Padre</td>
                                <td><input id="txt_CodOpcionMenuPadre" type="text" /></td>
                            </tr>

                            <tr>
                                <td style="height:35px; padding-right: 5px;" ><input id="btn_Guardar" value="Guardar" onclick="return GuardarOpcion();" type="submit" class="btn btn-integra" style="width:auto;" disabled="disabled"/></td>
                                <td><input id="btn_Nuevo" value="Nuevo Sub Nodo" onclick="return MostrarPanelNuevo();" type="submit" class="btn btn-integra" disabled="disabled" /></td>
                            </tr>
                                          
                        </table>

                        <table id="pnl_SubOp">
                            <tr>
                                <td>Título Opción</td>
                                <td><input id="txt_h_TituloOpcionMenu" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Orden</td>
                                <td><input id="txt_h_Orden" type="text" /></td>
                            </tr>
                            <tr>
                                <td>Url</td>
                                <td><input id="txt_h_Url" type="text" /></td>
                            </tr>

                            <tr>
                                <td style="height:35px; padding-right: 5px; width:150px" ><input id="btn_GuardarSubNodo" value="Guardar Sub Nodo" onclick="return InsertarSubOpcion();" type="submit" class="btn btn-integra" style="width:auto;" /></td>
                                <td><input value="Cancelar" onclick="return OcultarPanelNuevo();" type="submit" class="btn btn-integra" /></td>
                            </tr>
                                          
                        </table>
                
                    </div>

                    <div class="form-group">
                       <div class="content_wrap">                            
                            <div class="zTreeBackground left">
                                <ul id="Ul1" class="ztree">
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>



            </div>
        </fieldset>
        </div>
        </div>
    </form>
</body>
</html>
