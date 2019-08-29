<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDiferenciaValorCuota.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmDiferenciaValorCuota" %>

<%--'
 ======= Origen : Creación                                     ======
'======= Fecha Creación: 04/10/2017                            ======
'======= Autor: Jorge Luis Benites Del Aguila - GMD            ======
'======= Orden de trabajo: OT10902                             ======
'======= Descripción: Pantalla de diferencia del valor cuota   ======
--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="container-fluid">
        <header>
            <h5 style="text-align: center;">
               Listado de Diferencias Encontradas en Valor Cuota&nbsp;<asp:Label ID="lblCantidadDif" runat="server">(0)</asp:Label>
            </h5>
        </header>
                <div class="grilla">
                    <asp:GridView ID="dgListaDif" runat="server" SkinID="Grid">
                        <Columns>
                            <asp:BoundField DataField="FondoId" HeaderText="Código Fondo"></asp:BoundField>
                            <asp:BoundField DataField="Fondo" HeaderText="Fondo"></asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha"></asp:BoundField>
                            <asp:BoundField DataField="CodigoValor" HeaderText="C&#243;digo Valor"></asp:BoundField>
                            <asp:BoundField DataField="Valorizacion" HeaderText="Valorizaci&#243;n"></asp:BoundField>
                            <asp:BoundField DataField="Validador" HeaderText="Validador"></asp:BoundField>
                            <asp:BoundField DataField="DIF" HeaderText="Diferencia"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>