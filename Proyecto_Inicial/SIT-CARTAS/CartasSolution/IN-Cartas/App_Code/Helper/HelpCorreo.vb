Imports Microsoft.VisualBasic
Imports System.Data
Public Class HelpCorreo
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se adecua el mensaje para mejor entendimiento
    Public Shared Function MensajeNotificacionClave(ByVal Usuario As String, FechaOperacion As String, ByVal clave As String) As String
        Dim mensaje As New StringBuilder
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='650' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>Se ha generado aprobaciones de cartas:</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td colspan='3'>Usuario: " & Usuario & "</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td colspan='3'>Fecha: " & FechaOperacion & "</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td colspan='3'>Realice la firma de las operaciones con la siguiente clave generada: " & clave & "</td></tr>")
            .Append("<tr><td colspan='3' height='8'></td></tr>")
            .Append("<tr><td colspan='3'><strong>Fondos SURA</strong></td></tr>")
            .Append("<tr><td colspan='3'><strong>Grupo SURA</strong></td></tr></table>")
        End With
        Return mensaje.ToString
    End Function
End Class