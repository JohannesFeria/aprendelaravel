<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmError.aspx.vb" Inherits="frmError" %>

<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>tipo1</title>
		<META http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="styles/estilos1.css" type="text/css" rel="styleSheet">
			<script language="javascript">				
			

			</script>
	</HEAD>
	<body>
		<form id="frmError" runat="server">
			<P>&nbsp;</P>
			<P>
				<br>
				<br>
				&nbsp;</P>
			<table class="error" id="tblError" cellSpacing="0" cellPadding="10" width="50%" align="center"
				runat="server">
				<TR>
					<TD align="center" colSpan="2">
						<HR width="100%" SIZE="1">
					</TD>
				</TR>
				<tr>
					<th align="center">
                        <img src="App_Themes/img/error.gif" />
                    </th>
					<th width="91%">
						<div align="left">ADVERTENCIA</div>
					</th>
				</tr>
				<tr>
					<td align="center"></td>
					<td align="left" width="91%">
						<P>El sistema ha encontrado una situacion anormal.</P>
						<P>Intentelo mas tarde o comunique el detalle tecnico al area de&nbsp;Sistemas.</P>
					</td>
				</tr>
				<TR>
					<TD align="center" colSpan="2">
						<HR width="100%" SIZE="1">
						&nbsp;</TD>
				</TR>
			</table>
			<div id="si" style="DISPLAY: none">
				<br>
			</div>
			<br>
			<div id="mostrar">
			</div>
			<div id="ocultar">
				<table class="error" id="tblOcultar" cellSpacing="0" cellPadding="10" width="50%" align="center"
					runat="server">
					<tr>
						<td width="91%" align="right">
							<asp:Button id="Button1" runat="server" BackColor="#C0FFC0" Text="Ver detalle Tecnico" Width="145px"></asp:Button>
							&nbsp;&nbsp;
							<asp:HyperLink runat="server" ID="hlnkCancelarOcultar" ForeColor="black" BackColor="Khaki" Font-Size="11">Regresar</asp:HyperLink>
						</td>
					</tr>
				</table>
			</div>
			<div id="cancelar" style="DISPLAY: none">
				<table class="error" id="tblCancelar" cellSpacing="0" cellPadding="10" width="50%" align="center"
					runat="server">
					<tr>
						<td width="91%" align="right">
						</td>
					</tr>
				</table>
			</div>
			<div id="mensajeTecnico" style="DISPLAY: none">
			</div>
		</form>
	</body>
</HTML>

