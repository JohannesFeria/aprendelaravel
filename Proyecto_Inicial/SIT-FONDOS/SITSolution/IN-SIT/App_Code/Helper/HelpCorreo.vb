Imports Microsoft.VisualBasic
Imports System.Data

Public Class HelpCorreo
    Public Shared Function MensajeNotificacionClave(ByVal rutaArchivo As String, ByVal rutaCartas As String, ByVal clave As String) As String
        Dim mensaje As New StringBuilder
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='650' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>Se ha realizado la aprobación de las siguientes operaciones :</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td colspan='3'>Reporte de aprobación de cartas (debe descargar el documento antes del proceso de firma de operaciones) :</td></tr>")
            .Append("<tr><td><a href='" & rutaArchivo & "'>" & rutaArchivo & "</a></td></tr>")
            .Append("	<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td colspan='3'>Cartas de instrucción :</td></tr>")
            .Append("<tr><td><a href='" & rutaCartas & "'>" & rutaCartas & "</a></td></tr>")
            .Append("	<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td colspan='3'>Realice la firma de las operaciones con la siguiente clave generada: " & clave & "</td></tr>")
            .Append("<tr><td colspan='3' height='8'></td></tr>")
            .Append("<tr><td colspan='3'><strong>AFP Integra</strong></td></tr>")
            .Append("<tr><td colspan='3'><strong>Grupo SURA</strong></td></tr></table>")
        End With
        Return mensaje.ToString
    End Function

    Public Shared Function MensajeOperacionFutura(ByVal fecha As String, ByVal dt As DataTable, ByVal file As String) As String
        Dim mensaje As New StringBuilder
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='550' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='14'>El saldo emitido el " & fecha & ", c&aacute;lculo cuenta margen futuros:</td></tr>")
            .Append("</table><br/>")
            .Append("<table cellspacing='1' cellpadding='1' border='1' width='650' bordercolor='#000000' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td height='5' align=""center"">Tipo Operaci&oacute;n</td><td height='5' align=""center"">N&uacute;mero de Orden</td><td height='5' align=""center"">Mnem&oacute;nico</td><td height='5' align=""center"">Fecha Operaci&oacute;n</td>")
            .Append("<td height='5' align=""center"">Fondo</td><td height='5' align=""center"">Moneda</td><td height='5' align=""center"">Intermediario</td><td height='5' align=""center"">Precio</td><td height='5' align=""center"">Variaci&oacute;n</td>")
            .Append("<td height='5' align=""center"">Cuenta Margen</td><td height='5' align=""center"">Pago a Integra</td><td height='5' align=""center"">Pago a Cuenta Margen</td><td height='5' align=""center"">Saldo Cuenta Final</td><td height='5' align=""center"">Tipo Resultado</td></tr>")
            For Each fila As DataRow In dt.Rows
                .Append("<tr><td height='5' align=""center"">" & fila("TipoOperacion") & "</td>")
                .Append("<td align=""center"">" & fila("CodOrden") & "</td>")
                .Append("<td align=""center"">" & fila("Mnemonico") & "</td>")
                .Append("<td align=""center"">" & UIUtility.ConvertirFechaaString(fila("FechaOperacion")) & "</td>")
                .Append("<td align=""center"">" & fila("CodPortafolio") & "</td>")
                .Append("<td align=""center"">" & fila("Moneda") & "</td>")
                .Append("<td align=""center"">" & fila("Intermediario") & "</td>")
                .Append("<td align=""center"">" & fila("Precio") & "</td>")
                .Append("<td align=""center"">" & fila("Variacion") & "</td>")
                .Append("<td align=""center"">" & fila("CuentaMargen") & "</td>")
                .Append("<td align=""center"">" & fila("PagoHorizonte") & "</td>")
                .Append("<td align=""center"">" & fila("PagoCuentaMargen") & "</td>")
                .Append("<td align=""center"">" & fila("SaldoCuentaFinal") & "</td>")
                .Append("<td align=""center"">" & fila("TipoResultado") & "</td></tr>")
            Next
            .Append("</table><br/><br/>")
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='650' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='14'><b>" & file & "</b></td></tr>")
            .Append("<tr><td colspan='14'>&nbsp;</td></tr>")
            .Append("<tr><td colspan='14'>&nbsp;</td></tr>")
            .Append("<tr><td colspan='14'><strong>AFP Integra</strong></td></tr>")
            .Append("<tr><td colspan='14'><strong>Grupo SURA</strong></td></tr></table>")
        End With

        Return mensaje.ToString
    End Function

End Class
