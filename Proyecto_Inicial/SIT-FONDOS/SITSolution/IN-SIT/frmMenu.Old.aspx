<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMenu.Old.aspx.vb" Inherits="frmMenuOld" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>

    <script type="text/javascript" src="App_Themes/js/jquery.js"></script>

    <script>

        $(document).ready(function () {
            InfoOp_btnClose_onclick();
        });

        // ----- Temporal para obtener rapidamente los datos(Nombre y Link) de cada opcion
        function InfoOp_btnClose_onclick() {
            $("#infoOpcion").hide();
        }
        function InfoOp_Show(txt) {
            txt = txt.replace(/\?Variables=Rol1=ADMINUSU,Parametro1=\//g, "");
            txt = $.trim(txt);

            $("#InfoOp_txtInfo").val(txt);
            $("#InfoOp_txtInfo").select();
            //$("#InfoOp_txtInfo").focus(function () { $(this).select(); });

//            Copied = $("#InfoOp_txtInfo").createTextRange();
//            Copied.execCommand("Copy");

            $("#infoOpcion").show();
        }
        function GetInfo(ctrl) {
            // Temporal para obtener rapidamente los datos(Nombre y Link) de cada opcion
            InfoOp_Show($(ctrl).html());
        }
        // ----- 

        EstadoTamano = 0
        function Link(ctrl, strPagina) {
            //// Temporal para obtener rapidamente los datos(Nombre y Link) de cada opcion
            InfoOp_Show($(ctrl).html() + " " + strPagina);

            parent.document.getElementById('basefrm').src = strPagina;
            return false;
        }
        function ModificarTamano() {
            if (EstadoTamano == 0) {
                parent.CambiarTamanoFrame(200);
                EstadoTamano = 1
                MostrarDiv("taMenu")
                MostrarDiv("taFunciones")
                document.getElementById("imaMenu").src = "App_Themes/js/menu/ARW02LT.ico"
                document.getElementById("imaMenu").height = "16"
                document.getElementById("imaMenu").width = "16"
            } else {
                parent.CambiarTamanoFrame(20);
                EstadoTamano = 0
                OcultarDiv("taMenu")
                OcultarDiv("taFunciones")
                document.getElementById("imaMenu").src = "App_Themes/js/menu/ARW02RT.ico"
                document.getElementById("imaMenu").height = "16"
                document.getElementById("imaMenu").width = "16"
            }
        }

        function collapseTree() {
            // Close all folder nodes
            clickOnNodeObj(foldersTree)
            // Restore the tree to the top level
            clickOnNodeObj(foldersTree)
        }

        function openFolderInTree(linkID) {
            var folderObj;
            folderObj = findObj(linkID);
            folderObj.forceOpeningOfAncestorFolders();
            if (!folderObj.isOpen)
                clickOnNodeObj(folderObj);
        }

        function expandTree(folderObj) {
            var childObj;
            var i;
            // Open the folder
            if (!folderObj.isOpen)
                clickOnNodeObj(folderObj)

            // Call this function for all children
            for (i = 0; i < folderObj.nChildren; i++) {
                childObj = folderObj.children[i]
                if (typeof childObj.setState != "undefined") { // If this is a folder
                    expandTree(childObj)
                }
            }
        }
	
    </script>
    <!--------------------------------------------------------------->
    <!-- Copyright (c) 2006 by Conor O'Mahony.                     -->
    <!-- For enquiries, please email GubuSoft@GubuSoft.com.        -->
    <!-- Please keep all copyright notices below.                  -->
    <!-- Original author of TreeView script is Marcelino Martins.  -->
    <!--------------------------------------------------------------->
    <!-- This document includes the TreeView script.  The TreeView -->
    <!-- script can be found at http://www.TreeView.net.  The      -->
    <!-- script is Copyright (c) 2006 by Conor O'Mahony.           -->
    <!--------------------------------------------------------------->
    <!-- Instructions:                                             -->
    <!--   - Through the <STYLE> tag you can change the colors and -->
    <!--     types of fonts to the particular needs of your site.  -->
    <!--   - A predefined block with black background has been     -->
    <!--     made for stylish people :-)                           -->
    <!--------------------------------------------------------------->
    <!-- This is the <STYLE> block for the default styles.  If   -->
    <!-- you want the black background, remove this <STYLE>      -->
    <!-- block.                                                  -->
    <style type="text/css">
        BODY
        {
            background-color: #f5f5f5;
        }
        TD
        {
            font-size: 8pt;
            font-family: verdana,helvetica;
            white-space: nowrap;
            text-decoration: none;
        }
        A
        {
            color: black;
            text-decoration: none;
        }
        .specialClass
        {
            font-weight: bold;
            font-size: 12pt;
            color: #609AD8;
            font-family: garamond;
            text-decoration: underline;
        }
        
        .temp-div
        {
            border:1px Solid Green;
            position:absolute;
            z-index:10;
            left:0px; bottom:0px;
            }
        
    </style>
    <!-- If you want the black background, replace the contents  -->
    <!-- of the <STYLE> tag above with the following...
      
			<!-- This is the end of the <STYLE> contents.                -->
    <!-- Code for browser detection. DO NOT REMOVE.              -->
    <script src="App_Themes/js/menu/ua.js" type="text/javascript"></script>
    <!-- Infrastructure code for the TreeView. DO NOT REMOVE.    -->
    <script src="App_Themes/js/menu/ftiens4.js" type="text/javascript"></script>
    <!-- Scripts that define the tree. DO NOT REMOVE.            -->
    <script>
        //
        // Copyright (c) 2006 by Conor O'Mahony.
        // For enquiries, please email GubuSoft@GubuSoft.com.
        // Please keep all copyright notices below.
        // Original author of TreeView script is Marcelino Martins.
        //
        // This document includes the TreeView script.
        // The TreeView script can be found at http://www.TreeView.net.
        // The script is Copyright (c) 2006 by Conor O'Mahony.
        //

        // Decide if the names are links or just the icons
        USETEXTLINKS = 1  //replace 0 with 1 for hyperlinks

        // Decide if the tree is to start all open or just showing the root folders
        STARTALLOPEN = 0 //replace 0 with 1 to show the whole tree

        HIGHLIGHT = 1
        ICONPATH = "App_Themes/js/menu/"

        foldersTree = gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Sistema Integral de Tesorería</span>")
        foldersTree.treeID = "Funcs"
        aux0 = foldersTree

        aux3106 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Inversiones</span>", "javascript:parent.op()"))

        aux3107 = insFld(aux3106, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Ordenes de inversión</span>", "javascript:parent.op()"))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmBonos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Bonos'> Bonos</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmCertificadosSuscripcion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Certificado de Suscripción'> Certificado de Suscripción</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmInstrumentosEstructurados.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Instrumentos Estructurados'> Instrumentos Estructurados</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmDepositoPlazos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Depósito a plazo'> Depósito a plazo</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmCertificadoDeposito.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Certificado de Depósito'> Certificado de Depósito</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmOperacionesReporte.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operaciones de Reporte'> Operaciones de Reporte</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmPagares.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Pagaré'> Pagaré</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmOpcionesDerivadasForwardDivisas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Forward de Divisas'> Forward de Divisas</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmSwapDivisas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='SWAP Divisas'> SWAP Divisas</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmAcciones.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Acciones'> Acciones</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmPapelesComerciales.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Papeles Comerciales'> Papeles Comerciales</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmCompraVentaMonedaExtranjera.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Compra/Venta de Moneda Extranjera'> Compra/Venta de Moneda Extranjera</div>", ""))

        //NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmLetrasHipotecarias.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Letras Hipotecarias'> Letras Hipotecarias</div>", ""))

        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmOrdenesFondo.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Órdenes de Fondos'> Órdenes de Fondos</div>", ""))

        //JHC REQ 66056: Implementacion Futuros
        //Inicio
        NodoAux = insDoc(aux3107, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/InstrumentosNegociados/frmOperacionesFuturas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operación Futuros'> Operación Futuros</div>", ""))
        //Fin

        aux3129 = insFld(aux3106, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Reportes de Inversiones</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3129, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/Reportes/Limites/frmReporteLimitesV2.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Límites'> Límites</div>", ""))

        NodoAux = insDoc(aux3129, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/Reportes/Orden de Inversion/frmReporteOI_Estados.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Órdenes de Inversión'> Órdenes de Inversión</div>", ""))

        NodoAux = insDoc(aux3129, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/Reportes/Orden de Inversion/frmReporteOrdenesdeInversion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operaciones Ejecutadas'> Operaciones Ejecutadas</div>", ""))

        NodoAux = insDoc(aux3129, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/Reportes/Consolidado de Posiciones/frmReporteConsolidadoPosicionV2.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consolidado de Posición de Fondos'> Consolidado de Posición de Fondos</div>", ""))
        //CMB OT 65473 20120622
        NodoAux = insDoc(aux3129, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmBusquedaFirmaDocumento.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Firma de Documentos'> Firma de Documentos</div>", ""))

        //CMB OT 62087 REQ 6 20110225
        aux3130 = insFld(aux3106, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Ingreso masivo de operaciones</span>", "javascript:parent.op()"))

        //CMB OT 62087 REQ 6 20110126
        NodoAux = insDoc(aux3130, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmIngresoMasivoOperacionRV.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Renta variable'> Renta variable</div>", ""))

        //CMB OT 62087 REQ 6 20110126
        NodoAux = insDoc(aux3130, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmIngresoMasivoOperacionRF.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Renta fija'> Renta fija</div>", ""))

        //CMB OT 62087 REQ 6 20110223
        NodoAux = insDoc(aux3130, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmIngresoMasivoOperacionFX.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operación Fx'> Operación Fx</div>", ""))

        //JHC REQ 66056: Implementacion Futuros
        //Inicio
        //NodoAux = insDoc(aux3130, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmIngresoMasivoOperacionFT.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operación Futuros Masivo'> Operación Futuros Masivo</div>", ""))
        //Fin

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmIngresoOrdenesDatatec.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ingreso de órdenes DATATEC'> Ingreso de órdenes DATATEC</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/ConsultasPreOrden/frmConsultasPreOrden.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Pre Órdenes y Órdenes de Inversión'> Consulta de Pre Órdenes y Órdenes de Inversión</div>", ""))

//        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/ConsultasPreorden/frmConsultaHistoricaPreOrden.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta Histórica de Pre Ordenes de Inversión'> Consulta Histórica de Pre Ordenes de Inversión</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/ExcesosLimite/frmExcesosxLimites.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Aprobar Excesos por límite'> Aprobar Excesos por límite</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/ExcesosLimite/frmExcesosxBroker.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Aprobar Excesos por broker'> Aprobar Excesos por broker</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmEjecutarOrdenesDeInversion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ejecutar Ordenes de Inversión'> Ejecutar Ordenes de Inversión</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmConfirmarOrdenesDeInversion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Confirmación de órdenes'> Confirmación de órdenes</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmCorreccionDepositoPlazo.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Corrección de Depósitos a Plazo'> Corrección de Depósitos a Plazo</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmExtornoOrdenesEjecutadas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Extorno de Ordenes Ejecutadas'> Extorno de Ordenes Ejecutadas</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/AsignacionFondos/frmExtornoAsignacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Extorno de Ordenes Asignadas'> Extorno de Ordenes Asignadas</div>", ""))

        NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmNegociacionDiasAnteriores.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Negociación Dias anteriores'> Negociación Dias anteriores</div>", ""))

        //NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmTraspasoInstrumentos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Traspaso de Instrumentos'> Traspaso de Instrumentos</div>", ""))

        //HDG OT 63063 R09 20110725
        //NodoAux=insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmVencimientoForwardNoDelivery.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Preliquidacion Forward No Delivery'> Preliquidacion Forward No Delivery</div>", ""))

        //HDG OT 63063 R09 20110725
        //NodoAux=insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmInventarioForward.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Inventario Forward'> Inventario Forward</div>", ""))

        //NodoAux = insDoc(aux3106, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmModificacionFechaIDI.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Modificacion Fecha IDI'> Modificacion Fecha IDI</div>", ""))


        aux3202 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Riesgos</span>", "javascript:parent.op()"))

        NodoAux = insDoc(aux3202, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Riesgos/frmAprobarNuevoInstrumento.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Aprobación de alta de instrumentos'> Aprobación de alta de instrumentos</div>", ""))

        //CMB OT 62087 20110225 Nro 4
        NodoAux = insDoc(aux3202, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaLimiteTrading.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Limites Trading'> Limites Trading</div>", ""))

        //HDG OT 63063 R04 20110523
        NodoAux = insDoc(aux3202, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Riesgos/frmExcepcionLimiteNegociacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Excepciones límites de negociación'> Excepciones límites de negociación</div>", ""))


        aux3121 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Tesorería</span>", "javascript:parent.op()"))

        aux3233 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Saldos de Bancos</span>", "javascript:parent.op()"))
        //NodoAux = insDoc(aux3233, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmCapturaSaldosBancarios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Captura de Saldos bancarios'> Captura de Saldos bancarios</div>", ""))

        NodoAux = insDoc(aux3233, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmRegistroSaldosBancarios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Registro de Saldos bancarios'> Registro de Saldos bancarios</div>", ""))

        NodoAux = insDoc(aux3233, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmConsultaSaldosBancarios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Saldos bancarios'> Consulta de Saldos bancarios</div>", ""))

        aux3138 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Operaciones de Caja</span>", "javascript:parent.op()"))

        //ini HDG OT 63063 R09 20110725
        aux3207 = insFld(aux3138, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Forward</span>", "javascript:parent.op()"))

        NodoAux = insDoc(aux3207, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmVencimientoForwardNoDelivery.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Pre-liquidación FW Nom-Delivery'> Pre-liquidación FW Nom-Delivery</div>", ""))

        NodoAux = insDoc(aux3207, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/frmInventarioForward.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Inventario Forward'> Inventario Forward</div>", ""))

        NodoAux = insDoc(aux3207, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Reportes/frmGenerarReporteConstForward.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Constitución de Forwards'> Constitución de Forwards</div>", ""))
        //HDG OT AnexoSBS 20130610
        NodoAux = insDoc(aux3207, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Reportes/frmAnexoSwapRenovacionFWD.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Anexos SWAP y Renovaciones FWD'> Anexos SWAP y Renovaciones FWD</div>", ""))
        //fin HDG OT 63063 R09 20110725

        //ini JHC OT 66056 20110725
        aux3307 = insFld(aux3138, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Futuros</span>", "javascript:parent.op()"))

        NodoAux = insDoc(aux3307, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Reportes/frmGenerarReporteFuturo.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Constitución de Futuros'> Constitución de Futuros</div>", ""))
        //fin JHC OT 66056 20110725

        NodoAux = insDoc(aux3138, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmConsultaMovimientosBancarios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Movimientos bancarios'> Consulta de Movimientos bancarios</div>", ""))

        NodoAux = insDoc(aux3138, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmConsultaVencimientos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Vencimientos'> Consulta de Vencimientos</div>", ""))

        NodoAux = insDoc(aux3138, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmOperacionCaja.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ingresos y Egresos varios'> Ingresos y Egresos varios</div>", ""))

        NodoAux = insDoc(aux3138, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmTransferencias.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Transferencias Internas'> Transferencias Internas</div>", ""))

        NodoAux = insDoc(aux3138, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmPreLiquidacionDivisas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='PreLiquidacion de divisas NoPH'> PreLiquidacion de divisas NoPH</div>", ""))

        //CMB OT 63063 20110509 REQ 15
        NodoAux = insDoc(aux3138, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmExtornoOperacionesCaja.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Extorno de Operaciones Caja'> Extorno de Operaciones Caja</div>", ""))

        aux3139 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Cuentas por Cobrar</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3139, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Cuentasxcobrar/frmIngresos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ingresos Cuentas por Cobrar'> Ingresos Cuentas por Cobrar</div>", ""))

        NodoAux = insDoc(aux3139, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Cuentasxcobrar/frmExtornoCxC.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Anulacion Cuentas por Cobrar'> Anulacion Cuentas por Cobrar</div>", ""))

        NodoAux = insDoc(aux3139, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Cuentasxcobrar/frmLiquidaciones.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Liquidaciones Cuentas por Cobrar'> Liquidaciones Cuentas por Cobrar</div>", ""))

        aux3140 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Cuentas por Pagar</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3140, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Cuentasxpagar/frmIngresos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ingresos Cuentas por Pagar'> Ingresos Cuentas por Pagar</div>", ""))

        NodoAux = insDoc(aux3140, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Cuentasxpagar/frmExtornoCxP.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Anulación Cuentas por Pagar'> Anulación Cuentas por Pagar</div>", ""))

        NodoAux = insDoc(aux3140, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Cuentasxpagar/frmLiquidaciones.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Liquidación Cuentas por Pagar'> Liquidación Cuentas por Pagar</div>", ""))

        aux3141 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Cartas de Instrucción</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3141, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmGeneracionCartasEmitidas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Generación'> Generación</div>", ""))

        NodoAux = insDoc(aux3141, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmGeneracionCartasFirmadas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Impresión'> Impresión</div>", ""))

        NodoAux = insDoc(aux3141, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmAutorizacionCartas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Autorización'> Autorización</div>", ""))

        NodoAux = insDoc(aux3141, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmEnvioCartas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Envío'> Envío</div>", ""))
        //CMB OT 63063 20110525 REQ 15
        NodoAux = insDoc(aux3141, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/OperacionesCaja/frmInventarioCartas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Stock'> Stock</div>", ""))

        aux3142 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Flujos de Caja estimada</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3142, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/FlujoCajaEstimado/frmFlujoCajaExterior.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Flujo de Caja exterior'> Flujo de Caja exterior</div>", ""))

        NodoAux = insDoc(aux3142, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/FlujoCajaEstimado/frmFlujoCajaLocal.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Flujo de Caja local'> Flujo de Caja local</div>", ""))

        NodoAux = insDoc(aux3142, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/FlujoCajaEstimado/frmMovimientoNegociacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Movimientos de Negociación'> Movimientos de Negociación</div>", ""))

        NodoAux = insDoc(aux3142, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/FlujoCajaEstimado/frmIngresosFlujoCaja.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ingreso de Flujo'> Ingreso de Flujo</div>", ""))

        //aux3143 = insFld(aux3121, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Interfaz de Abono</span>", "javascript:parent.op()"))
        //NodoAux = insDoc(aux3143, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/InterfazAbonos/frmAprobacionAbonos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Aprobación de Abonos'> Aprobación de Abonos</div>", ""))

        //NodoAux = insDoc(aux3143, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/InterfazAbonos/frmEnvioAbonos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Envío de abonos'> Envío de abonos</div>", ""))


        NodoAux = insDoc(aux3121, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Reportes/frmGenerarReportes.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reportes de Tesorería'> Reportes de Tesorería</div>", ""))

        aux3167 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Valorización y Custodia</span>", "javascript:parent.op()"))




        aux3168 = insFld(aux3167, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Precios y Tipos de Cambio</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3168, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmMantenimientoPrecios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Mantenimiento de Precios'> Mantenimiento de Precios</div>", ""))

        NodoAux = insDoc(aux3168, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmConsultaTipoCambio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Mantenimiento de Tipo de Cambio'> Mantenimiento de Tipo de Cambio</div>", ""))

        aux3169 = insFld(aux3167, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Valorización</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3169, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmValorizacionSBS.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Procesos de Valorizacion'> Procesos de Valorizacion</div>", ""))

        NodoAux = insDoc(aux3169, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmExtornoDeValorizacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reversión de Valorización'> Reversión de Valorización</div>", ""))

        NodoAux = insDoc(aux3169, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmConsultaValoracion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Valorización'> Consulta de Valorización</div>", ""))

        NodoAux = insDoc(aux3169, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmCalculoPromedioDiario.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Precio Promedio por instrumento'> Precio Promedio por instrumento</div>", ""))

        NodoAux = insDoc(aux3169, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmConsultaskardex.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Kardex de la Cartera'> Kardex de la Cartera</div>", ""))

        aux3170 = insFld(aux3167, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Custodia</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmInterfasesInfCustodio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Carga inf. de Custodia'> Carga inf. de Custodia</div>", ""))

        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmCargarConciliacionCustodios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Conciliación inf. Custodios'> Conciliación inf. Custodios</div>", ""))

        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmConsultaTitulosCustodios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Títulos asociados a Custodios'> Títulos asociados a Custodios</div>", ""))

        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmIngresoCustodios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Registro de Faltantes'> Registro de Faltantes</div>", ""))

        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmEgresoCustodios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Registro de Excedentes'> Registro de Excedentes</div>", ""))

        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmTransferenciaCustodios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Transferencias entre Custodios'> Transferencias entre Custodios</div>", ""))
        //HDG INC 64460	20120102
        NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmVerificaSaldosCustodio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Saldos Custodio por Mnemonico'> Saldos Custodio por Mnemonico</div>", ""))
        //HDG OT 64765 20120312
        //NodoAux = insDoc(aux3170, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmReporteServiciosBBH.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Servicios BBH'> Reporte de Servicios BBH</div>", ""))

        aux3171 = insFld(aux3167, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Dividendos, Rebates y Liberadas</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3171, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmInterfasesInfoDivRebLib.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Recuperación de Archivos'> Recuperación de Archivos</div>", ""))

        NodoAux = insDoc(aux3171, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/DividendosRebatesyLiberadas/frmConsultaDivRebatesLib.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de dividendos, rebates y liberadas'> Consulta de dividendos, rebates y liberadas</div>", ""))

        NodoAux = insDoc(aux3171, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Custodia/frmRegistroDivRebLib.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Registro de dividendos, rebates y liberadas'> Registro de dividendos, rebates y liberadas</div>", ""))
        //PLD OT 65289 20120522
        NodoAux = insDoc(aux3171, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/DividendosRebatesyLiberadas/frmCalculoRebates.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Cálculo Rebates'> Reporte Cálculo Rebates</div>", ""))

        aux3172 = insFld(aux3167, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Encaje</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmParametriaEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Parametría de Encaje'> Parametría de Encaje</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmCapturaTasasEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tasas de Encaje'> Tasas de Encaje</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmGeneracionEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cálculo de Encaje'> Cálculo de Encaje</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmExtornoEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reversión de Encaje'> Reversión de Encaje</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmConsultaEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Encaje'> Consulta de Encaje</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmResultadoEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Resultados de Encaje'> Resultados de Encaje</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmRentabilidadFondoEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Rentabilidad'> Reporte de Rentabilidad</div>", ""))

        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmRentabilidadEncaje.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Rentabilidad de Encaje'> Reporte de Rentabilidad de Encaje</div>", ""))
        //HDG OT 65195 20120515
        NodoAux = insDoc(aux3172, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Tesoreria/Encaje/frmRepInteresDividendos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Intereses y Dividendos'> Reporte de Intereses y Dividendos</div>", ""))


        aux3063 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Parametría</span>", "javascript:parent.op()"))
        aux3064 = insFld(aux3063, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Tablas</span>", "javascript:parent.op()"))



        aux3068 = insFld(aux3064, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Tablas de Entidades</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaCustodios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Custodios'> Custodios</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaGruposEconomicos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Grupos Economicos'> Grupos Economicos</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaTerceros.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Terceros'> Terceros</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaContacto.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Contacto'> Contacto</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaIntermediarioContacto.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Intermediario Contacto'> Intermediario Contacto</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Tesoreria/frmBusquedaTipoOperaciones.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Operación'> Tipo de Operación</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Tesoreria/frmBusquedaOperacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operación'> Operación</div>", ""))


        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaBroker.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Broker'> Broker/Comisiones</div>", ""))

        NodoAux = insDoc(aux3068, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Entidades/frmBusquedaEntidadExcesos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Broker'> Broker/Excesos</div>", ""))


        aux3072 = insFld(aux3064, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Tablas Generales</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaClaseCuentas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Clase de Cuenta'> Clase de Cuenta</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Tesoreria/frmBusquedaModalidadesPago.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Modalidad de Pago'> Modalidad de Pago</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Contabilidad/frmBusquedaCentroCostos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Centro de Costos'> Centro de Costos</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaCodigosPostales.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Códigos Postales'> Códigos Postales</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaCuentasEconomicas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cuentas Economicas'> Cuentas Economicas</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaFeriados.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Feriados'> Feriados</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Contabilidad/frmBusquedaMatriz.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Matriz'> Matriz</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaMercados.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Mercados'> Mercados</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaModeloCartas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Modelo de Carta'> Modelo de Carta</div>", ""))
        //CMB 20110419 Nro 27
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaAprobadorCarta.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Modelo de Carta'> Aprobador Carta</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaMonedas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Moneda'> Moneda</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaNegocios.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Negocio'> Negocio</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaPaises.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='País'> País</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaSectoresEmpresariales.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Sector Empresarial'> Sector Empresarial</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaTipoDocumentos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Documento'> Tipo de Documento</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaTipoRenta.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Renta'> Tipo de Renta</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaTipoCambio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Cambio'> Tipo de Cambio</div>", ""))

        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaRating.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Rating'> Rating</div>", ""))
        //CMB OT 63063 20110523 REQ 15
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaMotivoExtorno.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Motivo de Extorno'> Motivo de Extorno</div>", ""))
        //HDG OT 64291 20111128
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaMedioTransmision.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Medio de Transmisión'> Medio de Transmisión</div>", ""))
        //HDG OT 64291 20111202
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaGrupoLimiteTrader.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Grupo Límite Trading'> Grupo Límite Trading</div>", ""))
        //HDG OT 64480 20120120
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaRolAprobadoresTrader.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Autorizados Aprobaciones Trader'> Autorizados Aprobaciones Trader</div>", ""))
        //HDG OT 64765 20120312
        //NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaLocacionBBH.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Locaciones BBH'> Locaciones BBH</div>", ""))
        //HDG OT 64926 20120321
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaLimiteIntermediario.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Límites de Intermediario Negociación'> Límites de Intermediario Negociación</div>", ""))
        //CMB OT 65473 20120618
        NodoAux = insDoc(aux3072, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaAprobadorReporte.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Aprobador Documentos'> Aprobador Documentos</div>", ""))

        aux3085 = insFld(aux3064, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Tablas de Limites</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaLimites.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Límite'> Límite</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaGrupoInstrumento.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Universo de Limites'> Universo de Limites</div>", ""))

        //NodoAux=insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaFactor.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Factores'> Factores</div>", ""))
        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaFactor.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Factores'> Factores por Emisor</div>", ""))

        //CMB - 20101027 SE AGREGO OPCION DE FACTORES POR EMISION
        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaFactorEmision.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='FactoresEmision'> Factores por Emisi&oacute;n</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaBalanceContable.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Balance Contable'> Balance Contable</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaPatrimonioFideicomiso.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Patrimonio Fideicomiso'> Patrimonio Fideicomiso</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaPatrimonioValor.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Patrimonio Valor fondo'> Patrimonio Valor fondo</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaLiquidezAccion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Acciones segun Liquidez'> Acciones segun Liquidez</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaMontoNegociadoBVL.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Montos Negociados BVL'> Montos Negociados BVL</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaGrupoTipoInstrumento.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Grupo por Tipo de Instrumento'> Grupo por Tipo de Instrumento</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaGrupoTipoRenta.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Grupo por Tipo de Renta'> Grupo por Tipo de Renta</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Limites/frmBusquedaPatrimonio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Incremento o decremento'> Incremento o decremento</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmUsuariosNotifica.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Usuarios Notificados'> Usuarios Notificados</div>", ""))

        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Limites/frmBusquedaNivelesCobertura.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Niveles de Cobertura'> Niveles de Cobertura</div>", ""))
        //CMB OT 65023 20120608
        NodoAux = insDoc(aux3085, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Generales/frmBusquedaPorcentajeRating.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Porcentajes por Rating'> Porcentajes por Rating</div>", ""))

        aux3088 = insFld(aux3064, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Tablas de Valores</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaCalificacionInstrumentos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Calificación de Instrumento'> Calificación de Instrumento</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaImpuestosComisiones.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Impuestos y Comisiones'> Impuestos y Comisiones</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaPeriocidad.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Periodicidad'> Periodicidad</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaPeriodosLibor.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Periodos Libor'> Periodos Libor</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaTipoBursatilidad.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Bursatilidad'> Tipo de Bursatilidad</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaTiposAmortizacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Amortización'> Tipo de Amortización</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaTiposCupon.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Cupón'> Tipo de Cupón</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaTiposInstrumentos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Instrumento'> Tipo de Instrumento</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaTiposTitulos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Tipo de Título'> Tipo de Título</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaTiposValorizacion.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='BCR Seriados y Unicos'> BCR Seriados y Unicos</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaCuentasporTipoInst.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cuentas por Tipo de Instrumento'> Cuentas por Tipo de Instrumento</div>", ""))

        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmLiborFecha.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Fecha Libor'> Fecha Libor</div>", ""))
        //HDG OT 64769-4 20120404
        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedaHechosImportancia.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Notificaciones de Importancia'> Notificaciones de Importancia</div>", ""))
        //PLD OT 65289 20120522
        NodoAux = insDoc(aux3088, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Tablas/Valores/frmBusquedasParamCalculoRebates.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cálculo Rebates'> Cálculo Rebates</div>", ""))


        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/AdministracionValores/frmBusquedaValores.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Administración de Valores'> Administración de Valores</div>", ""))

        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/AdministracionPortafolios/frmBusquedaPortafolio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Administración de Portafolios'> Administración de Portafolios</div>", ""))

        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/frmAperturaPortafolio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Apertura Negociacion'> Apertura Negociacion</div>", ""))

        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/frmCierrePortafolio.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cierre de IDI'> Cierre de IDI</div>", ""))

        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/CotizacionVAC/frmBusquedaCotizacionVAC.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Indicadores'> Indicadores</div>", ""))

        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/Reportes/frmGenerarReportes.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reportes'> Reportes</div>", ""))

        NodoAux = insDoc(aux3063, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Parametria/frmGeneraVencimientosFuturos.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Generacion de Vencimientos'> Generacion de Vencimientos</div>", ""))

        aux3194 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Gestión</span>", "javascript:parent.op()"))

        aux3195 = insFld(aux3194, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Archivos Planos</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfasesSBSNet.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar SBS'> Importar SBS</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/ValorizacionCustodia/Valorizacion/frmInterfaseBloomberg.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Bloomberg'> Importar Bloomberg</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfasesVAX.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar VAX'> Importar VAX </div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesComposicionCartera.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reportes IDI'> Reportes IDI</div>", ""))

        //NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos%20Planos/frmInterfaseIndicadores.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Curva Cupon Cero'> Importar Curva Cupon Cero</div>", ""))

        //NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos%20Planos/frmImportarELEXyValidarSBS.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar ELEX y validar con SBS'> Importar ELEX y validar con SBS</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfases.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Generar para VAX'> Generar para VAX</div>", ""))

        //NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfaseLBTR.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Generar LBTR'> Generar LBTR</div>", ""))

        //NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfaseCargarPreordenes.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Pre - Ordenes'>Importar Pre - Ordenes</div>", ""))

        //NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmImportarAFPGenerarGAPS.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar estimaciones AFPs y generar GAPS'>Importar estimaciones AFPs y generar GAPS</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfaseBenchmark.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Benchmark Indicadores'>Importar Benchmark Indicadores</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfaseLineasCredito.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Líneas de Crédito por Emisor'>Importar Líneas de Crédito por Emisor</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfaseLineaxContraparteFWD.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar para Líneas por Contraparte para Forwards'>Importar para Líneas por Contraparte para Forwards</div>", ""))
        //CMB OT 63063 20110601 REQ 06
        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfaseOperacionesVentaEPU.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Operaciones de Venta - Consitución EPU US'>Importar Operaciones de Venta - Consitución EPU US</div>", ""))
        //HDG OT 64765 20120312
        //NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Archivos Planos/frmInterfasesBBH.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Transaciones BBH'> Importar Transaciones BBH</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this, \"Modulos/Gestion/Archivos Planos/frmInterfaseLineasContraparte.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Líneas Contraparte'> Importar Líneas Contraparte</div>", ""))

        NodoAux = insDoc(aux3195, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this, \"Modulos/Gestion/Archivos Planos/frmInterfaseLineasSettlement.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Importar Líneas Settlement'> Importar Líneas Settlement</div>", ""))

        aux3228 = insFld(aux3194, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Reportes Gestión</span>", "javascript:parent.op()"))


        aux3214 = insFld(aux3228, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Reportes de Cartera</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Emisor'> Composicion Cartera Emisor</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCS&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Sector'> Composicion Cartera Sector</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCP1&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Plazo - Resumen'> Composicion Cartera Plazo - Resumen</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCPD&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Plazo - Detalle'> Composicion Cartera Plazo - Detalle</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCTR&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Tipo Renta'> Composicion Cartera Tipo Renta</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCCI&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Categoria Instrumento'> Composicion Cartera Categoria Instrumento</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=DCD&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Duracion Cartera Detalle'> Duracion Cartera Detalle</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=DC&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Duracion Cartera Resumen'> Duracion Cartera Resumen</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCR&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Riesgo - Detalle'> Composicion Cartera Riesgo - Detalle</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCRR&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Riesgo - Resumen'> Composicion Cartera Riesgo - Resumen</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCEX&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion Cartera Exterior'> Composicion Cartera Exterior</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCM&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion de Cartera por Moneda'> Composicion de Cartera por Moneda</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=RDU&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Utilidad'> Reporte de Utilidad</div>", ""))

        NodoAux = insDoc(aux3214, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=CCIE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion de Cartera por Instrumento-Empresa'> Composicion de Cartera por Instrumento-Empresa</div>", ""))

        aux3234 = insFld(aux3228, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Otros Reportes</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=RDSDF&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reportes Stock Forward'> Reportes Stock Forward</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=RDSDCDD&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Stock Certificados Depositos'> Reporte Stock Certificados Depositos</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=REPCOMPB&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Composición BenchMark SBS'> Reporte Composición BenchMark SBS</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmConsultaDuraciones.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Consulta de Duraciones'> Consulta de Duraciones</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmSeguimientoForwards.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Seguimiento de Forwards'> Seguimiento de Forwards</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=SAFP&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Lista Precios SAFP'> Lista Precios SAFP</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=SAFPM&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Lista de Tipo de Cambio SAFP'> Lista de Tipo de Cambio SAFP</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=RFC&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Flujo de Caja'> Reporte de Flujo de Caja</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=SIPE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Auxiliar BCR'> Reporte Auxiliar BCR</div>", ""))

        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=GESHORA&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Gestor-HORA'> Reporte Gestor-HORA</div>", ""))
        //CMB 20110420 Nro 27
        NodoAux = insDoc(aux3234, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesGestion.aspx?rpt=OCEXT&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Operaciones de Caja Extornadas'> Reporte de Operaciones de Caja Extornadas</div>", ""))

        aux3442 = insFld(aux3228, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Reportes Excel</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=MON&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Posicion Cartera por Moneda'> Posicion Cartera por Moneda</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=PRE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Vector de Precios'> Vector de Precios</div>", ""))

        //FQS 20110420 Nro 17
        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=VECVAR&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Vector Variación'> Vector Variación</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=UNI&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Unidades Negociadas'> Unidades Negociadas</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=UNIVAL&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Unidades Valoradas'> Unidades Valoradas</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=CAR&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Composicion de Cartera'> Composicion de Cartera</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=FON3&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cartera por VPN Local'> Cartera por VPN Local</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=OPE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Ordenes de Inversion'> Ordenes de Inversion</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=ENG&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Engrapado'> Engrapado</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=ACCCV&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Compra Venta'> Compra Venta</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=UNIXFECHA&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Unidades por Fecha'> Reporte Unidades por Fecha</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=VECSER&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Vector Serie'> Vector Serie</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=AUXMEN&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Auxiliar Mensual de Inversiones'> Auxiliar Mensual de Inversiones</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=DIVREBLIB&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Dividendos - Rebates -Liberadas Decretadas'> Dividendos - Rebates -Liberadas Decretadas</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=REPEXT&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Exterior'> Reporte Exterior</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=VEN&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='RV/NFD'> RV/NFD</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReporteComisionesAgente.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Comision de agente'>Liquidación de comisión Agente de Bolsa</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportesCalculoInteres.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Calculo de Intereses'> Calculo de Intereses</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReportePromedioTasas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Promedio de tasas'> Promedio de tasas</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=LIBCONVER&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Conversion Acciones'>Conversion Acciones</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReporteGipsa.aspx?rpt=LIBCONVER&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Gipsa'>Gipsa</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReporteControlForward.aspx?rpt=LIBCONVER&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Forward'>Control de Forward</div>", ""))

        //NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmGenerarAccess.aspx?rpt=LIBCONVER&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Forward'>Exportar Access</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=REPOPE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de Operaciones'>Reporte de Operaciones</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=LCRE&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Líneas de Crédito por Emisor'> Líneas de Crédito por Emisor</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReporteLimitesInstrumento.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de control de límites por instrumento'> Reporte de control de límites por instrumento</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReporteLimitesporTipo.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de control por parametría de límites'> Reporte de control por parametría de límites</div>", ""))
        //CMB OT 64292 20111123
        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmReporteFallasDeNegociacionOI.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte de fallas de ordenes de inversión'> Reporte de fallas de ordenes de inversión</div>", ""))
        //HDG OT 64926 20120321
        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmOperacionesNegociadas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reporte Operaciones Negociadas por Trader'> Reporte Operaciones Negociadas por Trader</div>", ""))
        //HDG OT 65018 20120418
        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmDetallePosicionBancoxTipoInst.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Detalle de Posición por Emisor'> Detalle de Posición por Emisor</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=LCNT&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Líneas por Contraparte'> Líneas por Contraparte</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=LSTL&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Líneas Settlement'> Líneas Settlement</div>", ""))
        //HDG OT 67627 20130610
        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/frmReportesEspeciales.aspx?rpt=TRZOI&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Trazabilidad de Operaciones'> Trazabilidad de Operaciones</div>", ""))

        NodoAux = insDoc(aux3442, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Gestion/Reportes/frmCodigoValor.aspx?rpt=TRZOI&Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Codigo Valor'> Codigo Valor</div>", ""))


        aux3201 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Interfase Contable SIT</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmPlanDeCuentas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Plan de Cuentas'> Plan de Cuentas</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmMatrizContable.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Matriz Contable'> Matriz Contable</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmGeneracionDeAsientosContables.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Generar Asientos Contables'> Generar Asientos Contables</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmRevisionAsientosContables.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Asientos Contables'> Asientos Contables</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmOperacionesNoContabilizadas.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Operaciones no contabilizadas'> Operaciones no contabilizadas</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmCierreYPreCierreContable.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Cierre Contable'> Cierre Contable</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmGeneracionRistraContable.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Generacion de Ristra Contable'> Generacion de Ristra Contable</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/Reportes/frmReportesContables.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reportes Contables'> Reportes Contables</div>", ""))

        NodoAux = insDoc(aux3201, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Contabilidad/frmReversionRistraContable.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Reversión Contables'> Reversión Contable</div>", ""))

        aux3257 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Partición de la Cartera</span>", "javascript:parent.op()"))

        NodoAux = insDoc(aux3257, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/Inversiones/ParticionCartera.aspx?Variables=Rol1=ADMINUSU,Parametro1=/ \")'   title='Partición de la Cartera'> Partición de la Cartera</div>", ""))

        aux1808 = insFld(aux0, gFld("<span class='txt_BoldArbolPadre' onclick='javascript:GetInfo(this)'>Provisión de Pagos</span>", "javascript:parent.op()"))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmPagos.aspx\")'   title='Registro Pago'>Registro Pago</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmRegPagos.aspx\")'   title='Aprobar Pago'>Aprobar Pago</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmConsultaPagos.aspx\")'   title='Consulta Pago'>Consulta Pago</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmBancoCuenta.aspx\")'   title='Parametria Cuentas Corrientes'>Parametria Cuentas Corrientes</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmUsuarios.aspx\")'   title='Parametria Usuario'>Parametria Usuario</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmParametriaGeneral.aspx\")'   title='Parametros Generales'>Parametros Generales</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmCierreHora.aspx\")'   title='Parametria Cierre'>Parametria Cierre</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmReportes.aspx\")'   title='Reportes'>Reportes</div>", ""))
        NodoAux = insDoc(aux1808, gLnk("R", "<div class='txt_BoldArbolHijo' onclick='javascript:Link(this,\"Modulos/PrevisionPagos/frmGenerarMemo.aspx\")'   title='Generar Memo'>Generar Memo</div>", ""))
        
    </script>
</head>
<body topmargin="0" marginheight="5">
    <form runat="server">
    <!------------------------------------------------------------->
    <!-- IMPORTANT NOTICE:                                       -->
    <!-- Removing the following link will prevent this script    -->
    <!-- from working.  Unless you purchase the registered       -->
    <!-- version of TreeView, you must include this link.        -->
    <!-- If you make any unauthorized changes to the following   -->
    <!-- code, you will violate the user agreement.  If you want -->
    <!-- to remove the link, see the online FAQ for instructions -->
    <!-- on how to obtain a version without the link.            -->
    <!------------------------------------------------------------->


    <%--Temporal para obtener rapidamente los datos(Nombre y Link) de cada opcion--%>
    <div id="infoOpcion" class="temp-div">
        <input id="InfoOp_txtInfo" type="text" />
        <input id="InfoOp_btnClose" type="button" value="Cerrar" onclick="InfoOp_btnClose_onclick()" />
    </div>



    <table height="0" cellspacing="0" cellpadding="0" width="100%" border="0" bgcolor="#f5f5f5">
        <tr valign="top">
            <td>
                <div id="divCabecera" style="left: 0px; position: absolute; top: 0px">
                    <table border="0">
                        <tr>
                            <td>
                                <font size="-2"><a style="font-size: 7pt; color: #609ad8; text-decoration: none"
                                    href="http://www.treemenu.net/" target="_blank"></a></font>
                            </td>
                        </tr>
                    </table>
                </div>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>
                            <table id="taFunciones" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td>
                                        <img style="cursor: hand" onclick="javascript:collapseTree();" height="16" src="App_Themes/js/menu/FOLDER.ICO"
                                            width="16">
                                    </td>
                                    <td>
                                        &nbsp;<img style="cursor: hand" onclick="javascript:expandTree(foldersTree);" height="16"
                                            src="App_Themes/js/menu/GRAPH14.ICO" width="16">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- Build the browser's objects and display default view  -->
                <!-- of the tree.-->
                <table id="taMenu" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr valign="top">
                        <td>
                            <script>                                initializeDocument()</script>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
